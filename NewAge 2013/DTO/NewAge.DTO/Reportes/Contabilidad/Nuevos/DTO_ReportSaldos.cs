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
    public class DTO_ReportSaldos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportSaldos(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["BalanceTipoID"].ToString()))
                    this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["NombreCuenta"].ToString()))
                    this.NombreCuenta.Value = dr["NombreCuenta"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["NombreTercero"].ToString()))
                    this.NombreTercero.Value = dr["NombreTercero"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoInicialML"].ToString()))
                    this.SaldoInicialML.Value = Convert.ToDecimal(dr["SaldoInicialML"]);
                if (!string.IsNullOrEmpty(dr["CreditoML"].ToString()))
                    this.CreditoML.Value = Convert.ToDecimal(dr["CreditoML"]);
                if (!string.IsNullOrEmpty(dr["DebitoML"].ToString()))
                    this.DebitoML.Value = Convert.ToDecimal(dr["DebitoML"]);
                if (!string.IsNullOrEmpty(dr["FinalML"].ToString()))
                    this.FinalML.Value = Convert.ToDecimal(dr["FinalML"]);
                if (!string.IsNullOrEmpty(dr["SaldoInicialME"].ToString()))
                    this.SaldoInicialME.Value = Convert.ToDecimal(dr["SaldoInicialME"]);
                if (!string.IsNullOrEmpty(dr["DebitoME"].ToString()))
                    this.DebitoME.Value = Convert.ToDecimal(dr["DebitoME"]);
                if (!string.IsNullOrEmpty(dr["CreditoME"].ToString()))
                    this.CreditoME.Value = Convert.ToDecimal(dr["CreditoME"]);
                if (!string.IsNullOrEmpty(dr["FinalME"].ToString()))
                    this.FinalME.Value = Convert.ToDecimal(dr["FinalME"]);
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoDesc"].ToString()))
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaDesc"].ToString()))
                    this.LineaDesc.Value = dr["LineaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroDesc"].ToString()))
                    this.CentroDesc.Value = dr["CentroDesc"].ToString();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportSaldos(IDataReader dr, bool isNullble)
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
        public DTO_ReportSaldos()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new DateTime();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.CuentaID = new UDT_CuentaID();
            this.NombreCuenta = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.NombreTercero = new UDT_Descriptivo();
            this.SaldoInicialML = new UDT_Valor();
            this.CreditoML = new UDT_Valor();
            this.DebitoML = new UDT_Valor();
            this.FinalML = new UDT_Valor();
            this.SaldoInicialME = new UDT_Valor();
            this.DebitoME = new UDT_Valor();
            this.CreditoME = new UDT_Valor();
            this.FinalME = new UDT_Valor();
            this.ProyectoID = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LineaDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.CentroDesc = new UDT_Descriptivo();
        }

        #region Propiedades

        [DataMember]
        public DateTime PeriodoID { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreCuenta { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreTercero { get; set; }

        [DataMember]
        public UDT_Valor SaldoInicialML { get; set; }

        [DataMember]
        public UDT_Valor CreditoML { get; set; }

        [DataMember]
        public UDT_Valor DebitoML { get; set; }

        [DataMember]
        public UDT_Valor FinalML { get; set; }

        [DataMember]
        public UDT_Valor SaldoInicialME { get; set; }

        [DataMember]
        public UDT_Valor DebitoME { get; set; }

        [DataMember]
        public UDT_Valor CreditoME { get; set; }

        [DataMember]
        public UDT_Valor FinalME { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaDesc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroDesc { get; set; }

        #endregion

    }
}
