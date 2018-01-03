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
    /// Models DTO_glGarantia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glGarantia : DTO_MasterBasic
    {
        #region glGarantia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glGarantia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.RelacionTipo.Value = Convert.ToByte(dr["RelacionTipo"]);
                this.GarantiaTipo.Value = Convert.ToByte(dr["GarantiaTipo"]);               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_glGarantia()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.RelacionTipo = new UDTSQL_tinyint();
            this.GarantiaTipo = new UDTSQL_tinyint();          
        }

        public DTO_glGarantia(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glGarantia(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint RelacionTipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint GarantiaTipo { get; set; }

    }

}
