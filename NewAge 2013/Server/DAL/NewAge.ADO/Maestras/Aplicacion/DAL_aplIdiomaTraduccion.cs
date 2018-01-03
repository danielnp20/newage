using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_aplIdiomaTraduccion : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplIdiomaTraduccion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los aplIdiomaTraduccion
        /// </summary>
        /// <returns>Lista de aplIdiomaTraduccion</returns>
        public DTO_aplIdiomaTraduccion DAL_aplIdiomaTraduccion_GetById(string idiomaId, string llave)
        {
            try
            {
                DTO_aplIdiomaTraduccion reg = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplIdiomaTraduccion WHERE IdiomaID = @IdiomaID and Llave = @Llave ";

                mySqlCommand.Parameters.Add("@IdiomaID", SqlDbType.Char, 3);
                mySqlCommand.Parameters.Add("@Llave", SqlDbType.Char, 200);
                mySqlCommand.Parameters["@IdiomaID"].Value = idiomaId;
                mySqlCommand.Parameters["@Llave"].Value = llave;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    reg = new DTO_aplIdiomaTraduccion(dr);
                }
                dr.Close();

                return reg;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplIdiomaTraduccion_DAL_aplIdiomaTraduccion_GetByIdiomaId");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los aplIdiomaTraduccion
        /// </summary>
        /// <returns>Lista de aplIdiomaTraduccion</returns>
        public IEnumerable<DTO_aplIdiomaTraduccion> DAL_aplIdiomaTraduccion_GetByIdiomaId(string idiomaId)
        {
            try
            {
                List<DTO_aplIdiomaTraduccion> idiomas = new List<DTO_aplIdiomaTraduccion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplIdiomaTraduccion WHERE IdiomaID = @IdiomaID ";

                mySqlCommand.Parameters.Add("@IdiomaID", SqlDbType.Char, 3);
                mySqlCommand.Parameters["@IdiomaID"].Value = idiomaId;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var idioma = new DTO_aplIdiomaTraduccion(dr);

                    idiomas.Add(idioma);
                }
                dr.Close();

                return idiomas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplIdiomaTraduccion_DAL_aplIdiomaTraduccion_GetByIdiomaId");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los aplIdiomaTraduccion
        /// </summary>
        /// <returns>Lista de aplIdiomaTraduccion</returns>
        public IEnumerable<DTO_aplIdiomaTraduccion> DAL_aplIdiomaTraduccion_GetRsxByKeys(string idiomaId, LanguageTypes rsxType, List<string> keys)
        {
            try
            {
                Int16 rsxTipo = (Int16)rsxType;
                string rsxKeys = string.Empty;
               
                for (int i = 0; i < keys.Count; ++i)
                {
                    keys[i] = keys[i].Replace("'", " ");
                    if(i == 0)
                        rsxKeys = "(Llave = '" + keys[i] + "'";
                    else
                        rsxKeys += " OR Llave = '" + keys[i] + "'";
                    
                    if (i == keys.Count - 1)
                        rsxKeys += ")";
                }

                List<DTO_aplIdiomaTraduccion> idiomas = new List<DTO_aplIdiomaTraduccion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplIdiomaTraduccion WHERE IdiomaID = @IdiomaID AND TipoID = @TipoID AND " + rsxKeys;

                mySqlCommand.Parameters.Add("@IdiomaID", SqlDbType.Char, 3);
                mySqlCommand.Parameters["@IdiomaID"].Value = idiomaId;

                mySqlCommand.Parameters.Add("@TipoID", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@TipoID"].Value = rsxTipo;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var idioma = new DTO_aplIdiomaTraduccion(dr);
                    idiomas.Add(idioma);
                }
                dr.Close();

                return idiomas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplIdiomaTraduccion_DAL_aplIdiomaTraduccion_GetRsxByKeys");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un grupo de recurtsos de un mismo tipo
        /// </summary>
        /// <param name="idiomaId">idioma</param>
        /// <param name="rsxType">tipo de recursos</param>
        /// <param name="keys">lista de llaves a consultar</param>
        /// <returns>diccionario con las llaves y los recursos</returns>
        public Dictionary<string, string> DAL_aplIdiomaTraduccion_GetRsxByKeysDict(string idiomaId, LanguageTypes rsxType, List<string> keys)
        {
            IEnumerable<DTO_aplIdiomaTraduccion> rsxs=DAL_aplIdiomaTraduccion_GetRsxByKeys(idiomaId,  rsxType,  keys);
            Dictionary<string, string> res=new Dictionary<string,string>();
            foreach (string key in keys){
                List<DTO_aplIdiomaTraduccion> rx = rsxs.Where(x => x.Llave.Value.Trim().Equals(key)).ToList();
                if (rx.Count == 0)
                    res.Add(key, key);
                else
                {
                    res.Add(key, rx.First().Dato.Value);
                }
            }
            return res;
        }

    }
}
