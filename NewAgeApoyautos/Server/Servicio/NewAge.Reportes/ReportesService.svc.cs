using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.ServiceModel;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using NewAge.Librerias.Project;
using NewAge.Reports;
using NewAge.DTO.Resultados;
using NewAge.Reports.Fijos.Documentos.Cartera;
using NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar;
using NewAge.Reports.Fijos.Documentos.Tesoreria;
using NewAge.Reports.Fijos.General;
using NewAge.Reports.Fijos;
using NewAge.DTO.UDT;
using NewAge.Reports.Dinamicos;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Reportes;

namespace NewAge.Server.ReportesService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall), System.Runtime.InteropServices.GuidAttribute("6FC99881-1F0E-4CA3-8A70-4ED73EC9BE0C")]
    public class ReportesService : IReportesService
    {
        #region Variables

        /// <summary>
        /// Diccionario con la lista de conexiones (canales abiertos)
        /// </summary>
        private static Dictionary<Guid, Tuple<DTO_glEmpresa, int>> _channels = new Dictionary<Guid, Tuple<DTO_glEmpresa, int>>();

        /// <summary>
        /// Lista de procesos que se están corriendo
        /// </summary>
        private static List<int> _currentProcess = new List<int>();

        /// <summary>
        /// Cadena de conexion
        /// </summary>
        private string _connString = string.Empty;

        /// <summary>
        /// Cadena de conexion
        /// </summary>
        private string _connLoggerString = string.Empty;

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        private List<SqlConnection> _mySqlConnections = new List<SqlConnection>();

        #endregion

        #region Modelos

        /// <summary>
        /// Fachada de acceso a los modulos
        /// </summary>
        ModuloFachada facade = new ModuloFachada();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportesService()
        {
            this._connString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString();
            this._connLoggerString = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString();
            if (this._mySqlConnections.Count == 0)
            {
                SqlConnection conn = new SqlConnection(this._connString);
                this._mySqlConnections.Add(conn);
            }
        }

        /// <summary>
        /// Constructor con el nombre de una cadena de conexion determinada
        /// </summary>
        /// <param name="connecctionStringName">Cadena de conexion completa</param>
        public ReportesService(string connString, string connLoggerString)
        {
            this._connString = connString;
            this._connLoggerString = connLoggerString;
            if (this._mySqlConnections.Count == 0)
            {
                SqlConnection conn = new SqlConnection(this._connString);
                this._mySqlConnections.Add(conn);
            }
        }

        /// <summary>
        /// Crea una nueva sesion pra el usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        public void CrearCanal(Guid channel)
        {
            Tuple<DTO_glEmpresa, int> t = new Tuple<DTO_glEmpresa, int>(null, 0);
            if (!_channels.ContainsKey(channel))
                _channels.Add(channel, t);
        }

        /// <summary>
        /// Cierra la sesion de un usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        public void CerrarCanal(Guid channel)
        {
            if (_channels.ContainsKey(channel))
                _channels.Remove(channel);
        }

        /// <summary>
        /// Inicializa la empresa y el usuario
        /// </summary>
        public void IniciarEmpresaUsuario(Guid channel, DTO_glEmpresa emp, int userID)
        {
            Tuple<DTO_glEmpresa, int> t = new Tuple<DTO_glEmpresa, int>(emp, userID);
            _channels[channel] = t;
        }

        #endregion

        #region DBConnection

        /// <summary>
        /// Retorna el indice de una conexion que este disponible
        /// </summary>
        /// <returns>Retorna una conexion disponible</returns>
        private int GetConnectionIndex()
        {
            int result = -1;
            SqlConnection conn;
            for (int i = 0; i < this._mySqlConnections.Count; ++i)
            {
                conn = this._mySqlConnections[i];
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    result = i;
                }
            }

            if (result == -1)
            {
                conn = new SqlConnection(this._connString);
                this._mySqlConnections.Add(conn);
                result = this._mySqlConnections.Count - 1;
            }

            return result;
        }

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        /// <returns>Retorna el indice con la conexion que se esta usando</returns>
        private int ADO_ConnectDB()
        {
            int connIndex = -1;
            try
            {
                connIndex = this.GetConnectionIndex();
                this._mySqlConnections[connIndex].Open();
                return connIndex;
            }
            catch
            {
                this.ADO_CloseDBConnection(connIndex);
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        public void ADO_CloseDBConnection(int connIndex)
        {
            try
            {
                if (connIndex == -1)
                {
                    this._mySqlConnections = new List<SqlConnection>();

                    SqlConnection conn = new SqlConnection(this._connString);
                    this._mySqlConnections.Add(conn);
                }
                else
                {
                    if (this._mySqlConnections[connIndex].State != ConnectionState.Closed)
                        this._mySqlConnections[connIndex].Close();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción
        /// </summary>
        /// <param name="DocumentID">Identificador del proceso (documento)</param>
        /// <returns>Retorna el porcentaje del progreso</returns>
        public int ConsultarProgreso(Guid channel, int documentID)
        {
            int prog = DictionaryProgress.ConsultarProgreso(_channels[channel].Item2, documentID);
            return prog == 0 ? 1 : prog;
        }

        #endregion

        #region Reportes

        #region Activos

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        ///<param name="formatType">Tipo de Formato a exportar el reporte</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public DTO_TxResult ReportesActivos_Saldos(Guid channel, string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Ac_Saldos reporte = new Report_Ac_Saldos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, true);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public DTO_TxResult ReportesActivos_SaldosML(Guid channel, string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Ac_SaldosML reporte = new Report_Ac_SaldosML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, true);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public DTO_TxResult ReportesActivos_SaldosME(Guid channel, string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Ac_SaldosME reporte = new Report_Ac_SaldosME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, false);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        public string ReportesActivos_ComparacionLibros(Guid channel, int año, int mes, string clase, string tipo, string grupo, string centroCost, string logFis,
            string proyecto, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Ac_ComparacionLibros reporte = new Report_Ac_ComparacionLibros(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, clase, tipo, grupo, centroCost, logFis, proyecto);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que carga los equipos arrendados
        /// </summary>
        /// <param name="channel">Canal de trasnmicion</param>
        /// <param name="libranza">Libranza q se liquida</param>
        /// <param name="formatType">Foma de exoportar al reporte</param>
        /// <param name="numDoc">Numero DOC para guardar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesActivos_EquiposArrendados(Guid channel, DateTime Periodo, int Estado, string Tercero, string Plaqueta, string Serial, string TipoRef, string Rompimiento, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Ac_EquiposArrendados reporte = new Report_Ac_EquiposArrendados(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, Estado, Tercero, Plaqueta, Serial, TipoRef, Rompimiento);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que carga las importanciones temporales
        /// </summary>
        /// <param name="channel">Canal de trasnmicion</param>
        /// <param name="libranza">Libranza q se liquida</param>
        /// <param name="formatType">Foma de exoportar al reporte</param>
        /// <param name="numDoc">Numero DOC para guardar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesActivos_ImpotacionesTemporales(Guid channel, DateTime Periodo, string Plaqueta, string Serial, string TipoRef, string Rompimiento, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Ac_ImportacionesTemporales reporte = new Report_Ac_ImportacionesTemporales(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, Plaqueta, Serial, TipoRef, Rompimiento);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }
        #endregion

        #region Balance

        /// <summary>
        /// Servicio que retoran el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de transmision de datos</param>
        /// <param name="libroAux">Parametro de entrada (Libro contra el cual se va a comparar el libro FUNC)</param>
        /// <param name="moneda">Tipo de Moneda en que se va a imprimir el reporte</param>
        /// <param name="año">Año </param>
        /// <param name="fechaFin"></param>
        /// <param name="fechaIni"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>

        public string ReportesContabilidad_BalanceComparativo(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_BalanceComparativoMtvosML reporte = new Report_Co_BalanceComparativoMtvosML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libroAux, moneda, año, fechaFin, fechaIni);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        public string ReportesContabilidad_BalanceComparativoME(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_BalanceComparativoMtvosME reporte = new Report_Co_BalanceComparativoMtvosME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libroAux, moneda, año, fechaFin, fechaIni);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        public string ReportesContabilidad_BalanceComparativosSaldosML(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_BalanceComparativoSaldosML reporte = new Report_Co_BalanceComparativoSaldosML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libroAux, moneda, año, fechaFin, fechaIni);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        public string ReportesContabilidad_BalanceComparativosSaldosME(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_BalanceComparativoSaldosME reporte = new Report_Co_BalanceComparativoSaldosME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libroAux, moneda, año, fechaFin, fechaIni);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        public string ReportesContabilidad_BalanceComparativosSaldosAM(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_BalanceComparativoSaldosAM reporte = new Report_Co_BalanceComparativoSaldosAM(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libroAux, moneda, año, fechaFin, fechaIni);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        #endregion

        #region Cartera

        #region Cartas de ofertas para la preventa y venta Cartera

        /// <summary>
        /// Genera reporte de oferta
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public string Report_Cc_Oferta(Guid channel, int numeroDoc, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_OfertaCerrada reporte = new Report_Cc_OfertaCerrada(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(numeroDoc);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Aportes(Guid channel, DateTime mes, DateTime fechaIni, DateTime fechaFin, string filter, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Aportes reporte = new Report_Cc_Aportes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, mes, formatType);
                return reporte.GenerateReport(mes, fechaIni, fechaFin, filter);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo a consultar</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <returns>Nombre del Reporte</returns>
        public string Report_Cc_AportesCliente(Guid channel, DateTime periodo, string clienteFiltro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_AportesCliente reporte = new Report_Cc_AportesCliente(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(periodo, clienteFiltro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo a consultar</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <returns>Nombre del Reporte</returns>
        public string Report_Cc_Aportes_a_Clientes(Guid channel,int Año, int Mes,string _tercero, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Aportes_a_Clientes reporte = new Report_Cc_Aportes_a_Clientes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Año, Mes,_tercero, formatType);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera el archivo plano para la pagaduria que tiene el centro de pago
        /// </summary>
        /// <param name="channel">Canal de transmision de datos</param>
        /// <param name="pagaduria">Filtro de la pagaduria la cual se desea generar</param>
        /// <returns></returns>
        public DTO_TxResult Report_Cc_ArchivosPlanos(Guid channel, string pagaduria)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera modulo = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesCartera_Cc_ArchivosPlanos(pagaduria);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Aseguradora(Guid channel, DateTime fechaIni, DateTime fechaFin, bool orderName, string filter, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Aseguradora reporte = new Report_Cc_Aseguradora(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, orderName, filter);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_ccCertificadoDeuda Report_Cc_CertificadoDeuda(Guid channel, DateTime fechaCorte, int libranza)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera modu = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Report_Cc_CertificadoDeuda(fechaCorte, libranza);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio q retorna el nombre del reporte
        /// </summary>
        /// <param name="documentReportID">documento del Reporte</param>
        /// <param name="numDoc">identificador del documento</param>
        /// <param name="isAprobada">Es aprobada</param>
        /// <param name="formatType">tipo de reporte</param>
        /// <returns></returns>
        public string Report_Cc_CarteraByNumeroDoc(Guid channel, int documentID, string _nameProposito, int numDoc, DateTime fechaCorte, DateTime? fechaFNC, byte diasFNC, bool isAprobada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                string result = string.Empty;

                if (documentID == AppDocuments.EstadoCuenta)
                {
                    Report_Cc_EstadoCuentaFinan reporte = new Report_Cc_EstadoCuentaFinan(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                    result = reporte.GenerateReport(_nameProposito,numDoc, fechaCorte,fechaFNC,diasFNC, isAprobada);
                }

                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio q retorna el nombre del reporte
        /// </summary>
        /// <param name="documentReportID">documento del Reporte</param>
        /// <param name="numDoc">identificador del documento</param>
        /// <param name="isAprobada">Es aprobada</param>
        /// <param name="formatType">tipo de reporte</param>
        /// <returns></returns>
        public string Report_Dr_DecisorByNumeroDoc(Guid channel, int documentID, int numDoc, int Libranza, ExportFormatType formatType)
        {

            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                string result = string.Empty;

                if (documentID == AppReports.drCondicionesEspecificas)
                {
                    Report_dr_CondicionesEspecificas reporte = new Report_dr_CondicionesEspecificas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                    result = reporte.GenerateReport(numDoc);
                }
                if (documentID == AppReports.drSolicitudCredito)
                {
                    Report_dr_SolicitudCredito reporte = new Report_dr_SolicitudCredito(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                    result = reporte.GenerateReport(Libranza);
                }
                if (documentID == AppReports.drReportePerfil)
                {
                    Report_dr_Perfil reporte = new Report_dr_Perfil(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                    result = reporte.GenerateReport(Libranza);
                }

                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        /// <summary>
        /// Genera el reporte de la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna el nombre del reporte</returns>
        public string Report_Cc_RecaudosMasivosGetRelacionPagos(Guid channel, int documentID, List<DTO_ccIncorporacionDeta> data)
        {           
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                string result = string.Empty;

                Report_Cc_RecaudosMasivos reporte = new Report_Cc_RecaudosMasivos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(documentID, data);
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera el reporte de la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna el nombre del reporte</returns>
        public string Report_Cc_CobroJuridico(Guid channel, int documentID, byte claseDeuda, byte tipoReporte, string cliente, string obligacion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                string result = string.Empty;

                Report_Cc_CobroJuridico reporte = new Report_Cc_CobroJuridico(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(claseDeuda,tipoReporte,cliente,obligacion);
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera el reporte de cobro juridico historico
        /// </summary>
        /// <param name="numDocCredito">num Doc del credito</param>
        /// <returns>Retorna el nombre del reporte</returns>
        public string Report_Cc_CobroJuridicoHistoria(Guid channel, int documentID, int numDocCredito,bool isPrincipal)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                string result = string.Empty;

                Report_Cc_CobroJurHistorico reporte = new Report_Cc_CobroJurHistorico(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(numDocCredito,isPrincipal);
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="claseDeuda">CLase de deuda</param>
        /// <returns></returns>
        public DataTable Report_Cc_CobroJuridicoToExcel(Guid channel, int documentoID, byte tipoReporte, string cliente, string libranza, byte claseDeuda)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCarteraFin module = (ModuloCarteraFin)facade.GetModule(ModulesPrefix.cf, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Report_Cc_CobroJuridicoToExcel(documentoID, tipoReporte, cliente, libranza, claseDeuda);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la informacion de una solicitud de credito para devolición
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        public string Report_cc_DevolucionSolicitud(Guid channel, string _credito, int _numDoc, int _numDev)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_cc_DevolucionSolicitud reporte = new Report_cc_DevolucionSolicitud(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(_credito, _numDoc, _numDev);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFini">Fecha Final de la consulta</param>
        /// <param name="clienteFiltro">ClienteID</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_EstadoDeCuenta(Guid channel, DateTime fechaIni, DateTime fechaFin, string _tercero, string clienteFiltro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_EstadoDeCuenta reporte = new Report_Cc_EstadoDeCuenta(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, _tercero, clienteFiltro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
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
        /// <param name="pagaduria">Pagaduria</param>
        /// <param name="centroPago">CentroPago</param>
        /// <param name="agrup">Agrup</param>
        /// <param name="romp">Romp</param>
        /// <returns></returns>
        public DataTable Report_Cc_CarteraToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza,
                string zona, string ciudad, string concesionario, string asesor, string lineaCred, string compCart, string pagaduria, string centroPago, byte? agrup, byte? romp, object filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera module = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Report_Cc_CarteraToExcel(documentoID, tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart, pagaduria, centroPago, agrup, romp,filter);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene datatable para Excel
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
        /// <param name="pagaduria">Pagaduria</param>
        /// <param name="centroPago">CentroPago</param>
        /// <param name="agrup">Agrup</param>
        /// <param name="romp">Romp</param>
        /// <returns></returns>
        public string Report_Cc_CarteraByParameter(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza,
                string zona, string ciudad, string concesionario, string asesor, string lineaCred, string compCart, string pagaduria, string centroPago, byte? agrup, byte? romp, object filter, int? numeroDoc)
        {
            int opIndex = -1;
            string result = string.Empty;
            try
            {
                opIndex = this.ADO_ConnectDB();
                if (documentoID == AppReports.ccRepAnalisisPagos)
                {
                    if (filter == null)
                    {
                        Report_Cc_AnalisisPagos reporte = new Report_Cc_AnalisisPagos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);
                    }
                    else if (filter != null && (bool)filter)
                    {
                        Report_Cc_AnalisisPagosXCuota reporte = new Report_Cc_AnalisisPagosXCuota(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza);
                    }
                }
                else if (documentoID == AppReports.ccReportesCartera)
                {
                    if (tipoReporte == 1) //Saldos
                    {
                        Report_Cc_SaldosFinan reporte = new Report_Cc_SaldosFinan(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart, agrup, romp);
                    }
                    else //Cuota
                    {
                        Report_Cc_SaldosCuota reporte = new Report_Cc_SaldosCuota(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart, agrup, romp);
                    }
                }
                else if (documentoID == AppReports.ccProyeccionVencim)
                {
                    if (tipoReporte == 1) //Vencimiento
                    {
                        Report_Cc_Vencimiento reporte = new Report_Cc_Vencimiento(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);
                    }
                    else //Proyeccion
                    {
                        Report_Cc_ProyeccionPagos reporte = new Report_Cc_ProyeccionPagos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);
                    }
                }
                else if (documentoID == AppReports.ccEstadoCesionCartera)
                {
                    if (tipoReporte == 1) //Estado Cuenta Cesion 
                    {
                        Report_Cc_EstCuentaCesionResumen reporte = new Report_Cc_EstCuentaCesionResumen(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);
                    }
                    else if (tipoReporte == 2) //Amortizacion Derechos 
                    {
                        Report_Cc_AmortizaDerechoResumen reporte = new Report_Cc_AmortizaDerechoResumen(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);                      
                    }
                    else if (tipoReporte == 3) //Cesion o Recompra Mes
                    {
                        Report_Cc_CesionRecompraMes reporte = new Report_Cc_CesionRecompraMes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);
                    }
                    else if (tipoReporte == 4) //Resumen Rentabilidad 
                    {
                        Report_Cc_ResumenRentab reporte = new Report_Cc_ResumenRentab(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(fechaIni, compCart);
                    }
                }
                else if (documentoID == AppReports.ccPolizaEstado)  // Poliza estado
                {
                    Report_Cc_PolizaEstado reporte = new Report_Cc_PolizaEstado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    result = reporte.GenerateReport(cliente, libranza.ToString(), fechaFin);
                }
                else if (documentoID == AppReports.ccAmortizacion) // Reporte  Amortización
                {
                    Reports_Cc_Amortizacion reporte = new Reports_Cc_Amortizacion(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    result = reporte.GenerateReport(cliente, libranza.ToString());
                }
                else if (documentoID == AppReports.ccSaldoCapital) // Reporte  Saldo Capital
                {
                    Reports_Cc_SaldoCapital reporte = new Reports_Cc_SaldoCapital(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    result = reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zona, ciudad, concesionario, asesor, lineaCred, compCart);
                }
                else if (documentoID == AppReports.ccRecaudosNominaDeta) // Reporte  Recaudos Nomina
                {
                    Report_Cc_RecaudosNominaDeta reporte = new Report_Cc_RecaudosNominaDeta(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    result = reporte.GenerateReport(fechaFin, centroPago, pagaduria);
                }
                else if (documentoID == AppReports.ccSaldosAFavor) // Reporte  Saldos a Favor / Reintegro Ajuste / Giro
                {
                    if (tipoReporte == 1)
                    {
                        //Report_Cc_SaldosAFavor reporte = new Report_Cc_SaldosAFavor(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        //return reporte.GenerateReport(fechaIni, cliente, pagaduria, lineaCred, compCart, asesor, tipoCartera, isSaldoFavor);
                    }
                    else
                    {
                        bool pendienteInd = (bool)filter;
                        Report_Cc_ReintegroAjusteGiro reporte = new Report_Cc_ReintegroAjusteGiro(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(tipoReporte.Value, fechaIni.Value, cliente, pendienteInd);
                    }
                }
                else if (documentoID == AppReports.ccCarteraEspeciales) // Especiales
                {
                    if (tipoReporte == 2) //Prejuridico
                    {
                        Report_cc_Prejuridico reporte = new Report_cc_Prejuridico(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(cliente, fechaIni.Value.Month, fechaIni.Value.Month, fechaIni.Value.Year);
                    }
                    else if (tipoReporte == 3) //Analisis Pagos
                    {
                        Report_cc_AnalisisPagosHistoria reporte = new Report_cc_AnalisisPagosHistoria(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(cliente);
                    }
                    else if (tipoReporte == 5) //Creditos cancelados
                    {
                        Report_Cc_LibranzaCanceladas reporte = new Report_Cc_LibranzaCanceladas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        result = reporte.GenerateReport(fechaIni,fechaFin,cliente);
                    }
                }

                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Reporte Datacrédito
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <returns></returns>
        public DataTable Report_Cc_DataCredito(Guid channel, DateTime periodo, byte tipo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera module = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Report_Cc_DataCredito(periodo,tipo);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Reporte Certificados
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="Lista de campos">data</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_Certificados(Guid channel, int document, Dictionary<int, string> data)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                if (document == AppReports.ccCertificadoDeuda)
                {
                    #region Certificado Deuda
                    Report_Cc_CertificadoDeuda reporte = null;
                    byte[] arr = module.aplReporte_GetByID(AppReports.ccCertificadoDeuda);
                    if (arr != null)
                    {
                        XtraReport customReport = new Report_Cc_CertificadoDeuda();
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        reporte = (Report_Cc_CertificadoDeuda)customReport;
                        reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    }
                    else
                        reporte = new Report_Cc_CertificadoDeuda(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                    return reporte.GenerateReport(document, data);
                    #endregion
                }
                else if (document == AppReports.ccCertificadoPazYSalvo)
                {
                    #region Certificado Paz y Salvo
                    Report_Cc_PazYSalvo reporte = null;
                    byte[] arr = module.aplReporte_GetByID(AppReports.ccCertificadoPazYSalvo);
                    if (arr != null)
                    {
                        XtraReport customReport = new Report_Cc_PazYSalvo();
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        reporte = (Report_Cc_PazYSalvo)customReport;
                        reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    }
                    else
                        reporte = new Report_Cc_PazYSalvo(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                    return reporte.GenerateReport(document, data);
                    #endregion
                }
                else if (document == AppReports.ccCertificadoPagosAlDia)
                {
                    #region Certificado Pagos al Dia
                    Report_Cc_CertificadoPagosAlDia reporte = null;
                    byte[] arr = module.aplReporte_GetByID(AppReports.ccCertificadoPagosAlDia);
                    if (arr != null)
                    {
                        XtraReport customReport = new Report_Cc_CertificadoPagosAlDia();
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        reporte = (Report_Cc_CertificadoPagosAlDia)customReport;
                        reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    }
                    else
                        reporte = new Report_Cc_CertificadoPagosAlDia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                    return reporte.GenerateReport(document, data);
                    #endregion
                }
                else if (document == AppReports.ccCertificadoRelacionPagos)
                {
                    #region Certificado Relacion Pagos
                    Report_Cc_CertificadoRelacionPagos reporte = null;
                    byte[] arr = module.aplReporte_GetByID(AppReports.ccCertificadoRelacionPagos);
                    if (arr != null)
                    {
                        XtraReport customReport = new Report_Cc_CertificadoRelacionPagos();
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        reporte = (Report_Cc_CertificadoRelacionPagos)customReport;
                        reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    }
                    else
                        reporte = new Report_Cc_CertificadoRelacionPagos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                    return reporte.GenerateReport(document, data);
                    #endregion
                }
                else
                    return string.Empty;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Reporte Cartas
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="Lista de campos">data</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_CartaCierreDiario(Guid channel, int document, string tipoReport,List<DTO_ccHistoricoGestionCobranza> dataHistorico)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Cc_CartaCierreDiario reporte = new Report_Cc_CartaCierreDiario(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                return reporte.GenerateReport(document, dataHistorico);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Reporte de GEstion de Cobranza Dia o mes
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="tipoReporte">Tipo de reporte</param>
        /// <param name="fechaCorte">fecha periodo</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_GestionCobranza(Guid channel, int document, string tipoReport, DateTime fechaCorte)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (tipoReport == "1" || tipoReport == "2")
                {
                    Report_Cc_GestionCobranzaDia reporte = new Report_Cc_GestionCobranzaDia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    return reporte.GenerateReport(tipoReport, fechaCorte);
                }
                else if (tipoReport == "3")
                {
                    Report_Cc_DeudoresDemanda reporte = new Report_Cc_DeudoresDemanda(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    return reporte.GenerateReport();
                }
                else
                {
                    return string.Empty;
                }
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Reporte Cartas personalizadas
        /// </summary>
        /// <param document="documentID">DocumentoID</param>
        /// <param data="Datos">data</param>
        /// <returns>nombre de reporte</returns>
        public string Report_Cc_CartaCustom(Guid channel, int document, object data, Dictionary<string, string> adicionales)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Cc_CartaCustom reporte = new Report_Cc_CartaCustom(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(document, data,adicionales);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        #region Incorporaciones

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc">Numero Doc que se usa para buscar documentos</param>
        /// <param name="isLiquidacion">Revisa si es liquidado por credito o por Solicitud</param>
        /// <param name="formatType"></param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Incorporacion(Guid channel, int numeroDoc, bool isLiquidacion, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Cc_Incorporacion reporte = null;
                byte[] arr = module.aplReporte_GetByID(32314);
                if (arr != null)
                {
                    XtraReport customReport = new Report_Cc_Incorporacion();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Cc_Incorporacion)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                }
                else
                    reporte = new Report_Cc_Incorporacion(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);

                return reporte.GenerateReport(numeroDoc, isLiquidacion);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de traer el nombre del reportes
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="formatType">Tipo de Formato de exportacion del reporte</param>
        /// <returns>Nombre del  reporte</returns>
        public string ReportesCartera_Cc_PagaduriaIncorporacion(Guid channel, DateTime FechaInicial, DateTime FechaFinal, string Pagaduria, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_PagaduriaIncoporaciones reporte = new Report_Cc_PagaduriaIncoporaciones(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(FechaInicial, FechaFinal, Pagaduria);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retonar el listado para generar el archivo en excel
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="formatType">Tipo de Formato de exportacion del reporte</param>
        /// <returns>Nombre del  reporte</returns>
        public string ReportesCartera_Cc_PagaduriaIncorporacionPlantilla(Guid channel, DateTime FechaInicial, DateTime FechaFinal, string Pagaduria)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera modu = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                return modu.ReportesCartera_Cc_PagaduriaIncorporacionPlantilla(FechaInicial, FechaFinal, Pagaduria);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Informe SIGCOOP

        /// <summary>
        /// Funcion que se encarga del traer los datos para excel
        /// </summary>
        /// <param name="channel">Canal de trasmion de datos</param>
        /// <param name="Periodo">Periodo a filtrar</param>
        /// <param name="Formato">Tipo de Formato que desea Exportar</param>
        /// <returns>Listado DTO</returns>
        public DataTable ReportesCartera_Cc_InformeSIGCOOP(Guid channel, DateTime Periodo, string Formato)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera modulo = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesCartera_Cc_InformeSIGCOOP(Periodo, Formato);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Liquidacion de Credito

        /// <summary>
        /// Funcion que carga el numero DOC de las Libranzas liquidadas
        /// </summary>
        /// <param name="channel">Canal de trasnmicion</param>
        /// <param name="libranza">Libranza q se liquida</param>
        /// <param name="formatType">Foma de exoportar al reporte</param>
        /// <param name="numDoc">Numero DOC para guardar el reporte</param>
        /// <returns></returns>
        public string ReportesCartera_Cc_LiquidacionCredito(Guid channel, int libranza, ExportFormatType formatType, int numDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Cc_LiquidacionCredito reporte = null;
                byte[] arr = module.aplReporte_GetByID(AppReports.ccLiquidacionCredito);
                if (arr != null)
                {
                    XtraReport customReport = new Report_Cc_LiquidacionCredito();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Cc_LiquidacionCredito)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, Convert.ToInt32(numDoc));
                }
                else
                    reporte = new Report_Cc_LiquidacionCredito(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);

                return reporte.GenerateReport(libranza);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Libranzas

        /// <summary>
        /// Funcion que carga el numero DOC de las Libranzas liquidadas
        /// </summary>
        /// <param name="channel">Canal de trasnmicion</param>
        /// <param name="libranza">Libranza q se liquida</param>
        /// <param name="formatType">Foma de exoportar al reporte</param>
        /// <param name="numDoc">Numero DOC para guardar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesCartera_Cc_Libranzas(Guid channel, DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Libranza reporte = new Report_Cc_Libranza(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, PeriodoFin, Cliente, Libranza, Asesor, Pagaduria);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que genera el reporte de Libranzas en excel
        /// </summary>
        /// <param name="channel">Canal de trasmision</param>
        /// <param name="año">Año por el cual se va a filtrar</param>
        /// <param name="mes">Mes por el cual se va a filtrar</param>
        /// <param name="comprobanteID">Comprobante especifico que se desea ver</param>
        /// <param name="libro">Libro que se esta consultando</param>
        /// <param name="comprobanteInicial">Numero de comprobante inicial que se desea ver</param>
        /// <param name="comprobanteFinal">Numero de comprobante final de que desea ver</param>
        /// <returns></returns>
        public DataTable ReportesCartera_PlantillaExcelLibranza(Guid channel, DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera modu = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                return modu.ReportesCartera_PlantillaExcelLibranzas(Periodo, PeriodoFin, Cliente, Libranza, Asesor, Pagaduria);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que carga una lista con información de los creditos segun filtro
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="zonaID"></param>
        /// <param name="ciudad"></param>
        /// <param name="pagaduria"></param>
        /// <param name="centroPagoID"></param>
        /// <param name="asesor"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <returns>Lsta de Creditos</returns>
        public string Report_Cc_AnalisisPagos(Guid channel, byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_AnalisisPagos reporte = new Report_Cc_AnalisisPagos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que carga una lista con información de los creditos segun filtro
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="zonaID"></param>
        /// <param name="ciudad"></param>
        /// <param name="pagaduria"></param>
        /// <param name="centroPagoID"></param>
        /// <param name="asesor"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <returns>Lsta de Creditos</returns>
        public DataTable Report_Cc_AnalisisPagosExcel(Guid channel, byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera modulo = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Report_Cc_AnalisisPagosExcel(tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Referenciacion

        /// <summary>
        /// Funcion que genera el nombre del reporte
        /// </summary>
        /// <param name="libranza">Numero de la libranza por la cual se desea filtrar</param>
        /// <param name="cliente">Cliente que se desea ver</param>
        /// <param name="FechaRef">Fecha en que se se refencio la libranza</param>
        /// <returns>Nombre del reporte</returns>
        public DTO_TxResult ReportesCartera_Cc_Referenciacion(Guid channel, string libranza, string cliente, DateTime FechaRef,bool _llamadaCtrl, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Referenciacion reporte = new Report_Cc_Referenciacion(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(libranza, cliente, FechaRef, _llamadaCtrl);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Saldos

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cc_Saldos(Guid channel, DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Saldos reporte = new Report_Cc_Saldos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, tipoCartera, isSaldoFavor);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo a consultar</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <param name="libranzaFiltro">Libranza?</param>
        /// <returns>Nombre del Reporte</returns>
        public string Report_Cc_SaldosAFavor(Guid channel, DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_SaldosAFavor reporte = new Report_Cc_SaldosAFavor(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, tipoCartera, isSaldoFavor);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="perido"></param>
        /// <param name="cliente"></param>
        /// <param name="pagaduria"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <param name="asesor"></param>
        /// <param name="plazo"></param>
        /// <returns></returns>
        public string Report_Cc_SaldosMora(Guid channel, DateTime perido, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, int plazo,
             string tipoCartera, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_SaldosMora reporte = new Report_Cc_SaldosMora(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(perido, cliente, pagaduria, lineaCredi, compCartera, asesor, plazo, tipoCartera);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// CAARTERA EN MORA
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="perido"></param>
        /// <param name="cliente"></param>
        /// <param name="pagaduria"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <param name="asesor"></param>
        /// <param name="plazo"></param>
        /// <returns></returns>
        public string Report_Cc_CarteraMora(Guid channel, DateTime periodo, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, string orden, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_CarteraMora reporte = new Report_Cc_CarteraMora(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(periodo, fechaIni, fechaFin, comprador, oferta, libranza, isResumida, orden, formatType);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Cartera Saldos
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="zonaID"></param>
        /// <param name="ciudad"></param>
        /// <param name="concesionario"></param>
        /// <param name="asesor"></param>
        /// <param name="lineaCredi"></param>
        /// <param name="compCartera"></param>
        /// <param name="agrupamiento"></param>
        /// <param name="romp"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public object Report_Cc_SaldosNuevo(Guid channel, byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, byte agrupamiento, byte romp, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                if (tipoReporte == 1 && formatType == ExportFormatType.pdf)
                {
                    Report_Cc_SaldosFinan reporte = new Report_Cc_SaldosFinan(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, agrupamiento, romp);
                }
                else if (tipoReporte == 2 && formatType == ExportFormatType.pdf)
                {
                    Report_Cc_SaldosCuota reporte = new Report_Cc_SaldosCuota(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, agrupamiento, romp);
                }
                else
                {
                    ModuloCartera modulo = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                    return modulo.Report_Cc_SaldosNuevo(tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, agrupamiento, romp, formatType);
                }
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Solicitudes

        /// <summary>
        /// Servio que carga el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de Trasmicion de datos</param>
        /// <param name="fechaIncial">Fecha Con que inicia el repote</param>
        /// <param name="fechaFinal">Fecha Con que termina el reporte</param>
        /// <param name="cliente">Cliente el cual desea ver las solicitudes</param>
        /// <param name="libranza">Libranza q se desea ver</param>
        /// <param name="asesor">Asesor que se desea ver</param>
        /// <param name="formatType">Tipo de exportacion del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesCartera_Cc_Solicitudes(Guid channel, DateTime fechaIncial, DateTime fechaFinal, string cliente, string libranza, string asesor, string estado, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_Solicitudes reporte = new Report_Cc_Solicitudes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIncial, fechaFinal, cliente, libranza, asesor, estado);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Credito
        /// <summary>
        /// Servio que carga el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIncial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        public string Reports_cc_Credito(Guid channel, int mesIni, int mesFin,int año, string libranza, string Credito)
        {
            int opIndex = -1;
            opIndex = this.ADO_ConnectDB();
            string nombreReport = string.Empty;
            try
            {
                // Seleccionar reporte segun el combo
                switch (Credito)
                {
                    case "33":
                        Report_Cc_Pagare reporte1 = new Report_Cc_Pagare(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport = reporte1.GenerateReport(mesIni, mesFin,año, libranza, Credito);
                        break;

                    case "44":
                        Report_Cc_FormularioISS reporte3 = new Report_Cc_FormularioISS(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport = reporte3.GenerateReport(mesIni, mesFin,año, libranza, Credito);
                        break;
                }
                return nombreReport;

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #region Credito Excel
        /// <summary>
        /// Reporte en excel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <param name="año"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        public DataTable Reports_cc_CreditoXLS(Guid channel, string Credito)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera module = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Reports_cc_CreditoXLS(Credito);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        } 
        #endregion
        #endregion

        #region Preventa
        /// <summary>
        /// Reporte Excel para formulario Preventa
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_libranzaTercero"></param>
        /// <param name="_cedulaTercero"></param>
        /// <returns></returns>
        public DataTable ExportExcel_cc_GetVistaCesionesByPreventa(Guid channel, List<int> numeroDocs)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCartera module = (ModuloCartera)facade.GetModule(ModulesPrefix.cc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.ExportExcel_cc_GetVistaCesionesByPreventa(numeroDocs);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        } 
        #endregion

        #region Venta Cartera

        /// <summary>
        /// Servicio q carga el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fechaIni">Mes inicial por el cual se va a filtrar</param>
        /// <param name="fechaFin">Mes final por el cual se va a filtrar</param>
        /// <param name="comprador">Comprador por el cual se desea filtrar</param>
        /// <param name="oferta">Oferta que se desea ver</param>
        /// <param name="libranza">Numero de Libranza por el cual se desea ver</param>
        /// <param name="isResumida">Filtra el reportes (True) para Resumido (False) para Detallado</param>
        /// <param name="formatType">Formato en que se va exportar el reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesCartera_VentaCartera(Guid channel, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_ResumidoCarteraVendida reporte = new Report_Cc_ResumidoCarteraVendida(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, comprador, oferta, libranza, isResumida);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio q carga el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fechaIni">Mes inicial por el cual se va a filtrar</param>
        /// <param name="fechaFin">Mes final por el cual se va a filtrar</param>
        /// <param name="comprador">Comprador por el cual se desea filtrar</param>
        /// <param name="oferta">Oferta que se desea ver</param>
        /// <param name="libranza">Numero de Libranza por el cual se desea ver</param>
        /// <param name="isResumida">Filtra el reportes (True) para Resumido (False) para Detallado</param>
        /// <param name="formatType">Formato en que se va exportar el reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesCartera_VentaCarteraDetallado(Guid channel, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cc_DetalladoCarteraVendida reporte = new Report_Cc_DetalladoCarteraVendida(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, comprador, oferta, libranza, isResumida);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region (CF) Prejuridico
        public string Report_cc_Prejuridico(Guid channel, string _tercero, int _mesIni, int _mesFin, int _año, string _report)
        {
            int opIndex = -1;
            opIndex = this.ADO_ConnectDB();
            string nombreReport = string.Empty;
            try
            {
                // Seleccionar reporte segun el combo
                switch (_report)
                {
                    case "2":
                        Report_cc_Prejuridico reporte1 = new Report_cc_Prejuridico(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport =  reporte1.GenerateReport(_tercero, _mesIni, _mesFin, _año);
                        break;
                }
                return nombreReport;

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #endregion

        #region Contabilidad

        #region Documentos

        #region Causacion Factura

        /// <summary>
        /// Funcion que se encarga de traer el resultado con el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="numeroDoc">Identificador de documentos</param>
        /// <param name="isAprovada">Verifica si es aprobada (True: Trae los Datos de la Tabla coAuxiliar, False: Trae los datos de la tabla coAuxiliarPre) </param>
        /// <param name="moneda">Verifica la moneda que se esta trabajando (True:Local, False: Extranjera) </param>
        /// <param name="formatType">Forma de exportar el Reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_ComprobanteManual(Guid channel, int numeroDoc, bool isAprovada, bool moneda, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ReportComprobateManual reporte = new ReportComprobateManual(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numeroDoc);
                return reporte.GenerateReport(numeroDoc, isAprovada, moneda);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Documento Contable

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="numDoc">Identificador del con q se guardan los registro en la BD</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre </param>
        /// <param name="documento">Verifica el tipo de documento que se va a generar para colocar el nombre del reporte</param>
        /// <param name="formatType">Formato para exportar el reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesContabilidad_DocumentoContable(Guid channel, int numDoc, bool isAprovada, int documento, ExportFormatType formatType)
        {

            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_DocumentoContable reporte = new Report_Co_DocumentoContable(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                return reporte.GenerateReport(numDoc, isAprovada, documento);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Reportes PDF

        #region General
        /// <summary>
        /// Funcion q se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="documentID">documento del reporte</param>
        /// <param name="tipoRep">Tipo de reporte</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">FechaFinal</param>
        /// <param name="libro">Libro balance</param>
        /// <param name="compID">ComprobanteID</param>
        /// <param name="compNro">Comp nro</param>
        /// <param name="cuenta">Cuenta ID</param>
        /// <param name="tercero">Tercero ID</param>
        /// <param name="proyecto">Proyecto ID</param>
        /// <param name="centroCto">Centro Cto ID</param>
        /// <param name="lineaPres">Linea Pres ID</param>
        /// <param name="otroFilter">Otro Filtro</param>
        /// <param name="orden">Ordenamiento</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>nombre del reporte</returns>
        public string ReportesContabilidad_GetByParameter(Guid channel, int documentID, byte? tipoRep, DateTime? fechaIni, DateTime? fechaFin, string libro, string compID, int? compNro,
                      string cuenta, string tercero, string proyecto, string centroCto, string lineaPres, object otroFilter, byte? orden, byte? romp)
        {
            int opIndex = -1;
            string nombreReport = string.Empty;
            try
            {
                opIndex = this.ADO_ConnectDB();
                if (documentID == AppReports.coCertificates)
                {                    
                    Report_Co_CertificadoImpuesto reporte = new Report_Co_CertificadoImpuesto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    nombreReport = reporte.GenerateReport(tipoRep.Value, fechaIni.Value, fechaFin.Value,tercero);
                }  
                else
                {
                    Report_Co_CertificadoIngresoRetencion reporte = new Report_Co_CertificadoIngresoRetencion(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    nombreReport = reporte.GenerateReport(tipoRep.Value, fechaIni.Value, fechaFin.Value,tercero);

                     
                }
                return nombreReport;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        #endregion

        #region Auxiliar

        /// <summary>
        /// Servicio que retorna el nombre del reporte para Auxiliar Moneda Local
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_AuxiliarML(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_AuxiliarML reporte = new Report_Co_AuxiliarML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte para Auxiliar Moneda Extranjera
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaIncial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_AuxiliarME(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaIncial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_AuxiliarME reporte = new Report_Co_AuxiliarME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaInicial, fechaFinal, libro, cuentaIncial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte para Auxiliar Con las dos Monedas
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuenta">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns></returns>
        public string ReportesContabiliad_AuxiliarMultiMoneda(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_AuxiliarMultimoneda reporte = new Report_Co_AuxiliarMultimoneda(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte para Auxiliar X tercero ML
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_AuxiliarxTerceroML(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_AuxiliarxTerceroML reporte = new Report_Co_AuxiliarxTerceroML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport( fechaInicial,  fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte para Auxiliar X tercero ME
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns></returns>
        public string ReportesContabilidad_AuxiliarxTerceroME(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_AuxiliarxTerceroME reporte = new Report_Co_AuxiliarxTerceroME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport( fechaInicial,  fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte para Auxiliar X tercero Ambas Mondefdas
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <param name="formatType">Tipo de formato que se va a exportar el reporte</param>
        /// <returns></returns>
        public string ReportesContabiliad_AuxiliarxTerceroMultiMoneda(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_AuxiliarxTerceroMultimoneda reporte = new Report_Co_AuxiliarxTerceroMultimoneda(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport( fechaInicial,  fechaFinal,libro, cuentaInicial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga el DTO para generar el excel con los auxiliar
        /// </summary>
        /// <param name="año">Año que se desea ver los comprobantes</param>
        /// <param name="mes">Mes Q se desea ver</param>
        /// <param name="comprobanteID">Filtra los comprobantes q se desean ver</param>
        /// <param name="libro">Libro con el cual se va filtrar</param>
        /// <param name="comprobanteInicial">Numero comprobante Inicial (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <param name="comprobanteFinal">Numero comprobante Final (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <returns></returns>
        public string ReportesContabilidad_PlantillaExcelAuxiliar(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modu = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                return modu.ReportesContabilidad_PlantillaExcelAuxiliar(fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto, centroCosto, lineaPresupuestal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Balance

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de filtro</param>
        /// <returns></returns>
        public string ReportesContabilidad_InventariosBalance(Guid channel, int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin,int _año, ExportFormatType formatType)
        {

            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_InventariosBalance reporte = new Report_Co_InventariosBalance(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(mesIni, mesFin, Libro, cuentaIni, cuentaFin,_año);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de filtro</param>
        /// <returns></returns>
        public string ReportesContabilidad_InventariosBalanceSinSaldo(Guid channel, int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin,int _año, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_InventariosBalanceSinSaldo reporte = new Report_Co_InventariosBalanceSinSaldo(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport( mesIni, mesFin, Libro, cuentaIni, cuentaFin, _año);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        public string ReportesContabilidad_PlantillaExcelInventarioBalance(Guid channel, int mes, string Libro, string cuentaIni, string cuentaFin, int _año)
        {

            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modu = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                return modu.ReportesContabilidad_PlantillaExcelInventarioBalance(mes, Libro, cuentaIni, cuentaFin, _año);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// crea el repoprte de balance
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns>nombre del reporte</returns>
        public string ReportesContabilidad_ReportBalancePruebas(Guid channel, int Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int _fechaIni, int _fechaFin, byte? Combo1, byte? Combo2)
        {
            int opIndex = -1;
            try
            {
                string name = string.Empty;
                opIndex = this.ADO_ConnectDB();

                if (tipoReport.Equals("DePrueba"))
                {
                    Report_Co_Balance reporte = new Report_Co_Balance(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    name = reporte.GenerateReport(Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro, tipoReport, Moneda, _fechaIni, _fechaFin, Combo1, Combo2);
                    
                }
                else if (tipoReport.Equals("PorM"))
                {
                    Report_Co_BalancePorMeses reporte = new Report_Co_BalancePorMeses(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    name = reporte.GenerateReport(Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro, tipoReport, Moneda,Combo1, Combo2);

                }
                else if (tipoReport.Equals("PorQ"))
                {
                }
                else if (tipoReport.Equals("Comparativo"))
                {
                } 
                else if (tipoReport.Equals("General"))
                {
                }
                else if(tipoReport.Equals("EstResultados"))
                {
                    Report_Co_BalanceEstadoResultados reporte = new Report_Co_BalanceEstadoResultados(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    name = reporte.GenerateReport(Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro, tipoReport, Moneda, _fechaIni, _fechaFin, Combo1, Combo2);

                }
                return name;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Certificado

        /// <summary>
        /// Funciones que se encarga de Genrar el reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="Impuesto">Impuesto al que se desea generar el Certificado</param>
        /// <param name="formatType">Formato de Exportacion del reporte</param>
        /// <returns>Resultado con La URL del reporte</returns>
        public DTO_TxResult ReportesContabilidad_CertificadoReteFuente(Guid channel, DateTime Periodo, string Impuesto, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_Comprobante reporte = new Report_Co_Comprobante(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                //return reporte.GenerateReport(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, porHoja);
                return null;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Comprobantes

        #region Comprobante

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_Comprobante(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_Comprobante reporte = new Report_Co_Comprobante(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, porHoja);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_ComprobanteME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_ComprobanteME reporte = new Report_Co_ComprobanteME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteInicial, porHoja);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_ComprobanteMLyME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_ComprobanteDetalleMultimoneda reporte = new Report_Co_ComprobanteDetalleMultimoneda(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal, porHoja);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Comprobante Preliminar

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_ComprobantePreliminar(Guid channel, int documentID, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_ComprobantePreliminar reporte = new Report_Co_ComprobantePreliminar(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(documentID, año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_ComprobantePreliminarME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_ComprobantePreliminarME reporte = new Report_Co_ComprobantePreliminarME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_ComprobantePreliminarMLyME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_ComprobantePreliminarMultimoneda reporte = new Report_Co_ComprobantePreliminarMultimoneda(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Comprobante Control

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesContabilidad_ComprobanteControl(Guid channel, int año, int mes, string comprobanteID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_ComprobanteControl reporte = new Report_Co_ComprobanteControl(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, comprobanteID);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #endregion

        #region Libros

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se necesita ver</param>
        /// <param name="mes">Ver que se necesita ver</param>
        /// <param name="tipoBalance">Tipo balance</param>
        /// <returns></returns>
        public string ReportesContabilidad_LibroDiario(Guid channel, int año, int mes, string tipoBalance, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_LibroDiario reporte = new Report_Co_LibroDiario(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, tipoBalance);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se necesita ver</param>
        /// <param name="mes">Ver que se necesita ver</param>
        /// <param name="tipoBalance">Tipo balance</param>
        /// <returns></returns>
        public string ReportesContabilidad_LibroDiarioComprobante(Guid channel, int año, int mes, string tipoBalance, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_LibroDiarioComprobante reporte = new Report_Co_LibroDiarioComprobante(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, tipoBalance);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se necesita ver</param>
        /// <param name="mes">Ver que se necesita ver</param>
        /// <param name="tipoBalance">Tipo balance</param>
        /// <returns></returns>
        public string ReportesContabilidad_LibroMayor(Guid channel, int año, int mes, string tipoBalance,/*, string cuentaIni, string cuentaFin,*/ ExportFormatType formatType)
        {

            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_LibroMayor reporte = new Report_Co_LibroMayor(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mes, tipoBalance/*, cuentaIni, cuentaFin*/);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        public string ReportesContabilidad_PlantillaExcelLibroMayor(Guid channel, int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modu = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                return modu.ReportesContabilidad_PlantillaExcelLibroMayor(año, mes, tipoBalance/*, cuentaIni, cuentaFin*/);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Saldos

        #region Filtro Cuenta

        /// <summary>
        /// Funcio  que retorna el nombre del reporte. (Cuenta-Tercero)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <param name="formatType">Formato con el cual se exporta el documento</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaTercero(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_Saldo reporte = new Report_Co_Saldo(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesInicial, mesFin, libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion  que retorna el nombre del reporte. (Cuenta-CentroCosto)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <param name="formatType">Formato con el cual se exporta el documento</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaCentroCosto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_SaldoCuentaCentroCosto reporte = new Report_Co_SaldoCuentaCentroCosto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesInicial, mesFin, libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion  que retorna el nombre del reporte. (Cuenta-Proyecto)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <param name="formatType">Formato con el cual se exporta el documento</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaProyecto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_SaldoCuentaProyecto reporte = new Report_Co_SaldoCuentaProyecto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesInicial, mesFin, libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion  que retorna el nombre del reporte. (Cuenta-lineaPresupuesto)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <param name="formatType">Formato con el cual se exporta el documento</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosCuentaLineaPresupuesto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_SaldoCuentaLineaPresupuesto reporte = new Report_Co_SaldoCuentaLineaPresupuesto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesInicial, mesFin, libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Filtro Terceros

        /// <summary>
        /// Funcio  que retorna el nombre del reporte. (Tercero-Cuenta)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <param name="formatType">Formato con el cual se exporta el documento</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosTerceroCuenta(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_SaldoTerceroCuenta reporte = new Report_Co_SaldoTerceroCuenta(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesInicial, mesFin, libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcio  que retorna el nombre del reporte. (Tercero-CentroCosto)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <param name="formatType">Formato con el cual se exporta el documento</param>
        /// <returns></returns>
        public string ReportesContabilidad_SaldosTerceroCentroCosto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_SaldoTerceroCentroCosto reporte = new Report_Co_SaldoTerceroCentroCosto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesInicial, mesFin, libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion
        #endregion

        #region Tasas

        /// <summary>
        ///  Funcion que se encarga de traer la informacion para el reporte de Tasas Diarias
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo a Consultar</param>
        /// <param name="isDiaria">Tipo de reporte a imprimir (True: Reportes Tasa Cierre, False: Reprote Tasa Diaria)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public DTO_TxResult ReportesContabilidad_TasasDiarias(Guid channel, DateTime Periodo, bool isDiaria, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_TasasDiarias reporte = new Report_Co_TasasDiarias(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, isDiaria);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Funcion que se encarga de traer la informacion para el reporte de Tasas Cierre
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo a Consultar</param>
        /// <param name="isDiaria">Tipo de reporte a imprimir (True: Reportes Tasa Cierre, False: Reprote Tasa Diaria)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public DTO_TxResult ReportesContabilidad_TasasCierre(Guid channel, DateTime Periodo, bool isDiaria, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_TasasCierre reporte = new Report_Co_TasasCierre(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, isDiaria);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Varios
        /// <summary>
        ///  Permite traer los saldos de un tipo de reporte con lineas y filtros personalizados
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="ReporteID">Periodo Inicial</param>
        /// <param name="PeriodoIni">Periodo Inicial</param>
        /// <param name="PeriodoFin">Periodo Inicial</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public string ReportesContabilidad_ReporteLineaParametrizable(Guid channel, int documentReportID, string reporteID, byte tipoReport, DateTime Periodoini,DateTime PeriodoFin)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                string reportName = string.Empty;
                byte[] arr = module.aplReporte_GetByID(documentReportID);
                if (documentReportID == AppReports.coReporteSituacionFinanciero)
                {
                    Report_Co_ReporteLineasH reporte = null;
                    if (arr != null)
                    {
                        XtraReport customReport = new Report_Co_ReporteLineasH();
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        reporte = (Report_Co_ReporteLineasH)customReport;
                        reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    }
                    else
                        reporte = new Report_Co_ReporteLineasH(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                    reportName = reporte.GenerateReport(documentReportID, reporteID, tipoReport, Periodoini, PeriodoFin);
                }
                else
                {
                    Report_Co_ReporteLineasV reporte = null;
                    if (arr != null)
                    {
                        XtraReport customReport = new Report_Co_ReporteLineasV();
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        reporte = (Report_Co_ReporteLineasV)customReport;
                        reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    }
                    else
                        reporte = new Report_Co_ReporteLineasV(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                    reportName = reporte.GenerateReport(documentReportID, reporteID, tipoReport, Periodoini, PeriodoFin);
                }
                return reportName;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Permite crear el reporte de presupuesto contable
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="periodo">Periodo Inicial</param>
        /// <param name="proyecto">Periodo Inicial</param>
        /// <param name="libro">Periodo Inicial</param>
        /// <param name="monedaID">Periodo Inicial</param>
        /// <returns>Resultado con la URL del reporte</returns>
        public string ReportesContabilidad_EjecucionPresupuestal(Guid channel, DateTime periodo, byte rompimiento, string proyecto, string centroCto, string libro, string monedaID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Co_EjecucionPresupuestal reporte = new Report_Co_EjecucionPresupuestal(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(periodo, proyecto,centroCto, rompimiento, libro, monedaID);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #endregion

        #region Reportes XLS


        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Documento relacionado</param>
        /// <param name="tipoReporte">Tipo reporte</param>
        /// <param name="fechaIni">Fecha ini</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="centroCtoID">Centro Cto</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="lineaPresupID">Linea Presup</param>
        /// <param name="balanceTipo">Balance Tipo</param>
        /// <param name="comprobID">Comprobante ID</param>
        /// <param name="compNro">Comp nro</param>
        /// <param name="otroFilter">otro filtro</param>
        /// <param name="agrup">Agrupar</param>
        /// <param name="romp">romper u ordenar</param>
        /// <returns>datatable</returns>
        public DataTable Reportes_Co_ContabilidadToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string terceroID, string cuentaID, string centroCtoID, string proyectoID, string lineaPresupID, string balanceTipo, string comprobID,
                                                         string compNro, object otroFilter, byte? agrup, byte? romp)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Reportes_Co_ContabilidadToExcel(documentoID, tipoReporte, fechaIni, fechaFin, terceroID,cuentaID,centroCtoID,proyectoID,lineaPresupID,balanceTipo,comprobID,compNro,otroFilter,agrup, romp);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>
        public DataTable ReportesContabilidad_BalancePruebas(Guid channel, DateTime Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
            string CuentaFinal, string libro, string tipoReport, string Moneda)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modulo = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return modulo.ReportesContabilidad_BalancePruebas(Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro, tipoReport, Moneda);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>

        public DataTable ReportesContabilidad_ReporteBalancePruebasXLS(Guid channel, int año, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int MesInicial, int MesFinal, byte? Combo1, byte? Combo2)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modulo = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return modulo.ReportesContabilidad_ReporteBalancePruebasXLS(año, LongitudCuenta, SaldoIncial, CuentaInicial,
                            CuentaFinal, libro, tipoReport, Moneda, MesInicial, MesFinal, Combo1, Combo2);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que se encarga de Traer la informacion para generar la plantilla XLS
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="comprobanteID">Filtra un comprobante especifico</param>
        /// <param name="libro">Libro que se desea consultar</param>
        /// <param name="comprobanteInicial">Numero Inicial de un Comprobante</param>
        /// <param name="comprobanteFinal">Numero Final de un comprobante</param>
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesContabilidad_ComprobanteXLS(Guid channel, DateTime Periodo, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modulo = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesContabilidad_ComprobanteXLS(Periodo, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #endregion

        #region Cuentas X Pagar

        #region Legalizacion:Caja Menor, Legalizacion Gastos, Leg Tarjetas

        /// <summary>
        /// Retorna el nombre del rerporte
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="numeroDoc">nro del doc</param>
        /// <param name="prefijoID">Prefijo Doc</param>
        /// <param name="docNro">Doc consecutivo</param>
        /// <returns>URL del reporte</returns>
        public string Report_Cp_CajaMenor(Guid channel, int? numeroDoc, string prefijoID,int? docNro, bool isPreliminar)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_CajaMenor reporte = new Report_Cp_CajaMenor(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numeroDoc);
                return reporte.GenerateReport(numeroDoc, prefijoID, docNro,isPreliminar);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Edades
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha de corte del reporte</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_PorEdadesDetallado(Guid channel, DateTime fechaIni, string terceroID, string cuentaID, bool isDetallada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_PorEdadesDetallada reporte = new Report_Cp_PorEdadesDetallada(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, terceroID,cuentaID, isDetallada);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha de corte del reporte</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_PorEdadesResumido(Guid channel, DateTime fechaCorte, string terceroID, string cuentaID, bool isDetallada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_PorEdadesResumido reporte = new Report_Cp_PorEdadesResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaCorte, terceroID,cuentaID, isDetallada);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Facturas
        /// <summary>
        /// Funcion que obtiene le nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fecha">Fecha a consultar</param>
        /// <param name="tercero">Filtro por tercero </param>
        /// <param name="formatType">Tipo de formato para exportar</param>
        /// <returns></returns>
        public string Report_FacturasXPagar(Guid channel, DateTime fecha, string Tercero, int Moneda, string Cuenta, bool isMultimoneda, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (isMultimoneda)
                {
                    Report_Cp_FacturasXPagarMulti reporte = new Report_Cp_FacturasXPagarMulti(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(fecha, Tercero, Moneda, Cuenta, isMultimoneda);
                }
                else
                {
                    Report_Cp_FacturasXPagar reporte = new Report_Cp_FacturasXPagar(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(fecha, Tercero, Moneda, Cuenta, isMultimoneda);
                }

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha inicial del reporte</param>
        /// <param name="fechaFin">fecha final del reporte</param>
        /// <param name="tercero">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Reporte_Cp_FacturasPagadas(Guid channel, DateTime fechaIni, DateTime fechaFin, string tercero, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_FacturasPagadas reporte = new Report_Cp_FacturasPagadas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, tercero);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="numDoc">Identificador de los causacion</param>
        /// <param name="isAprovada">Verifica si es para aprobacion o aprobada</param>
        /// <param name="formatType">Tipo de formato de exportacion</param>
        /// <returns>Nombre del reporte</returns>
        public string Reportes_Cp_CausacionFacturas(Guid channel, int numDoc, bool isAprobada,bool isNotaCredito, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_CxP_CausacionFacturas reporte = new Report_CxP_CausacionFacturas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                return reporte.GenerateReport(numDoc, isAprobada,isNotaCredito);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canl de trasmision e datos</param>
        /// <param name="fecha">Fecha de la factura equivalente</param>
        /// <param name="tercero">Tercero a quien se le genera la Factura Equivalente</param>
        /// <param name="facturaEquivalente">Verifica si se desea imprimir la factura Equivalente</param>
        /// <param name="formatType">Tipo de formato de exportacion del reporte</param>
        /// <returns>URL del reporte</returns>
        public string Reportes_Cp_FacturaEquivalente(Guid channel, DateTime fecha, string tercero, bool facturaEquivalente, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_FacturaEquivalente reporte = new Report_Cp_FacturaEquivalente(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fecha, tercero, facturaEquivalente);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que obtiene le nombre del reporte
        /// </summary>
        /// <param name="tipoReport">Tipo de Reporte</param>
        /// <param name="periodoIni">PeriodoIni</param>
        /// <param name="periodoFin">PeriodoFin</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="bancoCuentaID">Banco</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="orden">Orden</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_Cp_CxPvsPagos(Guid channel, byte tipoReport, DateTime periodoIni, DateTime periodoFin, string cuentaID, string bancoCuentaID, string terceroID,byte orden)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (tipoReport == 1) // CxP vs Pagos
                {
                    Report_Cp_CxPvsPagos reporte = new Report_Cp_CxPvsPagos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    return reporte.GenerateReport(periodoIni,periodoFin, cuentaID, bancoCuentaID, terceroID,orden);
                }
                else if (tipoReport == 2) //Pagos vs CxP
                {
                    Report_Cp_PagosVsCxP reporte = new Report_Cp_PagosVsCxP(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    return reporte.GenerateReport(periodoIni,periodoFin, cuentaID, bancoCuentaID, terceroID,orden);
                }
                else
                    return string.Empty;

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region FlujoSemanal
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaCorte">fecha de corte  del reporte</param>
        /// <param name="filtro">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_FlujoSemanalResumido(Guid channel, DateTime fechaCorte, string filtro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_FlujoSemanalResumido reporte = new Report_Cp_FlujoSemanalResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                //return reporte.GenerateReport(fechaCorte, filtro);
                return null;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaCorte">fecha de corte  del reporte</param>
        /// <param name="filtro">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Cp_FlujoSemanalDetallado(Guid channel, DateTime fechaCorte, string filtro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_FlujoSemanalDetallado reporte = new Report_Cp_FlujoSemanalDetallado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                //return reporte.GenerateReport(fechaCorte, filtro);
                return null;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaCorte">fecha de corte  del reporte</param>
        /// <param name="filtro">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesCuentasXPagar_FlujoSemanalDetallado(Guid channel, List<DateTime> fechaIni, int Moneda, string Tercero, bool isDetallado, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                if (isDetallado)
                {
                    Report_Cp_FlujoSemanalDetallado reporte = new Report_Cp_FlujoSemanalDetallado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(fechaIni, Moneda, Tercero, isDetallado);
                }
                else
                {
                    Report_Cp_FlujoSemanalResumido reporte = new Report_Cp_FlujoSemanalResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(fechaIni, Moneda, Tercero, isDetallado);
                }
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Libro de Compras

        /// <summary>
        /// Retorna el nombre del rerporte del libro de compras
        /// </summary>
        /// <param name="channel">Canal de trasmicion de datos</param>
        /// <param name="fecha">Fecha q se desea ver las compras</param>
        /// <param name="tercero">Tercero especifico q se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el Reporte</param>
        /// <returns>URL del reporte</returns>
        public string Reportes_Cp_LibroCompras(Guid channel, DateTime fecha, string tercero, bool facturaEquivalente, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_LibroCompras reporte = new Report_Cp_LibroCompras(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fecha, tercero, facturaEquivalente);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Radicaciones
        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        public string Reporte_Cp_Radicaiones(Guid channel, int yearIni, int yearFin, DateTime fechaIni, DateTime fechaFin, string Tercero, string Estado, string Orden, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Cp_Radicaciones reporte = new Report_Cp_Radicaciones(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(yearIni, yearFin, fechaIni, fechaFin, Tercero, Estado, Orden);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Tarjetas
        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        public string Report_CxP_TarjetasPagas(Guid channel, int numDoc, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_CxP_TarjetasPagas reporte = new Report_CxP_TarjetasPagas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, numDoc, formatType);
                return reporte.GenerateReport(numDoc);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        public string ReportesCuentasXPagar_LegalizaTarjetas(Guid channel, int numDoc, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ReportesCuentasXPagar_LegalizaTarjetas reporte = new ReportesCuentasXPagar_LegalizaTarjetas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(numDoc);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Anticipos

        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        public string ReportesCuentasXPagar_Anticipos(Guid channel, DateTime Fecha, int Moneda, string Tercero, bool isDetallado, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                if (isDetallado)
                {
                    Report_Cp_Anticipos reporte = new Report_Cp_Anticipos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(Fecha, Moneda, Tercero, isDetallado);
                }
                else
                {
                    Report_Cp_AnticiposResumen reporte = new Report_Cp_AnticiposResumen(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(Fecha, Moneda, Tercero, isDetallado);
                }
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        public string ReportesCuentasXPagar_DocumentoAnticipo(Guid channel, int numDoc, bool isAprobada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_CxP_Anticipos reporte = new Report_CxP_Anticipos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                return reporte.GenerateReport(numDoc, isAprobada);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        public string ReportesCuentasXPagar_DocumentoAnticipoViaje(Guid channel, int numDoc, bool isAprobada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_CxP_AnticiposViajes reporte = new Report_CxP_AnticiposViajes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, numDoc);
                return reporte.GenerateReport(numDoc, isAprobada);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region EXCEL
        /// <summary>
        /// Obtiene un datatable con la info de CxP segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_Cp_CxPToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string facturaNro,
                                                string cuentaID, string bancoCuentaID, string moneda, object otroFilter, byte? agrup, byte? romp)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar module = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Reportes_Cp_CxPToExcel(documentoID, tipoReporte, fechaIni, fechaFin, tercero, facturaNro,cuentaID, bancoCuentaID,moneda,otroFilter, agrup, romp);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        #endregion

        #endregion

        #region Facturacion

        /// <summary>
        /// Servicio q retorna el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="numDoc">Numero Documento con los que se van relacionados los datos a mostrar</param>
        /// <param name="formatType">Tipo Formato de exportación del reporte</param>
        /// <returns>URL del Reporte</returns>
        public string ReportesFacturacion_FacturaVenta(Guid channel, int documentID, string numDoc, bool isAprobada, ExportFormatType formatType, decimal valorAnticipo, decimal valorRteGarantia, decimal? porcRteGarantia)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Fa_FacturaVenta reporte = null;
                byte[] arr = module.aplReporte_GetByID(documentID);
                if (arr != null)
                {
                    XtraReport customReport = new Report_Fa_FacturaVenta();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Fa_FacturaVenta)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, Convert.ToInt32(numDoc));
                }
                else
                    reporte = new Report_Fa_FacturaVenta(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType, Convert.ToInt32(numDoc));

                return reporte.GenerateReport(numDoc, isAprobada,valorAnticipo, valorRteGarantia, porcRteGarantia);
            }
            catch
            {
                throw;
            }

            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el nombre del reporte de facturas masivo
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="prefijo">Prefijo</param>
        /// <param name="docNroIni">nro Inicial</param>
        /// <param name="docNroIni">nro Inicial</param>
        /// <returns>URL del Reporte</returns>
        public string ReportesFacturacion_FacturaVentaMasivo(Guid channel, string prefijo, int docNroIni, int docnroFin)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Fa_FacturaVenta reporte = null;
                byte[] arr = module.aplReporte_GetByID(AppDocuments.FacturaVenta);
                if (arr != null)
                {
                    XtraReport customReport = new Report_Fa_FacturaVenta();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Fa_FacturaVenta)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                }
                else
                    reporte = new Report_Fa_FacturaVenta(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                return reporte.GenerateReport(prefijo, true,0,0,0);
            }
            catch
            {
                throw;
            }

            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte de cuentas por cobrar detallasdas
        /// </summary>
        /// <param name="channel">Canal de Trasmicion de datos</param>
        /// <param name="fechaCorte">Fecha en que se esta haciendo la consulta</param>
        /// <param name="tercero">Si se quiere filtrar por un tercero en especifico</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesFacturacion_CxCPorEdadesDetalladas(Guid channel, DateTime fechaCorte, string tercero, bool isDetallada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_CxC_PorEdadesDetallada reporte = new Report_CxC_PorEdadesDetallada(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaCorte, tercero, isDetallada);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };

        }

        /// <summary>
        /// Servicio que retorna el nombre del reporte de cuentas por cobrar Resumida
        /// </summary>
        /// <param name="channel">Canal de Trasmicion de datos</param>
        /// <param name="fechaCorte">Fecha en que se esta haciendo la consulta</param>
        /// <param name="tercero">Si se quiere filtrar por un tercero en especifico</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns>URL del reporte</returns>
        public string ReportesFacturacion_CxCPorEdadesResumida(Guid channel, DateTime fechaCorte, string tercero, bool isDetallada, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_CxC_PorEdadesResumido reporte = new Report_CxC_PorEdadesResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaCorte, tercero, isDetallada);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };

        }

        public string ReportesFacturacion_LibroVentas(Guid channel, DateTime periodo, int diaFinal, string cliente, string prefijo, string NroFactura, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Fa_LibroVentas reporte = new Report_Fa_LibroVentas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(periodo, diaFinal, cliente, prefijo, NroFactura);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que obtiene el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fecha">Fecha a consultar</param>
        /// <param name="Tercero">Filtro por tercero </param>
        /// <param name="Moneda">Filtro de la Moneda de origen </param>
        /// <param name="Cuenta">Filtro de la cuenta</param>
        /// <param name="isMultimoneda">Indica si el reporte es para empresa Multimoneda</param>
        /// <param name="formatType">Tipo de formato para exportar</param>
        /// <returns></returns>
        public string Report_CuentasXCobrar(Guid channel, DateTime fecha, string Tercero, int Moneda, string Cuenta, bool isMultimoneda, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (isMultimoneda)
                {
                    Report_Cp_CuentasXCobrarMulti reporte = new Report_Cp_CuentasXCobrarMulti(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(fecha, Tercero, Moneda, Cuenta);
                }
                else
                {
                    Report_Cp_CuentasXCobrar reporte = new Report_Cp_CuentasXCobrar(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                    return reporte.GenerateReport(fecha, Tercero, Moneda, Cuenta);
                }

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Global

        /// <summary>
        /// Funcion que se encarga de traer el Nombre del reporte de Documentos Pendientes
        /// </summary>
        /// <param name="channel">Canale de trasmiscion de datos</param>
        /// <param name="Periodo">Periodo a consultar los documentos Pendientes</param>
        /// <param name="modulo">Filtrar un modulo especifico</param>
        /// <param name="formatType">Tipo de formato a exportar el Reporte</param>
        /// <returns>URl del reportes</returns>
        public DTO_TxResult ReportesGlobal_DocumentosPendiente(Guid channel, DateTime Periodo,byte tipoReport, string modulo,string documentoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Gl_DocumentosPendientes reporte = new Report_Gl_DocumentosPendientes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo,tipoReport, modulo,documentoID);
            }

            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        #endregion

        #region Inventarios

        #region Documentos
        /// <summary>
        /// Funcion que retorna el nombre del reporte de documentos
        /// </summary>
        /// <param name="mvto">mvto existente</param>
        /// <param name="documentID">Documento</param>
        /// <param name="numDoc">numero doc para consulta</param>
        /// <returns>Nombre del Reporte</returns>
        public string Reports_In_TransaccionMvto(Guid channel, DTO_MvtoInventarios mvto, int documentID, int numDoc, byte tipoMvto)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_MvtoInventario reporte = new Report_In_MvtoInventario(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numDoc);
                return reporte.GenerateReport(mvto, documentID, numDoc, tipoMvto);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Saldos

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Saldos
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_Saldos(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_Saldos reporte = new Report_In_Saldos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Saldos sin parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SaldosSinParametros(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SaldosSinParametros reporte = new Report_In_SaldosSinParametros(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Saldos x referencia
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SaldosxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SaldosxReferencia reporte = new Report_In_SaldosxReferencia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Saldos x referencia
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SaldosSinParametrosxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SaldosSinParametrosxReferencia reporte = new Report_In_SaldosSinParametrosxReferencia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Kardex

        /// <summary>
        /// Funcion que retorna el nombre del reporte Kardex
        /// </summary>
        /// <param name="año">Año que se va a verificar</param>
        /// <param name="mesIni">Fecha inicial del reporte</param>
        /// <param name="bodega">Bodega que se quiere revisar</param>
        /// <param name="referencia">Tipo de referencia que se quiere ver</param>
        /// <param name="formatType">Tipo de formato de exportacion del reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_KardexDetallado(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_KardexDetallado reporte = new Report_In_KardexDetallado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Kardex sin parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_KardexSinParametros(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_KardexSinParametros reporte = new Report_In_KardexSinParametros(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Kardex x referencia
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_KardexxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_KardexxReferencia reporte = new Report_In_KardexxReferencia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte de Kardex x referencia
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>

        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_KardexSinParametrosxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_KardexSinParametrosxReferencia reporte = new Report_In_KardexSinParametrosxReferencia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }
        #endregion

        #region Serial

        /// <summary>
        /// Funcion que retorna el Nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <param name="formatType">Tipo de Formato en que se va a imprimir el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SerialxBodega(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SerialSinCostosxBodega reporte = new Report_In_SerialSinCostosxBodega(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el Nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <param name="formatType">Tipo de Formato en que se va a imprimir el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SerialxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SerialSinCostosxReferencia reporte = new Report_In_SerialSinCostosxReferencia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el Nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <param name="formatType">Tipo de Formato en que se va a imprimir el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SerialxBodegaCosto(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SerialConCostoxBodega reporte = new Report_In_SerialConCostoxBodega(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el Nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <param name="formatType">Tipo de Formato en que se va a imprimir el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        public string ReportesInventarios_SerialxReferenciaCosto(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                   string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_In_SerialConCostoxReferencia reporte = new Report_In_SerialConCostoxReferencia(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }



        #endregion

        #region EXCEL

        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="mesFin">Fecha Final</param>
        /// <param name="bodega">bodega</param>
        /// <param name="tipoBodega">tipoBodega</param>
        /// <param name="grupo">Grupo</param>
        /// <param name="clase">tipoBodega</param>
        /// <param name="Tipo">Tipo</param>
        /// <param name="serie">serie</param>
        /// <param name="material">material</param>
        /// <param name="isSerial">isSerial</param>
        /// <param name="otroFilter">otroFilter</param>
        /// <param name="agrup">agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_In_InventarioToExcel(Guid channel, int documentID, DateTime? mesIni, DateTime? mesFin, string bodega, string tipoBodega, string referencia, string grupo, string clase, string tipo, 
                                                        string serie, string material, bool isSerial, string libro, object otroFilter, byte? agrup, byte? romp)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios module = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Reportes_In_InventarioToExcel(documentID,mesIni,mesFin,bodega,tipoBodega,referencia,grupo,clase,tipo, serie, material, isSerial,libro, otroFilter, agrup, romp);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Nomina

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">Documento por el cual realizarara la consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Forma de mostrar la info</param>
        /// <param name="fechaini">Fecha inicial de generacion del reporte</param>
        /// <param name="fechaFin">Fecha Final de generacion del reporte</param>
        /// <param name="isApro">Es para aprobar o esta aprobado</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_DetailLiquidaciones(Guid channel, int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_DetailLiquidaciones reporte = new Report_No_DetailLiquidaciones(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(documentoID, periodo, orden, fechaini, fechaFin, isAll, isOrderByName, isPre,terceroid,operacionnoid,areafuncionalid,conceptonoid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que arma el Dto_NominaDetail por Concepto
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        public string Report_No_XConcepto(Guid channel, int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, string orden, bool isAll, bool orderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_XConcepto reporte = new Report_No_XConcepto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(documentoID, periodo, orden, fechaIni, fechaFin, isAll, orderByName, isPre, terceroid, operacionnoid, areafuncionalid, conceptonoid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">Documento por el cual realizarara la consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Forma de mostrar la info</param>
        /// <param name="fechaini">Fecha inicial de generacion del reporte</param>
        /// <param name="fechaFin">Fecha Final de generacion del reporte</param>
        /// <param name="isApro">Es para aprobar o esta aprobado</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_Detalle(Guid channel, int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB(); 
                Report_No_Detalle reporte = new Report_No_Detalle(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(documentoID, periodo, orden, fechaini, fechaFin, isAll, isOrderByName, isPre,terceroid,operacionnoid,areafuncionalid,conceptonoid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">Documento por el cual realizarara la consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Forma de mostrar la info</param>
        /// <param name="fechaini">Fecha inicial de generacion del reporte</param>
        /// <param name="fechaFin">Fecha Final de generacion del reporte</param>
        /// <param name="isApro">Es para aprobar o esta aprobado</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_TotalXConcepto(Guid channel, int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool orderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_TotalXConcepto reporte = new Report_No_TotalXConcepto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(documentoID, periodo, orden, fechaini, fechaFin, isAll, orderByName, isPre,terceroid,operacionnoid,areafuncionalid,conceptonoid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">Documento por el cual realizarara la consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="orden">Forma de mostrar la info</param>
        /// <param name="fechaini">Fecha inicial de generacion del reporte</param>
        /// <param name="fechaFin">Fecha Final de generacion del reporte</param>
        /// <param name="isApro">Es para aprobar o esta aprobado</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_VacacionesPagadas(Guid channel, DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, ExportFormatType formatType, String empleadoid)
        {
            int opIndex = -1; 
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_VacacionesPagadas reporte = new Report_No_VacacionesPagadas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, empleadoFil, orderBy, empleadoid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna valores del reportes_Vaciones Pendientes
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_documentoID">Documento de la nomina</param>
        /// <param name="_otroPorAlgo"></param>
        /// <returns>Funcion que retorna valores del reportes_Vaciones Pendientes</returns>
        public string Report_No_VacacionesPendientes(Guid channel, string _empleadoID, int _vacaciones, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_VacacionesPendientes reporte = new Report_No_VacacionesPendientes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(_empleadoID, _vacaciones);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Docuemoten de Vacaciones
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_empleadoID"></param>
        /// <param name="_docID"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_No_VacacionesDocumento(Guid channel, string _empleadoID, int _docID,string fechaFiltro)
        {
            int opIndex = -1;
            opIndex = this.ADO_ConnectDB();
            string reportName = string.Empty;
            try
            {
                switch(_docID)
                {
                    case AppDocuments.Vacaciones:
                        Report_No_VacacionesDocumento reporteVac = new Report_No_VacacionesDocumento(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        reportName = reporteVac.GenerateReport(_empleadoID, _docID, fechaFiltro);
                    break;
                    case AppDocuments.LiquidacionContrato:
                        Reports_no_LiquidacionContrato reporteLiq = new Reports_no_LiquidacionContrato(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        reportName = reporteLiq.GenerateReport(_empleadoID, _docID, fechaFiltro);
                    break;
                }
                return reportName;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_Prestamo(Guid channel, DateTime fechaIni, DateTime fechaFin, bool orderByName, ExportFormatType formatType,String empleadoid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_Prestamo reporte = new Report_No_Prestamo(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, orderByName,empleadoid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_AportesPension(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            int opIndex = -1;
            try 
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_AportesPension reporte = new Report_No_AportesPension(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, filtro, orderByName,terceroid,nofondosaludid,nocajaid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex); 
            }
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_AportesSalud(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_AportesSalud reporte = new Report_No_AportesSalud(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, filtro, orderByName,terceroid,nofondosaludid,nocajaid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_AporteVoluntarioPension(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_AporteVoluntarioPension reporte = new Report_No_AporteVoluntarioPension(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, filtro, orderByName,terceroid,nofondosaludid,nocajaid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex); 
            };
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_AporteARP(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_AportesARP reporte = new Report_No_AportesARP(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, filtro, orderByName,terceroid,nofondosaludid,nocajaid);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex); 
            };
        }

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="_terceroID"></param>
        /// <returns></returns>
        public string Report_No_AportesCajaCompensacion(Guid channel, DateTime fechaIni, DateTime fechaFin, String _terceroID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_AportesCajaCompensacion reporte = new Report_No_AportesCajaCompensacion(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, _terceroID);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Servicio para Gastos de Empresa (Reporte)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="_terceroID"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public string Report_No_GastosEmpresa(Guid channel, DateTime fechaIni, DateTime fechaFin, String _terceroID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_GastosEmpresa reporte = new Report_No_GastosEmpresa(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(fechaIni, fechaFin, _terceroID);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo de Pago</param>
        /// <param name="empleadoId">Identificador del Empleado</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_No_BoletaPago(Guid channel, string empleadoID, int _mes, int _año, string _documentoNomina, string _quincena, int? numDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_No_BoletaPago reporte = new Report_No_BoletaPago(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf,numDoc);
                return reporte.GenerateReport(empleadoID, _mes, _año, _documentoNomina, _quincena);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Obtiene el nombre del reporte con la info de nomina segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="operacionNoID">Operacion Nomina</param>
        /// <param name="conceptoNoID">Concepto Nomina</param>
        /// <param name="areaFuncID">Area Funcional</param>
        /// <param name="fondoID">Fondo Nom</param>
        /// <param name="cajaID">Caja Nomina</param>
        /// <param name="otroFilter">Filtro adicional</param>
        /// <param name="agrup">Agrupar u ordenar</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public string Report_No_NominaGetByParameter(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                         string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp)
        {
            int opIndex = -1;
            string nombreReport = string.Empty;
            try
            {
                opIndex = this.ADO_ConnectDB();
                if (documentoID == AppReports.noCesantias)
                {
                    //if (tipoReporte == 1)
                    //{
                    //    Report_No_CesantiasDetalle reporte = new Report_No_CesantiasDetalle(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    //    nombreReport = reporte.GenerateReport(empleadoID, (int)otroFilter, string.Empty);
                    //}
                    if (tipoReporte == 2)
                    {
                        Report_No_CesantiasDetalle reporte = new Report_No_CesantiasDetalle(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport = reporte.GenerateReport(empleadoID, (int)otroFilter, fondoID);
                    }
                    else if(tipoReporte == 3)
                    {
                        Report_No_CesantiasDocumento reporte = new Report_No_CesantiasDocumento(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport = reporte.GenerateReport(empleadoID, (int)otroFilter, string.Empty);
                    }
                }
                else if (documentoID == AppReports.noProvisiones)
                {
                    if (tipoReporte == 1)
                    {
                        Report_No_ProvisionesSaldos reporte = new Report_No_ProvisionesSaldos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport = reporte.GenerateReport(fechaIni.Value, empleadoID); 
                    }
                    else if (tipoReporte == 2)
                    {
                        Report_No_ProvisionesDetalle reporte = new Report_No_ProvisionesDetalle(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        nombreReport = reporte.GenerateReport(fechaIni.Value, empleadoID);
                    }
                }
                return nombreReport;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region General

        /// <summary>
        /// Funcion que se encarga de los servicios
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="txResult"></param>
        /// <returns></returns>
        public string Rep_TxResult(Guid channel, DTO_TxResult txResult)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_TxResult reporte = new Report_TxResult(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2);
                return reporte.GenerateReport(txResult);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        public string Rep_TxResultDetails(Guid channel, List<DTO_TxResult> txResult)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_TxResultDetails reporte = new Report_TxResultDetails(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2);
                return reporte.GenerateReport(txResult);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Operaciones Conjuntas

        /// <summary>
        /// Funcion que se encarga de traer los datos de cierre
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="Periodo">Periodo que se va a consultar</param>
        /// <returns>Tabla con datos</returns>
        public DataTable ReportesOperacionesConjuntas_Legalizaciones(Guid channel, DateTime Periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloOpConjuntas modulo = (ModuloOpConjuntas)facade.GetModule(ModulesPrefix.oc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesOperacionesConjuntas_Legalizaciones(Periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Planeacion

        #region Cierre Legalizacion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="Periodo">Periodo que se desea verificar</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>        
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesPlaneacion_CierreLegalizacion(Guid channel, DateTime Periodo, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion modulo = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesPlaneacion_CierreLegalizacion(Periodo, contrato, bloque, campo, pozo, proyecto, actividad, lineaPresupuesto, centroCosto, recurso);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        #endregion

        #region Presupuestos

        //Reporte Acumulado
        /// <summary>
        /// Funcion que se encarga de generar el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="periodo">Perido para la consulta</param>
        /// <param name="proyecto">Proyecto q se desea ver</param>
        /// <param name="isAcumulado">Verifica si es acumulado (True: Ejecula Procedimiento, False: Ejecuta Consulta)</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL</returns>
        public DataTable ReportesPlaneacion_PresupuestoAcumulado(Guid channel, DateTime periodo, string proyecto, bool isAcumulado, bool tipoMoneda, bool isConsololidado)
        {
            int opIndex = -1;
            try
            {
                /*opIndex = this.ADO_ConnectDB();
                Report_Pl_PresupuestoAcumulado reporte = new Report_Pl_PresupuestoAcumulado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(periodo, proyecto, isAcumulado);*/

                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion modulo = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesPlaneacion_Presupuesto(periodo, proyecto, isAcumulado, tipoMoneda, isConsololidado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }
        #endregion

        #region Ejecucion Presupuesto

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Proyecto por Actividad Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalProyectoxActividadML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalProyectXActiviML reporte = new Report_Pl_EjePresupuestalProyectXActiviML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Proyecto por Actividad Moneda Extranjera
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalProyectoxActividadME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalProyectXActiviME reporte = new Report_Pl_EjePresupuestalProyectXActiviME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Centro de Costo Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCtoML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalLineaXCentroCtoML reporte = new Report_Pl_EjePresupuestalLineaXCentroCtoML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Centro de Costo Moneda Extranjera
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCtoME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalLineaXCentroCtoME reporte = new Report_Pl_EjePresupuestalLineaXCentroCtoME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Recursos Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXRecursoML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalLineaXRecursoML reporte = new Report_Pl_EjePresupuestalLineaXRecursoML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Recurso Moneda Extrajera
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXRecursoME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalLineaXRecursoME reporte = new Report_Pl_EjePresupuestalLineaXRecursoME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Recuros por Actividad Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalRecursoXActividadML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
           string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalRecursoXActiviML reporte = new Report_Pl_EjePresupuestalRecursoXActiviML(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Recurso por Activida Moneda Extranjera
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalRecursoXActividadME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                Report_Pl_EjePresupuestalRecursoXActiviME reporte = new Report_Pl_EjePresupuestalRecursoXActiviME(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }

        /// <summary>
        /// Funcion que se encarga de traer los datos para generar el reportes de Ejecucion Presupuestal por Origen
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        public DataTable ReportesPlaneacion_EjecucionPresupuestalXOrigen(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion modulo = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesPlaneacion_EjecucionPresupuestalXOrigen(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID, LineaPresupuestalID,
                    CentroCostoID, RecursoGrupoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region SobreEjecucion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>
        /// <param name="usuario">Filtra un usuario</param>
        /// <param name="prefijo">Filtra un prefijo </param>
        /// <param name="numeroDoc">Filtra un numero de Documento Especifico</param>
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesPlaneacion_SobreEjecucion(Guid channel, int year, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso, string usuario, string prefijo, string numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion modulo = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesPlaneacion_SobreEjecucion(year, contrato, bloque, campo, pozo, proyecto, actividad, lineaPresupuesto, centroCosto, recurso, usuario,
                    prefijo, numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            };
        }
        #endregion

        #endregion

        #region Proveedores

        #region Compromisos VS Facturas

        /// <summary>
        /// Funcion que se encarga de  traer los compromisos contra las facturas
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaInicial">Fecha de consulta inicial</param>
        /// <param name="FechaFinal">Fecha de consulta final</param>
        /// <param name="proveedor">Filtra un proveedor en especifico</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns>Listado de DTO</returns>
        public DTO_TxResult ReportesProveedores_CompromisosVSFacturas(Guid channel, DateTime FechaInicial, DateTime FechaFinal, string proveedor, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Pr_ComprVSFact reporte = new Report_Pr_ComprVSFact(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(FechaInicial, FechaFinal, proveedor);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Orden de compra

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaIni">Fecha incial que se desea ver los datos</param>
        /// <param name="FechaFin">Fecha Final hasta donde quiere verificar los datos</param>
        /// <param name="Proveedor">Filtra un proveedor en especifico</param>
        /// <param name="Estado">Filtra el estado de la orden de compra</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>Listado de DTO</returns>
        public DTO_TxResult ReportesProveedores_OrdenCompras(Guid channel, DateTime FechaIni, DateTime FechaFin, string Proveedor, int Estado, bool isDetallado, string Moneda, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {

                opIndex = this.ADO_ConnectDB();
                Report_Pr_OrdenCompraResumida   reporte = new Report_Pr_OrdenCompraResumida(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);            
                return reporte.GenerateReport(FechaIni, FechaFin, Proveedor, Estado, isDetallado, Moneda);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaIni">Fecha incial que se desea ver los datos</param>
        /// <param name="FechaFin">Fecha Final hasta donde quiere verificar los datos</param>
        /// <param name="Proveedor">Filtra un proveedor en especifico</param>
        /// <param name="Estado">Filtra el estado de la orden de compra</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="Moneda">Tipo de moneda con que seq quiere ver el reporte</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>Listado de DTO</returns>
        public DTO_TxResult ReportesProveedores_OrdenComprasDetallada(Guid channel, DateTime FechaIni, DateTime FechaFin, string Proveedor, int Estado, bool isDetallado, string Moneda, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Pr_OrdenCompraDetallada reporte = new Report_Pr_OrdenCompraDetallada(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(FechaIni, FechaFin, Proveedor, Estado, isDetallado, Moneda);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Solicitudes
        public string ReportesProveedores_Solicitudes(Guid channel, Dictionary<int, string> filtros, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Pr_EstadoSolicitudes reporte = new Report_Pr_EstadoSolicitudes(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, formatType);
                return reporte.GenerateReport(filtros);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera el reporte de un documento de Solicitud o Recibido
        /// </summary>
        /// <param name="documentoID">Id del Reporte</param>
        /// <param name="numeroDoc">numero del Doc</param>
        /// <param name="isPreliminar">si es para aprobacion</param>
        /// <param name="tipoReporte">Tipo de reporte</param>
        /// <returns></returns>
        public string ReportesProveedores_SolicitudOrRecibidoDoc(Guid channel, int documentoID, int numeroDoc, bool isPreliminar, byte tipoReporte)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Pr_SolicitudRecibido reporte = null;

                int documentIDReport = documentoID == AppDocuments.Solicitud ? AppReports.prSolicitudDoc : AppReports.prRecibidoDoc;
                byte[] arr = module.aplReporte_GetByID(documentIDReport);
                if (arr != null)
                {
                    XtraReport customReport = new Report_Pr_SolicitudRecibido();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Pr_SolicitudRecibido)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numeroDoc);
                }
                else
                    reporte = new Report_Pr_SolicitudRecibido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numeroDoc);

                return reporte.GenerateReport(documentoID, numeroDoc, isPreliminar, tipoReporte);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }        
        #endregion

        #region OrdenCompra
        /// <summary>
        /// Funcion q se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de Trasmision de datos</param>
        /// <param name="numDoc">Identificador de las facturas a pagar</param>
        /// <param name="exportType">Tipo de Exportacion del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesProveedores_OrdenCompra(Guid channel, int numDoc,byte tipoReporte,bool showReport, bool isPreliminar = false)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Pr_OrdenCompra reporte = null;

                int documentID = tipoReporte == 1 ? AppDocuments.OrdenCompra : AppReports.prOrdenCompraAnexo;
                byte[] arr = module.aplReporte_GetByID(documentID);
                if (arr != null)
                {
                    XtraReport customReport = new Report_Pr_OrdenCompra();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Pr_OrdenCompra)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numDoc);
                }
                else
                    reporte = new Report_Pr_OrdenCompra(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numDoc);

                return reporte.GenerateReport(numDoc,isPreliminar, tipoReporte,showReport);

                //Report_Pr_OrdenCompraQuantum reporte = new Report_Pr_OrdenCompraQuantum(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType, numDoc);
                //return reporte.GenerateReport(numDoc, isPreliminar);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Recibidos

        /// <summary>
        /// Funcion que se encarga de traer las ordenes recibidas resumido
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="exportType">Formato de Ezportacion del reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesProveedores_Recibidos(Guid channel, DateTime Periodo, string proveedor, bool isDetallado, bool isFacturdo, ExportFormatType exportType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Pr_RecibidoResumido reporte = new Report_Pr_RecibidoResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType);
                return reporte.GenerateReport(Periodo, proveedor, isDetallado, isFacturdo);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer las ordenes recibidas Detallado
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="exportType">Formato de Ezportacion del reporte</param>
        /// <returns></returns>
        public DTO_TxResult ReportesProveedores_RecibidosDetallado(Guid channel, DateTime Periodo, string proveedor, bool isDetallado, bool isFacturdo, ExportFormatType exportType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Pr_RecibidoDetallado reporte = new Report_Pr_RecibidoDetallado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType);
                return reporte.GenerateReport(Periodo, proveedor, isDetallado, isFacturdo);

            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        public DTO_TxResult ReportesProveedores_RecibidosNoFacturados(Guid channel, DateTime Periodo, string proveedor, bool isDetallado, bool isFacturdo, ExportFormatType exportType)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Report_Pr_RecibidoDetallado reporte = new Report_Pr_RecibidoDetallado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType);
                return reporte.GenerateReport(Periodo, proveedor, isDetallado, isFacturdo);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #endregion

        #region Proyectos

        /// <summary>
        /// Funcion q se encarga de traer el cumplimiento del proyecto
        /// </summary>
        /// <param name="channel">Cannal de trasmision de Datos</param>
        /// <param name="FechaCorte">Fecha de Corte</param>
        /// <param name="Proyecto">Filtra un Proyecto Especifico</param>
        /// <param name="Estado">Filtra un Estado Especifico</param>
        /// <param name="LineaFlujo">Filtra un LineaFlujo Especifico</param>
        /// <param name="Etapa">Filtra un Etapa Especifico</param>
        /// <returns></returns>
        public DataTable ReportesProyectos_Cumplimiento(Guid channel, DateTime FechaCorte, string Proyecto, string Estado, string LineaFlujo, string Etapa)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modulo = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ReportesProyectos_Cumplimiento(FechaCorte, Proyecto, Estado, LineaFlujo, Etapa);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer la informacion del Presupuesto que se requiere para cada proyecto
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Perido a presupuestar</param>
        /// <param name="Proyecto">Filtra un proyecto especifico a verificar</param>
        /// <returns>Tabla con el presupuesto</returns>
        public string ReportesProyectos_EjecPresupuesto(Guid channel,byte tipoReporte, string proyecto,string centroCto,string cliente, string prefijo,int? docNro)
        {
            int opIndex = -1;
            string reportName = string.Empty;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (tipoReporte == 1) //Detallado
                {
                    Report_py_EjecucionPresupuesto reporte = new Report_py_EjecucionPresupuesto(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    reportName = reporte.GenerateReport(proyecto,cliente,prefijo,docNro);
                }
                return reportName;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de Planeacion costos
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="tipoReport">agrupamiento</param>
        /// <param name="mesIni">Mes Inicial</param>
        /// <param name="mesFin">Mes Fin</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_py_PlaneacionCostos(Guid channel, DTO_SolicitudTrabajo solicitud,bool useMultiplicadorInd, byte tipoReport, DateTime? mesIni, DateTime? mesFin)
        {
            int opIndex = -1;
            string reportName = string.Empty;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (solicitud == null)
                {
                    if (tipoReport == 1)
                    {
                        Report_py_PlanCostosResumido reporte = new Report_py_PlanCostosResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        reportName = reporte.GenerateReport(tipoReport, mesIni, mesFin);
                    }
                    else if (tipoReport == 2)
                    {
                        Report_py_PlanCostosResumido reporte = new Report_py_PlanCostosResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        reportName = reporte.GenerateReport(tipoReport, mesIni, mesFin);
                    } 
                }
                else
                {
                    if (tipoReport == 1 || tipoReport == 2)
                     {
                         ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                         Report_py_PlaneacionCostos reporte = null;
                         byte[] arr = module.aplReporte_GetByID(AppReports.pySolicitudProyecto);
                         if (arr != null)
                         {
                             XtraReport customReport = new Report_py_PlaneacionCostos();
                             using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                                 customReport.LoadLayout(memoryStream);

                             reporte = (Report_py_PlaneacionCostos)customReport;
                             reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf,null);
                         }
                         else
                             reporte = new Report_py_PlaneacionCostos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                         reportName = reporte.GenerateReport(solicitud, tipoReport);

                     }
                     else if (tipoReport == 2)
                     {                        
                         Report_py_PlaneacionCostosCliente reporte = new Report_py_PlaneacionCostosCliente(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                         reportName = reporte.GenerateReport(solicitud);

                     }
                     else if (tipoReport == 3)
                     {
                         Report_py_PlaneacionCostosCompar reporte = new Report_py_PlaneacionCostosCompar(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                         reportName = reporte.GenerateReport(solicitud);
                     }
                     else if (tipoReport == 4 || tipoReport == 5)
                     {
                         Report_py_PlaneacionCostosAPU reporte = new Report_py_PlaneacionCostosAPU(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                         reportName = reporte.GenerateReport(tipoReport,solicitud, useMultiplicadorInd);
                     }
                    else if (tipoReport == 6)
                    {
                         Report_py_PlanCostosAPUResumido reporte = new Report_py_PlanCostosAPUResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                         reportName = reporte.GenerateReport(solicitud, false);
                    }
                    else if (tipoReport == 7)
                    {
                        Report_py_PlanCostosAPUResumido reporte = new Report_py_PlanCostosAPUResumido(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                        reportName = reporte.GenerateReport(solicitud, true);
                    }
                    else if (tipoReport == 8)
                    {
                        ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                        Report_py_PlaneacionCostosDetallado reporte = null;
                        byte[] arr = module.aplReporte_GetByID(AppReports.pySolicitudProyectoDetallado);
                        if (arr != null)
                        {
                            XtraReport customReport = new Report_py_PlaneacionCostosDetallado();
                            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                                customReport.LoadLayout(memoryStream);

                            reporte = (Report_py_PlaneacionCostosDetallado)customReport;
                            reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, null);
                        }
                        else
                            reporte = new Report_py_PlaneacionCostosDetallado(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                        reportName = reporte.GenerateReport(solicitud, tipoReport);
                    }

                }
                return reportName;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion para crear el reporte de Actas
        /// </summary>
        /// <param name="solicitud">Datos</param>
        /// <param name="tipoReport">agrupamiento</param>
        /// <param name="useMultiplicadorInd">Uso del multiplicador</param>
        /// <returns>nombre del reporte</returns>
        public string Reportes_py_Actas(Guid channel, DTO_SolicitudTrabajo solicitud, bool useMultiplicadorInd, byte tipoReport)
        {
            int opIndex = -1;
            string reportName = string.Empty;
            try
            {
                opIndex = this.ADO_ConnectDB();

                if (solicitud == null)
                {
                    
                }
                else
                {
                    if (tipoReport == 1) //Acta de Trabajo
                    {
                        ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                        Report_py_ActaTrabajo reporte = null;
                        byte[] arr = module.aplReporte_GetByID(AppReports.pyActaTrabajo);
                        if (arr != null)
                        {
                            XtraReport customReport = new Report_py_ActaTrabajo();
                            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                                customReport.LoadLayout(memoryStream);

                            reporte = (Report_py_ActaTrabajo)customReport;
                            reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, null);
                        }
                        else
                            reporte = new Report_py_ActaTrabajo(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);

                        reportName = reporte.GenerateReport(solicitud, tipoReport);

                    }    
                }
                return reportName;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Tesoreria

        /// <summary>
        /// Fincion que retorna el nombre del reporte
        /// </summary>
        /// <returns>nombre del reporte</returns>
        public string Report_Ts_ChequesGirados(Guid channel, List<DTO_ChequesGirados> data)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                Query_Ts_ChequesGirados reporte = new Query_Ts_ChequesGirados(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, null);
                return reporte.GenerateReport(data);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que envia los parametros para generar el reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="bancoID">Banco ID</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orden">Orden del reporte</param>
        /// <param name="nombreBen">Nombre del beneiciario</param>
        /// <returns>nombre del reporte</returns>
        public string Report_Ts_ChequesGiradosRep(Guid channel, string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_ChequesGirados reporte = new Report_Ts_ChequesGirados();
                Report_Ts_ChequesGiradosTercero report = new Report_Ts_ChequesGiradosTercero();
                opIndex = this.ADO_ConnectDB();
                if (orden == "1") //Romp Por banco
                {
                    reporte = new Report_Ts_ChequesGirados(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    return reporte.GenerateReport(bancoID, terceroID, fechaIni, fechaFin, orden, nombreBen);
                }
                else //Romp Por Tercero
                {
                    report = new Report_Ts_ChequesGiradosTercero(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                    return report.GenerateReport(bancoID, terceroID, fechaIni, fechaFin, orden, nombreBen);
                }
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Servicio que envia los parametros para generar el reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="bancoID">Banco ID</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orden">Orden del reporte</param>
        /// <param name="nombreBen">Nombre del beneiciario</param>
        /// <returns>nombre del reporte</returns>
        public string Report_Ts_ChequesGiradosDetalle(Guid channel, string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_ChequesGiradosDetalle reporte = new Report_Ts_ChequesGiradosDetalle();
                opIndex = this.ADO_ConnectDB();

                reporte = new Report_Ts_ChequesGiradosDetalle(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(bancoID, terceroID, fechaIni, fechaFin, orden, nombreBen);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="nit">TerceroID</param>
        /// <param name="caja">id de caja</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_RecibosDeCaja(Guid channel, DateTime fechaIni, DateTime fechaFin, string nit, string caja)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_RecibosDeCaja reporte = new Report_Ts_RecibosDeCaja();
                opIndex = this.ADO_ConnectDB();

                reporte = new Report_Ts_RecibosDeCaja(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(fechaIni, fechaFin, nit, caja);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentID">Documento</param>
        /// <param name="numeroDoc">Identificador del Doc</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_ReciboCajaDoc(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_ReciboCajaDoc reporte = new Report_Ts_ReciboCajaDoc();
                opIndex = this.ADO_ConnectDB();

                reporte = new Report_Ts_ReciboCajaDoc(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf,numeroDoc);
                return reporte.GenerateReport(documentID, numeroDoc);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filtro">Banco</param>
        /// <param name="formatType">tipo de formato en el que se va a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        public string Report_Ts_LibroDeBancos(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, ExportFormatType formatType)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_LibroDeBancos reporte = new Report_Ts_LibroDeBancos();
                opIndex = this.ADO_ConnectDB();

                reporte = new Report_Ts_LibroDeBancos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf);
                return reporte.GenerateReport(fechaIni, fechaFin, filtro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que trae las facturas 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="bancoID">Filtro del banco</param>
        /// <returns>Lista de facturas</returns>
        public string Report_Ts_RelacionPagos(Guid channel, DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque, ExportFormatType exportype)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_RelacionPagos reporte = new Report_Ts_RelacionPagos();
                opIndex = this.ADO_ConnectDB();

                reporte = new Report_Ts_RelacionPagos(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportype);
                return reporte.GenerateReport(fechaIni, fechaFin, bancoID, nit, numCheque);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que trae las facturas 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="bancoID">Filtro del banco</param>
        /// <returns>Lista de facturas</returns>
        public string Report_Ts_RelacionPagosXBancos(Guid channel, DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque, ExportFormatType exportype)
        {
            int opIndex = -1;
            try
            {
                Report_Ts_RelacionPagosXBanco reporte = new Report_Ts_RelacionPagosXBanco();
                opIndex = this.ADO_ConnectDB();

                reporte = new Report_Ts_RelacionPagosXBanco(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportype);
                return reporte.GenerateReport(fechaIni, fechaFin, bancoID, nit, numCheque);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion q se encarga de traer el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de Trasmision de datos</param>
        /// <param name="numDoc">Identificador de las facturas a pagar</param>
        /// <param name="exportType">Tipo de Exportacion del reporte</param>
        /// <returns>Nombre del reporte</returns>
        public string ReportesTesoreria_PagosFactura(Guid channel,int documentID, int numDoc, ExportFormatType exportType, bool isTransferencia = false)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();

                ModuloAplicacion module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Report_Ts_PagoFacturas reporte = null;
                byte[] arr = module.aplReporte_GetByID(documentID);
                if ((documentID == AppDocuments.DesembolsoFacturas || documentID == AppReports.tsPagoFacturas) && arr != null)
                {
                    XtraReport customReport = new Report_Ts_PagoFacturas();
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    reporte = (Report_Ts_PagoFacturas)customReport;
                    reporte.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType, numDoc);
                }
                else
                    if (documentID == AppDocuments.TransferenciasBancarias || documentID == AppReports.tsTransferenciaBancos)
                    {
                        Report_Ts_TransaccionBancaria reportTranfer = null;
                        arr = module.aplReporte_GetByID(documentID);
                        if (arr != null)
                        {
                            XtraReport customReport = new Report_Ts_TransaccionBancaria();
                            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                                customReport.LoadLayout(memoryStream);

                            reportTranfer = (Report_Ts_TransaccionBancaria)customReport;
                            reportTranfer.InitUserReport(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType, numDoc);
                        }
                        else
                            reportTranfer = new Report_Ts_TransaccionBancaria(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType, numDoc);
                        
                        return reportTranfer.GenerateReport(numDoc);
                    }
                    else 
                        reporte = new Report_Ts_PagoFacturas(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, exportType, numDoc);


                return reporte.GenerateReport(numDoc);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #region EXCEL

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        ///  <param name="chequeNro">cheque Nro</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_Ts_TesoreriaToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero,string ChequeNro, 
                                                 string facturaNro, string bancoCuentaID, byte? agrup, byte? romp)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return module.Reportes_Ts_TesoreriaToExcel(documentoID, tipoReporte, fechaIni, fechaFin, tercero,ChequeNro, facturaNro, bancoCuentaID, agrup, romp);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #endregion

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            this.ADO_CloseDBConnection(-1);
            GC.SuppressFinalize(this);
        }
    }
}
