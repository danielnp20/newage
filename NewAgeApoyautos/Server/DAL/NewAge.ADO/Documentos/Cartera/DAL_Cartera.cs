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
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_Cartera : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Cartera(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Generales

        /// <summary>
        /// Funcion que trae los saldos de los componentes de un credito
        /// </summary>
        /// <param name="numeroDoc">Numero de documento de consulta (pk)</param>
        /// <param name="periodo">Periodo de consulta para saber el saldo</param>
        /// <param name="fechaCorte">Fecha de corte para conocer la mora</param>
        /// <returns>Retorna os saldos de los componentes de un credito</returns>
        public List<DTO_ccSaldosComponentes> DAL_Cartera_GetComponentes(int numeroDoc, DateTime periodo, DateTime fechaCorte)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_ccSaldosComponentes> result = new List<DTO_ccSaldosComponentes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                mySqlCommandSel.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);

                #endregion

                mySqlCommandSel.CommandText = 
                    "SELECT * FROM Cartera_CreditoComponentes (@NumeroDoc,@EmpresaID,@PeriodoID,@FechaCorte," +
                    "@eg_ccCarteraComponente,@eg_coPlanCuenta,@eg_coTercero,@eg_ccAbogado)";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    if (string.IsNullOrWhiteSpace(dr["TotalSaldo"].ToString()))
                    {
                        result = null;
                        break;
                    }
                    else
                    {
                        DTO_ccSaldosComponentes saldosComp = new DTO_ccSaldosComponentes(dr);
                        saldosComp.Editable.Value = false;
                        result.Add(saldosComp);
                    }
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Cartera_GetComponentesStorePrecedure");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae los saldos de los componentes de un credito
        /// </summary>
        /// <param name="numeroDoc">Numero de documento de consulta (pk)</param>
        /// <param name="periodo">Periodo de consulta para saber el saldo</param>
        /// <param name="tercero">Identificador del tercero para las cuentas</param>
        /// <returns>Retorna os saldos de los componentes de un credito</returns>
        public List<DTO_ccSaldosComponentes> DAL_Cartera_GetSaldoCuentasByNumDoc(int numeroDoc, string tercero, DateTime periodo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_ccSaldosComponentes> result = new List<DTO_ccSaldosComponentes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@TerceroID"].Value = tercero;
                mySqlCommandSel.Parameters["@ModuloID"].Value = "CC"; //Modulo de Cartera                
                #endregion

                mySqlCommandSel.CommandText = "SELECT planCta.CuentaID, planCta.Descriptivo, " +
                                              "     SUM(ctaSaldo.DbSaldoIniLocML + ctaSaldo.CrSaldoIniLocML) As VlrInicial, "+
	                                          "     SUM(ctaSaldo.DbOrigenLocML + ctaSaldo.CrOrigenLocML) As VlrMovimiento, "+	   
	                                          "     SUM(ctaSaldo.DbOrigenLocML + ctaSaldo.CrOrigenLocML + ctaSaldo.DbSaldoIniLocML + ctaSaldo.CrSaldoIniLocML) As VlrSaldo "+ 
                                              " FROM coCuentaSaldo ctaSaldo " +
                                              "	    LEFT JOIN coPlanCuenta planCta with(nolock) ON ctaSaldo.CuentaID = planCta.CuentaID AND ctaSaldo.eg_coPlanCuenta = planCta.EmpresaGrupoID" +
                                              "     INNER JOIN glConceptoSaldo conSaldo with(nolock) ON conSaldo.ConceptoSaldoID = planCta.ConceptoSaldoID " +
                                              "                 AND conSaldo.ModuloID = @ModuloID AND conSaldo.coSaldoControl in (2,3,6) " +                                             
                                              "		AND ctaSaldo.PeriodoID = @PeriodoID AND ctaSaldo.IdentificadorTR = @NumeroDoc " +
                                              " GROUP BY planCta.CuentaID, planCta.Descriptivo";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    decimal VlrSaldo = Convert.ToDecimal(dr["VlrSaldo"]);
                    if (VlrSaldo > 0)
                    {
                        DTO_ccSaldosComponentes saldosComp = new DTO_ccSaldosComponentes();
                        saldosComp.CuentaID.Value = dr["CuentaID"].ToString();
                        saldosComp.Descriptivo.Value = dr["Descriptivo"].ToString();
                        saldosComp.TotalInicial.Value = Convert.ToDecimal(dr["VlrInicial"]);
                        saldosComp.AbonoSaldo.Value = Convert.ToDecimal(dr["VlrMovimiento"]);
                        saldosComp.TotalSaldo.Value = VlrSaldo;
                        result.Add(saldosComp);
                    }                    
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Cartera_GetComponentesStorePrecedure");
                throw exception;
            }
        }

        /// <summary>
        /// Procesa la migracon de nomina
        /// </summary>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="periodo">Periodo de proceso</param>
        /// <param name="fecha">Fecha de la migracion (15 o ultimo dia del mes)</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult DAL_Cartera_RecaudosMasivos_Inconsistencias(string centroPagoID, string pagaduriaID, DateTime periodo, DateTime fecha, List<DTO_ccIncorporacionDeta> data)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandTimeout = 0;

                #region Carga la info de los detalles
                DataTable nomina = new DataTable();
                nomina.Columns.Add("Consecutivo", typeof(int));
                nomina.Columns.Add("Fecha", typeof(DateTime));
                nomina.Columns.Add("Libranza", typeof(int));
                nomina.Columns.Add("ValorCuota", typeof(decimal));

                int i = 0;
                foreach (DTO_ccIncorporacionDeta dto in data)
                {
                    i++;

                    DataRow fila = nomina.NewRow();
                    fila["Consecutivo"] = i;
                    fila["Fecha"] = dto.FechaNomina.Value;
                    fila["Libranza"] = dto.Libranza.Value;
                    fila["ValorCuota"] = dto.ValorCuota.Value;

                    nomina.Rows.Add(fila);
                }
                #endregion
                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccNominaINC", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                //Parametro del footer (lista)
                mySqlCommand.Parameters.Add("@Nomina", SqlDbType.Structured);
                mySqlCommand.Parameters["@Nomina"].TypeName = "MigracionNomina";

                //Parametros de salida
                mySqlCommand.Parameters.Add("@ErrorDesc", SqlDbType.VarChar, 1000);
                mySqlCommand.Parameters["@ErrorDesc"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@CentroPagoID"].Value = centroPagoID;
                mySqlCommand.Parameters["@PagaduriaID"].Value = pagaduriaID;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@Fecha"].Value = fecha;
                mySqlCommand.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccNominaINC"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccNominaINC, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);

                mySqlCommand.Parameters["@Nomina"].Value = nomina;

                #endregion

                #region Ejecuta la consulta y carga los resultados

                List<DTO_TxResultDetail> rd_Opero = new List<DTO_TxResultDetail>();
                List<DTO_TxResultDetail> rd_NoOpero = new List<DTO_TxResultDetail>();
                
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Cartera_RecaudosMasivos_Inconsistencia";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (mySqlCommand.Parameters["@ErrorDesc"].Value != null && !string.IsNullOrWhiteSpace(mySqlCommand.Parameters["@ErrorDesc"].Value.ToString()))
                {
                    var exception = new Exception(mySqlCommand.Parameters["@ErrorDesc"].Value.ToString());
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Cartera_RecaudosMasivos_Inconsistencias");
                }
                else
                {
                    #region Resultado de cruce
                    int line = 1;

                    while (dr.Read())
                    {
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.line = line;

                        switch (dr["EstadoCruce"].ToString())
                        {
                            #region Tipos de inconsistencias
                            case "2":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce2 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_NoOpero.Add(rd);
                                break;
                            case "3":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce3 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_NoOpero.Add(rd);
                                break;
                            case "4":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce4 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_Opero.Add(rd);
                                break;
                            case "5":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce5 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_Opero.Add(rd);
                                break;
                            case "6":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce6 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_NoOpero.Add(rd);
                                break;
                            case "7":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce7 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_Opero.Add(rd);
                                break;
                            case "8":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce8 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_Opero.Add(rd);
                                break;
                            case "9":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce9 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_Opero.Add(rd);
                                break;
                            case "10":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce10 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_NoOpero.Add(rd);
                                break;
                            case "11":
                                rd.Message = DictionaryMessages.Err_Cc_EstadoCruce11 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                rd_Opero.Add(rd);
                                break;
                            #endregion
                        }

                        ++line;
                    }
                    #endregion
                }

                dr.Close();

                #endregion

                //Agrega los resultados
                result.Details.AddRange(rd_Opero);
                result.Details.AddRange(rd_NoOpero);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Cartera_MigracionNomina_Inconsistencias");
                return result;
            }
        }

        /// <summary>
        /// Trae la lista de comprobantes para realizar la distribucion
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="origen">Filtro de origen</param>
        /// <param name="excls">Lista de exclusiones</param>
        /// <returns>Retiorna la lista de auxiliares</returns>
        public List<DTO_ComprobanteFooter> DAL_Cartera_GetInfoCreditoConsumo(DateTime periodo, List<string> cuentasID, List<int> excludeIdTRs)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros generales

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;

                #endregion

                string Idtrs = string.Empty;
                for (int i = 0; i < excludeIdTRs.Count; ++i)
                {
                    if (i == 0)
                        Idtrs = " and IdentificadorTR not in(" + excludeIdTRs[i].ToString();
                    else
                        Idtrs += "," + excludeIdTRs[i].ToString();

                    if (i == excludeIdTRs.Count - 1)
                        Idtrs += ")";
                }

                string cuentas = string.Empty;
                for (int i = 0; i < cuentasID.Count; ++i)
                {
                    if (i == 0)
                        cuentas = " and saldo.CuentaID in(" + cuentasID[i];
                    else
                        cuentas += "," + cuentasID[i];

                    if (i == excludeIdTRs.Count - 1)
                        cuentas += ")";
                }
                mySqlCommand.CommandText =
                    "select distinct saldo.TerceroID,DocumentoTercero, " +
                    "    Sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) AS vlrMdaLoc, " +
                    "   Sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) AS vlrMdaExt " +
                    "from coCuentaSaldo saldo with(nolock) " +
                    "    inner join glDocumentoControl ctrl with(nolock) on saldo.IdentificadorTR = ctrl.NumeroDoc " +
                    "where saldo.EmpresaID = @EmpresaID and PeriodoID= @PeriodoID " + cuentas + Idtrs +
                    "group by saldo.TerceroID,DocumentoTercero " +
                    "order by TerceroID ";

                List<DTO_ComprobanteFooter> results = new List<DTO_ComprobanteFooter>();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ComprobanteFooter det = new DTO_ComprobanteFooter();
                    det.TerceroID.Value = dr["TerceroID"].ToString();
                    det.DocumentoCOM.Value = dr["DocumentoTercero"].ToString();
                    det.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]) * -1;
                    det.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]) * -1;
                    det.vlrMdaOtr.Value = Convert.ToDecimal(dr["vlrMdaLoc"]) * -1;

                    results.Add(det);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetForDistribucion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de comprobantes para realizar la distribucion
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="origen">Filtro de origen</param>
        /// <param name="excls">Lista de exclusiones</param>
        /// <returns>Retiorna la lista de auxiliares</returns>
        public decimal DAL_Cartera_GetValorAbonoPagoTotal(string terceroID, string libranzaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros generales

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.PagosTotales;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@Libranza"].Value = libranzaID;
                mySqlCommand.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;

                #endregion

                mySqlCommand.CommandText =
                    "select sum(Valor) " +
                    "from glDocumentoControl with(nolock) " +
                    "where EmpresaId = @EmpresaID and DocumentoID = @DocumentoID and TerceroID = @TerceroID and DocumentoTercero = @Libranza and Estado=@Estado";

                object obj =  mySqlCommand.ExecuteScalar();
                return !string.IsNullOrWhiteSpace(obj.ToString()) ? Convert.ToDecimal(obj) : 0;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Cartera_GetValorAbonoPagoTotal");
                throw exception;
            }
        }

        /// <summary>
        /// OBtiene los documentos de movimiento de cartera
        /// </summary>
        /// <param name="clienteMov">Filtro del cliente</param>
        /// <param name="libranza">Filtro del libranza</param>
        /// <param name="fechaInt">Filtro del fechaIni</param>
        /// <param name="fechaFin">Filtro del fechaFin</param>
        /// <param name="pagaduria">Filtro de la pagaduria</param>
        /// <param name="tipoMovimiento">Filtro del tipoMov</param>
        /// <returns>Lista de documentos</returns> 
        public List<DTO_QueryCarteraMvto> DAL_Cartera_GetMvto(string clienteMov, string NroCredito, DateTime fechaInt, DateTime fechaFin, int tipoMovimiento, int tipoAnulado)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                List<DTO_QueryCarteraMvto> results = new List<DTO_QueryCarteraMvto>();
                string where = string.Empty;

                #region Query
                mySqlCommand.CommandText =
                        " SELECT	Mvto.NumeroDoc, " +
                                    " mvto.NumCredito,  " +
                                    " mvto.NroCredito,   " +
                                    " mvto.DocumentoID, " +
                                    " mvto.Fecha_Movimiento, " +
                                    " mvto.ClienteID,   " +
                                    " mvto.Nom_Cliente,  " +
                                    " mvto.descripcion,  " +
                                    " mvto.FechaAplicacion, " +
                                    " mvto.PrefDoc,   " +
                                    " mvto.Comprobante, " +
                                    " mvto.TotalDocumento,  " +
                                    " mvto.AbonoDocumento, " +
                                    " mvto.TotalCuota,   " +
                                    " mvto.AbonoCuota, " +
                                    " mov.ComponenteCarteraID, " +
                                    " com.Descriptivo as ComponenteDesc " +
                        " FROM " +
                                " (Select	mov.NumeroDoc,		 " +
                                            " mov.NumCredito,  " +
                                            " crd.Libranza as NroCredito,  " +
                                            " doc.DocumentoID,  " +
                                            " doc.FechaDoc as Fecha_Movimiento, " +
                                            " crd.ClienteID,  " +
                                            " cli.descriptivo as Nom_Cliente,  " +
                                            " doc.descripcion, left(doc.observacion, 15) as FechaAplicacion, " +
                                            " CAST(RTRIM (doc.PrefijoID) +   ' - ' + CONVERT(varchar,doc.DocumentoNro)     AS VARCHAR(20)) AS PrefDoc, " +
                                            " CAST(RTRIM (doc.ComprobanteID) + ' - '  + CONVERT(varchar,doc.ComprobanteIDNro) AS varchar(30)) AS Comprobante,     " +
                                            " SUM(VlrComponente) as TotalDocumento, " +
                                            " SUM(VlrAbono)  as AbonoDocumento, " +
                                            " SUM(case when com.TipoComponente = 1 or com.TipoComponente = 4 then mov.VlrComponente else 0 end) as TotalCuota, " +
                                            " SUM(case when com.TipoComponente = 1 or com.TipoComponente = 4 then mov.VlrAbono else 0 end)   as AbonoCuota " +
                                    " FROM ccCarteraMvto Mov " +
                                            " left join ccCreditoDocu  crd on crd.NumeroDoc = mov.NumCredito " +
                                            " left join glDocumentoControl doc on doc.NumeroDoc = mov.numeroDoc " +
                                            " left join ccCliente   cli on crd.ClienteID = cli.ClienteID " +
                                            " left JOIN ccCarteraComponente com ON com.ComponenteCarteraID = mov.ComponenteCarteraID and mov.eg_ccCarteraComponente = com.EmpresaGrupoID " +
                                    " WHERE   doc.EmpresaID = @EmpresaID and   " +
                                            " ((@Cliente is null) or (crd.ClienteID = @Cliente)) and  " +
                                            " ((@Libranza is null)or (crd.Libranza  = @Libranza)) and  " +
                                            " ((@TipoMovimiento = 0 ) and  " +
                                            " (doc.DocumentoID = 161 or doc.DocumentoID = 90161 or " +
                                            " doc.DocumentoID = 166 or doc.DocumentoID = 90166 or " +
                                            " doc.DocumentoID = 167 or doc.DocumentoID = 90167 or " +
                                            " doc.DocumentoID = 168 or doc.DocumentoID = 90168 or " +
                                            " doc.DocumentoID = 172 or doc.DocumentoID = 90172) or   " +
                                            " ((@TipoMovimiento  <> 0 ) and  " +
                                            " (doc.DocumentoID = @TipoMovimiento  OR doc.DocumentoID = @TipoMovAnula))) AND  " +
                                            " ((@FechaInicial is null) or (doc.FechaDoc >= @FechaInicial)) and " +
                                            " ((@FechaFinal is null)   or (doc.FechaDoc <= @FechaFinal))  " +
                                    " GROUP BY  " +
                                            " mov.NumeroDoc, mov.NumCredito, crd.Libranza, doc.DocumentoID, doc.FechaDoc, " +
                                            " crd.ClienteID, cli.descriptivo, doc.descripcion,doc.observacion, " +
                                            " CAST(RTRIM (doc.PrefijoID) +   ' - ' + CONVERT(varchar,doc.DocumentoNro)     AS VARCHAR(20)), " +
                                            " CAST(RTRIM (doc.ComprobanteID) + ' - '  + CONVERT(varchar,doc.ComprobanteIDNro) AS varchar(30))) as mvto " +
                        " INNER JOIN ccCarteraMvto  Mov ON mov.NumeroDoc = mvto.NumeroDoc " +
                        " INNER JOIN ccCarteraComponente com ON com.ComponenteCarteraID = mov.ComponenteCarteraID and mov.eg_ccCarteraComponente = com.EmpresaGrupoID " +
                        " ORDER BY    " +
                                " Mvto.Fecha_Movimiento " ;
                #endregion
                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Cliente", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@FechaInicial", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFinal", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TipoMovimiento", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoMovAnula", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Cliente"].Value = !string.IsNullOrEmpty(clienteMov) ? clienteMov : null;
                mySqlCommand.Parameters["@Libranza"].Value = !string.IsNullOrWhiteSpace(NroCredito) ? NroCredito : null;
                mySqlCommand.Parameters["@FechaInicial"].Value = fechaInt.Date;
                mySqlCommand.Parameters["@FechaFinal"].Value = fechaFin.Date;
                mySqlCommand.Parameters["@TipoMovimiento"].Value = !string.IsNullOrWhiteSpace(tipoMovimiento.ToString()) ? tipoMovimiento : 0;
                mySqlCommand.Parameters["@TipoMovAnula"].Value = !string.IsNullOrWhiteSpace(tipoAnulado.ToString()) ? tipoAnulado : 0;
                #endregion
                #region Foreach .. Convierte los Nulls de SQL a Null de VS
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                } 
                #endregion

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    int numDocCred = Convert.ToInt32(dr["NumCredito"]);
                    bool nuevo = true;
                    DTO_QueryCarteraMvto dto = new DTO_QueryCarteraMvto(dr,false);
                    List<DTO_QueryCarteraMvto> list = results.Where(x => ((DTO_QueryCarteraMvto)x).NumCredito.Value.Value.Equals(numDocCred)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_QueryCarteraMvto(dr,false);
                    }

                    DTO_QueryCarteraMvtoDeta dtoDet = new DTO_QueryCarteraMvtoDeta(dr);
                    dto.Detalle.Add(dtoDet);

                    if (nuevo)
                        results.Add(dto);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DTO_QueryCarteraMvto");
                throw exception;
            }
        }

        /// <summary>
        /// OBtiene los documentos de movimiento de cartera
        /// </summary>
        /// <param name="clienteMov">Filtro del cliente</param>
        /// <param name="libranza">Filtro del libranza</param>
        /// <param name="fechaInt">Filtro del fechaIni</param>
        /// <param name="fechaFin">Filtro del fechaFin</param>
        /// <param name="pagaduria">Filtro de la pagaduria</param>
        /// <param name="tipoMovimiento">Filtro del tipoMov</param>
        /// <returns>Lista de documentos</returns> 
        public List<DTO_QueryGestionCobranza> DAL_Cartera_GestionCobranzaGetActividades(DateTime fechaFin,DTO_glIncumplimientoEtapa etapa, string clienteID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                List<DTO_QueryGestionCobranza> results = new List<DTO_QueryGestionCobranza>();
                string where = string.Empty;

                if (!string.IsNullOrEmpty(clienteID))
                {
                    mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char,UDT_ClienteID.MaxLength);
                    mySqlCommand.Parameters["@ClienteID"].Value = clienteID;
                    where = " and crd.ClienteID = @ClienteID ";
                }
                if (etapa.EstadoDeuda.Value == 4)
                    where = " and DATEDIFF(day,query2.FechaCuota, @FechaCorte) > 0 and query2.EstadoCartera in (4,5,6)";
                else
                    where = " and DATEDIFF(day,query2.FechaCuota, @FechaCorte) > 0 and query2.EstadoCartera not in (4,5,6)";

                #region Query
                mySqlCommand.CommandText =
                        " Select * from " +
                        " ( " +
                        "     Select query1.CuotaID,query1.FechaCuota,dateadd(day, @DiasFin,query1.FechaCuota) as FechaAct, crd.NumeroDoc,crd.EmpresaID,  crd.Libranza, crd.ClienteID,cli.Descriptivo as Nombre ,cli.EstadoCartera,crd.TipoEstado, crd.CobranzaGestionCierre," +
                        "            query1.VlrCuota - query1.VlrPagadoCuota as VlrSaldoCuota,gest.EtapaIncumplimiento as EtapaID " +
                        "     from ccCreditoDocu crd " +
                        "         Left join ( Select pp.numeroDoc,min(CuotaID) as CuotaID,min(FechaCuota)as FechaCuota,min(VlrCuota)as VlrCuota,min(VlrPagadoCuota)as VlrPagadoCuota " +
                        "                         from  ccCreditoPlanPagos pp	" +
                        "                         Where  pp.VlrPagadoCuota < pp.VlrCuota and pp.FechaCuota <=  @FechaCorte " +
                        "                         group by pp.numeroDoc " +
                        "                     ) query1 on query1.NumeroDoc = crd.numeroDoc	" +
                        "         Left join ccCliente cli on crd.ClienteID = cli.ClienteID " +
                        "         Left join ccCobranzaGestion gest on gest.CobranzaGestionID = crd.CobranzaGestionCierre  " +
                        "     ) query2 " +
                        "  where query2.EmpresaID = @EmpresaID and  query2.EtapaID = @EtapaID  " + where +
                        "  Order by FechaAct, Nombre " ;

                #endregion
                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaCorte", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@DiasFin", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EtapaID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaCorte"].Value = fechaFin;              
                mySqlCommand.Parameters["@EtapaID"].Value = etapa.ID.Value;
                mySqlCommand.Parameters["@DiasFin"].Value = etapa.DiasInicio.Value;    
                #region Valida campos nulls
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                #endregion 
                #endregion
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_QueryGestionCobranza dto = new DTO_QueryGestionCobranza(dr);
                    dto.EtapaDesc.Value = etapa.Descriptivo.Value;
                    results.Add(dto);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Cartera_GestionCobranzaGetActividades");
                throw exception;
            }
        }

        #endregion
    }
}
