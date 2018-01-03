using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de glEmpresa
    /// </summary>
    public class DAL_glEmpresa : DAL_Base
    {
        /// <summary>
        /// Documento de la maestra
        /// </summary>
        private int _documentId = AppMasters.glEmpresa;

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glEmpresa(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la imagen del logo de la empresa
        /// </summary>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] DAL_glEmpresaLogo()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT Logo FROM glEmpresa " +
                  "WHERE EmpresaID = @EmpresaID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, 10);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                const int bufferSize = 1000000;

                byte[] logo = new byte[bufferSize];

                while (dr.Read())
                {
                    try
                    { dr.GetBytes(0, 0, logo, 0, bufferSize); }
                    catch (Exception)
                    { ; } 
                }
                dr.Close();

                return logo;
            }
            catch (Exception ex)
            {

                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glEmpresa_DAL_glEmpresaLogo");
                throw exception;
            }
        }

       #endregion

    }
}
