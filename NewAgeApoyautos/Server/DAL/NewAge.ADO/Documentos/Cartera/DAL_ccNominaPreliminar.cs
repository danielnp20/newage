using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_ccNominaPreliminar : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccNominaPreliminar(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccNominaDeta> DAL_ccNominaPreliminar_GetByID(int NumDocNomina)
        {
            try
            {
                List<DTO_ccNominaDeta> result = new List<DTO_ccNominaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocNomina", SqlDbType.Int);
                mySqlCommand.Parameters["@NumDocNomina"].Value = NumDocNomina;

                mySqlCommand.CommandText = "SELECT * FROM ccNominaPreliminar with(nolock)  " +
                                           "WHERE NumDocNomina = @NumDocNomina";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccNominaDeta r = new DTO_ccNominaDeta(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCarteraDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccNominaPreliminar_Add(DTO_ccNominaDeta nomina, bool isCoperativa)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                if (isCoperativa)
                {
                    mySqlCommandSel.CommandText = "INSERT INTO ccNominaPreliminar   " +
                                                   "    ([NumDocNomina]   " +
                                                   "    ,[NumDocCredito]  " +
                                                   "    ,[CentroPagoID] " +
                                                   "    ,[ValorNomina]  " +
                                                   "    ,[ValorCuota]  " +
                                                   "    ,[FechaNomina]  " +
                                                   "    ,[FechaIncorpora]  " +
                                                   "    ,[NumDocIncorpora]  " +
                                                   "    ,[IndInconsistencia]  " +
                                                   "    ,[EstadoCruce]  " +
                                                   "    ,[InconsistenciaIncID]  " +
                                                   "    ,[Observacion]  " +
                                                   "    ,[eg_ccCentroPagoPAG]  " +
                                                   "    ,[eg_ccNominaINC])  " +
                                                   "VALUES    " +
                                                   "    (@NumeroDoc    " +
                                                   "    ,@NumDocCredito  " +
                                                   "    ,@CentroPagoID  " +
                                                   "    ,@ValorNomina  " +
                                                   "    ,@ValorCuota  " +
                                                   "    ,@FechaNomina  " +
                                                   "    ,@FechaIncorpora  " +
                                                   "    ,@NumDocIncorpora  " +
                                                   "    ,@IndInconsistencia  " +
                                                   "    ,@EstadoCruce  " +
                                                   "    ,@InconsistenciaIncID  " +
                                                   "    ,@Observacion  " +
                                                   "    ,@eg_ccCentroPagoPAG  " +
                                                   "    ,@eg_ccNominaINC)  ";
                }
                else
                {
                    mySqlCommandSel.CommandText = "INSERT INTO cfNominaPreliminar   " +
                                                   "    ([NumDocNomina]   " +
                                                   "    ,[NumDocCredito]  " +
                                                   "    ,[ValorNomina]  " +
                                                   "    ,[ValorCuota]  " +
                                                   "    ,[FechaNomina]  " +
                                                   "    ,[IndInconsistencia]  " +
                                                   "    ,[EstadoCruce]  " +
                                                   "    ,[InconsistenciaIncID]  " +
                                                   "    ,[eg_ccCentroPagoPAG]  " +
                                                   "    ,[eg_cfRecaudosINC])  " +
                                                   "VALUES    " +
                                                   "    (@NumeroDoc    " +
                                                   "    ,@NumDocCredito  " +
                                                   "    ,@ValorNomina  " +
                                                   "    ,@ValorCuota  " +
                                                   "    ,@FechaNomina  " +
                                                   "    ,@IndInconsistencia  " +
                                                   "    ,@EstadoCruce  " +
                                                   "    ,@InconsistenciaIncID  " +
                                                   "    ,@eg_ccCentroPagoPAG  " +
                                                   "    ,@eg_cfRecaudosINC)  ";
                }

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumDocNomina", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ValorNomina", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaNomina", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@IndInconsistencia", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EstadoCruce", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@InconsistenciaIncID", SqlDbType.Char, UDT_InconsistenciaIncID.MaxLength);


                if (isCoperativa)
                {
                    mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                    mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@eg_ccNominaINC", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIncorpora", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@NumDocIncorpora", SqlDbType.Int);
                }
                else
                {
                    mySqlCommandSel.Parameters.Add("@eg_cfRecaudosINC", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                }
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumDocNomina"].Value = nomina.NumDocNomina.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = nomina.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@ValorNomina"].Value = nomina.ValorNomina.Value;
                mySqlCommandSel.Parameters["@ValorCuota"].Value = nomina.ValorCuota.Value;
                mySqlCommandSel.Parameters["@FechaNomina"].Value = nomina.FechaNomina.Value;
                mySqlCommandSel.Parameters["@IndInconsistencia"].Value = nomina.IndInconsistencia.Value;
                mySqlCommandSel.Parameters["@EstadoCruce"].Value = nomina.EstadoCruce.Value;
                mySqlCommandSel.Parameters["@InconsistenciaIncID"].Value = nomina.InconsistenciaIncID.Value;
                if (isCoperativa)
                {
                    mySqlCommandSel.Parameters["@CentroPagoID"].Value = nomina.CentroPagoID.Value;
                    mySqlCommandSel.Parameters["@Observacion"].Value = nomina.Observacion.Value;
                    mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                    mySqlCommandSel.Parameters["@eg_ccNominaINC"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccNominaINC, this.Empresa, egCtrl);
                    mySqlCommandSel.Parameters["@FechaIncorpora"].Value = nomina.FechaIncorpora.Value;
                    mySqlCommandSel.Parameters["@NumDocIncorpora"].Value = nomina.NumDocIncorpora.Value;
                }
                else
                {
                    //mySqlCommandSel.Parameters["@eg_cfRecaudosINC"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cfRecaudosINC, this.Empresa, egCtrl);
                }
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
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccNominaPreliminar_Update(DTO_ccNominaDeta nomina, bool isCoperativa)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumDocNomina", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ValorNomina", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaNomina", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@IndInconsistencia", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EstadoCruce", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@InconsistenciaIncID", SqlDbType.Char, UDT_InconsistenciaIncID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                if (isCoperativa)
                {
                    mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIncorpora", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@NumDocIncorpora", SqlDbType.Int);
                }
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumDocNomina"].Value = nomina.NumDocNomina.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = nomina.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@ValorNomina"].Value = nomina.ValorNomina.Value;
                mySqlCommandSel.Parameters["@ValorCuota"].Value = nomina.ValorCuota.Value;
                mySqlCommandSel.Parameters["@FechaNomina"].Value = nomina.FechaNomina.Value;
                mySqlCommandSel.Parameters["@IndInconsistencia"].Value = nomina.IndInconsistencia.Value;
                mySqlCommandSel.Parameters["@EstadoCruce"].Value = nomina.EstadoCruce.Value;
                mySqlCommandSel.Parameters["@InconsistenciaIncID"].Value = nomina.InconsistenciaIncID.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = nomina.Consecutivo.Value;
                if (isCoperativa)
                {
                    mySqlCommandSel.Parameters["@CentroPagoID"].Value = nomina.CentroPagoID.Value;
                    mySqlCommandSel.Parameters["@Observacion"].Value = nomina.Observacion.Value;
                    mySqlCommandSel.Parameters["@FechaIncorpora"].Value = nomina.FechaIncorpora.Value;
                    mySqlCommandSel.Parameters["@NumDocIncorpora"].Value = nomina.NumDocIncorpora.Value;
                }
                #endregion
                #region CommandText
                if (isCoperativa)
                {
                    mySqlCommandSel.CommandText =
                        "UPDATE ccNominaPreliminar SET" +
                        "    NumDocNomina = @NumDocNomina  " +
                        "    ,NumDocCredito = @NumDocCredito  " +
                        "    ,CentroPagoID = @CentroPagoID  " +
                        "    ,ValorNomina = @ValorNomina  " +
                        "    ,ValorCuota = @ValorCuota  " +
                        "    ,FechaNomina = @FechaNomina  " +
                        "    ,FechaIncorpora = @FechaIncorpora  " +
                        "    ,NumDocIncorpora = @NumDocIncorpora  " +
                        "    ,IndInconsistencia = @IndInconsistencia  " +
                        "    ,EstadoCruce = @EstadoCruce  " +
                        "    ,InconsistenciaIncID = @InconsistenciaIncID  " +
                        "    ,Observacion = @Observacion  " +
                        " WHERE  Consecutivo = @Consecutivo";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                        "UPDATE cfNominaPreliminar SET" +
                        "    NumDocNomina = @NumDocNomina  " +
                        "    ,NumDocCredito = @NumDocCredito  " +
                        "    ,ValorNomina = @ValorNomina  " +
                        "    ,ValorCuota = @ValorCuota  " +
                        "    ,FechaNomina = @FechaNomina  " +
                        "    ,IndInconsistencia = @IndInconsistencia  " +
                        "    ,EstadoCruce = @EstadoCruce  " +
                        "    ,InconsistenciaIncID = @InconsistenciaIncID  " +
                        " WHERE  Consecutivo = @Consecutivo";
                }
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina la info de ccNominaPreliminar 
        /// </summary>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public void DAL_ccNominaPreliminar_Delete()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "delete from ccNominaPreliminar";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina la info de ccNominaPreliminar para un centro de pago
        /// </summary>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public void DAL_ccNominaPreliminar_DeleteByCentroPago(string centroPagoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommand.Parameters["@CentroPagoID"].Value = centroPagoID;

                mySqlCommand.CommandText = "delete from ccNominaPreliminar where CentroPagoID=@CentroPagoID";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae la info de una couta
        /// </summary>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="numDocCredito">Identificador del crédito</param>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public bool DAL_ccNominaPreliminar_HasIncorporacionPrevia(string centroPagoID, int numDocCredito)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoCruce", SqlDbType.Int);

                mySqlCommand.Parameters["@CentroPagoID"].Value = centroPagoID;
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommand.Parameters["@EstadoCruce"].Value = 11;

                mySqlCommand.CommandText = "select COUNT(*) from ccNominaPreliminar with(nolock) " +
                    "where CentroPagoID=@CentroPagoID AND NumDocNomina = @NumDocNomina AND NumDocCredito = @NumDocCredito and EstadoCruce = @EstadoCruce ";

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_HasIncorporacionPrevia");
                throw exception;
            }
        }

        #endregion

    }
}
