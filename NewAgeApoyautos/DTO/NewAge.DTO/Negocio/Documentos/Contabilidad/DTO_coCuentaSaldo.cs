using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_coCuentaSaldo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coCuentaSaldo
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coCuentaSaldo(IDataReader dr)
        {
            try
            {
                this.InitCols();
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.CuentaAlternaID.Value = dr["CuentaAlternaID"].ToString();
                this.IdentificadorTR.Value = Convert.ToInt64(dr["IdentificadorTR"]);
                this.vlBaseML.Value = Convert.ToDecimal(dr["vlBaseML"]);
                this.vlBaseME.Value = Convert.ToDecimal(dr["vlBaseME"]);
                this.DbOrigenLocML.Value = Convert.ToDecimal(dr["DbOrigenLocML"]);
                this.DbOrigenExtML.Value = Convert.ToDecimal(dr["DbOrigenExtML"]);
                this.DbOrigenLocME.Value = Convert.ToDecimal(dr["DbOrigenLocME"]);
                this.DbOrigenExtME.Value = Convert.ToDecimal(dr["DbOrigenExtME"]);
                this.CrOrigenLocML.Value = Convert.ToDecimal(dr["CrOrigenLocML"]);
                this.CrOrigenExtML.Value = Convert.ToDecimal(dr["CrOrigenExtML"]);
                this.CrOrigenLocME.Value = Convert.ToDecimal(dr["CrOrigenLocME"]);
                this.CrOrigenExtME.Value = Convert.ToDecimal(dr["CrOrigenExtME"]);
                this.DbSaldoIniLocML.Value = Convert.ToDecimal(dr["DbSaldoIniLocML"]);
                this.DbSaldoIniExtML.Value = Convert.ToDecimal(dr["DbSaldoIniExtML"]);
                this.DbSaldoIniLocME.Value = Convert.ToDecimal(dr["DbSaldoIniLocME"]);
                this.DbSaldoIniExtME.Value = Convert.ToDecimal(dr["DbSaldoIniExtME"]);
                this.CrSaldoIniLocML.Value = Convert.ToDecimal(dr["CrSaldoIniLocML"]);
                this.CrSaldoIniExtML.Value = Convert.ToDecimal(dr["CrSaldoIniExtML"]);
                this.CrSaldoIniLocME.Value = Convert.ToDecimal(dr["CrSaldoIniLocME"]);
                this.CrSaldoIniExtME.Value = Convert.ToDecimal(dr["CrSaldoIniExtME"]);
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
        public DTO_coCuentaSaldo()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.PeriodoID = new UDT_PeriodoID();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.CuentaID = new UDT_CuentaID();
            this.TerceroID = new UDT_TerceroID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.CuentaAlternaID = new UDT_CuentaAlternaID();
            this.IdentificadorTR = new UDT_IdentificadorTR();
            this.vlBaseML = new UDT_Valor();
            this.vlBaseME = new UDT_Valor();
            this.DbOrigenLocML = new UDT_Valor();
            this.DbOrigenExtML = new UDT_Valor();
            this.DbOrigenLocME = new UDT_Valor();
            this.DbOrigenExtME = new UDT_Valor();
            this.CrOrigenLocML = new UDT_Valor();
            this.CrOrigenExtML = new UDT_Valor();
            this.CrOrigenLocME = new UDT_Valor();
            this.CrOrigenExtME = new UDT_Valor();
            this.DbSaldoIniLocML = new UDT_Valor();
            this.DbSaldoIniExtML = new UDT_Valor();
            this.DbSaldoIniLocME = new UDT_Valor();
            this.DbSaldoIniExtME = new UDT_Valor();
            this.CrSaldoIniLocML = new UDT_Valor();
            this.CrSaldoIniExtML = new UDT_Valor();
            this.CrSaldoIniLocME = new UDT_Valor();
            this.CrSaldoIniExtME = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            //Campos adicionales
            this.Marca = new UDT_SiNo();
            this.PrefDoc = new UDT_DescripTBase();
            this.SaldoTotalML = new UDT_Valor();
            this.SaldoTotalME = new UDT_Valor();
            this.PrefijoID = new UDT_PrefijoID();
            this.LugarGeograficoID = new UDT_LugarGeograficoID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_CuentaAlternaID CuentaAlternaID { get; set; }

        [DataMember]
        public UDT_IdentificadorTR IdentificadorTR { get; set; }

        [DataMember]
        public UDT_Valor vlBaseML { get; set; }

        [DataMember]
        public UDT_Valor vlBaseME { get; set; }

        [DataMember]
        public UDT_Valor DbOrigenLocML { get; set; }

        [DataMember]
        public UDT_Valor DbOrigenExtML { get; set; }

        [DataMember]
        public UDT_Valor DbOrigenLocME { get; set; }

        [DataMember]
        public UDT_Valor DbOrigenExtME { get; set; }

        [DataMember]
        public UDT_Valor CrOrigenLocML { get; set; }

        [DataMember]
        public UDT_Valor CrOrigenExtML { get; set; }

        [DataMember]
        public UDT_Valor CrOrigenLocME { get; set; }

        [DataMember]
        public UDT_Valor CrOrigenExtME { get; set; }

        [DataMember]
        public UDT_Valor DbSaldoIniLocML { get; set; }

        [DataMember]
        public UDT_Valor DbSaldoIniExtML { get; set; }

        [DataMember]
        public UDT_Valor DbSaldoIniLocME { get; set; }

        [DataMember]
        public UDT_Valor DbSaldoIniExtME { get; set; }

        [DataMember]
        public UDT_Valor CrSaldoIniLocML { get; set; }

        [DataMember]
        public UDT_Valor CrSaldoIniExtML { get; set; }

        [DataMember]
        public UDT_Valor CrSaldoIniLocME { get; set; }

        [DataMember]
        public UDT_Valor CrSaldoIniExtME { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Campos Adicionales

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_SiNo Marca { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor SaldoTotalML { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor SaldoTotalME { get; set; }
        #endregion
    }
}
