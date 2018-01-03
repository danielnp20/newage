using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_QueryVentaCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryVentaCartera
    {
        #region DTO_QueryVentaCartera

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryVentaCartera()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Oferta = new UDT_DocTerceroID();
            this.FechaPago1 = new UDTSQL_smalldatetime();
            this.FechaPagoUlt = new UDTSQL_smalldatetime();
            this.Observacion = new UDT_DescripTBase();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.FactorCesion = new UDT_TasaID();
            this.VlrVenta = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.SaldoFlujo = new UDT_Valor();
            this.TotalLibranza = new UDTSQL_int();
            this.CredPendientes = new UDTSQL_int();
            this.CredMora = new UDTSQL_int();
            this.CredPrepagados = new UDTSQL_int();
            this.CredRecompra = new UDTSQL_int();
            this.Detalle = new List<DTO_QueryVentaCarteraDet>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPagoUlt { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }  

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_TasaID FactorCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujo { get; set; }

        [DataMember]
        public UDTSQL_int TotalLibranza { get; set; }

        [DataMember]
        public UDTSQL_int CredPendientes { get; set; }

        [DataMember]
        public UDTSQL_int CredMora { get; set; }

        [DataMember]
        public UDTSQL_int CredPrepagados { get; set; }

        [DataMember]
        public UDTSQL_int CredRecompra { get; set; }

        [DataMember]
        public List<DTO_QueryVentaCarteraDet> Detalle { get; set; }

        #endregion
    }

    /// <summary>
    /// Class Models DTO_prOrdenCompraAprobDet
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryVentaCarteraDet
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryVentaCarteraDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.Plazo = new UDTSQL_smallint();
            this.VlrCuota = new UDT_Valor();
            this.CuotasVEN = new UDT_CuotaID();
            this.VlrLibranza = new UDT_Valor();
            this.FactorCesion = new UDT_TasaID();
            this.VlrVenta = new UDT_Valor();
            this.SaldoFlujo = new UDT_Valor();
            this.AlturaFlujo = new UDT_CuotaID();
            this.SaldoFlujoCAP = new UDT_Valor();
            this.CuotasVEN = new UDT_CuotaID();
            this.CapitalVEN = new UDT_Valor();
            this.FechaDocPrepago = new UDTSQL_smalldatetime();
            this.FechaDocRecompra = new UDTSQL_smalldatetime();
            this.EC_Fecha = new UDTSQL_smalldatetime();
            this.CuotasFlujo = new UDT_CuotaID();       
        }

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDTSQL_int CuotasVend { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_TasaID FactorCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujo { get; set; }

        [DataMember]
        public UDT_CuotaID AlturaFlujo { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujoCAP { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasVEN { get; set; }

        [DataMember]
        public UDT_Valor CapitalVEN { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime EC_Fecha { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDocRecompra { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDocPrepago { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasFlujo { get; set; }

        #endregion
    }
}
