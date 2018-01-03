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
    /// Models DTO_ccCarteraComponente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCarteraComponente : DTO_MasterBasic
    {
        #region DTO_ccCarteraComponente
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCarteraComponente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoSaldoDesc.Value = dr["ConceptoSaldoDesc"].ToString();
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.ServicioDesc.Value = dr["ServicioDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ComponenteCarteraDesc.Value = dr["ComponenteCarteraDesc"].ToString();
                    this.CuentaAlternaPRJDesc.Value = dr["CuentaAlternaPRJDesc"].ToString();
                }
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.ComponenteLiquidaInd.Value = Convert.ToBoolean(dr["ComponenteLiquidaInd"]);
                this.TipoComponente.Value = Convert.ToByte(dr["TipoComponente"]);
                this.TipoLiquida.Value = Convert.ToByte(dr["TipoLiquida"]);
                this.TipoValor.Value = Convert.ToByte(dr["TipoValor"]);
                this.FuenteLiquida.Value = Convert.ToByte(dr["FuenteLiquida"]);
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                    this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                this.IvaLiquida.Value = Convert.ToByte(dr["IvaLiquida"]);
                this.ClienteNuevoInd.Value = Convert.ToBoolean(dr["ClienteNuevoInd"]);
                this.TerceroTipo.Value = Convert.ToByte(dr["TerceroTipo"]);
                this.PropiedadTercerosoInd.Value = Convert.ToBoolean(dr["PropiedadTercerosoInd"]);
                this.IngresoContab.Value = Convert.ToByte(dr["IngresoContab"]);
                this.RecAnticipadoInd.Value = Convert.ToBoolean(dr["RecAnticipadoInd"]);
                this.PagoTotalInd.Value = Convert.ToBoolean(dr["PagoTotalInd"]);
                this.ComponenteAUT.Value = dr["ComponenteAUT"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ServicioID.Value = dr["ServicioID"].ToString();
                if (!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                this.NumeroComp.Value = Convert.ToByte(dr["NumeroComp"]);
                this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["PorDistribuye"].ToString()))
                    this.PorDistribuye.Value = Convert.ToDecimal(dr["PorDistribuye"]);
                if (!string.IsNullOrEmpty(dr["AsistenciaInd"].ToString()))
                    this.AsistenciaInd.Value = Convert.ToBoolean(dr["AsistenciaInd"]);
                if (!string.IsNullOrEmpty(dr["TipoCreditoInd"].ToString()))
                    this.TipoCreditoInd.Value = Convert.ToBoolean(dr["TipoCreditoInd"]);
                this.CuentaAlternaPRJ.Value = dr["CuentaAlternaPRJ"].ToString();

                if (!string.IsNullOrEmpty(dr["SentenciaInd"].ToString()))
                    this.SentenciaInd.Value = Convert.ToBoolean(dr["SentenciaInd"]);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCarteraComponente()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConceptoSaldoID = new UDT_BasicID();
            this.ConceptoSaldoDesc = new UDT_Descriptivo();
            this.ComponenteLiquidaInd = new UDT_SiNo();
            this.TipoComponente = new UDTSQL_tinyint();
            this.TipoLiquida = new UDTSQL_tinyint();
            this.TipoValor = new UDTSQL_tinyint();
            this.FuenteLiquida = new UDTSQL_tinyint();
            this.Valor = new UDT_Valor();
            this.PorcentajeID = new  UDT_PorcentajeCarteraID();
            this.IvaLiquida = new UDTSQL_tinyint();
            this.ClienteNuevoInd = new UDT_SiNo();
            this.TerceroTipo = new UDTSQL_tinyint();
            this.PropiedadTercerosoInd = new UDT_SiNo();
            this.IngresoContab = new UDTSQL_tinyint();
            this.RecAnticipadoInd = new UDT_SiNo();
            this.PagoTotalInd = new UDT_SiNo();
            this.ComponenteAUT = new UDT_BasicID();
            this.ComponenteCarteraDesc=new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.ServicioID = new UDT_BasicID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.TerceroDesc = new UDT_Descriptivo();
            this.FactorCesion = new UDT_PorcentajeID();
            this.NumeroComp = new UDTSQL_tinyint();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.PorDistribuye = new UDT_PorcentajeID();
            this.AsistenciaInd = new UDT_SiNo();
            this.TipoCreditoInd = new UDT_SiNo();
            this.CuentaAlternaPRJ = new UDT_BasicID();
            this.CuentaAlternaPRJDesc = new UDT_Descriptivo();
            this.SentenciaInd = new UDT_SiNo();
        }

        public DTO_ccCarteraComponente(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCarteraComponente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoSaldoDesc { get; set; }

        [DataMember]
        public UDT_SiNo ComponenteLiquidaInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoComponente { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLiquida { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoValor { get; set; }

        [DataMember]
        public UDTSQL_tinyint FuenteLiquida { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorcentajeID { get; set; }

        [DataMember]
        public UDTSQL_tinyint IvaLiquida { get; set; }

        [DataMember]
        public UDT_SiNo ClienteNuevoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TerceroTipo { get; set; }

        [DataMember]
        public UDT_SiNo PropiedadTercerosoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint IngresoContab { get; set; }

        [DataMember]
        public UDT_SiNo RecAnticipadoInd { get; set; }

        [DataMember]
        public UDT_SiNo PagoTotalInd { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteAUT { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteCarteraDesc { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_BasicID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID FactorCesion { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroComp { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorDistribuye { get; set; }

        [DataMember]
        public UDT_SiNo AsistenciaInd { get; set; }

        [DataMember]
        public UDT_SiNo TipoCreditoInd { get; set; }

        [DataMember]
        public UDT_BasicID CuentaAlternaPRJ { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaAlternaPRJDesc { get; set; }

        [DataMember]
        public UDT_SiNo SentenciaInd { get; set; }

        #endregion
    }
}

