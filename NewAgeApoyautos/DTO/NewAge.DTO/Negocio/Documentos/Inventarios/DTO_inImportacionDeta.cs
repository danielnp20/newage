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
    /// DTO Tabla DTO_inImportacionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inImportacionDeta
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inImportacionDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsSaldoExistencia.Value = Convert.ToInt32(dr["ConsSaldoExistencia"]);
                this.NumeroDocNotaEnv.Value = Convert.ToInt32(dr["NumeroDocNotaEnv"]);
                this.NumeroDocFactura.Value = Convert.ToInt32(dr["NumeroDocFactura"]);
                this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.ValorUnidadUS.Value = Convert.ToDecimal(dr["ValorUnidadUS"]);
                this.ValorCostoUS.Value = Convert.ToDecimal(dr["ValorCostoUS"]);
                this.ValorFletesUS.Value = Convert.ToDecimal(dr["ValorFletesUS"]);
                this.ValorSeguroUS.Value = Convert.ToDecimal(dr["ValorSeguroUS"]);
                this.ValorOtrosUS.Value = Convert.ToDecimal(dr["ValorOtrosUS"]);
                this.ValorTotalUS.Value = Convert.ToDecimal(dr["ValorTotalUS"]);
                this.ValorTotalPS.Value = Convert.ToDecimal(dr["ValorTotalPS"]);
                this.PosArancelaria.Value = dr["PosArancelaria"].ToString();
                this.PorArancel.Value = Convert.ToDecimal(dr["PorArancel"]);
                this.PorIVA.Value = Convert.ToDecimal(dr["PorIVA"]);
                this.ValorArancel.Value = Convert.ToDecimal(dr["ValorArancel"]);
                this.ValorIVA.Value = Convert.ToDecimal(dr["ValorIVA"]);
                this.ValorAgente.Value = Convert.ToDecimal(dr["ValorAgente"]);
                this.ValorOtrosPS.Value = Convert.ToDecimal(dr["ValorOtrosPS"]);
                this.CostoTotalUS.Value = Convert.ToDecimal(dr["CostoTotalUS"]);
                this.CostoTotalPS.Value = Convert.ToDecimal(dr["CostoTotalPS"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inImportacionDeta()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsSaldoExistencia = new UDT_Consecutivo();          
            this.NumeroDocNotaEnv = new UDT_Consecutivo();
            this.NumeroDocFactura = new UDT_Consecutivo();
            this.Cantidad = new  UDT_Cantidad();
            this.ValorUnidadUS = new UDT_Valor();
            this.ValorCostoUS = new UDT_Valor();
            this.ValorFletesUS = new UDT_Valor();
            this.ValorSeguroUS = new UDT_Valor();
            this.ValorOtrosUS = new UDT_Valor();
            this.ValorTotalUS = new UDT_Valor();
            this.ValorTotalPS = new UDT_Valor();
            this.PosArancelaria = new UDTSQL_char(25);
            this.PorArancel = new UDT_PorcentajeID();
            this.PorIVA = new UDT_PorcentajeID();
            this.ValorArancel = new UDT_Valor();
            this.ValorIVA = new UDT_Valor();
            this.ValorAgente = new UDT_Valor();
            this.ValorOtrosPS = new UDT_Valor();
            this.CostoTotalUS = new UDT_Valor();
            this.CostoTotalPS = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            this.Movimiento = new DTO_glMovimientoDeta();
            this.Detalle = new List<DTO_glMovimientoDeta>();
            this.FacturaNro = new UDT_DescripTExt();
            this.ReferenciaIDP1P2 = new UDT_DescripTBase();
            this.ReferenciaIDP1P2Desc = new UDT_DescripTBase();
        }
        
        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsSaldoExistencia { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocNotaEnv { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocFactura { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }      
        
        [DataMember]
        public UDT_Valor ValorUnidadUS { get; set; }

        [DataMember]
        public UDT_Valor ValorCostoUS { get; set; }

        [DataMember]
        public UDT_Valor ValorFletesUS { get; set; }

        [DataMember]
        public UDT_Valor ValorSeguroUS { get; set; }

        [DataMember]
        public UDT_Valor ValorOtrosUS { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalUS { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalPS { get; set; }

        [DataMember]
        public UDTSQL_char PosArancelaria { get; set; }     
      
        [DataMember]
        public UDT_PorcentajeID PorArancel { get; set; } 
     
        [DataMember]
        public UDT_PorcentajeID PorIVA { get; set; } 
        
        [DataMember]
        public UDT_Valor ValorArancel { get; set; }

        [DataMember]
        public UDT_Valor ValorIVA { get; set; }

        [DataMember]
        public UDT_Valor ValorAgente { get; set; }

        [DataMember]
        public UDT_Valor ValorOtrosPS { get; set; }

        [DataMember]
        public UDT_Valor CostoTotalUS { get; set; }

        [DataMember]
        public UDT_Valor CostoTotalPS  { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Adicionales

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt FacturaNro { get; set; }

        [DataMember]
        public DTO_glMovimientoDeta Movimiento { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_glMovimientoDeta> Detalle { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ReferenciaIDP1P2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ReferenciaIDP1P2Desc { get; set; }

        #endregion
    }
}
