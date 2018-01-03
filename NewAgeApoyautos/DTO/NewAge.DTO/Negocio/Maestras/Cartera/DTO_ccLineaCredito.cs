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
    /// Models DTO_ccLineaCredito
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccLineaCredito : DTO_MasterBasic
    {
        #region DTO_ccLineaCredito

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccLineaCredito(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                    this.InstrumentoFinancieroDesc.Value = dr["InstrumentoFinancieroDesc"].ToString();
                    this.ClaseCreditoDesc.Value = Convert.ToString(dr["ClaseCreditoDesc"]);
                }

                this.IndSinDesembolso.Value = Convert.ToBoolean(dr["IndSinDesembolso"]);
                this.CuotaSancionInd.Value = Convert.ToBoolean(dr["CuotaSancionInd"]);
                this.BloqueaVentaInd.Value = Convert.ToBoolean(dr["BloqueaVentaInd"]);
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["InstrumentoFinancieroID"].ToString()))
                    this.InstrumentoFinancieroID.Value = dr["InstrumentoFinancieroID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ClaseCredito"].ToString()))
                    this.ClaseCredito.Value = Convert.ToString(dr["ClaseCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoGarantia"].ToString()))
                    this.TipoGarantia.Value = Convert.ToByte(dr["TipoGarantia"]);
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccLineaCredito()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.IndSinDesembolso = new UDT_SiNo();
            this.CuotaSancionInd = new UDT_SiNo();
            this.BloqueaVentaInd = new UDT_SiNo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.InstrumentoFinancieroID = new UDT_BasicID();
            this.InstrumentoFinancieroDesc = new UDT_Descriptivo();
            this.ClaseCredito = new UDT_BasicID();
            this.ClaseCreditoDesc = new UDT_Descriptivo();
            this.TipoGarantia = new UDTSQL_tinyint();
        }

        public DTO_ccLineaCredito(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccLineaCredito(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        [DataMember]
        public UDT_SiNo IndSinDesembolso { get; set; }

        [DataMember]
        public UDT_SiNo CuotaSancionInd { get; set; }

        [DataMember]
        public UDT_SiNo BloqueaVentaInd { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID InstrumentoFinancieroID { get; set; }

        [DataMember]
        public UDT_Descriptivo InstrumentoFinancieroDesc { get; set; }

        [DataMember]
        public UDT_BasicID ClaseCredito { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseCreditoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoGarantia { get; set; }

    }
}

