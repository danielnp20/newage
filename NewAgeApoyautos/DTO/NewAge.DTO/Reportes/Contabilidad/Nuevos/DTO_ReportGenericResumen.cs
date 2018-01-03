using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Saldos
    /// </summary>
    public class DTO_ReportGenericResumen
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportGenericResumen(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["ConsLineaREP"].ToString()))
                    this.ConsLineaREP.Value = Convert.ToInt32(dr["ConsLineaREP"]);
                if (!string.IsNullOrEmpty(dr["ReporteID"].ToString()))
                    this.ReporteID.Value = dr["ReporteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ReporteDesc"].ToString()))
                    this.ReporteDesc.Value = dr["ReporteDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["RepLineaID"].ToString()))
                    this.RepLineaID.Value = dr["RepLineaID"].ToString();
                if (!string.IsNullOrEmpty(dr["RepLineaDesc"].ToString()))
                    this.RepLineaDesc.Value = dr["RepLineaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoIniActML"].ToString()))
                    this.SaldoIniActML.Value = Convert.ToDecimal(dr["SaldoIniActML"]);
                if (!string.IsNullOrEmpty(dr["SaldoIniActME"].ToString()))
                    this.SaldoIniActME.Value = Convert.ToDecimal(dr["SaldoIniActME"]);
                if (!string.IsNullOrEmpty(dr["DebitoActML"].ToString()))
                    this.DebitoActML.Value = Convert.ToDecimal(dr["DebitoActML"]);
                if (!string.IsNullOrEmpty(dr["CreditoActML"].ToString()))
                    this.CreditoActML.Value = Convert.ToDecimal(dr["CreditoActML"]);
                if (!string.IsNullOrEmpty(dr["DebitoActME"].ToString()))
                    this.DebitoActME.Value = Convert.ToDecimal(dr["DebitoActME"]);
                if (!string.IsNullOrEmpty(dr["CreditoActME"].ToString()))
                    this.CreditoActME.Value = Convert.ToDecimal(dr["CreditoActME"]);
                if (!string.IsNullOrEmpty(dr["SaldoIniAntML"].ToString()))
                    this.SaldoIniAntML.Value = Convert.ToDecimal(dr["SaldoIniAntML"]);
                if (!string.IsNullOrEmpty(dr["SaldoIniAntME"].ToString()))
                    this.SaldoIniAntME.Value = Convert.ToDecimal(dr["SaldoIniAntME"]);
                if (!string.IsNullOrEmpty(dr["DebitoAntML"].ToString()))
                    this.DebitoAntML.Value = Convert.ToDecimal(dr["DebitoAntML"]);
                if (!string.IsNullOrEmpty(dr["CreditoAntML"].ToString()))
                    this.CreditoAntML.Value = Convert.ToDecimal(dr["CreditoAntML"]);
                if (!string.IsNullOrEmpty(dr["DebitoAntME"].ToString()))
                    this.DebitoAntME.Value = Convert.ToDecimal(dr["DebitoAntME"]);
                if (!string.IsNullOrEmpty(dr["CreditoAntME"].ToString()))
                    this.CreditoAntME.Value = Convert.ToDecimal(dr["CreditoAntME"]);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportGenericResumen(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {

            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportGenericResumen()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new DateTime();
            this.ConsLineaREP = new UDT_Consecutivo();
            this.ReporteID=new UDT_ReporteID();
            this.ReporteDesc = new UDT_Descriptivo();
            this.RepLineaID=new UDT_RepLineaID();
            this.RepLineaDesc=new UDT_Descriptivo();
            this.SaldoIniActML=new UDT_Valor();
            this.SaldoIniActME=new UDT_Valor();
            this.DebitoActML=new UDT_Valor();
            this.CreditoActML=new UDT_Valor();
            this.DebitoActME=new UDT_Valor();
            this.CreditoActME=new UDT_Valor();
            this.SaldoIniAntML=new UDT_Valor();
            this.SaldoIniAntME=new UDT_Valor();
            this.DebitoAntML=new UDT_Valor();
            this.CreditoAntML=new UDT_Valor();
            this.DebitoAntME=new UDT_Valor();
            this.CreditoAntME=new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public DateTime PeriodoID { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsLineaREP { get; set; }

        [DataMember]
        public UDT_ReporteID ReporteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ReporteDesc { get; set; }

        [DataMember]
        public UDT_RepLineaID RepLineaID { get; set; }

        [DataMember]
        public UDT_Descriptivo RepLineaDesc { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniActML { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniActME { get; set; }

        [DataMember]
        public UDT_Valor DebitoActML { get; set; }

        [DataMember]
        public UDT_Valor CreditoActML { get; set; }

        [DataMember]
        public UDT_Valor DebitoActME { get; set; }

        [DataMember]
        public UDT_Valor CreditoActME { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniAntML { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniAntME { get; set; }

        [DataMember]
        public UDT_Valor DebitoAntML { get; set; }

        [DataMember]
        public UDT_Valor CreditoAntML { get; set; }

        [DataMember]
        public UDT_Valor DebitoAntME { get; set; }

        [DataMember]
        public UDT_Valor CreditoAntME { get; set; }
        #endregion

    }
}
