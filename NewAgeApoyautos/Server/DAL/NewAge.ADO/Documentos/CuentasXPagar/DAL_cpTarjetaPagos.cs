using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_cpTarjetaPagos : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_cpTarjetaPagos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Consulta una cuenta por pagar segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public List<DTO_cpTarjetaPagos> DAL_cpTarjetaPagos_Get(int NumeroDoc)
        {
            try
            {
                List<DTO_cpTarjetaPagos> result = new List<DTO_cpTarjetaPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from cpTarjetaPagos with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                int index = 0;

                while (dr.Read())
                {
                    DTO_cpTarjetaPagos fisico = new DTO_cpTarjetaPagos(dr);
                    result.Add(fisico);
                    index++;
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaPagos_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega tarjeta
        /// </summary>
        /// <param name="listTarjetaPago">informacion del tarjeta</param>
        /// <returns></returns>
        public void DAL_cpTarjetaPagos_Add(DTO_cpTarjetaPagos listTarjetaPago)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                               "   INSERT INTO [cpTarjetaPagos]    " +
                               "    ([NumeroDoc]    " +
                               "    ,[CargoEspecialID]    " +
                               "    ,[Valor]    " +
                               "    ,[eg_cpCargoEspecial])    " +
                               "    VALUES    " +
                               "    (  @NumeroDoc  " +
                               "    ,  @CargoEspecialID  " +
                               "    ,  @Valor  " +
                               "    ,  @eg_cpCargoEspecial)  ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CargoEspecialID", SqlDbType.Char, UDT_CargoEspecialID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_cpCargoEspecial", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = listTarjetaPago.NumeroDoc.Value;
                mySqlCommand.Parameters["@CargoEspecialID"].Value = listTarjetaPago.CargoEspecialID.Value;
                mySqlCommand.Parameters["@Valor"].Value = listTarjetaPago.Valor.Value;
                mySqlCommand.Parameters["@eg_cpCargoEspecial"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpCargoEspecial, this.Empresa, egCtrl);
               
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
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaPagos_Add");
                throw exception;                
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de inFisicoInventario
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_cpTarjetaPagos_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM cpTarjetaPagos where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cpTarjetaPagos_Delete");
                throw exception;
            }
        }

    }
      
}
