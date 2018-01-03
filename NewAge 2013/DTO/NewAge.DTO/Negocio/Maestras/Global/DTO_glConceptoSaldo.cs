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
    /// Models DTO_glConceptoSaldo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glConceptoSaldo : DTO_MasterBasic
    {
        #region DTO_glConceptoSaldo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glConceptoSaldo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ModuloDesc.Value = dr["ModuloDesc"].ToString();
                }

                this.coSaldoControl.Value = Convert.ToByte(dr["coSaldoControl"]);
                this.ModuloID.Value = dr["ModuloID"].ToString();
                this.NumeroComp.Value = Convert.ToByte(dr["NumeroComp"]);
                this.ControlComponenteInd.Value = Convert.ToBoolean(dr["ControlComponenteInd"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glConceptoSaldo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.coSaldoControl = new UDTSQL_tinyint();
            this.ModuloID = new UDT_BasicID();
            this.ModuloDesc = new UDT_Descriptivo();
            this.NumeroComp = new UDTSQL_tinyint();
            this.ControlComponenteInd = new UDT_SiNo();
        }

        public DTO_glConceptoSaldo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glConceptoSaldo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint coSaldoControl { get; set; }

        [DataMember]
        public UDT_BasicID ModuloID { get; set; }

        [DataMember]
        public UDT_Descriptivo ModuloDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroComp { get; set; }

        [DataMember]
        public UDT_SiNo ControlComponenteInd { get; set; }
    }
}
