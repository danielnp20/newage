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
using NewAge.Forms;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class Contenedores : DocumentForm
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
            this.btnContenedores.EditValue = string.Empty;
            this.uc_Proyecto.Value = string.Empty;
            this.uc_CentroCosto.Value = string.Empty;
            this.uc_LocFisica.Value = string.Empty;
            this.uc_Bodega.Value = string.Empty;
            this.txtValorSalvamentoLocal.EditValue = string.Empty;
            this.txtValorSalvamentIFRS.EditValue = string.Empty;
            this.txtValorSalvamentUSG.EditValue = string.Empty;
            this.txtValorRetiro.EditValue = string.Empty;
            this.txtVidaUtilLocal.Text = string.Empty;
            this.txtVidaUtilUSG.Text = string.Empty;
            this.txtVidaUtilIFRS.Text = string.Empty;
        }
        
        #endregion

        #region Variables Formulario

        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _glDocumentoControl = null;
        private string _documento = string.Empty;
        private string _monedaOrigen;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private DTO_acActivoControl ActivoContenedor;
        private List<DTO_acActivoControl> _lActivos;
        private bool selected = false;
        private bool validMove = true;
        private bool _validRow = false;
        private bool _noLoaded = false;

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
                if (firstTime)
                {
                    if (this._lActivos != null && this._lActivos.Count > 0)
                    {
                        this.gcDocument.DataSource = this._lActivos;
                        this.gcDocument.RefreshDataSource();
                    }
                }
                else
                {
                    this._lActivos.Clear();
                    this.gcDocument.DataSource = this._lActivos;
                    this.gcDocument.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Contenedores.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Contenedores;
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

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {              
                //Plaqueta
                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this.unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 1;
                plaquetaID.Width = 110;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 80;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(serialID);
              
                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 3;
                inReferenciaID.Width = 100;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 4;
                descripcion.Width = 250;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descripcion);


                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this.unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 5;
                clase.Width = 90;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 6;
                ActivoTipoID.Width = 90;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this.unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 7;
                grupo.Width = 90;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(grupo);
               

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 8;
                locFisica.Width = 90;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(locFisica);

                //Centro Costo
                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 9;
                centroCostoID.Width = 90;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(centroCostoID);

                //Centro Costo
                GridColumn proyectoID = new GridColumn();
                proyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                proyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyectoID.UnboundType = UnboundColumnType.String;
                proyectoID.VisibleIndex = 10;
                proyectoID.Width = 90;
                proyectoID.Visible = true;
                proyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(proyectoID);
                        
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

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                //this.cmbMonedaOrigen.Enabled = false;
                this.lblTasaCambio.Visible = false;
                this.txtTasaCambio.Visible = false;
            }

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
                    this.dtFecha.DateTime = this._glDocumentoControl.Fecha.Value.Value;
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

            _bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.uc_CentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.uc_Bodega, AppMasters.inBodega, true, true, true, false);
            _bc.InitMasterUC(this.uc_LocFisica, AppMasters.glLocFisica, true, true, true, false);
           
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
            this.tlSeparatorPanel.RowStyles[0].Height = 40;
            
            this.EnableFooter(false);

        }
        
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
            if (enable)
            {
                FormProvider.Master.itemSave.Enabled = true;
            }
            else
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);

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
                acCtrl.Propietario.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                acCtrl.EstadoActID.Value = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_EstadoXDefecto);
                acCtrl.MonedaID.Value = this._glDocumentoControl.MonedaID.Value;
                acCtrl.TerceroID.Value = this.terceroID;
                acCtrl.NumeroDocCompra.Value = Convert.ToInt16(this.txtDocumentoID.Text);
                acCtrl.DocumentoTercero.Value = string.Empty;
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

        #endregion

        #region Eventos header

        /// <summary>
        /// Trae el control de busqueda de activos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContenedores_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FindActivosModal modalActivos = new FindActivosModal(true);
            modalActivos.ShowDialog();
            this.ActivoContenedor = modalActivos.Activo;
            if (this.ActivoContenedor != null)
            {
                btnContenedores.EditValue = this.ActivoContenedor.ActivoID.Value.ToString();
                this.uc_Proyecto.Value = this.ActivoContenedor.ProyectoID.Value;
                this.uc_CentroCosto.Value = this.ActivoContenedor.CentroCostoID.Value;
                this.uc_LocFisica.Value = this.ActivoContenedor.LocFisicaID.Value;
                this.uc_Bodega.Value = this.ActivoContenedor.BodegaID.Value;
                this._lActivos = this._bc.AdministrationModel.acActivoControl_GetChildrenActivos(this.ActivoContenedor.ActivoID.Value.Value);
                this.LoadData(true);
            }
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
                fieldName == "ActivoTipoID")
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

                this.txtValorSalvamentoLocal.Text = currentActivo.ValorSalvamentoLOC.Value.ToString();
                this.txtValorSalvamentIFRS.Text = currentActivo.ValorSalvamentoIFRS.Value.ToString();
                this.txtValorSalvamentUSG.Text = currentActivo.ValorSalvamentoUSG.Value.ToString();
                this.txtVidaUtilLocal.Text = currentActivo.VidaUtilLOC.Value.ToString();
                this.txtVidaUtilIFRS.Text = currentActivo.VidaUtilIFRS.Value.ToString();
                this.txtVidaUtilUSG.Text = currentActivo.VidaUtilUSG.Value.ToString();
                this.txtValorRetiro.Text = currentActivo.ValorRetiroIFRS.Value.ToString();
            }
        }

        /// <summary>
        /// Boton de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
            {
                if (this.ActivoContenedor == null)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_ActContenedor));
                    return;
                }
                #region Busqueda Activos

                //Busca un activo
                FindActivosModal modalActivos = new FindActivosModal(false, true);
                modalActivos.ShowDialog();

                List<DTO_acActivoControl> acts = modalActivos.ActivosSel;
                if (acts != null)
                {
                    foreach (var act in acts)
                    {
                        //Verifica si es contenedor
                        DTO_acTipo acTipo = (DTO_acTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acTipo, false, act.ActivoTipoID.Value, true);
                        if (acTipo != null)
                        {
                            //Es una Contenedor
                            if (acTipo.ContenedorInd.Value.Value)
                            {
                                List<DTO_acActivoControl> children = this._bc.AdministrationModel.acActivoControl_GetChildrenActivos(act.ActivoID.Value.Value);
                                foreach (var actChild in children)
                                {        
                                    if(actChild.ActivoPadreID.Value != this.ActivoContenedor.ActivoID.Value)
                                        this._lActivos.Add(actChild);
                                }
                            }
                            //Es una activo
                            else
                            {                                
                                this._lActivos.Add(act);

                            }
                        }               
                    
                    }
                }

                
                this.LoadData(true);

                #endregion
            }
            else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
            {
                if(this._lActivos !=  null && this._lActivos.Count > 0)
                    this._lActivos.Remove(this._lActivos[gvDocument.FocusedRowHandle]);
            }
            this.gcDocument.RefreshDataSource();
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
                        this.Invoke(this.refresGrd);
                        this.EnableFooter(false);
                    }
                }

                if (this._lActivos.Count == 0)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.Invoke(this.refresGrd);
                    this.EnableFooter(false);
                }
                this._noLoaded = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Contenedores.cs", "TBNew"));
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
                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                Thread process = new Thread(this.SaveThread);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Contenedores.cs", "TBSave"));
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

                if (this._lActivos == null || this._lActivos.Count == 0)
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoLoadActContenedor)); 

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = null;// _bc.AdministrationModel.acActivoControl_AddList(this.documentID, this._actFlujo.ID.Value, this.txtPrefix.Text, listAppr, this.masterMovimientoTipo.Value);
                
                
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