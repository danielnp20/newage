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
    public class DAL_ccReincorporacionDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccReincorporacionDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccReincorporacionDeta
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccReincorporacionDeta_Add(DTO_ccReincorporacionDeta incorparionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =  "INSERT INTO ccReIncorporacionDeta   " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[NumDocCredito]   " +
                                               "    ,[PeriodoNomina]   " +
                                               "    ,[CentroPagoID]   " +
                                               "    ,[NumeroINC]   " +
                                               "    ,[PlazoINC]   " +
                                               "    ,[ValorCuota]   " +
                                               "    ,[FechaCuota1]   " +
                                               "    ,[EstadoCruce]   " +
                                               "    ,[Observacion]   " +
                                               "    ,[NovedadIncorporaID]   " +
                                               "    ,[TipoNovedad]   " +
                                               "    ,[ConsIncorpora]   " +
                                               "    ,[CambioCentroPagoIND]   " +
                                               "    ,[CentroPagoModificaID]   " +
                                               "    ,[eg_ccIncorporacionNovedad]   " +
                                               "    ,[eg_ccCentroPagoPAG])   " +
                                               "VALUES    " +
                                               "    (@NumeroDoc  " +
                                               "    ,@NumDocCredito  " +
                                               "    ,@PeriodoNomina  " +
                                               "    ,@CentroPagoID  " +
                                               "    ,@NumeroINC  " +
                                               "    ,@PlazoINC  " +
                                               "    ,@ValorCuota  " +
                                               "    ,@FechaCuota1  " +
                                               "    ,@EstadoCruce  " +
                                               "    ,@Observacion  " +
                                               "    ,@NovedadIncorporaID  " +
                                               "    ,@TipoNovedad  " +
                                               "    ,@ConsIncorpora  " +
                                               "    ,@CambioCentroPagoIND  " +
                                               "    ,@CentroPagoModificaID  " +
                                               "    ,@eg_ccIncorporacionNovedad " +
                                               "    ,@eg_ccCentroPagoPAG) ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoNomina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroINC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PlazoINC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ValorCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EstadoCruce", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.VarChar);
                mySqlCommandSel.Parameters.Add("@NovedadIncorporaID", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@TipoNovedad", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ConsIncorpora", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CambioCentroPagoIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CentroPagoModificaID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccIncorporacionNovedad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = incorparionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = incorparionDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@PeriodoNomina"].Value = incorparionDeta.PeriodoNomina.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = incorparionDeta.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@NumeroINC"].Value = incorparionDeta.NumeroINC.Value;
                mySqlCommandSel.Parameters["@PlazoINC"].Value = incorparionDeta.PlazoINC.Value;
                mySqlCommandSel.Parameters["@ValorCuota"].Value = incorparionDeta.ValorCuota.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = incorparionDeta.FechaCuota1.Value;
                mySqlCommandSel.Parameters["@EstadoCruce"].Value = incorparionDeta.EstadoCruce.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = incorparionDeta.Observacion.Value;
                mySqlCommandSel.Parameters["@NovedadIncorporaID"].Value = incorparionDeta.NovedadIncorporaID.Value;
                mySqlCommandSel.Parameters["@TipoNovedad"].Value = incorparionDeta.TipoNovedad.Value;
                mySqlCommandSel.Parameters["@ConsIncorpora"].Value = incorparionDeta.ConsIncorpora.Value;
                mySqlCommandSel.Parameters["@CambioCentroPagoIND"].Value = incorparionDeta.CambioCentroPagoIND.Value;
                mySqlCommandSel.Parameters["@CentroPagoModificaID"].Value = incorparionDeta.CentroPagoModificaID.Value;
                mySqlCommandSel.Parameters["@eg_ccIncorporacionNovedad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccIncorporacionNovedad, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccReincorporacionDeta
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccReincorporacionDeta_Update(DTO_ccReincorporacionDeta incorparionDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                                           "UPDATE ccReIncorporacionDeta SET" +
                                           "    NumeroDoc = @NumeroDoc  " +
                                           "    ,NumDocCredito = @NumDocCredito  " +
                                           "    ,PeriodoNomina = @PeriodoNomina  " +
                                           "    ,CentroPagoID = @CentroPagoID  "+
                                           "    ,NumeroINC = @NumeroINC  "+
                                           "    ,PlazoINC = @PlazoINC  "+
                                           "    ,ValorCuota = @ValorCuota  " +
                                           "    ,FechaCuota1 = @FechaCuota1  " +
                                           "    ,EstadoCruce = @EstadoCruce  "+
                                           "    ,Observacion = @Observacion  "+
                                           "    ,NovedadIncorporaID = @NovedadIncorporaID  "+
                                           "    ,TipoNovedad = @TipoNovedad  " +
                                           "    ,ConsIncorpora = @ConsIncorpora  " +
                                           "    ,CambioCentroPagoIND = @CambioCentroPagoIND  " +
                                           "    ,CentroPagoModificaID = @CentroPagoModificaID  " +
                                           " WHERE  NumeroDoc = @NumeroDoc";
                #endregion                
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoNomina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char,10);
                mySqlCommandSel.Parameters.Add("@NumeroINC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PlazoINC", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ValorCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaCuota1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EstadoCruce", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.VarChar);
                mySqlCommandSel.Parameters.Add("@NovedadIncorporaID", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@TipoNovedad", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ConsIncorpora", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CambioCentroPagoIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CentroPagoModificaID", SqlDbType.Bit);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = incorparionDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = incorparionDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@PeriodoNomina"].Value = incorparionDeta.PeriodoNomina.Value;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = incorparionDeta.CentroPagoID.Value;
                mySqlCommandSel.Parameters["@NumeroINC"].Value = incorparionDeta.NumeroINC.Value;
                mySqlCommandSel.Parameters["@PlazoINC"].Value = incorparionDeta.PlazoINC.Value;
                mySqlCommandSel.Parameters["@ValorCuota"].Value = incorparionDeta.ValorCuota.Value;
                mySqlCommandSel.Parameters["@FechaCuota1"].Value = incorparionDeta.FechaCuota1.Value;
                mySqlCommandSel.Parameters["@EstadoCruce"].Value = incorparionDeta.EstadoCruce.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = incorparionDeta.Observacion.Value;
                mySqlCommandSel.Parameters["@NovedadIncorporaID"].Value = incorparionDeta.NovedadIncorporaID.Value;
                mySqlCommandSel.Parameters["@TipoNovedad"].Value = incorparionDeta.TipoNovedad.Value;
                mySqlCommandSel.Parameters["@ConsIncorpora"].Value = incorparionDeta.ConsIncorpora.Value;
                mySqlCommandSel.Parameters["@CambioCentroPagoIND"].Value = incorparionDeta.CambioCentroPagoIND.Value;
                mySqlCommandSel.Parameters["@CentroPagoModificaID"].Value = incorparionDeta.CentroPagoModificaID.Value;
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
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccReincorporacionDeta
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccReincorporacionDeta_Delete(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommandSel.CommandText ="DELETE FROM ccReIncorporacionDeta WHERE NumeroDoc = @NumeroDoc";
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la información para hacer reincorporaciones
        /// </summary>
        /// <returns></returns>
        public DTO_ccReincorporacionDeta DAL_ccReincorporacionDeta_GetByPK(int numDocCredito, DateTime periodo, string centroPagoID, string centroPagoModificaID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                DTO_ccReincorporacionDeta result = null;

                //Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoNomina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.VarChar, UDT_CentroPagoID.MaxLength);

                //Asigna los valores
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommandSel.Parameters["@PeriodoNomina"].Value = periodo;
                mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPagoID;

                string CPModQuery = string.Empty;
                if (!string.IsNullOrWhiteSpace(centroPagoModificaID))
                {
                    mySqlCommandSel.Parameters.Add("@CentroPagoModificaID", SqlDbType.VarChar, UDT_CentroPagoID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroPagoModificaID"].Value = centroPagoModificaID;
                    CPModQuery = "and CentroPagoModificaID = @CentroPagoModificaID";
                }
                else
                {
                    CPModQuery = "and CentroPagoModificaID IS NULL";
                }

                //CommandText
                mySqlCommandSel.CommandText =
                    "select * from ccReincorporacionDeta with(nolock) where NumDocCredito = @NumDocCredito and PeriodoNomina = @PeriodoNomina " +
                    "   and CentroPagoID = @CentroPagoID " + CPModQuery;

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccReincorporacionDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_GetByPK");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la información para hacer reincorporaciones
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccReincorporacionDeta> DAL_ccReincorporacionDeta_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_ccReincorporacionDeta> results = new List<DTO_ccReincorporacionDeta>();

                //Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                //CommandText
                mySqlCommandSel.CommandText = "select * from ccReincorporacionDeta with(nolock) where NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReincorporacionDeta result = new DTO_ccReincorporacionDeta(dr);
                    results.Add(result);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_GetByPK");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la información para hacer reincorporaciones
        /// </summary>
        /// <returns></returns>
        public DTO_ccReincorporacionDeta DAL_ccReincorporacionDeta_GetByConsec(int consecutivo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                DTO_ccReincorporacionDeta result = null;

                //Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;

                //CommandText
                mySqlCommandSel.CommandText = "select * from ccReincorporacionDeta with(nolock) where Consecutivo = @Consecutivo";

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccReincorporacionDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_GetByPK");
                throw exception;
            }
        }


        #endregion

        #region Otras

        /// <summary>
        /// Trae la información para hacer reincorporaciones
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccReincorporacionDeta> DAL_ccReincorporacionDeta_GetForReincorporacion(DateTime periodo, string centroPagoID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_ccReincorporacionDeta> results = new List<DTO_ccReincorporacionDeta>();
                int maxDay = DateTime.DaysInMonth(periodo.Year, periodo.Month);

                string queryCP = string.Empty;
                if(!string.IsNullOrWhiteSpace(centroPagoID))
                {
                    mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.VarChar, UDT_CentroPagoID.MaxLength);
                    mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPagoID;
                    queryCP = " and nom.CentroPagoID = @CentroPagoID ";
                }

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@FechaFin"].Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    "select " +
                    "   nom.NumeroDoc, nom_NumeroINC AS NumeroINC, nom.nom_FechaNomina AS FechaNomina, nom.nom_EstadoCruce AS EstadoCruce, nom.Libranza, " +
                    "   nom.ClienteID, cli.Descriptivo AS Nombre, cli.ProfesionID, nom.nom_PagaduriaID AS CentroPagoID, cp.Descriptivo AS CentroPagoDesc, cli.EmpleadoCodigo, " +
                    "   nom.VlrLibranza, (pp.VlrCuota - pp.VlrPagadoCuota) AS VlrSaldo, nom.VlrCuota, nom.FechaLiquida, " +
                    "   nom.CobranzaEstadoID, nom.CobranzaGestionID, nom.SiniestroEstadoID, nom.NovedadIncorporaID, nom.Plazo, nom.FechaCuota1, " +
                    "   case when coalesce(ctrlRecaudo.FechaDoc, '20100101') > coalesce(ctrlNomina.FechaDoc, '20100101') then ctrlRecaudo.FechaDoc else ctrlNomina.FechaDoc end AS FechaDoc, " +
                    "   case when coalesce(ctrlRecaudo.FechaDoc, '20100101') > coalesce(ctrlNomina.FechaDoc, '20100101') then 'Rec Manual' else 'Mig Nómina' end AS TipoMvto, " +
                    "   case when coalesce(ctrlRecaudo.FechaDoc, '20100101') > coalesce(ctrlNomina.FechaDoc, '20100101') then rcRec.FechaAplica else rcNom.FechaAplica end AS FechaNomina, " +
                    "   case when coalesce(ctrlRecaudo.FechaDoc, '20100101') > coalesce(ctrlNomina.FechaDoc, '20100101') then ctrlRecaudo.Valor else ctrlNomina.Valor end AS Valor " +
                    "from " +
                    "( " +
                    "	select cre.*, case when nom.EstadoCruce = 6 then coalesce(cre.NumeroINC, 0) + 1 else 1 end as nom_NumeroINC, " +
                    "		nom.FechaNomina AS nom_FechaNomina, nom.EstadoCruce as nom_EstadoCruce, nom.PagaduriaID as nom_PagaduriaID, nom.eg_ccPagaduria as nom_eg_ccPagaduria " +
                    "	from ccNominaDeta nom with(nolock) " +
                    "		inner join glDocumentoControl ctrl with(nolock) on nom.NumDocRCaja = ctrl.NumeroDoc and ctrl.PeriodoDoc = @Periodo and ctrl.estado = 3 " +
                    "		inner join ccCreditoDocu cre with(nolock) on nom.NumDocCredito = cre.NumeroDoc and cre.EmpresaID = @EmpresaID " +
                    "	where nom.EstadoCruce != 1 " + queryCP +
                    "	union " +
                    "	select cre.*, case when nom.EstadoCruce = 6 then coalesce(cre.NumeroINC, 0) + 1 else 1 end as nom_NumeroINC, " +
                    "		nom.FechaNomina AS nom_FechaNomina, nom.EstadoCruce as nom_EstadoCruce, nom.PagaduriaID as nom_PagaduriaID, nom.eg_ccPagaduria as nom_eg_ccPagaduria " +
                    "	from ccNominaDeta nom with(nolock) " +
                    "		inner join ccCreditoDocu cre with(nolock) on nom.NumDocCredito = cre.NumeroDoc and cre.EmpresaID = @EmpresaID " +
                    "	where nom.EstadoCruce != 1 and FechaNomina between @Periodo and @FechaFin " + queryCP +
                    ")as nom " +
                    "	left join ccCliente cli with(nolock) on nom.ClienteID = cli.ClienteID and nom.eg_ccCliente = cli.EmpresaGrupoID " +
                    "	left join ccPagaduria cp with(nolock) on nom.nom_PagaduriaID = cp.PagaduriaID and nom.nom_eg_ccPagaduria = cp.EmpresaGrupoID " +
                    "	inner join  " +
                    "	( " +
                    "		select NumeroDoc, sum(VlrCuota) AS VlrCuota, sum(VlrPagadoCuota) AS VlrPagadoCuota from ccCreditoPlanPagos with(nolock) group by NumeroDoc " +
                    "	) as pp on nom.NumeroDoc = pp.NumeroDoc " +
                    "	left join glDocumentoControl ctrlRecaudo with(nolock) on nom.DocUltRecaudo = ctrlRecaudo.NumeroDoc " +
                    "	left join tsReciboCajaDocu rcRec with(nolock) on ctrlRecaudo.NumeroDoc = rcRec.NumeroDoc " +
                    "	left join glDocumentoControl ctrlNomina with(nolock) on nom.DocUltNomina = ctrlNomina.NumeroDoc " +
                    "	left join tsReciboCajaDocu rcNom with(nolock) on ctrlNomina.NumeroDoc = rcNom.NumeroDoc " +
                    "order by numerodoc, nom.nom_PagaduriaID desc, CAST(Libranza AS INTEGER) ";

                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReincorporacionDeta dto = new DTO_ccReincorporacionDeta(dr, true);
                    dto.Aprobado.Value = false;
                    dto.Extra.Value = false;
                    dto.CambioCentroPagoIND.Value = false;
                    dto.TipoNovedad.Value = 0;
                    dto.NumeroINC.Value = 0;

                    results.Add(dto);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReincorporacionDeta_GetForReincorporacion");
                throw exception;
            }
        }

        #endregion
    }

}
