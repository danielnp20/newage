using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_prOrdenCompraCotiza
    /// </summary>
    public class DAL_prOrdenCompraCotiza : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prOrdenCompraCotiza(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prOrdenCompraCotiza

        #region Funciones publicas

        /// <summary>
        /// Consulta un prOrdenCompraCotiza segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cotizaciones</returns>
        public List<DTO_prOrdenCompraCotiza> DAL_prOrdenCompraCotiza_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prOrdenCompraCotiza with(nolock) where prOrdenCompraCotiza.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prOrdenCompraCotiza> list = new List<DTO_prOrdenCompraCotiza>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prOrdenCompraCotiza detail = new DTO_prOrdenCompraCotiza(dr);
                    list.Add(detail);
                    index++;
                }
                dr.Close();
                return list;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prOrdenCompraCotiza segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cotizaciones</returns>
        public DTO_prOrdenCompraCotiza DAL_prOrdenCompraCotiza_GetByConsecutivo(int consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prOrdenCompraCotiza with(nolock) where prOrdenCompraCotiza.Consecutivo = @Consecutivo ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = consec;

                DTO_prOrdenCompraCotiza list = new DTO_prOrdenCompraCotiza();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    list = new DTO_prOrdenCompraCotiza(dr);
                }
                dr.Close();
                return list;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla prSolicitudCargos
        /// </summary>
        /// <param name="list">Cotizacion</param>
        public void DAL_prOrdenCompraCotiza_Add(List<DTO_prOrdenCompraCotiza> list)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prOrdenCompraCotiza " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,FechaCotiza " +
                                           "    ,Direccion " +
                                           "    ,Telefono " +
                                           "    ,Observacion) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@FechaCotiza " +
                                           "    ,@Direccion " +
                                           "    ,@Telefono " +
                                           "    ,@Observacion) "+
                                           " SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char,50);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaCotiza", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Direccion", SqlDbType.Char,50);
                mySqlCommand.Parameters.Add("@Telefono", SqlDbType.Char,20);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                foreach (DTO_prOrdenCompraCotiza det in list)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = det.EmpresaID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@FechaCotiza"].Value = det.FechaCotiza.Value;
                    mySqlCommand.Parameters["@Direccion"].Value = det.Direccion.Value;
                    mySqlCommand.Parameters["@Telefono"].Value = det.Telefono.Value;
                    mySqlCommand.Parameters["@Observacion"].Value = det.Observacion.Value;
                    //mySqlCommand.Parameters["@Consecutivo"].Value = det.Consecutivo.Value;
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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla DAL_prOrdenCompraCotiza
        /// </summary>
        /// <param name="list">Cotizacion</param>
        public void DAL_prOrdenCompraCotiza_AddItem(DTO_prOrdenCompraCotiza list)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prOrdenCompraCotiza " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,FechaCotiza " +
                                           "    ,Observacion) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@FechaCotiza " +
                                           "    ,@Observacion) " +
                                           " SET @Consecutivo = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaCotiza", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = list.EmpresaID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = list.NumeroDoc.Value;
                mySqlCommand.Parameters["@FechaCotiza"].Value = list.FechaCotiza.Value;
                mySqlCommand.Parameters["@Observacion"].Value = list.Observacion.Value;
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
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar el registro en tabla prOrdenCompraCotiza y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_prOrdenCompraCotiza_Upd(DTO_prOrdenCompraCotiza cotiza)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
               
                //Actualiza Tabla prOrdenCompraCotiza
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prOrdenCompraCotiza " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,NumeroDoc = @NumeroDoc " +
                                           "    ,FechaCotiza = @FechaCotiza " +
                                           "    ,Observacion = @Observacion " +
                                           "    WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaCotiza", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = cotiza.EmpresaID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = cotiza.NumeroDoc.Value;
                mySqlCommand.Parameters["@FechaCotiza"].Value = cotiza.FechaCotiza.Value;
                mySqlCommand.Parameters["@Observacion"].Value = cotiza.Observacion.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = cotiza.Consecutivo.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prSolicitudCargos
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prOrdenCompraCotiza_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prOrdenCompraCotiza where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_Delete");
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
