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
    /// Models DTO_coControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coControl : DTO_MasterComplex
    {
        #region DTO_coImpuesto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coControl(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.OperacionDesc.Value = dr["OperacionDesc"].ToString();
                }
                
                this.CuentaID.Value = dr["CuentaID"].ToString(); 
                this.OperacionID.Value = dr["OperacionID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ExcluyeInd.Value = Convert.ToBoolean(dr["ExcluyeInd"]);               
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coControl() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.OperacionID = new UDT_BasicID();
            this.OperacionDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.ExcluyeInd = new UDT_SiNo();
        }

        public DTO_coControl(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_coControl(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
      
        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID OperacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo OperacionDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeInd { get; set; }

        

       
        
    }
}
