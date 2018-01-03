using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportCausacionFacturas
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr">Lectura de Data</param>
        /// <param name="isDocContable">Verifica si es Documento Contable (TRUE = Carga info Doc Contable, FALSE = Carga info Causacion Fact.)</param>
        /// <param name="?"></param>
        public DTO_ReportCausacionFacturas(IDataReader dr, bool isDocContable)
        {
            this.InitCols();
            try
            {
                //Valores Genericos
                #region Valores Genericos

                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["nombreTercero"].ToString()))
                    this.nombreTercero.Value = dr["nombreTercero"].ToString();
                if (!string.IsNullOrEmpty(dr["nombreTerceroAux"].ToString()))
                    this.nombreTerceroAux.Value = dr["nombreTerceroAux"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroAux"].ToString()))
                    this.TerceroAux.Value = dr["TerceroAux"].ToString();
                if (!string.IsNullOrEmpty(dr["facturaNro"].ToString()))
                    this.facturaNro.Value = dr["facturaNro"].ToString();
                if (!string.IsNullOrEmpty(dr["Descripcion"].ToString()))
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                if (!string.IsNullOrEmpty(dr["fechaFac"].ToString()))
                    this.fechaFac.Value = Convert.ToDateTime(dr["fechaFac"]);
                if (!string.IsNullOrEmpty(dr["periodoID"].ToString()))
                    this.periodoID.Value = Convert.ToDateTime(dr["periodoID"]);
                if (!string.IsNullOrEmpty(dr["Comprobante"].ToString()))
                    this.Comprobante.Value = dr["Comprobante"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["nombreCta"].ToString()))
                    this.nombreCta.Value = dr["nombreCta"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrEmpty(dr["vlrBaseML"].ToString()))
                    this.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);
                if (!string.IsNullOrEmpty(dr["cuentaCxP"].ToString()))
                    this.cuentaCxP.Value = dr["cuentaCxP"].ToString();
                if (!string.IsNullOrEmpty(dr["nomCuentaCxp"].ToString()))
                    this.nomCuentaCxp.Value = dr["nomCuentaCxp"].ToString();

                #endregion

                //Causacion Factura 
                if (!isDocContable)
                {
                    #region Carga la info para Causacion de Factura

                    if (!string.IsNullOrEmpty(dr["VtoFecha"].ToString()))
                        this.VtoFecha.Value = Convert.ToDateTime(dr["VtoFecha"]);
                    if (!string.IsNullOrEmpty(dr["vlrMdaLoc"].ToString()))
                        this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                    if (!string.IsNullOrEmpty(dr["DEBITO"].ToString()))
                        this.DEBITO.Value = Convert.ToDecimal(dr["DEBITO"]);
                    if (!string.IsNullOrEmpty(dr["CREDITO"].ToString()))
                        this.CREDITO.Value = Convert.ToDecimal(dr["CREDITO"]);
                    if (!string.IsNullOrEmpty(dr["vlrBruto"].ToString()))
                        this.vlrBruto.Value = Convert.ToDecimal(dr["vlrBruto"]);
                    if (!string.IsNullOrEmpty(dr["iva"].ToString()))
                        this.iva.Value = Convert.ToDecimal(dr["iva"]);
                    if (!string.IsNullOrEmpty(dr["reteIva"].ToString()))
                        this.reteIva.Value = Convert.ToDecimal(dr["reteIva"]);
                    if (!string.IsNullOrEmpty(dr["reteFuente"].ToString()))
                        this.reteFuente.Value = Convert.ToDecimal(dr["reteFuente"]); ;
                    if (!string.IsNullOrEmpty(dr["reteIca"].ToString()))
                        this.reteIca.Value = Convert.ToDecimal(dr["reteIca"]);
                    if (!string.IsNullOrEmpty(dr["anticipo"].ToString()))
                        this.anticipo.Value = Convert.ToDecimal(dr["anticipo"]);
                    if (!string.IsNullOrEmpty(dr["ImpuestoPorc"].ToString()))
                        this.ImpuestoPorc.Value = Convert.ToDecimal(dr["ImpuestoPorc"]);

                    #endregion
                }
                //Documento Contable
                else
                {
                    #region Carga la info para Documento Contable

                    if (!string.IsNullOrEmpty(dr["vlrBaseME"].ToString()))
                        this.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]);
                    if (!string.IsNullOrEmpty(dr["TasaCambioBase"].ToString()))
                        this.TasaCambioBase.Value = Convert.ToDecimal(dr["TasaCambioBase"]);
                    if (!string.IsNullOrEmpty(dr["DebitoML"].ToString()))
                        this.DebitoML.Value = Convert.ToDecimal(dr["DebitoML"]);
                    if (!string.IsNullOrEmpty(dr["CreditoML"].ToString()))
                        this.CreditoML.Value = Convert.ToDecimal(dr["CreditoML"]);
                    if (!string.IsNullOrEmpty(dr["DebitoExt"].ToString()))
                        this.DebitoExt.Value = Convert.ToDecimal(dr["DebitoExt"]);
                    if (!string.IsNullOrEmpty(dr["CreditoExt"].ToString()))
                        this.CreditoExt.Value = Convert.ToDecimal(dr["CreditoExt"]);

                    #endregion
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCausacionFacturas()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            #region Columnas Genericas

            this.TerceroID = new UDT_TerceroID();
            this.nombreTercero = new UDT_DescripTBase();
            this.TerceroAux = new UDT_TerceroID();
            this.nombreTerceroAux = new UDT_DescripTBase();
            this.facturaNro = new UDTSQL_char(20);
            this.Descripcion = new UDT_DescripTBase();
            this.fechaFac = new UDTSQL_smalldatetime();
            this.periodoID = new UDT_PeriodoID();
            this.Comprobante = new UDTSQL_char(20);
            this.CuentaID = new UDT_CuentaID();
            this.nombreCta = new UDT_DescripTBase();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ProyectoID = new UDT_ProyectoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.vlrBaseML = new UDT_Valor();
            this.cuentaCxP = new UDT_CuentaID();
            this.nomCuentaCxp = new UDT_DescripTBase();

            #endregion

            #region Columnas para Causacion de Factura

            this.VtoFecha = new UDTSQL_smalldatetime();
            this.vlrMdaLoc = new UDT_Valor();
            this.DEBITO = new UDT_Valor();
            this.CREDITO = new UDT_Valor();
            this.vlrBruto = new UDT_Valor();
            this.ImpuestoPorc = new UDT_PorcentajeID();
            this.iva = new UDT_Valor();
            this.reteIva = new UDT_Valor();
            this.reteFuente = new UDT_Valor();
            this.reteIca = new UDT_Valor();
            this.anticipo = new UDT_Valor();

            #endregion

            #region Columnas para Documento Contable

            this.vlrBaseME = new UDT_Valor();
            this.TasaCambioBase = new UDT_TasaID();
            this.DebitoML = new UDT_Valor();
            this.CreditoML = new UDT_Valor();
            this.DebitoExt = new UDT_Valor();
            this.CreditoExt = new UDT_Valor();

            #endregion
        }
        #endregion

        #region Propiedades

        #region Propiedades Genericas

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase nombreTercero { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroAux { get; set; }

        [DataMember]
        public UDT_DescripTBase nombreTerceroAux { get; set; }

        [DataMember]
        public UDTSQL_char facturaNro { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime fechaFac { get; set; }

        [DataMember]
        public UDT_PeriodoID periodoID { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_DescripTBase nombreCta { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Valor vlrBaseML { get; set; }

        [DataMember]
        public UDT_CuentaID cuentaCxP { get; set; }

        [DataMember]
        public UDT_DescripTBase nomCuentaCxp { get; set; }

        #endregion
        #region Propiedades para Causacion de Factura

        [DataMember]
        public UDTSQL_smalldatetime VtoFecha { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        public UDT_Valor DEBITO { get; set; }

        [DataMember]
        public UDT_Valor CREDITO { get; set; }

        [DataMember]
        public UDT_Valor vlrBruto { get; set; }

        [DataMember]
        public UDT_PorcentajeID ImpuestoPorc { get; set; }

        [DataMember]
        public UDT_Valor iva { get; set; }

        [DataMember]
        public UDT_Valor reteIva { get; set; }

        [DataMember]
        public UDT_Valor reteFuente { get; set; }

        [DataMember]
        public UDT_Valor reteIca { get; set; }

        [DataMember]
        public UDT_Valor anticipo { get; set; }

        #endregion
        #region Propiedades para Documento Contable

        [DataMember]
        public UDT_Valor vlrBaseME { get; set; }

        [DataMember]
        public UDT_TasaID TasaCambioBase { get; set; }

        [DataMember]
        public UDT_Valor DebitoML { get; set; }

        [DataMember]
        public UDT_Valor CreditoML { get; set; }

        [DataMember]
        public UDT_Valor DebitoExt { get; set; }

        [DataMember]
        public UDT_Valor CreditoExt { get; set; }

        #endregion

        #endregion
    }
}
