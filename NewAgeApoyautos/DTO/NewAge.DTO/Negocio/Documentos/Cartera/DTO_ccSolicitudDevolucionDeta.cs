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
    /// Models DTO_ccSolicitudDevolucionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDevolucionDeta
    {
        #region DTO_ccSolicitudDevolucionDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDevolucionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumeroDEV.Value = Convert.ToByte(dr["NumeroDEV"]);
                this.DevCausalID.Value = dr["DevCausalID"].ToString();   
                this.Observaciones.Value = dr["Observaciones"].ToString();
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
        public DTO_ccSolicitudDevolucionDeta()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();           
            this.NumeroDEV = new UDTSQL_tinyint();
            this.DevCausalID = new UDTSQL_char(5);
            this.Observaciones = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
            //Adicionales
            this.DevCausalDesc = new UDT_Descriptivo();
            this.DevCausalGrupoID = new UDTSQL_char(5);
            this.DevCausalGrupoDesc = new UDT_Descriptivo();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroDEV { get; set; }

        [DataMember]
        public UDTSQL_char DevCausalID { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Adicionales        

        [DataMember]
        public UDT_Descriptivo DevCausalDesc { get; set; }

        [DataMember]
        public UDTSQL_char DevCausalGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DevCausalGrupoDesc { get; set; }
    }
}
