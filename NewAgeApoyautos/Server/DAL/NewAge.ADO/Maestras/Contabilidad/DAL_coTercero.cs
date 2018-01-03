using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using System.Reflection;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_coTercero : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coTercero(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) :
            base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Valida credenciales de un tercero
        /// </summary>
        /// <param name="tercero">Identificador de tercero</param>
        /// <param name="password">Contraseña de tercero</param>
        /// <returns>Returna un tercero</returns>
        public UserResult DAL_coTercero_ValidateUserCredentials(DTO_coTercero tercero, string password, byte? constrasenaRepCtrl)
        {
            try
            {
                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                byte[] buffer = new byte[16];

                //Se hace la consulta de nuevo para traer la contrasena
                mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.CommandText =    "SELECT Contrasena FROM coTercero " +
                                              "WHERE TerceroID = @TerceroID";

                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters["@TerceroID"].Value = tercero.ID.Value;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                bool hasPass = false;
                if (dr.Read())
                {
                    if (dr["Contrasena"] != null && !string.IsNullOrWhiteSpace(dr["Contrasena"].ToString()))
                    {
                        hasPass = true;
                        dr.GetBytes(0, 0, buffer, 0, buffer.Length);
                    }
                }
                dr.Close();

                if (hasPass)
                {
                    var userPass = AESCrypto.Decrypt(buffer);

                    int numRep = 0;
                    double hDiff = 0;

                    mySqlCommand.CommandText = "UPDATE coTercero " +
                                                "SET ContrasenaRep = @ContrasenaRep, FechaUltimaDig = @FechaUltimaDig " +
                                                "WHERE TerceroID = @TerceroID";

                    mySqlCommand.Parameters.Add("@ContrasenaRep", SqlDbType.TinyInt);
                    mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.SmallDateTime);

                    mySqlCommand.Parameters["@FechaUltimaDig"].Value = DateTime.Now.Date;

                    if (password != userPass)
                    {
                        if (tercero.ContrasenaRep.Value.HasValue)
                        {
                            // Revisa si el usuario ya esta bloqueado
                            if ((constrasenaRepCtrl - tercero.ContrasenaRep.Value) <= 0)
                            {
                                return UserResult.BlockUser;
                            }

                            if (tercero.FechaUltimaDig.Value.HasValue)
                                hDiff = (DateTime.Now - tercero.FechaUltimaDig.Value.Value).TotalHours;

                            if (hDiff <= 6)
                                numRep = tercero.ContrasenaRep.Value.Value;

                            mySqlCommand.Parameters["@ContrasenaRep"].Value = numRep + 1;
                            int rows = mySqlCommand.ExecuteNonQuery();

                            return UserResult.IncorrectPassword;
                        }
                    }

                    mySqlCommand.Parameters["@ContrasenaRep"].Value = numRep;
                    int rows1 = mySqlCommand.ExecuteNonQuery();
                }
                else
                {
                    return UserResult.NotExists;
                }

                return UserResult.AlreadyMember;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UserValidate, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coTercero_ValidateUserCredentials");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un tercero
        /// </summary>
        /// <param name="userID">Identificador del tercero</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public int DAL_coTercero_UpdatePassword(string terceroID, string pwd)
        {
            try
            {
                var encripPass = AESCrypto.Encrypt(pwd);

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "UPDATE coTercero " +
                    "SET Contrasena = @Contrasena, ContrasenaRep = 0, FechaUltimaDig = @FechaUltimaDig  " +
                    "WHERE EmpresaGrupoID = @EmpresaGrupoID and TerceroID = @TerceroID ";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Contrasena", SqlDbType.VarBinary, 256);
                mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Contrasena"].Value = encripPass;
                mySqlCommand.Parameters["@FechaUltimaDig"].Value = DateTime.Now;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;

                int rows = mySqlCommand.ExecuteNonQuery();

                return rows;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coTercero_UpdatePassword");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un tercero
        /// </summary>
        /// <param name="userID">Identificador del tercero</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool DAL_coTercero_ResetPassword(string terceroID, string pwd, bool insideAnotherTx)
        {
            bool result = true;
            try
            {
                var encripPass = AESCrypto.Encrypt(pwd);

                if (!insideAnotherTx)
                    base.MySqlConnectionTx = base.MySqlConnection.BeginTransaction();

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "UPDATE coTercero " +
                    "SET Contrasena = @Contrasena, ContrasenaRep = 0, FechaUltimaDig = @FechaUltimaDig " +
                    "WHERE EmpresaGrupoID = @EmpresaGrupoID and TerceroID = @TerceroID ";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Contrasena", SqlDbType.VarBinary, 256);
                mySqlCommand.Parameters.Add("@FechaUltimaDig", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Contrasena"].Value = encripPass;
                mySqlCommand.Parameters["@FechaUltimaDig"].Value = DateTime.Now;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;

                int rows = mySqlCommand.ExecuteNonQuery();

                if (rows == 0)
                    result = false;

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                var exception = new Exception(DictionaryMessages.Err_UpdatePwd, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coTercero_ResetPassword");
                throw exception;
            }
            finally
            {
                if (result)
                {
                    if (!insideAnotherTx)
                        base.MySqlConnectionTx.Commit();
                }
                else
                {
                    if (base.MySqlConnectionTx != null)
                    {
                        base.MySqlConnectionTx.Rollback();
                    }
                }
            }

        }

        #endregion
    }
}
