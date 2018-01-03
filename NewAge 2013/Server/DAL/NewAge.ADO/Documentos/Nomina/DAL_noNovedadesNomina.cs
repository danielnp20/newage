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

namespace NewAge.ADO
{
    public class DAL_noNovedadesNomina : DAL_Base
    {
      /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noNovedadesNomina(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// lista las novedades de nomina por empleado
        /// </summary>
        /// <param name="empleadoID">identificador de empleado</param>
        /// <returns>listado de novedades</returns>
        public List<DTO_noNovedadesNomina> DAL_noNovedadesNomina_GetNovedades(string empleadoID)
        {        
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "    SELECT [EmpresaID] " +
                                                "    ,[EmpleadoID] " +
                                                "    ,noConceptoNOM.ConceptoNOID " +
                                                "    ,noConceptoNOM.Descriptivo " +
                                                "    ,[Valor] " +
                                                "    ,[FijaInd] " +
                                                "    ,[PeriodoPago] " +
                                                "    ,[OrigenNovedad] " +
                                                "    ,[ActivaInd] " +
                                                "    ,[eg_noEmpleado] " +
                                                "    ,[eg_noConceptoNOM] " +
                                                "    FROM  noNovedadesNomina with(nolock) " +
                                                "    INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noNovedadesNomina.ConceptoNOID " +
                                                "    AND noConceptoNOM.EmpresaGrupoID = @EmpresaID " +
                                                "    where noNovedadesNomina.EmpresaID = @EmpresaID AND EmpleadoID = @EmpleadoID ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                List<DTO_noNovedadesNomina> result = new List<DTO_noNovedadesNomina>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noNovedadesNomina dto = new DTO_noNovedadesNomina(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesNomina_GetNovedades");
                throw exception;
            }           
        }

        /// <summary>
        /// Agrega las novedades de nomina al sistmna
        /// </summary>
        /// <param name="novedad">novedad de nomnina</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noNovedadesNomina_AddNovedades(DTO_noNovedadesNomina novedad)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "    INSERT INTO noNovedadesNomina " +
                                                "    ([EmpresaID] " +
                                                "    ,[EmpleadoID] " +
                                                "    ,[ConceptoNOID] " +
                                                "    ,[Valor] " +
                                                "    ,[FijaInd] " +
                                                "    ,[PeriodoPago] " +
                                                "    ,[OrigenNovedad] " +
                                                "    ,[ActivaInd] " +
                                                "    ,[eg_noEmpleado] " +
                                                "    ,[eg_noConceptoNOM]) " +
                                                "    VALUES " +
                                                "    (@EmpresaID " +
                                                "    ,@EmpleadoID  " +
                                                "    ,@ConceptoNOID " +
                                                "    ,@Valor " +
                                                "    ,@FijaInd " +
                                                "    ,@PeriodoPago " +
                                                "    ,@OrigenNovedad " +
                                                "    ,@ActivaInd " +
                                                "    ,@eg_noEmpleado " +
                                                "    ,@eg_noConceptoNOM)";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FijaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PeriodoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@OrigenNovedad", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                

                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = novedad.ConceptoNOID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = novedad.Valor.Value.Value;
                mySqlCommandSel.Parameters["@FijaInd"].Value = novedad.FijaInd.Value;
                mySqlCommandSel.Parameters["@PeriodoPago"].Value = novedad.PeriodoPago.Value;
                mySqlCommandSel.Parameters["@OrigenNovedad"].Value = novedad.OrigenNovedad.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = novedad.ActivaInd.Value;
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noEmpleado, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);

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
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesNomina_GetNovedades");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza una novedad de nomina en el sistema
        /// </summary>
        /// <param name="novedad">novedad de nomnina</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noNovedadesNomina_UpdNovedad(DTO_noNovedadesNomina novedad)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "    UPDATE noNovedadesNomina "   +
                                                "    SET [Valor] = @Valor "   +
                                                "    ,[FijaInd] = @FijaInd "   +
                                                "    ,[PeriodoPago] = @PeriodoPago "   +
                                                "    ,[ActivaInd] = @ActivaInd "   +
                                                "    WHERE EmpleadoID = @EmpleadoID  "   +
                                                "    AND EmpresaID = @EmpresaID " +
                                                "    AND ConceptoNOID = @ConceptoNOID"; 

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FijaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PeriodoPago", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);
      
                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = novedad.ConceptoNOID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = novedad.Valor.Value.Value;
                mySqlCommandSel.Parameters["@FijaInd"].Value = novedad.FijaInd.Value;
                mySqlCommandSel.Parameters["@PeriodoPago"].Value = novedad.PeriodoPago.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = novedad.ActivaInd.Value;

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
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesNomina_UpdNovedad");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si la novedad eexiste
        /// /// </summary>
        /// <param name="empleadoID">detalle novedad</param>
        /// <returns>nunero de novedades</returns>
        public int DAL_noNovedadesNomina_ExistNovedad(DTO_noNovedadesNomina novedad)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "      SELECT COUNT(EmpleadoID)   " +
                                               "     FROM noNovedadesNomina   " +
                                               "     WHERE EmpleadoID = @EmpleadoID    " +
                                               "     AND EmpresaID = @EmpresaID   " +
                                               "     AND ConceptoNOID = @ConceptoNOID   ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = novedad.ConceptoNOID.Value;

                int count = (int) mySqlCommandSel.ExecuteScalar();
                return count;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesNomina_ExistNovedad");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina una novedad de Nomina
        /// </summary>
        /// <param name="novedad">novedad de nomina</param>
        public void DAL_noNovedadesNomina__Delete(DTO_noNovedadesNomina novedad)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "   DELETE    " +
                                                "   noNovedadesNomina  " +
                                                "   WHERE EmpresaID = @EmpresaID   " +
                                                "   AND EmpleadoID = @EmpleadoID   " +
                                                "   AND ConceptoNOID = @ConceptoNOID   ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = novedad.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = novedad.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = novedad.ConceptoNOID.Value;

                mySqlCommandSel.ExecuteNonQuery();
                  
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noNovedadesNomina__Delete");
                throw exception;
            }
        }

        #endregion
    }
}
