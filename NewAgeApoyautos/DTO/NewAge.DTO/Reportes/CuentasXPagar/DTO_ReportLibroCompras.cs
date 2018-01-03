using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Drawing;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Libro de Compras
    /// </summary>
    public class DTO_ReportLibroCompras
    {
        #region Constructores
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportLibroCompras(IDataReader dr, bool facturaEquivalente)
        {
            this.InitCols();
            try
            {
                //Valores Genericos
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["documentoCOM"].ToString()))
                    this.documentoCOM.Value = dr["documentoCOM"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["nomTercero"].ToString()))
                    this.nomTercero.Value = dr["nomTercero"].ToString();
                if (!string.IsNullOrEmpty(dr["Descripcion"].ToString()))
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (!string.IsNullOrEmpty(dr["iva"].ToString()))
                    this.iva.Value = Convert.ToDecimal(dr["iva"]);

                if (!facturaEquivalente)
                {
                    #region Valores Libro Compras

                    if (!string.IsNullOrEmpty(dr["vlrBruto"].ToString()))
                        this.vlrBruto.Value = Convert.ToDecimal(dr["vlrBruto"]);
                    if (!string.IsNullOrEmpty(dr["reteIva"].ToString()))
                        this.reteIva.Value = Convert.ToDecimal(dr["reteIva"]);
                    if (!string.IsNullOrEmpty(dr["reteFuente"].ToString()))
                        this.reteFuente.Value = Convert.ToDecimal(dr["reteFuente"]);
                    if (!string.IsNullOrEmpty(dr["reteIca"].ToString()))
                        this.reteIca.Value = Convert.ToDecimal(dr["reteIca"]);
                    if (!string.IsNullOrEmpty(dr["total"].ToString()))
                        this.total.Value = Convert.ToDecimal(dr["total"]);

                    #endregion
                }

                else
                {
                    #region Valores Factura Equivalente

                    if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                        this.CuentaID.Value = dr["CuentaID"].ToString();
                    if (!string.IsNullOrEmpty(dr["nomCuenta"].ToString()))
                        this.nomCuenta.Value = dr["nomCuenta"].ToString();
                    if (!string.IsNullOrEmpty(dr["Debito"].ToString()))
                        this.Debito.Value = Convert.ToDecimal(dr["Debito"]);
                    if (!string.IsNullOrEmpty(dr["Credito"].ToString()))
                        this.Credito.Value = Convert.ToDecimal(dr["Credito"]);
                    if (!string.IsNullOrEmpty(dr["FacturaEQ"].ToString()))
                        this.FacturaEQ.Value = Convert.ToInt32(dr["FacturaEQ"]);
                    if (!string.IsNullOrEmpty(dr["ImpuestoPorc"].ToString()))
                        this.ImpuestoPorc.Value = Convert.ToDecimal(dr["ImpuestoPorc"]);
                    if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                        this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                    if (!string.IsNullOrEmpty(dr["vlrBaseML"].ToString()))
                        this.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);
                    if (!string.IsNullOrEmpty(dr["comprobante"].ToString()))
                        this.comprobante.Value = dr["comprobante"].ToString();

                    #endregion
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLibroCompras()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Columnas Genericas
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.documentoCOM = new UDTSQL_varchar(20);
            this.TerceroID = new UDT_TerceroID();
            this.nomTercero = new UDT_DescripTBase();
            this.Descripcion = new UDT_DescripTBase();
            this.iva = new UDT_Valor();

            //Columnas Libro Compras
            this.vlrBruto = new UDT_Valor();
            this.reteIva = new UDT_Valor();
            this.reteFuente = new UDT_Valor();
            this.reteIca = new UDT_Valor();
            this.total = new UDT_Valor();

            //Columnas Factura Equivalente
            this.CuentaID = new UDT_CuentaID();
            this.nomCuenta = new UDT_DescripTBase();
            this.Debito = new UDT_Valor();
            this.Credito = new UDT_Valor();
            this.FacturaEQ = new UDT_Consecutivo();
            this.ImpuestoPorc = new UDT_PorcentajeID();
            this.NitEmpresa = new UDT_TerceroID();
            this.Valor = new UDT_Valor();
            this.vlrBaseML = new UDT_Valor();
            this.comprobante = new UDTSQL_char(20);

        }
        #endregion

        #region Propiedades

        #region Propiedades para Genericas

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_varchar documentoCOM { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase nomTercero { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_Valor iva { get; set; }

        #endregion

        #region Propiedades para Libro de Compras

        [DataMember]
        public UDT_Valor vlrBruto { get; set; }

        [DataMember]
        public UDT_Valor reteIva { get; set; }

        [DataMember]
        public UDT_Valor reteFuente { get; set; }

        [DataMember]
        public UDT_Valor reteIca { get; set; }

        [DataMember]
        public UDT_Valor total { get; set; }

        #endregion

        #region Propiedades para Factura Equivalente

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_DescripTBase nomCuenta { get; set; }

        [DataMember]
        public UDT_Valor Debito { get; set; }

        [DataMember]
        public UDT_Valor Credito { get; set; }

        [DataMember]
        public UDT_Consecutivo FacturaEQ { get; set; }

        [DataMember]
        public UDT_PorcentajeID ImpuestoPorc { get; set; }

        [DataMember]
        public UDT_TerceroID NitEmpresa { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor vlrBaseML { get; set; }

        [DataMember]
        public UDTSQL_char comprobante { get; set; }

        #endregion

        #endregion
    }
}
