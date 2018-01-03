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
    /// Models DTO_ccCarteraMvto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCarteraMvto
    {
        #region DTO_ccCarteraMvto

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCarteraMvto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumCredito.Value = Convert.ToInt32(dr["NumCredito"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.Tasa.Value = Convert.ToDecimal(dr["Tasa"]);
                this.VlrComponente.Value = Convert.ToDecimal(dr["VlrComponente"]);
                this.VlrAbono.Value = Convert.ToDecimal(dr["VlrAbono"]);
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
        public DTO_ccCarteraMvto()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumCredito = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.Tasa = new UDT_PorcentajeCarteraID();
            this.VlrComponente = new UDT_Valor();
            this.VlrAbono = new UDT_Valor();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID Tasa { get; set; }

        [DataMember]
        public UDT_Valor VlrComponente { get; set; }

        [DataMember]
        public UDT_Valor VlrAbono { get; set; }

    }
}
