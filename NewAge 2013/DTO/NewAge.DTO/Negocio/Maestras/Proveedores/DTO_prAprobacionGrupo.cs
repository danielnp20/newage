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
    /// Models DTO_prAprobacionGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prAprobacionGrupo : DTO_MasterBasic
    {
        #region DTO_prAprobacionGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prAprobacionGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                    this.UnidadInvDesc.Value = dr["UnidadInvDesc"].ToString();
                }
                this.EmplResponsableSOL.Value = dr["EmplResponsableSOL"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.TipoControl.Value = Convert.ToByte(dr["TipoControl"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prAprobacionGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmplResponsableSOL = new UDT_BasicID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.TipoControl = new UDTSQL_tinyint();
        }

        public DTO_prAprobacionGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prAprobacionGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID EmplResponsableSOL { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoControl { get; set; }
    }
}
