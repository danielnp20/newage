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
    public class DAL_ccSolicitudAnexo : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudAnexo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudAnexo
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudAnexo_Add(DTO_ccSolicitudAnexo footer)
        {
            try
            {
                List<DTO_ccSolicitudAnexo> result = new List<DTO_ccSolicitudAnexo>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region CommandText
                    mySqlCommandSel.CommandText = "INSERT INTO ccSolicitudAnexo   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[DocumListaID]   " +
                                                  "  ,[Descripcion]   " +
                                                  "  ,[IncluidoInd]   " +
                                                  "  ,[eg_ccAnexosLista])   " +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@DocumListaID    " +
                                                  "  ,@Descripcion    " +
                                                  "  ,@IncluidoInd    " +
                                                  "  ,@eg_ccAnexosLista)";
                #endregion
                #region Creacion Parametros
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@DocumListaID", SqlDbType.Char, UDT_DocumListaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.VarChar, UDT_DescripTExt.MaxLength);
                    mySqlCommandSel.Parameters.Add("@IncluidoInd", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@eg_ccAnexosLista", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value       = footer.NumeroDoc.Value;
                    mySqlCommandSel.Parameters["@DocumListaID"].Value    = footer.DocumListaID.Value;
                    mySqlCommandSel.Parameters["@Descripcion"].Value = footer.Descripcion.Value;
                    mySqlCommandSel.Parameters["@IncluidoInd"].Value     = footer.IncluidoInd.Value;
                    mySqlCommandSel.Parameters["@eg_ccAnexosLista"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAnexosLista, this.Empresa, egCtrl); ;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudAnexo_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudAnexo
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudAnexo</returns>
        public List<DTO_ccSolicitudAnexo> DAL_ccSolicitudAnexo_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudAnexo> result = new List<DTO_ccSolicitudAnexo>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT sa.*,anexo.Descriptivo " +
                                           " FROM ccSolicitudAnexo sa with(nolock)  " +
                                           "    inner join ccAnexosLista anexo  with(nolock) on sa.DocumListaID = anexo.DocumListaID and sa.eg_ccAnexosLista = anexo.EmpresaGrupoID " +
                                           "WHERE sa.NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccSolicitudAnexo anexo = new DTO_ccSolicitudAnexo(dr, false);
                    result.Add(anexo);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudAnexo_GetByNumeroDoc");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudAnexo_Update(DTO_ccSolicitudAnexo docSolicitud)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocumListaID", SqlDbType.Char, UDT_DocumListaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncluidoInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = docSolicitud.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocumListaID"].Value = docSolicitud.DocumListaID.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = docSolicitud.Descripcion.Value;
                mySqlCommandSel.Parameters["@IncluidoInd"].Value = docSolicitud.IncluidoInd.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = docSolicitud.Consecutivo.Value;
                #endregion

                mySqlCommandSel.CommandText =
                                    "UPDATE ccSolicitudAnexo SET" +
                                       "  DocumListaID = @DocumListaID  " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudAnexo_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los anexos asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudAnexo_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudAnexo WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudAnexo_Delete");
                throw exception;
            }
        }

    }
}
