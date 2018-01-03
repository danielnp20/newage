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
    /// Models DTO_coComprBalanceTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coComprBalanceTipo : DTO_MasterComplex
    {
        #region DTO_coComprBalanceTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coComprBalanceTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.BalanceDesc.Value = dr["BalanceDesc"].ToString();
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                }

                this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coComprBalanceTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BalanceTipoID = new UDT_BasicID();
            this.BalanceDesc = new UDT_Descriptivo();
            this.ComprobanteID = new UDT_BasicID();
            this.ComprobanteDesc = new UDT_Descriptivo();
        }

        public DTO_coComprBalanceTipo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coComprBalanceTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo BalanceDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }
    }

}
