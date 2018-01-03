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
    public class DAL_ccLineaComponentePlazo : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccLineaComponentePlazo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        public DTO_ccLineaComponentePlazo DAL_ccLineaComponentePlazo_GetByMonto(string lineaCreID, string componenteID, int monto, int plazo)
        {
            try
            {
                DTO_ccLineaComponentePlazo result = null;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommand.Parameters.Add("@Monto", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Plazo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccLineaCredito", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@LineaCreditoID"].Value = lineaCreID;
                mySqlCommand.Parameters["@ComponenteCarteraID"].Value =componenteID;
                mySqlCommand.Parameters["@Monto"].Value = monto;
                mySqlCommand.Parameters["@Plazo"].Value = plazo;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glEmpresaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccLineaCredito"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaCredito, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                mySqlCommand.CommandText =
                    "select top(1) * from ccLineaComponentePlazo with(nolock) " +
                    "where LineaCreditoID = @LineaCreditoID and ComponenteCarteraID = @ComponenteCarteraID and Monto >= @Monto and Plazo = @Plazo " +
                    "   and EmpresaGrupoID = @EmpresaGrupoID and eg_ccLineaCredito = @eg_ccLineaCredito and eg_ccCarteraComponente = @eg_ccCarteraComponente " +
                    "   and ActivoInd = @ActivoInd " +
                    "order by Monto";

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                if(dr.Read())
                {
                    result = new DTO_ccLineaComponentePlazo();

                    result.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                    result.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                    result.Plazo.Value = Convert.ToInt32(dr["Plazo"]);
                    result.Monto.Value = Convert.ToInt32(dr["Monto"]);
                    if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                        result.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                    if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                        result.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccLineaComponentePlazo_GetByMonto");
                throw exception;
            }
        }
    }
}
