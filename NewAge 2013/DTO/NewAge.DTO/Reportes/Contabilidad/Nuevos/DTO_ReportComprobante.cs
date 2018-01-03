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
    public class DTO_ReportComprobante
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportComprobante(IDataReader dr, bool isPreliminar)
        {
            this.InitCols();
            try
            {
                this.Comprobante.Value = dr["Comprobante"].ToString();
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha = Convert.ToDateTime(dr["Fecha"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro = Convert.ToInt32(dr["ComprobanteNro"]);
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.Linea.Value = dr["Linea"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["DebitoLoc"].ToString()))
                    this.DebitoLoc.Value = Convert.ToDecimal(dr["DebitoLoc"]);
                if (!string.IsNullOrEmpty(dr["CreditoLoc"].ToString()))
                    this.CreditoLoc.Value = Convert.ToDecimal(dr["CreditoLoc"]);
                if (!string.IsNullOrEmpty(dr["vlrBaseML"].ToString()))
                    this.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);

                if (!isPreliminar)
                {

                    this.nomTercero.Value = dr["nomTercero"].ToString();
                    if (!string.IsNullOrEmpty(dr["DocumentoCOM"].ToString()))
                        this.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["vlrBaseME"].ToString()))
                        this.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]);
                    if (!string.IsNullOrEmpty(dr["DebitoExt"].ToString()))
                        this.DebitoExt.Value = Convert.ToDecimal(dr["DebitoExt"]);
                    if (!string.IsNullOrEmpty(dr["CreditoExt"].ToString()))
                        this.CreditoExt.Value = Convert.ToDecimal(dr["CreditoExt"]);
                    this.UsuarioID.Value = dr["UsuarioID"].ToString();
                    this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                    this.DescDocumento.Value = dr["DescDocumento"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportComprobante(IDataReader dr, bool isPreliminar, bool isNullble)
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
        public DTO_ReportComprobante()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Comprobante = new UDTSQL_char(32);
            this.PeriodoID = new DateTime();
            this.Fecha = new DateTime();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new int();
            this.CuentaID = new UDT_CuentaID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.nomTercero = new UDT_Descriptivo();
            this.ProyectoID = new UDT_ProyectoID();
            this.Linea = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.DocumentoCOM = new UDTSQL_varchar(20);
            this.ComprobanteID = new UDT_ComprobanteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.vlrBaseML = new UDT_Valor();
            this.DebitoLoc = new UDT_Valor();
            this.CreditoLoc = new UDT_Valor();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.vlrBaseME = new UDT_Valor();
            this.DebitoExt = new UDT_Valor();
            this.CreditoExt = new UDT_Valor();
            this.UsuarioID = new UDT_UsuarioID();
            this.BalanceTipoID = new UDT_BalanceTipoID();
            this.DescDocumento = new UDT_Descriptivo();
            this.DatoAdd = new UDTSQL_char(50);
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public DateTime PeriodoID { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public int ComprobanteNro { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo nomTercero { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID Linea { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDTSQL_varchar DocumentoCOM { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor vlrBaseML { get; set; }

        [DataMember]
        public UDT_Valor DebitoLoc { get; set; }

        [DataMember]
        public UDT_Valor CreditoLoc { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Valor vlrBaseME { get; set; }

        [DataMember]
        public UDT_Valor DebitoExt { get; set; }

        [DataMember]
        public UDT_Valor CreditoExt { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDT_BalanceTipoID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescDocumento { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd { get; set; }

        [DataMember]
        public int Index { get; set; }

        #endregion

    }
}
