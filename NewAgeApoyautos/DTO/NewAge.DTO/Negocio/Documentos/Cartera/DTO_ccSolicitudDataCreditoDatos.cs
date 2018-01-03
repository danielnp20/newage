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
    /// Models DTO_ccSolicitudDataCreditoDatos 
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudDataCreditoDatos 
    {
        #region ccSolicitudDataCreditoDatos 

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudDataCreditoDatos (IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToInt32(dr["Version"]);
                this.TipoId.Value = Convert.ToString(dr["TipoId"]);
                this.NumeroId.Value = Convert.ToString(dr["NumeroId"]);
                //this.EstadoDoc.Value = Convert.ToString(dr["EstadoDoc"]);
                //this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                //this.RangoEdad.Value = Convert.ToString(dr["RangoEdad"]);
                //this.Genero.Value = Convert.ToString(dr["Genero"]);
                //this.CiudadExp.Value = Convert.ToString(dr["CiudadExp"]);
                //this.FechaAct.Value = Convert.ToDateTime(dr["FechaAct"]);
                //this.ActEconomica.Value = Convert.ToString(dr["ActEconomica"]);
                //this.RUT.Value = Convert.ToString(dr["RUT"]);
                //this.TipoContrato.Value = Convert.ToString(dr["TipoContrato"]);
                //this.FechaContrato.Value = Convert.ToDateTime(dr["FechaContrato"]);
                //this.NumeObligACT.Value = Convert.ToByte(dr["NumeObligACT"]);
                //this.NumObligaBAN.Value = Convert.ToByte(dr["NumObligaBAN"]);
                //this.VlrInicialBAN.Value = Convert.ToInt32(dr["VlrInicialBAN"]);
                //this.VlrSaldoBAN.Value = Convert.ToInt32(dr["VlrSaldoBAN"]);
                //this.VlrCuotasBAN.Value = Convert.ToInt32(dr["VlrCuotasBAN"]);
                //this.VlrMoraBAN.Value = Convert.ToInt32(dr["VlrMoraBAN"]);
                //this.NumObligaVIV.Value = Convert.ToByte(dr["NumObligaVIV"]);
                //this.VlrInicialVIV.Value = Convert.ToInt32(dr["VlrInicialVIV"]);
                //this.VlrSaldoVIV.Value = Convert.ToInt32(dr["VlrSaldoVIV"]);
                //this.VlrCuotasVIV.Value = Convert.ToInt32(dr["VlrCuotasVIV"]);
                //this.VlrMoraVIV.Value = Convert.ToInt32(dr["VlrMoraVIV"]);
                //this.NumObligaFIN.Value = Convert.ToByte(dr["NumObligaFIN"]);
                //this.VlrInicialFIN.Value = Convert.ToInt32(dr["VlrInicialFIN"]);
                //this.VlrSaldoFIN.Value = Convert.ToInt32(dr["VlrSaldoFIN"]);
                //this.VlrCuotasFIN.Value = Convert.ToInt32(dr["VlrCuotasFIN"]);
                //this.VlrMoraFIN.Value = Convert.ToInt32(dr["VlrMoraFIN"]);
                //this.NumeroTDC.Value = Convert.ToByte(dr["NumeroTDC"]);
                //this.VlrCuposTDC.Value = Convert.ToInt32(dr["VlrCuposTDC"]);
                //this.VlrUtilizadoTDC.Value = Convert.ToInt32(dr["VlrUtilizadoTDC"]);
                //this.PorUtilizaTDC.Value = Convert.ToDecimal(dr["PorUtilizaTDC"]);
                //this.VlrCuotasTDC.Value = Convert.ToInt32(dr["VlrCuotasTDC"]);
                //this.VlrMoraTDC.Value = Convert.ToInt32(dr["VlrMoraTDC"]);
                //this.Rango0TDC.Value = Convert.ToByte(dr["Rango0TDC"]);
                //this.Rango1TDC.Value = Convert.ToByte(dr["Rango1TDC"]);
                //this.Rango2TDC.Value = Convert.ToByte(dr["Rango2TDC"]);
                //this.Rango3TDC.Value = Convert.ToByte(dr["Rango3TDC"]);
                //this.Rango4TDC.Value = Convert.ToByte(dr["Rango4TDC"]);
                //this.Rango5TDC.Value = Convert.ToByte(dr["Rango5TDC"]);
                //this.Rango6TDC.Value = Convert.ToByte(dr["Rango6TDC"]);
                //this.FchAntiguaTDC.Value = Convert.ToDateTime(dr["FchAntiguaTDC"]);
                //this.NumObligaREA.Value = Convert.ToByte(dr["NumObligaREA"]);
                //this.VlrInicialREA.Value = Convert.ToInt32(dr["VlrInicialREA"]);
                //this.VlrSaldoREA.Value = Convert.ToInt32(dr["VlrSaldoREA"]);
                //this.VlrCuotasREA.Value = Convert.ToInt32(dr["VlrCuotasREA"]);
                //this.VlrMoraREA.Value = Convert.ToInt32(dr["VlrMoraREA"]);
                //this.NumObligaCEL.Value = Convert.ToByte(dr["NumObligaCEL"]);
                //this.VlrCuotasCEL.Value = Convert.ToInt32(dr["VlrCuotasCEL"]);
                //this.VlrMoraCEL.Value = Convert.ToInt32(dr["VlrMoraCEL"]);
                //this.NumObligaCOP.Value = Convert.ToByte(dr["NumObligaCOP"]);
                //this.VlrInicialCOP.Value = Convert.ToInt32(dr["VlrInicialCOP"]);
                //this.VlrSaldoCOP.Value = Convert.ToInt32(dr["VlrSaldoCOP"]);
                //this.VlrCuotasCOP.Value = Convert.ToInt32(dr["VlrCuotasCOP"]);
                //this.VlrMoraCOP.Value = Convert.ToInt32(dr["VlrMoraCOP"]);
                //this.NumObligaCOD.Value = Convert.ToByte(dr["NumObligaCOD"]);
                //this.VlrSaldoCOD.Value = Convert.ToInt32(dr["VlrSaldoCOD"]);
                //this.VlrCuotasCOD.Value = Convert.ToInt32(dr["VlrCuotasCOD"]);
                //this.VlrMoraCOD.Value = Convert.ToInt32(dr["VlrMoraCOD"]);
                //this.EstadoActDia.Value = Convert.ToByte(dr["EstadoActDia"]);
                //this.EstadoAct30.Value = Convert.ToByte(dr["EstadoAct30"]);
                //this.EstadoAct60.Value = Convert.ToByte(dr["EstadoAct60"]);
                //this.EstadoAct90.Value = Convert.ToByte(dr["EstadoAct90"]);
                //this.EstadoAct120.Value = Convert.ToByte(dr["EstadoAct120"]);
                //this.EstadoActCas.Value = Convert.ToByte(dr["EstadoActCas"]);
                //this.EstadoActDud.Value = Convert.ToByte(dr["EstadoActDud"]);
                //this.EstadoActCob.Value = Convert.ToByte(dr["EstadoActCob"]);
                //this.EstadoHis30.Value = Convert.ToByte(dr["EstadoHis30"]);
                //this.EstadoHis60.Value = Convert.ToByte(dr["EstadoHis60"]);
                //this.EstadoHis90.Value = Convert.ToByte(dr["EstadoHis90"]);
                //this.EstadoHis120.Value = Convert.ToByte(dr["EstadoHis120"]);
                //this.EstadoHisCan.Value = Convert.ToByte(dr["EstadoHisCan"]);
                //this.EstadoHisRec.Value = Convert.ToByte(dr["EstadoHisRec"]);
                //this.AlturaMorTDC.Value = Convert.ToByte(dr["AlturaMorTDC"]);
                //this.AlturaMorBAN.Value = Convert.ToByte(dr["AlturaMorBAN"]);
                //this.AlturaMorCOP.Value = Convert.ToByte(dr["AlturaMorCOP"]);
                //this.AlturaMorHIP.Value = Convert.ToByte(dr["AlturaMorHIP"]);
                //this.PeorEndeudT1.Value = Convert.ToString(dr["PeorEndeudT1"]);
                //this.PeorEndeudT2.Value = Convert.ToString(dr["PeorEndeudT2"]);
                //this.CtasAhorrosAct.Value = Convert.ToByte(dr["CtasAhorrosAct"]);
                //this.CtasCorrienteAct.Value = Convert.ToByte(dr["CtasCorrienteAct"]);
                //this.CtasEmbargadas.Value = Convert.ToByte(dr["CtasEmbargadas"]);
                //this.CtasMalManejo.Value = Convert.ToByte(dr["CtasMalManejo"]);
                //this.CtasSaldadas.Value = Convert.ToByte(dr["CtasSaldadas"]);
                //this.UltConsultas.Value = Convert.ToByte(dr["UltConsultas"]);
                //this.EstadoConsulta.Value = Convert.ToByte(dr["EstadoConsulta"]);
                //this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                if (!string.IsNullOrWhiteSpace(dr["EstadoDoc"].ToString()))
                    this.EstadoDoc.Value = Convert.ToString(dr["EstadoDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Nombre"].ToString()))
                    this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                if (!string.IsNullOrWhiteSpace(dr["RangoEdad"].ToString()))
                    this.RangoEdad.Value = Convert.ToString(dr["RangoEdad"]);
                if (!string.IsNullOrWhiteSpace(dr["Genero"].ToString()))
                    this.Genero.Value = Convert.ToString(dr["Genero"]);
                if (!string.IsNullOrWhiteSpace(dr["CiudadExp"].ToString())) 
                    this.CiudadExp.Value = Convert.ToString(dr["CiudadExp"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAct"].ToString())) 
                    this.FechaAct.Value = Convert.ToDateTime(dr["FechaAct"]);
                if (!string.IsNullOrWhiteSpace(dr["ActEconomica"].ToString()))
                    this.ActEconomica.Value = Convert.ToString(dr["ActEconomica"]);
                if (!string.IsNullOrWhiteSpace(dr["RUT"].ToString())) 
                    this.RUT.Value = Convert.ToString(dr["RUT"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoContrato"].ToString()))
                    this.TipoContrato.Value = Convert.ToString(dr["TipoContrato"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaContrato"].ToString()))
                    this.FechaContrato.Value = Convert.ToDateTime(dr["FechaContrato"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeObligACT"].ToString())) 
                    this.NumeObligACT.Value = Convert.ToByte(dr["NumeObligACT"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaBAN"].ToString())) 
                    this.NumObligaBAN.Value = Convert.ToByte(dr["NumObligaBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrInicialBAN"].ToString())) 
                    this.VlrInicialBAN.Value = Convert.ToInt32(dr["VlrInicialBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoBAN"].ToString())) 
                    this.VlrSaldoBAN.Value = Convert.ToInt32(dr["VlrSaldoBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasBAN"].ToString())) 
                    this.VlrCuotasBAN.Value = Convert.ToInt32(dr["VlrCuotasBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraBAN"].ToString())) 
                    this.VlrMoraBAN.Value = Convert.ToInt32(dr["VlrMoraBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaVIV"].ToString())) 
                    this.NumObligaVIV.Value = Convert.ToByte(dr["NumObligaVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrInicialVIV"].ToString())) 
                    this.VlrInicialVIV.Value = Convert.ToInt32(dr["VlrInicialVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoVIV"].ToString())) 
                    this.VlrSaldoVIV.Value = Convert.ToInt32(dr["VlrSaldoVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasVIV"].ToString())) 
                    this.VlrCuotasVIV.Value = Convert.ToInt32(dr["VlrCuotasVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraVIV"].ToString())) 
                    this.VlrMoraVIV.Value = Convert.ToInt32(dr["VlrMoraVIV"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaFIN"].ToString())) 
                    this.NumObligaFIN.Value = Convert.ToByte(dr["NumObligaFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrInicialFIN"].ToString()))
                    this.VlrInicialFIN.Value = Convert.ToInt32(dr["VlrInicialFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoFIN"].ToString())) 
                    this.VlrSaldoFIN.Value = Convert.ToInt32(dr["VlrSaldoFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasFIN"].ToString())) 
                    this.VlrCuotasFIN.Value = Convert.ToInt32(dr["VlrCuotasFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraFIN"].ToString()))
                    this.VlrMoraFIN.Value = Convert.ToInt32(dr["VlrMoraFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroTDC"].ToString()))
                    this.NumeroTDC.Value = Convert.ToByte(dr["NumeroTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuposTDC"].ToString())) 
                    this.VlrCuposTDC.Value = Convert.ToInt32(dr["VlrCuposTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrUtilizadoTDC"].ToString()))
                    this.VlrUtilizadoTDC.Value = Convert.ToInt32(dr["VlrUtilizadoTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["PorUtilizaTDC"].ToString())) 
                    this.PorUtilizaTDC.Value = Convert.ToDecimal(dr["PorUtilizaTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasTDC"].ToString())) 
                    this.VlrCuotasTDC.Value = Convert.ToInt32(dr["VlrCuotasTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraTDC"].ToString()))
                    this.VlrMoraTDC.Value = Convert.ToInt32(dr["VlrMoraTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango0TDC"].ToString())) 
                    this.Rango0TDC.Value = Convert.ToByte(dr["Rango0TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango1TDC"].ToString())) 
                    this.Rango1TDC.Value = Convert.ToByte(dr["Rango1TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango2TDC"].ToString())) 
                    this.Rango2TDC.Value = Convert.ToByte(dr["Rango2TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango3TDC"].ToString())) 
                    this.Rango3TDC.Value = Convert.ToByte(dr["Rango3TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango4TDC"].ToString()))
                    this.Rango4TDC.Value = Convert.ToByte(dr["Rango4TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango5TDC"].ToString()))
                    this.Rango5TDC.Value = Convert.ToByte(dr["Rango5TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["Rango6TDC"].ToString())) 
                    this.Rango6TDC.Value = Convert.ToByte(dr["Rango6TDC"]);
                if (!string.IsNullOrWhiteSpace(dr["FchAntiguaTDC"].ToString())) 
                    this.FchAntiguaTDC.Value = Convert.ToDateTime(dr["FchAntiguaTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaREA"].ToString())) 
                    this.NumObligaREA.Value = Convert.ToByte(dr["NumObligaREA"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrInicialREA"].ToString()))
                    this.VlrInicialREA.Value = Convert.ToInt32(dr["VlrInicialREA"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoREA"].ToString())) 
                    this.VlrSaldoREA.Value = Convert.ToInt32(dr["VlrSaldoREA"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasREA"].ToString()))
                    this.VlrCuotasREA.Value = Convert.ToInt32(dr["VlrCuotasREA"]);

                if (!string.IsNullOrWhiteSpace(dr["VlrMoraREA"].ToString()))
                    this.VlrMoraREA.Value = Convert.ToInt32(dr["VlrMoraREA"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaCEL"].ToString()))
                    this.NumObligaCEL.Value = Convert.ToByte(dr["NumObligaCEL"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasCEL"].ToString())) 
                    this.VlrCuotasCEL.Value = Convert.ToInt32(dr["VlrCuotasCEL"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraCEL"].ToString())) 
                    this.VlrMoraCEL.Value = Convert.ToInt32(dr["VlrMoraCEL"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaCOP"].ToString())) 
                    this.NumObligaCOP.Value = Convert.ToByte(dr["NumObligaCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrInicialCOP"].ToString())) 
                    this.VlrInicialCOP.Value = Convert.ToInt32(dr["VlrInicialCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoCOP"].ToString())) 
                    this.VlrSaldoCOP.Value = Convert.ToInt32(dr["VlrSaldoCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasCOP"].ToString()))
                    this.VlrCuotasCOP.Value = Convert.ToInt32(dr["VlrCuotasCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraCOP"].ToString()))
                    this.VlrMoraCOP.Value = Convert.ToInt32(dr["VlrMoraCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["NumObligaCOD"].ToString()))
                    this.NumObligaCOD.Value = Convert.ToByte(dr["NumObligaCOD"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoCOD"].ToString()))
                    this.VlrSaldoCOD.Value = Convert.ToInt32(dr["VlrSaldoCOD"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotasCOD"].ToString()))
                    this.VlrCuotasCOD.Value = Convert.ToInt32(dr["VlrCuotasCOD"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMoraCOD"].ToString())) 
                    this.VlrMoraCOD.Value = Convert.ToInt32(dr["VlrMoraCOD"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoActDia"].ToString())) 
                    this.EstadoActDia.Value = Convert.ToByte(dr["EstadoActDia"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoAct30"].ToString())) 
                    this.EstadoAct30.Value = Convert.ToByte(dr["EstadoAct30"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoAct60"].ToString()))
                    this.EstadoAct60.Value = Convert.ToByte(dr["EstadoAct60"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoAct90"].ToString())) 
                    this.EstadoAct90.Value = Convert.ToByte(dr["EstadoAct90"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoAct120"].ToString()))
                    this.EstadoAct120.Value = Convert.ToByte(dr["EstadoAct120"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoActCas"].ToString())) 
                    this.EstadoActCas.Value = Convert.ToByte(dr["EstadoActCas"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoActDud"].ToString()))
                    this.EstadoActDud.Value = Convert.ToByte(dr["EstadoActDud"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoActCob"].ToString()))
                    this.EstadoActCob.Value = Convert.ToByte(dr["EstadoActCob"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoHis30"].ToString())) 
                    this.EstadoHis30.Value = Convert.ToByte(dr["EstadoHis30"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoHis60"].ToString())) 
                    this.EstadoHis60.Value = Convert.ToByte(dr["EstadoHis60"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoHis90"].ToString()))
                    this.EstadoHis90.Value = Convert.ToByte(dr["EstadoHis90"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoHis120"].ToString())) 
                    this.EstadoHis120.Value = Convert.ToByte(dr["EstadoHis120"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoHisCan"].ToString())) 
                    this.EstadoHisCan.Value = Convert.ToByte(dr["EstadoHisCan"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoHisRec"].ToString())) 
                    this.EstadoHisRec.Value = Convert.ToByte(dr["EstadoHisRec"]);
                if (!string.IsNullOrWhiteSpace(dr["AlturaMorTDC"].ToString())) 
                    this.AlturaMorTDC.Value = Convert.ToByte(dr["AlturaMorTDC"]);
                if (!string.IsNullOrWhiteSpace(dr["AlturaMorBAN"].ToString())) 
                    this.AlturaMorBAN.Value = Convert.ToByte(dr["AlturaMorBAN"]);
                if (!string.IsNullOrWhiteSpace(dr["AlturaMorCOP"].ToString())) 
                    this.AlturaMorCOP.Value = Convert.ToByte(dr["AlturaMorCOP"]);
                if (!string.IsNullOrWhiteSpace(dr["AlturaMorHIP"].ToString())) 
                    this.AlturaMorHIP.Value = Convert.ToByte(dr["AlturaMorHIP"]);
                if (!string.IsNullOrWhiteSpace(dr["PeorEndeudT1"].ToString())) 
                    this.PeorEndeudT1.Value = Convert.ToString(dr["PeorEndeudT1"]);
                if (!string.IsNullOrWhiteSpace(dr["PeorEndeudT2"].ToString())) 
                    this.PeorEndeudT2.Value = Convert.ToString(dr["PeorEndeudT2"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasAhorrosAct"].ToString())) 
                    this.CtasAhorrosAct.Value = Convert.ToByte(dr["CtasAhorrosAct"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasCorrienteAct"].ToString())) 
                    this.CtasCorrienteAct.Value = Convert.ToByte(dr["CtasCorrienteAct"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasEmbargadas"].ToString())) 
                    this.CtasEmbargadas.Value = Convert.ToByte(dr["CtasEmbargadas"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasMalManejo"].ToString())) 
                    this.CtasMalManejo.Value = Convert.ToByte(dr["CtasMalManejo"]);
                if (!string.IsNullOrWhiteSpace(dr["CtasSaldadas"].ToString())) 
                    this.CtasSaldadas.Value = Convert.ToByte(dr["CtasSaldadas"]);
                if (!string.IsNullOrWhiteSpace(dr["UltConsultas"].ToString())) 
                    this.UltConsultas.Value = Convert.ToByte(dr["UltConsultas"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoConsulta"].ToString()))
                    this.EstadoConsulta.Value = Convert.ToByte(dr["EstadoConsulta"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudDataCreditoDatos()
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
            this.TipoId=new UDTSQL_varchar(1);
            this.NumeroId=new UDTSQL_varchar(11);
            this.EstadoDoc=new UDTSQL_varchar(11);
            this.Nombre=new UDTSQL_varchar(50);
            this.RangoEdad=new UDTSQL_varchar(5);
            this.Genero=new UDTSQL_varchar(1);
            this.CiudadExp=new UDTSQL_varchar(30);
            this.FechaAct=new UDTSQL_smalldatetime();
            this.ActEconomica=new UDTSQL_varchar(2);
            this.RUT=new UDTSQL_varchar(2);
            this.TipoContrato=new UDTSQL_varchar(1);
            this.FechaContrato=new UDTSQL_smalldatetime();
            this.NumeObligACT=new UDTSQL_tinyint();
            this.NumObligaBAN=new UDTSQL_tinyint();
            this.VlrInicialBAN=new UDTSQL_int();
            this.VlrSaldoBAN=new UDTSQL_int();
            this.VlrCuotasBAN=new UDTSQL_int();
            this.VlrMoraBAN=new UDTSQL_int();
            this.NumObligaVIV=new UDTSQL_tinyint();
            this.VlrInicialVIV=new UDTSQL_int();
            this.VlrSaldoVIV=new UDTSQL_int();
            this.VlrCuotasVIV=new UDTSQL_int();
            this.VlrMoraVIV=new UDTSQL_int();
            this.NumObligaFIN=new UDTSQL_tinyint();
            this.VlrInicialFIN=new UDTSQL_int();
            this.VlrSaldoFIN=new UDTSQL_int();
            this.VlrCuotasFIN=new UDTSQL_int();
            this.VlrMoraFIN=new UDTSQL_int();
            this.NumeroTDC=new UDTSQL_tinyint();
            this.VlrCuposTDC=new UDTSQL_int();
            this.VlrUtilizadoTDC=new UDTSQL_int();
            this.PorUtilizaTDC=new UDT_PorcentajeID();
            this.VlrCuotasTDC=new UDTSQL_int();
            this.VlrMoraTDC=new UDTSQL_int();
            this.Rango0TDC=new UDTSQL_tinyint();
            this.Rango1TDC=new UDTSQL_tinyint();
            this.Rango2TDC=new UDTSQL_tinyint(); 
            this.Rango3TDC=new UDTSQL_tinyint();
            this.Rango4TDC=new UDTSQL_tinyint();
            this.Rango5TDC=new UDTSQL_tinyint();
            this.Rango6TDC=new UDTSQL_tinyint();
            this.FchAntiguaTDC=new UDTSQL_smalldatetime();
            this.NumObligaREA=new UDTSQL_tinyint();
            this.VlrInicialREA=new UDTSQL_int();
            this.VlrSaldoREA=new UDTSQL_int();
            this.VlrCuotasREA=new UDTSQL_int();
            this.VlrMoraREA=new UDTSQL_int();
            this.NumObligaCEL=new UDTSQL_tinyint();
            this.VlrCuotasCEL=new UDTSQL_int();
            this.VlrMoraCEL=new UDTSQL_int();
            this.NumObligaCOP=new UDTSQL_tinyint();
            this.VlrInicialCOP=new UDTSQL_int();
            this.VlrSaldoCOP=new UDTSQL_int();
            this.VlrCuotasCOP=new UDTSQL_int();
            this.VlrMoraCOP=new UDTSQL_int();
            this.NumObligaCOD=new UDTSQL_tinyint();
            this.VlrSaldoCOD=new UDTSQL_int();
            this.VlrCuotasCOD=new UDTSQL_int();
            this.VlrMoraCOD=new UDTSQL_int();
            this.EstadoActDia=new UDTSQL_tinyint();
            this.EstadoAct30=new UDTSQL_tinyint();
            this.EstadoAct60=new UDTSQL_tinyint();
            this.EstadoAct90=new UDTSQL_tinyint();
            this.EstadoAct120=new UDTSQL_tinyint();
            this.EstadoActCas=new UDTSQL_tinyint();
            this.EstadoActDud=new UDTSQL_tinyint();
            this.EstadoActCob=new UDTSQL_tinyint();
            this.EstadoHis30=new UDTSQL_tinyint();
            this.EstadoHis60=new UDTSQL_tinyint();
            this.EstadoHis90=new UDTSQL_tinyint();
            this.EstadoHis120=new UDTSQL_tinyint();
            this.EstadoHisCan=new UDTSQL_tinyint();
            this.EstadoHisRec=new UDTSQL_tinyint();
            this.AlturaMorTDC=new UDTSQL_tinyint();
            this.AlturaMorBAN=new UDTSQL_tinyint();
            this.AlturaMorCOP=new UDTSQL_tinyint();
            this.AlturaMorHIP=new UDTSQL_tinyint();
            this.PeorEndeudT1=new UDTSQL_varchar(1);
            this.PeorEndeudT2=new UDTSQL_varchar(1);
            this.CtasAhorrosAct=new UDTSQL_tinyint();
            this.CtasCorrienteAct=new UDTSQL_tinyint();
            this.CtasEmbargadas=new UDTSQL_tinyint();
            this.CtasMalManejo=new UDTSQL_tinyint();
            this.CtasSaldadas=new UDTSQL_tinyint();
            this.UltConsultas=new UDTSQL_tinyint();
            this.EstadoConsulta=new UDTSQL_tinyint();           
            this.Consecutivo=new UDT_Consecutivo();
          }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        [DataMember]
        public UDTSQL_int Version{ get; set; }

        [DataMember]
        public UDTSQL_varchar TipoId{ get; set; }


        [DataMember]
        public UDTSQL_varchar NumeroId{ get; set;} 
        
        [DataMember]
        public UDTSQL_varchar EstadoDoc{ get; set;} 
        
        [DataMember]
        public UDTSQL_varchar Nombre{ get; set; }
        
        [DataMember]
        public UDTSQL_varchar RangoEdad{ get; set;} 

        [DataMember]
        public UDTSQL_varchar Genero{ get; set;}        
        
        [DataMember]
        public UDTSQL_varchar CiudadExp{ get; set;}

        [DataMember]
        public UDTSQL_smalldatetime FechaAct{ get; set;}

        
        [DataMember]
        public UDTSQL_varchar ActEconomica{ get; set;}
        
        [DataMember]
        public UDTSQL_varchar RUT{ get; set;}
        
        [DataMember]
        public UDTSQL_varchar TipoContrato{ get; set;}
        
        [DataMember]
        public UDTSQL_smalldatetime FechaContrato{ get; set;}

        [DataMember]
        public UDTSQL_tinyint NumeObligACT{ get; set;}

        [DataMember]
        public UDTSQL_tinyint NumObligaBAN{ get; set;}

        [DataMember]
        public UDTSQL_int VlrInicialBAN{ get; set;}            
        
        [DataMember]
        public UDTSQL_int VlrSaldoBAN{ get; set;}

        [DataMember]
        public UDTSQL_int VlrCuotasBAN{ get; set;}
        
        [DataMember]
        public UDTSQL_int VlrMoraBAN{ get; set;}
        
        [DataMember]
        public UDTSQL_tinyint NumObligaVIV{ get; set;}                

        [DataMember]
        public UDTSQL_int VlrInicialVIV{ get; set;}

        [DataMember]
        public UDTSQL_int VlrSaldoVIV{ get; set;}

        [DataMember]
        public UDTSQL_int VlrCuotasVIV{ get; set;}
                
        [DataMember]
        public UDTSQL_int VlrMoraVIV{ get; set;}
        
        [DataMember]
        public UDTSQL_tinyint NumObligaFIN{ get; set;}
                
        [DataMember]
        public UDTSQL_int VlrInicialFIN{ get; set;}
                
        [DataMember]
        public UDTSQL_int VlrSaldoFIN{ get; set;}
                    
        [DataMember]
        public UDTSQL_int VlrCuotasFIN{ get; set;}
                        
        [DataMember]
        public UDTSQL_int VlrMoraFIN{ get; set;}
                                    
        [DataMember]
        public UDTSQL_tinyint NumeroTDC{ get; set;}
        
        [DataMember]
        public UDTSQL_int VlrCuposTDC{ get; set;}
            
        [DataMember]
        public UDTSQL_int VlrUtilizadoTDC{ get; set;}

        [DataMember]
        public UDT_PorcentajeID PorUtilizaTDC{ get; set;}

        [DataMember]
        public UDTSQL_int VlrCuotasTDC{ get; set;}
        
        [DataMember]
        public UDTSQL_int VlrMoraTDC{ get; set;}
        
        [DataMember]
        public UDTSQL_tinyint Rango0TDC{ get; set;}
        
        [DataMember]
        public UDTSQL_tinyint Rango1TDC{ get; set;}
            
        [DataMember]
        public UDTSQL_tinyint Rango2TDC{ get; set;}
                
        [DataMember]
        public UDTSQL_tinyint Rango3TDC{ get; set;}

        [DataMember]
        public UDTSQL_tinyint Rango4TDC{ get; set;}

        [DataMember]
        public UDTSQL_tinyint Rango5TDC{ get; set;}

        [DataMember]
        public UDTSQL_tinyint Rango6TDC{ get; set;}

        [DataMember]
        public UDTSQL_smalldatetime FchAntiguaTDC{ get; set;}
        
        [DataMember]
        public UDTSQL_tinyint NumObligaREA{ get; set;}        
        
        [DataMember]
        public UDTSQL_int VlrInicialREA{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrSaldoREA{ get; set;}        
                
        [DataMember]
        public UDTSQL_int VlrCuotasREA{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrMoraREA{ get; set;}        
        
        [DataMember]
        public UDTSQL_tinyint NumObligaCEL{ get; set;}        
        
        [DataMember]
        public UDTSQL_int VlrCuotasCEL{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrMoraCEL{ get; set;}        
        
        [DataMember]
        public UDTSQL_tinyint NumObligaCOP{ get; set;}        
        
        [DataMember]
        public UDTSQL_int VlrInicialCOP{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrSaldoCOP{ get; set;}        
                
        [DataMember]
        public UDTSQL_int VlrCuotasCOP{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrMoraCOP{ get; set;}                

        [DataMember]
        public UDTSQL_tinyint NumObligaCOD{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrSaldoCOD{ get; set;}        
                
        [DataMember]
        public UDTSQL_int VlrCuotasCOD{ get; set;}        

        [DataMember]
        public UDTSQL_int VlrMoraCOD{ get; set;}                

        [DataMember]
        public UDTSQL_tinyint EstadoActDia{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoAct30{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoAct60{ get; set;}        
        
        [DataMember]
        public UDTSQL_tinyint EstadoAct90{ get; set;}        
        
        [DataMember]
        public UDTSQL_tinyint EstadoAct120{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoActCas{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoActDud{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoActCob{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoHis30{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoHis60{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoHis90{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoHis120{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoHisCan{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoHisRec{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint AlturaMorTDC{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint AlturaMorBAN{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint AlturaMorCOP{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint AlturaMorHIP{ get; set;}        

        [DataMember]
        public UDTSQL_varchar PeorEndeudT1{ get; set;}        

        [DataMember]
        public UDTSQL_varchar PeorEndeudT2{ get; set;}        
        
        [DataMember]
        public UDTSQL_tinyint CtasAhorrosAct{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint CtasCorrienteAct{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint CtasEmbargadas{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint CtasMalManejo{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint CtasSaldadas{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint UltConsultas{ get; set;}        

        [DataMember]
        public UDTSQL_tinyint EstadoConsulta{ get; set;}        
                
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        

        #endregion
    }
}
