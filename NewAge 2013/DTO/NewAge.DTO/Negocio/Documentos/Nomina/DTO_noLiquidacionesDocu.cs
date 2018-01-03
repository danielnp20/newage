using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noLiquidacionesDocu 
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noLiquidacionesDocu(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                this.Valor.Value = Convert.ToDecimal(dr["Valor"].ToString());
                this.Iva.Value = Convert.ToDecimal(dr["Iva"].ToString());
                this.Quincena.Value = Convert.ToByte(dr["Quincena"]);
                this.TipoLiquidacion.Value = Convert.ToByte(dr["TipoLiquidacion"]);
                if (!string.IsNullOrEmpty(dr["CausaLiquida"].ToString()))
                    this.CausaLiquida.Value = Convert.ToByte(dr["CausaLiquida"]);
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"].ToString());
                this.OperacionNOID.Value = dr["OperacionNOID"].ToString();
                this.ConvencionNOID.Value = dr["ConvencionNOID"].ToString();
                this.FondoSalud.Value = dr["FondoSalud"].ToString();
                this.FondoPension.Value = dr["FondoPension"].ToString();
                this.FondoCesantias.Value = dr["FondoCesantias"].ToString();
                this.CajaNOID.Value = dr["CajaNOID"].ToString();
                this.RiesgoNOID.Value = dr["RiesgoNOID"].ToString();
                this.BrigadaNOID.Value = dr["BrigadaNOID"].ToString();
                this.TurnoCompID.Value = dr["TurnoCompID"].ToString();
                this.RolNOID.Value = dr["RolNOID"].ToString();
                this.SueldoML.Value = Convert.ToDecimal(dr["SueldoML"].ToString());
                this.SueldoME.Value = Convert.ToDecimal(dr["SueldoME"].ToString());
                this.TipoContrato.Value = Convert.ToByte(dr["TipoContrato"].ToString());
                this.TerminoContrato.Value = Convert.ToByte(dr["TerminoContrato"].ToString());
                this.FormaPago.Value = Convert.ToByte(dr["FormaPago"]);
                if (!string.IsNullOrEmpty(dr["DiasContrato"].ToString()))
                    this.DiasContrato.Value = Convert.ToInt32(dr["DiasContrato"]);
                this.CargoEmpID.Value = dr["CargoEmpID"].ToString();
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ProcedimientoRteFte.Value = Convert.ToByte(dr["ProcedimientoRteFte"]);
                if (!string.IsNullOrEmpty(dr["PorcentajeRteFte"].ToString()))
                    this.PorcentajeRteFte.Value = Convert.ToDecimal(dr["PorcentajeRteFte"].ToString()); 
                this.DeclaraRentaInd.Value = Convert.ToBoolean(dr["DeclaraRentaInd"]);
                if (!string.IsNullOrEmpty(dr["ApSaludEmpresaValor"].ToString()))
                    this.ApSaludEmpresaValor.Value = Convert.ToDecimal(dr["ApSaludEmpresaValor"].ToString());
                if (!string.IsNullOrEmpty(dr["ApSaludEmpresaDias"].ToString()))
                    this.ApSaludEmpresaDias.Value = Convert.ToInt32(dr["ApSaludEmpresaDias"].ToString());
                if (!string.IsNullOrEmpty(dr["ApSaludOtrosValor"].ToString())) 
                    this.ApSaludOtrosValor.Value = Convert.ToDecimal(dr["ApSaludOtrosValor"].ToString());
                if (!string.IsNullOrEmpty(dr["ApSaludOtrosDias"].ToString()))
                    this.ApSaludOtrosDias.Value = Convert.ToInt32(dr["ApSaludOtrosDias"].ToString());
                this.DependenciaInd.Value = Convert.ToBoolean(dr["DependenciaInd"]);
                if (!string.IsNullOrEmpty(dr["BancoID"].ToString()))
                    this.BancoID.Value = dr["BancoID"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoCuenta"].ToString()))
                    this.TipoCuenta.Value = Convert.ToByte(dr["TipoCuenta"].ToString());
                if (!string.IsNullOrEmpty(dr["CuentaAbono"].ToString()))
                    this.CuentaAbono.Value = dr["CuentaAbono"].ToString();
                if (!string.IsNullOrEmpty(dr["NoAuxilioTranspInd"].ToString()))
                    this.NoAuxilioTranspInd.Value = Convert.ToBoolean(dr["NoAuxilioTranspInd"]);
                if (!string.IsNullOrEmpty(dr["NoAporteSaludInd"].ToString()))
                    this.NoAporteSaludInd.Value = Convert.ToBoolean(dr["NoAporteSaludInd"]);
                if (!string.IsNullOrEmpty(dr["NoAportePensionInd"].ToString()))
                    this.NoAportePensionInd.Value = Convert.ToBoolean(dr["NoAportePensionInd"]);
                if (!string.IsNullOrEmpty(dr["DiasPermanencia"].ToString()))
                    this.DiasPermanencia.Value = Convert.ToInt32(dr["DiasPermanencia"]);
                this.Pagos1.Value = Convert.ToDecimal(dr["Pagos1"].ToString());
                this.Pagos2.Value = Convert.ToDecimal(dr["Pagos2"].ToString());
                this.Pagos3.Value = Convert.ToDecimal(dr["Pagos3"].ToString());
                if (!string.IsNullOrEmpty(dr["Pagos4"].ToString()))
                    this.Pagos4.Value = Convert.ToDecimal(dr["Pagos4"]);
                if (!string.IsNullOrEmpty(dr["Pagos5"].ToString()))
                    this.Pagos5.Value = Convert.ToDecimal(dr["Pagos5"]);
                if (!string.IsNullOrEmpty(dr["Pagos6"].ToString()))
                    this.Pagos6.Value = Convert.ToDecimal(dr["Pagos6"]);
                if (!string.IsNullOrEmpty(dr["Pagos7"].ToString()))
                    this.Pagos7.Value = Convert.ToDecimal(dr["Pagos7"]);
                if (!string.IsNullOrEmpty(dr["Pagos8"].ToString()))
                    this.Pagos8.Value = Convert.ToDecimal(dr["Pagos8"]);
                if (!string.IsNullOrEmpty(dr["Pagos9"].ToString()))
                    this.Pagos9.Value = Convert.ToDecimal(dr["Pagos9"]);
                if (!string.IsNullOrEmpty(dr["Pagos10"].ToString()))
                    this.Pagos10.Value = Convert.ToDecimal(dr["Pagos10"]);
                if (!string.IsNullOrEmpty(dr["Dias1"].ToString()))
                    this.Dias1.Value = Convert.ToInt32(dr["Dias1"]);
                if (!string.IsNullOrEmpty(dr["Dias2"].ToString()))
                    this.Dias2.Value = Convert.ToInt32(dr["Dias2"]);
                if (!string.IsNullOrEmpty(dr["Dias3"].ToString()))
                    this.Dias3.Value = Convert.ToInt32(dr["Dias3"]);
                if (!string.IsNullOrEmpty(dr["Dias4"].ToString()))
                    this.Dias4.Value = Convert.ToInt32(dr["Dias4"]);
                if (!string.IsNullOrEmpty(dr["Dias5"].ToString()))
                    this.Dias5.Value = Convert.ToInt32(dr["Dias5"]);
                if (!string.IsNullOrEmpty(dr["FechaIni1"].ToString()))
                    this.FechaIni1.Value = Convert.ToDateTime(dr["FechaIni1"]);
                if (!string.IsNullOrEmpty(dr["FechaFin1"].ToString()))
                    this.FechaFin1.Value = Convert.ToDateTime(dr["FechaFin1"]);
                if (!string.IsNullOrEmpty(dr["FechaIni2"].ToString()))
                    this.FechaIni2.Value = Convert.ToDateTime(dr["FechaIni2"]);
                if (!string.IsNullOrEmpty(dr["FechaFin2"].ToString()))
                    this.FechaFin2.Value = Convert.ToDateTime(dr["FechaFin2"]);
                if (!string.IsNullOrEmpty(dr["FechaIni3"].ToString()))
                    this.FechaIni3.Value = Convert.ToDateTime(dr["FechaIni3"]);
                if (!string.IsNullOrEmpty(dr["FechaFin3"].ToString()))
                    this.FechaFin3.Value = Convert.ToDateTime(dr["FechaFin3"]);
                if (!string.IsNullOrEmpty(dr["Fecha1"].ToString()))
                    this.Fecha1.Value = Convert.ToDateTime(dr["Fecha1"]);
                if (!string.IsNullOrEmpty(dr["Fecha2"].ToString()))
                    this.Fecha2.Value = Convert.ToDateTime(dr["Fecha2"]);
                if (!string.IsNullOrEmpty(dr["Fecha3"].ToString()))
                    this.Fecha3.Value = Convert.ToDateTime(dr["Fecha3"]);
                if (!string.IsNullOrEmpty(dr["Fecha4"].ToString()))
                    this.Fecha4.Value = Convert.ToDateTime(dr["Fecha4"]);
                if (!string.IsNullOrEmpty(dr["Fecha5"].ToString()))
                    this.Fecha5.Value = Convert.ToDateTime(dr["Fecha5"]);
                if (!string.IsNullOrEmpty(dr["DatoAdd1"].ToString()))
                    this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd2"].ToString()))
                    this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd3"].ToString()))
                    this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd4"].ToString()))
                    this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd5"].ToString()))
                    this.DatoAdd5.Value = dr["DatoAdd5"].ToString(); 
                this.PagadoInd.Value = Convert.ToBoolean(dr["PagadoInd"]);
                if (!string.IsNullOrEmpty(dr["Valor1"].ToString()))
                    this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                if (!string.IsNullOrEmpty(dr["Valor2"].ToString()))
                    this.Valor2.Value = Convert.ToDecimal(dr["Valor2"]);
                if (!string.IsNullOrEmpty(dr["Valor3"].ToString()))
                    this.Valor3.Value = Convert.ToDecimal(dr["Valor3"]);
                if (!string.IsNullOrEmpty(dr["Valor4"].ToString()))
                    this.Valor4.Value = Convert.ToDecimal(dr["Valor4"]);
                if (!string.IsNullOrEmpty(dr["Valor5"].ToString()))
                    this.Valor5.Value = Convert.ToDecimal(dr["Valor5"]);
                this.ProcesadoInd.Value = Convert.ToBoolean(dr["ProcesadoInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noLiquidacionesDocu()
        {
            this.InitCols();
        }

          /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.Quincena = new UDTSQL_tinyint();
            this.TipoLiquidacion = new UDTSQL_tinyint();
            this.CausaLiquida = new UDTSQL_tinyint();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.OperacionNOID = new UDT_OperacionNOID();
            this.ConvencionNOID = new UDT_ConvencionNOID();
            this.FondoPension = new UDT_FondoNOID();
            this.FondoSalud = new UDT_FondoNOID();
            this.FondoCesantias = new UDT_FondoNOID();
            this.CajaNOID = new UDT_CajaNOID();
            this.RiesgoNOID = new UDT_RiesgoNOID();
            this.BrigadaNOID = new UDT_BrigadaNOID();
            this.TurnoCompID = new UDT_TurnoCompID();
            this.RolNOID = new UDT_RolNOID();
            this.SueldoML = new UDT_Valor();
            this.SueldoME = new UDT_Valor();
            this.TipoContrato = new UDTSQL_tinyint();
            this.TerminoContrato = new UDTSQL_tinyint();
            this.FormaPago = new UDTSQL_tinyint();
            this.DiasContrato = new UDTSQL_int();
            this.CargoEmpID = new UDT_CargoEmpID();
            this.AreaFuncionalID = new UDT_AreaFuncionalID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ProcedimientoRteFte = new UDTSQL_tinyint();
            this.PorcentajeRteFte = new UDT_PorcentajeID();
            this.DeclaraRentaInd = new UDT_SiNo();
            this.ApSaludEmpresaValor = new UDT_Valor();
            this.ApSaludEmpresaDias = new UDTSQL_int();
            this.ApSaludOtrosDias = new UDTSQL_int();
            this.ApSaludOtrosValor = new UDT_Valor();
            this.DependenciaInd = new UDT_SiNo();
            this.BancoID = new UDT_BancoID();
            this.TipoCuenta = new UDTSQL_tinyint();
            this.CuentaAbono = new UDTSQL_char(20);
            this.EmplConfianzaInd = new UDT_SiNo();
            this.NoAuxilioTranspInd = new UDT_SiNo();
            this.NoAporteSaludInd = new UDT_SiNo();
            this.NoAportePensionInd = new UDT_SiNo();
            this.DiasPermanencia = new UDTSQL_int();
            this.Pagos1 = new UDT_Valor();
            this.Pagos2 = new UDT_Valor();
            this.Pagos3 = new UDT_Valor();
            this.Pagos4 = new UDT_Valor();
            this.Pagos5 = new UDT_Valor();
            this.Pagos6 = new UDT_Valor();
            this.Pagos7 = new UDT_Valor();
            this.Pagos8 = new UDT_Valor();
            this.Pagos9 = new UDT_Valor();
            this.Pagos10 = new UDT_Valor();
            this.Dias1 = new UDTSQL_int();
            this.Dias2 = new UDTSQL_int();
            this.Dias3 = new UDTSQL_int();
            this.Dias4 = new UDTSQL_int();
            this.Dias5 = new UDTSQL_int();
            this.FechaIni1 = new UDTSQL_smalldatetime();
            this.FechaFin1 = new UDTSQL_smalldatetime();
            this.FechaIni2 = new UDTSQL_smalldatetime();
            this.FechaFin2 = new UDTSQL_smalldatetime();
            this.FechaIni3 = new UDTSQL_smalldatetime();
            this.FechaFin3 = new UDTSQL_smalldatetime();
            this.Fecha1 = new UDTSQL_smalldatetime();
            this.Fecha2 = new UDTSQL_smalldatetime();
            this.Fecha3 = new UDTSQL_smalldatetime();
            this.Fecha4 = new UDTSQL_smalldatetime();
            this.Fecha5 = new UDTSQL_smalldatetime();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.PagadoInd = new UDT_SiNo();
            this.Valor1 = new UDT_Valor();
            this.Valor2 =new UDT_Valor();
            this.Valor3 = new UDT_Valor();
            this.Valor4 = new UDT_Valor();
            this.Valor5 = new UDT_Valor();
            this.ProcesadoInd = new UDT_SiNo();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDTSQL_tinyint Quincena { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLiquidacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint CausaLiquida { get; set; }

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; }

        [DataMember]
        public UDT_OperacionNOID OperacionNOID { get; set; }

        [DataMember]
        public UDT_ConvencionNOID ConvencionNOID { get; set; }
        
        [DataMember]
        public UDT_FondoNOID FondoSalud { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoPension { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoCesantias { get; set; }

        [DataMember]
        public UDT_CajaNOID CajaNOID { get; set; }

        [DataMember]
        public UDT_RiesgoNOID RiesgoNOID { get; set; }

        [DataMember]
        public UDT_BrigadaNOID BrigadaNOID { get; set; }

        [DataMember]
        public UDT_TurnoCompID TurnoCompID { get; set; }
        
        [DataMember]
        public UDT_RolNOID RolNOID { get; set; }

        [DataMember]
        public UDT_Valor SueldoML { get; set; }
        
        [DataMember]
        public UDT_Valor SueldoME { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoContrato { get; set; }

        [DataMember]
        public UDTSQL_tinyint TerminoContrato { get; set; }

        [DataMember]
        public UDTSQL_tinyint FormaPago { get; set; }

        [DataMember]
        public UDTSQL_int DiasContrato { get; set; }

        [DataMember]
        public UDT_CargoEmpID CargoEmpID { get; set; }

        [DataMember]
        public UDT_AreaFuncionalID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint ProcedimientoRteFte { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeRteFte { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraRentaInd { get; set; }

        [DataMember]
        public UDT_Valor ApSaludEmpresaValor { get; set; }

        [DataMember]
        public UDTSQL_int ApSaludEmpresaDias { get; set; }

        [DataMember]
        public UDT_Valor ApSaludOtrosValor { get; set; }

        [DataMember]
        public UDTSQL_int ApSaludOtrosDias { get; set; }

        [DataMember]
        public UDT_SiNo DependenciaInd { get; set; }

        [DataMember]
        public UDT_BancoID BancoID { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoCuenta { get; set; }

        [DataMember]
        public UDTSQL_char CuentaAbono { get; set; }

        [DataMember]
        public UDT_SiNo EmplConfianzaInd { get; set; }

        [DataMember]
        public UDT_SiNo NoAuxilioTranspInd { get; set; }

        [DataMember]
        public UDT_SiNo NoAporteSaludInd { get; set; }

        [DataMember]
        public UDT_SiNo NoAportePensionInd { get; set; }

        [DataMember]
        public UDTSQL_int DiasPermanencia { get; set; }

        [DataMember]
        public UDT_Valor Pagos1 { get; set; }

        [DataMember]
        public UDT_Valor Pagos2 { get; set; }

        [DataMember]
        public UDT_Valor Pagos3 { get; set; }

        [DataMember]
        public UDT_Valor Pagos4 { get; set; }

        [DataMember]
        public UDT_Valor Pagos5 { get; set; }

        [DataMember]
        public UDT_Valor Pagos6 { get; set; }
        
        [DataMember]
        public UDT_Valor Pagos7 { get; set; }

        [DataMember]
        public UDT_Valor Pagos8 { get; set; }

        [DataMember]
        public UDT_Valor Pagos9 { get; set; }

        [DataMember]
        public UDT_Valor Pagos10 { get; set; }
        
        [DataMember]
        public UDTSQL_int Dias1 { get; set; }

        [DataMember]
        public UDTSQL_int Dias2 { get; set; }

        [DataMember]
        public UDTSQL_int Dias3 { get; set; }

        [DataMember]
        public UDTSQL_int Dias4 { get; set; }

        [DataMember]
        public UDTSQL_int Dias5 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIni1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIni2 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin2 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIni3 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin3 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha1 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha2 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha3 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha4 { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha5 { get; set; }

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
        public UDT_SiNo PagadoInd { get; set; }

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Valor3 { get; set; }

        [DataMember]
        public UDT_Valor Valor4 { get; set; }

        [DataMember]
        public UDT_Valor Valor5 { get; set; }

        [DataMember]
        public UDT_SiNo ProcesadoInd { get; set; }

        
        #endregion

    }
}
