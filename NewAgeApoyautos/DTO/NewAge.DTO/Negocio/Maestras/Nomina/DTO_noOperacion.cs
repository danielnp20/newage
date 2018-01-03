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
    /// Models DTO_noOperacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noOperacion : DTO_MasterBasic
    {
        #region DTO_noOperacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noOperacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.AdministrativaInd.Value = Convert.ToBoolean(dr["AdministrativaInd"]);
                this.PlanillaDiariaInd.Value = Convert.ToBoolean(dr["PlanillaDiariaInd"]);
                this.AnticipoPrimaInd.Value = Convert.ToBoolean(dr["AnticipoPrimaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noOperacion() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AdministrativaInd = new UDT_SiNo();
            this.PlanillaDiariaInd = new UDT_SiNo();
            this.AnticipoPrimaInd = new UDT_SiNo();
        }

        public DTO_noOperacion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noOperacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_SiNo AdministrativaInd { get; set; }

        [DataMember]
        public UDT_SiNo PlanillaDiariaInd { get; set; }

        [DataMember]
        public UDT_SiNo AnticipoPrimaInd { get; set; }
    }
}

