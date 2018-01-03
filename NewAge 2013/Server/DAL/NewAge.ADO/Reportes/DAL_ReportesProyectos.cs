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
    /// DAL de DAL_ReportesProyectos
    /// </summary>
    public class DAL_ReportesProyectos : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesProyectos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Funcion q se encarga de traer el cumplimiento del proyecto
        /// </summary>
        /// <param name="FechaCorte">Fecha de Corte</param>
        /// <param name="Proyecto">Filtra un Proyecto Especifico</param>
        /// <param name="Estado">Filtra un Estado Especifico</param>
        /// <param name="LineaFlujo">Filtra un LineaFlujo Especifico</param>
        /// <param name="Etapa">Filtra un Etapa Especifico</param>
        /// <returns>Tabla con el Cumplimiento</returns>
        public DataTable DAL_ReportesProyectos_Cumplimiento(DateTime FechaCorte, string Proyecto, string Estado, string LineaFlujo, string Etapa)
        {
            try
            {
                #region Filtros

                string proyect = !string.IsNullOrEmpty(Proyecto) ? " AND ctrl.ProyectoID = @ProyectoID " : string.Empty;
                string estado = Estado == "*" ? string.Empty : " AND servi.Estado = @Estado ";
                string lineFlujo = !string.IsNullOrEmpty(LineaFlujo) ? " AND LineaFlujoID = @LineaFlujoID " : string.Empty;
                string etapa = !string.IsNullOrEmpty(Etapa) ? " AND ActividadEtapaID = @Etapa " : string.Empty;

                #endregion
                #region CommandText

                string query =
                    " SELECT *, DATEDIFF(day, FechaInicialProgramada,FechaTerminacionProgramada) as DiasAtrasados " +
                    " FROM " +
                    " ( " +
                        " SELECT servi.TrabajoID Trabajo, trab.Descriptivo DescrionTrabajo, " +
                             " CASE WHEN(servi.Estado = 0 ) THEN 'Sin Iniciar' " +
                             " WHEN(servi.Estado = 1 ) THEN 'En Desarrollo' " +
                             " WHEN(servi.Estado = 2 ) THEN 'Cerrado' END AS Estado, " +
                             " UsuarioResponsable, FechaIniciaPRO FechaInicialProgramada, CASE WHEN (servi.Estado = 2) THEN FechaTerminaPRO ELSE GETDATE()  " +
                                 " END AS FechaTerminacionProgramada , " +
                             " FechaInicia, FechaTermina " +
                        " FROM pyServicioPrograma servi WITH(NOLOCK) " +
                            " INNER JOIN pyTrabajo trab WITH(NOLOCK) ON (trab.TrabajoID = servi.TrabajoID and trab.EmpresaGrupoID = servi.eg_pyTrabajo) " +
                            " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = servi.NumeroDoc " +
                        " WHERE ctrl.EmpresaID = @EmpresaID " +
                               " AND ctrl.FechaDoc = @Date " +
                            " /*AND ctrl.ProyectoID = @ProyectoID " +
                            " AND servi.Estado = @Estado " +
                            " AND LineaFlujoID = @LineaFlujoID " +
                            " AND ActividadEtapaID = @Etapa*/	 " +
                    " ) AS Consulta ";

                SqlDataAdapter sda = new SqlDataAdapter(query, MySqlConnection.CreateCommand().Connection);
                #endregion
                #region Parametros

                sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Date", SqlDbType.DateTime);
                sda.SelectCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Estado", SqlDbType.Char);
                sda.SelectCommand.Parameters.Add("@LineaFlujoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                sda.SelectCommand.Parameters.Add("@Etapa", SqlDbType.Char, UDT_CodigoGrl.MaxLength);

                #endregion
                #region Asignacion Valores a Parametros

                sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value ;
                sda.SelectCommand.Parameters["@Date"].Value = FechaCorte;
                sda.SelectCommand.Parameters["@ProyectoID"].Value = Proyecto;
                sda.SelectCommand.Parameters["@Estado"].Value = Estado;
                sda.SelectCommand.Parameters["@LineaFlujoID"].Value = LineaFlujo;
                sda.SelectCommand.Parameters["@Etapa"].Value = Etapa;
                
                #endregion

                DataSet dt = new DataSet();
                sda.Fill(dt, "pyServicioPrograma");

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                 //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesProyectos_Cumplimiento");
                throw exception;
            }

        }

        /// <summary>
        /// Funcion que se encarga de traer la informacion del Presupuesto que se requiere para cada proyecto
        /// </summary>
        /// <param name="Periodo">Perido a presupuestar</param>
        /// <param name="Proyecto">Filtra un proyecto especifico a verificar</param>
        /// <returns>Tabla con el presupuesto</returns>
        public DataTable DAL_ReportesProyectos_Presupuesto(DateTime Periodo, string Proyecto)
        {
            try
            {
                #region Filtros

                string proyect = !string.IsNullOrEmpty(Proyecto) ? " AND ctrl.ProyectoID = @ProyectoID " : string.Empty;
                
                #endregion
                #region CommandText

                string query =
                    " SELECT  trab.CapituloTrabajoID,serv.TrabajoID, trab.Descriptivo as Descripcion, UnidadInvID, Cantidad, CostoLocal as ValorUnitarioPesos, " +
                        " (Cantidad*CostoLocal) as ValorTotalPesos, CostoExtra ValorUnitarioDolar, (Cantidad * CostoExtra) as ValorTotalDolar " +
                    " FROM pyServicioDeta serv WITH(NOLOCK) " +
                        " INNER JOIN pyTrabajo trab WITH(NOLOCK) ON (trab.TrabajoID = serv.TrabajoID AND trab.EmpresaGrupoID = serv.eg_pyTrabajo) " +
                        " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = serv.NumeroDoc " +
                    " WHERE ctrl.EmpresaID = @EmpresaID " + proyect +
                        " /*AND ctrl.ProyectoID = @ProyectoID*/ ";

                SqlDataAdapter sda = new SqlDataAdapter(query, MySqlConnection.CreateCommand().Connection);

                #endregion
                #region Parametros

                sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);

                #endregion
                #region Asiganacion de Valores a Parametros

                sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sda.SelectCommand.Parameters["@ProyectoID"].Value = Proyecto;

                #endregion

                DataSet dt = new DataSet();
                sda.Fill(dt, "pyServicioDeta");

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesProyectos_Presupuesto");
                throw exception;
            }
        }
    }
}


