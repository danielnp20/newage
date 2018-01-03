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
    /// Models DTO_pyCapituloCliente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyCapituloCliente : DTO_MasterComplex
    {
        #region DTO_pyCapituloCliente
        /// <summary>
        /// Construye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyCapituloCliente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.CapituloTrabajoID.Value = dr["CapituloTrabajoID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.CapituloGrupoID.Value = dr["CapituloGrupoID"].ToString();
                if (!isReplica) // Solo Descriptivo, para validar campos que no existen en la base de datos
                {
                    this.ClienteDesc.Value = dr["ClienteDesc"].ToString();
                    this.CapituloGrupoDesc.Value = dr["CapituloGrupoDesc"].ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Inicializa Variables
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClienteID = new UDT_BasicID();
            this.ClienteDesc = new UDT_Descriptivo();
            this.CapituloTrabajoID = new UDT_CodigoGrl();
            this.Descriptivo = new UDT_DescripTBase();
            this.CapituloGrupoID = new UDT_BasicID();
            this.CapituloGrupoDesc = new UDT_Descriptivo();

        } 
        #endregion

        #region Constructor
        public DTO_pyCapituloCliente(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_pyCapituloCliente()
            : base()
        {
            InitCols();
        }

        public DTO_pyCapituloCliente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        } 
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_BasicID ClienteID { get; set; }
        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }
        [DataMember]
        public UDT_CodigoGrl CapituloTrabajoID { get; set; }
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }
        [DataMember]
        public UDT_BasicID CapituloGrupoID { get; set; }
        [DataMember]
        public UDT_Descriptivo CapituloGrupoDesc { get; set; }
        #endregion
        #endregion
    }

}
