using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_drSolicitudDatosVehiculo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drSolicitudDatosVehiculo
    {
        #region drSolicitudDatosVehiculo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drSolicitudDatosVehiculo(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value=Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToByte(dr["Version"]);
                this.GarantiaID.Value = Convert.ToString(dr["GarantiaID"]);
                if (!string.IsNullOrWhiteSpace(dr["Placa"].ToString())) 
                    this.Placa.Value = Convert.ToString(dr["Placa"]);
                if (!string.IsNullOrWhiteSpace(dr["DocPrenda"].ToString())) 
                    this.DocPrenda.Value = Convert.ToByte(dr["DocPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["OrigenDocumento"].ToString())) 
                    this.OrigenDocumento.Value = Convert.ToString(dr["OrigenDocumento"]);
                if (!string.IsNullOrWhiteSpace(dr["PrefijoPrenda"].ToString())) 
                    this.PrefijoPrenda.Value = Convert.ToString(dr["PrefijoPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroPrenda"].ToString()))
                    this.NumeroPrenda.Value = Convert.ToInt32(dr["NumeroPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoPrenda"].ToString())) 
                    this.TipoPrenda.Value = Convert.ToByte(dr["TipoPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["Garante"].ToString())) 
                    this.Garante.Value = Convert.ToByte(dr["Garante"]);
                if (!string.IsNullOrWhiteSpace(dr["Marca"].ToString())) 
                    this.Marca.Value = Convert.ToString(dr["Marca"]);
                if (!string.IsNullOrWhiteSpace(dr["Linea"].ToString())) 
                    this.Linea.Value = Convert.ToString(dr["Linea"]);
                if (!string.IsNullOrWhiteSpace(dr["Referencia"].ToString())) 
                    this.Referencia.Value = Convert.ToString(dr["Referencia"]);
                if (!string.IsNullOrWhiteSpace(dr["Cilindraje"].ToString())) 
                    this.Cilindraje.Value = Convert.ToInt32(dr["Cilindraje"]);
                if (!string.IsNullOrWhiteSpace(dr["Tipocaja"].ToString())) 
                    this.Tipocaja.Value = Convert.ToByte(dr["Tipocaja"]);
                if (!string.IsNullOrWhiteSpace(dr["Complemento"].ToString())) 
                    this.Complemento.Value = Convert.ToString(dr["Complemento"]);
                if (!string.IsNullOrWhiteSpace(dr["AireAcondicionado"].ToString()))
                    this.AireAcondicionado.Value = Convert.ToBoolean(dr["AireAcondicionado"]);
                else
                    this.AireAcondicionado.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["PuertasNro"].ToString()))
                    this.PuertasNro.Value = Convert.ToByte(dr["PuertasNro"]);
                if (!string.IsNullOrWhiteSpace(dr["Termoking"].ToString()))
                    this.Termoking.Value = Convert.ToBoolean(dr["Termoking"]);
                else
                    this.Termoking.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["FasecoldaCod"].ToString()))
                    this.FasecoldaCod.Value = Convert.ToString(dr["FasecoldaCod"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFasecolda"].ToString()))
                    this.VlrFasecolda.Value = Convert.ToDecimal(dr["VlrFasecolda"]);
                if (!string.IsNullOrWhiteSpace(dr["Carroceria"].ToString()))
                    this.Carroceria.Value = Convert.ToString(dr["Carroceria"]);
                if (!string.IsNullOrWhiteSpace(dr["Peso"].ToString())) 
                    this.Peso.Value = Convert.ToInt32(dr["Peso"]);
                if (!string.IsNullOrWhiteSpace(dr["Servicio"].ToString()))
                    this.Servicio.Value = Convert.ToByte(dr["Servicio"]);
                if (!string.IsNullOrWhiteSpace(dr["CeroKmInd"].ToString()))
                    this.CeroKmInd.Value = Convert.ToBoolean(dr["CeroKmInd"]);
                else
                    this.CeroKmInd.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["Modelo"].ToString())) 
                    this.Modelo.Value = Convert.ToInt32(dr["Modelo"]);
                if (!string.IsNullOrWhiteSpace(dr["PrecioVenta"].ToString())) 
                    this.PrecioVenta.Value = Convert.ToDecimal(dr["PrecioVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["PrecioVentaChasis"].ToString()))
                    this.PrecioVentaChasis.Value = Convert.ToDecimal(dr["PrecioVentaChasis"]);
                if (!string.IsNullOrWhiteSpace(dr["PrecioVentaComplemento"].ToString()))
                    this.PrecioVentaComplemento.Value = Convert.ToDecimal(dr["PrecioVentaComplemento"]);

                if (!string.IsNullOrWhiteSpace(dr["CuotaInicial"].ToString())) 
                    this.CuotaInicial.Value = Convert.ToDecimal(dr["CuotaInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["Registrada"].ToString())) 
                    this.Registrada.Value = Convert.ToInt32(dr["Registrada"]);
                if (!string.IsNullOrWhiteSpace(dr["ChasisYComplementoIND"].ToString()))
                    this.ChasisYComplementoIND.Value = Convert.ToBoolean(dr["ChasisYComplementoIND"]);
                else
                    this.ChasisYComplementoIND.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["Motor"].ToString())) 
                    this.Motor.Value = Convert.ToString(dr["Motor"]);
                if (!string.IsNullOrWhiteSpace(dr["Serie"].ToString())) 
                    this.Serie.Value = Convert.ToString(dr["Serie"]);
                if (!string.IsNullOrWhiteSpace(dr["Chasis"].ToString())) 
                    this.Chasis.Value = Convert.ToString(dr["Chasis"]);
                if (!string.IsNullOrWhiteSpace(dr["Clase"].ToString()))
                    this.Clase.Value = Convert.ToString(dr["Clase"]);

                if (!string.IsNullOrWhiteSpace(dr["Color"].ToString())) 
                    this.Color.Value = Convert.ToString(dr["Color"]);
                if (!string.IsNullOrWhiteSpace(dr["Tipo"].ToString())) 
                    this.Tipo.Value = Convert.ToString(dr["Tipo"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroFactura"].ToString())) 
                    this.NumeroFactura.Value = Convert.ToString(dr["NumeroFactura"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoFactura"].ToString())) 
                    this.TipoFactura.Value = Convert.ToString(dr["TipoFactura"]);
                if (!string.IsNullOrWhiteSpace(dr["InmuebleTipo"].ToString())) 
                    this.InmuebleTipo.Value = Convert.ToByte(dr["InmuebleTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["Matricula"].ToString())) 
                    this.Matricula.Value = Convert.ToString(dr["Matricula"]);
                if (!string.IsNullOrWhiteSpace(dr["Direccion"].ToString())) 
                    this.Direccion.Value = Convert.ToString(dr["Direccion"]);
                if (!string.IsNullOrWhiteSpace(dr["Ano"].ToString())) 
                    this.Ano.Value = Convert.ToInt32(dr["Ano"]);
                if (!string.IsNullOrWhiteSpace(dr["FuenteHIP"].ToString())) 
                    this.FuenteHIP.Value = Convert.ToByte(dr["FuenteHIP"]);
                if (!string.IsNullOrWhiteSpace(dr["CodigoGarantia"].ToString())) 
                    this.CodigoGarantia.Value = Convert.ToString(dr["CodigoGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFuente"].ToString())) 
                    this.VlrFuente.Value = Convert.ToDecimal(dr["VlrFuente"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFuente"].ToString()))
                    this.FechaFuente.Value = Convert.ToDateTime(dr["FechaFuente"]);
                if (!string.IsNullOrWhiteSpace(dr["CodigoGarantia1"].ToString()))
                    this.CodigoGarantia1.Value = Convert.ToString(dr["CodigoGarantia1"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGarantia"].ToString())) 
                    this.VlrGarantia.Value = Convert.ToDecimal(dr["VlrGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["IndValidado"].ToString()))
                    this.IndValidado.Value = Convert.ToBoolean(dr["IndValidado"]);
                else
                    this.IndValidado.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndValidaHipoteca"].ToString()))
                    this.IndValidaHipoteca.Value = Convert.ToBoolean(dr["IndValidaHipoteca"]);
                else
                    this.IndValidaHipoteca.Value = false;
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrWhiteSpace(dr["RUNT"].ToString()))
                    this.RUNT.Value = Convert.ToString(dr["RUNT"]);
                if (!string.IsNullOrWhiteSpace(dr["Confecamaras"].ToString()))
                    this.Confecamaras.Value = Convert.ToString(dr["Confecamaras"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVTO"].ToString()))
                    this.FechaVTO.Value = Convert.ToDateTime(dr["FechaVTO"]);

                this.Ciudad.Value = Convert.ToString(dr["Ciudad"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPredial"].ToString()))
                    this.FechaPredial.Value = Convert.ToDateTime(dr["FechaPredial"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorPredial"].ToString()))
                    this.ValorPredial.Value = Convert.ToDecimal(dr["ValorPredial"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAvaluo"].ToString()))
                    this.FechaAvaluo.Value = Convert.ToDateTime(dr["FechaAvaluo"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorAvaluo"].ToString()))
                    this.ValorAvaluo.Value = Convert.ToDecimal(dr["ValorAvaluo"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPromesa"].ToString()))
                    this.FechaPromesa.Value = Convert.ToDateTime(dr["FechaPromesa"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorCompraventa"].ToString()))
                    this.ValorCompraventa.Value = Convert.ToDecimal(dr["ValorCompraventa"]);

                if (!string.IsNullOrWhiteSpace(dr["ViviendaNuevaInd"].ToString()))
                    this.ViviendaNuevaInd.Value = Convert.ToBoolean(dr["ViviendaNuevaInd"]);
                else
                    this.ViviendaNuevaInd.Value = false;

                #region campos nuevos

                if (!string.IsNullOrWhiteSpace(dr["PrefijoHipoteca"].ToString()))
                    this.PrefijoHipoteca.Value = Convert.ToString(dr["PrefijoHipoteca"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroHipoteca"].ToString())) 
                    this.NumeroHipoteca.Value = Convert.ToInt32(dr["NumeroHipoteca"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicio"].ToString())) 
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorPoliza"].ToString()))
                    this.ValorPoliza.Value = Convert.ToDecimal(dr["ValorPoliza"]);
                if (!string.IsNullOrWhiteSpace(dr["Escritura"].ToString())) 
                    this.Escritura.Value = Convert.ToString(dr["Escritura"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaEscritura"].ToString())) 
                    this.FechaEscritura.Value = Convert.ToDateTime(dr["FechaEscritura"]);
                if (!string.IsNullOrWhiteSpace(dr["Notaria"].ToString())) 
                    this.Notaria.Value = Convert.ToString(dr["Notaria"]);
                if (!string.IsNullOrWhiteSpace(dr["Marca_2"].ToString())) 
                    this.Marca_2.Value = Convert.ToString(dr["Marca_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Linea_2"].ToString())) 
                    this.Linea_2.Value = Convert.ToString(dr["Linea_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Referencia_2"].ToString())) 
                    this.Referencia_2.Value = Convert.ToString(dr["Referencia_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Cilindraje_2"].ToString())) 
                    this.Cilindraje_2.Value = Convert.ToInt32(dr["Cilindraje_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Tipocaja_2"].ToString())) 
                    this.Tipocaja_2.Value = Convert.ToByte(dr["Tipocaja_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Complemento_2"].ToString())) 
                    this.Complemento_2.Value = Convert.ToString(dr["Complemento_2"]);
                if (!string.IsNullOrWhiteSpace(dr["AireAcondicionado_2"].ToString()))
                    this.AireAcondicionado_2.Value = Convert.ToBoolean(dr["AireAcondicionado_2"]);
                else
                    this.AireAcondicionado_2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["PuertasNro_2"].ToString())) 
                    this.PuertasNro_2.Value = Convert.ToByte(dr["PuertasNro_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Termoking_2"].ToString())) 
                    this.Termoking_2.Value = Convert.ToBoolean(dr["Termoking_2"]);
                else
                    this.Termoking_2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["FasecoldaCod_2"].ToString()))
                    this.FasecoldaCod_2.Value = Convert.ToString(dr["FasecoldaCod_2"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFasecolda_2"].ToString()))
                    this.VlrFasecolda_2.Value = Convert.ToDecimal(dr["VlrFasecolda_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Carroceria_2"].ToString())) 
                    this.Carroceria_2.Value = Convert.ToString(dr["Carroceria_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Peso_2"].ToString())) 
                    this.Peso_2.Value = Convert.ToInt32(dr["Peso_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Servicio_2"].ToString())) 
                    this.Servicio_2.Value = Convert.ToByte(dr["Servicio_2"]);
                if (!string.IsNullOrWhiteSpace(dr["CeroKmInd_2"].ToString()))
                    this.CeroKmInd_2.Value = Convert.ToBoolean(dr["CeroKmInd_2"]);
                else
                    this.CeroKmInd_2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["Modelo_2"].ToString())) 
                    this.Modelo_2.Value = Convert.ToInt32(dr["Modelo_2"]);
                if (!string.IsNullOrWhiteSpace(dr["PrecioVenta_2"].ToString())) 
                    this.PrecioVenta_2.Value = Convert.ToDecimal(dr["PrecioVenta_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Registrada_2"].ToString())) 
                    this.Registrada_2.Value = Convert.ToInt32(dr["Registrada_2"]);
                if (!string.IsNullOrWhiteSpace(dr["ChasisYComplementoIND_2"].ToString()))
                    this.ChasisYComplementoIND_2.Value = Convert.ToBoolean(dr["ChasisYComplementoIND_2"]);
                else
                    this.ChasisYComplementoIND_2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndValidado_2"].ToString()))
                    this.IndValidado_2.Value = Convert.ToBoolean(dr["IndValidado_2"]);
                else
                    this.IndValidado_2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["PrefijoPrenda_2"].ToString()))
                    this.PrefijoPrenda_2.Value = Convert.ToString(dr["PrefijoPrenda_2"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroPrenda_2"].ToString())) 
                    this.NumeroPrenda_2.Value = Convert.ToInt32(dr["NumeroPrenda_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Garante_2"].ToString())) 
                    this.Garante_2.Value = Convert.ToByte(dr["Garante_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Placa_2"].ToString())) 
                    this.Placa_2.Value = Convert.ToString(dr["Placa_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Motor_2"].ToString())) 
                    this.Motor_2.Value = Convert.ToString(dr["Motor_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Serie_2"].ToString())) 
                    this.Serie_2.Value = Convert.ToString(dr["Serie_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Chasis_2"].ToString()))
                    this.Chasis_2.Value = Convert.ToString(dr["Chasis_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Clase_2"].ToString())) 
                    this.Clase_2.Value = Convert.ToString(dr["Clase_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Color_2"].ToString())) 
                    this.Color_2.Value = Convert.ToString(dr["Color_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Tipo_2"].ToString())) 
                    this.Tipo_2.Value = Convert.ToString(dr["Tipo_2"]);
                if (!string.IsNullOrWhiteSpace(dr["DocPrenda_2"].ToString()))
                    this.DocPrenda_2.Value = Convert.ToByte(dr["DocPrenda_2"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroFactura_2"].ToString()))
                    this.NumeroFactura_2.Value = Convert.ToString(dr["NumeroFactura_2"]);
                if (!string.IsNullOrWhiteSpace(dr["InmuebleTipo_2"].ToString())) 
                    this.InmuebleTipo_2.Value = Convert.ToByte(dr["InmuebleTipo_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Direccion_2"].ToString())) 
                    this.Direccion_2.Value = Convert.ToString(dr["Direccion_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Ciudad_2"].ToString())) 
                    this.Ciudad_2.Value = Convert.ToString(dr["Ciudad_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Matricula_2"].ToString())) 
                    this.Matricula_2.Value = Convert.ToString(dr["Matricula_2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAvaluo_2"].ToString())) 
                    this.FechaAvaluo_2.Value = Convert.ToDateTime(dr["FechaAvaluo_2"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorAvaluo_2"].ToString())) 
                    this.ValorAvaluo_2.Value = Convert.ToDecimal(dr["ValorAvaluo_2"]);
                if (!string.IsNullOrWhiteSpace(dr["ViviendaNuevaInd_2"].ToString()))
                    this.ViviendaNuevaInd_2.Value = Convert.ToBoolean(dr["ViviendaNuevaInd_2"]);
                else
                    this.ViviendaNuevaInd_2.Value = false;

                if (!string.IsNullOrWhiteSpace(dr["FechaPredial_2"].ToString())) 
                    this.FechaPredial_2.Value = Convert.ToDateTime(dr["FechaPredial_2"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorPredial_2"].ToString())) 
                    this.ValorPredial_2.Value = Convert.ToDecimal(dr["ValorPredial_2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPromesa_2"].ToString())) 
                    this.FechaPromesa_2.Value = Convert.ToDateTime(dr["FechaPromesa_2"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorCompraventa_2"].ToString())) 
                    this.ValorCompraventa_2.Value = Convert.ToDecimal(dr["ValorCompraventa_2"]);
                if (!string.IsNullOrWhiteSpace(dr["IndValidaHipoteca_2"].ToString()))
                    this.IndValidaHipoteca_2.Value = Convert.ToBoolean(dr["IndValidaHipoteca_2"]);
                else
                    this.IndValidaHipoteca_2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["PrefijoHipoteca_2"].ToString())) 
                    this.PrefijoHipoteca_2.Value = Convert.ToString(dr["PrefijoHipoteca_2"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroHipoteca_2"].ToString())) 
                    this.NumeroHipoteca_2.Value = Convert.ToInt32(dr["NumeroHipoteca_2"]);
                if (!string.IsNullOrWhiteSpace(dr["RUNT_2"].ToString())) 
                    this.RUNT_2.Value = Convert.ToString(dr["RUNT_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Confecamaras_2"].ToString())) 
                    this.Confecamaras_2.Value = Convert.ToString(dr["Confecamaras_2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRegistro_2"].ToString()))
                    this.FechaRegistro_2.Value = Convert.ToDateTime(dr["FechaRegistro_2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicio_2"].ToString())) 
                    this.FechaInicio_2.Value = Convert.ToDateTime(dr["FechaInicio_2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVto_2"].ToString())) 
                    this.FechaVto_2.Value = Convert.ToDateTime(dr["FechaVto_2"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorPoliza_2"].ToString()))
                    this.ValorPoliza_2.Value = Convert.ToDecimal(dr["ValorPoliza_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Escritura_2"].ToString())) 
                    this.Escritura_2.Value = Convert.ToString(dr["Escritura_2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaEscritura_2"].ToString())) 
                    this.FechaEscritura_2.Value = Convert.ToDateTime(dr["FechaEscritura_2"]);
                if (!string.IsNullOrWhiteSpace(dr["Notaria_2"].ToString())) 
                    this.Notaria_2.Value = Convert.ToString(dr["Notaria_2"]);
                if (!string.IsNullOrWhiteSpace(dr["PagareCRD"].ToString())) 
                    this.PagareCRD.Value = Convert.ToString(dr["PagareCRD"]);
                if (!string.IsNullOrWhiteSpace(dr["PagarePOL"].ToString())) 
                    this.PagarePOL.Value = Convert.ToString(dr["PagarePOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PolizaVEH1"].ToString()))
                    this.PolizaVEH1.Value = Convert.ToString(dr["PolizaVEH1"]);
                if (!string.IsNullOrWhiteSpace(dr["PolizaVEH2"].ToString()))
                    this.PolizaVEH2.Value = Convert.ToString(dr["PolizaVEH2"]);
                if (!string.IsNullOrWhiteSpace(dr["PolizaHIP1"].ToString()))
                    this.PolizaHIP1.Value = Convert.ToString(dr["PolizaHIP1"]);
                if (!string.IsNullOrWhiteSpace(dr["PolizaHIP2"].ToString()))
                    this.PolizaHIP2.Value = Convert.ToString(dr["PolizaHIP2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIniVEH1"].ToString()))
                    this.FechaIniVEH1.Value = Convert.ToDateTime(dr["FechaIniVEH1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinVEH1"].ToString()))
                    this.FechaFinVEH1.Value = Convert.ToDateTime(dr["FechaFinVEH1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIniVEH2"].ToString()))
                    this.FechaIniVEH2.Value = Convert.ToDateTime(dr["FechaIniVEH2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinVEH2"].ToString()))
                    this.FechaFinVEH2.Value = Convert.ToDateTime(dr["FechaFinVEH2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIniHIP1"].ToString()))
                    this.FechaIniHIP1.Value = Convert.ToDateTime(dr["FechaIniHIP1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinHIP1"].ToString()))
                    this.FechaFinHIP1.Value = Convert.ToDateTime(dr["FechaFinHIP1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIniHIP2"].ToString()))
                    this.FechaIniHIP2.Value = Convert.ToDateTime(dr["FechaIniHIP2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFinHIP2"].ToString()))
                    this.FechaFinHIP2.Value = Convert.ToDateTime(dr["FechaFinHIP2"]);
                if (!string.IsNullOrWhiteSpace(dr["Aseguradora1VEH"].ToString())) 
                    this.Aseguradora1VEH.Value = Convert.ToString(dr["Aseguradora1VEH"]);
                if (!string.IsNullOrWhiteSpace(dr["Aseguradora2VEH"].ToString())) 
                    this.Aseguradora2VEH.Value = Convert.ToString(dr["Aseguradora2VEH"]);
                if (!string.IsNullOrWhiteSpace(dr["Aseguradora1HIP"].ToString())) 
                    this.Aseguradora1HIP.Value = Convert.ToString(dr["Aseguradora1HIP"]);
                if (!string.IsNullOrWhiteSpace(dr["Aseguradora2HIP"].ToString())) 
                    this.Aseguradora2HIP.Value = Convert.ToString(dr["Aseguradora2HIP"]);

                if (!string.IsNullOrWhiteSpace(dr["VlrPolizaVEH1"].ToString())) 
                    this.VlrPolizaVEH1.Value = Convert.ToDecimal(dr["VlrPolizaVEH1"]);
                if (!string.IsNullOrWhiteSpace(dr["CancelaContadoPolizaIndVEH1"].ToString()))
                    this.CancelaContadoPolizaIndVEH1.Value = Convert.ToBoolean(dr["CancelaContadoPolizaIndVEH1"]);
                else
                    this.CancelaContadoPolizaIndVEH1.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IntermediarioExternoIndVEH1"].ToString()))
                    this.IntermediarioExternoIndVEH1.Value = Convert.ToBoolean(dr["IntermediarioExternoIndVEH1"]);
                else
                    this.IntermediarioExternoIndVEH1.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["VlrPolizaVEH2"].ToString())) 
                    this.VlrPolizaVEH2.Value = Convert.ToDecimal(dr["VlrPolizaVEH2"]);
                if (!string.IsNullOrWhiteSpace(dr["CancelaContadoPolizaIndVEH2"].ToString()))
                    this.CancelaContadoPolizaIndVEH2.Value = Convert.ToBoolean(dr["CancelaContadoPolizaIndVEH2"]);
                else
                    this.CancelaContadoPolizaIndVEH2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IntermediarioExternoIndVEH2"].ToString()))
                    this.IntermediarioExternoIndVEH2.Value = Convert.ToBoolean(dr["IntermediarioExternoIndVEH2"]);
                else
                    this.IntermediarioExternoIndVEH2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["VlrPolizaHIP1"].ToString())) 
                    this.VlrPolizaHIP1.Value = Convert.ToDecimal(dr["VlrPolizaHIP1"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPolizaHIP2"].ToString())) 
                    this.VlrPolizaHIP2.Value = Convert.ToDecimal(dr["VlrPolizaHIP2"]);

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
        public DTO_drSolicitudDatosVehiculo()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {   
            this.NumeroDoc = new UDT_Consecutivo(); 
            this.Version = new UDTSQL_int();
            this.GarantiaID = new UDT_CodigoGrl10();
            this.Placa = new UDTSQL_char(6);
            this.DocPrenda = new UDTSQL_tinyint();            
            this.OrigenDocumento = new UDTSQL_char(50);
            this.PrefijoPrenda = new UDT_PrefijoID();
            this.NumeroPrenda = new UDTSQL_int();
            this.TipoPrenda = new UDTSQL_tinyint();
            this.Garante = new UDTSQL_tinyint();
            this.Marca = new UDTSQL_varchar(100);
            this.Linea = new UDTSQL_varchar(100);
            this.Referencia = new UDTSQL_varchar(100);
            this.Cilindraje = new UDTSQL_int ();
            this.Tipocaja = new UDTSQL_tinyint();
            this.Complemento = new UDTSQL_char(30);
            this.AireAcondicionado = new UDT_SiNo();
            this.PuertasNro = new UDTSQL_tinyint();
            this.Termoking = new UDT_SiNo();
            this.FasecoldaCod = new UDT_CodigoGrl10();
            this.VlrFasecolda = new UDT_Valor();
            this.Carroceria = new UDTSQL_varchar(30);
            this.Peso = new UDTSQL_int();
            this.Servicio = new UDTSQL_tinyint();
            this.CeroKmInd = new UDT_SiNo();
            this.Modelo = new UDTSQL_int();
            this.PrecioVenta= new UDT_Valor();
            this.PrecioVentaChasis = new UDT_Valor();
            this.PrecioVentaComplemento = new UDT_Valor();
            this.CuotaInicial = new UDT_Valor();
            this.Registrada = new UDTSQL_int();
            this.ChasisYComplementoIND = new UDT_SiNo();
            this.Motor = new UDTSQL_char(30);
            this.Serie = new UDTSQL_char(30);
            this.Chasis = new UDTSQL_char(30);
            this.Clase = new UDTSQL_char(30);
            this.Color = new UDTSQL_char(30);
            this.Tipo = new UDTSQL_char(30);
            this.NumeroFactura = new UDTSQL_char(30);
            this.TipoFactura = new UDTSQL_char(30);
            this.InmuebleTipo = new UDTSQL_tinyint();
            this.Matricula = new UDT_CodigoGrl20();
            this.Direccion = new UDT_DescripTBase();
            this.Ano = new UDTSQL_int();
            this.FuenteHIP = new UDTSQL_tinyint();
            this.CodigoGarantia = new UDT_CodigoGrl20();
            this.VlrFuente = new UDT_Valor();
            this.FechaFuente = new UDTSQL_smalldatetime();
            this.CodigoGarantia1 = new UDT_CodigoGrl20();
            this.VlrGarantia = new UDT_Valor();
            this.IndValidado = new UDT_SiNo();
            this.IndValidaHipoteca = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
            this.RUNT = new UDTSQL_char(50);
            this.Confecamaras = new UDTSQL_char(50);
            this.Ciudad = new UDTSQL_char(50);
            this.FechaVTO = new UDTSQL_smalldatetime();
            this.FechaPredial= new UDTSQL_smalldatetime();
            this.ValorPredial = new UDT_Valor();
            this.FechaAvaluo= new UDTSQL_smalldatetime();
            this.ValorAvaluo = new UDT_Valor();
            this.FechaPromesa = new  UDTSQL_smalldatetime();
            this.ValorCompraventa= new UDT_Valor();
            this.ViviendaNuevaInd= new UDT_SiNo();
            /////
            this.PrefijoHipoteca=new UDT_PrefijoID();
            this.NumeroHipoteca=new UDTSQL_int ();
            this.FechaInicio=new UDTSQL_smalldatetime ();
            this.ValorPoliza=new UDT_Valor ();
            this.Escritura=new UDTSQL_char(10);
            this.FechaEscritura=new UDTSQL_smalldatetime ();
            this.Notaria=new UDTSQL_char(10);
            this.Marca_2=new UDTSQL_varchar (100);
            this.Linea_2=new UDTSQL_char(30);
            this.Referencia_2=new UDTSQL_varchar (100);
            this.Cilindraje_2=new UDTSQL_int ();
            this.Tipocaja_2=new UDTSQL_tinyint ();
            this.Complemento_2=new UDTSQL_char (30);
            this.AireAcondicionado_2=new UDT_SiNo();
            this.PuertasNro_2=new UDTSQL_tinyint ();
            this.Termoking_2=new UDT_SiNo();
            this.FasecoldaCod_2=new UDT_CodigoGrl10();
            this.VlrFasecolda_2=new UDT_Valor ();
            this.Carroceria_2=new UDTSQL_varchar(30);
            this.Peso_2=new UDTSQL_int ();
            this.Servicio_2=new UDTSQL_tinyint ();
            this.CeroKmInd_2=new UDT_SiNo();
            this.Modelo_2=new UDTSQL_int ();
            this.PrecioVenta_2=new UDT_Valor ();
            this.Registrada_2=new UDTSQL_int ();
            this.ChasisYComplementoIND_2=new UDT_SiNo();
            this.IndValidado_2=new UDT_SiNo();
            this.PrefijoPrenda_2=new UDT_PrefijoID();
            this.NumeroPrenda_2=new UDTSQL_int ();
            this.Garante_2=new UDTSQL_tinyint ();
            this.Placa_2=new UDTSQL_char (6);
            this.Motor_2=new UDTSQL_char (30);
            this.Serie_2=new UDTSQL_char (30);
            this.Chasis_2=new UDTSQL_char (30);
            this.Clase_2=new UDTSQL_char (30);
            this.Color_2=new UDTSQL_char (30);
            this.Tipo_2=new UDTSQL_char (30);
            this.DocPrenda_2=new UDTSQL_tinyint ();
            this.NumeroFactura_2=new UDTSQL_char (30);
            this.InmuebleTipo_2=new UDTSQL_tinyint ();
            this.Direccion_2=new UDT_DescripTBase();
            this.Ciudad_2=new UDTSQL_varchar (50);
            this.Matricula_2=new UDT_CodigoGrl20();
            this.FechaAvaluo_2=new UDTSQL_smalldatetime ();
            this.ValorAvaluo_2=new UDT_Valor ();
            this.ViviendaNuevaInd_2=new UDT_SiNo();
            this.FechaPredial_2=new UDTSQL_smalldatetime ();
            this.ValorPredial_2=new UDT_Valor ();
            this.FechaPromesa_2=new UDTSQL_smalldatetime ();
            this.ValorCompraventa_2=new UDT_Valor ();
            this.IndValidaHipoteca_2=new UDT_SiNo();
            this.PrefijoHipoteca_2=new UDT_PrefijoID();
            this.NumeroHipoteca_2=new UDTSQL_int ();
            this.RUNT_2=new UDTSQL_char (50);
            this.Confecamaras_2=new UDTSQL_char (50);
            this.FechaRegistro_2=new UDTSQL_smalldatetime ();
            this.FechaInicio_2=new UDTSQL_smalldatetime ();
            this.FechaVto_2=new UDTSQL_smalldatetime ();
            this.ValorPoliza_2=new UDT_Valor ();
            this.Escritura_2=new UDTSQL_varchar (10);
            this.FechaEscritura_2=new UDTSQL_smalldatetime ();
            this.Notaria_2=new UDTSQL_varchar (10);

            this.PagareCRD = new UDTSQL_char(15);
            this.PagarePOL = new UDTSQL_char(15);
            this.PolizaVEH1 = new UDTSQL_char(20);
            this.PolizaVEH2 = new UDTSQL_char(20);
            this.PolizaHIP1 = new UDTSQL_char(20);
            this.PolizaHIP2 = new UDTSQL_char(20);
            this.FechaIniVEH1 = new UDTSQL_smalldatetime();
            this.FechaFinVEH1 = new UDTSQL_smalldatetime();
            this.FechaIniVEH2 = new UDTSQL_smalldatetime();
            this.FechaFinVEH2 = new UDTSQL_smalldatetime();
            this.FechaIniHIP1 = new UDTSQL_smalldatetime();
            this.FechaFinHIP1 = new UDTSQL_smalldatetime();
            this.FechaIniHIP2 = new UDTSQL_smalldatetime();
            this.FechaFinHIP2 = new UDTSQL_smalldatetime();
            this.Aseguradora1VEH = new UDT_CodigoGrl10();
            this.Aseguradora2VEH = new UDT_CodigoGrl10();
            this.Aseguradora1HIP = new UDT_CodigoGrl10();
            this.Aseguradora2HIP = new UDT_CodigoGrl10();

            this.VlrPolizaVEH1 = new UDT_Valor();
            this.CancelaContadoPolizaIndVEH1 = new UDT_SiNo();
            this.IntermediarioExternoIndVEH1 = new UDT_SiNo();
            this.VlrPolizaVEH2 = new UDT_Valor();
            this.CancelaContadoPolizaIndVEH2 = new UDT_SiNo();
            this.IntermediarioExternoIndVEH2 = new UDT_SiNo();
            this.VlrPolizaHIP1 = new UDT_Valor();
            this.VlrPolizaHIP2 = new UDT_Valor();


        }

        #endregion

        #region Propiedades
        [DataMember]
        public  UDT_Consecutivo NumeroDoc{get;set;}
        [DataMember]
        public  UDTSQL_int Version{get;set;}
        [DataMember]
        public  UDT_CodigoGrl10 GarantiaID{get;set;}
        [DataMember]
        public  UDTSQL_char Placa{get;set;}
        [DataMember]
        public  UDTSQL_tinyint DocPrenda{get;set;}
        [DataMember]
        public  UDTSQL_char OrigenDocumento{get;set;}
        [DataMember]
        public  UDT_PrefijoID PrefijoPrenda{get;set;}
        [DataMember]
        public  UDTSQL_int NumeroPrenda{get;set;}
        [DataMember]
        public  UDTSQL_tinyint TipoPrenda{get;set;}
        [DataMember]
        public  UDTSQL_tinyint Garante{get;set;}
        [DataMember]
        public  UDTSQL_varchar Marca{get;set;}
        [DataMember]
        public  UDTSQL_varchar Linea{get;set;}
        [DataMember]
        public  UDTSQL_varchar Referencia{get;set;}
        [DataMember]
        public  UDTSQL_int Cilindraje{get;set;}
        [DataMember]
        public  UDTSQL_tinyint Tipocaja{get;set;}
        [DataMember]
        public  UDTSQL_char Complemento{get;set;}
        [DataMember]
        public  UDT_SiNo AireAcondicionado{get;set;}
        [DataMember]
        public  UDTSQL_tinyint PuertasNro{get;set;}
        [DataMember]
        public  UDT_SiNo Termoking{get;set;}
        [DataMember]
        public UDT_CodigoGrl10 FasecoldaCod { get; set; }
        [DataMember]
        public  UDT_Valor VlrFasecolda{get;set;}
        [DataMember]
        public  UDTSQL_varchar Carroceria{get;set;}
        [DataMember]
        public  UDTSQL_int Peso{get;set;}
        [DataMember]
        public  UDTSQL_tinyint Servicio{get;set;}
        [DataMember]
        public  UDT_SiNo CeroKmInd{get;set;}
        [DataMember]
        public  UDTSQL_int Modelo{get;set;}
        [DataMember]
        public  UDT_Valor PrecioVenta{get;set;}
        [DataMember]
        public  UDT_Valor PrecioVentaChasis{get;set;}
        [DataMember]
        public  UDT_Valor PrecioVentaComplemento{get;set;}
        [DataMember]
        public  UDT_Valor CuotaInicial{get;set;}
        [DataMember]
        public  UDTSQL_int Registrada{get;set;}
        [DataMember]
        public  UDT_SiNo ChasisYComplementoIND{get;set;}
        [DataMember]
        public  UDTSQL_char Motor{get;set;}
        [DataMember]
        public  UDTSQL_char Serie{get;set;}
        [DataMember]
        public  UDTSQL_char Chasis{get;set;}
        [DataMember]
        public UDTSQL_char Clase { get; set; }

        [DataMember]
        public  UDTSQL_char Color{get;set;}
        [DataMember]
        public  UDTSQL_char Tipo{get;set;}
        [DataMember]
        public  UDTSQL_char NumeroFactura{get;set;}
        [DataMember]
        public  UDTSQL_char TipoFactura{get;set;}
        [DataMember]
        public  UDTSQL_tinyint InmuebleTipo{get;set;}
        [DataMember]
        public UDT_CodigoGrl20 Matricula { get; set; }
        [DataMember]
        public  UDT_DescripTBase Direccion{get;set;}
        [DataMember]
        public  UDTSQL_int Ano{get;set;}
        [DataMember]
        public  UDTSQL_tinyint FuenteHIP{get;set;}
        [DataMember]
        public  UDT_CodigoGrl20 CodigoGarantia{get;set;}
        [DataMember]
        public  UDT_Valor VlrFuente{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaFuente{get;set;}
        [DataMember]
        public  UDT_CodigoGrl20 CodigoGarantia1{get;set;}
        [DataMember]
        public  UDT_Valor VlrGarantia{get;set;}
        [DataMember]
        public UDT_SiNo IndValidado { get; set; }        
        [DataMember]
        public UDT_SiNo IndValidaHipoteca { get; set; }        
        [DataMember]
        public  UDT_Consecutivo Consecutivo{get;set;}

        [DataMember]
        public UDTSQL_char RUNT { get; set; }
        [DataMember]
        public UDTSQL_char Confecamaras { get; set; }
        [DataMember]
        public UDTSQL_char Ciudad { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaVTO { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPredial { get; set; }
        [DataMember]
        public UDT_Valor ValorPredial { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaAvaluo { get; set; }
        [DataMember]
        public UDT_Valor ValorAvaluo { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaPromesa { get; set; }
        [DataMember]
        public UDT_Valor ValorCompraventa { get; set; }
        [DataMember]
        public UDT_SiNo ViviendaNuevaInd { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoHipoteca { get; set; }
        [DataMember]
        public UDTSQL_int NumeroHipoteca { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }
        [DataMember]
        public UDT_Valor ValorPoliza { get; set; }
        [DataMember]
        public UDTSQL_char Escritura { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaEscritura { get; set; }
        [DataMember]
        public UDTSQL_char Notaria { get; set; }

        [DataMember]
        public  UDTSQL_varchar Marca_2{get;set;}
        [DataMember]
        public  UDTSQL_char Linea_2{get;set;}
        [DataMember]
        public  UDTSQL_varchar Referencia_2{get;set;}
        [DataMember]
        public  UDTSQL_int Cilindraje_2{get;set;}
        [DataMember]
        public  UDTSQL_tinyint Tipocaja_2{get;set;}
        [DataMember]
        public  UDTSQL_char Complemento_2{get;set;}
        [DataMember]
        public  UDT_SiNo AireAcondicionado_2{get;set;}
        [DataMember]
        public  UDTSQL_tinyint PuertasNro_2{get;set;}
        [DataMember]
        public  UDT_SiNo Termoking_2{get;set;}
        [DataMember]
        public  UDT_CodigoGrl10 FasecoldaCod_2{get;set;}
        [DataMember]
        public  UDT_Valor VlrFasecolda_2{get;set;}
        [DataMember]
        public  UDTSQL_varchar Carroceria_2{get;set;}
        [DataMember]
        public  UDTSQL_int Peso_2{get;set;}
        [DataMember]
        public  UDTSQL_tinyint Servicio_2{get;set;}
        [DataMember]
        public  UDT_SiNo CeroKmInd_2{get;set;}
        [DataMember]
        public  UDTSQL_int Modelo_2{get;set;}
        [DataMember]
        public  UDT_Valor PrecioVenta_2{get;set;}
        [DataMember]
        public  UDTSQL_int Registrada_2{get;set;}
        [DataMember]
        public  UDT_SiNo ChasisYComplementoIND_2{get;set;}
        [DataMember]
        public  UDT_SiNo IndValidado_2{get;set;}
        [DataMember]
        public  UDT_PrefijoID PrefijoPrenda_2{get;set;}
        [DataMember]
        public  UDTSQL_int NumeroPrenda_2{get;set;}
        [DataMember]
        public  UDTSQL_tinyint Garante_2{get;set;}
        [DataMember]
        public  UDTSQL_char Placa_2{get;set;}
        [DataMember]
        public  UDTSQL_char Motor_2{get;set;}
        [DataMember]
        public  UDTSQL_char Serie_2{get;set;}
        [DataMember]
        public  UDTSQL_char Chasis_2{get;set;}
        [DataMember]
        public  UDTSQL_char Clase_2{get;set;}
        [DataMember]
        public  UDTSQL_char Color_2{get;set;}
        [DataMember]
        public  UDTSQL_char Tipo_2{get;set;}
        [DataMember]
        public  UDTSQL_tinyint DocPrenda_2{get;set;}
        [DataMember]
        public  UDTSQL_char NumeroFactura_2{get;set;}
        [DataMember]
        public  UDTSQL_tinyint InmuebleTipo_2{get;set;}
        [DataMember]
        public  UDT_DescripTBase Direccion_2{get;set;}
        [DataMember]
        public  UDTSQL_varchar Ciudad_2{get;set;}
        [DataMember]
        public  UDT_CodigoGrl20 Matricula_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaAvaluo_2{get;set;}
        [DataMember]
        public  UDT_Valor ValorAvaluo_2{get;set;}
        [DataMember]
        public  UDT_SiNo ViviendaNuevaInd_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaPredial_2{get;set;}
        [DataMember]
        public  UDT_Valor ValorPredial_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaPromesa_2{get;set;}
        [DataMember]
        public  UDT_Valor ValorCompraventa_2{get;set;}
        [DataMember]
        public  UDT_SiNo IndValidaHipoteca_2{get;set;}
        [DataMember]
        public  UDT_PrefijoID PrefijoHipoteca_2{get;set;}
        [DataMember]
        public  UDTSQL_int NumeroHipoteca_2{get;set;}
        [DataMember]
        public  UDTSQL_char RUNT_2{get;set;}
        [DataMember]
        public  UDTSQL_char Confecamaras_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaRegistro_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaInicio_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaVto_2{get;set;}
        [DataMember]
        public  UDT_Valor ValorPoliza_2{get;set;}
        [DataMember]
        public  UDTSQL_varchar Escritura_2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaEscritura_2{get;set;}
        [DataMember]
        public  UDTSQL_varchar Notaria_2{get;set;}
        [DataMember]
        public  UDTSQL_char PagareCRD{get;set;}
        [DataMember]
        public  UDTSQL_char PagarePOL{get;set;}
        [DataMember]
        public  UDTSQL_char PolizaVEH1{get;set;}
        [DataMember]
        public  UDTSQL_char PolizaVEH2{get;set;}
        [DataMember]
        public  UDTSQL_char PolizaHIP1{get;set;}
        [DataMember]
        public  UDTSQL_char PolizaHIP2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaIniVEH1{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaFinVEH1{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaIniVEH2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaFinVEH2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaIniHIP1{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaFinHIP1{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaIniHIP2{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaFinHIP2{get;set;}
        [DataMember]
        public  UDT_CodigoGrl10 Aseguradora1VEH{get;set;}
        [DataMember]
        public UDT_CodigoGrl10 Aseguradora2VEH { get; set; }
        [DataMember]
        public UDT_CodigoGrl10 Aseguradora1HIP { get; set; }
        [DataMember]
        public UDT_CodigoGrl10 Aseguradora2HIP { get; set; }

        [DataMember]
        public  UDT_Valor VlrPolizaVEH1{get;set;}
        [DataMember]
        public  UDT_SiNo CancelaContadoPolizaIndVEH1{get;set;}
        [DataMember]
        public  UDT_SiNo IntermediarioExternoIndVEH1{get;set;}
        [DataMember]
        public  UDT_Valor VlrPolizaVEH2{get;set;}
        [DataMember]
        public  UDT_SiNo CancelaContadoPolizaIndVEH2{get;set;}
        [DataMember]
        public  UDT_SiNo IntermediarioExternoIndVEH2{get;set;}
        [DataMember]
        public  UDT_Valor VlrPolizaHIP1{get;set;}
        [DataMember]
        public  UDT_Valor VlrPolizaHIP2{get;set;}

        #endregion
    }
}
