using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_plCierres
    /// </summary>
    public class DAL_plCierres : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plCierres(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns></returns>
        public DTO_TxResult DAL_plCierreDia_Procesar(DateTime periodo, DateTime periodo_co, DateTime fechaCierre)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_ccCierreMesCartera> cierres = new List<DTO_ccCierreMesCartera>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@PeriodoID_co", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@PeriodoID_co"].Value = periodo_co;
                mySqlCommand.Parameters["@Fecha"].Value = fechaCierre;
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Planeacion_CierreDiario";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                if (dr.GetName(0) == "Linea")
                {
                    result.Result = ResultValue.NOK;

                    rd = new DTO_TxResultDetail();
                    rd.line = Convert.ToInt32(dr["Linea"]);
                    rd.Message = dr["Valor"].ToString();

                    result.Details.Add(rd);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_plCierreDia_Procesar");
                return result;
            }
        }
    }
}
