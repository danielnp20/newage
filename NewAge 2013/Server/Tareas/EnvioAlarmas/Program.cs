using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.ADO;
using NewAge.DTO;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.IO;
using System.Net.Mail;
using NewAge.Negocio;
using NewAge.DTO.UDT;

namespace EnvioAlarmas
{
    class Program
    {
        #region DBConnection

        /// <summary>
        /// Cadena de conexion a la bd del logger
        /// </summary>
        private static string loggerCon;

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        private static SqlConnection _mySqlConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        private static void ADO_ConnectDB()
        {
            try
            {
                if (_mySqlConnection.State == ConnectionState.Broken || _mySqlConnection.State == ConnectionState.Closed)
                {
                    _mySqlConnection.Open();
                }
            }
            catch
            {
                ADO_CloseDBConnection();
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        public static void ADO_CloseDBConnection()
        {
            try
            {
                if (_mySqlConnection.State != ConnectionState.Closed)
                {
                    _mySqlConnection.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Diccionario con la lista de recursos que se vaya a usar (Idioma - <Llave - Dato>)
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> _dictionaryRsx;

        /// <summary>
        /// Modelos de acceso a datos
        /// </summary>
        private static DAL_aplIdiomaTraduccion _aplIdiomaTraduccion;
        private static DAL_Alarmas _glTareas;
        private static DAL_seUsuario _seUsuario;
        private static DAL_MasterSimple _simpleDAL;

        static void Main(string[] args)
        {
            InitData();
            try
            {
                ModuloFachada facade = new ModuloFachada();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, _mySqlConnection, null, null, 0, loggerCon);

                List<DTO_Alarma> tareas = mod.Alarmas_GetAll().ToList();
                DateTime hoy = DateTime.Now;
                DTO_glEmpresa emp;
                UDT_BasicID udt = new UDT_BasicID();

                foreach (DTO_Alarma tarea in tareas)
                {
                    string usr1 = string.Empty;
                    string usr2 = string.Empty;
                    string usr3 = string.Empty;
                    DateTime fechaAlarma1 = !string.IsNullOrWhiteSpace(tarea.FechaAlarma1) ? Convert.ToDateTime(tarea.FechaAlarma1) : hoy;
                    DateTime fechaAlarma2 = !string.IsNullOrWhiteSpace(tarea.FechaAlarma2) ? Convert.ToDateTime(tarea.FechaAlarma2) : hoy;
                    DateTime fechaAlarma3 = !string.IsNullOrWhiteSpace(tarea.FechaAlarma3) ? Convert.ToDateTime(tarea.FechaAlarma3) : hoy;

                    if (!string.IsNullOrWhiteSpace(tarea.FechaAlarma1) && hoy > fechaAlarma1)
                        usr1 = tarea.UsuarioID1;
                    if (!string.IsNullOrWhiteSpace(tarea.FechaAlarma2) && hoy > fechaAlarma2)
                        usr2 = tarea.UsuarioID2;
                    if (!string.IsNullOrWhiteSpace(tarea.FechaAlarma3) && hoy > fechaAlarma3)
                        usr3 = tarea.UsuarioID3;

                    List<string> rsxKeys = new List<string>();
                    rsxKeys.Add(DictionaryMessages.Mail_AlarmDoc_Subject);
                    rsxKeys.Add(DictionaryMessages.Mail_AlarmDoc_Body);

                    try
                    {
                        udt.Value = tarea.EmpresaID;
                        emp = (DTO_glEmpresa)_simpleDAL.DAL_MasterSimple_GetByID(udt, true);
                        mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, _mySqlConnection, null, emp, Convert.ToInt32(tarea.UsuarioRESPID), loggerCon);
                        mod.Alarmas_SendAlarm(tarea.EmpresaID, Convert.ToInt32(tarea.NumeroDoc), Convert.ToInt32(tarea.DocumentoID),
                            tarea.ActividadFlujoID, Convert.ToInt32(tarea.UsuarioRESPID), usr1, usr2, usr3, false);

                        if (!string.IsNullOrWhiteSpace(tarea.FechaAlarma1) && hoy > fechaAlarma1)
                            SendDocumentMail(tarea, 1, rsxKeys);
                        if (!string.IsNullOrWhiteSpace(tarea.FechaAlarma2) && hoy > fechaAlarma2)
                            SendDocumentMail(tarea, 2, rsxKeys);
                        if (!string.IsNullOrWhiteSpace(tarea.FechaAlarma3) && hoy > fechaAlarma3)
                            SendDocumentMail(tarea, 3, rsxKeys);
                    }
                    catch (Exception ex) 
                    { ; }
                }
            }
            catch (Exception ex1) 
            { ; }
        }

        /// <summary>
        /// Inicializa las variables 
        /// </summary>
        private static void InitData()
        {
            //Conexion a BD
            loggerCon = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString();
            _mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString());
            ADO_ConnectDB();

            //Diccionario de recursos
            _dictionaryRsx = new Dictionary<string, Dictionary<string, string>>();

            //Modelos para BD
            _aplIdiomaTraduccion = new DAL_aplIdiomaTraduccion(_mySqlConnection, null, null, 0, loggerCon);
            _glTareas = new DAL_Alarmas(_mySqlConnection, null, null, 0, loggerCon);
            _seUsuario = new DAL_seUsuario(_mySqlConnection, null, null, 0, loggerCon);
            _simpleDAL = new DAL_MasterSimple(_mySqlConnection, null, null, 0, loggerCon);
            _simpleDAL.DocumentID = AppMasters.glEmpresa;
        }

        /// <summary>
        /// Envia un correo segun el documento
        /// </summary>
        /// <param name="tarea">Tarea con los parametros del correo</param>
        /// <param name="alarmaIndex">Numero de alarma que se esta enviando</param>
        private static void SendDocumentMail(DTO_Alarma tarea, int alarmaIndex, List<string> rsxKeys)
        {
            try
            {
                #region Configuracion de parametros del correo


                string smtp = ConfigurationManager.AppSettings["Mail.Smtp"];
                string smtpUser = ConfigurationManager.AppSettings["Mail.Sender"];
                string smtpMailFrom = ConfigurationManager.AppSettings["Mail.Sender"];
                string smtpPassword = ConfigurationManager.AppSettings["Mail.PasswordSender"];
                int port = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Mail.Port"]) ? Convert.ToInt32(ConfigurationManager.AppSettings["Mail.Port"]) : 25;
                bool ssl = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Mail.SSL"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["Mail.SSL"]) : false;
                
                List<string> mailParams = new List<string>();
                string[] extraParams = mailParams.ToArray();

                #endregion
                #region Carga de recursos
                switch (alarmaIndex)
                {
                    case 1:
                        if (!_dictionaryRsx.ContainsKey(tarea.UsuarioLang1))
                        {
                            Dictionary<string, string> langRsx = new Dictionary<string, string>();
                            IEnumerable<DTO_aplIdiomaTraduccion> rsx = _aplIdiomaTraduccion.DAL_aplIdiomaTraduccion_GetRsxByKeys(tarea.UsuarioLang1, LanguageTypes.Mail, rsxKeys);
                            foreach (DTO_aplIdiomaTraduccion r in rsx)
                            {
                                try
                                {
                                    langRsx.Add(r.Llave.Value, r.Dato.Value);
                                }
                                catch (Exception) { ; }
                            }

                            _dictionaryRsx.Add(tarea.UsuarioLang1, langRsx);
                        }
                        break;
                    
                    case 2:
                        if (!_dictionaryRsx.ContainsKey(tarea.UsuarioLang2))
                        {
                            Dictionary<string, string> langRsx = new Dictionary<string, string>();
                            IEnumerable<DTO_aplIdiomaTraduccion> rsx = _aplIdiomaTraduccion.DAL_aplIdiomaTraduccion_GetRsxByKeys(tarea.UsuarioLang2, LanguageTypes.Mail, rsxKeys);
                            foreach (DTO_aplIdiomaTraduccion r in rsx)
                            {
                                try
                                {
                                    langRsx.Add(r.Llave.Value, r.Dato.Value);
                                }
                                catch (Exception) { ; }
                            }

                            _dictionaryRsx.Add(tarea.UsuarioLang2, langRsx);
                        }
                        break;

                    case 3:
                        if (!_dictionaryRsx.ContainsKey(tarea.UsuarioLang3))
                        {
                            Dictionary<string, string> langRsx = new Dictionary<string, string>();
                            IEnumerable<DTO_aplIdiomaTraduccion> rsx = _aplIdiomaTraduccion.DAL_aplIdiomaTraduccion_GetRsxByKeys(tarea.UsuarioLang3, LanguageTypes.Mail, rsxKeys);
                            foreach (DTO_aplIdiomaTraduccion r in rsx)
                            {
                                try
                                {
                                    langRsx.Add(r.Llave.Value, r.Dato.Value);
                                }
                                catch (Exception) { ; }
                            }

                            _dictionaryRsx.Add(tarea.UsuarioLang3, langRsx);
                        }
                        break;
                }

                #endregion
                #region Informacion del correo
                string recipeMail = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;
                string bodyFormat = string.Empty;

                switch (alarmaIndex)
                {
                    case 1:
                        recipeMail = tarea.UsuarioMail1;
                        subject = GetResource(tarea.UsuarioLang1, DictionaryMessages.Mail_AlarmDoc_Subject);
                        bodyFormat = GetResource(tarea.UsuarioLang1, DictionaryMessages.Mail_AlarmDoc_Body);
                        break;
                    case 2:
                        recipeMail = tarea.UsuarioMail2;
                        subject = GetResource(tarea.UsuarioLang1, DictionaryMessages.Mail_AlarmDoc_Subject);
                        bodyFormat = GetResource(tarea.UsuarioLang1, DictionaryMessages.Mail_AlarmDoc_Body);
                        break;
                    case 3:
                        recipeMail = tarea.UsuarioMail3;
                        subject = GetResource(tarea.UsuarioLang1, DictionaryMessages.Mail_AlarmDoc_Subject);
                        bodyFormat = GetResource(tarea.UsuarioLang1, DictionaryMessages.Mail_AlarmDoc_Body);
                        break;
                }

                string userToken = tarea.UsuarioRESPID + tarea.DocumentoID + tarea.DocumentoID + tarea.FileName; 
                body = string.Format(bodyFormat, tarea.Actividad, tarea.DocumentoID, tarea.DocumentoDesc, tarea.TerceroID, tarea.TerceroDesc,
                    tarea.PrefijoID, tarea.Consecutivo, tarea.UsuarioRESP, tarea.FileName);

                MailUtility.SendMail(smtp, port, ssl, smtpUser, smtpPassword, smtpMailFrom, recipeMail, subject, body, userToken);

                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

        }

        #region Funciones para manejo de recursos

        /// <summary>
        /// Trae una traducción de un diccionario segun el modulo
        /// </summary>
        /// <param name="lang">Idioma para buscar la llave</param>
        /// <param name="key">Llave para buscar la traducción</param>
        /// <returns>Retorna la traduccion de una llaves</returns>
        public static string GetResource(string language, string key)
        {
            try
            {
                Dictionary<string, string> tempDictionary = _dictionaryRsx[language];
                return GetResource(tempDictionary, key);
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        /// <summary>
        /// Trae una traducción de un diccionario
        /// </summary>
        /// <param name="key">Llave para buscar la traducción</param>
        /// <returns>Retorna la traduccion de una llaves</returns>
        private static string GetResource(Dictionary<string, string> dic, string key)
        {
            try
            {
                string ret = string.Empty;

                dic.TryGetValue(key, out ret);
                return string.IsNullOrEmpty(ret) ? key : ret;
            }
            catch (Exception)
            {
                return key;
            }
        }

        #endregion
    }
}
