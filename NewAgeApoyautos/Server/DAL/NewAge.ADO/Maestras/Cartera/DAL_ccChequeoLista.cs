using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Diagnostics;

namespace NewAge.ADO
{
    public class DAL_ccChequeoLista : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccChequeoLista(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de tareas asociados a un documento
        /// </summary>
        /// <param name="documentoID">Id de la pagaduria</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_MasterBasic> DAL_ccChequeoLista_GetByDocumento(int documentoID)
        {
            List<DTO_MasterBasic> result = new List<DTO_MasterBasic>();
            try
            {
                #region Query
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glEmpresaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                //Query basico
                mySqlCommand.CommandText =
                    "select tarea.* " +
                    "from ccChequeoLista tarea with(nolock) " +
                    "	inner join glDocumento doc with(nolock) on tarea.DocumentoID = doc.DocumentoID " +
                    "where tarea.DocumentoID = @DocumentoID " +
                    "	and tarea.EmpresaGrupoID = @EmpresaGrupoID " +
                    "   and tarea.ActivoInd = @ActivoInd";

                #endregion
                
                SqlDataReader dr;
                
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_MasterBasic basic = new DTO_MasterBasic();
                    basic.ID.Value = dr["TareaID"].ToString();
                    basic.Descriptivo.Value = dr["Descriptivo"].ToString();
                    result.Add(basic);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccAnexosLista_GetByPagaduria");
                throw exception;
            }

        }               

        #endregion

    }
}
