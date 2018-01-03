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
    public class DAL_pyProyectoIngDetalleDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoIngDetalleDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        ///  Adicona Solicitud servicio detallado
        /// </summary>
        /// <param name="dto">solicitud</param>
        public int DAL_pyProyectoIngDetalleDeta_Add(DTO_pyProyectoIngDetalleDeta dto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query

                mySqlCommandSel.CommandText = "   INSERT INTO pyProyectoIngDetalleDeta  " +
                                              "    ([NumeroDoc] " +
                                              "    ,[LocacionID] " +
                                              "    ,[ConsecTarea] " +
                                              "    ,[RecursoID] " +
                                              "    ,[Cantidad] " +
                                              "    ,[eg_pyRecurso]) " +
                                         "   VALUES " +
                                              "    (@NumeroDoc " +
                                              "    ,@LocacionID " +
                                              "    ,@ConsecTarea " +
                                              "    ,@RecursoID " +
                                              "    ,@Cantidad " +
                                              "    ,@eg_pyRecurso) " +
                                              "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LocacionID", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_pyRecurso", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = dto.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@LocacionID"].Value = dto.LocacionID.Value;
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = dto.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = dto.RecursoID.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = dto.Cantidad.Value;
                mySqlCommandSel.Parameters["@eg_pyRecurso"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyRecurso, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_Add");
                throw exception;
                return 0;
            }
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="dto">documento</param>
        /// <returns></returns>
        public void DAL_pyProyectoIngDetalleDeta_Upd(DTO_pyProyectoIngDetalleDeta dto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyProyectoIngDetalleDeta SET					" +
                                                "   LocacionID			=	@LocacionID				" +
                                                "  , ConsecTarea			=	@ConsecTarea				" +
                                                "  ,RecursoID		=	@RecursoID			" +
                                                "  ,Cantidad		=	@Cantidad			" +
                                                "  ,eg_pyRecurso		=	@eg_pyRecurso			" +
                                                "   WHERE  Consecutivo	=	@Consecutivo				";

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LocacionID", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_pyRecurso", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = dto.Consecutivo.Value;
                mySqlCommandSel.Parameters["@LocacionID"].Value = dto.LocacionID.Value;
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = dto.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = dto.RecursoID.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = dto.Cantidad.Value;
                mySqlCommandSel.Parameters["@eg_pyRecurso"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyRecurso, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoIngDetalleDeta_DeleteByLocacion(string locacionID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoIngDetalleDeta " +
                                            "  where LocacionID = @LocacionID";

                mySqlCommandSel.Parameters.Add("@LocacionID", SqlDbType.Char,15);
                mySqlCommandSel.Parameters["@LocacionID"].Value = locacionID;
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_DeleteByLocacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_pyProyectoIngDetalleDeta> DAL_pyProyectoIngDetalleDeta_GetByParameter(int numeroDoc, int? consecutivo, string locacion, int? consecTarea, string recursoID)
        {
            try
            {
                List<DTO_pyProyectoIngDetalleDeta> result = new List<DTO_pyProyectoIngDetalleDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;

                query = "select item.*,tar.TareaID,tar.TareaCliente,tar.Descriptivo as TareaDesc,rec.Descriptivo as RecursoDesc " +
                        "from pyProyectoIngDetalleDeta item with(nolock)  " +
                       " inner join pyProyectoTarea tar on tar.Consecutivo = item.ConsecTarea " +
                       " inner join pyProyectoDeta det on det.ConsecTarea = tar.Consecutivo " +
                       " inner join pyRecurso rec on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso " +
                       " where  item.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                if (consecutivo != null)
                {
                    query += "and item.Consecutivo = @Consecutivo ";
                    mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                    mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;
                }
                if (!string.IsNullOrEmpty(locacion))
                {
                    query += "and item.LocacionID = @LocacionID ";
                    mySqlCommand.Parameters.Add("@LocacionID", SqlDbType.Char, 15);
                    mySqlCommand.Parameters["@LocacionID"].Value = locacion;
                }
                if (consecTarea != null)
                {
                    query += "and item.ConsecTarea = @ConsecTarea ";
                    mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsecTarea"].Value = consecTarea;
                }
                if (!string.IsNullOrEmpty(recursoID))
                {
                    query += "and item.RecursoID = @RecursoID ";
                    mySqlCommand.Parameters.Add("@RecursoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@RecursoID"].Value = recursoID;
                }
                    mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_pyProyectoIngDetalleDeta ctrl = new DTO_pyProyectoIngDetalleDeta(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoIngDetalleDeta DAL_pyProyectoIngDetalleDeta_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyProyectoIngDetalleDeta result = new DTO_pyProyectoIngDetalleDeta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select mvto.* from pyProyectoIngDetalleDeta mvto with(nolock)  " +
                        "inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                        "inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                        "inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                        "where mvto.Consecutivo =  @Consecutivo ";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    result = new DTO_pyProyectoIngDetalleDeta(dr);
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoIngDetalleDeta DAL_pyProyectoIngDetalleDeta_GetByRecurso(int? numDoc, int? consecTarea, string locacionID, string recursoID)
        {
            try
            {
                DTO_pyProyectoIngDetalleDeta result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select item.*,tar.TareaID,tar.TareaCliente,tar.Descriptivo as TareaDesc,rec.Descriptivo as RecursoDesc from pyProyectoIngDetalleDeta item with(nolock)  " +
                                           " inner join pyProyectoTarea tar on tar.Consecutivo = item.ConsecTarea " +
                                           " inner join pyProyectoDeta det on det.ConsecTarea = tar.Consecutivo " +
                                           " inner join pyRecurso rec on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso " +
                                            "where item.NumeroDoc =  @NumeroDoc and  LocacionID = @LocacionID and item.ConsecTarea = @consecTarea and item.RecursoID = @RecursoID ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numDoc;
                mySqlCommand.Parameters.Add("@LocacionID", SqlDbType.VarChar, 15);
                mySqlCommand.Parameters["@LocacionID"].Value = locacionID;
                mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecTarea"].Value = consecTarea;
                mySqlCommand.Parameters.Add("@RecursoID", SqlDbType.VarChar, UDT_CodigoGrl.MaxLength);
                mySqlCommand.Parameters["@RecursoID"].Value = recursoID;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    result = new DTO_pyProyectoIngDetalleDeta(dr);
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_GetByRecurso");
                throw exception;
            }
        }

        /// <summary>
        /// Trae las locaciones que ya no existen para eliminar
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<string> DAL_pyProyectoIngDetalleDeta_GetByLocacionInvalid(int numeroDoc)
        {
            try
            {
                List<string> result = new List<string>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = " Select det.LocacionID  from pyProyectoIngDetalleDeta det with(nolock)  " +
		                                   "    left join pyProyectoIngDetalleTarea tar on tar.LocacionID = det.LocacionID  " +
                                           "  where  det.NumeroDoc = @NumeroDoc and tar.NumeroDoc is null " + 
	                                       "  group by  det.LocacionID  ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;              

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    result.Add(dr["LocacionID"].ToString());
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoIngDetalleDeta_GetByLocacionInvalid");
                throw exception;
            }
        }

    }
}
