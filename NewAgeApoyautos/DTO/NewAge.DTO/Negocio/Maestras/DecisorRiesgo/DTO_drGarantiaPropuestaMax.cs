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
    /// Models DTO_drGarantiaPropuestaMax
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drGarantiaPropuestaMax : DTO_MasterBasic
    {
        #region drGarantiaPropuestaMax
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drGarantiaPropuestaMax(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drGarantiaPropuestaMax()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

        }

        public DTO_drGarantiaPropuestaMax(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drGarantiaPropuestaMax(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

    }

}
