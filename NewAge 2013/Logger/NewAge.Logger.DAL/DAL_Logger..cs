using NewAge.DTO.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAge.Logger.DAL
{
    public class DAL_Logger
    {
        #region Variables

        /// <summary>
        /// Instancia única del base controller
        /// </summary>
        private static DAL_Logger _instance;

        /// <summary>
        /// Numero de referencias del controlador base
        /// </summary>
        private static int _numOfReference;

        #endregion

        #region Propiedades

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        protected SqlConnection MySqlConnection
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        private DAL_Logger(string connStr)
        {
            if (this.MySqlConnection == null || this.MySqlConnection.State == ConnectionState.Broken || this.MySqlConnection.State == ConnectionState.Closed)
            {
                this.MySqlConnection = new SqlConnection(connStr);
                this.MySqlConnection.Open();
            }
        }

        /// <summary>
        /// Obtiene la única instancia del controlador base
        /// </summary>
        /// <returns>Retorna la única instancia del controlador base</returns>
        public static DAL_Logger GetInstance(string connStr)
        {
            try
            {
                if (_instance == null)
                    _instance = new DAL_Logger(connStr);

                _numOfReference++;
                return _instance;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region LogErrors

        #region CRUD

        /// <summary>
        /// Agrega un registro al logger
        /// </summary>
        public void LogErrors_Add(Error error)
        {
            try
            {
                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO LogErrors(Date,MachineName,LoginName,MethodName,MessageText,Context)" + 
                    "VALUES(@Date,@MachineName,@LoginName,@MethodName,@MessageText,@Context)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Date", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@MachineName", SqlDbType.NVarChar);
                mySqlCommand.Parameters.Add("@LoginName", SqlDbType.NVarChar);
                mySqlCommand.Parameters.Add("@MethodName", SqlDbType.NVarChar);
                mySqlCommand.Parameters.Add("@MessageText", SqlDbType.NVarChar);
                mySqlCommand.Parameters.Add("@Context", SqlDbType.NVarChar);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@Date"].Value = error.Date;
                mySqlCommand.Parameters["@MachineName"].Value = error.MachineName;
                mySqlCommand.Parameters["@LoginName"].Value = error.LoginName;
                mySqlCommand.Parameters["@MethodName"].Value = error.MethodName;
                mySqlCommand.Parameters["@MessageText"].Value = error.MessageText;
                mySqlCommand.Parameters["@Context"].Value = error.Context;
                #endregion

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #endregion
    }
}
