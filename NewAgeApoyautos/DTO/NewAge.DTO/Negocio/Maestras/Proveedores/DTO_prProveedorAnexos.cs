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
    /// Models DTO_prProveedorAnexos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prProveedorAnexos : DTO_MasterComplex
    {
        #region DTO_prProveedorAnexos
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prProveedorAnexos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.AnexoDesc.Value = dr["AnexoDesc"].ToString();
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                }

                this.AnexoID.Value = dr["AnexoID"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString(); 
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prProveedorAnexos() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AnexoID = new UDT_BasicID();
            this.AnexoDesc = new UDT_Descriptivo();
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
        }

        public DTO_prProveedorAnexos(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prProveedorAnexos(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID AnexoID { get; set; }

        [DataMember]
        public UDT_Descriptivo AnexoDesc { get; set; }
        [DataMember]

        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

    }

}
