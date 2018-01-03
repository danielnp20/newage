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

namespace NewAge.ADO
{
    public class DAL_noPlanillaAportesDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noPlanillaAportesDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
              
        /// <summary>
        /// Proceso que liquida la Planilla de Aportes de un empleado
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de pago</param>
        /// <param name="numeroDoc">numero de documento</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripción del error</param>
        public void DAL_noPlanillaAportesDeta_LiquidarPlanilla(string empleadoID, DateTime periodo, int numeroDoc, decimal tasaCambio, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarPlanilla", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;

                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);

                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;              
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaAportesDeta_LiquidarPlanilla");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public DTO_noPlanillaAportesDeta DAL_noPlanillaAportesDeta_Get(string empleadoID, DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "     SELECT noPlanillaAportesDeta.*  "   +
                                              "     FROM glDocumentoControl  "   +
                                              "     INNER JOIN noLiquidacionesDocu ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc  "   +
                                              "     INNER JOIN noPlanillaAportesDeta ON noLiquidacionesDocu.NumeroDoc = noPlanillaAportesDeta.NumeroDoc  " +
                                              "     INNER JOIN noEmpleado ON noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID  " +
                                              "     WHERE glDocumentoControl.EmpresaID = @EmpresaID  "   +
                                              "     AND glDocumentoControl.DocumentoID  = @DocumentoID  "   +
                                              "     AND glDocumentoControl.PeriodoDoc = @PeriodoDoc  " +
                                              "     AND noEmpleado.EmpleadoID = @EmpleadoID";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;
                mySqlCommandSel.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                DTO_noPlanillaAportesDeta result = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_noPlanillaAportesDeta(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaAportesDeta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="periodo">periodo de liquidacion de la Nomina</param>
        /// <returns>Planilla de Aportes</returns>
        public List<DTO_noPlanillaAportesDeta> DAL_noPlanillaAportesDeta_GetByPeriodo(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "     SELECT noPlanillaAportesDeta.*  " +
                                              "     FROM glDocumentoControl  " +
                                              "     INNER JOIN noLiquidacionesDocu ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc  " +
                                              "     INNER JOIN noPlanillaAportesDeta ON noLiquidacionesDocu.NumeroDoc = noPlanillaAportesDeta.NumeroDoc  " +
                                              "     INNER JOIN noEmpleado ON noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID  " +
                                              "     WHERE glDocumentoControl.EmpresaID = @EmpresaID  " +
                                              "     AND glDocumentoControl.DocumentoID  = @DocumentoID  " +
                                              "     AND glDocumentoControl.PeriodoDoc = @PeriodoDoc  ";                                             

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
             
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;
                mySqlCommandSel.Parameters["@PeriodoDoc"].Value = periodo;

                List<DTO_noPlanillaAportesDeta> results = new List<DTO_noPlanillaAportesDeta>();
                DTO_noPlanillaAportesDeta result = new DTO_noPlanillaAportesDeta();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    result = new DTO_noPlanillaAportesDeta(dr);
                    results.Add(result);
                }
                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaAportesDeta_GetByPeriodo");
                throw exception;
            }
        }

        /// <summary>
        /// Guarda la información de la Planilla Liquidada
        /// </summary>
        /// <param name="planilla">planilla</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripción de error</param>
        public void DAL_noPlanillaAportesDeta_Upd(DTO_noPlanillaAportesDeta planilla)
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "     UPDATE  noPlanillaAportesDeta   " +
	                                          "     SET  [TipoDocNomina] = @TipoDocNomina   " +
		                                      "     ,[TipoCotizante] = @TipoCotizante   " +
		                                      "     ,[SubTipo] = @SubTipo   " +
		                                      "     ,[ExtranjeroInd] = @ExtranjeroInd   " +
		                                      "     ,[ExteriorInd] = @ExteriorInd   " +
		                                          "     ,[Ciudad] = @Ciudad   " +
		                                          "     ,[Sueldo] = @Sueldo   " +
		                                          "     ,[SalIntegralInd] = @SalIntegralInd   " +
		                                          "     ,[INGrInd] = @INGrInd   " +
		                                          "     ,[RETInd] = @RETInd   " +
		                                          "     ,[TDEInd] = @TDEInd   " +
		                                          "     ,[TAEInd] = @TAEInd   " +
		                                          "     ,[TDPInd] = @TDPInd   " +
		                                          "     ,[TAPInd] = @TAPInd   " +
		                                          "     ,[VSPInd] = @VSPInd   " +
		                                          "     ,[VTEInd] = @VTEInd   " +
		                                          "     ,[VSTInd] = @VSTInd   " +
		                                          "     ,[SLNInd] = @SLNInd   " +
		                                          "     ,[IGEInd] = @IGEInd   " +
		                                          "     ,[LMAInd] = @LMAInd   " +
		                                          "     ,[VACInd] = @VACInd   " +
		                                          "     ,[AVPInd] = @AVPInd   " +
		                                          "     ,[VCTInd] = @VCTInd   " +
		                                          "     ,[IRPInd] = @IRPInd   " +
		                                          "     ,[FondoPension] = @FondoPension   " +
		                                          "     ,[FondoPensionTR] = @FondoPensionTR   " +
		                                          "     ,[DiasCotizadosPEN] = @DiasCotizadosPEN   " +
		                                          "     ,[IngresoBasePEN] = @IngresoBasePEN   " +
		                                          "     ,[TarifaPEN] = @TarifaPEN   " +
		                                          "     ,[VlrTrabajadorPEN] = @VlrTrabajadorPEN   " +
		                                          "     ,[VlrEmpresaPEN] = @VlrEmpresaPEN   " +
		                                          "     ,[VlrTrabajadorVOL] = @VlrTrabajadorVOL   " +
		                                          "     ,[VlrEmpresaVOL] = @VlrEmpresaVOL   " +
		                                          "     ,[VlrSolidaridad] = @VlrSolidaridad   " +
		                                          "     ,[VlrSubsistencia] = @VlrSubsistencia   " +
		                                          "     ,[VlrNoRetenido] = @VlrNoRetenido   " +
		                                          "     ,[FondoSalud] = @FondoSalud   " +
		                                          "     ,[FondoSaludTR] = @FondoSaludTR   " +
		                                          "     ,[DiasCotizadosSLD] = @DiasCotizadosSLD   " +
		                                          "     ,[IngresoBaseSLD] = @IngresoBaseSLD   " +
		                                          "     ,[TarifaSLD] = @TarifaSLD   " +
		                                          "     ,[VlrTrabajadorSLD] = @VlrTrabajadorSLD   " +
		                                          "     ,[VlrEmpresaSLD] = @VlrEmpresaSLD   " +
		                                          "     ,[VlrUPC] = @VlrUPC   " +
		                                          "     ,[AutorizacionEnf] = @AutorizacionEnf   " +
		                                          "     ,[VlrIncapacidad] = @VlrIncapacidad   " +
		                                          "     ,[AutorizacioMat] = @AutorizacioMat   " +
		                                          "     ,[VlrMaternidad] = @VlrMaternidad   " +
		                                          "     ,[DiasCotizadosARP] = @DiasCotizadosARP   " +
		                                          "     ,[IngresoBaseARP] = @IngresoBaseARP   " +
		                                          "     ,[TarifaARP] = @TarifaARP   " +
		                                          "     ,[CtoARP] = @CtoARP   " +
		                                          "     ,[VlrARP] = @VlrARP   " +
		                                          "     ,[CajaNOID] = @CajaNOID   " +
		                                          "     ,[DiasCotizadosCCF] = @DiasCotizadosCCF   " +
		                                          "     ,[IngresoBaseCCF] = @IngresoBaseCCF   " +
		                                          "     ,[TarifaCCF] = @TarifaCCF   " +
		                                          "     ,[VlrCCF] = @VlrCCF   " +
		                                          "     ,[TarifaIBF] = @TarifaIBF   " +
		                                          "     ,[VlrSEN] = @VlrSEN   " +
		                                          "     ,[IndExoneradoCCF] = @IndExoneradoCCF   " +
		                                          "     ,[VlrIBF] = @VlrIBF   " +
		                                          "     ,[TarifaSEN] = @TarifaSEN   " +
                                         "     WHERE noPlanillaAportesDeta.EmpresaID = @EmpresaID   " +
                                         "     AND noPlanillaAportesDeta.NumeroDoc = @NumeroDoc";

                   
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters.Add("@TipoDocNomina", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoCotizante", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@SubTipo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ExtranjeroInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ExteriorInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@Sueldo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SalIntegralInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@INGrInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@RETInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TDEInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TAEInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TDPInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TAPInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VSPInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VTEInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VSTInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@SLNInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IGEInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@LMAInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VACInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AVPInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VCTInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IRPInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FondoPension", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@FondoPensionTR", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@DiasCotizadosPEN", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IngresoBasePEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaPEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrTrabajadorPEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrEmpresaPEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrTrabajadorVOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrEmpresaVOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSolidaridad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSubsistencia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrNoRetenido", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@FondoSaludTR", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@DiasCotizadosSLD", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IngresoBaseSLD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaSLD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrTrabajadorSLD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrEmpresaSLD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrUPC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AutorizacionEnf", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@VlrIncapacidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AutorizacioMat", SqlDbType.Char, 15);
                mySqlCommandSel.Parameters.Add("@VlrMaternidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DiasCotizadosARP", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IngresoBaseARP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaARP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoARP", SqlDbType.Char, 9);
                mySqlCommandSel.Parameters.Add("@VlrARP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@DiasCotizadosCCF", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IngresoBaseCCF", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaCCF", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrCCF", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaICBF", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IndExoneradoCCF", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@VlrIBF", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaSEN", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TarifaIBF", SqlDbType.Decimal);

                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noFondo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConvencion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCaja", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRiesgo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noBrigada", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noTurnoCompensatorio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noRol", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_rhCargos", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_tsBanco", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                           

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = planilla.NumeroDoc.Value.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = planilla.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommandSel.Parameters["@TipoDocNomina"].Value = planilla.TipoDocNomina.Value.Value;
                mySqlCommandSel.Parameters["@TipoCotizante"].Value = planilla.TipoCotizante.Value;
                mySqlCommandSel.Parameters["@SubTipo"].Value = planilla.SubTipo.Value.Value;
                mySqlCommandSel.Parameters["@ExtranjeroInd"].Value = 0;
                mySqlCommandSel.Parameters["@ExteriorInd"].Value = 0;
                mySqlCommandSel.Parameters["@Ciudad"].Value = planilla.LugarGeograficoID.Value;
                mySqlCommandSel.Parameters["@Sueldo"].Value = planilla.Sueldo.Value.Value;
                mySqlCommandSel.Parameters["@SalIntegralInd"].Value = planilla.SalIntegralInd.Value;
                mySqlCommandSel.Parameters["@INGrInd"].Value = planilla.INGInd.Value;
                mySqlCommandSel.Parameters["@RETInd"].Value = planilla.RETInd.Value;
                mySqlCommandSel.Parameters["@TDEInd"].Value = planilla.TDEInd.Value;
                mySqlCommandSel.Parameters["@TAEInd"].Value = planilla.TAEInd.Value;
                mySqlCommandSel.Parameters["@TDPInd"].Value = planilla.TDPInd.Value;
                mySqlCommandSel.Parameters["@TAPInd"].Value = planilla.TAPInd.Value;
                mySqlCommandSel.Parameters["@VSPInd"].Value = planilla.VSPInd.Value;
                mySqlCommandSel.Parameters["@VTEInd"].Value = planilla.VTEInd.Value;
                mySqlCommandSel.Parameters["@VSTInd"].Value = planilla.VSTInd.Value;
                mySqlCommandSel.Parameters["@SLNInd"].Value = planilla.SLNInd.Value;
                mySqlCommandSel.Parameters["@IGEInd"].Value = planilla.IGEInd.Value;
                mySqlCommandSel.Parameters["@LMAInd"].Value = planilla.LMAInd.Value;
                mySqlCommandSel.Parameters["@VACInd"].Value = planilla.VACInd.Value;
                mySqlCommandSel.Parameters["@AVPInd"].Value = planilla.AVPInd.Value;
                mySqlCommandSel.Parameters["@VCTInd"].Value = planilla.VCTInd.Value;
                mySqlCommandSel.Parameters["@IRPInd"].Value = planilla.IRPInd.Value;
                mySqlCommandSel.Parameters["@FondoPension"].Value = planilla.FondoPension.Value;
                mySqlCommandSel.Parameters["@FondoPensionTR"].Value = !string.IsNullOrEmpty(planilla.FondoPensionTR.Value) ? planilla.FondoPensionTR.Value : planilla.FondoPension.Value;
                mySqlCommandSel.Parameters["@DiasCotizadosPEN"].Value = planilla.DiasCotizadosPEN.Value.Value;
                mySqlCommandSel.Parameters["@IngresoBasePEN"].Value = planilla.IngresoBasePEN.Value.Value;
                mySqlCommandSel.Parameters["@TarifaPEN"].Value = planilla.TarifaPEN.Value.Value;
                mySqlCommandSel.Parameters["@VlrTrabajadorPEN"].Value = planilla.VlrTrabajadorPEN.Value.Value;
                mySqlCommandSel.Parameters["@VlrEmpresaPEN"].Value = planilla.VlrEmpresaPEN.Value.Value;
                mySqlCommandSel.Parameters["@VlrTrabajadorVOL"].Value = 0;
                mySqlCommandSel.Parameters["@VlrEmpresaVOL"].Value = planilla.VlrEmpresaVOL.Value.Value;
                mySqlCommandSel.Parameters["@VlrSolidaridad"].Value = planilla.VlrSolidaridad.Value.Value;
                mySqlCommandSel.Parameters["@VlrSubsistencia"].Value = planilla.VlrSubsistencia.Value.Value;
                mySqlCommandSel.Parameters["@VlrNoRetenido"].Value = planilla.VlrNoRetenido.Value.Value;
                mySqlCommandSel.Parameters["@FondoSalud"].Value = planilla.FondoSalud.Value;
                mySqlCommandSel.Parameters["@FondoSaludTR"].Value = !string.IsNullOrEmpty(planilla.FondoSaludTR.Value) ? planilla.FondoSaludTR.Value : planilla.FondoSalud.Value;
                mySqlCommandSel.Parameters["@DiasCotizadosSLD"].Value = planilla.DiasCotizadosSLD.Value.Value;
                mySqlCommandSel.Parameters["@IngresoBaseSLD"].Value = planilla.IngresoBaseSLD.Value.Value;
                mySqlCommandSel.Parameters["@TarifaSLD"].Value = planilla.TarifaSLD.Value.Value;
                mySqlCommandSel.Parameters["@VlrTrabajadorSLD"].Value = planilla.VlrTrabajadorSLD.Value.Value;
                mySqlCommandSel.Parameters["@VlrEmpresaSLD"].Value = planilla.VlrEmpresaSLD.Value.Value;
                mySqlCommandSel.Parameters["@VlrUPC"].Value = planilla.VlrUPC.Value.Value;
                mySqlCommandSel.Parameters["@AutorizacionEnf"].Value = planilla.AutorizacionEnf.Value;
                mySqlCommandSel.Parameters["@VlrIncapacidad"].Value = planilla.VlrIncapacidad.Value.Value;
                mySqlCommandSel.Parameters["@AutorizacioMat"].Value = planilla.AutorizacioMat.Value;
                mySqlCommandSel.Parameters["@VlrMaternidad"].Value = planilla.VlrMaternidad.Value.Value;
                mySqlCommandSel.Parameters["@DiasCotizadosARP"].Value = planilla.DiasCotizadosARP.Value.Value;
                mySqlCommandSel.Parameters["@IngresoBaseARP"].Value = planilla.IngresoBaseARP.Value.Value;
                mySqlCommandSel.Parameters["@TarifaARP"].Value = planilla.TarifaARP.Value.Value;
                mySqlCommandSel.Parameters["@CtoARP"].Value = planilla.CtoARP.Value;
                mySqlCommandSel.Parameters["@VlrARP"].Value = planilla.VlrARP.Value.Value;
                mySqlCommandSel.Parameters["@CajaNOID"].Value = planilla.CajaNOID.Value;
                mySqlCommandSel.Parameters["@DiasCotizadosCCF"].Value = planilla.DiasCotizadosCCF.Value.Value;
                mySqlCommandSel.Parameters["@IngresoBaseCCF"].Value = planilla.IngresoBaseCCF.Value.Value;
                mySqlCommandSel.Parameters["@TarifaCCF"].Value = planilla.TarifaCCF.Value.Value;
                mySqlCommandSel.Parameters["@VlrCCF"].Value = planilla.VlrCCF.Value.Value;
                mySqlCommandSel.Parameters["@TarifaICBF"].Value = planilla.TarifaIBF.Value.Value;
                mySqlCommandSel.Parameters["@VlrSEN"].Value = planilla.VlrSEN.Value.Value;
                mySqlCommandSel.Parameters["@IndExoneradoCCF"].Value = planilla.IndExoneradoCCF.Value;
                mySqlCommandSel.Parameters["@VlrIBF"].Value = planilla.VlrICBF.Value.Value;
                mySqlCommandSel.Parameters["@TarifaSEN"].Value = planilla.TarifaSEN.Value.Value;
                mySqlCommandSel.Parameters["@TarifaIBF"].Value = planilla.TarifaIBF.Value.Value;

                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noFondo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noFondo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConvencion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConvencion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noCaja"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noCaja, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRiesgo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRiesgo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noBrigada"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noBrigada, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noTurnoCompensatorio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noTurnoCompensatorio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noRol"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noRol, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_rhCargos"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.rhCargos, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_tsBanco"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBanco, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);

                mySqlCommandSel.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaAportesDeta_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Proceso trae valores planilla X tercero
        /// </summary>
        public List<DTO_NominaPlanillaContabilizacion> DAL_noPlanillaAportesDeta_GetValoreXTercero(bool isPlanilla)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_ValoresContabilizacionPlanilla", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Planilla", SqlDbType.Bit);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Planilla"].Value = isPlanilla;


                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                List<DTO_NominaPlanillaContabilizacion> listaDocs = new List<DTO_NominaPlanillaContabilizacion>();
                DTO_NominaPlanillaContabilizacion doc = null;
                while (dr.Read())
                {
                    doc = new DTO_NominaPlanillaContabilizacion(dr);
                    doc.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                    doc.Total.Value = doc.Valor.Value + doc.Valor2.Value;
                    listaDocs.Add(doc);
                }
                dr.Close();
                return listaDocs;

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaAportesDeta_GetValoreXTercero");
                throw exception;
            }
        }

    }
}
