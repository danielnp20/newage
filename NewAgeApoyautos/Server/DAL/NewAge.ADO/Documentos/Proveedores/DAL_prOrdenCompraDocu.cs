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
    /// DAL_prOrdenCompraDocu
    /// </summary>
    public class DAL_prOrdenCompraDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prOrdenCompraDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prOrdenCompraDocu DAL_prOrdenCompraDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =  " Select docu.*,pr.Descriptivo as ProveedorDesc from prOrdenCompraDocu docu with(nolock) " +
                                            "  left join prProveedor pr with(nolock) on docu.ProveedorID = pr.ProveedorID and docu.eg_prProveedor = pr.EmpresaGrupoID " +
                                            " Where docu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_prOrdenCompraDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prOrdenCompraDocu(dr);
                    result.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prOrdenCompraDocu 
        /// </summary>
        /// <param name="sol">Orden de Compra</param>
        /// <returns></returns>
        public void DAL_prOrdenCompraDocu_Add(DTO_prOrdenCompraDocu orden)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prOrdenCompraDocu " +
                                           "    (EmpresaID " +
                                           "    ,NumeroDoc " +
                                           "    ,ProveedorID " +
                                           "    ,TipoOrden " +
                                           "    ,Inconterm " +
                                           "    ,ContratoNro " +
                                           "    ,MonedaOrden " +
                                           "    ,MonedaPago " +
                                           "    ,LugarEntrega " +
                                           "    ,FechaEntrega " +
                                           "    ,PagoVariablend " +
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
                                           "    ,ContactoComercial " +
                                           "    ,Encargado " +
                                           "    ,DireccionEntrega " +
                                           "    ,TelefonoEntrega " +                                          
                                           "    ,eg_glLocFisica " +
                                           "    ,eg_glAreaFuncional " +
                                           "    ,eg_prProveedor ) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@ProveedorID " +
                                           "    ,@TipoOrden " +
                                           "    ,@Inconterm " +
                                           "    ,@ContratoNro " +
                                           "    ,@MonedaOrden " +
                                           "    ,@MonedaPago " +
                                           "    ,@LugarEntrega " +
                                           "    ,@FechaEntrega " +
                                           "    ,@PagoVariablend " +
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
                                           "    ,@ContactoComercial " +
                                           "    ,@Encargado " +
                                           "    ,@DireccionEntrega " +
                                           "    ,@TelefonoEntrega " + 
                                           "    ,@eg_glLocFisica " +
                                           "    ,@eg_glAreaFuncional "+
                                           "    ,@eg_prProveedor) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoOrden", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Inconterm", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ContratoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaOrden", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@PagoVariablend", SqlDbType.Bit);
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
                mySqlCommand.Parameters.Add("@ContactoComercial", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Encargado", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@TelefonoEntrega", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@DireccionEntrega", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = orden.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = orden.ProveedorID.Value;
                mySqlCommand.Parameters["@TipoOrden"].Value = orden.TipoOrden.Value;
                mySqlCommand.Parameters["@Inconterm"].Value = orden.Inconterm.Value;
                mySqlCommand.Parameters["@ContratoNro"].Value = orden.ContratoNro.Value;
                mySqlCommand.Parameters["@MonedaOrden"].Value = orden.MonedaOrden.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = orden.MonedaPago.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = orden.LugarEntrega.Value;
                mySqlCommand.Parameters["@FechaEntrega"].Value = orden.FechaEntrega.Value;
                mySqlCommand.Parameters["@PagoVariablend"].Value = orden.PagoVariablend.Value;
                mySqlCommand.Parameters["@TerminosInd"].Value = orden.TerminosInd.Value;
                mySqlCommand.Parameters["@VlrAnticipo"].Value = orden.VlrAnticipo.Value;
                mySqlCommand.Parameters["@DtoProntoPago"].Value = orden.DtoProntoPago.Value;
                mySqlCommand.Parameters["@DiasPtoPago"].Value = orden.DiasPtoPago.Value;
                mySqlCommand.Parameters["@FormaPago"].Value = orden.FormaPago.Value;
                mySqlCommand.Parameters["@Instrucciones"].Value = orden.Instrucciones.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = orden.Observaciones.Value;
                mySqlCommand.Parameters["@ObservRechazo"].Value = orden.ObservRechazo.Value;
                mySqlCommand.Parameters["@FechaERechazo"].Value = orden.FechaERechazo.Value ?? SqlDateTime.Null;
                mySqlCommand.Parameters["@TasaOrden"].Value = orden.TasaOrden.Value;
                mySqlCommand.Parameters["@UsuarioRechaza"].Value = orden.UsuarioRechaza.Value;
                mySqlCommand.Parameters["@PorcentAdministra"].Value = orden.PorcentAdministra.Value;
                mySqlCommand.Parameters["@Porcentimprevisto"].Value = orden.Porcentimprevisto.Value;
                mySqlCommand.Parameters["@PorcentUtilidad"].Value = orden.PorcentUtilidad.Value;
                mySqlCommand.Parameters["@IncluyeAUICosto"].Value = orden.IncluyeAUICosto.Value;
                mySqlCommand.Parameters["@PorcentHolgura"].Value = orden.PorcentHolgura.Value;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = orden.AreaAprobacion.Value;
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = orden.UsuarioSolicita.Value;
                mySqlCommand.Parameters["@Prioridad"].Value = orden.Prioridad.Value;
                mySqlCommand.Parameters["@Valor"].Value = orden.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = orden.IVA.Value;
                mySqlCommand.Parameters["@VlrPagoMes"].Value = orden.VlrPagoMes.Value;
                mySqlCommand.Parameters["@NroPagos"].Value = orden.NroPagos.Value;
                mySqlCommand.Parameters["@FechaPago1"].Value = orden.FechaPago1.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = orden.FechaVencimiento.Value;
                mySqlCommand.Parameters["@ContactoComercial"].Value = orden.ContactoComercial.Value;
                mySqlCommand.Parameters["@Encargado"].Value = orden.Encargado.Value;
                mySqlCommand.Parameters["@TelefonoEntrega"].Value = orden.TelefonoEntrega.Value;
                mySqlCommand.Parameters["@DireccionEntrega"].Value = orden.DireccionEntrega.Value;
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar el registro en tabla prOrdenCompraDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_prOrdenCompraDocu_Upd(DTO_prOrdenCompraDocu orden)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla prOrdenCompraDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prOrdenCompraDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,ProveedorID = @ProveedorID " +
                                           "    ,TipoOrden = @TipoOrden " +
                                           "    ,Inconterm = @Inconterm " +
                                           "    ,ContratoNro = @ContratoNro " +
                                           "    ,MonedaOrden = @MonedaOrden " +
                                           "    ,MonedaPago = @MonedaPago " +
                                           "    ,LugarEntrega = @LugarEntrega " +
                                           "    ,FechaEntrega = @FechaEntrega " +
                                           "    ,PagoVariablend = @PagoVariablend " +
                                           "    ,TerminosInd = @TerminosInd " +
                                           "    ,VlrAnticipo = @VlrAnticipo " +
                                           "    ,DtoProntoPago = @DtoProntoPago " +
                                           "    ,DiasPtoPago = @DiasPtoPago " +  
                                           "    ,FormaPago = @FormaPago " +
                                           "    ,Instrucciones = @Instrucciones " +
                                           "    ,Observaciones = @Observaciones " +
                                           "    ,ObservRechazo = @ObservRechazo " +
                                           "    ,FechaERechazo = @FechaERechazo " +
                                           "    ,TasaOrden = @TasaOrden " +
                                           "    ,UsuarioRechaza = @UsuarioRechaza " +
                                           "    ,PorcentAdministra = @PorcentAdministra " +
                                           "    ,Porcentimprevisto = @Porcentimprevisto " +
                                           "    ,PorcentUtilidad = @PorcentUtilidad " +
                                           "    ,IncluyeAUICosto = @IncluyeAUICosto " +
                                           "    ,PorcentHolgura = @PorcentHolgura " +
                                           "    ,AreaAprobacion = @AreaAprobacion " +
                                           "    ,UsuarioSolicita = @UsuarioSolicita " +
                                           "    ,Prioridad = @Prioridad " +
                                           "    ,Valor = @Valor" +
                                           "    ,IVA = @IVA" +
                                           "    ,VlrPagoMes = @VlrPagoMes" +
                                           "    ,NroPagos = @NroPagos" +
                                           "    ,FechaPago1 = @FechaPago1" +
                                           "    ,FechaVencimiento = @FechaVencimiento" +
                                           "    ,ContactoComercial = @ContactoComercial" +
                                           "    ,Encargado = @Encargado" +
                                           "    ,TelefonoEntrega = @TelefonoEntrega" +
                                           "    ,DireccionEntrega = @DireccionEntrega" +   
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoOrden", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@Inconterm", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ContratoNro", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@MonedaOrden", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@MonedaPago", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@PagoVariablend", SqlDbType.Bit);
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
                mySqlCommand.Parameters.Add("@ContactoComercial", SqlDbType.Char, 50);
                mySqlCommand.Parameters.Add("@Encargado", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommand.Parameters.Add("@TelefonoEntrega", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommand.Parameters.Add("@DireccionEntrega", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = orden.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = orden.ProveedorID.Value;
                mySqlCommand.Parameters["@TipoOrden"].Value = orden.TipoOrden.Value;
                mySqlCommand.Parameters["@Inconterm"].Value = orden.Inconterm.Value;
                mySqlCommand.Parameters["@ContratoNro"].Value = orden.ContratoNro.Value;
                mySqlCommand.Parameters["@MonedaOrden"].Value = orden.MonedaOrden.Value;
                mySqlCommand.Parameters["@MonedaPago"].Value = orden.MonedaPago.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = orden.LugarEntrega.Value;
                mySqlCommand.Parameters["@FechaEntrega"].Value = orden.FechaEntrega.Value;
                mySqlCommand.Parameters["@PagoVariablend"].Value = orden.PagoVariablend.Value;
                mySqlCommand.Parameters["@TerminosInd"].Value = orden.TerminosInd.Value;
                mySqlCommand.Parameters["@VlrAnticipo"].Value = orden.VlrAnticipo.Value;
                mySqlCommand.Parameters["@DtoProntoPago"].Value = orden.DtoProntoPago.Value;
                mySqlCommand.Parameters["@DiasPtoPago"].Value = orden.DiasPtoPago.Value;
                mySqlCommand.Parameters["@FormaPago"].Value = orden.FormaPago.Value;
                mySqlCommand.Parameters["@Instrucciones"].Value = orden.Instrucciones.Value;
                mySqlCommand.Parameters["@Observaciones"].Value = orden.Observaciones.Value;
                mySqlCommand.Parameters["@ObservRechazo"].Value = orden.ObservRechazo.Value;
                mySqlCommand.Parameters["@FechaERechazo"].Value = orden.FechaERechazo.Value ?? SqlDateTime.Null;
                mySqlCommand.Parameters["@TasaOrden"].Value = orden.TasaOrden.Value;
                mySqlCommand.Parameters["@UsuarioRechaza"].Value = orden.UsuarioRechaza.Value;
                mySqlCommand.Parameters["@PorcentAdministra"].Value = orden.PorcentAdministra.Value;
                mySqlCommand.Parameters["@Porcentimprevisto"].Value = orden.Porcentimprevisto.Value;
                mySqlCommand.Parameters["@PorcentUtilidad"].Value = orden.PorcentUtilidad.Value;
                mySqlCommand.Parameters["@IncluyeAUICosto"].Value = orden.IncluyeAUICosto.Value;
                mySqlCommand.Parameters["@PorcentHolgura"].Value = orden.PorcentHolgura.Value;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = orden.AreaAprobacion.Value;
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = orden.UsuarioSolicita.Value;
                mySqlCommand.Parameters["@Prioridad"].Value = orden.Prioridad.Value;
                mySqlCommand.Parameters["@Valor"].Value = orden.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = orden.IVA.Value;
                mySqlCommand.Parameters["@VlrPagoMes"].Value = orden.VlrPagoMes.Value;
                mySqlCommand.Parameters["@NroPagos"].Value = orden.NroPagos.Value;
                mySqlCommand.Parameters["@FechaPago1"].Value = orden.FechaPago1.Value;
                mySqlCommand.Parameters["@FechaVencimiento"].Value = orden.FechaVencimiento.Value;
                mySqlCommand.Parameters["@ContactoComercial"].Value = orden.ContactoComercial.Value;
                mySqlCommand.Parameters["@Encargado"].Value = orden.Encargado.Value;
                mySqlCommand.Parameters["@TelefonoEntrega"].Value = orden.TelefonoEntrega.Value;
                mySqlCommand.Parameters["@DireccionEntrega"].Value = orden.DireccionEntrega.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de Compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraAprob> DAL_prOrdenCompraDocu_GetPendientesByModulo(DTO_glDocumento doc, string actividadFlujoID, DTO_glActividadPermiso actividadPerm, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_prOrdenCompraAprob> result = new List<DTO_prOrdenCompraAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                //mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = doc.ModuloID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                //mySqlCommand.Parameters["@AreaAprobacion"].Value = usuario.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = usuario.ReplicaID.Value;
                #endregion

                mySqlCommand.CommandText =
                    "   select temp.*, " +
                    "    det.ConsecutivoDetaID,det.OrdCompraDocuID,det.OrdCompraDetaID,det.CodigoBSID,det.Descriptivo,det.inReferenciaID,det.EmpaqueInvID as UnidadInvID,det.CantidadOC " +
                    "    ,solCtrl.PrefijoID PrefijoSolID,det.SolicitudDocuID,det.SolicitudDetaID,det.ValorUni,det.ValorTotML,det.IvaTotML,det.ValorTotME,det.IvaTotME,refer.MarcaInvID, refer.RefProveedor " +
                    "    ,empaque.UnidadInvID as UnidadEmpaque,IsNull(empaque.Cantidad,1) as CantidadEmpaque " +
                    "    from ( select distinct ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc Fecha,ctrl.Observacion Justificacion, " +
                    "        pr.ProveedorID,pr.Descriptivo ProveedorNombre,oc.LugarEntrega,oc.MonedaOrden,oc.MonedaPago,docAprueba.UsuarioAprueba,usr.UsuarioID, " +
                    "        SUM(det.ValorTotML) ValorTotalML,SUM(det.ValorTotME) ValorTotalME  " +
                    "    from glDocumentoControl ctrl with(nolock) " + 
                    "        inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc  " +
                    "               and act.CerradoInd=0  and act.ActividadFlujoID= @ActividadFlujoID  " +
	                "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
	                "        inner join prOrdenCompraDocu oc with(nolock) on ctrl.NumeroDoc = oc.NumeroDoc  " +
	                "        inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
	                "        inner join prProveedor pr with(nolock) on oc.ProveedorID = pr.ProveedorID " +
	                "        inner join prDetalleDocu det with(nolock) on oc.NumeroDoc = det.NumeroDoc " +
                    "        inner join glDocumentoAprueba docAprueba with(nolock) on docAprueba.NumeroDoc = ctrl.NumeroDoc " +
                    "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " +
                    "    and docAprueba.UsuarioAprueba = @UsuarioAprueba"  +
                    "    group by ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc,ctrl.Observacion, " +
                    "        pr.ProveedorID,pr.Descriptivo,oc.LugarEntrega,oc.MonedaOrden,oc.MonedaPago,docAprueba.UsuarioAprueba,usr.UsuarioID ) temp  " +
                    "        inner join prDetalleDocu det with(nolock) on temp.EmpresaID = det.EmpresaID and temp.NumeroDoc = det.NumeroDoc " +
                    "        left join inReferencia refer with(nolock) on refer.inReferenciaID = det.inReferenciaID and refer.EmpresaGrupoID = det.eg_inReferencia " +
                    "        left join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = refer.EmpaqueInvID and empaque.EmpresaGrupoID = refer.eg_inEmpaque " +
                    "        inner join glDocumentoControl solCtrl on det.EmpresaID = solCtrl.EmpresaID and det.SolicitudDocuID = solCtrl.NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_prOrdenCompraAprob dto = new DTO_prOrdenCompraAprob(dr);
                    List<DTO_prOrdenCompraAprob> list = result.Where(x => ((DTO_prOrdenCompraAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_prOrdenCompraAprob(dr);
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                    }

                    DTO_prOrdenCompraAprobDet dtoDet = new DTO_prOrdenCompraAprobDet(dr);
                    int cantEmp = Convert.ToInt32(dr["CantidadEmpaque"]);
                    dtoDet.CantidadOC.Value = dtoDet.CantidadOC.Value / (cantEmp != 0 ? cantEmp : 1);
                    dto.OrdenCompraAprobDet.Add(dtoDet);

                    if (nuevo)
                        result.Add(dto);
                }

                dr.Close();
                
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDocu_GetPendientesByModulo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de COmpra para Recibido
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraResumen> DAL_prOrdenCompraDocu_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, List<Tuple<string, string>> filtros)
        {
            try
            {
                List<DTO_prOrdenCompraResumen> result = new List<DTO_prOrdenCompraResumen>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filter = string.Empty;

                if (filtros != null)
                {
                    foreach (Tuple<string, string> filtro in filtros)
                        filter += " and " + filtro.Item1 + " = '" + filtro.Item2.Trim() + "' "; 
                }
                
                mySqlCommand.CommandText =
                "select ctrlOC.NumeroDoc,ctrlOC.DocumentoID,detOC.OrdCompraDocuID, detOC.ContratoDocuID,ctrlOC.PrefijoID PrefijoIDOC,ctrlOC.DocumentoNro DocumentoNroOC,solCargo.ProyectoID, " +
                "    detOC.SolicitudDocuID,ctrlSol.PrefijoID PrefijoIDSol,ctrlSol.DocumentoNro DocumentoNroSol, detOC.LineaPresupuestoID," +
                "    detOC.SolicitudDetaID,detOC.OrdCompraDetaID,detOC.CodigoBSID,detOC.inReferenciaID,detOC.Descriptivo, " +
                "    detSol.CantidadSol,detSol.UnidadInvID,detOC.EmpaqueInvID, refer.MarcaInvID,refer.RefProveedor,empaque.UnidadInvID as UnidadEmpaque,IsNull(empaque.Cantidad,1) as CantidadEmpaque, " +
                "    detOC.CantidadOC CantidadOrdComp,temp.CantidadSum CantidadOC,ctrlOC.FechaDoc FechaOC,ctrlOC.MonedaID MonedaIDOC, " +
                "    detSol.Documento1ID,detSol.Documento2ID,detSol.Documento3ID,detSol.Documento4ID,detSol.Documento5ID,  " +
                "    detSol.Detalle1ID,detSol.Detalle2ID,detSol.Detalle3ID,detSol.Detalle4ID,detSol.Detalle5ID,  " +
                "    detSol.CantidadDoc1,detSol.CantidadDoc2,detSol.CantidadDoc3,detSol.CantidadDoc4,detSol.CantidadDoc5, oc.ProveedorID,oc.MonedaPago as MonedaPagoOC  " +
                "from ( select det.OrdCompraDocuID, det.OrdCompraDetaID, SUM(det.CantidadOC) CantidadSum  " +
	            "        from prDetalleDocu det with(nolock)  " +
		        "        inner join glDocumentoControl ctrl with(nolock) on det.NumeroDoc = ctrl.NumeroDoc  " +
		        "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +
                "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID  " +
                "       and (ctrl.Estado = @Estado and (ctrl.DocumentoID = @DocumentoID1 or ctrl.DocumentoID = @DocumentoID3)) or ((ctrl.Estado in(2,3)) and ctrl.DocumentoID = @DocumentoID2) " + 
                "group by det.OrdCompraDocuID,det.OrdCompraDetaID ) temp  " +
                "    inner join prDetalleDocu detOC with(nolock) on temp.OrdCompraDetaID = detOC.ConsecutivoDetaID " +
                "    inner join prDetalleDocu detSol with(nolock) on detOC.SolicitudDetaID = detSol.ConsecutivoDetaID  " + 
	            "    inner join glDocumentoControl ctrlOC with(nolock) on temp.OrdCompraDocuID = ctrlOC.NumeroDoc  " +
                "    inner join glDocumentoControl ctrlSol with(nolock) on detOC.SolicitudDocuID = ctrlSol.NumeroDoc  " +
                "    inner join prOrdenCompraDocu oc with(nolock) on oc.NumeroDoc = ctrlOC.NumeroDoc   " +
                "    inner join prSolicitudCargos solCargo with(nolock) on detSol.ConsecutivoDetaID = solCargo.ConsecutivoDetaID " +
                "    left join inReferencia refer with(nolock) on refer.inReferenciaID = detSol.inReferenciaID and refer.EmpresaGrupoID = detSol.eg_inReferencia  " +
                "    left join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = detOC.EmpaqueInvID and empaque.EmpresaGrupoID = detOC.eg_inEmpaque " +
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
                    dtoRes.CantEmpaque.Value = dtoRes.CantidadxEmpaque.Value > 1 ? Math.Ceiling(Math.Abs(dtoRes.CantidadOC.Value.Value) / dtoRes.CantidadxEmpaque.Value.Value) : 1;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prOrdenCompraDocu_GetResumen");
                throw exception;
            }
        }
        #endregion
    }
}
