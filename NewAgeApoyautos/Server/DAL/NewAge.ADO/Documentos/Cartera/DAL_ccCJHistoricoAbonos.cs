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
    public class DAL_ccCJHistoricoAbonos : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCJHistoricoAbonos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_ccCJHistoricoAbonos
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCJHistoricoAbonos</returns>
        public List<DTO_ccCJHistoricoAbonos> DAL_ccCJHistoricoAbonos_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccCJHistoricoAbonos> result = new List<DTO_ccCJHistoricoAbonos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                #region Query Cooperativa
                mySqlCommand.CommandText =
                               "SELECT abono.*,cartera.Descriptivo " +
                               "FROM ccCJHistoricoAbonos abono WITH(NOLOCK) " +
                               "    INNER JOIN ccCarteraComponente cartera WITH(NOLOCK) on cartera.ComponenteCarteraID = abono.ComponenteCarteraID  " +
                               "        AND cartera.EmpresaGrupoID = abono.eg_ccCarteraComponente  " +
                               "WHERE abono.NumeroDoc = @numeroDoc " +
                               "ORDER  BY abono.ComponenteCarteraID";
                #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccCJHistoricoAbonos anexo = new DTO_ccCJHistoricoAbonos(dr);
                    result.Add(anexo);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CJHistoricoAbonos_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccabonosComponetes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccCJHistoricoAbonos_Add(DTO_ccCJHistoricoAbonos abonos)
        {
            try
            {
                List<DTO_ccCJHistoricoAbonos> result = new List<DTO_ccCJHistoricoAbonos>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "INSERT INTO ccCJHistoricoAbonos   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[ClaseDeuda]   " +
                                                  "  ,[ComponenteCarteraID]   " +
                                                  "  ,[Valor]   " +
                                                  "  ,[TipoAbono]   " +
                                                  "  ,[DocProceso]   " +
                                                  "  ,[eg_ccCarteraComponente])   " +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@ClaseDeuda    " +
                                                  "  ,@ComponenteCarteraID    " +
                                                  "  ,@Valor    " +
                                                  "  ,@TipoAbono    " +
                                                  "  ,@DocProceso    " +
                                                  "  ,@eg_ccCarteraComponente)";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClaseDeuda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoAbono", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocProceso", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = abonos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ClaseDeuda"].Value = abonos.ClaseDeuda.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = abonos.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = abonos.Valor.Value;
                mySqlCommandSel.Parameters["@TipoAbono"].Value = abonos.TipoAbono.Value;
                mySqlCommandSel.Parameters["@DocProceso"].Value = abonos.DocProceso.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistoricoAbonos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccabonosComponetes
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccCJHistoricoAbonos_Update(DTO_ccCJHistoricoAbonos abonos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                                "UPDATE ccCJHistoricoAbonos set " +
                                    "   ComponenteCarteraID = @ComponenteCarteraID  " +
                                    "  ,ClaseDeuda = @ClaseDeuda " +
                                    "  ,Valor = @Valor " +
                                    "  ,TipoAbono = @TipoAbono " +
                                    "  ,DocProceso = @DocProceso " +
                                    " WHERE  Consecutivo = @Consecutivo";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ClaseDeuda", SqlDbType.TinyInt);              
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TipoAbono", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocProceso", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion Campos
              
                mySqlCommandSel.Parameters["@ClaseDeuda"].Value = abonos.ClaseDeuda.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = abonos.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = abonos.Valor.Value;
                mySqlCommandSel.Parameters["@TipoAbono"].Value = abonos.TipoAbono.Value;
                mySqlCommandSel.Parameters["@DocProceso"].Value = abonos.DocProceso.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = abonos.Consecutivo.Value;               
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistoricoAbonos_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccCJHistoricoAbonos_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccCJHistoricoAbonos WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistoricoAbonos_Delete");
                throw exception;
            }
        }

    }
}
