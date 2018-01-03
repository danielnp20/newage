using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DeterioroRevalorizacionInv : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private DTO_inCostosExistencias _costosMvto;
        private DTO_MvtoInventarios _dataDeterioro = null;
        private DTO_glDocumentoControl _docCtrlDeterioro = new DTO_glDocumentoControl();

        //Variables con valores x defecto (glControl)
        private string monedaLocal;
        private string monedaExtranjera;      
        protected string defPrefijo = string.Empty;
        protected string defProyecto = string.Empty;
        protected string defCentroCosto = string.Empty;
        protected string defLugarGeo = string.Empty;
        protected string defTercero = string.Empty;
        protected string tipoMovDet = string.Empty;
        protected string tipoMovReval = string.Empty;
        //Indica si el header es valido
        private bool validHeader;
        //variables para funciones particulares
        private bool cleanDoc = true;

        private decimal _tasaCambio = 0;
        private string param1xDef = string.Empty;
        private string param2xDef = string.Empty;
        private bool _copyData = false;

        #endregion
        
        #region Delegados
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.CleanData();
        } 
        #endregion

        #region Propiedades

        ///// <summary>
        ///// Comprobante sobre el cual se esta trabajando
        ///// </summary>
        //private DTO_MvtoInventarios _data = null;


        //Numero de una fila segun el indice
        protected int NumFila
        {
            get
            {
                return this._dataDeterioro.Footer.FindIndex(det => det.Movimiento.Index == this.indexFila);
            }
        }

        #endregion

        public DeterioroRevalorizacionInv()
        {
         //  this.InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._costosMvto = new DTO_inCostosExistencias();
            this._dataDeterioro = new DTO_MvtoInventarios();
            this._docCtrlDeterioro = new DTO_glDocumentoControl();
            this.cleanDoc = true;
            this.validHeader = false;

            this.masterPrefijo.Value = string.Empty;         
            this.masterCosteoGrupo.Value = string.Empty;
            this.txtNro.Text = "0";
            this.cmbTipoDoc.EditValue = 1;

            this.EnableHeader(true);

            this.gcDocument.DataSource = this._dataDeterioro.Footer;
            this.newDoc = true;
            this.masterPrefijo.Focus();
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected void EnableHeader(bool enable)
        {
            this.masterPrefijo.EnableControl(enable);
            this.masterCosteoGrupo.EnableControl(enable);

            this.txtNro.Enabled = enable;
            this.btnQueryDoc.Enabled = enable;
            this.cmbEstado.Enabled = enable;
            this.cmbTipoDoc.Enabled = enable;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            bool result = true;
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();

                result = false;
            }
            if (!this.masterCosteoGrupo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCosteoGrupo.CodeRsx);

                MessageBox.Show(msg);
                this.masterCosteoGrupo.Focus();

                result = false;
            }
            if (string.IsNullOrEmpty(this.txtNro.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNro.Text);

                MessageBox.Show(msg);
                this.txtNro.Focus();

                result = false;
            }
          

            return result;
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                #region Columnas visibles

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 1;
                codRef.Width = 100;
                codRef.Visible = true;
                codRef.OptionsColumn.AllowEdit = true;
                codRef.ColumnEdit = this.editBtnGrid;
                codRef.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(codRef);

                //ReferenciaDesc
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "DescripTExt";
                Descriptivo.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 2;
                Descriptivo.Width = 150;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                Descriptivo.OptionsColumn.AllowFocus = false;
                Descriptivo.Fixed = FixedStyle.Left;               
                this.gvDocument.Columns.Add(Descriptivo);

                //CantidadUNI
                GridColumn CantidadUNI = new GridColumn();
                CantidadUNI.FieldName = this.unboundPrefix + "CantidadUNI";
                CantidadUNI.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
                CantidadUNI.UnboundType = UnboundColumnType.Integer;
                CantidadUNI.VisibleIndex = 3;
                CantidadUNI.Width = 70;
                CantidadUNI.Visible = true;
                CantidadUNI.OptionsColumn.AllowEdit = false;
                CantidadUNI.OptionsColumn.AllowFocus = false;
                CantidadUNI.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(CantidadUNI);

                #region Moneda Local
                //ValorActualUniLOC
                GridColumn ValorActualUniLOC = new GridColumn();
                ValorActualUniLOC.FieldName = this.unboundPrefix + "ValorActualUniLOC";
                ValorActualUniLOC.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorActualUniLOC");
                ValorActualUniLOC.UnboundType = UnboundColumnType.Decimal;
                ValorActualUniLOC.VisibleIndex = 4;
                ValorActualUniLOC.Width = 80;
                ValorActualUniLOC.ColumnEdit = this.editSpin;
                ValorActualUniLOC.Visible = true;
                ValorActualUniLOC.OptionsColumn.AllowEdit = false;
                ValorActualUniLOC.OptionsColumn.AllowFocus = false;
                this.gvDocument.Columns.Add(ValorActualUniLOC);

                //NuevoValorUniLOC
                GridColumn NuevoValorUniLOC = new GridColumn();
                NuevoValorUniLOC.FieldName = this.unboundPrefix + "NuevoValorUniLOC";
                NuevoValorUniLOC.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_NuevoValorUniLOC");
                NuevoValorUniLOC.UnboundType = UnboundColumnType.Decimal;
                NuevoValorUniLOC.VisibleIndex = 5;
                NuevoValorUniLOC.Width = 80;
                NuevoValorUniLOC.ColumnEdit = this.editSpin;
                NuevoValorUniLOC.Visible = true;
                NuevoValorUniLOC.OptionsColumn.AllowEdit = true;
                NuevoValorUniLOC.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                NuevoValorUniLOC.AppearanceCell.Options.UseTextOptions = true;
                NuevoValorUniLOC.AppearanceCell.Options.UseFont = true;
                this.gvDocument.Columns.Add(NuevoValorUniLOC);

                //ValorAjusteUniLOC
                GridColumn ValorAjusteUniLOC = new GridColumn();
                ValorAjusteUniLOC.FieldName = this.unboundPrefix + "ValorAjusteUniLOC";
                ValorAjusteUniLOC.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjusteUniLOC");
                ValorAjusteUniLOC.UnboundType = UnboundColumnType.Decimal;
                ValorAjusteUniLOC.VisibleIndex = 6;
                ValorAjusteUniLOC.Width = 90;
                ValorAjusteUniLOC.ColumnEdit = this.editSpin;
                ValorAjusteUniLOC.Visible = true;
                ValorAjusteUniLOC.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorAjusteUniLOC);

                //Valor1LOC (ValorAjusteTotalLOC)
                GridColumn ValorAjusteTotalML = new GridColumn();
                ValorAjusteTotalML.FieldName = this.unboundPrefix + "Valor1LOC";
                ValorAjusteTotalML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjusteTotalLOC");
                ValorAjusteTotalML.UnboundType = UnboundColumnType.Decimal;
                ValorAjusteTotalML.VisibleIndex = 7;
                ValorAjusteTotalML.Width = 90;
                ValorAjusteTotalML.ColumnEdit = this.editSpin;
                ValorAjusteTotalML.Visible = true;
                ValorAjusteTotalML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorAjusteTotalML); 
                #endregion
                #region Moneda Extranjera
                //ValorActualUniEXT
                GridColumn ValorActualUniEXT = new GridColumn();
                ValorActualUniEXT.FieldName = this.unboundPrefix + "ValorActualUniEXT";
                ValorActualUniEXT.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorActualUniEXT");
                ValorActualUniEXT.UnboundType = UnboundColumnType.Decimal;
                ValorActualUniEXT.VisibleIndex = 8;
                ValorActualUniEXT.Width = 80;
                ValorActualUniEXT.ColumnEdit = this.editSpin;
                ValorActualUniEXT.Visible = false;
                ValorActualUniEXT.OptionsColumn.AllowEdit = false;
                ValorActualUniEXT.OptionsColumn.AllowFocus = false;
                this.gvDocument.Columns.Add(ValorActualUniEXT);

                //NuevoValorUniEXT
                GridColumn NuevoValorUniEXT = new GridColumn();
                NuevoValorUniEXT.FieldName = this.unboundPrefix + "NuevoValorUniEXT";
                NuevoValorUniEXT.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_NuevoValorUniEXT");
                NuevoValorUniEXT.UnboundType = UnboundColumnType.Decimal;
                NuevoValorUniEXT.VisibleIndex = 9;
                NuevoValorUniEXT.Width = 80;
                NuevoValorUniEXT.ColumnEdit = this.editSpin;
                NuevoValorUniEXT.Visible = false;
                NuevoValorUniEXT.OptionsColumn.AllowEdit = true;
                NuevoValorUniEXT.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                NuevoValorUniEXT.AppearanceCell.Options.UseTextOptions = true;
                NuevoValorUniEXT.AppearanceCell.Options.UseFont = true;
                this.gvDocument.Columns.Add(NuevoValorUniEXT);

                //ValorAjusteUniEXT
                GridColumn ValorAjusteUniEXT = new GridColumn();
                ValorAjusteUniEXT.FieldName = this.unboundPrefix + "ValorAjusteUniEXT";
                ValorAjusteUniEXT.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjusteUniEXT");
                ValorAjusteUniEXT.UnboundType = UnboundColumnType.Decimal;
                ValorAjusteUniEXT.VisibleIndex = 10;
                ValorAjusteUniEXT.Width = 90;
                ValorAjusteUniEXT.ColumnEdit = this.editSpin;
                ValorAjusteUniEXT.Visible = false;
                ValorAjusteUniEXT.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorAjusteUniEXT);

                //Valor1EXT (ValorAjusteTotalEXT)
                GridColumn ValorAjusteTotalEXT = new GridColumn();
                ValorAjusteTotalEXT.FieldName = this.unboundPrefix + "Valor1EXT";
                ValorAjusteTotalEXT.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjusteTotalEXT");
                ValorAjusteTotalEXT.UnboundType = UnboundColumnType.Decimal;
                ValorAjusteTotalEXT.VisibleIndex = 11;
                ValorAjusteTotalEXT.Width = 90;
                ValorAjusteTotalEXT.ColumnEdit = this.editSpin;
                ValorAjusteTotalEXT.Visible = false;
                ValorAjusteTotalEXT.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorAjusteTotalEXT); 
                #endregion

                #endregion
                #region Columnas No Visibles
         
                //CodigoReferencia+Param1+Param2
                GridColumn codRefP1P2 = new GridColumn();
                codRefP1P2.FieldName = this.unboundPrefix + "ReferenciaIDP1P2";
                codRefP1P2.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRefP1P2.UnboundType = UnboundColumnType.String;
                codRefP1P2.Visible = false;
                this.gvDocument.Columns.Add(codRefP1P2);

                //Descripcion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "ReferenciaIDP1P2Desc";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                desc.UnboundType = UnboundColumnType.String;
                desc.Visible = false;
                this.gvDocument.Columns.Add(desc);

                //Serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serial.UnboundType = UnboundColumnType.String;
                serial.Visible = false;
                this.gvDocument.Columns.Add(serial);

                //Estado
                GridColumn estado = new GridColumn();
                estado.FieldName = this.unboundPrefix + "EstadoInv";
                estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoInv");
                estado.UnboundType = UnboundColumnType.Integer;
                estado.Visible = false;
                this.gvDocument.Columns.Add(estado);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.UnboundType = UnboundColumnType.String;
                param1.Visible = false;
                this.gvDocument.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.UnboundType = UnboundColumnType.String;
                param2.Visible = false;
                this.gvDocument.Columns.Add(param2);

                //Unidad
                GridColumn unidadRef = new GridColumn();
                unidadRef.FieldName = this.unboundPrefix + "UnidadRef";
                unidadRef.UnboundType = UnboundColumnType.String;
                unidadRef.Visible = false;
                this.gvDocument.Columns.Add(unidadRef);

                //IdentificadorTr
                GridColumn param3 = new GridColumn();
                param3.FieldName = this.unboundPrefix + "IdentificadorTr";
                param3.UnboundType = UnboundColumnType.Integer;
                param3.Visible = false;
                this.gvDocument.Columns.Add(param3);         

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);    

                //valorFOBML
                GridColumn valorFOBML = new GridColumn();
                valorFOBML.FieldName = this.unboundPrefix + "Valor2LOC";
                valorFOBML.Visible = false;
                valorFOBML.UnboundType = UnboundColumnType.Decimal;
                this.gvDocument.Columns.Add(valorFOBML);

                //valorFOBME
                GridColumn valorFOBME = new GridColumn();
                valorFOBME.FieldName = this.unboundPrefix + "Valor2EXT";
                valorFOBME.Visible = false;
                valorFOBME.UnboundType = UnboundColumnType.Decimal;
                this.gvDocument.Columns.Add(valorFOBME);

                #endregion                
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DeterioroRevalorizacionInv.cs-AddDocumentCols"));
            }
        }

        /// <summary>
        /// Obtiene un movimiento de Inventarios
        /// </summary>
        private void GetMvtoInventario()
        {
            try
            {
                if (this._docCtrlDeterioro != null)
                {
                    if (this._docCtrlDeterioro.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, this._docCtrlDeterioro.NumeroDoc.Value.Value);
                        if (this._copyData)
                        {
                            saldoCostos.DocCtrl.NumeroDoc.Value = 0;
                            saldoCostos.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            saldoCostos.Header.NumeroDoc.Value = 0;
                            this._copyData = false;
                        }                        
                        this.LoadData(true);
                        this.validHeader = true;
                        this.EnableHeader(false);
                        this.newDoc = false;                     
                    }
                    else
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DeterioroRevalorizacion.cs-GetMvtoInventario"));
            }
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        private decimal LoadTasaCambio(DateTime fecha)
        {
            try
            {
                decimal valor = 0;
                valor = _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, fecha);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "LoadTasaCambio"));
                return 0;
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.DeterioroInv;
            this.frmModule = ModulesPrefix.@in;

            InitializeComponent();
            base.SetInitParameters();

            this.AddDocumentCols();
            this._dataDeterioro = new DTO_MvtoInventarios();
            
            //Trae valores por defecto
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.defPrefijo = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.defProyecto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defLugarGeo = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            this.defTercero = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            this.tipoMovDet = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovDeterioros);
            this.tipoMovReval =  this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovRevalorizaciones);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            //Modifica el tamaño de las secciones
            this.tlSeparatorPanel.RowStyles[0].Height = 110;
            this.tlSeparatorPanel.RowStyles[1].Height = 280; 
            this.tlSeparatorPanel.RowStyles[2].Height = 100;

            //Carga controles del Header
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterCosteoGrupo, AppMasters.inCosteoGrupo, true, true, true, false);

            Dictionary<int, string> dicTipoDoc = new Dictionary<int, string>();
            dicTipoDoc.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DeterioroInv));
            dicTipoDoc.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_RevalorizacionInv));
            this.cmbTipoDoc.EditValue = 1;
            this.cmbTipoDoc.Properties.DataSource = dicTipoDoc;

            Dictionary<int, string> dicEstado = new Dictionary<int, string>();
            dicEstado.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvNuevo));
            dicEstado.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvCosto0));
            dicEstado.Add(3, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvEstado2));
            dicEstado.Add(4, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvEstado3));
            dicEstado.Add(5, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvEstado4));
            this.cmbEstado.EditValue = 1;
            this.cmbEstado.Properties.DataSource = dicEstado;

            Dictionary<int, string> dicTipoMda = new Dictionary<int, string>();
            dicTipoMda.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_MdaOrigenLocal));
            dicTipoMda.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_MdaOrigenExtr));
            this.cmbMdaOrigen.EditValue = 1;
            this.cmbMdaOrigen.Properties.DataSource = dicTipoMda;
            this.masterPrefijo.Focus();

            this._tasaCambio = this.LoadTasaCambio(this.dtFecha.DateTime);
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this._dataDeterioro.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this._dataDeterioro.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();
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
                this.isValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "RowIndexChanged"));
            }
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected Object LoadTempHeader()
        {
            DTO_inMovimientoDocu headerDeterioro = new DTO_inMovimientoDocu();
            headerDeterioro.EmpresaID.Value = this.empresaID;
            headerDeterioro.NumeroDoc.Value = 0;
            headerDeterioro.MvtoTipoInvID.Value = this.cmbTipoDoc.EditValue.Equals(1) ? this.tipoMovDet : this.tipoMovReval ;
            headerDeterioro.DatoAdd1.Value = "Grupo Costeo: " + this.masterCosteoGrupo.Value;  

            this._docCtrlDeterioro.NumeroDoc.Value = 0;
            this._docCtrlDeterioro.Fecha.Value = DateTime.Now;
            this._docCtrlDeterioro.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this._docCtrlDeterioro.ProyectoID.Value = this.defProyecto;
            this._docCtrlDeterioro.CentroCostoID.Value = this.defCentroCosto;
            this._docCtrlDeterioro.LugarGeograficoID.Value = this.defLugarGeo;
            this._docCtrlDeterioro.PrefijoID.Value = this.txtPrefix.Text;
            this._docCtrlDeterioro.TerceroID.Value = this.defTercero;
            this._docCtrlDeterioro.TasaCambioCONT.Value = this._tasaCambio;
            this._docCtrlDeterioro.TasaCambioDOCU.Value = this._tasaCambio;
            this._docCtrlDeterioro.DocumentoID.Value = this.cmbTipoDoc.EditValue.Equals(1)? AppDocuments.DeterioroInv : AppDocuments.RevalorizacionInv;
            this._docCtrlDeterioro.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
            this._docCtrlDeterioro.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._docCtrlDeterioro.seUsuarioID.Value = this.userID;
            this._docCtrlDeterioro.MonedaID.Value = this.monedaLocal;
            this._docCtrlDeterioro.AreaFuncionalID.Value = this.areaFuncionalID;
            this._docCtrlDeterioro.ConsSaldo.Value = 0;
            this._docCtrlDeterioro.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
            this._docCtrlDeterioro.FechaDoc.Value = this.dtFecha.DateTime;
            this._docCtrlDeterioro.Descripcion.Value = base.txtDocDesc.Text;
            this._docCtrlDeterioro.Valor.Value = 0;
            this._docCtrlDeterioro.Iva.Value = 0;           

            DTO_MvtoInventarios mvto = new DTO_MvtoInventarios();
            mvto.Header = headerDeterioro;
            mvto.DocCtrl = this._docCtrlDeterioro;
            mvto.Footer = new List<DTO_inMovimientoFooter>();

            return mvto;
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
                #region Validacion de Fks
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                #region inReferenciaID
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "inReferenciaID", false, true, false, AppMasters.inReferencia);
                if (!validField)
                    validRow = false;
                #endregion                
                #endregion
                #region Validaciones de valores
                if (this.cmbMdaOrigen.EditValue.Equals(1))
                {
                    validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "NuevoValorUniLOC", false, false, true, false);
                    if (!validField)
                        validRow = false;
                }
                else
                {
                    validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "NuevoValorUniEXT", false, false, true, false);
                    if (!validField)
                        validRow = false;
                }
                #endregion

                if (validRow)
                {
                    this.isValid = true;
                    if (!this.newReg)
                        this.UpdateTemp(this._dataDeterioro);
                }
                else
                    this.isValid = false;

                this.hasChanges = true;
                return validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_inMovimientoFooter footerDet = new DTO_inMovimientoFooter();
            try
            {
                #region Asigna datos a la fila

                footerDet.Movimiento.Index = this._dataDeterioro.Footer.Count > 0 ? this._dataDeterioro.Footer.Last().Movimiento.Index + 1 : 0;
                footerDet.Movimiento.EmpresaID.Value = this.empresaID;
                footerDet.Movimiento.TerceroID.Value = string.Empty;
                footerDet.Movimiento.BodegaID.Value = string.Empty;
                footerDet.Movimiento.inReferenciaID.Value = string.Empty;
                footerDet.Movimiento.DocSoporteTER.Value = string.Empty;
                footerDet.Movimiento.DescripTExt.Value = string.Empty;     
                footerDet.Movimiento.CantidadDoc.Value = 0;
                footerDet.Movimiento.CantidadEMP.Value = 0;
                footerDet.Movimiento.CantidadUNI.Value = 0;  
                footerDet.Movimiento.ValorUNI.Value = 0;
                footerDet.Movimiento.DocSoporte.Value = 0;
                footerDet.Movimiento.ValorActualUniLOC.Value = 0;
                footerDet.Movimiento.ValorActualUniEXT.Value = 0;
                footerDet.Movimiento.NuevoValorUniLOC.Value = 0;
                footerDet.Movimiento.NuevoValorUniEXT.Value = 0;
                footerDet.Movimiento.ValorAjusteUniLOC.Value = 0;
                footerDet.Movimiento.ValorAjusteUniEXT.Value = 0;
                footerDet.Movimiento.Valor1LOC.Value = 0; //Ajuste TotalLOC
                footerDet.Movimiento.Valor1EXT.Value = 0; //Ajuste TotalEXT
                footerDet.Movimiento.Valor2LOC.Value = 0;
                footerDet.Movimiento.Valor2EXT.Value = 0;
                footerDet.Movimiento.ProyectoID.Value = this.defProyecto;
                footerDet.Movimiento.CentroCostoID.Value = this.defCentroCosto;
                footerDet.Movimiento.EstadoInv.Value = Convert.ToByte(this.cmbEstado.EditValue);
                footerDet.Movimiento.MvtoTipoInvID.Value = this.cmbTipoDoc.EditValue.Equals(1) ? this.tipoMovDet : this.tipoMovReval;
                footerDet.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                #endregion

                this._dataDeterioro.Footer.Add(footerDet);
                this.gvDocument.RefreshData();
                this.gcDocument.RefreshDataSource();
                try { this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1; }  catch (Exception) { ;}
                this.isValid = false;
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "AddNewRow: " + ex.Message));
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
           
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemSave.Visible = false;

            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
            FormProvider.Master.itemGenerateTemplate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate); 
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida el tipo de moneda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbMdaOrigen_EditValueChanged(object sender, EventArgs e)
        {
            if (this.cmbMdaOrigen.EditValue.Equals(1))
            {
                this.gvDocument.Columns[this.unboundPrefix + "ValorActualUniEXT"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "NuevoValorUniEXT"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "ValorAjusteUniEXT"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "Valor1EXT"].Visible = false;            
                this.gvDocument.Columns[this.unboundPrefix + "ValorActualUniLOC"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorActualUniLOC"].VisibleIndex = 4;
                this.gvDocument.Columns[this.unboundPrefix + "NuevoValorUniLOC"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "NuevoValorUniLOC"].VisibleIndex = 5;
                this.gvDocument.Columns[this.unboundPrefix + "ValorAjusteUniLOC"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorAjusteUniLOC"].VisibleIndex = 6;
                this.gvDocument.Columns[this.unboundPrefix + "Valor1LOC"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "Valor1LOC"].VisibleIndex = 7;

            }
            else
            {
                this.gvDocument.Columns[this.unboundPrefix + "ValorActualUniLOC"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "NuevoValorUniLOC"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "ValorAjusteUniLOC"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "Valor1LOC"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "ValorActualUniEXT"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorActualUniEXT"].VisibleIndex = 4;
                this.gvDocument.Columns[this.unboundPrefix + "NuevoValorUniEXT"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "NuevoValorUniEXT"].VisibleIndex = 5;
                this.gvDocument.Columns[this.unboundPrefix + "ValorAjusteUniEXT"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorAjusteUniEXT"].VisibleIndex = 6;
                this.gvDocument.Columns[this.unboundPrefix + "Valor1EXT"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "Valor1EXT"].VisibleIndex = 7;
            }
            this.gcDocument.RefreshDataSource();
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Valida el tipo de moneda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbTipoDoc_EditValueChanged(object sender, EventArgs e)
        {
            if (this._dataDeterioro != null)
            {
                foreach (var item in this._dataDeterioro.Footer)
                {
                    item.Movimiento.NuevoValorUniLOC.Value = 0;
                    item.Movimiento.NuevoValorUniEXT.Value = 0;
                    item.Movimiento.ValorAjusteUniLOC.Value = 0;
                    item.Movimiento.ValorAjusteUniEXT.Value = 0;
                    item.Movimiento.Valor1LOC.Value = 0;
                    item.Movimiento.Valor1EXT.Value = 0;
                }
                this._docCtrlDeterioro.DocumentoID.Value = this.cmbTipoDoc.EditValue.Equals(1) ? AppDocuments.DeterioroInv : AppDocuments.RevalorizacionInv;
                this.gcDocument.RefreshDataSource();
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
            docs.Add(AppDocuments.DeterioroInv);
            docs.Add(AppDocuments.RevalorizacionInv);        
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                this._docCtrlDeterioro = getDocControl.DocumentoControl;
                this.GetMvtoInventario();
                this.txtNro.Text = this._docCtrlDeterioro.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = this._docCtrlDeterioro.PrefijoID.Value;
            }
        }

        #endregion

        #region Eventos Grilla (Detalle Mvto)

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateHeader())
                    this.validHeader = true;
                else
                    this.validHeader = false;
                #region Si entra al detalle y no tiene datos
              
                try
                {
                    if (this.validHeader &&  this._dataDeterioro.Footer.Count == 0)
                    {
                        this._dataDeterioro = (DTO_MvtoInventarios)this.LoadTempHeader();
                        this.UpdateTemp(this._dataDeterioro);
                        this.EnableHeader(false);
                        this.LoadData(true);
                        this.validHeader = true;
                        this.ValidHeaderTB();
                        this.newDoc = false;
                    }                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion                
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "DeterioroRevalorizacion.cs-gcDocument_Enter: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
                            DTO_inMovimientoFooter dtoM = (DTO_inMovimientoFooter)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                        e.Value = fi.GetValue(dtoM.Movimiento);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                                }
                            }
                        }
                    }
                }
            }

            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
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
                            DTO_inMovimientoFooter dtoM = (DTO_inMovimientoFooter)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
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
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            try
            {
                if (fieldName == "inReferenciaID")
                {
                    this._costosMvto = new DTO_inCostosExistencias();
                    DTO_inControlSaldosCostos saldos = new DTO_inControlSaldosCostos();
                    DTO_inMovimientoFooter footer = new DTO_inMovimientoFooter();
                    saldos.Periodo.Value = this.dtPeriod.DateTime;
                    saldos.EstadoInv.Value = Convert.ToByte(this.cmbEstado.EditValue);
                    saldos.CosteoGrupoInvID.Value = this.masterCosteoGrupo.Value;
                    saldos.inReferenciaID.Value = e.Value.ToString();
                    var rrr = this._bc.AdministrationModel.inControlSaldosCostos_GetByParameter(documentID, saldos);
                    decimal saldoDispInit = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, saldos, ref this._costosMvto);
                    if (saldoDispInit != 0)
                    {
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value = saldoDispInit;
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorActualUniLOC.Value = saldoDispInit != 0 ? (this._costosMvto.CtoLocSaldoIni.Value + this._costosMvto.CtoLocEntrada.Value - this._costosMvto.CtoLocSalida.Value) / saldoDispInit : 0;
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorActualUniEXT.Value = saldoDispInit != 0 ? (this._costosMvto.CtoExtSaldoIni.Value + this._costosMvto.CtoExtEntrada.Value - this._costosMvto.CtoExtSalida.Value) / saldoDispInit : 0;
                        footer.Movimiento = this._dataDeterioro.Footer[e.RowHandle].Movimiento;
                    }
                    else
                    {
                        MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_CantityAvailable), 0));
                        this.validHeader = false;
                        return;
                    }
                }

                #region Nuevo valor ML
                if (fieldName == "NuevoValorUniLOC")
                {
                    if (this.cmbTipoDoc.EditValue.Equals(1))
                    {
                        decimal ajusteML = this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorActualUniLOC.Value.Value - Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        if (ajusteML > 0)
                        {
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniLOC.Value = ajusteML;
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1LOC.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * ajusteML;
                            if (this._tasaCambio != 0)
                            {
                                this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniEXT.Value = ajusteML / this._tasaCambio;
                                this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1EXT.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniEXT.Value;
                            }
                        }
                        else
                            this.gvDocument.SetColumnError(e.Column, "El valor del ajuste no puede ser negativo");
                    }
                    else
                    {
                        decimal ajusteML = this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorActualUniLOC.Value.Value + Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniLOC.Value = ajusteML;
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1LOC.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * ajusteML;
                        if (this._tasaCambio != 0)
                        {
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniEXT.Value = ajusteML / this._tasaCambio;
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1EXT.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniEXT.Value;
                        }
                    }

                }
                #endregion
                #region Nuevo valor ME
                if (fieldName == "NuevoValorUniEXT")
                {
                    if (this.cmbTipoDoc.EditValue.Equals(1))
                    {
                        decimal ajusteME = this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorActualUniEXT.Value.Value - Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        if (ajusteME > 0)
                        {
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniEXT.Value = ajusteME;
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1EXT.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * ajusteME;
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniLOC.Value = ajusteME * this._tasaCambio;
                            this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1LOC.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniLOC.Value;
                        }
                        else
                        {
                            this.gvDocument.SetColumnError(e.Column, "El valor del ajuste no puede ser negativo");
                        }
                    }
                    else
                    {
                        decimal ajusteME = this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorActualUniEXT.Value.Value + Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniEXT.Value = ajusteME;
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1EXT.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * ajusteME;
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniLOC.Value = ajusteME * this._tasaCambio;
                        this._dataDeterioro.Footer[e.RowHandle].Movimiento.Valor1LOC.Value = this._dataDeterioro.Footer[e.RowHandle].Movimiento.CantidadUNI.Value * this._dataDeterioro.Footer[e.RowHandle].Movimiento.ValorAjusteUniLOC.Value;
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "DeterioroRevalorizacion.cs-gvDocument_CellValueChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Revisa botones al digitar algo sobre la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.lastColName) && this.gvDocument.IsLastRow && this.gvDocument.FocusedColumn.FieldName == this.lastColName && e.KeyCode == Keys.Tab)
            {
                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                if (isV)
                {
                    this.newReg = true;
                    this.AddNewRow();
                }
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
                if (this._dataDeterioro == null)
                {
                    this.gcDocument.Focus();
                    e.Handled = true;
                    return;
                }

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

                        if (this._dataDeterioro.Footer.Count == 1)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                            e.Handled = true;
                        }
                        else
                        {
                            this._dataDeterioro.Footer.RemoveAll(x => x.Movimiento.Index == this.indexFila);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this._dataDeterioro);

                            this.gvDocument.RefreshData();
                            this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                        }
                    }
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!this.disableValidate && this._dataDeterioro.Footer.Count > 0)
            {
                bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                this.deleteOP = false;

                if (validRow)
                    this.isValid = true;                  
                else
                {
                    e.Allow = false;
                    this.isValid = false;
                }
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();
                if (this.cleanDoc)
                    this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "TBNew: " + ex.Message));

            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion
        /// </summary>
        public override void TBSendtoAppr()
        {
            this.gvDocument.PostEditor();
            this.gvDocument.Focus();
            this.gvDocument.ActiveFilterString = string.Empty;
            if (this.ValidateHeader() && this._dataDeterioro.Footer.Count > 0)
            {
                Thread process = new Thread(this.SendToApproveThread);
                process.Start();
            }
          
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                int numeroDoc = 0;
                bool update = false;
                if (this._dataDeterioro.DocCtrl.NumeroDoc.Value.Value != 0)
                {
                    update = true;
                    numeroDoc = this._dataDeterioro.DocCtrl.NumeroDoc.Value.Value;
                }
                foreach (var item in this._dataDeterioro.Footer)
                    item.Movimiento.CantidadUNI.Value = 0;
                DTO_SerializedObject obj = this._bc.AdministrationModel.Deterioro_Add(this.cmbTipoDoc.EditValue.Equals(1)? AppDocuments.DeterioroInv: AppDocuments.RevalorizacionInv, this._dataDeterioro, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                    this._dataDeterioro = new DTO_MvtoInventarios();
                    this._docCtrlDeterioro = new DTO_glDocumentoControl();
                    this._costosMvto = new DTO_inCostosExistencias();
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DeterioroRevalorizacion.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
