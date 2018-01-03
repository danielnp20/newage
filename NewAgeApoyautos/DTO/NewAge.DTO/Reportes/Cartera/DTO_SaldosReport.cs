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
    public class DTO_SaldosReport
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_SaldosReport(IDataReader dr, bool isCuota)
        {
            this.InitCols();
            try
            {
                //Campos Generales
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["CapitalAbo"].ToString()))
                    this.CapitalAbo.Value = Convert.ToDecimal(dr["CapitalAbo"]);
                if (!string.IsNullOrEmpty(dr["InteresAbo"].ToString()))
                    this.InteresAbo.Value = Convert.ToDecimal(dr["InteresAbo"]);
                if (!string.IsNullOrEmpty(dr["SeguCapAbo"].ToString()))
                    this.SeguCapAbo.Value = Convert.ToDecimal(dr["SeguCapAbo"]);
                if (!string.IsNullOrEmpty(dr["SeguIntAbo"].ToString()))
                    this.SeguIntAbo.Value = Convert.ToDecimal(dr["SeguIntAbo"]);
                if (isCuota)
                {
                    this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                    this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"]);
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    this.SaldoCapital.Value = Convert.ToDecimal(dr["SaldoCapital"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(dr["CapitalSdo"].ToString()))
                        this.CapitalSdo.Value = Convert.ToDecimal(dr["CapitalSdo"]);
                    if (!string.IsNullOrEmpty(dr["InteresSdo"].ToString()))
                        this.InteresSdo.Value = Convert.ToDecimal(dr["InteresSdo"]);
                    if (!string.IsNullOrEmpty(dr["SeguCapSdo"].ToString()))
                        this.SeguCapSdo.Value = Convert.ToDecimal(dr["SeguCapSdo"]);
                    if (!string.IsNullOrEmpty(dr["SeguIntSdo"].ToString()))
                        this.SeguIntSdo.Value = Convert.ToDecimal(dr["SeguIntSdo"]);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SaldosReport()
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
            this.Nombre = new UDT_Descriptivo();
            this.EstadoCartera = new UDTSQL_tinyint();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ZonaID = new UDT_ZonaID();
            this.Ciudad = new UDT_LugarGeograficoID();
            this.ConcesionarioID = new UDT_PagaduriaID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.AsesorID = new UDT_AsesorID();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.VlrTotal = new UDT_Valor();
            this.CapitalAbo = new UDT_Valor();
            this.InteresAbo = new UDT_Valor();
            this.SeguCapAbo = new UDT_Valor();
            this.SeguIntAbo = new UDT_Valor();
            this.CapitalSdo = new UDT_Valor();
            this.InteresSdo = new UDT_Valor();
            this.SeguCapSdo = new UDT_Valor();
            this.SeguIntSdo = new UDT_Valor();
            this.Detalle = new List<DTO_SaldosReport>();
            //Cuota
            this.CuotaID = new UDT_CuotaID();
            this.FechaCuota = new UDTSQL_smalldatetime();
            this.VlrCuota = new UDT_Valor();
            this.SaldoCapital = new UDT_Valor();

        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCartera { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

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
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor SaldoCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrTotal { get; set; }

        [DataMember]
        public UDT_Valor CapitalAbo { get; set; }

        [DataMember]
        public UDT_Valor InteresAbo { get; set; }

        [DataMember]
        public UDT_Valor SeguCapAbo { get; set; }

        [DataMember]
        public UDT_Valor SeguIntAbo { get; set; }

        [DataMember]
        public UDT_Valor CapitalSdo { get; set; }

        [DataMember]
        public UDT_Valor InteresSdo { get; set; }

        [DataMember]
        public UDT_Valor SeguCapSdo { get; set; }

        [DataMember]
        public UDT_Valor SeguIntSdo { get; set; }

        [DataMember]
        public List<DTO_SaldosReport> Detalle { get; set; }

        #endregion
    }
}
