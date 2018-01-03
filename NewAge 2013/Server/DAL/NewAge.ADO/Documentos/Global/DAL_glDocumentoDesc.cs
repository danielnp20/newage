using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_glDocumentoDesc
    /// </summary>
    public class DAL_glDocumentoDesc : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glDocumentoDesc(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }

        /// <summary>
        /// Trae el padre de un documento (Si existe)
        /// </summary>
        /// <param name="numeroDoc">Numero de documento hijo</param>
        public bool DAL_glDocumentoDesc_HasParent(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT COUNT(*) FROM glDocumentoDesc with(nolock) WHERE NumeroDocHijo=@NumeroDoc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_glDocumentoDesc_HasParent", false);
                throw ex;
            }
        }

        /// <summary>
        /// Trae la lista de hijos de un documento
        /// </summary>
        /// <param name="numeroDoc">Numero de documento padre</param>
        public List<int> DAL_glDocumentoDesc_GetChilds(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM glDocumentoDesc with(nolock) WHERE NumeroDoc=@NumeroDoc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                List<int> childs = new List<int>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int child = Convert.ToInt32(dr["NumeroDocHijo"]);
                    childs.Add(child);
                }
                dr.Close();

                return childs;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_glDocumentoDesc_GetChilds", false);
                throw ex;
            }
        }

        /// <summary>
        /// Calcula la cantidad de "hijos" con una estado especifico
        /// </summary>
        /// <param name="numeroDoc">Numero de documento padre</param>
        /// <param name="estado">Estado del documento del hijo que se desa filtrar</param>
        public int DAL_glDocumentoDesc_GetApproveChilds(int numeroDoc, int estado)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.Int);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Estado"].Value = estado;

                mySqlCommand.CommandText = "SELECT COUNT(*) "+ 
                                           "FROM glDocumentoDesc glDocDesc with(nolock) "+
                                           "	INNER JOIN glDocumentoControl glDocCtrl with(nolock) ON glDocDesc.NumeroDocHijo = glDocCtrl.NumeroDoc "+ 
                                           "		AND glDocCtrl.Estado = @Estado "+
                                           "WHERE glDocDesc.NumeroDoc=@NumeroDoc";

                int approved = (int) mySqlCommand.ExecuteScalar();
                return approved;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_glDocumentoDesc_GetChilds", false);
                throw ex;
            }
        }

    }
}
