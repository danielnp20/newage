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
    public class DAL_coPlanCuenta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coPlanCuenta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Actualiza el concepto de saldo de una cuenta
        /// </summary>
        /// <param name="replicaID">PK del plan de cuentas</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo</param>
        public void DAL_coPlanCuenta_UpdateConceptoSaldo(int replicaID, string concSaldoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@ConceptoSaldoID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ReplicaID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@ConceptoSaldoID"].Value = concSaldoID;
                mySqlCommandSel.Parameters["@ReplicaID"].Value = replicaID;

                mySqlCommandSel.CommandText = "Update coPlanCuenta set ConceptoSaldoID = @ConceptoSaldoID where ReplicaID = @ReplicaID";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coPlanCuenta_UpdateConceptoSaldo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae una lista de tarifas para una cuenta
        /// </summary>
        /// <param name="impTipoID">Tipo de impuesto</param>
        public List<decimal> DAL_coPlanCuenta_TarifasImpuestos(string impTipoID)
        {
            try
            {
                List<decimal> result = new List<decimal>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@ImpuestoTipoID", SqlDbType.Char, UDT_ImpuestoTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coImpuestoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@ImpuestoTipoID"].Value = impTipoID;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coImpuestoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coImpuestoTipo, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText = 
                    "select distinct ImpuestoPorc from coPlanCuenta with(nolock) "  +
                    "where EmpresaGrupoID = @EmpresaGrupoID and ImpuestoPorc is not null " +
                    "	and ImpuestoTipoID = @ImpuestoTipoID and eg_coImpuestoTipo = @eg_coImpuestoTipo";

                SqlDataReader dr;

                dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    decimal porc = Convert.ToDecimal(dr["ImpuestoPorc"]);
                    result.Add(porc);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coPlanCuenta_TarifasImpuestos");
                throw exception;
            }
        }

        #endregion
    }
}
