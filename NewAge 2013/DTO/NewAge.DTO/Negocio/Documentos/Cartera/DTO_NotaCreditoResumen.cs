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
    /// Models DTO_NotaCreditoResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NotaCreditoResumen
    {
        #region DTO_NotaCreditoResumen

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_NotaCreditoResumen(IDataReader dr)
        {
            InitCols();
            try
            {
                //this.NumDocCredito.Value = Convert.ToInt32(dr["NumCredito"]);
                //this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                //this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                //this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                //this.Tasa.Value = Convert.ToDecimal(dr["Tasa"]);
                //this.VlrComponente.Value = Convert.ToDecimal(dr["VlrComponente"]);
                //this.VlrAbono.Value = Convert.ToDecimal(dr["VlrAbono"]);
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
        public DTO_NotaCreditoResumen()
        {
            InitCols();
            this.ComponenteAdicionalInd.Value = false;
        }

        public void InitCols()
        {
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.Descripcion = new UDT_Descriptivo();
            this.VlrSaldoActual = new UDT_Valor();
            this.VlrNotaCredito = new UDT_Valor();
            this.VlrNuevoSaldo = new UDT_Valor();
            this.ComponenteAdicionalInd = new UDT_SiNo();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descripcion { get; set; }
        
        [DataMember]
        public UDT_Valor VlrSaldoActual { get; set; }

        [DataMember]
        public UDT_Valor VlrNotaCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrNuevoSaldo { get; set; }

        [DataMember]
        public UDT_SiNo ComponenteAdicionalInd { get; set; }

    }
}
