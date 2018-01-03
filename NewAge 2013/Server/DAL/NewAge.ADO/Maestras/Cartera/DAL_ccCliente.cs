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

namespace NewAge.ADO
{
    public class DAL_ccCliente : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCliente(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene un cliente seg;un el codigo
        /// </summary>
        /// <param name="codigo">Codigo del empleado</param>
        /// <returns>Retorna el cliente</returns>
        public DTO_ccCliente DAL_ccCliente_GetClienteByCodigoEmpleado(string codigo)
        {
            try
            {
                DTO_ccCliente cliente = null;

                #region Query
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpleadoCodigo", SqlDbType.Char, 10);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpleadoCodigo"].Value = codigo;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);

                //Query basico
                mySqlCommand.CommandText =
                        "SELECT * from ccCliente with(nolock) where EmpleadoCodigo = @EmpleadoCodigo and EmpresaGrupoID = EmpresaGrupoID";

                #endregion

                DTO_aplMaestraPropiedades props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.ccCliente, this.loggerConnectionStr);
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    cliente = new DTO_ccCliente(dr, props, true);

                dr.Close();

                return cliente;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCliente_GetClienteByCodigoEmpleado");
                throw exception;
            }
        }
    }
}
