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
    /// DAL de DAL_ReportesTesoreria
    /// </summary>
    public class DAL_ReportesTesoreria : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesTesoreria(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Funcion que retorna el detalle de los cheques de acuerdo a los filtros
        /// </summary>
        /// <param name="bancoID">Banco </param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_ChequesGiradosDetaReport> Report_Ts_ChequesGiradosoDetalle(string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                string filtroBanco = " ";
                string terceroFiltro = " ";
                if (!string.IsNullOrWhiteSpace(bancoID))
                    filtroBanco = "  AND tsBanco.BancoID = " + bancoID;
                if (!string.IsNullOrWhiteSpace(terceroID))
                    terceroFiltro = " AND coTercero.TerceroID = " + "'" + terceroID + " '";

                List<DTO_ChequesGiradosDetaReport> results = new List<DTO_ChequesGiradosDetaReport>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    " SELECT Bancos.BancoCuentaID, tsBanco.Descriptivo, coTercero.TerceroID as Nit , Bancos.Beneficiario as Nombre, " +
                    " SUM(coAuxiliar.vlrMdaLoc) as VlrGirado , coAuxiliar.MdaTransacc " +
                    " FROM coAuxiliar  " +
                    " INNER JOIN glDocumentoControl Ctrl ON Ctrl.NumeroDoc = coAuxiliar.NumeroDoc    " +
                    " INNER JOIN tsBancosDocu Bancos ON Bancos.NumeroDoc = coAuxiliar.NumeroDoc   " +
                    " INNER JOin coTercero ON coTercero.TerceroID = coAuxiliar.TerceroID    " +
                    " INNER JOIN tsBancosCuenta BanCta ON BanCta.BancoCuentaID = Bancos.BancoCuentaID  " +
                    " INNER JOIN tsBanco ON tsBanco.BancoID = BanCta.BancoID  " +
                    " WHERE Ctrl.DocumentoID = @documentId   " +
                    "   AND coAuxiliar.IdentificadorTR != coAuxiliar.NumeroDoc    " +
                    filtroBanco +
                    terceroFiltro +
                    " AND Bancos.BancoCuentaID = BanCta.BancoCuentaID  " +
                    " AND coAuxiliar.Fecha BETWEEN @FechaIni AND @FechaFin    " +
                    " AND coAuxiliar.EmpresaID = @EmpresaID " +
                    " GROUP BY tsBanco.Descriptivo, Bancos.BancoCuentaID, coTercero.Descriptivo, coTercero.TerceroID , coAuxiliar.MdaTransacc ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Banco", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@Banco"].Value = bancoID;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.DesembolsoFacturas;

                DTO_ChequesGiradosDetaReport doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ChequesGiradosDetaReport(dr, true);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_SaldosCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae todos los recibos de caja de acuerdo al periodo
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha finl del reporte</param>
        /// <param name="nit">Tercero ID</param>
        /// <param name="caja">???</param>
        /// <returns>Lista de recibos de caja</returns>
        public List<DTO_RecibosDeCaja> Report_Ts_RecibosDeCaja(DateTime fechaIni, DateTime fechaFin, string nit, string caja)
        {
            try
            {
                string filtrocaja = " ";
                string terceroFiltro = " ";
                if (!string.IsNullOrWhiteSpace(caja))
                    filtrocaja = " AND docu.CajaID = " + "'" + caja + " '"; 
                if (!string.IsNullOrWhiteSpace(nit))
                    terceroFiltro = " AND docu.TerceroID = " + "'" + nit + " '";

                List<DTO_RecibosDeCaja> results = new List<DTO_RecibosDeCaja>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    " Select  Cast(RTrim(ctrl.PrefijoID)+'-'+ Convert(Varchar, ctrl.DocumentoNro)  as Varchar(100)) as Documento,  "+
                    "   ctrl.FechaDoc as Fecha,  docu.TerceroID as Nit, tercero.Descriptivo, docu.Valor, tsCaja.CajaID,  tsCaja.Descriptivo as CajaDesc" +
                    " From tsReciboCajaDocu docu " +
                    "   INNER JOIN glDocumentoControl ctrl on ctrl.NumeroDoc = docu.NumeroDoc " +
                    "   INNER JOIN coTercero tercero ON tercero.TerceroID = docu.TerceroID and  tercero.EmpresaGrupoID = docu.eg_coTercero " +
                    "   INNER JOIN tsCaja  on tsCaja.CajaID = docu.CajaID and  tsCaja.EmpresaGrupoID = docu.eg_tsCaja " +
                    " WHERE ctrl.Fecha BETWEEN @FechaIni AND @FechaFin " +
                    " AND DOCU.EmpresaID = @EmpresaID " +
                    terceroFiltro +
                    filtrocaja;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;

                DTO_RecibosDeCaja doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_RecibosDeCaja(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Report_Ts_RecibosDeCaja");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae el los bancos con sus saldos
        /// </summary>
        /// <param name="fechaIni">Periodo inicial del reporte</param>
        /// <param name="fechaFin">Periodo Final del reporte</param>
        /// <param name="bancoID">BancoId como filtro</param>
        /// <returns>Lista de bancos</returns>
        public List<DTO_LibroBancos> Report_Ts_LibroBancos(DateTime fechaIni, DateTime fechaFin, string bancoID)
        {
            try
            {
                #region Filtros
                string filtroBanco = " ";
                if (!string.IsNullOrWhiteSpace(bancoID))
                    filtroBanco = "AND bCta.BancoId = " + bancoID;
                #endregion

                List<DTO_LibroBancos> results = new List<DTO_LibroBancos>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    "SELECT bCta.BancoCuentaID, bCta.Descriptivo, saldo.CuentaID, " +
                    " (saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML ) - (saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML   ) as SaldoIniLoc, " +
                    " ( saldo.DbSaldoIniLocME + saldo.DbSaldoIniExtME ) - (CrSaldoIniExtME + CrSaldoIniLocME) as SaldoIniExt, " +
                    " saldo.IdentificadorTr " +
                    " FROM tsBancosDocu docu with(nolock) " +
                    " INNER JOIN tsBancosCuenta  bCta  with(nolock) ON bCta.BancoCuentaID = docu.BancoCuentaID " +
                    " INNER JOIN coCuentaSaldo saldo  with(nolock) ON saldo.IdentificadorTR = docu.NumeroDoc " +
                    " INNER JOIN glDocumentoControl ctrl with (noLock) ON ctrl.NumeroDoc = docu.NumeroDoc " +
                    " WHERE  " +
                    " docu.EmpresaID = @EmpresaID " +
                      filtroBanco +
                    " AND Ctrl.PeriodoDoc BETWEEN @FechaIni and @FechaFin ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;

                DTO_LibroBancos doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_LibroBancos(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Report_Ts_LibroBancos");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene una lista de los movimientos por bancos
        /// </summary>
        /// <param name="fechaIni">Fecha incial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="idTr">identificador del CoCuentaSaldo</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_LibroBancosDeta> Report_Ts_LibroBancosDeta(DateTime fechaIni, DateTime fechaFin, int idTr)
        {
            try
            {
                List<DTO_LibroBancosDeta> results = new List<DTO_LibroBancosDeta>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    "SELECT (aux.ComprobanteID) + ' - ' + CAST(aux.ComprobanteNro AS VARCHAR(90)  ) AS Comprobante , " +
                    " aux.Fecha, DocumentoCOM , Descriptivo, vlrMdaLoc, vlrMdaExt " +
                    " FROM coAuxiliar aux with(nolock) " +
                    " WHERE NumeroDoc = @idTr " +
                    " AND periodoID BETWEEN @FechaIni and @FechaFin " +
                    " AND EmpresaID = @EmpresaID ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@idTr", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@idTr"].Value = idTr;

                DTO_LibroBancosDeta doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_LibroBancosDeta(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Report_Ts_LibroBancos");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae las facturas 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="bancoID">Filtro del banco</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_RelacionPagosDeta1> Report_Ts_RelacionPagos(DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque)
        {
            try
            {

                List<DTO_RelacionPagosDeta1> results = new List<DTO_RelacionPagosDeta1>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string filtroBanco = "";
                string filterNit = "";
                string filternumCheque = "";
                string filterBancosWhere = "";

                if (!string.IsNullOrEmpty(numCheque))
                    filternumCheque = " AND Bancos.NroCheque = " + numCheque;
                if (!string.IsNullOrEmpty(nit))
                {
                    filterNit = " AND Ctrl.TerceroID = @Nit ";
                    mySqlCommandSel.Parameters.Add("@Nit", SqlDbType.Char);
                    mySqlCommandSel.Parameters["@Nit"].Value = nit;
                }
                   
                if (!string.IsNullOrEmpty(bancoID))
                {
                    filterBancosWhere = "   AND bcta.BancoCuentaID =  " + bancoID;
                    mySqlCommandSel.Parameters.Add("@Banco", SqlDbType.Char);
                    mySqlCommandSel.Parameters["@Banco"].Value = bancoID;
                }

                #endregion

                mySqlCommandSel.CommandText =
                    " SELECT Bancos.NroCheque,  aux.TerceroID as Nit, coTercero.Descriptivo, " +
                    "   aux.PeriodoID as Fecha,  aux.vlrMdaLoc as VlrGirado, aux.vlrMdaLoc as VlrFactura,  " +
                    "   aux.DocumentoCOM as NroFactura, abs(Bancos.Valor) as Valor, aux.Descriptivo as Observacion, " +
                    "   Ctrl.MonedaID, bcta.BancoCuentaID, bcta.Descriptivo as BancoDesc " +
                    " FROM coAuxiliar aux with(nolock)   " +
                    "   INNER JOIN glDocumentoControl Ctrl with(nolock) ON Ctrl.NumeroDoc = aux.NumeroDoc   " +
                    "   INNER JOIN coTercero with(nolock) ON coTercero.TerceroID = Ctrl.TerceroID  and coTercero.EmpresaGrupoID =  Ctrl.eg_coTercero " +
                    "   INNER JOIN tsBancosDocu Bancos with(nolock) ON Bancos.NumeroDoc = Ctrl.NumeroDoc " +
                    "   INNER JOIN tsBancosCuenta bcta  with(nolock) ON bcta.BancoCuentaId = Bancos.BancoCuentaId and bcta.EmpresaGrupoID = Bancos.eg_tsBancosCuenta " +
                    filtroBanco +
                    " WHERE Ctrl.EmpresaID = @EmpresaID and Ctrl.DocumentoID = 31    " +
                    " AND aux.Fecha BETWEEN @FechaIni AND @FechaFin  " +
                    " AND aux.IdentificadorTR <> aux.NumeroDoc   " +
                    " AND aux.NumeroDoc = Ctrl.NumeroDoc   " +
                    " AND aux.DatoAdd4 is null " +
                    filternumCheque + filterNit + filterBancosWhere;
            
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
             
                DTO_RelacionPagosDeta1 doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_RelacionPagosDeta1(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Report_Ts_RelacionPagosHeader");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el DTO de las facturas por pagar
        /// </summary>
        /// <param name="numDoc">Numero Doc q relaciona las facuras por pagar</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_ReportPagoFactura> DAL_ReportTesoreria_Ts_PagosFacturas(int numDoc)
        {
            try
            {
                List<DTO_ReportPagoFactura> results = new List<DTO_ReportPagoFactura>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText
                //" banco.Beneficiario as nombre,   " +
                mySqlCommandSel.CommandText =
                    " SELECT aux.CuentaID, aux.DocumentoCOM, aux.Descriptivo,   " +
		                    " CASE WHEN (aux.MdaOrigen = 1) THEN vlrMdaLoc ELSE vlrMdaExt END AS valor,   " +
                        " ctrl.DocumentoID, UPPER(doc.Descriptivo) as Documento, ctrl.FechaDoc  as PeriodoID, pref.Descriptivo as ciudad, aux.TerceroID, "+
                        "   tercero.Descriptivo as TerceroNombre,banco.Beneficiario as nombre,   " +
		                    " bancoCta.Descriptivo as banco, bancoCta.CuentaNumero, banco.NroCheque,   " +
		                    " RTRIM (CAST(aux.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST(aux.ComprobanteNro AS CHAR(15)) AS Comprobante,   " +
		                    " RTRIM (CAST( ctrlCxP.ComprobanteID AS CHAR(15))) +' '+'-'+' '+ CAST( ctrlCxP.ComprobanteIDNro AS CHAR(15)) AS ComprobanteCxP,   " +
                            " ctrl.CuentaID AS CuentaContable " +
                     " FROM coAuxiliar AS aux WITH(NOLOCK)   " +
		                    " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = aux.NumeroDoc   " +
		                    " INNER JOIN glDocumentoControl AS ctrlCxP WITH(NOLOCK) ON ctrlCxP.NumeroDoc = aux.IdentificadorTR    " +
		                    " INNER JOIN glDocumento AS doc WITH(NOLOCK) ON doc.DocumentoID = ctrl.DocumentoID   " +
		                    " INNER JOIN glPrefijo as pref WITH(NOLOCK) ON (pref.PrefijoID = aux.PrefijoCOM AND pref.EmpresaGrupoID = aux.eg_glPrefijo)   " +
		                    " INNER JOIN coTercero as tercero WITH(NOLOCK) ON (tercero.TerceroID = aux.TerceroID and tercero.EmpresaGrupoID = aux.eg_coTercero)   " +
		                    " INNER JOIN tsBancosDocu as banco WITH(NOLOCK) ON banco.NumeroDoc = ctrl.NumeroDoc   " +
		                    " INNER JOIN tsBancosCuenta as bancoCta WITH(NOLOCK) ON (bancoCta.BancoCuentaID = banco.BancoCuentaID and bancoCta.EmpresaGrupoID = banco.eg_tsBancosCuenta)  " +
                     " WHERE aux.EmpresaID = @EmpresaID   " +
		                    " AND aux.NumeroDoc = @NumeroDoc " +   
		                    " AND (aux.DatoAdd4 IS NULL OR aux.DatoAdd4 NOT IN ( 'AjEnCambio' ,'AjEnCambioContra' ,'Contrapartida' )) ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                #endregion
                #region Asignacion de Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                #endregion

                DTO_ReportPagoFactura doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportPagoFactura(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;

            }
            catch (Exception ex)
            {
               //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportTesoreria_Ts_PagosFacturas");
                throw exception;
            }
        }

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
        public DataTable DAL_Reportes_Ts_TesoreriaToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero,string nroCheque, string facturaNro,
                string bancoCuentaID, byte? agrup, byte? romp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();

                #region Relacion de Pagos
                if (documentoID == AppReports.tsRelacionPagos)
                {
                    #region Filtros
                    string filtroBanco = "";
                    string filterNit = "";
                    string filternumCheque = "";
                    string filterBancosWhere = "";

                    if (!string.IsNullOrEmpty(nroCheque))
                        filternumCheque = " AND Bancos.NroCheque = " + nroCheque;
                    if (!string.IsNullOrEmpty(tercero))
                    {
                        filterNit = " AND Ctrl.TerceroID = @Nit ";
                        mySqlCommandSel.Parameters.Add("@Nit", SqlDbType.Char);
                        mySqlCommandSel.Parameters["@Nit"].Value = tercero;
                    }

                    if (!string.IsNullOrEmpty(bancoCuentaID))
                    {
                        filterBancosWhere = "   AND bcta.BancoCuentaID =  " + bancoCuentaID;
                        mySqlCommandSel.Parameters.Add("@Banco", SqlDbType.Char);
                        mySqlCommandSel.Parameters["@Banco"].Value = bancoCuentaID;
                    }

                    #endregion

                    #region CommandText
                    mySqlCommandSel.CommandText =
                            " SELECT Bancos.NroCheque,  aux.TerceroID as Nit, coTercero.Descriptivo, " +
                            "   aux.PeriodoID as Fecha,  aux.vlrMdaLoc as VlrGirado, aux.vlrMdaLoc as VlrFactura,  " +
                            "   aux.DocumentoCOM as NroFactura, abs(Bancos.Valor) as Valor, aux.Descriptivo as Observacion, " +
                            "   Ctrl.MonedaID, bcta.BancoCuentaID, bcta.Descriptivo as BancoDesc " +
                            " FROM coAuxiliar aux with(nolock)   " +
                            "   INNER JOIN glDocumentoControl Ctrl with(nolock) ON Ctrl.NumeroDoc = aux.NumeroDoc   " +
                            "   INNER JOIN coTercero with(nolock) ON coTercero.TerceroID = Ctrl.TerceroID  and coTercero.EmpresaGrupoID =  Ctrl.eg_coTercero " +
                            "   INNER JOIN tsBancosDocu Bancos with(nolock) ON Bancos.NumeroDoc = Ctrl.NumeroDoc " +
                            "   INNER JOIN tsBancosCuenta bcta  with(nolock) ON bcta.BancoCuentaId = Bancos.BancoCuentaId and bcta.EmpresaGrupoID = Bancos.eg_tsBancosCuenta " +
                            filtroBanco +
                            " WHERE Ctrl.EmpresaID = @EmpresaID and Ctrl.DocumentoID = 31    " +
                            " AND aux.Fecha BETWEEN @FechaIni AND @FechaFin  " +
                            " AND aux.IdentificadorTR <> aux.NumeroDoc   " +
                            " AND aux.NumeroDoc = Ctrl.NumeroDoc   " +
                            " AND aux.DatoAdd4 is null " +
                            filternumCheque + filterNit + filterBancosWhere; 
                    #endregion

                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;

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


        #endregion

    }
}


