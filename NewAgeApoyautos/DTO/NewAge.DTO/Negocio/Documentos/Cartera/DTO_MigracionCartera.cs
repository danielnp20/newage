using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Clase para migracon de comprobantes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionCartera
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigracionCartera()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.AsesorID = new UDT_AsesorID();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.ZonaID = new UDT_ZonaID();
            this.FechaLiquidacion = new UDTSQL_smalldatetime();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.TipoEstado = new UDTSQL_tinyint();
            this.Ciudad = new UDT_LugarGeograficoID();
            this.CompradorID = new UDT_CompradorCarteraID();
            this.VlrVenta = new UDT_Valor();
            this.NumCuotaVendidas = new UDT_CuotaID();
            this.FlujosPago = new UDT_CuotaID();
            this.TasaVenta = new UDT_PorcentajeID();
            this.NumeroCesion = new UDTSQL_char(15);
            this.Plazo = new UDTSQL_smallint();
            this.PorInteres = new UDTSQL_decimal();
            this.VlrCuota = new UDT_Valor();
            this.VlrCredito = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrPagos = new UDT_Valor();
            this.VlrComponente1 = new UDT_Valor();
            this.VlrComponente2 = new UDT_Valor();
            this.VlrComponente3 = new UDT_Valor();
            this.VlrComponente4 = new UDT_Valor();
            this.VlrComponente5 = new UDT_Valor();
            this.VlrComponente6 = new UDT_Valor();
            this.VlrComponente7 = new UDT_Valor();
            this.VlrComponente8 = new UDT_Valor();
            this.VlrComponente9 = new UDT_Valor();
            this.VlrComponente10 = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoID { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquidacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Ciudad { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorID { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDT_CuotaID NumCuotaVendidas { get; set; }

        [DataMember]
        public UDT_PorcentajeID TasaVenta { get; set; }

        [DataMember]
        public UDTSQL_char NumeroCesion { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDTSQL_decimal PorInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrPagos { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente1 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente2 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente3 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente4 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente5 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente6 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente7 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente8 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente9 { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente10 { get; set; }

        [DataMember]
        public UDT_CuotaID FlujosPago { get; set; }

        #endregion
    }
}
