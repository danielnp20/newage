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
    /// Models DTO_ccSolicitudDevolución
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDevolución
    {
        #region DTO_ccSolicitudDevolución

         /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDevolución(IDataReader dr, bool isReplica)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {

                }
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["DevCausalID"].ToString()))
                    this.DevCausalID.Value = Convert.ToString(dr["DevCausalID"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDEV"].ToString()))
                    this.NumeroDEV.Value = Convert.ToByte(dr["NumeroDEV"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDEV"].ToString()))
                    this.FechaDEV.Value = Convert.ToDateTime(dr["FechaDEV"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrWhiteSpace(dr["Consecutivo"].ToString()))
                    this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
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
        public DTO_ccSolicitudDevolución()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc    = new UDT_Consecutivo();
            this.DevCausalID  = new UDTSQL_char(5);
            this.NumeroDEV = new UDTSQL_tinyint();
            this.FechaDEV = new UDTSQL_smalldatetime();
            this.Observaciones  = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();    
        }
        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc{ get; set; }

        [DataMember]
        public UDTSQL_char DevCausalID { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroDEV { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDEV { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

    }
}
