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
    /// Models DTO_cpConceptoCXP
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpConceptoCXP : DTO_MasterBasic
    {
        #region DTO_cpConceptoCXP

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpConceptoCXP(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.ConceptoTipo.Value = Convert.ToByte(dr["ConceptoTipo"]);
                this.ControlCostoInd.Value = Convert.ToBoolean(dr["ControlCostoInd"]);
                this.LibroComprasInd.Value = Convert.ToBoolean(dr["LibroComprasInd"]);
                this.DistribucionPagosInd.Value = Convert.ToBoolean(dr["DistribucionPagosInd"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpConceptoCXP() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //this.ComprobanteDocID  = new  UDT_BasicID();
            //this.ComprobanteDocDesc = new UDT_Descriptivo();
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.ConceptoTipo = new UDTSQL_tinyint();
            this.ControlCostoInd = new UDT_SiNo();
            this.LibroComprasInd = new UDT_SiNo();
            this.DistribucionPagosInd = new UDT_SiNo();
        }

        public DTO_cpConceptoCXP(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpConceptoCXP(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ConceptoTipo { get; set; }

        [DataMember]
        public UDT_SiNo ControlCostoInd { get; set; }

        [DataMember]
        public UDT_SiNo LibroComprasInd { get; set; }

        [DataMember]
        public UDT_SiNo DistribucionPagosInd { get; set; }
        
    }
}
