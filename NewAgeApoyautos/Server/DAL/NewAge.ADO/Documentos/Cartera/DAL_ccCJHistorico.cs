using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_ccCJHistorico : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCJHistorico(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCJHistorico> DAL_ccCJHistorico_GetAll()
        {
            try
            {
                List<DTO_ccCJHistorico> result = new List<DTO_ccCJHistorico>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT historico.* , cliente.Descriptivo AS Nombre " +
                                       "FROM ccCJHistorico historico with(nolock) " +
                                       "    INNER JOIN ccCliente cliente with(nolock) ON cliente.ClienteID = historico.ClienteID ";
                                     
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCJHistorico historico;
                    historico = new DTO_ccCJHistorico(dr);
                    result.Add(historico);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistorico_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCarteraDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_ccCJHistorico_Add(DTO_ccCJHistorico historico)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
               
                #region Creacion de comandos

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocMvto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocEstadoCta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClaseDeuda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaMvto", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Dias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocPagoUltimaCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPendiente", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SaldoCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SaldoInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SaldoGastos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MvtoCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MvtoInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MvtoGastos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FijadoInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value ;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = historico.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDocMvto"].Value = historico.NumeroDocMvto.Value;
                mySqlCommandSel.Parameters["@NumDocEstadoCta"].Value = historico.NumDocEstadoCta.Value;
                mySqlCommandSel.Parameters["@ClaseDeuda"].Value = historico.ClaseDeuda.Value;
                mySqlCommandSel.Parameters["@EstadoDeuda"].Value = historico.EstadoDeuda.Value;
                mySqlCommandSel.Parameters["@TipoMvto"].Value = historico.TipoMvto.Value;
                mySqlCommandSel.Parameters["@FechaMvto"].Value = historico.FechaMvto.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = historico.FechaInicial.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = historico.FechaFinal.Value;
                mySqlCommandSel.Parameters["@Dias"].Value = historico.Dias.Value;
                mySqlCommandSel.Parameters["@PorInteres"].Value = historico.PorInteres.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = historico.CuotaID.Value;
                mySqlCommandSel.Parameters["@DocPagoUltimaCuota"].Value = historico.DocPagoUltimaCuota.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = historico.Observacion.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = historico.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrPagado"].Value = historico.VlrPagado.Value;
                mySqlCommandSel.Parameters["@VlrPendiente"].Value = historico.VlrPendiente.Value;
                mySqlCommandSel.Parameters["@SaldoCapital"].Value = historico.SaldoCapital.Value;
                mySqlCommandSel.Parameters["@SaldoInteres"].Value = historico.SaldoInteres.Value;
                mySqlCommandSel.Parameters["@SaldoGastos"].Value = historico.SaldoGastos.Value;
                mySqlCommandSel.Parameters["@MvtoCapital"].Value = historico.MvtoCapital.Value;
                mySqlCommandSel.Parameters["@MvtoInteres"].Value = historico.MvtoInteres.Value;
                mySqlCommandSel.Parameters["@MvtoGastos"].Value = historico.MvtoGastos.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = historico.PeriodoID.Value; 
                mySqlCommandSel.Parameters["@FijadoInd"].Value = historico.FijadoInd.Value;
                mySqlCommandSel.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccCJHistorico " +
                    "( " +
                    "  EmpresaID " +
                    " ,NumeroDoc " +
                    " ,NumeroDocMvto " +
                    " ,NumDocEstadoCta " +
                    " ,ClaseDeuda " +
                    " ,EstadoDeuda " +
                    " ,TipoMvto " +
                    " ,FechaMvto " +
                    " ,FechaInicial " +
                    " ,FechaFinal " +
                    " ,Dias " +
                    " ,PorInteres " +
                    " ,CuotaID " +
                    " ,DocPagoUltimaCuota " +
                    " ,Observacion " +
                    " ,VlrCuota " +
                    " ,VlrPagado " +
                    " ,VlrPendiente " +
                    " ,SaldoCapital " +
                    " ,SaldoInteres " +
                    " ,SaldoGastos " +
                    " ,MvtoCapital " +
                    " ,MvtoInteres " +
                    " ,MvtoGastos " +
                    " ,PeriodoID " +
                    " ,FijadoInd " +
                    " ,eg_coComprobante " +
                    ") " +
                    "VALUES " +
                    "( " +
                    "  @EmpresaID " +
                    " ,@NumeroDoc " +
                    " ,@NumeroDocMvto " +
                    " ,@NumDocEstadoCta " +
                    " ,@ClaseDeuda " +
                    " ,@EstadoDeuda " +
                    " ,@TipoMvto " +
                    " ,@FechaMvto " +
                    " ,@FechaInicial " +
                    " ,@FechaFInal " +
                    " ,@Dias " +
                    " ,@PorInteres " +
                    " ,@CuotaID " +
                    " ,@DocPagoUltimaCuota " +
                    " ,@Observacion " +
                    " ,@VlrCuota " +
                    " ,@VlrPagado " +
                    " ,@VlrPendiente " +
                    " ,@SaldoCapital " +
                    " ,@SaldoInteres " +
                    " ,@SaldoGastos " +
                    " ,@MvtoCapital " +
                    " ,@MvtoInteres " +
                    " ,@MvtoGastos " +
                    " ,@PeriodoID " +
                    " ,@FijadoInd " +
                    " ,@eg_coComprobante " +
                    ") " +
                " SET @Consecutivo = SCOPE_IDENTITY()";

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
                int numDoc = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);

                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistorico_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccCJHistorico_Update(DTO_ccCJHistorico historico)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocMvto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocEstadoCta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClaseDeuda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoDeuda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaMvto", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaInicial", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Dias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocPagoUltimaCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPagado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrPendiente", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SaldoCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SaldoInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@SaldoGastos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MvtoCapital", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MvtoInteres", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@MvtoGastos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FijadoInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = historico.EmpresaID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = historico.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumeroDocMvto"].Value = historico.NumeroDocMvto.Value;
                mySqlCommandSel.Parameters["@NumDocEstadoCta"].Value = historico.NumDocEstadoCta.Value;
                mySqlCommandSel.Parameters["@ClaseDeuda"].Value = historico.ClaseDeuda.Value;
                mySqlCommandSel.Parameters["@EstadoDeuda"].Value = historico.EstadoDeuda.Value;
                mySqlCommandSel.Parameters["@TipoMvto"].Value = historico.TipoMvto.Value;
                mySqlCommandSel.Parameters["@FechaMvto"].Value = historico.FechaMvto.Value;
                mySqlCommandSel.Parameters["@FechaInicial"].Value = historico.FechaInicial.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = historico.FechaFinal.Value;
                mySqlCommandSel.Parameters["@Dias"].Value = historico.Dias.Value;
                mySqlCommandSel.Parameters["@PorInteres"].Value = historico.PorInteres.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = historico.CuotaID.Value;
                mySqlCommandSel.Parameters["@DocPagoUltimaCuota"].Value = historico.DocPagoUltimaCuota.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = historico.Observacion.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = historico.VlrCuota.Value;
                mySqlCommandSel.Parameters["@VlrPagado"].Value = historico.VlrPagado.Value;
                mySqlCommandSel.Parameters["@VlrPendiente"].Value = historico.VlrPendiente.Value;
                mySqlCommandSel.Parameters["@SaldoCapital"].Value = historico.SaldoCapital.Value;
                mySqlCommandSel.Parameters["@SaldoInteres"].Value = historico.SaldoInteres.Value;
                mySqlCommandSel.Parameters["@SaldoGastos"].Value = historico.SaldoGastos.Value;
                mySqlCommandSel.Parameters["@MvtoCapital"].Value = historico.MvtoCapital.Value;
                mySqlCommandSel.Parameters["@MvtoInteres"].Value = historico.MvtoInteres.Value;
                mySqlCommandSel.Parameters["@MvtoGastos"].Value = historico.MvtoGastos.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = historico.PeriodoID.Value; 
                mySqlCommandSel.Parameters["@FijadoInd"].Value = historico.FijadoInd.Value;
                mySqlCommandSel.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "UPDATE ccCJHistorico SET" +
                    "  Empresa = @Empresa " +
                    " ,NumeroDoc = @NumeroDoc " +
                    " ,NumeroDocMvto = @NumeroDocMvto " +
                    " ,NumDocEstadoCta = @NumDocEstadoCta " +
                    " ,ClaseDeuda = @ClaseDeuda " +
                    " ,EstadoDeuda = @EstadoDeuda " +
                    " ,TipoMvto = @TipoMvto " +
                    " ,FechaMvto = @FechaMvto " +
                    " ,FechaInicial = @FechaInicial " +
                    " ,FechaFinal = @FechaFinal " +
                    " ,Dias = @Dias " +
                    " ,PorInteres = @PorInteres " +
                    " ,CuotaID = @CuotaID " +
                    " ,DocPagoUltimaCuota = @DocPagoUltimaCuota " +
                    " ,Observacion = @Observacion " +
                    " ,VlrCuota = @VlrCuota " +
                    " ,VlrPagado = @VlrPagado " +
                    " ,VlrPendiente = @VlrPendiente " +
                    " ,SaldoCapital = @SaldoCapital " +
                    " ,SaldoInteres = @SaldoInteres " +
                    " ,SaldoGastos = @SaldoGastos " +
                    " ,MvtoCapital = @MvtoCapital " +
                    " ,MvtoInteres = @MvtoInteres " +
                    " ,MvtoGastos = @MvtoGastos " +
                    " ,PeriodoID = @PeriodoID " +
                    " ,FijadoInd = @FijadoInd " +
                    " Where Consecutivo = @Consecutivo ";
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistorico_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Elimina los registro de un movimiento
        /// </summary>
        /// <param name="numDocMvto">NumeroDoc del movimiento</param>
        public void DAL_ccCJHistorico_DeleteFromNumeroDocMvto(int numDocMvto)
        {
            try
            {
                List<DTO_ccCJHistorico> result = new List<DTO_ccCJHistorico>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDocMvto", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@NumeroDocMvto"].Value = numDocMvto;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText =
                    "DELETE FROM ccCJHistorico WHERE EmpresaID = @EmpresaID and NumeroDocMvto = @NumeroDocMvto";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la info de un historico
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un historico</param>
        /// <param name="numDocCredito">Libranza</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccCJHistorico> DAL_ccCJHistorico_GetByNumDocCredito(int numDocCredito)
        {
            try
            {
                List<DTO_ccCJHistorico> result = new List<DTO_ccCJHistorico>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText =
                    "SELECT cj.*, ctrl.ComprobanteID AS CompID, ctrl.ComprobanteIDNro AS CompNro " +
                    "FROM ccCJHistorico cj with(nolock) " +
                    "   LEFT JOIN glDocumentoControl ctrl with(nolock) on cj.NumeroDocMvto = ctrl.NumeroDoc " +
                    "WHERE cj.EmpresaID = @EmpresaID and cj.NumeroDoc = @NumDocCredito and ClaseDeuda in (1,2) and TipoMvto in (1,2,3,4,5) ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCJHistorico historico = new DTO_ccCJHistorico(dr);
                    result.Add(historico);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la info de un historico
        /// </summary>
        /// <param name="numDocCredito">Numero doc del crédito</param>
        /// <returns>retorna los 2 ultimos registros del histórico de CJ</returns>
        public Tuple<DTO_ccCJHistorico, DTO_ccCJHistorico> DAL_ccCJHistorico_GetForAbono(int numDocCredito)
        {
            try
            {
                DTO_ccCJHistorico cjCapital = null;
                DTO_ccCJHistorico cjPoliza = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength); 
                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ClaseDeuda", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@NumeroDocMvto", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value; 
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommand.Parameters["@NumeroDocMvto"].Value = 0;

                string queryCapital =
                    "SELECT TOP(1) * " +
                    "FROM ccCJHistorico cj with(nolock) " +
                    "WHERE cj.EmpresaID = @EmpresaID and cj.NumeroDoc = @NumDocCredito and ClaseDeuda = @ClaseDeuda and TipoMvto in (1,3,4) " +
                    "Order by Consecutivo desc";

                string queryMora =
                    "SELECT TOP(1) * " +
                    "FROM ccCJHistorico cj with(nolock) " +
                    "WHERE EmpresaID = @EmpresaID and NumeroDoc = @NumDocCredito and NumeroDocMvto = @NumeroDocMvto and ClaseDeuda = @ClaseDeuda and TipoMvto = 2 " +
                    "Order by Consecutivo desc";

                #region Capital

                mySqlCommand.CommandText = queryCapital;
                mySqlCommand.Parameters["@ClaseDeuda"].Value = (byte)ClaseDeuda.Principal;                
                
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    cjCapital = new DTO_ccCJHistorico(dr);
                }
                dr.Close();

                #endregion
                #region Poliza

                mySqlCommand.CommandText = queryCapital; 
                mySqlCommand.Parameters["@ClaseDeuda"].Value = (byte)ClaseDeuda.Adicional;
                
                dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    cjPoliza = new DTO_ccCJHistorico(dr);
                }
                dr.Close();

                #endregion

                return new Tuple<DTO_ccCJHistorico, DTO_ccCJHistorico>(cjCapital, cjPoliza);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistorico_GetForAbono");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la info de los históricos de cobro jurídico para el cierre mensual
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCJHistorico</returns>
        public List<DTO_ccCJHistorico> DAL_ccCJHistorico_GetForCierreMensual_CJ(DateTime fechaCierre, byte claseDeuda)
        {
            try
            {
                List<DTO_ccCJHistorico> result = new List<DTO_ccCJHistorico>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaMvto", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaMvto", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaMvto"].Value = fechaCierre;
                mySqlCommand.Parameters["@ClaseDeuda"].Value = claseDeuda;

                mySqlCommand.CommandText =
                    ";with queryRowNumber as " +
                    "( " +
                    "	select ROW_NUMBER() over(partition by CJ.NumeroDoc order by consecutivo desc) as RowNum, cli.EstadoCartera, CJ.* " +
                    "	from ccCJHistorico CJ with(nolock) " +
                    "		inner join ccCreditoDocu cre with(nolock) on CJ.NumeroDoc = cre.NumeroDoc " +
                    "		inner join ccCliente cli with(nolock) on cre.ClienteID = cli.ClienteID and cre.eg_ccCliente = cli.EmpresaGrupoID " +
                    "	where CJ.EmpresaID = @EmpresaID and SaldoCapital <> 0 and FechaMvto <= @FechaMvto and CJ.ClaseDeuda = @ClaseDeuda " +
		            "       AND ((CJ.EstadoDeuda = 4 AND TipoMvto NOT IN (0,5,6)) OR (CJ.EstadoDeuda <> 4 AND TipoMvto NOT IN (0,1,5,6))) " +
                    ") " +
                    "select * from queryRowNumber where RowNum = 1 ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCJHistorico historico = new DTO_ccCJHistorico(dr);
                    historico.EstadoCarteraCliente.Value = Convert.ToByte(dr["EstadoCartera"]);
                    result.Add(historico);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraDocu_GetByID");
                throw exception;
            }
        }

        public void DAL_ccCJHistorico_RecalcularInteresCJ(DateTime fechaCorte,int libranza)
        {
            try
            {
                #region CommanText

                SqlCommand mySqlCommandSel = new SqlCommand("Cartera_RecalculaInteresCJ", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Credito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                #endregion
                #region Asignacion Valores a Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Credito"].Value = libranza;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCJHistorico_RecalcularInteresCJ");
                throw exception;
            }

        }

        #endregion
    }
}
