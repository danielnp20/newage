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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class RetiroActivos : DocumentForm
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
            this.CleanHeader();           
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
        private bool validMove = false;
        private bool _validRow = false;
        private bool _noLoaded = false;
        private DTO_acActivoControl _Activo;
        private string balanceFuncional;
        private string balanceIFRS;
        private List<DTO_acRetiroActivoComponente> _Componentes = new List<DTO_acRetiroActivoComponente>();

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
            this.documentID = AppDocuments.RetiroActivos;
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
            this.balanceFuncional = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            this.balanceIFRS = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);

            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;
            this.tlSeparatorPanel.RowStyles[0].Height = 100;
            this.tlSeparatorPanel.RowStyles[1].Height = 270;
            this.tlSeparatorPanel.RowStyles[2].Height = 230;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                base.AddGridCols();

                #region Activos

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
                serialID.Width = 110;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(serialID);
               

                //Turnos
                GridColumn turnos = new GridColumn();
                turnos.FieldName = this.unboundPrefix + "Turnos";
                turnos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Turnos");
                turnos.UnboundType = UnboundColumnType.Integer;
                turnos.VisibleIndex = 3;
                turnos.Width = 90;
                turnos.Visible = true;
                turnos.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(turnos);

                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 4;
                inReferenciaID.Width = 100;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 5;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                descripcion.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(descripcion);
                

                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this.unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 6;
                clase.Width = 90;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 7;
                ActivoTipoID.Width = 90;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this.unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 8;
                grupo.Width = 90;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(grupo);

                //Modelo
                GridColumn modelo = new GridColumn();
                modelo.FieldName = this.unboundPrefix + "Modelo";
                modelo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Modelo");
                modelo.UnboundType = UnboundColumnType.String;
                modelo.VisibleIndex = 9;
                modelo.Width = 80;
                modelo.Visible = true;
                modelo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(modelo);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 11;
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

                #endregion

                #region Componentes Activos

                //ComponenteActivoID
                GridColumn componenteActivoID = new GridColumn();
                componenteActivoID.FieldName = this.unboundPrefix + "ComponenteActivoID";
                componenteActivoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteActivoID");
                componenteActivoID.UnboundType = UnboundColumnType.String;
                componenteActivoID.VisibleIndex = 1;
                componenteActivoID.Width = 110;
                componenteActivoID.Visible = true;
                componenteActivoID.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(componenteActivoID);

                //ComponenteActivoID
                GridColumn componenteActivoDesc = new GridColumn();
                componenteActivoDesc.FieldName = this.unboundPrefix + "ComponenteActivoDesc";
                componenteActivoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteActivoDesc");
                componenteActivoDesc.UnboundType = UnboundColumnType.String;
                componenteActivoDesc.VisibleIndex = 2;
                componenteActivoDesc.Width = 230;
                componenteActivoDesc.Visible = true;
                componenteActivoDesc.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(componenteActivoDesc);

                //VlrSaldoML
                GridColumn vlrSaldoML = new GridColumn();
                vlrSaldoML.FieldName = this.unboundPrefix + "VlrSaldoML";
                vlrSaldoML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldoML");
                vlrSaldoML.UnboundType = UnboundColumnType.String;
                vlrSaldoML.VisibleIndex = 3;
                vlrSaldoML.Width = 125;
                vlrSaldoML.Visible = true;
                vlrSaldoML.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(vlrSaldoML);

                //VlrSaldoME
                GridColumn vlrSaldoME = new GridColumn();
                vlrSaldoME.FieldName = this.unboundPrefix + "VlrSaldoME";
                vlrSaldoME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldoME");
                vlrSaldoME.UnboundType = UnboundColumnType.String;
                vlrSaldoME.VisibleIndex = 4;
                vlrSaldoME.Width = 125;
                vlrSaldoME.Visible = true;
                vlrSaldoME.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(vlrSaldoME);

                //VlrSaldoIFRSML
                GridColumn vlrSaldoIFRSML = new GridColumn();
                vlrSaldoIFRSML.FieldName = this.unboundPrefix + "VlrSaldoIFRSML";
                vlrSaldoIFRSML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldoIFRSML");
                vlrSaldoIFRSML.UnboundType = UnboundColumnType.String;
                vlrSaldoIFRSML.VisibleIndex = 5;
                vlrSaldoIFRSML.Width = 125;
                vlrSaldoIFRSML.Visible = true;
                vlrSaldoIFRSML.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(vlrSaldoIFRSML);

                //VlrSaldoIFRSME
                GridColumn vlrSaldoIFRSME = new GridColumn();
                vlrSaldoIFRSME.FieldName = this.unboundPrefix + "VlrSaldoIFRSME";
                vlrSaldoIFRSME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldoIFRSME");
                vlrSaldoIFRSME.UnboundType = UnboundColumnType.String;
                vlrSaldoIFRSME.VisibleIndex = 6;
                vlrSaldoIFRSME.Width = 125;
                vlrSaldoIFRSME.Visible = true;
                vlrSaldoIFRSME.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(vlrSaldoIFRSME);

                #endregion 
                
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
            this._lActivos = new List<DTO_acActivoControl>();
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
            #region Filtros Tipo de Movimientos (Baja y Ventas)

            List<DTO_glConsultaFiltro> filtrosComplejos = new List<DTO_glConsultaFiltro>();

            DTO_glConsultaFiltro eg = new DTO_glConsultaFiltro();
            eg.CampoFisico = "TipoMvto";
            eg.OperadorFiltro = OperadorFiltro.Igual;
            eg.ValorFiltro = "2"; //Corresponde al tipo de movientos (2) Baja
            eg.OperadorSentencia = "OR";

            DTO_glConsultaFiltro egVentas = new DTO_glConsultaFiltro();
            egVentas.CampoFisico = "TipoMvto";
            egVentas.OperadorFiltro = OperadorFiltro.Igual;
            egVentas.ValorFiltro = "3"; //Corresponde al tipo de movientos (3) Ventas
            egVentas.OperadorSentencia = "OR";

            filtrosComplejos.Add(eg);
            filtrosComplejos.Add(egVentas);
           
            #endregion

            this._bc.InitMasterUC(this.masterMovimientoTipo, AppMasters.acMovimientoTipo, true, true, true, false, filtrosComplejos);
            //Botones Barra.
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Enabled = false;

            this.editChkBox.CheckedChanged +=new EventHandler(editChkBox_CheckedChanged);
        }       
           
        /// <summary>
        /// Limia los Controls del Header 
        /// </summary>
        private void CleanHeader()
        {
            this.masterMovimientoTipo.Value = string.Empty;
            this.gcDocument.DataSource = null;
            this.gcComponentes.DataSource = null;
            this.txtFuncLoc.EditValue = 0;
            this.txtFuncExt.EditValue = 0;
            this.txtIFRSLoc.EditValue = 0;
            this.txtIFRSExt.EditValue = 0;
        }

        #endregion

        #region Eventos header superior
        
        /// <summary>
        /// Carga el Activo asociado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActivos_Click(object sender, EventArgs e)
        {
            FindActivosModal modalActivos = new FindActivosModal(false, true);
            modalActivos.ShowDialog();
            this._lActivos = modalActivos.ActivosSel;
            if(this._Activo != null)
                this.btnActivos.EditValue = this._Activo.ActivoID.Value.ToString();
            this.LoadData(true);
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

                if (coDoc.DocumentoID.Value != AppDocuments.RetiroActivos.ToString())
                {
                    if (coDoc.DocumentoID.Value != AppDocuments.VentasActivos.ToString())
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                        this.masterMovimientoTipo.Value = string.Empty;
                        this.validMove = false;
                    }
                    else
                    {
                        this.validMove = true;
                        this.documentID = AppDocuments.VentasActivos;                       
                    }
                }
                else
                {
                    this.validMove = true;
                    this.documentID = AppDocuments.RetiroActivos;
                }
                
                DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);
                this.txtDocumentoID.Text = this.documentID.ToString();
                this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RetiroActivos.cs", "TipoMovimientos"));
            }
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Enter de Formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            FormProvider.Master.itemGenerateTemplate.Visible = false;
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

        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this._lActivos != null && this._lActivos.Count > 0)
                {
                    DTO_acActivoControl currentActivo = this._lActivos[e.FocusedRowHandle];
                    this._Componentes = this._bc.AdministrationModel.acActivoFijos_GetComponenentes(currentActivo.ActivoID.Value.Value);
                    this.gcComponentes.DataSource = this._Componentes;
                    this.gcComponentes.RefreshDataSource();
                    if (this._Componentes != null)
                    {
                        this.txtFuncLoc.EditValue = this._Componentes.Sum(x => x.VlrSaldoML.Value).Value;
                        this.txtFuncExt.EditValue = this._Componentes.Sum(x => x.VlrSaldoME.Value).Value;
                        this.txtIFRSLoc.EditValue = this._Componentes.Sum(x => x.VlrSaldoIFRSML.Value).Value;
                        this.txtIFRSExt.EditValue = this._Componentes.Sum(x => x.VlrSaldoIFRSME.Value).Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComponentes_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        /// Maneja format cammpos grilla de componentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComponentes_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrSaldoML" || fieldName == "VlrSaldoME" || fieldName == "VlrSaldoIFRSML" || fieldName == "VlrSaldoIFRSME")
            {
                e.RepositoryItem = this.editValue;
            }         
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
                    }
                }

                if (this._lActivos.Count == 0)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this._lActivos = new List<DTO_acActivoControl>();
                    this.Invoke(this.refresGrd);
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

                this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                Thread process = new Thread(this.SaveThread);
                process.Start();               
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

                if (this._Activo == null)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoActivoSelect));
                    return;
                }

                foreach (int index in this.select)
                {
                    listAppr.Add(this._lActivos[index]);
                }
                              
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = _bc.AdministrationModel.acActivoControl_RetiroActivos(this.documentID, this._actFlujo.ID.Value, this.txtPrefix.Text, listAppr, this.masterMovimientoTipo.Value);
                
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