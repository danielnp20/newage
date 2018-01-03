using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario base de todos los formularios
    /// </summary>
    public partial class MDI : Form
    {
        #region Hilos

        #region Variables y Propiedades

        /// <summary>
        /// Diccionarios con el identificador del proceso y los valores
        /// </summary>
        private Dictionary<int, int> _docProgress;
        private Dictionary<int, string> _docStatus;

        /// <summary>
        /// Hilo con la barra de progreso
        /// </summary>
        public Thread ProgressBarThread
        {
            get;
            set;
        }

        /// <summary>
        /// Funcion que se ejecuta ver el estado 
        /// Dependiendo si es maestra, app, etc
        /// </summary>   
        public Func<int> FuncProgressBarThread
        {
            get;
            set;
        }

        #endregion

        #region Delegados

        private delegate void DisableProgress();
        private DisableProgress DisableProgressDelegate;
        /// <summary>
        /// Deshabilita los controles de la barra de estado
        /// </summary>
        /// <param name="status"></param>
        private void DisableProgressMethod()
        {
            this.pbStatus.Value = 0;
            this.pbStatus.Visible = false;
            this.lblCancelStatusBar.Visible = false;
            this.tbMaster.Visible = true;
        }

        internal delegate void UpdateProgress(int docID, int progress);
        internal UpdateProgress UpdateProgressDelegate;
        /// <summary>
        /// Delegado que actualiza la barra de progreso
        /// </summary>
        /// <param name="progress"></param>
        private void UpdateProgressBar(int docID, int progress)
        {
            if (!this._docProgress.ContainsKey(docID))
                this._docProgress.Add(docID, 0);

            if (this._docProgress[docID] != -2)
            {
                this._docProgress[docID] = progress;
                if (docID == this._currentFormCode)
                {
                    this.pbStatus.Value = progress > 100 ? 100 : progress;
                    if (progress == 0)
                    {
                        this.pbStatus.Visible = true;
                        this.lblCancelStatusBar.Visible = true;
                        this.tbMaster.Visible = false;
                    }
                }
            }
            else
            {
                this._docProgress[docID] = -1;
            }
        }

        internal delegate void ReportStatus(int docID, string status);
        internal ReportStatus ReportStatusDelegate;
        /// <summary>
        /// Informa un estado en la barra de estado
        /// </summary>
        /// <param name="status"></param>
        private void ReportStatusMethod(int docID, string status)
        {
            if (this._docStatus.ContainsKey(docID))
                this._docStatus[docID] = status;
            else
                this._docStatus.Add(docID, status);

            if (docID == this._currentFormCode)
                this.lblStatusMsg.Text = status;
        }

        internal delegate void ShowResultDialog(MessageForm frm);
        internal ShowResultDialog ShowResultDialogDelegate;
        /// <summary>
        /// Delegado que muestra un resultado dentro de un dialogo
        /// </summary>
        private void ShowResultDialogMethod(MessageForm frm)
        {
            frm.ShowDialog();
        }

        #endregion

        #endregion

        #region Variables

        //Obtiene la instancia del controlador base y modulo x defecto
        private BaseController _bc = BaseController.GetInstance();
        private ModulesPrefix _currentModule = ModulesPrefix.apl;
        private FormTypes _currentFormType = FormTypes.Other;
        private int _currentFormCode = Convert.ToInt32(AppForms.MDI);

        //Manejo de ayuda
        private HelpNavigator _navigator = HelpNavigator.TableOfContents;
        private HelpProvider _help = new HelpProvider();

        private Type _lastFormType;
        private static int _frmCode = AppForms.MDI;

        #endregion

        #region Propiedades

        /// <summary>
        /// Asigna un nuevo modulo segun el formulario del usuario
        /// </summary>
        public ModulesPrefix Module
        {
            get { return this._currentModule; }
            set
            {
                if (this._currentModule != value)
                {
                    this._currentModule = value;
                    this.Menu = _bc.GetMenu(value);
                    this.Text = this.ProjectName + " " + _bc.AdministrationModel.Modules[value.ToString()];
                }
            }
        }

        /// <summary>
        /// Asigna la barra de herramienta indicada segun el formulario activo
        /// </summary>
        public FormTypes FormType
        {
            set
            {
                if (this._currentFormType != value)
                {
                    this._currentFormType = value;
                    if (this._currentFormType == FormTypes.Other)
                    {
                        this.CleanToolBar();
                    }
                }
            }
        }

        /// <summary>
        /// Ruta del menu
        /// </summary>
        public string MenuPath
        {
            get;
            set;
        }

        /// <summary>
        /// Ruta del archivo de ayuda
        /// </summary>
        public string HelpPath
        {
            get;
            set;
        }

        /// <summary>
        /// Nombre del proyecto
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Nombre del proyecto
        /// </summary>
        public bool LoadFormTB
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// <param name="update">Indicador de actualizacion de version del programa</param>
        /// <param name="helpPath">Ruta del archivo de ayuda</param>
        /// <param name="mnuPath">Ruta del menu</param>
        /// <param name="pName">Nombre del proyecto</param>
        /// </summary>
        public MDI(string mnuPath, string helpPath, string pName)
        {
            try
            {
                //Variables de inicializacion
                this.ProjectName = pName;
                this.MenuPath = mnuPath;
                this.HelpPath = helpPath;
                FormProvider.Master = this;
                
                _navigator = HelpNavigator.KeywordIndex;
                _help.HelpNamespace = helpPath;

                //Carga el menu principal
                DynamicMenu dm = new DynamicMenu(mnuPath, helpPath);
                _bc.AdministrationModel.Menus = dm.LoadMainMenu();

                //Inicializa el formulario
                InitializeComponent();
                FormProvider.LoadResources(this, _frmCode);

                //Carga el menu
                this.Menu = _bc.GetMenu(this._currentModule);

                //Abre el formulario de login
                LogIn logIn = new LogIn();
                logIn.MdiParent = this;
                logIn.WindowState = FormWindowState.Maximized;
                logIn.Show();

                this.CleanToolBar();
                string resource = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ConnectionStatusBar);
                string msg = string.Format(resource, _bc.BasicConfig.ID.Value);
                this.lblStatusLAN.Text = msg;

                //Variables de hilos
                this._docProgress = new Dictionary<int, int>();
                this._docStatus = new Dictionary<int, string>();

                //Funciones de barra de progreso
                this.UpdateProgressDelegate = new UpdateProgress(this.UpdateProgressBar);
                this.ReportStatusDelegate = new ReportStatus(this.ReportStatusMethod);
                this.DisableProgressDelegate = new DisableProgress(DisableProgressMethod);
                this.ShowResultDialogDelegate = new ShowResultDialog(this.ShowResultDialogMethod);

                //Elementos de barra de progreso
                this.lblCancelStatusBar.Text = _bc.GetResource(LanguageTypes.Forms, AppForms.MDI + "_lblCancel");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI"));
            }
        }

        #region Eventos MDI

        /// <summary>
        /// Sobre escribe el metodo para shotcuts
        /// </summary>
        /// <param name="msg">Mensaje de respuesta</param>
        /// <param name="keyData">Teclas presionadas</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Para que no cambie las pestañas con Ctrl + Tab
            if (keyData == (Keys.Control | Keys.Tab))
                return true;

            return false;
        }

        #endregion

        #region Formularios - Eventos

        /// <summary>
        /// Evento que se ejecuta al cerrar la aplicacion
        /// </summary>
        /// <param name="sender">Formulario</param>
        /// <param name="e">Argumentos del evento</param>
        private void MDI_Closed(object sender, FormClosedEventArgs e)
        {
            _bc.AdministrationModel.CloseChannels();
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="form">Formulario</param>
        /// <param name="prefix">Prefijo del modulo</param>
        /// <param name="frmType">Tipo de formulario</param>
        /// <param name="documentID">Codigo del formulario (unico)</param>
        /// <param name="frmName">nombre del formulario</param>
        /// <param name="enter">Evento que se debe ejecutar al activar el formulario</param>
        /// <param name="close">Evento que se debe ejecutar al cerrar el formulario</param>
        /// <param name="leave">Evento que se debe ejecutar al abandonar el formulario</param>
        internal void Form_Load(Form form, ModulesPrefix prefix, int documentID, string frmName, EventHandler enter, EventHandler leave, FormClosingEventHandler closing, FormClosedEventHandler close)
        {
            try
            {
                FormProvider.LoadResources(form, documentID);
                FormProvider.Active = form;

                form.MdiParent = this;
                form.Enter += new System.EventHandler(enter);
                form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(close);
                form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(closing);
                form.Leave += new System.EventHandler(leave);

                this.AddFormNode(prefix, form.GetType().Name, frmName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppMDI.cs", "Form_Load"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="form">Formulario</param>
        /// <param name="frmType">Tipo de formulario</param>
        /// <param name="prefix">Prefijo del modulo</param>
        internal void Form_Enter(Form form, int frmCode, FormTypes frmType, ModulesPrefix prefix)
        {
            try
            {
                if (frmCode != this._currentFormCode)
                {
                    //FormProvider.Active = form;
                    this._currentFormCode = Convert.ToInt32(frmCode);
                    this.Module = prefix;
                    this.FormType = frmType;

                    //Revisa la barra de progreso
                    if (this._docProgress.ContainsKey(this._currentFormCode) && this._docProgress[this._currentFormCode] != -1 && this._docProgress[this._currentFormCode] != -2)
                    {
                        this.lblStatusMsg.Text = this._docStatus[this._currentFormCode];
                        this.pbStatus.Value = this._docProgress[this._currentFormCode];
                        this.pbStatus.Visible = true;
                        this.lblCancelStatusBar.Visible = true;
                        this.lblStatusMsg.Visible = true;
                        this.tbMaster.Visible = false;
                        this.btnResetSecurity.Visible = false;
                    }
                    else
                    {
                        this.pbStatus.Visible = false;
                        this.lblCancelStatusBar.Visible = false;
                        this.lblStatusMsg.Visible = false;
                        this.lblStatusMsg.Text = string.Empty;
                        this.tbMaster.Visible = true;
                        this.btnResetSecurity.Visible = true;
                    }

                    // * Verifica si tiene una barra de herramientas modificada
                    bool[] sec = FormProvider.GetTB(this._currentFormCode);
                    if (sec == null)
                    {
                        sec = this.GetSecurity(this._currentFormCode);
                        this.LoadFormTB = true;
                    }
                    else
                        this.LoadFormTB = false;

                    this.PrepareTBButtons(this._currentFormType, sec);
                    this._lastFormType = FormProvider.Active.GetType();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="form">Formulario</param>
        /// <param name="frmCode">Codigo del formulario</param>
        internal void Form_Leave(Form form, int frmCode)
        {
            try
            {
                this.SaveCurrentTB(this._currentFormCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-Form_Leave"));
            }

        }

        /// <summary>
        /// Se ejecuta cuando el formularioo se esta cerrando
        /// </summary>
        /// <param name="form">Formulario</param>
        /// <param name="frmCode">Codigo del formulario</param>
        internal void Form_Closing(Form form, int frmCode)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-Form_Leave"));
            }

        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="form">Nombre del formulario</param>
        /// <param name="frmType">Tipo de formulario</param>
        /// <param name="prefix">Prefijo del modulo</param>
        internal void Form_FormClosed(string name, Type type, ModulesPrefix prefix)
        {
            try
            {
                FormProvider.Remove(this._currentFormCode, type);
                this.DeleteFormNode(prefix, type.Name, name);

                if (!FormProvider.HasForms())
                {
                    this._currentFormCode = 0;
                    this.CleanToolBar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Formularios - Operaciones

        /// <summary>
        /// Crea un nuevo nodo en la lista de formularios
        /// </summary>
        /// <param name="pre">Prefijo del módulo></param>
        /// <param name="type">Identificador del tipo de formulario</param>
        /// <param name="frmName">Nombre del formulario</param>
        internal void AddFormNode(ModulesPrefix pre, string type, string frmName)
        {
            try
            {
                //crea el nodo en el arbol
                TreeNode forms = this.tvModules.Nodes[pre.ToString()];
                if (forms != null && !forms.Nodes.ContainsKey(type))
                {
                    forms.Nodes.Add(type, frmName, "TBIconPage.ico", "TBIconPage.ico");
                    this.tvModules.Nodes[pre.ToString()].ExpandAll();
                }

                //Crea la pestaña
                XtraTabPageCollection tabs = this.tabForms.TabPages;
                XtraTabPage tab = new XtraTabPage() { Name = type, Text = frmName };
                if (!tabs.Contains(tab))
                {
                    this.tabForms.TabPages.Add(tab);
                    this.tabForms.SelectedTabPage = tab;
                }

                if (!this.tabForms.Visible)
                {
                    this.tabForms.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-AddFormNode"));
            }
        }

        /// <summary>
        /// Valida si ya existe ese nodo en el listado de formularios abiertos para asiganrlo
        /// </summary>
        /// <param name="pre">Prefijo del módulo></param>
        /// <param name="type">Identificador del tipo de formulario</param>
        /// <param name="frmName">Nombre del formulario</param>
        private void DeleteFormNode(ModulesPrefix pre, string type, string frmName)
        {
            try
            {
                //Elimina el nodo del arbol
                TreeNode forms = this.tvModules.Nodes[pre.ToString()];
                if (forms != null && forms.Nodes.ContainsKey(type))
                    forms.Nodes.RemoveByKey(type);

                //Elimina la pestaña
                XtraTabPage tab = this.GetTabPage(type);
                if (!String.IsNullOrEmpty(tab.Name))
                    this.tabForms.TabPages.Remove(tab);

                if (this.tabForms.TabPages.Count <= 0)
                    this.tabForms.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-DeleteFormNode"));
            }
        }

        #endregion

        #region Operaciones Arbol (Modulos)

        /// <summary>
        /// Indica si un formulario es modal o no
        /// </summary>
        /// <param name="frmType">Tipo del formulario</param>
        /// <returns>Retorna verdadero si el formulario debe ser dialog</returns>
        private bool IsDialog(Type frmType)
        {
            Type parent1 = frmType.BaseType;
            Type parent2 = parent1.BaseType;
            Type parent3 = parent2.BaseType;
            Type parent4 = parent3.BaseType;
            Type parent5 = parent4.BaseType;
            if (parent1 == typeof(FormWithToolbar) || parent2 == typeof(FormWithToolbar) || parent3 == typeof(FormWithToolbar) ||
                parent4 == typeof(FormWithToolbar) || parent5 == typeof(FormWithToolbar))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Llena el arbol con los modulos
        /// </summary>
        internal void LoadModules()
        {
            try
            {
                this.tvOperations.Nodes.Clear();
                this.tvModules.Nodes.Clear();

                List<DTO_aplModulo> mods = _bc.AdministrationModel.aplModulo_GetByVisible(1, false).ToList();
                Dictionary<string, string> modsDictionary = new Dictionary<string, string>();
                for (int i = 0; i < mods.Count(); ++i)
                {
                    var m = mods.ElementAt(i);
                    modsDictionary.Add(m.ModuloID.Value, m.Descriptivo.Value);
                }

                _bc.AdministrationModel.Modules = modsDictionary;
                _bc.AdministrationModel.Start(false);

                TreeNode mNode = new TreeNode();
                string masterRsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Master);
                string documentRsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Document);
                //string documentAprobRsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentAprob);
                string processRsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Process);
                string reportRsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Report);
                string queryRsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Query);
                foreach (var m in _bc.AdministrationModel.Modules)
                {
                    if (m.Key != ModulesPrefix.apl.ToString())
                    {
                        //Arbol de formularios abiertos
                        this.tvModules.Nodes.Add(m.Key, m.Value, "TBIconFolder.ico");
                        //Arbol de operaciones disponibles
                        this.tvOperations.Nodes.Add(m.Key, m.Value, "TBIconFolder.ico");
                        this.tvOperations.Nodes[m.Key].Nodes.Add(FormTypes.Master.ToString().ToLower(), masterRsx, "TBIconFolder.ico");
                        this.tvOperations.Nodes[m.Key].Nodes.Add(FormTypes.Document.ToString().ToLower(), documentRsx, "TBIconFolder.ico");
                        //this.tvOperations.Nodes[m.Key].Nodes.Add(FormTypes.DocumentAprob.ToString().ToLower(), documentAprobRsx, "TBIconFolder.ico");
                        this.tvOperations.Nodes[m.Key].Nodes.Add(FormTypes.Process.ToString().ToLower(), processRsx, "TBIconFolder.ico");
                        this.tvOperations.Nodes[m.Key].Nodes.Add(FormTypes.Query.ToString().ToLower(), queryRsx, "TBIconFolder.ico");
                        this.tvOperations.Nodes[m.Key].Nodes.Add(FormTypes.Report.ToString().ToLower(), reportRsx, "TBIconFolder.ico");
                        this.tvOperations.Sort();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-LoadModules"));
            }
        }

        #region Arbol de operaciones

        /// <summary>
        /// Abre un formulario Modal del arbol de operaciones
        /// </summary>
        /// <param name="nodeOp">Nodo</param>
        /// <param name="nodeNameOp">Nombre del nodo</param>
        private void SelectModalOpForm(TreeNode node)
        {
            string nodeNameOp = node.Name;

            if (node.Level == 0)
            {
                this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), nodeNameOp);
            }
            else if (node.Level == 1)
            {
                string parent = node.Parent.Name;
                this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), parent);
            }
            else if (node.Level == 2)
            {
                string parent = node.Parent.Parent.Name;
                this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), parent);

                Type frmType = FormProvider.GetFormType(nodeNameOp);
                if (this.IsDialog(frmType))
                {
                    bool find = false;
                    Form f = (Form)Activator.CreateInstance(frmType);

                    FormCollection fc = Application.OpenForms;
                    foreach (Form frm in fc)
                    {
                        if (frm.GetType() == frmType)
                        {
                            find = true;
                            frm.BringToFront();

                            break;
                        }
                    }

                    if (!find)
                        f.Show();
                }
                else
                {
                    Form f = FormProvider.GetInstance(frmType);
                }
            }
        }

        /// <summary>
        /// Se dispara al hacer click en un item del arbol de operaciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        internal void tvOperations_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                this.BeginInvoke(new Action(() => this.SelectModalOpForm(e.Node)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-tvModules_NodeMouseClick"));
            }
        }

        /// <summary>
        /// Evento al hacer click o cambiar con teclas un nodo en el arbol de ventanas abiertas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        internal void tvOperations_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TreeNode nodeOp = this.tvOperations.SelectedNode;
                string nodeNameOp = nodeOp.Name;

                if (nodeOp.Level == 0)
                {
                    this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), nodeNameOp);
                }
                else if (nodeOp.Level == 1)
                {
                    string parent = nodeOp.Parent.Name;
                    this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), parent);
                }
                else if (nodeOp.Level == 2)
                {
                    string parent = nodeOp.Parent.Parent.Name;
                    this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), parent);

                    Type frmType = FormProvider.GetFormType(nodeNameOp);
                    if (this.IsDialog(frmType))
                    {
                        Form f = (Form)Activator.CreateInstance(frmType);
                        f.Show();
                    }
                    else
                    {
                        Form f = FormProvider.GetInstance(frmType);
                    }
                }
            }
        }

        #endregion

        #region Arbol de modulos

        /// <summary>
        /// Abre un formulario Modal del arbol de modulos
        /// </summary>
        /// <param name="nodeOp">Nodo</param>
        /// <param name="nodeNameOp">Nombre del nodo</param>
        private void SelectModalModForm(TreeNode node)
        {
            string nodeNameOp = node.Name;

            if (node.Level == 0)
            {
                this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), nodeNameOp);
            }
            else if (node.Level == 1)
            {
                string parent = node.Parent.Name;
                this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), parent);

                Type frmType = FormProvider.GetFormType(nodeNameOp);
                Form f = FormProvider.GetInstance(frmType);
            }
        }

        /// <summary>
        /// Evento al hacer click o cambiar con teclas un nodo en el arbol de operaciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        internal void tvModules_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                this.BeginInvoke(new Action(() => this.SelectModalModForm(e.Node)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-tvModules_NodeMouseClick"));
            }
        }

        /// <summary>
        /// Evento al hacer click o cambiar con teclas un nodo en el arbol de ventanas abiertas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        internal void tvModules_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TreeNode nodeOp = this.tvOperations.SelectedNode;
                string nodeNameOp = nodeOp.Name;

                if (nodeOp.Level == 0)
                {
                    this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), nodeNameOp);
                }
                else if (nodeOp.Level == 1)
                {
                    string parent = nodeOp.Parent.Name;
                    this.Module = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), parent);

                    Type frmType = FormProvider.GetFormType(nodeNameOp);
                    Form f = FormProvider.GetInstance(frmType);
                }
            }
        }

        #endregion

        #endregion

        #region Operaciones Pestañas

        /// <summary>
        /// Evento que se ejecuta al cambiar la pestana seleccionada
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void tabForms_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                if (this.tabForms.SelectedTabPage != null)
                {
                    var name = this.tabForms.SelectedTabPage.Name;
                    if (FormProvider.Active.GetType().Name != name)
                    {
                        Form f = FormProvider.GetInstance(FormProvider.GetFormType(name));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-tabForms_SelectedPageChanged"));
            }
        }

        /// <summary>
        /// Genera la acción del boton cerrar de las pestañas 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void tabForms_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                var page = (XtraTabPage)arg.Page;

                FormProvider.Close(FormProvider.GetFormType(page.Name));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-tabForms_CloseButtonClick"));
            }
        }

        /// <summary>
        /// Trae una pestaña segun el nombre
        /// </summary>
        /// <param name="name">Nombre</param>
        /// <returns>Devuelve la pagina deseada</returns>
        public XtraTabPage GetTabPage(string name)
        {
            try
            {
                foreach (XtraTabPage tp in this.tabForms.TabPages)
                {
                    if (tp.Name == name)
                    {
                        return tp;
                    }
                }

                return new XtraTabPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MDI.cs", "MDI.cs-GetTabPage"));

                return new XtraTabPage();
            }
        }

        #endregion

        #region Operaciones Barra de Herramientas

        #region Funciones TB

        /// <summary>
        /// Limpia la barra de herramientas
        /// </summary>
        private void CleanToolBar()
        {
            foreach (var item in this.tbMaster.Items)
            {
                try
                {
                    if (item is ToolStripButton)
                    {
                        ToolStripButton b = (ToolStripButton)item;
                        b.Visible = false;
                        b.Enabled = false;
                    }
                    else if (item is ToolStripSeparator)
                    {
                        ToolStripSeparator s = (ToolStripSeparator)item;
                        s.Visible = false;
                    }
                }
                catch (InvalidCastException e)
                {
                    //Se ignora la excepcion, ocurre cuando el elemento de la barra de herramientas no es un boton
                    //por ejemplo el separador.
                }
            }
        }

        /// <summary>
        /// Asigan los botonas básicos de la barra de herramientas
        /// </summary>
        private void SetBasicTB(bool[] sec)
        {
            this.tbBreak2.Visible = true;
            this.itemAlarm.Visible = true;
            this.itemHelp.Visible = true;
            this.itemClose.Visible = true;

            this.itemNew.Enabled = sec[(int)FormsActions.Add];
            this.itemSave.Enabled = sec[(int)FormsActions.Edit];
            this.itemDelete.Enabled = sec[(int)FormsActions.Delete];
            this.itemPrint.Enabled = sec[(int)FormsActions.Print];
            this.itemGenerateTemplate.Enabled = sec[(int)FormsActions.GenerateTemplate];
            this.itemCopy.Enabled = sec[(int)FormsActions.Copy];
            this.itemPaste.Enabled = sec[(int)FormsActions.Paste];
            this.itemImport.Enabled = sec[(int)FormsActions.Import];
            this.itemExport.Enabled = sec[(int)FormsActions.Export];
            this.itemRevert.Enabled = sec[(int)FormsActions.Revert];
            this.itemSendtoAppr.Enabled = sec[(int)FormsActions.SendtoAppr];

            this.itemUpdate.Enabled = true;
            this.itemSearch.Enabled = true;
            this.itemFilter.Enabled = true;
            this.itemFilterDef.Enabled = true;

            this.itemAlarm.Enabled = true;
            this.itemHelp.Enabled = true;
            this.itemClose.Enabled = true;
        }

        /// <summary>
        /// Segun el tipo de formulario ordena la barra de herramientas
        /// </summary>
        /// <param name="formType">Tipo del formulario</param>
        /// <param name="sec">Arreglo de seguridades</param>
        private void PrepareTBButtons(FormTypes formType, bool[] sec)
        {
            this.CleanToolBar();

            this.SetBasicTB(sec);
            switch (formType)
            {
                case FormTypes.Master:
                    if (this._currentFormCode == AppMasters.seUsuario)
                    {
                        this.itemResetPassword.Visible = true;
                        this.itemResetPassword.Enabled = sec[(int)FormsActions.SpecialRights];
                    }

                    this.itemNew.Visible = true;
                    this.itemSave.Visible = true;
                    this.itemDelete.Visible = true;
                    this.tbBreak.Visible = true; 
                    //
                    this.itemPrint.Visible = true;
                    this.itemSearch.Visible = true;
                    this.itemFilter.Visible = true;
                    this.itemFilterDef.Visible = true;
                    this.tbBreak0.Visible = true; 
                    //
                    this.itemGenerateTemplate.Visible = true;
                    this.itemImport.Visible = true;
                    this.itemExport.Visible = true;
                    this.tbBreak1.Visible = true; 
                    //
                    this.itemUpdate.Visible = true;
                    this.itemUpdate.Enabled = true;
                    break;

                case FormTypes.Document:
                    this.itemNew.Visible = true;
                    this.itemSave.Visible = true;
                    this.itemDelete.Visible = true;
                    this.tbBreak.Visible = true;
                    //
                    this.itemPrint.Visible = true;
                    this.itemFilter.Visible = true;
                    this.itemFilterDef.Visible = true;
                    this.tbBreak0.Visible = true;
                    //
                    this.itemGenerateTemplate.Visible = true;
                    this.itemCopy.Visible = true;
                    this.itemPaste.Visible = true;
                    this.itemExport.Visible = true;
                    this.tbBreak1.Visible = true; 
                    //
                    this.itemRevert.Visible = true;
                    this.itemSendtoAppr.Visible = true;
                    break;

                case FormTypes.DocumentAprob:
                    this.itemSave.Visible = true;
                    this.itemSave.Enabled = sec[(int)FormsActions.Approve] || sec[(int)FormsActions.Edit];
                    this.itemUpdate.Visible = true;
                    break;

                case FormTypes.Query:
                    this.itemSearch.Visible = true;
                    this.itemSearch.Enabled = true;
                    this.tbBreak1.Visible = true; 
                    //
                    this.itemUpdate.Visible = true;
                    break;

                case FormTypes.Bitacora:
                    this.itemFilter.Visible = true;
                    this.itemPrint.Visible = true;
                    this.itemExport.Visible = true;
                    break;

                case FormTypes.Control:
                    this.itemSave.Visible = true;
                    this.itemSave.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Trae un arreglo con los permisos de un documento
        /// </summary>
        /// <param name="docId">Identificador del documento</param>
        private void SaveCurrentTB(int docId)
        {
            try
            {
                bool[] arr = new bool[15];

                arr[(int)FormsActions.Get] = true;
                arr[(int)FormsActions.Add] = this.itemNew.Enabled;
                arr[(int)FormsActions.Edit] = this.itemSave.Enabled;
                arr[(int)FormsActions.Delete] = this.itemDelete.Enabled;
                arr[(int)FormsActions.Print] = this.itemPrint.Enabled;
                arr[(int)FormsActions.GenerateTemplate] = this.itemGenerateTemplate.Enabled;
                arr[(int)FormsActions.Copy] = this.itemCopy.Enabled;
                arr[(int)FormsActions.Paste] = this.itemPaste.Enabled;
                arr[(int)FormsActions.Import] = this.itemImport.Enabled;
                arr[(int)FormsActions.Export] = this.itemExport.Enabled;
                arr[(int)FormsActions.SpecialRights] = this.itemResetPassword.Enabled;
                arr[(int)FormsActions.Revert] = this.itemRevert.Enabled;
                arr[(int)FormsActions.SendtoAppr] = this.itemSendtoAppr.Enabled;
                arr[(int)FormsActions.Approve] = this.itemSave.Enabled;
                arr[(int)FormsActions.Search] = this.itemSearch.Enabled;

                FormProvider.SetTB(docId, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae un arreglo con los permisos de un documento
        /// </summary>
        /// <param name="docId">Identificador del documento</param>
        private bool[] GetSecurity(int docId)
        {
            try
            {
                bool[] arr = new bool[15];

                arr[(int)FormsActions.Get] = SecurityManager.HasAccess(docId, FormsActions.Get);
                arr[(int)FormsActions.Add] = SecurityManager.HasAccess(docId, FormsActions.Add);
                arr[(int)FormsActions.Edit] = SecurityManager.HasAccess(docId, FormsActions.Edit);
                arr[(int)FormsActions.Delete] = SecurityManager.HasAccess(docId, FormsActions.Delete);
                arr[(int)FormsActions.Print] = SecurityManager.HasAccess(docId, FormsActions.Print);
                arr[(int)FormsActions.GenerateTemplate] = SecurityManager.HasAccess(docId, FormsActions.GenerateTemplate);
                arr[(int)FormsActions.Copy] = SecurityManager.HasAccess(docId, FormsActions.Copy);
                arr[(int)FormsActions.Paste] = SecurityManager.HasAccess(docId, FormsActions.Paste);
                arr[(int)FormsActions.Import] = SecurityManager.HasAccess(docId, FormsActions.Import);
                arr[(int)FormsActions.Export] = SecurityManager.HasAccess(docId, FormsActions.Export);
                arr[(int)FormsActions.SpecialRights] = SecurityManager.HasAccess(docId, FormsActions.SpecialRights);
                arr[(int)FormsActions.Revert] = SecurityManager.HasAccess(docId, FormsActions.Revert);
                arr[(int)FormsActions.SendtoAppr] = SecurityManager.HasAccess(docId, FormsActions.SendtoAppr);
                arr[(int)FormsActions.Approve] = SecurityManager.HasAccess(docId, FormsActions.Approve);
                arr[(int)FormsActions.Search] = SecurityManager.HasAccess(docId, FormsActions.Search);

                return arr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Eventos botones

        /// <summary>
        /// Evento al presionar el boton nuevo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemNew_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBNew();
            }
        }

        /// <summary>
        /// Evento al presionar el boton guardar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemSave_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBSave();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de eliminar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemDelete_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBDelete();
            }     
        }

        /// <summary>
        /// Evento al presionar el boton de eliminar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemEdit_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBEdit();
            }
        }

        /// <summary>
        /// Evento al presionar el boton imprimir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemPrint_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBPrint();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de Búsqueda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemSearch_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBSearch();
            }
        }

        /// <summary>
        /// Evento al presionar el boton Filtro
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemFilter_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBFilter();
            }
        }

        /// <summary>
        /// Evento al presionar el boton Filtro por defecto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemFilterDef_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBFilterDef();
            }
        }

        /// <summary>
        /// Evento al presionar el boton importar datos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemImport_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBImport();
            }
        }

        /// <summary>
        /// Evento al presionar el boton exportar datos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemExport_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBExport();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de generar plantilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemGenerateTemplate_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBGenerateTemplate();
            }
        }

        /// <summary>
        /// Evento al presionar el boton copiar datos del documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemCopy_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBCopy();
            }
        }

        /// <summary>
        /// Evento al presionar el boton pegar datos en documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemPaste_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBPaste();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de resetear pwd
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemUpdate_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBUpdate();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de resetear pwd
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemResetPassword_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBResetPwd();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de revertir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemRevert_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBRevert();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de revertir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemSendtoAppr_Click(object sender, EventArgs e)
        {
            if (FormProvider.Active is FormWithToolbar)
            {
                FormWithToolbar ftb = (FormWithToolbar)FormProvider.Active;
                ftb.TBSendtoAppr();
            }
        }

        /// <summary>
        /// Evento al presionar el boton de alarmas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemAlarm_Click(object sender, EventArgs e)
        {
            AlarmForm frmAlarm = new AlarmForm();
            frmAlarm.ShowDialog();
        }

        /// <summary>
        /// Evento al presionar el boton de ayuda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemHelp_Click(object sender, EventArgs e)
        {
            //Verifica la existencia del archivo de ayuda y lo abre
            if (!System.IO.File.Exists(_help.HelpNamespace))
            {
                MessageBox.Show(_bc.GetResourceForException(new Exception("ERR_FIL_0002&&" + _help.HelpNamespace), "WinApp-MDI.cs", "MDI-itemHelp_Click"));
            }
            else
            {
                Help.ShowHelp(FormProvider.Master, _help.HelpNamespace, _navigator, Convert.ToString(_currentFormCode));
            }
        }

        /// <summary>
        /// Evento al presionar el boton de cerrar formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void itemClose_Click(object sender, EventArgs e)
        {
            try
            {
                FormProvider.CloseCurrent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Operaciones Barra de Estado

        /// <summary>
        /// Valida la seleccion de una nueva empresa
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void UpdateCompany_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem.Text != _bc.AdministrationModel.Empresa.Descriptivo.Value)
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgChangeCompany = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ChangeCompany);

                    if (MessageBox.Show(msgChangeCompany, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DTO_MasterBasic master = _bc.AdministrationModel.MasterSimple_GetByDesc(AppMasters.glEmpresa, e.ClickedItem.Text);
                        if (master != null)
                        {
                            this.Enabled = false;
                            this.lblStatusCompany.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.CargandoInfo);

                            #region Carga la info de la empresa y del usuario

                            //Trae la empresa para trabajar
                            DTO_glEmpresa emp = (DTO_glEmpresa)master;
                            //Cierra formularios abiertos
                            FormProvider.CloseAll();
                            this.CleanToolBar();
                            //Actualizar la empresa en la persistencia
                            _bc.AdministrationModel.Empresa = emp;
                            _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, emp.NumeroControl.Value).ToList();

                            //Asigna el indicador de multimoneda para la empresa
                            string multimoneda = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
                            _bc.AdministrationModel.MultiMoneda = multimoneda == "1" ? true : false;

                            // Actualizacion de la barra de estado
                            FormProvider.Master.lblStatusCompany.Text = emp.Descriptivo.Value;
                            #endregion
                            #region Asignacion de seguridades del usuario
                            int userId = _bc.AdministrationModel.User.ReplicaID.Value.Value;
                            var rsx = _bc.AdministrationModel.seGrupoDocumento_GetByUsuarioId(userId, emp.ID.Value);

                            string outp = string.Empty;
                            _bc.AdministrationModel.FormsSecurity.Clear();

                            foreach (var s in rsx)
                            {
                                _bc.AdministrationModel.FormsSecurity.TryGetValue(s.DocumentoID.ToString(), out outp);
                                if (string.IsNullOrEmpty(outp))
                                {
                                    _bc.AdministrationModel.FormsSecurity.Add(s.DocumentoID.ToString(), s.AccionesPerm.ToString());
                                }
                                else
                                {
                                    long oldV = Convert.ToInt64(outp);
                                    long newV = (long)s.AccionesPerm.Value;
                                    long ret = SecurityManager.SetFormSecurity(oldV, newV);

                                    _bc.AdministrationModel.FormsSecurity[s.DocumentoID.ToString()] = ret.ToString();
                                }
                            }
                            #endregion
                            #region Carga de menus y configuracion

                            _bc.AdministrationModel.IniciarEmpresaUsuario(true);

                            FormProvider.Master.LoadModules();
                            DynamicMenu dm = new DynamicMenu(FormProvider.Master.MenuPath, FormProvider.Master.HelpPath);
                            _bc.AdministrationModel.Menus = dm.LoadMenu();

                            _bc.AdministrationModel.IniciarEmpresaUsuario(false);
                            #endregion
                            #region Carga de tablas para el grupo de empresas asignado
                            Dictionary<int, string> empGrupo = new Dictionary<int, string>();

                            empGrupo.Add((int)GrupoEmpresa.Automatico, emp.ID.Value);
                            empGrupo.Add((int)GrupoEmpresa.Individual, emp.EmpresaGrupoID_.Value);
                            empGrupo.Add((int)GrupoEmpresa.General, _bc.GetControlValue(AppControl.GrupoEmpresaGeneral));

                            _bc.AssignTablesByCompany(empGrupo);
                            #endregion
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppMDI.cs", "UpdateCompany_Click"));
            }
            finally
            {
                this.Enabled = true;
            }
        }

        /// <summary>
        /// Revisa el estado del proceso que se este corriendo enel servidor
        /// </summary>
        internal void CheckServerProcessStatus(object docID)
        {
            try
            {
                int doc = (int)docID;
                this.Invoke(this.UpdateProgressDelegate, new object[] { doc, 0 });
                while (true)
                {
                    Thread.Sleep(1000);
                    int progress = FuncProgressBarThread.Invoke();
                    this.Invoke(this.UpdateProgressDelegate, new object[] { doc, progress });
                    if(this._currentFormCode == doc)
                        this.lblCancelStatusBar.Visible = false;
                }
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Revisa si un proceso de un documento esta activo o no
        /// </summary>
        /// <param name="docID">Identificador del documento que ejecuta el proceso</param>
        /// <returns>Retorna true si el proceso se cancelo de lo contrario retorna falso</returns>
        internal bool ProcessCanceled(int docID)
        {
            if (this._docProgress.ContainsKey(docID) && (this._docProgress[docID] == -1 || this._docProgress[docID] == -2))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Cancela el hilo que se este ejecutando con la barra de estado
        /// </summary>
        internal void StopProgressBarThread(int docID)
        {
            try
            {
                this.Invoke(this.ReportStatusDelegate, new object[] { docID, string.Empty });

                if (docID == this._currentFormCode)
                    this.Invoke(this.DisableProgressDelegate);

                if (this._docProgress.ContainsKey(docID))
                    this._docProgress[docID] = -1;

                if(this.ProgressBarThread != null)
                    this.ProgressBarThread.Abort();
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_StopThread));
            }
        }

        /// <summary>
        /// Permite cambiar las seguridades del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetSecurity_Click(object sender, EventArgs e)
        {
            try
            {
                #region Asignacion de seguridades del usuario
                int userId = _bc.AdministrationModel.User.ReplicaID.Value.Value;
                var rsx = _bc.AdministrationModel.seGrupoDocumento_GetByUsuarioId(userId, this._bc.AdministrationModel.Empresa.ID.Value);

                string outp = string.Empty;
                _bc.AdministrationModel.FormsSecurity.Clear();

                foreach (var s in rsx)
                {
                    _bc.AdministrationModel.FormsSecurity.TryGetValue(s.DocumentoID.ToString(), out outp);
                    if (string.IsNullOrEmpty(outp))
                    {
                        _bc.AdministrationModel.FormsSecurity.Add(s.DocumentoID.ToString(), s.AccionesPerm.ToString());
                    }
                    else
                    {
                        long oldV = Convert.ToInt64(outp);
                        long newV = (long)s.AccionesPerm.Value;
                        long ret = SecurityManager.SetFormSecurity(oldV, newV);

                        _bc.AdministrationModel.FormsSecurity[s.DocumentoID.ToString()] = ret.ToString();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.In_UserAnyPermission));
            }
        }

        #endregion

        #region Eventos Barra de estado

        /// <summary>
        /// Cancelar proceso barra de estado al entrar el mouse
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lblCancelStatusBar_MouseEnter(object sender, EventArgs e)
        {
            ToolStripStatusLabel lbl = (sender as ToolStripStatusLabel);
            if (lbl != null)
            {
                lbl.Font = new Font(lbl.Font, FontStyle.Bold);
            }
            this.Cursor = Cursors.Hand;
        }

        /// <summary>
        ///  No Cancelar proceso barra de estado al salir el mouse
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lblCancelStatusBar_MouseLeave(object sender, EventArgs e)
        {
            ToolStripStatusLabel lbl = (sender as ToolStripStatusLabel);
            if (lbl != null)
            {
                lbl.Font = new Font(lbl.Font, FontStyle.Regular);
            }
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Acción de cancelar de la barra de estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCancelStatusBar_Click(object sender, EventArgs e)
        {
            if (this._docProgress.ContainsKey(this._currentFormCode))
            {
                this._docProgress[this._currentFormCode] = -2;
                this.tbMaster.Visible = true;
            }
        }

        #endregion
    }
}