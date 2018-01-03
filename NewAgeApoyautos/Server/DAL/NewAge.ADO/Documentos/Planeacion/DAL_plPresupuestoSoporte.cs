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
    public class DAL_plPresupuestoSoporte : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plPresupuestoSoporte(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Privadas

        /// <summary>
        /// Agrega un registro de cierre 
        /// </summary>
        /// <param name="deta">Cierre nuevo</param>
        /// <returns></returns>
        private void DAL_plPresupuestoSoporte_AddItem(DTO_plPresupuestoSoporte deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plPresupuestoSoporte( " +
                    "	EmpresaID,PeriodoID,ProyectoID,CentroCostoID,LineaPresupuestoID, " +
                    "   CuentaID,ComprobanteID,ComprobanteNro,Prefijo,NumDocumento,Fecha," +
                    "	DatoAdd1,DatoAdd2,DatoAdd3,DatoAdd4,DatoAdd5,DatoAdd6,DatoAdd7,DatoAdd8,DatoAdd9,DatoAdd10, " +
                    "   Valor1,Valor2,Valor3,Valor4,Valor5,Valor6,Valor7,Valor8,Valor9,Valor10, " +
                    "	eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto " +
                    ")VALUES( " +
                    "	@EmpresaID,@PeriodoID,@ProyectoID,@CentroCostoID,@LineaPresupuestoID, " +
                    "   @CuentaID,@ComprobanteID,@ComprobanteNro,@Prefijo,@NumDocumento,@Fecha," +
                    "	@DatoAdd1,@DatoAdd2,@DatoAdd3,@DatoAdd4,@DatoAdd5,@DatoAdd6,@DatoAdd7,@DatoAdd8,@DatoAdd9,@DatoAdd10, " +
                    "   @Valor1,@Valor2,@Valor3,@Valor4,@Valor5,@Valor6,@Valor7,@Valor8,@Valor9,@Valor10, " +
                    //"	@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto,@eg_coPlanCuenta " +
                    "	@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto" +
                    ")";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Prefijo", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumDocumento", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd6", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd7", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd8", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd9", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd10", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor9", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor10", SqlDbType.Decimal);
                //mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength); 
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = deta.CuentaID.Value;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = deta.ComprobanteID.Value;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = deta.ComprobanteNro.Value;
                mySqlCommandSel.Parameters["@Prefijo"].Value = deta.Prefijo.Value;
                mySqlCommandSel.Parameters["@NumDocumento"].Value = deta.NumDocumento.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = deta.Fecha.Value;
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = deta.DatoAdd1.Value;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value = deta.DatoAdd2.Value;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value = deta.DatoAdd3.Value;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value = deta.DatoAdd4.Value;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value = deta.DatoAdd5.Value;
                mySqlCommandSel.Parameters["@DatoAdd6"].Value = deta.DatoAdd6.Value;
                mySqlCommandSel.Parameters["@DatoAdd7"].Value = deta.DatoAdd7.Value;
                mySqlCommandSel.Parameters["@DatoAdd8"].Value = deta.DatoAdd8.Value;
                mySqlCommandSel.Parameters["@DatoAdd9"].Value = deta.DatoAdd9.Value;
                mySqlCommandSel.Parameters["@DatoAdd10"].Value = deta.DatoAdd10.Value;
                mySqlCommandSel.Parameters["@Valor1"].Value = deta.Valor1.Value;
                mySqlCommandSel.Parameters["@Valor2"].Value = deta.Valor2.Value;
                mySqlCommandSel.Parameters["@Valor3"].Value = deta.Valor3.Value;
                mySqlCommandSel.Parameters["@Valor4"].Value = deta.Valor4.Value;
                mySqlCommandSel.Parameters["@Valor5"].Value = deta.Valor5.Value;
                mySqlCommandSel.Parameters["@Valor6"].Value = deta.Valor6.Value;
                mySqlCommandSel.Parameters["@Valor7"].Value = deta.Valor7.Value;
                mySqlCommandSel.Parameters["@Valor8"].Value = deta.Valor8.Value;
                mySqlCommandSel.Parameters["@Valor9"].Value = deta.Valor9.Value;
                mySqlCommandSel.Parameters["@Valor10"].Value = deta.Valor10.Value;
                //mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl); 
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoSoporte_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="total">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_plPresupuestoSoporte_UpdateItem(DTO_plPresupuestoSoporte deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE plPresupuestoSoporte " +
                    "   SET " +
                    "       CuentaID = @CuentaID " +
                    "       ,ComprobanteID = @ComprobanteID " +
                    "       ,ComprobanteNro = @ComprobanteNro " +
                    "       ,Prefijo = @Prefijo " +
                    "       ,NumDocumento = @NumDocumento " +
                    "       ,DatoAdd1 = @DatoAdd1 " +
                    "       ,DatoAdd2 = @DatoAdd2 " +
                    "       ,DatoAdd3 = @DatoAdd3 " +
                    "       ,DatoAdd4 = @DatoAdd4 " +
                    "       ,DatoAdd5 = @DatoAdd5 " +
                    "       ,DatoAdd6 = @DatoAdd6 " +
                    "       ,DatoAdd7 = @DatoAdd7 " +
                    "       ,DatoAdd8 = @DatoAdd8 " +
                    "       ,DatoAdd9 = @DatoAdd9 " +
                    "       ,DatoAdd10 = @DatoAdd10 " +
                    "       ,Valor1 = Valor1 + @Valor1 " +
                    "       ,Valor2 = Valor2 + @Valor2 " +
                    "       ,Valor3 = Valor3 + @Valor3 " +
                    "       ,Valor4 = Valor4 + @Valor4 " +
                    "       ,Valor5 = Valor5 + @Valor5 " +
                    "       ,Valor6 = Valor6 + @Valor6 " +
                    "       ,Valor7 = Valor7 + @Valor7 " +
                    "       ,Valor8 = Valor8 + @Valor8 " +
                    "       ,Valor9 = Valor9 + @Valor9 " +
                    "       ,Valor10 = Valor10 + @Valor10 " +
                    //"       ,eg_coPlanCuenta = @eg_coPlanCuenta " +
                   "WHERE EmpresaID= @EmpresaID AND PeriodoID= @PeriodoID AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID " +
                    "   AND eg_coProyecto=@eg_coProyecto AND eg_coCentroCosto=@eg_coCentroCosto AND eg_plLineaPresupuesto=@eg_plLineaPresupuesto";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteID", SqlDbType.Char, UDT_ComprobanteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComprobanteNro", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Prefijo", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumDocumento", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DatoAdd1", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd2", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd3", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd4", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd5", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd6", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd7", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd8", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd9", SqlDbType.Char, 30);
                mySqlCommandSel.Parameters.Add("@DatoAdd10", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor1", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor2", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor3", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor4", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor5", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor6", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor7", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor8", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor9", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor10", SqlDbType.Decimal);
                //mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength); 
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = deta.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = deta.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = deta.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = deta.CuentaID.Value;
                mySqlCommandSel.Parameters["@ComprobanteID"].Value = deta.ComprobanteID.Value;
                mySqlCommandSel.Parameters["@ComprobanteNro"].Value = deta.ComprobanteNro.Value;
                mySqlCommandSel.Parameters["@Prefijo"].Value = deta.Prefijo.Value;
                mySqlCommandSel.Parameters["@NumDocumento"].Value = deta.NumDocumento.Value;
                mySqlCommandSel.Parameters["@DatoAdd1"].Value = deta.DatoAdd1.Value;
                mySqlCommandSel.Parameters["@DatoAdd2"].Value = deta.DatoAdd2.Value;
                mySqlCommandSel.Parameters["@DatoAdd3"].Value = deta.DatoAdd3.Value;
                mySqlCommandSel.Parameters["@DatoAdd4"].Value = deta.DatoAdd4.Value;
                mySqlCommandSel.Parameters["@DatoAdd5"].Value = deta.DatoAdd5.Value;
                mySqlCommandSel.Parameters["@DatoAdd6"].Value = deta.DatoAdd6.Value;
                mySqlCommandSel.Parameters["@DatoAdd7"].Value = deta.DatoAdd7.Value;
                mySqlCommandSel.Parameters["@DatoAdd8"].Value = deta.DatoAdd8.Value;
                mySqlCommandSel.Parameters["@DatoAdd9"].Value = deta.DatoAdd9.Value;
                mySqlCommandSel.Parameters["@DatoAdd10"].Value = deta.DatoAdd10.Value;
                mySqlCommandSel.Parameters["@Valor1"].Value = deta.Valor1.Value;
                mySqlCommandSel.Parameters["@Valor2"].Value = deta.Valor2.Value;
                mySqlCommandSel.Parameters["@Valor3"].Value = deta.Valor3.Value;
                mySqlCommandSel.Parameters["@Valor4"].Value = deta.Valor4.Value;
                mySqlCommandSel.Parameters["@Valor5"].Value = deta.Valor5.Value;
                mySqlCommandSel.Parameters["@Valor6"].Value = deta.Valor6.Value;
                mySqlCommandSel.Parameters["@Valor7"].Value = deta.Valor7.Value;
                mySqlCommandSel.Parameters["@Valor8"].Value = deta.Valor8.Value;
                mySqlCommandSel.Parameters["@Valor9"].Value = deta.Valor9.Value;
                mySqlCommandSel.Parameters["@Valor10"].Value = deta.Valor10.Value;
                //mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl); 
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoSoporte_UpdateItem");
                throw exception;
            }
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Actualiza un registro de plPresupuestoSoporte
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoSoporte_Add(DTO_plPresupuestoSoporte deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                   "SELECT COUNT (*) from plPresupuestoSoporte with(nolock) " +
                   "WHERE EmpresaID= @EmpresaID AND PeriodoID= @PeriodoID AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID " +
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
                mySqlCommandSel.Parameters["@PeriodoID"].Value = deta.PeriodoID.Value; 
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
                    this.DAL_plPresupuestoSoporte_AddItem(deta);
                else
                    this.DAL_plPresupuestoSoporte_UpdateItem(deta);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoSoporte_Add");
                throw exception;
            }
        }

        #endregion

    }
}
