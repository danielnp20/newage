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
    /// Models DTO_coTercero
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glPais : DTO_MasterBasic 
    {
        #region DTO_glPais
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glPais(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.MonedaDesc.Value = dr["MonedaDesc"].ToString();
                }

                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.PaisPrefTel.Value = dr["PaisPrefTel"].ToString();
                this.PaisMM.Value = dr["PaisMM"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glPais() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.MonedaID = new UDT_BasicID();
            this.MonedaDesc = new UDT_Descriptivo();
            this.PaisPrefTel = new UDTSQL_char(7);
            this.PaisMM = new UDTSQL_char(3);
         }

        public DTO_glPais(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glPais(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
     
        [DataMember]
        public UDT_BasicID MonedaID { get; set; }

        [DataMember]
        public UDT_Descriptivo MonedaDesc { get; set; }

        [DataMember]
        public UDTSQL_char PaisPrefTel { get; set; }

        [DataMember]
        public UDTSQL_char PaisMM { get; set; }
    }

}
