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
    public class DAL_ccCreditoPlanPagos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCreditoPlanPagos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summMOary>
        /// Trae todos los registros de DTO_PlanDePagos
        /// </summary>
        /// <returns>retorna una lista de DTO_PlanDePagos</returns>
        public List<DTO_ccCreditoPlanPagos> DAL_ccCreditoPlanPagos_GetByNumDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPlanPagos PA with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc " +
                                           "ORDER BY CuotaID ";

                List<DTO_ccCreditoPlanPagos> result = new List<DTO_ccCreditoPlanPagos>();

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoPlanPagos dto = new DTO_ccCreditoPlanPagos(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPlanpagos_GetByNumDoc");
                throw exception;
            }
        }

        /// <summMOary>
        /// Trae la info de una couta
        /// </summary>
        /// <param name="creditoCuotaNum">Identificador único de la cuota</param>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public DTO_ccCreditoPlanPagos DAL_ccCreditoPlanpagos_GetByID(int creditoCuotaNum)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = creditoCuotaNum;
                mySqlCommand.CommandText = "SELECT * FROM ccCreditoPlanPagos with(nolock) WHERE Consecutivo = @Consecutivo ";

                DTO_ccCreditoPlanPagos result = new DTO_ccCreditoPlanPagos();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoPlanPagos(dr);
                    if (!string.IsNullOrWhiteSpace(dr["VlrSaldoSeguro"].ToString()))
                        result.VlrSaldoSeguro.Value = Convert.ToDecimal(dr["VlrSaldoSeguro"]);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPlanpagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="pagos"></param>
        /// <returns></returns>
        public void DAL_ccCreditoPlanPagos_Add(DTO_PlanDePagos pagos, int numDocNro)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosFijos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagadoCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagadoExtras", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaLiquidaMora", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrMoraLiquida", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMoraPago", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaFijadaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaFlujo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@IndIntCausados", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Variables fijas
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDocNro;
                mySqlCommandSel.Parameters["@CuotaFijadaInd"].Value = 0;
                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrCapitalIFRS"].Value = 0;
                mySqlCommandSel.Parameters["@VlrUtilidadIFRS"].Value = 0;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalIFRS"].Value = 0;
                mySqlCommandSel.Parameters["@IndIntCausados"].Value = 0;

                if (!string.IsNullOrWhiteSpace(pagos.CompradorCarteraID.Value))
                {
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = pagos.CompradorCarteraID.Value;
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                }
                else
                {
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = DBNull.Value;
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = DBNull.Value;
                }

                mySqlCommandSel.Parameters["@DocVenta"].Value = DBNull.Value;
                mySqlCommandSel.Parameters["@FechaFlujo"].Value = DBNull.Value;

                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccCreditoPlanPagos  " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[FechaCuota]   " +
                                               "    ,[CompradorCarteraID]   " +
                                               "    ,[VlrCuota]   " +
                                               "    ,[VlrCapital]   " +
                                               "    ,[VlrInteres]   " +
                                               "    ,[VlrSeguro]   " +
                                               "    ,[VlrOtro1]   " +
                                               "    ,[VlrOtro2]   " +
                                               "    ,[VlrOtro3]   " +
                                               "    ,[VlrOtrosFijos]   " +
                                               "    ,[VlrSaldoCapital]   " +
                                               "    ,[VlrSaldoSeguro]   " +
                                               "    ,[VlrCapitalCesion ]   " +
                                               "    ,[VlrUtilidadCesion ]   " +
                                               "    ,[VlrDerechosCesion ]   " +
                                               "    ,[VlrSaldoCapitalCesion]   " +
                                               "    ,[VlrCapitalIFRS] " +
                                               "    ,[VlrUtilidadIFRS]  " +
                                               "    ,[VlrSaldoCapitalIFRS]  " +
                                               "    ,[TipoPago]   " +
                                               "    ,[VlrPagadoCuota]   " +
                                               "    ,[VlrPagadoExtras]   " +
                                               "    ,[FechaLiquidaMora]   " +
                                               "    ,[VlrMoraLiquida]   " +
                                               "    ,[VlrMoraPago]   " +
                                               "    ,[CuotaFijadaInd] " +
                                               "    ,[DocVenta] " +
                                               "    ,[FechaFlujo] " +
                                               "    ,[IndIntCausados] " +
                                               "    ,[eg_ccCompradorCartera]   )" +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@CuotaID    " +
                                               "  ,@FechaCuota    " +
                                               "  ,@CompradorCarteraID    " +
                                               "  ,@VlrCuota    " +
                                               "  ,@VlrCapital    " +
                                               "  ,@VlrInteres    " +
                                               "  ,@VlrSeguro    " +
                                               "  ,@VlrOtro1    " +
                                               "  ,@VlrOtro2    " +
                                               "  ,@VlrOtro3    " +
                                               "  ,@VlrOtrosFijos    " +
                                               "  ,@VlrSaldoCapital    " +
                                               "  ,@VlrSaldoSeguro  " +
                                               "  ,@VlrCapitalCesion    " +
                                               "  ,@VlrUtilidadCesion    " +
                                               "  ,@VlrDerechosCesion    " +
                                               "  ,@VlrSaldoCapitalCesion    " +
                                               "  ,@VlrCapitalIFRS " +
                                               "  ,@VlrUtilidadIFRS " +
                                               "  ,@VlrSaldoCapitalIFRS " +
                                               "  ,@TipoPago    " +
                                               "  ,@VlrPagadoCuota " +
                                               "  ,@VlrPagadoExtras    " +
                                               "  ,@FechaLiquidaMora    " +
                                               "  ,@VlrMoraLiquida    " +
                                               "  ,@VlrMoraPago    " +
                                               "  ,@CuotaFijadaInd " +
                                               "  ,@DocVenta " +
                                               "  ,@FechaFlujo " +
                                               "  ,@IndIntCausados " +
                                               "  ,@eg_ccCompradorCartera)   ";

                #endregion
                #region Asigna Valores
                mySqlCommandSel.Parameters["@VlrCuota"].Value = pagos.VlrCuota;
                Dictionary<string, bool> dic = pagos.ComponentesFijos;
                decimal vlrSaldoCapital = pagos.VlrPrestamo;
                decimal vlrSaldoCapitalPoliza = pagos.VlrPoliza;
                for (int i = 0; i < pagos.Cuotas.Count; i++)
                {
                    mySqlCommandSel.Parameters["@CuotaID"].Value = pagos.Cuotas[i].NumCuota;
                    mySqlCommandSel.Parameters["@FechaCuota"].Value = pagos.Cuotas[i].Fecha.Date;
                    mySqlCommandSel.Parameters["@VlrCapital"].Value = pagos.Cuotas[i].Capital;
                    mySqlCommandSel.Parameters["@VlrInteres"].Value = pagos.Cuotas[i].Intereses;
                    mySqlCommandSel.Parameters["@VlrSeguro"].Value = pagos.Cuotas[i].Seguro;
                    mySqlCommandSel.Parameters["@VlrOtro1"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrOtro2"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrOtro3"].Value = 0;

                    List<string> extraComp = pagos.Cuotas[i].Componentes;
                    List<int> valores = pagos.Cuotas[i].ValoresComponentes;
                    int totalFijos = 0;
                    for (int j = 0; j < extraComp.Count; j++)
                    {
                        string nombre = extraComp[j];
                        int valor = valores[j];
                        DTO_ccSolicitudComponentes comp = pagos.ComponentesAll.Where(x => x.Descripcion.Value == nombre).First();
                        if (dic[comp.Descripcion.Value])
                        {
                            totalFijos += valores[j];
                        }
                        else
                        {
                            if (j == 0)
                                mySqlCommandSel.Parameters["@VlrOtro1"].Value = valores[j];
                            if (j == 1)
                                mySqlCommandSel.Parameters["@VlrOtro2"].Value = valores[j];
                            if (j == 2)
                                mySqlCommandSel.Parameters["@VlrOtro3"].Value = valores[j];
                        }
                    }

                    mySqlCommandSel.Parameters["@VlrOtrosfijos"].Value = totalFijos;
                    mySqlCommandSel.Parameters["@VlrSaldoCapital"].Value = vlrSaldoCapital;
                    mySqlCommandSel.Parameters["@VlrSaldoSeguro"].Value = vlrSaldoCapitalPoliza;
                    mySqlCommandSel.Parameters["@TipoPago"].Value = 1;
                    mySqlCommandSel.Parameters["@VlrPagadoCuota"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrPagadoExtras"].Value = 0;
                    mySqlCommandSel.Parameters["@FechaLiquidaMora"].Value = pagos.Cuotas[i].Fecha;
                    mySqlCommandSel.Parameters["@VlrMoraLiquida"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrMoraPago"].Value = 0;

                    mySqlCommandSel.ExecuteNonQuery();
                    if (vlrSaldoCapitalPoliza != 0)
                        vlrSaldoCapitalPoliza = vlrSaldoCapital - pagos.Cuotas[i].Seguro;
                    vlrSaldoCapital = vlrSaldoCapital - pagos.Cuotas[i].Capital;
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPlanPagos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccCreditoDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccCreditoPlanPagos_Update(DTO_ccCreditoPlanPagos pago)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrOtrosFijos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrPagadoCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagadoExtras", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaLiquidaMora", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrMoraLiquida", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMoraPago", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaFijadaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaFlujo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@IndIntCausados", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = pago.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = pago.CuotaID.Value;
                mySqlCommandSel.Parameters["@FechaCuota"].Value = pago.FechaCuota.Value.Value.Date;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = pago.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = pago.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCapital"].Value = pago.VlrCapital.Value;
                mySqlCommandSel.Parameters["@VlrInteres"].Value = pago.VlrInteres.Value;
                mySqlCommandSel.Parameters["@VlrSeguro"].Value = pago.VlrSeguro.Value;
                mySqlCommandSel.Parameters["@VlrOtro1"].Value = pago.VlrOtro1.Value;
                mySqlCommandSel.Parameters["@VlrOtro2"].Value = pago.VlrOtro2.Value;
                mySqlCommandSel.Parameters["@VlrOtro3"].Value = pago.VlrOtro3.Value;
                mySqlCommandSel.Parameters["@VlrOtrosFijos"].Value = pago.VlrOtrosFijos.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapital"].Value = pago.VlrSaldoCapital.Value;
                mySqlCommandSel.Parameters["@VlrSaldoSeguro"].Value = pago.VlrSaldoSeguro.Value;

                mySqlCommandSel.Parameters["@VlrCapitalIFRS"].Value = pago.VlrCapitalIFRS.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadIFRS"].Value = pago.VlrUtilidadIFRS.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalIFRS"].Value = pago.VlrSaldoCapitalIFRS.Value;

                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = pago.VlrCapitalCesion.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = pago.VlrUtilidadCesion.Value;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = pago.VlrDerechosCesion.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalCesion"].Value = pago.VlrSaldoCapitalCesion.Value;
                mySqlCommandSel.Parameters["@TipoPago"].Value = pago.TipoPago.Value;
                mySqlCommandSel.Parameters["@VlrPagadoCuota"].Value = pago.VlrPagadoCuota.Value;
                mySqlCommandSel.Parameters["@VlrPagadoExtras"].Value = pago.VlrPagadoExtras.Value;
                mySqlCommandSel.Parameters["@FechaLiquidaMora"].Value = pago.FechaLiquidaMora.Value;
                mySqlCommandSel.Parameters["@VlrMoraLiquida"].Value = pago.VlrMoraLiquida.Value;
                mySqlCommandSel.Parameters["@VlrMoraPago"].Value = pago.VlrMoraPago.Value;
                mySqlCommandSel.Parameters["@CuotaFijadaInd"].Value = pago.CuotaFijadaInd.Value;
                mySqlCommandSel.Parameters["@DocVenta"].Value = pago.DocVenta.Value;
                mySqlCommandSel.Parameters["@FechaFlujo"].Value = pago.FechaFlujo.Value;
                mySqlCommandSel.Parameters["@IndIntCausados"].Value = pago.IndIntCausados.Value;

                if (!string.IsNullOrWhiteSpace(pago.CompradorCarteraID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = DBNull.Value;

                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "UPDATE ccCreditoPlanPagos SET " +
                    "   FechaCuota = @FechaCuota, " +
                    "   CompradorCarteraID = @CompradorCarteraID," +
                    "   VlrCuota = @VlrCuota, " +
                    "   VlrCapital = @VlrCapital, " +
                    "   VlrInteres = @VlrInteres, " +
                    "   VlrSeguro = @VlrSeguro, " +
                    "   VlrOtro1 = @VlrOtro1, " +
                    "   VlrOtro2 = @VlrOtro2, " +
                    "   VlrOtro3 = @VlrOtro3, " +
                    "   VlrOtrosFijos = @VlrOtrosFijos, " +
                    "   VlrSaldoCapital = @VlrSaldoCapital, " +
                    "   VlrSaldoSeguro = @VlrSaldoSeguro, " +
                    "   VlrCapitalCesion = @VlrCapitalCesion, " +
                    "   VlrUtilidadCesion = @VlrUtilidadCesion, " +
                    "   VlrDerechosCesion = @VlrDerechosCesion," +
                    "   VlrSaldoCapitalCesion = @VlrSaldoCapitalCesion," +
                    "   VlrCapitalIFRS = @VlrCapitalIFRS," +
                    "   VlrUtilidadIFRS = @VlrUtilidadIFRS," +
                    "   VlrSaldoCapitalIFRS = @VlrSaldoCapitalIFRS," +
                    "   TipoPago = @TipoPago, " +
                    "   VlrPagadoCuota = VlrPagadoCuota + @VlrPagadoCuota, " +
                    "   VlrPagadoExtras = VlrPagadoExtras + @VlrPagadoExtras, " +
                    "   FechaLiquidaMora = @FechaLiquidaMora, " +
                    "   VlrMoraLiquida = @VlrMoraLiquida, " +
                    "   VlrMoraPago = @VlrMoraPago, " +
                    "   CuotaFijadaInd = @CuotaFijadaInd, " +
                    "   DocVenta = @DocVenta, " +
                    "   FechaFlujo = @FechaFlujo, " +
                    "   IndIntCausados = @IndIntCausados, " +
                    "   eg_ccCompradorCartera = @eg_ccCompradorCartera " +
                    "WHERE NumeroDoc = @numeroDoc AND CuotaID = @CuotaID";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPlanPagos_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina una cuota de una plan de pagos por su identificador único
        /// </summary>
        /// <returns></returns>
        public void DAL_ccCreditoPlanPagos_Delete(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " Delete from ccSolicitudPlanPagos WHERE Consecutivo =  @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPlanPagos_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Agrega un registro unicamente a ccCreditoPlanPago
        /// </summary>
        /// <param name="pagos"></param>
        /// <returns></returns>
        public void DAL_ccCreditoPlanPagos_AddCuota(DTO_ccCreditoPlanPagos cuota)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtrosFijos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagadoCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrPagadoExtras", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaLiquidaMora", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrMoraLiquida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaFijadaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaFlujo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna las variables
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = cuota.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = cuota.CuotaID.Value;
                mySqlCommandSel.Parameters["@FechaCuota"].Value = cuota.FechaCuota.Value.Value.Date;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = cuota.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = cuota.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCapital"].Value = cuota.VlrCapital.Value;
                mySqlCommandSel.Parameters["@VlrInteres"].Value = cuota.VlrInteres.Value;
                mySqlCommandSel.Parameters["@VlrSeguro"].Value = cuota.VlrSeguro.Value;
                mySqlCommandSel.Parameters["@VlrOtro1"].Value = cuota.VlrOtro1.Value;
                mySqlCommandSel.Parameters["@VlrOtro2"].Value = cuota.VlrOtro2.Value;
                mySqlCommandSel.Parameters["@VlrOtro3"].Value = cuota.VlrOtro3.Value;
                mySqlCommandSel.Parameters["@VlrOtrosFijos"].Value = cuota.VlrOtrosFijos.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapital"].Value = cuota.VlrSaldoCapital.Value;
                mySqlCommandSel.Parameters["@VlrSaldoSeguro"].Value = cuota.VlrSaldoSeguro.Value;
                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = cuota.VlrCapitalCesion.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = cuota.VlrUtilidadCesion.Value;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = cuota.VlrDerechosCesion.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalCesion"].Value = cuota.VlrSaldoCapitalCesion.Value;
                mySqlCommandSel.Parameters["@TipoPago"].Value = cuota.TipoPago.Value;
                mySqlCommandSel.Parameters["@VlrCapitalIFRS"].Value = cuota.VlrCapitalIFRS.Value;
                mySqlCommandSel.Parameters["@VlrUtilidadIFRS"].Value = cuota.VlrUtilidadIFRS.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalIFRS"].Value = cuota.VlrSaldoCapitalIFRS.Value;
                mySqlCommandSel.Parameters["@VlrPagadoCuota"].Value = cuota.VlrPagadoCuota.Value;
                mySqlCommandSel.Parameters["@VlrPagadoExtras"].Value = cuota.VlrPagadoExtras.Value;
                mySqlCommandSel.Parameters["@FechaLiquidaMora"].Value = cuota.FechaLiquidaMora.Value;
                mySqlCommandSel.Parameters["@VlrMoraLiquida"].Value = cuota.VlrMoraLiquida.Value;
                mySqlCommandSel.Parameters["@VlrMoraPago"].Value = cuota.VlrMoraPago.Value;
                mySqlCommandSel.Parameters["@CuotaFijadaInd"].Value = cuota.CuotaFijadaInd.Value;
                mySqlCommandSel.Parameters["@DocVenta"].Value = cuota.DocVenta.Value;
                mySqlCommandSel.Parameters["@FechaFlujo"].Value = cuota.FechaFlujo.Value;
                if (!string.IsNullOrWhiteSpace(cuota.CompradorCarteraID.Value))
                {
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                }
                else
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = DBNull.Value;
                #endregion
                #region Query
                mySqlCommandSel.CommandText = "    INSERT INTO ccCreditoPlanPagos  " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[FechaCuota]   " +
                                               "    ,[CompradorCarteraID]   " +
                                               "    ,[VlrCuota]   " +
                                               "    ,[VlrCapital]   " +
                                               "    ,[VlrInteres]   " +
                                               "    ,[VlrSeguro]   " +
                                               "    ,[VlrOtro1]   " +
                                               "    ,[VlrOtro2]   " +
                                               "    ,[VlrOtro3]   " +
                                               "    ,[VlrOtrosFijos]   " +
                                               "    ,[VlrSaldoCapital]   " +
                                               "    ,[VlrSaldoSeguro]   " +
                                               "    ,[VlrCapitalCesion ]   " +
                                               "    ,[VlrUtilidadCesion ]   " +
                                               "    ,[VlrDerechosCesion ]   " +
                                               "    ,[VlrSaldoCapitalCesion]   " +
                                               "    ,[VlrCapitalIFRS] " +
                                               "    ,[VlrUtilidadIFRS]  " +
                                               "    ,[VlrSaldoCapitalIFRS]  " +
                                               "    ,[TipoPago]   " +
                                               "    ,[VlrPagadoCuota]   " +
                                               "    ,[VlrPagadoExtras]   " +
                                               "    ,[FechaLiquidaMora]   " +
                                               "    ,[VlrMoraLiquida]   " +
                                               "    ,[VlrMoraPago]   " +
                                               "    ,[CuotaFijadaInd] " +
                                               "    ,[DocVenta] " +
                                               "    ,[FechaFlujo] " +
                                               "    ,[eg_ccCompradorCartera]   )" +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@CuotaID    " +
                                               "  ,@FechaCuota    " +
                                               "  ,@CompradorCarteraID    " +
                                               "  ,@VlrCuota    " +
                                               "  ,@VlrCapital    " +
                                               "  ,@VlrInteres    " +
                                               "  ,@VlrSeguro    " +
                                               "  ,@VlrOtro1    " +
                                               "  ,@VlrOtro2    " +
                                               "  ,@VlrOtro3    " +
                                               "  ,@VlrOtrosFijos    " +
                                               "  ,@VlrSaldoCapital    " +
                                               "  ,@VlrSaldoSeguro    " +
                                               "  ,@VlrCapitalCesion    " +
                                               "  ,@VlrUtilidadCesion    " +
                                               "  ,@VlrDerechosCesion    " +
                                               "  ,@VlrSaldoCapitalCesion    " +
                                               "  ,@VlrCapitalIFRS " +
                                               "  ,@VlrUtilidadIFRS " +
                                               "  ,@VlrSaldoCapitalIFRS " +
                                               "  ,@TipoPago    " +
                                               "  ,@VlrPagadoCuota " +
                                               "  ,@VlrPagadoExtras    " +
                                               "  ,@FechaLiquidaMora    " +
                                               "  ,@VlrMoraLiquida    " +
                                               "  ,@VlrMoraPago    " +
                                               "  ,@CuotaFijadaInd " +
                                               "  ,@DocVenta " +
                                               "  ,@FechaFlujo " +
                                               "  ,@eg_ccCompradorCartera)   ";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPlanPagos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega la informacion de la solicitu al credito
        /// </summary>
        /// <param name="numDocNew">Numero Doc del credito</param>
        /// <param name="planPago">DTO con la informacion de la solicitud</param>
        /// <param name="isCooperativa">Indicador para establecer si se esta trabajo en Cartera Cooperativa o Cartera Financiera</param>
        /// <returns></returns>
        public void DAL_ccCreditoPlanPagos_AddFromSolicitudPlanPagos(int numDocOld, int numDocNew)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccCreditoPlanPagos  " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[FechaCuota]   " +
                                               "    ,[CompradorCarteraID]   " +
                                               "    ,[VlrCuota]   " +
                                               "    ,[VlrCapital]   " +
                                               "    ,[VlrInteres]   " +
                                               "    ,[VlrSeguro]   " +
                                               "    ,[VlrOtro1]   " +
                                               "    ,[VlrOtro2]   " +
                                               "    ,[VlrOtro3]   " +
                                               "    ,[VlrOtrosFijos]   " +
                                               "    ,[VlrSaldoCapital]   " +
                                               "    ,[VlrSaldoSeguro]   " +
                                               "    ,[VlrCapitalCesion ]   " +
                                               "    ,[VlrUtilidadCesion ]   " +
                                               "    ,[VlrDerechosCesion ]   " +
                                               "    ,[VlrSaldoCapitalCesion ]   " +
                                               "    ,[VlrCapitalIFRS] " +
                                               "    ,[VlrUtilidadIFRS]  " +
                                               "    ,[VlrSaldoCapitalIFRS]  " +
                                               "    ,[TipoPago]   " +
                                               "    ,[VlrPagadoCuota]   " +
                                               "    ,[VlrPagadoExtras]   " +
                                               "    ,[FechaLiquidaMora]   " +
                                               "    ,[VlrMoraLiquida]   " +
                                               "    ,[VlrMoraPago]   " +
                                               "    ,[CuotaFijadaInd] " +
                                               "    ,[DocVenta] " +
                                               "    ,[FechaFlujo] " +
                                               "    ,[eg_ccCompradorCartera]   )" +
                                               " SELECT        " +
                                               "  @NumeroDoc    " +
                                               "  ,CuotaID    " +
                                               "  ,FechaCuota    " +
                                               "  ,@CompradorCarteraID    " +
                                               "  ,VlrCuota    " +
                                               "  ,VlrCapital    " +
                                               "  ,VlrInteres    " +
                                               "  ,VlrSeguro    " +
                                               "  ,VlrOtro1    " +
                                               "  ,VlrOtro2    " +
                                               "  ,VlrOtro3    " +
                                               "  ,VlrOtrosFijos    " +
                                               "  ,VlrSaldoCapital    " +
                                               "  ,VlrSaldoSeguro    " +
                                               "  ,@VlrCapitalCesion    " +
                                               "  ,@VlrUtilidadCesion    " +
                                               "  ,@VlrDerechosCesion    " +
                                               "  ,@VlrSaldoCapitalCesion    " +
                                               "  ,@VlrCapitalIFRS " +
                                               "  ,@VlrUtilidadIFRS " +
                                               "  ,@VlrSaldoCapitalIFRS " +
                                               "  ,@TipoPago    " +
                                               "  ,@VlrPagadoCuota " +
                                               "  ,@VlrPagadoExtras    " +
                                               "  ,FechaCuota    " +
                                               "  ,@VlrMoraLiquida    " +
                                               "  ,@VlrMoraPago    " +
                                               "  ,@CuotaFijadaInd " +
                                               "  ,@DocVenta " +
                                               "  ,@FechaFlujo " +
                                               "  ,@eg_ccCompradorCartera " +
                                               " FROM ccSolicitudPlanPagos " +
                                               " WHERE NumeroDoc =  @numDocOld";

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@numDocOld", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@VlrCapitalCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrDerechosCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUtilidadIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapitalIFRS", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagadoCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrPagadoExtras", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraLiquida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMoraPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaFijadaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaFlujo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@numDocOld"].Value = numDocOld;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDocNew;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = DBNull.Value;
                mySqlCommandSel.Parameters["@VlrCapitalCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrUtilidadCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrDerechosCesion"].Value = 0;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalCesion"].Value = 0;
                mySqlCommandSel.Parameters["@TipoPago"].Value = 1;
                mySqlCommandSel.Parameters["@VlrCapitalIFRS"].Value = 0;
                mySqlCommandSel.Parameters["@VlrUtilidadIFRS"].Value = 0;
                mySqlCommandSel.Parameters["@VlrSaldoCapitalIFRS"].Value = 0;
                mySqlCommandSel.Parameters["@VlrPagadoCuota"].Value = 0;
                mySqlCommandSel.Parameters["@VlrPagadoExtras"].Value = 0;
                mySqlCommandSel.Parameters["@VlrMoraLiquida"].Value = 0;
                mySqlCommandSel.Parameters["@VlrMoraPago"].Value = 0;
                mySqlCommandSel.Parameters["@CuotaFijadaInd"].Value = 0;
                mySqlCommandSel.Parameters["@DocVenta"].Value = DBNull.Value;
                mySqlCommandSel.Parameters["@FechaFlujo"].Value = DBNull.Value;
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = DBNull.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPlanPagos_AddFromSolicitudPlanPagos");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccCreditoDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccCreditoPlanPagos_UpdateIncorporacionPrevia(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@TipoPago", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                //mySqlCommandSel.Parameters["@TipoPago"].Value = 3;

                mySqlCommandSel.CommandText =
                    "update ccCreditoPlanPagos set FechaCuota = DATEADD(month, -1, FechaCuota), FechaLiquidaMora = DATEADD(month, -1, FechaLiquidaMora) where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPlanPagos_UpdateIncorporacionPrevia");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccCreditoDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public DateTime DAL_ccCreditoPlanPagos_GetFechaCuota1ForCierre(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@CuotaID"].Value = 1;

                mySqlCommandSel.CommandText =
                    "SELECT TipoPago, FechaCuota from ccCreditoPlanPagos with(nolock) where NumeroDoc = @NumeroDoc AND CuotaID = @CuotaID";

                string tipo = string.Empty;
                DateTime fecha = DateTime.Now;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    tipo = dr["TipoPago"].ToString();
                    fecha = Convert.ToDateTime(dr["FechaCuota"]);
                }
                dr.Close();

                return tipo == "3" ? fecha : fecha.AddMonths(-1);
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoPlanPagos_GetFechaCuota1ForCierre");
                throw exception;
            }
        }

        #endregion
    }

}
