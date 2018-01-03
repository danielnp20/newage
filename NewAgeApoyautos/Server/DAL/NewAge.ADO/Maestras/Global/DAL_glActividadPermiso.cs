using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_glActividadPermiso
    /// </summary>
    public class DAL_glActividadPermiso : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glActividadPermiso(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Obtiene la lista de usuarios a los que les tiene que enviar una alarma
        /// </summary>
        /// <param name="actFlujoID">Identificador de la actividad</param>
        /// <param name="afID">Identificador del area funcional</param>
        /// <returns>Retorna la lista de usuarios</returns>
        public List<string> DAL_ActividadPermiso_GetAlarmaUsuarioByActividadAndAF(string actFlujoID, string afID)
        {
            try
            { 
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select distinct AlarmaUsuario1 from glActividadPermiso with(nolock) " +
                    " where EmpresaGrupoID = @EmpresaGrupoID and ActividadFlujoID = @ActividadFlujoID and AreaFuncionalID = @AreaFuncionalID";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadPermiso, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@AreaFuncionalID"].Value = afID;

                List<string> usuarios = new List<string>();
                
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["AlarmaUsuario1"] != null)
                        usuarios.Add(dr["AlarmaUsuario1"].ToString());
                }
                dr.Close();


                return usuarios;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadPermiso_GetAlarmaUsuarioByActividadAndAF");
                throw exception;
            }
        }

    }
}
