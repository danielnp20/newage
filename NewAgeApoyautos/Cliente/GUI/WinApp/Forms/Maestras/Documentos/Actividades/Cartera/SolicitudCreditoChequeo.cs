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
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class SolicitudCreditoChequeo : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public virtual void RefreshDataMethod()
        {
            this.gvDocuments.Columns[2].Visible = false;
            this.currentRow = -1;
            this.LoadDocuments();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        protected int userID = 0;
        //Para manejo de propiedades
        protected string msgAnulado = string.Empty;
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected string unboundPrefix = "Unbound_";
        protected bool multiMoneda;
        protected object currentDoc = null;
        protected bool allowValidate = true;
        protected bool detailsLoaded = false;
        protected bool isOK = true;
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;
        protected string actFlujoTemp = string.Empty;
        protected DTO_glActividadFlujo actividadDTOTemp = null;

        protected int numDetails = 0;
        protected int currentRow = -1;
        protected Dictionary<string, string> actFlujoForReversion = new Dictionary<string, string>();
        protected List<DTO_SolicitudAprobacionCartera> solicitudCredito = new List<DTO_SolicitudAprobacionCartera>();
        protected List<DTO_ccSolicitudAnexo> anexos = new List<DTO_ccSolicitudAnexo>();
        protected List<DTO_ccTareaChequeoLista> tareas = new List<DTO_ccTareaChequeoLista>();
        protected List<DTO_ccSolicitudAnexo> anexosAll = new List<DTO_ccSolicitudAnexo>();
        protected List<DTO_ccTareaChequeoLista> tareasAll = new List<DTO_ccTareaChequeoLista>();
        protected List<string> actividadesCombo = new List<string>();

        //Variables Privadas
        /// <summary>
        /// docRecursos Obtiene el Id del docuemento que Contiene los recursos para remplazarla en todas las pantallas.
        /// </summary>
        private int docRecursos = AppDocuments.VerificacionPreliminar;
        private string _frmName;
        private FormTypes _frmType = FormTypes.DocumentAprob;

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        public SolicitudCreditoChequeo()
        {
            this.Constructor();
        }

        public SolicitudCreditoChequeo(string mod = null)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Información del constructor
        /// </summary>
        public void Constructor(string mod = null)
        {
            try
            {
                InitializeComponent();
                this.documentID = AppDocuments.ActividadChequeo_cc;
                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this.frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);
                
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Carga el rescurso de Anulado
                this.msgAnulado = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado);
                this.msgAnulado = this.msgAnulado.ToUpper();
                //Asigna la lista de columnas
                this.AddDocumentCols();
                this.AddDetailCols();
                this.AddTareasCols();

                this.numDetails = Convert.ToInt32(_bc.GetControlValue(AppControl.PaginadorAprobacionDocumentos));

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (this.documentID == AppDocuments.ActividadChequeo_cc)
                {
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    #region Carga los filtros

                    //Trae solo actividades que no sean del sistema
                    DTO_glConsultaFiltro f1 = new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "SistemaInd",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = false.ToString(),
                        OperadorSentencia = "AND"
                    };
                    filtros.Add(f1);

                    //Trae solo los que son de tipo chequeo
                    DTO_glConsultaFiltro f2 = new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "TipoDocumento",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = ((int)TipoDocumentoActividad.Chequeo).ToString(),
                        OperadorSentencia = "AND"
                    };
                    filtros.Add(f2);

                    //Trae solo as actividades que sean del modulo de cartera
                    DTO_glConsultaFiltro f3 = new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ModuloID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = this.frmModule.ToString().ToLower(),
                        OperadorSentencia = "AND"
                    };
                    filtros.Add(f3);

                    #endregion

                    _bc.InitMasterUC(this.masterActividad, AppMasters.glActividadFlujo, true, true, true, false, filtros);
                }
                else
                {
                    this.masterActividad.Visible = false;
                    if (actividades.Count != 1)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                        MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                    }
                    else
                    {
                        this.actividadFlujoID = actividades[0];
                        this.LoadDocuments();
                    }
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion

                this.AfterInitialize();
                this.refreshData = new RefreshData(RefreshDataMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "SolicitudCreditoChequeo"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        protected void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            this.IsModalFormOpened = true;
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        #endregion

        #region Funciones Virtuales protected

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.frmModule = ModulesPrefix.cc;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() { }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "Rechazado";
                noAprob.Caption = "X";
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 15;
                noAprob.Visible = true;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Campo del combo actividades
                GridColumn cmbActividades = new GridColumn();
                cmbActividades.FieldName = this.unboundPrefix + "ActividadFlujoDesc";
                cmbActividades.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ActividadesFlujo");
                cmbActividades.UnboundType = UnboundColumnType.String;
                cmbActividades.VisibleIndex = 2;
                cmbActividades.Width = 80;
                cmbActividades.Visible = false;
                cmbActividades.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                cmbActividades.OptionsColumn.AllowEdit = true;
                cmbActividades.ColumnEdit = this.editCmb;
                this.gvDocuments.Columns.Add(cmbActividades);

                //Campo de Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this.unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 3;
                Libranza.Width = 40;
                Libranza.Visible = true;
                Libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Libranza);

                //Nombre del Cliente
                GridColumn NombreCliente = new GridColumn();
                NombreCliente.FieldName = this.unboundPrefix + "NombreCliente";
                NombreCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_NombreCliente");
                NombreCliente.UnboundType = UnboundColumnType.String;
                NombreCliente.VisibleIndex = 4;
                NombreCliente.Width = 135;
                NombreCliente.Visible = true;
                NombreCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                NombreCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(NombreCliente);

                //Nombre del Asesor Descripcion
                GridColumn DescripcionAsesor = new GridColumn();
                DescripcionAsesor.FieldName = this.unboundPrefix + "DescripcionAsesor";
                DescripcionAsesor.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_DescripcionAsesor");
                DescripcionAsesor.UnboundType = UnboundColumnType.String;
                DescripcionAsesor.VisibleIndex = 5;
                DescripcionAsesor.Width = 135;
                DescripcionAsesor.Visible = true;
                DescripcionAsesor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                DescripcionAsesor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(DescripcionAsesor);

                //Pagaduria Descripcion
                GridColumn DescripcionPagaduria = new GridColumn();
                DescripcionPagaduria.FieldName = this.unboundPrefix + "DescripcionPagaduria";
                DescripcionPagaduria.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_DescripcionPagaduria");
                DescripcionPagaduria.UnboundType = UnboundColumnType.String;
                DescripcionPagaduria.VisibleIndex = 6;
                DescripcionPagaduria.Width = 100;
                DescripcionPagaduria.Visible = true;
                DescripcionPagaduria.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                DescripcionPagaduria.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(DescripcionPagaduria);

                //Valor Prestamo
                GridColumn VlrPrestamo = new GridColumn();
                VlrPrestamo.FieldName = this.unboundPrefix + "VlrSolicitado";
                VlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrPrestamo");
                VlrPrestamo.UnboundType = UnboundColumnType.Decimal;
                VlrPrestamo.VisibleIndex = 7;
                VlrPrestamo.Width = 70;
                VlrPrestamo.Visible = true;
                VlrPrestamo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrPrestamo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrPrestamo);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 8;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        protected virtual void AddDetailCols()
        {
            try
            {
                //Campo de IncluidoInd
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
                this.gvAnexos.Columns.Add(IncluidoInd);

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
                this.gvAnexos.Columns.Add(Descriptivo);

                //Campo de DescripTExt
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "Descripcion";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Descripcion");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 3;
                DescripTExt.Width = 200;
                DescripTExt.Visible = true;
                DescripTExt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                this.gvAnexos.Columns.Add(DescripTExt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "AddDetailCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla inferior
        /// </summary>
        protected virtual void AddTareasCols()
        {
            try
            {
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
                this.gvTareas.Columns.Add(IncluidoInd);

                //Descripcion
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 1;
                Descriptivo.Width = 70;
                Descriptivo.Visible = true;
                Descriptivo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(Descriptivo);

                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this.unboundPrefix + "Descripcion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Descripcion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 2;
                Observacion.Width = 200;
                Observacion.Visible = true;
                this.gvTareas.Columns.Add(Observacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "AddTareasCols"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected virtual void LoadDocuments()
        {
            try
            {
                this.currentDoc = null;
                DTO_SolicitudAprobacionCartera soliDocu = new DTO_SolicitudAprobacionCartera();
                this.solicitudCredito = this._bc.AdministrationModel.SolicitudLibranza_GetForVerificacion(this.documentID, this.actividadFlujoID);
                this.solicitudCredito = this.solicitudCredito.OrderBy(x => x.Libranza.Value).ToList();
                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this.solicitudCredito.Count > 0)
                {
                    this.detailsLoaded = false;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this.solicitudCredito;
                    this.allowValidate = true;

                    if (!detailsLoaded)
                    {
                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                        this.LoadAnexos();
                        this.LoadTareas();
                    }

                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this.gcAnexos.DataSource = null;
                    this.gcTareas.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga la información de la grilla de Anexos
        /// </summary>
        protected virtual void LoadAnexos()
        {
            try
            {
                DTO_SolicitudAprobacionCartera doc = (DTO_SolicitudAprobacionCartera)this.currentDoc;
                int numeroDoc = doc.NumeroDoc.Value.Value;

                List<DTO_ccSolicitudAnexo> temp =
                    this.anexosAll.Where(x => x.NumeroDoc.Value.Value == numeroDoc).ToList();

                if (temp.Count > 0)
                    this.anexos = temp;
                else
                {
                    this.anexos = _bc.AdministrationModel.SolicitudLibranza_GetAnexosByID(numeroDoc);
                    this.anexosAll.AddRange(this.anexos);
                }

                this.gcAnexos.DataSource = this.anexos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "LoadAnexos"));
            }
        }

        /// <summary>
        /// Carga la informacion de la grilla de Tareas
        /// </summary>
        protected virtual void LoadTareas()
        {
            try
            {
                DTO_SolicitudAprobacionCartera doc = (DTO_SolicitudAprobacionCartera)this.currentDoc;
                int numeroDoc = doc.NumeroDoc.Value.Value;

                List<DTO_ccTareaChequeoLista> temp =
                    this.tareasAll.Where(x => x.NumeroDoc.Value.Value == numeroDoc).ToList();

                if (temp.Count > 0)
                    this.tareas = temp;
                else
                {
                    this.tareas = _bc.AdministrationModel.SolicitudLibranza_GetTareasByNumeroDoc(numeroDoc);
                    this.tareasAll.AddRange(this.tareas);
                }

                this.gcTareas.DataSource = this.tareas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "LoadTareas"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected virtual bool ValidateDocRow(int fila)
        {
            try
            {
                if (this.allowValidate && fila >=0)
                {
                    if (fila != this.gvDocuments.FocusedRowHandle)
                        this.gvDocuments.MoveFirst();
                    string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                    GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
                    bool rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);
                    this.isOK = true;
                    if (rechazado)
                    {
                        #region Valida que la observacion no sea vacia
                        col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                        string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();
                        if (string.IsNullOrEmpty(desc))
                        {
                            string msg = string.Format(rsxEmpty, col.Caption);
                            this.gvDocuments.SetColumnError(col, msg);
                            this.isOK = false;
                        }
                        else
                            this.gvDocuments.SetColumnError(col, string.Empty);
                        #endregion
                        #region Valida que el combo de actividad de flujo no este vacio
                        col = this.gvDocuments.Columns[this.unboundPrefix + "ActividadFlujoDesc"];
                        string desc2 = this.gvDocuments.GetRowCellValue(fila, col).ToString();
                        if (string.IsNullOrEmpty(desc2))
                        {
                            string msg = string.Format(rsxEmpty, col.Caption);
                            this.gvDocuments.SetColumnError(col, msg);
                            isOK = false;
                        }
                        else
                            this.gvDocuments.SetColumnError(col, string.Empty);
                        #endregion
                    }                   
                }
            }
            catch (Exception ex)
            {
                this.isOK = false;
                //MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "ValidateDocRow"));
            }
            return this.isOK;
        }

        #endregion

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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "Form_Leave"));
            }
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "Form_FormClosing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkSeleccionar.Checked)
                {
                    for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                    {
                        this.solicitudCredito[i].Aprobado.Value = true;
                        this.solicitudCredito[i].Rechazado.Value = false;
                    }
                }
                else
                {
                    for (int i = 0; i < gvDocuments.DataRowCount; i++)
                    {
                        this.solicitudCredito[i].Aprobado.Value = false;
                        this.solicitudCredito[i].Rechazado.Value = false;
                    }
                }
                this.gcDocuments.RefreshDataSource();
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "chkSeleccionar_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de seleccon de actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterActividad_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.actividadFlujoID != this.masterActividad.Value)
                {
                    this.actividadFlujoID = this.masterActividad.Value;
                    if (this.masterActividad.ValidID)
                        this.LoadDocuments();
                    else
                    {
                        this.currentDoc = null;
                        this.solicitudCredito = new List<DTO_SolicitudAprobacionCartera>();

                        this.currentRow = -1;
                        this.gcDocuments.DataSource = null;
                        this.gcAnexos.DataSource = null;
                        this.gcTareas.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "masterActividad_Leave"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gcDocuments_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "ActividadFlujoDesc" && e.Value == null)
                {
                    e.Value = ((DTO_SolicitudAprobacionCartera)e.Row).ActividadFlujoDesc;
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
                if (fieldName == "ActividadFlujoDesc")
                {
                    DTO_SolicitudAprobacionCartera a = (DTO_SolicitudAprobacionCartera)e.Row;
                    a.ActividadFlujoDesc = e.Value.ToString();
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
                        else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
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
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewFile);
        }

        /// <summary>
        /// Evento que valida las columna de la grilla despues de editarlas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gcDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (!this.gvDocuments.IsFilterRow(e.RowHandle))
                {
                    //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                    string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                    if (fieldName == "Acierta")
                    {
                        if (e.Value.ToString().Length > 10)
                            this.solicitudCredito[this.currentRow].Acierta.Value = e.Value.ToString().Substring(0, 10);
                    }
                    if (fieldName == "AciertaCifin")
                    {
                        if (e.Value.ToString().Length > 10)
                            this.solicitudCredito[this.currentRow].AciertaCifin.Value = e.Value.ToString().Substring(0, 10);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "gcDocuments_CellValueChanged"));
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = null;
            fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "VlrSolicitado")
                e.RepositoryItem = this.editSpin;

            if (fieldName == "Observacion")
                e.RepositoryItem = this.richText1;
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Observacion")
                {
                    e.RepositoryItem = this.riPopup;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "gvDocuments_CustomRowCellEditForEditing"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
                    e.Allow = false;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (!this.gvDocuments.IsFilterRow(e.FocusedRowHandle))
                {
                    if (this.currentRow != -1)
                    {
                        if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                            this.currentRow = e.FocusedRowHandle;

                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                        this.LoadAnexos();
                        this.LoadTareas();
                        this.detailsLoaded = true;
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "gvDocuments_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected virtual void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!this.gvDocuments.IsFilterRow(e.RowHandle))
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region Generales
                if (fieldName == "Aprobado")
                {
                    if ((bool)e.Value)
                    {
                        this.solicitudCredito[e.RowHandle].Aprobado.Value = true;
                        this.solicitudCredito[e.RowHandle].Rechazado.Value = false;
                        this.solicitudCredito[e.RowHandle].Observacion.Value = string.Empty;
                        List<DTO_SolicitudAprobacionCartera> temp = this.solicitudCredito.Where(x => x.Rechazado.Value == true).ToList();
                        bool visibleON = temp.Count > 0 ? true : false;
                        this.gvDocuments.Columns[2].Visible = visibleON;
                    }
                }

                if (fieldName == "Rechazado")
                {
                    if ((bool)e.Value)
                    {
                        this.gvDocuments.Columns[2].Visible = true;
                        this.solicitudCredito[e.RowHandle].Aprobado.Value = false;
                        this.solicitudCredito[e.RowHandle].Rechazado.Value = true;
                    }
                }

                if (fieldName == "ActividadFlujoDesc")
                {
                    string actFlujoDesc = e.Value.ToString();
                    this.solicitudCredito[this.currentRow].ActividadFlujoDesc = actFlujoDesc;
                    this.solicitudCredito[this.currentRow].ActividadFlujoReversion.Value = this.actFlujoForReversion[actFlujoDesc];
                }

                if (fieldName == "Observacion")
                {
                    this.solicitudCredito[this.currentRow].Observacion.Value = e.Value.ToString();
                }

                #endregion
                this.ValidateDocRow(e.RowHandle);
                this.gcDocuments.RefreshDataSource(); 
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocuments.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambio de opcion en el combo
        /// </summary>
        protected virtual void editCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string actFlujoDesc = ((DTO_SolicitudAprobacionCartera)sender).ActividadFlujoDesc;
            //this.solicitudCredito[this.currentRow].ActividadFlujo.Value = this.actFlujoForReversion[actFlujoDesc];
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos grilla Tareas

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvTareas_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.richTextTareas1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvTareas_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.riPopupTareas;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopupTareas_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvTareas.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descripcion")
                this.richEditControlTareas.Document.Text = this.gvTareas.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopupTareas_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControlTareas.Document.Text;
        }

        #endregion

        #region Eventos grilla anexos

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvAnexos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.richTextAnexos1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvAnexos_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.riPopupAnexos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopupAnexos_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvAnexos.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descripcion")
                this.richEditControlAnexos.Document.Text = this.gvAnexos.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopupAnexos_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControlAnexos.Document.Text;
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.LoadDocuments();
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
            this.gvDocuments.PostEditor();
            try
            {
                List<DTO_SolicitudAprobacionCartera> soliTemp = this.solicitudCredito.Where(x => x.Aprobado.Value == true || x.Rechazado.Value == true).ToList();
                if (soliTemp != null && soliTemp.Count != 0 && this.isOK)
                {
                    this.solicitudCredito = soliTemp;
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                    MessageBox.Show(msg);
                    return;
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
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected virtual void ApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.SolicitudLibranza_AprobarRechazar(documentID, this.actividadFlujoID, this.solicitudCredito, this.anexosAll, this.tareasAll);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                int i = 0;
                int percent = 0;
                #region Variables para el mail

                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this.actividadDTO.seUsuarioID.Value);

                string body = string.Empty;
                string subject = string.Empty;
                string email = user.CorreoElectronico.Value;

                string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                string subjectRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                string bodyRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_RejectedCartera_Body);
                string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                #endregion
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object result in results)
                {
                    #region Manejo de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion
                    DTO_SolicitudAprobacionCartera solAprobacion = this.solicitudCredito[i];
                    if (solAprobacion.Aprobado.Value.Value || solAprobacion.Rechazado.Value.Value)
                    {
                        if (result.GetType() == typeof(DTO_TxResult))
                            resultsNOK.Add((DTO_TxResult)result);
                        else
                        {
                            #region Envia el correo
                            if (solAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, solAprobacion.Libranza.Value, solAprobacion.NombreCliente.Value,
                                  solAprobacion.Observacion.Value);
                            }
                            else if (solAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, solAprobacion.Observacion.Value, solAprobacion.Libranza.Value,
                                    solAprobacion.NombreCliente.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                    }
                    i++;
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "ApprovedThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
