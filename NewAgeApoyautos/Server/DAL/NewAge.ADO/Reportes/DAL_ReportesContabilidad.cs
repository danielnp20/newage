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
    public class DAL_ReportesContabilidad : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesContabilidad(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Documentos

        #region Comprobante Manual

        /// <summary>
        /// Funcion q se encarga de traer los datos para el comprobante manual
        /// </summary>
        /// <param name="numeroDoc">Numero Doc de identificacion</param>
        /// <param name="isAprovada">Verifica si es aprobada (True: Trae los Datos de la Tabla coAuxiliar, False: Trae los datos de la tabla coAuxiliarPre) </param>
        /// <param name="moneda">Verifica la moneda que se esta trabajando (True:Local, False: Extranjera) </param>
        /// <returns></returns>
        public List<DTO_ReportComprobanteManual> DAL_ReportesContabilidad_ComprobanteManual(int numeroDoc, bool isAprovada, bool moneda)
        {
            try
            {
                List<DTO_ReportComprobanteManual> result = new List<DTO_ReportComprobanteManual>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string vlrDebito, vlrCredito, vlrBase;
                string table = isAprovada ? " coAuxiliar " : "coAuxiliarPre";

                if (moneda)
                {
                    vlrDebito = " CASE WHEN (vlrMdaLoc > 0) THEN vlrMdaLoc else 0 end AS Debito, ";
                    vlrCredito = " CASE WHEN (vlrMdaLoc < 0) THEN vlrMdaLoc else 0 end AS Credito ";
                    vlrBase = " vlrBaseML Base,";
                }
                else 
                {
                    vlrDebito = " CASE WHEN (vlrMdaExt > 0) THEN vlrMdaExt else 0 end AS Debito, ";
                    vlrCredito = " CASE WHEN (vlrMdaExt < 0) THEN vlrMdaExt else 0 end AS Credito ";
                    vlrBase = " vlrBaseME Base, ";
                }
                
                #endregion
                #region CommandText

                mySqlCommandSel.CommandText =
                     " SELECT  CuentaID,  TerceroID, ProyectoID, ConceptoCargoID, LineaPresupuestoID, Descripcion,fechaFac, " +
                              " ComprobanteID, ComprobanteNro," + vlrBase + vlrDebito + vlrCredito +
                              " /* vlrBaseML,vlrBaseME, " + 
                              " CASE WHEN (vlrMdaLoc > 0) THEN vlrMdaLoc else 0 end AS DebitoML,   " + 
                              " CASE WHEN (vlrMdaLoc < 0) THEN vlrMdaLoc else 0 end AS CreditoML, " +   
                              " CASE WHEN (vlrMdaExt > 0) THEN vlrMdaExt else 0 end AS DebitoExt,   " + 
                              " CASE WHEN (vlrMdaExt < 0) THEN vlrMdaExt else 0 end AS CreditoExt */   " +  
                     " FROM    " +  
                     " (      " +
                       " SELECT  auxPre.CuentaID,  auxPre.TerceroID,  auxPre.ProyectoID, auxPre.ConceptoCargoID, auxPre.LineaPresupuestoID,ctrl.Descripcion as Descripcion, " +
		                  " auxPre.Fecha as fechaFac,auxPre.ComprobanteID, auxPre.ComprobanteNro, " + 
                          " auxPre.vlrMdaLoc, " +
                           " auxPre.vlrBaseML, auxPre.vlrBaseME,auxPre.vlrMdaExt " +
                       " FROM " + table + " AS auxPre " +
                           " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) on ctrl.NumeroDoc = auxPre.NumeroDoc " +   
                       " WHERE auxPre.EmpresaID = @EmpresaID " +
                            " AND auxPre.NumeroDoc = @NumeroDoc " +
                     " ) CONSULTA ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                #endregion
                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                #endregion

                DTO_ReportComprobanteManual docContable = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    docContable = new DTO_ReportComprobanteManual(dr);
                    result.Add(docContable);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
             {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_ComprobanteManual");
                throw exception;
            }
        }
        
        #endregion
        
        #region Documento Contable

        /// <summary>
        /// Funcion q se encar de traer los datos para general el Documento Contabla
        /// </summary>
        /// <param name="numeroDoc">Identificacion del documento Contable</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportCausacionFacturas> DAL_ReportesContabilidad_DocumentoContable(int numeroDoc, bool isAprovada)
        {
            try
            {
                List<DTO_ReportCausacionFacturas> result = new List<DTO_ReportCausacionFacturas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string tabla = isAprovada ? " coAuxiliar " : "coAuxiliarPre";

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                     " SELECT DatoAdd4,TerceroID,nombreTercero, facturaNro, Descripcion,fechaFac, periodoID, Comprobante, CuentaID, nombreCta,   " +
                             " CentroCostoID,ProyectoID,LineaPresupuestoID,  vlrBaseML, cuentaCxP,nomCuentaCxp, vlrBaseME, TasaCambioBase,  " +
                             " CASE WHEN (vlrMdaLoc > 0) THEN vlrMdaLoc else 0 end AS DebitoML,  " +
                             " CASE WHEN (vlrMdaLoc < 0) THEN ABS(vlrMdaLoc) else 0 end AS CreditoML,  " +
                             " CASE WHEN (vlrMdaExt > 0) THEN vlrMdaExt else 0 end AS DebitoExt,  " +
                             " CASE WHEN (vlrMdaExt < 0) THEN ABS(vlrMdaExt) else 0 end AS CreditoExt    " +
                    " FROM    " +
                    " (    " +
                      " SELECT auxpre.DatoAdd4, ctrl.TerceroID, ter.Descriptivo as nombreTercero, ctrl.DocumentoTercero as facturaNro, ctrl.Descripcion as Descripcion,    " +
                          " auxPre.Fecha as fechaFac, auxPre.periodoID,RTRIM (CAST(auxPre.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(auxPre.ComprobanteNro AS CHAR(15)) AS Comprobante,    " +
                          " auxPre.CuentaID, cta.Descriptivo as nombreCta, auxPre.CentroCostoID,auxPre.ProyectoID,auxPre.LineaPresupuestoID,auxPre.vlrMdaLoc,   " +
                          " auxPre.vlrBaseML, auxPre.vlrBaseME,  ctrl.CuentaID as cuentaCxP, ctaCxP.Descriptivo as nomCuentaCxp, auxPre.vlrMdaExt, auxPre.TasaCambioBase  " +
                      " FROM  " + tabla + "  AS auxPre    " +
                          " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) on ctrl.NumeroDoc = auxPre.NumeroDoc    " +
                          " INNER JOIN coTercero as ter WITH(NOLOCK) on  (ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID = ctrl.eg_coTercero)    " +
                          " INNER JOIN coPlanCuenta as cta WITH(NOLOCK) on (cta.CuentaID = auxPre.CuentaID and cta.EmpresaGrupoID = auxPre.eg_coPlanCuenta)   " +
                          " LEFT JOIN coPlanCuenta as ctaCxP WITH(NOLOCK) on (ctaCxP.CuentaID = ctrl.CuentaID and ctaCxP.EmpresaGrupoID = ctrl.eg_coPlanCuenta)   " +
                      " WHERE auxPre.EmpresaID = @EmpresaID    " +
                                      " AND auxPre.NumeroDoc = @NumeroDoc  " +
                    " )	CONSULTA ";

                #endregion
                #region Paramentros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                #endregion
                #region Asignacion valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                #endregion

                DTO_ReportCausacionFacturas docContable = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    docContable = new DTO_ReportCausacionFacturas(dr, true);
                    result.Add(docContable);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_DocumentoContable");
                throw exception;
            }
        }

        #endregion

        #endregion

        #region Reportes PDF

        #region Auxiliar

        /// <summary>
        /// Funcion que carga el DTO
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportLibroDiario> DAL_ReportesContabilidad_AuxiliarCuentaFuncinal(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            try
            {
                List<DTO_ReportLibroDiario> aux = new List<DTO_ReportLibroDiario>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string whereAll = "";
                string whereAux = "";

                if (!string.IsNullOrEmpty(cuentaInicial) || !string.IsNullOrEmpty(tercero) || !string.IsNullOrEmpty(proyecto) || !string.IsNullOrEmpty(centroCosto) ||
                    !string.IsNullOrEmpty(lineaPresupuestal))
                {
                    if (!string.IsNullOrEmpty(cuentaInicial))
                    {
                        if (!string.IsNullOrEmpty(cuentaFin))
                        {
                            whereAux = " AND aux.CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                            whereAll = " WHERE CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                        }
                        else
                        {
                            whereAux = " AND aux.CuentaID = " + "'" + cuentaInicial + "'";
                            whereAll = " WHERE CuentaID = " + "'" + cuentaInicial + "'";
                        }
                    }

                    if (!string.IsNullOrEmpty(tercero))
                    {
                        whereAux += " AND aux.TerceroID = " + "'" + tercero + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE TerceroID in ('','" + tercero + "')" :
                                " AND TerceroID in ('','" + tercero + "')";
                    }

                    if (!string.IsNullOrEmpty(proyecto))
                    {
                        whereAux += " AND aux.ProyectoID = " + "'" + proyecto + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE ProyectoID in ('','" + proyecto + "')" :
                                " AND ProyectoID in ('','" + proyecto + "')";
                    }

                    if (!string.IsNullOrEmpty(centroCosto))
                    {
                        whereAux += " AND aux.CentroCostoID = " + "'" + centroCosto + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE CentroCostoID in ('','" + centroCosto + "')" :
                                " AND CentroCostoID in ('','" + centroCosto + "')";
                    }

                    if (!string.IsNullOrEmpty(lineaPresupuestal))
                    {
                        whereAux += " AND aux.LineaPresupuestoID = " + "'" + lineaPresupuestal + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE LineaPresupuestoID in ('','" + lineaPresupuestal + "')" :
                                " AND LineaPresupuestoID in ('','" + lineaPresupuestal + "')";
                    }
                }

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                   " SELECT * FROM " +
                    " ( " +
                            " SELECT  " +
                                " '' AS Comprobante,  " +
                                " '' AS ComprobanteID, '' AS ComprobanteNro, '' AS DocumentoCOM, '' AS Descriptivo, PeriodoID, aux.CuentaID, " +
                                " cta.Descriptivo AS CuentaDesc, '' AS Fecha, '' AS ComprobanteDesc,'' AS TerceroID, '' AS nomTercero, " +
                                " '' AS CentroCostoID, '' AS LineaPresupuestoID, '' AS ProyectoID, 0 AS TasaCambioBase, BalanceTipoID, '' AS SaldoControl,  " +
                                " '' AS DocumentoNumero, '' as DocumentoPrefijo, '' AS DocumentoTercero,  " +
                                " 0 AS vlrBaseML, 0 AS vlrBaseME, 0 AS DebitoML, 0 AS CreditoML, 0 AS DebitoME, 0 AS CreditoME,	  " +
                                " SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) as InicialML, " +
                                " SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) as InicialME " +
                            " FROM coCuentaSaldo aux with(nolock)  " +
                            "   INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                    " AND aux.PeriodoID = @_peridoIni " +
                                    " AND aux.BalanceTipoID = @Libro " + whereAux +
                    /*AND aux.CuentaID = '13050501'*/
                            " GROUP BY PeriodoID,aux.CuentaID,cta.Descriptivo,BalanceTipoID " +
                        " UNION ALL " +
                            " SELECT  " +
                                " RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                                " aux.ComprobanteID, aux.ComprobanteNro, aux.DocumentoCOM, SUBSTRING( aux.Descriptivo,1,37) as Descriptivo, aux.PeriodoID, aux.CuentaID, " +
                                " cta.Descriptivo as CuentaDesc, aux.Fecha, '' as ComprobanteDesc,aux.TerceroID,SUBSTRING(tercero.Descriptivo,1,22) as nomTercero, " +
                                " aux.CentroCostoID ,aux.LineaPresupuestoID,aux.ProyectoID, aux.TasaCambioBase, comp.BalanceTipoID, '' as SaldoControl, " +
                                " aux.DocumentoCOM as DocumentoNumero, '' as DocumentoPrefijo, aux.DocumentoCOM as DocumentoTercero,  " +
                                " aux.vlrBaseML, aux.vlrBaseME,	  " +
                                " CASE WHEN aux.vlrMdaLoc >= 0 THEN aux.vlrMdaLoc ELSE 0 END AS DebitoML, " +
                                " CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc * -1 ELSE 0 END AS CreditoML, " +
                                " CASE WHEN aux.vlrMdaExt >= 0 THEN aux.vlrMdaExt ELSE 0 END AS DebitoME, " +
                                " CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt * -1 ELSE 0 END AS CreditoME, " +
                                " 0 InicialML,    0 InicialME   " +
                            " FROM coAuxiliar aux  WITH(NOLOCK)  " +
                                " INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta  " +
                                " INNER JOIN coComprobante comp WITH(NOLOCK) ON aux.ComprobanteID=comp.ComprobanteID AND aux.eg_coComprobante = comp.EmpresaGrupoID  " +
                                " INNER JOIN coTercero as tercero WITH(NOLOCK) ON tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero   " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                " AND comp.BalanceTipoID = @libro  " +
                                " AND aux.PeriodoID BETWEEN @_peridoIni AND @_peridoFin AND DAY(aux.PeriodoID) != 2 " + whereAux +
                    /*	AND aux.CuentaID = '13050501'*/
                    " ) AS Auxiliar " + whereAll +
                    "ORDER BY CuentaID, Fecha, Comprobante";


                #region MyRegion
                //mySqlCommandSel.CommandText =
                //    " SELECT RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                //        " aux.ComprobanteID, aux.ComprobanteNro, aux.DocumentoCOM, SUBSTRING( aux.Descriptivo,1,37) as Descriptivo, aux.PeriodoID, aux.CuentaID, " +
                //        " cta.Descriptivo as CuentaDesc, aux.Fecha,comp.Descriptivo as ComprobanteDesc,aux.TerceroID,SUBSTRING(tercero.Descriptivo,1,22) as nomTercero, " +
                //        " aux.CentroCostoID ,aux.LineaPresupuestoID	,aux.ProyectoID, aux.TasaCambioBase, comp.BalanceTipoID, csaldo.coSaldoControl as SaldoControl, " +
                //        " ctrl.DocumentoNro DocumentoNumero	,ctrl.PrefijoID as DocumentoPrefijo, ctrl.DocumentoTercero, aux.vlrBaseML,aux.vlrBaseME,  " +
                //        " ((aux.vlrMdaLoc + ABS(aux.vlrMdaLoc))/2) DebitoML, " +
                //        " ((aux.vlrMdaLoc - ABS(aux.vlrMdaLoc))/(-2)) CreditoML, " +
                //        " ((aux.vlrMdaExt + ABS(aux.vlrMdaExt))/2) DebitoME,  " +
                //        " ((aux.vlrMdaExt - ABS(aux.vlrMdaExt))/(-2)) CreditoME,  " +
                //        " CASE WHEN (saldo.InicialML is null)  THEN 0 ELSE InicialML END InicialML, " +
                //        " CASE WHEN (saldo.InicialME is null)  THEN 0 ELSE InicialME END InicialME " +
                //    " FROM coAuxiliar aux  WITH(NOLOCK) " +
                //         " LEFT JOIN glDocumentoControl as ctrl with(nolock) on ctrl.NumeroDoc = aux.IdentificadorTR " +
                //         " INNER JOIN coPlanCuenta as cta with(nolock) on (cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta) " +
                //         " INNER JOIN coComprobante comp WITH(NOLOCK)ON(aux.ComprobanteID=comp.ComprobanteID AND aux.eg_coComprobante=comp.EmpresaGrupoID) " +
                //         " INNER JOIN coTercero as tercero WITH(NOLOCK)ON (tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero)		 " +
                //         " INNER JOIN glConceptoSaldo csaldo WITH(NOLOCK)ON(aux.ConceptoSaldoID=csaldo.ConceptoSaldoID AND aux.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
                //         " LEFT JOIN  " + 
                //         " (   " +
                //            " SELECT EmpresaID,PeriodoID,BalanceTipoID,CuentaID,CuentaAlternaID,ProyectoID,CentroCostoID,LineaPresupuestoID, " +
                //                " SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML +  CrSaldoIniExtML) as InicialML,   " +    
                //                " SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +  CrSaldoIniExtME) as InicialME    " +  
                //            " FROM coCuentaSaldo aux  WITH(NOLOCK)   " +
                //            " WHERE  EmpresaID = @EmpresaID   " +
                //                " AND Year(PeriodoID) = @Año " +
                //                " AND Month(PeriodoID) BETWEEN @fechaInic AND @fechaFin " +
                //                /*AND AUX.CuentaID = '23700505' */
                //                " AND BalanceTipoID = @Libro " +
                //                tipoCuenta + tipoProyecto + centroCos + lineaPres +
                //                /*AND aux.TerceroID = 800229739*/
                //            " GROUP BY EmpresaID,PeriodoID,BalanceTipoID,CuentaID,CuentaAlternaID,ProyectoID,CentroCostoID,LineaPresupuestoID  " +
                //         " ) as saldo on  " +
                //            " ( " +
                //                " saldo.EmpresaID = aux.EmpresaID AND saldo.BalanceTipoID = comp.BalanceTipoID AND saldo.CuentaID = aux.CuentaID AND  " +
                //                " saldo.ProyectoID = aux.ProyectoID AND " +
                //                " saldo.CentroCostoID = aux.CentroCostoID AND saldo.LineaPresupuestoID = aux.LineaPresupuestoID  " +
                //                /*" AND saldo.PeriodoID = aux.PeriodoID " + AND saldo.TerceroID = aux.TerceroID
                //                --saldo.CuentaAlternaID = aux.CuentaAlternaID AND */
                //            " )  " +
                //    " WHERE aux.EmpresaID = @EmpresaID  " +
                //        " AND Year(aux.PeriodoID) = @Año" +
                //        " AND Month(aux.PeriodoID) BETWEEN @fechaInic AND @fechaFin " + 
                //        " AND (comp.BalanceTipoID) = @Libro " +
                //        /*AND AUX.CuentaID = '23700505' 
                //        --AND aux.TerceroID = 800229739*/
                //        tipoCuenta + tipoTercero + tipoProyecto + centroCos + lineaPres +
                //    " ORDER BY aux.PeriodoID, aux.CuentaID,aux.ComprobanteNro,aux.ComprobanteID "; 
                #endregion

                #region No borrar
                //mySqlCommandSel.CommandText =
                //           " SELECT RTRIM (CAST(temp1.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(temp1.ComprobanteNro AS CHAR(15)) AS Comprobante " +
                //           "   ,temp1.ComprobanteID, temp1.ComprobanteNro, temp1.DocumentoCOM, SUBSTRING( temp1.Descriptivo,1,37) as Descriptivo ,temp1.PeriodoID,temp1.CuentaID,temp1.CuentaDesc " +
                //           "	,temp1.Fecha,temp1.ComprobanteDesc,temp1.TerceroID,temp1.TerceroID TerceroDesc,SUBSTRING(tercero.Descriptivo,1,22) as nomTercero,temp1.CentroCostoID " +
                //           "	,temp1.LineaPresupuestoID	,temp1.ProyectoID,temp1.TasaCambioBase,temp1.BalanceTipoID,temp1.SaldoControl,temp1.DocumentoNumero" +
                //           "	,temp1.DocumentoPrefijo,temp1.DocumentoTercero,temp1.vlrBaseML,temp1.vlrBaseME	 " +
                //           "    ,((temp1.vlrMdaLoc + ABS(temp1.vlrMdaLoc))/2) DebitoML,((temp1.vlrMdaLoc - ABS(temp1.vlrMdaLoc))/(-2)) CreditoML " +
                //           "    ,((temp1.vlrMdaExt + ABS(temp1.vlrMdaExt))/2) DebitoME,((temp1.vlrMdaExt - ABS(temp1.vlrMdaExt))/(-2)) CreditoME " +
                //           "    ,CASE WHEN(MONTH(balance.PeriodoID)=@fechaInic)THEN (balance.DbSaldoIniLocML+ balance.DbSaldoIniExtML + balance.CrSaldoIniLocML + " +
                //           "        balance.CrSaldoIniExtML)ELSE 0 END InicialML  " +
                //           "    ,CASE WHEN(MONTH(balance.PeriodoID)=@fechaInic)THEN (balance.DbSaldoIniLocME + balance.DbSaldoIniExtME + balance.CrSaldoIniLocME + " +
                //           "        balance.CrSaldoIniExtME)ELSE 0 END InicialME " +
                //           " FROM " +
                //           " ( " +
                //           "        SELECT temp.*,comp.Descriptivo ComprobanteDesc,comp.BalanceTipoID,comp.eg_coBalanceTipo " +
                //           "        ,cuenta.Descriptivo CuentaDesc,csaldo.coSaldoControl SaldoControl   " +
                //           "        FROM " +
                //           "        ( " +
                //           "            SELECT aux.*,docCtrl.DocumentoNro DocumentoNumero,docCtrl.PrefijoID DocumentoPrefijo,docCtrl.DocumentoTercero  " +
                //           "            FROM coAuxiliar aux " +
                //           "            LEFT JOIN glDocumentoControl docCtrl WITH(NOLOCK) ON(aux.IdentificadorTR=docCtrl.NumeroDoc AND aux.EmpresaID=docCtrl.EmpresaID)" +
                //           "        )temp " +
                //           "            INNER JOIN coComprobante comp WITH(NOLOCK)ON(temp.ComprobanteID=comp.ComprobanteID AND temp.eg_coComprobante=comp.EmpresaGrupoID)  " +
                //           "            INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)ON(temp.CuentaID=cuenta.CuentaID AND temp.eg_coPlanCuenta=cuenta.EmpresaGrupoID) " +
                //           "            INNER JOIN glConceptoSaldo csaldo WITH(NOLOCK)ON(temp.ConceptoSaldoID=csaldo.ConceptoSaldoID AND temp.eg_glConceptoSaldo=csaldo.EmpresaGrupoID) " +
                //           " )temp1 " +
                //           "    INNER JOIN coBalance balance WITH(NOLOCK)ON(temp1.EmpresaID=balance.EmpresaID AND temp1.PeriodoID=balance.PeriodoID " +
                //           "    AND temp1.BalanceTipoID=balance.BalanceTipoID AND temp1.CuentaID=balance.CuentaID AND temp1.ProyectoID=balance.ProyectoID " +
                //           "    AND temp1.CentroCostoID=balance.CentroCostoID AND temp1.LineaPresupuestoID=balance.LineaPresupuestoID " +
                //           "    AND temp1.eg_coBalanceTipo=balance.eg_coBalanceTipo AND temp1.eg_coPlanCuenta=balance.eg_coPlanCuenta " +
                //           "    AND temp1.eg_coProyecto=balance.eg_coProyecto AND temp1.eg_coCentroCosto=balance.eg_coCentroCosto " +
                //           "    AND temp1.eg_plLineaPresupuesto=balance.eg_plLineaPresupuesto) " +
                //           "    INNER JOIN coTercero as tercero WITH(NOLOCK)ON tercero.TerceroID = temp1.TerceroID and tercero.EmpresaGrupoID = temp1.eg_coTercero " +
                //           " WHERE  temp1.EmpresaID = @EmpresaID  " +
                //           "    AND Month(temp1.PeriodoID) >=@fechaInic " +
                //           "    AND Year(temp1.PeriodoID) = @Año " +
                //           "    AND Month(temp1.PeriodoID) <= @fechaFin " +
                //           "    AND ( temp1.BalanceTipoID = @Libro)  " +
                //               tipoCuenta + tipoTercero + tipoProyecto + centroCos + lineaPres +
                //           "    ORDER BY temp1.CuentaID,temp1.ComprobanteNro,temp1.ComprobanteID "; 
                #endregion

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@_peridoIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@_peridoFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@CuentaFin", SqlDbType.Char);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@_peridoIni"].Value = fechaInicial;
                mySqlCommandSel.Parameters["@_peridoFin"].Value = fechaFinal;
                mySqlCommandSel.Parameters["@libro"].Value = libro;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaInicial;
                mySqlCommandSel.Parameters["@CuentaFin"].Value = cuentaFin;

                #endregion

                DTO_ReportLibroDiario auxCuenta = null;

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    auxCuenta = new DTO_ReportLibroDiario(dr, false);
                    aux.Add(auxCuenta);
                }
                dr.Close();

                return aux;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Dal_ReportesContabilidad_AuxiliarCuentaFuncinal");
                throw exception;
            }

        }

        #endregion

        #region Auxiliar por Tercero

        /// <summary>
        /// Funcion que carga el DTO
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportLibroDiario> DAL_ReportesContabilidad_AuxiliarCuentaFuncinalxTercero(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            try
            {
                List<DTO_ReportLibroDiario> aux = new List<DTO_ReportLibroDiario>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string whereAll = "";
                string whereAux = "";

                if (!string.IsNullOrEmpty(cuentaInicial) || !string.IsNullOrEmpty(tercero) || !string.IsNullOrEmpty(proyecto) || !string.IsNullOrEmpty(centroCosto) ||
                    !string.IsNullOrEmpty(lineaPresupuestal))
                {
                    if (!string.IsNullOrEmpty(cuentaInicial))
                    {
                        if (!string.IsNullOrEmpty(cuentaFin))
                        {
                            whereAux = " AND aux.CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                            whereAll = " WHERE CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                        }
                        else
                        {
                            whereAux = " AND aux.CuentaID = " + "'" + cuentaInicial + "'";
                            whereAll = " WHERE CuentaID = " + "'" + cuentaInicial + "'";
                        }
                    }

                    if (!string.IsNullOrEmpty(tercero))
                    {
                        whereAux += " AND aux.TerceroID = " + "'" + tercero + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE TerceroID in ('','" + tercero + "')" :
                                " AND TerceroID in ('','" + tercero + "')";
                    }

                    if (!string.IsNullOrEmpty(proyecto))
                    {
                        whereAux += " AND aux.ProyectoID = " + "'" + proyecto + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE ProyectoID in ('','" + proyecto + "')" :
                                " AND ProyectoID in ('','" + proyecto + "')";
                    }

                    if (!string.IsNullOrEmpty(centroCosto))
                    {
                        whereAux += " AND aux.CentroCostoID = " + "'" + centroCosto + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE CentroCostoID in ('','" + centroCosto + "')" :
                                " AND CentroCostoID in ('','" + centroCosto + "')";
                    }

                    if (!string.IsNullOrEmpty(lineaPresupuestal))
                    {
                        whereAux += " AND aux.LineaPresupuestoID = " + "'" + lineaPresupuestal + "'";
                        whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE LineaPresupuestoID in ('','" + lineaPresupuestal + "')" :
                                " AND LineaPresupuestoID in ('','" + lineaPresupuestal + "')";
                    }
                }

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                   " SELECT * FROM " +
                    " ( " +
                            " SELECT  " +
                                " '' AS Comprobante,  " +
                                " '' AS ComprobanteID, '' AS ComprobanteNro, '' AS DocumentoCOM, '' AS Descriptivo, PeriodoID, aux.CuentaID, " +
                                " cta.Descriptivo AS CuentaDesc, '' AS Fecha, '' AS ComprobanteDesc, aux.TerceroID,SUBSTRING(tercero.Descriptivo,1,22) as nomTercero, " +
                                " '' AS CentroCostoID, '' AS LineaPresupuestoID, '' AS ProyectoID, 0 AS TasaCambioBase, BalanceTipoID, '' AS SaldoControl,  " +
                                " '' AS DocumentoNumero, '' as DocumentoPrefijo, '' AS DocumentoTercero,  " +
                                " 0 AS vlrBaseML, 0 AS vlrBaseME, 0 AS DebitoML, 0 AS CreditoML, 0 AS DebitoME, 0 AS CreditoME,	  " +
                                " SUM(DbSaldoIniLocML+DbSaldoIniExtML+CrSaldoIniLocML+CrSaldoIniExtML) InicialML,  " +
                                " SUM(DbSaldoIniLocME+DbSaldoIniExtME+CrSaldoIniLocME+CrSaldoIniExtME)  InicialME  " +
                            " FROM coCuentaSaldo aux with(nolock)  " +
                            "   INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta " +
                            "   INNER JOIN coTercero as tercero WITH(NOLOCK) ON tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                    " AND aux.PeriodoID = @_peridoIni " +
                                    " AND aux.BalanceTipoID = @Libro " + whereAux +
                    /*AND aux.CuentaID = '13050501'*/
                            " GROUP BY PeriodoID,aux.CuentaID,cta.Descriptivo,BalanceTipoID,aux.TerceroID, tercero.Descriptivo" +
                        " UNION ALL " +
                            " SELECT  " +
                                " RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                                " aux.ComprobanteID, aux.ComprobanteNro, aux.DocumentoCOM, SUBSTRING( aux.Descriptivo,1,37) as Descriptivo, aux.PeriodoID, aux.CuentaID, " +
                                " cta.Descriptivo as CuentaDesc, aux.Fecha, '' as ComprobanteDesc,aux.TerceroID,SUBSTRING(tercero.Descriptivo,1,22) as nomTercero, " +
                                " aux.CentroCostoID ,aux.LineaPresupuestoID,aux.ProyectoID, aux.TasaCambioBase, comp.BalanceTipoID, '' as SaldoControl, " +
                                " aux.DocumentoCOM as DocumentoNumero, '' as DocumentoPrefijo, aux.DocumentoCOM as DocumentoTercero,  " +
                                " aux.vlrBaseML, aux.vlrBaseME,	  " +
                                " CASE WHEN aux.vlrMdaLoc >= 0 THEN aux.vlrMdaLoc ELSE 0 END AS DebitoML, " +
                                " CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc * -1 ELSE 0 END AS CreditoML, " +
                                " CASE WHEN aux.vlrMdaExt >= 0 THEN aux.vlrMdaExt ELSE 0 END AS DebitoME, " +
                                " CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt * -1 ELSE 0 END AS CreditoME, " +
                                " 0 InicialML,    0 InicialME  " +
                            " FROM coAuxiliar aux  WITH(NOLOCK)  " +
                                " INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta  " +
                                " INNER JOIN coComprobante comp WITH(NOLOCK) ON aux.ComprobanteID=comp.ComprobanteID AND aux.eg_coComprobante = comp.EmpresaGrupoID  " +
                                " INNER JOIN coTercero as tercero WITH(NOLOCK) ON tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero   " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                " AND aux.PeriodoID BETWEEN @_peridoIni AND @_peridoFin AND DAY(aux.PeriodoID) != 2  " + whereAux +
                    /*	AND aux.CuentaID = '13050501'*/
                    " ) AS Auxiliar " + whereAll +
                    "ORDER BY CuentaID, Fecha, Comprobante";
                #endregion
                #region Parametros

                //mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@_peridoIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@_peridoFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@CuentaFin", SqlDbType.Char);

                #endregion
                #region Valores Parametros

                //mySqlCommandSel.Parameters["@Año"].Value = año;
                mySqlCommandSel.Parameters["@_peridoIni"].Value = fechaFinal;
                mySqlCommandSel.Parameters["@_peridoFin"].Value = fechaFinal;
                mySqlCommandSel.Parameters["@libro"].Value = libro;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaInicial;
                mySqlCommandSel.Parameters["@CuentaFin"].Value = cuentaFin;

                #endregion

                DTO_ReportLibroDiario auxCuenta = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    auxCuenta = new DTO_ReportLibroDiario(dr, false);
                    aux.Add(auxCuenta);
                }
                dr.Close();

                return aux;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_AuxiliarCuentaFuncinalxTercero");
                throw exception;
            }

        }

        #endregion

        #region Balance
        /// <summary>
        /// Funcion que carga el DTO de Compracion de Balance
        /// </summary>
        /// <param name="libroFUNC">Libro FUNCIONAL (Este es fijo)</param>
        /// <param name="libroAux">Parametro de entrada (Libro contra el cual se va a comparar el libro FUNC)</param>
        /// <param name="fechaFin">Parametro para ver el mes inicial que se desea imprimir</param>
        /// <param name="fechaIni">Paramtro del mes hasta donde se desea que se imprima el reporte</param>
        /// <returns>List de DTO</returns>
        public List<DTO_ReportBalanceComparativo> DAL_ReportesContabilidad_BalanceComparativo(string libroFUNC, string libroAux, DateTime fechaFin, DateTime fechaIni, int año)
        {
            try
            {
                List<DTO_ReportBalanceComparativo> result = new List<DTO_ReportBalanceComparativo>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommandSel.CommandText =
                    "select consulta.*, (consulta.saldoNuevoFuncML - consulta.saldoNuevoAuxML) as ajusteNuevoML, " +
                    "        (consulta.saldoNuevoFuncME - consulta.saldoNuevoAuxME) as ajusteNuevoME " +
                    " FROM " +
                    "(SELECT cta.cuentaid as cuentaid, cta.Descriptivo, " +
                    "   case when LibroFunc.PeriodoID is null then LibroAux.PeriodoID else LibroFunc.PeriodoID end as PeriodoID, " +
                    "   case when LibroFunc.Descriptivo is null then @librofunc else LibroFunc.Descriptivo end as libroFunc, " +
                    "   case when LibroFunc.saldoInicialFuncML is null then 0 else LibroFunc.saldoInicialFuncML end as saldoInicialFuncML, " +
                    "   case when LibroFunc.saldoInicialFuncME is null then 0 else LibroFunc.saldoInicialFuncME end as saldoInicialFuncME, " +
                    "   case when LibroFunc.VlrMtoFuncML is null		  then 0 else LibroFunc.VlrMtoFuncML end as VlrMtoFuncML, " +
                    "   case when LibroFunc.VlrMtoFuncME is null		  then 0 else LibroFunc.VlrMtoFuncME end as VlrMtoFuncME, " +
                    "   case when LibroAux.Descriptivo is null then @libroAux else LibroAux.Descriptivo end as libroAux, " +
                    "   case when LibroAux.saldoInicialLibroML is null then 0 else LibroAux.saldoInicialLibroML end as saldoInicialLibroML, " +
                    "   case when LibroAux.saldoInicialLibroME is null then 0 else LibroAux.saldoInicialLibroME end as saldoInicialLibroME, " +
                    "   case when LibroAux.VlrMtoLibroML is null then 0 else LibroAux.VlrMtoLibroML end as VlrMtoLibroML, " +
                    "   case when LibroAux.VlrMtoLibroME is null then 0 else LibroAux.VlrMtoLibroME end as VlrMtoLibroME, " +
                    "  (case when LibroFunc.saldoInicialFuncML is null then 0 else LibroFunc.saldoInicialFuncML end - " +
                    "   case when LibroAux.saldoInicialLibroML is null then 0 else LibroAux.saldoInicialLibroML end) as ajusteSaldoML, " +
                    "  (case when LibroFunc.saldoInicialFuncME is null then 0 else LibroFunc.saldoInicialFuncME end - " +
                    "   case when LibroAux.saldoInicialLibroME is null then 0 else LibroAux.saldoInicialLibroME end) as ajusteSaldoME, " +
                    "  (case when LibroFunc.VlrMtoFuncML		  is null then 0 else LibroFunc.VlrMtoFuncML	   end - " +
                    "   case when LibroAux.VlrMtoLibroML		  is null then 0 else LibroAux.VlrMtoLibroML	   end)	as ajusteMovientoML, " +
                    "  (case when LibroFunc.VlrMtoFuncME       is null then 0 else LibroFunc.VlrMtoFuncME	   end - " +
                    "   case when LibroAux.VlrMtoLibroME       is null then 0 else LibroAux.VlrMtoLibroME	   end)	as ajusteMovientoME, " +
                    "  (case when LibroFunc.saldoInicialFuncML is null then 0 else LibroFunc.saldoInicialFuncML end + " +
                    "   case when LibroFunc.VlrMtoFuncML       is null then 0 else LibroFunc.VlrMtoFuncML	   end)	as saldoNuevoFuncML, " +
                    "  (case when LibroFunc.saldoInicialFuncME is null then 0 else LibroFunc.saldoInicialFuncME end + " +
                    "   case when LibroFunc.VlrMtoFuncME		  is null then 0 else LibroFunc.VlrMtoFuncME	   end)	as saldoNuevoFuncME, " +
                    "  (case when LibroAux.saldoInicialLibroML is null then 0 else LibroAux.saldoInicialLibroML end + " +
                    "   case when LibroAux.VlrMtoLibroML 	  is null then 0 else LibroAux.VlrMtoLibroML	   end) as saldoNuevoAuxML, " +
                    "  (case when LibroAux.saldoInicialLibroME is null then 0 else LibroAux.saldoInicialLibroME end + " +
                    "   case when LibroAux.VlrMtoLibroME 	  is null then 0 else LibroAux.VlrMtoLibroME	   end) as saldoNuevoAuxME " +
                    "FROM coPlanCuenta as cta with(nolock) " +
                    "  left JOIN ( " +
                    "    SELECT coBalance.PeriodoID, balTipo.Descriptivo,CuentaID,coBalance.eg_coPlanCuenta, " +
                    "            SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS saldoInicialFuncML, " +
                    "            SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) as  saldoInicialFuncME, " +
                    "            SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as VlrMtoFuncML, " +
                    "            SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as VlrMtoFuncME " +
                    "    FROM coBalance with(nolock) " +
                    "            INNER JOIN coBalanceTipo as balTipo with(nolock) on balTipo.BalanceTipoID = coBalance.BalanceTipoID and balTipo.EmpresaGrupoID = coBalance.eg_coBalanceTipo " +
                    "    WHERE EmpresaID = @empresa and coBalance.BalanceTipoID = @libroFunc " +
                    "            and DATEPART(MONTH, PeriodoID)=@fechaInic and DATEPART(YEAR, PeriodoID) = @año " +
                    "    GROUP BY coBalance.BalanceTipoID,  balTipo.Descriptivo,CuentaID, PeriodoID,coBalance.eg_coPlanCuenta " +
                    "        ) AS LibroFunc on cta.CuentaID = LibroFunc.CuentaID and cta.EmpresaGrupoID = LibroFunc.eg_coPlanCuenta " +
                    "  left JOIN ( " +
                    "    SELECT coBalance.PeriodoID, balTipo.Descriptivo,CuentaID,coBalance.eg_coPlanCuenta, " +
                    "            SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS saldoInicialLibroML, " +
                    "            SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) as  saldoInicialLibroME, " +
                    "            SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML) as VlrMtoLibroML, " +
                    "            SUM(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME) as VlrMtoLibroME " +
                    "    FROM coBalance with(nolock) " +
                    "            INNER JOIN coBalanceTipo as balTipo with(nolock) on balTipo.BalanceTipoID = coBalance.BalanceTipoID and balTipo.EmpresaGrupoID = coBalance.eg_coBalanceTipo " +
                    "    WHERE EmpresaID = @empresa and coBalance.BalanceTipoID = @libroAux " +
                    "            and DATEPART(MONTH, PeriodoID)=@fechaInic and DATEPART(YEAR, PeriodoID) = @año " +
                    "    GROUP BY coBalance.BalanceTipoID,  balTipo.Descriptivo,CuentaID, PeriodoID,coBalance.eg_coPlanCuenta " +
                    "           ) AS LibroAux on  cta.CuentaID = LibroAux.CuentaID and cta.EmpresaGrupoID = LibroAux.eg_coPlanCuenta " +
                    "where  librofunc.saldoInicialFuncML <> 0 OR librofunc.saldoInicialFuncME <> 0 OR librofunc.VlrMtoFuncML <> 0 OR librofunc.VlrMtoFuncME <> 0 OR " +
                    "       libroaux.saldoInicialLibroML <> 0 OR libroaux.saldoInicialLibroME <> 0 OR libroaux.VlrMtoLibroML <> 0 OR libroaux.VlrMtoLibroME <> 0 " +
                    ") as Consulta " +
                    "ORDER BY CuentaID ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@libroAux", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@libroFunc", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaInic", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@año", SqlDbType.Int);

                #endregion
                #region Asignacion de Valores a los parametros

                mySqlCommandSel.Parameters["@libroAux"].Value = libroAux;
                mySqlCommandSel.Parameters["@libroFunc"].Value = libroFUNC;
                mySqlCommandSel.Parameters["@empresa"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaInic"].Value = fechaIni.Month;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin.Month;
                mySqlCommandSel.Parameters["@año"].Value = año;

                #endregion

                DTO_ReportBalanceComparativo balance = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    balance = new DTO_ReportBalanceComparativo(dr);
                    result.Add(balance);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReporteContabilidad_BalanceComparativo");
                throw exception;
            }
        }

        #endregion

        #region Certificados

        /// <summary>
        /// Funcion q se encarga de consultar la informacion para general el cerficado de ReteFuente
        /// </summary>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="Impuesto">Impuesto a generar el Certificado</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_Certificates> DAL_ReportesContabilidad_CertificadoRetencion(DateTime Periodo, string Impuesto, string terceroID, string nitDIAN)
        {
            try
            {
                List<DTO_Certificates> result = new List<DTO_Certificates>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string where = string.Empty;

                #region Filtros
                if(string.IsNullOrEmpty(terceroID))
                {
                    where =" and aux.TerceroID = @TerceroID ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                }
                #endregion
                #region CommandText

                mySqlCommandSel.CommandText =
                   " Select " +
                   "    imp.Tercero," +
                   "     imp.Nombre," +
                   "     imp.DescCuenta," +
                   "     sum(imp.Base) as Base," +
                   "     sum(imp.Valor) as Valor" +
                   " From " +
                   "     (select year(aux.periodoID)*100+MONTH(aux.PeriodoID) as Mes," +
                   "      aux.CuentaID, cta.descriptivo as CuentaDesc," +
                   "      aux.TerceroID, ter.descriptivo as Nombre,  " +
                   "      sum((case when cta.Naturaleza = 2 then  1 else -1 end) * aux.vlrBaseML) as Base," +
                   "      sum((case when cta.Naturaleza = 2 then -1 else  1 end) * aux.vlrMdaLoc) as Valor" +
                   "     From coAuxiliar aux" +
                   "        left join coPlanCuenta cta on aux.CuentaID  = cta.CuentaID  and aux.EmpresaID = cta.EmpresaGrupoID " +
                   "        left join coTercero  Ter on aux.TerceroID = ter.TerceroID and aux.EmpresaID = ter.EmpresaGrupoID " +
                   "     Where EmpresaID = @empresa and cta.ImpuestoTipoID=@Impuesto and aux.terceroID <> @NitDIAN and " +
                   "           year(aux.periodoID) = @Año " + where +
                   "        group by aux.periodoID, aux.CuentaID, cta.descriptivo, aux.TerceroID, ter.descriptivo) Imp" +
                   " Group by Imp.TerceroID, Imp.Nombre, Imp.CuentaDesc" +
                   " Order by Imp.TerceroID, Imp.CuentaDesc";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@Impuesto", SqlDbType.Char, UDT_ImpuestoDeclID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NitDIAN", SqlDbType.Char, UDT_TerceroID.MaxLength);

                #endregion
                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Año"].Value = Periodo.Year;
                mySqlCommandSel.Parameters["@Impuesto"].Value = Impuesto;
                mySqlCommandSel.Parameters["@NitDIAN"].Value = nitDIAN;

                #endregion

                DTO_Certificates reteFuente = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    reteFuente = new DTO_Certificates(dr);
                    result.Add(reteFuente);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_CertificadoRetencion");
                throw exception;
            }
        }

        #endregion

        #region Comprobantes

        #region Comrpobante Diario

        /// <summary>
        /// COnsulta que llena el DTO de comprobante
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes a mostrar</param>
        /// <param name="comprobanteID">Comprobante que se desea ver</param>
        /// <param name="libro">Libro que se desea ver</param>
        /// <param name="comprobanteInicial">Nro de comprobonate inicial para filtrar </param>
        /// <param name="comprobanteFinal">Nro de comprobante final para filtrar</param>
        /// <returns></returns>
        public List<DTO_ReportComprobante> DAL_ReportesContabilidad_Comprobante(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            try
            {
                List<DTO_ReportComprobante> result = new List<DTO_ReportComprobante>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtro

                string filtro = "", filtroComprobante = "";

                if (!string.IsNullOrEmpty(comprobanteID))
                    filtro = " AND base.ComprobanteID = " + "'" + comprobanteID + "'";
                if (!string.IsNullOrEmpty(comprobanteInicial) && !string.IsNullOrEmpty(comprobanteFinal))
                    filtroComprobante = " AND base.ComprobanteNro BETWEEN " + Convert.ToInt32(comprobanteInicial) + " AND " + Convert.ToInt32(comprobanteFinal);

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT RTRIM (CAST(base.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(base.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                             " compro.BalanceTipoID, base.PeriodoID, base.Fecha, base.ComprobanteID, base.ComprobanteNro, base.CuentaID, cuenta.Descriptivo AS CuentaDesc, base.CentroCostoID, " +
                             " base.TerceroID, SUBSTRING( ter.Descriptivo,1,37) as nomTercero,base.ProyectoID,base.LineaPresupuestoID AS Linea,base.ConceptoCargoID,base.DocumentoCOM,SUBSTRING( base.Descriptivo,1,43) as Descriptivo , base.vlrBaseML, base.vlrBaseME, " +
                             " CASE WHEN base.vlrMdaLoc > 0  THEN base.vlrMdaLoc ELSE 0 END AS DebitoLoc, " +
                             " ABS(CASE WHEN base.vlrMdaLoc < 0  THEN base.vlrMdaLoc ELSE 0  END) AS CreditoLoc,  " +
                             " CASE WHEN base.vlrMdaExt > 0 THEN vlrMdaExt ELSE 0 END AS DebitoExt,  " +
                             " ABS(CASE WHEN base.vlrMdaExt < 0  THEN base.vlrMdaExt ELSE 0  END) AS CreditoExt, " +
                             " seUsuario.UsuarioID," +
                             " 'Doc: ' + '[ ' + glDoc.Descriptivo + ' ' + '-' + ' ' + RTRIM(CAST(glcontrol.DocumentoID as CHAR(10))) +' ]' + ' - ' + CAST(glcontrol.NumeroDoc as CHAR(10)) as DescDocumento " +
                    " FROM coAuxiliar AS base WITH(NOLOCK) " +
                        " INNER JOIN glDocumentoControl glcontrol WITH(NOLOCK) on glcontrol.NumeroDoc = base.NumeroDoc " +
                        " INNER JOIN seUsuario WITH(NOLOCK) on seUsuario.ReplicaID = glcontrol.seUsuarioID  " +
                        " INNER JOIN coComprobante compro WITH(NOLOCK) On (compro.ComprobanteID = base.ComprobanteID and compro.EmpresaGrupoID = base.eg_coComprobante) " +
                        " INNER JOIN coTercero ter WITH(NOLOCK) ON (ter.TerceroID=base.TerceroID and ter.EmpresaGrupoID=base.eg_coTercero) " +
                        " INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) ON cuenta.CuentaID=base.CuentaID and cuenta.EmpresaGrupoID=base.eg_coPlanCuenta " +
                        " INNER JOIN glDocumento glDoc WITH(NOLOCK)  ON glDoc.DocumentoID = glcontrol.DocumentoID " +
                    " WHERE base.EmpresaID = @EmpresaID " +
                            "  AND DATEPART(YEAR, base.PeriodoID) = @ano  " +
                            "  AND  DATEPART(MONTH, base.PeriodoID) = @mes  " +
                            "  AND ( 1 = 1 ) " + filtro + filtroComprobante +
                            "  AND compro.BalanceTipoID = @libro ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@ano"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;
                mySqlCommandSel.Parameters["@libro"].Value = libro;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                #endregion

                DTO_ReportComprobante comprobante = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    comprobante = new DTO_ReportComprobante(dr, false);
                    result.Add(comprobante);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_Comprobante");
                throw exception;
            }

        }


        #endregion

        #region Comrpobante Preliminar

        /// <summary>
        /// Funcion que carga el DTO de comprobante Preliminar
        /// </summary>
        /// <param name="año">Año que se va a mostrar en el reporte</param>
        /// <param name="mes">Mes Que sepor el cual se va a filtrar el reporte</param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportComprobante> DAL_ReportesContabilidad_ComprobantePreliminar(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            try
            {
                List<DTO_ReportComprobante> result = new List<DTO_ReportComprobante>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtro

                string filtro = "", filtroComprobante = "";

                if (!string.IsNullOrEmpty(comprobanteID))
                    filtro = " AND base.ComprobanteID = " + "'" + comprobanteID + "'";
                if (!string.IsNullOrEmpty(comprobanteInicial) && !string.IsNullOrEmpty(comprobanteFinal))
                    filtroComprobante = " AND base.ComprobanteNro BETWEEN " + Convert.ToInt32(comprobanteInicial) + " AND " + Convert.ToInt32(comprobanteFinal);

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT RTRIM (CAST(base.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(base.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                             " base.PeriodoID, base.Fecha, base.ComprobanteID, base.ComprobanteNro, base.CuentaID, null as CuentaDesc,  " +
                             " base.TerceroID,ter.Descriptivo as nomTercero, base.ProyectoID,base.ConceptoCargoID,base.LineaPresupuestoID AS Linea, SUBSTRING(base.Descriptivo,1,37) as Descriptivo , base.vlrBaseML, " +
                             " CASE WHEN base.vlrMdaLoc > 0  THEN base.vlrMdaLoc ELSE 0 END AS DebitoLoc,base.DatoAdd3, " +
                             " ABS(CASE WHEN base.vlrMdaLoc < 0  THEN base.vlrMdaLoc ELSE 0  END) AS CreditoLoc  " +
                    " FROM coAuxiliarPre AS base " +
                    " LEFT JOIN coTercero ter WITH(NOLOCK) ON (ter.TerceroID=base.TerceroID and ter.EmpresaGrupoID=base.eg_coTercero) " +
                    " WHERE base.EmpresaID = @EmpresaID " +
                            "  AND DATEPART(YEAR, base.PeriodoID) = @ano  " +
                            "  AND  DATEPART(MONTH, base.PeriodoID) = @mes  " +
                            "  AND ( 1 = 1 ) " + filtro + filtroComprobante +
                    " GROUP BY base.ComprobanteID,base.ComprobanteID, base.ComprobanteNro, base.CuentaID,  " +
                              " base.TerceroID,ter.Descriptivo,base.ProyectoID,base.ConceptoCargoID,base.LineaPresupuestoID,  " +
                              "  base.Descriptivo,base.vlrMdaLoc,base.PeriodoID,base.Fecha,base.vlrBaseML,base.DatoAdd3";
                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@ano"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                #endregion

                DTO_ReportComprobante comprobante = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    comprobante = new DTO_ReportComprobante(dr, true);
                    comprobante.DatoAdd.Value = dr["DatoAdd3"].ToString();
                    comprobante.nomTercero.Value = dr["nomTercero"].ToString();
                    result.Add(comprobante);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_ComprobantePreliminar");
                throw exception;
            }

        }
        #endregion

        #region Comprobante Control

        /// <summary>
        /// Funcion que carga el Comprobante para llevar un control
        /// </summary>
        /// <param name="año">Peridio que se quiere verificar</param>
        /// <param name="mes">Mes que se quiere verificar</param>
        /// <returns></returns>
        public List<DTO_ReportComprobanteControl> DAL_ReportesContabilidad_ComprobanteControl(int año, int mes)
        {
            try
            {
                List<DTO_ReportComprobanteControl> control = new List<DTO_ReportComprobanteControl>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT BASE.PeriodoID, base.ComprobanteID,comp.Descriptivo ComprobanteDesc,base.ComprobanteNro, " +
                             " COUNT(base.CuentaID) RecordQty " +
                    " FROM coAuxiliar base  " +
                         " inner join coComprobante comp with(nolock) on (base.ComprobanteID = comp.ComprobanteID and base.eg_coComprobante=comp.EmpresaGrupoID) " +
                    " WHERE base.EmpresaID = @EmpresaID " +
                            " AND DATEPART(YEAR, base.PeriodoID) = @ano  " +
                            " AND  DATEPART(MONTH, base.PeriodoID) = @mes " +
                    " GROUP BY base.ComprobanteID,comp.Descriptivo,base.ComprobanteNro,BASE.PeriodoID " +
                    " ORDER BY base.ComprobanteID,base.ComprobanteNro  ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@ano"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                #endregion

                DTO_ReportComprobanteControl comprobanteControl = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    comprobanteControl = new DTO_ReportComprobanteControl(dr);
                    control.Add(comprobanteControl);
                }
                dr.Close();

                return control;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_ComprobanteControl");
                throw exception;
            }
        }

        #endregion

        #endregion

        #region Inventario Balance

        /// <summary>
        ///  Funcion que carga una lista de DTO para el reporte Inventario y Balance
        /// </summary>
        /// <param name="mesFin">Mes que se necesita investigar</param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportInventariosBalance> DAL_ReportesContabilidad_InventariosBalance(int mesIni, int mesFin, string libro, string cuentaIni, string cuentaFin, int _año)
        {
            try
            {
                List<DTO_ReportInventariosBalance> result = new List<DTO_ReportInventariosBalance>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string filtroCuentasBalance = string.Empty, filtroCuentaCtaSaldo = string.Empty;

                if (!string.IsNullOrEmpty(cuentaIni) && !string.IsNullOrEmpty(cuentaFin))
                {
                    filtroCuentasBalance = " AND bal.CuentaID between @cuentaIni and @cuentaFin ";
                    filtroCuentaCtaSaldo = " AND CuentaID between @cuentaIni and @cuentaFin ";
                }

                if (!string.IsNullOrEmpty(cuentaIni) && string.IsNullOrEmpty(cuentaFin))
                {
                    filtroCuentasBalance = " AND bal.CuentaID = @cuentaIni ";
                    filtroCuentaCtaSaldo = " AND CuentaID = @cuentaIni ";
                }

                #endregion
                #region CommanText
                mySqlCommandSel.CommandText =
                                "    SELECT EmpresaID, cuentaID,CuentaDesc,    TerceroID,  TerceroDesc,   " +
                                "   BalanceTipoID,  sum(DebitoML_Cuenta) DebitoML_Cuenta, sum(CreditoML_Cuenta) CreditoML_Cuenta,  sum(InicialML_Cuenta) InicialML_Cuenta,       " +
                                "   sum(FinalML_Cuenta) FinalML_Cuenta, sum(DebitoML_Tercero) DebitoML_Tercero,   sum(CreditoML_Tercero) CreditoML_Tercero,   sum(InicialML_Tercero) InicialML_Tercero,      " +
                                "   sum(FinalML_Tercero) FinalML_Tercero,    TerceroInd from (   " +
                                "       SELECT	bal.EmpresaID,      " +
                                "       		bal.cuentaID,       " +
                                "       		bal.PeriodoID,       " +
                                "       		TerceroID,        " +
                                "       		TerceroDesc,       " +
                                "       		bal.BalanceTipoID,        " +
                                "       		cta.Descriptivo as CuentaDesc,      " +
                                "       		SUM(bal.DbOrigenLocML + bal.DbOrigenExtML) DebitoML_Cuenta,      " +
                                "       		SUM(bal.CrOrigenLocML +  bal.CrOrigenExtML) CreditoML_Cuenta,      " +
                                "       		CASE When Month(bal.PeriodoID) = @MonthIni Then SUM(bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML + bal.CrSaldoIniExtML) Else 0 End as InicialML_Cuenta,      " +
                                "       		SUM(bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML +  bal.CrSaldoIniExtML + bal.DbOrigenLocML + bal.DbOrigenExtML + bal.CrOrigenLocML + bal.CrOrigenExtML) as FinalML_Cuenta,      " +
                                "       		DebitoML_Tercero,       " +
                                "       		(CreditoML_Tercero)*(-1) as CreditoML_Tercero,       " +
                                "       		InicialML_Tercero,      " +
                                "       		(InicialML_Tercero + DebitoML_Tercero + (CreditoML_Tercero * -1)) as FinalML_Tercero,       " +
                                "       		cta.TerceroInd      " +
                                "       FROM		coBalance		bal      " +
                                "       	INNER JOIN	coPlanCuenta	cta WITH(NOLOCK) on (cta.cuentaID = bal.CuentaID AND cta.EmpresaGrupoID = bal.eg_coPlanCuenta)      " +
                                "       	LEFT JOIN       " +
                                "       	(	SELECT	EmpresaID,       " +
                                "       			cuentaID,	      " +
                                "       			ctaSaldo.PeriodoID,       " +
                                "       			ctaSaldo.TerceroID,      " +
                                "       			ter.Descriptivo as TerceroDesc,       " +
                                "       			BalanceTipoID, '' as CuentaDesc,      " +
                                "       			SUM((ctaSaldo.DbOrigenLocML + ctaSaldo.DbOrigenExtML)) DebitoML_Tercero,      " +
                                "       			SUM((ctaSaldo.CrOrigenLocML + ctaSaldo.CrOrigenExtML))*(-1) CreditoML_Tercero,      " +
                                "                   CASE When Month(ctaSaldo.PeriodoID) = @MonthIni Then SUM((ctaSaldo.DbSaldoIniLocML + ctaSaldo.DbSaldoIniExtML + ctaSaldo.CrSaldoIniLocML + ctaSaldo.CrSaldoIniExtML)) Else 0 End  as InicialML_Tercero,      " +
                                "       			SUM((ctaSaldo.DbSaldoIniLocML + ctaSaldo.DbSaldoIniExtML + ctaSaldo.CrSaldoIniLocML + ctaSaldo.CrSaldoIniExtML + DbOrigenLocML + DbOrigenExtML + ctaSaldo.CrOrigenLocML + ctaSaldo.CrOrigenExtML)) as FinalML_Tercero      " +
                                "       	FROM	coCuentaSaldo ctaSaldo      " +
                                "       		LEFT JOIN coTercero  ter with(nolock) on (ter.TerceroID = ctaSaldo.TerceroID and ter.EmpresaGrupoID = ctaSaldo.eg_coTercero)      " +
                                "       	WHERE	EmpresaID = @EmpresaID      " +
                                "       			AND Month(PeriodoID)  >= @MonthIni      " +
                                "       			AND Month(PeriodoID)  <= @MonthFin       " +
                                "       			and year (PeriodoID) = @Año      " +
                                "       			AND BalanceTipoID = @Libro      " +
                                "       			AND ctaSaldo.CuentaID = (CASE WHEN @CuentaIni != '' THEN @CuentaIni ELSE ctaSaldo.CuentaID END)	      " +
                                "       			AND ctaSaldo.CuentaID = (CASE WHEN @CuentaFin != '' THEN @CuentaFin ELSE ctaSaldo.CuentaID END)       " +
                                "       	GROUP BY	CuentaID,       " +
                                "       				ctaSaldo.TerceroID,      " +
                                "       				EmpresaID, PeriodoID,       " +
                                "       				ter.Descriptivo, BalanceTipoID) AS ctaSaldos       " +
                                "       			ON (ctaSaldos.CuentaID = bal.CuentaID AND ctaSaldos.EmpresaID = bal.EmpresaID AND ctaSaldos.BalanceTipoID = bal.BalanceTipoID AND ctaSaldos.PeriodoID = BAL.PeriodoID)      " +
                                "       WHERE	bal.EmpresaID = @EmpresaID      " +
                                "               AND Month(bal.PeriodoID)  >= @MonthIni " +
                                "	            AND Month(bal.PeriodoID)  <= @MonthFin " +
                                "       		AND YEAR(bal.PeriodoID)		=	@Año     " +
                                "       		AND bal.BalanceTipoID = @Libro      " +
                                "       		AND bal.CuentaID = (CASE WHEN @CuentaIni != '' THEN @CuentaIni ELSE bal.CuentaID END)        " +
                                "       		AND bal.CuentaID = (CASE WHEN @CuentaFin != '' THEN @CuentaFin ELSE bal.CuentaID END)        " +
                                "       GROUP BY	bal.CuentaID,       " +
                                "       			bal.empresaID,      " +
                                "       			bal.PeriodoID,       " +
                                "       			bal.BalanceTipoID,       " +
                                "       			cta.Descriptivo,       " +
                                "       			TerceroID,       " +
                                "       			TerceroDesc,       " +
                                "       			DebitoML_Tercero,       " +
                                "       			CreditoML_Tercero,       " +
                                "       			InicialML_Tercero,      " +
                                "       			FinalML_Tercero,       " +
                                "       			TerceroInd      " +
                                "       HAVING		TerceroID IS NULL      " +
                                "       		OR	(DebitoML_Tercero)	!= 0      " +
                                "       		OR	(CreditoML_Tercero) != 0      " +
                                "       		OR	(InicialML_Tercero) != 0      "+
                          "  ) q         " +
                          "        GROUP BY EmpresaID, cuentaID, CuentaDesc, TerceroID, TerceroDesc,     " +
                          "        BalanceTipoID, TerceroInd     ";

                #region NO BORRAR
                //" select bal.EmpresaID, bal.cuentaID, bal.PeriodoID, TerceroID,  TerceroDesc, bal.BalanceTipoID,  cta.Descriptivo as CuentaDesc, " +
                //                              " SUM(bal.DbOrigenLocML + bal.DbOrigenExtML) DebitoML_Cuenta,  " +
                //                              " SUM(bal.CrOrigenLocML +  bal.CrOrigenExtML)*(-1) CreditoML_Cuenta, " +
                //                              " SUM(bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML + bal.CrSaldoIniExtML) as InicialML_Cuenta, " +
                //                              " SUM(/*ABS(*/bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML +  bal.CrSaldoIniExtML + bal.DbOrigenLocML + bal.DbOrigenExtML + bal.CrOrigenLocML " +
                //                                   " + bal.CrOrigenExtML)/*) */ as FinalML_Cuenta, " +
                //                               " SUM(DebitoML_Tercero) as DebitoML_Tercero, SUM(CreditoML_Tercero) as CreditoML_Tercero, SUM(InicialML_Tercero) as InicialML_Tercero, " +
                //                               " SUM(FinalML_Tercero) as FinalML_Tercero, cta.TerceroInd " +
                //                   " from coBalance bal " +
                //                       " INNER JOIN coPlanCuenta cta WITH(NOLOCK) on (cta.cuentaID = bal.CuentaID AND cta.EmpresaGrupoID = bal.eg_coPlanCuenta) " +
                //                       " LEFT JOIN " +
                //                       " (	 " +
                //                               " select EmpresaID, cuentaID ,ctaSaldo.PeriodoID, ctaSaldo.TerceroID ,ter.Descriptivo as TerceroDesc, BalanceTipoID, '' as CuentaDesc, " +
                //                                       " SUM( (ctaSaldo.DbOrigenLocML + ctaSaldo.DbOrigenExtML)) DebitoML_Tercero,  " +
                //                                       " SUM((ctaSaldo.CrOrigenLocML + ctaSaldo.CrOrigenExtML))*(-1) CreditoML_Tercero,    " +
                //                                       " SUM(ABS((ctaSaldo.DbSaldoIniLocML + ctaSaldo.DbSaldoIniExtML + ctaSaldo.CrSaldoIniLocML + ctaSaldo.CrSaldoIniExtML))  )  as InicialML_Tercero,  " +
                //                                       " SUM(ABS((ctaSaldo.DbSaldoIniLocML + ctaSaldo.DbSaldoIniExtML + ctaSaldo.CrSaldoIniLocML + ctaSaldo.CrSaldoIniExtML + DbOrigenLocML + DbOrigenExtML  " +
                //                                           " + ctaSaldo.CrOrigenLocML + ctaSaldo.CrOrigenExtML))) as FinalML_Tercero " +
                //                               " from coCuentaSaldo ctaSaldo " +
                //                               " LEFT JOIN coTercero  ter with(nolock) on (ter.TerceroID = ctaSaldo.TerceroID and ter.EmpresaGrupoID = ctaSaldo.eg_coTercero) " +
                //                               " where  EmpresaID = @EmpresaID  " +
                //                                       " AND Month(PeriodoID)  =  @Month   " +
                //                                       " AND BalanceTipoID = @Libro " +
                //                                       filtroCuentaCtaSaldo +
                //                                       " /*AND CuentaID  = '11051005'*/ " +
                //                               " GROUP BY CuentaID, ctaSaldo.TerceroID, EmpresaID, PeriodoID, ter.Descriptivo, BalanceTipoID " +
                //                       " ) AS ctaSaldos on (ctaSaldos.CuentaID = bal.CuentaID AND ctaSaldos.EmpresaID = bal.EmpresaID) " +
                //                   " WHERE bal.EmpresaID = @EmpresaID   " +
                //                           " AND Month(bal.PeriodoID)  =  @Month 	  " +
                //                           " AND bal.BalanceTipoID = @Libro  " +
                //                           filtroCuentasBalance +
                //                           " /*AND bal.CuentaID  = '11051005' */ " +
                //                   " GROUP BY bal.CuentaID, bal.empresaID,bal.PeriodoID, bal.BalanceTipoID, bal.PeriodoID, cta.Descriptivo, TerceroID, TerceroDesc, TerceroInd "; 
                #endregion

                #region NO BORRAR
                /* " SELECT cuentaID, BalanceTipoID,PeriodoID, CuentaID_pre,CuentaDesc,MovInd,MascaraCta,TerceroInd,SUM(DebitoML_Cuenta) DebitoML_Cuenta,  " +
                        " SUM(CreditoML_Cuenta) CreditoML_Cuenta,	SUM(InicialML_Cuenta)InicialML_Cuenta,SUM(FinalML_Cuenta)FinalML_Cuenta, " +
                        " MaxLengthInd,temp2.TerceroID,SUM(DebitoML_Tercero)DebitoML_Tercero, " +
                        " SUM(CreditoML_Tercero)CreditoML_Tercero, " +
                        " SUM(InicialML_Tercero) InicialML_Tercero, " +
                        " SUM(FinalML_Tercero) FinalML_Tercero,  " +
                        " tercero.Descriptivo TerceroDesc " +
                    " FROM (" +
                    " SELECT CASE WHEN temp1.MovInd = 1 then substring(temp1.CuentaID_pre,1,temp1.MascaraCta) else temp1.CuentaID_pre end CuentaID, temp1.*,  " +
                            " CASE WHEN temp1.MovInd = 1 THEN 1 ELSE 0 END MaxLengthInd,TerceroID,eg_coTercero,  " +
                                 " SUM( (cs.DbOrigenLocML + DbOrigenExtML)) DebitoML_Tercero," +
                                  " SUM((cs.CrOrigenLocML + CrOrigenExtML))*(-1) CreditoML_Tercero,  " +
                            " CASE WHEN (MONTH(cs.PeriodoID)=@Month) THEN SUM(ABS((cs.DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML))  ) " +
                            " ELSE 0 END InicialML_Tercero,  " +
                            " CASE WHEN (MONTH(cs.PeriodoID)=@Month) THEN SUM(ABS((cs.DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML +  " +
                                 " cs.DbOrigenLocML + cs.DbOrigenExtML + cs.CrOrigenLocML + cs.CrOrigenExtML)))  " +
                            " ELSE SUM(ABS((cs.DbOrigenLocML + cs.DbOrigenExtML + cs.CrOrigenLocML + cs.CrOrigenExtML))) END FinalML_Tercero   " +
                     " FROM (SELECT bal.BalanceTipoID ,bal.PeriodoID,bal.CuentaID CuentaID_pre,cuenta.Descriptivo CuentaDesc, cuenta.MovInd,cuenta.MascaraCta,  " +
                                   "  TerceroInd TerceroInd,   " +
                                   "  SUM(bal.DbOrigenLocML + bal.DbOrigenExtML) DebitoML_Cuenta, " +
                                    " SUM(bal.CrOrigenLocML +  bal.CrOrigenExtML)*(-1) CreditoML_Cuenta,  " +
                                   "  SUM(CASE WHEN (MONTH(bal.PeriodoID)=@Month)  " +
                                   "  THEN (bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML + bal.CrSaldoIniExtML) ELSE 0 END) InicialML_Cuenta,  " +
                                   "  SUM(CASE WHEN (MONTH(bal.PeriodoID)=@Month) THEN  ABS(bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML + " +
                                    " bal.CrSaldoIniExtML + bal.DbOrigenLocML + bal.DbOrigenExtML + bal.CrOrigenLocML + bal.CrOrigenExtML) " +
                                   "  ELSE ABS((bal.DbOrigenLocML + bal.DbOrigenExtML + bal.CrOrigenLocML + bal.CrOrigenExtML)) END) FinalML_Cuenta   " +
                     " FROM coBalance bal  " +
                          " inner join coPlanCuenta cuenta with(nolock) on(bal.CuentaID = cuenta.CuentaID and bal.eg_coPlanCuenta=cuenta.EmpresaGrupoID)  " +
                    "  WHERE bal.EmpresaID = @EmpresaID " +
                             " AND (cuenta.MovInd = 1 or LEN(bal.CuentaID) < cuenta.MascaraCta)  " +
                             " AND Month(PeriodoID)  =  @Month  " +
                             " AND bal.BalanceTipoID = @Libro  " +
                             filtroCuentas +
                     " GROUP BY bal.CuentaID,  cuenta.Descriptivo,  cuenta.TerceroInd,  cuenta.MovInd,  cuenta.MascaraCta,   + " +
                                " bal.PeriodoID,  bal.BalanceTipoID   )  " +
                                    "  temp1  " +
                                        " LEFT JOIN coCuentaSaldo cs with(nolock) on (temp1.CuentaID_pre=cs.CuentaID AND temp1.TerceroInd = 1) " +
                                    " WHERE cs.EmpresaID = @EmpresaID  " +
						                " AND  (temp1.MovInd = 1 or LEN(cs.CuentaID) < temp1.MascaraCta)   " +
						                " AND Month(cs.PeriodoID)  =  @Month   " +
						                " AND cs.BalanceTipoID = @Libro  " +
                                        filtroCuentas +
                    " group by temp1.MovInd, temp1.CuentaID_pre,temp1.MascaraCta,temp1.BalanceTipoID,temp1.PeriodoID,temp1.CuentaDesc," +
                            " temp1.TerceroInd,temp1.DebitoML_Cuenta,temp1.CreditoML_Cuenta,temp1.InicialML_Cuenta,temp1.FinalML_Cuenta,TerceroID,eg_coTercero," +
                            " cs.PeriodoID) " +
                                    "  temp2 LEFT JOIN coTercero tercero with(nolock) on(temp2.TerceroID=tercero.TerceroID AND temp2.eg_coTercero=tercero.EmpresaGrupoID) " +
                     " group BY cuentaID, BalanceTipoID,PeriodoID, CuentaID_pre,CuentaDesc,MovInd,MascaraCta,TerceroInd, MaxLengthInd,temp2.TerceroID ,tercero.Descriptivo " +
                     " ORDER BY CuentaID "; */

                #endregion

                #endregion
                #region Paramentros
                mySqlCommandSel.Parameters.Add("@MonthIni", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MonthFin", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Libro", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@cuentaFin", SqlDbType.Char, UDT_CuentaID.MaxLength);
                #endregion
                #region Asignacion de valores a paramentros
                mySqlCommandSel.Parameters["@MonthIni"].Value = mesIni;
                mySqlCommandSel.Parameters["@MonthFin"].Value = mesFin;
                mySqlCommandSel.Parameters["@Año"].Value = _año;
                mySqlCommandSel.Parameters["@Libro"].Value = libro;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaIni;
                mySqlCommandSel.Parameters["@cuentaFin"].Value = cuentaFin;
                #endregion

                DTO_ReportInventariosBalance inventarioBalance = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    inventarioBalance = new DTO_ReportInventariosBalance(dr);
                    //inventarioBalance.PorTercero = new List<DTO_PorTercero>();
                    //if (inventarioBalance.TerceroInd.Value.Value == true)
                    //{
                    //    DTO_PorTercero terceros = new DTO_PorTercero(dr);                        
                    //    if (!string.IsNullOrEmpty(terceros.TerceroID.Value))
                    //        inventarioBalance.PorTercero.Add(terceros);
                    //}

                    result.Add(inventarioBalance);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_InventariosBalance");
                throw exception;
            }
        }

        #endregion

        #region Libros

        #region libro Diario

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte Libro Diario
        /// </summary>
        /// <param name="año">Año del cual se pido el reporte</param>
        /// <param name="mes">Mes que se necesita investigar</param>
        /// <param name="tipoBalance"></param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportLibroDiario> DAL_ReportesContabilidad_LibroDiario(int año, int mes, string tipoBalance)
        {
            try
            {
                List<DTO_ReportLibroDiario> result = new List<DTO_ReportLibroDiario>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    " SELECT aux.PeriodoID,aux.CuentaID, plancu.Descriptivo AS CuentaDesc, aux.ComprobanteID,compro.Descriptivo AS ComprobanteDesc, " +
                            " SUM(CASE WHEN aux.vlrMdaLoc > 0 THEN aux.vlrMdaLoc  ELSE 0 END) AS DebitoML,   " +
                            " ABS(SUM(CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc ELSE 0 END)) AS CreditoML,   " +
                            " SUM(CASE WHEN aux.vlrMdaExt > 0 THEN aux.vlrMdaExt  ELSE 0 END) AS DebitoME,   " +
                            " ABS(SUM(CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt ELSE 0 END)) AS CreditoME   " +
                            " /*RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante ,   " +
                            " aux.ComprobanteNro,    " +
                            " compro.BalanceTipoID, aux.TerceroID, ter.Descriptivo as nomTercero,   " +
                            " aux.CentroCostoID, aux.LineaPresupuestoID,aux.ProyectoID,aux.TasaCambioBase,   " +
                            " aux.vlrBaseML AS InicialML, aux.vlrBaseME InicialME,aux.DocumentoCOM,AUX.Descriptivo,  " +
                            " AUX.vlrBaseME, AUX.vlrBaseML, aux.Fecha  */		 " +
                     " FROM coAuxiliar aux WITH(NOLOCK)   " +
                             " INNER JOIN coPlanCuenta plancu WITH(NOLOCK) ON plancu.CuentaID = aux.CuentaID and plancu.EmpresaGrupoID = aux.eg_coPlanCuenta   " +
                             " INNER JOIN coComprobante compro WITH(NOLOCK) ON  compro.ComprobanteID = aux.ComprobanteID    " +
                                " and compro.EmpresaGrupoID = aux.eg_coComprobante   " +
                             " INNER JOIN coTercero  ter WITH(NOLOCK) ON ter.TerceroID = aux.TerceroID and ter.EmpresaGrupoID = aux.eg_coTercero   " +
                      " WHERE aux.EmpresaID = @EmpresaID   " +
                             " AND DATEPART(YEAR, aux.PeriodoID) = @ano    " +
                             " AND DATEPART(MONTH, aux.PeriodoID) = @mes   " +
                             " AND compro.BalanceTipoID = @tipoBalance   " +
                     " GROUP BY aux.PeriodoID,aux.CuentaID, plancu.Descriptivo, aux.ComprobanteID ,compro.Descriptivo " +
                                                    " /* aux.ComprobanteNro, compro.BalanceTipoID, aux.TerceroID, ter.Descriptivo,  " +
                                                      " aux.CentroCostoID, aux.LineaPresupuestoID,aux.ProyectoID, " +
                                                      " aux.TasaCambioBase, aux.vlrBaseML,aux.vlrBaseME,  " +
                                                     " aux.DocumentoCOM, aux.Descriptivo, aux.EmpresaID, aux.Fecha*/ " +
                       " ORDER By CuentaID ";

                #region NO BORRAR
                /* " SELECT RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante , " +
                                " aux.PeriodoID,aux.CuentaID, plancu.Descriptivo AS CuentaDesc,  " +
                                " aux.ComprobanteNro, aux.ComprobanteID ,  compro.Descriptivo AS ComprobanteDesc , " +
                                " compro.BalanceTipoID, aux.TerceroID, ter.Descriptivo as nomTercero, " +
                                " aux.CentroCostoID, aux.LineaPresupuestoID,aux.ProyectoID,aux.TasaCambioBase, " +
                                " SUM(CASE WHEN aux.vlrMdaLoc > 0 THEN aux.vlrMdaLoc  ELSE 0 END) AS DebitoML, " +
                                " ABS(SUM(CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc ELSE 0 END)) AS CreditoML, " +
                                " SUM(CASE WHEN aux.vlrMdaExt > 0 THEN aux.vlrMdaExt  ELSE 0 END) AS DebitoME,  " +
                                " ABS(SUM(CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt ELSE 0 END)) AS CreditoME, " +
                                " aux.vlrBaseML AS InicialML, aux.vlrBaseME InicialME,aux.DocumentoCOM,AUX.Descriptivo, AUX.Fecha, " +
                                " AUX.vlrBaseME, AUX.vlrBaseML " +
                        " FROM coAuxiliar aux WITH(NOLOCK) " +
                                " INNER JOIN coPlanCuenta plancu WITH(NOLOCK) ON plancu.CuentaID = aux.CuentaID and plancu.EmpresaGrupoID = aux.eg_coPlanCuenta " +
                                " INNER JOIN coComprobante compro WITH(NOLOCK) ON  compro.ComprobanteID = aux.ComprobanteID  " +
                                    "and compro.EmpresaGrupoID = aux.eg_coComprobante " +
                                " INNER JOIN coTercero  ter WITH(NOLOCK) ON ter.TerceroID = aux.TerceroID and ter.EmpresaGrupoID = aux.eg_coTercero " +
                        "  WHERE aux.EmpresaID = @EmpresaID " +
                                " AND DATEPART(YEAR, aux.PeriodoID) = @ano  " +
                                " AND DATEPART(MONTH, aux.PeriodoID) = @mes  " +
                                " AND compro.BalanceTipoID = @tipoBalance " +
                        " GROUP BY aux.PeriodoID,aux.CuentaID, plancu.Descriptivo, " +
                                 " aux.ComprobanteNro, aux.ComprobanteID ,  compro.Descriptivo, " +
                                 " compro.BalanceTipoID, aux.TerceroID, ter.Descriptivo, " +
                                 " aux.CentroCostoID, aux.LineaPresupuestoID,aux.ProyectoID," +
                                 " aux.TasaCambioBase, aux.vlrBaseML,aux.vlrBaseME, " +
                                 " aux.DocumentoCOM, aux.Descriptivo, aux.Fecha,aux.EmpresaID " +
                        "   ORDER By CuentaID,aux.Fecha "; */
                #endregion

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@tipoBalance", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Valores Parametros
                mySqlCommandSel.Parameters["@ano"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;
                mySqlCommandSel.Parameters["@tipoBalance"].Value = tipoBalance;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion

                DTO_ReportLibroDiario aux = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    aux = new DTO_ReportLibroDiario(dr, true);
                    result.Add(aux);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReportLibroDiario");
                throw exception;
            }
        }

        #endregion

        #region Libro Mayor

        /// <summary>
        /// Funcion que carga una lista de DTO para el reporte Libro Mayor
        /// </summary>
        /// <param name="año">Año del cual se pido el reporte</param>
        /// <param name="mes">Mes que se necesita investigar</param>
        /// <param name="tipoBalance">Libro que se va a imprimir</param>
        /// <param name="cuentaIni">Filtro para rango de cuentas cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro para rango de cuentas cuenta Final</param>
        /// <returns>List DTO</returns>
        public List<DTO_ReportLibroMayor> DAL_ReportesContabildiad_LibroMayor(int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/)
        {
            try
            {
                List<DTO_ReportLibroMayor> result = new List<DTO_ReportLibroMayor>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                //string filtroCuentas = string.Empty;

                //if (!string.IsNullOrEmpty(cuentaIni) && !string.IsNullOrEmpty(cuentaFin))
                //    filtroCuentas = " AND balance.CuentaID between @cuentaIni and @cuentaFin ";
                //if (!string.IsNullOrEmpty(cuentaIni) && string.IsNullOrEmpty(cuentaFin))
                //    filtroCuentas = " AND balance.CuentaID = @cuentaIni ";

                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    " SELECT CASE WHEN(cuenta.Naturaleza = 2) THEN 'CR' else '' end CuentaNaturaleza , " +
                            " balance.PeriodoID,balance.CuentaID, cuenta.Descriptivo AS CuentaDesc,   " +
                            " SUM(balance.DbSaldoIniLocML + balance.DbSaldoIniLocME + balance.CrSaldoIniLocML +balance.CrSaldoIniLocME) AS SaldoInicial, " +
                            " SUM(balance.DbOrigenLocML + balance.DbOrigenLocME) AS DebitoLocal, " +
                            " ABS(SUM(balance.CrOrigenLocML + balance.CrOrigenLocME))  AS CreditoLocal, " +
                            " CASE WHEN (cuenta.Naturaleza = 2) THEN SUM((balance.DbSaldoIniLocML + balance.CrSaldoIniLocML) - balance.DbOrigenLocML - balance.CrOrigenLocML) " +
                            " ELSE SUM((balance.DbSaldoIniLocML +  balance.CrSaldoIniLocML )+ balance.DbOrigenLocML +  balance.CrOrigenLocML ) END AS TOTAL," +
                            " balance.BalanceTipoID " +
                    " FROM coBalance balance " +
                            " INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)ON cuenta.CuentaID = balance.CuentaID AND balance.eg_coPlanCuenta = cuenta.EmpresaGrupoID " +
                    " WHERE balance.EmpresaID = @EmpresaID " +
                            " AND DATEPART(YEAR, balance.PeriodoID) = @ano " +
                            " AND DATEPART(MONTH, balance.PeriodoID) = @mes   " +
                            " AND balance.BalanceTipoID = @tipoBalance " +
                    //filtroCuentas +
                    " GROUP BY cuenta.Naturaleza, " +
                                " balance.PeriodoID, " +
                                " balance.CuentaID,  " +
                                " cuenta.Descriptivo, " +
                                " balance.DbOrigenLocML," +
                                " balance.CrOrigenLocML, " +
                                " balance.BalanceTipoID " +
                     " ORDER BY CuentaDesc ";

                #endregion
                #region Parametrsos
                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@tipoBalance", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char, UDT_CuentaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@cuentaFin", SqlDbType.Char, UDT_CuentaID.MaxLength);
                # endregion
                #region Valores Parametros
                mySqlCommandSel.Parameters["@ano"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;
                mySqlCommandSel.Parameters["@tipoBalance"].Value = tipoBalance;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaIni;
                //mySqlCommandSel.Parameters["@cuentaFin"].Value = cuentaFin;
                #endregion

                DTO_ReportLibroMayor LibroMayor = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    LibroMayor = new DTO_ReportLibroMayor(dr);
                    result.Add(LibroMayor);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabildiad_LibroMayor");
                throw exception;
            }
        }

        #endregion

        #endregion

        #region Saldos

        #region Saldo Funcional

        /// <summary>
        ///  Función que Carga el DTO de Saldos
        /// </summary>
        /// <param name="año">Año por el cual se va a filtrar</param>
        /// <param name="mes">Mes el cual se quiere ver</param>
        /// <param name="libro">Tipo de libro que se va a imprimir</param>
        /// <returns>Lis DTO</returns>
        public List<DTO_ReportSaldos> DAL_ReportesCantabilidad_SaldosFuncional(int año, int mesInicial, int mesFin, string libro)
        {
            try
            {
                List<DTO_ReportSaldos> saldos = new List<DTO_ReportSaldos>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT saldo.PeriodoID,saldo.BalanceTipoID, saldo.CuentaID,cuenta.Descriptivo  AS NombreCuenta, saldo.TerceroID," +
                            " tercero.Descriptivo AS NombreTercero, saldo.CentroCostoID, centro.Descriptivo AS CentroDesc, " +
                            " SUM(saldo.DbSaldoIniLocML + saldo.DbSaldoIniLocME + saldo.CrSaldoIniLocML + saldo.CrOrigenLocME) AS SaldoInicialML, " +
                            " saldo.DbOrigenLocML as DebitoML, saldo.CrOrigenLocML AS CreditoML, " +
                            " SUM(saldo.DbOrigenLocML + saldo.CrOrigenLocML ) AS FinalML, " +
                            " SUM(saldo.DbSaldoIniExtML + saldo.DbSaldoIniExtME + saldo.CrSaldoIniExtME + saldo.CrSaldoIniExtML) AS SaldoInicialME, " +
                            " saldo.DbOrigenExtML as DebitoME, saldo.CrOrigenExtML AS CreditoME, " +
                            " SUM(saldo.DbOrigenExtML + saldo.CrOrigenExtML ) AS FinalME, " +
                            " saldo.ProyectoID,proy.Descriptivo AS ProyectoDesc," +
                            " saldo.LineaPresupuestoID, linea.Descriptivo as LineaDesc " +
                    " FROM coCuentaSaldo saldo " +
                        " INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) ON (saldo.CuentaID=cuenta.CuentaID) " +
                        " INNER JOIN coTercero tercero WITH(NOLOCK) ON tercero.TerceroID = saldo.TerceroID " +
                        " INNER JOIN coProyecto proy WITH(NOLOCK) ON proy.ProyectoID = saldo.ProyectoID " +
                        " INNER JOIN plLineaPresupuesto linea WITH(NOLOCK) ON linea.LineaPresupuestoID = saldo.LineaPresupuestoID " +
                        " INNER JOIN coCentroCosto centro WITH(NOLOCK) ON centro.CentroCostoID = saldo.CentroCostoID " +
                    " WHERE saldo.EmpresaID = @EmpresaID " +
                            " saldo.BalanceTipoID = @libro " +
                            " AND DATEPART(MONTH, saldo.PeriodoID) > = @mesInicial " +
                            " AND DATEPART(MONTH, saldo.PeriodoID) < = @mesFin " +
                            " AND DATEPART(YEAR, saldo.PeriodoID) = @ano " +
                    " GROUP BY saldo.PeriodoID,saldo.BalanceTipoID,   " +
                                " saldo.CuentaID,cuenta.Descriptivo,   " +
                                " saldo.TerceroID,tercero.Descriptivo, " +
                                " saldo.DbOrigenLocML,saldo.CrOrigenLocML, " +
                                " saldo.DbOrigenExtML,saldo.CrOrigenExtML, " +
                                " saldo.ProyectoID,proy.Descriptivo, " +
                                " saldo.LineaPresupuestoID, linea.Descriptivo, " +
                                " saldo.CentroCostoID, centro.Descriptivo ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mesInicial", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mesFin", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@ano"].Value = año;
                mySqlCommandSel.Parameters["@mesInicial"].Value = mesInicial;
                mySqlCommandSel.Parameters["@mesFin"].Value = mesFin;
                mySqlCommandSel.Parameters["@libro"].Value = libro;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                #endregion

                DTO_ReportSaldos saldoFuncional = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    saldoFuncional = new DTO_ReportSaldos(dr);
                    saldos.Add(saldoFuncional);
                }
                dr.Close();

                return saldos;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCantabilidad_SaldosFuncional");
                throw exception;
            }

        }

        #endregion

        #endregion

        #region Tasas

        /// <summary>
        /// Funcion que se encarga de traer la informacion para el reporte de Tasas
        /// </summary>
        /// <param name="Periodo">Periodo a Consultar</param>
        /// <param name="isDiaria">Tipo de reporte a imprimir (True: Reportes Tasa Cierre, False: Reprote Tasa Diaria)</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_ReportTasas> DAL_ReportesContabilidad_Tasas(DateTime Periodo, bool isDiaria)
        {
            try
            {
                List<DTO_ReportTasas> result = new List<DTO_ReportTasas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                #endregion
                #region CommandText

                if (!isDiaria)
                {
                    mySqlCommandSel.CommandText =
                        " SELECT CuentaID, CuentaDesc, FinalML,  FinalME,  " +
                            " CASE WHEN(FinalME = 0 )THEN FinalML ELSE FinalML / FinalME END TasaCierre  " +
                        " FROM ( " +
                            " SELECT bal.CuentaID, cta.Descriptivo CuentaDesc , " +
                                " SUM(DbSaldoIniLocML + DbSaldoIniExtML+ CrSaldoIniLocML+ CrSaldoIniExtML+ DbOrigenLocML+ DbOrigenExtML+ CrOrigenLocML+ CrOrigenExtML) FinalML , " +
                                " SUM(DbSaldoIniLocME+ DbSaldoIniExtME+ CrSaldoIniLocME+ CrSaldoIniExtME+ DbOrigenLocME+ DbOrigenExtME+ CrOrigenLocME+ CrOrigenExtME) FinalME " +
                            " FROM coBalance bal " +
                                " inner join coPlanCuenta cta with(nolock) on (bal.CuentaID = cta.CuentaID and bal.eg_coPlanCuenta = cta.EmpresaGrupoID) " +
                            " WHERE bal.EmpresaID = @EmpresaID " +
                                " AND cta.MovInd = 1  " +
                                " AND (cta.OrigenMonetario = 1 or cta.OrigenMonetario = 2)  " +
                                " AND  Year(PeriodoID) = @Year " +
                                " AND Month(PeriodoID) = @Month " +
                        " GROUP BY  bal.CuentaID, Descriptivo " +
                        " ) Consulta " +
                        " ORDER BY CuentaID ";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                        " SELECT Dias,[1] Ene, [2] Febr, [3] Mar, [4] Abril, [5] May, [6] Jun, [7] Jul, [8] Ago, [9] Sep, [10] Oct, [11] Nov, [12] Dic " +
                        " FROM( " +
                             " SELECT month(Fecha) Meses,DAY(Fecha) Dias,TasaCambio  " +
                             " FROM glTasaCambio with(nolock) " +
                            " WHERE  EmpresaGrupoID = @EmpresaID " +
                            " AND YEAR(Fecha) = @Year " +
                        " )  SourceTable  " +
                        " PIVOT ( " +
                        " MAX(TasaCambio) " +
                        " FOR Meses in " +
                        " ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) PivotTable order by Dias ";
                }

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                if (!isDiaria)
                    mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);

                #endregion
                #region Asignacion Valores paramentros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Year"].Value = Periodo.Year;
                if (!isDiaria)
                    mySqlCommandSel.Parameters["@Month"].Value = Periodo.Month;

                #endregion

                DTO_ReportTasas tasasCierre = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    tasasCierre = new DTO_ReportTasas(dr, isDiaria);
                    result.Add(tasasCierre);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_Tasas");
                throw exception;
            }
        }

        #endregion

        #endregion

        #region Excel

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        ///  <param name="chequeNro">cheque Nro</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_Reportes_Co_ContabilidadToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string terceroID, string cuentaID, string centroCtoID, string proyectoID, string lineaPresupID, string balanceTipo, string comprobID,
                string compNro,object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();

                #region Auxiliar
                if (documentoID == AppReports.coAuxiliar)
                {
                    #region Filtros

                    string whereAll = "";
                    string whereAux = "";

                    if (!string.IsNullOrEmpty(cuentaID) || !string.IsNullOrEmpty(terceroID) || !string.IsNullOrEmpty(proyectoID) || !string.IsNullOrEmpty(centroCtoID) ||
                        !string.IsNullOrEmpty(lineaPresupID))
                    {
                        if (!string.IsNullOrEmpty(cuentaID))
                        {
                            if (!string.IsNullOrEmpty(otroFilter.ToString()))
                            {
                                whereAux = " AND aux.CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                                whereAll = " WHERE CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                            }
                            else
                            {
                                whereAux = " AND aux.CuentaID = " + "'" + cuentaID + "'";
                                whereAll = " WHERE CuentaID = " + "'" + cuentaID + "'";
                            }
                        }
                        if (!string.IsNullOrEmpty(terceroID))
                        {
                            whereAux += " AND aux.TerceroID = " + "'" + terceroID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE TerceroID in ('','" + terceroID + "')" :
                                    " AND TerceroID in ('','" + terceroID + "')";
                        }
                        if (!string.IsNullOrEmpty(proyectoID))
                        {
                            whereAux += " AND aux.ProyectoID = " + "'" + proyectoID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE ProyectoID in ('','" + proyectoID + "')" :
                                    " AND ProyectoID in ('','" + proyectoID + "')";
                        }
                        if (!string.IsNullOrEmpty(centroCtoID))
                        {
                            whereAux += " AND aux.CentroCostoID = " + "'" + centroCtoID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE CentroCostoID in ('','" + centroCtoID + "')" :
                                    " AND CentroCostoID in ('','" + centroCtoID + "')";
                        }
                        if (!string.IsNullOrEmpty(lineaPresupID))
                        {
                            whereAux += " AND aux.LineaPresupuestoID = " + "'" + lineaPresupID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE LineaPresupuestoID in ('','" + lineaPresupID + "')" :
                                    " AND LineaPresupuestoID in ('','" + lineaPresupID + "')";
                        }
                    }

                    #endregion
                    #region CommanText

                    mySqlCommandSel.CommandText =
                       " SELECT * FROM " +
                        " ( " +
                            " SELECT  " +
                                "  PeriodoID,'' AS Fecha,'' AS Comprobante,  " +
                                " '' AS DocumentoCOM, '' AS Descripcion, aux.CuentaID, " +
                                " cta.Descriptivo AS CuentaDesc,'' AS TerceroID, '' AS TerceroDesc, " +
                                " '' AS CentroCostoID, '' AS LineaPresupuestoID,'' AS ProyectoID, 0 AS TasaCambioBase,  " +
                                " 0 AS vlrBaseML, 0 AS vlrBaseME, 0 AS DebitoML, 0 AS CreditoML, 0 AS DebitoME, 0 AS CreditoME,	  " +
                                " SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) as InicialML, " +
                                " SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) as InicialME " +
                            " FROM coCuentaSaldo aux with(nolock)  " +
                            "   INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                    " AND aux.PeriodoID = @_peridoIni " +
                                    " AND aux.BalanceTipoID = @Libro " + whereAux +
                            " GROUP BY PeriodoID,aux.CuentaID,cta.Descriptivo,BalanceTipoID " +
                        " UNION ALL " +
                            " SELECT  " +
                                " aux.PeriodoID,aux.Fecha,RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                                " aux.DocumentoCOM, SUBSTRING( aux.Descriptivo,1,37) as Descripcion,  aux.CuentaID, " +
                                " cta.Descriptivo as CuentaDesc,aux.TerceroID,SUBSTRING(tercero.Descriptivo,1,22) as TerceroDesc, " +
                                " aux.CentroCostoID ,aux.LineaPresupuestoID,aux.ProyectoID, aux.TasaCambioBase, " +
                                " aux.vlrBaseML, aux.vlrBaseME,	  " +
                                " CASE WHEN aux.vlrMdaLoc >= 0 THEN aux.vlrMdaLoc ELSE 0 END AS DebitoML, " +
                                " CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc * -1 ELSE 0 END AS CreditoML, " +
                                " CASE WHEN aux.vlrMdaExt >= 0 THEN aux.vlrMdaExt ELSE 0 END AS DebitoME, " +
                                " CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt * -1 ELSE 0 END AS CreditoME, " +
                                " 0 InicialML,    0 InicialME   " +
                            " FROM coAuxiliar aux  WITH(NOLOCK)  " +
                                " INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta  " +
                                " INNER JOIN coComprobante comp WITH(NOLOCK) ON aux.ComprobanteID=comp.ComprobanteID AND aux.eg_coComprobante = comp.EmpresaGrupoID  " +
                                " INNER JOIN coTercero as tercero WITH(NOLOCK) ON tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero   " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                " AND comp.BalanceTipoID = @libro  " +
                                " AND aux.PeriodoID BETWEEN @_peridoIni AND @_peridoFin AND DAY(aux.PeriodoID) != 2 " + whereAux +
                        " ) AS Auxiliar " + whereAll +
                        "ORDER BY CuentaID, Fecha, Comprobante";

                    #endregion
                    #region Parametros

                    mySqlCommandSel.Parameters.Add("@_peridoIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@_peridoFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@CuentaFin", SqlDbType.Char);

                    #endregion
                    #region Valores Parametros

                    mySqlCommandSel.Parameters["@_peridoIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@_peridoFin"].Value = fechaFin;
                    mySqlCommandSel.Parameters["@libro"].Value = balanceTipo;
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaID;
                    mySqlCommandSel.Parameters["@CuentaFin"].Value = otroFilter.ToString();

                    #endregion
                }
                #endregion

                #region Saldos
                if (documentoID == AppReports.coSaldos)
                {
                    #region Filtros

                    string whereAll = "";
                    string whereAux = "";

                    if (!string.IsNullOrEmpty(cuentaID) || !string.IsNullOrEmpty(terceroID) || !string.IsNullOrEmpty(proyectoID) || !string.IsNullOrEmpty(centroCtoID) ||
                        !string.IsNullOrEmpty(lineaPresupID))
                    {
                        if (!string.IsNullOrEmpty(cuentaID))
                        {
                            if (!string.IsNullOrEmpty(otroFilter.ToString()))
                            {
                                whereAux = " AND aux.CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                                whereAll = " WHERE CuentaID BETWEEN @cuentaIni AND @CuentaFin ";
                            }
                            else
                            {
                                whereAux = " AND aux.CuentaID = " + "'" + cuentaID + "'";
                                whereAll = " WHERE CuentaID = " + "'" + cuentaID + "'";
                            }
                        }
                        if (!string.IsNullOrEmpty(terceroID))
                        {
                            whereAux += " AND aux.TerceroID = " + "'" + terceroID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE TerceroID in ('','" + terceroID + "')" :
                                    " AND TerceroID in ('','" + terceroID + "')";
                        }
                        if (!string.IsNullOrEmpty(proyectoID))
                        {
                            whereAux += " AND aux.ProyectoID = " + "'" + proyectoID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE ProyectoID in ('','" + proyectoID + "')" :
                                    " AND ProyectoID in ('','" + proyectoID + "')";
                        }
                        if (!string.IsNullOrEmpty(centroCtoID))
                        {
                            whereAux += " AND aux.CentroCostoID = " + "'" + centroCtoID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE CentroCostoID in ('','" + centroCtoID + "')" :
                                    " AND CentroCostoID in ('','" + centroCtoID + "')";
                        }
                        if (!string.IsNullOrEmpty(lineaPresupID))
                        {
                            whereAux += " AND aux.LineaPresupuestoID = " + "'" + lineaPresupID + "'";
                            whereAll += string.IsNullOrWhiteSpace(whereAll) ? " WHERE LineaPresupuestoID in ('','" + lineaPresupID + "')" :
                                    " AND LineaPresupuestoID in ('','" + lineaPresupID + "')";
                        }
                    }

                    #endregion
                    #region CommanText

                    mySqlCommandSel.CommandText =
                       " SELECT * FROM " +
                        " ( " +
                            " SELECT  " +
                                "  PeriodoID,'' AS Fecha,'' AS Comprobante,  " +
                                " '' AS DocumentoCOM, '' AS Descripcion, aux.CuentaID, " +
                                " cta.Descriptivo AS CuentaDesc,'' AS TerceroID, '' AS nomTercero, " +
                                " '' AS CentroCostoID, '' AS LineaPresupuestoID,'' AS ProyectoID, 0 AS TasaCambioBase,  " +
                                " 0 AS vlrBaseML, 0 AS vlrBaseME, 0 AS DebitoML, 0 AS CreditoML, 0 AS DebitoME, 0 AS CreditoME,	  " +
                                " SUM(DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) as InicialML, " +
                                " SUM(DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME + CrSaldoIniExtME) as InicialME " +
                            " FROM coCuentaSaldo aux with(nolock)  " +
                            "   INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                    " AND aux.PeriodoID = @_peridoIni " +
                                    " AND aux.BalanceTipoID = @Libro " + whereAux +
                            " GROUP BY PeriodoID,aux.CuentaID,cta.Descriptivo,BalanceTipoID " +
                        " UNION ALL " +
                            " SELECT  " +
                                " aux.PeriodoID,aux.Fecha,RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                                " aux.DocumentoCOM, SUBSTRING( aux.Descriptivo,1,37) as Descripcion,  aux.CuentaID, " +
                                " cta.Descriptivo as CuentaDesc,aux.TerceroID,SUBSTRING(tercero.Descriptivo,1,22) as nomTercero, " +
                                " aux.CentroCostoID ,aux.LineaPresupuestoID,aux.ProyectoID, aux.TasaCambioBase, " +
                                " aux.vlrBaseML, aux.vlrBaseME,	  " +
                                " CASE WHEN aux.vlrMdaLoc >= 0 THEN aux.vlrMdaLoc ELSE 0 END AS DebitoML, " +
                                " CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc * -1 ELSE 0 END AS CreditoML, " +
                                " CASE WHEN aux.vlrMdaExt >= 0 THEN aux.vlrMdaExt ELSE 0 END AS DebitoME, " +
                                " CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt * -1 ELSE 0 END AS CreditoME, " +
                                " 0 InicialML,    0 InicialME   " +
                            " FROM coAuxiliar aux  WITH(NOLOCK)  " +
                                " INNER JOIN coPlanCuenta as cta with(nolock) on cta.CuentaID = aux.CuentaID and cta.EmpresaGrupoID = aux.eg_coPlanCuenta  " +
                                " INNER JOIN coComprobante comp WITH(NOLOCK) ON aux.ComprobanteID=comp.ComprobanteID AND aux.eg_coComprobante = comp.EmpresaGrupoID  " +
                                " INNER JOIN coTercero as tercero WITH(NOLOCK) ON tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero   " +
                            " WHERE  aux.EmpresaID = @EmpresaID  " +
                                " AND comp.BalanceTipoID = @libro  " +
                                " AND aux.PeriodoID BETWEEN @_peridoIni AND @_peridoFin AND DAY(aux.PeriodoID) != 2 " + whereAux +
                        " ) AS Auxiliar " + whereAll +
                        "ORDER BY CuentaID, Fecha, Comprobante";

                    #endregion
                    #region Parametros

                    mySqlCommandSel.Parameters.Add("@_peridoIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@_peridoFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@libro", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@CuentaFin", SqlDbType.Char);

                    #endregion
                    #region Valores Parametros

                    mySqlCommandSel.Parameters["@_peridoIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@_peridoFin"].Value = fechaFin;
                    mySqlCommandSel.Parameters["@libro"].Value = balanceTipo;
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaID;
                    mySqlCommandSel.Parameters["@CuentaFin"].Value = otroFilter.ToString();

                    #endregion
                }
                #endregion

                #region Libro Mayor
                if (documentoID == AppReports.coLibroMayor)
                {

                    #region Filtros
                    #endregion
                    #region CommandText
                    mySqlCommandSel.CommandText =
                        " SELECT balance.PeriodoID,balance.BalanceTipoID,balance.CuentaID, cuenta.Descriptivo AS CuentaDesc,   " +
                                " SUM(balance.DbSaldoIniLocML + balance.DbSaldoIniLocME + balance.CrSaldoIniLocML +balance.CrSaldoIniLocME) AS SaldoInicial, " +
                                " SUM(balance.DbOrigenLocML + balance.DbOrigenLocME) AS DebitoML, " +
                                " ABS(SUM(balance.CrOrigenLocML + balance.CrOrigenLocME))  AS CreditoML, " +
                                " CASE WHEN (cuenta.Naturaleza = 2) THEN SUM((balance.DbSaldoIniLocML + balance.CrSaldoIniLocML) - balance.DbOrigenLocML - balance.CrOrigenLocML) " +
                                " ELSE SUM((balance.DbSaldoIniLocML +  balance.CrSaldoIniLocML )+ balance.DbOrigenLocML +  balance.CrOrigenLocML ) END AS Total " +
                        " FROM coBalance balance " +
                                " INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)ON cuenta.CuentaID = balance.CuentaID AND balance.eg_coPlanCuenta = cuenta.EmpresaGrupoID " +
                        " WHERE balance.EmpresaID = @EmpresaID " +
                                " AND DATEPART(YEAR, balance.PeriodoID) = @ano " +
                                " AND DATEPART(MONTH, balance.PeriodoID) = @mes   " +
                                " AND balance.BalanceTipoID = @tipoBalance " +
                        //filtroCuentas +
                        " GROUP BY   balance.PeriodoID, " +
                                    " balance.BalanceTipoID, " +
                                    " balance.CuentaID,  " +
                                    " cuenta.Descriptivo, " +
                                    " balance.DbOrigenLocML, " +
                                    " balance.CrOrigenLocML, " +
                                    " cuenta.Naturaleza "+
                         " ORDER BY CuentaDesc ";

                    #endregion
                    #region Parametrsos
                    mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@tipoBalance", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    //mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    //mySqlCommandSel.Parameters.Add("@cuentaFin", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    # endregion
                    #region Valores Parametros
                    mySqlCommandSel.Parameters["@ano"].Value = fechaIni.Value.Year;
                    mySqlCommandSel.Parameters["@mes"].Value = fechaIni.Value.Month;
                    mySqlCommandSel.Parameters["@tipoBalance"].Value = balanceTipo;
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    //mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaIni;
                    //mySqlCommandSel.Parameters["@cuentaFin"].Value = cuentaFin;
                    #endregion
                }
                #endregion

                #region Libro Diario
                if (documentoID == AppReports.coLibroDiario)
                {
                    #region CommandText
                    mySqlCommandSel.CommandText =
                        " SELECT aux.PeriodoID,aux.CuentaID, plancu.Descriptivo AS CuentaDesc, RTRIM(CAST(aux.ComprobanteID AS CHAR(15))) +' - '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante,compro.Descriptivo AS ComprobanteDesc, " +
                                " SUM(CASE WHEN aux.vlrMdaLoc > 0 THEN aux.vlrMdaLoc  ELSE 0 END) AS DebitoML,   " +
                                " ABS(SUM(CASE WHEN aux.vlrMdaLoc < 0 THEN aux.vlrMdaLoc ELSE 0 END)) AS CreditoML,   " +
                                " SUM(CASE WHEN aux.vlrMdaExt > 0 THEN aux.vlrMdaExt  ELSE 0 END) AS DebitoME,   " +
                                " ABS(SUM(CASE WHEN aux.vlrMdaExt < 0 THEN aux.vlrMdaExt ELSE 0 END)) AS CreditoME   " +   
                         " FROM coAuxiliar aux WITH(NOLOCK)   " +
                                 " INNER JOIN coPlanCuenta plancu WITH(NOLOCK) ON plancu.CuentaID = aux.CuentaID and plancu.EmpresaGrupoID = aux.eg_coPlanCuenta   " +
                                 " INNER JOIN coComprobante compro WITH(NOLOCK) ON  compro.ComprobanteID = aux.ComprobanteID    " +
                                    " and compro.EmpresaGrupoID = aux.eg_coComprobante   " +
                                 " INNER JOIN coTercero  ter WITH(NOLOCK) ON ter.TerceroID = aux.TerceroID and ter.EmpresaGrupoID = aux.eg_coTercero   " +
                          " WHERE aux.EmpresaID = @EmpresaID   " +
                                 " AND DATEPART(YEAR, aux.PeriodoID) = @ano    " +
                                 " AND DATEPART(MONTH, aux.PeriodoID) = @mes   " +
                                 " AND compro.BalanceTipoID = @tipoBalance   " +
                         " GROUP BY aux.PeriodoID,aux.CuentaID, plancu.Descriptivo, aux.ComprobanteID ,compro.Descriptivo, aux.ComprobanteNro " +
                                                        " /* aux.ComprobanteNro, compro.BalanceTipoID, aux.TerceroID, ter.Descriptivo,  " +
                                                          " aux.CentroCostoID, aux.LineaPresupuestoID,aux.ProyectoID, " +
                                                          " aux.TasaCambioBase, aux.vlrBaseML,aux.vlrBaseME,  " +
                                                         " aux.DocumentoCOM, aux.Descriptivo, aux.EmpresaID, aux.Fecha*/ " +
                           " ORDER By CuentaID ";
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@ano", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@tipoBalance", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    #endregion
                    #region Valores Parametros
                    mySqlCommandSel.Parameters["@ano"].Value = fechaIni.Value.Year;
                    mySqlCommandSel.Parameters["@mes"].Value = fechaIni.Value.Month;
                    mySqlCommandSel.Parameters["@tipoBalance"].Value = balanceTipo;
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    #endregion
                }
                #endregion

                #region Inventarios y Balance
                if (documentoID == AppReports.coInventariosBalance)
                {

                    #region Filtros

                    string cuentaFin = otroFilter != null ? otroFilter.ToString() : string.Empty;
                    string filtroCuentasBalance = string.Empty, filtroCuentaCtaSaldo = string.Empty;

                    if (!string.IsNullOrEmpty(cuentaID) && !string.IsNullOrEmpty(cuentaFin))
                    {
                        filtroCuentasBalance = " AND bal.CuentaID between @cuentaIni and @cuentaFin ";
                        filtroCuentaCtaSaldo = " AND CuentaID between @cuentaIni and @cuentaFin ";
                    }

                    if (!string.IsNullOrEmpty(cuentaID) && string.IsNullOrEmpty(cuentaFin))
                    {
                        filtroCuentasBalance = " AND bal.CuentaID = @cuentaIni ";
                        filtroCuentaCtaSaldo = " AND CuentaID = @cuentaIni ";
                    }

                    #endregion
                    #region CommanText
                    mySqlCommandSel.CommandText =

                                    "       SELECT	bal.BalanceTipoID,        " +
                                    "       		bal.CuentaID,       " +
                                    "       		bal.PeriodoID,       " +
                                    "       		TerceroID,        " +
                                    "       		TerceroDesc,       " +
                                    "       		cta.Descriptivo as CuentaDesc,      " +
                                    "       		SUM(bal.DbOrigenLocML + bal.DbOrigenExtML) DebitoML_Cuenta,      " +
                                    "       		SUM(bal.CrOrigenLocML +  bal.CrOrigenExtML) CreditoML_Cuenta,      " +
                                    "       		SUM(bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML + bal.CrSaldoIniExtML) as InicialML_Cuenta,      " +
                                    "       		SUM(bal.DbSaldoIniLocML + bal.DbSaldoIniExtML + bal.CrSaldoIniLocML +  bal.CrSaldoIniExtML + bal.DbOrigenLocML + bal.DbOrigenExtML + bal.CrOrigenLocML + bal.CrOrigenExtML) as FinalML_Cuenta,      " +
                                    "       		DebitoML_Tercero,       " +
                                    "       		(CreditoML_Tercero)*(-1) as CreditoML_Tercero,       " +
                                    "       		InicialML_Tercero,      " +
                                    "       		(InicialML_Tercero + DebitoML_Tercero + (CreditoML_Tercero * -1)) as FinalML_Tercero       " +
                                    "       FROM		coBalance		bal      " +
                                    "       	INNER JOIN	coPlanCuenta	cta WITH(NOLOCK) on (cta.cuentaID = bal.CuentaID AND cta.EmpresaGrupoID = bal.eg_coPlanCuenta)      " +
                                    "       	LEFT JOIN       " +
                                    "       	(	SELECT	EmpresaID,       " +
                                    "       			CuentaID,	      " +
                                    "       			ctaSaldo.PeriodoID,       " +
                                    "       			ctaSaldo.TerceroID,      " +
                                    "       			ter.Descriptivo as TerceroDesc,       " +
                                    "       			BalanceTipoID, '' as CuentaDesc,      " +
                                    "       			SUM((ctaSaldo.DbOrigenLocML + ctaSaldo.DbOrigenExtML)) DebitoML_Tercero,      " +
                                    "       			SUM((ctaSaldo.CrOrigenLocML + ctaSaldo.CrOrigenExtML))*(-1) CreditoML_Tercero,      " +
                                    "       			SUM((ctaSaldo.DbSaldoIniLocML + ctaSaldo.DbSaldoIniExtML + ctaSaldo.CrSaldoIniLocML + ctaSaldo.CrSaldoIniExtML))  as InicialML_Tercero,      " +
                                    "       			SUM((ctaSaldo.DbSaldoIniLocML + ctaSaldo.DbSaldoIniExtML + ctaSaldo.CrSaldoIniLocML + ctaSaldo.CrSaldoIniExtML + DbOrigenLocML + DbOrigenExtML + ctaSaldo.CrOrigenLocML + ctaSaldo.CrOrigenExtML)) as FinalML_Tercero      " +
                                    "       	FROM	coCuentaSaldo ctaSaldo      " +
                                    "       		LEFT JOIN coTercero  ter with(nolock) on (ter.TerceroID = ctaSaldo.TerceroID and ter.EmpresaGrupoID = ctaSaldo.eg_coTercero)      " +
                                    "       	WHERE	EmpresaID = @EmpresaID      " +
                                    "       			AND Month(PeriodoID)  =  @Month       " +
                                    "       			and year (PeriodoID) = @Año      " +
                                    "       			AND BalanceTipoID = @Libro      " +
                                    "       			AND ctaSaldo.CuentaID = (CASE WHEN @CuentaIni != '' THEN @CuentaIni ELSE ctaSaldo.CuentaID END)	      " +
                                    "       			AND ctaSaldo.CuentaID = (CASE WHEN @CuentaFin != '' THEN @CuentaFin ELSE ctaSaldo.CuentaID END)       " +
                                    "       	GROUP BY	CuentaID,       " +
                                    "       				ctaSaldo.TerceroID,      " +
                                    "       				EmpresaID, PeriodoID,       " +
                                    "       				ter.Descriptivo, BalanceTipoID) AS ctaSaldos       " +
                                    "       			ON (ctaSaldos.CuentaID = bal.CuentaID AND ctaSaldos.EmpresaID = bal.EmpresaID AND ctaSaldos.BalanceTipoID = bal.BalanceTipoID AND ctaSaldos.PeriodoID = BAL.PeriodoID)      " +
                                    "       WHERE	bal.EmpresaID = @EmpresaID      " +
                                    "       		AND Month(bal.PeriodoID)	=  @Month    " +
                                    "       		AND YEAR(bal.PeriodoID)		=	@Año     " +
                                    "       		AND bal.BalanceTipoID = @Libro      " +
                                    "       		AND bal.CuentaID = (CASE WHEN @CuentaIni != '' THEN @CuentaIni ELSE bal.CuentaID END)        " +
                                    "       		AND bal.CuentaID = (CASE WHEN @CuentaFin != '' THEN @CuentaFin ELSE bal.CuentaID END)        " +
                                    "       GROUP BY	bal.CuentaID,       " +
                                    "       			bal.PeriodoID,       " +
                                    "       			bal.BalanceTipoID,       " +
                                    "       			cta.Descriptivo,       " +
                                    "       			TerceroID,       " +
                                    "       			TerceroDesc,       " +
                                    "       			DebitoML_Tercero,       " +
                                    "       			CreditoML_Tercero,       " +
                                    "       			InicialML_Tercero,      " +
                                    "       			FinalML_Tercero        " +
                                    "       HAVING		TerceroID IS NULL      " +
                                    "       		OR	(DebitoML_Tercero)	!= 0      " +
                                    "       		OR	(CreditoML_Tercero) != 0      " +
                                    "       		OR	(InicialML_Tercero) != 0      ";
                    #endregion
                    #region Paramentros
                    mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Libro", SqlDbType.Char);
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@cuentaIni", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@cuentaFin", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    #endregion
                    #region Asignacion de valores a paramentros
                    mySqlCommandSel.Parameters["@Month"].Value = fechaFin.Value.Month;
                    mySqlCommandSel.Parameters["@Año"].Value = fechaFin.Value.Year;
                    mySqlCommandSel.Parameters["@Libro"].Value = balanceTipo;
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@cuentaIni"].Value = cuentaID;
                    mySqlCommandSel.Parameters["@cuentaFin"].Value = cuentaFin;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Reportes_Cp_CxPToExcel");
                return null;
            }
        }


        #region Balance

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>
        public DataTable DAL_ReportesContabilidad_BalancePruebas(DateTime Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial, string CuentaFinal,
            string libro, string tipoReport, string Moneda)
        {
            try
            {
                SqlCommand sc = new SqlCommand("Contabilidad_ReportBalance", base.MySqlConnection.CreateCommand().Connection);
                SqlDataAdapter sda = new SqlDataAdapter();

                #region Filtros

                string cuentaIni = !string.IsNullOrEmpty(CuentaInicial) ? CuentaInicial : "1";
                string cuentaFin = !string.IsNullOrEmpty(CuentaFinal) ? CuentaFinal : "999999999999999";

                #endregion
                #region Parametros

                sc.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sc.Parameters.Add("@Year", SqlDbType.Int);
                sc.Parameters.Add("@Month", SqlDbType.Int);
                sc.Parameters.Add("@Libro", SqlDbType.Char, 5);
                sc.Parameters.Add("@CuentaIni", SqlDbType.Char, UDT_CuentaID.MaxLength);
                sc.Parameters.Add("@CuentaFin", SqlDbType.Char, UDT_CuentaID.MaxLength);
                sc.Parameters.Add("@CuentaLength", SqlDbType.Int);
                sc.Parameters.Add("@TipoReporte", SqlDbType.Char, 15);
                sc.Parameters.Add("@Moneda", SqlDbType.Char, 10);
                sc.Parameters.Add("@SaldoInicialInd", SqlDbType.Int, 3);

                #endregion
                #region Asignacion Valores Parametros

                sc.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sc.Parameters["@Year"].Value = Periodo.Year;
                sc.Parameters["@Month"].Value = Periodo.Month;
                sc.Parameters["@Libro"].Value = libro;
                sc.Parameters["@CuentaIni"].Value = cuentaIni;
                sc.Parameters["@CuentaFin"].Value = cuentaFin;
                sc.Parameters["@CuentaLength"].Value = LongitudCuenta;
                sc.Parameters["@TipoReporte"].Value = tipoReport;
                sc.Parameters["@Moneda"].Value = Moneda;
                sc.Parameters["@SaldoInicialInd"].Value = SaldoIncial;

                #endregion

                sc.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = sc;

                DataTable table = new DataTable("Balance");
                sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_BalancePruebas");
                throw exception;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>
        public DataTable DAL_ReportesContabilidad_ReporteBalancePruebasXLS(int año, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
               string CuentaFinal, string libro, string tipoReport, string Moneda, int MesInicial, int MesFinal, byte? Combo1, byte? Combo2)
        {
            #region Filtros

            string cuentaIni = !string.IsNullOrEmpty(CuentaInicial) ? CuentaInicial : "1";
            string cuentaFin = !string.IsNullOrEmpty(CuentaFinal) ? CuentaFinal : "999999999999999";

            #endregion
            #region Query Balance 

            try
            {
                string QueryBalance = string.Empty;
                if (tipoReport.Equals("DePrueba"))
                {
                    #region Query Balance Prueba
		                   QueryBalance =

                                      "declare @PerIni int  " +
                                      "declare @PerFin int  " +
                                      "set  @PerIni			= (case when @MesIni<=12 then @MesIni else 12 end) * 100 + (case when @MesIni<=12 then 1 else 2 end) " +
                                      "set  @PerFin			= (case when @MesFin<=12 then @MesFin else 12 end) * 100 + (case when @MesFin<=12 then 1 else 2 end) " +
                                      "Select	 " +
                                      "    CuentaID,  " +
                                      "    CuentaDesc,  " +
                                      "    sum(InicialML) as InicialML, " +
                                      "    sum(DebitoML) as DebitoML,	 " +
                                      "    sum(CreditoML) as CreditoML, " +
                                      "    sum( (case when Naturaleza =1 then 1 else (-1) end) * (IniML + MovimientoML)) as FinalML " +
                                      "from  " +
                                      "        ( " +
                                      "           SELECT  " +
                                      "                (CASE WHEN LEN(b.CuentaID)>6 THEN '1' ELSE '0' END) AS NEGRITA, " +
                                      "                (case when @Moneda = 'Local'/*1*/ then 'BALANCE - PESOS' else 'BALANCE DOLARES' end) as Titulo, " +
                                      "                b.CuentaID, " +
                                      "                c.Descriptivo CuentaDesc ,  " +
                                      "                c.Naturaleza,  " +
                                      "                @Moneda as Moneda, " +
                                      "                case when  (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd , " +
                                      "                case when  (c.Naturaleza=1) then 1 else (-1) end Signo, " +
                                      "               case when	@Combo1 = 'Proyecto' then b.ProyectoID else " +
                                      "			                (case when @Combo1 = 'Centro Costo' then b.CentroCostoID else  " +
                                      "					                (case when @Combo1 = 'Linea Presupuesto' then b.LineaPresupuestoID else '' end) end ) end Codigo1, " +
                                      "                case when  @Combo1 = 'Proyecto' then proyecto.Descriptivo else " +
                                      "			                (case when @Combo1 = 'Centro Costo' then ccosto.Descriptivo else  " +
                                      "						                (case when @Combo1 = 'Linea Presupuesto' then lp.Descriptivo else '' end) end ) end Codigo1Desc, " +
                                      "                 case when	@Combo2 = 'Proyecto' then b.ProyectoID else " +
                                      "				                (case when @Combo2= 'Centro Costo' then b.CentroCostoID else  " +
                                      "					                (case when @Combo2 = 'Linea Presupuesto' then b.LineaPresupuestoID else '' end) end ) end Codigo2, " +
                                      "                 case when  @Combo2 = 'Proyecto' then proyecto.Descriptivo else " +
                                      "				                (case when @Combo2 = 'Centro Costo' then ccosto.Descriptivo else  " +
                                      "						                (case when @Combo2 = 'Linea Presupuesto' then lp.Descriptivo else '' end) end ) end Codigo2Desc, " +
                                      "	                (b.DbOrigenLocML+b.DbOrigenExtML) DebitoML , " +
                                      "	                (b.CrOrigenLocML+b.CrOrigenExtML)*(-1) CreditoML ,  " +
                                      "	                (b.DbOrigenLocME+b.DbOrigenExtME) DebitoME , " +
                                      "	                (b.CrOrigenLocME+b.CrOrigenExtME)*(-1)CreditoME, " +
                                      "	                ((b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)) MovimientoML, " +
                                      "	                ((b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)) MovimientoME, " +
                                      "	                 case	when month(b.PeriodoID)*100+DAY(b.PeriodoID) = @PerIni  " +
                                      "			                then (case  when c.Naturaleza=1 then 1 else -1 end) *  " +
                                      "						                (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd " +
                                      "			                else 0 end InicialML, " +
                                      "	                case	when month(b.PeriodoID)*100+DAY(b.PeriodoID) = @PerIni  " +
                                      "			                then (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd " +
                                      "			                else 0 end IniMl, " +
                                      "	                 case	when month(b.PeriodoID)*100+DAY(b.PeriodoID) = @PerIni  " +
                                      "			                then (case  when c.Naturaleza=1 then 1 else -1 end) *  " +
                                      "						                (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd " +
                                      "			                else 0 end InicialME, " +
                                      "	                case	when month(b.PeriodoID)*100+DAY(b.PeriodoID) = @PerIni  " +
                                      "			                then (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd " +
                                      "			                else 0 end IniME, " +
                                      "	                 case	when @Moneda = 'Local' /*1*/   " +
                                      "			                then  " +
                                      "	                (case when  " +
                                      "			                (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)!=0  " +
                                      "			                or 	(b.DbOrigenLocML+b.DbOrigenExtML)!=0  " +
                                      "			                or 	(b.CrOrigenLocML+b.CrOrigenExtML)!=0 then 1 else 0 end)  " +
                                      "		                  else " +
                                      "			                (case when @Moneda = 'Foreign'/*2*/ " +
                                      "					                  then	(case when	(b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)!=0  " +
                                      "							                or (b.DbOrigenLocME+b.DbOrigenExtME)!=0  " +
                                      "							                or (b.CrOrigenLocME+b.CrOrigenExtME)!=0 then 1 else 0 end)  " +
                                      "					                  else " +
                                      "							                ( case when (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)!=0 or  " +
                                      "							                (b.DbOrigenLocML+b.DbOrigenExtML)!=0 or " +
                                      "							                (b.CrOrigenLocML+b.CrOrigenExtML)!=0 or " +
                                      "							                (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)!=0 or  " +
                                      "							                (b.DbOrigenLocME+b.DbOrigenExtME)!=0 or " +
                                      "							                (b.CrOrigenLocME+b.CrOrigenExtME)!=0 then 1 else 0 end)  " +
                                      "					                  end) " +
                                      "	                 end as FiltroMoneda " +
                                      "               FROM coBalance b  " +
                                      "	                inner join coPlanCuenta c with(nolock)		  on (b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
                                      "	                inner join coProyecto proyecto with(nolock)   on (b.ProyectoID=proyecto.ProyectoID and b.eg_coProyecto=proyecto.EmpresaGrupoID)  " +
                                      "	                inner join coCentroCosto ccosto with(nolock)  on (b.CentroCostoID=ccosto.CentroCostoID and b.eg_coCentroCosto=ccosto.EmpresaGrupoID )  " +
                                      "	                inner join plLineaPresupuesto lp with(nolock) on (b.LineaPresupuestoID=lp.LineaPresupuestoID and b.eg_plLineaPresupuesto=lp.EmpresaGrupoID) " +
                                      "           WHERE 	b.EmpresaID = @EmpresaID and LEN(b.CuentaID)<=case when c.MascaraCta<=@CuentaLength then c.MascaraCta else @CuentaLength end " +
                                      "	                and month(b.PeriodoID)*100+DAY(b.PeriodoID) >=@PerIni " +
                                      "	                and month(b.PeriodoID)*100+DAY(b.PeriodoID) <=@PerFin " +
                                      "	                and Year(b.PeriodoID) = @Año " +
                                      "	                and ( b.BalanceTipoID = @BalanceTipo )  " +
                                      "	                and b.CuentaID BETWEEN @cuentaIni and @cuentaFin   " +
                                      "   ) temp	 " +
                                      "where FiltroMoneda = 1 " +
                                      "group by CuentaID, CuentaDesc, MaxLengthInd, Signo, Titulo, Moneda,FiltroMoneda, " +
                                      "        Codigo1, Codigo1Desc,Codigo2, Codigo2Desc,NEGRITA " +
                                      "ORDER BY CuentaID "; 
	                #endregion
                }
                else if (tipoReport.Equals("PorM"))
                {
                    #region Query Por Meses
                    QueryBalance =
                       "Select	 " +
                       " CuentaID, 	 " +
                       " CuentaDesc, 	 " +
                       " sum(SaldoIni) as SaldoIni,	 " +
                       " sum(VlrMes01) as VlrMes01,	 " +
                       " sum(VlrMes02) as VlrMes02,	 " +
                       " sum(VlrMes03) as VlrMes03,	 " +
                       " sum(VlrMes04) as VlrMes04,	 " +
                       " sum(VlrMes05) as VlrMes05,	 " +
                       " sum(VlrMes06) as VlrMes06,	 " +
                       " sum(VlrMes07) as VlrMes07,	 " +
                       " sum(VlrMes08) as VlrMes08,	 " +
                       " sum(VlrMes09) as VlrMes09,	 " +
                       " sum(VlrMes10) as VlrMes10,	 " +
                       " sum(VlrMes11) as VlrMes11,	 " +
                       " sum(VlrMes12) as VlrMes12	 " +
                       "from    		 " +
                       " (SELECT	 " +
                       "     (CASE WHEN LEN(b.CuentaID)>6 THEN '1' ELSE '0' END) AS NEGRITA,	 " +
                       "     (case when @Moneda = 'Local' then 'BALANCE - PESOS' else 'BALANCE DOLARES' end) as Titulo,	 " +
                       "     b.CuentaID,	 " +
                       "     c.Descriptivo CuentaDesc , 	 " +
                       "     c.Naturaleza, 	 " +
                       "     @Moneda as Moneda,	 " +
                       "     case when  (c.MascaraCta<=@CuentaLength) then (case when LEN(b.CuentaID)=c.MascaraCta then 1 else 0 end) else (case when LEN(b.CuentaID)=@CuentaLength then 1 else 0 end) end MaxLengthInd ,	 " +
                       "     case when  (c.Naturaleza=1) then 1 else (-1) end Signo,	 " +
                       "     case when	@Combo1 = 'Proyecto' then b.ProyectoID else	 " +
                       "                 (case when @Combo1 = 'Centro Costo' then b.CentroCostoID else 	 " +
                       "                         (case when @Combo1 = 'Linea Presupuesto' then b.LineaPresupuestoID else '' end) end ) end Codigo1,	 " +
                       "      case when  @Combo1 = 'Proyecto' then proyecto.Descriptivo else	 " +
                       "                 (case when @Combo1 = 'Centro Costo' then ccosto.Descriptivo else	 " +
                       "                         (case when @Combo1 = 'Linea Presupuesto' then lp.Descriptivo else '' end) end ) end Codigo1Desc,	 " +
                       "      case when	@Combo2 = 'Proyecto' then b.ProyectoID else	 " +
                       "                 (case when @Combo2= 'Centro Costo' then b.CentroCostoID else 	 " +
                       "                         (case when @Combo2 = 'Linea Presupuesto' then b.LineaPresupuestoID else '' end) end ) end Codigo2,	 " +
                       "      case when  @Combo2 = 'Proyecto' then proyecto.Descriptivo else	 " +
                       "                 (case when @Combo2 = 'Centro Costo' then ccosto.Descriptivo else 	 " +
                       "                         (case when @Combo2 = 'Linea Presupuesto' then lp.Descriptivo else '' end) end ) end Codigo2Desc,	 " +
                       "      b.SaldoIni, b.VlrMes01, b.VlrMes02, b.VlrMes03, b.VlrMes04, b.VlrMes05, b.VlrMes06,	 " +
                       "                  b.VlrMes07, b.VlrMes08, b.VlrMes09, b.VlrMes10, b.VlrMes11, b.VlrMes12		 " +
                       "   from	 " +
                       "     (SELECT b.cuentaID, b.CentroCostoID, b.ProyectoID, b.LineaPresupuestoID, 	 " +
                       "              b.eg_coPlanCuenta, b.eg_coProyecto, b.eg_coCentroCosto, b.eg_plLineaPresupuesto,	 " +
                       "          sum(case when month(b.PeriodoID) = 1	 " +
                       "                 then 	 " +
                       "                     case when @Moneda = 'Local' 	 " +
                       "                     then 	 " +
                       "                         (b.DbSaldoIniLocML+b.DbSaldoIniExtML+b.CrSaldoIniLocML+b.CrSaldoIniExtML)*@SaldoInicialInd	 " +
                       "                     else " +
                       "                         (b.DbSaldoIniLocME+b.DbSaldoIniExtME+b.CrSaldoIniLocME+b.CrSaldoIniExtME)*@SaldoInicialInd	 " +
                       "                     end	 " +
                       "                 else 0 end) SaldoIni,	 " +
                       "          sum(case when month(b.PeriodoID) = 1 " +
                       "                 then 	 " +
                       "                     case when @Moneda = 'Local' 	 " +
                       "                     then 	 " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)	 " +
                       "                     else	 " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)	 " +
                       "                     end	 " +
                       "                 else 0 end) VlrMes01,	 " +
                       "          sum(case when month(b.PeriodoID) = 2	 " +
                       "                 then 	 " +
                       "                     case when @Moneda = 'Local' 	 " +
                       "                     then 	 " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)	 " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)	 " +
                       "                     end	 " +
                       "                 else 0 end) VlrMes02,	 " +
                       "          sum(case when month(b.PeriodoID) = 3	 " +
                       "                 then 	 " +
                       "                     case when @Moneda = 'Local' 	 " +
                       "                     then 	 " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)	 " +
                       "                     else	 " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)	 " +
                       "                     end	 " +
                       "                else 0 end) VlrMes03,	 " +
                       "          sum(case when month(b.PeriodoID) = 4	 " +
                       "                 then 	 " +
                       "                     case when @Moneda = 'Local' 	 " +
                       "                     then 	 " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)	 " +
                       "                     else	 " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)	 " +
                       "                     end	 " +
                       "                 else 0 end) VlrMes04,	 " +
                       "          sum(case when month(b.PeriodoID) = 5	 " +
                       "                 then 	 " +
                       "                     case when @Moneda = 'Local' 	 " +
                       "                     then 	 " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML)	 " +
                       "                     else	 " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME)	 " +
                       "                     end	 " +
                       "                 else 0 end) VlrMes05,	 " +
                       "          sum(case when month(b.PeriodoID) = 6	 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes06, " +
                       "          sum(case when month(b.PeriodoID) = 7 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes07, " +
                       "          sum(case when month(b.PeriodoID) = 8 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes08, " +
                       "          sum(case when month(b.PeriodoID) = 9 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes09, " +
                       "          sum(case when month(b.PeriodoID) = 10 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes10, " +
                       "          sum(case when month(b.PeriodoID) = 11 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes11, " +
                       "          sum(case when month(b.PeriodoID) = 12 " +
                       "                 then  " +
                       "                     case when @Moneda = 'Local'  " +
                       "                     then  " +
                       "                         (b.DbOrigenLocML+b.DbOrigenExtML+b.CrOrigenLocML+b.CrOrigenExtML) " +
                       "                     else " +
                       "                         (b.DbOrigenLocME+b.DbOrigenExtME+b.CrOrigenLocME+b.CrOrigenExtME) " +
                       "                     end " +
                       "                 else 0 end) VlrMes12 " +
                       "     FROM coBalance b  " +
                       "             inner join coPlanCuenta c with(nolock)		  on (b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
                       "     WHERE 	b.EmpresaID = @EmpresaID and LEN(b.CuentaID)<=case when c.MascaraCta<=@CuentaLength then c.MascaraCta else @CuentaLength end " +
                       "                 and Year(b.PeriodoID) = @Año " +
                       "                 and b.CuentaID BETWEEN @cuentaIni and @cuentaFin   " +
                       "                 and ((@BalanceTipo is null) or (b.BalanceTipoID=@BalanceTipo))  " +
                       "     group by b.cuentaID, b.CentroCostoID, b.ProyectoID, b.LineaPresupuestoID,  " +
                       "              b.eg_coPlanCuenta, b.eg_coProyecto, b.eg_coCentroCosto, b.eg_plLineaPresupuesto " +
                       "         ) b " +
                       "         inner join coPlanCuenta c with(nolock)		  on (b.CuentaID=c.CuentaID and b.eg_coPlanCuenta=c.EmpresaGrupoID) " +
                       "         inner join coProyecto proyecto with(nolock)   on (b.ProyectoID=proyecto.ProyectoID and b.eg_coProyecto=proyecto.EmpresaGrupoID)  " +
                       "         inner join coCentroCosto ccosto with(nolock)  on (b.CentroCostoID=ccosto.CentroCostoID and b.eg_coCentroCosto=ccosto.EmpresaGrupoID )  " +
                       "         inner join plLineaPresupuesto lp with(nolock) on (b.LineaPresupuestoID=lp.LineaPresupuestoID and b.eg_plLineaPresupuesto=lp.EmpresaGrupoID) " +
                       " ) temp " +
                    "group by CuentaID, CuentaDesc  " +
                    "ORDER BY CuentaID ";
                    
                    #endregion
                }
                else if (tipoReport.Equals("PorQ"))
                {
                }
                else if (tipoReport.Equals("Comparativo"))
                {
                }
                else if (tipoReport.Equals("General"))
                {
                }

            SqlDataAdapter sda = new SqlDataAdapter(QueryBalance, MySqlConnection.CreateCommand().Connection);

            #endregion
            #region Parametros
            sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
            sda.SelectCommand.Parameters.Add("@MesIni", SqlDbType.Int);
            sda.SelectCommand.Parameters.Add("@MesFin", SqlDbType.Int);          
            sda.SelectCommand.Parameters.Add("@Año", SqlDbType.Int);
            sda.SelectCommand.Parameters.Add("@CuentaLength", SqlDbType.Int);
            sda.SelectCommand.Parameters.Add("@SaldoInicialInd", SqlDbType.Int);
            sda.SelectCommand.Parameters.Add("@BalanceTipo", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
            sda.SelectCommand.Parameters.Add("@cuentaIni", SqlDbType.Char, UDT_CuentaID.MaxLength);
            sda.SelectCommand.Parameters.Add("@cuentaFin", SqlDbType.Char, UDT_CuentaID.MaxLength);
            sda.SelectCommand.Parameters.Add("@Combo1", SqlDbType.Char);
            sda.SelectCommand.Parameters.Add("@Combo2", SqlDbType.Char);
            sda.SelectCommand.Parameters.Add("@Moneda", SqlDbType.Char);
            #endregion
            #region Valor a los Parametros
            sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
            sda.SelectCommand.Parameters["@MesIni"].Value = MesInicial;
            sda.SelectCommand.Parameters["@MesFin"].Value = MesFinal.ToString();
            sda.SelectCommand.Parameters["@Año"].Value = año.ToString();
            sda.SelectCommand.Parameters["@CuentaLength"].Value = LongitudCuenta;
            sda.SelectCommand.Parameters["@SaldoInicialInd"].Value = SaldoIncial;
            sda.SelectCommand.Parameters["@BalanceTipo"].Value = libro;
            sda.SelectCommand.Parameters["@cuentaIni"].Value = cuentaIni;
            sda.SelectCommand.Parameters["@cuentaFin"].Value = cuentaFin;
            sda.SelectCommand.Parameters["@Combo1"].Value = Combo1.ToString();
            sda.SelectCommand.Parameters["@Combo2"].Value = Combo2.ToString();
            sda.SelectCommand.Parameters["@Moneda"].Value = Moneda;
            if (Combo1 == 0)
            {
                sda.SelectCommand.Parameters["@Combo1"].Value = string.Empty;
                sda.SelectCommand.Parameters["@Combo2"].Value = string.Empty; 
            }
            #endregion

            foreach (SqlParameter param in sda.SelectCommand.Parameters)
            {
                if (param.Direction.Equals(ParameterDirection.Input))
                {
                    if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                        param.Value = DBNull.Value;
                }
            }
            DataSet dt = new DataSet();
            sda.Fill(dt, "coBalance");

            return dt.Tables[0];
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_ReporteBalancePruebasXLS");
                throw exception;
            }
        }

        #endregion

        #region Comprobante

        /// <summary>
        /// Funcion q se encarga de traer los datos para generar la plantilla de excel
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="comprobanteID">Filtra un comprobante en especifico</param>
        /// <param name="libro">Libro que se se va a consultar</param>
        /// <param name="comprobanteInicial">Numero de comprobante inicial</param>
        /// <param name="comprobanteFinal">Numero de comprobante Final</param>
        /// <returns>Tabla con listado de resultados</returns>
        public DataTable DAL_ReportesContabilidad_ComprobanteXLS(DateTime Periodo, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            try
            {
                #region Filtros

                string comprobante = !string.IsNullOrEmpty(comprobanteID) ? " AND base.ComprobanteID = @ComprobanteID " : string.Empty;
                string rangoComprobates = !string.IsNullOrEmpty(comprobanteInicial) && !string.IsNullOrEmpty(comprobanteFinal) ?
                    " AND base.ComprobanteNro BETWEEN @ComproInicial AND @ComproFinal " : string.Empty;

                #endregion
                #region CommandText

                string query =
                    " SELECT base.PeriodoID as Periodo,compro.BalanceTipoID,RTRIM (CAST(base.ComprobanteID AS CHAR(15)))+' - '+CAST(base.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
                             " base.Fecha, base.CuentaID, cuenta.Descriptivo AS CuentaDesc, " +
                             " base.TerceroID, SUBSTRING(ter.Descriptivo,1,37) as Nombre, base.CentroCostoID,base.ProyectoID, " +
                             " base.LineaPresupuestoID,base.ConceptoCargoID,base.DocumentoCOM,SUBSTRING( base.Descriptivo,1,43) as Descripcion, base.vlrBaseML, base.vlrBaseME, " +
                             " CASE WHEN base.vlrMdaLoc > 0  THEN base.vlrMdaLoc ELSE 0 END AS DebitoLoc, " +
                             " ABS(CASE WHEN base.vlrMdaLoc < 0  THEN base.vlrMdaLoc ELSE 0  END) AS CreditoLoc,  " +
                             " CASE WHEN base.vlrMdaExt > 0 THEN vlrMdaExt ELSE 0 END AS DebitoExt,  " +
                             " ABS(CASE WHEN base.vlrMdaExt < 0  THEN base.vlrMdaExt ELSE 0  END) AS CreditoExt, " +
                             " seUsuario.UsuarioID " +
                    " FROM coAuxiliar AS base WITH(NOLOCK) " +
                        " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) on ctrl.NumeroDoc = base.NumeroDoc " +
                        " INNER JOIN seUsuario WITH(NOLOCK) on seUsuario.ReplicaID = ctrl.seUsuarioID  " +
                        " INNER JOIN coComprobante compro WITH(NOLOCK) On (compro.ComprobanteID = base.ComprobanteID and compro.EmpresaGrupoID = base.eg_coComprobante) " +
                        " INNER JOIN coTercero ter WITH(NOLOCK) ON (ter.TerceroID=base.TerceroID and ter.EmpresaGrupoID=base.eg_coTercero) " +
                        " INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) ON cuenta.CuentaID=base.CuentaID and cuenta.EmpresaGrupoID=base.eg_coPlanCuenta " +
                        " INNER JOIN glDocumento glDoc WITH(NOLOCK)  ON glDoc.DocumentoID = ctrl.DocumentoID " +
                    " WHERE base.EmpresaID = @EmpresaID " +
                            "  AND DATEPART(YEAR, base.PeriodoID) = @ano  " +
                            "  AND  DATEPART(MONTH, base.PeriodoID) = @mes  " +
                            "  AND ( 1 = 1 ) " + comprobante + rangoComprobates +
                            "  AND compro.BalanceTipoID = @libro ";

                SqlDataAdapter sda = new SqlDataAdapter(query, MySqlConnection.CreateCommand().Connection);

                #endregion
                #region Parametros

                sda.SelectCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sda.SelectCommand.Parameters.Add("@ano", SqlDbType.Int);
                sda.SelectCommand.Parameters.Add("@mes", SqlDbType.Int);
                sda.SelectCommand.Parameters.Add("@libro", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                sda.SelectCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                sda.SelectCommand.Parameters.Add("@ComproInicial", SqlDbType.Char);
                sda.SelectCommand.Parameters.Add("@ComproFinal", SqlDbType.Char);

                #endregion
                #region Asignacion Valores Parametros

                sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                sda.SelectCommand.Parameters["@ano"].Value = Periodo.Year;
                sda.SelectCommand.Parameters["@mes"].Value = Periodo.Month;
                sda.SelectCommand.Parameters["@libro"].Value = libro;
                sda.SelectCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                sda.SelectCommand.Parameters["@ComproInicial"].Value = comprobanteInicial;
                sda.SelectCommand.Parameters["@ComproFinal"].Value = comprobanteFinal;

                #endregion

                DataSet dt = new DataSet();
                sda.Fill(dt, "coComprobantes");

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesContabilidad_ComprobanteXLS");
                throw exception;
            }
        }


        #endregion

        #endregion

    }
}


