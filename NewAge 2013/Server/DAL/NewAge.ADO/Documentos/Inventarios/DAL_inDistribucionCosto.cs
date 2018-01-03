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

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_inDistribucionCosto
    /// </summary>
    public class DAL_inDistribucionCosto : DAL_Base
    {

       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_inDistribucionCosto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Trae informacion de acuerdo al filtrom
        /// </summary>
        /// <param name="fisicoInventario"></param>
        /// <returns>Lista de items de inventario fisico</returns>
        public List<DTO_inDistribucionCosto> DAL_inDistribucionCosto_GetByNumeroDoc(int numeroDoc, bool byNroDocFact)
        {
            try
            {
                List<DTO_inDistribucionCosto> result = new List<DTO_inDistribucionCosto>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query =  string.Empty;

                if (byNroDocFact)
                {
                    query += "where  NumeroDocCto = @NumeroDocCto ";
                    mySqlCommand.Parameters.Add("@NumeroDocCto", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDocCto"].Value = numeroDoc;
                }
                else
                {
                    query += "where  NumeroDocINV = @NumeroDocINV ";
                    mySqlCommand.Parameters.Add("@NumeroDocINV", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDocINV"].Value = numeroDoc;
                }

                mySqlCommand.CommandText = "select * from inDistribucionCosto with(nolock) " +  query;
       
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_inDistribucionCosto costos = new DTO_inDistribucionCosto(dr);
                    result.Add(costos);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inDistribucionCosto_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla inFisicoInventario
        /// </summary>
        /// <param name="listCostos">items a agregar a inv fisico</param>
        /// <returns>Numero Doc</returns>
        public void DAL_inDistribucionCosto_Add(List<DTO_inDistribucionCosto> listCostos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = " INSERT INTO inDistribucionCosto " +
                                           "(NumeroDocINV " +
                                           ",NumeroDocCto " +
                                           ",Valor)" +
                                           "VALUES" +
                                           "(@NumeroDocINV " +
                                           ",@NumeroDocCto " +
                                           ",@Valor)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDocINV", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocCto", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                #endregion
                foreach (DTO_inDistribucionCosto det in listCostos)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@NumeroDocINV"].Value = det.NumeroDocINV.Value.Value;
                    mySqlCommand.Parameters["@NumeroDocCto"].Value = det.NumeroDocCto.Value.Value;
                    mySqlCommand.Parameters["@Valor"].Value = det.Valor.Value.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inDistribucionCosto_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Elimina registros de la tabla de inFisicoInventario
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_inDistribucionCosto_Delete(int numeroDoc, bool byNroDocFact = false)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                if (byNroDocFact)
                {
                    query += "where  NumeroDocCto = @NumeroDocCto ";
                    mySqlCommandSel.Parameters.Add("@NumeroDocCto", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@NumeroDocCto"].Value = numeroDoc;
                }
                else
                {
                    query += "where  NumeroDocINV = @NumeroDocINV ";
                    mySqlCommandSel.Parameters.Add("@NumeroDocINV", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@NumeroDocINV"].Value = numeroDoc;
                }

                mySqlCommandSel.CommandText = "DELETE FROM inDistribucionCosto " + query;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_inDistribucionCosto_Delete");
                throw exception;
            }
        }

        #endregion
    }
}
