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
    public class DAL_glTabla : DAL_Base
    {
        DTO_aplMaestraPropiedades props = null;

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glTabla(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        #region Funciones Publicas

        /// <summary>
        /// Retorna todas las gl tablas de un grupo de empresas
        /// </summary>
        /// <param name="empGrupoIDs">diccionario con los empresa grupo para cada tipo de EmpresaGrupo</param>
        /// <returns>Lista de glTabla</returns>
        public IEnumerable<DTO_glTabla> DAL_glTabla_GetAllByEmpresaGrupo(Dictionary<int, string> empGrupoIDs, bool jerarquiaInd = false)
        {
            try
            {
                List<DTO_glTabla> tablas = new List<DTO_glTabla>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filter = string.Empty;
                if (jerarquiaInd)
                {
                    filter = " and Jerarquica = @Jerarquica ";
                    mySqlCommand.Parameters.Add("@Jerarquica", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@Jerarquica"].Value = true;
                }
                mySqlCommand.CommandText =
                    "SELECT gl.*, prop.TipoSeguridad " +
                    "FROM glTabla gl with(nolock)" +
                    "   LEFT JOIN aplMaestraPropiedad prop with(nolock) ON gl.DocumentoID = prop.DocumentoID " +
                    "WHERE gl.EmpresaGrupoID = @EmpresaGrupoID and prop.TipoSeguridad = @TipoSeguridad " + filter +
                    "ORDER BY DocumentoID";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoSeguridad", SqlDbType.Int);

                props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.glTabla, this.loggerConnectionStr);

                foreach (int egTipo in empGrupoIDs.Keys)
                {
                    mySqlCommand.Parameters["@EmpresaGrupoID"].Value = empGrupoIDs[egTipo];
                    mySqlCommand.Parameters["@TipoSeguridad"].Value = egTipo;                   

                    SqlDataReader dr;
                    dr = mySqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        var tabla = new DTO_glTabla(dr, props, true);
                        tablas.Add(tabla);
                    }
                    dr.Close();
                }
                return tablas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glTabla_GetAllByEmpresaGrupo");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna la info de una tabla segun el nombre y el grupo de empresas
        /// </summary>
        /// <param name="tablaNombre">Tabla Nombre</param>
        /// <param name="empGrupo">Grupo de empresas</param>
        /// <returns>Retorna la informacion de una tabla</returns>
        public DTO_glTabla DAL_glTabla_GetByTablaNombre(string tablaNombre, string empGrupo)
        {
            try
            {
                DTO_glTabla tabla = new DTO_glTabla();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText =
                    "SELECT gl.*, prop.TipoSeguridad " +
                    "FROM glTabla gl LEFT JOIN aplMaestraPropiedad prop ON gl.DocumentoID = prop.DocumentoID " +
                    "WHERE gl.EmpresaGrupoID = @EmpresaGrupoID AND gl.Descriptivo = @TablaNombre";

                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.VarChar, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@TablaNombre", SqlDbType.VarChar, UDT_DescripTBase.MaxLength);

                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = empGrupo;
                mySqlCommand.Parameters["@TablaNombre"].Value = tablaNombre;

                props = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.glTabla, this.loggerConnectionStr);

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    tabla = new DTO_glTabla(dr, props, false);
                }
                dr.Close();

                return tabla;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glTabla_GetByTablaNombre");
                throw exception;
            }
        }

        #endregion
      
    }
}
