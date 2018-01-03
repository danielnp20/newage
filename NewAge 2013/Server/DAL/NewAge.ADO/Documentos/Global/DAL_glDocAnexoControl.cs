using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_glDocAnexoControl : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glDocAnexoControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Retorna la lista de anexos de un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <returns>Retorna la lista de anexos</returns>
        public List<DTO_glDocAnexoControl> DAL_glDocAnexoControl_GetAnexosByNumeroDoc(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_glDocAnexoControl> result = new List<DTO_glDocAnexoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.Bit);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glDocumentoAnexo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                mySqlCommand.CommandText =
                    "select ctrl.NumeroDoc, anexo.DocumentoID, Descriptivo, ReplicaID as 'ConsReplica', anexoCtrl.ArchivoNombre " +
                    "from glDocumentoAnexo anexo with(nolock) " +
                    "   inner join glDocumentoControl ctrl on anexo.DocumentoID = ctrl.DocumentoID and ctrl.NumeroDoc = @NumeroDoc " + 
                    "	left join glDocAnexoControl anexoCtrl with(nolock) on anexo.ReplicaID = anexoCtrl.ConsReplica and anexoCtrl.NumeroDoc = @NumeroDoc " +
                    "where EmpresaGrupoID = @EmpresaGrupoID and ActivoInd = @ActivoInd ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glDocAnexoControl anexo = new DTO_glDocAnexoControl(dr);
                    result.Add(anexo);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocAnexoControl_GetAnexosByNumeroDoc");
                throw exception;
            }

        }

        /// <summary>
        /// Retorna la lista de anexos de un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <returns>Retorna la lista de anexos</returns>
        public DTO_glDocAnexoControl DAL_glDocAnexoControl_GetAnexosByReplica(int replica)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                DTO_glDocAnexoControl result = new DTO_glDocAnexoControl();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ConsReplica", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsReplica"].Value = replica;

                mySqlCommand.CommandText =
                    "select Top 1 glDocAnexoControl from glDocAnexoControl anexo with(nolock) " +
                    "where ConsReplica = @ConsReplica ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_glDocAnexoControl(dr);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocAnexoControl_GetAnexosByReplica");
                throw exception;
            }

        }

        /// <summary>
        /// Elimina un anexo
        /// </summary>
        /// <param name="nombreArchivo">Nombre del anexo que se desea eliminar</param>
        public void DAL_glDocAnexoControl_Delete(string nombreArchivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_glDocAnexoControl> result = new List<DTO_glDocAnexoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ArchivoNombre", SqlDbType.Char, 50);
                mySqlCommand.Parameters["@ArchivoNombre"].Value = nombreArchivo;

                mySqlCommand.CommandText = "delete from glDocAnexoControl  where ArchivoNombre = @ArchivoNombre";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocAnexoControl_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un anexo
        /// </summary>
        /// <param name="anexo"></param>
        public void DAL_glDocAnexoControl_Add(DTO_glDocAnexoControl anexo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "INSERT INTO glDocAnexoControl(NumeroDoc,ConsReplica,ArchivoNombre)" +
                    "VALUES(@NumeroDoc,@ConsReplica,@ArchivoNombre)";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsReplica", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ArchivoNombre", SqlDbType.Char, 50);

                mySqlCommand.Parameters["@NumeroDoc"].Value = anexo.NumeroDoc.Value;
                mySqlCommand.Parameters["@ConsReplica"].Value = anexo.ConsReplica.Value;
                mySqlCommand.Parameters["@ArchivoNombre"].Value = anexo.ArchivoNombre.Value;

                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocAnexoControl_Add");
                throw exception;
            }
        }
    }
}
