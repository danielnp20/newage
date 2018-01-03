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
    /// Models DTO_inConversionUnidad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inConversionUnidad : DTO_MasterComplex
    {
        #region DTO_inConversionUnidad
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inConversionUnidad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.UnidadInvDesc.Value = dr["UnidadInvDesc"].ToString();
                    this.UnidadBaseDesc.Value = dr["UnidadBaseDesc"].ToString();
                }

                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.UnidadBase.Value = dr["UnidadBase"].ToString();
                this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inConversionUnidad() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.UnidadBase = new UDT_BasicID();
            this.UnidadBaseDesc = new UDT_Descriptivo();
            this.Factor = new UDTSQL_numeric();   
        }

        public DTO_inConversionUnidad(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inConversionUnidad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }
      
        [DataMember]
        public UDT_BasicID UnidadBase { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadBaseDesc { get; set; }

        [DataMember]
        public UDTSQL_numeric Factor { get; set; }
    }
}
