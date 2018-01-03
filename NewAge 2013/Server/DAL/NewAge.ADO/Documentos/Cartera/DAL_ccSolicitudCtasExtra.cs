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
    public class DAL_ccSolicitudCtasExtra : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudCtasExtra(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega informacion a la tabla DTO_ccSolicitudCtasExtra
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudCtasExtra_Add(DTO_ccSolicitudCtasExtra footer)
        {
            try
            {
                List<DTO_ccSolicitudCtasExtra> result = new List<DTO_ccSolicitudCtasExtra>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
               
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
                //CommandText
                mySqlCommandSel.CommandText = "INSERT INTO ccSolicitudCtasExtra(NumeroDoc,CuotaID,VlrCuota)VALUES(@NumeroDoc,@CuotaID,@VlrCuota)";

                #region Creacion Parametros
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                #endregion
                #region Asiganacion Campos
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value       = footer.NumeroDoc.Value;
                    mySqlCommandSel.Parameters["@CuotaID"].Value = footer.CuotaID.Value;
                    mySqlCommandSel.Parameters["@VlrCuota"].Value = footer.VlrCuota.Value;
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudCtasExtra_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudCtasExtra
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudCtasExtra</returns>
        public List<DTO_ccSolicitudCtasExtra> DAL_ccSolicitudAnexo_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudCtasExtra> result = new List<DTO_ccSolicitudCtasExtra>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccSolicitudCtasExtra with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudCtasExtra cuota = new DTO_ccSolicitudCtasExtra(dr);
                    result.Add(cuota);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudCtasExtra_GetByNumeroDoc");
                throw exception;
            }
        }
       
        /// <summary>
        /// Elimina los anexos asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudCtasExtra_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudCtasExtra WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudCtasExtra_Delete");
                throw exception;
            }
        }

    }
}
