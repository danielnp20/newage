using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.ADO
{
    public class DAL_noNovedadesContrato: DAL_Base
    {
      /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noNovedadesContrato(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Lista las novedades de contrato por empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de novedades de contrato</returns>
        public List<DTO_noNovedadesContrato> DAL_noNovedadesContrato_GetNovedades(string empleadoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "    SELECT * " +
                                                "    FROM  noNovedadesContrato with(nolock) " +
                                                "    INNER JOIN noContratoNov on noContratoNov.ContratoNONovID = noNovedadesContrato.ContratoNONovID    " +
                                                "    AND noContratoNov.EmpresaGrupoID  = @EmpresaID  " +
                                                "    where noNovedadesContrato.EmpresaID = @EmpresaID " +
                                                "    AND EmpleadoID = @EmpleadoID";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                List<DTO_noNovedadesContrato> result = new List<DTO_noNovedadesContrato>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noNovedadesContrato dto = new DTO_noNovedadesContrato(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noNovedadesContrato_GetNovedades");
                throw exception;
            }
        }

        /// <summary>
        /// Adicona una novedad de contrato 
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noNovedadesContrato_AddNovedades(DTO_noNovedadesContrato novedad)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    INSERT INTO noNovedadesContrato " +
                                                "    ([EmpresaID] " +
                                                "    ,[EmpleadoID] " +
                                                "    ,[ContratoNONovID] " +
                                                "    ,[FechaInicial] " +
                                                "    ,[FechaFinal] " +
                                                "    ,[Documento] " +
                                                "    ,[Valor] " +
                                                "    ,[ActivaInd] " +
                                                "    ,[Observacion] " +
                                                "    ,[ContratoNOID] " +
                                                "    ,[eg_noEmpleado] " +
                                                "    ,[eg_noContratoNov]) " +
                                                "    VALUES " +
                                                "    (@EmpresaID " +
                                                "    ,@EmpleadoID  " +
                                                "    ,@ContratoNONovID " +
                                                "    ,@FechaInicial " +
                                                "    ,@FechaFinal " +
                                                "    ,@Documento " +
                                                "    ,@Valor " +
                                                "    ,@ActivaInd " +
                                                "    ,@Observacion " +
                                                "    ,@ContratoNOID " +
                                                "    ,@eg_noEmpleado " +
                                                "    ,@eg_noContratoNov)";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNONovID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noContratoNov", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);


                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ContratoNONovID"].Value = novedad.ContratoNONovID.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = novedad.FechaInicial.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = novedad.FechaFinal.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = novedad.Documento.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = novedad.Valor.Value.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = novedad.ActivaInd.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = novedad.Observacion.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = novedad.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noContratoNov"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noContratoNov, this.Empresa, egCtrl); ;

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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noNovedadesContrato_AddNovedades");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza una novedad de nomina en el sistema
        /// </summary>
        /// <param name="novedad">novedad de nomnina</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noNovedadesContrato_UpdNovedad(DTO_noNovedadesContrato novedad)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  UPDATE noNovedadesContrato " +
                                              "  SET [FechaFinal] = @FechaFinal " +
                                              "  ,[Documento] = @Documento " +
                                              "  ,[Valor] = @Valor " +
                                              "  ,[ActivaInd] = @ActivaInd " +
                                              "  ,[Observacion] = @Observacion " +
                                              "  WHERE EmpleadoID = @EmpleadoID  " +
	                                          "  AND EmpresaID = @EmpresaID" +
                                              "  AND ContratoNONovID = @ContratoNONovID   " +
                                              "  AND FechaInicial = @FechaInicial   ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNONovID", SqlDbType.Char, UDT_ContratoNONovID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);                
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ContratoNONovID"].Value = novedad.ContratoNONovID.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = novedad.FechaInicial.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = novedad.FechaFinal.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = novedad.Documento.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = novedad.Valor.Value.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = novedad.ActivaInd.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = novedad.Observacion.Value;


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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesContrato_UpdNovedad");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si la novedad eexiste
        /// /// </summary>
        /// <param name="empleadoID">detalle novedad</param>
        /// <returns>nunero de novedades</returns>
        public int DAL_noNovedadesContrato_ExistNovedad(DTO_noNovedadesContrato novedad)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =  "      SELECT COUNT(*)   " +
                                               "     FROM noNovedadesContrato   " +
                                               "     WHERE EmpleadoID = @EmpleadoID    " +
                                               "     AND EmpresaID = @EmpresaID   " +
                                               "     AND ContratoNONovID = @ContratoNONovID   " +
                                               "     AND FechaInicial = @FechaInicial   ";
  
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNONovID", SqlDbType.Char, UDT_ContratoNONovID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ContratoNONovID"].Value = novedad.ContratoNONovID.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = novedad.FechaInicial.Value;
      
                int count = (int)mySqlCommandSel.ExecuteScalar();
                return count;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesContrato_ExistNovedad");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina una novedad de contrato
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la elimina</returns>
        public bool DAL_noNovedadesContrato_Delete(DTO_noNovedadesContrato novedad)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "  SELECT count(*)  " + 
                                                "  FROM noLiquidacionesDetalle  " +
                                                "  WHERE noLiquidacionesDetalle.ContratoNONovID = @ContratoNONovID  " + 
                                                "  AND ConsNovContrato =@Consecutivo  ";

                mySqlCommandSel.Parameters.Add("@ContratoNONovID", SqlDbType.Char, UDT_ContratoNONovID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommandSel.Parameters["@ContratoNONovID"].Value = novedad.ContratoNONovID.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = novedad.Consecutivo.Value;

                int count = (int)mySqlCommandSel.ExecuteScalar();

                if (count > 0)
                    return false;
                else
                {
                    mySqlCommandSel.CommandText = "   DELETE    " +
                                                  "   noNovedadesContrato  " +
                                                  "   WHERE ContratoNONovID = @ContratoNONovID   " +
                                                  "   AND Consecutivo = @Consecutivo  ";

                    mySqlCommandSel.ExecuteNonQuery();
                    return true;
                }           
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesContrato_ExistNovedad");
                throw exception;
            }
        }
  
    }
}
