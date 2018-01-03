using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con el listado de identificadores para los documentos
    /// </summary>
    public static class AppDocuments
    {


        #region Documentos Principales
        //Activos Fijos
        public const int AltaActivos = 61;
        public const int RetiroActivos = 62;
        public const int VentasActivos = 63;
        public const int TransaccionActivos = 64;
        public const int MovimientosActivos = 65;
        public const int AdicionActivos = 66;
        public const int AltaInvActivos = 67;
        public const int AdicionInvActivos = 68;
        public const int DeterioroActivos = 69;
        public const int DepreciacionActivos = 70;
        public const int Contenedores = 101;
        public const int MigracionSaldos = 102;
        public const int CapitalizacionActivos = 103;
      
        //Cartera Coorporativa
        //Activos Fijos
        public const int SolicitudLibranza = 160;//
        public const int LiquidacionCredito = 161;//
        public const int EstadoCuenta = 162;
        public const int Incorporacion = 163;        
        public const int VentaCartera = 164;//
		public const int RecompraCartera = 165;//
        public const int RecaudosMasivos = 166;//
        public const int RecaudosManuales = 167;//
        public const int PagosTotales = 168;//
        public const int PagoFlujos = 169;
        public const int ReasignaCompradorFinal = 170;
        public const int DesIncorporacion = 171;
        public const int ReintegroClientes = 172;
        public const int ReincorporacionCartera = 173;
        public const int FondeoCartera = 174;
        public const int Verificacion = 175;
        public const int LiquidacionComisiones = 176;
        public const int CobroJuridico = 177;
        public const int AcuerdoPago = 178;
        public const int AcuerdoPagoIncumplido = 179;
        public const int RenovacionPoliza = 180;
        public const int RechazoCredito = 181;
        public const int Desistimiento = 182;
        public const int RecaudoReoperacion = 183;
        public const int NotaCreditoCartera = 184;
        public const int PolizaRevocatoria = 185;
        public const int LiquidacionJuzgadoCJ = 186;
        public const int ReestructuracionCredito = 187;
        public const int Certificaciones = 188;
        public const int Garantias = 189;

        //Contabilidad
        public const int ComprobanteManual = 11;
        public const int DocumentoContable = 12;
        public const int ComprobanteCierreAnual = 13;
        public const int CruceCuentas = 18;
        //Cuentas x Pagar
        public const int CausarFacturas = 21;
        public const int Anticipos = 22;
        public const int CajaMenor = 23;
        public const int LegalizacionGastos = 24;
        public const int PagoTarjetaCredito = 25;
        public const int NotaCreditoCxP = 26;
        public const int LegalizaTarjeta = 27;
        public const int ProvisionesCxP = 28;
        //Global
        public const int DatosTerceros = 1;
        public const int ControlGarantias = 2;
        public const int RecordatorioActividadEstado = 3;
        public const int ControlIncumplimiento = 4;
        //Inventarios
        public const int TransaccionManual = 51;
        public const int NotaEnvio = 52;
        public const int InventarioFisico = 53;
        public const int DistribucionCostos = 54;
        public const int LiquidacionImportacion = 55;
        public const int TransaccionAutomatica = 56;
        public const int DeterioroInv = 57;
        public const int RevalorizacionInv = 58;
        public const int OrdenDespacho = 59;
        //Facturacion
        public const int FacturaVenta = 41;
        public const int NotaCredito = 42;
        //Nomina
        public const int Nomina = 81;
        public const int Vacaciones = 82;
        public const int Prima = 83;
        public const int LiquidacionContrato = 84;
        public const int Cesantias = 85;
        public const int Provisiones = 86;
        public const int PlanillaAutoLiquidAportes = 87;
        public const int Prestamos = 88;
        public const int NominaContabilidad = 881;
        public const int ProvisionesContabilidad = 886;
        public const int PlanillaAportesContabilidad = 887;
        //Planeacion
        public const int Presupuesto = 200;
        public const int AdicionPresupuesto = 201;
        public const int ReclasifPresupuesto = 202;
        public const int TrasladoPresupuesto = 203;
        public const int PresupuestoCAPEX = 204;
        public const int PresupuestoOPEX = 205;
        public const int PresupuestoPxQ = 206;
        public const int AdicionPresupuestoPxQ = 207;
        public const int ReclasifPresupuestoPxQ = 208;
        public const int TrasladoPresupuestoPxQ = 209;
        //Proveedores
        public const int Solicitud = 71;
        public const int OrdenCompra = 72;
        public const int Recibido = 73;
        public const int Contrato = 74;
        public const int ConsumoProyecto = 75;
        public const int SolicitudDespachoConvenio = 76;
        public const int SolicitudDirecta = 79;
        public const int ProvisionRecibNoFact = 80;
        //Proyectos
        public const int PreProyecto = 110;
        public const int Proyecto = 111;
        public const int AjusteProyecto = 112;
        public const int ActaTrabajo = 113;
        public const int ActaEntrega = 114;
        //Tesoreria
        public const int DesembolsoFacturas = 31;
        public const int ReciboCaja = 32;
        public const int TrasladoFondos = 33;
        public const int Consignaciones = 34;
        public const int NotasBancarias = 35;
        public const int TransferenciasBancarias = 36;
        #endregion

        #region Documentos Aprobacion

        //Cartera Cooperativa
        public const int VerificacionPreliminar = 32552;
        public const int EnvioSolicitudLibranza = 32553;
        public const int AnalisisRiesgo = 32554;
        public const int DigitacionCredito = 32555;
        public const int Referenciacion = 32556;
        public const int AprobacionSolicitudLibranza = 32558;
        public const int EnvioLiquidacionCartera = 32559;      
        public const int AprobacionGiros = 32561;
        public const int EntregaLibranza = 32562;
        public const int SolicitudAnticipo = 32563;
        public const int PreseleccionLibranzas = 32564;
        public const int EnvioPreseleccionLibranzas = 32565;
        public const int Preventa = 32566;
        public const int EndosoLibranzasVendidas = 32567;
        public const int CierreOferta = 32568;
        public const int CambioFechaPlanPagos = 32569;
        public const int AprobacionLiquidacionComisiones = 32570;
        public const int AprobacionReintegroClientes = 32571;

        //Cartera Financiera
        public const int DigitacionCreditoFinanciera = 32572;
        public const int AprobacionSolicitudFin = 32573;
        public const int ContactoConfirmacion = 32580;
        public const int ContactoLegalizacion = 32585;

        //Decisor Riesgo
        
        public const int RegistroSolicitud = 32574;
        public const int Revision1 = 32575;
        public const int VerificacionSolicitud = 32576;
        public const int CartasSolicitud = 32577;
        public const int RatificacionSolicitud = 32584;
        public const int LegalizacionSolicitud = 32578;
        public const int RevisionLegalizacionSolicitud = 32584;
        public const int DesembolsoSolicitud = 32579;
        //public const int AprobacionRatificacion = 32581;
        //public const int AprobacionLegalizacion = 32582;
        //public const int AprobacionDesembolso = 32583;
        public const int Firma2 = 32582;
        public const int Firma3 = 32583;


        public const int Desestimiento = 32591;
        public const int NegociosGestionar = 32592;
        public const int drSolicitudLibranza = 32593;


        //Contabilidad
        public const int ComprobanteManualAprob = 20551;
        public const int DocumentoContableAprob = 20552;
        public const int ComprobanteAjusteAprob = 20553;

        //Cuentas x Pagar
        public const int CausacionFacturasAprob = 21551;
        public const int AnticiposAprob = 21552;
        public const int CajaMenorAprob = 21553;
        public const int CajaMenorSolicita = 21554;
        public const int CajaMenorRevisa = 21555;
        public const int CajaMenorSupervisa = 21556;
        public const int CajaMenorContabiliza = 21557;
        public const int LegalGastosAprob = 21558;
        public const int LegalGastosContabiliza = 21559;
        public const int PagoTarjetaCreditoAprob = 21560;
        public const int NotaCreditoCxPAprob = 21561;
        public const int LegalizacionTarjetasAprob = 21562;
        public const int RadicacionFactAprob = 21563;

        //Facturacion
        public const int FacturaVentaAprob = 28551;
        public const int NotaCreditoAprob = 28552;

        //Planeacion
        public const int GenerarPresupuestoAprob = 25551;
        public const int AdicionPresupuestoAprob = 25552;
        public const int ReclasificPresupuestoAprob = 25553;
        public const int TrasladoPresupuestoAprob = 25554;
        public const int AprobacionBloqueos = 25555;
        public const int AreaPresupAprob = 25556;
        public const int ConsolidacionAprob = 25557;

        //Proveedores
        public const int SolicitudPreAprob = 71551;
        public const int SolicitudAprob = 71552;
        public const int SolicitudAsign = 71553;
        public const int SolicitudDirectaAprob = 71554;
        public const int OrdenCompAprob = 71555;
        public const int RecibidoAprob = 71556;
        public const int ContratoAprob = 71557;
        public const int ConsumoProyectoAprob = 71558;
        public const int ConvenioSolicitudAprob = 71559;

        //Proyectos
        public const int PlaneacionComprasAprob = 33551;

        //Inventarios
        public const int PosteoComprobantesInAprob = 50551;
        public const int InventarioFisicoAprob = 50552;
        public const int DeterioroInvAprob = 50553;
        public const int RevalorizacionInvAprob = 50554;

        //Nomina
        public const int PagoNominaAprob = 80551;
        public const int PagoVacacionesAprob = 80552;
        public const int PagoPrimaAprob = 80553;
        public const int PagoOtrosAprob = 80554;
        public const int PagoCesantiasAprob = 80555;
        public const int PagoProvisionesAprob = 80556;
        public const int PagoLiqContratoAprob = 80557;
        public const int PagoLiqPlanillaAprob = 80558;
        #endregion

        #region Documentos de actividades

        //Cartera Cooperativa
        public const int ActividadChequeo_cc = 32801;

        #endregion

        #region Otros Documentos

        //Activos
        public const int ActivosPlaqueta = 26501;
        public const int RevertirActivos = 26502;

        //Cartera
        public const int RevertirCartera = 32501;
        public const int GestionCobranza = 32502;
        public const int CJNoIncluidosCartera = 32503;
        public const int PolizasCartera = 32504;
        public const int PolizasPago = 32505;
        public const int CambioDatosCredito = 32506;
        public const int DevolucionSolicitudes = 32507;
        public const int AnularCartera = 32508;
        public const int CobroJuridicoPoliza = 32509;

        //Contabilidad
        public const int ComprobanteAjusteCambio = 20502;
        public const int ReclasificacionSaldos = 20503;
        public const int ComprobanteAjuste = 20504;
        public const int DeclaracionImpuestos = 20505;
        public const int ComprobanteDistribucion = 20506;
        public const int ReclasificacionFiscal = 20507;
        public const int RevertirContabilidad = 20509;
        public const int AnularContabilidad = 20510;
        public const int PresupuestoContable= 20511;
        //Cuentas x Pagar
        public const int RadicacionFactura = 21501;
        public const int RadicacionNotaCreditoCxP = 21502;
        public const int RevertirCxP = 21503;
        public const int AnularCxP = 21504;
        public const int PagoImpuestos = 21505;
        
        //Facturacion
        public const int RevertirFacturacion = 28501;
        public const int AnularFacturacion = 28502;

        //Inventarios
        public const int RecibidoNotaEnvio = 50501;
        public const int OrdenLegalizacion = 50502;
        public const int RevertirInventarios = 50503;
        public const int Despacho = 50504;

        //Nomina
        public const int NovedadesNomina = 29101;
        public const int NovedadesContrato = 29102;
        public const int PrestamosEmpleados = 29103;
        public const int Traslados = 29104;
        public const int PlanillaDiariaTrabajo = 29105;
        public const int Compensatorios = 29106;
        public const int AumentoSueldo = 29108;
        public const int ReIncorporaciones = 29109;
        public const int PagoNomina = 29110;
        public const int PagoCesantias = 29111;
        public const int PagoPrima = 29112;
        public const int PagoVacaciones = 29113;
        public const int PagoProvisiones = 29114;
        public const int PagoliqContrato = 29115;
        public const int PagoPlanilla = 29116;

        //Planeacion
        public const int RevertirPlaneacion = 25501;

        //Proveedores
        public const int CierreDetalleSolicitud = 71500;
        public const int CierreDetalleOrdenComp = 72500;
        public const int CierreDetalleRecibidos = 73500;
        public const int RadicacionRecibidos = 73501;
        public const int RevertirProveedores = 73502;
        public const int RecibidoPlanillaConsumoProy = 73503;
        public const int RecibidoSolicitudDespacho = 73504;

        //Proyectos
        public const int PlaneacionTiempo = 33507;
        public const int PlaneacionCompras = 33508;
        public const int Entregables = 33509;
        public const int ProgramacionEntregables = 33510;
        public const int ActaEntregaPreFactVenta = 33511;
        public const int PlaneacionCostosAPUCliente = 33512;
        public const int PrefacturaDirecta = 33513;

        //Tesoreria
        public const int ProgramacionDesembolsos = 22501;
        public const int RevertirTesoreria = 22502;
        public const int RevertirTransferenciaBanc = 22503;
        public const int AnularCheques = 22504;

        #endregion
    }
}
