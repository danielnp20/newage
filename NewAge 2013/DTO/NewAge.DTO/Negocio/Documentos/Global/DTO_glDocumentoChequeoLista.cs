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
    /// Models DTO_glDocumentoChequeoLista
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocumentoChequeoLista
    {
        #region DTO_glDocumentoChequeoLista

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumentoChequeoLista(IDataReader dr)
        {
            InitCols();
            try
            {

                this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.IncluidoInd.Value = Convert.ToBoolean(dr["IncluidoInd"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
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
        public DTO_glDocumentoChequeoLista()
        {
            InitCols();
        }

        public void InitCols()
        {
            //Inicializa las columnas
            this.NumeroDoc = new UDT_Consecutivo();
            this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.Descripcion = new UDTSQL_char(200);
            this.IncluidoInd = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_char Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo IncluidoInd { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

    }
}
