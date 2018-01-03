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
    /// Models DTO_ccDocPagaduria
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccPagaduriaAnexos : DTO_MasterComplex
    {
        #region DTO_ccDocPagaduria
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPagaduriaAnexos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica) this.DocumListaDesc.Value = dr["DocumListaDesc"].ToString();
                if (!isReplica) this.PagaduriaDesc.Value  = dr["PagaduriaDesc"].ToString();
                this.PagaduriaID.Value                    = dr["PagaduriaID"].ToString();
                this.DocumListaID.Value                   = dr["DocumListaID"].ToString();
                this.Descriptivo.Value                    = dr["Descriptivo"].ToString();
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPagaduriaAnexos() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PagaduriaID    = new UDT_BasicID();
            this.PagaduriaDesc  = new UDT_Descriptivo();
            this.DocumListaID   = new UDT_BasicID();
            this.DocumListaDesc = new UDT_Descriptivo();
            this.Descriptivo    = new UDT_DescripTBase();
            
        }

        public DTO_ccPagaduriaAnexos(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccPagaduriaAnexos(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }

        [DataMember]
        public UDT_BasicID DocumListaID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumListaDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

    }

}
