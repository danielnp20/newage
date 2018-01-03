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
    /// Models DTO_ccTareaChequeoLista
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccTareaChequeoLista
    {
        #region DTO_ccTareaChequeoLista

         /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccTareaChequeoLista(IDataReader dr, bool isReplica)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IncluidoInd"].ToString()))
                    this.IncluidoInd.Value = Convert.ToBoolean(dr["IncluidoInd"]);

                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TareaID.Value = dr["TareaID"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                //this.Descriptivo.Value = dr["Descriptivo"].ToString();
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
        public DTO_ccTareaChequeoLista()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc    = new UDT_Consecutivo();
            this.TareaID  = new UDT_BasicID();
            this.Descripcion = new UDT_DescripTExt();
            this.IncluidoInd = new UDT_SiNo();
            this.Consecutivo  = new UDT_Consecutivo();
            this.Descriptivo = new UDT_Descriptivo();    
        }
        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_BasicID TareaID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoInd { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

    }
}
