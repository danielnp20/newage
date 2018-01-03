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
    /// Class comprobante para aprobacion:
    /// Models DTO_ccSolicitudDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCreditoPagos
    {
        #region DTO_ccCreditoPlanPagos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCreditoPagos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.CreditoCuotaNum.Value = Convert.ToInt32(dr["CreditoCuotaNum"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                //this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"]);
                this.PagoDocu.Value = Convert.ToInt32(dr["PagoDocu"]);
                if (!string.IsNullOrWhiteSpace(dr["ComponenteCarteraID"].ToString()))
                    this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                this.VlrInteres.Value = Convert.ToDecimal(dr["VlrInteres"]);
                this.VlrSeguro.Value = Convert.ToDecimal(dr["VlrSeguro"]);
                this.VlrOtro1.Value = Convert.ToDecimal(dr["VlrOtro1"]);
                this.VlrOtro2.Value = Convert.ToDecimal(dr["VlrOtro2"]);
                this.VlrOtro3.Value = Convert.ToDecimal(dr["VlrOtro3"]);
                this.VlrOtrosFijos.Value = Convert.ToDecimal(dr["VlrOtrosfijos"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCapitalCesion"].ToString()))
                    this.VlrCapitalCesion.Value = Convert.ToDecimal(dr["VlrCapitalCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrUtilidadCesion"].ToString()))
                    this.VlrUtilidadCesion.Value = Convert.ToDecimal(dr["VlrUtilidadCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDerechosCesion"].ToString()))
                    this.VlrDerechosCesion.Value = Convert.ToDecimal(dr["VlrDerechosCesion"]);
                this.VlrMoraliquida.Value = Convert.ToDecimal(dr["VlrMoraliquida"]);
                this.VlrMoraPago.Value = Convert.ToDecimal(dr["VlrMoraPago"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPrejuridicoPago"].ToString()))
                    this.VlrPrejuridicoPago.Value = Convert.ToDecimal(dr["VlrPrejuridicoPago"]);
                this.VlrAjusteUsura.Value = Convert.ToDecimal(dr["VlrAjusteUsura"]);
                this.VlrOtrosComponentes.Value = Convert.ToDecimal(dr["VlrOtrosComponentes"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaLiquidaMoraANT"].ToString()))
                    this.FechaLiquidaMoraANT.Value = Convert.ToDateTime(dr["FechaLiquidaMoraANT"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraLiquidaANT"].ToString()))
                    this.VlrMoraLiquidaANT.Value = Convert.ToDecimal(dr["VlrMoraLiquidaANT"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraPagoANT"].ToString()))
                    this.VlrMoraPagoANT.Value = Convert.ToDecimal(dr["VlrMoraPagoANT"]);
                this.DiasMora.Value = Convert.ToDecimal(dr["DiasMora"]);
                if (!string.IsNullOrWhiteSpace(dr["DocVenta"].ToString()))
                    this.DocVenta.Value = Convert.ToInt32(dr["DocVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoAnula"].ToString()))
                    this.DocumentoAnula.Value = Convert.ToInt32(dr["DocumentoAnula"]);
                this.TipoPago.Value = Convert.ToByte(dr["TipoPago"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCreditoPagos()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.CreditoCuotaNum = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PagoDocu = new UDT_Consecutivo();
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.Valor = new UDT_Valor();
            this.VlrCapital = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrOtro1 = new UDT_Valor();
            this.VlrOtro2 = new UDT_Valor();
            this.VlrOtro3 = new UDT_Valor();
            this.VlrOtrosFijos = new UDT_Valor();
            this.VlrCapitalCesion = new UDT_Valor();
            this.VlrUtilidadCesion = new UDT_Valor();
            this.VlrDerechosCesion = new UDT_Valor();
            this.VlrMoraliquida = new UDT_Valor();
            this.VlrMoraPago = new UDT_Valor();
            this.VlrPrejuridicoPago = new UDT_Valor();
            this.VlrAjusteUsura = new UDT_Valor();
            this.DiasMora = new UDT_Valor();
            this.DocVenta = new UDT_Consecutivo();
            this.DocumentoAnula = new UDT_Consecutivo();
            this.TipoPago = new UDTSQL_tinyint();
            this.VlrOtrosComponentes = new UDT_Valor();
            this.FechaLiquidaMoraANT = new  UDTSQL_smalldatetime();
            this.VlrMoraLiquidaANT = new UDT_Valor();
            this.VlrMoraPagoANT = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();

            //Campos Extra
            this.PagoInd = new UDT_SiNo();
            this.CuotaID = new UDT_Consecutivo();
            this.TipoDocumento = new UDT_Consecutivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.Comprobante = new UDT_DescripTBase();
            this.FechaPago = new UDTSQL_smalldatetime();
            this.FechaConsigna = new UDTSQL_smalldatetime();
            this.FechaCuota = new UDTSQL_smalldatetime();
            this.CajaID = new UDT_CajaID();
            this.BancoCuentaID = new UDT_BancoCuentaID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo CreditoCuotaNum { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo PagoDocu { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrOtro1 { get; set; }

        [DataMember]
        public UDT_Valor VlrOtro2 { get; set; }

        [DataMember]
        public UDT_Valor VlrOtro3 { get; set; }

        [DataMember]
        public UDT_Valor VlrOtrosFijos { get; set; }

        [DataMember]
        public UDT_Valor VlrCapitalCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrUtilidadCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrDerechosCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraliquida { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraPago { get; set; }

        [DataMember]
        public UDT_Valor VlrPrejuridicoPago { get; set; }
   
        [DataMember]
        public UDT_Valor VlrAjusteUsura { get; set; }

        [DataMember]
        public UDT_Valor DiasMora { get; set; }

        [DataMember]
        public UDT_Consecutivo DocVenta { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoAnula { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoPago { get; set; }

        [DataMember]
        public UDT_Valor VlrOtrosComponentes { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquidaMoraANT { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraLiquidaANT { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraPagoANT { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Campos extras

        [DataMember]
        [AllowNull]
        public UDT_SiNo PagoInd { get; set; }

        [DataMember]
        public UDT_Consecutivo CuotaID { get; set; }

        [DataMember]
        public UDT_Consecutivo TipoDocumento { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDT_DescripTBase Comprobante { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaConsigna { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota { get; set; }

        [DataMember]
        public UDT_CajaID CajaID { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        #endregion

    }
}
