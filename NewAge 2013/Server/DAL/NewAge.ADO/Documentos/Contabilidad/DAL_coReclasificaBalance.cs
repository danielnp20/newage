using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_coReclasificaBalance
    /// </summary>
    public class DAL_coReclasificaBalance : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coReclasificaBalance(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_coReclasificaBalance
        /// </summary>
        /// <returns>retorna una lista de DTO_coReclasificaBalance</returns>
        public List<DTO_coReclasificaBalance> DAL_coReclasificaBalance_GetAll()
        {
            try
            {
                List<DTO_coReclasificaBalance> result = new List<DTO_coReclasificaBalance>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "select * from coReclasificaBalance with(nolock) where EmpresaID=@EmpresaID";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_coReclasificaBalance dto = new DTO_coReclasificaBalance(dr);
                    dto.Index.Value = index;
                    result.Add(dto);

                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalance_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro de la distribucion de comprobantes
        /// </summary>
        /// <param name="consecutivo">Identificador</param>
        /// <returns>retorna una lista de DTO_coReclasificaBalance</returns>
        public List<DTO_coReclasificaBalance> DAL_coReclasificaBalance_GetByID(int consecutivo)
        {
            try
            {
                List<DTO_coReclasificaBalance> result = new List<DTO_coReclasificaBalance>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommand.CommandText = "select * from coReclasificaBalance with(nolock) where EmpresaID = @EmpresaID and Consecutivo = @Consecutivo";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_coReclasificaBalance dto = new DTO_coReclasificaBalance(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalance_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro a coReclasificaBalance
        /// </summary>
        /// <param name="dto"></param>
        public void DAL_coReclasificaBalance_Add(DTO_coReclasificaBalance dto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuentaORIG", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoCostoORIG", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoORIG", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Orden", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CuentaCONT", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoCostoCONT", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoCONT", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaDEST", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoCostoDEST", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoDEST", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorcentajeID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = dto.Consecutivo.Value;
                mySqlCommandSel.Parameters["@CuentaORIG"].Value = dto.CuentaORIG.Value;
                mySqlCommandSel.Parameters["@CtoCostoORIG"].Value = dto.CtoCostoORIG.Value;
                mySqlCommandSel.Parameters["@ProyectoORIG"].Value = dto.ProyectoORIG.Value;
                mySqlCommandSel.Parameters["@Orden"].Value = dto.Orden.Value;
                mySqlCommandSel.Parameters["@CuentaCONT"].Value = dto.CuentaCONT.Value;
                mySqlCommandSel.Parameters["@CtoCostoCONT"].Value = dto.CtoCostoCONT.Value;
                mySqlCommandSel.Parameters["@ProyectoCONT"].Value = dto.ProyectoCONT.Value;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = dto.BalanceTipoID.Value;
                mySqlCommandSel.Parameters["@CuentaDEST"].Value = dto.CuentaDEST.Value;
                mySqlCommandSel.Parameters["@CtoCostoDEST"].Value = dto.CtoCostoDEST.Value;
                mySqlCommandSel.Parameters["@ProyectoDEST"].Value = dto.ProyectoDEST.Value;
                mySqlCommandSel.Parameters["@PorcentajeID"].Value = dto.PorcentajeID.Value;
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO coReclasificaBalance( " +
                    "	EmpresaID, Consecutivo,CuentaORIG,CtoCostoORIG,ProyectoORIG,Orden,CuentaCONT,CtoCostoCONT,ProyectoCONT,BalanceTipoID, " +
                    "	CuentaDEST,CtoCostoDEST,ProyectoDEST,PorcentajeID,eg_coPlanCuenta,eg_coCentroCosto,eg_coProyecto,eg_coBalanceTipo " +
                    ")VALUES( " +
                    "	@EmpresaID, @Consecutivo,@CuentaORIG,@CtoCostoORIG,@ProyectoORIG,@Orden,@CuentaCONT,@CtoCostoCONT,@ProyectoCONT,@BalanceTipoID, " +
                    "	@CuentaDEST,@CtoCostoDEST,@ProyectoDEST,@PorcentajeID,@eg_coPlanCuenta,@eg_coCentroCosto,@eg_coProyecto,@eg_coBalanceTipo " +
                    ") ";
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                        param.Value = DBNull.Value;
                }

                mySqlCommandSel.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalance_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los datos de DTO_coCompDistribuyeExcluye
        /// </summary>
        /// <returns>retorna una lista de DTO_coCompDistribuyeExcluye</returns>
        public void DAL_coReclasificaBalance_Delete()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "delete from coReclasificaBalance where EmpresaID=@EmpresaID";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalance_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los registros de DTO_coReclasificaBalance para distribuir comprobantes
        /// </summary>
        /// <returns>retorna una lista de DTO_coReclasificaBalance</returns>
        public List<DTO_coReclasificaBalance> DAL_coReclasificaBalance_GetForProcess(string tipoBalanceID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_coReclasificaBalance> result = new List<DTO_coReclasificaBalance>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@BalanceTipoID"].Value = tipoBalanceID;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);

                mySqlCommand.CommandText =
                    "select distinct EmpresaID, CuentaORIG, CtoCostoORIG, ProyectoORIG, Orden, BalanceTipoID, Consecutivo " +
                    "from coReclasificaBalance with(nolock) " +
                    "where EmpresaID = @EmpresaID and BalanceTipoID = @BalanceTipoID and eg_coBalanceTipo= @eg_coBalanceTipo " + 
                    "order by Orden ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_coReclasificaBalance dto = new DTO_coReclasificaBalance(dr);
                    dto.Index.Value = index;
                    result.Add(dto);

                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DTO_coCompDistribuyeExcluye");
                throw exception;
            }
        }

    }
}
