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
    /// Models DTO_inPartesComponentes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inPartesComponentes : DTO_MasterComplex
    {
        #region DTO_inPartesComponentes
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inPartesComponentes(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ReferenciaGrupoDesc.Value = dr["ReferenciaGrupoDesc"].ToString();
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                }

                this.ReferenciaGrupo.Value = dr["ReferenciaGrupo"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
            }
            catch(Exception e)
            {
                throw e; 
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inPartesComponentes() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ReferenciaGrupo = new UDT_BasicID();
            this.ReferenciaGrupoDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.Cantidad = new UDT_Cantidad();   
        }

        public DTO_inPartesComponentes(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inPartesComponentes(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID ReferenciaGrupo { get; set; }

        [DataMember]
        public UDT_Descriptivo ReferenciaGrupoDesc { get; set; }
      
        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }
    }
}
