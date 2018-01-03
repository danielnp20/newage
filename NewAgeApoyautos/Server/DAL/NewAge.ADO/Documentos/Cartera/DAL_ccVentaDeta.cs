using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccVentaDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccVentaDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccVentaDeta
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccVentaDeta_Add(DTO_ccVentaDeta ventaDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccVentaDeta   " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[NumDocCredito]   " +
                                               "    ,[NumDocSustituye]   " +
                                               "    ,[NumDocRecompra]   " +
                                               "    ,[Portafolio]   " +
                                               "    ,[CuotaID]   " +
                                               "    ,[VlrCuota]   " +
                                               "    ,[CuotasVend]   " +
                                               "    ,[VlrLibranza]   " +
                                               "    ,[VlrVenta]   " +
                                               "    ,[FactorCesion]   " +
                                               "    ,[VlrTotalDerechos] " +
                                               "    ,[VlrSustLibranza]   " +
                                               "    ,[VlrSustRecompra]   " +
                                               "    ,[VlrNeto]   " +
                                               "    ,[VlrProvGeneral]   " +
                                               "    ,[VlrProvComprador]   " +
                                               "    ,[VlrSdoCapital]   " +
                                               "    ,[VlrSdoAsistencias]   " +
                                               "    ,[VlrSdoOtros]   " +
                                               "    ,[CompradorFinal] " +
                                               "    ,[eg_ccCompradorCartera])   " +
                                               "  VALUES    " +
                                               "  (@NumeroDoc    " +
                                               "  ,@NumDocCredito  " +
                                               "  ,@NumDocSustituye  " +
                                               "  ,@NumDocRecompra  " +
                                               "  ,@Portafolio  " +
                                               "  ,@CuotaID  " +
                                               "  ,@VlrCuota  " +
                                               "  ,@CuotasVend  " +
                                               "  ,@VlrLibranza  " +
                                               "  ,@VlrVenta  " +
                                               "  ,@FactorCesion  " +
                                               "  ,@VlrTotalDerechos" +
                                               "  ,@VlrSustLibranza  " +
                                               "  ,@VlrSustRecompra  " +
                                               "  ,@VlrNeto " +
                                               "  ,@VlrProvGeneral " +
                                               "  ,@VlrProvComprador " +
                                               "  ,@VlrSdoCapital " +
                                               "  ,@VlrSdoAsistencias " +
                                               "  ,@VlrSdoOtros " +
                                               "  ,@CompradorFinal " +
                                               "  ,@eg_ccCompradorCartera)   ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocSustituye", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Portafolio", SqlDbType.Char, UDT_PortafolioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotasVend", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FactorCesion", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrTotalDerechos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSustLibranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSustRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrNeto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrProvGeneral", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrProvComprador", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSdoCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSdoAsistencias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSdoOtros", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorFinal", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = ventaDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = ventaDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocSustituye"].Value = ventaDeta.NumDocSustituye.Value;
                mySqlCommandSel.Parameters["@NumDocRecompra"].Value = ventaDeta.NumDocRecompra.Value;
                mySqlCommandSel.Parameters["@Portafolio"].Value = ventaDeta.Portafolio.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = ventaDeta.CuotaID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = ventaDeta.VlrCuota.Value;
                mySqlCommandSel.Parameters["@CuotasVend"].Value = ventaDeta.CuotasVend.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = ventaDeta.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrVenta"].Value = ventaDeta.VlrVenta.Value != null ? ventaDeta.VlrVenta.Value : ventaDeta.VlrRecompra.Value;
                mySqlCommandSel.Parameters["@FactorCesion"].Value = ventaDeta.FactorCesion.Value;
                mySqlCommandSel.Parameters["@VlrTotalDerechos"].Value = ventaDeta.VlrTotalDerechos.Value;
                mySqlCommandSel.Parameters["@VlrSustLibranza"].Value = ventaDeta.VlrSustLibranza.Value;
                mySqlCommandSel.Parameters["@VlrSustRecompra"].Value = ventaDeta.VlrSustRecompra.Value;
                mySqlCommandSel.Parameters["@VlrNeto"].Value = ventaDeta.VlrNeto.Value;
                mySqlCommandSel.Parameters["@VlrProvGeneral"].Value = ventaDeta.VlrProvGeneral.Value;
                mySqlCommandSel.Parameters["@VlrProvComprador"].Value = ventaDeta.VlrProvComprador.Value;
                mySqlCommandSel.Parameters["@VlrSdoCapital"].Value = ventaDeta.VlrSdoCapital.Value;
                mySqlCommandSel.Parameters["@VlrSdoAsistencias"].Value = ventaDeta.VlrSdoAsistencias.Value;
                mySqlCommandSel.Parameters["@VlrSdoOtros"].Value = ventaDeta.VlrSdoOtros.Value;
                mySqlCommandSel.Parameters["@CompradorFinal"].Value = ventaDeta.CompradorFinal.Value;
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
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
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccVentaDeta
        /// </summary>
        /// <returns></returns>
        public void DAL_ccVentaDeta_Update(DTO_ccVentaDeta ventaDeta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocSustituye", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Portafolio", SqlDbType.Char, UDT_PortafolioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuotaID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CuotasVend", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrLibranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FactorCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrTotalDerechos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrSustLibranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSustRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrNeto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrProvGeneral", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrProvComprador", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSdoCapital", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSdoAsistencias", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSdoOtros", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorFinal", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = ventaDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = ventaDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@NumDocSustituye"].Value = ventaDeta.NumDocSustituye.Value;
                mySqlCommandSel.Parameters["@NumDocRecompra"].Value = ventaDeta.NumDocRecompra.Value;
                mySqlCommandSel.Parameters["@Portafolio"].Value = ventaDeta.Portafolio.Value;
                mySqlCommandSel.Parameters["@CuotaID"].Value = ventaDeta.CuotaID.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = ventaDeta.VlrCuota.Value;
                mySqlCommandSel.Parameters["@CuotasVend"].Value = ventaDeta.CuotasVend.Value;
                mySqlCommandSel.Parameters["@VlrLibranza"].Value = ventaDeta.VlrLibranza.Value;
                mySqlCommandSel.Parameters["@VlrVenta"].Value = ventaDeta.VlrVenta.Value;
                mySqlCommandSel.Parameters["@FactorCesion"].Value = ventaDeta.FactorCesion.Value;
                mySqlCommandSel.Parameters["@VlrTotalDerechos"].Value = ventaDeta.VlrTotalDerechos.Value;
                mySqlCommandSel.Parameters["@VlrSustLibranza"].Value = ventaDeta.VlrSustLibranza.Value;
                mySqlCommandSel.Parameters["@VlrSustRecompra"].Value = ventaDeta.VlrSustRecompra.Value;
                mySqlCommandSel.Parameters["@VlrNeto"].Value = ventaDeta.VlrNeto.Value;
                mySqlCommandSel.Parameters["@VlrProvGeneral"].Value = ventaDeta.VlrProvGeneral.Value;
                mySqlCommandSel.Parameters["@VlrProvComprador"].Value = ventaDeta.VlrProvComprador.Value;
                mySqlCommandSel.Parameters["@VlrSdoCapital"].Value = ventaDeta.VlrSdoCapital.Value;
                mySqlCommandSel.Parameters["@VlrSdoAsistencias"].Value = ventaDeta.VlrSdoAsistencias.Value;
                mySqlCommandSel.Parameters["@VlrSdoOtros"].Value = ventaDeta.VlrSdoOtros.Value;
                mySqlCommandSel.Parameters["@CompradorFinal"].Value = ventaDeta.CompradorFinal.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccVentaDeta SET" +
                    "  NumDocSustituye = @NumDocSustituye  " +
                    "  ,NumDocRecompra = @NumDocRecompra  " +
                    "  ,Portafolio = @Portafolio  " +
                    "  ,CuotaID = @CuotaID  " +
                    "  ,VlrCuota = @VlrCuota  " +
                    "  ,CuotasVend = @CuotasVend  " +
                    "  ,VlrLibranza = @VlrLibranza  " +
                    "  ,VlrVenta = @VlrVenta  " +
                    "  ,FactorCesion = @FactorCesion  " +
                    "  ,VlrTotalDerechos = @VlrTotalDerechos " +
                    "  ,VlrSustLibranza = @VlrSustLibranza  " +
                    "  ,VlrSustRecompra = @VlrSustRecompra  " +
                    "  ,VlrNeto = @VlrNeto  " +
                    "  ,VlrProvGeneral = @VlrProvGeneral  " +
                    "  ,VlrProvComprador = @VlrProvComprador  " +
                    "  ,VlrSdoCapital = @VlrSdoCapital  " +
                    "  ,VlrSdoAsistencias = @VlrSdoAsistencias  " +
                    "  ,VlrSdoOtros = @VlrSdoOtros  " +
                    "  ,CompradorFinal = @CompradorFinal " +
                    " WHERE  NumeroDoc = @NumeroDoc AND NumDocCredito = @NumDocCredito ";
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
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDeta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimna el campo de la tabla ccVentaDeta
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccVentaDeta_Delete(DTO_ccVentaDeta ventaDeta)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = ventaDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = ventaDeta.NumDocCredito.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "DELETE FROM ccVentaDeta WHERE  NumeroDoc = @NumeroDoc AND NumDocCredito = @NumDocCredito ";
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
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDeta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de ccVentaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccVentaDeta</returns>
        public List<DTO_ccVentaDeta> DAL_ccVentaDeta_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                List<DTO_ccVentaDeta> result = new List<DTO_ccVentaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccVentaDeta with(nolock) WHERE NumeroDoc = @NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccVentaDeta r = new DTO_ccVentaDeta(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDeta_GetByID");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae un registro de DTO_ccVentaDeta con base a libranza
        /// </summary>
        /// <returns>retorna un registro de DTO_ccVentaDeta</returns>
        public DTO_ccVentaDeta DAL_ccVentaDeta_GetByNumDocLibranza(int NumDocCredito)
        {
            try
            {
                DTO_ccVentaDeta result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters["@NumDocCredito"].Value = NumDocCredito;

                mySqlCommand.CommandText = "SELECT * FROM ccVentaDeta with(nolock)  " +
                                           "WHERE NumDocCredito = @NumDocCredito";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccVentaDeta(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDeta_GetByLibranza");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que trae los registro de ccVentaDeta 
        /// </summary>
        /// <param name="actFlujoID">ACtividad de flujo ID</param>
        /// <returns></returns>
        public List<DTO_ccVentaDeta> DAL_ccVentaDeta_GetByActividadFlujo(string actFlujoID)
        {
            try
            {
                List<DTO_ccVentaDeta> result = new List<DTO_ccVentaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                #endregion
                #region Query Cooperativa
                mySqlCommand.CommandText =
                    "SELECT ventaDeta.*, cli.ClienteID as Cliente, cli.Descriptivo as Nombre, docu.Libranza " +
                    "FROM ccVentaDeta ventaDeta with(nolock) " +
                    "   INNER JOIN ccCreditoDocu docu with(nolock) ON docu.NumeroDoc = ventaDeta.NumDocCredito " +
                    "   INNER JOIN ccCliente cli with(nolock) ON cli.ClienteID = docu.ClienteID " +
                    "       AND cli.EmpresaGrupoID = docu.eg_ccCliente " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) ON ctrl.NumeroDoc = ventaDeta.NumeroDoc " +
                    "       AND ctrl.Estado = @Estado " +
                    "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = ventaDeta.NumDocCredito " +
                    "       AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd ";
                #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccVentaDeta dto = new DTO_ccVentaDeta(dr);
                    dto.Aprobado.Value = false;
                    dto.Rechazado.Value = false;
                    dto.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.ClienteID.Value = dr["Cliente"].ToString();
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDeta_GetByActividadFlujo");
                throw exception;
            }
        }

        #endregion
    }

}
