using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.Data.SqlTypes;
using NewAge.DTO.Reportes;

namespace NewAge.ADO
{
    [ADOExceptionManager]
    public class DAL_noLiquidacionesDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noLiquidacionesDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega unr registro en la tabla noLiquidacionesDocu 
        /// </summary>
        /// <param name="doc">documento</param>   
        /// <returns>true si la operacion es exitosa</returns>      
        public void DAL_noLiquidacionesDocu_Add(DTO_noLiquidacionesDocu doc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  INSERT INTO noLiquidacionesDocu    " +
                                               "  ([EmpresaID]    " +
                                               "  ,[NumeroDoc]    " +
                                               "  ,[Valor]    " +
                                               "  ,[Iva]    " +
                                               "  ,[Quincena]    " +
                                               "  ,[TipoLiquidacion]    " +
                                               "  ,[CausaLiquida]    " +
                                               "  ,[ContratoNOID]    " +
                                               "  ,[OperacionNOID]    " +
                                               "  ,[ConvencionNOID]    " +
                                               "  ,[FondoSalud]    " +
                                               "  ,[FondoPension]    " +
                                               "  ,[FondoCesantias]    " +
                                               "  ,[CajaNOID]    " +
                                               "  ,[RiesgoNOID]    " +
                                               "  ,[BrigadaNOID]    " +
                                               "  ,[TurnoCompID]    " +
                                               "  ,[RolNOID]    " +
                                               "  ,[SueldoML]    " +
                                               "  ,[SueldoME]    " +
                                               "  ,[TipoContrato]    " +
                                               "  ,[TerminoContrato]    " +
                                               "  ,[FormaPago]    " +
                                               "  ,[DiasContrato]    " +
                                               "  ,[CargoEmpID]    " +
                                               "  ,[AreaFuncionalID]    " +
                                               "  ,[ProyectoID]    " +
                                               "  ,[CentroCostoID]    " +
                                               "  ,[ProcedimientoRteFte]    " +
                                               "  ,[PorcentajeRteFte]    " +
                                               "  ,[DeclaraRentaInd]    " +
                                               "  ,[ApSaludEmpresaValor]    " +
                                               "  ,[ApSaludEmpresaDias]    " +
                                               "  ,[ApSaludOtrosValor]    " +
                                               "  ,[ApSaludOtrosDias]    " +
                                               "  ,[DependenciaInd]    " +
                                               "  ,[BancoID]    " +
                                               "  ,[TipoCuenta]    " +
                                               "  ,[CuentaAbono]    " +
                                               "  ,[EmplConfianzaInd]    " +
                                               "  ,[NoAuxilioTranspInd]    " +
                                               "  ,[NoAporteSaludInd]    " +
                                               "  ,[NoAportePensionInd]    " +
                                               "  ,[DiasPermanencia]    " +
                                               "  ,[Pagos1]    " +
                                               "  ,[Pagos2]    " +
                                               "  ,[Pagos3]    " +
                                               "  ,[Pagos4]    " +
                                               "  ,[Pagos5]    " +
                                               "  ,[Pagos6]    " +
                                               "  ,[Pagos7]    " +
                                               "  ,[Pagos8]    " +
                                               "  ,[Pagos9]    " +
                                               "  ,[Pagos10]    " +
                                               "  ,[Dias1]    " +
                                               "  ,[Dias2]    " +
                                               "  ,[Dias3]    " +
                                               "  ,[Dias4]    " +
                                               "  ,[Dias5]    " +
                                               "  ,[FechaIni1]    " +
                                               "  ,[FechaFin1]    " +
                                               "  ,[FechaIni2]    " +
                                               "  ,[FechaFin2]    " +
                                               "  ,[FechaIni3]    " +
                                               "  ,[FechaFin3]    " +
                                               "  ,[Fecha1]    " +
                                               "  ,[Fecha2]    " +
                                               "  ,[Fecha3]    " +
                                               "  ,[Fecha4]    " +
                                               "  ,[Fecha5]    " +
                                               "  ,[DatoAdd1]    " +
                                               "  ,[DatoAdd2]    " +
                                               "  ,[DatoAdd3]    " +
                                               "  ,[DatoAdd4]    " +
                                               "  ,[DatoAdd5]    " +
                                               "  ,[PagadoInd]    " +
                                               "  ,[Valor1]    " +
                                               "  ,[Valor2]    " +
                                               "  ,[Valor3]    " +
                                               "  ,[Valor4]    " +
                                               "  ,[Valor5]    " +
                                               "  ,[eg_noOperacion]    " +
                                               "  ,[eg_noConvencion]    " +
                                               "  ,[eg_noFondo]    " +
                                               "  ,[eg_noCaja]    " +
                                               "  ,[eg_noRiesgo]    " +
                                               "  ,[eg_noBrigada]    " +
                                               "  ,[eg_noTurnoCompensatorio]    " +
                                               "  ,[eg_noRol]    " +
                                               "  ,[eg_rhCargos]    " +
                                               "  ,[eg_glAreaFuncional]    " +
                                               "  ,[eg_coProyecto]    " +
                                               "  ,[eg_coCentroCosto]    " +
                                               "  ,[eg_tsBanco])    " +
                                               "  VALUES    " +
                                               "  (@EmpresaID   " +
                                               "  ,@NumeroDoc    " +
                                               "  ,@Valor    " +
                                               "  ,@Iva    " +
                                               "  ,@Quincena    " +
                                               "  ,@CausaLiquida    " + 
                                               "  ,@TipoLiquidacion    " +
                                               "  ,@ContratoNOID    " +
                                               "  ,@OperacionNOID    " +
                                               "  ,@ConvencionNOID    " +
                                               "  ,@FondoSalud    " +
                                               "  ,@FondoPension    " +
                                               "  ,@FondoCesantias    " +
                                               "  ,@CajaNOID    " +
                                               "  ,@RiesgoNOID    " +
                                               "  ,@BrigadaNOID    " +
                                               "  ,@TurnoCompID    " +
                                               "  ,@RolNOID    " +
                                               "  ,@SueldoML    " +
                                               "  ,@SueldoME    " +
                                               "  ,@TipoContrato    " +
                                               "  ,@TerminoContrato    " +
                                               "  ,@FormaPago    " +
                                               "  ,@DiasContrato    " +
                                               "  ,@CargoEmpID    " +
                                               "  ,@AreaFuncionalID    " +
                                               "  ,@ProyectoID    " +
                                               "  ,@CentroCostoID    " +
                                               "  ,@ProcedimientoRteFte    " +
                                               "  ,@PorcentajeRteFte    " +
                                               "  ,@DeclaraRentaInd    " +
                                               "  ,@ApSaludEmpresaValor    " +
                                               "  ,@ApSaludEmpresaDias    " +
                                               "  ,@ApSaludOtrosValor    " +
                                               "  ,@ApSaludOtrosDias    " +
                                               "  ,@DependenciaInd    " +
                                               "  ,@BancoID    " +
                                               "  ,@TipoCuenta    " +
                                               "  ,@CuentaAbono    " +
                                               "  ,@EmplConfianzaInd    " +
                                               "  ,@NoAuxilioTranspInd    " +
                                               "  ,@NoAporteSaludInd    " +
                                               "  ,@NoAportePensionInd    " +
                                               "  ,@DiasPermanencia    " +
                                               "  ,@Pagos1    " +
                                               "  ,@Pagos2    " +
                                               "  ,@Pagos3    " +
                                               "  ,@Pagos4    " +
                                               "  ,@Pagos5    " +
                                               "  ,@Pagos6    " +
                                               "  ,@Pagos7    " +
                                               "  ,@Pagos8    " +
                                               "  ,@Pagos9    " +
                                               "  ,@Pagos10    " +
                                               "  ,@Dias1    " +
                                               "  ,@Dias2    " +
                                               "  ,@Dias3    " +
                                               "  ,@Dias4    " +
                                               "  ,@Dias5    " +
                                               "  ,@FechaIni1    " +
                                               "  ,@FechaFin1    " +
                                               "  ,@FechaIni2    " +
                                               "  ,@FechaFin2    " +
                                               "  ,@FechaIni3    " +
                                               "  ,@FechaFin3    " +
                                               "  ,@Fecha1    " +
                                               "  ,@Fecha2    " +
                                               "  ,@Fecha3    " +
                                               "  ,@Fecha4    " +
                                               "  ,@Fecha5    " +
                                               "  ,@DatoAdd1    " +
                                               "  ,@DatoAdd2    " +
                                               "  ,@DatoAdd3    " +
                                               "  ,@DatoAdd4    " +
                                               "  ,@DatoAdd5    " +
                                               "  ,@PagadoInd    " +
                                               "  ,@Valor1    " +
                                               "  ,@Valor2    " +
                                               "  ,@Valor3    " +
                                               "  ,@Valor4    " +
                                               "  ,@Valor5    " +
                                               "  ,@eg_noOperacion    " +
                                               "  ,@eg_noConvencion    " +
                                               "  ,@eg_noFondo    " +
                                               "  ,@eg_noCaja    " +
                                               "  ,@eg_noRiesgo    " +
                                               "  ,@eg_noBrigada    " +
                                               "  ,@eg_noTurnoCompensatorio    " +
                                               "  ,@eg_noRol    " +
                                               "  ,@eg_rhCargos    " +
                                               "  ,@eg_glAreaFuncional    " +
                                               "  ,@eg_coProyecto    " +
                                               "  ,@eg_coCentroCosto    " +
                                               "  ,@eg_tsBanco)   ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Quincena", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoLiquidacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CausaLiquida", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@OperacionNOID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConvencionNOID", SqlDbType.Char, UDT_ConvencionNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FondoPension", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FondoCesantias", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, UDT_CajaNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RiesgoNOID", SqlDbType.Char, UDT_RiesgoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@BrigadaNOID", SqlDbType.Char, UDT_BrigadaNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TurnoCompID", SqlDbType.Char, UDT_TurnoCompID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RolNOID", SqlDbType.Char, UDT_RolNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@SueldoML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SueldoME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoContrato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TerminoContrato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FormaPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DiasContrato", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CargoEmpID", SqlDbType.Char, UDT_CargoEmpID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProcedimientoRteFte", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorcentajeRteFte", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DeclaraRentaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ApSaludEmpresaValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ApSaludEmpresaDias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ApSaludOtrosValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ApSaludOtrosDias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DependenciaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@BancoID", SqlDbType.Char, UDT_BancoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoCuenta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CuentaAbono", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@EmplConfianzaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NoAuxilioTranspInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NoAporteSaludInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@NoAportePensionInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@DiasPermanencia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Pagos1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos9", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Pagos10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Dias1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Dias2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Dias3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Dias4", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Dias5", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaIni1", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin1", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaIni2", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin2", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaIni3", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin3", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Fecha1", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Fecha2", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Fecha3", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Fecha4", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Fecha5", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@PagadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);


                mySqlCommandSel.Parameters["@EmpresaID"].Value = doc.EmpresaID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = doc.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = doc.Iva.Value;
                mySqlCommandSel.Parameters["@Quincena"].Value = doc.Quincena.Value;
                mySqlCommandSel.Parameters["@TipoLiquidacion"].Value = doc.TipoLiquidacion.Value;
                mySqlCommandSel.Parameters["@CausaLiquida"].Value = doc.CausaLiquida.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = doc.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@OperacionNOID"].Value = doc.OperacionNOID.Value;
                mySqlCommandSel.Parameters["@ConvencionNOID"].Value = doc.ConvencionNOID.Value;
                mySqlCommandSel.Parameters["@FondoSalud"].Value = doc.FondoSalud.Value;
                mySqlCommandSel.Parameters["@FondoPension"].Value = doc.FondoPension.Value;
                mySqlCommandSel.Parameters["@FondoCesantias"].Value = doc.FondoCesantias.Value;
                mySqlCommandSel.Parameters["@CajaNOID"].Value = doc.CajaNOID.Value;
                mySqlCommandSel.Parameters["@RiesgoNOID"].Value = doc.RiesgoNOID.Value;
                mySqlCommandSel.Parameters["@BrigadaNOID"].Value = doc.BrigadaNOID.Value;
                mySqlCommandSel.Parameters["@TurnoCompID"].Value = doc.TurnoCompID.Value;
                mySqlCommandSel.Parameters["@RolNOID"].Value = doc.RolNOID.Value;
                mySqlCommandSel.Parameters["@SueldoML"].Value = doc.SueldoML.Value;
                mySqlCommandSel.Parameters["@SueldoME"].Value = doc.SueldoME.Value;
                mySqlCommandSel.Parameters["@TipoContrato"].Value = doc.TipoContrato.Value;
                mySqlCommandSel.Parameters["@TerminoContrato"].Value = doc.TerminoContrato.Value;
                mySqlCommandSel.Parameters["@FormaPago"].Value = doc.FormaPago.Value;
                mySqlCommandSel.Parameters["@DiasContrato"].Value = doc.DiasContrato.Value;
                mySqlCommandSel.Parameters["@CargoEmpID"].Value = doc.CargoEmpID.Value;
                mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = doc.AreaFuncionalID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = doc.ProyectoID.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = doc.CentroCostoID.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@ProcedimientoRteFte"].Value = doc.ProcedimientoRteFte.Value;
                mySqlCommandSel.Parameters["@PorcentajeRteFte"].Value = doc.PorcentajeRteFte.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@DeclaraRentaInd"].Value = doc.DeclaraRentaInd.Value;
                mySqlCommandSel.Parameters["@ApSaludEmpresaValor"].Value = doc.ApSaludEmpresaValor.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@ApSaludEmpresaDias"].Value = doc.ApSaludEmpresaDias.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@ApSaludOtrosValor"].Value = doc.ApSaludOtrosValor.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@ApSaludOtrosDias"].Value = doc.ApSaludOtrosDias.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@DependenciaInd"].Value = doc.DependenciaInd.Value;
                mySqlCommandSel.Parameters["@BancoID"].Value = !string.IsNullOrEmpty(doc.BancoID.Value) ? doc.BancoID.Value : SqlString.Null;
                mySqlCommandSel.Parameters["@TipoCuenta"].Value = doc.TipoCuenta.Value ?? SqlInt16.Null;
                mySqlCommandSel.Parameters["@CuentaAbono"].Value = doc.CuentaAbono.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@EmplConfianzaInd"].Value = doc.EmplConfianzaInd.Value ?? SqlBoolean.Null;
                mySqlCommandSel.Parameters["@NoAuxilioTranspInd"].Value = doc.NoAuxilioTranspInd.Value ?? SqlBoolean.Null;
                mySqlCommandSel.Parameters["@NoAporteSaludInd"].Value = doc.NoAporteSaludInd.Value ?? SqlBoolean.Null;
                mySqlCommandSel.Parameters["@NoAportePensionInd"].Value = doc.NoAportePensionInd.Value ?? SqlBoolean.Null;
                mySqlCommandSel.Parameters["@DiasPermanencia"].Value = doc.DiasPermanencia.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@Pagos1"].Value = doc.Pagos1.Value;
                mySqlCommandSel.Parameters["@Pagos2"].Value = doc.Pagos2.Value;
                mySqlCommandSel.Parameters["@Pagos3"].Value = doc.Pagos3.Value;
                mySqlCommandSel.Parameters["@Pagos4"].Value = doc.Pagos4.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Pagos5"].Value = doc.Pagos5.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Pagos6"].Value = doc.Pagos6.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Pagos7"].Value = doc.Pagos7.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Pagos8"].Value = doc.Pagos8.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Pagos9"].Value = doc.Pagos9.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Pagos10"].Value = doc.Pagos10.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Dias1"].Value = doc.Dias1.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@Dias2"].Value = doc.Dias2.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@Dias3"].Value = doc.Dias3.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@Dias4"].Value = doc.Dias4.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@Dias5"].Value = doc.Dias5.Value ?? SqlInt32.Null;
                mySqlCommandSel.Parameters["@FechaIni1"].Value = doc.FechaIni1.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@FechaFin1"].Value = doc.FechaFin1.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@FechaIni2"].Value = doc.FechaIni2.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@FechaFin2"].Value = doc.FechaFin2.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@FechaIni3"].Value = doc.FechaIni3.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@FechaFin3"].Value = doc.FechaFin3.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@Fecha1"].Value = doc.Fecha1.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@Fecha2"].Value = doc.Fecha2.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@Fecha3"].Value = doc.Fecha3.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@Fecha4"].Value = doc.Fecha4.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@Fecha5"].Value = doc.Fecha5.Value ?? SqlDateTime.Null;
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = doc.DatoAdd1.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value = doc.DatoAdd2.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value = doc.DatoAdd3.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value = doc.DatoAdd4.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value = doc.DatoAdd5.Value ?? SqlString.Null;
                mySqlCommandSel.Parameters["@PagadoInd"].Value = doc.PagadoInd.Value ?? SqlBoolean.Null;
                mySqlCommandSel.Parameters["@Valor1"].Value = doc.Valor1.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Valor2"].Value = doc.Valor1.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Valor3"].Value = doc.Valor3.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Valor4"].Value = doc.Valor4.Value ?? SqlDecimal.Null;
                mySqlCommandSel.Parameters["@Valor5"].Value = doc.Valor5.Value ?? SqlDecimal.Null;

                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el numero de documento asociado al numero de contrato
        /// </summary>
        /// <param name="contratoNOID">numero de contrato</param>
        /// <returns>numero de documento</returns>
        public int DAL_noLiquidacionesDocu_GetByContrato(int contratoNOID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  select NumeroDoc   " +
                                              "  from noLiquidacionesDocu   " +
                                              "  where ContratoNOID = @ContratoNOID";

                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@ContratoNOID"].Value = contratoNOID;

                object numDoc = mySqlCommandSel.ExecuteScalar();
                return numDoc != null ? (int)numDoc : 0;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetByContrato");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el documento asociado al numero de documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>documento de liquidación</returns>
        public DTO_noLiquidacionesDocu DAL_noLiquidacionesDocu_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  select *   " +
                                              "  from noLiquidacionesDocu   " +
                                              "  where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                DTO_noLiquidacionesDocu result = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_noLiquidacionesDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetByContrato");
                throw exception;
            }
        }

        /// <summary>
        /// Cambia el indicador de procesamiento del documento
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <param name="estado">estado</param>
        public void DAL_noLiquidacionesDocu_ChangeStatus(int numeroDoc, bool estado)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  UPDATE noLiquidacionesDocu SET ProcesadoInd = @ProcesadoInd   " +
                                              "  WHERE NumeroDoc = @NumeroDoc";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = estado;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_ChangeStatus");
                throw exception;
            }
        }

        /// <summary>
        /// Proceso Encargado de Generar la Nomina Definita
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        public void DAL_noLiquidacionesDocu_LiquidarNomina(int numeroDoc, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_AprobarNomina", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);



                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_LiquidarNomina");
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo">perido del documento</param>
        /// <param name="actividadID">identificador de la actividad</param>
        /// <returns>lista de documentos de liquidación</returns>
        public List<DTO_noLiquidacionesDocu> DAL_noLiquidacionesDocu_GetDocumentsByActivity(DateTime periodo,EstadoDocControl estado, string actividadFlujoID)
        {
            try
            {
                List<DTO_noLiquidacionesDocu> results = new List<DTO_noLiquidacionesDocu>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  SELECT noLiquidacionesDocu.*  " +
                                                "  FROM glDocumentoControl  " +
                                                "    INNER JOIN noLiquidacionesDocu ON noLiquidacionesDocu.NumeroDoc = glDocumentoControl.NumeroDoc  " +
                                                "    INNER JOIN glActividadEstado ON glActividadEstado.NumeroDoc = glDocumentoControl.NumeroDoc  " +
                                                "    WHERE glDocumentoControl.EmpresaID = @EmpresaID  AND glDocumentoControl.Estado = @EstadoDoc " +
                                                "    AND  glDocumentoControl.PeriodoDoc = @PeriodoID" +
                                                "    AND glActividadEstado.ActividadFlujoID = @ActividadFlujoID  " +
                                                "    AND glActividadEstado.CerradoInd  = 0 ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EstadoDoc", SqlDbType.SmallInt);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommandSel.Parameters["@EstadoDoc"].Value = (byte)estado;

                DTO_noLiquidacionesDocu doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noLiquidacionesDocu(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetDocumentsByActivity");
                throw exception;
            }
        }

        /// <summary>
        /// Proceso encargadode Cerrar la Nomina 
        /// </summary>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion de error</param>
        public void DAL_noLiquidacionesDocu_CierreNomina(out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_CierrePeriodo", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value; 

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_CierreNomina");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados por periodo
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="tipoLiquida">Tipo de liquidacion</param>
        /// <returns>listado de liquidaciones aprobadas para generar boletas pago</returns>
        public List<DTO_NominaEnvioBoleta> DAL_noLiquidacionesDocu_GetNominaLiquida(DateTime periodo, byte tipoLiquida,bool quincenal)
        {
            try
            {
                List<DTO_NominaEnvioBoleta> results = new List<DTO_NominaEnvioBoleta>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string where = string.Empty;
                if (quincenal)
                    where = " and  docu.Quincena = (CASE WHEN DAY(@PeriodoID) <=15 THEN 1 ELSE 2 END) ";
                mySqlCommandSel.CommandText = "Select ctrl.NumeroDoc, ctrl.PeriodoDoc as PeriodoID,ctrl.TerceroID,ter.Descriptivo as Nombre,emp.CorreoElectronico, docu.Valor from noLiquidacionesDocu docu  " +
                                               "     inner join glDocumentoControl ctrl  (nolock) on ctrl.NumeroDoc = docu.NumeroDoc  " +
                                               "     inner join coTercero ter  (nolock) on ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID = ctrl.eg_coTercero " +
                                               "     inner join noEmpleado emp  (nolock) on emp.TerceroID = ter.TerceroID and emp.eg_coTercero = ter.EmpresaGrupoID " +
                                               " Where docu.EmpresaID = @EmpresaID and MONTH(ctrl.PeriodoDoc) = MONTH(@PeriodoID) AND  YEAR(ctrl.PeriodoDoc) = YEAR(@PeriodoID) " +
                                               "     and docu.TipoLiquidacion = @TipoLiquidacion and ctrl.Estado = 3  " + where;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TipoLiquidacion", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@TipoLiquidacion"].Value = tipoLiquida;

                DTO_NominaEnvioBoleta doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_NominaEnvioBoleta(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetNominaLiquida");
                throw exception;
            }
        }

        #region Reversiones

        /// <summary>
        /// Procedimiento que revierte un documento de Nomina 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <param name="empleadoID">identificación del Empleado</param>
        public void DAL_noLiquidacionesDocu_RevertirLiqNomina(int documentoID, DateTime periodo, string empleadoID)
        {
            string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
            SqlCommand mySqlCommandSel = new SqlCommand("Nomina_RevertirLiquidacionNomina", base.MySqlConnection.CreateCommand().Connection);
            mySqlCommandSel.Transaction = base.MySqlConnectionTx;
            mySqlCommandSel.CommandType = CommandType.StoredProcedure;

            mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
            mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
            mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
            mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);

            mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
            mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
            mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
            mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;                
            mySqlCommandSel.ExecuteNonQuery();                           
        }

        #endregion

        #region Cesantias e Intereses de Cesantias

        /// <summary>
        /// Actualiza los valores de cesantias ó intereses de cesantias
        /// </summary>
        /// <param name="numeroDoc">numero Doc</param>
        /// <param name="valorCesantias">valor cesantias</param>
        /// <param name="valorIntereses">valor intereses</param>
        /// <param name="indCesantias">ind cesantias</param>
        public void DAL_noLiquidacionesDocu_UpdateCesantias(int numeroDoc, decimal valorCesantias, decimal valorIntereses, bool indCesantias)
        {
            string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

            SqlCommand mySqlCommandSel = new SqlCommand("Nomina_UpdateValorCesantias", base.MySqlConnection.CreateCommand().Connection);
            mySqlCommandSel.Transaction = base.MySqlConnectionTx;
            mySqlCommandSel.CommandType = CommandType.StoredProcedure;

            mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
            mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
            mySqlCommandSel.Parameters.Add("@ValorCesantias", SqlDbType.Decimal);
            mySqlCommandSel.Parameters.Add("@ValorInteresesCesantias", SqlDbType.Decimal);
            mySqlCommandSel.Parameters.Add("@IndCesantias", SqlDbType.Bit);
                
            mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
            mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
            mySqlCommandSel.Parameters["@ValorCesantias"].Value = valorCesantias;
            mySqlCommandSel.Parameters["@ValorInteresesCesantias"].Value = valorIntereses;
            mySqlCommandSel.Parameters["@IndCesantias"].Value = indCesantias;

            mySqlCommandSel.ExecuteNonQuery();
        }

        #endregion

        #region Contabilizacion

        /// <summary>
        /// Obtiene el consolidado de las liquidacion de nomina X periodo
        /// </summary>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <returns></returns>
        public List<DTO_NominaContabilizacion> DAL_noLiquidacionesDocu_GetTotal(DateTime periodo)
        {
            try
            {
                List<DTO_NominaContabilizacion> results = new List<DTO_NominaContabilizacion>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  Select noLiquidacionesDocu.TipoLiquidacion as Liquidacion,    " + 
	                                          "  glDocumentoControl.PeriodoDoc as Periodo, 	       " +
	                                          "  sum(noLiquidacionesDocu.Valor) as Valor    " +
                                        "  from glDocumentoControl    " +
                                        "  inner join noLiquidacionesDocu on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc    " +
                                        "  where    " +
                                        "  glDocumentoControl.EmpresaID =  @EmpresaID    " +
                                        "  and glDocumentoControl.PeriodoDoc = @PeriodoID    " +
                                        "  and noLiquidacionesDocu.ProcesadoInd = 0    " +
                                        "  and glDocumentoControl.Estado = 3    " +
                                        "  and ( noLiquidacionesDocu.TipoLiquidacion = 1 " +
	                                            "  or noLiquidacionesDocu.TipoLiquidacion = 2  " +
	                                            "  or noLiquidacionesDocu.TipoLiquidacion = 3  " +
	                                            "  or noLiquidacionesDocu.TipoLiquidacion = 4  " +
	                                            "  or noLiquidacionesDocu.TipoLiquidacion = 5  " +
                                                "  or noLiquidacionesDocu.TipoLiquidacion = 6  " +
                                                "  or noLiquidacionesDocu.TipoLiquidacion = 7  " +
                                                "  )    " +
                                        "  group by noLiquidacionesDocu.TipoLiquidacion,    " + 
	                                                "  glDocumentoControl.PeriodoDoc    ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;

                DTO_NominaContabilizacion doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_NominaContabilizacion(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetTotal");
                throw exception;
            }
        }

        /// <summary>
        /// Obtine el detalle de la liquidacion 
        /// </summary>
        /// <param name="periodo">perido</param>
        /// <param name="tipoLiquidacion">tipo liquidacion</param>
        /// <returns>listado de detalles</returns>
        public List<DTO_NominaContabilizacionDetalle> DAL_noLiquidacionesDocu_GetTotalDetalle(DateTime periodo, int tipoLiquidacion)
        {
            try
            {
                List<DTO_NominaContabilizacionDetalle> results = new List<DTO_NominaContabilizacionDetalle>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    Select noEmpleado.EmpleadoID,    " + 
		                                      "          noEmpleado.Descriptivo as Empleado,    " +     
		                                      "          noLiquidacionesDocu.CentroCostoID,    " +
		                                      "          noLiquidacionesDocu.ProyectoID,    " +   
		                                      "          noLiquidacionesDocu.Valor as ValorDetalle,    " +     
                                              "          noLiquidacionesDocu.TipoLiquidacion as Liquidacion   " +  
                                              "    from glDocumentoControl    " +      
                                              "    inner join noLiquidacionesDocu on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc    " + 
                                              "    inner join noEmpleado on  noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID    " +    
                                              "    where      glDocumentoControl.EmpresaID =  @EmpresaID    " +     
                                              "    and glDocumentoControl.PeriodoDoc = @PeriodoID    " +
                                              "    and glDocumentoControl.Estado = 3    " +   
                                              "    and noLiquidacionesDocu.TipoLiquidacion = @TipoLiquidacion    ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TipoLiquidacion", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@TipoLiquidacion"].Value = tipoLiquidacion;                                      

                DTO_NominaContabilizacionDetalle doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_NominaContabilizacionDetalle(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetTotalDetalle");
                throw exception;
            }
        }

        /// <summary>
        /// Obtine el detalle de la liquidacion 
        /// </summary>
        /// <param name="periodo">perido</param>
        /// <param name="tipoLiquidacion">tipo liquidacion</param>
        /// <returns>listado de detalles</returns>
        public List<DTO_NominaContabilizacionPlanillaDetalle> DAL_noLiquidacionesDocu_GetTotalPlanilaDetalle(DateTime periodo, int tipoLiquidacion)
        {
            try
            {
                List<DTO_NominaContabilizacionPlanillaDetalle> results = new List<DTO_NominaContabilizacionPlanillaDetalle>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   SELECT noEmpleado.EmpleadoID, " +
	                                          "    noEmpleado.Descriptivo Empleado, " +
	                                          "         noLiquidacionesDocu.ProyectoID, " +
	                                          "         noLiquidacionesDocu.CentroCostoID, " +
	                                          "         noPlanillaAportesDeta.VlrEmpresaPEN, " +
	                                          "         noPlanillaAportesDeta.VlrTrabajadorPEN, " +
	                                          "         noPlanillaAportesDeta.VlrSolidaridad, " +
	                                          "         noPlanillaAportesDeta.VlrSubsistencia, " +
	                                          "         noPlanillaAportesDeta.VlrEmpresaSLD, " +
	                                          "         noPlanillaAportesDeta.VlrTrabajadorSLD, " +
	                                          "         noPlanillaAportesDeta.VlrARP, " +
	                                          "         noPlanillaAportesDeta.VlrCCF, " +
	                                          "         noPlanillaAportesDeta.VlrSEN, " +
	                                          "         noPlanillaAportesDeta.VlrIBF " +
                                            "    FROM glDocumentoControl " +
                                            "    INNER JOIN noLiquidacionesDocu ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                                            "    INNER JOIN noPlanillaAportesDeta ON noLiquidacionesDocu.NumeroDoc = noPlanillaAportesDeta.NumeroDoc " +
                                            "    INNER JOIN noEmpleado ON noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID " +
                                            "    WHERE glDocumentoControl.EmpresaID = @EmpresaID " +
                                            "    AND glDocumentoControl.PeriodoDoc = @PeriodoID " +
                                            "    AND glDocumentoControl.Estado = 3 " +
                                            "    AND noLiquidacionesDocu.TipoLiquidacion = @TipoLiquidacion"; 

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TipoLiquidacion", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@TipoLiquidacion"].Value = tipoLiquidacion;

                DTO_NominaContabilizacionPlanillaDetalle doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_NominaContabilizacionPlanillaDetalle(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetTotalPlanilaDetalle");
                throw exception;
            }
        }

        /// <summary>
        /// Obtine el detalle de la liquidacion 
        /// </summary>
        /// <param name="periodo">perido</param>
        /// <param name="tipoLiquidacion">tipo liquidacion</param>
        /// <returns>listado de detalles</returns>
        public List<DTO_NominaContabilizacionProvisionDetalle> DAL_noLiquidacionesDocu_GetTotalProvisionesDetalle(DateTime periodo, int tipoLiquidacion)
        {
            try
            {
                List<DTO_NominaContabilizacionProvisionDetalle> results = new List<DTO_NominaContabilizacionProvisionDetalle>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "     SELECT noEmpleado.EmpleadoID, "   + 
	                                          "    noEmpleado.Descriptivo Empleado, "   + 
	                                          "         noLiquidacionesDocu.ProyectoID, "   + 
	                                          "         noLiquidacionesDocu.CentroCostoID, "   + 
	                                          "         SUM(noProvisionDeta.VlrProvisionMES) as ValorProvision "   + 
                                            "    FROM glDocumentoControl "   + 
                                            "    INNER JOIN noLiquidacionesDocu ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc "   + 
                                            "    INNER JOIN noProvisionDeta ON noLiquidacionesDocu.NumeroDoc = noProvisionDeta.NumeroDoc "   + 
                                            "    INNER JOIN noEmpleado ON noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID "   + 
                                            "    WHERE glDocumentoControl.EmpresaID = @EmpresaID "   + 
                                            "    AND glDocumentoControl.PeriodoDoc = @PeriodoID "   + 
                                            "    AND glDocumentoControl.Estado = 3 "   +
                                            "    and noLiquidacionesDocu.TipoLiquidacion = @TipoLiquidacion " + 
                                            "    GROUP BY noEmpleado.EmpleadoID, "   + 
	                                               "    noEmpleado.Descriptivo, "   +
                                                   "    noLiquidacionesDocu.ProyectoID, " + 
	                                               "    noLiquidacionesDocu.CentroCostoID";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TipoLiquidacion", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@TipoLiquidacion"].Value = tipoLiquidacion;

                DTO_NominaContabilizacionProvisionDetalle doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_NominaContabilizacionProvisionDetalle(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_GetTotalProvisionesDetalle");
                throw exception;
            }
        }

        #endregion

        #region Indicadores de Pago y Contabilizacion

        /// <summary>
        /// update pagadoInd in table noLiquidacionesDocu
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="pagadoInd">pagadoInd</param>
        public void DAL_noLiquidacionesDocu_UpdatePagadoInd(int numeroDoc, bool pagadoInd)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " UPDATE noLiquidacionesDocu " +
                                              " SET PagadoInd = @PagadoInd" +
                                              " WHERE EmpresaID = @EmpresaID " +
                                              " AND NumeroDoc = @NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PagadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@PagadoInd"].Value = pagadoInd;              

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_UpdatePagadoInd");
                throw exception;
            }
        }

        /// <summary>
        /// update procesadoInd in table noLiquidacionesDocu
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="procesadoInd">procesadoInd</param>
        public void DAL_noLiquidacionesDocu_UpdateProcesadoInd(int numeroDoc, bool procesadoInd)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " UPDATE noLiquidacionesDocu " +
                                              " SET ProcesadoInd = @ProcesadoInd" +
                                              " WHERE EmpresaID = @EmpresaID " +
                                              " AND NumeroDoc = @NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = procesadoInd;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_UpdateProcesadoInd");
                throw exception;
            }
        }

        /// <summary>
        /// update pagadoInd in table noLiquidacionesDocu for Planilla
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="pagadoInd">pagadoInd</param>
        public void DAL_noLiquidacionesDocu_UpdatePagadoPlanillaInd(bool pagadoInd, DateTime periodo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   UPDATE R    " +
                                              "   SET R.PagadoInd = @PagadoInd   " +
                                              "   FROM noLiquidacionesDocu AS R   " +
                                              "   INNER JOIN glDocumentoControl as G ON G.NumeroDoc = R.NumeroDoc   " +
                                              "   INNER JOIN noPlanillaAportesDeta as N  ON N.NumeroDoc = G.NumeroDoc   " +
                                              "   WHERE G.EmpresaID = @EmpresaID   " +
                                              "   and G.PeriodoDoc = @Periodo ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@PagadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@PagadoInd"].Value = pagadoInd;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_UpdatePagadoInd");
                throw exception;
            }
        }

        /// <summary>
        /// update procesadoInd in table noLiquidacionesDocu for Planilla
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="procesadoInd">procesadoInd</param>
        public void DAL_noLiquidacionesDocu_UpdateProcesadoPlanillaInd(bool procesadoInd, DateTime periodo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "   UPDATE R    " +
                                              "   SET R.ProcesadoInd = @ProcesadoInd   " +
                                              "   FROM noLiquidacionesDocu AS R   " +
                                              "   INNER JOIN glDocumentoControl as G ON G.NumeroDoc = R.NumeroDoc   " +
                                              "   INNER JOIN noPlanillaAportesDeta as N  ON N.NumeroDoc = G.NumeroDoc   " +
                                              "   WHERE G.EmpresaID = @EmpresaID   " +
                                              "   and G.PeriodoDoc = @Periodo ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ProcesadoInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@ProcesadoInd"].Value = procesadoInd;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_UpdateProcesadoPlanillaInd");
                throw exception;
            }
        }    

        #endregion

        #region Vacaciones 

        /// <summary>
        /// Obtiene el periodo de vacaciones a liquidar
        /// </summary>
        /// <param name="empleadoId">identificador del empleado</param>
        /// <param name="estado">estado de la liquidacion</param>
        public List<DTO_noLiquidacionVacacionesDeta> DAL_noLiquidacionesDocu_GetPeriodoVacaciones(string empleadoId, bool estado)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " select *   " +
                                              "   from noLiquidacionVacacionesDeta    " +
                                              "   where EmpresaID = @EmpresaID    " +
                                              "   and EmpleadoID = @EmpleadoID    " +
                                              "   and Estado = @Estado    " +
                                              "   order by PeriodoInicial";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoId;
                mySqlCommandSel.Parameters["@Estado"].Value = estado;

                List<DTO_noLiquidacionVacacionesDeta> results = new List<DTO_noLiquidacionVacacionesDeta>();
                DTO_noLiquidacionVacacionesDeta doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noLiquidacionVacacionesDeta(dr);
                    results.Add(doc);
                }
                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noLiquidacionesDocu_ChangeStatus");
                throw exception;
            }
        }


        #endregion

    }
}
