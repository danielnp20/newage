using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_noBeneficiosxEmpleado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noBeneficiosxEmpleado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Adiciona un beneficio de compensación flexible a un empleado
        /// </summary>
        /// <param name="beneficio">objeto beneficio</param>
        public void DAL_noBeneficiosxEmpleado_Add(DTO_noBeneficiosxEmpleado beneficio)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    INSERT INTO noBeneficiosxEmpleado " +
                                                "    ([EmpresaID] " +
                                                "    ,[EmpleadoID] " +
                                                "    ,[ContratoNOID] " +
                                                "    ,[CompFlexibleID] " +
                                                "    ,[Valor] " +
                                                "    ,[TerceroID] " +
                                                "    ,[ActivaInd] " +
                                                "    ,[eg_noEmpleado] " +
                                                "    ,[eg_noCompFlexible]) " +
                                                "    VALUES " +
                                                "    (@EmpresaID " +
                                                "    ,@EmpleadoID  " +
                                                "    ,@ContratoNOID " +
                                                "    ,@CompFlexibleID " +
                                                "    ,@Valor " +
                                                "    ,@TerceroID " +
                                                "    ,@ActivaInd " +
                                                "    ,@eg_noEmpleado " +
                                                "    ,@eg_noCompFlexible)";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompFlexibleID", SqlDbType.Char, UDT_CompFlexibleID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noCompFlexible", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);


                mySqlCommandSel.Parameters["@EmpresaID"].Value = beneficio.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = beneficio.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = beneficio.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@CompFlexibleID"].Value = beneficio.CompFlexibleID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = beneficio.Valor.Value.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = beneficio.TerceroID.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = beneficio.ActivaInd.Value;
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = egCtrl;
                mySqlCommandSel.Parameters["@eg_noCompFlexible"].Value = egCtrl;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noBeneficiosxEmpleado_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el listado de beneficios por Empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de beneficios</returns>
        public List<DTO_noBeneficiosxEmpleado> DAL_noBeneficiosxEmpleado_Get(string empleadoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    SELECT [EmpresaID] " +
                                                "    ,[EmpleadoID] " +
                                                "    ,[ContratoNOID] " +
                                                "    ,[CompFlexibleID] " +
                                                "    ,[Valor] " +
                                                "    ,[TerceroID] " +
                                                "    ,[ActivaInd] " +
                                                "    FROM  noBeneficiosxEmpleado with(nolock) " +
                                                "    where EmpleadoID = @EmpleadoID";

                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                List<DTO_noBeneficiosxEmpleado> result = new List<DTO_noBeneficiosxEmpleado>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noBeneficiosxEmpleado dto = new DTO_noBeneficiosxEmpleado(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noBeneficiosxEmpleado_Get");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza los valores editables del beneficio
        /// </summary>
        /// <param name="beneficio">beneficio</param>
        public void DAL_noBeneficiosxEmpleado_Upd(DTO_noBeneficiosxEmpleado beneficio)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  UPDATE noBeneficiosxEmpleado " +
                                              "  SET " +
                                              "  ,[Valor] = @Valor " +
                                              "  ,[TerceroID] = @TerceroID " +
                                              "  ,[ActivaInd] = @ActivaInd " +                                              
                                              "  WHERE EmpresaID = @EmpresaID  " +
                                              "  AND EmpleadoID = @EmpleadoID" +
                                              "  AND CompFlexibleID = @CompFlexibleID   ";


                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompFlexibleID", SqlDbType.Char, UDT_CompFlexibleID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = beneficio.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = beneficio.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@CompFlexibleID"].Value = beneficio.CompFlexibleID.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = beneficio.Valor.Value.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = beneficio.TerceroID.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = beneficio.ActivaInd.Value;

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
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noBeneficiosxEmpleado_Upd");
                throw exception;
            }
        }

    }
}
