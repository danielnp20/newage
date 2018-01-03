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
    public class DAL_ccPolizaEstado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccPolizaEstado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public string DAL_ccPolizaEstado_Add(DTO_ccPolizaEstado poliza)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
               
                #region Query 
                mySqlCommandSel.CommandText =
                    "INSERT INTO dbo.ccPolizaEstado " +
                    "(EmpresaID " +
                    ",TerceroID " +
                    ",Poliza " +
                    ",AseguradoraID " +
                    ",SegurosAsesorID " +
                    ",FinanciadaIND " +
                    ",ColectivaInd " +
                    ",AnuladaIND " +
                    ",EstadoPoliza " +
                    ",FechaLiqSeguro " +
                    ",FechaPagoSeguro " +
                    ",FechaVigenciaINI " +
                    ",FechaVigenciaFIN " +
                    ",NumDocSolicitud " +
                    ",NumDocCredito " +
                    ",NumeroDocLiquida " +
                    ",VlrPoliza " +
                    ",ValorFinancia " +
                    ",PlazoFinancia " +
                    ",Cuota1Financia " +
                    ",VlrCuotaFinancia " +
                    ",TasaFinancia " +
                    ",NumeroDocPago " +
                    ",NCRevoca " +
                    ",FechaRevoca " +
                    ",ValorRevoca " +
                    ",NumDocPagoRevoca " +
                    ",NumDocEstCuenta " +
                    ",NumDocRevoca " +
                    ",TipoEspecial" +
                    ",ComponenteCarteraID" +
                    ",eg_ccCarteraComponente" +
                    ",eg_coTercero " +
                    ",eg_ccAseguradora " +
                    ",eg_ccSegurosAsesor"+
                    ")"+
                    "VALUES " +
                    "(@EmpresaID " +
                    ",@TerceroID " +
                    ",@Poliza " +
                    ",@AseguradoraID " +
                    ",@SegurosAsesorID " +
                    ",@FinanciadaIND " +
                    ",@ColectivaInd " +
                    ",@AnuladaIND " +
                    ",@EstadoPoliza " +
                    ",@FechaLiqSeguro " +
                    ",@FechaPagoSeguro " +
                    ",@FechaVigenciaINI " +
                    ",@FechaVigenciaFIN " +
                    ",@NumDocSolicitud " +
                    ",@NumDocCredito " +
                    ",@NumeroDocLiquida " +
                    ",@VlrPoliza " +
                    ",@ValorFinancia " +
                    ",@PlazoFinancia " +
                    ",@Cuota1Financia " +
                    ",@VlrCuotaFinancia " +
                    ",@TasaFinancia " +
                    ",@NumeroDocPago " +
                    ",@NCRevoca " +
                    ",@FechaRevoca " +
                    ",@ValorRevoca " +
                    ",@NumDocPagoRevoca " +
                    ",@NumDocEstCuenta " +
                    ",@NumDocRevoca " +
                    ",@TipoEspecial" +
                    ",@ComponenteCarteraID" +
                    ",@eg_ccCarteraComponente" +
                    ",@eg_coTercero " +
                    ",@eg_ccAseguradora"+
                    ",@eg_ccSegurosAsesor)" +
                    " SET @Consecutivo = SCOPE_IDENTITY()";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@SegurosAsesorID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@FinanciadaIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ColectivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AnuladaIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EstadoPoliza", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaLiqSeguro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaPagoSeguro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaINI", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaFIN", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumDocSolicitud", SqlDbType.Int); 
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocLiquida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorFinancia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PlazoFinancia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Cuota1Financia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotaFinancia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaFinancia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumeroDocPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NCRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaRevoca", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorRevoca", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumDocPagoRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocEstCuenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoEspecial", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccAseguradora", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccSegurosAsesor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int, 1);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = poliza.TerceroID.Value;
                mySqlCommandSel.Parameters["@Poliza"].Value = poliza.Poliza.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = poliza.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@SegurosAsesorID"].Value = poliza.SegurosAsesorID.Value;
                mySqlCommandSel.Parameters["@FinanciadaIND"].Value = poliza.FinanciadaIND.Value;
                mySqlCommandSel.Parameters["@ColectivaInd"].Value = poliza.ColectivaInd.Value;
                mySqlCommandSel.Parameters["@AnuladaIND"].Value = poliza.AnuladaIND.Value;
                mySqlCommandSel.Parameters["@EstadoPoliza"].Value = poliza.EstadoPoliza.Value;
                mySqlCommandSel.Parameters["@FechaLiqSeguro"].Value = poliza.FechaLiqSeguro.Value;
                mySqlCommandSel.Parameters["@FechaPagoSeguro"].Value = poliza.FechaPagoSeguro.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaINI"].Value = poliza.FechaVigenciaINI.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaFIN"].Value = poliza.FechaVigenciaFIN.Value;
                mySqlCommandSel.Parameters["@NumDocSolicitud"].Value = poliza.NumDocSolicitud.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = poliza.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumeroDocLiquida"].Value = poliza.NumeroDocLiquida.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = poliza.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@ValorFinancia"].Value = poliza.ValorFinancia.Value;
                mySqlCommandSel.Parameters["@PlazoFinancia"].Value = poliza.PlazoFinancia.Value;
                mySqlCommandSel.Parameters["@Cuota1Financia"].Value = poliza.Cuota1Financia.Value;
                mySqlCommandSel.Parameters["@VlrCuotaFinancia"].Value = poliza.VlrCuotaFinancia.Value;
                mySqlCommandSel.Parameters["@TasaFinancia"].Value = poliza.TasaFinancia.Value;
                mySqlCommandSel.Parameters["@NumeroDocPago"].Value = poliza.NumeroDocPago.Value;
                mySqlCommandSel.Parameters["@NCRevoca"].Value = poliza.NCRevoca.Value;
                mySqlCommandSel.Parameters["@FechaRevoca"].Value = poliza.FechaRevoca.Value;
                mySqlCommandSel.Parameters["@ValorRevoca"].Value = poliza.ValorRevoca.Value;
                mySqlCommandSel.Parameters["@NumDocPagoRevoca"].Value = poliza.NumDocPagoRevoca.Value;
                mySqlCommandSel.Parameters["@NumDocEstCuenta"].Value = poliza.NumDocEstCuenta.Value;
                mySqlCommandSel.Parameters["@NumDocRevoca"].Value = poliza.NumDocRevoca.Value;
                mySqlCommandSel.Parameters["@TipoEspecial"].Value = poliza.TipoEspecial.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = poliza.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccAseguradora"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAseguradora, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccSegurosAsesor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccSegurosAsesor, this.Empresa, egCtrl);

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
                return  mySqlCommandSel.Parameters["@Consecutivo"].Value.ToString();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccPolizaEstado_Upd(DTO_ccPolizaEstado poliza)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "UPDATE ccPolizaEstado  Set " +
                    "TerceroID  = @TerceroID " +
                    ",Poliza  = @Poliza " +
                    ",AseguradoraID = @AseguradoraID  " +
                    ",SegurosAsesorID = @SegurosAsesorID  " +
                    ",FinanciadaIND = @FinanciadaIND  " +
                    ",ColectivaInd = @ColectivaInd  " +
                    ",AnuladaIND = @AnuladaIND  " +
                    ",EstadoPoliza = @EstadoPoliza " +
                    ",FechaLiqSeguro = @FechaLiqSeguro " +
                    ",FechaPagoSeguro = @FechaPagoSeguro " +
                    ",FechaVigenciaINI = @FechaVigenciaINI " +
                    ",FechaVigenciaFIN = @FechaVigenciaFIN " +
                    ",NumDocSolicitud = @NumDocSolicitud " +
                    ",NumDocCredito = @NumDocCredito " +
                    ",NumeroDocLiquida = @NumeroDocLiquida " +
                    ",VlrPoliza = @VlrPoliza " +
                    ",ValorFinancia = @ValorFinancia " +
                    ",PlazoFinancia = @PlazoFinancia " +
                    ",Cuota1Financia = @Cuota1Financia " +
                    ",VlrCuotaFinancia = @VlrCuotaFinancia " +
                    ",TasaFinancia = @TasaFinancia " +
                    ",NumeroDocPago = @NumeroDocPago " +
                    ",NCRevoca = @NCRevoca " +
                    ",FechaRevoca = @FechaRevoca " +
                    ",ValorRevoca = @ValorRevoca " +
                    ",NumDocPagoRevoca = @NumDocPagoRevoca " +
                    ",NumDocEstCuenta = @NumDocEstCuenta " +
                    ",NumDocRevoca = @NumDocRevoca " +
                    ",TipoEspecial=@TipoEspecial " +
                    ",ComponenteCarteraID=@ComponenteCarteraID " +
                    " WHERE Consecutivo = @Consecutivo";
                
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@AseguradoraID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@SegurosAsesorID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@FinanciadaIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ColectivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@AnuladaIND", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@EstadoPoliza", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FechaLiqSeguro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaPagoSeguro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaINI", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaVigenciaFIN", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@NumDocSolicitud", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocLiquida", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrPoliza", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorFinancia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PlazoFinancia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Cuota1Financia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuotaFinancia", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaFinancia", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumeroDocPago", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NCRevoca", SqlDbType.Char,20);
                mySqlCommandSel.Parameters.Add("@FechaRevoca", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ValorRevoca", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@NumDocPagoRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocEstCuenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRevoca", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoEspecial", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@TerceroID"].Value = poliza.TerceroID.Value;
                mySqlCommandSel.Parameters["@Poliza"].Value = poliza.Poliza.Value;
                mySqlCommandSel.Parameters["@AseguradoraID"].Value = poliza.AseguradoraID.Value;
                mySqlCommandSel.Parameters["@SegurosAsesorID"].Value = poliza.SegurosAsesorID.Value;
                mySqlCommandSel.Parameters["@FinanciadaIND"].Value = poliza.FinanciadaIND.Value;
                mySqlCommandSel.Parameters["@ColectivaInd"].Value = poliza.ColectivaInd.Value;
                mySqlCommandSel.Parameters["@AnuladaIND"].Value = poliza.AnuladaIND.Value;
                mySqlCommandSel.Parameters["@EstadoPoliza"].Value = poliza.EstadoPoliza.Value;
                mySqlCommandSel.Parameters["@FechaLiqSeguro"].Value = poliza.FechaLiqSeguro.Value;
                mySqlCommandSel.Parameters["@FechaPagoSeguro"].Value = poliza.FechaPagoSeguro.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaINI"].Value = poliza.FechaVigenciaINI.Value;
                mySqlCommandSel.Parameters["@FechaVigenciaFIN"].Value = poliza.FechaVigenciaFIN.Value;
                mySqlCommandSel.Parameters["@NumDocSolicitud"].Value = poliza.NumDocSolicitud.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = poliza.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumeroDocLiquida"].Value = poliza.NumeroDocLiquida.Value;
                mySqlCommandSel.Parameters["@VlrPoliza"].Value = poliza.VlrPoliza.Value;
                mySqlCommandSel.Parameters["@ValorFinancia"].Value = poliza.ValorFinancia.Value;
                mySqlCommandSel.Parameters["@PlazoFinancia"].Value = poliza.PlazoFinancia.Value;
                mySqlCommandSel.Parameters["@Cuota1Financia"].Value = poliza.Cuota1Financia.Value;
                mySqlCommandSel.Parameters["@VlrCuotaFinancia"].Value = poliza.VlrCuotaFinancia.Value;
                mySqlCommandSel.Parameters["@TasaFinancia"].Value = poliza.TasaFinancia.Value;
                mySqlCommandSel.Parameters["@NumeroDocPago"].Value = poliza.NumeroDocPago.Value;
                mySqlCommandSel.Parameters["@NCRevoca"].Value = poliza.NCRevoca.Value;
                mySqlCommandSel.Parameters["@FechaRevoca"].Value = poliza.FechaRevoca.Value;
                mySqlCommandSel.Parameters["@ValorRevoca"].Value = poliza.ValorRevoca.Value;
                mySqlCommandSel.Parameters["@NumDocPagoRevoca"].Value = poliza.NumDocPagoRevoca.Value;
                mySqlCommandSel.Parameters["@NumDocEstCuenta"].Value = poliza.NumDocEstCuenta.Value;
                mySqlCommandSel.Parameters["@NumDocRevoca"].Value = poliza.NumDocRevoca.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = poliza.Consecutivo.Value;
                mySqlCommandSel.Parameters["@TipoEspecial"].Value = poliza.TipoEspecial.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = poliza.ComponenteCarteraID.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de las polizas por el identity
        /// </summary>
        /// <returns>Lista</returns>
        public DTO_ccPolizaEstado DAL_ccPolizaEstado_GetByConsecutivo(int consecutivo)
        {
            try
            {
                DTO_ccPolizaEstado result = null;
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;
                mySqlCommand.CommandText = "select * from ccPolizaEstado with(nolock) where Consecutivo = @Consecutivo ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccPolizaEstado(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de las polizas por el identity
        /// </summary>
        /// <returns>Lista</returns>
        public void DAL_ccPolizaEstado_Delete(string terceroID, string poliza)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;
                mySqlCommand.Parameters["@Poliza"].Value = poliza;
                mySqlCommand.CommandText = " Delete from ccPolizaEstado  " +
                                           " Where EmpresaID = @EmpresaID and TerceroID = @TerceroID and Poliza = @Poliza  ";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_Delete");
                throw exception;
            }
        }
        
        #endregion

        #region Otras

        /// <summary>
        /// Trae la informacion de las polizas con un filtro
        /// </summary>
        /// <returns>Lista</returns>
        public List<DTO_ccPolizaEstado> DAL_ccPolizaEstado_GetByParameter(DTO_ccPolizaEstado filter)
        {
            List<DTO_ccPolizaEstado> results = new List<DTO_ccPolizaEstado>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "select cred.Libranza AS LibranzaCred, sol.Libranza AS LibranzaSol, pol.*,ter.Descriptivo as Nombre " + 
                        " from ccPolizaEstado pol  with(nolock) " +
                        " left join ccCreditoDocu cred ON cred.NumeroDoc = pol.NumDocCredito  " +
                        " left join ccSolicitudDocu sol ON sol.NumeroDoc = pol.NumDocSolicitud " +
                        " left join coTercero ter with(nolock) ON ter.TerceroID = pol.TerceroID  and ter.EmpresaGrupoID = pol.eg_coTercero " +
                        "where pol.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.Poliza.Value.ToString()))
                {
                    query += "and pol.Poliza = @Poliza ";
                    mySqlCommand.Parameters.Add("@Poliza", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@Poliza"].Value = filter.Poliza.Value;
                }
                if (!string.IsNullOrEmpty(filter.Libranza.Value.ToString()))
                {
                    query += "and cred.Libranza = @Libranza ";
                    mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@Libranza"].Value = filter.Libranza.Value;
                }
                if (!string.IsNullOrEmpty(filter.Solicitud.Value.ToString()))
                {
                    query += "and sol.Solicitud = @Solicitud ";
                    mySqlCommand.Parameters.Add("@Solicitud", SqlDbType.Int);
                    mySqlCommand.Parameters["@Solicitud"].Value = filter.Solicitud.Value;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and pol.TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                }
                if (!string.IsNullOrEmpty(filter.AnuladaIND.Value.ToString()))
                {
                    query += filter.AnuladaIND.Value.Value ? "and AnuladaIND = @AnuladaIND " : "and (AnuladaIND IS NULL or AnuladaIND = @AnuladaIND) ";
                    mySqlCommand.Parameters.Add("@AnuladaIND", SqlDbType.Bit);
                    mySqlCommand.Parameters["@AnuladaIND"].Value = filter.AnuladaIND.Value;
                }
                if (!string.IsNullOrEmpty(filter.Consecutivo.Value.ToString()))
                {
                    query += "and pol.Consecutivo = @Consecutivo ";
                    mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                    mySqlCommand.Parameters["@Consecutivo"].Value = filter.Consecutivo.Value;
                }
                if (!string.IsNullOrEmpty(filter.FechaVigenciaINI.Value.ToString()))
                {
                    query += "and pol.FechaVigenciaINI = @FechaVigenciaINI ";
                    mySqlCommand.Parameters.Add("@FechaVigenciaINI", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaVigenciaINI"].Value = filter.FechaVigenciaINI.Value;
                }
                if (!string.IsNullOrEmpty(filter.FechaVigenciaFIN.Value.ToString()))
                {
                    query += "and pol.FechaVigenciaFIN = @FechaVigenciaFIN ";
                    mySqlCommand.Parameters.Add("@FechaVigenciaFIN", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaVigenciaFIN"].Value = filter.FechaVigenciaFIN.Value;
                }                

                mySqlCommand.CommandText = query;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccPolizaEstado poliza = new DTO_ccPolizaEstado(dr, true);
                    poliza.Nombre.Value = dr["Nombre"].ToString();
                    results.Add(poliza);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de las polizas con un filtro
        /// </summary>
        /// <returns>Lista</returns>
        public DTO_ccPolizaEstado DAL_ccPolizaEstado_GetForSolicitud(string terceroId, int numDocSolicitud)
        {
            DTO_ccPolizaEstado result = null;

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@NumDocSolicitud", SqlDbType.Int);
                //mySqlCommand.Parameters.Add("@FinanciadaIND", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@AnuladaIND", SqlDbType.Bit);
              
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroId;
                mySqlCommand.Parameters["@NumDocSolicitud"].Value = numDocSolicitud;
                //mySqlCommand.Parameters["@FinanciadaIND"].Value = true;
                mySqlCommand.Parameters["@AnuladaIND"].Value = false;

                mySqlCommand.CommandText = 
                    "select top(1) * from ccPolizaEstado with(nolock) " +
                    "where EmpresaID = @EmpresaID and TerceroID = @TerceroID and (AnuladaIND IS NULL or AnuladaIND = @AnuladaIND)" + //and FinanciadaIND = @FinanciadaIND
                    "   and (NumDocSolicitud = @NumDocSolicitud)"; 

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_ccPolizaEstado(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_GetForSolicitud");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la ultima poliza de un crédito en cobro jurídico
        /// </summary>
        /// <param name="numDocCredito">Identificador único del crédito</param>
        /// <returns>Información de la póliza</returns>
        public DTO_ccPolizaEstado DAL_ccPolizaEstado_GetLastPoliza(int numDocCredito)
        {
            DTO_ccPolizaEstado result = null;

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;

                mySqlCommand.CommandText = "select top(1) * from ccPolizaEstado with(nolock) " +
                        "where EmpresaID = @EmpresaID and NumDocCredito = @NumDocCredito and (AnuladaIND IS NULL or AnuladaIND = 0) " +
                        "order by Consecutivo desc";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_ccPolizaEstado(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_GetForSolicitud");
                throw exception;
            }
        }

        /// <summary>
        /// Retorna el Listado de las polizas para pagar
        /// </summary>
        /// <returns></returns>     
        public List<DTO_ccPolizaEstado> DAL_ccPolizaEstado_GetForPagos()
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                #endregion
                #region Asignacion Campos
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion
                #region CommandText
                mySqlCommandSel.CommandText =
                    " SELECT POL.*, ter.Descriptivo as Nombre  " +
                    " FROM ccPolizaEstado POL with(nolock)   " +
                    "    Left join coTercero ter   with(nolock) on ter.TerceroID = pol.TerceroID  and ter.EmpresaGrupoID = pol.eg_coTercero     " +
                    "WHERE pol.EmpresaID = @EmpresaID and pol.VlrPoliza <> 0 AND   " +
                    "  pol.EstadoPoliza <> 4 and pol.EstadoPoliza <>5 and (pol.AnuladaIND IS NULL or pol.AnuladaIND = 0)  and   " +
                    "  ((pol.NumeroDocPago  is null ) OR (pol.NumDocPagoRevoca  is null and pol.NumDocRevoca     is not null and POL.ValorRevoca <>0))  " +
                    "ORDER BY ter.descriptivo  ";

                #endregion

                List<DTO_ccPolizaEstado> result = new List<DTO_ccPolizaEstado>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccPolizaEstado dto = new DTO_ccPolizaEstado(dr);
                    dto.PagarInd.Value = false;
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    //dto.VlrPoliza.Value = dto.NumDocRevoca.Value.HasValue && dto.ValorRevoca.Value.HasValue ? Convert.ToDecimal(dr["ValorRevoca"]) * -1 : dto.VlrPoliza.Value;
                    dto.VlrPoliza.Value = !dto.NumeroDocPago.Value.HasValue || dto.NumeroDocPago.Value == 0 ? dto.VlrPoliza.Value : 0;
                    dto.ValorRevoca.Value = !dto.NumDocPagoRevoca.Value.HasValue || dto.NumDocPagoRevoca.Value == 0 ? dto.ValorRevoca.Value : 0;
                    if (!dto.ValorRevoca.Value.HasValue) dto.ValorRevoca.Value = 0;
                    if (!dto.VlrPoliza.Value.HasValue) dto.VlrPoliza.Value = 0;
                    if (dto.ValorRevoca.Value != 0)
                        dto.VlrPoliza.Value = dto.ValorRevoca.Value *-1;
                    dto.FechaLiqSeguro.Value = dto.NumDocRevoca.Value.HasValue && dto.FechaRevoca.Value.HasValue ? Convert.ToDateTime(dr["FechaRevoca"]) : dto.FechaLiqSeguro.Value;
                    dto.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccPolizaEstado_GetForPagos");
                throw exception;
            }
        }

        #endregion

    }
}
