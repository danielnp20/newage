using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Diagnostics;

namespace NewAge.ADO
{
    public class DAL_ccCarteraComponente : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCarteraComponente(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae los documentos anexos dependiendo de la pagaduria
        /// </summary>
        /// <param name="pagaduriaID">Id de la pagaduria</param>
        /// <returns></returns>
        public List<DTO_ccCarteraComponente> DAL_ccCarteraComponente_GetByLineaCredito(string lineaCreditoID)
        {
            List<DTO_ccCarteraComponente> result = new List<DTO_ccCarteraComponente>();
            try
            {
                #region Query
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@LineaCreditoID"].Value = lineaCreditoID;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaComponente, this.Empresa, egCtrl);

                //Query basico
                mySqlCommand.CommandText =
                        "SELECT carComp.* " +
                        "FROM ccLineaComponente lComp with(nolock) " +
                        "   INNER JOIN ccCarteraComponente carComp with(nolock) ON lComp.ComponenteCarteraID = carComp.ComponenteCarteraID " + 
                        "       and lComp.eg_ccCarteraComponente = carComp.EmpresaGrupoID " +
                        "WHERE	lComp.LineaCreditoID = @LineaCreditoID AND lComp.EmpresaGrupoID = @EmpresaGrupoID " +
                        "ORDER BY carComp.ComponenteCarteraID";

                #endregion

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCarteraComponente carteraComp = new DTO_ccCarteraComponente();
                    carteraComp.ID.Value = dr["ComponenteCarteraID"].ToString();
                    carteraComp.Descriptivo.Value = dr["Descriptivo"].ToString();
                    carteraComp.TipoComponente.Value = Convert.ToByte(dr["TipoComponente"]);
                    carteraComp.NumeroComp.Value = Convert.ToByte(dr["NumeroComp"]);
                    if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                        carteraComp.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                    if (!string.IsNullOrEmpty(dr["TipoCreditoInd"].ToString()))
                        carteraComp.TipoCreditoInd.Value = Convert.ToBoolean(dr["TipoCreditoInd"]);
                    result.Add(carteraComp);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCarteraComponente_GetByLineaCredito");
                throw exception;
            }

        }

        #endregion
    }
}
