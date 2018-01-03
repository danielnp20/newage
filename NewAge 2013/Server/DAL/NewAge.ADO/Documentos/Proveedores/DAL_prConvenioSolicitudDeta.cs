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
    /// DAL de DAL_prConvenioSolicitudDeta
    /// </summary>
    public class DAL_prConvenioSolicitudDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prConvenioSolicitudDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId) : base(c, tx, empresa, userId) { }

        #region DAL_prConvenioSolicitudDeta

        #region Funciones publicas

        /// <summary>
        /// Consulta un DAL_prConvenioSolicitudDeta segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cargos</returns>
        public List<DTO_prConvenioSolicitudDeta> DAL_prConvenioSolicitudDeta_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prConvenioSolicitudDeta with(nolock) where prConvenioSolicitudDeta.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prConvenioSolicitudDeta> footer = new List<DTO_prConvenioSolicitudDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prConvenioSolicitudDeta detail = new DTO_prConvenioSolicitudDeta(dr);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDeta_Get", false);
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prConvenioSolicitudDeta segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cargos</returns>
        public List<DTO_prConvenioSolicitudDeta> DAL_prConvenioSolicitudDeta_GetByID(int ConsecutivoDetaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prConvenioSolicitudDeta with(nolock) where prConvenioSolicitudDeta.ConsecutivoDetaID = @ConsecutivoDetaID ";

                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = ConsecutivoDetaID;

                List<DTO_prConvenioSolicitudDeta> footer = new List<DTO_prConvenioSolicitudDeta>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prConvenioSolicitudDeta detail = new DTO_prConvenioSolicitudDeta(dr);
                    detail.Index = index;
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDeta_GetByID", false);
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla prConvenioSolicitudDeta
        /// </summary>
        /// <param name="footer">Cargos</param>
        public void DAL_prConvenioSolicitudDeta_Add(List<DTO_prConvenioSolicitudDeta> footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prConvenioSolicitudDeta " +
                                           "    (NumeroDoc    " +
                                           "    ,CodigoBSID    " +
                                           "    ,inReferenciaID    " +
                                           "    ,CantidadSol " +
                                           "    ,Valor " +
                                           "    ,eg_prBienServicio " +
                                           "    ,eg_inReferencia) " +
                                           "    VALUES" +
                                           "    (@NumeroDoc " +
                                           "    ,@CodigoBSID " +
                                           "    ,@inReferenciaID " +
                                           "    ,@CantidadSol " +
                                           "    ,@Valor " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_inReferencia) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@CantidadSol", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                foreach (DTO_prConvenioSolicitudDeta det in footer)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@CodigoBSID"].Value = det.CodigoBSID.Value;
                    mySqlCommand.Parameters["@inReferenciaID"].Value = det.inReferenciaID.Value;
                    mySqlCommand.Parameters["@CantidadSol"].Value = det.CantidadSol.Value;
                    mySqlCommand.Parameters["@Valor"].Value = det.Valor.Value;
                    mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDeta_Add", false);
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prConvenioSolicitudDeta
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prConvenioSolicitudDeta_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prConvenioSolicitudDeta where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException(exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDeta_Delete", false);
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
