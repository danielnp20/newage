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
    public class DAL_ccCompraDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCompraDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public DTO_ccVentaDocu DAL_ccCompraDocu_GetByID(int NumeroDoc)
        {
            try
            {
                DTO_ccVentaDocu result = new DTO_ccVentaDocu();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccCompraDocu with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccVentaDocu(dr);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCompraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCompraDocu_Add(DTO_ccCompraDocu compraDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccVentaDocu   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[DocCtaxPagar] " +
                                               "    ,[FactVendedor] " +
                                               "    ,[VendedorID] " +
                                               "    ,[NumCuotas] " +
                                               "    ,[Oferta] " +
                                               "    ,[Observacion] " +
                                               "    ,[FechaPago1] " +
                                               "    ,[FechaPagoUlt] " +
                                               "    ,[TasaCompra] " +
                                               "    ,[Valor] " +
                                               "    ,[Iva] " +
                                               "    ,[eg_VendedorCartera] ) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@DocCtaxPagar " +
                                               "  ,@FactVendedor " +
                                               "  ,@VendedorID " +
                                               "  ,@NumCuotas " +
                                               "  ,@Oferta " +
                                               "  ,@Observacion " +
                                               "  ,@FechaPago1 " +
                                               "  ,@FechaPagoUlt " +
                                               "  ,@TasaCompra " +
                                               "  ,@Valor " +
                                               "  ,@Iva " +
                                               "  ,@eg_VendedorCartera) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocCtaxPagar", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FactVendedor", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VendedorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumCuotas", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaPago1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaPagoUlt", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TasaCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_VendedorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compraDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocCtaxPagar"].Value = compraDocu.DocCtaxPagar.Value;
                mySqlCommandSel.Parameters["@FactVendedor"].Value = compraDocu.FactVendedor.Value;
                mySqlCommandSel.Parameters["@VendedorID"].Value = compraDocu.VendedorID.Value;
                mySqlCommandSel.Parameters["@NumCuotas"].Value = compraDocu.NumCuotas.Value;
                mySqlCommandSel.Parameters["@Oferta"].Value = compraDocu.Oferta.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = compraDocu.Observacion.Value;
                mySqlCommandSel.Parameters["@FechaPago1"].Value = compraDocu.FechaPago1.Value;
                mySqlCommandSel.Parameters["@FechaPagoUlt"].Value = compraDocu.FechaPagoUlt.Value;
                mySqlCommandSel.Parameters["@TasaCompra"].Value = compraDocu.TasaCompra.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = compraDocu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = compraDocu.Iva.Value;
                mySqlCommandSel.Parameters["@eg_VendedorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccVendedorCartera, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCompraDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccCompraDocu_Update(DTO_ccCompraDocu compraDocu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocCtaxPagar", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FactVendedor", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@VendedorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumCuotas", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaPago1", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaPagoUlt", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TasaCompra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Iva", SqlDbType.Decimal);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compraDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocCtaxPagar"].Value = compraDocu.DocCtaxPagar.Value;
                mySqlCommandSel.Parameters["@FactVendedor"].Value = compraDocu.FactVendedor.Value;
                mySqlCommandSel.Parameters["@VendedorID"].Value = compraDocu.VendedorID.Value;
                mySqlCommandSel.Parameters["@NumCuotas"].Value = compraDocu.NumCuotas.Value;
                mySqlCommandSel.Parameters["@Oferta"].Value = compraDocu.Oferta.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = compraDocu.Observacion.Value;
                mySqlCommandSel.Parameters["@FechaPago1"].Value = compraDocu.FechaPago1.Value;
                mySqlCommandSel.Parameters["@FechaPagoUlt"].Value = compraDocu.FechaPagoUlt.Value;
                mySqlCommandSel.Parameters["@TasaCompra"].Value = compraDocu.TasaCompra.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = compraDocu.Valor.Value;
                mySqlCommandSel.Parameters["@Iva"].Value = compraDocu.Iva.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccCompraDocu SET" +
                    "  DocCtaxPagar = @DocCtaxPagar " +
                    "  ,FactVendedor = @FactVendedor " +
                    "  ,VendedorID = @VendedorID " +
                    "  ,NumCuotas = @NumCuotas " +
                    "  ,Oferta = @Oferta " +
                    "  ,Observacion = @Observacion " +
                    "  ,FechaPago1 = @FechaPago1 " +
                    "  ,FechaPagoUlt = @FechaPagoUlt " +
                    "  ,TasaCompra = @TasaCompra " +
                    "  ,Valor = @Valor " +
                    "  ,Iva = @Iva " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCompraDocu_Update");
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
        public List<DTO_ccCompraDocu> DAL_ccCompraDocu_GetByCompradorCartera(string compradorCarteraID, string oferta, DateTime MesINI, DateTime MesFIN)
        {
            try
            {
                List<DTO_ccCompraDocu> results = new List<DTO_ccCompraDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@MesINI", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@MesFIN", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                mySqlCommand.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@MesINI"].Value = MesINI.Date;
                mySqlCommand.Parameters["@MesFIN"].Value = new DateTime(MesFIN.Year, MesFIN.Month, DateTime.DaysInMonth(MesFIN.Year, MesFIN.Month));

                string whereOferta = string.Empty;
                if (!string.IsNullOrEmpty(oferta))
                {
                    mySqlCommand.Parameters.Add("@Oferta", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                    mySqlCommand.Parameters["@Oferta"].Value = oferta;
                    whereOferta = "and venta.Oferta = @Oferta";
                }
                #endregion
                #region Query Cooperativa
                mySqlCommand.CommandText =
                    "SELECT venta.* , venta.Oferta, venta.FactorCesion,docCtrl.FechaDoc " +
                        "FROM ccVentaDocu venta with(nolock) " +
                        "INNER JOIN glDocumentoControl docCtrl with(nolock) ON venta.NumeroDoc = docCtrl.NumeroDoc " +
                    "WHERE venta.CompradorCarteraID = @CompradorCarteraID " +
                    "and docCtrl.Estado = @Estado and FechaDoc between @MesINI and @MesFIN" + whereOferta;
                #endregion // between convert(datetime, @MesINI, 103) and convert(datetime, @MesFIN , 103)
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCompraDocu ventaDocu = new DTO_ccCompraDocu(dr);
                    results.Add(ventaDocu);
                }
                dr.Close();
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccCompraDocu_GetByCompradorCartera");
                throw exception;
            }
        }

        #endregion
    }

}
