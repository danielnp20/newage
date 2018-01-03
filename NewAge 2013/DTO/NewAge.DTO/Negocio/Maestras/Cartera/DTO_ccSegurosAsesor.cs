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
    /// Models DTO_caAseguradora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSegurosAsesor : DTO_MasterBasic
    {
        #region ccSegurosAsesor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSegurosAsesor(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.AsesorInternoInd.Value = Convert.ToBoolean(dr["AsesorInternoInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccSegurosAsesor()
            : base()
        {
            InitCols();
        }

        public DTO_ccSegurosAsesor(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccSegurosAsesor(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AsesorInternoInd = new UDT_SiNo();
        }

        #endregion

        [DataMember]
        public UDT_SiNo AsesorInternoInd { get; set; }

    }

}
