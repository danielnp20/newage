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
    /// Models DTO_acTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acTipo : DTO_MasterBasic
    {
        #region DTO_acTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoAct.Value = Convert.ToByte(dr["TipoAct"]);
                this.ContenedorInd.Value = Convert.ToBoolean(dr["ContenedorInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acTipo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoAct = new UDTSQL_tinyint();
            this.ContenedorInd = new UDT_SiNo();
        }

        public DTO_acTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoAct { get; set; }

        [DataMember]
        public UDT_SiNo ContenedorInd { get; set; }
    }
}
