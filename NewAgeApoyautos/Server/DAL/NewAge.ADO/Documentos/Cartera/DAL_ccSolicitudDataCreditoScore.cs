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
    public class DAL_ccSolicitudDataCreditoScore : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDataCreditoScore(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudDataCreditoScore que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudDataCreditoScore</returns>
        public List<DTO_ccSolicitudDataCreditoScore> DAL_ccSolicitudDataCreditoScore_GetAll()
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoScore> result = new List<DTO_ccSolicitudDataCreditoScore>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT Datos.* FROM ccSolicitudDataCreditoScore Datos with(nolock)";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDataCreditoScore Datos;
                    Datos = new DTO_ccSolicitudDataCreditoScore(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoScore_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDataCreditoScore> DAL_ccSolicitudDataCreditoScore_GetByNUmeroDoc(int numeroDoc, Int16? versionNro)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoScore> result = new List<DTO_ccSolicitudDataCreditoScore>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters.Add("@Version", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@Version"].Value = versionNro;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                if (versionNro.HasValue)
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoScore  with(nolock) " +
                                         " Where NumeroDoc =  @NumeroDoc and Version =  @Version  ";
                else
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoScore  with(nolock) " +
                                        " Where NumeroDoc =  @NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoScore(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_GetByConsecutivo");
                throw exception;
            }
        }


        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="numeroDoc">identificador de la actividad</param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDataCreditoScore> DAL_ccSolicitudDataCreditoScore_GetByLastVersion(int numeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoScore> result = new List<DTO_ccSolicitudDataCreditoScore>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "select Version from ccSolicitudDataCreditoScore  with(nolock) " +
                                      " Where NumeroDoc =  @NumeroDoc  order by Version desc ";

                var version = mySqlCommand.ExecuteScalar();

                mySqlCommand.Parameters.Add("@Version", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@Version"].Value = version != null ? Convert.ToInt16(version) : version;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.CommandText = "Select * from ccSolicitudDataCreditoScore data with(nolock) " +
                                             // "  inner join drSolicitudDatosPersonales sol on sol.NumeroDoc = data.NumeroDoc and sol.Version = data.Version and sol.TerceroID = cast(data.NumeroId as integer) " +
                                             " Where data.NumeroDoc = @NumeroDoc and data.Version = @Version ";// and sol.DataCreditoRecibeInd is not null  ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoScore(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoScore_GetByLastVersion");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDataCreditoScore
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_ccSolicitudDataCreditoScore_Add(DTO_ccSolicitudDataCreditoScore Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccSolicitudDataCreditoScore " +
                    "( " +
                    "   NumeroDoc,Version,TipoId,NumeroId,Nombre,Puntaje,Razon1,Razon2,Razon3,Razon4 " +
                    ") " +
                    "VALUES " +
                    "( " +
                    "   @NumeroDoc,@Version,@TipoId,@NumeroId,@Nombre,@Puntaje,@Razon1,@Razon2,@Razon3,@Razon4 " +
                ") SET @Consecutivo = SCOPE_IDENTITY() ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char,11);                
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char,50);
                mySqlCommandSel.Parameters.Add("@Puntaje", SqlDbType.Char,5);
                mySqlCommandSel.Parameters.Add("@Razon1", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@Razon2", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@Razon3", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@Razon4", SqlDbType.Char,2);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = Datos.Nombre.Value;
                mySqlCommandSel.Parameters["@Puntaje"].Value = Datos.Puntaje.Value;
                mySqlCommandSel.Parameters["@Razon1"].Value = Datos.Razon1.Value;
                mySqlCommandSel.Parameters["@Razon2"].Value = Datos.Razon2.Value;
                mySqlCommandSel.Parameters["@Razon3"].Value = Datos.Razon3.Value;
                mySqlCommandSel.Parameters["@Razon4"].Value = Datos.Razon4.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                //Eg
                //mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);

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
                int consec = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                return consec;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoScore_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDataCreditoScore
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudDataCreditoScore_Update(DTO_ccSolicitudDataCreditoScore Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                           "UPDATE ccSolicitudDataCreditoScore SET  " +
                                                " Version=@Version" +
                                                " ,TipoId=@TipoId" +
                                                " ,NumeroId=@NumeroId" +
                                                " ,Nombre=@Nombre" +
                                                " ,Puntaje=@Puntaje" +
                                                " ,Razon1=@Razon1" +
                                                " ,Razon2=@Razon2" +
                                                " ,Razon3=@Razon3" +
                                                " ,Razon4=@Razon4" +                                                
                                                " WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char, 50);
                mySqlCommandSel.Parameters.Add("@Puntaje", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@Razon1", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@Razon2", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@Razon3", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@Razon4", SqlDbType.Char, 2);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = Datos.Nombre.Value;
                mySqlCommandSel.Parameters["@Puntaje"].Value = Datos.Puntaje.Value;
                mySqlCommandSel.Parameters["@Razon1"].Value = Datos.Razon1.Value;
                mySqlCommandSel.Parameters["@Razon2"].Value = Datos.Razon2.Value;
                mySqlCommandSel.Parameters["@Razon3"].Value = Datos.Razon3.Value;
                mySqlCommandSel.Parameters["@Razon4"].Value = Datos.Razon4.Value;
                //mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Datos.Consecutivo.Value;
                //Eg
                //mySqlCommandSel.Parameters["@eg_glZona"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glZona, this.Empresa, egCtrl);

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
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoScore_Update");
                throw exception;
            }
        }

        public void DAL_ccSolicitudDataCreditoScore_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudDataCreditoScore WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoScore_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_ccSolicitudDataCreditoScore_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from ccSolicitudDataCreditoScore with(nolock) where Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoScore_Exist");
                throw exception;
            }
        }
        #endregion

       
    }
}
