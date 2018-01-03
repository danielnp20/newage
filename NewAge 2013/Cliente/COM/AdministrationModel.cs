using NewAge.ADO;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace NewAgeCOM
{
    /// <summary>
    /// Modela las operaciones y validaciones de la lógica de negocio para la información del sitio
    /// </summary>
    [ComVisible(true), GuidAttribute("9ca0d6a2-45ab-456b-829b-74d41b90c98a")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("NewAgeCOM.AdministrationModel")]
    public class AdministrationModel : IAdministrationModel
    {
        #region Variables y Propiedades

        //Manejo de resultados
        private enum MethodResult
        {
            OK,
            NOK
        }

        //Conexiones
        private string connString;
        private string connLoggerString;
        private SqlConnection mySqlConnection = new SqlConnection();

        //Negocio
        private ModuloFachada facade = new ModuloFachada();
        private ModuloAplicacion modApp;
        private ModuloGlobal modGlobal;
        private ModuloPlaneacion modPlaneacion;
        private ModuloProveedores modProveedores;

        //Maestras
        private DAL_MasterSimple _masterSimple;
        private DAL_MasterComplex _masterComplex;
        private DAL_MasterHierarchy _masterHierarchy;
        
        //Persistencia
        private string empresaGrupoGeneral;
        private Dictionary<string, string> rsxError;
        private Dictionary<string, string> rsxForms;
        private Dictionary<string, string> rsxMessages;

        //Utilidad
        private Dictionary<int, DTO_aplMaestraPropiedades> masterProperties;
        private Dictionary<Tuple<int, int>, int> batchProgress;


        //Propiedades
        private DTO_seUsuario user { get; set; }
        private DTO_glEmpresa empresa { get; set; }

        #endregion

        #region Funciones privadas

        #region DBConnection

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        /// <returns>Retorna el indice con la conexion que se esta usando</returns>
        private void ADO_ConnectDB()
        {
            try
            {
                if (this.mySqlConnection == null || this.mySqlConnection.State == ConnectionState.Broken || this.mySqlConnection.State == ConnectionState.Closed)
                {
                    this.mySqlConnection = new SqlConnection(this.connString);
                    this.mySqlConnection.Open();
                }
            }
            catch
            {
                this.ADO_CloseDBConnection();
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        private void ADO_CloseDBConnection()
        {
            try
            {
                this.mySqlConnection.Close();

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Recursos

        /// <summary>
        /// Trae el recurso del mensaje correspondiente, al llegar un TxResult del servidor
        /// </summary>
        /// <param name="rsxMessage">Nombre de la llave del recurso</param>
        /// <returns>Recurso del mensaje</returns>
        private string GetResourceForm(string rsxKey)
        {
            string ret = this.rsxForms.ContainsKey(rsxKey) ? this.rsxForms[rsxKey] : rsxKey;
            return ret;
        }

        /// <summary>
        /// Trae el recurso del mensaje correspondiente, al llegar un TxResult del servidor
        /// </summary>
        /// <param name="rsxKey">Nombre de la llave del recurso</param>
        /// <returns>Recurso del mensaje</returns>
        private string GetResourceMessage(string rsxKey)
        {
            string[] arr = rsxKey.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            string[] args = new string[arr.Count() - 1];


            string key = this.rsxMessages.ContainsKey(arr[0]) ? this.rsxMessages[arr[0]] : arr[0];
            if (arr.Count() > 1)
            {
                for (int i = 1; i < arr.Count(); ++i)
                {
                    args[i - 1] = arr[i];
                }
            }

            string value = string.Format(key, args);
            return value;
        }

        /// <summary>
        /// Esta funcionretorna el mesanje estraido de los resources, permite pasar un m,ensaje en formato
        /// mentor Exception 
        /// </summary>
        /// <param name="msg">Mensaje del cual se quiere obtener un valor visible al user</param>
        /// <returns>Mensaje para el user a partir de un msn tipo exception </returns>
        private string GetResourceError(string rsxKey)
        {
            if (rsxKey.StartsWith("ERR_"))
            {
                string ext = "";
                int place = rsxKey.Length;

                if (rsxKey.Contains("&&"))
                {
                    place = rsxKey.IndexOf("&&");
                    ext = rsxKey.Substring(place + 2);
                }

                var msg = this.rsxError.ContainsKey(rsxKey.Substring(0, place)) ? this.rsxError[rsxKey.Substring(0, place)] : rsxKey.Substring(0, place);
                if (msg.Equals(rsxKey.Substring(0, place)))
                    return msg;
                else
                {
                    var parametros = ext.Split(new string[] { "&&" }, StringSplitOptions.None);
                    return String.Format(msg, parametros);
                }
            }
            else
            {
                return this.rsxError.ContainsKey(rsxKey) ? this.rsxError[rsxKey] : rsxKey;
            }
        }

        /// <summary>
        /// Asigna los recursos a un objecto de tipo resultado
        /// </summary>
        /// <param name="result">Resultsdo</param>
        internal void AssignResultResources(int? documentID, DTO_TxResult result)
        {
            try
            {
                if (!string.IsNullOrEmpty(result.ResultMessage))
                    result.ResultMessage = this.GetResourceError(result.ResultMessage);

                if (result.Details == null)
                    result.Details = new List<DTO_TxResultDetail>();

                #region Detalles
                foreach (DTO_TxResultDetail detalle in result.Details)
                {
                    if (!string.IsNullOrWhiteSpace(detalle.Message) && detalle.Message != "OK")
                    {
                        detalle.Message = this.GetResourceError(detalle.Message);
                        if (detalle.Message == detalle.Message)
                            detalle.Message = this.GetResourceMessage(detalle.Message);
                    }

                    #region Campos
                    if (detalle.DetailsFields != null)
                    {
                        foreach (DTO_TxResultDetailFields campo in detalle.DetailsFields)
                        {
                            if (campo.Message.ToUpper().StartsWith("ERR"))
                                campo.Message = this.GetResourceError(campo.Message);
                            else
                                campo.Message = this.GetResourceMessage(campo.Message);

                            if (documentID.HasValue)
                                campo.Field = this.GetResourceForm(documentID.Value + "_" + campo.Field);
                        }
                    }
                    #endregion

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna los recursos a un objecto de tipo resultado
        /// </summary>
        /// <param name="result">Resultsdo</param>
        internal void AssignResultResources(int? documentID, List<DTO_TxResult> results)
        {
            foreach (DTO_TxResult result in results)
            {
                this.AssignResultResources(documentID, result);
            }
        }

        #endregion

        #region Maestras

        /// <summary>
        /// Trae el grupo de empresas de acuerdo al un documento
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <returns>Retorna el grupo de empresas</returns>
        internal string GetMaestraEmpresaGrupoByDocumentID(int documentID, GrupoEmpresa seguridad)
        {
            string empGrupo = string.Empty;
            if (seguridad == GrupoEmpresa.Automatico)
            {
                empGrupo = this.empresa.ID.Value;
            }
            else if (seguridad == GrupoEmpresa.Individual)
            {
                empGrupo = this.empresa.EmpresaGrupoID_.Value;
            }
            else
            {
                empGrupo = this.empresaGrupoGeneral;
            }

            return empGrupo;
        }

        #endregion

        #region Utilidades

        /// <summary>
        /// Concierte un objecto en XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string ObjectToXML(object obj)
        {
            Type objType = obj.GetType();
            var sw = new StringWriter();
            var serializer = new XmlSerializer(objType);
            serializer.Serialize(sw, obj);
            return sw.ToString();
        }

        /// <summary>
        /// Carga un DTO a partir de sus propiedades y valores
        /// </summary>
        /// <param name="obj">Objeto en el estado inicial</param>
        /// <param name="props">Lista de propiedades</param>
        /// <param name="values">Lista de valores asignados</param>
        private DTO_TxResult LoadDTO(object obj, string[] props, string[] values)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                string msgInvalidFormat = this.GetResourceMessage(DictionaryMessages.InvalidFormat);
                Type t = obj.GetType();

                #region Validación de formatos

                for (int i = 0; i < props.Count(); ++i)
                {
                    string prop = props[i];
                    string val = values[i];

                    PropertyInfo pi = t.GetProperty(prop);
                    if (pi != null)
                    {
                        if (pi.PropertyType.Equals(typeof(UDTSQL_datetime)) || pi.PropertyType.Equals(typeof(UDTSQL_smalldatetime)) || 
                            pi.PropertyType.Equals(typeof(UDT_PeriodoID)) ||  pi.PropertyType.Equals(typeof(UDT_PeriodoCert)))
                        {
                            try
                            {
                                DateTime aux = DateTime.ParseExact(val, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = 1;
                                rd.Key = prop;
                                rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatDate);

                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                        }
                        if (pi.PropertyType.Equals(typeof(UDTSQL_int)) || pi.PropertyType.Equals(typeof(UDT_Consecutivo)))
                        {
                            try
                            {
                                int aux = Convert.ToInt32(val);
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = 1;
                                rd.Key = prop;
                                rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatInvalidNumber);

                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                        }
                        if (pi.PropertyType.Equals(typeof(UDTSQL_smallint)))
                        {
                            try
                            {
                                short aux = Convert.ToInt16(val);
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = 1;
                                rd.Key = prop;
                                rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatLimitNumber);

                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                        }
                        if (pi.PropertyType.Equals(typeof(UDTSQL_bigint)))
                        {
                            try
                            {
                                long aux = Convert.ToInt64(val);
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = 1;
                                rd.Key = prop;
                                rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatLimitNumber);

                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                        }
                        if (pi.PropertyType.Equals(typeof(UDTSQL_tinyint)))
                        {
                            try
                            {
                                byte aux = Convert.ToByte(val);
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = 1;
                                rd.Key = prop;
                                rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatNumberRange);

                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                        }
                        if (pi.PropertyType.Equals(typeof(UDTSQL_decimal)) || pi.PropertyType.Equals(typeof(UDT_Valor)) ||
                            pi.PropertyType.Equals(typeof(UDT_PorcentajeID)) || pi.PropertyType.Equals(typeof(UDT_TasaID)))
                        {
                            try
                            {
                                decimal aux = Convert.ToDecimal(val, CultureInfo.InvariantCulture);
                                if (val.Contains(','))
                                {
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = 1;
                                    rd.Key = prop;
                                    rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatDecimal);

                                    result.Details.Add(rd);
                                    result.Result = ResultValue.NOK;
                                }
                            }
                            catch (Exception ex)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = 1;
                                rd.Key = prop;
                                rd.Message = msgInvalidFormat + this.GetResourceMessage(DictionaryMessages.FormatDate);

                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                        }

                    }
                }

                #endregion

                if (result.Result == ResultValue.OK)
                {
                    #region Carga los valores

                    for (int i = 0; i < props.Count(); ++i)
                    {
                        string prop = props[i];
                        string val = values[i];

                        PropertyInfo pi = t.GetProperty(prop);
                        if (pi != null)
                        {
                            if (pi.PropertyType == typeof(UDT_SiNo))
                                val = val == "1" ? "true" : "false";

                            UDT udt = (UDT)pi.GetValue(obj, null);
                            udt.SetValueFromString(val);
                        }
                    }

                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Caraga un DTO a partir de strings
        /// </summary>
        /// <param name="obj">DTO</param>
        /// <param name="props">Propiedades separadas pro comas</param>
        /// <param name="values">Valores de las propiedades separados por comas</param>
        private DTO_TxResult LoadDTO(object obj, string props, string values)
        {
            try
            {
                string[] propsArr = props.Split(new string[] { "&,&" }, StringSplitOptions.None);
                string[] valuesArr = values.Split(new string[] { "&,&" }, StringSplitOptions.None);

                DTO_TxResult result = this.LoadDTO(obj, propsArr, valuesArr);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public AdministrationModel() { }

        /// <summary>
        /// Revisa si la conexión abre exitosamente
        /// </summary>
        /// <returns></returns>
        public string InitVars(string connStr, string userID, string pass, string empresaID)
        {
            try
            {
                this.connString = connStr;
                this.connLoggerString = connStr.Replace("NewAge", "NewAgeDiagnostics");
                this.user = new DTO_seUsuario();
                this.user.ReplicaID.Value = 0;
                this.rsxError = new Dictionary<string, string>();
                this.rsxForms = new Dictionary<string, string>();
                this.rsxMessages = new Dictionary<string, string>();
                this.masterProperties = new Dictionary<int, DTO_aplMaestraPropiedades>();
                this.batchProgress = new Dictionary<Tuple<int, int>, int>();

                this.mySqlConnection = new SqlConnection(this.connString);
                this.mySqlConnection.Open();


                #region Carga los recursos

                //Carga los recursos
                this.modApp = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this.mySqlConnection, null, null, 0, this.connLoggerString);
                List<DTO_aplIdiomaTraduccion> rsx = (this.modApp.aplIdiomaTraduccion_GetByIdiomaId("ES1")).ToList();

                //Errores
                var errorRsx = (from r in rsx where r.TipoID.Value == 1 select r);
                foreach (var r in errorRsx)
                    this.rsxError.Add(r.Llave.Value, r.Dato.Value);

                //Forms
                var frmRsx = (from r in rsx where r.TipoID.Value == 2 select r);
                foreach (var r in frmRsx)
                    this.rsxForms.Add(r.Llave.Value, r.Dato.Value);

                //Mensajes
                var msgRsx = (from r in rsx where r.TipoID.Value == 4 select r);
                foreach (var r in msgRsx)
                    this.rsxMessages.Add(r.Llave.Value, r.Dato.Value);

                #endregion
                #region Carga el usuario (Esta en comentarios la validacion de la contraseña)

                this.modGlobal = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this.mySqlConnection, null, null, 0, this.connLoggerString);
                //var userVal = this.modGlobal.seUsuario_ValidateUserCredentials(AppMasters.seUsuario, userID, pass);
                //if (userVal == UserResult.NotExists)
                //{
                //    string res = this.GetResourceMessage(DictionaryMessages.LoginFailure);
                //    return res;
                //}
                //else if (userVal == UserResult.BlockUser)
                //{
                //    string res = this.GetResourceMessage(DictionaryMessages.LoginUserBlocked);
                //    return res;
                //}
                //else if (userVal == UserResult.IncorrectPassword)
                //{
                //    string res = this.GetResourceMessage(DictionaryMessages.LoginIncorrectPwd);
                //    return res;
                //}
                
                //Usuario existente
                this.user = this.modGlobal.seUsuario_GetUserbyID(userID);
                if(this.user == null)
                {
                    return MethodResult.NOK.ToString() + ": " + this.GetResourceMessage(DictionaryMessages.LoginFailure);
                }
                
                #endregion
                #region Carga la info de la empresa

                this._masterSimple = new DAL_MasterSimple(this.mySqlConnection, null, null, 0, this.connLoggerString);
                
                UDT_BasicID udt = new UDT_BasicID() { Value = empresaID, IsInt = false };
                DTO_MasterBasic empresaDTO = this.MasterSimple_GetByID(AppMasters.glEmpresa, udt);
                if(empresaDTO == null)
                {
                    string res = MethodResult.NOK.ToString() + ": " + this.GetResourceMessage(DictionaryMessages.InvalidUserCompany);
                    return res;
                }

                List<DTO_glEmpresa> empresas = this.modGlobal.seUsuario_GetUserCompanies(userID).ToList();
                if(!empresas.Any(x => x.ID.Value == empresaDTO.ID.Value))
                {
                    string res = MethodResult.NOK.ToString() + ": " + this.GetResourceMessage(DictionaryMessages.InvalidUserCompany);
                    return res;
                }

                this.empresa = (DTO_glEmpresa)empresaDTO;

                #endregion
                #region Carga las propiedades de las maestras

                var masters = this.modApp.aplMaestraPropiedades_GetAll();
                foreach (DTO_aplMaestraPropiedades p in masters)
                    this.masterProperties.Add(p.DocumentoID, p);

                #endregion
                #region Carga la info de control
                this.empresaGrupoGeneral = this.modGlobal.GetControlValue(AppControl.GrupoEmpresaGeneral);
                #endregion
                #region Inicializa modulos y DALs

                this.modApp = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);
                this.modGlobal = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);
                this.modPlaneacion = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);
                this.modProveedores = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);

                this._masterSimple = new DAL_MasterSimple(this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);
                this._masterHierarchy = new DAL_MasterHierarchy(this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);
                this._masterComplex = new DAL_MasterComplex(this.mySqlConnection, null, this.empresa, this.user.ReplicaID.Value.Value, this.connLoggerString);

                #endregion

                return MethodResult.OK.ToString();
            }
            catch(Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region Maestras

        #region MasterSimple

        /// <summary>
        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra basica</returns>
        private DTO_MasterBasic MasterSimple_GetByID(int documentID, UDT_BasicID id)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterSimple.DocumentID = documentID;
                DTO_MasterBasic response = this._masterSimple.DAL_MasterSimple_GetByID(id, true, null);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///  Adiciona una lista a la maestra básica
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de la operacion "usuario,progreso"</param>
        /// <returns>Resultado</returns>
        public string MasterSimple_Add(int documentID, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterSimple.DocumentID = documentID;

                //Propiedades
                DTO_aplMaestraPropiedades prop = this.masterProperties[documentID];
                Type t = Type.GetType("NewAge.DTO.Negocio." + prop.DTOTipo + ", NewAge.DTO");
                object objDTO = Activator.CreateInstance(t);
                DTO_TxResult result = this.LoadDTO(objDTO, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Carga el DTO y el grupo de empresas
                DTO_MasterBasic basicDTO = (DTO_MasterBasic)objDTO;
                if (prop.GrupoEmpresaInd)
                    basicDTO.EmpresaGrupoID.Value = this.GetMaestraEmpresaGrupoByDocumentID(documentID, prop.TipoSeguridad);

                //Realiza la consulta
                DTO_TxResultDetail txD = this._masterSimple.DAL_MasterSimple_AddItem(basicDTO);
                if (txD != null && txD.DetailsFields.Count > 0)
                {
                    txD.line = 1;
                    txD.Key = basicDTO.ID.Value;

                    result.Result = ResultValue.NOK;
                    result.Details.Add(txD);
                }

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Actualiza una maestra básica
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public string MasterSimple_Update(int documentID, string id, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterSimple.DocumentID = documentID;

                //Carga el DTO
                UDT_BasicID udt = new UDT_BasicID() { Value = id, IsInt = false };
                DTO_MasterBasic simpleDTO = this.MasterSimple_GetByID(documentID, udt);
                DTO_TxResult result = this.LoadDTO(simpleDTO, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Realiza la consulta
                result = this._masterSimple.DAL_MasterSimple_Update(simpleDTO, false);

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Borra una maestra básica a partir de su id
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <returns>Resultado</returns>  
        public string MasterSimple_Delete(int documentID, string id)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterSimple.DocumentID = documentID;

                //Realiza la consulta
                UDT_BasicID udt = new UDT_BasicID() { Value = id, IsInt = false };
                DTO_TxResult result = this._masterSimple.DAL_MasterSimple_Delete(udt, false);

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region MasterHierarchy

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra jerarquica</returns>
        private DTO_MasterHierarchyBasic MasterHierarchy_GetByID(int documentID, UDT_BasicID id)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterHierarchy.DocumentID = documentID;
                DTO_MasterBasic dto = this._masterHierarchy.DAL_MasterSimple_GetByID(id, true, null);

                if (dto != null)
                {
                    DTO_MasterHierarchyBasic response = this._masterHierarchy.CompleteHierarchy(dto);
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Adiciona una lista de dtos
        /// </summary>
        /// <param name="bItems">Lista de datos</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="progress">Información con el progreso de la transacción</param>
        /// <returns>Resultado</returns>
        public string MasterHierarchy_Add(int documentID, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterHierarchy.DocumentID = documentID;

                //Propiedades
                DTO_aplMaestraPropiedades prop = this.masterProperties[documentID];
                Type t = Type.GetType("NewAge.DTO.Negocio." + prop.DTOTipo + ", NewAge.DTO");
                object objDTO = Activator.CreateInstance(t);
                DTO_TxResult result = this.LoadDTO(objDTO, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Carga el DTO y el grupo de empresas
                DTO_MasterHierarchyBasic hierarchyDTO = (DTO_MasterHierarchyBasic)objDTO;
                if (prop.GrupoEmpresaInd)
                    hierarchyDTO.EmpresaGrupoID.Value = this.GetMaestraEmpresaGrupoByDocumentID(documentID, prop.TipoSeguridad);

                #region Valida que la jerarquía este bien y que exista el padre

                bool validParents = this._masterHierarchy.DAL_MasterHierarchy_CheckParents(hierarchyDTO.ID);
                if (!validParents)
                {
                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                    rd.line = 1;
                    rd.Key = hierarchyDTO.ID.Value;
                    rd.Message = "NOK";

                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = this.GetResourceForm(documentID.ToString() + "_" + prop.ColumnaID);
                    rdF.Message = this.GetResourceError(DictionaryMessages.Err_HierarNoParentFound);
                    rd.DetailsFields.Add(rdF);

                    result.Result = ResultValue.NOK;
                    result.Details.Add(rd);
                    string resParents = this.ObjectToXML(result);
                    return resParents;
                }
                #endregion

                //Realiza la consulta
                DTO_TxResultDetail txD = this._masterSimple.DAL_MasterSimple_AddItem(hierarchyDTO);
                if (txD != null && txD.DetailsFields.Count > 0)
                {
                    txD.line = 1;
                    txD.Key = hierarchyDTO.ID.Value;

                    result.Result = ResultValue.NOK;
                    result.Details.Add(txD);
                }

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Actualiza un registro de una maestra jerarquica
        /// </summary>
        /// <param name="item">Registro para actualizar</param>
        /// <returns>Resultado</returns>
        public string MasterHierarchy_Update(int documentID, string id, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterHierarchy.DocumentID = documentID;

                //Carga el DTO
                UDT_BasicID udt = new UDT_BasicID() { Value = id, IsInt = false };
                DTO_MasterHierarchyBasic hierarchyDTO = this.MasterHierarchy_GetByID(documentID, udt);
                DTO_TxResult result = this.LoadDTO(hierarchyDTO, props, values);
                if(result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Realiza la consulta
                result = this._masterHierarchy.DAL_MasterHierarchy_Update(hierarchyDTO, false);

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Borra un registro
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Resultado</returns>
        public string MasterHierarchy_Delete(int documentID, string id)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterHierarchy.DocumentID = documentID;
                
                //Realiza la consulta
                UDT_BasicID udt = new UDT_BasicID() { Value = id, IsInt = false };
                DTO_TxResult result = this._masterHierarchy.DAL_MasterHierarchy_Delete(udt, false);

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion

            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region MasterComplex - Llaves multiples

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="pks">Identificador de la maestra</param>
        /// <param name="EmpresaGrupoID">Identificador por el cual se filtra</param>
        /// <returns>Devuelve la maestra basica</returns>
        private DTO_MasterComplex MasterComplex_GetByID(int documentID, Dictionary<string, string> pks)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterComplex.DocumentID = documentID;
                DTO_MasterComplex response = this._masterComplex.DAL_MasterComplex_GetByID(pks, true);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Adiciona una lista a la maestra
        /// </summary>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de insercion de los datos</param>
        /// <returns>Resultado</returns>
        public string MasterComplex_Add(int documentID, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterComplex.DocumentID = documentID;

                //Propiedades
                DTO_aplMaestraPropiedades prop = this.masterProperties[documentID];
                Type t = Type.GetType("NewAge.DTO.Negocio." + prop.DTOTipo + ", NewAge.DTO");
                object objDTO = Activator.CreateInstance(t);
                DTO_TxResult result = this.LoadDTO(objDTO, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Carga el DTO y el grupo de empresas
                DTO_MasterComplex complexDTO = (DTO_MasterComplex)objDTO;
                if (prop.GrupoEmpresaInd)
                    complexDTO.EmpresaGrupoID.Value = this.GetMaestraEmpresaGrupoByDocumentID(documentID, prop.TipoSeguridad);

                //Realiza la consulta
                DTO_TxResultDetail txD = this._masterComplex.DAL_MasterComplex_AddItem(complexDTO);
                if (txD != null && txD.DetailsFields.Count > 0)
                {
                    txD.line = 1;
                    txD.Key = string.Empty;

                    result.Result = ResultValue.NOK;
                    result.Details.Add(txD);
                }

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Actualiza una maestra
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public string MasterComplex_Update(int documentID, string pkKeys, string pkValues, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterComplex.DocumentID = documentID;

                //Carga la info de la PK
                string[] pk_keysArr = pkKeys.Split(new string[] { "," }, StringSplitOptions.None);
                string[] pk_valuesArr = pkValues.Split(new string[] { "," }, StringSplitOptions.None);
                Dictionary<string, string> pks = new Dictionary<string, string>();
                for (int i = 0; i < pk_keysArr.Count(); ++i)
                    pks.Add(pk_keysArr[i], pk_valuesArr[i]);

                //Carga el DTO
                DTO_MasterComplex complexDTO = this.MasterComplex_GetByID(documentID, pks);
                DTO_TxResult result = this.LoadDTO(complexDTO, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Realiza la consulta
                result = this._masterComplex.DAL_MasterComplex_Update(complexDTO, false);

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion

            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        public string MasterComplex_Delete(int documentID, string pkKeys, string pkValues)
        {
            try
            {
                this.ADO_ConnectDB();
                this._masterComplex.DocumentID = documentID;

                //Carga la PK
                string[] pk_keysArr = pkKeys.Split(new string[] { "," }, StringSplitOptions.None);
                string[] pk_valuesArr = pkValues.Split(new string[] { "," }, StringSplitOptions.None);                
                Dictionary<string, string> pks = new Dictionary<string, string>();
                for (int i = 0; i < pkKeys.Count(); ++i)
                    pks.Add(pk_keysArr[i], pk_valuesArr[i]);

                //realiza la consulta
                DTO_TxResult result = this._masterComplex.DAL_MasterComplex_Delete(pks, false);

                #region Carga el resultado
                if (result.Result == ResultValue.OK)
                    return MethodResult.OK.ToString();
                else
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion

            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #endregion

        #region Global

        /// <summary>
        /// Agrega un registro a glDocumentoControl y guarda en las bitacoras
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el documento</param>
        /// <param name="docCtrl">Documento que se va a insertar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public string glDocumentoControl_Add(int documentID, string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                DTO_TxResult result = this.LoadDTO(docCtrl, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                //Realiza la consulta
                DTO_TxResultDetail txD = this.modGlobal.glDocumentoControl_Add(documentID, docCtrl, false);

                #region Carga el resultado
                if (txD.Message == ResultValue.OK.ToString())
                    return MethodResult.OK.ToString();
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(txD);

                    this.AssignResultResources(documentID, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        /// <param name="updBitacora">Indica si se debe actualizar la bitacora</param>
        /// <returns></returns>
        public string glDocumentoControl_Update(int documentID, int numeroDoc, string props, string values)
        {
            this.ADO_ConnectDB();

            DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
            DTO_TxResult result = this.LoadDTO(docCtrl, props, values);
            if (result.Result == ResultValue.NOK)
            {
                this.AssignResultResources(documentID, result);
                string res = this.ObjectToXML(result);
                return res;
            }

            //Realiza la consulta
            DTO_TxResultDetail txD = this.modGlobal.glDocumentoControl_Update(docCtrl, false, false);

            #region Carga el resultado
            if (txD.Message == ResultValue.OK.ToString())
                return MethodResult.OK.ToString();
            else
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = "NOK";
                result.Details.Add(txD);

                this.AssignResultResources(documentID, result);
                string res = this.ObjectToXML(result);
                return res;
            }
            #endregion
        }

        #endregion

        #region Planeacion

        #region plCierreLegalizacion

        /// <summary>
        /// Actualiza un registro de plCierreLegalizacion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        public string plCierreLegalizacion_Add(string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_plCierreLegalizacion cierre = new DTO_plCierreLegalizacion();
                DTO_TxResult result = this.LoadDTO(cierre, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(null, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                this.modPlaneacion.plCierreLegalizacion_Add(cierre);
                return MethodResult.OK.ToString();
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region plSobreEjecucion

        /// <summary>
        /// Actualiza un registro de plSobreEjecucion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public string plSobreEjecucion_Add(string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_plSobreEjecucion sobre = new DTO_plSobreEjecucion();
                DTO_TxResult result = this.LoadDTO(sobre, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(null, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                this.modPlaneacion.plSobreEjecucion_Add(sobre);
                return MethodResult.OK.ToString();
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region plPresupuestoSoporte

        /// <summary>
        /// Actualiza un registro de plPresupuestoSoporte
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public string plPresupuestoSoporte_Add(string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_plPresupuestoSoporte sobre = new DTO_plPresupuestoSoporte();
                DTO_TxResult result = this.LoadDTO(sobre, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(null, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                this.modPlaneacion.plPresupuestoSoporte_Add(sobre);
                return MethodResult.OK.ToString();
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region plPresupuestoPxQ

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQ
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public string plPresupuestoPxQ_Add(string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_plPresupuestoPxQ sobre = new DTO_plPresupuestoPxQ();
                DTO_TxResult result = this.LoadDTO(sobre, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(null, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                this.modPlaneacion.plPresupuestoPxQ_Add(sobre);
                return MethodResult.OK.ToString();
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #region plPresupuestoPxQ

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQDeta
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public string plPresupuestoPxQDeta_Add(string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_plPresupuestoPxQDeta sobre = new DTO_plPresupuestoPxQDeta();
                DTO_TxResult result = this.LoadDTO(sobre, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(null, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                this.modPlaneacion.plPresupuestoPxQDeta_Add(sobre);
                return MethodResult.OK.ToString();
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #endregion

        #region Proveedores

        #region prCierreMesCostos

        /// <summary>
        /// Actualiza un registro de plCierreLegalizacion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        public string prCierreMesCostos_Add(string props, string values)
        {
            try
            {
                this.ADO_ConnectDB();

                DTO_prCierreMesCostos cierre = new DTO_prCierreMesCostos();
                DTO_TxResult result = this.LoadDTO(cierre, props, values);
                if (result.Result == ResultValue.NOK)
                {
                    this.AssignResultResources(null, result);
                    string res = this.ObjectToXML(result);
                    return res;
                }

                this.modProveedores.prCierreMesCostos_Add(cierre);
                return MethodResult.OK.ToString();
            }
            catch (Exception ex)
            {
                this.ADO_CloseDBConnection();
                return MethodResult.NOK.ToString() + ": " + ex.Message;
            }
        }

        #endregion

        #endregion

    }
}
