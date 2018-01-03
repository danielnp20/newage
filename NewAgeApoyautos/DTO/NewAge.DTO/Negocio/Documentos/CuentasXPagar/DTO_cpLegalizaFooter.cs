using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_cpLegalizaDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpLegalizaFooter
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpLegalizaFooter(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = (dr["EmpresaID"]).ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Item.Value = Convert.ToByte(dr["Item"]);
                this.MonedaID.Value = (dr["MonedaID"]).ToString();
                this.CargoEspecialID.Value = (dr["CargoEspecialID"]).ToString();
                this.Factura.Value =(dr["Factura"]).ToString();
                this.FactEquivalente.Value = (dr["FactEquivalente"]).ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.TasaCambioDOCU.Value = Convert.ToDecimal(dr["TasaCambioDOCU"]);
                this.TasaCambioCONT.Value = Convert.ToDecimal(dr["TasaCambioCONT"]);
                this.TerceroID.Value = (dr["TerceroID"]).ToString();
                this.NuevoTerceroInd.Value = Convert.ToBoolean(dr["NuevoTerceroInd"]);
                this.Nombre.Value = (dr["Nombre"]).ToString();
                this.Descriptivo.Value = (dr["Descriptivo"]).ToString();
                this.ProyectoID.Value = (dr["ProyectoID"]).ToString();
                this.CentroCostoID.Value = (dr["CentroCostoID"]).ToString();
                this.LugarGeograficoID.Value = (dr["LugarGeograficoID"]).ToString();
                this.ValorBruto.Value = Convert.ToDecimal(dr["ValorBruto"]);
                this.ValorNeto.Value = Convert.ToDecimal(dr["ValorNeto"]);
                if (!string.IsNullOrWhiteSpace(dr["PorIVA1"].ToString()))
                    this.PorIVA1.Value = Convert.ToDecimal(dr["PorIVA1"]); 
                if (!string.IsNullOrWhiteSpace(dr["BaseIVA1"].ToString()))
                    this.BaseIVA1.Value = Convert.ToDecimal(dr["BaseIVA1"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorIVA1"].ToString()))
                    this.ValorIVA1.Value = Convert.ToDecimal(dr["ValorIVA1"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorIVA2"].ToString()))
                    this.PorIVA2.Value = Convert.ToDecimal(dr["PorIVA2"]); 
                if (!string.IsNullOrWhiteSpace(dr["BaseIVA2"].ToString()))
                    this.BaseIVA2.Value = Convert.ToDecimal(dr["BaseIVA2"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorIVA2"].ToString()))
                    this.ValorIVA2.Value = Convert.ToDecimal(dr["ValorIVA2"]); 
                if (!string.IsNullOrWhiteSpace(dr["RteIVA1AsumidoInd"].ToString()))
                    try { this.RteIVA1AsumidoInd.Value = Convert.ToBoolean(dr["RteIVA1AsumidoInd"]); } catch (Exception e) { this.RteIVA1AsumidoInd.Value = false; }
                if (!string.IsNullOrWhiteSpace(dr["RteIVA2AsumidoInd"].ToString()))
                    this.RteIVA2AsumidoInd.Value = Convert.ToBoolean(dr["RteIVA2AsumidoInd"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorRteIVA1"].ToString()))
                    this.PorRteIVA1.Value = Convert.ToDecimal(dr["PorRteIVA1"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorRteIVA2"].ToString()))
                    this.PorRteIVA2.Value = Convert.ToDecimal(dr["PorRteIVA2"]); 
                if (!string.IsNullOrWhiteSpace(dr["BaseRteIVA1"].ToString()))
                    this.BaseRteIVA1.Value = Convert.ToDecimal(dr["BaseRteIVA1"]); 
                if (!string.IsNullOrWhiteSpace(dr["BaseRteIVA2"].ToString()))
                    this.BaseRteIVA2.Value = Convert.ToDecimal(dr["BaseRteIVA2"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorRteIVA1"].ToString()))
                    this.ValorRteIVA1.Value = Convert.ToDecimal(dr["ValorRteIVA1"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorRteIVA2"].ToString()))
                    this.ValorRteIVA2.Value = Convert.ToDecimal(dr["ValorRteIVA2"]); 
                if (!string.IsNullOrWhiteSpace(dr["RteFteAsumidoInd"].ToString()))
                    this.RteFteAsumidoInd.Value = Convert.ToBoolean(dr["RteFteAsumidoInd"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorRteFuente"].ToString()))
                    this.PorRteFuente.Value = Convert.ToDecimal(dr["PorRteFuente"]); 
                if (!string.IsNullOrWhiteSpace(dr["BaseRteFuente"].ToString()))
                    this.BaseRteFuente.Value = Convert.ToDecimal(dr["BaseRteFuente"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorRteFuente"].ToString()))
                    this.ValorRteFuente.Value = Convert.ToDecimal(dr["ValorRteFuente"]); 
                if (!string.IsNullOrWhiteSpace(dr["RteICAAsumidoInd"].ToString()))
                    this.RteICAAsumidoInd.Value = Convert.ToBoolean(dr["RteICAAsumidoInd"]); 
                if (!string.IsNullOrWhiteSpace(dr["PorRteICA"].ToString()))
                    this.PorRteICA.Value = Convert.ToDecimal(dr["PorRteICA"]); 
                if (!string.IsNullOrWhiteSpace(dr["BaseRteICA"].ToString()))
                    this.BaseRteICA.Value = Convert.ToDecimal(dr["BaseRteICA"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorRteICA"].ToString()))
                    this.ValorRteICA.Value = Convert.ToDecimal(dr["ValorRteICA"]);
                if (!string.IsNullOrWhiteSpace(dr["PorImpConsumo"].ToString()))
                    this.PorImpConsumo.Value = Convert.ToDecimal(dr["PorImpConsumo"]);
                if (!string.IsNullOrWhiteSpace(dr["BaseImpConsumo"].ToString()))
                    this.BaseImpConsumo.Value = Convert.ToDecimal(dr["BaseImpConsumo"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorImpConsumo"].ToString()))
                    this.ValorImpConsumo.Value = Convert.ToDecimal(dr["ValorImpConsumo"]); 
            }
            catch (Exception e)
            {                 
                throw e; 
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpLegalizaFooter()
        {
            InitCols();
            this.MontoMinimo.Value = 0;
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Item = new UDTSQL_int();
            this.MonedaID = new UDT_MonedaID();
            this.CargoEspecialID = new UDT_CargoEspecialID();
            this.Factura = new UDTSQL_char(20);
            this.FactEquivalente = new UDTSQL_char(20);
            this.Fecha = new UDTSQL_smalldatetime();
            this.TasaCambioDOCU = new UDT_TasaID();
            this.TasaCambioCONT = new UDT_TasaID();
            this.TerceroID = new UDT_TerceroID();
            this.NuevoTerceroInd = new UDT_SiNo();
            this.Nombre = new UDT_DescripTBase();
            this.Descriptivo = new UDT_DescripTBase();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LugarGeograficoID = new UDT_LugarGeograficoID();
            this.ValorBruto = new UDT_Valor();
            this.ValorNeto = new UDT_Valor();
            this.PorIVA1 = new UDT_PorcentajeID();
            this.BaseIVA1 = new UDT_Valor();
            this.ValorIVA1 = new UDT_Valor();
            this.PorIVA2 = new UDT_PorcentajeID();
            this.BaseIVA2 = new UDT_Valor();
            this.ValorIVA2 = new UDT_Valor();
            this.RteIVA1AsumidoInd = new UDT_SiNo();
            this.RteIVA2AsumidoInd = new UDT_SiNo();
            this.PorRteIVA1 = new UDT_PorcentajeID();
            this.PorRteIVA2 = new UDT_PorcentajeID();
            this.BaseRteIVA1 = new UDT_Valor();
            this.BaseRteIVA2 = new UDT_Valor();
            this.ValorRteIVA1 = new UDT_Valor();
            this.ValorRteIVA2 = new UDT_Valor();
            this.RteFteAsumidoInd = new UDT_SiNo();
            this.PorRteFuente = new UDT_PorcentajeID();
            this.BaseRteFuente = new UDT_Valor();
            this.ValorRteFuente = new UDT_Valor();
            this.RteICAAsumidoInd = new UDT_SiNo();
            this.PorRteICA = new UDT_PorcentajeID();
            this.BaseRteICA = new UDT_Valor();
            this.ValorRteICA = new UDT_Valor();
            this.PorImpConsumo = new UDT_PorcentajeID();
            this.BaseImpConsumo = new UDT_Valor();
            this.ValorImpConsumo = new UDT_Valor();
            //Adicionales
            this.SumIVA = new UDT_Valor();
            this.SumRete = new UDT_Valor();
            this.ImpRFteDesc = new UDT_DescripTBase();
            this.ImpRIVA1Desc = new UDT_DescripTBase();
            this.ImpRIVA2Desc = new UDT_DescripTBase();
            this.ImpRICADesc = new UDT_DescripTBase();
            this.ImpConsumoDesc = new UDT_DescripTBase();
            this.MontoMinimo = new UDT_Valor();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.ProyectoDesc = new UDT_Descriptivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_int Item { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_CargoEspecialID CargoEspecialID { get; set; }

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char FactEquivalente { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_TasaID TasaCambioDOCU { get; set; }

        [DataMember]
        public UDT_TasaID TasaCambioCONT { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_TerceroID TerceroID { get; set; }
     
        [DataMember]
        public UDT_SiNo NuevoTerceroInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Valor ValorBruto { get; set; }

        [DataMember]
        public UDT_Valor ValorNeto { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorIVA1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseIVA1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorIVA1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorIVA2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseIVA2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorIVA2 { get; set; }

        [DataMember]
        public UDT_SiNo RteIVA1AsumidoInd { get; set; }

        [DataMember]
        public UDT_SiNo RteIVA2AsumidoInd { get; set; }
        
        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorRteIVA1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorRteIVA2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseRteIVA1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseRteIVA2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorRteIVA1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorRteIVA2 { get; set; }

        [DataMember]
        public UDT_SiNo RteFteAsumidoInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorRteFuente { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseRteFuente { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorRteFuente { get; set; }

        [DataMember]
        public UDT_SiNo RteICAAsumidoInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorRteICA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseRteICA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorRteICA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorImpConsumo  { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor BaseImpConsumo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorImpConsumo { get; set; }


        //Adicionales
        [DataMember]
        [AllowNull]
        public UDT_Valor SumIVA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SumRete { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ImpRFteDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ImpRIVA1Desc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ImpRIVA2Desc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ImpRICADesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ImpConsumoDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor MontoMinimo { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        #endregion
    }
}
