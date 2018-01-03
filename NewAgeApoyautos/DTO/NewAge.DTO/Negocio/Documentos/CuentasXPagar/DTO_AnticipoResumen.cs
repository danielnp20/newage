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
    /// Models DTO_AnticiposResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_AnticiposResumen
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_AnticiposResumen(IDataReader dr)
        {
            this.InitCols();
            this.Seleccionar.Value = false;
            try
            {
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.PrefijoID.Value = Convert.ToString(dr["PrefijoID"]);
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.MonedaID.Value = Convert.ToString(dr["MonedaID"]);
                this.OrigenMonetario.Value = Convert.ToByte(dr["OrigenMonetario"]);
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                //this.TarjetaCreditoID.Value = dr["TarjetaCreditoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocumentoTercero"].ToString()))
                    this.DocumentoTercero.Value = Convert.ToString(dr["DocumentoTercero"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTR"].ToString()))
                    this.IdentificadorTR.Value = Convert.ToInt32(dr["IdentificadorTR"]);
                this.ML.Value = Convert.ToDecimal(dr["ML"]);
                this.ME.Value = Convert.ToDecimal(dr["ME"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_AnticiposResumen()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.DocumentoID = new UDT_DocumentoID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.MonedaID = new UDT_MonedaID();
            this.OrigenMonetario = new UDTSQL_tinyint();
            this.CuentaID = new UDT_CuentaID();
            this.TerceroID = new UDT_TerceroID();
            this.TarjetaCreditoID = new UDT_TarjetaCreditoID();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoSaldoID = new UDT_ConceptoSaldoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.IdentificadorTR = new UDT_Consecutivo();
            this.ML = new UDT_Valor();
            this.ME = new UDT_Valor();
            this.MaxVal = new UDT_Valor();
        }
        
        #endregion

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint OrigenMonetario { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TarjetaCreditoID TarjetaCreditoID { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

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
        public UDT_Consecutivo IdentificadorTR { get; set; }

        [DataMember]
        public UDT_Valor ML { get; set; }

        [DataMember]
        public UDT_Valor ME { get; set; }

        [DataMember]
        public UDT_Valor MaxVal { get; set; }
    }
}
