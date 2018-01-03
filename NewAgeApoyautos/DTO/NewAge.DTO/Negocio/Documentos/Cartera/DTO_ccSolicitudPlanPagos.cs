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
    /// Models DTO_ccSolicitduPlanPagos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudPlanPagos
    {
        #region DTO_ccSolicitudPlanPagos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public DTO_ccSolicitudPlanPagos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                this.VlrInteres.Value = Convert.ToDecimal(dr["VlrInteres"]);
                this.VlrSeguro.Value = Convert.ToDecimal(dr["VlrSeguro"]);
                this.VlrOtro1.Value = Convert.ToDecimal(dr["VlrOtro1"]);
                this.VlrOtro2.Value = Convert.ToDecimal(dr["VlrOtro2"]);
                this.VlrOtro3.Value = Convert.ToDecimal(dr["VlrOtro3"]);
                this.VlrOtrosfijos.Value = Convert.ToDecimal(dr["VlrOtrosfijos"]);
                this.VlrSaldoCapital.Value = Convert.ToDecimal(dr["VlrSaldoCapital"]);
                //this.VlrSaldoSeguro.Value = Convert.ToDecimal(dr["VlrSaldoSeguro"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudPlanPagos()
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
            this.VlrCuota = new UDT_Valor();
            this.VlrCapital = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrOtro1 = new UDT_Valor();
            this.VlrOtro2 = new UDT_Valor();
            this.VlrOtro3 = new UDT_Valor();
            this.VlrOtrosfijos = new UDT_Valor();
            this.VlrSaldoCapital = new UDT_Valor();
            this.VlrSaldoSeguro = new UDT_Valor();

        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota { get; set; }

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
        public UDT_Valor VlrOtrosfijos { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoSeguro { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        #endregion
    }
}
