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
    /// Models DTO_ccPagaduriaSector
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccPagaduriaSector : DTO_MasterBasic
    {
        #region DTO_ccPagaduriaSector
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPagaduriaSector(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    
                }

                if (!string.IsNullOrEmpty(dr["TipoSector"].ToString()))
                    this.TipoSector.Value = Convert.ToByte(dr["TipoSector"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPagaduriaSector()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.TipoSector = new UDTSQL_tinyint();

        }

        public DTO_ccPagaduriaSector(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccPagaduriaSector(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoSector { get; set; }
    }
}
