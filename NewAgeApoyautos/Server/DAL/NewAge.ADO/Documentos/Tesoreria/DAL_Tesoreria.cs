using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL Tesoreria
    /// </summary>
    public class DAL_Tesoreria : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Tesoreria(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Pagos

        /// <summary>
        /// Obtiene la lista de pagos para su programación
        /// </summary>
        /// <returns>Lista de pagos para su programación</returns>
        public List<DTO_ProgramacionPagos> DAL_ProgramacionPagos_Get(DateTime periodo, string libroID)
        {
            try
            {
                List<DTO_ProgramacionPagos> result = new List<DTO_ProgramacionPagos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.Date);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = libroID;

                mySqlCommand.CommandText =
                    "select ctrl.DocumentoID, ctrl.NumeroDoc, ctrl.DocumentoTercero, ctrl.TerceroID, terc.Descriptivo, ctrl.MonedaID, ctrl.Fecha, ctrl.ComprobanteIdNro, " +
                    "	ctrl.Observacion, cxp.PagoInd, cxp.PagoInd as PagoIndInicial, cxp.BancoCuentaID, cxp.ValorPago, saldo.SaldoML, saldo.SaldoME " +
                    "from glDocumentoControl AS ctrl with(nolock) " +
                    "	INNER JOIN coTercero AS terc with(nolock) ON ctrl.TerceroID = terc.TerceroID and ctrl.eg_coTercero = terc.EmpresaGrupoID " +
                    "	INNER JOIN cpCuentaXPagar AS cxp with(nolock) ON ctrl.NumeroDoc = cxp.NumeroDoc " +
                    "	INNER JOIN " +
                    "	( " +
                    "		select IdentificadorTR, " +
                    "			sum(saldo.DbOrigenLocML + saldo.DbOrigenExtML + saldo.CrOrigenLocML + saldo.CrOrigenExtML + saldo.DbSaldoIniLocML + saldo.DbSaldoIniExtML + saldo.CrSaldoIniLocML + saldo.CrSaldoIniExtML) * -1 AS SaldoML, " +
                    "			sum(saldo.DbOrigenLocME + saldo.DbOrigenExtME + saldo.CrOrigenLocME + saldo.CrOrigenExtME + saldo.DbSaldoIniLocME + saldo.DbSaldoIniExtME + saldo.CrSaldoIniLocME + saldo.CrSaldoIniExtME) * -1 AS SaldoME  " +
                    "		from coCuentaSaldo saldo with(nolock)" +
                    "		where PeriodoID = @PeriodoID and BalanceTipoID = @BalanceTipoID" +
                    "		group by IdentificadorTR " +
                    "	) as saldo on ctrl.NumeroDoc = saldo.IdentificadorTR " +
                    "where ctrl.EmpresaID = @EmpresaID AND ctrl.DocumentoID in (21,26) AND (SaldoML <> 0 OR SaldoME <> 0) " +
                    "order by terc.Descriptivo, ctrl.DocumentoTercero ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_ProgramacionPagos programacionPagos = new DTO_ProgramacionPagos(dr);
                    programacionPagos.Index = index;
                    result.Add(programacionPagos);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_GetProgramacionPagos");
                throw exception;

            }
        }

        /// <summary>
        /// Actualiza la programacion de pago actual
        /// </summary>
        /// <param name="programacionPago">DTO de la programacion de pago a actualizar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Devuelve verdadero si se pago todo el pago, de lo contrario devuelve falso</returns>
        public bool DAL_CuentaXPagar_Update(DTO_ProgramacionPagos programacionPago, bool pagoAprobacionInd)
        {
            try
            {
                bool result = false;
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "UPDATE cpCuentaXPagar " +
                    "SET " +
                    "   PagoInd = @PagoInd, " +
                    "   BancoCuentaID = @BancoCuentaID, " +
                    "   ValorPago = @ValorPago, " +
                    "   PagoAprobacionInd = @PagoAprobacionInd," +
                    "   TerceroID = @TerceroID, " +
                    "   eg_coTercero = @eg_coTercero " +
                    "WHERE (NumeroDoc = @NumeroDoc)";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PagoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@ValorPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PagoAprobacionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@NumeroDoc"].Value = programacionPago.NumeroDoc.Value;
                mySqlCommand.Parameters["@PagoInd"].Value = programacionPago.PagoInd.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = programacionPago.BancoCuentaID.Value;
                mySqlCommand.Parameters["@ValorPago"].Value = programacionPago.ValorPago.Value;
                mySqlCommand.Parameters["@PagoAprobacionInd"].Value = pagoAprobacionInd;
                mySqlCommand.Parameters["@TerceroID"].Value = programacionPago.TerceroID.Value;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);


                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommand.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_Update");
                throw exception;

            }
        }

        /// <summary>
        /// Consulta las facturas programadas previamente para pagar
        /// </summary>
        /// <returns>Lista de facturas por tercero para pagar</returns>
        public List<DTO_PagoFacturas> DAL_PagoFacturas_Get()
        {
            try
            {
                List<DTO_PagoFacturas> result = new List<DTO_PagoFacturas>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaHoy", SqlDbType.Date);
                mySqlCommand.Parameters.Add("@PagoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PagoAprobacionInd", SqlDbType.TinyInt);

                mySqlCommand.CommandText =
                    "SELECT pf.BancoCuentaID, pf.MonedaBancoCuenta, pf.MonedaID, pf.TerceroID, pf.Descriptivo, pf.BeneficiarioID, pf.Beneficiario,pf.ComprobanteID, " +
                    "	COUNT(pf.ValorPago) AS NumeroFacturas, SUM(pf.ValorPago) AS TotalFacturas " +
                    "FROM " +
                    "( " +
                    "	SELECT DISTINCT cpcxp.NumeroDoc, cpcxp.BancoCuentaID, cod.MonedaOrigen AS MonedaBancoCuenta, gldc.MonedaID, " +
                    "		gldc.TerceroID, ct.Descriptivo, IsNull(cpcxp.ValorPago,0) as ValorPago, cpcxp.TerceroID as BeneficiarioID, be.Descriptivo as Beneficiario,cod.ComprobanteID " +
                    "	FROM  glDocumentoControl AS gldc with (nolock) " +
                    "		INNER JOIN cpCuentaXPagar AS cpcxp with(nolock) ON gldc.NumeroDoc = cpcxp.NumeroDoc " +
                    "		INNER JOIN coCuentaSaldo AS cocs with(nolock) ON cpcxp.NumeroDoc = cocs.IdentificadorTR " +
                    "		INNER JOIN coTercero AS ct with(nolock) ON cocs.TerceroID = ct.TerceroID and cocs.eg_coTercero = ct.EmpresaGrupoID " +
                    "       INNER JOIN coTercero AS be with(nolock) ON cpcxp.TerceroID = be.TerceroID and cpcxp.eg_coTercero = be.EmpresaGrupoID  " +
                    "		INNER JOIN tsBancosCuenta AS tsbc with(nolock) ON tsbc.BancoCuentaID = cpcxp.BancoCuentaID and cpcxp.eg_tsBancosCuenta = tsbc.EmpresaGrupoID " +
                    "		INNER JOIN coDocumento AS cod with(nolock) ON cod.EmpresaGrupoID = tsbc.eg_coDocumento AND cod.coDocumentoID = tsbc.coDocumentoID " +
                    "	WHERE gldc.EmpresaID=@EmpresaID AND gldc.DocumentoID in (21,26) AND cpcxp.PagoInd = @PagoInd AND cpcxp.PagoAprobacionInd = @PagoAprobacionInd " +
                    "       AND datediff(day, cpcxp.FacturaFecha, @FechaHoy) >= 0 " +
                    ") pf " +
                    "GROUP BY pf.BancoCuentaID, pf.MonedaBancoCuenta, pf.MonedaID, pf.TerceroID, pf.Descriptivo, pf.BeneficiarioID, pf.Beneficiario,pf.ComprobanteID ";

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaHoy"].Value = DateTime.Today;
                mySqlCommand.Parameters["@PagoInd"].Value = Convert.ToByte(true);
                mySqlCommand.Parameters["@PagoAprobacionInd"].Value = Convert.ToByte(true);


                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_PagoFacturas programacionPagos = new DTO_PagoFacturas(dr);
                    programacionPagos.Index = index;
                    programacionPagos.PagoFacturasInd.Value = true;
                    result.Add(programacionPagos);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_GetPagoFacturas");
                throw exception;

            }
        }

        /// <summary>
        /// Obtiene los detalles de las facturas con esos valores
        /// </summary>
        /// <param name="TerceroID">El Tercero al cual se le va a pagar las facturas</param>
        /// <param name="BancoCuentaID">La cuenta con la que se van a pagar las facturas</param>
        /// <returns>Facturas a pagar relacionadas</returns>
        public List<DTO_DetalleFactura> DAL_Pagos_GetDetallesFacturas(string terceroID, string bancoCuentaID)
        {
            try
            {
                List<DTO_DetalleFactura> result = new List<DTO_DetalleFactura>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PagoAprobacionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char);

                mySqlCommand.CommandText =
                    "SELECT DISTINCT cpcxp.NumeroDoc " +
                    "	, gldc.DocumentoTercero " +
                    "	, gldc.Observacion " +
                    "	, gldc.MonedaID " +
                    "	, cpcxp.ValorPago " +
                    "FROM  glDocumentoControl AS gldc with(nolock) " +
                    "	INNER JOIN cpCuentaXPagar AS cpcxp with(nolock) ON gldc.NumeroDoc = cpcxp.NumeroDoc " +
                    "WHERE (gldc.DocumentoID in (21,26)) " +
                    "	AND (cpcxp.FacturaFecha <= GETDATE()) " +
                    "	AND (cpcxp.PagoInd = @PagoInd) " +
                    "	AND (cpcxp.PagoAprobacionInd = @PagoAprobacionInd) " +
                    "	AND (gldc.TerceroID = @TerceroID) " +
                    "	AND (cpcxp.BancoCuentaID = @BancoCuentaID) ";

                mySqlCommand.Parameters["@PagoInd"].Value = Convert.ToByte(true);
                mySqlCommand.Parameters["@PagoAprobacionInd"].Value = Convert.ToByte(true);
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = bancoCuentaID;


                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    var detalleFactura = new DTO_DetalleFactura(dr);
                    detalleFactura.Index = index;
                    result.Add(detalleFactura);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_GetDetallesFacturas");
                throw exception;

            }
        }

        /// <summary>
        /// Función que cargauna lista de objetos con información de los cheques de acuerdo al filtro
        /// </summary>
        /// <param name="bancoID">Id de l banco </param>
        /// <param name="nit">Tercero ID</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha Final de la consulta</param>
        /// <param name="numCheque">Documento tercero del glControl</param>
        /// <returns>Lista de pagos</returns>
        public List<DTO_ChequesGirados> DAL_Pagos_GetChequesGirados(string bancoID, string nit, DateTime fechaIni, DateTime fechaFin, string numCheque)
        {
            try
            {
                List<DTO_ChequesGirados> results = new List<DTO_ChequesGirados>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string filterNit = "";
                string filternumCheque = "";
                string filterBancosWhere = "";

                if (!string.IsNullOrEmpty(numCheque))
                    filternumCheque = " AND Bancos.NroCheque = @numCheque ";
                if (!string.IsNullOrEmpty(nit))
                    filterNit = " AND Ctrl.TerceroID = @Nit ";
                if (!string.IsNullOrEmpty(bancoID))
                    filterBancosWhere = " AND BanCta.BancoCuentaID = @Banco ";

                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                            " SELECT Ctrl.DocumentoTercero, Ctrl.NumeroDoc, Bancos.BancoCuentaID, Bancos.NroCheque as NumCheque, Ctrl.LugarGeograficoID, " +
                            "   Ctrl.TerceroID as Nit, coTercero.Descriptivo as Nombre,Ctrl.FechaDoc as Fecha, " +
                            "   abs(Bancos.Valor) as VlrGirado, Ctrl.ComprobanteID, Ctrl.ComprobanteIDNro " +
                            " from glDocumentoControl Ctrl with(nolock) " +
                            "   INNER JOIN coTercero with(nolock) ON (coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID = Ctrl.eg_coTercero) " +
                            "   INNER JOIN tsBancosDocu Bancos with(nolock) ON Bancos.NumeroDoc = Ctrl.NumeroDoc " +
                            " INNER JOIN tsBancosCuenta BanCta with(nolock) ON  (Bancos.BancoCuentaID = BanCta.BancoCuentaID and Bancos.EmpresaID = BanCta.EmpresaGrupoID) " +
                            " Where Ctrl.DocumentoID = @DocumentId " + filterBancosWhere +
                            "   AND Ctrl.FechaDoc BETWEEN @FechaIni AND @FechaFin " +
                            "   AND Ctrl.EmpresaID = @EmpresaID " + filternumCheque + filterNit;
                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Banco", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Nit", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@numCheque", SqlDbType.Char);
                #endregion
                #region Asignacion de valores a parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.DesembolsoFacturas;
                mySqlCommandSel.Parameters["@Banco"].Value = bancoID;
                mySqlCommandSel.Parameters["@Nit"].Value = nit;
                mySqlCommandSel.Parameters["@numCheque"].Value = numCheque;
                #endregion

                DTO_ChequesGirados doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ChequesGirados(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Tesoreria_GetChequesGirados");
                throw exception;
            }
        }

        #endregion

        #region Pagos Electronicos

        /// <summary>
        /// Consulta los pagos sin transmitir al banco
        /// </summary>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> DAL_Tesoreria_GetPagosElectronicosSinTransmitir()
        {
            try
            {
                List<DTO_PagosElectronicos> result = new List<DTO_PagosElectronicos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Vacio", SqlDbType.Char, 20);

                mySqlCommand.CommandText =
                    "SELECT gldc.NumeroDoc,tsbd.BancoCuentaID,gldc.MonedaID,gldc.TerceroID,ct.Descriptivo,gldc.Fecha,gldc.ComprobanteID, " +
                    "   gldc.ComprobanteIDNro,tsbd.Valor,ct.BancoID_1 AS Banco,ct.CuentaTipo_1 AS TipoCuenta,ct.BcoCtaNro_1 AS CuentaNro, " +
	                "   GETDATE() AS FechaTransmision,tsbd.Dato4 AS PagosElectronicosInd " +
                    "FROM  glDocumentoControl AS gldc with(nolock) " +
                    "	INNER JOIN tsBancosDocu AS tsbd with(nolock) ON gldc.NumeroDoc = tsbd.NumeroDoc " +
                    "	INNER JOIN coTercero AS ct with(nolock) ON gldc.TerceroID = ct.TerceroID and gldc.eg_coTercero = ct.EmpresaGrupoID " +
                    "WHERE gldc.DocumentoID = @DocumentoID " +
                    "	AND (tsbd.Dato1 = @Vacio OR tsbd.Dato1 IS NULL) " +
                    "	AND (tsbd.Dato2 = @Vacio OR tsbd.Dato2 IS NULL) " +
                    "	AND (tsbd.Dato3 = @Vacio OR tsbd.Dato3 IS NULL)";
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.DesembolsoFacturas;
                mySqlCommand.Parameters["@Vacio"].Value = string.Empty;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    var pagoElectronico = new DTO_PagosElectronicos(dr);
                    pagoElectronico.Index = index;
                    pagoElectronico.PagosElectronicosInd.Value = string.IsNullOrWhiteSpace(dr["PagosElectronicosInd"].ToString()) ? false : true;
                    result.Add(pagoElectronico);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Tesoreria_GetPagosElectronicos");
                throw exception;

            }
        }

        /// <summary>
        /// Consulta los pagos transmitidos al banco, buscando por tercero y fecha de transmicion
        /// </summary>
        /// <param name="terceroID">Tercero al que se le realizó el pago</param>
        /// <param name="fechaTransmicion">Fecha en la que se realizó la transmición</param>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> DAL_Tesoreria_GetPagosElectronicosTransmitidos(string terceroID, DateTime fechaTransmicion)
        {
            try
            {
                List<DTO_PagosElectronicos> result = new List<DTO_PagosElectronicos>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaTransmicion", SqlDbType.DateTime);

                mySqlCommand.CommandText =
                    "SELECT gldc.NumeroDoc,tsbd.BancoCuentaID,gldc.MonedaID,gldc.TerceroID,ct.Descriptivo,gldc.Fecha,gldc.ComprobanteID," + 
	                "   gldc.ComprobanteIDNro, tsbd.Valor, tsbd.Dato1 as Banco, ct.CuentaTipo_1 AS TipoCuenta, tsbd.Dato2 as CuentaNro, tsbd.Dato3 as FechaTransmision " +
                    "FROM  glDocumentoControl AS gldc with(nolock) " +
                    "	INNER JOIN tsBancosDocu AS tsbd with(nolock) ON gldc.NumeroDoc = tsbd.NumeroDoc " +
                    "	INNER JOIN coTercero AS ct with(nolock) ON gldc.TerceroID = ct.TerceroID and gldc.eg_coTercero = ct.EmpresaGrupoID " +
                    "WHERE gldc.DocumentoID = @DocumentoID " +
                    "	AND tsbd.Dato1 IS NOT NULL " +
                    "	AND tsbd.Dato2 IS NOT NULL " +
                    "	AND tsbd.Dato3 IS NOT NULL " +
                    "	AND gldc.TerceroID = @TerceroID " +
                    "	AND tsbd.Dato3 = @FechaTransmicion";
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.DesembolsoFacturas;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@FechaTransmicion"].Value = fechaTransmicion;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    var pagoElectronico = new DTO_PagosElectronicos(dr);
                    pagoElectronico.Index = index;
                    pagoElectronico.DevolverTransmicionInd.Value = false;
                    pagoElectronico.FechaTransmicion.Value = fechaTransmicion;
                    result.Add(pagoElectronico);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Tesoreria_GetPagosElectronicos");
                throw exception;

            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que carga los movimientos de acuerdo al cheque
        /// </summary>
        /// <param name="numDoc">Numero del Cheque</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <returns>Detalles de movimientos</returns>
        public List<DTO_ChequesGiradosDeta> DAL_Pagos_GetChequesGiradosDeta(int numDoc, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<DTO_ChequesGiradosDeta> results = new List<DTO_ChequesGiradosDeta>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    " SELECT Ctrl.DocumentoTercero as NumCheque, coAuxiliar.TerceroID as Nit, " +
                    "   coAuxiliar.PeriodoID as Fecha, coAuxiliar.vlrMdaLoc as VlrGirado, coAuxiliar.ComprobanteID, " +
                    "   coAuxiliar.DocumentoCOM as NroFactura, coAuxiliar.NumeroDoc, Ctrl.Observacion, coAuxiliar.ComprobanteNro " +
                    " FROM coAuxiliar with(nolock) " +
                    "   INNER JOIN glDocumentoControl Ctrl with(nolock) ON Ctrl.NumeroDoc = coAuxiliar.NumeroDoc " +
                    " WHERE Ctrl.DocumentoID = @documentId " +
                        " AND coAuxiliar.IdentificadorTR != coAuxiliar.NumeroDoc " +
                        " AND coAuxiliar.NumeroDoc = @NumDoc " +
                        " AND coAuxiliar.PeriodoID BETWEEN @FechaIni AND @FechaFin " +
                        " AND coAuxiliar.EmpresaID = @EmpresaID AND coAuxiliar.vlrMdaLoc >= 0";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.DesembolsoFacturas;
                mySqlCommandSel.Parameters["@NumDoc"].Value = numDoc;

                DTO_ChequesGiradosDeta doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ChequesGiradosDeta(dr);
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
        /// Funcion que retorna el detalle de los cheques de acuerdo a los filtros
        /// </summary>
        /// <param name="bancoID">Banco </param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_ChequesGiradosDetaReport> GetChequesDetaAux(string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                string filtroBanco = " ";
                string terceroFiltro = " ";
                if (!string.IsNullOrWhiteSpace(bancoID))
                    filtroBanco = "  AND BanCta.BancoCuentaID = " + "'" + bancoID + " '";
                if (!string.IsNullOrWhiteSpace(terceroID))
                    terceroFiltro = " AND coTercero.TerceroID = " + "'" + terceroID + " '";

                List<DTO_ChequesGiradosDetaReport> results = new List<DTO_ChequesGiradosDetaReport>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    " SELECT Ctrl.DocumentoTercero as NumCheque, coAuxiliar.TerceroID as Nit, Bancos.BancoCuentaID,coTercero.Descriptivo as Nombre, Bancos.Beneficiario, BanCta.Descriptivo, " +
                    "   coAuxiliar.PeriodoID as Fecha, coAuxiliar.vlrMdaLoc as VlrGirado, coAuxiliar.MdaTransacc , " +
                    "   Cast(RTrim(coAuxiliar.ComprobanteID)+'-'+Convert(Varchar, coAuxiliar.ComprobanteNro)  as Varchar(100)) as ComprobanteID," +
                    "   coAuxiliar.DocumentoCOM as NroFactura " +
                    " FROM coAuxiliar   " +
                    "   INNER JOIN glDocumentoControl Ctrl ON Ctrl.NumeroDoc = coAuxiliar.NumeroDoc   " +
                    "   INNER JOIN tsBancosDocu Bancos ON Bancos.NumeroDoc = coAuxiliar.NumeroDoc  " +
                    "   INNER JOin coTercero ON coTercero.TerceroID = coAuxiliar.TerceroID and coTercero.EmpresaGrupoID = coAuxiliar.eg_coTercero  " +
                    "   INNER JOIN tsBancosCuenta BanCta ON BanCta.BancoCuentaID = Bancos.BancoCuentaID and BanCta.EmpresaGrupoID = Bancos.eg_tsBancosCuenta " +
                    " WHERE Ctrl.DocumentoID in (31,36) and coAuxiliar.vlrMdaLoc < 0 " +
                    " AND coAuxiliar.IdentificadorTR != coAuxiliar.NumeroDoc  " +
                      filtroBanco +
                      terceroFiltro +
                    " AND coAuxiliar.Fecha BETWEEN @FechaIni AND @FechaFin  " +
                    " AND coAuxiliar.EmpresaID = @EmpresaID "+
                    " order by  Ctrl.DocumentoTercero ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Banco", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@Banco"].Value = bancoID;

                DTO_ChequesGiradosDetaReport doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ChequesGiradosDetaReport(dr);
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
        /// Función que cargauna lista de objetos con información de los cheques de acuerdo al filtro
        /// </summary>
        /// <param name="cajaID">Id de l banco </param>
        /// <param name="terceroID">Tercero ID</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha Final de la consulta</param>
        /// <param name="numReciboCaja">Documento tercero del glControl</param>
        /// <returns>Lista de pagos</returns>
        public List<DTO_QueryReciboCaja> DAL_ReciboCaja_GetByParameter(string cajaID, string terceroID, DateTime fechaIni, DateTime fechaFin, string numReciboCaja)
        {
            SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
            mySqlCommandSel.Transaction = base.MySqlConnectionTx;

            List<DTO_QueryReciboCaja> results = new List<DTO_QueryReciboCaja>();
            string filter = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(numReciboCaja))
                {
                    filter = " AND Ctrl.DocumentoNro = @numReciboCaja ";
                    mySqlCommandSel.Parameters.Add("@numReciboCaja", SqlDbType.Int);
                    mySqlCommandSel.Parameters["@numReciboCaja"].Value = numReciboCaja;
                }
                if (!string.IsNullOrEmpty(terceroID))
                {
                    filter += " AND Ctrl.TerceroID = @TerceroID ";
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroID;
                }
                if (!string.IsNullOrEmpty(cajaID))
                {
                    filter += " AND ReciboCaja.CajaID = Caja.CajaID ";
                    mySqlCommandSel.Parameters.Add("@CajaID", SqlDbType.Char, UDT_CajaID.MaxLength);
                    mySqlCommandSel.Parameters["@CajaID"].Value = cajaID;
                }
                mySqlCommandSel.CommandText =
                    " SELECT distinct Ctrl.NumeroDoc, RTRIM(Ctrl.PrefijoID) +'-'+ CAST(Ctrl.DocumentoNro as char) As PrefDoc, Ctrl.DocumentoNro As NumReciboCaja, ReciboCaja.BancoCuentaID,  " +
                      " Ctrl.TerceroID, coTercero.Descriptivo as Nombre,Ctrl.FechaDoc,ReciboCaja.Valor,Ctrl.ComprobanteID,Ctrl.ComprobanteIDNro, Caja.CajaID " +
                      " from glDocumentoControl Ctrl with(nolock) " +
                      " INNER JOIN coTercero with(nolock) ON coTercero.TerceroID = Ctrl.TerceroID " +
                      " INNER JOIN tsReciboCajaDocu ReciboCaja with(nolock) ON ReciboCaja.NumeroDoc = Ctrl.NumeroDoc " +
                      " INNER JOIN tsCaja Caja with(nolock) ON Caja.CajaID = @CajaID " +
                   " Where Ctrl.EmpresaID = @EmpresaID AND Ctrl.DocumentoID = @DocumentId AND Ctrl.FechaDoc BETWEEN @FechaIni AND @FechaFin " + filter;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@DocumentId", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@DocumentId"].Value = AppDocuments.ReciboCaja;

                DTO_QueryReciboCaja doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_QueryReciboCaja(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReciboCaja_GetByParameter");
                throw exception;
            }
        }

        #endregion
    }
}
