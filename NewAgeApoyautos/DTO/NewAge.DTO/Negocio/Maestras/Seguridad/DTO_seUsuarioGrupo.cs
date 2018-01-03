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
    /// Models DTO_seUsuarioGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seUsuarioGrupo : DTO_MasterComplex 
    {
        #region DTO_seUsuarioGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seUsuarioGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.EmpresaDesc.Value = dr["EmpresaDesc"].ToString();
                    this.GrupoDesc.Value = dr["GrupoDesc"].ToString();
                    this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();

                    this.seUsuarioID.Value = dr["UsuarioID1"].ToString();
                }
                else
                    this.seUsuarioID.Value = dr["seUsuarioID"].ToString();

                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.seGrupoID.Value = dr["seGrupoID"].ToString();
                this.AutorizacionTotalPresupInd.Value = Convert.ToBoolean(dr["AutorizacionTotalPresupInd"]);
                this.AutorizacionTotalProveedInd.Value = Convert.ToBoolean(dr["AutorizacionTotalProveedInd"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seUsuarioGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_BasicID();
            this.EmpresaDesc = new UDT_Descriptivo();
            this.seGrupoID = new UDT_BasicID();
            this.GrupoDesc = new UDT_Descriptivo();
            this.seUsuarioID = new UDT_BasicID();
            this.UsuarioDesc = new UDT_Descriptivo();
            this.AutorizacionTotalPresupInd = new UDT_SiNo();
            this.AutorizacionTotalProveedInd = new UDT_SiNo();
        }

        public DTO_seUsuarioGrupo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_seUsuarioGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  

        #endregion 
 
        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaDesc { get; set; }

        [DataMember]
        public UDT_BasicID seGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoDesc { get; set; }

        [DataMember]
        public UDT_BasicID seUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDT_SiNo AutorizacionTotalPresupInd { get; set; }

        [DataMember]
        public UDT_SiNo AutorizacionTotalProveedInd { get; set; }
    }

}
