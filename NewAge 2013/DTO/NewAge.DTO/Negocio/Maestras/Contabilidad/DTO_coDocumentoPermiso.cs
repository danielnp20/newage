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
    /// Models DTO_coDocumentoPermiso
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coDocumentoPermiso : DTO_MasterComplex
    {
        #region DTO_coDocumentoPermiso
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coDocumentoPermiso(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString();
                    this.AlarmaUsuario1Desc.Value = dr["AlarmaUsuario1Desc"].ToString();
                    this.AlarmaUsuario2Desc.Value = dr["AlarmaUsuario2Desc"].ToString();
                    this.AlarmaUsuario3Desc.Value = dr["AlarmaUsuario3Desc"].ToString();
                }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
                this.AlarmaUsuario1.Value = dr["AlarmaUsuario1"].ToString();
                this.AlarmaUsuario2.Value = dr["AlarmaUsuario2"].ToString();
                this.AlarmaUsuario3.Value = dr["AlarmaUsuario3"].ToString();
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coDocumentoPermiso() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.UsuarioID = new UDT_BasicID();
            this.UsuarioDesc = new UDT_Descriptivo();
            this.AlarmaUsuario1 = new UDT_BasicID();
            this.AlarmaUsuario1Desc = new UDT_Descriptivo();
            this.AlarmaUsuario2 = new UDT_BasicID();
            this.AlarmaUsuario2Desc = new UDT_Descriptivo();
            this.AlarmaUsuario3 = new UDT_BasicID();
            this.AlarmaUsuario3Desc = new UDT_Descriptivo();
        }

        public DTO_coDocumentoPermiso(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_coDocumentoPermiso(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_BasicID UsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDT_BasicID AlarmaUsuario1 { get; set; }

        [DataMember]
        public UDT_Descriptivo AlarmaUsuario1Desc { get; set; }

        [DataMember]
        public UDT_BasicID AlarmaUsuario2 { get; set; }

        [DataMember]
        public UDT_Descriptivo AlarmaUsuario2Desc { get; set; }

        [DataMember]
        public UDT_BasicID AlarmaUsuario3 { get; set; }

        [DataMember]
        public UDT_Descriptivo AlarmaUsuario3Desc { get; set; }
    }
}
