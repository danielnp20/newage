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
    public class DAL_ccEstadoCuentaComponentes : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccEstadoCuentaComponentes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_ccEstadoCuentaComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccEstadoCuentaComponentes</returns>
        public List<DTO_ccEstadoCuentaComponentes> DAL_ccEstadoCuentaComponentes_GetByNumeroDoc(int NumeroDoc, string componenteMora, string componentePJ, string componenteUsura)
        {
            try
            {
                List<DTO_ccEstadoCuentaComponentes> result = new List<DTO_ccEstadoCuentaComponentes>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                #region Query Cooperativa
                mySqlCommand.CommandText =
                               "SELECT est.*,cartera.Descriptivo,cartera.TipoComponente " +
                               "FROM ccEstadoCuentaComponentes est WITH(NOLOCK) " +
                               "    INNER JOIN ccCarteraComponente cartera WITH(NOLOCK) on cartera.ComponenteCarteraID = est.ComponenteCarteraID  " +
                               "        AND cartera.EmpresaGrupoID = est.eg_ccCarteraComponente  "+
                               "WHERE est.NumeroDoc = @numeroDoc " +
                               "ORDER  BY est.ComponenteCarteraID";
                #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccEstadoCuentaComponentes comp = new DTO_ccEstadoCuentaComponentes(dr);
                    if (comp.SaldoValor.Value == 0)
                    {
                        comp.Editable.Value = true;
                    }

                    result.Add(comp);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaComponentes_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae el estado de cuenta con base al numero de la libranza
        /// </summary>
        /// <param name="libranzaID">Libranza Id</param>
        /// <returns></returns>
        public List<DTO_ccEstadoCuentaComponentes> DAL_ccEstadoCuentaComponentes_GetByLibranza(string libranzaID, string componenteMora, string componentePJ, string componenteUsura)
        {
            try
            {
                List<DTO_ccEstadoCuentaComponentes> result = new List<DTO_ccEstadoCuentaComponentes>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Char, 20);
                mySqlCommand.Parameters["@Libranza"].Value = libranzaID;

                #region Query Cooperativa
                mySqlCommand.CommandText =
                    "SELECT est.*, cartera.Descriptivo,cartera.TipoComponente " +
                    "FROM ccEstadoCuentaComponentes est WITH(NOLOCK) " +
                    "	INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) on est.NumeroDoc = ctrl.NumeroDoc AND ctrl.DocumentoTercero = @Libranza " +
                    "	INNER JOIN ccCarteraComponente cartera WITH(NOLOCK) on est.ComponenteCarteraID = cartera.ComponenteCarteraID and est.eg_ccCarteraComponente = cartera.EmpresaGrupoID " +
                    "WHERE est.NumeroDoc in " +
                    "( " +
                    "	SELECT TOP(1) est.NumeroDoc  " +
                    "	FROM ccEstadoCuentaComponentes est WITH(NOLOCK) " +
                    "		INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) on est.NumeroDoc = ctrl.NumeroDoc AND ctrl.DocumentoTercero = @Libranza " +
                    "	ORDER BY est.NumeroDoc DESC " +
                    ") " +
                    "ORDER BY est.ComponenteCarteraID";
                #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccEstadoCuentaComponentes comp = new DTO_ccEstadoCuentaComponentes(dr);
                    if (comp.ComponenteCarteraID.Value != componenteMora &&
                        comp.ComponenteCarteraID.Value != componentePJ &&
                        comp.ComponenteCarteraID.Value != componenteUsura &&
                        Convert.ToByte(dr["TipoComponente"]) != (byte)TipoComponente.CapitalSolicitado &&
                        Convert.ToByte(dr["TipoComponente"]) != (byte)TipoComponente.ComponenteCuota)
                    {
                        comp.Editable.Value = true;
                    }

                    result.Add(comp);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaComponentes_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccEstadoCuentaComponetes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccEstadoCuentaComponentes_Add(DTO_ccEstadoCuentaComponentes estadoCuenta)
        {
            try
            {
                List<DTO_ccEstadoCuentaComponentes> result = new List<DTO_ccEstadoCuentaComponentes>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "INSERT INTO ccEstadoCuentaComponentes   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[ComponenteCarteraID]   " +
                                                  "  ,[SaldoValor]   " +
                                                  "  ,[PagoValor]   " +
                                                  "  ,[AbonoValor]   " +
                                                  "  ,[eg_ccCarteraComponente])   " +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@ComponenteCarteraID    " +
                                                  "  ,@SaldoValor    " +
                                                  "  ,@PagoValor    " +
                                                  "  ,@AbonoValor    " +
                                                  "  ,@eg_ccCarteraComponente)";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@SaldoValor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PagoValor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AbonoValor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = estadoCuenta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = estadoCuenta.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@SaldoValor"].Value = estadoCuenta.SaldoValor.Value;
                mySqlCommandSel.Parameters["@PagoValor"].Value = estadoCuenta.PagoValor.Value;
                mySqlCommandSel.Parameters["@AbonoValor"].Value = estadoCuenta.AbonoValor.Value;
                mySqlCommandSel.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommandSel.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaComponentes_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccEstadoCuentaComponetes
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccEstadoCuentaComponentes_Update(DTO_ccEstadoCuentaComponentes estadoCuenta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                                "UPDATE ccEstadoCuentaComponentes" +
                                    "  ComponenteCarteraID = @ComponenteCarteraID  " +
                                    "  ,SaldoValor = @SaldoValor " +
                                    "  ,PagoValor = @PagoValor " +
                                    "  ,AbonoValor = @AbonoValor " +
                                    " WHERE  Consecutivo = @Consecutivo";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@SaldoValor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PagoValor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AbonoValor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = estadoCuenta.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@SaldoValor"].Value = estadoCuenta.SaldoValor.Value;
                mySqlCommandSel.Parameters["@PagoValor"].Value = estadoCuenta.PagoValor.Value;
                mySqlCommandSel.Parameters["@AbonoValor"].Value = estadoCuenta.AbonoValor.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = estadoCuenta.Consecutivo.Value;
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaComponentes_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccEstadoCuentaComponentes_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccEstadoCuentaComponentes WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaComponentes_Delete");
                throw exception;
            }
        }

    }
}
