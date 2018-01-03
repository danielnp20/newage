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
    /// Models DTO_ccCompradorCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompradorCartera : DTO_MasterBasic
    {
        #region DTO_caPagaduria
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorCartera(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.CompradorGrupoDesc.Value = dr["CompradorGrupoDesc"].ToString();
                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.CompradorGrupoID.Value = dr["CompradorGrupoID"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoOferta"].ToString()))
                    this.TipoOferta.Value = Convert.ToByte(dr["TipoOferta"]);
                if (!string.IsNullOrEmpty(dr["Antiguedad"].ToString()))
                    this.Antiguedad.Value = Convert.ToByte(dr["Antiguedad"]);
                if (!string.IsNullOrEmpty(dr["TopeEdad"].ToString()))
                    this.TopeEdad.Value = Convert.ToByte(dr["TopeEdad"]);
                if (!string.IsNullOrEmpty(dr["PlazoMinimo"].ToString()))
                    this.PlazoMinimo.Value = Convert.ToByte(dr["PlazoMinimo"]);
                if (!string.IsNullOrEmpty(dr["PlazoMaximo"].ToString()))
                    this.PlazoMaximo.Value = Convert.ToByte(dr["PlazoMaximo"]);
                if (!string.IsNullOrEmpty(dr["MaduracionAntInd"].ToString()))
                    this.MaduracionAntInd.Value = Convert.ToBoolean(dr["MaduracionAntInd"]);
                if (!string.IsNullOrEmpty(dr["CalifBancaria"].ToString()))
                    this.CalifBancaria.Value = Convert.ToByte(dr["CalifBancaria"]);
                if (!string.IsNullOrEmpty(dr["CalifCoop"].ToString()))
                    this.CalifCoop.Value = Convert.ToByte(dr["CalifCoop"]);
                if (!string.IsNullOrEmpty(dr["CalifOtros"].ToString()))
                    this.CalifOtros.Value = Convert.ToByte(dr["CalifOtros"]);
                this.DTFInd.Value = Convert.ToBoolean(dr["DTFInd"]);
                if (!string.IsNullOrEmpty(dr["PagoSeguroInd"].ToString()))
                    this.PagoSeguroInd.Value = Convert.ToBoolean(dr["PagoSeguroInd"]);
                if (!string.IsNullOrEmpty(dr["CtaPagosTotalesInd"].ToString()))
                    this.CtaPagosTotalesInd.Value = Convert.ToBoolean(dr["CtaPagosTotalesInd"]);
                this.TipoFactorCesion.Value = Convert.ToByte(dr["TipoFactorCesion"]);
                this.TipoFactorRecompra.Value = Convert.ToByte(dr["TipoFactorRecompra"]);
                this.TipoControlRecursos.Value = Convert.ToByte(dr["TipoControlRecursos"]);
                if (!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                if (!string.IsNullOrEmpty(dr["FactorRecompra"].ToString()))
                    this.FactorRecompra.Value = Convert.ToDecimal(dr["FactorRecompra"]);
                if (!string.IsNullOrEmpty(dr["PorReservaVta"].ToString()))
                    this.PorReservaVta.Value = Convert.ToDecimal(dr["PorReservaVta"]);
                if (!string.IsNullOrEmpty(dr["PeriodoGracia"].ToString()))
                    this.PeriodoGracia.Value = Convert.ToByte(dr["PeriodoGracia"]);
                this.Dias365Ind.Value = Convert.ToBoolean(dr["Dias365Ind"]);
                if (!string.IsNullOrEmpty(dr["CuotasMoraMax"].ToString()))
                    this.CuotasMoraMax.Value = Convert.ToByte(dr["CuotasMoraMax"]);
                this.DiaCorte.Value = Convert.ToByte(dr["DiaCorte"]);
                this.TipoLiquidacion.Value = Convert.ToByte(dr["TipoLiquidacion"]);
                this.ResponsabilidadInd.Value = Convert.ToBoolean(dr["ResponsabilidadInd"]);
                this.CompraFlujoTotalInd.Value = Convert.ToBoolean(dr["CompraFlujoTotalInd"]);
                this.PagoFlujosDirectoInd.Value = Convert.ToBoolean(dr["PagoFlujosDirectoInd"]);
                this.InversionistaFinalInd.Value = Convert.ToBoolean(dr["InversionistaFinalInd"]);
                this.CuotasMinPagadas.Value = Convert.ToByte(dr["CuotasMinPagadas"]);
                this.EditaTasaInd.Value = Convert.ToBoolean(dr["EditaTasaInd"]);
                this.PortafolioInd.Value = Convert.ToBoolean(dr["PortafolioInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorCartera()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.CompradorGrupoID = new UDT_BasicID();
            this.CompradorGrupoDesc = new UDT_Descriptivo();
            this.TipoOferta = new UDTSQL_tinyint();
            this.Antiguedad = new UDTSQL_tinyint();
            this.TopeEdad = new UDTSQL_tinyint();
            this.PlazoMinimo = new UDTSQL_tinyint();
            this.PlazoMaximo = new UDTSQL_tinyint();
            this.MaduracionAntInd = new UDT_SiNo();
            this.DTFInd = new UDT_SiNo();
            this.PagoSeguroInd = new UDT_SiNo();
            this.CtaPagosTotalesInd = new UDT_SiNo();
            this.CalifBancaria = new UDTSQL_tinyint();
            this.CalifCoop = new UDTSQL_tinyint();
            this.CalifOtros = new UDTSQL_tinyint();
            this.TipoFactorCesion = new UDTSQL_tinyint();
            this.TipoFactorRecompra = new UDTSQL_tinyint();
            this.TipoControlRecursos = new UDTSQL_tinyint();
            this.FactorCesion = new UDT_PorcentajeCarteraID();
            this.FactorRecompra = new UDT_PorcentajeCarteraID();
            this.PorReservaVta = new UDT_PorcentajeCarteraID();
            this.PeriodoGracia = new UDTSQL_tinyint();
            this.Dias365Ind = new UDT_SiNo();
            this.CuotasMoraMax = new UDTSQL_tinyint();
            this.DiaCorte = new UDTSQL_tinyint();
            this.TipoLiquidacion = new UDTSQL_tinyint();
            this.ResponsabilidadInd = new UDT_SiNo();
            this.CompraFlujoTotalInd = new UDT_SiNo();
            this.PagoFlujosDirectoInd = new UDT_SiNo();
            this.InversionistaFinalInd = new UDT_SiNo();
            this.CuotasMinPagadas = new UDTSQL_tinyint();
            this.EditaTasaInd = new UDT_SiNo();
            this.PortafolioInd = new UDT_SiNo();

        }

        public DTO_ccCompradorCartera(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCompradorCartera(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID CompradorGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CompradorGrupoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoOferta { get; set; }

        [DataMember]
        public UDTSQL_tinyint Antiguedad { get; set; }

        [DataMember]
        public UDTSQL_tinyint TopeEdad { get; set; }

        [DataMember]
        public UDTSQL_tinyint PlazoMinimo { get; set; }

        [DataMember]
        public UDTSQL_tinyint PlazoMaximo { get; set; }

        [DataMember]
        public UDT_SiNo MaduracionAntInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint CalifBancaria { get; set; }

        [DataMember]
        public UDTSQL_tinyint CalifCoop { get; set; }

        [DataMember]
        public UDTSQL_tinyint CalifOtros { get; set; }

        [DataMember]
        public UDT_SiNo DTFInd { get; set; }

        [DataMember]
        public UDT_SiNo PagoSeguroInd { get; set; }

        [DataMember]
        public UDT_SiNo CtaPagosTotalesInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoFactorCesion { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoFactorRecompra { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoControlRecursos { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID FactorCesion { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID FactorRecompra { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorReservaVta { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoGracia { get; set; }

        [DataMember]
        public UDT_SiNo Dias365Ind { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuotasMoraMax { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiaCorte { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLiquidacion { get; set; }

        [DataMember]
        public UDT_SiNo ResponsabilidadInd { get; set; }

        [DataMember]
        public UDT_SiNo CompraFlujoTotalInd { get; set; }

        [DataMember]
        public UDT_SiNo PagoFlujosDirectoInd { get; set; }

        [DataMember]
        public UDT_SiNo InversionistaFinalInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuotasMinPagadas { get; set; }

        [DataMember]
        public UDT_SiNo EditaTasaInd { get; set; }

        [DataMember]
        public UDT_SiNo PortafolioInd { get; set; }

        #endregion
    }
}
