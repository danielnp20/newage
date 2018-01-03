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
    public class DAL_ccVentaDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccVentaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public DTO_ccVentaDocu DAL_ccVentaDocu_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccVentaDocu result = new DTO_ccVentaDocu();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = 
                    "SELECT venta.*, FechaDoc FROM ccVentaDocu venta with(nolock)  " +
                    "   INNER JOIN glDocumentoControl ctrl with(nolock) ON venta.NumeroDoc = ctrl.NumeroDoc " +
                    "WHERE venta.NumeroDoc = @NumeroDoc";
      
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_ccVentaDocu(dr);
                    result.FechaVenta.Value = Convert.ToDateTime(dr["FechaDoc"]);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccVentaDocu_Add(DTO_ccVentaDocu ventaDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query
                mySqlCommandSel.CommandText = "    INSERT INTO ccVentaDocu   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[DocFactura] " +
                                               "    ,[CompradorCarteraID] " +
                                               "    ,[TipoVenta] " +
                                               "    ,[Oferta] " +
                                               "    ,[Observacion] " +
                                               "    ,[FechaPago1] " +
                                               "    ,[FechaPagoUlt] " +
                                               "    ,[NumCuotas] " +
                                               "    ,[TasaDescuento] " +
                                               "    ,[FactorCesion] " +
                                               "    ,[Valor] " +
                                               "    ,[Iva] " +
                                               "    ,[VlrVenta] " +
                                               "    ,[VlrSustRecompra] " +
                                               "    ,[NoComercialInd] " +
                                               "    ,[FactorUtilidadInd] " +
                                               "    ,[RefCartaAceptacion] " +
                                               "    ,[FechaAceptacion] " +
                                               "    ,[FechaLiquida] " +
                                               "    ,[eg_ccCompradorCartera] ) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@DocFactura " +
                                               "  ,@CompradorCarteraID " +
                                               "  ,@TipoVenta " +
                                               "  ,@Oferta " +
                                               "  ,@Observacion " +
                                               "  ,@FechaPago1 " +
                                               "  ,@FechaPagoUlt " +
                                               "  ,@NumCuotas " +
                                               "  ,@TasaDescuento " +
                                               "  ,@FactorCesion " +
                                               "  ,@Valor " +
                                               "  ,@Iva " +
                                               "  ,@VlrVenta " +
                                               "  ,@VlrSustRecompra " +
                                               "  ,@NoComercialInd " +
                                               "  ,@FactorUtilidadInd " +
                                               "  ,@RefCartaAceptacion" +
                                               "  ,@FechaAceptacion " +
                                               "  ,@FechaLiquida " +
                                               "  ,@eg_ccCompradorCartera) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoVenta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaPago1", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaPagoUlt", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@NumCuotas", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TasaDescuento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSustRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NoComercialInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FactorUtilidadInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@RefCartaAceptacion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@FechaAceptacion", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaLiquida", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = ventaDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocFactura"].Value = ventaDocu.DocFactura.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = ventaDocu.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@TipoVenta"].Value = ventaDocu.TipoVenta.Value;
                mySqlCommandSel.Parameters["@Oferta"].Value = ventaDocu.Oferta.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = ventaDocu.Observacion.Value;
                mySqlCommandSel.Parameters["@FechaPago1"].Value = ventaDocu.FechaPago1.Value;
                mySqlCommandSel.Parameters["@FechaPagoUlt"].Value = ventaDocu.FechaPagoUlt.Value;
                mySqlCommandSel.Parameters["@NumCuotas"].Value = ventaDocu.NumCuotas.Value;
                mySqlCommandSel.Parameters["@TasaDescuento"].Value = ventaDocu.TasaDescuento.Value;
                mySqlCommandSel.Parameters["@FactorCesion"].Value = ventaDocu.FactorCesion.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = ventaDocu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = ventaDocu.Iva.Value;
                mySqlCommandSel.Parameters["@VlrVenta"].Value = ventaDocu.VlrVenta.Value;
                mySqlCommandSel.Parameters["@VlrSustRecompra"].Value = ventaDocu.VlrSustRecompra.Value;
                mySqlCommandSel.Parameters["@NoComercialInd"].Value = ventaDocu.NoComercialInd.Value;
                mySqlCommandSel.Parameters["@FactorUtilidadInd"].Value = ventaDocu.FactorUtilidadInd.Value;
                mySqlCommandSel.Parameters["@RefCartaAceptacion"].Value = ventaDocu.RefCartaAceptacion.Value;
                mySqlCommandSel.Parameters["@FechaAceptacion"].Value = ventaDocu.FechaAceptacion.Value;
                mySqlCommandSel.Parameters["@FechaLiquida"].Value = ventaDocu.FechaLiquida.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccVentaDocu_Update(DTO_ccVentaDocu ventaDocu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoVenta", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaPago1", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaPagoUlt", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@TasaDescuento", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorCesion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrVenta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@VlrSustRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NoComercialInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@FactorUtilidadInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@RefCartaAceptacion", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@FechaAceptacion", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaLiquida", SqlDbType.SmallDateTime);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = ventaDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocFactura"].Value = ventaDocu.DocFactura.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = ventaDocu.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@TipoVenta"].Value = ventaDocu.TipoVenta.Value;
                mySqlCommandSel.Parameters["@Oferta"].Value = ventaDocu.Oferta.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = ventaDocu.Observacion.Value;
                mySqlCommandSel.Parameters["@FechaPago1"].Value = ventaDocu.FechaPago1.Value;
                mySqlCommandSel.Parameters["@FechaPagoUlt"].Value = ventaDocu.FechaPagoUlt.Value;
                mySqlCommandSel.Parameters["@TasaDescuento"].Value = ventaDocu.TasaDescuento.Value;
                mySqlCommandSel.Parameters["@FactorCesion"].Value = ventaDocu.FactorCesion.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = ventaDocu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = ventaDocu.Iva.Value;
                mySqlCommandSel.Parameters["@VlrVenta"].Value = ventaDocu.VlrVenta.Value;
                mySqlCommandSel.Parameters["@VlrSustRecompra"].Value = ventaDocu.VlrSustRecompra.Value;
                mySqlCommandSel.Parameters["@NoComercialInd"].Value = ventaDocu.NoComercialInd.Value;
                mySqlCommandSel.Parameters["@FactorUtilidadInd"].Value = ventaDocu.FactorUtilidadInd.Value;
                mySqlCommandSel.Parameters["@RefCartaAceptacion"].Value = ventaDocu.RefCartaAceptacion.Value;
                mySqlCommandSel.Parameters["@FechaAceptacion"].Value = ventaDocu.FechaAceptacion.Value;
                mySqlCommandSel.Parameters["@FechaLiquida"].Value = ventaDocu.FechaLiquida.Value;
                #endregion
                #region Query
                mySqlCommandSel.CommandText =
                    "UPDATE ccVentaDocu SET" +
                    "  DocFactura = @DocFactura " +
                    "  ,CompradorCarteraID = @CompradorCarteraID " +
                    "  ,TipoVenta = @TipoVenta " +
                    "  ,Oferta = @Oferta " +
                    "  ,Observacion = @Observacion " +
                    "  ,FechaPago1 = @FechaPago1 " +
                    "  ,FechaPagoUlt = @FechaPagoUlt " +
                    "  ,TasaDescuento = @TasaDescuento " +
                    "  ,FactorCesion = @FactorCesion " +
                    "  ,Valor = @Valor " +
                    "  ,Iva = @Iva " +
                    "  ,VlrVenta = @VlrVenta " +
                    "  ,VlrSustRecompra = @VlrSustRecompra " +
                    "  ,NoComercialInd = @NoComercialInd " +
                    "  ,FactorUtilidadInd = @FactorUtilidadInd " +
                    "  ,RefCartaAceptacion = @RefCartaAceptacion " +
                    "  ,FechaAceptacion = @FechaAceptacion " +
                    "  ,FechaLiquida = @FechaLiquida " +
                    " WHERE  NumeroDoc = @NumeroDoc ";
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDocu_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Funcion que trae los registro de ccRecompraDocu para la venta
        /// </summary>
        /// <param name="actFlujoID">ACtividad de flujo ID</param>
        /// <param name="compradorCarteraID">Comprador de cartera que se usa para filtrar la busqueda</param>
        /// <returns></returns>
        public List<DTO_ccVentaDocu> DAL_ccVentaDocu_GetByFilter(string compradorCarteraID, string oferta, DateTime? MesINI, DateTime? MesFIN)
        {
            try
            {
                List<DTO_ccVentaDocu> results = new List<DTO_ccVentaDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string where = string.Empty;
                #region Creacion Parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;

                if (!string.IsNullOrEmpty(compradorCarteraID))
                {
                    mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                    where = "and venta.CompradorCarteraID = @CompradorCarteraID";
                }

                if (!string.IsNullOrEmpty(oferta))
                {
                    mySqlCommand.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                    mySqlCommand.Parameters["@Oferta"].Value = oferta;
                    where = "and venta.Oferta = @Oferta";
                }

                if (MesINI.HasValue)
                {
                    mySqlCommand.Parameters.Add("@MesINI", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@MesINI"].Value = MesINI.Value.Date;
                    where = "and FechaDoc >= @MesINI";
                }

                if (MesFIN.HasValue)
                {
                    mySqlCommand.Parameters.Add("@MesFIN", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@MesFIN"].Value = MesFIN.Value.Date;
                    where = "and FechaDoc <= @MesFIN";
                }

                #endregion
                #region Query
                mySqlCommand.CommandText =
                    "SELECT venta.* , venta.Oferta, venta.FactorCesion,docCtrl.FechaDoc " +
                        "FROM ccVentaDocu venta with(nolock) " +
                        "INNER JOIN glDocumentoControl docCtrl with(nolock) ON venta.NumeroDoc = docCtrl.NumeroDoc " +
                    "WHERE EmpresaID = @EmpresaID and docCtrl.Estado = @Estado " + where;
                #endregion 

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccVentaDocu ventaDocu = new DTO_ccVentaDocu(dr);
                    results.Add(ventaDocu);
                }
                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDocu_GetByCompradorCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos las ofertas de preventa y venta
        /// </summary>
        /// <param name="compradorCarteraID">Comprador a consultar</param>
        /// <param name="isVenta">Indicador para establecer si trae las ofertas de preventas ode ventas</param>
        /// <returns>retorna una lista de ccCreditoDocu</returns>
        public List<string> DAL_ccVentaDocu_GetOfertasForPreventa(string actFlujoID, string compradorCarteraID, bool isVenta, string usuarioID)
        {
            try
            {
                List<string> results = new List<string>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;

                var ventaQuery = string.Empty;
                if(isVenta)
                {
                    mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                    mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                    mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);

                    mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                    mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                    mySqlCommand.Parameters["@CerradoInd"].Value = false;

                    ventaQuery =
                        "INNER JOIN ccCreditoDocu cred with(nolock)on venDocu.NumeroDoc = cred.DocVenta AND venDocu.CompradorCarteraID = @CompradorCarteraID " +
                        "INNER JOIN glActividadEstado act with(nolock) on act.NumeroDoc = cred.NumeroDoc " +
                        "	AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd " +
                        "INNER JOIN glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID AND perm.areaFuncionalID = ctrl.areaFuncionalID " +
                        "	AND perm.actividadFlujoID = @ActividadFlujoID  AND perm.UsuarioID = @UsuarioID "; 
                }

                mySqlCommand.CommandText =
                    "SELECT distinct Oferta " +
                    "FROM ccVentaDocu venDocu with(nolock) " +
                    "	INNER JOIN glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = venDocu.NumeroDoc AND ctrl.Estado = @Estado " + ventaQuery +
                    "WHERE ctrl.EmpresaID = @EmpresaID and venDocu.CompradorCarteraID = @CompradorCarteraID";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    results.Add(dr["Oferta"].ToString());
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CreditoDocu_GetCreditosForPreventa");
                throw exception;
            }
        }

        #endregion
    }

}
