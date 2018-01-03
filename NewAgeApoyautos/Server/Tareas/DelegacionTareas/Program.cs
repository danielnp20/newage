using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using NewAge.Negocio;
using NewAge.DTO.Negocio;

namespace DelegacionTareas
{
    class Program
    {
        #region DBConnection

        /// <summary>
        /// Cadena de conexion a la bd del logger
        /// </summary>
        private static string loggerCon;
        
        /// <summary>
        /// Get or sets the connection
        /// </summary>
        private static SqlConnection _mySqlConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        private static void ADO_ConnectDB()
        {
            try
            {
                if (_mySqlConnection.State == ConnectionState.Broken || _mySqlConnection.State == ConnectionState.Closed)
                {
                    _mySqlConnection.Open();
                }
            }
            catch
            {
                ADO_CloseDBConnection();
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        public static void ADO_CloseDBConnection()
        {
            try
            {
                if (_mySqlConnection.State != ConnectionState.Closed)
                {
                    _mySqlConnection.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        static void Main(string[] args)
        {
            try
            {
                InitData();
                ModuloFachada facade = new ModuloFachada();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, _mySqlConnection, null, null, 0, loggerCon);

                mod.seDelegacionHistoria_Activar();
                mod.seDelegacionHistoria_Desactivar();
            }
            catch (Exception ex1)
            { ; }
        }

        /// <summary>
        /// Inicializa las variables 
        /// </summary>
        private static void InitData()
        {
            //Conexion a BD
            loggerCon = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString();
            _mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString());
            ADO_ConnectDB();
        }
    }
}
