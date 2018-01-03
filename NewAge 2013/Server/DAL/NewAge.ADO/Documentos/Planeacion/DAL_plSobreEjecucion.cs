using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.Linq;

namespace NewAge.ADO
{
    public class DAL_plSobreEjecucion : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plSobreEjecucion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Privadas

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="deta">Cierre nuevo</param>
        /// <returns></returns>
        private void DAL_plSobreEjecucion_AddItem(DTO_plSobreEjecucion deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plSobreEjecucion " +
                    "( " +
                        "  EmpresaID " +
                        " ,ProyectoID " +
                        " ,CentroCostoID " +
                        " ,LineaPresupuestoID " +
                        " ,CodigoBSID " +
                        " ,PrefijoID " +
                        " ,DocumentoNro " +
                        " ,TipoDocumento " +
                        " ,NumeroDoc " +
                        " ,ConsecutivoDetaID " +
                        " ,FechaDoc " +
                        " ,MonedaOrigen " +
                        " ,AreaAprobacion " +
                        " ,Estado " +
                        " ,TipoAprobacion " +
                        " ,ProveedorID " +
                        " ,TasaOC " +
                        " ,CantidadDOC " +
                        " ,ValorOCLocML " +
                        " ,ValorOCLocME " +
                        " ,ValorOCExtME " +
                        " ,ValorOCExtML " +
                        " ,CantidadSOL " +
                        " ,CtoOrigenLocML " +
                        " ,CtoOrigenLocME " +
                        " ,CtoOrigenExtME " +
                        " ,CtoOrigenExtML " +
                        " ,CantidadPTO " +
                        " ,PtoMesLocML " +
                        " ,PtoMesLocME " +
                        " ,PtoMesExtME " +
                        " ,PtoMesExtML " +
                        " ,PtoTotalLocML " +
                        " ,PtoTotalLocME " +
                        " ,PtoTotalExtME " +
                        " ,PtoTotalExtML " +
                        " ,CompActLocML " +
                        " ,CompActLocME " +
                        " ,CompActExtME " +
                        " ,CompActExtML " +
                        " ,RecibidoLocML " +
                        " ,RecibidoLocME " +
                        " ,RecibidoExtME " +
                        " ,RecibidoExtML " +
                        " ,CtoInicialLocML " +
                        " ,ocProcesoLocML " +
                        " ,ocProcesoExtME " +
                        " ,CtoInicialExtME " +
                        " ,PtoInicialLocML " +
                        " ,PtoInicialExtME " +
                        " ,CompInicialocML " +
                        " ,CompinicialExtME " +
                        " ,RecInicialLocML " +
                        " ,RecInicialExtME " +
                        " ,ocProcesoInicialLocML " +
                        " ,ocProcesoInicialExtME " +
                        " ,UsuarioRevSobreejec " +
                        " ,FechaRevSobreejec " +
                        " ,UsuarioAprSobreejec " +
                        " ,FechaAprSobreejec " +
                        " ,SolicitaPresInd " +
                        " ,Observacion " +
                        " ,eg_coProyecto " +
                        " ,eg_coCentroCosto " +
                        " ,eg_plLineaPresupuesto " +
                        " ,eg_glPrefijo " +
                        " ,eg_prBienServicio " +
                        " ,eg_glAreaFuncional " +
                        " ,eg_prProveedor " +
                    ") " +
                    "VALUES " +
                    "( " +
                        "  @EmpresaID " +
                        " ,@ProyectoID " +
                        " ,@CentroCostoID " +
                        " ,@LineaPresupuestoID " +
                        " ,@CodigoBSID " +
                        " ,@PrefijoID " +
                        " ,@DocumentoNro " +
                        " ,@TipoDocumento " +
                        " ,@NumeroDoc " +
                        " ,@ConsecutivoDetaID " +
                        " ,@FechaDoc " +
                        " ,@MonedaOrigen " +
                        " ,@AreaAprobacion " +
                        " ,@Estado " +
                        " ,@TipoAprobacion " +
                        " ,@ProveedorID " +
                        " ,@TasaOC " +
                        " ,@CantidadDOC " +
                        " ,@ValorOCLocML " +
                        " ,@ValorOCLocME " +
                        " ,@ValorOCExtME " +
                        " ,@ValorOCExtML " +
                        " ,@CantidadSOL " +
                        " ,@CtoOrigenLocML " +
                        " ,@CtoOrigenLocME " +
                        " ,@CtoOrigenExtME " +
                        " ,@CtoOrigenExtML " +
                        " ,@CantidadPTO " +
                        " ,@PtoMesLocML " +
                        " ,@PtoMesLocME " +
                        " ,@PtoMesExtME " +
                        " ,@PtoMesExtML " +
                        " ,@PtoTotalLocML " +
                        " ,@PtoTotalLocME " +
                        " ,@PtoTotalExtME " +
                        " ,@PtoTotalExtML " +
                        " ,@CompActLocML " +
                        " ,@CompActLocME " +
                        " ,@CompActExtME " +
                        " ,@CompActExtML " +
                        " ,@RecibidoLocML " +
                        " ,@RecibidoLocME " +
                        " ,@RecibidoExtME " +
                        " ,@RecibidoExtML " +
                        " ,@CtoInicialLocML " +
                        " ,@ocProcesoLocML " +
                        " ,@ocProcesoExtME " +
                        " ,@CtoInicialExtME " +
                        " ,@PtoInicialLocML " +
                        " ,@PtoInicialExtME " +
                        " ,@CompInicialocML " +
                        " ,@CompinicialExtME " +
                        " ,@RecInicialLocML " +
                        " ,@RecInicialExtME " +
                        " ,@ocProcesoInicialLocML " +
                        " ,@ocProcesoInicialExtME " +
                        " ,@UsuarioRevSobreejec " +
                        " ,@FechaRevSobreejec " +
                        " ,@UsuarioAprSobreejec " +
                        " ,@FechaAprSobreejec " +
                        " ,@SolicitaPresInd " +
                        " ,@Observacion " +
                        " ,@eg_coProyecto " +
                        " ,@eg_coCentroCosto " +
                        " ,@eg_plLineaPresupuesto " +
                        " ,@eg_glPrefijo " +
                        " ,@eg_prBienServicio " +
                        " ,@eg_glAreaFuncional " +
                        " ,@eg_prProveedor " +
                    ") ";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                //Valores
                mySqlCommandSel.Parameters.Add("@TipoDocumento", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaDoc", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@MonedaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoAprobacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TasaOC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadDOC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPTO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompInicialocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompinicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@UsuarioRevSobreejec", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaRevSobreejec", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioAprSobreejec", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaAprSobreejec", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@SolicitaPresInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.VarChar);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = deta.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@PrefijoID"].Value = deta.PrefijoID.Value;
                mySqlCommandSel.Parameters["@DocumentoNro"].Value = deta.DocumentoNro.Value;
                //Valores
                mySqlCommandSel.Parameters["@TipoDocumento"].Value = deta.TipoDocumento.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsecutivoDetaID"].Value = deta.ConsecutivoDetaID.Value;
                mySqlCommandSel.Parameters["@FechaDoc"].Value = deta.FechaDoc.Value;
                mySqlCommandSel.Parameters["@MonedaOrigen"].Value = deta.MonedaOrigen.Value;
                mySqlCommandSel.Parameters["@AreaAprobacion"].Value = deta.AreaAprobacion.Value;
                mySqlCommandSel.Parameters["@Estado"].Value = deta.Estado.Value;
                mySqlCommandSel.Parameters["@TipoAprobacion"].Value = deta.TipoAprobacion.Value;
                mySqlCommandSel.Parameters["@ProveedorID"].Value = deta.ProveedorID.Value;
                mySqlCommandSel.Parameters["@TasaOC"].Value = deta.TasaOC.Value;
                mySqlCommandSel.Parameters["@CantidadDOC"].Value = deta.CantidadDOC.Value;
                mySqlCommandSel.Parameters["@ValorOCLocML"].Value = deta.ValorOCLocML.Value;
                mySqlCommandSel.Parameters["@ValorOCLocME"].Value = deta.ValorOCLocME.Value;
                mySqlCommandSel.Parameters["@ValorOCExtME"].Value = deta.ValorOCExtME.Value;
                mySqlCommandSel.Parameters["@ValorOCExtML"].Value = deta.ValorOCExtML.Value;
                mySqlCommandSel.Parameters["@CantidadSOL"].Value = deta.CantidadSOL.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocML"].Value = deta.CtoOrigenLocML.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocME"].Value = deta.CtoOrigenLocME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtME"].Value = deta.CtoOrigenExtME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtML"].Value = deta.CtoOrigenExtML.Value;
                mySqlCommandSel.Parameters["@CantidadPTO"].Value = deta.CantidadPTO.Value;
                mySqlCommandSel.Parameters["@PtoMesLocML"].Value = deta.PtoMesLocML.Value;
                mySqlCommandSel.Parameters["@PtoMesLocME"].Value = deta.PtoMesLocME.Value;
                mySqlCommandSel.Parameters["@PtoMesExtME"].Value = deta.PtoMesExtME.Value;
                mySqlCommandSel.Parameters["@PtoMesExtML"].Value = deta.PtoMesExtML.Value;
                mySqlCommandSel.Parameters["@PtoTotalLocML"].Value = deta.PtoTotalLocML.Value;
                mySqlCommandSel.Parameters["@PtoTotalLocME"].Value = deta.PtoTotalLocME.Value;
                mySqlCommandSel.Parameters["@PtoTotalExtME"].Value = deta.PtoTotalExtME.Value;
                mySqlCommandSel.Parameters["@PtoTotalExtML"].Value = deta.PtoTotalExtML.Value;
                mySqlCommandSel.Parameters["@CompActLocML"].Value = deta.CompActLocML.Value;
                mySqlCommandSel.Parameters["@CompActLocME"].Value = deta.CompActLocME.Value;
                mySqlCommandSel.Parameters["@CompActExtME"].Value = deta.CompActExtME.Value;
                mySqlCommandSel.Parameters["@CompActExtML"].Value = deta.CompActExtML.Value;
                mySqlCommandSel.Parameters["@RecibidoLocML"].Value = deta.RecibidoLocML.Value;
                mySqlCommandSel.Parameters["@RecibidoLocME"].Value = deta.RecibidoLocME.Value;
                mySqlCommandSel.Parameters["@RecibidoExtML"].Value = deta.RecibidoExtML.Value;
                mySqlCommandSel.Parameters["@RecibidoExtME"].Value = deta.RecibidoExtME.Value;
                mySqlCommandSel.Parameters["@CtoInicialLocML"].Value = deta.CtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@ocProcesoLocML"].Value = deta.ocProcesoLocML.Value;
                mySqlCommandSel.Parameters["@ocProcesoExtME"].Value = deta.ocProcesoExtME.Value;
                mySqlCommandSel.Parameters["@CtoInicialExtME"].Value = deta.CtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@PtoInicialLocML"].Value = deta.PtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@PtoInicialExtME"].Value = deta.PtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@CompInicialocML"].Value = deta.CompInicialocML.Value;
                mySqlCommandSel.Parameters["@CompinicialExtME"].Value = deta.CompinicialExtME.Value;
                mySqlCommandSel.Parameters["@RecInicialLocML"].Value = deta.RecInicialLocML.Value;
                mySqlCommandSel.Parameters["@RecInicialExtME"].Value = deta.RecInicialExtME.Value;
                mySqlCommandSel.Parameters["@ocProcesoInicialLocML"].Value = deta.ocProcesoInicialLocML.Value;
                mySqlCommandSel.Parameters["@ocProcesoInicialExtME"].Value = deta.ocProcesoInicialExtME.Value;
                mySqlCommandSel.Parameters["@UsuarioRevSobreejec"].Value = deta.UsuarioRevSobreejec.Value;
                mySqlCommandSel.Parameters["@FechaRevSobreejec"].Value = deta.FechaRevSobreejec.Value;
                mySqlCommandSel.Parameters["@UsuarioAprSobreejec"].Value = deta.UsuarioAprSobreejec.Value;
                mySqlCommandSel.Parameters["@FechaAprSobreejec"].Value = deta.FechaAprSobreejec.Value;
                mySqlCommandSel.Parameters["@SolicitaPresInd"].Value = deta.SolicitaPresInd.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = deta.Observacion.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="total">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_plSobreEjecucion_UpdateItem(DTO_plSobreEjecucion deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE plSobreEjecucion " +
                    "SET " +
                        " TipoDocumento = @TipoDocumento " +
                        " ,NumeroDoc = @NumeroDoc " +
                        " ,ConsecutivoDetaID = @ConsecutivoDetaID " +
                        " ,FechaDoc = @FechaDoc " +
                        " ,MonedaOrigen = @MonedaOrigen " +
                        " ,AreaAprobacion = @AreaAprobacion " +
                        " ,Estado = @Estado " +
                        " ,TipoAprobacion = @TipoAprobacion " +
                        " ,ProveedorID = @ProveedorID " +
                        " ,TasaOC = @TasaOC " +
                        " ,CantidadDOC = @CantidadDOC " +
                        " ,ValorOCLocML = @ValorOCLocML " +
                        " ,ValorOCLocME = @ValorOCLocME " +
                        " ,ValorOCExtME = @ValorOCExtME " +
                        " ,ValorOCExtML = @ValorOCExtML " +
                        " ,CantidadSOL = @CantidadSOL " +
                        " ,CtoOrigenLocML = @CtoOrigenLocML " +
                        " ,CtoOrigenLocME = @CtoOrigenLocME " +
                        " ,CtoOrigenExtME = @CtoOrigenExtME " +
                        " ,CtoOrigenExtML = @CtoOrigenExtML " +
                        " ,CantidadPTO = @CantidadPTO " +
                        " ,PtoMesLocML = @PtoMesLocML " +
                        " ,PtoMesLocME = @PtoMesLocME " +
                        " ,PtoMesExtME = @PtoMesExtME " +
                        " ,PtoMesExtML = @PtoMesExtML " +
                        " ,PtoTotalLocML = @PtoTotalLocML " +
                        " ,PtoTotalLocME = @PtoTotalLocME " +
                        " ,PtoTotalExtME = @PtoTotalExtME " +
                        " ,PtoTotalExtML = @PtoTotalExtML " +
                        " ,CompActLocML = @CompActLocML " +
                        " ,CompActLocME = @CompActLocME " +
                        " ,CompActExtME = @CompActExtME " +
                        " ,CompActExtML = @CompActExtML " +
                        " ,RecibidoLocML = @RecibidoLocML " +
                        " ,RecibidoLocME = @RecibidoLocME " +
                        " ,RecibidoExtME = @RecibidoExtME " +
                        " ,RecibidoExtML = @RecibidoExtML " +
                        " ,CtoInicialLocML = @CtoInicialLocML " +
                        " ,ocProcesoLocML = @ocProcesoLocML " +
                        " ,ocProcesoExtME = @ocProcesoExtME " +
                        " ,CtoInicialExtME = @CtoInicialExtME " +
                        " ,PtoInicialLocML = @PtoInicialLocML " +
                        " ,PtoInicialExtME = @PtoInicialExtME " +
                        " ,CompInicialocML = @CompInicialocML " +
                        " ,CompinicialExtME = @CompinicialExtME " +
                        " ,RecInicialLocML = @RecInicialLocML " +
                        " ,RecInicialExtME = @RecInicialExtME " +
                        " ,ocProcesoInicialLocML = @ocProcesoInicialLocML " +
                        " ,ocProcesoInicialExtME = @ocProcesoInicialExtME " +
                        " ,UsuarioRevSobreejec = @UsuarioRevSobreejec " +
                        " ,FechaRevSobreejec = @FechaRevSobreejec " +
                        " ,UsuarioAprSobreejec = @UsuarioAprSobreejec " +
                        " ,FechaAprSobreejec = @FechaAprSobreejec " +
                        " ,SolicitaPresInd = @SolicitaPresInd " +
                        " ,Observacion = @Observacion " +
                        " ,eg_glAreaFuncional = @eg_glAreaFuncional " +
                        " ,eg_prProveedor = @eg_prProveedor " +
                   "WHERE EmpresaID= @EmpresaID AND ProyectoID= @ProyectoID AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID " +
                    "   AND CodigoBSID= @CodigoBSID AND PrefijoID= @PrefijoID AND DocumentoNro= @DocumentoNro AND eg_coProyecto=@eg_coProyecto " +
                    "   AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto AND eg_glPrefijo = @eg_glPrefijo " +
                    "   AND eg_prBienServicio=@eg_prBienServicio";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                //Valores
                mySqlCommandSel.Parameters.Add("@TipoDocumento", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaDoc", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@MonedaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoAprobacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TasaOC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadDOC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorOCExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPTO", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoMesExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoTotalExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompActExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecibidoExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompInicialocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CompinicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RecInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ocProcesoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@UsuarioRevSobreejec", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaRevSobreejec", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@UsuarioAprSobreejec", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaAprSobreejec", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@SolicitaPresInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.VarChar);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = deta.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@PrefijoID"].Value = deta.PrefijoID.Value;
                mySqlCommandSel.Parameters["@DocumentoNro"].Value = deta.DocumentoNro.Value;
                //Valores
                mySqlCommandSel.Parameters["@TipoDocumento"].Value = deta.TipoDocumento.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsecutivoDetaID"].Value = deta.ConsecutivoDetaID.Value;
                mySqlCommandSel.Parameters["@FechaDoc"].Value = deta.FechaDoc.Value;
                mySqlCommandSel.Parameters["@MonedaOrigen"].Value = deta.MonedaOrigen.Value;
                mySqlCommandSel.Parameters["@AreaAprobacion"].Value = deta.AreaAprobacion.Value;
                mySqlCommandSel.Parameters["@Estado"].Value = deta.Estado.Value;
                mySqlCommandSel.Parameters["@TipoAprobacion"].Value = deta.TipoAprobacion.Value;
                mySqlCommandSel.Parameters["@ProveedorID"].Value = deta.ProveedorID.Value;
                mySqlCommandSel.Parameters["@TasaOC"].Value = deta.TasaOC.Value;
                mySqlCommandSel.Parameters["@CantidadDOC"].Value = deta.CantidadDOC.Value;
                mySqlCommandSel.Parameters["@ValorOCLocML"].Value = deta.ValorOCLocML.Value;
                mySqlCommandSel.Parameters["@ValorOCLocME"].Value = deta.ValorOCLocME.Value;
                mySqlCommandSel.Parameters["@ValorOCExtME"].Value = deta.ValorOCExtME.Value;
                mySqlCommandSel.Parameters["@ValorOCExtML"].Value = deta.ValorOCExtML.Value;
                mySqlCommandSel.Parameters["@CantidadSOL"].Value = deta.CantidadSOL.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocML"].Value = deta.CtoOrigenLocML.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocME"].Value = deta.CtoOrigenLocME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtME"].Value = deta.CtoOrigenExtME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtML"].Value = deta.CtoOrigenExtML.Value;
                mySqlCommandSel.Parameters["@CantidadPTO"].Value = deta.CantidadPTO.Value;
                mySqlCommandSel.Parameters["@PtoMesLocML"].Value = deta.PtoMesLocML.Value;
                mySqlCommandSel.Parameters["@PtoMesLocME"].Value = deta.PtoMesLocME.Value;
                mySqlCommandSel.Parameters["@PtoMesExtME"].Value = deta.PtoMesExtME.Value;
                mySqlCommandSel.Parameters["@PtoMesExtML"].Value = deta.PtoMesExtML.Value;
                mySqlCommandSel.Parameters["@PtoTotalLocML"].Value = deta.PtoTotalLocML.Value;
                mySqlCommandSel.Parameters["@PtoTotalLocME"].Value = deta.PtoTotalLocME.Value;
                mySqlCommandSel.Parameters["@PtoTotalExtME"].Value = deta.PtoTotalExtME.Value;
                mySqlCommandSel.Parameters["@PtoTotalExtML"].Value = deta.PtoTotalExtML.Value;
                mySqlCommandSel.Parameters["@CompActLocML"].Value = deta.CompActLocML.Value;
                mySqlCommandSel.Parameters["@CompActLocME"].Value = deta.CompActLocME.Value;
                mySqlCommandSel.Parameters["@CompActExtME"].Value = deta.CompActExtME.Value;
                mySqlCommandSel.Parameters["@CompActExtML"].Value = deta.CompActExtML.Value;
                mySqlCommandSel.Parameters["@RecibidoLocML"].Value = deta.RecibidoLocML.Value;
                mySqlCommandSel.Parameters["@RecibidoLocME"].Value = deta.RecibidoLocME.Value;
                mySqlCommandSel.Parameters["@RecibidoExtML"].Value = deta.RecibidoExtML.Value;
                mySqlCommandSel.Parameters["@RecibidoExtME"].Value = deta.RecibidoExtME.Value;
                mySqlCommandSel.Parameters["@CtoInicialLocML"].Value = deta.CtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@ocProcesoLocML"].Value = deta.ocProcesoLocML.Value;
                mySqlCommandSel.Parameters["@ocProcesoExtME"].Value = deta.ocProcesoExtME.Value;
                mySqlCommandSel.Parameters["@CtoInicialExtME"].Value = deta.CtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@PtoInicialLocML"].Value = deta.PtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@PtoInicialExtME"].Value = deta.PtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@CompInicialocML"].Value = deta.CompInicialocML.Value;
                mySqlCommandSel.Parameters["@CompinicialExtME"].Value = deta.CompinicialExtME.Value;
                mySqlCommandSel.Parameters["@RecInicialLocML"].Value = deta.RecInicialLocML.Value;
                mySqlCommandSel.Parameters["@RecInicialExtME"].Value = deta.RecInicialExtME.Value;
                mySqlCommandSel.Parameters["@ocProcesoInicialLocML"].Value = deta.ocProcesoInicialLocML.Value;
                mySqlCommandSel.Parameters["@ocProcesoInicialExtME"].Value = deta.ocProcesoInicialExtME.Value;
                mySqlCommandSel.Parameters["@UsuarioRevSobreejec"].Value = deta.UsuarioRevSobreejec.Value;
                mySqlCommandSel.Parameters["@FechaRevSobreejec"].Value = deta.FechaRevSobreejec.Value;
                mySqlCommandSel.Parameters["@UsuarioAprSobreejec"].Value = deta.UsuarioAprSobreejec.Value;
                mySqlCommandSel.Parameters["@FechaAprSobreejec"].Value = deta.FechaAprSobreejec.Value;
                mySqlCommandSel.Parameters["@SolicitaPresInd"].Value = deta.SolicitaPresInd.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = deta.Observacion.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_UpdateItem");
                throw exception;
            }
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Actualiza un registro de plSobreEjecucion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plSobreEjecucion_Add(DTO_plSobreEjecucion deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                   "SELECT COUNT (*) from plSobreEjecucion with(nolock) " +
                   "WHERE EmpresaID= @EmpresaID AND ProyectoID= @ProyectoID AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID " +
                    "   AND CodigoBSID= @CodigoBSID AND PrefijoID= @PrefijoID AND DocumentoNro= @DocumentoNro AND eg_coProyecto=@eg_coProyecto " +
                    "   AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto AND eg_glPrefijo = @eg_glPrefijo " + 
                    "   AND eg_prBienServicio=@eg_prBienServicio";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocumentoNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = deta.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@PrefijoID"].Value = deta.PrefijoID.Value;
                mySqlCommandSel.Parameters["@DocumentoNro"].Value = deta.DocumentoNro.Value;
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_plSobreEjecucion_AddItem(deta);
                else
                    this.DAL_plSobreEjecucion_UpdateItem(deta);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plSobreEjecucion_Delete(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommandSel.CommandText = "Delete from plSobreEjecucion WHERE NumeroDoc= @NumeroDoc";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el documento asociado a un presupuesto
        /// </summary>
        /// <returns></returns>
        public List<DTO_plSobreEjecucion> DAL_plSobreEjecucion_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_plSobreEjecucion> results = new List<DTO_plSobreEjecucion>();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "select * from plSobreEjecucion with(nolock) WHERE NumeroDoc= @NumeroDoc";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_plSobreEjecucion deta = new DTO_plSobreEjecucion(dr);
                    results.Add(deta);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_Get");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae un listado asociado a un CierreLegalizacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_plSobreEjecucion> DAL_plSobreEjecucion_GetOrdenCompraSobreEjec(int estado, string areaAprob)
        {
            try
            {
                List<DTO_plSobreEjecucion> result = new List<DTO_plSobreEjecucion>();

                SqlCommand mySqlCommandSel = new SqlCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText

                mySqlCommandSel = new SqlCommand("Planeacion_plSobreEjecucion_GetOrdenCompraSobreEjec", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);

                #endregion
                #region Asignacion Valores Parametros

                mySqlCommandSel.Parameters["@Estado"].Value = estado;
                mySqlCommandSel.Parameters["@AreaAprobacion"].Value = areaAprob;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_plSobreEjecucion sobreEjec = null;
                    List<DTO_plSobreEjecucion> list = result.Where(x => ((DTO_plSobreEjecucion)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        sobreEjec = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        sobreEjec = new DTO_plSobreEjecucion(dr);
                        sobreEjec.PrefDoc.Value = sobreEjec.PrefijoID.Value + "-" + sobreEjec.DocumentoNro.Value.ToString();
                    }

                    DTO_plSobreEjecucion sobreEjecDet = new DTO_plSobreEjecucion(dr);
                    sobreEjec.Detalle.Add(sobreEjecDet);

                    if (nuevo)
                        result.Add(sobreEjec);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_GetEstadoEjecByPeriodo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plSobreEjecucion> DAL_plSobreEjecucion_GetByParameter(DTO_plSobreEjecucion filter)
        {
            List<DTO_plSobreEjecucion> results = new List<DTO_plSobreEjecucion>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from plSobreEjecucion with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    query += "and ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LineaPresupuestoID.Value.ToString()))
                {
                    query += "and LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = filter.LineaPresupuestoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value.ToString()))
                {
                    query += "and CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return results;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_plSobreEjecucion total = new DTO_plSobreEjecucion(dr);
                    results.Add(total);
                    index++;
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plSobreEjecucion_GetByParameter");
                throw exception;
            }
        }

        #endregion
    }
}
