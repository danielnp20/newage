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
    /// Models DTO_coActividad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccClasificacionCredito : DTO_MasterBasic
    {
        #region DTO_ccClasificacionCredito
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccClasificacionCredito(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!string.IsNullOrEmpty(dr["ProvisionaInd"].ToString()))
                    this.ProvisionaInd.Value = Convert.ToBoolean(dr["ProvisionaInd"]);
                if (!string.IsNullOrEmpty(dr["PagaduriaInd"].ToString()))
                    this.PagaduriaInd.Value = Convert.ToBoolean(dr["PagaduriaInd"]);
                if (!string.IsNullOrEmpty(dr["HipotecarioInd"].ToString()))
                    this.HipotecarioInd.Value = Convert.ToBoolean(dr["HipotecarioInd"]);
                if (!string.IsNullOrEmpty(dr["PrendarioInd"].ToString()))
                    this.PrendarioInd.Value = Convert.ToBoolean(dr["PrendarioInd"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccClasificacionCredito()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProvisionaInd = new UDT_SiNo();
            this.PagaduriaInd = new UDT_SiNo();
            this.HipotecarioInd = new UDT_SiNo();
            this.PrendarioInd = new UDT_SiNo();  
        }

        public DTO_ccClasificacionCredito(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccClasificacionCredito(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_SiNo ProvisionaInd { get; set; }
        public UDT_SiNo PagaduriaInd { get; set; }
        public UDT_SiNo HipotecarioInd { get; set; }
        public UDT_SiNo PrendarioInd { get; set; }
  
    }
}
