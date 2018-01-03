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
    public class DAL_ccSolicitudPlanPagos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudPlanPagos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_PlanDePagos
        /// </summary>
        /// <returns>retorna una lista de DTO_PlanDePagos</returns>
        public List<DTO_ccSolicitudPlanPagos> DAL_ccSolicitudPlanPagos_GetByNumDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudPlanPagos PA with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc " +
                                           "ORDER BY CuotaID ";


                List<DTO_ccSolicitudPlanPagos> result = new List<DTO_ccSolicitudPlanPagos>();

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudPlanPagos dto = new DTO_ccSolicitudPlanPagos(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudPlanPagos_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="pagos"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudPlanPagos_Add(DTO_PlanDePagos pagos, int numDocNro)
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
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtrosFijos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoSeguro", SqlDbType.Int);
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccSolicitudPlanPagos  " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[FechaCuota]   " +
                                               "    ,[VlrCuota]   " +
                                               "    ,[VlrCapital]   " +
                                               "    ,[VlrInteres]   " +
                                               "    ,[VlrSeguro]   " +
                                               "    ,[VlrOtro1]   " +
                                               "    ,[VlrOtro2]   " +
                                               "    ,[VlrOtro3]   " +
                                               "    ,[VlrOtrosFijos]   " +
                                               "    ,[VlrSaldoCapital] "+
                                               "    ,[VlrSaldoSeguro]) " +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@CuotaID    " +
                                               "  ,@FechaCuota    " +
                                               "  ,@VlrCuota    " +
                                               "  ,@VlrCapital    " +
                                               "  ,@VlrInteres    " +
                                               "  ,@VlrSeguro    " +
                                               "  ,@VlrOtro1    " +
                                               "  ,@VlrOtro2    " +
                                               "  ,@VlrOtro3    " +
                                               "  ,@VlrOtrosFijos    " +
                                               "  ,@VlrSaldoCapital "+
                                               "  ,@VlrSaldoSeguro) ";

                #endregion                
                #region Asignacion Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDocNro;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = pagos.VlrCuota;
                Dictionary<string, bool> dic = pagos.ComponentesFijos;
                decimal vlrSaldoCapital = pagos.VlrPrestamo;
                decimal vlrSaldoCapitalPoliza = pagos.VlrPoliza;

                for (int i = 0; i < pagos.Cuotas.Count; i++)
                {
                    mySqlCommandSel.Parameters["@CuotaID"].Value = pagos.Cuotas[i].NumCuota;
                    mySqlCommandSel.Parameters["@FechaCuota"].Value = pagos.Cuotas[i].Fecha;
                    mySqlCommandSel.Parameters["@VlrCapital"].Value = pagos.Cuotas[i].Capital;
                    mySqlCommandSel.Parameters["@VlrInteres"].Value = pagos.Cuotas[i].Intereses;
                    mySqlCommandSel.Parameters["@VlrSeguro"].Value = pagos.Cuotas[i].Seguro;
                    mySqlCommandSel.Parameters["@VlrCuota"].Value = pagos.Cuotas[i].ValorCuota;
                    mySqlCommandSel.Parameters["@VlrOtro1"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrOtro2"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrOtro3"].Value = 0;

                    List<string> extraComp = pagos.Cuotas[i].Componentes;
                    List<int> valores = pagos.Cuotas[i].ValoresComponentes;
                    int totalFijos = 0;
                    int indexOtros = 1;
                    for (int j = 0; j < extraComp.Count; j++)
                    {
                        string nombre = extraComp[j];
                        int valor = valores[j];
                        DTO_ccSolicitudComponentes comp = pagos.ComponentesAll.Where(x => x.Descripcion.Value == nombre).First();
                        if (dic[comp.ComponenteCarteraID.Value])
                            totalFijos += valores[j];
                        else
                        {
                            if (indexOtros == 1)
                                mySqlCommandSel.Parameters["@VlrOtro1"].Value = valores[j];
                            if (indexOtros == 2)
                                mySqlCommandSel.Parameters["@VlrOtro2"].Value = valores[j];
                            if (indexOtros == 3)
                                mySqlCommandSel.Parameters["@VlrOtro3"].Value = valores[j];
                            indexOtros++;
                        }
                    }

                    mySqlCommandSel.Parameters["@VlrOtrosfijos"].Value = totalFijos;
                    mySqlCommandSel.Parameters["@VlrSaldoCapital"].Value = vlrSaldoCapital;
                    mySqlCommandSel.Parameters["@VlrSaldoSeguro"].Value = vlrSaldoCapitalPoliza;
                    mySqlCommandSel.ExecuteNonQuery();
                    
                    if(vlrSaldoCapitalPoliza != 0)
                        vlrSaldoCapitalPoliza = vlrSaldoCapitalPoliza - pagos.Cuotas[i].Seguro;
                    vlrSaldoCapital = vlrSaldoCapital - pagos.Cuotas[i].Capital;
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudPlanPagos_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccCreditoDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudPlanPagos_Update(DTO_ccSolicitudPlanPagos pago)
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
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtrosFijos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapital", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@VlrSaldoSeguro", SqlDbType.Int);
                #endregion
                #region Asignacion Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = pago.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = pago.CuotaID.Value;
                mySqlCommandSel.Parameters["@FechaCuota"].Value = pago.FechaCuota.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = pago.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrCapital"].Value = pago.VlrCapital.Value;
                mySqlCommandSel.Parameters["@VlrInteres"].Value = pago.VlrInteres.Value;
                mySqlCommandSel.Parameters["@VlrSeguro"].Value = pago.VlrSeguro.Value;
                mySqlCommandSel.Parameters["@VlrOtro1"].Value = pago.VlrOtro1.Value;
                mySqlCommandSel.Parameters["@VlrOtro2"].Value = pago.VlrOtro2.Value;
                mySqlCommandSel.Parameters["@VlrOtro3"].Value = pago.VlrOtro3.Value;
                mySqlCommandSel.Parameters["@VlrOtrosFijos"].Value = pago.VlrOtrosfijos.Value;
                mySqlCommandSel.Parameters["@VlrSaldoCapital"].Value = pago.VlrSaldoCapital.Value;
                //mySqlCommandSel.Parameters["@VlrSaldoSeguro"].Value = vlrSaldoCapitalPoliza;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccSolicitudPlanPagos SET " +
                    "   FechaCuota = @FechaCuota, " +
                    "   VlrCuota = @VlrCuota, " +
                    "   VlrCapital = @VlrCapital, " +
                    "   VlrInteres = @VlrInteres, " +
                    "   VlrSeguro = @VlrSeguro, " +
                    "   VlrOtro1 = @VlrOtro1, " +
                    "   VlrOtro2 = @VlrOtro2, " +
                    "   VlrOtro3 = @VlrOtro3, " +
                    "   VlrOtrosFijos = @VlrOtrosFijos, " +
                    "   VlrSaldoCapital = @VlrSaldoCapital, " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudPlanPagos_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccCreditoDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudPlanPagos_UpdateFromPlanPago(DTO_PlanDePagos pagos, int numeroDoc)
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
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrInteres", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSeguro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro1", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtro3", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrOtrosFijos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSaldoSeguro", SqlDbType.Int);
                #endregion
                #region Asignacion Valores
                #region Asignacion Valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = pagos.VlrCuota;
                Dictionary<string, bool> dic = pagos.ComponentesFijos;
                decimal vlrSaldoCapital = pagos.VlrPrestamo;
                decimal vlrSaldoCapitalPoliza = pagos.VlrPoliza;
                for (int i = 0; i < pagos.Cuotas.Count; i++)
                {
                    mySqlCommandSel.Parameters["@CuotaID"].Value = pagos.Cuotas[i].NumCuota;
                    mySqlCommandSel.Parameters["@FechaCuota"].Value = pagos.Cuotas[i].Fecha;
                    mySqlCommandSel.Parameters["@VlrCapital"].Value = pagos.Cuotas[i].Capital;
                    mySqlCommandSel.Parameters["@VlrInteres"].Value = pagos.Cuotas[i].Intereses;
                    mySqlCommandSel.Parameters["@VlrSeguro"].Value = pagos.Cuotas[i].Seguro;
                    mySqlCommandSel.Parameters["@VlrOtro1"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrOtro2"].Value = 0;
                    mySqlCommandSel.Parameters["@VlrOtro3"].Value = 0;

                    List<string> extraComp = pagos.Cuotas[i].Componentes;
                    List<int> valores = pagos.Cuotas[i].ValoresComponentes;
                    int totalFijos = 0;
                    for (int j = 0; j < extraComp.Count; j++)
                    {
                        string nombre = extraComp[j];
                        int valor = valores[j];
                        DTO_ccSolicitudComponentes comp = pagos.ComponentesAll.Where(x => x.Descripcion.Value == nombre).First();
                        if (dic[comp.ComponenteCarteraID.Value])
                        {
                            totalFijos += valores[j];
                        }
                        else
                        {
                            if (j == 0)
                                mySqlCommandSel.Parameters["@VlrOtro1"].Value = valores[j];
                            if (j == 1)
                                mySqlCommandSel.Parameters["@VlrOtro2"].Value = valores[j];
                            if (j == 2)
                                mySqlCommandSel.Parameters["@VlrOtro3"].Value = valores[j];
                        }
                    }

                    mySqlCommandSel.Parameters["@VlrOtrosfijos"].Value = totalFijos;
                    mySqlCommandSel.Parameters["@VlrSaldoCapital"].Value = vlrSaldoCapital;
                    mySqlCommandSel.Parameters["@VlrSaldoSeguro"].Value = vlrSaldoCapitalPoliza;
                    mySqlCommandSel.ExecuteNonQuery();

                    vlrSaldoCapital = vlrSaldoCapital - pagos.Cuotas[i].Capital;
                }
                #endregion
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccSolicitudPlanPagos SET " +
                    "   FechaCuota = @FechaCuota, " +
                    "   VlrCuota = @VlrCuota, " +
                    "   VlrCapital = @VlrCapital, " +
                    "   VlrInteres = @VlrInteres, " +
                    "   VlrSeguro = @VlrSeguro, " +
                    "   VlrOtro1 = @VlrOtro1, " +
                    "   VlrOtro2 = @VlrOtro2, " +
                    "   VlrOtro3 = @VlrOtro3, " +
                    "   VlrOtrosFijos = @VlrOtrosFijos, " +
                    "   VlrSaldoCapital = @VlrSaldoCapital, " +
                    "   VlrSaldoSeguro = @VlrSaldoSeguro, " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudPlanPagos_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina el plan de pagos asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudPlanPagos_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudPlanPagos WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudPlanPagos_Delete");
                throw exception;
            }
        }

    }

}
