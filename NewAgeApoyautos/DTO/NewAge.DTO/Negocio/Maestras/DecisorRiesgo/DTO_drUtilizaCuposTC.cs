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
    /// Models DTO_drUtilizaCuposTC
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drUtilizaCuposTC : DTO_MasterBasic
    {
        #region drUtilizaCuposTC
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drUtilizaCuposTC(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.FactorCupo.Value = Convert.ToDecimal(dr["FactorCupo"]);
                this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
                this.ValorCupo.Value = Convert.ToBoolean(dr["ValorCupo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drUtilizaCuposTC()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.FactorCupo = new UDT_PorcentajeID();
            this.Factor = new UDT_PorcentajeID();
            this.ValorCupo = new UDT_SiNo();
        }

        public DTO_drUtilizaCuposTC(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drUtilizaCuposTC(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_PorcentajeID FactorCupo
        { get; set; }

        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }

        [DataMember]
        public UDT_SiNo ValorCupo { get; set; }

    }

}
