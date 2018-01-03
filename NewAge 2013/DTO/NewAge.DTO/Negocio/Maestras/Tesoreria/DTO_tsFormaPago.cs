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
    /// Models DTO_tsFormaPago
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_tsFormaPago : DTO_MasterBasic
    {
        #region DTO_tsFormaPago
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsFormaPago(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.BancoDesc.Value = dr["BancoDesc"].ToString();

                this.BancoID.Value = dr["BancoID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsFormaPago() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BancoID = new UDT_BasicID();
            this.BancoDesc = new UDT_Descriptivo();
        }

        public DTO_tsFormaPago(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsFormaPago(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID BancoID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoDesc { get; set; }
    }
}
