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
    public class DAL_noTraslado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noTraslado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Obtiene los traslados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de traslados</returns>
        public List<DTO_noTraslado> DAL_noTraslado_GetTraslados(string empleadoID)
        {        
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =     "    SELECT [EmpresaID]  " +
                                                  "    ,[ContratoNOID]  " +
                                                  "    ,[Fecha]  " +
                                                  "    ,[EmpleadoID]  " +
                                                  "    ,[OperacionNOID]  " +
                                                  "    ,[DescripTBase]  " +
                                                  "    ,[NumeroDoc]  " +
                                                  "    ,[eg_noEmpleado]  " +
                                                  "    ,[eg_noOperacion]  " +
                                                  "    ,[ProyectoID]    " +
                                                  "    ,[CentroCostoID] "   + 
                                                  "    FROM noTrasladosHistoria  " +
                                                  "    where EmpleadoID = @EmpleadoID";

                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                
                List<DTO_noTraslado> result = new List<DTO_noTraslado>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noTraslado dto = new DTO_noTraslado(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noTraslado_GetTraslados");
                throw exception;
            }           
        }
        
        /// <summary>
        /// Adiciona un traslado
        /// </summary>
        /// <param name="prestamo">objeto traslado</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public bool DAL_noTraslado_AddTraslado(DTO_noTraslado traslado)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "     INSERT INTO noTrasladosHistoria " +
                                                "     ([EmpresaID] " +
                                                "     ,[ContratoNOID] " +
                                                "     ,[Fecha] " +
                                                "     ,[EmpleadoID] " +
                                                "     ,[OperacionNOID] " +
                                                "     ,[DescripTBase] " +
                                                "     ,[NumeroDoc] " +
                                                "     ,[ProyectoID] " +
                                                "     ,[CentroCostoID]  " +
                                                "     ,[eg_noEmpleado] " +
                                                "     ,[eg_noOperacion] " +
                                                "     ,[eg_coProyecto] " +
                                                "     ,[eg_coCentroCosto]) " +
                                                "      VALUES " +
                                                "     (@EmpresaID " +
                                                "     ,@ContratoNOID " +
                                                "     ,@Fecha " +
                                                "     ,@EmpleadoID " +
                                                "     ,@OperacionNOID " +
                                                "     ,@DescripTBase " +
                                                "     ,@NumeroDoc " +
                                                "     ,@ProyectoID " +
                                                "     ,@CentroCostoID " +
                                                "     ,@eg_noEmpleado " +
                                                "     ,@eg_noOperacion  " +
                                                "     ,@eg_coProyecto  " +
                                                "     ,@eg_coCentroCosto)  ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@OperacionNOID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DescripTBase", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                

                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = traslado.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = traslado.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = traslado.FechaTraslado.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = traslado.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@OperacionNOID"].Value = traslado.OperacionNOID.Value;
                mySqlCommandSel.Parameters["@DescripTBase"].Value = traslado.Descripcion.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = traslado.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = traslado.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = traslado.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = egCtrl;
                mySqlCommandSel.Parameters["@eg_noOperacion"].Value = egCtrl;
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = egCtrl;
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = egCtrl;
     

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noTraslado_AddPrestamo");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza traslado historico
        /// </summary>
        /// <param name="novedad">traslado</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noTraslado_UpdTraslado(DTO_noTraslado traslado)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =  "   UPDATE noTrasladosHistoria    " +
                                               "   SET [OperacionNOID] = @OperacionNOID    " +
                                               "   ,[DescripTBase] = @DescripTBase    " +
                                               "   ,[ProyectoID] = @ProyectoID    " +
                                               "   ,[CentroCostoID] = @CentroCostoID    " +
                                               "    WHERE EmpresaID = @EmpresaID    " +
                                               "   AND ContratoNOID = @ContratoNOID    " +
                                               "   AND Fecha = @Fecha";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DescripTBase", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@OperacionNOID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = traslado.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = traslado.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@DescripTBase"].Value = traslado.Descripcion.Value;
                mySqlCommandSel.Parameters["@OperacionNOID"].Value = traslado.OperacionNOID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = traslado.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = traslado.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = traslado.FechaTraslado.Value;
            
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noTraslado_UpdTraslado");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si el traslado
        /// /// </summary>
        /// <param name="empleadoID">detalle novedad</param>
        /// <returns>nunero de novedades</returns>
        public int DAL_noTraslado_ExistNovedad(DTO_noTraslado traslado)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "     SELECT COUNT(*) " +
                                                "     FROM noTrasladosHistoria " +
                                                "     WHERE EmpresaID = @EmpresaID " +
                                                "     AND ContratoNOID = @ContratoNOID " +
                                                "     AND Fecha = @Fecha";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = traslado.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = traslado.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = traslado.FechaTraslado.Value;

                int count = (int)mySqlCommandSel.ExecuteScalar();
                return count;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noTraslado_ExistNovedad");
                throw exception;
            }
        }
    }
}
