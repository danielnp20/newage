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
    public class DAL_ccSolicitudComponentes : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudComponentes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD
        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public List<DTO_ccSolicitudComponentes> DAL_ccSolicitudComponentes_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudComponentes> result = new List<DTO_ccSolicitudComponentes>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT sc.*,cc.Descriptivo " +
                                       "FROM ccSolicitudComponentes sc with(nolock)" +
                                            "inner join ccCarteraComponente cc  with(nolock) on sc.ComponenteCarteraID = cc.ComponenteCarteraID " +
                                            "   and sc.eg_ccCarteraComponente = cc.EmpresaGrupoID " +
                                       "WHERE sc.NumeroDoc = @numeroDoc " +
                                       "ORDER BY sc.ComponenteCarteraID";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;
                while (dr.Read())
                {
                    DTO_ccSolicitudComponentes anexo = new DTO_ccSolicitudComponentes(dr);
                    result.Add(anexo);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudComponentes_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudComponentes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudComponentes_Add(DTO_ccSolicitudComponentes componentes)
        {
            try
            {
                List<DTO_ccSolicitudComponentes> result = new List<DTO_ccSolicitudComponentes>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "INSERT INTO ccSolicitudComponentes   " +
                                                         "  ([NumeroDoc]   " +
                                                         "  ,[ComponenteCarteraID]   " +
                                                         "  ,[CuotaValor]   " +
                                                         "  ,[TotalValor]   " +
                                                         "  ,[CompInvisibleInd]   " +
                                                         "  ,[PorCapital]   " +
                                                         "  ,[eg_ccCarteraComponente])   " +
                                                         "VALUES    " +
                                                         "  (@NumeroDoc    " +
                                                         "  ,@ComponenteCarteraID    " +
                                                         "  ,@CuotaValor    " +
                                                         "  ,@TotalValor    " +
                                                         "  ,@CompInvisibleInd    " +
                                                         "  ,@PorCapital    " +
                                                         "  ,@eg_ccCarteraComponente)";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuotaValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TotalValor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompInvisibleInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PorCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asiganacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = componentes.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = componentes.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@CuotaValor"].Value = componentes.CuotaValor.Value;
                mySqlCommandSel.Parameters["@TotalValor"].Value = componentes.TotalValor.Value;
                mySqlCommandSel.Parameters["@CompInvisibleInd"].Value = componentes.CompInvisibleInd.Value.Value;
                mySqlCommandSel.Parameters["@PorCapital"].Value = componentes.PorCapital.Value.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudComponentes_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudComponentes_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudComponentes WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudComponentes_Delete");
                throw exception;
            }
        }

        #endregion

    }
}
