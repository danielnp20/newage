using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Saldos
    /// </summary>
    public class DTO_ReportGenericDetalle 
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportGenericDetalle(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["ConsLineaREP"].ToString()))
                    this.ConsLineaREP.Value = Convert.ToInt32(dr["ConsLineaREP"]);
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroDesc"].ToString()))
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoDesc"].ToString()))
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroCostoDesc"].ToString()))
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoDesc"].ToString()))
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["ConceptoCargoID"].ToString()))
                    this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                if (!string.IsNullOrEmpty(dr["ConceptoCargoDesc"].ToString()))
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["OperacionID"].ToString()))
                    this.OperacionID.Value = dr["OperacionID"].ToString();
                if (!string.IsNullOrEmpty(dr["OperacionDesc"].ToString()))
                    this.OperacionDesc.Value = dr["OperacionDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["ActividadID"].ToString()))
                    this.ActividadID.Value = dr["ActividadID"].ToString();
                if (!string.IsNullOrEmpty(dr["ActividadDesc"].ToString()))
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoIniActML"].ToString()))
                    this.SaldoIniActML.Value = Convert.ToDecimal(dr["SaldoIniActML"]);
                if (!string.IsNullOrEmpty(dr["SaldoIniActME"].ToString()))
                    this.SaldoIniActME.Value = Convert.ToDecimal(dr["SaldoIniActME"]);
                if (!string.IsNullOrEmpty(dr["DebitoActML"].ToString()))
                    this.DebitoActML.Value = Convert.ToDecimal(dr["DebitoActML"]);
                if (!string.IsNullOrEmpty(dr["CreditoActML"].ToString()))
                    this.CreditoActML.Value = Convert.ToDecimal(dr["CreditoActML"]);
                if (!string.IsNullOrEmpty(dr["DebitoActME"].ToString()))
                    this.DebitoActME.Value = Convert.ToDecimal(dr["DebitoActME"]);
                if (!string.IsNullOrEmpty(dr["CreditoActME"].ToString()))
                    this.CreditoActME.Value = Convert.ToDecimal(dr["CreditoActME"]);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportGenericDetalle(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {

            }
            catch (Exception e)
            { ; }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportGenericDetalle()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConsLineaREP = new UDT_Consecutivo();
            this.TerceroID= new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.ProyectoID=new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.CentroCostoID=new UDT_CentroCostoID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID=new UDT_LineaPresupuestoID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.ConceptoCargoID=new UDT_ConceptoCargoID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.OperacionID=new UDT_OperacionID();
            this.OperacionDesc = new UDT_Descriptivo();
            this.ActividadID=new UDT_OperacionID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.SaldoIniActML=new UDT_Valor();
            this.SaldoIniActME=new UDT_Valor();
            this.DebitoActML=new UDT_Valor();
            this.CreditoActML=new UDT_Valor();
            this.DebitoActME=new UDT_Valor();
            this.CreditoActME = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo ConsLineaREP { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_OperacionID OperacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo OperacionDesc { get; set; }

        [DataMember]
        public UDT_OperacionID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniActML { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniActME { get; set; }

        [DataMember]
        public UDT_Valor DebitoActML { get; set; }

        [DataMember]
        public UDT_Valor CreditoActML { get; set; }

        [DataMember]
        public UDT_Valor DebitoActME { get; set; }

        [DataMember]
        public UDT_Valor CreditoActME { get; set; }

        #endregion

    }
}
