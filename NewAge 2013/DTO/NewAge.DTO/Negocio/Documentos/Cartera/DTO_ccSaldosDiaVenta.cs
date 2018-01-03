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
    /// Models DTO_ccSaldosDiaVenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSaldosDiaVenta
    {
        #region DTO_ccSaldosDiaVenta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSaldosDiaVenta(IDataReader dr)
        {
            InitCols();
            try
            {
                #region Datos Comunes
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["NumCredito"].ToString()))
                    this.NumCredito.Value = Convert.ToInt32(dr["NumCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["CompradorCarteraID"].ToString()))
                    this.CompradorCarteraID.Value = Convert.ToString(dr["CompradorCarteraID"]);
                if (!string.IsNullOrWhiteSpace(dr["VendedorID"].ToString()))
                    this.VendedorID.Value = Convert.ToString(dr["VendedorID"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasCRD"].ToString()))
                    this.CuotasCRD.Value = Convert.ToInt32(dr["CuotasCRD"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorCRD"].ToString()))
                    this.ValorCRD.Value = Convert.ToDecimal(dr["ValorCRD"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoCRD"].ToString()))
                    this.SaldoCRD.Value = Convert.ToDecimal(dr["SaldoCRD"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasMoraCRD"].ToString()))
                    this.CuotasMoraCRD.Value = Convert.ToInt32(dr["CuotasMoraCRD"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoMoraCRD"].ToString()))
                    this.SaldoMoraCRD.Value = Convert.ToDecimal(dr["SaldoMoraCRD"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasCOM"].ToString()))
                    this.CuotasCOM.Value = Convert.ToInt32(dr["CuotasCOM"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorCOM"].ToString()))
                    this.ValorCOM.Value = Convert.ToDecimal(dr["ValorCOM"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaCOM"].ToString()))
                    this.TasaCOM.Value = Convert.ToDecimal(dr["TasaCOM"]);
                if (!string.IsNullOrWhiteSpace(dr["VpnCOM"].ToString()))
                    this.VpnCOM.Value = Convert.ToDecimal(dr["VpnCOM"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoCOM"].ToString()))
                    this.SaldoCOM.Value = Convert.ToDecimal(dr["SaldoCOM"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasVEN"].ToString()))
                    this.CuotasVEN.Value = Convert.ToInt32(dr["CuotasVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorVEN"].ToString()))
                    this.ValorVEN.Value = Convert.ToDecimal(dr["ValorVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaVEN"].ToString()))
                    this.TasaVEN.Value = Convert.ToDecimal(dr["TasaVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["VpnVEN"].ToString()))
                    this.VpnVEN.Value = Convert.ToDecimal(dr["VpnVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoVEN"].ToString()))
                    this.SaldoVEN.Value = Convert.ToDecimal(dr["SaldoVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["Consecutivo"].ToString()))
                    this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                #endregion
                #region Datos Comunes
                #endregion
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSaldosDiaVenta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Fecha = new UDTSQL_datetime();
            this.NumCredito = new UDT_Consecutivo();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.VendedorID = new UDT_CompradorCarteraID();
            this.VlrCuota = new UDT_Valor();
            this.CuotasCRD = new UDT_CuotaID();
            this.ValorCRD = new UDT_Valor();
            this.SaldoCRD = new UDT_Valor();
            this.CuotasMoraCRD = new UDT_CuotaID();
            this.SaldoMoraCRD = new UDT_Valor();
            this.CuotasCOM = new UDT_CuotaID();
            this.ValorCOM = new UDT_Valor();
            this.TasaCOM = new UDT_TasaID();
            this.VpnCOM = new UDT_Valor();
            this.SaldoCOM = new UDT_Valor();
            this.CuotasVEN = new UDT_CuotaID();
            this.ValorVEN = new UDT_Valor();
            this.TasaVEN = new UDT_TasaID();
            this.VpnVEN = new UDT_Valor();
            this.SaldoVEN = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_Consecutivo NumCredito { get; set; }
        
        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }
        
        [DataMember]
        public UDT_CompradorCarteraID VendedorID { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCuota { get; set; }
        
        [DataMember]
        public UDT_CuotaID CuotasCRD { get; set; }
        
        [DataMember]
        public UDT_Valor ValorCRD { get; set; }
        
        [DataMember]
        public UDT_Valor SaldoCRD { get; set; }
        
        [DataMember]
        public UDT_CuotaID CuotasMoraCRD { get; set; }
        
        [DataMember]
        public UDT_Valor SaldoMoraCRD { get; set; }
        
        [DataMember]
        public UDT_CuotaID CuotasCOM { get; set; }
        
        [DataMember]
        public UDT_Valor ValorCOM { get; set; }
        
        [DataMember]
        public UDT_TasaID TasaCOM { get; set; }
        
        [DataMember]
        public UDT_Valor VpnCOM { get; set; }
        
        [DataMember]
        public UDT_Valor SaldoCOM { get; set; }
        
        [DataMember]
        public UDT_CuotaID CuotasVEN { get; set; }
        
        [DataMember]
        public UDT_Valor ValorVEN { get; set; }
        
        [DataMember]
        public UDT_TasaID TasaVEN { get; set; }
        
        [DataMember]
        public UDT_Valor VpnVEN { get; set; }
        
        [DataMember]
        public UDT_Valor SaldoVEN { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}
