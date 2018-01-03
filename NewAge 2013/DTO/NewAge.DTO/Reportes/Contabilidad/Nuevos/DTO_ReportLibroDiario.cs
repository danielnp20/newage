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
    /// Clase del reporte Auxiliar
    /// </summary>
    public class DTO_ReportLibroDiario
    {
        /// <summary>
        /// Constructor con DataReader
        /// <param name="islibros">Verifica si lo que se va a imprimir son solo los libros</param>
        /// </summary>
        public DTO_ReportLibroDiario(IDataReader dr, bool islibros)
        {
            InitCols();
            try
            {
                //Datos Generales
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaDesc"].ToString()))
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();                
                if (!string.IsNullOrEmpty(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ComprobanteDesc"].ToString()))
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["DebitoML"].ToString()))
                    this.DebitoML.Value = Convert.ToDecimal(dr["DebitoML"]);
                if (!string.IsNullOrEmpty(dr["CreditoML"].ToString()))
                    this.CreditoML.Value = Convert.ToDecimal(dr["CreditoML"]);
                if (!string.IsNullOrEmpty(dr["DebitoME"].ToString()))
                    this.DebitoME.Value = Convert.ToDecimal(dr["DebitoME"]);
                if (!string.IsNullOrEmpty(dr["CreditoME"].ToString()))
                    this.CreditoME.Value = Convert.ToDecimal(dr["CreditoME"]);

                //Datos Especificos de reporte
                if (!islibros)
                {
                    #region Valores Especificos
                    if (!string.IsNullOrEmpty(dr["Comprobante"].ToString()))
                        this.Comprobante.Value = dr["Comprobante"].ToString();
                    if (!string.IsNullOrEmpty(dr["ComprobanteNro"].ToString()))
                        this.ComprobanteNro.Value = dr["ComprobanteNro"].ToString();
                    if (!string.IsNullOrEmpty(dr["BalanceTipoID"].ToString()))
                        this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                        this.TerceroID.Value = dr["TerceroID"].ToString();
                    if (!string.IsNullOrEmpty(dr["nomTercero"].ToString()))
                        this.TerceroDesc.Value = dr["nomTercero"].ToString();
                    if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                        this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                        this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                        this.ProyectoID.Value = dr["ProyectoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["TasaCambioBase"].ToString()))
                        this.TasaCambioBase.Value = Convert.ToDecimal(dr["TasaCambioBase"]);
                    if (!string.IsNullOrEmpty(dr["InicialML"].ToString()))
                        this.InicialML.Value = Convert.ToDecimal(dr["InicialML"]);
                    if (!string.IsNullOrEmpty(dr["InicialME"].ToString()))
                        this.InicialME.Value = Convert.ToDecimal(dr["InicialME"]);
                    if (!string.IsNullOrEmpty(dr["DocumentoCOM"].ToString()))
                        this.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();
                    if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                        this.Descriptivo.Value = dr["Descriptivo"].ToString();
                    if (!string.IsNullOrEmpty(dr["vlrBaseML"].ToString()))
                        this.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);
                    if (!string.IsNullOrEmpty(dr["vlrBaseME"].ToString()))
                        this.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]);
                    if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                        this.Fecha = Convert.ToDateTime(dr["Fecha"]);
                    #endregion
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportLibroDiario(IDataReader dr, bool isTerceroCuenta, bool isNullble)
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
        public DTO_ReportLibroDiario()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Comprobante = new UDTSQL_char(32);
            this.PeriodoID = new UDTSQL_datetime();
            this.Fecha = new DateTime();
            this.CuentaID = new UDT_CuentaID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.ComprobanteNro = new UDT_Descriptivo();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteDesc = new UDT_Descriptivo();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ProyectoID = new UDT_ProyectoID();
            this.TasaCambioBase = new UDT_Valor();
            this.DebitoML = new UDT_Valor();
            this.CreditoML = new UDT_Valor();
            this.DebitoME = new UDT_Valor();
            this.CreditoME = new UDT_Valor();
            this.InicialML = new UDT_Valor();
            this.InicialME = new UDT_Valor();
            this.DocumentoCOM = new UDTSQL_varchar(20);
            this.Descriptivo = new UDT_Descriptivo();
            this.vlrBaseML = new UDT_Valor();
            this.vlrBaseME = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDTSQL_datetime PeriodoID { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Valor TasaCambioBase { get; set; }

        [DataMember]
        public UDT_Valor DebitoML { get; set; }

        [DataMember]
        public UDT_Valor CreditoML { get; set; }

        [DataMember]
        public UDT_Valor DebitoME { get; set; }

        [DataMember]
        public UDT_Valor CreditoME { get; set; }

        [DataMember]
        public UDT_Valor InicialML { get; set; }

        [DataMember]
        public UDT_Valor InicialME { get; set; }

        [DataMember]
        public UDTSQL_varchar DocumentoCOM { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor vlrBaseML { get; set; }

        [DataMember]
        public UDT_Valor vlrBaseME { get; set; }

        #endregion

    }
}
