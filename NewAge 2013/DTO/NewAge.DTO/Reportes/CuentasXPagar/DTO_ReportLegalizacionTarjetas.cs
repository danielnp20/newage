using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Drawing;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del documetno Anticipo
    /// </summary>
    public class DTO_ReportLegalizacionTarjetas : DTO_ReportBase
    {
        #region Constructores
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportLegalizacionTarjetas(IDataReader dr)
        {

            InitCols();

            if (!string.IsNullOrEmpty(dr["TarjetaCredito"].ToString()))
                this.TarjetaCredito.Value = dr["TarjetaCredito"].ToString();
            if (!string.IsNullOrEmpty(dr["FechaIni"].ToString()))
                this.FechaIni.Value = Convert.ToDateTime(dr["FechaIni"]);
            if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
            if (!string.IsNullOrEmpty(dr["FechaSolicita"].ToString()))
                this.FechaSolicita.Value = Convert.ToDateTime(dr["FechaSolicita"]);
            if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                this.TerceroID.Value = dr["TerceroID"].ToString();
            if (!string.IsNullOrEmpty(dr["NombreTer"].ToString()))
                this.NombreTer.Value = dr["NombreTer"].ToString();
            if (!string.IsNullOrEmpty(dr["CargoEspDesc"].ToString()))
                this.CargoEspDesc.Value = dr["CargoEspDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["CentroCostoDesc"].ToString()))
                this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["ProyectoDesc"].ToString()))
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["ValorBruto"].ToString()))
                this.ValorBruto.Value = Convert.ToDecimal(dr["ValorBruto"]);
            if (!string.IsNullOrEmpty(dr["ValorRteICA"].ToString()))
                this.ValorRteICA.Value = Convert.ToDecimal(dr["ValorRteICA"]);
            if (!string.IsNullOrEmpty(dr["ValorRteFuente"].ToString()))
                this.ValorRteFuente.Value = Convert.ToDecimal(dr["ValorRteFuente"]);
            if (!string.IsNullOrEmpty(dr["ValorIVA1"].ToString()))
                this.ValorIVA1.Value = Convert.ToDecimal(dr["ValorIVA1"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        private DTO_ReportLegalizacionTarjetas()
        {
            this.InitCols();
        }

        // Inicializa las columnas
        // </summary>
        private void InitCols()
        {
            this.TarjetaCredito = new UDT_TarjetaCreditoID();
            this.FechaIni = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.FechaSolicita = new UDTSQL_smalldatetime();
            this.TerceroID = new UDT_TerceroID();
            this.NombreTer = new UDT_Descriptivo();
            this.CargoEspDesc = new UDT_Descriptivo();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.ValorBruto = new UDT_Valor();
            this.ValorRteICA = new UDT_Valor();
            this.ValorRteFuente = new UDT_Valor();
            this.ValorIVA1 = new UDT_Valor();
            this.Item = new UDTSQL_int();
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public UDT_TarjetaCreditoID TarjetaCredito { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIni { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaSolicita { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreTer { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoEspDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_Valor ValorBruto { get; set; }

        [DataMember]
        public UDT_Valor ValorRteICA { get; set; }

        [DataMember]
        public UDT_Valor ValorRteFuente { get; set; }

        [DataMember]
        public UDT_Valor ValorIVA1 { get; set; }
        // Propiedades que no vienen en la consulta

        [DataMember]
        public UDTSQL_int Item { get; set; }

        #endregion
    }
}
