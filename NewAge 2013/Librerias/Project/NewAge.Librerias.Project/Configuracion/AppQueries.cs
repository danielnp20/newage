using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con el listado documentos para las consultas
    /// </summary>
    public static class AppQueries
    {
        //Activos Fijos
        public const int QueryActivos = 23310;
        public const int QueryActivosControl = 23311;
        
        //Cartera
        public const int Liquidador = 32310;
        public const int QueryLibranza = 32311;
        public const int EstadisticasDiariasCartera = 32312;
        public const int EstadisticasMensualesCartera = 32313;
        public const int ArchivosIncorporaciones = 32314;
        public const int VentaCartera = 32315;
        public const int Movimiento = 32316;
        public const int ConsultasGeneralesCc = 20318;
        public const int ConsultaSolicitud = 32317;
        public const int QueryRecaudosMasivos = 32318;
        public const int QueryDataCredito = 32319;
        public const int QueryEstadoCtaComp = 32320;
        public const int QuerySaldosLibranza = 32321;
        public const int QueryGestionCobranza = 32322;
        public const int QuerySolicitudGestion = 32323;

        //Contabilidad
        public const int Simulador = 20310;
        public const int EstadisticasMensualesContabilidad = 20311;
        public const int QuerySaldosCierre = 20316;
        public const int ConsultasGeneralesCo = 20317;
        public const int QueryMvtoAuxiliar = 20319;

        //Cuentas X Pagar
        public const int QueryDocumentosCxP = 21310;

        //Facturacion
        public const int QueryDocumentosFacturas = 28310;

        //Inventarios
        public const int QueryBodega = 26310;
        public const int QueryReferencia = 26311;
        public const int QueryMovimiento = 26312;
        public const int QuerySeriales = 26313;

        //Planeacion
        public const int QueryInformeMensual = 25310;
        public const int QueryEstadoEjecucion = 25311;
        public const int QuerySobreEjecucion = 25312;

        //Proveedores
        public const int QuerySolicitud = 27310;
        public const int QueryOrdenCompra = 27311;
        public const int QueryRecibidos = 27312;
        public const int QueryComiteSolPermiso = 27313;
        public const int QueryComiteOcPermiso = 27314;
        public const int QueryComiteOcNoAprPermiso = 27315;
        public const int QueryComiteRecPermiso = 27316;
        //Proyectos
        public const int QueryAnalisis = 33310;
        public const int QueryTrazabilidad = 33311;
        public const int QueryCumplimiento = 33312;
        public const int QueryEjecucion = 33313;
        public const int QueryComiteTecnico = 33314;
        public const int QueryComiteCompras = 33315;
        public const int QueryComiteFinanciero = 33316;
        public const int QueryComiteTecPermiso = 33317;
        public const int QueryEjecucionPresupuestal = 33318;
        public const int QueryIndicadores = 33319;

        //Operaciones Conj
        public const int QueryInformeMensualOC = 30310;

        //Tesoreria
        public const int QueryChequesGirados = 22310;
        public const int QueryReciboCaja = 22311;
        public const int QueryFlujoCaja = 22312;
        public const int QueryFlujoMensual = 22313;
        
    }
}
