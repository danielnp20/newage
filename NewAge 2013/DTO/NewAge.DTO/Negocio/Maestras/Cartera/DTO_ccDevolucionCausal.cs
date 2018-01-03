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
    /// Models DTO_ccDevolucionCausal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccDevolucionCausal : DTO_MasterBasic
    {
        #region DTO_ccDevolucionCausal
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccDevolucionCausal(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.DevCausalGrupoDesc.Value = dr["DevCausalGrupoDesc"].ToString();
                }

                if (!string.IsNullOrEmpty(dr["DevCausalGrupoID"].ToString()))
                    this.DevCausalGrupoID.Value = dr["DevCausalGrupoID"].ToString(); 
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccDevolucionCausal()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DevCausalGrupoID = new UDT_BasicID();
            this.DevCausalGrupoDesc = new UDT_Descriptivo();
        }

        public DTO_ccDevolucionCausal(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccDevolucionCausal(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID DevCausalGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DevCausalGrupoDesc { get; set; }

    }

}
