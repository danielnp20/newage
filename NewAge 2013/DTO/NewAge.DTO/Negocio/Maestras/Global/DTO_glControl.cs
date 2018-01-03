using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_aplModulo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glControl
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glControl(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.glControlID.Value = Convert.ToInt32(dr["glControlID"]);
                this.Data.Value = Convert.ToString(dr["Data"]);
                this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
                this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
                this.ReplicaID.Value = Convert.ToInt32(dr["ReplicaID"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glControl()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.glControlID = new UDTSQL_int();
            this.Data = new UDTSQL_varchar(255);
            this.Descriptivo = new UDT_DescripTExt();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();
        }
        #endregion

        /// <summary>
        /// Gets or sets the glControlID
        /// </summary>
        [DataMember]
        public UDTSQL_int glControlID { get; set; }

        /// <summary>
        /// Gets or sets the Descriptivo
        /// </summary>
        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        [DataMember]
        public UDTSQL_varchar Data { get; set; }

        /// <summary>
        /// Gets or sets the CtrlVersion
        /// </summary>
        [DataMember]
        public UDT_CtrlVersion CtrlVersion { get; set; }

        /// <summary>
        /// Gets or sets the ReplicaID
        /// </summary>
        [DataMember]
        public UDT_ReplicaID ReplicaID { get; set; }

    }
}
