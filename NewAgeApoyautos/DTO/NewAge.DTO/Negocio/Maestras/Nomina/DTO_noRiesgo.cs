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
    /// Models DTO_noRiesgo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noRiesgo : DTO_MasterBasic
    {
        #region DTO_noRiesgo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noRiesgo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noRiesgo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PorcentajeID = new UDT_PorcentajeID();
        }

        public DTO_noRiesgo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noRiesgo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_PorcentajeID PorcentajeID { get; set; }          
    }
}


