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
        public DataTable DAL_ReportesCartera_Cc_CarteraToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string ConcesionarioID, string asesor, string lineaCredi, string compCartera, string pagaduria, string centroPago, byte? Agrupamiento, byte? Romp,object filter)
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

                    if (tipoReporte != 1) //SALDOS
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
                    else //CUOTA
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
                if (documentoID == AppReports.ccAmortizacion)
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
                if (documentoID == AppReports.ccSaldoCapital)
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
                if (documentoID == AppReports.ccPolizaEstado)
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
                if (documentoID == AppReports.ccRecaudosNominaDeta)
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
                if (documentoID == AppReports.ccSaldosAFavor)
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
                #region SaldoSeguro
                if (documentoID == AppReports.ccSaldosSeguroVida)
                {
                    #region Configuracion de Filtros
                    // Carga los filtros de acuerdo a la parametrizacion del usuario
                    string filtros = "";
                    if (!string.IsNullOrEmpty(cliente))
                    {
                        filtros += "AND cli.ClienteID = @ClienteID  ";
                        mySqlCommandSel.Parameters["@ClienteID"].Value = this.Empresa.ID.Value;
                        mySqlCommandSel.Parameters["@ClienteID"].Value = cliente;
                    }                    
                    #endregion

                    #region Parametros
                    mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                    mySqlCommandSel.Parameters.Add("@Periodo", SqlDbType.DateTime);
                    mySqlCommandSel.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                    #endregion
                    #region Asignacion Valores a Parametros
                    mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommandSel.Parameters["@Periodo"].Value = fechaIni;
                    #endregion

                    #region CommanText
                    mySqlCommandSel.CommandText =
                    "    DECLARE @EmpresaNumCtrl AS VARCHAR(4)	" +
                    "    DECLARE @CodigoCartera AS VARCHAR(6)		" +
                    "    DECLARE @PorcSeguroDeudor AS numeric(10,4)	" +
                    "    DECLARE @PorcSeguroConyuge AS numeric(10,4)	" +
                    "    SELECT @EmpresaNumCtrl = NumeroControl FROM glEmpresa WITH(NOLOCK) WHERE EmpresaID =  @EmpresaID	" +
                    "    SET @CodigoCartera = @EmpresaNumCtrl + '16' 	" +

                    "    SELECT @PorcSeguroDeudor = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '095' AS INT)	" +
                    "    SELECT @PorcSeguroConyuge = Data FROM glControl WHERE glControlId = CAST(@CodigoCartera + '096' AS INT)	" +

                    "   SELECT q.*, Round((CapitalSDO*PorSeguro/100),0) as VlrSeguro,Round((CapitalSDO*PorcExtra/1000),0) as VlrSeguroExtra, Round((CapitalSDO*PorSeguro/100)+(CapitalSDO*PorcExtra/1000),0) as VlrTotal FROM  " +
                    "    (	" +
                    "        (	" +
                    "             Select 'D' as Tipo,CAST(cli.ClienteID as bigint)as ClienteID, cli.ClienteID as CedConyuge, cli.Descriptivo as Nombre,cli.FechaNacimiento, 	" +
			        "                    @PorcSeguroDeudor as PorSeguro,ISNULL(cli.ExtraPrimaCliente,0) as PorcExtra,SUM(cierre.CapitalSDO) as CapitalSDO	" +
	                "             From ccCierremescartera cierre 	" +
			        "                    inner join ccCreditoDocu crd on cierre.numerodoc = crd.NumeroDoc 	" +
			        "                    inner join ccCliente cli on crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente	" +
                    "              Where CAST(Periodo as DATE) = @Periodo 	" + filtros +
	                "              Group  by cli.ClienteID,cli.Descriptivo,cli.FechaNacimiento,cli.ExtraPrimaCliente	" +
                    "        )	" +
                    "        Union	" +
                    "        (	" +
                    "             Select 'C' as Tipo,CAST(cli.ClienteID as bigint) as ClienteID, cli.CedEsposa as CedConyuge, cli.NomEsposa as Nombre,cli.FechEsposa as FechaNacimiento, 	" +
			        "                     @PorcSeguroConyuge as PorSeguro, ISNULL(cli.ExtraPrimaConyugue,0) as PorcExtra,SUM(cierre.CapitalSDO) as CapitalSDO	" +
	                "              From ccCierremescartera cierre 	" +
			        "                    inner join ccCreditoDocu crd on cierre.numerodoc = crd.NumeroDoc 	" +
			        "                    inner join ccCliente cli on crd.ClienteID = cli.ClienteID and cli.EmpresaGrupoID = crd.eg_ccCliente and cli.CedEsposa is not null	" +
                    "              Where CAST(Periodo as DATE) = @Periodo and (DATEDIFF(year,cli.FechEsposa,GETDATE()) <= 55 or cli.FechEsposa is null)	" + filtros +
	                "              Group  by cli.ClienteID,cli.CedEsposa,cli.NomEsposa,cli.FechEsposa,cli.ExtraPrimaConyugue    "    +
                    "       )	" +
                    "    )q where CapitalSDO > 0  order by  q.CLienteID asc, q.Tipo desc"; 
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

