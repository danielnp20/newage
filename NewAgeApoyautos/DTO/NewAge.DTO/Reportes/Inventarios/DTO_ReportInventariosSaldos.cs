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
    /// Clase del reporte Comrprobante Control
    /// </summary>
    public class DTO_ReportInventariosSaldos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportInventariosSaldos(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Periodo"].ToString()))
                    this.Periodo = Convert.ToDateTime(dr["Periodo"]);
                if (!string.IsNullOrEmpty(dr["BodegaID"].ToString()))
                    this.BodegaID.Value = dr["BodegaID"].ToString();
                if (!string.IsNullOrEmpty(dr["BodegaDes"].ToString()))
                    this.BodegaDes.Value = dr["BodegaDes"].ToString();
                //if (!string.IsNullOrEmpty(dr["TipoBodeDesc"].ToString()))
                //    this.TipoBodeDesc.Value = dr["TipoBodeDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Referencia"].ToString()))
                    this.Referencia.Value = dr["Referencia"].ToString();
                //if (!string.IsNullOrEmpty(dr["Serial"].ToString()))
                //    this.Serial = dr["Serial"].ToString();
                if (!string.IsNullOrEmpty(dr["Producto"].ToString()))
                    this.Producto.Value = dr["Producto"].ToString();
                //if (!string.IsNullOrEmpty(dr["GrupoDesc"].ToString()))
                //    this.GrupoDesc.Value = dr["GrupoDesc"].ToString();
                //if (!string.IsNullOrEmpty(dr["ClaseDesc"].ToString()))
                //    this.ClaseDesc.Value = dr["ClaseDesc"].ToString();
                //if (!string.IsNullOrEmpty(dr["TipoDesc"].ToString()))
                //    this.TipoDesc.Value = dr["TipoDesc"].ToString();
                //if (!string.IsNullOrEmpty(dr["SeriDesc"].ToString()))
                //    this.SeriDesc.Value = dr["SeriDesc"].ToString();
                //if (!string.IsNullOrEmpty(dr["MateDesc"].ToString()))
                //    this.MateDesc.Value = dr["MateDesc"].ToString();
                //if (!string.IsNullOrEmpty(dr["Parametro1"].ToString()))
                //    this.Parametro1.Value = dr["Parametro1"].ToString();
                //if (!string.IsNullOrEmpty(dr["Parametro2"].ToString()))
                //    this.Parametro2.Value = dr["Parametro2"].ToString();
                if (!string.IsNullOrEmpty(dr["Inicial"].ToString()))
                    this.Inicial.Value = Convert.ToDecimal(dr["Inicial"]);
                if (!string.IsNullOrEmpty(dr["Entrada"].ToString()))
                    this.Entrada.Value = Convert.ToDecimal(dr["Entrada"]);
                if (!string.IsNullOrEmpty(dr["Salidas"].ToString()))
                    this.Salidas.Value = Convert.ToDecimal(dr["Salidas"]);
                if (!string.IsNullOrEmpty(dr["CantidadLoc"].ToString()))
                    this.CantidadLoc.Value = Convert.ToDecimal(dr["CantidadLoc"]);
                if (!string.IsNullOrEmpty(dr["ValorLocal"].ToString()))
                    this.ValorLocal.Value = Convert.ToDecimal(dr["ValorLocal"]);
                if (!string.IsNullOrEmpty(dr["VlrUnidadLoc"].ToString()))
                    this.VlrUnidadLoc.Value = Convert.ToDecimal(dr["VlrUnidadLoc"]);
                if (!string.IsNullOrEmpty(dr["ValorExt"].ToString()))
                    this.ValorExt.Value = Convert.ToDecimal(dr["ValorExt"]);
                if (!string.IsNullOrEmpty(dr["VlrUnidadExt"].ToString()))
                    this.VlrUnidadExt.Value = Convert.ToDecimal(dr["VlrUnidadExt"]);
                //if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                //    this.TerceroID.Value = dr["TerceroID"].ToString();
                //if (!string.IsNullOrEmpty(dr["nombreTercero"].ToString()))
                //    this.nombreTercero.Value = dr["nombreTercero"].ToString();
                //if (!string.IsNullOrEmpty(dr["fecha"].ToString()))
                //    this.fecha = Convert.ToDateTime(dr["fecha"]);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportInventariosSaldos(IDataReader dr, bool isNullble)
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
        public DTO_ReportInventariosSaldos()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Periodo = new DateTime();
            this.BodegaID = new UDT_BodegaID();
            this.BodegaDes = new UDT_Descriptivo();
            this.TipoBodeDesc = new UDT_Descriptivo();
            this.Referencia = new UDT_ReferenciaID();
            this.Serial = string.Empty;
            this.Producto = new UDT_Descriptivo();
            this.GrupoDesc = new UDT_Descriptivo();
            this.ClaseDesc = new UDT_Descriptivo();
            this.TipoDesc = new UDT_Descriptivo();
            this.SeriDesc = new UDT_Descriptivo();
            this.MateDesc = new UDT_Descriptivo();
            this.Parametro1 = new UDTSQL_char(15);
            this.Parametro2 = new UDTSQL_char(15);
            this.Inicial = new UDTSQL_numeric();
            this.Entrada = new UDTSQL_numeric();
            this.Salidas = new UDTSQL_numeric();
            this.CantidadLoc = new UDTSQL_numeric();
            this.ValorLocal = new UDTSQL_numeric();
            this.VlrUnidadLoc = new UDTSQL_numeric();
            this.ValorExt = new UDTSQL_numeric();
            this.VlrUnidadExt = new UDTSQL_numeric();
            this.BodegaTipoID = new UDT_BasicID();
            this.GrupoInvID = new UDT_BasicID();
            this.ClaseInvID = new UDT_BasicID();
            this.TipoInvID = new UDT_BasicID();
            this.SerieID = new UDT_BasicID();
            this.MaterialInvID = new UDT_BasicID();
            this.TerceroID = new UDT_BasicID();
            this.nombreTercero = new UDT_BasicID();
            this.fecha = new DateTime();
        }

        #region Propiedades

        [DataMember]
        public DateTime Periodo { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaDes { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoBodeDesc { get; set; }

        [DataMember]
        public UDT_ReferenciaID Referencia { get; set; }

        [DataMember]
        public string Serial { get; set; }

        [DataMember]
        public UDT_Descriptivo Producto { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo SeriDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo MateDesc { get; set; }

        [DataMember]
        public UDTSQL_char Parametro1 { get; set; }

        [DataMember]
        public UDTSQL_char Parametro2 { get; set; }

        [DataMember]
        public UDTSQL_numeric Inicial { get; set; }

        [DataMember]
        public UDTSQL_numeric Entrada { get; set; }

        [DataMember]
        public UDTSQL_numeric Salidas { get; set; }

        [DataMember]
        public UDTSQL_numeric CantidadLoc { get; set; }

        [DataMember]
        public UDTSQL_numeric ValorLocal { get; set; }

        [DataMember]
        public UDTSQL_numeric VlrUnidadLoc { get; set; }

        [DataMember]
        public UDTSQL_numeric ValorExt { get; set; }

        [DataMember]
        public UDTSQL_numeric VlrUnidadExt { get; set; }

        [DataMember]
        public UDT_BasicID BodegaTipoID { get; set; }

        [DataMember]
        public UDT_BasicID GrupoInvID { get; set; }

        [DataMember]
        public UDT_BasicID ClaseInvID { get; set; }

        [DataMember]
        public UDT_BasicID TipoInvID { get; set; }

        [DataMember]
        public UDT_BasicID SerieID { get; set; }

        [DataMember]
        public UDT_BasicID MaterialInvID { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_BasicID nombreTercero { get; set; }

        [DataMember]
        public DateTime fecha { get; set; }

        #endregion

    }

}
