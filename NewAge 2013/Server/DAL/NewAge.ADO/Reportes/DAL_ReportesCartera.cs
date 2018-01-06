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
    public class DAL_ReportesCartera : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_ReportesCartera(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Reportes ccAseguradora
        /// <summary>
        /// Funcion que consulta la info para llenar el dto_ccAseguradora
        /// </summary>
        /// <param name="fechaIni">Fecha Inicial del Reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderName">Es par aordenar por nombre</param>
        /// <param name="filter">Filtro</param>
        /// <returns>Lista de DTOccAseguradora</returns>
        public List<DTO_ccAseguradoraReport> DAL_ReportesCartera_Cc_Aseguradora(DateTime fechaIni, DateTime fechaFin, bool orderName, string filter)
        {
            try
            {
                List<DTO_ccAseguradoraReport> results = new List<DTO_ccAseguradoraReport>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = "";
                if (filter != null)
                    filtro = "AND ccCreditoDocu.ClienteID = " + filter;
                mySqlCommandSel.CommandText =
                                    " SELECT ccCreditoDocu.ClienteID, ccCliente.Descriptivo, ccCliente.FechaNacimiento,	" +
                                        " sum(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoMLoc ,	" +
                                        " ccCreditoDocu.Libranza, ccCreditoDocu.FechaLiquida, ccPagaduria.Descriptivo as PagaduriaDesc	" +
                                    " FROM ccCreditoDocu	with(nolock) " +
                                        " INNER JOIN ccCliente ON ccCliente.ClienteID = ccCreditoDocu.ClienteID	" +
                                        " INNER JOIN coCuentaSaldo ON coCuentaSaldo.IdentificadorTR = ccCreditoDocu.NumeroDoc	" +
                                        " INNER JOIN ccPagaduria ON ccPagaduria.PagaduriaID = ccCreditoDocu.PagaduriaID	" +
                                    " WHERE ccCreditoDocu.FechaLiquida BETWEEN  @fechaIni AND @fechaFin	" +
                                        " AND ccCreditoDocu.EmpresaID = @EmpresaID " +
                                        filtro +
                                    " GROUP BY ccCreditoDocu.ClienteID, ccCliente.Descriptivo, ccCliente.FechaNacimiento,		" +
                                        " ccCreditoDocu.Libranza, ccCreditoDocu.FechaLiquida, ccPagaduria.Descriptivo, ccCreditoDocu.LineaCreditoID ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;

                DTO_ccAseguradoraReport doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccAseguradoraReport(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccAseguradoraReport");
                throw exception;
            }
        }
        #endregion

        #region Reportes ccCesion

        /// <summary>
        /// Carga el DTO de Cesion de Cartera
        /// </summary>
        /// <param name="numeroDoc">Numero doc que se usa para usar la consuklta</param>
        /// <returns>List DTO</returns>
        public List<DTO_ccCesionCartera> DAL_ReportesCartera_cc_Cesion(int numeroDoc, bool isCesion, Dictionary<int, string> autoriza)
        {
            try
            {
                List<DTO_ccCesionCartera> results = new List<DTO_ccCesionCartera>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                if (isCesion)
                {
                    #region CommanText Reporte Cesion Cartera
                    mySqlCommandSel.CommandText =
                        " SELECT   docu.CompradorCarteraID,  compCartera.TerceroID as TerceroIDComprador, tercero.Descriptivo as NombreComprador , /* Hasta aca Datos */ " +
                        "         docu.Libranza, docu.VlrLibranza as VlrNominal, docu.VlrPrestamo as VlrCredito, docu.VlrCuota, docu.Plazo,     " +
                        "         docu.FechaLiquida as fechaLiquida,	docu.PagaduriaID, docu.LineaCreditoID, docu.FechaVto ,  /* Hasta aca Datos del Credito */  " +
                        "         ventaDocu.Oferta,ventaDeta.Portafolio,  ventaDocu.TasaDescuento as TasaCesion,	ventaDocu.FechaAceptacion as FechaVenta, ventaDocu.FechaPago1 as FechaPrimerFlujo,   " +
                        "         ventaDeta.CuotaID as PrimeraCuotaCesion, ventaDeta.CuotasVend as NumeroCuotasCesion,	ventaDeta.VlrCuota as ValorCuotaCesion,   " +
                        "         ventaDeta.VlrLibranza as ValorTotalCesion,  ventaDeta.VlrVenta as ValorpagoCesion,  " +
                        "         (ventaDeta.VlrLibranza-ventaDeta.VlrVenta) as ValorMargenCesion, /* Hasta aca datos Secion  */  " +
                        "         docu.ClienteID CedulaDeudor,cliente.Descriptivo as NombreDeudor, " +
                        "         RTRIM(ctrl.ComprobanteID) +' - '+ CAST(ctrl.ComprobanteIDNro AS CHAR(5)) AS Comprobante " +
                        " from ccCreditoDocu docu WITH(NOLOCK)   " +
                        "         INNER JOIN ccCliente as cliente WITH(NOLOCK) ON cliente.ClienteID = docu.ClienteID and cliente.EmpresaGrupoID=docu.eg_ccCliente " +
                        "         INNER JOIN ccCompradorCartera as compCartera WITH(NOLOCK) ON compCartera.CompradorCarteraID = docu.CompradorCarteraID  and compCartera.EmpresaGrupoID=docu.eg_ccCompradorCartera " +
                        "         INNER JOIN ccVentaDeta as ventaDeta WITH(NOLOCK) ON ventaDeta.NumDocCredito = docu.NumeroDoc  " +
                        "         INNER JOIN ccVentaDocu as ventaDocu WITH(NOLOCK) ON ventaDocu.NumeroDoc = ventaDeta.NumeroDoc  " +
                        "         INNER JOIN coTercero  as tercero WITH(NOLOCK) ON tercero.TerceroID = compCartera.TerceroID and tercero.EmpresaGrupoID = compCartera.eg_coTercero  " +
                        "         INNER JOIN glDocumentoControl as ctrl WITH(NOLOCK) on ctrl.NumeroDoc=VentaDeta.NumeroDoc  " +
                        " where ventaDeta.NumeroDoc = @NumeroDoc  and docu.EmpresaID = @EmpresaID ";

                    // ¡¡¡¡¡ NO BORRAR !!!!!
                    //"SELECT docu.CompradorCarteraID,  compCartera.TerceroID as TerceroIDComprador, tercero.Descriptivo as NombreComprador , /* Hasta aca Datos */  " +
                    //    " docu.Libranza, docu.VlrLibranza as VlrNominal, docu.VlrPrestamo as VlrCredito, docu.VlrCuota, docu.Plazo,    " +
                    //    " docu.FechaLiquida as fechaLiquida,	docu.PagaduriaID, paga.Descriptivo as NombrePagaduria, docu.LineaCreditoID, docu.FechaVto ,  /* Hasta aca Datos del Credito */ " +
                    //    " ventaDocu.Oferta,ventaDeta.Portafolio,  ventaDeta.FactorCesion as TasaCesion,	ventaDocu.FechaAceptacion as FechaVenta, ventaDocu.FechaPago1 as FechaPrimerFlujo,  " +
                    //    " ventaDeta.CuotaID as PrimeraCuotaCesion, ventaDeta.CuotasVend as NumeroCuotasCesion,	ventaDeta.VlrCuota as ValorCuotaCesion,  " +
                    //    " ventaDeta.VlrLibranza as ValorTotalCesion,  ventaDeta.VlrVenta as ValorpagoCesion, " +
                    //    " (ventaDeta.VlrLibranza-ventaDeta.VlrVenta) as ValorMargenCesion, /* Hasta aca datos Secion  */ " +
                    //    " docu.ClienteID CedulaDeudor, cliente.Descriptivo as NombreDeudor, cliente.ResidenciaDir as DireccionDeudor, " +
                    //    " cliente.Telefono as Tel1Deudor, cliente.Cargo as cargoDeudor, profe.Descriptivo as  profesionDeudor, " +
                    //    " CASE WHEN(cliente.Sexo = 1) THEN 'Masculino'  " +
                    //        " WHEN(cliente.Sexo = 2) THEN 'Femenino' END as sexoDeudor, " +
                    //    " CASE WHEN(cliente.EstadoCivil = 0) THEN 'No Aplica'  " +
                    //        " WHEN(cliente.EstadoCivil = 1) THEN 'Soltero'  " +
                    //        " WHEN(cliente.EstadoCivil = 2) THEN 'Casado'  " +
                    //        " WHEN(cliente.EstadoCivil = 3) THEN 'Unión libre' " +
                    //        " WHEN(cliente.EstadoCivil = 4) THEN 'Separado'  " +
                    //        " WHEN(cliente.EstadoCivil = 5) THEN 'Divorciado' " +
                    //        " WHEN(cliente.EstadoCivil = 6) THEN 'Viudo' END AS estadoCivDeudor, " +
                    //    " cliente.VlrDevengado as VlrDevenDeudo, FLOOR(DATEDIFF(day,cliente.FechaNacimiento,GETDATE())/365.242199) as EdadDedudor, /*Hasta aca Datos Deudor */ " +
                    //    " soliDocu.Codeudor1 as CedulaCodeudor1, clienteCodeudor1.Descriptivo as NombreCodeudor1,  " +
                    //    " clienteCodeudor1.ResidenciaDir as DireccionCodeudor1,clienteCodeudor1.Telefono as TelefonoCodeudor1,clienteCodeudor1.Cargo as CargoCodeudor1, " +
                    //    " profeCodeudor1.Descriptivo as profesionCodeudor1, " +
                    //    " CASE WHEN(clienteCodeudor1.Sexo = 1) THEN 'Masculino'  " +
                    //        " WHEN(clienteCodeudor1.Sexo = 2)THEN 'Femenino' END as sexoCodeudor1, " +
                    //    " CASE WHEN(clienteCodeudor1.EstadoCivil = 0) THEN 'No Aplica'  " +
                    //        " WHEN(clienteCodeudor1.EstadoCivil = 1)  THEN 'Soltero' " +
                    //        " WHEN(clienteCodeudor1.EstadoCivil = 2) THEN 'Casado' " +
                    //        " WHEN(clienteCodeudor1.EstadoCivil = 3) THEN 'Unión libre' " +
                    //        " WHEN(clienteCodeudor1.EstadoCivil = 4) THEN 'Separado'  " +
                    //        " WHEN(clienteCodeudor1.EstadoCivil = 5) THEN 'Divorciado' " +
                    //        " WHEN(clienteCodeudor1.EstadoCivil = 6) THEN 'Viudo' END AS estadoCivCodeidor1, " +
                    //    " clienteCodeudor1.VlrDevengado as VlrDevenCodeudor1,FLOOR(DATEDIFF(day,clienteCodeudor1.FechaNacimiento,GETDATE())/365.242199) as EdadCodedudor1, " +
                    //    " soliDocu.Codeudor2 as CedulaCodeudor2,  clienteCodeudor2.Descriptivo as NombreCodeudor2,  " +
                    //    " clienteCodeudor2.ResidenciaDir as DireccionCodeudor2,clienteCodeudor2.Telefono as TelefonoCodeudor2, clienteCodeudor2.Cargo as CargoCodeudor2, " +
                    //    "  profeCodeudor2.Descriptivo as profesionCodeudor2, " +
                    //    " CASE WHEN(clienteCodeudor2.Sexo = 1) THEN 'Masculino'  " +
                    //        " WHEN(clienteCodeudor2.Sexo = 2) THEN 'Femenino' END as sexoCodeudor2, " +
                    //    " CASE WHEN(clienteCodeudor2.EstadoCivil = 0) THEN 'No Aplica' " +
                    //        " WHEN(clienteCodeudor2.EstadoCivil = 1)  THEN 'Soltero'  " +
                    //        " WHEN(clienteCodeudor2.EstadoCivil = 2)  THEN 'Casado'  " +
                    //        " WHEN(clienteCodeudor2.EstadoCivil = 3) THEN 'Unión libre'  " +
                    //        " WHEN(clienteCodeudor2.EstadoCivil = 4) THEN 'Separado'  " +
                    //        " WHEN(clienteCodeudor2.EstadoCivil = 5) THEN 'Divorciado' " +
                    //        " WHEN(clienteCodeudor2.EstadoCivil = 6) THEN 'Viudo' END AS estadoCivCodeidor2, " +
                    //    " clienteCodeudor2.VlrDevengado as  VlrDevenCodeudor2,FLOOR(DATEDIFF(day,clienteCodeudor2.FechaNacimiento,GETDATE())/365.242199) as EdadCodedudor2 " +
                    //" from ccCreditoDocu docu WITH(NOLOCK)  " +
                    //    " INNER JOIN ccCliente as cliente WITH(NOLOCK) ON cliente.ClienteID = docu.ClienteID " +
                    //    " LEFT JOIN ccProfesion as profe WITH(NOLOCK) ON  profe.ProfesionID = cliente.ProfesionID and profe.EmpresaGrupoID = cliente.eg_ccProfesion " +
                    //    " INNER JOIN ccCompradorCartera as compCartera WITH(NOLOCK) ON compCartera.CompradorCarteraID = docu.CompradorCarteraID  " +
                    //    " INNER JOIN ccSolicitudDocu as soliDocu  WITH(NOLOCK) ON soliDocu.NumeroDoc = docu.NumSolicitud " +
                    //    " INNER JOIN ccVentaDeta as ventaDeta WITH(NOLOCK) ON ventaDeta.NumDocCredito = docu.NumeroDoc " +
                    //    " INNER JOIN ccVentaDocu as ventaDocu WITH(NOLOCK) ON ventaDocu.NumeroDoc = ventaDeta.NumeroDoc " +
                    //    " INNER JOIN ccPagaduria as paga  WITH(NOLOCK) ON paga.PagaduriaID = docu.PagaduriaID and paga.EmpresaGrupoID = docu.eg_ccPagaduria	 " +
                    //    " INNER JOIN coTercero  as tercero WITH(NOLOCK) ON tercero.TerceroID = compCartera.TerceroID and tercero.EmpresaGrupoID = compCartera.eg_coTercero " +
                    //    " LEFT JOIN ccCliente  as clienteCodeudor1 WITH(NOLOCK) ON clienteCodeudor1.TerceroID = soliDocu.Codeudor1 and clienteCodeudor1.EmpresaGrupoID = soliDocu.ClienteID " +
                    //    " LEFT JOIN ccProfesion as profeCodeudor1 WITH(NOLOCK) ON profeCodeudor1.ProfesionID = clienteCodeudor1.ProfesionID and profeCodeudor1.EmpresaGrupoID = clienteCodeudor1.eg_ccProfesion " +
                    //    " LEFT JOIN ccCliente  as clienteCodeudor2 WITH(NOLOCK) ON clienteCodeudor2.TerceroID = soliDocu.Codeudor2 and clienteCodeudor2.EmpresaGrupoID = soliDocu.ClienteID " +
                    //    " LEFT JOIN ccProfesion as profeCodeudor2 WITH(NOLOCK) ON profeCodeudor2.ProfesionID = clienteCodeudor2.ProfesionID and profeCodeudor2.EmpresaGrupoID = clienteCodeudor2.eg_ccProfesion " +
                    //" where ventaDeta.NumeroDoc = @NumeroDoc  and docu.EmpresaID = @EmpresaID ";
                    #endregion
                }
                else
                {
                    #region CommanText Reporte Oferta

                    mySqlCommandSel.CommandText =
                        " select docu.Libranza,docu.ClienteID as Cedula,cliente.Descriptivo as NombreCliente, ventaDeta.CuotasVend,  ventaDeta.VlrLibranza as VlrNominal, " +
                                "  ventaDeta.VlrVenta as VlrOfertado, compCartera.Descriptivo  as NombreComprador ,ventaDocu.Oferta    " +
                        " from ccCreditoDocu as docu WITH(NOLOCK) " +
                                " INNER JOIN ccVentaDeta as ventaDeta WITH(NOLOCK) ON ventaDeta.NumDocCredito = docu.NumeroDoc " +
                                " INNER JOIN ccVentaDocu as ventaDocu WITH(NOLOCK) ON ventaDocu.NumeroDoc = ventaDeta.NumeroDoc " +
                                " INNER JOIN ccCliente as cliente WITH(NOLOCK) ON cliente.ClienteID = docu.ClienteID and cliente.EmpresaGrupoID = docu.eg_ccCliente " +
                                " INNER JOIN ccCompradorCartera as compCartera WITH(NOLOCK) ON (compCartera.CompradorCarteraID = docu.CompradorCarteraID " +
                                    " AND compCartera.EmpresaGrupoID = docu.eg_ccCompradorCartera)  " +
                        " where ventaDeta.NumeroDoc = @NumeroDoc and docu.EmpresaID = @EmpresaID " +
                        "  ORDER BY convert(int, docu.Libranza)  ";

                    #endregion
                }

                #region  Creacion Parametros

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                #endregion
                #region Asignacion de Valores

                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                #endregion

                DTO_ccCesionCartera cesion = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    cesion = new DTO_ccCesionCartera(dr, isCesion);
                    cesion.CedulaAutorizadora.Value = (autoriza[1]);
                    cesion.NombreAutorizadora.Value = autoriza[2];
                    if (isCesion)
                    {
                        cesion.TerceroCoperativa.Value = autoriza[3];
                        cesion.NomCoperativa.Value = autoriza[4];
                    }

                    results.Add(cesion);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_cc_Cesion");
                throw exception;
            }
        }

        #endregion

        #region Reportes ccCreditoDocu
        /// <summary>
        /// Funcion que trae la informacion de los aportes de acuerdo al Componente de aportes
        /// </summary>
        /// <param name="mes">Mes de la liquidacion</param>
        /// <param name="filter">Filtros</param>
        /// <param name="compAporte">Componente</param>
        /// <returns>Lista de Aportes</returns>
        public List<DTO_ccAportes> DAL_ReportesCartera_Cc_Aportes(DateTime mes, string filter, string compAfiliacion, string compAportes)
        {
            try
            {
                List<DTO_ccAportes> results = new List<DTO_ccAportes>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                string filtro = null;

                if (filter != null)
                    filtro = "AND pag.PagaduriaID = " + filter;

                mySqlCommandSel.CommandText = "SELECT CompAfiliacion.*, compo.CuotaValor " +
                                              "  FROM " +
                                              "      ( " +
                                              "          SELECT  CreDoc.NumeroDoc, cli.ClienteID, cli.Descriptivo as EmpleadoDesc, ResidenciaDir, FechaIngreso,  " +
                                              "                  cli.LaboralCiudad, pag.Descriptivo as Pagaduria " +
                                              "          FROM  ccCreditoDocu AS CreDoc with(nolock) " +
                                              "              INNER JOIN ccCreditoComponentes AS CredCom ON CredCom.NumeroDoc = CreDoc.NumeroDoc " +
                                              "              INNER JOIN ccCliente	cli ON (cli.ClienteID = CreDoc.ClienteID and cli.EmpresaGrupoID = CreDoc.eg_ccCliente) " +
                                              "              INNER JOIN ccPagaduria pag ON (pag.PagaduriaID = CreDoc.PagaduriaID and pag.EmpresaGrupoID = CreDoc.eg_ccPagaduria) " +
                                              "          WHERE  CreDoc.EmpresaID = @EmpresaID " +
                                              "              AND CredCom.ComponenteCarteraID = @CompAfiliacion " +
                                              "              AND datePart(YEAR,CreDoc.FechaLiquida) = @Año AND datePart(MONTH,CreDoc.FechaLiquida) = @Mes " +
                                              "      ) as CompAfiliacion " +
                                              "          INNER JOIN ccCreditoComponentes compo ON CompAfiliacion.NumeroDoc = compo.NumeroDoc " +
                                              "  WHERE  ComponenteCarteraID = @CompAportes ";


                mySqlCommandSel.Parameters.Add("@CompAfiliacion", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompAportes", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Mes", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CompAfiliacion"].Value = compAfiliacion;
                mySqlCommandSel.Parameters["@CompAportes"].Value = compAportes;
                mySqlCommandSel.Parameters["@Mes"].Value = mes.Month;
                mySqlCommandSel.Parameters["@Año"].Value = mes.Year;

                DTO_ccAportes doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccAportes(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noReporteTotalConceptoXEmpleado");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que obtiene la info correspondiente del cerdtificado de deuda por el numero de libranza y el mes 
        /// </summary>
        /// <param name="fechaCorte">Mes que se va a consultar</param>
        /// <param name="libranza">Numero de libranza</param>
        /// <returns>Info del certificado de deuda, ccCreditoDocu</returns>
        public DTO_ccCertificadoDeuda DAL_ReportesCartera_Cc_CertificadoDeuda(DateTime fechaCorte, int libranza)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT docu.NumeroDoc,docu.Libranza,cli.Descriptivo, docu.ClienteID,   " +
                                           "       pag.Descriptivo as Pagaduria , cli.Correo, docu.FechaLiquida, docu.VlrLibranza, docu.Plazo, docu.VlrCuota,  " +
                                           "       estado.EC_SaldoPend, estado.EC_FechaLimite" +
                                           " FROM ccCreditoDocu docu " +
                                           "    INNER JOIN ccCliente cli on cli.ClienteID = docu.ClienteID  and cli.EmpresaGrupoID = docu.eg_ccCliente " +
                                           "    INNER JOIN ccPagaduria pag on pag.PagaduriaID = docu.PagaduriaID and pag.EmpresaGrupoID = docu.eg_ccPagaduria " +
                                           "    INNER JOIN ccEstadoCuentaHistoria estado on estado.NumDocCredito = docu.NumeroDoc  " + 
                                           " WHERE docu.Libranza = @Libranza " +
                                           " AND docu.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);

                mySqlCommand.Parameters["@Libranza"].Value = libranza;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                DTO_ccCertificadoDeuda res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    res = new DTO_ccCertificadoDeuda(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_ReportesCartera", "DAL_ReportesCartera_Cc_CertificadoDeuda");
                throw exception;
            }
        }
        #endregion

        #region Reporte ccIncorporacion

        /// <summary>
        /// Carga el DTO de incorporacion
        /// </summary>
        /// <param name="numeroDoc">Numero doc que usa para buscar los datos</param>
        /// <param name="isLiquidacion">Revisa si esmliquidado por credito o  solicitud</param>
        /// <returns>Lista de incorporaciones</returns>
        public List<DTO_ccIncorporaciones> DAL_ReportesCartera_Cc_Incorporacion(int numeroDoc, bool isLiquidacion, string representanteLegal)
        {
            try
            {
                List<DTO_ccIncorporaciones> results = new List<DTO_ccIncorporaciones>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommanText
                //Valida si es aprobado por Credito
                if (isLiquidacion)
                {
                    mySqlCommandSel.CommandText =
                         " SELECT incorpora.NumeroDoc,credito.fechaCuota1,	pagaduria.Descriptivo as nomPagaduria, 'CC' as TipoDoc, credito.ClienteID, " +
                                    " cliente.Descriptivo as Nombre, credito.VlrLibranza,  incorpora.ValorCuota as VlrCuota, credito.Libranza " +
                         " FROM ccIncorporacionDeta incorpora WITH(NOLOCK) " +
                                " INNER JOIN ccCreditoDocu credito WITH(NOLOCK) ON credito.NumeroDoc = incorpora.NumDocCredito " +
                                " INNER JOIN ccCliente cliente WITH(NOLOCK)  ON cliente.ClienteID = credito.ClienteID and cliente.EmpresaGrupoID = credito.eg_ccCliente    " +
                                " INNER JOIN ccPagaduria pagaduria WITH(NOLOCK)  ON pagaduria.PagaduriaID = incorpora.PagaduriaID and pagaduria.EmpresaGrupoID = incorpora.eg_ccPagaduria  " +
                         " WHERE incorpora.NumeroDoc = @numeroDoc  " +
                         " GROUP BY pagaduria.Descriptivo, credito.ClienteID, cliente.Descriptivo,credito.VlrLibranza,incorpora.ValorCuota,credito.Libranza, " +
                                " incorpora.NumeroDoc,credito.fechaCuota1 " +
                         " ORDER BY cast(credito.Libranza as int)  ";
                }
                else
                {
                    mySqlCommandSel.CommandText =

                        " SELECT incorpora.NumeroDoc, soliDocu.fechaCuota1, pagaduria.Descriptivo as nomPagaduria, 'CC' as TipoDoc, soliDocu.ClienteID, " +
                                " cliente.Descriptivo as Nombre, soliDocu.VlrLibranza, incorpora.ValorCuota as VlrCuota, soliDocu.Libranza " +
                        " FROM ccIncorporacionDeta incorpora WITH(NOLOCK)   " +
                            " INNER JOIN ccSolicitudDocu soliDocu WITH(NOLOCK) ON soliDocu.NumeroDoc = incorpora.NumDocCredito  " +
                            " INNER JOIN ccCliente cliente WITH(NOLOCK)  ON cliente.ClienteID = soliDocu.ClienteID and cliente.EmpresaGrupoID = soliDocu.eg_ccCliente   " +
                            " INNER JOIN ccPagaduria pagaduria WITH(NOLOCK)  ON pagaduria.PagaduriaID = incorpora.PagaduriaID and pagaduria.EmpresaGrupoID = incorpora.eg_ccPagaduria " +
                        " WHERE incorpora.NumeroDoc = @numeroDoc " +
                        " GROUP BY pagaduria.Descriptivo, soliDocu.ClienteID, cliente.Descriptivo,soliDocu.VlrLibranza,incorpora.ValorCuota,soliDocu.Libranza, " +
                                " incorpora.NumeroDoc, soliDocu.fechaCuota1 " +
                         " ORDER BY cast(soliDocu.Libranza as int)  ";
                }

                #endregion

                mySqlCommandSel.Parameters.Add("@numeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@numeroDoc"].Value = numeroDoc;

                DTO_ccIncorporaciones incorpora = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    incorpora = new DTO_ccIncorporaciones(dr);
                    incorpora.RepresentateLegal.Value = representanteLegal;
                    results.Add(incorpora);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_Incorporacion");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion q se encarga cargar los clientes con su respectiva Pagaduria
        /// </summary>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="CodIncopora">Codigo de la pagaduria que esta parametrizada en el control</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ccPagaduriaIncoporacion> DAL_ReportesCartera_Cc_PagaduriaIncorporacion(DateTime FechaInicial, DateTime FechaFinal, string Pagaduria, string CodIncopora)
        {
            try
            {
                List<DTO_ccPagaduriaIncoporacion> result = new List<DTO_ccPagaduriaIncoporacion>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string paga = "";

                if (!string.IsNullOrEmpty(Pagaduria))
                    paga = " AND incorDeta.PagaduriaID  = @Pagaduria ";

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                    "SELECT ctrlIncorpora.FechaDoc, incorDeta.PagaduriaID as CodigoPagaduria, paga.Descriptivo as Pagaduria, clie.Cargo as Grado, " +
                    "	clie.EmpleadoCodigo as CodigoMilitar, clie.TerceroID as Tercero,   clie.Descriptivo as Nombre, cred.Libranza, " +
                    "	cred.FechaLiquida as FechaLibranza,   incorDeta.ValorCuota as Cuota, incorDeta.FechaCuota1 as FechaInicio,  cred.FechaVto as FechaTerminacion " +
                    "FROM glDocumentoControl as ctrlIncorpora WITH(NOLOCK) " +
                    "	INNER JOIN ccIncorporacionDeta as incorDeta  WITH(NOLOCK) ON  ctrlIncorpora.NumeroDoc = incorDeta.NumeroDoc " + paga +
                    "	INNER JOIN ccCreditoDocu AS cred WITH(NOLOCK) ON incorDeta.NumDocCredito = cred.NumeroDoc " +
                    "	INNER JOIN ccCliente as clie WITH(NOLOCK) ON cred.ClienteID = clie.ClienteID AND cred.eg_ccCliente = clie.EmpresaGrupoID " +
                    "	INNER JOIN ccPagaduria AS paga WITH(NOLOCK) ON paga.PagaduriaID = incorDeta.PagaduriaID AND paga.EmpresaGrupoID = incorDeta.eg_ccPagaduria " +
                    "WHERE ctrlIncorpora.EmpresaID = @EmpresaID AND ctrlIncorpora.DocumentoID = @DocumentoID " +
                    "	AND YEAR(ctrlIncorpora.FechaDoc) = @Año  AND MONTH(ctrlIncorpora.FechaDoc) >= @MesInicial  AND MONTH(ctrlIncorpora.FechaDoc) <= @MesFinal "; 

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodIncopora", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MesInicial", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@MesFinal", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);

                #endregion
                #region Asignacion de Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CodIncopora"].Value = CodIncopora;
                mySqlCommandSel.Parameters["@DocumentoID"].Value = AppDocuments.Incorporacion;
                mySqlCommandSel.Parameters["@Año"].Value = FechaInicial.Year;
                mySqlCommandSel.Parameters["@MesInicial"].Value = FechaInicial.Month;
                mySqlCommandSel.Parameters["@MesFinal"].Value = FechaFinal.Month;
                mySqlCommandSel.Parameters["@Pagaduria"].Value = Pagaduria;

                #endregion

                DTO_ccPagaduriaIncoporacion PagaIncorp = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    PagaIncorp = new DTO_ccPagaduriaIncoporacion(dr);
                    result.Add(PagaIncorp);
                }
                dr.Close();

                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_PagaduriaIncorporacion");
                throw exception;
            }
        }

        #endregion

        #region Reporte Liquidacion Credito

        /// <summary>
        /// Carga los Datos de la liquidacion de la cartera 
        /// </summary>
        /// <param name="libranza">Libranza la cual se liquida</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportLiquidacionCredito> DAL_ReportesCartera_Cc_LiquidacionCredito(int libranza)
        {
            try
            {
                List<DTO_ReportLiquidacionCredito> result = new List<DTO_ReportLiquidacionCredito>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Commantext
                mySqlCommandSel.CommandText =
                    " SELECT crediDoc.NumeroDoc, crediDoc.Libranza, crediDoc.ClienteID, cli.Descriptivo as nombreCliente, crediDoc.Plazo, planPag.VlrCuota,crediDoc.VlrCuotaSeguro, crediDoc.VlrLibranza, " +
                        " crediDoc.PorSeguro,crediDoc.Pagare,crediDoc.ConcesionarioID,conces.Descriptivo as ConcesionarioDesc,ctrl.CentroCostoID,crediDoc.PorInteres, crediDoc.AsesorID, paga.Descriptivo as PagaDesc, zona.Descriptivo as zonaDesc, crediDoc.FechaLiquida, " +
                        " planPag.FechaLiquidaMora, CAST(planPag.CuotaID AS CHAR(3)) +'-'+ +' '+ CAST(crediDoc.Plazo AS CHAR(3))AS NumCuota , planPag.VlrCapital, " +
                        "  planPag.VlrInteres,planPag.VlrSeguro, planPag.VlrOtro1, planPag.VlrOtro2, planPag.VlrOtro3, planPag.VlrOtrosfijos, planPag.VlrSaldoCapital, " +
                        " crediDoc.VlrSolicitado, crediDoc.VlrAdicional, asesor.Descriptivo as asesor " +
                    " FROM ccCreditoDocu as crediDoc WITH(NOLOCK) " +
                        " INNER JOIN glDocumentoControl as ctrl  WITH(NOLOCK) on ctrl.NumeroDoc = crediDoc.NumeroDoc " +
                        " INNER JOIN ccCreditoPlanPagos as planPag  WITH(NOLOCK) on planPag.NumeroDoc = crediDoc.NumeroDoc " +
                        " INNER JOIN ccCliente as cli WITH(NOLOCK) on (cli.ClienteID = crediDoc.ClienteID and cli.EmpresaGrupoID = crediDoc.eg_ccCliente) " +
                        " LEFT JOIN ccPagaduria as paga WITH(NOLOCK) on (paga.PagaduriaID = crediDoc.PagaduriaID and paga.EmpresaGrupoID = crediDoc.eg_ccPagaduria) " +
                        " LEFT JOIN glZona as zona WITH(NOLOCK) on (zona.ZonaID = crediDoc.ZonaID and zona.EmpresaGrupoID = crediDoc.eg_glZona) " +
                        " LEFT JOIN ccAsesor as asesor WITH(NOLOCK) on  ( asesor.AsesorID = crediDoc.AsesorID and asesor.EmpresaGrupoID = crediDoc.eg_ccAsesor) " +
                        " LEFT JOIN ccConcesionario as conces WITH(NOLOCK) on (conces.ConcesionarioID = crediDoc.ConcesionarioID and conces.EmpresaGrupoID = crediDoc.eg_ccConcesionario) " +

                        " where crediDoc.EmpresaID = @EmpresaID " +
                        " and crediDoc.Libranza = @libranza ";
                #endregion

                #region Paramentros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@libranza", SqlDbType.Int);
                #endregion

                #region Asiganacion Valores de Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@libranza"].Value = libranza;
                #endregion

                DTO_ReportLiquidacionCredito liquidaCredito = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    liquidaCredito = new DTO_ReportLiquidacionCredito(dr);
                    result.Add(liquidaCredito);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_LiquidacionCredito");
                throw exception;
            }
        }

        #endregion

        #region Libranzas

        /// <summary>
        /// Carga los Datos de la liquidacion de la cartera 
        /// </summary>
        /// <param name="libranza">Libranza la cual se liquida</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportLibranzas> DAL_ReportesCartera_Cc_Libranzas(DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria)
        {
            try
            {
                List<DTO_ReportLibranzas> result = new List<DTO_ReportLibranzas>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string filtro = "";

                if (!string.IsNullOrEmpty(Cliente))
                    filtro = filtro + " AND crdocu.ClienteID=@Cliente ";
                if (!string.IsNullOrEmpty(Libranza.ToString()))
                    filtro = filtro + " AND crdocu.Libranza=@Libranza ";
                if (!string.IsNullOrEmpty(Asesor))
                    filtro = filtro + " AND crdocu.AsesorID=@Asesor ";
                if (!string.IsNullOrEmpty(Pagaduria))
                    filtro = filtro + " AND crdocu.PagaduriaID=@Pagaduria ";

                #endregion
                #region Commantext
                mySqlCommandSel.CommandText =
                " SELECT  DISTINCT " +
                "        crdocu.NumeroDoc, " +
                "        CAST(crdocu.Libranza as integer) as Libranza, crdocu.ClienteID, " +
                "        cli.Descriptivo as NombreCliente, " +
                "        ase.Descriptivo as NombreAsesor, " +
                "        crdocu.Plazo, " +
                "        crdocu.PagaduriaID, " +
                "        crdocu.VlrCuota,crdocu.VlrSolicitado, " +
                "        crdocu.VlrPrestamo,crdocu.VlrLibranza, " +
                "        crdocu.VlrGiro " +
                " FROM ccCreditoDocu crdocu WITH(NOLOCK) " +
                "        INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc=crdocu.NumeroDoc " +
                "        INNER JOIN ccCliente cli WITH(NOLOCK) ON cli.ClienteID=crdocu.ClienteID AND cli.EmpresaGrupoID=crdocu.eg_ccCliente " +
                "        INNER JOIN ccAsesor ase WITH(NOLOCK) ON ase.AsesorID=crdocu.AsesorID AND ase.EmpresaGrupoID=crdocu.eg_ccAsesor " +
                "        INNER JOIN ccCreditoComponentes crecom WITH(NOLOCK) ON crecom.NumeroDoc=crdocu.NumeroDoc " +
                " WHERE ctrl.EmpresaID=@EmpresaID AND (ctrl.PeriodoDoc>=@Periodo AND ctrl.PeriodoDoc<=@PeriodoFin) " + filtro +
                " ORDER BY CAST(crdocu.Libranza as integer) ";
                #endregion
                #region Paramentros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@PeriodoFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Cliente", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char, 6);
                mySqlCommandSel.Parameters.Add("@Asesor", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                #endregion
                #region Asiganacion Valores de Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = Periodo;
                mySqlCommandSel.Parameters["@PeriodoFin"].Value = PeriodoFin;
                mySqlCommandSel.Parameters["@Cliente"].Value = Cliente;
                mySqlCommandSel.Parameters["@Libranza"].Value = Libranza;
                mySqlCommandSel.Parameters["@Asesor"].Value = Asesor;
                mySqlCommandSel.Parameters["@Pagaduria"].Value = Pagaduria;
                #endregion

                DTO_ReportLibranzas libranza = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    libranza = new DTO_ReportLibranzas(dr);
                    result.Add(libranza);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_LiquidacionCredito");
                throw exception;
            }
        }

        /// <summary>
        /// Carga los Datos de los componentes 
        /// </summary>
        /// <param name="libranza">Libranza la cual se liquida</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportLibranzasDetalle> DAL_ReportesCartera_Cc_LibranzasDetalle(int NumeroDoc)
        {
            try
            {
                List<DTO_ReportLibranzasDetalle> resultTotal = new List<DTO_ReportLibranzasDetalle>();
                
                    SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                    mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                    #region Commantext
                    mySqlCommandSel.CommandText =
                    " SELECT cre.TotalValor,SUBSTRING(carcom.Descriptivo,1,15) AS Nombre " +
                    "    FROM ccCreditoComponentes cre WITH(NOLOCK) " +
                    "    INNER JOIN ccCarteraComponente carcom WITH(NOLOCK) ON  carcom.ComponenteCarteraID=cre.ComponenteCarteraID AND carcom.EmpresaGrupoID=cre.eg_ccCarteraComponente " +
                    " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc=cre.NumeroDoc " +
                    "    WHERE cre.NumeroDoc=@NumeroDoc AND ctrl.EmpresaID=@EmpresaID order by cre.ComponenteCarteraID ";
                    #endregion

                    #region Paramentros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                    #endregion

                    #region Asiganacion Valores de Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@NumeroDoc"].Value = NumeroDoc;

                    #endregion

                    SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                    while (dr.Read())
                    {
                        DTO_ReportLibranzasDetalle result = new DTO_ReportLibranzasDetalle(dr);
                        resultTotal.Add(result);
                    }

                    dr.Close();


                    return resultTotal;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_LiquidacionCredito");
                throw exception;
            }
        }

        /// <summary>
        /// Carga los Datos de la liquidacion de la cartera 
        /// </summary>
        /// <param name="libranza">Libranza la cual se liquida</param>
        /// <returns>Listado de DTO</returns>
        public DataTable DAL_ReportesCartera_Cc_LibranzasExcel(DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria)
        {
            try
            {
                #region Filtros

                string filtro = "";

                if (!string.IsNullOrEmpty(Cliente))
                    filtro = filtro + " AND crdocu.ClienteID=@ClienteID ";
                if (!string.IsNullOrEmpty(Libranza.ToString()))
                    filtro = filtro + " AND crdocu.Libranza=@Libranza ";
                if (!string.IsNullOrEmpty(Asesor))
                    filtro = filtro + " AND crdocu.AsesorID=@AsesorID ";
                if (!string.IsNullOrEmpty(Pagaduria))
                    filtro = filtro + " AND crdocu.PagaduriaID=@PagaduriaID ";

                #endregion
                #region Commantext

                SqlCommand sc = new SqlCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                sc = new SqlCommand("Cartera_RepLibranzas", MySqlConnection.CreateCommand().Connection);

                #endregion
                #region Paramentros
                sc.Parameters.Add(new SqlParameter("@EmpresaID",this.Empresa.ID.Value));
                sc.Parameters.Add(new SqlParameter("@Periodo", Periodo));
                sc.Parameters.Add(new SqlParameter("@PeriodoFin", PeriodoFin));
                sc.Parameters.Add(new SqlParameter("@ClienteID", Cliente));
                sc.Parameters.Add(new SqlParameter("@Libranza", Libranza));
                sc.Parameters.Add(new SqlParameter("@AsesorID", Asesor));
                sc.Parameters.Add(new SqlParameter("@PagaduriaID", Pagaduria));
                sc.Parameters.Add(new SqlParameter("@Filtros", filtro));
                sc.Parameters.Add(new SqlParameter("@ErrorDesc", string.Empty));
                //sda.SelectCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                //sda.SelectCommand.Parameters.Add("@PeriodoFin", SqlDbType.DateTime);
                //sda.SelectCommand.Parameters.Add("@Cliente", SqlDbType.Char, UDT_ClienteID.MaxLength);
                //sda.SelectCommand.Parameters.Add("@Libranza", SqlDbType.Char, 6);
                //sda.SelectCommand.Parameters.Add("@Asesor", SqlDbType.Char, UDT_AsesorID.MaxLength);
                //sda.SelectCommand.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                //sda.SelectCommand.Parameters.Add("@Filtros", SqlDbType.Char, Char.MaxValue);
                #endregion
                #region Asiganacion Valores de Parametros
                //sda.SelectCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //sda.SelectCommand.Parameters["@Periodo"].Value = Periodo;
                //sda.SelectCommand.Parameters["@PeriodoFin"].Value = PeriodoFin;
                //sda.SelectCommand.Parameters["@Cliente"].Value = Cliente;
                //sda.SelectCommand.Parameters["@Libranza"].Value = Libranza;
                //sda.SelectCommand.Parameters["@Asesor"].Value = Asesor;
                //sda.SelectCommand.Parameters["@Pagaduria"].Value = Pagaduria;
                //sda.SelectCommand.Parameters["@Filtros"].Value = filtro;
                #endregion
                sc.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = sc;
                DataTable dt = new DataTable("Libranzas");
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_LibranzasExcel");
                throw exception;
            }
        }
        #endregion

        #region Reporte Por Pagaduria

        public List<DTO_ccArchivoPlanoXPagaduria> DAL_ReportesCartera_Cc_ArchivosPlanos(string pagaduria)
        {
            try
            {
                List<DTO_ccArchivoPlanoXPagaduria> results = new List<DTO_ccArchivoPlanoXPagaduria>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommanText

                mySqlCommand.CommandText =
                    " SELECT replicate ('0',(10 - len(cliente.EmpleadoCodigo))) + convert(varchar, cliente.EmpleadoCodigo,10) as codigo, " +
                        " replicate ('0',(20 - len(crediDocu.ClienteID))) + convert(varchar, crediDocu.ClienteID,20) as CC, " +
                        " replicate ('0',(20 - len(crediDocu.VlrCuota))) + convert(varchar, crediDocu.VlrCuota,20) as VlrCuota, " +
                        " replicate ('0',(2 - len(crediDocu.Plazo )))+ convert(varchar,(str(crediDocu.Plazo  ,2))) as  NroCuotas, " +
                        " replicate ('0',(20 - len(crediDocu.Libranza))) + convert(varchar, crediDocu.Libranza,20) as Libranza, " +
                        " 'P' as TipoNove " +
                    " FROM  ccCreditoDocu as crediDocu " +
                        " INNER JOIN ccCliente as cliente with(nolock) on cliente.ClienteID = crediDocu.ClienteID and cliente.EmpresaGrupoID = crediDocu.eg_ccCliente " +
                    " where crediDocu.PagaduriaID = @pagaduria ";

                #endregion

                mySqlCommand.Parameters.Add("@pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters["@pagaduria"].Value = pagaduria;

                DTO_ccArchivoPlanoXPagaduria archvivoPlano = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    archvivoPlano = new DTO_ccArchivoPlanoXPagaduria(dr);
                    results.Add(archvivoPlano);
                }
                dr.Close();

                return results;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_ReportesCartera", "DAL_ReportesCartera_Cc_ArchivosPlanos");
                throw exception;
            }
        }

        #endregion

        #region Reportes Referenciacion

        /// <summary>
        /// Funcion q se encarga de traer las libranza con su respectiva referenciacion
        /// </summary>
        /// <param name="libranza">Numero de libranza que se desea ver</param>
        /// <param name="cliente">Identificacion del cliente q se desea ver</param>
        /// <param name="FechaRef">Fecha en que se hizo la refenciacion</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_ccReferenciacion> DAL_ReportesCartera_Cc_Referenciacion(string libranza, string cliente, string actFlujoID, DateTime FechaRef, bool _llamadaCodEfect)
        {
            try
            {
                List<DTO_ccReferenciacion> results = new List<DTO_ccReferenciacion>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Filtros

                string filtro = "";

                if (string.IsNullOrEmpty(libranza) && string.IsNullOrEmpty(cliente))
                    filtro = " AND ctrl.FechaDoc = @FechaDoc ";
                if (!string.IsNullOrEmpty(libranza))
                    filtro = " AND soliDocu.Libranza = @LibranzaID ";
                if (!string.IsNullOrEmpty(cliente))
                    filtro += " AND soliDocu.ClienteID = @ClienteID ";
                if (_llamadaCodEfect == true)
                    filtro += " AND llamCod.NoEfectivaInd = 0";
                #endregion
                #region CommanText

                mySqlCommand.CommandText =
                    " select " +
                        " soliDocu.Libranza, clie.Descriptivo nombreCliente ,  soliDocu.ClienteID, ctrl.FechaDoc, terRef.Nombre,terRef.Telefono, " +
                        " terRef.Direccion, terRef.Correo, llamPre.Pregunta, llamCtrl.Observaciones, llamCtrl.PersonaConsulta, llamCtrl.RelacionTitular, " +
                        " CASE WHEN (llamPre.TipoReferencia = 1 ) THEN 'PERSONAL' " +
                        " WHEN (llamPre.TipoReferencia = 2) THEN 'FAMILIAR' " +
                        " WHEN (llamPre.TipoReferencia = 3) THEN 'COMERCIAL' " +
                        " WHEN (llamPre.TipoReferencia = 4) THEN 'TITULAR' END AS TipoReferencia " +
                    " FROM ccSolicitudDocu soliDocu WITH(NOLOCK) " +
                        " INNER JOIN glDocumentoControl ctrl   WITH(NOLOCK) on (ctrl.NumeroDoc = soliDocu.NumeroDoc) " +
                        " INNER JOIN ccCliente clie WITH(NOLOCK) on (clie.ClienteID = soliDocu.ClienteID and clie.EmpresaGrupoID = soliDocu.eg_ccCliente) " +
                        " INNER JOIN glActividadFlujo actFlujo WITH(NOLOCK) on (actFlujo.ActividadFlujoID =  @ActiFlujo ) " +
                        " INNER JOIN glTerceroReferencia terRef WITH(NOLOCK) on  (terRef.TerceroID = soliDocu.ClienteID and terRef.EmpresaGrupoID = soliDocu.EmpresaID) " +
                        " INNER JOIN glLlamadasControl llamCtrl WITH(NOLOCK) on (llamCtrl.NumeroDoc = soliDocu.NumeroDoc) " +
                        " LEFT JOIN glLLamadaCodigo llamCod WITH(NOLOCK) on (llamCtrl.CodLLamada = llamCod.CodLLamada and llamCtrl.eg_glLlamadaCodigo = llamCod.EmpresaGrupoID) " +
                        " INNER JOIN glLLamadaPregunta llamPre WITH(NOLOCK) on (llamPre.LLamadaID = actFlujo.LLamadaID and llamPre.TipoReferencia = terRef.TipoReferencia " +
                            " and llamPre.EmpresaGrupoID = terRef.EmpresaGrupoID and llamPre.ReplicaID = llamCtrl.IdentificadorPrg )   " +
                    " WHERE soliDocu.EmpresaID = @EmpresaID  " +
                        //" AND ctrl.FechaDoc = @FechaDoc  " +
                         filtro +                     
                    " order by CAST(soliDocu.Libranza as INT) ";

                #endregion
                #region Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@LibranzaID", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaDoc", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ActiFlujo", SqlDbType.Char);
                mySqlCommand.Parameters.Add("@llamadaCtrl", SqlDbType.Bit);
                #endregion
                #region Asignacion Valores a Parametros
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@LibranzaID"].Value = libranza;
                mySqlCommand.Parameters["@ClienteID"].Value = cliente;
                mySqlCommand.Parameters["@FechaDoc"].Value = FechaRef;
                mySqlCommand.Parameters["@ActiFlujo"].Value = actFlujoID;
                mySqlCommand.Parameters["@llamadaCtrl"].Value = _llamadaCodEfect;
                #endregion

                DTO_ccReferenciacion referenciacion = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    referenciacion = new DTO_ccReferenciacion(dr);
                    results.Add(referenciacion);
                }
                dr.Close();

                return results;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, "DAL_ReportesCartera", "DAL_Reportes_Cc_Referenciacion");
                throw exception;
            }
        }

        #endregion

        #region Reportes Saldos y Movimientos

        /// <summary>
        /// Carga el DTO de Saldo
        /// </summary>
        /// <param name="periodo">Periodo a Consiltar</param>
        /// <param name="cliente">Cliente que se quiere consultar</param>
        /// <param name="pagaduria">Pagaduria que se quiere consultar</param>
        /// <param name="lineaCredi">Linea de Credito que se quiere consultar</param>
        /// <param name="compCartera">Comprador de cartera que se quiere consultar</param>
        /// <param name="asesor">Asesor que se quiere consultar</param>
        /// <param name="plazo">Plazo que se quiere consultar</param>
        /// <param name="compradorCatera">Comprador Cartera que se quiere consultar</param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<DTO_ccSaldos> DAL_ReportesCartera_Cc_SaldosCartera(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, 
            string asesor, string tipoCartera, string compCapital, string compInteres, string compSeguro)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_ccSaldos> results = new List<DTO_ccSaldos>();

                #region Configuracion de Filtros

                // Carga los filtros de acuerdo a la parametrizacion del usuario
                string filtroCliente = "", filtroPagaduria = "", filtroLineaCre = "", filtroCompCartera = "", filtroAsesor = "", filtroPlazo = "", filtroTipoCartera = "";
                if (!string.IsNullOrEmpty(cliente))
                    filtroCliente = " AND cred.ClienteID = @ClienteID  ";
                if (!string.IsNullOrEmpty(pagaduria))
                    filtroPagaduria = " AND cred.PagaduriaID = @PagaduriaID ";
                if (!string.IsNullOrEmpty(lineaCredi))
                    filtroLineaCre = " AND cred.LineaCreditoID = @LineaCreditoID ";
                if (!string.IsNullOrEmpty(compCartera))
                    filtroCompCartera = " AND cred.CompradorCarteraID = @CompradorID ";
                if (!string.IsNullOrEmpty(asesor))
                    filtroAsesor = "  AND cred.AsesorID = @AsesorID ";

                switch (tipoCartera)
                {
                    case ("Propia"):
                        filtroTipoCartera = " AND cred.TipoEstado = 1 ";
                        break;
                    case ("Cedida"):
                        filtroTipoCartera = " AND cred.TipoEstado = 2 ";
                        break;
                    default:
                        break;
                }

                int mes = 0;
                if(!string.IsNullOrEmpty(Convert.ToString(periodo)))
                {
                    DateTime num = Convert.ToDateTime(periodo);
                    mes = num.Month; ;
                }
                #endregion
                #region CommanText
                mySqlCommandSel.CommandText =

                    "   DECLARE @Mes int    " +
                    "   set  @Mes			= (case when @Periodo <=12 then @Periodo else 12 end) * 100 + (case when @Periodo <=12 then 1 else 2 end)    " +
                    "   SELECT     " +
                    "   	Libranza,    " +
                    "   	ClienteID,    " +
                    "   	Descriptivo,    " +
                    "   	PagaduriaID,    " +
                    "   	CompradorCarteraID,    " +
                    "   	Oferta,    " +
                    "   	Capital,    " +
                    "   	Interes,    " +
                    "   	Seguro,    " +
                    "   	Otros,    " +
                    "   	(Capital+Interes+Seguro+Otros) VlrTotal    " +
                    "   FROM    " +
                    "   (    " +
                    "   SELECT     " +
                    "   	pla.NumeroDoc,    " +
                    "   	cred.Libranza,    " +
                    "   	cred.ClienteID,    " +
                    "   	cli.Descriptivo,    " +
                    "   	cred.PagaduriaID,    " +
                    "   	cred.CompradorCarteraID,    " +
                    "   	VENTA.Oferta,    " +
                    "   	sum(pla.vlrCapital -   (case when (pag.AboCapital is null) then 0 else pag.AboCapital end)) as Capital,    " +
                    "   	sum(Pla.VlrInteres -   (case when (pag.AboInteres is null) then 0 else pag.AboInteres end)) as Interes,    " +
                    "   	sum(Pla.VlrSeguro  -   (case when (pag.AboCapSegu is null) then 0 else pag.AboCapSegu end)) as Seguro,    " +
                    "   	sum(Pla.VlrOtrosfijos -    (case when (pag.AboOtros is null) then 0 else pag.AboOtros end)) as Otros    " +
                    "   FROM ccCreditoPlanPagos as pla WITH (NOLOCK)    " +
                    "   left join ( SELECT 					    " +
                    "   					pag.CreditoCuotaNum,    " +
                    "   					SUM(vlrCapital)   as AboCapital,    " +
                    "   					SUM(VlrInteres)   as AboInteres,    " +
                    "   					SUM(VlrSeguro)   as AboCapSegu,    " +
                    "   					SUM(VlrOtrosfijos)   as AboOtros    " +
                    "   				FROM ccCreditoPagos pag    group by pag.CreditoCuotaNum    " +
                    "   			) as  pag ON pag.CreditoCuotaNum = pla.consecutivo    " +
                    "   left join ccCreditoDocu				as cred	WITH(NOLOCK) on cred.NumeroDoc = pla.NumeroDoc     " +
                    "   left join ccCliente						as cli		WITH(NOLOCK) on  cred.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = @EmpresaID    " +
                    "   LEFT JOIN ccVentaDocu			venta with(nolock) ON venta.NumeroDoc = cred.DocVenta     " +
                    "   INNER JOIN glDocumentoControl CTL WITH(NOLOCK) ON pla.NumeroDoc = CTL.NumeroDoc    " +
                    "   where cred.EmpresaID = @EmpresaID AND month(cred.FechaCuota1) * 100+DAY(cred.FechaCuota1) >=@Mes AND CTL.DocumentoID = @documentId     " +
                        filtroCliente + filtroPagaduria + filtroLineaCre + filtroCompCartera + filtroAsesor + filtroPlazo + filtroTipoCartera +    
                    "   group by     " +
                    "   	cred.Libranza,    " +
                    "   	cred.ClienteID,    " +
                    "   	cli.Descriptivo,    " +
                    "   	cred.PagaduriaID,    " +
                    "   	cred.CompradorCarteraID,    " +
                    "   	VENTA.Oferta,    " +
                    "   	pla.NumeroDoc    " +
                    "   ) FIN    " +
                    "   order by Libranza, ClienteID    " ;

                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@periodo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);

                #endregion
                #region Asignacion de Valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@periodo"].Value = mes;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.LiquidacionCredito;
                mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                mySqlCommandSel.Parameters["@PagaduriaID"].Value = pagaduria;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                mySqlCommandSel.Parameters["@CompradorID"].Value = compCartera;
                mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;

                #endregion

                DTO_ccSaldos doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccSaldos(dr, false);
                    results.Add(doc);
                }
                dr.Close();

                #region Original StureProcedure (Comentariado)
                #region Parametros

                //mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@periodo", SqlDbType.DateTime);
                //mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@cliente", SqlDbType.Char, UDT_ClienteID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@linea", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@comprador", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@asesor", SqlDbType.Char, UDT_AsesorID.MaxLength);

                #endregion
                #region Asignacion de Valores a parametros

                //mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@periodo"].Value = periodo;
                //mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.LiquidacionCredito;
                //mySqlCommandSel.Parameters["@cliente"].Value = cliente;
                //mySqlCommandSel.Parameters["@pagaduria"].Value = pagaduria;
                //mySqlCommandSel.Parameters["@linea"].Value = lineaCredi;
                //mySqlCommandSel.Parameters["@comprador"].Value = compCartera;
                //mySqlCommandSel.Parameters["@asesor"].Value = asesor;

                #endregion
                //#region Parametros

                //mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                //mySqlCommandSel.Parameters.Add("@DocumentID", SqlDbType.Int);
                //mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@CompradorID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@CompCapital", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@CompInteres", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@CompSeguro", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@Filtros", SqlDbType.Char, 1000);
                //mySqlCommandSel.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                ////Parametros de salida
                //mySqlCommandSel.Parameters.Add("@ErrorDesc", SqlDbType.VarChar, 1000);
                //mySqlCommandSel.Parameters["@ErrorDesc"].Direction = ParameterDirection.Output;

                //#endregion
                //#region Asignacion de Valores a parametros

                //mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@PeriodoID"].Value = periodo;
                //mySqlCommandSel.Parameters["@DocumentID"].Value = AppDocuments.LiquidacionCredito;
                //mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                //mySqlCommandSel.Parameters["@PagaduriaID"].Value = pagaduria;
                //mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                //mySqlCommandSel.Parameters["@CompradorID"].Value = compCartera;
                //mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;
                //mySqlCommandSel.Parameters["@CompCapital"].Value = compCapital;
                //mySqlCommandSel.Parameters["@CompInteres"].Value = compInteres;
                //mySqlCommandSel.Parameters["@CompSeguro"].Value = compSeguro;
                //mySqlCommandSel.Parameters["@Filtros"].Value = filtroTipoCartera + filtroCliente + filtroPagaduria + filtroLineaCre + filtroCompCartera + 
                //                                                filtroAsesor + filtroPlazo;
                //mySqlCommandSel.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);

                //#endregion


                //mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                //mySqlCommandSel.CommandText = "Cartera_SaldosCreditos";

                //SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                //if (mySqlCommandSel.Parameters["@ErrorDesc"].Value != null && !string.IsNullOrWhiteSpace(mySqlCommandSel.Parameters["@ErrorDesc"].Value.ToString()))
                //{
                //    var exception = new Exception(mySqlCommandSel.Parameters["@ErrorDesc"].Value.ToString());
                //    Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Cartera_SaldosCreditos");
                //    return results;
                //}
                //else
                //{
                //    while (dr.Read())
                //    {
                //        DTO_ccSaldos doc = new DTO_ccSaldos(dr, false);
                //        results.Add(doc);
                //    }
                //} 
                #endregion
                //dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_SaldosCartera");
                return new List<DTO_ccSaldos>();

            }
        }

        /// <summary>
        /// Carga el DTO de Saldo
        /// </summary>
        /// <param name="periodo">Periodo a Consiltar</param>
        /// <param name="cliente">Cliente que se quiere consultar</param>
        /// <param name="pagaduria">Pagaduria que se quiere consultar</param>
        /// <param name="lineaCredi">Linea de Credito que se quiere consultar</param>
        /// <param name="compCartera">Comprador de cartera que se quiere consultar</param>
        /// <param name="asesor">Asesor que se quiere consultar</param>
        /// <param name="plazo">Plazo que se quiere consultar</param>
        /// <param name="compradorCatera">Comprador Cartera que se quiere consultar</param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<DTO_ccSaldos> DAL_ReportesCartera_Cc_SaldosFavor(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera,string compSaldosFavor, bool isSaldoFavor)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_ccSaldos> results = new List<DTO_ccSaldos>();

                #region Configuracion de Filtros

                // Carga los filtros de acuerdo a la parametrizacion del usuario
                string filtroCliente = "", filtroPagaduria = "", filtroLineaCre = "", filtroCompCartera = "", filtroAsesor = "", filtroPlazo = "", filtroTipoCartera = ""; /*, sentencia = "", operador ="", cartera ="";*/

                if (!string.IsNullOrEmpty(cliente))
                    filtroCliente = " AND cred.ClienteID = @cliente  ";
                if (!string.IsNullOrEmpty(pagaduria))
                    filtroPagaduria = " AND cred.PagaduriaID = @pagaduria ";
                if (!string.IsNullOrEmpty(lineaCredi))
                    filtroLineaCre = " AND cred.LineaCreditoID = @linea ";
                if (!string.IsNullOrEmpty(compCartera))
                    filtroCompCartera = " AND cred.CompradorCarteraID = @comprador ";
                if (!string.IsNullOrEmpty(asesor))
                    filtroAsesor = "  AND cred.AsesorID = @asesor ";

                switch (tipoCartera)
                {
                    case ("Propia"):
                        filtroTipoCartera = " AND cred.TipoEstado = 1 ";
                        break;
                    case ("Cedida"):
                        filtroTipoCartera = " AND cred.TipoEstado = 2 ";
                        break;
                    default:
                        break;
                }

                //if (!string.IsNullOrWhiteSpace(compradorCatera))
                //{
                //    sentencia = " AND cred.CompradorCarteraID ";
                //    if (tipo.Contains("propia"))
                //    {
                //        operador = " = ";
                //        cartera = sentencia + operador + "'" + compradorCatera + " '";
                //    }
                //    else if (tipo.Contains("cedida"))
                //    {
                //        operador = " <> ";
                //        cartera = sentencia + operador + "'" + compradorCatera + " '" + " OR cred.CompradorCarteraID IS NULL";
                //    }
                //}

                #endregion
                #region CommanText
                    mySqlCommandSel.CommandText =
                            "SELECT cred.Libranza, cli.ClienteID, cli.Descriptivo, " +
                                " SUM(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) AS SaldoAFavor " +
                                " FROM coCuentaSaldo s with(nolock) " +
                                    " INNER JOIN glDocumentoControl ctrl ON  ctrl.NumeroDoc = s.IdentificadorTR " +
                                    " INNER JOIN ccCreditoDocu cred ON cred.NumeroDoc = ctrl.NumeroDoc " +
                                    " INNER JOIN ccCarteraComponente comp ON comp.ConceptoSaldoID = s.ConceptoSaldoID and comp.eg_glConceptoSaldo = s.eg_glConceptoSaldo and comp.ComponenteCarteraID = @CompSaldosFavor  " +
                                    " INNER JOIN ccCliente cli ON  cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                            " WHERE " +
                                    " ctrl.DocumentoID = @documentId " +
                                    filtroCliente + filtroPagaduria + filtroLineaCre + filtroCompCartera + filtroAsesor + filtroPlazo +
                                    " AND s.PeriodoID <= @periodo and comp.CuentaID = s.CuentaID " +
                                    " AND cred.EmpresaID = @EmpresaID " +
                                    filtroTipoCartera +
                            " GROUP BY Libranza, cli.ClienteID, cli.Descriptivo " +
                            " ORDER BY cli.Descriptivo ";
                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@documentId", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@cliente", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@linea", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@comprador", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@asesor", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompSaldosFavor", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                
                #endregion
                #region Asignacion de Valores a parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@documentId"].Value = AppDocuments.LiquidacionCredito;
                mySqlCommandSel.Parameters["@cliente"].Value = cliente;
                mySqlCommandSel.Parameters["@pagaduria"].Value = pagaduria;
                mySqlCommandSel.Parameters["@linea"].Value = lineaCredi;
                mySqlCommandSel.Parameters["@comprador"].Value = compCartera;
                mySqlCommandSel.Parameters["@asesor"].Value = asesor;
                mySqlCommandSel.Parameters["@CompSaldosFavor"].Value = compSaldosFavor;
                #endregion
                DTO_ccSaldos doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccSaldos(dr, isSaldoFavor);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_SaldosCartera");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el DTO de con lo saldos en Mora
        /// </summary>
        /// <param name="fechaCorte">Filtra los Creditos en Mora por fecha</param>
        /// <param name="cliente">Filtra un Cliente  en Mora especifico</param>
        /// <param name="pagaduria">Filtra una Pagaduria en Mora </param>
        /// <param name="lineaCredi">Filtra una Linea De Credito en Mora</param>
        /// <param name="compCartera">Filtra Un comprador de Cartera en Mora</param>
        /// <param name="asesor">Filtra un Asesor en Mora</param>
        /// <param name="plazo">Filtra los Credito que tengan este plazo</param>
        /// <returns>Lista de DTo</returns>
        public List<DTO_ccSaldosMora> DAL_ReportesCartera_Cc_SaldosMora(DateTime fechaCorte, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor,
            int plazo, string tipoCartera)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_ccSaldosMora> results = new List<DTO_ccSaldosMora>();

                #region Configuracion de Filtros

                // Carga los filtros de acuerdo a la parametrizacion del usuario
                string filtroCliente = "", filtroPagaduria = "", filtroLineaCre = "", filtroCompCartera = "", filtroAsesor = "", filtroPlazo = "", filtroTipoCartera = ""; /*, sentencia = "", operador ="", cartera ="";*/

                if (!string.IsNullOrEmpty(cliente))
                    filtroCliente = " AND crd.ClienteID = @cliente  ";
                if (!string.IsNullOrEmpty(pagaduria))
                    filtroPagaduria = " AND crd.PagaduriaID = @pagaduria ";
                if (!string.IsNullOrEmpty(lineaCredi))
                    filtroLineaCre = " AND crd.LineaCreditoID = @linea ";
                if (!string.IsNullOrEmpty(compCartera))
                    filtroCompCartera = " AND crd.CompradorCarteraID = @comprador ";
                if (!string.IsNullOrEmpty(asesor))
                    filtroAsesor = "  AND crd.AsesorID = @asesor ";
                if (plazo != 0)
                    filtroPlazo = " AND crd.Plazo = @plazo ";

                switch (tipoCartera)
                {
                    case ("Propia"):
                        filtroTipoCartera = " AND crd.TipoEstado = 1 ";
                        break;
                    case ("Cedida"):
                        filtroTipoCartera = " AND crd.TipoEstado = 2 ";
                        break;
                    default:
                        break;
                }

                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                         "SELECT crd.CompradorCarteraID, " +
                         "     crd.Libranza, " +
                         "     crd.ClienteID, " +
                         "     crd.CentroPagoID, " +
                         "     cli.Descriptivo as Nombre, " +
                         "     pla.NumeroDoc, " +
                         "     sum(case when ((pla.vlrCapital - (case when (pag.AboCapital is null) then 0 else pag.AboCapital end) > 0 )) then 1 else 0 end) as NroCuotasVencidas, " +
                         "    (sum(pla.vlrCapital -  (case when (pag.AboCapital is null) then 0 else pag.AboCapital end))+ " +
                         "     sum(Pla.VlrInteres -  (case when (pag.AboInteres is null) then 0 else pag.AboInteres end)) + " +
                         "     sum(Pla.VlrSeguro  -  (case when (pag.AboCapSegu is null) then 0 else pag.AboCapSegu end)) + " +
                         "     sum(Pla.VlrOtro1 -    (case when (pag.AboIntSegu is null) then 0 else pag.AboIntSegu end)) + " +
                         "     sum(pla.VlrOtrosfijos -  (case when (pag.AboOtrFijo is null) then 0 else pag.AboOtrFijo end))) as SaldoMoraTotal      " +
                        "FROM ccCreditoPlanPagos (nolock)  as pla " +
                        "    left join ( SELECT pag.CreditoCuotaNum, " +
                        "                  SUM(vlrCapital)   as AboCapital, " +
                        "                  SUM(VlrInteres)   as AboInteres, " +
                        "                  SUM(VlrSeguro)   as AboCapSegu, " +
                        "                  SUM(VlrOtro1)   as AboIntSegu, " +
                        "                  SUM(VlrOtrosfijos)  as AboOtrFijo " +
                        "                FROM ccCreditoPagos (nolock) pag	 " +
                        "                    inner join gldocumentoControl (nolock)as docCtrlPago on pag.PagoDocu = docCtrlPago.NumeroDoc 	 " +
                        "                    inner join ccCreditoDocu (nolock)as credPago on pag.NumeroDoc = credPago.NumeroDoc 	 " +
                        "                Where credPago.FechaLiquida <= @FechaCorte and docCtrlPago.FechaDoc <= @FechaCorte " +
                        "                Group by pag.CreditoCuotaNum		 " +
                        "                ) as  pag ON pag.CreditoCuotaNum = pla.consecutivo " +
                        "     left join ccCreditoDocu (nolock)as crd on crd.NumeroDoc = pla.NumeroDoc  " +
                        "     left join ccCliente (nolock) as cli on  crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID =  @EmpresaID " +
                        "where crd.EmpresaID = @EmpresaID and  crd.FechaLiquida <= @FechaCorte and pla.FechaCuota <= @FechaCorte " +
                           filtroTipoCartera + filtroCliente + filtroPagaduria + filtroLineaCre + filtroCompCartera + filtroAsesor + filtroPlazo +
                        "group by crd.CompradorCarteraID " +
                        "  ,crd.Libranza ,crd.ClienteID " +
                        "  ,crd.CentroPagoID ,crd.EstadoDeuda " +
                        "  ,cli.Descriptivo ,pla.NumeroDoc " +
                        "Order by crd.CompradorCarteraID , crd.Libranza ";

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@cliente", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@linea", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@comprador", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@asesor", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@plazo", SqlDbType.Int);

                #endregion
                #region Asignacion de Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaCorte;
                mySqlCommandSel.Parameters["@cliente"].Value = cliente;
                mySqlCommandSel.Parameters["@pagaduria"].Value = pagaduria;
                mySqlCommandSel.Parameters["@linea"].Value = lineaCredi;
                mySqlCommandSel.Parameters["@comprador"].Value = compCartera;
                mySqlCommandSel.Parameters["@asesor"].Value = asesor;
                mySqlCommandSel.Parameters["@plazo"].Value = plazo;
                #endregion

                DTO_ccSaldosMora doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccSaldosMora(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;

            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_SaldosMora");
                throw exception;
            }

        }

        /// <summary>
        /// Funcion que carga el DTO_ccAportesCliente
        /// </summary>
        /// <param name="periodo">Periodo por el cual se filtra</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <param name="componenteCarteraID">Componente Aportes glcontrol</param>
        /// <returns>Lista de Aportes hechos por el cliente</returns>
        public List<DTO_ccAportesCliente> DAL_ReportesCartera_Cc_AportesCliente(DateTime periodo, string clienteFiltro, string cuenta, string componenteCarteraID)
        {
            string filtroCliente = "";
            try
            {
                if (!string.IsNullOrEmpty(clienteFiltro))
                    filtroCliente = " AND ccCliente.ClienteID = " + clienteFiltro;

                List<DTO_ccAportesCliente> results = new List<DTO_ccAportesCliente>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =
                                    " SELECT compCta.ComponenteCarteraID,aux.Fecha, cli.ClienteID, cli.Descriptivo, " +
                                    " abs(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML)AS SaldoMLoc, " +
                                    " CASE WHEN (aux.vlrMdaLoc<0) THEN aux.vlrMdaLoc ELSE '0' END AS 'Ingreso', " +
                                    " CASE WHEN (aux.vlrMdaLoc>=0) THEN aux.vlrMdaLoc ELSE '0' END AS 'Retiro', " +
                                    " abs(DbOrigenLocML + DbOrigenExtML + CrOrigenLocML + CrOrigenExtML + DbSaldoIniLocML + DbSaldoIniExtML + CrSaldoIniLocML + CrSaldoIniExtML) + aux.vlrMdaLoc AS NuevoSaldo " +
                                    " FROM coCuentaSaldo as ctaSal with(nolock) " +
                                    " INNER JOIN ccComponenteCuenta as compCta with(nolock) ON compCta.CtaRecursosTerceros = ctaSal.CuentaID and compCta.EmpresaGrupoID=ctaSal.eg_coPlanCuenta " +
                                    " INNER JOIN ccCliente as cli with(nolock) ON cli.ClienteID = ctaSal.TerceroID and cli.EmpresaGrupoID=ctaSal.eg_coTercero " +
                                    " INNER JOIN coAuxiliar as aux with(nolock) ON aux.CuentaID = ctaSal.CuentaID and aux.EmpresaID=ctaSal.eg_coPlanCuenta " +
                                    " WHERE ctaSal.EmpresaID = @EmpresaID " +
                                    " AND compCta.ComponenteCarteraID = @Componente " +
                                    " AND ctaSal.CuentaID = @Cuenta " +
                                    " AND ctaSal.PeriodoID = @Periodo AND aux.PeriodoID = @Periodo " +
                                    " AND compCta.TipoEstado=1 " +
                                      filtroCliente;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Componente", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Cuenta", SqlDbType.Char);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;
                mySqlCommandSel.Parameters["@Componente"].Value = componenteCarteraID;
                mySqlCommandSel.Parameters["@Cuenta"].Value = cuenta;

                DTO_ccAportesCliente doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccAportesCliente(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_AportesCliente");
                throw exception;
            }
        }

        /// <summary>
        /// Función que carga una lista de movimientos desde el coAuxiliar
        /// </summary>
        /// <param name="periodo">Periodo de la consulta</param>
        /// <param name="clienteFiltro">Filtro por el cliente</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_ccEstadoDeCuenta> DAL_ReportesCartera_Cc_EstadoCuenta(DateTime fechaIni, DateTime fechaFin,string _tercero, string cuenta, string clienteFiltro)
        {
            string filtroCliente = "";
            try
            {
                if (!string.IsNullOrEmpty(clienteFiltro))
                    filtroCliente = " AND ccCliente.ClienteID = " + clienteFiltro;

                List<DTO_ccEstadoDeCuenta> results = new List<DTO_ccEstadoDeCuenta>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                            "SELECT aux.TerceroID as ClienteId, Coter.Descriptivo, Aux.Fecha, Aux.DocumentoCOM as DocumentoTercero, ABS(Aux.vlrMdaLoc) AS vlrMdaLoc, " +
                                " (cta.DbSaldoIniLocML + cta.CrSaldoIniLocML)as SaldoIni, " +
                                    " 'Movimiento'= " +
                                  " CASE  " +
                                     " WHEN Aux.vlrMdaLoc <  0 THEN 'Recaudo' " +
                                     " ELSE 'Retiro' " +
                                  " END " +
                            " FROM coAuxiliar  Aux With (nolock) " +
                                " INNER JOIN glDocumentoControl Ctrl ON Ctrl.NumeroDoc = Aux.NumeroDoc " +
                                " INNER JOIN coTercero Coter ON Coter.TerceroID = aux.TerceroID		" +
                                " LEFT JOIN coCuentaSaldo cta ON cta.IdentificadorTR = aux.NumeroDoc " +
                            " AND Aux.PeriodoID = cta.PeriodoID " +
                            " Where aux.PeriodoID between @fechaIni AND @fechaFin " +
                                " AND Aux.CuentaID =   @cuenta " +
                                //" AND aux.TerceroID = CASE WHEN @tercero = '' OR @tercero IS NULL THEN aux.TerceroID ELSE @tercero END " +
                                " AND aux.EmpresaID = @EmpresaID "; 
                #endregion
                #region Paramentros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@cuenta", SqlDbType.Char);
                //mySqlCommandSel.Parameters.Add("@tercero", SqlDbType.Char);
                #endregion
                #region Asignacion Valores Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIni;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFin;
                mySqlCommandSel.Parameters["@cuenta"].Value = cuenta;
                //mySqlCommandSel.Parameters["@tercero"].Value = _tercero;
                #endregion

                DTO_ccEstadoDeCuenta doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccEstadoDeCuenta(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_EstadoCuenta");
                throw exception;
            }
        }

        /// <summary>
        /// Carga el DTO de Saldo
        /// </summary>
        /// <param name="periodo">Periodo a Consiltar</param>
        /// <param name="cliente">Cliente que se quiere consultar</param>
        /// <param name="pagaduria">Pagaduria que se quiere consultar</param>
        /// <param name="lineaCredi">Linea de Credito que se quiere consultar</param>
        /// <param name="compCartera">Comprador de cartera que se quiere consultar</param>
        /// <param name="asesor">Asesor que se quiere consultar</param>
        /// <param name="plazo">Plazo que se quiere consultar</param>
        /// <param name="compradorCatera">Comprador Cartera que se quiere consultar</param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<DTO_SaldosReport> DAL_ReportesCartera_Cc_SaldosNuevo(byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string ConcesionarioID, string asesor, string lineaCredi, string compCartera, byte Agrupamiento, byte Romp)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                List<DTO_SaldosReport> results = new List<DTO_SaldosReport>();

                #region Configuracion de Filtros

                // Carga los filtros de acuerdo a la parametrizacion del usuario
                string filtros = "";
                bool isCuota = tipoReporte == 1? false : true;
                if (!string.IsNullOrEmpty(cliente))
                    filtros += "AND cred.ClienteID = @ClienteID  ";
                if (!string.IsNullOrEmpty(libranza.ToString()))
                    filtros += " AND cred.Libranza = @Libranza  ";
                if (!string.IsNullOrEmpty(zonaID))
                    filtros += " AND cred.ZonaID = @ZonaID  ";
                if (!string.IsNullOrEmpty(ciudad))
                    filtros += " AND cred.Ciudad = @Ciudad  ";
                if (!string.IsNullOrEmpty(ConcesionarioID))
                    filtros += " AND cred.ConcesionarioID = @ConcesionarioID ";
                if (!string.IsNullOrEmpty(asesor))
                    filtros += " AND cred.AsesorID = @AsesorID ";
                if (!string.IsNullOrEmpty(lineaCredi))
                    filtros += " AND cred.LineaCreditoID = @LineaCreditoID ";
                if (!string.IsNullOrEmpty(compCartera))
                    filtros += " AND cred.CompradorCarteraID = @CompradorID ";

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                #endregion
                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                mySqlCommandSel.Parameters["@ZonaID"].Value = zonaID;
                mySqlCommandSel.Parameters["@Ciudad"].Value = ciudad;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = ConcesionarioID;
                mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compCartera;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni != null ? fechaIni.Value.Date : fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                #endregion
                #region CommanText

                if (!isCuota) //Saldos
                {
                    mySqlCommandSel.CommandText =
                           "SELECT q.NumeroDoc, q.Libranza,  q.ClienteID, q.Nombre, " +
                           "   sum(q.CapitalCta) as CapitalCta, sum(q.CapitalAbo) as CapitalAbo, " +
                           "   SUM(q.CapitalCta-q.CapitalAbo) as CapitalSdo, " +
                           "   sum(q.InteresCta) as InteresCta, sum(q.InteresAbo) as InteresAbo, " +
                           "   sum(q.InteresCta-q.InteresAbo) as InteresSdo,   " +
                           "   sum(q.SeguCapCta) as SeguCapCta, sum(q.SeguCapAbo) as SeguCapAbo, " +
                           "   sum(q.SeguCapCta-q.SeguCapAbo) as SeguCapSdo,  " +
                           "   sum(q.SeguIntCta) as SeguIntCta, sum(q.SeguIntAbo) as SeguIntAbo, " +
                           "   sum(q.SeguIntCta-q.SeguIntAbo) as SeguIntSdo " +
                           "FROM  " +
                           "	(SELECT cred.NumeroDoc,cred.Libranza, cred.ClienteID,cliente.Descriptivo  as Nombre, p.VlrCapital  as CapitalCta, " +
                           "	  sum(g.VlrCapital) as CapitalAbo, p.VlrInteres  as InteresCta,  " +
                           "	  sum(g.VlrInteres) as InteresAbo, p.VlrSeguro   as SeguCapCta, " +
                           "	  SUM(g.VlrSeguro) as SeguCapAbo, p.VlrOtro1   as SeguIntCta, " +
                           "	  SUM(g.VlrOtro1)  as SeguIntAbo " +
                           "	 FROM ccCreditoDocu cred " +
                           "	   inner join ccCreditoPlanPagos p on cred.NumeroDoc = p.NumeroDoc " +
                           "	   inner join ccCreditoPagos g on g.CreditoCuotaNum = p.Consecutivo " +
                           "	   inner join ccCliente cliente on cliente.ClienteID = cred.ClienteID" +
                           "	 WHERE  cred.EmpresaID = @EmpresaID " + filtros +
                           "	 GROUP BY cred.NumeroDoc,cred.Libranza,cred.ClienteID,cliente.Descriptivo, p.VlrCapital, p.VlrInteres, p.VlrSeguro, p.VlrOtro1 " +
                           "	) AS Q " +
                           "GROUP BY q.NumeroDoc, q.Libranza, q.ClienteID,q.Nombre "; 
                }
                else //Cuota
                {
                    mySqlCommandSel.CommandText =
                        "SELECT cred.NumeroDoc, cred.Libranza, cred.ClienteID,cliente.Descriptivo as Nombre,   " +
                        "       pp.FechaCuota,pp.CuotaID,sum(pg.VlrCapital+pg.VlrInteres+pg.VlrSeguro+pg.VlrOtro1) as VlrCuota, " +
                        "       sum(pg.VlrCapital) as CapitalAbo,    " +
                        "       sum(pg.VlrInteres) as InteresAbo,  " +
                        "       SUM(pg.VlrSeguro) as SeguCapAbo,  " +
                        "       SUM(pg.VlrOtro1)  as SeguIntAbo,  " +
                        "       pp.VlrSaldoCapital as SaldoCapital  " +
                        "FROM ccCreditoDocu cred " +
                        "   inner join ccCreditoPlanPagos pp on cred.NumeroDoc = pp.NumeroDoc  " +
                        "   inner join ccCreditoPagos pg on pg.CreditoCuotaNum = pp.Consecutivo  " +
                        "   inner join ccCliente cliente on cliente.ClienteID = cred.ClienteID  " +
                        "WHERE  cred.EmpresaID = @EmpresaID " + filtros +
                        "GROUP BY pp.FechaCuota, pp.CuotaID,pp.VlrSaldoCapital,cred.NumeroDoc, cred.Libranza, cred.ClienteID,cliente.Descriptivo,pp.VlrCapital, pp.VlrInteres, pp.VlrSeguro, pp.VlrOtro1 ";                       
                }

                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_SaldosReport doc = new DTO_SaldosReport(dr, isCuota);                  
                    results.Add(doc);
                }
                dr.Close();

                if (Agrupamiento == 1 && results.Count > 0) //Resumido
                {
                    List<DTO_SaldosReport> resultResumido = new List<DTO_SaldosReport>();
                    List<string> clienteDistinct = (from c in results select c.ClienteID.Value).Distinct().ToList();
                    foreach (string cl in clienteDistinct)
                    {
                        DTO_SaldosReport sdo = new DTO_SaldosReport();
                        sdo.ClienteID.Value = cl;
                        sdo.Nombre.Value = results.FindAll(x => x.ClienteID.Value == cl).First().Nombre.Value;
                        sdo.Detalle = results.FindAll(x => x.ClienteID.Value == cl).ToList();
                        resultResumido.Add(sdo);
                    }
                    results = resultResumido;
                }

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_SaldosCartera");
                return new List<DTO_SaldosReport>();

            }
        }

        /// <summary>
        /// Carga el DTO de Saldo
        /// </summary>
        /// <param name="periodo">Periodo a Consiltar</param>
        /// <param name="cliente">Cliente que se quiere consultar</param>
        /// <param name="pagaduria">Pagaduria que se quiere consultar</param>
        /// <param name="lineaCredi">Linea de Credito que se quiere consultar</param>
        /// <param name="compCartera">Comprador de cartera que se quiere consultar</param>
        /// <param name="asesor">Asesor que se quiere consultar</param>
        /// <param name="plazo">Plazo que se quiere consultar</param>
        /// <param name="compradorCatera">Comprador Cartera que se quiere consultar</param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public DataTable DAL_ReportesCartera_Cc_SaldosNuevoExcel(byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string ConcesionarioID, string asesor, string lineaCredi, string compCartera, byte Agrupamiento, byte Romp)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();

                #region Configuracion de Filtros

                // Carga los filtros de acuerdo a la parametrizacion del usuario
                string filtros = "";
                bool isCuota = tipoReporte == 1 ? false : true;
                if (!string.IsNullOrEmpty(cliente))
                    filtros += "AND cred.ClienteID = @ClienteID  ";
                if (!string.IsNullOrEmpty(libranza.ToString()))
                    filtros += " AND cred.Libranza = @Libranza  ";
                if (!string.IsNullOrEmpty(zonaID))
                    filtros += " AND cred.ZonaID = @ZonaID  ";
                if (!string.IsNullOrEmpty(ciudad))
                    filtros += " AND cred.Ciudad = @Ciudad  ";
                if (!string.IsNullOrEmpty(ConcesionarioID))
                    filtros += " AND cred.ConcesionarioID = @ConcesionarioID ";
                if (!string.IsNullOrEmpty(asesor))
                    filtros += " AND cred.AsesorID = @AsesorID ";
                if (!string.IsNullOrEmpty(lineaCredi))
                    filtros += " AND cred.LineaCreditoID = @LineaCreditoID ";
                if (!string.IsNullOrEmpty(compCartera))
                    filtros += " AND cred.CompradorCarteraID = @CompradorID ";

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                #endregion
                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                mySqlCommandSel.Parameters["@ZonaID"].Value = zonaID;
                mySqlCommandSel.Parameters["@Ciudad"].Value = ciudad;
                mySqlCommandSel.Parameters["@ConcesionarioID"].Value = ConcesionarioID;
                mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;
                mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compCartera;
                mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni != null ? fechaIni.Value.Date : fechaIni;
                mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                #endregion
                #region CommanText

                if (!isCuota) //Saldos
                {
                    mySqlCommandSel.CommandText =
                           "SELECT q.NumeroDoc, q.Libranza,  q.ClienteID, q.Nombre, " +
                           "   SUM(q.CapitalCta-q.CapitalAbo) as CapitalSdo, " +
                           "   sum(q.InteresCta-q.InteresAbo) as InteresSdo,   " +
                           "   sum(q.SeguCapCta-q.SeguCapAbo) as SeguCapSdo,  " +
                           "   sum(q.SeguIntCta-q.SeguIntAbo) as SeguIntSdo " +
                           "FROM  " +
                           "	(SELECT cred.NumeroDoc,cred.Libranza, cred.ClienteID,cliente.Descriptivo  as Nombre, p.VlrCapital  as CapitalCta, " +
                           "	  sum(g.VlrCapital) as CapitalAbo, p.VlrInteres  as InteresCta,  " +
                           "	  sum(g.VlrInteres) as InteresAbo, p.VlrSeguro   as SeguCapCta, " +
                           "	  SUM(g.VlrSeguro) as SeguCapAbo, p.VlrOtro1   as SeguIntCta, " +
                           "	  SUM(g.VlrOtro1)  as SeguIntAbo " +
                           "	 FROM ccCreditoDocu cred " +
                           "	   inner join ccCreditoPlanPagos p on cred.NumeroDoc = p.NumeroDoc " +
                           "	   inner join ccCreditoPagos g on g.CreditoCuotaNum = p.Consecutivo " +
                           "	   inner join ccCliente cliente on cliente.ClienteID = cred.ClienteID" +
                           "	 WHERE cred.EmpresaID = @EmpresaID " + filtros +
                           "	 GROUP BY cred.NumeroDoc,cred.Libranza,cred.ClienteID,cliente.Descriptivo, p.VlrCapital, p.VlrInteres, p.VlrSeguro, p.VlrOtro1 " +
                           "	) AS Q " +
                           "GROUP BY q.NumeroDoc, q.Libranza, q.ClienteID,q.Nombre ";
                }
                else //Cuota
                {
                    mySqlCommandSel.CommandText =
                        "SELECT cred.Libranza, cred.ClienteID,cliente.Descriptivo as Nombre ,  " +
                        "       pp.FechaCuota,pp.CuotaID,sum(pg.VlrCapital+pg.VlrInteres+pg.VlrSeguro+pg.VlrOtro1) as VlrCuota, " +
                        "       sum(pg.VlrCapital) as CapitalAbo,    " +
                        "       sum(pg.VlrInteres) as InteresAbo,  " +
                        "       SUM(pg.VlrSeguro) as SeguCapAbo,  " +
                        "       SUM(pg.VlrOtro1)  as SeguIntAbo,  " +
                        "       pp.VlrSaldoCapital as SaldoCapital  " +
                        "FROM ccCreditoDocu cred " +
                        "   inner join ccCreditoPlanPagos pp on cred.NumeroDoc = pp.NumeroDoc  " +
                        "   inner join ccCreditoPagos pg on pg.CreditoCuotaNum = pp.Consecutivo  " +
                        "   inner join ccCliente cliente on cliente.ClienteID = cred.ClienteID  " +
                        "WHERE  cred.EmpresaID = @EmpresaID " + filtros +
                        "GROUP BY pp.FechaCuota, pp.CuotaID,pp.VlrSaldoCapital,cred.NumeroDoc, cred.Libranza, cred.ClienteID,cliente.Descriptivo,pp.VlrCapital, pp.VlrInteres, pp.VlrSeguro, pp.VlrOtro1 ";
                }

                #endregion

                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable("Saldos");

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                sda.Fill(table); 
                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_SaldosCartera");
                return null;

            }
        }

        #endregion

        #region Reportes SIGCOOP

        /// <summary>
        /// Funcion que se encarga de traer los datos para el formato F19
        /// </summary>
        /// <param name="Periodo">Perido a Consultar</param>
        /// <param name="Formato">Tipo de Formato que desea Exportar</param>
        /// <returns></returns>
        public DataTable DAL_ReportesCartera_Cc_InformeSIGCOOP(DateTime Periodo, string Formato)
        {
            try
            {
                SqlCommand sc = new SqlCommand("Cartera_ReportInformesSIGCOOP", base.MySqlConnection.CreateCommand().Connection);
                SqlDataAdapter sda = new SqlDataAdapter();

                #region Parametros

                sc.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                sc.Parameters.Add("@Year", SqlDbType.Int);
                sc.Parameters.Add("@Month", SqlDbType.Int);
                sc.Parameters.Add("@Formato", SqlDbType.Char, 20);

                #endregion
                #region Asignacion de valores a Parametros
                
                sc.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value ;
                sc.Parameters["@Year"].Value = Periodo.Year;
                sc.Parameters["@Month"].Value = Periodo.Month ;
                sc.Parameters["@Formato"].Value = Formato;

                #endregion

                sc.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = sc;
                DataTable table = new DataTable("InformesSIGCOOP");
                sda.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_InformeSIGCOOP");
                throw exception;
            }
        }

        #endregion

        #region Reporte Solicitudes

        /// <summary>
        /// DTO que carga las solicitudes
        /// </summary>
        /// <param name="fechaIncial">Fecha en que inicia la consulta</param>
        /// <param name="fechaFinal">Fecha en que termina la consulta</param>
        /// <param name="cliente">Cliente que se desea ver</param>
        /// <param name="libranza">Número de la libranza a Consultar</param>
        /// <param name="asesor">Asesor que se desea ver</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<DTO_ccSolicitudes> DAL_ReportesCartera_Cc_Solicitudes(DateTime fechaIncial, DateTime fechaFinal, string cliente, string libranza, string asesor, string estado)
        {
            List<DTO_ccSolicitudes> result = new List<DTO_ccSolicitudes>();

            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Filtros

                //Carga los filtros al Query
                string clie = "", libranz = "", ases = "", esta = "", where = "";

                if (!string.IsNullOrEmpty(cliente))
                    clie = " AND soliDocu.ClienteID = @cliente ";
                if (!string.IsNullOrEmpty(libranza))
                    libranz = " AND soliDocu.Libranza = @libranza ";
                if (!string.IsNullOrEmpty(asesor))
                    ases = " AND soliDocu.AsesorID = @asesor ";

                switch (estado)
                {
                    case ("sinAprobar"):
                        esta = "  ,acFlu.Descriptivo AS estado ";
                        where = " AND CerradoInd = 0  ";
                        break;
                    case ("Aprobadas"):
                        esta = " ,CASE WHEN(ctrl.Estado = -1) THEN 'Cerrado' " +
                                    " WHEN (ctrl.Estado = 0) THEN 'Anulado'  " +
                                    " WHEN (ctrl.Estado = 3) THEN 'Aprobado' " +
                                    " WHEN (ctrl.Estado = 4) THEN 'Revertido' " +
                                    " WHEN (ctrl.Estado = 5) THEN 'Devuelto' " +
                                    " WHEN (ctrl.Estado = 6) THEN 'Radicado' " +
                                    " WHEN (ctrl.Estado = 7) THEN 'Revisado' " +
                                    " WHEN (ctrl.Estado = 8) THEN 'Contabilizado' END AS estado ";
                        where = " and ctrl.Estado=3 ";
                        break;
                    case ("Cerrada"):
                        esta = " ,CASE WHEN(ctrl.Estado = -1) THEN 'Cerrado' END AS estado ";
                        where = "  AND ctrl.Estado = -1 ";
                        break;
                    case ("Todos"):
                        esta = " ,CASE WHEN (ctrl.Estado = -1 or ctrl.Estado = 0 or ctrl.Estado = 3 or ctrl.Estado = 4 or ctrl.Estado = 5 or " +
                                    " ctrl.Estado = 6 or ctrl.Estado = 7 or ctrl.Estado = 8) then  " +
                                            " CASE WHEN(ctrl.Estado = -1) THEN 'Cerrado'  " +
                                            " WHEN (ctrl.Estado = 0) THEN 'Anulado'  " +
                                            " WHEN (ctrl.Estado = 3) THEN 'Aprobado'  " +
                                            " WHEN (ctrl.Estado = 4) THEN 'Revertido' " +
                                            " WHEN (ctrl.Estado = 5) THEN 'Devuelto' " +
                                            " WHEN (ctrl.Estado = 6) THEN 'Radicado' " +
                                            " WHEN (ctrl.Estado = 7) THEN 'Revisado' " +
                                            " WHEN (ctrl.Estado = 8) THEN 'Contabilizado' end " +
                                 " else acFlu.Descriptivo  end as estado ";
                        break;
                    case ("Anuladas"):
                        esta = " ,CASE WHEN(ctrl.Estado = 0) THEN 'Cerrado' END AS estado ";
                        where = " AND ctrl.Estado = 0 ";
                        break;

                }

                #endregion

                #region CommanText
                mySqlCommandSel.CommandText =
                            " SELECT DISTINCT soliDocu.Libranza, soliDocu.ClienteID as cc,  SUBSTRING(cliente.Descriptivo,1,30) as NombreCliente, ctrl.FechaDoc, soliDocu.VlrSolicitado, " +
                                " soliDocu.VlrPrestamo, soliDocu.VlrLibranza, VlrCuota" + esta +
                            "FROM ccSolicitudDocu as soliDocu WITH(NOLOCK) " +
                                " INNER JOIN ccCliente as cliente WITH(NOLOCK) on (cliente.ClienteID = soliDocu.ClienteID and cliente.EmpresaGrupoID = soliDocu.eg_ccCliente) " +
                                " INNER JOIN glDocumentoControl as ctrl  WITH(NOLOCK) on  ctrl.NumeroDoc = soliDocu.NumeroDoc " +
                                " INNER JOIN glActividadEstado as actEstado WITH(NOLOCK) on  actEstado.NumeroDoc =  ctrl.NumeroDoc " +
                                " INNER JOIN glActividadFlujo as acFlu WITH(NOLOCK) on  (acFlu.ActividadFlujoID = actEstado.ActividadFlujoID " +
                                      " AND acFlu.EmpresaGrupoID = actEstado.eg_glActividadFlujo) " +
                            " WHERE soliDocu.EmpresaID = @EmpresaID " +
                                where +
                                " AND  DATEPART(year, ctrl.FechaDoc) = @año " +
                                " AND  DATEPART(MONTH, ctrl.FechaDoc) >= @fechaIni " +
                                " AND DATEPART(MONTH, ctrl.FechaDoc) <= @fechaFin " +
                                clie + libranz + ases +
                             " ORDER BY Libranza ASC  ";
                #endregion

                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID ", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@año", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaIni", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@cliente", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@libranza", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@asesor", SqlDbType.Char);

                #endregion

                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID "].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@año"].Value = fechaIncial.Year;
                mySqlCommandSel.Parameters["@fechaIni"].Value = fechaIncial.Month;
                mySqlCommandSel.Parameters["@fechaFin"].Value = fechaFinal.Month;
                mySqlCommandSel.Parameters["@cliente"].Value = cliente;
                mySqlCommandSel.Parameters["@libranza"].Value = libranza;
                mySqlCommandSel.Parameters["@asesor"].Value = asesor;

                #endregion

                DTO_ccSolicitudes doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    doc = new DTO_ccSolicitudes(dr);
                    result.Add(doc);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Solicitudes");
                throw exception;
            }
        }

        #endregion

        #region Venta Cartera

        #region Resumen
        public List<DTO_ccVentaCartera> DAL_ReportesCartera_Cc_VentaCartera(DateTime periodo, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida)
        {
            List<DTO_ccVentaCartera> result = new List<DTO_ccVentaCartera>();


            try
            {
                #region CommanText

                SqlCommand mySqlCommandSel = new SqlCommand("Cartera_RepVentaCartera", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, 5);
                mySqlCommandSel.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@MesINI", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@MesFIN", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Oferta", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);

                #endregion
                #region Asignacion Valores a Parametros

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = comprador;
                mySqlCommandSel.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;
                mySqlCommandSel.Parameters["@MesINI"].Value = fechaIni; //MODIFICAR
                mySqlCommandSel.Parameters["@MesFIN"].Value = fechaFin; //MODIFICAR
                mySqlCommandSel.Parameters["@Oferta"].Value = oferta;
                mySqlCommandSel.Parameters["@Periodo"].Value = periodo;

                #endregion

                DTO_ccVentaCartera venta = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    venta = new DTO_ccVentaCartera(dr, isResumida);
                    result.Add(venta);
                }
                dr.Close();

                return result;
            }

            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_VentaCartera");
                throw exception;
            }

        }
        #endregion

        #region Detalle

        /// <summary>
        /// Trae el detalle en la vista de consulta
        /// </summary>
        /// <param name="numDoc">numero de documento</param>
        /// <returns></returns>
        public List<DTO_ccVentaCarteraVista> DAL_ReportesCartera_Cc_VentaCarteraVista(int numDoc)
        {
            try
            {
                List<DTO_ccVentaCarteraVista> results = new List<DTO_ccVentaCarteraVista>();

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                mySqlCommandSel.CommandText = " SELECT  * FROM Vista_VentaCarteraDetalle where NumeroDoc = @NumeroDoc ";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", DbType.Int32);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numDoc;

                DTO_ccVentaCarteraVista doc = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    doc = new DTO_ccVentaCarteraVista(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_VentaCarteraVista");
                throw exception;
            }
        }
        #endregion
        #endregion

        #region Reporte ComponenteMvto(Contabilidad)

        /// <summary>
        /// Funcion que se encarga de traer los datos para el formato F19
        /// </summary>
        /// <param name="Periodo">Perido a Consultar</param>
        /// <param name="Formato">Tipo de Formato que desea Exportar</param>
        /// <returns></returns>
        public DataTable DAL_ReportesCartera_Cc_AnalisisPagosExcel(byte tipoReporte, string cliente, int? libranza, string compCap, string compINT, string compSEG, string compINTS, string compMOR, string compPRJ, string compSFV)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();

                #region Configuracion de Filtros
                // Carga los filtros de acuerdo a la parametrizacion del usuario
                string filtros = "  crd.EmpresaID =  @EmpresaID  ";
                if (tipoReporte == 2)
                    filtros += " AND dmv.DocumentoID <> " + AppDocuments.LiquidacionCredito.ToString();               
                if (!string.IsNullOrEmpty(cliente))
                    filtros += " AND crd.ClienteID = @ClienteID  ";
                if (!string.IsNullOrEmpty(libranza.ToString()))
                    filtros += " AND crd.Libranza = @Libranza  ";
                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompCAP", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompINT", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompSEG", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompINS", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompMOR", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompPRJ", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompFAV", SqlDbType.Char, UDT_ComponenteCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@AbonoInd", SqlDbType.Bit); 
                #endregion
                #region Asignacion Valores a Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                mySqlCommandSel.Parameters["@CompCAP"].Value = compCap;
                mySqlCommandSel.Parameters["@CompINT"].Value = compINT;
                mySqlCommandSel.Parameters["@CompSEG"].Value = compSEG;
                mySqlCommandSel.Parameters["@CompINS"].Value = compINTS;
                mySqlCommandSel.Parameters["@CompMOR"].Value = compMOR;
                mySqlCommandSel.Parameters["@CompPRJ"].Value = compPRJ;
                mySqlCommandSel.Parameters["@CompFAV"].Value = compSFV;
                mySqlCommandSel.Parameters["@AbonoInd"].Value = tipoReporte == 2? true : false;
                #endregion
                #region CommanText

                mySqlCommandSel.CommandText =
                " SELECT res.numeroDoc, dmv.DocumentoID, dmv.PrefijoID, dmv.DocumentoNro, dmv.ComprobanteID, dmv.ComprobanteIDNro, " +
	            "        dmv.FechaDoc, rca.FechaAplica,  " +
	            "        (case when (dmv.DocumentoID = '166' OR dmv.DocumentoID = '167' OR dmv.DocumentoID = '168') " +
		        "          then (case when rca.FechaConsignacion IS null then dmv.Fecha else rca.FechaConsignacion end) " +
		        "          else '' end ) as FechaConsigna, " +
	            "        res.NumCredito, crd.Libranza, dmv.Descripcion, dmv.Observacion, " +
	            "        res.VlrCapital, res.VlrInteres, res.VlrSeguro, res.VlrOtrCuota, res.VlrMora, res.VlrPrejuridico, res.SdoFavor, res.VlrGastos, res.VlrExtra , " +
	            "        cliente.ClienteID, cliente.Descriptivo as Nom_Cliente,cliente.EstadoCartera, crd.CompradorCarteraID,comprador.Descriptivo as NombreCompra " +
	            "     from  " +
	            "      ( " +
	            "       select mov.NumeroDoc, mov.NumCredito, " +
	            "        sum((case when mov.ComponenteCarteraID = @CompCAP  " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrCapital, " +
	            "        sum((case when mov.ComponenteCarteraID = @CompINT " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrInteres, " +
	            "        sum((case when mov.ComponenteCarteraID = @CompSEG OR mov.ComponenteCarteraID = @CompINS " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrSeguro, " +
	            "        sum((case when com.TipoComponente = 4 and mov.ComponenteCarteraID <> @CompCAP and mov.ComponenteCarteraID <> @CompINT " +
				"                  and mov.ComponenteCarteraID <> @CompSEG and mov.ComponenteCarteraID <> @CompINS      " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrOtrCuota, " +
	            "        sum((case when mov.ComponenteCarteraID = @CompMOR " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrMora, " +
	            "        sum((case when mov.ComponenteCarteraID = @CompPRJ " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrPrejuridico, " +
	            "        sum((case when mov.ComponenteCarteraID = @CompFAV " +
		        "           then(case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as SdoFavor, " +
	            "        sum((case when (com.TipoComponente = 6)  " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrGastos, " +
	            "        sum((case when (com.TipoComponente = 5) and mov.ComponenteCarteraID <> @CompMOR and mov.ComponenteCarteraID <> @CompPRJ " +
				" 	                   and mov.ComponenteCarteraID <> @CompFAV " +
		        "           then (case when @AbonoInd = 0 then mov.VlrComponente else mov.VlrAbono end) else 0 end)) as VlrExtra " +
	            "      from ccCarteraMvto mov " +
	            "       left join ccCreditoDocu crd On mov.Numcredito   = crd.NumeroDoc " +
	            "       left join ccCarteraComponente com on mov.ComponenteCarteraID = com.ComponenteCarteraID and mov.eg_ccCarteraComponente = com.EmpresaGrupoID  " +
	            "      where crd.EmpresaID = @EmpresaID and ((@Libranza is null) or (crd.Libranza=@Libranza)) " +
	            "      group by mov.NumeroDoc, mov.NumCredito " +
	            "      ) as Res  " +
	            "      left join glDocumentoControl dmv on res.Numerodoc = dmv.NumeroDoc  " +
	            "      left join glDocumentoControl dcr on res.Numcredito = dcr.NumeroDoc " +
	            "      left join ccCreditoDocu  crd On res.Numcredito = crd.NumeroDoc " +
	            "      left join tsReciboCajaDocu  rca on res.NumeroDoc = rca.NumeroDoc " +
	            "      left join ccCliente cliente WITH(NOLOCK) ON cliente.ClienteID = crd.ClienteID AND cliente.EmpresaGrupoID = crd.eg_ccCliente " +
	            "      left join ccCompradorCartera comprador WITH(NOLOCK) ON comprador.CompradorCarteraID = crd.CompradorCarteraID AND comprador.EmpresaGrupoID = crd.eg_ccCompradorCartera " +
	            "     where  " + filtros ;

                #endregion

                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                sda.Fill(table);  
                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_AnalisisPagosExcel");
                throw exception;
            }
        }
        #endregion

        #region Reporte Cobro Juridico

        /// <summary>
        /// Trae cobros juridicos de cartera para informe
        /// </summary>
        /// <param name="claseDeuda">Tipo de deuda del cliente</param>
        /// <returns>Lista de cobros jur</returns>
        public List<DTO_ReporCobroJuridico> DAL_ReportesCartera_Cc_CobroJuridicoGet(byte claseDeuda, string cliente, string obligacion)
        {
            try
            {
                List<DTO_ReporCobroJuridico> results = new List<DTO_ReporCobroJuridico>();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                mySqlCommandSel.CommandText = "Select	 his.NumeroDoc, " +
                                              "          his.TipoMvto, " +
                                              "          his.FechaMvto," +
                                              "          his.SaldoCapital as VlrCapital," +
                                              "          his.FechaInicial, " +
                                              "          his.FechaFinal, " +
                                              "          his.Dias as DiasMora, " +
                                              "          his.PorInteres as Tasa," +
                                              "          (CASE WHEN his.MvtoInteres is null THEN 0 ELSE his.MvtoInteres End) as InteresMora" +
                                              "  From	 ccCJHistorico as his" +
                                              "  inner join ccCreditoDocu as cred on his.NumeroDoc = cred.NumeroDoc" +
                                              "  Where	 cred.EmpresaID = @EmpresaID and  TipoMvto = 2 or TipoMvto = 3 " +
                                              "           and ((@ClienteID is null) or (cred.ClienteID=@ClienteID))" +
                                              "           and ((@Libranza is null) or (cred.Libranza=@Libranza))" +
                                              "  Order by NumeroDoc, FechaMvto ";
                #region Parametros
                mySqlCommandSel.Parameters.Add("@ClaseDeuda", DbType.Byte);
                mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommandSel.Parameters["@ClaseDeuda"].Value = claseDeuda;
                mySqlCommandSel.Parameters["@ClienteID"].Value = string.IsNullOrEmpty(cliente)?null : cliente ;
                mySqlCommandSel.Parameters["@Libranza"].Value = string.IsNullOrEmpty(obligacion) ? null : obligacion;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();

                while (dr.Read())
                {
                    DTO_ReporCobroJuridico doc = new DTO_ReporCobroJuridico(dr);
                    results.Add(doc);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_VentaCarteraVista");
                throw exception;
            }
        }

        #endregion

        #region Excel

        /// <summary>
        /// Obtiene un datatable con la info de cartera segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="zonaID">Zona</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="ConcesionarioID">Concecionario</param>
        /// <param name="asesor">Asesor</param>
        /// <param name="lineaCredi">LineaCredito</param>
        /// <param name="compCartera">CompradorCartera</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_ReportesCartera_Cc_CarteraToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string ConcesionarioID, string asesor,
                                                                string lineaCredi, string compCartera, string pagaduria, string centroPago, byte? Agrupamiento, byte? Romp,object filter,string usuarioID)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                
                #region Saldos Cartera
                if (documentoID == AppReports.ccReportesCartera)
                {
                    #region Configuracion de Filtros
                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = "";
                    if (!string.IsNullOrEmpty(cliente))
                        filtros += "AND crd.ClienteID = @ClienteID  ";
                    if (!string.IsNullOrEmpty(libranza.ToString()))
                        filtros += " AND crd.Libranza = @Libranza  ";
                    if (!string.IsNullOrEmpty(zonaID))
                        filtros += " AND crd.ZonaID = @ZonaID  ";
                    if (!string.IsNullOrEmpty(ciudad))
                        filtros += " AND crd.Ciudad = @Ciudad  ";
                    if (!string.IsNullOrEmpty(ConcesionarioID))
                        filtros += " AND crd.ConcesionarioID = @ConcesionarioID ";
                    if (!string.IsNullOrEmpty(asesor))
                        filtros += " AND crd.AsesorID = @AsesorID ";
                    if (!string.IsNullOrEmpty(lineaCredi))
                        filtros += " AND crd.LineaCreditoID = @LineaCreditoID ";
                    if (!string.IsNullOrEmpty(compCartera))
                        filtros += " AND crd.CompradorCarteraID = @CompradorCarteraID ";
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    mySqlCommandSel.Parameters["@ZonaID"].Value = zonaID;
                    mySqlCommandSel.Parameters["@Ciudad"].Value = ciudad;
                    mySqlCommandSel.Parameters["@ConcesionarioID"].Value = ConcesionarioID;
                    mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;
                    mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compCartera;
                    mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni != null ? fechaIni.Value.Date : fechaIni;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                    #endregion
                    #region CommanText

                    if (tipoReporte != 1) //SALDOS
                    {
                        mySqlCommandSel.CommandText =
                            "SELECT res.NumeroDoc," +
                            "		res.ConcesionarioID," +
                            "		res.AsesorID," +
                            "		res.LineaCreditoID," +
                            "		res.PagaduriaID," +
                            "		res.CompradorCarteraID, " +
                            "		res.Obligacion, " +
                            "		res.Cliente, " +
                            "		res.Nombre, " +
                            "		res.SdoCapital," +
                            "       res.SdoInteres, " +
                            "        res.SdoCapSegu, " +
                            "        res.SdoIntSegu " +
                            "from" +
                            "(" +
                            "SELECT	(case when  crd.TipoEstado in(3,4,5) then '   ' else " +
                            "		(case when  crd.CompradorCarteraID is null then '000' else rtrim(crd.CompradorCarteraID) end) end) as CompradorCarteraID," +
                            "		crd.Libranza	as Obligacion," +
                            "		crd.ClienteID	as Cliente," +
                            "		(case when crd.TipoEstado<=2 then '1' else '0' end) as TipoEstado," +
                            "		crd.LineaCreditoID," +
                            "		crd.ConcesionarioID," +
                            "		crd.AsesorID," +
                            "		crd.PagaduriaID," +
                            "		cli.Descriptivo as Nombre," +
                            "		pla.NumeroDoc," +
                            "		sum(pla.vlrCapital -		 (case when (pag.AboCapital is null) then 0 else pag.AboCapital end)) as SdoCapital," +
                            "		sum(Pla.VlrInteres -		 (case when (pag.AboInteres is null) then 0 else pag.AboInteres end)) as SdoInteres," +
                            "		sum(Pla.VlrSeguro  -		 (case when (pag.AboCapSegu is null) then 0 else pag.AboCapSegu end)) as SdoCapSegu," +
                            "		sum(Pla.VlrOtro1 -			 (case when (pag.AboIntSegu is null) then 0 else pag.AboIntSegu end)) as SdoIntSegu," +
                            "		sum(pla.VlrOtrosfijos -		 (case when (pag.AboOtrFijo is null) then 0 else pag.AboOtrFijo end)) as SdoOtrFijo," +
                            "		sum(Pla.VlrCapitalCesion -	 (case when (pag.AboCapCesa is null) then 0 else pag.AboCapCesa end)) as SdoCapCesa," +
                            "		sum(Pla.VlrUtilidadCesion -  (case when (pag.AboUtiCesa is null) then 0 else pag.AboUtiCesa end)) as SdoUtiCesa," +
                            "		sum(Pla.VlrderechosCesion -	 (case when (pag.AboDerCesa is null) then 0 else pag.AboDerCesa end)) as SdoDerechos" +
                            "		FROM ccCreditoPlanPagos (nolock)  as pla" +
                            "			left join ( SELECT	pag.CreditoCuotaNum," +
                            "								SUM(vlrCapital)			as AboCapital," +
                            "								SUM(VlrInteres)			as AboInteres," +
                            "								SUM(VlrSeguro)			as AboCapSegu," +
                            "								SUM(VlrOtro1)			as AboIntSegu," +
                            "								SUM(VlrOtrosfijos)		as AboOtrFijo," +
                            "								SUM(VlrCapitalCesion)   as AboCapCesa," +
                            "								SUM(VlrUtilidadCesion)	as AboUtiCesa," +
                            "								SUM(VlrDerechosCesion)	as AboDerCesa" +
                            "						FROM ccCreditoPagos (nolock) pag" +
                            "						group by pag.CreditoCuotaNum) as  pag ON pag.CreditoCuotaNum = pla.consecutivo" +
                            "			left join ccCreditoDocu (nolock)as crd on crd.NumeroDoc = pla.NumeroDoc	" +
                            "			left join ccCliente (nolock) as cli on  crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = @EmpresaID" +
                            "		where crd.EmpresaID = @EmpresaID" +
                            "			  and crd.TipoEstado < 6 " + filtros +
                            "		group by crd.CompradorCarteraID" +
                            "				,crd.Libranza" +
                            "				,crd.ClienteID" +
                            "				,cli.EmpleadoCodigo" +
                            "				,crd.PagaduriaID" +
                            "				,crd.TipoEstado" +
                            "				,crd.LineaCreditoID" +
                            "				,crd.AsesorID" +
                            "				,crd.ConcesionarioID" +
                            "				,cli.Descriptivo" +
                            "				,pla.NumeroDoc" +
                            "		) Res " +
                            "		where SdoCapital > 0 or SdoCapSegu > 0" +
                            "		order by CompradorCarteraID, nombre, Obligacion ";                                     
                    }
                    else //CUOTA
                    {
                        mySqlCommandSel.CommandText =
                            "SELECT crd.Libranza, crd.ClienteID,cliente.Descriptivo as Nombre ,  " +
                            "       pp.FechaCuota,pp.CuotaID,sum(pg.VlrCapital+pg.VlrInteres+pg.VlrSeguro+pg.VlrOtro1) as VlrCuota, " +
                            "       sum(pg.VlrCapital) as CapitalAbo,    " +
                            "       sum(pg.VlrInteres) as InteresAbo,  " +
                            "       SUM(pg.VlrSeguro) as SeguCapAbo,  " +
                            "       SUM(pg.VlrOtro1)  as SeguIntAbo,  " +
                            "       pp.VlrSaldoCapital as SaldoCapital  " +
                            "FROM ccCreditoDocu crd " +
                            "   inner join ccCreditoPlanPagos pp on crd.NumeroDoc = pp.NumeroDoc  " +
                            "   inner join ccCreditoPagos pg on pg.CreditoCuotaNum = pp.Consecutivo  " +
                            "   inner join ccCliente cliente on cliente.ClienteID = crd.ClienteID  " +
                            "WHERE  crd.EmpresaID = @EmpresaID " + filtros +
                            "GROUP BY pp.FechaCuota, pp.CuotaID,pp.VlrSaldoCapital,crd.NumeroDoc, crd.Libranza, crd.ClienteID,cliente.Descriptivo,pp.VlrCapital, pp.VlrInteres, pp.VlrSeguro, pp.VlrOtro1 ";
                    }

                    #endregion
                }
                #endregion
                #region Analisis Pagos
                else if (documentoID == AppReports.ccRepAnalisisPagos)
                {
                    #region Configuracion de Filtros

                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = " AND ctrlcompMvto.FechaDoc <= @FechaFin  ";
                    if (tipoReporte == 2)
                        filtros += " AND ctrlcompMvto.DocumentoID = " + AppDocuments.LiquidacionCredito.ToString();
                    else if (tipoReporte == 3)
                        filtros += " AND (ctrlcompMvto.DocumentoID = " + AppDocuments.RecaudosMasivos.ToString() + "  or ctrlcompMvto.DocumentoID = " + AppDocuments.RecaudosManuales.ToString();
                    else if (tipoReporte == 4)
                        filtros += " AND ctrlcompMvto.DocumentoID = " + AppDocuments.RecaudosMasivos.ToString() + " and compMvto.CompLocal1 >= 0  ";
                    else if (tipoReporte == 5)
                        filtros += " AND ctrlcompMvto.DocumentoID = " + AppDocuments.RecaudosManuales.ToString();
                    else if (tipoReporte == 6)
                        filtros += " AND ctrlcompMvto.DocumentoID = " + AppDocuments.PagosTotales.ToString();
                    else if (tipoReporte == 7)
                        filtros += " AND ctrlcompMvto.DocumentoID = " + AppDocuments.LiquidacionCredito.ToString();
                    else if (tipoReporte == 8)
                        filtros += " AND ctrlcompMvto.DocumentoID = " + AppDocuments.RecaudosMasivos.ToString() + " and compMvto.CompLocal1 < 0  ";
                    if (!string.IsNullOrEmpty(cliente))
                        filtros += " AND cred.ClienteID = @ClienteID  ";
                    if (!string.IsNullOrEmpty(libranza.ToString()))
                        filtros += " AND cred.Libranza = @Libranza  ";
                    if (!string.IsNullOrEmpty(zonaID))
                        filtros += " AND cred.ZonaID = @ZonaID  ";
                    if (!string.IsNullOrEmpty(ciudad))
                        filtros += " AND cred.Ciudad = @Ciudad  ";
                    if (!string.IsNullOrEmpty(ConcesionarioID))
                        filtros += " AND cred.ConcesionarioID = @ConcesionarioID ";
                    if (!string.IsNullOrEmpty(asesor))
                        filtros += "  AND cred.AsesorID = @AsesorID ";
                    if (!string.IsNullOrEmpty(lineaCredi))
                        filtros += "  AND cred.LineaCreditoID = @LineaCreditoID ";
                    if (!string.IsNullOrEmpty(compCartera))
                        filtros += " AND cred.CompradorCarteraID = @CompradorID ";

                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@FechaIni", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.DateTime);

                    #endregion
                    #region Asignacion Valores a Parametros

                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    mySqlCommandSel.Parameters["@ZonaID"].Value = zonaID;
                    mySqlCommandSel.Parameters["@Ciudad"].Value = ciudad;
                    mySqlCommandSel.Parameters["@ConcesionarioID"].Value = ConcesionarioID;
                    mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;
                    mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compCartera;
                    mySqlCommandSel.Parameters["@FechaIni"].Value = fechaIni;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = fechaFin;
                    #endregion
                    #region CommanText
                    mySqlCommandSel.CommandText =
                     "SELECT cred.Libranza,ctrlcompMvto.FechaDoc,ctrlcompMvto.FechaDoc as FechaCont,ctrlcompMvto.DocumentoID,Cast(RTrim(ctrlcompMvto.ComprobanteID)+'-'+Convert(Varchar, ctrlcompMvto.ComprobanteIDNro) as Varchar(100)) as Comprobante, " +
                     "       compMvto.CompLocal1,compMvto.CompLocal2,compMvto.CompLocal3,compMvto.CompLocal4,compMvto.CompLocal7,compMvto.CompLocal8,compMvto.CompLocal9, " +
                     "       cred.NumeroDoc,cliente.ClienteID, cliente.Descriptivo as Nombre,cred.LineaCreditoID,lineaCred.Descriptivo " +
                     "FROM    coComponenteMvto compMvto WITH(NOLOCK)  " +
                     "        INNER JOIN glDocumentoControl ctrlcompMvto WITH(NOLOCK) ON ctrlcompMvto.NumeroDoc = compMvto.NumeroDoc   " +
                     "        INNER JOIN ccCreditoDocu cred WITH(NOLOCK) ON cred.NumeroDoc = compMvto.IdentificadorTR  " +
                     "        INNER JOIN ccCliente cliente WITH(NOLOCK) ON cliente.ClienteID = cred.ClienteID AND cliente.EmpresaGrupoID = cred.eg_ccCliente " +
                     "        INNER JOIN ccLineaCredito lineaCred WITH(NOLOCK) ON lineaCred.LineaCreditoID = cred.LineaCreditoID  " +
                     "WHERE	compMvto.EmpresaID = @EmpresaID " + filtros +
                     "GROUP BY cred.Libranza,ctrlcompMvto.FechaDoc,ctrlcompMvto.FechaDoc,ctrlcompMvto.DocumentoID,ctrlcompMvto.ComprobanteID, ctrlcompMvto.ComprobanteIDNro, " +
                     "    compMvto.CompLocal1,compMvto.CompLocal2,compMvto.CompLocal3,compMvto.CompLocal4,compMvto.CompLocal7,compMvto.CompLocal8,compMvto.CompLocal9, " +
                     "    cred.NumeroDoc,cliente.ClienteID, cliente.Descriptivo,cred.LineaCreditoID,lineaCred.Descriptivo ";

                    #endregion
                }
                #endregion
                #region Proyeccion y Vencimiento Pagos
                else if (documentoID == AppReports.ccProyeccionVencim)
                {
                    #region Configuracion de Filtros
                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = "";
                    if (!string.IsNullOrEmpty(cliente))
                        filtros += "AND cred.ClienteID = @ClienteID  ";
                    if (!string.IsNullOrEmpty(libranza.ToString()))
                        filtros += " AND cred.Libranza = @Libranza  ";
                    if (!string.IsNullOrEmpty(zonaID))
                        filtros += " AND cred.ZonaID = @ZonaID  ";
                    if (!string.IsNullOrEmpty(ciudad))
                        filtros += " AND cred.Ciudad = @Ciudad  ";
                    if (!string.IsNullOrEmpty(ConcesionarioID))
                        filtros += " AND cred.ConcesionarioID = @ConcesionarioID ";
                    if (!string.IsNullOrEmpty(asesor))
                        filtros += " AND cred.AsesorID = @AsesorID ";
                    if (!string.IsNullOrEmpty(lineaCredi))
                        filtros += " AND cred.LineaCreditoID = @LineaCreditoID ";
                    if (!string.IsNullOrEmpty(compCartera))
                        filtros += " AND cred.CompradorCarteraID = @CompradorID ";
                    #endregion
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ZonaID", SqlDbType.Char, UDT_ZonaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Ciudad", SqlDbType.Char, UDT_LugarGeograficoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@ConcesionarioID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                    mySqlCommandSel.Parameters.Add("@AsesorID", SqlDbType.Char, UDT_AsesorID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@LineaCreditoID", SqlDbType.Char, UDT_LineaCreditoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    mySqlCommandSel.Parameters["@ZonaID"].Value = zonaID;
                    mySqlCommandSel.Parameters["@Ciudad"].Value = ciudad;
                    mySqlCommandSel.Parameters["@ConcesionarioID"].Value = ConcesionarioID;
                    mySqlCommandSel.Parameters["@AsesorID"].Value = asesor;
                    mySqlCommandSel.Parameters["@LineaCreditoID"].Value = lineaCredi;
                    mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compCartera;
                    #endregion
                    #region CommanText
                    if (tipoReporte == 1) //Vencimiento
                    {
                        mySqlCommandSel.CommandText =
                        "SELECT	cred.ClienteID,cli.Descriptivo as Nombre,pla.NumeroDoc,cred.Libranza,	" +
                        "        pla.CuotaID,pla.FechaCuota, pla.VlrCuota		" +
                        "FROM ccCreditoPlanPagos as pla	" +
                        "    INNER JOIN ccCreditoDocu as cred on cred.NumeroDoc = pla.NumeroDoc	 " +
                        "    INNER JOIN ccCliente as cli on  cred.ClienteID = cli.ClienteID	 " +
                        "    WHERE	cred.EmpresaID =@EmpresaID " +
                        "        and ((@ClienteID is null) or (cred.ClienteID=@ClienteID or cred.ClienteID=@ClienteID)) " +
                        "        and ((@Libranza is null) or (cred.Libranza=@Libranza)) " +
                        "        and ((@ZonaID is null) or (cred.ZonaID=@ZonaID)) " +
                        "        and ((@Ciudad is null) or (cred.Ciudad=@Ciudad)) " +
                        "        and ((@ConcesionarioID is null) or (cred.ConcesionarioID=@ConcesionarioID)) " +
                        "        and ((@AsesorID is null) or (cred.AsesorID=@AsesorID)) " +
                        "        and ((@LineaCreditoID is null) or (cred.LineaCreditoID=@LineaCreditoID)) " +
                        "        and ((@CompradorCarteraID is null) or (cred.CompradorCarteraID=@CompradorCarteraID)) " +
                        "        and ((@FechaIni  is null) or (pla.FechaCuota >= @FechaIni)) " +
                        "        and pla.FechaCuota between GETDATE() and  GETDATE()+5 " +
                        "ORDER by pla.FechaCuota, cred.ClienteID   ";
                    }
                    else //Proyeccion Pagos
                    {
                        mySqlCommandSel = new SqlCommand("Cartera_ReportProyeccionPagos", this.MySqlConnection.CreateCommand().Connection);
                        mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                    }
                    #endregion
                } 
                #endregion
                #region Amortizacion
                else if (documentoID == AppReports.ccAmortizacion)
                {
                    #region Configuracion de Filtros
                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = "";
                    if (!string.IsNullOrEmpty(cliente))
                        filtros += "AND cred.ClienteID = @ClienteID  ";
                    if (!string.IsNullOrEmpty(libranza.ToString()))
                        filtros += " AND cred.Libranza = @Libranza  ";
                    #endregion

                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    #endregion

                    #region CommanText
 
                        mySqlCommandSel.CommandText =
                                                       "SELECT		VlrCuota, FechaCuota,  "+
			                                           "            VlrCapital,	VlrInteres,  "+
			                                           "            VlrSeguro, VlrOtrosfijos,  "+
			                                           "            sum (CASE WHEN  ([CAPITAL] IS NULL) THEN 0 ELSE [CAPITAL]  END) Capital,   "+
			                                           "            sum (CASE WHEN  ([APORTES] IS NULL) THEN 0 ELSE [APORTES] END)  Aporte  "+
                                                       "FROM (SELECT	CrComp.Descriptivo,	  "+
				                                       "                credDoc.ClienteID,			  "+
				                                       "                crcomp.ComponenteCarteraID,  "+
				                                       "                CrPP.NumeroDoc,   "+
				                                       "                Crpp.VlrCuota,  "+
				                                       "                CrCmp.CuotaValor,  "+
				                                       "                CrPP.FechaCuota,  "+
				                                       "                CrPP.VlrCapital,  "+
				                                       "                CrPP.VlrInteres,  "+
				                                       "                credDoc.EmpresaID,  "+
				                                       "                CrPP.VlrSeguro,  "+
				                                       "                CrPP.VlrOtrosfijos  "+
                                                       "FROM	ccCreditoPlanPagos AS CrPP		  "+
                                                       "inner join ccCreditoComponentes AS CrCmp on CrPP.NumeroDoc = CrCmp.NumeroDoc  "+
                                                       "inner join ccCreditoDocu AS credDoc on credDoc.NumeroDoc = CrCmp.NumeroDoc  "+
                                                       "inner join ccCarteraComponente as CrComp on CrComp.ComponenteCarteraID = CrCmp.ComponenteCarteraID	  "+
                                                       "WHERE credDoc.EmpresaID = @EmpresaID   "+
	                                                   "        and((@ClienteID is null) or (credDoc.ClienteID=@ClienteID))  "+
	                                                   "    	and ((@Libranza is null) or (credDoc.Libranza=@Libranza))) AS TablaPivot  "+
                                                       "PIVOT	(avg(CuotaValor)  "+
                                                       "FOR Descriptivo IN ([CAPITAL], [APORTES])) AS PVT  "+
                                                       "group by	VlrCuota, FechaCuota,  "+
			                                           "            VlrCapital, VlrInteres,  "+
                                                       "            VlrSeguro, VlrOtrosfijos  " +
                                                       "ORDER BY	FechaCuota "; 

                    #endregion
                }
                #endregion
                #region SaldoCapital
                else if (documentoID == AppReports.ccSaldoCapital)
                {
                    #region Configuracion de Filtros
                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = "";
                    if (!string.IsNullOrEmpty(cliente))
                        filtros += "AND cred.ClienteID = @ClienteID  ";
                    if (!string.IsNullOrEmpty(libranza.ToString()))
                        filtros += " AND cred.Libranza = @Libranza  ";
                    #endregion

                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    //mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    //mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    #endregion

                    #region CommanText

                    mySqlCommandSel.CommandText =
                                                   "SELECT  Nombre,  "+
		                                           "        Cliente,  "+
		                                           "        Obligación,  "+
		                                           "        Periodo,  "+
		                                           "        NoDoc,  "+
		                                           "        sum (CASE WHEN ( [CAPITAL] IS NULL) THEN 0 ELSE Promedio END) Capital,  "+
		                                           "        sum (CASE WHEN ( [INTERESES] IS NULL) THEN 0 ELSE Promedio END) Interes,  "+
		                                           "        sum (CASE WHEN ( [CAPITAL] IS NULL) THEN 0 ELSE Promedio END + CASE WHEN ( [INTERESES] IS NULL) THEN 0 ELSE Promedio END) as CapitalSDO  "+
                                                   "FROM (select	distinct CMC.CapitalSDO,  "+
		                                           "        CMC.NumeroDoc AS NoDoc,  "+
		                                           "        CMC.Periodo,  "+
		                                           "        CrC.ComponenteCarteraID AS Comp,  "+
		                                           "        CaC.Descriptivo AS NombreComponente,  "+
		                                           "        T.Descriptivo AS Nombre,  "+
		                                           "        CD.ClienteID AS Cliente,  "+
		                                           "        CD.Libranza AS Obligación,  "+
		                                           "        CrC.PorCapital,  "+
		                                           "        cmc.VlrCuota,  "+
		                                           "        ((PorCapital*CapitalSDO)/100) AS Promedio  "+
                                                   "from ccCierreMesCartera AS CMC  "+
                                                   "INNER JOIN ccCreditoDocu AS CD on CMC.NumeroDoc = CD.NumeroDoc  "+
                                                   "INNER JOIN ccCreditoComponentes AS CrC ON CMC.NumeroDoc = CrC.NumeroDoc	"+
                                                   "INNER JOIN ccCarteraComponente AS CaC ON CrC.ComponenteCarteraID = CaC.ComponenteCarteraID  "+
                                                   "INNER JOIN coTercero AS T ON CD.ClienteID = T.TerceroID  "+
                                                   "WHERE	cd.EmpresaID = @EmpresaID  " +
		                                           "        and ((@ClienteID is null) or (CD.ClienteID=@ClienteID))  "+
		                                           "        and ((@Libranza is null) or (CD.Libranza=@Libranza))  "+
	                                               "    ) AS TABLAPVT  "+
                                                   "PIVOT (AVG (CapitalSDO) FOR NombreComponente IN ([CAPITAL], [INTERESES])) AS PVT  "+
                                                   "GROUP BY  NoDoc,  "+
		                                           "        Cliente,  "+
		                                           "        Nombre,  "+
		                                           "        Obligación,  "+
		                                           "        Periodo  "+
                                                   "order by NoDoc,  "+
		                                           "        Cliente,  "+
		                                           "        Nombre,  "+
		                                           "        Obligación,  "+
		                                           "        Periodo";

                    #endregion
                }
                #endregion
                #region PolizaEstado
                else if(documentoID == AppReports.ccPolizaEstado)
                {
                    #region Configuracion de Filtros
                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = "";
                    if (!string.IsNullOrEmpty(cliente))
                        filtros += "AND cred.ClienteID = @ClienteID  ";
                    if (!string.IsNullOrEmpty(libranza.ToString()))
                        filtros += " AND cred.Libranza = @Libranza  ";
                    #endregion

                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    //mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    //mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    #endregion

                    #region CommanText

                    mySqlCommandSel.CommandText =
                                                   "SELECT	    (CASE WHEN PolEst.ValorFinancia is null THEN 0 ELSE PolEst.ValorFinancia END) AS ValorFinanciacion,  "+
			                                       "            PolEst.TerceroID,  "+
			                                       "            Ter.Descriptivo,  "+
			                                       "            CreDoc.Libranza,  "+
			                                       "            PolEst.Poliza,  "+
			                                       "            PolEst.FechaVigenciaINI,  "+
			                                       "            PolEst.FechaVigenciaFIN,  "+
			                                       "            PolEst.VlrPoliza  "+
                                                   "FROM		ccPolizaEstado AS PolEst  "+
                                                   "INNER JOIN	coTercero AS Ter ON PolEst.TerceroID = Ter.TerceroID  "+
                                                   "INNER JOIN	ccCreditoDocu AS CreDoc ON PolEst.NumDocCredito=CreDoc.NumeroDoc  "+
                                                   "WHERE		CreDoc.EmpresaID = @EmpresaID  "+
                                                   "		    and((@ClienteID is null) or (CreDoc.ClienteID=@ClienteID))  " +
                                                   "		    and ((@Libranza is null) or (CreDoc.Libranza=@Libranza))";

                    #endregion
                }
                #endregion
                #region Recaudos Nomina Deta
                else if(documentoID == AppReports.ccRecaudosNominaDeta)
                {   
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@EstadoCruce", SqlDbType.Int);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@CentroPagoID"].Value = centroPago;
                    mySqlCommandSel.Parameters["@Periodo"].Value = fechaFin;
                    mySqlCommandSel.Parameters["@EstadoCruce"].Value = !string.IsNullOrEmpty(pagaduria)? pagaduria : null;
                    #endregion

                    #region CommanText

                    mySqlCommandSel.CommandText =
                            "SELECT cred.Libranza, cli.ClienteID as Cedula, cli.Descriptivo as Nombre, " +
                            " SUM(nom.valorNomina) AS ValorNomina,   " +
                            "  CASE WHEN nom.EstadoCruce = 1 then 'Cruce correcto' 	 " +
                            "           else case when nom.EstadoCruce = 2 then 'No Opero Inc. Previa'  " +
                            "             else case when nom.EstadoCruce = 3 then 'No Opero Inc. Liquidación'  " +
                            "              else case when nom.EstadoCruce = 4 then 'No Opero Desincorporación'  " +
                            "               else case when nom.EstadoCruce = 5 then 'Opero por Valor Diferente' " +
                            "                else case when nom.EstadoCruce = 6 then 'Dejo de Operar' " +
                            "                 else case when nom.EstadoCruce = 7 then 'Valor diferente' " +
                            "                  else case when nom.EstadoCruce = 8 then 'Pago Atrasado, ' " +
                            "                   else case when nom.EstadoCruce = 9 then 'Desc. Sin Saldo' " +
                            "                    else case when nom.EstadoCruce = 10 then 'Solicitud' " +
                           // "                    else case when nom.EstadoCruce = 11 then 'Opero Adelantado' " +
                            "         end end end end end end end end end end as EstadoCruce " +                          
                            "FROM ccNominaDeta nom with(nolock)   " +
                            "    INNER JOIN ccCreditoDocu cred with(nolock)  ON cred.NumeroDoc = nom.NumDocCredito   " +
                            "    INNER JOIN ccCliente cli with(nolock)  ON  cli.ClienteID = cred.ClienteID  and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                            "WHERE	cred.EmpresaID = @EmpresaID  " +
                            "        AND nom.CentroPagoID = @CentroPagoID   " +
                            "        AND Month(nom.FechaNomina) = Month(@Periodo)   " +
                            "        AND Year(nom.FechaNomina) = Year(@Periodo)  " +
                            "        AND ((@EstadoCruce is null) or (nom.EstadoCruce=@EstadoCruce)) " +
                            "GROUP BY Libranza, cli.ClienteID, cli.Descriptivo,nom.EstadoCruce " +
                            "ORDER BY cli.Descriptivo      ";               
                    #endregion
                }
                #endregion
                #region Reintegro Ajuste /Giro
                else if (documentoID == AppReports.ccSaldosAFavor)
                {
                    if (tipoReporte == 2 || tipoReporte == 3)
	                {
		                #region Configuracion de Filtros
                        string filtros = "";
                        bool pendienteInd = (bool)filter;
                        if(pendienteInd)
                            filtros += " AND ctrl.Estado = 2 ";
                        else
                            filtros += " AND ctrl.Estado = 3 ";
                        if (!string.IsNullOrEmpty(cliente))
                            filtros += " AND cred.ClienteID = @ClienteID  ";
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);

                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                        #endregion
                        #region CommanText

                        mySqlCommandSel.CommandText =
                                " SELECT reint.*, ctrl.CuentaID AS CuentaReintegro, cred.Libranza, cred.ClienteID, cli.Descriptivo as Nombre, " +
                                "       CASE WHEN ctrl.CuentaID is null THEN 'Giro' ELSE 'Ajuste' END as Tipo, ctrl.ComprobanteID, ctrl.FechaDoc, " +
                                "       reint.Valor,ctrl.Observacion,ctrl.seUsuarioID,ctrl.Fecha  " +
                                " FROM ccReintegroClienteDeta reint WITH(NOLOCK)    " +
                                "    INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) on reint.NumeroDoc = ctrl.NumeroDoc " +
                                "    left join ccCreditoDocu cred with(nolock) on reint.NumDocCredito = cred.NumeroDoc   " +
                                "    left join ccCliente cli with(nolock) on cli.ClienteID = cred.ClienteID AND cli.EmpresaGrupoID = cred.eg_ccCliente " +
                                " WHERE ctrl.EmpresaID = @EmpresaID " + filtros;
                        #endregion 
	                }
                }
                #endregion
                #region ESPECIALES               
                else if(documentoID == AppReports.ccCarteraEspeciales)
                {
                    #region SaldoSeguro
                    if (tipoReporte == 4) 
                    {
                        #region Configuracion de Filtros
                        // Carga los filtros de acuerdo a la parametrizacion del usuario
                        string filtros = "";
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            filtros += " AND cli.ClienteID = @ClienteID  ";
                            mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                            mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                        }
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaIni;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                        "    DECLARE @Fecha1					AS DATE  " +
                        "    DECLARE @Fecha2					AS DATE  " +
                        "    SET @Fecha1                        = '20171101'	" +
                        "    SET @Fecha2                        = '20171201' 	" +
                        "    DECLARE @TasaC1					as numeric(10,4)	" +
                        "    DECLARE @TasaC2					AS numeric(10,4)	" +
                        "    DECLARE @TasaD1					as numeric(10,4)	" +
                        "    DECLARE @TasaD2					AS numeric(10,4)	" +
                        "    SELECT	@TasaD1 = Tasa4, @TasaC1 = Tasa5 FROM	glDatosMensuales WHERE   cast(Periodo as date) = @Fecha1	" +
                        "    SELECT	@TasaD2 = Tasa4, @TasaC2 = Tasa5 FROM	glDatosMensuales WHERE   cast(Periodo as date) = @Fecha2	" +
                        " SELECT	tipo, Mes, Cliente, CedConyugue, Nombre, FechaNacimiento, (PorSeguro1+PorSeguro2) as PorSeguro, PorcExtra, CapitalSdo,	" +
                        "		Round((CapitalSDO * (PorSeguro1+PorSeguro2)/100),0) as VlrSeguro,	" +
                        "		Round(((CapitalSDO* (PorSeguro1+PorSeguro2)/100)*PorcExtra/100),0) as VlrSeguroExtra,	" +
                        "		Round((CapitalSDO * (PorSeguro1+PorSeguro2)/100)+((CapitalSDO*(PorSeguro1+PorSeguro2)/100)*PorcExtra/100),0) as VlrTotal	" +
                        "FROM	" +
                        "(	" +
                        "	(	" +
                        "         Select 'D' as Tipo, Cast(year(doc.PeriodoDoc) as varchar(4)) + '-' + Cast(month(doc.PeriodoDoc) as varchar(4)) as Mes,	" +
                        "				CAST(cli.ClienteID as bigint) as Cliente, cli.ClienteID as CedConyugue, cli.Descriptivo as Nombre,cli.FechaNacimiento,	" +
                        "				(case when cast(doc.PeriodoDoc as date) <= @Fecha1 then @TasaD1 else 0 end) as PorSeguro1,	" +
                        "				(case when cast(doc.PeriodoDoc as date) >= @Fecha2 then @TasaD2 else 0 end) as PorSeguro2,	" +
                        "				ISNULL(cli.ExtraPrimaCliente,0) as PorcExtra,	" +
                        "				SUM(cie.CapitalSDO) as CapitalSDO	" +
                        "         From ccCierremescartera cie	" +
                        "                inner join ccCreditoDocu	crd on cie.numerodoc = crd.NumeroDoc	" +
                        "                left join glDocumentoControl doc on crd.Numerodoc = doc.NumeroDoc	" +
                        "                inner join ccCliente cli on crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente	" +
                        "          Where CAST(cie.Periodo as DATE) = @Periodo and crd.TipoEstado <= 2	" +
                        "          Group  by doc.PeriodoDoc, cli.ClienteID,cli.Descriptivo,cli.FechaNacimiento,cli.ExtraPrimaCliente	" +
                        "    )		" +
                        "    Union	" +
                        "    (	" +
                        "         Select 'C' as Tipo, Cast(year(doc.PeriodoDoc) as varchar(4)) + '-' + Cast(month(doc.PeriodoDoc) as varchar(4)) as Mes,	" +
                        "				 CAST(cli.ClienteID as bigint) as ClienteID, cli.CedEsposa as CedConyugue, cli.NomEsposa as Nombre,cli.FechEsposa as FechaNacimiento,	" +
                        "				(case when doc.PeriodoDoc <= @Fecha1 then @TasaC1 else 0 end) as PorSeguro1,	" +
                        "				(case when doc.PeriodoDoc >= @Fecha2 then @TasaC2 else 0 end) as PorSeguro2,	" +
                        "				ISNULL(cli.ExtraPrimaConyugue,0) as PorcExtra,	" +
                        "                SUM(cie.CapitalSDO) as CapitalSDO	" +
                        "	       From ccCierremescartera cie 	" +
                        "                inner join ccCreditoDocu crd on cie.numerodoc = crd.NumeroDoc	" +
                        "                left join glDocumentoControl doc on crd.Numerodoc = doc.NumeroDoc	" +
                        "                inner join ccCliente cli on crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente and	" +
                        "											cli.CedEsposa is not null	" +
                        "          Where CAST(cie.Periodo as DATE) = @Periodo and crd.TipoEstado <= 2 and 	" +
                        "				(DATEDIFF(year,cli.FechEsposa,GETDATE()) <= 55 or cli.FechEsposa is null)	" +
                        "          Group  by doc.PeriodoDoc, cli.ClienteID,cli.CedEsposa,cli.NomEsposa,cli.FechEsposa,cli.ExtraPrimaConyugue	" +
                        "  )	" +
                        ")q where CapitalSDO > 0  order by  CLiente asc, Tipo desc	";
              
                        #endregion 
                    }
                    #endregion
                    #region Creditos Cancelados
                    else if (tipoReporte == 5)
                    {
                        #region Configuracion de Filtros
                        // Carga los filtros de acuerdo a la parametrizacion del usuario
                        string filtros = "";
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            filtros += " AND cli.ClienteID = @ClienteID  ";
                            mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                            mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                        }
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@PeriodoAnterior", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@PeriodoActual", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@PeriodoAnterior"].Value = fechaIni;
                        mySqlCommandSel.Parameters["@PeriodoActual"].Value = fechaFin;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                      "Select crd.Libranza,crd.ClienteID,cli.Descriptivo as Nombre, " +
                      "          (case when crd.Pagare IS null  then '' else crd.Pagare end) as Pagare, cast(pag.UltPago as DATE) as UltPago " +
                      "  from " +
                      "      ( " +
                      "      select NumeroDoc, max(PeriodoANT) as PeriodoANT, MAX(PeriodoACT) as PeriodoACT " +
                      "      from " +
                      "          ( " +
                      "          Select NumeroDoc, max(dateadd(month, 1, periodo)) as PeriodoANT, NULL as PeriodoACT " +
                      "          from ccCierreMesCartera with(nolock) " +
                      "          where EmpresaID = @EmpresaID and Periodo >= dateadd(month, -1, @PeriodoAnterior) and Periodo <= dateadd(month, -1, @PeriodoActual) " +
                      "          group by NumeroDoc " +
                      "          UNION " +
                      "          Select NumeroDoc, null as PeriodoANT, max(periodo) as PeriodoACT " +
                      "          from ccCierreMesCartera with(nolock) " +
                      "          where EmpresaID = @EmpresaID and Periodo >= @PeriodoAnterior and Periodo <= @PeriodoActual " +
                      "          group by NumeroDoc " +
                      "          ) mm " +
                      "       group by NumeroDoc " +
                      "       ) can " +
                      "      left Join ccCreditoDocu crd with(nolock)on can.NumeroDoc = crd.NumeroDoc " +
                      "       left join ccCliente cli on cli.ClienteID = crd.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente " +
                      "       left join " +
                      "           ( " +
                      "            select pag.numerodoc, max(doc.FechaDoc) as UltPago " +
                      "            from ccCreditoPagos pag  with(nolock) " +
                      "                 left join glDocumentoControl doc on pag.PagoDocu = doc.NumeroDoc " +
                      "            where cast(doc.PeriodoDoc as date) <= @PeriodoActual " +
                      "           group by pag.numerodoc " +
                      "             ) pag on can.NumeroDoc = pag.NumeroDoc " +
                      "   where(can.PeriodoANT > can.PeriodoACT or can.PeriodoACT is null) and crd.TipoEstado < 6  " + filtros + 
                      "   order by Nombre ";
                        #endregion
                    }
                    #endregion
                    #region Creditos Nuevos
                    else if (tipoReporte == 6)
                    {
                        #region Configuracion de Filtros
                        string filtros = "";
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            filtros += " AND cli.ClienteID = @ClienteID  ";
                            mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                            mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                        }
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@PeriodoAnterior", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@PeriodoActual", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@PeriodoAnterior"].Value = fechaIni;
                        mySqlCommandSel.Parameters["@PeriodoActual"].Value = fechaFin;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                      " Select  crd.Libranza,crd.ClienteID,cli.Descriptivo as Nombre, " +
                      "    (case when crd.Pagare IS null  then '' else crd.Pagare end) as Pagare " +
                      "  from ccCreditoDocu crd  " +
                      "  left join glDocumentoControl doc on crd.numerodoc = doc.numerodoc " +
                      "  left join ccCliente cli on cli.ClienteID = crd.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente " +
                      "  where crd.empresaID = @EmpresaID and doc.Estado = 3 and " +
                      "    doc.PeriodoDOC >= @PeriodoAnterior and doc.PeriodoDOC <= @PeriodoActual " + filtros +
                      "  order by Nombre ";
                        #endregion
                    }
                    #endregion
                    #region Polizas para Renovar
                    else if (tipoReporte == 7)
                    {
                        #region Configuracion de Filtros
                        string filtros = "";
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            filtros += " AND cli.ClienteID = @ClienteID  ";
                            mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                            mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                        }
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@AsesorSeg", SqlDbType.VarChar);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Fecha"].Value = fechaFin;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaFin;
                        mySqlCommandSel.Parameters["@AsesorSeg"].Value = "001";
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                        " Select * from ( " +
                        "  select crd.Libranza as Credito, cli.Descriptivo as Nombre,'     ' as Placa, ase.Descriptivo as Aseguradora, pol.Poliza, " +
                       "       pol.fechaVigenciaFin as FechaVto, " +
                       "       (case when pol.SegurosAsesorID = @AsesorSeg then 'R' " +
                       "      when pol.ColectivaInd = 1 then 'C' else 'I' end) as Tipo, " +
                       "    DATEDIFF(day, getdate(), pol.fechaVigenciaFin) as Dias, DATEDIFF(month, @Periodo, pol.fechaVigenciaFin) as Meses " +
                       "  from   ( " +
                       "   select NumDocCredito, MAX(fechaVigenciaFin) as fechaVigenciaFin " +
                       "   from ccPolizaEstado " +
                       "   where NumDocCredito is not null " +
                       "   group by NumDocCredito " +
                       "   ) as ult " +
                       "   inner join ccCierreDiaCartera cie on ult.NumDocCredito = cie.NumeroDoc " +
                       "   left join ccCreditoDocu crd on ult.NumDocCredito = crd.NumeroDoc " +
                       "   left join ccCliente cli on crd.ClienteID = cli.ClienteID and crd.eg_ccCliente = cli.EmpresaGrupoID " +
                       "   left join ccPolizaEstado pol on ult.NumDocCredito = pol.NumDocCredito and ult.fechaVigenciaFin = pol.fechaVigenciaFin " +
                       "   left join ccAseguradora ase on pol.AseguradoraID = ase.AseguradoraID and pol.eg_ccAseguradora = ase.EmpresaGrupoID " +
                       "  where cie.SaldoCapital > 0 and cie.fecha = @Fecha and   pol.FechaRevoca is null " +
                       "  ) pol " +
                       " where  Meses in (-1,0,1) " +
                       " order by FechaVto ";
                        #endregion
                    }
                    #endregion
                }
                #endregion
                #region ESTADO CESIONES               
                else if(documentoID == AppReports.ccEstadoCesionCartera)
                {                    
                    #region Amortizacion Derechos 
                    if (tipoReporte == 2) 
                    {
                        mySqlCommandSel = new SqlCommand("Cartera_ReportEstCuentaCesionResumen", this.MySqlConnection.CreateCommand().Connection);
                        #region Parametros
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@EmpresaID", this.Empresa.ID.Value));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@TerceroID", compCartera));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@Periodo", fechaIni));
                        #endregion
                        mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                    }
                    #endregion
                    #region Cesion o Recompra Mes
                    else if (tipoReporte == 3)
                    {
                        mySqlCommandSel = new SqlCommand("Cartera_ReportCesionRecompraMes", this.MySqlConnection.CreateCommand().Connection);
                        #region Parametros
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@EmpresaID", this.Empresa.ID.Value));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@ClienteID", cliente));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@Libranza", libranza));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@ZonaID", zonaID));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@Ciudad", ciudad));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@ConcesionarioID", ConcesionarioID));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@AsesorID", asesor));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@LineaCreditoID", lineaCredi));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@CompradorCarteraID", compCartera));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@FechaIni", fechaIni));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
                        #endregion
                        mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                    }
                    #endregion
                    #region Detalle Saldos 
                    else if (tipoReporte == 5)
                    {
                        #region Configuracion de Filtros
                        string filtros = "";
                        if (!string.IsNullOrEmpty(compCartera))
                        {
                            filtros += " AND com.TerceroID = @compCartera ";
                            mySqlCommandSel.Parameters.Add("@compCartera", SqlDbType.Char, UDT_TerceroID.MaxLength);
                            mySqlCommandSel.Parameters["@compCartera"].Value = compCartera;
                        }
                        if (!string.IsNullOrEmpty(cliente))
                        {
                            filtros += " AND CRD.ClienteID = @TerceroID ";
                            mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                            mySqlCommandSel.Parameters["@TerceroID"].Value = cliente;
                        }
                        if (libranza != null)
                        {
                            filtros += " AND CRD.Libranza = @Libranza ";
                            mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.VarChar);
                            mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                        }
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Agrupamiento", SqlDbType.Int);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaIni.Value.Date;
                        mySqlCommandSel.Parameters["@Agrupamiento"].Value = Agrupamiento;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                            " DECLARE @Comprador AS varchar(10) " +
                            " DECLARE @EmpresaNumCtrl AS VARCHAR(4) " +
                            " DECLARE @CodigoCartera  AS VARCHAR(6) " +
                            " SELECT @EmpresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID = @EmpresaID " +
                            " SET @CodigoCartera = @EmpresaNumCtrl + '16' " +

                            " SELECT @Comprador = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '016' AS INT) " +
                            " select   " +
                            " Comprador,   " +
                            " NombreComprador, Meses,   " +
                            " (case when meses <= 12  then '1 ->12 Meses'   " +
                            "       when meses > 12 and meses <= 24 then '13->24 Meses'   " +
                            "       when meses > 24 and meses <= 36 then '25->36 Meses'   " +
                            "        when meses > 36 and meses <= 48  then '37->48 Meses'   " +
                            "       when meses > 48 and meses <= 60  then '49->60 Meses'   " +
                            "     else '> 60 Meses' end) as Rango,   " +
                            " Cliente,   " +
                            " NombreCliente,    " +
                            "  Credito,   " +
                            " FechaVto,    " +
                            " Garantia,   " +                           
                            "  TotGarantia,   " +
                            "  SaldoTOT,   " +
                            "  Saldo,   " +
                            "  (case when round(TotGarantia / SaldoTOT, 2) > 100 then 100 else round(TotGarantia / SaldoTOT, 2) end) as Cobertura   " +
                            "  from   " +
                            "  (   " +
                            "  SELECT cie.compradorcarteraID as Comprador,   " +
                            "        com.Descriptivo as NombreComprador,   " +
                            "        crd.ClienteID as Cliente,   " +
                            "        cli.Descriptivo as NombreCliente,   " +
                            "        crd.Libranza as Credito,   " +
                            "        cie.NumeroDoc,   " +
                            "        datediff(MONTH, @Periodo, crd.fechaVto) as Meses,   " +
                            "        cast(crd.fechaVto as date) as fechaVto,   " +
                            "        Left(cla.Descriptivo, 3) as Garantia,   " +
                            "        (case when gar.TotGarantia is null then 0 else gar.TotGarantia end) as TotGarantia,   " +
                            "        (case when sdo.SaldoTOT is null    then 0 else sdo.SaldoTOT end) as SaldoTOT,   " +
                            "        CapitalSDO as Saldo   " +
                            "  FROM ccCierreMesCartera cie   " +
                            "      left join ccCreditoDocu          crd on cie.NumeroDoc = crd.NumeroDoc   " +
                            "      left join ccCliente cli on crd.clienteID = cli.ClienteID and crd.eg_ccCliente = cli.EmpresaGrupoID   " +
                            "      left join ccCompradorCartera com on cie.CompradorCarteraID = com.CompradorCarteraID and cie.eg_ccCompradorCartera = com.EmpresaGrupoID   " +
                            "      left join ccLineaCredito lin on crd.LineaCreditoID = lin.LineaCreditoID and crd.eg_ccLineaCredito = lin.EmpresaGrupoID   " +
                            "      left join ccclasificacionCredito cla on lin.ClaseCredito = cla.ClaseCredito and lin.eg_ccclasificacionCredito = cla.EmpresaGrupoID   " +
                            "      left join   " +
                            "              (   " +
                            "              select TerceroID, SUM(vlrfuente) as TotGarantia   " +
                            "              from glGarantiaControl   " +
                            "              group by terceroID   " +
                            "              ) gar on crd.ClienteID = gar.TerceroID   " +
                            "      left join   " +
                            "              (   " +
                            "              select crd.ClienteID, SUM(CapitalSDO) as SaldoTOT   " +
                            "              FROM ccCierreMesCartera cie   " +
                            "                  left join ccCreditoDocu crd on cie.numerodoc = crd.NumeroDoc   " +
                            "                  left join ccCliente     cli on crd.clienteID = cli.ClienteID and crd.eg_ccCliente = cli.EmpresaGrupoID   " +
                            "              WHERE Periodo = @PERIODO and cie.CapitalSDO <> 0   " +
                            "              GROUP BY crd.ClienteID   " +
                            "              ) sdo on crd.ClienteID = sdo.ClienteID   " +
                               "     WHERE Periodo = cast(@Periodo as date) and CIE.EmpresaID = @EmpresaID and cie.CapitalSDO <> 0 and (cie.TipoEstado = 1 or cie.TipoEstado = 2)   " +
                               "             AND cie.CompradorCarteraID != CASE WHEN(@Comprador  IS NULL OR @Comprador = ''  or @Agrupamiento != 1) THEN 'null' ELSE @Comprador  END " +
                               filtros +                           
                            " ) Sdo  " +
                            " ORDER by Comprador, Meses, fechaVto ";
                        #endregion
                    }
                    #endregion
                    #region Resumen Saldos 
                    else if (tipoReporte == 6)
                    {                          
                        mySqlCommandSel = new SqlCommand("Cartera_SaldosxComprador", this.MySqlConnection.CreateCommand().Connection);
                        #region Parametros
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@EmpresaID", this.Empresa.ID.Value));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@Periodo", fechaIni));
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@CompradorFiltro", compCartera));
                        #endregion
                        mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                    }
                    #endregion
                }
                #endregion
                #region Gestion Cobranza Estadistico
                else if(documentoID == AppReports.ccGestionCobranza)
                {
                    if (tipoReporte == 3)//En Proceso Demanda
                    {
                        mySqlCommandSel = new SqlCommand("Cartera_RelacionDemandas", this.MySqlConnection.CreateCommand().Connection);
                        #region Parametros
                        mySqlCommandSel.Parameters.Add(new SqlParameter("@EmpresaID", this.Empresa.ID.Value));
                        #endregion
                        mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                        mySqlCommandSel.CommandTimeout = 2000;
                    }
                    else if (tipoReporte == 5) //Gestion Cobranza Estadistico
                    {
                        #region Configuracion de Filtros
                        // Carga los filtros de acuerdo a la parametrizacion del usuario
                        string filtros = "";
                        bool excluyeDemanda = Convert.ToBoolean(centroPago);
                        mySqlCommandSel.Parameters.Add("@ExcluyeDemanda", SqlDbType.Bit);
                        mySqlCommandSel.Parameters["@ExcluyeDemanda"].Value = excluyeDemanda;

                        if (!string.IsNullOrEmpty(filter.ToString()))
                        {
                            filtros += " AND cie.Etapa = @Etapa   ";
                            mySqlCommandSel.Parameters.Add("@Etapa", SqlDbType.Char, 10);
                            mySqlCommandSel.Parameters["@Etapa"].Value = filter.ToString();
                        }
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaFin;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                        " select  cie.Grupo,cie.MesesVenc, crd.libranza as Credito,cie.SaldoVencido, " +
                        " Case When cie.SaldoCapital > 0 Then 'X' Else '' End as isSaldoCapital, " +
                        " Case When cie.SaldoSeguro > 0 Then 'X' Else ''End as isSaldoSeguro,  " +
                        "  crd.ClienteID, cli.descriptivo as Nombre,  cie.FechaMora,   cie.FechaUltPago, " +
                        "  cie.Etapa, cie.gestion,  crd.CompradorCarteraID as Cesionario,  cie.DiasMora, crd.LineaCreditoID,cli.celular1" +
                        " from " +
                        "     ( " +
                        "        select cie.NumeroDoc, cie.FechaMora, cie.FechaUltPago, cie.Altura, cie.CobranzaGestionID as Gestion, ges.EtapaIncumplimiento as Etapa, 1 as Cantidad, cie.SaldoVencido, diasMora, cie.SaldoCapital, cie.SaldoSeguro, " +
                        "              (case when cie.Altura <= 12     then 'CUOTA ' + cast(cie.Altura AS varchar(10)) else " +
                        "              (Case when cie.Altura > 12 and cie.Altura <= 24 then 'CUOTA 13 a 24'  else " +
                        "              (Case when cie.Altura > 24 and cie.Altura <= 36 then 'CUOTA 25 a 36'  else '>a CUOTA 36' end) end) end) as Grupo,	 " +
                        "            case  " +
                        "               when cie.Altura <= 12      then cie.Altura " +
                        "                when cie.Altura > 12 and cie.Altura <= 24 then 13 " +
                        "              when cie.Altura > 24 and cie.Altura <= 36 then 14 " +
                        "              else 15 " +
                        "           end as Orden, " +
                        "          DATEDIFF(MONTH, cie.fechaMora, @FechaCorte) as MesesVenc " +
                        "        from ccCierreDiaCartera cie WITH(NOLOCK) " +
                        "        left join ccCobranzaGestion ges WITH(NOLOCK) on cie.CobranzaGestionID = ges.CobranzaGestionID and cie.eg_ccCobranzaGestion = ges.EmpresaGrupoID " +
                        "       where cie.EmpresaID = @EmpresaID and cast(Fecha as DATE) = @FechaCorte and FechaMora is not null and FechaMora <= @FechaCorte and  cie.saldoVencido > 0 and ges.EtapaIncumplimiento is not null " +
                        "             and ((@ExcluyeDemanda = 0) or(ges.ControlTipo <> 2 and ges.GestionDemanda not in (0, 2, 3))) " +
                        "    ) cie " +
                        " left join ccCreditoDocu crd WITH (NOLOCK)on cie.NumeroDoc = crd.NumeroDoc " +
                        " left join ccCliente cli   WITH(NOLOCK) on crd.ClienteID = cli.ClienteID and crd.eg_ccCliente = cli.EmpresaGrupoID " +
                        " where crd.TipoEstado <= 2 " + filtros +
                        " order by cie.MesesVenc desc, cie.Grupo, cie.SaldoVencido desc ";
                        #endregion
                    }
                    else if (tipoReporte == 6)//Estado Gestion Diaria
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@DiaAnt", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@DiaAct", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros                        
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@DiaAnt"].Value = fechaFin.AddDays(-1);
                        mySqlCommandSel.Parameters["@DiaAct"].Value = fechaFin;

                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                                "select	res.Cliente," +
                                "		cli.Descriptivo as Nombre," +
                                "		res.Credito, res.NumeroDoc,ant.Est_ANT, " +
                                "		(case when act.Est_ACT IS null then '' else act.Est_ACT end) as Est_ACT, " +
                                "		(case when ant.Est_ANT = act.Est_ACT then 1 else 0 end)						as IGUAL," +
                                "		(case when dia_ANT = 1 then 1 else 0 end)									as VienenDIA," +
                                "		(case when dia_ANT = 1 and dia_ACT = 0 and PJ1_ACT = 0 then 1 else 0 end)	as DIASaleCAN, " +
                                "		(case when pj1_ANT = 1 then 1 else 0 end)									as VienenPJ1," +
                                "		(case when dia_ANT = 1 and PJ1_ACT = 1 then 1 else 0 end)					as DIAIngrPJ1,	" +
                                "		(case when PJ2_ANT = 1 and PJ1_ACT = 1 then 1 else 0 end)					as PJ2IngrPJ1," +
                                "		(case when PJ3_ANT = 1 and PJ1_ACT = 1 then 1 else 0 end)					as PJ3_PJ1," +
                                "		(case when PJ1_ANT = 1 and dia_ACT = 0 and PJ1_ACT = 0 and PJ2_ACT = 0 then 1 else 0 end) as PJ1SaleCAN, " +
                                "		(case when PJ1_ANT = 1 and dia_ACT = 1 then 1 else 0 end)					as PJ1SaleDIA," +
                                "		(case when PJ1_ANT = 1 and PJ2_ACT = 1 then 1 else 0 end)					as PJ1SalePJ2," +
                                "		(case when pj2_ANT = 1 then 1 else 0 end)									as VienenPJ2," +
                                "		(case when dia_ANT = 1 and PJ2_ACT = 1 then 1 else 0 end)					as DIAIngrPJ2," +
                                "		(case when PJ1_ANT = 1 and PJ2_ACT = 1 then 1 else 0 end)					as PJ1IngrPJ2," +
                                "		(case when PJ3_ANT = 1 and PJ2_ACT = 1 then 1 else 0 end)					as PJ3IngrPJ2," +
                                "		(case when PJ2_ANT = 1 and dia_ACT = 0 and PJ1_ACT = 0 and PJ2_ACT = 0  and PJ3_ACT = 0 then 1 else 0 end) as PJ2SaleCAN," +
                                "		(case when PJ2_ANT = 1 and dia_ACT = 1 then 1 else 0 end)					as PJ2SaleDIA," +
                                "		(case when PJ2_ANT = 1 and PJ1_ACT = 1 then 1 else 0 end)					as PJ2SalePJ1," +
                                "		(case when PJ2_ANT = 1 and PJ3_ACT = 1 then 1 else 0 end)					as PJ2SalePJ3," +
                                "		(case when pj3_ANT = 1 then 1 else 0 end)									as VienenPJ3," +
                                "		(case when PJ2_ANT = 1 and PJ3_ACT = 1 then 1 else 0 end)					as PJ2IngrPJ3," +
                                "		(case when PJ3_ANT = 1 and dia_ACT = 0 and PJ1_ACT = 0 and PJ2_ACT = 0 and PJ3_ACT = 0 and PJ4_ACT = 0 then 1 else 0 end) as PJ3SaleCAN," +
                                "		(case when PJ3_ANT = 1 and dia_ACT = 1 then 1 else 0 end)					as PJ3SaleDIA," +
                                "		(case when PJ3_ANT = 1 and PJ1_ACT = 1 then 1 else 0 end)					as PJ3SalePJ1," +
                                "		(case when PJ3_ANT = 1 and PJ2_ACT = 1 then 1 else 0 end)					as PJ3SalePJ2," +
                                "		(case when PJ3_ANT = 1 and PJ4_ACT = 1 then 1 else 0 end)					as PJ3SalePJ4," +
                                "		(case when pj4_ANT = 1 then 1 else 0 end)									as VienenPJ4" +
                                " from" +
                                "	(" +
                                "	select	numerodoc, Credito, Cliente," +
                                "			sum(DIA_ANT) as DIA_ANT," +
                                "			sum(DIA_ACT) as DIA_ACT," +
                                "			sum(PJ1_ANT) as PJ1_ANT," +
                                "			sum(PJ1_ACT) as PJ1_ACT," +
                                "			sum(PJ2_ANT) as PJ2_ANT," +
                                "			sum(PJ2_ACT) as PJ2_ACT," +
                                "			sum(PJ3_ANT) as PJ3_ANT," +
                                "			sum(PJ3_ACT) as PJ3_ACT," +
                                "			sum(PJ4_ANT) as PJ4_ANT," +
                                "			sum(PJ4_ACT) as PJ4_ACT," +
                                "			sum(CJ_ANT)  as CJ_ANT," +
                                "			sum(CJ_ACT)  as CJ_ACT" +
                                "	from" +
                                "		(" +
                                "		select	cie.numerodoc, crd.Libranza as Credito, crd.ClienteID as Cliente," +
                                "				(case when ges.EtapaIncumplimiento IS NULL then 1 else 0 end) as DIA_ANT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ01' then 1 else 0 end) as PJ1_ANT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ02' then 1 else 0 end) as PJ2_ANT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ03' then 1 else 0 end) as PJ3_ANT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ04' then 1 else 0 end) as PJ4_ANT," +
                                "				(case when cie.EstadoDeuda >  3 then 1 else 0 end) as CJ_ANT," +
                                "				0 as DIA_ACT," +
                                "				0 as PJ1_ACT," +
                                "				0 as PJ2_ACT," +
                                "				0 as PJ3_ACT," +
                                "				0 as PJ4_ACT," +
                                "				0 as CJ_ACT" +
                                "		from ccCierreDiaCartera cie WITH(NOLOCK)" +
                                "			left join cccreditodocu crd on cie.NumeroDoc= crd.NumeroDoc" +
                                "			LEFT join ccCobranzaGestion ges on cie.CobranzaGestionID=ges.CobranzaGestionID and cie.eg_ccCobranzaGestion=ges.EmpresaGrupoID" +
                                "		where crd.TipoEstado <= 2 and  Fecha = @DiaAnt" +
                                "		UNION" +
                                "		select	cie.numerodoc, crd.Libranza as Credito, crd.ClienteID as Cliente," +
                                "				0 as DIA_ANT," +
                                "				0 as PJ1_ANT," +
                                "				0 as PJ2_ANT," +
                                "				0 as PJ3_ANT," +
                                "				0 as PJ4_ANT," +
                                "				0 as CJ_ANT," +
                                "				(case when ges.EtapaIncumplimiento IS NULL then 1 else 0 end) as DIA_ACT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ01' then 1 else 0 end) as PJ1_ACT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ02' then 1 else 0 end) as PJ2_ACT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ03' then 1 else 0 end) as PJ3_ACT," +
                                "				(case when ges.EtapaIncumplimiento='PRJ04' then 1 else 0 end) as PJ4_ACT," +
                                "				(case when cie.EstadoDeuda >  3 then 1 else 0 end) as CJ_ACT" +
                                "		from ccCierreDiaCartera cie WITH(NOLOCK)" +
                                "			left join cccreditodocu crd on cie.NumeroDoc= crd.NumeroDoc" +
                                "			LEFT join ccCobranzaGestion ges on cie.CobranzaGestionID=ges.CobranzaGestionID and cie.eg_ccCobranzaGestion=ges.EmpresaGrupoID  " +
                                "		where cie.EmpresaID = @EmpresaID and crd.TipoEstado <= 2 and Fecha = @DiaAct" +
                                "		) cie" +
                                "	GROUP BY numerodoc, Credito, Cliente" +
                                "	) res" +
                                "	left join" +
                                "		(" +
                                "		select	cie.numerodoc, " +
                                "				(case when ges.EtapaIncumplimiento IS null then '' else ges.EtapaIncumplimiento end) as Est_ANT" +
                                "		from ccCierreDiaCartera cie WITH(NOLOCK)" +
                                "			LEFT join ccCobranzaGestion ges on cie.CobranzaGestionID=ges.CobranzaGestionID and cie.eg_ccCobranzaGestion=ges.EmpresaGrupoID  " +
                                "		where cie.EmpresaID = @EmpresaID and cie.fecha = @DiaAnt" +
                                "		) ant on res.NumeroDoc = ant.NumeroDoc" +
                                "	LEFT join " +
                                "		(" +
                                "		select	cie.numerodoc," +
                                "				(case when ges.EtapaIncumplimiento IS null then '' else ges.EtapaIncumplimiento end) as Est_ACT" +
                                "		from ccCierreDiaCartera cie WITH(NOLOCK)" +
                                "			LEFT join ccCobranzaGestion ges on cie.CobranzaGestionID=ges.CobranzaGestionID and cie.eg_ccCobranzaGestion=ges.EmpresaGrupoID  " +
                                "		where cie.EmpresaID = @EmpresaID and cie.fecha = @DiaAct" +
                                "		) act on res.NumeroDoc = act.NumeroDoc " +
                                "	left join ccCliente cli  WITH(NOLOCK) on res.Cliente = cli.ClienteID" +
                                " where cli.EmpresaGrupoID = @empresaID" +
                                " order by  cli.Descriptivo, res.Credito";
                        #endregion
                    }
                    else if (tipoReporte == 7)//Compromisos incumplidos
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@ActividadFlujoCob", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Usuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@DocPantalla", SqlDbType.Int);
                        mySqlCommandSel.Parameters.Add("@EtapaID", SqlDbType.Char, UDT_CodigoGrl10.MaxLength);
                        #endregion
                        #region Asignacion Valores a Parametros                        
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@ActividadFlujoCob"].Value = filter.ToString();
                        mySqlCommandSel.Parameters["@Usuario"].Value = usuarioID;
                        mySqlCommandSel.Parameters["@DocPantalla"].Value = documentoID;
                        mySqlCommandSel.Parameters["@EtapaID"].Value = pagaduria;// se usa pagaduria para no modificar la funcion
                        #endregion
                        #region Filtros
                            string Filtros = "";

                            if (!string.IsNullOrEmpty(pagaduria))
                                Filtros = " AND ges.EtapaIncumplimiento= @EtapaID";
                        #endregion

                        #region CommanText
                        mySqlCommandSel.CommandText =
                        " SELECT  cr.ClienteID,cl.Descriptivo as Nombre, cr.Libranza,"+
                        " ges.EtapaIncumplimiento as Etapa,"+ //1RO
                        " act.FechaAlarma1 as FechaCompromiso,month(act.FechaAlarma1) as Mes, IsNull(act.Valor, 0) as Valor, " +
                        "    act.Observaciones as ObservacionCompromiso, act.CerradoInd as CumplidoInd " +
                        " FROM glActividadEstado act WITH(NOLOCK) " +
                        " INNER JOIN ccCreditoDocu cr WITH(NOLOCK) ON act.NumeroDoc = cr.NumeroDoc " +
                        " LEFT JOIN ccCobranzaGestion ges WITH(NOLOCK) ON cr.CobranzaGestionCierre = ges.CobranzaGestionID and"+
                        "   cr.eg_ccCobranzaGestion = ges.empresaGrupoID"+ //2DO
                        " INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = cr.NumeroDoc " +
                        " INNER JOIN ccCliente cl WITH(NOLOCK) ON cl.ClienteID = cr.ClienteID  and cl.EmpresaGrupoID = cr.eg_ccCliente " +
                        " INNER JOIN (Select numeroDoc from glDocumentoControl doc " +
                        "             inner join ( select per.AreaFuncionalID, per.UsuarioID, are.CentroCostoID, " +
                        "                           per.eg_glAreaFuncional, are.eg_coCentroCosto " +
                        "                            from glActividadPermiso per" +
                        "                           left join glAreaFuncional	are on per.AreaFuncionalID  = are.AreaFuncionalID and per.eg_glAreaFuncional = are.empresaGrupoID " +
                        "                           left join glActividadFlujo	act on per.ActividadFlujoID = act.ActividadFlujoID	and per.eg_glActividadFlujo = act.empresaGrupoID " +
                        "                           where per.UsuarioID = @Usuario and act.DocumentoID = @DocPantalla " +
                        "                         ) perC on doc.CentroCostoID = PerC.CentroCostoID and doc.eg_coCentroCosto = PerC.eg_coCentroCosto " +
                        "             where doc.documentoID = 161 and " +
                        "            ((doc.AreaFuncionalID	= perC.AreaFuncionalID	and doc.eg_glAreaFuncional	= perC.eg_glAreaFuncional) or " +
                        "             (doc.CentroCostoID = perC.CentroCostoID	and doc.eg_coCentroCosto = perC.eg_coCentroCosto)) ) q on q.numeroDoc = ctrl.NumeroDoc " +
                        " WHERE cr.EmpresaID = @EmpresaID and  act.ActividadFlujoID = @ActividadFlujoCob and act.FechaAlarma1 is not null  and act.FechaAlarma1 <= GETDATE() and act.CerradoInd = 0 "+
                        " and ges.EtapaIncumplimiento is not null" + Filtros +
                        " GROUP BY cr.Libranza, cr.ClienteID, cl.Descriptivo, act.FechaAlarma1, act.Valor, act.Observaciones, act.cerradoInd,ges.EtapaIncumplimiento " +
                        " Order by act.FechaAlarma1 desc, cr.ClienteID";
                        #endregion
                    }
                    else if (tipoReporte == 8)//LLamadas para normalizacion
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Usuario", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@DocPantalla", SqlDbType.Int);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Usuario"].Value = usuarioID;
                        mySqlCommandSel.Parameters["@DocPantalla"].Value = documentoID;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                        "   DECLARE @EmpresaNumCtrl AS VARCHAR(4)	" +
                        "   DECLARE @CodigoCartera AS VARCHAR(6)		" +
                        "   DECLARE @Etapa1 AS varchar(10)	" +
                        "   DECLARE @Etapa2 AS varchar(10)	" +
                        "   DECLARE @Etapa3 AS varchar(10)	" +
                        "   DECLARE @Etapa4 AS varchar(10)	" +                        
                        "   SELECT @EmpresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID =  @EmpresaID	" +
                        "   SET @CodigoCartera = @EmpresaNumCtrl + '16'" +
                        "   SELECT @Etapa1 = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '120' AS INT)	" +
                        "   SELECT @Etapa2 = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '121' AS INT)	" +
                        "   SELECT @Etapa3 = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '122' AS INT)	" +
                        "   SELECT @Etapa4 = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '123' AS INT)	" +
                        "   Select Cliente, Nombre, Credito, cast(Cuota as varchar(3)) + ' / ' + cast(Plazo as varchar(3)) as AlturaCuota, Etapa, Cuota" +
                        "   from" +
                        "   	(" +
                        "   	Select crd.ClienteID as Cliente, cli.Descriptivo as Nombre, crd.Libranza as Credito, crd.Plazo, min(pla.CuotaID) as Cuota, ges.EtapaIncumplimiento as Etapa" +
                        "   	from ccCreditoplanpagos pla" +
                        "   		left join ccCreditoDocu crd on pla.numerodoc = crd.numeroDoc" +
                        "   		left join ccCliente		cli on crd.ClienteID = cli.ClienteID and crd.eg_ccCLiente = cli.EmpresaGrupoID " +
                        "   		left join ccCobranzaGestion ges on crd.cobranzaGestionCierre = ges.CobranzaGestionID and crd.eg_ccCobranzaGestion = ges.empresaGrupoID" +
                        "   		left join ccTipoCredito     ope on crd.TipoCreditoID = ope.TipoCreditoID and crd.eg_ccTipoCredito = ope.empresaGrupoID" +
                        "           INNER JOIN (Select numeroDoc from glDocumentoControl doc " +
                        "             inner join ( select per.AreaFuncionalID, per.UsuarioID, are.CentroCostoID, " +
                        "                           per.eg_glAreaFuncional, are.eg_coCentroCosto " +
                        "                            from glActividadPermiso per" +
                        "                           left join glAreaFuncional	are on per.AreaFuncionalID  = are.AreaFuncionalID and per.eg_glAreaFuncional = are.empresaGrupoID " +
                        "                           left join glActividadFlujo	act on per.ActividadFlujoID = act.ActividadFlujoID	and per.eg_glActividadFlujo = act.empresaGrupoID " +
                        "                           where per.UsuarioID = @Usuario and act.DocumentoID = @DocPantalla " +
                        "                         ) perC on doc.CentroCostoID = PerC.CentroCostoID and doc.eg_coCentroCosto = PerC.eg_coCentroCosto " +
                        "             where doc.documentoID = 161 and " +
                        "            ((doc.AreaFuncionalID	= perC.AreaFuncionalID	and doc.eg_glAreaFuncional	= perC.eg_glAreaFuncional) or " +
                        "             (doc.CentroCostoID = perC.CentroCostoID	and doc.eg_coCentroCosto = perC.eg_coCentroCosto)) ) q on q.numeroDoc = crd.NumeroDoc " +
                        "       where (ges.EtapaIncumplimiento in(@Etapa1, @Etapa2, @Etapa3) or (ges.EtapaIncumplimiento = @Etapa4 and ges.GestionDemanda=1)) and pla.VlrCuota > pla.VlrPagadoCuota" +
                        "   		  and cast(100 * pla.CuotaID / crd.Plazo as int) <= 25  and crd.TipoEstado in(1,2)" +
                        "   		  and crd.EmpresaID = @EmpresaID " +
                        "             and ope.TipoCredito<>3 and ope.TipoCredito<>4" +
                        "   	group by crd.ClienteID, cli.Descriptivo, crd.Libranza, crd.Plazo, ges.EtapaIncumplimiento" +
                        "   	) cc" +
                        "   where (etapa = @Etapa1 and cuota <= 6)  or " +
                        "         (etapa = @Etapa2 and cuota <= 12) or " +
                        "          etapa = @Etapa3 or etapa = @Etapa4 " +
                        "   order by etapa, cuota, Nombre";
                        #endregion
                    }
                }
                #endregion
                #region Solicitudes
                else if(documentoID == AppReports.ccRepSolicitudes)
                {
                    if (tipoReporte == 1)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaFin;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                            "select CTL.Fecha," +
                            "        usu.Descriptivo as Recibe," +
                            "        sol.Libranza," +
                            "        sol.ClienteID as Cedula," +
                            "        cli.Descriptivo as NombresCliente," +
                            "        pag.Descriptivo as Pagaduria," +
                            "        ciu.Descriptivo as Ciudad," +
                            "        con.Descriptivo as AsesorCodigo," +
                            "        ASe.Descriptivo as Asesor" +
                            "    FROM ccSolicitudDocu	   SOL" +
                            "    INNER JOIN glDocumentoControl	CTL ON SOL.NumeroDoc	= CTL.NumeroDoc 		AND SOL.ClienteID = CTL.TerceroID " +
                            "    INNER JOIN ccCliente		CLI ON SOL.ClienteID	= CLI.ClienteID 			AND SOL.eg_ccCliente= CLI.EmpresaGrupoID" +
                            "    INNER JOIN seUsuario		USU ON ctl.seUsuarioID	= USU.ReplicaID" +
                            "    LEFT JOIN glLugarGeografico	CIU ON sol.Ciudad= ciu.LugarGeograficoID" +
                            "    left join ccPagaduria		PAG on sol.PagaduriaID=pag.PagaduriaID" +
                            "    left join ccConcesionario	CON	on sol.ConcesionarioID=con.ConcesionarioID" +
                            "    left join ccAsesor			ASE	on sol.AsesorID=ase.AsesorID" +
                            "    WHERE SOL.EmpresaID		=	@EmpresaID and ctl.Estado in(1,2) and ctl.DocumentoID=160 and month( CTL.FechaDoc) = month(@Periodo) and  year(CTL.FechaDoc) = year(@Periodo) ";
                        #endregion
                    }

                }
                #endregion 
                #region Gestion Comercial
                else if(documentoID == AppReports.ccActivacionReoperacion)
                {
                    #region Reoperaciones
                    if (tipoReporte == 1)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaFin;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =                           
                                    "   DECLARE @factReop	porcentajeID = 0"+
                                    "   DECLARE @factGar	porcentajeID = 0"+
                                    "   DECLARE @EmpresaNumCtrl			AS VARCHAR(4)"+
                                    "   DECLARE @CodigoCartera			AS VARCHAR(6)"+
                                    "   SELECT @EmpresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID = @EmpresaID"+
                                    "   SET @CodigoCartera = @EmpresaNumCtrl + '16'"+
                                    "   SELECT @factReop = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '125' AS INT)"+
                                    "   SELECT @factGar = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '126' AS INT)"+
                                    "   select * "+
                                    "   from "+
                                    "   	("+
                                    "   	select	crd.Libranza as Credito, crd.ClienteID, cli.Descriptivo as Nombre, cie.SaldoCapital,"+
			                        "   crd.Plazo,	cie.altura,"+
			                        "   DATEDIFF(year,cli.FechaNacimiento,getdate()) as Edad,"+
			                        "   gar.TotGarantia, gar.FechaSuscrGarantia,"+
			                        "   pol.fechaVtoPoliza,"+
			                        "   (case when gar.TotGarantia=0 or gar.TotGarantia is null then 0 else CAST(100 * sdo.TotCapital / gar.TotGarantia as  numeric(10,2)) end) as FactGarantias,"+
		                            "   sdo.TotCapital,"+
			                        "   cie.SaldoTotal, cie.SaldoVencido,"+
			                        "   cie.SaldoSeguro"+
	                                "   from ccCierreDiaCartera cie with (nolock)"+
	                                "   left join ccCreditoDocu			crd with (nolock) on cie.NumeroDoc			 = crd.numerodoc"+
		                            "   left join ccCliente				cli with (nolock) on crd.ClienteID			 = cli.ClienteID and crd.eg_ccCliente=cli.EmpresaGrupoID"+
		                            "   left join ccTipoCredito			tcr with (nolock) on crd.TipoCreditoID		 = tcr.TipoCreditoID and crd.eg_ccTipoCredito=tcr.EmpresaGrupoID"+
		                            "   left join ccCobranzaGestion		ges with (nolock) on cie.CobranzaGestionID	 = ges.CobranzaGestionID and cie.eg_ccCobranzaGestion = ges.EmpresaGrupoID"+
		                            "   left join glIncumplimientoEtapa Inc with (nolock) on ges.EtapaIncumplimiento = inc.EtapaIncumplimiento and ges.eg_glIncumplimientoEtapa = inc.EmpresaGrupoID"+
		                            "   left join "+
                                    "           ("+
                                    "           select TerceroID, SUM(vlrfuente) as TotGarantia, MIN(fechaIni) as FechaSuscrGarantia"+
                                    "               from glGarantiaControl"+
			                        "               where EmpresaID = @EmpresaID"+
			                        "         	    group by terceroID"+
			                        "         	) gar on cie.ClienteID = gar.TerceroID"+
		                            "   left join "+
			                        "           ("+
			                        "         	select ClienteID, SUM(SaldoCapital) as TotCapital"+
			                        "         	from ccCierreDiaCartera"+
			                        "         	where EmpresaID = @EmpresaID and Fecha = @FechaCorte"+
			                        "         	group by ClienteID"+
			                        "         	) sdo on cie.ClienteID = sdo.ClienteID "+
		                            "   left join "+
			                        "         	 ("+
			                        "         	 select NumDocCredito, max(FechaVigenciaFIN) as fechaVtoPoliza"+
	 		                        "         	from ccPolizaEstado"+
 	 		                        "         	where FechaVigenciaFIN > @FechaCorte"+
 	 		                        "         	group by NumDocCredito"+
	 		                        "         	) pol on cie.numerodoc = pol.NumDocCredito"+
	                                "   where	cie.Fecha = @FechaCorte and (inc.NivelRiesgo <= 3 or inc.NivelRiesgo is null) and cie.SaldoCapital > 0"+
                                    "   ) car"+
                                    "   Where 100 * car.altura/car.Plazo >= @factReop and car.FactGarantias >= @factGar   " +
                                    "   order by car.altura desc, car.Plazo asc, Nombre";
                        #endregion
                    }
                    #endregion 
                    #region Activaciones
                    if (tipoReporte == 2)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaFin;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                                            "   DECLARE	@NivelRiesgo	tinyint		= 5" +
                                            "   select * from " +
                                            "   (" +
                                            "   SELECT crd.ClienteID, cli.Descriptivo as Nombre, pag.Credito, pag.UltimoPago, pag.FormaPago, " +
                                            "   		DATEDIFF(year,cli.FechaNacimiento,getdate()) as Edad," +
                                            "   		(case when gar.TerceroID is null then '' else 'X' end) as GarantiasVIG, gar.TotGarantia, gar.FechaSuscrGarantia" +
                                            "   from " +
                                            "   (" +
                                            "   	Select ClienteID, eg_ccCliente" +
                                            "   	from ccCreditoDocu " +
                                            "   	group by ClienteID, eg_ccCliente" +
                                            "   	) crd" +
                                            "   		left join ccCliente	cli with (nolock) on crd.ClienteID = cli.ClienteID and crd.eg_ccCliente=cli.EmpresaGrupoID" +
                                            "   		left join " +
                                            "   			(" +
                                            "   			select ClienteID, SUM(SaldoCapital) as TotCapital" +
                                            "   			from ccCierreDiaCartera" +
                                            "   			where EmpresaID = @EmpresaID and Fecha = @FechaCorte " +
                                            "   			group by ClienteID" +
                                            "   			) sdo on crd.ClienteID = sdo.ClienteID" +
                                            "   		left join " +
                                            "   (" +
                                            "   select TerceroID, SUM(vlrfuente) as TotGarantia, MIN(fechaIni) as FechaSuscrGarantia" +
                                            "   			from glGarantiaControl" +
                                            "   			where EmpresaID = @EmpresaID" +
                                            "   			group by terceroID" +
                                            "   			) gar on crd.ClienteID = gar.TerceroID" +
                                            "   		left join " +
                                            "   			(" +
                                            "   			select ult.TerceroID, crd.Libranza as Credito, doc.FechaDoc as UltimoPago, ult.FormaPago" +
                                            "   			from " +
                                            "   				(" +
                                            "   				select  ult.terceroID, ult.pagodocu, pag.numeroDoc, " +
                                            "   						(case when doc.DocumentoID = '168' then 'PTO'" +
                                            "   							  when doc.DocumentoID = '183' then 'REO'" +
                                            "   							  else 'ULT' end) as FormaPago" +
                                            "   				from " +
                                            "   					(" +
                                            "   					select doc.TerceroID, max(pag.pagodocu) as PagoDocu" +
                                            "   					from ccCreditoPagos pag" +
                                            "   						left join glDocumentoControl doc on pag.PagoDocu = doc.NumeroDoc" +
                                            "   					where doc.EmpresaID = @EmpresaID" +
                                            "   					group by doc.TerceroID" +
                                            "   					) ult" +
                                            "   					left join glDocumentoControl doc on ult.PagoDocu = doc.NumeroDoc" +
                                            "   					left join ccCreditoPagos	 pag on ult.PagoDocu = pag.PagoDocu" +
                                            "   				group by ult.terceroID, ult.pagodocu, pag.numeroDoc, doc.DocumentoID" +
                                            "   				) ult" +
                                            "   				left join ccCreditoDocu crd		 on ult.NumeroDoc = crd.NumeroDoc" +
                                            "   				left join glDocumentoControl doc on ult.PagoDocu  = doc.NumeroDoc" +
                                            "   			) pag on crd.ClienteID = pag.TerceroID" +
                                            "   		left join " +
                                            "   			(" +
                                            "   			select doc.terceroid" +
                                            "   			from ccCJHistorico cju" +
                                            "   				left join glDocumentoControl doc on cju.numerodoc = doc.NumeroDoc" +
                                            "   			where doc.EmpresaID = @EmpresaID " +
                                            "   			group by doc.terceroid" +
                                            "   			) cju on crd.ClienteID = cju.terceroID" +
                                            "   		left join " +
                                            "   			(" +
                                            "   			select doc.terceroid, max(eta.NivelRiesgo) as NivelRiesgo" +
                                            "   			from ccHistoricoGestionCobranza his" +
                                            "   				left join glDocumentoControl	doc with (nolock) on his.NumeroDoc = doc.NumeroDoc" +
                                            "   				left join ccCobranzaGestion		ges with (nolock) on his.CobranzaGestionID = ges.CobranzaGestionID and his.eg_ccCobranzaGestion = ges.EmpresaGrupoID" +
                                            "   				left join glIncumplimientoEtapa eta with (nolock) on ges.EtapaIncumplimiento = eta.EtapaIncumplimiento and ges.eg_glIncumplimientoEtapa = eta.EmpresaGrupoID" +
                                            "   			group by doc.terceroid" +
                                            "   			) ges on crd.ClienteID = ges.terceroID" +
                                            "   where sdo.ClienteID is null and cju.TerceroID is null and ges.NivelRiesgo < 5 and DATEDIFF(day,pag.UltimoPago, GETDATE()) <= 365" +
                                            "   ) ttt"+ 
                                            "	order by GarantiasVIG, UltimoPago desc, Nombre";
                        #endregion
                    }
                    #endregion 
                }
                #endregion
                #region Pagaduria
                else if (documentoID == AppReports.ccReportePagaduria)
                {
                    #region nomina ahora Mvo Pagaduria
                    if (tipoReporte == 1)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Cuenta", SqlDbType.Char, UDT_CuentaID.MaxLength);

                        
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaFin;
                        if (!string.IsNullOrEmpty(pagaduria))
                            mySqlCommandSel.Parameters["@Pagaduria"].Value = pagaduria;
                        else
                            mySqlCommandSel.Parameters["@Pagaduria"].Value = null;
                        mySqlCommandSel.Parameters["@ClienteID"].Value = null;
                        mySqlCommandSel.Parameters["@Cuenta"].Value = "13809509";
                        #endregion

                        #region Filtros

                        string paga = "";

                        if (!string.IsNullOrEmpty(pagaduria))
                            paga = " and nom.PagaduriaID  = @Pagaduria ";

                        #endregion

                        #region CommanText

                        //"   select	cc.Pagaduria as PagaduriaID, cc.NomPagaduria as Pagaduria, cc.FechaNomina, cc.Libranza," +
                        //"   		cc.Cliente, cc.Nombre, cc.valornomina, cc.Observacion"+
                        //"   froM"+
                        //"   	("+
                        //"   	select	mvo.Pagaduria, mvo.NomPagaduria, rca.FechaAplica as FechaNomina,"+
                        //"   			aux.TerceroID as Cliente, ter.Descriptivo As Nombre, aux.DocumentoCOM AS Libranza, "+
                        //"      			-sum(aux.VlrMdaLoc) as ValorNomina,  nom.observacion"+
                        //"   	from coauxiliar aux"+
                        //"      		inner join"+
                        //"      			("+
                        //"      			select aux.NumeroDoc, aux.TerceroID as Pagaduria, ter.Descriptivo as NomPagaduria"+
                        //"      			from coauxiliar aux"+
                        //"      				left join coTercero ter on aux.TerceroID = ter.TerceroID and aux.eg_coTercero = ter.empresaGrupoID"+
                        //"      			where aux.PeriodoID = @FechaCorte and aux.cuentaID = @Cuenta " +
                        //"      			) Mvo on aux.numerodoc = mvo.NumeroDoc"+
                        //"      		left join glDocumentoControl doc on aux.NumeroDoc	= doc.NumeroDoc"+
                        //"      		left join tsReciboCajaDocu   rca on doc.NumeroDoc	= rca.NumeroDoc"+
                        //"      		left join ccNominaDeta		 nom  on doc.NumeroDoc  = nom.NumDocRCaja and aux.IdentificadorTR = nom.NumDocCredito"+
                        //"      		left join coTercero			 ter on aux.TerceroID = ter.TerceroID and aux.eg_coTercero = ter.empresaGrupoID"+
                        //"   	where doc.estado = 3 and aux.PeriodoID = @FechaCorte and aux.cuentaID <> @Cuenta AND DOC.EmpresaID=@EmpresaID and" +
                        //"   		(aux.TerceroID <> mvo.Pagaduria) and "+
                        //"      		((@ClienteID is null) or (aux.TerceroID=@ClienteID)) and"+
                        //"      		((@Pagaduria is null) or (mvo.Pagaduria=@Pagaduria))"+
                        //"   	group by mvo.Pagaduria, mvo.NomPagaduria, rca.FechaAplica, aux.TerceroID, ter.Descriptivo, aux.DocumentoCOM,  nom.observacion"+
                        //"   	) cc"+
                        //"   where  ValorNomina <> 0"+
                        //"   order by cc.NomPagaduria";

                        mySqlCommandSel.CommandText =
                            "   select	aux.TerceroID, ter.Descriptivo as  TerceroDesc, nom.PagaduriaID, pag.descriptivo as NomPagaduria, nom.FechaNomina, " +
                            "   		nom.Libranza, nom.clienteID as Cliente, nom.Nombre, " +
                            "   		aux.VlrMdaLoc as VlrNomina,  " +
                            "   		aux.NumeroDoc, rtrim(aux.ComprobanteID)+'-'+rtrim(aux.ComprobanteNro) as Comprobante, rtrim(nom.Observacion) as Observacion " +
                            "   from coauxiliar aux "+
                            "   	left join glDocumentoControl doc on aux.NumeroDoc	= doc.NumeroDoc "+
                            "   	left join  "+
                            "   		( "+
                            "   		select	nom.NumDocRCaja, nom.FechaNomina, nom.PagaduriaID, nom.eg_ccPagaduria, nom.NumDocCredito, "+
                            "   				crd.Libranza, crd.ClienteID,  cli.Descriptivo as Nombre, nom.Observacion "+
                            "   		from ccNominaDeta nom "+
                            "   	   		left join glDocumentoControl doc on nom.NumDocRCaja = doc.NumeroDoc "+
                            "   	   		left join ccCreditoDocu crd on nom.NumDocCredito = crd.NumeroDoc "+
                            "   	   		left join ccCliente cli on crd.ClienteID = cli.clienteID and crd.eg_ccCliente = cli.EmpresaGrupoID "+
                            "   		where doc.PeriodoDoc = @Periodo " +
                            "   		) nom on doc.NumeroDoc  = nom.NumDocRCaja "+
                            "   	left join coTercero			 ter on aux.TerceroID	= ter.TerceroID and aux.eg_coTercero = ter.empresaGrupoID "+
                            "   	left join ccPagaduria		 pag on nom.PagaduriaID = pag.PagaduriaID and nom.eg_ccPagaduria = pag.empresaGrupoID "+
                            "   	where ((doc.estado = 3 and aux.VlrMdaLoc > 0) or (doc.estado = 4 and aux.VlrMdaLoc < 0))  "+
                            "               and aux.PeriodoID = @Periodo and aux.cuentaID = @Cuenta AND DOC.EmpresaID=@Empresa "+
                            "               and (@Pagaduria is null or @Pagaduria='' or aux.TerceroID=@Pagaduria or nom.PagaduriaID = @Pagaduria) "+
                            "               and ((@ClienteID is null) or @ClienteID='' or (aux.TerceroID=@ClienteID)) "+
                            "   order by ter.Descriptivo, pag.descriptivo ";

                        #endregion
                    }
                    #endregion
                    #region novedades
                    if (tipoReporte == 2)
                    {
                    }
                    #endregion
                    #region Detalle por Deudores ahora detalle nomina
                    if (tipoReporte == 3)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Cuenta", SqlDbType.Char,UDT_CuentaID.MaxLength);

                        mySqlCommandSel.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                        
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaFin;
                        mySqlCommandSel.Parameters["@Cuenta"].Value = "13809509";
                        mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                        mySqlCommandSel.Parameters["@Pagaduria"].Value = pagaduria;
                        #endregion
                        #region CommanText
                        mySqlCommandSel.CommandText =
                         " select  cc.*,        " +
                         "           (case when VlrAbono > ValorCuota then 'ABONO MAYOR A CUOTA' else   " +
                         "           (case when VlrAbono < ValorCuota then 'ABONO MENOR A CUOTA' else '' end) end) as Observacion  " +
                         "    froM (  " +
                         "       select  mvo.Pagaduria, mvo.NomPagaduria,  " +
                         "               rca.CajaID + ' - ' + CAST(doc.DocumentoNro as varchar(6)) as RecCaja,  " +
                         "                  doc.ComprobanteID + ' - ' + CAST(doc.ComprobanteIDNro as varchar(6)) as Comprobante, doc.Descripcion,  " +
                         "                  aux.TerceroID, ter.Descriptivo as Nombre,  " +
                         "                  aux.DocumentoCOM as Libranza, nom.ValorCuota,  " +
                         "                  -sum(aux.VlrMdaLoc) as VlrAbono  " +
                         "       from coauxiliar aux   " +
                         "       inner join (  " +
                         "                 select aux.NumeroDoc, aux.TerceroID as Pagaduria, ter.Descriptivo as NomPagaduria  " +
                         "                  from coauxiliar aux  " +
                         "                      left join coTercero ter on aux.TerceroID = ter.TerceroID and aux.eg_coTercero = ter.empresaGrupoID  " +
                         "                  where aux.PeriodoID = @Periodo and aux.cuentaID = @Cuenta  " +
                         "                  ) Mvo on aux.numerodoc = mvo.NumeroDoc  " +
                         "              left join glDocumentoControl doc on aux.NumeroDoc = doc.NumeroDoc  " +
                         "              left join tsReciboCajaDocu rca on doc.NumeroDoc = rca.NumeroDoc  " +
                         "              left join ccNominaDeta nom  on doc.NumeroDoc = nom.NumDocRCaja and aux.IdentificadorTR = nom.NumDocCredito  " +
                         "              left join coTercero ter on aux.TerceroID = ter.TerceroID and aux.eg_coTercero = ter.empresaGrupoID  " +
                         "       where doc.estado = 3 and aux.PeriodoID = @Periodo and aux.cuentaID<> @Cuenta AND DOC.EmpresaID = @Empresa  " +
                         "              and((@ClienteID is null) or(aux.TerceroID = @ClienteID))  " +
                         "           and((@Pagaduria is null) or(mvo.Pagaduria = @Pagaduria))  " +
                         "       group by mvo.Pagaduria, mvo.NomPagaduria,   " +
                         "                rca.CajaID, doc.DocumentoNro,  " +
                         "                doc.ComprobanteID, doc.ComprobanteIDNro, doc.Descripcion,  " +
                         "                aux.terceroID, ter.Descriptivo, aux.DocumentoCOM, nom.ValorCuota  " +
                         "       ) cc  " +
                         "   where ValorCuota <> 0  " +
                         "   order by cc.NomPagaduria, Nombre  ";
                        #endregion
                    }
                    #endregion
                    #region Reporte Analisis
                    if (tipoReporte == 4)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@FechaRep", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);


                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@FechaRep"].Value = fechaFin;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaIni;
                        mySqlCommandSel.Parameters["@Pagaduria"].Value = pagaduria;
                        #endregion
                        //oscar david
                        mySqlCommandSel.CommandText =
                        " EXECUTE Cartera_InformePagaduriaAnalisis  @Empresa,@Periodo,@Pagaduria";
                        #region CommanText ini
                        //mySqlCommandSel.CommandText =


                        //    "select	 Libranza " +
                        //    "		,Pagaduria as PagaduriaID" +
                        //    "		,NomPagaduria as Pagaduria" +
                        //    "		,Cliente as ClienteID" +
                        //    "		,NomCliente as Cliente" +
                        //    "		,FechaCorte" +
                        //    "		,FechaLiquida" +
                        //    "		,Plazo" +
                        //    "		,vlrCuota  as ValorCuota" +
                        //    "		,Pagare" +
                        //    "		,Garantia" +
                        //    "		,VlrPrestamo" +
                        //    "		,SdoCapital" +
                        //    "		,VlrPrestamo - SdoCapital  as  VlrAbonos" +
                        //    "		,UltNomina" +
                        //    "		,datediff(day,FechaCorte, @FechaRep) as  DiasMadures" +
                        //    "		,(case	when datediff(month,FechaCorte, @FechaRep) < Plazo " +
                        //    "				then datediff(month,FechaCorte, @FechaRep)+1 else plazo end) as MesMadures" +
                        //    "		,vlrCuota * (case	when datediff(month,FechaCorte, @FechaRep) < Plazo " +
                        //    "							then datediff(month,FechaCorte, @FechaRep)+1 else plazo end) as RecEstimado" +
                        //    "		, vlrCuota * (case	when datediff(month,FechaCorte, @FechaRep) < Plazo " +
                        //    "							then datediff(month,FechaCorte, @FechaRep)+1 else plazo end) - (VlrPrestamo - SdoCapital) as RecaudoMora" +
                        //    "		,0 as RecaudoSol" +
                        //    "		, 0 + vlrCuota as TotalDto" +
                        //    "		, null as Observaciones" +
                        //    "		,Codeudor" +
                        //    "		,NomCodeudor" +
                        //    "		,PagCodeudor" +
                        //    "		,NomPagCodeudor" +
                        //    "		,UltPagoCodeudor" +
                        //    " from" +
                        //    " (" +
                        //    " select	 crd.libranza" +
                        //    "		,crd.PagaduriaID as Pagaduria" +
                        //    "		,pag.Descriptivo as NomPagaduria" +
                        //    "		,crd.ClienteID	as Cliente" +
                        //    "		,cli.Descriptivo as NomCliente" +
                        //    "		,cast(cast(year(crd.FechaLiquida)	as varchar(4)) + '-' + cast(month(crd.FechaLiquida)	as varchar(2)) + '-' +" +
                        //    "		 (case when day(crd.FechaLiquida) < pag.DiaTope then '01' else " +
                        //    "		 (case when month(crd.FechaLiquida) = 2 then '28' else '30' end) end) as date) as FechaCorte" +
                        //    "		,cast(crd.FechaLiquida as date) as FechaLiquida" +
                        //    "		,crd.Plazo" +
                        //    "		,cast(crd.VlrCuota as numeric(10,0)) as VlrCuota" +
                        //    "		,crd.Pagare" +
                        //    "		,(case when crd.cobranzaEstadoID is null or crd.cobranzaEstadoID='' then '' else crd.cobranzaEstadoID end) as Garantia " +
                        //    "		,cast(crd.VlrPrestamo as numeric(10,0)) as VlrPrestamo" +
                        //    "		,cast(sdo.SdoCapital as numeric(10,0))	as SdoCapital" +
                        //    "		,cast(pgc.VlrRecaudo as numeric(10,0))	as VlrRecaudo" +
                        //    "		,cast(pla.SaldoFUT	 as numeric(10,0))	as SaldoFUT" +
                        //    "		,cast(pla.SaldoVEN	 as numeric(10,0))	as SaldoVEN" +
                        //    "		,cast(sdo.UltNomina	 as date)			as UltNomina" +
                        //    "		,cast(sdo.UltPago	 as date)			as UltPago" +
                        //    "		,cast(pla.FechaFinal as date)			as FechaFinal" +
                        //    "		,cast(Pla.CtaAltura	 as date)			as CtaAltura" +
                        //    "		,(case when doc.PeriodoDoc = @Periodo then 'X' else '' end) as Alta" +
                        //    "		,(case when  pgc.VlrRecaudo <> 0 and sdo.SdoCapital = 0 then 'X' else '' end) as Baja" +
                        //    "		,crd.Codeudor1							as Codeudor" +
                        //    "		,(case when cod.NomCodeudor is null then ter.Descriptivo else cod.NomCodeudor end) as NomCodeudor" +
                        //    "		,cod.PagCodeudor" +
                        //    "		,cod.NomPagCodeudor" +
                        //    "		,cod.UltPagoCodeudor" +
                        //    " FROM ccCreditoDocu crd" +
                        //    "	left join glDocumentoControl doc on crd.NumeroDoc = doc.NumeroDoc" +
                        //    "	left join ccCliente cli   on crd.clienteID   = cli.clienteID and crd.eg_ccCliente = cli.EmpresaGrupoID" +
                        //    "	left join ccPagaduria pag on crd.PagaduriaID = pag.PagaduriaID and crd.eg_ccPagaduria = pag.EmpresaGrupoID" +
                        //    "	left join " +
                        //    "		( " +
                        //    "		select	 numeroDoc" +
                        //    "				,min(FechaCuota)				as CtaAltura" +
                        //    "				,Max(FechaCuota)				as FechaFinal" +
                        //    "				,sum(VlrCuota - VlrPagadoCuota) as SaldoFUT" +
                        //    "				,sum(case when FechaCuota <= @FechaRep then VlrCuota - VlrPagadoCuota else 0 end)	as SaldoVEN" +
                        //    "				,sum(case when FechaCuota <= @FechaRep then 1 else 0 end)							as CuotasVEN" +
                        //    "		from ccCreditoPlanPagos" +
                        //    "		where VlrCuota > VlrPagadoCuota " +
                        //    "		group by numeroDoc" +
                        //    "		) pla on crd.NumeroDoc = pla.NumeroDoc" +
                        //    "	left join " +
                        //    "		( " +
                        //    "		select	 mov.numcredito" +
                        //    "				,max(rca.FechaAplica)	as UltNomina" +
                        //    "				,max(doc.FechaDoc)		as UltPago" +
                        //    "				,sum(case	when doc.documentoID <> '161' and doc.documentoID <> '90161'  and com.TipoComponente in (1,4) " +
                        //    "							then -VlrComponente else 0 end) as VlrAbonos" +
                        //    "				,sum(case when mov.componenteCarteraID = '001' then VlrComponente else 0 end) as SdoCapital" +
                        //    "		from ccCarteraMvto mov" +
                        //    "			left join glDocumentoControl	doc on mov.NumeroDoc = doc.NumeroDoc" +
                        //    "			left join tsReciboCajaDocu		rca on mov.NumeroDOc = Rca.NumeroDoc" +
                        //    "			left join ccCarteraComponente	com on mov.componenteCarteraID = com.componenteCarteraID and mov.eg_ccCarteraComponente = com.EmpresaGrupoID" +
                        //    "		group by numcredito" +
                        //    "		) sdo on crd.NumeroDoc = sdo.NumCredito" +
                        //    "	left join " +
                        //    "		( " +
                        //    "		select	 crd.numerodoc" +
                        //    "				,crd.ClienteID" +
                        //    "				,cli.Descriptivo as NomCodeudor" +
                        //    "				,crd.PagaduriaID as PagCodeudor" +
                        //    "				,pag.Descriptivo as NomPagCodeudor" +
                        //    "				,rec.UltPago	 as UltPagoCodeudor" +
                        //    "		from ccCreditodocu crd" +
                        //    "			left join ccCliente		cli on crd.ClienteID   = cli.clienteID	 and crd.eg_ccCliente	= cli.EmpresaGrupoID" +
                        //    "			left join ccPagaduria	pag on crd.PagaduriaID = pag.PagaduriaID and crd.eg_ccPagaduria	= pag.EmpresaGrupoID" +
                        //    "			left join " +
                        //    "				(" +
                        //    "				select	 mov.numcredito" +
                        //    "						,crd.CobranzaEstadoID	as EstadoCrdCod" +
                        //    "						,max(rca.FechaAplica)	as UltNomina" +
                        //    "						,max(doc.FechaDoc)		as UltPago" +
                        //    "				from ccCarteraMvto mov" +
                        //    "					left join ccCreditoDocu			crd on mov.numcredito	= crd.NumeroDoc" +
                        //    "					left join glDocumentoControl	doc on mov.NumeroDoc	= doc.NumeroDoc" +
                        //    "					left join tsReciboCajaDocu		rca on mov.NumeroDOc	= Rca.NumeroDoc" +
                        //    "				group by mov.numcredito, crd.CobranzaEstadoID" +
                        //    "				) rec on crd.Numerodoc = rec.NumCredito" +
                        //    "		) cod on crd.Codeudor1 = cod.ClienteID" +
                        //    "	left join " +
                        //    "		(" +
                        //    "		select	pag.NumeroDoc, pag.PagoDocu, " +
                        //    "				sum(VlrCapital+VlrInteres+VlrSeguro+VlrOtro1+VlrOtro2+VlrOtro3+VlrOtrosFijos) as VlrRecaudo" +
                        //    "		from CCCREDITOPAGOS pag" +
                        //    "			left join glDocumentoControl doc on pag.PagoDocu = doc.NumeroDoc" +
                        //    "			left join tsReciboCajaDocu rec on pag.PagoDocu = rec.NumeroDoc" +
                        //    "		where doc.PeriodoDoc = @Periodo and doc.estado = 3" +
                        //    "		group by pag.NumeroDoc, pag.PagoDocu, doc.FechaDoc" +
                        //    "		) pgc on pgc.Numerodoc = crd.NumeroDoc " +
                        //    "	left join coTercero ter on crd.Codeudor1 = ter.TerceroID and crd.eg_ccCliente	= ter.EmpresaGrupoID" +
                        //    ") rel" +
                        //    " where SdoCapital<>0 or VlrRecaudo<>0 AND " +
                        //    " ((@Pagaduria is null or @Pagaduria='') or (rel.Pagaduria=@Pagaduria))" +
                        //    " order by NomPagaduria, NomCliente";
 
 
                        #endregion

                        

                    }
                    #endregion
                    #region Reporte Auditoria
                    if (tipoReporte == 5)
                    {
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@Empresa", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@FechaRep", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@PeriodoAnt1", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@PeriodoAnt2", SqlDbType.DateTime);
                        mySqlCommandSel.Parameters.Add("@Pagaduria", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                        

                        #endregion
                        
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@Empresa"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@FechaRep"].Value = fechaFin;
                        mySqlCommandSel.Parameters["@Periodo"].Value = fechaIni;
                        mySqlCommandSel.Parameters["@PeriodoAnt1"].Value = fechaFin.AddMonths(-1);
                        mySqlCommandSel.Parameters["@PeriodoAnt2"].Value = fechaFin.AddMonths(-2);
                        mySqlCommandSel.Parameters["@Pagaduria"].Value = pagaduria;
                        #endregion

                        mySqlCommandSel.CommandText =
                        " EXECUTE Cartera_InformePagaduriaAuditoria  @Empresa,@Periodo,@FechaRep,@Pagaduria";

                        #region CommanText ini
                        //mySqlCommandSel.CommandText =

                        //    "select	Pagaduria" +
                        //    "		,NomPagaduria" +
                        //    "		,Cliente		as Cedula" +
                        //    "		,NomCliente		as Nombre" +
                        //    "		,Libranza 		" +
                        //    "		,Plazo		" +
                        //    "		,VlrCuota" +
                        //    "		,(case when VlrMora			IS null then 0 else VlrMora end)	 as ValorMora" +
                        //    "		,(case when VlrIncrMora		IS null then 0 else VlrIncrMora end) as VlrIncrMora" +
                        //    "		,(case when VlrDescuento	IS null then 0 else VlrDescuento end)  + " +
                        //    "		 (case when VlrMora			IS null then 0 else VlrMora end) + " +
                        //    "		  (case when VlrIncrMora	IS null then 0 else VlrIncrMora end)  as VlrDtoMes" +
                        //    "		from " +
                        //    "			( " +
                        //    "		select	 crd.PagaduriaID as Pagaduria		" +
                        //    "				,pag.Descriptivo as NomPagaduria		" +
                        //    "				,crd.ClienteID	as Cliente		" +
                        //    "				,cli.Descriptivo as NomCliente		" +
                        //    "				,crd.libranza		" +
                        //    "				,crd.Plazo		" +
                        //    "				,cast(crd.VlrCuota	as numeric(10,0)) as VlrCuota		" +
                        //    "				,cie.SaldoVEN		as VlrMora" +
                        //    "				,(case when cie.SaldoVEN > cie.SaldoANT then cie.SaldoVEN -cie.SaldoANT else 0 end) as VlrIncrMora" +
                        //    "				,(case when cie.SaldoNV > crd.VlrCuota or cie.SaldoNV = 0 and doc.PeriodoDoc = @Periodo then crd.VlrCuota else cie.SaldoNV end) as VlrDescuento		" +
                        //    "		FROM ccCreditoDocu crd	" +
                        //    "				left join glDocumentoControl doc on crd.NumeroDoc = doc.NumeroDoc	" +
                        //    "				left join ccCliente cli   on crd.clienteID   = cli.clienteID and crd.eg_ccCliente = cli.EmpresaGrupoID	" +
                        //    "				left join coTercero ter on crd.Codeudor1 = ter.TerceroID and crd.eg_ccCliente	= ter.EmpresaGrupoID" +
                        //    "				left join ccPagaduria pag on crd.PagaduriaID = pag.PagaduriaID and crd.eg_ccPagaduria = pag.EmpresaGrupoID	" +
                        //    "				left join " +
                        //    "						(" +
                        //    "						select numerodoc," +
                        //    "							(case when periodo = @PeriodoAnt2 then Saldo30+Saldo60+Saldo90+Saldo180+Saldo360+Saldo360m else 0 end) as SaldoANT," +
                        //    "							(case when periodo = @PeriodoAnt1 then Saldo30+Saldo60+Saldo90+Saldo180+Saldo360+Saldo360m else 0 end) as SaldoVEN," +
                        //    "							(case when periodo = @PeriodoAnt1 then SaldoNV else 0 end) as SaldoNV" +
                        //    "						from cccierremescartera" +
                        //    "						where periodo >= @PeriodoAnt1" +
                        //    "						) cie on crd.NumeroDoc = cie.NumeroDoc" +
                        //    "		WHERE doc.Estado = 3 and crd.FechaCuota1 <= @Periodo" +
                        //    "		) " +
                        //    "		rel " +
                        //    "		where VlrDescuento<>0 AND ((@Pagaduria is null or @Pagaduria='') or (rel.Pagaduria=@Pagaduria)) " +
                        //    "		order by NomPagaduria, NomCliente";
                        #endregion
                    }
                    #endregion

                    
                }
                #endregion
                #region Creditos
                else if (documentoID == AppReports.ccReporteCredito)
                {
                    #region Relacion Creditos
                    if (tipoReporte == 1)
                    {
                        #region Configuracion de Filtros
                        string filtros = "";
                        if (filter != null)
                        {
                            filtros += " AND Docu.TipoCreditoID = @TipoCredito ";
                            mySqlCommandSel.Parameters.Add("@TipoCredito", SqlDbType.Char,5);
                            mySqlCommandSel.Parameters["@TipoCredito"].Value = filter.ToString();
                            mySqlCommandSel.Parameters.Add("@Romp", SqlDbType.TinyInt);
                            mySqlCommandSel.Parameters["@Romp"].Value = Romp;
                            if (Romp == 1)
                                filtros += " AND (Tipo.TipoCredito= 1 or  Tipo.TipoCredito=2)";


                        }
                            
                        #endregion
                        #region Parametros
                        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                        mySqlCommandSel.Parameters.Add("@FechaCorte", SqlDbType.DateTime);
                        #endregion
                        #region Asignacion Valores a Parametros
                        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@FechaCorte"].Value = fechaFin;
                        #endregion

                        #region CommanText

                        mySqlCommandSel.CommandText =
                                    "   Select UsuarioID,Cliente,Cedula," +
	                                "    		Telefono, celular1, Correo, TipoCreditoID,Libranza,Plazo,VlrCuota,VlrLibranza,VlrPrestamo,VlrGiro,VlrDescuento" +
                                    "   		,DesCuota, FechaCuota1 as FechaCuota, INTERESES,SdoCarteraExterna,SdoCarteraInterna," +
	                                "   		(VlrPrestamo - VlrGiro - VlrDescuento - DesCuota - Intereses - SdoCarteraExterna - SdoCarteraInterna) as Diferencia," +
	                                "   		FechaLiquida,Caja,Activo,AsesorP,AsesorC,Observacion,Reportado " +
	                                "   		from" +
	                                "   			(" +
	                                "   			Select  rtrim(usu.UsuarioID) as UsuarioID,Isnull(rtrim(clie.ApellidoPri), '') + ' ' + Isnull(rtrim(clie.ApellidoSdo), '') + ' ' + Isnull(rtrim(clie.NombrePri), '') + ' ' + Isnull(rtrim(clie.NombreSdo), '') as Cliente," +
	                                "   					clie.Telefono1 as Telefono, clie.celular1, clie.Correo," +
	                                "   					docu.ClienteID as Cedula," +
	                                "   					Docu.TipoCreditoID,docu.Libranza," +
	                                "   					docu.Plazo," +
	                                "   					docu.VlrCuota," +
	                                "   					docu.VlrLibranza," +
	                                "   					docu.VlrPrestamo," +
	                                "   					docu.VlrGiro," +
	                                "   					docu.VlrDescuento - ISNULL((select top(1) ValorDtoPrimeraCuota from ccSolicitudDocu where NumeroDoc = Docu.NumSolicitud), 0) as VlrDescuento," +
	                                "   					ctrl.NumeroDoc," +
	                                "   					ISNULL((select top(1) ValorDtoPrimeraCuota from ccSolicitudDocu where NumeroDoc = Docu.NumSolicitud),0) as DesCuota," +
	                                "   					docu.FechaCuota1," +
	                                "   					0 as Intereses," +
	                                "   					ISNULL((" +
	                                "   						select sum(VlrSaldo) " +
	                                "   						from ccCreditoCompraCartera c " +
	                                "   						inner  join ccFinanciera f on f.FinancieraID = c.FinancieraID and f.EmpresaGrupoID = c.eg_ccFinanciera" +
	                                "   						where NumeroDoc = Docu.NumeroDoc and f.TipoEmpresa <> 0),0) as SdoCarteraExterna," +
	                                "   					ISNULL((" +
	                                "   						select sum(VlrSaldo)" +
	                                "   						from ccCreditoCompraCartera c" +
	                                "   						inner  join ccFinanciera f on f.FinancieraID = c.FinancieraID and f.EmpresaGrupoID = c.eg_ccFinanciera" +
	                                "   						where NumeroDoc = Docu.NumeroDoc and f.TipoEmpresa = 0),0) as SdoCarteraInterna," +
	                                "   						docu.FechaLiquida," +
	                                "   						Pag.Descriptivo as Caja," +
	                                "   						'' as Activo," +
	                                "   						Ase.Descriptivo as AsesorP," +
	                                "   						Con.Descriptivo as AsesorC," +
	                                "   						'' as Observacion," +
	                                "   						'' as Reportado" +
	                                "   				from CCCREDITODOCU Docu" +
	                                "   				inner  join glDocumentoControl ctrl on docu.NumeroDoc = ctrl.NumeroDoc" +
	                                "   				inner join ccCliente Clie on docu.ClienteID = clie.ClienteID and Clie.EmpresaGrupoID = docu.eg_ccCliente" +
	                                "   				inner join  seUsuario usu on usu.ReplicaID = ctrl.seUsuarioID" +
	                                "   				left join ccPagaduria Pag on Pag.PagaduriaID = Docu.PagaduriaID" +	
	                                "   				left join ccAsesor Ase on Ase.AsesorID = Docu.AsesorID" +
	                                "   				left join ccConcesionario Con on Con.ConcesionarioID = Ase.ConcesionarioID" +	
	                                "   				left join ccTipoCredito Tipo on Docu.TipoCreditoID=tipo.TipoCreditoID" +	
	                                "   				where ctrl.EmpresaID =  @EmpresaID and ctrl.PeriodoDoc = @FechaCorte" +
	                                "   		) 	final" +
	                                "   	Order by final.Cliente" ;
                        //mySqlCommandSel.CommandText =
                        //   " Select UsuarioID,Cliente,Cedula," +
                        //   "		Telefono, celular1, Correo, TipoCreditoID,Libranza,Plazo,VlrCuota,VlrLibranza,VlrPrestamo,VlrGiro,VlrDescuento" +
                        //   "		,DesCuota,INTERESES,SdoCarteraExterna,SdoCarteraInterna,    " +
                        //   "		(VlrPrestamo - VlrGiro - VlrDescuento - DesCuota - Intereses - SdoCarteraExterna - SdoCarteraInterna) as Diferencia," +
                        //   "	 FechaLiquida,Caja,Activo,AsesorP,AsesorC,Observacion,Reportado" +
                        //   " from " +
                        //   "	(" +
                        //   "	Select  rtrim(usu.UsuarioID) as UsuarioID," +
                        //               "Isnull(rtrim(clie.ApellidoPri), '') + ' ' + Isnull(rtrim(clie.ApellidoSdo), '') + ' ' + Isnull(rtrim(clie.NombrePri), '') + ' ' + Isnull(rtrim(clie.NombreSdo), '') as Cliente," +
                        //   "			clie.Telefono1 as Telefono, clie.celular1, clie.Correo," +
                        //   "			docu.ClienteID as Cedula," +
                        //   "			Docu.TipoCreditoID,docu.Libranza," +
                        //   "			docu.Plazo," +
                        //   "			docu.VlrCuota," +
                        //   "			docu.VlrLibranza," +
                        //   "			docu.VlrPrestamo," +
                        //   "			docu.VlrGiro," +
                        //   "			docu.VlrDescuento - ISNULL((select top(1) ValorDtoPrimeraCuota from ccSolicitudDocu where NumeroDoc = Docu.NumSolicitud), 0) as VlrDescuento," +
                        //   "			ctrl.NumeroDoc," +
                        //   "			ISNULL((select top(1) ValorDtoPrimeraCuota from ccSolicitudDocu where NumeroDoc = Docu.NumSolicitud),0) as DesCuota," +
                        //   "			0 as Intereses," +
                        //   "			ISNULL((select sum(VlrSaldo) " +
                        //   "					from ccCreditoCompraCartera c" +
                        //   "						inner  join ccFinanciera f on f.FinancieraID = c.FinancieraID and f.EmpresaGrupoID = c.eg_ccFinanciera" +
                        //   "					where NumeroDoc = Docu.NumeroDoc and f.TipoEmpresa <> 0),0) as SdoCarteraExterna," +
                        //   "			ISNULL((select sum(VlrSaldo) " +
                        //   "					from ccCreditoCompraCartera c" +
                        //   "						inner  join ccFinanciera f on f.FinancieraID = c.FinancieraID and f.EmpresaGrupoID = c.eg_ccFinanciera" +                           
                        //   "					where NumeroDoc = Docu.NumeroDoc and f.TipoEmpresa = 0),0) as SdoCarteraInterna,               " +
                        //   "			docu.FechaLiquida," +
                        //   "			Pag.Descriptivo as Caja," +
                        //   "			'' as Activo, " +
                        //   "			Ase.Descriptivo as AsesorP," +
                        //   "			Con.Descriptivo as AsesorC," +
                        //   "			'' as Observacion," +          
                        //   "			'' as Reportado" +
                        //   "		from CCCREDITODOCU Docu " +
                        //   "			inner  join glDocumentoControl ctrl on docu.NumeroDoc = ctrl.NumeroDoc" +
                        //   "			inner join ccCliente Clie on docu.ClienteID = clie.ClienteID and Clie.EmpresaGrupoID = docu.eg_ccCliente" +
                        //   "			inner join  seUsuario usu on usu.ReplicaID = ctrl.seUsuarioID" + 
                        //   "			left join ccPagaduria Pag on Pag.PagaduriaID = Docu.PagaduriaID" +      
                        //   "			left join ccAsesor Ase on Ase.AsesorID = Docu.AsesorID" +
                        //   "			left join ccConcesionario Con on Con.ConcesionarioID = Ase.ConcesionarioID" +       
                        //   "			left join ccTipoCredito Tipo on Docu.TipoCreditoID=tipo.TipoCreditoID    " +    
                        //   "		where ctrl.EmpresaID =  @EmpresaID and ctrl.PeriodoDoc = @FechaCorte" +
                        //   "	) " +
                        //   "	final" +
                        //   "    Order by final.Cliente ";

                        #endregion
                    }
                    #endregion
                    #region Extractos
                    if (tipoReporte == 2)
                    {
                    }
                    #endregion
                }
                #endregion
                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_CarteraToExcel");
                return null;

            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de cartera segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="zonaID">Zona</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="ConcesionarioID">Concecionario</param>
        /// <param name="asesor">Asesor</param>
        /// <param name="lineaCredi">LineaCredito</param>
        /// <param name="compCartera">CompradorCartera</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable DAL_ReportesCartera_Cc_CobroJuridicoToExcel(int documentoID, byte tipoReporte, string cliente, string libranza, byte claseDeuda)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                    
                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@TipoReporte", SqlDbType.TinyInt);
                    mySqlCommandSel.Parameters.Add("@ClienteID", SqlDbType.Char, UDT_ClienteID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Libranza", SqlDbType.Int);
                    mySqlCommandSel.Parameters.Add("@ClaseDeuda", SqlDbType.TinyInt);

                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@TipoReporte"].Value = tipoReporte;
                    mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    mySqlCommandSel.Parameters["@Libranza"].Value = libranza;
                    mySqlCommandSel.Parameters["@ClaseDeuda"].Value = claseDeuda;
                    #endregion
                    #region CommanText


                        mySqlCommandSel.CommandText = "Select	 his.NumeroDoc, " +
                                                      "          his.TipoMvto, " +
                                                      "          his.FechaMvto," +
                                                      "          his.SaldoCapital as VlrCapital," +
                                                      "          his.FechaInicial, " +
                                                      "          his.FechaFinal, " +
                                                      "          his.Dias as DiasMora, " +
                                                      "          his.PorInteres as Tasa," +
                                                      "          (CASE WHEN his.MvtoInteres is null THEN 0 ELSE his.MvtoInteres End) as InteresMora" +
                                                      "  From	 ccCJHistorico as his" +
                                                      "  inner join ccCreditoDocu as cred on his.NumeroDoc = cred.NumeroDoc" +
                                                      "  Where	 cred.EmpresaID = @EmpresaID and TipoMvto = 2 or TipoMvto = 3 " +
                                                      "           and ((@ClienteID is null) or (cred.ClienteID=@ClienteID))" +
                                                      "           and ((@Libranza is null) or (cred.Libranza=@Libranza))" +
                                                      "  Order by NumeroDoc, FechaMvto ";


                    #endregion
                    sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_CarteraToExcel");
                return null;

            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de datacredito
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable DAL_ReportesCartera_Cc_DataCredito(DateTime periodo, byte tipo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable table = new DataTable();
                #region CommanText               
                mySqlCommandSel = new SqlCommand("Cartera_Report_DataCredito", MySqlConnection.CreateCommand().Connection);
                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add(new SqlParameter("@EmpresaID", this.Empresa.ID.Value));
                mySqlCommandSel.Parameters.Add(new SqlParameter("@Periodo", periodo));
                mySqlCommandSel.Parameters.Add(new SqlParameter("@Tipo", tipo));
                #endregion                

                mySqlCommandSel.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = mySqlCommandSel;

                if (!string.IsNullOrEmpty(mySqlCommandSel.CommandText))
                    sda.Fill(table);

                foreach (DataRow item in table.Rows)
                    item["Dato"] = item.ItemArray[0].ToString().Length > 0 ? item.ItemArray[0].ToString().TrimEnd().Substring(0, item.ItemArray[0].ToString().TrimEnd().Length - 1) : string.Empty;
                
                return table;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ReportesCartera_Cc_CarteraToExcel");
                return null;

            }
        }

        #region Crédito Formulario Excel 1
        /// <summary>
        /// Reporte Credito // Tabla Amortizacion
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <param name="año"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        public DataTable DAL_Reports_cc_CreditoXLS(string Credito)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();

                #region CommanText
                mySqlCommandSel.CommandText =

                        " DECLARE @Query		VARCHAR (4000)  " +
                        " DECLARE @Columna	VARCHAR (2000)  " +
                        " SELECT @Columna=STUFF((SELECT DISTINCT'],['+ ComponenteCarteraID FROM ccCreditoComponentes ORDER BY  '],['+ ComponenteCarteraID FOR XML PATH('') ),1,2,'')+']'   " +
                        " SET @Query = 'Select  *  " +
                        " 					from(  " +
                        " SELECT	COM.NumeroDoc,  " +
                        " 		PAG.CuotaID,  " +
                        " 		COM.ComponenteCarteraID,  " +
                        " 		TER.Descriptivo AS Nombre,  " +
                        " 		TER.TerceroID AS Cedula,  " +
                        " 		DOC.Libranza,  " +
                        " 		CTL.FechaDoc AS Fecha_Liquidacion,  " +
                        " 		YEAR(CTL.FechaDoc) AS Año_Liquidacion,  " +
                        " 		DOC.Plazo,	  " +
                        " 		DOC.VlrGiro AS Monto_Desembolso,  " +
                        " 		DOC.PorInteres AS Interes_Pactado,  " +
                        " 		PAG.FechaCuota,  " +
                        " 		PAG.VlrSaldoCapital AS Saldo_Inicial,  " +
                        " 		PAG.VlrCuota		AS Cuota,  " +
                        " 		PAG.VlrCapital		AS Abono_Capital,  " +
                        " 		PAG.VlrInteres		AS Intereses,  " +
                        " 		PAG.VlrSeguro		AS Seguro_Obligatorio,  " +
                        " 		PAG.VlrOtrosfijos	AS Aportes,   " +
                        " 		(PAG.VlrCapital * COM.PorCapital / 100) AS Prorrateado  " +
                        " FROM ccCreditoComponentes		COM    " +
                        " INNER JOIN ccCreditoPlanPagos	PAG ON COM.NumeroDoc = PAG.NumeroDoc  " +
                        " INNER JOIN glDocumentoControl	CTL ON COM.NumeroDoc = CTL.NumeroDoc AND COM.eg_ccCarteraComponente = CTL.EmpresaID  " +
                        " INNER JOIN coTercero			TER ON CTL.TerceroID = TER.TerceroID AND CTL.EmpresaID = TER.EmpresaGrupoID  " +
                        " INNER JOIN ccCreditoDocu		DOC ON CTL.NumeroDoc = DOC.NumeroDoc AND CTL.EmpresaID = DOC.EmpresaID  " +
                        " WHERE		COM.PorCapital != 0  " +
                        " 						) TABLA2	  " +
                        " PIVOT  " +
                        " (sum(Prorrateado) FOR ComponenteCarteraID IN ('+@Columna+')) AS PivotTable ORDER BY NumeroDoc;'  " +
                        " EXECUTE (@Query)  ";

                #endregion
                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Reports_cc_CreditoXLS");
                return null;

            }
        } 
        #endregion

        #region Credito Formulario Excel 2
        /// <summary>
        /// Descripcion componentes en Query Pivot // Tabla Amortizacion
        /// </summary>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <param name="año"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        public DataTable DAL_Reports_cc_DescripcionComponentesXLS(string Credito)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();

                #region CommanText
                mySqlCommandSel.CommandText =
                "   DECLARE @Texto		VARCHAR (500)   " +
                "   DECLARE @Titulos	VARCHAR (2000)   " +
                "   SELECT @Titulos=STUFF((SELECT DISTINCT'],['+ ComponenteCarteraID FROM ccCarteraComponente ORDER BY  '],['+ ComponenteCarteraID FOR XML PATH('') ),1,2,'')+']'    " +
                "   SET @Texto = 'Select  *   " +
                "   					from(   " +
                "   						SELECT	ComponenteCarteraID,   " +
                "   								Descriptivo   " +
                "   						FROM ccCarteraComponente   " +
                "   						) TABLA2						   " +
                "   PIVOT   " +
                "   (MIN(Descriptivo) FOR ComponenteCarteraID IN ('+@Titulos+')) AS PivotTable;'   " +
                "   EXECUTE (@Texto)   ";
                #endregion
                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Reports_cc_DescripcionComponentesXLS");
                return null;

            }
        } 
        #endregion

        #region Reporte Excel Preventa
        /// <summary>
        /// Exportar Reporte Excel de la preventa
        /// </summary>
        /// <param name="_libranzaTercero"></param>
        /// <param name="_cedulaTercero"></param>
        /// <returns></returns>
        public DataTable DAL_ExportExcel_cc_GetVistaCesionesByPreventa(List<int> numeroDocs)
        {
            try
            {
                string whereNumDoc = " and (";

                foreach (int item in numeroDocs)
	            {
                    if (item != numeroDocs.Last())
                        whereNumDoc += " NumeroDoc = " + item.ToString() + " or "; 
                    else
                        whereNumDoc += " NumeroDoc = " + item.ToString(); 
	            }
                whereNumDoc += ")";

                if (numeroDocs.Count == 0) whereNumDoc = string.Empty;

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                SqlDataAdapter sda = new SqlDataAdapter();                

                #region Parametros
                mySqlCommandSel.Parameters.Add("@_empresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                //mySqlCommandSel.Parameters.Add("@NumeroDoc",SqlDbType.Int);
                #endregion

                #region Asignacion Valores a Parametros
                mySqlCommandSel.Parameters["@_empresaID"].Value = this.Empresa.ID.Value;
                //mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDocs;

                #endregion

                #region CommanText
                mySqlCommandSel.CommandText =

                                        "   SELECT  " +
                                        "   	*   " +
                                        "   FROM   " +
                                        "   	VistaQ_ccCesiones C  " +
                                        "   WHERE   " +
                                        "   	C.EmpresaID = @_empresaID  " + whereNumDoc +
                                        "   ORDER BY NumeroDoc  " ;

                #endregion
                List<DTO_VistaQ_ccCesiones> result = new List<DTO_VistaQ_ccCesiones>();

                sda.SelectCommand = mySqlCommandSel;
                DataTable table = new DataTable();

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ExportExcel_cc_GetVistaCesionesByPreventa");
                return null;

            }
        }  
        #endregion

    
        #endregion

    }
}

