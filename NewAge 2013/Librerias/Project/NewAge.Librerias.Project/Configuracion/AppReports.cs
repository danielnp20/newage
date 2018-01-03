using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con los numeros de los reportes que se generan con la configuracion generica de reportes
    /// </summary>
    public static class AppReports
    {
        //Ultimo ID Utilizado 35155
        #region Activos Fijos

        public const int acSaldos = 35087;
        public const int acRelacionActivos = 35089;
        public const int acEquiposArrendados = 35035;
        public const int acImportacionesTemporales = 35041;

        #endregion
        #region Cartera Coorporativa
        public const int ccLibranzas = 35029;
        public const int ccLiquidacionCredito = 35059;
        public const int ccEstadoCredito = 35060;
        public const int ccReportesEpeciales = 35066;
        public const int ccReportesCartera = 35068;
        public const int ccAportesCartera = 35069;
        public const int ccCertificados = 35086;
        public const int ccCesionCartera = 35090;
        public const int CcVentaCartera = 35094;
        
        public const int ccCreditos = 35100;
        public const int ccPagaduriaIncorporaciones = 35101;
        public const int ccInformesSIGCOOP = 35032;
        public const int ccRepExcelLibranzas = 35085;
        public const int ccRepAnalisisPagos = 35102;
        public const int ccRepSolicitudes = 35096;       
        public const int ccProyeccionVencim = 35104;
        public const int ccEstadoCesionCartera = 35105;
        public const int ccCobroJuridico = 35106;
        public const int ccPolizaEstado= 35107;
        public const int ccAmortizacion = 35108;
        public const int ccSaldoCapital = 35109;
        public const int ccCreditoReport = 35112;
        public const int ccDevolucionSolicitud = 35113;
        public const int ccCarteraEspeciales = 35115;
        public const int ccRecaudosNominaDeta = 35118;
        public const int ccCertificadoDeuda = 35121;
        public const int ccCertificadoPazYSalvo = 35122;
        public const int ccSaldosAFavor = 35123;
        public const int ccSaldosSeguroVida = 35131;
        public const int ccCertificadoPagosAlDia = 35132;
        public const int ccCertificadoRelacionPagos = 35133;
        public const int ccCertificadoRelacionPagosXCuota = 35140;
        public const int ccGestionCobranza = 35135;
        public const int ccActivacionReoperacion = 35141;
        public const int ccContratoMutuo = 35143;
        public const int ccAutDescuento = 35144;
        public const int ccReportePagaduria = 35147;
        public const int ccReporteCredito= 35149;
        public const int ccReporteEstadisticos = 35152;
        public const int ccReporteGEstion = 35153;

        #endregion
        #region Contabilidad

        public const int coLibroMayor = 35001;
        public const int coLibroDiario = 35002;
        public const int coBalanceDePrueba = 35003;
        public const int coBalanceDePruebaPorMeses = 35004;
        public const int coBalanceDePruebaPorQ = 35005;
        public const int coReporteComprobantes = 35006;
        public const int coAuxiliar = 35008;
        public const int coAuxiliarPorTercero = 35009;
        public const int coSaldos = 35010;
        public const int coDocumentoContable = 35011;
        public const int coFormularios = 35012;
        public const int coFormularios_balance = 35013;
        public const int coFormulariosDetail = 35014;
        public const int coFormulariosCuenta = 35015;
        public const int coFormulariosCuenta_balance = 35016;
        public const int coInventariosBalance = 35017;
        public const int coSaldosDocumentos = 35018;
        public const int coRelacionDocumentos = 35019;
        public const int coReciboCaja = 35027;
        public const int coBalanceDePruebaComparativo = 35034;
        public const int coTasasDeCierre = 35036;
        public const int coTasasDiarias = 35037;
        public const int coBalanceGeneral = 35040;
        public const int coCertificates = 35043;
        public const int coCertificatesDetail = 35044;
        public const int coAjusteSaldos = 35046;
        public const int coSeveralComprobantes = 35047;
        public const int coBalanceComparativos = 35091;
        public const int coimpuestoDeclaraciones = 35092;
        public const int coReporteEstadoResultados = 35136;
        public const int coReporteSituacionFinanciero = 35137;
        public const int coReporteLineas = 35138;
        public const int coEjecucionPresupuesto = 35139;
        public const int coMediosMagneticos = 35146;
        #endregion
        #region Cuentas x Pagar

        public const int cpAutorizacionDeGiro = 35020;
        public const int cpFacturaEquivalente = 35021;
        public const int cpAnticipo = 35025;
        public const int cpAnticipoViaje = 35026;
        public const int cpFacturasPorPagar = 35038;
        public const int cpLibroDeCompras = 35039;
        public const int cpAnticiposPendientes = 35045;
        public const int cpCajaMenor = 35049;
        public const int cpLegalizacionGastos = 35051;
        public const int cpMovimientosPeriodo = 35052;
        public const int cpTarjetasPago = 35062;
        public const int cpFacturasXPagar = 35072;
        public const int cpFacturasPagadas = 35073;
        public const int cpAnticipos = 35074;
        public const int cpRadicaciones = 35075;
        public const int cpPorEdades = 35081;
        public const int cpFlujoSemanalResumido = 35082;
        public const int cpCxPvsPagos = 35117;
        #endregion
        #region Facturacion

        public const int faCuentaCobro = 35007;
        public const int faCxCxEdades = 35022;
        public const int faCxPxEdadesResumido = 35023;
        public const int faCxPxEdadesFlujoSemanal = 35024;
        public const int faLibroVentas = 35099;
        public const int faFacturasVentaMasivo = 35124;
        public const int faFacturasVentaAIU = 35150;
        #endregion
        #region General
        public const int ReportBase = 35000;
        #endregion
        #region Global

        public const int glDocumentosPendientes = 35031;
        public const int glReportesDinamicos = 35155;
        #endregion
        #region Inventarios

        public const int inTransaccionManual = 35053;
        public const int inNotaEnvio = 35054;
        public const int inFisicoInventario = 35058;
        public const int inSaldos = 35071;
        public const int inKardex = 35076;
        public const int inSerial = 35080;
        public const int inRelacionPrestamos = 35145;
        public const int inRepDocumentos = 35148;

        #endregion
        #region Nomina

        public const int noPreNomina = 35056;
        public const int noVacaciones = 35057;
        public const int noDetalleNomina = 35061;
        public const int noAportes = 35063;
        public const int noVacacionesParameter = 35064;
        public const int noDocumentosYCertificados = 35067;
        public const int noBoletaPago = 35103;
        public const int noPrestamo = 35110;
        public const int noCajaCompensacion = 35113;
        public const int noLiquidacionContrato = 35116;
        public const int noCesantias = 35119;
        public const int noProvisiones = 35120;
        #endregion
        #region Operaciones Conjuntas

        public const int ocOperaciones = 35093;

        #endregion
        #region Planeacion

        public const int plPresupuesto = 35028;
        public const int plEjecucionPresupuestal = 35030;
        public const int plSobreEjecicion = 35095;
        public const int plCierreLegalizacion = 35097;

        #endregion
        #region Proveedores
        public const int prSolicitudOld = 35055;
        public const int prProcedimientoCompras = 35088;
        public const int prComproVSFact = 35033;
        public const int prOrdenCompras = 35092;
        public const int prOrdenCompraAnexo = 35114;
        public const int prSolicitudDoc = 35128;
        public const int prRecibidoDoc = 35129;
        public const int prOrdenCompraServicio = 35154;
        #endregion
        #region Proyectos
        public const int pyCumplimiento = 35042;
        public const int pyPresupuesto = 35065;
        public const int pySolicitudProyecto = 35111;
        public const int pySolicitudProyectoCompara = 35125;
        public const int pySolicitudProyectoDetallado = 35126;
        public const int pyActaTrabajo = 35127;
        public const int pyActaEntrega = 35148;
        #endregion
        #region Tesoreria
        public const int tsProgramacionPagos = 35048;
        public const int tsChequesGirados = 35070;
        public const int tsReciboCaja = 35077;
        public const int tsOtrosMovBancarios = 35078;
        public const int tsLibroBancos = 35079;
        public const int tsLibroDeBancos = 35083;
        public const int tsRelacionPagos = 35084;
        public const int tsPagoFacturasBanco1 = 35130;
        public const int tsTransferenciaBancos = 35131;
        public const int tsPagoFacturasBanco2 = 35142;
        public const int tsEjecutado = 35151;
        #endregion

        #region Codigos Sin Utilizar
        // 35097
        // 35098
        //public const int PagoFacturas = 35050; No tocar, aun no se sabe donde se genera este reporte
        #endregion
    }

    public enum AppReportParametersForm
    {
        #region Activos Fijos

        acSaldos,
        acRelacionActivos,
        acEquiposArrendados,

        #endregion
        #region Cartera Corporativa

        ccSaldosMora,
        ccSolicitudes,
        ccVentaCartera,
        ccCreditos,
        ccPagaduriaIncorporaciones,
        ccInformesSIGCOOP,
        ccCarteraVarios,

        #endregion
        #region Contabilidad

        coBalancePrueba,
        coLibroDiario,
        coLibroMayor,
        coComprobantes,
        coAuxiliar,
        coSaldos,
        coFormularios,
        coInventariosBalance,
        coSaldosDocumentos,
        coRelacionDocumentos,
        coTasas,
        coCertificates,
        coBalanceComparativos,
        coMediosMagneticos,
        
        #endregion
        #region Cuentas x Pagar

        cpFacturasPorPagar,
        cpLibroDeCompras,
        cpMovimientosPeriodo,
        cpRadicaciones,

        #endregion
        #region Facturacion

        faEdades,
        faFacturaEquivalente,
        faLibroVentas,

        #endregion
        #region Global

        glDocumentosPendientes,

        #endregion
        #region Inventarios

        inSaldos,
        inKardex,
        inSerial,

        #endregion
        #region Tesoreria

        tsReciboCaja,
        
        #endregion
        #region Nomina

        noVacacionesParameter,
        noPrestamo,
        noLiquidacionContrato,
        
        #endregion
        #region Operaciones Conjuntas

        ocOperaciones,

        #endregion
        #region Planeacion

        plPresupuesto,
        plEjecucionPresupuestal,
        plSobreEjecicion,
        plCierreLegalizacion,

        #endregion
        #region Proveedores

        prProcedimientoCompras,
        prComproVSFact,
        prOrdenCompras,

        #endregion
        #region Proyectos
        
        pyCumplimiento,
        pyPresupuesto,

        #endregion
    }
}
