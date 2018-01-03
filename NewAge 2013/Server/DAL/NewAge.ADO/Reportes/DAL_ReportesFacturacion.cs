
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
    /// DAL de DAL_Contabilidad
    /// </summary>
    public class DAL_ReportesFacturacion : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesFacturacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Carga la infomacion para generar la Pre-factura
        /// </summary>
        /// <param name="numDoc">Numero Doc de la factura que se genera</param>
        /// <param name="datosEmpresa">Lista de Datos de la empresa </param>
        /// <returns>Informacion de la factura</returns>
        public List<DTO_ReportFacturaVenta> DAL_ReportesFacturacion_FacturaVenta(string numDoc, DTO_coTercero dtoTerceroEmpr , bool isAprobada)
        {
            List<DTO_ReportFacturaVenta> result = new List<DTO_ReportFacturaVenta>();
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region filtros
                //Si es para aprobar saca la info de glMovimientoDetaPRE, si es aprobada glMovimientoDeta
                string deta = "";
                deta = isAprobada == true ? " glMovimientoDeta " : " glMovimientoDetaPRE ";
                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT movdeta.NroItem,movdeta.ImprimeInd,rTrim(clie.TerceroID) + '-' + terc.DigitoVerif as NITCliente,faDocu.ClienteID,clie.Descriptivo as ClienteDesc, clie.Responsable,clie.DireccionCom as DirComercialCli, " +
                    "        terc.Direccion as DirCliente, terc.Tel1 as TelCliente, terc.CECorporativo as CorreoCliente, ciudad.Descriptivo as CiudadCliente, ctrl.Observacion, " +
                    "        movDeta.ServicioID,movDeta.inReferenciaID,movDeta.DescripTExt as Producto,faDocu.NumeroDoc, RIGHT('000000' +  Ltrim(Rtrim(ctrl.DocumentoNro)),6)  as  DocumentoNro, ctrl.PrefijoID,   " +
                    "        ctrl.FechaDoc,faDocu.FechaVto,ctrl.ProyectoID,proy.Descriptivo as ProyectoDesc, movDeta.ValorUNI,movDeta.CantidadUNI,movDeta.Valor1LOC AS VlrBruto, ctrl.Iva Iva, (faDocu.Bruto + faDocu.Iva) as VlrTotal, " +
                    "        Isnull(faDocu.PorcRteGarantia,0)as PorcRteGarantia,IsNull(faDocu.PorcAnticipo,0) as PorcAnticipo,faDocu.Retencion1,faDocu.Retencion2,faDocu.Retencion3,faDocu.Retencion4,faDocu.Retencion5,faDocu.Retencion6,faDocu.Retencion7," +
                    "        faDocu.Retencion8,faDocu.Retencion9,faDocu.Retencion10,faDocu.FormaPago, faDocu.DatoAdd1,faDocu.DatoAdd2,faDocu.DatoAdd3,faDocu.DatoAdd4,faDocu.DatoAdd5,faDocu.Administracion,faDocu.Imprevistos,faDocu.Utilidad " +
                    " FROM " + deta +" AS movDeta WITH(NOLOCK) " +
                    " INNER JOIN faFacturaDocu AS faDocu WITH(NOLOCK)  ON  movDeta.NumeroDoc = faDocu.NumeroDoc " +
                    " INNER JOIN faCliente AS clie WITH(NOLOCK)  ON (clie.ClienteID = faDocu.ClienteID AND clie.EmpresaGrupoID = faDocu.eg_faCliente) " +
                    " INNER JOIN coTercero AS terc  WITH(NOLOCK)  ON (terc.TerceroID = clie.TerceroID AND terc.EmpresaGrupoID = clie.eg_coTercero) " +
                    " LEFT JOIN glLugarGeografico AS ciudad WITH(NOLOCK)  ON  (ciudad.LugarGeograficoID = terc.LugarGeograficoID and  ciudad.EmpresaGrupoID = terc.eg_glLugarGeografico) "+
	                " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK)  ON ctrl.NumeroDoc = faDocu.NumeroDoc "+
                    " LEFT JOIN coProyecto AS proy WITH(NOLOCK)  ON  (proy.ProyectoID = ctrl.ProyectoID and  proy.EmpresaGrupoID = ctrl.eg_coProyecto) " + 
                    " WHERE faDocu.EmpresaID  = @EmpresaID "+
	                    " AND faDocu.NumeroDoc = @NumeroDoc ";

                #endregion 

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                DTO_ReportFacturaVenta facVenta = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    facVenta = new DTO_ReportFacturaVenta(dr);
                    facVenta.NitEmpresa.Value = dtoTerceroEmpr.ID.Value;
                    facVenta.DireccionEmpresa.Value = dtoTerceroEmpr.Direccion.Value;
                    facVenta.TelefonoEmpresa.Value = dtoTerceroEmpr.Tel1.Value;
                    facVenta.CiudadEmpresa.Value = dtoTerceroEmpr.LugarGeoDesc.Value;
                    result.Add(facVenta);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesFacturacion_FacturaVenta");
                throw exception;
            }
 
        }

        /// <summary>
        /// Carga la infomacion para generar las facturas seleccionadas
        /// </summary>
        /// <param name="numDocs">Numeros Doc de la facturas que se generan</param>
        /// <param name="datosEmpresa">Lista de Datos de la empresa </param>
        /// <returns>Informacion de la factura</returns>
        public List<DTO_ReportFacturaVenta> DAL_ReportesFacturacion_FacturaVentaMasivo(string prefijoID, int docNroIni, int docNroFin, List<string> datosEmpresa, bool isAprobada)
        {
            List<DTO_ReportFacturaVenta> result = new List<DTO_ReportFacturaVenta>();
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                //#region filtros
                ////Si es para aprobar saca la info de glMovimientoDetaPRE, si es aprobada glMovimientoDeta
                //string deta = "";
                //deta = isAprobada == true ? " glMovimientoDeta " : " glMovimientoDetaPRE ";
                //#endregion
                //#region CommanText

                //mySqlCommandSel.CommandText =
                //    " SELECT movdeta.NroItem,movdeta.ImprimeInd,movDeta.DescripTExt as Producto, faDocu.NumeroDoc, faDocu.ClienteID,clie.TerceroID as NITCliente,terc.Descriptivo as ClienteDesc, " +
                //        " terc.Direccion, terc.Tel1 as TelCliente, terc.CECorporativo as Correo, ciudad.Descriptivo as Ciudad, " +
                //        " RIGHT('000000' + Ltrim(Rtrim(ctrl.DocumentoNro)),6)  as  DocumentoNro, " +
                //        " ctrl.FechaDoc, movDeta.Valor1LOC AS vlrBruto, ctrl.Iva Iva, (faDocu.Valor + faDocu.Iva) as Total " +
                //    " FROM " + deta + " AS movDeta WITH(NOLOCK) " +
                //        " INNER JOIN faFacturaDocu AS faDocu WITH(NOLOCK)  ON  movDeta.NumeroDoc = faDocu.NumeroDoc " +
                //        " INNER JOIN faCliente AS clie WITH(NOLOCK)  ON (clie.ClienteID = faDocu.ClienteID AND clie.EmpresaGrupoID = faDocu.eg_faCliente) " +
                //        " INNER JOIN coTercero AS terc  WITH(NOLOCK)  ON (terc.TerceroID = clie.TerceroID AND terc.EmpresaGrupoID = clie.eg_coTercero) " +
                //        " INNER JOIN glLugarGeografico AS ciudad WITH(NOLOCK)  ON  (ciudad.LugarGeograficoID = terc.LugarGeograficoID and " +
                //            " ciudad.EmpresaGrupoID = terc.eg_glLugarGeografico) " +
                //        " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK)  ON ctrl.NumeroDoc = faDocu.NumeroDoc " +
                //    " WHERE faDocu.EmpresaID = @EmpresaID  and ctrl.PrefijoID = @PrefijoID and (ctrl.DocumentoNro between @DocNroIni and @DocNroFin)" +

                //#endregion

                //mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@DocNroIni", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@DocNroFin", SqlDbType.Int);

                //mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@PrefijoID"].Value = prefijoID;
                //mySqlCommandSel.Parameters["@DocNroIni"].Value = docNroIni;
                //mySqlCommandSel.Parameters["@DocNroFin"].Value = docNroFin;

                //DTO_ReportFacturaVenta facVenta = null;
                //SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                //while (dr.Read())
                //{
                //    facVenta = new DTO_ReportFacturaVenta(dr);
                //    facVenta.NitEmpresa.Value = datosEmpresa[0];
                //    facVenta.DireccionEmpresa.Value = datosEmpresa[1];
                //    facVenta.TelefonoEmpresa.Value = datosEmpresa[2];
                //    result.Add(facVenta);
                //}
                //dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesFacturacion_FacturaVenta");
                throw exception;
            }

        }

        /// <summary>
        /// Carga la lista con las cuentas por cobrar
        /// </summary>
        /// <param name="fechaCorte">Fecha de corte para revisar las cuentas por cobrar</param>
        /// <param name="tercero">Si se quiere filtrar por un tercero en especifico</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportCxCPorEdades> DAL_ReportesFacturacion_CxCPorEdades(DateTime fechaCorte, string tercero, bool isDetallada)
        {
            try
            {
                List<DTO_ReportCxCPorEdades> results = new List<DTO_ReportCxCPorEdades>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Validación del filtro

                string terceroFil = "";
                if (!string.IsNullOrWhiteSpace(tercero))
                    terceroFil = " AND ctrl.TerceroID = " + "'" + tercero + " '";

                #endregion

                #region CommanText
                if (isDetallada)
                {
                    #region Carga consulta para Reporte Detallado

                    mySqlCommandSel.CommandText =
                                        " SELECT Detalle.Factura, Detalle.FechaPtoPago, Detalle.FechaVto,  Detalle.CuentaID, TerceroID, Descriptivo,Valor, DifDia, " +
                                            " No_Vencidas = CASE WHEN DifDia <= 0 then No_Vencidas else 0 end,  " +
                        //" No_Vencidas = CASE WHEN DifDia <= 0 then 'No Vencidas' end, "+  // Se comentarea para que salga los valores de la facturas vencida y no el texto no vencidas odgr
	                                        " Treinta = CASE WHEN DifDia BETWEEN  1 AND 30 then No_Vencidas else 0 end, " +
	                                        " Sesenta = CASE WHEN DifDia BETWEEN  31 AND 60 then No_Vencidas else 0 end, " +
	                                        " Noventa = CASE WHEN DifDia BETWEEN 61 AND 90 then No_Vencidas else 0 end , " +
	                                        " COchenta = CASE WHEN DifDia BETWEEN 91 AND 180 then No_Vencidas else 00 end,  " +
	                                        " MasCOchenta = CASE WHEN DifDia > 181 then No_Vencidas else 0 end  FROM   " +
     		                                         " (   " +
                                                     " SELECT faDocu.NumeroDoc, rtrim(ctrl.PrefijoID)+'-'+cast(ctrl.DocumentoNro as varchar(10))  as Factura, faDocu.FechaPtoPago, faDocu.FechaVto,      " +
			                                             " saldo.CuentaID, sum((DbOrigenLocML + DbOrigenExtML +  CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +    " +  
			                                             " CrSaldoIniLocML + CrSaldoIniExtML)) as No_Vencidas, tercero.TerceroID, tercero.Descriptivo , faDocu.Valor, saldo.PeriodoID ,  " +
			                                             " (DATEDIFF (DAY, @FechaCorte, FechaVto))  " + 
			                                             " AS DifDia,  " +   
			                                             " 0 as 'Treinta',  " +  
			                                             " 0 as 'Sesenta',  " +  
			                                             " 0 as 'Noventa', " +  
			                                             " 0 as 'CVeinte', " +   
			                                             " 0 as 'CCincuenta',   " + 
			                                             " 0 as 'COchenta', 0 as 'MasCOchenta'  " + 
			                                         " FROM faFacturaDocu faDocu   WITH(NOLOCK)  " +
			                                             " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on (ctrl.NumeroDoc = faDocu.NumeroDoc )  " +
                                                         " INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on (saldo.IdentificadorTR = ctrl.NumeroDoc) and saldo.PeriodoID =@FechaCorte" +
                                                         " INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID  and tercero.EmpresaGrupoID=ctrl.EmpresaID" +    
			                                         " WHERE  " +  
			                                             " (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2) " +    
			                                             " AND faDocu.EmpresaID = @EmpresaID " +
                                                         " AND YEAR(PeriodoID) = @Año " +
		                                                 " AND MONTH( saldo.PeriodoID) =  @Mes " +
                                                             terceroFil +
                                                         " group by faDocu.NumeroDoc,ctrl.PrefijoID,ctrl.DocumentoNro ,faDocu.FechaPtoPago, faDocu.FechaVto,saldo.CuentaID, " +
                                                         " tercero.TerceroID, tercero.Descriptivo , faDocu.Valor,saldo.PeriodoID , "+
                                                         " (DATEDIFF (DAY, @FechaCorte, FechaVto))" +// se agrega para resumir  por factura odgr
                                     " ) AS Detalle "+
                                     "where  No_Vencidas<>0    order by Descriptivo";  // se agrega para filtrar saldos en 0 odgr
                    #endregion
                }
                else
                {
                    #region Carga consulta para  Reporte Resumido

                    mySqlCommandSel.CommandText =

                        " SELECT *, (No_Vencidas+Treinta +Sesenta +Noventa+COchenta+MasCOchenta) AS total " + 
                        " FROM  " + 
                        " ( " + 
                        " SELECT  TerceroID, Descriptivo, " +
                        //"  No_Vencidas = CASE WHEN DifDia <= 0 then 'No Vencidas'  else ''end,  " +  // Se comentarea para que salga los valores de la facturas vencida y no el texto no vencidas odgr
                            "  No_Vencidas = SUM(CASE WHEN DifDia <= 0 then No_Vencidas  else 0 end),  " + 
	                        " Treinta = SUM(CASE WHEN DifDia BETWEEN  1 AND 30 then No_Vencidas else 0 end), " + 
	                        " Sesenta = SUM(CASE WHEN DifDia BETWEEN  31 AND 60 then No_Vencidas else 0 end), " + 
	                        " Noventa = SUM(CASE WHEN DifDia BETWEEN 61 AND 90 then No_Vencidas else 0 end), " + 
	                        " COchenta = SUM(CASE WHEN DifDia BETWEEN 91 AND 180 then No_Vencidas else 0 end), " +  
	                        " MasCOchenta = SUM(CASE WHEN DifDia > 181 then No_Vencidas else 0 end) " + 
                        " FROM    " +    
                        " (     " +
                        " SELECT faDocu.NumeroDoc as Factura, faDocu.FechaPtoPago, faDocu.FechaVto, " +      
		                        " saldo.CuentaID, ((DbOrigenLocML + DbOrigenExtML +  CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +     
		                        " CrSaldoIniLocML + CrSaldoIniExtML) )as No_Vencidas, tercero.TerceroID, tercero.Descriptivo ,  " +  
	                        " (DATEDIFF (DAY,@FechaCorte, FechaVto)) " +   
	                         " AS DifDia,  " +    
			                        "  0 as 'Treinta', " +    
			                        " 0 as 'Sesenta', " +    
			                         " 0 as 'Noventa', " +   
			                         " 0 as 'CVeinte',  " +   
			                         " 0 as 'CCincuenta', " +    
			                         " 0 as 'COchenta', 0 as 'MasCOchenta'  " +  
	                        " FROM faFacturaDocu faDocu   WITH(NOLOCK)  " + 
			                         " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on (ctrl.NumeroDoc = faDocu.NumeroDoc ) " +
                                     " INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on (saldo.IdentificadorTR = ctrl.NumeroDoc ) " +  
				                         " AND DATEPART(MONTH, saldo.PeriodoID) = @Mes AND DATEPART(YEAR, SALDO.PeriodoID) =@Año " +
                                     " INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID   and tercero.EmpresaGrupoID=ctrl.EmpresaID   " +     
	                        " WHERE    " + 
		                        " (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2)   " +   
		                        " AND faDocu.EmpresaID = @EmpresaID  " +
                                    terceroFil +                      
		                        " ) AS Detalle " +
//                        " GROUP BY Descriptivo, Detalle.TerceroID, Detalle.DifDia) as consulta ";// Se comentarea para que quede en un solo registro 
                    " where (No_Vencidas+Treinta +Sesenta +Noventa+COchenta+MasCOchenta) <>0 " + // Se filtran los valores en 0 odgr
                    " GROUP BY Descriptivo, Detalle.TerceroID) as consulta  order by Descriptivo"; 

                    #endregion
                }
                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);

                #endregion

                #region Asignacion de valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.FacturaVenta;
                mySqlCommandSel.Parameters["@documentId2"].Value = AppDocuments.NotaCredito;
                mySqlCommandSel.Parameters["@Mes"].Value = fechaCorte.Month;
                mySqlCommandSel.Parameters["@Año"].Value = fechaCorte.Year;
                #endregion

                DTO_ReportCxCPorEdades doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportCxCPorEdades(dr, isDetallada);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesFacturacion_CxCPOrEdades");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el DTO de libros de ventas de Facturacion
        /// </summary>
        /// <param name="periodo">Fecha en con la que se desea consultar la facura</param>
        /// <param name="diaFinal">Dia limite de la consulta</param>
        /// <param name="cliente">Filtro por para ver un cliente en Especifico</param>
        /// <param name="prefijo">Filtra  los Prefijo de la facura</param>
        /// <param name="NroFactura">Filtra por una factura en especila</param>
        /// <returns></returns>
        public List<DTO_ReportLibroVentas> DAL_ReportesFacturacion_LibroVentas(DateTime periodo, int MesFinal, string cliente, string prefijo, string NroFactura)
        {
            try
            {
                List<DTO_ReportLibroVentas> result = new List<DTO_ReportLibroVentas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string cli = "", pref = "", factura = "";

                //if (!string.IsNullOrEmpty(cliente))
                //    cli = " AND faDocu.ClienteID = @ClienteID ";
                if (!string.IsNullOrEmpty(prefijo))
                    pref = " AND ctrl.PrefijoID = @Prefijo ";
                if (!string.IsNullOrEmpty(NroFactura))
                    factura = " AND ctrl.DocumentoNro = @NroFactura ";

                #endregion

                #region CommanText

                mySqlCommandSel.CommandText =
                    " SELECT ctrl.DocumentoNro, ctrl .PrefijoID, RTRIM(ctrl.PrefijoID) + '' + '-' + ''+CAST(ctrl.DocumentoNro as CHAR) as factura, " +
                        " ctrl.FechaDoc  ,faDocu.ClienteID, cli.Descriptivo as NombreCliente,Bruto, faDocu.Iva, " +
                        " (Bruto + faDocu.Iva) as vlrTotal " +
                    " FROM faFacturaDocu as faDocu " +
                        " INNER JOIN glDocumentoControl AS ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = faDocu.NumeroDoc " +
                        " INNER JOIN faCliente AS cli WITH(NOLOCK)	on (cli.ClienteID = faDocu.ClienteID and cli.EmpresaGrupoID = faDocu.eg_faCliente) " +
                    " where faDocu.EmpresaID = @EmpresaID " +
                        " AND DATEPART(YEAR, ctrl.FechaDoc) = @Año " +
                        " AND DATEPART(MONTH, ctrl.FechaDoc) = @MesInicial " +
                        " AND DATEPART(MONTH, ctrl.FechaDoc)<= @MesFinal " +
                        cli + pref + factura;
                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MesInicial", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MesFinal", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@Prefijo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@NroFactura", SqlDbType.Char);

                #endregion

                #region Asignacion de Valores A Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value; 
                    mySqlCommandSel.Parameters["@Año"].Value = periodo.Year;
                    mySqlCommandSel.Parameters["@MesInicial"].Value = periodo.Month;
                    mySqlCommandSel.Parameters["@MesFinal"].Value = MesFinal;
                    //mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Prefijo"].Value = prefijo;
                    mySqlCommandSel.Parameters["@NroFactura"].Value = NroFactura;

                #endregion

                    DTO_ReportLibroVentas libroVent = null;
                    SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                    while (dr.Read())
                    {
                        libroVent = new DTO_ReportLibroVentas(dr);
                        result.Add(libroVent);
                    }
                    dr.Close();

                    return result;

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesFacturacion_LibroVentas");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que retorna el detalle de los cheques de acuerdo a los filtros
        /// </summary>
        /// <param name="bancoID">Banco </param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_ReportCuentasPorCobrar> DAL_ReportesFacturacion_CuentasPorCobrar(string Tercero, int Moneda, string Cuenta, DateTime fecha)
        {
            string terceroFil = "";
            string cuenta = "";
            string moneda = "";
            if (!string.IsNullOrWhiteSpace(Tercero))
                terceroFil = "  AND  ter.TerceroID  = @Tercero ";
            if (!string.IsNullOrWhiteSpace(Cuenta))
                cuenta = " AND sdo.CuentaID=@Cuenta ";
            if (Moneda == 1 || Moneda == 2)
                moneda = " AND cuenta.OrigenMonetario=@Origen ";
            if (Moneda == 3)
                moneda = " AND (cuenta.OrigenMonetario=@Origen OR cuenta.OrigenMonetario=@Origen2) ";
            try
            {
                List<DTO_ReportCuentasPorCobrar> results = new List<DTO_ReportCuentasPorCobrar>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommandSel.CommandText =

                    " select doc.cuentaid,  doc.TerceroID, ter.Descriptivo," +
                    "   rtrim(doc.PrefijoID) + ' - ' + cast(DocumentoNro as varchar(6)) as Factura, doc.numerodoc," +
                    " 	RTRIM(doc.ComprobanteID) + ' - ' + CAST(doc.ComprobanteIDNro AS CHAR(5)) as Comprobante, " +
                    "   doc.FechaDoc as FacturaFecha, fac.FechaVto," +
                    "   doc.Observacion," +
                    "   fac.bruto as ValorBruto, doc.valor as ValorNeto," +
                    "   doc.valor - (DbSaldoIniLocML + CrSaldoIniExtML + DbOrigenExtML + DbOrigenLocML) as VlrAbono," +
                    "   cast(DbSaldoIniLocML + CrSaldoIniExtML + DbOrigenExtML + DbOrigenLocML as money) as SaldoTotal" +
                    " from cocuentasaldo sdo" +
                    "  left join glDocumentoControl doc on sdo.IdentificadorTR = doc.NumeroDoc" +
                    "  left join faFacturaDocu   fac on sdo.IdentificadorTR = fac.NumeroDoc" +
                    "  left join cotercero ter on doc.TerceroID = ter.TerceroID and doc.eg_coTercero = ter.EmpresaGrupoID" +
                    "  left JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=sdo.CuentaID and cuenta.EmpresaGrupoID=sdo.eg_coPlanCuenta" +                    
                    " where  sdo.periodoID = @fecha and doc.DocumentoID in('41','42','43','44')  and sdo.EmpresaID =  @EmpresaID and " +
                    " (sdo.DbSaldoIniLocML + sdo.CrSaldoIniExtML + sdo.DbOrigenExtML + sdo.DbOrigenLocML) > 0" +
                    terceroFil + cuenta + moneda +
                    " order by ter.descriptivo, doc.PrefijoID + ' - ' + cast(DocumentoNro as varchar(6)) ";

                    //" SELECT A.* FROM " +
                   //"          ( " +
                   //"          SELECT    Ctrl.TerceroID, coTercero.Descriptivo,  " +
                   //"                    saldo.CuentaID,  " +
                   //"                    Ctrl.Descripcion as CuentaDesc, " +
                   //"                    ctrl.Observacion,  " +
                   //"                    Ctrl.DocumentoTercero Factura, " +
                   //"                    Ctrl.FechaDoc as FacturaFecha, " +
                   //"                    fac.FechaVto, " +
                   //"                    RTRIM(ctrl.ComprobanteID) + ' - ' + CAST(ctrl.ComprobanteIDNro AS CHAR(5)) as Comprobante,  " +
                   //"                    CASE WHEN cuenta.OrigenMonetario=1 THEN 'LOC' " +
                   //"                         WHEN cuenta.OrigenMonetario=2 THEN 'EXT' " +
                   //"                         END AS MdaOrigen, " +
                   //"                    fac.Valor AS ValorBruto,Ctrl.Valor AS ValorNeto, " +
                   //"                    (Ctrl.Valor - ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML +  CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML +   " +
                   //"                    CrSaldoIniExtML)) as VlrAbono , " +
                   //"                    ABS(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                   //"                    CrSaldoIniLocML +  CrSaldoIniExtML) AS SaldoTotal, " +
                   //"                     ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE fac.Valor/cambio.TasaCambio END,2) AS ValorBrutoEXT , ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE Ctrl.Valor/cambio.TasaCambio END,2) AS ValorNetoEXT, " +
                   //"                     (ROUND(CASE WHEN cambio.TasaCambio=0 OR cambio.TasaCambio=NULL THEN 0 ELSE Ctrl.Valor/cambio.TasaCambio END,2) - ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME +  CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +  " + 
                   ////"                  fac.Valor*Ctrl.TasaCambioDOCU AS ValorBrutoEXT , Ctrl.Valor*Ctrl.TasaCambioDOCU AS ValorNetoEXT, " +
                   ////"                  ((fac.Valor*Ctrl.TasaCambioDOCU) - ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME +  CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +   " +
                   ////"                    fac.Valor AS ValorNetoEXT , Ctrl.Valor AS ValorBrutoEXT, " +
                   ////"                    ((fac.Valor) - ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME +  CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME + CrSaldoIniLocME +   " +
                   //"                    CrSaldoIniExtME)) as VlrAbonoEXT, " +
                   //"                    ABS(DbOrigenLocME + DbOrigenExtME + CrOrigenLocME + CrOrigenExtME +  DbSaldoIniLocME + DbSaldoIniExtME +  " +
                   //"                    CrSaldoIniLocME +  CrSaldoIniExtME) AS SaldoTotalEXT " +
                   //"          FROM coCuentaSaldo saldo WITH(NOLOCK) " +
                   //"            INNER JOIN faFacturaDocu  fac WITH(NOLOCK) ON fac.NumeroDoc = saldo.IdentificadorTR   " +
                   //"            INNER JOIN glDocumentoControl Ctrl WITH(NOLOCK) ON Ctrl.DocumentoID = @documentId   AND Ctrl.NumeroDoc = saldo.IdentificadorTR    " +
                   //"            INNER JOIN coTercero WITH(NOLOCK) ON coTercero.TerceroID = Ctrl.TerceroID and coTercero.EmpresaGrupoID=ctrl.eg_coTercero " +
                   //"            INNER JOIN coPlanCuenta cuenta  WITH(NOLOCK) ON cuenta.CuentaID=saldo.CuentaID and cuenta.EmpresaGrupoID=saldo.eg_coPlanCuenta " +
                   //"            left JOIN glTasaCambio cambio WITH(NOLOCK) ON cambio.Fecha=@Fecha and cambio.EmpresaGrupoID=@EmpresaID " +
                   //"          WHERE   saldo.EmpresaID =  @EmpresaID " +
                   //"            AND saldo.PeriodoID <=  @Fecha   " +
                   //terceroFil + cuenta + moneda +
                   //"          ) AS  A " +
                   //" WHERE round(A.SaldoTotal,2) > 0";
                #endregion

                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Tercero", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Origen", SqlDbType.TinyInt);
                if (Moneda == 3)
                {
                    mySqlCommandSel.Parameters.Add("@Origen2", SqlDbType.TinyInt);
                }
                mySqlCommandSel.Parameters.Add("@Cuenta", SqlDbType.Char, UDT_CuentaID.MaxLength);
                #endregion

                #region Asignacion de Paramtros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = fecha;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.FacturaVenta;
                mySqlCommandSel.Parameters["@Tercero"].Value = Tercero;
                mySqlCommandSel.Parameters["@Origen"].Value = Moneda;
                if (Moneda == 3)
                {
                    mySqlCommandSel.Parameters["@Origen"].Value = Moneda - 1;
                    mySqlCommandSel.Parameters["@Origen2"].Value = Moneda - 2;
                }

                mySqlCommandSel.Parameters["@Cuenta"].Value = Cuenta;
                #endregion

                DTO_ReportCuentasPorCobrar doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ReportCuentasPorCobrar(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCuentasPorCobrar_CuentasPorPagar");
                throw exception;
            }
        }

        public DataTable DAL_Reportes_CC_CxCToExcel(int documentoID, byte? tipoReporte, DateTime fechaIni, DateTime? fechaFin, string tercero,bool isDetallada)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();
                string where = string.Empty;

                #region Validación del filtro

                string terceroFil = "";
                if (!string.IsNullOrWhiteSpace(tercero))
                    terceroFil = " AND ctrl.TerceroID = " + "'" + tercero + " '";

                #endregion

                #region CommanText
                if (documentoID == AppReports.faCxCxEdades)
                {
                    if (tipoReporte == 1) //Detallado)
                    {
                        #region Carga consulta para Reporte Detallado

                        mySqlCommandSel.CommandText =
                                            " SELECT Detalle.Factura, Detalle.FechaPtoPago, Detalle.FechaVto,  Detalle.CuentaID, TerceroID, Descriptivo,Valor, DifDia, " +
                                                " No_Vencidas = CASE WHEN DifDia <= 0 then No_Vencidas else 0 end,  " +
                            //" No_Vencidas = CASE WHEN DifDia <= 0 then 'No Vencidas' end, "+  // Se comentarea para que salga los valores de la facturas vencida y no el texto no vencidas odgr
                                                " Treinta = CASE WHEN DifDia BETWEEN  1 AND 30 then No_Vencidas else 0 end, " +
                                                " Sesenta = CASE WHEN DifDia BETWEEN  31 AND 60 then No_Vencidas else 0 end, " +
                                                " Noventa = CASE WHEN DifDia BETWEEN 61 AND 90 then No_Vencidas else 0 end , " +
                                                " COchenta = CASE WHEN DifDia BETWEEN 91 AND 180 then No_Vencidas else 00 end,  " +
                                                " MasCOchenta = CASE WHEN DifDia > 181 then No_Vencidas else 0 end  FROM   " +
                                                         " (   " +
                                                         " SELECT faDocu.NumeroDoc, rtrim(ctrl.PrefijoID)+'-'+cast(ctrl.DocumentoNro as varchar(10))  as Factura, faDocu.FechaPtoPago, faDocu.FechaVto,      " +
                                                             " saldo.CuentaID, sum((DbOrigenLocML + DbOrigenExtML +  CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +    " +
                                                             " CrSaldoIniLocML + CrSaldoIniExtML)) as No_Vencidas, tercero.TerceroID, tercero.Descriptivo , faDocu.Valor, saldo.PeriodoID ,  " +
                                                             " (DATEDIFF (DAY, @FechaCorte, FechaVto))  " +
                                                             " AS DifDia,  " +
                                                             " 0 as 'Treinta',  " +
                                                             " 0 as 'Sesenta',  " +
                                                             " 0 as 'Noventa', " +
                                                             " 0 as 'CVeinte', " +
                                                             " 0 as 'CCincuenta',   " +
                                                             " 0 as 'COchenta', 0 as 'MasCOchenta'  " +
                                                         " FROM faFacturaDocu faDocu   WITH(NOLOCK)  " +
                                                             " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on (ctrl.NumeroDoc = faDocu.NumeroDoc )  " +
                                                             " INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on (saldo.IdentificadorTR = ctrl.NumeroDoc) and saldo.PeriodoID =@FechaCorte" +
                                                             " INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID  and tercero.EmpresaGrupoID=ctrl.EmpresaID" +
                                                         " WHERE  " +
                                                             " (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2) " +
                                                             " AND faDocu.EmpresaID = @EmpresaID " +
                                                             " AND YEAR(PeriodoID) = @Año " +
                                                             " AND MONTH( saldo.PeriodoID) =  @Mes " +
                                                                 terceroFil +
                                                             " group by faDocu.NumeroDoc,ctrl.PrefijoID,ctrl.DocumentoNro ,faDocu.FechaPtoPago, faDocu.FechaVto,saldo.CuentaID, " +
                                                             " tercero.TerceroID, tercero.Descriptivo , faDocu.Valor,saldo.PeriodoID , " +
                                                             " (DATEDIFF (DAY, @FechaCorte, FechaVto))" +// se agrega para resumir  por factura odgr
                                         " ) AS Detalle " +
                                         "where  No_Vencidas<>0    order by Descriptivo";  // se agrega para filtrar saldos en 0 odgr
                        #endregion
                    }
                    else
                    {
                        #region Carga consulta para  Reporte Resumido

                        mySqlCommandSel.CommandText =

                            " SELECT *, (No_Vencidas+Treinta +Sesenta +Noventa+COchenta+MasCOchenta) AS total " +
                            " FROM  " +
                            " ( " +
                            " SELECT  TerceroID, Descriptivo, " +
                            //"  No_Vencidas = CASE WHEN DifDia <= 0 then 'No Vencidas'  else ''end,  " +  // Se comentarea para que salga los valores de la facturas vencida y no el texto no vencidas odgr
                                "  No_Vencidas = SUM(CASE WHEN DifDia <= 0 then No_Vencidas  else 0 end),  " +
                                " Treinta = SUM(CASE WHEN DifDia BETWEEN  1 AND 30 then No_Vencidas else 0 end), " +
                                " Sesenta = SUM(CASE WHEN DifDia BETWEEN  31 AND 60 then No_Vencidas else 0 end), " +
                                " Noventa = SUM(CASE WHEN DifDia BETWEEN 61 AND 90 then No_Vencidas else 0 end), " +
                                " COchenta = SUM(CASE WHEN DifDia BETWEEN 91 AND 180 then No_Vencidas else 0 end), " +
                                " MasCOchenta = SUM(CASE WHEN DifDia > 181 then No_Vencidas else 0 end) " +
                            " FROM    " +
                            " (     " +
                            " SELECT faDocu.NumeroDoc as Factura, faDocu.FechaPtoPago, faDocu.FechaVto, " +
                                    " saldo.CuentaID, ((DbOrigenLocML + DbOrigenExtML +  CrOrigenLocML + CrOrigenExtML +  DbSaldoIniLocML + DbSaldoIniExtML +  " +
                                    " CrSaldoIniLocML + CrSaldoIniExtML) )as No_Vencidas, tercero.TerceroID, tercero.Descriptivo ,  " +
                                " (DATEDIFF (DAY,@FechaCorte, FechaVto)) " +
                                 " AS DifDia,  " +
                                        "  0 as 'Treinta', " +
                                        " 0 as 'Sesenta', " +
                                         " 0 as 'Noventa', " +
                                         " 0 as 'CVeinte',  " +
                                         " 0 as 'CCincuenta', " +
                                         " 0 as 'COchenta', 0 as 'MasCOchenta'  " +
                                " FROM faFacturaDocu faDocu   WITH(NOLOCK)  " +
                                         " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK)  on (ctrl.NumeroDoc = faDocu.NumeroDoc ) " +
                                         " INNER JOIN coCuentaSaldo saldo WITH(NOLOCK)  on (saldo.IdentificadorTR = ctrl.NumeroDoc ) " +
                                             " AND DATEPART(MONTH, saldo.PeriodoID) = @Mes AND DATEPART(YEAR, SALDO.PeriodoID) =@Año " +
                                         " INNER JOIN coTercero tercero WITH(NOLOCK)  on tercero.TerceroID = ctrl.TerceroID   and tercero.EmpresaGrupoID=ctrl.EmpresaID   " +
                                " WHERE    " +
                                    " (ctrl.DocumentoID = @documentId or ctrl.DocumentoID = @documentId2)   " +
                                    " AND faDocu.EmpresaID = @EmpresaID  " +
                                        terceroFil +
                                    " ) AS Detalle " +
                            //                        " GROUP BY Descriptivo, Detalle.TerceroID, Detalle.DifDia) as consulta ";// Se comentarea para que quede en un solo registro 
                        " where (No_Vencidas+Treinta +Sesenta +Noventa+COchenta+MasCOchenta) <>0 " + // Se filtran los valores en 0 odgr
                        " GROUP BY Descriptivo, Detalle.TerceroID) as consulta  order by Descriptivo";

                        #endregion
                    }
                }
                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId2", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);

                #endregion

                #region Asignacion de valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaIni;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.FacturaVenta;
                mySqlCommandSel.Parameters["@documentId2"].Value = AppDocuments.NotaCredito;
                mySqlCommandSel.Parameters["@Mes"].Value = fechaIni.Month;
                mySqlCommandSel.Parameters["@Año"].Value = fechaIni.Year;
                #endregion

                sda.SelectCommand = mySqlCommandSel;
                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Reportes_Cp_CxPToExcel");
                return null;
            }
        }


    }
}


