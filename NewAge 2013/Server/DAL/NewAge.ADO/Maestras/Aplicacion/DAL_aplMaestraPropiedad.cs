using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_aplMaestraPropiedad : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplMaestraPropiedad(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Retorna todas la lista de paramestros para las maestras
        /// </summary>
        /// <returns>Lista de MasterParams</returns>
        public IEnumerable<DTO_aplMaestraPropiedades> DAL_aplMaestraPropiedades_GetAll()
        {
            try
            {
                List<DTO_aplMaestraPropiedades> maestras = new List<DTO_aplMaestraPropiedades>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                  "SELECT * FROM aplMaestraPropiedad ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_aplMaestraPropiedades tabla = new DTO_aplMaestraPropiedades(dr);
                    maestras.Add(tabla);
                }
                dr.Close();

                maestras.ForEach(m =>
                {
                    mySqlCommand.CommandText =
                        "SELECT * FROM aplMaestraCampo where DocumentoID = @DocumentoID ORDER BY ColumnaPosicion";

                    if(!mySqlCommand.Parameters.Contains("@DocumentoID"))
                    {
                        mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    }
                    mySqlCommand.Parameters["@DocumentoID"].Value = m.DocumentoID;

                    //Agrega los campos extras 
                    dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        DTO_aplMaestraCampo campo=null;
                        if (Convert.ToBoolean(dr["FKInd"])==true)
                        {
                            campo = new ForeignKeyField(dr);
                        }
                        else{
                            campo = new DTO_aplMaestraCampo(dr);
                        }
                        m.Campos.Add(campo);
                    }
                    dr.Close();
                });

                return maestras;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplMaestraPropiedad_DAL_aplMaestraPropiedades_GetAll");
                throw exception;
            }
        }


    }
}
