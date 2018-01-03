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
    /// Models DTO_acComponenteActivo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acComponenteActivo : DTO_MasterBasic
    {
        #region DTO_acComponenteActivo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acComponenteActivo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.ConceptoSaldoDesc.Value = dr["ConceptoSaldoDesc"].ToString();

                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.TipoComponente.Value = Convert.ToByte(dr["TipoComponente"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acComponenteActivo() : base()
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
        }

        public DTO_acComponenteActivo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acComponenteActivo(DTO_aplMaestraPropiedades masterProperties)
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
    }
}
