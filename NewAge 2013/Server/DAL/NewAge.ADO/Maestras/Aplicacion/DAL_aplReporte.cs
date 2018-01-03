using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Reportes;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NewAge.ADO
{
    public class DAL_aplReporte : DAL_Base
    {
        private int maxBufferSize = 1024 * 1024 * 50;

        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_aplReporte(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene un resporte predefinido para una empresa
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public byte[] DAL_aplReporte_GetByID(int documentoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;

                mySqlCommand.CommandText =
                 "SELECT Data FROM aplReporte with(nolock) where EmpresaID=@EmpresaID and DocumentoID=@DocumentoID ";
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                byte[] objetoSerializado = null;
                if (dr.Read())
                {
                    try
                    {
                        objetoSerializado = new byte[maxBufferSize];
                        dr.GetBytes(0, 0, objetoSerializado, 0, maxBufferSize);
                        MemoryStream memStream = new MemoryStream();
                        BinaryFormatter binForm = new BinaryFormatter();
                        memStream.Write(objetoSerializado, 0, objetoSerializado.Length);
                        memStream.Seek(0, SeekOrigin.Begin);
                    }
                    catch (Exception e)
                    {
                        ;
                    }
                }
                dr.Close();
                return objetoSerializado;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplReporte_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa o actualiza un reporte predefinido para un usuario
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public void DAL_aplReporte_Update(DTO_aplReporte report)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = report.DocumentoID.Value.Value;

                #region Revisa si tiene info
                mySqlCommand.CommandText =
                    "SELECT COUNT(*) FROM aplReporte with(nolock) where EmpresaID=@EmpresaID and DocumentoID=@DocumentoID ";
               
                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());

                #endregion
                #region Ingresa o actualiza el valor

                mySqlCommand.Parameters.Add("@Data", SqlDbType.VarBinary, maxBufferSize);
                mySqlCommand.Parameters["@Data"].Value = report.Data; //arr;

                if(count == 0)
                    mySqlCommand.CommandText =
                        "INSERT INTO aplReporte (EmpresaID, DocumentoID, Data) VALUES(@EmpresaID, @DocumentoID, @Data)";
                else
                    mySqlCommand.CommandText =
                        "UPDATE aplReporte SET Data = @Data where EmpresaID=@EmpresaID and DocumentoID=@DocumentoID ";
                #endregion
                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplReporte_Update");
                throw exception;
            }
        }

    }
}
