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
    /// DAL de DAL_coCompDistribuyeTabla
    /// </summary>
    public class DAL_coCompDistribuyeTabla : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coCompDistribuyeTabla(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_coCompDistribuyeTabla
        /// </summary>
        /// <returns>retorna una lista de DTO_coCompDistribuyeTabla</returns>
        public List<DTO_coCompDistribuyeTabla> DAL_coCompDistribuyeTabla_GetAll()
        {
            try
            {
                List<DTO_coCompDistribuyeTabla> result = new List<DTO_coCompDistribuyeTabla>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "select * from coCompDistribuyeTabla with(nolock) where EmpresaID=@EmpresaID";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_coCompDistribuyeTabla dto = new DTO_coCompDistribuyeTabla(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCompDistribuyeTabla_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un registro de la distribucion de comprobantes
        /// </summary>
        /// <param name="consecutivo">Identificador</param>
        /// <returns>retorna una lista de DTO_coCompDistribuyeTabla</returns>
        public List<DTO_coCompDistribuyeTabla> DAL_coCompDistribuyeTabla_GetByID(int consecutivo)
        {
            try
            {
                List<DTO_coCompDistribuyeTabla> result = new List<DTO_coCompDistribuyeTabla>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommand.CommandText = "select * from coCompDistribuyeTabla with(nolock) where EmpresaID = @EmpresaID and Consecutivo = @Consecutivo";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_coCompDistribuyeTabla dto = new DTO_coCompDistribuyeTabla(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCompDistribuyeTabla_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro a coCompDistribuyeTabla
        /// </summary>
        /// <param name="dto"></param>
        public void DAL_coCompDistribuyeTabla_Add(DTO_coCompDistribuyeTabla dto)
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
                mySqlCommandSel.Parameters.Add("@CuentaDEST", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoCostoDEST", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoDEST", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorcentajeID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = dto.Consecutivo.Value;
                mySqlCommandSel.Parameters["@CuentaORIG"].Value = dto.CuentaORIG.Value;
                mySqlCommandSel.Parameters["@CtoCostoORIG"].Value = dto.CtoCostoORIG.Value;
                mySqlCommandSel.Parameters["@ProyectoORIG"].Value = dto.ProyectoORIG.Value;
                mySqlCommandSel.Parameters["@Orden"].Value = dto.Orden.Value;
                mySqlCommandSel.Parameters["@CuentaCONT"].Value = dto.CuentaCONT.Value;
                mySqlCommandSel.Parameters["@CtoCostoCONT"].Value = dto.CtoCostoCONT.Value;
                mySqlCommandSel.Parameters["@ProyectoCONT"].Value = dto.ProyectoCONT.Value;
                mySqlCommandSel.Parameters["@CuentaDEST"].Value = dto.CuentaDEST.Value;
                mySqlCommandSel.Parameters["@CtoCostoDEST"].Value = dto.CtoCostoDEST.Value;
                mySqlCommandSel.Parameters["@ProyectoDEST"].Value = dto.ProyectoDEST.Value;
                mySqlCommandSel.Parameters["@PorcentajeID"].Value = dto.PorcentajeID.Value;
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO coCompDistribuyeTabla( " +
                    "	EmpresaID, Consecutivo,CuentaORIG,CtoCostoORIG,ProyectoORIG,Orden,CuentaCONT,CtoCostoCONT,ProyectoCONT, " +
                    "	CuentaDEST,CtoCostoDEST,ProyectoDEST,PorcentajeID,eg_coPlanCuenta,eg_coCentroCosto,eg_coProyecto " +
                    ")VALUES( " +
                    "	@EmpresaID, @Consecutivo,@CuentaORIG,@CtoCostoORIG,@ProyectoORIG,@Orden,@CuentaCONT,@CtoCostoCONT,@ProyectoCONT, " +
                    "	@CuentaDEST,@CtoCostoDEST,@ProyectoDEST,@PorcentajeID,@eg_coPlanCuenta,@eg_coCentroCosto,@eg_coProyecto " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCompDistribuyeTabla_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los datos de DTO_coCompDistribuyeExcluye
        /// </summary>
        /// <returns>retorna una lista de DTO_coCompDistribuyeExcluye</returns>
        public void DAL_coCompDistribuyeTabla_Delete()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "delete from coCompDistribuyeTabla where EmpresaID=@EmpresaID";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coCompDistribuyeTabla_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los registros de DTO_coCompDistribuyeTabla para distribuir comprobantes
        /// </summary>
        /// <returns>retorna una lista de DTO_coCompDistribuyeTabla</returns>
        public List<DTO_coCompDistribuyeTabla> DAL_coCompDistribuyeTabla_GetForProcess()
        {
            try
            {
                List<DTO_coCompDistribuyeTabla> result = new List<DTO_coCompDistribuyeTabla>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText =
                    "select distinct EmpresaID, CuentaORIG, CtoCostoORIG, ProyectoORIG, Orden, Consecutivo " +
                    "from coCompDistribuyeTabla with(nolock) " +
                    "where EmpresaID = @EmpresaID order by Orden  ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_coCompDistribuyeTabla dto = new DTO_coCompDistribuyeTabla(dr);
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
