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
    public class DAL_ccSolicitudDataCreditoQuanto : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDataCreditoQuanto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccSolicitudDataCreditoQuanto que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_ccSolicitudDataCreditoQuanto</returns>
        public List<DTO_ccSolicitudDataCreditoQuanto> DAL_ccSolicitudDataCreditoQuanto_GetAll()
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoQuanto> result = new List<DTO_ccSolicitudDataCreditoQuanto>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT Datos.* FROM ccSolicitudDataCreditoQuanto Datos with(nolock)";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDataCreditoQuanto Datos;
                    Datos = new DTO_ccSolicitudDataCreditoQuanto(dr);
                    result.Add(Datos);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoQuanto_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_ccSolicitudDataCreditoQuanto> DAL_ccSolicitudDataCreditoQuanto_GetByNUmeroDoc(int numeroDoc, Int16? versionNro)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoQuanto> result = new List<DTO_ccSolicitudDataCreditoQuanto>();

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
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoQuanto  with(nolock) " +
                                         " Where NumeroDoc =  @NumeroDoc and Version =  @Version  ";
                else
                    mySqlCommand.CommandText = "select * from ccSolicitudDataCreditoQuanto  with(nolock) " +
                                        " Where NumeroDoc =  @NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoQuanto(dr));
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
        public List<DTO_ccSolicitudDataCreditoQuanto> DAL_ccSolicitudDataCreditoQuanto_GetByLastVersion(int numeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudDataCreditoQuanto> result = new List<DTO_ccSolicitudDataCreditoQuanto>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommand.CommandText = "select Version from ccSolicitudDataCreditoQuanto  with(nolock) " +
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

                mySqlCommand.CommandText = "Select * from ccSolicitudDataCreditoQuanto data with(nolock) " +
                                             // "  inner join drSolicitudDatosPersonales sol on sol.NumeroDoc = data.NumeroDoc and sol.Version = data.Version and sol.TerceroID = cast(data.NumeroId as integer) " +
                                             " Where data.NumeroDoc = @NumeroDoc and data.Version = @Version ";// and sol.DataCreditoRecibeInd is not null  ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                    result.Add(new DTO_ccSolicitudDataCreditoQuanto(dr));
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoQuanto_GetByLastVersion");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDataCreditoQuanto
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public int DAL_ccSolicitudDataCreditoQuanto_Add(DTO_ccSolicitudDataCreditoQuanto Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                    "INSERT INTO ccSolicitudDataCreditoQuanto " +
                    "( " +
                    "   NumeroDoc,Version,TipoId,NumeroId,VlrMinimo,VlrMedio,VlrMaximo,VlrMinimoSMLV,VlrMedioSMLV,VlrMaximoSMLV,Exclusiones" +
                    ") " +
                    "VALUES " +
                    "( " +
                    "   @NumeroDoc,@Version,@TipoId,@NumeroId,@VlrMinimo,@VlrMedio,@VlrMaximo,@VlrMinimoSMLV,@VlrMedioSMLV,@VlrMaximoSMLV,@Exclusiones" +
                ") SET @Consecutivo = SCOPE_IDENTITY() ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char,1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char,11);
                mySqlCommandSel.Parameters.Add("@VlrMinimo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMedio", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMaximo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMinimoSMLV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMedioSMLV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMaximoSMLV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Exclusiones", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@VlrMinimo"].Value = Datos.VlrMinimo.Value;
                mySqlCommandSel.Parameters["@VlrMedio"].Value = Datos.VlrMedio.Value;
                mySqlCommandSel.Parameters["@VlrMaximo"].Value = Datos.VlrMaximo.Value;
                mySqlCommandSel.Parameters["@VlrMinimoSMLV"].Value = Datos.VlrMinimoSMLV.Value.HasValue? Datos.VlrMinimoSMLV.Value : 0;
                mySqlCommandSel.Parameters["@VlrMedioSMLV"].Value = Datos.VlrMinimoSMLV.Value.HasValue? Datos.VlrMedioSMLV.Value : 0;
                mySqlCommandSel.Parameters["@VlrMaximoSMLV"].Value = Datos.VlrMinimoSMLV.Value.HasValue? Datos.VlrMaximoSMLV.Value : 0;
                mySqlCommandSel.Parameters["@Exclusiones"].Value = Datos.Exclusiones.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoQuanto_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDataCreditoQuanto
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccSolicitudDataCreditoQuanto_Update(DTO_ccSolicitudDataCreditoQuanto Datos)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                           "UPDATE ccSolicitudDataCreditoQuanto SET  " +
                                                " Version=@Version" +
                                                " ,TipoId=@TipoId" +
                                                " ,NumeroId=@NumeroId" +
                                                " ,VlrMinimo=@VlrMinimo" +
                                                " ,VlrMedio=@VlrMedio" +
                                                " ,VlrMaximo=@VlrMaximo" +
                                                " ,VlrMinimoSMLV=@VlrMinimoSMLV" +
                                                " ,VlrMedioSMLV=@VlrMedioSMLV" +
                                                " ,VlrMaximoSMLV=@VlrMaximoSMLV" +
                                                " ,Exclusiones=@Exclusiones" +
                                                " WHERE Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@TipoId", SqlDbType.Char, 1);
                mySqlCommandSel.Parameters.Add("@NumeroId", SqlDbType.Char, 11);
                mySqlCommandSel.Parameters.Add("@VlrMinimo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMedio", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMaximo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMinimoSMLV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMedioSMLV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrMaximoSMLV", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Exclusiones", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);


                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = Datos.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@Version"].Value = Datos.Version.Value;
                mySqlCommandSel.Parameters["@TipoId"].Value = Datos.TipoId.Value;
                mySqlCommandSel.Parameters["@NumeroId"].Value = Datos.NumeroId.Value;
                mySqlCommandSel.Parameters["@VlrMinimo"].Value = Datos.VlrMinimo.Value;
                mySqlCommandSel.Parameters["@VlrMedio"].Value = Datos.VlrMedio.Value;
                mySqlCommandSel.Parameters["@VlrMaximo"].Value = Datos.VlrMaximo.Value;
                mySqlCommandSel.Parameters["@VlrMinimoSMLV"].Value = Datos.VlrMinimoSMLV.Value;
                mySqlCommandSel.Parameters["@VlrMedioSMLV"].Value = Datos.VlrMedioSMLV.Value;
                mySqlCommandSel.Parameters["@VlrMaximoSMLV"].Value = Datos.VlrMaximoSMLV.Value;
                mySqlCommandSel.Parameters["@Exclusiones"].Value = Datos.Exclusiones.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoQuanto_Update");
                throw exception;
            }
        }

        public void DAL_ccSolicitudDataCreditoQuanto_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudDataCreditoQuanto WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoQuanto_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto</returns>
        public bool DAL_ccSolicitudDataCreditoQuanto_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from ccSolicitudDataCreditoQuanto with(nolock) where Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDataCreditoQuanto_Exist");
                throw exception;
            }
        }
        #endregion       
    }
}
