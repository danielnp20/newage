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
    /// Models DTO_ccReintegroSaldo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccReintegroSaldo : DTO_MasterBasic
    {
        #region DTO_ccReintegroSaldo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccReintegroSaldo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.CoDocumentoDesc.Value = dr["CoDocumentoDesc"].ToString();
                }

                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.CoDocumentoID.Value = dr["CoDocumentoID"].ToString(); 
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccReintegroSaldo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.CoDocumentoID = new UDT_BasicID();
            this.CoDocumentoDesc = new UDT_Descriptivo();
        }

        public DTO_ccReintegroSaldo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccReintegroSaldo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID CoDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CoDocumentoDesc { get; set; }


    }

}
