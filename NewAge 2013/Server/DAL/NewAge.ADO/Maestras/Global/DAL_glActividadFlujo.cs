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
    public class DAL_glActividadFlujo : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glActividadFlujo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene la lista de tareas de un documento
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<string> DAL_glActividadFlujo_GetTareaID(int documentoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select ActividadFlujoID from glActividadFlujo " +
                    " where EmpresaGrupoID = @EmpresaGrupoID and DocumentoID = @DocumentoID";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glActividadFlujo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;

                List<string> actividades = new List<string>();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    actividades.Add(dr["ActividadFlujoID"].ToString());
                }
                dr.Close();


                return actividades;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadFlujo_GetTareaID");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la lista de padres de una actividad de flujo
        /// </summary>
        /// <param name="actFlujoID">Actividad hija</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<DTO_glActividadFlujo> DAL_glActividadFlujo_GetParents(string actFlujoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ActividadPadre", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters["@ActividadPadre"].Value = actFlujoID;

                mySqlCommand.CommandText =
                    ";with MyCTE as ( " +
                    "    SELECT ActividadHija, ActividadPadre " +
                    "    FROM glProcedimientoFlujo " +
                    "    WHERE ActividadPadre = @ActividadPadre " +
                    "    UNION all " +
                    "	SELECT parent.ActividadHija, parent.ActividadPadre " +
                    "	FROM MyCTE AS child INNER JOIN glProcedimientoFlujo AS parent ON parent.ActividadHija = child.ActividadPadre " +
                    ") " +
                    "select ActividadPadre as ActividadFlujoID, Descriptivo " +
                    "from MyCTE cte inner join glActividadFlujo act on cte.ActividadPadre = act.ActividadFlujoID " +
                    "where ActividadPadre <> @ActividadPadre";


                List<DTO_glActividadFlujo> actividades = new List<DTO_glActividadFlujo>();

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glActividadFlujo act = new DTO_glActividadFlujo();
                    act.ID.Value = dr["ActividadFlujoID"].ToString();
                    act.Descriptivo.Value = dr["Descriptivo"].ToString();

                    actividades.Add(act);
                }
                dr.Close();

                return actividades;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadFlujo_GetParents");
                throw exception;
            }
        }

    }
}
