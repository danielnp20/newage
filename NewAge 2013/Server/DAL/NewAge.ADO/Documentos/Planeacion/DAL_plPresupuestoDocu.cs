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
    public class DAL_plPresupuestoDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plPresupuestoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="docu">Cierre nuevo</param>
        /// <returns></returns>
        public void DAL_plPresupuestoDocu_Add(DTO_plPresupuestoDocu docu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocPresup", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = docu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDocPresup"].Value = docu.NumeroDocPresup.Value;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plPresupuestoDocu(EmpresaID,NumeroDoc,NumeroDocPresup) VALUES(@EmpresaID,@NumeroDoc,@NumeroDocPresup)";
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="docu">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoDocu_Update(DTO_plPresupuestoDocu docu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE plPresupuestoDocu SET NumeroDocPresup=@NumeroDocPresup " +
                    "WHERE NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion de comandos

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocPresup", SqlDbType.Int);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = docu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDocPresup"].Value = docu.NumeroDocPresup.Value;
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDocu_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina un registro 
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento a eliminar</param>
        /// <returns></returns>
        public void DAL_plPresupuestoDocu_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =  "DELTE FROM plPresupuestoDocu WHERE NumeroDoc = @NumeroDoc";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDocu_Delete");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae la informacion de los presupuestos 
        /// </summary>
        /// <returns>Lista de Docus</returns>
        public List<DTO_plPresupuestoDocu> DAL_plPresupuestoDocu_GetByNumeroDocPresup(int numeroDocPresup)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                List<DTO_plPresupuestoDocu> result = new List<DTO_plPresupuestoDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "select * from plPresupuestoDocu with(nolock) where EmpresaID = @EmpresaID and NumeroDocPresup = @NumeroDocPresup";
                #endregion
                #region Creacion de comandos
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocPresup", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDocPresup"].Value = numeroDocPresup;
                #endregion

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_plPresupuestoDocu dto = new DTO_plPresupuestoDocu(dr);                 
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDocu_GetByNumeroDocPresup");
                throw exception;
            }
        }

        #endregion
    }
}
