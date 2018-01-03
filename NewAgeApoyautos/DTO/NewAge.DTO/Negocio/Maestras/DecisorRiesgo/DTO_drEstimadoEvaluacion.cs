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
    /// Models DTO_drEstimadoEvaluacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drEstimadoEvaluacion : DTO_MasterBasic
    {
        #region drEstimadoEvaluacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drEstimadoEvaluacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
                this.Tasa.Value = Convert.ToDecimal(dr["Tasa"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drEstimadoEvaluacion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.Factor = new UDT_PorcentajeID();
            this.Tasa = new UDT_PorcentajeID();
        }

        public DTO_drEstimadoEvaluacion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drEstimadoEvaluacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion
        
        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }

        [DataMember]
        public UDT_PorcentajeID Tasa{ get; set; }

    }

}
