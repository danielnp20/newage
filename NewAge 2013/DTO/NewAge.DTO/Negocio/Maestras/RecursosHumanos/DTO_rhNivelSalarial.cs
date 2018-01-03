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
    /// Models DTO_rhNivelSalarial
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_rhNivelSalarial : DTO_MasterBasic
    {
        #region DTO_rhNivelSalarial
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_rhNivelSalarial(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.ValorMinimo.Value = Convert.ToDecimal(dr["ValorMinimo"]);
                this.ValorMaximo.Value = Convert.ToDecimal(dr["ValorMaximo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_rhNivelSalarial()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ValorMinimo = new UDT_Valor();
            this.ValorMaximo = new UDT_Valor();
        }

        public DTO_rhNivelSalarial(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_rhNivelSalarial(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_Valor ValorMinimo { get; set; }

        [DataMember]
        public UDT_Valor ValorMaximo { get; set; }

    }
}
