using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccIncorporacionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccIncorporacionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccIncorporacionDeta
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccIncorporacionDeta_Add(DTO_ccIncorporacionDeta incorparionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =  "INSERT INTO ccIncorporacionDeta   " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[NumDocSolicitud]  " +
                                               "    ,[NumDocCredito]  " +
                                               "    ,[CentroPagoID]  " +
                                               "    ,[PagaduriaID]  "+
                                               "    ,[FechaNovedad]  "+
                                               "    ,[FechaCuota1]  "+
                                               "    ,[ValorCuota]  "+
                                               "    ,[NumDocNomina]  "+
                                               "    ,[ValorNomina]  "+
                                               "    ,[IncPreviaInd]  " +
                                               "    ,[TipoNovedad]  " +
                                               "    ,[IndInconsistencia]  "+
                                               "    ,[TipoInconsistencia]  "+
                                               "    ,[InconsistenciaIncID]  "+
                                               "    ,[Observacion]  "+
                                               "    ,[NovedadIncorporaID]  " +
                                               "    ,[NumeroINC]  " +
                                               "    ,[PlazoINC]  " +
                                               "    ,[OrigenDato]  " +
                                               "    ,[eg_ccPagaduria]  "+
                                               "    ,[eg_ccNominaINC]  " +
                                               "    ,[eg_ccIncorporacionNovedad]  " +
                                               "    ,[eg_ccCentroPagoPAG] )  "+
                                               "VALUES    " +
                                               "    (@NumeroDoc    " +
                                               "    ,@NumDocSolicitud  " +
                                               "    ,@NumDocCredito  " +
                                               "    ,@CentroPagoID  " +
                                               "    ,@PagaduriaID  "+
                                               "    ,@FechaNovedad  "+
                                               "    ,@FechaCuota1  "+
                                               "    ,@ValorCuota  "+
                                               "    ,@NumDocNomina  "+
                                               "    ,@ValorNomina  "+
                                               "    ,@IncPreviaInd  " +
                                               "    ,@TipoNovedad  " +
                                               "    ,@IndInconsistencia  "+
                                               "    ,@TipoInconsistencia  "+
                                               "    ,@InconsistenciaIncID  "+
                                               "    ,@Observacion  "+
                                               "    ,@NovedadIncorporaID  " +
                                               "    ,@NumeroINC  " +
                                               "    ,@PlazoINC  " +
                                               "    ,@OrigenDato  " +
                                               "    ,@eg_ccPagaduria  "+
                                               "    ,@eg_ccNominaINC  " +
                                               "    ,@eg_ccIncorporacionNovedad  " +
                                               "    ,@eg_ccCentroPagoPAG) ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaNovedad", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ValorCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumDocNomina", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ValorNomina", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@IncPreviaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TipoNovedad", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IndInconsistencia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoInconsistencia", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@InconsistenciaIncID", SqlDbType.Char, UDT_InconsistenciaIncID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@NovedadIncorporaID", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@NumeroINC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PlazoINC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@OrigenDato", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_ccPagaduria", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccNominaINC", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccIncorporacionNovedad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = incorparionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocSolicitud"].Value = incorparionDeta.NumDocSolicitud.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = incorparionDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = incorparionDeta.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = incorparionDeta.PagaduriaID.Value;
                mySqlCommandSel.Parameters["@FechaNovedad"].Value = incorparionDeta.FechaNovedad.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = incorparionDeta.FechaCuota1.Value;
                mySqlCommandSel.Parameters["@ValorCuota"].Value = incorparionDeta.ValorCuota.Value;
                mySqlCommandSel.Parameters["@NumDocNomina"].Value = incorparionDeta.NumDocNomina.Value;
                mySqlCommandSel.Parameters["@ValorNomina"].Value = incorparionDeta.ValorNomina.Value;
                mySqlCommandSel.Parameters["@IncPreviaInd"].Value = incorparionDeta.IncPreviaInd.Value;
                mySqlCommandSel.Parameters["@TipoNovedad"].Value = incorparionDeta.TipoNovedad.Value;
                mySqlCommandSel.Parameters["@IndInconsistencia"].Value = incorparionDeta.IndInconsistencia.Value;
                mySqlCommandSel.Parameters["@TipoInconsistencia"].Value = incorparionDeta.TipoInconsistencia.Value;
                mySqlCommandSel.Parameters["@InconsistenciaIncID"].Value = incorparionDeta.InconsistenciaIncID.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = incorparionDeta.Observacion.Value;
                mySqlCommandSel.Parameters["@NovedadIncorporaID"].Value = incorparionDeta.NovedadIncorporaID.Value;
                mySqlCommandSel.Parameters["@NumeroINC"].Value = incorparionDeta.NumeroINC.Value;
                mySqlCommandSel.Parameters["@PlazoINC"].Value = incorparionDeta.PlazoINC.Value;
                mySqlCommandSel.Parameters["@OrigenDato"].Value = incorparionDeta.OrigenDato.Value;
                mySqlCommandSel.Parameters["@eg_ccPagaduria"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccNominaINC"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccNominaINC, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccIncorporacionNovedad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccIncorporacionNovedad, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccIncorporacionDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccIncorporacionDeta
        /// </summary>
        public void DAL_ccIncorporacionDeta_UpdateFechaTransmite(int consecutivo, DateTime fechaTransmite)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaTransmite", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;
                mySqlCommandSel.Parameters["@FechaTransmite"].Value = fechaTransmite;

                mySqlCommandSel.CommandText = "UPDATE ccIncorporacionDeta SET FechaTransmite = @FechaTransmite WHERE Consecutivo = @Consecutivo";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccIncorporacionDeta_UpdateFechaTransmite");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Rertorna los pagaduria inválidas en una fechade incorporación
        /// </summary>
        /// <returns></returns>
        public List<string> DAL_ccIncorporacionDeta_GetInvalidPagadurias(DateTime fechaIncorpora)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIncorpora", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIncorpora"].Value = fechaIncorpora;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccPagaduria, this.Empresa, egCtrl);

                #region CommandText
                mySqlCommandSel.CommandText =
                    "select distinct PagaduriaID FROM " +
                    "( " +
                    "	select PagaduriaID " +
                    "	from ccPagaduria with(nolock) " +
                    "	where EmpresaGrupoID = @EmpresaGrupoID and DiaCorte > DAY(@FechaIncorpora) " +
                    "	union " +
                    "	select PagaduriaID " +
                    "	from ccIncorporacionDeta inc with(nolock) " +
                    "		inner join glDocumentoControl ctrl with(nolock) on inc.NumeroDoc = ctrl.NumeroDoc and ctrl.estado = 3 " +
                    "	where EmpresaID = @EmpresaID and MONTH(ctrl.FechaDoc) = MONTH(@FechaIncorpora) and YEAR(ctrl.FechaDoc) = YEAR(@FechaIncorpora) " +
                    ") as q";

                #endregion

                List<string> result = new List<string>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr["PagaduriaID"].ToString());
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccIncorporacionDeta_GetInvalidPagadurias");
                throw exception;
            }
        }

        /// <summary>
        /// Rertorna los Documentos pertenecientes a una pagaduria
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccIncorporacionDeta> DAL_ccIncorporacionDeta_GetByCentroPago(string centroPago)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentropagoID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CentropagoID"].Value = centroPago;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT incorDeta.*, " +
                    "   CASE WHEN paga.CodEmpleadoInd = 1 THEN cli.EmpleadoCodigo " +
                    "   ELSE cli.ClienteID END as CodEmpleado ," +
                    "   crediDocu.Libranza as Libranza , cli.Descriptivo as NombreCliente, nov.Descriptivo as Novedad " +
                    "FROM ccIncorporacionDeta  incorDeta WITH(NOLOCK) " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = incorDeta.NumeroDoc and ctrl.estado = 3 " +
                    "   INNER JOIN ccPagaduria paga with(nolock) on paga.PagaduriaID = incorDeta.PagaduriaID " +
                    "   INNER JOIN seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +
                    "   INNER JOIN ccCreditoDocu crediDocu with(nolock) on crediDocu.NumeroDoc = incorDeta.NumDocCredito " +
                    "   INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = crediDocu.ClienteID " +
                    "   LEFT JOIN ccIncorporacionNovedad nov on inc.NovedadIncorporaID = nov.NovedadIncorporaID and inc.eg_ccIncorporacionNovedad = nov.EmpresaGrupoID " +
                    "WHERE ctrl.EmpresaID = @EmpresaID AND incorDeta.CentroPagoID = @CentroPagoID ";

                #endregion

                List<DTO_ccIncorporacionDeta> result = new List<DTO_ccIncorporacionDeta>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccIncorporacionDeta dto = new DTO_ccIncorporacionDeta(dr);
                    dto.Observacion.Value = dr["Novedad"].ToString();
                    dto.CodEmpleado.Value = dr["CodEmpleado"].ToString();
                    dto.Nombre.Value = dr["NombreCliente"].ToString();
                    dto.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccIncorporacionDeta_GetForPagaduria");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccIncorporacionDeta
        /// </summary>
        /// <param name="numDocCredito">Identificador unico del credito</param>
        /// <returns>retorna una lista de DTO_ccIncorporacionDeta</returns>
        public bool DAL_ccIncorporacionDeta_HasIncorporaciones(int numDocCredito)
        {
            try
            {
                List<DTO_ccIncorporacionDeta> result = new List<DTO_ccIncorporacionDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;

                mySqlCommand.CommandText = "SELECT COUNT(*) FROM ccIncorporacionDeta with(nolock)  " +
                                           "WHERE NumDocCredito = @NumDocCredito";

                int count = (int)mySqlCommand.ExecuteScalar();
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccIncorporacionDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccIncorporacionDeta con filtro
        /// </summary>
        /// <param name="numDocCredito">Identificador unico del credito</param>
        /// <returns>retorna una lista de DTO_ccIncorporacionDeta</returns>
        public List<DTO_ccIncorporacionDeta> DAL_ccIncorporacionDeta_GetByNumDocCred(int numDocCredito)
        {
            try
            {
                List<DTO_ccIncorporacionDeta> result = new List<DTO_ccIncorporacionDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;

                mySqlCommand.CommandText = " SELECT inc.*, ctrl.Observacion as Obs,IsNull(inc.PlazoINC, cred.plazo) as Plazo " +
                                           " FROM ccIncorporacionDeta inc with(nolock)  " +
                                           " inner join glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = inc.NumeroDoc " +
                                           " inner join ccCreditoDocu cred with(nolock) on cred.NumeroDoc = inc.NumDocCredito  " +
                                           " WHERE NumDocCredito = @NumDocCredito";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccIncorporacionDeta dto = new DTO_ccIncorporacionDeta(dr);
                    dto.Observacion.Value = dr["Obs"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                        dto.PlazoINC.Value = Convert.ToByte(dr["Plazo"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccIncorporacionDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el DTO de Incoporaciones 
        /// </summary>
        /// <param name="fechaIncorpora">Fecha que se incorporaron los Creditos</param>
        /// <param name="centroPago">Centro de Pago para las incorporaciones</param>
        /// <param name="isLiquidacion">Se revisa si es incorporacion Previa</param>
        /// <returns>Lista de Incorporaciones</returns>
        public List<DTO_ccArchivoIncorporaciones> DAL_CreditosIncorporados(DateTime periodo, string centroPago, string terceroCoop)
        {
            try
            {
                List<DTO_ccArchivoIncorporaciones> results = new List<DTO_ccArchivoIncorporaciones>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt); 
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@centroPag", SqlDbType.Char, UDT_CentroPagoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;
                mySqlCommandSel.Parameters["@FechaIni"].Value = periodo;
                mySqlCommandSel.Parameters["@FechaFin"].Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                mySqlCommandSel.Parameters["@centroPag"].Value = centroPago;

                #region CommanText

                mySqlCommandSel.CommandText =
                    "SELECT incorDeta.TipoNovedad, cre.Libranza AS LibranzaID, incorDeta.NumeroINC, cre.ClienteID, cli.Descriptivo AS Nombre, cli.EmpleadoCodigo AS CodEmpleado, " +
                    "	cli.ProfesionID, incorDeta.CentroPagoID, incorDeta.ValorNomina AS VlrIncorpora, pp.VlrCuota - pp.VlrPagadoCuota AS VlrSaldo, " + 
                    "	incorDeta.PlazoINC AS Plazo, incorDeta.FechaCuota1, cre.FechaVto, pro.TipoNominaPolicia AS CodPolicia, " + 
                    "	pag.CodIncorpora AS CodNemonico, cli.FechaIngresoPAG AS FechaAfilia, cli.FechaRetiro AS FechaDesafilia, " +
                    "	pag.IncorporaIDE, pag.DesincorporaIDE, pag.AfiliaIDE, pag.DesafiliaIDE, incorDeta.Consecutivo " +
                    "FROM ccIncorporacionDeta incorDeta WITH(NOLOCK) " +
                    "	INNER JOIN glDocumentoControl ctrlInc WITH(NOLOCK) on incorDeta.numerodoc = ctrlInc.NumeroDoc and Estado = @Estado " +
                    "	INNER JOIN ccCreditoDocu cre WITH(NOLOCK) on incorDeta.NumDocCredito = cre.NumeroDoc " + 
	                "	INNER JOIN " +
                    "	( " + 
	                "		select NumeroDoc, sum(VlrCuota) AS VlrCuota, sum(VlrPagadoCuota) AS VlrPagadoCuota from ccCreditoPlanPagos with(nolock) group by NumeroDoc " + 
	                "	) as pp on cre.NumeroDoc = pp.NumeroDoc " + 
                    "	INNER JOIN ccCliente cli WITH(NOLOCK) on cli.ClienteID = cre.ClienteID and cli.EmpresaGrupoID = cre.eg_ccCliente " +  
                    "	INNER JOIN ccProfesion pro WITH(NOLOCK) on cli.ProfesionID  = pro.ProfesionID and cli.eg_ccProfesion = pro.EmpresaGrupoID " +
                    "	INNER JOIN ccCentroPagoPAG cp WITH(NOLOCK) on incorDeta.CentroPagoID = cp.CentroPagoID and incorDeta.eg_ccCentroPagoPAG = cp.EmpresaGrupoID " +
                    "	INNER JOIN ccPagaduria pag WITH(NOLOCK) on incorDeta.PagaduriaID  = pag.PagaduriaID and incorDeta.eg_ccPagaduria = pag.EmpresaGrupoID " +
                    " WHERE cre.EmpresaID = @EmpresaID AND incorDeta.FechaNovedad BETWEEN @FechaIni AND @FechaFin AND incorDeta.CentroPagoID = @centroPag  ";

                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccArchivoIncorporaciones archInc = new DTO_ccArchivoIncorporaciones(dr);
                    archInc.TerceroCoop.Value = terceroCoop;
                    results.Add(archInc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditosIncorporados");
                throw exception;
            }
        }

        #endregion
    }

}
