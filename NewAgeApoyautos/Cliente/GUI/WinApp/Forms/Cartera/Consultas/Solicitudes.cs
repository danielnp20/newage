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
using System.Diagnostics;
using System.Net;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Solicitudes : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int libranza = 0;
        private string _numeroLibranza = string.Empty;
        private object currentDoc = null;
        private int docRecursos = AppDocuments.VerificacionPreliminar;
        //Para manejo de propiedades
        protected List<DTO_ccSolicitudAnexo> anexosAll = new List<DTO_ccSolicitudAnexo>();
        protected List<DTO_ccSolicitudAnexo> anexos = new List<DTO_ccSolicitudAnexo>();
        protected List<DTO_ccTareaChequeoLista> tareas = new List<DTO_ccTareaChequeoLista>();
        protected List<DTO_ccTareaChequeoLista> tareasAll = new List<DTO_ccTareaChequeoLista>();
        protected List<DTO_ccSolicitudDevolucion> devoluciones = new List<DTO_ccSolicitudDevolucion>();
        protected int documentID;
        private int numeroDoc = 0;
        DTO_SolicitudLibranza solicitud = null;
        private SectorCartera sector = SectorCartera.Solidario;
        protected string unboundPrefix = "Unbound_";
               
        #endregion

        #region Constructor Solicitudes

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Solicitudes()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Solicitudes(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "Solicitudes.cs-Solicitudes"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.ConsultaSolicitud;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.mfCliente, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.mfLinea, AppMasters.ccLineaCredito, true, true, true, false);
            this._bc.InitMasterUC(this.mfTipoCredito, AppMasters.ccTipoCredito, true, true, true, false);
            this._bc.InitMasterUC(this.mfTarea, AppMasters.seUsuario, false, true, true, false);

            //Deshabilita los controles de maestras
            this.mfCliente.EnableControl(false);
            this.mfLinea.EnableControl(false);
            this.mfTipoCredito.EnableControl(false);
            this.lbTarea.Enabled.Equals(false);

            //Deshabilita los botones +- de la grilla
            this.gcGenerales.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            string sectorStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            if (!string.IsNullOrWhiteSpace(sectorStr) && sectorStr != "0")
                sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorStr);
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columna Grilla Principal

                //IncluidoInd
                GridColumn IncluidoInd = new GridColumn();
                IncluidoInd.FieldName = this.unboundPrefix + "IncluidoInd";
                IncluidoInd.Caption = "√";
                IncluidoInd.UnboundType = UnboundColumnType.Boolean;
                IncluidoInd.VisibleIndex = 0;
                IncluidoInd.Width = 15;
                IncluidoInd.Visible = true;
                IncluidoInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_IncluidoInd");
                IncluidoInd.AppearanceHeader.ForeColor = Color.Lime;
                IncluidoInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                IncluidoInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                IncluidoInd.AppearanceHeader.Options.UseTextOptions = true;
                IncluidoInd.AppearanceHeader.Options.UseFont = true;
                IncluidoInd.AppearanceHeader.Options.UseForeColor = true;
                this.gvGenerales.Columns.Add(IncluidoInd);

                //Campo de Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 2;
                Descriptivo.Width = 70;
                Descriptivo.Visible = true;
                Descriptivo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(Descriptivo);

                //Campo de DescripTExt
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "Descripcion";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Descripcion");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 3;
                DescripTExt.Width = 200;
                DescripTExt.Visible = true;
                DescripTExt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                DescripTExt.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(DescripTExt);

                #endregion
                #region Columna No Visibles Grilla Principal

                //FechaDEV
                GridColumn FechaDEV = new GridColumn();
                FechaDEV.FieldName = this.unboundPrefix + "FechaDEV";
                FechaDEV.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_FechaDEV");
                FechaDEV.UnboundType = UnboundColumnType.DateTime;
                FechaDEV.VisibleIndex = 1;
                FechaDEV.Width = 70;
                FechaDEV.Visible = false;
                FechaDEV.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                FechaDEV.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(FechaDEV);

                //FechaRAD
                GridColumn FechaRAD = new GridColumn();
                FechaRAD.FieldName = this.unboundPrefix + "FechaRAD";
                FechaRAD.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_FechaRAD");
                FechaRAD.UnboundType = UnboundColumnType.DateTime;
                FechaRAD.VisibleIndex = 2;
                FechaRAD.Width = 70;
                FechaRAD.Visible = false;
                FechaRAD.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                FechaRAD.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(FechaRAD);

                //UsuarioID
                GridColumn UsuarioID = new GridColumn();
                UsuarioID.FieldName = this.unboundPrefix + "UsuarioID";
                UsuarioID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_UsuarioID");
                UsuarioID.UnboundType = UnboundColumnType.String;
                UsuarioID.VisibleIndex = 3;
                UsuarioID.Width = 100;
                UsuarioID.Visible = false;
                UsuarioID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                UsuarioID.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(UsuarioID);

                //UsuarioDesc
                GridColumn UsuarioDesc = new GridColumn();
                UsuarioDesc.FieldName = this.unboundPrefix + "UsuarioDesc";
                UsuarioDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_UsuarioDesc");
                UsuarioDesc.UnboundType = UnboundColumnType.String;
                UsuarioDesc.VisibleIndex = 4;
                UsuarioDesc.Width = 100;
                UsuarioDesc.Visible = false;
                UsuarioDesc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                UsuarioDesc.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(UsuarioDesc);

                //ActividadFlujoID
                GridColumn ActividadFlujoID = new GridColumn();
                ActividadFlujoID.FieldName = this.unboundPrefix + "ActividadFlujoID";
                ActividadFlujoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ActividadFlujoID");
                ActividadFlujoID.UnboundType = UnboundColumnType.String;
                ActividadFlujoID.VisibleIndex = 5;
                ActividadFlujoID.Width = 100;
                ActividadFlujoID.Visible = false;
                ActividadFlujoID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                ActividadFlujoID.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(ActividadFlujoID);

                //ActividadFlujoDesc
                GridColumn ActividadFlujoDesc = new GridColumn();
                ActividadFlujoDesc.FieldName = this.unboundPrefix + "ActividadFlujoDesc";
                ActividadFlujoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ActividadFlujoDesc");
                ActividadFlujoDesc.UnboundType = UnboundColumnType.String;
                ActividadFlujoDesc.VisibleIndex = 6;
                ActividadFlujoDesc.Width = 100;
                ActividadFlujoDesc.Visible = false;
                ActividadFlujoDesc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                ActividadFlujoDesc.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(ActividadFlujoDesc);

                #endregion
                #region Columnas Grilla Detalle

                //DevCausalID
                GridColumn DevCausalID = new GridColumn();
                DevCausalID.FieldName = this._unboundPrefix + "DevCausalID";
                DevCausalID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_DevCausalID");
                DevCausalID.UnboundType = UnboundColumnType.String;
                DevCausalID.VisibleIndex = 1;
                DevCausalID.Width = 40;
                DevCausalID.Visible = true;
                DevCausalID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DevCausalID);

                //DevCausalDesc
                GridColumn DevCausalDesc = new GridColumn();
                DevCausalDesc.FieldName = this._unboundPrefix + "DevCausalDesc";
                DevCausalDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_DevCausalDesc");
                DevCausalDesc.UnboundType = UnboundColumnType.String;
                DevCausalDesc.VisibleIndex = 2;
                DevCausalDesc.Width = 150;
                DevCausalDesc.Visible = true;
                DevCausalDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DevCausalDesc);

                //DevCausalGrupoID
                GridColumn DevCausalGrupoID = new GridColumn();
                DevCausalGrupoID.FieldName = this._unboundPrefix + "DevCausalGrupoID";
                DevCausalGrupoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_DevCausalGrupoID");
                DevCausalGrupoID.UnboundType = UnboundColumnType.String;
                DevCausalGrupoID.VisibleIndex = 3;
                DevCausalGrupoID.Width = 20;
                DevCausalGrupoID.Visible = false;
                DevCausalGrupoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DevCausalGrupoID);

                //DevCausalGrupoDesc
                GridColumn DevCausalGrupoDesc = new GridColumn();
                DevCausalGrupoDesc.FieldName = this._unboundPrefix + "DevCausalGrupoDesc";
                DevCausalGrupoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_DevCausalGrupoDesc");
                DevCausalGrupoDesc.UnboundType = UnboundColumnType.String;
                DevCausalGrupoDesc.VisibleIndex = 4;
                DevCausalGrupoDesc.Width = 70;
                DevCausalGrupoDesc.Visible = true;
                DevCausalGrupoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DevCausalGrupoDesc);

                //Observaciones
                GridColumn Observaciones = new GridColumn();
                Observaciones.FieldName = this._unboundPrefix + "Observaciones";
                Observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Observaciones");
                Observaciones.UnboundType = UnboundColumnType.String;
                Observaciones.VisibleIndex = 5;
                Observaciones.Width = 200;
                Observaciones.Visible = true;
                Observaciones.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Observaciones); 
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudess.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            //Header
            this.txtLibranza.EditValue = string.Empty;
            this.mfCliente.Value = string.Empty;
            this.mfLinea.Value = string.Empty;
            this.mfTipoCredito.Value = string.Empty;
            this.mfTarea.Text = string.Empty;
            this.txtValor.Text = string.Empty;
            this.txtPlazo.Text = string.Empty;
            this.lbTarea.Text = string.Empty;
            this.libranza = 0;
            this._numeroLibranza = string.Empty;
            this.numeroDoc = 0;

            //Botones
            this.btnAnexos.Enabled = false;
            this.btnAnalisis.Enabled = false;
            this.btnLiquidacion.Enabled = false;
            this.btnReferencia.Enabled = false;
            this.btnDocumento.Enabled = false;
            this.btnDevolucion.Enabled = false;

            // Grilla
            this.anexos = new List<DTO_ccSolicitudAnexo>();
            this.tareas = new List<DTO_ccTareaChequeoLista>();
            this.devoluciones = new List<DTO_ccSolicitudDevolucion>();
            this.gcGenerales.DataSource = null;
            this.gcGenerales.RefreshDataSource();
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanColumns(bool isDevolucion)
        {
            try
            {
                if (isDevolucion)
                {
                    this.gvGenerales.Columns[this._unboundPrefix + "FechaDEV"].VisibleIndex = 0;
                    this.gvGenerales.Columns[this._unboundPrefix + "FechaRAD"].VisibleIndex = 1;
                    this.gvGenerales.Columns[this._unboundPrefix + "UsuarioID"].VisibleIndex = 2;
                    this.gvGenerales.Columns[this._unboundPrefix + "UsuarioDesc"].VisibleIndex = 3;
                    this.gvGenerales.Columns[this._unboundPrefix + "ActividadFlujoID"].VisibleIndex = 4;
                    this.gvGenerales.Columns[this._unboundPrefix + "ActividadFlujoDesc"].VisibleIndex = 5;

                    this.gvGenerales.Columns[this._unboundPrefix + "FechaDEV"].Visible = true;
                    this.gvGenerales.Columns[this._unboundPrefix + "FechaRAD"].Visible = true;
                    this.gvGenerales.Columns[this._unboundPrefix + "UsuarioID"].Visible = true;
                    this.gvGenerales.Columns[this._unboundPrefix + "UsuarioDesc"].Visible = true;
                    this.gvGenerales.Columns[this._unboundPrefix + "ActividadFlujoID"].Visible = true;
                    this.gvGenerales.Columns[this._unboundPrefix + "ActividadFlujoDesc"].Visible = true;

                    this.gvGenerales.Columns[this._unboundPrefix + "IncluidoInd"].Visible = false;
                    this.gvGenerales.Columns[this._unboundPrefix + "Descriptivo"].Visible = false;
                    this.gvGenerales.Columns[this._unboundPrefix + "Descripcion"].Visible = false;
                }
                else
                {
                    this.gvGenerales.Columns[this.unboundPrefix + "IncluidoInd"].VisibleIndex = 0;
                    this.gvGenerales.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 1;
                    this.gvGenerales.Columns[this.unboundPrefix + "Descripcion"].VisibleIndex = 2;

                    this.gvGenerales.Columns[this.unboundPrefix + "IncluidoInd"].Visible = true;
                    this.gvGenerales.Columns[this.unboundPrefix + "Descriptivo"].Visible = true;
                    this.gvGenerales.Columns[this.unboundPrefix + "Descripcion"].Visible = true;

                    this.gvGenerales.Columns[this.unboundPrefix + "FechaDEV"].Visible = false;
                    this.gvGenerales.Columns[this.unboundPrefix + "FechaRAD"].Visible = false;
                    this.gvGenerales.Columns[this.unboundPrefix + "UsuarioID"].Visible = false;
                    this.gvGenerales.Columns[this.unboundPrefix + "UsuarioDesc"].Visible = false;
                    this.gvGenerales.Columns[this.unboundPrefix + "ActividadFlujoID"].Visible = false;
                    this.gvGenerales.Columns[this.unboundPrefix + "ActividadFlujoDesc"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "chkRecursoXTrabInd_CheckedChanged"));
            }
        }

        #endregion Funciones Privadas

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
                FormProvider.Master.itemUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos formulario

        /// <summary>
        /// Evento para la logica de los controles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtLibranza.Text))
                {
                    if (this.libranza != Convert.ToInt32(this.txtLibranza.EditValue))
                    {
                        this.libranza = Convert.ToInt32(this.txtLibranza.EditValue);
                        this.solicitud = this._bc.AdministrationModel.SolicitudLibranza_GetByLibranza(libranza, string.Empty);

                        if (this.solicitud.DocCtrl == null)
                        {
                            this.btnAnexos.Enabled = false;
                            this.btnAnalisis.Enabled = false;
                            this.btnLiquidacion.Enabled = false;
                            this.btnReferencia.Enabled = false;
                            this.btnDocumento.Enabled = false;
                            this.btnDevolucion.Enabled = false;
                            this.mfTarea.Enabled = false;

                            MessageBox.Show("La libranza " + this.libranza + " no existe.", "Error en la busqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.CleanData();
                        }
                        else
                        {
                            this._numeroLibranza = Convert.ToString(this.solicitud.DocCtrl.NumeroDoc.Value.Value);
                            string estado = this._bc.AdministrationModel.EstadoSolicitud_GetByNumeroDoc(this._numeroLibranza);

                            this.btnAnexos.Enabled = true;
                            this.btnAnalisis.Enabled = true;
                            this.btnLiquidacion.Enabled = true;
                            this.btnReferencia.Enabled = true;
                            this.btnDocumento.Enabled = true;
                            this.btnDevolucion.Enabled = true;
                            this.mfTarea.Enabled = true;

                            this.numeroDoc = this.solicitud.DocCtrl.NumeroDoc.Value.Value;
                            this.mfCliente.Value = this.solicitud.Header.ClienteID.Value;
                            this.mfLinea.Value = this.solicitud.Header.LineaCreditoID.Value;
                            this.mfTipoCredito.Value = this.solicitud.Header.TipoCreditoID.Value;
                            this.mfTarea.Value = this.solicitud.Header.AnalisisUsuario.Value;
                            this.txtValor.EditValue = this.solicitud.DocCtrl.Valor.Value.ToString();
                            this.txtPlazo.Text = this.solicitud.Header.Plazo.Value.ToString();
                            this.lbTarea.Text = estado.ToString();
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Abre la info de devoluciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.numeroDoc);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "btnDocumento_Click"));
            }
        }

        /// <summary>
        /// Abre la info de devoluciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnDevolucion_Click(object sender, EventArgs e)
        {
            try
            {
                this.CleanColumns(true);

                this.devoluciones = this._bc.AdministrationModel.DevolucionSolicitud_GetByNumeroDoc(this.documentID,this.numeroDoc);

                this.gcGenerales.DataSource = this.devoluciones;
                this.gcGenerales.RefreshDataSource();
            }
            catch (Exception ex)
            {                
                throw ex;
            }           
        }

        /// <summary>
        /// Abre la info de devoluciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAnexos_Click(object sender, EventArgs e)
        {
            try
            {
                this.CleanColumns(false);
                this.anexos = _bc.AdministrationModel.SolicitudLibranza_GetAnexosByID(this.numeroDoc);
                this.gcGenerales.DataSource = this.anexos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "btnAnexos_Click"));
            }
        }

        /// <summary>
        /// Abre la info de devoluciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            try
            {
                this.CleanColumns(false);
                this.tareas = new List<DTO_ccTareaChequeoLista>();

                List<DTO_MasterBasic> chequeos = _bc.AdministrationModel.ccChequeoLista_GetByDocumento(AppDocuments.AnalisisRiesgo);
                foreach (DTO_MasterBasic basic in chequeos)
                {
                    DTO_ccTareaChequeoLista chequeo = new DTO_ccTareaChequeoLista();
                    chequeo.NumeroDoc.Value = numeroDoc;
                    chequeo.TareaID.Value = basic.ID.Value;
                    chequeo.Descriptivo.Value = basic.Descriptivo.Value;
                    chequeo.IncluidoInd.Value = false;

                    this.tareas.Add(chequeo);
                }                
                this.gcGenerales.DataSource = this.tareas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "btnAnalisis_Click"));
            }
        }

        /// <summary>
        ///  Abre la pantalla de Digitacion de Crédito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLiquidacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (sector != SectorCartera.Financiero)
                {
                    Type frmDigitacionCr = typeof(DigitacionCredito);
                    FormProvider.GetInstance(frmDigitacionCr, new object[] { this.libranza, this._frmModule.ToString() });
                }
                else
                {
                    Type frmDigitacionCr = typeof(DigitacionCreditoFinanciera);
                    FormProvider.GetInstance(frmDigitacionCr, new object[] { this.libranza, this._frmModule.ToString() });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "btnLiquidacion_Click"));
            }

        }

        /// <summary>
        /// Abre la pantalla de referenciacion de solicitudes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReferencia_Click(object sender, EventArgs e)
        {
            try
            {
                Type frmReferenciacion = typeof(Referenciacion);
                FormProvider.GetInstance(frmReferenciacion, new object[] { this.libranza, this._frmModule.ToString() });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "btnReferencia_Click"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
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
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = true;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
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

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.solicitud = new DTO_SolicitudLibranza();
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "TBNew"));
            }
        }

        /// <summary>
        ///  Boton para salvar el registro
        /// </summary>
        public override void TBSave()
        {            
            try
            {
                if (this.mfTarea.ValidID)
                {
                    DTO_seUsuario user = (DTO_seUsuario)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, false, this.mfTarea.Value, true);
                    this.solicitud.DocCtrl.seUsuarioID.Value = user.ReplicaID.Value;
                    this._bc.AdministrationModel.Solicitud_UpdateUSer(solicitud.DocCtrl);

                    MessageBox.Show("Guardado con exíto","Guardado",MessageBoxButtons.OK,MessageBoxIcon.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitudes.cs", "TBSave"));
            }
        }
      

        #endregion Eventos Barra Herramientas

       
     
    }
}
