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
    public class DAL_ccCreditoDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCreditoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccCreditoDocu DAL_ccCreditoDocu_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccCreditoDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;


                mySqlCommand.CommandText = "SELECT * FROM ccCreditoDocu SA with(nolock)  " +
                                       "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoDocu(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetAll()
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT credito.* , cliente.Descriptivo AS Nombre " +
                                       "FROM ccCreditoDocu credito with(nolock) " +
                                       "    INNER JOIN ccCliente cliente with(nolock) ON cliente.ClienteID = credito.ClienteID and cliente.EmpresaGrupoID = credito.eg_ccCliente " +
                                       "WHERE credito.CanceladoInd = 0 ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu credito;
                    credito = new DTO_ccCreditoDocu(dr);
                    credito.Nombre.Value = dr["Nombre"].ToString();
                    result.Add(credito);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCarteraDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCreditoDocu_Add(DTO_ccCreditoDocu credito)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccCreditoDocu " +
                    "( " +
                    "	NumeroDoc, NumSolicitud, EmpresaID, ClienteID, Libranza, DigitoVer1, DigitoVer2, Solicitud, Pagare,PagarePOL,ComponenteExtraInd, Poliza, LineaCreditoID, AsesorID, PagaduriaID, " +
                    "   CentroPagoID, ZonaID, IncorporaMesInd, IncorporacionTipo, NumDocVerificado, NumDocOpera, PeriodoPago, FechaLiquida, FechaCuota1, " +
                    "   FechaVto, TipoEstado, EstadoDeuda, IndRestructurado, Ciudad, NumDocCompra, VendedorID, CompradorCarteraID, CompradorFinalID, TipoCredito, " +
                    "   TasaEfectivaCredito, TasaEfectivaVenta, TasaEfectivaReCompra, PorInteres, PorSeguro, PorComponente1, PorComponente2, PorComponente3, CompraCarteraInd, " +
                    "   VlrSolicitado, VlrAdicional, VlrPrestamo, VlrLibranza, VlrCompra, VlrDescuento, VlrGiro, VlrMaximoNivelRiesgo, VlrPoliza, Plazo, VlrCuota, PlazoSeguro, " +
                    "	Cuota1Seguro, VlrCuotaSeguro, VlrFinanciaSeguro, FechaRestructura, FechaLiqSeguro, FechaVigenciaINI, FechaVigenciaFIN, DocSeguro, VlrCupoDisponible, PagoVentanillaInd, " +
                    "   BloqueaVentaInd, VendidaInd, NumeroDocCXP, DocSustituye, DocVenta, DocFactura, NumeroCesion, TipoVenta, Observacion, NumIncorporaDoc, " +
                    "   NumDesIncorporaDoc, CanceladoInd, DocAcuerdoPago, DocEstadoCuenta, Abogado, EtapaIncumplimiento,CobranzaEstadoID,CobranzaGestionID,ObsCobranza, FechaReliquidaCJ, " +
                    "   TipoCreditoID, Codeudor1, Codeudor2, Codeudor3, Codeudor4, Codeudor5, ConcesionarioID, AseguradoraID, CooperativaID, Sentencia,FechaSentencia,Juzgado," +
                    "   eg_ccCliente, eg_ccLineaCredito, eg_ccAsesor, eg_ccPagaduria, eg_glZona, eg_glLugarGeografico, eg_ccCentroPagoPAG, " +
                    "   eg_ccCompradorCartera, eg_ccVendedorCartera, eg_ccConcesionario, eg_ccAseguradora, eg_ccCooperativa ,eg_ccCobranzaEstado ,eg_ccCobranzaGestion" +
                    ") " +
                    "VALUES " +
                    "( " +
                    "	@NumeroDoc, @NumSolicitud, @EmpresaID, @ClienteID, @Libranza, @DigitoVer1, @DigitoVer2, @Solicitud, @Pagare,@PagarePOL,@ComponenteExtraInd, @Poliza, @LineaCreditoID, @AsesorID, @PagaduriaID, " +
                    "   @CentroPagoID, @ZonaID, @IncorporaMesInd, @IncorporacionTipo, @NumDocVerificado, @NumDocOpera, @PeriodoPago, @FechaLiquida, @FechaCuota1, " +
                    "   @FechaVto, @TipoEstado, @EstadoDeuda, @IndRestructurado, @Ciudad, @NumDocCompra, @VendedorID, @CompradorCarteraID, @CompradorFinalID, @TipoCredito, " +
                    "   @TasaEfectivaCredito, @TasaEfectivaVenta, @TasaEfectivaReCompra, @PorInteres, @PorSeguro, @PorComponente1, @PorComponente2, @PorComponente3, @CompraCarteraInd, " +
                    "   @VlrSolicitado, @VlrAdicional, @VlrPrestamo, @VlrLibranza, @VlrCompra, @VlrDescuento, @VlrGiro, @VlrMaximoNivelRiesgo, @VlrPoliza, @Plazo, @VlrCuota, @PlazoSeguro, " +
                    "	@Cuota1Seguro, @VlrCuotaSeguro, @VlrFinanciaSeguro, @FechaRestructura, @FechaLiqSeguro, @FechaVigenciaINI, @FechaVigenciaFIN, @DocSeguro, @VlrCupoDisponible, @PagoVentanillaInd, " +
                    "   @BloqueaVentaInd, @VendidaInd, @NumeroDocCXP, @DocSustituye, @DocVenta, @DocFactura, @NumeroCesion, @TipoVenta, @Observacion, @NumIncorporaDoc, " +
                    "   @NumDesIncorporaDoc, @CanceladoInd, @DocAcuerdoPago, @DocEstadoCuenta,, @Abogado, @EtapaIncumplimiento,@CobranzaEstadoID,@CobranzaGestionID,@ObsCobranza, @FechaReliquidaCJ, " +
                    "   @TipoCreditoID, @Codeudor1, @Codeudor2, @Codeudor3, @Codeudor4, @Codeudor5, @ConcesionarioID, @AseguradoraID, @CooperativaID, @Sentencia,@FechaSentencia,@Juzgado" +
                    "   @eg_ccCliente, @eg_ccLineaCredito, @eg_ccAsesor, @eg_ccPagaduria, @eg_glZona, @eg_glLugarGeografico, @eg_ccCentroPagoPAG, " +
                    "   @eg_ccCompradorCartera, @eg_ccVendedorCartera, @eg_ccConcesionario, @eg_ccAseguradora, @eg_ccCooperativa ,@eg_ccCobranzaEstado ,@eg_ccCobranzaGestion " +
                    ") ";
                #endregion
                #region Creacion de comandos
                //Linea1
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DigitoVer1", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@DigitoVer2", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@Solicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Pagare", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@PagarePOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@ComponenteExtraInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                //Linea2
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncorporaMesInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IncorporacionTipo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumDocVerificado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocOpera", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaLiquida", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.SmallDateTime);
                //Linea3
                mySqlCommandSel.Parameters.Add("@FechaVto", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TipoEstado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EstadoDeuda", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IndRestructurado", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumDocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VendedorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorFinalID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCredito", SqlDbType.Int);
                //Linea4
                mySqlCommandSel.Parameters.Add("@TasaEfectivaCredito", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaVenta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaReCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompraCarteraInd", SqlDbType.Bit);
                //Linea5
                mySqlCommandSel.Parameters.Add("@VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAdicional", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPrestamo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDescuento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGiro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMaximoNivelRiesgo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PlazoSeguro", SqlDbType.Int);
                //Linea6
                mySqlCommandSel.Parameters.Add("@Cuota1Seguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotaSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrFinanciaSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaRestructura", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaLiqSeguro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaINI", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaFIN", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DocSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCupoDisponible", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PagoVentanillaInd", SqlDbType.Bit);
                //Linea 7
                mySqlCommandSel.Parameters.Add("@BloqueaVentaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VendidaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocSustituye", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroCesion", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@TipoVenta", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumIncorporaDoc", SqlDbType.Int);
                //Linea 8
                mySqlCommandSel.Parameters.Add("@NumDesIncorporaDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CanceladoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DocAcuerdoPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocEstadoCuenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Abogado", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EtapaIncumplimiento", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);

                mySqlCommandSel.Parameters.Add("@CobranzaEstadoID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@ObsCobranza", SqlDbType.Char, UDT_DescripTExt.MaxLength);

                mySqlCommandSel.Parameters.Add("@FechaReliquidaCJ", SqlDbType.SmallDateTime);
                //Linea 9
                mySqlCommandSel.Parameters.Add("@TipoCreditoID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor1", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor2", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor3", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor4", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor5", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CooperativaID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@Sentencia", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@FechaSentencia", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Juzgado", SqlDbType.Char, 10);

                //Eg
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccVendedorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccTipoCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccConcesionario", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCooperativa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCobranzaEstado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCobranzaGestion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //Linea1
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = credito.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumSolicitud"].Value = credito.NumSolicitud.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = credito.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = credito.ClienteID.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = credito.Libranza.Value;
                mySqlCommandSel.Parameters["@DigitoVer1"].Value = credito.DigitoVer1.Value;
                mySqlCommandSel.Parameters["@DigitoVer2"].Value = credito.DigitoVer2.Value;
                mySqlCommandSel.Parameters["@Solicitud"].Value = credito.Solicitud.Value;
                mySqlCommandSel.Parameters["@Pagare"].Value = credito.Pagare.Value;
                mySqlCommandSel.Parameters["@PagarePOL"].Value = credito.PagarePOL.Value;
                mySqlCommandSel.Parameters["@ComponenteExtraInd"].Value = credito.ComponenteExtraInd.Value;
                mySqlCommandSel.Parameters["@Poliza"].Value = credito.Poliza.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = credito.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = credito.AsesorID.Value;
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = credito.PagaduriaID.Value;
                //Linea2
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = credito.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = credito.ZonaID.Value;
                mySqlCommandSel.Parameters["@IncorporaMesInd"].Value = credito.IncorporaMesInd.Value;
                mySqlCommandSel.Parameters["@IncorporacionTipo"].Value = credito.IncorporacionTipo.Value;
                mySqlCommandSel.Parameters["@NumDocVerificado"].Value = credito.NumDocVerificado.Value;
                mySqlCommandSel.Parameters["@NumDocOpera"].Value = credito.NumDocOpera.Value;
                mySqlCommandSel.Parameters["@PeriodoPago"].Value = credito.PeriodoPago.Value;
                mySqlCommandSel.Parameters["@FechaLiquida"].Value = credito.FechaLiquida.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = credito.FechaCuota1.Value;
                //Linea3
                mySqlCommandSel.Parameters["@FechaVto"].Value = credito.FechaVto.Value;
                mySqlCommandSel.Parameters["@TipoEstado"].Value = credito.TipoEstado.Value;
                mySqlCommandSel.Parameters["@EstadoDeuda"].Value = credito.EstadoDeuda.Value;
                mySqlCommandSel.Parameters["@IndRestructurado"].Value = credito.IndRestructurado.Value;
                mySqlCommandSel.Parameters["@Ciudad"].Value = credito.Ciudad.Value;
                mySqlCommandSel.Parameters["@NumDocCompra"].Value = credito.NumDocCompra.Value;
                mySqlCommandSel.Parameters["@VendedorID"].Value = credito.VendedorID.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = credito.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@CompradorFinalID"].Value = credito.CompradorFinalID.Value;
                mySqlCommandSel.Parameters["@TipoCredito"].Value = credito.TipoCredito.Value;
                //Linea4
                mySqlCommandSel.Parameters["@TasaEfectivaCredito"].Value = credito.TasaEfectivaCredito.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaVenta"].Value = credito.TasaEfectivaVenta.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaReCompra"].Value = credito.TasaEfectivaReCompra.Value;
                mySqlCommandSel.Parameters["@PorInteres"].Value = credito.PorInteres.Value;
                mySqlCommandSel.Parameters["@PorSeguro"].Value = credito.PorSeguro.Value;
                mySqlCommandSel.Parameters["@PorComponente1"].Value = credito.PorComponente1.Value;
                mySqlCommandSel.Parameters["@PorComponente2"].Value = credito.PorComponente2.Value;
                mySqlCommandSel.Parameters["@PorComponente3"].Value = credito.PorComponente3.Value;
                mySqlCommandSel.Parameters["@CompraCarteraInd"].Value = credito.CompraCarteraInd.Value;
                //Linea5
                mySqlCommandSel.Parameters["@VlrSolicitado"].Value = credito.VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@VlrAdicional"].Value = credito.VlrAdicional.Value;
                mySqlCommandSel.Parameters["@VlrPrestamo"].Value = credito.VlrPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = credito.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrCompra"].Value = credito.VlrCompra.Value;
                mySqlCommandSel.Parameters["@VlrDescuento"].Value = credito.VlrDescuento.Value;
                mySqlCommandSel.Parameters["@VlrGiro"].Value = credito.VlrGiro.Value;
                mySqlCommandSel.Parameters["@VlrMaximoNivelRiesgo"].Value = credito.VlrMaximoNivelRiesgo.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = credito.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = credito.Plazo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = credito.VlrCuota.Value;
                mySqlCommandSel.Parameters["@PlazoSeguro"].Value = credito.PlazoSeguro.Value;
                //Linea6
                mySqlCommandSel.Parameters["@Cuota1Seguro"].Value = credito.Cuota1Seguro.Value;
                mySqlCommandSel.Parameters["@VlrCuotaSeguro"].Value = credito.VlrCuotaSeguro.Value;
                mySqlCommandSel.Parameters["@VlrFinanciaSeguro"].Value = credito.VlrFinanciaSeguro.Value;
                mySqlCommandSel.Parameters["@FechaRestructura"].Value = credito.FechaRestructura.Value;
                mySqlCommandSel.Parameters["@FechaLiqSeguro"].Value = credito.FechaLiqSeguro.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaINI"].Value = credito.FechaVigenciaINI.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaFIN"].Value = credito.FechaVigenciaFIN.Value;
                mySqlCommandSel.Parameters["@DocSeguro"].Value = credito.DocSeguro.Value;
                mySqlCommandSel.Parameters["@VlrCupoDisponible"].Value = credito.VlrCupoDisponible.Value;
                mySqlCommandSel.Parameters["@PagoVentanillaInd"].Value = credito.PagoVentanillaInd.Value;
                //Linea 7
                mySqlCommandSel.Parameters["@BloqueaVentaInd"].Value = credito.BloqueaVentaInd.Value;
                mySqlCommandSel.Parameters["@VendidaInd"].Value = credito.VendidaInd.Value;
                mySqlCommandSel.Parameters["@NumeroDocCXP"].Value = credito.NumeroDocCXP.Value;
                mySqlCommandSel.Parameters["@DocSustituye"].Value = credito.DocSustituye.Value;
                mySqlCommandSel.Parameters["@DocVenta"].Value = credito.DocVenta.Value;
                mySqlCommandSel.Parameters["@DocFactura"].Value = credito.DocFactura.Value;
                mySqlCommandSel.Parameters["@NumeroCesion"].Value = credito.NumeroCesion.Value;
                mySqlCommandSel.Parameters["@TipoVenta"].Value = credito.TipoVenta.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = credito.Observacion.Value;
                mySqlCommandSel.Parameters["@NumIncorporaDoc"].Value = credito.NumIncorporaDoc.Value;
                //Linea 8
                mySqlCommandSel.Parameters["@NumDesIncorporaDoc"].Value = credito.NumDesIncorporaDoc.Value;
                mySqlCommandSel.Parameters["@CanceladoInd"].Value = credito.CanceladoInd.Value;
                mySqlCommandSel.Parameters["@DocAcuerdoPago"].Value = credito.DocAcuerdoPago.Value;
                mySqlCommandSel.Parameters["@DocEstadoCuenta"].Value = credito.DocEstadoCuenta.Value;
                mySqlCommandSel.Parameters["@Abogado"].Value = credito.Abogado.Value;
                mySqlCommandSel.Parameters["@EtapaIncumplimiento"].Value = credito.EtapaIncumplimiento.Value;

                mySqlCommandSel.Parameters["@CobranzaEstadoID"].Value = credito.CobranzaEstadoID.Value;
                mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = credito.CobranzaGestionID.Value;//
                mySqlCommandSel.Parameters["@ObsCobranza"].Value = credito.ObsCobranza.Value;

                mySqlCommandSel.Parameters["@FechaReliquidaCJ"].Value = credito.FechaReliquidaCJ.Value;
                //Linea 9
                mySqlCommandSel.Parameters["@TipoCreditoID"].Value = credito.TipoCreditoID.Value;
                mySqlCommandSel.Parameters["@Codeudor1"].Value = credito.Codeudor1.Value;
                mySqlCommandSel.Parameters["@Codeudor2"].Value = credito.Codeudor2.Value;
                mySqlCommandSel.Parameters["@Codeudor3"].Value = credito.Codeudor3.Value;
                mySqlCommandSel.Parameters["@Codeudor4"].Value = credito.Codeudor4.Value;
                mySqlCommandSel.Parameters["@Codeudor5"].Value = credito.Codeudor5.Value;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = credito.ConcesionarioID.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = credito.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@CooperativaID"].Value = credito.CooperativaID.Value;
                mySqlCommandSel.Parameters["@Sentencia"].Value = credito.Sentencia.Value;
                mySqlCommandSel.Parameters["@FechaSentencia"].Value = credito.FechaSentencia.Value;
                mySqlCommandSel.Parameters["@Juzgado"].Value = credito.Juzgado.Value;

                //Eg
                mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAsesor, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccVendedorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccVendedorCartera, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccTipoCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccTipoCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCobranzaEstado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCobranzaEstado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCobranzaGestion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCobranzaGestion, this.Empresa, egCtrl);

                //Concesionario
                if (!string.IsNullOrWhiteSpace(credito.ConcesionarioID.Value))
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccConcesionario, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = DBNull.Value;

                //Aseguradora
                if (!string.IsNullOrWhiteSpace(credito.AseguradoraID.Value))
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = DBNull.Value;

                //Cooperativa
                if (!string.IsNullOrWhiteSpace(credito.CooperativaID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCooperativa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCooperativa, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCooperativa"].Value = DBNull.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCarteraDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCreditoDocu_AddFromccSolicitudDocu(DTO_ccSolicitudDocu solicitud, int numDocNew)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
                #region Query
                mySqlCommandSel.CommandText = " INSERT INTO ccCreditoDocu   " +
                                                       "    (NumeroDoc   " +
                                                       "    ,NumSolicitud   " +
                                                       "    ,EmpresaID   " +
                                                       "    ,ClienteID   " +
                                                       "    ,Libranza   " +
                                                       "    ,DigitoVer1   " +
                                                       "    ,DigitoVer2   " +
                                                       "    ,Solicitud   " +
                                                       "    ,Pagare   " +
                                                       "    ,PagarePol   " +
                                                       "    ,ComponenteExtraInd   " +
                                                       "    ,Poliza   " +
                                                       "    ,LineaCreditoID   " +
                                                       "    ,AsesorID   " +
                                                       "    ,PagaduriaID   " +
                                                       "    ,CentroPagoID   " +
                                                       "    ,ZonaID   " +
                                                       "    ,ConcesionarioID " +
                                                       "    ,AseguradoraID " +
                                                       "    ,CooperativaID " +
                                                       "    ,TipoCreditoID   " +
                                                       "    ,Codeudor1   " +
                                                       "    ,Codeudor2   " +
                                                       "    ,Codeudor3   " +
                                                       "    ,Codeudor4   " +
                                                       "    ,Codeudor5   " +
                                                       "    ,IncorporaMesInd  " +
                                                       "    ,IncorporacionTipo   " +
                                                       "    ,NumDocVerificado   " +
                                                       "    ,NumDocOpera   " +
                                                       "    ,PeriodoPago   " +
                                                       "    ,FechaLiquida  " +
                                                       "    ,FechaCuota1  " +
                                                       "    ,FechaVto  " +
                                                       "    ,TipoEstado   " +
                                                       "    ,EstadoDeuda   " +
                                                       "    ,Ciudad   " +
                                                       "    ,NumDocCompra   " +
                                                       "    ,VendedorID   " +
                                                       "    ,TipoCredito   " +
                                                       "    ,TasaEfectivaCredito   " +
                                                       "    ,TasaEfectivaVenta   " +
                                                       "    ,TasaEfectivaReCompra   " +
                                                       "    ,PorInteres   " +
                                                       "    ,PorSeguro   " +
                                                       "    ,PorComponente1   " +
                                                       "    ,PorComponente2   " +
                                                       "    ,PorComponente3   " +
                                                       "    ,CompraCarteraInd   " +
                                                       "    ,VlrSolicitado   " +
                                                       "    ,VlrAdicional   " +
                                                       "    ,VlrPrestamo   " +
                                                       "    ,VlrLibranza  " +
                                                       "    ,VlrCompra   " +
                                                       "    ,VlrDescuento   " +
                                                       "    ,VlrGiro   " +
                                                       "    ,VlrMaximoNivelRiesgo " +
                                                       "    ,VlrPoliza   " +
                                                       "    ,Plazo   " +
                                                       "    ,VlrCuota   " +
                                                       "    ,PlazoSeguro  " +
                                                       "    ,Cuota1Seguro  " +
                                                       "    ,VlrCuotaSeguro  " +
                                                       "    ,VlrFinanciaSeguro  " +
                                                       "    ,FechaLiqSeguro  " +
                                                       "    ,FechaVigenciaINI  " +
                                                       "    ,FechaVigenciaFIN  " +
                                                       "    ,DocSeguro  " +
                                                       "    ,VlrCupoDisponible   " +
                                                       "    ,PagoVentanillaInd   " +
                                                       "    ,BloqueaVentaInd " +
                                                       "    ,VendidaInd " +
                                                       "    ,DocVenta " +
                                                       "    ,DocFactura " +
                                                       "    ,TipoVenta   " +
                                                       "    ,Observacion   " +
                                                       "    ,NumIncorporaDoc" +
                                                       "    ,NumDesIncorporaDoc" +
                                                       "    ,CanceladoInd   " +
                                                       "    ,DocAcuerdoPago   " +
                                                       "    ,eg_ccCliente   " +
                                                       "    ,eg_ccLineaCredito   " +
                                                       "    ,eg_ccAsesor   " +
                                                       "    ,eg_ccPagaduria   " +
                                                       "    ,eg_ccCentroPagoPAG  " +
                                                       "    ,eg_glZona   " +
                                                       "    ,eg_ccConcesionario " +
                                                       "    ,eg_ccAseguradora " +
                                                       "    ,eg_ccCooperativa " + 
                                                       "    ,eg_glLugarGeografico  " +
                                                       "    ,eg_ccVendedorCartera " +
                                                       "    ,eg_ccTipoCredito) " +
                                                       " SELECT        " +
                                                        "  @NumeroDoc     " +
                                                        " ,@NumSolicitud  " +
                                                        " ,EmpresaID   " +
                                                        " ,ClienteID     " +
                                                        " ,@Libranza     " +
                                                        " ,@DigitoVer1     " +
                                                        " ,@DigitoVer2     " +
                                                        " ,Solicitud   " +
                                                        " ,Pagare   " +
                                                        " ,PagarePol   " +
                                                        " ,ComponenteExtraInd   " +
                                                        " ,Poliza   " +
                                                        " ,LineaCreditoID     " +
                                                        " ,AsesorID     " +
                                                        " ,PagaduriaID     " +
                                                        " ,CentroPagoID     " +
                                                        " ,ZonaID     " +
                                                       "  ,ConcesionarioID " +
                                                       "  ,AseguradoraID " +
                                                       "  ,CooperativaID " +
                                                        " ,TipoCreditoID     " +
                                                        " ,Codeudor1     " +
                                                        " ,Codeudor2     " +
                                                        " ,Codeudor3     " +
                                                        " ,Codeudor4     " +
                                                        " ,Codeudor5     " +
                                                        " ,IncorporaMesInd    " +
                                                        " ,IncorporacionTipo " +
                                                        " ,NumDocVerificado " +
                                                        " ,NumDocOpera " +
                                                        " ,PeriodoPago " +
                                                        " ,@FechaLiquida  " +
                                                        " ,FechaCuota1    " +
                                                        " ,@FechaVto  " +
                                                        " ,@TipoEstado  " +
                                                        " ,@EstadoDeuda " +
                                                        " ,Ciudad     " +
                                                        " ,NumDocCompra   " +
                                                        " ,VendedorID   " +
                                                        " ,TipoCredito    " +
                                                        " ,TasaEfectivaCredito    " +
                                                        " ,@TasaEfectivaVenta    " +
                                                        " ,@TasaEfectivaReCompra    " +
                                                        " ,PorInteres     " +
                                                        " ,PorSeguro     " +
                                                        " ,PorComponente1     " +
                                                        " ,PorComponente2     " +
                                                        " ,PorComponente3     " +
                                                        " ,CompraCarteraInd     " +
                                                        " ,VlrSolicitado     " +
                                                        " ,VlrAdicional     " +
                                                        " ,VlrPrestamo     " +
                                                        " ,VlrLibranza     " +
                                                        " ,VlrCompra     " +
                                                        " ,VlrDescuento    " +
                                                        " ,VlrGiro     " +
                                                        " ,@VlrMaximoNivelRiesgo " +
                                                        " ,VlrPoliza   " +
                                                        " ,Plazo     " +
                                                        " ,VlrCuota    " +
                                                        " ,PlazoSeguro  " +
                                                        " ,Cuota1Seguro  " +
                                                        " ,VlrCuotaSeguro  " +
                                                        " ,VlrFinanciaSeguro  " +
                                                        " ,FechaLiqSeguro  " +
                                                        " ,FechaVigenciaINI  " +
                                                        " ,FechaVigenciaFIN  " +
                                                        " ,DocSeguro  " +
                                                        " ,VlrCupoDisponible     " +
                                                        " ,PagoVentanillaInd     " +
                                                        " ,@BloqueaVentaInd" +
                                                        " ,@VendidaInd" +
                                                        " ,@DocVenta" +
                                                        " ,@DocFactura" +
                                                        " ,@TipoVenta " +
                                                        " ,Observacion     " +
                                                        " ,@NumIncorporaDoc " +
                                                        " ,@NumDesIncorporaDoc " +
                                                        " ,@CanceladoInd " +
                                                        " ,@DocAcuerdoPago " +
                                                        " ,eg_ccCliente     " +
                                                        " ,eg_ccLineaCredito  " +
                                                        " ,eg_ccAsesor     " +
                                                        " ,eg_ccPagaduria    " +
                                                        " ,eg_ccCentroPagoPAG    " +
                                                        " ,eg_glZona     " +
                                                       "  ,eg_ccConcesionario " +
                                                       "  ,eg_ccAseguradora " +
                                                       "  ,eg_ccCooperativa " +
                                                        " ,eg_glLugarGeografico   " +
                                                        " ,eg_ccVendedorCartera   " +
                                                        " ,eg_ccTipoCredito   " +
                                                        " FROM ccSolicitudDocu " +
                                                        " WHERE NumeroDoc =  @numDocOld";
                #endregion
                #region Creacion de parametros

                mySqlCommandSel.Parameters.Add("@numDocOld", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaLiquida", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVto", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaVenta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaReCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoEstado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PagoVentanillaInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@BloqueaVentaInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VendidaInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CanceladoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DocAcuerdoPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMaximoNivelRiesgo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumIncorporaDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDesIncorporaDoc", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@eg_ccGestionCobranza", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DigitoVer1", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@DigitoVer2", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@FechaEstado", SqlDbType.DateTime);

                #endregion
                #region Asigna los valores

                mySqlCommandSel.Parameters["@numDocOld"].Value = solicitud.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDocNew;
                mySqlCommandSel.Parameters["@Libranza"].Value = solicitud.Libranza.Value;
                mySqlCommandSel.Parameters["@NumSolicitud"].Value = solicitud.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FechaLiquida"].Value = solicitud.FechaLiquida.Value;
                mySqlCommandSel.Parameters["@FechaVto"].Value = solicitud.FechaVto.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaVenta"].Value = 0;
                mySqlCommandSel.Parameters["@TasaEfectivaReCompra"].Value = 0;
                mySqlCommandSel.Parameters["@TipoEstado"].Value = (int)TipoEstadoCartera.Propia;
                mySqlCommandSel.Parameters["@EstadoDeuda"].Value = (byte)EstadoDeuda.Normal;
                mySqlCommandSel.Parameters["@PagoVentanillaInd"].Value = false;
                mySqlCommandSel.Parameters["@BloqueaVentaInd"].Value = false;
                mySqlCommandSel.Parameters["@VendidaInd"].Value = false;
                mySqlCommandSel.Parameters["@DocVenta"].Value = 0;
                mySqlCommandSel.Parameters["@DocFactura"].Value = 0;
                mySqlCommandSel.Parameters["@CanceladoInd"].Value = 0;
                mySqlCommandSel.Parameters["@DocAcuerdoPago"].Value = 0;
                mySqlCommandSel.Parameters["@TipoVenta"].Value = 2;
                mySqlCommandSel.Parameters["@VlrMaximoNivelRiesgo"].Value = 0;
                mySqlCommandSel.Parameters["@NumIncorporaDoc"].Value = 0;
                mySqlCommandSel.Parameters["@NumDesIncorporaDoc"].Value = 0;
                mySqlCommandSel.Parameters["@DigitoVer1"].Value = 0;
                mySqlCommandSel.Parameters["@DigitoVer2"].Value = 0;
                mySqlCommandSel.Parameters["@FechaEstado"].Value = DBNull.Value;

                //mySqlCommandSel.Parameters["@eg_ccGestionCobranza"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccGestionCobranza, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_AddFromSolicitudDocu");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccCreditoDocu_Update(DTO_ccCreditoDocu credito)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                           "UPDATE ccCreditoDocu SET" +
                                               " NumeroDoc = @NumeroDoc " +
                                               " ,NumSolicitud = @NumSolicitud " +
                                               " ,EmpresaID = @EmpresaID " +
                                               " ,ClienteID = @ClienteID" +
                                               " ,Libranza = @Libranza" +
                                               " ,DigitoVer1 = @DigitoVer1" +
                                               " ,DigitoVer2 = @DigitoVer2" +
                                               " ,Solicitud = @Solicitud" +
                                               " ,Pagare = @Pagare" +
                                               " ,PagarePOL = @PagarePOL" +
                                               " ,ComponenteExtraInd = @ComponenteExtraInd" +
                                               " ,Poliza = @Poliza" +
                                               " ,LineaCreditoID = @LineaCreditoID" +
                                               " ,AsesorID = @AsesorID" +
                                               " ,ConcesionarioID = @ConcesionarioID  " +
                                               " ,AseguradoraID = @AseguradoraID  " +
                                               " ,CooperativaID = @CooperativaID  " +
                                               " ,PagaduriaID = @PagaduriaID" +
                                               " ,CentroPagoID = @CentroPagoID" +
                                               " ,ZonaID = @ZonaID " +
                                               " ,TipoCreditoID = @TipoCreditoID " +
                                               " ,Codeudor1 = @Codeudor1 " +
                                               " ,Codeudor2 = @Codeudor2 " +
                                               " ,Codeudor3 = @Codeudor3 " +
                                               " ,Codeudor4 = @Codeudor4 " +
                                               " ,Codeudor5 = @Codeudor5 " +
                                               " ,IncorporaMesInd = @IncorporaMesInd " +
                                               " ,IncorporacionTipo = @IncorporacionTipo" +
                                               " ,NumDocVerificado = @NumDocVerificado" +
                                               " ,NumDocOpera = @NumDocOpera" +
                                               " ,PeriodoPago = @PeriodoPago" +
                                               " ,FechaLiquida = @FechaLiquida " +
                                               " ,FechaCuota1 = @FechaCuota1 " +
                                               " ,FechaVto = @FechaVto " +
                                               " ,TipoEstado = @TipoEstado " +
                                               " ,EstadoDeuda = @EstadoDeuda " +
                                               " ,IndRestructurado = @IndRestructurado " +
                                               " ,Ciudad = @Ciudad " +
                                               " ,NumDocCompra = @NumDocCompra" +
                                               " ,VendedorID = @VendedorID" +
                                               " ,CompradorCarteraID = @CompradorCarteraID " +
                                               " ,CompradorFinalID = @CompradorFinalID " +
                                               " ,TipoCredito = @TipoCredito " +
                                               " ,TasaEfectivaCredito = @TasaEfectivaCredito " +
                                               " ,TasaEfectivaVenta = @TasaEfectivaVenta " +
                                               " ,TasaEfectivaReCompra = @TasaEfectivaReCompra " +
                                               " ,PorInteres = @PorInteres " +
                                               " ,PorSeguro = @PorSeguro " +
                                               " ,PorComponente1 = @PorComponente1 " +
                                               " ,PorComponente2 = @PorComponente2 " +
                                               " ,PorComponente3 = @PorComponente3 " +
                                               " ,CompraCarteraInd = @CompraCarteraInd " +
                                               " ,VlrSolicitado = @VlrSolicitado " +
                                               " ,VlrAdicional = @VlrAdicional " +
                                               " ,VlrPrestamo = @VlrPrestamo " +
                                               " ,VlrLibranza = @VlrLibranza " +
                                               " ,VlrCompra = @VlrCompra " +
                                               " ,VlrDescuento = @VlrDescuento " +
                                               " ,VlrGiro = @VlrGiro " +
                                               " ,VlrMaximoNivelRiesgo = @VlrMaximoNivelRiesgo " +
                                               " ,VlrPoliza = @VlrPoliza" +
                                               " ,Plazo = @Plazo " +
                                               " ,VlrCuota = @VlrCuota " +
                                               " ,PlazoSeguro = @PlazoSeguro " +
                                               " ,Cuota1Seguro = @Cuota1Seguro " +
                                               " ,VlrCuotaSeguro = @VlrCuotaSeguro " +
                                               " ,VlrFinanciaSeguro = @VlrFinanciaSeguro " +
                                               " ,FechaRestructura = @FechaRestructura " +
                                               " ,FechaLiqSeguro = @FechaLiqSeguro " +
                                               " ,FechaVigenciaINI = @FechaVigenciaINI " +
                                               " ,FechaVigenciaFIN = @FechaVigenciaFIN " +
                                               " ,DocSeguro = @DocSeguro " +
                                               " ,VlrCupoDisponible = @VlrCupoDisponible " +
                                               " ,PagoVentanillaInd = @PagoVentanillaInd " +
                                               " ,BloqueaVentaInd = @BloqueaVentaInd " +
                                               " ,VendidaInd = @VendidaInd " +
                                               " ,NumeroDocCXP = @NumeroDocCXP " +
                                               " ,DocSustituye = @DocSustituye " +
                                               " ,DocVenta = @DocVenta " +
                                               " ,DocFactura = @DocFactura " +
                                               " ,NumeroCesion = @NumeroCesion " +
                                               " ,TipoVenta = @TipoVenta " +
                                               " ,Observacion = @Observacion " +
                                               " ,NumIncorporaDoc = @NumIncorporaDoc" +
                                               " ,NumDesIncorporaDoc = @NumDesIncorporaDoc" +
                                               " ,CanceladoInd = @CanceladoInd " +
                                               " ,DocAcuerdoPago = @DocAcuerdoPago " +
                                               " ,DocEstadoCuenta = @DocEstadoCuenta " +
                                               " ,Abogado = @Abogado " +
                                               " ,EtapaIncumplimiento = @EtapaIncumplimiento " +
                                               " ,CobranzaEstadoID = @CobranzaEstadoID " +
                                               " ,CobranzaGestionID = @CobranzaGestionID " +
                                               " ,CobranzaGestionCierre = @CobranzaGestionCierre " +
                                               " ,ObsCobranza = @ObsCobranza " +
                                               " ,FechaReliquidaCJ = @FechaReliquidaCJ " +
                                               " ,DocRechazo = @DocRechazo " +
                                               " ,DocDesestimiento = @DocDesestimiento " +
                                               " ,SustituidoInd = @SustituidoInd " +
                                               " ,DocPrepago = @DocPrepago " +
                                               " ,VlrPrepago = @VlrPrepago " +
                                               " ,DocUltNomina = @DocUltNomina " +
                                               " ,DocUltRecaudo = @DocUltRecaudo " +
                                               " ,NovedadIncorporaID = @NovedadIncorporaID " +
                                               " ,SiniestroEstadoID = @SiniestroEstadoID " +
                                               " ,Sentencia=Sentencia" +
                                               " ,FechaSentencia=@FechaSentencia" +
                                               " ,Juzgado=@Juzgado" +
                                               " ,eg_ccCompradorCartera = @eg_ccCompradorCartera " +
                                               " ,eg_ccConcesionario = @eg_ccConcesionario " +
                                               " ,eg_ccAseguradora = @eg_ccAseguradora " +
                                               " ,eg_ccCooperativa = @eg_ccCooperativa " +
                                               " ,eg_ccCobranzaEstado = @eg_ccCobranzaEstado " +
                                               " ,eg_ccCobranzaGestion = @eg_ccCobranzaGestion " +
                                               " ,eg_ccIncorporacionNovedad = @eg_ccIncorporacionNovedad " +
                                               " ,eg_ccSiniestroEstado = @eg_ccSiniestroEstado " +
                                               " ,eg_ccAbogado = @eg_ccAbogado " +
                                               " WHERE  NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CooperativaID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCreditoID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor1", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor2", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor3", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor4", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor5", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaLiquida", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVto", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TipoEstado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EstadoDeuda", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@IndRestructurado", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VendedorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaCredito", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaVenta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaReCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorFinalID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompraCarteraInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAdicional", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPrestamo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDescuento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGiro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrMaximoNivelRiesgo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncorporaMesInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IncorporacionTipo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumDocVerificado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocOpera", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumDocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumIncorporaDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDesIncorporaDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCupoDisponible", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PagoVentanillaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@BloqueaVentaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VendidaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocSustituye", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroCesion", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@TipoVenta", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@CanceladoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DocAcuerdoPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocEstadoCuenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Abogado", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EtapaIncumplimiento", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CobranzaEstadoID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CobranzaGestionID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CobranzaGestionCierre", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@ObsCobranza", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaEstado", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DigitoVer1", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@DigitoVer2", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@Solicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Pagare", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@PagarePOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@ComponenteExtraInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PlazoSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Cuota1Seguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotaSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrFinanciaSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaRestructura", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaLiqSeguro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaINI", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaFIN", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DocSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaReliquidaCJ", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DocRechazo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocDesestimiento", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@SustituidoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DocPrepago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrPrepago", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DocUltNomina", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocUltRecaudo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NovedadIncorporaID", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@SiniestroEstadoID", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@Sentencia", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@FechaSentencia", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Juzgado", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccConcesionario", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCooperativa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCobranzaEstado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCobranzaGestion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccIncorporacionNovedad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccSiniestroEstado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = credito.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumSolicitud"].Value = credito.NumSolicitud.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = credito.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = credito.ClienteID.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = credito.Libranza.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = credito.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = credito.AsesorID.Value;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = credito.ConcesionarioID.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = credito.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@CooperativaID"].Value = credito.CooperativaID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = credito.ZonaID.Value;
                mySqlCommandSel.Parameters["@TipoCreditoID"].Value = credito.TipoCreditoID.Value;
                mySqlCommandSel.Parameters["@Codeudor1"].Value = credito.Codeudor1.Value;
                mySqlCommandSel.Parameters["@Codeudor2"].Value = credito.Codeudor2.Value;
                mySqlCommandSel.Parameters["@Codeudor3"].Value = credito.Codeudor3.Value;
                mySqlCommandSel.Parameters["@Codeudor4"].Value = credito.Codeudor4.Value;
                mySqlCommandSel.Parameters["@Codeudor5"].Value = credito.Codeudor5.Value;
                mySqlCommandSel.Parameters["@FechaLiquida"].Value = credito.FechaLiquida.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = credito.FechaCuota1.Value;
                mySqlCommandSel.Parameters["@FechaVto"].Value = credito.FechaVto.Value;
                mySqlCommandSel.Parameters["@TipoEstado"].Value = credito.TipoEstado.Value;
                mySqlCommandSel.Parameters["@EstadoDeuda"].Value = credito.EstadoDeuda.Value;
                mySqlCommandSel.Parameters["@IndRestructurado"].Value = credito.IndRestructurado.Value;
                mySqlCommandSel.Parameters["@Ciudad"].Value = credito.Ciudad.Value;
                mySqlCommandSel.Parameters["@VendedorID"].Value = credito.VendedorID.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaCredito"].Value = credito.TasaEfectivaCredito.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaVenta"].Value = credito.TasaEfectivaVenta.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaReCompra"].Value = credito.TasaEfectivaReCompra.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = credito.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@CompradorFinalID"].Value = credito.CompradorFinalID.Value;
                mySqlCommandSel.Parameters["@TipoCredito"].Value = credito.TipoCredito.Value;
                mySqlCommandSel.Parameters["@PorInteres"].Value = credito.PorInteres.Value;
                mySqlCommandSel.Parameters["@PorSeguro"].Value = credito.PorSeguro.Value;
                mySqlCommandSel.Parameters["@PorComponente1"].Value = credito.PorComponente1.Value;
                mySqlCommandSel.Parameters["@PorComponente2"].Value = credito.PorComponente2.Value;
                mySqlCommandSel.Parameters["@PorComponente3"].Value = credito.PorComponente3.Value;
                mySqlCommandSel.Parameters["@CompraCarteraInd"].Value = credito.CompraCarteraInd.Value;
                mySqlCommandSel.Parameters["@VlrSolicitado"].Value = credito.VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@VlrAdicional"].Value = credito.VlrAdicional.Value;
                mySqlCommandSel.Parameters["@VlrPrestamo"].Value = credito.VlrPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = credito.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrCompra"].Value = credito.VlrCompra.Value;
                mySqlCommandSel.Parameters["@VlrDescuento"].Value = credito.VlrDescuento.Value;
                mySqlCommandSel.Parameters["@VlrGiro"].Value = credito.VlrGiro.Value;
                mySqlCommandSel.Parameters["@VlrMaximoNivelRiesgo"].Value = credito.VlrMaximoNivelRiesgo.Value;
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = credito.PagaduriaID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = credito.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@IncorporaMesInd"].Value = credito.IncorporaMesInd.Value;
                mySqlCommandSel.Parameters["@IncorporacionTipo"].Value = credito.IncorporacionTipo.Value;
                mySqlCommandSel.Parameters["@NumDocVerificado"].Value = credito.NumDocVerificado.Value;
                mySqlCommandSel.Parameters["@NumDocOpera"].Value = credito.NumDocOpera.Value;
                mySqlCommandSel.Parameters["@PeriodoPago"].Value = credito.PeriodoPago.Value;
                mySqlCommandSel.Parameters["@NumDocCompra"].Value = credito.NumDocCompra.Value;
                mySqlCommandSel.Parameters["@NumIncorporaDoc"].Value = credito.NumIncorporaDoc.Value;
                mySqlCommandSel.Parameters["@NumDesIncorporaDoc"].Value = credito.NumDesIncorporaDoc.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = credito.Plazo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = credito.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCupoDisponible"].Value = credito.VlrCupoDisponible.Value;
                mySqlCommandSel.Parameters["@PagoVentanillaInd"].Value = credito.PagoVentanillaInd.Value;
                mySqlCommandSel.Parameters["@BloqueaVentaInd"].Value = credito.BloqueaVentaInd.Value;
                mySqlCommandSel.Parameters["@VendidaInd"].Value = credito.VendidaInd.Value;
                mySqlCommandSel.Parameters["@NumeroDocCXP"].Value = credito.NumeroDocCXP.Value;
                mySqlCommandSel.Parameters["@DocSustituye"].Value = credito.DocSustituye.Value;
                mySqlCommandSel.Parameters["@DocVenta"].Value = credito.DocVenta.Value;
                mySqlCommandSel.Parameters["@DocFactura"].Value = credito.DocFactura.Value;
                mySqlCommandSel.Parameters["@NumeroCesion"].Value = credito.NumeroCesion.Value;
                mySqlCommandSel.Parameters["@TipoVenta"].Value = credito.TipoVenta.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = credito.Observacion.Value;
                mySqlCommandSel.Parameters["@CanceladoInd"].Value = credito.CanceladoInd.Value;
                mySqlCommandSel.Parameters["@DocAcuerdoPago"].Value = credito.DocAcuerdoPago.Value;
                mySqlCommandSel.Parameters["@DocEstadoCuenta"].Value = credito.DocEstadoCuenta.Value;
                mySqlCommandSel.Parameters["@Abogado"].Value = credito.Abogado.Value;
                mySqlCommandSel.Parameters["@EtapaIncumplimiento"].Value = credito.EtapaIncumplimiento.Value;
                mySqlCommandSel.Parameters["@CobranzaEstadoID"].Value = credito.CobranzaEstadoID.Value;
                mySqlCommandSel.Parameters["@CobranzaGestionID"].Value = credito.CobranzaGestionID.Value;
                mySqlCommandSel.Parameters["@CobranzaGestionCierre"].Value = credito.CobranzaGestionCierre.Value;
                mySqlCommandSel.Parameters["@ObsCobranza"].Value = credito.ObsCobranza.Value;
                mySqlCommandSel.Parameters["@FechaEstado"].Value = credito.FechaEstado.Value;
                mySqlCommandSel.Parameters["@DigitoVer1"].Value = credito.DigitoVer1.Value;
                mySqlCommandSel.Parameters["@DigitoVer2"].Value = credito.DigitoVer2.Value;
                mySqlCommandSel.Parameters["@Solicitud"].Value = credito.Solicitud.Value;
                mySqlCommandSel.Parameters["@Pagare"].Value = credito.Pagare.Value;
                mySqlCommandSel.Parameters["@PagarePOL"].Value = credito.PagarePOL.Value;
                mySqlCommandSel.Parameters["@ComponenteExtraInd"].Value = credito.ComponenteExtraInd.Value;
                mySqlCommandSel.Parameters["@Poliza"].Value = credito.Poliza.Value;
                mySqlCommandSel.Parameters["@PlazoSeguro"].Value = credito.PlazoSeguro.Value;
                mySqlCommandSel.Parameters["@Cuota1Seguro"].Value = credito.Cuota1Seguro.Value;
                mySqlCommandSel.Parameters["@VlrCuotaSeguro"].Value = credito.VlrCuotaSeguro.Value;
                mySqlCommandSel.Parameters["@VlrFinanciaSeguro"].Value = credito.VlrFinanciaSeguro.Value;
                mySqlCommandSel.Parameters["@FechaRestructura"].Value = credito.FechaRestructura.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = credito.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@FechaLiqSeguro"].Value = credito.FechaLiqSeguro.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaINI"].Value = credito.FechaVigenciaINI.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaFIN"].Value = credito.FechaVigenciaFIN.Value;
                mySqlCommandSel.Parameters["@DocSeguro"].Value = credito.DocSeguro.Value;
                mySqlCommandSel.Parameters["@DocRechazo"].Value = credito.DocRechazo.Value;
                mySqlCommandSel.Parameters["@FechaReliquidaCJ"].Value = credito.FechaReliquidaCJ.Value;
                mySqlCommandSel.Parameters["@DocDesestimiento"].Value = credito.DocDesestimiento.Value;
                mySqlCommandSel.Parameters["@SustituidoInd"].Value = credito.SustituidoInd.Value;
                mySqlCommandSel.Parameters["@DocPrepago"].Value = credito.DocPrepago.Value;
                mySqlCommandSel.Parameters["@VlrPrepago"].Value = credito.VlrPrepago.Value;
                mySqlCommandSel.Parameters["@DocUltNomina"].Value = credito.DocUltNomina.Value;
                mySqlCommandSel.Parameters["@DocUltRecaudo"].Value = credito.DocUltRecaudo.Value;
                mySqlCommandSel.Parameters["@NovedadIncorporaID"].Value = credito.NovedadIncorporaID.Value;
                mySqlCommandSel.Parameters["@SiniestroEstadoID"].Value = credito.SiniestroEstadoID.Value;
                mySqlCommandSel.Parameters["@Sentencia"].Value = credito.Sentencia.Value;
                mySqlCommandSel.Parameters["@FechaSentencia"].Value = credito.FechaSentencia.Value;
                mySqlCommandSel.Parameters["@Juzgado"].Value = credito.Juzgado.Value;

                //Vendedor
                if (!String.IsNullOrWhiteSpace(credito.CompradorCarteraID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = DBNull.Value;

                //Concesionario
                if (!string.IsNullOrWhiteSpace(credito.ConcesionarioID.Value))
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccConcesionario, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = DBNull.Value;

                //Aseguradora
                if (!string.IsNullOrWhiteSpace(credito.AseguradoraID.Value))
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = DBNull.Value;

                //Cooperativa
                if (!string.IsNullOrWhiteSpace(credito.CooperativaID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCooperativa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCooperativa, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCooperativa"].Value = DBNull.Value;

                //CobranzaEstado
                if (!string.IsNullOrWhiteSpace(credito.CobranzaEstadoID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCobranzaEstado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCobranzaEstado, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCobranzaEstado"].Value = DBNull.Value;

                //CobranzaGestion
                if (!string.IsNullOrWhiteSpace(credito.CobranzaGestionID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCobranzaGestion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCobranzaGestion, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCobranzaGestion"].Value = DBNull.Value;

                //Incorpora Novedad
                if (!string.IsNullOrWhiteSpace(credito.NovedadIncorporaID.Value))
                    mySqlCommandSel.Parameters["@eg_ccIncorporacionNovedad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccIncorporacionNovedad, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccIncorporacionNovedad"].Value = DBNull.Value;

                //Siniestro Estado
                if (!string.IsNullOrWhiteSpace(credito.SiniestroEstadoID.Value))
                    mySqlCommandSel.Parameters["@eg_ccSiniestroEstado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccSiniestroEstado, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccSiniestroEstado"].Value = DBNull.Value;

                //Abogado
                if (!string.IsNullOrWhiteSpace(credito.Abogado.Value))
                    mySqlCommandSel.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccAbogado"].Value = DBNull.Value;

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
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_Update");
                throw exception;
            }
        }

        #endregion

        #region Generales

        /// <summary>
        /// Trae la info de un credito
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranza">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccCreditoDocu DAL_ccCreditoDocu_GetByLibranza(int libranza)
        {
            try
            {
                DTO_ccCreditoDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@Libranza"].Value = libranza;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "SELECT cred.*, cli.Descriptivo as Nombre, ctrl.Estado " +
                                       "FROM ccCreditoDocu cred with(nolock) " +
                                       "    INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cred.eg_ccCliente = cli.EmpresaGrupoID " +
                                        "	INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cred.NumeroDoc  " +
                                       "WHERE cred.EmpresaID = @EmpresaID and cred.Libranza = @Libranza ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoDocu(dr);
                    result.Nombre.Value = dr["Nombre"].ToString();
                    result.Estado.Value = Convert.ToByte(dr["Estado"]);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los creditos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetByCliente(string cliente)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "SELECT cred.*, cli.Descriptivo as Nombre, ctrl.Estado " +
                                           "FROM ccCreditoDocu cred with(nolock) " +
                                           "INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                                           "INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente   " +
                                           "WHERE cred.EmpresaID = @EmpresaID and cred.ClienteID = @ClienteID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                result = new List<DTO_ccCreditoDocu>();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.Estado.Value = Convert.ToByte(dr["Estado"]);
                    result.Add(dto);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetByCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de todos los creditos aprobados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="allEmpresas">Indica si trae la infomacion de todas las empresas</param>
        /// <returns></returns>     
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetForAprobacion(string actFlujoID, DateTime fechaActual, string usuarioID, bool allEmpresas)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string query = string.Empty;
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaActual", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@FechaActual"].Value = fechaActual;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = usuarioID;
                #endregion
                if (!allEmpresas)
                {
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                    query = " and crediDocu.EmpresaID=@EmpresaID";
                }
                #region CommandText
                mySqlCommandSel.CommandText =
                    " SELECT crediDocu.*  " +
                    " FROM ccCreditoDocu crediDocu with(nolock)  " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) on crediDocu.NumeroDoc = ctrl.NumeroDoc " +
                    "	INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    AND act.CerradoInd=@CerradoInd AND act.actividadFlujoID=@ActividadFlujoID " +
                    "	INNER JOIN glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "		AND perm.areaFuncionalID = ctrl.areaFuncionalID and perm.UsuarioID = @UsuarioID " +
                    " WHERE perm.actividadFlujoID = @ActividadFlujoID AND DATEDIFF(MONTH,crediDocu.FechaLiquida,@FechaActual) >= 0 " + query +
                    " ORDER BY CAST(crediDocu.Libranza AS INTEGER) ";

                #endregion
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    dto.Observacion.Value = string.Empty;
                    dto.Rechazado.Value = false;
                    dto.Aprobado.Value = false;
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetForAprobacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="actAprobacionGiro">Actividad para el documento de aprobacion de giro</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosPendientesByCliente(string clienteID, string actAprobacionGiro, bool validCancelado = true)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string cancelado = string.Empty;
                if (validCancelado)
                    cancelado = " and cred.CanceladoInd = @CanceladoInd";

                string queryAct = string.Empty;
                if(!string.IsNullOrWhiteSpace(actAprobacionGiro))
                {
                    mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);

                    mySqlCommand.Parameters["@ActividadFlujoID"].Value = actAprobacionGiro;
                    mySqlCommand.Parameters["@CerradoInd"].Value = true;

                    queryAct =
                        "	INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cred.NumeroDoc " +
                        "       AND act.actividadFlujoID=@ActividadFlujoID AND act.CerradoInd=@CerradoInd ";
                }

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@ClienteID"].Value = clienteID;
                mySqlCommand.Parameters["@CanceladoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT DISTINCT cred.*, his.EC_Fecha, his.EC_FijadoInd, his.EC_ValorPago, his.EC_Proposito, his.EC_PrimeraCtaPagada, his.NumDocProceso " +
                    "FROM ccCreditoDocu cred with(nolock) " +
                    "   LEFT JOIN ccEstadoCuentaHistoria his with(nolock) on his.NumeroDoc = cred.DocEstadoCuenta " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cred.NumeroDoc AND ctrl.Estado=@Estado " + queryAct + 
                    "WHERE cred.ClienteID=@ClienteID " + cancelado+ "  and cred.EmpresaID=@EmpresaID ";

                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    if(!string.IsNullOrWhiteSpace(dr["EC_Fecha"].ToString()))
                        dto.EC_Fecha.Value = Convert.ToDateTime(dr["EC_Fecha"]);
                    if (!string.IsNullOrWhiteSpace(dr["EC_FijadoInd"].ToString()))
                        dto.EC_FijadoInd.Value = Convert.ToBoolean(dr["EC_FijadoInd"]);
                    else
                        dto.EC_FijadoInd.Value = false;
                    if (!string.IsNullOrWhiteSpace(dr["EC_Proposito"].ToString()))
                        dto.EC_Proposito.Value = Convert.ToByte(dr["EC_Proposito"]);
                    else
                        dto.EC_Proposito.Value = 0;
                    if (!string.IsNullOrWhiteSpace(dr["EC_PrimeraCtaPagada"].ToString()))
                        dto.EC_PrimeraCtaPagada.Value = Convert.ToInt32(dr["EC_PrimeraCtaPagada"]);
                    else
                        dto.EC_PrimeraCtaPagada.Value = 1;
                    if(!string.IsNullOrWhiteSpace(dr["EC_ValorPago"].ToString()))
                        dto.EC_ValorPago.Value = Convert.ToDecimal(dr["EC_ValorPago"]);
                    if (!string.IsNullOrWhiteSpace(dr["NumDocProceso"].ToString()))
                        dto.NumDocProceso.Value = Convert.ToInt32(dr["NumDocProceso"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosPendientesByCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="actAprobacionGiro">Actividad para el documento de aprobacion de giro</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosPendientesByClienteAndEstado(string clienteID, List<byte> estados)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if(estados.Count == 0)
                    return result;

                string queryEstados = string.Empty;
                for (int i = 0; i < estados.Count; ++i)
                {
                    queryEstados += estados[i].ToString();
                    if (i != estados.Count - 1)
                        queryEstados += ",";
                }

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@ClienteID"].Value = clienteID;
                mySqlCommand.Parameters["@CanceladoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT DISTINCT cred.*, his.EC_Fecha, his.EC_FijadoInd, his.EC_ValorPago, his.EC_Proposito, his.EC_PrimeraCtaPagada, his.NumDocProceso " +
                    "FROM ccCreditoDocu cred with(nolock) " +
                    "   LEFT JOIN ccEstadoCuentaHistoria his with(nolock) on his.NumeroDoc = cred.DocEstadoCuenta " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cred.NumeroDoc AND ctrl.Estado=@Estado " +
                    "WHERE cred.EmpresaID=@EmpresaID and cred.ClienteID=@ClienteID and cred.CanceladoInd=@CanceladoInd and cred.EstadoDeuda in (" + queryEstados + ") ";

                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    if (!string.IsNullOrWhiteSpace(dr["EC_Fecha"].ToString()))
                        dto.EC_Fecha.Value = Convert.ToDateTime(dr["EC_Fecha"]);
                    if (!string.IsNullOrWhiteSpace(dr["EC_FijadoInd"].ToString()))
                        dto.EC_FijadoInd.Value = Convert.ToBoolean(dr["EC_FijadoInd"]);
                    else
                        dto.EC_FijadoInd.Value = false;
                    if (!string.IsNullOrWhiteSpace(dr["EC_Proposito"].ToString()))
                        dto.EC_Proposito.Value = Convert.ToByte(dr["EC_Proposito"]);
                    else
                        dto.EC_Proposito.Value = 0;
                    if (!string.IsNullOrWhiteSpace(dr["EC_PrimeraCtaPagada"].ToString()))
                        dto.EC_PrimeraCtaPagada.Value = Convert.ToInt32(dr["EC_PrimeraCtaPagada"]);
                    else
                        dto.EC_PrimeraCtaPagada.Value = 1;
                    if (!string.IsNullOrWhiteSpace(dr["EC_ValorPago"].ToString()))
                        dto.EC_ValorPago.Value = Convert.ToDecimal(dr["EC_ValorPago"]);
                    if (!string.IsNullOrWhiteSpace(dr["NumDocProceso"].ToString()))
                        dto.NumDocProceso.Value = Convert.ToInt32(dr["NumDocProceso"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosPendientesByCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="estado">Estado de la actividad</param>
        /// <param name="actFlujoID">Actividad de clujo para la consulta</param>
        /// <param name="actCerrada">Indicador si busca que la actividad este cerrada</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosByProposito(string clienteID, string actAprobacionGiro, int proposito, SectorCartera sector)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string aprobGiro = string.Empty;
                #region Valida la aprobacion de giro

                if (sector == SectorCartera.Solidario)
                {
                    mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);

                    mySqlCommand.Parameters["@ActividadFlujoID"].Value = actAprobacionGiro;
                    mySqlCommand.Parameters["@CerradoInd"].Value = true;

                    aprobGiro = "	INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cred.NumeroDoc AND act.actividadFlujoID=@ActividadFlujoID AND act.CerradoInd=@CerradoInd ";
                }

                #endregion

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@Proposito", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ClienteID"].Value = clienteID;
                mySqlCommand.Parameters["@Proposito"].Value = proposito; //Se filtra 2 y 11
                mySqlCommand.Parameters["@CanceladoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                #endregion
                #region CommandText

                mySqlCommand.CommandText =
                    "SELECT DISTINCT cred.*, his.EC_FijadoInd " +
                    "FROM ccCreditoDocu cred with(nolock) " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cred.NumeroDoc AND ctrl.Estado=@Estado " +
                    "   INNER JOIN ccEstadoCuentaHistoria his with(nolock) ON his.NumeroDoc = cred.DocEstadoCuenta AND his.EC_Proposito in(2,11) " + aprobGiro +
                    "WHERE cred.EmpresaID = @EmpresaID AND cred.ClienteID=@ClienteID and cred.CanceladoInd=@CanceladoInd ";

                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    dto.EC_FijadoInd.Value = Convert.ToBoolean(dr["EC_FijadoInd"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosByProposito");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los creditos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosForRenovacionPoliza(string cliente)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = 
                    "SELECT cred.*, cli.Descriptivo as Nombre, ctrl.Estado " +
                    "FROM ccCreditoDocu cred with(nolock) " +
                    "    INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                    "    INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                    "    INNER JOIN ccPolizaEstado pol with(nolock) ON pol.NumDocCredito = cred.NumeroDoc and pol.NumeroDocLiquida is null " +
                    "        and (pol.AnuladaIND IS NULL or pol.AnuladaIND = 0)" + 
                    "   INNER JOIN ccSegurosAsesor seg WITH(NOLOCK) on seg.SegurosAsesorID = pol.SegurosAsesorID and seg.EmpresaGrupoID = pol.eg_ccSegurosAsesor " + 
                    "       and seg.AsesorInternoInd = 1 " +
                    "WHERE cred.EmpresaID = @EmpresaID and cred.ClienteID = @ClienteID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                result = new List<DTO_ccCreditoDocu>();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.Estado.Value = Convert.ToByte(dr["Estado"]);

                    result.Add(dto);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetByCliente");
                throw exception;
            }
        }

        #endregion

        #region Generales con filtros extras

        /// <summary>
        /// Trae la info de un credito
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranza">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccCreditoDocu DAL_ccCreditoDocu_GetCreditoByLibranzaAndFechaCorte(int libranza, int numeroDoc, DateTime fechaCorte)
        {
            try
            {
                DTO_ccCreditoDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaCorte", SqlDbType.DateTime);

                mySqlCommand.Parameters["@Libranza"].Value = libranza;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@FechaCorte"].Value = fechaCorte;

                #region CommanText
                mySqlCommand.CommandText =
                    " SELECT DISTINCT capital, " +
                    " COUNT(CuotaID) over() as CuotasMora, cred.*, cli.Descriptivo as Nombre " +
                    " FROM ccCreditoDocu cred with(nolock)  " +
                        " INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente  	" +
                        " left JOIN " +
                        " ( " +
                            " select cp.NumeroDoc, " +
                            " case when (SUM(TotalValor - VlrPagado) is null) then TotalValor else  SUM(TotalValor - VlrPagado) end as capital " +
                            " from  ccCreditoComponentes cp with(nolock)  " +
                                " left join " +
                                " ( " +
                                    " select numerodoc, sum(VlrCapital) as VlrPagado from cccreditopagos pag with(nolock) " +
                                    " where numerodoc = @NumeroDoc group by numerodoc " +
                                " ) as pagos on cp.numerodoc = pagos.numerodoc " +
                            " where cp.NumeroDoc = @NumeroDoc and (cp.ComponenteCarteraID = '001' or cp.ComponenteCarteraID = '003') " +
                            " group by cp.numerodoc, TotalValor	" +
                        " ) AS capital on cred.NumeroDoc = capital.NumeroDoc	" +
                        " LEFT JOIN ccCreditoPlanPagos pag with (nolock) ON cred.NumeroDoc = pag.NumeroDoc and @FechaCorte > FechaCuota " +
                            " and VlrPagadoCuota < (pag.VlrCuota + pag.VlrPagadoExtras) " +
                    " WHERE EmpresaID = @EmpresaID and cred.Libranza = @Libranza ";

                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoDocu(dr);
                    result.Nombre.Value = dr["Nombre"].ToString();
                    result.VlrCapital.Value = Convert.ToDecimal(dr["capital"]);
                    result.CuotasMora.Value = Convert.ToInt16(dr["CuotasMora"]);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los creditos de un cliente cuando la fecha de la cuota es menor a la fecha de corte
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosByClienteAndFecha(string cliente, DateTime fechaCorte,SectorCartera sectorCart,bool onlyWithSaldo, bool useFechaCorte)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                #endregion
                #region Asignacion de valores a Parametros
                mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaCorte"].Value = fechaCorte;
                #endregion
                #region CommanText
                if (sectorCart == SectorCartera.Financiero)
                {
                    string where = string.Empty;
                    if(onlyWithSaldo)
                        where = " where pag2.SdoCapital+pag2.SdoCapSegu > 0  " ;
                    if (useFechaCorte)
                        where += string.IsNullOrEmpty(where)? " where pag2.FechaCuota <= @FechaCorte  " : " and pag2.FechaCuota <= @FechaCorte  ";

                    mySqlCommand.CommandText =
                        " SELECT DISTINCT pag3.capital, pag3.CuotasMora, cred.*, cli.Descriptivo as Nombre,ctrl.Estado  " +
                        " FROM ccCreditoDocu cred with(nolock)    " +
                        "      INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                        "      INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente    " +
                        "     LEFT JOIN   " +
                        "             (  " +
                        "              SELECT Numerodoc, sum(SdoCapital+SdoCapSegu) as capital, COUNT(*) as CuotasMora  " +
                        "                from  " +
                        "                     (   " +
                        "                      SELECT	pla.NumeroDoc, pla.cuotaid, pla.FechaCuota, " +
                        " 		                    pla.vlrCapital -		 (case when (pag1.AboCapital is null) then 0 else pag1.AboCapital end) as SdoCapital, " +
                        " 		                    Pla.VlrSeguro  -		 (case when (pag1.AboCapSegu is null) then 0 else pag1.AboCapSegu end) as SdoCapSegu " +
                        "                     FROM ccCreditoPlanPagos pla  with(nolock) " +
                        " 	                    left join ccCreditoDocu crd   with(nolock) on   pla.NumeroDoc = crd.NumeroDoc " +
                        " 		                    LEFT  join   " +
                        " 		                    (   " +
                        " 		                      select pag.numerodoc, pag.CreditoCuotaNum, " +
                        " 				                    sum(pag.VlrCapital) as AboCapital,   " +
                        " 				                    sum(pag.VlrSeguro)  as AboCapSegu   " +
                        " 		                      from cccreditopagos pag with(nolock)  " +
                        " 				                    left join ccCreditoDocu crd on   pag.NumeroDoc = crd.NumeroDoc " +
                        " 		                      where crd.ClienteID = @ClienteID " +
                        " 		                      group by pag.numerodoc, pag.CreditoCuotaNum  " +
                        " 		                    ) as pag1 on pla.Consecutivo = pag1.CreditoCuotaNum " +
                        "                    where crd.ClienteID = @ClienteID " +
                        "                     group by pla.numerodoc, pla.cuotaid, pla.FechaCuota, pla.vlrCapital, Pla.VlrSeguro, pag1.AboCapital, pag1.AboCapSegu " +
                        "                   ) as Pag2 " + where + 
                        "             group by pag2.Numerodoc " +
                        "         ) as Pag3 on cred.numerodoc = pag3.NumeroDoc " +
                        " where cred.EmpresaID = @EmpresaID and  cred.clienteid = @ClienteID and capital is not null ";
                }
                else //cooperativas
                {
                    string where = string.Empty;
                    if (useFechaCorte)
                        where = " and @FechaCorte > pag.FechaCuota ";

                     mySqlCommand.CommandText =
                        " SELECT DISTINCT cred.NumeroDoc, capital, COUNT(CuotaID) over() as CuotasMora, cred.*, cli.Descriptivo as Nombre,ctrl.Estado " +
                        "      FROM ccCreditoDocu cred with(nolock) " +
                        "      INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                        "      INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                                  " LEFT JOIN " +
                                 " ( " +
                                     " select ComponenteCarteraID, cp.NumeroDoc, " +
                                       "	case when (SUM(TotalValor - VlrPagado) is null) then TotalValor else  SUM(TotalValor - VlrPagado) end as capital " +
                                     " from  ccCreditoComponentes cp with(nolock) " +
                                      "    LEFT  join " +
                                       "   ( " +
                                            "  select numerodoc, sum(VlrCapital) as VlrPagado " +
                                             "  from cccreditopagos pag with(nolock) " +
                                              " group by numerodoc " +
                                        "  ) as pagos on cp.numerodoc = pagos.numerodoc " +
                                      " and cp.ComponenteCarteraID = '001' " +
                                      " group by cp.numerodoc,TotalValor,ComponenteCarteraID" +
                                 " ) AS capital on cred.NumeroDoc = capital.NumeroDoc	" +
                                 " LEFT JOIN ccCreditoPlanPagos pag with (nolock) ON cred.NumeroDoc = pag.NumeroDoc  " + where +
                                    "  and pag.VlrPagadoCuota < (pag.VlrCuota + pag.VlrPagadoExtras) " +
                               " WHERE EmpresaID = @EmpresaID and cred.ClienteID = @ClienteID and capital.ComponenteCarteraID = '001' ";
                }
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                result = new List<DTO_ccCreditoDocu>();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);

                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.VlrCapital.Value = Convert.ToDecimal(dr["capital"]);
                    dto.CuotasMora.Value = Convert.ToInt16(dr["CuotasMora"]);
                    dto.Estado.Value = Convert.ToByte(dr["Estado"]);
                    result.Add(dto);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetByCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los creditos de un cliente en un estado específico
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosByClienteAndEstado(string cliente, byte estado)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Parametros

                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);

                #endregion
                #region Asignacion de valores a Parametros

                mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@EstadoDeuda"].Value = estado;

                #endregion
                #region CommanText

                mySqlCommand.CommandText =
                       " SELECT * " +
                       " FROM ccCreditoDocu cred with(nolock) " +
                        " WHERE EmpresaID = @EmpresaID and cred.ClienteID = @ClienteID and EstadoDeuda = @EstadoDeuda ";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                result = new List<DTO_ccCreditoDocu>();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu(dr);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetCreditosByClienteAndEstado");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los creditos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        public List<DTO_CodeudorDet> DAL_ccCreditoDocu_GetInfoCodeudor(List<string> clientes, int? numDocCred)
        {
            try
            {
                List<DTO_CodeudorDet> result = new List<DTO_CodeudorDet>(); ;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;


                mySqlCommand.CommandText = " SELECT  ter.TerceroID as ClienteID, (Case when cl.Descriptivo is null Then ter.Descriptivo else cl.Descriptivo End) as ClienteDesc, " +
		                                   "    (Case when cl.Telefono is null Then ter.Tel1 else cl.Telefono End) as Telefono,  " +
		                                   "    (Case when cl.Celular is null Then ter.Tel2 else cl.Celular End) as Celular,  " +
		                                   "    (Case when cl.Correo is null Then ter.CECorporativo else cl.Correo End) as Correo,  " +
		                                   "    (Case when cl.ResidenciaDir is null Then ter.Direccion else cl.ResidenciaDir End) as ResidenciaDir,  " +
		                                   "    pr.Descriptivo as ProfesionDes " +
                                           " FROM  coTercero ter with(nolock)  " +
                                           "    LEFT JOIN ccCliente cl with(nolock) on cl.TerceroID = ter.TerceroID and cl.eg_coTercero = ter.EmpresaGrupoID  " +
                                           "    LEFT JOIN ccProfesion pr with(nolock) on cl.ProfesionID = pr.ProfesionID and cl.eg_ccProfesion = pr.EmpresaGrupoID " +
                                           " WHERE ter.TerceroID = @ClienteID and ter.EmpresaGrupoID = @EmpresaID"; 

                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                foreach (string cliente in clientes)
                {
                    mySqlCommand.Parameters["@ClienteID"].Value = cliente;

                    SqlDataReader dr = mySqlCommand.ExecuteReader();
                    if (dr.Read())
                    {
                        DTO_CodeudorDet dto = new DTO_CodeudorDet(dr);
                        dto.NumDocCredito.Value = numDocCred;
                        result.Add(dto);
                    }
                    dr.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetInfoCodeudor");
                throw exception;
            }
        }


        #endregion

        #region Para ventas de cartera

        /// <summary>
        /// Trae la info de un credito
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranza">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccCreditoDocu DAL_ccCreditoDocu_GetByOferta(string oferta)
        {
            try
            {
                DTO_ccCreditoDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@Oferta"].Value = oferta;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.CommandText = "SELECT cred.*, cli.Descriptivo as Nombre " +
                                           "FROM ccCreditoDocu cred with(nolock) " +
                                           "INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                                           "WHERE EmpresaID = @EmpresaID and Oferta = @Oferta ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccCreditoDocu(dr);
                    result.Nombre.Value = dr["Nombre"].ToString();
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetByOferta");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la info de un credito
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranza">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetByCompradorCartera(string compradorCarteraID)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.CommandText = "SELECT cred.*, cli.Descriptivo as Nombre, ctrl.Estado " +
                                           "FROM ccCreditoDocu cred with(nolock) " +
                                           "INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                                           "INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                                           "WHERE cred.EmpresaID = @EmpresaID and cred.CompradorCarteraID = @CompradorCarteraID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                result = new List<DTO_ccCreditoDocu>();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.Estado.Value = Convert.ToByte(dr["Estado"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetByCompradoCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="estado">Estado de la actividad</param>
        /// <param name="actFlujoID">Actividad de clujo para la consulta</param>
        /// <param name="actCerrada">Indicador si busca que la actividad este cerrada</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccSolicitudCompraCartera> DAL_ccCreditoDocu_GetCreditosWithRecompra(string clienteID, string userName, string actAprobacionGiro, bool allEmpresas)
        {
            try
            {
                List<DTO_ccSolicitudCompraCartera> result = new List<DTO_ccSolicitudCompraCartera>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                //mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                //mySqlCommand.Parameters.Add("@Proposito", SqlDbType.Int);
                //mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EstadoCompraCartera1", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EstadoCompraCartera2", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Fijado", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                //mySqlCommand.Parameters["@ActividadFlujoID"].Value = actAprobacionGiro;
                mySqlCommand.Parameters["@ClienteID"].Value = clienteID;
                //mySqlCommand.Parameters["@Proposito"].Value = (int)PropositoEstadoCuenta.RecogeSaldo;
                //mySqlCommand.Parameters["@CerradoInd"].Value = true;
                mySqlCommand.Parameters["@CanceladoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EstadoCompraCartera1"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@EstadoCompraCartera2"].Value = (int)EstadoDocControl.SinAprobar;
                mySqlCommand.Parameters["@Fijado"].Value = true;

                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT cred.* , his.EC_ValorPago,his.EC_Proposito,his.EC_ValorAbono  " +
                    "FROM ccCreditoDocu cred with(nolock)  " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cred.NumeroDoc  " +
                    "       AND ctrl.Estado=@Estado " +                         
                    "   INNER JOIN ccEstadoCuentaHistoria his with(nolock) on his.NumDocCredito = cred.NumeroDoc " +
                    "       AND his.EC_Proposito in (3,4) and his.EC_FijadoInd=@Fijado " +
                    "WHERE cred.ClienteID=@ClienteID and cred.CanceladoInd=@CanceladoInd and cred.EmpresaID=@EmpresaID " +
                    "	AND cred.NumeroDoc not in " +
                    "   ( " +
                    "		SELECT solCompra.DocCompra FROM ccSolicitudCompraCartera solCompra with(nolock) " +
                    "			INNER JOIN glDocumentoControl ctrlCompra with(nolock) ON solCompra.NumeroDoc = ctrlCompra.NumeroDoc " +
                    "				AND ctrl.Estado in (@EstadoCompraCartera1,@EstadoCompraCartera2) " +
                    "	) " +
                    "ORDER BY CAST(cred.Libranza AS INTEGER) ";
                #endregion

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudCompraCartera dto = new DTO_ccSolicitudCompraCartera();
                    dto.Documento.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.DocCompra.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    dto.VlrSaldo.Value = Convert.ToDecimal(dr["EC_ValorPago"]);
                    dto.EC_Proposito.Value = Convert.ToByte(dr["EC_Proposito"]);
                    if (!string.IsNullOrWhiteSpace(dr["EC_ValorAbono"].ToString())) 
                        dto.EC_ValorAbono.Value = Convert.ToDecimal(dr["EC_ValorAbono"]);
                    dto.ExternaInd.Value = false;

                    //Revisa si es extarena la compra
                    if (dr["EmpresaID"].ToString().Trim() == this.Empresa.ID.Value)
                    {
                        dto.AnticipoInd.Value = true;
                        dto.IndRecibePazySalvo.Value = true;
                        dto.FechaPazySalvo.Value = DateTime.Now.Date;
                        dto.UsuarioID.Value = userName;
                    }
                    else
                    {
                        dto.AnticipoInd.Value = false;
                        dto.IndRecibePazySalvo.Value = false;
                    }

                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosWithRecompra");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoDocu segun la pagaduria
        /// </summary>
        /// <param name="pagaduria">Identificador del cliente</param>
        /// <param name="estado">Estado de la actividad</param>
        /// <param name="actFlujoID">Actividad de clujo para la consulta</param>
        /// <param name="actCerrada">Indicador si busca que la actividad este cerrada</param>
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosForIncorporacion(DateTime periodo, string centroPago, DateTime fechaIncorpora, string actFlujo, bool getPendientesIncor)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@FechasIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechasFin", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaIncorpora", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujo;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechasIni"].Value = periodo;
                mySqlCommand.Parameters["@FechasFin"].Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                mySqlCommand.Parameters["@CerradoInd"].Value = true;
                mySqlCommand.Parameters["@FechaIncorpora"].Value = fechaIncorpora;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);

                #endregion
                #region CommandText

                // Query CP
                string queryCP = string.Empty;
                string queryCPMod = string.Empty;
                if (!string.IsNullOrWhiteSpace(centroPago))
                {
                    mySqlCommand.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                    mySqlCommand.Parameters["@CentroPagoID"].Value = centroPago;

                    queryCP = " and cre.CentroPagoID = @CentroPagoID ";
                    queryCPMod = " AND (reInc.CentroPagoID = @CentroPagoID or reInc.CentroPagoModificaID = @CentroPagoID) ";
                }
                // Filtro para traer los pendientes por incorporar
                string queryDiaCorte = string.Empty;
                string queryFechaFilter = string.Empty;
                if (!getPendientesIncor)
                {
                    queryDiaCorte = "  Where q.PagaduriaID is null and  pag.DiaCorte <= DAY(@FechaIncorpora) ";
                    queryFechaFilter = " and MONTH(ctrl.FechaDoc) = MONTH(@FechaIncorpora)  and YEAR(ctrl.FechaDoc) = YEAR(@FechaIncorpora) ";
                }
                mySqlCommand.CommandText =
                    "SELECT * FROM" +
                    "(" +
                    "   SELECT cre.* , cli.Descriptivo as Nombre, 0 as NumeroReINC, '' AS NovedadReInc, 4 AS TipoNovedad, cre.Plazo AS PlazoINC, " +
                    "      cre.VlrCuota AS ValorCuota, cre.Observacion AS Obs, cp.Descriptivo AS PagaduriaDesc, 1 AS OrigenDato, 0 as ConsReinc" +
                    "   FROM ccCreditoDocu cre WITH(NOLOCK)" +
                    "       INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cre.NumeroDoc AND ctrl.Estado=@Estado " +
                    "       INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "       INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "   	INNER JOIN glactividadEstado act with(nolock) on act.NumeroDoc = cre.NumeroDoc " +
                    "   	    AND act.actividadFlujoID=@ActividadFlujoID AND act.CerradoInd=@CerradoInd " +
                    "   WHERE cre.EmpresaID = @EmpresaID " + queryCP +
                    "       AND (cre.NumIncorporaDoc = 0 or cre.NumIncorporaDoc IS NULL) AND cre.IncorporacionTipo = 1 " +
                    "   UNION " +
                    "   SELECT cre.*, cli.Descriptivo as Nombre, " +
                    "   	CASE WHEN cp.DigitoReincorporaInd = 1 THEN reInc.NumeroINC ELSE 0 END AS NumeroReINC, " +
                    "   	reInc.NovedadIncorporaID AS NovedadReInc, reInc.TipoNovedad, ISNULL(reInc.PlazoINC, cre.plazo) AS PlazoINC, reInc.ValorCuota, nov.Descriptivo AS Obs, " +
                    "       cp.Descriptivo AS PagaduriaDesc, 3 AS OrigenDato,reInc.Consecutivo as ConsReinc " +
                    "   FROM ccReincorporacionDeta reInc WITH(NOLOCK) " +
                    "   	INNER JOIN ccCreditoDocu cre WITH(NOLOCK) ON reInc.NumDocCredito = cre.NumeroDoc " +
                    "       INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = reInc.NumeroDoc AND ctrl.Estado=@Estado " +
                    "   	INNER JOIN ccCliente cli WITH(NOLOCK) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "       INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "       INNER JOIN ccIncorporacionNovedad nov WITH(NOLOCK) on reInc.NovedadIncorporaID = nov.NovedadIncorporaID AND nov.EmpresaGrupoID = reInc.eg_ccIncorporacionNovedad " +
                    "   WHERE cre.EmpresaID = @EmpresaID  AND (reInc.ConsIncorpora = 0 or reInc.ConsIncorpora is null) " + queryCPMod +
                    "       AND  reInc.TipoNovedad <> 6 " +
                    ") AS q " +
                    "WHERE q.PagaduriaID IN (" + 
                    " Select distinct validPag.PagaduriaID FROM " +
                    " ( " +
                     "     Select distinct pag.PagaduriaID from ccPagaduria pag with(nolock)  " + 
			        "        left join  " + 
					"        ( select PagaduriaID, inc.eg_ccPagaduria	 " + 
				    "		        from ccIncorporacionDeta inc with(nolock) 	 " +
                    "	        inner join glDocumentoControl ctrl with(nolock) on inc.NumeroDoc = ctrl.NumeroDoc and ctrl.estado = @Estado  " +
                    "	        where EmpresaID = @EmpresaID " + queryFechaFilter + 
					"	        group by PagaduriaID, inc.eg_ccPagaduria " + 
					"        ) as q on q.PagaduriaID = pag.PagaduriaID and pag.EmpresaGrupoID = q.eg_ccPagaduria " + 
			        "       " + queryDiaCorte +
                    "	 ) as validPag " +
                    " )";

                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Editable.Value = false;
                    
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.NumeroINC.Value = Convert.ToByte(dr["NumeroReINC"]);
                    dto.NumReincorpora.Value = Convert.ToByte(dr["NumeroReINC"]);
                    
                    dto.Plazo.Value = Convert.ToInt16(dr["PlazoINC"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["ValorCuota"]);

                    dto.NovedadIncorporaID.Value = dr["NovedadReInc"].ToString();
                    dto.Otro.Value = dr["TipoNovedad"].ToString();
                    dto.Otro1.Value = dr["PagaduriaDesc"].ToString();
                    dto.Otro2.Value = dr["OrigenDato"].ToString();
                    dto.Observacion.Value = dr["Obs"].ToString();
                    dto.ConsReinc.Value = Convert.ToInt32(dr["ConsReinc"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetCreditosForIncorporacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoDocu segun la pagaduria
        /// </summary>
        /// <param name="pagaduria">Identificador del cliente</param>
        /// <param name="estado">Estado de la actividad</param>
        /// <param name="actFlujoID">Actividad de clujo para la consulta</param>
        /// <param name="actCerrada">Indicador si busca que la actividad este cerrada</param>
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosForIncorporacionVerificacion(DateTime periodo, string centroPago, string actFlujo)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@FechasIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechasFin", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujo;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechasIni"].Value = periodo;
                mySqlCommand.Parameters["@FechasFin"].Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                #endregion
                #region CommandText

                // Query CP
                string queryCP = string.Empty;
                string queryCPMod = string.Empty;
                if (!string.IsNullOrWhiteSpace(centroPago))
                {
                    mySqlCommand.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                    mySqlCommand.Parameters["@CentroPagoID"].Value = centroPago;

                    queryCP = " and cre.CentroPagoID = @CentroPagoID ";
                    queryCPMod = " AND (reInc.CentroPagoID = @CentroPagoID or reInc.CentroPagoModificaID = @CentroPagoID) ";
                }

                mySqlCommand.CommandText =
                    "SELECT cre.* , cli.Descriptivo as Nombre, 0 as NumeroReINC, '' AS NovedadIncorporaID, 4 AS TipoNovedad, cre.Plazo AS PlazoINC, " +
                        "cre.VlrCuota AS ValorCuota, cre.Observacion AS Obs, centPag.Descriptivo AS PagaduriaDesc " +
                    "FROM ccCreditoDocu cre WITH(NOLOCK)" +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cre.NumeroDoc AND ctrl.Estado=@Estado " +
                    "   INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "   INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " + 
                    "	INNER JOIN glactividadEstado act with(nolock) on act.NumeroDoc = cre.NumeroDoc " +
                    "	    AND act.actividadFlujoID=@ActividadFlujoID AND act.CerradoInd=@CerradoInd " +
                    "WHERE cre.EmpresaID = @EmpresaID  " + queryCP +
                    "   AND cre.NumIncorporaDoc != 0 AND cre.NumDocVerificado IS NULL AND cre.IncorporacionTipo = 1 " +
                    "UNION " +
                    "SELECT cre.*, cli.Descriptivo as Nombre, " +
                    "	CASE WHEN cp.DigitoReincorporaInd = 1 THEN reInc.NumeroINC ELSE 0 END AS NumeroReINC, " +
                    "	reInc.NovedadIncorporaID, reInc.TipoNovedad, ISNULL(reInc.PlazoINC, cre.plazo) AS PlazoINC, reInc.ValorCuota, nov.Descriptivo AS Obs, " +
                    "   centPag.Descriptivo AS PagaduriaDesc " +
                    "FROM ccReincorporacionDeta reInc WITH(NOLOCK) " +
                    "	INNER JOIN ccCreditoDocu cre WITH(NOLOCK) ON reInc.NumDocCredito = cre.NumeroDoc " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = reInc.NumeroDoc AND ctrl.Estado=@Estado " +
                    "	INNER JOIN ccCliente cli WITH(NOLOCK) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "   INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "   INNER JOIN ccIncorporacionNovedad nov WITH(NOLOCK) on reInc.NovedadIncorporaID = nov.NovedadIncorporaID AND nov.EmpresaGrupoID = reInc.eg_ccIncorporacionNovedad " +
                    "WHERE cre.EmpresaID = @EmpresaID " + queryCPMod + 
                    " AND reInc.PeriodoNomina BETWEEN @FechasIni AND @FechasFin AND reInc.TipoNovedad <> 6";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Editable.Value = false;

                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.NumeroINC.Value = Convert.ToByte(dr["NumeroReINC"]);
                    dto.NumReincorpora.Value = Convert.ToByte(dr["NumeroReINC"]);

                    dto.Plazo.Value = Convert.ToInt16(dr["PlazoINC"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["ValorCuota"]);

                    dto.NovedadIncorporaID.Value = dr["NovedadIncorporaID"].ToString();
                    dto.Otro.Value = dr["TipoNovedad"].ToString();
                    dto.Otro1.Value = dr["PagaduriaDesc"].ToString();
                    dto.Observacion.Value = dr["Obs"].ToString();
                    
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetCreditosForIncorporacionVerificacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoDocu segun el centro de pago
        /// </summary>
        /// <param name="centroPago">Identificador de centro de pago</param>
        /// <param name="estado">Estado de la actividad</param>
        /// <param name="actFlujoID">Actividad de clujo para la consulta</param>
        /// <param name="actCerrada">Indicador si busca que la actividad este cerrada</param>
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosForDesIncorporacion(DateTime periodo, string actFlujoEntregaLibranza)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechasIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechasFin", SqlDbType.DateTime);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoEntregaLibranza;
                mySqlCommand.Parameters["@CerradoInd"].Value = true;
                mySqlCommand.Parameters["@CanceladoInd"].Value = true;
                mySqlCommand.Parameters["@FechasIni"].Value = periodo;
                mySqlCommand.Parameters["@FechasFin"].Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT DISTINCT * FROM " +
                    "( " +
                    "   SELECT cre.* , cli.Descriptivo AS Nombre, cp.Descriptivo AS cpDesc, " +
                    "       cre.NovedadIncorporaID AS NovedadInc, 6 as TipoNovedad, cre.plazo AS PlazoINC, cre.VlrCuota AS ValorCuota, 1 AS OrigenDato " +
                    "   FROM ccCreditoDocu cre WITH(NOLOCK) " +
                    "   	INNER JOIN ccCliente cli WITH(NOLOCK) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "   	INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "       LEFT JOIN " +
                    "       ( " +
                    "       	select distinct (pag.NumeroDoc) " +
                    "	        from ccCreditoPagos pag with(nolock) " +
                    "		        INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = pag.PagoDocu " +
                    "	        where ctrl.PeriodoDoc = @FechasIni " +
                    "       ) as q on cre.NumeroDoc = q.NumeroDoc " +
                    "   WHERE cre.EmpresaID = @EmpresaID and cre.CanceladoInd = @CanceladoInd " +
                    "   	AND cre.NumIncorporaDoc IS NOT NULL AND  cre.NumIncorporaDoc <> 0 AND (cre.NumDesIncorporaDoc is null or cre.NumDesIncorporaDoc = 0) " +
                    "   UNION " +
                    "   SELECT cre.*, cli.Descriptivo as Nombre, cp.Descriptivo AS cpDesc, " +
                    "       cre.NovedadIncorporaID AS NovedadInc, 6 as TipoNovedad, cre.plazo AS PlazoINC, cre.VlrCuota AS ValorCuota, 1 AS OrigenDato " +
                    "   FROM ccNominaDeta nom with(nolock) " +
                    "   	INNER JOIN ccCreditoDocu cre WITH(NOLOCK) ON nom.NumDocCredito = cre.NumeroDoc " +
                    "   	INNER JOIN ccCliente cli WITH(NOLOCK) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "   	INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "   WHERE cre.EmpresaID = @EmpresaID AND nom.FechaNomina between @FechasIni and @FechasFin and nom.EstadoCruce in (4, 9) " +
                    "   	AND cre.NumIncorporaDoc IS NOT NULL AND cre.NumIncorporaDoc <> 0 AND (cre.NumDesIncorporaDoc is null or cre.NumDesIncorporaDoc = 0) " +
                    "   UNION " +
                    "   SELECT cre.*, cli.Descriptivo as Nombre, cp.Descriptivo AS cpDesc, " +
                    "       reInc.NovedadIncorporaID AS NovedadInc, reInc.TipoNovedad, ISNULL(reInc.PlazoINC, cre.plazo) AS PlazoINC, reInc.ValorCuota, 3 AS OrigenDato " + 
                    "   FROM ccReincorporacionDeta reInc WITH(NOLOCK) " +
                    "   	INNER JOIN ccCreditoDocu cre WITH(NOLOCK) ON reInc.NumDocCredito = cre.NumeroDoc " +
                    "   	INNER JOIN ccCliente cli WITH(NOLOCK) on cli.ClienteID = cre.ClienteID AND cli.EmpresaGrupoID = cre.eg_ccCliente " +
                    "   	INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on cre.CentroPagoID = cp.CentroPagoID AND cre.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "   WHERE cre.EmpresaID = @EmpresaID AND reInc.PeriodoNomina between @FechasIni and @FechasFin AND reInc.TipoNovedad IN (5, 6) " +
                    "   	 AND cre.NumIncorporaDoc IS NOT NULL AND  cre.NumIncorporaDoc <> 0 AND (cre.NumDesIncorporaDoc is null or cre.NumDesIncorporaDoc = 0) " +
                    ") AS q INNER JOIN glactividadEstado act with(nolock) on act.NumeroDoc = q.NumeroDoc  AND act.actividadFlujoID=@ActividadFlujoID AND act.CerradoInd=@CerradoInd";
                
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu(dr);
                    dto.Editable.Value = false;
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.Otro.Value = dr["cpDesc"].ToString();
                    dto.NovedadIncorporaID.Value = dr["NovedadInc"].ToString();
                    dto.Otro1.Value = dr["TipoNovedad"].ToString();
                    dto.Otro2.Value = dr["OrigenDato"].ToString();
                    dto.Plazo.Value = Convert.ToInt16(dr["PlazoINC"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["ValorCuota"]);

                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetCreditosForDesIncorporacion");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de ccCreditoDocu segun la pagaduria
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <param name="isCooperativa">Indicador para establecer si se esta trabajando con cartera financiera o cartera cooperativa</param>
        /// <param name="actFlujoAprobGiro">Activiad de flujo de la aporbacion del giro SOLO APLICA PARA FINANCIERA</param>
        /// <param name="usuarioID">Usuario que ingreso al sistema</param>
        /// <returns>retorna una lista de ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosForPreventa(string actFlujoID, string actAprobGiro, string compradorPropio, string usuarioID, 
            DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@BloqueaVentaLinCred", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@BloqueaVentaPag", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoAprobGiro", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                //mySqlCommand.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CompradorCarteraPropio", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@BloqueaVentaLinCred"].Value = false;
                mySqlCommand.Parameters["@BloqueaVentaPag"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@ActividadFlujoAprobGiro"].Value = actAprobGiro;
                mySqlCommand.Parameters["@CerradoInd"].Value = true;
                mySqlCommand.Parameters["@CanceladoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                //mySqlCommand.Parameters["@EstadoDeuda"].Value = (int)EstadoDeuda.Normal;
                mySqlCommand.Parameters["@CompradorCarteraPropio"].Value = compradorPropio;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommand.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommand.Parameters["@FechaFin"].Value = fechaFin;
                #endregion
                #region Query
                mySqlCommand.CommandText =
                        "SELECT cred.NumeroDoc, cred.Libranza, cred.ClienteID, cli.Descriptivo as Nombre, cred.LineaCreditoID, cred.VlrCuota, cred.VlrLibranza, cred.VlrPrestamo " +
                        "FROM ccCreditoDocu cred with(nolock) " +
                        "	INNER JOIN ccLineaCredito lcd with(nolock) on lcd.LineaCreditoId = cred.LineaCreditoID AND lcd.EmpresaGrupoID = cred.eg_ccLineaCredito " +
                        "		AND lcd.BloqueaVentaInd = @BloqueaVentaLinCred " +
                        "	INNER JOIN ccPagaduria pag with(nolock) on pag.PagaduriaId = cred.PagaduriaID AND pag.EmpresaGrupoID = cred.eg_ccPagaduria" +
                        "		AND pag.BloqueaVentaInd = @BloqueaVentaPag  " +
                        "    INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = cred.ClienteID AND cli.EmpresaGrupoID = cred.eg_ccCliente " +
                        "    INNER JOIN glDocumentoControl ctrl with(nolock) on cred.NumeroDoc = ctrl.NumeroDoc AND ctrl.Estado = @Estado " +
                        "    INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cred.NumeroDoc AND act.actividadFlujoID = @ActividadFlujoAprobGiro AND act.CerradoInd = @CerradoInd " +
                        "    INNER JOIN glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                        "        AND perm.areaFuncionalID = ctrl.areaFuncionalID AND perm.actividadFlujoID = @ActividadFlujoID  AND perm.UsuarioID = @UsuarioID " +
                        "WHERE cred.EmpresaID = @EmpresaID AND cred.CanceladoInd = @CanceladoInd AND cred.SustituidoInd IS NULL AND EstadoDeuda in(1,2) " +
                        "   AND (cred.DocVenta IS NULL or cred.DocVenta = 0) AND (CompradorCarteraID IS NULL OR CompradorCarteraID = @CompradorCarteraPropio) " + 
                        "   AND (ctrl.FechaDoc between @FechaIni and @FechaFin) " + 
                        "ORDER BY CAST(cred.Libranza AS INTEGER) ";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu();
                    dto.IsPreventa.Value = false;
                    dto.VendidaInd.Value = false;
                    dto.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    dto.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.ClienteID.Value = dr["ClienteID"].ToString();
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                    dto.VlrPrestamo.Value = 0;
                    dto.VlrCuota.Value = 0;
                    dto.VlrLibranza.Value = 0;
                    dto.VlrVenta.Value = 0;
                    dto.VlrUtilidad.Value = 0;
                    dto.NumCuotas.Value = 0;

                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosForPreventa");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de ccCreditoDocu segun la pagaduria
        /// </summary>
        /// <param name="actAprobacionGiro">Actividad para el documento de aprobacion de giro</param>    
        /// <returns>retorna una lista de ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosInPreventa(string actFlujoPreventa, string oferta, string usuarioID,
            DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@EstadoCredito", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EstadoVenta", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoPreventa;
                mySqlCommand.Parameters["@CerradoInd"].Value = true;
                mySqlCommand.Parameters["@Oferta"].Value = oferta;
                mySqlCommand.Parameters["@EstadoCredito"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EstadoVenta"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommand.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommand.Parameters["@FechaFin"].Value = fechaFin;
                #endregion
                #region Query

                mySqlCommand.CommandText =
                        "SELECT cred.NumeroDoc, cred.Libranza, cred.ClienteID, cli.Descriptivo as Nombre, cred.LineaCreditoID, cred.DocVenta,cred.VlrPrestamo, venDeta.Portafolio, " +
                        "   venDeta.VlrCuota, venDeta.VlrVenta, venDeta.VlrLibranza, venDeta.VlrNeto, venDeta.CuotasVend, venDeta.CuotaID, cred.CompradorCarteraID " +
                        "FROM ccCreditoDocu cred with(nolock) " +
                        "   INNER JOIN glDocumentoControl ctrlCre with(nolock) on cred.NumeroDoc = ctrlCre.NumeroDoc AND ctrlCre.Estado = @EstadoCredito " + 
                        "   INNER JOIN ccVentaDocu venDocu with(nolock) on venDocu.NumeroDoc = cred.DocVenta AND venDocu.Oferta = @Oferta " +
                        "   INNER JOIN ccVentaDeta venDeta with(nolock) on venDeta.NumeroDoc = venDocu.NumeroDoc AND venDeta.NumDocCredito = cred.NumeroDoc " +
                        "   INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = cred.ClienteID AND cli.EmpresaGrupoID = cred.eg_ccCliente " +
                        "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = venDocu.NumeroDoc AND ctrl.Estado = @EstadoVenta " +
                        "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cred.NumeroDoc AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd " +
                        "   INNER JOIN glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                        "       AND perm.areaFuncionalID = ctrl.areaFuncionalID  AND perm.actividadFlujoID = @ActividadFlujoID " +
                        "       AND perm.UsuarioID = @UsuarioID " +
                        "WHERE cred.EmpresaID = @EmpresaID " + // AND ctrlCre.FechaDoc between @FechaIni and @FechaFin " +
                        "ORDER BY CAST(cred.Libranza AS INTEGER) ";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu();
                    dto.IsPreventa.Value = true;
                    dto.VendidaInd.Value = true;

                    dto.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    dto.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.ClienteID.Value = dr["ClienteID"].ToString();
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                    dto.PortafolioID.Value = dr["Portafolio"].ToString();
                    dto.DocVenta.Value = Convert.ToInt32(dr["DocVenta"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    dto.VlrVenta.Value = Convert.ToDecimal(dr["VlrVenta"]);
                    dto.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);                 
                    dto.VlrUtilidad.Value = Convert.ToDecimal(dr["VlrNeto"]);
                    dto.VlrPrestamo.Value = 0;
                    dto.PrimeraCuota.Value = Convert.ToInt32(dr["CuotaID"]);
                    dto.NumCuotas.Value = Convert.ToInt32(dr["CuotasVend"]);
                    dto.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();

                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosInPreventa");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de ccCreditoDocu
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <param name="compradorCarteraID">Comprador de cartea asigando al credito</param>
        /// <param name="oferta">Oferta de la compra de cartera</param>
        /// <returns>retorna una lista de ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosVenta(string actFlujoID, string compradorCarteraID, string oferta, string usuarioID)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@VendidaInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@VendidaInd"].Value = false;
                mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                mySqlCommand.Parameters["@Oferta"].Value = oferta;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                #endregion
                #region Query
                mySqlCommand.CommandText =
                    "SELECT cred.NumeroDoc, cred.Libranza, cred.ClienteID, cli.Descriptivo as Nombre, cred.DocVenta, venDeta.VlrCuota, venDeta.VlrVenta, " +
                    "   venDeta.VlrLibranza, venDeta.VlrNeto, venDeta.CuotasVend, venDeta.CuotaID " + 
                    "FROM ccCreditoDocu cred with(nolock) " +
                    "   INNER JOIN ccVentaDocu venDocu with(nolock) on venDocu.NumeroDoc = cred.DocVenta AND venDocu.Oferta = @Oferta " +
                    "       AND venDocu.CompradorCarteraID = @CompradorCarteraID " +
                    "   INNER JOIN ccVentaDeta venDeta with(nolock) on venDeta.NumeroDoc = venDocu.NumeroDoc AND venDeta.NumDocCredito = cred.NumeroDoc " +
                    "   INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = cred.ClienteID AND cli.EmpresaGrupoID = cred.eg_ccCliente " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on cred.NumeroDoc = ctrl.NumeroDoc " +
                    "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cred.NumeroDoc " +
                    "       AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd " +
                    "   INNER JOIN glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "       AND perm.areaFuncionalID = ctrl.areaFuncionalID  AND perm.actividadFlujoID = @ActividadFlujoID  AND perm.UsuarioID = @UsuarioID " +
                    "WHERE ctrl.EmpresaID = @EmpresaID AND cred.VendidaInd = @VendidaInd AND cred.SustituidoInd IS NULL " +
                    "ORDER BY CAST(cred.Libranza AS INTEGER) ";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu();
                    dto.Aprobado.Value = true;
                    dto.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    dto.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.ClienteID.Value = dr["ClienteID"].ToString();
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.DocVenta.Value = Convert.ToInt32(dr["DocVenta"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    dto.VlrVenta.Value = Convert.ToDecimal(dr["VlrVenta"]);
                    dto.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                    dto.VlrUtilidad.Value = Convert.ToDecimal(dr["VlrNeto"]);
                    dto.PrimeraCuota.Value = Convert.ToInt32(dr["CuotaID"]); 
                    dto.NumCuotas.Value = Convert.ToInt32(dr["CuotasVend"]);

                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosVenta");
                throw exception;
            }
        }

        #endregion

        #region Otras (Particulares)

        /// <summary>
        /// Trae los creditos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetForReversionMigracionNom(int numeroDoc)
        {
            try
            {
                List<DTO_ccCreditoDocu> results = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.LiquidacionCredito;

                mySqlCommand.CommandText =
                    "select distinct cr.* " +
                    "from coauxiliar aux with(nolock) " +
                    "	inner join gldocumentocontrol ctrl with(nolock) on aux.IdentificadorTR = ctrl.NumeroDoc and ctrl.documentoID = @DocumentoID " +
                    "	inner join ccCreditoDocu cr on ctrl.NumeroDoc = cr.NumeroDoc " +
                    "where aux.EmpresaID = @EmpresaID and aux.NumeroDoc = @NumeroDoc and aux.IdentificadorTR <> @NumeroDoc order by NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                results = new List<DTO_ccCreditoDocu>();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu(dr);
                    results.Add(dto);
                }

                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetForReversionMigracionNom");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion  de cartera en cobro juridico de contabilidad
        /// </summary>
        /// <returns>retorna una lista </returns>
        public List<DTO_CobroJuridicoAuxiliar> DAL_ccCreditoDocu_GetAuxiliarCobroJur()
        {
            try
            {
                List<DTO_CobroJuridicoAuxiliar> results = new List<DTO_CobroJuridicoAuxiliar>();
                SqlCommand mySqlCommand = new SqlCommand("Cartera_GetMvtosAuxiliar", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandType = CommandType.StoredProcedure;


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
              
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_CobroJuridicoAuxiliar dto = new DTO_CobroJuridicoAuxiliar(dr);
                    results.Add(dto);
                }

                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetAuxiliarCobroJur");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el total de movimientos de un crédito
        /// </summary>
        public int DAL_ccCreditoDocu_GetTotalMovimientos(int libranza, List<int> mvtos)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Libranza"].Value = libranza;

                int i = 0;
                string whereMvtos = string.Empty;
                foreach (int mvto in mvtos)
                {
                    ++i;
                    whereMvtos += mvto.ToString();
                    if (i < mvtos.Count)
                        whereMvtos += ",";
                }
                if (!string.IsNullOrWhiteSpace(whereMvtos))
                {
                    whereMvtos = "and DocumentoID in(" + whereMvtos + ")";
                }

                mySqlCommand.CommandText =
                    "select count(*) " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join ccCreditoDocu cred with(nolock)  on ctrl.NumeroDoc = cred.NumeroDoc and Libranza = @Libranza " +
                    "where ctrl.EmpresaID = @EmpresaID and estado not in (-1, 0, 4, 7) and DocumentoID <> 161 " + whereMvtos;

                object obj = mySqlCommand.ExecuteScalar();
                return !string.IsNullOrWhiteSpace(obj.ToString()) ? Convert.ToInt32(obj) : 0;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetTotalMovimientosByDate");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el total de movimientos de un crédito despues de una fecha determinada
        /// </summary>
        public int DAL_ccCreditoDocu_GetTotalMovimientosPosteriores(int libranza, DateTime fechaDoc)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaDoc"].Value = fechaDoc;
                mySqlCommand.Parameters["@Libranza"].Value = libranza;

                mySqlCommand.CommandText =
                    "select count(*) " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join ccCreditoDocu cred with(nolock)  on ctrl.NumeroDoc = cred.NumeroDoc and Libranza = @Libranza " +
                    "where ctrl.EmpresaID = @EmpresaID and FechaDoc > @FechaDoc and estado not in (-1, 0, 4, 7) and DocumentoID <> 161 ";

                object obj = mySqlCommand.ExecuteScalar();
                return !string.IsNullOrWhiteSpace(obj.ToString()) ? Convert.ToInt32(obj) : 0; 
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetTotalMovimientosByDate");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de todos los creditos aprobados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="allEmpresas">Indica si trae la infomacion de todas las empresas</param>
        /// <returns></returns>     
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetForAprobacionGiroRechazo()
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommandSel.CommandText =
                    "select * from ccCreditoDocu docu with(nolock) " +
                    "where docu.EmpresaID = @EmpresaID and DocRechazo is not null and DocEstadoCuenta is  null and DocDesestimiento is null " +
                    "ORDER BY docu.ClienteID ";

                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    dto.Aprobado.Value = false;
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetForAprobacion");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de todos los creditos aprobados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="allEmpresas">Indica si trae la infomacion de todas las empresas</param>
        /// <returns></returns>     
        public List<DTO_CorreoCliente> DAL_ccCreditoDocu_GetClienteForCorreo(string clienteID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                string where = string.Empty;
                if (!string.IsNullOrEmpty(clienteID))
                {
                    where = "  and crd.ClienteID = @ClienteID  ";
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters["@ClienteID"].Value = clienteID;
                }
                   
                mySqlCommandSel.CommandText =
                  
                    " SELECT   crd.Libranza,crd.ClienteID,cli.Descriptivo as Nombre,cli.Correo,cli.CedEsposa as Conyuge, " +
		            "          crd.Codeudor1,crd.Codeudor2,crd.Codeudor3,crd.Codeudor4,crd.Codeudor5,1 as ClienteInd,crd.Estadodeuda,pla.NumeroDoc, " +
		            "          sum(pla.vlrCapital - (case when (pag.AboCapital is null) then 0 else pag.AboCapital end)) as SdoCapital,		 " +
                    "          sum(Pla.VlrSeguro  - (case when (pag.AboCapSegu is null) then 0 else pag.AboCapSegu end)) as SdoSeguro " +
                    "  FROM ccCreditoPlanPagos (nolock)  as pla " +
	                "      left join ( SELECT	pag.CreditoCuotaNum, " +
				    " 		                    SUM(vlrCapital)			as AboCapital, " +
				    " 		                    SUM(VlrInteres)			as AboInteres, " +
                    " 						    SUM(VlrSeguro)			as AboCapSegu, " +
					" 	                        SUM(VlrOtro1)			as AboIntSegu, " +
					" 	                        SUM(VlrOtrosfijos)		as AboOtrFijo, " +
					" 	                        SUM(VlrCapitalCesion)   as AboCapCesa, " +
					" 	                        SUM(VlrUtilidadCesion)	as AboUtiCesa, " +
					" 	                        SUM(VlrDerechosCesion)	as AboDerCesa " +
				    "                 FROM ccCreditoPagos (nolock) pag " +
				    "                 group by pag.CreditoCuotaNum) as  pag ON pag.CreditoCuotaNum = pla.consecutivo " +
	                "     left join ccCreditoDocu (nolock)as crd on crd.NumeroDoc = pla.NumeroDoc	 " +
                    "     left join ccCliente (nolock) as cli on  crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID =  @EmpresaID " +
                    "     left join ccCliente (nolock) as conyuge on conyuge.ClienteID = cli.CedEsposa and conyuge.EmpresaGrupoID =  @EmpresaID " +
                    " Where crd.EmpresaID = @EmpresaID " + where +
                    " group by crd.Libranza,crd.ClienteID,cli.Descriptivo,cli.Correo ,crd.TipoEstado,crd.EstadoDeuda,pla.NumeroDoc,cli.CedEsposa,crd.Codeudor1,crd.Codeudor2,crd.Codeudor3,crd.Codeudor4,crd.Codeudor5 " +
                    " HAVING sum(pla.vlrCapital - IsNull(pag.AboCapital,0)) > 0 or sum(Pla.VlrSeguro - IsNull(pag.AboCapSegu,0)) > 0 " +
                    " ORDER BY ClienteID ";

                List<DTO_CorreoCliente> result = new List<DTO_CorreoCliente>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_CorreoCliente dto;
                    dto = new DTO_CorreoCliente(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetClienteForCorreo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoDocu segun el codeudor
        /// </summary>
        /// <param name="codeudor">Identificador del codeudor</param>
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccCreditoDocu_GetCreditosByCodeudor(string codeudor)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string cancelado = string.Empty;
                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@Codeudor", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@CanceladoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@Codeudor"].Value = codeudor;
                mySqlCommand.Parameters["@CanceladoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    "SELECT DISTINCT cred.* FROM ccCreditoDocu cred with(nolock) " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cred.NumeroDoc AND ctrl.Estado=@Estado " +
                    "   WHERE (cred.Codeudor1=@Codeudor or cred.Codeudor2=@Codeudor or cred.Codeudor3=@Codeudor) and cred.CanceladoInd = 0 and cred.EmpresaID=@EmpresaID ";

                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto;
                    dto = new DTO_ccCreditoDocu(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoDocu_GetCreditosByCodeudor");
                throw exception;
            }
        }

        #endregion

    }
}
