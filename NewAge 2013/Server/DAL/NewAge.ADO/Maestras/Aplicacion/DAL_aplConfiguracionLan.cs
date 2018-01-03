using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Comunicacion;
using NewAge.DTO.Recursos;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Negocio;

namespace NewAge.ADO
{
    public class DAL_aplConfiguracionLan : DAL_Base
    {
       
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplConfiguracionLan(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la configuración de una LAN
        /// </summary>
        /// <param name="lan">Nombre de la LAN</param>
        /// <returns>Retorna la configuracion de una LAN</returns>
        public DTO_aplConfiguracionLan GetConfigByLanName(string lan)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplConfiguracionLan WHERE LanNombre=@LanNombre";

                mySqlCommand.Parameters.Add("LanNombre", System.Data.SqlDbType.NVarChar, 50);
                mySqlCommand.Parameters["LanNombre"].Value = lan;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                DTO_aplConfiguracionLan config = null;

                while (dr.Read())
                {
                    config = new DTO_aplConfiguracionLan(dr);
                }
                dr.Close();

                return config;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_aplConfiguracionLAN_GetConfigByLanName", false);
                throw exception;
            }
        }

        /// <summary>
        /// Trae todas las configuraciones de LAN
        /// </summary>
        /// <returns>Retorna la lista de LANs y sus configuraciones</returns>
        public List<DTO_aplConfiguracionLan> GetLanConfigs()
        {
            try
            {
                List<DTO_aplConfiguracionLan> list = new List<DTO_aplConfiguracionLan>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplConfiguracionLan ORDER BY LanNombre";
                
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                DTO_aplConfiguracionLan config = null;

                while (dr.Read())
                {
                    config = new DTO_aplConfiguracionLan(dr);
                    list.Add(config);
                }
                dr.Close();

                return list;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_aplConfiguracionLAN_GetLanConfigs", false);
                throw exception;
            }
        }

        #endregion
    }
}
