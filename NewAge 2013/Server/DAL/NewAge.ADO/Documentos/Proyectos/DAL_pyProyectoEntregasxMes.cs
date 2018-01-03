using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_pyProyectoEntregasxMes : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoEntregasxMes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        ///  Adicona Solicitud servicio detallado
        /// </summary>
        /// <param name="dto">solicitud</param>
        public int DAL_pyProyectoEntregasxMes_Add(DTO_pyProyectoEntregasxMes dto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query

                mySqlCommandSel.CommandText = "   INSERT INTO pyProyectoEntregasxMes  " +
                                              "    ([NumeroDoc] " +
                                              "    ,[ConsecTarea] " +
                                              "    ,[Fecha] " +
                                              "    ,[Cantidad]) " +
                                         "   VALUES " +
                                              "    (@NumeroDoc " +
                                              "    ,@ConsecTarea " +
                                              "    ,@Fecha " +
                                              "    ,@Cantidad) " +
                                              "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);

                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = dto.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = dto.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = dto.Fecha.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = dto.Cantidad.Value;

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                int consecutivo = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                dto.Consecutivo.Value = consecutivo;
                return consecutivo;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoEntregasxMes_Add");
                throw exception;
                return 0;
            }
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="dto">documento</param>
        /// <returns></returns>
        public void DAL_pyProyectoEntregasxMes_Upd(DTO_pyProyectoEntregasxMes dto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyProyectoEntregasxMes SET					" +
                                                "   ConsecTarea			=	@ConsecTarea				" +
                                                "  ,Fecha		=	@Fecha			" +
                                                "  ,Cantidad		=	@Cantidad			" +
                                                "   WHERE  Consecutivo	=	@Consecutivo				";

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);

                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = dto.Consecutivo.Value;
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = dto.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = dto.Fecha.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = dto.Cantidad.Value;

                #endregion
                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoEntregasxMes_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoEntregasxMes_Delete(int? numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoEntregasxMes " +
                                            "  where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoEntregasxMes_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_pyProyectoEntregasxMes> DAL_pyProyectoEntregasxMes_GetByParameter(int numeroDoc, int? consecutivo, int? consecTarea, DateTime? fecha)
        {
            try
            {
                List<DTO_pyProyectoEntregasxMes> result = new List<DTO_pyProyectoEntregasxMes>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = " Select item.*, tar.TareaID,tar.TareaCliente,tar.Descriptivo as TareaDesc " +
                        " From pyProyectoEntregasxMes item with(nolock)  " +
                        " inner join pyProyectoTarea tar on tar.Consecutivo = item.ConsecTarea " +
                        " where  item.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                if (consecutivo != null)
                {
                    query += "and item.Consecutivo = @Consecutivo ";
                    mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                    mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;
                }
                if (consecTarea != null)
                {
                    query += "and item.ConsecTarea = @ConsecTarea ";
                    mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsecTarea"].Value = consecTarea;
                }
                if (fecha != null)
                {
                    query += "and item.Fecha = @Fecha ";
                    mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Fecha"].Value = fecha;
                }

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_pyProyectoEntregasxMes ctrl = new DTO_pyProyectoEntregasxMes(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoEntregasxMes_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoEntregasxMes DAL_pyProyectoEntregasxMes_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyProyectoEntregasxMes result = new DTO_pyProyectoEntregasxMes();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select mvto.* from pyProyectoEntregasxMes mvto with(nolock)  " +
                        "inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                        "inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                        "inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                        "where mvto.Consecutivo =  @Consecutivo ";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    result = new DTO_pyProyectoEntregasxMes(dr);
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoEntregasxMes_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoEntregasxMes DAL_pyProyectoEntregasxMes_GetByFecha(int? numDoc, int? consecTarea, DateTime? fechaEntrega)
        {
            try
            {
                DTO_pyProyectoEntregasxMes result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select det.*, tar.TareaID,tar.TareaCliente,tar.Descriptivo as TareaDesc from pyProyectoEntregasxMes det with(nolock)  " +
                                            " inner join pyProyectoTarea tar on tar.Consecutivo = det.ConsecTarea " +
                                            "where det.NumeroDoc =  @NumeroDoc and  ConsecTarea = @ConsecTarea  and Fecha = @Fecha ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numDoc;
                mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecTarea"].Value = consecTarea;
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@Fecha"].Value = fechaEntrega;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    result = new DTO_pyProyectoEntregasxMes(dr);
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoEntregasxMes_GetByFecha");
                throw exception;
            }
        }
    }
}
