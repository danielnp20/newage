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
    public class DAL_ccAnexosLista : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccAnexosLista(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae los documentos anexos dependiendo de la pagaduria
        /// </summary>
        /// <param name="pagaduriaID">Id de la pagaduria</param>
        /// <returns></returns>
        public List<DTO_MasterBasic> DAL_ccAnexosLista_GetByPagaduria(string pagaduriaID)
        {
            List<DTO_MasterBasic> result = new List<DTO_MasterBasic>();
            try
            {
                
                #region Query
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccAnexosLista", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@PagaduriaID"].Value = pagaduriaID;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glEmpresaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl); 
                mySqlCommand.Parameters["@eg_ccAnexosLista"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAnexosLista, this.Empresa, egCtrl); 
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                //Query basico
                mySqlCommand.CommandText =
                        "SELECT t2.* " +
                        "FROM   ccPagaduriaAnexos t1 with(nolock) " +
                        "       INNER JOIN ccAnexosLista t2 with(nolock) ON (t1.DocumListaID=t2.DocumListaID) " +
                        "WHERE  t1.PagaduriaID= @PagaduriaID " +
                        "       AND t2.EmpresaGrupoID= @EmpresaGrupoID " +
                        "       AND t1.eg_ccPagaduria= @eg_ccPagaduria " +
                        "       AND t1.eg_ccAnexosLista= @eg_ccAnexosLista " +
                        "       AND t1.ActivoInd = @ActivoInd " +
                        "ORDER BY t2.DocumListaID ";

                #endregion
                
                SqlDataReader dr;
                
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_MasterBasic basic = new DTO_MasterBasic();
                    basic.ID.Value = dr["DocumListaID"].ToString();
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
