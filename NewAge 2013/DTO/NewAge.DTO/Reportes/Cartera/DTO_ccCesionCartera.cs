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
    public class DTO_ccCesionCartera
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCesionCartera(IDataReader dr, bool isCesion)
        {
            this.InitCols();
            try
            {
                #region Carga los Datos para General el reporte de  Preventa Cartera

                if (!isCesion)
                {
                    if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                        this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    if (!string.IsNullOrEmpty(dr["Cedula"].ToString()))
                        this.CedulaDeudor.Value = dr["Cedula"].ToString();
                    if (!string.IsNullOrEmpty(dr["NombreCliente"].ToString()))
                        this.NombreDeudor.Value = dr["NombreCliente"].ToString();
                    if (!string.IsNullOrEmpty(dr["CuotasVend"].ToString()))
                        this.NumeroCuotasCesion.Value = Convert.ToInt32(dr["CuotasVend"]);
                    if (!string.IsNullOrEmpty(dr["VlrNominal"].ToString()))
                        this.ValorTotalCesion.Value = Convert.ToDecimal(dr["VlrNominal"]);
                    if (!string.IsNullOrEmpty(dr["VlrOfertado"].ToString()))
                        this.ValorpagoCesion.Value = Convert.ToDecimal(dr["VlrOfertado"]);
                    if (!string.IsNullOrEmpty(dr["NombreComprador"].ToString()))
                        this.NombreComprador.Value = dr["NombreComprador"].ToString();
                    if (!string.IsNullOrEmpty(dr["Oferta"].ToString()))
                        this.Oferta.Value = dr["Oferta"].ToString();
                }

                #endregion

                #region Carga los Datos para generar el reporte de Cesion Cartera
                else
                {
                    //if (!string.IsNullOrEmpty(dr["TerceroCoperativa"].ToString()))
                    //    this.TerceroCoperativa.Value = dr["TerceroCoperativa"].ToString();
                    //if (!string.IsNullOrEmpty(dr["NomCoperativa"].ToString()))
                    //    this.NomCoperativa.Value = dr["NomCoperativa"].ToString();
                    if (!string.IsNullOrEmpty(dr["CompradorCarteraID"].ToString()))
                        this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                    if (!string.IsNullOrEmpty(dr["TerceroIDComprador"].ToString()))
                        this.TerceroIDComprador.Value = dr["TerceroIDComprador"].ToString();
                    if (!string.IsNullOrEmpty(dr["NombreComprador"].ToString()))
                        this.NombreComprador.Value = dr["NombreComprador"].ToString();
                    if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                        this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    if (!string.IsNullOrEmpty(dr["VlrNominal"].ToString()))
                        this.VlrNominal.Value = Convert.ToDecimal(dr["VlrNominal"]);
                    if (!string.IsNullOrEmpty(dr["VlrCredito"].ToString()))
                        this.VlrCredito.Value = Convert.ToDecimal(dr["VlrCredito"]);
                    if (!string.IsNullOrEmpty(dr["VlrCuota"].ToString()))
                        this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    if (!string.IsNullOrEmpty(dr["Plazo"].ToString()))
                        this.Plazo.Value = Convert.ToInt32(dr["Plazo"]);
                    if (!string.IsNullOrEmpty(dr["fechaLiquida"].ToString()))
                        this.fechaLiquida.Value = Convert.ToDateTime(dr["fechaLiquida"]);
                    if (!string.IsNullOrEmpty(dr["PagaduriaID"].ToString()))
                        this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                    //if (!string.IsNullOrEmpty(dr["NombrePagaduria"].ToString()))
                    //    this.NombrePagaduria.Value = dr["NombrePagaduria"].ToString();
                    if (!string.IsNullOrEmpty(dr["LineaCreditoID"].ToString()))
                        this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["FechaVto"].ToString()))
                        this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                    if (!string.IsNullOrEmpty(dr["Oferta"].ToString()))
                        this.Oferta.Value = dr["Oferta"].ToString();
                    if (!string.IsNullOrEmpty(dr["Portafolio"].ToString()))
                        this.Portafolio.Value = dr["Portafolio"].ToString();
                    if (!string.IsNullOrEmpty(dr["TasaCesion"].ToString()))
                        this.TasaCesion.Value = Convert.ToDecimal(dr["TasaCesion"]);
                    if (!string.IsNullOrEmpty(dr["FechaVenta"].ToString()))
                        this.FechaVenta.Value = Convert.ToDateTime(dr["FechaVenta"]);
                    if (!string.IsNullOrEmpty(dr["FechaPrimerFlujo"].ToString()))
                        this.FechaPrimerFlujo.Value = Convert.ToDateTime(dr["FechaPrimerFlujo"]);
                    if (!string.IsNullOrEmpty(dr["PrimeraCuotaCesion"].ToString()))
                        this.PrimeraCuotaCesion.Value = Convert.ToInt32(dr["PrimeraCuotaCesion"]);
                    if (!string.IsNullOrEmpty(dr["NumeroCuotasCesion"].ToString()))
                        this.NumeroCuotasCesion.Value = Convert.ToInt32(dr["NumeroCuotasCesion"]);
                    if (!string.IsNullOrEmpty(dr["ValorCuotaCesion"].ToString()))
                        this.ValorCuotaCesion.Value = Convert.ToDecimal(dr["ValorCuotaCesion"]);
                    if (!string.IsNullOrEmpty(dr["ValorTotalCesion"].ToString()))
                        this.ValorTotalCesion.Value = Convert.ToDecimal(dr["ValorTotalCesion"]);
                    if (!string.IsNullOrEmpty(dr["ValorpagoCesion"].ToString()))
                        this.ValorpagoCesion.Value = Convert.ToDecimal(dr["ValorpagoCesion"]);
                    if (!string.IsNullOrEmpty(dr["ValorMargenCesion"].ToString()))
                        this.ValorMargenCesion.Value = Convert.ToDecimal(dr["ValorMargenCesion"]);
                    if (!string.IsNullOrEmpty(dr["CedulaDeudor"].ToString()))
                        this.CedulaDeudor.Value = dr["CedulaDeudor"].ToString();
                    
                    // Campos adicionales del último cambio sobre este reporte

                    if (!string.IsNullOrEmpty(dr["NombreDeudor"].ToString()))
                        this.NombreDeudor.Value = dr["NombreDeudor"].ToString();
                    if (!string.IsNullOrEmpty(dr["Comprobante"].ToString()))
                        this.Comprobante.Value = dr["Comprobante"].ToString();

                    // ¡¡¡¡¡ NO BORRAR !!!!!

                    //if (!string.IsNullOrEmpty(dr["DireccionDeudor"].ToString()))
                    //    this.DireccionDeudor.Value = dr["DireccionDeudor"].ToString();
                    //if (!string.IsNullOrEmpty(dr["Tel1Deudor"].ToString()))
                    //    this.Tel1Deudor.Value = dr["Tel1Deudor"].ToString();
                    //if (!string.IsNullOrEmpty(dr["cargoDeudor"].ToString()))
                    //    this.cargoDeudor.Value = dr["cargoDeudor"].ToString();
                    //if (!string.IsNullOrEmpty(dr["profesionDeudor"].ToString()))
                    //    this.profesionDeudor.Value = dr["profesionDeudor"].ToString();
                    //if (!string.IsNullOrEmpty(dr["sexoDeudor"].ToString()))
                    //    this.sexoDeudor.Value = dr["sexoDeudor"].ToString();
                    //if (!string.IsNullOrEmpty(dr["estadoCivDeudor"].ToString()))
                    //    this.estadoCivDeudor.Value = dr["estadoCivDeudor"].ToString();
                    //if (!string.IsNullOrEmpty(dr["EdadDedudor"].ToString()))
                    //    this.EdadDedudor.Value = Convert.ToInt16(dr["EdadDedudor"]);
                    //if (!string.IsNullOrEmpty(dr["VlrDevenDeudo"].ToString()))
                    //    this.VlrDevenDeudo.Value = Convert.ToDecimal(dr["VlrDevenDeudo"]);
                    //if (!string.IsNullOrEmpty(dr["CedulaCodeudor1"].ToString()))
                    //    this.CedulaCodeudor1.Value = dr["CedulaCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["NombreCodeudor1"].ToString()))
                    //    this.NombreCodeudor1.Value = dr["NombreCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["DireccionCodeudor1"].ToString()))
                    //    this.DireccionCodeudor1.Value = dr["DireccionCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["TelefonoCodeudor1"].ToString()))
                    //    this.TelefonoCodeudor1.Value = dr["TelefonoCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["CargoCodeudor1"].ToString()))
                    //    this.CargoCodeudor1.Value = dr["CargoCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["profesionCodeudor1"].ToString()))
                    //    this.profesionCodeudor1.Value = dr["profesionCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["sexoCodeudor1"].ToString()))
                    //    this.sexoCodeudor1.Value = dr["sexoCodeudor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["estadoCivCodeidor1"].ToString()))
                    //    this.estadoCivCodeidor1.Value = dr["estadoCivCodeidor1"].ToString();
                    //if (!string.IsNullOrEmpty(dr["EdadCodedudor1"].ToString()))
                    //    this.EdadCodedudor1.Value = Convert.ToInt16(dr["EdadCodedudor1"]);
                    //if (!string.IsNullOrEmpty(dr["VlrDevenCodeudor1"].ToString()))
                    //    this.VlrDevenCodeudor1.Value = Convert.ToDecimal(dr["VlrDevenCodeudor1"]);
                    //if (!string.IsNullOrEmpty(dr["CedulaCodeudor2"].ToString()))
                    //    this.CedulaCodeudor2.Value = dr["CedulaCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["NombreCodeudor2"].ToString()))
                    //    this.NombreCodeudor2.Value = dr["NombreCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["DireccionCodeudor2"].ToString()))
                    //    this.DireccionCodeudor2.Value = dr["DireccionCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["TelefonoCodeudor2"].ToString()))
                    //    this.TelefonoCodeudor2.Value = dr["TelefonoCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["CargoCodeudor2"].ToString()))
                    //    this.CargoCodeudor2.Value = dr["CargoCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["profesionCodeudor2"].ToString()))
                    //    this.profesionCodeudor2.Value = dr["profesionCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["sexoCodeudor2"].ToString()))
                    //    this.sexoCodeudor2.Value = dr["sexoCodeudor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["estadoCivCodeidor2"].ToString()))
                    //    this.estadoCivCodeidor2.Value = dr["estadoCivCodeidor2"].ToString();
                    //if (!string.IsNullOrEmpty(dr["EdadCodedudor2"].ToString()))
                    //    this.EdadCodedudor2.Value = Convert.ToInt16(dr["EdadCodedudor2"]);
                    //if (!string.IsNullOrEmpty(dr["VlrDevenCodeudor2"].ToString()))
                    //    this.VlrDevenCodeudor2.Value = Convert.ToDecimal(dr["VlrDevenCodeudor2"]);


                    //if (!string.IsNullOrEmpty(dr["CedulaAutorizadora"].ToString()))
                    //    this.CedulaAutorizadora.Value = Convert.ToInt16(dr["CedulaAutorizadora"]);
                    //if (!string.IsNullOrEmpty(dr["NombreAutorizadora"].ToString()))
                    //    this.NombreAutorizadora.Value = dr["NombreAutorizadora"].ToString();
                }
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
        public DTO_ccCesionCartera()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.TerceroCoperativa = new UDT_TerceroID();
            this.NomCoperativa = new UDT_Descriptivo();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.TerceroIDComprador = new UDT_TerceroID();
            this.NombreComprador = new UDT_DescripTBase();
            this.Libranza = new UDT_LibranzaID();
            this.VlrNominal = new UDT_Valor();
            this.VlrCredito = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.Plazo = new UDTSQL_int();
            this.fechaLiquida = new UDTSQL_datetime();
            this.PagaduriaID = new UDT_PagaduriaID();
            //this.NombrePagaduria = new UDT_Descriptivo();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.FechaVto = new UDTSQL_datetime();
            this.Oferta = new UDT_DocTerceroID();
            this.Portafolio = new UDT_PortafolioID();
            this.TasaCesion = new UDT_TasaID();
            this.FechaVenta = new UDTSQL_datetime();
            this.FechaPrimerFlujo = new UDTSQL_datetime();
            this.PrimeraCuotaCesion = new UDT_CuotaID();
            this.NumeroCuotasCesion = new UDTSQL_int();
            this.ValorCuotaCesion = new UDT_Valor();
            this.ValorTotalCesion = new UDT_Valor();
            this.ValorpagoCesion = new UDT_Valor();
            this.ValorMargenCesion = new UDT_Valor();
            this.CedulaDeudor = new UDT_ClienteID();

            // Campos adicionales del último cambio sobre este reporte
            this.NombreDeudor = new UDT_Descriptivo();
            this.Comprobante = new UDTSQL_char(15);

            // ¡¡¡¡¡ NO BORRAR !!!!!

            //this.NombreDeudor = new UDT_Descriptivo();
            //this.DireccionDeudor = new UDTSQL_char(50);
            //this.Tel1Deudor = new UDTSQL_char(20);
            //this.cargoDeudor = new UDTSQL_char(25);
            //this.profesionDeudor = new UDTSQL_char(30);
            //this.sexoDeudor = new UDT_Descriptivo();
            //this.estadoCivDeudor = new UDT_Descriptivo();
            //this.EdadDedudor = new UDTSQL_int();
            //this.VlrDevenDeudo = new UDT_Valor();
            //this.CedulaCodeudor1 = new UDT_ClienteID();
            //this.NombreCodeudor1 = new UDT_Descriptivo();
            //this.DireccionCodeudor1 = new UDTSQL_char(50);
            //this.TelefonoCodeudor1 = new UDTSQL_char(20);
            //this.CargoCodeudor1 = new UDTSQL_char(25);
            //this.profesionCodeudor1 = new UDTSQL_char(30);
            //this.sexoCodeudor1 = new UDT_Descriptivo();
            //this.estadoCivCodeidor1 = new UDT_Descriptivo();
            //this.EdadCodedudor1 = new UDTSQL_int();
            //this.VlrDevenCodeudor1 = new UDT_Valor();
            //this.CedulaCodeudor2 = new UDT_ClienteID();
            //this.NombreCodeudor2 = new UDT_Descriptivo();
            //this.DireccionCodeudor2 = new UDTSQL_char(50);
            //this.TelefonoCodeudor2 = new UDTSQL_char(20);
            //this.CargoCodeudor2 = new UDTSQL_char(25);
            //this.profesionCodeudor2 = new UDTSQL_char(30);
            //this.sexoCodeudor2 = new UDT_Descriptivo();
            //this.estadoCivCodeidor2 = new UDT_Descriptivo();
            //this.EdadCodedudor2 = new UDTSQL_int();
            //this.VlrDevenCodeudor2 = new UDT_Valor();
            this.CedulaAutorizadora = new UDTSQL_char(15);
            this.NombreAutorizadora = new UDTSQL_char(50);
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_TerceroID TerceroCoperativa { get; set; }

        [DataMember]
        public UDT_Descriptivo NomCoperativa { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroIDComprador { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreComprador { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_Valor VlrNominal { get; set; }

        [DataMember]
        public UDT_Valor VlrCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDTSQL_int Plazo { get; set; }

        [DataMember]
        public UDTSQL_datetime fechaLiquida { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        //[DataMember]
        //public UDT_Descriptivo NombrePagaduria { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaVto { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDT_PortafolioID Portafolio { get; set; }

        [DataMember]
        public UDT_TasaID TasaCesion { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaVenta { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaPrimerFlujo { get; set; }

        [DataMember]
        public UDT_CuotaID PrimeraCuotaCesion { get; set; }

        [DataMember]
        public UDTSQL_int NumeroCuotasCesion { get; set; }

        [DataMember]
        public UDT_Valor ValorCuotaCesion { get; set; }

        [DataMember]
        public UDT_Valor ValorTotalCesion { get; set; }

        [DataMember]
        public UDT_Valor ValorpagoCesion { get; set; }

        [DataMember]
        public UDT_Valor ValorMargenCesion { get; set; }

        [DataMember]
        public UDT_ClienteID CedulaDeudor { get; set; }

        // Campos adicionales del ultimo cambio sobre este reporte

        [DataMember]
        public UDT_Descriptivo NombreDeudor { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        // ¡¡¡¡¡ NO BORRAR !!!!!

        //[DataMember]
        //public UDT_Descriptivo NombreDeudor { get; set; }

        //[DataMember]
        //public UDTSQL_char DireccionDeudor { get; set; }

        //[DataMember]
        //public UDTSQL_char Tel1Deudor { get; set; }

        //[DataMember]
        //public UDTSQL_char cargoDeudor { get; set; }

        //[DataMember]
        //public UDTSQL_char profesionDeudor { get; set; }

        //[DataMember]
        //public UDT_Descriptivo sexoDeudor { get; set; }

        //[DataMember]
        //public UDT_Descriptivo estadoCivDeudor { get; set; }

        //[DataMember]
        //public UDTSQL_int EdadDedudor { get; set; }

        //[DataMember]
        //public UDT_Valor VlrDevenDeudo { get; set; }

        //[DataMember]
        //public UDT_ClienteID CedulaCodeudor1 { get; set; }

        //[DataMember]
        //public UDT_Descriptivo NombreCodeudor1 { get; set; }

        //[DataMember]
        //public UDTSQL_char DireccionCodeudor1 { get; set; }

        //[DataMember]
        //public UDTSQL_char TelefonoCodeudor1 { get; set; }

        //[DataMember]
        //public UDTSQL_char CargoCodeudor1 { get; set; }

        //[DataMember]
        //public UDTSQL_char profesionCodeudor1 { get; set; }

        //[DataMember]
        //public UDT_Descriptivo sexoCodeudor1 { get; set; }

        //[DataMember]
        //public UDT_Descriptivo estadoCivCodeidor1 { get; set; }

        //[DataMember]
        //public UDTSQL_int EdadCodedudor1 { get; set; }

        //[DataMember]
        //public UDT_Valor VlrDevenCodeudor1 { get; set; }

        //[DataMember]
        //public UDT_ClienteID CedulaCodeudor2 { get; set; }

        //[DataMember]
        //public UDT_Descriptivo NombreCodeudor2 { get; set; }

        //[DataMember]
        //public UDTSQL_char DireccionCodeudor2 { get; set; }

        //[DataMember]
        //public UDTSQL_char TelefonoCodeudor2 { get; set; }

        //[DataMember]
        //public UDTSQL_char CargoCodeudor2 { get; set; }

        //[DataMember]
        //public UDTSQL_char profesionCodeudor2 { get; set; }

        //[DataMember]
        //public UDT_Descriptivo sexoCodeudor2 { get; set; }

        //[DataMember]
        //public UDT_Descriptivo estadoCivCodeidor2 { get; set; }

        //[DataMember]
        //public UDTSQL_int EdadCodedudor2 { get; set; }

        //[DataMember]
        //public UDT_Valor VlrDevenCodeudor2 { get; set; }

        [DataMember]
        public UDTSQL_char CedulaAutorizadora { get; set; }

        [DataMember]
        public UDTSQL_char NombreAutorizadora { get; set; }
        #endregion
    }
}
