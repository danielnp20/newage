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
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccCobranzaTareas : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCobranzaTareas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public DTO_ccCobranzaTareas DAL_ccCobranzaTareas_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccCobranzaTareas result = new DTO_ccCobranzaTareas();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "SELECT * FROM ccCobranzaTareas with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccCobranzaTareas(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCobranzaTareas_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCobranzaTareas_Add(DTO_ccCobranzaTareas CobranzaTareas)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccCobranzaTareas   " +
                                               "    ([EmpresaID] " +
                                               "    ,[ClienteID] " +
                                               "    ,[Tarea] " +
                                               "    ,[Fecha] " +
                                               "    ,[NumeroDoc] " + 
                                               "    ,[Observaciones] " +
                                               "    ,[ProcesadoIND] " +   
                                               "    ,[eg_ccCliente] " +
                                               "    ,[eg_glActividadFlujo] ) " +
                                               "  VALUES " +
                                               "  (@EmpresaID " +
                                               "  ,@ClienteID " +
                                               "  ,@Tarea " +
                                               "  ,@Fecha " +
                                               "  ,@NumeroDoc " + 
                                               "  ,@Observaciones " +
                                               "  ,@ProcesadoIND " +   
                                               "  ,@eg_ccCliente " +
                                               "  ,@eg_glActividadFlujo) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Tarea", SqlDbType.Char, UDT_TareaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char,UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProcesadoIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_ccCliente", SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = CobranzaTareas.ClienteID.Value;
                mySqlCommandSel.Parameters["@Tarea"].Value = CobranzaTareas.Tarea.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = CobranzaTareas.Fecha.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = CobranzaTareas.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = CobranzaTareas.Observaciones.Value;
                mySqlCommandSel.Parameters["@ProcesadoIND"].Value = CobranzaTareas.ProcesadoIND.Value;
                mySqlCommandSel.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCobranzaTareas_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccCobranzaTareas_Update(DTO_ccCobranzaTareas CobranzaTareas)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char,UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Tarea", SqlDbType.Char, UDT_TareaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProcesadoIND", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                #endregion
                #region Asigna los valores

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = CobranzaTareas.ClienteID.Value;
                mySqlCommandSel.Parameters["@Tarea"].Value = CobranzaTareas.Tarea.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = CobranzaTareas.Fecha.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = CobranzaTareas.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = CobranzaTareas.Observaciones.Value;
                mySqlCommandSel.Parameters["@ProcesadoIND"].Value = CobranzaTareas.ProcesadoIND.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccCobranzaTareas SET" +
                    "  EmpresaID = @EmpresaID " +
                    "  ,ClienteID = @ClienteID " +                    
                    "  ,Tarea = @Tarea " +                    
                    "  ,Fecha = @Fecha " +        
                    "  ,NumeroDoc = @NumeroDoc " +
                    "  ,Observaciones = @Observaciones " +
                    "  ,ProcesadoIND = @ProcesadoIND " +
                    " WHERE  NumeroDoc = @NumeroDoc ";
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
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCobranzaTareas_Update");
                throw exception;
            }
        }

        #endregion

    }

}
