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
    public class DAL_ccSentenciaPlanPagos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSentenciaPlanPagos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summMOary>
        /// Trae todos los registros de DTO_PlanDePagos
        /// </summary>
        /// <returns>retorna una lista de DTO_PlanDePagos</returns>
        public List<DTO_ccSentenciaPlanPagos> DAL_ccSentenciaPlanPagos_GetByNumDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccSentenciaPlanPagos PA with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc " +
                                           "ORDER BY CuotaID ";

                List<DTO_ccSentenciaPlanPagos> result = new List<DTO_ccSentenciaPlanPagos>();

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSentenciaPlanPagos dto = new DTO_ccSentenciaPlanPagos(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSentenciaPlanPagos_GetByNumDoc");
                throw exception;
            }
        }

        /// <summMOary>
        /// Trae la info de una couta
        /// </summary>
        /// <param name="creditoCuotaNum">Identificador único de la cuota</param>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public DTO_ccSentenciaPlanPagos DAL_ccSentenciaPlanPagos_GetByID(int creditoCuotaNum)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = creditoCuotaNum;
                mySqlCommand.CommandText = "SELECT * FROM ccSentenciaPlanPagos with(nolock) WHERE Consecutivo = @Consecutivo ";

                DTO_ccSentenciaPlanPagos result = new DTO_ccSentenciaPlanPagos();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccSentenciaPlanPagos(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSentenciaPlanPagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="pagos"></param>
        /// <returns></returns>
        public void DAL_ccSentenciaPlanPagos_Add(DTO_ccCreditoDocu credito)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccSentenciaPlanPagos  " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[FechaCuota]   " +
                                               "    ,[VlrCuota]  )" +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@CuotaID    " +
                                               "  ,@FechaCuota    " +
                                               "  ,@VlrCuota  )   ";

                #endregion
                #region Asigna Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = credito.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = credito.VlrCuotaSentencia.Value;
                for (int i = 0; i < credito.PlazoSentencia.Value; i++)
                {
                    mySqlCommandSel.Parameters["@CuotaID"].Value = i+1;
                    mySqlCommandSel.Parameters["@FechaCuota"].Value = i == 0? credito.FechaCuota1Sentencia.Value : credito.FechaCuota1Sentencia.Value.Value.AddMonths(i);
                    mySqlCommandSel.ExecuteNonQuery();
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPlanPagos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccCreditoDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccSentenciaPlanPagos_Update(DTO_ccSentenciaPlanPagos pago)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCuota", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);

                #endregion
                #region Asignacion Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = pago.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = pago.CuotaID.Value;
                mySqlCommandSel.Parameters["@FechaCuota"].Value = pago.FechaCuota.Value.Value.Date;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = pago.VlrCuota.Value;
      
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "UPDATE ccSentenciaPlanPagos SET " +
                    "   FechaCuota = @FechaCuota, " +
                    "   VlrCuota = @VlrCuota, " +
                    "WHERE NumeroDoc = @numeroDoc AND CuotaID = @CuotaID";

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
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoPlanPagos_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina una cuota de una plan de pagos por su identificador único
        /// </summary>
        /// <returns></returns>
        public void DAL_ccSentenciaPlanPagos_Delete(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " Delete from ccSentenciaPlanPagos WHERE Consecutivo =  @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSentenciaPlanPagos_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summMOary>
        /// Trae todos los registros de DTO_PlanDePagos
        /// </summary>
        /// <returns>retorna una lista de DTO_PlanDePagos</returns>
        public List<DTO_ccSentenciaPlanPagos> DAL_ccSentenciaPlanPagos_GetSaldoCuotas(int numeroDoc)
        {
            try
            {
                List<DTO_ccSentenciaPlanPagos> result = new List<DTO_ccSentenciaPlanPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM CARTERA_SentenciaSaldoCuotas (@EmpresaID,@NumeroDoc)";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSentenciaPlanPagos  dto = new DTO_ccSentenciaPlanPagos();
                    dto.CuotaID.Value = Convert.ToInt16(dr["Cuota"]);
                    dto.FechaCuota.Value = Convert.ToDateTime(dr["Fecha"]);
                    dto.VlrCuota.Value = Convert.ToDecimal(dr["Valor"]);
                    dto.Abono.Value = Convert.ToDecimal(dr["Abono"]);
                    dto.Saldo.Value = Convert.ToDecimal(dr["Saldo"]);
                    result.Add(dto);             
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraFin_GetComprobanteAmortizaDerechos");
                throw exception;
            }
        }

        #endregion
    }

}
