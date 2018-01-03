using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class recibidos de bienes y servicios:
    /// Models DTO_plPlaneacion_Proveedores
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plPlaneacion_Proveedores
    {
        #region DTO_plPlaneacion_Proveedores

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plPlaneacion_Proveedores(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ConsActLinea.Value = Convert.ToInt32(dr["ConsActLinea"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plPlaneacion_Proveedores()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.ConsActLinea = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Consecutivo = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsActLinea{ get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Valor Consecutivo { get; set; }

        #endregion
    }
}

