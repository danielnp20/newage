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
    /// Models DTO_glEmpresaGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glEmpresaGrupo : DTO_MasterBasic
    {
        #region DTO_glEmpresaGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glEmpresaGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glEmpresaGrupo()
            : base()
        {
        }

        public DTO_glEmpresaGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
        }

        public DTO_glEmpresaGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion
    }

}

