using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_seDelegacionHistoria
    /// </summary>
    public class DAL_seDelegacionHistoria : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_seDelegacionHistoria(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Obtiene el historico de delegaciones de tareas
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna l lista de delegaciones</returns>
        public List<DTO_seDelegacionHistoria> DAL_seDelegacionHistoria_Get(string userID)
        {
            try
            {
                List<DTO_seDelegacionHistoria> result = new List<DTO_seDelegacionHistoria>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = userID;

                mySqlCommand.CommandText =
                    "select * from seDelegacionHistoria with(nolock) " +
                    "where EmpresaID=@EmpresaID and UsuarioID=@UsuarioID " +
                    "order by FechaInicialAsig desc ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_seDelegacionHistoria dto = new DTO_seDelegacionHistoria(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el historico de delegaciones de tareas
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna l lista de delegaciones</returns>
        public bool DAL_seDelegacionHistoria_Exists(string userID, DateTime fechaIni)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaInicialAsig", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = userID;
                mySqlCommand.Parameters["@FechaInicialAsig"].Value = fechaIni;

                mySqlCommand.CommandText =
                    "select count(*) from seDelegacionHistoria with(nolock) " +
                    "where EmpresaID=@EmpresaID and UsuarioID=@UsuarioID and FechaInicialAsig=@FechaInicialAsig ";

                Int16 count = Convert.ToInt16(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_Exists");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un delegado
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fichaFin">Fecha Final</param>
        /// <param name="usuarioRem">Usuario responsable</param>
        public void DAL_seDelegacionHistoria_Update(string userID, DateTime fechaIni, DateTime fichaFin, string usuarioRem)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaInicialAsig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFinalAsig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioRemplazo", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = userID;
                mySqlCommand.Parameters["@FechaInicialAsig"].Value = fechaIni;
                mySqlCommand.Parameters["@FechaFinalAsig"].Value = fichaFin;
                mySqlCommand.Parameters["@UsuarioRemplazo"].Value = usuarioRem;

                mySqlCommand.CommandText =
                    "update seDelegacionHistoria set FechaFinalAsig=@FechaFinalAsig, UsuarioRemplazo=@UsuarioRemplazo " +
                    "where EmpresaID=@EmpresaID and UsuarioID=@UsuarioID and FechaInicialAsig=@FechaInicialAsig ";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro a la delegacion de tareas
        /// </summary>
        /// <param name="delegacion">DTO a agregar</param>
        public void DAL_seDelegacionHistoria_Add(DTO_seDelegacionHistoria delegacion)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicialAsig", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinalAsig", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioRemplazo", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DelegacionActivaInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = delegacion.UsuarioID.Value;
                mySqlCommandSel.Parameters["@UsuarioRemplazo"].Value = delegacion.UsuarioRemplazo.Value;
                mySqlCommandSel.Parameters["@DelegacionActivaInd"].Value = delegacion.DelegacionActivaInd.Value.Value;

                if (delegacion.FechaInicialAsig.Value.HasValue)
                    mySqlCommandSel.Parameters["@FechaInicialAsig"].Value = delegacion.FechaInicialAsig.Value.Value;
                else
                    mySqlCommandSel.Parameters["@FechaInicialAsig"].Value = DBNull.Value;

                if (delegacion.FechaFinalAsig.Value.HasValue)
                    mySqlCommandSel.Parameters["@FechaFinalAsig"].Value = delegacion.FechaFinalAsig.Value.Value;
                else
                    mySqlCommandSel.Parameters["@FechaFinalAsig"].Value = DBNull.Value;

                mySqlCommandSel.CommandText =
                    "INSERT INTO seDelegacionHistoria(" +
                    "	EmpresaID,UsuarioID,FechaInicialAsig,FechaFinalAsig,UsuarioRemplazo,DelegacionActivaInd" +
                    ")VALUES(" +
                    "	@EmpresaID,@UsuarioID,@FechaInicialAsig,@FechaFinalAsig,@UsuarioRemplazo,@DelegacionActivaInd" +
                    ")";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_Add");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Actualiza el estado de un delegado
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        public void DAL_seDelegacionHistoria_UpdateStatus(string userID, DateTime fechaIni, bool enabled)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaInicialAsig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.Bit);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = userID;
                mySqlCommand.Parameters["@FechaInicialAsig"].Value = fechaIni;
                mySqlCommand.Parameters["@DelegacionActivaInd"].Value = enabled;

                mySqlCommand.CommandText =
                    "update seDelegacionHistoria set DelegacionActivaInd=@DelegacionActivaInd " +
                    "where EmpresaID=@EmpresaID and UsuarioID=@UsuarioID and FechaInicialAsig=@FechaInicialAsig ";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_UpdateStatus");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene al lista de delegaciones pendientes de asignar
        /// </summary>
        /// <returns>Retorna la lista de nuevas delegaciones</returns>
        public List<DTO_seDelegacionHistoria> DAL_seDelegacionHistoria_GetNewDelegaciones()
        {
            try
            {
                //Lista de nuevos registros
                List<DTO_seDelegacionHistoria> result = new List<DTO_seDelegacionHistoria>();
                Dictionary<string, DTO_glEmpresa> cacheEmpresas = new Dictionary<string, DTO_glEmpresa>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.Bit);

                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now.Date;
                mySqlCommand.Parameters["@DelegacionActivaInd"].Value = 0;

                mySqlCommand.CommandText =
                    "select * from seDelegacionHistoria with (nolock) " +
                    "where DelegacionActivaInd = @DelegacionActivaInd " +
                    "	and FechaInicialAsig <> FechaFinalAsig " +
                    "	and FechaInicialAsig <= @Fecha " +
                    "	and FechaFinalAsig > @Fecha";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_seDelegacionHistoria dto = new DTO_seDelegacionHistoria(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_GetNewDelegaciones");
                throw exception;
            }
        }

        /// <summary>
        /// Asigna delegaciones
        /// </summary>
        /// <param name="del">Nueva delegacion</param>
        /// <param name="emp">Empresa de trabajo</param>
        public void DAL_seDelegacionHistoria_Activar(DTO_seDelegacionHistoria del, DTO_glEmpresa emp)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@UsuarioDelegado", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDelegaINI", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaDelegaFIN", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = emp.ID.Value;
                mySqlCommand.Parameters["@DelegacionActivaInd"].Value = 1;
                mySqlCommand.Parameters["@EmpresaGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadPermiso, emp, egCtrl);
                mySqlCommand.Parameters["@UsuarioDelegado"].Value = del.UsuarioRemplazo.Value;
                mySqlCommand.Parameters["@FechaDelegaINI"].Value = del.FechaInicialAsig.Value.Value;
                mySqlCommand.Parameters["@FechaDelegaFIN"].Value = del.FechaFinalAsig.Value.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = del.UsuarioID.Value;

                #region Actualiza el usuario
                mySqlCommand.CommandText =
                    "update seUsuario " +
                    "set UsuarioDelegado=@UsuarioDelegado, FechaDelegaINI=@FechaDelegaINI, FechaDelegaFIN=@FechaDelegaFIN, DelegacionActivaInd=@DelegacionActivaInd " +
                    "where UsuarioID=@UsuarioID";

                mySqlCommand.ExecuteNonQuery();
                #endregion
                #region Actualiza la tabla de delegacion historia
                mySqlCommand.CommandText =
                    "update seDelegacionHistoria " +
                    "set DelegacionActivaInd = @DelegacionActivaInd " +
                    "where EmpresaID = @EmpresaID and UsuarioID=@UsuarioID and FechaInicialAsig = @FechaDelegaINI and FechaFinalAsig = @FechaDelegaFIN";

                mySqlCommand.ExecuteNonQuery();
                #endregion
                #region Crea los nuevos permisos
                mySqlCommand.CommandText =
                    "insert into glActividadPermiso (EmpresaGrupoID, ActividadFlujoID, AreaFuncionalID, UsuarioID, " +
                    "	AlarmaUsuario1, AlarmaUsuario2, AlarmaUsuario3, eg_glAreaFuncional, eg_glActividadFlujo, ActivoInd, CtrlVersion, DelegadoInd, Nivel) " +
                    "select EmpresaGrupoID, ActividadFlujoID, AreaFuncionalID, @UsuarioDelegado as UsuarioID, AlarmaUsuario1, AlarmaUsuario2, AlarmaUsuario3, " +
                    "	eg_glAreaFuncional, eg_glActividadFlujo, ActivoInd, CtrlVersion, 1 as DelegadoInd, Nivel " +
                    "from glActividadPermiso t1 with (nolock) " +
                    "where EmpresaGrupoID = @EmpresaGrupo and UsuarioID=@UsuarioID " +
                    "	and not exists " +
                    "	( " +
                    "		select ActividadFlujoID, AreaFuncionalID, UsuarioID, DelegadoInd " +
                    "		from glActividadPermiso t2 with (nolock) " +
                    "		where EmpresaGrupoID = @EmpresaGrupo and UsuarioID=@UsuarioDelegado	" +
                    "			and t1.ActividadFlujoID=t2.ActividadFlujoID and t1.AreaFuncionalID=t2.AreaFuncionalID " +
                    "	) ";

                mySqlCommand.ExecuteNonQuery();
                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_Activar");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene al lista de delegaciones que se deben quitar
        /// </summary>
        /// <returns>Retorna la lista de nuevas delegaciones</returns>
        public List<DTO_seDelegacionHistoria> DAL_seDelegacionHistoria_GetOldDelegaciones()
        {
            try
            {
                //Lista de nuevos registros
                List<DTO_seDelegacionHistoria> result = new List<DTO_seDelegacionHistoria>();
                Dictionary<string, DTO_glEmpresa> cacheEmpresas = new Dictionary<string, DTO_glEmpresa>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.Bit);

                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now.Date;
                mySqlCommand.Parameters["@DelegacionActivaInd"].Value = 1;

                mySqlCommand.CommandText =
                    "select * from seDelegacionHistoria with (nolock) " +
                    "where DelegacionActivaInd = @DelegacionActivaInd " +
                    "	and FechaFinalAsig <= @Fecha";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_seDelegacionHistoria dto = new DTO_seDelegacionHistoria(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_GetOldDelegaciones");
                throw exception;
            }
        }

        /// <summary>
        /// Asigna delegaciones
        /// </summary>
        /// <param name="del">Nueva delegacion</param>
        /// <param name="emp">Empresa de trabajo</param>
        public void DAL_seDelegacionHistoria_Desactivar(DTO_seDelegacionHistoria del, DTO_glEmpresa emp)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DelegacionActivaInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@UsuarioDelegado", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDelegaINI", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaDelegaFIN", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = emp.ID.Value;
                mySqlCommand.Parameters["@EmpresaGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadPermiso, emp, egCtrl);
                mySqlCommand.Parameters["@DelegacionActivaInd"].Value = 0;
                mySqlCommand.Parameters["@UsuarioDelegado"].Value = DBNull.Value;
                mySqlCommand.Parameters["@FechaDelegaINI"].Value = DBNull.Value;
                mySqlCommand.Parameters["@FechaDelegaFIN"].Value = DBNull.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = del.UsuarioID.Value;

                #region Actualiza el usuario
                mySqlCommand.CommandText =
                    "update seUsuario " +
                    "set UsuarioDelegado=@UsuarioDelegado, FechaDelegaINI=@FechaDelegaINI, FechaDelegaFIN=@FechaDelegaFIN, DelegacionActivaInd=@DelegacionActivaInd " +
                    "where UsuarioID=@UsuarioID";

                mySqlCommand.ExecuteNonQuery();
                #endregion
                #region Actualiza la tabla de delegacion historia

                mySqlCommand.Parameters["@UsuarioDelegado"].Value = del.UsuarioRemplazo.Value;
                mySqlCommand.Parameters["@FechaDelegaINI"].Value = del.FechaInicialAsig.Value.Value;
                mySqlCommand.Parameters["@FechaDelegaFIN"].Value = del.FechaFinalAsig.Value.Value;

                mySqlCommand.CommandText =
                    "update seDelegacionHistoria " +
                    "set DelegacionActivaInd = @DelegacionActivaInd " +
                    "where EmpresaID = @EmpresaID and UsuarioID=@UsuarioID and FechaInicialAsig = @FechaDelegaINI and FechaFinalAsig = @FechaDelegaFIN";

                mySqlCommand.ExecuteNonQuery();
                #endregion
                #region Elimina los permisos de los delegados
                mySqlCommand.CommandText =
                    "delete t1 " +
                    "from glActividadPermiso as t1 " +
                    "where UsuarioID = @UsuarioDelegado and DelegadoInd <> @DelegacionActivaInd " +
                    "	and not exists " +
                    "	( " +
                    "		select perm.EmpresaGrupoID, perm.ActividadFlujoID, perm.AreaFuncionalID, perm.UsuarioID " +
                    "		from seUsuario usr with(nolock) " +
                    "			right join glActividadPermiso perm with(nolock) on usr.UsuarioID = perm.UsuarioID and perm.DelegadoInd = @DelegacionActivaInd  " +
                    "		where usr.DelegacionActivaInd = 1 and usr.UsuarioID <> @UsuarioID " +
                    "			and t1.ActividadFlujoID = perm.ActividadFlujoID and t1.AreaFuncionalID = perm.AreaFuncionalID " +
                    "	)";

                    mySqlCommand.ExecuteNonQuery();
                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seDelegacionHistoria_Activar");
                throw exception;
            }
        }


        #endregion
    }
}
