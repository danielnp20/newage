using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con el listado de maestras
    /// </summary>
    public static class AppMasters
    {
        public enum MasterType
        {
            Simple,
            Hierarchy,
            Complex
        }

        // RESERVAR LOS CODIGOS 35000 HASTA 36999 PARA REPORTES, NO USAR PARA MAESTRAS

        #region Activos Fijos
        public static int acGrupo = 23001;
        public static int acClase = 23002;
        public static int acMovimientoTipo = 23003;
        public static int acEstado = 23004;
        public static int acContabiliza = 23006;
        public static int acTipo = 23007;
        public static int acDotacion = 23008;
        public static int acComponenteActivo = 23009;
        public static int acReservasxPozoDUP = 23010;
        public static int acProduccionxPozoDUP = 23011;
        #endregion

        #region Aplicacion
        public static int aplModulo = 900;
        public static int aplIdioma = 901;
        #endregion

        #region Cartera Coorporativa
        public static int ccPagaduriaGrupo       = 32001;
        public static int ccPagaduria            = 32002;
        public static int ccAnexosLista          = 32003;
        public static int ccAsesor               = 32004;
        public static int ccComisionDescuento    = 32005;
        public static int ccCliente              = 32006;
        public static int ccLineaCredito         = 32007;
        public static int ccCompradorCartera     = 32008;
        public static int ccConceptoECCesionario = 32009;
        public static int ccPoliticaIndicesCobranza = 32010;
        public static int ccCarteraComponente    = 32011;
        public static int ccTipoCredito          = 32012;
        public static int ccLineaComponente      = 32013;
        public static int ccLineaComponentePlazo = 32014;
        public static int ccPagaduriaAnexos      = 32015;
        public static int ccComisionDescAsesor   = 32016;
        public static int ccRechazoCausal        = 32017;
        public static int ccFinanciera           = 32018;
        public static int ccChequeoLista         = 32019;
        public static int ccValorAutorizado      = 32020;
        public static int ccValorAmparado        = 32021;
        public static int ccLineaComponenteMonto = 32022;
        public static int ccCompradorGrupo       = 32023; 
        public static int ccComponenteCuenta     = 32024;
        public static int ccCompradorAnexos      = 32025;
        public static int ccNominaINC            = 32026;
        public static int ccCentroPagoPAG        = 32027;
        public static int ccCompradorPagaduria   = 32028;
        public static int ccCompradorPortafilio  = 32029;
        public static int ccCompradorMonto       = 32030;
        public static int ccProfesion            = 32031;
        public static int ccTipoNomina           = 32032;
        public static int ccComponenteEdad       = 32033;
        public static int ccVendedorCartera      = 32034;
        public static int ccAseguradora          = 32035;
        public static int ccClasificacionCredito = 32036;
        public static int ccClasificacionxRiesgo = 32037;
        public static int ccActividadesCobranza  = 32038;
        public static int ccCooperativa          = 32039;
        public static int ccConcesionario        = 32040;
        public static int ccFasecolda            = 32041;
        public static int ccFasecoldaModelo      = 32042;
        public static int ccAbogado              = 32043;
        public static int ccInformeSicoopCuenta  = 32044;
        public static int ccSiniestroEstado = 32045;
        public static int ccEstadoCuentaCesionarioTabla = 32046;
        public static int ccCJTipoMovimiento     = 32047;
        public static int ccCobranzaEstado       = 32048;
        public static int ccCobranzaGestion      = 32049;
        public static int ccComisionTabla        = 32050;
        public static int ccPagaduriaPrograma    = 32051;
        public static int ccDevolucionCausal     = 32052;
        public static int ccSolicitudDevolución  = 32053;
        public static int ccReintegroSaldo       = 32054;
        public static int ccDevolucionCausalGrupo = 32055;
        public static int ccIncorporacionNovedad = 32056;
        public static int ccSegurosAsesor = 32057;
        public static int ccPagaduriaSector = 32058;

        #endregion

        #region Contabilidad
        public static int coRegimenFiscal = 20001;
        public static int coTerceroDocTipo = 20002;
        public static int coActEconomica = 20003;
        public static int coCuentaGrupo = 20004;
        public static int coImpuestoTipo = 20005;
        public static int coBalanceTipo = 20006;
        public static int coOperacion = 20007;
        public static int coConceptoCargo = 20008;
        public static int coImpuestoDeclaracion = 20009;
        public static int coImpuestoFormato = 20010;
        public static int coImpuestoConcepto = 20011;
        public static int coDocumento = 20013;
        public static int coGrupoFiscal = 20014;
        public static int coTasaCierre = 20016;
        public static int coImpDeclaracionCuenta = 20017;
        public static int coImpDeclaracionRenglon = 20018;
        public static int coImpDeclaracionCalendario = 20019;
        public static int coBalanceReclasifica = 20020;
        public static int coFuenteUso = 20021;
        public static int coPlanCorporativo = 20022;
        public static int coPlanillaConsolidacion = 20023;
        public static int coReporteNota = 20024;
        public static int coCentroCosto = 20100;
        public static int coActividad = 20102;
        public static int coReporte = 20103;
        public static int coActividadTercero = 20104;
        public static int coTercero = 20200;
        public static int coPlanCuenta = 20201;
        public static int coProyecto = 20202;
        public static int coComprobante = 20203;
        public static int coComprobantePrefijo = 20205;
        public static int coComprBalanceTipo = 20300;
        public static int coReporteLinea = 20301;
        public static int coReporteFiltro = 20302;
        public static int coCargoCosto = 20303;
        public static int coImpuesto = 20304;
        public static int coValIVA = 20305;
        public static int coIVARetencion = 20306;
        public static int coControl = 20307;
        public static int coImpuestoFiltro = 20308;
        public static int coImpuestoLocal = 20309;
        public static int coLineaNegocio = 20312;
        public static int coUnidadGenEfectivo = 20313;
        public static int coNormasIFRS = 20315;
        #endregion

        #region CuentasXPagar
        public static int cpConceptoCXP = 21001;
        public static int cpCajaMenor = 21002;
        public static int cpAnticipoTipo = 21003;
        public static int cpCargoEspecial = 21004;
        public static int cpCausalesDev = 21006;
        public static int cpDistribuyeImpLocal = 21007;
        public static int cpCargoAnticipoTipo = 21008;
        public static int cpTarjetaCredito = 21009;
        #endregion

        #region Facturacion
        public static int faCliente = 28001;
        public static int faConceptos = 28002;
        public static int faServicios = 28003;
        public static int faAsesor = 28004;
        public static int faListaPrecio = 28006;
        public static int faPrecioServicio = 28008;
        public static int faFacturaTipo = 28009;
        #endregion

        #region Global
        public static int glAreaFuncional = 11001;
        public static int glAreaFuncionalDocumentoPrefijo = 11002;
        public static int glBanco = 11003;
        public static int glDocMigracionEstructura = 11004;
        public static int glDocMigracionCampo = 11005;
        public static int glDocumento = 11006;
        public static int glEmpresa = 11008;
        public static int glEmpresaGrupo = 11009;
        public static int glLocFisica = 11010;
        public static int glLugarGeografico = 11011;
        public static int glPais = 11012;
        public static int glPrefijo = 11013;
        public static int glTabla = 11014;
        public static int glMoneda = 11015;
        public static int glTasaCambio = 11016;
        public static int glConceptoSaldo = 11017;
        public static int glDocumentoAnexo = 11018;
        public static int glHorarioTrabajo = 11019;
        public static int glGarantia = 11020;
        public static int glGrupoUsuario = 11021;
        public static int glUsuarioxGrupo = 11022;
        public static int glDatosAnuales = 11023;
        public static int glActividadAreaFuncional = 11024;
        public static int glProcedimiento = 11025;
        public static int glActividadFlujo = 11026;
        public static int glActividadPermiso = 11027;
        public static int glProcedimientoFlujo = 11028;
        public static int glLLamadaProposito = 11032;
        public static int glLLamadaPregunta = 11033;
        public static int glDiasFestivos = 11034;
        public static int glDatosMensuales = 11035;
        public static int glBienServicioClase = 11036;
        public static int glAreaFisica = 11037;
        public static int glZona = 11038;
        public static int glEmpresaModulo = 11039;
        public static int glTerceroReferencia = 11040;
        public static int glSeccionFuncional = 11041;
        public static int glGrupoApruebaDOC = 11042;
        public static int glNivelesAprobacionDoc = 11043;
        public static int glGrupoAprueba = 11044;
        public static int glIncumplimientoEtapa = 11045;
        public static int glDocumentoTipo = 11046;
        public static int glDocumentoClase = 11047;
        public static int glDocumentoMovimientoTipo = 11048;
        public static int glLLamadaCodigo = 11049;
        public static int glActividadChequeoLista = 11050;
        public static int glCorreosEspeciales = 11051;

        #endregion

        #region Inventarios
        public static int inMaterial = 26001;
        public static int inRefParametro1 = 26002;
        public static int inRefParametro2 = 26003;
        public static int inRefParametro3 = 26004;
        public static int inRefTipo = 26005;
        public static int inRefGrupo = 26006;
        public static int inRefClase = 26007;
        public static int inMarca = 26008;
        public static int inSerie = 26009;
        public static int inReferencia = 26010;
        public static int inBodegaContab = 26011;
        public static int inCosteoGrupo = 26012;
        public static int inBodegaTipo = 26013;
        public static int inBodega = 26014;
        public static int inBodegaRefe = 26015;
        public static int inBodegaUbicacion = 26016;
        public static int inMovimientoTipo = 26018;
        public static int inUnidad = 26019;
        public static int inEmpaque = 26020;
        public static int inConversionUnidad = 26021;
        public static int inPartesComponentes = 26022;
        public static int inRefEquivalentes = 26023;
        public static int inContabiliza = 26024;
        public static int inTipoParameter1 = 26025;
        public static int inPosicionArancel = 26026;
        public static int inTipoParameter2 = 26027;
        public static int inReferenciaCod = 26028;
        public static int inImportacionModalidad = 26030;
        public static int inAgenciaAduana = 26031;
        #endregion

        #region Nomina

        public static int noEmpleado = 29001;
        public static int noContratoTipo = 29002;
        public static int noOperacion = 29003;
        public static int noFondo = 29004;
        public static int noCaja = 29005;
        public static int noBrigada = 29006;
        public static int noRiesgo = 29007;
        public static int noConceptoPlaTra = 29008;
        public static int noTurnoCompensatorio = 29009;
        public static int noRol = 29010;
        public static int noConvencion = 29011;
        public static int noComponenteNomina = 29012;
        public static int noConceptoGrupoNOM = 29030;
        public static int noConceptoNOM = 29032;
        public static int noContratoNov = 29033;
        public static int noCompFlexible = 29034;
        public static int noAportesPorcentaje = 29051;
        public static int noBonificaciones = 29052;
        public static int noDistribuyeNomina = 29053;
        public static int noNovedadesxDia = 29054;
        public static int noCertificadoIngresosLinea = 29055;
        public static int noReteFuenteBasica = 29056;
        public static int noReteFuenteMinima = 29057;
        public static int noPrestacionesConvencion = 29060;
        public static int noEmpleadoFamilia = 29061;
        public static int noTipoVinculacion = 29062;
        public static int noCondicionEspecial = 29063;
        public static int noConceptoMM = 29064;
        public static int noConceptoNominaMM = 29065;
        public static int noPrestacionCod = 29066;
        
        #endregion

        #region Operaciones Conjuntas

        public static int ocOtrosConceptos = 30001;
        public static int ocConceptoCuenta = 30002;
        public static int ocProyectoTRM = 30003;
        public static int ocBloque = 30004;
        public static int ocGrupoCtaSocio = 30005;
        public static int ocContratoCampo = 30006;
        public static int ocParticionTablaFija = 30007;
        public static int ocTipoCosteoSocio = 30008;
        public static int ocTipoCosto = 30009;
        public static int ocCuentaGrupo = 30100;
        public static int ocParticionTabla = 30102;
        public static int ocRangosOverhead = 30103;
        public static int ocSocio = 30108;

        #endregion

        #region Planeacion

        public static int plLineaPresupuesto = 25001;
        public static int plGrupoPresupuestal = 25004;
        public static int plGrupoPresupuestalLinea = 25005;
        public static int plGrupoPresupuestalUsuario = 25006;
        public static int plRecursoGrupo = 25007;
        public static int plAreaPresupuestal = 25008;
        public static int plIndicadorFinanciero = 25009;
        public static int plRazonFinanciera = 25010;
        public static int plDistribucionCampo = 25011;
        public static int plGrupoPresupuestalActividad = 25012;
        public static int plRecurso = 25013;
        public static int plTasasPresupuesto = 25015;
        public static int plActividadLineaPresupuestal = 25101;
        public static int plLineaGrupo = 25016;
        #endregion

        #region Proveedores

        public static int prBienServicio = 27001;
        public static int prProveedor = 27002;
        public static int prPolizaCubrimiento = 27005;
        public static int prProveedorProductoTipo = 27006;
        public static int prProveedorExperiencia = 27007;
        public static int prProveedorAnexos = 27008;
        public static int prProveedorMarca = 27009;
        public static int prAnexoLista = 27010;
        public static int prAprobacionGrupo = 27011;
        public static int prProveedorPrecio = 27012;

        #endregion

        #region Proyectos
        public static int pyContrato = 33001;
        public static int pyEtapa = 33002;
        public static int pyClaseProyecto = 33005;
        public static int pyTrabajo = 33007;
        public static int pyTareaClase = 33008;
        public static int pyLineaFlujo = 33010;
        public static int pyTareaXLineaFlujo = 33011;
        public static int pyTareaCapitulo = 33012;
        public static int pyTareaRecurso = 33013;
        public static int pyRecursoCostoBase = 33014;
        public static int pyListaPrecio = 33015;
        public static int pyRecurso = 33016;
        public static int pyTarea = 33017;
        public static int pyCapituloGrupo = 33018;
        public static int pyTrabajoFlujo = 33019;
        public static int pyCapituloCliente = 33020;
        public static int pyEntregaClase = 33021;
        public static int pyTareaCliente = 33022;
        #endregion

        #region Seguridad

        public static int seUsuario = 12001;
        public static int seUsuarioGrupo = 12002;
        public static int seUsuarioPrefijo = 12003;
        public static int seGrupo = 12005;
        public static int seGrupoDocumento = 12006;
        public static int seMaquina = 12007;//
        public static int seUsuarioBodega = 12008;
        public static int seLAN = 12010;
        public static int seDelegacionTareas = 12011; // Mezcla entre maestra y documentos

        #endregion

        #region Recursos Humanos

        public static int rhCargos = 24001;
        public static int rhNivelSalarial = 24002;
        public static int rhNivelResponsabilidad = 24003;
        public static int rhCompetencia = 24004;
        public static int rhEnfermedadProf = 24005;
        public static int rhCargosxAreaFuncional = 24006;
        public static int rhCompetenciasxCargo = 24007;
        public static int rhFuncionxCargo = 24008;
        public static int rhExperienciaxCargo = 24009;
        public static int rhEstudioxCargo = 24010;
        public static int rhPruebasxCargo = 24011;
        public static int rhSeleccionForma = 24015;

        #endregion

        #region Tesoreria
        public static int tsBancosCuenta = 22001;
        public static int tsConceptoExtracto = 22002;
        public static int tsCaja = 22003;
        public static int tsIngresoConcepto = 22004;
        public static int tsNotaBancaria = 22005;
        public static int tsFlujoFondo = 22006;
        public static int tsBanco = 22007;
        public static int tsFormaPago = 22008;
        public static int tsBancosEncabezadoPE = 22010;
        public static int tsBancosDatosPE = 22011;
        public static int tsBancoCierrePE = 22012;
        public static int tsInstrumentoFinanciero = 22013;
        public static int tsInstFinancieroComponente = 22014;
        #endregion
        
        #region Decisor
        public const int drFincaRaiz = 32201;
        public const int drEstadoActualObligaciones = 32202;
        public const int drEstadoActualUtilizaCuposTC = 32203;
        public const int drEstadoPeorCalificacionTrim = 32204;
        public const int drEstadoActualConsultas = 32205;
        public const int drMoraOblVigentes30 = 32206;
        public const int drMoraOblVigentes60 = 32207;
        public const int drMoraOblVigentes90 = 32208;
        public const int drMoraOblVigentes120 = 32209;
        public const int drMoraUltAno30 = 32210;
        public const int drMoraUltAno60 = 32211;
        public const int drMoraUltAno90 = 32212;
        public const int drMoraUltAno120 = 32213;
        public const int drRepNegObligacionCobrador = 32214;
        public const int drRepNegDudosoRecaudo = 32215;
        public const int drRepNegCarteraCastigada = 32216;
        public const int drRepNegCuentasEmbargadas = 32217;
        public const int drRepNegCarteraRecuperada = 32218;
        public const int drRepNegCuentasCanceladas = 32219;
        public const int drEstabilidadReporteDireccion = 32220;
        public const int drEstabilidadNumeroEntidades = 32221;
        public const int drEstabilidadCelular = 32222;
        public const int drEstabilidadCorreo = 32223;
        public const int drUbicabilidadDirecciones = 32224;
        public const int drUbicabilidadTelefonos = 32225;
        public const int drUbicabilidadCelulares = 32226;
        public const int drUbicabilidadCorreos = 32227;
        public const int drProbabilidadMora = 32228;
        public const int drProbabilidadResultado = 32229;
        public const int drEstimadoEvaluacion = 32230;
        public const int drFactoresEndeudamiento = 32231;
        public const int drGarantiaPropuestaMin = 32232;
        public const int drGarantiaPropuestaMax = 32233;
        public const int drPorcentajeFinMax = 32234;
        public const int drCuotaInicialIng = 32235;
        public const int drActividadChequeoLista = 32236;
        public const int drUtilizaCuposTC= 32237;
        public const int drMoraOblVigentes= 32238;
        public const int drMoraUltAno= 32239;
        public const int drRepNegMora= 32240;
        public const int drRepNegRecuperado = 32241;
        #endregion
    }
}
