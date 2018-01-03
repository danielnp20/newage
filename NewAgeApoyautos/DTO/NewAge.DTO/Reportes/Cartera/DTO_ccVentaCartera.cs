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
    public class DTO_ccVentaCartera
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccVentaCartera(IDataReader dr, bool isResumida)
        {
            this.InitCols();
            try
            {
                //Genericas
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                if (!string.IsNullOrEmpty(dr["comprador"].ToString()))
                    this.comprador.Value = dr["comprador"].ToString();
                if (!string.IsNullOrEmpty(dr["VlrSaldo"].ToString()))
                    this.VlrSaldo.Value = Convert.ToDecimal(dr["VlrSaldo"]);
                if (!string.IsNullOrEmpty(dr["VlrOferta"].ToString()))
                    this.VlrOferta.Value = Convert.ToDecimal(dr["VlrOferta"]);

                #region Valores

                //Venta Cartera Resumida
               
                    if (!string.IsNullOrEmpty(dr["Oferta"].ToString()))
                        this.Oferta.Value = dr["Oferta"].ToString();
                    if (!string.IsNullOrEmpty(dr["PrimerFlujo"].ToString()))
                        this.PrimerFlujo.Value = Convert.ToDateTime(dr["PrimerFlujo"]);
                    if (!string.IsNullOrEmpty(dr["UltimoFlujo"].ToString()))
                        this.UltimoFlujo.Value = Convert.ToDateTime(dr["UltimoFlujo"]);
                    if (!string.IsNullOrEmpty(dr["Observaciones"].ToString()))
                        this.Observaciones.Value = dr["Observaciones"].ToString();
                    if (!string.IsNullOrEmpty(dr["fecha"].ToString()))
                        this.fecha.Value = Convert.ToDateTime(dr["fecha"]);
                    if (!string.IsNullOrEmpty(dr["Factor"].ToString()))
                        this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
                    if (!string.IsNullOrEmpty(dr["VlrFlujos"].ToString()))
                        this.VlrFlujos.Value = Convert.ToDecimal(dr["VlrFlujos"]);
                    if (!string.IsNullOrEmpty(dr["Total"].ToString()))
                        this.Total.Value = Convert.ToDecimal(dr["Total"]);
                    if (!string.IsNullOrEmpty(dr["Activos"].ToString()))
                        this.Activos.Value = Convert.ToDecimal(dr["Activos"]);
                    if (!string.IsNullOrEmpty(dr["Mora"].ToString()))
                        this.Mora.Value = Convert.ToDecimal(dr["Mora"]);
                    if (!string.IsNullOrEmpty(dr["Prepagos"].ToString()))
                        this.Prepagos.Value = Convert.ToDecimal(dr["Prepagos"]);
                    if (!string.IsNullOrEmpty(dr["Recompras"].ToString()))
                        this.Recompras.Value = Convert.ToDecimal(dr["Recompras"]);

                //Venta Cartera Resumida
               
                #endregion
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccVentaCartera()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Genericas
            this.NumeroDoc = new UDT_Consecutivo();
            this.VlrSaldo = new UDT_Valor();
            this.VlrOferta = new UDT_Valor();
            this.comprador = new UDT_DescripTBase();

            //Venta Cartera Resumida
            this.Oferta = new UDT_DocTerceroID();
            this.PrimerFlujo = new UDTSQL_smalldatetime();
            this.UltimoFlujo = new UDTSQL_smalldatetime();
            this.Observaciones = new UDT_DescripTExt();
            this.fecha = new UDTSQL_smalldatetime();
            this.Factor = new UDT_TasaID();
            this.VlrFlujos = new UDT_Valor();
            this.Total = new UDT_Valor();
            this.Activos = new UDT_Valor();
            this.Mora = new UDT_Valor();
            this.Prepagos = new UDT_Valor();
            this.Recompras = new UDT_Valor();

            //Venta Cartera Resumida
            this.Libranza = new UDT_LibranzaID();
            this.Codigo = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.Plazo = new UDTSQL_smallint();
            this.Venta = new UDT_CuotaID();
            this.VlrTotal = new UDT_Valor();
            this.TotalCuotas = new UDTSQL_int();
            this.CuotasPendientes = new UDTSQL_int();
            this.Saldo = new UDT_Valor();
            this.SaldoMora = new UDT_Valor();
            this.Altura = new UDT_CuotaID();
            this.ctaMora = new UDTSQL_int();
            this.FechaPrepago = new UDTSQL_smalldatetime();
            this.FechaRecompra = new UDTSQL_smalldatetime();
        }
        #endregion

        #region Propiedades

        //Propiedades Genericas
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        //Propiedades Genericas
        [DataMember]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        public UDT_Valor VlrOferta { get; set; }

        public UDT_DescripTBase comprador { get; set; }

        //Propiedades Venta Resumen
        #region Venta Cartera Resumida
        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime PrimerFlujo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime UltimoFlujo { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime fecha { get; set; }

        [DataMember]
        public UDT_TasaID Factor { get; set; }

        [DataMember]
        public UDT_Valor VlrFlujos { get; set; }

        [DataMember]
        public UDT_Valor Total { get; set; }

        [DataMember]
        public UDT_Valor Activos { get; set; }

        [DataMember]
        public UDT_Valor Mora { get; set; }

        [DataMember]
        public UDT_Valor Prepagos { get; set; }

        [DataMember]
        public UDT_Valor Recompras { get; set; }
        #endregion

        //Propiedades Venta Detallada
        #region Venta Cartera Detallada
        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID Codigo { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_CuotaID Venta { get; set; }

        [DataMember]
        public UDT_Valor VlrTotal { get; set; }

        [DataMember]
        public UDTSQL_int TotalCuotas { get; set; }

        [DataMember]
        public UDTSQL_int CuotasPendientes { get; set; }

        [DataMember]
        public UDT_Valor Saldo { get; set; }

        [DataMember]
        public UDT_Valor SaldoMora { get; set; }

        [DataMember]
        public UDT_CuotaID Altura { get; set; }

        [DataMember]
        public UDTSQL_int ctaMora { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPrepago { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRecompra { get; set; }
        #endregion

        /// <summary>
        /// Detalle
        /// </summary>
        public List<DTO_ccVentaCarteraVista> Detalle { get; set; }

        #endregion
    }
}
