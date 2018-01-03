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
    /// DAL de DAL_prConvenio
    /// </summary>
    public class DAL_prConvenio : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prConvenio(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prConvenio

        #region Funciones publicas

        /// <summary>
        /// Adiciona el registro en tabla prSaldosDocu
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_prConvenio_Add(DTO_prConvenio convenio)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prConvenio " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,CodigoBSID " +
                                           "    ,inReferenciaID " +
                                           "    ,MonedaID " +
                                           "    ,Valor " +
                                           "    ,eg_prBienServicio  " +
                                           "    ,eg_inReferencia ) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@CodigoBSID " +
                                           "    ,@inReferenciaID " +
                                           "    ,@MonedaID " +
                                           "    ,@Valor " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_inReferencia)"; 
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);  
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = convenio.NumeroDoc.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = convenio.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = convenio.inReferenciaID.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = convenio.MonedaID.Value;
                mySqlCommand.Parameters["@Valor"].Value = convenio.Valor.Value;
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
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenio_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar la solicitud en tabla prSolicitudDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">solicitud</param>
        public void DAL_prConvenio_Upd(DTO_prConvenio convenio)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla prConvenio
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prConvenio " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,CodigoBSID  = @CodigoBSID " +
                                           "    ,inReferenciaID  = @inReferenciaID " +
                                           "    ,MonedaID  = @MonedaID " +
                                           "    ,Valor  = @Valor " +
                                           "     WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
             
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;             
                mySqlCommand.Parameters["@CodigoBSID"].Value = convenio.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = convenio.inReferenciaID.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = convenio.MonedaID.Value;
                mySqlCommand.Parameters["@Valor"].Value = convenio.Valor.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = convenio.Consecutivo.Value;
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
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenio_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prSaldosDocu segun el ConsecutivoDetaID
        /// </summary>
        /// <param name="NumeroDoc">ID del registro del detalle</param>
        /// <returns>lista de prSaldosDocu</returns>
        public List<DTO_prConvenio> DAL_prConvenio_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prConvenio with(nolock) where prConvenio.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prConvenio> list = new List<DTO_prConvenio>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prConvenio detail = new DTO_prConvenio(dr);
                    list.Add(detail);
                    index++;
                }
                dr.Close();
                return list;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenio_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prConvenio
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prConvenio_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prConvenio where EmpresaID = @EmpresaID " +
                " and NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenio_Delete");
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
