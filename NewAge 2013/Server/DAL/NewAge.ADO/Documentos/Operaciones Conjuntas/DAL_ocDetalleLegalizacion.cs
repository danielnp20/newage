using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_ocDetalleLegalizacion : DAL_Base
    {
        private int maxBufferSize = 1024 * 1024 * 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ocDetalleLegalizacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Elimina los temporales de un usuario
        /// </summary>
        /// <param name="origen">Origen de los datos</param>
        /// <param name="usuario">Usuario que esta buscando temporales</param>
        public void DAL_ocDetalleLegalizacion_Delete(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                    "DELETE FROM ocDetalleLegalizacion WHERE EmpresaID=@EmpresaID AND PeriodoID=@PeriodoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ocDetalleLegalizacion_Delete");
                throw exception;
            }
        }


        /// <summary>
        /// Distribuye entre los socios de acuerdo a los porcentajes dados
        /// </summary>
        /// <param name="periodo">PeriodoID</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion de error</param>
        public void DAL_ocDetalleLegalizacion_Distribucion(DateTime periodo, out int errorInd, out string errorDesc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("OC_DistribucionPorcentual", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;

                SqlParameter ErrorInd = new SqlParameter("@ErrorInd", SqlDbType.Int); ErrorInd.Direction = ParameterDirection.Output;
                SqlParameter ErrorDesc = new SqlParameter("@ErrorDesc", SqlDbType.VarChar, 200); ErrorDesc.Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters.Add(ErrorInd);
                mySqlCommandSel.Parameters.Add(ErrorDesc);

                mySqlCommandSel.ExecuteNonQuery();
                errorInd = (int)mySqlCommandSel.Parameters["@ErrorInd"].Value;
                errorDesc = (string)mySqlCommandSel.Parameters["@ErrorDesc"].Value;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ocDetalleLegalizacion_Distribucion");
                throw exception;
            }
        
        }

        /// <summary>
        /// Trae un listado asociado a un DetalleLegalizacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_QueryInformeMensualCierre> DAL_ocDetalleLegalizacion_GetInfoMensual(DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoModa, TipoMoneda mdaOrigen)
        {

            try
            {
                List<DTO_QueryInformeMensualCierre> result = new List<DTO_QueryInformeMensualCierre>();

                SqlCommand mySqlCommandSel = new SqlCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText

                mySqlCommandSel = new SqlCommand("OperacionesConj_ocDetalleLegalizacion_GetInfoMes", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoInforme", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoMda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ProyectoTipo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActividadID", SqlDbType.VarChar, UDT_ActividadID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPres", SqlDbType.VarChar, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCost", SqlDbType.VarChar, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecurGrupo", SqlDbType.VarChar, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.VarChar, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoID", SqlDbType.VarChar, UDT_ContratoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Pozo", SqlDbType.VarChar, UDT_LocFisicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Campo", SqlDbType.VarChar, UDT_AreaFisicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@SocioID", SqlDbType.VarChar, UDT_SocioID.MaxLength);

                #endregion
                #region Asignacion Valores Parametros

                mySqlCommandSel.Parameters["@Year"].Value = filter.PeriodoID.Value.Value.Year;
                mySqlCommandSel.Parameters["@Month"].Value = filter.PeriodoID.Value.Value.Month;
                mySqlCommandSel.Parameters["@TipoInforme"].Value = tipoInforme;
                mySqlCommandSel.Parameters["@TipoMda"].Value = (byte)tipoModa;
                mySqlCommandSel.Parameters["@MdaOrigen"].Value = (byte)mdaOrigen;
                mySqlCommandSel.Parameters["@ProyectoTipo"].Value = (byte)proType;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                mySqlCommandSel.Parameters["@ActividadID"].Value = filter.ActividadID.Value;
                mySqlCommandSel.Parameters["@LineaPres"].Value = filter.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CentroCost"].Value = filter.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@RecurGrupo"].Value = filter.Grupo.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = filter.RecursoID.Value;
                mySqlCommandSel.Parameters["@ContratoID"].Value = filter.ContratoID.Value;
                mySqlCommandSel.Parameters["@Pozo"].Value = filter.Pozo.Value;
                mySqlCommandSel.Parameters["@Campo"].Value = filter.Campo.Value;
                mySqlCommandSel.Parameters["@SocioID"].Value = filter.SocioID.Value;

                #endregion

                DTO_QueryInformeMensualCierre presupuesto = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    presupuesto = new DTO_QueryInformeMensualCierre(dr,true);
                    result.Add(presupuesto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ocDetalleLegalizacion_GetParameterQuery");
                throw exception;
            }
        }


    }
}
