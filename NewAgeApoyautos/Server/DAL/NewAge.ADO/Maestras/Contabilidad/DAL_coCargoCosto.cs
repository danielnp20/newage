using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_coCargoCosto : DAL_Base
    {      

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coCargoCosto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="ConceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o null si no existe</returns>
        public string DAL_coCargoCosto_GetCuentaIDByCargoOper(string ConceptoCargoID, string operID, string lineaPresID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                #region Query

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@OperacionID", SqlDbType.Char, UDT_OperacionID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_OperacionID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = ConceptoCargoID;
                mySqlCommandSel.Parameters["@OperacionID"].Value = operID;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = lineaPresID;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCargoCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "select CuentaID " +
                    "from coCargoCosto with(nolock) " +
                    "where " +
                    "   ConceptoCargoID = @ConceptoCargoID " + 
                    "   and OperacionID = @OperacionID " + 
                    "   and LineaPresupuestoID = @LineaPresupuestoID " +
                    "   and ActivoInd = 1 " +
                    "   and EmpresaGrupoID = @EmpresaGrupoID " +
                    "   and eg_coOperacion = @eg_coOperacion " +
                    "   and eg_coConceptoCargo = @eg_coConceptoCargo " +
                    "   and eg_plLineaPresupuesto = @eg_plLineaPresupuesto " +
                    "   and eg_coPlanCuenta = @eg_coPlanCuenta ";

                #endregion

                object obj = mySqlCommandSel.ExecuteScalar();
                return obj != null ? obj.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCargoCosto_GetCuentaIDByCargoProy");
                throw exception;
            }
        }
            
        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o string.Empty si no existe</returns>
        public DTO_CuentaValor DAL_coCargoCosto_GetCuentaByCargoOper(string conceptoCargoID, string operacionID, string lineaPresID, decimal valor)
        {
            try
            {
                DTO_CuentaValor result = null;            

                #region Query
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@OperacionID", SqlDbType.Char, UDT_OperacionID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_OperacionID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = conceptoCargoID;
                mySqlCommandSel.Parameters["@OperacionID"].Value = operacionID;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = lineaPresID;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCargoCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coOperacion, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "select CuentaID " +
                    "from coCargoCosto with(nolock) " +
                    "where " +
                    "   ConceptoCargoID = @ConceptoCargoID " +
                    "   and OperacionID = @OperacionID " +
                    "   and LineaPresupuestoID = @LineaPresupuestoID " +
                    "   and ActivoInd = 1 " +
                    "   and EmpresaGrupoID = @EmpresaGrupoID " +
                    "   and eg_coOperacion = @eg_coOperacion " +
                    "   and eg_coConceptoCargo = @eg_coConceptoCargo " +
                    "   and eg_plLineaPresupuesto = @eg_plLineaPresupuesto " +
                    "   and eg_coPlanCuenta = @eg_coPlanCuenta ";
                #endregion

                object objc = mySqlCommandSel.ExecuteScalar();
                result = objc != null ? new DTO_CuentaValor(objc.ToString(), valor, 0, string.Empty, string.Empty) : null;
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCargoCosto_GetCuentaByCargoOper");
                throw exception;
            }
        }

        #endregion
  
    }
}
