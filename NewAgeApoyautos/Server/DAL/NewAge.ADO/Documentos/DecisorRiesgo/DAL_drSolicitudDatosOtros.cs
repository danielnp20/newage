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
    public class DAL_drSolicitudDatosOtros : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_drSolicitudDatosOtros(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosOtros que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosOtros</returns>
        public List<DTO_drSolicitudDatosOtros> DAL_drSolicitudDatosOtros_GetAll()
        {
            try
            {
                List<DTO_drSolicitudDatosOtros> result = new List<DTO_drSolicitudDatosOtros>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosOtros with(nolock) ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_drSolicitudDatosOtros Datos;
                    Datos = new DTO_drSolicitudDatosOtros(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_drSolicitudDatosOtros que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_drSolicitudDatosOtros</returns>
        public DTO_drSolicitudDatosOtros DAL_drSolicitudDatosOtros_GetByNumeroDoc(int numeroDoc, int version)
        {
            try
            {
                DTO_drSolicitudDatosOtros result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("Version", SqlDbType.Int);
                mySqlCommand.Parameters["NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["Version"].Value = version;

                mySqlCommand.CommandText = "SELECT * FROM drSolicitudDatosOtros with(nolock) where NumeroDoc = @NumeroDoc and Version = @Version ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_drSolicitudDatosOtros(dr);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla drSolicitudDatosOtros
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_drSolicitudDatosOtros_Add(DTO_drSolicitudDatosOtros Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO drSolicitudDatosOtros " +
                    "( " +

                     "  NumeroDoc,Version,VlrPoliza,CubriendoInd,AseguradoraID,FinanciaPOLInd,VlrMensualSV,TipoPrenda,PrefijoPrenda,NumeroPrenda,"+
                     "  Registro,CedulaReg,NombreREG,DireccionREG,Alternativa1,Alternativa2,Alternativa3,Alternativa4,Alternativa5,Alternativa6,"+
                     "  Alternativa7,FactorAlt1,FactorAlt2,FactorAlt3,FactorAlt4,FactorAlt5,FactorAlt6,FactorAlt7,MontoAlt1,MontoAlt2,MontoAlt3,"+
                     "  MontoAlt4,MontoAlt5,MontoAlt6,MontoAlt7,GarantiaAlt1,GarantiaAlt2,GarantiaAlt3,GarantiaAlt4,GarantiaAlt5,GarantiaAlt6,"+
                     "  GarantiaAlt7,VlrGtiaEvaluacion,MontoMaximo,EstimadoSeguros,MaxFinanciacionAut,Plazo,EstimadoObl,CuotaFin,CuotaSeg,CuotaTotal,"+
                     "  Estado,Calificacion,Revision,CargoResp,Firma1Ind,Firma2Ind,Firma3Ind,UsuarioResp,UsuarioFirma1,UsuarioFirma2,UsuarioFirma3,FechaFirmaResp,FechaFirma1," +
                     "  FechaFirma2,FechaFirma3,FechaDatacredito,FechaLegalizacion,FechaDesembolso,eg_ccAseguradora,CartaAprobDirInd,CartaAprobDocInd,CartapreAprobInd,CartaNoViableInd,CartaRevocaInd,CartaRatificaInd," +
                     "  CartaAprobDirUsu,CartaAprobDocUsu,CartapreAprobUsu,CartaNoViableUsu,CartaRevocaUsu,CartaRatificaUsu,CartaAprobDirFecha,"+
                     "  CartaAprobDocFecha,CartapreAprobFecha,CartaNoViableFecha,CartaRevocaFecha,CartaRatificaFecha,PerfilUsuario,PerfilFecha,EstActualFactor,"+
                     "  VlrSolicitado,SMMLV,PF_PorIngrPagoCtas,PF_IngrDispApoyosDEU,PF_IngrDispApoyosCON,PF_IngrDispApoyos,PF_VlrMontoSOL,PF_VlrMontoAJU,"+
                     "  PF_VlrMontoINC,PF_VlrMontoFIN,PF_CtaFinanciaSOL,PF_CtaFinanciaAJU,PF_CtaFinanciaINC,PF_CtaFinanciaFIN,PF_CtaSeguroSOL,PF_CtaSeguroAJU,"+
                     "  PF_CtaSeguroINC,PF_CtaSeguroFIN,PF_CtaTotalSOL,PF_CtaTotalAJU,PF_CtaTotalINC,PF_CtaTotalFIN,PF_CtaApoyosDifSOL,PF_CtaApoyosDifAJU,"+
                     "  PF_CtaApoyosDifINC,PF_CtaApoyosDifFIN,PF_IngDispApoyosSOL,PF_IngDispApoyosAJU,PF_IngDispApoyosINC,PF_IngDispApoyosFIN,PF_ReqSopIngrIndDEU,"+
                     "  PF_ReqSopIngrIndCON,PF_IngReqDeu1SOL,PF_IngReqDeu1AJU,PF_IngReqDeu1INC,PF_IngReqDeu1FIN,PF_IngReqDeu2SOL,PF_IngReqDeu2AJU,PF_IngReqDeu2INC,"+
                     "  PF_IngReqDeu2FIN,PF_IngReqDeu3SOL,PF_IngReqDeuAJU,PF_IngReqDeu3INC,PF_IngReqDeu3FIN,PF_IngReqDeuFinSOL,PF_IngReqDeuFinAJU,PF_IngReqDeuFinINC,"+
                     "  PF_IngReqDeuFinFIN,PF_IngReqCon1SOL,PF_IngReqCon1AJU,PF_IngReqCon1INC,PF_IngReqCon1FIN,PF_IngReqCon2SOL,PF_IngReqCon2AJU,PF_IngReqCon2INC,"+
                     "  PF_IngReqCon2FIN,PF_IngReqConFinSOL,PF_IngReqConFinAJU,PF_IngReqConFinINC,PF_IngReqConFinFIN,PF_Cuantia,PF_TasaTablEva1,PF_FactTablEva,"+
                     "  PF_TasaTablEva2,PF_TasaPonderada," +
                     "  PF_PolCubriendoInd,PF_PlazoFinal,PF_VlrMinimoGar,PF_VlrMinimoFirma2,PF_VlrMinimoFirma3,PF_CtaFinanciacion,PF_CtaSeguros,PF_PorEstimado,Verificacion1,Verificacion2,Verificacion3,AN_Resultado1SOL," +
                     "  AN_Resultado1CA1,AN_Resultado1CA2,AN_Resultado1CA3,AN_Resultado1AJ1,AN_Resultado1AJ2,AN_Resultado1V11,AN_Resultado1V12,AN_Resultado1V21," +
                     "  AN_Resultado1V22,AN_Resultado1V31,AN_Resultado1V32,AN_Resultado2SOL,AN_Resultado2CA1,AN_Resultado2CA2,AN_Resultado2CA3,AN_Resultado2AJ1," +
                     "  AN_Resultado2AJ2,AN_Resultado2V11,AN_Resultado2V12,AN_Resultado2V21,AN_Resultado2V22,AN_Resultado2V31,AN_Resultado2V32,AN_VlrVenta," +
                     "  AN_VlrFasecolda,AN_VlrCtaEvaSOL,AN_VlrCtaEvaCA1,AN_VlrCtaEvaCA2,AN_VlrCtaEvaCA3,AN_VlrCtaEvaAJ1,AN_VlrCtaEvaAJ2,AN_VlrCtaEvaV11," +
                     "  AN_VlrCtaEvaV12,AN_VlrCtaEvaV21,AN_VlrCtaEvaV22,AN_VlrCtaEvaV31,AN_VlrCtaEvaV32,AN_VlrMinLim,AN_VlrMaxLim,AN_VlrSolicitado," +
                     "  AN_VlrAlternCA1,AN_VlrAlternCA2,AN_VlrAlternCA3,AN_VlrAlternAJ1,AN_VlrAlternAJ2,AN_VlrAlternV11,AN_VlrAlternV12,AN_VlrAlternV21," +
                     "  AN_VlrAlternV22,AN_VlrAlternV31,AN_VlrAlternV32,AN_CtaInicial,AN_VlrIncremSOL,AN_VlrIncremCA1,AN_VlrIncremCA2,AN_VlrIncremCA3," +
                     "  AN_VlrIncremAJ1,AN_VlrIncremAJ2,AN_VlrIncremV11,AN_VlrIncremV12,AN_VlrIncremV21,AN_VlrIncremV22,AN_VlrIncremV31,AN_VlrIncremV32," +
                     "  AN_CtaIniAjuCA1,AN_CtaIniAjuCA2,AN_CtaIniAjuCA3,AN_CtaIniAjuAJ1,AN_CtaIniAjuAJ2,AN_CtaIniAjuV11,AN_CtaIniAjuV12,AN_CtaIniAjuV21," +
                     "  AN_CtaIniAjuV22,AN_CtaIniAjuV31,AN_CtaIniAjuV32,AN_porFinSoli,AN_porFinMaxSOL,AN_porFinMaxCA1,AN_porFinMaxCA2,AN_porFinMaxCA3," +
                     "  AN_porFinMaxAJ1,AN_porFinMaxAJ2,AN_porFinMaxV11,AN_porFinMaxV12,AN_porFinMaxV21,AN_porFinMaxV22,AN_porFinMaxV31,AN_porFinMaxV32," +
                     "  AN_porFinAltCA1,AN_porFinAltCA2,AN_porFinAltCA3,AN_porFinAltAJ1,AN_porFinAltAJ2,AN_porFinAltV11,AN_porFinAltV12,AN_porFinAltV21," +
                     "  AN_porFinAltV22,AN_porFinAltV31,AN_porFinAltV32,AN_CumParGarAJ1,AN_CumParGarAJ2,AN_CumParGarV11,AN_CumParGarV12,AN_CumParGarV21," +
                     "  AN_CumParGarV22,AN_CumParGarV31,AN_CumParGarV32,AN_CapPagoSOL,AN_CapPagoCA1,AN_CapPagoCA2,AN_CapPagoCA3,AN_CapPagoAJ1,AN_CapPagoAJ2," +
                     "  AN_CapPagoV11,AN_CapPagoV12,AN_CapPagoV21,AN_CapPagoV22,AN_CapPagoV31,AN_CapPagoV32,AN_PuedeIncCICA1,AN_PuedeIncCICA2,AN_PuedeIncCICA3," +
                     "  AN_PuedeIncCIAJ1,AN_PuedeIncCIAJ2,AN_PuedeIncCIV11,AN_PuedeIncCIV12,AN_PuedeIncCIV21,AN_PuedeIncCIV22,AN_PuedeIncCIV31,AN_PuedeIncCIV32," +
                     "  AN_CumPorMaxSOL,AN_CumPorMaxCA1,AN_CumPorMaxCA2,AN_CumPorMaxCA3,AN_CumPorMaxAJ1,AN_CumPorMaxAJ2,AN_CumPorMaxV11,AN_CumPorMaxV12," +
                     "  AN_CumPorMaxV21,AN_CumPorMaxV22,AN_CumPorMaxV31,AN_CumPorMaxV32,AN_CumMtoMinCA1,AN_CumMtoMinCA2,AN_CumMtoMinCA3,AN_CumMtoMinAJ1," +
                     "  AN_CumMtoMinAJ2,AN_CumMtoMinV11,AN_CumMtoMinV12,AN_CumMtoMinV21,AN_CumMtoMinV22,AN_CumMtoMinV31,AN_CumMtoMinV32,AN_CumOtroSOL," +
                     "  AN_CumOtroCA1,AN_CumOtroCA2,AN_CumOtroCA3,AN_CumOtroAJ1,AN_CumOtroAJ2,AN_CumOtroV11,AN_CumOtroV12,AN_CumOtroV21,AN_CumOtroV22," +
                     "  AN_CumOtroV31,AN_CumOtroV32,AN_VlrAutorizaSOL,AN_VlrAutorizaCA1,AN_VlrAutorizaCA2,AN_VlrAutorizaCA3,AN_VlrAutorizaAJ1,AN_VlrAutorizaAJ2," +
                     "  AN_VlrAutorizaV11,AN_VlrAutorizaV12,AN_VlrAutorizaV21,AN_VlrAutorizaV22,AN_VlrAutorizaV31,AN_VlrAutorizaV32,AN_VlrAutFinSOL," +
                     "  AN_VlrAutFinCA2,AN_VlrAutFinAJ1,AN_VlrAutFinV12,AN_VlrAutFinV21,AN_VlrAutFinV31,AN_AltNoComAJ1,AN_AltNoComAJ2,AN_AltNoComV12," +
                     "  AN_AltNoComV21,AN_AltNoComV22,AN_AltNoComV31,AN_AltNoComV32," +
                     "  Alternativa8,FactorAlt8,MontoAlt8,GarantiaAlt8,IngrMinSopDeu1,IngrMinSopDeu2,IngrMinSopDeu3,IngrMinSopDeu4,IngrMinSopDeu5," +
                     "  IngrMinSopDeu6,IngrMinSopDeu7,IngrMinSopDeu8,IngrMinSopCon1,IngrMinSopCon2,IngrMinSopCon3,IngrMinSopCon4,IngrMinSopCon5," +
                     "  IngrMinSopCon6,IngrMinSopCon7,IngrMinSopCon8,FechaFirmaDocumento,AccionSolicitud," +
                     "  Observacion,PF_Plazo,PF_PorMaxEstadoActual,porMaximo,VlrGarantia,"+
                     "  AccionSolicitud1,AccionSolicitud2,AccionSolicitud3,PF_PlazoFinal1,PF_PlazoFinal2,PF_PlazoFinal3,PF_VlrMontoFirma1,PF_VlrMontoFirma2," +
                     "  PF_VlrMontoFirma3,PF_TasaPerfilOBL,PF_TasaFirma3OBL,PF_VlrGarantiaPerfil,PF_VlrGarantiaFirma1,PF_VlrGarantiaFirma2,PF_VlrGarantiaFirma3," +
                     "  PF_PorMaxFirma1,PF_PorMaxFirma2,PF_PorMaxFirma3" +
                     ") " +
                    "VALUES " +
                    "( " +
                    "  @NumeroDoc,@Version,@VlrPoliza,@CubriendoInd,@AseguradoraID,@FinanciaPOLInd,@VlrMensualSV,@TipoPrenda,@PrefijoPrenda,@NumeroPrenda," +
                    "  @Registro,@CedulaReg,@NombreREG,@DireccionREG,@Alternativa1,@Alternativa2,@Alternativa3,@Alternativa4,@Alternativa5,@Alternativa6," +
                    "  @Alternativa7,@FactorAlt1,@FactorAlt2,@FactorAlt3,@FactorAlt4,@FactorAlt5,@FactorAlt6,@FactorAlt7,@MontoAlt1,@MontoAlt2,@MontoAlt3," +
                    "  @MontoAlt4,@MontoAlt5,@MontoAlt6,@MontoAlt7,@GarantiaAlt1,@GarantiaAlt2,@GarantiaAlt3,@GarantiaAlt4,@GarantiaAlt5,@GarantiaAlt6," +
                    "  @GarantiaAlt7,@VlrGtiaEvaluacion,@MontoMaximo,@EstimadoSeguros,@MaxFinanciacionAut,@Plazo,@EstimadoObl,@CuotaFin,@CuotaSeg,@CuotaTotal," +
                    "  @Estado,@Calificacion,@Revision,@CargoResp,@Firma1Ind,@Firma2Ind,@Firma3Ind,@UsuarioResp,@UsuarioFirma1,@UsuarioFirma2,@UsuarioFirma3,@FechaFirmaResp,@FechaFirma1," +
                    "  @FechaFirma2,@FechaFirma3,@FechaDatacredito,@FechaLegalizacion,@FechaDesembolso,@eg_ccAseguradora,@CartaAprobDirInd,@CartaAprobDocInd,@CartapreAprobInd,@CartaNoViableInd,@CartaRevocaInd,@CartaRatificaInd," +
                    "  @CartaAprobDirUsu,@CartaAprobDocUsu,@CartapreAprobUsu,@CartaNoViableUsu,@CartaRevocaUsu,@CartaRatificaUsu,@CartaAprobDirFecha," +
                    "  @CartaAprobDocFecha,@CartapreAprobFecha,@CartaNoViableFecha,@CartaRevocaFecha,@CartaRatificaFecha,@PerfilUsuario,@PerfilFecha,@EstActualFactor," +
                    "  @VlrSolicitado,@SMMLV,@PF_PorIngrPagoCtas,@PF_IngrDispApoyosDEU,@PF_IngrDispApoyosCON,@PF_IngrDispApoyos,@PF_VlrMontoSOL,@PF_VlrMontoAJU," +
                    "  @PF_VlrMontoINC,@PF_VlrMontoFIN,@PF_CtaFinanciaSOL,@PF_CtaFinanciaAJU,@PF_CtaFinanciaINC,@PF_CtaFinanciaFIN,@PF_CtaSeguroSOL,@PF_CtaSeguroAJU," +
                    "  @PF_CtaSeguroINC,@PF_CtaSeguroFIN,@PF_CtaTotalSOL,@PF_CtaTotalAJU,@PF_CtaTotalINC,@PF_CtaTotalFIN,@PF_CtaApoyosDifSOL,@PF_CtaApoyosDifAJU," +
                    "  @PF_CtaApoyosDifINC,@PF_CtaApoyosDifFIN,@PF_IngDispApoyosSOL,@PF_IngDispApoyosAJU,@PF_IngDispApoyosINC,@PF_IngDispApoyosFIN,@PF_ReqSopIngrIndDEU," +
                    "  @PF_ReqSopIngrIndCON,@PF_IngReqDeu1SOL,@PF_IngReqDeu1AJU,@PF_IngReqDeu1INC,@PF_IngReqDeu1FIN,@PF_IngReqDeu2SOL,@PF_IngReqDeu2AJU,@PF_IngReqDeu2INC," +
                    "  @PF_IngReqDeu2FIN,@PF_IngReqDeu3SOL,@PF_IngReqDeuAJU,@PF_IngReqDeu3INC,@PF_IngReqDeu3FIN,@PF_IngReqDeuFinSOL,@PF_IngReqDeuFinAJU,@PF_IngReqDeuFinINC," +
                    "  @PF_IngReqDeuFinFIN,@PF_IngReqCon1SOL,@PF_IngReqCon1AJU,@PF_IngReqCon1INC,@PF_IngReqCon1FIN,@PF_IngReqCon2SOL,@PF_IngReqCon2AJU,@PF_IngReqCon2INC," +
                    "  @PF_IngReqCon2FIN,@PF_IngReqConFinSOL,@PF_IngReqConFinAJU,@PF_IngReqConFinINC,@PF_IngReqConFinFIN,@PF_Cuantia,@PF_TasaTablEva1,@PF_FactTablEva," +
                    "  @PF_TasaTablEva2,@PF_TasaPonderada," +
                    "  @PF_PolCubriendoInd,@PF_PlazoFinal,@PF_VlrMinimoGar,@PF_VlrMinimoFirma2,@PF_VlrMinimoFirma3,@PF_CtaFinanciacion,@PF_CtaSeguros,@PF_PorEstimado,@Verificacion1,@Verificacion2,@Verificacion3,@AN_Resultado1SOL," +
                    "  @AN_Resultado1CA1,@AN_Resultado1CA2,@AN_Resultado1CA3,@AN_Resultado1AJ1,@AN_Resultado1AJ2,@AN_Resultado1V11,@AN_Resultado1V12,@AN_Resultado1V21," +
                    "  @AN_Resultado1V22,@AN_Resultado1V31,@AN_Resultado1V32,@AN_Resultado2SOL,@AN_Resultado2CA1,@AN_Resultado2CA2,@AN_Resultado2CA3,@AN_Resultado2AJ1," +
                    "  @AN_Resultado2AJ2,@AN_Resultado2V11,@AN_Resultado2V12,@AN_Resultado2V21,@AN_Resultado2V22,@AN_Resultado2V31,@AN_Resultado2V32,@AN_VlrVenta," +
                    "  @AN_VlrFasecolda,@AN_VlrCtaEvaSOL,@AN_VlrCtaEvaCA1,@AN_VlrCtaEvaCA2,@AN_VlrCtaEvaCA3,@AN_VlrCtaEvaAJ1,@AN_VlrCtaEvaAJ2,@AN_VlrCtaEvaV11," +
                    "  @AN_VlrCtaEvaV12,@AN_VlrCtaEvaV21,@AN_VlrCtaEvaV22,@AN_VlrCtaEvaV31,@AN_VlrCtaEvaV32,@AN_VlrMinLim,@AN_VlrMaxLim,@AN_VlrSolicitado," +
                    "  @AN_VlrAlternCA1,@AN_VlrAlternCA2,@AN_VlrAlternCA3,@AN_VlrAlternAJ1,@AN_VlrAlternAJ2,@AN_VlrAlternV11,@AN_VlrAlternV12,@AN_VlrAlternV21," +
                    "  @AN_VlrAlternV22,@AN_VlrAlternV31,@AN_VlrAlternV32,@AN_CtaInicial,@AN_VlrIncremSOL,@AN_VlrIncremCA1,@AN_VlrIncremCA2,@AN_VlrIncremCA3," +
                    "  @AN_VlrIncremAJ1,@AN_VlrIncremAJ2,@AN_VlrIncremV11,@AN_VlrIncremV12,@AN_VlrIncremV21,@AN_VlrIncremV22,@AN_VlrIncremV31,@AN_VlrIncremV32," +
                    "  @AN_CtaIniAjuCA1,@AN_CtaIniAjuCA2,@AN_CtaIniAjuCA3,@AN_CtaIniAjuAJ1,@AN_CtaIniAjuAJ2,@AN_CtaIniAjuV11,@AN_CtaIniAjuV12,@AN_CtaIniAjuV21," +
                    "  @AN_CtaIniAjuV22,@AN_CtaIniAjuV31,@AN_CtaIniAjuV32,@AN_porFinSoli,@AN_porFinMaxSOL,@AN_porFinMaxCA1,@AN_porFinMaxCA2,@AN_porFinMaxCA3," +
                    "  @AN_porFinMaxAJ1,@AN_porFinMaxAJ2,@AN_porFinMaxV11,@AN_porFinMaxV12,@AN_porFinMaxV21,@AN_porFinMaxV22,@AN_porFinMaxV31,@AN_porFinMaxV32," +
                    "  @AN_porFinAltCA1,@AN_porFinAltCA2,@AN_porFinAltCA3,@AN_porFinAltAJ1,@AN_porFinAltAJ2,@AN_porFinAltV11,@AN_porFinAltV12,@AN_porFinAltV21," +
                    "  @AN_porFinAltV22,@AN_porFinAltV31,@AN_porFinAltV32,@AN_CumParGarAJ1,@AN_CumParGarAJ2,@AN_CumParGarV11,@AN_CumParGarV12,@AN_CumParGarV21," +
                    "  @AN_CumParGarV22,@AN_CumParGarV31,@AN_CumParGarV32,@AN_CapPagoSOL,@AN_CapPagoCA1,@AN_CapPagoCA2,@AN_CapPagoCA3,@AN_CapPagoAJ1,@AN_CapPagoAJ2," +
                    "  @AN_CapPagoV11,@AN_CapPagoV12,@AN_CapPagoV21,@AN_CapPagoV22,@AN_CapPagoV31,@AN_CapPagoV32,@AN_PuedeIncCICA1,@AN_PuedeIncCICA2,@AN_PuedeIncCICA3," +
                    "  @AN_PuedeIncCIAJ1,@AN_PuedeIncCIAJ2,@AN_PuedeIncCIV11,@AN_PuedeIncCIV12,@AN_PuedeIncCIV21,@AN_PuedeIncCIV22,@AN_PuedeIncCIV31,@AN_PuedeIncCIV32," +
                    "  @AN_CumPorMaxSOL,@AN_CumPorMaxCA1,@AN_CumPorMaxCA2,@AN_CumPorMaxCA3,@AN_CumPorMaxAJ1,@AN_CumPorMaxAJ2,@AN_CumPorMaxV11,@AN_CumPorMaxV12," +
                    "  @AN_CumPorMaxV21,@AN_CumPorMaxV22,@AN_CumPorMaxV31,@AN_CumPorMaxV32,@AN_CumMtoMinCA1,@AN_CumMtoMinCA2,@AN_CumMtoMinCA3,@AN_CumMtoMinAJ1," +
                    "  @AN_CumMtoMinAJ2,@AN_CumMtoMinV11,@AN_CumMtoMinV12,@AN_CumMtoMinV21,@AN_CumMtoMinV22,@AN_CumMtoMinV31,@AN_CumMtoMinV32,@AN_CumOtroSOL," +
                    "  @AN_CumOtroCA1,@AN_CumOtroCA2,@AN_CumOtroCA3,@AN_CumOtroAJ1,@AN_CumOtroAJ2,@AN_CumOtroV11,@AN_CumOtroV12,@AN_CumOtroV21,@AN_CumOtroV22," +
                    "  @AN_CumOtroV31,@AN_CumOtroV32,@AN_VlrAutorizaSOL,@AN_VlrAutorizaCA1,@AN_VlrAutorizaCA2,@AN_VlrAutorizaCA3,@AN_VlrAutorizaAJ1,@AN_VlrAutorizaAJ2," +
                    "  @AN_VlrAutorizaV11,@AN_VlrAutorizaV12,@AN_VlrAutorizaV21,@AN_VlrAutorizaV22,@AN_VlrAutorizaV31,@AN_VlrAutorizaV32,@AN_VlrAutFinSOL," +
                    "  @AN_VlrAutFinCA2,@AN_VlrAutFinAJ1,@AN_VlrAutFinV12,@AN_VlrAutFinV21,@AN_VlrAutFinV31,@AN_AltNoComAJ1,@AN_AltNoComAJ2,@AN_AltNoComV12," +
                    "  @AN_AltNoComV21,@AN_AltNoComV22,@AN_AltNoComV31,@AN_AltNoComV32," +
                    "  @Alternativa8,@FactorAlt8,@MontoAlt8,@GarantiaAlt8,@IngrMinSopDeu1,@IngrMinSopDeu2,@IngrMinSopDeu3,@IngrMinSopDeu4,@IngrMinSopDeu5," +
                    "  @IngrMinSopDeu6,@IngrMinSopDeu7,@IngrMinSopDeu8,@IngrMinSopCon1,@IngrMinSopCon2,@IngrMinSopCon3,@IngrMinSopCon4,@IngrMinSopCon5," +
                    "  @IngrMinSopCon6,@IngrMinSopCon7,@IngrMinSopCon8,@FechaFirmaDocumento,@AccionSolicitud," +
                    "  @Observacion,@PF_Plazo,@PF_PorMaxEstadoActual,@porMaximo,@VlrGarantia," +
                    "  @AccionSolicitud1,@AccionSolicitud2,@AccionSolicitud3,@PF_PlazoFinal1,@PF_PlazoFinal2,@PF_PlazoFinal3,@PF_VlrMontoFirma1,@PF_VlrMontoFirma2," +
                    "  @PF_VlrMontoFirma3,@PF_TasaPerfilOBL,@PF_TasaFirma3OBL,@PF_VlrGarantiaPerfil,@PF_VlrGarantiaFirma1,@PF_VlrGarantiaFirma2,@PF_VlrGarantiaFirma3," +
                    "  @PF_PorMaxFirma1,@PF_PorMaxFirma2,@PF_PorMaxFirma3" +
                    ") SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de comandos                
                mySqlCommandSel.Parameters.Add("@NumeroDoc",SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version",SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrPoliza",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CubriendoInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AseguradoraID",SqlDbType.Char,UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@FinanciaPOLInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrMensualSV",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoPrenda",SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PrefijoPrenda",SqlDbType.VarChar,5);
                mySqlCommandSel.Parameters.Add("@NumeroPrenda",SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Registro",SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CedulaReg",SqlDbType.Char,UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombreREG",SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@DireccionREG",SqlDbType.VarChar,100);
                mySqlCommandSel.Parameters.Add("@Alternativa1",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@Alternativa2",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@Alternativa3",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@Alternativa4",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@Alternativa5",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@Alternativa6",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@Alternativa7",SqlDbType.VarChar,15);
                mySqlCommandSel.Parameters.Add("@FactorAlt1",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt2",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt3",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt4",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt5",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt6",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt7",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt1",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt2",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt3",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt4",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt5",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt6",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt7",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt1",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt2",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt3",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt4",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt5",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt6",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt7",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGtiaEvaluacion",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoMaximo",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EstimadoSeguros",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MaxFinanciacionAut",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Plazo",SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstimadoObl",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaFin",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaSeg",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaTotal",SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Estado",SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@Calificacion",SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@Revision",SqlDbType.VarChar,50);
                mySqlCommandSel.Parameters.Add("@CargoResp",SqlDbType.VarChar,30);
                mySqlCommandSel.Parameters.Add("@Firma1Ind",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Firma2Ind",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Firma3Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@UsuarioResp",SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioFirma1",SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioFirma2",SqlDbType.Char,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioFirma3", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaFirmaResp",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFirma1",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFirma2",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFirma3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaDatacredito",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaLegalizacion",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaDesembolso", SqlDbType.SmallDateTime);


                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora",SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo",SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@CartaAprobDirInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaAprobDocInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartapreAprobInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaNoViableInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaRevocaInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaRatificaInd",SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaAprobDirUsu",SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaAprobDocUsu",SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartapreAprobUsu",SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaNoViableUsu",SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaRevocaUsu",SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaRatificaUsu",SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaAprobDirFecha",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaAprobDocFecha",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartapreAprobFecha",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaNoViableFecha",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaRevocaFecha",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaRatificaFecha",SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@PerfilUsuario", SqlDbType.VarChar,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PerfilFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EstActualFactor", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SMMLV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorIngrPagoCtas", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyosDEU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyosCON", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ReqSopIngrIndDEU", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_ReqSopIngrIndCON", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu3SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu3INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu3FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Cuantia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaTablEva1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FactTablEva", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaTablEva2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaPonderada", SqlDbType.Decimal);


                mySqlCommandSel.Parameters.Add("@PF_PolCubriendoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_VlrMinimoGar", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMinimoFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMinimoFirma3", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguros", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorEstimado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Verificacion1", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Verificacion2", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Verificacion3", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1CA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1CA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1CA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1AJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1AJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2CA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2CA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2CA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2AJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2AJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrVenta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrFasecolda", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrMinLim", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrMaxLim", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaInicial", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinSoli", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoSOL", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoCA1", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoCA2", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoCA3", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoAJ1", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoAJ2", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV11", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV12", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV21", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV22", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV31", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV32", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCICA1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCICA2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCICA3", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxSOL", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxCA1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxCA2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxCA3", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinCA1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinCA2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinCA3", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroSOL", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroCA1", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroCA2", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroCA3", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroAJ1", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroAJ2", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV11", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV12", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV21", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV22", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV31", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV32", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComAJ1", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComAJ2", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV12", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV21", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV22", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV31", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV32", SqlDbType.VarChar, 2);

                mySqlCommandSel.Parameters.Add("@Alternativa8", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@FactorAlt8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaFirmaDocumento", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@AccionSolicitud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char,UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@PF_Plazo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxEstadoActual", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@porMaximo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGarantia", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@AccionSolicitud1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AccionSolicitud2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AccionSolicitud3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFirma1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFirma3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaPerfilOBL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaFirma3OBL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFirma1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFirma3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaPerfil", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaFirma1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaFirma3", SqlDbType.Decimal);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = Datos.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@CubriendoInd"].Value = Datos.CubriendoInd.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = Datos.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@FinanciaPOLInd"].Value = Datos.FinanciaPOLInd.Value;
                mySqlCommandSel.Parameters["@VlrMensualSV"].Value = Datos.VlrMensualSV.Value;
                mySqlCommandSel.Parameters["@TipoPrenda"].Value = Datos.TipoPrenda.Value;
                mySqlCommandSel.Parameters["@PrefijoPrenda"].Value = Datos.PrefijoPrenda.Value;
                mySqlCommandSel.Parameters["@NumeroPrenda"].Value = Datos.NumeroPrenda.Value;
                mySqlCommandSel.Parameters["@Registro"].Value = Datos.Registro.Value;
                mySqlCommandSel.Parameters["@CedulaReg"].Value = Datos.CedulaReg.Value;
                mySqlCommandSel.Parameters["@NombreREG"].Value = Datos.NombreREG.Value;
                mySqlCommandSel.Parameters["@DireccionREG"].Value = Datos.DireccionREG.Value;
                mySqlCommandSel.Parameters["@Alternativa1"].Value = Datos.Alternativa1.Value;
                mySqlCommandSel.Parameters["@Alternativa2"].Value = Datos.Alternativa2.Value;
                mySqlCommandSel.Parameters["@Alternativa3"].Value = Datos.Alternativa3.Value;
                mySqlCommandSel.Parameters["@Alternativa4"].Value = Datos.Alternativa4.Value;
                mySqlCommandSel.Parameters["@Alternativa5"].Value = Datos.Alternativa5.Value;
                mySqlCommandSel.Parameters["@Alternativa6"].Value = Datos.Alternativa6.Value;
                mySqlCommandSel.Parameters["@Alternativa7"].Value = Datos.Alternativa7.Value;
                mySqlCommandSel.Parameters["@FactorAlt1"].Value = Datos.FactorAlt1.Value;
                mySqlCommandSel.Parameters["@FactorAlt2"].Value = Datos.FactorAlt2.Value;
                mySqlCommandSel.Parameters["@FactorAlt3"].Value = Datos.FactorAlt3.Value;
                mySqlCommandSel.Parameters["@FactorAlt4"].Value = Datos.FactorAlt4.Value;
                mySqlCommandSel.Parameters["@FactorAlt5"].Value = Datos.FactorAlt5.Value;
                mySqlCommandSel.Parameters["@FactorAlt6"].Value = Datos.FactorAlt6.Value;
                mySqlCommandSel.Parameters["@FactorAlt7"].Value = Datos.FactorAlt7.Value;
                mySqlCommandSel.Parameters["@MontoAlt1"].Value = Datos.MontoAlt1.Value;
                mySqlCommandSel.Parameters["@MontoAlt2"].Value = Datos.MontoAlt2.Value;
                mySqlCommandSel.Parameters["@MontoAlt3"].Value = Datos.MontoAlt3.Value;
                mySqlCommandSel.Parameters["@MontoAlt4"].Value = Datos.MontoAlt4.Value;
                mySqlCommandSel.Parameters["@MontoAlt5"].Value = Datos.MontoAlt5.Value;
                mySqlCommandSel.Parameters["@MontoAlt6"].Value = Datos.MontoAlt6.Value;
                mySqlCommandSel.Parameters["@MontoAlt7"].Value = Datos.MontoAlt7.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt1"].Value = Datos.GarantiaAlt1.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt2"].Value = Datos.GarantiaAlt2.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt3"].Value = Datos.GarantiaAlt3.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt4"].Value = Datos.GarantiaAlt4.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt5"].Value = Datos.GarantiaAlt5.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt6"].Value = Datos.GarantiaAlt6.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt7"].Value = Datos.GarantiaAlt7.Value;
                mySqlCommandSel.Parameters["@VlrGtiaEvaluacion"].Value = Datos.VlrGtiaEvaluacion.Value;
                mySqlCommandSel.Parameters["@MontoMaximo"].Value = Datos.MontoMaximo.Value;
                mySqlCommandSel.Parameters["@EstimadoSeguros"].Value = Datos.EstimadoSeguros.Value;
                mySqlCommandSel.Parameters["@MaxFinanciacionAut"].Value = Datos.MaxFinanciacionAut.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = Datos.Plazo.Value;

                mySqlCommandSel.Parameters["@EstimadoObl"].Value = Datos.EstimadoObl.Value;
                mySqlCommandSel.Parameters["@CuotaFin"].Value = Datos.CuotaFin.Value;
                mySqlCommandSel.Parameters["@CuotaSeg"].Value = Datos.CuotaSeg.Value;
                mySqlCommandSel.Parameters["@CuotaTotal"].Value = Datos.CuotaTotal.Value;
                mySqlCommandSel.Parameters["@Estado"].Value = Datos.Estado.Value;
                mySqlCommandSel.Parameters["@Calificacion"].Value = Datos.Calificacion.Value;
                mySqlCommandSel.Parameters["@Revision"].Value = Datos.Revision.Value;
                mySqlCommandSel.Parameters["@CargoResp"].Value = Datos.CargoResp.Value;
                mySqlCommandSel.Parameters["@Firma1Ind"].Value = Datos.Firma1Ind.Value;
                mySqlCommandSel.Parameters["@Firma2Ind"].Value = Datos.Firma2Ind.Value;
                mySqlCommandSel.Parameters["@Firma3Ind"].Value = Datos.Firma3Ind.Value;
                mySqlCommandSel.Parameters["@UsuarioResp"].Value = Datos.UsuarioResp.Value;
                mySqlCommandSel.Parameters["@UsuarioFirma1"].Value = Datos.UsuarioFirma1.Value;
                mySqlCommandSel.Parameters["@UsuarioFirma2"].Value = Datos.UsuarioFirma2.Value;
                mySqlCommandSel.Parameters["@UsuarioFirma3"].Value = Datos.UsuarioFirma3.Value;
                mySqlCommandSel.Parameters["@FechaFirmaResp"].Value = Datos.FechaFirmaResp.Value;
                mySqlCommandSel.Parameters["@FechaFirma1"].Value = Datos.FechaFirma1.Value;
                mySqlCommandSel.Parameters["@FechaFirma2"].Value = Datos.FechaFirma2.Value;
                mySqlCommandSel.Parameters["@FechaFirma3"].Value = Datos.FechaFirma3.Value;
                mySqlCommandSel.Parameters["@FechaDatacredito"].Value = Datos.FechaDatacredito.Value;
                mySqlCommandSel.Parameters["@FechaLegalizacion"].Value = Datos.FechaLegalizacion.Value;
                mySqlCommandSel.Parameters["@FechaDesembolso"].Value = Datos.FechaDesembolso.Value;
                
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;

                mySqlCommandSel.Parameters["@CartaAprobDirInd"].Value = Datos.CartaAprobDirInd.Value;
                mySqlCommandSel.Parameters["@CartaAprobDocInd"].Value = Datos.CartaAprobDocInd.Value;
                mySqlCommandSel.Parameters["@CartapreAprobInd"].Value = Datos.CartapreAprobInd.Value;
                mySqlCommandSel.Parameters["@CartaNoViableInd"].Value = Datos.CartaNoViableInd.Value;
                mySqlCommandSel.Parameters["@CartaRevocaInd"].Value = Datos.CartaRevocaInd.Value;
                mySqlCommandSel.Parameters["@CartaRatificaInd"].Value = Datos.CartaRatificaInd.Value;
                mySqlCommandSel.Parameters["@CartaAprobDirUsu"].Value = Datos.CartaAprobDirUsu.Value;
                mySqlCommandSel.Parameters["@CartaAprobDocUsu"].Value = Datos.CartaAprobDocUsu.Value;
                mySqlCommandSel.Parameters["@CartapreAprobUsu"].Value = Datos.CartapreAprobUsu.Value;
                mySqlCommandSel.Parameters["@CartaNoViableUsu"].Value = Datos.CartaNoViableUsu.Value;
                mySqlCommandSel.Parameters["@CartaRevocaUsu"].Value = Datos.CartaRevocaUsu.Value;
                mySqlCommandSel.Parameters["@CartaRatificaUsu"].Value = Datos.CartaRatificaUsu.Value;
                mySqlCommandSel.Parameters["@CartaAprobDirFecha"].Value = Datos.CartaAprobDirFecha.Value;
                mySqlCommandSel.Parameters["@CartaAprobDocFecha"].Value = Datos.CartaAprobDocFecha.Value;
                mySqlCommandSel.Parameters["@CartapreAprobFecha"].Value = Datos.CartapreAprobFecha.Value;
                mySqlCommandSel.Parameters["@CartaNoViableFecha"].Value = Datos.CartaNoViableFecha.Value;
                mySqlCommandSel.Parameters["@CartaRevocaFecha"].Value = Datos.CartaRevocaFecha.Value;
                mySqlCommandSel.Parameters["@CartaRatificaFecha"].Value = Datos.CartaRatificaFecha.Value;
                mySqlCommandSel.Parameters["@PerfilUsuario"].Value = Datos.PerfilUsuario.Value;
                mySqlCommandSel.Parameters["@PerfilFecha"].Value = Datos.PerfilFecha.Value;
                mySqlCommandSel.Parameters["@EstActualFactor"].Value = Datos.EstActualFactor.Value;

                mySqlCommandSel.Parameters["@VlrSolicitado"].Value = Datos.VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@SMMLV"].Value = Datos.SMMLV.Value;
                mySqlCommandSel.Parameters["@PF_PorIngrPagoCtas"].Value = Datos.PF_PorIngrPagoCtas.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyosDEU"].Value = Datos.PF_IngrDispApoyosDEU.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyosCON"].Value = Datos.PF_IngrDispApoyosCON.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyos"].Value = Datos.PF_IngrDispApoyos.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoSOL"].Value = Datos.PF_VlrMontoSOL.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoAJU"].Value = Datos.PF_VlrMontoAJU.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoINC"].Value = Datos.PF_VlrMontoINC.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFIN"].Value = Datos.PF_VlrMontoFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaSOL"].Value = Datos.PF_CtaFinanciaSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaAJU"].Value = Datos.PF_CtaFinanciaAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaINC"].Value = Datos.PF_CtaFinanciaINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaFIN"].Value = Datos.PF_CtaFinanciaFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroSOL"].Value = Datos.PF_CtaSeguroSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroAJU"].Value = Datos.PF_CtaSeguroAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroINC"].Value = Datos.PF_CtaSeguroINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroFIN"].Value = Datos.PF_CtaSeguroFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalSOL"].Value = Datos.PF_CtaTotalSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalAJU"].Value = Datos.PF_CtaTotalAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalINC"].Value = Datos.PF_CtaTotalINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalFIN"].Value = Datos.PF_CtaTotalFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifSOL"].Value = Datos.PF_CtaApoyosDifSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifAJU"].Value = Datos.PF_CtaApoyosDifAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifINC"].Value = Datos.PF_CtaApoyosDifINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifFIN"].Value = Datos.PF_CtaApoyosDifFIN.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosSOL"].Value = Datos.PF_IngDispApoyosSOL.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosAJU"].Value = Datos.PF_IngDispApoyosAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosINC"].Value = Datos.PF_IngDispApoyosINC.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosFIN"].Value = Datos.PF_IngDispApoyosFIN.Value;
                mySqlCommandSel.Parameters["@PF_ReqSopIngrIndDEU"].Value = Datos.PF_ReqSopIngrIndDEU.Value;
                mySqlCommandSel.Parameters["@PF_ReqSopIngrIndCON"].Value = Datos.PF_ReqSopIngrIndCON.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1SOL"].Value = Datos.PF_IngReqDeu1SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1AJU"].Value = Datos.PF_IngReqDeu1AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1INC"].Value = Datos.PF_IngReqDeu1INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1FIN"].Value = Datos.PF_IngReqDeu1FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2SOL"].Value = Datos.PF_IngReqDeu2SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2AJU"].Value = Datos.PF_IngReqDeu2AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2INC"].Value = Datos.PF_IngReqDeu2INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2FIN"].Value = Datos.PF_IngReqDeu2FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu3SOL"].Value = Datos.PF_IngReqDeu3SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuAJU"].Value = Datos.PF_IngReqDeuAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu3INC"].Value = Datos.PF_IngReqDeu3INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu3FIN"].Value = Datos.PF_IngReqDeu3FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinSOL"].Value = Datos.PF_IngReqDeuFinSOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinAJU"].Value = Datos.PF_IngReqDeuFinAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinINC"].Value = Datos.PF_IngReqDeuFinINC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinFIN"].Value = Datos.PF_IngReqDeuFinFIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1SOL"].Value = Datos.PF_IngReqCon1SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1AJU"].Value = Datos.PF_IngReqCon1AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1INC"].Value = Datos.PF_IngReqCon1INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1FIN"].Value = Datos.PF_IngReqCon1FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2SOL"].Value = Datos.PF_IngReqCon2SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2AJU"].Value = Datos.PF_IngReqCon2AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2INC"].Value = Datos.PF_IngReqCon2INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2FIN"].Value = Datos.PF_IngReqCon2FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinSOL"].Value = Datos.PF_IngReqConFinSOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinAJU"].Value = Datos.PF_IngReqConFinAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinINC"].Value = Datos.PF_IngReqConFinINC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinFIN"].Value = Datos.PF_IngReqConFinFIN.Value;
                mySqlCommandSel.Parameters["@PF_Cuantia"].Value = Datos.PF_Cuantia.Value;
                mySqlCommandSel.Parameters["@PF_TasaTablEva1"].Value = Datos.PF_TasaTablEva1.Value;
                mySqlCommandSel.Parameters["@PF_FactTablEva"].Value = Datos.PF_FactTablEva.Value;
                mySqlCommandSel.Parameters["@PF_TasaTablEva2"].Value = Datos.PF_TasaTablEva2.Value;
                mySqlCommandSel.Parameters["@PF_TasaPonderada"].Value = Datos.PF_TasaPonderada.Value;

                mySqlCommandSel.Parameters["@PF_PolCubriendoInd"].Value = Datos.PF_PolCubriendoInd.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal"].Value = Datos.PF_PlazoFinal.Value;
                mySqlCommandSel.Parameters["@PF_VlrMinimoGar"].Value = Datos.PF_VlrMinimoGar.Value;
                mySqlCommandSel.Parameters["@PF_VlrMinimoFirma2"].Value = Datos.PF_VlrMinimoFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrMinimoFirma3"].Value = Datos.PF_VlrMinimoFirma3.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciacion"].Value = Datos.PF_CtaFinanciacion.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguros"].Value = Datos.PF_CtaSeguros.Value;
                mySqlCommandSel.Parameters["@PF_PorEstimado"].Value = Datos.PF_PorEstimado.Value;
                mySqlCommandSel.Parameters["@Verificacion1"].Value = Datos.Verificacion1.Value;
                mySqlCommandSel.Parameters["@Verificacion2"].Value = Datos.Verificacion2.Value;
                mySqlCommandSel.Parameters["@Verificacion3"].Value = Datos.Verificacion3.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1SOL"].Value = Datos.AN_Resultado1SOL.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1CA1"].Value = Datos.AN_Resultado1CA1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1CA2"].Value = Datos.AN_Resultado1CA2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1CA3"].Value = Datos.AN_Resultado1CA3.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1AJ1"].Value = Datos.AN_Resultado1AJ1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1AJ2"].Value = Datos.AN_Resultado1AJ2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V11"].Value = Datos.AN_Resultado1V11.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V12"].Value = Datos.AN_Resultado1V12.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V21"].Value = Datos.AN_Resultado1V21.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V22"].Value = Datos.AN_Resultado1V22.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V31"].Value = Datos.AN_Resultado1V31.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V32"].Value = Datos.AN_Resultado1V32.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2SOL"].Value = Datos.AN_Resultado2SOL.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2CA1"].Value = Datos.AN_Resultado2CA1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2CA2"].Value = Datos.AN_Resultado2CA2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2CA3"].Value = Datos.AN_Resultado2CA3.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2AJ1"].Value = Datos.AN_Resultado2AJ1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2AJ2"].Value = Datos.AN_Resultado2AJ2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V11"].Value = Datos.AN_Resultado2V11.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V12"].Value = Datos.AN_Resultado2V12.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V21"].Value = Datos.AN_Resultado2V21.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V22"].Value = Datos.AN_Resultado2V22.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V31"].Value = Datos.AN_Resultado2V31.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V32"].Value = Datos.AN_Resultado2V32.Value;
                mySqlCommandSel.Parameters["@AN_VlrVenta"].Value = Datos.AN_VlrVenta.Value;
                mySqlCommandSel.Parameters["@AN_VlrFasecolda"].Value = Datos.AN_VlrFasecolda.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaSOL"].Value = Datos.AN_VlrCtaEvaSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaCA1"].Value = Datos.AN_VlrCtaEvaCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaCA2"].Value = Datos.AN_VlrCtaEvaCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaCA3"].Value = Datos.AN_VlrCtaEvaCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaAJ1"].Value = Datos.AN_VlrCtaEvaAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaAJ2"].Value = Datos.AN_VlrCtaEvaAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV11"].Value = Datos.AN_VlrCtaEvaV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV12"].Value = Datos.AN_VlrCtaEvaV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV21"].Value = Datos.AN_VlrCtaEvaV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV22"].Value = Datos.AN_VlrCtaEvaV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV31"].Value = Datos.AN_VlrCtaEvaV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV32"].Value = Datos.AN_VlrCtaEvaV32.Value;
                mySqlCommandSel.Parameters["@AN_VlrMinLim"].Value = Datos.AN_VlrMinLim.Value;
                mySqlCommandSel.Parameters["@AN_VlrMaxLim"].Value = Datos.AN_VlrMaxLim.Value;
                mySqlCommandSel.Parameters["@AN_VlrSolicitado"].Value = Datos.AN_VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternCA1"].Value = Datos.AN_VlrAlternCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternCA2"].Value = Datos.AN_VlrAlternCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternCA3"].Value = Datos.AN_VlrAlternCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternAJ1"].Value = Datos.AN_VlrAlternAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternAJ2"].Value = Datos.AN_VlrAlternAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV11"].Value = Datos.AN_VlrAlternV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV12"].Value = Datos.AN_VlrAlternV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV21"].Value = Datos.AN_VlrAlternV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV22"].Value = Datos.AN_VlrAlternV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV31"].Value = Datos.AN_VlrAlternV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV32"].Value = Datos.AN_VlrAlternV32.Value;
                mySqlCommandSel.Parameters["@AN_CtaInicial"].Value = Datos.AN_CtaInicial.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremSOL"].Value = Datos.AN_VlrIncremSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremCA1"].Value = Datos.AN_VlrIncremCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremCA2"].Value = Datos.AN_VlrIncremCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremCA3"].Value = Datos.AN_VlrIncremCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremAJ1"].Value = Datos.AN_VlrIncremAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremAJ2"].Value = Datos.AN_VlrIncremAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV11"].Value = Datos.AN_VlrIncremV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV12"].Value = Datos.AN_VlrIncremV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV21"].Value = Datos.AN_VlrIncremV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV22"].Value = Datos.AN_VlrIncremV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV31"].Value = Datos.AN_VlrIncremV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV32"].Value = Datos.AN_VlrIncremV32.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuCA1"].Value = Datos.AN_CtaIniAjuCA1.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuCA2"].Value = Datos.AN_CtaIniAjuCA2.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuCA3"].Value = Datos.AN_CtaIniAjuCA3.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuAJ1"].Value = Datos.AN_CtaIniAjuAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuAJ2"].Value = Datos.AN_CtaIniAjuAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV11"].Value = Datos.AN_CtaIniAjuV11.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV12"].Value = Datos.AN_CtaIniAjuV12.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV21"].Value = Datos.AN_CtaIniAjuV21.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV22"].Value = Datos.AN_CtaIniAjuV22.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV31"].Value = Datos.AN_CtaIniAjuV31.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV32"].Value = Datos.AN_CtaIniAjuV32.Value;
                mySqlCommandSel.Parameters["@AN_porFinSoli"].Value = Datos.AN_porFinSoli.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxSOL"].Value = Datos.AN_porFinMaxSOL.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxCA1"].Value = Datos.AN_porFinMaxCA1.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxCA2"].Value = Datos.AN_porFinMaxCA2.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxCA3"].Value = Datos.AN_porFinMaxCA3.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxAJ1"].Value = Datos.AN_porFinMaxAJ1.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxAJ2"].Value = Datos.AN_porFinMaxAJ2.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV11"].Value = Datos.AN_porFinMaxV11.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV12"].Value = Datos.AN_porFinMaxV12.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV21"].Value = Datos.AN_porFinMaxV21.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV22"].Value = Datos.AN_porFinMaxV22.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV31"].Value = Datos.AN_porFinMaxV31.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV32"].Value = Datos.AN_porFinMaxV32.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltCA1"].Value = Datos.AN_porFinAltCA1.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltCA2"].Value = Datos.AN_porFinAltCA2.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltCA3"].Value = Datos.AN_porFinAltCA3.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltAJ1"].Value = Datos.AN_porFinAltAJ1.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltAJ2"].Value = Datos.AN_porFinAltAJ2.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV11"].Value = Datos.AN_porFinAltV11.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV12"].Value = Datos.AN_porFinAltV12.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV21"].Value = Datos.AN_porFinAltV21.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV22"].Value = Datos.AN_porFinAltV22.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV31"].Value = Datos.AN_porFinAltV31.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV32"].Value = Datos.AN_porFinAltV32.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarAJ1"].Value = Datos.AN_CumParGarAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarAJ2"].Value = Datos.AN_CumParGarAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV11"].Value = Datos.AN_CumParGarV11.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV12"].Value = Datos.AN_CumParGarV12.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV21"].Value = Datos.AN_CumParGarV21.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV22"].Value = Datos.AN_CumParGarV22.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV31"].Value = Datos.AN_CumParGarV31.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV32"].Value = Datos.AN_CumParGarV32.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoSOL"].Value = Datos.AN_CapPagoSOL.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoCA1"].Value = Datos.AN_CapPagoCA1.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoCA2"].Value = Datos.AN_CapPagoCA2.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoCA3"].Value = Datos.AN_CapPagoCA3.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoAJ1"].Value = Datos.AN_CapPagoAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoAJ2"].Value = Datos.AN_CapPagoAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV11"].Value = Datos.AN_CapPagoV11.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV12"].Value = Datos.AN_CapPagoV12.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV21"].Value = Datos.AN_CapPagoV21.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV22"].Value = Datos.AN_CapPagoV22.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV31"].Value = Datos.AN_CapPagoV31.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV32"].Value = Datos.AN_CapPagoV32.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCICA1"].Value = Datos.AN_PuedeIncCICA1.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCICA2"].Value = Datos.AN_PuedeIncCICA2.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCICA3"].Value = Datos.AN_PuedeIncCICA3.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIAJ1"].Value = Datos.AN_PuedeIncCIAJ1.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIAJ2"].Value = Datos.AN_PuedeIncCIAJ2.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV11"].Value = Datos.AN_PuedeIncCIV11.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV12"].Value = Datos.AN_PuedeIncCIV12.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV21"].Value = Datos.AN_PuedeIncCIV21.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV22"].Value = Datos.AN_PuedeIncCIV22.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV31"].Value = Datos.AN_PuedeIncCIV31.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV32"].Value = Datos.AN_PuedeIncCIV32.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxSOL"].Value = Datos.AN_CumPorMaxSOL.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxCA1"].Value = Datos.AN_CumPorMaxCA1.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxCA2"].Value = Datos.AN_CumPorMaxCA2.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxCA3"].Value = Datos.AN_CumPorMaxCA3.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxAJ1"].Value = Datos.AN_CumPorMaxAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxAJ2"].Value = Datos.AN_CumPorMaxAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV11"].Value = Datos.AN_CumPorMaxV11.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV12"].Value = Datos.AN_CumPorMaxV12.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV21"].Value = Datos.AN_CumPorMaxV21.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV22"].Value = Datos.AN_CumPorMaxV22.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV31"].Value = Datos.AN_CumPorMaxV31.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV32"].Value = Datos.AN_CumPorMaxV32.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinCA1"].Value = Datos.AN_CumMtoMinCA1.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinCA2"].Value = Datos.AN_CumMtoMinCA2.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinCA3"].Value = Datos.AN_CumMtoMinCA3.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinAJ1"].Value = Datos.AN_CumMtoMinAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinAJ2"].Value = Datos.AN_CumMtoMinAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV11"].Value = Datos.AN_CumMtoMinV11.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV12"].Value = Datos.AN_CumMtoMinV12.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV21"].Value = Datos.AN_CumMtoMinV21.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV22"].Value = Datos.AN_CumMtoMinV22.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV31"].Value = Datos.AN_CumMtoMinV31.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV32"].Value = Datos.AN_CumMtoMinV32.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroSOL"].Value = Datos.AN_CumOtroSOL.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroCA1"].Value = Datos.AN_CumOtroCA1.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroCA2"].Value = Datos.AN_CumOtroCA2.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroCA3"].Value = Datos.AN_CumOtroCA3.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroAJ1"].Value = Datos.AN_CumOtroAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroAJ2"].Value = Datos.AN_CumOtroAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV11"].Value = Datos.AN_CumOtroV11.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV12"].Value = Datos.AN_CumOtroV12.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV21"].Value = Datos.AN_CumOtroV21.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV22"].Value = Datos.AN_CumOtroV22.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV31"].Value = Datos.AN_CumOtroV31.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV32"].Value = Datos.AN_CumOtroV32.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaSOL"].Value = Datos.AN_VlrAutorizaSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaCA1"].Value = Datos.AN_VlrAutorizaCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaCA2"].Value = Datos.AN_VlrAutorizaCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaCA3"].Value = Datos.AN_VlrAutorizaCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaAJ1"].Value = Datos.AN_VlrAutorizaAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaAJ2"].Value = Datos.AN_VlrAutorizaAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV11"].Value = Datos.AN_VlrAutorizaV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV12"].Value = Datos.AN_VlrAutorizaV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV21"].Value = Datos.AN_VlrAutorizaV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV22"].Value = Datos.AN_VlrAutorizaV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV31"].Value = Datos.AN_VlrAutorizaV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV32"].Value = Datos.AN_VlrAutorizaV32.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinSOL"].Value = Datos.AN_VlrAutFinSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinCA2"].Value = Datos.AN_VlrAutFinCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinAJ1"].Value = Datos.AN_VlrAutFinAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinV12"].Value = Datos.AN_VlrAutFinV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinV21"].Value = Datos.AN_VlrAutFinV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinV31"].Value = Datos.AN_VlrAutFinV31.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComAJ1"].Value = Datos.AN_AltNoComAJ1.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComAJ2"].Value = Datos.AN_AltNoComAJ2.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV12"].Value = Datos.AN_AltNoComV12.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV21"].Value = Datos.AN_AltNoComV21.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV22"].Value = Datos.AN_AltNoComV22.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV31"].Value = Datos.AN_AltNoComV31.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV32"].Value = Datos.AN_AltNoComV32.Value;

                mySqlCommandSel.Parameters["@Alternativa8"].Value = Datos.Alternativa8.Value;
                mySqlCommandSel.Parameters["@FactorAlt8"].Value = Datos.FactorAlt8.Value;
                mySqlCommandSel.Parameters["@MontoAlt8"].Value = Datos.MontoAlt8.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt8"].Value = Datos.GarantiaAlt8.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu1"].Value = Datos.IngrMinSopDeu1.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu2"].Value = Datos.IngrMinSopDeu2.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu3"].Value = Datos.IngrMinSopDeu3.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu4"].Value = Datos.IngrMinSopDeu4.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu5"].Value = Datos.IngrMinSopDeu5.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu6"].Value = Datos.IngrMinSopDeu6.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu7"].Value = Datos.IngrMinSopDeu7.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu8"].Value = Datos.IngrMinSopDeu8.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon1"].Value = Datos.IngrMinSopCon1.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon2"].Value = Datos.IngrMinSopCon2.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon3"].Value = Datos.IngrMinSopCon3.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon4"].Value = Datos.IngrMinSopCon4.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon5"].Value = Datos.IngrMinSopCon5.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon6"].Value = Datos.IngrMinSopCon6.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon7"].Value = Datos.IngrMinSopCon7.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon8"].Value = Datos.IngrMinSopCon8.Value;
                mySqlCommandSel.Parameters["@FechaFirmaDocumento"].Value = Datos.FechaFirmaDocumento.Value;
                mySqlCommandSel.Parameters["@AccionSolicitud"].Value = Datos.AccionSolicitud.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = Datos.Observacion.Value;
                mySqlCommandSel.Parameters["@PF_Plazo"].Value = Datos.PF_Plazo.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxEstadoActual"].Value = Datos.PF_PorMaxEstadoActual.Value;
                mySqlCommandSel.Parameters["@porMaximo"].Value = Datos.porMaximo.Value;
                mySqlCommandSel.Parameters["@VlrGarantia"].Value = Datos.VlrGarantia.Value;

                mySqlCommandSel.Parameters["@AccionSolicitud1"].Value = Datos.AccionSolicitud1.Value;
                mySqlCommandSel.Parameters["@AccionSolicitud2"].Value = Datos.AccionSolicitud2.Value;
                mySqlCommandSel.Parameters["@AccionSolicitud3"].Value = Datos.AccionSolicitud3.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal1"].Value = Datos.PF_PlazoFinal1.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal2"].Value = Datos.PF_PlazoFinal2.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal3"].Value = Datos.PF_PlazoFinal3.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFirma1"].Value = Datos.PF_VlrMontoFirma1.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFirma2"].Value = Datos.PF_VlrMontoFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFirma3"].Value = Datos.PF_VlrMontoFirma3.Value;
                mySqlCommandSel.Parameters["@PF_TasaPerfilOBL"].Value = Datos.PF_TasaPerfilOBL.Value;
                mySqlCommandSel.Parameters["@PF_TasaFirma3OBL"].Value = Datos.PF_TasaFirma3OBL.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFirma1"].Value = Datos.PF_PorMaxFirma1.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFirma2"].Value = Datos.PF_PorMaxFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaPerfil"].Value = Datos.PF_VlrGarantiaPerfil.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaFirma1"].Value = Datos.PF_VlrGarantiaFirma1.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaFirma2"].Value = Datos.PF_VlrGarantiaFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaFirma3"].Value = Datos.PF_VlrGarantiaFirma3.Value;

                //Eg
                mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
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
                int consec = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                return consec;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla drSolicitudDatosOtros
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudDatosOtros_Update(DTO_drSolicitudDatosOtros Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                              "UPDATE drSolicitudDatosOtros SET " +
                                                                " NumeroDoc=@NumeroDoc" +
                                                                ",Version=@Version" +
                                                                ",VlrPoliza=@VlrPoliza" +
                                                                ",CubriendoInd=@CubriendoInd" +
                                                                ",AseguradoraID=@AseguradoraID" +
                                                                ",FinanciaPOLInd=@FinanciaPOLInd" +
                                                                ",VlrMensualSV=@VlrMensualSV" +
                                                                ",TipoPrenda=@TipoPrenda" +
                                                                ",PrefijoPrenda=@PrefijoPrenda" +
                                                                ",NumeroPrenda=@NumeroPrenda" +
                                                                ",Registro=@Registro" +
                                                                ",CedulaReg=@CedulaReg" +
                                                                ",NombreREG=@NombreREG" +
                                                                ",DireccionREG=@DireccionREG" +
                                                                ",Alternativa1=@Alternativa1" +
                                                                ",Alternativa2=@Alternativa2" +
                                                                ",Alternativa3=@Alternativa3" +
                                                                ",Alternativa4=@Alternativa4" +
                                                                ",Alternativa5=@Alternativa5" +
                                                                ",Alternativa6=@Alternativa6" +
                                                                ",Alternativa7=@Alternativa7" +
                                                                ",FactorAlt1=@FactorAlt1" +
                                                                ",FactorAlt2=@FactorAlt2" +
                                                                ",FactorAlt3=@FactorAlt3" +
                                                                ",FactorAlt4=@FactorAlt4" +
                                                                ",FactorAlt5=@FactorAlt5" +
                                                                ",FactorAlt6=@FactorAlt6" +
                                                                ",FactorAlt7=@FactorAlt7" +
                                                                ",MontoAlt1=@MontoAlt1" +
                                                                ",MontoAlt2=@MontoAlt2" +
                                                                ",MontoAlt3=@MontoAlt3" +
                                                                ",MontoAlt4=@MontoAlt4" +
                                                                ",MontoAlt5=@MontoAlt5" +
                                                                ",MontoAlt6=@MontoAlt6" +
                                                                ",MontoAlt7=@MontoAlt7" +
                                                                ",GarantiaAlt1=@GarantiaAlt1" +
                                                                ",GarantiaAlt2=@GarantiaAlt2" +
                                                                ",GarantiaAlt3=@GarantiaAlt3" +
                                                                ",GarantiaAlt4=@GarantiaAlt4" +
                                                                ",GarantiaAlt5=@GarantiaAlt5" +
                                                                ",GarantiaAlt6=@GarantiaAlt6" +
                                                                ",GarantiaAlt7=@GarantiaAlt7" +
                                                                ",VlrGtiaEvaluacion=@VlrGtiaEvaluacion" +
                                                                ",MontoMaximo=@MontoMaximo" +
                                                                ",EstimadoSeguros=@EstimadoSeguros" +
                                                                ",MaxFinanciacionAut=@MaxFinanciacionAut" +
                                                                ",Plazo=@Plazo" +
                                                                ",EstimadoObl=@EstimadoObl" +
                                                                ",CuotaFin=@CuotaFin" +
                                                                ",CuotaSeg=@CuotaSeg" +
                                                                ",CuotaTotal=@CuotaTotal" +
                                                                ",Estado=@Estado" +
                                                                ",Calificacion=@Calificacion" +
                                                                ",Revision=@Revision" +
                                                                ",CargoResp=@CargoResp" +
                                                                ",Firma1Ind=@Firma1Ind" +
                                                                ",Firma2Ind=@Firma2Ind" +
                                                                ",Firma3Ind=@Firma3Ind" +
                                                                ",UsuarioResp=@UsuarioResp" +
                                                                ",UsuarioFirma1=@UsuarioFirma1" +
                                                                ",UsuarioFirma2=@UsuarioFirma2" +
                                                                ",UsuarioFirma3=@UsuarioFirma3" +
                                                                ",FechaFirmaResp=@FechaFirmaResp" +
                                                                ",FechaFirma1=@FechaFirma1" +
                                                                ",FechaFirma2=@FechaFirma2" +
                                                                ",FechaFirma3=@FechaFirma3" +
                                                                ",FechaDatacredito=@FechaDatacredito" +
                                                                ",FechaLegalizacion=@FechaLegalizacion" +
                                                                ",FechaDesembolso=@FechaDesembolso" +
                                                                ",CartaAprobDirInd=@CartaAprobDirInd" +
                                                                ",CartaAprobDocInd=@CartaAprobDocInd" +
                                                                ",CartapreAprobInd=@CartapreAprobInd" +
                                                                ",CartaNoViableInd=@CartaNoViableInd" +
                                                                ",CartaRevocaInd=@CartaRevocaInd" +
                                                                ",CartaRatificaInd=@CartaRatificaInd" +
                                                                ",CartaAprobDirUsu=@CartaAprobDirUsu" +
                                                                ",CartaAprobDocUsu=@CartaAprobDocUsu" +
                                                                ",CartapreAprobUsu=@CartapreAprobUsu" +
                                                                ",CartaNoViableUsu=@CartaNoViableUsu" +
                                                                ",CartaRevocaUsu=@CartaRevocaUsu" +
                                                                ",CartaRatificaUsu=@CartaRatificaUsu" +
                                                                ",CartaAprobDirFecha=@CartaAprobDirFecha" +
                                                                ",CartaAprobDocFecha=@CartaAprobDocFecha" +
                                                                ",CartapreAprobFecha=@CartapreAprobFecha" +
                                                                ",CartaNoViableFecha=@CartaNoViableFecha" +
                                                                ",CartaRevocaFecha=@CartaRevocaFecha" +
                                                                ",CartaRatificaFecha=@CartaRatificaFecha" +
                                                                ",PerfilUsuario=@PerfilUsuario"+
                                                                ",PerfilFecha=@PerfilFecha"+
                                                                ",EstActualFactor=@EstActualFactor"+
                                                                ",VlrSolicitado=@VlrSolicitado" +
                                                                ",SMMLV=@SMMLV" +
                                                                ",PF_PorIngrPagoCtas=@PF_PorIngrPagoCtas" +
                                                                ",PF_IngrDispApoyosDEU=@PF_IngrDispApoyosDEU" +
                                                                ",PF_IngrDispApoyosCON=@PF_IngrDispApoyosCON" +
                                                                ",PF_IngrDispApoyos=@PF_IngrDispApoyos" +
                                                                ",PF_VlrMontoSOL=@PF_VlrMontoSOL" +
                                                                ",PF_VlrMontoAJU=@PF_VlrMontoAJU" +
                                                                ",PF_VlrMontoINC=@PF_VlrMontoINC" +
                                                                ",PF_VlrMontoFIN=@PF_VlrMontoFIN" +
                                                                ",PF_CtaFinanciaSOL=@PF_CtaFinanciaSOL" +
                                                                ",PF_CtaFinanciaAJU=@PF_CtaFinanciaAJU" +
                                                                ",PF_CtaFinanciaINC=@PF_CtaFinanciaINC" +
                                                                ",PF_CtaFinanciaFIN=@PF_CtaFinanciaFIN" +
                                                                ",PF_CtaSeguroSOL=@PF_CtaSeguroSOL" +
                                                                ",PF_CtaSeguroAJU=@PF_CtaSeguroAJU" +
                                                                ",PF_CtaSeguroINC=@PF_CtaSeguroINC" +
                                                                ",PF_CtaSeguroFIN=@PF_CtaSeguroFIN" +
                                                                ",PF_CtaTotalSOL=@PF_CtaTotalSOL" +
                                                                ",PF_CtaTotalAJU=@PF_CtaTotalAJU" +
                                                                ",PF_CtaTotalINC=@PF_CtaTotalINC" +
                                                                ",PF_CtaTotalFIN=@PF_CtaTotalFIN" +
                                                                ",PF_CtaApoyosDifSOL=@PF_CtaApoyosDifSOL" +
                                                                ",PF_CtaApoyosDifAJU=@PF_CtaApoyosDifAJU" +
                                                                ",PF_CtaApoyosDifINC=@PF_CtaApoyosDifINC" +
                                                                ",PF_CtaApoyosDifFIN=@PF_CtaApoyosDifFIN" +
                                                                ",PF_IngDispApoyosSOL=@PF_IngDispApoyosSOL" +
                                                                ",PF_IngDispApoyosAJU=@PF_IngDispApoyosAJU" +
                                                                ",PF_IngDispApoyosINC=@PF_IngDispApoyosINC" +
                                                                ",PF_IngDispApoyosFIN=@PF_IngDispApoyosFIN" +
                                                                ",PF_ReqSopIngrIndDEU=@PF_ReqSopIngrIndDEU" +
                                                                ",PF_ReqSopIngrIndCON=@PF_ReqSopIngrIndCON" +
                                                                ",PF_IngReqDeu1SOL=@PF_IngReqDeu1SOL" +
                                                                ",PF_IngReqDeu1AJU=@PF_IngReqDeu1AJU" +
                                                                ",PF_IngReqDeu1INC=@PF_IngReqDeu1INC" +
                                                                ",PF_IngReqDeu1FIN=@PF_IngReqDeu1FIN" +
                                                                ",PF_IngReqDeu2SOL=@PF_IngReqDeu2SOL" +
                                                                ",PF_IngReqDeu2AJU=@PF_IngReqDeu2AJU" +
                                                                ",PF_IngReqDeu2INC=@PF_IngReqDeu2INC" +
                                                                ",PF_IngReqDeu2FIN=@PF_IngReqDeu2FIN" +
                                                                ",PF_IngReqDeu3SOL=@PF_IngReqDeu3SOL" +
                                                                ",PF_IngReqDeuAJU=@PF_IngReqDeuAJU" +
                                                                ",PF_IngReqDeu3INC=@PF_IngReqDeu3INC" +
                                                                ",PF_IngReqDeu3FIN=@PF_IngReqDeu3FIN" +
                                                                ",PF_IngReqDeuFinSOL=@PF_IngReqDeuFinSOL" +
                                                                ",PF_IngReqDeuFinAJU=@PF_IngReqDeuFinAJU" +
                                                                ",PF_IngReqDeuFinINC=@PF_IngReqDeuFinINC" +
                                                                ",PF_IngReqDeuFinFIN=@PF_IngReqDeuFinFIN" +
                                                                ",PF_IngReqCon1SOL=@PF_IngReqCon1SOL" +
                                                                ",PF_IngReqCon1AJU=@PF_IngReqCon1AJU" +
                                                                ",PF_IngReqCon1INC=@PF_IngReqCon1INC" +
                                                                ",PF_IngReqCon1FIN=@PF_IngReqCon1FIN" +
                                                                ",PF_IngReqCon2SOL=@PF_IngReqCon2SOL" +
                                                                ",PF_IngReqCon2AJU=@PF_IngReqCon2AJU" +
                                                                ",PF_IngReqCon2INC=@PF_IngReqCon2INC" +
                                                                ",PF_IngReqCon2FIN=@PF_IngReqCon2FIN" +
                                                                ",PF_IngReqConFinSOL=@PF_IngReqConFinSOL" +
                                                                ",PF_IngReqConFinAJU=@PF_IngReqConFinAJU" +
                                                                ",PF_IngReqConFinINC=@PF_IngReqConFinINC" +
                                                                ",PF_IngReqConFinFIN=@PF_IngReqConFinFIN" +
                                                                ",PF_Cuantia=@PF_Cuantia" +
                                                                ",PF_TasaTablEva1=@PF_TasaTablEva1" +
                                                                ",PF_FactTablEva=@PF_FactTablEva" +
                                                                ",PF_TasaTablEva2=@PF_TasaTablEva2" +
                                                                ",PF_TasaPonderada=@PF_TasaPonderada" +
                                                                ",PF_PolCubriendoInd=@PF_PolCubriendoInd" +
                                                                ",PF_PlazoFinal=@PF_PlazoFinal" +
                                                                ",PF_VlrMinimoGar=@PF_VlrMinimoGar" +
                                                                ",PF_VlrMinimoFirma2=@PF_VlrMinimoFirma2" +
                                                                ",PF_VlrMinimoFirma3=@PF_VlrMinimoFirma3" +
                                                                ",PF_CtaFinanciacion=@PF_CtaFinanciacion" +
                                                                ",PF_CtaSeguros=@PF_CtaSeguros" +
                                                                ",PF_PorEstimado=@PF_PorEstimado" +
                                                                ",Verificacion1=@Verificacion1" +
                                                                ",Verificacion2=@Verificacion2" +
                                                                ",Verificacion3=@Verificacion3" +
                                                                ",AN_Resultado1SOL=@AN_Resultado1SOL" +
                                                                ",AN_Resultado1CA1=@AN_Resultado1CA1" +
                                                                ",AN_Resultado1CA2=@AN_Resultado1CA2" +
                                                                ",AN_Resultado1CA3=@AN_Resultado1CA3" +
                                                                ",AN_Resultado1AJ1=@AN_Resultado1AJ1" +
                                                                ",AN_Resultado1AJ2=@AN_Resultado1AJ2" +
                                                                ",AN_Resultado1V11=@AN_Resultado1V11" +
                                                                ",AN_Resultado1V12=@AN_Resultado1V12" +
                                                                ",AN_Resultado1V21=@AN_Resultado1V21" +
                                                                ",AN_Resultado1V22=@AN_Resultado1V22" +
                                                                ",AN_Resultado1V31=@AN_Resultado1V31" +
                                                                ",AN_Resultado1V32=@AN_Resultado1V32" +
                                                                ",AN_Resultado2SOL=@AN_Resultado2SOL" +
                                                                ",AN_Resultado2CA1=@AN_Resultado2CA1" +
                                                                ",AN_Resultado2CA2=@AN_Resultado2CA2" +
                                                                ",AN_Resultado2CA3=@AN_Resultado2CA3" +
                                                                ",AN_Resultado2AJ1=@AN_Resultado2AJ1" +
                                                                ",AN_Resultado2AJ2=@AN_Resultado2AJ2" +
                                                                ",AN_Resultado2V11=@AN_Resultado2V11" +
                                                                ",AN_Resultado2V12=@AN_Resultado2V12" +
                                                                ",AN_Resultado2V21=@AN_Resultado2V21" +
                                                                ",AN_Resultado2V22=@AN_Resultado2V22" +
                                                                ",AN_Resultado2V31=@AN_Resultado2V31" +
                                                                ",AN_Resultado2V32=@AN_Resultado2V32" +
                                                                ",AN_VlrVenta=@AN_VlrVenta" +
                                                                ",AN_VlrFasecolda=@AN_VlrFasecolda" +
                                                                ",AN_VlrCtaEvaSOL=@AN_VlrCtaEvaSOL" +
                                                                ",AN_VlrCtaEvaCA1=@AN_VlrCtaEvaCA1" +
                                                                ",AN_VlrCtaEvaCA2=@AN_VlrCtaEvaCA2" +
                                                                ",AN_VlrCtaEvaCA3=@AN_VlrCtaEvaCA3" +
                                                                ",AN_VlrCtaEvaAJ1=@AN_VlrCtaEvaAJ1" +
                                                                ",AN_VlrCtaEvaAJ2=@AN_VlrCtaEvaAJ2" +
                                                                ",AN_VlrCtaEvaV11=@AN_VlrCtaEvaV11" +
                                                                ",AN_VlrCtaEvaV12=@AN_VlrCtaEvaV12" +
                                                                ",AN_VlrCtaEvaV21=@AN_VlrCtaEvaV21" +
                                                                ",AN_VlrCtaEvaV22=@AN_VlrCtaEvaV22" +
                                                                ",AN_VlrCtaEvaV31=@AN_VlrCtaEvaV31" +
                                                                ",AN_VlrCtaEvaV32=@AN_VlrCtaEvaV32" +
                                                                ",AN_VlrMinLim=@AN_VlrMinLim" +
                                                                ",AN_VlrMaxLim=@AN_VlrMaxLim" +
                                                                ",AN_VlrSolicitado=@AN_VlrSolicitado" +
                                                                ",AN_VlrAlternCA1=@AN_VlrAlternCA1" +
                                                                ",AN_VlrAlternCA2=@AN_VlrAlternCA2" +
                                                                ",AN_VlrAlternCA3=@AN_VlrAlternCA3" +
                                                                ",AN_VlrAlternAJ1=@AN_VlrAlternAJ1" +
                                                                ",AN_VlrAlternAJ2=@AN_VlrAlternAJ2" +
                                                                ",AN_VlrAlternV11=@AN_VlrAlternV11" +
                                                                ",AN_VlrAlternV12=@AN_VlrAlternV12" +
                                                                ",AN_VlrAlternV21=@AN_VlrAlternV21" +
                                                                ",AN_VlrAlternV22=@AN_VlrAlternV22" +
                                                                ",AN_VlrAlternV31=@AN_VlrAlternV31" +
                                                                ",AN_VlrAlternV32=@AN_VlrAlternV32" +
                                                                ",AN_CtaInicial=@AN_CtaInicial" +
                                                                ",AN_VlrIncremSOL=@AN_VlrIncremSOL" +
                                                                ",AN_VlrIncremCA1=@AN_VlrIncremCA1" +
                                                                ",AN_VlrIncremCA2=@AN_VlrIncremCA2" +
                                                                ",AN_VlrIncremCA3=@AN_VlrIncremCA3" +
                                                                ",AN_VlrIncremAJ1=@AN_VlrIncremAJ1" +
                                                                ",AN_VlrIncremAJ2=@AN_VlrIncremAJ2" +
                                                                ",AN_VlrIncremV11=@AN_VlrIncremV11" +
                                                                ",AN_VlrIncremV12=@AN_VlrIncremV12" +
                                                                ",AN_VlrIncremV21=@AN_VlrIncremV21" +
                                                                ",AN_VlrIncremV22=@AN_VlrIncremV22" +
                                                                ",AN_VlrIncremV31=@AN_VlrIncremV31" +
                                                                ",AN_VlrIncremV32=@AN_VlrIncremV32" +
                                                                ",AN_CtaIniAjuCA1=@AN_CtaIniAjuCA1" +
                                                                ",AN_CtaIniAjuCA2=@AN_CtaIniAjuCA2" +
                                                                ",AN_CtaIniAjuCA3=@AN_CtaIniAjuCA3" +
                                                                ",AN_CtaIniAjuAJ1=@AN_CtaIniAjuAJ1" +
                                                                ",AN_CtaIniAjuAJ2=@AN_CtaIniAjuAJ2" +
                                                                ",AN_CtaIniAjuV11=@AN_CtaIniAjuV11" +
                                                                ",AN_CtaIniAjuV12=@AN_CtaIniAjuV12" +
                                                                ",AN_CtaIniAjuV21=@AN_CtaIniAjuV21" +
                                                                ",AN_CtaIniAjuV22=@AN_CtaIniAjuV22" +
                                                                ",AN_CtaIniAjuV31=@AN_CtaIniAjuV31" +
                                                                ",AN_CtaIniAjuV32=@AN_CtaIniAjuV32" +
                                                                ",AN_porFinSoli=@AN_porFinSoli" +
                                                                ",AN_porFinMaxSOL=@AN_porFinMaxSOL" +
                                                                ",AN_porFinMaxCA1=@AN_porFinMaxCA1" +
                                                                ",AN_porFinMaxCA2=@AN_porFinMaxCA2" +
                                                                ",AN_porFinMaxCA3=@AN_porFinMaxCA3" +
                                                                ",AN_porFinMaxAJ1=@AN_porFinMaxAJ1" +
                                                                ",AN_porFinMaxAJ2=@AN_porFinMaxAJ2" +
                                                                ",AN_porFinMaxV11=@AN_porFinMaxV11" +
                                                                ",AN_porFinMaxV12=@AN_porFinMaxV12" +
                                                                ",AN_porFinMaxV21=@AN_porFinMaxV21" +
                                                                ",AN_porFinMaxV22=@AN_porFinMaxV22" +
                                                                ",AN_porFinMaxV31=@AN_porFinMaxV31" +
                                                                ",AN_porFinMaxV32=@AN_porFinMaxV32" +
                                                                ",AN_porFinAltCA1=@AN_porFinAltCA1" +
                                                                ",AN_porFinAltCA2=@AN_porFinAltCA2" +
                                                                ",AN_porFinAltCA3=@AN_porFinAltCA3" +
                                                                ",AN_porFinAltAJ1=@AN_porFinAltAJ1" +
                                                                ",AN_porFinAltAJ2=@AN_porFinAltAJ2" +
                                                                ",AN_porFinAltV11=@AN_porFinAltV11" +
                                                                ",AN_porFinAltV12=@AN_porFinAltV12" +
                                                                ",AN_porFinAltV21=@AN_porFinAltV21" +
                                                                ",AN_porFinAltV22=@AN_porFinAltV22" +
                                                                ",AN_porFinAltV31=@AN_porFinAltV31" +
                                                                ",AN_porFinAltV32=@AN_porFinAltV32" +
                                                                ",AN_CumParGarAJ1=@AN_CumParGarAJ1" +
                                                                ",AN_CumParGarAJ2=@AN_CumParGarAJ2" +
                                                                ",AN_CumParGarV11=@AN_CumParGarV11" +
                                                                ",AN_CumParGarV12=@AN_CumParGarV12" +
                                                                ",AN_CumParGarV21=@AN_CumParGarV21" +
                                                                ",AN_CumParGarV22=@AN_CumParGarV22" +
                                                                ",AN_CumParGarV31=@AN_CumParGarV31" +
                                                                ",AN_CumParGarV32=@AN_CumParGarV32" +
                                                                ",AN_CapPagoSOL=@AN_CapPagoSOL" +
                                                                ",AN_CapPagoCA1=@AN_CapPagoCA1" +
                                                                ",AN_CapPagoCA2=@AN_CapPagoCA2" +
                                                                ",AN_CapPagoCA3=@AN_CapPagoCA3" +
                                                                ",AN_CapPagoAJ1=@AN_CapPagoAJ1" +
                                                                ",AN_CapPagoAJ2=@AN_CapPagoAJ2" +
                                                                ",AN_CapPagoV11=@AN_CapPagoV11" +
                                                                ",AN_CapPagoV12=@AN_CapPagoV12" +
                                                                ",AN_CapPagoV21=@AN_CapPagoV21" +
                                                                ",AN_CapPagoV22=@AN_CapPagoV22" +
                                                                ",AN_CapPagoV31=@AN_CapPagoV31" +
                                                                ",AN_CapPagoV32=@AN_CapPagoV32" +
                                                                ",AN_PuedeIncCICA1=@AN_PuedeIncCICA1" +
                                                                ",AN_PuedeIncCICA2=@AN_PuedeIncCICA2" +
                                                                ",AN_PuedeIncCICA3=@AN_PuedeIncCICA3" +
                                                                ",AN_PuedeIncCIAJ1=@AN_PuedeIncCIAJ1" +
                                                                ",AN_PuedeIncCIAJ2=@AN_PuedeIncCIAJ2" +
                                                                ",AN_PuedeIncCIV11=@AN_PuedeIncCIV11" +
                                                                ",AN_PuedeIncCIV12=@AN_PuedeIncCIV12" +
                                                                ",AN_PuedeIncCIV21=@AN_PuedeIncCIV21" +
                                                                ",AN_PuedeIncCIV22=@AN_PuedeIncCIV22" +
                                                                ",AN_PuedeIncCIV31=@AN_PuedeIncCIV31" +
                                                                ",AN_PuedeIncCIV32=@AN_PuedeIncCIV32" +
                                                                ",AN_CumPorMaxSOL=@AN_CumPorMaxSOL" +
                                                                ",AN_CumPorMaxCA1=@AN_CumPorMaxCA1" +
                                                                ",AN_CumPorMaxCA2=@AN_CumPorMaxCA2" +
                                                                ",AN_CumPorMaxCA3=@AN_CumPorMaxCA3" +
                                                                ",AN_CumPorMaxAJ1=@AN_CumPorMaxAJ1" +
                                                                ",AN_CumPorMaxAJ2=@AN_CumPorMaxAJ2" +
                                                                ",AN_CumPorMaxV11=@AN_CumPorMaxV11" +
                                                                ",AN_CumPorMaxV12=@AN_CumPorMaxV12" +
                                                                ",AN_CumPorMaxV21=@AN_CumPorMaxV21" +
                                                                ",AN_CumPorMaxV22=@AN_CumPorMaxV22" +
                                                                ",AN_CumPorMaxV31=@AN_CumPorMaxV31" +
                                                                ",AN_CumPorMaxV32=@AN_CumPorMaxV32" +
                                                                ",AN_CumMtoMinCA1=@AN_CumMtoMinCA1" +
                                                                ",AN_CumMtoMinCA2=@AN_CumMtoMinCA2" +
                                                                ",AN_CumMtoMinCA3=@AN_CumMtoMinCA3" +
                                                                ",AN_CumMtoMinAJ1=@AN_CumMtoMinAJ1" +
                                                                ",AN_CumMtoMinAJ2=@AN_CumMtoMinAJ2" +
                                                                ",AN_CumMtoMinV11=@AN_CumMtoMinV11" +
                                                                ",AN_CumMtoMinV12=@AN_CumMtoMinV12" +
                                                                ",AN_CumMtoMinV21=@AN_CumMtoMinV21" +
                                                                ",AN_CumMtoMinV22=@AN_CumMtoMinV22" +
                                                                ",AN_CumMtoMinV31=@AN_CumMtoMinV31" +
                                                                ",AN_CumMtoMinV32=@AN_CumMtoMinV32" +
                                                                ",AN_CumOtroSOL=@AN_CumOtroSOL" +
                                                                ",AN_CumOtroCA1=@AN_CumOtroCA1" +
                                                                ",AN_CumOtroCA2=@AN_CumOtroCA2" +
                                                                ",AN_CumOtroCA3=@AN_CumOtroCA3" +
                                                                ",AN_CumOtroAJ1=@AN_CumOtroAJ1" +
                                                                ",AN_CumOtroAJ2=@AN_CumOtroAJ2" +
                                                                ",AN_CumOtroV11=@AN_CumOtroV11" +
                                                                ",AN_CumOtroV12=@AN_CumOtroV12" +
                                                                ",AN_CumOtroV21=@AN_CumOtroV21" +
                                                                ",AN_CumOtroV22=@AN_CumOtroV22" +
                                                                ",AN_CumOtroV31=@AN_CumOtroV31" +
                                                                ",AN_CumOtroV32=@AN_CumOtroV32" +
                                                                ",AN_VlrAutorizaSOL=@AN_VlrAutorizaSOL" +
                                                                ",AN_VlrAutorizaCA1=@AN_VlrAutorizaCA1" +
                                                                ",AN_VlrAutorizaCA2=@AN_VlrAutorizaCA2" +
                                                                ",AN_VlrAutorizaCA3=@AN_VlrAutorizaCA3" +
                                                                ",AN_VlrAutorizaAJ1=@AN_VlrAutorizaAJ1" +
                                                                ",AN_VlrAutorizaAJ2=@AN_VlrAutorizaAJ2" +
                                                                ",AN_VlrAutorizaV11=@AN_VlrAutorizaV11" +
                                                                ",AN_VlrAutorizaV12=@AN_VlrAutorizaV12" +
                                                                ",AN_VlrAutorizaV21=@AN_VlrAutorizaV21" +
                                                                ",AN_VlrAutorizaV22=@AN_VlrAutorizaV22" +
                                                                ",AN_VlrAutorizaV31=@AN_VlrAutorizaV31" +
                                                                ",AN_VlrAutorizaV32=@AN_VlrAutorizaV32" +
                                                                ",AN_VlrAutFinSOL=@AN_VlrAutFinSOL" +
                                                                ",AN_VlrAutFinCA2=@AN_VlrAutFinCA2" +
                                                                ",AN_VlrAutFinAJ1=@AN_VlrAutFinAJ1" +
                                                                ",AN_VlrAutFinV12=@AN_VlrAutFinV12" +
                                                                ",AN_VlrAutFinV21=@AN_VlrAutFinV21" +
                                                                ",AN_VlrAutFinV31=@AN_VlrAutFinV31" +
                                                                ",AN_AltNoComAJ1=@AN_AltNoComAJ1" +
                                                                ",AN_AltNoComAJ2=@AN_AltNoComAJ2" +
                                                                ",AN_AltNoComV12=@AN_AltNoComV12" +
                                                                ",AN_AltNoComV21=@AN_AltNoComV21" +
                                                                ",AN_AltNoComV22=@AN_AltNoComV22" +
                                                                ",AN_AltNoComV31=@AN_AltNoComV31" +
                                                                ",AN_AltNoComV32=@AN_AltNoComV32" +
                                                                ",Alternativa8=@Alternativa8" +
                                                                ",FactorAlt8=@FactorAlt8" +
                                                                ",MontoAlt8=@MontoAlt8" +
                                                                ",GarantiaAlt8=@GarantiaAlt8" +
                                                                ",IngrMinSopDeu1=@IngrMinSopDeu1" +
                                                                ",IngrMinSopDeu2=@IngrMinSopDeu2" +
                                                                ",IngrMinSopDeu3=@IngrMinSopDeu3" +
                                                                ",IngrMinSopDeu4=@IngrMinSopDeu4" +
                                                                ",IngrMinSopDeu5=@IngrMinSopDeu5" +
                                                                ",IngrMinSopDeu6=@IngrMinSopDeu6" +
                                                                ",IngrMinSopDeu7=@IngrMinSopDeu7" +
                                                                ",IngrMinSopDeu8=@IngrMinSopDeu8" +
                                                                ",IngrMinSopCon1=@IngrMinSopCon1" +
                                                                ",IngrMinSopCon2=@IngrMinSopCon2" +
                                                                ",IngrMinSopCon3=@IngrMinSopCon3" +
                                                                ",IngrMinSopCon4=@IngrMinSopCon4" +
                                                                ",IngrMinSopCon5=@IngrMinSopCon5" +
                                                                ",IngrMinSopCon6=@IngrMinSopCon6" +
                                                                ",IngrMinSopCon7=@IngrMinSopCon7" +
                                                                ",IngrMinSopCon8=@IngrMinSopCon8" +
                                                                ",FechaFirmaDocumento=@FechaFirmaDocumento"+
                                                                ",AccionSolicitud=@AccionSolicitud"+
                                                                ",Observacion=@Observacion" +
                                                                ",PF_Plazo=@PF_Plazo"+
                                                                ",PF_PorMaxEstadoActual=@PF_PorMaxEstadoActual"+
                                                                ",porMaximo=@porMaximo"+
                                                                ",VlrGarantia=@VlrGarantia"+
                                                                ",AccionSolicitud1=@AccionSolicitud1" +
                                                                ",AccionSolicitud2=@AccionSolicitud2"+
                                                                ",AccionSolicitud3=@AccionSolicitud3"+
                                                                ",PF_PlazoFinal1=@PF_PlazoFinal1"+
                                                                ",PF_PlazoFinal2=@PF_PlazoFinal2"+
                                                                ",PF_PlazoFinal3=@PF_PlazoFinal3"+
                                                                ",PF_VlrMontoFirma1=@PF_VlrMontoFirma1"+
                                                                ",PF_VlrMontoFirma2=@PF_VlrMontoFirma2"+
                                                                ",PF_VlrMontoFirma3=@PF_VlrMontoFirma3"+
                                                                ",PF_TasaPerfilOBL=@PF_TasaPerfilOBL"+
                                                                ",PF_TasaFirma3OBL=@PF_TasaFirma3OBL"+
                                                                ",PF_PorMaxFirma1=@PF_PorMaxFirma1" +
                                                                ",PF_PorMaxFirma2=@PF_PorMaxFirma2" +
                                                                ",PF_PorMaxFirma3=@PF_PorMaxFirma3" +
                                                                ",PF_VlrGarantiaPerfil=@PF_VlrGarantiaPerfil"+
                                                                ",PF_VlrGarantiaFirma1=@PF_VlrGarantiaFirma1" +
                                                                ",PF_VlrGarantiaFirma2=@PF_VlrGarantiaFirma2"+
                                                                ",PF_VlrGarantiaFirma3=@PF_VlrGarantiaFirma3" +
                                                                ",eg_ccAseguradora=@eg_ccAseguradora" +
                                                                " WHERE  Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CubriendoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@FinanciaPOLInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@VlrMensualSV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoPrenda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PrefijoPrenda", SqlDbType.VarChar, 5);
                mySqlCommandSel.Parameters.Add("@NumeroPrenda", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Registro", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CedulaReg", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NombreREG", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@DireccionREG", SqlDbType.VarChar, 100);
                mySqlCommandSel.Parameters.Add("@Alternativa1", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@Alternativa2", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@Alternativa3", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@Alternativa4", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@Alternativa5", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@Alternativa6", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@Alternativa7", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@FactorAlt1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorAlt7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGtiaEvaluacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoMaximo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EstimadoSeguros", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MaxFinanciacionAut", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Plazo", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EstimadoObl", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaFin", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaSeg", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaTotal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Calificacion", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Revision", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@CargoResp", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@Firma1Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Firma2Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Firma3Ind", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@UsuarioResp", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioFirma1", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioFirma2", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UsuarioFirma3", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaFirmaResp", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFirma1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFirma2", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFirma3", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaDatacredito", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaLegalizacion", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaDesembolso", SqlDbType.SmallDateTime);

                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@CartaAprobDirInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaAprobDocInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartapreAprobInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaNoViableInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaRevocaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaRatificaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CartaAprobDirUsu", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaAprobDocUsu", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartapreAprobUsu", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaNoViableUsu", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaRevocaUsu", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaRatificaUsu", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CartaAprobDirFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaAprobDocFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartapreAprobFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaNoViableFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaRevocaFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CartaRatificaFecha", SqlDbType.SmallDateTime);

                mySqlCommandSel.Parameters.Add("@PerfilUsuario", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PerfilFecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EstActualFactor", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SMMLV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorIngrPagoCtas", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyosDEU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyosCON", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngrDispApoyos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciaFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguroFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaTotalFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaApoyosDifFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngDispApoyosFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_ReqSopIngrIndDEU", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_ReqSopIngrIndCON", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu1FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu2FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu3SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu3INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeu3FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqDeuFinFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon1FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2AJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2INC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqCon2FIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinAJU", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinINC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_IngReqConFinFIN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_Cuantia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaTablEva1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_FactTablEva", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaTablEva2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaPonderada", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@PF_PolCubriendoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PF_VlrMinimoGar", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMinimoFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMinimoFirma3", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@PF_CtaFinanciacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_CtaSeguros", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorEstimado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Verificacion1", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Verificacion2", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@Verificacion3", SqlDbType.VarChar, 50);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1CA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1CA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1CA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1AJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1AJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado1V32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2SOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2CA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2CA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2CA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2AJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2AJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_Resultado2V32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrVenta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrFasecolda", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrCtaEvaV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrMinLim", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrMaxLim", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrSolicitado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAlternV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaInicial", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrIncremV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CtaIniAjuV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinSoli", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinMaxV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_porFinAltV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumParGarV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoSOL", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoCA1", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoCA2", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoCA3", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoAJ1", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoAJ2", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV11", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV12", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV21", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV22", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV31", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_CapPagoV32", SqlDbType.VarChar, 30);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCICA1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCICA2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCICA3", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_PuedeIncCIV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxSOL", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxCA1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxCA2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxCA3", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumPorMaxV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinCA1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinCA2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinCA3", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinAJ1", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinAJ2", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV11", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV12", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV21", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV22", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV31", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumMtoMinV32", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroSOL", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroCA1", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroCA2", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroCA3", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroAJ1", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroAJ2", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV11", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV12", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV21", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV22", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV31", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_CumOtroV32", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaCA1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaCA3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaAJ2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV22", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutorizaV32", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinCA2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinAJ1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinV12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinV21", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_VlrAutFinV31", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComAJ1", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComAJ2", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV12", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV21", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV22", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV31", SqlDbType.VarChar, 2);
                mySqlCommandSel.Parameters.Add("@AN_AltNoComV32", SqlDbType.VarChar, 2);


                mySqlCommandSel.Parameters.Add("@Alternativa8", SqlDbType.VarChar, 15);
                mySqlCommandSel.Parameters.Add("@FactorAlt8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MontoAlt8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@GarantiaAlt8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopDeu8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IngrMinSopCon8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaFirmaDocumento", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@AccionSolicitud", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@PF_Plazo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxEstadoActual", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@porMaximo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrGarantia", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@AccionSolicitud1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AccionSolicitud2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AccionSolicitud3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal1", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_PlazoFinal3", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFirma1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrMontoFirma3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaPerfilOBL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_TasaFirma3OBL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFirma1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_PorMaxFirma3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaPerfil", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaFirma1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaFirma2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PF_VlrGarantiaFirma3", SqlDbType.Decimal);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = Datos.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@CubriendoInd"].Value = Datos.CubriendoInd.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = Datos.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@FinanciaPOLInd"].Value = Datos.FinanciaPOLInd.Value;
                mySqlCommandSel.Parameters["@VlrMensualSV"].Value = Datos.VlrMensualSV.Value;
                mySqlCommandSel.Parameters["@TipoPrenda"].Value = Datos.TipoPrenda.Value;
                mySqlCommandSel.Parameters["@PrefijoPrenda"].Value = Datos.PrefijoPrenda.Value;
                mySqlCommandSel.Parameters["@NumeroPrenda"].Value = Datos.NumeroPrenda.Value;
                mySqlCommandSel.Parameters["@Registro"].Value = Datos.Registro.Value;
                mySqlCommandSel.Parameters["@CedulaReg"].Value = Datos.CedulaReg.Value;
                mySqlCommandSel.Parameters["@NombreREG"].Value = Datos.NombreREG.Value;
                mySqlCommandSel.Parameters["@DireccionREG"].Value = Datos.DireccionREG.Value;
                mySqlCommandSel.Parameters["@Alternativa1"].Value = Datos.Alternativa1.Value;
                mySqlCommandSel.Parameters["@Alternativa2"].Value = Datos.Alternativa2.Value;
                mySqlCommandSel.Parameters["@Alternativa3"].Value = Datos.Alternativa3.Value;
                mySqlCommandSel.Parameters["@Alternativa4"].Value = Datos.Alternativa4.Value;
                mySqlCommandSel.Parameters["@Alternativa5"].Value = Datos.Alternativa5.Value;
                mySqlCommandSel.Parameters["@Alternativa6"].Value = Datos.Alternativa6.Value;
                mySqlCommandSel.Parameters["@Alternativa7"].Value = Datos.Alternativa7.Value;
                mySqlCommandSel.Parameters["@FactorAlt1"].Value = Datos.FactorAlt1.Value;
                mySqlCommandSel.Parameters["@FactorAlt2"].Value = Datos.FactorAlt2.Value;
                mySqlCommandSel.Parameters["@FactorAlt3"].Value = Datos.FactorAlt3.Value;
                mySqlCommandSel.Parameters["@FactorAlt4"].Value = Datos.FactorAlt4.Value;
                mySqlCommandSel.Parameters["@FactorAlt5"].Value = Datos.FactorAlt5.Value;
                mySqlCommandSel.Parameters["@FactorAlt6"].Value = Datos.FactorAlt6.Value;
                mySqlCommandSel.Parameters["@FactorAlt7"].Value = Datos.FactorAlt7.Value;
                mySqlCommandSel.Parameters["@MontoAlt1"].Value = Datos.MontoAlt1.Value;
                mySqlCommandSel.Parameters["@MontoAlt2"].Value = Datos.MontoAlt2.Value;
                mySqlCommandSel.Parameters["@MontoAlt3"].Value = Datos.MontoAlt3.Value;
                mySqlCommandSel.Parameters["@MontoAlt4"].Value = Datos.MontoAlt4.Value;
                mySqlCommandSel.Parameters["@MontoAlt5"].Value = Datos.MontoAlt5.Value;
                mySqlCommandSel.Parameters["@MontoAlt6"].Value = Datos.MontoAlt6.Value;
                mySqlCommandSel.Parameters["@MontoAlt7"].Value = Datos.MontoAlt7.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt1"].Value = Datos.GarantiaAlt1.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt2"].Value = Datos.GarantiaAlt2.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt3"].Value = Datos.GarantiaAlt3.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt4"].Value = Datos.GarantiaAlt4.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt5"].Value = Datos.GarantiaAlt5.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt6"].Value = Datos.GarantiaAlt6.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt7"].Value = Datos.GarantiaAlt7.Value;
                mySqlCommandSel.Parameters["@VlrGtiaEvaluacion"].Value = Datos.VlrGtiaEvaluacion.Value;
                mySqlCommandSel.Parameters["@MontoMaximo"].Value = Datos.MontoMaximo.Value;
                mySqlCommandSel.Parameters["@EstimadoSeguros"].Value = Datos.EstimadoSeguros.Value;
                mySqlCommandSel.Parameters["@MaxFinanciacionAut"].Value = Datos.MaxFinanciacionAut.Value;
                mySqlCommandSel.Parameters["@Plazo"].Value = Datos.Plazo.Value;
                mySqlCommandSel.Parameters["@EstimadoObl"].Value = Datos.EstimadoObl.Value;
                mySqlCommandSel.Parameters["@CuotaFin"].Value = Datos.CuotaFin.Value;
                mySqlCommandSel.Parameters["@CuotaSeg"].Value = Datos.CuotaSeg.Value;
                mySqlCommandSel.Parameters["@CuotaTotal"].Value = Datos.CuotaTotal.Value;
                mySqlCommandSel.Parameters["@Estado"].Value = Datos.Estado.Value;
                mySqlCommandSel.Parameters["@Calificacion"].Value = Datos.Calificacion.Value;
                mySqlCommandSel.Parameters["@Revision"].Value = Datos.Revision.Value;
                mySqlCommandSel.Parameters["@CargoResp"].Value = Datos.CargoResp.Value;
                mySqlCommandSel.Parameters["@Firma1Ind"].Value = Datos.Firma1Ind.Value;
                mySqlCommandSel.Parameters["@Firma2Ind"].Value = Datos.Firma2Ind.Value;
                mySqlCommandSel.Parameters["@Firma3Ind"].Value = Datos.Firma3Ind.Value;
                mySqlCommandSel.Parameters["@UsuarioResp"].Value = Datos.UsuarioResp.Value;
                mySqlCommandSel.Parameters["@UsuarioFirma1"].Value = Datos.UsuarioFirma1.Value;
                mySqlCommandSel.Parameters["@UsuarioFirma2"].Value = Datos.UsuarioFirma2.Value;
                mySqlCommandSel.Parameters["@UsuarioFirma3"].Value = Datos.UsuarioFirma3.Value;
                mySqlCommandSel.Parameters["@FechaFirmaResp"].Value = Datos.FechaFirmaResp.Value;
                mySqlCommandSel.Parameters["@FechaFirma1"].Value = Datos.FechaFirma1.Value;
                mySqlCommandSel.Parameters["@FechaFirma2"].Value = Datos.FechaFirma2.Value;
                mySqlCommandSel.Parameters["@FechaFirma3"].Value = Datos.FechaFirma3.Value;
                mySqlCommandSel.Parameters["@FechaDatacredito"].Value = Datos.FechaDatacredito.Value;
                mySqlCommandSel.Parameters["@FechaLegalizacion"].Value = Datos.FechaLegalizacion.Value;
                mySqlCommandSel.Parameters["@FechaDesembolso"].Value = Datos.FechaDesembolso.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;

                mySqlCommandSel.Parameters["@CartaAprobDirInd"].Value = Datos.CartaAprobDirInd.Value;
                mySqlCommandSel.Parameters["@CartaAprobDocInd"].Value = Datos.CartaAprobDocInd.Value;
                mySqlCommandSel.Parameters["@CartapreAprobInd"].Value = Datos.CartapreAprobInd.Value;
                mySqlCommandSel.Parameters["@CartaNoViableInd"].Value = Datos.CartaNoViableInd.Value;
                mySqlCommandSel.Parameters["@CartaRevocaInd"].Value = Datos.CartaRevocaInd.Value;
                mySqlCommandSel.Parameters["@CartaRatificaInd"].Value = Datos.CartaRatificaInd.Value;
                mySqlCommandSel.Parameters["@CartaAprobDirUsu"].Value = Datos.CartaAprobDirUsu.Value;
                mySqlCommandSel.Parameters["@CartaAprobDocUsu"].Value = Datos.CartaAprobDocUsu.Value;
                mySqlCommandSel.Parameters["@CartapreAprobUsu"].Value = Datos.CartapreAprobUsu.Value;
                mySqlCommandSel.Parameters["@CartaNoViableUsu"].Value = Datos.CartaNoViableUsu.Value;
                mySqlCommandSel.Parameters["@CartaRevocaUsu"].Value = Datos.CartaRevocaUsu.Value;
                mySqlCommandSel.Parameters["@CartaRatificaUsu"].Value = Datos.CartaRatificaUsu.Value;
                mySqlCommandSel.Parameters["@CartaAprobDirFecha"].Value = Datos.CartaAprobDirFecha.Value;
                mySqlCommandSel.Parameters["@CartaAprobDocFecha"].Value = Datos.CartaAprobDocFecha.Value;
                mySqlCommandSel.Parameters["@CartapreAprobFecha"].Value = Datos.CartapreAprobFecha.Value;
                mySqlCommandSel.Parameters["@CartaNoViableFecha"].Value = Datos.CartaNoViableFecha.Value;
                mySqlCommandSel.Parameters["@CartaRevocaFecha"].Value = Datos.CartaRevocaFecha.Value;
                mySqlCommandSel.Parameters["@CartaRatificaFecha"].Value = Datos.CartaRatificaFecha.Value;

                mySqlCommandSel.Parameters["@PerfilUsuario"].Value = Datos.PerfilUsuario.Value;
                mySqlCommandSel.Parameters["@PerfilFecha"].Value = Datos.PerfilFecha.Value;
                mySqlCommandSel.Parameters["@EstActualFactor"].Value = Datos.EstActualFactor.Value;

                mySqlCommandSel.Parameters["@VlrSolicitado"].Value = Datos.VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@SMMLV"].Value = Datos.SMMLV.Value;
                mySqlCommandSel.Parameters["@PF_PorIngrPagoCtas"].Value = Datos.PF_PorIngrPagoCtas.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyosDEU"].Value = Datos.PF_IngrDispApoyosDEU.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyosCON"].Value = Datos.PF_IngrDispApoyosCON.Value;
                mySqlCommandSel.Parameters["@PF_IngrDispApoyos"].Value = Datos.PF_IngrDispApoyos.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoSOL"].Value = Datos.PF_VlrMontoSOL.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoAJU"].Value = Datos.PF_VlrMontoAJU.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoINC"].Value = Datos.PF_VlrMontoINC.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFIN"].Value = Datos.PF_VlrMontoFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaSOL"].Value = Datos.PF_CtaFinanciaSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaAJU"].Value = Datos.PF_CtaFinanciaAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaINC"].Value = Datos.PF_CtaFinanciaINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciaFIN"].Value = Datos.PF_CtaFinanciaFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroSOL"].Value = Datos.PF_CtaSeguroSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroAJU"].Value = Datos.PF_CtaSeguroAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroINC"].Value = Datos.PF_CtaSeguroINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguroFIN"].Value = Datos.PF_CtaSeguroFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalSOL"].Value = Datos.PF_CtaTotalSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalAJU"].Value = Datos.PF_CtaTotalAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalINC"].Value = Datos.PF_CtaTotalINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaTotalFIN"].Value = Datos.PF_CtaTotalFIN.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifSOL"].Value = Datos.PF_CtaApoyosDifSOL.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifAJU"].Value = Datos.PF_CtaApoyosDifAJU.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifINC"].Value = Datos.PF_CtaApoyosDifINC.Value;
                mySqlCommandSel.Parameters["@PF_CtaApoyosDifFIN"].Value = Datos.PF_CtaApoyosDifFIN.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosSOL"].Value = Datos.PF_IngDispApoyosSOL.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosAJU"].Value = Datos.PF_IngDispApoyosAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosINC"].Value = Datos.PF_IngDispApoyosINC.Value;
                mySqlCommandSel.Parameters["@PF_IngDispApoyosFIN"].Value = Datos.PF_IngDispApoyosFIN.Value;
                mySqlCommandSel.Parameters["@PF_ReqSopIngrIndDEU"].Value = Datos.PF_ReqSopIngrIndDEU.Value;
                mySqlCommandSel.Parameters["@PF_ReqSopIngrIndCON"].Value = Datos.PF_ReqSopIngrIndCON.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1SOL"].Value = Datos.PF_IngReqDeu1SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1AJU"].Value = Datos.PF_IngReqDeu1AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1INC"].Value = Datos.PF_IngReqDeu1INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu1FIN"].Value = Datos.PF_IngReqDeu1FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2SOL"].Value = Datos.PF_IngReqDeu2SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2AJU"].Value = Datos.PF_IngReqDeu2AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2INC"].Value = Datos.PF_IngReqDeu2INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu2FIN"].Value = Datos.PF_IngReqDeu2FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu3SOL"].Value = Datos.PF_IngReqDeu3SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuAJU"].Value = Datos.PF_IngReqDeuAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu3INC"].Value = Datos.PF_IngReqDeu3INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeu3FIN"].Value = Datos.PF_IngReqDeu3FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinSOL"].Value = Datos.PF_IngReqDeuFinSOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinAJU"].Value = Datos.PF_IngReqDeuFinAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinINC"].Value = Datos.PF_IngReqDeuFinINC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqDeuFinFIN"].Value = Datos.PF_IngReqDeuFinFIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1SOL"].Value = Datos.PF_IngReqCon1SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1AJU"].Value = Datos.PF_IngReqCon1AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1INC"].Value = Datos.PF_IngReqCon1INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon1FIN"].Value = Datos.PF_IngReqCon1FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2SOL"].Value = Datos.PF_IngReqCon2SOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2AJU"].Value = Datos.PF_IngReqCon2AJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2INC"].Value = Datos.PF_IngReqCon2INC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqCon2FIN"].Value = Datos.PF_IngReqCon2FIN.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinSOL"].Value = Datos.PF_IngReqConFinSOL.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinAJU"].Value = Datos.PF_IngReqConFinAJU.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinINC"].Value = Datos.PF_IngReqConFinINC.Value;
                mySqlCommandSel.Parameters["@PF_IngReqConFinFIN"].Value = Datos.PF_IngReqConFinFIN.Value;
                mySqlCommandSel.Parameters["@PF_Cuantia"].Value = Datos.PF_Cuantia.Value;
                mySqlCommandSel.Parameters["@PF_TasaTablEva1"].Value = Datos.PF_TasaTablEva1.Value;
                mySqlCommandSel.Parameters["@PF_FactTablEva"].Value = Datos.PF_FactTablEva.Value;
                mySqlCommandSel.Parameters["@PF_TasaTablEva2"].Value = Datos.PF_TasaTablEva2.Value;
                mySqlCommandSel.Parameters["@PF_TasaPonderada"].Value = Datos.PF_TasaPonderada.Value;

                mySqlCommandSel.Parameters["@PF_PolCubriendoInd"].Value = Datos.PF_PolCubriendoInd.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal"].Value = Datos.PF_PlazoFinal.Value;
                mySqlCommandSel.Parameters["@PF_VlrMinimoGar"].Value = Datos.PF_VlrMinimoGar.Value;
                mySqlCommandSel.Parameters["@PF_VlrMinimoFirma2"].Value = Datos.PF_VlrMinimoFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrMinimoFirma3"].Value = Datos.PF_VlrMinimoFirma3.Value;
                mySqlCommandSel.Parameters["@PF_CtaFinanciacion"].Value = Datos.PF_CtaFinanciacion.Value;
                mySqlCommandSel.Parameters["@PF_CtaSeguros"].Value = Datos.PF_CtaSeguros.Value;
                mySqlCommandSel.Parameters["@PF_PorEstimado"].Value = Datos.PF_PorEstimado.Value;
                mySqlCommandSel.Parameters["@Verificacion1"].Value = Datos.Verificacion1.Value;
                mySqlCommandSel.Parameters["@Verificacion2"].Value = Datos.Verificacion2.Value;
                mySqlCommandSel.Parameters["@Verificacion3"].Value = Datos.Verificacion3.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1SOL"].Value = Datos.AN_Resultado1SOL.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1CA1"].Value = Datos.AN_Resultado1CA1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1CA2"].Value = Datos.AN_Resultado1CA2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1CA3"].Value = Datos.AN_Resultado1CA3.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1AJ1"].Value = Datos.AN_Resultado1AJ1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1AJ2"].Value = Datos.AN_Resultado1AJ2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V11"].Value = Datos.AN_Resultado1V11.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V12"].Value = Datos.AN_Resultado1V12.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V21"].Value = Datos.AN_Resultado1V21.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V22"].Value = Datos.AN_Resultado1V22.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V31"].Value = Datos.AN_Resultado1V31.Value;
                mySqlCommandSel.Parameters["@AN_Resultado1V32"].Value = Datos.AN_Resultado1V32.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2SOL"].Value = Datos.AN_Resultado2SOL.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2CA1"].Value = Datos.AN_Resultado2CA1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2CA2"].Value = Datos.AN_Resultado2CA2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2CA3"].Value = Datos.AN_Resultado2CA3.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2AJ1"].Value = Datos.AN_Resultado2AJ1.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2AJ2"].Value = Datos.AN_Resultado2AJ2.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V11"].Value = Datos.AN_Resultado2V11.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V12"].Value = Datos.AN_Resultado2V12.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V21"].Value = Datos.AN_Resultado2V21.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V22"].Value = Datos.AN_Resultado2V22.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V31"].Value = Datos.AN_Resultado2V31.Value;
                mySqlCommandSel.Parameters["@AN_Resultado2V32"].Value = Datos.AN_Resultado2V32.Value;
                mySqlCommandSel.Parameters["@AN_VlrVenta"].Value = Datos.AN_VlrVenta.Value;
                mySqlCommandSel.Parameters["@AN_VlrFasecolda"].Value = Datos.AN_VlrFasecolda.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaSOL"].Value = Datos.AN_VlrCtaEvaSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaCA1"].Value = Datos.AN_VlrCtaEvaCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaCA2"].Value = Datos.AN_VlrCtaEvaCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaCA3"].Value = Datos.AN_VlrCtaEvaCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaAJ1"].Value = Datos.AN_VlrCtaEvaAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaAJ2"].Value = Datos.AN_VlrCtaEvaAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV11"].Value = Datos.AN_VlrCtaEvaV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV12"].Value = Datos.AN_VlrCtaEvaV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV21"].Value = Datos.AN_VlrCtaEvaV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV22"].Value = Datos.AN_VlrCtaEvaV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV31"].Value = Datos.AN_VlrCtaEvaV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrCtaEvaV32"].Value = Datos.AN_VlrCtaEvaV32.Value;
                mySqlCommandSel.Parameters["@AN_VlrMinLim"].Value = Datos.AN_VlrMinLim.Value;
                mySqlCommandSel.Parameters["@AN_VlrMaxLim"].Value = Datos.AN_VlrMaxLim.Value;
                mySqlCommandSel.Parameters["@AN_VlrSolicitado"].Value = Datos.AN_VlrSolicitado.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternCA1"].Value = Datos.AN_VlrAlternCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternCA2"].Value = Datos.AN_VlrAlternCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternCA3"].Value = Datos.AN_VlrAlternCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternAJ1"].Value = Datos.AN_VlrAlternAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternAJ2"].Value = Datos.AN_VlrAlternAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV11"].Value = Datos.AN_VlrAlternV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV12"].Value = Datos.AN_VlrAlternV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV21"].Value = Datos.AN_VlrAlternV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV22"].Value = Datos.AN_VlrAlternV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV31"].Value = Datos.AN_VlrAlternV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrAlternV32"].Value = Datos.AN_VlrAlternV32.Value;
                mySqlCommandSel.Parameters["@AN_CtaInicial"].Value = Datos.AN_CtaInicial.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremSOL"].Value = Datos.AN_VlrIncremSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremCA1"].Value = Datos.AN_VlrIncremCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremCA2"].Value = Datos.AN_VlrIncremCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremCA3"].Value = Datos.AN_VlrIncremCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremAJ1"].Value = Datos.AN_VlrIncremAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremAJ2"].Value = Datos.AN_VlrIncremAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV11"].Value = Datos.AN_VlrIncremV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV12"].Value = Datos.AN_VlrIncremV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV21"].Value = Datos.AN_VlrIncremV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV22"].Value = Datos.AN_VlrIncremV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV31"].Value = Datos.AN_VlrIncremV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrIncremV32"].Value = Datos.AN_VlrIncremV32.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuCA1"].Value = Datos.AN_CtaIniAjuCA1.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuCA2"].Value = Datos.AN_CtaIniAjuCA2.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuCA3"].Value = Datos.AN_CtaIniAjuCA3.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuAJ1"].Value = Datos.AN_CtaIniAjuAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuAJ2"].Value = Datos.AN_CtaIniAjuAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV11"].Value = Datos.AN_CtaIniAjuV11.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV12"].Value = Datos.AN_CtaIniAjuV12.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV21"].Value = Datos.AN_CtaIniAjuV21.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV22"].Value = Datos.AN_CtaIniAjuV22.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV31"].Value = Datos.AN_CtaIniAjuV31.Value;
                mySqlCommandSel.Parameters["@AN_CtaIniAjuV32"].Value = Datos.AN_CtaIniAjuV32.Value;
                mySqlCommandSel.Parameters["@AN_porFinSoli"].Value = Datos.AN_porFinSoli.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxSOL"].Value = Datos.AN_porFinMaxSOL.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxCA1"].Value = Datos.AN_porFinMaxCA1.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxCA2"].Value = Datos.AN_porFinMaxCA2.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxCA3"].Value = Datos.AN_porFinMaxCA3.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxAJ1"].Value = Datos.AN_porFinMaxAJ1.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxAJ2"].Value = Datos.AN_porFinMaxAJ2.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV11"].Value = Datos.AN_porFinMaxV11.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV12"].Value = Datos.AN_porFinMaxV12.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV21"].Value = Datos.AN_porFinMaxV21.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV22"].Value = Datos.AN_porFinMaxV22.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV31"].Value = Datos.AN_porFinMaxV31.Value;
                mySqlCommandSel.Parameters["@AN_porFinMaxV32"].Value = Datos.AN_porFinMaxV32.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltCA1"].Value = Datos.AN_porFinAltCA1.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltCA2"].Value = Datos.AN_porFinAltCA2.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltCA3"].Value = Datos.AN_porFinAltCA3.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltAJ1"].Value = Datos.AN_porFinAltAJ1.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltAJ2"].Value = Datos.AN_porFinAltAJ2.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV11"].Value = Datos.AN_porFinAltV11.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV12"].Value = Datos.AN_porFinAltV12.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV21"].Value = Datos.AN_porFinAltV21.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV22"].Value = Datos.AN_porFinAltV22.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV31"].Value = Datos.AN_porFinAltV31.Value;
                mySqlCommandSel.Parameters["@AN_porFinAltV32"].Value = Datos.AN_porFinAltV32.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarAJ1"].Value = Datos.AN_CumParGarAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarAJ2"].Value = Datos.AN_CumParGarAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV11"].Value = Datos.AN_CumParGarV11.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV12"].Value = Datos.AN_CumParGarV12.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV21"].Value = Datos.AN_CumParGarV21.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV22"].Value = Datos.AN_CumParGarV22.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV31"].Value = Datos.AN_CumParGarV31.Value;
                mySqlCommandSel.Parameters["@AN_CumParGarV32"].Value = Datos.AN_CumParGarV32.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoSOL"].Value = Datos.AN_CapPagoSOL.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoCA1"].Value = Datos.AN_CapPagoCA1.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoCA2"].Value = Datos.AN_CapPagoCA2.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoCA3"].Value = Datos.AN_CapPagoCA3.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoAJ1"].Value = Datos.AN_CapPagoAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoAJ2"].Value = Datos.AN_CapPagoAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV11"].Value = Datos.AN_CapPagoV11.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV12"].Value = Datos.AN_CapPagoV12.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV21"].Value = Datos.AN_CapPagoV21.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV22"].Value = Datos.AN_CapPagoV22.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV31"].Value = Datos.AN_CapPagoV31.Value;
                mySqlCommandSel.Parameters["@AN_CapPagoV32"].Value = Datos.AN_CapPagoV32.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCICA1"].Value = Datos.AN_PuedeIncCICA1.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCICA2"].Value = Datos.AN_PuedeIncCICA2.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCICA3"].Value = Datos.AN_PuedeIncCICA3.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIAJ1"].Value = Datos.AN_PuedeIncCIAJ1.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIAJ2"].Value = Datos.AN_PuedeIncCIAJ2.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV11"].Value = Datos.AN_PuedeIncCIV11.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV12"].Value = Datos.AN_PuedeIncCIV12.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV21"].Value = Datos.AN_PuedeIncCIV21.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV22"].Value = Datos.AN_PuedeIncCIV22.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV31"].Value = Datos.AN_PuedeIncCIV31.Value;
                mySqlCommandSel.Parameters["@AN_PuedeIncCIV32"].Value = Datos.AN_PuedeIncCIV32.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxSOL"].Value = Datos.AN_CumPorMaxSOL.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxCA1"].Value = Datos.AN_CumPorMaxCA1.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxCA2"].Value = Datos.AN_CumPorMaxCA2.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxCA3"].Value = Datos.AN_CumPorMaxCA3.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxAJ1"].Value = Datos.AN_CumPorMaxAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxAJ2"].Value = Datos.AN_CumPorMaxAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV11"].Value = Datos.AN_CumPorMaxV11.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV12"].Value = Datos.AN_CumPorMaxV12.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV21"].Value = Datos.AN_CumPorMaxV21.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV22"].Value = Datos.AN_CumPorMaxV22.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV31"].Value = Datos.AN_CumPorMaxV31.Value;
                mySqlCommandSel.Parameters["@AN_CumPorMaxV32"].Value = Datos.AN_CumPorMaxV32.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinCA1"].Value = Datos.AN_CumMtoMinCA1.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinCA2"].Value = Datos.AN_CumMtoMinCA2.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinCA3"].Value = Datos.AN_CumMtoMinCA3.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinAJ1"].Value = Datos.AN_CumMtoMinAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinAJ2"].Value = Datos.AN_CumMtoMinAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV11"].Value = Datos.AN_CumMtoMinV11.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV12"].Value = Datos.AN_CumMtoMinV12.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV21"].Value = Datos.AN_CumMtoMinV21.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV22"].Value = Datos.AN_CumMtoMinV22.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV31"].Value = Datos.AN_CumMtoMinV31.Value;
                mySqlCommandSel.Parameters["@AN_CumMtoMinV32"].Value = Datos.AN_CumMtoMinV32.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroSOL"].Value = Datos.AN_CumOtroSOL.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroCA1"].Value = Datos.AN_CumOtroCA1.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroCA2"].Value = Datos.AN_CumOtroCA2.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroCA3"].Value = Datos.AN_CumOtroCA3.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroAJ1"].Value = Datos.AN_CumOtroAJ1.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroAJ2"].Value = Datos.AN_CumOtroAJ2.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV11"].Value = Datos.AN_CumOtroV11.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV12"].Value = Datos.AN_CumOtroV12.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV21"].Value = Datos.AN_CumOtroV21.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV22"].Value = Datos.AN_CumOtroV22.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV31"].Value = Datos.AN_CumOtroV31.Value;
                mySqlCommandSel.Parameters["@AN_CumOtroV32"].Value = Datos.AN_CumOtroV32.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaSOL"].Value = Datos.AN_VlrAutorizaSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaCA1"].Value = Datos.AN_VlrAutorizaCA1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaCA2"].Value = Datos.AN_VlrAutorizaCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaCA3"].Value = Datos.AN_VlrAutorizaCA3.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaAJ1"].Value = Datos.AN_VlrAutorizaAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaAJ2"].Value = Datos.AN_VlrAutorizaAJ2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV11"].Value = Datos.AN_VlrAutorizaV11.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV12"].Value = Datos.AN_VlrAutorizaV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV21"].Value = Datos.AN_VlrAutorizaV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV22"].Value = Datos.AN_VlrAutorizaV22.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV31"].Value = Datos.AN_VlrAutorizaV31.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutorizaV32"].Value = Datos.AN_VlrAutorizaV32.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinSOL"].Value = Datos.AN_VlrAutFinSOL.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinCA2"].Value = Datos.AN_VlrAutFinCA2.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinAJ1"].Value = Datos.AN_VlrAutFinAJ1.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinV12"].Value = Datos.AN_VlrAutFinV12.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinV21"].Value = Datos.AN_VlrAutFinV21.Value;
                mySqlCommandSel.Parameters["@AN_VlrAutFinV31"].Value = Datos.AN_VlrAutFinV31.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComAJ1"].Value = Datos.AN_AltNoComAJ1.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComAJ2"].Value = Datos.AN_AltNoComAJ2.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV12"].Value = Datos.AN_AltNoComV12.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV21"].Value = Datos.AN_AltNoComV21.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV22"].Value = Datos.AN_AltNoComV22.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV31"].Value = Datos.AN_AltNoComV31.Value;
                mySqlCommandSel.Parameters["@AN_AltNoComV32"].Value = Datos.AN_AltNoComV32.Value;

                mySqlCommandSel.Parameters["@Alternativa8"].Value = Datos.Alternativa8.Value;
                mySqlCommandSel.Parameters["@FactorAlt8"].Value = Datos.FactorAlt8.Value;
                mySqlCommandSel.Parameters["@MontoAlt8"].Value = Datos.MontoAlt8.Value;
                mySqlCommandSel.Parameters["@GarantiaAlt8"].Value = Datos.GarantiaAlt8.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu1"].Value = Datos.IngrMinSopDeu1.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu2"].Value = Datos.IngrMinSopDeu2.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu3"].Value = Datos.IngrMinSopDeu3.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu4"].Value = Datos.IngrMinSopDeu4.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu5"].Value = Datos.IngrMinSopDeu5.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu6"].Value = Datos.IngrMinSopDeu6.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu7"].Value = Datos.IngrMinSopDeu7.Value;
                mySqlCommandSel.Parameters["@IngrMinSopDeu8"].Value = Datos.IngrMinSopDeu8.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon1"].Value = Datos.IngrMinSopCon1.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon2"].Value = Datos.IngrMinSopCon2.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon3"].Value = Datos.IngrMinSopCon3.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon4"].Value = Datos.IngrMinSopCon4.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon5"].Value = Datos.IngrMinSopCon5.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon6"].Value = Datos.IngrMinSopCon6.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon7"].Value = Datos.IngrMinSopCon7.Value;
                mySqlCommandSel.Parameters["@IngrMinSopCon8"].Value = Datos.IngrMinSopCon8.Value;
                mySqlCommandSel.Parameters["@FechaFirmaDocumento"].Value = Datos.FechaFirmaDocumento.Value;
                mySqlCommandSel.Parameters["@AccionSolicitud"].Value = Datos.AccionSolicitud.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = Datos.Observacion.Value;
                mySqlCommandSel.Parameters["@PF_Plazo"].Value = Datos.PF_Plazo.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxEstadoActual"].Value = Datos.PF_PorMaxEstadoActual.Value;
                mySqlCommandSel.Parameters["@porMaximo"].Value = Datos.porMaximo.Value;
                mySqlCommandSel.Parameters["@VlrGarantia"].Value = Datos.VlrGarantia.Value;

                mySqlCommandSel.Parameters["@AccionSolicitud1"].Value = Datos.AccionSolicitud1.Value;
                mySqlCommandSel.Parameters["@AccionSolicitud2"].Value = Datos.AccionSolicitud2.Value;
                mySqlCommandSel.Parameters["@AccionSolicitud3"].Value = Datos.AccionSolicitud3.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal1"].Value = Datos.PF_PlazoFinal1.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal2"].Value = Datos.PF_PlazoFinal2.Value;
                mySqlCommandSel.Parameters["@PF_PlazoFinal3"].Value = Datos.PF_PlazoFinal3.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFirma1"].Value = Datos.PF_VlrMontoFirma1.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFirma2"].Value = Datos.PF_VlrMontoFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrMontoFirma3"].Value = Datos.PF_VlrMontoFirma3.Value;
                mySqlCommandSel.Parameters["@PF_TasaPerfilOBL"].Value = Datos.PF_TasaPerfilOBL.Value;
                mySqlCommandSel.Parameters["@PF_TasaFirma3OBL"].Value = Datos.PF_TasaFirma3OBL.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFirma1"].Value = Datos.PF_PorMaxFirma1.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFirma2"].Value = Datos.PF_PorMaxFirma2.Value;
                mySqlCommandSel.Parameters["@PF_PorMaxFirma3"].Value = Datos.PF_PorMaxFirma3.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaPerfil"].Value = Datos.PF_VlrGarantiaPerfil.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaFirma1"].Value = Datos.PF_VlrGarantiaFirma1.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaFirma2"].Value = Datos.PF_VlrGarantiaFirma2.Value;
                mySqlCommandSel.Parameters["@PF_VlrGarantiaFirma3"].Value = Datos.PF_VlrGarantiaFirma3.Value;


                //Eg
                mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_Update");
                throw exception;
            }
        }

        public void DAL_drSolicitudDatosOtros_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM drSolicitudDatosOtros WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_Delete");
                throw exception;
            }
        }

        #endregion

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="consec"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_drSolicitudDatosOtros_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from drSolicitudDatosOtros with(nolock) where Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;


                if (mySqlCommand.Parameters["@Consecutivo"].Value == null || ((mySqlCommand.Parameters["@Consecutivo"].Value is string) &&
                    string.IsNullOrWhiteSpace(mySqlCommand.Parameters["@Consecutivo"].Value.ToString())))
                    mySqlCommand.Parameters["@Consecutivo"].Value = DBNull.Value;


                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_Exist");
                throw exception;
            }
        }

    }
}
