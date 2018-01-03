using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con el listado de documentos para los procesos
    /// </summary>
    public static class AppProcess
    {
        //Ultimo usado 1158
        //Activos
        public const int DepreciacionMensual = 1132;
        public const int ReprocesoDepreciacion = 1134;
        public const int MigrarActivoGarantia = 1152;
        //Cartera
        public const int RecaudosMasivos = 1123;
        public const int MigracionCartera = 1125;
        public const int CierreCartera = 1131;
        public const int MigracionEstadoCartera = 1147;
        public const int SustitucionCreditos = 1148;
        public const int MigracionVerificacion = 1149;
        public const int AmortizacionDerechos = 1155;
        public const int CarteraAdministracion = 1156;
        

        //Cartera Financiera
        public const int RecaudosMasivosFin = 1142;
        public const int CierreDiaCartera = 1143;
        public const int CierreCentralRiesgo = 1144;
        public const int GestionDiaria = 1154;
        public const int GestionFasecolda = 1157;

        //Contabilidad
        public const int MayorizarForm = 1103;
        public const int CierreAnual = 1104;
        public const int CierreMensual = 1105;
        public const int AjusteEnCambio = 1106;
        public const int ReclasificacionLibros = 1107;
        public const int AjusteComprobante = 1108;
        public const int MigracionComprobantes = 1111;
        public const int AbrirMes = 1112;
        public const int ComprobanteDistrib = 1117;
        public const int ProrrateoIVA = 1118;
        public const int ReclasifFiscal = 1119;
        public const int ConsolidacionBalances = 1120;
        //CxP
        public const int MigracionCxP = 1102;
        public const int FacturaEquivalente = 1137;
        //Facturacion
        public const int MigracionFactCxC = 1130;
        public const int ProcesaFacturaFija = 1133;
        public const int MigracionNewFact = 1135;
        //Inventarios
        public const int PosteoComprobantesIn = 1121;
        public const int TrasladoSaldoInventario =  1153;
        //Nomina
        public const int CierreNomina = 1124;
        public const int ContabilizarNomina = 1127;
        public const int ContabilizarPlanilla = 1128;
        public const int ContabilizaProvisiones = 1129;
        public const int MigracionNominas1 = 1140;
        public const int MigracionProvisiones = 1141;
        public const int EnvioBoletas = 1150;
        public const int MigracionNominaAnt = 1151;
        //Planeacion
        public const int CierreLegalizacion = 1138;
        //Proveedores
        public const int GeneraCargosRecibidos = 1126;
        //Proyectos
        public const int MigrarInsumos = 1145;
        public const int MigrarLocacionEntregas = 1158;
        //Operaciones Conjuntas
        public const int ParticionBilling = 1122;
        public const int CashCall = 1136;
        public const int DistribucionLegalizacion = 1139;   
        //Tesoreria
        public const int PagosElectronicos = 1109;
        public const int PagosNoProcesados = 1110;
        public const int MigracionRecibosCaja = 1146;
    }
}
