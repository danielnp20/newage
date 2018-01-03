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
    public class DTO_SaldosVista
    {

        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SaldosVista(IDataReader dr)
        {
            this.InitCols();

            this.EmpresaID.Value = dr["EmpresaID"].ToString();
            this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
            this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
            this.TerceroID.Value = dr["TerceroID"].ToString();
            this.CuentaID.Value = dr["CuentaID"].ToString();
            this.ProyectoID.Value = dr["ProyectoID"].ToString();
            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
            this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
            this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
            this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
            this.DebitoML.Value = Convert.ToDecimal(dr["DebitoML"].ToString());
            this.DebitoME.Value = Convert.ToDecimal(dr["DebitoME"].ToString());
            this.CreditoML.Value = Convert.ToDecimal(dr["CreditoML"].ToString());
            this.CreditoME.Value = Convert.ToDecimal(dr["CreditoME"].ToString());
            this.SaldoIniML.Value = Convert.ToDecimal(dr["SaldoIniML"].ToString());
            this.SaldoIniME.Value = Convert.ToDecimal(dr["SaldoIniME"].ToString());
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SaldosVista()
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
            this.TerceroID = new UDT_TerceroID();
            this.CuentaID = new UDT_CuentaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.DebitoML = new UDT_Valor();
            this.DebitoME = new UDT_Valor();
            this.CreditoML = new UDT_Valor();
            this.CreditoME = new UDT_Valor();
            this.SaldoIniML = new UDT_Valor();
            this.SaldoIniME = new UDT_Valor();
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
        public UDT_TerceroID TerceroID { get; set; }

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
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }
               
        [DataMember]
        [AllowNull]
        public UDT_Valor DebitoML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor DebitoME { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CreditoML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CreditoME { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoIniML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor SaldoIniME { get; set; }


        #endregion
    }
}
