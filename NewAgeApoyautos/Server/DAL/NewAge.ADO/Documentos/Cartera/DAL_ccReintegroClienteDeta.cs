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
    public class DAL_ccReintegroClienteDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccReintegroClienteDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccReintegroClienteDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccReintegroClienteDeta DAL_ccReintegroClienteDeta_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccReintegroClienteDeta result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccReintegroClienteDeta with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccReintegroClienteDeta(dr);
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccReintegroClienteDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public DTO_ccReintegroClienteDeta DAL_ccReintegroClienteDeta_GetByCreditoID(int NumDocCredito)
        {
            try
            {
                DTO_ccReintegroClienteDeta result = null;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters["@NumDocCredito"].Value = NumDocCredito;

                mySqlCommand.CommandText = "SELECT * FROM ccReintegroClienteDeta with(nolock)  " +
                                           "WHERE NumDocCredito = @NumDocCredito";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccReintegroClienteDeta(dr);
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los registros de DTO_ccReintegroClienteDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        public List<DTO_ccReintegroClienteDeta> DAL_ccReintegroClienteDeta_GetAll()
        {
            try
            {
                List<DTO_ccReintegroClienteDeta> result = new List<DTO_ccReintegroClienteDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT * FROM ccReintegroClienteDeta with(nolock) ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReintegroClienteDeta reintegroCliente = new DTO_ccReintegroClienteDeta(dr);
                    result.Add(reintegroCliente);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccIncorporacionDeta
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccReintegroClienteDeta_Add(DTO_ccReintegroClienteDeta reintegroClienteDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO ccReintegroClienteDeta   " +
                                               "    ([NumeroDoc]   " +
                                               "    ,[NumDocCredito]   " +
                                               "    ,[ClienteID] " +
                                               "    ,[CuentaID] " +
                                               "    ,[TerceroID] " +
                                               "    ,[Nombre] " +
                                               "    ,[ComponenteCarteraID] " +
                                               "    ,[FechaReintegro] " +
                                               "    ,[FechaAprobacionReintegro] " +
                                               "    ,[Valor] " +
                                               "    ,[Observacion] " +
                                               "    ,[eg_ccCliente] " +
                                               "    ,[eg_coTercero] " +
                                               "    ,[eg_coPlanCuenta] " +
                                               "    ,[eg_ccCarteraComponente])  " +
                                               "VALUES    " +
                                               "    (@NumeroDoc    " +
                                               "    ,@NumDocCredito " +
                                               "    ,@ClienteID " +
                                               "    ,@CuentaID " +
                                               "    ,@TerceroID " +
                                               "    ,@Nombre " +
                                               "    ,@ComponenteCarteraID " +
                                               "    ,@FechaReintegro " +
                                               "    ,@FechaAprobacionReintegro " +
                                               "    ,@Valor " +
                                               "    ,@Observacion " +
                                               "    ,@eg_ccCliente " +
                                               "    ,@eg_coTercero " +
                                               "    ,@eg_coPlanCuenta " +
                                               "    ,@eg_ccCarteraComponente) ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@FechaReintegro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaAprobacionReintegro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCliente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = reintegroClienteDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = reintegroClienteDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = reintegroClienteDeta.ClienteID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = reintegroClienteDeta.CuentaID.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = reintegroClienteDeta.TerceroID.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = reintegroClienteDeta.Nombre.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = reintegroClienteDeta.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@FechaReintegro"].Value = reintegroClienteDeta.FechaReintegro.Value;
                mySqlCommandSel.Parameters["@FechaAprobacionReintegro"].Value = reintegroClienteDeta.FechaAprobacionReintegro.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = reintegroClienteDeta.Valor.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = reintegroClienteDeta.Observacion.Value;
                mySqlCommandSel.Parameters["@eg_ccCliente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCliente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccIncorporacionDeta
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccReintegroClienteDeta_Update(DTO_ccReintegroClienteDeta reintegroClienteDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                                           "UPDATE ccReintegroClienteDeta SET" +
                                           " ClienteID = @ClienteID " +
                                           " ,CuentaID = @CuentaID " +
                                           " ,TerceroID = @TerceroID " +
                                           " ,Nombre = @Nombre " +
                                           " ,ComponenteCarteraID = @ComponenteCarteraID " +
                                           " ,FechaReintegro = @FechaReintegro " +
                                           " ,FechaAprobacionReintegro = @FechaAprobacionReintegro " +
                                           " ,Valor = @Valor " +
                                           " ,Observacion = @Observacion " +
                                           "WHERE  Consecutivo = @Consecutivo ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_CuentaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Nombre", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@FechaReintegro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaAprobacionReintegro", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = reintegroClienteDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = reintegroClienteDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@ClienteID"].Value = reintegroClienteDeta.ClienteID.Value;
                mySqlCommandSel.Parameters["@CuentaID"].Value = reintegroClienteDeta.CuentaID.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = reintegroClienteDeta.TerceroID.Value;
                mySqlCommandSel.Parameters["@Nombre"].Value = reintegroClienteDeta.Nombre.Value;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = reintegroClienteDeta.ComponenteCarteraID.Value;
                mySqlCommandSel.Parameters["@FechaReintegro"].Value = reintegroClienteDeta.FechaReintegro.Value;
                mySqlCommandSel.Parameters["@FechaAprobacionReintegro"].Value = reintegroClienteDeta.FechaAprobacionReintegro.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = reintegroClienteDeta.Valor.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = reintegroClienteDeta.Observacion.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = reintegroClienteDeta.Consecutivo.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccReintegroClienteDeta
        /// </summary>
        /// <returns></returns>
        public bool DAL_ccReintegroClienteDeta_Delete(int numDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccReintegroClienteDeta WHERE NumeroDoc=@NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        #region Pagos Especiales

        /// <summary>
        /// Trae todos los registros de DTO_ccCreditoComponentes
        /// </summary>
        /// <param name="compCarteraID">Identificar del componente filtra la lista de los componentes</param>
        /// <param name="isCooperativa">Indentifica si debe buscar en la tabla de cooperativa o de financiera</param>
        /// <returns>retorna una lista de DTO_ccSolicitudComponentes</returns>
        public DTO_ReintegrosCartera DAL_ccReintegroClienteDeta_GetByCompCartera(string actFlujoID, string compCarteraID, DateTime periodo)
        {
            try
            {
                DTO_ReintegrosCartera result = new DTO_ReintegrosCartera();
                List<DTO_ccReintegroClienteDeta> detalleReintegros = new List<DTO_ccReintegroClienteDeta>();
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@ComponenteCarteraID"].Value = compCarteraID;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText = "SELECT CASE WHEN reintegroCli.NumeroDoc IS NULL THEN 0 ELSE reintegroCli.NumeroDoc  END AS NumeroDoc, " +
                                           "    cd.NumeroDoc AS NumDocCredito, cd.Libranza, cd.ClienteID, cli.Descriptivo as Nombre, cc.TotalValor, " +
                                           "    cd.AsesorID, ter.TerceroID " +
                                           "FROM ccCreditoDocu cd with(nolock) " +
                                           "    INNER JOIN ccCreditoComponentes cc with(nolock) on cc.NumeroDoc = cd.NumeroDoc AND cc.DocPago IS NULL " +
                                           "    INNER JOIN ccCliente cli with(nolock) on cli.ClienteID = cd.ClienteID AND cli.EmpresaGrupoID = cd.eg_ccCliente " +
                                           "    INNER JOIN coTercero ter with(nolock) on cli.TerceroID = ter.TerceroID  AND cli.eg_coTercero = ter.EmpresaGrupoID " +
                                           "    INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = cd.NumeroDoc AND ctrl.Estado = @Estado " +
                                           "    INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cd.NumeroDoc " +
                                           "        AND act.CerradoInd = @CerradoInd and act.ActividadFlujoID = @ActividadFlujoID " +
                                           "    LEFT JOIN ccReintegroClienteDeta reintegroCli with(nolock) on reintegroCli.NumDocCredito = cd.NumeroDoc " +
                                           "        AND reintegroCli.ComponenteCarteraID = @ComponenteCarteraID " +
                                           "WHERE cc.ComponenteCarteraID = @ComponenteCarteraID AND ctrl.FechaDoc <= @Periodo AND cd.EmpresaID = @EmpresaID " +
                                           "ORDER BY cd.AsesorID, CAST(cd.Libranza AS INTEGER) ";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReintegroClienteDeta reintegro = new DTO_ccReintegroClienteDeta();
                    reintegro.ComponenteCarteraID.Value = compCarteraID;
                    reintegro.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                    reintegro.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                    reintegro.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    reintegro.ClienteID.Value = dr["ClienteID"].ToString();
                    reintegro.TerceroID.Value = dr["TerceroID"].ToString();
                    reintegro.Nombre.Value = dr["Nombre"].ToString();
                    reintegro.Valor.Value = Convert.ToDecimal(dr["TotalValor"]);
                    reintegro.AsesorID.Value = dr["AsesorID"].ToString();
                    reintegro.Aprobado.Value = true;

                    detalleReintegros.Add(reintegro);
                }
                dr.Close();

                result.AddData(detalleReintegros);
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCreditoComponentes_GetForReintegro");
                throw exception;
            }
        }

        /// <summary>
        /// Rertorna los Documentos pertenecientes a una pagaduria
        /// </summary>
        /// <returns></returns>
        public DTO_ReintegrosCartera DAL_ccReintegroClienteDeta_GetByCompCarteraForAprob(string actFlujoID, string componenteCarteraID)
        {
            try
            {
                DTO_ReintegrosCartera result = new DTO_ReintegrosCartera();
                List<DTO_ccReintegroClienteDeta> detalleReintegros = new List<DTO_ccReintegroClienteDeta>();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ComponenteCarteraID", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@ComponenteCarteraID"].Value = componenteCarteraID;
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT reintegroDeta.*, crediDocu.Libranza, crediDocu.AsesorID " +
                    "FROM ccReintegroClienteDeta  reintegroDeta WITH(NOLOCK) " +
                    "   INNER JOIN ccCreditoDocu crediDocu with(nolock) on crediDocu.NumeroDoc = reintegroDeta.NumDocCredito AND EmpresaID = @EmpresaID " +
                    "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = reintegroDeta.NumeroDoc " +
                    "        AND act.CerradoInd = @CerradoInd and act.ActividadFlujoID = @ActividadFlujoID " +
                    "WHERE  reintegroDeta.ComponenteCarteraID = @ComponenteCarteraID And CuentaID IS NULL " +
                    "ORDER BY CAST(crediDocu.Libranza AS INTEGER) ";
                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReintegroClienteDeta reintegro = new DTO_ccReintegroClienteDeta(dr);
                    reintegro.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    reintegro.AsesorID.Value = dr["AsesorID"].ToString();
                    reintegro.Aprobado.Value = true;
                    reintegro.Rechazado.Value = false;
                    detalleReintegros.Add(reintegro);
                }
                dr.Close();
                result.AddData(detalleReintegros);
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_GetByComponenteCartera");
                throw exception;
            }
        }

        #endregion

        #region Reintegro Clientes

        /// <summary>
        /// Rertorna los reintegraos por cuenta y tercero
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccReintegroClienteDeta> DAL_ccReintegroClienteDeta_GetByCuenta(DateTime periodo, string libroID, string cuentaID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_ccReintegroClienteDeta> results = new List<DTO_ccReintegroClienteDeta>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@BalanceTipoID", SqlDbType.Char, UDT_BalanceTipoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                mySqlCommandSel.Parameters["@BalanceTipoID"].Value = libroID;
                mySqlCommandSel.Parameters["@CuentaID"].Value = cuentaID;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT Saldo.*, deta.NumeroDoc AS NumeroDocReintegro, deta.CuentaReintegro, CASE WHEN deta.Valor IS NULL THEN 0 ELSE deta.Valor END AS VlrReintegro " + 
                    "FROM " +
                    "( " +
                    "	select saldo.TerceroID, terc.Descriptivo as Nombre, cred.Libranza, cred.NumeroDoc AS NumDocCredito, SUM(DebitoML + CreditoML + SaldoIniML) AS Saldo " +
                    "	from Vista_coCuentaSaldo saldo with(nolock) " +
                    "		inner join coTercero terc with(nolock) on saldo.TerceroID = terc.TerceroID AND terc.EmpresaGrupoID = saldo.eg_coTercero " +
                    "		left join ccCreditoDocu cred with(nolock) on saldo.IdentificadorTR = cred.NumeroDoc " +
                    "	where saldo.EmpresaID = @EmpresaID and PeriodoID=@PeriodoID and BalanceTipoID=@BalanceTipoID and saldo.CuentaID=@CuentaID " +
                    "	group by saldo.TerceroID, terc.Descriptivo, cred.Libranza, cred.NumeroDoc " +
                    ") AS saldo " +
                    "LEFT JOIN " +
                    "( " +
                    "	SELECT reint.*, ctrl.CuentaID AS CuentaReintegro " +
                    "	FROM ccReintegroClienteDeta reint WITH(NOLOCK) " +
                    "       INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) on reint.NumeroDoc = ctrl.NumeroDoc " +
                    "	WHERE ctrl.EmpresaID = @EmpresaID and FechaAprobacionReintegro IS NULL AND reint.CuentaID = @CuentaID " +
                    ") AS deta ON saldo.TerceroID = deta.TerceroID " +
                    "WHERE saldo.Saldo < 0 " +
                    "order by TerceroID ";
                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReintegroClienteDeta reintegro = new DTO_ccReintegroClienteDeta();
                    reintegro.CuentaID.Value = cuentaID; 
                    reintegro.TerceroID.Value = dr["TerceroID"].ToString();
                    reintegro.Nombre.Value = dr["Nombre"].ToString();
                    reintegro.ValorMax.Value = Convert.ToDecimal(dr["Saldo"]) * -1;
                    reintegro.Valor.Value = Convert.ToDecimal(dr["VlrReintegro"]);

                    if (!string.IsNullOrWhiteSpace(dr["NumDocCredito"].ToString()))
                        reintegro.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);

                    if (!string.IsNullOrWhiteSpace(dr["NumeroDocReintegro"].ToString()))
                    {
                        reintegro.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDocReintegro"]);
                        reintegro.CuentaReintegroID.Value = dr["CuentaReintegro"].ToString();
                        if (!string.IsNullOrWhiteSpace(dr["CuentaReintegro"].ToString()))
                        {
                            reintegro.ValorAjuste.Value = reintegro.Valor.Value;
                            reintegro.ValorPago.Value = 0;
                        }
                        else
                        {
                            reintegro.ValorAjuste.Value = 0;
                            reintegro.ValorPago.Value = reintegro.Valor.Value;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dr["Libranza"].ToString()))
                        reintegro.Libranza.Value = Convert.ToInt32(dr["Libranza"]);

                    reintegro.Aprobado.Value = reintegro.Valor.Value > 0 ? true : false;
                    reintegro.Rechazado.Value = false;
                    results.Add(reintegro);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_GetByComponenteCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Rertorna los Documentos pertenecientes a una pagaduria
        /// </summary>
        /// <returns></returns>
        public List<DTO_ccReintegroClienteDeta> DAL_ccReintegroClienteDeta_GetByCuentaForAprob(string actFlujoID, string cuentaID)
        {
            try
            {
                List<DTO_ccReintegroClienteDeta> results = new List<DTO_ccReintegroClienteDeta>();
               
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CuentaID", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Estado"].Value = (byte)EstadoDocControl.ParaAprobacion;
                mySqlCommandSel.Parameters["@CerradoInd"].Value = false;
                mySqlCommandSel.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommandSel.Parameters["@CuentaID"].Value = cuentaID;
                #region CommandText
                mySqlCommandSel.CommandText =
                    "SELECT reintegroDeta.*, crediDocu.Libranza, terc.Descriptivo, ctrl.CuentaID AS CuentaReintegro " +
                    "FROM ccReintegroClienteDeta reintegroDeta WITH(NOLOCK) " +
                    "   INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) on reintegroDeta.NumeroDoc = ctrl.NumeroDoc and ctrl.estado = @Estado " + 
                    "	INNER JOIN coTercero terc with(nolock) on reintegroDeta.TerceroID = terc.TerceroID AND terc.EmpresaGrupoID = reintegroDeta.eg_coTercero " +
                    "   LEFT JOIN ccCreditoDocu crediDocu with(nolock) on crediDocu.NumeroDoc = reintegroDeta.NumDocCredito " +
                    "   INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = reintegroDeta.NumeroDoc " +
                    "        AND act.CerradoInd = @CerradoInd and act.ActividadFlujoID = @ActividadFlujoID " +
                    "WHERE ctrl.EmpresaID=@EmpresaID AND reintegroDeta.CuentaID = @CuentaID And ComponenteCarteraID IS NULL AND FechaAprobacionReintegro IS NULL";

                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccReintegroClienteDeta reintegro = new DTO_ccReintegroClienteDeta(dr);

                    if (!string.IsNullOrWhiteSpace(dr["Libranza"].ToString()))
                        reintegro.Libranza.Value = Convert.ToInt32(dr["Libranza"]);

                    reintegro.Nombre.Value = dr["Descriptivo"].ToString();
                    reintegro.CuentaReintegroID.Value = dr["CuentaReintegro"].ToString(); 
                    reintegro.Aprobado.Value = true;
                    reintegro.Rechazado.Value = false;
                    results.Add(reintegro);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccReintegroClienteDeta_GetByCuentaForAprob");
                throw exception;
            }
        }

        #endregion

        #endregion
    }

}
