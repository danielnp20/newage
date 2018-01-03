using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Auxiliar
    /// </summary>
    public class DTO_ReportEjecucionPresupuestal
    {
        /// <summary>
        /// Constructor con DataReader
        /// <param name="islibros">Verifica si lo que se va a imprimir son solo los libros</param>
        /// </summary>
        public DTO_ReportEjecucionPresupuestal(string loggerConn, IDataReader dr, string TipoReporte)
        {
            InitCols();
            try
            {
                #region Propiedades Genericas

                #region Valores Genericos

                this.TipoProyecto.Value = dr["TipoProyecto"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoAnteIniLoc"].ToString()))
                    this.SaldoAnteIniLoc.Value = Convert.ToDecimal(dr["SaldoAnteIniLoc"]);
                if (!string.IsNullOrEmpty(dr["SaldoAnteIniExt"].ToString()))
                    this.SaldoAnteIniExt.Value = Convert.ToDecimal(dr["SaldoAnteIniExt"]);
                if (!string.IsNullOrEmpty(dr["MovimientoLoc"].ToString()))
                    this.MovimientoLoc.Value = Convert.ToDecimal(dr["MovimientoLoc"]);
                if (!string.IsNullOrEmpty(dr["MovimientoExt"].ToString()))
                    this.MovimientoExt.Value = Convert.ToDecimal(dr["MovimientoExt"]);
                if (!string.IsNullOrEmpty(dr["NvoSaldoLoc"].ToString()))
                    this.NvoSaldoLoc.Value = Convert.ToDecimal(dr["NvoSaldoLoc"]);
                if (!string.IsNullOrEmpty(dr["NvoSaldoExt"].ToString()))
                    this.NvoSaldoExt.Value = Convert.ToDecimal(dr["NvoSaldoExt"]);
                if (!string.IsNullOrEmpty(dr["CompromisoLoc"].ToString()))
                    this.CompromisoLoc.Value = Convert.ToDecimal(dr["CompromisoLoc"]);
                if (!string.IsNullOrEmpty(dr["CompromisoExt"].ToString()))
                    this.CompromisoExt.Value = Convert.ToDecimal(dr["CompromisoExt"]);
                if (!string.IsNullOrEmpty(dr["PresuLoc"].ToString()))
                    this.PresuLoc.Value = Convert.ToDecimal(dr["PresuLoc"]);
                if (!string.IsNullOrEmpty(dr["PresuExt"].ToString()))
                    this.PresuExt.Value = Convert.ToDecimal(dr["PresuExt"]);
                if (!string.IsNullOrEmpty(dr["EjeMasComprLoc"].ToString()))
                    this.EjeMasComprLoc.Value = Convert.ToDecimal(dr["EjeMasComprLoc"]);
                if (!string.IsNullOrEmpty(dr["EjeMasComprExt"].ToString()))
                    this.EjeMasComprExt.Value = Convert.ToDecimal(dr["EjeMasComprExt"]);
                if (!string.IsNullOrEmpty(dr["ValorLoc"].ToString()))
                    this.ValorLoc.Value = Convert.ToDecimal(dr["ValorLoc"]);
                if (!string.IsNullOrEmpty(dr["ValorExt"].ToString()))
                    this.ValorExt.Value = Convert.ToDecimal(dr["ValorExt"]);
                if (!string.IsNullOrEmpty(dr["PorcenLoc"].ToString()))
                    this.PorcenLoc.Value = Convert.ToDecimal(dr["PorcenLoc"]);
                if (!string.IsNullOrEmpty(dr["PorcenExt"].ToString()))
                    this.PorcenExt.Value = Convert.ToDecimal(dr["PorcenExt"]);

                #endregion
                #region Valores por Reporte

                switch (TipoReporte)
                {
                    case "LinRecur":
                        #region Agrupa el reporte Recurso por Linea Presupuestal

                        if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                            this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["LineaDesc"].ToString()))
                            this.LineaDesc.Value = dr["LineaDesc"].ToString();
                        if (!string.IsNullOrEmpty(dr["RecursoID"].ToString()))
                            this.RecursoID.Value = dr["RecursoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["GrupoRecursoDesc"].ToString()))
                            this.GrupoRecursoDesc.Value = dr["GrupoRecursoDesc"].ToString();

                        #endregion
                        break;

                    case "RecurAct":
                        #region Agrupa el reporte Actividad por Recurso

                        if (!string.IsNullOrEmpty(dr["ActividadID"].ToString()))
                            this.ActividadID.Value = dr["ActividadID"].ToString();
                        if (!string.IsNullOrEmpty(dr["ActivDesc"].ToString()))
                            this.ActivDesc.Value = dr["ActivDesc"].ToString();
                        if (!string.IsNullOrEmpty(dr["RecursoID"].ToString()))
                            this.RecursoID.Value = dr["RecursoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["GrupoRecursoDesc"].ToString()))
                            this.GrupoRecursoDesc.Value = dr["GrupoRecursoDesc"].ToString();

                        #endregion
                        break;

                    case "LineCosto":
                        #region Agrupa el reporte Lineas por Centro de Costo

                        if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["CostoDesc"].ToString()))
                            this.CostoDesc.Value = dr["CostoDesc"].ToString();
                        if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                            this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["LineaDesc"].ToString()))
                            this.LineaDesc.Value = dr["LineaDesc"].ToString();

                        #endregion
                        break;

                    default:
                        #region Agrupa el reporte Proyecto por Actividad

                        if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                            this.ProyectoID.Value = dr["ProyectoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["ProyectoDesc"].ToString()))
                            this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                        if (!string.IsNullOrEmpty(dr["ActividadID"].ToString()))
                            this.ActividadID.Value = dr["ActividadID"].ToString();
                        if (!string.IsNullOrEmpty(dr["ActivDesc"].ToString()))
                            this.ActivDesc.Value = dr["ActivDesc"].ToString();

                        #endregion
                        break;
                }

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(loggerConn, ex, string.Empty, "DTO_ReportEjecucionPresupuestal");
                throw exception;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportEjecucionPresupuestal()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Columnas Genericas

            this.TipoProyecto = new UDTSQL_char(20);
            this.SaldoAnteIniLoc = new UDT_Valor();
            this.SaldoAnteIniExt = new UDT_Valor();
            this.MovimientoLoc = new UDT_Valor();
            this.MovimientoExt = new UDT_Valor();
            this.NvoSaldoLoc = new UDT_Valor();
            this.NvoSaldoExt = new UDT_Valor();
            this.CompromisoLoc = new UDT_Valor();
            this.CompromisoExt = new UDT_Valor();
            this.PresuLoc = new UDT_Valor();
            this.PresuExt = new UDT_Valor();
            this.EjeMasComprLoc = new UDT_Valor();
            this.EjeMasComprExt = new UDT_Valor();
            this.ValorLoc = new UDT_Valor();
            this.ValorExt = new UDT_Valor();
            this.PorcenLoc = new UDT_Valor();
            this.PorcenExt = new UDT_Valor();

            #endregion
            #region Columnas Especificas para Reporte

            this.ProyectoID = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_DescripTBase();
            this.ActividadID = new UDT_ActividadID();
            this.ActivDesc = new UDT_DescripTBase();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.CostoDesc = new UDT_DescripTBase();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LineaDesc = new UDT_DescripTBase();
            this.RecursoID = new UDT_RecursoID();
            this.GrupoRecursoDesc = new UDT_DescripTBase();

            #endregion
        }

        #region Propiedades

        #region Propiedades Genericas

        [DataMember]
        public UDTSQL_char TipoProyecto { get; set; }

        [DataMember]
        public UDT_Valor SaldoAnteIniLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoAnteIniExt { get; set; }

        [DataMember]
        public UDT_Valor MovimientoLoc { get; set; }

        [DataMember]
        public UDT_Valor MovimientoExt { get; set; }

        [DataMember]
        public UDT_Valor NvoSaldoLoc { get; set; }

        [DataMember]
        public UDT_Valor NvoSaldoExt { get; set; }

        [DataMember]
        public UDT_Valor CompromisoLoc { get; set; }

        [DataMember]
        public UDT_Valor CompromisoExt { get; set; }

        [DataMember]
        public UDT_Valor PresuLoc { get; set; }

        [DataMember]
        public UDT_Valor PresuExt { get; set; }

        [DataMember]
        public UDT_Valor EjeMasComprLoc { get; set; }

        [DataMember]
        public UDT_Valor EjeMasComprExt { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc { get; set; }

        [DataMember]
        public UDT_Valor ValorExt { get; set; }

        [DataMember]
        public UDT_Valor PorcenLoc { get; set; }

        [DataMember]
        public UDT_Valor PorcenExt { get; set; }

        #endregion
        #region Propiedades Especificas Segun reporte

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_DescripTBase ProyectoDesc { get; set; }

        [DataMember]
        public UDT_ActividadID ActividadID { get; set; }

        [DataMember]
        public UDT_DescripTBase ActivDesc { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTBase LineaDesc { get; set; }

        [DataMember]
        public UDT_RecursoID RecursoID { get; set; }

        [DataMember]
        public UDT_DescripTBase GrupoRecursoDesc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_DescripTBase CostoDesc { get; set; }

        #endregion

        #endregion

    }
}
