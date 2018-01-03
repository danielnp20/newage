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
    /// Models DTO_coImpuestoFormato
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpuestoFormato : DTO_MasterBasic
    {
        #region DTO_coImpuestoFormato
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpuestoFormato(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                this.TipoFiscal.Value = Convert.ToByte(dr["TipoFiscal"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpuestoFormato() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.TipoFiscal = new UDTSQL_tinyint();
        }

        public DTO_coImpuestoFormato(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coImpuestoFormato(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoFiscal { get; set; }
  
    }
}
