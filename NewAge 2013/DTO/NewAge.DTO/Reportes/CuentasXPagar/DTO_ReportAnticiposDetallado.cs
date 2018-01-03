using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Reporte Facturas Por Pagar
    /// </summary>
    public class DTO_ReportAnticiposDetallado
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportAnticiposDetallado(IDataReader dr, int numDoc, bool isDocumento = false)
        {
            this.InitCols();

            //Valores Genericos
            if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
            if (!string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()))
                this.TerceroID.Value = dr["TerceroID"].ToString();
            if (!string.IsNullOrWhiteSpace(dr["NombreTercero"].ToString()))
                this.NombreTercero.Value = dr["NombreTercero"].ToString();
            if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            this.Documento.Value = dr["Documento"].ToString();
            this.AnticipoTipoID.Value = dr["AnticipoTipoID"].ToString();
            this.AnticipoTipoDesc.Value = dr["AnticipoTipoDesc"].ToString();

            //Valores para reportes de Anticipos
            if (!isDocumento)
            {
               
                if (!string.IsNullOrWhiteSpace(dr["MonedaOrigen"].ToString()))
                    this.MonedaOrigen.Value = dr["MonedaOrigen"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Concepto"].ToString()))
                    this.Concepto.Value = dr["Concepto"].ToString();
            }

            //Valores para Documento
            else
            {
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
            }
        }

        public DTO_ReportAnticiposDetallado(IDataReader dr, bool isDetallado)
        {
            this.InitCols();
            
            if (!string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()))
                this.TerceroID.Value = dr["TerceroID"].ToString();
            if (!string.IsNullOrWhiteSpace(dr["NombreTercero"].ToString()))
                this.NombreTercero.Value = dr["NombreTercero"].ToString();
            if (!string.IsNullOrWhiteSpace(dr["MonedaOrigen"].ToString()))
                this.MonedaOrigen.Value = dr["MonedaOrigen"].ToString();
            if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
        }

         /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportAnticiposDetallado()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_TerceroID();
            this.NombreTercero = new UDT_Descriptivo();
            this.MonedaOrigen = new UDTSQL_char(15);
            this.AnticipoTipoID = new UDT_AnticipoTipoID();
            this.Documento = new UDT_DocTerceroID();
            this.Concepto = new UDT_Descriptivo();
            this.Fecha = new UDTSQL_datetime();
            this.Valor = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.AnticipoTipoDesc = new UDT_Descriptivo();
        }
        #region Propiedades

        /// <summary>
        /// Moneda origen
        /// </summary>
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        /// <summary>
        /// Moneda origen
        /// </summary>
        [DataMember]
        public UDT_Descriptivo NombreTercero { get; set; }

        /// <summary>
        /// Moneda origen
        /// </summary>
        [DataMember]
        public UDTSQL_char MonedaOrigen { get; set; }

        /// <summary>
        /// Tipo Anticipo
        /// </summary>
        [DataMember]
        public UDT_AnticipoTipoID AnticipoTipoID { get; set; }

        /// <summary>
        /// Documento
        /// </summary>
        [DataMember]
        public UDT_DocTerceroID Documento { get; set; }

        /// <summary>
        /// Concepto
        /// </summary>
        [DataMember]
        public UDT_Descriptivo Concepto { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public UDT_Valor Valor { get; set; }

        /// <summary>
        /// Concepto
        /// </summary>
        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        /// <summary>
        /// Tipo Anticipo Desc
        /// </summary>
        [DataMember]
        public UDT_Descriptivo AnticipoTipoDesc { get; set; }

        #endregion
    }
}
