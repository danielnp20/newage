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
    /// DAL de DAL_ReportesCuentasXPagar
    /// </summary>
    public class DAL_ReportesCuentasXPagar : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesCuentasXPagar(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Reportes de Anticipos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fecha"></param>
        /// <param name="Moneda"></param>
        /// <param name="Tercero"></param>
        /// <param name="isDetallado"></param>
        /// <returns></returns>
        public List<DTO_ReportAnticiposDetallado> DAL_ReportesCuentasXPagar_Anticipos(DateTime Fecha, int Moneda, string Tercero, bool isDetallado)
        {
            try
            {
                List<DTO_ReportAnticiposDetallado> results = new List<DTO_ReportAnticiposDetallado>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string moneda = "", tercero = "";

                if (!string.IsNullOrEmpty(Tercero))
                    tercero = " AND ctrl.TerceroID=@Tercero ";

                if (Moneda == 1 || Moneda == 2)
                    moneda = " and cuenta.OrigenMonetario= @OrigenMonetario ";
                if (Moneda == 3)
                    moneda = " and (cuenta.OrigenMonetario= @OrigenMonetario OR cuenta.OrigenMonetario= @OrigenMonetario2) ";
                #endregion
                #region Query
                if (isDetallado)
                {
                    mySqlCommandSel.CommandText =
                                                        " SELECT anticipos.* FROM( " +
                                                        " SELECT  ctrl.TerceroID, " +
                                                        "        tercero.Descriptivo as NombreTercero, " +
                                                        "        CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                                                        "             WHEN cuenta.OrigenMonetario=2 THEN 'EXT' END AS MonedaOrigen, " +
                                                        "        anti.AnticipoTipoID,tipo.Descriptivo as AnticipoTipoDesc, " +
                                                        "        ctrl.DocumentoTercero as Documento, " +
                                                        "        ctrl.Observacion AS Concepto, " +
                                                        "        anti.RadicaFecha AS Fecha, " +
                                                        "        anti.Valor " +
                                                        " FROM glDocumentoControl ctrl WITH(NOLOCK)  " +
                                                        "        INNER JOIN cpAnticipo anti WITH(NOLOCK) on  ctrl.NumeroDoc = anti.NumeroDoc " +
                                                        "        INNER JOIN coTercero tercero WITH(NOLOCK) on tercero.TerceroID=ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                                                        "        INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) on cuenta.CuentaID=ctrl.CuentaID and cuenta.EmpresaGrupoID=ctrl.eg_coPlanCuenta " +
                                                        "        INNER JOIN cpAnticipoTipo tipo WITH(NOLOCK) on tipo.AnticipoTipoID=anti.AnticipoTipoID and tipo.EmpresaGrupoID=anti.eg_cpAnticipoTipo " +
                                                        " WHERE anti.EmpresaID=@EmpresaID " +
                                                        "        and ctrl.DocumentoID = @DocumentoID " +
                                                        "        and ctrl.FechaDoc<=@FechaDoc " + moneda + tercero +
                                                        " ) AS anticipos WHERE anticipos.Valor > 0 ";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                                     " SELECT anticipos.* FROM( " +
                                     "               SELECT  ctrl.TerceroID,  " +
                                     "                       tercero.Descriptivo as NombreTercero,tipo.Descriptivo as AnticipoTipoDesc , " +
                                     "                       CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                                     "                            WHEN cuenta.OrigenMonetario=2 THEN 'EXT' END AS MonedaOrigen, " +
                                     "                       SUM(anti.Valor) AS Valor " +
                                     "               FROM glDocumentoControl ctrl WITH(NOLOCK)  " +
                                     "               INNER JOIN cpAnticipo anti WITH(NOLOCK) on  ctrl.NumeroDoc = anti.NumeroDoc " +
                                     "               INNER JOIN coTercero tercero WITH(NOLOCK) on tercero.TerceroID=ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                                     "               INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) on cuenta.CuentaID=ctrl.CuentaID and cuenta.EmpresaGrupoID=ctrl.eg_coPlanCuenta " +
                                     "               INNER JOIN cpAnticipoTipo tipo WITH(NOLOCK) on tipo.AnticipoTipoID=anti.AnticipoTipoID and tipo.EmpresaGrupoID=anti.eg_cpAnticipoTipo " +
                                     "               INNER JOIN cpConceptoCXP concepto WITH(NOLOCK) on concepto.ConceptoCxPID=anti.ConceptoCxPID and concepto.EmpresaGrupoID=anti.eg_cpConceptoCXP " +
                                     "               WHERE anti.EmpresaID=@EmpresaID " +
                                     "                     AND	ctrl.DocumentoID = @DocumentoID " +
                                     "                     and ctrl.FechaDoc <= @FechaDoc " + moneda + tercero +
                                     "               GROUP BY ctrl.TerceroID, " +
                                     "                       tercero.Descriptivo, " +
                                     "                       tipo.Descriptivo, " +
                                     "                       cuenta.OrigenMonetario " +
                                     " ) AS anticipos WHERE anticipos.Valor > 0 ";
                }

                #endregion
                #region Creación de parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaDoc", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                if (Moneda == 3)
                    mySqlCommandSel.Parameters.Add("@OrigenMonetario2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);

                #endregion
                #region Asignación de parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Anticipos;
                mySqlCommandSel.Parameters["@FechaDoc"].Value = Fecha;
                mySqlCommandSel.Parameters["@OrigenMonetario"].Value = Moneda;
                if (Moneda == 3)
                {
                    mySqlCommandSel.Parameters["@OrigenMonetario"].Value = Moneda - 1;
                    mySqlCommandSel.Parameters["@OrigenMonetario2"].Value = Moneda - 2;
                }
                mySqlCommandSel.Parameters["@Tercero"].Value = Tercero;
                #endregion


                DTO_ReportAnticiposDetallado tar = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    if (isDetallado)
                    {
                        tar = new DTO_ReportAnticiposDetallado(dr, 0);
                        results.Add(tar);
                    }
                    else
                    {
                        tar = new DTO_ReportAnticiposDetallado(dr, isDetallado);
                        results.Add(tar);
                    }
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_Anticipos");
                throw exception;
            }

        }

        public List<DTO_ReportAnticiposDetallado> DAL_ReportesCuentasXPagar_DocumentoAnticipo(int NumeroDoc)
        {
            try
            {
                List<DTO_ReportAnticiposDetallado> results = new List<DTO_ReportAnticiposDetallado>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                    mySqlCommandSel.CommandText =
                                            " SELECT FechaDoc as Fecha, ctrl.TerceroID, ter.Descriptivo as NombreTercero, "+
                                            " ctrl.DocumentoTercero as Documento, ctrl.Observacion,ctrl.Valor,anti.AnticipoTipoID,tipoAnt.Descriptivo as AnticipoTipoDesc " +
                                            " FROM cpAnticipo anti WITH(NOLOCK) "+
	                                        "    INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON anti.NumeroDoc = ctrl.NumeroDoc And anti.EmpresaID = ctrl.EmpresaID "+
	                                        "    INNER JOIN coTercero ter WITH(NOLOCK) ON ter.TerceroID  = ctrl.TerceroID AND ter.EmpresaGrupoID = ctrl.eg_coTercero "+
                                            "    INNER JOIN cpAnticipoTipo tipoAnt WITH(NOLOCK) ON tipoAnt.AnticipoTipoID  = anti.AnticipoTipoID AND tipoAnt.EmpresaGrupoID = anti.eg_cpAnticipoTipo " +
                                            " WHERE ctrl.NumeroDoc = @NumeroDoc";

                #endregion
                #region Creación de parametros
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Char, 10);
                

                #endregion
                #region Asignación de parametros
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;
                #endregion


                DTO_ReportAnticiposDetallado tar = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                        tar = new DTO_ReportAnticiposDetallado(dr, 0,true);
                        results.Add(tar);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_Anticipos");
                throw exception;
            }
        }

        public List<DTO_ReportAnticiposViaje> DAL_ReportesCuentasXPagar_DocumentoAnticipoViaje(int NumeroDoc)
        {
            try
            {
                List<DTO_ReportAnticiposViaje> results = new List<DTO_ReportAnticiposViaje>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                        " SELECT RadicaFecha AS Fecha, "+
		                                "         area.Descriptivo as Area, "+
		                                "         TipoViaje, "+
		                                "         anti.Valor, "+
                                        "         tipoAnt.AnticipoTipoID,tipoAnt.Descriptivo as AnticipoTipoDesc, " +
		                                "         ctrl.DocumentoTercero, "+
                                        "         ctrl.TerceroID, " +
                                        "         ctrl.Observacion, " +
		                                "         ter.Descriptivo as NombreTercero, "+
		                                "         DiasAlojamiento, "+
		                                "         ValorAlojamiento, "+
		                                "         DiasAlimentacion, "+
		                                "         ValorAlimentacion, "+
		                                "         DiasTransporte, "+
		                                "         ValorTransporte, "+
		                                "         DiasOtrosGastos, "+
		                                "         ValorOtrosGastos, "+
                                        "         ValorTiquetes " +
                                        " FROM cpAnticipo anti WITH(NOLOCK)  "+
                                        " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc=anti.NumeroDoc "+
                                        " INNER JOIN coTercero ter WITH(NOLOCK) ON ter.TerceroID=ctrl.TerceroID and ter.EmpresaGrupoID=ctrl.eg_coTercero "+
                                        " LEFT JOIN glAreaFuncional area WITH(NOLOCK) ON area.AreaFuncionalID=ctrl.AreaFuncionalID and area.EmpresaGrupoID=ctrl.eg_glAreaFuncional " +
                                        " LEFT JOIN cpAnticipoTipo tipoAnt WITH(NOLOCK) ON tipoAnt.AnticipoTipoID  = anti.AnticipoTipoID AND tipoAnt.EmpresaGrupoID = anti.eg_cpAnticipoTipo " +
                                        " WHERE anti.NumeroDoc=@NumeroDoc";

                #endregion
                #region Creación de parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Char, 10);


                #endregion
                #region Asignación de parametros
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;
                #endregion


                DTO_ReportAnticiposViaje tar = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    tar = new DTO_ReportAnticiposViaje(dr);
                    results.Add(tar);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_Anticipos");
                throw exception;
            }
        }
        #endregion

        #region  Reportes de Facturas

        /// <summary>
        /// Funcion que retorna el detalle de los cheques de acuerdo a los filtros
        /// </summary>
        /// <param name="bancoID">Banco </param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_ReportFacturasXPagar> DAL_ReportesCuentasXPagar_FacturasXPagar(string Tercero, int Moneda, string Cuenta, DateTime fecha, bool isMultimoneda)
        {
            

            try
            {
                List<DTO_ReportFacturasXPagar> results = new List<DTO_ReportFacturasXPagar>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string terceroFil = "";
                string cuenta = "";
                string moneda = "";
                string multi = "";
                if (!string.IsNullOrWhiteSpace(Tercero))
                    terceroFil = "  AND  coTercero.TerceroID  = @Tercero ";
                if (!string.IsNullOrWhiteSpace(Cuenta))
                    cuenta = " AND saldo.CuentaID=@Cuenta ";
                if (Moneda == 1 || Moneda == 2)
                    moneda = " AND cuenta.OrigenMonetario=@Origen ";
                if (Moneda == 3)
                    moneda = " AND (cuenta.OrigenMonetario=@Origen OR cuenta.OrigenMonetario=@Origen2) ";
                if (isMultimoneda)
                    multi = " AND SaldoTotalEXT > 0 ";
                
                #endregion
                #region CommandText

                mySqlCommandSel.CommandText =
                   " SELECT A.* FROM " +
                   "         ( " +
                   "         SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                   "                   cxp.FactEquivalente, saldo.CuentaID,  " +
                   "                   Ctrl.Descripcion as CuentaDesc, " +
                   "                   ctrl.Observacion,  " +
                   "                   Ctrl.DocumentoTercero Factura, " +
                   "                   cxp.FacturaFecha, cxp.VtoFecha, " +
                   "                   RTRIM(ctrl.ComprobanteID) + ' - ' + CAST(ctrl.ComprobanteIDNro AS CHAR(5)) as Comprobante,  " +
                   "                    CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                   "                         WHEN cuenta.OrigenMonetario=2 THEN 'EXT' " +
                   "                         END AS MdaOrigen, " +
                   "                   cxp.Valor AS ValorBruto,Ctrl.Valor AS ValorNeto, " +
                   "                   (Ctrl.Valor - ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML +  CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML +   " +
                   "                   CrSaldoIniExtML)) as VlrAbono , " +
                   "                   ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                   "                   CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal, " +
                    //"                   cxp.Valor AS ValorBrutoEXT , Ctrl.Valor AS ValorNetoEXT, " +
                    //"                   ((cxp.Valor*Ctrl.TasaCambioDOCU) - ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME +  CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +   " +
                   "                     ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE cxp.Valor/cambio.TasaCambio END,2) AS ValorBrutoEXT , ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE Ctrl.Valor/cambio.TasaCambio END,2) AS ValorNetoEXT, " +
                   "                     (ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE Ctrl.Valor/cambio.TasaCambio END,2) - ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME +  CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +  " +
                   "                   CrSaldoIniExtME)) as VlrAbonoEXT, " +
                   "                   ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME +  " +
                   "                   CrSaldoIniLocME +  CrSaldoIniExtME) AS SaldoTotalEXT " +
                   "     FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                   "       INNER JOIN cpCuentaXPagar  cxp WITH(NOLOCK) ON cxp.NumeroDoc = saldo.IdentificadorTR   " +
                   "       INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                   "       INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                   "       INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta  " +
                   "       LEFT JOIN glTasaCambio cambio WITH(NOLOCK) ON cambio.Fecha=@Fecha and cambio.EmpresaGrupoID=@EmpresaID " +
                   "     WHERE   saldo.EmpresaID = @EmpresaID " +
                   "	 AND DATEPART(MONTH, saldo.PeriodoID) =  @Month " +
                   "     AND DATEPART(YEAR, saldo.PeriodoID) =  @Year " +
                   "     AND Ctrl.DocumentoID = @documentId " +
                   moneda + terceroFil + cuenta +
                   "       ) AS  A " +
                   "       WHERE A.SaldoTotal > 0 " + multi;
                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.Date);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Origen", SqlDbType.TinyInt);

                if (Moneda == 3)
                {
                    mySqlCommandSel.Parameters.Add("@Origen2", SqlDbType.TinyInt);
                }
                mySqlCommandSel.Parameters.Add("@Cuenta", SqlDbType.Char, UDT_CuentaID.MaxLength);

                #endregion
                #region Asignacion de Paramtros

                mySqlCommandSel.Parameters["@Fecha"].Value = fecha;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Month"].Value = fecha.Month;
                mySqlCommandSel.Parameters["@Year"].Value = fecha.Year;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@Tercero"].Value = Tercero;
                mySqlCommandSel.Parameters["@Origen"].Value = Moneda;
                if (Moneda == 3)
                {
                    mySqlCommandSel.Parameters["@Origen"].Value = Moneda - 1;
                    mySqlCommandSel.Parameters["@Origen2"].Value = Moneda - 2;
                }

                mySqlCommandSel.Parameters["@Cuenta"].Value = Cuenta;

                #endregion

                DTO_ReportFacturasXPagar doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportFacturasXPagar(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_FacturasXPagar");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae una lista de comprobantes de acuerdo a un periodo (factuas pagadas)
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="Tercero">Tercero</param>
        /// <returns>Lista de comprobantes (facturas pagadas)</returns>
        public List<DTO_ReportFacturasPagadas> DAL_ReportesCuentasXPagar_FacturasPagadas(DateTime fechaIni, DateTime fechaFin, string Tercero)
        {
            try
            {
                List<DTO_ReportFacturasPagadas> results = new List<DTO_ReportFacturasPagadas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Validación del Tercero
                string terceroFil = "";
                if (!string.IsNullOrWhiteSpace(Tercero))
                {
                    terceroFil = " AND ctrlCxP.TerceroID = @Tercero ";
                    mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@Tercero"].Value = Tercero;
                }
                #endregion

                mySqlCommandSel.CommandText =
                 "Select CtrlCxP.DocumentoID, CtrlCxP.NumeroDoc, CtrlCxP.DocumentoTercero as Factura,CtrlCxP.TerceroID AS  TerceroId, tercero.Descriptivo,  " +
                    "CtrlPag.DocumentoTercero as ChequeNro,CtrlPag.FechaDoc as FechaPago, CtrlCxP.ComprobanteID + '-' + CAST(CtrlCxP.ComprobanteIDNro AS CHAR(5)) as Comprobante,  " +
                    "banco.BancoCuentaID as Banco,CtrlCxP.CuentaID as CuentaBanco,CtrlCxP.Valor AS ValorFactura, aux.vlrMdaLoc as ValorPago, ABS(CtrlCxP.Valor - aux.vlrMdaLoc) AS SaldoTotal " +
                "from coAuxiliar aux " +
                    "INNER JOIN glDocumentoControl CtrlPag WITH(NOLOCK) ON CtrlPag.NumeroDoc = aux.NumeroDoc	" +
                    "INNER JOIN glDocumentoControl CtrlCxP WITH(NOLOCK) ON CtrlCxP.NumeroDoc = aux.IdentificadorTR	 " +
                    "INNER JOIN tsBancosDocu docuBanco WITH(NOLOCK) ON docuBanco.NumeroDoc = CtrlPag.NumeroDoc   " +
                    "INNER JOIN tsBancosCuenta banco WITH(NOLOCK) ON banco.BancoCuentaID = docuBanco.BancoCuentaID  and banco.EmpresaGrupoID=docuBanco.eg_tsBancosCuenta  " +
                    "INNER JOIN coTercero tercero WITH(NOLOCK) ON tercero.TerceroID = CtrlCxP.TerceroID and tercero.EmpresaGrupoID=CtrlCxP.eg_coTercero  " +
                "WHERE aux.EmpresaID =  @EmpresaID  AND aux.Fecha BETWEEN @FechaIni AND @FechaFin   and  " +
                    " (CtrlPag.DocumentoID = 31 or CtrlPag.DocumentoID = 36 or CtrlPag.DocumentoID = 90031 or CtrlPag.DocumentoID = 90036) and " +
                    " (CtrlCxP.DocumentoID = 21 or CtrlCxP.DocumentoID = 26 or CtrlCxP.DocumentoID = 90021 or CtrlPag.DocumentoID = 90026 ) and " +
                    " ABS(CtrlCxP.Valor - aux.vlrMdaLoc) = 0 " + terceroFil;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;

                DTO_ReportFacturasPagadas doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportFacturasPagadas(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_FacturasPagadas");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que carga las facturas correspondientes aun periodo y los detalla por edades
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPPorEdades> DAL_ReportesCuentasXPagar_CuentasPorPagarPorEdades(DateTime fechaCorte, string terceroID, string cuentaID, bool isDetallada)
        {
            try
            {
                List<DTO_ReportCxPPorEdades> results = new List<DTO_ReportCxPPorEdades>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Validación del filtro

                string filter = "";
                if (!string.IsNullOrWhiteSpace(terceroID))
                {
                    filter = " AND ctrl.TerceroID = @Tercero ";
                    mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@Tercero"].Value = terceroID;
                }
                if (!string.IsNullOrWhiteSpace(cuentaID))
                {
                    filter += " AND ctrl.CuentaID = @CuentaID ";
                    mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommandSel.Parameters["@CuentaID"].Value = cuentaID;
                }

                #endregion

                #region CommanText
                if (isDetallada)
                {
                    #region Carga consulta para Reporte Detallado

                    mySqlCommandSel.CommandText =
                                            "   SELECT   Detalle.Factura, Detalle.FacturaFecha, Detalle.VtoFecha,  Detalle.CuentaID, TerceroID,  " +
                                            "            Descriptivo, " +
                                            "            Signo * CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END as Valor, " +
                                            "            DifDia,    " +
                                            "            No_Vencidas = Signo * CASE WHEN DifDia <= 0 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end,  " +
                                            "            Treinta = Signo * CASE WHEN DifDia BETWEEN  1 AND 30 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end,  " +
                                            "            Sesenta = Signo * CASE WHEN DifDia BETWEEN  31 AND 60 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end, " +
                                            "            Noventa =Signo *  CASE WHEN DifDia BETWEEN 61 AND 90 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end ,  " +
                                            "            COchenta = Signo * CASE WHEN DifDia BETWEEN 91 AND 180 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end, " +
                                            "            MasCOchenta = Signo * CASE WHEN DifDia > 181 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end    " +
                                            "   FROM     " +
                                            "            ( SELECT ctrl.DocumentoTercero as Factura, cta.FacturaFecha, cta.VtoFecha,  saldo.CuentaID,    " +
                                            "            ctrl.Valor as No_Vencidas,  cuenta.OrigenMonetario,CASE WHEN cuenta.Naturaleza = 1 then 1 ELSE -1 END as Signo,   " +
                                            "            tercero.TerceroID, tercero.Descriptivo ,       " +
                                            "            saldo.DbSaldoIniLocML+saldo.DbOrigenLocML+saldo.CrSaldoIniLocML+saldo.CrOrigenLocML as VlrLocML,   " +
                                            "            saldo.DbSaldoIniExtML+saldo.DbOrigenExtML+saldo.CrSaldoIniExtML+saldo.CrOrigenExtML as VlrExtML,   " +
                                            "            saldo.DbSaldoIniLocME+saldo.DbOrigenLocME+saldo.CrSaldoIniLocME+saldo.CrOrigenLocME as VlrLocME,   " +
                                            "            saldo.DbSaldoIniExtME+saldo.DbOrigenExtME+saldo.CrSaldoIniExtME+saldo.CrOrigenExtME as VlrExtME,   " +
                                            "            ( DATEDIFF (DAY, VtoFecha, DATEADD(MONTH,1,@FechaCorte)-1))  AS DifDia, " +
                                            "            0 as 'Treinta',  0 as 'Sesenta',   " +
                                            "            0 as 'Noventa',   0 as 'CVeinte',     " +
                                            "            0 as 'CCincuenta',   0 as 'COchenta', 0 as 'MasCOchenta'     " +
                                            "            FROM cpCuentaXPagar cta WITH(NOLOCK)      " +
                                            "            INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on ctrl.NumeroDoc = cta.NumeroDoc       " +
                                            "            INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on saldo.IdentificadorTR = ctrl.NumeroDoc      " +
                                            "            INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                                            " 			 INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)  on cuenta.CuentaID = saldo.CuentaID  and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta   " +
                                            "            WHERE     (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2)    " +
                                            "               AND cta.EmpresaID = @EmpresaID  " +
                                            "               AND YEAR(PeriodoID) = @Año " +
                                            "               AND MONTH( saldo.PeriodoID) =  @Mes " +
                                            filter +
                                            "            ) AS Detalle " +
                                            "  Where CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML  END <> 0 "+
                                            " ORDER BY  Detalle.FacturaFecha ";
                    #endregion
                }
                else
                {
                    #region Carga consulta para  Reporte Resumido

                    mySqlCommandSel.CommandText =
                       " SELECT consulta.TerceroID, consulta.Descriptivo, " +
                       "        SUM(consulta.No_Vencidas) AS No_Vencidas, " +
                       "        SUM(consulta.Treinta) AS Treinta, " +
                       "        SUM(consulta.Sesenta)AS Sesenta, " +
                       "        SUM(consulta.Noventa)AS Noventa, " +
                       "        SUM(consulta.COchenta)AS COchenta, " +
                       "        SUM(consulta.MasCOchenta)AS MasCOchenta, " +
                       "        SUM(consulta.No_Vencidas + consulta.Treinta +consulta.Sesenta +consulta.Noventa+ consulta.COchenta + consulta.MasCOchenta) AS Total   " +
                       " FROM   (   " +
                       "         SELECT TerceroID, Descriptivo,   " +
                       "            No_Vencidas = Signo * CASE WHEN DifDia <= 0 then No_Vencidas else 0 end,    " +
                       "            Treinta =   Signo * SUM(CASE WHEN DifDia BETWEEN  1 AND 30 then No_Vencidas else 0 end),  " +
                       "            Sesenta =   Signo * SUM(CASE WHEN DifDia BETWEEN  31 AND 60 then No_Vencidas else 0 end),  " +
                       "            Noventa =   Signo * SUM(CASE WHEN DifDia BETWEEN 61 AND 90 then No_Vencidas else 0 end),  " +
                       "            COchenta =  Signo * SUM(CASE WHEN DifDia BETWEEN 91 AND 180 then No_Vencidas else 0 end),  " +
                       "            MasCOchenta =  Signo * SUM(CASE WHEN DifDia > 181 then No_Vencidas else 0 end)   " +
                       "         FROM  (     " +
                       "              SELECT cta.FacturaFecha, cta.VtoFecha,    " +
                       "                saldo.CuentaID,	tercero.TerceroID, tercero.Descriptivo,    " +
                       "                CASE WHEN(cuenta.OrigenMonetario = 1) THEN (saldo.DbSaldoIniLocML+saldo.DbOrigenLocML+saldo.CrSaldoIniLocML+saldo.CrOrigenLocML)    " +
					   "             	ELSE (saldo.DbSaldoIniExtML+saldo.DbOrigenExtML+saldo.CrSaldoIniExtML+saldo.CrOrigenExtML) END as No_Vencidas,	   " +
                       "                (DATEDIFF (DAY, VtoFecha, DATEADD(MONTH,1,'01/08/2015')-1))  AS DifDia,     " +
					   "				CASE WHEN cuenta.Naturaleza = 1 then 1 ELSE -1 END as Signo,     " +
                       "                0 as Treinta, 0 as Sesenta, 0 as Noventa, 0 as CVeinte, 0 as CCincuenta, 0 as COchenta, 0 as MasCOchenta     " +   
                       "              FROM cpCuentaXPagar cta   WITH(NOLOCK)       " +
					   "				INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on (ctrl.NumeroDoc = cta.NumeroDoc)     " +
					   "				INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on saldo.IdentificadorTR = ctrl.NumeroDoc      " +
					   "				INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)  on cuenta.CuentaID = saldo.CuentaID  and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta           " +
					   "				INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero      " +
                       "              WHERE  (ctrl.DocumentoID = 21 or ctrl.DocumentoID = 26)  AND cta.EmpresaID = @EmpresaID  " +
                       "				  AND YEAR(PeriodoID) = @Año AND MONTH(saldo.PeriodoID) = @Mes " +
                       "                  AND CASE WHEN(cuenta.OrigenMonetario = 1) THEN (saldo.DbSaldoIniLocML+saldo.DbOrigenLocML+saldo.CrSaldoIniLocML+saldo.CrOrigenLocML)   " +
                       "				      ELSE (saldo.DbSaldoIniExtML+saldo.DbOrigenExtML+saldo.CrSaldoIniExtML+saldo.CrOrigenExtML) END <> 0  " +
                                           filter +
                       "               ) AS Detalle    " +
                       "            GROUP BY Descriptivo, Detalle.TerceroID, Detalle.DifDia, Detalle.No_Vencidas, Detalle.Signo    " +
                       "                         )  " +
                       "             as consulta  " +
                       "             GROUP BY consulta.TerceroID, consulta.Descriptivo " +
                       "             ORDER BY consulta.Descriptivo ";
                    #endregion
                }
                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);

                #endregion

                #region Asignacion de valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@documentId2"].Value = AppDocuments.NotaCreditoCxP;
                mySqlCommandSel.Parameters["@Mes"].Value = fechaCorte.Month;
                mySqlCommandSel.Parameters["@Año"].Value = fechaCorte.Year;
                #endregion

                DTO_ReportCxPPorEdades doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportCxPPorEdades(dr, isDetallada);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_PorEdadesDetallado");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el DTO de causacion de factura y factura equivalente
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a causar</param>
        /// <param name="impuestos">Listado de los impuestos a cobrar (se obtiene de glControl)</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre</param>
        /// <param name="facEquivalente">Verifica si el tercero al que se le esta haciendola Factura equivalente es Independiente (True = Genera Fac.Equivalente)</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportCausacionFacturas> DAL_ReportesCuentasXPagar_CausacionFacturas(int numDoc, List<string> impuestos, bool isAprovada)
        {
            try
            {
                List<DTO_ReportCausacionFacturas> results = new List<DTO_ReportCausacionFacturas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros
                string tabla;

                if (isAprovada)
                    tabla = "coAuxiliar";
                else
                    tabla = "coAuxiliarPre";

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                #region Query Original
        //"  SELECT VtoFecha,DatoAdd4,TerceroID,nombreTercero, facturaNro, Descripcion,fechaFac, periodoID, Comprobante, CuentaID, nombreCta, " +
        //                " CentroCostoID,ProyectoID,LineaPresupuestoID,SUM(vlrMdaLoc) AS vlrMdaLoc,  SUM(vlrBaseML)  AS vlrBaseML,  vlrBruto, ImpuestoPorc, iva, reteIva, " +
        //                " reteIva, reteFuente, reteIca, anticipo, cuentaCxP,nomCuentaCxp " +
        //    " FROM " +
        //    " ( " +
        //        " SELECT cxp.VtoFecha, auxpre.DatoAdd4, ctrl.TerceroID, ter.Descriptivo as nombreTercero, ctrl.DocumentoTercero as facturaNro, ctrl.Descripcion as Descripcion, " +
        //            " auxPre.Fecha as fechaFac, auxPre.periodoID,RTRIM (CAST(auxPre.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(auxPre.ComprobanteNro AS CHAR(15)) AS Comprobante, " +
        //            " auxPre.CuentaID, cta.Descriptivo as nombreCta, auxPre.CentroCostoID,auxPre.ProyectoID,auxPre.LineaPresupuestoID,auxPre.vlrMdaLoc, " +
        //            " auxPre.vlrBaseML, cxp.Valor as vlrBruto,   " +
        //            " case when (cta.ImpuestoPorc is null) then 0 else cta.ImpuestoPorc end as ImpuestoPorc, " +
        //            " case when (cta.ImpuestoTipoID = @iva) then auxPre.vlrMdaLoc else 0 end as iva, " +
        //            " case when (cta.ImpuestoTipoID = @reteIva) then auxPre.vlrMdaLoc else 0 end as reteIva, " +
        //            " case when (cta.ImpuestoTipoID = @reteFuente) then auxPre.vlrMdaLoc else 0 end as reteFuente, " +
        //            " case when (cta.ImpuestoTipoID = @reteIca) then auxPre.vlrMdaLoc else 0 end as reteIca,	 " +
        //            " case when (auxpre.DatoAdd4 = 'Anticipo') then auxPre.vlrMdaLoc else 0 end as anticipo, " +
        //            " ctrl.CuentaID as cuentaCxP, ctaCxP.Descriptivo as nomCuentaCxp " +
        //        " FROM " + tabla + " AS auxPre " +
        //            " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) on ctrl.NumeroDoc = auxPre.NumeroDoc " +
        //            " INNER JOIN coTercero as ter WITH(NOLOCK) on  (ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID = ctrl.eg_coTercero) " +
        //            " INNER JOIN coPlanCuenta as cta WITH(NOLOCK) on (cta.CuentaID = auxPre.CuentaID and cta.EmpresaGrupoID = auxPre.eg_coPlanCuenta) " +
        //            " INNER JOIN coPlanCuenta as ctaCxP WITH(NOLOCK) on (ctaCxP.CuentaID = ctrl.CuentaID and ctaCxP.EmpresaGrupoID = ctrl.eg_coPlanCuenta) " +
        //            " INNER JOIN cpCuentaXPagar as cxp  WITH(NOLOCK) on (cxp.NumeroDoc = auxPre.NumeroDoc) " +
        //        " WHERE auxPre.EmpresaID = @EmpresaID " +
        //            " AND auxPre.NumeroDoc = @NumeroDoc " +
        //            " AND (auxPre.DatoAdd4 IS NULL OR auxPre.DatoAdd4 NOT IN ( 'AjEnCambio' ,'AjEnCambioContra' ,'Contrapartida' )) " +
        //    " )	CONSULTA " +
        //    " GROUP BY VtoFecha,DatoAdd4,TerceroID,nombreTercero, facturaNro, Descripcion,fechaFac, periodoID, Comprobante, CuentaID, nombreCta, " +
        //                " CentroCostoID,ProyectoID,LineaPresupuestoID, vlrBruto,  ImpuestoPorc, iva, reteIva, reteIva, reteFuente, " +
        //                " reteIca, anticipo, cuentaCxP,nomCuentaCxp "; 
                #endregion

                #region Script Jefferson 17-07-2015

                "   SELECT              " +
                "   	VtoFecha,				DatoAdd4,         " +
                "   	TerceroID,				nombreTercero,          " +
                "   	TerceroAux,				nombreTerceroAux,          " +
                "   	facturaNro,				Descripcion,         " +
                "   	fechaFac,				periodoID,          " +
                "   	Comprobante,			CuentaID,          " +
                "   	nombreCta,				CentroCostoID,         " +
                "   	ProyectoID,				LineaPresupuestoID,         " +
                "   	DEBITO,					ABS(CREDITO) AS CREDITO,         " +
                "   	vlrBruto,				ImpuestoPorc,          " +
                "   	iva,					reteIva,           " +
                "   	reteIva,				reteFuente,          " +
                "   	reteIca,				anticipo,          vlrBaseML,	vlrMdaLoc,	" +
                "   	cuentaCxP,				nomCuentaCxp           " +
                "   FROM  (  SELECT          " +
                "   				cxp.VtoFecha,							auxpre.DatoAdd4,          " +
                "   				ctrl.TerceroID,							ter.Descriptivo as nombreTercero,          " +
                "   				terAux.TerceroID as TerceroAux,			terAux.Descriptivo as nombreTerceroAux,          " +
                "   				ctrl.DocumentoTercero as facturaNro,	ctrl.Descripcion as Descripcion,           " +
                "   				auxPre.Fecha as fechaFac,				auxPre.periodoID,         " +
                "   				RTRIM (CAST(auxPre.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(auxPre.ComprobanteNro AS CHAR(15)) AS Comprobante,           " +
                "   				auxPre.CuentaID,						cta.Descriptivo as nombreCta,          " +
                "   				auxPre.CentroCostoID,					auxPre.ProyectoID,         " +
                "   				auxPre.LineaPresupuestoID,				auxPre.vlrBaseML,       auxPre.vlrMdaLoc,  " +
                "   				CASE WHEN auxPre.vlrMdaLoc > 0 THEN auxPre.vlrMdaLoc ELSE 0 END AS DEBITO,           " +
                "   				CASE WHEN auxPre.vlrMdaLoc < 0 THEN auxPre.vlrMdaLoc ELSE 0 END AS CREDITO,          " +
                "   				cxp.Valor as vlrBruto,             " +
                "   				case when (cta.ImpuestoPorc is null) then 0 else cta.ImpuestoPorc end as ImpuestoPorc,           " +
                "   				case when (cta.ImpuestoTipoID = @iva) then auxPre.vlrMdaLoc else 0 end as iva,           " +
                "   				case when (cta.ImpuestoTipoID = @reteIva) then auxPre.vlrMdaLoc else 0 end as reteIva,           " +
                "   				case when (cta.ImpuestoTipoID = @reteFuente) then auxPre.vlrMdaLoc else 0 end as reteFuente,           " +
                "   				case when (cta.ImpuestoTipoID = @reteIca) then auxPre.vlrMdaLoc else 0 end as reteIca,	           " +
                "   				case when (auxpre.DatoAdd4 = 'Anticipo') then auxPre.vlrMdaLoc else 0 end as anticipo,           " +
                "   				ctrl.CuentaID as cuentaCxP,				ctaCxP.Descriptivo as nomCuentaCxp           " +
                "   			FROM " + tabla + " AS auxPre           " +
                "   				INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) on ctrl.NumeroDoc = auxPre.NumeroDoc           " +
                "   				INNER JOIN coTercero as ter WITH(NOLOCK) on  (ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID = ctrl.eg_coTercero)           " +
                "   				INNER JOIN coTercero as terAux WITH(NOLOCK) on  (terAux.TerceroID = auxPre.TerceroID and terAux.EmpresaGrupoID = auxPre.eg_coTercero)           " +
                "   				INNER JOIN coPlanCuenta as cta WITH(NOLOCK) on (cta.CuentaID = auxPre.CuentaID and cta.EmpresaGrupoID = auxPre.eg_coPlanCuenta)           " +
                "   				INNER JOIN coPlanCuenta as ctaCxP WITH(NOLOCK) on (ctaCxP.CuentaID = ctrl.CuentaID and ctaCxP.EmpresaGrupoID = ctrl.eg_coPlanCuenta)           " +
                "   				INNER JOIN cpCuentaXPagar as cxp  WITH(NOLOCK) on (cxp.NumeroDoc = auxPre.NumeroDoc)           " +
                "   			WHERE          " +
                "   				auxPre.EmpresaID = @EmpresaID           " +
                "   				AND auxPre.NumeroDoc = @NumeroDoc    )	CONSULTA          ";

                #endregion

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@iva", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@reteIva", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@reteFuente", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@reteIca", SqlDbType.Char);
                #endregion
                #region Asignacion de valores a parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;
                mySqlCommandSel.Parameters["@iva"].Value = impuestos[0];
                mySqlCommandSel.Parameters["@reteIva"].Value = impuestos[1];
                mySqlCommandSel.Parameters["@reteFuente"].Value = impuestos[2];
                mySqlCommandSel.Parameters["@reteIca"].Value = impuestos[3];
                #endregion

                DTO_ReportCausacionFacturas casusacion = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    casusacion = new DTO_ReportCausacionFacturas(dr, false);
                    results.Add(casusacion);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_CausacionFacturas");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarga de llenar el DTO con los datos del cliente a quien se le va  generar la factura equivalente
        /// </summary>
        /// <param name="fecha">Fecha de la factura equivalente</param>
        /// <param name="tercero">Tercero a quien se le genera la Factura Equivalente</param>
        /// <param name="iva">Impuesto de la factura Equivalente</param>
        /// <param name="facturaEquivalente">Verifica si se desea imprimir la factura Equivalente</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_ReportLibroCompras> DAL_ReportesCuentasXPagar_FacturaEquivalente(DateTime fecha, string tercero, string iva, string nitEmpresa, bool facturaEquivalente)
        {
            try
            {
                List<DTO_ReportLibroCompras> results = new List<DTO_ReportLibroCompras>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string terceroID = string.Empty;

                if (!string.IsNullOrEmpty(tercero))
                    terceroID = " AND TerceroID = @TerceroID ";

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                     " SELECT /* aux.DatoAdd4, ctrl.DocumentoID , */ ctrl.FechaDoc,  aux.documentoCOM, aux.TerceroID, ter.Descriptivo as nomTercero,  " +
                            " aux.Descriptivo as Descripcion, aux.CuentaID, cta.Descriptivo as nomCuenta, " +
                            " CASE WHEN (aux.vlrMdaLoc) > 0 THEN aux.vlrMdaLoc ELSE 0 END AS Debito, " +
                            " CASE WHEN (aux.vlrMdaLoc) < 0 THEN (-1) * aux.vlrMdaLoc ELSE 0 END AS Credito, " +
                            " CASE WHEN (cta.ImpuestoTipoID = @iva) THEN aux.vlrMdaLoc ELSE 0 END AS iva,  " +
                            " factEquiv.FacturaEQ,cta.ImpuestoPorc, cxp.Valor, aux.vlrBaseML, " +
                            " RTRIM(CAST(aux.ComprobanteID as CHAR(5))) + ' ' + '-' + ' ' + CAST (aux.ComprobanteNro as VARCHAR(10)) as comprobante " +
                     " FROM coAuxiliar AS aux WITH(NOLOCK) " +
                            " INNER JOIN glDocumentoControl AS  ctrl WITH(NOLOCK) on (ctrl.NumeroDoc = aux.NumeroDoc) " +
                            " INNER JOIN coTercero AS ter WITH(NOLOCK) on (ter.TerceroID = aux.TerceroID and ter.EmpresaGrupoID = aux.eg_coTercero) " +
                            " INNER JOIN coPlanCuenta AS cta WITH(NOLOCK) on (cta.CuentaID = aux.CuentaID and  cta.EmpresaGrupoID = aux.eg_coPlanCuenta) " +
                            " INNER JOIN cpFactEquivalente AS factEquiv WITH(NOLOCK) on (factEquiv.EmpresaID = ctrl.EmpresaID AND factEquiv.NumeroDoc = ctrl.NumeroDoc " +
                                    " AND factEquiv.TerceroID = ter.TerceroID AND factEquiv.DocumentoCOM = aux.DocumentoCOM  AND factEquiv.eg_coTercero = ter.EmpresaGrupoID) " +
                            " INNER JOIN coRegimenFiscal AS regFis  WITH(NOLOCK) on (regFis.ReferenciaID = ter.ReferenciaID and regFis.EmpresaGrupoID = ter.eg_coRegimenFiscal) " +
                            " INNER JOIN cpCuentaXPagar AS cxp with(nolock) on(ctrl.NumeroDoc = cXp.NumeroDoc and ctrl.EmpresaID = cXp.EmpresaID)   " +
                    " WHERE aux.EmpresaID = @EmpresaID  " +
                            " AND MONTH(FechaDoc) = @FechaDocMonth " +
                            " AND YEAR(FechaDoc) = @FechaDocYear " +
                            " AND aux.TerceroID = @TerceroID " +
                            " AND DocumentoID in (@CausarFact, @LegaGastos, @PagoTar) " +
                            " AND regFis.FactEquivalenteInd = 1	 " +
                            " AND (aux.DatoAdd4 IS NULL OR aux.DatoAdd4 NOT IN ( 'AjEnCambio' ,'AjEnCambioContra' ,'Contrapartida' )) " +
                            " AND CTA.CuentaGrupoID <> '001' 	 ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaDocMonth", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaDocYear", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CausarFact", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LegaGastos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PagoTar", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@iva", SqlDbType.Char);

                #endregion
                #region Asignacion de Valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaDocMonth"].Value = fecha.Month;
                mySqlCommandSel.Parameters["@FechaDocYear"].Value = fecha.Year;
                mySqlCommandSel.Parameters["@TerceroID"].Value = tercero;
                mySqlCommandSel.Parameters["@CausarFact"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@LegaGastos"].Value = AppDocuments.LegalizacionGastos;
                mySqlCommandSel.Parameters["@PagoTar"].Value = AppDocuments.PagoTarjetaCredito;
                mySqlCommandSel.Parameters["@iva"].Value = iva;

                #endregion

                DTO_ReportLibroCompras doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportLibroCompras(dr, facturaEquivalente);
                    doc.NitEmpresa.Value = nitEmpresa;
                    results.Add(doc);
                }
                dr.Close();

                return results;

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_FacturaEquivalente");
                throw exception;
            }
        }

        #endregion

        #region Reportes Flujos

        /// <summary>
        /// Funcion que trae las cuentas x Pagar de manera resumida en un flujo semanal
        /// </summary>
        /// <param name="fechaCorte">Fecha limite del flujo</param>
        /// <param name="filtro">Tercero Id</param>
        /// <returns>Lista de Cunetas por pagar </returns>
        public List<DTO_ReportCxPFlujoSemanalResumido> DAL_ReportesCuentasXPagar_FlujoSemanalResumido(DateTime fechaCorte, string filtro)
        {
            List<DTO_ReportCxPFlujoSemanalResumido> results = new List<DTO_ReportCxPFlujoSemanalResumido>();

            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Validación del filtro
                string terceroFil = "";
                if (!string.IsNullOrWhiteSpace(filtro))
                    terceroFil = " AND ctrl.TerceroID = " + "'" + filtro + " '";
                #endregion

                #region CommandText

                mySqlCommandSel.CommandText =
                    " SELECT  TerceroID, Descriptivo,  " +
                    " Semana1 = SUM(CASE WHEN Semana = 1 then No_Vencidas else 00 end), " +
                    " Semana2 = SUM(CASE WHEN Semana = 2 then No_Vencidas else 00 end), " +
                    " Semana3 = SUM(CASE WHEN Semana = 3 then No_Vencidas else 00 end), " +
                    " Semana4 = SUM(CASE WHEN Semana = 4 then No_Vencidas else 00 end), " +
                    " Semana5 = SUM(CASE WHEN Semana = 5 then No_Vencidas else 00 end), " +
                    " Semana " +
                    " FROM      " +
                    " (    " +
                    " SELECT cta.NumeroDoc as Factura, cta.FacturaFecha, cta.VtoFecha,     " +
                    " saldo.CuentaID, ((DbOrigenLocML + DbOrigenExtML +  CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +     " +
                    " CrSaldoIniLocML + CrSaldoIniExtML) * -1)as No_Vencidas, tercero.TerceroID, tercero.Descriptivo ,  " +
                    " (	DATEDIFF (DAY,@FechaCorte,VtoFecha))  " +
                    " AS DifDia,    " +
                    " 0 as 'Semana1',  " +
                    " 0 as 'Semana2',  " +
                    " 0 as 'Semana3',  " +
                    " 0 as 'Semana4'  ," +
                    " 0 as 'Semana5'  ," +
                    " datepart(week, VtoFecha ) " +
                    " - datepart(week, dateadd(dd,-day(VtoFecha )+1,VtoFecha )) " +
                    " +1 as Semana " +
                    " FROM cpCuentaXPagar cta    " +
                    " INNER JOIN glDocumentoControl ctrl on ctrl.NumeroDoc = cta.NumeroDoc    " +
                    " INNER JOIN coCuentaSaldo saldo on saldo.IdentificadorTR = ctrl.NumeroDoc    " +
                    " INNER JOIN coTercero tercero on tercero.TerceroID = ctrl.TerceroID     " +
                    " WHERE   " +
                    " (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2)    " +
                    " AND cta.EmpresaID = @EmpresaID   " +
                    terceroFil +
                    " AND  FacturaFecha = @FechaCorte ) AS Detalle   " +
                    " WHERE No_Vencidas <> 0 " +
                    " GROUP BY Descriptivo, Detalle.TerceroID,   " +
                    " Detalle.Semana1, Detalle.Semana2, Detalle.Semana3, Detalle.Semana4, Detalle.Semana5, semana ";
                #endregion

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId2", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@documentId2"].Value = AppDocuments.NotaCreditoCxP;

                DTO_ReportCxPFlujoSemanalResumido doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportCxPFlujoSemanalResumido(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_FlujoSemanalResumido");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae las cuentas x Pagar de manera resumida en un flujo semanal
        /// </summary>
        /// <param name="fechaCorte">Fecha limite del flujo</param>
        /// <param name="filtro">Tercero Id</param>
        /// <returns>Lista de Cunetas por pagar </returns>
        public List<DTO_ReportCxPFlujoSemanalResumido> DAL_Report_Cp_FlujoSemanalDetallado(DateTime fechaCorte, string filtro)
        {
            List<DTO_ReportCxPFlujoSemanalResumido> results = new List<DTO_ReportCxPFlujoSemanalResumido>();

            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Validación del filtro
                string terceroFil = "";
                if (!string.IsNullOrWhiteSpace(filtro))
                    terceroFil = " AND ctrl.TerceroID = " + "'" + filtro + " '";
                #endregion

                #region CommandText

                mySqlCommandSel.CommandText =
                    " SELECT  TerceroID, Descriptivo,   " +
                    " Semana1 = (CASE WHEN Semana = 1 then No_Vencidas else 00 end),    " +
                    "  Semana2 = (CASE WHEN Semana = 2 then No_Vencidas else 00 end),    " +
                    " Semana3 = (CASE WHEN Semana = 3 then No_Vencidas else 00 end),    " +
                    " Semana4 = (CASE WHEN Semana = 4 then No_Vencidas else 00 end),    " +
                    " Semana5 = (CASE WHEN Semana = 5 then No_Vencidas else 00 end),    " +
                    " Semana    " +
                    " FROM         " +
                    " (       " +
                    " SELECT cta.NumeroDoc as Factura, cta.FacturaFecha, cta.VtoFecha,        " +
                    " saldo.CuentaID, ((DbOrigenLocML + DbOrigenExtML +  CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +        " +
                    " CrSaldoIniLocML + CrSaldoIniExtML) * -1)as No_Vencidas, tercero.TerceroID, tercero.Descriptivo ,     " +
                    " (	DATEDIFF (DAY,@FechaCorte , VtoFecha  ))     " +
                    " AS DifDia,       " +
                    " 0 as 'Semana1',     " +
                    " 0 as 'Semana2',     " +
                    " 0 as 'Semana3',     " +
                    " 0 as 'Semana4'  ,   " +
                    " 0 as 'Semana5'  ,   " +
                    " datepart(week, VtoFecha )    " +
                    " - datepart(week, dateadd(dd,-day(VtoFecha )+1,VtoFecha ))    " +
                    " +1 as Semana    " +
                    " FROM cpCuentaXPagar cta       " +
                    " INNER JOIN glDocumentoControl ctrl on ctrl.NumeroDoc = cta.NumeroDoc       " +
                    " INNER JOIN coCuentaSaldo saldo on saldo.IdentificadorTR = ctrl.NumeroDoc      " +
                    " INNER JOIN coTercero tercero on tercero.TerceroID = ctrl.TerceroID        " +
                    " WHERE      " +
                    " (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2 )     " +
                    " AND cta.EmpresaID = @EmpresaID  " +
                    terceroFil +
                    " AND  FacturaFecha = @FechaCorte ) AS Detalle      " +
                    "  WHERE No_Vencidas <> 0 ";
                #endregion

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId2", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@documentId2"].Value = AppDocuments.NotaCreditoCxP;

                DTO_ReportCxPFlujoSemanalResumido doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportCxPFlujoSemanalResumido(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_FlujoSemanalDetallado");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae las cuentas x Pagar de manera Detallada en un flujo semanal
        /// </summary>
        /// <param name="Fecha">Lista de fechas que contiene los cortes semanales de domingo a domingo, la primera fecha del mes y la ultima</param>
        /// <param name="Tercero">Tercero Id</param>
        /// <param name="Moneda">Origen Moneda</param>
        /// <returns>Lista de Cunetas por pagar </returns>
        public List<DTO_ReportCxPFlujoSemanalDetallado> DAL_ReportesCuentasXPagar_FlujoSemanalDetallado(List<DateTime> Fecha, int Moneda, string Tercero, bool isDetallado)
        {
            try
            {

                List<DTO_ReportCxPFlujoSemanalDetallado> results = new List<DTO_ReportCxPFlujoSemanalDetallado>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string moneda = "", tercero = "";

                if (!string.IsNullOrEmpty(Tercero))
                    tercero = " AND ctrl.TerceroID=@Tercero ";

                if (Moneda == 1 || Moneda == 2)
                    moneda = " and cuenta.OrigenMonetario= @OrigenMonetario ";
                if (Moneda == 3)
                    moneda = " and (cuenta.OrigenMonetario= @OrigenMonetario OR cuenta.OrigenMonetario= @OrigenMonetario2) ";
                #endregion

                #region Query
                if (isDetallado)
                {
                    mySqlCommandSel.CommandText =
                                            " SELECT A.TerceroID,A.Descriptivo AS NombreTerc,A.ValorNeto, " +
                                            "                           CASE WHEN A.Moneda=1 THEN 'LOC' " +
                                            "                            WHEN A.Moneda=2 THEN 'EXT' " +
                                            "                            END AS MdaOrigen, " +
                                            "                            A.Factura, " +
                                            "                            A.Descripcion, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fechaIni AND @fecha1 THEN A.ValorNeto ELSE 0 END AS Semana1, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fecha1+1 AND @fecha2 THEN A.ValorNeto ELSE 0 END AS Semana2, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fecha2+1 AND @fecha3 THEN A.ValorNeto ELSE 0 END AS Semana3, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fecha3+1 AND @fecha4 THEN A.ValorNeto ELSE 0 END AS Semana4, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN  " +
                                            "    CASE WHEN @fecha4=@fechaFin THEN @fecha4 ELSE @fecha4+1 END  " +
                                            "    AND  " +
                                            "    CASE WHEN @fecha5+1=@fechaFin THEN @fechaFin ELSE @fecha5 END  " +
                                            "    THEN A.ValorNeto ELSE 0 END AS Semana5, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN  " +
                                            "    CASE WHEN @fecha5=@fechaFin THEN @fecha5 ELSE @fecha5+1 END " +
                                            "    AND  " +
                                            "    @fechaFin  " +
                                            "    THEN A.ValorNeto ELSE 0 END AS Semana6 " +
                                            " FROM " +
                                            " ( " +
                                            " SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                                            "           ctrl.Valor AS ValorNeto, " +
                                            "           cxp.VtoFecha, " +
                                            "           cuenta.OrigenMonetario AS Moneda, " +
                                            "           ctrl.DocumentoTercero Factura, " +
                                            "           ctrl.Descripcion, " +
                                            "           ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                                            "           CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal    " +
                                            " FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                                            "   INNER JOIN cpCuentaXPagar  cxp WITH(NOLOCK) ON cxp.NumeroDoc = saldo.IdentificadorTR   " +
                                            "   INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.DocumentoID = 21   AND Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                                            "   INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero  " +
                                            "   INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta  " +
                                            " WHERE   saldo.EmpresaID =  @EmpresaID " +
                                            "   AND saldo.PeriodoID BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 " +
                                             tercero + moneda +
                                            "   ) AS  A " +
                                            "   WHERE A.SaldoTotal > 0 " +
                                            "   AND A.VtoFecha BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 ";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                                "SELECT Detallado.TerceroID,Detallado.NombreTerc, " +
                                    "SUM(Detallado.Semana1) AS Semana1, " +
                                    "SUM(Detallado.Semana2) AS Semana2, " +
                                    "SUM(Detallado.Semana3) AS Semana3, " +
                                    "SUM(Detallado.Semana4) AS Semana4, " +
                                    "SUM(Detallado.Semana5) AS Semana5, " +
                                    "SUM(Detallado.Semana6) AS Semana6 " +
                                "FROM ( " +
                                     " SELECT A.TerceroID,A.Descriptivo AS NombreTerc,A.ValorNeto, " +
                                            "                           CASE WHEN A.Moneda=1 THEN 'LOC' " +
                                            "                            WHEN A.Moneda=2 THEN 'EXT' " +
                                            "                            END AS MdaOrigen, " +
                                            "                            A.Factura, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fechaIni AND @fecha1 THEN A.ValorNeto ELSE 0 END AS Semana1, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fecha1+1 AND @fecha2 THEN A.ValorNeto ELSE 0 END AS Semana2, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fecha2+1 AND @fecha3 THEN A.ValorNeto ELSE 0 END AS Semana3, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN @fecha3+1 AND @fecha4 THEN A.ValorNeto ELSE 0 END AS Semana4, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN  " +
                                            "    CASE WHEN @fecha4=@fechaFin THEN @fecha4 ELSE @fecha4+1 END  " +
                                            "    AND  " +
                                            "    CASE WHEN @fecha5+1=@fechaFin THEN @fechaFin ELSE @fecha5 END  " +
                                            "    THEN A.ValorNeto ELSE 0 END AS Semana5, " +
                                            "    CASE WHEN A.VtoFecha BETWEEN  " +
                                            "    CASE WHEN @fecha5=@fechaFin THEN @fecha5 ELSE @fecha5+1 END " +
                                            "    AND  " +
                                            "    @fechaFin  " +
                                            "    THEN A.ValorNeto ELSE 0 END AS Semana6 " +
                                            " FROM " +
                                            " ( " +
                                            " SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                                            "           ctrl.Valor AS ValorNeto, " +
                                            "           cxp.VtoFecha, " +
                                            "           cuenta.OrigenMonetario AS Moneda, " +
                                            "           ctrl.DocumentoTercero Factura, " +
                                            "           ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                                            "           CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal    " +
                                            " FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                                            "   INNER JOIN cpCuentaXPagar  cxp WITH(NOLOCK) ON cxp.NumeroDoc = saldo.IdentificadorTR   " +
                                            "   INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.DocumentoID = 21   AND Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                                            "   INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero  " +
                                            "   INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta  " +
                                            " WHERE   saldo.EmpresaID =  @EmpresaID " +
                                            "   AND saldo.PeriodoID BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 " +
                                             tercero + moneda +
                                            "   ) AS  A " +
                                            "   WHERE A.SaldoTotal > 0 " +
                                            "   AND A.VtoFecha BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 " +
                                    " ) AS Detallado " +
                                    "GROUP BY TerceroID,NombreTerc";
                }

                #endregion

                #region Creación de parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                if (Moneda == 3)
                    mySqlCommandSel.Parameters.Add("@OrigenMonetario2", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);

                for (int i = 1; i < Fecha.Count - 1; i++)
                {
                    mySqlCommandSel.Parameters.Add("@fecha" + i, SqlDbType.DateTime);
                }
                // fecha adicional si hay 5 domingos en el mes
                if (Fecha.Count == 6)
                {
                    mySqlCommandSel.Parameters.Add("@fecha5", SqlDbType.DateTime);
                }
                #endregion

                #region Asignación de parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Anticipos;
                mySqlCommandSel.Parameters["@fechaIni"].Value = Fecha[0];
                mySqlCommandSel.Parameters["@fechaFin"].Value = Fecha[Fecha.Count - 1];

                for (int i = 1; i < Fecha.Count - 1; i++)
                {
                    mySqlCommandSel.Parameters["@fecha" + i].Value = Fecha[i];
                }
                // fecha adicional si hay 5 domingos en el mes
                if (Fecha.Count == 6)
                {
                    mySqlCommandSel.Parameters["@fecha5"].Value = Fecha[Fecha.Count - 1].AddDays(1);
                }
                mySqlCommandSel.Parameters["@OrigenMonetario"].Value = Moneda;
                if (Moneda == 3)
                {
                    mySqlCommandSel.Parameters["@OrigenMonetario"].Value = Moneda - 1;
                    mySqlCommandSel.Parameters["@OrigenMonetario2"].Value = Moneda - 2;
                }
                mySqlCommandSel.Parameters["@Tercero"].Value = Tercero;
                #endregion


                DTO_ReportCxPFlujoSemanalDetallado tar = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    tar = new DTO_ReportCxPFlujoSemanalDetallado(dr, isDetallado);
                    results.Add(tar);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_Anticipos");
                throw exception;
            }

        }
        #endregion

        #region Reportes Libro de Compras

        /// <summary>
        /// Funcion que se encarga de llenar el DTO del libro de compras
        /// </summary>
        /// <param name="impuestos">Listado de impuestos parametrizados en glCxP</param>
        /// <param name="fecha">Fecha q se desea ver las compras</param>
        /// <param name="tercero">Tercero especifico q se desea ver</param>
        /// <param name="facturaEquivalente">Verifica si es factura Equivalente</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportLibroCompras> DAL_ReportesCuentasXPagar_LibroCompras(List<string> impuestos, DateTime fecha, string tercero, bool facturaEquivalente)
        {
            try
            {
                List<DTO_ReportLibroCompras> results = new List<DTO_ReportLibroCompras>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string terceroID = string.Empty;

                if (!string.IsNullOrEmpty(tercero))
                    terceroID = " AND TerceroID = @TerceroID ";

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT *, (vlrBruto + iva + reteIva + reteFuente + reteIca) AS total " +
                    " FROM " +
                    " ( " +
                            " SELECT /*CuentaID, */FechaDoc,  documentoCOM, TerceroID, nomTercero, Descripcion, /* ImpuestoTipoID,*/ " +
                            " SUM(vlrBruto) as vlrBruto,SUM(iva) as iva, SUM(reteIva) as reteIva, SUM(reteFuente) as reteFuente,SUM(reteIca) as reteIca " +
                            "  FROM " +
                            " ( " +
                                    " SELECT/* aux.DatoAdd4, aux.CuentaID, */aux.FechaDoc,  aux.documentoCOM, aux.TerceroID, aux.Tercero AS nomTercero, aux.Descripcion, " +
                                            " vlrMdaLoc as  vlrBruto, " +
                                            " /*CASE WHEN (ctaGrup.TipoCuenta = 1) THEN aux.vlrMdaLoc ELSE 0 END AS vlrBruto,*/ " +
                                            " CASE WHEN (cta.ImpuestoTipoID = @iva) THEN aux.vlrMdaLoc ELSE 0 END AS iva, " +
                                            " CASE	WHEN (cta.ImpuestoTipoID = @ReteIva) THEN aux.vlrMdaLoc ELSE 0 END AS reteIva, " +
                                            " CASE	WHEN (cta.ImpuestoTipoID = @ReteFuente) THEN aux.vlrMdaLoc ELSE 0 END AS reteFuente, " +
                                            " CASE WHEN (cta.ImpuestoTipoID = @ReteIca) THEN aux.vlrMdaLoc ELSE 0 END AS reteIca, " +
                                            " cta.ImpuestoTipoID " +
                                    " FROM ( " +
                                             " SELECT aux.DatoAdd4, ctrl.DocumentoID , ctrl.FechaDoc,  aux.documentoCOM, aux.TerceroID, ter.Descriptivo as Tercero, " +
                                                    " aux.Descriptivo as Descripcion, aux.CuentaID, " +
                                                    " aux.EmpresaID,aux.eg_coPlanCuenta, aux.vlrMdaLoc " +
                                             " FROM coAuxiliar AS aux WITH(NOLOCK) " +
                                                    " INNER JOIN glDocumentoControl AS  ctrl WITH(NOLOCK) on (ctrl.NumeroDoc = aux.NumeroDoc) " +
                                                    " INNER JOIN coTercero AS ter WITH(NOLOCK) on (ter.TerceroID = aux.TerceroID and ter.EmpresaGrupoID = aux.eg_coTercero) " +
                                    " ) AS aux	 " +
                                            " INNER JOIN coPlanCuenta AS cta WITH(NOLOCK) on (cta.CuentaID = aux.CuentaID and  cta.EmpresaGrupoID = aux.eg_coPlanCuenta) " +
                                            " INNER JOIN coCuentaGrupo AS ctaGrup  WITH(NOLOCK) on (ctaGrup.CuentaGrupoID = cta. CuentaGrupoID  " +
                                                " and ctaGrup.EmpresaGrupoID = cta.eg_coCuentaGrupo) " +
                                    " WHERE aux.EmpresaID = @EmpresaID  " +
                                             " AND aux.DocumentoID in (@CausarFact, @LegaGastos)	 " +
                                             " AND ImpuestoTipoID in (@iva, @ReteIva, @ReteFuente, @ReteIca) " +
                                             " or ctaGrup.TipoCuenta = 1 " +
                        " )AS consulta  " +
                            " GROUP BY  /*consulta.DatoAdd4, CuentaID, */FechaDoc,  documentoCOM, TerceroID, nomTercero, Descripcion /*, ImpuestoTipoID*/ " +
                    " ) FINAL " +
                    " WHERE MONTH(FechaDoc) = @FechaDocMonth " +
                                    " AND YEAR(FechaDoc) = @FechaDocYear " +
                                    terceroID +
                                    " /*AND TerceroID = @TerceroID*/ " +
                    " ORDER BY /*CuentaID, */  TerceroID, nomTercero,  DocumentoCOM ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CausarFact", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@LegaGastos", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@iva", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ReteIva", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ReteFuente", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@ReteIca", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@FechaDocMonth", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaDocYear", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                #endregion
                #region Asignacion de valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CausarFact"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@LegaGastos"].Value = AppDocuments.LegalizacionGastos;
                mySqlCommandSel.Parameters["@iva"].Value = impuestos[0];
                mySqlCommandSel.Parameters["@ReteIva"].Value = impuestos[1];
                mySqlCommandSel.Parameters["@ReteFuente"].Value = impuestos[2];
                mySqlCommandSel.Parameters["@ReteIca"].Value = impuestos[3];
                mySqlCommandSel.Parameters["@FechaDocMonth"].Value = fecha.Month;
                mySqlCommandSel.Parameters["@FechaDocYear"].Value = fecha.Year;
                mySqlCommandSel.Parameters["@TerceroID"].Value = tercero;

                #endregion

                DTO_ReportLibroCompras doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportLibroCompras(dr, facturaEquivalente);
                    results.Add(doc);
                }
                dr.Close();

                return results;

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_LibroCompras");
                throw exception;
            }
        }

        #endregion

        #region Reporte Radicaciones

        /// <summary>
        /// Funcion q retorna una lista con radicaciones
        /// </summary>
        /// <returns>Radicaciones</returns>
        public List<DTO_ReportRadicaciones> DAL_ReportesCuentasXPagar_Radicaciones(int yearIni, int yearFin, DateTime fechaIni, DateTime fechaFin, string Tercero, string Estado, string Orden)
        {
            try
            {

                List<DTO_ReportRadicaciones> results = new List<DTO_ReportRadicaciones>();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Configuracion Filtros

                string filterTercero = "", estado = "", orden = "";

                if (!string.IsNullOrWhiteSpace(Tercero))                   
                {
                    filterTercero = " AND ctrl.TerceroId = @TerceroID";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = Tercero;
                }

                switch (Estado)
                {
                    case ("Causados"):
                        estado = " AND ctrl.Estado in (3,8) ";
                        break;
                    case ("PorCausar"):
                        estado = " AND ctrl.Estado in (1,2,6) ";
                        break;
                    case ("Devuelto"):
                        estado = " AND ctrl.Estado in (5) ";
                        break;
                    case ("Todos"):
                        estado = "";
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrWhiteSpace(Orden) || !Orden.Equals(""))
                {
                    if (!Orden.Equals("Consecutivo"))
                        orden = " ORDER BY NombreTer ";
                }

                #endregion

                #region CommanText

                mySqlCommandSel.CommandText =
                             " SELECT RANK() over (order by RadicaFecha,DocumentoTercero) AS Item, RadicaFecha, DocumentoTercero, ctrl.TerceroID, " +
                             " ter.Descriptivo as NombreTer, FacturaFecha, ctrl.Valor, " +
                             " CASE WHEN ctrl.Iva IS NULL  THEN 0 ELSE ctrl.Iva END AS Iva, " +
                             " ctrl.Valor + CASE WHEN ctrl.Iva IS NULL  THEN 0 ELSE ctrl.Iva END as Total, " +
                             " CASE   WHEN(ctrl.Estado = 0) THEN 'ANULADO'  " +
                                    " WHEN(ctrl.Estado = 1 OR ctrl.Estado = 2) THEN 'POR CAUSAR'  " +
                                    " WHEN (ctrl.Estado = 3 OR ctrl.Estado = 8) THEN 'CAUSADO'  " +
                                    " WHEN (ctrl.Estado = 4) THEN 'REVERTIDO' " +
                                    " WHEN (ctrl.Estado = 5) THEN 'DEVUELTO' " +
                                    " WHEN (ctrl.Estado = 6) THEN 'RADICADO' " +
                                    " WHEN (ctrl.Estado = 7) THEN 'REVISADO' " +
                                    " WHEN (ctrl.Estado = -1) THEN 'CANCELADO' " +
                                    " END AS Estado " +
                             " FROM cpCuentaXPagar with(nolock) " +
                             " INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cpCuentaXPagar.NumeroDoc  " +
                             " INNER JOIN coTercero ter with(nolock) ON ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID=ctrl.eg_coTercero " +
                             " WHERE cpCuentaXPagar.EmpresaID = @EmpresaID  " +
                             " AND DATEPART(YEAR, cpCuentaXPagar.RadicaFecha)>= @yearIni " +
                             " AND DATEPART(YEAR, cpCuentaXPagar.RadicaFecha)<= @yearFin " +
                             " AND DATEPART(MONTH, cpCuentaXPagar.RadicaFecha) >= @FechaIni " +
                             " AND DATEPART(MONTH, cpCuentaXPagar.RadicaFecha) <= @FechaFin " +
                            filterTercero + estado + orden;

                #endregion

                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@yearIni", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@yearFin", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.Int);
                #endregion

                #region Asignacion valores a Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@yearIni"].Value = yearIni;
                mySqlCommandSel.Parameters["@yearFin"].Value = yearFin;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni.Month;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin.Month;
                #endregion

                DTO_ReportRadicaciones doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportRadicaciones(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_Radicaciones");
                throw exception;
            }
        }

        #endregion

        #region Reportes de Tarjetas

        /// <summary>
        /// Funcion que se encarga de traer los datos de la tarjeta credito
        /// </summary>
        /// <param name="NumDocu">Identificador de los datos de la Tarjeta Credtio</param>
        /// <returns>Listado de datos con la Tarjeta Credito</returns>
        public List<DTO_ReportTarjetaPago> DAL_ReportesCuentasXPagar_TarjetasPago(int NumDocu)
        {
            try
            {
                List<DTO_ReportTarjetaPago> results = new List<DTO_ReportTarjetaPago>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                "SELECT RANK() OVER( order by tarp.CargoEspecialID) as item,tard.NumeroDoc, tard.TarjetaCreditoID AS NumTarjeta, tard.PeriodoPago AS PeriodoPago, " +
                "ter.Descriptivo AS NombreTercero, " +
                "tarp.CargoEspecialID AS CargoID,care.Descriptivo,tarp.Valor AS ValorCargoEsp " +
                "FROM cpTarjetaDocu AS tard WITH (NOLOCK) " +
                "INNER JOIN cpTarjetaPagos AS tarp WITH (NOLOCK) ON tarp.NumeroDoc = tard.NumeroDoc " +
                "INNER JOIN cpCargoEspecial AS care WITH (NOLOCK) ON care.CargoEspecialID = tarp.CargoEspecialID and care.EmpresaGrupoID = tarp.eg_cpCargoEspecial " +
                "INNER JOIN glDocumentoControl AS ctrl WITH (NOLOCK) ON ctrl.NumeroDoc = tard.NumeroDoc " +
                "INNER JOIN coTercero AS ter WITH(NOLOCK) ON ter.TerceroID = ctrl.TerceroID  and ter.EmpresaGrupoID = ctrl.eg_coTercero " +
                "WHERE tard.NumeroDoc= @numDocu and tard.EmpresaID = @EmpresaID";

                mySqlCommandSel.Parameters.Add("@numDocu", SqlDbType.TinyInt, 10);
                mySqlCommandSel.Parameters["@numDocu"].Value = NumDocu;
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, 10);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                DTO_ReportTarjetaPago tar = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    tar = new DTO_ReportTarjetaPago(dr);
                    results.Add(tar);
                }

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_TarjetasPago");
                throw exception;
            }

        }

        public List<DTO_ReportLegalizacionTarjetas> DAL_ReportesCuentasXPagar_LegalizaTarjetas(int NumDocu)
        {
            try
            {
                List<DTO_ReportLegalizacionTarjetas> results = new List<DTO_ReportLegalizacionTarjetas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region  CommanText
                mySqlCommandSel.CommandText =
                    "SELECT legdocu.TarjetaCreditoID AS TarjetaCredito,legdocu.FechaIni,legdocu.FechaFin,legdocu.FechaSolicita,legdeta.TerceroID, ter.Descriptivo AS NombreTer" +
                    ",cares.Descriptivo AS CargoEspDesc, cenco.Descriptivo AS CentroCostoDesc, pro.Descriptivo AS ProyectoDesc, legdeta.ValorBruto,legdeta.ValorRteICA,legdeta.ValorRteFuente,legdeta.ValorIVA1 " +
                    "FROM cpLegalizaDocu AS legdocu WITH(NOLOCK) " +
                    "INNER JOIN cpLegalizaDeta AS legdeta WITH(NOLOCK) on legdeta.NumeroDoc=legdocu.NumeroDoc " +
                    "INNER JOIN coTercero AS ter WITH(NOLOCK) on ter.TerceroID=legdeta.TerceroID and ter.EmpresaGrupoID=legdeta.eg_coTercero " +
                    "INNER JOIN cpCargoEspecial AS cares WITH(NOLOCK) on cares.CargoEspecialID=legdeta.CargoEspecialID and cares.EmpresaGrupoID=legdeta.eg_cpCargoEspecial " +
                    "INNER JOIN coCentroCosto AS cenco WITH(NOLOCK) on cenco.CentroCostoID=legdeta.CentroCostoID and cenco.EmpresaGrupoID=legdeta.eg_coCentroCosto " +
                    "INNER JOIN coProyecto AS pro WITH(NOLOCK) on pro.ProyectoID=legdeta.ProyectoID and pro.EmpresaGrupoID=legdeta.eg_coProyecto " +
                    "INNER JOIN glDocumentoControl AS ctrl WITH (NOLOCK) ON ctrl.NumeroDoc = legdocu.NumeroDoc " +
                    "WHERE legdocu.NumeroDoc=@numDocu and legdocu.EmpresaID=@EmpresaID";
                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@numDocu", SqlDbType.TinyInt, 10);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, 10);

                #endregion
                #region Valores Parematros

                mySqlCommandSel.Parameters["@numDocu"].Value = NumDocu;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion

                DTO_ReportLegalizacionTarjetas tar = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                #region Items
                int Item = 1;
                #endregion

                while (dr.Read())
                {
                    tar = new DTO_ReportLegalizacionTarjetas(dr);
                    tar.Item.Value = Item;
                    results.Add(tar);
                    Item++;
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasXPagar_LegalizaTarjetas");
                throw exception;
            }

        }

        #endregion

        #region Excel 

        /// <summary>
        /// Obtiene un datatable con la info de CxP segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_Reportes_Cp_CxPToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string facturaNro,
                string cuentaID, string bancoCuentaID, string moneda, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();
                string where = string.Empty;

                #region CxP Edades
                if (documentoID == AppReports.cpPorEdades)
                { 
                    #region Filtro
                    if (!string.IsNullOrWhiteSpace(tercero))
                    {
                        where = " AND ctrl.TerceroID = @Tercero ";
                        mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@Tercero"].Value = tercero;
                    }
                    if (!string.IsNullOrWhiteSpace(cuentaID))
                    {
                        where = " AND ctrl.CuentaID = @CuentaID ";
                        mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                        mySqlCommandSel.Parameters["@CuentaID"].Value = cuentaID;
                    }

                    #endregion
                    #region CommanText
                    if (tipoReporte == 1) //Detallado
                    {
                        #region Carga consulta para Reporte Detallado

                        mySqlCommandSel.CommandText =
                                                "   SELECT   Detalle.Factura, Detalle.FacturaFecha, Detalle.VtoFecha,  Detalle.CuentaID, TerceroID,  " +
                                                "            Descriptivo, " +
                                                "            Signo * CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END as Valor, " +
                                                "            DifDia,    " +
                                                "            No_Vencidas = Signo * CASE WHEN DifDia <= 0 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end,  " +
                                                "            Treinta = Signo * CASE WHEN DifDia BETWEEN  1 AND 30 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end,  " +
                                                "            Sesenta = Signo * CASE WHEN DifDia BETWEEN  31 AND 60 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end, " +
                                                "            Noventa =Signo *  CASE WHEN DifDia BETWEEN 61 AND 90 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end ,  " +
                                                "            COchenta = Signo * CASE WHEN DifDia BETWEEN 91 AND 180 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end, " +
                                                "            MasCOchenta = Signo * CASE WHEN DifDia > 181 then (CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML END) else 0 end    " +
                                                "   FROM     " +
                                                "            ( SELECT ctrl.DocumentoTercero as Factura, cta.FacturaFecha, cta.VtoFecha,  saldo.CuentaID,    " +
                                                "            ctrl.Valor as No_Vencidas,  cuenta.OrigenMonetario,CASE WHEN cuenta.Naturaleza = 1 then 1 ELSE -1 END as Signo,   " +
                                                "            tercero.TerceroID, tercero.Descriptivo ,       " +
                                                "            saldo.DbSaldoIniLocML+saldo.DbOrigenLocML+saldo.CrSaldoIniLocML+saldo.CrOrigenLocML as VlrLocML,   " +
                                                "            saldo.DbSaldoIniExtML+saldo.DbOrigenExtML+saldo.CrSaldoIniExtML+saldo.CrOrigenExtML as VlrExtML,   " +
                                                "            saldo.DbSaldoIniLocME+saldo.DbOrigenLocME+saldo.CrSaldoIniLocME+saldo.CrOrigenLocME as VlrLocME,   " +
                                                "            saldo.DbSaldoIniExtME+saldo.DbOrigenExtME+saldo.CrSaldoIniExtME+saldo.CrOrigenExtME as VlrExtME,   " +
                                                "            ( DATEDIFF (DAY, VtoFecha, DATEADD(MONTH,1,@FechaCorte)-1))  AS DifDia, " +
                                                "            0 as 'Treinta',  0 as 'Sesenta',   " +
                                                "            0 as 'Noventa',   0 as 'CVeinte',     " +
                                                "            0 as 'CCincuenta',   0 as 'COchenta', 0 as 'MasCOchenta'     " +
                                                "            FROM cpCuentaXPagar cta WITH(NOLOCK)      " +
                                                "            INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on ctrl.NumeroDoc = cta.NumeroDoc       " +
                                                "            INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on saldo.IdentificadorTR = ctrl.NumeroDoc      " +
                                                "            INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                                                " 			 INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)  on cuenta.CuentaID = saldo.CuentaID  and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta   " +
                                                "            WHERE     (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2)    " +
                                                "               AND cta.EmpresaID = @EmpresaID  " +
                                                "               AND YEAR(PeriodoID) = @Año " +
                                                "               AND MONTH( saldo.PeriodoID) =  @Mes " +
                                                where +
                                                "            ) AS Detalle " +
                                                "  Where CASE WHEN(OrigenMonetario = 1) THEN VlrLocML ELSE VlrExtML  END <> 0 " +
                                                " ORDER BY Descriptivo";
                        #endregion
                    }
                    else if(tipoReporte == 2) //Resumido
                    {
                        #region Carga consulta para  Reporte Resumido

                        mySqlCommandSel.CommandText =
                               " SELECT consulta.TerceroID, consulta.Descriptivo, " +
                               "        SUM(consulta.No_Vencidas) AS No_Vencidas, " +
                               "        SUM(consulta.Treinta) AS Treinta, " +
                               "        SUM(consulta.Sesenta)AS Sesenta, " +
                               "        SUM(consulta.Noventa)AS Noventa, " +
                               "        SUM(consulta.COchenta)AS COchenta, " +
                               "        SUM(consulta.MasCOchenta)AS MasCOchenta, " +
                               "        SUM(consulta.No_Vencidas + consulta.Treinta +consulta.Sesenta +consulta.Noventa+ consulta.COchenta + consulta.MasCOchenta) AS Total   " +
                               " FROM   (   " +
                               "         SELECT TerceroID, Descriptivo,   " +
                               "            No_Vencidas = Signo * CASE WHEN DifDia <= 0 then No_Vencidas else 0 end,    " +
                               "            Treinta =   Signo * SUM(CASE WHEN DifDia BETWEEN  1 AND 30 then No_Vencidas else 0 end),  " +
                               "            Sesenta =   Signo * SUM(CASE WHEN DifDia BETWEEN  31 AND 60 then No_Vencidas else 0 end),  " +
                               "            Noventa =   Signo * SUM(CASE WHEN DifDia BETWEEN 61 AND 90 then No_Vencidas else 0 end),  " +
                               "            COchenta =  Signo * SUM(CASE WHEN DifDia BETWEEN 91 AND 180 then No_Vencidas else 0 end),  " +
                               "            MasCOchenta =  Signo * SUM(CASE WHEN DifDia > 181 then No_Vencidas else 0 end)   " +
                               "         FROM  (     " +
                               "              SELECT cta.FacturaFecha, cta.VtoFecha,    " +
                               "                saldo.CuentaID,	tercero.TerceroID, tercero.Descriptivo,    " +
                               "                CASE WHEN(cuenta.OrigenMonetario = 1) THEN (saldo.DbSaldoIniLocML+saldo.DbOrigenLocML+saldo.CrSaldoIniLocML+saldo.CrOrigenLocML)    " +
                               "             	ELSE (saldo.DbSaldoIniExtML+saldo.DbOrigenExtML+saldo.CrSaldoIniExtML+saldo.CrOrigenExtML) END as No_Vencidas,	   " +
                               "                (DATEDIFF (DAY, VtoFecha, DATEADD(MONTH,1,'01/08/2015')-1))  AS DifDia,     " +
                               "				CASE WHEN cuenta.Naturaleza = 1 then 1 ELSE -1 END as Signo,     " +
                               "                0 as Treinta, 0 as Sesenta, 0 as Noventa, 0 as CVeinte, 0 as CCincuenta, 0 as COchenta, 0 as MasCOchenta     " +
                               "              FROM cpCuentaXPagar cta   WITH(NOLOCK)       " +
                               "				INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on (ctrl.NumeroDoc = cta.NumeroDoc)     " +
                               "				INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on saldo.IdentificadorTR = ctrl.NumeroDoc      " +
                               "				INNER JOIN coPlanCuenta cuenta WITH(NOLOCK)  on cuenta.CuentaID = saldo.CuentaID  and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta           " +
                               "				INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero      " +
                               "              WHERE  (ctrl.DocumentoID = 21 or ctrl.DocumentoID = 26)  AND cta.EmpresaID = @EmpresaID  " +
                               "				  AND YEAR(PeriodoID) = @Año AND MONTH(saldo.PeriodoID) = @Mes " +
                               "                  AND CASE WHEN(cuenta.OrigenMonetario = 1) THEN (saldo.DbSaldoIniLocML+saldo.DbOrigenLocML+saldo.CrSaldoIniLocML+saldo.CrOrigenLocML)   " +
                               "				      ELSE (saldo.DbSaldoIniExtML+saldo.DbOrigenExtML+saldo.CrSaldoIniExtML+saldo.CrOrigenExtML) END <> 0  " +
                                                   where +
                               "               ) AS Detalle    " +
                               "            GROUP BY Descriptivo, Detalle.TerceroID, Detalle.DifDia, Detalle.No_Vencidas, Detalle.Signo    " +
                               "                         )  " +
                               "             as consulta  " +
                               "             GROUP BY consulta.TerceroID, consulta.Descriptivo " +
                               "             ORDER BY consulta.Descriptivo ";
                        #endregion
                    }
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@documentId2", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Mes", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);

                    #endregion
                    #region Asignacion de valores a parametros

                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.CausarFacturas;
                    mySqlCommandSel.Parameters["@documentId2"].Value = AppDocuments.NotaCreditoCxP;
                    mySqlCommandSel.Parameters["@Mes"].Value = fechaIni.Value.Month;
                    mySqlCommandSel.Parameters["@Año"].Value = fechaIni.Value.Year;
                    #endregion

                }
                #endregion
                #region Radicaciones
                else if (documentoID == AppReports.cpRadicaciones)
                {
                    #region Configuracion Filtros

                    string filterTercero = "", estado = "", order = "";

                    if (!string.IsNullOrWhiteSpace(tercero))
                    {
                        filterTercero = " AND ctrl.TerceroId = @Tercero";
                        mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@Tercero"].Value = tercero;
                    }                       

                    switch (tipoReporte.Value)
                    {
                        case (1):
                            estado = " AND ctrl.Estado in (3,8) "; //Aprobado - Contabilizado
                            break;
                        case (2):
                            estado = " AND ctrl.Estado in (1,2,6) "; //Para aprobacion - Radicado
                            break;
                        case (3):
                            estado = " AND ctrl.Estado in (5) "; //Devuelto
                            break;
                    }
                    if (romp == 1)
                        order = " ORDER BY RadicaFecha,DocumentoTercero ";
                    else
                        order = " ORDER BY NombreTer ";
                    #endregion
                    #region CommanText

                    mySqlCommandSel.CommandText =
                            " SELECT RANK() over (order by RadicaFecha,DocumentoTercero) AS Item, "+
                            "   cpCuentaXPagar.RadicaFecha, ctrl.DocumentoTercero, ctrl.TerceroID, " +
                            "   ter.Descriptivo as NombreTer, cpCuentaXPagar.FacturaFecha, ctrl.Valor, " +
                            "   CASE WHEN ctrl.Iva IS NULL  THEN 0 ELSE ctrl.Iva END AS Iva, " +
                            "   ctrl.Valor + CASE WHEN ctrl.Iva IS NULL THEN 0 ELSE ctrl.Iva END as Total, " +
                            "   CASE   WHEN(ctrl.Estado = 0) THEN 'ANULADO'  " +
                                " WHEN(ctrl.Estado = 1 OR ctrl.Estado = 2) THEN 'POR CAUSAR'  " +
                                " WHEN (ctrl.Estado = 3 OR ctrl.Estado = 8) THEN 'CAUSADO'  " +
                                " WHEN (ctrl.Estado = 4) THEN 'REVERTIDO' " +
                                " WHEN (ctrl.Estado = 5) THEN 'DEVUELTO' " +
                                " WHEN (ctrl.Estado = 6) THEN 'RADICADO' " +
                                " WHEN (ctrl.Estado = 7) THEN 'REVISADO' " +
                                " WHEN (ctrl.Estado = -1) THEN 'CANCELADO' " +
                                " END AS Estado " +
                            " FROM cpCuentaXPagar with(nolock) " +
                            " INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = cpCuentaXPagar.NumeroDoc  " +
                            " INNER JOIN coTercero ter with(nolock) ON ter.TerceroID = ctrl.TerceroID and ter.EmpresaGrupoID=ctrl.eg_coTercero " +
                            " WHERE cpCuentaXPagar.EmpresaID = @EmpresaID  " +
                            " AND DATEPART(YEAR, cpCuentaXPagar.RadicaFecha)>= @yearIni " +
                            " AND DATEPART(YEAR, cpCuentaXPagar.RadicaFecha)<= @yearFin " +
                            " AND DATEPART(MONTH, cpCuentaXPagar.RadicaFecha) >= @FechaIni " +
                            " AND DATEPART(MONTH, cpCuentaXPagar.RadicaFecha) <= @FechaFin " +
                        filterTercero + estado + order;

                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@yearIni", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@yearFin", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.Int);                 
                    #endregion
                    #region Asignacion valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@yearIni"].Value = fechaIni.Value.Year;
                    mySqlCommandSel.Parameters["@yearFin"].Value = fechaFin.Value.Year;
                    mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni.Value.Month;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin.Value.Month;                 
                    #endregion

                }
                #endregion
                #region CxP vs Pagos
                else if (documentoID == AppReports.cpCxPvsPagos)
                {
                    #region Filtro y Order
                    if (!string.IsNullOrWhiteSpace(cuentaID)) // CuentaID
                    {
                        where = " AND CtrlCxP.CuentaID = @CuentaID ";
                        mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                        mySqlCommandSel.Parameters["@CuentaID"].Value = facturaNro;
                    }
                    if (!string.IsNullOrWhiteSpace(bancoCuentaID)) // BancoCuentaID
                    {
                        if(tipoReporte == 1)
                            where += " AND Pagos.BancoCuentaID = @BancoCuentaID ";
                        else
                            where += " AND docuBanco.BancoCuentaID = @BancoCuentaID ";
                        mySqlCommandSel.Parameters.Add("@BancoCuentaID", SqlDbType.Char, UDT_BancoCuentaID.MaxLength);
                        mySqlCommandSel.Parameters["@BancoCuentaID"].Value = bancoCuentaID;
                    }
                    if (!string.IsNullOrWhiteSpace(tercero)) // terceroID
                    {
                        where += " AND CtrlCxP.TerceroID = @TerceroID ";
                        mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@TerceroID"].Value = tercero;
                    }
                    if (romp != null) // orden
                    {
                        if (tipoReporte == 1) //CxP vs Pagos
                        {
                            if (romp == 1) //Comprobante
                                where += " Order by CtrlCxP.ComprobanteID,CtrlCxP.ComprobanteIDNro ";
                            else if (romp == 2) //Tercero
                                where += " Order by tercero.Descriptivo,CtrlCxP.DocumentoTercero  "; 
                        }
                        else
                        {
                            if (romp == 1) //Comprobante
                                where += " Order by CtrlPag.ComprobanteID,CtrlPag.ComprobanteIDNro ";
                            else if (romp == 2) //Tercero
                                where += " Order by tercero.Descriptivo,docuBanco.NroCheque  "; 
                        }
                    }

                    #endregion
                    #region CommanText
                    if (tipoReporte == 1) //CxP vs Pagos
                    {
                        #region Carga consulta CxP vs Pagos
                        mySqlCommandSel.CommandText =
                        "SELECT  CtrlCxP.TerceroID, tercero.Descriptivo as TerceroDesc,ctrlCxP.DocumentoTercero as Factura,  " +
                        "        Cast(RTrim(ctrlCxP.ComprobanteID)+'-'+Convert(Varchar, ctrlCxP.ComprobanteIDNro)  as Varchar(100)) as CompCxP,  " +
			            "        MONTH(ctrlCxP.FechaDoc) as MesCxP,YEAR(ctrlCxP.FechaDoc) as AnoCxP,ctrlCxP.Valor as ValorCxP, " +
                        "        Pagos.BancoCuentaID,Pagos.NroCheque,Pagos.CompPago,Pagos.MesPago,Pagos.AnoPago,Pagos.ValorPago " +
	                    "FROM " +
	                    "cpCuentaXPagar cxp " +
	                    "LEFT JOIN (	Select aux.IdentificadorTR,docuBanco.BancoCuentaID,docuBanco.NroCheque, " +
                        "            CASE WHEN docuBanco.BancoCuentaID is not null then Cast(RTrim(CtrlPag.ComprobanteID)+'-'+Convert(Varchar, CtrlPag.ComprobanteIDNro)  as Varchar(100)) ELSE '' END  as CompPago,  " +
				        "            CASE WHEN docuBanco.BancoCuentaID is not null then MONTH(CtrlPag.FechaDoc) ELSE null END as  MesPago, " +
				        "            CASE WHEN docuBanco.BancoCuentaID is not null then YEAR(CtrlPag.FechaDoc) ELSE null END as  AnoPago, " +
				        "            CASE WHEN docuBanco.BancoCuentaID is not null then aux.vlrMdaLoc ELSE null END as  ValorPago,aux.CuentaID " +
			            "        from coAuxiliar aux " +
				        "            INNER JOIN glDocumentoControl CtrlCxP WITH(NOLOCK) ON CtrlCxP.NumeroDoc = aux.IdentificadorTR	  " +
				        "            INNER JOIN glDocumentoControl CtrlPag WITH(NOLOCK) ON CtrlPag.NumeroDoc = aux.NumeroDoc " +
				        "            INNER JOIN tsBancosDocu docuBanco WITH(NOLOCK) ON docuBanco.NumeroDoc = CtrlPag.NumeroDoc   " +
                        "        WHERE aux.EmpresaID = @EmpresaID   AND  " +
                        "                 CtrlCxP.PeriodoDoc between @FechaIni and @FechaFin and  " +
					    "                (CtrlCxP.DocumentoID = 21 or   CtrlCxP.DocumentoID = 26 or   CtrlCxP.DocumentoID = 90021 or   ctrlCxP.DocumentoID = 90026) and " +
					    "                (CtrlPag.DocumentoID = 31 or   CtrlPag.DocumentoID = 36 or   CtrlPag.DocumentoID = 90031 or CtrlPag.DocumentoID = 90036)		 " +
		                "        ) Pagos ON Pagos.IdentificadorTR = cxp.NumeroDoc 	 " +
	                    "INNER JOIN glDocumentoControl ctrlCxP WITH(NOLOCK) ON ctrlCxP.NumeroDoc = cxp.NumeroDoc	   " +
	                    "INNER JOIN coTercero tercero WITH(NOLOCK) ON tercero.TerceroID = CtrlCxP.TerceroID and tercero.EmpresaGrupoID=CtrlCxP.eg_coTercero   " +
                        "WHERE CtrlCxP.EmpresaID = @EmpresaID AND CtrlCxP.PeriodoDoc between @FechaIni and @FechaFin    " + 
                        where;
                        #endregion
                    }
                    else if (tipoReporte == 2) //Pagos vs CxP
                    {
                        #region Carga consulta para  Pagos vs CxP

                        mySqlCommandSel.CommandText =
                          "Select	 " +
                          "  CtrlCxP.TerceroID,tercero.Descriptivo as TerceroDesc,ctrlCxP.DocumentoTercero as Factura,  " +                        
                          "  docuBanco.BancoCuentaID,docuBanco.NroCheque, " +
                          "  Cast(RTrim(CtrlPag.ComprobanteID)+'-'+Convert(Varchar, CtrlPag.ComprobanteIDNro)  as Varchar(100)) as CompPago,  " +
                          "  MONTH(CtrlPag.FechaDoc) as MesPago,YEAR(CtrlPag.FechaDoc) as Ano,aux.vlrMdaLoc as ValorPago, " +
                          "  Cast(RTrim(ctrlCxP.ComprobanteID)+'-'+Convert(Varchar, ctrlCxP.ComprobanteIDNro)  as Varchar(100)) as CompCxP,  " +
                          "  MONTH(ctrlCxP.FechaDoc) as MesCxP,YEAR(ctrlCxP.FechaDoc) as AnoCxP,ctrlCxP.Valor as ValorCxP " +
                        "from coAuxiliar aux " +
                         "   INNER JOIN glDocumentoControl CtrlPag WITH(NOLOCK) ON CtrlPag.NumeroDoc = aux.NumeroDoc " +
                         "   INNER JOIN glDocumentoControl CtrlCxP WITH(NOLOCK) ON CtrlCxP.NumeroDoc = aux.IdentificadorTR  " +
                         "   INNER JOIN tsBancosDocu docuBanco WITH(NOLOCK) ON docuBanco.NumeroDoc = CtrlPag.NumeroDoc  " +
                         "   INNER JOIN coTercero tercero WITH(NOLOCK) ON tercero.TerceroID = CtrlCxP.TerceroID and tercero.EmpresaGrupoID=CtrlCxP.eg_coTercero   " +
                        "WHERE aux.EmpresaID = @EmpresaID  AND "+
                          " CtrlPag.PeriodoDoc between @FechaIni and @FechaFin   and  " +
                          "(CtrlPag.DocumentoID = 31 or   CtrlPag.DocumentoID = 36 or   CtrlPag.DocumentoID = 90031 or CtrlPag.DocumentoID = 90036) " +
                        where;
                        #endregion
                    }
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                    #endregion
                    #region Asignacion de valores a parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                    #endregion                    
                }
                #endregion
                #region Facturas x Pagar
                else if (documentoID == AppReports.cpFacturasXPagar)
                {
                    #region Config. Filtros
                    int mda = Convert.ToInt32(moneda);
                    string terceroFil = "";
                    string cuenta = "";
                    string monedaOr = "";
                    string multi = "";
                    bool isMultimoneda = otroFilter.Equals("1") ? true : false;

                    if (!string.IsNullOrWhiteSpace(tercero))
                        terceroFil = "  AND  coTercero.TerceroID  = @Tercero ";
                    if (!string.IsNullOrWhiteSpace(cuentaID))
                        cuenta = " AND saldo.CuentaID=@Cuenta ";
                    if (mda == 1 || mda == 2)
                        monedaOr = " AND cuenta.OrigenMonetario=@Origen ";
                    if (mda == 3)
                        monedaOr = " AND (cuenta.OrigenMonetario=@Origen OR cuenta.OrigenMonetario=@Origen2) ";                   
                    if (isMultimoneda)
                        multi = " AND SaldoTotalEXT > 0 ";

                    #endregion
                    #region CommandText

                    mySqlCommandSel.CommandText =
                       " SELECT A.* FROM " +
                       "         ( " +
                       "         SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                       "                   cxp.FactEquivalente, saldo.CuentaID,  " +
                       "                   Ctrl.Descripcion as CuentaDesc, " +
                       "                   RTrim(ctrl.Observacion) as Observacion,  " +
                       "                   Ctrl.DocumentoTercero Factura, " +
                       "                   cxp.FacturaFecha, cxp.VtoFecha, " +
                       "                   RTRIM(ctrl.ComprobanteID) + ' - ' + CAST(ctrl.ComprobanteIDNro AS CHAR(5)) as Comprobante,  " +
                       "                    CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                       "                         WHEN cuenta.OrigenMonetario=2 THEN 'EXT' " +
                       "                         END AS MdaOrigen, " +
                       "                   cxp.Valor AS ValorBruto,Ctrl.Valor AS ValorNeto, " +
                       "                   (Ctrl.Valor - ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML +  CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML +   " +
                       "                   CrSaldoIniExtML)) as VlrAbono , " +
                       "                   ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                       "                   CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal, " +
                       "                     ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE cxp.Valor/cambio.TasaCambio END,2) AS ValorBrutoEXT , ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE Ctrl.Valor/cambio.TasaCambio END,2) AS ValorNetoEXT, " +
                       "                     (ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE Ctrl.Valor/cambio.TasaCambio END,2) - ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME +  CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +  " +
                       "                   CrSaldoIniExtME)) as VlrAbonoEXT, " +
                       "                   ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME +  " +
                       "                   CrSaldoIniLocME +  CrSaldoIniExtME) AS SaldoTotalEXT " +
                       "     FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                       "       INNER JOIN cpCuentaXPagar  cxp WITH(NOLOCK) ON cxp.NumeroDoc = saldo.IdentificadorTR   " +
                       "       INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                       "       INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                       "       INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta  " +
                       "       LEFT JOIN glTasaCambio cambio WITH(NOLOCK) ON cambio.Fecha=@Fecha and cambio.EmpresaGrupoID=@EmpresaID " +
                       "     WHERE   saldo.EmpresaID = @EmpresaID " +
                       "	 AND DATEPART(MONTH, saldo.PeriodoID) =  @Month " +
                       "     AND DATEPART(YEAR, saldo.PeriodoID) =  @Year " +
                       "     AND Ctrl.DocumentoID = @documentId " +
                       monedaOr + terceroFil + cuenta +
                       "       ) AS  A " +
                       "       WHERE A.SaldoTotal > 0 " + multi;
                    #endregion
                    #region Parametros

                    mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.Date);
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Origen", SqlDbType.TinyInt);

                    if (mda == 3)
                    {
                        mySqlCommandSel.Parameters.Add("@Origen2", SqlDbType.TinyInt);
                    }
                    mySqlCommandSel.Parameters.Add("@Cuenta", SqlDbType.Char, UDT_CuentaID.MaxLength);

                    #endregion
                    #region Asignacion de Paramtros

                    mySqlCommandSel.Parameters["@Fecha"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@Month"].Value = fechaIni.Value.Month;
                    mySqlCommandSel.Parameters["@Year"].Value = fechaIni.Value.Year;
                    mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.CausarFacturas;
                    mySqlCommandSel.Parameters["@Tercero"].Value = tercero;
                    mySqlCommandSel.Parameters["@Origen"].Value = mda;
                    if (mda == 3)
                    {
                        mySqlCommandSel.Parameters["@Origen"].Value = mda - 1;
                        mySqlCommandSel.Parameters["@Origen2"].Value = mda - 2;
                    }
                    mySqlCommandSel.Parameters["@Cuenta"].Value = cuentaID;

                    #endregion

                }
                #endregion
                #region Facturas Pagadas
                else if (documentoID == AppReports.cpFacturasPagadas)
                {
                    #region Validación del Tercero
                    string terceroFil = "";
                    if (!string.IsNullOrWhiteSpace(tercero))
                    {
                        terceroFil = " AND ctrlCxP.TerceroID = @Tercero ";
                        mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                        mySqlCommandSel.Parameters["@Tercero"].Value = tercero;
                    }
                    #endregion
                    #region CommandText
                    mySqlCommandSel.CommandText =
                     "Select CtrlCxP.DocumentoTercero as Factura,CtrlCxP.TerceroID AS  TerceroId, tercero.Descriptivo,  " +
                        "CtrlPag.DocumentoTercero as ChequeNro,CtrlPag.FechaDoc as FechaPago, CtrlCxP.ComprobanteID + '-' + CAST(CtrlCxP.ComprobanteIDNro AS CHAR(5)) as Comprobante,  " +
                        "banco.BancoCuentaID as Banco,CtrlCxP.CuentaID as CuentaBanco,CtrlCxP.Valor AS ValorFactura, aux.vlrMdaLoc as ValorPago, ABS(CtrlCxP.Valor - aux.vlrMdaLoc) AS SaldoTotal " +
                    "from coAuxiliar aux " +
                        "INNER JOIN glDocumentoControl CtrlPag WITH(NOLOCK) ON CtrlPag.NumeroDoc = aux.NumeroDoc	" +
                        "INNER JOIN glDocumentoControl CtrlCxP WITH(NOLOCK) ON CtrlCxP.NumeroDoc = aux.IdentificadorTR	 " +
                        "INNER JOIN tsBancosDocu docuBanco WITH(NOLOCK) ON docuBanco.NumeroDoc = CtrlPag.NumeroDoc   " +
                        "INNER JOIN tsBancosCuenta banco WITH(NOLOCK) ON banco.BancoCuentaID = docuBanco.BancoCuentaID  and banco.EmpresaGrupoID=docuBanco.eg_tsBancosCuenta  " +
                        "INNER JOIN coTercero tercero WITH(NOLOCK) ON tercero.TerceroID = CtrlCxP.TerceroID and tercero.EmpresaGrupoID=CtrlCxP.eg_coTercero  " +
                    "WHERE aux.EmpresaID =  @EmpresaID  AND aux.Fecha BETWEEN @FechaIni AND @FechaFin   and  " +
                        " (CtrlPag.DocumentoID = 31 or CtrlPag.DocumentoID = 36 or CtrlPag.DocumentoID = 90031 or CtrlPag.DocumentoID = 90036) and " +
                        " (CtrlCxP.DocumentoID = 21 or CtrlCxP.DocumentoID = 26 or CtrlCxP.DocumentoID = 90021 or CtrlPag.DocumentoID = 90026 ) and " +
                        " ABS(CtrlCxP.Valor - aux.vlrMdaLoc) = 0 " + terceroFil;
                    
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                    
                    #endregion
                    #region Asigna Valores
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                    
                    #endregion
                }
                #endregion
                #region Anticipos
                else if (documentoID == AppReports.cpAnticipos)
                {
                    #region Filtros

                    string monedaOr = "", terceroFilter = "";

                    if (!string.IsNullOrEmpty(tercero))
                        terceroFilter = " AND ctrl.TerceroID=@Tercero ";

                    if (moneda == "1" || moneda == "2")
                        monedaOr = " and cuenta.OrigenMonetario= @OrigenMonetario ";
                    else if (moneda == "3")
                        monedaOr = " and (cuenta.OrigenMonetario= @OrigenMonetario OR cuenta.OrigenMonetario= @OrigenMonetario2) ";
                    #endregion
                    #region Query
                    if (agrup == 0)// Detallado
                    {
                        mySqlCommandSel.CommandText =
                                                            " SELECT anticipos.* FROM( " +
                                                            " SELECT  ctrl.TerceroID, " +
                                                            "        tercero.Descriptivo as NombreTercero, " +
                                                            "        CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                                                            "             WHEN cuenta.OrigenMonetario=2 THEN 'EXT' END AS MonedaOrigen, " +
                                                            "        anti.AnticipoTipoID AS TipoAnticipoID, " +
                                                            "        ctrl.DocumentoTercero as Documento, " +
                                                            "        RTRIM(ctrl.Observacion) AS Concepto, " +
                                                            "        anti.RadicaFecha AS Fecha, " +
                                                            "        anti.Valor " +
                                                            " FROM glDocumentoControl ctrl WITH(NOLOCK)  " +
                                                            "        INNER JOIN cpAnticipo anti WITH(NOLOCK) on  ctrl.NumeroDoc = anti.NumeroDoc " +
                                                            "        INNER JOIN coTercero tercero WITH(NOLOCK) on tercero.TerceroID=ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                                                            "        INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) on cuenta.CuentaID=ctrl.CuentaID and cuenta.EmpresaGrupoID=ctrl.eg_coPlanCuenta " +
                                                            "        INNER JOIN cpAnticipoTipo tipo WITH(NOLOCK) on tipo.AnticipoTipoID=anti.AnticipoTipoID and tipo.EmpresaGrupoID=anti.eg_cpAnticipoTipo " +
                                                            " WHERE anti.EmpresaID=@EmpresaID " +
                                                            "        and ctrl.DocumentoID = @DocumentoID " +
                                                            "        and ctrl.FechaDoc<=@FechaDoc " + monedaOr + terceroFilter +
                                                            " ) AS anticipos WHERE anticipos.Valor > 0 ";
                    }
                    else // Resumido
                    {
                        mySqlCommandSel.CommandText =
                                         " SELECT anticipos.* FROM( " +
                                         "               SELECT  ctrl.TerceroID,  " +
                                         "                       tercero.Descriptivo as NombreTercero, " +
                                         "                       CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                                         "                            WHEN cuenta.OrigenMonetario=2 THEN 'EXT' END AS MonedaOrigen, " +
                                         "                       SUM(anti.Valor) AS Valor " +
                                         "               FROM glDocumentoControl ctrl WITH(NOLOCK)  " +
                                         "               INNER JOIN cpAnticipo anti WITH(NOLOCK) on  ctrl.NumeroDoc = anti.NumeroDoc " +
                                         "               INNER JOIN coTercero tercero WITH(NOLOCK) on tercero.TerceroID=ctrl.TerceroID and tercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                                         "               INNER JOIN coPlanCuenta cuenta WITH(NOLOCK) on cuenta.CuentaID=ctrl.CuentaID and cuenta.EmpresaGrupoID=ctrl.eg_coPlanCuenta " +
                                         "               INNER JOIN cpAnticipoTipo tipo WITH(NOLOCK) on tipo.AnticipoTipoID=anti.AnticipoTipoID and tipo.EmpresaGrupoID=anti.eg_cpAnticipoTipo " +
                                         "               INNER JOIN cpConceptoCXP concepto WITH(NOLOCK) on concepto.ConceptoCxPID=anti.ConceptoCxPID and concepto.EmpresaGrupoID=anti.eg_cpConceptoCXP " +
                                         "               WHERE anti.EmpresaID=@EmpresaID " +
                                         "                     AND	ctrl.DocumentoID = @DocumentoID " +
                                         "                     and ctrl.FechaDoc <= @FechaDoc " + monedaOr + terceroFilter +
                                         "               GROUP BY ctrl.TerceroID, " +
                                         "                       tercero.Descriptivo, " +
                                         "                       cuenta.OrigenMonetario " +
                                         " ) AS anticipos WHERE anticipos.Valor > 0 ";
                    }

                    #endregion
                    #region Creación de parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, 10);
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@FechaDoc", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                    if (moneda == "3")
                        mySqlCommandSel.Parameters.Add("@OrigenMonetario2", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);

                    #endregion
                    #region Asignación de parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Anticipos;
                    mySqlCommandSel.Parameters["@FechaDoc"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@OrigenMonetario"].Value = moneda;
                    if (moneda == "3")
                    {
                        mySqlCommandSel.Parameters["@OrigenMonetario"].Value = Convert.ToInt32(moneda) - 1;
                        mySqlCommandSel.Parameters["@OrigenMonetario2"].Value = Convert.ToInt32(moneda) - 2;
                    }
                    mySqlCommandSel.Parameters["@Tercero"].Value = tercero;
                    #endregion
                }
                #endregion

                #region Flujo Semanal
                else if (documentoID == AppReports.cpFlujoSemanalResumido)
                {
                    #region Filtros
                    List<DateTime> semanas = (List<DateTime>)otroFilter;
                    string monedaOr = "", terceroFil = "";

                    if (!string.IsNullOrEmpty(tercero))
                        terceroFil = " AND ctrl.TerceroID=@Tercero ";

                    if (moneda == "1" || moneda == "2")
                        monedaOr = " and cuenta.OrigenMonetario= @OrigenMonetario ";
                    if (moneda == "3")
                        monedaOr = " and (cuenta.OrigenMonetario= @OrigenMonetario OR cuenta.OrigenMonetario= @OrigenMonetario2) ";
                    #endregion

                    #region Query
                    if (agrup == 1) // Detallado
                    {
                        mySqlCommandSel.CommandText =
                                                " SELECT A.TerceroID,A.Descriptivo AS NombreTerc,A.ValorNeto, " +
                                                "                           CASE WHEN A.Moneda=1 THEN 'LOC' " +
                                                "                            WHEN A.Moneda=2 THEN 'EXT' " +
                                                "                            END AS MdaOrigen, " +
                                                "                            A.Factura, " +
                                                "                            A.Descripcion, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fechaIni AND @fecha1 THEN A.ValorNeto ELSE 0 END AS Semana1, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fecha1+1 AND @fecha2 THEN A.ValorNeto ELSE 0 END AS Semana2, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fecha2+1 AND @fecha3 THEN A.ValorNeto ELSE 0 END AS Semana3, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fecha3+1 AND @fecha4 THEN A.ValorNeto ELSE 0 END AS Semana4, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN  " +
                                                "    CASE WHEN @fecha4=@fechaFin THEN @fecha4 ELSE @fecha4+1 END  " +
                                                "    AND  " +
                                                "    CASE WHEN @fecha5+1=@fechaFin THEN @fechaFin ELSE @fecha5 END  " +
                                                "    THEN A.ValorNeto ELSE 0 END AS Semana5, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN  " +
                                                "    CASE WHEN @fecha5=@fechaFin THEN @fecha5 ELSE @fecha5+1 END " +
                                                "    AND  " +
                                                "    @fechaFin  " +
                                                "    THEN A.ValorNeto ELSE 0 END AS Semana6 " +
                                                " FROM " +
                                                " ( " +
                                                " SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                                                "           ctrl.Valor AS ValorNeto, " +
                                                "           cxp.VtoFecha, " +
                                                "           cuenta.OrigenMonetario AS Moneda, " +
                                                "           ctrl.DocumentoTercero Factura, " +
                                                "           ctrl.Descripcion, " +
                                                "           ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                                                "           CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal    " +
                                                " FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                                                "   INNER JOIN cpCuentaXPagar  cxp WITH(NOLOCK) ON cxp.NumeroDoc = saldo.IdentificadorTR   " +
                                                "   INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.DocumentoID = 21   AND Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                                                "   INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero  " +
                                                "   INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta  " +
                                                " WHERE   saldo.EmpresaID =  @EmpresaID " +
                                                "   AND saldo.PeriodoID BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 " +
                                                 terceroFil + monedaOr +
                                                "   ) AS  A " +
                                                "   WHERE A.SaldoTotal > 0 " +
                                                "   AND A.VtoFecha BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 ";
                    }
                    else // Resumido
                    {
                        mySqlCommandSel.CommandText =
                                    "SELECT Detallado.TerceroID,Detallado.NombreTerc, " +
                                        "SUM(Detallado.Semana1) AS Semana1, " +
                                        "SUM(Detallado.Semana2) AS Semana2, " +
                                        "SUM(Detallado.Semana3) AS Semana3, " +
                                        "SUM(Detallado.Semana4) AS Semana4, " +
                                        "SUM(Detallado.Semana5) AS Semana5, " +
                                        "SUM(Detallado.Semana6) AS Semana6 " +
                                    "FROM ( " +
                                         " SELECT A.TerceroID,A.Descriptivo AS NombreTerc,A.ValorNeto, " +
                                                "                           CASE WHEN A.Moneda=1 THEN 'LOC' " +
                                                "                            WHEN A.Moneda=2 THEN 'EXT' " +
                                                "                            END AS MdaOrigen, " +
                                                "                            A.Factura, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fechaIni AND @fecha1 THEN A.ValorNeto ELSE 0 END AS Semana1, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fecha1+1 AND @fecha2 THEN A.ValorNeto ELSE 0 END AS Semana2, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fecha2+1 AND @fecha3 THEN A.ValorNeto ELSE 0 END AS Semana3, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN @fecha3+1 AND @fecha4 THEN A.ValorNeto ELSE 0 END AS Semana4, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN  " +
                                                "    CASE WHEN @fecha4=@fechaFin THEN @fecha4 ELSE @fecha4+1 END  " +
                                                "    AND  " +
                                                "    CASE WHEN @fecha5+1=@fechaFin THEN @fechaFin ELSE @fecha5 END  " +
                                                "    THEN A.ValorNeto ELSE 0 END AS Semana5, " +
                                                "    CASE WHEN A.VtoFecha BETWEEN  " +
                                                "    CASE WHEN @fecha5=@fechaFin THEN @fecha5 ELSE @fecha5+1 END " +
                                                "    AND  " +
                                                "    @fechaFin  " +
                                                "    THEN A.ValorNeto ELSE 0 END AS Semana6 " +
                                                " FROM " +
                                                " ( " +
                                                " SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                                                "           ctrl.Valor AS ValorNeto, " +
                                                "           cxp.VtoFecha, " +
                                                "           cuenta.OrigenMonetario AS Moneda, " +
                                                "           ctrl.DocumentoTercero Factura, " +
                                                "           ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                                                "           CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal    " +
                                                " FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                                                "   INNER JOIN cpCuentaXPagar  cxp WITH(NOLOCK) ON cxp.NumeroDoc = saldo.IdentificadorTR   " +
                                                "   INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.DocumentoID = 21   AND Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                                                "   INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero  " +
                                                "   INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta  " +
                                                " WHERE   saldo.EmpresaID =  @EmpresaID " +
                                                "   AND saldo.PeriodoID BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 " +
                                                 terceroFil + monedaOr +
                                                "   ) AS  A " +
                                                "   WHERE A.SaldoTotal > 0 " +
                                                "   AND A.VtoFecha BETWEEN  @fechaIni  and DATEADD(MONTH,1,@fechaIni)-1 " +
                                        " ) AS Detallado " +
                                        "GROUP BY TerceroID,NombreTerc";
                    }

                    #endregion

                    #region Creación de parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, 10);
                    mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@OrigenMonetario", SqlDbType.TinyInt);
                    if (moneda == "3")
                        mySqlCommandSel.Parameters.Add("@OrigenMonetario2", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);

                    for (int i = 1; i < semanas.Count - 1; i++)
                    {
                        mySqlCommandSel.Parameters.Add("@fecha" + i, SqlDbType.DateTime);
                    }
                    // fecha adicional si hay 5 domingos en el mes
                    if (semanas.Count == 6)
                    {
                        mySqlCommandSel.Parameters.Add("@fecha5", SqlDbType.DateTime);
                    }
                    #endregion

                    #region Asignación de parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Anticipos;
                    mySqlCommandSel.Parameters["@fechaIni"].Value = semanas[0];
                    mySqlCommandSel.Parameters["@fechaFin"].Value = semanas[semanas.Count - 1];

                    for (int i = 1; i < semanas.Count - 1; i++)
                    {
                        mySqlCommandSel.Parameters["@fecha" + i].Value = semanas[i];
                    }
                    // fecha adicional si hay 5 domingos en el mes
                    if (semanas.Count == 6)
                    {
                        mySqlCommandSel.Parameters["@fecha5"].Value = semanas[semanas.Count - 1].AddDays(1);
                    }
                    mySqlCommandSel.Parameters["@OrigenMonetario"].Value = moneda;
                    if (moneda == "3")
                    {
                        mySqlCommandSel.Parameters["@OrigenMonetario"].Value = Convert.ToInt32(moneda) - 1;
                        mySqlCommandSel.Parameters["@OrigenMonetario2"].Value = Convert.ToInt32(moneda) - 2;
                    }
                    mySqlCommandSel.Parameters["@Tercero"].Value = tercero;
                    #endregion
                }
                #endregion
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


        #endregion
    }
}


