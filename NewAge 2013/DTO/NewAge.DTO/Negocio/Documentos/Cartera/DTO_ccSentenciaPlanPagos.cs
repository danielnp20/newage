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
    public class DTO_ccSentenciaPlanPagos
    {
        #region DTO_ccSentenciaPlanPagos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSentenciaPlanPagos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSentenciaPlanPagos()
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

            //Adicionales
            this.Abono = new UDT_Valor();
            this.Saldo = new UDT_Valor();
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
        public UDT_Valor VlrCuota { get; set; }

        //Adicionales
        [DataMember]
        public UDT_Valor Abono { get; set; }

        [DataMember]
        public UDT_Valor Saldo { get; set; }
        #endregion
    }
}
