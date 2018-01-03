using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_ReportesOperacionesConjuntas
    /// </summary>
    public class DAL_ReportesOperacionesConjuntas : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesOperacionesConjuntas(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Funcion q se encarga de Traer los datos para Cierre
        /// </summary>
        /// <param name="Periodo">Fecha que se va a consultar</param>
        /// <returns>Tabla con Datos</returns>
        public DataTable DAL_ReportesOperacionesConjuntas_Legalizaciones(DateTime Periodo)
        {
            try
            {
                #region Filtros
                
                #endregion
                #region CommandText

                string query =
                    " SELECT ProyectoID, LineaPresupuestoID, " +
                        " CASE WHEN ([1] IS NULL) THEN 0 ELSE [1] END Enero, " +
                        " CASE WHEN ([2] IS NULL) THEN 0 ELSE [2] END Febrero, " +
                        " CASE WHEN ([3] IS NULL) THEN 0 ELSE [3] END Marzo, " +
                        " CASE WHEN ([4] IS NULL) THEN 0 ELSE [4] END Abril, " +
                        " CASE WHEN ([5] IS NULL) THEN 0 ELSE [5] END Mayo, " +
                        " CASE WHEN ([6] IS NULL) THEN 0 ELSE [6] END Junio, " +
                        " CASE WHEN ([7] IS NULL) THEN 0 ELSE [7] END Julio, " +
                        " CASE WHEN ([8] IS NULL) THEN 0 ELSE [8] END Agosto, " +
                        " CASE WHEN ([9] IS NULL) THEN 0 ELSE [9] END Septiembre, " +
                        " CASE WHEN ([10] IS NULL) THEN 0 ELSE [10] END Octubre, " +
                        " CASE WHEN ([11] IS NULL) THEN 0 ELSE [11] END Noviembre, " +
                        " CASE WHEN ([12] IS NULL) THEN 0 ELSE [12] END Diciembre " +
                    " FROM " +
                    " ( " +
                        " SELECT cierre.ProyectoID, LineaPresupuestoID, CtoOrigenLocME,MONTH(PeriodoID) Mes " +
                        " FROM plCierreLegalizacion cierre WITH(NOLOCK) " +
                        " INNER JOIN coProyecto proy WITH(NOLOCK) ON (proy.ProyectoID = cierre.ProyectoID AND proy.EmpresaGrupoID = cierre.eg_coProyecto) " +
                        " WHERE cierre.EmpresaID = @EmpresaID " +
                            " AND YEAR(PeriodoID) = @Year " +
                            " /*AND ProyectoTipo = 1 */" +
                    " ) Consulta " +
                    " PIVOT " +
                    " (  " +
                        " SUM(CtoOrigenLocME) " +
                        " FOR MES  IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]) " +
                     " ) Meses " +
                     " ORDER BY ProyectoID  ";

                SqlDataAdapter sda = new SqlDataAdapter(query, MySqlConnection.CreateCommand().Connection);

                #endregion
                #region Parametros

                sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Year", SqlDbType.Int);

                #endregion
                #region Asignacion valores parametros

                sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sda.SelectCommand.Parameters["@Year"].Value = Periodo.Year;

                #endregion

                DataSet dt = new DataSet();
                sda.Fill(dt, "plCierreLegalizacion");

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                 //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesOperacionesConjuntas");
                throw exception;
            }
        }
      
    }
}


