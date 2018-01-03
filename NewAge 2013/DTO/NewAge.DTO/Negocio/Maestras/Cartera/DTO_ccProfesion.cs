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
    /// Models DTO_caProfesion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccProfesion : DTO_MasterBasic
    {
        #region ccProfesion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccProfesion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoNominaPolicia.Value = dr["TipoNominaPolicia"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccProfesion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoNominaPolicia = new UDTSQL_char(10);

        }

        public DTO_ccProfesion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccProfesion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_char TipoNominaPolicia { get; set; }

    }

}
