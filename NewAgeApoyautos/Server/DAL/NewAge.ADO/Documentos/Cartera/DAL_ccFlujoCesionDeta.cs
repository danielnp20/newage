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
    public class DAL_ccFlujoCesionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccFlujoCesionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de ccFlujoCesionDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public List<DTO_ccFlujoCesionDeta> DAL_ccFlujoCesionDeta_GetByID(int libranza)
        {
            try
            {
                List<DTO_ccFlujoCesionDeta> result = new List<DTO_ccFlujoCesionDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Libranza"].Value = libranza;

                mySqlCommand.CommandText =
                    "SELECT ctrlFlujo.FechaDoc AS FechaPago, flujo.* " +
                    "FROM ccFlujoCesionDeta flujo with(nolock) " +
                    "   INNER JOIN glDocumentoControl ctrlFlujo WITH(NOLOCK) ON flujo.NumeroDoc = ctrlFlujo.NumeroDoc AND ctrlFlujo.Estado NOT IN (-1, 0) " +
                    "WHERE flujo.Libranza = @Libranza and ctrlFlujo.EmpresaID=@EmpresaID";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccFlujoCesionDeta r = new DTO_ccFlujoCesionDeta(dr);
                    if (!string.IsNullOrWhiteSpace(dr["FechaPago"].ToString()))
                        r.FechaPago.Value = Convert.ToDateTime(dr["FechaPago"]);

                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_FlujoCesionDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccFlujoCesionDeta
        /// </summary>
        /// <returns></returns>
        public void DAL_ccFlujoCesionDeta_Add(DTO_ccFlujoCesionDeta flujoCesionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccFlujoCesionDeta   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[CreditoCuotaNum] " +
                                               "    ,[VentadocNum] " +
                                               "    ,[DocPago] " +
                                               "    ,[Libranza] " +
                                               "    ,[Oferta] " +
                                               "    ,[Valor] " +
                                               "    ,[VlrCapitalCesion] " +
                                               "    ,[VlrUtilidadCesion] " +
                                               "    ,[VlrDerechosCesion]) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@CreditoCuotaNum " +
                                               "  ,@VentadocNum " +
                                               "  ,@DocPago " +
                                               "  ,@Libranza " +
                                               "  ,@Oferta " +
                                               "  ,@Valor " +
                                               "  ,@VlrCapitalCesion " +
                                               "  ,@VlrUtilidadCesion " +
                                               "  ,@VlrDerechosCesion) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CreditoCuotaNum", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VentadocNum", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = flujoCesionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CreditoCuotaNum"].Value = flujoCesionDeta.CreditoCuotaNum.Value;
                mySqlCommandSel.Parameters["@DocPago"].Value = flujoCesionDeta.DocPago.Value;
                mySqlCommandSel.Parameters["@VentadocNum"].Value = flujoCesionDeta.VentaDocNum.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = flujoCesionDeta.Libranza.Value;
                mySqlCommandSel.Parameters["@Oferta"].Value = flujoCesionDeta.Oferta.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = flujoCesionDeta.Valor.Value;
                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = flujoCesionDeta.VlrCapitalCesion.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = flujoCesionDeta.VlrUtilidadCesion.Value;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = flujoCesionDeta.VlrDerechosCesion.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_FlujoCesionDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccFlujoCesionDeta
        /// </summary>
        /// <returns></returns>
        public void DAL_ccFlujoCesionDeta_Update(DTO_ccFlujoCesionDeta flujoCesionDeta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CreditoCuotaNum", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VentadocNum", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = flujoCesionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CreditoCuotaNum"].Value = flujoCesionDeta.CreditoCuotaNum.Value;
                mySqlCommandSel.Parameters["@DocPago"].Value = flujoCesionDeta.DocPago.Value;
                mySqlCommandSel.Parameters["@VentadocNum"].Value = flujoCesionDeta.VentaDocNum.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = flujoCesionDeta.Libranza.Value;
                mySqlCommandSel.Parameters["@Oferta"].Value = flujoCesionDeta.Oferta.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = flujoCesionDeta.Valor.Value;
                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = flujoCesionDeta.Valor.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = flujoCesionDeta.Valor.Value;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = flujoCesionDeta.VlrDerechosCesion.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccFlujoCesionDeta SET" +
                    "  ,CreditoCuotaNum = @CreditoCuotaNum " +
                    "  ,DocPago = @DocPago " +
                    "  ,VentadocNum = @VentadocNum " +
                    "  ,Libranza = @Libranza " +
                    "  ,Oferta = @Oferta " +
                    "  ,VlrCapitalCesion = @VlrCapitalCesion " +
                    "  ,VlrUtilidadCesion = @VlrUtilidadCesion " +
                    "  ,VlrDerechosCesion = @VlrDerechosCesion " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_FlujoCesionDeta_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae el saldo de un crédito
        /// </summary>
        /// <returns>retorna una el saldo de un crédito</returns>
        public decimal DAL_ccFlujoCesionDeta_GetSaldo(int NumeroDoc, out int flujosPagados)
        {
            try
            {
                List<DTO_ccFlujoCesionDeta> result = new List<DTO_ccFlujoCesionDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText =
                    "SELECT " +
                    "	CASE WHEN flujo.FlujosPagados IS NOT NULL THEN FlujosPagados ELSE 0 END AS FlujosPagados, " +
                    "	CASE WHEN flujo.Valor IS NOT NULL THEN query.VlrLibranza - flujo.Valor ELSE query.VlrLibranza END AS VlrSaldoLibranza " +
                    "FROM " +
                    "( " +
                    "    SELECT docu.NumeroDoc, docu.DocVenta, docu.Libranza, venDeta.VlrLibranza " +
                    "    FROM ccCreditoDocu docu with(nolock) " +
                    "        INNER JOIN ccVentaDeta venDeta with(nolock) ON venDeta.NumDocCredito = docu.NumeroDoc AND vendeta.NumDocRecompra IS NULL " +
                    "        INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = docu.NumeroDoc AND ctrl.Estado = 3 " +
                    "    WHERE docu.NumeroDoc = @NumeroDoc " +
                    ") AS query " +
                    "LEFT JOIN " +
                    "( " +
                        "SELECT ctrlCredito.NumeroDoc, COUNT(*) AS FlujosPagados, SUM(flujo.Valor) AS Valor " +
                        "FROM ccFlujoCesionDeta flujo WITH(NOLOCK) " +
                        "	INNER JOIN ccCreditoPlanPagos pp WITH(NOLOCK) ON flujo.CreditoCuotaNum = pp.Consecutivo " +
                        "	INNER JOIN glDocumentoControl ctrlCredito WITH(NOLOCK) ON ctrlCredito.NumeroDoc = pp.NumeroDoc " +
                        "GROUP BY ctrlCredito.NumeroDoc " +
                    ") AS flujo ON query.NumeroDoc = flujo.NumeroDoc ";

                flujosPagados = 0;
                decimal saldo = 0;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    flujosPagados = Convert.ToInt32(dr["FlujosPagados"]);
                    saldo = Convert.ToInt32(dr["VlrSaldoLibranza"]);
                }

                dr.Close();
                return saldo;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_FlujoCesionDeta_GetByID");
                throw exception;
            }
        }

        #endregion
    }

}
