using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NewAge.ReportesComunes
{
    //Clase con las columnas segun el reporte
    public static class ColumnsInfo
    {
        //Lista de columnas x defecto para el reporte de comprobantes
        public static ArrayList ComprobanteFields = new ArrayList() 
            {"CuentaID", "TerceroID", "ProyectoID", "ConceptoCargoID", "LineaPresupuestoID", "Descriptivo", "vlrBaseML", "DebitoML", "CreditoML"};

        //Lista de columnas x defecto para el reporte de Causacion Factura (Autorizacion de giro)
        public static ArrayList AutorGiroFields = new ArrayList() { "CuentaID", "CuentaDesc", "CentroCostoID", "ProyectoID", "LineaPresupuestoID", "BaseML", "Percent", "ValorML" };

        //Lista de columnas x defecto para el reporte de Recibo de caja
        public static ArrayList ReciboCajaFields = new ArrayList() 
            { "CuentaID", "CuentaDesc", "TerceroID_cuenta", "ValorML_cuenta", };

        //Lista de columnas x defecto para el reporte de ProgramacionPagos
        public static ArrayList ProgramacionPagosFields = new ArrayList() 
            { "Factura", "Concepto", "FacturaFecha", "MonedaID", "ValorPago" };
        
        //Lista de columnas x defecto para el reporte de CajaMenor
        public static ArrayList CajaMenorFields = new ArrayList() 
            { "Fecha", "Documento", "TerceroID", "TerceroDesc", "CentroCostoDesc", "ProyectoDesc", "FacturaDesc", "ValorBruto", "ValorIva", "ValorRteF", "ValorRteIVA", "ValorRteICA" };

        //Lista de columnas x defecto para el reporte de PagoFacturas
        public static ArrayList ChequeFields = new ArrayList() { "CuentaID", "FacturaID", "FacturaDesc", "ValorFactura" };

        //Lista de columnas x defecto para el reporte de LegalizacionGastos
        public static ArrayList LegaGastosFooterFields = new ArrayList() { "Fecha", "Observacion", "ValorAlojamiento", "ValorAlimentacion", "ValorTranspAer", "ValorTranspTer", "ValorViaticos", "ValorOtros", "ValorImpuestos", "ValorTotal" };
        public static ArrayList LegaGastosDetailFields = new ArrayList() { "TipoCargoDesc", "CuentaID", "ProyectoID", "CentroCostoID", "LugarGeoID", "Valor"};

        //Lista de columnas x defecto para el reporte "Formularios Soporte"
        public static ArrayList FormSoporteFields = new ArrayList() { "TerceroID", "TerceroDesc", "DocumentoID", "ValorML", "BaseML", "Percent", "ComprobanteID", };   

        //Lista de columnas x defecto para el reporte "Formularios por Cuenta"
        public static ArrayList FormCuentaFields = new ArrayList() {  "CuentaID", "CuentaDesc", "ValorML" };

        //Lista de columnas x defecto para el reporte "transaccion manual"
        public static ArrayList FormTransacMnlDetailFields = new ArrayList() { "inReferenciaID", "DocSoporte", "DescripReferencia", "CantidadUni", "Serial" };
        public static ArrayList FormTransacMnlFooterFields = new ArrayList() { "Total" };

        //Lista de Columnas x defecto para el reporte "Nota Envio"
        public static ArrayList FormNotaEDetailFields = new ArrayList() { "inReferenciaID", "DescripReferencia", "CantidadUni", "Serial"};
        public static ArrayList FormNotaEFooterFields = new ArrayList() { "EnviadoPor", "RecibidoPor", "DireccionBase", "FechaRecepcion" };

        public static ArrayList FormSoliciDetailFields = new ArrayList() { "CodigoBSID", "inReferenciaID", "Descripción", "UnidadInv", "CantidadSol", "Proyecto", "CentroCosto" };

        public static ArrayList FormPreNominaDetailFields = new ArrayList() { "CodigoDevengos", "DescripcionDevengos", "BaseDevengos", "ValorDevengos","CodigoDeducciones", "DescripcionDeducciones", "BaseDeducciones", "ValorDeducciones" };
        public static ArrayList FormPreNominaFooterFields = new ArrayList() { "TotalDevengado", "TotalDeducido", "Báse", "NetoPagar" };

        public static ArrayList FormPreVacacionesFields1 = new ArrayList() { "CodigoDevengos", "DescripcionDevengos", "BaseDevengos", "ValorDevengos"};
        public static ArrayList FormPreVacacionesFields2 = new ArrayList {"CodigoDeducciones","DescripcionDeducciones","BaseDeducciones","ValorDeducciones"};

        public static ArrayList FormReportInvFisicoConteo = new ArrayList() { "Codigo", "Descripcion", "Stand", "Cantidades" };
        public static ArrayList FormReportInvFisicoFisico = new ArrayList() { "Codigo", "Descripcion", "Clase", "Stand", "Fisico" };
        public static ArrayList FormReportInvFisicoDiferencia = new ArrayList() { "Codigo", "Descripcion", "Kardex", "Fisico", "Diferencia", "UnidadLocal", "TotalLocal", "UnidadExt", "TotalExt"  };

        //Reporte de liquidacion Cartera
        public static ArrayList FormLiquidcacionCreditoDetailFields = new ArrayList() { "Venc_Cta", "CuotaID", "VlrCuota", "Capital", "Seguro", "Interes", "Otros1", "VlrOtros", "SaldoCapital" };
        public static ArrayList FormLiquidcacionCreditoFooterFields = new ArrayList() { "OtrosCostos", "Mensual", "Total", "Porcent" };
        
        /// <summary>
        /// Arreglo para el reporte correspondiente al detalle de la nomina  NominaDetalleCxE
        /// </summary>
        public static ArrayList FormReportNominaDetalleCxE = new ArrayList() { "Cédula", "Nombre", "Cuenta", "Valor" };
    }
}
    