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
    /// Models DTO_coActividadTercero
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coActividadTercero : DTO_MasterComplex
    {
        #region DTO_coActividadTercero
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coActividadTercero(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.ActEconomicaDesc.Value = dr["ActEconomicaDesc"].ToString();
                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ActEconomicaID.Value = dr["ActEconomicaID"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coActividadTercero() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.ActEconomicaID = new UDT_BasicID();
            this.ActEconomicaDesc = new UDT_Descriptivo();
        }

        public DTO_coActividadTercero(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coActividadTercero(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActEconomicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActEconomicaDesc { get; set; }
    }

}
