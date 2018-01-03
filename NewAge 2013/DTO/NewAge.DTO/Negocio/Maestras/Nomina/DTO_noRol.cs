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
    /// Models DTO_noRol
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noRol : DTO_MasterBasic
    {
        #region DTO_noRol
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noRol(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.DiasCalendarioInd.Value = Convert.ToBoolean(dr["DiasCalendarioInd"]);
                this.SabadoLaboralInd.Value = Convert.ToBoolean(dr["SabadoLaboralInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noRol() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DiasCalendarioInd = new UDT_SiNo();
            this.SabadoLaboralInd = new UDT_SiNo();
        }

        public DTO_noRol(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noRol(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_SiNo DiasCalendarioInd { get; set; }

        [DataMember]
        public UDT_SiNo SabadoLaboralInd{ get; set; }

    }
}

