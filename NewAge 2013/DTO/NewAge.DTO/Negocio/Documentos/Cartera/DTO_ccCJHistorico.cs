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
    /// Models DTO_ccCJHistorico.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCJHistorico : DTO_SerializedObject
    {
        #region DTO_ccCJHistorico

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCJHistorico(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocMvto"].ToString()))
                    this.NumeroDocMvto.Value = Convert.ToInt32(dr["NumeroDocMvto"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocEstadoCta"].ToString()))
                    this.NumDocEstadoCta.Value = Convert.ToInt32(dr["NumDocEstadoCta"]);
                this.ClaseDeuda.Value = Convert.ToByte(dr["ClaseDeuda"].ToString());
                this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"].ToString());
                this.TipoMvto.Value = Convert.ToByte(dr["TipoMvto"].ToString());
                this.FechaMvto.Value = Convert.ToDateTime(dr["FechaMvto"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicial"].ToString()))
                    this.FechaInicial.Value = Convert.ToDateTime(dr["FechaInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinal"].ToString()))
                    this.FechaFinal.Value = Convert.ToDateTime(dr["FechaFinal"]);
                if (!string.IsNullOrWhiteSpace(dr["Dias"].ToString()))
                    this.Dias.Value = Convert.ToInt32(dr["Dias"]);
                if (!string.IsNullOrWhiteSpace(dr["PorInteres"].ToString()))               
                    this.PorInteres.Value = Convert.ToDecimal(dr["PorInteres"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotaID"].ToString()))
                    this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                if (!string.IsNullOrWhiteSpace(dr["DocPagoUltimaCuota"].ToString()))
                    this.DocPagoUltimaCuota.Value = Convert.ToInt32(dr["DocPagoUltimaCuota"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPagado"].ToString()))
                    this.VlrPagado.Value = Convert.ToDecimal(dr["VlrPagado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPendiente"].ToString()))
                    this.VlrPendiente.Value = Convert.ToDecimal(dr["VlrPendiente"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoCapital"].ToString()))
                    this.SaldoCapital.Value = Convert.ToDecimal(dr["SaldoCapital"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoInteres"].ToString()))
                    this.SaldoInteres.Value = Convert.ToDecimal(dr["SaldoInteres"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoGastos"].ToString()))
                    this.SaldoGastos.Value = Convert.ToDecimal(dr["SaldoGastos"]);
                if (!string.IsNullOrWhiteSpace(dr["MvtoCapital"].ToString()))
                    this.MvtoCapital.Value = Convert.ToDecimal(dr["MvtoCapital"]);
                if (!string.IsNullOrWhiteSpace(dr["MvtoInteres"].ToString()))
                    this.MvtoInteres.Value = Convert.ToDecimal(dr["MvtoInteres"]);
                if (!string.IsNullOrWhiteSpace(dr["MvtoGastos"].ToString()))
                    this.MvtoGastos.Value = Convert.ToDecimal(dr["MvtoGastos"]);
                if (!string.IsNullOrWhiteSpace(dr["PeriodoID"].ToString()))
                    this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrWhiteSpace(dr["FijadoInd"].ToString()))
                    this.FijadoInd.Value = Convert.ToBoolean(dr["FijadoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["Orden"].ToString()))
                    this.Orden.Value = Convert.ToByte(dr["Orden"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrInforme"].ToString()))
                    this.VlrInforme.Value = Convert.ToDecimal(dr["VlrInforme"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrJuzgado"].ToString()))
                    this.VlrJuzgado.Value = Convert.ToDecimal(dr["VlrJuzgado"]);

                try
                {
                    if (!string.IsNullOrWhiteSpace(dr["CompNro"].ToString()))
                    {
                        this.Comprobante.Value = dr["CompID"].ToString() + "-" + dr["CompNro"].ToString();
                    }
                }
                catch (Exception ex) { }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCJHistorico()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.EmpresaID = new UDT_BasicID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumeroDocMvto = new UDT_Consecutivo();
            this.NumDocEstadoCta = new UDT_Consecutivo();
            this.ClaseDeuda = new UDTSQL_tinyint();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.TipoMvto = new UDTSQL_tinyint();
            this.FechaMvto = new UDTSQL_smalldatetime();
            this.FechaInicial = new UDTSQL_smalldatetime();
            this.FechaFinal = new UDTSQL_smalldatetime();
            this.Dias = new UDTSQL_int();
            this.CuotaID = new UDT_CuotaID();
            this.DocPagoUltimaCuota = new UDT_Consecutivo();
            this.Observacion = new UDT_DescripTExt();
            this.PorInteres = new UDT_PorcentajeID();
            this.VlrCuota = new UDT_Valor();
            this.VlrPagado = new UDT_Valor();
            this.VlrPendiente = new UDT_Valor();
            this.SaldoCapital = new UDT_Valor();
            this.SaldoInteres = new UDT_Valor();
            this.SaldoGastos = new UDT_Valor();
            this.MvtoCapital = new UDT_Valor();
            this.MvtoInteres = new UDT_Valor();
            this.MvtoGastos = new UDT_Valor();
            this.PeriodoID = new UDT_PeriodoID();
            this.FijadoInd = new UDT_SiNo();
            this.Orden = new UDTSQL_tinyint();
            this.VlrInforme = new UDT_Valor();
            this.VlrJuzgado = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            //Adicionales

            this.Libranza = new UDT_LibranzaID();
            this.Comprobante = new UDTSQL_char(30);
            this.EstadoCarteraCliente = new UDTSQL_tinyint();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int index { get; set; }

        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocMvto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocEstadoCta { get; set; }

        [DataMember]
        public UDTSQL_tinyint ClaseDeuda { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoMvto { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaMvto { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicial { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinal { get; set; }

        [DataMember]
        public UDTSQL_int Dias { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorInteres { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocPagoUltimaCuota { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrPagado { get; set; }

        [DataMember]
        public UDT_Valor VlrPendiente { get; set; }

        [DataMember]
        public UDT_Valor SaldoCapital { get; set; }

        [DataMember]
        public UDT_Valor SaldoInteres { get; set; }

        [DataMember]
        public UDT_Valor SaldoGastos { get; set; }

        [DataMember]
        public UDT_Valor MvtoCapital { get; set; }

        [DataMember]
        public UDT_Valor MvtoInteres { get; set; }

        [DataMember]
        public UDT_Valor MvtoGastos { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_SiNo FijadoInd { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint Orden { get; set; }

        [DataMember]
        public UDT_Valor VlrInforme { get; set; }

        [DataMember]
        public UDT_Valor VlrJuzgado { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        // Adicionales
        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCarteraCliente { get; set; }

        #endregion
    }
}
