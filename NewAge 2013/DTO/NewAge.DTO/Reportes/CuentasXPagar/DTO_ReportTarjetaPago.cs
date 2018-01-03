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
    public class DTO_ReportTarjetaPago : DTO_ReportBase
    {
        #region Constructores
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportTarjetaPago(IDataReader dr)
        {

            InitCols();

            if (!string.IsNullOrEmpty(dr["NumTarjeta"].ToString()))
                this.NumTarjeta.Value = dr["NumTarjeta"].ToString();
            if (!string.IsNullOrEmpty(dr["PeriodoPago"].ToString()))
                this.PeriodoPago.Value = Convert.ToDateTime(dr["PeriodoPago"]);
            if (!string.IsNullOrEmpty(dr["NombreTercero"].ToString()))
                this.NombreTercero.Value = dr["NombreTercero"].ToString();
            if (!string.IsNullOrEmpty(dr["CargoID"].ToString()))
                this.CargoID.Value = dr["CargoID"].ToString();
            if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
            if (!string.IsNullOrEmpty(dr["ValorCargoEsp"].ToString()))
                this.ValorCargoEsp.Value = Convert.ToDecimal(dr["ValorCargoEsp"]);
            this.Item.Value = Convert.ToInt16(dr["Item"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        private DTO_ReportTarjetaPago()
        {
            this.InitCols();
        }

        // Inicializa las columnas
        // </summary>
        private void InitCols()
        {

            this.NumTarjeta = new UDT_TarjetaCreditoID();
            this.PeriodoPago = new UDTSQL_smalldatetime();
            this.NombreTercero = new UDT_Descriptivo();
            this.CargoID = new UDT_CargoEspecialID();
            this.Descriptivo = new UDT_Descriptivo();
            this.ValorCargoEsp = new UDT_Valor();
            this.Item = new UDTSQL_int();
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public UDT_TarjetaCreditoID NumTarjeta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime PeriodoPago { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreTercero { get; set; }

        [DataMember]
        public UDT_CargoEspecialID CargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor ValorCargoEsp { get; set; }

        // Propiedades que no vienen en la consulta

        [DataMember]
        public UDTSQL_int Item { get; set; }

        #endregion
    }
}
