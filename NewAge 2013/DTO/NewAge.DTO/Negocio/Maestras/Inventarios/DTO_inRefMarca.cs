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
    /// Models DTO_inRefMarca
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inRefMarca : DTO_MasterComplex
    {
        #region DTO_inRefMarca
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inRefMarca(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                    this.MarcaInvDesc.Value = dr["MarcaInvDesc"].ToString();
                }

                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.MarcaInvID.Value = dr["MarcaInvID"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inRefMarca() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.MarcaInvID = new UDT_BasicID();
            this.MarcaInvDesc = new UDT_Descriptivo();                
        }

        public DTO_inRefMarca(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inRefMarca(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion     

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }

        [DataMember]
        public UDT_BasicID MarcaInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaInvDesc { get; set; }
    }
}
