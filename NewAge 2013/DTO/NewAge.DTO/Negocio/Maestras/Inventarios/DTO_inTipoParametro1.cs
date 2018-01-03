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
    /// Models DTO_inTipoParametro1
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inTipoParametro1 : DTO_MasterComplex
    {
        #region DTO_inTipoParametro1
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inTipoParametro1(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TipoInvDesc.Value = dr["TipoInvDesc"].ToString();
                    this.Parametro1Desc.Value = dr["Parametro1Desc"].ToString();
                }

                this.TipoInvID.Value = dr["TipoInvID"].ToString();
                this.Parametro1ID.Value = dr["Parametro1ID"].ToString();

                this.PKValues["TipoInvID"] = this.TipoInvID.Value;
                this.PKValues["Parametro1ID"] = this.Parametro1ID.Value;
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inTipoParametro1() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoInvID = new UDT_BasicID();
            this.TipoInvDesc = new UDT_Descriptivo();
            this.Parametro1ID = new UDT_BasicID();
            this.Parametro1Desc = new UDT_Descriptivo();
        }

        public DTO_inTipoParametro1(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inTipoParametro1(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID TipoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID Parametro1ID  { get; set; }

        [DataMember]
        public UDT_Descriptivo Parametro1Desc { get; set; }
    }

}
