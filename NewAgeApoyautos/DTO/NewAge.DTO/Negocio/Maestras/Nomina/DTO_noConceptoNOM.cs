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
    /// Models DTO_noConceptoNOM
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noConceptoNOM : DTO_MasterBasic
    {
        #region DTO_noConceptoNOM
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noConceptoNOM(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoNOGrupoDesc.Value = dr["ConceptoNOGrupoDesc"].ToString();
                    this.ConceptoAutomaticoDesc.Value = dr["ConceptoAutomaticoDesc"].ToString();
                    this.FondoNODesc.Value = dr["FondoNODesc"].ToString();
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ClaseBSDesc.Value = dr["ClaseBSDesc"].ToString();
                    this.LineaCertificadoDesc.Value = dr["LineaCertificadoDesc"].ToString(); ;
                }
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                this.ConceptoNOGrupoID.Value = dr["ConceptoNOGrupoID"].ToString();
                this.TipoLiquidacion.Value = Convert.ToByte(dr["TipoLiquidacion"]);
                this.Formula.Value = dr["Formula"].ToString();
                this.BaseFormula.Value = Convert.ToByte(dr["BaseFormula"]);
                this.ConceptoAutomatico.Value = dr["ConceptoAutomatico"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["PorConcAutomatico"].ToString()))
                    this.PorConcAutomatico.Value = Convert.ToDecimal(dr["PorConcAutomatico"]);
                if (!string.IsNullOrWhiteSpace(dr["TopeVlrAutomatico"].ToString()))
                    this.TopeVlrAutomatico.Value = Convert.ToDecimal(dr["TopeVlrAutomatico"]);
                if (!string.IsNullOrWhiteSpace(dr["BaseMinima"].ToString()))
                    this.BaseMinima.Value = Convert.ToDecimal(dr["BaseMinima"]);
                if (!string.IsNullOrWhiteSpace(dr["BaseMaxima"].ToString()))
                    this.BaseMaxima.Value = Convert.ToDecimal(dr["BaseMaxima"]);
                this.FondoNOID.Value = dr["FondoNOID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.TipoTercero.Value = Convert.ToByte(dr["TipoTercero"]);
                this.Ind_01.Value = Convert.ToBoolean(dr["Ind_01"]);
                this.Ind_02.Value = Convert.ToBoolean(dr["Ind_02"]);
                this.Ind_03.Value = Convert.ToBoolean(dr["Ind_03"]);
                this.Ind_04.Value = Convert.ToBoolean(dr["Ind_04"]);
                this.Ind_05.Value = Convert.ToBoolean(dr["Ind_05"]);
                this.Ind_06.Value = Convert.ToBoolean(dr["Ind_06"]);
                this.Ind_07.Value = Convert.ToBoolean(dr["Ind_07"]);
                this.Ind_08.Value = Convert.ToBoolean(dr["Ind_08"]);
                this.Ind_09.Value = Convert.ToBoolean(dr["Ind_09"]);
                this.Ind_10.Value = Convert.ToBoolean(dr["Ind_10"]);
                this.Ind_11.Value = Convert.ToBoolean(dr["Ind_11"]);
                this.Ind_12.Value = Convert.ToBoolean(dr["Ind_12"]);
                this.Ind_13.Value = Convert.ToBoolean(dr["Ind_13"]);
                this.Ind_14.Value = Convert.ToBoolean(dr["Ind_14"]);
                this.Ind_15.Value = Convert.ToBoolean(dr["Ind_15"]);
                this.Ind_16.Value = Convert.ToBoolean(dr["Ind_16"]);
                this.Ind_17.Value = Convert.ToBoolean(dr["Ind_17"]);
                this.Ind_18.Value = Convert.ToBoolean(dr["Ind_18"]);
                this.Ind_19.Value = Convert.ToBoolean(dr["Ind_19"]);
                this.Ind_20.Value = Convert.ToBoolean(dr["Ind_20"]);
                this.Ind_21.Value = Convert.ToBoolean(dr["Ind_21"]);
                this.Ind_22.Value = Convert.ToBoolean(dr["Ind_22"]);
                this.Ind_23.Value = Convert.ToBoolean(dr["Ind_23"]);
                this.Ind_24.Value = Convert.ToBoolean(dr["Ind_24"]);
                this.Ind_25.Value = Convert.ToBoolean(dr["Ind_25"]);
                this.Ind_26.Value = Convert.ToBoolean(dr["Ind_26"]);
                this.Ind_27.Value = Convert.ToBoolean(dr["Ind_27"]);
                this.Ind_28.Value = Convert.ToBoolean(dr["Ind_28"]);
                this.Ind_29.Value = Convert.ToBoolean(dr["Ind_29"]);
                this.Ind_30.Value = Convert.ToBoolean(dr["Ind_30"]);
                this.Ind_31.Value = Convert.ToBoolean(dr["Ind_31"]);
                this.Ind_32.Value = Convert.ToBoolean(dr["Ind_32"]);
                this.Ind_33.Value = Convert.ToBoolean(dr["Ind_33"]);
                this.Ind_34.Value = Convert.ToBoolean(dr["Ind_34"]);
                this.Ind_35.Value = Convert.ToBoolean(dr["Ind_35"]);
                this.Ind_36.Value = Convert.ToBoolean(dr["Ind_36"]);
                this.Ind_37.Value = Convert.ToBoolean(dr["Ind_37"]);
                this.Ind_38.Value = Convert.ToBoolean(dr["Ind_38"]);
                this.Ind_39.Value = Convert.ToBoolean(dr["Ind_39"]);
                this.Ind_40.Value = Convert.ToBoolean(dr["Ind_40"]);
                if (!string.IsNullOrEmpty(dr["ClaseBSID"].ToString()))
                    this.ClaseBSID.Value = dr["ClaseBSID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaCertificado"].ToString()))
                    this.LineaCertificado.Value = dr["LineaCertificado"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noConceptoNOM()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.ConceptoNOGrupoID = new UDT_BasicID();
            this.ConceptoNOGrupoDesc = new UDT_Descriptivo();
            this.TipoLiquidacion = new UDTSQL_tinyint();
            this.Formula = new UDT_DescripTBase();
            this.BaseFormula = new UDTSQL_tinyint();
            this.ConceptoAutomatico = new UDT_BasicID();
            this.ConceptoAutomaticoDesc = new UDT_Descriptivo();
            this.PorConcAutomatico = new UDT_PorcentajeID();
            this.TopeVlrAutomatico = new UDT_FactorID();
            this.BaseMinima = new UDTSQL_numeric();
            this.BaseMaxima = new UDTSQL_numeric();
            this.FondoNOID = new UDT_BasicID();
            this.FondoNODesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.TipoTercero = new UDTSQL_tinyint();
            this.Ind_01 = new UDT_SiNo();
            this.Ind_02 = new UDT_SiNo();
            this.Ind_03 = new UDT_SiNo();
            this.Ind_04 = new UDT_SiNo();
            this.Ind_05 = new UDT_SiNo();
            this.Ind_06 = new UDT_SiNo();
            this.Ind_07 = new UDT_SiNo();
            this.Ind_08 = new UDT_SiNo();
            this.Ind_09 = new UDT_SiNo();
            this.Ind_10 = new UDT_SiNo();
            this.Ind_11 = new UDT_SiNo();
            this.Ind_12 = new UDT_SiNo();
            this.Ind_13 = new UDT_SiNo();
            this.Ind_14 = new UDT_SiNo();
            this.Ind_15 = new UDT_SiNo();
            this.Ind_16 = new UDT_SiNo();
            this.Ind_17 = new UDT_SiNo();
            this.Ind_18 = new UDT_SiNo();
            this.Ind_19 = new UDT_SiNo();
            this.Ind_20 = new UDT_SiNo();
            this.Ind_21 = new UDT_SiNo();
            this.Ind_22 = new UDT_SiNo();
            this.Ind_23 = new UDT_SiNo();
            this.Ind_24 = new UDT_SiNo();
            this.Ind_25 = new UDT_SiNo();
            this.Ind_26 = new UDT_SiNo();
            this.Ind_27 = new UDT_SiNo();
            this.Ind_28 = new UDT_SiNo();
            this.Ind_29 = new UDT_SiNo();
            this.Ind_30 = new UDT_SiNo();
            this.Ind_31 = new UDT_SiNo();
            this.Ind_32 = new UDT_SiNo();
            this.Ind_33 = new UDT_SiNo();
            this.Ind_34 = new UDT_SiNo();
            this.Ind_35 = new UDT_SiNo();
            this.Ind_36 = new UDT_SiNo();
            this.Ind_37 = new UDT_SiNo();
            this.Ind_38 = new UDT_SiNo();
            this.Ind_39 = new UDT_SiNo();
            this.Ind_40 = new UDT_SiNo();
            this.ClaseBSID = new UDT_BasicID();
            this.ClaseBSDesc = new UDT_BasicID();
            this.LineaCertificado = new UDT_BasicID();
            this.LineaCertificadoDesc = new UDT_BasicID();
        }

        public DTO_noConceptoNOM(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noConceptoNOM(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNOGrupoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLiquidacion { get; set; }

        [DataMember]
        public UDT_DescripTBase Formula { get; set; }

        [DataMember]
        public UDTSQL_tinyint BaseFormula { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoAutomatico { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoAutomaticoDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorConcAutomatico { get; set; }

        [DataMember]
        public UDT_FactorID TopeVlrAutomatico { get; set; }

        [DataMember]
        public UDTSQL_numeric BaseMinima { get; set; }

        [DataMember]
        public UDTSQL_numeric BaseMaxima { get; set; }

        [DataMember]
        public UDT_BasicID FondoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo FondoNODesc { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoTercero { get; set; }

        [DataMember]
        public UDT_SiNo Ind_01 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_02 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_03 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_04 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_05 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_06 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_07 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_08 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_09 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_10 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_11 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_12 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_13 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_14 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_15 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_16 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_17 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_18 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_19 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_20 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_21 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_22 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_23 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_24 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_25 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_26 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_27 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_28 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_29 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_30 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_31 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_32 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_33 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_34 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_35 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_36 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_37 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_38 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_39 { get; set; }

        [DataMember]
        public UDT_SiNo Ind_40 { get; set; }

        [DataMember]
        public UDT_BasicID ClaseBSID { get; set; }

        [DataMember]
        public UDT_BasicID ClaseBSDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaCertificado { get; set; }

        [DataMember]
        public UDT_BasicID LineaCertificadoDesc { get; set; } 
        #endregion
    }
}

