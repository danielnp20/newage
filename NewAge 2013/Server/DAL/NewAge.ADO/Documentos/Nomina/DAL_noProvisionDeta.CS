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
using System.ComponentModel;
using System.Reflection;
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    public class DAL_noProvisionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noProvisionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

       
        /// <summary>
        /// Procedimiento para Liquidar las Provisiones para el Periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">numero de documento</param>
        /// <param name="tc">tasa de cambio</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">Descripción de error</param>
        public void DAL_noProvisionDeta_LiquidarProvisiones(string empleadoID, int numeroDoc, decimal tc, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_LiquidarProvisiones", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                               
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);     
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                             
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

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tc;
       
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

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noProvisionDeta_LiquidarProvisiones");
                throw exception;
            }
        }


        /// <summary>
        /// Obtiene un listado de la liquidación de Provisiones del empleado en el periodo
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="contratoNOID">número contrato empleado</param>
        /// <returns>lista de provisiones</returns>
        public List<DTO_noProvisionDeta> DAL_noProvisionDeta_Get(DateTime periodo) 
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " SELECT noProvisionDeta.*, noConceptoNOM.Descriptivo " +
                                              "   FROM glDocumentoControl " +
                                              "   INNER JOIN noLiquidacionesDocu ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                                              "   INNER JOIN noProvisionDeta ON noProvisionDeta.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                                              "   INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noProvisionDeta.ConceptoNOID  " +
                                              "   AND noConceptoNOM.EmpresaGrupoID = @EmpresaID " +
                                              "   WHERE glDocumentoControl.EmpresaID = @EmpresaID " +
                                              "   AND glDocumentoControl.DocumentoID = @DocumentoID " +
                                              "   AND glDocumentoControl.PeriodoDoc = @Periodo ";                                     

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Provisiones;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
   
                List<DTO_noProvisionDeta> result = new List<DTO_noProvisionDeta>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noProvisionDeta dto = new DTO_noProvisionDeta(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noProvisionDeta_Get");
                throw exception;
            }           
        }


        /// <summary>
        /// Obtiene un listado de la liquidación de Provisiones del empleado en el periodo
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="contratoNOID">número contrato empleado</param>
        /// <returns>lista de provisiones</returns>
        public List<DTO_noProvisionDeta> DAL_noProvisionDeta_Get(DateTime periodo, int contratoNOID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " SELECT noProvisionDeta.*, noConceptoNOM.Descriptivo " +
                                              "   FROM glDocumentoControl " +
                                              "   INNER JOIN noLiquidacionesDocu ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                                              "   INNER JOIN noProvisionDeta ON noProvisionDeta.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                                              "   INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noProvisionDeta.ConceptoNOID  " +
                                              "   AND noConceptoNOM.EmpresaGrupoID =  @EmpresaID" +
                                              "   WHERE glDocumentoControl.EmpresaID = @EmpresaID " +
                                              "   AND glDocumentoControl.DocumentoID = @DocumentoID " +
                                              "   AND glDocumentoControl.PeriodoDoc = @Periodo " +
                                              "   AND noLiquidacionesDocu.ContratoNOID = @ContratoNOID  ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Provisiones;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = contratoNOID;

                List<DTO_noProvisionDeta> result = new List<DTO_noProvisionDeta>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noProvisionDeta dto = new DTO_noProvisionDeta(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noProvisionDeta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Procedimiento almacenado para migración de Provisiones
        /// </summary>
        /// <param name="reg">registro de provision</param>
        /// <param name="numeroDoc">numero doc</param>
        /// <param name="tasaCambio">tasa de cambio</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripción de error</param>
        public void DAL_noProvisionDeta_MigracionProvisiones(DTO_noProvisionDeta reg, int numeroDoc, decimal tasaCambio, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_MigracionProvisiones", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaCambio", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);                

                mySqlCommandSel.Parameters.Add("@DiasTrabajados", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DiasProvision", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Sueldo", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AuxTransporte", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@BaseVariable", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@BaseNeta", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorMes", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorPagado", SqlDbType.Decimal);
             

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

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = reg.EmpleadoID.Value; ;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TasaCambio"].Value = tasaCambio;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = reg.Periodo.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = reg.ConceptoNOID.Value;                

                mySqlCommandSel.Parameters["@DiasTrabajados"].Value = reg.DiasTrabajados.Value;
                mySqlCommandSel.Parameters["@DiasProvision"].Value = reg.DiasProvision.Value;
                mySqlCommandSel.Parameters["@Sueldo"].Value = reg.Sueldo.Value;
                mySqlCommandSel.Parameters["@AuxTransporte"].Value = reg.AuxilioTransporte.Value;
                mySqlCommandSel.Parameters["@BaseVariable"].Value = reg.BaseVariable.Value;
                mySqlCommandSel.Parameters["@BaseNeta"].Value = reg.BaseNeta.Value;
                mySqlCommandSel.Parameters["@ValorMes"].Value = reg.VlrProvisionMES.Value;
                mySqlCommandSel.Parameters["@ValorPagado"].Value = reg.VlrPagosMES.Value;
           
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


                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();

                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noProvisionDeta_MigracionProvisiones");
                throw exception;
            }
        }

      
    }
}
