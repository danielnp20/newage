using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ccCierreMes
    {
        public DTO_ccCierreMes(IDataReader dr)
        {
            InitCols();
            try
            {
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.TipoDato.Value = Convert.ToByte(dr["TipoDato"]);
                //this.CentroPagoID.Value = dr["CentroPagoID"].ToString();

                //Carga los valores con la información de CompLocal# para despeus procesarla
                if (!String.IsNullOrWhiteSpace(dr["ValorMes01"].ToString()))
                    this.ValorMes01.Value = Convert.ToDecimal(dr["ValorMes01"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes02"].ToString()))
                    this.ValorMes02.Value = Convert.ToDecimal(dr["ValorMes02"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes03"].ToString()))
                    this.ValorMes03.Value = Convert.ToDecimal(dr["ValorMes03"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes04"].ToString()))
                    this.ValorMes04.Value = Convert.ToDecimal(dr["ValorMes04"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes05"].ToString()))
                    this.ValorMes05.Value = Convert.ToDecimal(dr["ValorMes05"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes06"].ToString()))
                    this.ValorMes06.Value = Convert.ToDecimal(dr["ValorMes06"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes07"].ToString()))
                    this.ValorMes07.Value = Convert.ToDecimal(dr["ValorMes07"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes08"].ToString()))
                    this.ValorMes08.Value = Convert.ToDecimal(dr["ValorMes08"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes09"].ToString()))
                    this.ValorMes09.Value = Convert.ToDecimal(dr["ValorMes09"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes10"].ToString()))
                    this.ValorMes10.Value = Convert.ToDecimal(dr["ValorMes10"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes11"].ToString()))
                    this.ValorMes11.Value = Convert.ToDecimal(dr["ValorMes11"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorMes12"].ToString()))
                    this.ValorMes12.Value = Convert.ToDecimal(dr["ValorMes12"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.AsesorID = new UDT_AsesorID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.ZonaID = new UDT_ZonaID();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.Plazo = new UDTSQL_smallint();
            this.TipoDato = new UDTSQL_tinyint();
            this.ValorMes01 = new UDT_Valor();
            this.ValorMes02 = new UDT_Valor();
            this.ValorMes03 = new UDT_Valor();
            this.ValorMes04 = new UDT_Valor();
            this.ValorMes05 = new UDT_Valor();
            this.ValorMes06 = new UDT_Valor();
            this.ValorMes07 = new UDT_Valor();
            this.ValorMes08 = new UDT_Valor();
            this.ValorMes09 = new UDT_Valor();
            this.ValorMes10 = new UDT_Valor();
            this.ValorMes11 = new UDT_Valor();
            this.ValorMes12 = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #region Propiedades

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoID { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDato { get; set; }

        [DataMember]
        public UDT_Valor ValorMes01 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes02 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes03 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes04 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes05 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes06 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes07 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes08 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes09 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes10 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes11 { get; set; }

        [DataMember]
        public UDT_Valor ValorMes12 { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion

    }
}
