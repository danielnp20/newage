using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_inOrdenSalidaDeta
    /// </summary>
    public class DAL_inOrdenSalidaDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inOrdenSalidaDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Adiciona en tabla inOrdenSalidaDeta
        /// </summary>
        /// <param name="salida">items a agregar a inOrdenSalidaDeta</param>
        /// <returns>Numero Doc</returns>
        public void DAL_inOrdenSalidaDeta_Add(DTO_inOrdenSalidaDeta salida)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inOrdenSalidaDeta " +
                                            "(NumeroDoc " +
                                            ",ConsProyectoMvto " +
                                            ",inReferenciaID " +
                                            ",CantidadAPR " +
                                            ",eg_inReferencia) " +
                                            "VALUES" +
                                            "(@NumeroDoc " +
                                            ",@ConsProyectoMvto " +
                                            ",@inReferenciaID " +
                                            ",@CantidadAPR " +
                                            ",@eg_inReferencia) " +
                                            " SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsProyectoMvto", SqlDbType.Int); 
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char,UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadAPR", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);             
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = salida.NumeroDoc.Value;
                mySqlCommand.Parameters["@ConsProyectoMvto"].Value = salida.ConsProyectoMvto.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = salida.inReferenciaID.Value;
                mySqlCommand.Parameters["@CantidadAPR"].Value = salida.CantidadAPR.Value;
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
                salida.Consecutivo.Value = Convert.ToInt32(mySqlCommand.Parameters["@Consecutivo"].Value);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDeta_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Consulta una Orden Salida segun un filtro de parametros
        /// </summary>
        /// <param name="numeroDoc">Numero Doc</param>
        /// <returns>Dto de importacion </returns>
        public List<DTO_inOrdenSalidaDeta> DAL_inOrdenSalidaDeta_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                List<DTO_inOrdenSalidaDeta> result = new List<DTO_inOrdenSalidaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "select * from inOrdenSalidaDeta with(nolock) " +
                                           "where NumeroDoc = @NumeroDoc "; 

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_inOrdenSalidaDeta fisico = new DTO_inOrdenSalidaDeta(dr);
                    result.Add(fisico);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDeta_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta una Orden Salida segun un filtro de parametros
        /// </summary>
        /// <param name="numeroDoc">Numero Doc</param>
        /// <returns>Dto de importacion </returns>
        public DTO_inOrdenSalidaDeta DAL_inOrdenSalidaDeta_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_inOrdenSalidaDeta result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                mySqlCommand.CommandText = "Select * from inOrdenSalidaDeta with(nolock) " +
                                           "Where Consecutivo = @Consecutivo ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_inOrdenSalidaDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDeta_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta una Orden Salida segun un filtro de parametros
        /// </summary>
        /// <param name="numeroDoc">Numero Doc</param>
        /// <returns>Dto de importacion </returns>
        public List<DTO_inOrdenSalidaDeta> DAL_inOrdenSalidaDeta_GetByOrdenServicio(int ConsOrdServicio)
        {
            try
            {
                List<DTO_inOrdenSalidaDeta> result = new List<DTO_inOrdenSalidaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ConsOrdServicio", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsOrdServicio"].Value = ConsOrdServicio;

                mySqlCommand.CommandText = "select * from inOrdenSalidaDeta with(nolock) " +
                                           "where ConsOrdServicio = @ConsOrdServicio "; 

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_inOrdenSalidaDeta fisico = new DTO_inOrdenSalidaDeta(dr);
                    result.Add(fisico);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDeta_GetByOrdenServicio");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar inOrdenSalidaDeta
        /// </summary>
        /// <param name="salida">orden salida</param>
        public void DAL_inOrdenSalidaDeta_Upd(DTO_inOrdenSalidaDeta salida)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inOrdenSalidaDeta
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inOrdenSalidaDeta SET    " +
                                           "    inReferenciaID  = @inReferenciaID  " +
                                           "    ,CantidadAPR  = @CantidadAPR  " +
                                           "    ,ConsProyectoMvto  = @ConsProyectoMvto  " +
                                           "    ,eg_inReferencia  = @eg_inReferencia " +
                                           "    WHERE Consecutivo = @Consecutivo";                
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@ConsProyectoMvto", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadAPR", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@ConsProyectoMvto"].Value = salida.ConsProyectoMvto.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = salida.inReferenciaID.Value;
                mySqlCommand.Parameters["@CantidadAPR"].Value = salida.CantidadAPR.Value;
                mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@Consecutivo"].Value = salida.Consecutivo.Value;
                #endregion
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDeta_Upd");
                throw exception;
            }

        }

        #endregion
    }
}
