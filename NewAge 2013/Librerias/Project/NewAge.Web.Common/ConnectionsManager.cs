using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAge.Web.Common
{
    /// <summary>
    /// Connections manager class
    /// </summary>
    public static class ConnectionsManager
    {
        /// <summary>
        /// Cadena de conexion
        /// </summary>
        public static string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        private static List<SqlConnection> _mySqlConnections = new List<SqlConnection>();

        /// <summary>
        /// Retorna el indice de una conexion que este disponible
        /// </summary>
        /// <returns>Retorna una conexion disponible</returns>
        private static int GetConnectionIndex()
        {
            int result = -1;
            SqlConnection conn;
            for (int i = 0; i < _mySqlConnections.Count; ++i)
            {
                conn = _mySqlConnections[i];
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    result = i;
                }
            }

            if (result == -1)
            {
                conn = new SqlConnection(ConnectionString);
                _mySqlConnections.Add(conn);
                result = _mySqlConnections.Count - 1;
            }

            return result;
        }

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        /// <returns>Retorna el indice con la conexion que se esta usando</returns>
        public static SqlConnection ADO_ConnectDB()
        {
            int connIndex = -1;
            try
            {
                connIndex = GetConnectionIndex();
                _mySqlConnections[connIndex].Open();
                return _mySqlConnections[connIndex];
            }
            catch
            {
                ADO_CloseDBConnection(connIndex);
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        private static void ADO_CloseDBConnection(int connIndex)
        {
            try
            {
                if (connIndex == -1)
                {
                    _mySqlConnections = new List<SqlConnection>();

                    SqlConnection conn = new SqlConnection(ConnectionString);
                    _mySqlConnections.Add(conn);
                }
                else
                {
                    if (_mySqlConnections[connIndex].State != ConnectionState.Closed)
                        _mySqlConnections[connIndex].Close();
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
                throw;
            }
        }

    }
}
