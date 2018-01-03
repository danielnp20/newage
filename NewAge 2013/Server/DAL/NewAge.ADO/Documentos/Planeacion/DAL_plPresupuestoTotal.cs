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
    public class DAL_plPresupuestoTotal : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_plPresupuestoTotal(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla plPresupuestoTotal
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private void DAL_plPresupuestoTotal_AddItem(DTO_plPresupuestoTotal total)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO plPresupuestoTotal " +
                    "( " +
                        "EmpresaID,Ano,ProyectoID,CentroCostoID,LineaPresupuestoID," +
                        "ValorLoc00,ValorLoc01,ValorLoc02,ValorLoc03,ValorLoc04,ValorLoc05,ValorLoc06,ValorLoc07,ValorLoc08,ValorLoc09,ValorLoc10,ValorLoc11,ValorLoc12," +
                        "EquivExt00,EquivExt01,EquivExt02,EquivExt03,EquivExt04,EquivExt05,EquivExt06,EquivExt07,EquivExt08,EquivExt09,EquivExt10,EquivExt11,EquivExt12," +
                        "ValorExt00,ValorExt01,ValorExt02,ValorExt03,ValorExt04,ValorExt05,ValorExt06,ValorExt07,ValorExt08,ValorExt09,ValorExt10,ValorExt11,ValorExt12," +
                        "EquivLoc00,EquivLoc01,EquivLoc02,EquivLoc03,EquivLoc04,EquivLoc05,EquivLoc06,EquivLoc07,EquivLoc08,EquivLoc09,EquivLoc10,EquivLoc11,EquivLoc12," +
                        "eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto" +
                    ") " +
                    "VALUES " +
                    "( " +
                        "@EmpresaID,@Ano,@ProyectoID,@CentroCostoID,@LineaPresupuestoID," +
                        "@ValorLoc00,@ValorLoc01,@ValorLoc02,@ValorLoc03,@ValorLoc04,@ValorLoc05,@ValorLoc06,@ValorLoc07,@ValorLoc08,@ValorLoc09,@ValorLoc10,@ValorLoc11,@ValorLoc12," +
                        "@EquivExt00,@EquivExt01,@EquivExt02,@EquivExt03,@EquivExt04,@EquivExt05,@EquivExt06,@EquivExt07,@EquivExt08,@EquivExt09,@EquivExt10,@EquivExt11,@EquivExt12," +
                        "@ValorExt00,@ValorExt01,@ValorExt02,@ValorExt03,@ValorExt04,@ValorExt05,@ValorExt06,@ValorExt07,@ValorExt08,@ValorExt09,@ValorExt10,@ValorExt11,@ValorExt12," +
                        "@EquivLoc00,@EquivLoc01,@EquivLoc02,@EquivLoc03,@EquivLoc04,@EquivLoc05,@EquivLoc06,@EquivLoc07,@EquivLoc08,@EquivLoc09,@EquivLoc10,@EquivLoc11,@EquivLoc12," +
                        "@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto" +
                    ") ";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                //Valores
                mySqlCommandSel.Parameters.Add("@ValorLoc00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc12", SqlDbType.Decimal);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = total.Ano.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = total.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = total.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = total.LineaPresupuestoID.Value;
                //Valores
                mySqlCommandSel.Parameters["@ValorLoc00"].Value = total.ValorLoc00.Value;
                mySqlCommandSel.Parameters["@ValorLoc01"].Value = total.ValorLoc01.Value;
                mySqlCommandSel.Parameters["@ValorLoc02"].Value = total.ValorLoc02.Value;
                mySqlCommandSel.Parameters["@ValorLoc03"].Value = total.ValorLoc03.Value;
                mySqlCommandSel.Parameters["@ValorLoc04"].Value = total.ValorLoc04.Value;
                mySqlCommandSel.Parameters["@ValorLoc05"].Value = total.ValorLoc05.Value;
                mySqlCommandSel.Parameters["@ValorLoc06"].Value = total.ValorLoc06.Value;
                mySqlCommandSel.Parameters["@ValorLoc07"].Value = total.ValorLoc07.Value;
                mySqlCommandSel.Parameters["@ValorLoc08"].Value = total.ValorLoc08.Value;
                mySqlCommandSel.Parameters["@ValorLoc09"].Value = total.ValorLoc09.Value;
                mySqlCommandSel.Parameters["@ValorLoc10"].Value = total.ValorLoc10.Value;
                mySqlCommandSel.Parameters["@ValorLoc11"].Value = total.ValorLoc11.Value;
                mySqlCommandSel.Parameters["@ValorLoc12"].Value = total.ValorLoc12.Value;
                mySqlCommandSel.Parameters["@EquivExt00"].Value = total.EquivExt00.Value;
                mySqlCommandSel.Parameters["@EquivExt01"].Value = total.EquivExt01.Value;
                mySqlCommandSel.Parameters["@EquivExt02"].Value = total.EquivExt02.Value;
                mySqlCommandSel.Parameters["@EquivExt03"].Value = total.EquivExt03.Value;
                mySqlCommandSel.Parameters["@EquivExt04"].Value = total.EquivExt04.Value;
                mySqlCommandSel.Parameters["@EquivExt05"].Value = total.EquivExt05.Value;
                mySqlCommandSel.Parameters["@EquivExt06"].Value = total.EquivExt06.Value;
                mySqlCommandSel.Parameters["@EquivExt07"].Value = total.EquivExt07.Value;
                mySqlCommandSel.Parameters["@EquivExt08"].Value = total.EquivExt08.Value;
                mySqlCommandSel.Parameters["@EquivExt09"].Value = total.EquivExt09.Value;
                mySqlCommandSel.Parameters["@EquivExt10"].Value = total.EquivExt10.Value;
                mySqlCommandSel.Parameters["@EquivExt11"].Value = total.EquivExt11.Value;
                mySqlCommandSel.Parameters["@EquivExt12"].Value = total.EquivExt12.Value;
                mySqlCommandSel.Parameters["@ValorExt00"].Value = total.ValorExt00.Value;
                mySqlCommandSel.Parameters["@ValorExt01"].Value = total.ValorExt01.Value;
                mySqlCommandSel.Parameters["@ValorExt02"].Value = total.ValorExt02.Value;
                mySqlCommandSel.Parameters["@ValorExt03"].Value = total.ValorExt03.Value;
                mySqlCommandSel.Parameters["@ValorExt04"].Value = total.ValorExt04.Value;
                mySqlCommandSel.Parameters["@ValorExt05"].Value = total.ValorExt05.Value;
                mySqlCommandSel.Parameters["@ValorExt06"].Value = total.ValorExt06.Value;
                mySqlCommandSel.Parameters["@ValorExt07"].Value = total.ValorExt07.Value;
                mySqlCommandSel.Parameters["@ValorExt08"].Value = total.ValorExt08.Value;
                mySqlCommandSel.Parameters["@ValorExt09"].Value = total.ValorExt09.Value;
                mySqlCommandSel.Parameters["@ValorExt10"].Value = total.ValorExt10.Value;
                mySqlCommandSel.Parameters["@ValorExt11"].Value = total.ValorExt11.Value;
                mySqlCommandSel.Parameters["@ValorExt12"].Value = total.ValorExt12.Value;
                mySqlCommandSel.Parameters["@EquivLoc00"].Value = total.EquivLoc00.Value;
                mySqlCommandSel.Parameters["@EquivLoc01"].Value = total.EquivLoc01.Value;
                mySqlCommandSel.Parameters["@EquivLoc02"].Value = total.EquivLoc02.Value;
                mySqlCommandSel.Parameters["@EquivLoc03"].Value = total.EquivLoc03.Value;
                mySqlCommandSel.Parameters["@EquivLoc04"].Value = total.EquivLoc04.Value;
                mySqlCommandSel.Parameters["@EquivLoc05"].Value = total.EquivLoc05.Value;
                mySqlCommandSel.Parameters["@EquivLoc06"].Value = total.EquivLoc06.Value;
                mySqlCommandSel.Parameters["@EquivLoc07"].Value = total.EquivLoc07.Value;
                mySqlCommandSel.Parameters["@EquivLoc08"].Value = total.EquivLoc08.Value;
                mySqlCommandSel.Parameters["@EquivLoc09"].Value = total.EquivLoc09.Value;
                mySqlCommandSel.Parameters["@EquivLoc10"].Value = total.EquivLoc10.Value;
                mySqlCommandSel.Parameters["@EquivLoc11"].Value = total.EquivLoc11.Value;
                mySqlCommandSel.Parameters["@EquivLoc12"].Value = total.EquivLoc12.Value;
                //Eg
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoDeta_AddItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="total">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_plPresupuestoTotal_UpdateItem(DTO_plPresupuestoTotal total)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE plPresupuestoTotal " +
                    "SET " + 
                    "   ValorLoc00 = ValorLoc00 + @ValorLoc00, " +
                    "   ValorLoc01 = ValorLoc01 + @ValorLoc01, " +
                    "   ValorLoc02 = ValorLoc02 + @ValorLoc02, " +
                    "   ValorLoc03 = ValorLoc03 + @ValorLoc03, " +
                    "   ValorLoc04 = ValorLoc04 + @ValorLoc04, " +
                    "   ValorLoc05 = ValorLoc05 + @ValorLoc05, " +
                    "   ValorLoc06 = ValorLoc06 + @ValorLoc06, " +
                    "   ValorLoc07 = ValorLoc07 + @ValorLoc07, " +
                    "   ValorLoc08 = ValorLoc08 + @ValorLoc08, " +
                    "   ValorLoc09 = ValorLoc09 + @ValorLoc09, " +
                    "   ValorLoc10 = ValorLoc10 + @ValorLoc10, " +
                    "   ValorLoc11 = ValorLoc11 + @ValorLoc11, " +
                    "   ValorLoc12 = ValorLoc12 + @ValorLoc12, " +
                    "   EquivExt00 = EquivExt00 + @EquivExt00, " +
                    "   EquivExt01 = EquivExt01 + @EquivExt01, " +
                    "   EquivExt02 = EquivExt02 + @EquivExt02, " +
                    "   EquivExt03 = EquivExt03 + @EquivExt03, " +
                    "   EquivExt04 = EquivExt04 + @EquivExt04, " +
                    "   EquivExt05 = EquivExt05 + @EquivExt05, " +
                    "   EquivExt06 = EquivExt06 + @EquivExt06, " +
                    "   EquivExt07 = EquivExt07 + @EquivExt07, " +
                    "   EquivExt08 = EquivExt08 + @EquivExt08, " +
                    "   EquivExt09 = EquivExt09 + @EquivExt09, " +
                    "   EquivExt10 = EquivExt10 + @EquivExt10, " +
                    "   EquivExt11 = EquivExt11 + @EquivExt11, " +
                    "   EquivExt12 = EquivExt12 + @EquivExt12, " +
                    "   ValorExt00 = ValorExt00 + @ValorExt00, " +
                    "   ValorExt01 = ValorExt01 + @ValorExt01, " +
                    "   ValorExt02 = ValorExt02 + @ValorExt02, " +
                    "   ValorExt03 = ValorExt03 + @ValorExt03, " +
                    "   ValorExt04 = ValorExt04 + @ValorExt04, " +
                    "   ValorExt05 = ValorExt05 + @ValorExt05, " +
                    "   ValorExt06 = ValorExt06 + @ValorExt06, " +
                    "   ValorExt07 = ValorExt07 + @ValorExt07, " +
                    "   ValorExt08 = ValorExt08 + @ValorExt08, " +
                    "   ValorExt09 = ValorExt09 + @ValorExt09, " +
                    "   ValorExt10 = ValorExt10 + @ValorExt10, " +
                    "   ValorExt11 = ValorExt11 + @ValorExt11, " +
                    "   ValorExt12 = ValorExt12 + @ValorExt12, " +
                    "   EquivLoc00 = EquivLoc00 + @EquivLoc00, " +
                    "   EquivLoc01 = EquivLoc01 + @EquivLoc01, " +
                    "   EquivLoc02 = EquivLoc02 + @EquivLoc02, " +
                    "   EquivLoc03 = EquivLoc03 + @EquivLoc03, " +
                    "   EquivLoc04 = EquivLoc04 + @EquivLoc04, " +
                    "   EquivLoc05 = EquivLoc05 + @EquivLoc05, " +
                    "   EquivLoc06 = EquivLoc06 + @EquivLoc06, " +
                    "   EquivLoc07 = EquivLoc07 + @EquivLoc07, " +
                    "   EquivLoc08 = EquivLoc08 + @EquivLoc08, " +
                    "   EquivLoc09 = EquivLoc09 + @EquivLoc09, " +
                    "   EquivLoc10 = EquivLoc10 + @EquivLoc10, " +
                    "   EquivLoc11 = EquivLoc11 + @EquivLoc11, " +
                    "   EquivLoc12 = EquivLoc12 + @EquivLoc12 " +
                    "WHERE EmpresaID= @EmpresaID AND Ano= @Ano AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                //Valores
                mySqlCommandSel.Parameters.Add("@ValorLoc00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorLoc12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivExt12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ValorExt12", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc00", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc01", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc02", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc03", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc04", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc05", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc06", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc07", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc08", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc09", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc10", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc11", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@EquivLoc12", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = total.Ano.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = total.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = total.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = total.LineaPresupuestoID.Value;
                //Valores
                mySqlCommandSel.Parameters["@ValorLoc00"].Value = total.ValorLoc00.Value;
                mySqlCommandSel.Parameters["@ValorLoc01"].Value = total.ValorLoc01.Value;
                mySqlCommandSel.Parameters["@ValorLoc02"].Value = total.ValorLoc02.Value;
                mySqlCommandSel.Parameters["@ValorLoc03"].Value = total.ValorLoc03.Value;
                mySqlCommandSel.Parameters["@ValorLoc04"].Value = total.ValorLoc04.Value;
                mySqlCommandSel.Parameters["@ValorLoc05"].Value = total.ValorLoc05.Value;
                mySqlCommandSel.Parameters["@ValorLoc06"].Value = total.ValorLoc06.Value;
                mySqlCommandSel.Parameters["@ValorLoc07"].Value = total.ValorLoc07.Value;
                mySqlCommandSel.Parameters["@ValorLoc08"].Value = total.ValorLoc08.Value;
                mySqlCommandSel.Parameters["@ValorLoc09"].Value = total.ValorLoc09.Value;
                mySqlCommandSel.Parameters["@ValorLoc10"].Value = total.ValorLoc10.Value;
                mySqlCommandSel.Parameters["@ValorLoc11"].Value = total.ValorLoc11.Value;
                mySqlCommandSel.Parameters["@ValorLoc12"].Value = total.ValorLoc12.Value;
                mySqlCommandSel.Parameters["@EquivExt00"].Value = total.EquivExt00.Value;
                mySqlCommandSel.Parameters["@EquivExt01"].Value = total.EquivExt01.Value;
                mySqlCommandSel.Parameters["@EquivExt02"].Value = total.EquivExt02.Value;
                mySqlCommandSel.Parameters["@EquivExt03"].Value = total.EquivExt03.Value;
                mySqlCommandSel.Parameters["@EquivExt04"].Value = total.EquivExt04.Value;
                mySqlCommandSel.Parameters["@EquivExt05"].Value = total.EquivExt05.Value;
                mySqlCommandSel.Parameters["@EquivExt06"].Value = total.EquivExt06.Value;
                mySqlCommandSel.Parameters["@EquivExt07"].Value = total.EquivExt07.Value;
                mySqlCommandSel.Parameters["@EquivExt08"].Value = total.EquivExt08.Value;
                mySqlCommandSel.Parameters["@EquivExt09"].Value = total.EquivExt09.Value;
                mySqlCommandSel.Parameters["@EquivExt10"].Value = total.EquivExt10.Value;
                mySqlCommandSel.Parameters["@EquivExt11"].Value = total.EquivExt11.Value;
                mySqlCommandSel.Parameters["@EquivExt12"].Value = total.EquivExt12.Value;
                mySqlCommandSel.Parameters["@ValorExt00"].Value = total.ValorExt00.Value;
                mySqlCommandSel.Parameters["@ValorExt01"].Value = total.ValorExt01.Value;
                mySqlCommandSel.Parameters["@ValorExt02"].Value = total.ValorExt02.Value;
                mySqlCommandSel.Parameters["@ValorExt03"].Value = total.ValorExt03.Value;
                mySqlCommandSel.Parameters["@ValorExt04"].Value = total.ValorExt04.Value;
                mySqlCommandSel.Parameters["@ValorExt05"].Value = total.ValorExt05.Value;
                mySqlCommandSel.Parameters["@ValorExt06"].Value = total.ValorExt06.Value;
                mySqlCommandSel.Parameters["@ValorExt07"].Value = total.ValorExt07.Value;
                mySqlCommandSel.Parameters["@ValorExt08"].Value = total.ValorExt08.Value;
                mySqlCommandSel.Parameters["@ValorExt09"].Value = total.ValorExt09.Value;
                mySqlCommandSel.Parameters["@ValorExt10"].Value = total.ValorExt10.Value;
                mySqlCommandSel.Parameters["@ValorExt11"].Value = total.ValorExt11.Value;
                mySqlCommandSel.Parameters["@ValorExt12"].Value = total.ValorExt12.Value;
                mySqlCommandSel.Parameters["@EquivLoc00"].Value = total.EquivLoc00.Value;
                mySqlCommandSel.Parameters["@EquivLoc01"].Value = total.EquivLoc01.Value;
                mySqlCommandSel.Parameters["@EquivLoc02"].Value = total.EquivLoc02.Value;
                mySqlCommandSel.Parameters["@EquivLoc03"].Value = total.EquivLoc03.Value;
                mySqlCommandSel.Parameters["@EquivLoc04"].Value = total.EquivLoc04.Value;
                mySqlCommandSel.Parameters["@EquivLoc05"].Value = total.EquivLoc05.Value;
                mySqlCommandSel.Parameters["@EquivLoc06"].Value = total.EquivLoc06.Value;
                mySqlCommandSel.Parameters["@EquivLoc07"].Value = total.EquivLoc07.Value;
                mySqlCommandSel.Parameters["@EquivLoc08"].Value = total.EquivLoc08.Value;
                mySqlCommandSel.Parameters["@EquivLoc09"].Value = total.EquivLoc09.Value;
                mySqlCommandSel.Parameters["@EquivLoc10"].Value = total.EquivLoc10.Value;
                mySqlCommandSel.Parameters["@EquivLoc11"].Value = total.EquivLoc11.Value;
                mySqlCommandSel.Parameters["@EquivLoc12"].Value = total.EquivLoc12.Value;
                #endregion
                
                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoTotal_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de plPresupuestoTotal
        /// </summary>
        /// <param name="total">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_plPresupuestoTotal_Add(DTO_plPresupuestoTotal total, ref bool validateSaldoMensual)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from plPresupuestoTotal with(nolock) " +
                    "WHERE EmpresaID= @EmpresaID AND Ano= @Ano AND ProyectoID= @ProyectoID " +
                    "   AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = total.Ano.Value.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = total.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = total.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = total.LineaPresupuestoID.Value;

                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_plPresupuestoTotal_AddItem(total);
                else
                {
                    #region Verifica que exista Saldo Disponible por Mes
                    DTO_plPresupuestoTotal exist = this.DAL_plPresupuestoTotal_GetByParameter(total).First();
                    if ((exist.ValorLoc00.Value + total.ValorLoc00.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc01.Value + total.ValorLoc01.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc02.Value + total.ValorLoc02.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc03.Value + total.ValorLoc03.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc04.Value + total.ValorLoc04.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc05.Value + total.ValorLoc05.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc06.Value + total.ValorLoc06.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc07.Value + total.ValorLoc07.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc08.Value + total.ValorLoc08.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc09.Value + total.ValorLoc09.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc10.Value + total.ValorLoc10.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc11.Value + total.ValorLoc11.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorLoc12.Value + total.ValorLoc12.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt01.Value + total.ValorExt01.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt02.Value + total.ValorExt02.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt03.Value + total.ValorExt03.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt04.Value + total.ValorExt04.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt05.Value + total.ValorExt05.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt06.Value + total.ValorExt06.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt07.Value + total.ValorExt07.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt08.Value + total.ValorExt08.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt09.Value + total.ValorExt09.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt10.Value + total.ValorExt10.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt11.Value + total.ValorExt11.Value) < 0) validateSaldoMensual = false;
                    else if ((exist.ValorExt12.Value + total.ValorExt12.Value) < 0) validateSaldoMensual = false; 
                    #endregion
                    if(validateSaldoMensual)
                        this.DAL_plPresupuestoTotal_UpdateItem(total);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Err_AddData");
                throw exception;
            }
        }

        #endregion

        #region Otras
        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoTotal> DAL_plPresupuestoTotal_GetByParameter(DTO_plPresupuestoTotal filter)
        {
            List<DTO_plPresupuestoTotal> results = new List<DTO_plPresupuestoTotal>();

            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from plPresupuestoTotal doc with(nolock) " +
                        "where EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.Ano.Value.ToString()))
                {
                    query += "and Ano = @Ano ";
                    mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                    mySqlCommand.Parameters["@Ano"].Value = filter.Ano.Value.Value;
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
                    DTO_plPresupuestoTotal total = new DTO_plPresupuestoTotal(dr);
                    results.Add(total);
                    index++;
                }
                dr.Close();

                #region Consulta Totales
                if (results.Count > 0)
                {
                    mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                }
                foreach (DTO_plPresupuestoTotal item in results)
                {
                    mySqlCommand.Parameters["@EmpresaID"].Value = item.EmpresaID.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = item.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = item.CentroCostoID.Value;
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = item.LineaPresupuestoID.Value;
                    mySqlCommand.Parameters["@Ano"].Value = item.Ano.Value;

                    mySqlCommand.CommandText = "select SUM(ValorLoc00 +ValorLoc01+ValorLoc02+ValorLoc03+ValorLoc04+ValorLoc05+ValorLoc06+ValorLoc07+ValorLoc08+ValorLoc09+ValorLoc10+ValorLoc11+ValorLoc12) as SaldoAnteriorLoc," +
                                               "       SUM(ValorExt00 +ValorExt01+ValorExt02+ValorExt03+ValorExt04+ValorExt05+ValorExt06+ValorExt07+ValorExt08+ValorExt09+ValorExt10+ValorExt11+ValorExt12) as SaldoAnteriorExt from plPresupuestoTotal with(nolock)  " +
                                               "where EmpresaID=@EmpresaID and ProyectoID=@ProyectoID and Ano=@Ano and CentroCostoID=@CentroCostoID and LineaPresupuestoID=@LineaPresupuestoID";
                    dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        item.VlrSaldoAntLoc.Value = dr["SaldoAnteriorLoc"] != DBNull.Value ? Math.Round(Convert.ToDecimal(dr["SaldoAnteriorLoc"])) : 0;
                        item.VlrSaldoAntExtr.Value = dr["SaldoAnteriorExt"] != DBNull.Value ? Math.Round(Convert.ToDecimal(dr["SaldoAnteriorExt"])) : 0;
                        item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                        item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                    }
                    dr.Close();
                }
                #endregion      

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_plPresupuestoTotal_GetByParameter");
                throw exception;
            }
        } 
        #endregion

    }
}
