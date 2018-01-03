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
    /// DAL de DAL_prContratoPlanPago
    /// </summary>
    public class DAL_prContratoPlanPago : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prContratoPlanPago(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prContratoPlanPago

        #region Funciones publicas

        /// <summary>
        /// Adiciona el registro en tabla prSaldosDocu
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_prContratoPlanPago_Add(DTO_prContratoPlanPago planPago)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prContratoPlanPago " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,Fecha " +
                                           "    ,Valor " + 
                                           "    ,ValorAdicional " +
                                           "    ,Observacion ) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@Fecha " +
                                           "    ,@Valor " +
                                           "    ,@ValorAdicional " +
                                           "    ,@Observacion) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorAdicional", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = planPago.NumeroDoc.Value;
                mySqlCommand.Parameters["@Fecha"].Value = planPago.Fecha.Value;
                mySqlCommand.Parameters["@Valor"].Value = planPago.Valor.Value;
                mySqlCommand.Parameters["@ValorAdicional"].Value = planPago.ValorAdicional.Value;
                mySqlCommand.Parameters["@Observacion"].Value = planPago.Observacion.Value; 
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPlanPago_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro en tabla prSaldosDocu
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_prContratoPlanPago_Upd(DTO_prContratoPlanPago planPago)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prContratoPlanPago " +
                                           "    SET Fecha = @Fecha,Valor = @Valor,ValorAdicional = @ValorAdicional,Observacion = @Observacion" +
                                           "    WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorAdicional", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@Consecutivo"].Value = planPago.Consecutivo.Value;
                mySqlCommand.Parameters["@Fecha"].Value = planPago.Fecha.Value;
                mySqlCommand.Parameters["@Valor"].Value = planPago.Valor.Value;
                mySqlCommand.Parameters["@ValorAdicional"].Value = planPago.ValorAdicional.Value;
                mySqlCommand.Parameters["@Observacion"].Value = planPago.Observacion.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPlanPago_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prSaldosDocu segun el ConsecutivoDetaID
        /// </summary>
        /// <param name="NumeroDoc">ID del registro del detalle</param>
        /// <returns>lista de prSaldosDocu</returns>
        public  List<DTO_prContratoPlanPago> DAL_prContratoPlanPago_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prContratoPlanPago with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prContratoPlanPago> list = new List<DTO_prContratoPlanPago>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prContratoPlanPago detail = new DTO_prContratoPlanPago(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPlanPago_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prContratoPlanPago
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prContratoPlanPago_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prContratoPlanPago where EmpresaID = @EmpresaID " +
                " and NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoPlanPago_Delete");
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
