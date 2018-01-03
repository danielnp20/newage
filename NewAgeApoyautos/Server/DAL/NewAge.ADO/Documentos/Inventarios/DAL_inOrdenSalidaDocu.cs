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
    /// DAL_inOrdenSalidaDocu
    /// </summary>
    public class DAL_inOrdenSalidaDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inOrdenSalidaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Adiciona en tabla inImportacionDocu
        /// </summary>
        /// <param name="ordenDocu">items a agregar a inImportacionDocu</param>
        /// <returns>Numero Doc</returns>
        public void DAL_inOrdenSalidaDocu_Add(DTO_inOrdenSalidaDocu ordenDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inOrdenSalidaDocu " +
                                            "(NumeroDoc " +
                                            ",EmpresaID " +
                                            ",DocSalidaINV " +
                                            ",BodegaID " +
                                            ",eg_inBodega) " +
                                            "VALUES" +
                                            "(@NumeroDoc " +
                                            ",@EmpresaID " +
                                            ",@DocSalidaINV " +
                                            ",@BodegaID " +
                                            ",@eg_inBodega) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSalidaINV", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = ordenDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocSalidaINV"].Value = ordenDocu.DocSalidaINV.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = ordenDocu.BodegaID.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="fisicoInventario"></param>
        /// <returns>una liquidacion Docu</returns>
        public DTO_inOrdenSalidaDocu DAL_inOrdenSalidaDocu_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from inOrdenSalidaDocu with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                DTO_inOrdenSalidaDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_inOrdenSalidaDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDocu_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta una orden Header segun un filtro de parametros
        /// </summary>
        /// <param name="header">Filtro de parametros</param>
        /// <returns>Dto de orden Header</returns>
        public List<DTO_inOrdenSalidaDocu> DAL_inOrdenSalidaDocu_GetByParameter(DTO_inOrdenSalidaDocu header)
        {
            try
            {
                List<DTO_inOrdenSalidaDocu> result = new List<DTO_inOrdenSalidaDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filtroInd = false;

                query = "select * from inOrdenSalidaDocu with(nolock) " +
                                           "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(header.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.BodegaID.Value.ToString()))
                {
                    query += "and BodegaID = @BodegaID ";
                    mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                    mySqlCommand.Parameters["@BodegaID"].Value = header.BodegaID.Value;
                    filtroInd = true;
                }
                if (!string.IsNullOrEmpty(header.DocSalidaINV.Value.ToString()))
                {
                    query += "and DocSalidaINV = @DocSalidaINV ";
                    mySqlCommand.Parameters.Add("@DocSalidaINV", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocSalidaINV"].Value = header.DocSalidaINV.Value;
                    filtroInd = true;
                }
                mySqlCommand.CommandText = query;

                if (!filtroInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_inOrdenSalidaDocu fisico = new DTO_inOrdenSalidaDocu(dr);
                    result.Add(fisico);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDocu_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar inOrdenSalidaDocu
        /// </summary>
        /// <param name="mvtoHeader">orden</param>
        public void DAL_inOrdenSalidaDocu_Upd(DTO_inOrdenSalidaDocu ordenDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla inOrdenDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE inOrdenSalidaDocu " +
                                           "    SET  BodegaID  = @BodegaID,  " +
                                           "    DocSalidaINV  = @DocSalidaINV  " +
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocSalidaINV", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_inBodega", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = ordenDocu.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = ordenDocu.EmpresaID.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = ordenDocu.BodegaID.Value;
                mySqlCommand.Parameters["@DocSalidaINV"].Value = ordenDocu.DocSalidaINV.Value;
                mySqlCommand.Parameters["@eg_inBodega"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inBodega, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inOrdenSalidaDocu_Upd");
                throw exception;
            }

        }

        #endregion
    }
}
