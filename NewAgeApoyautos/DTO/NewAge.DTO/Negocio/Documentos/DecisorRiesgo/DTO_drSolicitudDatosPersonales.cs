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
    /// Models DTO_drSolicitudDatosPersonales
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drSolicitudDatosPersonales
    {
        #region drSolicitudDatosPersonales

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drSolicitudDatosPersonales(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToByte(dr["Version"]);
                this.TipoPersona.Value = Convert.ToByte(dr["TipoPersona"]);
                
                this.TerceroID.Value=Convert.ToString(dr["TerceroID"]);
                this.TerceroDocTipoID.Value=Convert.ToString(dr["TerceroDocTipoID"]);
                this.FechaExpDoc.Value = Convert.ToDateTime(dr["FechaExpDoc"]);
                this.CiudadExpDoc.Value=Convert.ToString(dr["CiudadExpDoc"]);
                this.FechaNacimiento.Value = Convert.ToDateTime(dr["FechaNacimiento"]);
                //this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]); 
                this.ApellidoPri.Value = Convert.ToString(dr["ApellidoPri"]);                
                this.ApellidoSdo.Value = Convert.ToString(dr["ApellidoSdo"]);
                this.NombrePri.Value = Convert.ToString(dr["NombrePri"]);
                this.NombreSdo.Value = Convert.ToString(dr["NombreSdo"]);
                this.EstadoCivil.Value = Convert.ToByte(dr["EstadoCivil"]);
                this.ActEconomica1.Value = Convert.ToString(dr["ActEconomica1"]);
                if (!string.IsNullOrWhiteSpace(dr["FuenteIngresos1"].ToString()))
                    this.FuenteIngresos1.Value = Convert.ToByte(dr["FuenteIngresos1"]);
                if (!string.IsNullOrWhiteSpace(dr["FuenteIngresos2"].ToString()))
                    this.FuenteIngresos2.Value = Convert.ToByte(dr["FuenteIngresos2"]);

                if (!string.IsNullOrWhiteSpace(dr["ActEconomica2"].ToString()))
                    this.ActEconomica2.Value = Convert.ToString(dr["ActEconomica2"]);
                if (!string.IsNullOrWhiteSpace(dr["IngresosREG"].ToString()))
                    this.IngresosREG.Value = Convert.ToDecimal(dr["IngresosREG"]);
                if (!string.IsNullOrWhiteSpace(dr["IngresosSOP"].ToString()))
                    this.IngresosSOP.Value = Convert.ToDecimal(dr["IngresosSOP"]);
                if (!string.IsNullOrWhiteSpace(dr["NroInmuebles"].ToString()))
                    this.NroInmuebles.Value = Convert.ToDecimal(dr["NroInmuebles"]);
                if (!string.IsNullOrWhiteSpace(dr["AntCompra"].ToString()))
                    this.AntCompra.Value = Convert.ToByte(dr["AntCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["AntUltimoMOV"].ToString()))
                    this.AntUltimoMOV.Value = Convert.ToByte(dr["AntUltimoMOV"]);
                if (!string.IsNullOrWhiteSpace(dr["HipotecasNro"].ToString()))
                    this.HipotecasNro.Value = Convert.ToDecimal(dr["HipotecasNro"]);
                if (!string.IsNullOrWhiteSpace(dr["RestriccionesNro"].ToString()))
                    this.RestriccionesNro.Value = Convert.ToDecimal(dr["RestriccionesNro"]);
                if (!string.IsNullOrWhiteSpace(dr["FolioMatricula"].ToString()))
                    this.FolioMatricula.Value = Convert.ToString(dr["FolioMatricula"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaMatricula"].ToString()))
                    this.FechaMatricula.Value = Convert.ToDateTime(dr["FechaMatricula"]);
                this.Correo.Value = Convert.ToString(dr["Correo"]);
                this.CiudadResidencia.Value = Convert.ToString(dr["CiudadResidencia"]);
                this.UsuarioDigita.Value=Convert.ToString(dr["UsuarioDigita"]);
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.Consecutivo.Value=Convert.ToInt32(dr["Consecutivo"]);

                if (!string.IsNullOrWhiteSpace(dr["DataCreditoRecibeInd"].ToString()))
                    this.DataCreditoRecibeInd.Value = Convert.ToBoolean(dr["DataCreditoRecibeInd"]);
                if (!string.IsNullOrWhiteSpace(dr["DataCreditoRecibeFecha"].ToString()))
                    this.DataCreditoRecibeFecha.Value = Convert.ToDateTime(dr["DataCreditoRecibeFecha"]);


                if (!string.IsNullOrWhiteSpace(dr["DataCreditoRecibeUsuario"].ToString()))
                    this.DataCreditoRecibeUsuario.Value = Convert.ToString(dr["DataCreditoRecibeUsuario"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_FincaRaiz"].ToString()))
                    this.PF_FincaRaiz.Value = Convert.ToDecimal(dr["PF_FincaRaiz"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_FincaRaizDato"].ToString()))
                    this.PF_FincaRaizDato.Value = Convert.ToDecimal(dr["PF_FincaRaizDato"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_EstadoActual"].ToString()))
                    this.PF_EstadoActual.Value = Convert.ToDecimal(dr["PF_EstadoActual"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_MorasActuales"].ToString()))
                    this.PF_MorasActuales.Value = Convert.ToDecimal(dr["PF_MorasActuales"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUltAno"].ToString()))
                    this.PF_MorasUltAno.Value = Convert.ToDecimal(dr["PF_MorasUltAno"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_RepNegativos"].ToString()))
                    this.PF_RepNegativos.Value = Convert.ToDecimal(dr["PF_RepNegativos"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_Estabilidad"].ToString()))
                    this.PF_Estabilidad.Value = Convert.ToDecimal(dr["PF_Estabilidad"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_Ubicabilidad"].ToString()))
                    this.PF_Ubicabilidad.Value = Convert.ToDecimal(dr["PF_Ubicabilidad"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_Probabilidad"].ToString()))
                    this.PF_Probabilidad.Value = Convert.ToDecimal(dr["PF_Probabilidad"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxFincaRaiz"].ToString()))
                    this.PF_PorMaxFincaRaiz.Value = Convert.ToDecimal(dr["PF_PorMaxFincaRaiz"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxEstadoActual"].ToString()))
                    this.PF_PorMaxEstadoActual.Value = Convert.ToDecimal(dr["PF_PorMaxEstadoActual"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxMorasActuales"].ToString()))
                    this.PF_PorMaxMorasActuales.Value = Convert.ToDecimal(dr["PF_PorMaxMorasActuales"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxMorasUltAno"].ToString()))
                    this.PF_PorMaxMorasUltAno.Value = Convert.ToDecimal(dr["PF_PorMaxMorasUltAno"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxRepNegativos"].ToString()))
                    this.PF_PorMaxRepNegativos.Value = Convert.ToDecimal(dr["PF_PorMaxRepNegativos"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxEstabilidad"].ToString()))
                    this.PF_PorMaxEstabilidad.Value = Convert.ToDecimal(dr["PF_PorMaxEstabilidad"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxUbicabilidad"].ToString()))
                    this.PF_PorMaxUbicabilidad.Value = Convert.ToDecimal(dr["PF_PorMaxUbicabilidad"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxProbabilidad"].ToString()))
                    this.PF_PorMaxProbabilidad.Value = Convert.ToDecimal(dr["PF_PorMaxProbabilidad"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxFinancia"].ToString()))
                    this.PF_PorMaxFinancia.Value = Convert.ToDecimal(dr["PF_PorMaxFinancia"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_IngresoEstimado"].ToString()))
                    this.PF_IngresoEstimado.Value = Convert.ToDecimal(dr["PF_IngresoEstimado"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_RecAguayLuz"].ToString()))
                    this.PF_RecAguayLuz.Value = Convert.ToBoolean(dr["PF_RecAguayLuz"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_ConfirmaCel"].ToString()))
                    this.PF_ConfirmaCel.Value = Convert.ToBoolean(dr["PF_ConfirmaCel"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_ConfirmaMail"].ToString()))
                    this.PF_ConfirmaMail.Value = Convert.ToBoolean(dr["PF_ConfirmaMail"]);


                if (!string.IsNullOrWhiteSpace(dr["PF_VigenciaFMI"].ToString()))
                    this.PF_VigenciaFMI.Value = Convert.ToInt32(dr["PF_VigenciaFMI"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_VigenciaConsData"].ToString()))
                    this.PF_VigenciaConsData.Value = Convert.ToInt32(dr["PF_VigenciaConsData"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_NumHipotecas"].ToString()))
                    this.PF_NumHipotecas.Value = Convert.ToByte(dr["PF_NumHipotecas"]);
    
                if (!string.IsNullOrWhiteSpace(dr["PF_NumBienes"].ToString()))
                    this.PF_NumBienes.Value = Convert.ToByte(dr["PF_NumBienes"]);


                if (!string.IsNullOrWhiteSpace(dr["PF_Restricciones"].ToString()))
                    this.PF_Restricciones.Value = Convert.ToByte(dr["PF_Restricciones"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_AntCompra"].ToString()))
                    this.PF_AntCompra.Value = Convert.ToByte(dr["PF_AntCompra"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_UltAnotacion"].ToString()))
                    this.PF_UltAnotacion.Value = Convert.ToByte(dr["PF_UltAnotacion"]);


                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin1"].ToString()))
                    this.PF_IngresosMin1.Value = Convert.ToDecimal(dr["PF_IngresosMin1"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin2"].ToString()))
                    this.PF_IngresosMin2.Value = Convert.ToDecimal(dr["PF_IngresosMin2"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin3"].ToString()))
                    this.PF_IngresosMin3.Value = Convert.ToDecimal(dr["PF_IngresosMin3"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin4"].ToString()))
                    this.PF_IngresosMin7.Value = Convert.ToDecimal(dr["PF_IngresosMin4"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin5"].ToString()))
                    this.PF_IngresosMin5.Value = Convert.ToDecimal(dr["PF_IngresosMin5"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin6"].ToString()))
                    this.PF_IngresosMin6.Value = Convert.ToDecimal(dr["PF_IngresosMin6"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngresosMin7"].ToString()))
                    this.PF_IngresosMin7.Value = Convert.ToDecimal(dr["PF_IngresosMin7"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorObligaciones"].ToString()))
                    this.PF_PorObligaciones.Value = Convert.ToDecimal(dr["PF_PorObligaciones"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorUtilizaTDC"].ToString()))
                    this.PF_PorUtilizaTDC.Value = Convert.ToDecimal(dr["PF_PorUtilizaTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PeorCalificacion"].ToString()))
                    this.PF_PeorCalificacion.Value = Convert.ToDecimal(dr["PF_PeorCalificacion"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_Consultas6Meses"].ToString())) 
                    this.PF_Consultas6Meses.Value = Convert.ToDecimal(dr["PF_Consultas6Meses"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct30"].ToString())) 
                    this.PF_MorasAct30.Value = Convert.ToDecimal(dr["PF_MorasAct30"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct60"].ToString())) 
                    this.PF_MorasAct60.Value = Convert.ToDecimal(dr["PF_MorasAct60"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct90"].ToString())) 
                    this.PF_MorasAct90.Value = Convert.ToDecimal(dr["PF_MorasAct90"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct120"].ToString()))
                    this.PF_MorasAct120.Value = Convert.ToDecimal(dr["PF_MorasAct120"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt30"].ToString())) 
                    this.PF_MorasUlt30.Value = Convert.ToDecimal(dr["PF_MorasUlt30"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt60"].ToString())) 
                    this.PF_MorasUlt60.Value = Convert.ToDecimal(dr["PF_MorasUlt60"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt90"].ToString())) 
                    this.PF_MorasUlt90.Value = Convert.ToDecimal(dr["PF_MorasUlt90"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt120"].ToString())) 
                    this.PF_MorasUlt120.Value = Convert.ToDecimal(dr["PF_MorasUlt120"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionCOB"].ToString()))
                    this.PF_ObligacionCOB.Value = Convert.ToDecimal(dr["PF_ObligacionCOB"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionDUD"].ToString()))
                    this.PF_ObligacionDUD.Value = Convert.ToDecimal(dr["PF_ObligacionDUD"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionCAS"].ToString()))
                    this.PF_ObligacionCAS.Value = Convert.ToDecimal(dr["PF_ObligacionCAS"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionEMB"].ToString()))
                    this.PF_ObligacionEMB.Value = Convert.ToDecimal(dr["PF_ObligacionEMB"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionREC"].ToString()))
                    this.PF_ObligacionREC.Value = Convert.ToDecimal(dr["PF_ObligacionREC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionCAN"].ToString()))
                    this.PF_ObligacionCAN.Value = Convert.ToDecimal(dr["PF_ObligacionCAN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_DireccDesde"].ToString())) 
                    this.PF_DireccDesde.Value = Convert.ToDecimal(dr["PF_DireccDesde"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EntidadesNum"].ToString()))
                    this.PF_EntidadesNum.Value = Convert.ToDecimal(dr["PF_EntidadesNum"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CelularDesde"].ToString()))
                    this.PF_CelularDesde.Value = Convert.ToDecimal(dr["PF_CelularDesde"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CorreoDesde"].ToString())) 
                    this.PF_CorreoDesde.Value = Convert.ToDecimal(dr["PF_CorreoDesde"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_DireccionNum"].ToString()))
                    this.PF_DireccionNum.Value = Convert.ToDecimal(dr["PF_DireccionNum"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TelefonoNum"].ToString())) 
                    this.PF_TelefonoNum.Value = Convert.ToDecimal(dr["PF_TelefonoNum"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CelularNum"].ToString())) 
                    this.PF_CelularNum.Value = Convert.ToDecimal(dr["PF_CelularNum"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CorreoNum"].ToString())) 
                    this.PF_CorreoNum.Value = Convert.ToDecimal(dr["PF_CorreoNum"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_FactorAcierta"].ToString()))
                    this.PF_FactorAcierta.Value = Convert.ToDecimal(dr["PF_FactorAcierta"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_AciertaResultado"].ToString()))
                    this.PF_AciertaResultado.Value = Convert.ToDecimal(dr["PF_AciertaResultado"]);


                if (!string.IsNullOrWhiteSpace(dr["PF_PorObligacionesDato"].ToString()))
                    this.PF_PorObligacionesDato.Value = Convert.ToDecimal(dr["PF_PorObligacionesDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorUtilizaTDCDato"].ToString())) 
                    this.PF_PorUtilizaTDCDato.Value = Convert.ToDecimal(dr["PF_PorUtilizaTDCDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PeorCalificacionDato"].ToString()))
                    this.PF_PeorCalificacionDato.Value = Convert.ToDecimal(dr["PF_PeorCalificacionDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_Consultas6MesesDato"].ToString())) 
                    this.PF_Consultas6MesesDato.Value = Convert.ToDecimal(dr["PF_Consultas6MesesDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct30Dato"].ToString())) 
                    this.PF_MorasAct30Dato.Value = Convert.ToDecimal(dr["PF_MorasAct30Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct60Dato"].ToString())) 
                    this.PF_MorasAct60Dato.Value = Convert.ToDecimal(dr["PF_MorasAct60Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct90Dato"].ToString())) 
                    this.PF_MorasAct90Dato.Value = Convert.ToDecimal(dr["PF_MorasAct90Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasAct120Dato"].ToString())) 
                    this.PF_MorasAct120Dato.Value = Convert.ToDecimal(dr["PF_MorasAct120Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt30Dato"].ToString())) 
                    this.PF_MorasUlt30Dato.Value = Convert.ToDecimal(dr["PF_MorasUlt30Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt60Dato"].ToString())) 
                    this.PF_MorasUlt60Dato.Value = Convert.ToDecimal(dr["PF_MorasUlt60Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt90Dato"].ToString())) 
                    this.PF_MorasUlt90Dato.Value = Convert.ToDecimal(dr["PF_MorasUlt90Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_MorasUlt120Dato"].ToString())) 
                    this.PF_MorasUlt120Dato.Value = Convert.ToDecimal(dr["PF_MorasUlt120Dato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionCOBDato"].ToString())) 
                    this.PF_ObligacionCOBDato.Value = Convert.ToDecimal(dr["PF_ObligacionCOBDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionDUDDato"].ToString())) 
                    this.PF_ObligacionDUDDato.Value = Convert.ToDecimal(dr["PF_ObligacionDUDDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionCASDato"].ToString()))
                    this.PF_ObligacionCASDato.Value = Convert.ToDecimal(dr["PF_ObligacionCASDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionEMBDato"].ToString()))
                    this.PF_ObligacionEMBDato.Value = Convert.ToDecimal(dr["PF_ObligacionEMBDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionRECDato"].ToString()))
                    this.PF_ObligacionRECDato.Value = Convert.ToDecimal(dr["PF_ObligacionRECDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ObligacionCANDato"].ToString()))
                    this.PF_ObligacionCANDato.Value = Convert.ToDecimal(dr["PF_ObligacionCANDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_DireccDesdeDato"].ToString())) 
                    this.PF_DireccDesdeDato.Value = Convert.ToDecimal(dr["PF_DireccDesdeDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EntidadesNumDato"].ToString()))
                    this.PF_EntidadesNumDato.Value = Convert.ToDecimal(dr["PF_EntidadesNumDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CelularDesdeDato"].ToString()))
                    this.PF_CelularDesdeDato.Value = Convert.ToDecimal(dr["PF_CelularDesdeDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CorreoDesdeDato"].ToString())) 
                    this.PF_CorreoDesdeDato.Value = Convert.ToDecimal(dr["PF_CorreoDesdeDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_DireccDesdeMeses"].ToString()))
                    this.PF_DireccDesdeMeses.Value = Convert.ToInt32(dr["PF_DireccDesdeMeses"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EntidadesNumMeses"].ToString()))
                    this.PF_EntidadesNumMeses.Value = Convert.ToInt32(dr["PF_EntidadesNumMeses"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CelularDesdeMeses"].ToString()))
                    this.PF_CelularDesdeMeses.Value = Convert.ToInt32(dr["PF_CelularDesdeMeses"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CorreoDesdeMeses"].ToString()))
                    this.PF_CorreoDesdeMeses.Value = Convert.ToInt32(dr["PF_CorreoDesdeMeses"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_DireccionNumDato"].ToString()))
                    this.PF_DireccionNumDato.Value = Convert.ToDecimal(dr["PF_DireccionNumDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TelefonoNumDato"].ToString())) 
                    this.PF_TelefonoNumDato.Value = Convert.ToDecimal(dr["PF_TelefonoNumDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CelularNumDato"].ToString())) 
                    this.PF_CelularNumDato.Value = Convert.ToDecimal(dr["PF_CelularNumDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CorreoNumDato"].ToString())) 
                    this.PF_CorreoNumDato.Value = Convert.ToDecimal(dr["PF_CorreoNumDato"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_DireccionNumCant"].ToString()))
                    this.PF_DireccionNumCant.Value = Convert.ToByte(dr["PF_DireccionNumCant"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TelefonoNumCant"].ToString()))
                    this.PF_TelefonoNumCant.Value = Convert.ToByte(dr["PF_TelefonoNumCant"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CelularNumCant"].ToString())) 
                    this.PF_CelularNumCant.Value = Convert.ToByte(dr["PF_CelularNumCant"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CorreoNumCant"].ToString())) 
                    this.PF_CorreoNumCant.Value = Convert.ToByte(dr["PF_CorreoNumCant"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_CapacidadPago"].ToString()))
                    this.PF_CapacidadPago.Value = Convert.ToDecimal(dr["PF_CapacidadPago"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxFinDeuCon"].ToString())) 
                    this.PF_PorMaxFinDeuCon.Value = Convert.ToDecimal(dr["PF_PorMaxFinDeuCon"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CapPagAdDeu"].ToString()))
                    this.PF_CapPagAdDeu.Value = Convert.ToDecimal(dr["PF_CapPagAdDeu"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CapPagAdCon"].ToString())) 
                    this.PF_CapPagAdCon.Value = Convert.ToDecimal(dr["PF_CapPagAdCon"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasVIV"].ToString()))
                    this.PF_EstCtasVIV.Value = Convert.ToDecimal(dr["PF_EstCtasVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotVIV"].ToString())) 
                    this.PF_CtasTotVIV.Value = Convert.ToDecimal(dr["PF_CtasTotVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasBAN"].ToString()))
                    this.PF_EstCtasBAN.Value = Convert.ToDecimal(dr["PF_EstCtasBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotBAN"].ToString()))
                    this.PF_CtasTotBAN.Value = Convert.ToDecimal(dr["PF_CtasTotBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasFIN"].ToString()))
                    this.PF_EstCtasFIN.Value = Convert.ToDecimal(dr["PF_EstCtasFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotFIN"].ToString()))
                    this.PF_CtasTotFIN.Value = Convert.ToDecimal(dr["PF_CtasTotFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasCOP"].ToString())) 
                    this.PF_EstCtasCOP.Value = Convert.ToDecimal(dr["PF_EstCtasCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotCOP"].ToString())) 
                    this.PF_CtasTotCOP.Value = Convert.ToDecimal(dr["PF_CtasTotCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasTDC"].ToString()))
                    this.PF_EstCtasTDC.Value = Convert.ToDecimal(dr["PF_EstCtasTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotTDC"].ToString()))
                    this.PF_CtasTotTDC.Value = Convert.ToDecimal(dr["PF_CtasTotTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasREA"].ToString()))
                    this.PF_EstCtasREA.Value = Convert.ToDecimal(dr["PF_EstCtasREA"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotREA"].ToString()))
                    this.PF_CtasTotREA.Value = Convert.ToDecimal(dr["PF_CtasTotREA"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_EstCtasCEL"].ToString()))
                    this.PF_EstCtasCEL.Value = Convert.ToDecimal(dr["PF_EstCtasCEL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotCEL"].ToString()))
                    this.PF_CtasTotCEL.Value = Convert.ToDecimal(dr["PF_CtasTotCEL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtasTotIngEst"].ToString())) 
                    this.PF_CtasTotIngEst.Value = Convert.ToDecimal(dr["PF_CtasTotIngEst"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_QuiantiMIN"].ToString())) 
                    this.PF_QuiantiMIN.Value = Convert.ToDecimal(dr["PF_QuiantiMIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_QuiantiMAX"].ToString()))
                    this.PF_QuiantiMAX.Value = Convert.ToDecimal(dr["PF_QuiantiMAX"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_QuantoIngrEst"].ToString()))
                    this.PF_QuantoIngrEst.Value = Convert.ToDecimal(dr["PF_QuantoIngrEst"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrEstxQuanto"].ToString()))
                    this.PF_IngrEstxQuanto.Value = Convert.ToDecimal(dr["PF_IngrEstxQuanto"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_FactIngresosREG"].ToString())) 
                    this.PF_FactIngresosREG.Value = Convert.ToDecimal(dr["PF_FactIngresosREG"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrCapacPAG"].ToString())) 
                    this.PF_IngrCapacPAG.Value = Convert.ToDecimal(dr["PF_IngrCapacPAG"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorIngrAporta"].ToString()))
                    this.PF_PorIngrAporta.Value = Convert.ToDecimal(dr["PF_PorIngrAporta"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ReqSopIngrInd"].ToString())) 
                    this.PF_ReqSopIngrInd.Value = Convert.ToDecimal(dr["PF_ReqSopIngrInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorIngrPagoCtas"].ToString())) 
                    this.PF_PorIngrPagoCtas.Value = Convert.ToDecimal(dr["PF_PorIngrPagoCtas"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrDispPagoCtas"].ToString())) 
                    this.PF_IngrDispPagoCtas.Value = Convert.ToDecimal(dr["PF_IngrDispPagoCtas"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CuotasACT"].ToString())) 
                    this.PF_CuotasACT.Value = Convert.ToDecimal(dr["PF_CuotasACT"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrDispApoyos"].ToString())) 
                    this.PF_IngrDispApoyos.Value = Convert.ToDecimal(dr["PF_IngrDispApoyos"]);
                /// 
                /// campos de validacion
                /// 
                if (!string.IsNullOrWhiteSpace(dr["IndTerceroID"].ToString()))
                    this.IndTerceroID.Value = Convert.ToBoolean(dr["IndTerceroID"]);
                else
                    this.IndTerceroID.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndApellidoPri"].ToString())) 
                    this.IndApellidoPri.Value = Convert.ToBoolean(dr["IndApellidoPri"]);
                else
                    this.IndApellidoPri.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndApellidoSdo"].ToString()))
                    this.IndApellidoSdo.Value = Convert.ToBoolean(dr["IndApellidoSdo"]);
                else
                    this.IndApellidoSdo.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndNombrePri"].ToString()))
                    this.IndNombrePri.Value = Convert.ToBoolean(dr["IndNombrePri"]);
                else
                    this.IndNombrePri.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndNombreSdo"].ToString())) 
                    this.IndNombreSdo.Value = Convert.ToBoolean(dr["IndNombreSdo"]);
                else
                    this.IndNombreSdo.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndTerceroDocTipoID"].ToString())) 
                    this.IndTerceroDocTipoID.Value = Convert.ToBoolean(dr["IndTerceroDocTipoID"]);
                else
                    this.IndTerceroDocTipoID.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndFechaExpDoc"].ToString())) 
                    this.IndFechaExpDoc.Value = Convert.ToBoolean(dr["IndFechaExpDoc"]);
                else
                    this.IndFechaExpDoc.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndFechaNacimiento"].ToString()))
                    this.IndFechaNacimiento.Value = Convert.ToBoolean(dr["IndFechaNacimiento"]);
                else
                    this.IndFechaNacimiento.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndEstadoCivil"].ToString())) 
                    this.IndEstadoCivil.Value = Convert.ToBoolean(dr["IndEstadoCivil"]);
                else
                    this.IndEstadoCivil.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndActEconomica1"].ToString())) 
                    this.IndActEconomica1.Value = Convert.ToBoolean(dr["IndActEconomica1"]);
                else
                    this.IndActEconomica1.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndFuenteIngresos1"].ToString()))
                    this.IndFuenteIngresos1.Value = Convert.ToBoolean(dr["IndFuenteIngresos1"]);
                else
                    this.IndFuenteIngresos1.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndFuenteIngresos2"].ToString()))
                    this.IndFuenteIngresos2.Value = Convert.ToBoolean(dr["IndFuenteIngresos2"]);
                else
                    this.IndFuenteIngresos2.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndIngresosREG"].ToString()))
                    this.IndIngresosREG.Value = Convert.ToBoolean(dr["IndIngresosREG"]);
                else
                    this.IndIngresosREG.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndIngresosSOP"].ToString())) 
                    this.IndIngresosSOP.Value = Convert.ToBoolean(dr["IndIngresosSOP"]);
                else
                    this.IndIngresosSOP.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndCorreo"].ToString())) 
                    this.IndCorreo.Value = Convert.ToBoolean(dr["IndCorreo"]);
                else
                    this.IndCorreo.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndCiudadResidencia"].ToString())) 
                    this.IndCiudadResidencia.Value = Convert.ToBoolean(dr["IndCiudadResidencia"]);
                else
                    this.IndCiudadResidencia.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndNroInmuebles"].ToString())) 
                    this.IndNroInmuebles.Value = Convert.ToBoolean(dr["IndNroInmuebles"]);
                else
                    this.IndNroInmuebles.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndAntCompra"].ToString())) 
                    this.IndAntCompra.Value = Convert.ToBoolean(dr["IndAntCompra"]);
                else
                    this.IndAntCompra.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndAntUltimoMOV"].ToString()))
                    this.IndAntUltimoMOV.Value = Convert.ToBoolean(dr["IndAntUltimoMOV"]);
                else
                    this.IndAntUltimoMOV.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndHipotecasNro"].ToString())) 
                    this.IndHipotecasNro.Value = Convert.ToBoolean(dr["IndHipotecasNro"]);
                else
                    this.IndHipotecasNro.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndRestriccionesNro"].ToString()))
                    this.IndRestriccionesNro.Value = Convert.ToBoolean(dr["IndRestriccionesNro"]);
                else
                    this.IndRestriccionesNro.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndFolioMatricula"].ToString())) 
                    this.IndFolioMatricula.Value = Convert.ToBoolean(dr["IndFolioMatricula"]);
                else
                    this.IndFolioMatricula.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["IndFechaMatricula"].ToString()))
                    this.IndFechaMatricula.Value = Convert.ToBoolean(dr["IndFechaMatricula"]);
                else
                    this.IndFechaMatricula.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["GarantePrenda1Ind"].ToString()))
                    this.GarantePrenda1Ind.Value = Convert.ToBoolean(dr["GarantePrenda1Ind"]);
                else
                    this.GarantePrenda1Ind.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["GarantePrenda2Ind"].ToString()))
                    this.GarantePrenda2Ind.Value = Convert.ToBoolean(dr["GarantePrenda2Ind"]);
                else
                    this.GarantePrenda2Ind.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["GaranteHipoteca1Ind"].ToString()))
                    this.GaranteHipoteca1Ind.Value = Convert.ToBoolean(dr["GaranteHipoteca1Ind"]);
                else
                    this.GaranteHipoteca1Ind.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["GaranteHipoteca2Ind"].ToString()))
                    this.GaranteHipoteca2Ind.Value = Convert.ToBoolean(dr["GaranteHipoteca2Ind"]);
                else
                    this.GaranteHipoteca2Ind.Value = false;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_drSolicitudDatosPersonales()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Version = new UDTSQL_int() ;
            this.TipoPersona = new UDTSQL_tinyint();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDocTipoID = new UDT_TerceroTipoID();
            this.FechaExpDoc= new UDTSQL_smalldatetime();
            this.CiudadExpDoc= new UDT_LugarGeograficoID();
            this.FechaNacimiento= new UDTSQL_smalldatetime();
            this.Descriptivo = new UDT_DescripTBase();
            this.ApellidoPri= new UDT_DescripTBase();
            this.ApellidoSdo= new UDT_DescripTBase();
            this.NombrePri= new UDT_DescripTBase();
            this.NombreSdo= new UDT_DescripTBase();
            this.EstadoCivil= new UDTSQL_tinyint();
            this.ActEconomica1= new UDT_ActEconomicaID();
            this.ActEconomica2= new UDT_ActEconomicaID();
            this.FuenteIngresos1 = new UDTSQL_tinyint();
            this.FuenteIngresos2 = new UDTSQL_tinyint();
            this.IngresosREG= new   UDTSQL_decimal();
            this.IngresosSOP= new UDTSQL_decimal();
            this.NroInmuebles = new  UDTSQL_numeric();
            this.AntCompra= new UDTSQL_tinyint();
            this.AntUltimoMOV= new UDTSQL_tinyint();
            this.HipotecasNro= new UDTSQL_numeric();
            this.RestriccionesNro= new UDTSQL_numeric();
            this.FolioMatricula= new UDTSQL_char(10);
            this.FechaMatricula= new UDTSQL_smalldatetime();
            this.Correo = new UDTSQL_char(60);
            this.CiudadResidencia = new UDT_LugarGeograficoID();
            this.UsuarioDigita= new UDT_UsuarioID();
            this.Fecha= new UDTSQL_smalldatetime();
            this.Consecutivo= new UDT_Consecutivo();   
            this.DataCreditoRecibeInd=new UDT_SiNo();
            this.DataCreditoRecibeFecha=new UDTSQL_smalldatetime();
            this.DataCreditoRecibeUsuario=new UDT_UsuarioID();

            this.PF_FincaRaiz = new UDT_PorcentajeID();
            this.PF_FincaRaizDato = new UDT_PorcentajeID();
            this.PF_EstadoActual = new UDT_PorcentajeID();
            this.PF_MorasActuales = new UDT_PorcentajeID();
            this.PF_MorasUltAno = new UDT_PorcentajeID();
            this.PF_RepNegativos = new UDT_PorcentajeID();
            this.PF_Estabilidad = new UDT_PorcentajeID();
            this.PF_Ubicabilidad = new UDT_PorcentajeID();
            this.PF_Probabilidad = new UDT_PorcentajeID();
            this.PF_PorMaxFincaRaiz=new UDT_PorcentajeID();
            this.PF_PorMaxEstadoActual=new UDT_PorcentajeID();
            this.PF_PorMaxMorasActuales = new UDT_PorcentajeID();
            this.PF_PorMaxMorasUltAno = new UDT_PorcentajeID();
            this.PF_PorMaxRepNegativos = new UDT_PorcentajeID();
            this.PF_PorMaxEstabilidad = new UDT_PorcentajeID();
            this.PF_PorMaxUbicabilidad = new UDT_PorcentajeID();
            this.PF_PorMaxProbabilidad = new UDT_PorcentajeID();
            this.PF_PorMaxFinancia = new UDT_PorcentajeID();
            this.PF_IngresoEstimado=new UDTSQL_decimal();
            this.PF_RecAguayLuz=new UDT_SiNo();
            this.PF_ConfirmaCel=new UDT_SiNo();
            this.PF_ConfirmaMail=new UDT_SiNo();
            this.PF_VigenciaFMI=new UDTSQL_int();
            this.PF_VigenciaConsData=new UDTSQL_int();
            this.PF_NumHipotecas=new UDTSQL_tinyint();
            this.PF_NumBienes=new UDTSQL_tinyint();
            this.PF_Restricciones=new UDTSQL_tinyint();
            this.PF_AntCompra=new UDTSQL_tinyint();
            this.PF_UltAnotacion=new UDTSQL_tinyint();
            this.PF_IngresosMin1=new UDT_Valor();
            this.PF_IngresosMin2=new UDT_Valor();
            this.PF_IngresosMin3=new UDT_Valor();
            this.PF_IngresosMin4=new UDT_Valor();
            this.PF_IngresosMin5=new UDT_Valor();
            this.PF_IngresosMin6=new UDT_Valor();
            this.PF_IngresosMin7=new UDT_Valor();
            this.ListaClintonInd = new UDT_SiNo();

            this.PF_PorObligaciones = new UDT_PorcentajeID();
            this.PF_PorUtilizaTDC = new UDT_PorcentajeID();
            this.PF_PeorCalificacion = new UDT_PorcentajeID();
            this.PF_Consultas6Meses = new UDT_PorcentajeID();
            this.PF_MorasAct30 = new UDT_PorcentajeID();
            this.PF_MorasAct60 = new UDT_PorcentajeID();
            this.PF_MorasAct90 = new UDT_PorcentajeID();
            this.PF_MorasAct120 = new UDT_PorcentajeID();
            this.PF_MorasUlt30 = new UDT_PorcentajeID();
            this.PF_MorasUlt60 = new UDT_PorcentajeID();
            this.PF_MorasUlt90 = new UDT_PorcentajeID();
            this.PF_MorasUlt120 = new UDT_PorcentajeID();
            this.PF_ObligacionCOB = new UDT_PorcentajeID();
            this.PF_ObligacionDUD = new UDT_PorcentajeID();
            this.PF_ObligacionCAS = new UDT_PorcentajeID();
            this.PF_ObligacionEMB = new UDT_PorcentajeID();
            this.PF_ObligacionREC = new UDT_PorcentajeID();
            this.PF_ObligacionCAN = new UDT_PorcentajeID();
            this.PF_DireccDesde = new UDT_PorcentajeID();
            this.PF_EntidadesNum = new UDT_PorcentajeID();
            this.PF_CelularDesde = new UDT_PorcentajeID();
            this.PF_CorreoDesde = new UDT_PorcentajeID();
            this.PF_DireccionNum = new UDT_PorcentajeID();
            this.PF_TelefonoNum = new UDT_PorcentajeID();
            this.PF_CelularNum = new UDT_PorcentajeID();
            this.PF_CorreoNum = new UDT_PorcentajeID();
            this.PF_FactorAcierta = new UDT_PorcentajeID();
            this.PF_AciertaResultado = new UDT_PorcentajeID();

            this.PF_PorObligacionesDato = new UDT_PorcentajeID();
            this.PF_PorUtilizaTDCDato = new UDT_PorcentajeID();
            this.PF_PeorCalificacionDato = new UDT_PorcentajeID();
            this.PF_Consultas6MesesDato = new UDT_PorcentajeID();
            this.PF_MorasAct30Dato = new UDT_PorcentajeID();
            this.PF_MorasAct60Dato = new UDT_PorcentajeID();
            this.PF_MorasAct90Dato = new UDT_PorcentajeID();
            this.PF_MorasAct120Dato = new UDT_PorcentajeID();
            this.PF_MorasUlt30Dato = new UDT_PorcentajeID();
            this.PF_MorasUlt60Dato = new UDT_PorcentajeID();
            this.PF_MorasUlt90Dato = new UDT_PorcentajeID();
            this.PF_MorasUlt120Dato = new UDT_PorcentajeID();
            this.PF_ObligacionCOBDato = new UDT_PorcentajeID();
            this.PF_ObligacionDUDDato = new UDT_PorcentajeID();
            this.PF_ObligacionCASDato = new UDT_PorcentajeID();
            this.PF_ObligacionEMBDato = new UDT_PorcentajeID();
            this.PF_ObligacionRECDato = new UDT_PorcentajeID();
            this.PF_ObligacionCANDato = new UDT_PorcentajeID();
            this.PF_DireccDesdeDato = new UDT_PorcentajeID();
            this.PF_EntidadesNumDato = new UDT_PorcentajeID();
            this.PF_CelularDesdeDato = new UDT_PorcentajeID();
            this.PF_CorreoDesdeDato = new UDT_PorcentajeID();
            this.PF_DireccDesdeMeses = new UDTSQL_int();
            this.PF_EntidadesNumMeses = new UDTSQL_int();
            this.PF_CelularDesdeMeses = new UDTSQL_int();
            this.PF_CorreoDesdeMeses = new UDTSQL_int();
            this.PF_DireccionNumDato = new UDT_PorcentajeID();
            this.PF_TelefonoNumDato = new UDT_PorcentajeID();
            this.PF_CelularNumDato = new UDT_PorcentajeID();
            this.PF_CorreoNumDato = new UDT_PorcentajeID();
            this.PF_DireccionNumCant = new UDTSQL_tinyint();
            this.PF_TelefonoNumCant = new UDTSQL_tinyint();
            this.PF_CelularNumCant = new UDTSQL_tinyint();
            this.PF_CorreoNumCant = new UDTSQL_tinyint();

            this.PF_CapacidadPago = new UDT_PorcentajeID();
            this.PF_PorMaxFinDeuCon = new UDT_PorcentajeID();
            this.PF_CapPagAdDeu = new UDT_PorcentajeID();
            this.PF_CapPagAdCon = new UDT_PorcentajeID();

            this.PF_EstCtasVIV = new UDT_Valor();
            this.PF_CtasTotVIV = new UDT_Valor();
            this.PF_EstCtasBAN = new UDT_Valor();
            this.PF_CtasTotBAN = new UDT_Valor();
            this.PF_EstCtasFIN = new UDT_Valor();
            this.PF_CtasTotFIN = new UDT_Valor();
            this.PF_EstCtasCOP = new UDT_Valor();
            this.PF_CtasTotCOP = new UDT_Valor();
            this.PF_EstCtasTDC = new UDT_Valor();
            this.PF_CtasTotTDC = new UDT_Valor();
            this.PF_EstCtasREA = new UDT_Valor();
            this.PF_CtasTotREA = new UDT_Valor();
            this.PF_EstCtasCEL = new UDT_Valor();
            this.PF_CtasTotCEL = new UDT_Valor();
            this.PF_CtasTotIngEst = new UDT_Valor();
            this.PF_QuiantiMIN = new UDT_Valor();
            this.PF_QuiantiMAX = new UDT_Valor();
            this.PF_QuantoIngrEst = new UDT_Valor();
            this.PF_IngrEstxQuanto = new UDT_Valor();
            this.PF_FactIngresosREG = new UDT_Valor();
            this.PF_IngrCapacPAG = new UDT_Valor();
            this.PF_PorIngrAporta = new UDT_Valor();
            this.PF_ReqSopIngrInd = new UDT_Valor();
            this.PF_PorIngrPagoCtas = new UDT_Valor();
            this.PF_IngrDispPagoCtas = new UDT_Valor();
            this.PF_CuotasACT = new UDT_Valor();
            this.PF_IngrDispApoyos = new UDT_Valor();
            /// 
            /// campos de validacion
            /// 
            this.IndTerceroID = new UDT_SiNo();
            this.IndApellidoPri = new UDT_SiNo();
            this.IndApellidoSdo = new UDT_SiNo();
            this.IndNombrePri = new UDT_SiNo();
            this.IndNombreSdo = new UDT_SiNo();
            this.IndTerceroDocTipoID = new UDT_SiNo();
            this.IndFechaExpDoc = new UDT_SiNo();
            this.IndFechaNacimiento = new UDT_SiNo();
            this.IndEstadoCivil = new UDT_SiNo();
            this.IndActEconomica1 = new UDT_SiNo();
            this.IndFuenteIngresos1 = new UDT_SiNo();
            this.IndFuenteIngresos2 = new UDT_SiNo();
            this.IndIngresosREG = new UDT_SiNo();
            this.IndIngresosSOP = new UDT_SiNo();
            this.IndCorreo = new UDT_SiNo();
            this.IndCiudadResidencia = new UDT_SiNo();
            this.IndNroInmuebles = new UDT_SiNo();
            this.IndAntCompra = new UDT_SiNo();
            this.IndAntUltimoMOV = new UDT_SiNo();
            this.IndHipotecasNro = new UDT_SiNo();
            this.IndRestriccionesNro = new UDT_SiNo();
            this.IndFolioMatricula = new UDT_SiNo();
            this.IndFechaMatricula = new UDT_SiNo();
            /// Datos Garantias
            this.GarantePrenda1Ind = new UDT_SiNo();
            this.GarantePrenda2Ind = new UDT_SiNo();
            this.GaranteHipoteca1Ind = new UDT_SiNo();
            this.GaranteHipoteca2Ind = new UDT_SiNo();

        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_int  Version { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoPersona { get; set; }
        
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_TerceroTipoID TerceroDocTipoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaExpDoc { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadExpDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNacimiento { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_DescripTBase ApellidoPri{ get; set; }

        [DataMember]
        public UDT_DescripTBase ApellidoSdo{ get; set; }
      
        [DataMember]
        public UDT_DescripTBase NombrePri{ get; set; }
        
        [DataMember]
        public UDT_DescripTBase NombreSdo{ get; set; }       

        [DataMember]
        public UDTSQL_tinyint EstadoCivil{ get; set; }
      
        [DataMember]
        public UDT_ActEconomicaID ActEconomica1{ get; set; }

        [DataMember]
        public UDTSQL_tinyint FuenteIngresos1 { get; set; }
        [DataMember]
        public UDTSQL_tinyint FuenteIngresos2 { get; set; }
      
        [DataMember]
        public UDT_ActEconomicaID ActEconomica2{ get; set; }
      
        [DataMember]
        public UDTSQL_decimal IngresosREG{ get; set; }
      
        [DataMember]
        public UDTSQL_decimal IngresosSOP{ get; set; }

        [DataMember]
        public UDTSQL_numeric NroInmuebles{ get; set; }

        [DataMember]
        public UDTSQL_tinyint AntCompra{ get; set; }

        [DataMember]
        public UDTSQL_tinyint AntUltimoMOV{ get; set; }
        
        [DataMember]
        public UDTSQL_numeric HipotecasNro{ get; set; }

        [DataMember]
        public UDTSQL_numeric RestriccionesNro{ get; set; }

        [DataMember]
        public UDTSQL_char FolioMatricula{ get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaMatricula{ get; set; }

        [DataMember]
        public UDTSQL_char Correo{ get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadResidencia{ get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioDigita{ get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha{ get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        
        [DataMember]
        public UDT_SiNo DataCreditoRecibeInd { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime DataCreditoRecibeFecha{ get; set; }
        
        [DataMember]
        public UDT_UsuarioID DataCreditoRecibeUsuario { get; set; }


        [DataMember]
        public UDT_PorcentajeID PF_FincaRaiz { get; set; }

        [DataMember]
        public UDT_PorcentajeID PF_FincaRaizDato { get; set; }

        [DataMember]
        public UDT_PorcentajeID PF_EstadoActual { get; set; }

        [DataMember]
        public UDT_PorcentajeID PF_MorasActuales { get; set; }

        [DataMember]
        public UDT_PorcentajeID PF_MorasUltAno { get; set; }
        
        [DataMember]
        public UDT_PorcentajeID PF_RepNegativos { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_Estabilidad	{ get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_Ubicabilidad { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_Probabilidad { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxFincaRaiz { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxEstadoActual { get; set; }		
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxMorasActuales { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxMorasUltAno { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxRepNegativos { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxEstabilidad { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxUbicabilidad { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxProbabilidad { get; set; }	
        
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxFinancia { get; set; }	
        
        [DataMember]
        public UDTSQL_decimal PF_IngresoEstimado	{ get; set; }	
        
        [DataMember]
        public UDT_SiNo PF_RecAguayLuz	{ get; set; }
        
        [DataMember]
        public UDT_SiNo PF_ConfirmaCel	{ get; set; }
        
        [DataMember]
        public UDT_SiNo PF_ConfirmaMail	{ get; set; }
        
        [DataMember]
        public UDTSQL_int PF_VigenciaFMI	{ get; set; }
        
        [DataMember]
        public UDTSQL_int PF_VigenciaConsData	{ get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PF_NumHipotecas	{ get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PF_NumBienes	{ get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PF_Restricciones	{ get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PF_AntCompra	{ get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PF_UltAnotacion	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin1	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin2	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin3	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin4	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin5	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin6	{ get; set; }
        
        [DataMember]
        public UDT_Valor PF_IngresosMin7 { get; set; }

        [DataMember]
        public UDT_SiNo ListaClintonInd { get; set; }

        [DataMember]
        public  UDT_PorcentajeID PF_PorObligaciones{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PorUtilizaTDC{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PeorCalificacion{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_Consultas6Meses{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct30{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct60{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct90{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct120{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt30{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt60{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt90{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt120{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionCOB{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionDUD{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionCAS{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionEMB{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionREC{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionCAN{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_DireccDesde{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_EntidadesNum{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CelularDesde{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CorreoDesde{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_DireccionNum{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_TelefonoNum{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CelularNum{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CorreoNum{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_FactorAcierta{get;set;}
        [DataMember]
        public UDT_PorcentajeID PF_AciertaResultado { get; set; }
        [DataMember]
        public  UDT_PorcentajeID PF_PorObligacionesDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PorUtilizaTDCDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PeorCalificacionDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_Consultas6MesesDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct30Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct60Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct90Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasAct120Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt30Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt60Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt90Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_MorasUlt120Dato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionCOBDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionDUDDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionCASDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionEMBDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionRECDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_ObligacionCANDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_DireccDesdeDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_EntidadesNumDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CelularDesdeDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CorreoDesdeDato{get;set;}
        [DataMember]
        public UDTSQL_int PF_DireccDesdeMeses{get;set;}
        [DataMember]
        public UDTSQL_int PF_EntidadesNumMeses { get; set; }
        [DataMember]
        public UDTSQL_int PF_CelularDesdeMeses { get; set; }
        [DataMember]
        public UDTSQL_int PF_CorreoDesdeMeses { get; set; }
        [DataMember]
        public  UDT_PorcentajeID PF_DireccionNumDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_TelefonoNumDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CelularNumDato{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CorreoNumDato{get;set;}
        [DataMember]
        public  UDTSQL_tinyint PF_DireccionNumCant{get;set;}
        [DataMember]
        public UDTSQL_tinyint PF_TelefonoNumCant { get; set; }
        [DataMember]
        public UDTSQL_tinyint PF_CelularNumCant { get; set; }
        [DataMember]
        public UDTSQL_tinyint PF_CorreoNumCant { get; set; }

        [DataMember]
        public  UDT_PorcentajeID PF_CapacidadPago{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PorMaxFinDeuCon{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CapPagAdDeu{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_CapPagAdCon{get;set;}
        
        [DataMember]
        public  UDT_Valor PF_EstCtasVIV{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotVIV{get;set;}
        [DataMember]
        public  UDT_Valor PF_EstCtasBAN{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotBAN{get;set;}
        [DataMember]
        public  UDT_Valor PF_EstCtasFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_EstCtasCOP{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotCOP{get;set;}
        [DataMember]
        public  UDT_Valor PF_EstCtasTDC{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotTDC{get;set;}
        [DataMember]
        public  UDT_Valor PF_EstCtasREA{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotREA{get;set;}
        [DataMember]
        public  UDT_Valor PF_EstCtasCEL{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotCEL{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtasTotIngEst{get;set;}
        [DataMember]
        public  UDT_Valor PF_QuiantiMIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_QuiantiMAX{get;set;}
        [DataMember]
        public  UDT_Valor PF_QuantoIngrEst{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrEstxQuanto{get;set;}
        [DataMember]
        public  UDT_Valor PF_FactIngresosREG{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrCapacPAG{get;set;}
        [DataMember]
        public  UDT_Valor PF_PorIngrAporta{get;set;}
        [DataMember]
        public  UDT_Valor PF_ReqSopIngrInd{get;set;}
        [DataMember]
        public  UDT_Valor PF_PorIngrPagoCtas{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrDispPagoCtas{get;set;}
        [DataMember]
        public  UDT_Valor PF_CuotasACT{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrDispApoyos{get;set;}
        /// 
        /// campos de validacion
        /// 
        [DataMember]
        public  UDT_SiNo IndTerceroID{get;set;}
        [DataMember]
        public  UDT_SiNo IndApellidoPri{get;set;}
        [DataMember]
        public  UDT_SiNo IndApellidoSdo{get;set;}
        [DataMember]
        public  UDT_SiNo IndNombrePri{get;set;}
        [DataMember]
        public  UDT_SiNo IndNombreSdo{get;set;}
        [DataMember]
        public  UDT_SiNo IndTerceroDocTipoID{get;set;}
        [DataMember]
        public  UDT_SiNo IndFechaExpDoc{get;set;}
        [DataMember]
        public  UDT_SiNo IndFechaNacimiento{get;set;}
        [DataMember]
        public  UDT_SiNo IndEstadoCivil{get;set;}
        [DataMember]
        public  UDT_SiNo IndActEconomica1{get;set;}
        [DataMember]
        public  UDT_SiNo IndFuenteIngresos1{get;set;}
        [DataMember]
        public  UDT_SiNo IndFuenteIngresos2{get;set;}
        [DataMember]
        public  UDT_SiNo IndIngresosREG{get;set;}
        [DataMember]
        public  UDT_SiNo IndIngresosSOP{get;set;}
        [DataMember]
        public  UDT_SiNo IndCorreo{get;set;}
        [DataMember]
        public  UDT_SiNo IndCiudadResidencia{get;set;}
        [DataMember]
        public  UDT_SiNo IndNroInmuebles{get;set;}
        [DataMember]
        public  UDT_SiNo IndAntCompra{get;set;}
        [DataMember]
        public  UDT_SiNo IndAntUltimoMOV{get;set;}
        [DataMember]
        public  UDT_SiNo IndHipotecasNro{get;set;}
        [DataMember]
        public  UDT_SiNo IndRestriccionesNro{get;set;}
        [DataMember]
        public  UDT_SiNo IndFolioMatricula{get;set;}
        [DataMember]
        public  UDT_SiNo IndFechaMatricula{get;set;}

        [DataMember]
        public UDT_SiNo GarantePrenda1Ind { get; set; }
        [DataMember]
        public UDT_SiNo GarantePrenda2Ind { get; set; }
        [DataMember]
        public UDT_SiNo GaranteHipoteca1Ind { get; set; }
        [DataMember]
        public UDT_SiNo GaranteHipoteca2Ind { get; set; }

        #endregion
    }
}


