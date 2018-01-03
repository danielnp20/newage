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


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ContactoLegalizacion : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentRecursos;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int numeroDoc = 0;
        private bool loaded = false;
        private bool isValid;
        private bool validateData;
        private bool deleteOp;

        //DTOs        
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

        //DTOs Grilla Superior
        private DTO_DigitacionCredito _digiCredito = new DTO_DigitacionCredito();
        //List<DTO_glTerceroReferencia> _cliRefs = new List<DTO_glTerceroReferencia>();

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

        //Variables Protected
        protected int indexFila = 0;
        protected int currentRow = -1;
        protected DTO_glTerceroReferencia currentDoc = null;

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
        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ContactoLegalizacion()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ContactoLegalizacion(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string mod = null)
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "ContactoLegalizacion"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentRecursos = AppDocuments.Referenciacion;
            this._documentID = AppDocuments.ContactoLegalizacion;
            this._frmModule = ModulesPrefix.cf;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, false, true, false);
            this.masterCliente.EnableControl(false);
        }

        /// <summary>
        /// Agrega las columnas a la 2 grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Preguntas Llamada
                //PersonaConsultada
                GridColumn personaConsultada = new GridColumn();
                personaConsultada.FieldName = this._unboundPrefix + "PersonaConsulta";
                personaConsultada.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentRecursos + "_PersonaConsultada");
                personaConsultada.UnboundType = UnboundColumnType.String;
                personaConsultada.VisibleIndex = 1;
                personaConsultada.Width = 80;
                personaConsultada.Visible = true;
                personaConsultada.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(personaConsultada);

                //Relacion Titular
                GridColumn relacionTitular = new GridColumn();
                relacionTitular.FieldName = this._unboundPrefix + "RelacionTitular";
                relacionTitular.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentRecursos + "_RelacionTitular");
                relacionTitular.UnboundType = UnboundColumnType.String;
                relacionTitular.VisibleIndex = 2;
                relacionTitular.Width = 100;
                relacionTitular.Visible = true;
                relacionTitular.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(relacionTitular);

                //Pregunta
                GridColumn pregunta = new GridColumn();
                pregunta.FieldName = this._unboundPrefix + "Pregunta";
                pregunta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentRecursos + "_Pregunta");
                pregunta.UnboundType = UnboundColumnType.String;
                pregunta.VisibleIndex = 3;
                pregunta.Width = 150;
                pregunta.Visible = true;
                pregunta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(pregunta);

                //Respuesta
                GridColumn respuesta = new GridColumn();
                respuesta.FieldName = this._unboundPrefix + "Observaciones";
                respuesta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentRecursos + "_Respuesta");
                respuesta.UnboundType = UnboundColumnType.String;
                respuesta.VisibleIndex = 4;
                respuesta.Width = 150;
                respuesta.Visible = true;
                respuesta.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(respuesta);

                //Nueva Llamada
                GridColumn marca = new GridColumn();
                marca.FieldName = this._unboundPrefix + "NuevallamadaInd";
                marca.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentRecursos + "_NuevallamadaInd");
                marca.UnboundType = UnboundColumnType.Boolean;
                marca.VisibleIndex = 5;
                marca.Width = 50;
                marca.Visible = true;
                marca.OptionsColumn.ShowCaption = true;
                this.gvDocument.Columns.Add(marca);

                //Fecha Proxima Llamada
                GridColumn fechaProxllamada = new GridColumn();
                fechaProxllamada.FieldName = this._unboundPrefix + "FechaProxllamada";
                fechaProxllamada.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentRecursos + "_FechaProxllamada");
                fechaProxllamada.UnboundType = UnboundColumnType.DateTime;
                fechaProxllamada.VisibleIndex = 6;
                fechaProxllamada.Width = 50;
                fechaProxllamada.Visible = true;
                fechaProxllamada.OptionsColumn.ShowCaption = true;
                this.gvDocument.Columns.Add(fechaProxllamada);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "AddGridCols"));
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

                this._digiCredito = this._bc.AdministrationModel.DigitacionCredito_GetByLibranza(tmp, this._actFlujo.ID.Value);

                if (this._digiCredito.DocCtrl != null)
                {
                    this.numeroDoc = this._digiCredito.DocCtrl.NumeroDoc.Value.Value;

                    if (!String.IsNullOrWhiteSpace(this._digiCredito.Header.ClienteID.Value))
                    {
                        this._clienteID = this._digiCredito.Header.ClienteID.Value;
                        this.masterCliente.Value = this._digiCredito.Header.ClienteID.Value;
                        this.txtPriNombre.Text = this._digiCredito.Header.NombrePri.Value;
                        this.txtSdoNombre.Text = this._digiCredito.Header.NombreSdo.Value;
                        this.txtPriApellido.Text = this._digiCredito.Header.ApellidoPri.Value;
                        this.txtSdoApellido.Text = this._digiCredito.Header.ApellidoSdo.Value;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "LoadRef"));
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
            this.validateData = false;
            this.numeroDoc = 0;
            this._libranzaID = String.Empty;
            this._clienteID = String.Empty;
            this._llamadasCtrl = new List<DTO_glLlamadasControl>();
            this._llamadasCtrlAll = new List<DTO_glLlamadasControl>();

            //Grillas
            this.gcDocument.DataSource = null;

        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateDetail()
        {
            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData);

            if (this._llamadasCtrl.Count == 0)
            {
                MessageBox.Show(msg);
                return false;
            }

            List<DTO_glLlamadasControl> llamadas = this._llamadasCtrlAll.Where(x => x.Observaciones.Value.Trim() == string.Empty).ToList();
            if (llamadas.Count > 0)
            {
                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LlamadaSinRespuesta);
                MessageBox.Show(msg);
                return false;
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
                if (!String.IsNullOrWhiteSpace(this._actFlujo.LLamadaID.Value))
                {
                    #region Carga las preguntas
                    DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this._clienteID, true);
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
                        ValorFiltro = "4"
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
                        _llamaCtrl.NumReferencia.Value = cliente.ReplicaID.Value.Value;
                        _llamaCtrl.IdentificadorPrg.Value = item.ReplicaID.Value.Value;
                        _llamaCtrl.CodPregunta.Value = item.CodPregunta.Value;
                        _llamaCtrl.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                        _llamaCtrl.Fecha.Value = DateTime.Now;
                        _llamaCtrl.PersonaConsulta.Value = cliente.Descriptivo.Value;
                        _llamaCtrl.RelacionTitular.Value = "Ninguna"; //Va quemado porque es el mismo, y el campo es obligatorio
                        _llamaCtrl.Pregunta.Value = item.Pregunta.Value;
                        _llamaCtrl.Observaciones.Value = String.Empty;
                        _llamaCtrl.NuevallamadaInd.Value = false;
                        _llamaCtrl.FechaProxllamada.Value = DateTime.Now;
                        //Campos Adicionales Para Filtros
                        _llamaCtrl.TipoReferencia.Value = item.TipoReferencia.Value;
                        _llamaCtrl.NombreReferencia.Value = cliente.Descriptivo.Value.ToUpper();
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
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "GetPreguntas"));
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
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSendtoAppr.Visible = true;
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSearch.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "Form_FormClosed"));
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
                    if (this.numeroDoc != 0)
                    {
                        List<DTO_glLlamadasControl> llamdasTemp = this._bc.AdministrationModel.glLlamadasControl_GetByID(this.numeroDoc);
                        if (llamdasTemp.Count > 0)
                            this._llamadasCtrlAll = llamdasTemp;
                        else
                            this.GetPreguntas();
                        this.gcDocument.DataSource = this._llamadasCtrl;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "txtLibranza_Leave"));
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

        #endregion

        #region Eventos Grilla de Preguntas

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "gvDocument_CustomRowCellEdit"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "gvDocument_CustomRowCellEditForEditing"));
            }
        }

        /// <summary>
        /// Evento que se presenta al modificar un valor de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

                if (fieldName == "PersonaConsulta")
                {
                    this._llamadasCtrl.ForEach(x => x.PersonaConsulta.Value = e.Value.ToString().ToUpper());
                    this.gcDocument.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "gvDetail_CellValueChanged"));
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
            string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Observaciones")
                this.richEditControl.Document.Text = this.gvDocument.GetFocusedRowCellValue(fieldName).ToString();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "TBNew"));
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
                    if (this.numeroDoc != 0)
                    {
                        List<DTO_glLlamadasControl> llamdasTemp = this._bc.AdministrationModel.glLlamadasControl_GetByID(this.numeroDoc);
                        if (llamdasTemp.Count > 0)
                            this._llamadasCtrlAll = llamdasTemp;
                        else
                            this.GetPreguntas();

                        this.gcDocument.DataSource = this._llamadasCtrl;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton para Enviar un documento para aprobacion
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                this.gvDocument.PostEditor();
                if (this.isValid && ValidateDetail())
                {
                    this.gvDocument.PostEditor();
                    List<DTO_glTerceroReferencia> cliRefs = new List<DTO_glTerceroReferencia>();
                    DTO_TxResult result = _bc.AdministrationModel.glLlamadasControl_Add(this._documentID, this._actFlujo.ID.Value, this._llamadasCtrlAll, cliRefs, true);
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

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();
                if (this.isValid)
                {
                    List<DTO_glTerceroReferencia> cliRefs = new List<DTO_glTerceroReferencia>();
                    DTO_TxResult result = _bc.AdministrationModel.glLlamadasControl_Add(this._documentID, this._actFlujo.ID.Value, this._llamadasCtrlAll, cliRefs, false);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContactoLegalizacion.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}