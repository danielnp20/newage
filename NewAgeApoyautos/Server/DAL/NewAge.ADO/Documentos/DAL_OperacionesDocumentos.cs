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
using System.Collections;
using NewAge.ReportesComunes;
using System.IO;
using NewAge.ADO.Reportes;
using NewAge.DTO.Reportes;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_DocumentOps
    /// </summary>
    public class DAL_OperacionesDocumentos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_OperacionesDocumentos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Obtiene el prefijo de un documento dado
        /// </summary>
        /// <param name="areaFuncionalID">Codigo del area funcional</param>
        /// <param name="documentoID">Codigo del documento</param>
        /// <param name="empresaGrupoID">Codigo de grupo de empresas</param>
        /// <returns>Retorna el codigo del prefijo</returns>
        public string PrefijoDocumento_Get(string areaFuncionalID, int documentoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "select PrefijoID from glAreaFuncionalDocumentoPrefijo with(nolock) where AreaFuncionalID = @AreaFuncionalID " +
                    " and DocumentoID = @DocumentoID and EmpresaGrupoID = @EmpresaGrupoID and eg_glPrefijo = @eg_glPrefijo";

                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncionalDocumentoPrefijo, this.Empresa, egCtrl); ;
                mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areaFuncionalID;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
       
                var prefijoObj = mySqlCommandSel.ExecuteScalar();
                if (prefijoObj != null)
                {
                    return prefijoObj.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "PrefijoDocumento_Get");
                throw exception;
            }
        }        

        /// <summary>
        /// Trae el consecutivo para un numero de documento
        /// Si no existe crea uno y lo inicia en 1
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="onlyGet">Indica si solo puede traer la info o tambien crear un nuevo numero</param>
        /// <returns>Retorna el consecutivo</returns>
        public int GenerateNumeroDoc(int documentID, string prefijoID, bool onlyGet = false)
        {
            try
            {
                int res = 0;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select NumeroDoc from glConsPrefijoDoc where PrefijoId = @PrefijoID" +
                    " and DocumentoID = @DocumentoID and EmpresaID = @EmpresaID";

                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@PrefijoID"].Value = prefijoID;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                var consObj = mySqlCommand.ExecuteScalar();
                if (onlyGet)
                {
                    if (consObj != null)
                        return Convert.ToInt32(consObj);
                    else
                        return 0;
                }

                if (consObj != null)
                {
                    res = Convert.ToInt32(consObj);
                    mySqlCommand.CommandText = "UPDATE glConsPrefijoDoc set NumeroDoc = NumeroDoc + 1 where PrefijoID = @PrefijoID" +
                    " and DocumentoID = @DocumentoID and EmpresaID = @EmpresaID";
                    mySqlCommand.ExecuteScalar();

                    res = (int)consObj + 1;
                }
                else
                {
                    res = 1;

                    mySqlCommand.CommandText = "Insert into glConsPrefijoDoc(EmpresaID, PrefijoID, DocumentoID, NumeroDoc) " +
                        "values(@EmpresaID, @PrefijoID, @DocumentoID, @NumeroDoc)";

                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = res;

                    mySqlCommand.ExecuteNonQuery();
                }

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "GenerateNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un listado de tareas de glActividadPermiso para usuarios
        /// </summary>
        /// <param name="NumeroDoc">Usuario relacionado para la consulta</param>
        /// <returns>Listado de TareaID(int)</returns>
        public List<string> glActividadPermiso_GetActividadByUser(string UsuarioID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select ActividadFlujoID from glActividadPermiso with(nolock) where UsuarioID = @UsuarioID ";

                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char);
                mySqlCommand.Parameters["@UsuarioID"].Value = UsuarioID;

                List<string> result = new List<string>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    string dto = dr["ActividadFlujoID"].ToString();
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "glActividadPermiso_GetTareaByUser");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el valor original de un documento
        /// </summary>
        /// <param name="doc">Dogumento que se esta consultando</param>
        /// <param name="numeroDoc">Identificador del glDocumentoControl que se debe consultar</param>
        /// <returns></returns>
        public decimal GetValorOriginalDocumento(DTO_glDocumento doc, int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                if (doc != null && doc.ValidaValorInd.Value.Value)
                {
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                    mySqlCommandSel.CommandText = "Select valor from " + doc.TablaDocumento.Value + " with(nolock) where NumeroDoc = @NumeroDoc";
                    object res = mySqlCommandSel.ExecuteScalar();
                    if (res != null)
                        return Convert.ToDecimal(res);
                    else
                    return 0;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "GetValorOriginalDocumento");
                throw exception;
            }

        }

        #endregion
       
    }
}
