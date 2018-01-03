using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Configuration;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_ReportesProveedores
    /// </summary>
    public class DAL_ReportesProveedores : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesProveedores(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Compromisos vs Facturas

        /// <summary>
        /// Funcion que se encarga de  traer los compromisos contra las facturas
        /// </summary>
        /// <param name="FechaInicial">Fecha de consulta inicial</param>
        /// <param name="FechaFinal">Fecha de consulta final</param>
        /// <param name="proveedor">Filtra un proveedor en especifico</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportCompromisoVSFacturas> DAL_ReportesProveedores_CompromisosVSFacturas(DateTime FechaInicial, DateTime FechaFinal, string proveedor)
        {
            try
            {
                List<DTO_ReportCompromisoVSFacturas> result = new List<DTO_ReportCompromisoVSFacturas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string prov = !string.IsNullOrEmpty(proveedor) ? " AND ctrl.TerceroID = @Proveedor " : string.Empty;

                #endregion
                #region CommandText

                mySqlCommandSel.CommandText =
                    " SELECT Proveedor, DescProveedor, Recibido, FechaDoc, " +
                        " CASE WHEN(Documento5ID IS NULL)  " +
                            " THEN " +
                                " CompromisoID " +
                            " ELSE " +
                                " CAST(RTRIM(PrefijoID)+'-'+CONVERT(VARCHAR, Documento5ID)  AS VARCHAR(100))  " +
                        " END Compromiso, " +
                        " CASE WHEN(Documento5ID IS NULL) THEN TipoID ELSE 'OSD' END Tipo, " +
                        " MonedaID, Factura, NumeroRadica,Observacion,Valor,Iva	 " +
                    " FROM  " +
                    " ( " +
                        " SELECT  ctrl.terceroID Proveedor, ter.Descriptivo DescProveedor, " +
                            " CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS Recibido, FechaDoc , " +
                            " CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, OrdCompraDocuID)  AS VARCHAR(100)) AS CompromisoID, Documento5ID, PrefijoID, " +
                            " CASE WHEN (ContratoDocuID IS NULL ) THEN 'OCO' ELSE 'CON' END TipoID, ctrl.MonedaID, FacturaDocuID Factura, NumeroRadica,ctrl.Observacion, " +
                            " ctrl.Valor, ctrl.iva " +
                         " FROM prDetalleDocu deta WITH(NOLOCK) " +
                            " INNER JOIN  glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = deta.NumeroDoc " +
                            " INNER JOIN coTercero ter WITH(NOLOCK) ON  (ter.TerceroID = ctrl.TerceroID AND ter.EmpresaGrupoID = ctrl.eg_coTercero) " +
                            " INNER JOIN cpCuentaXPagar cxp WITH(NOLOCK) ON (cxp.NumeroDoc = deta.FacturaDocuID) " +
                        " WHERE deta.EmpresaID = @EmpresaID  " +
                            " AND RecibidoDocuID IS NOT NULL " +
                            " AND FechaDoc BETWEEN @FechaInic and @FechaFin " +
                            " /*AND ctrl.TerceroID = @Proveedor*/ " +
                            " GROUP BY ctrl.terceroID, ter.Descriptivo, PrefijoID, FechaDoc,DocumentoNro, OrdCompraDocuID, FechaDoc, Documento5ID, PrefijoID, ContratoDocuID, " +
                            " ctrl.MonedaID, FacturaDocuID, NumeroRadica, Observacion,ctrl.Valor, ctrl.iva " +
                    " ) Consulta ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInic", SqlDbType.Date);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.Date);
                mySqlCommandSel.Parameters.Add("@Proveedor", SqlDbType.Char, UDT_ProveedorID.MaxLength);

                #endregion
                #region Asignacion Valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaInic"].Value = FechaInicial;
                mySqlCommandSel.Parameters["@FechaFin"].Value = FechaFinal;
                mySqlCommandSel.Parameters["@Proveedor"].Value = prov;

                #endregion

                DTO_ReportCompromisoVSFacturas vs = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    vs = new DTO_ReportCompromisoVSFacturas(dr);
                    result.Add(vs);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_CompromisosVSFacturas");
                throw exception;
            }
        }

        #endregion

        #region Orden Compra

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// </summary>
        /// <param name="FechaIni">Fecha incial que se desea ver los datos</param>
        /// <param name="FechaFin">Fecha Final hasta donde quiere verificar los datos</param>
        /// <param name="Proveedor">Filtra un proveedor en especifico</param>
        /// <param name="Estado">Filtra el estado de la orden de compra</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="moneda">Tipo de Moneda que se desea ver</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportOrdenCompra> DAL_ReportesProveedores_OrdenCompraArchivo(DateTime FechaIni, DateTime FechaFin, string Proveedor, int Estado, bool isDetallado, string moneda)
        {
            try
            {
                List<DTO_ReportOrdenCompra> result = new List<DTO_ReportOrdenCompra>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string tipoMoneda = string.Empty;
                string prov = !string.IsNullOrEmpty(Proveedor) ? " AND ord.ProveedorID = @ProveedorID " : string.Empty;
                string est = Estado != 10 ? " AND ctrl.Estado = @Estado " : string.Empty;

                if (moneda == "Loc")
                    tipoMoneda = " ValorTotML Valor ";
                else
                    tipoMoneda = " ValorTotME Valor ";

                #endregion
                #region CommandText

                if (!isDetallado)
                {
                    mySqlCommandSel.CommandText =
                        " SELECT PrefijoID, CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS OrdenCompra, ctrl.FechaDoc, " +
                            " CAST(RTRIM(ord.ProveedorID)+' '+'-'+ ' ' +CONVERT(VARCHAR, prov.Descriptivo)  AS VARCHAR(100)) AS Proveedor, " +
                            " CAST(RTRIM( ctrl.AreaFuncionalID)+' '+'-'+ ' ' +CONVERT(VARCHAR, func.Descriptivo )  AS VARCHAR(100)) AS AreaFuncional, " +
                            " GETDATE() FechaAprueba, '' usuarioAprueba,ctrl.Valor,ctrl.Iva " +
                        " FROM prOrdenCompraDocu ord WITH(NOLOCK) " +
                            " INNER JOIN  glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = ord.NumeroDoc " +
                            " INNER JOIN prProveedor prov WITH(NOLOCK) ON (prov.ProveedorID = ord.ProveedorID AND prov.EmpresaGrupoID = ord.EmpresaID) " +
                            " INNER JOIN glAreaFuncional func WITH(NOLOCK) ON (func.AreaFuncionalID = ctrl	.AreaFuncionalID AND func.EmpresaGrupoID = ctrl.eg_glAreaFuncional) " +
                        " WHERE ORD.EmpresaID = @EmpresaID " +
                            " AND ctrl.FechaDoc BETWEEN @FechaIni AND @FechaFin  " + prov + est +
                            " /*AND ord.ProveedorID = @ProveedorID " +
                            " AND MonedaID = @MonedaID  " +
                            " AND ctrl.Estado = @Estado */ " +
                        " ORDER BY   FechaDoc DESC,OrdenCompra ";
                }
                else
                {
                    mySqlCommandSel.CommandText =
                        " SELECT PrefijoID, CAST(RTRIM(ctrl.PrefijoID)+ '' +'-'+ '' +CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS OrdenCompra, ctrl.FechaDoc, " +
                            " CAST(RTRIM(ord.ProveedorID)+' '+'-'+ ' ' +CONVERT(VARCHAR, prov.Descriptivo)  AS VARCHAR(100)) AS Proveedor, " +
                            " ProyectoID,CodigoBSID , deta.Descriptivo, ABS(CantidadSol) CantidadSol, ValorUni,ValorTotML, ValorTotME, "+
                            " CASE WHEN deta.OrigenMonetario <> 1 THEN deta.IvaTotML ELSE  deta.IvaTotML END as Iva, " + tipoMoneda +
                        " FROM prDetalleDocu deta " +
                            " INNER JOIN prOrdenCompraDocu ord WITH(NOLOCK) ON (ord.NumeroDoc = deta.NumeroDoc) " +
                            " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON (deta.NumeroDoc = ctrl.NumeroDoc) " +
                            " INNER JOIN prProveedor prov WITH(NOLOCK) ON (prov.ProveedorID = ord.ProveedorID AND prov.EmpresaGrupoID = ord.EmpresaID) " +
                        " WHERE deta.EmpresaID  = @EmpresaID " +
                            " AND ctrl.FechaDoc BETWEEN @FechaIni AND @FechaFin  " + prov + est +
                            " /*AND ord.ProveedorID = @ProveedorID " +
                            " AND MonedaID = @MonedaID  " +
                            " AND ctrl.Estado = @Estado */" +
                        " ORDER BY   FechaDoc DESC,OrdenCompra ";
                }

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.Date);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.Date);
                mySqlCommandSel.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.Int);

                #endregion
                #region Asignacion Valores parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaIni"].Value = FechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = FechaFin;
                mySqlCommandSel.Parameters["@ProveedorID"].Value = Proveedor;
                mySqlCommandSel.Parameters["@Estado"].Value = Estado;


                #endregion

                DTO_ReportOrdenCompra ord = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    ord = new DTO_ReportOrdenCompra(dr, isDetallado);
                    result.Add(ord);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_OrdenCompra");
                throw exception;
            }
        }

        #endregion

        #region Recibidos

        /// <summary>
        /// Funcion que se encarga de traer las ordenes recibidas
        /// </summary>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="isFacturdo">Verifica si desea imprimir el reporte de Documentos recibidos sin factura</param>
        /// <returns></returns>
        public List<DTO_ReportProveedoresRecibidos> DAL_ReportesProveedores_Recibidos(DateTime Periodo, string proveedor, bool isDetallado,int? numDoc, bool isFacturdo = false)
        {
            try
            {
                List<DTO_ReportProveedoresRecibidos> result = new List<DTO_ReportProveedoresRecibidos>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                SqlDataReader dr = null;
                if (!numDoc.HasValue)
                {

                    #region Filtros

                    string text = string.Empty;
                    string condition = string.Empty;
                    string prov = !string.IsNullOrEmpty(proveedor) ? " AND recb.ProveedorID = @ProveedorID " : string.Empty;

                    if (isFacturdo)
                    {
                        text = " ,ctrl.CentroCostoID, SUM(ctrl.Valor) Valor ";
                        condition = " AND FacturaDocuID IS  NULL ";
                    }

                    #endregion
                    #region CommandText

                    if (!isDetallado)
                    {
                        mySqlCommandSel.CommandText =
                            " SELECT CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS Recibido,us.Descriptivo as Elaboro, " +
                                " FechaDoc, CAST(RTRIM(recb.ProveedorID) + ' ' + '-' + ' ' + CONVERT(VARCHAR, prov.Descriptivo)  AS VARCHAR(100)) AS Proveedor, bod.Descriptivo as BodegaDesc, " +
                                " SUM(CantidadRec) CantidadRecidida  " +
                            " FROM prRecibidoDocu recb WITH(NOLOCK) " +
                                " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON (ctrl.NumeroDoc = recb.NumeroDoc) " +
                                " INNER JOIN seUsuario us WITH(NOLOCK) ON (us.ReplicaID = ctrl.seUsuarioID) " +
                                " INNER JOIN prProveedor prov WITH(NOLOCK) ON (prov.ProveedorID = recb.ProveedorID AND prov.EmpresaGrupoID = recb.eg_Proveedor) " +
                                " LEFT JOIN inBodega bod WITH(NOLOCK) ON (bod.BodegaID = recb.BodegaID) " +
                                " INNER JOIN prDetalleDocu deta WITH(NOLOCK) ON  (deta.RecibidoDocuID = recb.NumeroDoc) " +
                            " WHERE ctrl.EmpresaID = @EmpresaID " +
                                " AND DATEPART(YEAR, ctrl.FechaDoc) = @Year " +
                                " AND DATEPART(MONTH, ctrl.FechaDoc) = @Month " +
                            " GROUP BY ctrl.PrefijoID, ctrl.DocumentoNro, ctrl.FechaDoc, recb.ProveedorID, prov.Descriptivo,bod.Descriptivo,us.Descriptivo ";
                    }
                    else
                    {
                        mySqlCommandSel.CommandText =
                            " SELECT CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS Recibido,us.Descriptivo as Elaboro,  " +
                                " ctrl.FechaDoc, CAST(RTRIM(recb.ProveedorID) + ' ' + '-' + ' ' + CONVERT(VARCHAR, prov.Descriptivo)  AS VARCHAR(100)) AS Proveedor,bod.Descriptivo as BodegaDesc, " +
                                " SUM(deta.CantidadRec) CantidadRecidida, " +
                                " CAST(RTRIM(ctrlOC.PrefijoID)+'-'+CONVERT(VARCHAR, ctrlOC.DocumentoNro)  AS VARCHAR(100)) AS OrdenCompra, " +
                                " CodigoBSID, deta.Descriptivo " + text +
                            " FROM prRecibidoDocu recb WITH(NOLOCK) " +
                                " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON (ctrl.NumeroDoc = recb.NumeroDoc) " +
                                " INNER JOIN seUsuario us WITH(NOLOCK) ON (us.ReplicaID = ctrl.seUsuarioID) " +
                                " INNER JOIN prProveedor prov WITH(NOLOCK) ON (prov.ProveedorID = recb.ProveedorID AND prov.EmpresaGrupoID = recb.eg_Proveedor) " +
                                " LEFT JOIN inBodega bod WITH(NOLOCK) ON (bod.BodegaID = recb.BodegaID) " +
                                " INNER JOIN prDetalleDocu deta WITH(NOLOCK) ON  (deta.RecibidoDocuID = recb.NumeroDoc) " +
                                " INNER JOIN glDocumentoControl ctrlOC WITH(NOLOCK) ON (ctrlOC.NumeroDoc = deta.OrdCompraDocuID) " +
                            " WHERE ctrl.EmpresaID = @EmpresaID " +
                                " AND DATEPART(YEAR, ctrl.FechaDoc) = @Year " +
                                " AND DATEPART(MONTH, ctrl.FechaDoc) = @Month " + condition +
                            " GROUP BY ctrl.PrefijoID, ctrl.DocumentoNro, ctrl.FechaDoc, recb.ProveedorID, prov.Descriptivo, ctrlOC.PrefijoID, ctrlOC.DocumentoNro, " +
                                " deta.CodigoBSID,deta.Descriptivo,bod.Descriptivo,us.Descriptivo " +
                            " ORDER BY Recibido ";
                    }

                    #endregion
                    #region Parametros

                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Year", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@Month", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);

                    #endregion
                    #region Asignacion Valores a Parametros

                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@Year"].Value = Periodo.Year;
                    mySqlCommandSel.Parameters["@Month"].Value = Periodo.Month;
                    mySqlCommandSel.Parameters["@ProveedorID"].Value = proveedor;

                    #endregion
                    DTO_ReportProveedoresRecibidos aux = null;
                    dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        aux = new DTO_ReportProveedoresRecibidos(dr, isDetallado, isFacturdo);
                        aux.Elaboro.Value = dr["Elaboro"].ToString();
                        result.Add(aux);
                    }

                }
                else
                {
                    #region CommanText
                    mySqlCommandSel.CommandText =
                        " SELECT  PrefDoc, FechaDoc, Descriptivo,CantidadRec,Estado, ProveedorID,Elaboro, " +
                            " CodigoBSID, inReferenciaID,UnidadInvID,ProyectoID,CentroCostoID,LineaPresupuestoID " +
                        "  FROM  " +
                        " ( " +
                            " SELECT det.EmpresaID, CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS PrefDoc, ctrl.FechaDoc, " +
                                " det.Descriptivo, det.CantidadRec, ctrl.Estado, rec.ProveedorID,ctrl.NumeroDoc, ctrl.DocumentoID,det.CodigoBSID,us.Descriptivo as Elaboro, " +
                                " det.inReferenciaID,det.UnidadInvID,soliCargo.ProyectoID,soliCargo.CentroCostoID,soliCargo.LineaPresupuestoID " +
                            "  FROM prDetalleDocu AS det " +
                                "  INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON  ctrl.NumeroDoc = det.NumeroDoc " +
                                "  INNER JOIN seUsuario us WITH(NOLOCK) ON (us.ReplicaID = ctrl.seUsuarioID) " +
                                "  INNER JOIN prRecibidoDocu rec  WITH(NOLOCK) ON  rec.NumeroDoc = det.NumeroDoc " +
                                "  INNER JOIN prSolicitudCargos  soliCargo WITH(NOLOCK) ON soliCargo.ConsecutivoDetaID = det.SolicitudDetaID " +
                            " WHERE det.EmpresaID = @EmpresaID and  ctrl.NumeroDoc = @NumeroDoc" +
                        " ) as Consulta ";

                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                    #endregion
                    DTO_ReportProveedoresRecibidos aux = null;
                    dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        aux = new DTO_ReportProveedoresRecibidos(dr);
                        aux.Elaboro.Value = dr["Elaboro"].ToString();
                        result.Add(aux);
                    }
                }
               
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_Recibidos");
                throw exception;
            }
        }

        /// <summary>
        /// Genera un reporte de documento de recibidos 
        /// </summary>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="isFacturdo">Verifica si desea imprimir el reporte de Documentos recibidos sin factura</param>
        /// <returns></returns>
        public List<DTO_ReportProveedoresRecibidos> DAL_ReportesProveedores_RecibidoDoc()
        {
            try
            {
                List<DTO_ReportProveedoresRecibidos> result = new List<DTO_ReportProveedoresRecibidos>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                
         
                DTO_ReportProveedoresRecibidos aux = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    aux = new DTO_ReportProveedoresRecibidos(dr);
                    result.Add(aux);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_Recibidos");
                throw exception;
            }
        }

        #endregion

        #region Solicitudes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="numDoc"></param>
        /// <returns></returns>
        public List<DTO_ReportProveedoresSolicitudes> DAL_ReportesProveedores_Solicitudes(Dictionary<int, string> filtros, int? numDoc)
        {
            try
            {
                List<DTO_ReportProveedoresSolicitudes> result = new List<DTO_ReportProveedoresSolicitudes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                SqlDataReader dr = null;

                if (!numDoc.HasValue)
                {
                    #region Parametros de condicionales
                    //Lista para el filtro de las consulta
                    string centroCosto = "", proyecto = "", bienServicio = "", referencia = "", area = "", destino = "", usuarioSoli = "";

                    if (!string.IsNullOrEmpty(filtros[0]))
                        centroCosto = " AND soliCargo.CentroCostoID = " + "'" + filtros[0] + "'";

                    if (!string.IsNullOrEmpty(filtros[1]))
                        proyecto = " AND soliCargo.ProyectoID = " + "'" + filtros[1] + "'";

                    if (!string.IsNullOrEmpty(filtros[2]))
                        bienServicio = " AND detaDocu.CodigoBSID = " + "'" + filtros[2] + "'";

                    if (!string.IsNullOrEmpty(filtros[3]))
                        referencia = " AND detaDocu.inReferenciaID = " + "'" + filtros[3] + "'";

                    if (!string.IsNullOrEmpty(filtros[4]))
                        area = " AND soli.LugarEntrega = " + "'" + filtros[4] + "'";

                    if (!string.IsNullOrEmpty(filtros[5]))
                        destino = " AND soli.Destino = " + "'" + filtros[5] + "'";

                    if (!string.IsNullOrEmpty(filtros[6]))
                        usuarioSoli = " AND SOLI.UsuarioSolicita = " + "'" + filtros[6] + "'";

                    #endregion
                    #region CommanText

                    mySqlCommandSel.CommandText =
                        " SELECT  PrefDoc, FechaDoc, Descriptivo,CantidadSol, CantidadOC, " +
                            " CASE WHEN (CantidadPendiente is null ) Then CantidadSol  else CantidadPendiente End as CantidadPendiente, " +
                            " CASE WHEN (Estado = 1) THEN 'Sin Aprobar' " +
                                " WHEN (Estado = 2) THEN 'Para Aprobacion' " +
                                " WHEN(Estado = 3 ) THEN 'Aprobadas' " +
                                " WHEN(Estado = 0 ) THEN 'Anulada'  " +
                                " ELSE 'Tramitadas' end as Estado, " +
                            " UsuarioSolicita, UsuarioPreaprueba, GerenAprueba,FechaPreaprueba, aprueba,fechaAprobacion, ProyectoID, LugarEntrega,  " +
                            " CASE WHEN(OrdCompraDocuID IS NULL ) THEN 0 ELSE OrdCompraDocuID END AS OrdCompraDocuID, " +
                            " CodigoBSID, inReferenciaID,UnidadInvID,ProyectoID,CentroCostoID,LineaPresupuestoID " +
                        "  FROM  " +
                        " ( " +
                        " SELECT det.EmpresaID, CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS PrefDoc, ctrl.FechaDoc, " +
                            " det.Descriptivo, det.CantidadSol, CASE WHEN ( det.CantidadOC  IS NULL) THEN 0 ELSE CantidadOC  END AS CantidadOC,  " +
                            " (det.CantidadSol - det.CantidadOC) as CantidadPendiente, ctrl.Estado, soli.UsuarioSolicita,ctrl.ProyectoID,soli.LugarEntrega, " +
                            " ctrl.NumeroDoc, ctrl.DocumentoID,UsuarioPre.seUsuarioID as UsuarioPreaprueba,UsuarioGerenApru.seUsuarioID as GerenAprueba,UsuarioPre.FechaInicio as FechaPreaprueba, " +
                            " UsuarioGerenApru.seUsuarioID as aprueba, UsuarioGerenApru.FechaFin as fechaAprobacion,det.OrdCompraDocuID, " +
                            " det.CodigoBSID, det.inReferenciaID,det.UnidadInvID,soliCargo.ProyectoID,soliCargo.CentroCostoID,soliCargo.LineaPresupuestoID " +
                        "  FROM prDetalleDocu AS det " +
                            "  INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON  ctrl.NumeroDoc = det.NumeroDoc " +
                            "  INNER JOIN prSolicitudDocu soli  WITH(NOLOCK) ON  soli.NumeroDoc = det.NumeroDoc " +
                            "  INNER JOIN prSolicitudCargos  soliCargo WITH(NOLOCK) ON soliCargo.ConsecutivoDetaID = det.ConsecutivoDetaID " +
                            "  LEFT JOIN glActividadEstado UsuarioPre on UsuarioPre.NumeroDoc = ctrl.NumeroDoc  " +
                            "  LEFT JOIN glActividadEstado UsuarioGerenApru on UsuarioGerenApru.NumeroDoc = ctrl.NumeroDoc " +
                        " WHERE det.EmpresaID = @Empresa " +
                            centroCosto + proyecto + bienServicio + referencia + area + destino + usuarioSoli +
                        " ) as Consulta ";

                    #endregion
                    #region Parametros

                    mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                    #endregion
                    #region Valores Parametros

                    mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;

                    #endregion
                    DTO_ReportProveedoresSolicitudes aux = null;
                    dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        aux = new DTO_ReportProveedoresSolicitudes(dr);

                        if (!string.IsNullOrEmpty(dr["UsuarioPreaprueba"].ToString()))
                            aux.UsuarioPreaprueba.Value = dr["UsuarioPreaprueba"].ToString();
                        if (!string.IsNullOrEmpty(dr["GerenAprueba"].ToString()))
                            aux.GerenAprueba.Value = dr["GerenAprueba"].ToString();
                        if (!string.IsNullOrEmpty(dr["FechaPreaprueba"].ToString()))
                            aux.FechaPreaprueba.Value = Convert.ToDateTime(dr["FechaPreaprueba"]);
                        if (!string.IsNullOrEmpty(dr["aprueba"].ToString()))
                            aux.aprueba.Value = dr["aprueba"].ToString();
                        if (!string.IsNullOrEmpty(dr["fechaAprobacion"].ToString()))
                            aux.fechaAprobacion.Value = Convert.ToDateTime(dr["fechaAprobacion"]);
                        result.Add(aux);
                    }
                }
                else
                {                    
                    #region CommanText
                    mySqlCommandSel.CommandText =
                        " SELECT  PrefDoc, FechaDoc, Descriptivo,CantidadSol, CantidadOC, " +
                            " CASE WHEN (CantidadPendiente is null ) Then CantidadSol  else CantidadPendiente End as CantidadPendiente, " +
                            " CASE WHEN (Estado = 1) THEN 'Sin Aprobar' " +
                                " WHEN (Estado = 2) THEN 'Para Aprobacion' " +
                                " WHEN(Estado = 3 ) THEN 'Aprobadas' " +
                                " WHEN(Estado = 0 ) THEN 'Anulada'  " +
                                " ELSE 'Tramitadas' end as Estado, UsuarioSolicita, LugarEntrega,  " +                          
                            " CASE WHEN(OrdCompraDocuID IS NULL ) THEN 0 ELSE OrdCompraDocuID END AS OrdCompraDocuID, " +
                            " CodigoBSID, inReferenciaID,UnidadInvID,ProyectoID,CentroCostoID,LineaPresupuestoID " +
                        "  FROM  " +
                        " ( " +
                            " SELECT det.EmpresaID, CAST(RTRIM(ctrl.PrefijoID)+'-'+CONVERT(VARCHAR, ctrl.DocumentoNro)  AS VARCHAR(100)) AS PrefDoc, ctrl.FechaDoc, " +
                                " det.Descriptivo, det.CantidadSol, CASE WHEN ( det.CantidadOC  IS NULL) THEN 0 ELSE CantidadOC  END AS CantidadOC, " +
                                " (det.CantidadSol - det.CantidadOC) as CantidadPendiente, ctrl.Estado, soli.UsuarioSolicita,soli.LugarEntrega, " +
                                " ctrl.NumeroDoc, ctrl.DocumentoID, det.OrdCompraDocuID,det.CodigoBSID, det.inReferenciaID,det.UnidadInvID,soliCargo.ProyectoID,soliCargo.CentroCostoID,soliCargo.LineaPresupuestoID " +
                            "  FROM prDetalleDocu AS det " +
                                "  INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON  ctrl.NumeroDoc = det.NumeroDoc " +
                                "  INNER JOIN prSolicitudDocu soli  WITH(NOLOCK) ON  soli.NumeroDoc = det.NumeroDoc " +
                                "  INNER JOIN prSolicitudCargos  soliCargo WITH(NOLOCK) ON soliCargo.ConsecutivoDetaID = det.ConsecutivoDetaID " +
                            " WHERE det.EmpresaID = @Empresa and  ctrl.NumeroDoc = @NumeroDoc" +
                        " ) as Consulta ";

                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    #endregion
                    #region Valores Parametros
                    mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;
                    #endregion
                    DTO_ReportProveedoresSolicitudes aux = null;
                    dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        aux = new DTO_ReportProveedoresSolicitudes(dr);
                        result.Add(aux);
                    }
                }

              
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_Solicitudes");
                throw exception;
            }
        }

        #endregion

        #region Orden Compra

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// <param name="NumDoc">numero del Doc</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportOrdenCompraDoc> DAL_ReportesOrdenCompra(int NumDoc,string mdaLocal)
        {
            try
            {
                List<DTO_ReportOrdenCompraDoc> result = new List<DTO_ReportOrdenCompraDoc>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT prov.Descriptivo as Proveedor,prov.TerceroID, prov.Direccion as DireccionProv, ter.Tel1 as TelefonoProv, " +
                    " orde.ContactoComercial, CASE WHEN prov.TelContacto is null THEN prov.TelContactoComercial ELSE prov.TelContacto END as TelContacto, " +
                    " CASE WHEN prov.MailContacto is null THEN prov.MailContactoComercial ELSE prov.MailContacto END as MailContacto, " +
                    "    orde.LugarEntrega, locFis.Descriptivo as LugarEntregaDesc, orde.Encargado as EncargadoRecibe, orde.DireccionEntrega, orde.TelefonoEntrega,     " +
                    "    lugarTer.Descriptivo +' - ' + pais.Descriptivo AS CiudadPais, " +                  
                    "    CASE WHEN ctrl.Estado=-1 THEN 'CERRADO'    WHEN ctrl.Estado=0 THEN 'ANULADO' " +
                    "       WHEN ctrl.Estado=1 THEN 'SIN APROBAR'   WHEN ctrl.Estado=2 THEN 'PARA APROBAR' " +
                    "       WHEN ctrl.Estado=3 THEN 'APROBADO'      WHEN ctrl.Estado=4 THEN 'REVERTIDO' " +
                    "       WHEN ctrl.Estado=5 THEN 'DEVUELTO'      WHEN ctrl.Estado=6 THEN 'RADICADO' " +
                    "       WHEN ctrl.Estado=7 THEN 'REVISADO'      WHEN ctrl.Estado=8 THEN 'CONTABILIZADO'  END as Estado, " +
                    "    ctrl.FechaDoc as Fecha, us.Descriptivo as Elaboro, ctrl.ProyectoID,proy.Descriptivo as ProyectoDesc, ctrl.Fecha as FechaConsec," +
                    "    Cast(RTrim(ctrl.PrefijoID)+'-'+Convert(Varchar, ctrl.DocumentoNro)  as Varchar(100)) as PrefDoc, " +
                    "    orde.MonedaOrden as MonedaNeg, orde.MonedaPago, orde.FechaEntrega," +
                   // "    ROW_NUMBER() OVER(ORDER BY ctrl.NumeroDoc) as Item, " +
                    "    deta.CodigoBSID as BienServicio, " +
                    "    deta.CantidadOC as Cantidad, " +
                    "    deta.UnidadInvID as Unidad, " +
                    "    deta.inReferenciaID as Referencia, " +
                    "    ref.MarcaInvID,  ref.RefProveedor,(Case When deta.EmpaqueInvID is null Then ref.EmpaqueInvID Else deta.EmpaqueInvID End) as EmpaqueInvID,empaque.Cantidad as CantidadEmpaque, " +
                    "    deta.Descriptivo as Descripcion,   " +
                    "    deta.ValorUni as ValorUnitario, " +
                    "    CASE WHEN orde.MonedaOrden = @MdaLocal THEN deta.ValorTotML ELSE  deta.ValorTotME END as ValorTotal, " +
                    "    deta.ValorAIU, " +
                    "    CASE WHEN orde.MonedaOrden = @MdaLocal THEN deta.IvaTotML ELSE  deta.IvaTotME END as ValorIVA, " +
                    "    orde.FormaPago, orde.Observaciones as Clausulas, orde.Instrucciones,orde.Observaciones,orde.DtoProntoPago " +                
                    " FROM " +
                    "    glDocumentoControl ctrl WITH (NOLOCK) " +
                    "    INNER JOIN prOrdenCompraDocu orde WITH (NOLOCK) ON orde.NumeroDoc=ctrl.NumeroDoc " +
                    "    INNER JOIN prDetalleDocu deta WITH (NOLOCK) ON deta.NumeroDoc=ctrl.NumeroDoc " +
                    "    LEFT JOIN coProyecto proy WITH(NOLOCK) on proy.ProyectoID = ctrl.ProyectoID " +
                    "    LEFT JOIN glLocFisica locFis WITH(NOLOCK) on locFis.LocFisicaID = orde.LugarEntrega and locFis.EmpresaGrupoID = orde.eg_glLocFisica " +
                    "    INNER JOIN prProveedor prov WITH (NOLOCK) ON prov.ProveedorID=orde.ProveedorID and prov.EmpresaGrupoID=orde.eg_prProveedor " +
                    "    LEFT JOIN inReferencia ref WITH (NOLOCK) ON ref.inReferenciaID=deta.inReferenciaID and ref.EmpresaGrupoID=deta.eg_inReferencia " +
                    "    LEFT join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = ref.EmpaqueInvID and empaque.EmpresaGrupoID = ref.eg_inEmpaque " +
                    "    LEFT JOIN coTercero ter WITH (NOLOCK) ON ter.TerceroID=ctrl.TerceroID and ter.EmpresaGrupoID=ctrl.eg_coTercero " +
                    "    INNER JOIN glLugarGeografico lugarTer WITH(NOLOCK) ON lugarTer.LugarGeograficoID=ter.LugarGeograficoID and lugarTer.EmpresaGrupoID=ter.eg_glLugarGeografico " +
                    "    INNER JOIN glPais pais WITH(NOLOCK) on pais.PaisID=lugarTer.PaisID " +
                    "    LEFT JOIN seUsuario us WITH(NOLOCK) on us.ReplicaID = ctrl.seUsuarioID " +
                    " WHERE  " +
                    "    ctrl.NumeroDoc=@NumDoc ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@NumDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MdaLocal", SqlDbType.Char,UDT_MonedaID.MaxLength);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@NumDoc"].Value = NumDoc;
                mySqlCommandSel.Parameters["@MdaLocal"].Value = mdaLocal;
                #endregion

                DTO_ReportOrdenCompraDoc aux = null;
                int index = 1;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    aux = new DTO_ReportOrdenCompraDoc(dr);
                    aux.Item.Value = (index++).ToString();
                    result.Add(aux);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_Solicitudes");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// <param name="NumDoc">numero del Doc</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportOrdenCompraDoc> DAL_ReportesOrdenCompraAnexo(int NumDoc, string mdaLocal)
        {
            try
            {
                List<DTO_ReportOrdenCompraDoc> result = new List<DTO_ReportOrdenCompraDoc>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT prov.Descriptivo as Proveedor,prov.TerceroID, prov.Direccion as DireccionProv, ter.Tel1 as TelefonoProv, " +
                    "  orde.ContactoComercial, CASE WHEN prov.TelContacto is null THEN prov.TelContactoComercial ELSE prov.TelContacto END as TelContacto, " +
                    " CASE WHEN prov.MailContacto is null THEN prov.MailContactoComercial ELSE prov.MailContacto END as MailContacto, " +
                    "    orde.LugarEntrega,locFis.Descriptivo as LugarEntregaDesc, orde.Encargado as EncargadoRecibe, orde.DireccionEntrega, orde.TelefonoEntrega,     " +
                    "    lugarTer.Descriptivo +' - ' + pais.Descriptivo AS CiudadPais, " +
                    "    CASE WHEN ctrl.Estado=-1 THEN 'CERRADO'    WHEN ctrl.Estado=0 THEN 'ANULADO' " +
                    "       WHEN ctrl.Estado=1 THEN 'SIN APROBAR'   WHEN ctrl.Estado=2 THEN 'PARA APROBAR' " +
                    "       WHEN ctrl.Estado=3 THEN 'APROBADO'      WHEN ctrl.Estado=4 THEN 'REVERTIDO' " +
                    "       WHEN ctrl.Estado=5 THEN 'DEVUELTO'      WHEN ctrl.Estado=6 THEN 'RADICADO' " +
                    "       WHEN ctrl.Estado=7 THEN 'REVISADO'      WHEN ctrl.Estado=8 THEN 'CONTABILIZADO'  END as Estado, " +
                    "    ctrl.FechaDoc as Fecha, ctrl.TerceroID as Elaboro, ctrl.ProyectoID,proy.Descriptivo as ProyectoDesc, ctrl.Fecha as FechaConsec," +
                    "   Cast(RTrim(ctrl.PrefijoID)+'-'+Convert(Varchar, ctrl.DocumentoNro)  as Varchar(100)) as PrefDoc, " +
                    "    orde.MonedaOrden as MonedaNeg, orde.MonedaPago, orde.FechaEntrega," +
                   // "    ROW_NUMBER() OVER(ORDER BY ctrl.NumeroDoc) as Item, " +
                    "    deta.CodigoBSID as BienServicio, " +
                    "    deta.CantidadOC as Cantidad, " +
                    "    deta.UnidadInvID as Unidad, " +
                    "    deta.inReferenciaID as Referencia, " +
                    "    ref.MarcaInvID,  ref.RefProveedor, ref.EmpaqueInvID,empaque.Cantidad as CantidadEmpaque, " +
                    "    deta.Descriptivo as Descripcion,   " +
                    "    deta.ValorUni as ValorUnitario, " +
                    "    deta.ValorUni*deta.CantidadOC as ValorTotal, " +
                    "    deta.ValorAIU, " +
                    "    CASE WHEN deta.OrigenMonetario <> 1 THEN deta.IvaTotML ELSE  deta.IvaTotML END as ValorIVA, " +
                    "    orde.FormaPago, orde.Observaciones as Clausulas, orde.Instrucciones,orde.Observaciones, orde.DtoProntoPago," +
                    "    (Select top 1 Detalle4ID from prDetalleDocu where ConsecutivoDetaID = deta.SolicitudDetaID) as ConsProyMvto, " +
                    "    (Select top 1 Documento4ID from prDetalleDocu where ConsecutivoDetaID = deta.SolicitudDetaID) as NumDocProyMvto  " +
                    " FROM " +
                    "    glDocumentoControl ctrl WITH (NOLOCK) " +
                    "    INNER JOIN prOrdenCompraDocu orde WITH (NOLOCK) ON orde.NumeroDoc=ctrl.NumeroDoc " +
                    "    INNER JOIN prDetalleDocu deta WITH (NOLOCK) ON deta.NumeroDoc=ctrl.NumeroDoc " +
                    "    LEFT JOIN coProyecto proy WITH(NOLOCK) on proy.ProyectoID = ctrl.ProyectoID " +
                    "    LEFT JOIN glLocFisica locFis WITH(NOLOCK) on locFis.LocFisicaID = orde.LugarEntrega and locFis.EmpresaGrupoID = orde.eg_glLocFisica " +
                    "    INNER JOIN prProveedor prov WITH (NOLOCK) ON prov.ProveedorID=orde.ProveedorID and prov.EmpresaGrupoID=orde.eg_prProveedor " +
                    "    LEFT JOIN inReferencia ref WITH (NOLOCK) ON ref.inReferenciaID=deta.inReferenciaID and ref.EmpresaGrupoID=deta.eg_inReferencia " +
                    "    LEFT join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = ref.EmpaqueInvID and empaque.EmpresaGrupoID = ref.eg_inEmpaque " +
                    "    INNER JOIN coTercero ter WITH (NOLOCK) ON ter.TerceroID=ctrl.TerceroID and ter.EmpresaGrupoID=ctrl.eg_coTercero " +
                    "    INNER JOIN glLugarGeografico lugarTer WITH(NOLOCK) ON lugarTer.LugarGeograficoID=ter.LugarGeograficoID and lugarTer.EmpresaGrupoID=ter.eg_glLugarGeografico " +
                    "    INNER JOIN glPais pais WITH(NOLOCK) on pais.PaisID=lugarTer.PaisID " +
                    " WHERE  " +
                    "    ctrl.NumeroDoc=@NumDoc ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@NumDoc", SqlDbType.Int);

                #endregion
                #region Valores Parametros

                mySqlCommandSel.Parameters["@NumDoc"].Value = NumDoc;

                #endregion

                DTO_ReportOrdenCompraDoc aux = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    aux = new DTO_ReportOrdenCompraDoc(dr);
                    if (!string.IsNullOrEmpty(dr["ConsProyMvto"].ToString()))
                        aux.ConsProyMvto.Value = Convert.ToInt32(dr["ConsProyMvto"]);
                    if (!string.IsNullOrEmpty(dr["NumDocProyMvto"].ToString()))
                        aux.NumDocProyMvto.Value = Convert.ToInt32(dr["NumDocProyMvto"]);
                    result.Add(aux);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_Solicitudes");
                throw exception;
            }
        }

        #endregion
    }
}


