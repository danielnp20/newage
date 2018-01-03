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
    public class DAL_ccFlujoCesionDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccFlujoCesionDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de ccFlujoCesionDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public List<DTO_ccFlujoCesionDocu> DAL_ccFlujoCesionDocu_GetByID(int NumeroDoc)
        {
            try
            {
                List<DTO_ccFlujoCesionDocu> result = new List<DTO_ccFlujoCesionDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
                mySqlCommand.CommandText = "SELECT * FROM ccFlujoCesionDocu with(nolock)  " +
                                                           "WHERE NumeroDoc = NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccFlujoCesionDocu r = new DTO_ccFlujoCesionDocu(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_FlujoCesionDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccFlujoCesionDocu
        /// </summary>
        /// <returns></returns>
        public void DAL_ccFlujoCesionDocu_Add(DTO_ccFlujoCesionDocu flujoCesionDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = " INSERT INTO ccFlujoCesionDocu   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[CompradorCarteraID] " +
                                               "    ,[TerceroPago] " +
                                               "    ,[Valor] " +
                                               "    ,[Iva] " +
                                               "    ,[NumeroDocCXP] " +
                                               "    ,[eg_ccCompradorCartera] " +
                                               "    ,[eg_coTercero] ) " +
                                               " VALUES " +
                                               "    (@NumeroDoc " +
                                               "    ,@CompradorCarteraID " +
                                               "    ,@TerceroPago " +
                                               "    ,@Valor " +
                                               "    ,@Iva " +
                                               "    ,@NumeroDocCXP " +
                                               "    ,@eg_ccCompradorCartera " +
                                               "    ,@eg_coTercero) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroPago", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = flujoCesionDoc.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = flujoCesionDoc.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@TerceroPago"].Value = flujoCesionDoc.TerceroPago.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = flujoCesionDoc.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = flujoCesionDoc.Iva.Value;
                mySqlCommandSel.Parameters["@NumeroDocCXP"].Value = flujoCesionDoc.NumeroDocCXP.Value;
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccFlujoCesionDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccFlujoCesionDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccFlujoCesionDocu_Update(DTO_ccFlujoCesionDocu flujoCesionDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TerceroPago", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = flujoCesionDoc.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = flujoCesionDoc.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@TerceroPago"].Value = flujoCesionDoc.TerceroPago.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = flujoCesionDoc.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = flujoCesionDoc.Iva.Value;
                mySqlCommandSel.Parameters["@NumeroDocCXP"].Value = flujoCesionDoc.NumeroDocCXP.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccFlujoCesionDocu SET" +
                    "  ,CompradorCarteraID = @CompradorCarteraID " +
                    "  ,TerceroPago = @TerceroPago " +
                    "  ,Valor = @Valor " +
                    "  ,Iva = @Iva " +
                    "  ,NumeroDocCXP = @NumeroDocCXP " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccVentaDocu_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Funcion que trae los registro de ccRecompraDocu para la venta
        /// </summary>
        /// <param name="actFlujoID">ACtividad de flujo ID</param>
        /// <param name="fechaPeriodo">Fecha de pago de las cuotas del flujo</param>
        /// <param name="pagoCuota">Identifica si la cuota del flujo debe estar pagada o no</param>
        /// <returns></returns>
        public List<DTO_ccFlujoCesionDocu> DAL_ccFlujoCesionDocu_GetForPago(string actFlujoID, string usuarioID, DateTime fechaPeriodo, string oferta, 
            int? libranza, string comprador)
        {
            try
            {
                List<DTO_ccFlujoCesionDocu> result = new List<DTO_ccFlujoCesionDocu>();
                List<DTO_ccFlujoCesionDocu> flujoTemp = new List<DTO_ccFlujoCesionDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Carga los filtros
                
                string ventaFiltro = string.Empty; 
                string creditoFiltro = string.Empty;
                
                //Oferta
                if (!string.IsNullOrWhiteSpace(comprador))
                {
                    mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommand.Parameters["@CompradorCarteraID"].Value = comprador;

                    ventaFiltro += " AND venDocu.CompradorCarteraID = @CompradorCarteraID ";
                }

                //Comprador
                if (!string.IsNullOrWhiteSpace(oferta))
                {
                    mySqlCommand.Parameters.Add("@Oferta", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommand.Parameters["@Oferta"].Value = oferta;

                    ventaFiltro += " AND venDocu.Oferta = @Oferta ";
                }

                //Libranza
                if(libranza.HasValue)
                {
                    mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                    mySqlCommand.Parameters["@Libranza"].Value = libranza.Value;

                    creditoFiltro += "AND credDocu.Libranza = @Libranza";
                }

                #endregion
                #region Creacion Parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaPeriodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@FechaPeriodo"].Value = fechaPeriodo;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommand.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;

                #endregion
                #region Query
                mySqlCommand.CommandText =
                    "SELECT compCar.InversionistaFinalInd ,compCar.Descriptivo as NombreComprador, compCar.TerceroID, credDocu.CompradorCarteraID, credDocu.CompradorFinalID, " +
                    "       credDocu.Libranza, credDocu.NumeroDoc as NumDocCredito, credDocu.ClienteID, cli.Descriptivo as Nombre, venDocu.Oferta, venDocu.NumeroDoc as VentadocNum, " +
                    "       planPag.CuotaID, planPag.Consecutivo as CreditoCuotaNum, planpag.FechaCuota, planPag.VlrCapitalCesion, planPag.VlrUtilidadCesion, planPag.VlrDerechosCesion, " +
                    "       venDeta.VlrCuota,  venDeta.Portafolio " +
                    "FROM ccVentaDeta venDeta WITH(NOLOCK) " +
                    "    INNER JOIN ccVentaDocu venDocu WITH(NOLOCK) ON venDocu.NumeroDoc = venDeta.NumeroDoc " + ventaFiltro +
                    "    INNER JOIN glDocumentoControl ctrlVenta WITH(NOLOCK) ON ctrlVenta.NumeroDoc = venDocu.NumeroDoc AND ctrlVenta.EmpresaID = @EmpresaID and ctrlVenta.Estado=@Estado " +
                    "    INNER JOIN ccCreditoPlanPagos planPag WITH(NOLOCK) ON planPag.NumeroDoc = venDeta.NumDocCredito " +
                    "        AND DATEDIFF(month,@FechaPeriodo, planPag.FechaFlujo) < 1 " +
                    "    INNER JOIN ccCreditoDocu credDocu WITH(NOLOCK) ON credDocu.NumeroDoc = venDeta.NumDocCredito AND credDocu.EmpresaID = @EmpresaID " + creditoFiltro +
                    "    INNER JOIN ccCompradorCartera compCar WITH(NOLOCK) ON compCar.CompradorCarteraID = credDocu.CompradorCarteraID " +
                    "        AND compCar.EmpresaGrupoID = credDocu.eg_ccCompradorCartera " +
                    "    LEFT JOIN ccFlujoCesionDeta flujoDeta WITH(NOLOCK) ON  flujoDeta.VentadocNum = venDocu.NumeroDoc " +
                    "       AND flujoDeta.CreditoCuotaNum = planPag.Consecutivo " +
                    "    INNER JOIN ccCliente cli WITH(NOLOCK) ON cli.ClienteID = credDocu.ClienteID " +
                    "        AND cli.EmpresaGrupoID = credDocu.eg_ccCliente " +
                    "    INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = credDocu.NumeroDoc AND ctrl.EmpresaID = @EmpresaID " +
                    "    INNER JOIN glActividadEstado act WITH(NOLOCK) on act.NumeroDoc = credDocu.NumeroDoc " +
                    "        AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd " +
                    "    INNER JOIN glActividadPermiso perm WITH(NOLOCK) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "        AND perm.areaFuncionalID = ctrl.areaFuncionalID  AND perm.actividadFlujoID = @ActividadFlujoID " +
                    "        AND perm.UsuarioID = @UsuarioID " +
                    "WHERE flujoDeta.NumeroDoc is NULL and  " +
                    " ((CASE WHEN (compCar.ResponsabilidadInd = 1) THEN 1 ELSE 0 END) = 1 or " +
                    "  (CASE WHEN (compCar.ResponsabilidadInd = 0 and planPag.VlrPagadoCuota = planPag.VlrCuota) THEN 1 ELSE 0 END) = 1) " +
                    "ORDER BY venDocu.Oferta, credDocu.CompradorFinalID, CAST(credDocu.Libranza as INTEGER)  ";
                #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                string ofertaOld = string.Empty;
                string compradorFinalOld = string.Empty;
                DTO_ccFlujoCesionDocu flujoCesDocu;
                int i = 0;
                while (dr.Read())
                {
                    DTO_ccCreditoDocu detaFlujo = new DTO_ccCreditoDocu();
                    oferta = dr["Oferta"].ToString();
                    string compradorFinal = dr["CompradorFinalID"].ToString();
                    if (compradorFinal != compradorFinalOld || oferta != ofertaOld)
                    {
                        compradorFinalOld = compradorFinal;
                        ofertaOld = oferta;

                        flujoCesDocu = new DTO_ccFlujoCesionDocu();
                        flujoCesDocu.index = i;
                        flujoCesDocu.ValorPago.Value = 0;
                        flujoCesDocu.PagoFlujoInd.Value = false;
                        flujoCesDocu.Oferta.Value = oferta;
                        flujoCesDocu.Portafolio.Value = dr["Portafolio"].ToString();
                        flujoCesDocu.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                        flujoCesDocu.NombreCompradorCartera.Value = dr["NombreComprador"].ToString();
                        flujoCesDocu.Inversionista.Value = compradorFinal;
                        flujoCesDocu.TerceroPago.Value = dr["TerceroID"].ToString();
                        flujoTemp.Add(flujoCesDocu);
                        i++;
                    }

                    detaFlujo.Aprobado.Value = false;
                    detaFlujo.NumeroDoc.Value = Convert.ToInt32(dr["NumDocCredito"]);
                    detaFlujo.CreditoCuotaNum.Value = Convert.ToInt32(dr["CreditoCuotaNum"]);
                    detaFlujo.PrimeraCuota.Value = Convert.ToInt32(dr["CuotaID"]);
                    detaFlujo.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota"]); // Corresponde a la fecha de la cuota
                    detaFlujo.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                    detaFlujo.ClienteID.Value = dr["ClienteID"].ToString();
                    detaFlujo.Nombre.Value = dr["Nombre"].ToString();
                    detaFlujo.DocVenta.Value = Convert.ToInt32(dr["VentadocNum"]);
                    detaFlujo.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                    detaFlujo.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapitalCesion"]);
                    detaFlujo.VlrUtilidad.Value = Convert.ToDecimal(dr["VlrUtilidadCesion"]);
                    detaFlujo.VlrSaldo.Value = Convert.ToDecimal(dr["VlrDerechosCesion"]);
                    flujoTemp.Last().Detalle.Add(detaFlujo);
                }
                result.AddRange(flujoTemp);
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_FlujoCesionDeta_GetForPago");
                throw exception;
            }
        }

        #endregion

    }

}
