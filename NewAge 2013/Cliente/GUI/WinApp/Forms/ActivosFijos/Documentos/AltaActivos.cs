using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class AltaActivos : DocumentForm
    {       
        #region Delegados
        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void RefreshGrid();
        public RefreshGrid refresGrd;
        public void RefreshGridMethod()
        {
            this._noLoaded = true;
            FormProvider.Master.itemSave.Enabled = false;
            this.LoadData(false);
            this.EnableFooter(false);
            this.EnableHeader(false);
            this.CleanHeader();
            this.CleanFooter();
            this.select = new List<int>();
        }
        #endregion

        #region Variables Formulario

        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _glDocumentoControl = null;
        private string _documento = string.Empty;
        private string _monedaOrigen;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private List<DTO_acActivoControl > _lActivos;
        private bool selected = false;
        private bool validMove = true;
        private bool _validRow = false;
        private bool _noLoaded = false;
        private bool _modCxP = true;
        private bool _modProveedores = true;
        private decimal vlrFactura = 0;
        private string centroCosto;
        private string proyecto;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            try
            {
                this.gcDocument.DataSource = this._lActivos;
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AltaActivos.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AltaActivos;
            InitializeComponent();

            base.SetInitParameters();

            this.InitControls();
            this.AddGridCols();

            //Inicializa los delegados
            this.refresGrd = new RefreshGrid(RefreshGridMethod);

            //Carga info del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.ac;

            //Carga info de las monedas
            this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                base.AddGridCols();

                //Plaqueta
                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this.unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 1;
                plaquetaID.Width = 110;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 80;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(serialID);

                //Valor Pesos
                GridColumn valorP = new GridColumn();
                valorP.FieldName = this.unboundPrefix + "CostoLOC";
                valorP.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLOC");
                valorP.UnboundType = UnboundColumnType.Decimal;
                valorP.VisibleIndex = 3;
                valorP.Width = 90;
                valorP.Visible = true;
                valorP.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorP);

                //Valor Dolares
                GridColumn valorD = new GridColumn();
                valorD.FieldName = this.unboundPrefix + "CostoEXT";
                valorD.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoEXT");
                valorD.UnboundType = UnboundColumnType.Decimal;
                valorD.VisibleIndex = 4;
                valorD.Width = 90;
                valorD.Visible = true;
                valorD.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorD);

                //Turnos
                GridColumn turnos = new GridColumn();
                turnos.FieldName = this.unboundPrefix + "Turnos";
                turnos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Turnos");
                turnos.UnboundType = UnboundColumnType.Integer;
                turnos.VisibleIndex = 5;
                turnos.Width = 90;
                turnos.Visible = true;
                turnos.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(turnos);

                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 6;
                inReferenciaID.Width = 100;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 7;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = true;
                descripcion.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(descripcion);
                

                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this.unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 8;
                clase.Width = 90;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 9;
                ActivoTipoID.Width = 90;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this.unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 10;
                grupo.Width = 90;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(grupo);

                //Modelo
                GridColumn modelo = new GridColumn();
                modelo.FieldName = this.unboundPrefix + "Modelo";
                modelo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Modelo");
                modelo.UnboundType = UnboundColumnType.String;
                modelo.VisibleIndex = 11;
                modelo.Width = 80;
                modelo.Visible = true;
                modelo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(modelo);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 12;
                locFisica.Width = 90;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(locFisica);  
              

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AltaActivos.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            #region Inicializa comportamiento del form segun los modulos activos

            //Lista los modulos activos
            var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false);
            if (!modules.Any(x => x.ModuloID.Value == ModulesPrefix.pr.ToString()))
                this._modProveedores = false;
            if (!modules.Any(x => x.ModuloID.Value == ModulesPrefix.cp.ToString()))
                this._modCxP = false;


            //Esta activo el modulo de CxP é inactivo el modulo de Proveedores
            if (!this._modProveedores && this._modCxP)
            {
                //Habilita los botones +- de la grilla
                this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = true;
                this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = true;
                this.gvDocument.Columns[this.unboundPrefix + "CostoLOC"].OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns[this.unboundPrefix + "CostoEXT"].OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"].OptionsColumn.AllowEdit = true; 
            }
            else
                //Estan inactivos los modulos de CxP y Proveedores
                if (!this._modProveedores && !this._modCxP)
                {
                    this.txtDocumento.Enabled = false;
                    this.masterTercero.EnableControl(false);
                    this.txtTCLoc.Enabled = true;
                    
                    //Habilita los botones +- de la grilla
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = true;
                    this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLOC"].OptionsColumn.AllowEdit = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoEXT"].OptionsColumn.AllowEdit = true;
                    this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"].OptionsColumn.AllowEdit = true; 
                    
                }

            #endregion 

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                //this.cmbMonedaOrigen.Enabled = false;
                this.lblTasaCambio.Visible = false;
                this.txtTCLoc.Visible = false;
            }

            this.format = _bc.GetImportExportFormat(typeof(DTO_acActivoControl), this.documentID);
            this._lActivos = new List<DTO_acActivoControl>();            
            #region Carga temporales
            if (this.HasTemporales())
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var aux = _bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (aux != null)
                        {
                            try
                            {
                                this.LoadTempData(aux);
                            }
                            catch (Exception ex1)
                            {
                                //this.validHeader = false;
                                MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                            }
                        }
                        else
                        {
                            //this.validHeader = false;
                            MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                            _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        }
                    }
                    else
                    {
                        //this.validHeader = false;
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManual.cs", "AfterInitialize: " + ex.Message));
                }
            }
            #endregion
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            this._lActivos = (List<DTO_acActivoControl>)aux;
            this.LoadData(true);
            DTO_acActivoControl actrl = this._lActivos.First();
            this._glDocumentoControl = _bc.AdministrationModel.glDocumentoControl_GetByID(actrl.NumeroDoc.Value.Value);

            if (this._glDocumentoControl.Fecha.Value.HasValue &&
                this._glDocumentoControl.Fecha.Value.Value.Year == this.dtPeriod.DateTime.Year &&
                this._glDocumentoControl.Fecha.Value.Value.Month == this.dtPeriod.DateTime.Month)
            {
                List<DTO_acActivoControl> acTemp = this._bc.AdministrationModel.AltaActivos_GetActivosByNumDoc(this._glDocumentoControl.NumeroDoc.Value.Value);
                if (acTemp.Count == this._lActivos.Count)
                {
                    this.txtDocumento.Text = this._glDocumentoControl.DocumentoTercero.Value;
                    this.masterTercero.Value = this._glDocumentoControl.TerceroID.Value;
                    this.dtFecha.DateTime = this._glDocumentoControl.Fecha.Value.Value;

                    this.EnableHeader(false);
                    this.EnableFooter(true);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentChanged));
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                }
            }
            else
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_FacturaFueraPeriodo));
                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
            }
        }

        /// <summary>
        /// Adiciona un nuevo registro desde la grilla de Activos
        /// </summary>
        protected override void AddNewRow()
        {
            string detaultEstado = this._bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_EstadoXDefecto);
            DTO_acActivoControl acCtrl = new DTO_acActivoControl();
            acCtrl.EmpresaID.Value = this.empresaID;
            acCtrl.Tipo.Value = (byte)TipoActivo.Activo;
            acCtrl.DocumentoID.Value = this.documentID;
            acCtrl.Fecha.Value = this.dtFecha.DateTime;
            acCtrl.Periodo.Value = this.dtPeriod.DateTime;
            acCtrl.EstadoInv.Value = (byte)EstadoInv.Activo;
            acCtrl.Propietario.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            acCtrl.EstadoActID.Value = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_EstadoXDefecto);
            acCtrl.MonedaID.Value = this._monedaLocal;
            acCtrl.TerceroID.Value = this.terceroID;
            acCtrl.NumeroDocCompra.Value = Convert.ToInt16(this.txtDocumentoID.Text);
            acCtrl.DocumentoTercero.Value = this.txtDocumento.Text;
            acCtrl.VidaUtilLOC.Value = Convert.ToInt32(this.txtVidaUtilLocal.Text);
            acCtrl.VidaUtilIFRS.Value = Convert.ToInt32(this.txtVidaUtilIFRS.Text);
            acCtrl.VidaUtilUSG.Value = Convert.ToInt32(this.txtVidaUtilUSG.Text);
            acCtrl.TipoDepreLOC.Value = Convert.ToByte((this.cmbTipoDecpreciacionLocal.SelectedItem as ComboBoxItem).Value);
            acCtrl.TipoDepreIFRS.Value = Convert.ToByte((this.cmbTipoDecpreciacionIFRS.SelectedItem as ComboBoxItem).Value);
            acCtrl.TipoDepreUSG.Value = Convert.ToByte((this.cmbTipoDecpreciacionUSG.SelectedItem as ComboBoxItem).Value);
            acCtrl.ValorSalvamentoLOC.Value = Convert.ToDecimal(this.txtValorSalvamentoLocal.EditValue, CultureInfo.InvariantCulture);
            acCtrl.ValorSalvamentoIFRS.Value = Convert.ToDecimal(this.txtValorSalvamentIFRS.EditValue, CultureInfo.InvariantCulture);
            acCtrl.ValorSalvamentoUSG.Value = Convert.ToDecimal(this.txtValorSalvamentUSG.EditValue, CultureInfo.InvariantCulture);
            acCtrl.ValorRetiroIFRS.Value = 0;
            acCtrl.EstadoActID.Value = detaultEstado;
            acCtrl.NumeroDocUltMvto.Value = this.documentID;
            acCtrl.CostoLOC.Value = 0;
            acCtrl.CostoEXT.Value = 0;
            acCtrl.ProyectoID.Value = this.proyecto != null ? this.proyecto : string.Empty;
            acCtrl.CentroCostoID.Value = this.centroCosto != null ? this.centroCosto : string.Empty;
            acCtrl.NumeroDoc.Value = this._modCxP ? this._glDocumentoControl.NumeroDoc.Value : 0;           
            
            this._lActivos.Add(acCtrl);
            this.LoadData(true);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles de la aplicacion
        /// </summary>
        public void InitControls()
        {
            //Habilitar o Deshabilitar Controles
            this.dtFecha.Enabled = false;

            //Controles del header
            #region Filtros Tipo de Movimientos (Compras)

            List<DTO_glConsultaFiltro> filtrosComplejos = new List<DTO_glConsultaFiltro>();

            DTO_glConsultaFiltro eg = new DTO_glConsultaFiltro();
            eg.CampoFisico = "TipoMvto";
            eg.OperadorFiltro = OperadorFiltro.Igual;
            eg.ValorFiltro = "0"; //Corresponde al tipo de movientos (0) Compras
            eg.OperadorSentencia = "AND";

            filtrosComplejos.Add(eg);            
           
            #endregion
            
            _bc.InitMasterUC(this.masterMovimientoTipo, AppMasters.acMovimientoTipo, true, true, true, false, filtrosComplejos);
            this.masterMovimientoTipo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);

            _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.uc_CentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this.uc_Proyecto.Leave += new EventHandler(uc_Proyecto_Leave);
            this.uc_CentroCosto.Leave += new EventHandler(uc_CentroCosto_Leave);
            
            this.uc_Proyecto.EnableControl(false);
            this.uc_CentroCosto.EnableControl(false);            

            //Llenar Combos
            TablesResources.GetTableResources(this.cmbTipoDecpreciacionLocal, typeof(TipoDepreciacion));
            TablesResources.GetTableResources(this.cmbTipoDecpreciacionIFRS, typeof(TipoDepreciacion));
            TablesResources.GetTableResources(this.cmbTipoDecpreciacionUSG, typeof(TipoDepreciacion));

            if (this.cmbTipoDecpreciacionLocal.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                this.txtVidaUtilLocal.Enabled = true;
            if (this.cmbTipoDecpreciacionIFRS.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                this.txtVidaUtilIFRS.Enabled = true;
            if (this.cmbTipoDecpreciacionUSG.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                this.txtVidaUtilUSG.Enabled = true;
            
            //Botones Barra.
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Enabled = false;

            this.editChkBox.CheckedChanged +=new EventHandler(editChkBox_CheckedChanged);
            
            this.EnableFooter(false);
                      
        }       

        /// <summary>
        /// Verifica la informacion del DTO_acActivoControl
        /// </summary>
        /// <returns>DTO_TxResult</returns>
        private DTO_TxResult ValidarData()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                result.Details = new List<DTO_TxResultDetail>();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                DTO_TxResultDetailFields drF = new DTO_TxResultDetailFields();

                //Nombre de las columnas.
                string colCtroCosto = string.Empty;
                string colPlaquetaID = string.Empty;
                string colAcClase = string.Empty;
                string colAcGrupo = string.Empty;
                string colTipo = string.Empty;
                string colProyecto = string.Empty;
                string colLocFisica = string.Empty;
                string colInRefID = string.Empty;

                drF = new DTO_TxResultDetailFields();
                drF.Message = string.Empty;
                foreach (int index in this.select)
                {
                    DTO_acActivoControl acCtrl = this._lActivos[index];
                    rd = new DTO_TxResultDetail();
                    rd.Message = string.Empty;
                    rd.line = index;

                    if (this.gcDocument.DataSource != null)
                    {
                        #region Validacion de nulls y FKs
                        #region Cto Costo
                        colCtroCosto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                        drF = _bc.ValidGridCell(colCtroCosto, acCtrl.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region PlaquetaID
                        colPlaquetaID = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_plaquetaid");
                        drF = _bc.ValidGridCell(colPlaquetaID, acCtrl.PlaquetaID.Value, true, false, false, null);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region Ac ClaseID
                        colAcClase = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                        drF = _bc.ValidGridCell(colAcClase, acCtrl.ActivoClaseID.Value, true, true, false, AppMasters.acClase);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region Ac GrupoID
                        colAcGrupo = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                        drF = _bc.ValidGridCell(colAcGrupo, acCtrl.ActivoGrupoID.Value, true, true, false, AppMasters.acGrupo);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region Tipo
                        colTipo = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Tipo");
                        drF = _bc.ValidGridCell(colTipo, acCtrl.ActivoTipoID.Value, true, true, false, AppMasters.acTipo);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region ProyectoID
                        colProyecto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                        drF = _bc.ValidGridCell(colProyecto, acCtrl.ProyectoID.Value, false, true, true, AppMasters.coProyecto);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region LocFisica
                        colLocFisica = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                        drF = _bc.ValidGridCell(colLocFisica, acCtrl.LocFisicaID.Value, false, true, true, AppMasters.glLocFisica);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region ReferenciaID
                        colInRefID = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                        drF = _bc.ValidGridCell(colInRefID, acCtrl.inReferenciaID.Value, true, true, false, AppMasters.inReferencia);


                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion

                        #endregion
                       
                    }
                    if (rd.DetailsFields.Count > 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                    }
                }

                if (result.Result == ResultValue.NOK)
                {
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica que no se duplique la plaqueta.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>isValid</returns>
        private bool ValidarPlaqueta(int index)
        {
            this.isValid = true;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "PlaquetaID"];
            string plaqueta = Convert.ToString(gvDocument.GetRowCellValue(index, col));
            DTO_acActivoControl dtoPLaqueta = new DTO_acActivoControl();

            dtoPLaqueta = _bc.AdministrationModel.acActivoControl_GetByPlaqueta(plaqueta);
            if (dtoPLaqueta != null)
            {
                this.isValid = false;
            }

            return this.isValid;
        }

        /// <summary>
        /// Función que habilita o deshabilita controles del Footer
        /// </summary>
        /// <param name="enable">enable</param>
        private void EnableFooter(bool enable)
        {
            if (!enable)
            {
                //Combos
                this.cmbTipoDecpreciacionLocal.SelectedIndex = 0;
                this.cmbTipoDecpreciacionIFRS.SelectedIndex = 0;
                this.cmbTipoDecpreciacionUSG.SelectedIndex = 0;

                //Campos del DTO
                this.txtVidaUtilLocal.Text = "1";
                this.txtVidaUtilIFRS.Text = "1";
                this.txtVidaUtilUSG.Text = "1";             

                //Totales
                this.txtTotalLocPesos.Text = string.Empty;
                this.txtTotalLocDolares.Text = string.Empty;
            }
                       
            this.txtVidaUtilLocal.Enabled = enable;            
            this.txtVidaUtilIFRS.Enabled = enable;
            this.txtVidaUtilUSG.Enabled = enable;

            this.txtValorSalvamentoLocal.Enabled = enable;
            this.txtValorSalvamentIFRS.Enabled = enable;
            this.txtValorSalvamentUSG.Enabled = enable;
            
            this.cmbTipoDecpreciacionLocal.Enabled = enable;
            this.cmbTipoDecpreciacionIFRS.Enabled = enable;
            this.cmbTipoDecpreciacionUSG.Enabled = enable;

            this.txtValorRetiro.Enabled = enable;
            
            this.uc_CentroCosto.EnableControl(enable);
            this.uc_Proyecto.EnableControl(enable);
        }

        /// <summary>
        /// Función que habilita o deshabilita controles del Header
        /// </summary>
        /// <param name="enable">enable</param>
        private void EnableHeader(bool enable)
        {
            //this.masterMovimientoTipo.EnableControl(enable);
            this.masterTercero.EnableControl(enable);
            this.txtDocumento.Enabled = enable;

            if (enable)
            {
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                this.txtDocumento.Text = string.Empty;
            }
            else
                FormProvider.Master.itemSave.Enabled = false;

        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        private void CalcularTotal()
        {
            try
            {
                decimal sumLocPes = 0;
                decimal sumLocExt = 0;
                decimal sumIFRSPes = 0;
                decimal sumIFRSDol = 0;
                foreach (int index in this.select)
                {
                    sumLocPes += this._lActivos[index].CostoLOC.Value.Value;
                    sumLocExt += this._lActivos[index].CostoEXT.Value.Value;

                    sumIFRSPes += this._lActivos[index].CostoLOC.Value.Value + this._lActivos[index].ValorRetiroIFRS.Value.Value;
                    if( Convert.ToDecimal(this.txtTCIFRS.EditValue, CultureInfo.InvariantCulture) != 0)
                        sumIFRSDol += this._lActivos[index].CostoEXT.Value.Value + this._lActivos[index].ValorRetiroIFRS.Value.Value / Convert.ToDecimal(this.txtTCIFRS.EditValue, CultureInfo.InvariantCulture);
                }
                this.txtTotalLocPesos.EditValue = sumLocPes;
                this.txtTotalLocDolares.EditValue = sumLocExt;
                this.txtTotalIFRSPesos.EditValue = sumIFRSPes;
                this.txtTotalIFRSDolares.EditValue = sumIFRSDol;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AltaActivos.cs", "CalcularTotal"));
            }
        }

        /// <summary>
        /// Informacion por defecto
        /// </summary>
        private void DefaultData()
        {
            string detaultEstado = this._bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_EstadoXDefecto);
            foreach (var acCtrl in this._lActivos)
            {                
                acCtrl.Tipo.Value = (byte)TipoActivo.Activo;
                acCtrl.DocumentoID.Value = this.documentID;
                acCtrl.Fecha.Value = this.dtFecha.DateTime;
                acCtrl.Periodo.Value = this.dtPeriod.DateTime;
                acCtrl.EstadoInv.Value = (byte)EstadoInv.Activo;
                acCtrl.Propietario.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto);
                acCtrl.EstadoActID.Value = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_EstadoXDefecto);
                acCtrl.MonedaID.Value = this._glDocumentoControl.MonedaID.Value;
                acCtrl.TerceroID.Value = this.terceroID;
                acCtrl.NumeroDocCompra.Value = Convert.ToInt16(this.txtDocumentoID.Text);
                acCtrl.DocumentoTercero.Value = this.txtDocumento.Text;
                acCtrl.VidaUtilLOC.Value = Convert.ToInt32(this.txtVidaUtilLocal.Text);
                acCtrl.VidaUtilIFRS.Value = Convert.ToInt32(this.txtVidaUtilIFRS.Text);
                acCtrl.VidaUtilUSG.Value = Convert.ToInt32(this.txtVidaUtilUSG.Text);
                acCtrl.TipoDepreLOC.Value = Convert.ToByte((this.cmbTipoDecpreciacionLocal.SelectedItem as ComboBoxItem).Value);
                acCtrl.TipoDepreIFRS.Value = Convert.ToByte((this.cmbTipoDecpreciacionIFRS.SelectedItem as ComboBoxItem).Value);
                acCtrl.TipoDepreUSG.Value = Convert.ToByte((this.cmbTipoDecpreciacionUSG.SelectedItem as ComboBoxItem).Value);
                acCtrl.ValorSalvamentoLOC.Value = Convert.ToDecimal(this.txtValorSalvamentoLocal.EditValue, CultureInfo.InvariantCulture);
                acCtrl.ValorSalvamentoIFRS.Value = Convert.ToDecimal(this.txtValorSalvamentIFRS.EditValue, CultureInfo.InvariantCulture);
                acCtrl.ValorSalvamentoUSG.Value = Convert.ToDecimal(this.txtValorSalvamentUSG.EditValue, CultureInfo.InvariantCulture);
                acCtrl.ValorRetiroIFRS.Value = 0;
                acCtrl.EstadoActID.Value = detaultEstado;
                acCtrl.NumeroDocUltMvto.Value = this.documentID;
            }
        }       

        /// <summary>
        /// Limia los Controls del Header 
        /// </summary>
        private void CleanHeader()
        {
            this.txtNumeroDoc.Text = string.Empty;
            this.txtDocumento.Text = string.Empty;
            this.masterTercero.Value = string.Empty;
            this.masterMovimientoTipo.Value = string.Empty;
        }

        /// <summary>
        /// Funcion que limpia los coontroles del footer
        /// </summary>
        private void CleanFooter()
        {
            this.cmbTipoDecpreciacionIFRS.SelectedItem = 0;
            this.cmbTipoDecpreciacionLocal.SelectedItem = 0;
            this.txtVidaUtilLocal.Text = string.Empty;
            this.txtVidaUtilIFRS.Text = string.Empty;           
        }

        /// <summary>
        /// Valida que los items incluidos correspondan al valor de la Factura
        /// </summary>
        /// <returns></returns>
        private bool ValidateVlrFactura()
        {
            bool bandera = false;
            if (this.vlrFactura == this._lActivos.Sum(x => x.CostoLOC.Value.Value))
                bandera = true;
            return bandera;
        }

        #endregion

        #region Eventos header superior

        /// <summary>
        /// Evento que carga un documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDocumento_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.txtDocumento.Text) && this.masterTercero.ValidID)
                {
                    this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                    this.newDoc = true;
                    this._documento = this.txtDocumento.Text;
                    this.terceroID = this.masterTercero.Value;
                    this._glDocumentoControl = this._bc.AdministrationModel.glDocumentoControl_GetExternalDoc(AppDocuments.CausarFacturas, terceroID, this._documento);
                    if (this._glDocumentoControl != null)
                    {
                        #region Valida la moneda de origen con la factura
                        if (this._monedaOrigen == "2")
                        {
                            if (this._monedaLocal != this._glDocumentoControl.MonedaID.ToString())
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ac_BillAndDocumentInfo));
                                this.CleanHeader();
                                return;
                            }
                        }
                        if (this._monedaOrigen == "3")
                        {
                            if (this._monedaExtranjera != this._glDocumentoControl.MonedaID.ToString())
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ac_BillAndDocumentInfo));
                                this.CleanHeader();
                                return;
                            }
                        }
                        #endregion
                        //Valida y define fecha
                        if (this._glDocumentoControl.FechaDoc.Value.HasValue &&
                            this._glDocumentoControl.FechaDoc.Value.Value.Year == this.dtPeriod.DateTime.Year &&
                            this._glDocumentoControl.FechaDoc.Value.Value.Month == this.dtPeriod.DateTime.Month)
                        {
                            this.dtFecha.DateTime = this._glDocumentoControl.Fecha.Value.Value;

                            this.txtTCLoc.Text = this._glDocumentoControl.TasaCambioCONT.Value.ToString();
                            string indTcIFRS = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndTasaCambIFRSMen);

                            if (!string.IsNullOrEmpty(indTcIFRS))
                            {
                                if (indTcIFRS == "1")
                                {
                                    Dictionary<string, string> dic = new Dictionary<string, string>();
                                    dic.Add("Periodo", this.dtPeriod.DateTime.ToString());
                                    dic.Add("PeriodoID", this.dtPeriod.DateTime.ToString());
                                    DTO_glDatosMensuales datosMensuales = (DTO_glDatosMensuales)this._bc.AdministrationModel.MasterComplex_GetByID(AppMasters.glDatosMensuales, dic, true);
                                    if(datosMensuales != null)
                                        this.txtTCIFRS.Text = datosMensuales.Valor1.Value.ToString();
                                }
                                else
                                {
                                    this.txtTCIFRS.Text = this._glDocumentoControl.TasaCambioCONT.Value.ToString();
                                }
                            }
                            else                            
                                this.txtTCIFRS.Text = this._glDocumentoControl.TasaCambioCONT.Value.ToString();
                            
                            this.dtFechaFactura.DateTime = this._glDocumentoControl.FechaDoc.Value.Value;
                            this.vlrFactura = this._glDocumentoControl.Valor.Value.Value;
                            this.proyecto = this._glDocumentoControl.ProyectoID.Value;
                            this.centroCosto = this._glDocumentoControl.CentroCostoID.Value;
                        }
                        else
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_FacturaFueraPeriodo));
                            return;
                        }
                        this._lActivos = this._bc.AdministrationModel.AltaActivos_GetActivosByNumDoc(this._glDocumentoControl.NumeroDoc.Value.Value);
                        this.LoadData(true);
                        if (this._lActivos.Count == 0)
                        {
                            this.EnableFooter(false);
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoActFact));
                        }
                        else
                        {
                            this.CalcularTotal();
                            this.DefaultData();
                            this.EnableHeader(false);
                            this.EnableFooter(false);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoFact));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AltaActivos.cs", "txtDocumento_Leave"));
            }
        }

        /// <summary>
        /// Funcionque valida si la informacionel master movimiento escorrecta y es igual al documentID
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterMovimientoTipo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.masterMovimientoTipo.Value))
                    return;

                string mvtoTipo = this.masterMovimientoTipo.Value;
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, false, mvtoTipo, true);
                DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, MvtoTipo.coDocumentoID.Value, true);

                this._monedaOrigen = coDoc.MonedaOrigen.ToString();

                if (coDoc.DocumentoID.Value != this.documentID.ToString())
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                    this.masterMovimientoTipo.Value = string.Empty;
                    this.validMove = false;
                }
                else
                    this.validMove = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Tasa de Cambio digitada (solo aplica cuando no estan activos los modulos de cxp ni proveedores)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTCLoc_Leave(object sender, System.EventArgs e)
        {
            string indTcIFRS = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndTasaCambIFRSMen);

            if (!string.IsNullOrEmpty(indTcIFRS))
            {
                if (indTcIFRS == "1")
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("Periodo", this.dtPeriod.DateTime.ToString());
                    dic.Add("PeriodoID", this.dtPeriod.DateTime.ToString());
                    DTO_glDatosMensuales datosMensuales = (DTO_glDatosMensuales)this._bc.AdministrationModel.MasterComplex_GetByID(AppMasters.glDatosMensuales, dic, true);
                    if (datosMensuales != null)
                        this.txtTCIFRS.EditValue = datosMensuales.Valor1.Value.ToString();
                }
                else
                {
                    this.txtTCIFRS.EditValue = this.txtTCLoc.EditValue;
                }
            }
            else
                this.txtTCIFRS.EditValue = this.txtTCLoc.EditValue;

            this.dtFechaFactura.DateTime = DateTime.Now;
        }

        #endregion

        #region Eventos Grilla
                
        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (this._noLoaded)
            {
                if (fieldName == "Marca")
                    e.Value = null;
            }

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                {
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                e.Value = fi.GetValue(dto);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
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
                        {
                            e.Value = pi.GetValue(dto, null);
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
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Marca")
            {
                e.RepositoryItem = this.editChkBox;
            }
            if (fieldName == "ActivoGrupoID" || fieldName == "ActivoClaseID" || fieldName == "LocFisicaID" ||
                fieldName == "ActivoTipoID" || fieldName == "inReferenciaID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "CostoLOC" || fieldName == "CostoEXT")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar un checbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editChkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (((CheckEdit)sender).Checked)
                    EnableFooter(true);
                else
                    EnableFooter(false);

                this.gvDocument.RefreshData();
                this.CalcularTotal();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
            
            if (fieldName == "Marca")
            {
                this.CalcularTotal();
                this.UpdateTemp(this._lActivos);

                if ((bool)e.Value)
                {
                    this.selected = true;
                    this._validRow = true;
                }
                else
                {
                    this.selected = false;
                    this._validRow = false;
                }
            }
            if (fieldName == "PlaquetaID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lActivos[e.RowHandle].PlaquetaID.Value = val;
            }
            if (fieldName == "SerialID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lActivos[e.RowHandle].SerialID.Value = val;
            }
            if (fieldName == "Turnos")
            {
                byte val = (byte)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lActivos[e.RowHandle].Turnos.Value = val;
            }
            if (fieldName == "Modelo")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lActivos[e.RowHandle].Modelo.Value = val;
            }
            if (fieldName == "inReferenciaID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lActivos[e.RowHandle].inReferenciaID.Value = val;
            }
            if (fieldName == "CostoLOC")
            {
                if (Convert.ToDecimal(this.txtTCLoc.EditValue, CultureInfo.InvariantCulture) != 0)
                {
                    decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                    this._lActivos[e.RowHandle].CostoLOC.Value = val;
                    this.CalcularTotal();
                }
                else
                {
                    if (this.multiMoneda)
                    {
                        this._lActivos[e.RowHandle].CostoLOC.Value = 0;
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoAssignedTasaCamabio));
                    }
                }
                this.LoadData(true);
            }
            if (fieldName == "CostoEXT")
            {
                if (Convert.ToDecimal(this.txtTCLoc.EditValue, CultureInfo.InvariantCulture) != 0)
                {
                    decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                    this._lActivos[e.RowHandle].CostoEXT.Value = val;
                    this.CalcularTotal();
                }
                else
                {
                    this._lActivos[e.RowHandle].CostoLOC.Value = 0;
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoAssignedTasaCamabio));
                }
                this.LoadData(true);
            }
            if (fieldName == "Descriptivo")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lActivos[e.RowHandle].Observacion.Value = val;
            }
        }

        /// <summary>
        /// Evento que envia el evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this._noLoaded)
                return;
            
            int fila = this.gvDocument.FocusedRowHandle;
            bool selectMark;

            if (this._validRow)
            {
                #region Serial
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "SerialID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ActivoGrupoID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ActivoGrupoID", false, false, false, null);
                    this.deleteOP = false;
                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ActivoClaseID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ActivoClaseID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region LocFisicaID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "LocFisicaID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ActivoTipoID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ActivoTipoID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {            
            base.gvDocument_FocusedRowChanged(sender, e);
            if (this._lActivos != null && this._lActivos.Count > 0)
            {
                DTO_acActivoControl currentActivo = this._lActivos[e.FocusedRowHandle];
                this.uc_Proyecto.Value = currentActivo.ProyectoID.Value;
                this.uc_CentroCosto.Value = currentActivo.CentroCostoID.Value;

                DTO_acClase acClase = (DTO_acClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acClase, false, currentActivo.ActivoClaseID.Value, true);
                if (acClase != null)
                {
                    this.txtValorSalvamentoLocal.EditValue = (((acClase.PorcSalvamentoLOC.Value != null ? acClase.PorcSalvamentoLOC.Value : 0) * currentActivo.CostoLOC.Value) / 100);
                    this.txtValorSalvamentIFRS.EditValue = (((acClase.PorcSalvamentoIFRS.Value != null ? acClase.PorcSalvamentoIFRS.Value : 0) * currentActivo.CostoLOC.Value) / 100);
                    this.txtValorSalvamentUSG.EditValue = (((acClase.PorcSalvamentoUSG.Value != null ? acClase.PorcSalvamentoUSG.Value : 0) * currentActivo.CostoEXT.Value) / 100);
                    this.txtVidaUtilLocal.Text = acClase.VidaUtilLOC.Value > 0 ? acClase.VidaUtilLOC.Value.ToString() : "1";
                    this.txtVidaUtilIFRS.Text = acClase.VidaUtilIFRS.Value > 0 ? acClase.VidaUtilIFRS.Value.ToString() : "1";
                    this.txtVidaUtilUSG.Text = acClase.VidaUtilUSG.Value > 0 ? acClase.VidaUtilUSG.Value.ToString() : "1";
                }

                //Libro Local
                this.txtLocalPesos.EditValue = currentActivo.CostoLOC.Value;
                this.txtLocalDolares.EditValue = currentActivo.CostoEXT.Value;

                //Libro IFRS
                this.txtIFRSPesos.EditValue = currentActivo.CostoLOC.Value + currentActivo.ValorRetiroIFRS.Value;
                if(Convert.ToDecimal(this.txtTCIFRS.EditValue, CultureInfo.InvariantCulture) != 0)
                    this.txtIFRSDolares.EditValue = currentActivo.CostoEXT.Value + currentActivo.ValorRetiroIFRS.Value.Value / Convert.ToDecimal(this.txtTCIFRS.EditValue, CultureInfo.InvariantCulture);
                
                this.CalcularTotal();
            }
        }

        /// <summary>
        /// Maneja el boton mas de la grilla de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);

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
                e.Handled = true;
                //Revisa si desea cargar los temporales
                if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.deleteOP = true;
                    int rowHandle = this.gvDocument.FocusedRowHandle;

                    if (this._lActivos.Count == 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                        e.Handled = true;
                    }
                    else
                    {
                        this._lActivos.Remove(this._lActivos[rowHandle]);
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
            }
        }

        #endregion

        #region Eventos Footer
              

        /// <summary>
        /// Evento que se ejecuta al salir de un textbox 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void textControl_Leave(object sender, EventArgs e)
        {
            if (this._noLoaded)
                return;

            TextBox ctrl = (TextBox)sender;

            int index = this.gvDocument.FocusedRowHandle;

            switch (ctrl.Name)
            {
                // Vida Util Moneda Local 
                case "txtVidaUtilLocal":
                    if (!string.IsNullOrWhiteSpace(this.txtVidaUtilLocal.Text))
                       this._lActivos[index].VidaUtilLOC.Value = Convert.ToInt16(ctrl.Text);
                    break;

                // Vida Util Moneda Exranjera 
                case "txtVidaUtilIFRS":
                    if (!string.IsNullOrWhiteSpace(this.txtVidaUtilIFRS.Text))
                        this._lActivos[index].VidaUtilIFRS.Value = Convert.ToInt16(ctrl.Text);
                    break;
                case "txtVidaUtilUSG":
                    if (!string.IsNullOrWhiteSpace(this.txtVidaUtilUSG.Text))
                        this._lActivos[index].VidaUtilUSG.Value = Convert.ToInt16(ctrl.Text);
                    break;              
            }

            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Evento q habilita el valor resifdual
        /// </summary>
        /// <param name="sender">e</param>
        /// <param name="e">e</param>
        private void cmbTipoDecpreciacionML_SelectedIndexChanged(object sender, EventArgs e)
        {

            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            if (this.cmbTipoDecpreciacionLocal.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
            {
                this.txtValorSalvamentoLocal.Enabled = true;
                this._lActivos[fila].TipoDepreLOC.Value = 1;
            }
            else
                this.txtValorSalvamentoLocal.Enabled = false;

            if (this.cmbTipoDecpreciacionLocal.SelectedIndex == Convert.ToInt32(TipoDepreciacion.LineaRecta))
                this._lActivos[fila].TipoDepreLOC.Value = 0;

            if (this.cmbTipoDecpreciacionLocal.SelectedIndex == Convert.ToInt32(TipoDepreciacion.UnidadesDeProduccion))
                this._lActivos[fila].TipoDepreLOC.Value = 2;
        }

        /// <summary>
        /// Evento q habilita el valor resifdual
        /// </summary>
        /// <param name="sender">e</param>
        /// <param name="e">e</param>
        private void cmbTipoDecpreciacionME_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            if (this.cmbTipoDecpreciacionIFRS.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
            {
                this.txtValorSalvamentIFRS.Enabled = true;
            }
            else
                this.txtValorSalvamentIFRS.Enabled = false;       
        }

        /// <summary>
        /// Evento q habilita el valor resifdual
        /// </summary>
        /// <param name="sender">e</param>
        /// <param name="e">e</param>
        private void cmbTipoDecpreciacionUSG_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            if (this.cmbTipoDecpreciacionUSG.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
            {
                this.txtValorSalvamentUSG.Enabled = true;
            }
            else
                this.txtValorSalvamentUSG.Enabled = false;       
        }        
    
        /// <summary>
        /// Salvamento Local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorSalvamentoLocal_EditValueChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtValorSalvamentoLocal.Text))
            {
                int fila = this.gvDocument.FocusedRowHandle;
                if (fila < 0)
                    return;
                this._lActivos[fila].ValorSalvamentoLOC.Value = Convert.ToDecimal(this.txtValorSalvamentoLocal.EditValue, CultureInfo.InvariantCulture);
            }
        }    

        /// <summary>
        /// Salvamento IFRS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorSalvamentIFRS_EditValueChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtValorSalvamentIFRS.Text))
            {
                int fila = this.gvDocument.FocusedRowHandle;
                if (fila < 0)
                    return;
                this._lActivos[fila].ValorSalvamentoIFRS.Value = Convert.ToDecimal(this.txtValorSalvamentIFRS.EditValue, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Salvamento US-GAP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorSalvamentUSG_EditValueChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtValorSalvamentUSG.Text))
            {
                int fila = this.gvDocument.FocusedRowHandle;
                if (fila < 0)
                    return;
                this._lActivos[fila].ValorSalvamentoUSG.Value = Convert.ToDecimal(this.txtValorSalvamentUSG.EditValue, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Valor Desmantelamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorRetiro_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtValorRetiro.Text))
            {
                int fila = this.gvDocument.FocusedRowHandle;
                if (fila < 0)
                    return;

                this._lActivos[fila].ValorRetiroIFRS.Value = Convert.ToDecimal(this.txtValorRetiro.EditValue, CultureInfo.InvariantCulture);
                this.txtIFRSPesos.EditValue = (this._lActivos[fila].CostoLOC.Value + this._lActivos[fila].ValorRetiroIFRS.Value);
                if (Convert.ToDecimal(this.txtTCIFRS.EditValue, CultureInfo.InvariantCulture) != 0)
                    this.txtIFRSDolares.EditValue = (this._lActivos[fila].CostoEXT.Value + (this._lActivos[fila].ValorRetiroIFRS.Value / Convert.ToDecimal(this.txtTCIFRS.EditValue, CultureInfo.InvariantCulture)));
                else
                    this.txtIFRSDolares.EditValue = 0;

                this.CalcularTotal();
            }
        }

        /// <summary>
        /// Cambia centro de costo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_CentroCosto_Leave(object sender, EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            this._lActivos[fila].ProyectoID.Value = this.uc_Proyecto.Value;
        }

        /// <summary>
        /// Cambia Proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Proyecto_Leave(object sender, EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            this._lActivos[fila].CentroCostoID.Value = this.uc_CentroCosto.Value;
        }
        
        #endregion

        #region Eventos MDI

        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            try
            {
                if (this._lActivos.Count > 0)
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

                    if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        this._lActivos = new List<DTO_acActivoControl>();
                        this.Invoke(this.refresGrd);
                        this.EnableFooter(false);
                        this.EnableHeader(true);

                        this.masterMovimientoTipo.Value = string.Empty;
                        this.masterTercero.Value = string.Empty;
                    }
                }

                if (this._lActivos.Count == 0)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this._lActivos = new List<DTO_acActivoControl>();
                    this.Invoke(this.refresGrd);
                    this.EnableFooter(false);
                    this.EnableHeader(true);

                    this.masterMovimientoTipo.Value = string.Empty;
                    this.masterTercero.Value = string.Empty;
                }
                this._noLoaded = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            base.TBSave();
            this.gvDocument.PostEditor();
            try
            {
                if (base.select.Count == 0)
                    return;

                //validarChek();
                if (this.selected == false)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_anyRowSelected));
                    return;
                }
                if (!this.validMove)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                    return;
                }
                DTO_TxResult res = this.ValidarData();

                if (res.Result == ResultValue.OK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                this.gvDocument.PostEditor();
                List<DTO_acActivoControl> listAppr = new List<DTO_acActivoControl>();

                if (!this._modProveedores && this._modCxP)
                {
                    if (!this.ValidateVlrFactura())
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoSumActFactura));
                        return;
                    }
                }
                
                foreach (int index in this.select)
                {
                    listAppr.Add(this._lActivos[index]);
                    //Valida que no haya una plaqueta repetida en la bd
                    this.ValidarPlaqueta(index);
                }
                //Valida que la info que viene no repita la plaqueta
                List<string> indices = listAppr.Select(x => x.PlaquetaID.Value).Distinct().ToList();

                if (!this.isValid || indices.Count != listAppr.Count)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_DuplicatePlate));
                    return;
                }


                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = _bc.AdministrationModel.acActivoControl_AddList(this.documentID, this._actFlujo.ID.Value, this.txtPrefix.Text, listAppr, this.masterMovimientoTipo.Value, Convert.ToDecimal(this.txtTCLoc.EditValue, CultureInfo.InvariantCulture));
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
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this._lActivos = new List<DTO_acActivoControl>();
                    this.Invoke(this.saveDelegate);
                    this.Invoke(this.refresGrd);
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