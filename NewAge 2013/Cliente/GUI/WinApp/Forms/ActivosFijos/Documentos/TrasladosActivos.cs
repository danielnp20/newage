using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class TrasladosActivos : DocumentForm
    {
        //public TrasladosActivos()
        //{
        //    this.InitializeComponent();
        //}

        #region Delegados
        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void RefreshGrid();
        public RefreshGrid refresGrd;
        public void RefreshGridMethod()
        {
            this._noLoaded = true;
            this.LoadData(false);
            this.select = new List<int>();
            this.CleanControls(false);
            this.CleanGroup();
            this._noLoaded = false;
            
        }
        #endregion

        #region Variables Formulario

        private BaseController _bc = BaseController.GetInstance();

        private FormTypes _frmType = FormTypes.Document;
        private string _documento = string.Empty;
        private bool selected = false;
        private int _tipoMvto = 0;
        private List<DTO_acActivoControl> _activo;
        private bool isValid;
        private DTO_acMovimientoTipo _Mto = null;
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
            this.gcDocument.DataSource = this._activo;
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.MovimientosActivos;
            InitializeComponent();

            base.SetInitParameters();
            this.tlSeparatorPanel.RowStyles[0].Height = 80;

            this.AddGridCols();

            //Inicializa los delegados
            this.refresGrd = new RefreshGrid(RefreshGridMethod);

            //Carga info del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.ac;

            //Cargar los Controles de Mestras
            _bc.InitMasterUC(this.MasterCcostMntoFiltro, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.MasterCCostoCampoMnto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.MasterCCostoFilResponsable, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.MasterCctoFiltTrasla, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.MasterCentroCostoCampoTras, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.MasterLocFCampMnto, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.MasterLocFFilResponsable, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.MasterLocFisicaCampoTras, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.MasterlocFiTrasladoFiltro, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.masterLocFMntoFiltro, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.MasterProyectoCampMnto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.MasterProyectoCampoTras, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.MasterProyectoFilResponsable, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.MasterProyectoFiltroTraslado, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.MasterProyeMntoFiltro, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.MasterRespCampoAsResp, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.MasterRespMntCampo, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.MasterMvtotipo, AppMasters.acMovimientoTipo, true, true, true, false);
            _bc.InitMasterUC(this.MasterFiltroMnoRespons, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.MasterFilAsRespResponsable, AppMasters.coTercero, true, true, true, false);

            ////Deshabilita los botones +- de la grilla
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
                plaquetaID.Width = 150;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 100;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(serialID);

                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 3;
                inReferenciaID.Width = 150;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descripcion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 4;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descripcion);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 5;
                locFisica.Width = 120;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(locFisica);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 6;
                proyecto.Width = 120;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(proyecto);


                //centroCosto
                GridColumn centroCosto = new GridColumn();
                centroCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                centroCosto.UnboundType = UnboundColumnType.String;
                centroCosto.VisibleIndex = 7;
                centroCosto.Width = 120;
                centroCosto.Visible = true;
                centroCosto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(centroCosto);

                //EstadoActID
                GridColumn estadoActivo = new GridColumn();
                estadoActivo.FieldName = this.unboundPrefix + "EstadoActID";
                estadoActivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoActID");
                estadoActivo.UnboundType = UnboundColumnType.String;
                estadoActivo.VisibleIndex = 8;
                estadoActivo.Width = 120;
                estadoActivo.Visible = true;
                estadoActivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(estadoActivo);

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
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Limpia los controles del header
        /// </summary>
        private void CleanControls(bool first)
        {
            // limpia los controles de acuerdo al tipo de movimiento.           
            this.txtPlaqueta.Text = string.Empty;
            this.txtPlaquetaFilResponsable.Text = string.Empty;
            this.MasterFilAsRespResponsable.Value = string.Empty;
            this.MasterFiltroMnoRespons.Value = string.Empty;
            this.txtSerial.Text = string.Empty;
            this.txtSerialFilResponsable.Text = string.Empty;
            this.MasterCcostMntoFiltro.Value = string.Empty;
            this.MasterCCostoCampoMnto.Value = string.Empty;
            this.MasterCCostoFilResponsable.Value = string.Empty;
            this.MasterCctoFiltTrasla.Value = string.Empty;
            this.masterLocFMntoFiltro.Value = string.Empty;
            this.MasterProyectoCampMnto.Value = string.Empty;
            this.MasterProyectoCampoTras.Value = string.Empty;
            this.MasterProyectoFilResponsable.Value = string.Empty;
            this.MasterProyectoFiltroTraslado.Value = string.Empty;
            this.MasterProyeMntoFiltro.Value = string.Empty;
            this.MasterLocFCampMnto.Value = string.Empty;
            this.MasterLocFFilResponsable.Value = string.Empty;
            this.MasterLocFisicaCampoTras.Value = string.Empty;
            this.MasterlocFiTrasladoFiltro.Value = string.Empty;
            this.MasterRespMntCampo.Value = string.Empty;
            this.MasterCentroCostoCampoTras.Value = string.Empty;
            if (!first)
                this.MasterMvtotipo.Value = string.Empty;
            
            // LImpia la grilla
            this._activo = new List<DTO_acActivoControl>();
            this.gcDocument.DataSource = this._activo;
        }

        /// <summary>
        /// Verifica la informacion del DTO_acActivoControl
        /// </summary>
        /// <returns>DTO_TxResult</returns>
        private DTO_TxResult ValidarDataSearch(bool isSearch)
        {
            bool validFK = false;
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            
            try
            {
                result.Details = new List<DTO_TxResultDetail>();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                DTO_TxResultDetailFields drF = new DTO_TxResultDetailFields();

                string colCtroCosto = string.Empty;
                string colPlaquetaID = string.Empty;
                string colAcClase = string.Empty;
                string colAcGrupo = string.Empty;
                string colTipo = string.Empty;
                string colProyecto = string.Empty;
                string colLocFisica = string.Empty;
                string colInRefID = string.Empty;

                if (isSearch)
                {
                    #region Movimiento Traslado
                    if (this._tipoMvto == 4)
                    {
                        if (string.IsNullOrWhiteSpace(this.MasterlocFiTrasladoFiltro.Value)
                            && string.IsNullOrWhiteSpace(this.MasterCctoFiltTrasla.Value)
                            && string.IsNullOrWhiteSpace(this.MasterProyectoFiltroTraslado.Value)
                            && string.IsNullOrWhiteSpace(this.txtSerial.Text)
                            && string.IsNullOrWhiteSpace(this.txtPlaqueta.Text))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = (_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected));
                            this.CleanControls(true);
                        }
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    #endregion
                    #region Mantenimiento
                    if (this._tipoMvto == 8)
                    {
                        if (string.IsNullOrWhiteSpace(this.MasterFiltroMnoRespons.Value)
                            && string.IsNullOrWhiteSpace(this.txtSerialFilMnto.Text)
                            && string.IsNullOrWhiteSpace(this.masterLocFMntoFiltro.Value)
                            && string.IsNullOrWhiteSpace(this.MasterProyeMntoFiltro.Value)
                            && string.IsNullOrWhiteSpace(this.MasterCcostMntoFiltro.Value)
                            && string.IsNullOrWhiteSpace(this.txtPlaqFilMnto.Text))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = (_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected));
                            this.CleanControls(true);
                        }
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    #endregion
                    #region Asignacion de responsable
                    if (this._tipoMvto == 11)
                    {
                        if (string.IsNullOrWhiteSpace(this.txtPlaquetaFilResponsable.Text)
                            && string.IsNullOrWhiteSpace(this.txtSerialFilResponsable.Text)
                            && string.IsNullOrWhiteSpace(this.MasterLocFFilResponsable.Value)
                            && string.IsNullOrWhiteSpace(this.MasterProyectoFilResponsable.Value)
                            && string.IsNullOrWhiteSpace(this.MasterCCostoFilResponsable.Value)
                            && string.IsNullOrWhiteSpace(this.MasterFilAsRespResponsable.Value))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = (_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected));
                            this.CleanControls(true);
                        }
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    #endregion
                }
                else
                {
                    #region Movimiento traslado
                    #region Traslado fks y null
                    if (this._tipoMvto == 4)
                    {
                        if (!string.IsNullOrWhiteSpace(this.MasterLocFisicaCampoTras.Value))
                        {
                            colLocFisica = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                            drF = _bc.ValidGridCell(colLocFisica, this.MasterLocFisicaCampoTras.Value, false, true, true, AppMasters.glLocFisica);
                            if (drF != null)
                                rd.DetailsFields.Add(drF);
                            else
                                FormProvider.Master.itemSearch.Enabled = true;
                        }
                        if (!string.IsNullOrWhiteSpace(this.MasterCentroCostoCampoTras.Value))
                        {
                            colCtroCosto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                            drF = _bc.ValidGridCell(colCtroCosto, this.MasterCentroCostoCampoTras.Value, false, true, true, AppMasters.coCentroCosto);
                            if (drF != null)
                                rd.DetailsFields.Add(drF);
                            else
                                FormProvider.Master.itemSearch.Enabled = true;
                        }

                        if (!string.IsNullOrWhiteSpace(this.MasterProyectoCampoTras.Value))
                        {
                            colProyecto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                            drF = _bc.ValidGridCell(colProyecto, this.MasterProyectoCampoTras.Value, false, true, true, AppMasters.coProyecto);
                            if (drF != null)
                                rd.DetailsFields.Add(drF);
                            else
                                FormProvider.Master.itemSearch.Enabled = true;
                        }
                    }
                    #endregion
                    #endregion
                    #region Movimiento Mantenimiento
                    #region Mantnimeitnto fks y null
                    if (!string.IsNullOrWhiteSpace(this.MasterLocFCampMnto.Value))
                    {
                        colLocFisica = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                        drF = _bc.ValidGridCell(colLocFisica, this.MasterLocFCampMnto.Value, false, true, true, AppMasters.glLocFisica);
                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    if (!string.IsNullOrWhiteSpace(this.MasterProyectoCampMnto.Value))
                    {
                        colProyecto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                        drF = _bc.ValidGridCell(colProyecto, this.MasterProyectoCampMnto.Value, false, true, true, AppMasters.coProyecto);
                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    if (!string.IsNullOrWhiteSpace(this.MasterCCostoCampoMnto.Value))
                    {
                        colCtroCosto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                        drF = _bc.ValidGridCell(colCtroCosto, this.MasterCCostoCampoMnto.Value, false, true, true, AppMasters.coCentroCosto);
                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    #endregion
                    #endregion
                    #region Movimiento Asignacion de Responsable
                    if (this._tipoMvto == 11)
                    {
                        if (string.IsNullOrWhiteSpace(this.MasterRespCampoAsResp.Value))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = (_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Err_Ac_InvalidInfo));
                        }
                        else
                            FormProvider.Master.itemSearch.Enabled = true;
                    }
                    #endregion
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
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                return result;
            }
        }
        
        /// <summary>
        /// Limpia los groupbox
        /// </summary>
        private void CleanGroup()
        {
            // oculta los groupBox
            //Traslados
            this.grbCamposTraslados.Visible = false;
            this.grbFiltrosTraslados.Visible = false;
            //Mantenimiento
            this.grbCamposMnto.Visible = false;
            this.grbMantenimiento.Visible = false;
            //Asignacion de Responsable
            this.grbAsignacionResp.Visible = false;
            this.grbAsignRespon.Visible = false;
        }

        #endregion

        #region Funciones MDI

        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);
                //Manejo de Botones de la Barra de Herramientas
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Enabled = false;
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                //Boton guardar
                FormProvider.Master.itemSave.Enabled = true;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);

                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;

                FormProvider.Master.itemSearch.Enabled = false;
                FormProvider.Master.itemUpdate.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Enter"));
            }
        }
        #endregion

        #region Eventos header
        /// <summary>
        /// Habilita los controles del header de acuerdo al tipo de mvto
        /// </summary>
        private void MasterMvtotipo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.MasterMvtotipo.Value))
                return;
            if (!string.IsNullOrWhiteSpace(this.MasterMvtotipo.Value))
                FormProvider.Master.itemSearch.Enabled = true;

            string mvtoTipoMaster = this.MasterMvtotipo.Value;
            this._Mto = (DTO_acMovimientoTipo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.acMovimientoTipo, new UDT_BasicID() { Value = this.MasterMvtotipo.Value }, true);

            if (this._Mto.TipoMvto.Value == (int)TipoMvtoTraslado.Traslado)
            {
                this._tipoMvto = 4;
                this.txtEstado.Text = this._Mto.EstadoActID.Value;

                this.grbMantenimiento.Visible = false;
                this.grbCamposMnto.Visible = false;
                this.grbAsignacionResp.Visible = false;
                this.grbAsignRespon.Visible = false;

                this.grbCamposTraslados.Visible = true;
                this.grbFiltrosTraslados.Visible = true;
                this.selected = true;
            }
            if (this._Mto.TipoMvto.Value == (int)TipoMvtoTraslado.Mantenimiento)
            {
                this._tipoMvto = 8;
                this.txtEstado.Text = this._Mto.EstadoActID.Value;

                this.grbCamposTraslados.Visible = false;
                this.grbFiltrosTraslados.Visible = false;
                this.grbAsignacionResp.Visible = false;
                this.grbAsignRespon.Visible = false;

                this.grbMantenimiento.Visible = true;
                this.grbCamposMnto.Visible = true;
                this.selected = true;
            }
            if (this._Mto.TipoMvto.Value == (int)TipoMvtoTraslado.AsignacionResponsable)
            {
                this._tipoMvto = 11;
                this.txtEstado.Text = this._Mto.EstadoActID.Value;

                this.grbMantenimiento.Visible = false;
                this.grbCamposMnto.Visible = false;
                this.grbCamposTraslados.Visible = false;
                this.grbFiltrosTraslados.Visible = false;

                this.grbAsignacionResp.Visible = true;
                this.grbAsignRespon.Visible = true;
                this.selected = true;
            }
            if (this._Mto.TipoMvto.Value != (int)TipoMvtoTraslado.Traslado && this._Mto.TipoMvto.Value != (int)TipoMvtoTraslado.Mantenimiento
               && this._Mto.TipoMvto.Value != (int)TipoMvtoTraslado.AsignacionResponsable)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                this.CleanControls(true);
            }
            else
                this.CleanControls(true);
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para realizar la busqueda.
        /// </summary>
        public override void TBSearch()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            base.TBSearch();
            this.gvDocument.PostEditor();
            result = this.ValidarDataSearch(true);
            if (result.Result == ResultValue.OK)
            {
                #region Traslado
                if (this._tipoMvto == 4)
                {
                    DTO_acActivoControl dtCtrl = new DTO_acActivoControl();

                    dtCtrl.PlaquetaID.Value = this.txtPlaqueta.Text;
                    dtCtrl.SerialID.Value = this.txtSerial.Text;
                    dtCtrl.LocFisicaID.Value = this.MasterlocFiTrasladoFiltro.Value;
                    dtCtrl.ProyectoID.Value = this.MasterProyectoFiltroTraslado.Value;
                    dtCtrl.CentroCostoID.Value = this.MasterCctoFiltTrasla.Value;

                    this._activo = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(dtCtrl);
                    this.LoadData(true);
                }
                #endregion
                #region Mantenimiento
                if (this._tipoMvto == 8)
                {
                    DTO_acActivoControl dtCtrl = new DTO_acActivoControl();

                    dtCtrl.PlaquetaID.Value = this.txtPlaqFilMnto.Text;
                    dtCtrl.SerialID.Value = this.txtSerialFilMnto.Text;
                    dtCtrl.LocFisicaID.Value = this.masterLocFMntoFiltro.Value;
                    dtCtrl.ProyectoID.Value = this.MasterProyeMntoFiltro.Value;
                    dtCtrl.CentroCostoID.Value = this.MasterCcostMntoFiltro.Value;
                    dtCtrl.Responsable.Value = this.MasterFiltroMnoRespons.Value;

                    this._activo = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(dtCtrl);
                    this.LoadData(true);
                }
                #endregion
                #region Asignacion Responsable
                if (this._tipoMvto == 11)
                {
                    DTO_acActivoControl dtCtrl = new DTO_acActivoControl();

                    dtCtrl.PlaquetaID.Value = this.txtPlaquetaFilResponsable.Text;
                    dtCtrl.SerialID.Value = this.txtSerialFilResponsable.Text;
                    dtCtrl.LocFisicaID.Value = this.MasterLocFFilResponsable.Value;
                    dtCtrl.ProyectoID.Value = this.MasterProyectoFilResponsable.Value;
                    dtCtrl.CentroCostoID.Value = this.MasterCCostoFilResponsable.Value;
                    dtCtrl.Responsable.Value = this.MasterFilAsRespResponsable.Value;

                    this._activo = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(dtCtrl);
                    this.LoadData(true);
                }
                #endregion

                if (this._activo.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = (_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    MessageForm mform = new MessageForm(result);
                    mform.ShowDialog();
                }
            }
            else
            {
                MessageForm mform = new MessageForm(result);
                mform.ShowDialog();
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result = this.ValidarDataSearch(false);
                if (this.select.Count == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoSelected));
                    return;
                }
                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = (_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Err_Ac_InvalidInfo));
                    return;
                }
                base.TBSave();
                this.gvDocument.PostEditor();
                Thread process = new Thread(this.SaveThread);
                process.Start();
                this.CleanGroup();
                this._noLoaded = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.select = new List<int>();

                if (this._activo.Count == 0)
                {
                    this.CleanControls(true);
                    this.CleanGroup();
                    this.MasterMvtotipo.Value = string.Empty;

                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                }
                else
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

                    if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.CleanControls(true);
                        this.CleanGroup();
                        this.MasterMvtotipo.Value = string.Empty;

                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Marca")
                this.UpdateTemp(this._activo);
        }

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
        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            bool isEmpty = false;
            try
            {
                string mvtTipo = this.MasterMvtotipo.Value;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                List<DTO_acActivoControl> listAcCtrl = new List<DTO_acActivoControl>();
                List<DTO_TxResult> results = new List<DTO_TxResult>();

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int i = 0;
                foreach (int index in this.select)
                {
                    DTO_acActivoControl acCtrl = this._activo[i];
                    //Valida que tenga info y la actualiza.
                    #region Moviemiento Traslado
                    if (this._tipoMvto == 4)
                    {
                        #region LocFisica
                        if (!string.IsNullOrWhiteSpace(this.MasterLocFisicaCampoTras.Value))
                        {
                            acCtrl.LocFisicaID.Value = this.MasterLocFisicaCampoTras.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        #region Proyecto
                        if (!string.IsNullOrWhiteSpace(this.MasterProyectoCampoTras.Value))
                        {
                            acCtrl.ProyectoID.Value = this.MasterProyectoCampoTras.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;

                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        #region CentroCosto
                        if (!string.IsNullOrWhiteSpace(this.MasterCentroCostoCampoTras.Value))
                        {
                            acCtrl.CentroCostoID.Value = this.MasterCentroCostoCampoTras.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        FormProvider.Master.StopProgressBarThread(this.documentID);
                    }
                    #endregion
                    #region Movimiento Mantenimiento
                    if (this._tipoMvto == 8)
                    {
                        #region Respnsable
                        if (!string.IsNullOrWhiteSpace(this.MasterRespMntCampo.Value))
                        {
                            acCtrl.Responsable.Value = this.MasterRespMntCampo.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        #region LocFisica
                        if (!string.IsNullOrWhiteSpace(this.MasterLocFCampMnto.Value))
                        {
                            acCtrl.LocFisicaID.Value = this.MasterLocFCampMnto.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        #region Proyecto
                        if (!string.IsNullOrWhiteSpace(this.MasterProyectoCampMnto.Value))
                        {
                            acCtrl.ProyectoID.Value = this.MasterProyectoCampMnto.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        #region CentroCostoID
                        if (!string.IsNullOrWhiteSpace(this.MasterCCostoCampoMnto.Value))
                        {
                            acCtrl.CentroCostoID.Value = this.MasterCCostoCampoMnto.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                        #endregion
                        FormProvider.Master.StopProgressBarThread(this.documentID);
                    }
                    #endregion
                    #region Movimiento Asignacion de Responsable
                    #region Responsable
                    if (this._tipoMvto == 11)
                    {
                        if (!string.IsNullOrWhiteSpace(this.MasterRespCampoAsResp.Value))
                        {
                            acCtrl.Responsable.Value = this.MasterRespCampoAsResp.Value;
                            acCtrl.EstadoActID.Value = this.txtEstado.Text;
                            listAcCtrl.Add(this._activo[i]);
                            int acID = (int)listAcCtrl[i].ActivoID.Value;
                            results = _bc.AdministrationModel.acActivoControl_TrasladoActivos(this.documentID, listAcCtrl, acID, prefijoID, mvtTipo, Convert.ToDateTime(this.dtFecha.Text));

                            foreach (DTO_TxResult result in results)
                            {
                                if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                                    resultsNOK.Add(result);
                            }
                        }
                    }
                    #endregion
                    FormProvider.Master.StopProgressBarThread(this.documentID);
                    #endregion
                    i++;
                }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (resultsNOK.Count == 0)
                {
                    this.newDoc = true;
                    this.deleteOP = true;
                    this._activo = new List<DTO_acActivoControl>();
                    this.Invoke(this.saveDelegate);
                    this.Invoke(this.refresGrd);
                }
                else
                {
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