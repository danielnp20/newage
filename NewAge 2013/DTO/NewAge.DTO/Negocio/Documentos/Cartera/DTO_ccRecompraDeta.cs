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
    /// Class comprobante para aprobacion:
    /// Models DTO_ccCreditoDocu.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccRecompraDeta
    {
        #region DTO_ccRecompraDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccRecompraDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocSustituye"].ToString()))
                    this.NumDocSustituye.Value = Convert.ToInt32(dr["NumDocSustituye"]);
                if (!string.IsNullOrWhiteSpace(dr["Portafolio"].ToString())) 
                    this.Portafolio.Value = dr["Portafolio"].ToString();
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.CuotasRecompra.Value = Convert.ToInt32(dr["CuotasRecompra"]);
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.VlrRecompra.Value = Convert.ToDecimal(dr["VlrRecompra"]);
                this.VlrDerechos.Value = Convert.ToDecimal(dr["VlrDerechos"]);
                this.FactorRecompra.Value = Convert.ToInt32(dr["FactorRecompra"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSustLibranza"].ToString()))
                    this.VlrSustLibranza.Value = Convert.ToDecimal(dr["VlrSustLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSustRecompra"].ToString()))
                    this.VlrSustRecompra.Value = Convert.ToDecimal(dr["VlrSustRecompra"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrNeto"].ToString()))
                    this.VlrNeto.Value = Convert.ToDecimal(dr["VlrNeto"]);
                if (!string.IsNullOrWhiteSpace(dr["SustituyeInd"].ToString())) 
                    this.SustituyeInd.Value = Convert.ToBoolean(dr["SustituyeInd"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccRecompraDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumDocSustituye = new UDT_Consecutivo();
            this.Portafolio = new UDT_PortafolioID();
            this.CuotaID = new UDT_CuotaID();
            this.VlrCuota = new UDT_Valor();
            this.CuotasRecompra = new UDT_CuotaID();
            this.VlrLibranza = new UDT_Valor();
            this.VlrRecompra = new UDT_Valor();
            this.VlrDerechos = new UDT_Valor();
            this.FactorRecompra = new UDT_TasaID();
            this.VlrSustLibranza = new UDT_Valor();
            this.VlrSustRecompra = new UDT_Valor();
            this.VlrNeto = new UDT_Valor();
            this.SustituyeInd = new UDT_SiNo();

            //Campos Adicionales
            this.Aprobado = new UDT_SiNo();
            this.PrepagoInd = new UDT_SiNo();
            this.ClienteID = new UDT_ClienteID();
            this.ClaseCredito = new UDT_CodigoGrl5();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.Libranza = new UDT_LibranzaID();
            this.LibranzaSustituida = new UDT_LibranzaID();
            this.Nombre = new UDT_Descriptivo();
            this.Oferta = new UDT_DocTerceroID();
            this.TipoEstado = new UDTSQL_tinyint();
            this.VlrUtilidad = new UDT_Valor();
            this.VlrPrepago = new UDT_Valor();
            this.FlujosPagados = new UDTSQL_int();
            this.VlrSaldosPagos = new UDT_Valor();
            this.VlrCapital = new UDT_Valor();          
            this.VlrInteres = new UDT_Valor();
            this.VlrCapitalContab = new UDT_Valor();
            this.VlrInteresContab = new UDT_Valor();
            this.VlrCapitalCesion = new UDT_Valor();
            this.VlrUtilidadCesion = new UDT_Valor();
            this.VlrDerechosCesion = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocSustituye { get; set; }

        [DataMember]
        public UDT_PortafolioID Portafolio { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasRecompra { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrRecompra { get; set; }

        [DataMember]
        public UDT_Valor VlrDerechos { get; set; }

        [DataMember]
        public UDT_TasaID FactorRecompra { get; set; }

        [DataMember]
        public UDT_Valor VlrSustLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrSustRecompra { get; set; }

        [DataMember]
        public UDT_Valor VlrNeto { get; set; }

        [DataMember]
        public UDT_SiNo SustituyeInd { get; set; }

        #endregion

        #region Campos Adicionales

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo PrepagoInd { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 ClaseCredito { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; } 
        
        [DataMember]
        public UDT_LibranzaID LibranzaSustituida { get; set; } 

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDT_Valor VlrUtilidad { get; set; }

        [DataMember]
        public UDT_Valor VlrPrepago { get; set; }

        [DataMember]
        public UDTSQL_int FlujosPagados { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldosPagos { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrCapitalContab { get; set; }

        [DataMember]
        public UDT_Valor VlrInteresContab { get; set; }

        [DataMember]
        public UDT_Valor VlrCapitalCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrDerechosCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrUtilidadCesion { get; set; }
        #endregion
    }
}
