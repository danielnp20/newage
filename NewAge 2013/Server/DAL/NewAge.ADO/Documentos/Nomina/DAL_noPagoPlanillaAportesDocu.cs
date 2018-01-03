using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_noPagoPlanillaAportesDocu : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noPagoPlanillaAportesDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Agrega un pago a la tabla noPagoPlanillaAportesDocu
        /// </summary>
        /// <param name="detalle">detalle del pago</param>
        public void DAL_noPagoPlanillaAportesDocu_Add(DTO_noPagoPlanillaAportesDocu detalle)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  INSERT INTO noPagoPlanillaAportesDocu  " +
                                              "   (EmpresaID  " +
                                              "   ,NumeroDoc  " +
                                              "   ,NumeroDocCXP  " +
                                              "   ,Valor  " +
                                              "   ,Iva)  " +
                                              "   VALUES  " +
                                              "   (@EmpresaID,  " +
                                              "   ,@Consecutivo,  " +
                                              "   ,@Consecutivo,  " +
                                              "   ,@Valor,  " +
                                              "   ,@Iva)";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = detalle.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDocCXP"].Value = detalle.NumeroDocCXP.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = detalle.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = detalle.Valor.Value;               

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPagoPlanillaAportesDocu_Add");
                throw exception;
            }        
        }

    }
}
