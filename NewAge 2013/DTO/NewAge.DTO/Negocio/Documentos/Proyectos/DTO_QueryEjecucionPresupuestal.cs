using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    #region Documentos
    /// <summary>
    /// Class Models DTO_QueryEjecucionPresupuestal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryEjecucionPresupuestal
    {
        #region DTO_QueryEjecucionPresupuestal

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryEjecucionPresupuestal(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Proyecto.Value = dr["Proyecto"].ToString();
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["IngPresupuesto"].ToString()))
                    this.IngPresupuesto.Value = Convert.ToDecimal(dr["IngPresupuesto"]);
                //if (!string.IsNullOrEmpty(dr["IngPresAjustado"].ToString()))
                //    this.IngPresAjustado.Value = Convert.ToDecimal(dr["IngPresAjustado"]);
                if (!string.IsNullOrEmpty(dr["IngEjecucion"].ToString()))
                    this.IngEjecucion.Value = Convert.ToDecimal(dr["IngEjecucion"]);
                if (!string.IsNullOrEmpty(dr["IngxEjecutar"].ToString()))
                    this.IngxEjecutar.Value = Convert.ToDecimal(dr["IngxEjecutar"]);
                if (!string.IsNullOrEmpty(dr["CostoPresupuesto"].ToString()))
                    this.CostoPresupuesto.Value = Convert.ToDecimal(dr["CostoPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["CostoPresAjustado"].ToString()))
                    this.CostoPresAjustado.Value = Convert.ToDecimal(dr["CostoPresAjustado"]);
                if (!string.IsNullOrEmpty(dr["CostoEjecucion"].ToString()))
                    this.CostoEjecucion.Value = Convert.ToDecimal(dr["CostoEjecucion"]);
                if (!string.IsNullOrEmpty(dr["CostoxEjecutar"].ToString()))
                    this.CostoxEjecutar.Value = Convert.ToDecimal(dr["CostoxEjecutar"]);
                if (!string.IsNullOrEmpty(dr["RentaPresupuesto"].ToString()))
                    this.RentaPresupuesto.Value = Convert.ToDecimal(dr["RentaPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["RentaPresAjustado"].ToString()))
                    this.RentaPresAjustado.Value = Convert.ToDecimal(dr["RentaPresAjustado"]);
                if (!string.IsNullOrEmpty(dr["RentaEjecucion"].ToString()))
                    this.RentaEjecucion.Value = Convert.ToDecimal(dr["RentaEjecucion"]);
                if (!string.IsNullOrEmpty(dr["FactINGxEjecutar"].ToString()))
                    this.FactINGxEjecutar.Value = Convert.ToDecimal(dr["FactINGxEjecutar"]);
                if (!string.IsNullOrEmpty(dr["FactCTOxEjecutar"].ToString()))
                    this.FactCTOxEjecutar.Value = Convert.ToDecimal(dr["FactCTOxEjecutar"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryEjecucionPresupuestal()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Proyecto = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.IngPresupuesto = new UDTSQL_decimal();
            //this.IngPresAjustado = new UDTSQL_decimal();
            this.IngEjecucion = new UDTSQL_decimal();
            this.IngxEjecutar = new UDTSQL_decimal();
            this.CostoPresupuesto = new UDTSQL_decimal();
            this.CostoPresAjustado = new UDTSQL_decimal();
            this.CostoEjecucion = new UDTSQL_decimal();
            this.CostoxEjecutar = new UDTSQL_decimal();
            this.RentaPresupuesto = new UDTSQL_decimal();
            this.RentaPresAjustado = new UDTSQL_decimal();
            this.RentaEjecucion = new UDTSQL_decimal();
            this.FactINGxEjecutar = new UDTSQL_decimal();
            this.FactCTOxEjecutar = new UDTSQL_decimal();
            this.Detalle = new List<DTO_QueryEjecucionPresupuestalDetalle>();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_ProyectoID Proyecto { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDTSQL_decimal IngPresupuesto { get; set; }

        //[DataMember]
        //public UDTSQL_decimal IngPresAjustado { get; set; }

        [DataMember]
        public UDTSQL_decimal IngEjecucion { get; set; }

        [DataMember]
        public UDTSQL_decimal IngxEjecutar { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoPresupuesto { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoPresAjustado { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoEjecucion { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoxEjecutar { get; set; }

        [DataMember]
        public UDTSQL_decimal RentaPresupuesto { get; set; }

        [DataMember]
        public UDTSQL_decimal RentaPresAjustado { get; set; }

        [DataMember]
        public UDTSQL_decimal RentaEjecucion { get; set; }

        [DataMember]
        public UDTSQL_decimal FactINGxEjecutar { get; set; }

        [DataMember]
        public UDTSQL_decimal FactCTOxEjecutar { get; set; }

        [DataMember]
        public List<DTO_QueryEjecucionPresupuestalDetalle> Detalle { get; set; }
        #endregion
    }
    #endregion


    #region EjecucionPresupuestal Detalle
    /// <summary>
    /// Class Models DTO_QueryEjecucionPresupuestalDetalle
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryEjecucionPresupuestalDetalle
    {
        public DTO_QueryEjecucionPresupuestalDetalle(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.Proyecto.Value = dr["Proyecto"].ToString();
                this.LineaPRE.Value = dr["LineaPRE"].ToString();
                this.LineaDesc.Value = dr["LineaDesc"].ToString();

                if (!string.IsNullOrEmpty(dr["IngPresupuesto"].ToString()))
                    this.IngPresupuesto.Value = Convert.ToDecimal(dr["IngPresupuesto"]);
                //if (!string.IsNullOrEmpty(dr["IngPresAjustado"].ToString()))
                //    this.IngPresAjustado.Value = Convert.ToDecimal(dr["IngPresAjustado"]);
                if (!string.IsNullOrEmpty(dr["IngEjecucion"].ToString()))
                    this.IngEjecucion.Value = Convert.ToDecimal(dr["IngEjecucion"]);
                if (!string.IsNullOrEmpty(dr["IngxEjecutar"].ToString()))
                    this.IngxEjecutar.Value = Convert.ToDecimal(dr["IngxEjecutar"]);
                if (!string.IsNullOrEmpty(dr["CostoPresupuesto"].ToString()))
                    this.CostoPresupuesto.Value = Convert.ToDecimal(dr["CostoPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["CostoPresAjustado"].ToString()))
                    this.CostoPresAjustado.Value = Convert.ToDecimal(dr["CostoPresAjustado"]);
                if (!string.IsNullOrEmpty(dr["CostoEjecucion"].ToString()))
                    this.CostoEjecucion.Value = Convert.ToDecimal(dr["CostoEjecucion"]);
                if (!string.IsNullOrEmpty(dr["CostoxEjecutar"].ToString()))
                    this.CostoxEjecutar.Value = Convert.ToDecimal(dr["CostoxEjecutar"]);
                if (!string.IsNullOrEmpty(dr["RentaPresupuesto"].ToString()))
                    this.RentaPresupuesto.Value = Convert.ToDecimal(dr["RentaPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["RentaPresAjustado"].ToString()))
                    this.RentaPresAjustado.Value = Convert.ToDecimal(dr["RentaPresAjustado"]);
                if (!string.IsNullOrEmpty(dr["RentaEjecucion"].ToString()))
                    this.RentaEjecucion.Value = Convert.ToDecimal(dr["RentaEjecucion"]);
                if (!string.IsNullOrEmpty(dr["FactINGxEjecutar"].ToString()))
                    this.FactINGxEjecutar.Value = Convert.ToDecimal(dr["FactINGxEjecutar"]);
                if (!string.IsNullOrEmpty(dr["FactCTOxEjecutar"].ToString()))
                    this.FactCTOxEjecutar.Value = Convert.ToDecimal(dr["FactCTOxEjecutar"]);

                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_QueryEjecucionPresupuestalDetalle()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.Proyecto = new UDT_ProyectoID();
            this.LineaPRE = new UDT_LineaPresupuestoID();
            this.LineaDesc = new UDT_Descriptivo();
            this.IngPresupuesto = new UDTSQL_decimal();
            //this.IngPresAjustado = new UDTSQL_decimal();
            this.IngEjecucion = new UDTSQL_decimal();
            this.IngxEjecutar = new UDTSQL_decimal();
            this.CostoPresupuesto = new UDTSQL_decimal();
            this.CostoPresAjustado = new UDTSQL_decimal();
            this.CostoEjecucion = new UDTSQL_decimal();
            this.CostoxEjecutar = new UDTSQL_decimal();
            this.RentaPresupuesto = new UDTSQL_decimal();
            this.RentaPresAjustado = new UDTSQL_decimal();
            this.RentaEjecucion = new UDTSQL_decimal();
            this.FactINGxEjecutar = new UDTSQL_decimal();
            this.FactCTOxEjecutar = new UDTSQL_decimal();
            this.Documento = "";

        }

        #region Propiedades

        [DataMember]
        public UDT_ProyectoID Proyecto { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPRE { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaDesc { get; set; }


        [DataMember]
        public UDTSQL_decimal IngPresupuesto { get; set; }

        //[DataMember]
        //public UDTSQL_decimal IngPresAjustado { get; set; }

        [DataMember]
        public UDTSQL_decimal IngEjecucion { get; set; }

        [DataMember]
        public UDTSQL_decimal IngxEjecutar { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoPresupuesto { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoPresAjustado { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoEjecucion { get; set; }

        [DataMember]
        public UDTSQL_decimal CostoxEjecutar { get; set; }

        [DataMember]
        public UDTSQL_decimal RentaPresupuesto { get; set; }

        [DataMember]
        public UDTSQL_decimal RentaPresAjustado { get; set; }

        [DataMember]
        public UDTSQL_decimal RentaEjecucion { get; set; }

        [DataMember]
        public UDTSQL_decimal FactINGxEjecutar { get; set; }

        [DataMember]
        public UDTSQL_decimal FactCTOxEjecutar { get; set; }

        [DataMember]
        public string Documento { get; set; }

        #endregion
    }
    #endregion


}
