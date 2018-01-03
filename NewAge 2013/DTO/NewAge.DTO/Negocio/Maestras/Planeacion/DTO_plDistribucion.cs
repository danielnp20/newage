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
    /// Models DTO_plDistribucion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plDistribucion : DTO_MasterComplex
    {
        #region DTO_plDistribucion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plDistribucion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ServicioDesc.Value = dr["ServicioDesc"].ToString();
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                }

                this.ServicioID.Value = dr["ServicioID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.Porcentaje.Value = Convert.ToDecimal(dr["Porcentaje"]);
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plDistribucion() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ServicioID = new UDT_BasicID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.Porcentaje = new UDT_PorcentajeID();
        }

        public DTO_plDistribucion(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_plDistribucion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
      
        [DataMember]
        public UDT_BasicID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje { get; set; }
    }
}
