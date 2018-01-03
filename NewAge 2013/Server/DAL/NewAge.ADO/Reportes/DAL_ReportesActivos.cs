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
    public class DAL_ReportesActivos : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesActivos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        /// <param name="tipoLibro">Trae el nombre del libor que se va a generar</param>
        /// <param name="isMonedaLoc">Tipo Moneda en que se desea ver el reporte</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public List<DTO_acCostos> DAL_ReportesActivos_Saldos(string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, string tipoLibro, bool isMonedaLoc, List<string> ConceptoSaldo)
        {
            try
            {
                List<DTO_acCostos> result = new List<DTO_acCostos>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros para la cosulta

                //Variables para filtros
                string plaq = "", serie = "", refe = "", act = "", actTipo = "", grupo = "", propie = "", pivot = "", vlrMtoML = "", vlrMtoME = "";

                #region Asignacion de codigo a la consulta dependiento de la moneda

                switch (isMonedaLoc)
                {
                    //Si la modeda es local Pivotea los movimiento locales
                    case true:
                        vlrMtoML = "  SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as VlrMtoML ";
                        pivot = " (SUM(VlrMtoML) ";
                        break;
                    //Si la moneda es extrajera Pivotea los movimiento Extranjeros
                    case false:
                        vlrMtoME = " SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as VlrMtoME	 ";
                        pivot = " (SUM(VlrMtoME) ";
                        break;
                }

                #endregion

                #region Asignacion de para el Where

                // Carga las Variable para asiganar el fraGmento de codigo a la consulta para su respectivo filtro
                if (!string.IsNullOrEmpty(plaqueta))
                    plaq = " AND acCtrl.PlaquetaID = @plaqueta ";
                if (!string.IsNullOrEmpty(serial))
                    serie = " AND acCtrl.SerialID = @serial ";
                if (!string.IsNullOrEmpty(referencia))
                    refe = " AND acCtrl.inReferenciaID = @ref ";
                if (!string.IsNullOrEmpty(clase))
                    act = " AND acCtrl.ActivoClaseID = @activo ";
                if (!string.IsNullOrEmpty(tipo))
                    actTipo = " AND acCtrl.ActivoTipoID = @tipo ";
                if (!string.IsNullOrEmpty(grupos))
                    grupo = "  AND acCtrl.ActivoGrupoID = @grupo ";
                if (!string.IsNullOrEmpty(propietario))
                    propie = " AND acCtrl.Propietario   = @propietario ";
                #endregion

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT  /* coSaldoControl,  DecripComponente,*/ " +
                        " BalanceTipoID, PeriodoID,PlaquetaID,Observacion,SerialID,LocFisicaID,ActivoClaseID ,NombreClase,SaldoML, SaldoME, " +
                        " CASE WHEN ([@Costo] IS NULL ) THEN 0 ELSE [@Costo] END AS 'Costo', " +
                        " CASE WHEN ([@Depresiacion] IS NULL ) THEN 0 ELSE [@Depresiacion] END AS 'Depresiacion', " +
                        " CASE WHEN ([@Deterioro] IS NULL ) THEN 0 ELSE [@Deterioro] END AS 'Deterioro', " +
                        " CASE WHEN ([@Revaloriacion] IS NULL ) THEN 0 ELSE [@Revaloriacion] END AS 'Revalorizacion', " +
                        " CASE WHEN ([@Desmantelamiento] IS NULL ) THEN 0 ELSE [@Desmantelamiento] END AS 'Desmantelamiento' " +
                     " FROM " +
                     " ( " +
                        " SELECT /*glconsSaldo.coSaldoControl , compAct.ComponenteActivoID [ComponenteActivoID], compAct.Descriptivo as DecripComponente, */ " +
                            " coSaldo.BalanceTipoID,/*coSaldo.CuentaID,*/acCtrl.ActivoID,  coSaldo.ConceptoSaldoID [ConceptoSaldoID], " +
                            " coSaldo.PeriodoID, acCtrl.PlaquetaID,acCtrl.Observacion,   acCtrl.SerialID, acCtrl.LocFisicaID, acCtrl.ActivoClaseID, clase.Descriptivo as NombreClase, " +
                            " SUM(coSaldo.DbSaldoIniLocML + coSaldo.DbSaldoIniExtML + coSaldo.CrSaldoIniLocML + coSaldo.CrSaldoIniExtML) as SaldoML, " +
                            " SUM(coSaldo.DbSaldoIniLocME + coSaldo.DbSaldoIniExtME + coSaldo.CrSaldoIniLocME + coSaldo.CrSaldoIniExtME) as SaldoME,   " +
                            vlrMtoML + vlrMtoME +
                        " FROM coCuentaSaldo  AS coSaldo WITH(NOLOCK) " +
                            " INNER JOIN glConceptoSaldo AS glconsSaldo WITH(NOLOCK) ON ( glconsSaldo.ConceptoSaldoID = coSaldo.ConceptoSaldoID) " +
                            " INNER JOIN acComponenteActivo AS compAct  WITH(NOLOCK) ON (compAct.ConceptoSaldoID = glconsSaldo.ConceptoSaldoID and compAct.tipoComponente <> 3 ) " +
                            " INNER JOIN acActivoControl AS acCtrl WITH(NOLOCK) ON (acCtrl.ActivoID = coSaldo.IdentificadorTR ) " +
                            " INNER JOIN acClase as clase WITH(NOLOCK) ON (clase.ActivoClaseID = acCtrl.ActivoClaseID and clase.EmpresaGrupoID = acCtrl.eg_acClase) " +
                        " WHERE coSaldo.EmpresaID = @EmpresaID " +
                            " AND  glconsSaldo.coSaldoControl = 5 " +
                            " AND coSaldo.BalanceTipoID = @libro " +
                            " AND DATEPART(YEAR, coSaldo.PeriodoID) = @año " +
                            " AND DATEPART(MONTH, coSaldo.PeriodoID) = @mes " +
                            plaq + serie + refe + act + actTipo + grupo + propie +
                        " GROUP BY BalanceTipoID, /*CuentaID,*/ ActivoID, coSaldo.ConceptoSaldoID, PeriodoID,PlaquetaID,Observacion,SerialID, " +
                                " LocFisicaID,acCtrl.ActivoClaseID,clase.Descriptivo " +
                    " )PVT " +
                    " PIVOT " +
                    pivot +
                    " FOR [ConceptoSaldoID] IN ([@Costo], [@Depresiacion] ,[@Deterioro], [@Revaloriacion] , [@Desmantelamiento])) as VlrMtoML ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@plaqueta", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@serial", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ref", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@activo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@tipo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@grupo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@propietario", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Costo", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Depresiacion", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Deterioro", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Revaloriacion", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Desmantelamiento", SqlDbType.Char, UDT_ConceptoSaldoID.MaxLength);

                #endregion
                #region Asignacion de Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@libro"].Value = libro;
                mySqlCommandSel.Parameters["@año"].Value = Periodo.Year;
                mySqlCommandSel.Parameters["@mes"].Value = Periodo.Month;
                mySqlCommandSel.Parameters["@plaqueta"].Value = plaqueta;
                mySqlCommandSel.Parameters["@serial"].Value = serial;
                mySqlCommandSel.Parameters["@ref"].Value = referencia;
                mySqlCommandSel.Parameters["@activo"].Value = clase;
                mySqlCommandSel.Parameters["@tipo"].Value = tipo;
                mySqlCommandSel.Parameters["@grupo"].Value = grupo;
                mySqlCommandSel.Parameters["@propietario"].Value = propietario;
                mySqlCommandSel.Parameters["@Costo"].Value = ConceptoSaldo[0];
                mySqlCommandSel.Parameters["@Depresiacion"].Value = ConceptoSaldo[1];
                mySqlCommandSel.Parameters["@Deterioro"].Value = ConceptoSaldo[2];
                mySqlCommandSel.Parameters["@Revaloriacion"].Value = ConceptoSaldo[3];
                mySqlCommandSel.Parameters["@Desmantelamiento"].Value = ConceptoSaldo[4];

                #endregion

                DTO_acCostos costos = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    costos = new DTO_acCostos(dr);
                    costos.Libro.Value = tipoLibro;
                    result.Add(costos);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesActivos");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion q carga el DTO que compara libros
        /// </summary>
        /// <param name="libros">Lirbo a Comparar</param>
        /// <param name="año">Año que se desea comparar</param>
        /// <param name="mes">Mes que se va a comparar</param>
        /// <param name="clase">Clase que se desea ver</param>
        /// <param name="tipo">Tipo que se desea ver</param>
        /// <param name="grupo">Grupo que se desea ver</param>
        /// <param name="centroCost">Centro Costo que se desea ver</param>
        /// <param name="logFis">Localizacion Fisica que se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea ver</param>
        /// <param name="componentes">Compoenente que se desea ver</param>
        /// <param name="orderby">Por cual se desea ordenar</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_acComparacionLibros> DAL_ReportesActivos_ComparacionLibros(Dictionary<int, string> libros, int año, int mes, string clase, string tipo, string grupo, string centroCost, string logFis, string proyecto)
        {
            try
            {
                List<DTO_acComparacionLibros> result = new List<DTO_acComparacionLibros>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros para la cosulta

                //Variables
                string classe = "", type = "", group = "", centroCosto = "", logFisica = "", proyect = "", componente = "", cuenta = "", plaqueta = "", idetificador = "", order = "";
                string costo, depreciacion, deterioro, revalorizacion;

                //Carga las Variable para asiganar el fracmento de codigo a la consulta
                if (!string.IsNullOrEmpty(clase))
                    classe = " and acCtrl.ActivoClaseID = " + "'" + clase + "'";
                if (!string.IsNullOrEmpty(tipo))
                    type = "  and acCtrl.ActivoTipoID = " + "'" + tipo + "'";
                if (!string.IsNullOrEmpty(grupo))
                    group = "  and acCtrl.ActivoGrupoID = " + "'" + grupo + "'";
                if (!string.IsNullOrEmpty(centroCost))
                    centroCosto = "  and acCtrl.ActivoGrupoID = " + "'" + centroCost + "'";
                if (!string.IsNullOrEmpty(logFis))
                    logFisica = "  and acCtrl.ActivoGrupoID = " + "'" + logFis + "'";
                if (!string.IsNullOrEmpty(proyecto))
                    proyect = "  and acCtrl.ActivoGrupoID = " + "'" + proyecto + "'";

                //Organiza el Reporte
                //if (orderby[1])
                //    componente = "compAct.ComponenteActivoID";
                //if (orderby[2])
                //    cuenta = ", acCtrl.ActivoClaseID";
                //if (orderby[3])
                //    plaqueta = ", acCtrl.PlaquetaID";
                //if (orderby[4])
                //    idetificador = ", acCtrl.ActivoID";

                //if (!string.IsNullOrEmpty(componente) || !string.IsNullOrEmpty(cuenta) || !string.IsNullOrEmpty(plaqueta) || !string.IsNullOrEmpty(idetificador))
                //    order = "ORDER BY " + componente + cuenta + plaqueta + idetificador;

                //costo = componentes[1];
                //depreciacion = componentes[2];
                //deterioro = componentes[3];
                //revalorizacion = componentes[4];

                #endregion

                #region CommanText

                mySqlCommandSel.CommandText =
                    " Select DISTINCT a.PeriodoID, a.PlaquetaID, a.SerialID, a.LocFisicaID, a.NombreClase, a.ActivoClaseID, a.Observacion, a.SaldoML as SaldoMLFUNC, " +
                        " a.SaldoME as SaldoMEFUNC, a.VlrMtoML as VlrMtoMLFUNC, a.VlrMtoME as VlrMtoMEFUNC,B.SaldoML AS SaldoMLIFRS, B.SaldoME AS SaldoMEIFRS, " +
                        " B.VlrMtoML AS  VlrMtoMLIFRS, B.VlrMtoME AS VlrMtoMEIFRS " +
                    " FROM " +
                    " (SELECT glconsSaldo.coSaldoControl ,compAct.ComponenteActivoID, compAct.Descriptivo as DecripComponente , " +
                    " coSaldo.BalanceTipoID,balan.Descriptivo as Libro,  coSaldo.CuentaID,acCtrl.ActivoID, " +
                      " coSaldo.ConceptoSaldoID, coSaldo.PeriodoID,acCtrl.PlaquetaID,acCtrl.Observacion, " +
                      " acCtrl.SerialID, acCtrl.LocFisicaID, acCtrl.ActivoClaseID, clase.Descriptivo as NombreClase, " +
                      " SUM(coSaldo.DbSaldoIniLocML + coSaldo.DbSaldoIniExtML + coSaldo.CrSaldoIniLocML + coSaldo.CrSaldoIniExtML) as SaldoML, " +
                      " SUM(coSaldo.DbSaldoIniLocME + coSaldo.DbSaldoIniExtME + coSaldo.CrSaldoIniLocME + coSaldo.CrSaldoIniExtME) as SaldoME, " +
                      " SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as VlrMtoML, " +
                      " SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as VlrMtoME, " +
                      " compAct.TipoComponente " +
                    " from coCuentaSaldo  AS coSaldo WITH(NOLOCK) " +
                         " inner join glConceptoSaldo AS glconsSaldo WITH(NOLOCK) ON  glconsSaldo.ConceptoSaldoID = coSaldo.ConceptoSaldoID " +
                         " inner join coBalanceTipo as balan WITH(NOLOCK) ON balan.BalanceTipoID = coSaldo.BalanceTipoID " +
                         " inner join acComponenteActivo AS compAct  WITH(NOLOCK) ON compAct.ConceptoSaldoID = coSaldo.ConceptoSaldoID and compAct.tipoComponente <> 3 " +
                         " inner join acActivoControl AS acCtrl WITH(NOLOCK) ON acCtrl.ActivoID = coSaldo.IdentificadorTR " +
                         " inner join acClase as clase WITH(NOLOCK) ON clase.ActivoClaseID = acCtrl.ActivoClaseID and clase.EmpresaGrupoID = acCtrl.eg_acClase " +
                    " WHERE  glconsSaldo.coSaldoControl = 5 " +
                        "  and coSaldo.BalanceTipoID = " + "'" + libros[1] + "'" +
                        " and DATEPART(YEAR, coSaldo.PeriodoID) = @año " +
                        " and DATEPART(MONTH, coSaldo.PeriodoID) = @mes " +
                        classe + type + group + centroCosto + logFisica + proyect +
                    //" and (compAct.ComponenteActivoID =  " + "'" + costo + "'" +
                    //" or compAct.ComponenteActivoID = " + "'" + depreciacion + "'" +
                    //" or compAct.ComponenteActivoID = " + "'" + deterioro + "'" +
                    //" or compAct.ComponenteActivoID = " + "'" + revalorizacion + "'" + ")" +
                    "  GROUP BY glconsSaldo.coSaldoControl ,compAct.ComponenteActivoID, compAct.Descriptivo, " +
                            " coSaldo.BalanceTipoID,balan.Descriptivo,  coSaldo.CuentaID,acCtrl.ActivoID, " +
                            " coSaldo.ConceptoSaldoID, coSaldo.PeriodoID,acCtrl.PlaquetaID,acCtrl.Observacion " +
                            " ,acCtrl.SerialID, acCtrl.LocFisicaID, acCtrl.ActivoClaseID, clase.Descriptivo, " +
                            " coSaldo.DbOrigenLocML,coSaldo.CrOrigenLocML,coSaldo.DbSaldoIniLocME,coSaldo.CrOrigenLocME,	" +
                            " compAct.TipoComponente) as A, " +
                    " (SELECT glconsSaldo.coSaldoControl ,compAct.ComponenteActivoID, compAct.Descriptivo as DecripComponente , " +
                          " coSaldo.BalanceTipoID,balan.Descriptivo as Libro,  coSaldo.CuentaID,acCtrl.ActivoID, " +
                          " coSaldo.ConceptoSaldoID, coSaldo.PeriodoID,acCtrl.PlaquetaID,acCtrl.Observacion, " +
                          " acCtrl.SerialID, acCtrl.LocFisicaID, acCtrl.ActivoClaseID, clase.Descriptivo as NombreClase, " +
                          " SUM(coSaldo.DbSaldoIniLocML + coSaldo.DbSaldoIniExtML + coSaldo.CrSaldoIniLocML + coSaldo.CrSaldoIniExtML) as SaldoML, " +
                          " SUM(coSaldo.DbSaldoIniLocME + coSaldo.DbSaldoIniExtME + coSaldo.CrSaldoIniLocME + coSaldo.CrSaldoIniExtME) as SaldoME, " +
                          " SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as VlrMtoML, " +
                          " SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as VlrMtoME, " +
                          " compAct.TipoComponente " +
                        " from  coCuentaSaldo  AS coSaldo WITH(NOLOCK) " +
                             " inner join glConceptoSaldo AS glconsSaldo WITH(NOLOCK) ON  glconsSaldo.ConceptoSaldoID = coSaldo.ConceptoSaldoID " +
                             " inner join coBalanceTipo as balan WITH(NOLOCK) ON balan.BalanceTipoID = coSaldo.BalanceTipoID " +
                             " inner join acComponenteActivo AS compAct  WITH(NOLOCK) ON compAct.ConceptoSaldoID = coSaldo.ConceptoSaldoID and compAct.tipoComponente <> 3" +
                             " inner join acActivoControl AS acCtrl WITH(NOLOCK) ON acCtrl.ActivoID = coSaldo.IdentificadorTR " +
                             " inner join acClase as clase WITH(NOLOCK) ON clase.ActivoClaseID = acCtrl.ActivoClaseID and clase.EmpresaGrupoID = acCtrl.eg_acClase " +
                        " WHERE  glconsSaldo.coSaldoControl = 5 " +
                            " and coSaldo.BalanceTipoID = " + "'" + libros[2] + "'" +
                            " and DATEPART(YEAR, coSaldo.PeriodoID) = @año " +
                            " and DATEPART(MONTH, coSaldo.PeriodoID) = @mes " +
                             classe + type + group + centroCosto + logFisica + proyect +
                    //" and (compAct.ComponenteActivoID =  " + "'" + costo + "'" +
                    //" or compAct.ComponenteActivoID = " + "'" + depreciacion + "'" +
                    //" or compAct.ComponenteActivoID = " + "'" + deterioro + "'" +
                    //" or compAct.ComponenteActivoID = " + "'" + revalorizacion + "'" + ")" +
                    "  GROUP BY glconsSaldo.coSaldoControl ,compAct.ComponenteActivoID, compAct.Descriptivo, " +
                            " coSaldo.BalanceTipoID,balan.Descriptivo,  coSaldo.CuentaID,acCtrl.ActivoID, " +
                            " coSaldo.ConceptoSaldoID, coSaldo.PeriodoID,acCtrl.PlaquetaID,acCtrl.Observacion " +
                            " ,acCtrl.SerialID, acCtrl.LocFisicaID, acCtrl.ActivoClaseID, clase.Descriptivo, " +
                            " coSaldo.DbOrigenLocML,coSaldo.CrOrigenLocML,coSaldo.DbSaldoIniLocME,coSaldo.CrOrigenLocME,	" +
                            " compAct.TipoComponente ) AS B ";


                #endregion

                #region Parametros
                mySqlCommandSel.Parameters.Add("@año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);

                #endregion

                #region Asignacion de Parametros

                mySqlCommandSel.Parameters["@año"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;

                #endregion

                DTO_acComparacionLibros compLibros = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    compLibros = new DTO_acComparacionLibros(dr);
                    result.Add(compLibros);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesActivos_ComparacionLibros");
                throw exception;
            }
        }

        #region Arriendos

        /// <summary>
        /// Funcion q carga el DTO de Equipos Arrendados
        /// </summary>
        /// <param name="Periodo">Periodo</param>
        /// <param name="Estado">Estado que identifica si es arrendado o no</param>
        /// <param name="Tercero">Terceror</param>
        /// <param name="Plaqueta">Plaqueta</param>
        /// <param name="Serial">Serial</param>
        /// <param name="TipoRef">Tipo de la referencia</param>
        /// <param name="Rompimiento">Tipo de rompimiento</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportEquiposArrendados> DAL_ReportesActivos_EquiposArrendados(DateTime Periodo, int Estado, string Tercero, string Plaqueta, string Serial, string TipoRef, string Rompimiento)
        {
            try
            {
                List<DTO_ReportEquiposArrendados> result = new List<DTO_ReportEquiposArrendados>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros para la cosulta

                //Variables
                string tercero = "", plaqueta = "", serial = "", tiporef = "";

                //Filtros
                if (!string.IsNullOrEmpty(Tercero))
                    tercero = " AND act.TerceroID=@Tercero ";
                if (!string.IsNullOrEmpty(Plaqueta))
                    plaqueta = " AND act.PlaquetaID=@Plaqueta ";
                if (!string.IsNullOrEmpty(Serial))
                    serial = " AND act.SerialID=@Serial ";
                if (!string.IsNullOrEmpty(TipoRef))
                    tiporef = " AND reft.TipoInvID=@TipoRef ";

                string filtro = tercero + plaqueta + serial + tiporef;
                #endregion

                #region CommanText

                mySqlCommandSel.CommandText =
                    "   SELECT       act.PlaquetaID,act.Observacion as Descripcion, " +
                    "                act.SerialID, " +
                    "                act.LocFisicaID as Localizacion, " +
                    "                RTRIM (CAST (ctrl.PrefijoID as CHAR))+ ' - ' + RTRIM(CAST(ctrl.NumeroDoc as CHAR)) as NotaEnvio, " +
                    "                ctrl.FechaDoc as FechaEntrega, " +
                    "                act.TerceroID, " +
                    "                ter.Descriptivo, " +
                    "                reft.TipoInvID, " +
                    "                reft.Descriptivo as DescriptivoRef " +
                    "    FROM	 " +
                    "                acActivoControl act  WITH (NOLOCK) " +
                    "                INNER JOIN glDocumentoControl ctrl WITH (NOLOCK) on ctrl.NumeroDoc=act.NumeroDocUltMvto " +
                    "                INNER JOIN inReferencia ref WITH (NOLOCK) on ref.inReferenciaID=act.inReferenciaID and ref.EmpresaGrupoID=act.eg_inReferencia " +
                    "                INNER JOIN inRefTipo reft WITH (NOLOCK) on reft.TipoInvID=ref.TipoInvID and reft.EmpresaGrupoID=ref.eg_inRefTipo " +
                    "                INNER JOIN coTercero ter WITH (NOLOCK) on ter.TerceroID=act.TerceroID and ter.EmpresaGrupoID=act.eg_coTercero " +
                    "    WHERE  " +
                    "                ctrl.EmpresaID=@Empresa AND " +
                    "                act.FechaVencimiento<=@Periodo AND " +
                    "                act.EstadoInv=@Estado " + filtro;


                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plaqueta", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Serial", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoRef", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);

                #endregion

                #region Asignacion de Parametros

                mySqlCommandSel.Parameters["@Periodo"].Value = Periodo;
                mySqlCommandSel.Parameters["@Estado"].Value = Estado;
                mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Tercero"].Value = Tercero;
                mySqlCommandSel.Parameters["@Plaqueta"].Value = Plaqueta;
                mySqlCommandSel.Parameters["@Serial"].Value = Serial;
                mySqlCommandSel.Parameters["@TipoRef"].Value = TipoRef;

                #endregion

                DTO_ReportEquiposArrendados Equipo = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    Equipo = new DTO_ReportEquiposArrendados(dr);
                    //if (Rompimiento == "Tercero")
                    //    Equipo.Rompimiento.Value = Equipo.TerceroID.Value;
                    //if (Rompimiento == "TipoRef")
                    //    Equipo.Rompimiento.Value = Equipo.TipoRef.Value;
                    result.Add(Equipo);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesActivos_EquiposArrendados");
                throw exception;
            }
        }


        #endregion

        /// <summary>
        /// Funcion q carga el DTO de Importaciones Temporales
        /// </summary>
        /// <param name="Periodo">Periodo</param>
        /// <param name="Plaqueta">Plaqueta</param>
        /// <param name="Serial">Serial</param>
        /// <param name="TipoRef">Tipo de la referencia</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportImportacionesTemporales> DAL_ReportesActivos_ImportacionesTemporales(DateTime Periodo, string Plaqueta, string Serial, string TipoRef)
        {
            try
            {
                List<DTO_ReportImportacionesTemporales> result = new List<DTO_ReportImportacionesTemporales>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros para la cosulta

                //Variables
                string plaqueta = "", serial = "", tiporef = "";

                //Filtros
                if (!string.IsNullOrEmpty(Plaqueta))
                    plaqueta = " AND act.PlaquetaID=@Plaqueta ";
                if (!string.IsNullOrEmpty(Serial))
                    serial = " AND act.SerialID=@Serial ";
                if (!string.IsNullOrEmpty(TipoRef))
                    tiporef = " AND reft.TipoInvID=@TipoRef ";

                string filtro = plaqueta + serial + tiporef;
                #endregion

                #region CommanText

                mySqlCommandSel.CommandText =
                    " select	PlaquetaID,Observacion,SerialID, " +
                    "            LocFisicaID,FechaImportacion,FechaVencimiento  " +
                    "  from	    acActivoControl act WITH (NOLOCK)  " +
                    "            INNER JOIN inReferencia ref WITH (NOLOCK)  " +
                    "            on ref.inReferenciaID=act.inReferenciaID and ref.EmpresaGrupoID=act.eg_inReferencia " +
                    "            INNER JOIN inRefTipo reft WITH (NOLOCK)  " +
                    "            on reft.TipoInvID=ref.TipoInvID AND reft.EmpresaGrupoID=ref.eg_inRefTipo " +
                    "  where 	(DatoAdd1<>null or DatoAdd1<>'') " +
                    "            AND act.FechaVencimiento<@Periodo " +
                    "            AND act.EmpresaID=@Empresa ";

                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);                
                mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Plaqueta", SqlDbType.Char, UDT_PlaquetaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Serial", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoRef", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);

                #endregion

                #region Asignacion de Parametros

                mySqlCommandSel.Parameters["@Periodo"].Value = Periodo;
                mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Plaqueta"].Value = Plaqueta;
                mySqlCommandSel.Parameters["@Serial"].Value = Serial;
                mySqlCommandSel.Parameters["@TipoRef"].Value = TipoRef;

                #endregion

                DTO_ReportImportacionesTemporales Equipo = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    Equipo = new DTO_ReportImportacionesTemporales(dr);
                    //if (Rompimiento == "Tercero")
                    //    Equipo.Rompimiento.Value = Equipo.TerceroID.Value;
                    //if (Rompimiento == "TipoRef")
                    //    Equipo.Rompimiento.Value = Equipo.TipoRef.Value;
                    result.Add(Equipo);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesActivos_ImportacionesTemporales");
                throw exception;
            }
        }


    }
}


