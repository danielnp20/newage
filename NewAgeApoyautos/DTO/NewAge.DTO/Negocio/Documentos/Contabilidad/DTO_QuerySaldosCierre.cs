using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_QuerySaldosCierre
    {

        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QuerySaldosCierre()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.Ano = new UDTSQL_smallint();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.CuentaID = new UDT_CuentaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.SaldoLocalIni = new UDT_Valor();
            this.SaldoExtraIni = new UDT_Valor();
            this.SaldoLocalFinal = new UDT_Valor();
            this.SaldoExtraFinal = new UDT_Valor();
            this.LocalDB = new UDT_Valor();
            this.LocalCR = new UDT_Valor();
            this.ExtraDB = new UDT_Valor();
            this.ExtraCR = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_smallint Ano { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        [AllowNull]
        public string Mes { get; set; }

        [DataMember]
        [AllowNull]
        public int MesNro { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoLocalIni { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoExtraIni { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor LocalDB { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor LocalCR { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ExtraDB { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ExtraCR { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoLocalFinal { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoExtraFinal { get; set; }

        #endregion
    }
}
