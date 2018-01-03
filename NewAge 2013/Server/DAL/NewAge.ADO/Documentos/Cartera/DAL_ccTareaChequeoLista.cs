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
    public class DAL_ccTareaChequeoLista : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccTareaChequeoLista(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DAL_ccTareaChequeoLista
        /// </summary>
        /// <param name="NumeroDoc"></param>
        /// <returns>retorna una lista de DAL_ccTareaChequeoLista</returns>
        public List<DTO_ccTareaChequeoLista> DAL_ccTareaChequeoLista_GetNumeroDoc(int NumeroDoc) 
        {
            try
            {
                List<DTO_ccTareaChequeoLista> result = new List<DTO_ccTareaChequeoLista>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccTareaChequeoLista  with(nolock)   " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccTareaChequeoLista anexo = new DTO_ccTareaChequeoLista(dr, false);
                    result.Add(anexo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccTareaChequeoLista_GetNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudAnexo
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccTareaChequeoLista_Add(DTO_ccTareaChequeoLista footer)
        {
            try
            {
                List<DTO_ccTareaChequeoLista> result = new List<DTO_ccTareaChequeoLista>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText = "INSERT INTO ccTareaChequeoLista   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[TareaID]   " +
                                                  "  ,[Descripcion]   " +
                                                  "  ,[IncluidoInd]   " +
                                                  "  ,[eg_ccChequeoLista])   " +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@TareaID    " +
                                                  "  ,@Descripcion    " +
                                                  "  ,@IncluidoInd   " +
                                                  "  ,@eg_ccChequeoLista)    ";
                                                  
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TareaID", SqlDbType.Char, UDT_TareaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@IncluidoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_ccChequeoLista", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value   = footer.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TareaID"].Value = footer.TareaID.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = footer.Descripcion.Value;
                mySqlCommandSel.Parameters["@IncluidoInd"].Value = footer.IncluidoInd.Value;
                mySqlCommandSel.Parameters["@eg_ccChequeoLista"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccChequeoLista, this.Empresa, egCtrl); ;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccTareaChequeoLista_Add");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docTarea"></param>
        /// <returns></returns>
        public bool DAL_ccTareaChequeoLista_Update(DTO_ccTareaChequeoLista docTarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TareaID", SqlDbType.Char, UDT_TareaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@IncluidoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value   = docTarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TareaID"].Value = docTarea.TareaID.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = docTarea.Descripcion.Value;
                mySqlCommandSel.Parameters["@IncluidoInd"].Value = docTarea.IncluidoInd.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = docTarea.Consecutivo.Value;
                #endregion

                mySqlCommandSel.CommandText =
                                    "UPDATE ccTareaChequeoLista SET" +
                                       "  TareaID = @TareaID  " +
                                       "  ,Descripcion = @Descripcion " +
                                       "  ,IncluidoInd = @IncluidoInd " +
                                       " WHERE  Consecutivo = @Consecutivo";

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccTareaChequeoLista_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los anexos asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccTareaChequeoLista_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccTareaChequeoLista WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccTareaChequeoLista_Delete");
                throw exception;
            }
        }
        #endregion
    }
}
