using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_QueryEstadoEjecucion
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryEstadoEjecucion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_QueryEstadoEjecucion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.RecursoID.Value = dr["RecursoID"].ToString();
                this.ActividadID.Value = dr["ActividadID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.Campo.Value = dr["Campo"].ToString();
                this.Pozo.Value = dr["Pozo"].ToString();
                this.Disponible.Value = Convert.ToDecimal(dr["Disponible"]);
                this.Presupuesto.Value = Convert.ToDecimal(dr["Presupuesto"]);
                this.Compromisos.Value = Convert.ToDecimal(dr["Compromisos"]);
                this.Recibidos.Value = Convert.ToDecimal(dr["Recibidos"]);
                this.Ejecutado.Value = Convert.ToDecimal(dr["Ejecutado"]);
                this.EjeVsPre.Value = Convert.ToDecimal(dr["EjeVsPre"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_QueryEstadoEjecucion(bool InitValues)
        {
            InitCols();
            try
            {
                this.Disponible.Value = 0;
                this.Presupuesto.Value = 0;
                this.Compromisos.Value = 0;
                this.Recibidos.Value = 0;
                this.Ejecutado.Value = 0;
                this.EjeVsPre.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.PeriodoID= new UDT_PeriodoID();
            this.MonedaID = new UDT_MonedaID();
            this.ProyectoID= new UDT_ProyectoID();
            this.CentroCostoID= new UDT_CentroCostoID();
            this.RecursoID = new UDT_RecursoID();
            this.ActividadID = new UDT_ActividadID();
            this.LineaPresupuestoID= new UDT_LineaPresupuestoID();
            this.ContratoID = new UDT_ContratoID();
            this.Campo = new UDT_AreaFisicaID();
            this.Pozo = new UDT_LugarGeograficoID();
            this.Grupo = new UDT_ActividadID();
            this.Descriptivo = new UDT_Descriptivo();
            this.Disponible = new UDT_Valor();
            this.Presupuesto = new UDT_Valor();
            this.Compromisos = new UDT_Valor();
            this.Recibidos = new UDT_Valor();
            this.Ejecutado = new UDT_Valor();
            this.EjeVsPre = new UDT_Valor();
            this.DetalleNivel1 = new List<DTO_QueryEstadoEjecucion>();
            this.DetalleNivel2 = new List<DTO_QueryEstadoEjecucion>();
        }  

        #region Propiedades

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_RecursoID RecursoID { get; set; }

        [DataMember]
        public UDT_ActividadID ActividadID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ContratoID ContratoID { get; set; }

        [DataMember]
        public UDT_AreaFisicaID Campo { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Pozo { get; set; }

        [DataMember]
        public UDT_ActividadID Grupo { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor Disponible { get; set; }

        [DataMember]
        public UDT_Valor Presupuesto { get; set; }

        [DataMember]
        public UDT_Valor Compromisos { get; set; }

        [DataMember]
        public UDT_Valor Recibidos { get; set; }

        [DataMember]
        public UDT_Valor Ejecutado { get; set; }

        [DataMember]
        public UDT_Valor EjeVsPre { get; set; }      

        [DataMember]
        public List<DTO_QueryEstadoEjecucion> DetalleNivel1 { get; set; }

        [DataMember]
        public List<DTO_QueryEstadoEjecucion> DetalleNivel2 { get; set; }

        #endregion
    }
}
