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
    /// Models DTO_prAnexoLista
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prAnexoLista : DTO_MasterBasic
    {
        #region DTO_prBienServicio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prAnexoLista(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.ObligatorioInd.Value = Convert.ToBoolean(dr["ObligatorioInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prAnexoLista() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ObligatorioInd = new UDT_SiNo();
        }

        public DTO_prAnexoLista(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prAnexoLista(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_SiNo ObligatorioInd { get; set; }
    }
}
