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
    public class DTO_ccCreditoPlanPagos
    {
        #region DTO_ccCreditoPlanPagos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCreditoPlanPagos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"]);
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                this.VlrInteres.Value = Convert.ToDecimal(dr["VlrInteres"]);
                this.VlrSeguro.Value = Convert.ToDecimal(dr["VlrSeguro"]);
                this.VlrOtro1.Value = Convert.ToDecimal(dr["VlrOtro1"]);
                this.VlrOtro2.Value = Convert.ToDecimal(dr["VlrOtro2"]);
                this.VlrOtro3.Value = Convert.ToDecimal(dr["VlrOtro3"]);
                this.VlrOtrosFijos.Value = Convert.ToDecimal(dr["VlrOtrosfijos"]);
                this.VlrSaldoCapital.Value = Convert.ToDecimal(dr["VlrSaldoCapital"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoSeguro"].ToString()))
                    this.VlrSaldoSeguro.Value = Convert.ToDecimal(dr["VlrSaldoSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoPago"].ToString()))
                    this.TipoPago.Value = Convert.ToByte(dr["TipoPago"]);
                this.FechaLiquidaMora.Value = Convert.ToDateTime(dr["FechaLiquidaMora"]);
                this.VlrMoraLiquida.Value = Convert.ToDecimal(dr["VlrMoraLiquida"]);
                this.VlrPagadoCuota.Value = Convert.ToDecimal(dr["VlrPagadoCuota"]);
                this.VlrPagadoExtras.Value = Convert.ToDecimal(dr["VlrPagadoExtras"]);
                this.VlrMoraPago.Value = Convert.ToDecimal(dr["VlrMoraPago"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotaFijadaInd"].ToString()))
                    this.CuotaFijadaInd.Value = Convert.ToBoolean(dr["CuotaFijadaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["DocVenta"].ToString()))
                    this.DocVenta.Value = Convert.ToInt32(dr["DocVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFlujo"].ToString()))
                    this.FechaFlujo.Value = Convert.ToDateTime(dr["FechaFlujo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCapitalCesion"].ToString()))
                    this.VlrCapitalCesion.Value = Convert.ToDecimal(dr["VlrCapitalCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrUtilidadCesion"].ToString()))
                    this.VlrUtilidadCesion.Value = Convert.ToDecimal(dr["VlrUtilidadCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDerechosCesion"].ToString()))
                    this.VlrDerechosCesion.Value = Convert.ToDecimal(dr["VlrDerechosCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoCapitalCesion"].ToString()))
                    this.VlrSaldoCapitalCesion.Value = Convert.ToDecimal(dr["VlrSaldoCapitalCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCapitalIFRS"].ToString()))
                    this.VlrCapitalIFRS.Value = Convert.ToDecimal(dr["VlrCapitalIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrUtilidadIFRS"].ToString()))
                    this.VlrUtilidadIFRS.Value = Convert.ToDecimal(dr["VlrUtilidadIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoCapitalIFRS"].ToString()))
                    this.VlrSaldoCapitalIFRS.Value = Convert.ToDecimal(dr["VlrSaldoCapitalIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["IndIntCausados"].ToString()))
                    this.IndIntCausados.Value = Convert.ToBoolean(dr["IndIntCausados"]);

                this.FechaLiquidaMoraANT.Value = this.FechaLiquidaMora.Value;
                this.VlrMoraLiquidaANT.Value = this.VlrMoraLiquida.Value;
                this.VlrMoraPagoANT.Value =this.VlrMoraPago.Value;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCreditoPlanPagos()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Consecutivo = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CuotaID = new UDT_CuotaID();
            this.FechaCuota = new UDTSQL_smalldatetime();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.VlrCuota = new UDT_Valor();
            this.VlrCapital = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrOtro1 = new UDT_Valor();
            this.VlrOtro2 = new UDT_Valor();
            this.VlrOtro3 = new UDT_Valor();
            this.VlrOtrosFijos = new UDT_Valor();
            this.VlrSaldoCapital = new UDT_Valor();
            this.VlrSaldoSeguro = new UDT_Valor();
            this.TipoPago = new UDTSQL_tinyint();
            this.FechaLiquidaMora = new UDTSQL_smalldatetime();
            this.VlrMoraLiquida = new UDT_Valor();
            this.VlrPagadoCuota = new UDT_Valor();
            this.VlrPagadoExtras = new UDT_Valor();
            this.VlrMoraPago = new UDT_Valor();
            this.CuotaFijadaInd = new UDT_SiNo();
            this.DocVenta = new UDT_Consecutivo();
            this.FechaFlujo = new UDTSQL_smalldatetime();
            this.VlrCapitalCesion = new UDT_Valor();
            this.VlrUtilidadCesion = new UDT_Valor();
            this.VlrDerechosCesion = new UDT_Valor();
            this.VlrSaldoCapitalCesion = new UDT_Valor();
            this.VlrCapitalIFRS = new UDT_Valor();
            this.VlrUtilidadIFRS = new UDT_Valor();
            this.VlrSaldoCapitalIFRS = new UDT_Valor();
            this.IndIntCausados = new UDT_SiNo();
            //Campos Extra
            this.PagoInd = new UDT_SiNo();
            this.FechaPago = new UDTSQL_smalldatetime();
            this.VlrAjusteUsura = new UDT_Valor();
            this.VlrPrejuridico = new UDT_Valor();
            this.VlrSaldo = new UDT_Valor();
            this.NuevoVlrMoraLiquida = new UDT_Valor();
            this.DocAnulaPag = new UDT_Consecutivo();
            this.ConsecutivoPag = new UDT_Consecutivo();
            this.VlrAbonos = new UDT_Valor();
            this.VlrNotaCredito = new UDT_Valor();
            this.VlrNuevoSaldo = new UDT_Valor();
            this.VlrSaldoComponentes = new UDT_Valor();
            this.VlrSaldoComponentes.Value = 0;
            this.FechaLiquidaMoraANT = new UDTSQL_smalldatetime();
            this.VlrMoraLiquidaANT = new UDT_Valor();
            this.VlrMoraPagoANT = new UDT_Valor();
            this.DiasMora = new UDTSQL_int();
            this.DetalleComp = new List<DTO_ccSaldosComponentes>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

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
        public UDT_Valor VlrSaldoCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoSeguro { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoPago { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquidaMora { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraLiquida { get; set; }

        [DataMember]
        public UDT_Valor VlrPagadoCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrPagadoExtras { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraPago { get; set; }

        [DataMember]
        public UDT_Valor VlrMora
        {
            get 
            {
                var vlrMoraLiquida = this.VlrMoraLiquida.Value.HasValue ? this.VlrMoraLiquida.Value.Value : 0;
                //var vlrMoraPago = this.VlrMoraPago.Value.HasValue ? this.VlrMoraPago.Value.Value : 0;
               
                //return new UDT_Valor() { Value = vlrMoraLiquida - vlrMoraPago };

                //Se cambio el 15 de sept por un error en Apoyos (Cliente 79650530 / Lib: 10022)
                return new UDT_Valor() { Value = vlrMoraLiquida };
            } 
        }

        [DataMember]
        public UDT_Consecutivo DocVenta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFlujo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo CuotaFijadaInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrCapitalCesion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrUtilidadCesion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrDerechosCesion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrSaldoCapitalCesion { get; set; }

        [DataMember]
        public UDT_Valor VlrCapitalIFRS { get; set; }

        [DataMember]
        public UDT_Valor VlrUtilidadIFRS { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoCapitalIFRS { get; set; }

        [DataMember]
        public UDT_SiNo IndIntCausados { get; set; }

        //Campos extras

        [DataMember]
        public UDT_SiNo PagoInd { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago { get; set; }

        [DataMember]
        public UDT_Valor VlrAjusteUsura { get; set; }

        [DataMember]
        public UDT_Valor VlrPrejuridico { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoComponentes
        {
            get;set;
            //{
            //    var vlrSaldo = this.VlrSaldo.Value.HasValue ? this.VlrSaldo.Value.Value : 0;
            //    var vlrPrejuridico = this.VlrPrejuridico.Value.HasValue ? this.VlrPrejuridico.Value.Value : 0;
            //    var vlrUsura = this.VlrAjusteUsura.Value.HasValue ? this.VlrAjusteUsura.Value.Value : 0;

            //    return new UDT_Valor() { Value = vlrSaldo - vlrUsura - vlrPrejuridico - this.VlrMora.Value.Value };
            //}
        }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquidaMoraANT { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraLiquidaANT { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraPagoANT { get; set; }

        [DataMember]
        public UDT_Valor NuevoVlrMoraLiquida { get; set; }

        [DataMember]
        public UDT_Consecutivo DocAnulaPag { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoPag { get; set; }

        [DataMember]
        public UDT_Valor VlrAbonos { get; set; }

        [DataMember]
        public UDT_Valor VlrNotaCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrNuevoSaldo { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public List<DTO_ccSaldosComponentes> DetalleComp { get; set; }

        [DataMember]
        public List<DTO_ccSentenciaPlanPagos> DetalleSentencia { get; set; }

        #endregion
    }
}
