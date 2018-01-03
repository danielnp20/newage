using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_faAsesor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faZona : DTO_MasterBasic
    {
        #region DTO_faZona
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faZona(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.AsesorDesc.Value = dr["AsesorDesc"].ToString();
                }
                
                this.AsesorID.Value = dr["AsesorID"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faZona() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AsesorID = new UDT_BasicID();
            this.AsesorDesc = new UDT_Descriptivo();
        }

        public DTO_faZona(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faZona(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID AsesorID { get; set; }

        [DataMember]
        public UDT_Descriptivo AsesorDesc { get; set; }
    }
}



