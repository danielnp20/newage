using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccSolicitudDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDocu
    {
        #region DTO_ccSolicitudDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ClienteRadica.Value = dr["ClienteRadica"].ToString();
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.ApellidoPri.Value = dr["ApellidoPri"].ToString();
                this.ApellidoSdo.Value = dr["ApellidoSdo"].ToString();
                this.NombrePri.Value = dr["NombrePri"].ToString();
                this.NombreSdo.Value = dr["NombreSdo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Edad"].ToString()))
                    this.Edad.Value = Convert.ToInt16(dr["Edad"]);
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Solicitud"].ToString())) // Financiera
                    this.Solicitud.Value = Convert.ToInt32(dr["Solicitud"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Pagare"].ToString())) // Financiera
                    this.Pagare.Value = Convert.ToString(dr["Pagare"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["PagarePOL"].ToString())) // Financiera
                    this.PagarePOL.Value = Convert.ToString(dr["PagarePOL"].ToString());
                this.Poliza.Value = dr["Poliza"].ToString(); // Financiera
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DevueltaInd"].ToString()))
                    this.DevueltaInd.Value = Convert.ToBoolean(dr["DevueltaInd"]);
                this.ConcesionarioID.Value = dr["ConcesionarioID"].ToString();
                this.Concesionario2.Value = dr["Concesionario2"].ToString();
                this.AseguradoraID.Value = dr["AseguradoraID"].ToString();
                this.CooperativaID.Value = dr["CooperativaID"].ToString();
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                this.CentroPagoID.Value = dr["CentroPagoID"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.Ciudad.Value = dr["Ciudad"].ToString();
                this.TipoCreditoID.Value = dr["TipoCreditoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumDocCompra"].ToString()))
                    this.NumDocCompra.Value = Convert.ToInt32(dr["NumDocCompra"]);
                this.VendedorID.Value = Convert.ToString(dr["VendedorID"]);
                this.Codeudor1.Value = dr["Codeudor1"].ToString();
                this.Codeudor2.Value = dr["Codeudor2"].ToString();
                this.Codeudor3.Value = dr["Codeudor3"].ToString();
                this.Codeudor4.Value = dr["Codeudor4"].ToString();
                this.Codeudor5.Value = dr["Codeudor5"].ToString();
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.DatoAdd6.Value = dr["DatoAdd6"].ToString();
                this.DatoAdd7.Value = dr["DatoAdd7"].ToString();
                this.DatoAdd8.Value = dr["DatoAdd8"].ToString();
                this.DatoAdd9.Value = dr["DatoAdd9"].ToString();
                this.DatoAdd10.Value = dr["DatoAdd10"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoGarantia"].ToString()))
                    this.TipoGarantia.Value = Convert.ToByte(dr["TipoGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoOperacion"].ToString()))
                    this.TipoOperacion.Value = Convert.ToByte(dr["TipoOperacion"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoCredito"].ToString()))
                    this.TipoCredito.Value = Convert.ToByte(dr["TipoCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaEfectivaCredito"].ToString()))
                    this.TasaEfectivaCredito.Value = Convert.ToDecimal(dr["TasaEfectivaCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["IncorporacionPreviaInd"].ToString()))
                    this.IncorporacionPreviaInd.Value = Convert.ToBoolean(dr["IncorporacionPreviaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["IncorporacionTipo"].ToString()))
                    this.IncorporacionTipo.Value = Convert.ToByte(dr["IncorporacionTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocVerificado"].ToString()))
                    this.NumDocVerificado.Value = Convert.ToInt32(dr["NumDocVerificado"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocOpera"].ToString()))
                    this.NumDocOpera.Value = Convert.ToInt32(dr["NumDocOpera"]);
                if (!string.IsNullOrWhiteSpace(dr["PeriodoPago"].ToString()))
                    this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCuota1"].ToString()))
                    this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                if (!string.IsNullOrWhiteSpace(dr["PorInteres"].ToString()))
                    this.PorInteres.Value = Convert.ToDecimal(dr["PorInteres"]);
                if (!string.IsNullOrWhiteSpace(dr["PorSeguro"].ToString()))
                    this.PorSeguro.Value = Convert.ToDecimal(dr["PorSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["CompraCarteraInd"].ToString()))
                    this.CompraCarteraInd.Value = Convert.ToBoolean(dr["CompraCarteraInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PagoVentanillaInd"].ToString()))
                    this.PagoVentanillaInd.Value = Convert.ToBoolean(dr["PagoVentanillaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrAdicional"].ToString()))
                    this.VlrAdicional.Value = Convert.ToDecimal(dr["VlrAdicional"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPrestamo"].ToString()))
                    this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCompra"].ToString()))
                    this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrDescuento"].ToString()))
                    this.VlrDescuento.Value = Convert.ToDecimal(dr["VlrDescuento"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGiro"].ToString()))
                    this.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPoliza"].ToString())) // Financiera
                    this.VlrPoliza.Value = Convert.ToDecimal(dr["VlrPoliza"]);
                if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoSeguro"].ToString())) // Financiera
                    this.PlazoSeguro.Value = Convert.ToInt16(dr["PlazoSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["Cuota1Seguro"].ToString())) // Financiera
                    this.Cuota1Seguro.Value = Convert.ToInt16(dr["Cuota1Seguro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotaSeguro"].ToString())) // Financiera
                    this.VlrCuotaSeguro.Value = Convert.ToDecimal(dr["VlrCuotaSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFinanciaSeguro"].ToString())) // Financiera
                    this.VlrFinanciaSeguro.Value = Convert.ToDecimal(dr["VlrFinanciaSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaLiqSeguro"].ToString())) // Financiera
                    this.FechaLiqSeguro.Value = Convert.ToDateTime(dr["FechaLiqSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVigenciaINI"].ToString())) // Financiera
                    this.FechaVigenciaINI.Value = Convert.ToDateTime(dr["FechaVigenciaINI"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVigenciaFIN"].ToString())) // Financiera
                    this.FechaVigenciaFIN.Value = Convert.ToDateTime(dr["FechaVigenciaFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["DocSeguro"].ToString())) // Financiera
                    this.DocSeguro.Value = Convert.ToInt32(dr["DocSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCupoDisponible"].ToString()))
                    this.VlrCupoDisponible.Value = Convert.ToDecimal(dr["VlrCupoDisponible"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCapacidad"].ToString()))
                    this.VlrCapacidad.Value = Convert.ToDecimal(dr["VlrCapacidad"]);
                if (!string.IsNullOrWhiteSpace(dr["RechazoInd"].ToString()))
                    this.RechazoInd.Value = Convert.ToBoolean(dr["RechazoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PrendaConyugueInd"].ToString()))
                    this.PrendaConyugueInd.Value = Convert.ToBoolean(dr["PrendaConyugueInd"]);

                this.RechazoCausal.Value = dr["RechazoCausal"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["RechazoFecha"].ToString()))
                    this.RechazoFecha.Value = Convert.ToDateTime(dr["RechazoFecha"]);
                this.AnalisisUsuario.Value = dr["AnalisisUsuario"].ToString();
                this.RechazoUsuario.Value = dr["RechazoUsuario"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComponenteExtraInd"].ToString()))
                    this.ComponenteExtraInd.Value = Convert.ToBoolean(dr["ComponenteExtraInd"]);
                this.BancoID_1.Value = dr["BancoID_1"].ToString();
                this.BcoCtaNro_1.Value = (dr["BcoCtaNro_1"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaTipo_1"].ToString()))
                    this.CuentaTipo_1.Value = Convert.ToByte(dr["CuentaTipo_1"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPreSolicitado"].ToString()))
                    this.VlrPreSolicitado.Value = Convert.ToDecimal(dr["VlrPreSolicitado"]);
                if (!string.IsNullOrWhiteSpace(dr["VersionNro"].ToString()))
                    this.VersionNro.Value = Convert.ToInt16(dr["VersionNro"]);
                if (!string.IsNullOrWhiteSpace(dr["DtoPrimeraCuotaInd"].ToString()))
                    this.DtoPrimeraCuotaInd.Value = Convert.ToBoolean(dr["DtoPrimeraCuotaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorDtoPrimeraCuota"].ToString()))
                    this.ValorDtoPrimeraCuota.Value = Convert.ToDecimal(dr["ValorDtoPrimeraCuota"]);

                //Otros
                if (!string.IsNullOrWhiteSpace(dr["PorComponente1"].ToString()))
                    this.PorComponente1.Value = Convert.ToDecimal(dr["PorComponente1"]);
                if (!string.IsNullOrWhiteSpace(dr["PorComponente2"].ToString()))
                    this.PorComponente2.Value = Convert.ToDecimal(dr["PorComponente2"]);
                if (!string.IsNullOrWhiteSpace(dr["PorComponente3"].ToString()))
                    this.PorComponente3.Value = Convert.ToDecimal(dr["PorComponente3"]);
                if (!string.IsNullOrWhiteSpace(dr["IncorporaMesInd"].ToString()))
                    this.IncorporaMesInd.Value = Convert.ToBoolean(dr["IncorporaMesInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocIncorporacion"].ToString()))
                    this.NumDocIncorporacion.Value = Convert.ToInt32(dr["NumDocIncorporacion"]);

                ///otros poliza
                ///
                if (!string.IsNullOrWhiteSpace(dr["CancelaContadoPolizaInd"].ToString()))
                    this.CancelaContadoPolizaInd.Value = Convert.ToBoolean(dr["CancelaContadoPolizaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CancelaContadoOtrosSegInd"].ToString()))
                    this.CancelaContadoOtrosSegInd.Value = Convert.ToBoolean(dr["CancelaContadoOtrosSegInd"]);
                if (!string.IsNullOrWhiteSpace(dr["IntermediarioExternoInd"].ToString()))
                    this.IntermediarioExternoInd.Value = Convert.ToBoolean(dr["IntermediarioExternoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrOtrasFinanciaciones"].ToString()))
                    this.VlrOtrasFinanciaciones.Value = Convert.ToInt32(dr["VlrOtrasFinanciaciones"]);
                if (!string.IsNullOrWhiteSpace(dr["OtrasFinancPagoContadoInd"].ToString()))
                    this.OtrasFinancPagoContadoInd.Value = Convert.ToBoolean(dr["OtrasFinancPagoContadoInd"]);
                
                if (!string.IsNullOrWhiteSpace(dr["CartaInstrucciones"].ToString()))
                    this.CartaInstrucciones.Value = Convert.ToString(dr["CartaInstrucciones"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaInstruccionesPOL"].ToString()))
                    this.CartaInstruccionesPOL.Value = Convert.ToString(dr["CartaInstruccionesPOL"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoWEB"].ToString()))
                    this.ConsecutivoWEB.Value = Convert.ToInt32(dr["ConsecutivoWEB"]);
                
                #region campos adicionales vehiculo
                if (!string.IsNullOrWhiteSpace(dr["Marca"].ToString()))
                    this.Marca.Value = Convert.ToString(dr["Marca"]);                    
                if (!string.IsNullOrWhiteSpace(dr["Cilindraje"].ToString()))
                    this.Cilindraje.Value = Convert.ToInt32(dr["Cilindraje"]);
                if (!string.IsNullOrWhiteSpace(dr["Linea"].ToString()))
                    this.Linea.Value = Convert.ToString(dr["Linea"]);
                if (!string.IsNullOrWhiteSpace(dr["Servicio"].ToString()))
                    this.Servicio.Value = Convert.ToByte(dr["Servicio"]);
                if (!string.IsNullOrWhiteSpace(dr["AireAcondicionado"].ToString()))
                    this.AireAcondicionado.Value = Convert.ToBoolean(dr["AireAcondicionado"]);
                else
                    this.AireAcondicionado.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["Tipocaja"].ToString()))
                    this.Tipocaja.Value = Convert.ToByte(dr["Tipocaja"]);
                if (!string.IsNullOrWhiteSpace(dr["PuertasNro"].ToString()))
                    this.PuertasNro.Value = Convert.ToByte(dr["PuertasNro"]);
                if (!string.IsNullOrWhiteSpace(dr["Referencia"].ToString()))
                    this.Referencia.Value = Convert.ToString(dr["Referencia"]);
                if (!string.IsNullOrWhiteSpace(dr["Complemento"].ToString()))
                    this.Complemento.Value = Convert.ToString(dr["Complemento"]);
                if (!string.IsNullOrWhiteSpace(dr["Termoking"].ToString()))
                    this.Termoking.Value = Convert.ToBoolean(dr["Termoking"]);
                else
                    this.Termoking.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["CeroKmInd"].ToString()))
                    this.CeroKmInd.Value = Convert.ToBoolean(dr["CeroKmInd"]);
                else
                    this.CeroKmInd.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["PrecioVenta"].ToString()))
                    this.PrecioVenta.Value = Convert.ToDecimal(dr["PrecioVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotaInicial"].ToString()))
                    this.CuotaInicial.Value = Convert.ToDecimal(dr["CuotaInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["Modelo"].ToString()))
                    this.Modelo.Value = Convert.ToInt32(dr["Modelo"]);
                if (!string.IsNullOrWhiteSpace(dr["PrecioVentaChasis"].ToString()))
                    this.PrecioVentaChasis.Value = Convert.ToDecimal(dr["PrecioVentaChasis"]);
                if (!string.IsNullOrWhiteSpace(dr["PrecioVentaComplemento"].ToString()))
                    this.PrecioVentaComplemento.Value = Convert.ToDecimal(dr["PrecioVentaComplemento"]);


                // flujo
                if (!string.IsNullOrWhiteSpace(dr["DesestimientoInd"].ToString()))
                    this.DesestimientoInd.Value = Convert.ToBoolean(dr["DesestimientoInd"]);
                else
                    this.DesestimientoInd.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["NegociosGestionarInd"].ToString()))
                    this.NegociosGestionarInd.Value = Convert.ToBoolean(dr["NegociosGestionarInd"]);
                else
                    this.NegociosGestionarInd.Value = false;
                this.ActividadFlujoNegociosGestionarID.Value = dr["ActividadFlujoNegociosGestionarID"].ToString();
                ///
                if (!string.IsNullOrWhiteSpace(dr["CtasPagadas"].ToString()))
                    this.CtasPagadas.Value = Convert.ToInt32(dr["CtasPagadas"]);
                if (!string.IsNullOrWhiteSpace(dr["AbonosCapital"].ToString()))
                    this.AbonosCapital.Value = Convert.ToInt32(dr["AbonosCapital"]);
                if (!string.IsNullOrWhiteSpace(dr["OblPrepagadas"].ToString()))
                    this.OblPrepagadas.Value = Convert.ToInt32(dr["OblPrepagadas"]);
                if (!string.IsNullOrWhiteSpace(dr["NroPrejuridicos"].ToString()))
                    this.NroPrejuridicos.Value = Convert.ToInt32(dr["NroPrejuridicos"]);

                if (!string.IsNullOrWhiteSpace(dr["PrendaNuevaInd"].ToString()))
                    this.PrendaNuevaInd.Value = Convert.ToBoolean(dr["PrendaNuevaInd"]);
                else
                    this.PrendaNuevaInd.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["HipotecaNuevaInd"].ToString()))
                    this.HipotecaNuevaInd.Value = Convert.ToBoolean(dr["HipotecaNuevaInd"]);
                else
                    this.HipotecaNuevaInd.Value = false;

                if (!string.IsNullOrWhiteSpace(dr["PrendaNuevaInd2"].ToString()))
                    this.PrendaNuevaInd2.Value = Convert.ToBoolean(dr["PrendaNuevaInd2"]);
                else
                    this.PrendaNuevaInd2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["HipotecaNuevaInd2"].ToString()))
                    this.HipotecaNuevaInd2.Value = Convert.ToBoolean(dr["HipotecaNuevaInd2"]);
                else
                    this.HipotecaNuevaInd2.Value = false;

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
        public DTO_ccSolicitudDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            #region Campos Principales

            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_BasicID();
            this.ClienteID = new UDT_ClienteID();
            this.ClienteRadica = new UDT_ClienteID();
            this.ApellidoPri = new UDT_DescripTBase();
            this.ApellidoSdo = new UDT_DescripTBase();
            this.NombrePri = new UDT_DescripTBase();
            this.NombreSdo = new UDT_DescripTBase();
            this.Libranza = new UDT_LibranzaID();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.AsesorID = new UDT_AsesorID();
            this.DevueltaInd = new UDT_SiNo();
            this.ConcesionarioID = new UDT_CodigoGrl10();
            this.Concesionario2 = new UDT_CodigoGrl10();
            this.AseguradoraID = new UDT_CodigoGrl10();
            this.CooperativaID = new UDT_CodigoGrl5();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.ZonaID = new UDT_ZonaID();
            this.Ciudad = new UDT_LugarGeograficoID();
            this.TipoCreditoID = new UDT_CodigoGrl5();
            this.NumDocCompra = new UDT_Consecutivo();
            this.VendedorID = new UDT_CompradorCarteraID();
            this.TipoCredito = new UDTSQL_tinyint();
            this.IncorporaMesInd = new UDT_SiNo();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.FechaVto = new UDTSQL_smalldatetime();
            this.CompraCarteraInd = new UDT_SiNo();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrAdicional = new UDT_Valor();
            this.TasaEfectivaCredito = new UDT_PorcentajeCarteraID();
            this.PorInteres = new UDT_PorcentajeCarteraID();
            this.PorSeguro = new UDT_PorcentajeCarteraID();
            this.PorComponente1 = new UDT_PorcentajeCarteraID();
            this.PorComponente2 = new UDT_PorcentajeCarteraID();
            this.PorComponente3 = new UDT_PorcentajeCarteraID();
            this.DatoAdd1 = new UDTSQL_char(50);
            this.DatoAdd2 = new UDTSQL_char(50);
            this.DatoAdd3 = new UDTSQL_char(50);
            this.DatoAdd4 = new UDTSQL_char(50);
            this.DatoAdd5 = new UDTSQL_char(50);
            this.DatoAdd6 = new UDTSQL_char(50);
            this.DatoAdd7 = new UDTSQL_char(50);
            this.DatoAdd8 = new UDTSQL_char(50);
            this.DatoAdd9 = new UDTSQL_char(50);
            this.DatoAdd10 = new UDTSQL_char(50);
            this.VlrPrestamo = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrCompra = new UDT_Valor();
            this.VlrDescuento = new UDT_Valor();
            this.VlrGiro = new UDT_Valor();
            this.Plazo = new UDTSQL_smallint();
            this.VlrCuota = new UDT_Valor();
            this.VlrCupoDisponible = new UDT_Valor();
            this.VlrCapacidad = new UDT_Valor();
            this.PagoVentanillaInd = new UDT_SiNo();
            this.RechazoInd = new UDT_SiNo();
            this.PrendaConyugueInd= new UDT_SiNo();

            this.RechazoCausal = new UDT_CausalID();
            this.RechazoFecha = new UDTSQL_smalldatetime();
            this.AnalisisUsuario = new UDT_UsuarioID();
            this.RechazoUsuario = new UDT_UsuarioID();
            this.Observacion = new UDT_DescripTExt();
            this.Codeudor1 = new UDT_TerceroID();
            this.Codeudor2 = new UDT_TerceroID();
            this.Codeudor3 = new UDT_TerceroID();
            this.Codeudor4 = new UDT_TerceroID();
            this.Codeudor5 = new UDT_TerceroID();
            this.IncorporacionPreviaInd = new UDT_SiNo();
            this.IncorporacionTipo = new UDTSQL_tinyint();
            this.NumDocIncorporacion = new UDT_Consecutivo();
            this.NumDocVerificado = new UDT_Consecutivo();
            this.NumDocOpera = new UDT_Consecutivo();
            this.PeriodoPago = new UDTSQL_tinyint();
            this.TipoGarantia = new UDTSQL_tinyint();
            this.Solicitud = new UDTSQL_int();
            this.Pagare = new UDTSQL_char(15);
            this.PagarePOL = new UDTSQL_char(15);
            this.Poliza = new UDTSQL_char(20);
            this.PlazoSeguro = new UDTSQL_smallint();
            this.Cuota1Seguro = new UDTSQL_smallint();
            this.VlrCuotaSeguro = new UDT_Valor();
            this.VlrFinanciaSeguro = new UDT_Valor();
            this.VlrPoliza = new UDT_Valor();
            this.FechaLiqSeguro = new UDTSQL_smalldatetime();
            this.FechaVigenciaINI = new UDTSQL_smalldatetime();
            this.FechaVigenciaFIN = new UDTSQL_smalldatetime();
            this.DocSeguro = new UDT_Consecutivo();
            this.LiquidaAll = new UDT_SiNo();
            this.ComponenteExtraInd = new UDT_SiNo();
            this.BancoID_1 = new UDT_BasicID();
            this.CuentaTipo_1 = new UDTSQL_tinyint();
            this.BcoCtaNro_1 = new UDTSQL_varchar(15);
            this.VlrPreSolicitado = new UDT_Valor();
            this.VersionNro = new UDTSQL_int();
            this.DtoPrimeraCuotaInd = new UDT_SiNo();
            this.ValorDtoPrimeraCuota = new UDT_Valor();
            #endregion

            #region Campos Adicionales
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Estado = new UDTSQL_smallint();
            this.ObservacionRechazo = new UDT_DescripTExt();
            this.Nombre = new UDTSQL_char(200);
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.FechaIncorpora = new UDTSQL_smalldatetime();
            this.NumDocCredito = new UDT_Consecutivo();
            this.Edad = new UDTSQL_int();
            this.Otro = new UDT_DescripTExt();

            //Devoluciones
            this.NumDevoluciones = new UDTSQL_int();
            this.CausalDevolucion = new UDT_CodigoGrl5();
            this.ActividadFlujoReversion = new UDT_ActividadFlujoID();
            this.ActividadFlujoDesc = string.Empty;

            //otros Poliza
            this.CancelaContadoPolizaInd = new UDT_SiNo();
            this.CancelaContadoOtrosSegInd = new UDT_SiNo();
            this.IntermediarioExternoInd = new UDT_SiNo();
            this.VlrOtrasFinanciaciones = new UDT_Valor();
            this.OtrasFinancPagoContadoInd = new UDT_SiNo();
            this.CartaInstrucciones = new UDTSQL_varchar(10);
            this.CartaInstruccionesPOL = new UDTSQL_varchar(10);
            this.ConsecutivoWEB = new UDT_Consecutivo();

            // Datos Flujo
            this.ActividadFlujoNegociosGestionarID = new UDT_ActividadFlujoID();
            this.DesestimientoInd = new UDT_SiNo();
            this.NegociosGestionarInd = new UDT_SiNo();

            //Datos Adicionales
            this.CtasPagadas = new UDTSQL_int();
            this.AbonosCapital = new UDTSQL_int();
            this.OblPrepagadas = new UDTSQL_int();
            this.NroPrejuridicos = new UDTSQL_int();
            this.PrendaNuevaInd = new UDT_SiNo();
            this.HipotecaNuevaInd = new UDT_SiNo();
            this.PrendaNuevaInd2 = new UDT_SiNo();
            this.HipotecaNuevaInd2 = new UDT_SiNo();
            #endregion

            #region campos adicionales vehiculo
            this.Marca = new UDTSQL_varchar(100);
            this.Cilindraje = new UDTSQL_int();
            this.Linea = new UDTSQL_varchar(100);
            this.Servicio = new UDTSQL_tinyint ();
            this.AireAcondicionado = new UDT_SiNo();
            this.Tipocaja = new UDTSQL_tinyint ();
            this.PuertasNro = new UDTSQL_tinyint ();
            this.Referencia = new UDTSQL_varchar(100);
            this.Complemento = new UDTSQL_varchar(30);
            this.Termoking = new UDT_SiNo();
            this.CeroKmInd = new UDT_SiNo();
            this.PrecioVenta = new  UDT_Valor();
            this.CuotaInicial = new UDT_Valor();
            this.Modelo = new UDTSQL_int();
            this.PrecioVentaChasis = new UDT_Valor();
            this.PrecioVentaComplemento = new UDT_Valor();
            
            #endregion
            this.TipoOperacion = new UDTSQL_tinyint();
        }
        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public int index { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_ClienteID ClienteRadica { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase ApellidoPri { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase ApellidoSdo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase NombrePri { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase NombreSdo { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_SiNo DevueltaInd { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 ConcesionarioID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 Concesionario2 { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 AseguradoraID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CooperativaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoID { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Ciudad { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 TipoCreditoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocCompra { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CompradorCarteraID VendedorID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IncorporaMesInd { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PorcentajeCarteraID TasaEfectivaCredito { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorInteres { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorSeguro { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorComponente1 { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorComponente2 { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorComponente3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd6 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd7 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd8 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd9 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd10 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo CompraCarteraInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAdicional { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrCompra { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrDescuento { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrGiro { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrCupoDisponible { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrCapacidad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo PagoVentanillaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo RechazoInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo PrendaConyugueInd { get; set; }

        
        [DataMember]
        [NotImportable]
        public UDT_CausalID RechazoCausal { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime RechazoFecha { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_UsuarioID AnalisisUsuario { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_UsuarioID RechazoUsuario { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TerceroID Codeudor1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TerceroID Codeudor2 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TerceroID Codeudor3 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TerceroID Codeudor4 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TerceroID Codeudor5 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IncorporacionPreviaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint IncorporacionTipo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocIncorporacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocVerificado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocOpera { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint PeriodoPago { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoGarantia { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int Solicitud { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Pagare { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char PagarePOL { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Poliza { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint PlazoSeguro { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint Cuota1Seguro { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrCuotaSeguro { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrFinanciaSeguro { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrPoliza { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaLiqSeguro { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaVigenciaINI { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaVigenciaFIN { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo DocSeguro { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo ComponenteExtraInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo LiquidaAll { get; set; }

        //Info de Bancos para Tercero
        [DataMember]
        [NotImportable]
        public UDT_BasicID BancoID_1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint CuentaTipo_1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar BcoCtaNro_1 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrPreSolicitado { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int VersionNro { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo DtoPrimeraCuotaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorDtoPrimeraCuota { get; set; }



        #endregion

        #region Campos Extras

        [DataMember]
        [NotImportable]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDTSQL_smallint Estado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt ObservacionRechazo { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIncorpora { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int Edad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt Otro { get; set; }


        //Info Devolución
        [DataMember]
        [NotImportable]
        public UDTSQL_int NumDevoluciones { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl5 CausalDevolucion { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoReversion { get; set; }

        [DataMember]
        public string ActividadFlujoDesc { get; set; }


        [DataMember]
        [NotImportable]
        public UDT_SiNo CancelaContadoPolizaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo CancelaContadoOtrosSegInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IntermediarioExternoInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrOtrasFinanciaciones { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo OtrasFinancPagoContadoInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar CartaInstrucciones { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar CartaInstruccionesPOL { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ConsecutivoWEB { get; set; }

        [DataMember]
        [NotImportable]
        public bool SolicitarDatacreditoDeudor { get; set; }

        [DataMember]
        [NotImportable]
        public bool SolicitarDatacreditoCony { get; set; }

        [DataMember]
        [NotImportable]
        public bool SolicitarDatacreditoCod1 { get; set; }

        [DataMember]
        [NotImportable]
        public bool SolicitarDatacreditoCod2 { get; set; }


        [DataMember]
        [NotImportable]
        public bool SolicitarDatacreditoCod3 { get; set; }

        // Informacion Flujo
        [DataMember]
        [NotImportable]
        public UDT_ActividadFlujoID ActividadFlujoNegociosGestionarID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo DesestimientoInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo NegociosGestionarInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int CtasPagadas { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int AbonosCapital { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDTSQL_int OblPrepagadas { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int NroPrejuridicos { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo PrendaNuevaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo HipotecaNuevaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo PrendaNuevaInd2 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo HipotecaNuevaInd2 { get; set; }


        #endregion

        #region Campos Adicionales Vehiculo

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar Marca { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int Cilindraje { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar Linea { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint Servicio { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoOperacion { get; set; }
        [DataMember]
        [NotImportable]
        public UDT_SiNo  AireAcondicionado { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint Tipocaja { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint PuertasNro { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar Referencia { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_varchar Complemento { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo Termoking { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo CeroKmInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor PrecioVenta { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CuotaInicial { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int Modelo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor PrecioVentaChasis { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor PrecioVentaComplemento { get; set; }



        #endregion
    }
}
