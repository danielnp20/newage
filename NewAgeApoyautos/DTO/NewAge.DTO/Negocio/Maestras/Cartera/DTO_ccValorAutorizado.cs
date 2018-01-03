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
    /// Models DTO_ccLineaComponente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccValorAutorizado : DTO_MasterComplex
    {
        #region DTO_ccValorAutorizado
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccValorAutorizado(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.Plazo.Value = Convert.ToByte(dr["Plazo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccValorAutorizado() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Valor = new UDT_Valor();
            this.Plazo = new UDTSQL_tinyint();
        }

        public DTO_ccValorAutorizado(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccValorAutorizado(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_tinyint Plazo { get; set; }
    }

}
