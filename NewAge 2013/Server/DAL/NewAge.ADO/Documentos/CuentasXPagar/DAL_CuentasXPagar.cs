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
    /// DAL Cuenta X Pagar
    /// </summary>
    public class DAL_CuentasXPagar : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_CuentasXPagar(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Consulta una cuenta por pagar segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_cpCuentaXPagar DAL_CuentasXPagar_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from cpCuentaXPagar with(nolock) where cpCuentaXPagar.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_cpCuentaXPagar result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_cpCuentaXPagar(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla cpCuentaXPagar 
        /// </summary>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public void DAL_CuentasXPagar_Add(DTO_cpCuentaXPagar cta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                //    string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO [cpCuentaXPagar] " +
                                           "    ([EmpresaID]    " +
                                           "    ,[NumeroDoc]    " +
                                           "    ,[NumeroDocPadre]    " +
                                           "    ,[RadicaFecha]    " +
                                           "    ,[ConceptoCxPID]    " +
                                           "    ,[Valor]    " +
                                           "    ,[Iva]    " +
                                           "    ,[MonedaPago]    " +
                                           "    ,[FacturaFecha]    " +
                                           "    ,[VtoFecha]    " +
                                           "    ,[ContabFecha]    " +
                                           "    ,[ProvisionInd]    " +
                                           "    ,[PeriComprRevierteProv]    " +
                                           "    ,[TipoComprRevierteProv]    " +
                                           "    ,[NumeComprRevierteProv]    " +
                                           "    ,[CausalDevID]    " +
                                           "    ,[DistribuyeImpLocalInd]    " +
                                           "    ,[RadicaCodigo]    " +
                                           "    ,[NumeroRadica]    " +
                                           "    ,[FactEquivalente]    " +
                                           "    ,[ValorLocal]    " +
                                           "    ,[ValorExtra]    " +
                                           "    ,[PagoInd]    " +
                                           "    ,[PagoAprobacionInd]    " +
                                           "    ,[ValorPago]    " +
                                           "    ,[BancoCuentaID]    " +
                                           "    ,[Dato1]    " +
                                           "    ,[Dato2]    " +
                                           "    ,[Dato3]    " +
                                           "    ,[Dato4]    " +
                                           "    ,[Dato5]    " +
                                           "    ,[Dato6]    " +
                                           "    ,[Dato7]    " +
                                           "    ,[Dato8]    " +
                                           "    ,[Dato9]    " +
                                           "    ,[Dato10]    " +
                                           "    ,[ValorTercero]    " +
                                           "    ,[TerceroID]    " +
                                           "    ,[eg_cpConceptoCXP]    " +
                                           "    ,[eg_coTercero]    " +
                                           "    ,[eg_tsBancosCuenta])   " +
                                           "    VALUES" +
                                           "    (@EmpresaID" +
                                           "    ,@NumeroDoc    " +
                                           "    ,@NumeroDocPadre " +
                                           "    ,@RadicaFecha    " +
                                           "    ,@ConceptoCxPID    " +
                                           "    ,@Valor    " +
                                           "    ,@Iva    " +
                                           "    ,@MonedaPago   " +
                                           "    ,@FacturaFecha    " +
                                           "    ,@VtoFecha   " +
                                           "    ,@ContabFecha    " +
                                           "    ,@ProvisionInd   " +
                                           "    ,@PeriComprRevierteProv   " +
                                           "    ,@TipoComprRevierteProv    " +
                                           "    ,@NumeComprRevierteProv    " +
                                           "    ,@CausalDevID   " +
                                           "    ,@DistribuyeImpLocalInd    " +
                                           "    ,@RadicaCodigo   " +
                                           "    ,@NumeroRadica  " +
                                           "    ,@FactEquivalente  " +
                                           "    ,@ValorLocal  " +
                                           "    ,@ValorExtra  " +
                                           "    ,@PagoInd  " +
                                           "    ,@PagoAprobacionInd  " +
                                           "    ,@ValorPago  " +
                                           "    ,@BancoCuentaID  " +
                                           "    ,@Dato1  " +
                                           "    ,@Dato2  " +
                                           "    ,@Dato3    " +
                                           "    ,@Dato4    " +
                                           "    ,@Dato5   " +
                                           "    ,@Dato6    " +
                                           "    ,@Dato7    " +
                                           "    ,@Dato8   " +
                                           "    ,@Dato9    " +
                                           "    ,@Dato10   " +
                                           "    ,@ValorTercero    " +
                                           "    ,@TerceroID    " +
                                           "    ,@eg_cpConceptoCXP   " +
                                           "    ,@eg_coTercero   " +
                                           "    ,@eg_tsBancosCuenta)    " +
                                           "    SET @NumeroDoc = SCOPE_IDENTITY()";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocPadre", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@RadicaFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ConceptoCxPID", SqlDbType.Char, UDT_ConceptoCxPID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@FacturaFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@VtoFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ContabFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ProvisionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PeriComprRevierteProv", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TipoComprRevierteProv", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeComprRevierteProv", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CausalDevID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DistribuyeImpLocalInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@RadicaCodigo", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@NumeroRadica", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FactEquivalente", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ValorLocal", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorExtra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PagoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PagoAprobacionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, UDT_BancoCuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato4", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato5", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato6", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato7", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato8", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato9", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato10", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@ValorTercero", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_cpConceptoCXP", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_tsBancosCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = cta.NumeroDoc.Value;
                mySqlCommand.Parameters["@NumeroDocPadre"].Value = cta.NumeroDocPadre.Value;
                mySqlCommand.Parameters["@RadicaFecha"].Value = DateTime.Now;
                mySqlCommand.Parameters["@ConceptoCxPID"].Value = cta.ConceptoCxPID.Value;
                mySqlCommand.Parameters["@Valor"].Value = cta.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = cta.IVA.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = cta.MonedaPago.Value;
                mySqlCommand.Parameters["@FacturaFecha"].Value = cta.FacturaFecha.Value;
                mySqlCommand.Parameters["@VtoFecha"].Value = cta.VtoFecha.Value;
                mySqlCommand.Parameters["@ContabFecha"].Value = cta.ContabFecha.Value;
                mySqlCommand.Parameters["@ProvisionInd"].Value = cta.ProvisionInd.Value;
                mySqlCommand.Parameters["@PeriComprRevierteProv"].Value = cta.PeriComprRevierteProv.Value;
                mySqlCommand.Parameters["@TipoComprRevierteProv"].Value = cta.TipoComprRevierteProv.Value;
                mySqlCommand.Parameters["@NumeComprRevierteProv"].Value = cta.NumeComprRevierteProv.Value;
                mySqlCommand.Parameters["@CausalDevID"].Value = cta.CausalDevID.Value;
                mySqlCommand.Parameters["@DistribuyeImpLocalInd"].Value = cta.DistribuyeImpLocalInd.Value;
                mySqlCommand.Parameters["@RadicaCodigo"].Value = cta.RadicaCodigo.Value;
                mySqlCommand.Parameters["@NumeroRadica"].Value = cta.NumeroRadica.Value;
                mySqlCommand.Parameters["@FactEquivalente"].Value = cta.FacturaEquivalente.Value;
                mySqlCommand.Parameters["@ValorLocal"].Value = cta.ValorLocal.Value;
                mySqlCommand.Parameters["@ValorExtra"].Value = cta.ValorExtra.Value;
                mySqlCommand.Parameters["@PagoInd"].Value = cta.PagoInd.Value;
                mySqlCommand.Parameters["@PagoAprobacionInd"].Value = cta.PagoAprobacionInd.Value;
                mySqlCommand.Parameters["@ValorPago"].Value = cta.ValorPago.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = cta.BancoCuentaID.Value;
                mySqlCommand.Parameters["@Dato1"].Value = cta.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = cta.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = cta.Dato3.Value;
                mySqlCommand.Parameters["@Dato4"].Value = cta.Dato4.Value;
                mySqlCommand.Parameters["@Dato5"].Value = cta.Dato5.Value;
                mySqlCommand.Parameters["@Dato6"].Value = cta.Dato6.Value;
                mySqlCommand.Parameters["@Dato7"].Value = cta.Dato7.Value;
                mySqlCommand.Parameters["@Dato8"].Value = cta.Dato8.Value;
                mySqlCommand.Parameters["@Dato9"].Value = cta.Dato9.Value;
                mySqlCommand.Parameters["@Dato10"].Value = cta.Dato10.Value;
                mySqlCommand.Parameters["@ValorTercero"].Value = cta.ValorTercero.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = cta.TerceroID.Value;
                mySqlCommand.Parameters["@eg_cpConceptoCXP"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpConceptoCXP, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_tsBancosCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.tsBancosCuenta, this.Empresa, egCtrl);

                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar ñla Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public void DAL_CuentasXPagar_Upd(DTO_cpCuentaXPagar cta)
        {

            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                //Actualiza Tabla cpCuentaXPagar
                #region CommandText
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "    UPDATE [cpCuentaXPagar]  " +
                                           "    SET [EmpresaID] = @EmpresaID    " +
                                           "    ,[NumeroDocPadre] = @NumeroDocPadre " +
                                           "    ,[ConceptoCxPID] = @ConceptoCxPID   " +
                                           "    ,[Valor] = @Valor   " +
                                           "    ,[Iva] = @Iva   " +
                                           "    ,[MonedaPago] = @MonedaPago   " +
                                           "    ,[FacturaFecha] = @FacturaFecha     " +
                                           "    ,[VtoFecha] = @VtoFecha    " +
                                           "    ,[ContabFecha] = @ContabFecha   " +
                                           "    ,[ProvisionInd]  = @ProvisionInd " +
                                           "    ,[PeriComprRevierteProv] =  @PeriComprRevierteProv  " +
                                           "    ,[TipoComprRevierteProv]  = @TipoComprRevierteProv " +
                                           "    ,[NumeComprRevierteProv]  = @NumeComprRevierteProv " +
                                           "    ,[CausalDevID] = @CausalDevID   " +
                                           "    ,[DistribuyeImpLocalInd] = @DistribuyeImpLocalInd   " +
                                           "    ,[RadicaCodigo] = @RadicaCodigo  " +
                                           "    ,[NumeroRadica] = @NumeroRadica  " +
                                           "    ,[FactEquivalente] = @FactEquivalente  " +
                                           "    ,[ValorLocal] = @ValorLocal  " +
                                           "    ,[ValorExtra] = @ValorExtra  " +
                                           "    ,[PagoInd] = @PagoInd  " +
                                           "    ,[PagoAprobacionInd] = @PagoAprobacionInd  " +
                                           "    ,[ValorPago] = @ValorPago  " +
                                           "    ,[BancoCuentaID] = @BancoCuentaID  " +
                                           "    ,[Dato1] = @Dato1  " +
                                           "    ,[Dato2] = @Dato2  " +
                                           "    ,[Dato3] = @Dato3  " +
                                           "    ,[Dato4] = @Dato4  " +
                                           "    ,[Dato5] = @Dato5  " +
                                           "    ,[Dato6] = @Dato6  " +
                                           "    ,[Dato7] = @Dato7  " +
                                           "    ,[Dato8] = @Dato8  " +
                                           "    ,[Dato9] = @Dato9  " +
                                           "    ,[Dato10] = @Dato10 " +
                                           "    ,[ValorTercero] = @ValorTercero  " +
                                           "    ,[TerceroID] = @TerceroID  " +
                                           "    ,[eg_coTercero] = @eg_coTercero  " +
                                           "   WHERE NumeroDoc = @NumeroDoc  ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NumeroDocPadre", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConceptoCxPID", SqlDbType.Char, UDT_ConceptoCxPID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@FacturaFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@VtoFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ContabFecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ProvisionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PeriComprRevierteProv", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TipoComprRevierteProv", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeComprRevierteProv", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CausalDevID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@DistribuyeImpLocalInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@RadicaCodigo", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@NumeroRadica", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FactEquivalente", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@ValorLocal", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ValorExtra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PagoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PagoAprobacionInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ValorPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, UDT_BancoCuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@Dato1", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato2", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato3", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato4", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato5", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato6", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato7", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato8", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato9", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@Dato10", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@ValorTercero", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_cpConceptoCXP", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);


                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = cta.NumeroDoc.Value;
                mySqlCommand.Parameters["@NumeroDocPadre"].Value = cta.NumeroDocPadre.Value;
                mySqlCommand.Parameters["@ConceptoCxPID"].Value = cta.ConceptoCxPID.Value;
                mySqlCommand.Parameters["@Valor"].Value = cta.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = cta.IVA.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = cta.MonedaPago.Value;
                mySqlCommand.Parameters["@FacturaFecha"].Value = cta.FacturaFecha.Value;
                mySqlCommand.Parameters["@VtoFecha"].Value = cta.VtoFecha.Value;
                mySqlCommand.Parameters["@ContabFecha"].Value = cta.ContabFecha.Value;
                mySqlCommand.Parameters["@ProvisionInd"].Value = cta.ProvisionInd.Value;
                mySqlCommand.Parameters["@PeriComprRevierteProv"].Value = cta.PeriComprRevierteProv.Value;
                mySqlCommand.Parameters["@TipoComprRevierteProv"].Value = cta.TipoComprRevierteProv.Value;
                mySqlCommand.Parameters["@NumeComprRevierteProv"].Value = cta.NumeComprRevierteProv.Value;
                mySqlCommand.Parameters["@CausalDevID"].Value = cta.CausalDevID.Value;
                mySqlCommand.Parameters["@DistribuyeImpLocalInd"].Value = cta.DistribuyeImpLocalInd.Value;
                mySqlCommand.Parameters["@RadicaCodigo"].Value = cta.RadicaCodigo.Value;
                mySqlCommand.Parameters["@NumeroRadica"].Value = cta.NumeroRadica.Value;
                mySqlCommand.Parameters["@FactEquivalente"].Value = cta.FacturaEquivalente.Value;
                mySqlCommand.Parameters["@ValorLocal"].Value = cta.ValorLocal.Value;
                mySqlCommand.Parameters["@ValorExtra"].Value = cta.ValorExtra.Value;
                mySqlCommand.Parameters["@PagoInd"].Value = cta.PagoInd.Value;
                mySqlCommand.Parameters["@PagoAprobacionInd"].Value = cta.PagoAprobacionInd.Value;
                mySqlCommand.Parameters["@ValorPago"].Value = cta.ValorPago.Value;
                mySqlCommand.Parameters["@BancoCuentaID"].Value = cta.BancoCuentaID.Value;
                mySqlCommand.Parameters["@Dato1"].Value = cta.Dato1.Value;
                mySqlCommand.Parameters["@Dato2"].Value = cta.Dato2.Value;
                mySqlCommand.Parameters["@Dato3"].Value = cta.Dato3.Value;
                mySqlCommand.Parameters["@Dato4"].Value = cta.Dato4.Value;
                mySqlCommand.Parameters["@Dato5"].Value = cta.Dato5.Value;
                mySqlCommand.Parameters["@Dato6"].Value = cta.Dato6.Value;
                mySqlCommand.Parameters["@Dato7"].Value = cta.Dato7.Value;
                mySqlCommand.Parameters["@Dato8"].Value = cta.Dato8.Value;
                mySqlCommand.Parameters["@Dato9"].Value = cta.Dato9.Value;
                mySqlCommand.Parameters["@Dato10"].Value = cta.Dato10.Value;
                mySqlCommand.Parameters["@ValorTercero"].Value = cta.ValorTercero.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = cta.TerceroID.Value;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_cpConceptoCXP"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.cpConceptoCXP, this.Empresa, egCtrl);

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                #endregion
                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_CausacionAprobacion> DAL_CuentasXPagar_GetPendientesByModulo(int documentoID, ModulesPrefix mod, string actividadFlujoID, string usuarioID, bool checkUser)
        {
            try
            {
                List<DTO_CausacionAprobacion> result = new List<DTO_CausacionAprobacion>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string join = string.Empty;
                string where = string.Empty;
                if (documentoID != AppDocuments.RadicacionFactAprob)
                {
                    join = " inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID ";
                    where = " and perm.ActividadFlujoID = @ActividadFlujoID ";
                    mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);                  
                    mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                    mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                }
                else 
                {
                    join =  "  inner join glDocumentoAprueba docAprueba with(nolock) on docAprueba.NumeroDoc = ctrl.NumeroDoc ";                                     
                    mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Radicado;
                    if (checkUser)
                    {
                        where = "  and docAprueba.UsuarioAprueba = @UsuarioAprueba ";
                        mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Int);
                        mySqlCommand.Parameters["@UsuarioAprueba"].Value = usuarioID;                       
                    }
                }

                mySqlCommand.CommandText =
                    "select distinct ctrl.NumeroDoc, PeriodoDoc as PeriodoID, FacturaFecha, TasaCambioDOCU AS TasaCambio, ComprobanteID, ComprobanteIDNro as ComprobanteNro, ctrl.Descripcion, " +
                    "  ctrl.DocumentoTercero, ctrl.DocumentoID, DocumentoNro, ter.TerceroID, ter.Descriptivo as DescriptivoTercero, MonedaID, cxp.valor, usr.UsuarioID  " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                    "	inner join cpCuentaXPagar cxp with(nolock) on ctrl.NumeroDoc = cxp.NumeroDoc " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "	inner join coTercero ter with(nolock) on ctrl.TerceroID = ter.TerceroID  and ter.EmpresaGrupoID = @eg_coTercero	" +
                    "	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID " +  join +
                    " where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " + where;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);             
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength); 

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);

                SqlDataReader dr;

                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_CausacionAprobacion dto = new DTO_CausacionAprobacion(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Observacion.Value = string.Empty;
                    result.Add(dto);
                }
                dr.Close();

                if (documentoID != AppDocuments.RadicacionFactAprob)
                {
                    mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                    mySqlCommand.Parameters.Add("@ComprobanteNro", SqlDbType.Int);

                    foreach (DTO_CausacionAprobacion dto in result)
                    {
                        mySqlCommand.Parameters["@PeriodoID"].Value = dto.PeriodoID.Value.Value;
                        mySqlCommand.Parameters["@ComprobanteID"].Value = dto.ComprobanteID.Value;
                        mySqlCommand.Parameters["@ComprobanteNro"].Value = dto.ComprobanteNro.Value.Value;

                        mySqlCommand.CommandText =
                            "select TOP 1 Descriptivo " +
                            "from coAuxiliarPre with(nolock) " +
                            "where  " +
                            "	EmpresaID = @EmpresaID AND " +
                            "	PeriodoID = @PeriodoID AND " +
                            "	ComprobanteID = @ComprobanteID AND " +
                            "	ComprobanteNro = @ComprobanteNro ";

                        object des = mySqlCommand.ExecuteScalar();
                        dto.Descriptivo.Value = des == null ? string.Empty : des.ToString();
                    } 
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_GetPendientesByModulo");
                throw exception;
            }
        } 
        #endregion

        #region OTRAS

        /// <summary>
        /// Función que carga una lista de facrutas
        /// </summary>
        /// <param name="año">Año para filtrar</param>
        /// <param name="mes">Mes</param>
        /// <param name="terceroId">Filtro del tercero</param>
        /// <param name="factNro">Numero de factura</param>
        /// <returns>Lista de Facturas</returns>
        public List<DTO_QueryFacturas> Consultar_Facturas(int año, int mes, string terceroId, string conceptoCxP, string factNro, int tipoConsul, int? tipoFact)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_QueryFacturas> results = new List<DTO_QueryFacturas>();
                

                #region Filtros
                string where = "";

                //Carga el tipo filtro por el cual se va a consultar 
                switch (tipoConsul)
                {
                    #region Casos para el filtro
                    case 1:
                        where = " AND  ctrl.DocumentoID IN (@causarFac, @notasCredi)  ";
                        break;
                    case 2:
                        where = " AND ctrl.DocumentoID = @causarFac ";
                        break;
                    case 3:
                        where = " AND ctrl.DocumentoID = @notasCredi  ";
                        break;
                    #endregion
                }

                if (!string.IsNullOrEmpty(conceptoCxP))
                {
                    mySqlCommandSel.Parameters.Add("@ConceptoCxPID", SqlDbType.Char, UDT_ConceptoCxPID.MaxLength);
                    mySqlCommandSel.Parameters["@ConceptoCxPID"].Value = conceptoCxP;
                    where += " and cxp.ConceptoCxPID = @ConceptoCxPID ";
                }
                if (!string.IsNullOrEmpty(terceroId))
                {
                    mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommandSel.Parameters["@TerceroID"].Value = terceroId;
                    where += " and ctrl.TerceroID = @TerceroID ";
                }
                if (!string.IsNullOrEmpty(factNro))
                {
                    mySqlCommandSel.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                    mySqlCommandSel.Parameters["@DocumentoTercero"].Value = factNro;
                    where += " and ctrl.DocumentoTercero = @DocumentoTercero ";
                }

                #endregion

                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT ctrl.NumeroDoc, ctrl.MonedaID, ctrl.DocumentoTercero as numFac, ctrl.TerceroID as numIdentifica, " +
                    " tercero.Descriptivo, cxp.FacturaFecha, ctrl.Observacion, cxp.Iva,ctrl.Valor, " +
                        " (DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) * (-1) as SaldoLoc  " +
                    "  FROM coCuentaSaldo saldo  WITH(NOLOCK)   " +
                        " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = saldo.IdentificadorTR " +
                        " INNER JOIN cpCuentaXPagar AS cxp WITH(NOLOCK) ON cxp.NumeroDoc = ctrl.NumeroDoc " +
                        " INNER JOIN coTercero AS tercero  WITH(NOLOCK) ON tercero.TerceroID = ctrl.TerceroID AND  tercero.EmpresaGrupoID = ctrl.eg_coTercero " +
                    " WHERE saldo.EmpresaID = @EmpresaID AND YEAR(saldo.PeriodoID) = @año AND MONTH(saldo.PeriodoID) = @mes " + where;

                #endregion

                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@causarFac", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@notasCredi", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@mes", SqlDbType.Int);
                #endregion

                #region Asiganacion de valores a parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@causarFac"].Value = AppDocuments.CausarFacturas;
                mySqlCommandSel.Parameters["@notasCredi"].Value = AppDocuments.NotaCreditoCxP;           
                mySqlCommandSel.Parameters["@año"].Value = año;
                mySqlCommandSel.Parameters["@mes"].Value = mes;
                #endregion

                DTO_QueryFacturas doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_QueryFacturas(dr);
                    if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                        doc.Valor.Value = Convert.ToDecimal(dr["Valor"]);
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
        /// Funcion que consulta el detalle de la factura por el idTr
        /// </summary>
        /// <param name="idTr">NUmero Doc del glDoc Cntrl</param>
        /// <returns>detalle de la factura</returns>
        public List<DTO_QueryFacturasDetail> Consultar_Facturas_Detalle(int idTr, DateTime periodo)
        {
            try
            {
                List<DTO_QueryFacturasDetail> results = new List<DTO_QueryFacturasDetail>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                    " SELECT ctrl.NumeroDoc,ctrl.Estado,ctrl.DocumentoID,doc.Descriptivo as DocumentoDesc,ctrl.DocumentoTercero,  "+
                    "   aux.DocumentoCOM, aux.Fecha,aux.ComprobanteID, aux.ComprobanteNro, " +
                    "   ABS(aux.vlrMdaLoc) as vlrMdaLoc,  aux.IdentificadorTR  " +
                    " FROM coAuxiliar aux with(nolock)" +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = aux.NumeroDoc " +
                    "   INNER JOIN glDocumento AS doc WITH(NOLOCK) ON doc.DocumentoID = ctrl.DocumentoID " +
                    " WHERE IdentificadorTR = @idTr " +
                    "   AND aux.PeriodoID <= @periodo";

                mySqlCommandSel.Parameters.Add("@idTr", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@periodo", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@idTr"].Value = idTr;
                mySqlCommandSel.Parameters["@periodo"].Value = periodo;

                DTO_QueryFacturasDetail doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_QueryFacturasDetail(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Consultar_Facturas_Detalle");
                throw exception;
            }
        }

        /// <summary>
        /// Generar los consecutivos de las Facturas Equivalentes
        /// </summary>
        /// <param name="periodo">periodo</param>
        public void DAL_CuentasXPagar_ConsecutivoFactEquivalente(DateTime periodo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("CuentasXPagar_GenerarFactEquivalente", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.noConceptoNOM, this.Empresa, egCtrl);

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CuentasXPagar_ConsecutivoFactEquivalente");
                throw exception;
            }
        }

        #endregion
    }
}
