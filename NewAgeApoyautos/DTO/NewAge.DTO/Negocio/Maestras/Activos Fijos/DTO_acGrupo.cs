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
    /// Models DTO_acGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acGrupo : DTO_MasterBasic
    {
        #region DTO_acGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            
        }

        public DTO_acGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
    }
}
