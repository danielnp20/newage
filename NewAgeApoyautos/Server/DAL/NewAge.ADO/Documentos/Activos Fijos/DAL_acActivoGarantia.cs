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
using NewAge.DTO.Negocio.Documentos.Activos;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_DocumentOps
    /// </summary>
    public class DAL_acActivoGarantia : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_acActivoGarantia(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_acActivoGarantia DAL_acActivoGarantia_GetByID(int activoID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from acActivoGarantia with(nolock) where ActivoID = @ActivoID";

                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters["@ActivoID"].Value = activoID;

                DTO_acActivoGarantia res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_acActivoGarantia(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoGarantia", "DAL_acActivoGarantia_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        public void DAL_acActivoGarantia_Add(DTO_acActivoGarantia acCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                           "INSERT INTO [acActivoGarantia]" +
                                    "   ([EmpresaID]    " +
                                    "   ,[ActivoID]   " +
                                    "   ,[GarantiaRef]   " +
                                    "   ,[FechaINICliente]   " +
                                    "   ,[FechaFINCliente]   " +
                                    "   ,[FechaINIProveedor]   " +
                                    "   ,[FechaFINProveedor]   " +
                                    "   ,[FechaFINEmpresa])   " +
                                    "     VALUES    " +
                                    "   (@EmpresaID  " +
                                    "   ,@ActivoID  " +
                                    "   ,@GarantiaRef  " +
                                    "   ,@FechaINICliente  " +
                                    "   ,@FechaFINCliente  " +
                                    "   ,@FechaINIProveedor  " +
                                    "   ,@FechaFINProveedor  " +
                                    "   ,@FechaFINEmpresa) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@GarantiaRef", SqlDbType.Char, 30);
                mySqlCommand.Parameters.Add("@FechaINICliente", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFINCliente", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaINIProveedor", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFINProveedor", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFINEmpresa", SqlDbType.SmallDateTime);
                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = acCtrl.ActivoID.Value;
                mySqlCommand.Parameters["@GarantiaRef"].Value = acCtrl.GarantiaRef.Value;
                mySqlCommand.Parameters["@FechaINICliente"].Value = acCtrl.FechaINICliente.Value;
                mySqlCommand.Parameters["@FechaFINCliente"].Value = acCtrl.FechaFINCliente.Value; 
                mySqlCommand.Parameters["@FechaINIProveedor"].Value = acCtrl.FechaINIProveedor.Value; 
                mySqlCommand.Parameters["@FechaFINProveedor"].Value = acCtrl.FechaFINProveedor.Value;
                mySqlCommand.Parameters["@FechaFINEmpresa"].Value = acCtrl.FechaFINEmpresa.Value;        
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
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoGarantia", "DAL_acActivoGarantia_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro al control de documentos
        /// </summary>
        public void DAL_acActivoGarantia_Update(DTO_acActivoGarantia acCtrl)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText =
                       " UPDATE acActivoGarantia SET     " +
                       " [GarantiaRef] = @GarantiaRef    " +
                       " ,[FechaINICliente] = @FechaINICliente  " +
                       " ,[FechaFINCliente] = @FechaFINCliente    " +
                       " ,[FechaINIProveedor] = @FechaINIProveedor    " +
                       " ,[FechaFINProveedor] = @FechaFINProveedor    " +
                       " ,[FechaFINEmpresa] = @FechaFINEmpresa    " +  
                       " WHERE EmpresaID = @EmpresaID AND ActivoID = @ActivoID";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@GarantiaRef", SqlDbType.Char, 30);
                mySqlCommand.Parameters.Add("@FechaINICliente", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFINCliente", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaINIProveedor", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFINProveedor", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@FechaFINEmpresa", SqlDbType.SmallDateTime);
                #endregion
                #region Asignacion de Valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActivoID"].Value = acCtrl.ActivoID.Value;
                mySqlCommand.Parameters["@GarantiaRef"].Value = acCtrl.GarantiaRef.Value;
                mySqlCommand.Parameters["@FechaINICliente"].Value = acCtrl.FechaINICliente.Value;
                mySqlCommand.Parameters["@FechaFINCliente"].Value = acCtrl.FechaFINCliente.Value;
                mySqlCommand.Parameters["@FechaINIProveedor"].Value = acCtrl.FechaINIProveedor.Value;
                mySqlCommand.Parameters["@FechaFINProveedor"].Value = acCtrl.FechaFINProveedor.Value;
                mySqlCommand.Parameters["@FechaFINEmpresa"].Value = acCtrl.FechaFINEmpresa.Value;
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
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                dr.Close();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_acActivoGarantia", "DAL_acActivoGarantia_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrl">Doc Control filtro</param>
        /// <returns>Lista de Doc Control </returns>
        public List<DTO_acActivoGarantia> DAL_acActivoGarantia_GetByParameter(DTO_acActivoGarantia filter)
        {
            try
            {
                List<DTO_acActivoGarantia> result = new List<DTO_acActivoGarantia>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select * from acActivoGarantia doc with(nolock) " +
                        "where doc.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if (!string.IsNullOrEmpty(filter.ActivoID.Value.ToString()))
                {
                    query += "and ActivoID = @ActivoID ";
                    mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                    mySqlCommand.Parameters["@ActivoID"].Value = filter.ActivoID.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.GarantiaRef.Value.ToString()))
                {
                    query += "and GarantiaRef = @GarantiaRef ";
                    mySqlCommand.Parameters.Add("@GarantiaRef", SqlDbType.Char, 30);
                    mySqlCommand.Parameters["@GarantiaRef"].Value = filter.GarantiaRef.Value;
                    filterInd = true;
                }          

                query += " order by ActivoID desc";
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_acActivoGarantia ctrl = new DTO_acActivoGarantia(dr);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoGarantia_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_acActivoGarantia_Exist(int? activo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from acActivoGarantia with(nolock) where  ActivoID = @ActivoID";

                mySqlCommand.Parameters.Add("@ActivoID", SqlDbType.Int);
                mySqlCommand.Parameters["@ActivoID"].Value = activo;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acActivoGarantia_Exist");
                throw exception;
            }
        }

        #endregion
    }
}
