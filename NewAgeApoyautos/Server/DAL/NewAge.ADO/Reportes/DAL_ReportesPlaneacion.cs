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
    /// DAL de DAL_ReportesPlaneacion
    /// </summary>
    public class DAL_ReportesPlaneacion : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesPlaneacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CierreLegalizacion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="Periodo">Periodo que se desea verificar</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>        
        /// <returns>Tabla con resultados</returns>
        public DataTable DAL_ReportesPlaneacion_CierreLegalizacion(DateTime Periodo, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso)
        {
            try
            {
                #region filtros

                string contr = !string.IsNullOrEmpty(contrato) ? " AND pro.ContratoID = @ContratoID " : string.Empty;
                string bloq = !string.IsNullOrEmpty(bloque) ? " AND area.BloqueID = @BloqueID " : string.Empty;
                string camp = !string.IsNullOrEmpty(campo) ? " AND logFisica.AreaFisica = @Campo " : string.Empty;
                string poz = !string.IsNullOrEmpty(pozo) ? " AND pro.LocFisicaID = @pozo " : string.Empty;
                string proyec = !string.IsNullOrEmpty(proyecto) ? " AND sobreEje.ProyectoID = @ProyectoID " : string.Empty;
                string activi = !string.IsNullOrEmpty(actividad) ? " AND pro.ActividadID = @Actividad " : string.Empty;
                string line = !string.IsNullOrEmpty(lineaPresupuesto) ? " AND sobreEje.LineaPresupuestoID = @LineaPresupuesto " : string.Empty;
                string centroCos = !string.IsNullOrEmpty(centroCosto) ? " AND sobreEje.CentroCostoID = @CentroCosto " : string.Empty;
                string recur = !string.IsNullOrEmpty(recurso) ? " AND presu.RecursoID= @recurso " : string.Empty;


                #endregion
                #region CommadText

                string query =
                    " SELECT  Mes " +  
                            " ,ProyectoID   " +
                            " ,CentroCostoID   " +
                            " ,LineaPresupuestoID   " +
                            " ,SUM(Eje$Ori$) Eje$Ori$ " +
                            " ,SUM(EjeU$Equiv) EjeU$Equiv " +
                            " ,SUM(EjeU$OriU$) EjeU$OriU$ " +
                            " ,SUM(Eje$Equiv) Eje$Equiv " +
                            " ,SUM(PrestYTD$) PrestYTD$ " +
                            " ,SUM(PrestYTDU$Equiv) PrestYTDU$Equiv " +
                            " ,SUM(PrestYTDU$) PrestYTDU$ " +
                            " ,SUM(PrestYTD$Equiv) PrestYTD$Equiv " + 
                            " ,SUM(PtoTotalLocML ) PtoTotalLocML " +
                            " ,SUM(PtoTotalLocME) PtoTotalLocME " +
                            " ,SUM(PtoTotalExtME ) PtoTotalExtME " +
                            " ,SUM(PtoTotalExtML) PtoTotalExtML " +
                            " ,SUM(CompActLocML) CompActLocML " +
                            " ,SUM(CompActExtME) CompActExtME " +
                            " ,SUM(RecibidoLocML) RecibidoLocML " +
                            " ,SUM(RecibidoExtML) RecibidoExtML " +
                            " ,SUM(CompPendLoc) CompPendLoc " +
                            " ,SUM(CompPendExt) CompPendExt " +
                    " FROM " +
                    " ( " +
                         " SELECT  MONTH(PeriodoID) Mes   " +
                                                   " ,cierreLeg.ProyectoID   " +
                                                   " ,cierreLeg.CentroCostoID   " +
                                                   " ,cierreLeg.LineaPresupuestoID   " + 
                                                   " ,(CtoOrigenLocML  + CtoInicialLocML) Eje$Ori$ " +
                                                   " ,(CtoOrigenLocME + CtoInicialLocME ) EjeU$Equiv " +
                                                   " ,(CtoOrigenExtME  + CtoInicialExtME) EjeU$OriU$ " +
                                                   " ,(CtoOrigenExtML  + CtoInicialExtML) Eje$Equiv " +
                                                   " ,(PtoInicialLocML  + PtoMesLocML ) PrestYTD$ " +
                                                   " ,(PtoInicialLocME  + PtoMesLocME) PrestYTDU$Equiv " +
                                                   " ,(PtoInicialExtME  + PtoMesExtME) PrestYTDU$ " +
                                                   " ,(PtoInicialExtML  + PtoMesExtML) PrestYTD$Equiv " +
                                                   " ,(PtoTotalLocML ) PtoTotalLocML " +
                                                   " ,(PtoTotalLocME) PtoTotalLocME " +
                                                   " ,(PtoTotalExtME)  PtoTotalExtME " +
                                                   " ,(PtoTotalExtML) PtoTotalExtML " +
                                                   " ,(CompActLocML) CompActLocML  " +
                                                   " ,(CompActExtME) CompActExtME " +
                                                   " ,(RecibidoLocML) RecibidoLocML " +
                                                   " ,(RecibidoExtML)  RecibidoExtML " + 
                                                   " ,(CompPendLoc)  CompPendLoc " +
                                                   " ,(CompPendExt) CompPendExt  " +
                        " FROM plCierreLegalizacion cierreLeg   " +
                         " INNER JOIN plLineaPresupuesto presu WITH(NOLOCK) ON (presu.LineaPresupuestoID = cierreLeg.LineaPresupuestoID AND presu.EmpresaGrupoID = cierreLeg.eg_plLineaPresupuesto)   " +
                         " INNER JOIN coProyecto pro WITH(NOLOCK) ON (pro.ProyectoID = cierreLeg.ProyectoID AND pro.EmpresaGrupoID = cierreLeg.eg_coProyecto)   " +
                         " INNER JOIN glLocFisica logFisica WITH(NOLOCK) ON (logFisica.LocFisicaID = pro.LocFisicaID AND logFisica.EmpresaGrupoID = pro.eg_glLocFisica)   " +
                         " INNER JOIN glAreaFisica area WITH(NOLOCK) ON (area.AreaFisica = logFisica.AreaFisica AND area.EmpresaGrupoID = logFisica.eg_glAreaFisica) " +  
                        " WHERE cierreLeg.EmpresaID = @EmpresaID   " +
                             " AND YEAR(PeriodoID) = @year   " +
                             " AND MONTH(PeriodoID) = @Month  " +
                                contr + bloq + camp + poz + proyec + activi + line + centroCos + recur +
                             /*AND pro.ContratoID = @ContratoID
                             AND area.BloqueID = @BloqueID
                             AND logFisica.AreaFisica = @Campo
                             AND pro.LocFisicaID = @pozo
                             AND cierreLeg.ProyectoID = @ProyectoID
                             AND pro.ActividadID = @Actividad
                             AND cierreLeg.LineaPresupuestoID = @LineaPresupuesto
                             AND cierreLeg.CentroCostoID = @CentroCosto */
                    " )Consulta " +     
                    " GROUP BY Mes, ProyectoID ,CentroCostoID, LineaPresupuestoID ";
                            
                         

                SqlDataAdapter sda = new SqlDataAdapter(query, MySqlConnection.CreateCommand().Connection);
                #endregion
                #region Parametros

                sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@year", SqlDbType.Int);
                sda.SelectCommand.Parameters.Add("@Month", SqlDbType.Int);
                sda.SelectCommand.Parameters.Add("@ContratoID", SqlDbType.Char, UDT_ContratoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@BloqueID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                sda.SelectCommand.Parameters.Add("@Campo", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@pozo", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Actividad", SqlDbType.Char, UDT_ActividadID.MaxLength);
                sda.SelectCommand.Parameters.Add("@LineaPresupuesto", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@CentroCosto", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@recurso", SqlDbType.Char, UDT_RecursoID.MaxLength);

                #endregion
                #region Asignacion de valores a parametros

                sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sda.SelectCommand.Parameters["@year"].Value = Periodo.Year;
                sda.SelectCommand.Parameters["@Month"].Value = Periodo.Month;
                sda.SelectCommand.Parameters["@ContratoID"].Value = contrato;
                sda.SelectCommand.Parameters["@BloqueID"].Value = bloque;
                sda.SelectCommand.Parameters["@Campo"].Value = campo;
                sda.SelectCommand.Parameters["@pozo"].Value = pozo;
                sda.SelectCommand.Parameters["@ProyectoID"].Value = proyecto;
                sda.SelectCommand.Parameters["@Actividad"].Value = actividad;
                sda.SelectCommand.Parameters["@LineaPresupuesto"].Value = lineaPresupuesto;
                sda.SelectCommand.Parameters["@CentroCosto"].Value = centroCosto;
                sda.SelectCommand.Parameters["@recurso"].Value = recurso;

                #endregion

                DataSet dt = new DataSet();
                sda.Fill(dt, "plCierreLegalizacion");

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesPlaneacion_CierreLegalizacion");
                throw exception;
            }
        }

        #endregion

        #region Presupuesto

        /// <summary>
        /// Funcion que se encarga de los presupuestos
        /// </summary>
        /// <param name="periodo">Periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea Ver</param>
        /// <param name="isAcumulado">Verifica si el Acumulado (True) Para correr el Procedimiento Almacenado</param>
        /// <param name="tipoMoneda">Tipo de Moneda con que se va a imprimir el reporte</param>
        /// <param name="isConsololidado">Verifica si se desea ver el reporte consolidado</param>
        /// <returns>Listado de DTO</returns>
        public DataTable DAL_ReportesPlaneacion_Presupuesto(DateTime periodo, string proyecto, bool isAcumulado, bool tipoMoneda, bool isConsololidado)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText

                if (!isAcumulado)
                    mySqlCommandSel = new SqlCommand("Planeacion_ReportPresupuestoMesaMes", base.MySqlConnection.CreateCommand().Connection);
                else
                    mySqlCommandSel = new SqlCommand("Planeacion_ReptPresupuesto", base.MySqlConnection.CreateCommand().Connection);

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);

                if (isAcumulado)
                    mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                else
                {
                    mySqlCommandSel.Parameters.Add("@Proyecto", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@TipoMoneda", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Consolidado", SqlDbType.TinyInt);
                }

                #endregion
                #region Asignacion de Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Year"].Value = periodo.Year;

                if (isAcumulado)
                    mySqlCommandSel.Parameters["@Month"].Value = periodo.Month;
                else
                {
                    mySqlCommandSel.Parameters["@Proyecto"].Value = proyecto;
                    mySqlCommandSel.Parameters["@TipoMoneda"].Value = tipoMoneda;
                    mySqlCommandSel.Parameters["@Consolidado"].Value = isConsololidado;
                }


                #endregion

                mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable("Presupuesto");
                sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesPlaneacion_Presupuesto");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarga de los presupuestos
        /// </summary>
        /// <param name="periodo">Periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea Ver</param>
        /// <param name="isAcumulado">Verifica si el Acumulado (True) Para correr el Procedimiento Almacenado</param>
        /// <param name="tipoMoneda">Tipo de Moneda con que se va a imprimir el reporte</param>
        /// <param name="isConsololidado">Verifica si se desea ver el reporte consolidado</param>
        /// <returns>Listado de DTO</returns>
        public DataTable DAL_ReportesPlaneacion_PresupuestoPxQ(DateTime periodo, string proyecto, bool isAcumulado, bool tipoMoneda, bool isConsololidado)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText

                if (!isAcumulado)
                    mySqlCommandSel = new SqlCommand("Planeacion_ReportPresupuestoMesaMes", base.MySqlConnection.CreateCommand().Connection);
                else
                    mySqlCommandSel = new SqlCommand("Planeacion_ReptPresupuesto", base.MySqlConnection.CreateCommand().Connection);

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);

                if (isAcumulado)
                    mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                else
                {
                    mySqlCommandSel.Parameters.Add("@Proyecto", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@TipoMoneda", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Consolidado", SqlDbType.TinyInt);
                }

                #endregion
                #region Asignacion de Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Year"].Value = periodo.Year;

                if (isAcumulado)
                    mySqlCommandSel.Parameters["@Month"].Value = periodo.Month;
                else
                {
                    mySqlCommandSel.Parameters["@Proyecto"].Value = proyecto;
                    mySqlCommandSel.Parameters["@TipoMoneda"].Value = tipoMoneda;
                    mySqlCommandSel.Parameters["@Consolidado"].Value = isConsololidado;
                }


                #endregion

                mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable("Presupuesto");
                sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesPlaneacion_Presupuesto");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada
        /// </summary>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public List<DTO_ReportEjecucionPresupuestal> DAL_ReportesPlaneacion_EjecucionPresupuestalxMoneda(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID)
        {
            try
            {
                List<DTO_ReportEjecucionPresupuestal> result = new List<DTO_ReportEjecucionPresupuestal>();

                SqlCommand mySqlCommandSel = new SqlCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string proyecto = !string.IsNullOrEmpty(ProyectoID) ? " AND proy.ProyectoID LIKE '%' + @ProyectoID + '%' " : string.Empty;
                string actividad = !string.IsNullOrEmpty(ActividadID) ? " AND proy.ActividadID LIKE '%' + @Actividad + '%' " : string.Empty;
                string LineaPresu = !string.IsNullOrEmpty(LineaPresupuestalID) ? " AND cierre.LineaPresupuestoID LIKE '%' + @LineaPres + '%' " : string.Empty;
                string centroCosto = !string.IsNullOrEmpty(CentroCostoID) ? " AND cierre.CentroCostoID  LIKE '%' + @CentroCost + '%' " : string.Empty;
                string recursoGrup = !string.IsNullOrEmpty(RecursoGrupoID) ? " AND recur.RecursoGrupoID  LIKE '%' + @RecurGrupo + '%' " : string.Empty;
                string proyectType = ProyectoTipo == "*" ? string.Empty : ProyectoTipo;

                #endregion
                #region CommandText

                mySqlCommandSel = new SqlCommand("ReportPlaneacion_EjecucionPresupustalxMoneda", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ProyectoTipo", SqlDbType.VarChar);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActividadID", SqlDbType.VarChar, UDT_ActividadID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPres", SqlDbType.VarChar, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCost", SqlDbType.VarChar, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecurGrupo", SqlDbType.VarChar, UDT_RecursoID.MaxLength);

                #endregion
                #region Asignacion Valores Parametros

                mySqlCommandSel.Parameters["@Year"].Value = Periodo.Year;
                mySqlCommandSel.Parameters["@Month"].Value = Periodo.Month;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TipoReporte"].Value = TipoReporte;
                mySqlCommandSel.Parameters["@ProyectoTipo"].Value = proyectType;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = proyecto;
                mySqlCommandSel.Parameters["@ActividadID"].Value = actividad;
                mySqlCommandSel.Parameters["@LineaPres"].Value = LineaPresu;
                mySqlCommandSel.Parameters["@CentroCost"].Value = centroCosto;
                mySqlCommandSel.Parameters["@RecurGrupo"].Value = recursoGrup;

                #endregion

                DTO_ReportEjecucionPresupuestal presupuesto = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    presupuesto = new DTO_ReportEjecucionPresupuestal(this.loggerConnectionStr, dr, TipoReporte);
                    result.Add(presupuesto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesPlaneacion_EjecucionPresupuestal");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer los datos para generar la Ejecucion Presupuestal por Origen
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        public DataTable DAL_ReportesPlaneacion_EjecucionPresupuestaXOrigen(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID)
        {
            try
            {
                SqlCommand sc = new SqlCommand("ReportPlaneacion_EjecucionPresupuestalxOrigen", base.MySqlConnection.CreateCommand().Connection);
                SqlDataAdapter sda = new SqlDataAdapter();

                #region Filtros

                string proyecto = !string.IsNullOrEmpty(ProyectoID) ? " AND proy.ProyectoID LIKE '%' + @ProyectoID + '%' " : string.Empty;
                string actividad = !string.IsNullOrEmpty(ActividadID) ? " AND proy.ActividadID LIKE '%' + @Actividad + '%' " : string.Empty;
                string LineaPresu = !string.IsNullOrEmpty(LineaPresupuestalID) ? " AND cierre.LineaPresupuestoID LIKE '%' + @LineaPres + '%' " : string.Empty;
                string centroCosto = !string.IsNullOrEmpty(CentroCostoID) ? " AND cierre.CentroCostoID  LIKE '%' + @CentroCost + '%' " : string.Empty;
                string recursoGrup = !string.IsNullOrEmpty(RecursoGrupoID) ? " AND recur.RecursoGrupoID  LIKE '%' + @RecurGrupo + '%' " : string.Empty;
                string proyectType = ProyectoTipo == "*" ? string.Empty : ProyectoTipo;

                #endregion
                #region Parametros

                sc.Parameters.Add("@Year", SqlDbType.Int);
                sc.Parameters.Add("@Month", SqlDbType.Int);
                sc.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sc.Parameters.Add("@TipoReporte", SqlDbType.Char);
                sc.Parameters.Add("@ProyectoTipo", SqlDbType.VarChar);
                sc.Parameters.Add("@ProyectoID", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                sc.Parameters.Add("@ActividadID", SqlDbType.VarChar, UDT_ActividadID.MaxLength);
                sc.Parameters.Add("@LineaPres", SqlDbType.VarChar, UDT_LineaPresupuestoID.MaxLength);
                sc.Parameters.Add("@CentroCost", SqlDbType.VarChar, UDT_CentroCostoID.MaxLength);
                sc.Parameters.Add("@RecurGrupo", SqlDbType.VarChar, UDT_RecursoID.MaxLength);


                #endregion
                #region Asignacion valores a parametros

                sc.Parameters["@Year"].Value = Periodo.Year;
                sc.Parameters["@Month"].Value = Periodo.Month;
                sc.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sc.Parameters["@TipoReporte"].Value = TipoReporte;
                sc.Parameters["@ProyectoTipo"].Value = proyectType;
                sc.Parameters["@ProyectoID"].Value = proyecto;
                sc.Parameters["@ActividadID"].Value = actividad;
                sc.Parameters["@LineaPres"].Value = LineaPresu;
                sc.Parameters["@CentroCost"].Value = centroCosto;
                sc.Parameters["@RecurGrupo"].Value = recursoGrup;

                #endregion

                sc.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = sc;
                DataTable table = new DataTable("EjecucionPresupuestalOrigen");
                sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesPlaneacion_EjecucionPresupuestaXOrigen");
                throw exception;
            }
        }

        #endregion

        #region SobreEjecucion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="year">Año que se desea ver</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>
        /// <param name="usuario">Filtra un usuario</param>
        /// <param name="prefijo">Filtra un prefijo </param>
        /// <param name="numeroDoc">Filtra un numero de Documento Especifico</param>
        /// <returns>Tabla con resultados</returns>
        public DataTable DAL_ReportesPlaneacion_SobreEjecucion(int year, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso, string usuario, string prefijo, string numeroDoc)
        {
            try
            {
                #region Filtros
                string contr = !string.IsNullOrEmpty(contrato) ? " AND pro.ContratoID = @ContratoID " : string.Empty;
                string bloq = !string.IsNullOrEmpty(bloque) ? " AND area.BloqueID = @BloqueID " : string.Empty;
                string camp = !string.IsNullOrEmpty(campo) ? " AND logFisica.AreaFisica = @Campo " : string.Empty;
                string poz = !string.IsNullOrEmpty(pozo) ? " AND pro.LocFisicaID = @pozo " : string.Empty;
                string proyec = !string.IsNullOrEmpty(proyecto) ? " AND sobreEje.ProyectoID = @ProyectoID " : string.Empty;
                string activi = !string.IsNullOrEmpty(actividad) ? " AND pro.ActividadID = @Actividad " : string.Empty;
                string line = !string.IsNullOrEmpty(lineaPresupuesto) ? " AND sobreEje.LineaPresupuestoID = @LineaPresupuesto " : string.Empty;
                string centroCos = !string.IsNullOrEmpty(centroCosto) ? " AND sobreEje.CentroCostoID = @CentroCosto " : string.Empty;
                string recur = !string.IsNullOrEmpty(recurso) ? " AND presu.RecursoID= @recurso " : string.Empty;
                string usuer = !string.IsNullOrEmpty(usuario) ? " AND sobreEje.UsuarioAprSobreejec = @Usuario " : string.Empty;
                string pref = !string.IsNullOrEmpty(prefijo) ? " AND sobreEje.PrefijoID = @Prefijo " : string.Empty;
                string numDoc = !string.IsNullOrEmpty(numeroDoc) ? " AND sobreEje.NumeroDoc =  " + Convert.ToUInt16(numeroDoc) : string.Empty;
                #endregion
                #region CommandText

                string query =
                    " SELECT Ano, " +
                            " sobreEje.ProyectoID, " +
                            " sobreEje.CentroCostoID, " +
                            " sobreEje.LineaPresupuestoID, " +
                            " CodigoBSID, " +
                            " PrefijoID, " +
                            " DocumentoNro, " +
                            " TipoDocumento, " +
                            " NumeroDoc, " +
                            " ConsecutivoDetaID, " +
                            " FechaDoc, " +
                            " MonedaOrigen, " +
                            " AreaAprobacion, " +
                            " Estado, " +
                            " TipoAprobacion, " +
                            " ProveedorID, " +
                            " TasaOC, " +
                            " CantidadDOC, " +
                            " ValorOCLocML, " +
                            " ValorOCLocME, " +
                            " ValorOCExtME, " +
                            " ValorOCExtML, " +
                            " CantidadSOL, " +
                            " CtoOrigenLocML, " +
                            " CtoOrigenLocME, " +
                            " CtoOrigenExtME, " +
                            " CtoOrigenExtML, " +
                            " CantidadPTO, " +
                            " PtoMesLocML, " +
                            " PtoMesLocME, " +
                            " PtoMesExtME, " +
                            " PtoMesExtML, " +
                            " PtoTotalLocML, " +
                            " PtoTotalLocME, " +
                            " PtoTotalExtME, " +
                            " PtoTotalExtML, " +
                            " CompActLocML, " +
                            " CompActLocME, " +
                            " CompActExtME, " +
                            " CompActExtML, " +
                            " RecibidoLocML, " +
                            " RecibidoLocME, " +
                            " RecibidoExtME, " +
                            " RecibidoExtML, " +
                            " CtoInicialLocML, " +
                            " ocProcesoLocML, " +
                            " ocProcesoExtME, " +
                            " CtoInicialExtME, " +
                            " PtoInicialLocML, " +
                            " PtoInicialExtME, " +
                            " CompInicialocML, " +
                            " CompinicialExtME, " +
                            " RecInicialLocML, " +
                            " RecInicialExtME, " +
                            " ocProcesoInicialLocML, " +
                            " ocProcesoInicialExtME, " +
                            " UsuarioRevSobreejec, " +
                            " FechaRevSobreejec, " +
                            " UsuarioAprSobreejec, " +
                            " FechaAprSobreejec, " +
                            " IndSolicitaPres, " +
                            " Observaciones " +
                        " FROM  plSobreEjecucion sobreEje WITH(NOLOCK) " +
                            " INNER JOIN plLineaPresupuesto presu WITH(NOLOCK) ON (presu.LineaPresupuestoID = " +
                                " sobreEje.LineaPresupuestoID AND presu.EmpresaGrupoID = sobreEje.eg_plLineaPresupuesto) " +
                        " INNER JOIN coProyecto pro WITH(NOLOCK) ON (pro.ProyectoID = sobreEje.ProyectoID AND  " +
                            " pro.EmpresaGrupoID = sobreEje.eg_coProyecto) " +
                        " INNER JOIN glLocFisica logFisica WITH(NOLOCK) ON (logFisica.LocFisicaID = pro.LocFisicaID AND " +
                            " logFisica.EmpresaGrupoID = pro.eg_glLocFisica) " +
                        "INNER JOIN glAreaFisica area WITH(NOLOCK) ON (area.AreaFisica = logFisica.AreaFisica AND " +
                            " area.EmpresaGrupoID = logFisica.eg_glAreaFisica) " +
                        " WHERE sobreEje.EmpresaID = @EmpresaID " +
                            " AND ano = @year " +
                             contr + bloq + camp + poz + proyec + activi + line + centroCos + recur + usuer + pref + numDoc;
                /*AND pro.ContratoID = @ContratoID
                AND area.BloqueID = @BloqueID
                AND logFisica.AreaFisica = @Campo
                AND pro.LocFisicaID = @pozo
                AND sobreEje.ProyectoID = @ProyectoID
                AND pro.ActividadID = @Actividad
                AND sobreEje.LineaPresupuestoID = @LineaPresupuesto
                AND sobreEje.CentroCostoID = @CentroCosto
                AND presu.RecursoID= @recurso
                AND sobreEje.UsuarioAprSobreejec = @Usuario
                AND sobreEje.PrefijoID = @Prefijo
                AND sobreEje.NumeroDoc = @numeroDoc*/

                SqlDataAdapter sda = new SqlDataAdapter(query, MySqlConnection.CreateCommand().Connection);
                #endregion
                #region Parametros

                sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@year", SqlDbType.Int);
                sda.SelectCommand.Parameters.Add("@ContratoID", SqlDbType.Char, UDT_ContratoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@BloqueID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                sda.SelectCommand.Parameters.Add("@Campo", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@pozo", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Actividad", SqlDbType.Char, UDT_ActividadID.MaxLength);
                sda.SelectCommand.Parameters.Add("@LineaPresupuesto", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@CentroCosto", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@recurso", SqlDbType.Char, UDT_RecursoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Usuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                sda.SelectCommand.Parameters.Add("@Prefijo", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                //sda.SelectCommand.Parameters.Add("@numeroDoc", SqlDbType.Char);

                #endregion
                #region Asignacion Valores Parametros

                sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sda.SelectCommand.Parameters["@year"].Value = year;
                sda.SelectCommand.Parameters["@ContratoID"].Value = contrato;
                sda.SelectCommand.Parameters["@BloqueID"].Value = bloque;
                sda.SelectCommand.Parameters["@Campo"].Value = campo;
                sda.SelectCommand.Parameters["@pozo"].Value = pozo;
                sda.SelectCommand.Parameters["@ProyectoID"].Value = proyecto;
                sda.SelectCommand.Parameters["@Actividad"].Value = actividad;
                sda.SelectCommand.Parameters["@LineaPresupuesto"].Value = lineaPresupuesto;
                sda.SelectCommand.Parameters["@CentroCosto"].Value = centroCosto;
                sda.SelectCommand.Parameters["@recurso"].Value = recurso;
                sda.SelectCommand.Parameters["@Usuario"].Value = usuario;
                sda.SelectCommand.Parameters["@Prefijo"].Value = pref;
                //sda.SelectCommand.Parameters["@numeroDoc"].Value = numDoc;
                #endregion

                DataSet dt = new DataSet();
                sda.Fill(dt, "plSobreEjecucion");

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesPlaneacion_SobreEjecucion");
                throw exception;
            }
        }

        #endregion
    }
}


