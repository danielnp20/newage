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
    public class DAL_ccRecompraDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccRecompraDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public List<DTO_ccNominaDeta> DAL_ccRecompraDeta_GetByID(int NumeroDoc)
        {
            try
            {
                List<DTO_ccNominaDeta> result = new List<DTO_ccNominaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccRecompraDeta with(nolock)  " +
                                            "WHERE NumeroDoc = @NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccNominaDeta r = new DTO_ccNominaDeta(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccRecompraDeta_Add(DTO_ccRecompraDeta recompraDeta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccRecompraDeta   " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[NumDocCredito]   " +
                                               "    ,[NumDocSustituye]   " +
                                               "    ,[Portafolio]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[VlrCuota]   " +
                                               "    ,[CuotasRecompra]   " +
                                               "    ,[VlrLibranza]   " +
                                               "    ,[VlrRecompra]   " +
                                               "    ,[VlrDerechos]   " +
                                               "    ,[FactorRecompra]   " +
                                               "    ,[VlrSustLibranza]   " +
                                               "    ,[VlrSustRecompra]   " +
                                               "    ,[VlrNeto]   " +
                                               "    ,[SustituyeInd])   " +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@NumDocCredito  " +
                                               "  ,@NumDocSustituye  " +
                                               "  ,@Portafolio  " +
                                               "  ,@CuotaID  " +
                                               "  ,@VlrCuota  " +
                                               "  ,@CuotasRecompra  " +
                                               "  ,@VlrLibranza  " +
                                               "  ,@VlrRecompra  " +
                                               "  ,@VlrDerechos  " +
                                               "  ,@FactorRecompra  " +
                                               "  ,@VlrSustLibranza  " +
                                               "  ,@VlrSustRecompra  " +
                                               "  ,@VlrNeto " +
                                               "  ,@SustituyeInd)   ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocSustituye", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Portafolio", SqlDbType.Char, UDT_PortafolioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotasRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrRecompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDerechos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSustLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSustRecompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrNeto", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SustituyeInd", SqlDbType.Int);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = recompraDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = recompraDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocSustituye"].Value = recompraDeta.NumDocSustituye.Value;
                mySqlCommandSel.Parameters["@Portafolio"].Value = recompraDeta.Portafolio.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = recompraDeta.CuotaID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = recompraDeta.VlrCuota.Value;
                mySqlCommandSel.Parameters["@CuotasRecompra"].Value = recompraDeta.CuotasRecompra.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = recompraDeta.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrRecompra"].Value = recompraDeta.VlrRecompra.Value;
                mySqlCommandSel.Parameters["@VlrDerechos"].Value = recompraDeta.VlrDerechos.Value;
                mySqlCommandSel.Parameters["@FactorRecompra"].Value = recompraDeta.FactorRecompra.Value;
                mySqlCommandSel.Parameters["@VlrSustLibranza"].Value = recompraDeta.VlrSustLibranza.Value;
                mySqlCommandSel.Parameters["@VlrSustRecompra"].Value = recompraDeta.VlrSustRecompra.Value;
                mySqlCommandSel.Parameters["@VlrNeto"].Value = recompraDeta.VlrNeto.Value;
                mySqlCommandSel.Parameters["@SustituyeInd"].Value = recompraDeta.SustituyeInd.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccRecompraDeta_Update(DTO_ccRecompraDeta recompraDeta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "UPDATE ccRecompraDeta SET" +
                    "  ,NumDocCredito = @NumDocCredito  " +
                    "  ,NumDocSustituye = @NumDocSustituye  " +
                    "  ,Portafolio = @Portafolio  " +
                    "  ,CuotaID = @CuotaID  " +
                    "  ,VlrCuota = @VlrCuota  " +
                    "  ,CuotasRecompra = @CuotasRecompra  " +
                    "  ,VlrLibranza = @VlrLibranza  " +
                    "  ,VlrRecompra = @VlrRecompra  " +
                    "  ,VlrDerechos  = @VlrDerechos  " +
                    "  ,FactorRecompra = @FactorRecompra  " +
                    "  ,VlrSustLibranza = @VlrSustLibranza  " +
                    "  ,VlrSustRecompra = @VlrSustRecompra  " +
                    "  ,VlrNeto = @VlrNeto  " +
                    "  ,SustituyeInd = @SustituyeInd " +
                    " WHERE  NumeroDoc = @NumeroDoc ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocSustituye", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Portafolio", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotasRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrRecompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDerechos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSustLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSustRecompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrNeto", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SustituyeInd", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = recompraDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = recompraDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocSustituye"].Value = recompraDeta.NumDocSustituye.Value;
                mySqlCommandSel.Parameters["@Portafolio"].Value = recompraDeta.Portafolio.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = recompraDeta.CuotaID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = recompraDeta.VlrCuota.Value;
                mySqlCommandSel.Parameters["@CuotasRecompra"].Value = recompraDeta.CuotasRecompra.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = recompraDeta.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrRecompra"].Value = recompraDeta.VlrRecompra.Value;
                mySqlCommandSel.Parameters["@VlrDerechos"].Value = recompraDeta.VlrDerechos.Value;
                mySqlCommandSel.Parameters["@FactorRecompra"].Value = recompraDeta.FactorRecompra.Value;
                mySqlCommandSel.Parameters["@VlrSustLibranza"].Value = recompraDeta.VlrSustLibranza.Value;
                mySqlCommandSel.Parameters["@VlrSustRecompra"].Value = recompraDeta.VlrSustRecompra.Value;
                mySqlCommandSel.Parameters["@VlrNeto"].Value = recompraDeta.VlrNeto.Value;
                mySqlCommandSel.Parameters["@SustituyeInd"].Value = recompraDeta.SustituyeInd.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cfRecompraDeta_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Funcion que retorna la lista de recompras asignadas a ese comprado
        /// </summary>
        /// <param name="compradorCarteraID">Comprador de cartera</param>
        /// <returns>Lista con las recompras del comprador de cartera</returns>
        public List<DTO_ccRecompraDeta> DAL_ccRecompraDeta_GetByCompradorCartera(string compradorCarteraID)
        {
            try
            {
                List<DTO_ccRecompraDeta> result = new List<DTO_ccRecompraDeta>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                string query = string.Empty;

                mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                #region Query Cooperativa
                mySqlCommand.CommandText =
                    "SELECT recompDeta.* " +
                    "FROM ccRecompraDeta recompDeta with(nolock) " +
                    "   INNER JOIN ccRecompraDocu recompDocu with(nolock) ON recompDocu.NumeroDoc = recompDeta.NumeroDoc " +
                    "       AND recompDocu.CompradorCarteraID = @CompradorCarteraID ";
                #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccRecompraDeta dto = new DTO_ccRecompraDeta(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDeta_GetByCompradorCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Indica si existe una incorporacoion en un periodo
        /// </summary>
        /// <param name="centroPagoID">CIdentifidcador del centro de pago</param>
        /// <param name="fechaIni">Fecha inicial de consulta</param>
        /// <param name="fechaFin">Fecha final de consulta</param>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <returns>Verdadero si existe, de lo contrario falso</returns>
        public List<DTO_ccRecompraDeta> DAL_ccRecompraDeta_GetForCompraAndSustitucion(string actFlujoID, string compradorCarteraID, DateTime periodo, string cuentaPagoEspecialID,int? libranza)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                List<DTO_ccRecompraDeta> result = new List<DTO_ccRecompraDeta>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if (libranza == null)
                {
                    #region Creacion Parametros
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                    #endregion
                    #region Asignacion Campos
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                    mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                    mySqlCommand.Parameters["@CuentaID"].Value = cuentaPagoEspecialID;
                    mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                    #endregion
                    #region Query
                    mySqlCommand.CommandText =
                        "SELECT query.NumeroDoc, query.DocVenta, query.Libranza, query.ClienteID, query.Nombre, query.CompradorCarteraID, query.TipoEstado,query.ClaseCredito,  " +
                        "    CASE WHEN flujo.FlujosPagados IS NOT NULL THEN query.CuotasVend - flujo.FlujosPagados ELSE query.CuotasVend END AS CuotasRecompra, " +
                        "    query.Oferta, query.VlrCuota, query.CuotaID, query.VlrLibranza, query.VlrPrepago, " +
                        "    CASE WHEN flujo.FlujosPagados IS NOT NULL THEN FlujosPagados ELSE 0 END AS FlujosPagados, " +
                        "    CASE WHEN flujo.Valor IS NOT NULL THEN query.VlrLibranza - flujo.Valor ELSE query.VlrLibranza END AS VlrSaldoLibranza, " +
                        "	 saldos.Saldo AS SaldoPagos,0 as VlrCapital,0 as VlrInteres,0 as VlrCapitalCesion,0 as VlrUtilidadCesion,0 as VlrDerechosCesion " +
                        "FROM " +
                        "( " +
                        "    SELECT docu.NumeroDoc, docu.DocVenta, docu.Libranza, docu.ClienteID, cli.Descriptivo AS Nombre, docu.CompradorCarteraID, docu.TipoEstado, " +
                        "        venDeta.CuotasVend, 0 AS Saldo, venDocu.Oferta, venDeta.VlrCuota, venDeta.CuotaID, venDeta.VlrLibranza, docu.VlrPrepago,clase.ClaseCredito  " +
                        "    FROM ccCreditoDocu docu with(nolock) " +
                        "        INNER JOIN ccVentaDeta venDeta with(nolock) ON venDeta.NumDocCredito = docu.NumeroDoc AND vendeta.NumDocRecompra IS NULL " +
                        "        INNER JOIN ccVentaDocu venDocu with(nolock) ON venDocu.NumeroDoc = venDeta.NumeroDoc AND docu.DocVenta = venDocu.NumeroDoc " +
                        "        INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = docu.ClienteID  and cli.EmpresaGrupoID = docu.eg_ccCliente " +
                        "        LEFT JOIN ccLineaCredito lin with(nolock) ON lin.LineaCreditoID = docu.LineaCreditoID  and lin.EmpresaGrupoID = docu.eg_ccLineaCredito " +
                        "        LEFT JOIN ccClasificacionCredito clase with(nolock) ON clase.ClaseCredito = lin.ClaseCredito  and clase.EmpresaGrupoID = lin.eg_ccClasificacionCredito " +
                        "        INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = docu.NumeroDoc AND ctrl.Estado = @Estado " +
                        "    WHERE docu.EmpresaID = @EmpresaID AND docu.CompradorCarteraID = @CompradorCarteraID AND DocPrepago IS NOT NULL AND SustituidoInd IS NULL " +
                        ") AS query " +
                        "INNER JOIN " +
                        "( " +
                        "    SELECT NumeroDoc, DocVenta " +
                        "    FROM ccCreditoPlanPagos WITH(NOLOCK)  " +
                        "    GROUP BY NumeroDoc, DocVenta " +
                        "    HAVING SUM(VlrCuota) = SUM(VlrPagadoCuota) " +
                        ") AS pp ON query.NumeroDoc = pp.NumeroDoc and query.DocVenta = pp.DocVenta " +
                        "INNER JOIN " +
                        "( " +
                        "    SELECT IdentificadorTR, SUM(SaldoIniML + DebitoML + CreditoML) AS Saldo " +
                        "    FROM Vista_coCuentaSaldo saldos WITH(NOLOCK) " +
                        "	WHERE EmpresaID = @EmpresaID and PeriodoID = @PeriodoID AND CuentaID = @CuentaID " +
                        "	GROUP BY IdentificadorTR " +
                        ") AS saldos ON query.NumeroDoc = saldos.IdentificadorTR " +
                        "LEFT JOIN " +
                        "( " +
                        "    SELECT ctrlCredito.NumeroDoc, flujo.VentadocNum AS DocVenta, COUNT(*) AS FlujosPagados, SUM(flujo.Valor) AS Valor " +
                        "    FROM ccFlujoCesionDeta flujo WITH(NOLOCK) " +
                        "        INNER JOIN ccCreditoPlanPagos pp WITH(NOLOCK) ON flujo.CreditoCuotaNum = pp.Consecutivo " +
                        "        INNER JOIN glDocumentoControl ctrlCredito WITH(NOLOCK) ON ctrlCredito.NumeroDoc = pp.NumeroDoc " +
                        "    GROUP BY ctrlCredito.NumeroDoc, flujo.VentadocNum " +
                        ") AS flujo ON query.NumeroDoc = flujo.NumeroDoc  and query.DocVenta = flujo.DocVenta ";

                    #endregion
                }
                else
                {
                    #region Creacion Parametros
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                    #endregion
                    #region Asignacion Campos
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                    mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                    mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                    mySqlCommand.Parameters["@Libranza"].Value = libranza;
                    #endregion
                    #region Query
                    mySqlCommand.CommandText =
                        "SELECT query.NumeroDoc, query.DocVenta, query.Libranza, query.ClienteID, query.Nombre, query.CompradorCarteraID, query.TipoEstado,query.ClaseCredito, " +
                        "    CASE WHEN flujo.FlujosPagados IS NOT NULL THEN query.CuotasVend - flujo.FlujosPagados ELSE query.CuotasVend END AS CuotasRecompra, " +
                        "    query.Oferta, query.VlrCuota, query.CuotaID, query.VlrLibranza, query.VlrPrepago, " +
                        "    CASE WHEN flujo.FlujosPagados IS NOT NULL THEN FlujosPagados ELSE 0 END AS FlujosPagados, " +
                        "    CASE WHEN flujo.Valor IS NOT NULL THEN query.VlrLibranza - flujo.Valor ELSE query.VlrLibranza END AS VlrSaldoLibranza, " +
                        "	 0 AS SaldoPagos, pp.VlrCapital,pp.VlrInteres,pp.VlrCapitalCesion,pp.VlrUtilidadCesion,pp.VlrDerechosCesion " +
                        "FROM " +
                        "( " +
                        "    SELECT docu.NumeroDoc, docu.DocVenta, docu.Libranza, docu.ClienteID, cli.Descriptivo AS Nombre, docu.CompradorCarteraID, docu.TipoEstado, " +
                        "        venDeta.CuotasVend, 0 AS Saldo, venDocu.Oferta, venDeta.VlrCuota, venDeta.CuotaID, venDeta.VlrLibranza, Isnull( docu.VlrPrepago,0) as VlrPrepago,clase.ClaseCredito  " +
                        "    FROM ccCreditoDocu docu with(nolock) " +
                        "        INNER JOIN ccVentaDeta venDeta with(nolock) ON venDeta.NumDocCredito = docu.NumeroDoc AND vendeta.NumDocRecompra IS NULL " +
                        "        INNER JOIN ccVentaDocu venDocu with(nolock) ON venDocu.NumeroDoc = venDeta.NumeroDoc  " +
                        "        INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = docu.ClienteID  AND cli.EmpresaGrupoID = docu.eg_ccCliente " +
                        "        LEFT JOIN ccLineaCredito lin with(nolock) ON lin.LineaCreditoID = docu.LineaCreditoID  AND lin.EmpresaGrupoID = docu.eg_ccLineaCredito " +
                        "        LEFT JOIN ccClasificacionCredito clase with(nolock) ON clase.ClaseCredito = lin.ClaseCredito  AND clase.EmpresaGrupoID = lin.eg_ccClasificacionCredito " +
                        "        INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = docu.NumeroDoc AND ctrl.Estado = @Estado " +
                        "    WHERE docu.EmpresaID = @EmpresaID AND docu.CompradorCarteraID = @CompradorCarteraID AND  docu.Libranza = @Libranza " +
                        ") AS query " +
                        "INNER JOIN " +
                        "( " +
                        "    SELECT pp.NumeroDoc,crd.Libranza, pp.DocVenta, " +
	                    "            Sum(pp.vlrcapital-Isnull(pag.AboCapital,0)) as VlrCapital , " +
	                    "            Sum(pp.VlrInteres-Isnull(pag.AboInteres,0)) as VlrInteres, " +
	                    "            Sum(pp.VlrCapitalCesion-Isnull(pag.AboCapCesa,0)) as VlrCapitalCesion,  " +
                        "            Sum(pp.VlrUtilidadCesion-Isnull(pag.AboUtiCesa,0)) as VlrUtilidadCesion,  " +
                        "            Sum(pp.VlrDerechosCesion-Isnull(pag.AboDerCesa,0)) as VlrDerechosCesion  " +
	                    "         FROM ccCreditoPlanPagos pp WITH(NOLOCK)   " +
                        "     left join ccCreditoDocu (nolock)as crd on crd.NumeroDoc = pp.NumeroDoc  " +
                        "     left join ( SELECT pag.CreditoCuotaNum, " +
                        "           SUM(vlrCapital)   as AboCapital, " +
                        "           SUM(VlrInteres)   as AboInteres, " +
                        "           SUM(VlrSeguro)   as AboCapSegu, " +
                        "           SUM(VlrOtro1)   as AboIntSegu, " +
                        "           SUM(VlrOtrosfijos)  as AboOtrFijo, " +
                        "           SUM(VlrCapitalCesion)   as AboCapCesa, " +
                        "           SUM(VlrUtilidadCesion) as AboUtiCesa, " +
                        "           SUM(VlrDerechosCesion) as AboDerCesa " +
                        "         FROM ccCreditoPagos (nolock) pag " +
                        "         group by pag.CreditoCuotaNum) as  pag ON pag.CreditoCuotaNum = pp.consecutivo " +
                        "     WHERE pp.CompradorCarteraID =  @CompradorCarteraID and pp.VlrCuota != pp.VlrPagadoCuota  " +
                        "     GROUP BY pp.NumeroDoc, pp.DocVenta, crd.Libranza " +
                        ") AS pp ON query.NumeroDoc = pp.NumeroDoc  " +                       
                        "LEFT JOIN " +
                        "( " +
                        "    SELECT ctrlCredito.NumeroDoc, flujo.VentadocNum AS DocVenta, COUNT(*) AS FlujosPagados, SUM(flujo.Valor) AS Valor " +
                        "    FROM ccFlujoCesionDeta flujo WITH(NOLOCK) " +
                        "        INNER JOIN ccCreditoPlanPagos pp WITH(NOLOCK) ON flujo.CreditoCuotaNum = pp.Consecutivo " +
                        "        INNER JOIN glDocumentoControl ctrlCredito WITH(NOLOCK) ON ctrlCredito.NumeroDoc = pp.NumeroDoc " +
                        "    GROUP BY ctrlCredito.NumeroDoc, flujo.VentadocNum " +
                        ") AS flujo ON query.NumeroDoc = flujo.NumeroDoc  and query.DocVenta = flujo.DocVenta ";

                    #endregion
                }
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccRecompraDeta dto = new DTO_ccRecompraDeta();
                    dto.Aprobado.Value = false;
                    dto.PrepagoInd.Value = true;
                    dto.NumDocCredito.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    dto.Oferta.Value = dr["Oferta"].ToString();
                    dto.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.ClienteID.Value = dr["ClienteID"].ToString();
                    dto.ClaseCredito.Value = dr["ClaseCredito"].ToString();
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                    dto.CuotasRecompra.Value = Convert.ToInt32(dr["CuotasRecompra"]);
                    dto.TipoEstado.Value = Convert.ToByte(dr["TipoEstado"]);
                    dto.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    dto.VlrLibranza.Value = Convert.ToDecimal(dr["VlrSaldoLibranza"]);
                    dto.VlrPrepago.Value = Convert.ToDecimal(dr["VlrPrepago"]);
                    dto.FlujosPagados.Value = Convert.ToInt32(dr["FlujosPagados"]);
                    dto.VlrDerechos.Value = 0;
                    dto.VlrSaldosPagos.Value = Convert.ToDecimal(dr["SaldoPagos"]);
                    dto.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                    dto.VlrInteres.Value = Convert.ToDecimal(dr["VlrInteres"]);
                    dto.VlrCapitalCesion.Value = Convert.ToDecimal(dr["VlrCapitalCesion"]);
                    dto.VlrUtilidadCesion.Value = Convert.ToDecimal(dr["VlrUtilidadCesion"]);
                    dto.VlrDerechosCesion.Value = Convert.ToDecimal(dr["VlrDerechosCesion"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDeta_GetForCompraYSust");
                throw exception;
            }
        }

        /// <summary>
        /// Indica si un crédito fue recomprado
        /// </summary>
        public bool DAL_ccRecompraDeta_IsRecomprado(int numDocCredito)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;
                #endregion
                #region Query
                mySqlCommand.CommandText =
                    "select count(*) " +
                    "from ccRecompraDeta deta with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on deta.NumeroDoc = ctrl.NumeroDoc " +
                    "where deta.NumDocCredito = @NumDocCredito and ctrl.EmpresaID = @EmpresaID and ctrl.Estado = @Estado ";
                #endregion

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDeta_GetForCompraYSust");
                throw exception;
            }
        }

        #endregion
    }

}
