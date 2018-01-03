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
    public class DTO_ccCierreDia
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCierreDia()
        {
            this.InitCols();
        }

        public DTO_ccCierreDia(IDataReader dr)
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
                this.CentroPagoID.Value = dr["CentroPagoID"].ToString();

                //Carga los valores con la información de CompLocal# para despeus procesarla
                if (!String.IsNullOrWhiteSpace(dr["ValorDia01"].ToString()))
                    this.ValorDia01.Value = Convert.ToDecimal(dr["ValorDia01"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia02"].ToString()))
                    this.ValorDia02.Value = Convert.ToDecimal(dr["ValorDia02"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia03"].ToString()))
                    this.ValorDia03.Value = Convert.ToDecimal(dr["ValorDia03"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia04"].ToString()))
                    this.ValorDia04.Value = Convert.ToDecimal(dr["ValorDia04"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia05"].ToString()))
                    this.ValorDia05.Value = Convert.ToDecimal(dr["ValorDia05"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia06"].ToString()))
                    this.ValorDia06.Value = Convert.ToDecimal(dr["ValorDia06"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia07"].ToString()))
                    this.ValorDia07.Value = Convert.ToDecimal(dr["ValorDia07"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia08"].ToString()))
                    this.ValorDia08.Value = Convert.ToDecimal(dr["ValorDia08"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia09"].ToString()))
                    this.ValorDia09.Value = Convert.ToDecimal(dr["ValorDia09"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia10"].ToString()))
                    this.ValorDia10.Value = Convert.ToDecimal(dr["ValorDia10"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia11"].ToString()))
                    this.ValorDia11.Value = Convert.ToDecimal(dr["ValorDia11"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia12"].ToString()))
                    this.ValorDia12.Value = Convert.ToDecimal(dr["ValorDia12"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia13"].ToString()))
                    this.ValorDia13.Value = Convert.ToDecimal(dr["ValorDia13"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia14"].ToString()))
                    this.ValorDia14.Value = Convert.ToDecimal(dr["ValorDia14"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia15"].ToString()))
                    this.ValorDia15.Value = Convert.ToDecimal(dr["ValorDia15"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia16"].ToString()))
                    this.ValorDia16.Value = Convert.ToDecimal(dr["ValorDia16"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia17"].ToString()))
                    this.ValorDia17.Value = Convert.ToDecimal(dr["ValorDia17"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia18"].ToString()))
                    this.ValorDia18.Value = Convert.ToDecimal(dr["ValorDia18"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia19"].ToString()))
                    this.ValorDia19.Value = Convert.ToDecimal(dr["ValorDia19"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia20"].ToString()))
                    this.ValorDia20.Value = Convert.ToDecimal(dr["ValorDia20"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia21"].ToString()))
                    this.ValorDia21.Value = Convert.ToDecimal(dr["ValorDia21"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia22"].ToString()))
                    this.ValorDia22.Value = Convert.ToDecimal(dr["ValorDia22"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia23"].ToString()))
                    this.ValorDia23.Value = Convert.ToDecimal(dr["ValorDia23"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia24"].ToString()))
                    this.ValorDia24.Value = Convert.ToDecimal(dr["ValorDia24"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia25"].ToString()))
                    this.ValorDia25.Value = Convert.ToDecimal(dr["ValorDia25"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia26"].ToString()))
                    this.ValorDia26.Value = Convert.ToDecimal(dr["ValorDia26"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia27"].ToString()))
                    this.ValorDia27.Value = Convert.ToDecimal(dr["ValorDia27"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia28"].ToString()))
                    this.ValorDia28.Value = Convert.ToDecimal(dr["ValorDia28"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia29"].ToString()))
                    this.ValorDia29.Value = Convert.ToDecimal(dr["ValorDia29"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia30"].ToString()))
                    this.ValorDia30.Value = Convert.ToDecimal(dr["ValorDia30"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorDia31"].ToString()))
                    this.ValorDia31.Value = Convert.ToDecimal(dr["ValorDia31"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="fromMvto">Carga las valores de movimientos para despues procesarlos (true), de lo contrario solo carga el valor del mes buscado</param>
        /// <param name="?"></param>
        public DTO_ccCierreDia(IDataReader dr, bool fromMvto)
        {
            InitCols();
            try
            {
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.CentroPagoID.Value = dr["CentroPagoID"].ToString();

                if (fromMvto)
                {
                    #region Movimiento
                    this.IdentificadorTR.Value = Convert.ToInt32(dr["NumDocCredito"]);
                    this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoOrigen"]);

                    //Carga los valores con la información de CompLocal# para despeus procesarla
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal1"].ToString()))
                        this.ValorDia01.Value = Convert.ToDecimal(dr["CompLocal1"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal2"].ToString()))
                        this.ValorDia02.Value = Convert.ToDecimal(dr["CompLocal2"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal3"].ToString()))
                        this.ValorDia03.Value = Convert.ToDecimal(dr["CompLocal3"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal4"].ToString()))
                        this.ValorDia04.Value = Convert.ToDecimal(dr["CompLocal4"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal5"].ToString()))
                        this.ValorDia05.Value = Convert.ToDecimal(dr["CompLocal5"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal6"].ToString()))
                        this.ValorDia06.Value = Convert.ToDecimal(dr["CompLocal6"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal7"].ToString()))
                        this.ValorDia07.Value = Convert.ToDecimal(dr["CompLocal7"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal8"].ToString()))
                        this.ValorDia08.Value = Convert.ToDecimal(dr["CompLocal8"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal9"].ToString()))
                        this.ValorDia09.Value = Convert.ToDecimal(dr["CompLocal9"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal10"].ToString()))
                        this.ValorDia10.Value = Convert.ToDecimal(dr["CompLocal10"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal11"].ToString()))
                        this.ValorDia11.Value = Convert.ToDecimal(dr["CompLocal11"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal12"].ToString()))
                        this.ValorDia12.Value = Convert.ToDecimal(dr["CompLocal12"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal13"].ToString()))
                        this.ValorDia13.Value = Convert.ToDecimal(dr["CompLocal13"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal14"].ToString()))
                        this.ValorDia14.Value = Convert.ToDecimal(dr["CompLocal14"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal15"].ToString()))
                        this.ValorDia15.Value = Convert.ToDecimal(dr["CompLocal15"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal16"].ToString()))
                        this.ValorDia16.Value = Convert.ToDecimal(dr["CompLocal16"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal17"].ToString()))
                        this.ValorDia17.Value = Convert.ToDecimal(dr["CompLocal17"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal18"].ToString()))
                        this.ValorDia18.Value = Convert.ToDecimal(dr["CompLocal18"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal19"].ToString()))
                        this.ValorDia19.Value = Convert.ToDecimal(dr["CompLocal19"]);
                    if (!String.IsNullOrWhiteSpace(dr["CompLocal20"].ToString()))
                        this.ValorDia20.Value = Convert.ToDecimal(dr["CompLocal20"]);
                    #endregion
                }
                else
                {
                    #region cierre mensual
                    this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                    this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                    this.TipoDato.Value = Convert.ToByte(dr["TipoDato"]);
                    this.ValorDia01.Value = Convert.ToDecimal(dr["ValorMes"]);
                    #endregion
                }

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
            this.IdentificadorTR = new UDT_Consecutivo();
            this.Periodo = new UDTSQL_datetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.AsesorID = new UDT_AsesorID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.ZonaID = new UDT_ZonaID();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.Plazo = new UDTSQL_smallint();
            this.TipoDato = new UDTSQL_tinyint();
            this.ValorDia01 = new UDT_Valor();
            this.ValorDia02 = new UDT_Valor();
            this.ValorDia03 = new UDT_Valor();
            this.ValorDia04 = new UDT_Valor();
            this.ValorDia05 = new UDT_Valor();
            this.ValorDia06 = new UDT_Valor();
            this.ValorDia07 = new UDT_Valor();
            this.ValorDia08 = new UDT_Valor();
            this.ValorDia09 = new UDT_Valor();
            this.ValorDia10 = new UDT_Valor();
            this.ValorDia11 = new UDT_Valor();
            this.ValorDia12 = new UDT_Valor();
            this.ValorDia13 = new UDT_Valor();
            this.ValorDia14 = new UDT_Valor();
            this.ValorDia15 = new UDT_Valor();
            this.ValorDia16 = new UDT_Valor();
            this.ValorDia17 = new UDT_Valor();
            this.ValorDia18 = new UDT_Valor();
            this.ValorDia19 = new UDT_Valor();
            this.ValorDia20 = new UDT_Valor();
            this.ValorDia21 = new UDT_Valor();
            this.ValorDia22 = new UDT_Valor();
            this.ValorDia23 = new UDT_Valor();
            this.ValorDia24 = new UDT_Valor();
            this.ValorDia25 = new UDT_Valor();
            this.ValorDia26 = new UDT_Valor();
            this.ValorDia27 = new UDT_Valor();
            this.ValorDia28 = new UDT_Valor();
            this.ValorDia29 = new UDT_Valor();
            this.ValorDia30 = new UDT_Valor();
            this.ValorDia31 = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo IdentificadorTR { get; set; }

        [DataMember]
        public UDTSQL_datetime Periodo { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

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
        public UDT_Valor ValorDia01 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia02 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia03 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia04 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia05 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia06 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia07 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia08 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia09 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia10 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia11 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia12 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia13 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia14 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia15 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia16 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia17 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia18 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia19 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia20 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia21 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia22 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia23 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia24 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia25 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia26 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia27 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia28 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia29 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia30 { get; set; }

        [DataMember]
        public UDT_Valor ValorDia31 { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion

    }
}
