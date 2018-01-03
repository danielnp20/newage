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
    /// Clase del reporte Compromiso VS Facturas
    /// </summary>
    public class DTO_ReportCompromisoVSFacturas
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportCompromisoVSFacturas(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Proveedor"].ToString()))
                    this.Proveedor.Value = dr["Proveedor"].ToString();
                if (!string.IsNullOrEmpty(dr["Recibido"].ToString()))
                    this.Recibido.Value = dr["Recibido"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["Compromiso"].ToString()))
                    this.Compromiso.Value = dr["Compromiso"].ToString();
                if (!string.IsNullOrEmpty(dr["MonedaID"].ToString()))
                    this.MonedaID.Value = dr["MonedaID"].ToString();
                if (!string.IsNullOrEmpty(dr["Tipo"].ToString()))
                    this.Tipo.Value= dr["Tipo"].ToString();
                if (!string.IsNullOrEmpty(dr["Factura"].ToString()))
                    this.Factura.Value = dr["Factura"].ToString();
                if (!string.IsNullOrEmpty(dr["NumeroRadica"].ToString()))
                    this.NumeroRadica.Value = Convert.ToInt32(dr["NumeroRadica"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCompromisoVSFacturas()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Proveedor = new UDT_ProveedorID();
            this.Recibido = new UDTSQL_char(10);
            this.FechaDoc = new UDTSQL_datetime();
            this.Compromiso = new UDTSQL_char(10);
            this.Tipo =  new UDTSQL_char(4);
            this.MonedaID = new UDT_MonedaID();            
            this.Factura = new UDTSQL_char(20);
            this.NumeroRadica = new UDT_Consecutivo();
            this.Observacion = new UDT_DescripTBase();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_ProveedorID Proveedor { get; set; }

        [DataMember]
        public UDTSQL_char Recibido { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_char Compromiso { get; set; }

        [DataMember]
        public UDTSQL_char Tipo { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }       

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroRadica { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        #endregion

    }

}
