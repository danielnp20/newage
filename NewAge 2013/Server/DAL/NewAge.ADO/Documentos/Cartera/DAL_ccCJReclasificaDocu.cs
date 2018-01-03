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
    public class DAL_ccCJReclasificaDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCJReclasificaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCJReclasificaDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCJReclasificaDocu</returns>
        public DTO_ccCJReclasificaDocu DAL_ccCJReclasificaDocu_GetByNumDoc(int NumeroDoc)
        {
            try
            {
                DTO_ccCJReclasificaDocu result = new DTO_ccCJReclasificaDocu();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccCJReclasificaDocu with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if(dr.Read())
                    result = new DTO_ccCJReclasificaDocu(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJReclasificaDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCJReclasificaDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCJReclasificaDocu_Add(DTO_ccCJReclasificaDocu docu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "    INSERT INTO ccCJReclasificaDocu   " +
                                               "    ([Empresa] " +
                                               "    ,[NumeroDoc] " +
                                               "    ,[NumDocCredito] " + 
                                               "    ,[FechaCorte] "+ 
                                               "    ,[Valor] "+
                                               "    ,[Iva] ) " +
                                               "  VALUES " +
                                               "  (@Empresa " +
                                               "  ,@NumeroDoc " +
                                               "  ,@NumDocCredito " +
                                               "  ,@FechaCorte " +
                                               "  ,@Valor " +
                                               "  ,@Iva ) ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char,UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = docu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = docu.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = docu.FechaCorte.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = docu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = docu.Iva.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJReclasificaDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla DTO_ccCJReclasificaDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccCJReclasificaDocu_Update(DTO_ccCJReclasificaDocu docu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@Empresa"].Value = docu.Empresa.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = docu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = docu.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = docu.FechaCorte.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = docu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = docu.Iva.Value;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE ccCJReclasificaDocu SET" +
                    "  NumDocCredito = @NumDocCredito " +
                    "  ,FechaCorte = @FechaCorte " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJReclasificaDocu_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras


        #endregion
    }

}
    