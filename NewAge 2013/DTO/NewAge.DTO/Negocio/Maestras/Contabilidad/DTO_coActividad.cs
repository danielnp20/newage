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
    /// Models DTO_coActividad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coActividad : DTO_MasterBasic
    {
        #region DTO_coActividad
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coActividad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.ProyectoTipo.Value = Convert.ToByte(dr["ProyectoTipo"]);
                this.ModuloProyectosInd.Value = Convert.ToBoolean(dr["ModuloProyectosInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coActividad()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProyectoTipo = new UDTSQL_tinyint();
            this.ModuloProyectosInd = new UDT_SiNo();
        }

        public DTO_coActividad(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coActividad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_tinyint ProyectoTipo { get; set; }

        [DataMember]
        public UDT_SiNo ModuloProyectosInd { get; set; }

        #endregion

    }

}
