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
    /// DAL de DAL_plPlaneacion_Proveedores
    /// </summary>
    public class DAL_plPlaneacion_Proveedores : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plPlaneacion_Proveedores(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_plPlaneacion_Proveedores

        #region Funciones publicas

        /// <summary>
        /// Adiciona el registro en tabla plPlaneacion_Proveedores
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_plPlaneacion_Proveedores_Add(DTO_plPlaneacion_Proveedores planeaProveedor)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO plPlaneacion_Proveedores " +
                                           "    (EmpresaID    " +
                                           "    ,ConsActLinea    " +
                                           "    ,CodigoBSID " +
                                           "    ,inReferenciaID " +
                                           "    ,eg_prBienServicio  " +
                                           "    ,eg_inReferencia ) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@ConsActLinea " +
                                           "    ,@CodigoBSID " +
                                           "    ,@inReferenciaID " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_inReferencia)"; 
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ConsActLinea", SqlDbType.Int);  
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ConsActLinea"].Value = planeaProveedor.ConsActLinea.Value;
                mySqlCommand.Parameters["@CodigoBSID"].Value = planeaProveedor.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = planeaProveedor.inReferenciaID.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPlaneacion_Proveedores_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualizar en tabla plPlaneacion_Proveedores y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">planeaProveedor</param>
        public void DAL_plPlaneacion_Proveedores_Upd(DTO_plPlaneacion_Proveedores planeaProveedor)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla plPlaneacion_Proveedores
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE plPlaneacion_Proveedores " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,CodigoBSID  = @CodigoBSID " +
                                           "    ,inReferenciaID  = @inReferenciaID " +
                                           "     WHERE Consecutivo = @Consecutivo";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);             
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_ReferenciaID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;             
                mySqlCommand.Parameters["@CodigoBSID"].Value = planeaProveedor.CodigoBSID.Value;
                mySqlCommand.Parameters["@inReferenciaID"].Value = planeaProveedor.inReferenciaID.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = planeaProveedor.Consecutivo.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPlaneacion_Proveedores_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un plPlaneacion_Proveedores segun el ConsActLinea
        /// </summary>
        /// <param name="ConsActLinea">ID del registro del detalle</param>
        /// <returns>lista de plPlaneacion_Proveedores</returns>
        public List<DTO_plPlaneacion_Proveedores> DAL_plPlaneacion_Proveedores_GetByConsActLinea(int ConsActLinea)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from plPlaneacion_Proveedores with(nolock) where ConsActLinea = @ConsActLinea ";

                mySqlCommand.Parameters.Add("@ConsActLinea", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsActLinea"].Value = ConsActLinea;

                List<DTO_plPlaneacion_Proveedores> list = new List<DTO_plPlaneacion_Proveedores>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_plPlaneacion_Proveedores detail = new DTO_plPlaneacion_Proveedores(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPlaneacion_Proveedores_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de la tabla con un filtro
        /// </summary>
        /// <returns>Lista </returns>
        public List<DTO_plPlaneacion_Proveedores> DAL_plPlaneacion_Proveedores_GetByParameter(DTO_plPlaneacion_Proveedores filter)
        {
            List<DTO_plPlaneacion_Proveedores> results = new List<DTO_plPlaneacion_Proveedores>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from plPlaneacion_Proveedores doc with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.ConsActLinea.Value.ToString()))
                {
                    query += "and ConsActLinea = @ConsActLinea ";
                    mySqlCommand.Parameters.Add("@ConsActLinea", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsActLinea"].Value = filter.ConsActLinea.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CodigoBSID.Value.ToString()))
                {
                    query += "and CodigoBSID = @CodigoBSID ";
                    mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                    mySqlCommand.Parameters["@CodigoBSID"].Value = filter.CodigoBSID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.inReferenciaID.Value.ToString()))
                {
                    query += "and inReferenciaID = @inReferenciaID ";
                    mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);
                    mySqlCommand.Parameters["@inReferenciaID"].Value = filter.inReferenciaID.Value;
                    filterInd = true;
                }               
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return results;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_plPlaneacion_Proveedores total = new DTO_plPlaneacion_Proveedores(dr);
                    results.Add(total);
                    index++;
                }
                dr.Close();               

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPlaneacion_Proveedores_GetByParameter");
                throw exception;
            }
        } 

        /// <summary>
        /// Elimina registros de la tabla de plPlaneacion_Proveedores
        /// </summary>
        /// <param name="ConsActLinea">ConsActLinea</param>
        public void DAL_plPlaneacion_Proveedores_Delete(int ConsActLinea)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConsActLinea", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ConsActLinea"].Value = ConsActLinea;

                mySqlCommandSel.CommandText = "DELETE FROM plPlaneacion_Proveedores where EmpresaID = @EmpresaID " +
                " and ConsActLinea = @ConsActLinea";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPlaneacion_Proveedores_Delete");
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
