using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_Contabilidad : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Contabilidad(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Migracion

        /// <summary>
        /// Borra todos los documentos de un periodo
        /// </summary>
        /// <param name="periodo">Periodo del cual se desea eliminar la info</param>
        public void DAL_Contabilidad_BorraInfoPeriodo(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;

                //Elimina la info de glDocumentoControl
                mySqlCommand.CommandText = "Delete from glDocumentoControl WHERE EmpresaID = @EmpresaID AND PeriodoDoc = @PeriodoDoc";
                mySqlCommand.ExecuteNonQuery();

                //Elimina la info de los comprobantes
                mySqlCommand.CommandText = "Delete from coAuxiliar WHERE EmpresaID = @EmpresaID AND PeriodoID = @PeriodoDoc";
                mySqlCommand.ExecuteNonQuery();

                //Elimina los movimientos de los saldos
                mySqlCommand.CommandText = "Update coCuentaSaldo SET " +
                        "   DbOrigenLocML = 0, DbOrigenExtML = 0, DbOrigenLocME = 0, DbOrigenExtME = 0," +
                        "   CrOrigenLocML = 0, CrOrigenExtML = 0, CrOrigenLocME = 0, CrOrigenExtME = 0 " +
                        "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoDoc";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_BorraInfoPeriodo");
                throw exception;
            }
        }

        /// <summary>
        /// Valida la existencia comprobantes en algun periodo
        /// </summary>
        /// <param name="periodo">Periodo del cual se desea consultar la info</param>
        /// <param name="comps">Info de los comprobantes a consultar</param>
        public int DAL_Contabilidad_UltimoComprobanteNro(DateTime periodo, string compID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = compID;

                //Elimina la info de los comprobantes
                mySqlCommand.CommandText = 
                    "select top(1) ComprobanteIDNro from gldocumentocontrol with(nolock) " +
                    "WHERE EmpresaID = @EmpresaID AND PeriodoDoc = @PeriodoDoc AND ComprobanteID=@ComprobanteID " + 
                    "order by ComprobanteIDNro desc";

                int count = 0;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                try
                {
                    if (dr.Read())
                        count = Convert.ToInt32(dr["ComprobanteIDNro"]);
                }
                finally
                {
                    dr.Close();
                }

                return count;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_BorraInfoPeriodo");
                throw exception;
            }
        }

        #endregion

        #region Saldos

        #region Traer Saldos

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal DAL_Contabilidad_GetSaldoByDocumentoCuenta(bool isML, DateTime PeriodoID, long idTR, string cuentaID, string libroID)
        {
            try
            {
                decimal result = 0;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = PeriodoID;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = idTR;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select " +
                    "	sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoML, " +
                    "	sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoME " +
                    "from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and IdentificadorTR = @IdentificadorTR and CuentaID = @CuentaID " +
                    "  and BalanceTipoID = @BalanceTipoID and eg_coPlanCuenta = @eg_coPlanCuenta and eg_coBalanceTipo = @eg_coBalanceTipo";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                try
                {
                    if (dr.Read())
                    {
                        if (string.IsNullOrWhiteSpace(dr["SaldoML"].ToString()))
                            result = 0;
                        else
                            result = isML ? Convert.ToDecimal(dr["SaldoML"]) : Convert.ToDecimal(dr["SaldoME"]);
                    }
                }
                finally
                {
                    dr.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldosByPeriodoCuenta");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal DAL_Contabilidad_GetSaldoByPeriodoCuenta(bool isML, DateTime PeriodoID, string cuentaID, string libroID, List<int> excludeIdTRs = null)
        {
            try
            {
                decimal result = 0;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = PeriodoID;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                string Idtrs = string.Empty;
                if (excludeIdTRs != null)
                {
                    for (int i = 0; i < excludeIdTRs.Count; ++i)
                    {
                        if (i == 0)
                            Idtrs = " and IdentificadorTR not in(" + excludeIdTRs[i].ToString();
                        else
                            Idtrs += "," + excludeIdTRs[i].ToString();

                        if (i == excludeIdTRs.Count - 1)
                            Idtrs += ")";
                    }
                }

                mySqlCommand.CommandText =
                    "select " +
                    "	sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoML, " +
                    "	sum(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME + DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) AS SaldoME " +
                    "from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and CuentaID = @CuentaID " +
                    "  and BalanceTipoID = @BalanceTipoID and eg_coPlanCuenta = @eg_coPlanCuenta and eg_coBalanceTipo = @eg_coBalanceTipo" + Idtrs;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                try
                {
                    if (dr.Read())
                    {
                        if (isML && !string.IsNullOrWhiteSpace(dr["SaldoML"].ToString()))
                            result = Convert.ToDecimal(dr["SaldoML"]);
                        else if (!isML && !string.IsNullOrWhiteSpace(dr["SaldoME"].ToString()))
                            result = Convert.ToDecimal(dr["SaldoME"]);
                    }
                }
                finally
                {
                    dr.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldosByPeriodoCuenta");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="libroID">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        public DTO_coCuentaSaldo DAL_Contabilidad_GetSaldoByDocumento(DateTime periodo, string cuentaID, string concSaldo, long identificadorTR, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                DTO_coCuentaSaldo saldo = null;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = concSaldo;
                mySqlCommandSel.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommandSel.Parameters["@IdentificadorTR"].Value = identificadorTR;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "select * from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and ConceptoSaldoID = @ConceptoSaldoID" +
                    "   and IdentificadorTR = @IdentificadorTR and BalanceTipoID=@BalanceTipoID and eg_coBalanceTipo=@eg_coBalanceTipo ";

                if (!string.IsNullOrWhiteSpace(cuentaID))
                   mySqlCommandSel.CommandText += " and CuentaID = @CuentaID";

                SqlDataReader dr;
                dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                    saldo = new DTO_coCuentaSaldo(dr);

                dr.Close();

                return saldo;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetSaldo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los saldos de un periodo dada la cuenta
        /// </summary>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <param name="fromMaster">Indica si la consulta es solo en el periodo actual (false) o tambien se toman periodos superiores (true)</param>
        /// <returns>Retornala lista de saldos</returns>
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetSaldosByPeriodoCuenta(DateTime PeriodoID, string cuentaID, bool fromMaster, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = PeriodoID;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select * " +
                    "from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and CuentaID = @CuentaID " +
                    "  and BalanceTipoID = @BalanceTipoID and eg_coPlanCuenta = @eg_coPlanCuenta and eg_coBalanceTipo = @eg_coBalanceTipo ";

                if (!fromMaster)
                    mySqlCommand.CommandText += " and PeriodoID = @PeriodoID";
                else
                    mySqlCommand.CommandText += " and PeriodoID >= @PeriodoID";

                List<DTO_coCuentaSaldo> saldos = new List<DTO_coCuentaSaldo>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                    saldos.Add(saldo);
                }
                dr.Close();

                return saldos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetSaldosByPeriodoCuenta");
                throw exception;
            }
        }
                
        /// <summary>
        /// Genera los comprobantes y saldos para el ajuste en cambio
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <returns></returns>
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetSaldosForAjusteEnCambio(DateTime periodo, TipoMoneda tipoIteracion, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_coCuentaSaldo> saldosML = new List<DTO_coCuentaSaldo>();
                List<DTO_coCuentaSaldo> saldosME = new List<DTO_coCuentaSaldo>();

                mySqlCommand.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@OrigenMonetario"].Value = tipoIteracion == TipoMoneda.Local ? (int)TipoMoneda.Foreign : (int)TipoMoneda.Local;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                List<DTO_coCuentaSaldo> saldosTemp = new List<DTO_coCuentaSaldo>();
                string query =
                    "select coCuentaSaldo.* " + 
                    "from coCuentaSaldo with(nolock) " +
                    "   INNER JOIN coPlanCuenta with(nolock) ON coCuentaSaldo.CuentaID = coPlanCuenta.CuentaID " +
                    "       AND coPlanCuenta.OrigenMonetario=@OrigenMonetario AND coCuentaSaldo.eg_coPlanCuenta = coPlanCuenta.EmpresaGrupoID " +
                    "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID AND BalanceTipoID=@BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo";

                mySqlCommand.CommandText = query;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                        saldosTemp.Add(saldo);
                    }
                }
                finally
                {
                    dr.Close();
                }

                return saldosTemp;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetSaldosForAjusteEnCambio");
                throw exception;
            }
        }

        /// <summary>
        /// Genera los comprobantes y saldos para el cierre anual
        /// </summary>
        /// <param name="ctasResultado">Indica si se tienen que traer las cuentas relacionadas con el nit</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="libroID">Tipo de balance del cual se deben traer los saldos</param>
        /// <returns></returns>
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetSaldosForCierreAnual(bool ctasResultado, DateTime periodo, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string queryCta = string.Empty;

                List<DTO_coCuentaSaldo> saldosML = new List<DTO_coCuentaSaldo>();
                List<DTO_coCuentaSaldo> saldosME = new List<DTO_coCuentaSaldo>();

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                if (ctasResultado)
                {
                    mySqlCommand.Parameters.Add("@Tipo", SqlDbType.Char, 1);
                    mySqlCommand.Parameters["@Tipo"].Value = "2";

                    queryCta = "AND cta.Tipo=@Tipo ";
                }
                else
                {
                    queryCta = "AND cta.NITCierreAnual IS NOT NULL ";
                }

                List<DTO_coCuentaSaldo> saldosTemp = new List<DTO_coCuentaSaldo>();
                string query =
                    "select cs.* " + 
                    "from coCuentaSaldo cs with(nolock) " +
                    "   INNER JOIN coPlanCuenta cta with(nolock) ON cs.CuentaID = cta.CuentaID AND cs.eg_coPlanCuenta = cta.EmpresaGrupoID " + queryCta +
                    "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID and BalanceTipoID = @BalanceTipoID and cs.eg_coBalanceTipo = @eg_coBalanceTipo";

                mySqlCommand.CommandText = query;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                        saldosTemp.Add(saldo);
                    }
                }
                finally
                {
                    dr.Close();
                }

                return saldosTemp;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetSaldosForCierreAnual");
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
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetSaldosForReclasificacion(DateTime periodo, DTO_coReclasificaBalance origen, List<DTO_coReclasificaBalExcluye> excls, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros generales

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                #endregion

                bool isFirst = true;
                string query = string.Empty;
                string queryGeneral = "select * from coCuentaSaldo with(nolock) ";
                string whereGeneral = "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo ";

                #region Where general

                //Cuenta
                if (!string.IsNullOrWhiteSpace(origen.CuentaORIG.Value))
                {
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaID"].Value = origen.CuentaORIG.Value;

                    whereGeneral += " and CuentaID LIKE @CuentaID + '%' and eg_coPlanCuenta = @eg_coPlanCuenta ";
                }

                //CentroCosto
                if (!string.IsNullOrWhiteSpace(origen.CtoCostoORIG.Value))
                {
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = origen.CtoCostoORIG.Value;

                    whereGeneral += " and CentroCostoID LIKE @CentroCostoID + '%' and eg_coCentroCosto = @eg_coCentroCosto ";
                }

                //Proyecto
                if (!string.IsNullOrWhiteSpace(origen.ProyectoORIG.Value))
                {
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = origen.ProyectoORIG.Value;

                    whereGeneral += " and ProyectoID LIKE @ProyectoID + '%' and eg_coProyecto = @eg_coProyecto ";
                }
                #endregion
                #region Query con exclusiones
                if (excls.Count == 0)
                    query = queryGeneral + whereGeneral;
                else
                {
                    int i = 0;
                    isFirst = true;
                    string whereExcl = string.Empty;
                    foreach (DTO_coReclasificaBalExcluye exc in excls)
                    {
                        if (!isFirst)
                            query += " UNION ";

                        #region Where exclusiones
                        whereExcl = string.Empty;

                        //Cuenta
                        if (!string.IsNullOrWhiteSpace(origen.CuentaORIG.Value))
                        {
                            mySqlCommand.Parameters.Add("@CuentaID" + i.ToString(), SqlDbType.Char, UDT_CuentaID.MaxLength);
                            mySqlCommand.Parameters["@CuentaID" + i.ToString()].Value = origen.CuentaORIG.Value;

                            whereExcl += " and CuentaID not LIKE @CuentaID" + i.ToString() + " + '%' and eg_coPlanCuenta = @eg_coPlanCuenta ";
                        }

                        //CentroCosto
                        if (!string.IsNullOrWhiteSpace(origen.CtoCostoORIG.Value))
                        {
                            mySqlCommand.Parameters.Add("@CentroCostoID" + i.ToString(), SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                            mySqlCommand.Parameters["@CentroCostoID" + i.ToString()].Value = origen.CtoCostoORIG.Value;

                            whereExcl += " and CentroCostoID not LIKE @CentroCostoID" + i.ToString() + " + '%' and eg_coCentroCosto = @eg_coCentroCosto ";
                        }

                        //Proyecto
                        if (!string.IsNullOrWhiteSpace(origen.ProyectoORIG.Value))
                        {
                            mySqlCommand.Parameters.Add("@ProyectoID" + i.ToString(), SqlDbType.Char, UDT_ProyectoID.MaxLength);
                            mySqlCommand.Parameters["@ProyectoID" + i.ToString()].Value = origen.ProyectoORIG.Value;

                            whereExcl += " and ProyectoID not LIKE @ProyectoID" + i.ToString() + " + '%' and eg_coProyecto = @eg_coProyecto ";
                        }
                        #endregion

                        query += queryGeneral + whereGeneral + whereExcl;

                        isFirst = false;
                        i++;
                    }
                }
                #endregion

                mySqlCommand.CommandText = query;


                #region Realiza la consulta
                List<DTO_coCuentaSaldo> saldos = new List<DTO_coCuentaSaldo>();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                }

                dr.Close();


                #endregion

                return saldos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Comprobante_GetForDistribucion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los saldosn de acuerdo a los filtros de la vista de resumen
        /// </summary>
        /// <param name="PeriodoID">Periodo</param>
        /// <param name="libroID">Tipo Balance</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="centroCostoID">Centro Costo</param>
        /// <param name="lineaPresupuestoID">Linea Presupuesto</param>
        /// <param name="conceptoSaldoID">Concepto Saldo</param>
        /// <param name="conceptoCargoID">Concept Cargo</param>
        /// <param name="identificadorTr">Identificador Tr</param>
        /// <returns></returns>
        public List<DTO_SaldosVista> DAL_Contabilidad_GetSaldosResumen(DateTime PeriodoID, string libroID, string cuentaID, string terceroID, string proyectoID,
                                                         string centroCostoID, string lineaPresupuestoID, string conceptoSaldoID, string conceptoCargoID,
                                                         int? identificadorTr)
        {
            try
            {
                SqlCommand mySqlCommand = new SqlCommand("Contabilidad_RepSaldos", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);                
                
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = PeriodoID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@ProyectoID"].Value = proyectoID;
                mySqlCommand.Parameters["@CentroCostoID"].Value = centroCostoID;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = lineaPresupuestoID;
                mySqlCommand.Parameters["@ConceptoSaldoID"].Value = conceptoSaldoID;
                mySqlCommand.Parameters["@ConceptoCargoID"].Value = conceptoCargoID;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = identificadorTr;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                List<DTO_SaldosVista> result = new List<DTO_SaldosVista>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_SaldosVista dto = new DTO_SaldosVista(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldosByPeriodoCuenta");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los saldosn de acuerdo a los filtros de la vista de resumen
        /// </summary>
        /// <param name="PeriodoID">Periodo</param>
        /// <param name="libroID">Tipo Balance</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="centroCostoID">Centro Costo</param>
        /// <param name="lineaPresupuestoID">Linea Presupuesto</param>
        /// <param name="conceptoSaldoID">Concepto Saldo</param>
        /// <param name="conceptoCargoID">Concept Cargo</param>
        /// <param name="identificadorTr">Identificador Tr</param>
        /// <returns></returns>
        public List<DTO_SaldosVista> DAL_Contabilidad_GetSaldosByLineaRep(DateTime PeriodoIni, DateTime PeriodoFin, string libroID, string cuentaID, string terceroID, string proyectoID,
                                                         string centroCostoID, string lineaPresupuestoID, string conceptoSaldoID, string conceptoCargoID,
                                                         int? identificadorTr)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@PeriodoIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PeriodoFin", SqlDbType.DateTime);               
                mySqlCommand.Parameters["@PeriodoIni"].Value = PeriodoIni;
                mySqlCommand.Parameters["@PeriodoFin"].Value = PeriodoFin;
                string query = string.Empty;

                if (!string.IsNullOrEmpty(libroID))
                {
                    query += " and BalanceTipoID = @BalanceTipoID ";
                    mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                }
                if (!string.IsNullOrEmpty(cuentaID))
                {
                    query += " and CuentaID like '" + cuentaID + "%' ";
                    //mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    //mySqlCommand.Parameters["@CuentaID"].Value = cuentaID+ "%";
                    //mySqlCommand.Parameters.AddWithValue("@CuentaID", cuentaID + "'%'");
                }
                if (!string.IsNullOrEmpty(terceroID))
                {
                    query += " and TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                }
                if (!string.IsNullOrEmpty(proyectoID))
                {
                    query += " and ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = proyectoID;
                }
                if (!string.IsNullOrEmpty(centroCostoID))
                {
                    query += "and CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = centroCostoID;
                }
                if (!string.IsNullOrEmpty(lineaPresupuestoID))
                {
                    query += "and LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = lineaPresupuestoID;
                }
                if (!string.IsNullOrEmpty(conceptoSaldoID))
                {
                    query += "and ConceptoSaldoID = @ConceptoSaldoID ";
                    mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                    mySqlCommand.Parameters["@ConceptoSaldoID"].Value = conceptoSaldoID;
                }
                if (!string.IsNullOrEmpty(conceptoCargoID))
                {
                    query += "and ConceptoCargoID = @ConceptoCargoID ";
                    mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                    mySqlCommand.Parameters["@ConceptoCargoID"].Value = conceptoCargoID;
                }
                if (identificadorTr != null)
                {
                    query += "and IdentificadorTR = @IdentificadorTR ";
                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = identificadorTr;
                }             

                mySqlCommand.CommandText =
                 "  SELECT EmpresaID, PeriodoID,BalanceTipoID,CuentaID,TerceroID,ProyectoID,CentroCostoID, LineaPresupuestoID,ConceptoSaldoID,ConceptoCargoID,IdentificadorTR, " +
                 "   SUM(DbOrigenLocML + DbOrigenExtML) DebitoML, " +
                 "   SUM(DbOrigenLocME + DbOrigenExtME) DebitoME, " +
                 "   SUM(CrOrigenLocML + CrOrigenExtML) CreditoML, " +
                 "   SUM(CrOrigenLocME + CrOrigenExtME) CreditoME, " +
                 "   SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) SaldoIniML, " +
                 "   SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) SaldoIniME " +
                 "   FROM coCuentaSaldo WITH(NOLOCK) " +
                 "   WHERE EmpresaID = @EmpresaID and (PeriodoID BETWEEN @PeriodoIni AND @PeriodoFin)  " + query +
                 "   GROUP BY EmpresaID, PeriodoID, BalanceTipoID, CuentaID, TerceroID, ProyectoID, CentroCostoID, LineaPresupuestoID, ConceptoSaldoID, ConceptoCargoID,IdentificadorTR  ";

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                List<DTO_SaldosVista> result = new List<DTO_SaldosVista>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_SaldosVista dto = new DTO_SaldosVista(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldosByPeriodoCuenta");
                throw exception;
            }
        }


        /// <summary>
        /// Trae los saldos de un periodo de presupuesto
        /// </summary>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <param name="libroID">Identificador del Libro</param>
        /// <param name="proyectoID">Identificador del proyecto</param>
        /// <param name="centroCostoID">Identificador del centroCosto</param>
        /// <param name="LineaPresup">Identificador de la linea presup</param>
        /// <returns>Retornala lista de saldos</returns>
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetSaldosForPresupuesto(DateTime PeriodoID, string cuentaID, string libroID, string proyectoID, string centroCtoID, string lineaPresID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = PeriodoID;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@ProyectoID"].Value = proyectoID;
                mySqlCommand.Parameters["@CentroCostoID"].Value = centroCtoID;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = lineaPresID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select * " +
                    "from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and CuentaID = @CuentaID  and BalanceTipoID = @BalanceTipoID " +
                    "  and ProyectoID = @ProyectoID and CentroCostoID = @CentroCostoID and LineaPresupuestoID = @LineaPresupuestoID " +
                    "  and eg_coPlanCuenta = @eg_coPlanCuenta and eg_coBalanceTipo = @eg_coBalanceTipo " +
                    "  and eg_coProyecto = @eg_coProyecto and eg_coCentroCosto = @eg_coCentroCosto and eg_plLineaPresupuesto = @eg_plLineaPresupuesto ";

                List<DTO_coCuentaSaldo> saldos = new List<DTO_coCuentaSaldo>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                    saldos.Add(saldo);
                }
                dr.Close();

                return saldos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetSaldosForPresupuesto");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetByParameter(DTO_coCuentaSaldo filter)
        {
            try
            {
                List<DTO_coCuentaSaldo> result = new List<DTO_coCuentaSaldo>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from coCuentaSaldo with(nolock) where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
             
                if (!string.IsNullOrEmpty(filter.PeriodoID.Value.ToString()))
                {
                    query += "and PeriodoID = @PeriodoID ";
                    mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.Date);
                    mySqlCommand.Parameters["@PeriodoID"].Value = filter.PeriodoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.BalanceTipoID.Value.ToString()))
                {
                    query += "and BalanceTipoID = @BalanceTipoID ";
                    mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@BalanceTipoID"].Value = filter.BalanceTipoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CuentaID.Value.ToString()))
                {
                    query += "and CuentaID = @CuentaID ";
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaID"].Value = filter.CuentaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value))
                {
                    query += "and CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LineaPresupuestoID.Value.ToString()))
                {
                    query += "and LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = filter.LineaPresupuestoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConceptoSaldoID.Value.ToString()))
                {
                    query += "and ConceptoSaldoID = @ConceptoSaldoID ";
                    mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                    mySqlCommand.Parameters["@ConceptoSaldoID"].Value = filter.ConceptoSaldoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConceptoCargoID.Value.ToString()))
                {
                    query += "and ConceptoCargoID = @ConceptoCargoID ";
                    mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                    mySqlCommand.Parameters["@ConceptoCargoID"].Value = filter.ConceptoCargoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.IdentificadorTR.Value.ToString()))
                {
                    query += "and IdentificadorTR = @IdentificadorTR ";
                    mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = filter.IdentificadorTR.Value;
                    filterInd = true;
                }  
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_coCuentaSaldo ctrl = new DTO_coCuentaSaldo(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        public List<DTO_coCuentaSaldo> DAL_Contabilidad_GetByTerceroCartera(DateTime periodoID, string terceroID, string cuentaCapital, string libroID)
        {
            try
            {
                List<DTO_coCuentaSaldo> result = new List<DTO_coCuentaSaldo>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from coCuentaSaldo sdo with(nolock)  " +
                        " where sdo.EmpresaID = @EmpresaID and sdo.BalanceTipoID = @BalanceTipoID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;

                if (periodoID != null)
                {
                    query += " and sdo.PeriodoID = @PeriodoID ";
                    mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(terceroID))
                {
                    query += " and sdo.TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                    filterInd = true;
                }
                //else if (!string.IsNullOrEmpty(cuentaCapital) && !string.IsNullOrEmpty(cuentaInteres))
                //{
                //    query += "and (sdo.CuentaID = @CuentaCapital or sdo.CuentaID = @CuentaInteres) ";
                //    mySqlCommand.Parameters.Add("@CuentaCapital", SqlDbType.Char, UDT_CuentaID.MaxLength);
                //    mySqlCommand.Parameters["@CuentaCapital"].Value = cuentaCapital;
                //    mySqlCommand.Parameters.Add("@CuentaInteres", SqlDbType.Char, UDT_CuentaID.MaxLength);
                //    mySqlCommand.Parameters["@CuentaInteres"].Value = cuentaInteres;
                //    filterInd = true;
                //}
                if (!string.IsNullOrEmpty(cuentaCapital))
                {
                    query += " and sdo.CuentaID = @CuentaCapital ";
                    mySqlCommand.Parameters.Add("@CuentaCapital", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaCapital"].Value = cuentaCapital;
                    filterInd = true;
                }               
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_coCuentaSaldo ctrl = new DTO_coCuentaSaldo(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetByParameter");
                throw exception;
            }
        }

        #endregion

        #region Existencia de saldos

        /// <summary>
        /// Valida si existe informacion en los saldos con el tipo de balance preliminar
        /// </summary>
        /// <returns>Retorna el numero de registros encontrados</returns>
        public int DAL_Contabilidad_SaldoExistsPreliminares(string libroPreliminar)
        {
            SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
            mySqlCommand.Transaction = base.MySqlConnectionTx;

            mySqlCommand.CommandText = "select COUNT(*) from coCuentaSaldo with(nolock) where EmpresaID=@EmpresaID AND BalanceTipoID=@BalanceTipoID";

            mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
            mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

            mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
            mySqlCommand.Parameters["@BalanceTipoID"].Value = libroPreliminar;


            int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
            return count;
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="idTR">Llave de busqueda</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>true si existe</returns>
        public bool DAL_Contabilidad_SaldoExistsByIdentificadorTR(long idTR, DateTime PeriodoID, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = idTR.ToString();
                mySqlCommand.Parameters["@PeriodoID"].Value = PeriodoID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                            "SELECT count(*) FROM coCuentaSaldo with(nolock) " +
                            "WHERE EmpresaID=@EmpresaID and IdentificadorTR=@IdentificadorTR and PeriodoID=@PeriodoID and " +
                            "  and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo and" +
                            "   (DbOrigenLocML <> 0 or " +
                            "   DbOrigenExtML <> 0 or " +
                            "   DbOrigenLocME <> 0 or " +
                            "   DbOrigenExtME <> 0 or " +
                            "   CrOrigenLocML <> 0 or " +
                            "   CrOrigenExtML <> 0 or " +
                            "   CrOrigenLocME <> 0 or " +
                            "   CrOrigenExtME <> 0) ";

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldoExistsByIdentificadorTR");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si hay conceptos de saldo en uso teniendo en cuenta la CuentaID
        /// </summary>
        /// <param name="conceptoSaldoID">Id de concepto saldo</param>
        /// <param name="cuentaID">Id de la cuenta relacionada</param>
        /// <returns>true si existe</returns>
        public bool DAL_Contabilidad_SaldoExistsByCtaConcSaldo(string conceptoSaldoID, string cuentaID, string libroID)
        {
            try
            {
                string query = string.Empty;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ConceptoSaldoID"].Value = conceptoSaldoID;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                query = "SELECT count(*) FROM coCuentaSaldo with(nolock) " +
                            "WHERE EmpresaID=@EmpresaID and ConceptoSaldoID=@ConceptoSaldoID " +
                            "  and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo"; 
                
                if (cuentaID != null)
                {
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                    mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                    mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                    query += " AND CuentaID=@CuentaID and eg_coPlanCuenta = @eg_coPlanCuenta";
                }

                mySqlCommand.CommandText = query;
                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());

                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldoExists");
                throw exception;
            }
        }

        /// <summary>
        /// Dice si el balance esta de acuerdo con los saldos
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="longCuenta">Longitud del primer nivel para el plan de cuentas</param>
        /// <returns></returns>
        public bool DAL_Contabilidad_SaldosExistsForBalance(DateTime periodo, int longCuenta)
        {
            try
            {
                bool validData = true;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;

                mySqlCommand.CommandText =
                    "select saldos.*, balance.CuentaID as cuentaID_bal, balance.dbLocML as dbLocML_bal, balance.dbExtML as dbExtML_bal from " +
                    "( " +
                    "	select q.CuentaID, SUM(DbOrigenLocML) as dbLocML, SUM(DbOrigenExtML) as dbExtML " +
                    "	from " +
                    "	( " +
                    "		select PeriodoID, BalanceTipoID, SUBSTRING (CuentaID, 1, " + longCuenta.ToString() + ") as CuentaID, DbOrigenLocML, DbOrigenExtML " +
                    "		from cocuentasaldo with(nolock) " +
                    "		where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID" +
                    "	) as q " +
                    "	group by CuentaID " +
                    ") as saldos left join " +
                    "( " +
                    "	select CuentaID, SUM(DbOrigenLocML) as dbLocML, SUM(DbOrigenExtML) as dbExtML " +
                    "	from cobalance with(nolock) " +
                    "	where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and CuentaID = SUBSTRING (CuentaID, 1, " + longCuenta.ToString() + ") " +
                    "	group by CuentaID " +
                    ") as balance on saldos.CuentaID = balance.CuentaID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["cuentaID_bal"] == null || string.IsNullOrWhiteSpace(dr["cuentaID_bal"].ToString()))
                        validData = false;
                    else
                    {
                        decimal saldosLoc = Convert.ToDecimal(dr["dbLocML"]);
                        decimal saldosExt = Convert.ToDecimal(dr["dbExtML"]);
                        decimal balLoc = Convert.ToDecimal(dr["dbLocML_bal"]);
                        decimal balExt = Convert.ToDecimal(dr["dbExtML_bal"]);

                        if (saldosLoc != balLoc || saldosExt != balExt)
                            validData = false;
                    }

                    if (!validData)
                        break;
                }
                dr.Close();

                return validData ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_BalanceCompleto");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si el componente capital de un credito tiene saldos
        /// </summary>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <param name="concSaldoCapital">Concepto de slado del componente capital</param>
        /// <param name="idTR">Llave de busqueda</param>
        /// <returns>true si existe</returns>
        public bool DAL_Contabilidad_CreditoHasSaldo(DateTime periodoID, string concSaldoCapital, string concSaldoSeguro, int idTR, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID_Capital", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID_Seguro", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@ConceptoSaldoID_Capital"].Value = concSaldoCapital;
                mySqlCommand.Parameters["@ConceptoSaldoID_Seguro"].Value = concSaldoSeguro;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = idTR;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "SELECT CASE WHEN (SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML)) is  not null " +
                    "       THEN (SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML))  ELSE 0 END  " +
                    "FROM coCuentaSaldo saldo with(nolock) " +
                    "WHERE EmpresaID = @EmpresaID AND PeriodoID = @PeriodoID AND IdentificadorTR = @IdentificadorTR " +
                    "  and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo = @eg_coBalanceTipo AND ConceptoSaldoID IN(@ConceptoSaldoID_Capital, @ConceptoSaldoID_Seguro) ";

                decimal sum = Convert.ToDecimal(mySqlCommand.ExecuteScalar());
                return sum != 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_SaldoExistsByIdentificadorTR");
                throw exception;
            }
        }

        /// <summary>
        /// Revisa si un documento ha tenido movimientos de saldos despues de su creación
        /// </summary>
        /// <param name="idTR">Identificador del documento</param>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta del documento</param>
        /// <param name="libroID">Libro de consulta</param>
        /// <returns>Retorna true si ha tenido nuevos movimientos, de lo contrario false</returns>
        public bool DAL_Contabilidad_HasMovimiento(int idTR, DateTime periodoID, DTO_coPlanCuenta cta, string libroID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = idTR;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@CuentaID"].Value = cta.ID.Value;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select " +
                    "	SUM(DbOrigenLocML+DbOrigenLocME+DbOrigenExtML+DbOrigenExtME) AS DB, " +
                    "	SUM(CrOrigenLocML+CrOrigenLocME+CrOrigenExtML+CrOrigenExtME) AS CR " +
                    "from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @PeriodoID and BalanceTipoID = @BalanceTipoID and CuentaID = @CuentaID and IdentificadorTR = @IdentificadorTR " +
                    "	and eg_coBalanceTipo = @eg_coBalanceTipo and eg_coPlanCuenta = @eg_coPlanCuenta ";

                decimal db = 0;
                decimal cr = 0;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    db = string.IsNullOrWhiteSpace(dr["DB"].ToString()) ? 0 : Convert.ToDecimal(dr["DB"]);
                    cr = string.IsNullOrWhiteSpace(dr["CR"].ToString()) ? 0 : Convert.ToDecimal(dr["CR"]);
                }
                dr.Close();

                //Debito
                if (cta.Naturaleza.Value.Value == 1 && Math.Abs(cr) > 0)
                    return true;
                else if(cta.Naturaleza.Value.Value == 2 && Math.Abs(db) > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_DocumentoConMovimiento");
                throw exception;
            }
        }

        #endregion

        #region Operaciones con saldos

        /// <summary>
        /// Genera los saldos de un comprobante
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="mod">Modulo de operacion</param>
        /// <param name="comprobante">Comprobante que genera los saldos</param>
        /// <param name="libroID">Tipo de balance</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DAL_Contabilidad_GenerarSaldos(int documentID, ModulesPrefix mod, DTO_Comprobante comprobante, string libroID)
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

                #region Carga la tabla del footer
                DataTable table = new DataTable();
                table.Columns.Add("Consecutivo", typeof(int));
                table.Columns.Add("CuentaID", typeof(string));
                table.Columns.Add("CuentaAlternaID", typeof(string));
                table.Columns.Add("TerceroID", typeof(string));
                table.Columns.Add("ProyectoID", typeof(string));
                table.Columns.Add("CentroCostoID", typeof(string));
                table.Columns.Add("LineaPresupuestoID", typeof(string));
                table.Columns.Add("ConceptoCargoID", typeof(string));
                table.Columns.Add("LugarGeograficoID", typeof(string));
                table.Columns.Add("PrefijoCOM", typeof(string));
                table.Columns.Add("DocumentoCOM", typeof(string));
                table.Columns.Add("ActivoCOM", typeof(string));
                table.Columns.Add("ConceptoSaldoID", typeof(string));
                table.Columns.Add("IdentificadorTR", typeof(long));
                table.Columns.Add("Descriptivo", typeof(string));
                table.Columns.Add("TasaCambio", typeof(decimal));
                table.Columns.Add("vlrBaseML", typeof(decimal));
                table.Columns.Add("vlrBaseME", typeof(decimal));
                table.Columns.Add("vlrMdaLoc", typeof(decimal));
                table.Columns.Add("vlrMdaExt", typeof(decimal));
                table.Columns.Add("vlrMdaOtr", typeof(decimal));
                table.Columns.Add("DatoAdd1", typeof(string));
                table.Columns.Add("DatoAdd2", typeof(string));
                table.Columns.Add("DatoAdd3", typeof(string));
                table.Columns.Add("DatoAdd4", typeof(string));
                table.Columns.Add("DatoAdd5", typeof(string));
                table.Columns.Add("DatoAdd6", typeof(string));
                table.Columns.Add("DatoAdd7", typeof(string));
                table.Columns.Add("DatoAdd8", typeof(string));
                table.Columns.Add("DatoAdd9", typeof(string));
                table.Columns.Add("DatoAdd10", typeof(string));
                #endregion
                #region Lleno los datos del footer
                for (int i = 0; i < comprobante.Footer.Count; ++i)
                {
                    DTO_ComprobanteFooter det = comprobante.Footer[i];

                    DataRow fila = table.NewRow();
                    fila["Consecutivo"] = i + 1;
                    fila["CuentaID"] = det.CuentaID.Value;
                    fila["CuentaAlternaID"] = det.CuentaAlternaID.Value;
                    fila["TerceroID"] = det.TerceroID.Value;
                    fila["ProyectoID"] = det.ProyectoID.Value;
                    fila["CentroCostoID"] = det.CentroCostoID.Value;
                    fila["LineaPresupuestoID"] = det.LineaPresupuestoID.Value;
                    fila["ConceptoCargoID"] = det.ConceptoCargoID.Value;
                    fila["LugarGeograficoID"] = det.LugarGeograficoID.Value;
                    fila["PrefijoCOM"] = det.PrefijoCOM.Value;
                    fila["DocumentoCOM"] = det.DocumentoCOM.Value;
                    fila["ActivoCOM"] = det.ActivoCOM.Value;
                    fila["ConceptoSaldoID"] = det.ConceptoSaldoID.Value;
                    fila["IdentificadorTR"] = det.IdentificadorTR.Value;
                    fila["Descriptivo"] = det.Descriptivo.Value;
                    fila["TasaCambio"] = det.TasaCambio.Value;
                    fila["vlrBaseML"] = det.vlrBaseML.Value;
                    fila["vlrBaseME"] = det.vlrBaseME.Value;
                    fila["vlrMdaLoc"] = det.vlrMdaLoc.Value;
                    fila["vlrMdaExt"] = det.vlrMdaExt.Value;
                    fila["vlrMdaOtr"] = det.vlrMdaOtr.Value;
                    fila["DatoAdd1"] = det.DatoAdd1.Value;
                    fila["DatoAdd2"] = det.DatoAdd2.Value;
                    fila["DatoAdd3"] = det.DatoAdd3.Value;
                    fila["DatoAdd4"] = det.DatoAdd4.Value;
                    fila["DatoAdd5"] = det.DatoAdd5.Value;
                    fila["DatoAdd6"] = det.DatoAdd6.Value;
                    fila["DatoAdd7"] = det.DatoAdd7.Value;
                    fila["DatoAdd8"] = det.DatoAdd8.Value;
                    fila["DatoAdd9"] = det.DatoAdd9.Value;
                    fila["DatoAdd10"] = det.DatoAdd10.Value;

                    table.Rows.Add(fila);
                }
                #endregion
                #region Creación de parametros

                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@MdaTransacc", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coControl", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coComprBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuentaAlterna", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plActividadLineaPresupuestal", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                //Parametro del footer (lista)
                mySqlCommand.Parameters.Add("@Footer", SqlDbType.Structured);
                mySqlCommand.Parameters["@Footer"].TypeName = "ComprobanteFooter";

                #endregion
                #region Asignacion de parametros

                DTO_ComprobanteHeader header = comprobante.Header;

                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@ModuloID"].Value = mod;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = header.PeriodoID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = header.Fecha.Value;
                mySqlCommand.Parameters["@MdaOrigen"].Value = header.MdaOrigen.Value;
                mySqlCommand.Parameters["@MdaTransacc"].Value = header.MdaTransacc.Value;
                mySqlCommand.Parameters["@ComprobanteID"].Value = header.ComprobanteID.Value;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommand.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coControl"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coControl, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plActividadLineaPresupuestal"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plActividadLineaPresupuestal, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coComprBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuentaAlterna"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@Footer"].Value = table;

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Contabilidad_GenerarSaldos";
              
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;
                while (dr.Read())
                {
                    result.Result = ResultValue.NOK;

                    DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
                    line = Convert.ToInt32(dr["Linea"]);

                    if (line != rd.line)
                    {
                        if (addLine)
                            result.Details.Add(rd);

                        addLine = true;
                        rd = new DTO_TxResultDetail();
                        rd.line = line;
                        rd.Message = "NOK";
                        rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                    }

                    #region Carga el resultados con los mensajes de error
                    switch (dr["CodigoError"].ToString())
                    {
                        case "001": // Falta datos en glControl
                            rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
                            break;
                        case "002": // Llave fk inconsistente
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.FkNotFound + "&&" + dr["Valor"].ToString();
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "003": // Periodo en cierre
                            rd.Message = DictionaryMessages.Err_PeriodoEnCierre;
                            break;
                        case "004": // Periodo cerrado
                            rd.Message = DictionaryMessages.Err_PeriodoCerrado;
                            break;
                        case "005": // Fecha no corresponde con el periodo
                            rd.Message = DictionaryMessages.Err_InvalidDatePeriod;
                            break;
                        case "006": // Sumatoria de creditos y debitos incorrecta
                            rd.Message = DictionaryMessages.Err_Co_InvalidDebCred;
                            break;
                        case "007": // Cuenta Con módulo cerrado 
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.Err_Co_CtaPeriodClosed;
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "008": // Siguiente periodo con saldos
                            DateTime nextP = Convert.ToDateTime(dr["Valor"]);
                            rd.Message = DictionaryMessages.Err_ModWithSaldos + "&&" + nextP.ToString(FormatString.Period);
                            break;
                        case "009": // Cuenta de inventarios en modulo diferente
                            rd.Message = DictionaryMessages.Err_Co_CtaInv;
                            break;
                        case "010": // Operacion Vacia
                            rd.Message = DictionaryMessages.Err_OperacionIsNullorEmpty;
                            break;
                        case "011": // Control de consistencia contable
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.Err_Co_InvalidCtaCtoCostoOp + "&&" + dr["Valor"].ToString();
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "012": // Control de consistencia presupuestal
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.Err_Co_InvalidActLineaPres + "&&" + dr["Valor"].ToString();
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "013": // Control de consistencia fiscal (impuestos)
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.Err_Co_InvalidImpValue;
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "014": // Valor debe ser mayor a 0
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.PositiveValue + "&&" + rdf.Field;
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "015": // Cuenta débito con valor negativo
                            rdf.Field = dr["Campo"].ToString();

                            string[] fields15 = dr["Valor"].ToString().Split(new string[] { "&&" }, StringSplitOptions.None);
                            string cuenta15 = fields15[0].Trim();
                            string tercero15 = fields15[1].Trim();
                            string documentoCOM15 = fields15[2].Trim();

                            rdf.Message = DictionaryMessages.Err_SaldoUpdCtaDeb + "&&" + cuenta15 + "&&" + tercero15 + "&&" + documentoCOM15;
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "016": // Cuenta crédito con valor positivo
                            rdf.Field = dr["Campo"].ToString();
                           
                            string[] fields16 = dr["Valor"].ToString().Split(new string[] { "&&" }, StringSplitOptions.None);
                            string cuenta16 = fields16[0].Trim();
                            string tercero16 = fields16[1].Trim();
                            string documentoCOM16 = fields16[2].Trim();

                            rdf.Message = DictionaryMessages.Err_SaldoUpdCtaCre + "&&" + cuenta16 + "&&" + tercero16 + "&&" + documentoCOM16;
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "017": // La cuenta no puede superar el saldo inicial
                            rdf.Field = dr["Campo"].ToString();

                            string[] fields17 = dr["Valor"].ToString().Split(new string[] { "&&" }, StringSplitOptions.None);
                            string cuenta17 = fields17[0].Trim();
                            string tercero17 = fields17[1].Trim();
                            string documentoCOM17 = fields17[2].Trim();

                            rdf.Message = DictionaryMessages.Err_BalanceSaldoIni + "&&" + cuenta17 + "&&" + tercero17 + "&&" + documentoCOM17;
                            rd.DetailsFields.Add(rdf);
                            break;
                        case "018": // No se encontró el documento relacionado
                            rd.Message = DictionaryMessages.Err_NoDocument;
                            break;
                        case "019": // El proyecto no tiene una actividad relacionada	para la validación de consistencia presupuestal
                            rd.Message = DictionaryMessages.Err_Co_ProyNoAct + "&&" + dr["Valor"].ToString();
                            break;
                        case "020": // El identificador TR (componente tercero) no corresponde con el tercero ingresado en el registro
                            rdf.Field = dr["Campo"].ToString();
                            rd.Message = DictionaryMessages.Err_Co_InvalidIdentTR_Terc;
                            break;
                        case "021": //El documento actual solo permite procesar con un libro funcional, verifique la parametrizacion del comprobante
                            rdf.Field = dr["Campo"].ToString();
                            rd.Message = DictionaryMessages.Err_Co_LibroFuncionalInvalid;
                            break;
                        default: // (999): Error del SP 
                            rd.Message = dr["Valor"].ToString();
                            break;
                    }
                    #endregion
                }
                dr.Close();

                if (result.Result == ResultValue.NOK)
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Err_Co_GenerarSaldos + "&&" + comprobante.Header.ComprobanteID.Value + "&&" + 
                        comprobante.Header.PeriodoID.Value.Value.ToString(FormatString.Period);
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Contabilidad_GenerarSaldos");
                return result;
            }
        }

        /// <summary>
        /// Genera los saldos de un comprobante
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="mod">Modulo de operacion</param>
        /// <param name="comprobante">Comprobante que genera los saldos</param>
        /// <param name="libroID">Tipo de balance</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DAL_Contabilidad_SustraerSaldos(int documentID, DTO_Comprobante comprobante, string libroID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Carga la tabla del footer
                DataTable table = new DataTable();
                table.Columns.Add("Consecutivo", typeof(int));
                table.Columns.Add("CuentaID", typeof(string));
                table.Columns.Add("CuentaAlternaID", typeof(string));
                table.Columns.Add("TerceroID", typeof(string));
                table.Columns.Add("ProyectoID", typeof(string));
                table.Columns.Add("CentroCostoID", typeof(string));
                table.Columns.Add("LineaPresupuestoID", typeof(string));
                table.Columns.Add("ConceptoCargoID", typeof(string));
                table.Columns.Add("LugarGeograficoID", typeof(string));
                table.Columns.Add("PrefijoCOM", typeof(string));
                table.Columns.Add("DocumentoCOM", typeof(string));
                table.Columns.Add("ActivoCOM", typeof(string));
                table.Columns.Add("ConceptoSaldoID", typeof(string));
                table.Columns.Add("IdentificadorTR", typeof(long));
                table.Columns.Add("Descriptivo", typeof(string));
                table.Columns.Add("TasaCambio", typeof(decimal));
                table.Columns.Add("vlrBaseML", typeof(decimal));
                table.Columns.Add("vlrBaseME", typeof(decimal));
                table.Columns.Add("vlrMdaLoc", typeof(decimal));
                table.Columns.Add("vlrMdaExt", typeof(decimal));
                table.Columns.Add("vlrMdaOtr", typeof(decimal));
                table.Columns.Add("DatoAdd1", typeof(string));
                table.Columns.Add("DatoAdd2", typeof(string));
                table.Columns.Add("DatoAdd3", typeof(string));
                table.Columns.Add("DatoAdd4", typeof(string));
                table.Columns.Add("DatoAdd5", typeof(string));
                table.Columns.Add("DatoAdd6", typeof(string));
                table.Columns.Add("DatoAdd7", typeof(string));
                table.Columns.Add("DatoAdd8", typeof(string));
                table.Columns.Add("DatoAdd9", typeof(string));
                table.Columns.Add("DatoAdd10", typeof(string));
                #endregion
                #region Lleno los datos del footer
                for (int i = 0; i < comprobante.Footer.Count; ++i)
                {
                    DTO_ComprobanteFooter det = comprobante.Footer[i];

                    DataRow fila = table.NewRow();
                    fila["Consecutivo"] = i + 1;
                    fila["CuentaID"] = det.CuentaID.Value;
                    fila["CuentaAlternaID"] = det.CuentaAlternaID.Value;
                    fila["TerceroID"] = det.TerceroID.Value;
                    fila["ProyectoID"] = det.ProyectoID.Value;
                    fila["CentroCostoID"] = det.CentroCostoID.Value;
                    fila["LineaPresupuestoID"] = det.LineaPresupuestoID.Value;
                    fila["ConceptoCargoID"] = det.ConceptoCargoID.Value;
                    fila["LugarGeograficoID"] = det.LugarGeograficoID.Value;
                    fila["PrefijoCOM"] = det.PrefijoCOM.Value;
                    fila["DocumentoCOM"] = det.DocumentoCOM.Value;
                    fila["ActivoCOM"] = det.ActivoCOM.Value;
                    fila["ConceptoSaldoID"] = det.ConceptoSaldoID.Value;
                    fila["IdentificadorTR"] = det.IdentificadorTR.Value;
                    fila["Descriptivo"] = det.Descriptivo.Value;
                    fila["TasaCambio"] = det.TasaCambio.Value;
                    fila["vlrBaseML"] = det.vlrBaseML.Value;
                    fila["vlrBaseME"] = det.vlrBaseME.Value;
                    fila["vlrMdaLoc"] = det.vlrMdaLoc.Value;
                    fila["vlrMdaExt"] = det.vlrMdaExt.Value;
                    fila["vlrMdaOtr"] = det.vlrMdaOtr.Value;
                    fila["DatoAdd1"] = det.DatoAdd1.Value;
                    fila["DatoAdd2"] = det.DatoAdd2.Value;
                    fila["DatoAdd3"] = det.DatoAdd3.Value;
                    fila["DatoAdd4"] = det.DatoAdd4.Value;
                    fila["DatoAdd5"] = det.DatoAdd5.Value;
                    fila["DatoAdd6"] = det.DatoAdd6.Value;
                    fila["DatoAdd7"] = det.DatoAdd7.Value;
                    fila["DatoAdd8"] = det.DatoAdd8.Value;
                    fila["DatoAdd9"] = det.DatoAdd9.Value;
                    fila["DatoAdd10"] = det.DatoAdd10.Value;

                    table.Rows.Add(fila);
                }
                #endregion
                #region Creación de parametros

                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@LibroComprobante", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coComprBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuentaAlterna", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plActividadLineaPresupuestal", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                //Parametro del footer (lista)
                mySqlCommand.Parameters.Add("@Footer", SqlDbType.Structured);
                mySqlCommand.Parameters["@Footer"].TypeName = "ComprobanteFooter";

                #endregion
                #region Asignacion de parametros

                DTO_ComprobanteHeader header = comprobante.Header;

                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = header.PeriodoID.Value;
                mySqlCommand.Parameters["@ComprobanteID"].Value = header.ComprobanteID.Value;
                mySqlCommand.Parameters["@LibroComprobante"].Value = libroID;
                mySqlCommand.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plActividadLineaPresupuestal"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plActividadLineaPresupuestal, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coComprBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuentaAlterna"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@Footer"].Value = table;

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Contabilidad_SustraerSaldos";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;
                while (dr.Read())
                {
                    result.Result = ResultValue.NOK;

                    DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
                    line = Convert.ToInt32(dr["Linea"]);

                    if (line != rd.line)
                    {
                        if (addLine)
                            result.Details.Add(rd);

                        addLine = true;
                        rd = new DTO_TxResultDetail();
                        rd.line = line;
                        rd.Message = "NOK";
                        rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                    }

                    #region Carga el resultados con los mensajes de error
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
                }
                dr.Close();

                if (result.Result == ResultValue.NOK)
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Err_Co_SustraerSaldos + "&&" + comprobante.Header.ComprobanteID.Value + "&&" +
                        comprobante.Header.ComprobanteNro.Value.Value.ToString() + "&&" + comprobante.Header.PeriodoID.Value.Value.ToString(FormatString.Period);
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Contabilidad_GenerarSaldos");
                return result;
            }
        }

        /// <summary>
        /// Reclasifica el libro IFRS
        /// </summary>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult DAL_Contabilidad_ReclasificarIFRS(DateTime periodoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);


                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Contabilidad_ReclasificacionIFRS";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;
                while (dr.Read())
                {
                    result.Result = ResultValue.NOK;

                    DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
                    line = Convert.ToInt32(dr["Linea"]);

                    if (line != rd.line)
                    {
                        if (addLine)
                            result.Details.Add(rd);

                        addLine = true;
                        rd = new DTO_TxResultDetail();
                        rd.line = line;
                        rd.Message = "NOK";
                        rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                    }

                    #region Carga el resultados con los mensajes de error
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
                }
                dr.Close();

                if (result.Result == ResultValue.NOK)
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Err_UpdateData;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Contabilidad_ReclasificarIFRS");
                return result;
            }
        }

        /// <summary>
        /// Borra la informacion de los saldos y del balance en un periodo
        /// </summary>
        /// <param name="isMensual">Indica si elimina informacion un mes o de todo el año</param>
        /// <param name="periodo">Periodo para aliminar los datos</param>
        /// <param name="libroPreliminar">Tipo de balance preliminar</param>
        public void DAL_Contabilidad_BorrarSaldosXLibro(bool isMensual, DateTime periodo, string libroPreliminar)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = libroPreliminar;
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                string querySaldo = "DELETE FROM coCuentaSaldo where EmpresaID = @EmpresaID and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo=@eg_coBalanceTipo";
                string queryBalance = "DELETE FROM coBalance where EmpresaID = @EmpresaID and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo=@eg_coBalanceTipo";

                if (isMensual)
                {
                    mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;

                    querySaldo += " and PeriodoID = @PeriodoID";
                    queryBalance += " and PeriodoID = @PeriodoID";
                }
                else
                {
                    mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@Year"].Value = periodo.Year;

                    querySaldo += " and YEAR(PeriodoID) = @Year";
                    queryBalance += " and YEAR(PeriodoID) = @Year";
                }

                //Elimina los saldos
                mySqlCommandSel.CommandText = querySaldo;
                mySqlCommandSel.ExecuteNonQuery();

                //Elimina el balance
                mySqlCommandSel.CommandText = queryBalance;
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Delete_Register, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "BorrarDatosBalancePreliminar");
                throw exception;
            }
        }

        #endregion

        #endregion

        #region Cierres

        /// <summary>
        /// Verifica si un periodo ya esta cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        public bool DAL_Contabilidad_IsPeriodoCerrado(ModulesPrefix mod, DateTime periodo, ref bool exists)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Periodo"].Value = periodo;

                mySqlCommand.CommandText = "select * from coCierresControl with(nolock) " +
                                            "where EmpresaID=@EmpresaID and ModuloID=@ModuloID and Periodo=@Periodo";

                bool result;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    exists = true;
                    int abierto = Convert.ToInt32(dr["AbiertoInd"]);
                    if (abierto == 0)
                        result = true;
                    else
                        result = false;
                }
                else
                {
                    exists = false;
                    result = false;
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ComprobantePre_IsPeriodoCerrado");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna True si el aperiodo se puede abrir de lo contrario false</returns>
        public bool DAL_Contabilidad_UltimoMesCerrado(ModulesPrefix mod, DateTime periodo, ref bool exists)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();

                mySqlCommand.CommandText = "select * from coCierresControl with(nolock) " +
                                            "where EmpresaID=@EmpresaID and ModuloID=@ModuloID " + 
                                            "order by Periodo desc";

                bool result;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    exists = true;
                    DateTime fecha = Convert.ToDateTime(dr["Periodo"]);
                    if (fecha == periodo)
                        result = true;
                    else
                        result = false;
                }
                else
                    result = false;

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_UltimoMesCerrado");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <returns>Retorna True si el aperiodo se puede abrir de lo contrario false</returns>
        public DateTime? DAL_Contabilidad_GetUltimoMesCerrado(ModulesPrefix mod)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                DateTime? ultimoPeriodoCer = null;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();

                mySqlCommand.CommandText = "select * from coCierresControl with(nolock) " +
                                            "where EmpresaID=@EmpresaID and ModuloID=@ModuloID " +
                                            "order by Periodo desc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    ultimoPeriodoCer = Convert.ToDateTime(dr["Periodo"]);                    
                dr.Close();

                return ultimoPeriodoCer;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_UltimoMesCerrado");
                throw exception;
            }
        }

        /// <summary>
        /// Guarda en la tabla coCierre control la info de un periodo cerrado
        /// </summary>
        /// <param name="mod">Modulo que se esta cerrando</param>
        /// <param name="periodo">Periodo que se esta cerrando</param>
        public void DAL_Contabilidad_AgregarCierreControl(ModulesPrefix mod, DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select top(1) NumeroDoc from glDocumentoControl with(nolock) order by NumeroDoc desc";
                object numDoc = mySqlCommand.ExecuteScalar();
                if (numDoc != null)
                {
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                    mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                    mySqlCommand.Parameters.Add("@UltimoDoc", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@AbiertoInd", SqlDbType.TinyInt);

                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                    mySqlCommand.Parameters["@Periodo"].Value = periodo;
                    mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now;
                    mySqlCommand.Parameters["@UltimoDoc"].Value = Convert.ToInt32(numDoc);
                    mySqlCommand.Parameters["@AbiertoInd"].Value = 0;

                    mySqlCommand.CommandText = "select count (*) from coCierresControl with(nolock) " +
                            "where @EmpresaID=EmpresaID and @ModuloID=ModuloID and @Periodo=Periodo ";

                    int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                    if (count == 0)
                    {
                        mySqlCommand.CommandText = "Insert into coCierresControl(EmpresaID, ModuloID, Periodo, Fecha, UltimoDoc, AbiertoInd)" +
                            "values (@EmpresaID, @ModuloID, @Periodo, @Fecha, @UltimoDoc, @AbiertoInd)";
                    }
                    else
                    {
                        mySqlCommand.CommandText = "update coCierresControl set Fecha=@Fecha, UltimoDoc=@UltimoDoc, AbiertoInd=@AbiertoInd " +
                            "where @EmpresaID=EmpresaID and @ModuloID=ModuloID and @Periodo=Periodo";
                    }
                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ComprobantePre_AgregarCierresControl");
                throw exception;
            }
        }

        /// <summary>
        /// Hace el cierre mensual
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="modulo">Modulo de cierre</param>
        /// <returns>Retorna verdadero si el auxiliarPre tiene informacion</returns>
        public bool DAL_Contabilidad_HasPreliminares(DateTime periodo, string moduloId)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommand.Parameters["EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["ModuloID"].Value = moduloId;

                mySqlCommand.CommandText =
                    "SELECT COUNT(*) " +
                    "FROM coAuxiliarPre " +
                    "   JOIN glConceptoSaldo ON coAuxiliarPre.ConceptoSaldoID = glConceptoSaldo.ConceptoSaldoID " +
                    "       AND coAuxiliarPre.eg_glConceptoSaldo = glConceptoSaldo.EmpresaGrupoID " +
                    "WHERE coAuxiliarPre.EmpresaID=@EmpresaID AND coAuxiliarPre.PeriodoID=@PeriodoID AND UPPER(glConceptoSaldo.ModuloID)=UPPER(@ModuloID) ";

                int cantPre = (int)mySqlCommand.ExecuteScalar();
                if (cantPre > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Contabilidad_HasPreliminares");
                throw ex;
            }
        }

        /// <summary>
        /// Carga la lista de saldos para el nuevo periodo
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="modulo">Modulo de cierre</param>
        /// <param name="siguientePeriodo">Nuevo periodo</param>
        public void DAL_Contabilidad_BorrarSaldosIniciales(DateTime periodo, string moduloId)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommand.Parameters["EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["ModuloID"].Value = moduloId;
                #endregion
                #region Query
                //La primero columna (saldoNew.PeriodoID - NuevoPeriodo) indica si ese saldo existe en el siguiente periodo
                mySqlCommand.CommandText =
                    "UPDATE coCuentaSaldo SET  " +
                    "	DbSaldoIniLocML = 0, DbSaldoIniExtML = 0, DbSaldoIniLocME = 0, DbSaldoIniExtME = 0,  " +
                    "	CrSaldoIniLocML = 0, CrSaldoIniExtML = 0, CrSaldoIniLocME = 0, CrSaldoIniExtME = 0 " +
                    "FROM coCuentaSaldo saldo   " +
                    "	JOIN glConceptoSaldo ON saldo.ConceptoSaldoID = glConceptoSaldo.ConceptoSaldoID   " +
                    "		AND eg_glConceptoSaldo=glConceptoSaldo.EmpresaGrupoID   " +
                    "WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID  ";
                #endregion

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Contabilidad_BorrarSaldosIniciales");
                throw ex;
            }
        }

        /// <summary>
        /// Carga la lista de saldos para el nuevo periodo
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="modulo">Modulo de cierre</param>
        /// <param name="siguientePeriodo">Nuevo periodo</param>
        public void DAL_Contabilidad_GetSaldosCierre(DateTime periodo, string moduloId, DateTime siguientePeriodo, List<DTO_coCuentaSaldo> saldosInsert, List<DTO_coCuentaSaldo> saldosUpdate)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("NuevoPeriodo", SqlDbType.DateTime);

                mySqlCommand.Parameters["EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["ModuloID"].Value = moduloId;
                mySqlCommand.Parameters["NuevoPeriodo"].Value = siguientePeriodo;
                #endregion
                #region Query
                //La primero columna (saldoNew.PeriodoID - NuevoPeriodo) indica si ese saldo existe en el siguiente periodo
                mySqlCommand.CommandText =
                    "SELECT saldoNew.PeriodoID NuevoPeriodo, saldoOld.* " +
                    "FROM coCuentaSaldo saldoOld " +
                    "JOIN glConceptoSaldo ON saldoOld.ConceptoSaldoID = glConceptoSaldo.ConceptoSaldoID " +
                    "   AND saldoOld.eg_glConceptoSaldo=glConceptoSaldo.EmpresaGrupoID " +
                    "LEFT JOIN coCuentaSaldo saldoNew ON saldoOld.EmpresaID=saldoNew.EmpresaID " +
                    "   AND saldoOld.BalanceTipoID=saldoNew.BalanceTipoID " +
                    "   AND saldoOld.CuentaID=saldoNew.CuentaID " +
                    "   AND saldoOld.TerceroID=saldoNew.TerceroID " +
                    "   AND saldoOld.ProyectoID=saldoNew.ProyectoID " +
                    "   AND saldoOld.CentroCostoID=saldoNew.CentroCostoID " +
                    "   AND saldoOld.LineaPresupuestoID=saldoNew.LineaPresupuestoID " +
                    "   AND saldoOld.ConceptoSaldoID=saldoNew.ConceptoSaldoID " +
                    "   AND saldoOld.IdentificadorTR=saldoNew.IdentificadorTR " +
                    "   AND saldoOld.ConceptoCargoID=saldoNew.ConceptoCargoID " +
                    "   AND saldoNew.PeriodoID=@NuevoPeriodo " +
                    "WHERE saldoOld.EmpresaID=@EmpresaID AND saldoOld.PeriodoID=@PeriodoID  ";
                #endregion
                #region Carga los datos
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coCuentaSaldo saldo = new DTO_coCuentaSaldo(dr);
                    if (dr.IsDBNull(0))
                        saldosInsert.Add(saldo);
                    else
                        saldosUpdate.Add(saldo);
                }
                dr.Close();
                #endregion
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Contabilidad_GetSaldosCierre");
                throw ex;
            }
        }

        /// <summary>
        /// Hace el cierre mensual
        /// </summary>
        /// <param name="modulo">Modulo de cierre</param>
        /// <param name="siguientePeriodo">Nuevo periodo</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public void DAL_Contabilidad_HacerCierrePeriodo( DateTime siguientePeriodo, List<DTO_coCuentaSaldo> saldosInsert, List<DTO_coCuentaSaldo> saldosUpdate)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                
                #region Ingresa los nuevos saldos
                #region Definicion de parametros para insertar
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@vlBaseML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@vlBaseME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbOrigenLocML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbOrigenExtML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbOrigenLocME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbOrigenExtME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrOrigenLocML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrOrigenExtML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrOrigenLocME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrOrigenExtME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniLocML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniExtML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniLocME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniExtME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniLocML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniExtML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniLocME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniExtME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Valores de parametros generales para insertar
                mySqlCommand.Parameters["@EmpresaID"].Value = Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = siguientePeriodo;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                #endregion
                foreach (DTO_coCuentaSaldo saldoInsert in saldosInsert)
                {
                    bool insert = false;
                    #region Validacion de saldo
                    if (saldoInsert.DbOrigenLocML.Value.Value + saldoInsert.DbSaldoIniLocML.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.DbOrigenExtML.Value.Value + saldoInsert.DbSaldoIniExtML.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.DbOrigenLocME.Value.Value + saldoInsert.DbSaldoIniLocME.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.DbOrigenExtME.Value.Value + saldoInsert.DbSaldoIniExtME.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.CrOrigenLocML.Value.Value + saldoInsert.CrSaldoIniLocML.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.CrOrigenExtML.Value.Value + saldoInsert.CrSaldoIniExtML.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.CrOrigenLocME.Value.Value + saldoInsert.CrSaldoIniLocME.Value.Value != 0)
                        insert = true;
                    else if (saldoInsert.CrOrigenExtME.Value.Value + saldoInsert.CrSaldoIniExtME.Value.Value != 0)
                        insert = true;
                    #endregion
                    if (insert)
                    {
                        #region Valores de los parametros
                        mySqlCommand.Parameters["@BalanceTipoID"].Value = saldoInsert.BalanceTipoID.Value;
                        mySqlCommand.Parameters["@CuentaID"].Value = saldoInsert.CuentaID.Value;
                        mySqlCommand.Parameters["@TerceroID"].Value = saldoInsert.TerceroID.Value;
                        mySqlCommand.Parameters["@ProyectoID"].Value = saldoInsert.ProyectoID.Value;
                        mySqlCommand.Parameters["@CentroCostoID"].Value = saldoInsert.CentroCostoID.Value;
                        mySqlCommand.Parameters["@LineaPresupuestoID"].Value = saldoInsert.LineaPresupuestoID.Value;
                        mySqlCommand.Parameters["@ConceptoSaldoID"].Value = saldoInsert.ConceptoSaldoID.Value;
                        mySqlCommand.Parameters["@ConceptoCargoID"].Value = saldoInsert.ConceptoCargoID.Value;
                        mySqlCommand.Parameters["@IdentificadorTR"].Value = saldoInsert.IdentificadorTR.Value.Value;
                        mySqlCommand.Parameters["@vlBaseML"].Value = 0;
                        mySqlCommand.Parameters["@vlBaseME"].Value = 0;
                        mySqlCommand.Parameters["@DbOrigenLocML"].Value = 0;
                        mySqlCommand.Parameters["@DbOrigenExtML"].Value = 0;
                        mySqlCommand.Parameters["@DbOrigenLocME"].Value = 0;
                        mySqlCommand.Parameters["@DbOrigenExtME"].Value = 0;
                        mySqlCommand.Parameters["@CrOrigenLocML"].Value = 0;
                        mySqlCommand.Parameters["@CrOrigenExtML"].Value = 0;
                        mySqlCommand.Parameters["@CrOrigenLocME"].Value = 0;
                        mySqlCommand.Parameters["@CrOrigenExtME"].Value = 0;
                        mySqlCommand.Parameters["@DbSaldoIniLocML"].Value = saldoInsert.DbOrigenLocML.Value.Value + saldoInsert.DbSaldoIniLocML.Value.Value;
                        mySqlCommand.Parameters["@DbSaldoIniExtML"].Value = saldoInsert.DbOrigenExtML.Value.Value + saldoInsert.DbSaldoIniExtML.Value.Value;
                        mySqlCommand.Parameters["@DbSaldoIniLocME"].Value = saldoInsert.DbOrigenLocME.Value.Value + saldoInsert.DbSaldoIniLocME.Value.Value;
                        mySqlCommand.Parameters["@DbSaldoIniExtME"].Value = saldoInsert.DbOrigenExtME.Value.Value + saldoInsert.DbSaldoIniExtME.Value.Value;
                        mySqlCommand.Parameters["@CrSaldoIniLocML"].Value = saldoInsert.CrOrigenLocML.Value.Value + saldoInsert.CrSaldoIniLocML.Value.Value;
                        mySqlCommand.Parameters["@CrSaldoIniExtML"].Value = saldoInsert.CrOrigenExtML.Value.Value + saldoInsert.CrSaldoIniExtML.Value.Value;
                        mySqlCommand.Parameters["@CrSaldoIniLocME"].Value = saldoInsert.CrOrigenLocME.Value.Value + saldoInsert.CrSaldoIniLocME.Value.Value;
                        mySqlCommand.Parameters["@CrSaldoIniExtME"].Value = saldoInsert.CrOrigenExtME.Value.Value + saldoInsert.CrSaldoIniExtME.Value.Value;
                        #endregion
                        #region Query de insertar
                        string sqlInsert =
                            "INSERT INTO coCuentaSaldo (" +
                            "   EmpresaID,PeriodoID,BalanceTipoID,CuentaID,TerceroID,ProyectoID,CentroCostoID," +
                            "   LineaPresupuestoID,ConceptoSaldoID,IdentificadorTR,ConceptoCargoID,vlBaseML,vlBaseME," +
                            "   DbOrigenLocML,DbOrigenExtML,DbOrigenLocME,DbOrigenExtME,CrOrigenLocML,CrOrigenExtML,CrOrigenLocME,CrOrigenExtME," +
                            "   DbSaldoIniLocML,DbSaldoIniExtML,DbSaldoIniLocME,DbSaldoIniExtME,CrSaldoIniLocML,CrSaldoIniExtML,CrSaldoIniLocME,CrSaldoIniExtME," +
                            "   eg_coBalanceTipo,eg_coPlanCuenta,eg_coTercero,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,eg_glConceptoSaldo,eg_coConceptoCargo" +
                            ") VALUES (" +
                            "   @EmpresaID,@PeriodoID,@BalanceTipoID,@CuentaID,@TerceroID,@ProyectoID,@CentroCostoID," +
                            "   @LineaPresupuestoID,@ConceptoSaldoID,@IdentificadorTR,@ConceptoCargoID,@vlBaseML,@vlBaseME," +
                            "   @DbOrigenLocML,@DbOrigenExtML,@DbOrigenLocME,@DbOrigenExtME,@CrOrigenLocML,@CrOrigenExtML,@CrOrigenLocME,@CrOrigenExtME," +
                            "   @DbSaldoIniLocML,@DbSaldoIniExtML,@DbSaldoIniLocME,@DbSaldoIniExtME,@CrSaldoIniLocML,@CrSaldoIniExtML,@CrSaldoIniLocME,@CrSaldoIniExtME," +
                            "   @eg_coBalanceTipo,@eg_coPlanCuenta,@eg_coTercero,@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto,@eg_glConceptoSaldo,@eg_coConceptoCargo" +
                            ")";

                        mySqlCommand.CommandText = sqlInsert;
                        #endregion
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
                #endregion
                #region Actualiza los saldos existentes

                #region Definicion de parametros para actualizar
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@DbSaldoIniLocML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniExtML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniLocME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DbSaldoIniExtME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniLocML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniExtML", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniLocME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CrSaldoIniExtME", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glConceptoSaldo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Valores de parametros generales para actualizar
                mySqlCommand.Parameters["@EmpresaID"].Value = Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = siguientePeriodo;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glConceptoSaldo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glConceptoSaldo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                #endregion
                foreach (DTO_coCuentaSaldo saldoUpdate in saldosUpdate)
                {
                    #region Parametros del query para actualizar
                    mySqlCommand.Parameters["@CuentaID"].Value = saldoUpdate.CuentaID.Value;
                    mySqlCommand.Parameters["@BalanceTipoID"].Value = saldoUpdate.BalanceTipoID.Value;
                    mySqlCommand.Parameters["@TerceroID"].Value = saldoUpdate.TerceroID.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = saldoUpdate.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = saldoUpdate.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = saldoUpdate.LineaPresupuestoID.Value;
                    mySqlCommand.Parameters["@ConceptoSaldoID"].Value = saldoUpdate.ConceptoSaldoID.Value;
                    mySqlCommand.Parameters["@ConceptoCargoID"].Value = saldoUpdate.ConceptoCargoID.Value;
                    mySqlCommand.Parameters["@IdentificadorTR"].Value = saldoUpdate.IdentificadorTR.Value.Value;

                    mySqlCommand.Parameters["@DbSaldoIniLocML"].Value = saldoUpdate.DbOrigenLocML.Value.Value + saldoUpdate.DbSaldoIniLocML.Value.Value;
                    mySqlCommand.Parameters["@DbSaldoIniExtML"].Value = saldoUpdate.DbOrigenExtML.Value.Value + saldoUpdate.DbSaldoIniExtML.Value.Value;
                    mySqlCommand.Parameters["@DbSaldoIniLocME"].Value = saldoUpdate.DbOrigenLocME.Value.Value + saldoUpdate.DbSaldoIniLocME.Value.Value;
                    mySqlCommand.Parameters["@DbSaldoIniExtME"].Value = saldoUpdate.DbOrigenExtME.Value.Value + saldoUpdate.DbSaldoIniExtME.Value.Value;
                    mySqlCommand.Parameters["@CrSaldoIniLocML"].Value = saldoUpdate.CrOrigenLocML.Value.Value + saldoUpdate.CrSaldoIniLocML.Value.Value;
                    mySqlCommand.Parameters["@CrSaldoIniExtML"].Value = saldoUpdate.CrOrigenExtML.Value.Value + saldoUpdate.CrSaldoIniExtML.Value.Value;
                    mySqlCommand.Parameters["@CrSaldoIniLocME"].Value = saldoUpdate.CrOrigenLocME.Value.Value + saldoUpdate.CrSaldoIniLocME.Value.Value;
                    mySqlCommand.Parameters["@CrSaldoIniExtME"].Value = saldoUpdate.CrOrigenExtME.Value.Value + saldoUpdate.CrSaldoIniExtME.Value.Value;
                    #endregion
                    #region Query para actualizar
                    string sqlUpdate =
                        "UPDATE coCuentaSaldo SET " +
                        "   DbSaldoIniLocML = @DbSaldoIniLocML," +
                        "   DbSaldoIniExtML = @DbSaldoIniExtML," +
                        "   DbSaldoIniLocME = @DbSaldoIniLocME," +
                        "   DbSaldoIniExtME = @DbSaldoIniExtME," +
                        "   CrSaldoIniLocML = @CrSaldoIniLocML," +
                        "   CrSaldoIniExtML = @CrSaldoIniExtML," +
                        "   CrSaldoIniLocME = @CrSaldoIniLocME," +
                        "   CrSaldoIniExtME = @CrSaldoIniExtME" +
                        " WHERE " +
                        "   EmpresaID=@EmpresaID AND " +
                        "   PeriodoID=@PeriodoID AND " +
                        "   BalanceTipoID=@BalanceTipoID AND " +
                        "   CuentaID=@CuentaID AND " +
                        "   TerceroID=@TerceroID AND " +
                        "   ProyectoID=@ProyectoID AND " +
                        "   CentroCostoID=@CentroCostoID AND " +
                        "   LineaPresupuestoID=@LineaPresupuestoID AND " +
                        "   ConceptoSaldoID=@ConceptoSaldoID AND " +
                        "   ConceptoCargoID=@ConceptoCargoID AND " +
                        "   IdentificadorTR=@IdentificadorTR AND " +
                        "   eg_coBalanceTipo=@eg_coBalanceTipo AND " +
                        "   eg_coPlanCuenta=@eg_coPlanCuenta AND " +
                        "   eg_coTercero=@eg_coTercero AND " +
                        "   eg_coProyecto=@eg_coProyecto AND " +
                        "   eg_coCentroCosto=@eg_coCentroCosto AND " +
                        "   eg_plLineaPresupuesto=@eg_plLineaPresupuesto AND " +
                        "   eg_glConceptoSaldo=@eg_glConceptoSaldo AND " +
                        "   eg_coConceptoCargo=@eg_coConceptoCargo";

                    mySqlCommand.CommandText = sqlUpdate;
                    #endregion
                    mySqlCommand.ExecuteNonQuery();
                } 

                #endregion
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Comprobante_HacerCierrePeriodo");
                throw ex;
            }
        }

        /// <summary>
        /// Abre un nuevo mes
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="modulo">Modulo de cierre</param>
        public void DAL_Contabilidad_AbrirMes(DateTime periodo, string moduloId)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@AbiertoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = moduloId;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@AbiertoInd"].Value = 1;

                mySqlCommand.CommandText = "Select * from coCierresControl " +
                            "where EmpresaID=@EmpresaID and ModuloID=@ModuloID and Periodo <= @Periodo";

                List<DateTime> periodos = new List<DateTime>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DateTime p = Convert.ToDateTime(dr["Periodo"]);
                    periodos.Add(p);
                }
                dr.Close();

                mySqlCommand.CommandText = "Update coCierresControl set AbiertoInd=@AbiertoInd " +
                            "where EmpresaID=@EmpresaID and ModuloID=@ModuloID and Periodo=@Periodo";

                foreach (DateTime p in periodos)
                {
                    mySqlCommand.Parameters["@Periodo"].Value = p;
                    mySqlCommand.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_AbrirMes");
                throw exception;
            }
        }

        #endregion

        #region Impuestos

        /// <summary>
        /// Trae los registros 
        /// </summary>
        /// <param name="imp">Impuesto sobre el cual se esta haciendo la declaracion</param>
        /// <param name="ctas">Lista de cuentas para hacer la consulta</param>
        /// <param name="renglon">Renglon sobre el cual se esta haciendo la consulta</param>
        /// <param name="fechaIni">Periodo inicial de consulta</param>
        /// <param name="fechaFin">Periodo final de consultya</param>
        /// <returns>Retorna los detalles de la consulta</returns>
        public List<DTO_ComprobanteFooter> DAL_Contabilidad_GetAuxiliaresForImpuesto(DTO_coImpuestoDeclaracion imp, DTO_coImpDeclaracionRenglon renglon, List<string> ctas, 
            DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
              
                #region Parametros generales

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@InfExogenaInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommand.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommand.Parameters["@InfExogenaInd"].Value = true;

                #endregion
                #region Query de las cuentas
                int index = 0;
                string queryCtas = string.Empty;
                string queryCostoInd = renglon.CostosInd.Value.Value ? string.Empty : " and aux.DatoAdd1 = " + AuxiliarDatoAdd1.IVA.ToString() + " ";
                if (ctas.Count > 0)
                {
                    queryCtas = " AND (";
                    foreach (string cta in ctas)
                    {
                        if (index > 0)
                            queryCtas += " OR ";

                        queryCtas += "(aux.CuentaID = '" + cta.Trim() + "'" + " and aux.DatoAdd2 = '" + renglon.Tarifa.Value.Value.ToString() + "'" + queryCostoInd + ")";
                        index++;
                    }
                    queryCtas += ")";
                }

                #endregion
                #region Carga la informacion del lugar geografico
                string queryLG = string.Empty;
                if (imp.MunicipalInd.Value.Value)
                    queryLG = " AND LugarGeograficoID='" + imp.LugarGeograficoID.Value + "'";
                #endregion
                #region Consulta
                string query =
                    "SELECT aux.CuentaID, SUM(vlrBaseML) as vlrBaseML, SUM(vlrMdaLoc) as vlrMdaLoc " + 
                    "FROM coAuxiliar aux with(nolock) " + 
                    "   left join coPlanCuenta cta with(nolock) on aux.CuentaID = cta.CuentaID " +
                    "   left join coComprobante comp with(nolock) on aux.ComprobanteID = comp.ComprobanteID " +
                    "WHERE EmpresaID=@EmpresaID" +
                    "   AND (fecha BETWEEN @FechaIni AND @FechaFin)" + queryCtas + queryLG +
                    "   AND (cta.NITCierreAnual is null or aux.TerceroID <> cta.NITCierreAnual) " +
                    "   AND comp.InfExogenaInd = @InfExogenaInd " + 
                    "GROUP BY aux.CuentaID";
                #endregion
                #region Realiza la consulta

                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                mySqlCommand.CommandText = query;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
               
                try
                {
                    while (dr.Read())
                    {
                        string cta = dr["CuentaID"].ToString();
                        decimal vlrBase = Convert.ToDecimal(dr["vlrBaseML"]);
                        decimal vlrMdaLoc = Convert.ToDecimal(dr["vlrMdaLoc"]);

                        DTO_ComprobanteFooter det = new DTO_ComprobanteFooter(dr);
                        det.CuentaID.Value = cta;
                        det.vlrBaseML.Value = vlrBase;
                        det.vlrMdaLoc.Value = vlrMdaLoc;
                        footer.Add(det);
                    }
                }
                finally
                {
                    dr.Close();
                }
                #endregion

                return footer;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_GetAuxiliaresForImpuesto");
                throw exception;
            }
        }

        #endregion

        #region Cuenta Alterna

        /// <summary>
        /// Actualiza el valor de la cuenta alterna en las tablas de coBalance coCuentaSaldo y coAuxiliar
        /// </summary>
        public void DAL_Proceso_CuentaAlterna()
        {
            try
            {               
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "    update coCuentaSaldo    " + 
                                           "    set coCuentaSaldo.CuentaAlternaID = coCuentaSaldo.CuentaID   " +
                                           "    where (exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coCuentaSaldo.CuentaAlternaID))    " + 
                                           "    update coCuentaSaldo    " +
                                           "    set coCuentaSaldo.CuentaAlternaID = '' where (not exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coCuentaSaldo.CuentaAlternaID)) " + 
                                           "    update coCuentaSaldo    " +
                                           "    set coCuentaSaldo.CuentaAlternaID = coCuentaSaldo.CuentaID where (exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coCuentaSaldo.CuentaAlternaID)) " + 
                                           "    update coCuentaSaldo    " +
                                           "    set coCuentaSaldo.CuentaAlternaID = '' where (not exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coCuentaSaldo.CuentaAlternaID)) " + 
                                           "    update coBalance   " +  
                                           "    set coBalance.CuentaAlternaID = coBalance.CuentaID   " +
                                           "    where (exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coBalance.CuentaAlternaID))      " +  
                                           "    update coBalance      " +
                                           "    set coBalance.CuentaAlternaID = '' where (not exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coBalance.CuentaAlternaID))   " +  
                                           "    update coBalance      " +
                                           "    set coBalance.CuentaAlternaID = coBalance.CuentaID where (exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coBalance.CuentaAlternaID))   " +  
                                           "    update coBalance      " +
                                           "    set coBalance.CuentaAlternaID = '' where (not exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coBalance.CuentaAlternaID))   " +                                             
                                           "    update coAuxiliar  " +  
                                           "    set coAuxiliar.CuentaAlternaID = coAuxiliar.CuentaID  " +
                                           "    where (exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coAuxiliar.CuentaAlternaID))  " +  
                                           "    update coAuxiliar  " +
                                           "    set coAuxiliar.CuentaAlternaID = '' where (not exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAltern with(nolock)a where coPlanCuentaAlterna.CuentaID = coAuxiliar.CuentaAlternaID)) " +  
                                           "    update coAuxiliar  " +
                                           "    set coAuxiliar.CuentaAlternaID = coAuxiliar.CuentaID where (exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coAuxiliar.CuentaAlternaID)) " +  
                                           "    update coAuxiliar  " +
                                           "    set coAuxiliar.CuentaAlternaID = '' where (not exists (select coPlanCuentaAlterna.CuentaID from coPlanCuentaAlterna with(nolock) where coPlanCuentaAlterna.CuentaID = coAuxiliar.CuentaAlternaID)) ";
                               
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Contabilidad_DAL_Proceso_CuentaAlterna");
                throw exception;
            }
        }

        #endregion
    }
}
