using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_aplModulo : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplModulo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los aplModulo 
        /// </summary>
        /// <returns>Enumeracion de aplModulo</returns>
        public IEnumerable<DTO_aplModulo> DAL_aplModulo_GetAll()
        {
            try
            {
                List<DTO_aplModulo> modulos = new List<DTO_aplModulo>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplModulo ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var modulo = new DTO_aplModulo(dr);

                    modulos.Add(modulo);
                }
                dr.Close();

                return modulos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplModulo_DAL_aplModulo_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna todos los aplModulo que tiene estado activo
        /// </summary>
        /// <param name="active">activo</param>
        /// <returns></returns>
        public IEnumerable<DTO_aplModulo> DAL_aplModulo_GetByVisible(short active)
        {
            try
            {
                List<DTO_aplModulo> modulos = new List<DTO_aplModulo>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select distinct * from " +
                    "( " +
                    "	select mo.* " +
                    "	from glEmpresaModulo em with(nolock) " +
                    "		inner join aplModulo mo with(nolock) on em.ModuloID = mo.ModuloID and mo.ActivoInd = @ActivoInd " +
                    "	where em.ActivoInd = @ActivoInd and EmpresaID = @EmpresaID and em.ModuloID <> 'dr' " +
                    "	Union all " +
                    "	SELECT * FROM aplModulo  with(nolock) " +
                    "	WHERE ModuloID in ('apl','gl', 'se') " +
                    ") as m " +
                    "ORDER BY m.ModuloID";

                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@ActivoInd"].Value = active;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var modulo = new DTO_aplModulo(dr);
                    modulos.Add(modulo);
                }
                dr.Close();

                return modulos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplModulo_DAL_aplModulo_GetByVisible");
                throw exception;
            }
        }     
       
    }
}
