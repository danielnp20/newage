using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_glDocumentoControl
    /// </summary>
    public class DAL_glDocumentoControl : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glDocumentoControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByID(int numeroDoc)
        {
            try
            {
                DTO_glDocumentoControl result = null;
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT TOP(1) * from glDocumentoControl with(nolock) WHERE NumeroDoc = @NumeroDoc";
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_glDocumentoControl(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        public int DAL_glDocumentoControl_Add(DTO_glDocumentoControl docCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "INSERT INTO [glDocumentoControl]" +
                            "           ([EmpresaID]" +
                            "           ,[DocumentoID]" +
                            "           ,[Fecha]" +
                            "           ,[DocumentoTipo]" +
                            "           ,[PeriodoDoc]" +
                            "           ,[PeriodoUltMov]" +
                            "           ,[AreaFuncionalID]" +
                            "           ,[PrefijoID]" +
                            "           ,[DocumentoNro]" +
                            "           ,[MonedaID]" +
                            "           ,[TasaCambioDOCU]" +
                            "           ,[TasaCambioCONT]" +
                            "           ,[ComprobanteID]" +
                            "           ,[ComprobanteIDNro]" +
                            "           ,[TerceroID]" +
                            "           ,[DocumentoTercero]" +
                            "           ,[CuentaID]" +
                            "           ,[ProyectoID]" +
                            "           ,[CentroCostoID]" +
                            "           ,[LineaPresupuestoID]" +
                            "           ,[LugarGeograficoID]" +
                            "           ,[Observacion]" +
                            "           ,[Estado]" +
                            "           ,[DocumentoAnula]" +
                            "           ,[PeriodoAnula]" +
                            "           ,[ConsSaldo]" +
                            "           ,[seUsuarioID]" +
                            "           ,[FechaDoc]" +
                            "           ,[Valor]" +
                            "           ,[Iva]" +
                            "           ,[Descripcion]" +
                            "           ,[Revelacion]" +
                            "           ,[DocumentoPadre]" +
                            "           ,[eg_glAreaFuncional]" +
                            "           ,[eg_glPrefijo]" +
                            "           ,[eg_glMoneda]" +
                            "           ,[eg_coComprobante]" +
                            "           ,[eg_coTercero]" +
                            "           ,[eg_coPlanCuenta]" +
                            "           ,[eg_coProyecto]" +
                            "           ,[eg_coCentroCosto]" +
                            "           ,[eg_plLineaPresupuesto]" +
                            "           ,[eg_glLugarGeografico])" +
                            "     VALUES" +
                            "           (@EmpresaID" +
                            "           ,@DocumentoID" +
                            "           ,@Fecha" +
                            "           ,@DocumentoTipo" +
                            "           ,@PeriodoDoc" +
                            "           ,@PeriodoUltMov" +
                            "           ,@AreaFuncionalID" +
                            "           ,@PrefijoID" +
                            "           ,@DocumentoNro" +
                            "           ,@MonedaID" +
                            "           ,@TasaCambioDOCU" +
                            "           ,@TasaCambioCONT" +
                            "           ,@ComprobanteID" +
                            "           ,@ComprobanteIDNro" +
                            "           ,@TerceroID" +
                            "           ,@DocumentoTercero" +
                            "           ,@CuentaID" +
                            "           ,@ProyectoID" +
                            "           ,@CentroCostoID" +
                            "           ,@LineaPresupuestoID" +
                            "           ,@LugarGeograficoID" +
                            "           ,@Observacion" +
                            "           ,@Estado" +
                            "           ,@DocumentoAnula" +
                            "           ,@PeriodoAnula" +
                            "           ,@ConsSaldo" +
                            "           ,@seUsuarioID" +
                            "           ,@FechaDoc" +
                            "           ,@Valor" +
                            "           ,@Iva" +
                            "           ,@Descripcion" +
                            "           ,@Revelacion" +
                            "           ,@DocumentoPadre" +
                            "           ,@eg_glAreaFuncional" +
                            "           ,@eg_glPrefijo" +
                            "           ,@eg_glMoneda" +
                            "           ,@eg_coComprobante" +
                            "           ,@eg_coTercero" +
                            "           ,@eg_coPlanCuenta" +
                            "           ,@eg_coProyecto" +
                            "           ,@eg_coCentroCosto" +
                            "           ,@eg_plLineaPresupuesto" +
                            "           ,@eg_glLugarGeografico)" +
                            " SET @NumeroDoc = SCOPE_IDENTITY()";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@PeriodoUltMov", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@TasaCambioDOCU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@TasaCambioCONT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteIDNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoAnula", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ConsSaldo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@Revelacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoPadre", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glMoneda", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLugarGeografico", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int, 1);
                mySqlCommand.Parameters["@NumeroDoc"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = docCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@Fecha"].Value = DateTime.Now;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = docCtrl.DocumentoTipo.Value;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = docCtrl.PeriodoDoc.Value;
                mySqlCommand.Parameters["@PeriodoUltMov"].Value = docCtrl.PeriodoUltMov.Value;
                mySqlCommand.Parameters["@AreaFuncionalID"].Value = docCtrl.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@PrefijoID"].Value = docCtrl.PrefijoID.Value;
                mySqlCommand.Parameters["@DocumentoNro"].Value = docCtrl.DocumentoNro.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = docCtrl.MonedaID.Value;
                mySqlCommand.Parameters["@TasaCambioDOCU"].Value = docCtrl.TasaCambioDOCU.Value;
                mySqlCommand.Parameters["@TasaCambioCONT"].Value = docCtrl.TasaCambioCONT.Value;
                mySqlCommand.Parameters["@ComprobanteID"].Value = docCtrl.ComprobanteID.Value;
                mySqlCommand.Parameters["@ComprobanteIDNro"].Value = docCtrl.ComprobanteIDNro.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = docCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = docCtrl.DocumentoTercero.Value;
                mySqlCommand.Parameters["@CuentaID"].Value = docCtrl.CuentaID.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = docCtrl.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = docCtrl.CentroCostoID.Value;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = docCtrl.LineaPresupuestoID.Value;
                mySqlCommand.Parameters["@LugarGeograficoID"].Value = docCtrl.LugarGeograficoID.Value;
                mySqlCommand.Parameters["@Observacion"].Value = docCtrl.Observacion.Value;
                mySqlCommand.Parameters["@Estado"].Value = docCtrl.Estado.Value;
                mySqlCommand.Parameters["@DocumentoAnula"].Value = docCtrl.DocumentoAnula.Value;
                mySqlCommand.Parameters["@PeriodoAnula"].Value = docCtrl.PeriodoAnula.Value;
                mySqlCommand.Parameters["@ConsSaldo"].Value = docCtrl.ConsSaldo.Value;
                mySqlCommand.Parameters["@seUsuarioID"].Value = this.UserId;
                mySqlCommand.Parameters["@FechaDoc"].Value = docCtrl.FechaDoc.Value;
                mySqlCommand.Parameters["@Valor"].Value = docCtrl.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = docCtrl.Iva.Value;
                mySqlCommand.Parameters["@Descripcion"].Value = docCtrl.Descripcion.Value;
                mySqlCommand.Parameters["@Revelacion"].Value = docCtrl.Revelacion.Value;
                mySqlCommand.Parameters["@DocumentoPadre"].Value = docCtrl.DocumentoPadre.Value;
                mySqlCommand.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glMoneda"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glMoneda, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLugarGeografico"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLugarGeografico, this.Empresa, egCtrl);
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

                return numDoc;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        public void DAL_glDocumentoControl_Update(DTO_glDocumentoControl docCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "UPDATE glDocumentoControl SET" +
                    "   DocumentoID = @DocumentoID, " +
                    "   PeriodoUltMov = @PeriodoUltMov," +
                    "   AreaFuncionalID = @AreaFuncionalID," +
                    "   PrefijoID = @PrefijoID," +
                    "   DocumentoTipo = @DocumentoTipo," + 
                    "   DocumentoNro = @DocumentoNro," +
                    "   MonedaID = @MonedaID," +
                    "   TasaCambioDOCU = @TasaCambioDOCU," +
                    "   TasaCambioCONT = @TasaCambioCONT," +
                    "   ComprobanteID = @ComprobanteID," +
                    "   ComprobanteIDNro = @ComprobanteIDNro," +
                    "   TerceroID = @TerceroID," +
                    "   DocumentoTercero = @DocumentoTercero," +
                    "   CuentaID = @CuentaID," +
                    "   ProyectoID = @ProyectoID," +
                    "   CentroCostoID = @CentroCostoID," +
                    "   LineaPresupuestoID = @LineaPresupuestoID," +
                    "   LugarGeograficoID = @LugarGeograficoID," +
                    "   DocumentoAnula = @DocumentoAnula," +
                    "   PeriodoAnula = @PeriodoAnula," +
                    "   ConsSaldo = @ConsSaldo," +
                    "   Observacion = @Observacion," +
                    "   Descripcion = @Descripcion, " +
                    "   FechaDoc = @FechaDoc, " +
                    "   Valor = @Valor, " +
                    "   Iva = @Iva, " +
                    "   Revelacion = @Revelacion, " +
                    "   DocumentoPadre = @DocumentoPadre, " +
                    "   seUsuarioID = @seUsuarioID " +
                    "WHERE NumeroDoc=@NumeroDoc";
                #endregion
                #region Creacion de parametros

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoUltMov", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@AreaFuncionalID", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@TasaCambioDOCU", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@TasaCambioCONT", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteIDNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoAnula", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoAnula", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ConsSaldo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Observacion", SqlDbType.VarChar);
                mySqlCommand.Parameters.Add("@FechaDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@Revelacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoPadre", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = docCtrl.NumeroDoc.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = docCtrl.DocumentoID.Value;
                mySqlCommand.Parameters["@PeriodoUltMov"].Value = docCtrl.PeriodoUltMov.Value;
                mySqlCommand.Parameters["@AreaFuncionalID"].Value = docCtrl.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@PrefijoID"].Value = docCtrl.PrefijoID.Value;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = docCtrl.DocumentoTipo.Value;
                mySqlCommand.Parameters["@DocumentoNro"].Value = docCtrl.DocumentoNro.Value;
                mySqlCommand.Parameters["@MonedaID"].Value = docCtrl.MonedaID.Value;
                mySqlCommand.Parameters["@TasaCambioDOCU"].Value = docCtrl.TasaCambioDOCU.Value;
                mySqlCommand.Parameters["@TasaCambioCONT"].Value = docCtrl.TasaCambioCONT.Value;
                mySqlCommand.Parameters["@ComprobanteID"].Value = docCtrl.ComprobanteID.Value;
                mySqlCommand.Parameters["@ComprobanteIDNro"].Value = docCtrl.ComprobanteIDNro.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = docCtrl.TerceroID.Value;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = docCtrl.DocumentoTercero.Value;
                mySqlCommand.Parameters["@CuentaID"].Value = docCtrl.CuentaID.Value;
                mySqlCommand.Parameters["@ProyectoID"].Value = docCtrl.ProyectoID.Value;
                mySqlCommand.Parameters["@CentroCostoID"].Value = docCtrl.CentroCostoID.Value;
                mySqlCommand.Parameters["@LineaPresupuestoID"].Value = docCtrl.LineaPresupuestoID.Value;
                mySqlCommand.Parameters["@LugarGeograficoID"].Value = docCtrl.LugarGeograficoID.Value;
                mySqlCommand.Parameters["@DocumentoAnula"].Value = docCtrl.DocumentoAnula.Value;
                mySqlCommand.Parameters["@PeriodoAnula"].Value = docCtrl.PeriodoAnula.Value;
                mySqlCommand.Parameters["@ConsSaldo"].Value = docCtrl.ConsSaldo.Value;
                mySqlCommand.Parameters["@Observacion"].Value = docCtrl.Observacion.Value;
                mySqlCommand.Parameters["@FechaDoc"].Value = docCtrl.FechaDoc.Value;
                mySqlCommand.Parameters["@Valor"].Value = docCtrl.Valor.Value;
                mySqlCommand.Parameters["@Iva"].Value = docCtrl.Iva.Value;
                mySqlCommand.Parameters["@Descripcion"].Value = docCtrl.Descripcion.Value;
                mySqlCommand.Parameters["@Revelacion"].Value = docCtrl.Revelacion.Value;
                mySqlCommand.Parameters["@DocumentoPadre"].Value = docCtrl.DocumentoPadre.Value;
                mySqlCommand.Parameters["@seUsuarioID"].Value = docCtrl.seUsuarioID.Value != null? docCtrl.seUsuarioID.Value : this.UserId;
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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los consecutivos del documento control
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        /// <param name="tipo">Dice si es un documento interno o externo</param>
        /// <param name="documentoNro">Consecutivo del documento (Null si no se debe actualizar)</param>
        /// <param name="comprobanteNro">Consecutivo del comprobante (Null si no se debe actualizar)</param>
        /// <param name="isPre">Indica si debe actualizar la tabla de aux o auxPre (para docs internos)</param>
        public void DAL_glDocumentoControl_UpdateConsecutivos(int numeroDoc, DocumentoTipo tipo, int? documentoNro, int? comprobanteNro, string compID, bool isPre, int? documentoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                //Agrega consecutivo de documento
                if (documentoNro.HasValue)
                {
                    mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoNro"].Value = documentoNro.Value;

                    query += " DocumentoNro = @DocumentoNro";
                }
                //Agrega consecutivo de comprobante
                if (comprobanteNro.HasValue)
                {
                    mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                    mySqlCommand.Parameters["@ComprobanteID"].Value = compID;

                    mySqlCommand.Parameters.Add("@ComprobanteIDNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@ComprobanteIDNro"].Value = comprobanteNro.Value;

                    if (documentoNro.HasValue)
                        query += ",";

                    query += " ComprobanteID = @ComprobanteID, ComprobanteIDNro = @ComprobanteIDNro";
                }

                mySqlCommand.CommandText = "UPDATE glDocumentoControl SET " + query + " WHERE NumeroDoc=@NumeroDoc";
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.ExecuteNonQuery();

                //Actualiza la info de la llave para un doc interno en los comprobantes
                if (documentoNro.HasValue && tipo == DocumentoTipo.DocInterno)
                {
                    if (documentoID != AppDocuments.FacturaVenta && documentoID != AppDocuments.NotaCredito)
                    {
                        string table = isPre ? "coAuxiliarPre" : "coAuxiliar";
                        mySqlCommand.CommandText = "UPDATE " + table + " SET DocumentoCOM = @DocumentoNro WHERE IdentificadorTR=@NumeroDoc";
                        mySqlCommand.ExecuteNonQuery();
                    }
                    else 
                    {
                        string table = isPre ? "coAuxiliarPre" : "coAuxiliar";
                        mySqlCommand.CommandText = "UPDATE " + table + " SET DocumentoCOM = @DocumentoNro WHERE NumeroDoc=@NumeroDoc";
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_UpdateConsecutivos");
                throw exception;
            }
        }

        #endregion

        #region Otras

        #region Traer datos

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetInternalDoc(int documentID, string idPrefijo, int documentoNro)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select doc.*, usr.UsuarioID as UsuarioIDDesc " +
                    "from glDocumentoControl doc with(nolock) inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID " +
                    "where doc.EmpresaID = @EmpresaID and PrefijoID = @PrefijoID and DocumentoNro = @DocumentoNro and DocumentoTipo = @DocumentoTipo and DocumentoID=@DocumentoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PrefijoID"].Value = idPrefijo;
                mySqlCommand.Parameters["@DocumentoNro"].Value = documentoNro;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = (byte)DocumentoTipo.DocInterno;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;

                DTO_glDocumentoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_glDocumentoControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetInternalDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetExternalDoc(int documentID, string idTercero, string DocumentoTercero)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select doc.*, usr.UsuarioID as UsuarioIDDesc " +
                    "from glDocumentoControl doc with(nolock) inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID " +
                    "where doc.EmpresaID = @EmpresaID and TerceroID = @TerceroID and DocumentoTercero = @DocumentoTercero and DocumentoTipo = @DocumentoTipo and DocumentoID=@DocumentoID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@TerceroID"].Value = idTercero;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = DocumentoTercero;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = (byte)DocumentoTipo.DocExterno;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;

                DTO_glDocumentoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_glDocumentoControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetExternalDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="cuentaID">Identificadior de la cuenta</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetInternalDocByCta(string cuentaID, string idPrefijo, int documentoNro)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select doc.*, usr.UsuarioID as UsuarioIDDesc " +
                    "from glDocumentoControl doc with(nolock) inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID " +
                    "where doc.EmpresaID = @EmpresaID and PrefijoID = @PrefijoID and DocumentoNro = @DocumentoNro and DocumentoTipo = @DocumentoTipo " +
                    "   and CuentaID = @CuentaID and eg_coPlanCuenta = @eg_coPlanCuenta";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@PrefijoID"].Value = idPrefijo;
                mySqlCommand.Parameters["@DocumentoNro"].Value = documentoNro;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = (byte)DocumentoTipo.DocInterno;
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                DTO_glDocumentoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    res = new DTO_glDocumentoControl(dr);

                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetInternalDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="cuentaID">Identificadior de la cuenta</param>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetExternalDocByCta(string cuentaID, string idTercero, string DocumentoTercero)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select doc.*, usr.UsuarioID as UsuarioIDDesc " +
                    "from glDocumentoControl doc with(nolock) inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID " +
                    "where doc.EmpresaID = @EmpresaID and TerceroID = @TerceroID and DocumentoTercero = @DocumentoTercero and DocumentoTipo = @DocumentoTipo " +
                    "   and CuentaID = @CuentaID and eg_coPlanCuenta = @eg_coPlanCuenta";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@CuentaID"].Value = cuentaID;
                mySqlCommand.Parameters["@TerceroID"].Value = idTercero;
                mySqlCommand.Parameters["@DocumentoTercero"].Value = DocumentoTercero;
                mySqlCommand.Parameters["@DocumentoTipo"].Value = (byte)DocumentoTipo.DocExterno;
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                DTO_glDocumentoControl res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    res = new DTO_glDocumentoControl(dr);

                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetExternalDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un proceso de billing
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="compID">Comprobante</param>
        /// <param name="monedaID">Moneda</param>
        /// <returns>Retorna el documento control </returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByBilling(DateTime periodo, string compID, string monedaID)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glDocumentoControl doc with(nolock) " +
                    "WHERE doc.EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND PeriodoDoc=@PeriodoDoc " +
                    "   AND ComprobanteID = @ComprobanteID AND MonedaID = @MonedaID";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppProcess.ParticionBilling;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = compID;
                mySqlCommand.Parameters["@MonedaID"].Value = monedaID;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_glDocumentoControl(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByComprobante");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByComprobante(int documentoID, DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT TOP(1) doc.*, usr.UsuarioID as UsuarioIDDesc " +
                    "from glDocumentoControl doc with(nolock) inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID " +
                    "WHERE doc.EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND PeriodoDoc=@PeriodoDoc AND ComprobanteID=@ComprobanteID AND ComprobanteIDNro=@ComprobanteIDNro ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommand.Parameters.Add("@ComprobanteIDNro", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@ComprobanteID"].Value = comprobanteID;
                mySqlCommand.Parameters["@ComprobanteIDNro"].Value = compNro;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_glDocumentoControl(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByComprobante");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un cierre anual
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <returns>Retorna el documento control </returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByCierreAnual(DateTime periodo)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glDocumentoControl doc with(nolock) " +
                    "WHERE doc.EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND PeriodoDoc=@PeriodoDoc  ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppProcess.CierreAnual;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_glDocumentoControl(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByComprobante");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la liquiadcion documento aociado al tipo de documento y periodo para un empleado
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="empleadoID">empleadoID</param>
        /// <param name="periodo">periodo</param>
        /// <returns>Documento</returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByDocEmpleado(int documentID, string terceroID, DateTime periodo)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glDocumentoControl with(nolock) " +
                    "WHERE EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND PeriodoDoc=@PeriodoDoc AND TerceroID=@TerceroID";


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_glDocumentoControl(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetDocEmpleado");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la liquidacion documento aociado al tipo de documento y periodo para un empleado
        /// </summary>
        /// <param name="documentID">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="contrato">numero de contrato del empleado</param>
        /// <returns>documento asociado al empleado</returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByDocEmpleado(int documentID, DateTime periodo, int contrato)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "   SELECT glDocumentoControl.*     " +
                    "   FROM glDocumentoControl with(nolock)     " +
                    "   INNER JOIN noLiquidacionesDocu on glDocumentoControl.NumeroDoc = noLiquidacionesDocu.NumeroDoc     " +
                    "   WHERE glDocumentoControl.EmpresaID = @EmpresaID " +
                    "   AND  glDocumentoControl.DocumentoID = @DocumentoID     " +
                    "   AND glDocumentoControl.PeriodoDoc = @Periodo     " +
                    "   AND noLiquidacionesDocu.ContratoNOID = @ContratoNOID";


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ContratoNOID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@ContratoNOID"].Value = contrato;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_glDocumentoControl(dr);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetDocEmpleado");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el documento reklacionado con una libranza
        /// </summary>
        /// <param name="libranza">Libranza</param>
        /// <param name="validateAct">Indica si se debe validar el documento segun el estado de una actividad</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="cerradoInd">Indica si trae la actividad con estado cerrado o abierto</param>
        /// <returns>Documento</returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByLibranzaSolicitud(int libranza, string actFlujoID, bool cerradoInd)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@CerradoInd"].Value = cerradoInd;
                if (String.IsNullOrEmpty(actFlujoID))
                {
                    mySqlCommand.CommandText =
                    "select ctrl.* " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join ccSolicitudDocu sol with(nolock) on ctrl.NumeroDoc = sol.NumeroDoc " +
                    "where ctrl.EmpresaID = @EmpresaID and sol.Libranza = @Libranza";
                }
                else
                {
                    mySqlCommand.CommandText =
                    "select ctrl.* " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join ccSolicitudDocu sol with(nolock) on ctrl.NumeroDoc = sol.NumeroDoc " +
                    "   inner join glActividadEstado act with(nolock) on ctrl.NumeroDoc = act.NumeroDoc " +
                    "where ctrl.EmpresaID = @EmpresaID and sol.Libranza = @Libranza" +
                    " and act.ActividadFlujoID = @ActividadFlujoID and act.CerradoInd = @CerradoInd ";
                }

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Libranza"].Value = libranza;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_glDocumentoControl(dr);
                }

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByLibranzaSolicitud");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el documento reklacionado con una libranza
        /// </summary>
        /// <param name="NumDocPadre">Numero de dpcumento padre</param>
        /// <returns>Documento</returns>
        public DTO_glDocumentoControl DAL_glDocumentoControl_GetByCxP(int NumDocPadre)
        {
            try
            {
                DTO_glDocumentoControl result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumDocPadre;

                mySqlCommand.CommandText =
                    "select ctrl.* " +
                    "from cpCuentaXPagar cxp with(nolock) " +
                    "	inner join gldocumentocontrol ctrl with(nolock) on cxp.NumeroDoc = ctrl.NumeroDoc " +
                    "where cxp.NumeroDocPadre = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_glDocumentoControl(dr);

                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByCxP");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un documento de la migarcion de nomina
        /// </summary>
        /// <param name="documentID">Documento del cual esta buscando el control</param>
        /// <param name="periodo">Periodo de búsqueda</param>
        /// <param name="pagaduria">Identificador del centro de pago</param>
        /// <returns>Retorna el glDocumentoControl de la ML y laME</returns>
        public List<DTO_glDocumentoControl> DAL_glDocumentoControl_GetByMigracionNomina(int documentID, DateTime periodo, string pagaduria)
        {
            try
            {
                List<DTO_glDocumentoControl> result = new List<DTO_glDocumentoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.VarChar, UDT_PagaduriaID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@PagaduriaID"].Value = pagaduria;

                mySqlCommand.CommandText =
                    "select distinct ctrl.* " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	left join ccNominaDeta deta with(nolock) on ctrl.NumeroDoc = deta.NumDocNomina and deta.PagaduriaID = @PagaduriaID " +
                    "	left join ccNominaPreliminar pre with(nolock) on ctrl.NumeroDoc = pre.NumDocNomina and pre.PagaduriaID = @PagaduriaID " +
                    "where EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND PeriodoDoc=@PeriodoDoc " +
                    "Order by NumeroDoc";


                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl(dr);
                    result.Add(ctrl);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByMigracionNomina");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de documentos para ajuste de saldos en un periodo
        /// </summary>
        /// <param name="documentID">Documento del cual esta buscando el control</param>
        /// <param name="periodo">Periodo de búsqueda</param>
        /// <returns>Retorna el glDocumentoControl de la ML y laME</returns>
        public List<DTO_glDocumentoControl> DAL_glDocumentoControl_GetByPeriodoDocumento(int documentID, DateTime periodo)
        {
            try
            {
                List<DTO_glDocumentoControl> result = new List<DTO_glDocumentoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glDocumentoControl with(nolock) " +
                    "WHERE EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND PeriodoDoc=@PeriodoDoc " +
                    "order by NumeroDoc desc";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glDocumentoControl doc = new DTO_glDocumentoControl(dr);
                    result.Add(doc);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByPeriodoDocumento");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la liquiadcion documento aociado al tipo de documento y periodo para un empleado
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Listado de Documentos</returns>
        public List<DTO_glDocumentoControl> DAL_glDocumentoControl_GetDocEmpleado(int documentID, string terceroID)
        {
            try
            {
                List<DTO_glDocumentoControl> result = new List<DTO_glDocumentoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT * from glDocumentoControl with(nolock) " +
                    "WHERE EmpresaID = @EmpresaID AND DocumentoID=@DocumentoID AND TerceroID=@TerceroID";


                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommand.Parameters["@TerceroID"].Value = terceroID;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_glDocumentoControl doc = new DTO_glDocumentoControl(dr);
                    result.Add(doc);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetDocEmpleado");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de documentos de un modulo
        /// </summary>
        /// <param name="mod">Modulo</param>
        /// <param name="periodo">Periodo de búsqueda</param>
        /// <param name="contabilizado">Indica si trae los documentos que ya fueron procesados</param>
        /// <returns>Retorna la lista de documentos</returns>
        public List<DTO_glDocumentoControl> DAL_glDocumentoControl_GetByModulo(DateTime periodo, ModulesPrefix mod, bool contabilizado)
        {
            try
            {
                List<DTO_glDocumentoControl> result = new List<DTO_glDocumentoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;

                mySqlCommand.CommandText =
                    "select ctrl.* " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "where ctrl.EmpresaID = @EmpresaID and ctrl.PeriodoDoc = @PeriodoDoc and doc.ModuloID = @ModuloID";

                if (contabilizado)
                    mySqlCommand.CommandText +=
                        " and ComprobanteID is not null and ComprobanteIDNro is not null " +
                        " and RTRIM(LTRIM(ComprobanteID)) <> '' and ComprobanteIDNro <> 0";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glDocumentoControl doc = new DTO_glDocumentoControl(dr);
                    result.Add(doc);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByModulo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la lista de hijos de un documento
        /// </summary>
        /// <param name="numeroDoc">Numero de documento padre</param>
        public List<int> DAL_glDocumentoControl_GetChilds(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM glDocumentoControl with(nolock) WHERE DocumentoPadre=@NumeroDoc";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                List<int> childs = new List<int>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int child = Convert.ToInt32(dr["NumeroDoc"]);
                    childs.Add(child);
                }
                dr.Close();

                return childs;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetChilds");
                throw ex;
            }
        }

        /// <summary>
        /// Calcula la cantidad de "hijos" con una estado especifico
        /// </summary>
        /// <param name="numeroDoc">Numero de documento padre</param>
        /// <param name="estado">Estado del documento del hijo que se desa filtrar</param>
        public int DAL_glDocumentoControl_GetApproveChilds(int numeroDoc, int estado)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.Int);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Estado"].Value = estado;

                mySqlCommand.CommandText = "SELECT COUNT(*) " +
                                           "FROM glDocumentoControl ctrlHijo with(nolock) " +
                                           "	INNER JOIN glDocumentoControl ctrlPadre with(nolock) ON ctrlHijo.DocumentoPadre = ctrlPadre.NumeroDoc " +
                                           "		AND ctrlPadre.Estado = @Estado " +
                                           "WHERE ctrlHijo.DocumentoPadre=@NumeroDoc";

                int approved = (int)mySqlCommand.ExecuteScalar();
                return approved;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetApproveChilds");
                throw ex;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrl">Doc Control filtro</param>
        /// <returns>Lista de Doc Control </returns>
        public List<DTO_glDocumentoControl> DAL_glDocumentoControl_GetByParameter(DTO_glDocumentoControl filter)
        {
            try
            {
                List<DTO_glDocumentoControl> result = new List<DTO_glDocumentoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = " Select doc.*, usr.UsuarioID as UsuarioIDDesc, ter.Descriptivo as TerceroDesc " +
                        " from glDocumentoControl doc with(nolock) " +
                        " inner join seUsuario usr with(nolock) on doc.seUsuarioID = usr.ReplicaID " +
                        " left join coTercero ter with(nolock) on ter.TerceroID = doc.TerceroID and ter.EmpresaGrupoID = doc.eg_coTercero  " +
                        "where doc.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and doc.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoID.Value.ToString()))
                {
                    query += "and doc.DocumentoID = @DocumentoID ";
                    mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoID"].Value = filter.DocumentoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoNro.Value.ToString()))
                {
                    query += "and doc.DocumentoNro = @DocumentoNro ";
                    mySqlCommand.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoNro"].Value = filter.DocumentoNro.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoTercero.Value.ToString()))
                {
                    query += "and doc.DocumentoTercero = @DocumentoTercero ";
                    mySqlCommand.Parameters.Add("@DocumentoTercero", SqlDbType.Char, 20);
                    mySqlCommand.Parameters["@DocumentoTercero"].Value = filter.DocumentoTercero.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoTipo.Value.ToString()))
                {
                    query += "and doc.DocumentoTipo = @DocumentoTipo ";
                    mySqlCommand.Parameters.Add("@DocumentoTipo", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@DocumentoTipo"].Value = filter.DocumentoTipo.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Estado.Value.ToString()))
                {
                    query += "and doc.Estado = @Estado ";
                    mySqlCommand.Parameters.Add("@Estado", SqlDbType.SmallInt);
                    mySqlCommand.Parameters["@Estado"].Value = filter.Estado.Value;
                    filterInd = true;
                }
                if (string.IsNullOrEmpty(filter.ComprobanteID.Value.ToString()) && 
                    !string.IsNullOrEmpty(filter.FechaDoc.Value.ToString()) && string.IsNullOrEmpty(filter.FechaInicial.Value.ToString()))
                {
                    query += "and doc.FechaDoc = @FechaDoc ";
                    mySqlCommand.Parameters.Add("@FechaDoc", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaDoc"].Value = filter.FechaDoc.Value;
                    filterInd = true;
                }
                if (string.IsNullOrEmpty(filter.ComprobanteID.Value.ToString()) &&
                    (!string.IsNullOrEmpty(filter.FechaInicial.Value.ToString()) || !string.IsNullOrEmpty(filter.FechaFinal.Value.ToString())))
                {
                    query += "and doc.FechaDoc between @FechaInicial and @FechaFinal ";
                    mySqlCommand.Parameters.Add("@FechaInicial", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaInicial"].Value = filter.FechaInicial.Value;
                    mySqlCommand.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaFinal"].Value = filter.FechaFinal.Value;

                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ComprobanteID.Value.ToString()) && !string.IsNullOrEmpty(filter.PeriodoDoc.Value.ToString()))
                {
                    query += "and doc.PeriodoDoc = @PeriodoDoc ";
                    mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.Date);
                    mySqlCommand.Parameters["@PeriodoDoc"].Value = filter.PeriodoDoc.Value;
                    filterInd = true;
                }
                else if (!string.IsNullOrEmpty(filter.PeriodoDoc.Value.ToString()))
                {
                    query += "and doc.PeriodoDoc = @PeriodoDoc ";
                    mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.Date);
                    mySqlCommand.Parameters["@PeriodoDoc"].Value = filter.PeriodoDoc.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CuentaID.Value.ToString()))
                {
                    query += "and doc.CuentaID = @CuentaID ";
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaID"].Value = filter.CuentaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value))
                {
                    query += "and doc.CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LineaPresupuestoID.Value.ToString()))
                {
                    query += "and doc.LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = filter.LineaPresupuestoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LugarGeograficoID.Value.ToString()))
                {
                    query += "and doc.LugarGeograficoID = @LugarGeograficoID ";
                    mySqlCommand.Parameters.Add("@LugarGeograficoID", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                    mySqlCommand.Parameters["@LugarGeograficoID"].Value = filter.LugarGeograficoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.MonedaID.Value.ToString()))
                {
                    query += "and doc.MonedaID = @MonedaID ";
                    mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                    mySqlCommand.Parameters["@MonedaID"].Value = filter.MonedaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.PrefijoID.Value.ToString()))
                {
                    query += "and doc.PrefijoID = @PrefijoID ";
                    mySqlCommand.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                    mySqlCommand.Parameters["@PrefijoID"].Value = filter.PrefijoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and doc.ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    query += "and doc.TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ComprobanteID.Value))
                {
                    query += "and doc.ComprobanteID = @ComprobanteID ";
                    mySqlCommand.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                    mySqlCommand.Parameters["@ComprobanteID"].Value = filter.ComprobanteID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ComprobanteIDNro.Value.ToString()))
                {
                    query += "and doc.ComprobanteIDNro = @ComprobanteIDNro ";
                    mySqlCommand.Parameters.Add("@ComprobanteIDNro", SqlDbType.Int);
                    mySqlCommand.Parameters["@ComprobanteIDNro"].Value = filter.ComprobanteIDNro.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.DocumentoPadre.Value.ToString()))
                {
                    query += "and doc.DocumentoPadre = @DocumentoPadre ";
                    mySqlCommand.Parameters.Add("@DocumentoPadre", SqlDbType.Int);
                    mySqlCommand.Parameters["@DocumentoPadre"].Value = filter.DocumentoPadre.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConsSaldo.Value.ToString()))
                {
                    query += "and doc.ConsSaldo = @ConsSaldo ";
                    mySqlCommand.Parameters.Add("@ConsSaldo", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsSaldo"].Value = filter.ConsSaldo.Value.Value;
                    filterInd = true;
                }
                query += " order by doc.NumeroDoc desc";
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl(dr);
                    ctrl.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_GetByParameter");
                throw exception;
            }
        }

        #endregion

        #region Validar Datos

        /// <summary>
        /// Valida si se puede realizar el proceso de cierre anual
        /// </summary>
        /// <param name="year">Año de validacion</param>
        /// <returns>Retirna verdadero si ya se realizo el proceso de ajuste en cambio</returns>
        public bool DAL_glDocumentoControl_ValidaAjusteEnCambio(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from glDocumentoControl with(nolock) where EmpresaID=@EmpresaID and DocumentoID=@DocumentoID and PeriodoDoc=@PeriodoDoc";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@DocumentoID"].Value = AppDocuments.ComprobanteAjusteCambio;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;

                int res = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return res == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_ValidaCierreAnual");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el padre de un documento (Si existe)
        /// </summary>
        /// <param name="numeroDoc">Numero de documento hijo</param>
        public bool DAL_glDocumentoControl_HasParent(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT COUNT(*) FROM glDocumentoControl with(nolock) WHERE NumeroDoc=@NumeroDoc and DocumentoPadre is not null";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_HasParent");
                throw ex;
            }
        }

        #endregion

        #region Generales

        /// <summary>
        /// Le cambia el estado a un documentoControl y permite activar las alarmas
        /// </summary>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) de glDocumentoControl</param>
        /// <param name="estado">Nuevo estado</param>
        public void DAL_glDocumentoControl_ChangeDocumentStatus(int numeroDoc, EstadoDocControl estado, string observacion)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.SmallInt);

                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@Estado"].Value = Convert.ToInt16(estado);

                string set = " Estado = @Estado ";
                if (!string.IsNullOrWhiteSpace(observacion))
                {
                    string newObs = Environment.NewLine + Environment.NewLine + " (" + DateTime.Now.ToString() + ")" + " -> " + observacion;

                    mySqlCommand.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                    mySqlCommand.Parameters["@Observacion"].Value = newObs;

                    set += ", Observacion = ltrim(rtrim(Observacion)) + @Observacion";
                }
                mySqlCommand.CommandText =
                    "UPDATE glDocumentoControl SET " + set + " WHERE NumeroDoc=@NumeroDoc";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_ChangeDocumentStatus");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el numero de documentos en un periodo segun el estado
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="estados">Posibles estados</param>
        /// <returns>Retorna ael numero de documentos</returns>
        public int DAL_glDocumentoControl_CountDocumentsByEstado(DateTime periodo, ModulesPrefix mod, List<EstadoDocControl> estados, string excluyeDocs)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string where = string.Empty;
                string whereDocs = string.Empty;
                for (int i = 0; i < estados.Count; ++i)
                {
                    short estado = (short)estados[i];

                    if (i == 0)
                        where = " and (ctrl.Estado = " + estado;
                    else
                        where += " or ctrl.Estado = " + estado;

                    if (i == estados.Count - 1)
                        where += ")";
                }

                if (!string.IsNullOrEmpty(excluyeDocs))
                    whereDocs = " and ctrl.DocumentoID not in (" + excluyeDocs  + " ) "; 

                mySqlCommand.CommandText =
                    "select COUNT(*) from glDocumentoControl ctrl with(nolock) " +
                    "   inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "where ctrl.EmpresaID = @EmpresaID and ctrl.PeriodoDoc=@PeriodoDoc and doc.ModuloID = @ModuloID" + where + whereDocs;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();

                return Convert.ToInt32(mySqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_CountDocumentsByEstado");
                throw exception;
            }
        }

        /// <summary>
        /// Revisa cuantos documentos existen en un estado y un periodo
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="periodo">Periodo</param>
        /// <returns>Retorna la cantidad de documentos en un estado</returns>
        public int DAL_glDocumentoControl_CountByEstadoModulo(EstadoDocControl estado, DateTime periodo, ModulesPrefix mod)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "SELECT count(*) " +
                    "from glDocumentoControl ctrl with(nolock) " +
                    "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID and doc.ModuloID = @ModuloID " +
                    "WHERE EmpresaID = @EmpresaID AND Estado=@Estado AND PeriodoDoc=@PeriodoDoc ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, 3);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)estado;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();

                int rows = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return rows;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_CountByEstadoModulo");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el periodo del documentoControl
        /// </summary>
        /// <param name="numero Doc">numero del doc</param>
        public void DAL_glDocumentoControl_UpdatePeriodo(int numeroDoc,DateTime periodo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                    "UPDATE glDocumentoControl SET" +
                    "   PeriodoDoc = @PeriodoDoc " +
                    "WHERE NumeroDoc=@NumeroDoc";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@PeriodoDoc", SqlDbType.SmallDateTime);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@PeriodoDoc"].Value = periodo;
                #endregion

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glDocumentoControl_UpdatePeriodo");
                throw exception;
            }
        }

        #endregion

        #endregion
    }
}
