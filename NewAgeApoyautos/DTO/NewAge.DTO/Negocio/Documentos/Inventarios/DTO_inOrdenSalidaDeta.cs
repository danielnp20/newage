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
    /// Models DTO_inOrdenSalidaDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inOrdenSalidaDeta : DTO_BasicReport
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inOrdenSalidaDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["ConsProyectoMvto"].ToString()))
                    this.ConsProyectoMvto.Value = Convert.ToInt32(dr["ConsProyectoMvto"]);
                if (!string.IsNullOrEmpty(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                this.CantidadAPR.Value = Convert.ToDecimal(dr["CantidadAPR"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrEmpty(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
                /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inOrdenSalidaDeta()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsProyectoMvto = new UDT_Consecutivo();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.CantidadAPR = new UDT_Cantidad();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ConsProyectoMvto { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_inReferenciaID inReferenciaID { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadAPR { get; set; }
                
        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }
   
        #endregion
    }

}
