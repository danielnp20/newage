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
    /// Models DTO_pyTrabajoCapitulo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTrabajoCapitulo : DTO_MasterBasic
    {
        #region DTO_pyTrabajoCapitulo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTrabajoCapitulo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if(!isReplica)
                    this.CapituloGrupoDesc.Value = dr["CapituloGrupoDesc"].ToString();
                    this.CapituloGrupoID.Value = dr["CapituloGrupoID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_pyTrabajoCapitulo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CapituloGrupoID = new UDT_BasicID();
            this.CapituloGrupoDesc = new UDT_Descriptivo();
        }

        public DTO_pyTrabajoCapitulo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTrabajoCapitulo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID CapituloGrupoID { get; set; }
        [DataMember]
        public UDT_Descriptivo CapituloGrupoDesc { get; set; }

    }

}
