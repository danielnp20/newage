using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using MySql.Data.MySqlClient;

namespace NewAge.ADO
{
    public class DAL_Decisor : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_Decisor(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_OperacionesPendientes que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_OperacionesPendientes</returns>
        public List<DTO_OperacionesPendientes> DAL_Decisor_GetPendientes(string usuarioID)
        {
            try
            {
                List<DTO_OperacionesPendientes> result = new List<DTO_OperacionesPendientes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region query

                mySqlCommandSel.CommandText =
                                        "   DECLARE @EmpresaNumCtrl AS VARCHAR(4)"+
                                        "   DECLARE @CodigoDecisor AS VARCHAR(10)"+
                                        "   DECLARE @ActNegociosGestionar AS VARCHAR(6)"+
                                        "   DECLARE @Actrevision AS VARCHAR(10)"+
                                        "   DECLARE @ActRVC AS VARCHAR(10)"+
                                        "   DECLARE @ActEvaluacion AS VARCHAR(10)"+
                                        "   DECLARE @ActEvaluacionF2 AS VARCHAR(10)" +
                                        "   DECLARE @ActEvaluacionF3 AS VARCHAR(10)" +
                                        "   DECLARE @ActrevisionDesc AS VARCHAR(50)"+
                                        "   DECLARE @ActRVCDesc AS VARCHAR(50)"+
                                        "   DECLARE @ActEvaluacionDesc AS VARCHAR(50)"+
                                        "   SELECT @EmpresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID =  @EmpresaID"+
                                        "   SET @CodigoDecisor = @EmpresaNumCtrl + '50'"+
                                        "   SELECT @ActNegociosGestionar = Data FROM glControl WHERE glControlId = CAST(@CodigoDecisor + '126' AS INT)"+
                                        "   SELECT @Actrevision = Data,@ActrevisionDesc = Descriptivo FROM glControl WHERE glControlId = CAST(@CodigoDecisor + '132' AS INT)"+
                                        "   SELECT @ActRVC = Data,@ActRVCDesc = Descriptivo FROM glControl WHERE glControlId = CAST(@CodigoDecisor + '133' AS INT)"+
                                        "   SELECT @ActEvaluacion= Data,@ActEvaluacionDesc = Descriptivo FROM glControl WHERE glControlId = CAST(@CodigoDecisor + '134' AS INT)"+
                                        "   SELECT @ActEvaluacionF2= Data FROM glControl WHERE glControlId = CAST(@CodigoDecisor + '137' AS INT)" +
                                        "   SELECT @ActEvaluacionF3= Data FROM glControl WHERE glControlId = CAST(@CodigoDecisor + '138' AS INT)" +
                                        "   select  ctrl.NumeroDoc,ctrl.Fecha, docu.Libranza," +
                                        "   ctrl.FechaDoc as FechaRadica," +
                                        "   act.FechaInicio,"+
                                        "   docu.ClienteID,RTRIM(docu.ClienteRadica) as ClienteRadica,RTRIM(Isnull(docu.ApellidoPri, '')) + ' ' + RTRIM(Isnull(docu.ApellidoSdo, '')) + ' ' + RTRIM(Isnull(docu.NombrePri, '')) + ' ' + RTRIM(Isnull(docu.NombreSdo, '')) as Nombre,docu.ApellidoPri,docu.ApellidoSdo,docu.NombrePri,docu.NombreSdo, " +
                                        "   docu.Plazo, docu.VlrSolicitado, " +
                                        "	con.Descriptivo as Vitrina," +
                                        "   Zona.Descriptivo  as Zona,"+
                                        "	linCred.Descriptivo as LineaCredito," +
                                        "	tipcre.Descriptivo as TipoOperacion," +                                        
                                        "	flujo.Descriptivo as Etapa,Flujo.DocumentoID, "+
                                        "	docu.ActividadFlujoNegociosGestionarID,"+
                                        "	case"+
  	                                    "       WHEN rtrim(docu.ActividadFlujoNegociosGestionarID)=rtrim(@Actrevision) then @ActrevisionDesc"+ 
                                        "	    WHEN rtrim(docu.ActividadFlujoNegociosGestionarID)=rtrim(@ActRVC) then @ActRVCDesc"+
                                        "	    WHEN rtrim(docu.ActividadFlujoNegociosGestionarID)=rtrim(@ActEvaluacion) then @ActEvaluacionDesc"+
                                        "	    WHEN rtrim(docu.ActividadFlujoNegociosGestionarID)=rtrim(@ActEvaluacionF2) then @ActEvaluacionDesc "+
                                        "	    WHEN rtrim(docu.ActividadFlujoNegociosGestionarID)=rtrim(@ActEvaluacionF3) then @ActEvaluacionDesc "+

                                        "	    else '' "+
                                        "	END ESTADO "+
                                        "   from ccSolicitudDocu docu with(nolock)" +
                                       "	    inner join glDocumentoControl ctrl on ctrl.NumeroDoc=docu.NumeroDoc" +
                                        "	    left join ccCliente Clie on clie.ClienteID=docu.ClienteRadica " +
                                        "	    inner join ccConcesionario con on con.ConcesionarioID=docu.ConcesionarioID AND docu.eg_ccConcesionario = con.EmpresaGrupoID" +
                                        "	    inner join glZona Zona on zona.ZonaID=docu.ZonaID  AND docu.eg_glZona = zona.EmpresaGrupoID" +
                                        "	    inner join ccTipoCredito tipcre on docu.TipoCreditoID = tipcre.TipoCreditoID" +
                                        "	    inner join ccLineaCredito linCred on docu.LineaCreditoID = linCred.LineaCreditoID AND DOCU.eg_ccLineaCredito = linCred.EmpresaGrupoID " +
                                        "	    inner join glActividadEstado Act on docu.NumeroDoc= act.NumeroDoc" +
                                        "	    inner join glActividadPermiso perm with(nolock) ON perm.EmpresaGrupoID = ctrl.EmpresaID " +
                                        "		    and perm.AreaFuncionalID = ctrl.AreaFuncionalID AND perm.UsuarioID = @UsuarioID  and perm.ActividadFlujoID = Act.ActividadFlujoID " +
                                        "	    inner join glActividadFlujo Flujo on act.ActividadFlujoID=flujo.ActividadFlujoID and act.eg_glActividadFlujo=Flujo.EmpresaGrupoID" +
                                        "	where ctrl.EmpresaID=@empresaID and ctrl.Estado in (1,2) and ctrl.DocumentoID = 160 and Act.CerradoInd=0 and tipcre.TipoCredito in(1,2) " +
                                        "   order by ctrl.Fecha desc";

                #endregion
                #region Creacion de comandos    
                    mySqlCommandSel.Parameters.Add("@empresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@empresaID"].Value =this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = usuarioID;
                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_OperacionesPendientes datos = new DTO_OperacionesPendientes(dr);
                    if (string.IsNullOrWhiteSpace(datos.ClienteID.Value))
                        datos.ClienteID.Value = dr["ClienteRadica"].ToString();
                    //if (string.IsNullOrWhiteSpace(datos.Nombre.Value))
                    //    datos.Nombre.Value = dr["ApellidoPri"].ToString().TrimEnd() + " " + dr["ApellidoSdo"].ToString().TrimEnd() + " " + dr["NombrePri"].ToString().TrimEnd() + " " + dr["NombreSdo"].ToString().TrimEnd();
                    result.Add(datos);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Decisor_GetPendientes");
                throw exception;
            }
        }


        /// <summary>
        /// Trae todos los registros de DTO_OperacionesPendientes que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_OperacionesPendientes</returns>
        public List<DTO_QueryObligaciones> DAL_Decisor_Obligaciones(int NumeroDoc)
        {
            try
            {
                List<DTO_QueryObligaciones> result = new List<DTO_QueryObligaciones>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region query

                    mySqlCommandSel.CommandText =
                            "   select NumeroDoc,TipoPersona,NumeroDocCRD,Version," +
                            "   Obligacion as Oblig," +
                            "   Pagare," +
                            "   LineaCreditoID," +
                            "   CuotasMora," +
                            "   Altura," +
                            "   VlrCuota," +
                            "   SdoCapital," +
                            "   SdoVencido," +
                            "   SdoCredito," +
                            "   OblVsGarantia as Cubrimiento," +
                            "   Consecutivo," +
                            "   CancelaInd," +
                            "   NivelRiesgo"+
                            "   from drSolicitudObligaciones" +
                            "   where NumeroDoc=@NumeroDoc";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;
                #endregion

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_QueryObligaciones datos = new DTO_QueryObligaciones(dr);
                    result.Add(datos);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Decisor_Obligaciones");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo CancelaInd de la tabla drSolicitudObligaciones
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudObligaciones_Update(DTO_QueryObligaciones Garantia)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                              "UPDATE drSolicitudObligaciones SET " +
                                                                "CancelaInd=@CancelaInd" +
                                                                " WHERE  Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@CancelaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@CancelaInd"].Value = Garantia.CancelaInd.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Garantia.Consecutivo.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo CancelaInd de la tabla drSolicitudGarantias
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_drSolicitudGarantias_Update(DTO_QueryGarantiaControl Garantia)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                              "UPDATE drSolicitudGarantias SET " +
                                                                "CancelaInd=@CancelaInd" +
                                                                " WHERE  Consecutivo = @Consecutivo";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@CancelaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@CancelaInd"].Value = Garantia.CancelaInd.Value;
                mySqlCommandSel.Parameters["@Consecutivo"].Value = Garantia.Consecutivo.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_drSolicitudDatosOtros_Update");
                throw exception;
            }
        }


        #endregion
        /// <summary>
        /// Trae todos los registros de DTO_OperacionesPendientes que no esten cancelados
        /// </summary>
        /// <returns>retorna una lista de DTO_OperacionesPendientes</returns>
        public List<DTO_DigitaSolicitudDecisor> DAL_Decisor_GetMYSQL(int lastConsecWeb)
        {
            try
            {
                MySqlCommand Query = new MySqlCommand();
                MySqlConnection Conexion = new MySqlConnection();

                Conexion.ConnectionString = "Server=apoyautos.com;Uid=apoyauto_apoyos;Database=apoyauto_apoyos;Pwd=C0n7r453n@db";   
                Conexion.Open();
                Query.Connection = Conexion;
                Query.CommandText = " SELECT r.id redId,r.`name` redNombre,r.id_ciudad, sol.* " +
                                    " FROM solicitudes sol " +
                                    " inner join users usu on sol.users_id = usu.id " +
                                    " inner join companias comp on usu.companias_id = comp.id " +
                                    " inner join redes r on comp.red = r.`name` " +
                                    " where sol.id > @id  order by sol.id "; 


                Query.Parameters.AddWithValue("@id", lastConsecWeb);
               
                MySqlDataReader consultar = Query.ExecuteReader();
                List<DTO_DigitaSolicitudDecisor> result = new List<DTO_DigitaSolicitudDecisor>();
                while (consultar.Read())
                {
                    DTO_DigitaSolicitudDecisor sol = new DTO_DigitaSolicitudDecisor();
                    sol.SolicituDocu.ConsecutivoWEB.Value = consultar.GetInt32("id");
                    sol.SolicituDocu.ClienteRadica.Value = consultar.GetString("documento_solicitante").ToString();
                    sol.SolicituDocu.TipoGarantia.Value = (byte)TipoGarantia.Real;
                    sol.SolicituDocu.IncorporacionTipo.Value = 1;
                    sol.SolicituDocu.IncorporacionPreviaInd.Value = false;
                    sol.SolicituDocu.ApellidoPri.Value = consultar.GetString("apellido_solicitante");
                    sol.SolicituDocu.ApellidoSdo.Value = consultar.GetString("segundo_apellido_solicitante");
                    sol.SolicituDocu.NombrePri.Value = consultar.GetString("nombre_solicitante");
                    sol.SolicituDocu.NombreSdo.Value = consultar.GetString("segundo_nombre_solicitante");
                    sol.SolicituDocu.Libranza.Value = 0;
                    sol.SolicituDocu.CooperativaID.Value = string.Empty;
                    sol.SolicituDocu.AsesorID.Value = consultar.GetInt32("users_id").ToString();
                    sol.SolicituDocu.ConcesionarioID.Value = consultar.GetString("redId");
                    //sol.SolicituDocu.Ciudad.Value = consultar.GetInt32("id_ciudad").ToString();  //Ciudad sale del concesionarioID
                    sol.SolicituDocu.TipoCredito.Value = 1;
                    sol.SolicituDocu.CompraCarteraInd.Value = false;
                    sol.SolicituDocu.PorInteres.Value = 0;
                    sol.SolicituDocu.PorSeguro.Value = 0;
                    sol.SolicituDocu.VlrPrestamo.Value = 0;
                    sol.SolicituDocu.VlrPreSolicitado.Value = consultar.GetDecimal("precio_venta");
                    sol.SolicituDocu.VlrSolicitado.Value = consultar.GetDecimal("precio_venta");
                    sol.SolicituDocu.VlrAdicional.Value = 0;
                    sol.SolicituDocu.VlrLibranza.Value = 0;
                    sol.SolicituDocu.VlrCompra.Value = 0;
                    sol.SolicituDocu.VlrDescuento.Value = 0;
                    sol.SolicituDocu.VlrGiro.Value = 0;
                    sol.SolicituDocu.Plazo.Value = 36;                                          
                    sol.SolicituDocu.VlrCuota.Value = 0;
                    sol.SolicituDocu.VlrCupoDisponible.Value = 0;
                    sol.SolicituDocu.VlrCapacidad.Value = 0;
                    sol.SolicituDocu.PagoVentanillaInd.Value = false;
                    sol.SolicituDocu.RechazoInd.Value = false;

                    sol.SolicituDocu.PrecioVenta.Value = consultar.GetDecimal("precio_venta");
                    sol.SolicituDocu.CuotaInicial.Value = consultar.GetDecimal("cuota_inicial");
                    sol.SolicituDocu.Servicio.Value = consultar.GetString("servicio").Equals("particular") ? (byte)1 : (byte)2;
                    sol.SolicituDocu.Modelo.Value = consultar.GetInt32("modelo");
                    sol.SolicituDocu.CeroKmInd.Value = consultar.GetString("vel_0_km").Equals("si") ? true : false;
                    sol.SolicituDocu.Marca.Value = consultar.GetString("marca");
                    sol.SolicituDocu.Tipocaja.Value = consultar.GetString("tipo_caja").TrimEnd().Equals("mecanica") ? (byte)1 : (consultar.GetString("tipo_caja").TrimEnd().Equals("automatica") ? (byte)2 : (byte)3);
                    sol.SolicituDocu.Linea.Value = consultar.GetString("linea");
                    sol.SolicituDocu.PuertasNro.Value = consultar.GetString("puertas") != null ? consultar.GetByte("puertas") : (byte)0;
                    sol.SolicituDocu.Referencia.Value = consultar.GetString("referencia");
                    sol.SolicituDocu.AireAcondicionado.Value = consultar.GetString("aire_acondicionado").Equals("si") ? true : false;
                    sol.SolicituDocu.Cilindraje.Value = consultar.GetInt32("cilindraje");
                    sol.SolicituDocu.Complemento.Value = consultar.GetString("tipo_complemento");
                    sol.SolicituDocu.Termoking.Value = consultar.GetString("termoking").Equals("si") ? true : false;
                    sol.SolicituDocu.PrecioVentaChasis.Value = consultar.GetDecimal("precio_venta_chasis");
                    sol.SolicituDocu.PrecioVentaComplemento.Value = consultar.GetDecimal("precio_venta_complemento");

                    //sol.DatosVehiculo.PrecioVenta.Value = consultar.GetDecimal("precio_venta");
                    //sol.DatosVehiculo.CuotaInicial.Value = consultar.GetDecimal("cuota_inicial");
                    //sol.DatosVehiculo.Servicio.Value = consultar.GetString("servicio").Equals("particular") ? (byte)1 : (byte)2;
                    //sol.DatosVehiculo.Modelo.Value = consultar.GetInt32("modelo");
                    //sol.DatosVehiculo.CeroKmInd.Value = consultar.GetString("vel_0_km").Equals("si") ? true : false;
                    //sol.DatosVehiculo.Marca.Value = consultar.GetString("marca");
                    //sol.DatosVehiculo.Tipocaja.Value = consultar.GetString("tipo_caja").TrimEnd().Equals("mecanica") ? (byte)1 : (consultar.GetString("tipo_caja").TrimEnd().Equals("automatica") ? (byte)2 : (byte)3);
                    //sol.DatosVehiculo.Linea.Value = consultar.GetString("linea");
                    //sol.DatosVehiculo.PuertasNro.Value = consultar.GetString("puertas") != null ? consultar.GetByte("puertas") : (byte)0;
                    //sol.DatosVehiculo.Referencia.Value = consultar.GetString("referencia");
                    //sol.DatosVehiculo.AireAcondicionado.Value = consultar.GetString("aire_acondicionado").Equals("si") ? true : false;
                    //sol.DatosVehiculo.Cilindraje.Value = consultar.GetInt32("cilindraje");
                    //sol.DatosVehiculo.Complemento.Value = consultar.GetString("tipo_complemento");
                    //sol.DatosVehiculo.Termoking.Value = consultar.GetString("termoking").Equals("si") ? true : false;
                    //sol.DatosVehiculo.PrecioVentaChasis.Value = consultar.GetDecimal("precio_venta_chasis");
                    //sol.DatosVehiculo.PrecioVentaComplemento.Value = consultar.GetDecimal("precio_venta_complemento");
                    //sol.DatosVehiculo.Registrada.Value = 0;
                    //sol.DatosVehiculo.ChasisYComplementoIND.Value = false;
                    result.Add(sol);
                }
                Conexion.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Decisor_GetMYSQL");
                throw exception;
            }
        }
    }
}
