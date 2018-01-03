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
    /// DAL de DAL_Contabilidad
    /// </summary>
    [ADOExceptionManager]
    public class DAL_ReportesNomina : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesNomina(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Reportes Aportes

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte de Aportes a Pension
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Feha final del Reporte</param>
        /// <param name="empleadoFil">Filtro</param> 
        /// <param name="orderBy">is Ordenado Por nombre?</param>
        /// <returns>Lista de dto</returns>
        public List<DTO_noAportesPension> DAL_ReportesNomina_AportesPension(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy,String terceroid,String nofondosaludid,String nocajaid)
        {
            try
            {
                List<DTO_noAportesPension> results = new List<DTO_noAportesPension>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand(); 
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                string order = "noEmpleado.EmpleadoID";

                if (orderBy)
                {
                    order = "ORDER BY noEmpleado.Descriptivo";
                }
                if (!string.IsNullOrEmpty(empleadoFil))
                {
                    filtro = "AND noEmpleado.EmpleadoID = " + "'" + empleadoFil + " '";
                }

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID = CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(nofondosaludid))
                {
                    filtro += " AND noLiquidacionesDocu.FondoSalud =  @FondoSalud ";
                    mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@FondoSalud"].Value = nofondosaludid;
                }

                if (!string.IsNullOrEmpty(nocajaid))
                {
                    filtro += " AND noLiquidacionesDocu.CajaNOID =  @CajaNOID ";
                    mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, UDT_CajaNOID.MaxLength);
                    mySqlCommandSel.Parameters["@CajaNOID"].Value = nocajaid;
                }

                mySqlCommandSel.CommandText =

                    "   SELECT noFondo.FondoNOID,       "   +
                    "   		noFondo.Descriptivo						as FondoDesc,         "   +
                    "   		noEmpleado.EmpleadoID					as Cedula,        "   +
                    "   		noEmpleado.Descriptivo					as EmpleadoDesc,         "   +
                    "   		noPlanillaAportesDeta.IngresoBasePEN	as Base,        "   +
                    "   		noPlanillaAportesDeta.VlrEmpresaPEN		as AporteEmpresa,         "   +
                    "   		noPlanillaAportesDeta.VlrTrabajadorPEN	as AporteEmpleado,        "   +
                    "   		noPlanillaAportesDeta.VlrSolidaridad + noPlanillaAportesDeta.VlrSubsistencia as AporteSolidaridad,         "   +
                    "   		(noPlanillaAportesDeta.VlrTrabajadorPEN + noPlanillaAportesDeta.VlrTrabajadorSLD  + noPlanillaAportesDeta.VlrSolidaridad)  as TotalAporte  "   +
                    "   FROM noPlanillaAportesDeta			with(nolock)         "   +
                    "   	INNER JOIN noLiquidacionesDocu	ON noPlanillaAportesDeta.NumeroDoc	= noLiquidacionesDocu.NumeroDoc         "   +
                    "   	INNER JOIN glDocumentoControl	ON glDocumentoControl.NumeroDoc		= noLiquidacionesDocu.NumeroDoc         "   +
                    "   	INNER JOIN noEmpleado			ON noEmpleado.ContratoNOID			= noLiquidacionesDocu.ContratoNOID   AND noEmpleado.EmpresaGrupoID = @EmpresaID       " +
                    "   	INNER JOIN noFondo				ON noFondo.FondoNOID				= noPlanillaAportesDeta.FondoPension  and noFondo.EmpresaGrupoID = noPlanillaAportesDeta.eg_noFondo  " +
                    "   WHERE	glDocumentoControl.EmpresaID = @EmpresaID         "   +
                    "   		AND glDocumentoControl.DocumentoID = @DocumentoID          "   +
                    "   		AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin        "   +

                                            filtro + order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;

                DTO_noAportesPension doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noAportesPension(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportAportesPension");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte de aportes a salud
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Feha final del Reporte</param>
        /// <param name="empleadoFil">Filtro</param>
        /// <param name="orderBy">is Ordenado Por nombre?</param>
        /// <returns>Lista de dto</returns>
        public List<DTO_noAportesSalud> DAL_ReportesNomina_AportesSalud(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String terceroid, String nofondosaludid, String nocajaid)
        {
            try
            {
                List<DTO_noAportesSalud> results = new List<DTO_noAportesSalud>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                string order = "noEmpleado.EmpleadoID";
                if (orderBy)
                {
                    order = "ORDER BY noEmpleado.Descriptivo";
                }
                //if (empleadoFil != "")
                //{
                //    filtro = "AND noEmpleado.EmpleadoID = " + "'" + empleadoFil + " '";
                //}

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID = CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(nofondosaludid))
                {
                    filtro += " AND noLiquidacionesDocu.FondoSalud =  @FondoSalud ";
                    mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@FondoSalud"].Value = nofondosaludid;
                }

                if (!string.IsNullOrEmpty(nocajaid))
                {
                    filtro += " AND noLiquidacionesDocu.CajaNOID =  @CajaNOID ";
                    mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, UDT_CajaNOID.MaxLength);
                    mySqlCommandSel.Parameters["@CajaNOID"].Value = nocajaid;
                }

                mySqlCommandSel.CommandText =

                        "   SELECT	noFondo.FondoNOID,      " +
                        "   		noFondo.Descriptivo as FondoDesc,        " +
                        "   		noEmpleado.EmpleadoID as Cedula,      " +
                        "   		noEmpleado.Descriptivo as EmpleadoDesc,       " +
                        "   		noPlanillaAportesDeta.IngresoBaseSLD as Base,      " +
                        "   		noPlanillaAportesDeta.VlrTrabajadorSLD as Trabajador,      " +
                        "   		noPlanillaAportesDeta.VlrEmpresaSLD as Empresa,      " +
                        "   		(noPlanillaAportesDeta.VlrTrabajadorSLD + noPlanillaAportesDeta.VlrEmpresaSLD) as TotalAporte       " +
                        "   FROM noPlanillaAportesDeta			with(nolock)       " +
                        "   	INNER JOIN noLiquidacionesDocu	ON noPlanillaAportesDeta.NumeroDoc = noLiquidacionesDocu.NumeroDoc       " +
                        "   	INNER JOIN glDocumentoControl	ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc        " +
                        "   	INNER JOIN noEmpleado			ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID      " +
                        "   	INNER JOIN noFondo				ON noFondo.FondoNOID = noPlanillaAportesDeta.FondoSalud  AND noFondo.EmpresaGrupoID = @EmpresaID     " +
                        "   WHERE	glDocumentoControl.EmpresaID = @EmpresaID       " +
                        "   		AND glDocumentoControl.DocumentoID = @DocumentoID      " +
                        "   		AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin      " +

                    filtro + order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;

                DTO_noAportesSalud doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noAportesSalud(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportAportesPension");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte de aporte Voluntario a Solidaridad
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Feha final del Reporte</param>
        /// <param name="empleadoFil">Filtro</param>
        /// <param name="orderBy">is Ordenado Por nombre?</param>
        /// <returns>Lista de dto</returns>
        public List<DTO_noAportesVoluntariosPension> DAL_ReportesNomina_AporteVoluntarioPension(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String terceroid, String nofondosaludid, String nocajaid)
        {
            try 
            {
                List<DTO_noAportesVoluntariosPension> results = new List<DTO_noAportesVoluntariosPension>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                string order = "noEmpleado.EmpleadoID";
                if (orderBy)
                {
                    order = "ORDER BY noEmpleado.Descriptivo";
                }
                if (empleadoFil != "")
                {
                    filtro = "AND noEmpleado.EmpleadoID = " + "'" + empleadoFil + " '";
                }

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID = CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(nofondosaludid))
                {
                    filtro += " AND noLiquidacionesDocu.FondoSalud =  @FondoSalud ";
                    mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@FondoSalud"].Value = nofondosaludid;
                }

                if (!string.IsNullOrEmpty(nocajaid))
                {
                    filtro += " AND noLiquidacionesDocu.CajaNOID =  @CajaNOID ";
                    mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, UDT_CajaNOID.MaxLength);
                    mySqlCommandSel.Parameters["@CajaNOID"].Value = nocajaid;
                }

                mySqlCommandSel.CommandText =

                        "   SELECT	noEmpleado.EmpleadoID as Cedula,    " +
                        "   		noEmpleado.Descriptivo as EmpleadoDesc,      " +
                        "   		noFondo.FondoNOID, noFondo.Descriptivo as FondoDesc,      " +
                        "   		noLiquidacionesDetalle.Valor      " +
                        "   FROM noLiquidacionesDetalle with(nolock)      " +
                        "   	INNER JOIN noLiquidacionesDocu ON noLiquidacionesDetalle.NumeroDoc = noLiquidacionesDocu.NumeroDoc      " +
                        "   	INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc      " +
                        "   	INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noLiquidacionesDetalle.ConceptoNOID  AND noConceptoNOM.EmpresaGrupoID = @EmpresaID    " +
                        "   	INNER JOIN noEmpleado on noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID     " +
                        "   	INNER JOIN noFondo on noFondo.FondoNOID = noLiquidacionesDetalle.FondoNOID  AND noFondo.EmpresaGrupoID = @EmpresaID      " +
                        "   WHERE glDocumentoControl.EmpresaID = @EmpresaID     " +
                        "   	and  noLiquidacionesDetalle.FondoNOID is Not Null      " +
                        "   	and noLiquidacionesDetalle.OrigenConcepto != 2     " +

                                                filtro +
                                           order;
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;

                DTO_noAportesVoluntariosPension doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noAportesVoluntariosPension(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportAporteVoluntarioSolidaridad");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte de aporte ARP
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Feha final del Reporte</param>
        /// <param name="empleadoFil">Filtro</param>
        /// <param name="orderBy">is Ordenado Por nombre?</param>
        /// <returns>Lista de dto</returns>
        public List<DTO_noAportesArp> DAL_ReportesNomina_AportesARP(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String terceroid, String nofondosaludid, String nocajaid) 
        {
            try
            {
                List<DTO_noAportesArp> results = new List<DTO_noAportesArp>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                string order = "noEmpleado.EmpleadoID";
                if (orderBy)
                {
                    order = "ORDER BY noEmpleado.Descriptivo";
                }

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID = CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(nofondosaludid))
                {
                    filtro += " AND noLiquidacionesDocu.FondoSalud =  @FondoSalud ";
                    mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@FondoSalud"].Value = nofondosaludid;
                }

                if (!string.IsNullOrEmpty(nocajaid))
                {
                    filtro += " AND noLiquidacionesDocu.CajaNOID =  @CajaNOID ";
                    mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, UDT_CajaNOID.MaxLength);
                    mySqlCommandSel.Parameters["@CajaNOID"].Value = nocajaid;
                }

                mySqlCommandSel.CommandText =

                            "   SELECT	DISTINCT  noEmpleado.EmpleadoID as Cedula,  " +
                            "   		noEmpleado.Descriptivo as EmpleadoDesc,     " +
                            "   		noPlanillaAportesDeta.IngresoBaseARP as Base,    " +  
                            "   		noPlanillaAportesDeta.VlrARP as ValorArp,       " +
                            "   		noPlanillaAportesDeta.TarifaARP as Tarifa     " +
                            "   FROM noPlanillaAportesDeta  with(nolock)     " +
                            "   	INNER JOIN noLiquidacionesDocu ON noPlanillaAportesDeta.NumeroDoc = noLiquidacionesDocu.NumeroDoc       " +
                            "   	INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc       " +
                            "   	INNER JOIN noEmpleado on noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID     " +
                            "   WHERE   glDocumentoControl.EmpresaID = @EmpresaID     " +                            
                            "   	    AND glDocumentoControl.DocumentoID	= @DocumentoID  " +
                            "   	    AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin    " +
                                       filtro +   
                                       order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;

                DTO_noAportesArp doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noAportesArp(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportAporteVoluntarioSolidaridad");
                throw exception;
            }
        }

        #endregion

        #region Reportes Liquidacion

        /// <summary>
        /// Funcion que permite trael la informacion del cliente 
        /// </summary>
        /// <param name="documentoID">Docuemento desde el q se consulta</param>
        /// <param name="periodo">Peridod del modulo</param>
        /// <returns>Lista de dto con los headers del reporte de nomina detalle </returns>
        public List<DTO_ReportNominaInfoEmpleado> DAL_ReportesNomina_DetalleNominaHeader(int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, bool isAll, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                List<DTO_ReportNominaInfoEmpleado> results = new List<DTO_ReportNominaInfoEmpleado>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                String aux = terceroid;
                string filtro = "";

                if (!isAll)
                {
                    filtro = " AND glDocumentoControl.DocumentoID = @DocumentoID  ";
                }

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;                    
                }

                if (!string.IsNullOrEmpty(operacionnoid))
                {
                    filtro += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                    mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                    mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionnoid;
                }

                if (!string.IsNullOrEmpty(areafuncionalid))
                {
                    filtro += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                    mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                    mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areafuncionalid;
                }

                if (!string.IsNullOrEmpty(conceptonoid))
                {
                    filtro += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                    mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptonoid;
                }

                if (isPre)
                {
                    mySqlCommandSel.CommandText =
                    "SELECT distinct noEmpleado.EmpleadoID, noEmpleado.Descriptivo as EmpleadoDesc,  noEmpleado.BrigadaID, noEmpleado.FechaIngreso, " +
                      " noEmpleado.FechaRetiro, noEmpleado.CargoEmpID, rhCargos.Descriptivo as CargoEmpDesc, noEmpleado.OperacionNOID, noEmpleado.LugarGeograficoID, noEmpleado.Descriptivo,  " +
                      " glDocumentoControl.DocumentoID, noLiquidacionesDocu.*   " +
                    " FROM glDocumentoControl with(nolock)  " +
                    " INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc  " +
                    " INNER JOIN noLiquidacionesPreliminar ON  glDocumentoControl.NumeroDoc = noLiquidacionesPreliminar.NumeroDoc  " +
                    " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID  " +
                    " INNER JOIN rhCargos ON rhCargos.CargoEmpID = noEmpleado.CargoEmpID AND rhCargos.EmpresaGrupoID = @EmpresaID " +
                    " WHERE glDocumentoControl.EmpresaID = @EmpresaID " + filtro +
                    " AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin ";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                   " SELECT distinct noEmpleado.EmpleadoID, noEmpleado.Descriptivo as EmpleadoDesc,  noEmpleado.BrigadaID, noEmpleado.FechaIngreso, noEmpleado.FechaRetiro, noEmpleado.CargoEmpID, " +
                   " rhCargos.Descriptivo as CargoEmpDesc,  noEmpleado.OperacionNOID, noEmpleado.LugarGeograficoID, noEmpleado.Descriptivo, glDocumentoControl.DocumentoID, noLiquidacionesDocu.* " +
                   " FROM glDocumentoControl with(nolock) " +
                   " INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                   " INNER JOIN noLiquidacionesDetalle ON noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc " +
                   " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID AND noEmpleado.EmpresaGrupoID = @EmpresaID " +
                   " INNER JOIN rhCargos ON rhCargos.CargoEmpID = noEmpleado.CargoEmpID AND rhCargos.EmpresaGrupoID = @EmpresaID " +
                   " WHERE glDocumentoControl.EmpresaID = @EmpresaID " + filtro +
                   " AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin ";
                }
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                DTO_ReportNominaInfoEmpleado doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportNominaInfoEmpleado(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportDetalleNominaHeader");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que permite trael la informacion del cliente 
        /// </summary>
        /// <param name="documentoID">Docuemento desde el q se consulta</param>
        /// <param name="periodo">Peridod del modulo</param>
        /// <returns>Lista de dto con los headers del reporte de nomina detalle </returns>
        public List<DTO_noReportDetalleEmpleadoConcepto> DAL_ReportesNomina_DetailLiquidaciones(string empleadoID, int numeroDoc, int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, bool isAll, bool isPre)
        {
            try
            {
                List<DTO_noReportDetalleEmpleadoConcepto> results = new List<DTO_noReportDetalleEmpleadoConcepto>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                string tabla = "noLiquidacionesDetalle";
                if (!isAll)
                    filtro = " AND glDocumentoControl.DocumentoID = @DocumentoID ";
                if (isPre)
                    tabla = "noLiquidacionesPreliminar";

                mySqlCommandSel.CommandText =
                    " SELECT noConceptoNOM.ConceptoNOID, noConceptoNOM.Descriptivo as ConceptoNODesc, " +
                    " noLiquidacionesDocu.Dias1, " +
                      tabla + ".Base, " +
                    " ABS(SUM(" + tabla + ".Valor))as Valor, noConceptoNOM.Tipo " +
                    " FROM glDocumentoControl  with(nolock)  " +
                    " INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                    " INNER JOIN " + tabla + " ON noLiquidacionesDocu.NumeroDoc = " + tabla + ".NumeroDoc " +
                    " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID " +
                    "  INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = " + tabla + ".ConceptoNOID " + " and noConceptoNOM.EmpresaGrupoID = " + tabla + ".eg_noConceptoNOM " +
                    " WHERE glDocumentoControl.EmpresaID = @EmpresaID  " + filtro +
                    " AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin " +
                    " AND noEmpleado.EmpleadoID = @EmpleadoID " +
                    " GROUP BY  noConceptoNOM.ConceptoNOID, noConceptoNOM.Descriptivo,  noLiquidacionesDocu.Dias1, "+ tabla +".Base,noConceptoNOM.Tipo"; /*+
                    " ORDER BY noLiquidacionesDocu.NumeroDoc";*/

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@numeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@numeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                DTO_noReportDetalleEmpleadoConcepto doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noReportDetalleEmpleadoConcepto(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportDetalleNominaDetalle");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que permite trar el la informacion del cliente 
        /// </summary>
        /// <param name="documentoID">Docuemento desde el q se consulta</param>
        /// <param name="periodo">Peridod del modulo</param>
        /// <returns>Lista de dto con los headers del reporte de nomina detalle </returns>
        public List<DTO_noReportDetalleNominaXConcepto> DAL_ReportesNomina_XConcepto(int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, bool isAll, bool orderName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                List<DTO_noReportDetalleNominaXConcepto> results = new List<DTO_noReportDetalleNominaXConcepto>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string order = "";
                string filtro = "";
                string tabla = isPre ? "noLiquidacionesPreliminar" : "noLiquidacionesDetalle";

                if (!isAll)
                {
                    filtro = " AND glDocumentoControl.DocumentoID = @DocumentoID ";
                    if (orderName)
                    {
                        order = " ORDER BY noEmpleado.Descriptivo ";
                    }
                    else
                    {
                        order = " ORDER BY glDocumentoControl.TerceroID ";
                    }
                }

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(operacionnoid))
                {
                    filtro += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                    mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                    mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionnoid;
                }

                if (!string.IsNullOrEmpty(areafuncionalid))
                {
                    filtro += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                    mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                    mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areafuncionalid;
                }

                if (!string.IsNullOrEmpty(conceptonoid))
                {
                    filtro += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                    mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptonoid;
                }
              

                mySqlCommandSel.CommandText =

                            "   SELECT		"   +
                            "   	noEmpleado.Descriptivo as EmpleadoDesc,  		" +
                            "   	glDocumentoControl.TerceroID as EmpleadoID,		" +
                            "   	noConceptoNOM.ConceptoNOID, 		"   +
                            "   	noConceptoNOM.Descriptivo, 		"   +
                            "   	noLiquidacionesDetalle.Base, 		"   +
                            "   	noLiquidacionesDetalle.Valor AS Valor		" +
                            "   FROM		noLiquidacionesDetalle			"   +
                            "   INNER JOIN noLiquidacionesDocu		ON noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc 		"   +
                            "   INNER JOIN glDocumentoControl		ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc 	 		"   +
                            "   INNER JOIN noConceptoNOM			ON noConceptoNOM.ConceptoNOID = noLiquidacionesDetalle.ConceptoNOID 		"   +
                            "   										AND noConceptoNOM.EmpresaGrupoID = noLiquidacionesDetalle.eg_noConceptoNOM 		"   +
                            "   INNER JOIN noEmpleado				ON noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID AND noEmpleado.EmpresaGrupoID = @EmpresaID		" +
                            "   WHERE	glDocumentoControl.EmpresaID = @EmpresaID		"   +
                            "   		AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin  "   +
                            "   		AND glDocumentoControl.DocumentoID = @DocumentoID		"+
                            filtro + order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                DTO_noReportDetalleNominaXConcepto doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noReportDetalleNominaXConcepto(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportDetalleNominaXConcepto");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que ejecuta trae la info resumida por concepto
        /// </summary>
        /// <param name="documentoID">documento por el cual se consulta </param>
        /// <param name="periodo">Periodod del modulo</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isAll">Es todos sin documento de filtro</param>
        /// <param name="orderName">Ordena Alfabeticamente o por Codigo</param>
        /// <returns></returns>
        public List<DTO_noReportNominaResumidoXConcepto> DAL_ReportesNomina_TotalXConcepto(int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, bool isAll, bool orderName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                List<DTO_noReportNominaResumidoXConcepto> results = new List<DTO_noReportNominaResumidoXConcepto>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string order = "";
                string filtro = "";
                string tabla = "noLiquidacionesDetalle";

                if (isPre)
                {
                    tabla = "noLiquidacionesPreliminar";
                }
                if (!isAll)
                {
                    filtro = " AND glDocumentoControl.DocumentoID = @DocumentoID ";
                    if (orderName)
                    {
                        order = " order by noConceptoNOM.Descriptivo DESC";
                    }
                    else
                    {
                        order = " order by noConceptoNOM.Tipo DESC ";
                    }
                }

                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(operacionnoid))
                {
                    filtro += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                    mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                    mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionnoid;
                }

                if (!string.IsNullOrEmpty(areafuncionalid))
                {
                    filtro += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                    mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                    mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areafuncionalid;
                }

                if (!string.IsNullOrEmpty(conceptonoid))
                {
                    filtro += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                    mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptonoid;
                }

                mySqlCommandSel.CommandText =
                    " SELECT " + tabla + ".ConceptoNOID, noConceptoNOM.Descriptivo, noConceptoNOM.Tipo, " +
                    " ABS(SUM(" + tabla + ".Base)) as Base, ABS(SUM(" + tabla + ".Valor))as Valor " +
                    " FROM " + tabla + " with(nolock) " +
                    " INNER JOIN noLiquidacionesDocu ON " + tabla + ".NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                    " INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                    " INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = " + tabla + ".ConceptoNOID  " +  " and noConceptoNOM.EmpresaGrupoID = " + tabla + ".eg_noConceptoNOM " +
                    " WHERE glDocumentoControl.EmpresaID = @EmpresaID " + filtro +
                    " AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin " +
                    " GROUP BY " + tabla + ".ConceptoNOID, noConceptoNOM.Descriptivo, noConceptoNOM.Tipo " +
                    order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                DTO_noReportNominaResumidoXConcepto doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noReportNominaResumidoXConcepto(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReportDetalleResumidoXConcepto");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que ejecuta trae la info resumida por concepto
        /// </summary>
        /// <param name="documentoID">documento por el cual se consulta </param>
        /// <param name="periodo">Periodod del modulo</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="isAll">Es todos sin documento de filtro</param>
        /// <param name="orderName">Ordena Alfabeticamente o por Codigo</param>
        /// <returns></returns>
        public List<DTO_noReportResumidoXEmpleado> DAL_ReportesNomina_Detalle(int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, bool isAll, bool orderName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try 
            {
                List<DTO_noReportResumidoXEmpleado> results = new List<DTO_noReportResumidoXEmpleado>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string order = " ";
                string query = " ";
                string filtro = " ";
                string tabla = "noLiquidacionesDetalle";

                if (isPre)
                    tabla = "noLiquidacionesPreliminar";

                if (!isAll)
                    filtro = " AND glDocumentoControl.DocumentoID = @DocumentoID ";
                if (orderName)
                    order = " ORDER BY noEmpleado.Descriptivo ";                   
                else
                    order = " ORDER BY noEmpleado.EmpleadoID ";


                if (!string.IsNullOrEmpty(terceroid))
                {
                    filtro += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroid;
                }

                if (!string.IsNullOrEmpty(operacionnoid))
                {
                    filtro += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                    mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                    mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionnoid;
                }

                if (!string.IsNullOrEmpty(areafuncionalid))
                {
                    filtro += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                    mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                    mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areafuncionalid;
                }

                if (!string.IsNullOrEmpty(conceptonoid))
                {
                    filtro += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                    mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                    mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptonoid;
                }


                query =
               " SELECT noEmpleado.EmpleadoID, noEmpleado.Descriptivo as EmpleadoDesc, noEmpleado.CuentaAbono, " +
               " ABS(SUM(" + tabla + ".Valor))as Valor " +
               " FROM glDocumentoControl  with(nolock) " +
               " INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc  " +
               " INNER JOIN " + tabla + " ON " + tabla + ".NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
               " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID and noEmpleado.EmpresaGrupoID = @EmpresaID" +
               " INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = " + tabla + ".ConceptoNOID " + " and noConceptoNOM.EmpresaGrupoID = " + tabla + ".eg_noConceptoNOM " +
               " WHERE glDocumentoControl.EmpresaID = @EmpresaID" + filtro +
               " AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin " +
               " GROUP BY noEmpleado.EmpleadoID, noEmpleado.Descriptivo, noEmpleado.CuentaAbono" +
                order;

                mySqlCommandSel.CommandText = query;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                DTO_noReportResumidoXEmpleado doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noReportResumidoXEmpleado(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReporteTotalConceptoXEmpleado");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte de vacaciones
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Feha final del Reporte</param>
        /// <param name="empleadoFil">Filtro</param>
        /// <param name="orderBy">is Ordenado Por nombre?</param>
        /// <returns>Lista de dto</returns>
        public List<DTO_noReportVacionesPagadas> DAL_ReportesNomina_VacacionesPagadas(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String empleadoid)
        {
            try 
            {
                List<DTO_noReportVacionesPagadas> results = new List<DTO_noReportVacionesPagadas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                string order = " ";
                if (orderBy)
                    order = "ORDER BY noEmpleado.Descriptivo";

                if (!string.IsNullOrEmpty(empleadoFil))
                    filtro = "  AND noEmpleado.EmpleadoID = " + "'" + empleadoFil + " '  ";

                if (!string.IsNullOrEmpty(empleadoid))
                {
                    filtro = "and Ctrl.TerceroID = @TerceroID";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = empleadoid;
                }

                mySqlCommandSel.CommandText = " SELECT " +
                                              " FechaIni1 , FechaFin1, FechaIni2, FechaFin2, Ctrl.TerceroID as ConceptoNOID, noEmpleado.Descriptivo, Ctrl.Fecha , " +
                                              " noLiquidacionesDocu.Dias1, 	noLiquidacionesDocu.Dias2" +
                                              " FROM noLiquidacionesDocu with(nolock) " +
                                              " INNER JOIN glDocumentoControl Ctrl ON Ctrl.NumeroDoc = noLiquidacionesDocu.NumeroDoc" +
                                              " INNER JOIN noEmpleado ON noEmpleado.EmpleadoID = Ctrl.TerceroID  and noEmpleado.EmpresaGrupoID = @EmpresaID " +
                                              " WHERE  " +
                    //" Ctrl.Fecha BETWEEN @fechaIni AND @fechaFin " +
                                              " Ctrl.EmpresaID = @EmpresaID " +
                                              " AND Ctrl.DocumentoID = @DocumentoID AND noLiquidacionesDocu.PagadoInd = 0 " + filtro +                                           
                                                order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                //mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                //mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Vacaciones;

                DTO_noReportVacionesPagadas doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noReportVacionesPagadas(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReporteTotalConceptoXEmpleado");
                throw exception;
            }
        }

        #endregion

        #region Reportes de Prestamos
        /// <summary>
        /// Funcion que carga una lista de dto_noReportePrestamo
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="orderByName">Orena por nombre?</param>
        /// <returns>Lista de dto_noReportePrestamo</returns>
        public List<DTO_noReportPrestamo> DAL_ReportesNomina_Prestamo(DateTime fechaIni, DateTime fechaFin, bool orderByName, String empleadoid)
        {
            try
            {
                List<DTO_noReportPrestamo> results = new List<DTO_noReportPrestamo>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string order = " ORDER BY noPrestamo.EmpleadoID	";
                string filter = string.Empty;

                if (!string.IsNullOrEmpty(empleadoid))
                {
                    filter = "AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = empleadoid;                    
                }
                if (orderByName)
                    order = " ORDER BY noEmpleado.Descriptivo ";

                mySqlCommandSel.CommandText = " SELECT noPrestamo.Numero, " +
                                              " noPrestamo.EmpleadoID, " +
                                              " noEmpleado.Descriptivo , " +
                                              " noPrestamo.VlrPrestamo, " +
                                              " noPrestamo.VlrCuota,noPrestamo.VlrAbono " +
                                              " from noPrestamo with(nolock) " +
                                              " INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noPrestamo.DocPrestamo " +
                                              " INNER JOIN noEmpleado ON noEmpleado.EmpleadoID = noPrestamo.EmpleadoID and noEmpleado.EmpresaGrupoID =  @EmpresaID " +
                                              " WHERE noPrestamo.EmpresaID	= @EmpresaID " +
                                              " AND glDocumentoControl.Fecha BETWEEN @fechaIni AND @fechaFin " + filter +
                                              order;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                DTO_noReportPrestamo doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_noReportPrestamo(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_reportPrestamo");
                throw exception;
            }
        }

        #endregion
        
        #region Nuevos Reportes

        /// <summary>
        /// Reporte Boleta de Pago Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Liquidación</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="getAll">Todos los Empleados</param>
        /// <returns>DataSet con Resultados</returns>
        public DataSet DAL_ReportesNomina_BoletaPago(DateTime periodo, string empleadoID,  bool getAll)
        {
            DataSet results = null;
                
            SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
            mySqlCommandSel.Transaction = base.MySqlConnectionTx;

            string empleado = " AND noEmpleado.EmpleadoID = @EmpleadoID	";
            if (getAll)
                empleado = string.Empty;

            #region CommanText

            mySqlCommandSel.CommandText = " SELECT noEmpleado.* " +
                        " FROM noLiquidacionesDocu  " +
                        " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID " +
                        empleado +

                        " SELECT  noEmpleado.EmpleadoID,  " +
                                " noLiquidacionesDetalle.ConceptoNOID, " +
                                " noConceptoNOM.Descriptivo as ConceptoNOIDDesc, " +
                                " noLiquidacionesDetalle.Base, " +
                                " ABS(noLiquidacionesDetalle.Valor) as Valor " +
                        " FROM noLiquidacionesDetalle  " +
                        " INNER JOIN noLiquidacionesDocu on noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc " +
                        " AND noLiquidacionesDocu.EmpresaID = @EmpresaID AND noLiquidacionesDocu.PagadoInd = 0 " +
                        " INNER JOIN glDocumentoControl on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                        " AND glDocumentoControl.EmpresaID = @EmpresaID AND glDocumentoControl.Estado = 3 " +
                        " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID " +
                        " AND noEmpleado.EmpresaGrupoID = @EmpresaID " +
                        " INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noLiquidacionesDetalle.ConceptoNOID " +
                        " AND noConceptoNOM.EmpresaGrupoID = @EmpresaID " +
                        " WHERE glDocumentoControl.DocumentoID IN (81,82,83,84,85) " +
                        " AND glDocumentoControl.PeriodoDoc = @PeriodoID " +                         
                        " AND noConceptoNOM.Tipo = 1 /* --Devengo */ " +
                        empleado +

                        " SELECT  noEmpleado.EmpleadoID,  " +
                        " 		noLiquidacionesDetalle.ConceptoNOID, " +
                        " 		noConceptoNOM.Descriptivo as ConceptoNOIDDesc, " +
                        " 		noLiquidacionesDetalle.Base, " +
                        " 		ABS(noLiquidacionesDetalle.Valor) as Valor " +
                        " FROM noLiquidacionesDetalle  " +
                        " INNER JOIN noLiquidacionesDocu on noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc " +
                        " AND noLiquidacionesDocu.EmpresaID = @EmpresaID AND noLiquidacionesDocu.PagadoInd = 0 " +
                        " INNER JOIN glDocumentoControl on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                        " AND glDocumentoControl.EmpresaID = @EmpresaID AND glDocumentoControl.Estado = 3 " +
                        " INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID " +
                        " AND noEmpleado.EmpresaGrupoID = @EmpresaID " +
                        " INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noLiquidacionesDetalle.ConceptoNOID " +
                        " AND noConceptoNOM.EmpresaGrupoID = @EmpresaID " +
                        " WHERE glDocumentoControl.DocumentoID IN (81,82,83,84,85) -- Documentos de Nomina " +
                        " AND glDocumentoControl.PeriodoDoc = @PeriodoID " +
                        " AND noConceptoNOM.Tipo = 2 /* --Deduccion*/ " +
                        empleado;

            #endregion

            mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
            mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
            mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);

            mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
            mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
            mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleado;

            SqlDataAdapter da = new SqlDataAdapter(mySqlCommandSel);
            da.Fill(results, "Report_BoletaPago");
            return results;
        }


        #endregion

        #region Excel

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="operacionNoID">Operacion Nomina</param>
        /// <param name="conceptoNoID">Concepto Nomina</param>
        /// <param name="areaFuncID">Area Funcional</param>
        /// <param name="fondoID">Fondo Nom</param>
        /// <param name="cajaID">Caja Nomina</param>
        /// <param name="otroFilter">Filtro adicional</param>
        /// <param name="agrup">Agrupar u ordenar</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_Reportes_No_NominaToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID,string operacionNoID,
                                                         string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();
                string order = string.Empty;
                string where = string.Empty;

                #region Prestamo
                if (documentoID == AppReports.noPrestamo)
                {
                    order = " ORDER BY noPrestamo.EmpleadoID	";
                    where = string.Empty;
                    bool orderByName = (bool)otroFilter;

                    if (!string.IsNullOrEmpty(empleadoID))
                    {
                        where = "AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END";
                        mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@TerceroID"].Value = empleadoID;
                    }
                    if (orderByName)
                        order = " ORDER BY noEmpleado.Descriptivo ";

                    mySqlCommandSel.CommandText = " SELECT noPrestamo.Numero, " +
                                                  " noPrestamo.EmpleadoID, " +
                                                  " noEmpleado.Descriptivo , " +
                                                  " noPrestamo.VlrPrestamo, " +
                                                  " noPrestamo.VlrCuota,noPrestamo.VlrAbono " +
                                                  " from noPrestamo with(nolock) " +
                                                  " INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noPrestamo.DocPrestamo " +
                                                  " INNER JOIN noEmpleado ON noEmpleado.EmpleadoID = noPrestamo.EmpleadoID and noEmpleado.EmpresaGrupoID =  @EmpresaID " +
                                                  " WHERE noPrestamo.EmpresaID	= @EmpresaID " +
                                                  " AND glDocumentoControl.Fecha BETWEEN @fechaIni AND @fechaFin " + where +
                                                  order;

                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                }
                #endregion
                #region Aportes
                else if (documentoID == AppReports.noAportes)
                {
                    order = " ORDER BY noEmpleado.Descriptivo ";

                    if (!string.IsNullOrEmpty(empleadoID))
                    {
                        where = " AND noEmpleado.EmpleadoID = @EmpleadoID ";
                        mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                        mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                    }                            
                    if (!string.IsNullOrEmpty(terceroID))
                    {
                        where += " AND glDocumentoControl.TerceroID = CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                        mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                    }                    
                    if (!string.IsNullOrEmpty(fondoID))
                    {
                        where += " AND noLiquidacionesDocu.FondoSalud =  @FondoSalud ";
                        mySqlCommandSel.Parameters.Add("@FondoSalud", SqlDbType.Char, UDT_FondoNOID.MaxLength);
                        mySqlCommandSel.Parameters["@FondoSalud"].Value = fondoID;
                    }
                    if (!string.IsNullOrEmpty(cajaID))
                    {
                        where += " AND noLiquidacionesDocu.CajaNOID =  @CajaNOID ";
                        mySqlCommandSel.Parameters.Add("@CajaNOID", SqlDbType.Char, UDT_CajaNOID.MaxLength);
                        mySqlCommandSel.Parameters["@CajaNOID"].Value = cajaID;
                    }

                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int); 
                    #endregion

                    #region Asignacion de valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.PlanillaAutoLiquidAportes;

                    
                    #endregion                   

                    #region CommandText
                    if (tipoReporte == 1 || tipoReporte == 0) // Pensión
                    {
                        #region Pension
                        mySqlCommandSel.CommandText =

                            "   SELECT noFondo.FondoNOID,       " +
                            "   		noFondo.Descriptivo						as FondoDesc,         " +
                            "   		noEmpleado.EmpleadoID					as Cedula,        " +
                            "   		noEmpleado.Descriptivo					as EmpleadoDesc,         " +
                            "   		noPlanillaAportesDeta.IngresoBasePEN	as Base,        " +
                            "   		noPlanillaAportesDeta.VlrEmpresaPEN		as AporteEmpresa,         " +
                            "   		noPlanillaAportesDeta.VlrTrabajadorPEN	as AporteEmpleado,        " +
                            "   		noPlanillaAportesDeta.VlrSolidaridad + noPlanillaAportesDeta.VlrSubsistencia as AporteSolidaridad,         " +
                            "   		(noPlanillaAportesDeta.VlrTrabajadorPEN + noPlanillaAportesDeta.VlrTrabajadorSLD  + noPlanillaAportesDeta.VlrSolidaridad)  as TotalAporte  " +
                            "   FROM noPlanillaAportesDeta			with(nolock)         " +
                            "   	INNER JOIN noLiquidacionesDocu	ON noPlanillaAportesDeta.NumeroDoc	= noLiquidacionesDocu.NumeroDoc         " +
                            "   	INNER JOIN glDocumentoControl	ON glDocumentoControl.NumeroDoc		= noLiquidacionesDocu.NumeroDoc         " +
                            "   	INNER JOIN noEmpleado			ON noEmpleado.ContratoNOID			= noLiquidacionesDocu.ContratoNOID   AND noEmpleado.EmpresaGrupoID = @EmpresaID       " +
                            "   	INNER JOIN noFondo				ON noFondo.FondoNOID				= noPlanillaAportesDeta.FondoPension  and noFondo.EmpresaGrupoID = noPlanillaAportesDeta.eg_noFondo  " +
                            "   WHERE	glDocumentoControl.EmpresaID = @EmpresaID         " +
                            "   		AND glDocumentoControl.DocumentoID = @DocumentoID          " +
                            "   		AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin   " +
                            where + order;
                        #endregion
                    }
                    else if (tipoReporte == 2) //  Salud
                    {
                        #region Salud
                        mySqlCommandSel.CommandText =

                                "   SELECT	noFondo.FondoNOID,      " +
                                "   		noFondo.Descriptivo as FondoDesc,        " +
                                "   		noEmpleado.EmpleadoID as Cedula,      " +
                                "   		noEmpleado.Descriptivo as EmpleadoDesc,       " +
                                "   		noPlanillaAportesDeta.IngresoBaseSLD as Base,      " +
                                "   		noPlanillaAportesDeta.VlrTrabajadorSLD as Trabajador,      " +
                                "   		noPlanillaAportesDeta.VlrEmpresaSLD as Empresa,      " +
                                "   		(noPlanillaAportesDeta.VlrTrabajadorSLD + noPlanillaAportesDeta.VlrEmpresaSLD) as TotalAporte       " +
                                "   FROM noPlanillaAportesDeta			with(nolock)       " +
                                "   	INNER JOIN noLiquidacionesDocu	ON noPlanillaAportesDeta.NumeroDoc = noLiquidacionesDocu.NumeroDoc       " +
                                "   	INNER JOIN glDocumentoControl	ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc        " +
                                "   	INNER JOIN noEmpleado			ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID      " +
                                "   	INNER JOIN noFondo				ON noFondo.FondoNOID = noPlanillaAportesDeta.FondoSalud  AND noFondo.EmpresaGrupoID = @EmpresaID     " +
                                "   WHERE	glDocumentoControl.EmpresaID = @EmpresaID       " +
                                "   		AND glDocumentoControl.DocumentoID = @DocumentoID      " +
                                "   		AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin      " +
                            where + order;
                        #endregion
                    }
                    else if (tipoReporte == 3) // Pension Voluntaria
                    {
                        #region Voluntario Pension
                        mySqlCommandSel.CommandText =

                            "   SELECT	noEmpleado.EmpleadoID as Cedula,    " +
                            "   		noEmpleado.Descriptivo as EmpleadoDesc,      " +
                            "   		noFondo.FondoNOID, noFondo.Descriptivo as FondoDesc,      " +
                            "   		noLiquidacionesDetalle.Valor      " +
                            "   FROM noLiquidacionesDetalle with(nolock)      " +
                            "   	INNER JOIN noLiquidacionesDocu ON noLiquidacionesDetalle.NumeroDoc = noLiquidacionesDocu.NumeroDoc      " +
                            "   	INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc      " +
                            "   	INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = noLiquidacionesDetalle.ConceptoNOID  AND noConceptoNOM.EmpresaGrupoID = @EmpresaID    " +
                            "   	INNER JOIN noEmpleado on noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID     " +
                            "   	INNER JOIN noFondo on noFondo.FondoNOID = noLiquidacionesDetalle.FondoNOID  AND noFondo.EmpresaGrupoID = @EmpresaID      " +
                            "   WHERE glDocumentoControl.EmpresaID = @EmpresaID     " +
                            "   	and  noLiquidacionesDetalle.FondoNOID is Not Null      " +
                            "   	and noLiquidacionesDetalle.OrigenConcepto != 2     " +
                            where + order;
                        #endregion
                    }
                    else if (tipoReporte == 4) //ARP
                    {
                        #region ARP
                        mySqlCommandSel.CommandText =

                                    "   SELECT	DISTINCT  noEmpleado.EmpleadoID as Cedula,  " +
                                    "   		noEmpleado.Descriptivo as EmpleadoDesc,     " +
                                    "   		noPlanillaAportesDeta.IngresoBaseARP as Base,    " +
                                    "   		noPlanillaAportesDeta.VlrARP as ValorArp,       " +
                                    "   		noPlanillaAportesDeta.TarifaARP as Tarifa     " +
                                    "   FROM noPlanillaAportesDeta  with(nolock)     " +
                                    "   	INNER JOIN noLiquidacionesDocu ON noPlanillaAportesDeta.NumeroDoc = noLiquidacionesDocu.NumeroDoc       " +
                                    "   	INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc       " +
                                    "   	INNER JOIN noEmpleado on noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID     " +
                                    "   WHERE   glDocumentoControl.EmpresaID = @EmpresaID     " +
                                    "   	    AND glDocumentoControl.DocumentoID	= @DocumentoID  " +
                                    "   	    AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin    " +
                                               where +
                                               order;
                        #endregion
                    }
                    else if (tipoReporte == 5) //Caja Compensacion
                    {
                        #region Caja Compensacion

                        mySqlCommandSel.CommandText = "   SELECT   glDocumentoControl.PeriodoDoc, " +
                                                        "          EMP.TerceroID 		AS Cedula,	 " +
                                                        "          EMP.Descriptivo		AS Nombre, " +                                                        
                                                        "          DET.CajaNOID, " +
                                                        "          CAJ.Descriptivo		AS Nombre_Caja, " +                                                        
                                                        "          DET.IngresoBasePEN 	AS Sueldo, " +
                                                        "          DET.TarifaCCF		AS Tarifa_Caja, " +
                                                        "          DET.VlrCCF			AS Vlr_Caja, " +
                                                        "          DET.TarifaIBF		AS Tarifa_ICBF, " +
                                                        "          DET.VlrIBF			AS Vlr_ICBF, " +
                                                        "          DET.TarifaSEN		AS Tarifa_Sena, " +
                                                        "          DET.VlrSEN			AS Vlr_Sena " +
                                                        "  FROM noPlanillaAportesDeta			DET	with(nolock)    " +
                                                        "      INNER JOIN noLiquidacionesDocu	DOC ON DET.NumeroDoc = DOC.NumeroDoc    " +
                                                        "      INNER JOIN glDocumentoControl	ON glDocumentoControl.NumeroDoc = DOC.NumeroDoc AND DET.EmpleadoID = glDocumentoControl.TerceroID " +
                                                        "      INNER JOIN noEmpleado			EMP ON EMP.ContratoNOID = DOC.ContratoNOID  " +
                                                        "              AND EMP.EmpresaGrupoID = DET.eg_noEmpleado " +
                                                        "      INNER JOIN noCaja				CAJ ON DET.CajaNOID = CAJ.CajaNOID  " +
                                                        "              AND DET.EmpresaID = CAJ.EmpresaGrupoID " +
                                                        "  WHERE EMP.EmpresaGrupoID = @EmpresaID   	 " +
                                                        "      AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin    " +
                                                        where +
                                                        "  ORDER BY EMP.TerceroID  ";
                        #endregion
                    }
                    if (tipoReporte == 6)
                    {

                    } 
                    #endregion
                }
                #endregion
                #region Vacaciones
                else if (documentoID == AppReports.noVacacionesParameter)
                {
                    #region Config Filtros
                    order = "ORDER BY noEmpleado.Descriptivo";

                    if (!string.IsNullOrEmpty(empleadoID))
                    {
                        where = " AND noEmpleado.EmpleadoID = @EmpleadoID ";
                        mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                        mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                    }
                    if (!string.IsNullOrEmpty(empleadoID))
                    {
                        where = "and Ctrl.TerceroID = @TerceroID";
                        mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@TerceroID"].Value = empleadoID;
                    } 
                    #endregion
                    #region CommandText
                    if (tipoReporte == 1) // Vacaciones Pagadas
                    {
                        mySqlCommandSel.CommandText = " SELECT " +
                                                      " FechaIni1 , FechaFin1, FechaIni2, FechaFin2, Ctrl.TerceroID as Cedula, noEmpleado.Descriptivo, Ctrl.Fecha , " +
                                                      " noLiquidacionesDocu.Dias1, 	noLiquidacionesDocu.Dias2" +
                                                      " FROM noLiquidacionesDocu with(nolock) " +
                                                      " INNER JOIN glDocumentoControl Ctrl ON Ctrl.NumeroDoc = noLiquidacionesDocu.NumeroDoc" +
                                                      " INNER JOIN noEmpleado ON noEmpleado.EmpleadoID = Ctrl.TerceroID  and noEmpleado.EmpresaGrupoID = @EmpresaID " +
                                                      " WHERE  " +
                                                        //" Ctrl.Fecha BETWEEN @fechaIni AND @fechaFin " +
                                                      " Ctrl.EmpresaID = @EmpresaID " +
                                                      " AND Ctrl.DocumentoID = @DocumentoID AND noLiquidacionesDocu.PagadoInd = 0 " +
                                                       where + order;
                    }
                    else if (tipoReporte == 2) // Vacaciones Pendientes
                    {
                        mySqlCommandSel.CommandText = " SELECT  " +
                                                       "        Ctrl.TerceroID AS Cedula, " +
                                                       "        EPL.Descriptivo AS Nombre, " +                                                     
                                                       "        DOC.FechaIni2," +
                                                       "        DOC.FechaFin2," +
                                                       "        DOC.Dias1" +
                                                       " FROM noLiquidacionesDocu	DOC with (nolock)  " +
                                                       "        INNER JOIN glDocumentoControl	Ctrl  with (nolock) ON Ctrl.NumeroDoc = DOC.NumeroDoc " +
                                                       "        INNER JOIN noEmpleado	EPL  with (nolock) ON EPL.EmpleadoID = Ctrl.TerceroID  and EPL.EmpresaGrupoID = @EmpresaID  " +
                                                       " WHERE   Ctrl.EmpresaID = @EmpresaID  " +
                                                       "        AND Ctrl.DocumentoID = @DocumentoID   AND DOC.PagadoInd = 1 " +
                                                       where +
                                                       " ORDER BY	Ctrl.TerceroID, EPL.Descriptivo";
                    }
                    else if (tipoReporte == 3) // Vacaciones Documento
                    {

                        mySqlCommandSel.CommandText = " SELECT  " +
                                                        "    DISTINCT " +
                                                        "    EPL.TerceroID AS Cedula, " +
                                                        "    EPL.Descriptivo AS Nombre,	 " +                                                      
                                                        "    DOC.FechaIni1, " +
                                                        "    DOC.FechaFin1, " +
                                                        "    DOC.FechaFin1 + 1 AS FechaIngreso, " +
                                                        "    DOC.FechaIni3 AS PeriodoPagoInicial, " +
                                                        "    DOC.FechaFin3 AS PeriodoPagoFinal, " +
                                                        "    DOC.FechaIni2, " +
                                                        "    DOC.FechaFin2, " +
                                                        "    DOC.Fecha1 AS FechaReintegro, " +
                                                        "    DOC.Dias1, " +
                                                        "    DOC.Dias2, " +
                                                        "    DOC.DatoAdd1 AS Resolucion, " +
                                                        "   (DOC.SueldoML + DOC.SueldoME) AS Salario, " +
                                                        "    NOM.ConceptoNOGrupoID AS Codigo, " +
                                                        "    NOM.Descriptivo AS Concepto, " +
                                                        "    DET.Dias, " +
                                                        "    DET.Base, " +
                                                        "    CASE WHEN DET.Valor < 0 THEN DET.Valor * (-1) ELSE DET.Valor END AS VALOR, " +
                                                        "    CASE WHEN DET.Valor > 0 THEN DET.Valor ELSE 0 END AS DEVENGOS, " +
                                                        "    CASE WHEN DET.Valor < 0 THEN DET.Valor ELSE 0 END AS DEDUCCIONES, " +
                                                        "    CASE WHEN DET.Valor > 0 THEN 'DEVENGOS' ELSE 'DEDUCCIONES' END AS NomTipo " +
                                                        "FROM		noLiquidacionesDetalle	DET " +
                                                        "INNER JOIN noLiquidacionesDocu	DOC WITH (NOLOCK) ON DOC.NumeroDoc = DET.NumeroDoc  " +
                                                        "INNER JOIN glDocumentoControl		Ctrl WITH (NOLOCK) ON Ctrl.NumeroDoc = DOC.NumeroDoc 	 " +
                                                        "INNER JOIN noEmpleado			EPL WITH (NOLOCK) ON EPL.TerceroID = Ctrl.TerceroID	AND EPL.eg_coTercero = Ctrl.EmpresaID  " +
                                                        "INNER JOIN noConceptoNOM		NOM WITH (NOLOCK) ON NOM.ConceptoNOID = DET.ConceptoNOID AND NOM.eg_noConceptoGrupoNOM = DET.eg_noConceptoNOM  " +
                                                        "WHERE   Ctrl.EmpresaID = @EmpresaID AND Ctrl.DocumentoID = @DocumentoID " +
                                                        where +
                                                        "        AND DOC.PagadoInd = 0 " +
                                                        "        AND DOC.FechaIni2 =  CASE WHEN @fechaIni is null or @fechaIni = '' THEN DOC.FechaIni2 ELSE @fechaIni END  " +
                                                        "ORDER BY EPL.TerceroID,EPL.Descriptivo ";
                    } 
                    #endregion
                    #region parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int); 
                    #endregion
                    #region Asignacion de Valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Vacaciones;  
                    #endregion
                }
                #endregion
                #region Documentos y Certificados
                else if (documentoID == AppReports.noDocumentosYCertificados)
                {
                    #region CommandText
                    mySqlCommandSel.CommandText = " SELECT Ctrl.NumeroDoc, " +
                                                "        Ctrl.PeriodoDoc, " +
                                                "        DOC.Quincena, " +
                                                "        EPL.EmpleadoID, " +
                                                "        EPL.Descriptivo, " +
                                                "        DOC.SueldoML, " +
                                                "        EPL.PorcentajeRteFte, " +
                                                "        EPL.ProcedimientoRteFte, " +
                                                "        EPL.CentroCostoID, " +
                                                "        EPL.eg_glLugarGeografico, " +
                                                "        EPL.OperacionNOID, " +
                                                "        CAR.Descriptivo AS Cargo, " +
                                                "       DET.ConceptoNOID,  " +
                                                "       NOM.Descriptivo as ConceptoNOIDDesc,  " +
                                                "        DET.Base,  " +
                                                "        CASE WHEN DET.Valor > 0 THEN DET.Valor ELSE 0 END AS Devengo, " +
                                                "        CASE WHEN DET.Valor < 0 THEN DET.Valor ELSE 0	END AS Deduccion	 " +
                                                " FROM		noLiquidacionesDetalle	DET " +
                                                "   INNER JOIN noLiquidacionesDocu		DOC ON DOC.NumeroDoc = DET.NumeroDoc  " +
                                                "   INNER JOIN glDocumentoControl		Ctrl ON Ctrl.NumeroDoc = DOC.NumeroDoc  " +
                                                "   INNER JOIN noEmpleado				EPL ON EPL.TerceroID = Ctrl.TerceroID " +
                                                "				                            AND EPL.eg_coTercero = Ctrl.EmpresaID  " +
                                                "   INNER JOIN noConceptoNOM			NOM ON NOM.ConceptoNOID = DET.ConceptoNOID  " +
                                                "			                            AND NOM.eg_noConceptoGrupoNOM = DET.eg_noConceptoNOM  " +
                                                "   INNER JOIN rhCargos CAR					ON  CAR.CargoEmpID = DOC.CargoEmpID " +
                                                "			                            AND  CAR.EmpresaGrupoID = @EmpresaID " +
                                                " WHERE	Ctrl.EmpresaID = @EmpresaID " +
                                                "        AND Ctrl.DocumentoID = @Documento " +
                                                "        AND MONTH (Ctrl.PeriodoDoc) = @Mes " +
                                                "        AND YEAR (Ctrl.PeriodoDoc) = @Año " +
                                                "        AND EPL.EmpleadoID	=  (CASE WHEN @Empleado != '' THEN @Empleado ELSE EPL.EmpleadoID END)  " +
                                                "        AND DOC.Quincena = @Quincena " +
                                                " ORDER BY EPL.EmpleadoID  ";
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Mes", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Quincena", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                    #endregion
                    #region Asignacion valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = otroFilter;
                    mySqlCommandSel.Parameters["@Mes"].Value = fechaIni.Value.Month;
                    mySqlCommandSel.Parameters["@Año"].Value = fechaIni.Value.Year;
                    mySqlCommandSel.Parameters["@Quincena"].Value = (byte)otroFilter;
                    mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                    #endregion 
                }

                #endregion
                #region Detalle Nómina
                else if (documentoID == AppReports.noDetalleNomina)
                {
                    #region Filtros generales
                    bool isPre = Convert.ToBoolean(otroFilter); //valida la tabla a consultar
                    bool isAll = Convert.ToBoolean(cajaID); // valida si trae trae todo o no
                    bool orderName = Convert.ToBoolean(fondoID); // valida si ordena por nombre 
                    int docNomina = Convert.ToInt32(romp); // valida si ordena por nombre 

                    string tabla = isPre ? "noLiquidacionesPreliminar" : "noLiquidacionesDetalle";
                    where = !isAll ? " AND glDocumentoControl.DocumentoID = @DocumentoID " : string.Empty;
                    
                    #endregion

                    if (tipoReporte == 1) //Nomina Detalle
                    { 
                        #region CommandText
                        mySqlCommandSel.CommandText =
                        " SELECT noConceptoNOM.ConceptoNOID, noConceptoNOM.Descriptivo as ConceptoNODesc,noLiquidacionesDocu.Dias1, " +
                            tabla + ".Base, ABS(SUM(" + tabla + ".Valor))as Valor, noConceptoNOM.Tipo " +
                        " FROM glDocumentoControl  with(nolock)  " +
                        "   INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                        "   INNER JOIN " + tabla + " ON noLiquidacionesDocu.NumeroDoc = " + tabla + ".NumeroDoc " +
                        "   INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID  AND noEmpleado.EmpresaGrupoID = @EmpresaID " +
                        "   INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = " + tabla + ".ConceptoNOID " + " and noConceptoNOM.EmpresaGrupoID = " + tabla + ".eg_noConceptoNOM " +
                        " WHERE glDocumentoControl.EmpresaID = @EmpresaID  " + where +
                        "   AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin " +
                        "   AND ((@EmpleadoID is null) or (noEmpleado.EmpleadoID=@EmpleadoID))  " +
                        " GROUP BY  noConceptoNOM.ConceptoNOID, noConceptoNOM.Descriptivo,  noLiquidacionesDocu.Dias1, " + tabla + ".Base,noConceptoNOM.Tipo"; 
 
                        #endregion                   
                        mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char); 
                        mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                    }
                    else if(tipoReporte == 2) //Por Concepto
                    {
                        #region Filtros
                        if (!isAll)
                        {
                            if (orderName)
                                order = " ORDER BY noEmpleado.Descriptivo ";
                            else
                                order = " ORDER BY glDocumentoControl.TerceroID ";
                        }

                        if (!string.IsNullOrEmpty(terceroID))
                        {
                            where += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                            mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                            mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                        }

                        if (!string.IsNullOrEmpty(operacionNoID))
                        {
                            where += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                            mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                            mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionNoID;
                        }

                        if (!string.IsNullOrEmpty(areaFuncID))
                        {
                            where += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                            mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                            mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areaFuncID;
                        }

                        if (!string.IsNullOrEmpty(conceptoNoID))
                        {
                            where += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                            mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                            mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptoNoID;
                        } 
                        #endregion
                        #region CommandText
		                        mySqlCommandSel.CommandText =

                                    "   SELECT		" +
                                    "   	glDocumentoControl.TerceroID as EmpleadoID,		" +
                                    "   	noEmpleado.Descriptivo as EmpleadoDesc,  		" +                                   
                                    "   	noConceptoNOM.ConceptoNOID, 		" +
                                    "   	noConceptoNOM.Descriptivo, 		" +
                                    "   	noLiquidacionesDetalle.Base, 		" +
                                    "   	noLiquidacionesDetalle.Valor AS Valor		" +
                                    "   FROM		noLiquidacionesDetalle			" +
                                    "   INNER JOIN noLiquidacionesDocu		ON noLiquidacionesDocu.NumeroDoc = noLiquidacionesDetalle.NumeroDoc 		" +
                                    "   INNER JOIN glDocumentoControl		ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc 	 		" +
                                    "   INNER JOIN noConceptoNOM			ON noConceptoNOM.ConceptoNOID = noLiquidacionesDetalle.ConceptoNOID 		" +
                                    "   										AND noConceptoNOM.EmpresaGrupoID = noLiquidacionesDetalle.eg_noConceptoNOM 		" +
                                    "   INNER JOIN noEmpleado				ON noLiquidacionesDocu.ContratoNOID = noEmpleado.ContratoNOID AND noEmpleado.EmpresaGrupoID = @EmpresaID		" +
                                    "   WHERE	glDocumentoControl.EmpresaID = @EmpresaID		" +
                                    "   		AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin  " +
                                    where + order;
 
	                    #endregion 
                    }
                    else if (tipoReporte == 3) //Resumido Por Concepto
                    {
                        #region Filtros
                        if (!isAll)
                        {
                            if (orderName)
                                order = " order by noConceptoNOM.Descriptivo ";
                            else 
                                order = " order by noConceptoNOM.Tipo ";                               
                        }

                        if (!string.IsNullOrEmpty(terceroID))
                        {
                            where += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                            mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                            mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                        }

                        if (!string.IsNullOrEmpty(operacionNoID))
                        {
                            where += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                            mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                            mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionNoID;
                        }

                        if (!string.IsNullOrEmpty(areaFuncID))
                        {
                            where += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                            mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                            mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areaFuncID;
                        }

                        if (!string.IsNullOrEmpty(conceptoNoID))
                        {
                            where += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                            mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                            mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptoNoID;
                        } 
                        #endregion
                        #region CommandText
                        mySqlCommandSel.CommandText =
                         " SELECT " + tabla + ".ConceptoNOID, noConceptoNOM.Descriptivo," +
                         "   CASE WHEN(noConceptoNOM.Tipo = 1) THEN 'Devengo' ELSE 'Deducción' END AS Tipo, " +
                         "   ABS(SUM(" + tabla + ".Base)) as Base, ABS(SUM(" + tabla + ".Valor))as Valor " +
                         " FROM " + tabla + " with(nolock) " +
                         "   INNER JOIN noLiquidacionesDocu ON " + tabla + ".NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                         "   INNER JOIN glDocumentoControl ON glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                         "   INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = " + tabla + ".ConceptoNOID  " + " and noConceptoNOM.EmpresaGrupoID = " + tabla + ".eg_noConceptoNOM " +
                         " WHERE glDocumentoControl.EmpresaID = @EmpresaID " + where +
                         "   AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin " +
                         " GROUP BY " + tabla + ".ConceptoNOID, noConceptoNOM.Descriptivo, noConceptoNOM.Tipo " +
                         order; 
                        #endregion
                    }
                    else if (tipoReporte == 4) //Resumido Por Empleado
                    {
                        #region Filtros
                        if (orderName)
                            order = " ORDER BY noEmpleado.Descriptivo ";                            
                        else
                            order = " ORDER BY noEmpleado.EmpleadoID ";

                        if (!string.IsNullOrEmpty(terceroID))
                        {
                            where += " AND glDocumentoControl.TerceroID =  CASE WHEN @TerceroID = '' THEN glDocumentoControl.TerceroID ELSE @TerceroID END ";
                            mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                            mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                        }
                        if (!string.IsNullOrEmpty(operacionNoID))
                        {
                            where += " AND NoLiquidacionesDocu.OperacionNoID =  @OperacionNoID ";
                            mySqlCommandSel.Parameters.Add("@OperacionNoID", SqlDbType.Char, UDT_OperacionNOID.MaxLength);
                            mySqlCommandSel.Parameters["@OperacionNoID"].Value = operacionNoID;
                        }
                        if (!string.IsNullOrEmpty(areaFuncID))
                        {
                            where += " AND NoLiquidacionesDocu.AreaFuncionalID =  @AreaFuncionalID ";
                            mySqlCommandSel.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                            mySqlCommandSel.Parameters["@AreaFuncionalID"].Value = areaFuncID;
                        }
                        if (!string.IsNullOrEmpty(conceptoNoID))
                        {
                            where += " AND NoLiquidacionesDetalle.ConceptonoID =  @ConceptonoID ";
                            mySqlCommandSel.Parameters.Add("@ConceptonoID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                            mySqlCommandSel.Parameters["@ConceptonoID"].Value = conceptoNoID;
                        } 
                        #endregion
                        #region CommandText
                        mySqlCommandSel.CommandText =
                            " SELECT noEmpleado.EmpleadoID, noEmpleado.Descriptivo as EmpleadoDesc,  " +
                            "    ABS(SUM(" + tabla + ".Valor))as Valor " +
                            " FROM glDocumentoControl  with(nolock) " +
                            "    INNER JOIN noLiquidacionesDocu ON  glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc  " +
                            "    INNER JOIN " + tabla + " ON " + tabla + ".NumeroDoc = noLiquidacionesDocu.NumeroDoc " +
                            "    INNER JOIN noEmpleado ON noEmpleado.ContratoNOID = noLiquidacionesDocu.ContratoNOID and noEmpleado.EmpresaGrupoID = @EmpresaID" +
                            "    INNER JOIN noConceptoNOM ON noConceptoNOM.ConceptoNOID = " + tabla + ".ConceptoNOID " + " and noConceptoNOM.EmpresaGrupoID = " + tabla + ".eg_noConceptoNOM " +
                            " WHERE glDocumentoControl.EmpresaID = @EmpresaID" + where +
                            "    AND glDocumentoControl.PeriodoDoc BETWEEN @fechaIni AND @fechaFin " +
                            " GROUP BY noEmpleado.EmpleadoID, noEmpleado.Descriptivo, noEmpleado.CuentaAbono" +
                             order; 
                        #endregion
                    }

                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime); 
                    #endregion
                    #region Asignacion Valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = docNomina;
                    mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin; 
                    #endregion
                }
                #endregion
                #region Cesantias
                else if (documentoID == AppReports.noCesantias)
                {                   
                    #region CommandText
                    if (tipoReporte == 1) // Cesantrias acumuladas
                    {
                      
                    }
                    else if (tipoReporte == 2) // Cesantias Detalle
                    {
                        mySqlCommandSel.CommandText =
                                                    " DECLARE @_empresaNumCtrl		AS VARCHAR(10)	 " +
                                                    " DECLARE @_codigoCartera			AS VARCHAR(10)	 " +
                                                    " DECLARE @_cesantias				AS VARCHAR(10) " +
                                                    " DECLARE @_interesCesantias		AS VARCHAR(10)	" +

                                                    " SELECT @_empresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID = @EmpresaID " +
                                                    " SET @_codigoCartera = @_empresaNumCtrl + '11'	 " +
                                                    
                                                    " SELECT @_cesantias			= Data FROM glControl WHERE glControlId = CAST(@_codigoCartera + '406' AS INT)  " +
                                                    " SELECT @_interesCesantias	= Data FROM glControl WHERE glControlId = CAST(@_codigoCartera + '408' AS INT)  " +

                                                    " SELECT	emple.TerceroID AS Cedula,emple.Descriptivo AS Nombre, " +
                                                    "         emple.FondoCesantias,fondo.Descriptivo as FondoDesc, " +
                                                    "         emple.FechaIngreso + 1 AS FechaIngreso,doc.DiasContrato, " +
                                                    "         doc.FechaIni3 AS FechaCorte, " +
                                                     "        CTL.FechaDoc AS FechaPago, " +
                                                    "         (doc.SueldoML + doc.SueldoME) AS Salario, " +
                                                    "         SUM(CASE WHEN DET.ConceptoNOID = @_cesantias THEN DET.Valor ELSE 0 END) AS CESANTIAS, " +
                                                    "         SUM(CASE WHEN DET.ConceptoNOID = @_interesCesantias THEN DET.Valor ELSE 0 END) AS INTERES, " +
                                                    "         SUM(DET.Valor) AS TOTAL " +
                                                    " FROM noLiquidacionesDetalle	DET " +
                                                    " INNER JOIN noLiquidacionesDocu	doc WITH (NOLOCK) ON doc.NumeroDoc = DET.NumeroDoc  " +
                                                    " INNER JOIN glDocumentoControl		CTL WITH (NOLOCK) ON CTL.NumeroDoc = doc.NumeroDoc 	 " +
                                                   "  INNER JOIN noEmpleado			emple WITH (NOLOCK) ON emple.TerceroID = CTL.TerceroID	AND emple.eg_coTercero = CTL.EmpresaID  " +
                                                    " INNER JOIN noFondo			fondo WITH (NOLOCK) ON fondo.FondoNOID = emple.FondoCesantias AND fondo.EmpresaGrupoID =  CTL.EmpresaID " +
                                                    " WHERE   CTL.EmpresaID = @EmpresaID " +
                                                    "         AND CTL.DocumentoID = @DocumentoID " +
                                                    "         AND ((@EmpleadoID is null) or (emple.TerceroID=@EmpleadoID))	 " +
                                                    "  Group by emple.Descriptivo,emple.TerceroID,emple.FondoCesantias,fondo.Descriptivo, emple.FechaIngreso,doc.FechaIni3, " +
                                                    "             doc.DiasContrato,CTL.FechaDoc,doc.FechaFin3,doc.DatoAdd1,doc.SueldoML, doc.SueldoME " +
                                                    " order by  emple.Descriptivo ";                                                    
                    }
                    #endregion
                    #region parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);                  
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    #endregion
                    #region Asignacion de Valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Cesantias;
                    #endregion
                }
                #endregion
                #region Provisiones
                else if (documentoID == AppReports.noProvisiones)
                {
                    #region CommandText
                    if (tipoReporte == 1) // Provisiones Saldos
                    {
                        mySqlCommandSel.CommandText =
                               "DECLARE @_empresaNumCtrl		AS VARCHAR(10)	" +
                               "DECLARE @_codigoCartera			AS VARCHAR(10)		 " +
                               "DECLARE @_cesantias				AS VARCHAR(10)	" +
                               "DECLARE @_interesCesantias		AS VARCHAR(10)	 " +
                               "DECLARE @_vacaciones			AS VARCHAR(10)	" +
                               "DECLARE @_primaServicios		AS VARCHAR(10)	" +

                               "SELECT @_empresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID = @EmpresaID " +
                               "SET @_codigoCartera = @_empresaNumCtrl + '11'	 " +
                               
                               "SELECT @_primaServicios		= Data FROM glControl WHERE glControlId = CAST(@_codigoCartera + '401' AS INT)  " +
                               "SELECT @_vacaciones			= Data FROM glControl WHERE glControlId = CAST(@_codigoCartera + '405' AS INT)  " +
                               "SELECT @_cesantias			= Data FROM glControl WHERE glControlId = CAST(@_codigoCartera + '406' AS INT)  " +
                               "SELECT @_interesCesantias	= Data FROM glControl WHERE glControlId = CAST(@_codigoCartera + '408' AS INT)  " +

                               "SELECT	emple.EmpleadoID, emple.Descriptivo as EmpleadoDesc,   " +
                               "        SUM(CASE WHEN det.ConceptoNOID = @_vacaciones THEN (DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) ELSE 0 END) AS VlrVacaciones, " +
                               "        SUM(CASE WHEN det.ConceptoNOID = @_primaServicios THEN (DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) ELSE 0 END) AS VlrPrima, " +
                               "        SUM(CASE WHEN det.ConceptoNOID = @_cesantias THEN(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) ELSE 0 END) AS VlrCesantias, " +
                               "        SUM(CASE WHEN det.ConceptoNOID = @_interesCesantias THEN (DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) ELSE 0 END) AS VlrInteresCes " +
                               "     FROM coCuentaSaldo s with(nolock)   " +
                               "         INNER JOIN glDocumentoControl ctrl ON  ctrl.NumeroDoc = s.IdentificadorTR   " +
                               "         INNER JOIN noLiquidacionesDocu docu ON docu.NumeroDoc = ctrl.NumeroDoc   " +
                               "         INNER JOIN noLiquidacionesDetalle	det WITH (NOLOCK) ON docu.NumeroDoc = det.NumeroDoc  " +
                               "         INNER JOIN noComponenteNomina	comp ON (comp.CuentaID =s.CuentaID or s.CuentaID = comp.CtaAnticipo) and comp.eg_coPlanCuenta = s.eg_coPlanCuenta   " +
                               "         INNER JOIN noEmpleado			emple WITH (NOLOCK) ON emple.TerceroID = ctrl.TerceroID AND emple.eg_coTercero = ctrl.EmpresaID  " +
                               "WHERE  docu.EmpresaID = @EmpresaID AND s.PeriodoID <= @PeriodoID   " +
                               "       AND ((@EmpleadoID is null) or (emple.EmpleadoID=@EmpleadoID)) " +
                               " GROUP BY emple.EmpleadoID, emple.Descriptivo " +
                               " order by emple.Descriptivo ";
                    }
                    else if (tipoReporte == 2) // Provisiones Detalle
                    {
                         mySqlCommandSel.CommandText =
                                "SELECT emple.EmpleadoID as Cedula, emple.Descriptivo as Nombre, concep.ConceptoNOID as Concepto,concep.Descriptivo as ConceptoDesc, " +
	                            "    emple.FechaIngreso,prov.DiasProvision,emple.Sueldo, prov.BaseVariable, prov.BaseNeta, " +
	                            "    prov.VlrConsolidadoIni, prov.VlrProvisionIni,prov.VlrProvisionMes, prov.VlrPagosMes, " +
                                "SUM(CASE WHEN (prov.VlrConsolidadoIni+prov.VlrProvisionIni+prov.VlrProvisionMes-prov.VlrPagosMes) is not null THEN (prov.VlrConsolidadoIni+prov.VlrProvisionIni+prov.VlrProvisionMes-prov.VlrPagosMes) ELSE 0 END) AS ProvisionAcumulada  " +
                                "from noProvisionDeta prov " +
	                            "    INNER JOIN glDocumentoControl	ctrl WITH (NOLOCK) ON ctrl.NumeroDoc = prov.NumeroDoc  " +									
	                            "    INNER JOIN noEmpleado			emple WITH (NOLOCK) ON emple.TerceroID = ctrl.TerceroID		AND emple.eg_coTercero = ctrl.EmpresaID  " +
	                            "    INNER JOIN noConceptoNOM		concep WITH (NOLOCK) ON concep.ConceptoNOID = prov.ConceptoNOID	AND concep.eg_noConceptoGrupoNOM = prov.eg_noConceptoNOM  " +
                                "where prov.EmpresaID = @EmpresaID  and prov.Periodo = @PeriodoID " +
	                            "      AND ((@EmpleadoID is null) or (emple.EmpleadoID=@EmpleadoID)) " +	
                                "group by emple.EmpleadoID, emple.Descriptivo,concep.ConceptoNOID, concep.Descriptivo,emple.FechaIngreso,prov.DiasProvision,emple.Sueldo, " +
                                "prov.BaseVariable, prov.BaseNeta,prov.VlrConsolidadoIni, prov.VlrProvisionIni,prov.VlrProvisionMes, prov.VlrPagosMes " +
                                "order by emple.Descriptivo ";
                    }
                    #endregion
                    #region parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                    #endregion
                    #region Asignacion de Valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                    mySqlCommandSel.Parameters["@PeriodoID"].Value = fechaIni;
                    #endregion
                }
                #endregion

                #region Llena Datatable
                sda.SelectCommand = mySqlCommandSel;
                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);
                #endregion
                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Reportes_No_NominaToExcel");
                return null;
            }
        }


        #endregion
    }
}


