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
    public class DAL_ccSolicitudDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD
        
        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudAnexo
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudAnexo</returns>
        public DTO_ccSolicitudDocu DAL_ccSolicitudDocu_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                DTO_ccSolicitudDocu result = new DTO_ccSolicitudDocu();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudDocu with(nolock) WHERE NumeroDoc = @NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccSolicitudDocu(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetByID");
                throw exception;
            }
        }
       
        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudDocu_Add(DTO_ccSolicitudDocu header)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "    INSERT INTO ccSolicitudDocu   " +
                                               "    (NumeroDoc   " +
                                               "    ,EmpresaID   " +
                                               "    ,ClienteID   " +
                                               "    ,ClienteRadica   " +
                                               "    ,ApellidoPri   " +
                                               "    ,ApellidoSdo   " +
                                               "    ,NombrePri   " +
                                               "    ,NombreSdo   " +
                                               "    ,Libranza   " +
                                               "    ,Solicitud   " +
                                               "    ,Pagare   " +
                                               "    ,PagarePol   " +
                                               "    ,ComponenteExtraInd   " +
                                               "    ,Poliza   " +
                                               "    ,LineaCreditoID   " +
                                               "    ,AsesorID   " +
                                               "    ,ConcesionarioID   " +
                                               "    ,AseguradoraID " +
                                               "    ,CooperativaID " +
                                               "    ,PagaduriaID   " +
                                               "    ,CentroPagoID   " +
                                               "    ,ZonaID   " +
                                               "    ,Ciudad   " +
                                               "    ,TipoCreditoID   " +
                                               "    ,NumDocCompra   " +
                                               "    ,VendedorID   " +
                                               "    ,TipoCredito   " +
                                               "    ,IncorporaMesInd  " +
                                               "    ,IncorporacionPreviaInd  " +
                                               "    ,IncorporacionTipo   " +
                                               "    ,NumDocIncorporacion   " +
                                               "    ,NumDocVerificado   " +
                                               "    ,NumDocOpera   " +
                                               "    ,PeriodoPago   " +
                                               "    ,FechaCuota1  " +
                                               "    ,CompraCarteraInd  " +
                                               "    ,FechaVto  " +
                                               "    ,TasaEfectivaCredito   " +
                                               "    ,PorInteres   " +
                                               "    ,PorSeguro   " +
                                               "    ,PorComponente1   " +
                                               "    ,PorComponente2   " +
                                               "    ,PorComponente3   " +
                                               "    ,VlrPrestamo   " +
                                               "    ,VlrLibranza   " +
                                               "    ,VlrCompra   " +
                                               "    ,VlrDescuento   " +
                                               "    ,VlrGiro   " +
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
                                               "    ,VlrCapacidad   " +
                                               "    ,PagoVentanillaInd   " +
                                               "    ,RechazoInd   " +
                                               "    ,RechazoCausal   " +
                                               "    ,RechazoFecha   " +
                                               "    ,AnalisisUsuario   " +
                                               "    ,RechazoUsuario   " +
                                               "    ,Observacion   " +
                                               "    ,Codeudor1   " +
                                               "    ,Codeudor2   " +
                                               "    ,Codeudor3   " +
                                               "    ,Codeudor4   " +
                                               "    ,Codeudor5   " +
                                               "    ,DatoAdd1   " +
                                               "    ,DatoAdd2   " +
                                               "    ,DatoAdd3   " +
                                               "    ,DatoAdd4   " +
                                               "    ,DatoAdd5   " +
                                               "    ,DatoAdd6   " +
                                               "    ,DatoAdd7   " +
                                               "    ,DatoAdd8   " +
                                               "    ,DatoAdd9   " +
                                               "    ,DatoAdd10   " +
                                               "    ,TipoGarantia   " +
                                               "    ,TipoOperacion   " +
                                               "    ,VlrSolicitado   " +
                                               "    ,VlrAdicional   " +
                                               "    ,VlrPreSolicitado   " +
                                               "    ,BancoID_1   " +
                                               "    ,CuentaTipo_1   " +
                                               "    ,BcoCtaNro_1   " +
                                               "    ,VersionNro   " +
                                               "    ,DtoPrimeraCuotaInd   " +
                                               "    ,ValorDtoPrimeraCuota   " +
                                               "    ,CancelaContadoPolizaInd" +
                                               "    ,CancelaContadoOtrosSegInd" +
                                               "    ,IntermediarioExternoInd" +
                                               "    ,VlrOtrasFinanciaciones" +
                                               "    ,OtrasFinancPagoContadoInd" +
                                               "    ,PrendaConyugueInd" +
                                               "    ,CartaInstrucciones" +
                                               "    ,CartaInstruccionesPOL" +
                                               "    ,ConsecutivoWEB" +
                                               "    ,DesestimientoInd" +
                                               "    ,NegociosGestionarInd" +
                                               "    ,ActividadFlujoNegociosGestionarID" +
                                               "    ,Concesionario2	" +
                                               "    ,eg_ccCliente   " +
                                               "    ,eg_ccLineaCredito   " +
                                               "    ,eg_ccAsesor   " +
                                               "    ,eg_ccConcesionario " +
                                               "    ,eg_ccAseguradora " +
                                               "    ,eg_ccCooperativa " +
                                               "    ,eg_ccPagaduria   " +
                                               "    ,eg_ccCentroPagoPAG   " +
                                               "    ,eg_glZona   " +
                                               "    ,eg_glLugarGeografico   " +
                                               "    ,eg_ccVendedorCartera   " +
                                               "    ,eg_ccTipoCredito  " +
                                               "    ,eg_tsBanco )   " +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@EmpresaID    " +
                                               "  ,@ClienteID    " +
                                               "  ,@ClienteRadica    " +
                                               "  ,@ApellidoPri    " +
                                               "  ,@ApellidoSdo    " +
                                               "  ,@NombrePri    " +
                                               "  ,@NombreSdo    " +
                                               "  ,@Libranza    " +
                                               "  ,@Solicitud  " +
                                               "  ,@Pagare   " +
                                               "  ,@PagarePol   " +
                                               "  ,@ComponenteExtraInd   " +
                                               "  ,@Poliza   " +
                                               "  ,@LineaCreditoID    " +
                                               "  ,@AsesorID    " +
                                               "  ,@ConcesionarioID   " +
                                               "  ,@AseguradoraID " +
                                               "  ,@CooperativaID " +
                                               "  ,@PagaduriaID    " +
                                               "  ,@CentroPagoID   " +
                                               "  ,@ZonaID    " +
                                               "  ,@Ciudad    " +
                                               "  ,@TipoCreditoID    " +
                                               "  ,@NumDocCompra    " +
                                               "  ,@VendedorID    " +
                                               "  ,@TipoCredito    " +
                                               "  ,@IncorporaMesInd  " +
                                               "  ,@IncorporacionPreviaInd  " +
                                               "  ,@IncorporacionTipo   " +
                                               "  ,@NumDocIncorporacion   " +
                                               "  ,@NumDocVerificado   " +
                                               "  ,@NumDocOpera   " +
                                               "  ,@PeriodoPago   " +
                                               "  ,@FechaCuota1  " +
                                               "  ,@CompraCarteraInd " +
                                               "  ,@FechaVto " +
                                               "  ,@TasaEfectivaCredito    " +
                                               "  ,@PorInteres    " +
                                               "  ,@PorSeguro    " +
                                               "  ,@PorComponente1    " +
                                               "  ,@PorComponente2    " +
                                               "  ,@PorComponente3    " +
                                               "  ,@VlrPrestamo    " +
                                               "  ,@VlrLibranza    " +
                                               "  ,@VlrCompra    " +
                                               "  ,@VlrDescuento    " +
                                               "  ,@VlrGiro    " +
                                               "  ,@VlrPoliza    " +
                                               "  ,@Plazo    " +
                                               "  ,@VlrCuota    " +
                                               "  ,@PlazoSeguro  " +
                                               "  ,@Cuota1Seguro  " +
                                               "  ,@VlrCuotaSeguro  " +
                                               "  ,@VlrFinanciaSeguro  " +
                                               "  ,@FechaLiqSeguro  " +
                                               "  ,@FechaVigenciaINI  " +
                                               "  ,@FechaVigenciaFIN  " +
                                               "  ,@DocSeguro  " +
                                               "  ,@VlrCupoDisponible    " +
                                               "  ,@VlrCapacidad    " +
                                               "  ,@PagoVentanillaInd    " +
                                               "  ,@RechazoInd    " +
                                               "  ,@RechazoCausal    " +
                                               "  ,@RechazoFecha    " +
                                               "  ,@AnalisisUsuario    " +
                                               "  ,@RechazoUsuario    " +
                                               "  ,@Observacion    " +
                                               "  ,@Codeudor1    " +
                                               "  ,@Codeudor2    " +
                                               "  ,@Codeudor3   " +
                                               "  ,@Codeudor4   " +
                                               "  ,@Codeudor5    " +
                                               "  ,@DatoAdd1    " +
                                               "  ,@DatoAdd2    " +
                                               "  ,@DatoAdd3    " +
                                               "  ,@DatoAdd4    " +
                                               "  ,@DatoAdd5    " +
                                               "  ,@DatoAdd6    " +
                                               "  ,@DatoAdd7    " +
                                               "  ,@DatoAdd8    " +
                                               "  ,@DatoAdd9    " +
                                               "  ,@DatoAdd10    " +
                                               "  ,@TipoGarantia" +
                                               "  ,@TipoOperacion" +                                               
                                               "  ,@VlrSolicitado    " +
                                               "  ,@VlrAdicional    " +
                                               "  ,@VlrPreSolicitado   " +
                                               "  ,@BancoID_1   " +
                                               "  ,@CuentaTipo_1   " +
                                               "  ,@BcoCtaNro_1   " +
                                               "  ,@VersionNro   " +
                                               "  ,@DtoPrimeraCuotaInd   " +
                                               "  ,@ValorDtoPrimeraCuota   " +
                                               "  ,@CancelaContadoPolizaInd" +
                                               "  ,@CancelaContadoOtrosSegInd" +
                                               "  ,@IntermediarioExternoInd" +
                                               "  ,@VlrOtrasFinanciaciones" +
                                               "  ,@OtrasFinancPagoContadoInd" +
                                               "  ,@PrendaConyugueInd"+
                                               "  ,@CartaInstrucciones" +
                                               "  ,@CartaInstruccionesPOL" +
                                               "  ,@ConsecutivoWEB" +
                                               "  ,@DesestimientoInd" +
                                               "  ,@NegociosGestionarInd" +
                                               "  ,@ActividadFlujoNegociosGestionarID" +
                                               "  ,@Concesionario2	" +
                                               "  ,@eg_ccCliente    " +
                                               "  ,@eg_ccLineaCredito    " +
                                               "  ,@eg_ccAsesor    " +
                                               "  ,@eg_ccConcesionario " +
                                               "  ,@eg_ccAseguradora " +
                                               "  ,@eg_ccCooperativa " +
                                               "  ,@eg_ccPagaduria    " +
                                               "  ,@eg_ccCentroPagoPAG    " +
                                               "  ,@eg_glZona    " +
                                               "  ,@eg_glLugarGeografico   " +
                                               "  ,@eg_ccVendedorCartera   " +
                                               "  ,@eg_ccTipoCredito   " +
                                               "  ,@eg_tsBanco)   ";


                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteRadica", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoPri", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@ApellidoSdo", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@NombrePri", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@NombreSdo", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncorporaMesInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IncorporacionPreviaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IncorporacionTipo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumDocIncorporacion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocVerificado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocOpera", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumDocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorComponente1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PagoVentanillaInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AnalisisUsuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RechazoUsuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CooperativaID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCreditoID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@VendedorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor1", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor2", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor3", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor4", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor5", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd6", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd7", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd8", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd9", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd10", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@TipoGarantia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoOperacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CompraCarteraInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaCredito", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaVto", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@PorInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAdicional", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPreSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@BancoID_1", SqlDbType.Char, UDT_BancoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@BcoCtaNro_1", SqlDbType.VarChar);
                mySqlCommandSel.Parameters.Add("@CuentaTipo_1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VersionNro", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@VlrPrestamo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDescuento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGiro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCupoDisponible", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapacidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RechazoInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@RechazoCausal", SqlDbType.Char, UDT_CausalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RechazoFecha", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Solicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Pagare", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@PagarePOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@ComponenteExtraInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PlazoSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Cuota1Seguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotaSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrFinanciaSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaLiqSeguro", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaINI", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaFIN", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DtoPrimeraCuotaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorDtoPrimeraCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CancelaContadoPolizaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CancelaContadoOtrosSegInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IntermediarioExternoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrOtrasFinanciaciones", SqlDbType.Int);                
                mySqlCommandSel.Parameters.Add("@OtrasFinancPagoContadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PrendaConyugueInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaInstrucciones", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@CartaInstruccionesPOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@ConsecutivoWEB", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DesestimientoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NegociosGestionarInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoNegociosGestionarID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Concesionario2", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);                
                mySqlCommandSel.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccConcesionario", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCooperativa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccVendedorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_cctipoCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ClienteRadica"].Value = header.ClienteRadica.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = header.ClienteID.Value;
                mySqlCommandSel.Parameters["@ApellidoPri"].Value = header.ApellidoPri.Value;
                mySqlCommandSel.Parameters["@ApellidoSdo"].Value = header.ApellidoSdo.Value;
                mySqlCommandSel.Parameters["@NombrePri"].Value = header.NombrePri.Value;
                mySqlCommandSel.Parameters["@NombreSdo"].Value = header.NombreSdo.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = header.Libranza.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = header.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = header.AsesorID.Value;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = header.ConcesionarioID.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = header.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@CooperativaID"].Value = header.CooperativaID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = header.ZonaID.Value;
                mySqlCommandSel.Parameters["@Ciudad"].Value = header.Ciudad.Value;
                mySqlCommandSel.Parameters["@TipoCreditoID"].Value = header.TipoCreditoID.Value;
                mySqlCommandSel.Parameters["@VendedorID"].Value = header.VendedorID.Value;
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = header.PagaduriaID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = header.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@IncorporaMesInd"].Value = header.IncorporaMesInd.Value;
                mySqlCommandSel.Parameters["@IncorporacionPreviaInd"].Value = header.IncorporacionPreviaInd.Value;
                mySqlCommandSel.Parameters["@IncorporacionTipo"].Value = header.IncorporacionTipo.Value;
                mySqlCommandSel.Parameters["@NumDocIncorporacion"].Value = header.NumDocIncorporacion.Value;
                mySqlCommandSel.Parameters["@NumDocVerificado"].Value = header.NumDocVerificado.Value;
                mySqlCommandSel.Parameters["@NumDocOpera"].Value = header.NumDocOpera.Value;
                mySqlCommandSel.Parameters["@PeriodoPago"].Value = header.PeriodoPago.Value;
                mySqlCommandSel.Parameters["@NumDocCompra"].Value = header.NumDocCompra.Value;
                mySqlCommandSel.Parameters["@PorComponente1"].Value = header.PorComponente1.Value;
                mySqlCommandSel.Parameters["@PorComponente2"].Value = header.PorComponente2.Value;
                mySqlCommandSel.Parameters["@PorComponente3"].Value = header.PorComponente3.Value;
                mySqlCommandSel.Parameters["@PagoVentanillaInd"].Value = header.PagoVentanillaInd.Value;
                mySqlCommandSel.Parameters["@AnalisisUsuario"].Value = header.AnalisisUsuario.Value;
                mySqlCommandSel.Parameters["@RechazoUsuario"].Value = header.RechazoUsuario.Value;
                mySqlCommandSel.Parameters["@Codeudor1"].Value = header.Codeudor1.Value;
                mySqlCommandSel.Parameters["@Codeudor2"].Value = header.Codeudor2.Value;
                mySqlCommandSel.Parameters["@Codeudor3"].Value = header.Codeudor3.Value;
                mySqlCommandSel.Parameters["@Codeudor4"].Value = header.Codeudor4.Value;
                mySqlCommandSel.Parameters["@Codeudor5"].Value = header.Codeudor5.Value;
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = header.DatoAdd1.Value;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value = header.DatoAdd2.Value;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value = header.DatoAdd3.Value;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value = header.DatoAdd4.Value;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value = header.DatoAdd5.Value;
                mySqlCommandSel.Parameters["@DatoAdd6"].Value = header.DatoAdd6.Value;
                mySqlCommandSel.Parameters["@DatoAdd7"].Value = header.DatoAdd7.Value;
                mySqlCommandSel.Parameters["@DatoAdd8"].Value = header.DatoAdd8.Value;
                mySqlCommandSel.Parameters["@DatoAdd9"].Value = header.DatoAdd9.Value;
                mySqlCommandSel.Parameters["@DatoAdd10"].Value = header.DatoAdd10.Value;
                mySqlCommandSel.Parameters["@TipoGarantia"].Value = header.TipoGarantia.Value;
                mySqlCommandSel.Parameters["@TipoOperacion"].Value = header.TipoOperacion.Value;
                mySqlCommandSel.Parameters["@TipoCredito"].Value = header.TipoCredito.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = header.FechaCuota1.Value;
                mySqlCommandSel.Parameters["@CompraCarteraInd"].Value = header.CompraCarteraInd.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaCredito"].Value = header.TasaEfectivaCredito.Value;
                mySqlCommandSel.Parameters["@FechaVto"].Value = header.FechaVto.Value;
                mySqlCommandSel.Parameters["@PorInteres"].Value = header.PorInteres.Value;
                mySqlCommandSel.Parameters["@PorSeguro"].Value = header.PorSeguro.Value;
                mySqlCommandSel.Parameters["@VlrSolicitado"].Value = header.VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@VlrAdicional"].Value = header.VlrAdicional.Value;
                mySqlCommandSel.Parameters["@VlrPreSolicitado"].Value = header.VlrPreSolicitado.Value;
                mySqlCommandSel.Parameters["@BancoID_1"].Value = header.BancoID_1.Value;
                mySqlCommandSel.Parameters["@BcoCtaNro_1"].Value = header.BcoCtaNro_1.Value;
                mySqlCommandSel.Parameters["@CuentaTipo_1"].Value = header.CuentaTipo_1.Value;
                mySqlCommandSel.Parameters["@VersionNro"].Value = header.VersionNro.Value;
                mySqlCommandSel.Parameters["@VlrPrestamo"].Value = header.VlrPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = header.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrCompra"].Value = header.VlrCompra.Value;
                mySqlCommandSel.Parameters["@VlrDescuento"].Value = header.VlrDescuento.Value;
                mySqlCommandSel.Parameters["@VlrGiro"].Value = header.VlrGiro.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = header.Plazo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = header.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCupoDisponible"].Value = header.VlrCupoDisponible.Value;
                mySqlCommandSel.Parameters["@VlrCapacidad"].Value = header.VlrCapacidad.Value;
                mySqlCommandSel.Parameters["@RechazoInd"].Value = header.RechazoInd.Value;
                mySqlCommandSel.Parameters["@RechazoCausal"].Value = header.RechazoCausal.Value;
                mySqlCommandSel.Parameters["@RechazoFecha"].Value = header.RechazoFecha.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = header.Observacion.Value;
                mySqlCommandSel.Parameters["@Solicitud"].Value = header.Solicitud.Value;
                mySqlCommandSel.Parameters["@Pagare"].Value = header.Pagare.Value;
                mySqlCommandSel.Parameters["@PagarePOL"].Value = header.PagarePOL.Value;
                mySqlCommandSel.Parameters["@ComponenteExtraInd"].Value = !string.IsNullOrEmpty(header.ComponenteExtraInd.Value.ToString()) ? header.ComponenteExtraInd.Value : true;
                mySqlCommandSel.Parameters["@Poliza"].Value = header.Poliza.Value;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = header.ConcesionarioID.Value;
                mySqlCommandSel.Parameters["@PlazoSeguro"].Value = header.PlazoSeguro.Value;
                mySqlCommandSel.Parameters["@Cuota1Seguro"].Value = header.Cuota1Seguro.Value;
                mySqlCommandSel.Parameters["@VlrCuotaSeguro"].Value = header.VlrCuotaSeguro.Value;
                mySqlCommandSel.Parameters["@VlrFinanciaSeguro"].Value = header.VlrFinanciaSeguro.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = header.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@FechaLiqSeguro"].Value = header.FechaLiqSeguro.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaINI"].Value = header.FechaVigenciaINI.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaFIN"].Value = header.FechaVigenciaFIN.Value;
                mySqlCommandSel.Parameters["@DocSeguro"].Value = header.DocSeguro.Value;
                mySqlCommandSel.Parameters["@DtoPrimeraCuotaInd"].Value = header.DtoPrimeraCuotaInd.Value;
                mySqlCommandSel.Parameters["@ValorDtoPrimeraCuota"].Value = header.ValorDtoPrimeraCuota.Value;
                mySqlCommandSel.Parameters["@CancelaContadoPolizaInd"].Value = header.CancelaContadoPolizaInd.Value;
                mySqlCommandSel.Parameters["@CancelaContadoOtrosSegInd"].Value = header.CancelaContadoOtrosSegInd.Value;
                mySqlCommandSel.Parameters["@IntermediarioExternoInd"].Value = header.IntermediarioExternoInd.Value;
                mySqlCommandSel.Parameters["@VlrOtrasFinanciaciones"].Value = header.VlrOtrasFinanciaciones.Value;
                mySqlCommandSel.Parameters["@OtrasFinancPagoContadoInd"].Value = header.OtrasFinancPagoContadoInd.Value;
                mySqlCommandSel.Parameters["@PrendaConyugueInd"].Value = header.PrendaConyugueInd.Value;
                mySqlCommandSel.Parameters["@CartaInstrucciones"].Value = header.CartaInstrucciones.Value;
                mySqlCommandSel.Parameters["@CartaInstruccionesPOL"].Value = header.CartaInstruccionesPOL.Value;
                mySqlCommandSel.Parameters["@ConsecutivoWEB"].Value = header.ConsecutivoWEB.Value;

                mySqlCommandSel.Parameters["@DesestimientoInd"].Value = header.DesestimientoInd.Value;
                mySqlCommandSel.Parameters["@NegociosGestionarInd"].Value = header.NegociosGestionarInd.Value;
                mySqlCommandSel.Parameters["@ActividadFlujoNegociosGestionarID"].Value = header.ActividadFlujoNegociosGestionarID.Value;
                mySqlCommandSel.Parameters["@Concesionario2"].Value = header.Concesionario2.Value;


                mySqlCommandSel.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAsesor, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);

                //Concesionario
                if (!string.IsNullOrWhiteSpace(header.ConcesionarioID.Value))
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccConcesionario, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = DBNull.Value;

                //Aseguradora
                if (!string.IsNullOrWhiteSpace(header.AseguradoraID.Value))
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = DBNull.Value;

                //Cooperativa
                if (!string.IsNullOrWhiteSpace(header.CooperativaID.Value))
                    mySqlCommandSel.Parameters["@eg_ccCooperativa"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCooperativa, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccCooperativa"].Value = DBNull.Value;

                //Vendedor
                if (!string.IsNullOrWhiteSpace(header.VendedorID.Value))
                    mySqlCommandSel.Parameters["@eg_ccVendedorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccVendedorCartera, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccVendedorCartera"].Value = DBNull.Value;

                //Tipo de crédito
                if (!string.IsNullOrWhiteSpace(header.TipoCreditoID.Value))
                    mySqlCommandSel.Parameters["@eg_ccTipoCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccTipoCredito, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccTipoCredito"].Value = DBNull.Value;

                //Tipo de crédito
                if (!string.IsNullOrWhiteSpace(header.BancoID_1.Value))
                    mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_tsBanco"].Value = DBNull.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudDocu_Update(DTO_ccSolicitudDocu header)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                           "UPDATE ccSolicitudDocu SET" +
                                               " ClienteID = @ClienteID  " +
                                               " ,ClienteRadica = @ClienteRadica   " +
                                               " ,ApellidoPri = @ApellidoPri   " +
                                               " ,ApellidoSdo = @ApellidoSdo  " +
                                               " ,NombrePri = @NombrePri  " +
                                               " ,NombreSdo= @NombreSdo   " +
                                               " ,Libranza = @Libranza  " +
                                               " ,Solicitud = @Solicitud  " +
                                               " ,Pagare = @Pagare  " +
                                               " ,PagarePOL = @PagarePOL  " +
                                               " ,ComponenteExtraInd = @ComponenteExtraInd  " +
                                               " ,Poliza = @Poliza  " +
                                               " ,LineaCreditoID = @LineaCreditoID  " +
                                               " ,AsesorID = @AsesorID  " +
                                               " ,ConcesionarioID = @ConcesionarioID  " +
                                               " ,AseguradoraID = @AseguradoraID  " +
                                               " ,CooperativaID = @CooperativaID  " +
                                               " ,PagaduriaID = @PagaduriaID  " +
                                               " ,CentroPagoID = @CentroPagoID   " +
                                               " ,ZonaID = @ZonaID  " +
                                               " ,Ciudad = @Ciudad  " +
                                               " ,TipoCreditoID = @TipoCreditoID  " +
                                               " ,NumDocCompra = @NumDocCompra  " +
                                               " ,VendedorID = @VendedorID  " +
                                               " ,TipoCredito = @TipoCredito  " +
                                               " ,IncorporaMesInd = @IncorporaMesInd  " +
                                               " ,IncorporacionPreviaInd = @IncorporacionPreviaInd  " +
                                               " ,IncorporacionTipo = @IncorporacionTipo  " +
                                               " ,NumDocIncorporacion = @NumDocIncorporacion  " +
                                               " ,NumDocVerificado = @NumDocVerificado  " +
                                               " ,NumDocOpera = @NumDocOpera " +
                                               " ,PeriodoPago = @PeriodoPago " +
                                               " ,FechaCuota1 = @FechaCuota1  " +
                                               " ,CompraCarteraInd = @CompraCarteraInd  " +
                                               " ,FechaVto = @FechaVto  " +
                                               " ,TasaEfectivaCredito= @TasaEfectivaCredito   " +
                                               " ,PorInteres= @PorInteres   " +
                                               " ,PorSeguro = @PorSeguro   " +
                                               " ,PorComponente1 = @PorComponente1   " +
                                               " ,PorComponente2 = @PorComponente2   " +
                                               " ,PorComponente3 = @PorComponente3   " +
                                               " ,VlrPrestamo = @VlrPrestamo  " +
                                               " ,VlrLibranza = @VlrLibranza  " +
                                               " ,VlrCompra = @VlrCompra  " +
                                               " ,VlrDescuento= @VlrDescuento  " +
                                               " ,VlrGiro  = @VlrGiro " +
                                               " ,VlrPoliza  = @VlrPoliza " +
                                               " ,Plazo  = @Plazo " +
                                               " ,VlrCuota = @VlrCuota  " +
                                               " ,PlazoSeguro = @PlazoSeguro  " +
                                               " ,Cuota1Seguro = @Cuota1Seguro  " +
                                               " ,VlrCuotaSeguro = @VlrCuotaSeguro  " +
                                               " ,VlrFinanciaSeguro = @VlrFinanciaSeguro  " +
                                               " ,FechaLiqSeguro = @FechaLiqSeguro  " +
                                               " ,FechaVigenciaINI = @FechaVigenciaINI  " +
                                               " ,FechaVigenciaFIN = @FechaVigenciaFIN  " +
                                               " ,DocSeguro = @DocSeguro  " +
                                               " ,VlrCupoDisponible= @VlrCupoDisponible   " +
                                               " ,VlrCapacidad= @VlrCapacidad   " +
                                               " ,PagoVentanillaInd= @PagoVentanillaInd   " +
                                               " ,RechazoInd = @RechazoInd  " +
                                               " ,RechazoCausal  = @RechazoCausal " +
                                               " ,RechazoFecha = @RechazoFecha  " +
                                               " ,AnalisisUsuario = @AnalisisUsuario  " +
                                               " ,RechazoUsuario= @RechazoUsuario   " +
                                               " ,Observacion  = @Observacion " +
                                               " ,Codeudor1 = @Codeudor1 " +
                                               " ,Codeudor2 = @Codeudor2 " +
                                               " ,Codeudor3 = @Codeudor3 " +
                                               " ,Codeudor4 = @Codeudor4 " +
                                               " ,Codeudor5 = @Codeudor5 " +
                                               " ,DatoAdd1 = @DatoAdd1 " +
                                               " ,DatoAdd2 = @DatoAdd2 " +
                                               " ,DatoAdd3 = @DatoAdd3 " +
                                               " ,DatoAdd4 = @DatoAdd4 " +
                                               " ,DatoAdd5 = @DatoAdd5 " +
                                               " ,DatoAdd6 = @DatoAdd6 " +
                                               " ,DatoAdd7 = @DatoAdd7 " +
                                               " ,DatoAdd8 = @DatoAdd8 " +
                                               " ,DatoAdd9 = @DatoAdd9 " +
                                               " ,DatoAdd10 = @DatoAdd10 " +
                                               " ,TipoGarantia = @TipoGarantia " +
                                               " ,TipoOperacion = @TipoOperacion " +
                                               " ,VlrSolicitado = @VlrSolicitado " +
                                               " ,VlrAdicional = @VlrAdicional " +
                                               " ,VlrPreSolicitado =@VlrPreSolicitado   " +
                                               " ,BancoID_1 =@BancoID_1   " +
                                               " ,CuentaTipo_1 =@CuentaTipo_1   " +
                                               " ,BcoCtaNro_1 =@BcoCtaNro_1   " +
                                               " ,VersionNro =@VersionNro   " +
                                               " ,DevueltaInd = @DevueltaInd " +
                                               " ,DtoPrimeraCuotaInd = @DtoPrimeraCuotaInd  " +
                                               " ,ValorDtoPrimeraCuota  = @ValorDtoPrimeraCuota " +
                                               " ,CancelaContadoPolizaInd=@CancelaContadoPolizaInd" +
                                               " ,CancelaContadoOtrosSegInd=@CancelaContadoOtrosSegInd" +
                                               " ,IntermediarioExternoInd=@IntermediarioExternoInd" +
                                               " ,VlrOtrasFinanciaciones=@VlrOtrasFinanciaciones" +
                                               " ,OtrasFinancPagoContadoInd=@OtrasFinancPagoContadoInd" +
                                               " ,PrendaConyugueInd=@PrendaConyugueInd" +
                                               " ,CartaInstrucciones=@CartaInstrucciones" +
                                               " ,CartaInstruccionesPOL=@CartaInstruccionesPOL" +
                                               " ,ConsecutivoWEB=@ConsecutivoWEB" +
                                               " ,DesestimientoInd=@DesestimientoInd" +
                                               " ,NegociosGestionarInd=@NegociosGestionarInd" +
                                               " ,ActividadFlujoNegociosGestionarID=@ActividadFlujoNegociosGestionarID" +
                                               " ,Concesionario2=@Concesionario2" +
                                               " ,HipotecaNuevaInd=@HipotecaNuevaInd" +
                                               " ,PrendaNuevaInd=@PrendaNuevaInd" +
                                               " ,HipotecaNuevaInd2=@HipotecaNuevaInd2" +
                                               " ,PrendaNuevaInd2=@PrendaNuevaInd2" +
                                               " WHERE  NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteRadica", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ApellidoPri", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@ApellidoSdo", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@NombrePri", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@NombreSdo", SqlDbType.Char, 100);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@CooperativaID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCreditoID", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommandSel.Parameters.Add("@VendedorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor1", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor2", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor3", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor4", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Codeudor5", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd6", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd7", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd8", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd9", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@DatoAdd10", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@TipoGarantia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoOperacion", SqlDbType.TinyInt);                
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncorporaMesInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IncorporacionPreviaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IncorporacionTipo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NumDocIncorporacion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocVerificado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocOpera", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumDocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorComponente1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorComponente3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PagoVentanillaInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AnalisisUsuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RechazoUsuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CompraCarteraInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaVto", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TasaEfectivaCredito", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PorSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAdicional", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPreSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@BancoID_1", SqlDbType.Char, UDT_BancoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@BcoCtaNro_1", SqlDbType.VarChar);
                mySqlCommandSel.Parameters.Add("@CuentaTipo_1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VersionNro", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@VlrPrestamo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrDescuento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGiro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCupoDisponible", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCapacidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RechazoInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@RechazoCausal", SqlDbType.Char, UDT_CausalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RechazoFecha", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Solicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Pagare", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@PagarePOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@ComponenteExtraInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PlazoSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Cuota1Seguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotaSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrFinanciaSeguro", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaLiqSeguro", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaINI", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaFIN", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DevueltaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DtoPrimeraCuotaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorDtoPrimeraCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CancelaContadoPolizaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CancelaContadoOtrosSegInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@IntermediarioExternoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrOtrasFinanciaciones", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@OtrasFinancPagoContadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PrendaConyugueInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaInstrucciones", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@CartaInstruccionesPOL", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@ConsecutivoWEB", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DesestimientoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NegociosGestionarInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoNegociosGestionarID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Concesionario2", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@HipotecaNuevaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PrendaNuevaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@HipotecaNuevaInd2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PrendaNuevaInd2", SqlDbType.Bit);

                mySqlCommandSel.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glZona", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccVendedorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccConcesionario", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCooperativa", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = header.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ClienteRadica"].Value = header.ClienteRadica.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = header.ClienteID.Value;
                mySqlCommandSel.Parameters["@ApellidoPri"].Value = header.ApellidoPri.Value;
                mySqlCommandSel.Parameters["@ApellidoSdo"].Value = header.ApellidoSdo.Value;
                mySqlCommandSel.Parameters["@NombrePri"].Value = header.NombrePri.Value;
                mySqlCommandSel.Parameters["@NombreSdo"].Value = header.NombreSdo.Value;
                mySqlCommandSel.Parameters["@Libranza"].Value = header.Libranza.Value;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = header.LineaCreditoID.Value;
                mySqlCommandSel.Parameters["@AsesorID"].Value = header.AsesorID.Value;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = header.ConcesionarioID.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = header.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@CooperativaID"].Value = header.CooperativaID.Value;
                mySqlCommandSel.Parameters["@ZonaID"].Value = header.ZonaID.Value;
                mySqlCommandSel.Parameters["@Ciudad"].Value = header.Ciudad.Value;
                mySqlCommandSel.Parameters["@TipoCreditoID"].Value = header.TipoCreditoID.Value;
                mySqlCommandSel.Parameters["@VendedorID"].Value = header.VendedorID.Value;
                mySqlCommandSel.Parameters["@Codeudor1"].Value = header.Codeudor1.Value;
                mySqlCommandSel.Parameters["@Codeudor2"].Value = header.Codeudor2.Value;
                mySqlCommandSel.Parameters["@Codeudor3"].Value = header.Codeudor3.Value;
                mySqlCommandSel.Parameters["@Codeudor4"].Value = header.Codeudor4.Value;
                mySqlCommandSel.Parameters["@Codeudor5"].Value = header.Codeudor5.Value;
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = header.DatoAdd1.Value;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value = header.DatoAdd2.Value;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value = header.DatoAdd3.Value;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value = header.DatoAdd4.Value;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value = header.DatoAdd5.Value;
                mySqlCommandSel.Parameters["@DatoAdd6"].Value = header.DatoAdd6.Value;
                mySqlCommandSel.Parameters["@DatoAdd7"].Value = header.DatoAdd7.Value;
                mySqlCommandSel.Parameters["@DatoAdd8"].Value = header.DatoAdd8.Value;
                mySqlCommandSel.Parameters["@DatoAdd9"].Value = header.DatoAdd9.Value;
                mySqlCommandSel.Parameters["@DatoAdd10"].Value = header.DatoAdd10.Value;
                mySqlCommandSel.Parameters["@TipoGarantia"].Value = header.TipoGarantia.Value;
                mySqlCommandSel.Parameters["@TipoOperacion"].Value = header.TipoOperacion.Value;                
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = header.PagaduriaID.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = header.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@IncorporaMesInd"].Value = header.IncorporaMesInd.Value;
                mySqlCommandSel.Parameters["@IncorporacionPreviaInd"].Value = header.IncorporacionPreviaInd.Value;
                mySqlCommandSel.Parameters["@IncorporacionTipo"].Value = header.IncorporacionTipo.Value;
                mySqlCommandSel.Parameters["@NumDocIncorporacion"].Value = header.NumDocIncorporacion.Value;
                mySqlCommandSel.Parameters["@NumDocVerificado"].Value = header.NumDocVerificado.Value;
                mySqlCommandSel.Parameters["@NumDocOpera"].Value = header.NumDocOpera.Value;
                mySqlCommandSel.Parameters["@PeriodoPago"].Value = header.PeriodoPago.Value;
                mySqlCommandSel.Parameters["@NumDocCompra"].Value = header.NumDocCompra.Value;
                mySqlCommandSel.Parameters["@PorComponente1"].Value = header.PorComponente1.Value;
                mySqlCommandSel.Parameters["@PorComponente2"].Value = header.PorComponente2.Value;
                mySqlCommandSel.Parameters["@PorComponente3"].Value = header.PorComponente3.Value;
                mySqlCommandSel.Parameters["@PagoVentanillaInd"].Value = header.PagoVentanillaInd.Value;
                mySqlCommandSel.Parameters["@AnalisisUsuario"].Value = header.AnalisisUsuario.Value;
                mySqlCommandSel.Parameters["@RechazoUsuario"].Value = header.RechazoUsuario.Value;
                mySqlCommandSel.Parameters["@TipoCredito"].Value = header.TipoCredito.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = header.FechaCuota1.Value;
                mySqlCommandSel.Parameters["@CompraCarteraInd"].Value = header.CompraCarteraInd.Value;
                mySqlCommandSel.Parameters["@FechaVto"].Value = header.FechaVto.Value;
                mySqlCommandSel.Parameters["@TasaEfectivaCredito"].Value = header.TasaEfectivaCredito.Value;
                mySqlCommandSel.Parameters["@PorInteres"].Value = header.PorInteres.Value;
                mySqlCommandSel.Parameters["@PorSeguro"].Value = header.PorSeguro.Value;
                mySqlCommandSel.Parameters["@VlrSolicitado"].Value = header.VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@VlrAdicional"].Value = header.VlrAdicional.Value;
                mySqlCommandSel.Parameters["@VlrPreSolicitado"].Value = header.VlrPreSolicitado.Value;
                mySqlCommandSel.Parameters["@BancoID_1"].Value = header.BancoID_1.Value;
                mySqlCommandSel.Parameters["@BcoCtaNro_1"].Value = header.BcoCtaNro_1.Value;
                mySqlCommandSel.Parameters["@CuentaTipo_1"].Value = header.CuentaTipo_1.Value;
                mySqlCommandSel.Parameters["@VersionNro"].Value = header.VersionNro.Value;
                mySqlCommandSel.Parameters["@VlrPrestamo"].Value = header.VlrPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = header.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrCompra"].Value = header.VlrCompra.Value;
                mySqlCommandSel.Parameters["@VlrDescuento"].Value = header.VlrDescuento.Value;
                mySqlCommandSel.Parameters["@VlrGiro"].Value = header.VlrGiro.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = header.Plazo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = header.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCupoDisponible"].Value = header.VlrCupoDisponible.Value;
                mySqlCommandSel.Parameters["@VlrCapacidad"].Value = header.VlrCapacidad.Value;
                mySqlCommandSel.Parameters["@RechazoInd"].Value = header.RechazoInd.Value;
                mySqlCommandSel.Parameters["@RechazoCausal"].Value = header.RechazoCausal.Value;
                mySqlCommandSel.Parameters["@RechazoFecha"].Value = header.RechazoFecha.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = header.Observacion.Value;
                mySqlCommandSel.Parameters["@Solicitud"].Value = header.Solicitud.Value;
                mySqlCommandSel.Parameters["@Pagare"].Value = header.Pagare.Value;
                mySqlCommandSel.Parameters["@PagarePOL"].Value = header.PagarePOL.Value;
                mySqlCommandSel.Parameters["@ComponenteExtraInd"].Value = !string.IsNullOrEmpty(header.ComponenteExtraInd.Value.ToString()) ? header.ComponenteExtraInd.Value : true;
                mySqlCommandSel.Parameters["@Poliza"].Value = header.Poliza.Value;
                mySqlCommandSel.Parameters["@PlazoSeguro"].Value = header.PlazoSeguro.Value;
                mySqlCommandSel.Parameters["@Cuota1Seguro"].Value = header.Cuota1Seguro.Value;
                mySqlCommandSel.Parameters["@VlrCuotaSeguro"].Value = header.VlrCuotaSeguro.Value;
                mySqlCommandSel.Parameters["@VlrFinanciaSeguro"].Value = header.VlrFinanciaSeguro.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = header.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@FechaLiqSeguro"].Value = header.FechaLiqSeguro.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaINI"].Value = header.FechaVigenciaINI.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaFIN"].Value = header.FechaVigenciaFIN.Value;
                mySqlCommandSel.Parameters["@DocSeguro"].Value = header.DocSeguro.Value;
                mySqlCommandSel.Parameters["@DevueltaInd"].Value = header.DevueltaInd.Value;
                mySqlCommandSel.Parameters["@DtoPrimeraCuotaInd"].Value = header.DtoPrimeraCuotaInd.Value;
                mySqlCommandSel.Parameters["@ValorDtoPrimeraCuota"].Value = header.ValorDtoPrimeraCuota.Value;
                mySqlCommandSel.Parameters["@CancelaContadoPolizaInd"].Value = header.CancelaContadoPolizaInd.Value;
                mySqlCommandSel.Parameters["@CancelaContadoOtrosSegInd"].Value = header.CancelaContadoOtrosSegInd.Value;
                mySqlCommandSel.Parameters["@IntermediarioExternoInd"].Value = header.IntermediarioExternoInd.Value;
                mySqlCommandSel.Parameters["@VlrOtrasFinanciaciones"].Value = header.VlrOtrasFinanciaciones.Value;
                mySqlCommandSel.Parameters["@OtrasFinancPagoContadoInd"].Value = header.OtrasFinancPagoContadoInd.Value;
                mySqlCommandSel.Parameters["@PrendaConyugueInd"].Value = header.PrendaConyugueInd.Value;
                mySqlCommandSel.Parameters["@CartaInstrucciones"].Value = header.CartaInstrucciones.Value;
                mySqlCommandSel.Parameters["@CartaInstruccionesPOL"].Value = header.CartaInstruccionesPOL.Value;
                mySqlCommandSel.Parameters["@ConsecutivoWEB"].Value = header.ConsecutivoWEB.Value;

                mySqlCommandSel.Parameters["@DesestimientoInd"].Value = header.DesestimientoInd.Value;
                mySqlCommandSel.Parameters["@NegociosGestionarInd"].Value = header.NegociosGestionarInd.Value;
                mySqlCommandSel.Parameters["@ActividadFlujoNegociosGestionarID"].Value = header.ActividadFlujoNegociosGestionarID.Value;
                mySqlCommandSel.Parameters["@Concesionario2"].Value = header.Concesionario2.Value;
                mySqlCommandSel.Parameters["@HipotecaNuevaInd"].Value = header.HipotecaNuevaInd.Value;
                mySqlCommandSel.Parameters["@PrendaNuevaInd"].Value = header.PrendaNuevaInd.Value;
                mySqlCommandSel.Parameters["@HipotecaNuevaInd2"].Value = header.HipotecaNuevaInd2.Value;
                mySqlCommandSel.Parameters["@PrendaNuevaInd2"].Value = header.PrendaNuevaInd2.Value;
                mySqlCommandSel.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaCredito, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAsesor, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);

                //Vendedor
                if (!string.IsNullOrWhiteSpace(header.VendedorID.Value))
                    mySqlCommandSel.Parameters["@eg_ccVendedorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccVendedorCartera, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccVendedorCartera"].Value = DBNull.Value;

                //Concesionario
                if (!string.IsNullOrWhiteSpace(header.ConcesionarioID.Value))
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccConcesionario, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccConcesionario"].Value = DBNull.Value;

                //Aseguradora
                if (!string.IsNullOrWhiteSpace(header.AseguradoraID.Value))
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
                else
                    mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = DBNull.Value;

                //Cooperativa
                if (!string.IsNullOrWhiteSpace(header.CooperativaID.Value))
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
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudDocu_Update");
                throw exception;
            }
        }
        #endregion

        #region Otras

        /// <summary>
        /// Trae la info de un credito
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranza">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccSolicitudDocu DAL_ccSolicitudDocu_GetByLibranza(int libranza)
        {
            try
            {
                DTO_ccSolicitudDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@Libranza"].Value = libranza;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;


                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudDocu with(nolock)  " +
                                       "WHERE EmpresaID = @EmpresaID and Libranza = @Libranza order by NumeroDoc";


                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccSolicitudDocu(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudDocu_GetByLibranza");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la info de un credito
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranza">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccSolicitudDocu DAL_ccSolicitudDocu_GetByRenovacionPoliza(int libranza, string poliza)
        {
            try
            {
                DTO_ccSolicitudDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Poliza", SqlDbType.VarChar);


                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Libranza"].Value = libranza;
                mySqlCommand.Parameters["@Poliza"].Value = poliza;


                mySqlCommand.CommandText =
                    "SELECT * FROM ccSolicitudDocu soli with(nolock)  " +
                  "	INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = soli.NumeroDoc " +
                    "WHERE soli.EmpresaID = @EmpresaID and ctrl.DocumentoID = 160 and ctrl.Estado not in(0,4) and soli.Libranza = @Libranza and soli.Poliza = @Poliza " +
                    "ORDER BY soli.NumeroDoc";


                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccSolicitudDocu(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudDocu_GetByLibranza");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <returns></returns>     
        public List<DTO_SolicitudAprobacionCartera> DAL_ccSolicitudDocu_GetForVerificacion(int documentoID, string actFlujoID, string usuarioID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ModuloID"].Value = ModulesPrefix.cc;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = usuarioID;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT DISTINCT soli.*, ctrl.seUsuarioID, ctrl.Observacion as CtrlObservacion, paga.Descriptivo as DescripcionPagaduria, " +
                    " asesor.Descriptivo as DescripcionAsesor,clie.Acierta,clie.AciertaCifin " +
                    "FROM glDocumentoControl ctrl with(nolock) " +
                    "   INNER JOIN glActividadEstado act with(nolock) ON act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    AND act.CerradoInd=@CerradoInd AND act.ActividadFlujoID=@ActividadFlujoID " +
                    "	INNER JOIN glDocumento doc with(nolock) ON ctrl.DocumentoID = doc.DocumentoID " +
                    "	INNER JOIN ccSolicitudDocu soli with(nolock) ON soli.NumeroDoc = ctrl.NumeroDoc " +
                    "	INNER JOIN ccPagaduria paga with(nolock) ON paga.PagaduriaID = soli.PagaduriaID  and paga.EmpresaGrupoID = soli.eg_ccPagaduria" +
                    "	INNER JOIN glActividadPermiso perm with(nolock) ON perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "		AND perm.AreaFuncionalID = ctrl.AreaFuncionalID AND perm.UsuarioID = @UsuarioID " +
                    "	INNER JOIN ccAsesor asesor with(nolock) ON asesor.AsesorID =soli.AsesorID  and asesor.EmpresaGrupoID = soli.eg_ccAsesor " +
                    "   LEFT JOIN ccCliente clie with(nolock) ON clie.ClienteID =soli.ClienteID and clie.EmpresaGrupoID = soli.eg_ccCliente  " +
                    "WHERE ctrl.EmpresaID = @EmpresaID AND doc.ModuloID = @ModuloID AND perm.ActividadFlujoID = @ActividadFlujoID";
                #endregion

                List<DTO_SolicitudAprobacionCartera> result = new List<DTO_SolicitudAprobacionCartera>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_SolicitudAprobacionCartera dto = new DTO_SolicitudAprobacionCartera(dr);
                    dto.Observacion.Value = dr["CtrlObservacion"].ToString();
                    dto.Acierta.Value = dr["Acierta"].ToString();
                    dto.AciertaCifin.Value = dr["AciertaCifin"].ToString();
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetForVerificacion");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito para aprobacion
        /// </summary>
        /// <returns></returns>     
        public List<DTO_SolicitudAprobacionCartera> DAL_ccSolicitudDocu_GetForAprobacion(int documentoID, string actFlujoID, string usuarioID, int _libranza)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@libranza", SqlDbType.Int);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ModuloID"].Value = ModulesPrefix.cc;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommandSel.Parameters["@libranza"].Value = _libranza;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT DISTINCT soli.*, ctrl.seUsuarioID, paga.Descriptivo as DescripcionPagaduria, asesor.Descriptivo as DescripcionAsesor, clie.EmpleadoCodigo  " +
                    "FROM ccSolicitudDocu soli with(nolock) " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = soli.NumeroDoc " +
                    "	INNER JOIN glActividadEstado act with(nolock) ON act.NumeroDoc = ctrl.NumeroDoc  " +
                    "		AND act.CerradoInd=@CerradoInd AND act.ActividadFlujoID=@ActividadFlujoID  " +
                    "	INNER JOIN glDocumento doc with(nolock) ON ctrl.DocumentoID = doc.DocumentoID " +
                    "	INNER JOIN glActividadPermiso perm with(nolock) ON perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "		AND perm.AreaFuncionalID = ctrl.AreaFuncionalID AND perm.UsuarioID = @UsuarioID " +
                    "	INNER JOIN ccPagaduria paga with(nolock) ON paga.PagaduriaID = soli.PagaduriaID " +
                    "	INNER JOIN ccAsesor asesor with(nolock) ON asesor.AsesorID =soli.AsesorID  "    +
                    "   LEFT JOIN ccCliente clie with(nolock) ON clie.ClienteID =soli.ClienteID  " +
                    "WHERE ctrl.EmpresaID = @EmpresaID AND doc.ModuloID = @ModuloID AND perm.ActividadFlujoID = @ActividadFlujoID " +
                    "	AND " +
                    "   ( " +
                    "       soli.IncorporacionPreviaInd = 0 OR (soli.IncorporacionPreviaInd = 1 AND soli.IncorporacionTipo in (2,3) AND soli.NumDocVerificado IS NOT NULL)" +
                    "   ) " +
                    "   AND " +
                    "   ( " +
                    "	    SELECT COUNT(*) FROM ccSolicitudCompraCartera WITH(NOLOCK) " +
                    "	    WHERE ccSolicitudCompraCartera.NumeroDoc = soli.NumeroDoc AND ccSolicitudCompraCartera.IndRecibePazySalvo = 0 " +
                    "   ) = 0 " +
                    "   AND soli.Libranza = CASE WHEN @libranza = ''  then soli.Libranza else @libranza end  ";
                #endregion

                List<DTO_SolicitudAprobacionCartera> result = new List<DTO_SolicitudAprobacionCartera>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_SolicitudAprobacionCartera dto = new DTO_SolicitudAprobacionCartera(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetForAprobacion");
                throw exception;
            }
        }

        /// <summary>
        /// Rertorna los Documentos pertenecientes a una pagaduria
        /// </summary>
        /// <returns></returns>
        public List<DTO_SolicitudAprobacionCartera> DAL_ccSolicitudDocu_GetForPagaduria(string pagaduria)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = pagaduria;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT soliDocu.NumeroDoc,soliDocu.EmpresaID,soliDocu.ApellidoPri,soliDocu.ApellidoSdo, " +
                    "   soliDocu.NombrePri,soliDocu.NombreSdo,soliDocu.Libranza,soliDocu.VlrPrestamo,ctrl.seUsuarioID,  " +
                    "   soliDocu.ClienteID,soliDocu.VlrGiro,soliDocu.VlrCompra,soliDocu.VlrLibranza,soliDocu.LineaCreditoID, " +
                    "   soli.VlrCuota,soli.VlrCupoDisponible,soli.Plazo,  " +
                    "   paga.Descriptivo as DescripcionPagaduria,asesor.Descriptivo as DescripcionAsesor  " +
                    "   usr.UsuarioID, paga.Descriptivo as DescripcionPagaduria,asesor.Descriptivo as DescripcionAsesor  " + // De donde sale el usuario
                    "   paga.Descriptivo as DescripcionPagaduria,asesor.Descriptivo as DescripcionAsesor " +
                    "FROM ccSolicitudDocu  soliDocu WITH(NOLOCK)  " +
                    "   INNER JOIN glDocumentoControl ctrl on ctrl.NumeroDoc = soliDocu.NumeroDoc   " +
                    "   INNER JOIN ccPagaduria paga with(nolock) on paga.PagaduriaID = soliDocu.PagaduriaID  " +
                    "   INNER JOIN ccAsesor asesor with(nolock) on asesor.AsesorID = soliDocu.AsesorID  " +
                    "   INNER JOIN seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "where solidocu.PagaduriaID = @PagaduriaID";

                #endregion

                List<DTO_SolicitudAprobacionCartera> result = new List<DTO_SolicitudAprobacionCartera>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_SolicitudAprobacionCartera dto = new DTO_SolicitudAprobacionCartera(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetForPagaduria");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito para aprobacion
        /// </summary>
        /// <returns></returns>     
        public List<DTO_ccSolicitudDocu> DAL_ccSolicitudDocu_GetByActividad(string actFlujoID, string usuarioID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glActividadPermiso", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ModuloID"].Value = ModulesPrefix.cf;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommandSel.Parameters["@eg_glActividadPermiso"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadPermiso, this.Empresa, egCtrl);
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT DISTINCT soli.*, ctrl.FechaDoc as Fecha, ctrl.Observacion as DescripCtrl " +
                    "FROM ccSolicitudDocu soli with(nolock) " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = soli.NumeroDoc " +
                    "	INNER JOIN glActividadEstado act with(nolock) ON act.NumeroDoc = ctrl.NumeroDoc  " +
                    "		AND act.CerradoInd=@CerradoInd AND act.ActividadFlujoID=@ActividadFlujoID  " +
                    "	INNER JOIN glDocumento doc with(nolock) ON ctrl.DocumentoID = doc.DocumentoID " +
                    "	INNER JOIN glActividadPermiso perm with(nolock) ON perm.AreaFuncionalID = ctrl.AreaFuncionalID " +
                    "	    AND perm.UsuarioID = @UsuarioID AND perm.EmpresaGrupoID = @eg_glActividadPermiso " +
                    "WHERE ctrl.EmpresaID = @EmpresaID AND perm.ActividadFlujoID = @ActividadFlujoID --AND doc.ModuloID = @ModuloID";
                #endregion

                List<DTO_ccSolicitudDocu> result = new List<DTO_ccSolicitudDocu>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDocu dto = new DTO_ccSolicitudDocu(dr);
                    dto.Rechazado.Value = false;
                    dto.Aprobado.Value = false;
                    dto.Observacion.Value = string.Empty;
                    dto.ObservacionRechazo.Value = dr["DescripCtrl"].ToString();
                    string nombre = dto.NombrePri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.NombreSdo.Value))
                        nombre += " " + dto.NombreSdo.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value))
                        nombre += " " + dto.ApellidoPri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoSdo.Value))
                        nombre += " " + dto.ApellidoSdo.Value;
                    dto.Nombre.Value = nombre;
                    dto.FechaLiquida.Value = Convert.ToDateTime(dr["Fecha"]);

                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetByUsuarioActividad");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los creditos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccSolicitudDocu</returns>
        public List<DTO_ccSolicitudDocu> DAL_ccSolicitudDocu_GetByCliente(string cliente)
        {
            try
            {
                List<DTO_ccSolicitudDocu> result = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = 
                    "SELECT cred.*, ctrl.Estado " +
                    "FROM ccSolicitudDocu cred with(nolock) " +
                    "INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                    "WHERE cred.EmpresaID = @EmpresaID and cred.ClienteRadica = @ClienteID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                result = new List<DTO_ccSolicitudDocu>();
                while (dr.Read())
                {
                    DTO_ccSolicitudDocu dto;
                    dto = new DTO_ccSolicitudDocu(dr);
                    dto.Estado.Value = Convert.ToInt16(dr["Estado"]);

                    result.Add(dto);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetByCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="allEmpresas">Indica si trae la infomacion de todas las empresas</param>
        /// <returns></returns>     
        public List<DTO_ccSolicitudDocu> DAL_ccSolicitudDocu_GetForLiquidacion(string actFlujoID, string usuarioID, bool allEmpresas)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = usuarioID;
                #endregion

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT DISTINCT soli.*, ctrl.FechaDoc " +
                    "from ccSolicitudDocu soli with(nolock) " +
                    "	inner join glDocumentoControl ctrl with(nolock) on soli.NumeroDoc = ctrl.NumeroDoc " +
                    "	inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "		and perm.AreaFuncionalID = ctrl.AreaFuncionalID and perm.UsuarioID = @UsuarioID " +
                    "where perm.ActividadFlujoID = @ActividadFlujoID ";
                #endregion
                if (!allEmpresas)
                {
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                    mySqlCommandSel.CommandText += " and soli.EmpresaID=@EmpresaID";
                }

                List<DTO_ccSolicitudDocu> result = new List<DTO_ccSolicitudDocu>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDocu dto;

                    dto = new DTO_ccSolicitudDocu(dr);
                    string nombre = dto.NombrePri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.NombreSdo.Value))
                        nombre += " " + dto.NombreSdo.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value))
                        nombre += " " + dto.ApellidoPri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoSdo.Value))
                        nombre += " " + dto.ApellidoSdo.Value;
                    dto.Nombre.Value = nombre;


                    dto.Rechazado.Value = false;
                    dto.Aprobado.Value = false;
                    dto.FechaLiquida.Value = Convert.ToDateTime(dr["FechaDoc"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudDocu_GetForLiquidacion");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <param name="actFlujoReferenciacion">Actividad de flujo</param>
        /// <param name="pagaduria">Pagaduria por la cual se filtra</param>
        /// <returns>Retorna una lista con los creditos que tienen el indicador de incorporacion previa activado</returns>     
        public List<DTO_ccSolicitudDocu> DAL_ccSolicitudDocu_GetForIncorporacionPrevia(string actFlujoID, string centroPago, DateTime fechaIncorpora)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string query1 = string.Empty;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncorporacionPreviaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaIncorpora", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@IncorporacionPreviaInd"].Value = true;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = true;
                mySqlCommandSel.Parameters["@FechaIncorpora"].Value = fechaIncorpora;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
                #endregion
                #region CommandText

                string cpQuery = string.Empty;
                if(!string.IsNullOrWhiteSpace(centroPago))
                {
                    mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPago;
                    cpQuery = " AND centPag.CentroPagoID = @CentroPagoID ";
                }

                mySqlCommandSel.CommandText =
                    "SELECT DISTINCT soli.*, centPag.Descriptivo AS PagaduriaDesc " +
                    "FROM ccSolicitudDocu soli with(nolock) " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on soli.NumeroDoc = ctrl.NumeroDoc AND ctrl.Estado=@Estado " +
                    "   INNER JOIN ccCentroPagoPAG centPag with(nolock) on soli.CentroPagoID = centPag.CentroPagoID and soli.eg_ccCentroPagoPAG = centPag.EmpresaGrupoID " +
                    "   INNER JOIN ccPagaduria pag with(nolock) on soli.PagaduriaID = pag.PagaduriaID and soli.eg_ccPagaduria = pag.EmpresaGrupoID " +
                    "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "       AND act.CerradoInd = @CerradoInd and act.ActividadFlujoID = @ActividadFlujoID " +
                    "WHERE soli.EmpresaID = @EmpresaID and act.ActividadFlujoID = @ActividadFlujoID " + cpQuery +
                    "   AND soli.IncorporacionPreviaInd = @IncorporacionPreviaInd AND soli.IncorporacionTipo in (2,3) " +
                    "   AND soli.PagaduriaID IN " +
                    "   ( " +
                    "   	select distinct validPag.PagaduriaID FROM " +
                    "   	( " +
                    "           select PagaduriaID " +
                    "           from ccPagaduria with(nolock) " +
                    "           where EmpresaGrupoID = @EmpresaGrupoID and DiaCorte <= DAY(@FechaIncorpora) " +
                    "           UNION " +
                    "           select PagaduriaID " +
                    "           from ccIncorporacionDeta inc with(nolock) " +
                    "               inner join glDocumentoControl ctrl with(nolock) on inc.NumeroDoc = ctrl.NumeroDoc and ctrl.estado = @Estado " +
                    "           where EmpresaID = @EmpresaID and MONTH(ctrl.FechaDoc) = MONTH(@FechaIncorpora) and YEAR(ctrl.FechaDoc) = YEAR(@FechaIncorpora) " +
                    "   	) as validPag " +
                    "   ) ";
                #endregion

                List<DTO_ccSolicitudDocu> result = new List<DTO_ccSolicitudDocu>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDocu dto = new DTO_ccSolicitudDocu(dr);
                    dto.Rechazado.Value = false;
                    dto.Aprobado.Value = false;

                    string nombre = dto.NombrePri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.NombreSdo.Value))
                        nombre += " " + dto.NombreSdo.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value))
                        nombre += " " + dto.ApellidoPri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoSdo.Value))
                        nombre += " " + dto.ApellidoSdo.Value;

                    dto.Nombre.Value = nombre;
                    dto.Otro.Value = dr["PagaduriaDesc"].ToString();
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetForIncorporacionPrevia");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <param name="actFlujoReferenciacion">Actividad de flujo</param>
        /// <param name="pagaduria">Pagaduria por la cual se filtra</param>
        /// <returns>Retorna una lista con los creditos que tienen el indicador de incorporacion previa activado</returns>     
        public List<DTO_ccSolicitudDocu> DAL_ccSolicitudDocu_GetForIncorporacionVerificacion(string actFlujoID, string centroPago, int tipoVerificacion)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@IncorporacionPreviaInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IncorporacionTipo", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPago;
                mySqlCommandSel.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@IncorporacionPreviaInd"].Value = true;
                mySqlCommandSel.Parameters["@IncorporacionTipo"].Value = tipoVerificacion;
                #endregion
                #region CommandText

                string cpQuery = string.Empty;
                if (!string.IsNullOrWhiteSpace(centroPago))
                {
                    mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPago;
                    cpQuery = " AND centPag.CentroPagoID = @CentroPagoID ";
                }

                mySqlCommandSel.CommandText =
                    "SELECT DISTINCT soli.*, centPag.Descriptivo AS PagaduriaDesc " +
                    "FROM ccSolicitudDocu soli with(nolock) " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on soli.NumeroDoc = ctrl.NumeroDoc AND ctrl.Estado=@Estado " +
                    "   INNER JOIN ccCentroPagoPAG centPag with(nolock) on soli.CentroPagoID = centPag.CentroPagoID and soli.eg_ccCentroPagoPAG = centPag.EmpresaGrupoID " +
                    "   INNER JOIN ccPagaduria pag with(nolock) on soli.PagaduriaID = pag.PagaduriaID and soli.eg_ccPagaduria = pag.EmpresaGrupoID " +
                    "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "       AND act.CerradoInd = @CerradoInd and act.ActividadFlujoID = @ActividadFlujoID " +
                    "WHERE soli.EmpresaID = @EmpresaID and act.ActividadFlujoID = @ActividadFlujoID " + cpQuery +
                    "   AND soli.IncorporacionPreviaInd = @IncorporacionPreviaInd AND soli.IncorporacionTipo == @IncorporacionTipo AND soli.NumDocVerificado IS NULL  " + 
                    "   AND soli.NumDocIncorporacion != 0";
                #endregion

                List<DTO_ccSolicitudDocu> result = new List<DTO_ccSolicitudDocu>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDocu dto = new DTO_ccSolicitudDocu(dr);
                    dto.Rechazado.Value = false;
                    dto.Aprobado.Value = false;

                    string nombre = dto.NombrePri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.NombreSdo.Value))
                        nombre += " " + dto.NombreSdo.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value))
                        nombre += " " + dto.ApellidoPri.Value;
                    if (!string.IsNullOrWhiteSpace(dto.ApellidoSdo.Value))
                        nombre += " " + dto.ApellidoSdo.Value;

                    dto.Nombre.Value = nombre;
                    dto.Otro.Value = dr["PagaduriaDesc"].ToString();
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetForIncorporacionPrevia");
                throw exception;
            }
        }
        
        /// <summary>
        /// Trae los creditos de un cliente
        /// </summary>
        /// <param name="cliente">Cliente</param>
        /// <returns>retorna una lista de DTO_ccSolicitudDocu</returns>
        public List<DTO_CodeudorDet> DAL_ccSolicitudDocu_GetInfoCodeudor(List<string> clientes, int? numSolicitud)
        {
            try
            {
                List<DTO_CodeudorDet> result = new List<DTO_CodeudorDet>();;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;


                mySqlCommand.CommandText = " SELECT cl.ClienteID, cl.Descriptivo as ClienteDesc, pr.Descriptivo as ProfesionDes,cl.Telefono, cl.Celular,cl.Correo,cl.ResidenciaDir " +
                                           " FROM  ccCliente cl with(nolock) " +
                                           "      INNER JOIN ccProfesion pr with(nolock) on cl.ProfesionID = pr.ProfesionID and cl.eg_ccProfesion = pr.EmpresaGrupoID " +
                                           " WHERE cl.ClienteID = @ClienteID and cl.EmpresaGrupoID = @EmpresaID";

                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                foreach (var cliente in clientes)
                {
                    mySqlCommand.Parameters["@ClienteID"].Value = cliente;

                    SqlDataReader dr = mySqlCommand.ExecuteReader();
                    if (dr.Read())
                    {
                        DTO_CodeudorDet dto = new DTO_CodeudorDet(dr);
                        dto.NumSolicitud.Value = numSolicitud;
                        result.Add(dto);
                    }
                    dr.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetCodeudor");
                throw exception;
            }
        }

        /// <summary>
        /// Trae solicitud por consecutivoWeb
        /// </summary>
        /// <param name="NumeroDoc">id del registro web</param>
        public DTO_ccSolicitudDocu DAL_ccSolicitudDocu_GetByConsecWeb(int consec)
        {
            try
            {
                DTO_ccSolicitudDocu result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ConsecutivoWEB", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoWEB"].Value = consec;

                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudDocu with(nolock) WHERE ConsecutivoWeb = @ConsecutivoWEB";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccSolicitudDocu(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDocu_GetByConsecWeb");
                throw exception;
            }
        }
        #endregion
    }

}
