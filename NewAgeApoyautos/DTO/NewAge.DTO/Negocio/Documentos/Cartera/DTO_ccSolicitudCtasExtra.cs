using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudCtasExtra
    {
        public DTO_ccSolicitudCtasExtra()
        {
            this.InitCols();
        }

        public DTO_ccSolicitudCtasExtra(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CuotaID = new UDT_CuotaID();
            this.VlrCuota = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }
        
        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion

    }
}
