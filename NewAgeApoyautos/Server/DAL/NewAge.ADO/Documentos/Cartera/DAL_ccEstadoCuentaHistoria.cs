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
    public class DAL_ccEstadoCuentaHistoria : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccEstadoCuentaHistoria(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_ccEstadoCuentaComponentes
        /// </summary>
        /// <returns>retorna una lista de DTO_ccEstadoCuentaComponentes</returns>
        public DTO_ccEstadoCuentaHistoria DAL_ccEstadoCuentaHistoria_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                DTO_ccEstadoCuentaHistoria result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT ccEstadoCuentaHistoria.* " +
                                           "FROM ccEstadoCuentaHistoria WITH(NOLOCK) " +
                                           "WHERE ccEstadoCuentaHistoria.NumeroDoc = @numeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if(dr.Read())
                    result = new DTO_ccEstadoCuentaHistoria(dr);

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaHistoria_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccEstadoCuentaComponetes
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccEstadoCuentaHistoria_Add(DTO_ccEstadoCuentaHistoria estadoCuentaHistoria)
        {
            try
            {
                List<DTO_ccEstadoCuentaHistoria> result = new List<DTO_ccEstadoCuentaHistoria>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
              
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO ccEstadoCuentaHistoria   " +
                                                  "  ([NumeroDoc]   " +
                                                  "  ,[NumDocCredito] " +
                                                  "  ,[NumDocProceso] " +
                                                  "  ,[TasaMora] " +
                                                  "  ,[EC_Proposito] " +
                                                  "  ,[EC_Fecha] " +
                                                  "  ,[EC_FechaLimite] " +
                                                  "  ,[EC_Altura] " +
                                                  "  ,[EC_CuotasMora] " +
                                                  "  ,[EC_PrimeraCtaPagada] " +
                                                  "  ,[EC_SaldoPend] " +
                                                  "  ,[EC_SaldoMora] " +
                                                  "  ,[EC_SaldoTotal] " +
                                                  "  ,[EC_ValorPago] " +
                                                  "  ,[EC_ValorAbono] " +
                                                  "  ,[EC_SeguroVida] " +
                                                  "  ,[EC_FijadoInd] " +
                                                  "  ,[EC_NormalizaInd] " +
                                                  "  ,[EC_PolizaMvto]" +
                                                  "  ,[NumDocRevoca]" +
                                                  "  ,[FechaInicialFNC]" +
                                                  "  ,[DiasFNC])" +
                                                  "VALUES    " +
                                                  "  (@NumeroDoc    " +
                                                  "  ,@NumDocCredito " +
                                                  "  ,@NumDocProceso " +
                                                  "  ,@TasaMora " +
                                                  "  ,@EC_Proposito " +
                                                  "  ,@EC_Fecha " +
                                                  "  ,@EC_FechaLimite " +
                                                  "  ,@EC_Altura " +
                                                  "  ,@EC_CuotasMora " +
                                                  "  ,@EC_PrimeraCtaPagada " +
                                                  "  ,@EC_SaldoPend " +
                                                  "  ,@EC_SaldoMora " +
                                                  "  ,@EC_SaldoTotal " +
                                                  "  ,@EC_ValorPago " +
                                                  "  ,@EC_ValorAbono " +
                                                  "  ,@EC_SeguroVida " +
                                                  "  ,@EC_FijadoInd " +
                                                  "  ,@EC_NormalizaInd" +
                                                  "  ,@EC_PolizaMvto" +
                                                  "  ,@NumDocRevoca" +
                                                  "  ,@FechaInicialFNC" +
                                                  "  ,@DiasFNC)";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocProceso", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaMora", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EC_Proposito", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EC_Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EC_FechaLimite", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EC_Altura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_CuotasMora", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_PrimeraCtaPagada", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_SaldoPend", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_SaldoMora", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_SaldoTotal", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_ValorPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_ValorAbono", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_SeguroVida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_FijadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EC_NormalizaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EC_PolizaMvto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumDocRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaInicialFNC", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DiasFNC", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = estadoCuentaHistoria.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = estadoCuentaHistoria.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocProceso"].Value = estadoCuentaHistoria.NumDocProceso.Value;
                mySqlCommandSel.Parameters["@TasaMora"].Value = estadoCuentaHistoria.TasaMora.Value;
                mySqlCommandSel.Parameters["@EC_Proposito"].Value = estadoCuentaHistoria.EC_Proposito.Value;
                mySqlCommandSel.Parameters["@EC_Fecha"].Value = estadoCuentaHistoria.EC_Fecha.Value;
                mySqlCommandSel.Parameters["@EC_FechaLimite"].Value = estadoCuentaHistoria.EC_FechaLimite.Value;
                mySqlCommandSel.Parameters["@EC_Altura"].Value = estadoCuentaHistoria.EC_Altura.Value;
                mySqlCommandSel.Parameters["@EC_CuotasMora"].Value = estadoCuentaHistoria.EC_CuotasMora.Value;
                mySqlCommandSel.Parameters["@EC_PrimeraCtaPagada"].Value = estadoCuentaHistoria.EC_PrimeraCtaPagada.Value;
                mySqlCommandSel.Parameters["@EC_SaldoPend"].Value = estadoCuentaHistoria.EC_SaldoPend.Value;
                mySqlCommandSel.Parameters["@EC_SaldoMora"].Value = estadoCuentaHistoria.EC_SaldoMora.Value;
                mySqlCommandSel.Parameters["@EC_SaldoTotal"].Value = estadoCuentaHistoria.EC_SaldoTotal.Value;
                mySqlCommandSel.Parameters["@EC_ValorPago"].Value = estadoCuentaHistoria.EC_ValorPago.Value;
                mySqlCommandSel.Parameters["@EC_ValorAbono"].Value = estadoCuentaHistoria.EC_ValorAbono.Value;
                mySqlCommandSel.Parameters["@EC_SeguroVida"].Value = estadoCuentaHistoria.EC_SeguroVida.Value;
                mySqlCommandSel.Parameters["@EC_FijadoInd"].Value = estadoCuentaHistoria.EC_FijadoInd.Value;
                mySqlCommandSel.Parameters["@EC_NormalizaInd"].Value = estadoCuentaHistoria.EC_NormalizaInd.Value;
                mySqlCommandSel.Parameters["@EC_PolizaMvto"].Value = estadoCuentaHistoria.EC_PolizaMvto.Value;
                mySqlCommandSel.Parameters["@NumDocRevoca"].Value = estadoCuentaHistoria.NumDocRevoca.Value;
                mySqlCommandSel.Parameters["@FechaInicialFNC"].Value = estadoCuentaHistoria.FechaInicialFNC.Value;
                mySqlCommandSel.Parameters["@DiasFNC"].Value = estadoCuentaHistoria.DiasFNC.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccEstadoCuentaHistoria_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccEstadoCuentaComponetes
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccEstadoCuentaHistoria_Update(DTO_ccEstadoCuentaHistoria estadoCuentaHistoria)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query 
                mySqlCommandSel.CommandText =
                                "UPDATE ccEstadoCuentaHistoria SET" +
                                   "  NumDocProceso = @NumDocProceso " +
                                   "  ,EC_Proposito = @EC_Proposito " +
                                   "  ,EC_FechaLimite  = @EC_FechaLimite " +
                                   "  ,EC_Altura  = @EC_Altura " +
                                   "  ,EC_CuotasMora  = @EC_CuotasMora " +
                                   "  ,EC_PrimeraCtaPagada  = @EC_PrimeraCtaPagada " +
                                   "  ,EC_SaldoPend  = @EC_SaldoPend " +
                                   "  ,EC_SaldoMora  = @EC_SaldoMora " +
                                   "  ,EC_SaldoTotal  = @EC_SaldoTotal " +
                                   "  ,EC_ValorPago  = @EC_ValorPago " +
                                   "  ,EC_ValorAbono  = @EC_ValorAbono " +
                                   "  ,EC_SeguroVida  = @EC_SeguroVida " +
                                   "  ,EC_FijadoInd  = @EC_FijadoInd " +
                                   "  ,EC_NormalizaInd  = @EC_NormalizaInd " +
                                   "  ,EC_PolizaMvto  = @EC_PolizaMvto " +
                                   "  ,NumDocRevoca  = @NumDocRevoca " +
                                   "  ,FechaInicialFNC  = @FechaInicialFNC " +
                                   "  ,DiasFNC  = @DiasFNC " +
                                   " WHERE  NumDocCredito = @NumDocCredito";
                #endregion
                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocProceso", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_Proposito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EC_FechaLimite", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EC_Altura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_CuotasMora", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_PrimeraCtaPagada", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_SaldoPend", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EC_SaldoMora", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EC_SaldoTotal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EC_ValorPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_ValorAbono", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_SeguroVida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EC_FijadoInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EC_NormalizaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EC_PolizaMvto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumDocRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaInicialFNC", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DiasFNC", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = estadoCuentaHistoria.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocProceso"].Value = estadoCuentaHistoria.NumDocProceso.Value;                
                mySqlCommandSel.Parameters["@EC_Proposito"].Value = estadoCuentaHistoria.EC_Proposito.Value;
                mySqlCommandSel.Parameters["@EC_Fecha"].Value = estadoCuentaHistoria.EC_Fecha.Value;
                mySqlCommandSel.Parameters["@EC_FechaLimite"].Value = estadoCuentaHistoria.EC_FechaLimite.Value;
                mySqlCommandSel.Parameters["@EC_Altura"].Value = estadoCuentaHistoria.EC_Altura.Value;
                mySqlCommandSel.Parameters["@EC_CuotasMora"].Value = estadoCuentaHistoria.EC_CuotasMora.Value;
                mySqlCommandSel.Parameters["@EC_PrimeraCtaPagada"].Value = estadoCuentaHistoria.EC_PrimeraCtaPagada.Value;
                mySqlCommandSel.Parameters["@EC_SaldoPend"].Value = estadoCuentaHistoria.EC_SaldoPend.Value;
                mySqlCommandSel.Parameters["@EC_SaldoMora"].Value = estadoCuentaHistoria.EC_SaldoMora.Value;
                mySqlCommandSel.Parameters["@EC_SaldoTotal"].Value = estadoCuentaHistoria.EC_SaldoTotal.Value;
                mySqlCommandSel.Parameters["@EC_ValorPago"].Value = estadoCuentaHistoria.EC_ValorPago.Value;
                mySqlCommandSel.Parameters["@EC_ValorAbono"].Value = estadoCuentaHistoria.EC_ValorAbono.Value;
                mySqlCommandSel.Parameters["@EC_SeguroVida"].Value = estadoCuentaHistoria.EC_SeguroVida.Value;
                mySqlCommandSel.Parameters["@EC_FijadoInd"].Value = estadoCuentaHistoria.EC_FijadoInd.Value;
                mySqlCommandSel.Parameters["@EC_NormalizaInd"].Value = estadoCuentaHistoria.EC_NormalizaInd.Value;
                mySqlCommandSel.Parameters["@EC_PolizaMvto"].Value = estadoCuentaHistoria.EC_PolizaMvto.Value;
                mySqlCommandSel.Parameters["@NumDocRevoca"].Value = estadoCuentaHistoria.NumDocRevoca.Value;
                mySqlCommandSel.Parameters["@FechaInicialFNC"].Value = estadoCuentaHistoria.FechaInicialFNC.Value;
                mySqlCommandSel.Parameters["@DiasFNC"].Value = estadoCuentaHistoria.DiasFNC.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaHistoria_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccEstadoCuentaHistoria_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccEstadoCuentaHistoria WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_EstadoCuentaHistoria_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="actAprobacionGiro">Actividad para el documento de aprobacion de giro</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCreditoDocu> DAL_ccEstadoCuentaHistoria_GetForRevocatoria(string aseguradora, string componenteSeguro)
        {
            try
            {
                List<DTO_ccCreditoDocu> result = new List<DTO_ccCreditoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                if (string.IsNullOrEmpty(componenteSeguro))
                    return result;

                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@AseguradoraID"].Value = aseguradora;
                mySqlCommand.Parameters["@ComponenteCarteraID"].Value = componenteSeguro;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion
                #region CommandText
                mySqlCommand.CommandText =
                    " SELECT cred.*,cli.Descriptivo as Nombre,his.NumeroDoc as NumDocHistoria,his.EC_PolizaMvto, " +
		            "        (Select Case when SUM(SaldoValor-PagoValor) is not null Then SUM(SaldoValor-PagoValor) else 0 End  " +
                    "         from ccEstadoCuentaComponentes where ComponenteCarteraID = @ComponenteCarteraID and NumeroDoc = his.NumeroDoc) as VlrSaldoSeguro, " +
                    "        (Select top(1) Poliza from ccPolizaEstado where EmpresaID = @EmpresaID and NumDocCredito = his.NumDocCredito and AseguradoraID = @AseguradoraID order by Consecutivo desc) as PolizaUltima	" +
                    " FROM ccEstadoCuentaHistoria his  with(nolock)  " +
                    "    INNER JOIN ccCreditoDocu cred with(nolock) on his.NumDocCredito = cred.NumeroDoc    " +
                    "	 INNER JOIN gldocumentoCOntrol ctrl with(nolock) on his.NumeroDoc = ctrl.NumeroDoc    " +
                    "    LEFT JOIN ccCliente cli with(nolock) on cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                    " WHERE cred.EmpresaID=@EmpresaID and ctrl.Estado = 3 and his.EC_PolizaMvto = 3 and his.NumDocRevoca is null   ";
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCreditoDocu dto = new DTO_ccCreditoDocu(dr);
                    if (!string.IsNullOrWhiteSpace(dr["NumDocHistoria"].ToString()))
                        dto.NumDocHistoria.Value = Convert.ToInt32(dr["NumDocHistoria"]);
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.Poliza.Value = dr["PolizaUltima"].ToString();                  
                    dto.VlrSaldoSeguro.Value = Convert.ToDecimal(dr["VlrSaldoSeguro"]);
                    dto.VlrRevoca.Value = 0;
                    dto.VlrDiferencia.Value = dto.VlrSaldoSeguro.Value - dto.VlrRevoca.Value;
                    dto.Aprobado.Value = false;
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccEstadoCuentaHistoria_GetForRevocatoria");
                throw exception;
            }
        }

    }
}
