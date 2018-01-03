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
    public class DAL_glActividadChequeoLista : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glActividadChequeoLista(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de tareas asociados a un documento
        /// </summary>
        /// <param name="documentoID">Id de la pagaduria</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_MasterBasic> DAL_glActividadChequeoLista_GetByActividad(string actividadFlujoID)
        {
            List<DTO_MasterBasic> result = new List<DTO_MasterBasic>();
            try
            {
                #region Query
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glEmpresaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                //Query basico
                mySqlCommand.CommandText =
                    "select chequeo.* from glActividadChequeoLista chequeo with(nolock) " +
                    "where chequeo.ActividadFlujoID = @ActividadFlujoID " +
                    "	and chequeo.EmpresaGrupoID = @EmpresaGrupoID " +
                    "   and chequeo.ActivoInd = @ActivoInd";

                #endregion
                
                SqlDataReader dr;
                
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_MasterBasic basic = new DTO_MasterBasic();
                    basic.ID.Value = dr["ActividadFlujoID"].ToString();
                    basic.Descriptivo.Value = dr["Descripcion"].ToString();
                    result.Add(basic);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glActividadChequeoLista_GetByActividad");
                throw exception;
            }

        }               

        #endregion

    }
}
