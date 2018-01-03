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
    public class DAL_ccCreditoPagos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCreditoPagos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagos_GetByNumDocCredito(int NumeroDoc)
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
                    "from cccreditopagos pag with(nolock) " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCreditoPagos_Add(DTO_ccCreditoPagos creditoPagos)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query
                    mySqlCommandSel.CommandText = "    INSERT INTO ccCreditoPagos   " +
                                                   "    ([CreditoCuotaNum]   " +
                                                   "    ,[NumeroDoc]   " +
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
        /// Trae la cantidad de pagos de un crédito
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public long DAL_ccCreditoPagos_CountByNumDocCredito(int NumeroDoc, DateTime? fecha)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                string queryFecha = string.Empty;
                if (fecha.HasValue)
                {
                    mySqlCommand.Parameters.Add("@FechaDoc", SqlDbType.DateTime);
                    mySqlCommand.Parameters["@FechaDoc"].Value = fecha.Value;

                    mySqlCommand.CommandText +=
                        "SELECT COUNT(*) " +
                        "FROM ccCreditoPagos pag with(nolock) " +
                        "   INNER JOIN gldocumentoControl ctrl with(nolock) ON pag.PagoDocu = ctrl.NumeroDoc and FechaDoc > @FechaDoc " +
                        "WHERE pag.NumeroDoc = @NumeroDoc";
                }
                else
                {
                    mySqlCommand.CommandText = "SELECT sum(Valor) FROM ccCreditoPagos with(nolock) WHERE NumeroDoc = @NumeroDoc";
                }

                object res = mySqlCommand.ExecuteScalar();
                return Convert.ToInt64(res);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_CountByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCreditoPagos</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagos_GetByPagoID(int numeroDocPago)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagoDocu", SqlDbType.Int);
                mySqlCommand.Parameters["@PagoDocu"].Value = numeroDocPago;

                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPagos with(nolock)  " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByPagoID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCreditoPagos</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagos_GetByCuotaNum(int cuotaNum)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@CreditoCuotaNum", SqlDbType.Int);
                mySqlCommand.Parameters["@CreditoCuotaNum"].Value = cuotaNum;
                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPagos SA with(nolock) WHERE CreditoCuotaNum = @CreditoCuotaNum";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros del último pago
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCreditoPagos</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagos_GetUltimoPago(int numDocCredito)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numDocCredito;
                
                mySqlCommand.CommandText =
                    "select CuotaID, pag.* " +
                    "from cccreditopagos pag with(nolock) " +
                    "	inner join ccCreditoPlanPagos pp with(nolock) on pag.CreditoCuotaNum = pp.Consecutivo " +
                    "where PagoDocu in " +
                    "( " +
                    "	select top(1) PagoDocu " +
                    "	from cccreditopagos pag with(nolock) " +
                    "		inner join glDocumentoControl ctrl with(nolock) on pag.PagoDocu = ctrl.NumeroDoc and Estado = 3 " +
                    "	where pag.NumeroDoc = @NumeroDoc order by Consecutivo desc " +
                    ") ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoPagos r = new DTO_ccCreditoPagos(dr);
                    r.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros del último pago
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCreditoPagos</returns>
        public List<DTO_ccCreditoPagos> DAL_ccCreditoPagos_GetPagosByPeriodo(DateTime periodo, int numDocCredito)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numDocCredito;
                mySqlCommand.Parameters["@Estado"].Value = numDocCredito;
                mySqlCommand.Parameters["@Periodo"].Value = numDocCredito;

                mySqlCommand.CommandText =
                    "select pp.CuotaID, pag.DocumentoAnula, pag.VlrCapitalCesion, pag.VlrDerechosCesion " +
                    "from ccCreditoPagos pag with(nolock) " +
                    "	inner join glDocumentoControl ctrlAnula with(nolock) on pag.DocumentoAnula = ctrlAnula.NumeroDoc " +
                    "	inner join ccCreditoPlanPagos pp with(nolock) on pag.CreditoCuotaNum = pp.Consecutivo " +
                    "where pag.NumeroDoc = @NumeroDoc and ctrlAnula.PeriodoDoc = Periodo and ctrl.Estado = @Estado and pag.DocumentoAnula is not null " +
                    "union " +
                    "select pp.CuotaID, pag.DocumentoAnula, pag.VlrCapitalCesion, pag.VlrDerechosCesion " +
                    "from ccCreditoPagos pag with(nolock) " +
                    "	inner join ccCreditoPlanPagos pp with(nolock) on pag.CreditoCuotaNum = pp.Consecutivo " +
                    "	inner join glDocumentoControl ctrl with(nolock) on pag.PagoDocu = ctrl.NumeroDoc " +
                    "where pag.NumeroDoc = @NumeroDoc and ctrl.PeriodoDoc = Periodo and ctrl.Estado = @Estado and pag.DocumentoAnula is null";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoPagos r = new DTO_ccCreditoPagos();
                    r.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                    r.VlrCapitalCesion.Value = Convert.ToDecimal(dr["VlrCapitalCesion"]);
                    r.VlrDerechosCesion.Value = Convert.ToDecimal(dr["VlrDerechosCesion"]);

                    if (!string.IsNullOrWhiteSpace(dr["DocumentoAnula"].ToString()))
                        r.DocumentoAnula.Value = Convert.ToInt32(dr["DocumentoAnula"]);

                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los pagos posteriores a un pago que se desea revertir
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public int DAL_ccCreditoPagos_GetByPagosForReversion(int numeroDocCredito, int numDocPago)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;


                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDocCredito;

                mySqlCommand.Parameters.Add("@PagoDocu", SqlDbType.Int);
                mySqlCommand.Parameters["@PagoDocu"].Value = numDocPago;

                mySqlCommand.CommandText = "SELECT COUNT(*) FROM ccCreditoPagos pag  with(nolock) " +
                                            " Inner join glDocumentoControl ctrlPago with(nolock) on pag.PagoDocu = ctrlPago.NumeroDoc  " +
                                            " WHERE  pag.NumeroDoc = @NumeroDoc and pag.PagoDocu > @PagoDocu and ctrlPago.Estado = 3 and pag.Valor > 0 ";

                return Convert.ToInt32(mySqlCommand.ExecuteScalar());

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByPagoID");
                throw exception;
            }
        }

        /// <summary>
        /// Tar la información del penúltimo pago
        /// </summary>
        /// <param name="numDocCredito">N'umero doc del crédito</param>
        /// <param name="cuotaId">Identificador de la cuota</param>
        /// <param name="vlrMoraLiquida">Valor Mora Liquida</param>
        /// <param name="vlrMoraPago">Valor mora pagado</param>
        /// <param name="fechaMoraPago">Fecha Valor Mora liquida</param>
        public void DAL_ccCreditoPagos_GetInfoPagoAnterior(int numDocCredito, int pagodocu, int cuotaId, ref decimal vlrMoraLiquida, ref decimal vlrMoraPago, ref DateTime? fechaLiquidaMora)
        {
            try
            {
                List<DTO_ccCreditoPagos> result = new List<DTO_ccCreditoPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;


                mySqlCommand.Parameters.Add("@NumeroDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CreditoCuotaNum", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PagoDocu", SqlDbType.Int);

                mySqlCommand.Parameters["@NumeroDocCredito"].Value = numDocCredito;
                mySqlCommand.Parameters["@CreditoCuotaNum"].Value = cuotaId;
                mySqlCommand.Parameters["@PagoDocu"].Value = pagodocu;

                mySqlCommand.CommandText =
                    "select top(1) VlrMoraliquida, VlrMoraPago, FechaDoc AS FechaLiquidaMora " +
                    "from ccCreditoPagos pag with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on pag.PagoDocu = ctrl.NumeroDoc " +
                    "where pag.numerodoc = @NumeroDocCredito and CreditoCuotaNum = @CreditoCuotaNum and PagoDocu <> @PagoDocu " +
                    "   and pag.Valor > 0 and pag.DocumentoAnula is null " +
                    "order by PagoDocu desc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    vlrMoraLiquida = Convert.ToDecimal(dr["VlrMoraLiquida"]);
                    vlrMoraPago = Convert.ToDecimal(dr["VlrMoraPago"]);
                    fechaLiquidaMora = Convert.ToDateTime(dr["FechaLiquidaMora"]);
                }

                dr.Close();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetInfoUltimoPago");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el documento de anulación
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccCreditoPagos_UpdateDocAnula(int consecutivo, int numDocAnula)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommandSel.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommandSel.Parameters["@DocumentoAnula"].Value = numDocAnula;

                #region Query
                mySqlCommandSel.CommandText =
                    " UPDATE ccCreditoPagos SET" +
                    "    DocumentoAnula = @DocumentoAnula " +
                    " WHERE Consecutivo = @Consecutivo ";
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_UpdateVals");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro
        /// </summary>
        /// <returns>retorna un registro</returns>
        public DTO_ccCreditoPagos DAL_ccCreditoPagos_GetByCons(int Consecutivo)
        {
            try
            {
                DTO_ccCreditoPagos result = new DTO_ccCreditoPagos();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = Consecutivo;

                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPagos SA with(nolock)  " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPagos_GetByID");
                throw exception;
            }
        }

        #endregion
    }
}
