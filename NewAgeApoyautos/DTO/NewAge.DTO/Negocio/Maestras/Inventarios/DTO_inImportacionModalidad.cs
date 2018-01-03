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
    /// Models DTO_inImportacionModelo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inImportacionModalidad : DTO_MasterBasic
    {
        #region DTO_inImportacionModelo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inImportacionModalidad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.MesesTope.Value = Convert.ToByte(dr["MesesTope"]);
                this.ImpTemporalInd.Value = Convert.ToBoolean(dr["ImpTemporalInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inImportacionModalidad()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.MesesTope = new UDTSQL_tinyint();
            this.ImpTemporalInd = new UDT_SiNo();
        }

        public DTO_inImportacionModalidad(DTO_MasterBasic comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_inImportacionModalidad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint MesesTope { get; set; }

        [DataMember]
        public UDT_SiNo ImpTemporalInd { get; set; }

    }
}
