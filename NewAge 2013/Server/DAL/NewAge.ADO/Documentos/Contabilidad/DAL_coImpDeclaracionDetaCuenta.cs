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
    /// <summary>
    /// Operaciones de BD sobre la tabla DAL_coImpDeclaracionDetaCuenta
    /// </summary>
    public class DAL_coImpDeclaracionDetaCuenta : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coImpDeclaracionDetaCuenta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="docu"></param>
        public void DAL_coImpDeclaracionDetaCuenta_Add(DTO_coImpDeclaracionDetaCuenta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Renglon", SqlDbType.Char, UDT_Renglon.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrBaseML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value.Value;
                mySqlCommandSel.Parameters["@Renglon"].Value = deta.Renglon.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = deta.CuentaID.Value;
                mySqlCommandSel.Parameters["@VlrBaseML"].Value = deta.VlrBaseML.Value.Value;
                mySqlCommandSel.Parameters["@ValorML"].Value = deta.ValorML.Value.Value;
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "INSERT INTO coImpDeclaracionDetaCuenta(" +
                    "	EmpresaID,NumeroDoc,Renglon,CuentaID,VlrBaseML,ValorML,eg_coPlanCuenta" +
                    ")VALUES(" +
                    "	@EmpresaID,@NumeroDoc,@Renglon,@CuentaID,@VlrBaseML,@ValorML,@eg_coPlanCuenta" +
                    ")";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDetaCuenta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los registros de un documento
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_coImpDeclaracionDetaCuenta_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText =
                    "DELETE from coImpDeclaracionDetaCuenta where EmpresaID=@EmpresaID and NumeroDoc=@NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDetaCuenta_Delete");
                throw exception;
            }
        }

    }
}
