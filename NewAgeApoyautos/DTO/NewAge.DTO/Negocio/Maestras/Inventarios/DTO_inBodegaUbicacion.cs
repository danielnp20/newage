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
    /// Models DTO_inBodegaUbicacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inBodegaUbicacion : DTO_MasterComplex
    {
        #region DTO_inBodegaUbicacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inBodegaUbicacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.BodegaDesc.Value = dr["BodegaDesc"].ToString();
                }

                this.BodegaID.Value = dr["BodegaID"].ToString();
                this.UbicacionID.Value = dr["UbicacionID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inBodegaUbicacion() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.BodegaID = new UDT_BasicID();
            this.BodegaDesc = new UDT_Descriptivo();
            this.UbicacionID = new UDT_Ubicacion();
            this.Descriptivo = new UDT_DescripTBase();    
        }

        public DTO_inBodegaUbicacion(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inBodegaUbicacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID BodegaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaDesc { get; set; }
      
        [DataMember]
        public UDT_Ubicacion UbicacionID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }


    }
}
