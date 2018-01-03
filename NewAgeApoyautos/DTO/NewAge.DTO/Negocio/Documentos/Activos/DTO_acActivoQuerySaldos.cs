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
    public class DTO_acActivoQuerySaldos
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acActivoQuerySaldos()
        {
            InitCols();
        }

        /// <summary>
        /// Construye el dto a partir de una consula hecha en la bd
        /// </summary>
        /// <param name="dr">DataReader</param>
        public DTO_acActivoQuerySaldos(IDataReader dr)
        {
            InitCols();
            try
            {

                if (!string.IsNullOrWhiteSpace(dr["VlrMtoML"].ToString()))
                    this.VlrMtoML.Value = Convert.ToDecimal(dr["VlrMtoML"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMtoME"].ToString()))
                    this.VlrMtoME.Value = Convert.ToDecimal(dr["VlrMtoME"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoIniML"].ToString()))
                    this.SaldoIniML.Value = Convert.ToDecimal(dr["SaldoIniML"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoIniME"].ToString()))
                    this.SaldoIniME.Value = Convert.ToDecimal(dr["SaldoIniME"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoActualML"].ToString()))
                    this.SaldoActualML.Value = Convert.ToDecimal(dr["SaldoActualML"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoActualME"].ToString()))
                    this.SaldoActualME.Value = Convert.ToDecimal(dr["SaldoActualME"]);
                if (!string.IsNullOrWhiteSpace(dr["ConceptoSaldoID"].ToString()))
                    this.ConceptoSaldoID.Value = Convert.ToString(dr["ConceptoSaldoID"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTR"].ToString()))
                    this.IdentificadorTR.Value = Convert.ToInt32(dr["IdentificadorTR"]);
                if (!string.IsNullOrWhiteSpace(dr["BalanceTipoId"].ToString()))
                    this.BalanceTipoId.Value = Convert.ToString(dr["BalanceTipoId"]);
                if (!string.IsNullOrWhiteSpace(dr["Componente"].ToString()))
                    this.Componente.Value = Convert.ToString(dr["Componente"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void InitCols()
        {
            this.VlrMtoML = new UDT_Valor();
            this.VlrMtoME = new UDT_Valor();
            this.SaldoIniML = new UDT_Valor();
            this.SaldoIniME = new UDT_Valor();
            this.SaldoActualME = new UDT_Valor();
            this.SaldoActualML = new UDT_Valor();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.IdentificadorTR = new UDT_Consecutivo();
            this.BalanceTipoId = new UDT_BalanceTipoID();
            this.Componente = new UDT_Descriptivo();
        }

        #region Propiedades

        [DataMember]
        public UDT_Valor VlrMtoML { get; set; }

        [DataMember]
        public UDT_Valor VlrMtoME { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniML { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniME { get; set; }

        [DataMember]
        public UDT_Valor SaldoActualML { get; set; }

        [DataMember]
        public UDT_Valor SaldoActualME { get; set; }

        [DataMember]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTR { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoId { get; set; }

        [DataMember]
        public UDT_Descriptivo Componente { get; set; }

        #endregion
    }
}
