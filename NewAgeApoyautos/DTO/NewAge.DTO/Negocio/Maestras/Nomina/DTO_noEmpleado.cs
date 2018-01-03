using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noEmpleado
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noEmpleado : DTO_MasterBasic
    {
        #region DTO_noEmpleado

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noEmpleado(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.LugarGeograficoDesc.Value = dr["LugarGeograficoDesc"].ToString();
                    this.OperacionNODesc.Value = dr["OperacionNODesc"].ToString();
                    this.SeleccionRHDesc.Value = dr["SeleccionRHDesc"].ToString();
                    this.ConvencionNODesc.Value = dr["ConvencionNODesc"].ToString();
                    this.FondoSaludDesc.Value = dr["FondoSaludDesc"].ToString();
                    this.FondoPensionDesc.Value = dr["FondoPensionDesc"].ToString();
                    this.FondoCesantiasDesc.Value = dr["FondoCesantiasDesc"].ToString();
                    this.CajaNODesc.Value = dr["CajaNODesc"].ToString();
                    this.RiesgoNODesc.Value = dr["RiesgoNODesc"].ToString();
                    this.TurnoCompDesc.Value = dr["TurnoCompDesc"].ToString();
                    this.RolNODesc.Value = dr["RolNODesc"].ToString();
                    this.CargoEmpDesc.Value = dr["CargoEmpDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.TipoVinculacionDesc.Value = dr["TipoVinculacionDesc"].ToString();
                    this.CondicionEspecialDesc.Value = dr["CondicionEspecialDesc"].ToString();
                    this.BancoDesc.Value = dr["BancoDesc"].ToString();
                    this.BrigadaDesc.Value = dr["BrigadaDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }

                this.BrigadaID.Value = dr["BrigadaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.TerceroIDOld.Value = this.TerceroID.Value;
                this.QuincenalInd.Value = Convert.ToBoolean(dr["QuincenalInd"]);
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"]);
                this.Sexo.Value = Convert.ToByte(dr["Sexo"]);
                this.EstadoCivil.Value = Convert.ToByte(dr["EstadoCivil"]);
                this.FechaNacimiento.Value = Convert.ToDateTime(dr["FechaNacimiento"]);
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                this.SeguroID.Value = dr["SeguroID"].ToString();
                this.OperacionNOID.Value = dr["OperacionNOID"].ToString();
                this.CorreoElectronico.Value = dr["CorreoElectronico"].ToString();
                this.DireccionResidencia.Value = dr["DireccionResidencia"].ToString();
                this.TelefonoResidencia.Value = dr["TelefonoResidencia"].ToString();
                this.TelefonoCelular.Value = dr["TelefonoCelular"].ToString();
                this.SeleccionRHID.Value = dr["SeleccionRHID"].ToString();
                this.PaseCategoria.Value = Convert.ToByte(dr["PaseCategoria"]);
                this.PaseMotoInd.Value = Convert.ToBoolean(dr["PaseMotoInd"]);
                this.GrupoSanguineo.Value = Convert.ToByte(dr["GrupoSanguineo"]);
                this.FactorRH.Value = Convert.ToByte(dr["FactorRH"]);
                this.ConvencionNOID.Value = dr["ConvencionNOID"].ToString();
                this.FondoSalud.Value = dr["FondoSalud"].ToString();
                this.FondoPension.Value = dr["FondoPension"].ToString();
                this.FondoCesantias.Value = dr["FondoCesantias"].ToString();
                this.CajaNOID.Value = dr["CajaNOID"].ToString();
                this.RiesgoNOID.Value = dr["RiesgoNOID"].ToString();
                this.TurnoCompID.Value = dr["TurnoCompID"].ToString();
                this.RolNOID.Value = dr["RolNOID"].ToString();
                this.MonedaExtInd.Value = Convert.ToBoolean(dr["MonedaExtInd"]);
                this.FechaIngreso.Value = Convert.ToDateTime(dr["FechaIngreso"]);
                if (!string.IsNullOrEmpty(dr["FechaRetiro"].ToString()))
                    this.FechaRetiro.Value = Convert.ToDateTime(dr["FechaRetiro"]);
                this.Sueldo.Value = Convert.ToDecimal(dr["Sueldo"]);
                this.TipoContrato.Value = Convert.ToByte(dr["TipoContrato"]);
                this.TerminoContrato.Value = Convert.ToByte(dr["TerminoContrato"]);
                this.Estado.Value = Convert.ToByte(dr["Estado"]);
                this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                this.FormaPago.Value = Convert.ToByte(dr["FormaPago"]);
                if (!string.IsNullOrEmpty(dr["DiasContrato"].ToString()))
                    this.DiasContrato.Value = Convert.ToInt32(dr["DiasContrato"]);
                this.CargoEmpID.Value = dr["CargoEmpID"].ToString();
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.TipoVinculacionID.Value = dr["TipoVinculacionID"].ToString();
                this.CondicionEspecialID.Value = dr["CondicionEspecialID"].ToString();
                this.ProcedimientoRteFte.Value = Convert.ToByte(dr["ProcedimientoRteFte"]);
                if (!string.IsNullOrEmpty(dr["PorcentajeRteFte"].ToString()))
                    this.PorcentajeRteFte.Value = Convert.ToDecimal(dr["PorcentajeRteFte"]);
                if (!string.IsNullOrEmpty(dr["ApSaludEmpresaValor"].ToString()))
                    this.ApSaludEmpresaValor.Value = Convert.ToDecimal(dr["ApSaludEmpresaValor"]);
                if (!string.IsNullOrEmpty(dr["ApSaludEmpresaDias"].ToString()))
                    this.ApSaludEmpresaDias.Value = Convert.ToInt32(dr["ApSaludEmpresaDias"]);
                if (!string.IsNullOrEmpty(dr["ApSaludOtrosValor"].ToString()))
                    this.ApSaludOtrosValor.Value = Convert.ToDecimal(dr["ApSaludOtrosValor"]);
                if (!string.IsNullOrEmpty(dr["ApSaludOtrosDias"].ToString()))
                    this.ApSaludOtrosDias.Value = Convert.ToInt32(dr["ApSaludOtrosDias"]);
                this.DependenciaInd.Value = Convert.ToBoolean(dr["DependenciaInd"]);
                this.Pagos1.Value = Convert.ToDecimal(dr["Pagos1"]);
                this.Pagos2.Value = Convert.ToDecimal(dr["Pagos2"]);
                this.Pagos3.Value = Convert.ToDecimal(dr["Pagos3"]);
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
                this.BancoID.Value = dr["BancoID"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoCuenta"].ToString()))
                    this.TipoCuenta.Value = Convert.ToByte(dr["TipoCuenta"]);
                if (!string.IsNullOrEmpty(dr["CuentaAbono"].ToString()))
                    this.CuentaAbono.Value = dr["CuentaAbono"].ToString();
                this.EmplConfianzaInd.Value = Convert.ToBoolean(dr["EmplConfianzaInd"]);
                this.NoAuxilioTranspInd.Value = Convert.ToBoolean(dr["NoAuxilioTranspInd"]);
                this.NoAporteSaludInd.Value = Convert.ToBoolean(dr["NoAporteSaludInd"]);
                this.NoAportePensionInd.Value = Convert.ToBoolean(dr["NoAportePensionInd"]);
                this.DeclaraRentaInd.Value = Convert.ToBoolean(dr["DeclaraRentaInd"]);
                this.Visible.Value = false;
                if (!string.IsNullOrEmpty(dr["FechaActSueldo"].ToString()))
                    this.FechaActSueldo.Value = Convert.ToDateTime(dr["FechaActSueldo"]);
                this.SalarioVariableInd.Value = Convert.ToBoolean(dr["SalarioVariableInd"]);
                if (!string.IsNullOrEmpty(dr["CupoCompFlexible"].ToString()))
                    this.CupoCompFlexible.Value = Convert.ToDecimal(dr["CupoCompFlexible"]);
                if (!string.IsNullOrEmpty(dr["CupoPlusAdicional"].ToString()))
                    this.CupoPlusAdicional.Value = Convert.ToDecimal(dr["CupoPlusAdicional"]);
                if (!string.IsNullOrEmpty(dr["FondoApVoluntario"].ToString()))
                    this.FondoApVoluntario.Value = dr["FondoApVoluntario"].ToString();
                if (!string.IsNullOrEmpty(dr["LugarNacimiento"].ToString()))
                    this.LugarNacimiento.Value = dr["LugarNacimiento"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaModSalario"].ToString()))
                    this.FechaModSalario.Value = Convert.ToDateTime(dr["FechaModSalario"]);
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor solo DataReader
        /// </summary>
        /// <param name="?"></param>
        public DTO_noEmpleado(IDataReader dr)
        {
            this.ID = new UDT_BasicID();
            this.Descriptivo = new UDT_Descriptivo();
            this.ActivoInd = new UDT_SiNo();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();
            this.EmpresaGrupoID = new UDT_EmpresaGrupoID();

            this.InitCols();

            this.ID.Value = dr["EmpleadoID"].ToString();
            this.Descriptivo.Value = dr["Descriptivo"].ToString();

            if (!string.IsNullOrEmpty(dr["TerceroDesc"].ToString()))
                this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["LugarGeograficoDesc"].ToString()))
                this.LugarGeograficoDesc.Value = dr["LugarGeograficoDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["OperacionNODesc"].ToString()))
                this.OperacionNODesc.Value = dr["OperacionNODesc"].ToString();
            if (!string.IsNullOrEmpty(dr["SeleccionRHDesc"].ToString()))
                this.SeleccionRHDesc.Value = dr["SeleccionRHDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["ConvencionNODesc"].ToString()))
                this.ConvencionNODesc.Value = dr["ConvencionNODesc"].ToString();
            if (!string.IsNullOrEmpty(dr["FondoSaludDesc"].ToString()))
                this.FondoSaludDesc.Value = dr["FondoSaludDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["FondoPensionDesc"].ToString()))
                this.FondoPensionDesc.Value = dr["FondoPensionDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["FondoCesantiasDesc"].ToString()))
                this.FondoCesantiasDesc.Value = dr["FondoCesantiasDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["CajaNODesc"].ToString()))
                this.CajaNODesc.Value = dr["CajaNODesc"].ToString();
            if (!string.IsNullOrEmpty(dr["RiesgoNODesc"].ToString()))
                this.RiesgoNODesc.Value = dr["RiesgoNODesc"].ToString();
            if (!string.IsNullOrEmpty(dr["TurnoCompDesc"].ToString()))
                this.TurnoCompDesc.Value = dr["TurnoCompDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["RolNODesc"].ToString()))
                this.RolNODesc.Value = dr["RolNODesc"].ToString();
            if (!string.IsNullOrEmpty(dr["CargoEmpDesc"].ToString()))
                this.CargoEmpDesc.Value = dr["CargoEmpDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["AreaFuncionalDesc"].ToString()))
                this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["ProyectoDesc"].ToString()))
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["CentroCostoDesc"].ToString()))
                this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["BancoDesc"].ToString()))
                this.BancoDesc.Value = dr["BancoDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["BrigadaDesc"].ToString()))
                this.BrigadaDesc.Value = dr["BrigadaDesc"].ToString();
            if (!string.IsNullOrEmpty(dr["LineaPresupuestoDesc"].ToString()))
                this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();

            this.BrigadaID.Value = dr["BrigadaID"].ToString();
            this.TerceroID.Value = dr["TerceroID"].ToString();
            this.TerceroIDOld.Value = this.TerceroID.Value;
            this.QuincenalInd.Value = Convert.ToBoolean(dr["QuincenalInd"]);
            this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"]);
            this.Sexo.Value = Convert.ToByte(dr["Sexo"]);
            this.EstadoCivil.Value = Convert.ToByte(dr["EstadoCivil"]);
            this.FechaNacimiento.Value = Convert.ToDateTime(dr["FechaNacimiento"]);
            this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
            this.SeguroID.Value = dr["SeguroID"].ToString();
            this.OperacionNOID.Value = dr["OperacionNOID"].ToString();
            this.CorreoElectronico.Value = dr["CorreoElectronico"].ToString();
            this.DireccionResidencia.Value = dr["DireccionResidencia"].ToString();
            this.TelefonoResidencia.Value = dr["TelefonoResidencia"].ToString();
            this.TelefonoCelular.Value = dr["TelefonoCelular"].ToString();
            this.SeleccionRHID.Value = dr["SeleccionRHID"].ToString();
            this.PaseCategoria.Value = Convert.ToByte(dr["PaseCategoria"]);
            this.PaseMotoInd.Value = Convert.ToBoolean(dr["PaseMotoInd"]);
            this.GrupoSanguineo.Value = Convert.ToByte(dr["GrupoSanguineo"]);
            this.FactorRH.Value = Convert.ToByte(dr["FactorRH"]);
            this.ConvencionNOID.Value = dr["ConvencionNOID"].ToString();
            this.FondoSalud.Value = dr["FondoSalud"].ToString();
            this.FondoPension.Value = dr["FondoPension"].ToString();
            this.FondoCesantias.Value = dr["FondoCesantias"].ToString();
            this.CajaNOID.Value = dr["CajaNOID"].ToString();
            this.RiesgoNOID.Value = dr["RiesgoNOID"].ToString();
            this.TurnoCompID.Value = dr["TurnoCompID"].ToString();
            this.RolNOID.Value = dr["RolNOID"].ToString();
            this.MonedaExtInd.Value = Convert.ToBoolean(dr["MonedaExtInd"]);
            this.FechaIngreso.Value = Convert.ToDateTime(dr["FechaIngreso"]);
            if (!string.IsNullOrEmpty(dr["FechaRetiro"].ToString()))
                this.FechaRetiro.Value = Convert.ToDateTime(dr["FechaRetiro"]);
            this.Sueldo.Value = Convert.ToDecimal(dr["Sueldo"]);
            this.TipoContrato.Value = Convert.ToByte(dr["TipoContrato"]);
            this.TerminoContrato.Value = Convert.ToByte(dr["TerminoContrato"]);
            this.Estado.Value = Convert.ToByte(dr["Estado"]);
            this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
            this.FormaPago.Value = Convert.ToByte(dr["FormaPago"]);
            this.DiasContrato.Value = Convert.ToInt32(dr["DiasContrato"]);
            this.CargoEmpID.Value = dr["CargoEmpID"].ToString();
            this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
            this.ProyectoID.Value = dr["ProyectoID"].ToString();
            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
            this.ProcedimientoRteFte.Value = Convert.ToByte(dr["ProcedimientoRteFte"]);
            if (!string.IsNullOrEmpty(dr["PorcentajeRteFte"].ToString()))
                this.PorcentajeRteFte.Value = Convert.ToDecimal(dr["PorcentajeRteFte"]);
            if (!string.IsNullOrEmpty(dr["ApSaludEmpresaValor"].ToString()))
                this.ApSaludEmpresaValor.Value = Convert.ToDecimal(dr["ApSaludEmpresaValor"]);
            if (!string.IsNullOrEmpty(dr["ApSaludEmpresaDias"].ToString()))
                this.ApSaludEmpresaDias.Value = Convert.ToInt32(dr["ApSaludEmpresaDias"]);
            if (!string.IsNullOrEmpty(dr["ApSaludOtrosValor"].ToString()))
                this.ApSaludOtrosValor.Value = Convert.ToDecimal(dr["ApSaludOtrosValor"]);
            if (!string.IsNullOrEmpty(dr["ApSaludOtrosDias"].ToString()))
                this.ApSaludOtrosDias.Value = Convert.ToInt32(dr["ApSaludOtrosDias"]);
            this.DependenciaInd.Value = Convert.ToBoolean(dr["DependenciaInd"]);
            this.Pagos1.Value = Convert.ToDecimal(dr["Pagos1"]);
            this.Pagos2.Value = Convert.ToDecimal(dr["Pagos2"]);
            this.Pagos3.Value = Convert.ToDecimal(dr["Pagos3"]);
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

            this.BancoID.Value = dr["BancoID"].ToString();
            if (!string.IsNullOrEmpty(dr["TipoCuenta"].ToString()))
                this.TipoCuenta.Value = Convert.ToByte(dr["TipoCuenta"]);
            if (!string.IsNullOrEmpty(dr["CuentaAbono"].ToString()))
                this.CuentaAbono.Value = dr["CuentaAbono"].ToString();
            this.EmplConfianzaInd.Value = Convert.ToBoolean(dr["EmplConfianzaInd"]);
            this.NoAuxilioTranspInd.Value = Convert.ToBoolean(dr["NoAuxilioTranspInd"]);
            this.NoAporteSaludInd.Value = Convert.ToBoolean(dr["NoAporteSaludInd"]);
            this.NoAportePensionInd.Value = Convert.ToBoolean(dr["NoAportePensionInd"]);
            this.DeclaraRentaInd.Value = Convert.ToBoolean(dr["DeclaraRentaInd"]);
            this.Visible.Value = false;
            if (!string.IsNullOrEmpty(dr["FechaActSueldo"].ToString()))
                this.FechaActSueldo.Value = Convert.ToDateTime(dr["FechaActSueldo"]);
            if (!string.IsNullOrEmpty(dr["SalarioVariableInd"].ToString()))
                this.SalarioVariableInd.Value = Convert.ToBoolean(dr["SalarioVariableInd"]);
            if (!string.IsNullOrEmpty(dr["CupoCompFlexible"].ToString()))
                this.CupoCompFlexible.Value = Convert.ToDecimal(dr["CupoCompFlexible"]);
            if (!string.IsNullOrEmpty(dr["CupoPlusAdicional"].ToString()))
                this.CupoPlusAdicional.Value = Convert.ToDecimal(dr["CupoPlusAdicional"]);
            if (!string.IsNullOrEmpty(dr["FondoApVoluntario"].ToString()))
                this.FondoApVoluntario.Value = dr["FondoApVoluntario"].ToString();
            if (!string.IsNullOrEmpty(dr["LugarNacimiento"].ToString()))
                this.LugarNacimiento.Value = dr["LugarNacimiento"].ToString();
            if (!string.IsNullOrEmpty(dr["FechaModSalario"].ToString()))
                this.FechaModSalario.Value = Convert.ToDateTime(dr["FechaModSalario"]);
            if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
            
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noEmpleado()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroIDOld = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.QuincenalInd = new UDT_SiNo();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.Sexo = new UDTSQL_tinyint();
            this.EstadoCivil = new UDTSQL_tinyint();
            this.FechaNacimiento = new UDTSQL_smalldatetime();
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeograficoDesc = new UDT_Descriptivo();
            this.SeguroID = new UDTSQL_char(25);
            this.OperacionNOID = new UDT_BasicID();
            this.OperacionNODesc = new UDT_Descriptivo();
            this.CorreoElectronico = new UDTSQL_char(100);
            this.DireccionResidencia = new UDTSQL_char(50);
            this.TelefonoResidencia = new UDTSQL_char(50);
            this.TelefonoCelular = new UDTSQL_char(50);
            this.SeleccionRHID = new UDT_BasicID();
            this.SeleccionRHDesc = new UDT_Descriptivo();
            this.PaseCategoria = new UDTSQL_tinyint();
            this.PaseMotoInd = new UDT_SiNo();
            this.GrupoSanguineo = new UDTSQL_tinyint();
            this.FactorRH = new UDTSQL_tinyint();
            this.ConvencionNOID = new UDT_BasicID();
            this.ConvencionNODesc = new UDT_Descriptivo();
            this.FondoSalud = new UDT_BasicID();
            this.FondoSaludDesc = new UDT_Descriptivo();
            this.FondoPension = new UDT_BasicID();
            this.FondoPensionDesc = new UDT_Descriptivo();
            this.FondoCesantias = new UDT_BasicID();
            this.FondoCesantiasDesc = new UDT_Descriptivo();
            this.CajaNOID = new UDT_BasicID();
            this.CajaNODesc = new UDT_Descriptivo();
            this.RiesgoNOID = new UDT_BasicID();
            this.RiesgoNODesc = new UDT_Descriptivo();
            this.TurnoCompID = new UDT_BasicID();
            this.TurnoCompDesc = new UDT_Descriptivo();
            this.RolNOID = new UDT_BasicID();
            this.RolNODesc = new UDT_Descriptivo();
            this.MonedaExtInd = new UDT_SiNo();
            this.FechaIngreso = new UDTSQL_smalldatetime();
            this.FechaRetiro = new UDTSQL_smalldatetime();
            this.Sueldo = new UDT_Valor();
            this.TipoContrato = new UDTSQL_tinyint();
            this.TerminoContrato = new UDTSQL_tinyint();
            this.Estado = new UDTSQL_tinyint();
            this.PeriodoPago = new UDTSQL_tinyint();
            this.FormaPago = new UDTSQL_tinyint();
            this.DiasContrato = new UDTSQL_int();
            this.CargoEmpID = new UDT_BasicID();
            this.CargoEmpDesc = new UDT_Descriptivo();
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.TipoVinculacionID=new UDT_BasicID();
            this.TipoVinculacionDesc = new UDT_Descriptivo();
            this.CondicionEspecialID = new UDT_BasicID();
            this.CondicionEspecialDesc = new UDT_Descriptivo();
            this.ProcedimientoRteFte = new UDTSQL_tinyint();
            this.PorcentajeRteFte = new UDT_PorcentajeID();
            this.ApSaludEmpresaValor = new UDT_Valor();
            this.ApSaludEmpresaDias = new UDTSQL_int();
            this.ApSaludOtrosValor = new UDT_Valor();
            this.ApSaludOtrosDias = new UDTSQL_int();
            this.DependenciaInd = new UDT_SiNo();
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
            this.BancoID = new UDT_BasicID();
            this.BancoDesc = new UDT_Descriptivo();
            this.TipoCuenta = new UDTSQL_tinyint();
            this.CuentaAbono = new UDTSQL_char(50);
            this.EmplConfianzaInd = new UDT_SiNo();
            this.NoAuxilioTranspInd = new UDT_SiNo();
            this.NoAporteSaludInd = new UDT_SiNo();
            this.NoAportePensionInd = new UDT_SiNo();
            this.DeclaraRentaInd = new UDT_SiNo();
            this.Visible = new UDT_SiNo();
            this.FechaActSueldo = new UDTSQL_datetime();
            this.SalarioVariableInd = new UDT_SiNo();
            this.CupoCompFlexible = new UDT_PorcentajeID();
            this.CupoPlusAdicional = new UDT_PorcentajeID();
            this.FondoApVoluntario = new UDT_BasicID();
            this.FondoApVoluntarioDesc = new UDT_Descriptivo();
            this.LugarNacimiento = new UDTSQL_char(50);
            this.BrigadaID = new UDT_BasicID();
            this.BrigadaDesc = new UDT_Descriptivo();
            this.FechaModSalario = new UDTSQL_smalldatetime();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
        }

        public DTO_noEmpleado(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noEmpleado(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID TerceroID { get; set; } //

        [DataMember]
        public UDT_BasicID TerceroIDOld { get; set; } //

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; } //

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; } // 

        [DataMember]
        public UDTSQL_tinyint Sexo { get; set; } //

        [DataMember]
        public UDTSQL_tinyint EstadoCivil { get; set; } //

        [DataMember]
        public UDTSQL_smalldatetime FechaNacimiento { get; set; } //

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; } //

        [DataMember]
        public UDT_Descriptivo LugarGeograficoDesc { get; set ; } // 

        [DataMember]
        public UDTSQL_char SeguroID { get; set; } // 

        [DataMember]
        public UDT_BasicID OperacionNOID { get; set; } //

        [DataMember]
        public UDT_Descriptivo OperacionNODesc { get; set; } // 

        [DataMember]
        public UDTSQL_char CorreoElectronico { get; set; } //

        [DataMember]
        public UDTSQL_char DireccionResidencia { get; set; } //

        [DataMember]
        public UDTSQL_char TelefonoResidencia { get; set; } //

        [DataMember]
        public UDTSQL_char TelefonoCelular { get; set; } //

        [DataMember]
        public UDT_BasicID SeleccionRHID { get; set; } //

        [DataMember]
        public UDT_Descriptivo SeleccionRHDesc { get; set; } //

        [DataMember]
        public UDTSQL_tinyint PaseCategoria { get; set; } //

        [DataMember]
        public UDT_SiNo PaseMotoInd { get; set; } //

        [DataMember]
        public UDTSQL_tinyint GrupoSanguineo { get; set; } //

        [DataMember]
        public UDTSQL_tinyint FactorRH { get; set; } //

        [DataMember]
        public UDT_SiNo QuincenalInd { get; set; } //

        [DataMember]
        public UDT_BasicID ConvencionNOID { get; set; } //

        [DataMember]
        public UDT_Descriptivo ConvencionNODesc { get; set; } //

        [DataMember]
        public UDT_BasicID FondoSalud { get; set; } //

        [DataMember]
        public UDT_Descriptivo FondoSaludDesc { get; set; } //

        [DataMember]
        public UDT_BasicID FondoPension { get; set; } //

        [DataMember]
        public UDT_Descriptivo FondoPensionDesc { get; set; } //

        [DataMember]
        public UDT_BasicID FondoCesantias { get; set; } //

        [DataMember]
        public UDT_Descriptivo FondoCesantiasDesc { get; set; } //

        [DataMember]
        public UDT_BasicID CajaNOID { get; set; } //

        [DataMember]
        public UDT_Descriptivo CajaNODesc { get; set; } //

        [DataMember]
        public UDT_BasicID RiesgoNOID { get; set; } //

        [DataMember]
        public UDT_Descriptivo RiesgoNODesc { get; set; } //

        [DataMember]
        public UDT_BasicID TurnoCompID { get; set; } //

        [DataMember]
        public UDT_Descriptivo TurnoCompDesc { get; set; } //

        [DataMember]
        public UDT_BasicID RolNOID { get; set; } //

        [DataMember]
        public UDT_Descriptivo RolNODesc { get; set; } //

        [DataMember]
        public UDT_SiNo MonedaExtInd { get; set; } //

        [DataMember]
        public UDTSQL_smalldatetime FechaIngreso { get; set; } //

        [DataMember]
        public UDTSQL_smalldatetime FechaRetiro { get; set; } //

        [DataMember]
        public UDT_Valor Sueldo { get; set; } //

        [DataMember]
        public UDTSQL_tinyint TipoContrato { get; set; } //

        [DataMember]
        public UDTSQL_tinyint TerminoContrato { get; set; } //

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; } //

        [DataMember]
        public UDTSQL_tinyint PeriodoPago { get; set; } //

        [DataMember]
        public UDTSQL_tinyint FormaPago { get; set; } //

        [DataMember]
        public UDTSQL_int DiasContrato { get; set; } //

        [DataMember]
        public UDT_BasicID CargoEmpID { get; set; } //

        [DataMember]
        public UDT_Descriptivo CargoEmpDesc { get; set; } //

        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; } //

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; } //

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; } //

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; } //

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; } //

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; } //

        [DataMember]
        public UDT_BasicID TipoVinculacionID { get; set; } //

        [DataMember]
        public UDT_Descriptivo TipoVinculacionDesc { get; set; } //

        [DataMember]
        public UDT_BasicID CondicionEspecialID { get; set; } //

        [DataMember]
        public UDT_Descriptivo CondicionEspecialDesc { get; set; } //

        [DataMember]
        public UDTSQL_tinyint ProcedimientoRteFte { get; set; } //

        [DataMember]
        public UDT_PorcentajeID PorcentajeRteFte { get; set; } //

        [DataMember]
        public UDT_SiNo DeclaraRentaInd { get; set; } //

        [DataMember]
        public UDT_Valor ApSaludEmpresaValor { get; set; } //

        [DataMember]
        public UDTSQL_int ApSaludEmpresaDias { get; set; } //

        [DataMember]
        public UDT_Valor ApSaludOtrosValor { get; set; } //

        [DataMember]
        public UDTSQL_int ApSaludOtrosDias { get; set; } //

        [DataMember]
        public UDT_SiNo DependenciaInd { get; set; } //

        [DataMember]
        public UDT_Valor Pagos1 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos2 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos3 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos4 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos5 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos6 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos7 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos8 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos9 { get; set; } //

        [DataMember]
        public UDT_Valor Pagos10 { get; set; } //

        [DataMember]
        public UDT_BasicID BancoID { get; set; } //

        [DataMember]
        public UDT_Descriptivo BancoDesc { get; set; } //

        [DataMember]
        public UDTSQL_tinyint TipoCuenta { get; set; } //

        [DataMember]
        public UDTSQL_char CuentaAbono { get; set; } //

        [DataMember]
        public UDT_SiNo EmplConfianzaInd { get; set; } //

        [DataMember]
        public UDT_SiNo NoAuxilioTranspInd { get; set; } //

        [DataMember]
        public UDT_SiNo NoAporteSaludInd { get; set; } //

        [DataMember]
        public UDT_SiNo NoAportePensionInd { get; set; } //

        [DataMember]
        public UDTSQL_datetime FechaActSueldo { get; set; } //

        [DataMember]
        public UDT_SiNo SalarioVariableInd { get; set; } //

        [DataMember]
        public UDT_PorcentajeID CupoCompFlexible { get; set; } //

        [DataMember]
        public UDT_PorcentajeID CupoPlusAdicional { get; set; } //

        [DataMember]
        public UDT_BasicID FondoApVoluntario { get; set; } //

        [DataMember]
        public UDT_Descriptivo FondoApVoluntarioDesc { get; set; } //

        [DataMember]
        public UDTSQL_char LugarNacimiento { get; set; } //

        [DataMember]
        public UDT_BasicID BrigadaID { get; set; } //

        [DataMember]
        public UDT_Descriptivo BrigadaDesc { get; set; } //

        [DataMember]
        public UDTSQL_smalldatetime FechaModSalario { get; set; } //

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; } //

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; } //


        // Extra 

        [DataMember]
        public UDT_SiNo Visible { get; set; }

        #endregion

    }
}

