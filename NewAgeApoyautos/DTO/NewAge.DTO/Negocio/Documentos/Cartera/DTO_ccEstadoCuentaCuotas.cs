using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_ccEstadoCuentaCuotas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccEstadoCuentaCuotas
    {
        #region DTO_ccEstadoCuentaCuotas

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCuentaCuotas(IDataReader dr)
        {
            InitCols();
            try
            {
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"].ToString());
                    this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["CompradorCarteraID"].ToString()))
                        this.CompradorCarteraID.Value = Convert.ToString(dr["CompradorCarteraID"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                        this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrCapitalSDO"].ToString()))
                        this.VlrCapitalSDO.Value = Convert.ToDecimal(dr["VlrCapitalSDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrInteresSDO"].ToString()))
                        this.VlrInteresSDO.Value = Convert.ToDecimal(dr["VlrInteresSDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrSeguroSDO"].ToString()))
                        this.VlrSeguroSDO.Value = Convert.ToDecimal(dr["VlrSeguroSDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro1SDO"].ToString()))
                        this.VlrOtro1SDO.Value = Convert.ToDecimal(dr["VlrOtro1SDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro2SDO"].ToString()))
                        this.VlrOtro2SDO.Value = Convert.ToDecimal(dr["VlrOtro2SDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro3SDO"].ToString()))
                        this.VlrOtro3SDO.Value = Convert.ToDecimal(dr["VlrOtro3SDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtrosfijosSDO"].ToString()))
                        this.VlrOtrosfijosSDO.Value = Convert.ToDecimal(dr["VlrOtrosfijosSDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrMoraSDO"].ToString()))
                        this.VlrMoraSDO.Value = Convert.ToDecimal(dr["VlrMoraSDO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrCapitalABO"].ToString()))
                        this.VlrCapitalABO.Value = Convert.ToDecimal(dr["VlrCapitalABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrInteresABO"].ToString()))
                        this.VlrInteresABO.Value = Convert.ToDecimal(dr["VlrInteresABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrSeguroABO"].ToString()))
                        this.VlrSeguroABO.Value = Convert.ToDecimal(dr["VlrSeguroABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro1ABO"].ToString()))
                        this.VlrOtro1ABO.Value = Convert.ToDecimal(dr["VlrOtro1ABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro2ABO"].ToString()))
                        this.VlrOtro2ABO.Value = Convert.ToDecimal(dr["VlrOtro2ABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro3ABO"].ToString()))
                        this.VlrOtro3ABO.Value = Convert.ToDecimal(dr["VlrOtro3ABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtrosfijosABO"].ToString()))
                        this.VlrOtrosfijosABO.Value = Convert.ToDecimal(dr["VlrOtrosfijosABO"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrCapitalPAG"].ToString()))
                        this.VlrCapitalPAG.Value = Convert.ToDecimal(dr["VlrCapitalPAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrInteresPAG"].ToString()))
                        this.VlrInteresPAG.Value = Convert.ToDecimal(dr["VlrInteresPAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrSeguroPAG"].ToString()))
                        this.VlrSeguroPAG.Value = Convert.ToDecimal(dr["VlrSeguroPAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro1PAG"].ToString()))
                        this.VlrOtro1PAG.Value = Convert.ToDecimal(dr["VlrOtro1PAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro2PAG"].ToString()))
                        this.VlrOtro2PAG.Value = Convert.ToDecimal(dr["VlrOtro2PAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtro3PAG"].ToString()))
                        this.VlrOtro3PAG.Value = Convert.ToDecimal(dr["VlrOtro3PAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrOtrosfijosPAG"].ToString()))
                        this.VlrOtrosfijosPAG.Value = Convert.ToDecimal(dr["VlrOtrosfijosPAG"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrPRJ"].ToString()))
                        this.VlrPRJ.Value = Convert.ToDecimal(dr["VlrPRJ"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrMora"].ToString()))
                        this.VlrMora.Value = Convert.ToDecimal(dr["VlrMora"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["VlrGastos"].ToString()))
                        this.VlrGastos.Value = Convert.ToDecimal(dr["VlrGastos"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["IndCuotaVencida"].ToString()))
                        this.IndCuotaVencida.Value = Convert.ToBoolean(dr["IndCuotaVencida"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCuentaCuotas()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CuotaID = new UDT_CuotaID();
            this.FechaCuota = new UDTSQL_datetime();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.VlrCuota = new UDT_Valor();
            this.VlrCapitalSDO = new UDT_Valor();
            this.VlrInteresSDO = new UDT_Valor();
            this.VlrSeguroSDO = new UDT_Valor();
            this.VlrOtro1SDO = new UDT_Valor();
            this.VlrOtro2SDO = new UDT_Valor();
            this.VlrOtro3SDO = new UDT_Valor();
            this.VlrOtrosfijosSDO = new UDT_Valor();
            this.VlrMoraSDO = new UDT_Valor();
            this.VlrCapitalABO = new UDT_Valor();
            this.VlrInteresABO = new UDT_Valor();
            this.VlrSeguroABO = new UDT_Valor();
            this.VlrOtro1ABO = new UDT_Valor();
            this.VlrOtro2ABO = new UDT_Valor();
            this.VlrOtro3ABO = new UDT_Valor();
            this.VlrOtrosfijosABO = new UDT_Valor();
            this.VlrCapitalPAG = new UDT_Valor();
            this.VlrInteresPAG = new UDT_Valor();
            this.VlrSeguroPAG = new UDT_Valor();
            this.VlrOtro1PAG = new UDT_Valor();
            this.VlrOtro2PAG = new UDT_Valor();
            this.VlrOtro3PAG = new UDT_Valor();
            this.VlrOtrosfijosPAG = new UDT_Valor();
            this.VlrPRJ = new UDT_Valor();
            this.VlrMora = new UDT_Valor();
            this.VlrGastos = new UDT_Valor();
            this.IndCuotaVencida = new UDT_SiNo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }
        
        [DataMember]
        public UDTSQL_datetime FechaCuota { get; set; }
        
        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCuota { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCapitalSDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrInteresSDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrSeguroSDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro1SDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro2SDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro3SDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtrosfijosSDO { get; set; }

        [DataMember]
        public UDT_Valor VlrMoraSDO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCapitalABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrInteresABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrSeguroABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro1ABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro2ABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro3ABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtrosfijosABO { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCapitalPAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrInteresPAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrSeguroPAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro1PAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro2PAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtro3PAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrOtrosfijosPAG { get; set; }
        
        [DataMember]
        public UDT_Valor VlrPRJ { get; set; }

        [DataMember]
        public UDT_Valor VlrMora { get; set; }

        [DataMember]
        public UDT_Valor VlrGastos { get; set; }

        [DataMember]
        public UDT_SiNo IndCuotaVencida { get; set; }

        #endregion
    }
}
