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
    /// Models DTO_glUsuarioxGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glUsuarioxGrupo : DTO_MasterComplex
    {
        #region DTO_glUsuarioxGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glUsuarioxGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.GrupoUsuarioDesc.Value = dr["GrupoUsuarioDesc"].ToString();
                    this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();
                }

                this.GrupoUsuarioID.Value = dr["GrupoUsuarioID"].ToString();
                this.seUsuarioID.Value = dr["seUsuarioID"].ToString();
            }
            catch (Exception e)
            {
               throw e;
            }         
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glUsuarioxGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.GrupoUsuarioID = new UDT_BasicID();
            this.GrupoUsuarioDesc = new UDT_Descriptivo();
            this.seUsuarioID = new UDT_BasicID();
            this.UsuarioDesc = new UDT_Descriptivo();
        }

        public DTO_glUsuarioxGrupo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glUsuarioxGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID GrupoUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoUsuarioDesc { get; set; }

        [DataMember]
        public UDT_BasicID seUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

    }

}
