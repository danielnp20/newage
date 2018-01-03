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
    /// Models DTO_coCuentaGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coCuentaGrupo : DTO_MasterBasic
    {
        #region DTO_coCuentaGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coCuentaGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.ConsolidaPor.Value = Convert.ToByte(dr["ConsolidaPor"]);
                this.MascaraCta.Value = Convert.ToByte(dr["MascaraCta"]);
                this.TipoCuenta.Value = Convert.ToByte(dr["TipoCuenta"]);
                this.CostoInd.Value = Convert.ToBoolean(dr["CostoInd"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCuentaGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConsolidaPor = new UDTSQL_tinyint();
            this.MascaraCta = new UDTSQL_tinyint();
            this.TipoCuenta = new UDTSQL_tinyint();
            this.CostoInd = new UDT_SiNo();
        }

        public DTO_coCuentaGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coCuentaGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDTSQL_tinyint ConsolidaPor { get; set; }

        [DataMember]
        public UDTSQL_tinyint MascaraCta { get; set; }

        [DataMember]
        public UDT_SiNo CostoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCuenta { get; set; }
  
    }
}
