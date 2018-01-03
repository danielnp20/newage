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
    /// Models DTO_noConvencion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noConvencion : DTO_MasterBasic
    {
        #region DTO_noConvencion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noConvencion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConcepSindicalizadoDesc.Value = dr["ConcepSindicalizadoDesc"].ToString();
                    this.ConceptoNOSindicalizadoDesc.Value = dr["ConceptoNOSindicalizadoDesc"].ToString();
                }
                this.ConcepSindicalizado.Value = dr["ConcepSindicalizado"].ToString();
                this.ConcepNOSindicalizado.Value = dr["ConcepNOSindicalizado"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noConvencion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConcepSindicalizado = new UDT_BasicID();
            this.ConcepSindicalizadoDesc = new UDT_Descriptivo();
            this.ConcepNOSindicalizado = new UDT_BasicID();
            this.ConceptoNOSindicalizadoDesc = new UDT_Descriptivo();
        }

        public DTO_noConvencion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noConvencion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ConcepSindicalizado { get; set; }

        [DataMember]
        public UDT_Descriptivo ConcepSindicalizadoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConcepNOSindicalizado { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNOSindicalizadoDesc { get; set; }
    }
}

