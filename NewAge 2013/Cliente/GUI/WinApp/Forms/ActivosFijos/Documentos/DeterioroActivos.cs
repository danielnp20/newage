using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using System.Threading;
using SentenceTransformer;
using NewAge.Forms.Dialogs.Documentos;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DeterioroActivos : DocumentForm 
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private string _locFisicaRsx;
        private int NumFila = 1;
        private List<DTO_acActivoControl> _lActivos = new List<DTO_acActivoControl>();
        private string mdaLocal;
        private string mdaExt;
        private decimal tasaCambio = 0;
        private string conceptoSaldo = string.Empty;
        private int index = 0;
        private string tipoBalFuncional = string.Empty;
        private string tipoBalIFRS = string.Empty;

        private string _monedaOrigen;
        private bool validMove = false;
        private DTO_coDocumentoRevelacion revelacion;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        public void InitControls()
        {
            //Trae tasa de Cambio del día
            this.txtTasaCambio.EditValue = tasaCambio;

            #region Filtros Tipo de Movimientos (Deterioros)

            List<DTO_glConsultaFiltro> filtrosComplejos = new List<DTO_glConsultaFiltro>();

            DTO_glConsultaFiltro eg = new DTO_glConsultaFiltro();
            eg.CampoFisico = "TipoMvto";
            eg.OperadorFiltro = OperadorFiltro.Igual;
            eg.ValorFiltro = "10"; //Corresponde al tipo de movientos (10) Deterioros/Revalorizaciones
            eg.OperadorSentencia = "AND";

            filtrosComplejos.Add(eg);

            #endregion

            this._bc.InitMasterUC(this.uc_TipoMovimiento, AppMasters.acMovimientoTipo, true, true, true, false, filtrosComplejos); 
          
            //Inicializa Combo Tipo Balance
            Dictionary<string, string> tipoBalance = new Dictionary<string, string>();
            tipoBalance.Add(this.tipoBalFuncional, TipoBalance.Fiscal.ToString());
            tipoBalance.Add(this.tipoBalIFRS, TipoBalance.IFRS.ToString());
            this.lkpTipoBalance.Properties.DataSource = tipoBalance;
            this.lkpTipoBalance.EditValue = this.tipoBalIFRS;

            Dictionary<string, string> tipoDepreciacion = new Dictionary<string, string>();
            tipoDepreciacion.Add(((int)TipoDepreciacion.LineaRecta).ToString(), TipoDepreciacion.LineaRecta.ToString());
            tipoDepreciacion.Add(((int)TipoDepreciacion.SaldosDecrecientes).ToString(), TipoDepreciacion.SaldosDecrecientes.ToString());
            tipoDepreciacion.Add(((int)TipoDepreciacion.UnidadesDeProduccion).ToString(), TipoDepreciacion.UnidadesDeProduccion.ToString());

            this.lkpTipDeprFISCAL.Properties.DataSource = tipoDepreciacion;
            this.lkpTipDeprIFRS.Properties.DataSource = tipoDepreciacion;
            this.lkpTipDeprGAP.Properties.DataSource = tipoDepreciacion;
        }

        /// <summary>
        /// Carga la información de un activo
        /// </summary>
        /// <param name="index"></param>
        public void LoadInfo(int index)
        {
            if (this._lActivos != null && this._lActivos.Count > 0)
            {
                this.txtVUtilFiscal.EditValue = this._lActivos[index].VidaUtilLOC.Value;
                this.txtVUtilIFRS.EditValue = this._lActivos[index].VidaUtilIFRS.Value;
                this.txtVUtilGAP.EditValue = this._lActivos[index].VidaUtilUSG.Value;

                this.txtValorSalvamentoFISCAL.EditValue = this._lActivos[index].ValorSalvamentoLOC.Value;
                this.txtValorSalvamentoIFRS.EditValue = this._lActivos[index].ValorSalvamentoIFRS.Value;
                this.txtValorSalvamentoGAP.EditValue = this._lActivos[index].ValorSalvamentoUSG.Value;

                this.lkpTipDeprFISCAL.EditValue = this._lActivos[index].TipoDepreLOC.Value.ToString();
                this.lkpTipDeprIFRS.EditValue = this._lActivos[index].TipoDepreIFRS.Value.ToString();
                this.lkpTipDeprGAP.EditValue = this._lActivos[index].TipoDepreUSG.Value.ToString();

                this.txtVlrActML.EditValue = this._lActivos[index].CostoIniLOC.Value;
                this.txtVlrActME.EditValue = this._lActivos[index].CostoIniEXT.Value;
                this.txtVlrNewML.EditValue = this._lActivos[index].CostoLOC.Value;
                this.txtVlrNewME.EditValue = this._lActivos[index].CostoEXT.Value;
                this.txtVlrDifML.EditValue = Convert.ToDecimal(this.txtVlrNewML.EditValue, CultureInfo.InvariantCulture) - Convert.ToDecimal(this.txtVlrActML.EditValue, CultureInfo.InvariantCulture);
                this.txtVlrDifME.EditValue = Convert.ToDecimal(this.txtVlrNewME.EditValue, CultureInfo.InvariantCulture) - Convert.ToDecimal(this.txtVlrActME.EditValue, CultureInfo.InvariantCulture);

                //Carga el valor de la diferecia con respecto al valor del activo
                this._lActivos[index].CostoDiferencia.Value = Convert.ToDecimal(this.txtVlrDifML.EditValue, CultureInfo.InvariantCulture);

                //Determina si es una revalorizacion , un deterioro o no hubo cambios en el costo del activo
                if (Convert.ToDecimal(this.txtVlrDifML.EditValue, CultureInfo.InvariantCulture) < 0)
                {
                    this.chkDeterioro.Checked = true;
                    this.chkRevalorizacion.Checked = false;
                }
                else if (Convert.ToDecimal(this.txtVlrDifML.EditValue, CultureInfo.InvariantCulture) == 0)
                {
                    this.chkDeterioro.Checked = false;
                    this.chkRevalorizacion.Checked = false;
                }
                else if (Convert.ToDecimal(this.txtVlrDifML.EditValue, CultureInfo.InvariantCulture) > 0)
                {
                    this.chkDeterioro.Checked = false;
                    this.chkRevalorizacion.Checked = true;
                }
            }
        }

        /// <summary>
        /// Iniciliza controles de la pantalla 
        /// </summary>
        public void RefreshData()
        {
            this._lActivos.Clear();
            this.txtVUtilFiscal.EditValue = string.Empty;
            this.txtVUtilIFRS.EditValue = string.Empty;
            this.txtVUtilGAP.EditValue = string.Empty;
            this.txtValorSalvamentoFISCAL.EditValue = string.Empty;
            this.txtValorSalvamentoIFRS.EditValue = string.Empty;
            this.txtValorSalvamentoGAP.EditValue = string.Empty;
            this.lkpTipDeprFISCAL.EditValue = string.Empty;
            this.lkpTipDeprIFRS.EditValue = string.Empty;
            this.lkpTipDeprGAP.EditValue = string.Empty;
            this.txtVlrActML.EditValue = string.Empty;
            this.txtVlrActME.EditValue = string.Empty;
            this.txtVlrNewML.EditValue = string.Empty;
            this.txtVlrNewME.EditValue = string.Empty;
            this.txtVlrDifML.EditValue = string.Empty;
            this.txtVlrDifME.EditValue = string.Empty;
            this.chkDeterioro.Checked = false;
            this.chkRevalorizacion.Checked = false;
            this.LoadData(true);
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los parametros del la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.DeterioroActivos;
            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            base.SetInitParameters();            
            this.AddGridCols();
             
            //Carga info del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            
            //Asignación mondedas local y extragera
            this.mdaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.mdaExt = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            this.tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(this.mdaExt, currentDate);
            this.tipoBalFuncional = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            this.tipoBalIFRS = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
            
            this.frmModule = ModulesPrefix.ac;

            this.format = _bc.GetImportExportFormat(typeof(DTO_DeterioroActivo), this.documentID);
            this._locFisicaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_locFisicaID");

            string componenteCosto = this._bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
            if (!string.IsNullOrEmpty(componenteCosto))
            {
                DTO_acComponenteActivo componenteActivo = (DTO_acComponenteActivo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acComponenteActivo, false, componenteCosto, true);
                this.conceptoSaldo = componenteActivo.ConceptoSaldoID.Value;
            }
            this.InitControls();
        }

        /// <summary>
        /// Inicialia las columnas de la grilla de Detalle
        /// </summary>
        protected override void AddGridCols()
        {
             try
            {

                GridColumn activoID = new GridColumn();
                activoID.FieldName = this.unboundPrefix + "ActivoID";
                activoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoID");
                activoID.UnboundType = UnboundColumnType.Integer;
                activoID.VisibleIndex = 1;
                activoID.Width = 30;
                activoID.Visible = false;
                activoID.OptionsColumn.AllowEdit = false;
                activoID.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(activoID);

                //Plaqueta
                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this.unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 4;
                plaquetaID.Width = 100;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                plaquetaID.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 5;
                serialID.Width = 100;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                serialID.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(serialID);

                
                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 15;
                inReferenciaID.Width = 100;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descriptivo
                GridColumn desriptivo = new GridColumn();
                desriptivo.FieldName = this.unboundPrefix + "Observacion";
                desriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desriptivo.UnboundType = UnboundColumnType.String;
                desriptivo.VisibleIndex = 15;
                desriptivo.Width = 200;
                desriptivo.Visible = true;
                desriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desriptivo);

                //proyectoID
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 31;
                locFisica.Width = 100;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(locFisica);

                //proyectoID
                GridColumn proyectoID = new GridColumn();
                proyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                proyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyectoID.UnboundType = UnboundColumnType.String;
                proyectoID.VisibleIndex = 31;
                proyectoID.Width = 150;
                proyectoID.Visible = true;
                proyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(proyectoID);

                //centroCostoID
                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 31;
                centroCostoID.Width = 150;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(centroCostoID);                

                //Localizacion Fisica
                GridColumn costoLOC = new GridColumn();
                costoLOC.FieldName = this.unboundPrefix + "CostoLOC";
                costoLOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLOC");
                costoLOC.UnboundType = UnboundColumnType.Decimal;
                costoLOC.VisibleIndex = 33;
                costoLOC.Width = 200;
                costoLOC.Visible = true;
                costoLOC.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(costoLOC);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeterioroActivos", "AddGridColumns"));
            }
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Maestras
            if (colName == this._locFisicaRsx)
                return AppMasters.glLocFisica;

            return 0;
        }

        /// <summary>
        /// Carga la informacion
        /// </summary>
        /// <param name="firstTime"></param>
        protected override void LoadData(bool firstTime)
        {
            base.LoadData(firstTime);
            if (firstTime)
            {
                this.gcDocument.DataSource = this._lActivos;
                this.gcDocument.RefreshDataSource();
                this.gvDocument.PostEditor();
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Determina el menu a mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);

                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemImport.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = true;

                FormProvider.Master.itemImport.Enabled = true;
                FormProvider.Master.itemGenerateTemplate.Enabled = false;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.itemSave.Enabled = true;
                FormProvider.Master.itemGenerateTemplate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeterioroActivos", "Form_Enter"));
            }

        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Importa la información desde un archivo Excel
        /// </summary>
        public override void TBImport()
        {
            //Revisa que cumple las condiciones
            if (!this.ReplaceDocument())
                return;

            this.gvDocument.ActiveFilterString = string.Empty;                     

            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        /// <summary>
        ///  Limpia la Grilla de Detalle
        /// </summary>
        public override void TBNew()
        {
            base.TBNew();
            this.RefreshData();
        }

        /// <summary>
        /// Guarda el Deterioro de los activos
        /// </summary>
        public override void TBSave()
        {    
            base.TBSave();
            this.gvDocument.PostEditor();
            try
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Genera Plantilla de carga de Datos en Excel
        /// </summary>
        public override void TBGenerateTemplate()
        {
            base.TBGenerateTemplate();
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Da formato a los campos de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            base.gvDocument_CustomRowCellEdit(sender, e);
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "inReferenciaID" || fieldName == "ProyectoID" || fieldName == "CentroCostoID" || fieldName == "LocFisicaID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "CostoLOC")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Se ejectuta al cambiar un registro de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            base.gvDocument_FocusedRowChanged(sender, e);
            if (this._lActivos != null && this._lActivos.Count > 0)
            {
                this.index = e.FocusedRowHandle;
                LoadInfo(this.index);
            }
        } 

        #endregion

        #region Eventos Header

        /// <summary>
        /// Tasa de cambio 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtTasaCambio_Leave(object sender, System.EventArgs e)
        {
            this.tasaCambio = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            if (Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture) > 0)
                this.tasaCambio = 1 / Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            else
                this.tasaCambio = 0;

            this.LoadInfo(index);
        }

        /// <summary>
        /// Verifica el tipo de movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_TipoMovimiento_Leave(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.uc_TipoMovimiento.Value))
                return;

            string mvtoTipo = this.uc_TipoMovimiento.Value;
            DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, false, mvtoTipo, true);
            DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, MvtoTipo.coDocumentoID.Value, true);

            this._monedaOrigen = coDoc.MonedaOrigen.ToString();

            if (coDoc.DocumentoID.Value != this.documentID.ToString())
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                this.uc_TipoMovimiento.Value = string.Empty;
                this.validMove = false;
            }
            else
                this.validMove = true;
        }

        /// <summary>
        /// Incluye la revelación en el sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevelaciones_Click(object sender, System.EventArgs e)
        {
            DTO_coDocumento coDoc = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this.documentID.ToString(), true);
            ModalRevelaciones modalRev = new ModalRevelaciones(coDoc.NotaRevelacionID.Value);
            modalRev.ShowDialog();
            revelacion = modalRev.DocRevelacion;
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Carga la información proveniente del archivo excel
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
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgCtaCargoProy = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                    string msgCtaPeriodClosed = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CtaPeriodClosed);
                    //Popiedades de un comprobante
                    DTO_acActivoControl det = new DTO_acActivoControl();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_acActivoControl).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
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

                    //Fks
                    fks.Add(this._locFisicaRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        //Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
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
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];

                                    //Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        if (colRsx ==this._locFisicaRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

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
                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
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
                                    }
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                det = new DTO_acActivoControl();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&
                                                (colRsx == this._locFisicaRsx ||
                                                colName == "SerialID" ||
                                                colName == "PlaquetaID" 
                                              )
                                        )
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);
                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        PropertyInfo pi = det.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(det, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

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
                                            //Si paso las validaciones asigne el valor al DTO
                                            if (createDTO)
                                            {
                                                udt.SetValueFromString(colVal);
                                            }
                                        }

                                        #region Otros Formatos

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

                                        } //validacion si no es null
                                        #endregion

                                        #endregion
                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                        {
                                            udt.SetValueFromString(colValue);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "DeterioroActivos.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                            {
                                DTO_acActivoControl activo = this._bc.AdministrationModel.acActivoControl_GetFilters(det.SerialID.Value,det.PlaquetaID.Value, det.LocFisicaID.Value, string.Empty,
                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, 1, 
                                    this._bc.AdministrationModel.acActivoControl_GetFiltersCount(det.SerialID.Value, det.PlaquetaID.Value, det.LocFisicaID.Value, string.Empty,
                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false
                                    )
                                    ).FirstOrDefault();

                                if (Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture) > 0)
                                {
                                    activo.CostoLOC.Value = det.CostoLOC.Value;
                                    activo.CostoEXT.Value = det.CostoLOC.Value * 1 / Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

                                    activo.CostoIniLOC.Value = this._bc.AdministrationModel.acActivoControl_GetSaldoCompraActivo(this.conceptoSaldo, activo.ActivoID.Value.Value, this.lkpTipoBalance.EditValue.ToString()).Sum(x => x.SaldoMLoc.Value).Value;
                                    activo.CostoIniEXT.Value = this._bc.AdministrationModel.acActivoControl_GetSaldoCompraActivo(this.conceptoSaldo, activo.ActivoID.Value.Value, this.lkpTipoBalance.EditValue.ToString()).Sum(x => x.SaldoMExt.Value).Value;

                                    activo.CostoDiferencia.Value = activo.CostoLOC.Value - activo.CostoIniLOC.Value;
                                    
                                    if (!this._lActivos.Any(x => x.SerialID.Value == det.SerialID.Value ||
                                                            x.PlaquetaID.Value == det.PlaquetaID.Value ||
                                                            x.LocFisicaID.Value == det.LocFisicaID.Value ||
                                                            x.inReferenciaID.Value == det.inReferenciaID.Value
                                                           )

                                       )
                                    {
                                        this._lActivos.Add(activo);
                                    }
                                }
                                else
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Ac_NoAssignedTasaCamabio;
                                    validList = false;
                                }


                                
                            }
                            else
                                validList = false;
                        }
                    }
                    #endregion
                    #region Valida las restricciones particulares del comprobante
                    if (validList)
                    {
                        result.Details = new List<DTO_TxResultDetail>();

                        int index = this.NumFila;
                        int i = 0;
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        percent = 0;
                        foreach (DTO_acActivoControl dto in this._lActivos)
                        {
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (this._lActivos.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }

                            i++;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Validaciones particulares del documento al importar del DTO
                            //this.ValidateDataImport(dto, cta, rd, msgCero, msgVals);
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
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.Invoke(this.refreshGridDelegate);                         
                        }
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
                MessageBox.Show(e.Message);
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }

        }

        /// <summary>
        /// Refresca la grilla de contenido
        /// </summary>
        protected override void RefreshGridMethod()
        {
            base.RefreshGridMethod();
            this.gcDocument.DataSource = this._lActivos;
            if (this._lActivos != null && this._lActivos.Count > 0)
            {
                LoadInfo(0);
            }
        }

        /// <summary>
        /// Genera Deterioro o Revalorizacion
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                if (this._lActivos.Count == 0)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoActivosList));
                    return;
                }
                if (this.revelacion != null)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoRevelaciones));
                    return;               
                }
                if (!this.validMove)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                    return;   
                }

                this.gvDocument.PostEditor();
                
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                bool deterioroInd = false;
                if (this.chkDeterioro.Checked)
                    deterioroInd = true;

                List<DTO_TxResult> results = _bc.AdministrationModel.acActivoControl_Deterioro(this.documentID, this._actFlujo.ID.Value, lkpTipoBalance.EditValue.ToString(), this.txtPrefix.Text, deterioroInd, this.uc_TipoMovimiento.Value, this.revelacion, this._lActivos);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                
                foreach (DTO_TxResult result in results)
                {
                    if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                        resultsNOK.Add(result);
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (resultsNOK.Count == 0)
                {
                    this._lActivos = new List<DTO_acActivoControl>();
                    this.Invoke(this.saveDelegate);
                    this.Invoke(this.refreshGridDelegate);
                    this.RefreshData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

      
    }
}
