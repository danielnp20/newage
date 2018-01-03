using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_prContratoDocu
    /// </summary>
    public class DAL_prContratoDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prContratoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prContratoDocu DAL_prContratoDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prContratoDocu with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_prContratoDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prContratoDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prOrdenCompraDocu 
        /// </summary>
        /// <param name="sol">Orden de Compra</param>
        /// <returns></returns>
        public void DAL_prContratoDocu_Add(DTO_prContratoDocu contrato)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prContratoDocu " +
                                           "    (EmpresaID " +
                                           "    ,NumeroDoc " +
                                           "    ,ProveedorID " +
                                           "    ,TipoOrden " +
                                           "    ,TipoDocumento " +
                                           "    ,TipoOtroSi " +
                                           "    ,DocContratoPRY "   +   //
                                           "    ,OrdenCompraNro " +
                                           "    ,ContratoMacroNro " +
                                           "    ,MonedaOrden " +
                                           "    ,MonedaPago " +
                                           "    ,LugarEntrega " +
                                           "    ,PagoVariableInd " +
                                           "    ,TerminosInd " +
                                           "    ,VlrAnticipo " +
                                           "    ,DtoProntoPago " +
                                           "    ,DiasPtoPago " +
                                           "    ,FormaPago " +
                                           "    ,Instrucciones " +
                                           "    ,Observaciones " +
                                           "    ,ObservRechazo " +
                                           "    ,FechaERechazo " +
                                           "    ,TasaOrden " +
                                           "    ,UsuarioRechaza " +
                                           "    ,PorcentAdministra " +
                                           "    ,Porcentimprevisto " +
                                           "    ,PorcentUtilidad " +
                                           "    ,IncluyeAUICosto " +
                                           "    ,PorcentHolgura " +
                                           "    ,AreaAprobacion " +
                                           "    ,UsuarioSolicita " +
                                           "    ,Prioridad " +
                                           "    ,Valor " +
                                           "    ,IVA " +
                                           "    ,VlrPagoMes " +
                                           "    ,NroPagos " +
                                           "    ,FechaPago1 " +
                                           "    ,FechaVencimiento " +
                                           "    ,RteGarantiaPor " +
                                           "    ,eg_glAreaFuncional) " +
                                            "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@ProveedorID " +
                                           "    ,@TipoOrden " +
                                           "    ,@TipoDocumento " +
                                           "    ,@TipoOtroSi " +
                                           "    ,@OrdenCompraNro " +
                                           "    ,@ContratoMacroNro " +
                                           "    ,@MonedaOrden " +
                                           "    ,@MonedaPago " +
                                           "    ,@LugarEntrega " +
                                           "    ,@PagoVariableInd " +
                                           "    ,@TerminosInd " +
                                           "    ,@VlrAnticipo " +
                                           "    ,@DtoProntoPago " +
                                           "    ,@DiasPtoPago " +
                                           "    ,@FormaPago " +
                                           "    ,@Instrucciones " +
                                           "    ,@Observaciones " +
                                           "    ,@ObservRechazo " +
                                           "    ,@FechaERechazo " +
                                           "    ,@TasaOrden " +
                                           "    ,@UsuarioRechaza " +
                                           "    ,@PorcentAdministra " +
                                           "    ,@Porcentimprevisto " +
                                           "    ,@PorcentUtilidad " +
                                           "    ,@IncluyeAUICosto " +
                                           "    ,@PorcentHolgura " +
                                           "    ,@AreaAprobacion " +
                                           "    ,@UsuarioSolicita " +
                                           "    ,@Prioridad " +
                                           "    ,@Valor " +
                                           "    ,@IVA " +
                                           "    ,@VlrPagoMes " +
                                           "    ,@NroPagos " +
                                           "    ,@FechaPago1 " +
                                           "    ,@FechaVencimiento " +
                                           "    ,@RteGarantiaPor " +
                                           "    ,@eg_glAreaFuncional)";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoOrden", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoDocumento", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoOtroSi", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocContratoPRY",SqlDbType.Int);//
                mySqlCommand.Parameters.Add("@OrdenCompraNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ContratoMacroNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaOrden", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@PagoVariableInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@TerminosInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@VlrAnticipo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DtoProntoPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasPtoPago", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FormaPago", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Instrucciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ObservRechazo", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@FechaERechazo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TasaOrden", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@UsuarioRechaza", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@PorcentAdministra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Porcentimprevisto", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorcentUtilidad", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IncluyeAUICosto", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@PorcentHolgura", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioSolicita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Prioridad", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrPagoMes", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@NroPagos", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaPago1", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@RteGarantiaPor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = contrato.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = contrato.ProveedorID.Value;
                mySqlCommand.Parameters["@TipoOrden"].Value = contrato.TipoOrden.Value;
                mySqlCommand.Parameters["@TipoDocumento"].Value = contrato.TipoDocumento.Value;
                mySqlCommand.Parameters["@TipoOtroSi"].Value = contrato.TipoOtroSi.Value;
                mySqlCommand.Parameters["@DocContratoPRY"].Value = contrato.DocContratoPRY.Value;//
                mySqlCommand.Parameters["@OrdenCompraNro"].Value = contrato.OrdenCompraNro.Value;
                mySqlCommand.Parameters["@ContratoMacroNro"].Value = contrato.ContratoMacroNro.Value;
                mySqlCommand.Parameters["@MonedaOrden"].Value = contrato.MonedaOrden.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = contrato.MonedaPago.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = contrato.LugarEntrega.Value;
                mySqlCommand.Parameters["@PagoVariableInd"].Value = contrato.PagoVariableInd.Value;
                mySqlCommand.Parameters["@TerminosInd"].Value = contrato.TerminosInd.Value;
                mySqlCommand.Parameters["@VlrAnticipo"].Value = contrato.VlrAnticipo.Value;
                mySqlCommand.Parameters["@DtoProntoPago"].Value = contrato.DtoProntoPago.Value;
                mySqlCommand.Parameters["@DiasPtoPago"].Value = contrato.DiasPtoPago.Value;
                mySqlCommand.Parameters["@FormaPago"].Value = contrato.FormaPago.Value;
                mySqlCommand.Parameters["@Instrucciones"].Value = contrato.Instrucciones.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = contrato.Observaciones.Value;
                mySqlCommand.Parameters["@ObservRechazo"].Value = contrato.ObservRechazo.Value;
                mySqlCommand.Parameters["@FechaERechazo"].Value = contrato.FechaERechazo.Value ?? SqlDateTime.Null;
                mySqlCommand.Parameters["@TasaOrden"].Value = contrato.TasaOrden.Value;
                mySqlCommand.Parameters["@UsuarioRechaza"].Value = contrato.UsuarioRechaza.Value;
                mySqlCommand.Parameters["@PorcentAdministra"].Value = contrato.PorcentAdministra.Value;
                mySqlCommand.Parameters["@Porcentimprevisto"].Value = contrato.Porcentimprevisto.Value;
                mySqlCommand.Parameters["@PorcentUtilidad"].Value = contrato.PorcentUtilidad.Value;
                mySqlCommand.Parameters["@IncluyeAUICosto"].Value = contrato.IncluyeAUICosto.Value;
                mySqlCommand.Parameters["@PorcentHolgura"].Value = contrato.PorcentHolgura.Value;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = contrato.AreaAprobacion.Value;
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = contrato.UsuarioSolicita.Value;
                mySqlCommand.Parameters["@Prioridad"].Value = contrato.Prioridad.Value;
                mySqlCommand.Parameters["@Valor"].Value = contrato.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = contrato.IVA.Value;
                mySqlCommand.Parameters["@VlrPagoMes"].Value = contrato.VlrPagoMes.Value;
                mySqlCommand.Parameters["@NroPagos"].Value = contrato.NroPagos.Value;
                mySqlCommand.Parameters["@FechaPago1"].Value = contrato.FechaPago1.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = contrato.FechaVencimiento.Value;
                mySqlCommand.Parameters["@RteGarantiaPor"].Value = contrato.RteGarantiaPor.Value;
                mySqlCommand.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
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
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar el registro en tabla prOrdenCompraDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_prContratoDocu_Upd(DTO_prContratoDocu contrato)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                //Actualiza Tabla prOrdenCompraDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prContratoDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,ProveedorID = @ProveedorID " +
                                           "    ,TipoOrden = @TipoOrden " +
                                           "    ,TipoDocumento = @TipoDocumento " +
                                           "    ,TipoOtroSi = @TipoOtroSi " +
                                           "    ,DocContratoPRY = @DocContratoPRY " +
                                           "    ,OrdenCompraNro = @OrdenCompraNro " +
                                           "    ,ContratoMacroNro = @ContratoMacroNro " +
                                           "    ,MonedaOrden = @MonedaOrden " +
                                           "    ,MonedaPago = @MonedaPago " +
                                           "    ,LugarEntrega = @LugarEntrega " +
                                           "    ,PagoVariableInd = @PagoVariableInd " +
                                           "    ,TerminosInd = @TerminosInd " +
                                           "    ,VlrAnticipo = @VlrAnticipo" +
                                           "    ,DtoProntoPago = @DtoProntoPago" +
                                           "    ,DiasPtoPago = @DiasPtoPago" +
                                           "    ,FormaPago = @FormaPago " +
                                           "    ,Instrucciones = @Instrucciones " +
                                           "    ,Observaciones = @Observaciones " +
                                           "    ,ObservRechazo = @ObservRechazo " +
                                           "    ,FechaERechazo = @FechaERechazo " +
                                           "    ,TasaOrden = @TasaOrden " +
                                           "    ,UsuarioRechaza = @UsuarioRechaza " +
                                           "    ,PorcentAdministra = @PorcentAdministra" +
                                           "    ,Porcentimprevisto = @Porcentimprevisto" +
                                           "    ,PorcentUtilidad = @PorcentUtilidad" +
                                           "    ,IncluyeAUICosto = @IncluyeAUICosto" +
                                           "    ,PorcentHolgura = @PorcentHolgura" +
                                           "    ,AreaAprobacion = @AreaAprobacion " +
                                           "    ,UsuarioSolicita = @UsuarioSolicita " +
                                           "    ,Prioridad = @Prioridad " +
                                           "    ,Valor = @Valor" +
                                           "    ,IVA = @IVA" +
                                           "    ,VlrPagoMes = @VlrPagoMes" +
                                           "    ,NroPagos = @NroPagos" +
                                           "    ,FechaPago1 = @FechaPago1" +
                                           "    ,FechaVencimiento = @FechaVencimiento" +
                                           "    ,RteGarantiaPor = @RteGarantiaPor" +
                                           "    ,eg_glAreaFuncional = @eg_glAreaFuncional" +   
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoOrden", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoDocumento", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@TipoOtroSi", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocContratoPRY", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@OrdenCompraNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ContratoMacroNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaOrden", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@PagoVariableInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@TerminosInd", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@VlrAnticipo", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DtoProntoPago", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@DiasPtoPago", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FormaPago", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Instrucciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@ObservRechazo", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@FechaERechazo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@TasaOrden", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@UsuarioRechaza", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@PorcentAdministra", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@Porcentimprevisto", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@PorcentUtilidad", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IncluyeAUICosto", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@PorcentHolgura", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioSolicita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Prioridad", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@VlrPagoMes", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@NroPagos", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@FechaPago1", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@RteGarantiaPor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = contrato.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = contrato.ProveedorID.Value;
                mySqlCommand.Parameters["@TipoOrden"].Value = contrato.TipoOrden.Value;
                mySqlCommand.Parameters["@TipoDocumento"].Value = contrato.TipoDocumento.Value;
                mySqlCommand.Parameters["@TipoOtroSi"].Value = contrato.TipoOtroSi.Value;
                mySqlCommand.Parameters["@DocContratoPRY"].Value = contrato.DocContratoPRY.Value;
                mySqlCommand.Parameters["@OrdenCompraNro"].Value = contrato.OrdenCompraNro.Value;
                mySqlCommand.Parameters["@ContratoMacroNro"].Value = contrato.ContratoMacroNro.Value;
                mySqlCommand.Parameters["@MonedaOrden"].Value = contrato.MonedaOrden.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = contrato.MonedaPago.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = contrato.LugarEntrega.Value;
                mySqlCommand.Parameters["@PagoVariableInd"].Value = contrato.PagoVariableInd.Value;
                mySqlCommand.Parameters["@TerminosInd"].Value = contrato.TerminosInd.Value;
                mySqlCommand.Parameters["@VlrAnticipo"].Value = contrato.VlrAnticipo.Value;
                mySqlCommand.Parameters["@DtoProntoPago"].Value = contrato.DtoProntoPago.Value;
                mySqlCommand.Parameters["@DiasPtoPago"].Value = contrato.DiasPtoPago.Value;
                mySqlCommand.Parameters["@FormaPago"].Value = contrato.FormaPago.Value;
                mySqlCommand.Parameters["@Instrucciones"].Value = contrato.Instrucciones.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = contrato.Observaciones.Value;
                mySqlCommand.Parameters["@ObservRechazo"].Value = contrato.ObservRechazo.Value;
                mySqlCommand.Parameters["@FechaERechazo"].Value = contrato.FechaERechazo.Value ?? SqlDateTime.Null;
                mySqlCommand.Parameters["@TasaOrden"].Value = contrato.TasaOrden.Value;
                mySqlCommand.Parameters["@UsuarioRechaza"].Value = contrato.UsuarioRechaza.Value;
                mySqlCommand.Parameters["@PorcentAdministra"].Value = contrato.PorcentAdministra.Value;
                mySqlCommand.Parameters["@Porcentimprevisto"].Value = contrato.Porcentimprevisto.Value;
                mySqlCommand.Parameters["@PorcentUtilidad"].Value = contrato.PorcentUtilidad.Value;
                mySqlCommand.Parameters["@IncluyeAUICosto"].Value = contrato.IncluyeAUICosto.Value;
                mySqlCommand.Parameters["@PorcentHolgura"].Value = contrato.PorcentHolgura.Value;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = contrato.AreaAprobacion.Value;
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = contrato.UsuarioSolicita.Value;
                mySqlCommand.Parameters["@Prioridad"].Value = contrato.Prioridad.Value;
                mySqlCommand.Parameters["@Valor"].Value = contrato.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = contrato.IVA.Value;
                mySqlCommand.Parameters["@VlrPagoMes"].Value = contrato.VlrPagoMes.Value;
                mySqlCommand.Parameters["@NroPagos"].Value = contrato.NroPagos.Value;
                mySqlCommand.Parameters["@FechaPago1"].Value = contrato.FechaPago1.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = contrato.FechaVencimiento.Value;
                mySqlCommand.Parameters["@RteGarantiaPor"].Value = contrato.RteGarantiaPor.Value;
                mySqlCommand.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
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
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de Compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prContratoAprob> DAL_prContratoDocu_GetPendientesByModulo(DTO_glDocumento doc, string actividadFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_prContratoAprob> result = new List<DTO_prContratoAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                //mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = doc.ModuloID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = usuario.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = usuario.ReplicaID.Value;
                #endregion

                  mySqlCommand.CommandText =
                    "   select temp.*, " +
                    "    det.ConsecutivoDetaID,det.ContratoDocuID,det.ContratoDetaID,det.CodigoBSID,det.Descriptivo,det.inReferenciaID,det.UnidadInvID,det.CantidadCont " +
                    "    ,solCtrl.PrefijoID PrefijoSolID,det.SolicitudDocuID,det.SolicitudDetaID,det.ValorUni,det.ValorTotML,det.IvaTotML " +
                    "    from ( select distinct ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc Fecha,ctrl.Observacion Justificacion, " +
                    "        pr.ProveedorID,pr.Descriptivo ProveedorNombre,cont.MonedaOrden,cont.MonedaPago,docAprueba.UsuarioAprueba,usr.UsuarioID, " +
	                "        SUM(det.ValorTotML) ValorTotalML " +
                    "    from glDocumentoControl ctrl with(nolock) " + 
                    "        inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc  " +
                    "               and act.CerradoInd=0  and act.ActividadFlujoID= @ActividadFlujoID  " +
	                "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "        inner join prContratoDocu cont with(nolock) on ctrl.NumeroDoc = cont.NumeroDoc  " +
	                "        inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "        inner join prProveedor pr with(nolock) on cont.ProveedorID = pr.ProveedorID " +
                    "        inner join prDetalleDocu det with(nolock) on cont.NumeroDoc = det.NumeroDoc " +
                    "        inner join glDocumentoAprueba docAprueba with(nolock) on docAprueba.NumeroDoc = ctrl.NumeroDoc " +
                    //"        inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    //"               and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " +
                    //"    and perm.ActividadFlujoID = @ActividadFlujoID and cont.AreaAprobacion = @AreaAprobacion " +
                    "    and docAprueba.UsuarioAprueba = @UsuarioAprueba" +
                    "    group by ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc,ctrl.Observacion, " +
                    "        pr.ProveedorID,pr.Descriptivo,cont.MonedaOrden,cont.MonedaPago,docAprueba.UsuarioAprueba,usr.UsuarioID ) temp  " +
                    "        inner join prDetalleDocu det with(nolock) on temp.EmpresaID = det.EmpresaID and temp.NumeroDoc = det.NumeroDoc " +
                    "        inner join glDocumentoControl solCtrl on det.EmpresaID = solCtrl.EmpresaID and det.SolicitudDocuID = solCtrl.NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                        bool nuevo = true;
                        DTO_prContratoAprob dto = new DTO_prContratoAprob(dr);
                        List<DTO_prContratoAprob> list = result.Where(x => ((DTO_prContratoAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                        if (list.Count > 0)
                        {
                            dto = list.First();
                            nuevo = false;
                        }
                        else
                        {
                            dto = new DTO_prContratoAprob(dr);
                            dto.Aprobado.Value = false;
                            dto.Rechazado.Value = false;
                        }

                        DTO_prContratoAprobDet dtoDet = new DTO_prContratoAprobDet(dr);
                        dto.ContratoAprobDet.Add(dtoDet);

                        if (nuevo)
                            result.Add(dto);
                    }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoDocu_GetPendientesByModulo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de COmpra para Recibido
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraResumen> DAL_prContratoDocu_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, List<Tuple<string, string>> filtros)
        {
            try
            {
                List<DTO_prOrdenCompraResumen> result = new List<DTO_prOrdenCompraResumen>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filter = string.Empty;

                foreach (Tuple<string, string> filtro in filtros)
                {
                    filter += " and " + filtro.Item1 + " = '" + filtro.Item2.Trim() + "' ";
                }
                
                mySqlCommand.CommandText =
                "select ctrlOC.NumeroDoc,ctrlOC.DocumentoID,detOC.ContratoDocuID,ctrlOC.PrefijoID PrefijoIDOC,ctrlOC.DocumentoNro DocumentoNroOC,solCargo.ProyectoID, " +
                "    detOC.SolicitudDocuID,ctrlSol.PrefijoID PrefijoIDSol,ctrlSol.DocumentoNro DocumentoNroSol,detOC.LineaPresupuestoID, " +
                "    detOC.SolicitudDetaID,detOC.ContratoDetaID,detOC.CodigoBSID,detOC.inReferenciaID,detOC.Descriptivo,detOC.ValorTotML as ValorTotMLOC,detOC.ValorTotME as ValorTotMEOC,detOC.ValorUni, " +
                "    detSol.CantidadSol,detSol.UnidadInvID,refer.EmpaqueInvID,detOC.CantidadOC CantidadOrdComp,temp.CantidadSum CantidadOC,ctrlOC.FechaDoc FechaOC,ctrlOC.MonedaID MonedaIDOC " +
                "    detSol.Documento1ID,detSol.Documento2ID,detSol.Documento3ID,detSol.Documento4ID,detSol.Documento5ID, " +
                "    detSol.Detalle1ID,detSol.Detalle2ID,detSol.Detalle3ID,dedetSolt.Detalle4ID,detSol.Detalle5ID,refer.MarcaInvID,refer.RefProveedor,empaque.UnidadInvID as UnidadEmpaque,empaque.Cantidad as CantidadEmpaque, " +
                "    detSol.CantidadDoc1,detSol.CantidadDoc2,detSol.CantidadDoc3,detSol.CantidadDoc4,detSol.CantidadDoc5,cont.ProveedorID,cont.MonedaPago as MonedaPagoOC   " +
                "from ( " +
                "    select det.ContratoDocuID,det.ContratoDetaID, " + 
		        "        SUM(det.CantidadOC) CantidadSum  " +
	            "    from prDetalleDocu det with(nolock)  " +
		        "        inner join glDocumentoControl ctrl with(nolock) on det.NumeroDoc = ctrl.NumeroDoc  " +
		        "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +
                "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID  " +
                "        and ( ctrl.DocumentoID = @DocumentoID1 and ctrl.Estado = @Estado  or ctrl.DocumentoID = @DocumentoID2 or ctrl.DocumentoID = @DocumentoID3)  " +
                "group by det.ContratoDocuID,det.ContratoDetaID ) temp  " +
                "    inner join prDetalleDocu detOC with(nolock) on temp.ContratoDetaID = detOC.ConsecutivoDetaID " +
                "    inner join prDetalleDocu detSol with(nolock) on detOC.SolicitudDetaID = detSol.ConsecutivoDetaID  " +
                "    inner join glDocumentoControl ctrlOC with(nolock) on temp.ContratoDocuID = ctrlOC.NumeroDoc  " +
                "    inner join glDocumentoControl ctrlSol with(nolock) on detOC.SolicitudDocuID = ctrlSol.NumeroDoc  " +
                "    inner join prContratoDocu cont with(nolock) on cont.NumeroDoc = ctrlOC.NumeroDoc   " +
                "    inner join prSolicitudCargos solCargo with(nolock) on detSol.ConsecutivoDetaID = solCargo.ConsecutivoDetaID " +
                "    left join inReferencia refer with(nolock) on refer.inReferenciaID = detSol.inReferenciaID and refer.EmpresaGrupoID = detSol.eg_InReferencia " +
                "    left join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = refer.EmpaqueInvID and empaque.EmpresaGrupoID = refer.eg_inEmpaque " +
                "where temp.CantidadSum!=0 " + filter;
                
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                //mySqlCommand.Parameters.Add("@UsuarioAsignado", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID1", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID2", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID3", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                //mySqlCommand.Parameters["@UsuarioAsignado"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@DocumentoID1"].Value = AppDocuments.OrdenCompra;
                mySqlCommand.Parameters["@DocumentoID2"].Value = AppDocuments.Recibido;
                mySqlCommand.Parameters["@DocumentoID3"].Value = AppDocuments.CierreDetalleOrdenComp;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_prOrdenCompraResumen dtoRes = new DTO_prOrdenCompraResumen(dr);
                    dtoRes.CantidadRec.Value = 0;
                    dtoRes.SerialID.Value = string.Empty;
                    dtoRes.invSerialInd = false;
                    result.Add(dtoRes);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prContratoDocu_GetResumen");
                throw exception;
            }
        }
        #endregion
    }
}
