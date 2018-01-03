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
    public partial class ConvenioSolicitud : DocumentForm
    {
        public ConvenioSolicitud()
        {
            //InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private int _tipoMoneda = 0;

        private string monedaLocal;
        private string monedaExtranjera;
        private decimal _tasaCambio = 0;

        //Variables formulario
        protected DTO_Convenios data = null;
        private List<DTO_prConvenio> _listConvenio = null;
        private Dictionary<string, DTO_glBienServicioClase> _codigoBSClase = new Dictionary<string, DTO_glBienServicioClase>();
        private string terceroID = string.Empty;
        //Indica si el header es valido
        protected bool validHeader;

        //Variables con los recursos de las Fks
        private string _codigoBSRsx = string.Empty;
        private string _referenciaRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string _centroCtoRsx = string.Empty;
        private string _cantidadRsx = string.Empty;
        private string _valorUniRsx = string.Empty;
        private string _valorTotalRsx = string.Empty;
        private bool _copyData = false;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = null;
            this.disableValidate = false;

            this.masterProveedor.Value = string.Empty;
            this.masterMoneda.Value = string.Empty;
            this.txtNroOrdenCompra.Text = string.Empty;
            this.txtTotal.EditValue = 0;

            this.EnableHeader(true);
            this.masterProveedor.Focus();

            FormProvider.Master.itemSendtoAppr.Enabled = false;
            FormProvider.Master.itemImport.Enabled = false;
            this.btnQueryDoc.Enabled = true;
            this.data = new DTO_Convenios();
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
            get { return this.data.FooterSolDespacho.FindIndex(det => det.Index == this.indexFila); }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            base.gvDocument.OptionsView.ColumnAutoWidth = true;

            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, true);
            this._bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, true);

            this.masterMoneda.EnableControl(false);
            this.txtTotal.Text = "0";

            this.tlSeparatorPanel.RowStyles[0].Height = 25;
            this.tlSeparatorPanel.RowStyles[1].Height = 80;
            this.tlSeparatorPanel.RowStyles[2].Height = 40;
        }

        /// <summary>
        /// Calcula valor factura + iva
        /// </summary>
        private void CalcularTotal()
        {
            decimal valor = 0;
            foreach (var item in this.data.FooterSolDespacho)
            {
                valor += item.Valor.Value.Value;
            }
            this.txtTotal.EditValue = valor;
            this.data.Header.Valor.Value = valor;
        }   

        /// <summary>
        /// Valida un DTO el footer en la importacion
        /// </summary>
        /// <param name="ctrl">DTO_SolicitudDespachoFooter a validar</param>
        /// <param name="suppl">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidDate">Fecha en periodo invalido</param>
        /// <param name="msgFkNotFound">FK inexistente</param>
        /// <param name="msgFkHierarchyFather">No es una hoja de la jerarquia</param>
        /// <param name="msgCero">Mensaje que no permite ceros en un campo</param>
        /// <param name="msgPositive">Solo permite valores positivos</param>
        private void ValidateDataImport(DTO_SolicitudDespachoFooter dto, DTO_glBienServicioClase bsClase, DTO_inReferencia refTipo, DTO_TxResultDetail rd, string msgFkNotFound, string msgInvalidField)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            DTO_SolicitudDespachoFooter dtoDet = (DTO_SolicitudDespachoFooter)dto;
            #region Variables y diccionarios
            TipoCodigo tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString());

            Dictionary<string, string> pksPar1 = new Dictionary<string, string>();
            Dictionary<string, string> pksPar2 = new Dictionary<string, string>();

            #endregion
            #region Valida las FKs
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
            #region ProyectoID
            colRsx = this._proyectoRsx;
            rdF = _bc.ValidGridCell(colRsx, dtoDet.ProyectoID.Value, false, true, false, AppMasters.coProyecto);
            if (rdF != null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = string.Format(msgFkNotFound, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CentroCostoID
            colRsx = this._centroCtoRsx;
            rdF = _bc.ValidGridCell(colRsx, dtoDet.CentroCostoID.Value, false, true, false, AppMasters.coCentroCosto);
            if (rdF != null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = string.Format(msgFkNotFound, colRsx);
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #endregion
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
                this.data.DocCtrl.TerceroID.Value = this.terceroID; 
                this.data.DocCtrl.NumeroDoc.Value = 0;
                this.data.DocCtrl.ComprobanteID.Value = string.Empty; 
                this.data.DocCtrl.ComprobanteIDNro.Value = 0;
                this.data.DocCtrl.MonedaID.Value = this.masterMoneda.Value; 
                this.data.DocCtrl.CuentaID.Value = string.Empty; 
                this.data.DocCtrl.ProyectoID.Value = string.Empty; 
                this.data.DocCtrl.CentroCostoID.Value = string.Empty; 
                this.data.DocCtrl.LugarGeograficoID.Value = string.Empty; 
                this.data.DocCtrl.LineaPresupuestoID.Value = string.Empty;
                this.data.DocCtrl.Fecha.Value = DateTime.Now;
                this.data.DocCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this.data.DocCtrl.PrefijoID.Value = this.prefijoID;
                this.data.DocCtrl.TasaCambioCONT.Value = this._bc.AdministrationModel.TasaDeCambio_Get(monedaExtranjera, this.dtFecha.DateTime);
                this.data.DocCtrl.TasaCambioDOCU.Value = this.data.DocCtrl.TasaCambioCONT.Value;
                this.data.DocCtrl.DocumentoNro.Value = 0;
                this.data.DocCtrl.DocumentoID.Value = this.documentID;
                this.data.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
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

            this.gcDocument.DataSource = this.data.FooterSolDespacho;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.data.FooterSolDespacho.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
             this.gvDocument.MoveFirst();
            this.dataLoaded = true;
            if (this.data.FooterSolDespacho.Count > 0)
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
            this.documentID = AppDocuments.SolicitudDespachoConvenio;

            base.SetInitParameters();

            this.data = new DTO_Convenios();
            this.data.FooterSolDespacho = new List<DTO_SolicitudDespachoFooter>(); 

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.pr;

            //Carga recursos importacion
            this._codigoBSRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            this._referenciaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            this._proyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            this._centroCtoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            this._cantidadRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
            this._valorUniRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
            this._valorTotalRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");

            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            
            //Carga config de controles           
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

                //CodigoBSID
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = this._codigoBSRsx;
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 0;
                CodigoBSID.Width = 80;
                CodigoBSID.Visible = true;
                CodigoBSID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(CodigoBSID);

                //inReferenciaID
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = this._referenciaRsx;
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 0;
                inReferenciaID.Width = 80;
                inReferenciaID.Visible = true;
                inReferenciaID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Cantidad
                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "CantidadSol";
                Cantidad.Caption = this._cantidadRsx;
                Cantidad.UnboundType = UnboundColumnType.Decimal;
                Cantidad.VisibleIndex = 1;
                Cantidad.Width = 120;
                Cantidad.Visible = true;
                Cantidad.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(Cantidad);

                //ValorUnit
                GridColumn ValorUnit = new GridColumn();
                ValorUnit.FieldName = this.unboundPrefix + "ValorUni";
                ValorUnit.Caption = this._valorUniRsx;
                ValorUnit.UnboundType = UnboundColumnType.Decimal;
                ValorUnit.VisibleIndex = 2;
                ValorUnit.Width = 120;
                ValorUnit.Visible = true;
                ValorUnit.OptionsColumn.AllowEdit = false;
                ValorUnit.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(ValorUnit);

                //Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = this._valorTotalRsx;
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 3;
                Valor.Width = 120;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                Valor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(Valor);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = this._proyectoRsx;
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 4;
                ProyectoID.Width = 80;
                ProyectoID.Visible = true;
                ProyectoID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(ProyectoID);

                //CentroCostoID
                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                CentroCostoID.Caption = this._centroCtoRsx;
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.VisibleIndex = 5;
                CentroCostoID.Width = 80;
                CentroCostoID.Visible = true;
                CentroCostoID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(CentroCostoID);

                //Porcentaje
                GridColumn PorcentajeDistr = new GridColumn();
                PorcentajeDistr.FieldName = this.unboundPrefix + "Porcentaje";
                PorcentajeDistr.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Porcentaje");
                PorcentajeDistr.UnboundType = UnboundColumnType.Decimal;
                PorcentajeDistr.VisibleIndex = 6;
                PorcentajeDistr.Width = 40;
                PorcentajeDistr.Visible = true;
                PorcentajeDistr.OptionsColumn.AllowEdit = false;
                PorcentajeDistr.ColumnEdit = this.editSpinPorcen;
                this.gvDocument.Columns.Add(PorcentajeDistr);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "ConvenioSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.format += this._bc.GetImportExportFormat(typeof(DTO_SolicitudDespachoFooter), this.documentID);
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_SolicitudDespachoFooter footerDet = new DTO_SolicitudDespachoFooter();

            #region Asigna datos a la fila
            try
            {
                if (this.data.FooterSolDespacho.Count > 0)
                    footerDet.Index = this.data.FooterSolDespacho.Last().Index + 1;
                else
                    footerDet.Index = 0;
                footerDet.CodigoBSID.Value = string.Empty;
                footerDet.inReferenciaID.Value = string.Empty;
                footerDet.ProyectoID.Value = string.Empty;
                footerDet.CentroCostoID.Value = string.Empty;
                footerDet.CantidadSol.Value = 0;
                footerDet.ValorUni.Value = 0;
                footerDet.Valor.Value = 0;
                footerDet.Porcentaje.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "AddNewRow"));
            }
            #endregion

            this.data.FooterSolDespacho.Add(footerDet);
            this.gvDocument.RefreshData();
            this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

            this.isValid = false;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected  bool ValidateHeader()
        {
            #region Valida los datos obligatorios

            #region Valida datos en la maestra de Proveedor
            if (!this.masterProveedor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProveedor.CodeRsx);

                MessageBox.Show(msg);
                this.masterProveedor.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Moneda
            if (!this.masterMoneda.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMoneda.CodeRsx);

                MessageBox.Show(msg);
                this.masterMoneda.Focus();

                return false;
            }
            #endregion

            #region Valida datos en el control de Nro ORden
            if (string.IsNullOrEmpty(this.txtNroOrdenCompra.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNroOrdenCompra.Text);

                MessageBox.Show(msg);
                this.txtNroOrdenCompra.Focus();

                return false;
            }
            #endregion

            #region Valida que existan convenios del contrato
            if (this._listConvenio == null || this._listConvenio.Count == 0)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_ConveniosEmpty));
                this.txtNroOrdenCompra.Focus();

                return false;
            }
            #endregion

            #endregion
            return true;
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected  void EnableHeader(bool enable)
        {
            this.masterProveedor.EnableControl(enable);
            this.masterMoneda.EnableControl(false);
            this.txtNroOrdenCompra.Enabled = enable;
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
                GridColumn col = new GridColumn();
                #region Validacion de nulls y Fks       

                #region CodigoBS
                validField =this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CodigoBSID", false, true, true, AppMasters.prBienServicio);

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
                #region Proyecto
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProyectoID", false, true, false, AppMasters.coProyecto);
                if (!validField)
                {
                    MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this._proyectoRsx));
                    validRow = false;
                }
                #endregion
                #region CentroCosto
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CentroCostoID", false, true, false, AppMasters.coCentroCosto);
                if (!validField)
                {
                    MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this._centroCtoRsx));
                    validRow = false;
                }
                #endregion
                #region Cantidad Sol
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadSol", false, false, true, false);

                if (!validField)
                    validRow = false;
                #endregion
                #region ValorUni
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorUni", false, false, true, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region Valor
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Valor", false, false, true, false);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                if (validRow)
                    this.isValid = true;
                else
                    this.isValid = false;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "ValidateRow"));
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
            //CodigoBS
            if (colName == this._codigoBSRsx)
                return AppMasters.prBienServicio;
            //Referencia
            if (colName == this._referenciaRsx)
                return AppMasters.inReferencia;
            //Proyecto
            if (colName == this._proyectoRsx)
                return AppMasters.coProyecto;
            //CentroCosto
            if (colName == this._centroCtoRsx)
                return AppMasters.coCentroCosto;

            return 0;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {
            try
            {
                this.newReg = false;
                int cFila = fila;
                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Index"];
                this.indexFila = Convert.ToInt16(this.gvDocument.GetRowCellValue(cFila, col));

                this.LoadEditGridData(false, cFila);

                this.isValid = true;

                if (oper)
                    this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "RowIndexChanged"));
            }
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
        private void txtNroOrdenCompra_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.txtNroOrdenCompra.Text) && this.masterProveedor.ValidID)
                {
                    //Carga Orden de Compra
                    DTO_prOrdenCompra Orden = this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.OrdenCompra, this.prefijoID, Convert.ToInt32(this.txtNroOrdenCompra.Text));
                    if (Orden == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_NoOrdenCompra)); ////
                        this.txtNroOrdenCompra.Focus();
                        this.validHeader = false;
                        return;
                    }
                    else
                    {
                        DTO_Convenios convenioExist = this._bc.AdministrationModel.Convenio_GetByNroContrato(this.documentID, this.prefijoID, Orden.HeaderOrdenCompra.ContratoNro.Value.Value);

                        //Carga Contrato relacionado a la Orden de Compra
                        DTO_glDocumentoControl docContrato = this._bc.AdministrationModel.glDocumentoControl_GetByID(Orden.HeaderOrdenCompra.ContratoNro.Value.Value);

                        if (docContrato != null)
                        {
                            DTO_prOrdenCompra contrato = this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.Contrato, this.prefijoID.ToString(), docContrato.DocumentoNro.Value.Value);
                            if (this._copyData)
                            {
                                contrato.DocCtrl.NumeroDoc.Value = 0;
                                contrato.DocCtrl.DocumentoNro.Value = 0;
                                contrato.HeaderContrato.NumeroDoc.Value = 0;
                                contrato.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                                this._copyData = false;  
                            }
                            if (contrato != null && contrato.Convenio.Count > 0)
                            {
                                this._listConvenio = contrato.Convenio;
                                FormProvider.Master.itemImport.Enabled = true;
                                this.btnConvenios.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_NotAlreadyConvenioOrden));
                                this.btnConvenios.Enabled = false;
                            }

                            if (convenioExist == null || convenioExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                            {
                                #region Llena datos para traer Convenios de Contrato
                                this.masterMoneda.Value = Orden.HeaderOrdenCompra.MonedaOrden.Value;
                                this.data.Header.Moneda.Value = this.masterMoneda.Value;
                                this.data.Header.NumeroDocContrato.Value = Orden.HeaderOrdenCompra.ContratoNro.Value;
                                this.data.Header.Valor.Value = 0;
                                this.data.Header.IVA.Value = 0;
                                #endregion
                            }
                            else
                            {
                                #region Llena datos obteniendo el detalle de convenios existentes
                                this.masterMoneda.Value = Orden.HeaderOrdenCompra.MonedaOrden.Value;
                                this.txtTotal.EditValue = convenioExist.Header.Valor.Value;
                                this.data.Header.Moneda.Value = this.masterMoneda.Value;
                                this.data.Header.NumeroDocContrato.Value = Orden.HeaderOrdenCompra.ContratoNro.Value;
                                this.data.Header.Valor.Value = convenioExist.Header.Valor.Value;
                                this.data.Header.IVA.Value = convenioExist.Header.IVA.Value;
                                if (this.ValidateHeader())
                                {
                                    //this.EnableHeader(false);
                                    this.data = convenioExist;
                                    this.validHeader = true;
                                    if (convenioExist.FooterSolDespacho != null)
                                    {
                                        this.LoadData(false);
                                        this.gcDocument.Focus();
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_ContratoNotExist)); ////
                            this.txtNroOrdenCompra.Focus();
                            this.validHeader = false;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "txtNroOrdenCompra_Leave"));
            }
        }

        /// <summary>
        ///  Valida los datos ingresados al momento de dejar el control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            if (this.masterProveedor.ValidID)
            {
                this.data.Header.ProveedorID.Value = this.masterProveedor.Value;
                DTO_prProveedor proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
                this.terceroID = proveedor.TerceroID.Value;
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            if (this.masterProveedor.ValidID)
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.OrdenCompra);
                ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.txtNroOrdenCompra.Enabled = true;
                    this.txtNroOrdenCompra.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.prefijoID = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtNroOrdenCompra.Focus();
                    this.btnQueryDoc.Focus();
                    this.btnQueryDoc.Enabled = false;
                }
            }
            else
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                MessageBox.Show(string.Format(msg, this.masterProveedor.CodeRsx));
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
                object dto = (object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                        else
                        {
                            DTO_SolicitudDespachoFooter dtoDet = (DTO_SolicitudDespachoFooter)e.Row;
                            pi = dtoDet.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoDet, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoDet, null), null);
                            }
                            else
                            {
                                fi = dtoDet.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                        e.Value = fi.GetValue(dtoDet);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoDet), null);
                                }
                            }
                        }
                    }
                }

                if (e.IsSetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            pi.SetValue(dto, e.Value, null);
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                pi.SetValue(dto, e.Value, null);
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            DTO_SolicitudDespachoFooter dtoDet = (DTO_SolicitudDespachoFooter)e.Row;
                            pi = dtoDet.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                {
                                    e.Value = pi.GetValue(dtoDet, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoDet, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoDet.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        pi.SetValue(dto, e.Value, null);
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoDet);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
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
            int index = e.RowHandle;
            DTO_prConvenio convenio;
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                GridColumn col = this.gvDocument.Columns[e.Column.FieldName];
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region Valida el Tipo de Servicio

                if (fieldName != "CantidadSol")
                {
                    string value = this.data.FooterSolDespacho[index].CodigoBSID.Value;
                    if (!_codigoBSClase.Keys.Contains(value))
                    {
                        DTO_prBienServicio bs = (DTO_prBienServicio)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, value, true);
                        DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bs.ClaseBSID.Value, true);
                        _codigoBSClase.Add(value, bsClase);
                    }
                    if (_codigoBSClase != null && _codigoBSClase[value].TipoCodigo.Value != (byte)TipoCodigo.Inventario)
                    {
                        string referencia = this.data.FooterSolDespacho[index].inReferenciaID.Value;
                        string refxDefecto = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);

                        if (!string.IsNullOrEmpty(referencia) && referencia != refxDefecto)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Pr_ReferenciaInvalid));
                            validField = false;
                        }
                        else
                            this.data.FooterSolDespacho[index].inReferenciaID.Value = refxDefecto;
                    } 
                }
                #endregion

                #region Se modifican FKs
                if (fieldName == "CodigoBSID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.prBienServicio);
                    if (this._listConvenio.Count > 0)
                        if (this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(e.Value.ToString()) && d.inReferenciaID.Value.Equals(this.data.FooterSolDespacho[index].inReferenciaID.Value)))
                        {
                            validField = true;
                            object res = this._listConvenio.Where(x => x.CodigoBSID.Value.Equals(e.Value) && x.inReferenciaID.Value.Equals(this.data.FooterSolDespacho[index].inReferenciaID.Value)).First();
                            convenio = (DTO_prConvenio)res;
                            this.data.FooterSolDespacho[index].ValorUni.Value = convenio.Valor.Value;
                        }
                        else
                            this.data.FooterSolDespacho[index].ValorUni.Value = 0;
                }
                else if (fieldName == "inReferenciaID")
                {
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.inReferencia);
                    if (this._listConvenio.Count > 0)
                        if (this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(this.data.FooterSolDespacho[index].CodigoBSID.Value) && d.inReferenciaID.Value.Equals(e.Value.ToString())))
                        {
                            validField = true;
                            object res = this._listConvenio.Where(x => x.inReferenciaID.Value.Equals(e.Value) && x.CodigoBSID.Value.Equals(this.data.FooterSolDespacho[index].CodigoBSID.Value)).First();
                            convenio = (DTO_prConvenio)res;
                            this.data.FooterSolDespacho[index].ValorUni.Value = convenio.Valor.Value;
                        }
                        else
                            this.data.FooterSolDespacho[index].ValorUni.Value = 0;
                }
                else if (fieldName == "ProyectoID")
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.coProyecto);
                else if (fieldName == "CentroCostoID")
                    validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.coCentroCosto); 
                #endregion
                #region Se modifican CantidadSol
                else if (fieldName == "CantidadSol")
                {
                    validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, true, false);
                    if (this.data.FooterSolDespacho[index].ValorUni.Value != 0)
                        this.data.FooterSolDespacho[index].Valor.Value = this.data.FooterSolDespacho[index].ValorUni.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);   
                }
                #endregion

                if (!validField)
                    this.isValid = false;
                else
                    this.CalcularTotal();
                this.gcDocument.RefreshDataSource();
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "gvDocument_CellValueChanged"));
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
                if (!this.disableValidate && this.data.FooterSolDespacho.Count > 0)
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "gvDocument_BeforeLeaveRow"));
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

                        if (this.data.FooterSolDespacho.Count == 1)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                            e.Handled = true;
                        }
                        else
                        {
                            this.data.FooterSolDespacho.RemoveAll(x => x.Index == this.indexFila);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

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
                    if(this.data.FooterSolDespacho.Count == 0)
                        this.LoadData(true);                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "gcDocument_Enter"));
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

            bool valid = this.ValidateHeader();
            if (valid && this.data.FooterSolDespacho.Count > 0)
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            this.gcDocument.Focus();
            if (!this.ReplaceDocument())
                return;

            this.gvDocument.ActiveFilterString = string.Empty;

            //bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
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
                    this.terceroID = string.Empty;
                    this.validHeader = false;
                    this._listConvenio = new List<DTO_prConvenio>();
                    this.Invoke(this.saveDelegate);
                }
                else if (!update)
                     this.data.DocCtrl.NumeroDoc.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConvenioSolicitud.cs", "SaveThread" + ex.Message));
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
                    List<DTO_SolicitudDespachoFooter> listFooter = new List<DTO_SolicitudDespachoFooter>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    Dictionary<string, DTO_glBienServicioClase> bienServ = new Dictionary<string, DTO_glBienServicioClase>();
                    Dictionary<string, DTO_inReferencia> refer = new Dictionary<string, DTO_inReferencia>();
                    Dictionary<string, DTO_coProyecto> proy = new Dictionary<string, DTO_coProyecto>();
                    Dictionary<string, DTO_coCentroCosto> centroCto = new Dictionary<string, DTO_coCentroCosto>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();

                    #region Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField); // "Vacio"
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat); // "Formato incorrecto"
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound); // "El código {0} no existe'
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField); // "Ningún registro copiado"
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine); // "Linea copiada incompleta"
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather); // "No puede importar ningún código jerárquico sin movimiento"
                    string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField); // "Inválido"

                    #endregion

                    //Popiedades 
                    DTO_SolicitudDespachoFooter det = new DTO_SolicitudDespachoFooter();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion

                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis1 = typeof(DTO_SolicitudDespachoFooter).GetProperties();

                    #region Columnas que corresponden a prConvenioSolicitudDeta
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
                    fks.Add(this._codigoBSRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._referenciaRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._proyectoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._centroCtoRsx, new List<Tuple<string, bool>>());
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
                                        #region Recorre las FKs solamente
                                        if (colRsx == this._codigoBSRsx || colRsx == this._referenciaRsx || colRsx == this._proyectoRsx || colRsx == this._centroCtoRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            #region Revisa si la columna ya existe
                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                                continue;
                                            if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);
                                                createDTO = false;
                                            }
                                            #endregion
                                            else
                                            {
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
                                                    #region Asigna los valores de las FKs

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
                                                    if (colRsx == this._proyectoRsx && !string.IsNullOrEmpty(line[colIndex].Trim()))
                                                    {
                                                        if (!proy.Keys.Contains(line[colIndex].Trim().ToUpper()))
                                                        {
                                                            DTO_coProyecto pr = (DTO_coProyecto)dto;
                                                            proy.Add(line[colIndex].Trim().ToUpper(), pr);
                                                        }
                                                    }
                                                    if (colRsx == this._centroCtoRsx && !string.IsNullOrEmpty(line[colIndex].Trim()))
                                                    {
                                                        if (!centroCto.Keys.Contains(line[colIndex].Trim().ToUpper()))
                                                        {
                                                            DTO_coCentroCosto cto = (DTO_coCentroCosto)dto;
                                                            centroCto.Add(line[colIndex].Trim().ToUpper(), cto);
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
                                det = new DTO_SolicitudDespachoFooter();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx]; // Nombres de las columnas
                                        string colValue = colVals[colRsx].ToString().Trim(); // Valores de las columnas

                                        #region Valida si exige la referencia o aplica una por defecto de acuerdo al CodigoBS
                                        if (colRsx == this._codigoBSRsx)
                                        {
                                            DTO_glBienServicioClase bsClase = bienServ[colValue];
                                            if (bsClase != null && bsClase.TipoCodigo.Value != (byte)TipoCodigo.Inventario)
                                            {
                                                if (!string.IsNullOrEmpty(colVals[this._referenciaRsx].ToString()))
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
                                                    colVals[this._referenciaRsx] = refxDefecto;
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
                                        if (string.IsNullOrEmpty(colValue) && (colRsx == this._codigoBSRsx || colRsx == this._referenciaRsx ||
                                            colRsx == this._cantidadRsx || colRsx == this._proyectoRsx || colRsx == this._centroCtoRsx))
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

                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue); // Llena el campo del dto con info valida
                                    }
                                    #region Exception
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "ConvenioSolicitud.cs - Creacion de DTO y validacion Formatos");
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
                                listFooter.Add(det); // agrega una linea valida a la lista
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

                        foreach (DTO_SolicitudDespachoFooter dto in listFooter)
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

                            #endregion
                            #region Valida que no exista una llave CodigoBS y Referencia duplicada
                            int exist = listFooter.Count(x => x.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && x.inReferenciaID.Value.Equals(dto.inReferenciaID.Value));
                            if (exist > 1)
                            {
                                rdF = new DTO_TxResultDetailFields();
                                rdF.Field = dto.CodigoBSID.ColRsx;
                                rdF.Message = this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Pr_DuplicateKey); 
                                rd.DetailsFields.Add(rdF);
                                createDTO = false;
                            }
                            #endregion
                            #region Valida los datos de los convenios
                            if (this._listConvenio.Count > 0)
                            {
                               
                                DTO_prConvenio convenio;
                                if (!this._listConvenio.Exists(d => d.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && d.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)))
                                {
                                    rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = dto.CodigoBSID.ColRsx;
                                    rdF.Message = string.Format(msgFkNotFound, dto.CodigoBSID.ColRsx);
                                    rd.DetailsFields.Add(rdF);
                                    createDTO = false;
                                }
                                else
                                {
                                    object res = this._listConvenio.Where(x => x.CodigoBSID.Value.Equals(dto.CodigoBSID.Value) && x.inReferenciaID.Value.Equals(dto.inReferenciaID.Value)).First();
                                    convenio = (DTO_prConvenio)res;
                                    dto.ValorUni.Value = convenio.Valor.Value;
                                    dto.Valor.Value = dto.ValorUni.Value * dto.CantidadSol.Value;
                                    dto.Porcentaje.Value = 100;
                                }
                            }
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
                            this.data.FooterSolDespacho = listFooter; // Modificar si se agregan con los registros existentes
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

                result = _bc.AdministrationModel.Convenio_SendToAprob(this.documentID, this._actFlujo.ID.Value,this.data.DocCtrl.NumeroDoc.Value.Value, true);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ConvenioSolicitud.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

      

    }
}
