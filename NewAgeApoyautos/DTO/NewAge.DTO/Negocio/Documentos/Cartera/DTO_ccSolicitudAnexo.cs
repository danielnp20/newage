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
    /// Models DTO_SolicitudAnexo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudAnexo
    {
        #region DTO_ccSolicitudAnexo

         /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudAnexo(IDataReader dr, bool isReplica)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IncluidoInd"].ToString()))
                    this.IncluidoInd.Value   = Convert.ToBoolean(dr["IncluidoInd"]);
                this.NumeroDoc.Value         = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumListaID.Value      = dr["DocumListaID"].ToString();
                this.Consecutivo.Value       = Convert.ToInt32(dr["Consecutivo"]);
                this.Descriptivo.Value       = dr["Descriptivo"].ToString();
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
        public DTO_ccSolicitudAnexo()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc    = new UDT_Consecutivo();
            this.DocumListaID = new UDT_BasicID();
            this.Descripcion  = new UDT_DescripTExt();
            this.IncluidoInd  = new UDT_SiNo();
            this.Consecutivo  = new UDT_Consecutivo();
            this.Descriptivo = new UDT_Descriptivo();
        }
        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc   { get; set; }

        [DataMember]
        public UDT_BasicID DocumListaID    { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoInd        { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

    }
}
