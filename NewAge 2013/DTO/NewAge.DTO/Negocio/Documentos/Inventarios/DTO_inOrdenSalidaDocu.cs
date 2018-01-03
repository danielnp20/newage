using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inOrdenSalidaDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inOrdenSalidaDocu : DTO_BasicReport
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inOrdenSalidaDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value = (dr["EmpresaID"]).ToString();
                if (!string.IsNullOrEmpty(dr["DocSalidaINV"].ToString()))
                    this.DocSalidaINV.Value = Convert.ToInt32(dr["DocSalidaINV"]);
                if (!string.IsNullOrEmpty(dr["BodegaID"].ToString()))
                    this.BodegaID.Value = (dr["BodegaID"]).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
                /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inOrdenSalidaDocu()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.DocSalidaINV = new UDT_Consecutivo();
            this.BodegaID = new UDT_BodegaID();

        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocSalidaINV { get; set; }
        
        [DataMember]        
        public UDT_BodegaID BodegaID { get; set; }

        #endregion
    }

}
