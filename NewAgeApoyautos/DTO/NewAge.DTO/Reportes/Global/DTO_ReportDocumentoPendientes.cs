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
    /// Clase del reporte Documentos Pendientes
    /// </summary>
    public class DTO_ReportDocumentoPendientes
    {
        /// <summary>
        /// Constructor con DataReader
        /// <param name="islibros">Verifica si lo que se va a imprimir son solo los libros</param>
        /// </summary>
        public DTO_ReportDocumentoPendientes(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["DocumentoID"].ToString()))
                    this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                if (!string.IsNullOrEmpty(dr["DocumentoDesc"].ToString()))
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["PeriodoDoc"].ToString()))
                    this.PeriodoDoc.Value = Convert.ToDateTime(dr["PeriodoDoc"]);
                if (!string.IsNullOrEmpty(dr["Documento"].ToString()))
                    this.Documento.Value = dr["Documento"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroDesc"].ToString()))
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Comprobante"].ToString()))
                    this.Comprobante.Value = dr["Comprobante"].ToString();
                if (!string.IsNullOrEmpty(dr["Descripcion"].ToString()))
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["DocumentoTercero"].ToString()))
                    this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["DocumentoPadre"].ToString()))
                    this.DocumentoPadre.Value = Convert.ToInt32(dr["DocumentoPadre"]);
                if (!string.IsNullOrEmpty(dr["Estado"].ToString()))
                    this.Estado.Value = Convert.ToInt16(dr["Estado"]);
                if (!string.IsNullOrEmpty(dr["ModuloID"].ToString()))
                    this.ModuloID.Value = dr["ModuloID"].ToString();

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportDocumentoPendientes()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Propiedades Genericas

            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.DocumentoDesc = new UDT_DescripTBase();
            this.PeriodoDoc = new UDT_PeriodoID();
            this.Documento = new UDTSQL_char(10);
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_DescripTBase();
            this.Comprobante = new UDTSQL_char(10);
            this.Descripcion = new UDT_DescripTBase();
            this.CuentaID = new UDT_CuentaID();
            this.DocumentoTercero = new UDTSQL_varchar(50);
            this.Valor = new UDT_Valor();
            this.DocumentoPadre = new UDT_Consecutivo();
            this.Estado = new UDTSQL_smallint();
            this.ModuloID = new UDT_ModuloID();

            #endregion
        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_DescripTBase DocumentoDesc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoDoc { get; set; }

        [DataMember]
        public UDTSQL_char Documento { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDTSQL_varchar DocumentoTercero { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoPadre { get; set; }

        [DataMember]
        public UDTSQL_smallint Estado { get; set; }

        [DataMember]
        public UDT_ModuloID ModuloID { get; set; }

        #endregion

    }
}
