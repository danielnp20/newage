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
    /// Models DTO_plGrupoPresupuestalUsuario
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plGrupoPresupuestalUsuario : DTO_MasterComplex
    {
        #region DTO_plGrupoPresupuestalUsuario
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plGrupoPresupuestalUsuario(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.GrupoPresDesc.Value = dr["GrupoPresDesc"].ToString();
                    this.AreaPresupuestalDesc.Value = dr["AreaPresupuestalDesc"].ToString();
                    this.seUsuarioDesc.Value = dr["seUsuarioDesc"].ToString();
                }

                this.GrupoPresupuestoID.Value = dr["GrupoPresupuestoID"].ToString();
                this.AreaPresupuestalID.Value = dr["AreaPresupuestalID"].ToString();
                this.seUsuarioID.Value = dr["seUsuarioID"].ToString();
            }
            catch(Exception e)
            {
               throw e ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plGrupoPresupuestalUsuario() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.GrupoPresupuestoID = new UDT_BasicID();
            this.GrupoPresDesc = new UDT_Descriptivo();
            this.AreaPresupuestalID = new UDT_BasicID();
            this.AreaPresupuestalDesc = new UDT_Descriptivo();
            this.seUsuarioID = new UDT_BasicID();
            this.seUsuarioDesc = new UDT_Descriptivo();       
        }

        public DTO_plGrupoPresupuestalUsuario(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_plGrupoPresupuestalUsuario(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID GrupoPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoPresDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaPresupuestalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaPresupuestalDesc { get; set; }

        [DataMember]
        public UDT_BasicID seUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo seUsuarioDesc { get; set; }
    }
}
