using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Summary description for clsDynamicMenu.
    /// </summary>
    public class DynamicMenu
    {
        #region Variables

        /// <summary>
        /// Max numero de items en el menu principal
        /// </summary>
        private int _max = 200;

        /// <summary>
        /// Instancia del base controller para saber el diccionario y las traduccionesS
        /// </summary>
        private BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Ruta del archivo que contiene el menu
        /// </summary>
        private static string _mnu_path;

        /// <summary>
        /// Ruta del archivo que contiene la ayuda
        /// </summary>
        private static string _help_path;

        /// <summary>
        /// Navegador para la tabla de contenidos del archivo de ayuda
        /// </summary>
        private static HelpNavigator _navigator;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="form">Formulario q inicia el menu</param>
        /// <param name="mnuPath">Ruta del archivo de menu</param>
        /// <param name="helpPath">Ruta del archivo de ayuda</param>
        public DynamicMenu(string mnuPath, string helpPath)
        {
            _mnu_path = mnuPath;
            _help_path = helpPath;

            _navigator = HelpNavigator.TableOfContents;
            _navigator = HelpNavigator.KeywordIndex;
        }

        #endregion

        #region Funciones

        /// <summary>
        /// Usa el algoritmo BFS para cerar el menu
        /// </summary>
        /// <param name="module">Modulo para cargar el menu</param>
        /// <param name="node">Nodo</param>
        /// <param name="global">Indica si es el menu genérico</param>
        /// <returns>Retirna un menu para mostrarlo en el formulario</returns>
        private MainMenu CreateMenu(string module, XmlNode node, bool global)
        {
            try
            {
                MainMenu miMenu = new MainMenu();
                ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), module);

                int first = 1, last = 0, level = 0;

                //Declara una cola de nodos XML para almacenar items principales del menu
                XmlNode[] queue = new XmlNode[_max];

                //Agrega a la cola el primer item
                last++;
                queue[last] = node;

                while (first <= last)
                {
                    //Obtiene un item de la ola y lo asigna a  una variable temporal
                    XmlNode temp = queue[first++];

                    //Declara un arreglo de MenuItem para almacenar un nodo del menu actual
                    MenuItem[] mnuItem = new MenuItem[temp.ChildNodes.Count];

                    //Si el nodo actual no tiene hijos continue
                    if (temp.ChildNodes.Count == 0)
                    {
                        continue;
                    }

                    for (int j = 0; j < temp.ChildNodes.Count; j++)
                    {
                        //Toma un hijo del nodo actual
                        XmlNode child = temp.ChildNodes.Item(j);

                        //Obtiene los atributos del nodo
                        string value = child.Attributes["value"] != null ? child.Attributes["value"].InnerText : string.Empty;
                        string e = child.Attributes["event"] != null ? child.Attributes["event"].InnerText : string.Empty;
                        string f = child.Attributes["form"] != null ? child.Attributes["form"].InnerText : string.Empty;
                        string id = child.Attributes["formId"] != null ? child.Attributes["formId"].InnerText : string.Empty;
                        string h = child.Attributes["nodeId"] != null ? child.Attributes["nodeId"].InnerText : string.Empty;
                        string n = child.Attributes["node"] != null ? child.Attributes["node"].InnerText : string.Empty;

                        //Obtiene la traducción del menu
                        string rsx = string.Empty;
                        if (f == "LiquidacionVacacionesColectivas" || f == "AprobacionGiroRechazo" || f == "PagosEspeciales")
                        {
                            rsx = _bc.GetResource(LanguageTypes.Menu, value);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(e) && !global)
                                rsx = _bc.GetResource(LanguageTypes.Forms, id);
                            else
                                rsx = _bc.GetResource(LanguageTypes.Menu, value);
                        }
                        //Obtiene el atributo del nivel 
                        level = Convert.ToInt32(child.Attributes["level"].InnerText);

                        //Asigna a la cola de hijos un MenuItem con el evento correspondiente
                        if (String.IsNullOrEmpty(e))
                            mnuItem[j] = new MenuItem(rsx);
                        else
                            mnuItem[j] = new MenuItem(rsx, this.AddEvent(value, n.ToLower(), h, f, id, e, mod));

                        //Asigna el nombre de la forma para poder identificarlo
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(e) && !global)
                        {
                            if (e.Equals("OpenForm_clicked") || e.Equals("OpenFormDialog_clicked") || e.Equals("OpenFormModal_clicked"))
                                mnuItem[j].Enabled = SecurityManager.HasAccess(Convert.ToInt32(id), FormsActions.Get);
                        }

                        //Agrega el nuevo nodo a la cola de hijos
                        queue[++last] = child;

                        // father: es la posicion donde se encuentra inmediato
                        // grantfather: posicion del papa del father
                        // great_grantfather: posicion del papa del grantfather
                        // Ejemplo: Para el segundo menu, segunda posicion y tercer item seria:
                        // father: 2 / grantfather: 1  / great_grantfather: 1
                        // *** Los separadores del menu se identifican porque el texto tiene un "-"
                        if (level == 0)
                        {
                            miMenu.MenuItems.Add(mnuItem[j]);
                        }
                        else if (level == 1)
                        {
                            int father = Convert.ToInt32(child.Attributes["father"].InnerText);
                            miMenu.MenuItems[father].MenuItems.Add(mnuItem[j]);
                        }
                        else if (level == 2)
                        {
                            int grantfather = Convert.ToInt32(child.Attributes["grantfather"].InnerText);
                            int father = Convert.ToInt32(child.Attributes["father"].InnerText);
                            miMenu.MenuItems[grantfather].MenuItems[father].MenuItems.Add(mnuItem[j]);
                        }
                        else if (level == 3)
                        {
                            int grantfather = Convert.ToInt32(child.Attributes["grantfather"].InnerText);
                            int father = Convert.ToInt32(child.Attributes["father"].InnerText);
                            int great_grantfather = Convert.ToInt32(child.Attributes["great_grantfather"].InnerText);
                            miMenu.MenuItems[great_grantfather].MenuItems[grantfather].MenuItems[father].MenuItems.Add(mnuItem[j]);
                        }
                    }
                }

                return miMenu;
            }
            catch (Exception ex)
            {
                return new MainMenu();
            }
        }

        /// <summary>
        /// Carga elmenu del inicio
        /// </summary>
        /// <returns>Retorna el man por defecto del sitio</returns>
        public Dictionary<string, MainMenu> LoadMainMenu()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_mnu_path);
                XmlNode root = doc.DocumentElement;

                Dictionary<string, MainMenu> menus = new Dictionary<string, MainMenu>();

                for (int j = 0; j < root.ChildNodes.Count; ++j)
                {
                    XmlNode node = root.ChildNodes.Item(j);
                    string module = node.Attributes["module"].InnerText;

                    if (module == ModulesPrefix.apl.ToString())
                        menus.Add(module, this.CreateMenu(module, node, true));
                }

                return menus;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Carga todos los menus del sitio
        /// </summary>
        /// <returns>Retorna un menu</returns>
        public Dictionary<string, MainMenu> LoadMenu()
        {
            try
            {
                //Formatea el menu izquierdo
                foreach(TreeNode nodeMod in FormProvider.Master.tvOperations.Nodes)
                {
                    foreach (TreeNode nodeOper in nodeMod.Nodes)
                        nodeOper.Nodes.Clear();
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(_mnu_path);
                XmlNode root = doc.DocumentElement;

                Dictionary<string, MainMenu> menus = new Dictionary<string, MainMenu>();

                for (int j = 0; j < root.ChildNodes.Count; ++j)
                {
                    XmlNode node = root.ChildNodes.Item(j);
                    string module = node.Attributes["module"].InnerText;

                    menus.Add(module, this.CreateMenu(module, node, false));
                }

                return menus;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Crea una función genérica para cuando le dan click a un formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Mnu_clicked(object sender, System.EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string iText = item.Text.ToString();
            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidEvent);
            MessageBox.Show(msg + iText);
        }

        /// <summary>
        /// Cierra el formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Exit_clicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Cierra el formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void CloseForm_clicked(object sender, EventArgs e)
        {
            //FormProvider.Active.Close();
            Application.Exit();
        }

        /// <summary>
        /// Actualiza la información de la LAN
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void UpdateControl_clicked(object sender, EventArgs e)
        {
            string numeroControl = _bc.AdministrationModel.Empresa.NumeroControl.Value;
            _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, numeroControl).ToList();
        }

        /// <summary>
        /// Abre un formulario
        /// </summary>
        /// <param name="form">Nombre del formulario</param>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void OpenForm_clicked(TreeNode node, string form, ModulesPrefix mod, object sender, EventArgs e)
        {
            FormProvider.Master.tvOperations.SelectedNode = node;
            Form f = null;

            //Valida si es el modulo de CF y algunas pantallas particulares
            if (mod == ModulesPrefix.cf && !form.Equals("GarantiaControl") && !form.Equals("glTerceroReferencia") && !form.Equals("ReintegroClientes") &&
                                           !form.Equals("glGarantia") && !form.Equals("glIncumplimientoEtapa") && !form.Equals("glZona") && !form.Equals("ccReintegroSaldo"))
                f = FormProvider.GetInstance(FormProvider.GetFormType(form), mod);
            else
                f = FormProvider.GetInstance(FormProvider.GetFormType(form));
        }

        /// <summary>
        /// Abre un formulario de carga de procesos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void OpenFormDialog_clicked(string form, object sender, EventArgs e)
        {
            Type t = FormProvider.GetFormType(form);
            Form f = (Form)Activator.CreateInstance(t);
            f.ShowDialog();
        }

        /// <summary>
        /// Abre un formulario para edicion de reportes
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void OpenFormReports_clicked(string docID, object sender, EventArgs e)
        {
            int id = Convert.ToInt32(docID);
            CustomizeReport report = new CustomizeReport(id);
        }

        /// <summary>
        /// Abre un reporte
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void OpenFormModal_clicked(string form, object sender, EventArgs e)
        {
            Type t = FormProvider.GetFormType(form);
            Form f = (Form)Activator.CreateInstance(t);

            bool find = false;
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.GetType() == t)
                {
                    find = true;
                    frm.BringToFront();

                    break;
                }
            }

            if(!find)
                f.Show();
        }

        /// <summary>
        /// Abre la Ayuda
        /// </summary>
        /// <param name="id">Id del formulario</param>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void OpenHelp_clicked(string id, object sender, EventArgs e)
        {
            //Verifica la existencia del archivo de ayuda y lo abre
            if (!System.IO.File.Exists(_help_path))
            {
                MessageBox.Show(_bc.GetResourceForException(new Exception("ERR_FIL_0002&&" + _help_path), "WinApp", "DynamicMenu-OpenHelp_clicked"));
            }
            else
            {
                Help.ShowHelp(FormProvider.Master, _help_path, _navigator, id);
            }
        }

        /// <summary>
        /// Abre un formulario de cambio de contraseña
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void ResetPassword_clicked(object sender, EventArgs e)
        {
            RenewPassword rp = new RenewPassword(_bc.AdministrationModel.User.ContrasenaFecCambio.Value.Value);
            rp.ShowDialog();
        }

        #endregion

        #region Manejo de Eventos

        /// <summary>
        /// Asigna un evento al menu
        /// </summary>
        /// <param name="n">Nombre del tipo de formulario (maestra, documento, etc)</param>
        /// <param name="h">Codigo del nodo para el archivo de ayuda</param>
        /// <param name="f">Nombre del formulario</param>
        /// <param name="id">Id del formulario</param>
        /// <param name="e">Evento</param>
        private System.EventHandler AddEvent(string value, string n, string h, string f, string id, string e, ModulesPrefix mod)
        {
            switch (e)
            {
                case "Exit_clicked":
                    return new System.EventHandler(this.Exit_clicked);
                
                case "CloseForm_clicked":
                    return new System.EventHandler(this.CloseForm_clicked);

                case "UpdateControl_clicked":
                    return new System.EventHandler(this.UpdateControl_clicked);

                case "OpenForm_clicked":
                    TreeNode node = FormProvider.Master.tvOperations.Nodes[h];
                    if (node != null)
                    {
                        TreeNode subNode = node.Nodes[n];
                        string frmName = string.Empty;
                        if (f == "LiquidacionVacacionesColectivas" || f == "PagosEspeciales")
                        {
                            frmName = _bc.GetResource(LanguageTypes.Menu, value);
                        }
                        else
                        {
                            frmName = _bc.GetResource(LanguageTypes.Forms, id);
                        }
                        
                        if (subNode != null && !string.IsNullOrEmpty(id) && SecurityManager.HasAccess(Convert.ToInt32(id), FormsActions.Get))
                        {
                            subNode.Nodes.Add(f, frmName, "TBIconPage.ico", "TBIconPage.ico");
                        }
                    }
                    return new System.EventHandler((sender, ev) => { this.OpenForm_clicked(node, f, mod, sender, ev); });

                case "OpenFormDialog_clicked":
                    TreeNode nodeDialog = FormProvider.Master.tvOperations.Nodes[h];
                    if (nodeDialog != null)
                    {
                        TreeNode subNode = nodeDialog.Nodes[n];
                        string frmName = _bc.GetResource(LanguageTypes.Forms, id);
                        if (subNode != null && !string.IsNullOrEmpty(id) && SecurityManager.HasAccess(Convert.ToInt32(id), FormsActions.Get))
                        {
                            subNode.Nodes.Add(f, frmName, "TBIconPage.ico", "TBIconPage.ico");
                        }
                    }
                    return new System.EventHandler((sender, ev) => { this.OpenFormDialog_clicked(f, sender, ev); });

                case "OpenFormReports_clicked":
                    TreeNode nodeFormReports = FormProvider.Master.tvOperations.Nodes[h];
                    if (nodeFormReports != null)
                    {
                        TreeNode subNode = nodeFormReports.Nodes[n];
                        string frmName = _bc.GetResource(LanguageTypes.Forms, id);
                        if (subNode != null && !string.IsNullOrEmpty(id) && SecurityManager.HasAccess(Convert.ToInt32(id), FormsActions.Get))
                        {
                            subNode.Nodes.Add(f, frmName, "TBIconPage.ico", "TBIconPage.ico");
                        }
                    }

                    return new System.EventHandler((sender, ev) => { this.OpenFormReports_clicked(id, sender, ev); });

                case "OpenFormModal_clicked":
                    TreeNode nodeReports = FormProvider.Master.tvOperations.Nodes[h];
                    if (nodeReports != null)
                    {
                        TreeNode subNode = nodeReports.Nodes[n];
                        string frmName = _bc.GetResource(LanguageTypes.Forms, id);
                        if (subNode != null && !string.IsNullOrEmpty(id) && SecurityManager.HasAccess(Convert.ToInt32(id), FormsActions.Get))
                        {
                            subNode.Nodes.Add(f, frmName, "TBIconPage.ico", "TBIconPage.ico");
                        }
                    }
                    return new System.EventHandler((sender, ev) => { this.OpenFormModal_clicked(f, sender, ev); });

                case "GetAlarms_clicked":
                    return new System.EventHandler((sender, ev) => { this.OpenFormDialog_clicked(f, sender, ev); });

                case "OpenHelp_clicked":
                    return new System.EventHandler((sender, ev) => { this.OpenHelp_clicked(h, sender, ev); });

                case "ResetPassword_clicked":
                    return new System.EventHandler(this.ResetPassword_clicked);

                default:
                    return new System.EventHandler(Mnu_clicked);
            }
        }

        #endregion

    }

}
