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
    public class DAL_ccComisionDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccComisionDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public DTO_ccComisionDocu DAL_ccComisionDocu_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccComisionDocu result = new DTO_ccComisionDocu();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccComisionDocu with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if(dr.Read())
                    result = new DTO_ccComisionDocu(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccComisionDocu_Add(DTO_ccComisionDocu comisionDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "    INSERT INTO ccComisionDocu   " +
                                               "    ([NumeroDoc] "+ 
                                               "    ,[FechaInicial] "+ 
                                               "    ,[FechaFinal] "+ 
                                               "    ,[Valor] "+
                                               "    ,[Iva] ) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@FechaInicial " +
                                               "  ,@FechaFinal " +
                                               "  ,@Valor " +
                                               "  ,@Iva ) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = comisionDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = comisionDocu.FechaInicial.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = comisionDocu.FechaFinal.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = comisionDocu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = comisionDocu.Iva.Value;
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDocu_Add");
                throw exception;
            }
        }
        
        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccComisionDocu_Update(DTO_ccComisionDocu comisionDocu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = comisionDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = comisionDocu.FechaInicial.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = comisionDocu.FechaFinal.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = comisionDocu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = comisionDocu.Iva.Value;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE ccComisionDocu SET" +
                    "  FechaInicial = @FechaInicial "+
                    "  ,FechaFinal = @FechaFinal "+
                    "  ,Valor = @Valor "+
                    "  ,Iva = @Iva " +
                    " WHERE  NumeroDoc = @NumeroDoc ";
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccComisionDocu_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras


        #endregion
    }

}
    