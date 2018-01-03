using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio.Documentos.Activos
{
    [Serializable]
    [DataContract]
    public class DTO_ActivoSaldo
    {
        #region Constructor

        public DTO_ActivoSaldo(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.IdentificadorTR.Value = Convert.ToInt32(dr["IdentificadorTR"]);
                this.SaldoIniML.Value = Convert.ToDecimal(dr["SaldoIniML"]);
                this.SaldoIniME.Value = Convert.ToDecimal(dr["SaldoIniME"]);
                this.MvtoML.Value = Convert.ToDecimal(dr["MvtoML"]);
                this.MvtoME.Value = Convert.ToDecimal(dr["MvtoME"]);
                this.VidaUtilLOC.Value = Convert.ToInt32(dr["VidaUtilLOC"]);
                this.VidaUtilUSG.Value = Convert.ToInt32(dr["VidaUtilUSG"]);
                this.VidaUtilIFRS.Value = Convert.ToInt32(dr["VidaUtilIFRS"]);
                this.TipoDepreLOC.Value = Convert.ToByte(dr["TipoDepreLOC"]);
                this.TipoDepreUSG.Value = Convert.ToByte(dr["TipoDepreUSG"]);
                this.TipoDepreIFRS.Value = Convert.ToByte(dr["TipoDepreIFRS"]);
                this.ValorSalvamentoLOC.Value = Convert.ToDecimal(dr["ValorSalvamentoLOC"]);
                this.ValorSalvamentoUSG.Value = Convert.ToDecimal(dr["ValorSalvamentoUSG"]);
                this.ValorSalvamentoIFRS.Value = Convert.ToDecimal(dr["ValorSalvamentoIFRS"]);
                this.ValorSalvamentoIFRSUS.Value = Convert.ToDecimal(dr["ValorSalvamentoIFRSUS"]);
                this.ValorRetiroIFRS.Value = Convert.ToDecimal(dr["ValorRetiroIFRS"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Saldos
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.CuentaID = new UDT_CuentaID();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.IdentificadorTR = new UDT_Consecutivo();
            this.SaldoIniML = new UDT_Valor();
            this.SaldoIniME = new UDT_Valor();
            this.MvtoML = new UDT_Valor();
            this.MvtoME = new UDT_Valor();
            this.SaldoCostoML = new UDT_Valor();
            this.SaldoCostoME = new UDT_Valor();
            this.MvtoCostoML = new UDT_Valor();
            this.MvtoCostoME = new UDT_Valor();
            //Activo
            this.VidaUtilLOC = new UDTSQL_int();
            this.VidaUtilUSG = new UDTSQL_int();
            this.VidaUtilIFRS = new UDTSQL_int();
            this.TipoDepreLOC = new UDTSQL_tinyint();
            this.TipoDepreUSG = new UDTSQL_tinyint();
            this.TipoDepreIFRS = new UDTSQL_tinyint();
            this.ValorSalvamentoLOC = new UDT_Valor();
            this.ValorSalvamentoUSG = new UDT_Valor();
            this.ValorSalvamentoIFRS = new UDT_Valor();
            this.ValorSalvamentoIFRSUS = new UDT_Valor();
            this.ValorRetiroIFRS = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        //Saldos
        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTR { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniML { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniME { get; set; }

        [DataMember]
        public UDT_Valor MvtoML { get; set; }

        [DataMember]
        public UDT_Valor MvtoME { get; set; }

        [DataMember]
        public UDT_Valor SaldoCostoML { get; set; }

        [DataMember]
        public UDT_Valor SaldoCostoME { get; set; }

        [DataMember]
        public UDT_Valor MvtoCostoML { get; set; }

        [DataMember]
        public UDT_Valor MvtoCostoME { get; set; }

        //Activo
        [DataMember]
        public UDTSQL_int VidaUtilLOC { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilUSG { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilIFRS { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreLOC { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreUSG { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreIFRS { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoLOC { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoUSG { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoIFRS { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoIFRSUS { get; set; }

        [DataMember]
        public UDT_Valor ValorRetiroIFRS { get; set; }

        #endregion
    }
}
