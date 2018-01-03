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
    /// Models DTO_inBodegaTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inBodegaTipo : DTO_MasterBasic
    {
        #region DTO_inBodegaTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inBodegaTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.BodegaTipo.Value = Convert.ToByte(dr["BodegaTipo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inBodegaTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BodegaTipo = new UDTSQL_tinyint();
        }

        public DTO_inBodegaTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inBodegaTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint BodegaTipo  { get; set; }

    }

}
