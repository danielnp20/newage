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
    public class DAL_ccCreditoCompraCartera : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCreditoCompraCartera(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public List<DTO_ccSolicitudCompraCartera> DAL_ccCreditoCompraCartera_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudCompraCartera> result = new List<DTO_ccSolicitudCompraCartera>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT scc.*, fi.Descriptivo " +
                                       "FROM ccCreditoCompraCartera scc with(nolock) " +
                                       "    inner join ccFinanciera fi  with(nolock) on scc.FinancieraID = fi.FinancieraID " +
                                       "WHERE scc.NumeroDoc = @NumeroDoc ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccSolicitudCompraCartera compraCartera = new DTO_ccSolicitudCompraCartera(dr);
                    result.Add(compraCartera);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudComponentes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccCreditoCompraCartera_Add(DTO_ccSolicitudCompraCartera compraCartera)
        {
            try
            {
                List<DTO_ccSolicitudCompraCartera> result = new List<DTO_ccSolicitudCompraCartera>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "INSERT INTO ccCreditoCompraCartera " +
                                              "  ([NumeroDoc]   " +
                                              "  ,[FinancieraID]   " +
                                              "  ,[Documento]   " +
                                              "  ,[DocCompra]   " +
                                              "  ,[VlrCuota]   " +
                                              "  ,[VlrSalgo]   " +
                                              "  ,[DocAnticipo]   " +
                                              "  ,[IndRecibePazySalvo]   " +
                                              "  ,[FechaPazySalvo] " +
                                              "  ,[UsuarioID] " +
                                              "  ,[ExternaInd] " +
                                              "  ,[eg_ccFinanciera])   " +
                                              "VALUES    " +
                                              "  (@NumeroDoc    " +
                                              "  ,@FinancieraID   " +
                                              "  ,@Documento   " +
                                              "  ,@DocCompra   " +
                                              "  ,@VlrCuota   " +
                                              "  ,@VlrSalgo   " +
                                              "  ,@DocAnticipo   " +
                                              "  ,@IndRecibePazySalvo   " +
                                              "  ,@FechaPazySalvo " +
                                              "  ,@UsuarioID " +
                                              "  ,@ExternaInd " +
                                              "  ,@eg_ccFinanciera)   ";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FinancieraID", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndRecibePazySalvo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocAnticipo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaPazySalvo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ExternaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_ccFinanciera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compraCartera.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FinancieraID"].Value = compraCartera.FinancieraID.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = compraCartera.Documento.Value;
                mySqlCommandSel.Parameters["@DocCompra"].Value = compraCartera.DocCompra.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = compraCartera.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrSaldo"].Value = compraCartera.VlrSaldo.Value;
                mySqlCommandSel.Parameters["@DocAnticipo"].Value = compraCartera.DocAnticipo.Value;
                mySqlCommandSel.Parameters["@IndRecibePazySalvo"].Value = compraCartera.IndRecibePazySalvo.Value;
                mySqlCommandSel.Parameters["@FechaPazySalvo"].Value = compraCartera.FechaPazySalvo.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = compraCartera.UsuarioID.Value;
                mySqlCommandSel.Parameters["@ExternaInd"].Value = compraCartera.ExternaInd.Value;
                mySqlCommandSel.Parameters["@eg_ccFinanciera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudComponentes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccCreditoCompraCartera_AddFromccSolicitudCompraCartera(int numDocOld, int numDocNew)
        {
            try
            {
                List<DTO_ccSolicitudCompraCartera> result = new List<DTO_ccSolicitudCompraCartera>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "INSERT INTO ccCreditoCompraCartera " +
                                              "  ([NumeroDoc]   " +
                                              "  ,[FinancieraID]   " +
                                              "  ,[Documento]   " +
                                              "  ,[DocCompra]   " +
                                              "  ,[VlrCuota]   " +
                                              "  ,[VlrSaldo]   " +
                                              "  ,[DocAnticipo]   " +
                                              "  ,[IndRecibePazySalvo]   " +
                                              "  ,[FechaPazySalvo]   " +
                                              "  ,[UsuarioID]   " +
                                              "  ,[ExternaInd]   " +
                                              "  ,[eg_ccFinanciera])   " +
                                              "  SELECT    " +
                                              "   @numDocNew    " +
                                              "  ,FinancieraID   " +
                                              "  ,Documento   " +
                                              "  ,DocCompra   " +
                                              "  ,VlrCuota   " +
                                              "  ,VlrSaldo   " +
                                              "  ,DocAnticipo   " +
                                              "  ,IndRecibePazySalvo   " +
                                              "  ,FechaPazySalvo " +
                                              "  ,UsuarioID  " +
                                              "  ,ExternaInd  " +
                                              "  ,eg_ccFinanciera " +
                                              "  from ccSolicitudCompraCartera " +
                                              "  WHERE numeroDoc = @numDocOld ";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@numDocOld", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@numDocNew", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@numDocOld"].Value = numDocOld;
                mySqlCommandSel.Parameters["@numDocNew"].Value = numDocNew;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudCompraCartera_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudComponentes
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccCreditoCompraCartera_Update(DTO_ccSolicitudCompraCartera compraCartera)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FinancieraID", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndRecibePazySalvo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocAnticipo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaPazySalvo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ExternaInd", SqlDbType.TinyInt);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compraCartera.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FinancieraID"].Value = compraCartera.FinancieraID.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = compraCartera.Documento.Value;
                mySqlCommandSel.Parameters["@DocCompra"].Value = compraCartera.DocCompra.Value;
                mySqlCommandSel.Parameters["@DocAnticipo"].Value = compraCartera.DocAnticipo.Value;
                mySqlCommandSel.Parameters["@IndRecibePazySalvo"].Value = compraCartera.IndRecibePazySalvo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = compraCartera.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrSaldo"].Value = compraCartera.VlrSaldo.Value;
                mySqlCommandSel.Parameters["@FechaPazySalvo"].Value = compraCartera.FechaPazySalvo.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = compraCartera.UsuarioID.Value;
                mySqlCommandSel.Parameters["@ExternaInd"].Value = compraCartera.ExternaInd.Value;
                #endregion
                mySqlCommandSel.CommandText =
                                "UPDATE ccCreditoCompraCartera SET" +
                                " FinancieraID = @FinancieraID  " +
                                " ,Documento = @Documento " +
                                " ,DocCompra = @DocCompra   " +
                                " ,VlrCuota = @VlrCuota   " +
                                " ,VlrSaldo = @VlrSaldo   " +
                                " ,DocAnticipo = @DocAnticipo " +
                                " ,IndRecibePazySalvo = @IndRecibePazySalvo " +
                                " ,FechaPazySalvo = @FechaPazySalvo " +
                                " ,UsuarioID = @UsuarioID " +
                                " ,ExternaInd = @ExternaInd " +
                                " WHERE  NumeroDoc = @NumeroDoc";
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoCompraCartera_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccCreditoCompraCartera_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccCreditoCompraCartera WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoCompraCartera_Delete");
                throw exception;
            }
        }

        #endregion
    }
}
