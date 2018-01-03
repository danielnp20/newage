using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using SentenceTransformer;
using NewAge.DTO.Reportes;
using System.Data;

namespace NewAge.Server.ReportesService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IReportesService
    {
        #region Conection Management

        /// <summary>
        /// Cierra la conexion
        /// </summary>
        [OperationContract]
        void ADO_CloseDBConnection(int connIndex);

        #endregion

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción
        /// </summary>
        /// <param name="DocumentID">Identificador del proceso (documento)</param>
        /// <returns>Retorna el porcentaje del progreso</returns>
        [OperationContract]
        int ConsultarProgreso(Guid channel, int documentID);

        #endregion

        #region Funciones Propias del Servicio

        /// <summary>
        /// Crea una nueva sesion pra el usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        [OperationContract]
        void CrearCanal(Guid channel);

        /// <summary>
        /// Cierra la sesion de un usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        [OperationContract]
        void CerrarCanal(Guid channel);

        /// <summary>
        /// Carga las variable de empresa y usario para el servicio
        /// </summary>
        /// <param name="emp">empresa</param>
        /// <param name="userID">userId</param>
        [OperationContract]
        void IniciarEmpresaUsuario(Guid channel, DTO_glEmpresa emp, int userID);

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
        [OperationContract]
        DTO_TxResult ReportesActivos_Saldos(Guid channel, string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType);

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
        [OperationContract]
        DTO_TxResult ReportesActivos_SaldosML(Guid channel, string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType);

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
        [OperationContract]
        DTO_TxResult ReportesActivos_SaldosME(Guid channel, string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, ExportFormatType formatType);

        [OperationContract]
        string ReportesActivos_ComparacionLibros(Guid channel, int año, int mes, string clase, string tipo, string grupo, string centroCost, string logFis,
            string proyecto, ExportFormatType formatType);

        [OperationContract]
        DTO_TxResult ReportesActivos_EquiposArrendados(Guid channel, DateTime Periodo, int Estado, string Tercero, string Plaqueta, string Serial, string TipoRef, string Rompimiento, ExportFormatType formatType);

        [OperationContract]
        DTO_TxResult ReportesActivos_ImpotacionesTemporales(Guid channel, DateTime Periodo, string Plaqueta, string Serial, string TipoRef, string Rompimiento, ExportFormatType formatType);

        #endregion

        #region Cartera

        /// <summary>
        /// Genera archivo plano para la pagaduria que tiene el centro de pago
        /// </summary>
        /// <param name="channel">Cannal de transmision de datos</param>
        /// <param name="pagaduria">filtro de pagaduria por cual se desea generar</param>
        /// <returns>Archivo plano</returns>
        [OperationContract]
        DTO_TxResult Report_Cc_ArchivosPlanos(Guid channel, string pagaduria);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_Aportes(Guid channel, DateTime mes, DateTime fechaIni, DateTime fechaFin, string filter, ExportFormatType formatType);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filter">Filtros</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_Aseguradora(Guid channel, DateTime fechaIni, DateTime fechaFin, bool orderName, string filter, ExportFormatType formatType);

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo a consultar</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <returns>Nombre del Reporte</returns>
        [OperationContract]
        string Report_Cc_AportesCliente(Guid channel, DateTime periodo, string clienteFiltro, ExportFormatType formatType);

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo a consultar</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <returns>Nombre del Reporte</returns>
        [OperationContract]
        string Report_Cc_Aportes_a_Clientes(Guid channel, int Año, int Mes,string _tercero, ExportFormatType formatType);

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFini">Fecha Final de la consulta</param>
        /// <param name="clienteFiltro">ClienteID</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_EstadoDeCuenta(Guid channel, DateTime fechaIni, DateTime fechaFin,string _tercero, string clienteFiltro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que obtiene la info correspondiente del cerdtificado de deuda por el numero de libranza y el mes 
        /// </summary>
        /// <param name="fechaCorte">Mes que se va a consultar</param>
        /// <param name="libranza">Numero de libranza</param>
        /// <returns>Info del certificado de deuda, ccCreditoDocu</returns>
        [OperationContract]
        DTO_ccCertificadoDeuda Report_Cc_CertificadoDeuda(Guid channel, DateTime fechaCorte, int libranza);

        /// <summary>
        /// Carga el reporte de la oferta
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        [OperationContract]
        string Report_Cc_Oferta(Guid channel, int numeroDoc, ExportFormatType formatType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="libranza"></param>
        /// <param name="formatType"></param>
        [OperationContract]
        string ReportesCartera_Cc_LiquidacionCredito(Guid channel, int libranza, ExportFormatType formatType, int numDoc);

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
        [OperationContract]
        DataTable Report_Cc_CarteraToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza,
                string zona, string ciudad, string concesionario, string asesor, string lineaCred, string compCart, string pagaduria, string centroPago, byte? agrup, byte? romp, object filter);

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
        [OperationContract]
        string Report_Cc_CarteraByParameter(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza,
                string zona, string ciudad, string concesionario, string asesor, string lineaCred, string compCart, string pagaduria, string centroPago, byte? agrup, byte? romp, object filter, int? numeroDoc);

        /// <summary>
        /// Servicio q retorna el nombre del reporte
        /// </summary>
        /// <param name="documentReportID">documento del Reporte</param>
        /// <param name="numDoc">identificador del documento</param>
        /// <param name="isAprobada">Es aprobada</param>
        /// <param name="formatType">tipo de reporte</param>
        /// <returns></returns>
        [OperationContract]
        string Report_Cc_CarteraByNumeroDoc(Guid channel, int documentID, string _nameProposito, int numDoc, DateTime fechaCorte, DateTime? fechaFNC, byte diasFNC, bool isAprobada, ExportFormatType formatType);

        /// <summary>
        /// Genera el reporte de la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna el nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_RecaudosMasivosGetRelacionPagos(Guid channel, int documentID, List<DTO_ccIncorporacionDeta> data);

         /// <summary>
         /// Genera el reporte de la relación de pagos de un recauso masivo
         /// </summary>
         /// <param name="data">Datos a migrar</param>
         /// <returns>Retorna el nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_CobroJuridico(Guid channel, int documentID, byte claseDeuda, byte tipoReporte, string cliente, string obligacion,byte tipoEstado);

        /// <summary>
        /// Genera el reporte de cobro juridico historico
        /// </summary>
        /// <param name="numDocCredito">num Doc del credito</param>
        /// <returns>Retorna el nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_CobroJuridicoHistoria(Guid channel, int documentID, int numDocCredito, byte claseDeuda);

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="claseDeuda">CLase de deuda</param>
        /// <returns></returns>
        [OperationContract]
        DataTable Report_Cc_CobroJuridicoToExcel(Guid channel, int documentoID, byte tipoReporte, string cliente, string libranza, byte claseDeuda);

        /// <summary>
        /// Trae la informacion de una solicitud de credito para devolición
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        [OperationContract]
        string Report_cc_DevolucionSolicitud(Guid channel, string _credito, int _numDoc, int _numDev);

        /// <summary>
        /// Reporte Datacrédito
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <returns></returns>
        [OperationContract]
        DataTable Report_Cc_DataCredito(Guid channel, DateTime periodo, byte tipo);

        /// <summary>
        /// Reporte Certificados
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="Lista de campos">data</param>
        /// <returns>nombre de reporte</returns>
        [OperationContract]
        string Report_Cc_Certificados(Guid channel, int document, Dictionary<int, string> data);

        /// <summary>
        /// Reporte Cartas
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="Lista de campos">data</param>
        /// <returns>nombre de reporte</returns>
        [OperationContract]
        string Report_Cc_CartaCierreDiario(Guid channel, int document, string tipoReport, List<DTO_ccHistoricoGestionCobranza> dataHistorico);

        /// <summary>
        /// Reporte de GEstion de Cobranza Dia o mes
        /// </summary>
        /// <param name="documentID">DocumentoID</param>
        /// <param name="tipoReporte">Tipo de reporte</param>
        /// <param name="fechaCorte">fecha periodo</param>
        /// <returns>nombre de reporte</returns>
        [OperationContract]
        string Report_Cc_GestionCobranza(Guid channel, int document, string tipoReport, DateTime fechaCorte, string etapa,bool excluyeDemanda);

        #region Incorporaciones

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc"></param>
        /// <param name="isLiquidacion"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        [OperationContract]
        string Report_Cc_Incorporacion(Guid channel, int numeroDoc, bool isLiquidacion, ExportFormatType formatType);

        /// <summary>
        /// Funcion q trae el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="formatType">Tipo de Formato de exportacion del reporte</param>
        /// <returns>Nombre del  reporte</returns>
        [OperationContract]
        string ReportesCartera_Cc_PagaduriaIncorporacion(Guid channel, DateTime FechaInicial, DateTime FechaFinal, string Pagaduria, ExportFormatType formatType);

        /// <summary>
        /// Funcion q genera el archivo de excel
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="FechaInicial">Filtro de Fecha Inicial desde que fecha se desean ver las Incoporaciones</param>
        /// <param name="FechaFinal">Filtro de Fecha Final hasta que fecha se desean ver las Incoporaciones</param>
        /// <param name="Pagaduria">Pagaduria que se desea filtrar</param>
        /// <param name="formatType">Tipo de Formato de exportacion del reporte</param>
        /// <returns>Nombre del  reporte</returns>
        [OperationContract]
        string ReportesCartera_Cc_PagaduriaIncorporacionPlantilla(Guid channel, DateTime FechaInicial, DateTime FechaFinal, string Pagaduria);
        #endregion

        #region Informes SIGCOOP

        /// <summary>
        /// Funcion que se encarga del traer los datos para excel
        /// </summary>
        /// <param name="channel">Canal de trasmion de datos</param>
        /// <param name="Periodo">Periodo a filtrar</param>
        /// <param name="Formato">Tipo de Formato que desea Exportar</param>
        /// <returns>Listado DTO</returns>
        [OperationContract]
        DataTable ReportesCartera_Cc_InformeSIGCOOP(Guid channel, DateTime Periodo, string Formato);

        #endregion

        #region Referenciacion

        [OperationContract]
        DTO_TxResult ReportesCartera_Cc_Referenciacion(Guid channel, string libranza, string cliente, DateTime FechaRef,bool _llamadaCtrl, ExportFormatType formatType);

        #endregion

        #region Libranzas

        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        DTO_TxResult ReportesCartera_Cc_Libranzas(Guid channel, DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria, ExportFormatType formatType);

        /// <summary>
        /// Funcion q se encarga de traer los comprobantes para general el archivo de excel
        /// </summary>
        /// <param name="channel">Canal de Trasmision de Datos</param>
        /// <param name="año">Año que se desea ver los comprobantaes</param>
        /// <param name="mes">Mes en que se desea ver los comprobantes</param>
        /// <param name="comprobanteID">Numero  de comprobante por el cual se desea filtrar</param>
        /// <param name="libro">Libro que se desea ver</param>
        /// <param name="comprobanteInicial">Numero comprobante Inicial (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <param name="comprobanteFinal">Numero comprobante Final (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <returns></returns>
        [OperationContract]
        DataTable ReportesCartera_PlantillaExcelLibranza(Guid channel, DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria);

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
        [OperationContract]
        string Report_Cc_AnalisisPagos(Guid channel, byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario , string asesor, string lineaCredi, string compCartera, ExportFormatType formatType);

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
        [OperationContract]
        DataTable Report_Cc_AnalisisPagosExcel(Guid channel, byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, ExportFormatType formatType);

        #endregion

        #region Saldos

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo del reporte</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cc_Saldos(Guid channel, DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor, ExportFormatType formatType);

        /// <summary>
        /// Servicio que envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo a consultar</param>
        /// <param name="clienteFiltro">Cliente ?</param>
        /// <param name="libranzaFiltro">Libranza?</param>
        /// <returns></returns>
        [OperationContract]
        string Report_Cc_SaldosAFavor(Guid channel, DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor, ExportFormatType formatType);

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
        /// <param name="formatType"></param>
        /// <returns></returns>
        [OperationContract]
        string Report_Cc_SaldosMora(Guid channel, DateTime perido, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, int plazo, string tipoCartera, ExportFormatType formatType);

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
        /// <param name="formatType"></param>
        /// <returns></returns>
        [OperationContract]
        string Report_Cc_CarteraMora(Guid channel, DateTime periodo, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, string orden, ExportFormatType formatType);

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
        [OperationContract] 
        object Report_Cc_SaldosNuevo(Guid channel, byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, byte agrupamiento, byte romp, ExportFormatType formatType);

        #endregion

        #region Solicitudes

        /// <summary>
        /// Servicio q se encarga de enviar el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fechaIncial">Fecha con que comienza el reporte</param>
        /// <param name="fechaFinal">Fehca con que termina el Reporte</param>
        /// <param name="cliente">Cliente que se desea ver</param>
        /// <param name="libranza">Libranza que se desea ver</param>
        /// <param name="asesor">Asesor que se desea Ver</param>
        /// <param name="formatType">Formato con q se va a exportar el reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesCartera_Cc_Solicitudes(Guid channel, DateTime fechaIncial, DateTime fechaFinal, string cliente, string libranza, string asesor, string estado, ExportFormatType formatType);

        #endregion

        #region Credito
        /// <summary>
        /// Servicio q se encarga de enviar el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIncial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="cliente"></param>
        /// <param name="libranza"></param>
        /// <param name="asesor"></param>
        /// <param name="estado"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        [OperationContract]
        string Reports_cc_Credito(Guid channel,int mesIni, int mesFin,int año, string libranza, string Credito);

        /// <summary>
        /// Reporte en Excel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <param name="año"></param>
        /// <param name="libranza"></param>
        /// <param name="Credito"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable Reports_cc_CreditoXLS(Guid channel, string Credito);
        #endregion        

        #region Preventa
        /// <summary>
        /// Reporte excel
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_libranzaTercero"></param>
        /// <param name="_cedulaTercero"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable ExportExcel_cc_GetVistaCesionesByPreventa(Guid channel, List<int> numeroDocs); 
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
        [OperationContract]
        string ReportesCartera_VentaCartera(Guid channel, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesCartera_VentaCarteraDetallado(Guid channel, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, ExportFormatType formatType);

        #endregion

        #region (CF) Prejuridico
        /// <summary>
        /// Reporte Prejuridico
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_tercero"></param>
        /// <param name="_mesIni"></param>
        /// <param name="_mesFin"></param>
        /// <param name="_año"></param>
        /// <returns>Retorna la Informacion del reporte Prejuridico (PDF)</returns>
        [OperationContract]
        string Report_cc_Prejuridico(Guid channel, string _tercero, int _mesIni, int _mesFin, int _año, string _report);
        #endregion

        #endregion

        #region Contabilidad

        #region Documentos

        #region Comprobante Manual

        /// <summary>
        /// Funcion que se encarga de traer el resultado con el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="numeroDoc">Identificador de documentos</param>
        /// <param name="isAprovada">Verifica si es aprobada (True: Trae los Datos de la Tabla coAuxiliar, False: Trae los datos de la tabla coAuxiliarPre) </param>
        /// <param name="moneda">Verifica la moneda que se esta trabajando (True:Local, False: Extranjera) </param>
        /// <param name="formatType">Forma de exportar el Reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_ComprobanteManual(Guid channel, int numeroDoc, bool isAprovada, bool moneda, ExportFormatType formatType);

        #endregion

        #region Documento Contable

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="numDoc">Identificador del con q se guardan los registro en la BD</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre </param>
        /// <param name="formatType">Formato para exportar el reporte</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesContabilidad_DocumentoContable(Guid channel, int numDoc, bool isAprovada, int documento, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesContabilidad_GetByParameter(Guid channel, int documentID, byte? tipoRep, DateTime? fechaIni, DateTime? fechaFin, string libro, string compID, int? compNro,
                      string cuenta, string tercero, string proyecto, string centroCto, string lineaPres, object otroFilter, byte? orden, byte? romp);

        #endregion

        #region Auxiliar

        /// <summary>
        /// Funcion que retorna el nombre del reporte Auxiliar ML
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
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
        [OperationContract]
        string ReportesContabilidad_AuxiliarML(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string CuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte Auxiliar ME
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
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
        [OperationContract]
        string ReportesContabilidad_AuxiliarME(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string CuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte Auxiliar Ambas Monedas
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
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
        [OperationContract]
        string ReportesContabiliad_AuxiliarMultiMoneda(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string CuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte Auxiliar x Tercero ML
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
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
        [OperationContract]
        string ReportesContabilidad_AuxiliarxTerceroML(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte Auxiliar x Tercero ME
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
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
        [OperationContract]
        string ReportesContabilidad_AuxiliarxTerceroME(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte Auxiliar x Tercero Ambas Mondeas
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
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
        [OperationContract]
        string ReportesContabiliad_AuxiliarxTerceroMultiMoneda(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFinal,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        [OperationContract]
        string ReportesContabilidad_PlantillaExcelAuxiliar(Guid channel, DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal);

        #endregion

        #region Balance

        [OperationContract]
        string ReportesContabilidad_BalanceComparativo(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType);

        [OperationContract]
        string ReportesContabilidad_BalanceComparativoME(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType);

        [OperationContract]
        string ReportesContabilidad_BalanceComparativosSaldosML(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType);

        [OperationContract]
        string ReportesContabilidad_BalanceComparativosSaldosME(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType);

        [OperationContract]
        string ReportesContabilidad_BalanceComparativosSaldosAM(Guid channel, string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de conexion</param>
        /// <param name="mes">Mes de consultas</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_InventariosBalance(Guid channel, int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin,int _año, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de conexion</param>
        /// <param name="mes">Mes de consultas</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_InventariosBalanceSinSaldo(Guid channel, int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin,int _año, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        [OperationContract]
        string ReportesContabilidad_PlantillaExcelInventarioBalance(Guid channel, int mes, string Libro, string cuentaIni, string cuentaFin, int _año);

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
        [OperationContract]
        string ReportesContabilidad_ReportBalancePruebas(Guid channel, int Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int _fechaIni, int _fechaFin, byte? Combo1, byte? Combo2, string proyecto, string centroCto);

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
        [OperationContract]
        DTO_TxResult ReportesContabilidad_CertificadoReteFuente(Guid channel, DateTime Periodo, string Impuesto, ExportFormatType formatType);

        #endregion

        #region Comprobante

        #region Comprobante

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_Comprobante(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_ComprobanteME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_ComprobanteMLyME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja, ExportFormatType formatType);

        #endregion

        #region Preliminar

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_ComprobantePreliminar(Guid channel,int documentID, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_ComprobantePreliminarME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_ComprobantePreliminarMLyME(Guid channel, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, ExportFormatType formatType);

        #endregion

        #region Control

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="año">Año que se desea mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtrar el reporte</param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_ComprobanteControl(Guid channel, int año, int mes, string comprobanteID, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesContabilidad_LibroDiario(Guid channel, int año, int mes, string tipoBalance, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se necesita ver</param>
        /// <param name="mes">Ver que se necesita ver</param>
        /// <param name="tipoBalance">Tipo balance</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_LibroDiarioComprobante(Guid channel, int año, int mes, string tipoBalance, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="año">Año que se necesita ver</param>
        /// <param name="mes">Ver que se necesita ver</param>
        /// <param name="tipoBalance">Tipo balance</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_LibroMayor(Guid channel, int año, int mes, string tipoBalance,/*, string cuentaIni, string cuentaFin,*/ ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro que se va a consultar</param>
        /// <param name="cuentaIni">Filtro rango cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango cuentas, Cuenta Final</param>
        /// <returns>URL con el nombre del reporte</returns>
        [OperationContract]
        string ReportesContabilidad_PlantillaExcelLibroMayor(Guid channel, int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/);

        #endregion

        #region Saldos

        #region Filtro Cuenta

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-Tercero)
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_SaldosCuentaTercero(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-CentroCosto)
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_SaldosCuentaCentroCosto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-Proyecto)
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_SaldosCuentaProyecto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Cuenta-LineaPresupuesto)
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_SaldosCuentaLineaPresupuesto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType);
        #endregion

        #region Filtro Tercero

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Tercero-Cuenta)
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_SaldosTerceroCuenta(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte (Tercero-CentroCosto)
        /// </summary>
        /// <param name="channel">Canal de Conexion</param>
        /// <param name="año">Año de consulta</param>
        /// <param name="mes">Mes que se quiere filtar</param>
        /// <param name="libro">Tipo de libro a mostrar</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesContabilidad_SaldosTerceroCentroCosto(Guid channel, int año, int mesInicial, int mesFin, string libro, ExportFormatType formatType);

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
        [OperationContract]
        DTO_TxResult ReportesContabilidad_TasasDiarias(Guid channel, DateTime Periodo, bool isDiaria, ExportFormatType formatType);

        /// <summary>
        ///  Funcion que se encarga de traer la informacion para el reporte de Tasas Cierre
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo a Consultar</param>
        /// <param name="isDiaria">Tipo de reporte a imprimir (True: Reportes Tasa Cierre, False: Reprote Tasa Diaria)</param>
        /// <param name="formatType">Tipo de Formato para exportar el reporte</param>
        /// <returns>Resultado con la URL del reporte</returns>
        [OperationContract]
        DTO_TxResult ReportesContabilidad_TasasCierre(Guid channel, DateTime Periodo, bool isDiaria, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesContabilidad_ReporteLineaParametrizable(Guid channel, int documentReportID, string reporteID, byte tipoReport, DateTime Periodoini, DateTime PeriodoFin);

        /// <summary>
        ///  Permite crear el reporte de presupuesto contable
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="periodo">Periodo Inicial</param>
        /// <param name="proyecto">Periodo Inicial</param>
        /// <param name="libro">Periodo Inicial</param>
        /// <param name="monedaID">Periodo Inicial</param>
        /// <returns>Resultado con la URL del reporte</returns>
        [OperationContract]
        string ReportesContabilidad_EjecucionPresupuestal(Guid channel, DateTime periodo, byte rompimiento, string proyecto, string centroCto, string libro, string monedaID);

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
        [OperationContract]
        DataTable Reportes_Co_ContabilidadToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string terceroID, string cuentaID, string centroCtoID, string proyectoID, string lineaPresupID, string balanceTipo, string comprobID,
                                                         string compNro, object otroFilter, byte? agrup, byte? romp);

        #region Balance

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
        [OperationContract]
        DataTable ReportesContabilidad_BalancePruebas(Guid channel, DateTime Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial, string CuentaFinal,
            string libro, string tipoReport, string Moneda);
            
        [OperationContract]
        DataTable ReportesContabilidad_ReporteBalancePruebasXLS(Guid channel, int Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                                                string CuentaFinal, string libro, string tipoReport, string Moneda, int _fechaIni, int _fechaFin, byte? Combo1, byte? Combo2, string proyecto, string centroCto);

        #endregion

        #region Comprobante

        /// <summary>
        /// Funcion que se encarga de traer la informacion para generar el XLS de los comprobantes
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="comprobanteID">Filtra un comprobante especifico</param>
        /// <param name="libro">Libro q se desea Consultar</param>
        /// <param name="comprobanteInicial">Numero de comprobante Inicial</param>
        /// <param name="comprobanteFinal">Numero de comprobante Final</param>
        /// <returns>Tabla con resultados</returns>
        [OperationContract]
        DataTable ReportesContabilidad_ComprobanteXLS(Guid channel, DateTime Periodo, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal);

        
        #endregion

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
        [OperationContract]
        string Report_Cp_CajaMenor(Guid channel, int? numeroDoc, string prefijoID, int? docNro, bool isPreliminar);

        #endregion

        #region Edades
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha inicial del reporte</param>
        /// <param name="fechaFin">fecha final del reporte</param>
        /// <param name="tercero">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cp_PorEdadesDetallado(Guid channel, DateTime fechaIni, string terceroID, string cuentaID, bool isDetallada, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaCorte">fecha de corte  del reporte</param>
        /// <param name="filtro">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cp_PorEdadesResumido(Guid channel, DateTime fechaCorte, string terceroID, string cuentaID, bool isDetallada, ExportFormatType formatType);

        #endregion

        #region Facturas
        /// <summary>
        /// Fincion que retorna el nombre del reporte
        /// <param name="channel"></param>
        /// <param name="fecha">fecha del periodod a consultar</param>
        /// <param name="tercero">Filtro del tercero</param>
        /// <returns>nombre del reporte</returns>
        /// </summary>
        [OperationContract]
        string Report_FacturasXPagar(Guid channel, DateTime fecha, string Tercero, int Moneda, string Cuenta, bool isMultimoneda, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha inicial del reporte</param>
        /// <param name="fechaFin">fecha final del reporte</param>
        /// <param name="tercero">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Reporte_Cp_FacturasPagadas(Guid channel, DateTime fechaIni, DateTime fechaFin, string tercero, ExportFormatType formatType);

        /// <summary>
        /// Retorna el mombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="numDoc">Identificador de la factura a causar</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>Nombre del Reporte</returns>
        [OperationContract]
        string Reportes_Cp_CausacionFacturas(Guid channel, int numDoc, bool isAprobada, bool isNotaCredito, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga del nombre del reporte
        /// </summary>
        /// <param name="channel">Canl de trasmision e datos</param>
        /// <param name="fecha">Fecha de la factura equivalente</param>
        /// <param name="tercero">Tercero a quien se le genera la Factura Equivalente</param>
        /// <param name="facturaEquivalente">Verifica si se desea imprimir la factura Equivalente</param>
        /// <param name="formatType">Tipo de formato de exportacion del reporte</param>
        /// <returns>URL del reporte</returns>
        [OperationContract]
        string Reportes_Cp_FacturaEquivalente(Guid channel, DateTime fecha, string tercero, bool facturaEquivalente, ExportFormatType formatType);

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
        [OperationContract]
        string Reportes_Cp_CxPvsPagos(Guid channel, byte tipoReport, DateTime periodoIni, DateTime periodoFin, string cuentaID, string bancoCuentaID, string terceroID, byte orden);
        
        #endregion

        #region Flujos
        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaCorte">fecha de corte  del reporte</param>
        /// <param name="filtro">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cp_FlujoSemanalResumido(Guid channel, DateTime fechaCorte, string filtro, ExportFormatType formatType);


        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaCorte">fecha de corte  del reporte</param>
        /// <param name="filtro">Filtro de terceroID</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Cp_FlujoSemanalDetallado(Guid channel, DateTime fechaCorte, string filtro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="fechaIni">fecha de corte  del reporte</param>
        /// <param name="Moneda">Filtro de Origen Monetario</param>
        /// <param name="Tercero">Filtro de terceroID</param>
        /// <param name="isDetallado">indica si es detallado</param>
        /// <param name="formatType">Tipo de formato a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesCuentasXPagar_FlujoSemanalDetallado(Guid channel, List<DateTime> fechaIni, int Moneda, string Tercero, bool isDetallado, ExportFormatType formatType);
        #endregion

        #region Libro de Compras

        /// <summary>
        /// Nombre del reporte de libro de compras
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fecha">Fecha q se desea ver las compras</param>
        /// <param name="tercero">Tercero especifico q se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el Reporte</param>
        /// <returns>URL del reporte</returns>
        [OperationContract]
        string Reportes_Cp_LibroCompras(Guid channel, DateTime fecha, string tercero, bool facturaEquivalente, ExportFormatType formatType);

        #endregion

        #region Radicaciones
        /// <summary>
        /// Funcion que devuelve al nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="filtro">Nit</param>
        /// <param name="formatType">Tipo de formato para imprimir el reporte</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Reporte_Cp_Radicaiones(Guid channel, int yearIni, int yearFin, DateTime fechaIni, DateTime fechaFin, string Tercero, string Estado, string Orden, ExportFormatType formatType);

        #endregion

        #region Tarjetas
        /// <summary>
        /// Funcion que devuelve el nombre del reporte 
        /// </summary>
        /// <param name="channel">Canal de transmisión de datos</param>
        /// <param name="numDoc">Filtra la información</param>
        /// <param name="formatType">Tipo de exportación del ducumento</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_CxP_TarjetasPagas(Guid channel, int numDoc, ExportFormatType formatType);

        /// <summary>
        /// Funcion que devuelve el nombre del reporte 
        /// </summary>
        /// <param name="channel">Canal de transmisión de datos</param>
        /// <param name="numDoc">Filtra la información</param>
        /// <param name="formatType">Tipo de exportación del ducumento</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesCuentasXPagar_LegalizaTarjetas(Guid channel, int numDoc, ExportFormatType formatType);

        #endregion

        #region Anticipos

        /// <summary>
        /// Funcion que devuelve el nombre del reporte 
        /// </summary>
        /// <param name="channel">Canal de transmisión de datos</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="Moneda">Filtra la información por origen moneda</param>
        /// <param name="Tercero">Filtra la información por tercero</param>
        /// <param name="isDetallado">indica si es detallado o resumido</param>
        /// <param name="formatType">Tipo de exportación del ducumento</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesCuentasXPagar_Anticipos(Guid channel, DateTime Fecha, int Moneda, string Tercero, bool isDetallado, ExportFormatType formatType);

        /// <summary>
        /// Funcion que devuelve el nombre del reporte 
        /// </summary>
        /// <param name="channel">Canal de transmisión de datos</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="Moneda">Filtra la información por origen moneda</param>
        /// <param name="Tercero">Filtra la información por tercero</param>
        /// <param name="isDetallado">indica si es detallado o resumido</param>
        /// <param name="formatType">Tipo de exportación del ducumento</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesCuentasXPagar_DocumentoAnticipo(Guid channel, int numDoc, bool isAprobada, ExportFormatType formatType);

        /// <summary>
        /// Funcion que devuelve el nombre del reporte 
        /// </summary>
        /// <param name="channel">Canal de transmisión de datos</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="Moneda">Filtra la información por origen moneda</param>
        /// <param name="Tercero">Filtra la información por tercero</param>
        /// <param name="isDetallado">indica si es detallado o resumido</param>
        /// <param name="formatType">Tipo de exportación del ducumento</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesCuentasXPagar_DocumentoAnticipoViaje(Guid channel, int numDoc, bool isAprobada, ExportFormatType formatType);
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
        [OperationContract]
        DataTable Reportes_Cp_CxPToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string facturaNro,
                                         string cuentaID, string bancoCuentaID,string moneda,object otroFilter, byte? agrup, byte? romp);

        [OperationContract]
        DataTable Reportes_CC_CxCToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime fechaIni, DateTime? fechaFin, string tercero, bool isDetallada);

        #endregion

        #endregion

        #region Facturacion

        /// <summary>
        /// Funcion que retorna El nombre del Reporte
        /// </summary>
        /// <param name="channel">Canal de Trasmision</param>
        /// <param name="numDoc">Numero Doc con q se guarda la factura</param>
        /// <param name="formatType">Formato de exportacion del reporte</param>
        /// <returns>URL del reporte</returns>
        [OperationContract]
        string ReportesFacturacion_FacturaVenta(Guid channel, int documentID, string numDoc, bool isAprobada, ExportFormatType formatType, decimal valorAnticipo, decimal valorRteGarantia, decimal? porcRteGarantia,bool printAIUind);

        /// <summary>
        /// Trae el nombre del reporte de facturas masivo
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="prefijo">Prefijo</param>
        /// <param name="docNroIni">nro Inicial</param>
        /// <param name="docNroIni">nro Inicial</param>
        /// <returns>URL del Reporte</returns>
        [OperationContract]
        string ReportesFacturacion_FacturaVentaMasivo(Guid channel, string prefijo, int docNroIni, int docnroFin);

        /// <summary>
        /// Funcion que retorna el nombre del reporte de cuentas por cobrar Detallado
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fechaCorte">Fecha corte par la consutlta</param>
        /// <param name="tercero">tercero por el cual se va a filtrar</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</para
        /// <returns>URL del reporte</returns>
        [OperationContract]
        string ReportesFacturacion_CxCPorEdadesDetalladas(Guid channel, DateTime fechaCorte, string tercero, bool isDetallada, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte de cuentas por cobrar Resumido
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="fechaCorte">Fecha corte par la consutlta</param>
        /// <param name="tercero">tercero por el cual se va a filtrar</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</para
        /// <returns>URL del reporte</returns>
        [OperationContract]
        string ReportesFacturacion_CxCPorEdadesResumida(Guid channel, DateTime fechaCorte, string tercero, bool isDetallada, ExportFormatType formatType);


        [OperationContract]
        string ReportesFacturacion_LibroVentas(Guid channel, DateTime periodo, int diaFinal, string cliente, string prefijo, string NroFactura, ExportFormatType formatType);

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
        [OperationContract]
        string Report_CuentasXCobrar(Guid channel, DateTime fecha, string Tercero, int Moneda, string Cuenta, bool isMultimoneda, ExportFormatType formatType);

        #endregion

        #region Global

        /// <summary>
        /// Funcion que se encarga de traer el Nombre del reporte de Documentos Pendientes
        /// </summary>
        /// <param name="channel">Canale de trasmiscion de datos</param>
        /// <param name="Periodo">Periodo a consultar los documentos Pendientes<</param>
        /// <param name="modulo">Filtrar un modulo especifico</param>
        /// <param name="formatType">Tipo de formato a exportar el Reporte</param>
        /// <returns>URl del reportes</returns>
        [OperationContract]
        DTO_TxResult ReportesGlobal_DocumentosPendiente(Guid channel, DateTime Periodo,byte tipoReport, string modulo,string documentoID, ExportFormatType formatType);

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
        [OperationContract]
        string Reports_In_TransaccionMvto(Guid channel, DTO_MvtoInventarios mvto, int documentID, int numDoc, byte tipoMvto);


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
        /// <returns></returns>
        [OperationContract]
        string ReportesInventarios_Saldos(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro,
                string rompimiento, bool _parametro, string tipoReporte, ExportFormatType formatType);

        #endregion
        #region Kardex

        /// <summary>
        /// Funcion que retorna el nombre del reporte Kardex Detallado
        /// </summary>
        /// <param name="año">Año que se va a verificar</param>
        /// <param name="mesIni">Fecha inicial del reporte</param>
        /// <param name="bodega">Bodega que se quiere revisar</param>
        /// <param name="referencia">Tipo de referencia que se quiere ver</param>
        /// <param name="formatType">Tipo de formato de exportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesInventarios_KardexDetallado(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesInventarios_KardexSinParametros(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesInventarios_KardexxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesInventarios_KardexSinParametrosxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

        #endregion
        #region Serial

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
        /// <returns>Nombre del Reporte</returns>pe"></param>
        /// <returns></returns>
        [OperationContract]
        string ReportesInventarios_SerialxBodega(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesInventarios_SerialxReferencia(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesInventarios_SerialxBodegaCosto(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, ExportFormatType formatType);

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
        [OperationContract]
        string ReportesInventarios_SerialxReferenciaCosto(Guid channel, int año, int mesIni, string bodega, string tipoBodega,
                   string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial,string tipoReporte, ExportFormatType formatType);

        #endregion
        #region EXCEL

        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        
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
        /// </summary>
        [OperationContract]
        DataTable Reportes_In_InventarioToExcel(Guid channel, int documentID, DateTime? mesIni, DateTime? mesFin, string bodega, string tipoBodega, string referencia, string grupo, string clase, string tipo,
                                                        string serie, string material, bool isSerial, string libro, string proyectoID, string mvtoTipoID, object otroFilter, byte? agrup, byte? romp);


        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros        
        /// <param name="movimiento">Tipo  de movimiento</param>
        /// <param name="bodega">bodega</param>
        /// <param name="proyecto">proyecto</param>
        /// <param name="TipoReporte">report</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="detalle">detalle</param>
        /// <returns>Datatable</returns>
        /// </summary>

        [OperationContract]
        DataTable Reportes_In_DocumentoToExcel(Guid channel, string movimientoID, string bodegaID, string proyectoID, string tipoReporte, DateTime fechaIni, byte detalle);
        
        #endregion
        #region Inventario Fisico

        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        /// <param name="documentoID">Documento Asosiado</param>
        ///<param name="_listFisicoInventario">Lista de items a mostrar</param>
        ///<param name="tipoReporte">Tipo de Reporte</param>
        ///<returns>Resultado</returns>
        [OperationContract]
        string ReporteInvFisico(Guid channel, int documentID, string bodegaID, DateTime periodoID, List<DTO_inFisicoInventario> _listFisicoInventario, InventarioFisicoReportType tipoReporte);
        #endregion

        #region Relacion Prestamos
        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        /// <param name="proyecto">Documento Asosiado</param>
        ///<param name="inReferenciaID">Lista de items a mostrar</param>
        ///<returns>Resultado</returns>
        [OperationContract]
        string Report_RelacionPrestamos(Guid channel, string proyecto, string inReferenciaID);

        #endregion
        #region Movimiento Inventario
        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        /// <param name="proyecto">Documento Asociado</param>        
        ///<returns>Resultado</returns>
        [OperationContract]

        string Report_Movimientos(Guid channel, string movimientoID, string bodegaID, string proyectoID, string tipoReporte, DateTime fechaIni, byte resumido);
        #endregion
        #endregion

        #region Nomina

        [OperationContract]
        string Report_No_DetailLiquidaciones(Guid channel, int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid);

        /// <summary>
        /// Funcion que arma el Dto_NominaDetail por Concepto
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        [OperationContract]
        string Report_No_XConcepto(Guid channel, int documentoID, DateTime periodo, DateTime fechaIni, DateTime fechaFin, string orden, bool isAll, bool orderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid);

        /// <summary>
        /// Funcion que arma el Dto_NominaDetail por Empleado
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        [OperationContract]
        string Report_No_Detalle(Guid channel, int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid);

        /// <summary>
        /// Funcion que arma el Dto_NominaDetail por Empleado
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        [OperationContract]
        string Report_No_TotalXConcepto(Guid channel, int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool orderByName, bool isPre, ExportFormatType formatType, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid);

        /// <summary>
        /// Funcion que arma el DTO_ReporvacacionesPagadas
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        [OperationContract]
        string Report_No_VacacionesPagadas(Guid channel, DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, ExportFormatType formatType, String empleadoid);

        /// <summary>
        /// Funcion que arma el DTO_ReporvacacionesPagadas
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_documentoID"></param>
        /// <param name="_otroPorAlgo"></param>
        /// <returns>Vaciones Pendientes Iterface</returns>
        [OperationContract]
        string Report_No_VacacionesPendientes(Guid channel, string _empleadoID, int _vacaciones, ExportFormatType formatType);

        /// <summary>
        /// Reporte Documento de Liquidación 
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <param name="_vacaciones"></param>
        /// <param name="formatType"></param>
        /// <returns>Reporte Documento de Liquidación </returns>
        [OperationContract]
        string Report_No_VacacionesDocumento(Guid channel, string _empleadoID, int _vacaciones, string fechaFiltro);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_No_Prestamo(Guid channel, DateTime fechaIni, DateTime fechaFin, bool orderByName, ExportFormatType formatType, String empleadoid);


        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_No_AportesPension(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid);
         
        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_No_AportesSalud(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns> 
        [OperationContract]
        string Report_No_AporteVoluntarioPension(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="orderByName">Ordena por nombre?</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_No_AporteARP(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, ExportFormatType formatType, String terceroid, String nofondosaludid, String nocajaid);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="filtro"></param>
        /// <param name="orderByName"></param>
        /// <param name="formatType"></param>
        /// <param name="terceroid"></param>
        /// <param name="nofondosaludid"></param>
        /// <param name="nocajaid"></param>
        /// <returns></returns>
        [OperationContract]
        string Report_No_AportesCajaCompensacion(Guid channel,DateTime fechaIni, DateTime fechaFin, String _terceroID, ExportFormatType formatType);

        /// <summary>
        /// Servicio para los Gastos de Mempresa (Reporte)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="_terceroID"></param>
        /// <param name="formatType"></param>
        /// <returns></returns>
        [OperationContract]
        string Report_No_GastosEmpresa(Guid channel, DateTime fechaIni, DateTime fechaFin, String _terceroID, ExportFormatType formatType);

        /// <summary>
        /// Servicio q envia el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="periodo">Periodo de Pago</param>
        /// <param name="empleadoId">Identificador del Empleado</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_No_BoletaPago(Guid channel, string empleadoID, int _mes, int _año, string _documentoNomina, string _quincena, int? numDoc);

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
        [OperationContract]
        string Report_No_NominaGetByParameter(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                         string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp);


        #endregion

        #region General

        /// <summary>
        /// Genera reporte de errores encontrados en la importacion
        /// </summary>
        /// <returns>Lista de campos con error</returns>
        [OperationContract]
        string Rep_TxResult(Guid channel, DTO_TxResult result);

        /// <summary>
        /// Genera reporte de errores encontrados en la importacion
        /// </summary>
        /// <returns>Lista de campos con error</returns>
        [OperationContract]
        string Rep_TxResultDetails(Guid channel, List<DTO_TxResult> result);

        #endregion

        #region Operaciones Conjuntas

        /// <summary>
        /// Funcion que se encarga de traer los datos de cierre
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="Periodo">Periodo que se va a consultar</param>
        /// <returns>Tabla con datos</returns>
        [OperationContract]
        DataTable ReportesOperacionesConjuntas_Legalizaciones(Guid channel, DateTime Periodo);

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
        [OperationContract]
        DataTable ReportesPlaneacion_CierreLegalizacion(Guid channel, DateTime Periodo, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso);

        #endregion

        #region Presupuesto

        #region NO BORRAR
        /*//Reportes Sin Consolidar
        /// <summary>
        /// Funcion que se encarga del nombre del reporte Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="periodo">periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL Con el reporte</returns>
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_PresupuestoMLSinConsolidar(Guid channel, DateTime periodo, string proyecto, bool isAcumulado, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga del nombre del reporte Moneda Extranjera
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="periodo">periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL Con el reporte</returns>
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_PresupuestoMESinConsolidar(Guid channel, DateTime periodo, string proyecto, bool isAcumulado, ExportFormatType formatType);

        //Reportes Consolidados
        /// <summary>
        /// Funcion que se encarga del nombre del reporte Moneda Local
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="periodo">periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL Con el reporte</returns>
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_PresupuestoMLConsolidados(Guid channel, DateTime periodo, string proyecto, bool isAcumulado, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga del nombre del reporte Moneda Extranjera
        /// </summary>
        /// <param name="channel">Canal de trasmision de datos</param>
        /// <param name="periodo">periodo q se desea ver</param>
        /// <param name="proyecto">Proyecto que se desea ver</param>
        /// <param name="formatType">Tipo de formato para exportar el reporte</param>
        /// <returns>URL Con el reporte</returns>
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_PresupuestoMEConsolidados(Guid channel, DateTime periodo, string proyecto, bool isAcumulado, ExportFormatType formatType);*/

        #endregion

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
        [OperationContract]
        DataTable ReportesPlaneacion_PresupuestoAcumulado(Guid channel, DateTime periodo, string proyecto, bool isAcumulado, bool tipoMoneda, bool isConsololidado);
        #endregion

        #region Ejecucion Presupuestal

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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalProyectoxActividadML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalProyectoxActividadME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCtoML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Liena por Centro de Costo Moneda Extranjera
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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCtoME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Recurso Moneda Local
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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXRecursoML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Linea por Recurso Moneda Extranjera
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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalLineaXRecursoME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Recurso por Actividad Moneda Local
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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalRecursoXActividadML(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada Recurso por Actividad Moneda Local
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
        [OperationContract]
        DTO_TxResult ReportesPlaneacion_EjecucionPresupuestalRecursoXActividadME(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID, ExportFormatType formatType);

        /// <summary>
        /// Funcion que se encarga de traer los datos para generar el reportes de Ejecucion Presupuestal por Origen
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        [OperationContract]
        DataTable ReportesPlaneacion_EjecucionPresupuestalXOrigen(Guid channel, DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID);
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
        [OperationContract]
        DataTable ReportesPlaneacion_SobreEjecucion(Guid channel, int year, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso, string usuario, string prefijo, string numeroDoc);

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
        [OperationContract]
        DTO_TxResult ReportesProveedores_CompromisosVSFacturas(Guid channel, DateTime FechaInicial, DateTime FechaFinal, string proveedor, ExportFormatType formatType);

        /// <summary>
        /// Funcion que trae la trazabilidad de los proveedores
        /// </summary>
        /// <param name="FechaInicial">Fecha de consulta inicial</param>
        /// <param name="FechaFinal">Fecha de consulta final</param>
        /// <param name="resumidoInd">valida si es resumido o detallado</param>
        ///  <param name="proveedor">Filtra un proveedor en especifico</param>
        ///  <param name="proyectoID">Filtra un proyecto en especifico</param>
        /// <returns>Listado de DTO</returns>
        [OperationContract]
        string  Report_ProveedoresTrazabilizad(Guid channel, DateTime fechaIni, DateTime? fechaFin, bool resumidoInd, bool pendientesInd, string proveedorID, string proyectoID);


        #endregion

        #region Orde Compras

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
        [OperationContract]
        DTO_TxResult ReportesProveedores_OrdenCompras(Guid channel, DateTime FechaIni, DateTime FechaFin, string Proveedor, Dictionary<int, string> filtros, bool isDetallado, string Moneda, ExportFormatType formatType);

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
        [OperationContract]
        DTO_TxResult ReportesProveedores_OrdenComprasDetallada(Guid channel, DateTime FechaIni, DateTime FechaFin, string Proveedor, Dictionary<int, string> filtros, bool isDetallado, string Moneda, ExportFormatType formatType);
        #endregion

        #region Estado de Compras
        [OperationContract]
        string ReportesProveedores_Solicitudes(Guid channel, Dictionary<int, string> filtros, ExportFormatType formatType);

        /// <summary>
        /// Genera el reporte de un documento de Solicitud o Recibido
        /// </summary>
        /// <param name="documentoID">Id del Reporte</param>
        /// <param name="numeroDoc">numero del Doc</param>
        /// <param name="isPreliminar">si es para aprobacion</param>
        /// <param name="tipoReporte">Tipo de reporte</param>
        /// <returns></returns>
        [OperationContract]
        string ReportesProveedores_SolicitudOrRecibidoDoc(Guid channel, int documentoID, int numeroDoc, bool isPreliminar, byte tipoReporte);

        #endregion

        #region OrdenCompra
        /// <summary>
        /// Retorna el Nombre del reporte
        /// </summary>
        /// <param name="channel">Canal de trasmicion de Datos</param>
        /// <param name="numDoc">Identificar de los facturas q se va a pagar</param>
        /// <param name="exportType">Formato de Exportacion del reporte</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string ReportesProveedores_OrdenCompra(Guid channel, int numDoc, byte tipoReporte,bool showReport, bool isPreliminar = false);
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
        [OperationContract]
        DTO_TxResult ReportesProveedores_Recibidos(Guid channel, DateTime Periodo, string proveedor, bool isDetallado, Dictionary<int, string> filtros, bool isFacturdo, ExportFormatType exportType);

        /// <summary>
        /// Funcion que se encarga de traer las ordenes recibidas Detallado
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="exportType">Formato de Ezportacion del reporte</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReportesProveedores_RecibidosDetallado(Guid channel, DateTime Periodo, string proveedor, bool isDetallado, Dictionary<int, string> filtros, bool isFacturdo, ExportFormatType exportType);

        #endregion

        #region Excel
        
        /// <summary>
        /// Funcion que se encarga de traer los datos para generar el reportes de Ejecucion Presupuestal por Origen
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        [OperationContract]
        DataTable ReportesProveedores_ProcedimientoComprasXLS(Guid channel,Dictionary<int, string> filtros,DateTime fechaIni, DateTime fechaFinal);
        
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
        [OperationContract]
        DataTable ReportesProyectos_Cumplimiento(Guid channel, DateTime FechaCorte, string Proyecto, string Estado, string LineaFlujo, string Etapa);

        /// <summary>
        /// Funcion que se encarga de traer la informacion del Presupuesto que se requiere para cada proyecto
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="Periodo">Perido a presupuestar</param>
        /// <param name="Proyecto">Filtra un proyecto especifico a verificar</param>
        /// <returns>Tabla con el presupuesto</returns>
        [OperationContract]
        string ReportesProyectos_EjecPresupuesto(Guid channel, byte tipoReporte, string proyecto, string centroCto, string cliente, string prefijo, int? docNro);

        /// <summary>
        /// Funcion para crear el reporte de Planeacion costos
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="agrupam">agrupamiento</param>
        /// <param name="mesIni">Mes Inicial</param>
        /// <param name="mesFin">Mes Fin</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Reportes_py_PlaneacionCostos(Guid channel, DTO_SolicitudTrabajo solicitud, bool useMultiplicadorInd, byte agrupam, DateTime? mesIni, DateTime? mesFin);

        /// <summary>
        /// Funcion para crear el reporte de Indicadores
        /// </summary>
        /// <param name="channel">Canal de trasmision de Datos</param>
        /// <param name="agrupam">periodo</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Reportes_py_Indicadores(Guid channel,DateTime? periodo);

        /// <summary>
        /// Funcion para crear el reporte de Actas TRabajo
        /// </summary>
        /// <param name="solicitud">Datos</param>
        /// <param name="actaNroActual">acta actual</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Reportes_py_ActaTrabajo(Guid channel, DTO_SolicitudTrabajo solicitud, int actaNroActual, DateTime fechaActa);

        /// <summary>
        /// Funcion para crear el reporte de Actas
        /// </summary>
        /// <param name="solicitud">Datos</param>
        /// <param name="actaNroActual">Acta actual</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Reportes_py_ActaEntrega(Guid channel, DTO_SolicitudTrabajo solicitud, int actaNroActual, DateTime fechaActa);

        /// <summary>
        /// Funcion para crear el reporte de resumen Documentos de COtizacion
        /// </summary>
        /// <param name="mesIni">MEs inicial</param>
        /// <param name="mesFin">MEs Final</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Reportes_pyResumenDocsCotiz(Guid channel, DateTime mesIni, DateTime mesFin);

        #endregion

        #region Tesoreria

        /// <summary>
        /// Fincion que retorna el nombre del reporte
        /// </summary>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_ChequesGirados(Guid channel, List<DTO_ChequesGirados> data);

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
        [OperationContract]
        string Report_Ts_ChequesGiradosRep(Guid channel, string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen);

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
        [OperationContract]
        string Report_Ts_ChequesGiradosDetalle(Guid channel, string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen);


        /// <summary>
        /// Servicio que envia los parametros para generar el reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <returns>nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_Ejecutado(Guid channel,DateTime fechaIni, string tipoReporte);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="nit">TerceroID</param>
        /// <param name="caja">id de caja</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_RecibosDeCaja(Guid channel, DateTime fechaIni, DateTime fechaFin, string nit, string caja);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentID">Documento</param>
        /// <param name="numeroDoc">Identificador del Doc</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_ReciboCajaDoc(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filtro">Banco</param>
        /// <param name="formatType">tipo de formato en el que se va a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_LibroDeBancos(Guid channel, DateTime fechaIni, DateTime fechaFin, string filtro, ExportFormatType formatType);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filtro">Banco</param>
        /// <param name="formatType">tipo de formato en el que se va a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_RelacionPagos(Guid channel, DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque, ExportFormatType exportype);

        /// <summary>
        /// Funcion que retorna el nombre del reporte
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filtro">Banco</param>
        /// <param name="formatType">tipo de formato en el que se va a imprimir</param>
        /// <returns>Nombre del reporte</returns>
        [OperationContract]
        string Report_Ts_RelacionPagosXBancos(Guid channel, DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque, ExportFormatType exportype);

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
        [OperationContract]
        string ReportesTesoreria_PagosFactura(Guid channel, int documentID, int numDoc, ExportFormatType exportType, bool isTransferencia = false);

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
        [OperationContract]
        DataTable Reportes_Ts_TesoreriaToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero,string ChequeNro,
                                                string facturaNro,string bancoCuentaID, byte? agrup, byte? romp);
       

        #endregion
        #endregion

        #endregion
    }
}
