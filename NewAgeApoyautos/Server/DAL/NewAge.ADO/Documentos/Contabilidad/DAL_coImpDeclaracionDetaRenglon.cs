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

namespace NewAge.ADO
{
    /// <summary>
    /// Operaciones de BD sobre la tabla DAL_coImpDeclaracionDetaCuenta
    /// </summary>
    public class DAL_coImpDeclaracionDetaRenglon : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coImpDeclaracionDetaRenglon(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae tdos los renglones de una declaracion
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        /// <returns></returns>
        public List<DTO_coImpDeclaracionDetaRenglon> DAL_coImpDeclaracionDetaRenglon_Get(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText =
                    "SELECT distinct deta.*, r.Descriptivo " + 
                    "from coImpDeclaracionDetaRenglon deta with(nolock) " +
                    "	left join coImpDeclaracionRenglon r with(nolock) on deta.Renglon = r.Renglon " +
                    "where EmpresaID=@EmpresaID and NumeroDoc=@NumeroDoc " +
                    "order by Renglon";

                List<DTO_coImpDeclaracionDetaRenglon> result = new List<DTO_coImpDeclaracionDetaRenglon>();
                SqlDataReader dr;

                dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coImpDeclaracionDetaRenglon det = new DTO_coImpDeclaracionDetaRenglon(dr);
                    result.Add(det);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDetaRenglon_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="docu"></param>
        public void DAL_coImpDeclaracionDetaRenglon_Add(DTO_coImpDeclaracionDetaRenglon deta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Renglon", SqlDbType.Char, UDT_Renglon.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorAjustado", SqlDbType.Decimal);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value.Value;
                mySqlCommandSel.Parameters["@Renglon"].Value = deta.Renglon.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = deta.Valor.Value.Value;
                mySqlCommandSel.Parameters["@ValorAjustado"].Value = deta.ValorAjustado.Value.Value;

                mySqlCommandSel.CommandText =
                    "INSERT INTO coImpDeclaracionDetaRenglon(" +
                    "	EmpresaID,NumeroDoc,Renglon,Valor,ValorAjustado" +
                    ")VALUES(" +
                    "	@EmpresaID,@NumeroDoc,@Renglon,@Valor,@ValorAjustado" +
                    ")";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDetaRenglon_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los registros de un documento
        /// </summary>
        /// <param name="numeroDoc"></param>
        public void DAL_coImpDeclaracionDetaRenglon_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText =
                    "DELETE from coImpDeclaracionDetaRenglon where EmpresaID=@EmpresaID and NumeroDoc=@NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coImpDeclaracionDetaRenglon_Delete");
                throw exception;
            }
        }

    }
}
