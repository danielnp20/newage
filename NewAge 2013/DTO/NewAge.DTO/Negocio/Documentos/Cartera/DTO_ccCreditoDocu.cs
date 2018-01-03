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
    /// Models DTO_ccCreditoDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCreditoDocu : DTO_SerializedObject
    {
        #region DTO_ccCreditoDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCreditoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);

                this.NumSolicitud.Value = Convert.ToInt32(dr["NumSolicitud"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"].ToString());
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.ConcesionarioID.Value = dr["ConcesionarioID"].ToString();
                this.AseguradoraID.Value = dr["AseguradoraID"].ToString();
                this.CooperativaID.Value = dr["CooperativaID"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.TipoCreditoID.Value = dr["TipoCreditoID"].ToString();
                this.Codeudor1.Value = dr["Codeudor1"].ToString();
                this.Codeudor2.Value = dr["Codeudor2"].ToString();
                this.Codeudor3.Value = dr["Codeudor3"].ToString();
                this.Codeudor4.Value = dr["Codeudor4"].ToString();
                this.Codeudor5.Value = dr["Codeudor5"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaLiquida"].ToString()))
                    this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCuota1"].ToString()))
                    this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                this.TipoEstado.Value = Convert.ToByte(dr["TipoEstado"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoDeuda"].ToString()))
                    this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"]);
                if (!string.IsNullOrWhiteSpace(dr["IndRestructurado"].ToString()))
                    this.IndRestructurado.Value = Convert.ToBoolean(dr["IndRestructurado"]);
                this.Ciudad.Value = dr["Ciudad"].ToString();
                this.VendedorID.Value = Convert.ToString(dr["VendedorID"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaEfectivaCredito"].ToString()))
                    this.TasaEfectivaCredito.Value = Convert.ToDecimal(dr["TasaEfectivaCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaEfectivaVenta"].ToString()))
                    this.TasaEfectivaVenta.Value = Convert.ToDecimal(dr["TasaEfectivaVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaEfectivaReCompra"].ToString()))
                    this.TasaEfectivaReCompra.Value = Convert.ToDecimal(dr["TasaEfectivaReCompra"]);
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.CompradorFinalID.Value = dr["CompradorFinalID"].ToString();
                this.TipoCredito.Value = Convert.ToByte(dr["TipoCredito"]);
                this.PorInteres.Value = Convert.ToDecimal(dr["PorInteres"]);
                this.PorSeguro.Value = Convert.ToDecimal(dr["PorSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["PorComponente1"].ToString()))
                    this.PorComponente1.Value = Convert.ToDecimal(dr["PorComponente1"]);
                if (!string.IsNullOrWhiteSpace(dr["PorComponente2"].ToString()))
                    this.PorComponente2.Value = Convert.ToDecimal(dr["PorComponente2"]);
                if (!string.IsNullOrWhiteSpace(dr["PorComponente3"].ToString()))
                    this.PorComponente3.Value = Convert.ToDecimal(dr["PorComponente3"]);
                this.CompraCarteraInd.Value = Convert.ToBoolean(dr["CompraCarteraInd"]);
                this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.VlrAdicional.Value = Convert.ToDecimal(dr["VlrAdicional"]);
                this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);
                this.VlrDescuento.Value = Convert.ToDecimal(dr["VlrDescuento"]);
                this.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.VlrCupoDisponible.Value = Convert.ToDecimal(dr["VlrCupoDisponible"]);
                this.PagoVentanillaInd.Value = Convert.ToBoolean(dr["PagoVentanillaInd"]);
                this.BloqueaVentaInd.Value = Convert.ToBoolean(dr["BloqueaVentaInd"]);
                this.VendidaInd.Value = Convert.ToBoolean(dr["VendidaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCXP"].ToString()))
                    this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"]);
                if (!string.IsNullOrWhiteSpace(dr["DocSustituye"].ToString()))
                    this.DocSustituye.Value = Convert.ToInt32(dr["DocSustituye"]);
                if (!string.IsNullOrWhiteSpace(dr["DocVenta"].ToString()))
                    this.DocVenta.Value = Convert.ToInt32(dr["DocVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["DocFactura"].ToString()))
                    this.DocFactura.Value = Convert.ToInt32(dr["DocFactura"]);
                if (!string.IsNullOrWhiteSpace(dr["DocDesestimiento"].ToString()))
                    this.DocDesestimiento.Value = Convert.ToInt32(dr["DocDesestimiento"]);
                if (!string.IsNullOrWhiteSpace(dr["DocRechazo"].ToString()))
                    this.DocRechazo.Value = Convert.ToInt32(dr["DocRechazo"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroCesion"].ToString()))
                    this.NumeroCesion.Value = dr["NumeroCesion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoVenta"].ToString()))
                    this.TipoVenta.Value = Convert.ToByte(dr["TipoVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                this.CanceladoInd.Value = Convert.ToBoolean(dr["CanceladoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["DocAcuerdoPago"].ToString()))
                    this.DocAcuerdoPago.Value = Convert.ToInt32(dr["DocAcuerdoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["DocPrepago"].ToString()))
                    this.DocPrepago.Value = Convert.ToInt32(dr["DocPrepago"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPrepago"].ToString()))
                    this.VlrPrepago.Value = Convert.ToDecimal(dr["VlrPrepago"]);
                if (!string.IsNullOrWhiteSpace(dr["DocEstadoCuenta"].ToString()))
                    this.DocEstadoCuenta.Value = Convert.ToInt32(dr["DocEstadoCuenta"]);
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CentroPagoID"].ToString()))
                    this.CentroPagoID.Value = dr["CentroPagoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumIncorporaDoc"].ToString()))
                    this.NumIncorporaDoc.Value = Convert.ToInt32(dr["NumIncorporaDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["SustituidoInd"].ToString()))
                    this.SustituidoInd.Value = Convert.ToBoolean(dr["SustituidoInd"]);                
                if (!string.IsNullOrWhiteSpace(dr["IncorporaMesInd"].ToString()))
                    this.IncorporaMesInd.Value = Convert.ToBoolean(dr["IncorporaMesInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDesIncorporaDoc"].ToString()))
                    this.NumDesIncorporaDoc.Value = Convert.ToInt32(dr["NumDesIncorporaDoc"]);
                this.IncorporacionTipo.Value = Convert.ToByte(dr["IncorporacionTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocVerificado"].ToString()))
                    this.NumDocVerificado.Value = Convert.ToInt32(dr["NumDocVerificado"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocOpera"].ToString()))
                    this.NumDocOpera.Value = Convert.ToInt32(dr["NumDocOpera"]);
                this.Abogado.Value = Convert.ToString(dr["Abogado"]);
                this.EtapaIncumplimiento.Value = Convert.ToString(dr["EtapaIncumplimiento"]);

                this.CobranzaEstadoID.Value = dr["CobranzaEstadoID"].ToString();
                this.CobranzaGestionID.Value = dr["CobranzaGestionID"].ToString();
                this.CobranzaGestionCierre.Value = dr["CobranzaGestionCierre"].ToString();
                this.ObsCobranza.Value = dr["ObsCobranza"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["PeriodoPago"].ToString()))
                    this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocCompra"].ToString()))
                    this.NumDocCompra.Value = Convert.ToInt32(dr["NumDocCompra"]);
                this.VlrMaximoNivelRiesgo.Value = Convert.ToDecimal(dr["VlrMaximoNivelRiesgo"]);
                if (!string.IsNullOrWhiteSpace(dr["CategoriaRestructura"].ToString()))
                    this.CategoriaRestructura.Value = Convert.ToString(dr["CategoriaRestructura"]);
                if (!string.IsNullOrWhiteSpace(dr["DocUltNomina"].ToString()))
                    this.DocUltNomina.Value = Convert.ToInt32(dr["DocUltNomina"]);
                if (!string.IsNullOrWhiteSpace(dr["DocUltRecaudo"].ToString()))
                    this.DocUltRecaudo.Value = Convert.ToInt32(dr["DocUltRecaudo"]);
                this.NovedadIncorporaID.Value = Convert.ToString(dr["NovedadIncorporaID"]);
                this.SiniestroEstadoID.Value = Convert.ToString(dr["SiniestroEstadoID"]);
                if (!string.IsNullOrWhiteSpace(dr["Sentencia"].ToString()))
                    this.Sentencia.Value = Convert.ToString(dr["Sentencia"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaSentencia"].ToString()))
                    this.FechaSentencia.Value = Convert.ToDateTime(dr["FechaSentencia"]);
                if (!string.IsNullOrWhiteSpace(dr["Juzgado"].ToString()))
                    this.Juzgado.Value = Convert.ToString(dr["Juzgado"]);

                if (!string.IsNullOrWhiteSpace(dr["VlrCuotaSentencia"].ToString()))
                    this.VlrCuotaSentencia.Value = Convert.ToDecimal(dr["VlrCuotaSentencia"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCuota1Sentencia"].ToString()))
                    this.FechaCuota1Sentencia.Value = Convert.ToDateTime(dr["FechaCuota1Sentencia"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoSentencia"].ToString()))
                    this.PlazoSentencia.Value = Convert.ToInt16(dr["PlazoSentencia"]);

                #region Campos especiales financiera
                if (!string.IsNullOrWhiteSpace(dr["Solicitud"].ToString()))
                    this.Solicitud.Value = Convert.ToInt32(dr["Solicitud"]);
                if (!string.IsNullOrWhiteSpace(dr["Pagare"].ToString()))
                    this.Pagare.Value = Convert.ToString(dr["Pagare"]);
                if (!string.IsNullOrWhiteSpace(dr["PagarePOL"].ToString()))
                    this.PagarePOL.Value = Convert.ToString(dr["PagarePOL"]);
                if (!string.IsNullOrWhiteSpace(dr["Poliza"].ToString()))
                    this.Poliza.Value = dr["Poliza"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DigitoVer1"].ToString()))
                    this.DigitoVer1.Value = dr["DigitoVer1"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DigitoVer2"].ToString()))
                    this.DigitoVer2.Value = dr["DigitoVer2"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroINC"].ToString()))
                    this.NumeroINC.Value = Convert.ToByte(dr["NumeroINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoSeguro"].ToString()))
                    this.PlazoSeguro.Value = Convert.ToInt16(dr["PlazoSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["Cuota1Seguro"].ToString()))
                    this.Cuota1Seguro.Value = Convert.ToInt16(dr["Cuota1Seguro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotaSeguro"].ToString()))
                    this.VlrCuotaSeguro.Value = Convert.ToDecimal(dr["VlrCuotaSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFinanciaSeguro"].ToString())) // Financiera
                    this.VlrFinanciaSeguro.Value = Convert.ToDecimal(dr["VlrFinanciaSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRestructura"].ToString()))
                    this.FechaRestructura.Value = Convert.ToDateTime(dr["FechaRestructura"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPoliza"].ToString()))
                    this.VlrPoliza.Value = Convert.ToDecimal(dr["VlrPoliza"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaLiqSeguro"].ToString()))
                    this.FechaLiqSeguro.Value = Convert.ToDateTime(dr["FechaLiqSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVigenciaINI"].ToString()))
                    this.FechaVigenciaINI.Value = Convert.ToDateTime(dr["FechaVigenciaINI"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVigenciaFIN"].ToString()))
                    this.FechaVigenciaFIN.Value = Convert.ToDateTime(dr["FechaVigenciaFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["DocSeguro"].ToString()))
                    this.DocSeguro.Value = Convert.ToInt32(dr["DocSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaReliquidaCJ"].ToString()))
                    this.FechaReliquidaCJ.Value = Convert.ToDateTime(dr["FechaReliquidaCJ"]);
                if (!string.IsNullOrWhiteSpace(dr["ComponenteExtraInd"].ToString()))
                    this.ComponenteExtraInd.Value = Convert.ToBoolean(dr["ComponenteExtraInd"]);

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
        public DTO_ccCreditoDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            #region Campos Propios
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumSolicitud = new UDT_Consecutivo();
            this.EmpresaID = new UDT_BasicID();
            this.ClienteID = new UDT_ClienteID();
            this.Libranza = new UDT_LibranzaID();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.AsesorID = new UDT_AsesorID();
            this.ConcesionarioID = new UDT_CodigoGrl10();
            this.AseguradoraID = new UDT_CodigoGrl10();
            this.CooperativaID = new UDT_CodigoGrl5();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.ZonaID = new UDT_ZonaID();
            this.TipoCreditoID = new UDT_CodigoGrl5();
            this.Codeudor1 = new UDT_TerceroID();
            this.Codeudor2 = new UDT_TerceroID();
            this.Codeudor3 = new UDT_TerceroID();
            this.Codeudor4 = new UDT_TerceroID();
            this.Codeudor5 = new UDT_TerceroID();
            this.IncorporaMesInd = new UDT_SiNo();
            this.IncorporacionTipo = new UDTSQL_tinyint();
            this.NumDocVerificado = new UDT_Consecutivo();
            this.NumDocOpera = new UDT_Consecutivo();
            this.Abogado = new UDT_AsesorID();
            this.EtapaIncumplimiento = new UDTSQL_char(10);

            this.CobranzaEstadoID = new UDT_CodigoGrl10();
            this.CobranzaGestionID = new UDT_CodigoGrl10();//
            this.CobranzaGestionCierre = new UDT_CodigoGrl10();
            this.ObsCobranza = new UDT_DescripTExt();

            this.PeriodoPago = new UDTSQL_tinyint();
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.FechaVto = new UDTSQL_smalldatetime();
            this.TipoEstado = new UDTSQL_tinyint();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.IndRestructurado = new UDT_SiNo();
            this.FechaEstado = new UDTSQL_smalldatetime();
            this.Ciudad = new UDT_LugarGeograficoID();
            this.NumDocCompra = new UDT_Consecutivo();
            this.VendedorID = new UDT_CompradorCarteraID();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.CompradorFinalID = new UDT_CompradorCarteraID();
            this.TipoCredito = new UDTSQL_tinyint();
            this.TasaEfectivaCredito = new UDT_PorcentajeCarteraID();
            this.TasaEfectivaVenta = new UDT_PorcentajeCarteraID();
            this.TasaEfectivaReCompra = new UDT_PorcentajeCarteraID();
            this.PorInteres = new UDT_PorcentajeCarteraID();
            this.PorSeguro = new UDT_PorcentajeCarteraID();
            this.PorComponente1 = new UDT_PorcentajeCarteraID();
            this.PorComponente2 = new UDT_PorcentajeCarteraID();
            this.PorComponente3 = new UDT_PorcentajeCarteraID();
            this.CompraCarteraInd = new UDT_SiNo();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrAdicional = new UDT_Valor();
            this.VlrPrestamo = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrCompra = new UDT_Valor();
            this.VlrDescuento = new UDT_Valor();
            this.VlrGiro = new UDT_Valor();
            this.VlrMaximoNivelRiesgo = new UDT_Valor();
            this.Plazo = new UDTSQL_smallint();
            this.VlrCuota = new UDT_Valor();
            this.VlrCupoDisponible = new UDT_Valor();
            this.PagoVentanillaInd = new UDT_SiNo();
            this.BloqueaVentaInd = new UDT_SiNo();
            this.VendidaInd = new UDT_SiNo();
            this.NumeroDocCXP = new UDT_Consecutivo();
            this.DocSustituye = new UDT_Consecutivo();
            this.DocVenta = new UDT_Consecutivo();
            this.DocFactura = new UDT_Consecutivo();

            this.DocDesestimiento = new UDT_Consecutivo();
            this.DocRechazo = new UDT_Consecutivo();

            this.NumeroCesion = new UDTSQL_char(15);
            this.TipoVenta = new UDTSQL_tinyint();
            this.Observacion = new UDT_DescripTExt();
            this.NumIncorporaDoc = new UDT_Consecutivo();
            this.SustituidoInd = new UDT_SiNo();
            this.NumDesIncorporaDoc = new UDT_Consecutivo();
            this.CanceladoInd = new UDT_SiNo();
            this.DocAcuerdoPago = new UDT_Consecutivo();
            this.DocPrepago = new UDT_Consecutivo();
            this.VlrPrepago = new UDT_Valor();
            this.DocEstadoCuenta = new UDT_Consecutivo();
            this.CategoriaRestructura = new UDTSQL_char(2);
            this.DocUltNomina = new UDT_Consecutivo();
            this.DocUltRecaudo = new UDT_Consecutivo();
            this.NovedadIncorporaID = new UDTSQL_char(5);
            this.SiniestroEstadoID = new UDT_CodigoGrl5();
            this.Sentencia = new UDTSQL_char(30);
            this.FechaSentencia = new UDTSQL_smalldatetime();
            this.Juzgado = new UDTSQL_char(10);
            this.VlrCuotaSentencia = new UDT_Valor();
            this.FechaCuota1Sentencia = new UDTSQL_smalldatetime();
            this.PlazoSentencia = new UDTSQL_smallint();
            #endregion

            #region Campos Especiales Cartera Financiera
            this.Solicitud = new UDTSQL_int();
            this.Pagare = new UDTSQL_char(15);
            this.PagarePOL = new UDTSQL_char(15);
            this.Poliza = new UDTSQL_char(20);
            this.DigitoVer1 = new UDTSQL_char(1);
            this.DigitoVer2 = new UDTSQL_char(1);
            this.NumeroINC = new UDTSQL_tinyint();
            this.PlazoSeguro = new UDTSQL_smallint();
            this.Cuota1Seguro = new UDTSQL_smallint();
            this.VlrCuotaSeguro = new UDT_Valor();
            this.VlrFinanciaSeguro = new UDT_Valor();
            this.FechaRestructura = new UDTSQL_smalldatetime();
            this.VlrPoliza = new UDT_Valor();
            this.FechaLiqSeguro = new UDTSQL_smalldatetime();
            this.FechaVigenciaINI = new UDTSQL_smalldatetime();
            this.FechaVigenciaFIN = new UDTSQL_smalldatetime();
            this.DocSeguro = new UDT_Consecutivo();
            this.FechaReliquidaCJ = new UDTSQL_smalldatetime();
            #endregion

            #region Campos Adicionales
            this.Editable = new UDT_SiNo();
            this.Aprobado = new UDT_SiNo();
            this.Estado = new UDTSQL_tinyint();
            this.CreditoCuotaNum = new UDT_Consecutivo();
            this.CuotasMora = new UDTSQL_int();
            this.BancoID = new UDT_BancoID();
            this.DescBanco = new UDT_Descriptivo();
            this.EC_Fecha = new UDTSQL_smalldatetime();
            this.EC_FijadoInd = new UDT_SiNo();
            this.EC_PrimeraCtaPagada = new UDT_CuotaID();
            this.EC_Proposito = new UDTSQL_tinyint();
            this.EC_ValorPago = new UDT_Valor();
            this.EC_PolizaMvto = new UDTSQL_tinyint();
            this.NumDocProceso = new UDT_Consecutivo();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.FechaIncorpora = new UDTSQL_datetime();
            this.FechaDesIncorpora = new UDTSQL_datetime();
            this.FechaPagoParcial = new UDTSQL_datetime();
            this.IsPreventa = new UDT_SiNo();
            this.LibranzaSustituye = new UDT_LibranzaID();
            this.NumeroDocSustituye = new UDT_Consecutivo();
            this.Nombre = new UDTSQL_char(200);
            this.NumCuenta = new UDTSQL_char(30);
            this.NumCuotas = new UDTSQL_int();
            this.PortafolioID = new UDT_PortafolioID();
            this.PrimeraCuota = new UDT_CuotaID();
            this.Rechazado = new UDT_SiNo();
            this.TipoCuenta = new UDTSQL_tinyint();
            this.VlrCapital = new UDT_Valor();
            this.VlrComision = new UDT_Valor();

            this.VlrNominal = new UDT_Valor();
            this.VlrPagar = new UDT_Valor();
            this.VlrRecompra = new UDT_Valor();
            this.VlrSaldo = new UDT_Valor();
            this.VlrUtilidad = new UDT_Valor();
            this.VlrVenta = new UDT_Valor();
            this.Detalle = new List<DTO_ccCreditoComponentes>();
            this.DetallePagosBeneficiarios = new List<DTO_ccSolicitudDetallePago>();
            this.FechaUltPago = new UDTSQL_smalldatetime();
            this.DiasMora = new UDTSQL_int();
            this.VlrSaldoSeguro = new UDT_Valor();
            this.VlrSaldoVencido = new UDT_Valor();
            this.VlrSaldoOtros = new UDT_Valor();
            this.VlrIntCapital = new UDT_Valor();
            this.VlrIntPoliza = new UDT_Valor();
            this.VlrGastos = new UDT_Valor();
            this.VlrAbonado = new UDT_Valor();
            this.DetalleCJHistorico = new List<DTO_ccCJHistorico>();
            this.NumReincorpora = new UDTSQL_tinyint();
            this.Otro = new UDT_DescripTExt();
            this.Otro1 = new UDT_DescripTExt();
            this.Otro2 = new UDT_DescripTExt();
            this.ConsReinc = new UDT_Consecutivo();
            this.VlrProvGeneral = new UDT_Valor();
            this.VlrSdoCapital = new UDT_Valor();
            this.VlrSdoAsistencias = new UDT_Valor();
            this.VlrSdoOtros = new UDT_Valor();
            this.ComponenteExtraInd = new UDT_SiNo();
            this.NumDocHistoria = new UDT_Consecutivo();
            this.VlrRevoca = new UDT_Valor();
            this.VlrDiferencia = new UDT_Valor();
            //Consultas
            this.Cuotas = new List<DTO_ccCreditoPlanPagos>();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrIntSeguro = new UDT_Valor();
            this.VlrPrejuridico = new UDT_Valor();
            this.VlrMora = new UDT_Valor();
            this.VlrBasePublico = new UDT_Valor();
            this.VlrBasePrivado = new UDT_Valor();
        
            #endregion
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int index { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumSolicitud { get; set; }

        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 ConcesionarioID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 AseguradoraID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CooperativaID { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoID { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 TipoCreditoID { get; set; }
        
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
        public UDT_SiNo IncorporaMesInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint IncorporacionTipo { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocVerificado { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocOpera { get; set; }

        [DataMember]
        public UDT_AsesorID Abogado { get; set; }

        [DataMember]
        public UDTSQL_char EtapaIncumplimiento { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaEstadoID { get; set; } 

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestionID { get; set; } //

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestionCierre { get; set; } 

        [DataMember]
        public UDT_DescripTExt ObsCobranza { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoPago { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDT_SiNo IndRestructurado { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaEstado { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Ciudad { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCompra { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID VendedorID { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorFinalID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCredito { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID TasaEfectivaCredito { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID TasaEfectivaVenta { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID TasaEfectivaReCompra { get; set; }

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
        public UDT_SiNo CompraCarteraInd { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrAdicional { get; set; }

        [DataMember]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrCompra { get; set; }

        [DataMember]
        public UDT_Valor VlrDescuento { get; set; }

        [DataMember]
        public UDT_Valor VlrGiro { get; set; }

        [DataMember]
        public UDT_Valor VlrMaximoNivelRiesgo { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCupoDisponible { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo PagoVentanillaInd { get; set; }

        [DataMember]
        public UDT_SiNo BloqueaVentaInd { get; set; }

        [DataMember]
        public UDT_SiNo VendidaInd { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

        [DataMember]
        public UDT_Consecutivo DocSustituye { get; set; }

        [DataMember]
        public UDT_Consecutivo DocVenta { get; set; }

        [DataMember]
        public UDT_Consecutivo DocFactura { get; set; }

        [DataMember]
        public UDT_Consecutivo DocDesestimiento { get; set; }

        [DataMember]
        public UDT_Consecutivo DocRechazo { get; set; }

        [DataMember]
        public UDTSQL_char NumeroCesion { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoVenta { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumIncorporaDoc { get; set; }

        [DataMember]
        public UDT_SiNo SustituidoInd { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDesIncorporaDoc { get; set; }

        [DataMember]
        public UDT_SiNo CanceladoInd { get; set; }

        [DataMember]
        public UDT_Consecutivo DocAcuerdoPago { get; set; }

        [DataMember]
        public UDT_Consecutivo DocPrepago { get; set; }

        [DataMember]
        public UDT_Valor VlrPrepago { get; set; }

        [DataMember]
        public UDT_Consecutivo DocEstadoCuenta { get; set; }

        [DataMember]
        public UDTSQL_int Solicitud { get; set; }

        [DataMember]
        public UDTSQL_char Pagare { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char PagarePOL { get; set; }

        [DataMember]
        public UDTSQL_char Poliza { get; set; }
        
        [DataMember]
        public UDTSQL_char DigitoVer1 { get; set; }

        [DataMember]
        public UDTSQL_char DigitoVer2 { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumeroINC { get; set; }

        [DataMember]
        public UDTSQL_smallint PlazoSeguro { get; set; }

        [DataMember]
        public UDTSQL_smallint Cuota1Seguro { get; set; }

        [DataMember]
        public UDT_Valor VlrCuotaSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrFinanciaSeguro { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaRestructura { get; set; }

        [DataMember]
        public UDT_Valor VlrPoliza { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiqSeguro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVigenciaINI { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVigenciaFIN { get; set; }

        [DataMember]
        public UDT_Consecutivo DocSeguro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaReliquidaCJ { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo DocUltNomina { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo DocUltRecaudo { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char NovedadIncorporaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl5 SiniestroEstadoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo ComponenteExtraInd { get; set; }

        [DataMember]
        public UDTSQL_char Sentencia { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaSentencia { get; set; }

        [DataMember]
        public UDTSQL_char Juzgado { get; set; }

        [DataMember]
        public UDT_Valor VlrCuotaSentencia { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1Sentencia { get; set; }

        [DataMember]
        public UDTSQL_smallint PlazoSentencia { get; set; }    

        #endregion

        #region Campos Adicionales

        [DataMember]
        [NotImportable]
        public UDT_SiNo Editable { get; set; }

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        public UDT_Consecutivo CreditoCuotaNum { get; set; }

        [DataMember]
        public UDTSQL_int CuotasMora { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_BancoID BancoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescBanco { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime EC_Fecha { get; set; }

        [DataMember]
        public UDT_SiNo EC_FijadoInd { get; set; }

        [DataMember]
        public UDT_CuotaID EC_PrimeraCtaPagada { get; set; }

        [DataMember]
        public UDTSQL_tinyint EC_Proposito { get; set; }

        [DataMember]
        public UDT_Valor EC_ValorPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint EC_PolizaMvto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocProceso { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaIncorpora { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaDesIncorpora { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaPagoParcial { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public UDT_SiNo IsPreventa { get; set; }

        [DataMember]
        public UDT_LibranzaID LibranzaSustituye { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocSustituye { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDTSQL_char NumCuenta { get; set; }

        [DataMember]
        public UDTSQL_int NumCuotas { get; set; }

        [DataMember]
        public UDT_PortafolioID PortafolioID { get; set; }

        [DataMember]
        public UDT_CuotaID PrimeraCuota { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCuenta { get; set; }

        [DataMember]
        public UDT_Valor VlrComision { get; set; }

        [DataMember]
        public UDT_Valor VlrBasePrivado { get; set; }

        [DataMember]
        public UDT_Valor VlrBasePublico { get; set; }
                
        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrNominal { get; set; }

        [DataMember]
        public UDT_Valor VlrPagar { get; set; }

        [DataMember]
        public UDT_Valor VlrRecompra { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        public UDT_Valor VlrUtilidad { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public List<DTO_ccCreditoComponentes> Detalle { get; set; }

        [DataMember]
        public List<DTO_ccSolicitudDetallePago> DetallePagosBeneficiarios { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaUltPago { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoVencido { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoOtros { get; set; }

        [DataMember]
        public UDT_Valor VlrIntCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrIntPoliza { get; set; }

        [DataMember]
        public UDT_Valor VlrGastos { get; set; }

        [DataMember]
        public UDT_Valor VlrAbonado { get; set; }

        [DataMember]
        public List<DTO_ccCJHistorico> DetalleCJHistorico { get; set; }

        [DataMember]
        public UDTSQL_char CategoriaRestructura { get; set; }

        [DataMember]
        public UDTSQL_tinyint NumReincorpora { get; set; }

        [DataMember]
        public UDT_DescripTExt Otro { get; set; }

        [DataMember]
        public UDT_DescripTExt Otro1 { get; set; }

        [DataMember]
        public UDT_DescripTExt Otro2 { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsReinc { get; set; }

        [DataMember]
        public UDT_Valor VlrRevoca { get; set; }

        [DataMember]
        public UDT_Valor VlrDiferencia { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocHistoria { get; set; }

        //Para Preventa
        [DataMember]
        public UDT_Valor VlrProvGeneral { get; set; }

        [DataMember]
        public UDT_Valor VlrSdoCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrSdoAsistencias { get; set; }

        [DataMember]
        public UDT_Valor VlrSdoOtros { get; set; }

        //Consultas
        [DataMember]
        public List<DTO_ccCreditoPlanPagos> Cuotas { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrIntSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrMora { get; set; }

        [DataMember]
        public UDT_Valor VlrPrejuridico { get; set; }

        #endregion

    }
}
