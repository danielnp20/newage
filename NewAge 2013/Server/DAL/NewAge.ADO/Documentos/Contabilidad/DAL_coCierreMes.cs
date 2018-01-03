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
    public class DAL_coCierreMes : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coCierreMes(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla coCierreMes
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private void DAL_coCierreMes_AddItem(DTO_coCierreMes cierre, int mes)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string mesStr = mes.ToString();
                if (mesStr.Length == 1)
                    mesStr = "0" + mesStr;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "INSERT INTO coCierreMes " +
                    "( " +
                        "EmpresaID,Ano,BalanceTipoID,CuentaID,ProyectoID,CentroCostoID,LineaPresupuestoID,ConceptoCargoID," +
                        "LocalDB" + mesStr + ",LocalCR" + mesStr + ",ExtraDB" + mesStr + ",ExtraCR" + mesStr + "," +
                        "eg_coBalanceTipo,eg_coPlanCuenta,eg_coProyecto,eg_coCentroCosto,eg_plLineaPresupuesto,eg_coConceptoCargo" +
                    ") " +
                    "VALUES " +
                    "( " +
                        "@EmpresaID,@Ano,@BalanceTipoID,@CuentaID,@ProyectoID,@CentroCostoID,@LineaPresupuestoID,@ConceptoCargoID," +
                        "@LocalDB,@LocalCR,@ExtraDB,@ExtraCR," +
                        "@eg_coBalanceTipo,@eg_coPlanCuenta,@eg_coProyecto,@eg_coCentroCosto,@eg_plLineaPresupuesto,@eg_coConceptoCargo" +
                    ") ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LocalDB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@LocalCR", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ExtraDB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ExtraCR", SqlDbType.Decimal);
                //Eg
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = cierre.Ano.Value.Value;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = cierre.BalanceTipoID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = cierre.CuentaID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = cierre.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = cierre.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = cierre.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = cierre.ConceptoCargoID.Value;
                mySqlCommandSel.Parameters["@LocalDB"].Value = cierre.LocalDB01.Value;
                mySqlCommandSel.Parameters["@LocalCR"].Value = cierre.LocalCR01.Value;
                mySqlCommandSel.Parameters["@ExtraDB"].Value = cierre.ExtraDB01.Value;
                mySqlCommandSel.Parameters["@ExtraCR"].Value = cierre.ExtraCR01.Value;
                //Eg
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCierreMes_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de 
        /// </summary>
        /// <param name="cierre">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        private void DAL_coCierreMes_UpdateItem(DTO_coCierreMes cierre, int mes)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string mesStr = mes.ToString();
                if (mesStr.Length == 1)
                    mesStr = "0" + mesStr;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "UPDATE coCierreMes SET LocalDB" + mesStr + "=@LocalDB,LocalCR" + mesStr + "=@LocalCR,ExtraDB" + mesStr + "=@ExtraDB,ExtraCR" + mesStr + "=@ExtraCR " +
                    "WHERE EmpresaID= @EmpresaID AND Ano= @Ano AND BalanceTipoID = @BalanceTipoID AND CuentaID= @CuentaID " +
                    "   AND ProyectoID= @ProyectoID AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID " +
                    "   AND ConceptoCargoID=@ConceptoCargoID";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LocalDB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@LocalCR", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ExtraDB", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ExtraCR", SqlDbType.Decimal);

                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = cierre.Ano.Value.Value;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = cierre.BalanceTipoID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = cierre.CuentaID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = cierre.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = cierre.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = cierre.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = cierre.ConceptoCargoID.Value;
                mySqlCommandSel.Parameters["@LocalDB"].Value = cierre.LocalDB01.Value;
                mySqlCommandSel.Parameters["@LocalCR"].Value = cierre.LocalCR01.Value;
                mySqlCommandSel.Parameters["@ExtraDB"].Value = cierre.ExtraDB01.Value;
                mySqlCommandSel.Parameters["@ExtraCR"].Value = cierre.ExtraCR01.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCierreMes_UpdateItem");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro de coCierreMes
        /// </summary>
        /// <param name="cierreDia">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void DAL_coCierreMes_Add(DTO_coCierreMes cierre, int mes)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT COUNT (*) from coCierreMes with(nolock) " +
                    "WHERE EmpresaID= @EmpresaID AND Ano= @Ano AND BalanceTipoID = @BalanceTipoID AND CuentaID= @CuentaID " +
                    "   AND ProyectoID= @ProyectoID AND CentroCostoID= @CentroCostoID AND LineaPresupuestoID= @LineaPresupuestoID "  +
                    "   AND ConceptoCargoID=@ConceptoCargoID";
                #endregion
                #region Creacion de comandos
                //PK
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ano", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                #endregion
                #region Asigna los valores
                //PK
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Ano"].Value = cierre.Ano.Value.Value;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = cierre.BalanceTipoID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = cierre.CuentaID.Value;
                mySqlCommandSel.Parameters["@ProyectoID"].Value = cierre.ProyectoID.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = cierre.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = cierre.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@ConceptoCargoID"].Value = cierre.ConceptoCargoID.Value;

                #endregion

                //Verifica si agrega o actualiza el registro
                int count = Convert.ToInt32(mySqlCommandSel.ExecuteScalar());
                if (count == 0)
                    this.DAL_coCierreMes_AddItem(cierre, mes);
                else
                    this.DAL_coCierreMes_UpdateItem(cierre, mes);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCierreMes_Add");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae la info del cierre
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<DTO_coCierreMes> DAL_coCierreMes_GetForCierre(DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                // Paramametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;

                // CommandText
                mySqlCommandSel.CommandText =
                    "select Year(PeriodoID) as Año, BalanceTipoID, CuentaID, ProyectoID, CentroCostoID, LineaPresupuestoID, ConceptoCargoID, " +
                    "	SUM(DbOrigenLocML + DbOrigenExtML) as LocalDB, " +
                    "	SUM(CROrigenLocML + CROrigenExtML) as LocalCR, " +
                    "	SUM(DbOrigenLocME + DbOrigenExtME) as ExtraDB, " +
                    "	SUM(CROrigenLocME + CROrigenExtME) as ExtraCR " +
                    "from coCuentaSaldo with(nolock) " +
                    "where EmpresaID = @EmpresaID and PeriodoID = @Periodo " +
                    "group by PeriodoID, BalanceTipoID, CuentaID, ProyectoID, CentroCostoID, LineaPresupuestoID, ConceptoCargoID";

                List<DTO_coCierreMes> results = new List<DTO_coCierreMes>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coCierreMes cierre = new DTO_coCierreMes(dr, false);
                    results.Add(cierre);
                }

                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCierreMes_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Carga todos los cierres diarios de un periodo
        /// </summary>
        /// <param name="año">Periodo de Consulta</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> DAL_coCierreMes_GetAll(Int16 año)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_coCierreMes> cierres = new List<DTO_coCierreMes>();

                #region CommanText

                mySqlCommand.CommandText =
                    " SELECT  * " +
                    " FROM coCierreMes cierre  WITH(NOLOCK)  " +
                    " WHERE cierre.EmpresaID = @EmpresaID and cierre.Ano = @Año ";

                #endregion
                #region Definicion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Año", SqlDbType.SmallInt);
                #endregion
                #region Asignación de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Año"].Value = año;
                #endregion

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_coCierreMes cierre = new DTO_coCierreMes(dr, true);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCierreMes_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Carga todos los cierres mes con uno o varios filtros desde el coCuentaSaldo
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> DAL_coCierreMes_GetByParameter(DTO_coCierreMes filter)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_coCierreMes> cierres = new List<DTO_coCierreMes>(); 
                string where = string.Empty;
                bool filterInd = false;

                #region Parametros
		        mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommand.Parameters["@Ano"].Value = filter.Ano.Value.Value;
               
                if (!string.IsNullOrEmpty(filter.BalanceTipoID.Value))
                {
                    where += "and BalanceTipoID = @BalanceTipoID ";
                    mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                    mySqlCommand.Parameters["@BalanceTipoID"].Value = filter.BalanceTipoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CuentaID.Value))
                {
                    where += "and CuentaID = @CuentaID ";
                    mySqlCommand.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                    mySqlCommand.Parameters["@CuentaID"].Value = filter.CuentaID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ProyectoID.Value.ToString()))
                {
                    where += "and ProyectoID = @ProyectoID ";
                    mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                    mySqlCommand.Parameters["@ProyectoID"].Value = filter.ProyectoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.CentroCostoID.Value.ToString()))
                {
                    where += "and CentroCostoID = @CentroCostoID ";
                    mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                    mySqlCommand.Parameters["@CentroCostoID"].Value = filter.CentroCostoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.LineaPresupuestoID.Value.ToString()))
                {
                    where += "and LineaPresupuestoID = @LineaPresupuestoID ";
                    mySqlCommand.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                    mySqlCommand.Parameters["@LineaPresupuestoID"].Value = filter.LineaPresupuestoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConceptoCargoID.Value.ToString()))
                {
                    where += "and ConceptoCargoID = @ConceptoCargoID ";
                    mySqlCommand.Parameters.Add("@ConceptoCargoID", SqlDbType.Char, UDT_ConceptoCargoID.MaxLength);
                    mySqlCommand.Parameters["@ConceptoCargoID"].Value = filter.ConceptoCargoID.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TerceroID.Value.ToString()))
                {
                    where += "and TerceroID = @TerceroID ";
                    mySqlCommand.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                    mySqlCommand.Parameters["@TerceroID"].Value = filter.TerceroID.Value;
                    filterInd = true;
                } 
	            #endregion
                #region CommandText
                mySqlCommand.CommandText =
                     "SELECT PeriodoID, Year(PeriodoID) as Ano, BalanceTipoID, CuentaID, ProyectoID, CentroCostoID,LineaPresupuestoID, ConceptoCargoID,TerceroID,   " +
                     //Moneda Local
                     "    CASE WHEN(MONTH(PeriodoID) = 1) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI01,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 2) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI02,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 3) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI03,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 4) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI04,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 5) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI05,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 6) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI06,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 7) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI07,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 8) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI08,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 9) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI09,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 10)THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI10,  " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 11)THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI11,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 12 and DAY(PeriodoID) = 1)THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as LocalINI12,   " +
                     "    CASE WHEN(MONTH(PeriodoID) = 1) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB01,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 2) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB02,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 3) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB03,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 4) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB04,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 5) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB05,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 6) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB06,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 7) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB07,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 8) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB08,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 9) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB09,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 10) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB10,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 11) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB11,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 12 and DAY(PeriodoID) = 1) THEN SUM(DbOrigenLocML + DbOrigenExtML) ELSE 0 END as LocalDB12,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 1) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END   as LocalCR01,   " +
                     "    CASE WHEN(MONTH(PeriodoID) = 2) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR02,   " +
                     "    CASE WHEN(MONTH(PeriodoID) = 3) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR03,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 4) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR04,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 5) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR05,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 6) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR06,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 7) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR07,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 8) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR08,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 9) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR09,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 10) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR10,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 11) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR11,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 12 and DAY(PeriodoID) = 1) THEN SUM(CROrigenLocML + CROrigenExtML) ELSE 0 END as LocalCR12,   " +
                     //Moneda Extranjera
                     "    CASE WHEN(MONTH(PeriodoID) = 1) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI01,     " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 2) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI02,    " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 3) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI03,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 4) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI04,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 5) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI05,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 6) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI06,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 7) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI07,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 8) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI08,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 9) THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI09,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 10)THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI10,   " +
	                 "    CASE WHEN(MONTH(PeriodoID) = 11)THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI11,   " +
                     "    CASE WHEN(MONTH(PeriodoID) = 12 and DAY(PeriodoID) = 1)THEN SUM(DbSaldoIniLocML + DbSaldoIniExtML+CRSaldoIniLocML + CRSaldoIniExtML) ELSE 0 END as ExtraINI12,    " +
                     "    CASE WHEN( MONTH(PeriodoID) = 1) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END  as ExtraDB01,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 2) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB02,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 3) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB03,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 4) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB04,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 5) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB05,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 6) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB06,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 7) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB07,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 8) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB08,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 9) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB09,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 10) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB10,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 11) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB11,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 12 and DAY(PeriodoID) = 1) THEN SUM(DbOrigenLocME + DbOrigenExtME) ELSE 0 END as ExtraDB12,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 1) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END   as ExtraCR01,   " +
                     "    CASE WHEN(MONTH(PeriodoID) = 2) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR02,   " +
                     "    CASE WHEN(MONTH(PeriodoID) = 3) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR03,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 4) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR04,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 5) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR05,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 6) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR06,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 7) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR07,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 8) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR08,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 9) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR09,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 10) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR10,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 11) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR11,  " +
                     "    CASE WHEN(MONTH(PeriodoID) = 12 and DAY(PeriodoID) = 1) THEN SUM(CROrigenLocME + CROrigenExtME) ELSE 0 END as ExtraCR12,   " +
                     "    0 as LocalINI, 0 as ExtraINI "+
                     "FROM coCuentaSaldo with(nolock)    " +
                     "Where EmpresaID = @EmpresaID and YEAR(PeriodoID) = @Ano  " + where +
                     " Group by PeriodoID, BalanceTipoID, CuentaID, ProyectoID, CentroCostoID, LineaPresupuestoID, ConceptoCargoID, TerceroID  ";
                
                #endregion
                if (!filterInd)
                    return cierres;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_coCierreMes cierre = new DTO_coCierreMes(dr,true);
                    cierre.TerceroID.Value = dr["TerceroID"].ToString();
                    cierre.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                    //Saldos inicial - Loc
                    cierre.LocalINI01.Value = Convert.ToDecimal(dr["LocalINI01"]);
                    cierre.LocalINI02.Value = Convert.ToDecimal(dr["LocalINI02"]);
                    cierre.LocalINI03.Value = Convert.ToDecimal(dr["LocalINI03"]);
                    cierre.LocalINI04.Value = Convert.ToDecimal(dr["LocalINI04"]);
                    cierre.LocalINI05.Value = Convert.ToDecimal(dr["LocalINI05"]);
                    cierre.LocalINI06.Value = Convert.ToDecimal(dr["LocalINI06"]);
                    cierre.LocalINI07.Value = Convert.ToDecimal(dr["LocalINI07"]);
                    cierre.LocalINI08.Value = Convert.ToDecimal(dr["LocalINI08"]);
                    cierre.LocalINI09.Value = Convert.ToDecimal(dr["LocalINI09"]);
                    cierre.LocalINI10.Value = Convert.ToDecimal(dr["LocalINI10"]);
                    cierre.LocalINI11.Value = Convert.ToDecimal(dr["LocalINI11"]);
                    cierre.LocalINI12.Value = Convert.ToDecimal(dr["LocalINI12"]);
                    //Saldos inicial - Extra
                    cierre.ExtraINI01.Value = Convert.ToDecimal(dr["ExtraINI01"]);
                    cierre.ExtraINI02.Value = Convert.ToDecimal(dr["ExtraINI02"]);
                    cierre.ExtraINI03.Value = Convert.ToDecimal(dr["ExtraINI03"]);
                    cierre.ExtraINI04.Value = Convert.ToDecimal(dr["ExtraINI04"]);
                    cierre.ExtraINI05.Value = Convert.ToDecimal(dr["ExtraINI05"]);
                    cierre.ExtraINI06.Value = Convert.ToDecimal(dr["ExtraINI06"]);
                    cierre.ExtraINI07.Value = Convert.ToDecimal(dr["ExtraINI07"]);
                    cierre.ExtraINI08.Value = Convert.ToDecimal(dr["ExtraINI08"]);
                    cierre.ExtraINI09.Value = Convert.ToDecimal(dr["ExtraINI09"]);
                    cierre.ExtraINI10.Value = Convert.ToDecimal(dr["ExtraINI10"]);
                    cierre.ExtraINI11.Value = Convert.ToDecimal(dr["ExtraINI11"]);
                    cierre.ExtraINI12.Value = Convert.ToDecimal(dr["ExtraINI12"]);
                    cierres.Add(cierre);
                }
                dr.Close();

                return cierres;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCierreMes_GetByParameter");
                throw exception;
            }

        } 

        #endregion

    }
}
