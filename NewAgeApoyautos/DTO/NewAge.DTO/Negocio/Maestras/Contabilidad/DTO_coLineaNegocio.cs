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
    /// Models DTO_coLineaNegocio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coLineaNegocio : DTO_MasterBasic
    {
        #region DTO_coLineaNegocio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coLineaNegocio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.UnidGenEfectID.Value = dr["UnidGenEfectID"].ToString();
                if (!isReplica)
                {
                    this.UnidadGenEfectivoDesc.Value = dr["UnidadGenEfectivoDesc"].ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_coLineaNegocio()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.UnidGenEfectID = new UDT_BasicID();
            this.UnidadGenEfectivoDesc = new UDT_Descriptivo();
        }

        public DTO_coLineaNegocio(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coLineaNegocio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID UnidGenEfectID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadGenEfectivoDesc { get; set; }

    }

}
