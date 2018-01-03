using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_glDocumentoAprueba
    /// </summary>
    public class DAL_glDocumentoAprueba : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glDocumentoAprueba(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_glDocumentoAprueba  DAL_glDocumentoAprueba_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from glDocumentoAprueba with(nolock) where NumeroDoc = @NumeroDoc  and EmpresaID = @EmpresaID";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                DTO_glDocumentoAprueba result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_glDocumentoAprueba(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoAprueba_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla glDocumentoAprueba 
        /// </summary>
        /// <param name="sol">Orden de Compra</param>
        /// <returns></returns>
        public void DAL_glDocumentoAprueba_Add(DTO_glDocumentoAprueba docAprueba)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =  "    INSERT INTO glDocumentoAprueba " +
                                            "    (EmpresaID " +
                                            "    ,NumeroDoc " +
                                            "    ,UsuarioAprueba " +
                                            "    ,UsuarioAprueba1 " +
                                            "    ,UsuarioAprueba2 " +
                                            "    ,UsuarioAprueba3 " +
                                            "    ,UsuarioAprueba4 " +
                                            "    ,UsuarioAprueba5 " +
                                            "    ,UsuarioAprueba6 " +
                                            "    ,UsuarioAprueba7 " +
                                            "    ,UsuarioAprueba8 " +
                                            "    ,UsuarioAprueba9 " +
                                            "    ,UsuarioAprueba10 " +
                                            "    ,FechaAprueba1 " +
                                            "    ,FechaAprueba2 " +
                                            "    ,FechaAprueba3 " +
                                            "    ,FechaAprueba4 " +
                                            "    ,FechaAprueba5 " +
                                            "    ,FechaAprueba6 " +
                                            "    ,FechaAprueba7 " +
                                            "    ,FechaAprueba8 " +
                                            "    ,FechaAprueba9 " +
                                            "    ,FechaAprueba10) " +
                                            "    VALUES" +
                                            "    (@EmpresaID " +
                                            "    ,@NumeroDoc " +
                                            "    ,@UsuarioAprueba " +
                                            "    ,@UsuarioAprueba1 " +
                                            "    ,@UsuarioAprueba2 " +
                                            "    ,@UsuarioAprueba3 " +
                                            "    ,@UsuarioAprueba4 " +
                                            "    ,@UsuarioAprueba5 " +
                                            "    ,@UsuarioAprueba6 " +
                                            "    ,@UsuarioAprueba7 " +
                                            "    ,@UsuarioAprueba8 " +
                                            "    ,@UsuarioAprueba9 " +
                                            "    ,@UsuarioAprueba10 " +
                                            "    ,@FechaAprueba1 " +
                                            "    ,@FechaAprueba2 " +
                                            "    ,@FechaAprueba3 " +
                                            "    ,@FechaAprueba4 " +
                                            "    ,@FechaAprueba5 " +
                                            "    ,@FechaAprueba6 " +
                                            "    ,@FechaAprueba7 " +
                                            "    ,@FechaAprueba8 " +
                                            "    ,@FechaAprueba9 " +
                                            "    ,@FechaAprueba10) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba5", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba6", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba7", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba8", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba9", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba10", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaAprueba1", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba2", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba3", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba4", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba5", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba6", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba7", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba8", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba9", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba10", SqlDbType.DateTime);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = docAprueba.NumeroDoc.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = docAprueba.UsuarioAprueba.Value;
                mySqlCommand.Parameters["@UsuarioAprueba1"].Value = docAprueba.UsuarioAprueba1.Value;
                mySqlCommand.Parameters["@UsuarioAprueba2"].Value = docAprueba.UsuarioAprueba2.Value;
                mySqlCommand.Parameters["@UsuarioAprueba3"].Value = docAprueba.UsuarioAprueba3.Value;
                mySqlCommand.Parameters["@UsuarioAprueba4"].Value = docAprueba.UsuarioAprueba4.Value;
                mySqlCommand.Parameters["@UsuarioAprueba5"].Value = docAprueba.UsuarioAprueba5.Value;
                mySqlCommand.Parameters["@UsuarioAprueba6"].Value = docAprueba.UsuarioAprueba6.Value;
                mySqlCommand.Parameters["@UsuarioAprueba7"].Value = docAprueba.UsuarioAprueba7.Value;
                mySqlCommand.Parameters["@UsuarioAprueba8"].Value = docAprueba.UsuarioAprueba8.Value;
                mySqlCommand.Parameters["@UsuarioAprueba9"].Value = docAprueba.UsuarioAprueba9.Value;
                mySqlCommand.Parameters["@UsuarioAprueba10"].Value = docAprueba.UsuarioAprueba10.Value;
                mySqlCommand.Parameters["@FechaAprueba1"].Value = docAprueba.FechaAprueba1.Value;
                mySqlCommand.Parameters["@FechaAprueba2"].Value = docAprueba.FechaAprueba2.Value;
                mySqlCommand.Parameters["@FechaAprueba3"].Value = docAprueba.FechaAprueba3.Value;
                mySqlCommand.Parameters["@FechaAprueba4"].Value = docAprueba.FechaAprueba4.Value;
                mySqlCommand.Parameters["@FechaAprueba5"].Value = docAprueba.FechaAprueba5.Value;
                mySqlCommand.Parameters["@FechaAprueba6"].Value = docAprueba.FechaAprueba6.Value;
                mySqlCommand.Parameters["@FechaAprueba7"].Value = docAprueba.FechaAprueba7.Value;
                mySqlCommand.Parameters["@FechaAprueba8"].Value = docAprueba.FechaAprueba8.Value;
                mySqlCommand.Parameters["@FechaAprueba9"].Value = docAprueba.FechaAprueba9.Value;
                mySqlCommand.Parameters["@FechaAprueba10"].Value = docAprueba.FechaAprueba10.Value;
                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoAprueba_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar el registro en tabla DTO_glDocumentoAprueba y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_glDocumentoAprueba_Upd(DTO_glDocumentoAprueba docAprueba)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla DTO_glDocumentoAprueba
                #region CommandText
                mySqlCommand.CommandText =  "    UPDATE glDocumentoAprueba " +
                                            "    SET EmpresaID  = @EmpresaID  " +
                                            "    ,UsuarioAprueba = @UsuarioAprueba" +
                                            "    ,UsuarioAprueba1 = @UsuarioAprueba1" +
                                            "    ,UsuarioAprueba2 = @UsuarioAprueba2" +
                                            "    ,UsuarioAprueba3 = @UsuarioAprueba3" +
                                            "    ,UsuarioAprueba4 = @UsuarioAprueba4" +
                                            "    ,UsuarioAprueba5 = @UsuarioAprueba5" +
                                            "    ,UsuarioAprueba6 = @UsuarioAprueba6" +
                                            "    ,UsuarioAprueba7 = @UsuarioAprueba7" +
                                            "    ,UsuarioAprueba8 = @UsuarioAprueba8" +
                                            "    ,UsuarioAprueba9 = @UsuarioAprueba9" +
                                            "    ,UsuarioAprueba10 = @UsuarioAprueba10" +
                                            "    ,FechaAprueba1 = @FechaAprueba1" +
                                            "    ,FechaAprueba2 = @FechaAprueba2" +
                                            "    ,FechaAprueba3 = @FechaAprueba3" +
                                            "    ,FechaAprueba4 = @FechaAprueba4" +
                                            "    ,FechaAprueba5 = @FechaAprueba5" +
                                            "    ,FechaAprueba6 = @FechaAprueba6" +
                                            "    ,FechaAprueba7 = @FechaAprueba7" +
                                            "    ,FechaAprueba8 = @FechaAprueba8" +
                                            "    ,FechaAprueba9 = @FechaAprueba9" +
                                            "    ,FechaAprueba10 = @FechaAprueba10" +                      
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba3", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba4", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba5", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba6", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba7", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba8", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba9", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@UsuarioAprueba10", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaAprueba1", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba2", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba3", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba4", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba5", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba6", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba7", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba8", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba9", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaAprueba10", SqlDbType.DateTime);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = docAprueba.NumeroDoc.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = docAprueba.UsuarioAprueba.Value;
                mySqlCommand.Parameters["@UsuarioAprueba1"].Value = docAprueba.UsuarioAprueba1.Value;
                mySqlCommand.Parameters["@UsuarioAprueba2"].Value = docAprueba.UsuarioAprueba2.Value;
                mySqlCommand.Parameters["@UsuarioAprueba3"].Value = docAprueba.UsuarioAprueba3.Value;
                mySqlCommand.Parameters["@UsuarioAprueba4"].Value = docAprueba.UsuarioAprueba4.Value;
                mySqlCommand.Parameters["@UsuarioAprueba5"].Value = docAprueba.UsuarioAprueba5.Value;
                mySqlCommand.Parameters["@UsuarioAprueba6"].Value = docAprueba.UsuarioAprueba6.Value;
                mySqlCommand.Parameters["@UsuarioAprueba7"].Value = docAprueba.UsuarioAprueba7.Value;
                mySqlCommand.Parameters["@UsuarioAprueba8"].Value = docAprueba.UsuarioAprueba8.Value;
                mySqlCommand.Parameters["@UsuarioAprueba9"].Value = docAprueba.UsuarioAprueba9.Value;
                mySqlCommand.Parameters["@UsuarioAprueba10"].Value = docAprueba.UsuarioAprueba10.Value;
                mySqlCommand.Parameters["@FechaAprueba1"].Value = docAprueba.FechaAprueba1.Value;
                mySqlCommand.Parameters["@FechaAprueba2"].Value = docAprueba.FechaAprueba2.Value;
                mySqlCommand.Parameters["@FechaAprueba3"].Value = docAprueba.FechaAprueba3.Value;
                mySqlCommand.Parameters["@FechaAprueba4"].Value = docAprueba.FechaAprueba4.Value;
                mySqlCommand.Parameters["@FechaAprueba5"].Value = docAprueba.FechaAprueba5.Value;
                mySqlCommand.Parameters["@FechaAprueba6"].Value = docAprueba.FechaAprueba6.Value;
                mySqlCommand.Parameters["@FechaAprueba7"].Value = docAprueba.FechaAprueba7.Value;
                mySqlCommand.Parameters["@FechaAprueba8"].Value = docAprueba.FechaAprueba8.Value;
                mySqlCommand.Parameters["@FechaAprueba9"].Value = docAprueba.FechaAprueba9.Value;
                mySqlCommand.Parameters["@FechaAprueba10"].Value = docAprueba.FechaAprueba10.Value;
                #endregion
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }                
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoAprueba_Upd");
                throw exception;
            }
        }
        #endregion
    }
}
