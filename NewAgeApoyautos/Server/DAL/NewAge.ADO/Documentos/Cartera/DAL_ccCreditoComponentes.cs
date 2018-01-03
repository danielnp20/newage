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
    public class DAL_ccCreditoComponentes : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCreditoComponentes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public List<DTO_ccCreditoComponentes> DAL_ccCreditoComponentes_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccCreditoComponentes> result = new List<DTO_ccCreditoComponentes>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "SELECT sc.*,cc.Descriptivo " +
                                           "FROM ccCreditoComponentes sc with(nolock)" +
                                                "inner join ccCarteraComponente cc  with(nolock) on sc.ComponenteCarteraID = cc.ComponenteCarteraID " +
                                                "   and cc.EmpresaGrupoID = sc.eg_ccCarteraComponente " +
                                           "WHERE sc.NumeroDoc = @numeroDoc ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccCreditoComponentes anexo = new DTO_ccCreditoComponentes(dr);
                    result.Add(anexo);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoComponentes_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public DTO_ccCreditoComponentes DAL_ccCreditoComponentes_GetByComponenteCartera(int NumeroDoc, string componenteCarteraID)
        {
            try
            {
                DTO_ccCreditoComponentes result = new DTO_ccCreditoComponentes();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@componenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.Parameters["@componenteCarteraID"].Value = componenteCarteraID;

                mySqlCommand.CommandText = "SELECT sc.*,cc.Descriptivo " +
                                           "FROM ccCreditoComponentes sc with(nolock)" +
                                                "INNER JOIN ccCarteraComponente cc  with(nolock) ON sc.ComponenteCarteraID = cc.ComponenteCarteraID " +
                                                "AND cc.EmpresaGrupoID = sc.eg_ccCarteraComponente " +
                                           "WHERE sc.NumeroDoc = @numeroDoc AND sc.ComponenteCarteraID = @componenteCarteraID ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoComponentes(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoComponentes_GetByComponenteCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Añade los componentes a la tabla ccCreditoComponente
        /// </summary>
        /// <param name="numDocOld">NumeroDocViejo</param>
        /// <param name="numDocNew">NumeroDocViejo</param>
        public void DAL_ccCarteraComponentes_Add(DTO_ccCreditoComponentes crediComponentes)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
      
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "INSERT INTO ccCreditoComponentes   " +
                                              "  ([NumeroDoc]   " +
                                              "  ,[ComponenteCarteraID]   " +
                                              "  ,[CuotaValor]   " +
                                              "  ,[TotalValor]   " +
                                              "  ,[AbonoValor] " +
                                              "  ,[DescuentoInd] " +
                                              "  ,[CompInvisibleInd] " +
                                              "  ,[PorCapital]   " +
                                              "  ,[eg_ccCarteraComponente] )" +
                                              "   VALUES   " +
                                              "   (@NumeroDoc " +
                                              "  ,@ComponenteCarteraID " +
                                              "  ,@CuotaValor " +
                                              "  ,@TotalValor " +
                                              "  ,@AbonoValor " +
                                              "  ,@DescuentoInd " +
                                              " ,@CompInvisibleInd " +
                                              " ,@PorCapital    " +
                                              "  ,@eg_ccCarteraComponente )";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuotaValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TotalValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AbonoValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescuentoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CompInvisibleInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = crediComponentes.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = crediComponentes.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@CuotaValor"].Value = crediComponentes.CuotaValor.Value;
                mySqlCommandSel.Parameters["@TotalValor"].Value = crediComponentes.TotalValor.Value;
                mySqlCommandSel.Parameters["@AbonoValor"].Value = crediComponentes.AbonoValor.Value;
                mySqlCommandSel.Parameters["@DescuentoInd"].Value = crediComponentes.DescuentoInd.Value;
                mySqlCommandSel.Parameters["@CompInvisibleInd"].Value = crediComponentes.CompInvisibleInd.Value;
                mySqlCommandSel.Parameters["@PorCapital"].Value = crediComponentes.PorCapital.Value.Value;
                mySqlCommandSel.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraComponentes_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Añade los componentes a la tabla ccCreditoComponente
        /// </summary>
        /// <param name="numDocOld">NumeroDocViejo</param>
        /// <param name="numDocNew">NumeroDocViejo</param>
        public void DAL_ccCarteraComponentes_AddFromccSolicitudComponentes(int numDocOld, int numDocNew)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
               
                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO ccCreditoComponentes   " +
                                              "  ([NumeroDoc]   " +
                                              "  ,[ComponenteCarteraID]   " +
                                              "  ,[CuotaValor]   " +
                                              "  ,[TotalValor]   " +
                                              "  ,[AbonoValor] " +
                                              "  ,[DescuentoInd] " +
                                              "  ,[CompInvisibleInd] " +
                                              "  ,[PorCapital] " +
                                              "  ,[eg_ccCarteraComponente] )" +
                                              "   SELECT    " +
                                              "   @NumeroDoc    " +
                                              "  ,ComponenteCarteraID    " +
                                              "  ,CuotaValor    " +
                                              "  ,TotalValor    " +
                                              "  ,@AbonoValor    " +
                                              "  ,@DescuentoInd    " +
                                              "  ,CompInvisibleInd    " +
                                              "  ,PorCapital    " +
                                              "  ,eg_ccCarteraComponente" +
                                              "  FROM ccSolicitudComponentes " +
                                              "  WHERE NumeroDoc = @numDocOld ";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@numDocOld", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AbonoValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescuentoInd", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@numDocOld"].Value = numDocOld;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDocNew;
                mySqlCommandSel.Parameters["@AbonoValor"].Value = 0;
                mySqlCommandSel.Parameters["@DescuentoInd"].Value = 0;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraComponentes_Add_From_SolicitudComponentes");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudComponentes_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudComponentes WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudComponentes_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Añade los componentes a la tabla ccCreditoComponente
        /// </summary>
        /// <param name="numDocOld">NumeroDocViejo</param>
        /// <param name="numDocNew">NumeroDocViejo</param>
        public void DAL_ccCarteraComponentes_Update(DTO_ccCreditoComponentes crediComponentes)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuotaValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TotalValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AbonoValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DescuentoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CompInvisibleInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocPago", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = crediComponentes.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = crediComponentes.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@CuotaValor"].Value = crediComponentes.CuotaValor.Value;
                mySqlCommandSel.Parameters["@TotalValor"].Value = crediComponentes.TotalValor.Value;
                mySqlCommandSel.Parameters["@AbonoValor"].Value = crediComponentes.AbonoValor.Value;
                mySqlCommandSel.Parameters["@DescuentoInd"].Value = crediComponentes.DescuentoInd.Value;
                mySqlCommandSel.Parameters["@CompInvisibleInd"].Value = crediComponentes.CompInvisibleInd.Value;
                mySqlCommandSel.Parameters["@DocPago"].Value = crediComponentes.DocPago.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "UPDATE ccCreditoComponentes SET" +
                                              "  ComponenteCarteraID = @ComponenteCarteraID " +
                                              "  ,CuotaValor = @CuotaValor " +
                                              "  ,TotalValor = @TotalValor " +
                                              "  ,AbonoValor = @AbonoValor " +
                                              "  ,DescuentoInd = @DescuentoInd " +
                                              "  ,CompInvisibleInd = @CompInvisibleInd " +
                                              "  ,DocPago = @DocPago " +
                                              "WHERE  NumeroDoc = @NumeroDoc " +
                                              "AND ComponenteCarteraID = @ComponenteCarteraID ";
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCarteraComponentes_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoComponentes
        /// </summary>
        /// <param name="NumeroDoc">Numero doc que filtra los componentes</param>
        /// <param name="compCarteraID">Identificar del componente filtra la lista de los componentes</param>
        /// <param name="isCooperativa">Indentifica si debe buscar en la tabla de cooperativa o de financiera</param>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public DTO_ccCreditoComponentes DAL_ccCreditoComponentes_GetForReintegro(int NumeroDoc, string compCarteraID)
        {
            try
            {
                DTO_ccCreditoComponentes result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.Parameters["@ComponenteCarteraID"].Value = compCarteraID;

                mySqlCommand.CommandText = "SELECT sc.*,cc.Descriptivo " +
                                           "FROM ccCreditoComponentes sc with(nolock)" +
                                           "    INNER JOIN ccCarteraComponente cc  with(nolock) on sc.ComponenteCarteraID = cc.ComponenteCarteraID " +
                                           "        AND cc.EmpresaGrupoID = sc.eg_ccCarteraComponente " +
                                           "WHERE sc.NumeroDoc = @numeroDoc AND sc.ComponenteCarteraID = @ComponenteCarteraID AND sc.DocPago IS NULL ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoComponentes(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoComponentes_GetForReintegro");
                throw exception;
            }
        }

        #endregion
    }
}
