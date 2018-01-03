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
    /// Models DTO_inTipoParametro2
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inTipoParametro2 : DTO_MasterComplex
    {
        #region DTO_inTipoParametro2
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inTipoParametro2(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TipoInvDesc.Value = dr["TipoInvDesc"].ToString();
                    this.Parametro2Desc.Value = dr["Parametro2Desc"].ToString();
                }

                this.TipoInvID.Value = dr["TipoInvID"].ToString();
                this.Parametro2ID.Value = dr["Parametro2ID"].ToString();
              
                this.PKValues["TipoInvID"] = this.TipoInvID.Value;
                this.PKValues["Parametro2ID"] = this.Parametro2ID.Value;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inTipoParametro2() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoInvID = new UDT_BasicID();
            this.TipoInvDesc = new UDT_Descriptivo();
            this.Parametro2ID = new UDT_BasicID();
            this.Parametro2Desc = new UDT_Descriptivo();
        }

        public DTO_inTipoParametro2(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inTipoParametro2(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID TipoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID Parametro2ID  { get; set; }

        [DataMember]
        public UDT_Descriptivo Parametro2Desc { get; set; }
    }

}
