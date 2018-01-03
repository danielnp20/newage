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
    public class DAL_plCierreLegalizacion : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plCierreLegalizacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Privadas

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="deta">Cierre nuevo</param>
        /// <returns></returns>
        private void DAL_plCierreLegalizacion_AddItem(DTO_plCierreLegalizacion deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plCierreLegalizacion " +
                    "( " +
                        " EmpresaID " +
                        " ,PeriodoID " +
                        " ,ProyectoID " +
                        " ,CentroCostoID " +
                        " ,LineaPresupuestoID " +
                        " ,CtoOrigenLocML " +
                        " ,CtoOrigenLocME " +
                        " ,CtoOrigenExtME " +
                        " ,CtoOrigenExtML " +
                        " ,CtoInicialLocML " +
                        " ,CtoInicialLocME " +
                        " ,CtoInicialExtME " +
                        " ,CtoInicialExtML " +
                        " ,PtoInicialLocML " +
                        " ,PtoInicialLocME " +
                        " ,PtoInicialExtME " +
                        " ,PtoInicialExtML " +
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
                        " ,RecibidoExtML " +
                        " ,RecibidoExtME " +
                        " ,eg_coProyecto " +
                        " ,eg_coCentroCosto " +
                        " ,eg_plLineaPresupuesto " +
                    ") " +
                    "VALUES " +
                    "( " +
                        " @EmpresaID " +
                        " ,@PeriodoID " +
                        " ,@ProyectoID " +
                        " ,@CentroCostoID " +
                        " ,@LineaPresupuestoID " +
                        " ,@CtoOrigenLocML " +
                        " ,@CtoOrigenLocME " +
                        " ,@CtoOrigenExtME " +
                        " ,@CtoOrigenExtML " +
                        " ,@CtoInicialLocML " +
                        " ,@CtoInicialLocME " +
                        " ,@CtoInicialExtME " +
                        " ,@CtoInicialExtML " +
                        " ,@PtoInicialLocML " +
                        " ,@PtoInicialLocME " +
                        " ,@PtoInicialExtME " +
                        " ,@PtoInicialExtML " +
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
                        " ,@RecibidoExtML " +
                        " ,@RecibidoExtME " +
                        " ,@eg_coProyecto " +
                        " ,@eg_coCentroCosto " +
                        " ,@eg_plLineaPresupuesto " +
                    ") ";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialExtML", SqlDbType.Decimal);
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
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocML"].Value = deta.CtoOrigenLocML.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocME"].Value = deta.CtoOrigenLocME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtME"].Value = deta.CtoOrigenExtME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtML"].Value = deta.CtoOrigenExtML.Value;
                mySqlCommandSel.Parameters["@CtoInicialLocML"].Value = deta.CtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@CtoInicialLocME"].Value = deta.CtoInicialLocME.Value;
                mySqlCommandSel.Parameters["@CtoInicialExtME"].Value = deta.CtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@CtoInicialExtML"].Value = deta.CtoInicialExtML.Value;
                mySqlCommandSel.Parameters["@PtoInicialLocML"].Value = deta.PtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@PtoInicialLocME"].Value = deta.PtoInicialLocME.Value;
                mySqlCommandSel.Parameters["@PtoInicialExtME"].Value = deta.PtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@PtoInicialExtML"].Value = deta.PtoInicialExtML.Value;
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
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="total">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_plCierreLegalizacion_UpdateItem(DTO_plCierreLegalizacion deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE plCierreLegalizacion " +
                    "SET " +
                    "   CtoOrigenLocML = CtoOrigenLocML + @CtoOrigenLocML, " +
                    "   CtoOrigenLocME = CtoOrigenLocME + @CtoOrigenLocME, " +
                    "   CtoOrigenExtME = CtoOrigenExtME + @CtoOrigenExtME, " +
                    "   CtoOrigenExtML = CtoOrigenExtML + @CtoOrigenExtML, " +
                    "   CtoInicialLocML = CtoInicialLocML + @CtoInicialLocML, " +
                    "   CtoInicialLocME = CtoInicialLocME + @CtoInicialLocME, " +
                    "   CtoInicialExtME = CtoInicialExtME + @CtoInicialExtME, " +
                    "   CtoInicialExtML = CtoInicialExtML + @CtoInicialExtML, " +
                    "   PtoInicialLocML = PtoInicialLocML + @PtoInicialLocML, " +
                    "   PtoInicialLocME = PtoInicialLocME + @PtoInicialLocME, " +
                    "   PtoInicialExtME = PtoInicialExtME + @PtoInicialExtME, " +
                    "   PtoInicialExtML = PtoInicialExtML + @PtoInicialExtML, " +
                    "   PtoMesLocML = PtoMesLocML + @PtoMesLocML, " +
                    "   PtoMesLocME = PtoMesLocME + @PtoMesLocME, " +
                    "   PtoMesExtME = PtoMesExtME + @PtoMesExtME, " +
                    "   PtoMesExtML = PtoMesExtML + @PtoMesExtML, " +
                    "   PtoTotalLocML = PtoTotalLocML + @PtoTotalLocML, " +
                    "   PtoTotalLocME = PtoTotalLocME + @PtoTotalLocME, " +
                    "   PtoTotalExtME = PtoTotalExtME + @PtoTotalExtME, " +
                    "   PtoTotalExtML = PtoTotalExtML + @PtoTotalExtML, " +
                    "   CompActLocML = CompActLocML + @CompActLocML, " +
                    "   CompActLocME = CompActLocME + @CompActLocME, " +
                    "   CompActExtME = CompActExtME + @CompActExtME, " +
                    "   CompActExtML = CompActExtML + @CompActExtML, " +
                    "   RecibidoLocML = RecibidoLocML + @RecibidoLocML, " +
                    "   RecibidoLocME = RecibidoLocME + @RecibidoLocME, " +
                    "   RecibidoExtML = RecibidoExtML + @RecibidoExtML, " +
                    "   RecibidoExtME = RecibidoExtME + @RecibidoExtME " +
                    "WHERE EmpresaID= @EmpresaID AND PeriodoID= @PeriodoID AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID" +
                    "   AND eg_coProyecto=@eg_coProyecto AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoOrigenExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CtoInicialExtML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialLocML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialLocME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialExtME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@PtoInicialExtML", SqlDbType.Decimal);
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
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocML"].Value = deta.CtoOrigenLocML.Value;
                mySqlCommandSel.Parameters["@CtoOrigenLocME"].Value = deta.CtoOrigenLocME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtME"].Value = deta.CtoOrigenExtME.Value;
                mySqlCommandSel.Parameters["@CtoOrigenExtML"].Value = deta.CtoOrigenExtML.Value;
                mySqlCommandSel.Parameters["@CtoInicialLocML"].Value = deta.CtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@CtoInicialLocME"].Value = deta.CtoInicialLocME.Value;
                mySqlCommandSel.Parameters["@CtoInicialExtME"].Value = deta.CtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@CtoInicialExtML"].Value = deta.CtoInicialExtML.Value;
                mySqlCommandSel.Parameters["@PtoInicialLocML"].Value = deta.PtoInicialLocML.Value;
                mySqlCommandSel.Parameters["@PtoInicialLocME"].Value = deta.PtoInicialLocME.Value;
                mySqlCommandSel.Parameters["@PtoInicialExtME"].Value = deta.PtoInicialExtME.Value;
                mySqlCommandSel.Parameters["@PtoInicialExtML"].Value = deta.PtoInicialExtML.Value;
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
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                #endregion

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_UpdateItem");
                throw exception;
            }
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Actualiza un registro de plCierreLegalizacion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plCierreLegalizacion_Add(DTO_plCierreLegalizacion deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from plCierreLegalizacion with(nolock) " +
                    "WHERE EmpresaID= @EmpresaID AND PeriodoID= @PeriodoID AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID" +
                    "   AND eg_coProyecto=@eg_coProyecto AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_plCierreLegalizacion_AddItem(deta);
                else
                {
                    #region Verifica que exista Saldo Disponible por Mes (En comentarios)
                    //DTO_plCierreLegalizacion exist = this.DAL_plCierreLegalizacion_GetByParameter(deta).First();
                    //if ((exist.CtoOrigenLocML.Value + deta.CtoOrigenLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoOrigenLocME.Value + deta.CtoOrigenLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoOrigenExtME.Value + deta.CtoOrigenExtME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoOrigenExtML.Value + deta.CtoOrigenExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoInicialLocML.Value + deta.CtoInicialLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoInicialLocME.Value + deta.CtoInicialLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoInicialExtME.Value + deta.CtoInicialExtME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CtoInicialExtML.Value + deta.CtoInicialExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoInicialLocML.Value + deta.PtoInicialLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoInicialLocME.Value + deta.PtoInicialLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoInicialExtME.Value + deta.PtoInicialExtME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoInicialExtML.Value + deta.PtoInicialExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoMesLocML.Value + deta.PtoMesLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoMesLocME.Value + deta.PtoMesLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoMesExtME.Value + deta.PtoMesExtME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoMesExtML.Value + deta.PtoMesExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoTotalLocML.Value + deta.PtoTotalLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoTotalLocME.Value + deta.PtoTotalLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoTotalExtME.Value + deta.PtoTotalExtME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.PtoTotalExtML.Value + deta.PtoTotalExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CompActLocML.Value + deta.CompActLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CompActLocME.Value + deta.CompActLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CompActExtME.Value + deta.CompActExtME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.CompActExtML.Value + deta.CompActExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.RecibidoLocML.Value + deta.RecibidoLocML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.RecibidoLocME.Value + deta.RecibidoLocME.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.RecibidoExtML.Value + deta.RecibidoExtML.Value) < 0) validateSaldoMensual = false;
                    //else if ((exist.RecibidoExtME.Value + deta.RecibidoExtME.Value) < 0) validateSaldoMensual = false;
                    #endregion
                    this.DAL_plCierreLegalizacion_UpdateItem(deta);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plCierreLegalizacion_Delete(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommandSel.CommandText = "Delete from plCierreLegalizacion WHERE NumeroDoc= @NumeroDoc";
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el documento asociado a un presupuesto
        /// </summary>
        /// <returns></returns>
        public List<DTO_plCierreLegalizacion> DAL_plCierreLegalizacion_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_plCierreLegalizacion> results = new List<DTO_plCierreLegalizacion>();

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "select * from plCierreLegalizacion with(nolock) WHERE NumeroDoc= @NumeroDoc";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_plCierreLegalizacion deta = new DTO_plCierreLegalizacion(dr);
                    results.Add(deta);
                }

                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_Get");
                throw exception;
            }
        }

        #endregion

        #region Otras 

        /// <summary>
        /// Ejecutar proceso de cierre Legalizacion en Planeacion 
        /// </summary>
        /// <param name="periodo">Periodo de Cierre</param>
        public void DAL_plCierreLegalizacion_ProcesoCierre(DateTime periodo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Planeacion_CierreLegalizacion", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_ProcesoCierre");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado asociado a un CierreLegalizacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_QueryInformeMensualCierre> DAL_plCierreLegalizacion_GetInfoMensual(DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType,TipoMoneda_LocExt tipoModa, TipoMoneda mdaOrigen)
        {

            try
            {
                List<DTO_QueryInformeMensualCierre> result = new List<DTO_QueryInformeMensualCierre>();

                SqlCommand mySqlCommandSel = new SqlCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
                #region CommandText

                mySqlCommandSel = new SqlCommand("Planeacion_plCierreLegalizacion_GetInfoMes", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoInforme", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoMda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);                
                mySqlCommandSel.Parameters.Add("@ProyectoTipo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActividadID", SqlDbType.VarChar, UDT_ActividadID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPres", SqlDbType.VarChar, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCost", SqlDbType.VarChar, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecurGrupo", SqlDbType.VarChar, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.VarChar, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoID", SqlDbType.VarChar, UDT_ContratoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Pozo", SqlDbType.VarChar, UDT_LocFisicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Campo", SqlDbType.VarChar, UDT_AreaFisicaID.MaxLength);

                #endregion
                #region Asignacion Valores Parametros

                mySqlCommandSel.Parameters["@Year"].Value = filter.PeriodoID.Value.Value.Year;
                mySqlCommandSel.Parameters["@Month"].Value = filter.PeriodoID.Value.Value.Month;
                mySqlCommandSel.Parameters["@TipoInforme"].Value = tipoInforme;
                mySqlCommandSel.Parameters["@TipoMda"].Value = (byte)tipoModa;
                mySqlCommandSel.Parameters["@MdaOrigen"].Value = (byte)mdaOrigen;          
                mySqlCommandSel.Parameters["@ProyectoTipo"].Value = (byte)proType;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                mySqlCommandSel.Parameters["@ActividadID"].Value = filter.ActividadID.Value;
                mySqlCommandSel.Parameters["@LineaPres"].Value = filter.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CentroCost"].Value = filter.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@RecurGrupo"].Value = filter.Grupo.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = filter.RecursoID.Value;
                mySqlCommandSel.Parameters["@ContratoID"].Value = filter.ContratoID.Value;
                mySqlCommandSel.Parameters["@Pozo"].Value = filter.Pozo.Value;
                mySqlCommandSel.Parameters["@Campo"].Value = filter.Campo.Value;

                #endregion

                DTO_QueryInformeMensualCierre presupuesto = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    presupuesto = new DTO_QueryInformeMensualCierre(dr);
                    result.Add(presupuesto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_GetParameterQuery");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado asociado a un CierreLegalizacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_QueryEstadoEjecucion> DAL_plCierreLegalizacion_GetEstadoEjecByPeriodo(DTO_QueryEstadoEjecucion filter, ProyectoTipo proType, TipoMoneda_LocExt tipoModa, TipoMoneda mdaOrigen)
        {

            try
            {
                List<DTO_QueryEstadoEjecucion> result = new List<DTO_QueryEstadoEjecucion>();

                SqlCommand mySqlCommandSel = new SqlCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText

                mySqlCommandSel = new SqlCommand("Planeacion_plCierreLegalizacion_GetEstadoEjecByPeriodo", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoMda", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@MdaOrigen", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ProyectoTipo", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.VarChar, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ActividadID", SqlDbType.VarChar, UDT_ActividadID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPres", SqlDbType.VarChar, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCost", SqlDbType.VarChar, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecurGrupo", SqlDbType.VarChar, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.VarChar, UDT_RecursoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoID", SqlDbType.VarChar, UDT_ContratoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Pozo", SqlDbType.VarChar, UDT_LocFisicaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Campo", SqlDbType.VarChar, UDT_AreaFisicaID.MaxLength);

                #endregion
                #region Asignacion Valores Parametros

                mySqlCommandSel.Parameters["@Year"].Value = filter.PeriodoID.Value.Value.Year;
                mySqlCommandSel.Parameters["@Month"].Value = filter.PeriodoID.Value.Value.Month;
                mySqlCommandSel.Parameters["@TipoMda"].Value = (byte)tipoModa;
                mySqlCommandSel.Parameters["@MdaOrigen"].Value = (byte)mdaOrigen;
                mySqlCommandSel.Parameters["@ProyectoTipo"].Value = (byte)proType;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                mySqlCommandSel.Parameters["@ActividadID"].Value = filter.ActividadID.Value;
                mySqlCommandSel.Parameters["@LineaPres"].Value = filter.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CentroCost"].Value = filter.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@RecurGrupo"].Value = filter.Grupo.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = filter.RecursoID.Value;
                mySqlCommandSel.Parameters["@ContratoID"].Value = filter.ContratoID.Value;
                mySqlCommandSel.Parameters["@Pozo"].Value = filter.Pozo.Value;
                mySqlCommandSel.Parameters["@Campo"].Value = filter.Campo.Value;

                #endregion

                DTO_QueryEstadoEjecucion presupuesto = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    presupuesto = new DTO_QueryEstadoEjecucion(dr);
                    result.Add(presupuesto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_GetEstadoEjecByPeriodo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plCierreLegalizacion> DAL_plCierreLegalizacion_GetByParameter(DTO_plCierreLegalizacion filter)
        {
            List<DTO_plCierreLegalizacion> results = new List<DTO_plCierreLegalizacion>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from plCierreLegalizacion with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.PeriodoID.Value.ToString()))
                {
                    query += "and PeriodoID = @PeriodoID ";
                    mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@PeriodoID"].Value = filter.PeriodoID.Value.Value;
                    filterInd = true;
                }
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
                    DTO_plCierreLegalizacion total = new DTO_plCierreLegalizacion(dr);
                    results.Add(total);
                    index++;
                }
                dr.Close();                

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plCierreLegalizacion_GetByParameter");
                throw exception;
            }
        }

        #endregion
    }
}
