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
    public class DAL_cfCierreMes : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transacfion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_cfCierreMes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla cfCierreMes
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private void DAL_cfCierreMes_AddItem(DTO_ccCierreDia cierreDia)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                int mes = cierreDia.Periodo.Value.Value.Month;
                string mesStr = mes.ToString();
                if (mesStr.Length == 1)
                    mesStr = "0" + mesStr;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO cfCierreMes " +
                    "( " +
                        "EmpresaID,Año,DocumentoID,LineaCreditoID,AsesorID,ZonaID,CompradorCarteraID,Plazo,TipoDato,ValorMes" + mesStr + "," +
                        "eg_cfLineaCredito,eg_cfAsesor,eg_glZona,eg_cfCompradorCartera" +
                    ") " +
                    "VALUES " +
                    "( " +
                        "@EmpresaID,@Año,@DocumentoID,@LineaCreditoID,@AsesorID,@ZonaID,@CompradorCarteraID,@Plazo,@TipoDato,@ValorMes," +
                        "@eg_cfLineaCredito,@eg_cfAsesor,@eg_glZona,@eg_cfCompradorCartera" +
                    ") ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@TipoDato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ValorMes", SqlDbType.Decimal);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_cfLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_cfAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_cfCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Año"].Value = cierreDia.Periodo.Value.Value.Year; ;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = cierreDia.DocumentoID.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = cierreDia.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = cierreDia.AsesorID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = cierreDia.ZonaID.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cierreDia.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = cierreDia.Plazo.Value;
                mySqlCommandSel.Parameters["@TipoDato"].Value = cierreDia.TipoDato.Value;
                mySqlCommandSel.Parameters["@ValorMes"].Value = cierreDia.ValorDia01.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_cfLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfLineaCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_cfAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfAsesor, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_cfCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfCompradorCartera, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cfCierreMes_Add", false);
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_cfCierreMes_UpdateItem(DTO_ccCierreDia cierre)
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
                    "UPDATE cfCierreMes SET ValorMes" + mesStr + " = @ValorMes " +
                    "WHERE EmpresaID= @EmpresaID AND Año= @Año AND DocumentoID= @DocumentoID AND LineaCreditoID= @LineaCreditoID " +
                    "   AND AsesorID= @AsesorID AND ZonaID= @ZonaID AND Plazo=@Plazo AND TipoDato= @TipoDato";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cfCierreMes_UpdateItem", false);
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de cfCierreMes
        /// </summary>
        /// <param name="cierreDia">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_cfCierreMes_Add(DTO_ccCierreDia cierreDia)
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
                    "SELECT COUNT (*) from cfCierreMes with(nolock) " +
                    "WHERE EmpresaID= @EmpresaID AND Año= @Año AND DocumentoID= @DocumentoID AND LineaCreditoID= @LineaCreditoID " +
                    "   AND AsesorID= @AsesorID AND ZonaID= @ZonaID AND Plazo=@Plazo AND TipoDato= @TipoDato";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
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
                    this.DAL_cfCierreMes_AddItem(cierreDia);
                else
                    this.DAL_cfCierreMes_UpdateItem(cierreDia);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "Err_AddData", false);
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
        public List<DTO_ccCierreMes> DAL_cfCierreMes_GetAll(Int16 año)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreMes> cierres = new List<DTO_ccCierreMes>();
                #region Query

                mySqlCommand.CommandText =
                    "select * " +
                    "from cfCierreMes with(nolock) " +
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
                    DTO_ccCierreMes cierre = new DTO_ccCierreMes(dr, false);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cfCierreMes_GetAll", false);
                throw exception;
            }

        }

        /// <summary>
        /// Trae la lista de creditos que pagaron la totalidad de la cuota 1 en el periodo selecfionado
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public List<Tuple<int, decimal, decimal, DateTime, DateTime?>> DAL_cfCierreMes_GetSaldoIntAnt(string compIntAnt, DateTime periodoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);
                List<Tuple<int, decimal, decimal, DateTime, DateTime?>> results = new List<Tuple<int, decimal, decimal, DateTime, DateTime?>>();
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_cfCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@ComponenteCarteraID"].Value = compIntAnt;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@eg_cfCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfCarteraComponente, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select comps.NumeroDoc, TotalValor, AbonoValor, FechaCuota, ctrlPago.FechaDoc as FechaPago " +
                    "from cfCreditoComponentes comps with(nolock) " +
                    "   inner join glDocumentoControl ctrl with(nolock) on comps.NumeroDoc = ctrl.NumeroDoc and ctrl.PeriodoDoc <= @PeriodoID " + 
                    "	inner join cfCreditoPlanPagos pp with(nolock) on comps.NumeroDoc = pp.NumeroDoc and pp.CuotaID = 1 " +
                    "	left join cfCreditoPagos pag with(nolock) on pp.Consecutivo = pag.CreditoCuotaNum " +
                    "	left join glDocumentoControl ctrlPago with(nolock) on pag.PagoDocu = ctrlPago.NumeroDoc " +
                    "where comps.ComponenteCarteraID = @ComponenteCarteraID and " +
                    "	eg_cfCarteraComponente = @eg_cfCarteraComponente and TotalValor > AbonoValor";

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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cfCierreMes_GetSaldoIntAnt", false);
                throw exception;
            }

        }

        #endregion

        #region cfCierreMesCartera

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public object DAL_cfCierreMesCartera_Procesar(DateTime periodo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_ccCierreMesCartera> cierres = new List<DTO_ccCierreMesCartera>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_cfCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_cfGestionCobranza", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@eg_cfCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfCompradorCartera, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_cfGestionCobranza"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfGestionCobranza, this.Empresa, egCtrl);

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
                result.ResultMessage = Mentor_Exception.LogException(ex, this.UserId.ToString(), "DAL_cfCierreMes_Procesar", false);
                return result;
            }
        }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public DTO_ccCierreMesCartera DAL_cfCierreMesCartera_GetByCreditoMes(int numeroDocCredito, DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                DTO_ccCierreMesCartera cierre = null;
                #region Query

                mySqlCommand.CommandText =
                "select * from cfCierreMesCartera with(nolock) " +
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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cfCierreMesCartera_GetAll", false);
                throw exception;
            }

        }

        /// <summary>
        /// Carga todos los cierres mes con uno o varios filtros
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns></returns>
        public List<DTO_ccCierreMesCartera> DAL_cfCierreMesCartera_GetByParameter(DTO_ccCierreMesCartera filter)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_ccCierreMesCartera> cierres = new List<DTO_ccCierreMesCartera>(); ;
                string query;
                bool filterInd = false;

                query = "select * from cfCierreMesCartera with(nolock) where EmpresaID = @EmpresaID ";

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
                if (!string.IsNullOrEmpty(filter.GestionCobranzaID.Value))
                {
                    query += "and GestionCobranzaID = @GestionCobranzaID ";
                    mySqlCommand.Parameters.Add("@GestionCobranzaID", SqlDbType.Char, UDT_GestionCobranzaID.MaxLength);
                    mySqlCommand.Parameters["@GestionCobranzaID"].Value = filter.GestionCobranzaID.Value;
                    filterInd = true;
                }
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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_cfCierreMesCartera_GetAll", false);
                throw exception;
            }

        } 

        #endregion
    }
}
