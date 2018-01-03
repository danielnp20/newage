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
    /// Models DTO_ccNominaINC
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccNominaINC : DTO_MasterBasic
    {
        #region DTO_ccNominaINC
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccNominaINC(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                this.InconsisitenciaNovedadInd.Value = Convert.ToBoolean(dr["InconsisitenciaNovedadInd"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccNominaINC() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.InconsisitenciaNovedadInd = new UDT_SiNo();
        }

        public DTO_ccNominaINC(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccNominaINC(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_SiNo InconsisitenciaNovedadInd { get; set; }

    }
}
