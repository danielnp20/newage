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
    /// Models DTO_drProbabilidadMora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drProbabilidadMora : DTO_MasterBasic
    {
        #region drProbabilidadMora
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drProbabilidadMora(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.FactorCliente.Value = Convert.ToDecimal(dr["FactorCliente"]);
                this.FactorConyugue.Value = Convert.ToDecimal(dr["FactorConyugue"]);
                this.FactorCodeudor.Value = Convert.ToDecimal(dr["FactorCodeudor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drProbabilidadMora()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.FactorCliente = new UDT_PorcentajeID();
            this.FactorConyugue = new UDT_PorcentajeID();
            this.FactorCodeudor = new UDT_PorcentajeID();

        }

        public DTO_drProbabilidadMora(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drProbabilidadMora(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_PorcentajeID FactorCliente { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorConyugue { get; set; }

        [DataMember]
        public UDT_PorcentajeID FactorCodeudor { get; set; }






    }


}