using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Resultados;
using NewAge.ADO;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System.Net;
using System.Configuration;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;
using System.IO;

namespace NewAge.Negocio
{
    public class ModuloAplicacion : ModuloBase
    {
        #region Variables

        #region DALS

        private DAL_Alarmas _dal_Alarmas = null;
        private DAL_aplBitacora _dal_aplBitacora = null;
        private DAL_aplIdioma _dal_Idioma = null;
        private DAL_aplIdiomaTraduccion _dal_aplIdiomaTraduccion = null;
        private DAL_aplMaestraPropiedad _dal_aplMaestraPropiedad = null;
        private DAL_aplModulo _dal_aplModulo = null;
        private DAL_OperacionesDocumentos _dal_OperacionesDocumentos = null;
        private DAL_Reportes _dal_Reportes = null;
        private DAL_Temporales _dal_Temporales = null;
        private DAL_aplMaestraCampo _dal_aplMaestraCampo = null;

        #endregion

        #region Modulos

        private ModuloGlobal _moduloGlobal = null;
        private ModuloContabilidad _moduloContabilidad = null;

        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Aplicacion
        /// </summary>
        /// <param name="conn">conexion</param>
        public ModuloAplicacion(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Alarmas

        /// <summary>
        /// Dice si un usuario tiene alarmas pendientes
        /// </summary>
        /// <returns>Devuelve verdadero si el usuario tiene alarmas</returns>
        public bool Alarmas_HasAlarms(string userName)
        {
            this._dal_Alarmas = (DAL_Alarmas)base.GetInstance(typeof(DAL_Alarmas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Alarmas.DAL_Alarmas_HasAlarms(userName);
        }

        /// <summary>
        /// Trae el listado de tareas pendientes para envio de correos
        /// </summary>
        /// <returns>Retorna el listado de tareas pendientes</returns>
        public IEnumerable<DTO_Alarma> Alarmas_GetAll(string userName = null)
        {
            List<DTO_Alarma> tareas = new List<DTO_Alarma>();
            List<DTO_Alarma> tareasResult = new List<DTO_Alarma>();

            this._dal_Alarmas = (DAL_Alarmas)base.GetInstance(typeof(DAL_Alarmas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            try
            {
                tareas = (List<DTO_Alarma>)this._dal_Alarmas.DAL_Alarmas_GetAll(userName);                
                #region Carga los DTO
                foreach (var tarea in tareas)
                {
                    tarea.FileName = base.GetFileRemotePath(tarea.NumeroDoc,TipoArchivo.Documentos); //filePath + string.Format(fileFormat, tarea.ID) + ext;

                    //try
                    //{
                    //    WebRequest req = WebRequest.Create(tarea.FileName);
                    //    WebResponse res = req.GetResponse();
                    //}
                    //catch (WebException ex)
                    //{
                    //    tarea.FileName = string.Empty;
                    //}

                    tareasResult.Add(tarea);
                }

                #endregion

                if (userName == null)
                {
                    Dictionary<string, string> userMails = new Dictionary<string, string>();
                    Dictionary<string, string> userLangs = new Dictionary<string, string>();

                    #region Saca los correos de los usuarios a los que toca enviarles alarmas
                    DAL_MasterSimple usrDAL = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    usrDAL.DocumentID = AppMasters.seUsuario;

                    UDT_BasicID udt = new UDT_BasicID();

                    DTO_seUsuario user;
                    foreach (DTO_Alarma t in tareas)
                    {
                        #region Revisa que los usuarios esten en la lista de los correos
                        //Usuario 1
                        if (!userMails.ContainsKey(t.UsuarioID1))
                        {
                            udt.Value = t.UsuarioID1;
                            user = (DTO_seUsuario)usrDAL.DAL_MasterSimple_GetByID(udt, false);
                            userMails.Add(t.UsuarioID1, user.CorreoElectronico.Value);
                            userLangs.Add(t.UsuarioID1, user.IdiomaID.Value);
                        }

                        //Usuario 2
                        if (!userMails.ContainsKey(t.UsuarioID2))
                        {
                            udt.Value = t.UsuarioID2;
                            user = (DTO_seUsuario)usrDAL.DAL_MasterSimple_GetByID(udt, false);
                            userMails.Add(t.UsuarioID2, user.CorreoElectronico.Value);
                            userLangs.Add(t.UsuarioID2, user.IdiomaID.Value);
                        }

                        //Usuario 3
                        if (!userMails.ContainsKey(t.UsuarioID3))
                        {
                            udt.Value = t.UsuarioID3;
                            user = (DTO_seUsuario)usrDAL.DAL_MasterSimple_GetByID(udt, false);
                            userMails.Add(t.UsuarioID3, user.CorreoElectronico.Value);
                            userLangs.Add(t.UsuarioID3, user.IdiomaID.Value);
                        }
                        #endregion
                    }
                    #endregion
                    #region Asigna los correos a los usuarios
                    foreach (DTO_Alarma t in tareas)
                    {
                        //Asigna los correos de los usuarios
                        t.UsuarioMail1 = userMails[t.UsuarioID1];
                        t.UsuarioMail2 = userMails[t.UsuarioID2];
                        t.UsuarioMail3 = userMails[t.UsuarioID3];

                        //Asigna los idiomas de los usuarios
                        t.UsuarioLang1 = userLangs[t.UsuarioID1];
                        t.UsuarioLang2 = userLangs[t.UsuarioID2];
                        t.UsuarioLang3 = userLangs[t.UsuarioID3];
                    }
                    #endregion
                }

                return tareasResult;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Alarmas_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Envia una alarma a un usuario
        /// </summary>
        /// <param name="numeroDoc">Identificador unico del documento</param>
        /// <param name="documentoID">Pk del documento (glDocumentoControl)</param>
        /// <param name="usuario1">Usuario 1 al que se le envio la alarma</param>
        /// <param name="usuario2">Usuario 2 al que se le envio la alarma</param>
        /// <param name="usuario3">Usuario 3 al que se le envio la alarma</param>
        public void Alarmas_SendAlarm(string EmpresaID, int numeroDoc, int documentoID, string actFlujoID, int usuarioID, string usuario1, string usuario2, string usuario3, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool resutOk = true;

            try
            {
                this._dal_Alarmas = (DAL_Alarmas)base.GetInstance(typeof(DAL_Alarmas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Alarmas.DAL_Alarmas_SendAlarm(numeroDoc, actFlujoID, usuario1, usuario2, usuario3);

                #region Guarda en la bitacora de documento (gltareasControl)
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glActividadControl actCtrl = new DTO_glActividadControl();
                actCtrl.EmpresaID.Value = EmpresaID;
                actCtrl.NumeroDoc.Value = numeroDoc;
                actCtrl.DocumentoID.Value = documentoID;
                actCtrl.ActividadFlujoID.Value = actFlujoID;
                actCtrl.UsuarioID.Value = usuarioID;
                actCtrl.Fecha.Value = DateTime.Now;
                actCtrl.Periodo.Value = DateTime.Now;
                actCtrl.Observacion.Value = DictionaryMessages.Gl_TareaAlarma;
                actCtrl.AlarmaInd.Value = true;

                this.AgregarActividadControl(actCtrl);
                #endregion

            }
            catch (Exception ex)
            {
                resutOk = false;
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Alarmas_SendAlarm");
                throw exception;
            }
            finally
            {
                if (resutOk)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        #region Funciones Servidor

        /// <summary>
        /// Asigna las alarmas de un documento
        /// </summary>
        /// <param name="actividadFlujoID">Identificador de la tarea</param>
        /// <param name="currentDate">Fecha actual</param>
        /// <param name="NumeroDoc">Consecutivo UNICO de glDocumentoControl</param>
        internal DTO_glActividadEstado Alarmas_GetDocAlarms(string actividadFlujoID)
        {
            try
            {
                DTO_glActividadEstado resp = new DTO_glActividadEstado();

                DTO_glActividadFlujo actFlujo = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadFlujoID, true, false);
                UnidadTiempo un = (UnidadTiempo)Enum.Parse(typeof(UnidadTiempo), actFlujo.UnidadTiempo.Value.Value.ToString());

                //Alarma 1
                if (actFlujo.Alarma1Ind.Value.Value && un != UnidadTiempo.NoAplica)
                    resp.FechaAlarma1.Value = this.CalculateAlarmDate(un, actFlujo.AlarmaPeriodo1.Value.Value);
                else
                    resp.FechaAlarma1 = null;

                //Alarma 2
                if (actFlujo.Alarma2Ind.Value.Value && un != UnidadTiempo.NoAplica)
                    resp.FechaAlarma2.Value = this.CalculateAlarmDate(un, actFlujo.AlarmaPeriodo2.Value.Value);
                else
                    resp.FechaAlarma2 = null;

                //Alarma 3
                if (actFlujo.Alarma3Ind.Value.Value && un != UnidadTiempo.NoAplica)
                    resp.FechaAlarma3.Value = this.CalculateAlarmDate(un, actFlujo.AlarmaPeriodo3.Value.Value);
                else
                    resp.FechaAlarma3 = null;

                return resp;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Alarmas_GetDocAlarms"); 
                throw exception;
            }
        }

        /// <summary>
        /// Calcula una nueva fecha a partir de un periodo de tiempo
        /// </summary>
        /// <param name="un">Unidad de tiempo</param>
        /// <param name="cantTiempo">Tiempo para incrementar en la fecha</param>
        /// <returns>Retorna la nueva fecha</returns>
        private DateTime CalculateAlarmDate(UnidadTiempo un, int cantTiempo)
        {
            DateTime res = DateTime.Now;
            switch (un)
            {
                case UnidadTiempo.Minuto:
                    res = res.AddMinutes(cantTiempo);
                    break;
                case UnidadTiempo.Hora:
                    res = res.AddHours(cantTiempo);
                    break;
                case UnidadTiempo.Dia:
                    res = res.AddDays(cantTiempo);
                    break;
                case UnidadTiempo.Semana:
                    int days = cantTiempo * 7;
                    res = res.AddDays(days);
                    break;
                case UnidadTiempo.Mes:
                    res = res.AddMonths(cantTiempo);
                    break;
            }

            return res;
        }

        #endregion

        #endregion

        #region AplBitacora

        /// <summary>
        /// Inserta una bitcora
        /// </summary>
        /// <param name="empresaID">Empresa en la que se esta trabajando</param>
        /// <param name="documentoID">formaID</param>
        /// <param name="accionID">accionID</param>
        /// <param name="fecha">fecha</param>
        /// <param name="seUsuarioID">seUsuarioID</param>
        /// <param name="llp01">llp01</param>
        /// <param name="llp02">llp02</param>
        /// <param name="llp03">llp03</param>
        /// <param name="llp04">llp04</param>
        /// <param name="llp03">llp05</param>
        /// <param name="llp04">llp06</param>
        /// <param name="bitacoraOrigenID">bitacoraOrigenID</param>
        /// <param name="bitacoraPadreID">bitacoraPadreID</param>
        /// <param name="bitacoraAnulacionID">bitacoraAnulacionID</param>
        /// <returns>True si se ingreso False de lo contrario</returns>
        public int aplBitacora_Add(string empresaID, int documentoID, short accionID, DateTime fecha, int seUsuarioID, string llp01, string llp02, string llp03, string llp04, string llp05, string llp06, int bitacoraOrigenID, int bitacoraPadreID, int bitacoraAnulacionID)
        {
            this._dal_aplBitacora = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_aplBitacora.DAL_aplBitacora_Add(empresaID, documentoID, accionID, fecha, seUsuarioID, llp01, llp02, llp03, llp04, llp05, llp06, bitacoraOrigenID, bitacoraPadreID, bitacoraAnulacionID);
        }

        /// <summary>
        /// Consulta la bitácora dado un filtro por páginas
        /// </summary>
        /// <param name="pageSize">Tamano de la pagina</param>
        /// <param name="pageNum">Numero de la pagina</param>
        /// <param name="consulta">dto con el filtro</param>
        /// <returns>enumeracion de dtos de bitacora</returns>
        public IEnumerable<DTO_aplBitacora> aplBitacoraGetFilteredPaged(int pageSize, int pageNum, DTO_glConsulta consulta)
        {
            try
            {
                int ini = (pageNum - 1) * pageSize + 1;
                int fin = pageNum * pageSize;

                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                if (consulta != null && consulta.Filtros != null)
                    filtros = consulta.Filtros;

                string where = Transformer.WhereSql(filtros, typeof(DTO_aplBitacora));
                if (!string.IsNullOrWhiteSpace(where))
                    where = "WHERE " + where;

                //DAL
                this._dal_aplBitacora = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_aplBitacora.DAL_aplBitacoraGetFilteredPaged(pageSize, pageNum, consulta, ini, fin, where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae datos de la bitacora filtrada
        /// </summary>
        /// <param name="consulta">Filtros a aplicar</param>
        /// <returns>Retorna los registros de la bitacora</returns>
        public IEnumerable<DTO_aplBitacora> aplBitacoraGetFiltered(DTO_glConsulta consulta)
        {
            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

            if (consulta != null && consulta.Filtros != null)
                filtros = consulta.Filtros;

            string where = Transformer.WhereSql(filtros, typeof(DTO_aplBitacora));
            if (!string.IsNullOrWhiteSpace(where))
                where = "WHERE " + where;

            //DAL
            this._dal_aplBitacora = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_aplBitacora.DAL_aplBitacoraGetFiltered(consulta, where);
        }

        /// <summary>
        /// Trae la cantidad de registros de la bitacora despues de aplicar un filtro
        /// </summary>
        /// <param name="consulta">Filtros a aplicar</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        public long aplBitacoraCountFiltered(DTO_glConsulta consulta)
        {
            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            if (consulta != null && consulta.Filtros != null)
                filtros = consulta.Filtros;

            string where = Transformer.WhereSql(filtros, typeof(DTO_aplBitacora));
            if (!string.IsNullOrWhiteSpace(where))
                where = "WHERE " + where;

            this._dal_aplBitacora = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_aplBitacora.DAL_aplBitacoraCountFiltered(consulta, where);
        }

        /// <summary>
        /// Dice si una empresa ha sido eliminada
        /// </summary>
        /// <param name="empresaID">empresa a eliminar</param>
        /// <returns>devuelve true si la empresa se elimino correctamente sino devuelve false</returns>
        public bool aplBitacora_DeleteCompany(string empresaID)
        {
            this._dal_aplBitacora = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_aplBitacora.DAL_aplBitacora_DeleteCompany(empresaID);
        }

        #endregion

        #region AplIdioma

        /// <summary>
        /// Trae todos los aplIdioma
        /// </summary>
        /// <returns>Lista de aplIdioma</returns>
        public IEnumerable<DTO_aplIdioma> aplIdioma_GetAll()
        {
            _dal_Idioma = (DAL_aplIdioma)this.GetInstance(typeof(DAL_aplIdioma), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_Idioma.DAL_aplIdioma_GetAll();
        }

        #endregion

        #region AplIdiomaTraduccion

        /// <summary>
        /// Trae todos los aplIdiomaTraduccion
        /// </summary>
        /// <returns>Lista de aplIdiomaTraduccion</returns>
        public DTO_aplIdiomaTraduccion aplIdiomaTraduccion_GetById(string idiomaId, string llave)
        {
            _dal_aplIdiomaTraduccion = (DAL_aplIdiomaTraduccion)this.GetInstance(typeof(DAL_aplIdiomaTraduccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_aplIdiomaTraduccion.DAL_aplIdiomaTraduccion_GetById(idiomaId, llave);
        }

        /// <summary>
        /// Trae todos los aplIdiomaTraduccion
        /// </summary>
        /// <returns>Lista de aplIdiomaTraduccion</returns>
        public IEnumerable<DTO_aplIdiomaTraduccion> aplIdiomaTraduccion_GetByIdiomaId(string idiomaId)
        {
            _dal_aplIdiomaTraduccion = (DAL_aplIdiomaTraduccion)this.GetInstance(typeof(DAL_aplIdiomaTraduccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_aplIdiomaTraduccion.DAL_aplIdiomaTraduccion_GetByIdiomaId(idiomaId);
        }

        #endregion

        #region AplMaestraCampo

         /// <summary>
        /// Trae la informacion el registro de maestra campo asociado al documento y columna
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="columnName">nombre de la columna</param>
        /// <returns></returns>
        public DTO_aplMaestraCampo aplMaestraCampo_GetColumn(int documentID, string columnName)
        {
            _dal_aplMaestraCampo = (DAL_aplMaestraCampo)this.GetInstance(typeof(DAL_aplMaestraCampo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_aplMaestraCampo.DAL_aplMaestraCampo_GetColumn(documentID, columnName);
        }

        #endregion

        #region AplMaestraPropiedad

        /// <summary>
        /// Retorna todas la lista de paramestros para las maestras
        /// </summary>
        /// <returns>Lista de MasterParams</returns>
        public IEnumerable<DTO_aplMaestraPropiedades> aplMaestraPropiedades_GetAll()
        {
            _dal_aplMaestraPropiedad = (DAL_aplMaestraPropiedad)this.GetInstance(typeof(DAL_aplMaestraPropiedad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_aplMaestraPropiedad.DAL_aplMaestraPropiedades_GetAll();
        }

        #endregion

        #region AplModulo

        /// <summary>
        /// Indica si están abiertos(true) o cerrados(false) los periodos de los módulos
        /// </summary>
        /// <returns></returns>
        internal Dictionary<ModulesPrefix, EstadoPeriodo> aplModulo_EstadoPeriodoModulos(DateTime periodo)
        {
            try
            {
                Dictionary<ModulesPrefix, EstadoPeriodo> res = new Dictionary<ModulesPrefix, EstadoPeriodo>();
                this._dal_aplModulo = (DAL_aplModulo)this.GetInstance(typeof(DAL_aplModulo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                IEnumerable<DTO_aplModulo> modulos = this._dal_aplModulo.DAL_aplModulo_GetByVisible(1);
                
                //Revisa solo los modulos activos
                foreach (ModulesPrefix mod in Enum.GetValues(typeof(ModulesPrefix)))
                {
                    if (mod != ModulesPrefix.apl && mod != ModulesPrefix.gl && mod != ModulesPrefix.se)
                    {
                        int countMod = modulos.Where(x => x.ModuloID.Value.ToUpper().Trim().Equals(mod.ToString().ToUpper())).Count();
                        if (countMod > 0)
                        {
                            EstadoPeriodo est = this.CheckPeriod(periodo, mod, true);
                            res.Add(mod, est);
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "aplModulo_EstadoPeriodoModulos");
                throw exception;
            }
        }

        /// <summary>
        /// Indica los periodos actuales de los módulos
        /// </summary>
        /// <returns></returns>
        internal Dictionary<ModulesPrefix, string> aplModulo_GetAllPeriodoActual()
        {
            try
            {
                Dictionary<ModulesPrefix, string> res = new Dictionary<ModulesPrefix, string>();
                string EmpNro = this.Empresa.NumeroControl.Value;

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_aplModulo = (DAL_aplModulo)this.GetInstance(typeof(DAL_aplModulo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                IEnumerable<DTO_aplModulo> modulos = this._dal_aplModulo.DAL_aplModulo_GetByVisible(1);

                //Revisa solo los modulos activos
                foreach (ModulesPrefix mod in Enum.GetValues(typeof(ModulesPrefix)))
                {
                    if (mod != ModulesPrefix.apl && mod != ModulesPrefix.gl && mod != ModulesPrefix.se)
                    {
                        int countMod = modulos.Where(x => x.ModuloID.Value.ToUpper().Trim().Equals(mod.ToString().ToUpper())).Count();
                        if (countMod > 0)
                        {
                            string _modId = ((int)mod).ToString();
                            if (_modId.Length == 1)
                                _modId = "0" + _modId;
                            DTO_glControl periodoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(EmpNro + _modId + AppControl.co_Periodo.ToString()));
                            if (periodoControl != null)   
                                res.Add(mod, periodoControl.Data.Value);
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "aplModulo_EstadoPeriodoModulos");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna todos los aplModulo que tiene estado activo
        /// </summary>
        /// <param name="active">activo</param>
        /// <param name="onlyOperative">Indica si solo trae los modulo operativos</param>
        /// <returns>Enumeracion de aplModulo</returns>
        public IEnumerable<DTO_aplModulo> aplModulo_GetByVisible(short active, bool onlyOperative)
        {
            try
            {
                this._dal_aplModulo = (DAL_aplModulo)this.GetInstance(typeof(DAL_aplModulo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_aplModulo> mods = this._dal_aplModulo.DAL_aplModulo_GetByVisible(active).ToList();
                if(onlyOperative)
                {
                    List<DTO_aplModulo> mOperatives = new List<DTO_aplModulo>();
                    foreach (DTO_aplModulo m in mods)
                    {
                        string mID = m.ModuloID.Value.ToLower();
                        if (mID != ModulesPrefix.apl.ToString() && mID != ModulesPrefix.gl.ToString() && mID != ModulesPrefix.se.ToString())
                            mOperatives.Add(m);
                    }
                    return mOperatives;
                }

                return mods;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "aplModulo_GetByVisible");
                throw exception;
            }
        }

        #endregion

        #region AplReporte

        /// <summary>
        /// Obtiene un resporte predefinido para una empresa
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public byte[] aplReporte_GetByID(int documentoID)
        {
            DAL_aplReporte dal_reporte = new DAL_aplReporte(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return dal_reporte.DAL_aplReporte_GetByID(documentoID);
        }

        /// <summary>
        /// Ingresa o actualiza un reporte predefinido para un usuario
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public void aplReporte_Update(DTO_aplReporte report)
        {
            DAL_aplReporte dal_reporte = new DAL_aplReporte(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            dal_reporte.DAL_aplReporte_Update(report);
        }


        #endregion

        #region Operaciones Documentos

        internal long AsignarIdentificadorTR(DTO_ComprobanteFooter footer, DTO_glConceptoSaldo concSaldo)
        {
            try
            {
                int saldoCtrl = concSaldo.coSaldoControl.Value.Value;
                if (saldoCtrl == (byte)SaldoControl.Doc_Interno)
                {
                    this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    Int16 docInt = 0;
                    Int16.TryParse(footer.DocumentoCOM.Value, out docInt);
                    if (docInt != 0)
                    {
                        DTO_glDocumentoControl ctrlInt = this._moduloGlobal.glDocumentoControl_GetInternalDocByCta(footer.CuentaID.Value, footer.PrefijoCOM.Value, docInt);
                        if (ctrlInt != null)
                            return ctrlInt.NumeroDoc.Value.Value;
                    }
                }
                else if (saldoCtrl == (byte)SaldoControl.Doc_Externo || saldoCtrl == (byte)SaldoControl.Componente_Documento)
                {
                    this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_glDocumentoControl ctrlExt = this._moduloGlobal.glDocumentoControl_GetExternalDocByCta(footer.CuentaID.Value, footer.TerceroID.Value, footer.DocumentoCOM.Value);
                    if (ctrlExt != null)
                        return ctrlExt.NumeroDoc.Value.Value;
                }
                else if (saldoCtrl == (byte)SaldoControl.Componente_Tercero)
                {
                    return Convert.ToInt64(footer.TerceroID.Value);
                }

                return 0;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "AsignarIdentificadorTR");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si se ha realizado algun proceso de cierre de un modulo para un oerdioo determinado
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo ha tenido cierres</returns>
        public bool PeriodoHasCierre(DateTime periodo, ModulesPrefix mod)
        {
            try
            {
                bool exists = false;
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad.IsPeriodoCerrado(ModulesPrefix.co, periodo, ref exists);

                return exists;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "PeriodoHasCierre");
                return true;
            }
        }

        /// <summary>
        /// Verifica si un periodo existe y el mes esta abierto
        /// </summary>
        /// <param name="empresaID">Identificador de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <param name="validateCierre">Para modulos diferentes de contabilidad verifica si debe estar en coCierresControl</param>
        /// <returns>Verdadero si el periodo se puede usar</returns>
        public EstadoPeriodo CheckPeriod(DateTime periodo, ModulesPrefix mod, bool validateCierre = false)
        {
            try
            {
                bool exists = false;
                string cierre = this.GetControlValueByCompany(mod, AppControl.co_IndBloqueoModuloTransAuxPrelim);
                if (cierre == "1")
                    return EstadoPeriodo.EnCierre;

                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool periodoCerrado;
                if (mod == ModulesPrefix.co)
                {
                    periodoCerrado = this._moduloContabilidad.IsPeriodoCerrado(ModulesPrefix.co, periodo, ref exists);
                    if (periodoCerrado)
                        return EstadoPeriodo.Cerrado;
                    else
                        return EstadoPeriodo.Abierto;
                }
                else
                {
                    DateTime pAbierto = Convert.ToDateTime(this.GetControlValueByCompany(mod, AppControl.co_Periodo));
                    if (pAbierto == periodo)
                        return EstadoPeriodo.Abierto;
                    else
                    {
                        periodoCerrado = this._moduloContabilidad.IsPeriodoCerrado(mod, periodo, ref exists);
                        if (periodoCerrado)
                            return EstadoPeriodo.Cerrado;
                        else if(validateCierre)
                            return exists ? EstadoPeriodo.Abierto : EstadoPeriodo.Cerrado;
                        else
                            return EstadoPeriodo.Abierto;
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "CheckPeriod");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el prefijo de un documento dado
        /// </summary>
        /// <param name="areaFuncionalID">Codigo del area funcional</param>
        /// <param name="documentoID">Codigo del documento</param>
        /// <param name="empresaGrupoID">Codigo de grupo de empresas</param>
        /// <returns>Retorna el codigo del prefijo</returns>
        public string PrefijoDocumento_Get(string areaFuncionalID, int documentoID)
        {
            this._dal_OperacionesDocumentos = (DAL_OperacionesDocumentos)this.GetInstance(typeof(DAL_OperacionesDocumentos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_OperacionesDocumentos.PrefijoDocumento_Get(areaFuncionalID, documentoID);
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Trae la data para un reporte
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public List<DTO_BasicReport> GetReportData(int reportId, DTO_glConsulta consulta, byte[] bItems)
        {
            this._dal_Reportes = (DAL_Reportes)base.GetInstance(typeof(DAL_Reportes), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<ConsultasFields> fields = CompressedSerializer.Decompress<List<ConsultasFields>>(bItems);
            CompressedSerializer.Decompress<List<DTO_acActivoControl>>(bItems);
            
            return this._dal_Reportes.GetData(reportId, consulta, fields);
        }

        #endregion

        #region Temporales

        /// <summary>
        /// Revisa si existe un temporal segun el origen y el usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public bool aplTemporales_HasTemp(string origen, DTO_seUsuario usuario)
        {
            this._dal_Temporales = (DAL_Temporales)this.GetInstance(typeof(DAL_Temporales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Temporales.DAL_aplTemporales_HasTemp(origen, usuario);
        }

        /// <summary>
        /// Trae el temporal de un origen determinado y luego lo borra de los temporales
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public byte[] aplTemporales_GetByOrigen(string origen, DTO_seUsuario usuario)
        {
            this._dal_Temporales = (DAL_Temporales)this.GetInstance(typeof(DAL_Temporales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Temporales.DAL_aplTemporales_GetByOrigen(origen, usuario);
        }

        /// <summary>
        /// Guarda un objeto en temporales. También borra un objeto que anteriormente estuviese bajo el mismo origen para ese usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario</param>
        /// <param name="objeto">objeto a guardar</param>
        public void aplTemporales_Save(string origen, DTO_seUsuario usuario, object objeto)
        {
            this._dal_Temporales = (DAL_Temporales)this.GetInstance(typeof(DAL_Temporales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Temporales.DAL_aplTemporales_Save(origen, usuario, objeto);
        }

        /// <summary>
        /// Elimina los temporales de un usuario
        /// </summary>
        /// <param name="origen">Origen de los datos</param>
        /// <param name="usuario">Usuario que esta buscando temporales</param>
        public void aplTemporales_Clean(string origen, DTO_seUsuario usuario)
        {
            this._dal_Temporales = (DAL_Temporales)this.GetInstance(typeof(DAL_Temporales), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Temporales.DAL_aplTemporales_Clean(origen, usuario);
        }

        /// <summary>
        /// Elimina todos los archivos de la carpeta de temporales
        /// </summary>
        public void DeleteTempFiles()
        {
            try
            {
                string filesPath = this.GetControlValue(AppControl.RutaFisicaArchivos);
                string docsPath = this.GetControlValue(AppControl.RutaTemporales);

                DirectoryInfo downloadedMessageInfo = new DirectoryInfo(filesPath + docsPath);
                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception ex) { }
                }

            }
            catch(Exception ex){}
        }
        #endregion

    }
}
