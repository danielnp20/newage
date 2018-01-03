using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Librerias.ExceptionHandler;
using Microsoft.VisualBasic.ApplicationServices;
using NewAge.DTO.Resultados;
using System.Data.SqlClient;
using System.Reflection;
using DevExpress.Skins;
using System.Deployment.Application;

namespace NewAge.Cliente.GUI.WinApp
{
    public class Program : WindowsFormsApplicationBase
    {
        #region Variables

        // Variable que maneja la persistencia y fachada de las operaciones
        private static BaseController _bc;

        /// <summary>
        /// Ruta del proyecto
        /// </summary>
        private static string ProjectPath
        {
            set;
            get;
        }

        /// <summary>
        /// Información de archivos binarios
        /// </summary>
        private static string FilesPath
        {
            set;
            get;
        }

        /// <summary>
        /// Extension de archivos binarios
        /// </summary>
        private static string BinExt
        {
            set;
            get;
        }

        /// <summary>
        /// Formato de archivos de configuracion
        /// </summary>
        private static string ConfigFormat
        {
            set;
            get;
        }

        /// <summary>
        /// Ruta de archivos de configuraciob
        /// </summary>
        private static string ConfigPath
        {
            set;
            get;
        }

        /// <summary>
        /// Formato de idiomas
        /// </summary>
        private static string LangFormat
        {
            set;
            get;
        }

        /// <summary>
        /// Ruta de idiomas
        /// </summary>
        private static string LangPath
        {
            set;
            get;
        }

        /// <summary>
        /// Ruta de archivo de ayuda
        /// </summary>
        private static string HelpPath
        {
            set;
            get;
        }

        /// <summary>
        /// Ruta de archivo de menu
        /// </summary>
        private static string MenuPath
        {
            get;
            set;
        }

        /// <summary>
        /// Nmobre del projecto
        /// </summary>
        private static string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Pantalla que informa el proceso que se esta ejecutando
        /// </summary>
        private static SplashScreen Splash;

        // Variables de version (v: servidor / c: actual)
        private static int vLanguage;
        private static Version vHelp;
        private static int cLanguage;
        private static Version cHelp;

        private static string defaultLanguage = string.Empty;
        private static string localVersion = string.Empty;

        #endregion

        /// <summary>
        /// Punto de entrada principal para la aplicación
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BonusSkins).Assembly);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    new Program().Run(args);
                }
                catch (NoStartupFormException)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                if (_bc != null)
                {
                    TopMessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Program", "Main"));
                }
                else
                {
                    TopMessageBox.Show(ex.Message );
                }
            }
        }

        /// <summary>
        /// Primer evento en ejecutarse
        /// Carga la pantalle de inicio que muestra los datos de carga
        /// </summary>
        protected override void OnCreateSplashScreen()
        {
            this.SplashScreen = new SplashScreen();
        }

        /// <summary>
        /// Evento que se ejecuta al crear el formulario principal
        /// </summary>
        protected override void OnCreateMainForm()
        {
            try
            {
                ((SplashScreen)this.SplashScreen).StateChanged("Cargando archivo de configuración");
                if (!ConfigurationManagerHelper.Exists())
                    throw new Exception("El archivo de configuración de la aplicación no existe !!!");
                else
                {
                    ((SplashScreen)this.SplashScreen).StateChanged("Cargando llaves del archivo de configuración");
                    ProjectName = ConfigurationManagerHelper.GetKey("Project.Name");
                    ProjectPath = ConfigurationManagerHelper.GetKey("Project.Path");
                    MenuPath = ProjectPath + ConfigurationManagerHelper.GetKey("Menus.Files.Path");
                    FilesPath = ProjectPath + ConfigurationManagerHelper.GetKey("Files.Path");
                    BinExt = ConfigurationManagerHelper.GetKey("Files.Binary.Extension");
                    ConfigFormat = ConfigurationManagerHelper.GetKey("Files.Configuration.Version.Format");
                    ConfigPath = string.Format(FilesPath, ConfigurationManagerHelper.GetKey("Files.Configuration.Path"));
                    LangFormat = ConfigurationManagerHelper.GetKey("Files.Configuration.Languages.Format");
                    LangPath = string.Format(FilesPath, ConfigurationManagerHelper.GetKey("Files.Languages.Path"));
                    HelpPath = ProjectPath + ConfigurationManagerHelper.GetKey("Files.HelpDesk.Path");
                    string masterMainPgSize = ConfigurationManagerHelper.GetKey("Pagging.Master.PageSize.Modal");
                }

                ((SplashScreen)this.SplashScreen).StateChanged(ConfigurationManagerHelper.GetKey("Loading.BaseController"));
                //Inicializa el controlador base
                _bc = BaseController.GetInstance();
                Program.Splash = ((SplashScreen)this.SplashScreen);
                Program.Splash.StateChanged(ConfigurationManagerHelper.GetKey("Loading.AdministrationModel.Config"));
                //Carga configuracion del cliente
                _bc.AdministrationModel.Config = DictionaryFormatter.Read(ConfigPath + BinaryFiles.Program + BinExt);

                //Traer configuración básica
                string lan = null;
                _bc.AdministrationModel.Config.TryGetValue("lan", out lan);
                if (!string.IsNullOrWhiteSpace(lan))
                {
                    DTO_seLAN configLAN = _bc.AdministrationModel.seLAN_GetLanByID(lan);
                    if (configLAN != null)
                    {
                        bool correcta = TestConnection(configLAN.CadenaConn.Value, configLAN.CadenaConnLogger.Value);
                        if (correcta)
                            _bc.BasicConfig = configLAN;
                    }
                }

                while (_bc.BasicConfig == null)
                {
                    LanSelectForm lsf = new LanSelectForm(lan);
                    lsf.Focus();
                    lsf.BringToFront();
                    lsf.TopMost = true;
                    lsf.ShowDialog();                
                    DTO_seLAN config = lsf.SelectedConfig;

                    bool asDefault = lsf.SelectedAsDefault;

                    bool correcta = TestConnection(config.CadenaConn.Value, config.CadenaConnLogger.Value);
                    if (correcta)
                    {
                        _bc.BasicConfig = config;
                        if (asDefault)
                        {
                            if (_bc.AdministrationModel.Config.ContainsKey("lan"))
                                _bc.AdministrationModel.Config["lan"] = config.ID.Value;
                            else
                                _bc.AdministrationModel.Config.Add("lan", config.ID.Value);

                            DictionaryFormatter.Write(_bc.AdministrationModel.Config, ConfigPath + BinaryFiles.Program + BinExt);
                        }
                    }
                }

                LoadMemoryData();
                this.MainForm = new MDI(MenuPath, HelpPath, ProjectName);
            }
            catch (Exception ex)
            {
                if (_bc != null)
                    TopMessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Program.cs", "Main"));
                else
                    TopMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Carga las variables de memoria
        /// </summary>
        private static void LoadMemoryData()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-Co");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-Co");

                _bc.AdministrationModel.Start(true);
                localVersion = string.Empty;

                Program.Splash.StateChanged(ConfigurationManagerHelper.GetKey("Loading.AdministrationModel.Config"));
                #region Revisa si la MAC del PC puede ingresar al sistema
                List<string> userMacs = ComputerInfo.GetLocalMACAddress();
                bool validMAC = _bc.AdministrationModel.seMaquina_ValidatePC(userMacs);
                if (!validMAC)
                {
                    throw new Exception("ERR_CFG_0011");
                }
                #endregion

                Program.Splash.StateChanged(ConfigurationManagerHelper.GetKey("Loading.LoadMemoryData.DefaultLanguage"));
                #region Carga info del idioma por defecto y archivo de ayuda
                _bc.AdministrationModel.Config.TryGetValue(BinaryFiles.Languages.ToString(), out defaultLanguage);
                if (String.IsNullOrEmpty(defaultLanguage))
                {
                    throw new Exception("ERR_CFG_0004");
                }
                LangPath = string.Format(LangPath, defaultLanguage);

                //Carga version del archivo de ayuda
                try
                {
                    HelpPath = string.Format(HelpPath, defaultLanguage);
                    if (!System.IO.File.Exists(HelpPath))
                    {
                        TopMessageBox.Show(_bc.GetResourceForException(new Exception("ERR_FIL_0002&&" + HelpPath), "WinApp", "Program.cs-LoadMemoryData.SetHelpPath"));
                    }
                }
                catch (Exception ex)
                {
                    TopMessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Program.cs", "LoadMemoryData.SetHelpPath"));
                }
                #endregion

                Program.Splash.StateChanged(ConfigurationManagerHelper.GetKey("Loading.LoadMemoryData"));
                #region Carga variables de control, los idiomas y los Grupos de empresa
                try
                {
                    //Carga las variables de version
                    _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(true, string.Empty).ToList();

                    Program.Splash.StateChanged(ConfigurationManagerHelper.GetKey("Loading.CheckUpdates.Languages"));
                    #region Carga la info de los idiomas
                    _bc.AdministrationModel.Languages = UpdateLanguages();
                    #endregion

                    //Grupo de empresas general
                    _bc.AdministrationModel.EmpresaGrupoGeneral = _bc.GetControlValue(AppControl.GrupoEmpresaGeneral);
                }
                catch
                {
                    throw new Exception("ERR_CFG_0010&&Program-LoadMemoryData");
                }
                #endregion
                                           
                Program.Splash.StateChanged(ConfigurationManagerHelper.GetKey("Loading.LoadMemoryData.Masters"));
                #region Trae la lista de maestras con su configuracion
                //Maestras
                var masters = _bc.AdministrationModel.aplMaestraPropiedad_GetAll();
                foreach (DTO_aplMaestraPropiedades p in masters)
                    _bc.AdministrationModel.MasterProperties.Add(p.DocumentoID, p);

                #endregion
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Revisa si un archivo local tiene cambios y lo actualiza de ser necesario
        /// </summary>
        /// <returns>Retorna el diccionario para cargar en memoria</returns>
        private static Dictionary<string, Dictionary<string, string>> UpdateLanguages()
        {
            try
            {
                string lFormat = string.Format(LangFormat, defaultLanguage);
                Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();

                #region Verifica la existencia de nuevos idiomas

                var languages = _bc.AdministrationModel.aplIdioma_GetAll();
                foreach (var l in languages)
                {
                    localVersion = string.Empty;
                    string tempFormat = string.Format(LangFormat, l.IdiomaID);
                    _bc.AdministrationModel.Config.TryGetValue(tempFormat, out localVersion);

                    //Verifica si el idioma existe
                    if (string.IsNullOrEmpty(localVersion))
                        _bc.AdministrationModel.Config.Add(tempFormat, "0");

                    //Obtiene la versión real del idioma por defecto
                    if (defaultLanguage == l.IdiomaID.Value)
                        vLanguage = l.Version.Value.Value;
                }

                #endregion
                // Obtiene la version del idioma local
                localVersion = string.Empty;
                _bc.AdministrationModel.Config.TryGetValue(lFormat, out localVersion);

                if (String.IsNullOrEmpty(localVersion))
                    throw new Exception("ERR_CFG_0008");
                else
                    cLanguage = Convert.ToInt32(localVersion);

                if (vLanguage > cLanguage)
                {
                    #region Actualiza los recursos del idioma x defecto

                    //Carga de la bd las traducciones y las asigna al diccionario
                    var rsx = _bc.AdministrationModel.aplIdiomaTraduccion_GetResources(defaultLanguage);

                    //Barra Herramientas
                    Dictionary<string, string> lToolb = new Dictionary<string, string>();
                    var tbRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.ToolBar select r);
                    foreach (var r in tbRsx)
                    {
                        lToolb.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lToolb, LangPath + LanguageTypes.ToolBar + BinExt);
                    dictionary.Add(LanguageTypes.ToolBar.ToString(), lToolb);

                    //Errores
                    Dictionary<string, string> lErrors = new Dictionary<string, string>();
                    var errorRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Error select r);
                    foreach (var r in errorRsx)
                    {
                        lErrors.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lErrors, LangPath + LanguageTypes.Error + BinExt);
                    dictionary.Add(LanguageTypes.Error.ToString(), lErrors);

                    //Formularios
                    Dictionary<string, string> lForms = new Dictionary<string, string>();
                    var formsRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Forms select r);
                    foreach (var r in formsRsx)
                    {
                        lForms.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lForms, LangPath + LanguageTypes.Forms + BinExt);
                    dictionary.Add(LanguageTypes.Forms.ToString(), lForms);

                    //Mail
                    Dictionary<string, string> lMail = new Dictionary<string, string>();
                    var mailRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Mail select r);
                    foreach (var r in mailRsx)
                    {
                        lMail.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lMail, LangPath + LanguageTypes.Mail + BinExt);
                    dictionary.Add(LanguageTypes.Mail.ToString(), lMail);


                    //Mensajes
                    Dictionary<string, string> lMsg = new Dictionary<string, string>();
                    var msgRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Messages select r);
                    foreach (var r in msgRsx)
                    {
                        lMsg.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lMsg, LangPath + LanguageTypes.Messages + BinExt);
                    dictionary.Add(LanguageTypes.Messages.ToString(), lMsg);

                    //Menus
                    Dictionary<string, string> lMnu = new Dictionary<string, string>();
                    var mnuRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Menu select r);
                    foreach (var r in mnuRsx)
                    {
                        lMnu.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lMnu, LangPath + LanguageTypes.Menu + BinExt);
                    dictionary.Add(LanguageTypes.Menu.ToString(), lMnu);

                    //Modulos
                    Dictionary<string, string> lMods = new Dictionary<string, string>();
                    var modRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Modules select r);
                    foreach (var r in modRsx)
                    {
                        lMods.Add(r.Llave.Value, r.Dato.Value);
                    }
                    DictionaryFormatter.Write(lMods, LangPath + LanguageTypes.Modules + BinExt);
                    dictionary.Add(LanguageTypes.Modules.ToString(), lMods);

                    //Tablas
                    Dictionary<string, string> lTab = new Dictionary<string, string>();
                    var tabRsx = (from r in rsx where r.TipoID.Value == (int)LanguageTypes.Tables select r);
                    foreach (var r in tabRsx)
                        lTab.Add(r.Llave.Value, r.Dato.Value);

                    DictionaryFormatter.Write(lTab, LangPath + LanguageTypes.Tables + BinExt);
                    dictionary.Add(LanguageTypes.Tables.ToString(), lTab);

                    _bc.AdministrationModel.Config[lFormat] = vLanguage.ToString();
                    #endregion
                }
                else
                    dictionary = ReadLanguages();


                return dictionary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de traducciones del diccionario
        /// </summary>
        /// <returns>Retorna el diccionario para cargar en memoria</returns>
        private static Dictionary<string, Dictionary<string, string>> ReadLanguages()
        {
            Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                LangFormat = string.Format(LangFormat, defaultLanguage);

                dictionary.Add(LanguageTypes.Error.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Error.ToString() + BinExt));
                dictionary.Add(LanguageTypes.Forms.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Forms.ToString() + BinExt));
                dictionary.Add(LanguageTypes.Mail.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Mail.ToString() + BinExt));
                dictionary.Add(LanguageTypes.Menu.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Menu.ToString() + BinExt));
                dictionary.Add(LanguageTypes.Messages.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Messages.ToString() + BinExt));
                dictionary.Add(LanguageTypes.Modules.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Modules.ToString() + BinExt));
                dictionary.Add(LanguageTypes.Tables.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.Tables.ToString() + BinExt));
                dictionary.Add(LanguageTypes.ToolBar.ToString(), DictionaryFormatter.Read(LangPath + LanguageTypes.ToolBar.ToString() + BinExt));

                return dictionary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Prueba una conexion
        /// </summary>
        /// <param name="cadena1"></param>
        /// <param name="cadena2"></param>
        /// <returns></returns>
        public static bool TestConnection(string cadena1, string cadena2)
        {
            bool correcta = false;
            try
            {
                SqlConnection temp = new SqlConnection(cadena1);
                temp.Open();
                temp.Close();
                temp = new SqlConnection(cadena2);
                temp.Open();
                temp.Close();
                correcta = true;
            }
            catch (Exception ex)
            {
                ;
            }
            return correcta;
        }
    }
}
