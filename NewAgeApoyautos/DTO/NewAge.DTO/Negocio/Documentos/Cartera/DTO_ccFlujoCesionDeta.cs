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
    /// Models DTO_ccFlujoCesionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccFlujoCesionDeta
    {
        #region DTO_ccFlujoCesionDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccFlujoCesionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CreditoCuotaNum.Value = Convert.ToInt32(dr["CreditoCuotaNum"]);
                this.VentaDocNum.Value = Convert.ToInt32(dr["VentadocNum"]);
                this.Libranza.Value = dr["Libranza"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Oferta"].ToString()))
                    this.Oferta.Value = dr["Oferta"].ToString();
                this.Valor.Value = Convert.ToInt32(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["DocPago"].ToString()))
                    this.DocPago.Value = Convert.ToInt32(dr["DocPago"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCapitalCesion"].ToString()))
                    this.VlrCapitalCesion.Value = Convert.ToInt32(dr["VlrCapitalCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrUtilidadCesion"].ToString()))
                    this.VlrUtilidadCesion.Value = Convert.ToInt32(dr["VlrUtilidadCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDerechosCesion"].ToString()))
                    this.VlrDerechosCesion.Value = Convert.ToInt32(dr["VlrDerechosCesion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccFlujoCesionDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CreditoCuotaNum = new UDT_Consecutivo();
            this.VentaDocNum = new UDT_Consecutivo();
            this.Libranza = new UDT_DocTerceroID();
            this.Oferta = new UDT_DocTerceroID();
            this.Valor = new UDT_Valor();
            this.DocPago = new UDT_Consecutivo();
            this.VlrCapitalCesion = new UDT_Valor();
            this.VlrUtilidadCesion = new UDT_Valor();
            this.VlrDerechosCesion = new UDT_Valor();

            //Campos Adicionales
            this.Inversionista = new UDT_CompradorCarteraID();
            this.NumDocCedito = new UDT_Consecutivo();
            this.FechaPago = new UDTSQL_smalldatetime();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo CreditoCuotaNum { get; set; }

        [DataMember]
        public UDT_Consecutivo VentaDocNum { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Consecutivo DocPago { get; set; }

        [DataMember]
        public UDT_Valor VlrCapitalCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrUtilidadCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrDerechosCesion { get; set; }        

        #endregion

        #region Campos Adicionales

        [DataMember]
        public UDT_CompradorCarteraID Inversionista { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCedito { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago { get; set; }

        #endregion
    }
}
