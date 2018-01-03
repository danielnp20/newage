using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.DTO.Attributes;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsumoProyectos : DocumentForm
    {
        public ConsumoProyectos()
        {
           //InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private string monedaLocal;
        private string monedaExtranjera;

        //Variables formulario
        protected DTO_Convenios data = null;
        private List<DTO_prConvenio> _listConvenio = null;
        private Dictionary<string, DTO_glBienServicioClase> _codigoBSClase = new Dictionary<string, DTO_glBienServicioClase>();

        private string terceroID = string.Empty;
        //Indica si el header es valido
        protected bool validHeader;

        //Variables con los recursos de las Fks
        private string _fechaPlanillaRsx = string.Empty;
        private string _codigoProyectoRsx = string.Empty;
        private string _codigoCtoCostoRsx = string.Empty;
        private string _codigoProveedorRsx = string.Empty;
        private string _codigoBSRsx = string.Empty;
        private string _referenciaRsx = string.Empty;
        private string _serialRsx = string.Empty;
        private string _cantidadRsx = string.Empty;
        private string _numeroProcesoRsx = string.Empty;
        private string _codigoPrefijoIDRsx = string.Empty;
        private string _documentoNroRsx = string.Empty;
        private bool _copyData = false;
        private bool moduleProyectoActive = false;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.disableValidate = true;
          
            this.masterProyecto.Value = string.Empty;
            this.masterCtoCosto.Value = string.Empty;
            this.masterLocFisica.Value = string.Empty;
            this.txtNroConsumo.Text = string.Empty;
            this.dtFechaConsumo.DateTime = this.dtFecha.DateTime;
            this.txtTotal.EditValue = "0";

            this.EnableHeader(true);
            this.dtFechaConsumo.Focus();
            FormProvider.Master.itemSendtoAppr.Enabled = false;
            //FormProvider.Master.itemImport.Enabled = false;
            this.btnQueryDoc.Enabled = true;
            this.data = new DTO_Convenios();
            this.gcDocument.DataSource = this.data.FooterConsumo;
            this.gcDocument.RefreshDataSource();
            this.disableValidate = false;
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
            FormProvider.Master.itemSendtoAppr.Enabled = false;
            this.btnQueryDoc.Enabled = false;
        }

        #endregion

        #region Propiedades

        //Numero de una fila segun el indice
        protected int NumFila
        {
            get { return this.data.FooterConsumo.FindIndex(det => det.Index == this.indexFila); }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
            this._bc.InitMasterUC(this.masterCtoCosto, AppMasters.coCentroCosto, true, true, true, true);
            this._bc.InitMasterUC(this.masterLocFisica, AppMasters.glLocFisica, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefOrdenTrabajo, AppMasters.glPrefijo, false, true, true, true);
            this.txtTotal.Text = "0";         

            this.tlSeparatorPanel.RowStyles[0].Height = 31;
            this.tlSeparatorPanel.RowStyles[1].Height = 80;
            this.tlSeparatorPanel.RowStyles[2].Height = 40;
        }

        /// <summary>
        /// Calcula valor factura + iva
        /// </summary>
        private void CalcularTotal()
        {
            decimal valor = 0;
            foreach (var item in this.data.FooterConsumo)
            {
                valor += item.Valor.Value != null? item.Valor.Value.Value : 0;
            }
            this.txtTotal.EditValue = valor;
            this.data.Header.Valor.Value = valor;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="ctrl">glDocumentoControl a validar</param>
        /// <param name="suppl">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidDate">Fecha en periodo invalido</param>
        /// <param name="msgFkNotFound">FK inexistente</param>
        /// <param name="msgFkHierarchyFather">No es una hoja de la jerarquia</param>
        /// <param name="msgCero">Mensaje que no permite ceros en un campo</param>
        /// <param name="msgPositive">Solo permite valores positivos</param>
        private void ValidateDataImport(DTO_prConvenioConsumoDirecto dto, DTO_glBienServicioClase bsClase, DTO_inReferencia refTipo, DTO_TxResultDetail rd, string msgFkNotFound, string msgInvalidField)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            DTO_prConvenioConsumoDirecto dtoDet = (DTO_prConvenioConsumoDirecto)dto;
            #region Variables y diccionarios
            TipoCodigo tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString());

            Dictionary<string, string> pksPar1 = new Dictionary<string, string>();
            Dictionary<string, string> pksPar2 = new Dictionary<string, string>();

            #endregion
            #region Valida las FKs
            #region Proveedor
            colRsx = this._codigoProveedorRsx;
            rdF = _bc.ValidGridCell(colRsx, dtoDet.ProveedorID.Value, false, true, false, AppMasters.prProveedor);
            if (rdF != null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = string.Format(msgFkNotFound, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CodigoBSID
            colRsx = this._codigoBSRsx;
            rdF = _bc.ValidGridCell(colRsx, dtoDet.CodigoBSID.Value, false, true, false, AppMasters.prBienServicio);
            if (rdF != null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = string.Format(msgFkNotFound, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region inReferenciaID
            colRsx = this._referenciaRsx;
            rdF = _bc.ValidGridCell(colRsx, dtoDet.inReferenciaID.Value, false, true, false, AppMasters.inReferencia);
            if (rdF != null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = string.Format(msgFkNotFound, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region Prefijo
            colRsx = this._codigoPrefijoIDRsx;
            rdF = _bc.ValidGridCell(colRsx, dtoDet.PrefijoID.Value, false, true, false, AppMasters.glPrefijo);
            if (rdF != null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = string.Format(msgFkNotFound, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #endregion
            #region Validacion de otros campos
            //colRsx = this._fechaPlanillaRsx;
            //rdF = this._bc.ValidGridCellValue(colRsx, string.Empty, false, false, true, false);
            //if (rdF != null)
            //{
            //    rdF.Message = string.Format(msgInvalidField, colRsx);
            //    rd.DetailsFields.Add(rdF);
            //}
            if (this.dtFechaConsumo.DateTime != dto.FechaPlanilla.Value)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = DictionaryMessages.Pr_DateInvalid;
                rd.DetailsFields.Add(rdF);
            }
            colRsx = this._cantidadRsx;
            rdF = this._bc.ValidGridCellValue(colRsx, dtoDet.Cantidad.Value.Value.ToString(), false, false, true, false);
            if (rdF != null)
            {
                rdF.Message = string.Format(msgInvalidField, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            colRsx = this._serialRsx;
            rdF = this._bc.ValidGridCellValue(colRsx, string.Empty, true, false, false, false);
            if (rdF != null)
            {
                rdF.Message = string.Format(msgInvalidField, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion
        }

        /// <summary>
        /// Obtiene los convenios o detalle de la orden de compra de cada item
        /// </summary>
        /// <param name="prefijoID"></param>
        /// <param name="DocumentoNro"></param>
        private int GetConveniosOrdenCompra(DTO_prConvenioConsumoDirecto dto)
        {
            int NumeroDocOc = 0;
            DTO_prConvenio  convenio = null;
            DTO_prOrdenCompra Orden = this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.OrdenCompra, dto.PrefijoID.Value, dto.DocumentoNro.Value.Value);
            if (Orden == null)
            {
                this.validHeader = false;
                return NumeroDocOc;
            }
            else
            {
                if (Orden.HeaderOrdenCompra.ContratoNro.Value != null && Orden.HeaderOrdenCompra.ContratoNro.Value != 0)
                {
                    NumeroDocOc = Orden.DocCtrl.NumeroDoc.Value.Value;
                    DTO_Convenios convenioExist = this._bc.AdministrationModel.Convenio_GetByNroContrato(this.documentID, this.prefijoID, Orden.HeaderOrdenCompra.ContratoNro.Value.Value);

                    //Carga Contrato relacionado a la Orden de Compra               
                    DTO_glDocumentoControl docContrato = this._bc.AdministrationModel.glDocumentoControl_GetByID(Orden.HeaderOrdenCompra.ContratoNro.Value.Value);
                    DTO_prOrdenCompra contrato = docContrato != null ? this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.Contrato, this.prefijoID.ToString(), docContrato.DocumentoNro.Value.Value) : null;

                    if (contrato != null && contrato.Convenio.Count > 0)
                    {
                        //TRAE EL DETALLE DE LOS CONVENIOS DE LA OC Y LLENA LOS VALORES CON ESE DETALLE
                        this._listConvenio = contrato.Convenio;
                        this.btnConvenios.Enabled = true;
                        if (this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && d.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)))
                        {
                            object res = this._listConvenio.Where(x => x.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && x.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)).First();
                            convenio = (DTO_prConvenio)res;
                            dto.ValorUni.Value = convenio.Valor.Value;
                            dto.Valor.Value = dto.ValorUni.Value * dto.Cantidad.Value;
                        }
                        else
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_BienServicioNotAvailable);
                            MessageBox.Show(string.Format(msg,dto.CodigoBSID.Value,dto.inReferenciaID.Value,dto.PrefijoID.Value,dto.DocumentoNro.Value));
                            dto.ValorUni.Value = 0;
                            dto.Valor.Value = 0;
                        }
                    }
                    else
                    {
                        //TRAE EL DETALLE DE LA ORDEN DE COMPRA Y LLENA LOS VALORES CON ESE DETALLE
                        if (Orden.Footer.Exists(d => d.DetalleDocu.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && d.DetalleDocu.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)))
                        {
                            object res = Orden.Footer.Where(d => d.DetalleDocu.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && d.DetalleDocu.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)).First();
                            DTO_prOrdenCompraFooter ordenFooter = (DTO_prOrdenCompraFooter)res;
                            dto.ValorUni.Value = ordenFooter.DetalleDocu.ValorUni.Value;
                            dto.Valor.Value = dto.ValorUni.Value * dto.Cantidad.Value;
                        }                       
                    }

                    if (convenioExist == null)
                    {
                        #region Llena datos para traer Convenios de Contrato
                        //this.masterMoneda.Value = Orden.HeaderOrdenCompra.MonedaOrden.Value;
                        //this.data.Header.Moneda.Value = this.masterMoneda.Value;
                        //this.data.Header.NumeroDocContrato.Value = Orden.HeaderOrdenCompra.ContratoNro.Value;
                        //this.data.Header.Valor.Value = 0;
                        //this.data.Header.IVA.Value = 0;
                        #endregion
                    }
                    else
                    {
                        #region Llena datos obteniendo el detalle de convenios existentes
                        //this.txtTotal.EditValue = convenioExist.Header.Valor.Value;
                        this.data.Header.NumeroDocContrato.Value = Orden.HeaderOrdenCompra.ContratoNro.Value;
                        this.data.Header.Valor.Value = convenioExist.Header.Valor.Value;
                        this.data.Header.IVA.Value = convenioExist.Header.IVA.Value;
                        //this.EnableHeader(false);
                        this.data = convenioExist;
                        this.validHeader = true;
                        if (convenioExist.FooterSolDespacho != null)
                        {
                            this.Invoke(this.refreshGridDelegate);
                            //this.LoadData(false);
                            //this.gcDocument.Focus();
                        }
                        #endregion
                    }
                }
            }
            return NumeroDocOc;
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected override void CleanFormat()
        {
            string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            string f = string.Empty;
            foreach (string col in cols)
            {
                if (col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaOtr") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd1") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd2") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd3") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd4") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd5") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd6") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd7") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd8") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd9") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd10") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambio"))
                {
                    if (col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrBaseME") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaExt"))
                    {
                        if (this.multiMoneda)
                            f += col + this.formatSeparator;
                    }
                    else
                        f += col + this.formatSeparator;
                }
            }
            this.format = f;
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            if (firstTime)
            {
                #region Load DocumentoControl
                this.data.DocCtrl.EmpresaID.Value = this.empresaID;
                this.data.DocCtrl.TerceroID.Value = this.terceroID; ////
                this.data.DocCtrl.NumeroDoc.Value = 0;
                this.data.DocCtrl.ComprobanteID.Value = string.Empty; ////this.comprobanteID;
                this.data.DocCtrl.ComprobanteIDNro.Value = 0;
                this.data.DocCtrl.MonedaID.Value = this.monedaLocal; ////
                this.data.DocCtrl.CuentaID.Value = string.Empty; ////
                this.data.DocCtrl.ProyectoID.Value = this.masterProyecto.Value; ////
                this.data.DocCtrl.CentroCostoID.Value = this.masterCtoCosto.Value; ////
                this.data.DocCtrl.LugarGeograficoID.Value = string.Empty; ////
                this.data.DocCtrl.LineaPresupuestoID.Value = string.Empty;////
                this.data.DocCtrl.Fecha.Value = DateTime.Now;
                this.data.DocCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this.data.DocCtrl.PrefijoID.Value = this.prefijoID;
                this.data.DocCtrl.TasaCambioCONT.Value = this._bc.AdministrationModel.TasaDeCambio_Get(monedaExtranjera, this.dtFecha.DateTime);////
                this.data.DocCtrl.TasaCambioDOCU.Value = this.data.DocCtrl.TasaCambioCONT.Value;////
                this.data.DocCtrl.DocumentoNro.Value = 0;
                this.data.DocCtrl.DocumentoID.Value = this.documentID;
                this.data.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;////
                this.data.DocCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this.data.DocCtrl.seUsuarioID.Value = this.userID;
                this.data.DocCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this.data.DocCtrl.ConsSaldo.Value = 0;
                this.data.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                this.data.DocCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this.data.DocCtrl.Descripcion.Value = this.txtDocDesc.Text;
                this.data.DocCtrl.Valor.Value = 0;
                this.data.DocCtrl.Iva.Value = 0;
                #endregion
            }

            this.gcDocument.DataSource = this.data.FooterConsumo;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.data.FooterConsumo.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();
            this.dataLoaded = true;
            if (this.data.FooterConsumo.Count > 0)
            {
                FormProvider.Master.itemSendtoAppr.Enabled = true;
                this.CalcularTotal();
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            this.documentID = AppDocuments.ConsumoProyecto;

            base.SetInitParameters();

            this.data = new DTO_Convenios();
            this.data.FooterConsumo = new List<DTO_prConvenioConsumoDirecto>();

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.pr;

            //Carga recursos importacion        
            this._fechaPlanillaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaPlanilla");
            this._codigoProyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            this._codigoCtoCostoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            this._codigoProveedorRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
            this._codigoBSRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            this._referenciaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            this._serialRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
            this._cantidadRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            this._codigoPrefijoIDRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoID");
            this._documentoNroRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoNro");
            this.terceroID = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga config de controles
            base.gvDocument.OptionsView.ColumnAutoWidth = true;
            this.InitControls();
            this.AddGridCols();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas Visibles

                //fechaPlanilla
                GridColumn fechaPlanilla = new GridColumn();
                fechaPlanilla.FieldName = this.unboundPrefix + "FechaPlanilla";
                fechaPlanilla.Caption = this._fechaPlanillaRsx;
                fechaPlanilla.UnboundType = UnboundColumnType.DateTime;
                fechaPlanilla.VisibleIndex = 0;
                fechaPlanilla.Width = 80;
                fechaPlanilla.Visible = true;
                fechaPlanilla.ColumnEdit = this.editDate;
                this.gvDocument.Columns.Add(fechaPlanilla);

                //ProveedorID
                GridColumn ProveedorID = new GridColumn();
                ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
                ProveedorID.Caption = this._codigoProveedorRsx;
                ProveedorID.UnboundType = UnboundColumnType.String;
                ProveedorID.VisibleIndex = 3;
                ProveedorID.Width = 80;
                ProveedorID.Visible = false;
                ProveedorID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(ProveedorID);

                //CodigoBSID
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = this._codigoBSRsx;
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 5;
                CodigoBSID.Width = 80;
                CodigoBSID.Visible = true;
                CodigoBSID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(CodigoBSID);

                //inReferenciaID
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = this._referenciaRsx;
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 6;
                inReferenciaID.Width = 80;
                inReferenciaID.Visible = true;
                inReferenciaID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(inReferenciaID);

                //SerialID
                GridColumn SerialID = new GridColumn();
                SerialID.FieldName = this.unboundPrefix + "SerialID";
                SerialID.Caption = this._serialRsx;
                SerialID.UnboundType = UnboundColumnType.String;
                SerialID.VisibleIndex = 7;
                SerialID.Width = 80;
                SerialID.Visible = true;
                this.gvDocument.Columns.Add(SerialID);

                //Cantidad
                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "Cantidad";
                Cantidad.Caption = this._cantidadRsx;
                Cantidad.UnboundType = UnboundColumnType.Decimal;
                Cantidad.VisibleIndex = 8;
                Cantidad.Width = 100;
                Cantidad.Visible = true;
                this.gvDocument.Columns.Add(Cantidad);

                //PrefijoID
                GridColumn PrefijoID = new GridColumn();
                PrefijoID.FieldName = this.unboundPrefix + "PrefijoID";
                PrefijoID.Caption = this._codigoPrefijoIDRsx;
                PrefijoID.UnboundType = UnboundColumnType.String;
                PrefijoID.VisibleIndex = 9;
                PrefijoID.Width = 100;
                PrefijoID.Visible = true;
                PrefijoID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(PrefijoID);

                //Cantidad
                GridColumn DocumentoNro = new GridColumn();
                DocumentoNro.FieldName = this.unboundPrefix + "DocumentoNro";
                DocumentoNro.Caption = this._documentoNroRsx;
                DocumentoNro.UnboundType = UnboundColumnType.Integer;
                DocumentoNro.VisibleIndex = 10;
                DocumentoNro.Width = 100;
                DocumentoNro.Visible = true;
                this.gvDocument.Columns.Add(DocumentoNro);

                //ValorUni
                GridColumn ValorUni = new GridColumn();
                ValorUni.FieldName = this.unboundPrefix + "ValorUni";
                ValorUni.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                ValorUni.UnboundType = UnboundColumnType.Decimal;
                ValorUni.VisibleIndex = 11;
                ValorUni.Width = 100;
                ValorUni.Visible = true;
                ValorUni.ColumnEdit = this.editSpin;
                ValorUni.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorUni);

                //Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 12;
                Valor.Width = 100;
                Valor.Visible = true;
                Valor.ColumnEdit = this.editSpin;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                #endregion
                #region Columnas no Visibles

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.Visible = false;
                this.gvDocument.Columns.Add(ProyectoID);

                //CentroCostoID
                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.Visible = false;
                this.gvDocument.Columns.Add(CentroCostoID);


                //NumeroProceso
                GridColumn NumeroProceso = new GridColumn();
                NumeroProceso.FieldName = this.unboundPrefix + "NumeroProceso";
                NumeroProceso.UnboundType = UnboundColumnType.Integer;
                NumeroProceso.Visible = false;
                this.gvDocument.Columns.Add(NumeroProceso);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "ConvenioSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            #region Import Format
            this.format += this._bc.GetImportExportFormat(typeof(DTO_prConvenioConsumoDirecto), this.documentID);
            #endregion
            this.dtFechaConsumo.DateTime = this.dtFecha.DateTime;
            var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false);
            bool controlSolicitudProyInd = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndControlSolicitudesBS).Equals("1") ? true : false;
            if (modules.Any(x => x.ModuloID.Value == ModulesPrefix.py.ToString()) && controlSolicitudProyInd)
            {
                this.masterPrefOrdenTrabajo.Visible = true;
                this.lblOrdenTrabajoNro.Visible = true;
                //this.lblPrefijoProy.Visible = true;
                this.txtOrdenTrabajoNro.Visible = true;
                this.btnProyecto.Visible = true;
                this.moduleProyectoActive = true;
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_prConvenioConsumoDirecto footerDet = new DTO_prConvenioConsumoDirecto();

            #region Asigna datos a la fila
            try
            {
                if (this.data.FooterConsumo.Count > 0)
                    footerDet.Index = this.data.FooterConsumo.Last().Index + 1;
                else
                    footerDet.Index = 0;
                footerDet.FechaPlanilla.Value = null;
                footerDet.ProyectoID.Value = string.Empty;
                footerDet.CentroCostoID.Value = string.Empty;
                footerDet.ProveedorID.Value = string.Empty;
                footerDet.CodigoBSID.Value = string.Empty;
                footerDet.inReferenciaID.Value = string.Empty;
                footerDet.SerialID.Value = string.Empty;
                footerDet.Cantidad.Value = 0;
                footerDet.PrefijoID.Value = string.Empty;
                footerDet.DocumentoNro.Value = null;
                footerDet.NumeroDocOC.Value = 0;
                footerDet.Valor.Value = 0;
                footerDet.ValorUni.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "AddNewRow"));
            }
            #endregion

            this.data.FooterConsumo.Add(footerDet);
            this.gvDocument.RefreshData();
            this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

            this.isValid = false;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected bool ValidateHeader()
        {
            #region Valida los datos obligatorios 

            #region Valida datos en la maestra de Proyecto
            if (!this.masterProyecto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyecto.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyecto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Centro Costo
            if (!this.masterCtoCosto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCtoCosto.CodeRsx);

                MessageBox.Show(msg);
                this.masterCtoCosto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Loc Fisica
            //if (!this.masterLocFisica.ValidID)
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLocFisica.CodeRsx);

            //    MessageBox.Show(msg);
            //    this.masterLocFisica.Focus();

            //    return false;
            //}
            #endregion

            #region Valida datos en el control de Nro ORden
            //if (string.IsNullOrEmpty(this.txtNroConsumo.Text))
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNroConsumo.Text);

            //    MessageBox.Show(msg);
            //    this.txtNroConsumo.Focus();

            //    return false;
            //}
            #endregion

            #region Valida que existan convenios del contrato
            //if (this._listConvenio == null || this._listConvenio.Count == 0)
            //{
            //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Lista de convenios vacia, no tiene valor adicional al contrato"));
            //    this.txtNroConsumo.Focus();

            //    return false;
            //}
            #endregion

            #region Valida datos del Modulo Proyectos
            if (this.moduleProyectoActive)
            {
                if (!this.masterPrefOrdenTrabajo.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefOrdenTrabajo.CodeRsx);
                    MessageBox.Show(msg);

                    this.masterPrefOrdenTrabajo.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtOrdenTrabajoNro.Text))
                {
                    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblNroProyecto");
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                    MessageBox.Show(msg);

                    this.txtOrdenTrabajoNro.Focus();
                    return false;
                }
            }
            #endregion

            #endregion
            return true;
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected void EnableHeader(bool enable)
        {
            this.masterProyecto.EnableControl(enable);
            this.masterCtoCosto.EnableControl(enable);
            this.masterLocFisica.EnableControl(enable);
            this.txtNroConsumo.Enabled = enable;
            this.btnQueryDoc.Enabled = enable;
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (this.data.FooterConsumo.Count > 0)
                {
                    GridColumn col = new GridColumn();
                    #region Validacion de nulls y Fks
                    #region Proveedor
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProveedorID", false, true, false, AppMasters.prProveedor);
                    if (!validField)
                    {
                        MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this._codigoProveedorRsx));
                        validRow = false;
                    }
                    #endregion
                    #region CodigoBS
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CodigoBSID", false, true, true, AppMasters.prBienServicio);

                    if (!validField)
                    {
                        MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this._codigoBSRsx));
                        validRow = false;
                    }

                    #endregion
                    #region Referencia
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "inReferenciaID", false, true, false, AppMasters.inReferencia);
                    if (!validField)
                    {
                        MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this._referenciaRsx));
                        validRow = false;
                    }
                    #endregion
                    #region SerialID
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "SerialID", true, false, false, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Cantidad
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Cantidad", false, false, true, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Prefijo
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "PrefijoID", false, true, false, AppMasters.glPrefijo);
                    if (!validField)
                    {
                        MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this._codigoPrefijoIDRsx));
                        validRow = false;
                    }
                    #endregion
                    #region DocumentoNro
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "DocumentoNro", false, false, true, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region NumeroProceso
                    //validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "NumeroProceso", false, false, true, false);
                    //if (!validField)
                    //    validRow = false;
                    #endregion
                    #region Valor
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Valor", false, false, true, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region ValorUni
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorUni", false, false, true, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #endregion
                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false; 
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "ValidateRow"));
            }

            this.hasChanges = true;
            return validRow;
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Proveedor
            if (colName == this._codigoProveedorRsx)
                return AppMasters.prProveedor;
            //CodigoBS
            if (colName == this._codigoBSRsx)
                return AppMasters.prBienServicio;
            //Referencia
            if (colName == this._referenciaRsx)
                return AppMasters.inReferencia;

            return 0;
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;           
            FormProvider.Master.itemGenerateTemplate.Visible = true;
            FormProvider.Master.itemSendtoAppr.Visible = true;
            FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al salir del control de numero de factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroConsumo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.txtNroConsumo.Text) && !this.txtNroConsumo.Text.Equals("0"))
                {
                    DTO_Convenios convenioConsumoExist = this._bc.AdministrationModel.Convenio_Get(this.documentID, this.prefijoID, Convert.ToInt32(this.txtNroConsumo.Text), true);
                    if (convenioConsumoExist == null)
                    {
                        MessageBox.Show(this.Text + " "+  this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FieldNotFound));
                        this.txtNroConsumo.Focus();
                        this.validHeader = false;
                        return;
                    }
                    if (this._copyData)
                    {
                        convenioConsumoExist.DocCtrl.NumeroDoc.Value = 0;
                        convenioConsumoExist.DocCtrl.DocumentoNro.Value = 0;
                        convenioConsumoExist.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        convenioConsumoExist.Header.NumeroDoc.Value = 0;
                        this._copyData = false;                       
                    }
                    else if (convenioConsumoExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NumberAlreadyIsAprob)); 
                        this.txtNroConsumo.Focus();
                        this.validHeader = false;
                        return;
                    }
                    else
                    {
                        #region Llena datos 
                        this.masterProyecto.Value = convenioConsumoExist.DocCtrl.ProyectoID.Value;
                        this.masterCtoCosto.Value = convenioConsumoExist.DocCtrl.CentroCostoID.Value;
                        this.txtNroConsumo.EditValue = convenioConsumoExist.DocCtrl.DocumentoNro.Value;
                        if (this.ValidateHeader())
                        {
                            this.data = convenioConsumoExist;
                            this.validHeader = true;
                            if (convenioConsumoExist.FooterConsumo != null)
                            {
                                this.LoadData(false);
                                this.gcDocument.Focus();
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "txtNroConsumo_Leave"));
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.ConsumoProyecto);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                this.txtNroConsumo.Enabled = true;
                this.prefijoID = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtNroConsumo.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.txtNroConsumo.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        /// <summary>
        /// Consulta de convenios
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnConvenios_Click(object sender, EventArgs e)
        {
            ModalConvenioProveedor convenios = new ModalConvenioProveedor(this._listConvenio, Convert.ToInt32(this.txtNumeroDoc.Text), true);
            convenios.ShowDialog();
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    UDT udtProp = (UDT)pi.GetValue(dto, null);
                    udtProp.SetValueFromString(e.Value.ToString());
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            bool validField = true;
            int index = this.NumFila;
            DTO_prConvenio convenio;
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                GridColumn col = this.gvDocument.Columns[e.Column.FieldName];
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region Valida el Tipo de Servicio

                if (fieldName == "CodigoBSID" || fieldName == "inReferenciaID")
                {
                    string value = this.data.FooterConsumo[index].CodigoBSID.Value;
                    if (!_codigoBSClase.Keys.Contains(value))
                    {
                        DTO_prBienServicio bs = (DTO_prBienServicio)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, value, true);
                        DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bs.ClaseBSID.Value, true);
                        _codigoBSClase.Add(value, bsClase);
                    }
                    if (_codigoBSClase != null && _codigoBSClase[value].TipoCodigo.Value != (byte)TipoCodigo.Inventario)
                    {
                        string referencia = this.data.FooterConsumo[index].inReferenciaID.Value;
                        string refxDefecto = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);

                        if (!string.IsNullOrEmpty(referencia) && referencia != refxDefecto)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Pr_ReferenciaInvalid));
                            validField = false;
                        }
                        else
                        {
                            this.data.FooterConsumo[index].inReferenciaID.Value = refxDefecto;
                        }
                    }
                }
                #endregion
                #region Se modifican FKs
                #region Proveedor
                if (fieldName == "ProveedorID")
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.prProveedor);
                #endregion
                #region CodigoBS
                if (fieldName == "CodigoBSID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true,true, AppMasters.prBienServicio);
                    if (this._listConvenio != null && this._listConvenio.Count > 0)
                        if (this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(e.Value.ToString()) && d.inReferenciaID.Value.Equals(this.data.FooterConsumo[index].inReferenciaID.Value)))
                        {
                            validField = true;
                            //object res = this._listConvenio.Where(x => x.CodigoBSID.Value.Equals(e.Value) && x.inReferenciaID.Value.Equals(this.data.FooterConsumo[index].inReferenciaID.Value)).First();
                            //convenio = (DTO_prConvenio)res;
                            //this.data.FooterConsumo[index].ValorUni.Value = convenio.Valor.Value;
                        }
                    //else
                    //    this.data.FooterConsumo[index].ValorUni.Value = 0;
                    this.gcDocument.RefreshDataSource();
                } 
                #endregion
                #region Referencia
                if (fieldName == "inReferenciaID")
                {
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.inReferencia);
                    if (this._listConvenio != null &&  this._listConvenio.Count > 0)
                        if (this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(this.data.FooterConsumo[index].CodigoBSID.Value) && d.inReferenciaID.Value.Equals(e.Value.ToString())))
                        {
                            validField = true;
                            //object res = this._listConvenio.Where(x => x.inReferenciaID.Value.Equals(e.Value) && x.CodigoBSID.Value.Equals(this.data.FooterConsumo[index].CodigoBSID.Value)).First();
                            //convenio = (DTO_prConvenio)res;
                            //this.data.FooterConsumo[index].ValorUni.Value = convenio.Valor.Value;
                        }
                    //else
                    //    this.data.FooterConsumo[index].ValorUni.Value = 0;
                    this.gcDocument.RefreshDataSource();
                } 
                #endregion
                #region Prefijo
                if (fieldName == "PrefijoID")
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.glPrefijo);
                #endregion
                #endregion
                #region Otros campos
                //if (fieldName == "FechaPlanilla")
                //    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, true, false);

                if (fieldName == "Cantidad")
                {
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, true, false);
                    if (this.data.FooterConsumo[index].ValorUni.Value != 0)
                    {
                        this.data.FooterConsumo[index].Valor.Value = this.data.FooterConsumo[index].ValorUni.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                    }
                    this.gcDocument.RefreshDataSource();
                }
                if (fieldName == "DocumentoNro")
                {
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, true, false, true, false);

                    if ((!string.IsNullOrEmpty(this.data.FooterConsumo[e.RowHandle].CodigoBSID.Value) || !string.IsNullOrEmpty(this.data.FooterConsumo[e.RowHandle].inReferenciaID.Value)) &&
                         !string.IsNullOrEmpty(this.data.FooterConsumo[e.RowHandle].PrefijoID.Value) && this.data.FooterConsumo[e.RowHandle].Cantidad.Value > 0)
                    {
                        int numeroDocOC = this.GetConveniosOrdenCompra(this.data.FooterConsumo[e.RowHandle]);
                        if (numeroDocOC == 0)
                        {
                            validField = false;
                            //rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_NotExistOrdenCompra);
 
                        }
                        else
                            this.data.FooterConsumo[e.RowHandle].NumeroDocOC.Value = numeroDocOC;
                    }
                }
                if (fieldName == "SerialID")
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, true, false, true, false);
                #endregion

                if (!validField)
                    this.isValid = false;
                else
                    this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.disableValidate)
                {
                    bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        this.isValid = true;
                        FormProvider.Master.itemSendtoAppr.Enabled = true;
                    }
                    else
                    {
                        e.Allow = false;
                        this.isValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "gvDocument_BeforeLeaveRow"));
            }

        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (this.validHeader)
            {
                if (this.data == null)
                {
                    this.gcDocument.Focus();
                    e.Handled = true;
                }
                else
                    this.gvDocument.PostEditor();

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvDocument.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                        {
                            this.newReg = true;
                            this.AddNewRow();
                        }
                        else
                        {
                            bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                            if (isV)
                            {
                                this.newReg = true;
                                this.AddNewRow();
                            }
                        }
                    }
                }
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvDocument.FocusedRowHandle;

                        if (this.data.FooterConsumo.Count == 1)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                            e.Handled = true;
                        }
                        else
                        {
                            this.data.FooterConsumo.RemoveAll(x => x.Index == this.indexFila);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this.data);

                            this.gvDocument.RefreshData();
                            this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                        }
                    }
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;

            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                try
                {
                    if(this.data.FooterConsumo.Count == 0)
                        this.LoadData(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para recargar los controles del formulario
        /// </summary>
        public override void TBNew()
        {
            this.SaveMethod();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            if (this.ValidateHeader() && this.data.FooterConsumo.Count > 0)
            {
                int c = 0;
                foreach (var det in this.data.FooterConsumo)
                {
                    det.ProyectoID.Value = this.masterProyecto.Value;
                    det.CentroCostoID.Value = this.masterCtoCosto.Value;
                    //this.ValidateRow(c);
                    c++;
                }
                if(this.data.FooterConsumo.TrueForAll(det => this.ValidateRow(det.Index)))
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
          
            if (!this.ValidateHeader() || !this.ReplaceDocument())
                return;

            this.gvDocument.ActiveFilterString = string.Empty;

            //bool hasItems = this.data.FooterConsumo.GetEnumerator().MoveNext() ? true : false;
            //if (hasItems)
            //    this.gvDocument.MoveFirst();

            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                int numDoc = 0;
                bool update = false;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                if (this.data.DocCtrl.NumeroDoc.Value != 0)
                    update = true;
                DTO_SerializedObject obj = this._bc.AdministrationModel.Convenio_Add(this.documentID, this.data, out numDoc, update);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true,true);
                if (isOK)
                {
                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_Convenios();
                    this._listConvenio = new List<DTO_prConvenio>();
                    this.terceroID = string.Empty;
                    this.validHeader = false;
                    this.Invoke(this.saveDelegate);
                }
                else if (!update)
                    this.data.DocCtrl.NumeroDoc.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsumoProyecto.cs", "SaveThread" + ex.Message));
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_prConvenioConsumoDirecto> listFooter = new List<DTO_prConvenioConsumoDirecto>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    Dictionary<string, DTO_glBienServicioClase> bienServ = new Dictionary<string, DTO_glBienServicioClase>();
                    Dictionary<string, DTO_inReferencia> refer = new Dictionary<string, DTO_inReferencia>();
                    Dictionary<string, DTO_prProveedor> proveedor = new Dictionary<string, DTO_prProveedor>();
                    Dictionary<string, DTO_MasterBasic> prefijo = new Dictionary<string, DTO_MasterBasic>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();

                    #region Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField); // "Vacio"
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat); // "Formato incorrecto"
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength); // "Los datos ingresados tienen longitud superior a la permitida"
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound); // "El código {0} no existe'
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField); // "Ningún registro copiado"
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine); // "Linea copiada incompleta"
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather); // "No puede importar ningún código jerárquico sin movimiento"
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField); // "{0} debe tener un valor diferente de cero'
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue); // "El valor de {0} debe ser un número positivo'
                    string msgCantidadDeCargos = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_CantidadDeCargos); // "El numero de cargos no puede ser superior a 1 para esta clase de bienes y servicios"
                    string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField); // "Inválido"

                    #endregion

                    //Popiedades 
                    DTO_prConvenioConsumoDirecto det = new DTO_prConvenioConsumoDirecto();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion

                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis1 = typeof(DTO_prConvenioConsumoDirecto).GetProperties();

                    #region Columnas que corresponden a prConvenioConsumoDirecto
                    foreach (PropertyInfo pi in pis1)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Fks
                    fks.Add(this._codigoProveedorRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._codigoBSRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._referenciaRsx, new List<Tuple<string, bool>>());
                    #endregion
                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;

                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Barra de Progresso
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }
                        #endregion
                        #region Valida si existe data en la lista importada
                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        #endregion

                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        #endregion

                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];  // Obtiene nombre de Columna
                                    colVals[colRsx] = line[colIndex]; // Obtiene valor columna

                                    //Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        #region Works with Fks only
                                        if (colRsx == this._codigoBSRsx || colRsx == this._referenciaRsx || colRsx == this._codigoProveedorRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            #region Revisa si la columna ya existe
                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                            {
                                                continue;
                                            }
                                            if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                            #endregion

                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = this._bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                                    #region Asigna los valores de las referencias y Bien Servicio
                                                    if (colRsx == this._codigoProveedorRsx)
                                                    {
                                                        if (!proveedor.Keys.Contains(line[colIndex].Trim().ToUpper()))
                                                        {
                                                            DTO_prProveedor rf = (DTO_prProveedor)dto;
                                                            proveedor.Add(line[colIndex].Trim().ToUpper(), rf);
                                                        }
                                                    }
                                                    if (colRsx == this._codigoBSRsx)
                                                    {
                                                        if (!bienServ.Keys.Contains(line[colIndex].Trim().ToUpper()))
                                                        {
                                                            DTO_prBienServicio bs = (DTO_prBienServicio)dto;
                                                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bs.ClaseBSID.Value, true);
                                                            bienServ.Add(line[colIndex].Trim().ToUpper(), bsClase);
                                                        }
                                                    }

                                                    if (colRsx == this._referenciaRsx && !string.IsNullOrEmpty(line[colIndex].Trim()))
                                                    {
                                                        if (!refer.Keys.Contains(line[colIndex].Trim().ToUpper()))
                                                        {
                                                            DTO_inReferencia rf = (DTO_inReferencia)dto;
                                                            refer.Add(line[colIndex].Trim().ToUpper(), rf);
                                                        }
                                                    }
                                                    if (colRsx == "PrefijoID")
                                                    {
                                                        if (!prefijo.Keys.Contains(line[colIndex].Trim().ToUpper()))
                                                        {
                                                            DTO_MasterBasic rf = (DTO_MasterBasic)dto;
                                                            prefijo.Add(line[colIndex].Trim().ToUpper(), rf);
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                det = new DTO_prConvenioConsumoDirecto();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx]; // Nombres de las columnas
                                        string colValue = colVals[colRsx].ToString().Trim(); // Valores de las columnas

                                        #region Valida si exige la referencia o aplica una por defecto de acuerdo al CodigoBS
                                        if (colRsx == _codigoBSRsx)
                                        {
                                            DTO_glBienServicioClase bsClase = bienServ[colValue];
                                            if (bsClase != null && bsClase.TipoCodigo.Value != (byte)TipoCodigo.Inventario)
                                            {
                                                if (!string.IsNullOrEmpty(colVals[_referenciaRsx].ToString()))
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Pr_ReferenciaInvalid);
                                                    rd.DetailsFields.Add(rdF);
                                                    createDTO = false;
                                                }
                                                else
                                                {
                                                    string refxDefecto = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                                                    colVals[_referenciaRsx] = refxDefecto;
                                                    if (!refer.Keys.Contains(refxDefecto))
                                                    {
                                                        DTO_inReferencia rf = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, refxDefecto, true);
                                                        refer.Add(refxDefecto, rf);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colRsx == this._codigoProveedorRsx || colRsx == this._codigoBSRsx || colRsx == this._referenciaRsx ||
                                                                              colRsx == this._fechaPlanillaRsx || colRsx == this._cantidadRsx || colRsx == "PrefijoID" || colRsx == "DocumentoNro"))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);
                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        #region Defines UDT type
                                        PropertyInfo pi = det.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(det, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #endregion
                                        #region Validaciones basicas
                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion
                                        #endregion
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue); // Llena el campo del dto con info valida
                                    }
                                    #region Exception
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "ConsumoProyecto.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Revisa si algun campo invalido
                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion

                            if (createDTO && validList)
                            {
                                listFooter.Add(det); // agrega una linea valida a la lista
                            }
                            else
                                validList = false;
                        }

                    }
                    #endregion
                    #region Valida las restricciones particulares de la solicitud
                    if (validList)
                    {
                        result.Details = new List<DTO_TxResultDetail>();

                        int index = this.NumFila;
                        int i = 0;

                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        percent = 0;

                        foreach (DTO_prConvenioConsumoDirecto dto in listFooter)
                        {
                            #region Progress bar
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (listFooter.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion

                            #region Indexes
                            dto.Index = i;
                            i++;
                            #endregion

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF;
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";


                            DTO_glBienServicioClase bsClase = bienServ[dto.CodigoBSID.Value];
                            DTO_inReferencia referencia = null;
                            if (!string.IsNullOrEmpty(dto.inReferenciaID.Value))
                                referencia = refer[dto.inReferenciaID.Value];

                            #region Validaciones particulares de la solicitud al importar del DTO
                            this.ValidateDataImport(dto, bsClase, referencia, rd, msgFkNotFound, msgInvalidField);

                            int numeroDocOC = this.GetConveniosOrdenCompra(dto);
                            if (numeroDocOC == 0)
                            {
                                rdF = new DTO_TxResultDetailFields();
                                rdF.Field = dto.PrefijoID.ColRsx;
                                rdF.Message = this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Pr_NotExistOrdenCompra);
                                rd.DetailsFields.Add(rdF);
                            }
                            dto.NumeroDocOC.Value = numeroDocOC;
                            #endregion                            
                            #region Valida los datos de los convenios
                            //if (this._listConvenio.Count > 0)
                            //{                               
                                //DTO_prConvenio convenio;
                                //if (!this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && d.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)))
                                //{
                                //    rdF = new DTO_TxResultDetailFields();
                                //    rdF.Field = dto.CodigoBSID.ColRsx;
                                //    rdF.Message = string.Format(msgFkNotFound, dto.CodigoBSID.ColRsx);
                                //    rd.DetailsFields.Add(rdF);
                                //    createDTO = false;
                                //}
                                //else
                                //{
                                //    object res = this._listConvenio.Where(x => x.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && x.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)).First();
                                //    convenio = (DTO_prConvenio)res;
                                //    dto.ValorUni.Value = convenio.Valor.Value;
                                //    dto.Valor.Value = dto.ValorUni.Value * dto.CantidadSol.Value;
                                //}
                            //}
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = ResultValue.NOK.ToString();
                            }
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.data.FooterConsumo = listFooter; // Modificar si se agregan con los registros existentes
                            this.Invoke(this.refreshGridDelegate);
                        }

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result;

                result = _bc.AdministrationModel.Convenio_SendToAprob(this.documentID, this._actFlujo.ID.Value, this.data.DocCtrl.NumeroDoc.Value.Value, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_Convenios();
                    this._listConvenio = new List<DTO_prConvenio>();
                    this.terceroID = string.Empty;
                    this.validHeader = false;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ConsumoProyecto.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

       
    }
}
