using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_ccCierreMes : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCierreMes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccCierreMes
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private void DAL_ccCierreMes_AddItem(DTO_ccCierreDia cierreDia)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                int mes = cierreDia.Periodo.Value.Value.Month;
                string mesStr = mes.ToString();
                if (mesStr.Length == 1)
                    mesStr = "0" + mesStr;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccCierreMes " +
                    "( " +
                        "EmpresaID,Año,DocumentoID,LineaCreditoID,AsesorID,CentroPagoID,ZonaID,CompradorCarteraID,Plazo,TipoDato,ValorMes" + mesStr + "," +
                        "eg_ccLineaCredito,eg_ccAsesor,eg_ccCentroPagoPAG,eg_glZona,eg_ccCompradorCartera" +
                    ") " +
                    "VALUES " +
                    "( " +
                        "@EmpresaID,@Año,@DocumentoID,@LineaCreditoID,@AsesorID,@CentroPagoID,@ZonaID,@CompradorCarteraID,@Plazo,@TipoDato,@ValorMes," +
                        "@eg_ccLineaCredito,@eg_ccAsesor,@eg_ccCentroPagoPAG,@eg_glZona,@eg_ccCompradorCartera" +
                    ") ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ValorMes", SqlDbType.Decimal);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_ccLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Año"].Value = cierreDia.Periodo.Value.Value.Year; ;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierreDia.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierreDia.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierreDia.AsesorID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = cierreDia.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierreDia.ZonaID.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierreDia.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierreDia.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierreDia.TipoDato.Value;
                mySqlCommandSel.Parameters["@ValorMes"].Value = cierreDia.ValorDia01.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_ccLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAsesor, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMes_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_ccCierreMes_UpdateItem(DTO_ccCierreDia cierre)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                int mes = cierre.Periodo.Value.Value.Month;
                string mesStr = mes.ToString();
                if (mesStr.Length == 1)
                    mesStr = "0" + mesStr;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE ccCierreMes SET ValorMes" + mesStr + " = @ValorMes " +
                    "WHERE EmpresaID= @EmpresaID AND Año= @Año AND DocumentoID= @DocumentoID AND LineaCreditoID= @LineaCreditoID " +
                    "   AND AsesorID= @AsesorID AND CentroPagoID= @CentroPagoID AND ZonaID= @ZonaID AND Plazo=@Plazo AND TipoDato= @TipoDato";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ValorMes", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Año"].Value = cierre.Periodo.Value.Value.Year;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierre.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierre.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierre.AsesorID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = cierre.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierre.ZonaID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierre.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierre.TipoDato.Value;
                mySqlCommandSel.Parameters["@ValorMes"].Value = cierre.ValorDia01.Value;
                #endregion
                #region Campos null

                if (string.IsNullOrWhiteSpace(cierre.CompradorCarteraID.Value))
                    mySqlCommandSel.CommandText += " AND CompradorCarteraID IS NULL ";
                else
                {
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierre.CompradorCarteraID.Value;

                    mySqlCommandSel.CommandText += " AND CompradorCarteraID = @CompradorCarteraID ";
                }

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
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMes_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de ccCierreMes
        /// </summary>
        /// <param name="cierreDia">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_ccCierreMes_Add(DTO_ccCierreDia cierreDia)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                DateTime periodo = cierreDia.Periodo.Value.Value;
                #region Verifica si tiene datos
                if (!cierreDia.ValorDia01.Value.HasValue || cierreDia.ValorDia01.Value.Value == 0)
                    return;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from ccCierreMes with(nolock) " +
                    "WHERE EmpresaID= @EmpresaID AND Año= @Año AND DocumentoID= @DocumentoID AND LineaCreditoID= @LineaCreditoID " +
                    "   AND AsesorID= @AsesorID AND CentroPagoID= @CentroPagoID AND ZonaID= @ZonaID AND Plazo=@Plazo AND TipoDato= @TipoDato";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Año"].Value = cierreDia.Periodo.Value.Value.Year;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierreDia.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierreDia.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierreDia.AsesorID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = cierreDia.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierreDia.ZonaID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierreDia.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierreDia.TipoDato.Value;

                #endregion
                #region Campos null

                if (string.IsNullOrWhiteSpace(cierreDia.CompradorCarteraID.Value))
                    mySqlCommandSel.CommandText += " AND CompradorCarteraID IS NULL ";
                else
                {
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierreDia.CompradorCarteraID.Value;

                    mySqlCommandSel.CommandText += " AND CompradorCarteraID = @CompradorCarteraID ";
                }

                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_ccCierreMes_AddItem(cierreDia);
                else
                    this.DAL_ccCierreMes_UpdateItem(cierreDia);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Err_AddData");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public List<DTO_ccCierreMes> DAL_ccCierreMes_GetAll(Int16 año)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreMes> cierres = new List<DTO_ccCierreMes>();
                #region Query

                mySqlCommand.CommandText =
                    "select * " +
                    "from ccCierreMes with(nolock) " +
                    "where EmpresaID = @EmpresaID AND Año = @Año ";

                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Año", SqlDbType.SmallInt);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Año"].Value = año;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCierreMes cierre = new DTO_ccCierreMes(dr);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMes_GetAll");
                throw exception;
            }

        }

        /// <summary>
        /// Trae la lista de creditos que pagaron la totalidad de la cuota 1 en el periodo seleccionado
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public List<Tuple<int, decimal, decimal, DateTime, DateTime?>> DAL_ccCierreMes_GetSaldoIntAnt(string compIntAnt, DateTime periodoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<Tuple<int, decimal, decimal, DateTime, DateTime?>> results = new List<Tuple<int, decimal, decimal, DateTime, DateTime?>>();
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@ComponenteCarteraID"].Value = compIntAnt;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select comps.NumeroDoc, TotalValor, AbonoValor, FechaCuota, ctrlPago.FechaDoc as FechaPago " +
                    "from ccCreditoComponentes comps with(nolock) " +
                    "   inner join glDocumentoControl ctrl with(nolock) on comps.NumeroDoc = ctrl.NumeroDoc and ctrl.PeriodoDoc <= @PeriodoID " + 
                    "	inner join ccCreditoPlanPagos pp with(nolock) on comps.NumeroDoc = pp.NumeroDoc and pp.CuotaID = 1 " +
                    "	left join ccCreditoPagos pag with(nolock) on pp.Consecutivo = pag.CreditoCuotaNum " +
                    "	left join glDocumentoControl ctrlPago with(nolock) on pag.PagoDocu = ctrlPago.NumeroDoc " +
                    "where comps.ComponenteCarteraID = @ComponenteCarteraID and " +
                    "	eg_ccCarteraComponente = @eg_ccCarteraComponente and TotalValor > AbonoValor";

                List<int> docs = new List<int>();

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);

                    if (!docs.Contains(numDoc))
                    {
                        docs.Add(numDoc);

                        decimal total = Convert.ToDecimal(dr["TotalValor"]);
                        decimal abono = Convert.ToDecimal(dr["AbonoValor"]);
                        DateTime fechaCuota = Convert.ToDateTime(dr["FechaCuota"]);
                        
                        DateTime? fechaPago = null;
                        if (!string.IsNullOrWhiteSpace(dr["FechaPago"].ToString()))
                            fechaPago = Convert.ToDateTime(dr["FechaPago"]);

                        Tuple<int, decimal, decimal, DateTime, DateTime?> t = new Tuple<int, decimal, decimal, DateTime, DateTime?>(numDoc, total, abono, fechaCuota, fechaPago);
                        results.Add(t);
                    }
                }
                dr.Close();


                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMes_GetSaldoIntAnt");
                throw exception;
            }

        }

        #endregion

        #region ccCierreMesCartera

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public object DAL_ccCierreMesCartera_Procesar(DateTime periodo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_ccCierreMesCartera> cierres = new List<DTO_ccCierreMesCartera>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandTimeout = 0;

                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);                
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength); 
                mySqlCommand.Parameters.Add("@eg_ccClasificacionxRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccComponenteCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glActividadFlujo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glIncumplimientoEtapa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccClasificacionxRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccClasificacionxRiesgo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccComponenteCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccComponenteCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glActividadFlujo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glIncumplimientoEtapa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glIncumplimientoEtapa, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Cartera_CierreMensual";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                while (dr.Read())
                {
                    if (dr.GetName(0) == "Linea")
                    {
                        #region Carga el error
                        result.Result = ResultValue.NOK;

                        rd = new DTO_TxResultDetail();
                        rd.line = Convert.ToInt32(dr["Linea"]);

                        switch (dr["CodigoError"].ToString())
                        {
                            case "001": // Falta datos en glControl
                                rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
                                break;

                            case "002": // No hay información para clasificar los riesgos 
                                rd.Message = DictionaryMessages.Err_Cc_NoInfoRiesgos + "&&" + dr["Valor"].ToString();
                                break;

                            case "003": // Dias de vencimiento en clasificación de riesgo = 0 
                                rd.Message = DictionaryMessages.Err_Cc_DiasRango0 + "&&" + dr["Valor"].ToString();
                                break;

                            default: // (999): Error del SP 
                                rd.Message = dr["Valor"].ToString();
                                break;
                        }

                        #endregion

                        result.Details.Add(rd);
                    }
                    else
                    {
                        DTO_ccCierreMesCartera det = new DTO_ccCierreMesCartera(dr);
                        cierres.Add(det);
                    }
                }
                dr.Close();

                if (result.Result == ResultValue.NOK)
                    return result;
                else
                    return cierres;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_ccCierreMes_Procesar");
                return result;
            }
        }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public object DAL_ccCierreMesCarteraFin_Procesar(DateTime periodo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_ccCierreMesCartera> cierres = new List<DTO_ccCierreMesCartera>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandTimeout = 0;

                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccClasificacionxRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccComponenteCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccClasificacionxRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccClasificacionxRiesgo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccComponenteCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccComponenteCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "CarteraFin_CierreMensual";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                while (dr.Read())
                {
                    if (dr.GetName(0) == "Linea")
                    {
                        #region Carga el error
                        result.Result = ResultValue.NOK;

                        rd = new DTO_TxResultDetail();
                        rd.line = Convert.ToInt32(dr["Linea"]);

                        switch (dr["CodigoError"].ToString())
                        {
                            case "001": // Falta datos en glControl
                                rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
                                break;

                            case "002": // No hay información para clasificar los riesgos 
                                rd.Message = DictionaryMessages.Err_Cc_NoInfoRiesgos + "&&" + dr["Valor"].ToString();
                                break;

                            case "003": // Dias de vencimiento en clasificación de riesgo = 0 
                                rd.Message = DictionaryMessages.Err_Cc_DiasRango0 + "&&" + dr["Valor"].ToString();
                                break;

                            default: // (999): Error del SP 
                                rd.Message = dr["Valor"].ToString();
                                break;
                        }

                        #endregion

                        result.Details.Add(rd);
                    }
                    else
                    {
                        DTO_ccCierreMesCartera det = new DTO_ccCierreMesCartera(dr);
                        cierres.Add(det);
                    }
                }
                dr.Close();

                if (result.Result == ResultValue.NOK)
                    return result;
                else
                    return cierres;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_ccCierreMes_Procesar");
                return result;
            }
        }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public DTO_ccCierreMesCartera DAL_ccCierreMesCartera_GetByCreditoMes(int numeroDocCredito, DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                DTO_ccCierreMesCartera cierre = null;
                #region Query

                mySqlCommand.CommandText =
                "select * from ccCierreMesCartera with(nolock) " +
                "where EmpresaID = @EmpresaID AND Periodo = @Periodo AND @NumeroDoc = NumeroDoc";

                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDocCredito;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    cierre = new DTO_ccCierreMesCartera(dr);
                }
                dr.Close();

                return cierre;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMesCartera_GetAll");
                throw exception;
            }

        }

        /// <summary>
        /// Carga todos los cierres mes con uno o varios filtros
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns></returns>
        public List<DTO_ccCierreMesCartera> DAL_ccCierreMesCartera_GetByParameter(DTO_ccCierreMesCartera filter)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreMesCartera> cierres = new List<DTO_ccCierreMesCartera>(); ;
                string query;
                bool filterInd = false;

                query = "select * from ccCierreMesCartera with(nolock) where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CompradorCarteraID.Value))
                {
                    query += "and CompradorCarteraID = @CompradorCarteraID ";
                    mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommand.Parameters["@CompradorCarteraID"].Value = filter.CompradorCarteraID.Value;
                    filterInd = true;
                }
                //if (!string.IsNullOrEmpty(filter.GestionCobranzaID.Value))
                //{
                //    query += "and GestionCobranzaID = @GestionCobranzaID ";
                //    mySqlCommand.Parameters.Add("@GestionCobranzaID", SqlDbType.Char, UDT_GestionCobranzaID.MaxLength);
                //    mySqlCommand.Parameters["@GestionCobranzaID"].Value = filter.GestionCobranzaID.Value;
                //    filterInd = true;
                //}
                if (!string.IsNullOrEmpty(filter.Periodo.Value.ToString()))
                {
                    query += "and Periodo = @Periodo ";
                    mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@Periodo"].Value = filter.Periodo.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TipoEstado.Value.ToString()))
                {
                    query += "and TipoEstado = @TipoEstado ";
                    mySqlCommand.Parameters.Add("@TipoEstado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@TipoEstado"].Value = filter.TipoEstado.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.EstadoDeuda.Value.ToString()))
                {
                    query += "and EstadoDeuda = @EstadoDeuda ";
                    mySqlCommand.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@EstadoDeuda"].Value = filter.EstadoDeuda.Value;
                    filterInd = true;
                }
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return cierres;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ccCierreMesCartera cierre = new DTO_ccCierreMesCartera(dr);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMesCartera_GetAll");
                throw exception;
            }

        }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public List<DTO_CentralRiesgoMes> DAL_ccCierreMesCartera_GetCierreCentralRiesgo(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_CentralRiesgoMes> cierres = new List<DTO_CentralRiesgoMes>();
                #region Query
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Cartera_GetCierresCentralRiesgo";
                
                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                #endregion

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_CentralRiesgoMes cierre = new DTO_CentralRiesgoMes(dr);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCierreMesCartera_GetDatacreditoMes");
                throw exception;
            }

        }


        #endregion
    }
}
