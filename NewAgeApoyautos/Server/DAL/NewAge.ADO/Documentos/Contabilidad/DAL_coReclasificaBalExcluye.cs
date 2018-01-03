﻿using System;
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
    /// DAL de DAL_coReclasificaBalExcluye
    /// </summary>
    public class DAL_coReclasificaBalExcluye : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_coReclasificaBalExcluye(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los registros de DTO_coReclasificaBalExcluye
        /// </summary>
        /// <returns>retorna una lista de DTO_coReclasificaBalExcluye</returns>
        public List<DTO_coReclasificaBalExcluye> DAL_coReclasificaBalExcluye_GetAll()
        {
            try
            {
                List<DTO_coReclasificaBalExcluye> result = new List<DTO_coReclasificaBalExcluye>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "select * from coReclasificaBalExcluye with(nolock) where EmpresaID=@EmpresaID";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_coReclasificaBalExcluye dto = new DTO_coReclasificaBalExcluye(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DTO_coReclasificaBalExcluye");
                throw exception;
            }
        }

        /// <summary>
        /// Trae los registros de exclusiones de un orogen
        /// </summary>
        /// <param name="consecutivo">Consecutivo de busqueda</param>
        /// <returns>retorna una lista de DTO_coReclasificaBalExcluye</returns>
        public List<DTO_coReclasificaBalExcluye> DAL_coReclasificaBalExcluye_GetByConsecutivo(int consecutivo)
        {
            try
            {
                List<DTO_coReclasificaBalExcluye> result = new List<DTO_coReclasificaBalExcluye>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Consecutivo"].Value = consecutivo;

                mySqlCommand.CommandText = "select * from coReclasificaBalExcluye with(nolock) where EmpresaID = @EmpresaID and Consecutivo = @Consecutivo";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_coReclasificaBalExcluye dto = new DTO_coReclasificaBalExcluye(dr);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalExcluye_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro a DAL_coReclasificaBalExcluye
        /// </summary>
        /// <param name="dto"></param>
        public void DAL_coReclasificaBalExcluye_Add(DTO_coReclasificaBalExcluye dto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuentaEXCL", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CtoCostoEXCL", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ProyectoEXCL", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = dto.Consecutivo.Value;
                mySqlCommandSel.Parameters["@CuentaEXCL"].Value = dto.CuentaEXCL.Value;
                mySqlCommandSel.Parameters["@CtoCostoEXCL"].Value = dto.CtoCostoEXCL.Value;
                mySqlCommandSel.Parameters["@ProyectoEXCL"].Value = dto.ProyectoEXCL.Value;
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO coReclasificaBalExcluye( " +
                    "	EmpresaID,Consecutivo,CuentaEXCL,CtoCostoEXCL,ProyectoEXCL,eg_coPlanCuenta,eg_coCentroCosto,eg_coProyecto " +
                    ")VALUES( " +
                    "	@EmpresaID,@Consecutivo,@CuentaEXCL,@CtoCostoEXCL,@ProyectoEXCL,@eg_coPlanCuenta,@eg_coCentroCosto,@eg_coProyecto " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalExcluye");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los datos de DTO_coReclasificaBalExcluye
        /// </summary>
        /// <returns>retorna una lista de DTO_coReclasificaBalExcluye</returns>
        public void DAL_coReclasificaBalExcluye_Delete()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "delete from coReclasificaBalExcluye where EmpresaID=@EmpresaID";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coReclasificaBalExcluye_Delete");
                throw exception;
            }
        }
    }
}