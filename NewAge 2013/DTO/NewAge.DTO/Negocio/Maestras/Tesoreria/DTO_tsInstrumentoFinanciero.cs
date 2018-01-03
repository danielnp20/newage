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
    /// Models DTO_tsInstrumentoFinanciero
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_tsInstrumentoFinanciero : DTO_MasterBasic
    {
        #region DTO_tsInstrumentoFinanciero
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsInstrumentoFinanciero(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoIngresoDesc.Value = dr["ConceptoIngresoDesc"].ToString();
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.ConceptoCxPDesc.Value = dr["ConceptoCxPDesc"].ToString();
                }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.TipoInstrumento.Value = Convert.ToByte(dr["TipoInstrumento"]);
                this.RiesgoCreditoInd.Value = Convert.ToBoolean(dr["RiesgoCreditoInd"]);
                this.RiesgoTasaCambioInd.Value = Convert.ToBoolean(dr["RiesgoTasaCambioInd"]);
                this.RiesgoTasaInteresInd.Value = Convert.ToBoolean(dr["RiesgoTasaInteresInd"]);
                this.RiesgoLiquidezInd.Value = Convert.ToBoolean(dr["RiesgoLiquidezInd"]);
                this.RiesgoPmoxPagarInd.Value = Convert.ToBoolean(dr["RiesgoPmoxPagarInd"]);
                this.RiesgoMercadoInd.Value = Convert.ToBoolean(dr["RiesgoMercadoInd"]);
                this.RiesgoOtrosInd.Value = Convert.ToBoolean(dr["RiesgoOtrosInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ConceptoCxPID"].ToString()))
                    this.ConceptoCxPID.Value = dr["ConceptoCxPID"].ToString();
                this.ConceptoIngresoID.Value = dr["ConceptoIngresoID"].ToString();
                this.Relevancia.Value = dr["Relevancia"].ToString();
                this.RevelacionRiesgos.Value = dr["RevelacionRiesgos"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsInstrumentoFinanciero() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.TipoInstrumento = new UDTSQL_tinyint();
            this.RiesgoCreditoInd = new UDT_SiNo();
            this.RiesgoLiquidezInd = new UDT_SiNo();
            this.RiesgoPmoxPagarInd = new UDT_SiNo();
            this.RiesgoTasaCambioInd = new UDT_SiNo();
            this.RiesgoMercadoInd = new UDT_SiNo();
            this.RiesgoOtrosInd = new UDT_SiNo();
            this.ConceptoCxPID = new UDT_BasicID();
            this.ConceptoCxPDesc = new UDT_Descriptivo();
            this.ConceptoIngresoID = new UDT_BasicID();
            this.ConceptoIngresoDesc = new UDT_Descriptivo();
            this.Relevancia  = new UDT_DescripTExt();
            this.RevelacionRiesgos = new UDT_DescripTExt();
            this.RiesgoTasaInteresInd = new UDT_SiNo();
        }

        public DTO_tsInstrumentoFinanciero(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsInstrumentoFinanciero(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoInstrumento { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoCreditoInd { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoTasaCambioInd { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoTasaInteresInd { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoLiquidezInd { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoPmoxPagarInd { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoMercadoInd { get; set; }

        [DataMember]
        public UDT_SiNo RiesgoOtrosInd { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCxPID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCxPDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoIngresoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoIngresoDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Relevancia { get; set; }

        [DataMember]
        public UDT_DescripTExt RevelacionRiesgos { get; set; }
    }
}
