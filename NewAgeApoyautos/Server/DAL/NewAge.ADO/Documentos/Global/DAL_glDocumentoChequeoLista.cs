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
    public class DAL_glDocumentoChequeoLista : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glDocumentoChequeoLista(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DAL_glDocumentoChequeoLista
        /// </summary>
        /// <param name="NumeroDoc"></param>
        /// <returns>retorna una lista de DAL_glDocumentoChequeoLista</returns>
        public List<DTO_glDocumentoChequeoLista> DAL_glDocumentoChequeoLista_GetNumeroDoc(int NumeroDoc) 
        {
            try
            {
                List<DTO_glDocumentoChequeoLista> result = new List<DTO_glDocumentoChequeoLista>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM glDocumentoChequeoLista  with(nolock)   " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_glDocumentoChequeoLista anexo = new DTO_glDocumentoChequeoLista(dr);
                    result.Add(anexo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoChequeoLista_GetNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudAnexo
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_glDocumentoChequeoLista_Add(DTO_glDocumentoChequeoLista footer)
        {
            try
            {
                List<DTO_glDocumentoChequeoLista> result = new List<DTO_glDocumentoChequeoLista>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText = "INSERT INTO glDocumentoChequeoLista   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[ActividadFlujoID]   " +
                                                  "  ,[TerceroID]   " +
                                                  "  ,[Descripcion]   " +
                                                  "  ,[IncluidoInd]   " +
                                                  "  ,[Observacion]   " +
                                                  "  ,[eg_glActividadFlujo])   " +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@ActividadFlujoID    " +
                                                  "  ,@TerceroID    " +
                                                  "  ,@Descripcion    " +
                                                  "  ,@IncluidoInd   " +
                                                  "  ,@Observacion  " +
                                                  "  ,@eg_glActividadFlujo)    ";
                                                  
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@IncluidoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value   = footer.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = footer.ActividadFlujoID.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = footer.TerceroID.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = footer.Descripcion.Value;
                mySqlCommandSel.Parameters["@IncluidoInd"].Value = footer.IncluidoInd.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = footer.Observacion.Value;
                mySqlCommandSel.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl); ;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoChequeoLista_Add");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docTarea"></param>
        /// <returns></returns>
        public bool DAL_glDocumentoChequeoLista_Update(DTO_glDocumentoChequeoLista docTarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@IncluidoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value   = docTarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = docTarea.ActividadFlujoID.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = docTarea.TerceroID.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = docTarea.Descripcion.Value;
                mySqlCommandSel.Parameters["@IncluidoInd"].Value = docTarea.IncluidoInd.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = docTarea.Observacion.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = docTarea.Consecutivo.Value;
                #endregion

                mySqlCommandSel.CommandText =
                                    "UPDATE glDocumentoChequeoLista SET" +
                                       "  ActividadFlujoID = @ActividadFlujoID  " +
                                       "  ,TerceroID = @TerceroID  " +
                                       "  ,Descripcion = @Descripcion " +
                                       "  ,IncluidoInd = @IncluidoInd " +
                                       "  ,Observacion = @Observacion  " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoChequeoLista_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los anexos asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_glDocumentoChequeoLista_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM glDocumentoChequeoLista WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoChequeoLista_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_glDocumentoChequeoLista_Exist(int? numeroDoc , string actFlujo, string terceroID, string desc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "Select count(*) from glDocumentoChequeoLista with(nolock) where NumeroDoc = @NumeroDoc and ActividadFlujoID = @ActividadFlujoID and " +
                                            " TerceroID = @TerceroID and Descripcion = @Descripcion";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char,UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char,UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Descripcion", SqlDbType.Char,50);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujo;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@Descripcion"].Value = desc;


                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoChequeoLista_Exist");
                throw exception;
            }
        }
        #endregion
    }
}
