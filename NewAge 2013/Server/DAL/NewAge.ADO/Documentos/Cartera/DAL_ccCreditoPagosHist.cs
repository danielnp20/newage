using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_ccCreditoPagosHist : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCreditoPagosHist(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagosHist_GetByNumDocCredito(int NumeroDoc)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText =
                    "select ctrl.DocumentoID as TipoDocumento, Cast(RTrim(ctrl.PrefijoID)+'-'+Convert(Varchar, ctrl.DocumentoNro) as Varchar(50)) as PrefDoc, " +
                    " Cast(RTrim(ctrl.ComprobanteID)+'-'+Convert(Varchar, ctrl.ComprobanteIDNro) as Varchar(50)) as Comprobante, " +
                    " ctrl.PrefijoID, ctrl.DocumentoNro, ctrl.FechaDoc as FechaPago,IsNull(rec.FechaConsignacion,ctrl.FechaDoc) as FechaConsigna, rec.CajaID, rec.BancoCuentaID, pag.* " +
                    "from ccCreditoPagosHist pag with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on pag.PagoDocu = ctrl.NumeroDoc " + //and Estado = 3 " +
                    "   left join tsReciboCajaDocu rec with(nolock) on pag.PagoDocu = rec.NumeroDoc " +
                    "where pag.NumeroDoc = @NumeroDoc ";                    

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoPagos r = new DTO_ccCreditoPagos(dr);
                    if (!string.IsNullOrWhiteSpace(dr["TipoDocumento"].ToString()))
                        r.TipoDocumento.Value = Convert.ToInt32(dr["TipoDocumento"]);
                    r.FechaPago.Value = Convert.ToDateTime(dr["FechaPago"]);
                    r.FechaConsigna.Value = Convert.ToDateTime(dr["FechaConsigna"]);
                    r.CajaID.Value = dr["CajaID"].ToString();
                    r.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();

                    r.PrefDoc.Value = dr["PrefDoc"].ToString().Trim();
                    r.Comprobante.Value = dr["Comprobante"].ToString().Trim();

                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagosHist_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCreditoPagosHist_Add(DTO_ccCreditoPagos creditoPagos, int numDocProceso)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query
                    mySqlCommandSel.CommandText = "    INSERT INTO ccCreditoPagosHist   " +
                                                   "    ([CreditoCuotaNum]   " +
                                                   "    ,[NumeroDoc]   " +
                                                   "    ,[NumeroDocProceso]   " +
                                                   "    ,[PagoDocu]   " +
                                                   "    ,[ComponenteCarteraID]   " +
                                                   "    ,[Valor]   " +
                                                   "    ,[VlrCapital]   " +
                                                   "    ,[VlrInteres]   " +
                                                   "    ,[VlrSeguro]   " +
                                                   "    ,[VlrOtro1]   " +
                                                   "    ,[VlrOtro2]   " +
                                                   "    ,[VlrOtro3]   " +
                                                   "    ,[VlrOtrosfijos]   " +
                                                   "    ,[VlrCapitalCesion]   " +
                                                   "    ,[VlrUtilidadCesion]   " +
                                                   "    ,[VlrDerechosCesion]   " +
                                                   "    ,[VlrMoraliquida]   " +
                                                   "    ,[VlrMoraPago]   " +
                                                   "    ,[VlrPrejuridicoPago] " +
                                                   "    ,[VlrAjusteUsura] " +
                                                   "    ,[VlrOtrosComponentes] " +
                                                   "    ,[FechaLiquidaMoraANT] " +
                                                   "    ,[VlrMoraLiquidaANT] " +
                                                   "    ,[VlrMoraPagoANT] " +
                                                   "    ,[DiasMora]   " +
                                                   "    ,[DocVenta]   " +
                                                   "    ,[DocumentoAnula]   " +
                                                   "    ,[TipoPago])   " +
                                                   "  VALUES    " +
                                                   "  (@CreditoCuotaNum    " +
                                                   "  ,@NumeroDoc    " +
                                                   "  ,@NumeroDocProceso    " +
                                                   "  ,@PagoDocu    " +
                                                   "  ,@ComponenteCarteraID " +
                                                   "  ,@Valor    " +
                                                   "  ,@VlrCapital    " +
                                                   "  ,@VlrInteres    " +
                                                   "  ,@VlrSeguro    " +
                                                   "  ,@VlrOtro1    " +
                                                   "  ,@VlrOtro2    " +
                                                   "  ,@VlrOtro3    " +
                                                   "  ,@VlrOtrosfijos    " +
                                                   "  ,@VlrCapitalCesion   " +
                                                   "  ,@VlrUtilidadCesion   " +
                                                   "  ,@VlrDerechosCesion   " +
                                                   "  ,@VlrMoraliquida    " +
                                                   "  ,@VlrMoraPago    " +
                                                   "  ,@VlrPrejuridicoPago    " +
                                                   "  ,@VlrAjusteUsura " +
                                                   "  ,@VlrOtrosComponentes " +
                                                   "  ,@FechaLiquidaMoraANT " +
                                                   "  ,@VlrMoraLiquidaANT " +
                                                   "  ,@VlrMoraPagoANT " +
                                                   "  ,@DiasMora    " +
                                                   "  ,@DocVenta    " +
                                                   "  ,@DocumentoAnula    " +
                                                   "  ,@TipoPago)  ";
                    #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@CreditoCuotaNum", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocProceso", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PagoDocu", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtrosfijos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMoraliquida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrPrejuridicoPago", SqlDbType.Int); 
                mySqlCommandSel.Parameters.Add("@VlrAjusteUsura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtrosComponentes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaLiquidaMoraANT", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@VlrMoraLiquidaANT", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraPagoANT", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DiasMora", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoPago", SqlDbType.TinyInt);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@CreditoCuotaNum"].Value = creditoPagos.CreditoCuotaNum.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = creditoPagos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDocProceso"].Value = numDocProceso;
                mySqlCommandSel.Parameters["@PagoDocu"].Value = creditoPagos.PagoDocu.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = creditoPagos.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = creditoPagos.Valor.Value;
                mySqlCommandSel.Parameters["@VlrCapital"].Value = creditoPagos.VlrCapital.Value;
                mySqlCommandSel.Parameters["@VlrInteres"].Value = creditoPagos.VlrInteres.Value;
                mySqlCommandSel.Parameters["@VlrSeguro"].Value = creditoPagos.VlrSeguro.Value;
                mySqlCommandSel.Parameters["@VlrOtro1"].Value = creditoPagos.VlrOtro1.Value;
                mySqlCommandSel.Parameters["@VlrOtro2"].Value = creditoPagos.VlrOtro2.Value;
                mySqlCommandSel.Parameters["@VlrOtro3"].Value = creditoPagos.VlrOtro3.Value;
                mySqlCommandSel.Parameters["@VlrOtrosfijos"].Value = creditoPagos.VlrOtrosFijos.Value;
                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = creditoPagos.VlrCapitalCesion.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = creditoPagos.VlrUtilidadCesion.Value;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = creditoPagos.VlrDerechosCesion.Value;
                mySqlCommandSel.Parameters["@VlrMoraliquida"].Value = creditoPagos.VlrMoraliquida.Value;
                mySqlCommandSel.Parameters["@VlrMoraPago"].Value = creditoPagos.VlrMoraPago.Value;
                mySqlCommandSel.Parameters["@VlrPrejuridicoPago"].Value = creditoPagos.VlrPrejuridicoPago.Value; 
                mySqlCommandSel.Parameters["@VlrAjusteUsura"].Value = creditoPagos.VlrAjusteUsura.Value;
                mySqlCommandSel.Parameters["@VlrOtrosComponentes"].Value = creditoPagos.VlrOtrosComponentes.Value;
                mySqlCommandSel.Parameters["@FechaLiquidaMoraANT"].Value = creditoPagos.FechaLiquidaMoraANT.Value;
                mySqlCommandSel.Parameters["@VlrMoraLiquidaANT"].Value = creditoPagos.VlrMoraLiquidaANT.Value;
                mySqlCommandSel.Parameters["@VlrMoraPagoANT"].Value = creditoPagos.VlrMoraPagoANT.Value;
                mySqlCommandSel.Parameters["@DiasMora"].Value = creditoPagos.DiasMora.Value;
                mySqlCommandSel.Parameters["@DocVenta"].Value = creditoPagos.DocVenta.Value;
                mySqlCommandSel.Parameters["@DocumentoAnula"].Value = creditoPagos.DocumentoAnula.Value;
                mySqlCommandSel.Parameters["@TipoPago"].Value = creditoPagos.TipoPago.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPagos_Add");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCreditoPagos</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagosHist_GetByPagoID(int numeroDocPago)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagoDocu", SqlDbType.Int);
                mySqlCommand.Parameters["@PagoDocu"].Value = numeroDocPago;

                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPagosHist with(nolock)  " +
                                                           "WHERE PagoDocu = @PagoDocu";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoPagos r = new DTO_ccCreditoPagos(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagosHist_GetByPagoID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCreditoPagos</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagosHist_GetByCuotaNum(int cuotaNum)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@CreditoCuotaNum", SqlDbType.Int);
                mySqlCommand.Parameters["@CreditoCuotaNum"].Value = cuotaNum;
                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPagosHist SA with(nolock) WHERE CreditoCuotaNum = @CreditoCuotaNum";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoPagos r = new DTO_ccCreditoPagos(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagosHist_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro
        /// </summary>
        /// <returns>retorna un registro</returns>
        public DTO_ccCreditoPagos DAL_ccCreditoPagosHist_GetByCons(int Consecutivo)
        {
            try
            {
                DTO_ccCreditoPagos result = new DTO_ccCreditoPagos();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPagosHist SA with(nolock)  " +
                                                           "WHERE Consecutivo = @Consecutivo";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoPagos(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagosHist_GetByID");
                throw exception;
            }
        }

        #endregion
    }
}
