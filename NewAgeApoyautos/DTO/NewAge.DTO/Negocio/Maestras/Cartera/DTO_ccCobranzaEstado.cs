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
    /// Models DTO_ccCobranzaEstado
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCobranzaEstado : DTO_MasterBasic
    {
        #region DTO_ccCobranzaEstado
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCobranzaEstado(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.CobroJuridicoInd.Value = Convert.ToBoolean(dr["CobroJuridicoInd"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCobranzaEstado() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CobroJuridicoInd = new UDT_SiNo();
        }

        public DTO_ccCobranzaEstado(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCobranzaEstado(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_SiNo CobroJuridicoInd { get; set; }
    }

}
