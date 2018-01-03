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
    /// Models DTO_PagoImpuesto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_PagoImpuesto 
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_PagoImpuesto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ImpuestoTipoID.Value = dr["ImpuestoTipoID"].ToString();
                this.ImpuestoTipoDesc.Value = dr["ImpuestoTipoDesc"].ToString();
                this.LugarGeoID.Value = dr["LugarGeoID"].ToString();
                this.LugarGeoDesc.Value = dr["LugarGeoDesc"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ValorLocal"].ToString()))
                    this.ValorLocal.Value = Convert.ToDecimal(dr["ValorLocal"]);
                if (!string.IsNullOrWhiteSpace(dr["PeriodoPago"].ToString()))
                    this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                this.Selected.Value = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_PagoImpuesto()
        {
            this.InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new UDT_PeriodoID();
            this.ImpuestoTipoID = new UDT_ImpuestoTipoID();
            this.ImpuestoTipoDesc = new UDT_Descriptivo();
            this.LugarGeoID = new UDT_LugarGeograficoID();
            this.LugarGeoDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_CuentaID();
            this.CuentaDesc = new UDT_Descriptivo();    
            this.ValorLocal = new UDT_Valor();
            this.ValorTotal = new UDT_Valor();
            this.ValorTotalMiles = new UDT_Valor();
            this.ValorTotalDif = new UDT_Valor();
            this.PeriodoPago = new UDTSQL_tinyint();
            this.Selected = new UDT_SiNo();
            this.Detalle = new List<DTO_PagoImpuesto>();
        }
	    #endregion

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_ImpuestoTipoID ImpuestoTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpuestoTipoDesc { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID LugarGeoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeoDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_Valor ValorLocal { get; set; }

        [DataMember]
        public UDT_Valor ValorTotal { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalMiles { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalDif { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoPago { get; set; }

        [DataMember]
        public UDT_SiNo Selected { get; set; }

        [DataMember]
        public List<DTO_PagoImpuesto> Detalle { get; set; }

    }
}
