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
    /// Models DTO_tsInstFinancieroComponente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_tsInstFinancieroComponente : DTO_MasterBasic
    {
        #region DTO_tsInstFinancieroComponente
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsInstFinancieroComponente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoSaldoDesc.Value = dr["ConceptoSaldoDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.CuentaIVADesc.Value = dr["CuentaIVADesc"].ToString();
                }
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.TipoComponente.Value = Convert.ToByte(dr["TipoComponente"].ToString());
                this.NumeroComp.Value = Convert.ToByte(dr["NumeroComp"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaIVA"].ToString()))
                    this.CuentaIVA.Value = dr["CuentaIVA"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsInstFinancieroComponente()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConceptoSaldoID = new UDT_BasicID();
            this.ConceptoSaldoDesc = new UDT_Descriptivo();
            this.TipoComponente = new UDTSQL_tinyint();
            this.NumeroComp = new UDTSQL_tinyint();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.CuentaIVA = new UDT_BasicID();
            this.CuentaIVADesc = new UDT_Descriptivo();
        }

        public DTO_tsInstFinancieroComponente(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsInstFinancieroComponente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoSaldoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoComponente { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroComp { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaIVA { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIVADesc { get; set; }
    }
}
