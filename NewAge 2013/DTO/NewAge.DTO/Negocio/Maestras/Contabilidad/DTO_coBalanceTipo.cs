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
    /// Models DTO_coBalanceTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coBalanceTipo : DTO_MasterBasic 
    {
        #region DTO_coBalanceTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coBalanceTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.seGrupoDesc.Value = dr["seGrupoDesc"].ToString();

                this.CuentaAlternaInd.Value = Convert.ToBoolean(dr["CuentaAlternaInd"]);
                this.seGrupoID.Value = dr["seGrupoID"].ToString();
             }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coBalanceTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaAlternaInd = new UDT_SiNo();
            this.seGrupoID = new UDT_BasicID();
            this.seGrupoDesc = new UDT_Descriptivo();
        }

        public DTO_coBalanceTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coBalanceTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_SiNo CuentaAlternaInd { get; set; }

        [DataMember]
        public UDT_BasicID seGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo seGrupoDesc { get; set; }

    }

}
