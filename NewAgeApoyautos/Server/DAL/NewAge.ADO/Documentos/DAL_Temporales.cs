using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_Temporales : DAL_Base
    {
        private int maxBufferSize = 1024 * 1024 * 50;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Temporales(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn): base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Revisa si existe un temporal segun el origen y el usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public bool DAL_aplTemporales_HasTemp(string origen, DTO_seUsuario usuario)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                    "SELECT Count(*) FROM aplTemporales WHERE EmpresaID=@EmpresaID AND UsuarioID=@UsuarioID AND Llave=@Llave";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Llave", SqlDbType.VarChar, 100);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Llave"].Value = origen;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;

                int res = Convert.ToInt16(mySqlCommand.ExecuteScalar());
                return res == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplTemporales_HasTemp");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el temporal de un origen determinado y luego lo borra de los temporales
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public byte[] DAL_aplTemporales_GetByOrigen(string origen, DTO_seUsuario usuario)
        {
            try
            {
                byte[] objetoSerializado = new byte[maxBufferSize];

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                    "SELECT Objeto, * FROM aplTemporales WHERE EmpresaID=@EmpresaID AND UsuarioID=@UsuarioID AND Llave=@Llave";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Llave", SqlDbType.VarChar, 100);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Llave"].Value = origen;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                    dr.GetBytes(0, 0, objetoSerializado, 0, maxBufferSize);

                dr.Close();

                return objetoSerializado;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplTemporales_GetByOrigen");
                throw exception;
            }
        }

        /// <summary>
        /// Guarda un objeto en temporales. También borra un objeto que anteriormente estuviese bajo el mismo origen para ese usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario</param>
        /// <param name="objeto">objeto a guardar</param>
        public void DAL_aplTemporales_Save(string origen, DTO_seUsuario usuario, object objeto)
        {
            try
            {
                this.DAL_aplTemporales_Clean(origen, usuario);

                byte[] arr = null;
                if (objeto != null)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream();
                    bf.Serialize(ms, objeto);
                    arr = ms.ToArray();
                }

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                    "INSERT INTO aplTemporales (EmpresaID,Llave,UsuarioID,Fecha,TipoObjeto,Objeto)"+
                        "VALUES (@EmpresaID, @Llave ,@UsuarioID ,@Fecha, @TipoObjeto ,@Objeto)";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Llave", SqlDbType.VarChar, 100);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TipoObjeto", SqlDbType.VarChar, 100);
                mySqlCommand.Parameters.Add("@Objeto", SqlDbType.VarBinary, maxBufferSize);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Llave"].Value = origen;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now;
                mySqlCommand.Parameters["@TipoObjeto"].Value = objeto.GetType().FullName;
                mySqlCommand.Parameters["@Objeto"].Value = arr;

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplTemporales_Save");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los temporales de un usuario
        /// </summary>
        /// <param name="origen">Origen de los datos</param>
        /// <param name="usuario">Usuario que esta buscando temporales</param>
        public void DAL_aplTemporales_Clean(string origen, DTO_seUsuario usuario)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                    "DELETE FROM aplTemporales WHERE EmpresaID=@EmpresaID AND Llave=@Llave AND UsuarioID=@UsuarioID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.VarChar, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Llave", SqlDbType.VarChar, 100);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Llave"].Value = origen;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;
                
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplTemporales_Clean");
                throw exception;
            }
        }
    }
}
