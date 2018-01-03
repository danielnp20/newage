using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Collections;

namespace NewAge.Cliente.InitialConfig.LoadData
{
    class Program : System.Configuration.Install.Installer
    {
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            this.CargaInicial();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        public void CargaInicial()
        {
            if (!ConfigurationManagerHelper.Exists())
                throw new Exception("El archivo de configuración de la aplicación no existe !!!");

            //Create the project folder
            var projectPath = ConfigurationManagerHelper.GetKey("Project.Path");
            if (!Directory.Exists(projectPath))
                Directory.CreateDirectory(projectPath);

            #region Carga de archivos binarios

            //Paths
            //ProjectPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + ConfigurationManagerHelper.GetKey("Project.Path");
            string binaryPath = ConfigurationManagerHelper.GetKey("Files.Binary.Path");
            string configPath = projectPath + binaryPath + ConfigurationManagerHelper.GetKey("Files.Configuration.Path");
            string currentLang = ConfigurationManagerHelper.GetKey("Files.Languages.Current");
            string langPath = projectPath + binaryPath + ConfigurationManagerHelper.GetKey("Files.Languages.Path") + ConfigurationManagerHelper.GetKey("Files.Languages.Path.Current");
            string binaryExtension = ConfigurationManagerHelper.GetKey("Files.Binary.Extension");
            string configFormat = ConfigurationManagerHelper.GetKey("Files.Configuration.Version.Format");
            string langFormat = ConfigurationManagerHelper.GetKey("Files.Configuration.Languages.Format");

            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);

            if (!Directory.Exists(langPath))
                Directory.CreateDirectory(langPath);

            //Idiomas
            //Error
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Error.ToString() + binaryExtension);
            //Formas
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Forms.ToString() + binaryExtension);
            //Mail
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Mail.ToString() + binaryExtension);
            //Menu
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Menu.ToString() + binaryExtension);
            //Mensajes
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Messages.ToString() + binaryExtension);
            //Modulos
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Modules.ToString() + binaryExtension);
            //Tablas definidas (sexo, paises, etc)
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.Tables.ToString() + binaryExtension);
            //Barra de herramientas
            DictionaryFormatter.Write(new Dictionary<string, string>(), langPath + LanguageTypes.ToolBar.ToString() + binaryExtension);

            //Configuración
            string currentVersion = ConfigurationManagerHelper.GetKey("Files.Configuration.Version.Current");
            string languageVersion = ConfigurationManagerHelper.GetKey("Files.Configuration.Languages.Current");
            Dictionary<string, string> cDict = new Dictionary<string, string>();
            //Crea las llaves iniciales de modulos e idiomas
            cDict.Add(BinaryFiles.Languages.ToString(), currentLang);
            //Inicia la primera version de los archivos
            cDict.Add(String.Format(langFormat, currentLang), languageVersion);
            DictionaryFormatter.Write(cDict, configPath + BinaryFiles.Program.ToString() + binaryExtension);

            //Errores en espanol
            Dictionary<string, string> cDictError = new Dictionary<string, string>();

            cDictError.Add("ERR_CFG_0000", "El archivo de configuración de la aplicación no existe");
            cDictError.Add("ERR_CFG_0001", "La llave {0} no existe en el archivo de configuración");
            cDictError.Add("ERR_CFG_0002", "(version de programa) no encontrada en la tabla de control");
            cDictError.Add("ERR_CFG_0003", "Archivo de configuración sin llave de version del programa");
            cDictError.Add("ERR_CFG_0004", "Archivo de configuración sin llave de idioma por defecto");
            cDictError.Add("ERR_CFG_0005", "(modulos activos) no encontrada en la tabla de control");
            cDictError.Add("ERR_CFG_0006", "Archivo de configuración sin llave de version del idioma");
            cDictError.Add("ERR_CFG_0007", "No se devolvio versión del idioma actual");
            cDictError.Add("ERR_CFG_0008", "No pudo ser encontrada la llave del idioma seleccionado en el archivo de configuración");
            cDictError.Add("ERR_CFG_0009", "No pudo ser encontrada la llave de version de modulos en el archivo de configuración");
            cDictError.Add("ERR_CFG_0010", "No fue posible cargar la versión de {0}");
            cDictError.Add("ERR_SYS_0024", "Error del sistema, por favor reinicie los servicios");
            cDictError.Add("ERR_SQL_0005", "No hay conexión con la base de datos revise, el archivo de configuración del servicio y/o si existe el servidor");
            cDictError.Add("ERR_DIR_0001", "El directorio {0} no existe ");
            cDictError.Add("ERR_FIL_0002", "El archivo {0} no existe ");
            cDictError.Add("ERR_UAE_0003", "No esta autorizado para acceder a la carpeta {0}, Inicie la aplicación como administrador");
            cDictError.Add("ERR_ETY_0004", "Error EntityException");
            cDictError.Add("ERR_ARG_0006", "Error ArgumentException");
            cDictError.Add("ERR_AGN_0007", "Error ArgumentNullException");
            cDictError.Add("ERR_AOR_0008", "Error ArgumentOutOfRangeException");
            cDictError.Add("ERR_ART_0009", "Error ArithmeticException");
            cDictError.Add("ERR_ATM_0010", "Error ArrayTypeMismatchException");
            cDictError.Add("ERR_BIF_0011", "Error BadImageFormatException");
            cDictError.Add("ERR_COE_0012", "Error CoreException");
            cDictError.Add("ERR_DBZ_0013", "Error DivideByZeroException");
            cDictError.Add("ERR_FOR_0014", "Error FormatException");
            cDictError.Add("ERR_IOR_0015", "Error IndexOutOfRangeException");
            cDictError.Add("ERR_ICE_0016", "Error InvalidCastExpression");
            cDictError.Add("ERR_IOP_0017", "Error InvalidOperationException");
            cDictError.Add("ERR_MSM_0018", "Error MissingMemberException");
            cDictError.Add("ERR_NFN_0019", "Error NotFiniteNumberException");
            cDictError.Add("ERR_NSP_0020", "Error NotSupportedException");
            cDictError.Add("ERR_NRF_0021", "Error NullReferenceException");
            cDictError.Add("ERR_OOM_0022", "Error OutOfMemoryException");
            cDictError.Add("ERR_SOF_0023", "Error StackOverflowException");
            cDictError.Add("ERR_UKW_0025", "Error UnKnownException");
            cDictError.Add("ERR_DEL_0001", "El registro {0} no puede ser eliminado");
            ///SQL ERRORS
            cDictError.Add("ERR_SQL_1005", "Error de llave primaria {0}, ya existe");
            cDictError.Add("ERR_SQL_2005", "Error de llave foranea {0}, está siendo utilizada por otra tabla");

            DictionaryFormatter.Write(cDictError, langPath + LanguageTypes.Error.ToString() + binaryExtension);

            #endregion
        }

        static void Main(string[] args)
        {
            new Program().CargaInicial();
        }
    }
}
