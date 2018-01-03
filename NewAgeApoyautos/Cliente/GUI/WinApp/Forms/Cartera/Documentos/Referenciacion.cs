using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using SentenceTransformer;
using System.Linq;
using DevExpress.XtraEditors;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Referenciacion : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int numeroDoc;
        private bool loaded = false;
        private bool isValid;
        private bool validateData;
        private bool deleteOp;
        private bool editReferencias;

        //DTOs        
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

        //DTOs Grilla Superior
        private DTO_DigitacionCredito _digiCredito = new DTO_DigitacionCredito();
        List<DTO_glTerceroReferencia> _cliRefs = new List<DTO_glTerceroReferencia>();

        //DTOs Grilla inferior
        private List<DTO_glLlamadasControl> _llamadasCtrl = new List<DTO_glLlamadasControl>();
        private List<DTO_glLlamadasControl> _llamadasCtrlAll = new List<DTO_glLlamadasControl>();
        private DTO_glLlamadasControl _llamaCtrl = new DTO_glLlamadasControl();

        //Variables formulario
        private int replicaTemp = 1;
        private string _clienteID;
        private string _libranzaID = string.Empty;

        //Variables ToolBar
        private bool _canSave;

        //Variables operación
        private bool readOnly = false;
        private int indexFila = 0;
        private int currentRow = -1;
        private int refMinimas = 99;
        private string rechazoObs = string.Empty;
        private DTO_glTerceroReferencia currentDoc = null;

        #endregion Variables

        #region Propiedades

        //Numero de una fila segun el indice
        protected int NumFila
        {
            get
            {
                return this._llamadasCtrl == null ? -1 : this._llamadasCtrl.FindIndex(det => det.Index == this.indexFila);
            }
        }

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Referenciacion()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Referenciacion(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Referenciacion(int libranza, string mod)
        {
            this.Constructor(libranza, mod);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(int? libranza, string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();

                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                string editReferenciasStr = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_IndModificarReferencias);
                this.editReferencias = editReferenciasStr == "1";
                
                this.AddGridCols();
                this.AddGridColsDetail();
                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                    this.txtLibranza.Enabled = false;
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);

                }
                #endregion
                #region Flujo permitido para devolver

                Dictionary<string, string> dictAct = new Dictionary<string, string>();

                //Libre
                dictAct[string.Empty] = string.Empty;

                //Anular
                string anularStr = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado).ToUpper();
                dictAct["AN"] = anularStr;

                List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujo.ID.Value);
                foreach (DTO_glActividadFlujo act in actPadres)
                    dictAct[act.ID.Value] = act.Descriptivo.Value;

                this.lkp_Flujo.Properties.DataSource = dictAct;
                this.lkp_Flujo.Enabled = false;

                #endregion
                #region Revisa si el formulario esta solo en modo de lectura
                this.readOnly = libranza.HasValue;
                if (this.readOnly)
                {
                    this.txtLibranza.Text = libranza.Value.ToString();
                    this.txtLibranza_Leave(null, null);

                    this.txtLibranza.Enabled = false;
                    this.lkp_Flujo.Enabled = false;

                    //Documents
                    foreach(var col in this.gvDocument.Columns)
                    {
                        ((GridColumn)col).OptionsColumn.AllowEdit = false;
                    }

                    //Details
                    foreach (var col in this.gvDetail.Columns)
                    {
                        ((GridColumn)col).OptionsColumn.AllowEdit = false;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "Referenciacion"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.Referenciacion;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, false, true, false);
            this.masterCliente.EnableControl(false);

            //Referencias mínimas
            string refMinimasStr = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ReferenciaMinimaReq);
            if(!string.IsNullOrWhiteSpace(refMinimasStr))
                this.refMinimas = Convert.ToInt32(refMinimasStr);

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Referencias Cliente

                //Tipo Referencia
                GridColumn tipoRef = new GridColumn();
                tipoRef.FieldName = this._unboundPrefix + "TipoReferencia";
                tipoRef.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoReferencia");
                tipoRef.UnboundType = UnboundColumnType.String;
                tipoRef.VisibleIndex = 0;
                tipoRef.Width = 150;
                tipoRef.Visible = true;
                tipoRef.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(tipoRef);

                //Código Llamada
                GridColumn codLlamada = new GridColumn();
                codLlamada.FieldName = this._unboundPrefix + "CodLLamada";
                codLlamada.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodLlamada");
                codLlamada.UnboundType = UnboundColumnType.String;
                codLlamada.VisibleIndex = 1;
                codLlamada.Width = 120;
                codLlamada.Visible = true;
                codLlamada.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(codLlamada);

                //llamadaREF
                GridColumn llamadaREF = new GridColumn();
                llamadaREF.FieldName = this._unboundPrefix + "LlamadaREF";
                llamadaREF.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LlamadaREF");
                llamadaREF.UnboundType = UnboundColumnType.String;
                llamadaREF.VisibleIndex = 2;
                llamadaREF.Width = 150;
                llamadaREF.Visible = true;
                llamadaREF.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(llamadaREF);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this._unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 3;
                nombre.Width = 100;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(nombre);

                //Telefono
                GridColumn telefono = new GridColumn();
                telefono.FieldName = this._unboundPrefix + "Telefono";
                telefono.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Telefono");
                telefono.UnboundType = UnboundColumnType.Integer;
                telefono.VisibleIndex = 4;
                telefono.Width = 150;
                telefono.Visible = true;
                telefono.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(telefono);

                //Direccion
                GridColumn direccion = new GridColumn();
                direccion.FieldName = this._unboundPrefix + "Direccion";
                direccion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Direccion");
                direccion.UnboundType = UnboundColumnType.String;
                direccion.VisibleIndex = 5;
                direccion.Width = 150;
                direccion.Visible = true;
                direccion.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(direccion);

                //Barrio
                GridColumn barrio = new GridColumn();
                barrio.FieldName = this._unboundPrefix + "Barrio";
                barrio.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Barrio");
                barrio.UnboundType = UnboundColumnType.String;
                barrio.VisibleIndex = 6;
                barrio.Width = 150;
                barrio.Visible = true;
                barrio.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(barrio);

                //Correo
                GridColumn correo = new GridColumn();
                correo.FieldName = this._unboundPrefix + "Correo";
                correo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Correo");
                correo.UnboundType = UnboundColumnType.String;
                correo.VisibleIndex = 7;
                correo.Width = 150;
                correo.Visible = true;
                correo.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(correo);

                //Relacion
                GridColumn relacion = new GridColumn();
                relacion.FieldName = this._unboundPrefix + "Relacion";
                relacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Relacion");
                relacion.UnboundType = UnboundColumnType.String;
                relacion.VisibleIndex = 8;
                relacion.Width = 150;
                relacion.Visible = true;
                relacion.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(relacion);

                //Ciudad
                GridColumn ciudad = new GridColumn();
                ciudad.FieldName = this._unboundPrefix + "Ciudad";
                ciudad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ciudad");
                ciudad.UnboundType = UnboundColumnType.String;
                ciudad.VisibleIndex = 9;
                ciudad.Width = 150;
                ciudad.Visible = true;
                ciudad.OptionsColumn.AllowEdit = this.editReferencias;
                this.gvDocument.Columns.Add(ciudad);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la 2 grilla
        /// </summary>
        private void AddGridColsDetail()
        {
            try
            {
                #region Preguntas Llamada
                //PersonaConsultada
                GridColumn personaConsultada = new GridColumn();
                personaConsultada.FieldName = this._unboundPrefix + "PersonaConsulta";
                personaConsultada.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PersonaConsultada");
                personaConsultada.UnboundType = UnboundColumnType.String;
                personaConsultada.VisibleIndex = 1;
                personaConsultada.Width = 80;
                personaConsultada.Visible = true;
                personaConsultada.OptionsColumn.AllowEdit = true;
                this.gvDetail.Columns.Add(personaConsultada);

                //Relacion Titular
                GridColumn relacionTitular = new GridColumn();
                relacionTitular.FieldName = this._unboundPrefix + "RelacionTitular";
                relacionTitular.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RelacionTitular");
                relacionTitular.UnboundType = UnboundColumnType.String;
                relacionTitular.VisibleIndex = 2;
                relacionTitular.Width = 100;
                relacionTitular.Visible = true;
                relacionTitular.OptionsColumn.AllowEdit = true;
                this.gvDetail.Columns.Add(relacionTitular);

                //Pregunta
                GridColumn pregunta = new GridColumn();
                pregunta.FieldName = this._unboundPrefix + "Pregunta";
                pregunta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Pregunta");
                pregunta.UnboundType = UnboundColumnType.String;
                pregunta.VisibleIndex = 3;
                pregunta.Width = 150;
                pregunta.Visible = true;
                pregunta.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(pregunta);

                //Respuesta
                GridColumn respuesta = new GridColumn();
                respuesta.FieldName = this._unboundPrefix + "Observaciones";
                respuesta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Respuesta");
                respuesta.UnboundType = UnboundColumnType.String;
                respuesta.VisibleIndex = 4;
                respuesta.Width = 150;
                respuesta.Visible = true;
                respuesta.OptionsColumn.AllowEdit = true;
                this.gvDetail.Columns.Add(respuesta);

                //Nueva Llamada
                GridColumn marca = new GridColumn();
                marca.FieldName = this._unboundPrefix + "NuevallamadaInd";
                marca.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NuevallamadaInd");
                marca.UnboundType = UnboundColumnType.Boolean;
                marca.VisibleIndex = 5;
                marca.Width = 50;
                marca.Visible = true;
                marca.OptionsColumn.ShowCaption = true;
                this.gvDetail.Columns.Add(marca);

                //Fecha Proxima Llamada
                GridColumn fechaProxllamada = new GridColumn();
                fechaProxllamada.FieldName = this._unboundPrefix + "FechaProxllamada";
                fechaProxllamada.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaProxllamada");
                fechaProxllamada.UnboundType = UnboundColumnType.DateTime;
                fechaProxllamada.VisibleIndex = 6;
                fechaProxllamada.Width = 50;
                fechaProxllamada.Visible = true;
                fechaProxllamada.OptionsColumn.ShowCaption = true;
                this.gvDetail.Columns.Add(fechaProxllamada);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "AddGridColsDetail"));
            }
        }

        /// <summary>
        /// Carga las referencias del cliente asociado a la libranza
        /// </summary>
        private void LoadRef()
        {
            try
            {
                int tmp = Convert.ToInt32(this.txtLibranza.Text);

                string actFlujo = this.readOnly ? string.Empty : this._actFlujo.ID.Value;
                this._digiCredito = this._bc.AdministrationModel.DigitacionCredito_GetByLibranza(tmp, actFlujo);
                if (this._digiCredito.DocCtrl != null)
                {
                    this.numeroDoc = this._digiCredito.DocCtrl.NumeroDoc.Value.Value;

                    if (!String.IsNullOrWhiteSpace(this._digiCredito.Header.ClienteID.Value))
                    {
                        this.lkp_Flujo.EditValue = string.Empty;
                        
                        this._clienteID = this._digiCredito.Header.ClienteID.Value;
                        this.masterCliente.Value = this._digiCredito.Header.ClienteID.Value;
                        this.txtPriNombre.Text = this._digiCredito.Header.NombrePri.Value;
                        this.txtSdoNombre.Text = this._digiCredito.Header.NombreSdo.Value;
                        this.txtPriApellido.Text = this._digiCredito.Header.ApellidoPri.Value;
                        this.txtSdoApellido.Text = this._digiCredito.Header.ApellidoSdo.Value;

                        #region Carga las referencias del tercero del credito
                      
                        #region Carga la info de las referencias
                        DTO_glConsulta query = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "TerceroID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            OperadorSentencia = OperadorSentencia.And,
                            ValorFiltro = this._digiCredito.Header.ClienteID.Value
                        });
                        query.Filtros = filtros;

                        long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.glTerceroReferencia, query, true);
                        List<DTO_MasterComplex> complexList = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glTerceroReferencia, count, 1, query, true).ToList();
                        this._cliRefs = complexList.Cast<DTO_glTerceroReferencia>().ToList();
                        #endregion
                        if (this._cliRefs.Count != 0)
                        {
                            #region Carga la info de los códigos de llamadas

                            DTO_glConsulta queryCodigos = new DTO_glConsulta();
                            List<DTO_glConsultaFiltro> filtrosCodigos = new List<DTO_glConsultaFiltro>();
                            filtrosCodigos.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "NoEfectivaInd",
                                OperadorFiltro = OperadorFiltro.Igual,
                                OperadorSentencia = OperadorSentencia.And,
                                ValorFiltro = false.ToString()
                            });
                            queryCodigos.Filtros = filtrosCodigos;

                            string codLlamadaID = string.Empty;
                            long countCodigos = _bc.AdministrationModel.MasterSimple_Count(AppMasters.glLLamadaCodigo, queryCodigos, null, true);
                            DTO_MasterBasic codLlamadaXDef = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.glLLamadaCodigo, countCodigos, 1, queryCodigos, null, true).ToList().FirstOrDefault();
                            if (codLlamadaXDef != null)
                            {
                                //codLlamadaID = codLlamadaXDef.ID.Value;
                            }

                            #endregion

                            //Convierte el nombre de la referencia a mayusculas
                            this._cliRefs.ForEach(
                                x => 
                                {
                                    x.Nombre.Value = x.Nombre.Value.ToUpper();
                                    x.CodLLamada.Value = codLlamadaID;
                                }
                            );
                            this.currentRow = 0;
                            this.gcDocument.DataSource = this._cliRefs;
                            this.gcDocument.RefreshDataSource();
                        }
                        else
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NoClienteRef);
                            string msgFinal = String.Format(msg, this._digiCredito.Header.NombrePri.Value + " " + this._digiCredito.Header.ApellidoPri.Value);
                            MessageBox.Show(msgFinal);
                        }
                        #endregion

                        this.lkp_Flujo.EditValue = string.Empty;
                        this.lkp_Flujo.Enabled = false;
                    }
                }
                else
                {
                    this.CleanData();
                    this.txtLibranza.Text = tmp.ToString();
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoDisponible);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "LoadRef"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            //Header
            this.masterCliente.Value = String.Empty;
            this.txtLibranza.Text = String.Empty;
            this.txtPriNombre.Text = String.Empty;
            this.txtSdoNombre.Text = String.Empty;
            this.txtPriApellido.Text = String.Empty;
            this.txtSdoApellido.Text = String.Empty;

            //Variables
            this.rechazoObs = string.Empty;
            this.validateData = false;
            this._libranzaID = String.Empty;
            this._clienteID = String.Empty;
            this._cliRefs = new List<DTO_glTerceroReferencia>();
            this._llamadasCtrl = new List<DTO_glLlamadasControl>();
            this._llamadasCtrlAll = new List<DTO_glLlamadasControl>();

            this.lkp_Flujo.EditValue = string.Empty;
            this.lkp_Flujo.Enabled = false;

            //Grillas
            this.gcDocument.DataSource = null;
            this.gcDetail.DataSource = null;

        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateDetail()
        {
            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData);
            if (this._cliRefs.Count == 0)
            {
                MessageBox.Show(msg);
                return false;
            }

            if (this._llamadasCtrl.Count == 0)
            {
                MessageBox.Show(msg);
                return false;
            }

            //Verifica si todas las referencias deben estar llenas
            if (string.IsNullOrWhiteSpace(this.lkp_Flujo.EditValue.ToString()))
            {
                if (this.refMinimas == 99)
                {
                    List<DTO_glLlamadasControl> llamadas = this._llamadasCtrlAll.Where(x => x.Observaciones.Value.Trim() == string.Empty).ToList();
                    if (llamadas.Count > 0)
                    {
                        msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LlamadaSinRespuesta);
                        MessageBox.Show(msg);
                        return false;
                    }
                }
                else
                {
                    int refCompletas = 0;
                    foreach (DTO_glTerceroReferencia tRef in this._cliRefs)
                    {
                        var refCliente = this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value == tRef.TipoReferencia.Value && x.NombreReferencia.Value == tRef.Nombre.Value).ToList();
                        if (!refCliente.Any(x => x.Observaciones.Value.Trim() == string.Empty))
                        {
                            refCompletas++;
                        }
                    }   

                    //if (refCompletas < this.refMinimas)
                    //{
                    //    msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReferenciasMinimas);
                    //    MessageBox.Show(string.Format(msg, this.refMinimas.ToString()));
                    //    return false;
                    //}
                    ////Valida
                    //var tiposRef = this._llamadasCtrlAll.FindAll(x=>!string.IsNullOrEmpty(x.CodLLamada.Value));
                    //List<byte> tipoRefDist = (from x in tiposRef select x.TipoReferencia.Value.Value).Distinct().ToList();
                    //if (!tipoRefDist.Contains(1) || !tipoRefDist.Contains(2) || !tipoRefDist.Contains(4)) // Tipos requeridos siempre
                    //{
                    //    msg = _bc.GetResource(LanguageTypes.Messages,"Debe digitar como mínimo una referencia tipo Personal, una familiar y una solicitante");
                    //    MessageBox.Show(msg);
                    //    return false;
                    //}
                }
            }
            else 
            {
                if (string.IsNullOrWhiteSpace(this.rechazoObs))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DevolucionFlujoObsVacio));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Funcion que carga las preguntas de las referencias
        /// </summary>
        /// <param name="refrencias">Lista de las referencias</param>
        private void GetPreguntas()
        {
            try
            {
                foreach (DTO_glTerceroReferencia referencia in this._cliRefs)
                {
                    if (!String.IsNullOrWhiteSpace(this._actFlujo.LLamadaID.Value))
                    {
                        #region Carga las preguntas

                        DTO_glConsulta query = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "LLamadaID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            OperadorSentencia = OperadorSentencia.And,
                            ValorFiltro = this._actFlujo.LLamadaID.Value
                        });

                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "TipoReferencia",
                            OperadorFiltro = OperadorFiltro.Igual,
                            OperadorSentencia = OperadorSentencia.And,
                            ValorFiltro = referencia.TipoReferencia.Value.ToString()
                        });
                        query.Filtros = filtros;

                        long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.glLLamadaPregunta, query, true);
                        List<DTO_MasterComplex> complexList = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glLLamadaPregunta, count, 1, query, true).ToList();
                        List<DTO_glLLamadaPregunta> preguntasRefencias = complexList.Cast<DTO_glLLamadaPregunta>().ToList();

                        #region Creo las preguntas del cliente referencia
                        foreach (DTO_glLLamadaPregunta item in preguntasRefencias)
                        {
                            DTO_glLlamadasControl _llamaCtrl = new DTO_glLlamadasControl();
                            _llamaCtrl.NumeroDoc.Value = this.numeroDoc;
                            _llamaCtrl.ActividadFlujoID.Value = this._actFlujo.ID.Value;
                            _llamaCtrl.NumReferencia.Value = referencia.ReplicaID.Value.Value;
                            _llamaCtrl.IdentificadorPrg.Value = item.ReplicaID.Value.Value;
                            _llamaCtrl.CodPregunta.Value = item.CodPregunta.Value;
                            _llamaCtrl.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                            _llamaCtrl.Fecha.Value = DateTime.Now;
                            _llamaCtrl.PersonaConsulta.Value = referencia.Nombre.Value;
                            _llamaCtrl.RelacionTitular.Value = referencia.Relacion.Value;
                            _llamaCtrl.Pregunta.Value = item.Pregunta.Value;
                            _llamaCtrl.Observaciones.Value = String.Empty;
                            _llamaCtrl.NuevallamadaInd.Value = false;
                            _llamaCtrl.FechaProxllamada.Value = DateTime.Now;
                            //Campos Adicionales Para Filtros
                            _llamaCtrl.TipoReferencia.Value = item.TipoReferencia.Value;
                            _llamaCtrl.NombreReferencia.Value = referencia.Nombre.Value.ToUpper();
                            this._llamadasCtrlAll.Add(_llamaCtrl);
                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        this.CleanData();
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_ActividadFlujoNoLlamada);
                        MessageBox.Show(string.Format(msg, this._actFlujo.ID.Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "GetPreguntas"));
            }
        }

        /// <summary>
        /// Metodo que permite crea una nueva fila en una grilla
        /// </summary>
        private void AddNewRow()
        {
            DTO_glTerceroReferencia refCliente = new DTO_glTerceroReferencia();
            try
            {
                DTO_ccCliente cli = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this._clienteID, true);
                isValid = false;
                #region Asigna datos a la fila
                refCliente.EmpresaGrupoID.Value = this._bc.AdministrationModel.Empresa.EmpresaGrupoID_.Value;
                refCliente.TerceroID.Value = this._clienteID;
                refCliente.TerceroDesc.Value = cli.Descriptivo.Value;
                refCliente.TipoReferencia.Value = 1;
                refCliente.Nombre.Value = string.Empty;
                refCliente.Relacion.Value = string.Empty;
                refCliente.Direccion.Value = string.Empty;
                refCliente.Barrio.Value = string.Empty;
                refCliente.Telefono.Value = string.Empty;
                refCliente.Ciudad.Value = string.Empty;
                refCliente.Correo.Value = string.Empty;
                refCliente.ActivoInd.Value = true;
                refCliente.CtrlVersion.Value = 1;
                refCliente.ReplicaID.Value = this.replicaTemp;
                refCliente.NuevoRegistro.Value = true;
                this.replicaTemp++;
                #endregion
                this._cliRefs.Add(refCliente);
                this.gcDocument.DataSource = this._cliRefs;
                this.gcDocument.RefreshDataSource();

                #region Carga las preguntas
                DTO_glConsulta query = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "LLamadaID",
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = OperadorSentencia.And,
                    ValorFiltro = this._actFlujo.LLamadaID.Value
                });

                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoReferencia",
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = OperadorSentencia.And,
                    ValorFiltro = refCliente.TipoReferencia.Value.ToString()
                });
                query.Filtros = filtros;

                long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.glLLamadaPregunta, query, true);
                List<DTO_MasterComplex> complexList = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glLLamadaPregunta, count, 1, query, true).ToList();
                List<DTO_glLLamadaPregunta> preguntasRefencias = complexList.Cast<DTO_glLLamadaPregunta>().ToList();
                #region Creo las preguntas del cliente referencia
                foreach (DTO_glLLamadaPregunta item in preguntasRefencias)
                {
                    DTO_glLlamadasControl _llamaCtrl = new DTO_glLlamadasControl();
                    _llamaCtrl.NumeroDoc.Value = this.numeroDoc;
                    _llamaCtrl.ActividadFlujoID.Value = this._actFlujo.ID.Value;
                    _llamaCtrl.NumReferencia.Value = refCliente.ReplicaID.Value.Value;
                    _llamaCtrl.IdentificadorPrg.Value = item.ReplicaID.Value.Value;
                    _llamaCtrl.CodPregunta.Value = item.CodPregunta.Value;
                    _llamaCtrl.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                    _llamaCtrl.Fecha.Value = DateTime.Now;
                    _llamaCtrl.PersonaConsulta.Value = refCliente.Nombre.Value;
                    _llamaCtrl.RelacionTitular.Value = refCliente.Relacion.Value;
                    _llamaCtrl.Pregunta.Value = item.Pregunta.Value;
                    _llamaCtrl.Observaciones.Value = String.Empty;
                    _llamaCtrl.NuevallamadaInd.Value = false;
                    _llamaCtrl.FechaProxllamada.Value = DateTime.Now;
                    //Campos Adicionales Para Filtros
                    _llamaCtrl.TipoReferencia.Value = item.TipoReferencia.Value;
                    _llamaCtrl.NombreReferencia.Value = refCliente.Nombre.Value.ToUpper();
                    this._llamadasCtrlAll.Add(_llamaCtrl);
                }
                #endregion
                #endregion
                this.gcDetail.DataSource = this._llamadasCtrlAll;
                this.gcDetail.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "AddNewRow"));
            }
        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_RefCliente(int fila)
        {
            try
            {
                this.gvDocument.PostEditor();
                this.isValid = true;

                if (fila >= 0)
                {
                    bool rowValid = true;
                    string fieldName;
                    #region Nombre
                    rowValid = true;
                    fieldName = "Nombre";
                    GridColumn colNombre = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = this._bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colNombre, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                    #region Relacion
                    rowValid = true;
                    fieldName = "Relacion";
                    GridColumn colRelacion = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = this._bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colRelacion, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                    #region Direccion
                    rowValid = true;
                    fieldName = "Direccion";
                    GridColumn colDireccion = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = this._bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colDireccion, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                    #region Barrio
                    rowValid = true;
                    fieldName = "Barrio";
                    GridColumn colBarrio = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = this._bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colBarrio, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                    #region Telefono
                    rowValid = true;
                    fieldName = "Telefono";
                    GridColumn colTelefono = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = _bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colTelefono, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                    #region Ciudad
                    rowValid = true;
                    fieldName = "Ciudad";
                    GridColumn colCiudad = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = this._bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colCiudad, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                    #region Correo
                    rowValid = true;
                    fieldName = "Correo";
                    GridColumn colCorreo = this.gvDocument.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores
                    rowValid = this._bc.ValidGridCell(this.gvDocument, this._unboundPrefix, fila, fieldName, false, false, false, null);
                    if (rowValid)
                        this.gvDetail.SetColumnError(colCorreo, string.Empty);
                    else
                        this.isValid = false;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "ValidateRow_RefCliente"));
            }
        }

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
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemNew.Enabled = this.readOnly ? false : true;
                    FormProvider.Master.itemSendtoAppr.Visible = true;
                    FormProvider.Master.itemSendtoAppr.Enabled = this.readOnly ? false : SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSave.Enabled = this.readOnly ? false : SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemUpdate.Enabled = this.readOnly ? false : true;
                    FormProvider.Master.itemSearch.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que permite crear, habilitar o deshabilitar las propiedades del documento con base a la Libranza  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._libranzaID != this.txtLibranza.Text.Trim())
                {
                    this._libranzaID = this.txtLibranza.Text.Trim();
                    this.currentRow = -1;
                    this.LoadRef();
                    this.validateData = true;
                    this.isValid = true;
                    if (this._cliRefs.Count > 0)
                    {
                        this.GetPreguntas();
                        
                        List<DTO_glLlamadasControl> llamadasTemp = this._bc.AdministrationModel.glLlamadasControl_GetByID(this.numeroDoc);
                        if (llamadasTemp.Count > 0)
                        {
                            List<DTO_glLlamadasControl> llamadasAllTemp = new List<DTO_glLlamadasControl>();
                            foreach(DTO_glTerceroReferencia tRef in this._cliRefs)
                            {
                                List<DTO_glLlamadasControl> llamadas = llamadasTemp.Where(x => x.TipoReferencia.Value == tRef.TipoReferencia.Value &&
                                                                          x.NombreReferencia.Value == tRef.Nombre.Value).ToList();

                                if (llamadas.Count > 0)
                                {
                                    llamadasAllTemp.AddRange(llamadas);
                                }
                                else
                                {
                                    llamadasAllTemp.AddRange(this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value == tRef.TipoReferencia.Value &&
                                                                          x.NombreReferencia.Value == tRef.Nombre.Value).ToList());
                                }
                            }

                            this._llamadasCtrlAll = llamadasAllTemp;
                            this._llamadasCtrl = this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value == this._cliRefs.First().TipoReferencia.Value &&
                                                                          x.NombreReferencia.Value == this._cliRefs.First().Nombre.Value).ToList();

                            //Asigna el código de llamada a las referencias del tercero
                            bool reloadRef = false;
                            foreach (DTO_glTerceroReferencia terRef in this._cliRefs)
                            {
                                DTO_glLlamadasControl ctrl = this._llamadasCtrlAll.FirstOrDefault(
                                    x => 
                                        x.TipoReferencia.Value == terRef.TipoReferencia.Value &&
                                        x.NombreReferencia.Value == terRef.Nombre.Value
                                    );

                                if (ctrl != null)
                                {
                                    string codLlamadaID = ctrl.CodLLamada.Value;
                                    string llamadaREF = ctrl.LLamadaREF.Value;
                                    if (!string.IsNullOrWhiteSpace(codLlamadaID))
                                    {
                                        reloadRef = true;
                                        terRef.CodLLamada.Value = codLlamadaID;
                                        terRef.LlamadaREF.Value = llamadaREF;
                                    } 
                                }
                            }

                            if (reloadRef)
                            {
                                this.gcDocument.DataSource = this._cliRefs;
                                this.gcDocument.RefreshDataSource();
                            }
                        }
                        else
                        {
                            //this.GetPreguntas();
                            this._llamadasCtrl = this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value == this._cliRefs.First().TipoReferencia.Value &&
                                                                          x.NombreReferencia.Value == this._cliRefs.First().Nombre.Value).ToList();
                        }
                        this.gcDetail.DataSource = this._llamadasCtrl;

                        this.lkp_Flujo.EditValue = string.Empty;
                        this.lkp_Flujo.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que solo permite digitar numeros en el campo de libranza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Boton para poner una observación de rechazo del documento
        /// </summary>
        private void btnFlujoRechazo_Click(object sender, EventArgs e)
        {
            try
            {
                string title = _bc.GetResource(LanguageTypes.Forms, "32017_lblTitle");
                string label = _bc.GetResource(LanguageTypes.Forms, "1015_lblObservacion");
                string currentObs = this.rechazoObs;
                if (Prompt.InputBox(title, label, ref currentObs,false) == DialogResult.OK)
                    this.rechazoObs = currentObs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "btnFlujoRechazo_Click"));
            }
        }

        /// <summary>
        /// Boton para habilitar o deshabilitar el boton de rechazo de un documento
        /// </summary>
        private void lkp_Flujo_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.lkp_Flujo.EditValue.ToString()))
            {
                this.btnFlujoRechazo.Enabled = false;
                this.rechazoObs = string.Empty;
            }
            else
            {
                this.btnFlujoRechazo.Enabled = true;
            }
        }
        #endregion

        #region Eventos Grilla Referencias

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;
                if (validateData)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.ValidateRow_RefCliente(fila);
                        if (this.isValid)
                        {
                            this.AddNewRow();
                            this.gvDocument.FocusedRowHandle = this._cliRefs.Count - 1;
                        }
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        if (fila >= 0)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.isValid = true;
                                this.deleteOp = true;
                                this._cliRefs.RemoveAt(fila);
                                if (this._cliRefs.Count > 0)
                                {
                                    this.gcDocument.RefreshDataSource();
                                    this.gvDocument.FocusedRowHandle = fila - 1;
                                }
                                else
                                {
                                    this.validateData = false;
                                    this.gcDocument.RefreshDataSource();
                                    this._llamadasCtrlAll = new List<DTO_glLlamadasControl>();
                                    this.gcDetail.DataSource = _llamadasCtrlAll;
                                    this.gcDetail.RefreshDataSource();
                                    this.validateData = true;
                                }
                                this.deleteOp = false;
                            }

                            e.Handled = true;
                        }
                        else
                            e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "gcDetail_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que valida las columna de la grilla antes de salir de esta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.validateData)
            {
                int fila = e.RowHandle;
                if (!this.deleteOp)
                {
                    this.ValidateRow_RefCliente(fila);
                    if (!this.isValid)
                        e.Allow = false;
                }
            }
        }

        /// <summary>
        /// Maneja campos de controles en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "NuevaLlamadaInd")
                {
                    e.RepositoryItem = this.editChkBox;
                }

                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.riPopup;
                }

                if (fieldName == "FechaProxllamada")
                {
                    e.RepositoryItem = this.dateEdit;
                }

                if (fieldName == "CodLLamada")
                {
                    e.RepositoryItem = this.editBtnGrid;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
                    e.Value = String.Empty;
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

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "Observaciones")
                {
                    e.RepositoryItem = this.riPopup;
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
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (validateData)
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                int fila = e.RowHandle;

                if (fieldName == "CodLLamada")
                {                   
                    bool cleanCodigo = true;
                    if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        DTO_glLLamadaCodigo cod = (DTO_glLLamadaCodigo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLLamadaCodigo, false, e.Value.ToString(), true);
                        if (cod != null && cod.NoEfectivaInd.Value.Value)
                        {
                            cleanCodigo = false;
                            foreach (DTO_glLlamadasControl item in this._llamadasCtrl)
                            {
                                //item.CodLLamada.Value = cod.ID.Value;
                                item.Observaciones.Value = cod.Descriptivo.Value;
                            }
                        }                       
                    }

                    if(cleanCodigo)
                    {
                        foreach (DTO_glLlamadasControl item in this._llamadasCtrl)
                        {
                            item.CodLLamada.Value = string.Empty;
                            item.Observaciones.Value = string.Empty;
                        }
                    }
                    this._llamadasCtrl.ForEach(x => x.CodLLamada.Value = e.Value.ToString());
                }

                if (fieldName == "Nombre")
                {
                    this._cliRefs[fila].Nombre.Value = e.Value.ToString().ToUpper();
                    foreach (DTO_glLlamadasControl item in this._llamadasCtrl)
                    {
                        item.PersonaConsulta.Value = e.Value.ToString().ToUpper();
                        item.NombreReferencia.Value = e.Value.ToString().ToUpper();
                    }
                }

                if (fieldName == "Relacion")
                    this._llamadasCtrl.ForEach(x => x.RelacionTitular.Value = e.Value.ToString());

                if (fieldName == "LlamadaREF")
                    this._llamadasCtrl.ForEach(x => x.LLamadaREF.Value = e.Value.ToString());

                this.gcDocument.RefreshDataSource();
                this.gcDetail.RefreshDataSource();
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this.gcDetail.DataSource = null;
                if (this.currentRow != -1 && this.validateData)
                {
                    if (e.FocusedRowHandle <= this.gvDocument.RowCount - 1)
                        this.currentRow = e.FocusedRowHandle;
                    this.currentDoc = (DTO_glTerceroReferencia)this.gvDocument.GetRow(this.currentRow);
                    this._llamadasCtrl = this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value.Value == this.currentDoc.TipoReferencia.Value &&
                                                                     x.NombreReferencia.Value == this.currentDoc.Nombre.Value).ToList();
                    this.gcDetail.DataSource = this._llamadasCtrl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "gldocuments_focusedRowChanged"));
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion Eventos Documentos

        #region Eventos Grilla de Preguntas
        /// <summary>
        /// Evento que se presenta al modificar un valor de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

                if (fieldName == "PersonaConsulta")
                {
                    this._llamadasCtrl.ForEach(x => x.PersonaConsulta.Value = e.Value.ToString().ToUpper());
                    this.gcDetail.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "gvDetail_CellValueChanged"));
            }
        }

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDetail.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Observaciones")
                this.richEditControl.Document.Text = this.gvDetail.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
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
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Actualiza la informacion del la pantalla
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this._actFlujo.ID.Value))
                {
                    this.LoadRef();
                    if (this._cliRefs.Count > 0)
                    {
                        this._llamadasCtrlAll = new List<DTO_glLlamadasControl>();

                        List<DTO_glLlamadasControl> llamdasTemp = this._bc.AdministrationModel.glLlamadasControl_GetByID(this.numeroDoc);
                        if (llamdasTemp.Count > 0)
                        {
                            this._llamadasCtrlAll = llamdasTemp;
                            this._llamadasCtrl = this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value == this._cliRefs.First().TipoReferencia.Value &&
                                                                          x.NombreReferencia.Value == this._cliRefs.First().Nombre.Value).ToList();
                        }
                        else
                        {
                            this.GetPreguntas();
                            this._llamadasCtrl = this._llamadasCtrlAll.Where(x => x.TipoReferencia.Value == this._cliRefs.First().TipoReferencia.Value &&
                                                                          x.NombreReferencia.Value == this._cliRefs.First().Nombre.Value).ToList();
                        }
                        this.gcDetail.DataSource = this._llamadasCtrl;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para Enviar un documento para aprobacion
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();

                this.gvDocument.Focus();
                this.gvDocument.PostEditor();
                this.gvDetail.PostEditor();
                if (this.isValid && ValidateDetail())
                {
                    if (!string.IsNullOrWhiteSpace(this.lkp_Flujo.EditValue.ToString()))
                    {
                        string titleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgWarning = this.lkp_Flujo.EditValue == "AN" ? _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Anular_Doc) :
                            _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActualizarFlujo);

                        if (MessageBox.Show(msgWarning, titleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                        else 
                        {
                            DTO_SolicitudAprobacionCartera sol = new DTO_SolicitudAprobacionCartera();
                            sol.Aprobado.Value = false;
                            sol.Rechazado.Value = true;
                            sol.NumeroDoc.Value = this.numeroDoc;
                            sol.ActividadFlujoReversion.Value = this.lkp_Flujo.EditValue.ToString() == "AN" ? string.Empty : this.lkp_Flujo.EditValue.ToString();
                            sol.Observacion.Value = this.rechazoObs;

                            List<DTO_SolicitudAprobacionCartera> list = new List<DTO_SolicitudAprobacionCartera>();
                            list.Add(sol);

                            List<DTO_SerializedObject> results = _bc.AdministrationModel.SolicitudLibranza_AprobarRechazar(this._documentID, this._actFlujo.ID.Value, list, 
                                new List<DTO_ccSolicitudAnexo>(), new List<DTO_ccTareaChequeoLista>());

                            object r = results.First();
                            if (r.GetType() == typeof(DTO_TxResult))
                                result = (DTO_TxResult)r;
                            else
                            { 
                                result.Result = ResultValue.OK;
                                result.ResultMessage = string.Empty;

                                this.CleanData();
                                this.txtLibranza.Focus();
                            }
                        }
                    }
                    else
                    {
                        this.gvDocument.PostEditor();
                        this.gvDetail.PostEditor();

                        result = _bc.AdministrationModel.glLlamadasControl_Add(this._documentID, this._actFlujo.ID.Value, this._llamadasCtrlAll, this._cliRefs, true);
                        if (result.Result == ResultValue.OK)
                        {
                            #region Obtiene el nombre

                            string nombre = this._digiCredito.Header.NombrePri.Value;
                            if (!string.IsNullOrWhiteSpace(this._digiCredito.Header.NombreSdo.Value))
                                nombre += " " + this._digiCredito.Header.NombreSdo.Value;
                            if (!string.IsNullOrWhiteSpace(this._digiCredito.Header.ApellidoPri.Value))
                                nombre += " " + this._digiCredito.Header.ApellidoPri.Value;
                            if (!string.IsNullOrWhiteSpace(this._digiCredito.Header.ApellidoSdo.Value))
                                nombre += " " + this._digiCredito.Header.ApellidoSdo.Value;

                            #endregion
                            #region Variables para el mail

                            DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                            string body = string.Empty;
                            string subject = string.Empty;
                            string email = user.CorreoElectronico.Value;

                            string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                            string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_SentToAprobCartera_Body);
                            string formName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                            #endregion
                            #region Envia el correo
                            subject = string.Format(subjectApr, formName);
                            body = string.Format(bodyApr, formName, this._libranzaID, nombre, this._digiCredito.Header.Observacion.Value);
                            _bc.SendMail(this._documentID, subject, body, email);
                            #endregion
                            this.CleanData();
                            this.txtLibranza.Focus();
                        }
                    }

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.Focus();
                this.gvDocument.PostEditor();
                this.gvDetail.PostEditor();
                if (this.isValid)
                {
                    DTO_TxResult result = _bc.AdministrationModel.glLlamadasControl_Add(this._documentID, this._actFlujo.ID.Value, this._llamadasCtrlAll, this._cliRefs, false);
                    if (result.Result == ResultValue.OK)
                    {
                        this.CleanData();
                        this.txtLibranza.Focus();
                    }

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}