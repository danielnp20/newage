using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ccAnalisisPagosReport
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccAnalisisPagosReport(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ComprobanteIDNro"].ToString()))
                    this.ComprobanteIDNro.Value = Convert.ToInt32(dr["ComprobanteIDNro"]);
                this.TipoDocumento.Value = dr["TipoDocumento"].ToString();
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                if (!string.IsNullOrEmpty(dr["ValorDoc"].ToString()))
                    this.ValorDoc.Value = Convert.ToDecimal(dr["ValorDoc"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.Ciudad.Value = dr["Ciudad"].ToString();
                this.ConcesionarioID.Value = dr["ConcesionarioID"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.CompLocal1.Value = Convert.ToDecimal(dr["CompLocal1"]);
                this.CompLocal2.Value = Convert.ToDecimal(dr["CompLocal2"]);
                this.CompLocal3.Value = Convert.ToDecimal(dr["CompLocal3"]);
                this.CompLocal4.Value = Convert.ToDecimal(dr["CompLocal4"]);
                this.CompLocal7.Value = Convert.ToDecimal(dr["CompLocal7"]);
                this.CompLocal8.Value = Convert.ToDecimal(dr["CompLocal8"]);
                this.CompLocal9.Value = Convert.ToDecimal(dr["CompLocal9"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccAnalisisPagosReport()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteIDNro = new UDT_Consecutivo();
            this.TipoDocumento = new UDT_DescripTBase();
            this.DocumentoID = new UDT_DocumentoID();
            this.ValorDoc = new UDT_Valor();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_Consecutivo();
            this.ZonaID = new UDT_ZonaID();
            this.Ciudad = new UDT_LugarGeograficoID();
            this.ConcesionarioID = new UDT_PagaduriaID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.AsesorID = new UDT_AsesorID();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.VlrTotalComp = new UDT_Valor();
            this.CompLocal1 = new UDT_Valor();
            this.CompLocal2 = new UDT_Valor();
            this.CompLocal3 = new UDT_Valor();
            this.CompLocal4 = new UDT_Valor();
            this.CompLocal5 = new UDT_Valor();
            this.CompLocal6 = new UDT_Valor();
            this.CompLocal7 = new UDT_Valor();
            this.CompLocal8 = new UDT_Valor();
            this.CompLocal9 = new UDT_Valor();
            this.CompLocal10 = new UDT_Valor();
            this.CompLocal11 = new UDT_Valor();
            this.CompLocal12 = new UDT_Valor();
            this.CompLocal13 = new UDT_Valor();
            this.CompLocal14 = new UDT_Valor();
            this.CompLocal15 = new UDT_Valor();
            this.CompLocal16 = new UDT_Valor();
            this.CompLocal17 = new UDT_Valor();
            this.CompLocal18 = new UDT_Valor();
            this.CompLocal19 = new UDT_Valor();
            this.CompLocal20 = new UDT_Valor();
            this.Detalle = new List<DTO_ccAnalisisPagosReport>();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteIDNro { get; set; }

        [DataMember]
        public UDT_DescripTBase TipoDocumento { get; set; } //Comp+Nro

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_Valor ValorDoc { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCartera { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo Libranza { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Ciudad { get; set; }

        [DataMember]
        public UDT_PagaduriaID ConcesionarioID { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoID { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Valor VlrTotalComp { get; set; }

        [DataMember]
        public UDT_Valor CompLocal1 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal2 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal3 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal4 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal5 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal6 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal7 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal8 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal9 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal10 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal11 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal12 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal13 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal14 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal15 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal16 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal17 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal18 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal19 { get; set; }

        [DataMember]
        public UDT_Valor CompLocal20 { get; set; }

        [DataMember]
        public List<DTO_ccAnalisisPagosReport> Detalle { get; set; }

        #endregion
    }
}
