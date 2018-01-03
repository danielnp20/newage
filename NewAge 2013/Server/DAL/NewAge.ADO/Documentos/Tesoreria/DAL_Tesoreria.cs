using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;

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
                    "select ctrl.DocumentoID, ctrl.NumeroDoc, ctrl.DocumentoTercero,ctrl.TerceroID , terc.Descriptivo, ctrl.MonedaID, ctrl.Fecha, ctrl.ComprobanteIdNro, " +
                    "	ctrl.Observacion, cxp.PagoInd, cxp.PagoInd as PagoIndInicial, cxp.BancoCuentaID, cxp.ValorPago,cxp.ConceptoCxPID,concepto.Descriptivo as ConceptoDesc, saldo.SaldoML, saldo.SaldoME, cxp.TerceroID as BeneficiarioID," +
                    "   coterc.Descriptivo as Beneficiario "+
                    "from glDocumentoControl AS ctrl with(nolock) " +
                    "	INNER JOIN coTercero AS terc with(nolock) ON ctrl.TerceroID = terc.TerceroID and ctrl.eg_coTercero = terc.EmpresaGrupoID " +
                    "	INNER JOIN cpCuentaXPagar AS cxp with(nolock) ON ctrl.NumeroDoc = cxp.NumeroDoc " +
                    "	LEFT JOIN tsBancosDocu AS tsbd with(nolock) ON ctrl.NumeroDoc = tsbd.NumeroDoc " +
                    "	LEFT JOIN cpConceptoCxP AS concepto with(nolock) ON concepto.ConceptoCxPID = cxp.ConceptoCxPID and concepto.EmpresaGrupoID = cxp.eg_cpConceptoCxP " +
                    "	LEFT JOIN coTercero AS coterc with(nolock) ON cxp.TerceroID = coterc.TerceroID and cxp.eg_coTercero = coterc.EmpresaGrupoID " +
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
                    programacionPagos.ConceptoCxPDesc.Value = dr["ConceptoDesc"].ToString();
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

                mySqlCommand.Parameters["@TerceroID"].Value = string.IsNullOrEmpty(programacionPago.BeneficiarioID.Value) ? programacionPago.TerceroID.Value : programacionPago.BeneficiarioID.Value;
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



                //"SELECT pf.BancoCuentaID, pf.MonedaBancoCuenta, pf.MonedaID, pf.TerceroID, pf.Descriptivo, pf.BeneficiarioID, pf.Beneficiario,pf.ComprobanteID, " 
                mySqlCommand.CommandText =
                    
                    
                    "SELECT pf.BancoCuentaID, pf.MonedaBancoCuenta, pf.MonedaID, pf.BeneficiarioID AS TerceroID, pf.Beneficiario AS Descriptivo, pf.BeneficiarioID, pf.Beneficiario,pf.ComprobanteID, " +

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
                    "GROUP BY pf.BancoCuentaID, pf.MonedaBancoCuenta, pf.MonedaID,  pf.BeneficiarioID, pf.Beneficiario,pf.ComprobanteID ";
                    //"GROUP BY pf.BancoCuentaID, pf.MonedaBancoCuenta, pf.MonedaID, pf.TerceroID, pf.Descriptivo, pf.BeneficiarioID, pf.Beneficiario,pf.ComprobanteID ";
                
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
        /// Obtiene los detalles de las facturas con esos valores
        /// </summary>
        /// <param name="TerceroID">El Tercero al cual se le va a pagar las facturas</param>
        /// <param name="BancoCuentaID">La cuenta con la que se van a pagar las facturas</param>
        /// <returns>Facturas a pagar relacionadas</returns>
        public List<DTO_DetalleFactura> DAL_Pagos_GetDetallesFacturasBeneficiaro(string BeneficiarioID, string bancoCuentaID)
        {
            try
            {
                List<DTO_DetalleFactura> result = new List<DTO_DetalleFactura>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PagoAprobacionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@BeneficiarioID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char);

                mySqlCommand.CommandText =
                    "SELECT DISTINCT cpcxp.NumeroDoc " +
                    "	, gldc.DocumentoTercero " +
                    "	, gldc.Observacion " +
                    "	, gldc.MonedaID " +
                    "	, gldc.TerceroID" +
                    "	, coter.Descriptivo as Tercero" +
                    "	, cpcxp.ValorPago " +
                        "FROM  glDocumentoControl AS gldc with(nolock) " +
                    "	INNER JOIN cpCuentaXPagar AS cpcxp with(nolock) ON gldc.NumeroDoc = cpcxp.NumeroDoc " +
                    "	INNER JOIN coTercero AS coter with(nolock) ON gldc.TerceroID = coter.TerceroID and gldc.eg_coTercero = coter.EmpresaGrupoID " +
                    "   WHERE (gldc.DocumentoID in (21,26)) " +
                    "	AND (cpcxp.FacturaFecha <= GETDATE()) " +
                    "	AND (cpcxp.PagoInd = @PagoInd) " +
                    "	AND (cpcxp.PagoAprobacionInd = @PagoAprobacionInd) " +
                    "	AND (cpcxp.TerceroID = @BeneficiarioID) " +
                    "	AND (cpcxp.BancoCuentaID = @BancoCuentaID) ";

                mySqlCommand.Parameters["@PagoInd"].Value = Convert.ToByte(true);
                mySqlCommand.Parameters["@PagoAprobacionInd"].Value = Convert.ToByte(true);
                mySqlCommand.Parameters["@BeneficiarioID"].Value = BeneficiarioID;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Pagos_GetDetallesFacturasBeneficiaro");
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
        #region  Flujo de caja

        /// <summary>
        /// Consulta para revisar el flujo de caja
        /// </summary>
        /// <returns>Lista de el flujo de caja</returns>
        public List<DTO_QueryFlujoCaja> DAL_Tesoreria_tsFlujoCaja()
        {
            try
            {
                List<DTO_QueryFlujoCaja> result = new List<DTO_QueryFlujoCaja>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char,UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.CommandText =

                "   select	Documento," +
                "   		sum(case when semanaDif < 0 then SaldoML else 0 end) as PerA, " +
                "   		sum(case when semanaDif = 0 then SaldoML else 0 end) as Per0, " +
                "   		sum(case when semanaDif = 1 then SaldoML else 0 end) as Per1, " +
                "   		sum(case when semanaDif = 2 then SaldoML else 0 end) as Per2, " +
                "   		sum(case when semanaDif = 3 then SaldoML else 0 end) as Per3, " +
                "   		sum(case when semanaDif = 4 then SaldoML else 0 end) as Per4, " +
                "   		sum(case when semanaDif = 5 then SaldoML else 0 end) as Per5, " +
                "   		sum(case when semanaDif = 6 then SaldoML else 0 end) as Per6, " +
                "   		sum(case when semanaDif > 6 then SaldoML else 0 end) as PerM " +
                "   FROM" +
                "   	(" +
                "   	select (case when cc.DocumentoID in ('21','26') then 'Desembolsos' else 'Recaudos' end) as Documento," +
                "   			cc.TerceroID, " +
                "   			ter.Descriptivo as Nombre," +
                "   			cc.Factura, " +
                "   			cc.FechaVto," +
                "   			cc.SaldoML," +
                "   			cc.SaldoME," +
                "   		DATEPART(week,cc.FechaVto) as Semana, " +
                "   		DATEPART(week,GETDATE()) as SemanaAct," +
                "           datediff(week, GETDATE(),cc.FechaVto)  as SemanaDif "+
                "   	from"+
                "   		("+
                "   		select	sdo.EmpresaID, doc.DocumentoID, sdo.TerceroID, "+
                "   				(case when doc.DocumentoID in('21','26') "+
                "   					  then doc.DocumentoTercero "+
                "   					  else rtrim(doc.PrefijoID) + ' - ' +  cast(doc.DocumentoNro as varchar(10)) end) as Factura,"+
                "   				(case when doc.DocumentoID in('21','26') "+
                "   					  then cast(cxp.VtoFecha as date)"+
                "   					  else cast(cxc.FechaVto as date) end) as FechaVto,"+
                "   				(Case when doc.DocumentoID in('21','26') then -1 else 1 end) * "+
                "   						sum(sdo.DbSaldoIniLocML+sdo.CrSaldoIniLocML+sdo.DbOrigenLocML+sdo.CrOrigenLocML) as SaldoML,"+
                "   				(Case when doc.DocumentoID in('21','26') then -1 else 1 end) *"+
                "   				sum(sdo.DbSaldoIniExtME+sdo.CrSaldoIniExtME+sdo.DbOrigenExtME+sdo.CrOrigenExtME) as SaldoME"+
                "   		from coCuentaSaldo sdo"+
                "   			left join glDOcumentoControl doc on sdo.IdentificadorTr = doc.NumeroDoc"+
                "   			left join cpCuentaXPagar	 cxp on sdo.IdentificadorTr = cxp.NumeroDoc"+
                "   			left join faFacturaDocu		 cxc on sdo.IdentificadorTr = cxc.NumeroDoc"+
                "   			inner join "+
                "   				("+
                "   				Select	sdo.CuentaID, sdo.TerceroID, sdo.IdentificadorTr, "+
                "   						max(sdo.PeriodoID) as PeriodoID"+
                "   				from coCuentaSaldo sdo"+
                "   					left join glDocumentoControl doc on sdo.IdentificadorTr = doc.NumeroDoc"+
                "   				where doc.empresaID =@EmpresaID and  doc.DocumentoID in('21','26','41', '43')"+
                "   				group by sdo.CuentaID, sdo.TerceroID, sdo.IdentificadorTr"+
                "   				) Ult on sdo.IdentificadorTr = ult.IdentificadorTr and sdo.PeriodoID = ult.PeriodoID"+
                "   		group by sdo.EmpresaID, doc.DocumentoID, sdo.TerceroID, doc.DocumentoTercero, doc.PrefijoID, doc.DocumentoNro, cxp.VtoFecha, cxc.FechaVto"+
                "   		) cc"+
                "   		LEFT JOIN coTercero ter on cc.terceroID = ter.TerceroID and ter.empresaGrupoId = @EmpresaID"+
                "   	WHERE (round(SaldoML,2) <>0 or SaldoME <> 0)" +
                "   	) det"+
                "   group by Documento"+
                "   order by Documento desc";

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    var flujo = new DTO_QueryFlujoCaja(dr);
                    //flujo.Document = index;
                    //flujo.Documento.Value = string.IsNullOrWhiteSpace(dr["Flujo"].ToString()) ? false : true;
                    result.Add(flujo);
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
        /// Consulta para revisar el flujo de caja
        /// </summary>
        /// <returns>Lista de el flujo de caja</returns>
        public List<DTO_QueryFlujoCajaDetalle> DAL_Tesoreria_tsFlujoCajaDetalle(string Documento)
        {
            try
            {
                List<DTO_QueryFlujoCajaDetalle> result = new List<DTO_QueryFlujoCajaDetalle>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@Documento", SqlDbType.Char, 15);

                mySqlCommand.CommandText =

                "   		select cc.Documento," +
                "   				cc.Tercero, " +
                "   				ter.Descriptivo as Nombre," +
                "   				cc.Factura, " +
                "   				cc.FechaVto," +
                "   				cc.SaldoML," +
                "   				cc.SaldoME," +
                "   			DATEPART(week,cc.FechaVto) as Semana, " +
                "   			DATEPART(week,GETDATE()) as SemanaAct," +
                "           datediff(week, GETDATE(),cc.FechaVto)  as SemanaDif "+
                "   		from" +
                "   			(" +
                "   			select	sdo.EmpresaID, " +
                "   			(case when doc.DocumentoID in ('21','26') then 'Desembolsos' else 'Recaudos' end) as Documento," +
                "   			 sdo.TerceroID as Tercero, " +
                "   					(case when doc.DocumentoID in('21','26') " +
                "   						  then doc.DocumentoTercero " +
                "   						  else rtrim(doc.PrefijoID) + ' - ' +  cast(doc.DocumentoNro as varchar(10)) end) as Factura," +
                "   					(case when doc.DocumentoID in('21','26') " +
                "   						  then cast(cxp.VtoFecha as date)" +
                "   						  else cast(cxc.FechaVto as date) end) as FechaVto," +
                "   					(Case when doc.DocumentoID in('21','26') then -1 else 1 end) * " +
                "   							sum(sdo.DbSaldoIniLocML+sdo.CrSaldoIniLocML+sdo.DbOrigenLocML+sdo.CrOrigenLocML) as SaldoML," +
                "   					(Case when doc.DocumentoID in('21','26') then -1 else 1 end) *" +
                "   					sum(sdo.DbSaldoIniExtME+sdo.CrSaldoIniExtME+sdo.DbOrigenExtME+sdo.CrOrigenExtME) as SaldoME" +
                "   			from coCuentaSaldo sdo" +
                "   				left join glDOcumentoControl doc on sdo.IdentificadorTr = doc.NumeroDoc" +
                "   				left join cpCuentaXPagar	 cxp on sdo.IdentificadorTr = cxp.NumeroDoc" +
                "   				left join faFacturaDocu		 cxc on sdo.IdentificadorTr = cxc.NumeroDoc" +
                "   				inner join " +
                "   					(" +
                "   					Select	sdo.CuentaID, sdo.TerceroID, sdo.IdentificadorTr, " +
						                "   	max(sdo.PeriodoID) as PeriodoID" +
                "   					from coCuentaSaldo sdo" +
                "   						left join glDocumentoControl doc on sdo.IdentificadorTr = doc.NumeroDoc" +
                "   					where doc.empresaID =@EmpresaID and  doc.DocumentoID in('21','26','41', '43')" +
                "   					group by sdo.CuentaID, sdo.TerceroID, sdo.IdentificadorTr" +
                "   					) Ult on sdo.IdentificadorTr = ult.IdentificadorTr and sdo.PeriodoID = ult.PeriodoID" +
                "   			group by sdo.EmpresaID, doc.DocumentoID, sdo.TerceroID, doc.DocumentoTercero, doc.PrefijoID, doc.DocumentoNro, cxp.VtoFecha, cxc.FechaVto" +
                "   			) cc" +
                "   			LEFT JOIN coTercero ter on cc.tercero = ter.TerceroID and ter.empresaGrupoId = @EmpresaID" +
                "   		WHERE (round(SaldoML,2) <>0 or SaldoME <> 0) and Documento=@Documento" +
                "   		order by Documento,SemanaDif,Tercero";

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Documento"].Value = Documento;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    var flujo = new DTO_QueryFlujoCajaDetalle(dr);
                    //flujo.Document = index;
                    //flujo.Documento.Value = string.IsNullOrWhiteSpace(dr["Flujo"].ToString()) ? false : true;
                    result.Add(flujo);
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
        /// Consulta para revisar la semana del flujo de caja
        /// </summary>
        /// <returns>Semana </returns>
        public string DAL_Tesoreria_Global_DiaSemana( int Semana)
        {
            try
            {
                string result = string.Empty;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                
                mySqlCommand.Parameters.Add("@Semana", SqlDbType.Int);


                mySqlCommand.CommandText =

                " select  dbo.Global_DiaSemana(@Semana) as Semana";

                mySqlCommand.Parameters["@Semana"].Value = Semana;
                
                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();


                if (dr.Read())
                {
                    result = dr["Semana"].ToString();
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Tesoreria_Global_DiaSemana");
                throw exception;

            }
        }

        /// <summary>
        /// Consulta para revisar el mes del flujo de caja
        /// </summary>
        /// <returns>Mes</returns>
        public string DAL_Tesoreria_Global_Mes(int Mes)
        {
            try
            {
                DateTime fecha = DateTime.Now.AddMonths(Mes);
                int numMes = fecha.Month;

                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(numMes);
                return nombreMes;
            }
            catch
            {
                return "Desconocido";
            }
        }

        /// <summary>
        /// Consulta para revisar el flujo de caja
        /// </summary>
        /// <returns>Lista de el flujo de caja</returns>
        public List<DTO_QueryFlujoFondos> DAL_Tesoreria_tsFlujoFondos(DateTime fechaCorte)
        {
            try
            {
                List<DTO_QueryFlujoFondos> result = new List<DTO_QueryFlujoFondos>();

                SqlCommand mySqlCommandSel = new SqlCommand("Tesoreria_FlujoFondoProyectoTarea", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoFlujoFondo", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TipoFlujoFondo"].Value = "";
                mySqlCommandSel.Parameters["@ProyectoID"].Value = "";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    string codProyecto = (dr["Proyecto"]).ToString().TrimEnd();
                    bool nuevo = true;
                    DTO_QueryFlujoFondos dto = new DTO_QueryFlujoFondos(dr);
                    List<DTO_QueryFlujoFondos> list = result.Where(x => ((DTO_QueryFlujoFondos)x).Proyecto.Value.TrimEnd().Equals(codProyecto)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_QueryFlujoFondos(dr);
                    }

                    DTO_QueryFlujoFondosDetalle dtoDet = new DTO_QueryFlujoFondosDetalle(dr);
                    
                    if (dtoDet.Documento.Value == "Recaudos")
                        dtoDet.Factor.Value=1;
                    else
                        dtoDet.Factor.Value=-1;

                      dto.Detalle.Add(dtoDet);
                    if (nuevo)
                        result.Add(dto);
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
        /// Consulta para revisar el flujo de caja
        /// </summary>
        /// <returns>Lista de el flujo de caja</returns>
        public List<DTO_QueryFlujoFondosTareas> DAL_Tesoreria_tsFlujoFondosTarea(DateTime fechaCorte, string proyecto, bool? recaudosInd)
        {
            try
            {
                List<DTO_QueryFlujoFondosTareas> result = new List<DTO_QueryFlujoFondosTareas>();

                SqlCommand mySqlCommandSel = new SqlCommand("Tesoreria_FlujoFondoProyectoTarea", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoFlujoFondo", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = proyecto;
                if (recaudosInd == null)
                    mySqlCommandSel.Parameters["@TipoFlujoFondo"].Value = "";
                else if (!recaudosInd.Value)
                    mySqlCommandSel.Parameters["@TipoFlujoFondo"].Value = "Desembolsos";
                else
                    mySqlCommandSel.Parameters["@TipoFlujoFondo"].Value = "Recaudos";
            
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    string codTarea = (dr["TareaID"]).ToString().TrimEnd();
                    bool nuevo = true;
                    DTO_QueryFlujoFondosTareas dto = new DTO_QueryFlujoFondosTareas(dr);
                    List<DTO_QueryFlujoFondosTareas> list = result.Where(x => ((DTO_QueryFlujoFondosTareas)x).TareaID.Value.TrimEnd().Equals(codTarea)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_QueryFlujoFondosTareas(dr);
                    }

                    DTO_QueryFlujoFondosDetalleTarea dtoDet = new DTO_QueryFlujoFondosDetalleTarea(dr);

                    if (dtoDet.Documento.Value == "Recaudos")
                        dtoDet.Factor.Value = 1;
                    else
                        dtoDet.Factor.Value = -1;
                    
                    dto.DetalleTarea.Add(dtoDet);

                    if (nuevo)
                        result.Add(dto);
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
