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
    /// Models DTO_ccCierreDiaCartera.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCierreDiaCartera
    {
        #region DTO_ccCierreDiaCartera

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCierreDiaCartera(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocControlIncumplido"].ToString()))
                    this.NumDocControlIncumplido.Value = Convert.ToInt32(dr["NumDocControlIncumplido"]);
                this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoEstado"].ToString()))
                    this.TipoEstado.Value = Convert.ToByte(dr["TipoEstado"]);


                if (!string.IsNullOrWhiteSpace(dr["FechaMora"].ToString()))
                    this.FechaMora.Value = Convert.ToDateTime(dr["FechaMora"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaUltPago"].ToString()))
                    this.FechaUltPago.Value = Convert.ToDateTime(dr["FechaUltPago"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCtaVencida"].ToString()))
                    this.FechaCtaVencida.Value = Convert.ToDateTime(dr["FechaCtaVencida"]);


                if (!string.IsNullOrWhiteSpace(dr["SaldoTotal"].ToString()))
                    this.SaldoTotal.Value = Convert.ToDecimal(dr["SaldoTotal"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoVencido"].ToString()))
                    this.SaldoVencido.Value = Convert.ToDecimal(dr["SaldoVencido"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrIntMora"].ToString()))
                    this.VlrIntMora.Value = Convert.ToDecimal(dr["VlrIntMora"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPrejuridico"].ToString()))
                    this.VlrPrejuridico.Value = Convert.ToDecimal(dr["VlrPrejuridico"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrOtros"].ToString()))
                    this.VlrOtros.Value = Convert.ToDecimal(dr["VlrOtros"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGarantia"].ToString()))
                    this.VlrGarantia.Value = Convert.ToDecimal(dr["VlrGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["PorGarantia"].ToString()))
                    this.PorGarantia.Value = Convert.ToDecimal(dr["PorGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["PtosGarantia"].ToString()))
                    this.PtosGarantia.Value = Convert.ToDecimal(dr["PtosGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["NumPrejuridico"].ToString()))
                    this.NumPrejuridico.Value = Convert.ToInt32(dr["NumPrejuridico"]);
                if (!string.IsNullOrWhiteSpace(dr["PorPrejuridico"].ToString()))
                    this.PorPrejuridico.Value = Convert.ToDecimal(dr["PorPrejuridico"]);
                if (!string.IsNullOrWhiteSpace(dr["PtosPrejuridico"].ToString()))
                    this.PtosPrejuridico.Value = Convert.ToDecimal(dr["PtosPrejuridico"]);
                if (!string.IsNullOrWhiteSpace(dr["Altura"].ToString()))
                    this.Altura.Value = Convert.ToString(dr["Altura"]);
                if (!string.IsNullOrWhiteSpace(dr["PorAltura"].ToString()))
                    this.PorAltura.Value = Convert.ToDecimal(dr["PorAltura"]);
                if (!string.IsNullOrWhiteSpace(dr["PtosAltura"].ToString()))
                    this.PtosAltura.Value = Convert.ToDecimal(dr["PtosAltura"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasPagadas"].ToString()))
                    this.CtasPagadas.Value = Convert.ToInt32(dr["CtasPagadas"]);
                if (!string.IsNullOrWhiteSpace(dr["PtosPagadas"].ToString()))
                    this.PtosPagadas.Value = Convert.ToDecimal(dr["PtosPagadas"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasVencidas"].ToString()))
                    this.CtasVencidas.Value = Convert.ToInt32(dr["CtasVencidas"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasMora"].ToString()))
                    this.DiasMora.Value = Convert.ToInt32(dr["DiasMora"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsEstadoIncumplido"].ToString()))
                    this.ConsEstadoIncumplido.Value = Convert.ToInt32(dr["ConsEstadoIncumplido"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCierreDiaCartera()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.Fecha = new UDTSQL_datetime();
            this.ClienteID = new UDT_ClienteID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocControlIncumplido = new UDT_Consecutivo();
            this.NumCredito = new UDT_Consecutivo();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.TipoEstado = new UDTSQL_tinyint();
            this.FechaMora = new UDTSQL_datetime();
            this.FechaCtaVencida = new UDTSQL_datetime();
            this.FechaUltPago = new UDTSQL_datetime();
            this.SaldoTotal = new UDT_Valor();
            this.SaldoVencido = new UDT_Valor();
            this.VlrIntMora = new UDT_Valor();
            this.VlrPrejuridico = new UDT_Valor();
            this.VlrOtros = new UDT_Valor();
            this.VlrGarantia = new UDT_Valor();
            this.PorGarantia = new UDT_PorcentajeID();
            this.PtosGarantia = new UDT_FactorID();
            this.NumPrejuridico = new UDT_Cantidad();
            this.PorPrejuridico = new UDT_PorcentajeID();
            this.PtosPrejuridico = new UDT_FactorID();
            this.Altura = new UDT_CodigoGrl10();
            this.PorAltura = new UDT_PorcentajeID();
            this.PtosAltura = new UDT_FactorID();
            this.CtasPagadas = new UDT_Cantidad();
            this.PtosPagadas = new UDT_FactorID();
            this.CtasVencidas = new UDT_Cantidad();
            this.DiasMora = new UDTSQL_int();
            this.ConsEstadoIncumplido = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocControlIncumplido { get; set; }

        [DataMember]
        public UDT_Consecutivo NumCredito { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }



        [DataMember]
        public UDTSQL_datetime FechaMora { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaUltPago { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaCtaVencida { get; set; }

        [DataMember]
        public UDT_Valor SaldoTotal { get; set; }

        [DataMember]
        public UDT_Valor SaldoVencido { get; set; }

        [DataMember]
        public UDT_Valor VlrIntMora { get; set; }

        [DataMember]
        public UDT_Valor VlrPrejuridico { get; set; }

        [DataMember]
        public UDT_Valor VlrOtros { get; set; }

        [DataMember]
        public UDT_Valor VlrGarantia { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorGarantia { get; set; }

        [DataMember]
        public UDT_FactorID PtosGarantia { get; set; }

        [DataMember]
        public UDT_Cantidad NumPrejuridico { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorPrejuridico { get; set; }

        [DataMember]
        public UDT_FactorID PtosPrejuridico { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 Altura { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorAltura { get; set; }

        [DataMember]
        public UDT_FactorID PtosAltura { get; set; }

        [DataMember]
        public UDT_Cantidad CtasPagadas { get; set; }

        [DataMember]
        public UDT_FactorID PtosPagadas { get; set; }

        [DataMember]
        public UDT_Cantidad CtasVencidas { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsEstadoIncumplido { get; set; }

        #endregion
    }
}
