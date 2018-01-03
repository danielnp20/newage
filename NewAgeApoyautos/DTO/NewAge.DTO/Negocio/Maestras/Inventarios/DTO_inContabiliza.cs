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
    /// Models DTO_inContabiliza
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inContabiliza : DTO_MasterComplex
    {
        #region DTO_inContabiliza
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inContabiliza(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.inBodegaContabDesc.Value = dr["inBodegaContabDesc"].ToString();
                    this.GrupoInvDesc.Value = dr["GrupoInvDesc"].ToString();
                }

                this.inBodegaContabID.Value = dr["inBodegaContabID"].ToString();
                this.GrupoInvID.Value = dr["GrupoInvID"].ToString();
                this.CuentaCosto.Value = dr["CuentaCosto"].ToString();
                this.CuentaCostoDesc.Value = dr["CuentaCostoDesc"].ToString();
                this.CuentaFOB.Value = dr["CuentaFOB"].ToString();
                this.CuentaFOBDesc.Value = dr["CuentaFOBDesc"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inContabiliza() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.inBodegaContabID = new UDT_BasicID();
            this.inBodegaContabDesc = new UDT_Descriptivo();
            this.GrupoInvID = new UDT_BasicID();
            this.GrupoInvDesc = new UDT_Descriptivo();
            this.CuentaCosto = new UDT_BasicID();
            this.CuentaFOB = new UDT_BasicID();
            this.CuentaCostoDesc = new UDT_Descriptivo();
            this.CuentaFOBDesc = new UDT_Descriptivo();
        }

        public DTO_inContabiliza(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inContabiliza(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID inBodegaContabID { get; set; }

        [DataMember]
        public UDT_Descriptivo inBodegaContabDesc { get; set; }
      
        [DataMember]
        public UDT_BasicID GrupoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaCosto { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaCostoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaFOB { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaFOBDesc { get; set; }
    }
}
