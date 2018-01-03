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
    /// Models DTO_noContratoNov
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noContratoNov : DTO_MasterBasic
    {
        #region DTO_noContratoNov
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noContratoNov(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.Concepto1Desc.Value = dr["Concepto1Desc"].ToString();
                    this.Concepto2Desc.Value = dr["Concepto2Desc"].ToString();
                    this.Concepto3Desc.Value = dr["Concepto3Desc"].ToString();
                }
                this.DescuentaDiasInd.Value = Convert.ToBoolean(dr["DescuentaDiasInd"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasIni1"].ToString()))
                    this.DiasIni1.Value = Convert.ToInt32(dr["DiasIni1"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasFin1"].ToString()))
                    this.DiasFin1.Value = Convert.ToInt32(dr["DiasFin1"]);
                if (!string.IsNullOrWhiteSpace(dr["Concepto1"].ToString()))
                    this.Concepto1.Value = dr["Concepto1"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Porcentaje1"].ToString()))
                    this.Porcentaje1.Value = Convert.ToDecimal(dr["Porcentaje1"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasIni2"].ToString()))
                    this.DiasIni2.Value = Convert.ToInt32(dr["DiasIni2"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasFin2"].ToString()))
                    this.DiasFin2.Value = Convert.ToInt32(dr["DiasFin2"]);
                if (!string.IsNullOrWhiteSpace(dr["Concepto2"].ToString()))
                    this.Concepto2.Value = dr["Concepto2"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Porcentaje2"].ToString()))
                    this.Porcentaje2.Value = Convert.ToDecimal(dr["Porcentaje2"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasIni3"].ToString()))
                    this.DiasIni3.Value = Convert.ToInt32(dr["DiasIni3"]);
                if (!string.IsNullOrWhiteSpace(dr["Concepto3"].ToString()))
                    this.Concepto3.Value = dr["Concepto3"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DiasFin3"].ToString()))
                    this.DiasFin3.Value = Convert.ToInt32(dr["DiasFin3"]);
                if (!string.IsNullOrWhiteSpace(dr["Porcentaje3"].ToString()))
                    this.Porcentaje3.Value = Convert.ToDecimal(dr["Porcentaje3"]);
                this.TipoNovedad.Value = Convert.ToByte(dr["TipoNovedad"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noContratoNov() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DescuentaDiasInd = new UDT_SiNo();
            this.DiasIni1 = new UDTSQL_int();
            this.DiasFin1 = new UDTSQL_int();
            this.Concepto1 = new UDT_BasicID();
            this.Concepto1Desc = new UDT_Descriptivo();
            this.Porcentaje1 = new UDT_PorcentajeID();
            this.DiasIni2 = new UDTSQL_int();
            this.DiasFin2 = new UDTSQL_int();
            this.Concepto2 = new UDT_BasicID();
            this.Concepto2Desc = new UDT_Descriptivo();
            this.Porcentaje2 = new UDT_PorcentajeID();
            this.DiasIni3 = new UDTSQL_int();
            this.DiasFin3 = new UDTSQL_int();
            this.Concepto3 = new UDT_BasicID();
            this.Concepto3Desc = new UDT_Descriptivo();
            this.Porcentaje3 = new UDT_PorcentajeID();
            this.TipoNovedad = new UDTSQL_tinyint();
        }

        public DTO_noContratoNov(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noContratoNov(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_SiNo DescuentaDiasInd { get; set; }

        [DataMember]
        public UDTSQL_int DiasIni1 { get; set; }

        [DataMember]
        public UDTSQL_int DiasFin1 { get; set; }

        [DataMember]
        public UDT_BasicID Concepto1 { get; set; }

        [DataMember]
        public UDT_Descriptivo Concepto1Desc { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje1 { get; set; }

        [DataMember]
        public UDTSQL_int DiasIni2 { get; set; }

        [DataMember]
        public UDTSQL_int DiasFin2 { get; set; }

        [DataMember]
        public UDT_BasicID Concepto2 { get; set; }

        [DataMember]
        public UDT_Descriptivo Concepto2Desc { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje2 { get; set; }

        [DataMember]
        public UDTSQL_int DiasIni3 { get; set; }

        [DataMember]
        public UDTSQL_int DiasFin3 { get; set; }

        [DataMember]
        public UDT_BasicID Concepto3 { get; set; }

        [DataMember]
        public UDT_Descriptivo Concepto3Desc { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje3 { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoNovedad { get; set; }  
    }
}

