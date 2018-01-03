using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.DTO.Negocio
{
    #region General

    /// <summary>
    /// Enumeración para el manejo de ordenamiento en las funciones de paginación
    /// </summary>
    public enum OrderDirection
    {
        ASC,
        DESC
    }

    /// <summary>
    /// Enumeración para manejo de versiones
    /// </summary>
    public enum VersionResults
    {
        Earlier = -1,
        Same = 0,
        Later = 1
    }

    /// <summary>
    /// Enumeracion con las posibles respuesta del logeo del usuario
    /// </summary>
    public enum UserResult
    {
        NotExists = 0,
        NotInSite = 1,
        AlreadyMember = 2,
        AlreadyInSite = 3,
        UserAdded = 4,
        UserUpdated = 5,
        PasswordUpdated = 6,
        IncorrectPassword = 7,
        Error = 8,
        IncorrectAnswer = 9,
        NotActive = 10,
        NotRegistered = 11,
        BlockUser = 12
    }

    /// <summary>
    /// Enumeración con la lista de archivos locales que se deben actualizar
    /// </summary>
    public enum BinaryFiles
    {
        Program,
        Languages
    }

    /// <summary>
    /// Enumeración para la lista de prefijos de los modulos
    /// </summary>
    public enum ModulesPrefix
    {
        ac = 6, // Activos Fijos
        apl = 99, // Aplicacion
        cc = 16, // Cartera Corporativa
        cf = 17, // Cartera Financiera
        co = 1, // Contabilidad
        cp = 2, // Cuentas por pagar
        di = 7, // Diferidos
        fa = 4, // Facturación
        gl = 30, // Global
        @in = 8, // Inventarios
        im = 14, // Impuestos
        no = 11, // Nomina
        oc = 9, // Operaciones conjuntas
        pl = 10, // Planeación
        pr = 5, // Proveedores
        py = 15, // Proyectos
        se = 20, // Seguridad
        ts = 3, // Tesoreria
        rh = 13, // Recursos Humanos
        dr = 50 // Recursos Humanos
    }

    /// <summary>
    /// Enumeración para los tipos de formas existentes
    /// </summary>
    public enum FormTypes
    {
        Master = 1,
        Process = 2,
        Bitacora = 3,
        Report = 4,
        Query = 5,
        Document = 6,
        DocumentAprob = 7,
        Control = 8,
        Activities = 9,
        Other = 99
    }

    /// <summary>
    /// Lista con las posibles acciones
    /// </summary>
    public enum FormsActions
    {
        Get = 0, // 2^0 = 1
        Add = 1, // 2^1 = 2
        Edit = 2, // 2^2 = 4
        Delete = 3, // 2^3 = 8
        Print = 4, // 2^4 = 16
        GenerateTemplate = 5, // 2^5 = 32
        Copy = 6, // 2^6 = 64
        Paste = 7, // 2^7 = 128
        Import = 8, // 2^8= 256
        Export = 9, // 2^9 = 512
        SpecialRights = 10, // 2^10 = 1024
        Revert = 11, // 2^11 = 2048
        SendtoAppr = 12, // 2^12 = 4096
        Approve = 13, // 2^13 = 8192
        Search = 14, // 2^14 = 16384
    }

    /// <summary>
    /// Enumeración con las posibles operaciones de un registro
    /// </summary>
    public enum MessageType
    {
        Confirmation,
        Error,
        Message,
        Warning
    }

    /// <summary>
    /// Tipos de campos de texto en un formulario
    /// </summary>
    public enum TextFieldType
    {
        Everything,
        NoSymbols,
        Numbers,
        Letters
    }

    /// <summary>
    /// opciones para manejo de grupos de empresas en las maestras
    /// </summary>
    public enum GrupoEmpresa
    {
        Automatico = 1,
        Individual = 2,
        General = 3
    }

    /// <summary>
    /// Enumeración con el control de saldos
    /// </summary>
    public enum SaldoControl
    {
        Cuenta = 1,
        Doc_Interno = 2,
        Doc_Externo = 3,
        Componente_Tercero = 4,
        Componente_Activo = 5,
        Componente_Documento = 6,
        Inventario = 7
    }

    /// <summary>
    /// Enumeracion con los tipos de monedas
    /// </summary>
    public enum TipoMoneda
    {
        Local = 1,
        Foreign = 2,
        Both = 3,       
    }

    /// <summary>
    /// Enumeracion con los tipos de monedas (Solo local y extranjera)
    /// </summary>
    public enum TipoMoneda_LocExt
    {
        Local = 1,
        Foreign = 2
    }

    /// <summary>
    /// Enumeración con la naturaleza de una cuenta
    /// </summary>
    public enum NaturalezaCuenta
    {
        Debito = 1,
        Credito = 2
    }

    /// <summary>
    /// Enumeración con las posibles unidades de tiempo
    /// </summary>
    public enum UnidadTiempo
    {
        NoAplica = 0,
        Minuto = 1,
        Hora = 2,
        Dia = 3,
        Semana = 4,
        Mes = 5
    }


    /// <summary>
    /// Enumeracion con los tipos de los archivos
    /// </summary>
    public enum TipoArchivo
    {
        AnexosDocumentos,
        Documentos,
        Imagenes,
        Mails,
        Plantillas,
        Temp
    }

    /// <summary>
    /// Enumeracion con los tipos de correos genericos
    /// </summary>
    public enum MailType
    {
        NewDoc,
        SendToApprove,
        Approve,
        Reject,
        Assign,
        NotSend,
    }

    public enum ExportFormatType
    {
        pdf,
        xls,
        xlsx,
        html,
        Diseñador
    }

    /// <summary>
    /// Application response codes
    /// </summary>
    public enum ResponseCode
    {
        Success = 0,
        EntityNotAdded = 1,
        EntityNotUpdated = 2,
        EntityNotDeleted = 3,
        EntityNotExists = 4,
        EntityExists = 5,
        InvalidCredentials = 6,
        NotAuthorized = 7,
        GeneralError = 8,
        ReservationTablesBlocked = 9,
        CutomerUserInactive = 11,
        NotRegistered = 12
    }

    #endregion

    #region Activos

    /// <summary>
    /// Ennumeracion con el tipo de activo
    /// </summary>
    public enum TipoActivo
    {
        Activo = 0,
        Inventario = 1,
        Diferido = 2
    }

    /// <summary>
    /// Enumeracion para el tipo de Codigo
    /// </summary>
    public enum TipoCodigo
    {
        Servicio = 0,
        Inventario = 1,
        Activo = 2,
        Diferido = 3,
        Suministros = 4,
        SuministroPersonal = 5,
        NoAplica = 6
    }

    /// <summary>
    /// Enumeracion para el Tipo de Depreciación
    /// </summary>
    public enum TipoDepreciacion
    {
        LineaRecta = 0,
        SaldosDecrecientes = 1,
        UnidadesDeProduccion = 2
    }

    /// <summary>
    /// Enumeracion para Presupuestar Costos
    /// </summary>
    public enum PresupuestarCostos1
    {
        CentrosDeCostos = 1,
        Proyectos = 2
    }

    /// <summary>
    /// Enumeracion para Presupuestar Costos
    /// </summary>
    public enum PresupuestarCostos2
    {
        LineaPresupuestal = 1,
        ConceptoCargo = 2
    }

    /// <summary>
    /// Enumeracion para Moneda
    /// </summary>
    public enum Mda
    {
        Local = 1,
        Extranjera = 2
    }



    /// <summary>
    /// Enumeración para el traslado de activos  .
    /// </summary>
    public enum TipoMvtoTraslado
    {
        Traslado = 4,
        Mantenimiento = 8,
        AsignacionResponsable = 11
    }

    /// <summary>
    /// Enumeracion con los posibles años de la aplicacion
    /// </summary>
    public enum AñosSaldos
    {
        Dos_Mil = 2000,
        Dos_Mil_Uno = 2001,
        Dos_Mil_Dos = 2002,
        Dos_Mil_Tres = 2003,
        Dos_Mil_Cuatro = 2004,
        Dos_Mil_Cinco = 2005,
        Dos_Mil_Seis = 2006,
        Dos_Mil_Siete = 2007,
        Dos_Mil_Ocho = 2008,
        Dos_Mil_Nueve = 2009,
        Dos_Mil_Diez = 2010,
        Dos_Mil_Once = 2011,
        Dos_Mil_Doce = 2012,
        Dos_Mil_Trece = 2013,
        Dos_Mil_Catorce = 2014,
        Dos_Mil_Quince = 2015,
        Dos_Mil_DieciSeis = 2016,
        Dos_Mil_DieciSiete = 2017,
        Dos_Mil_Dieciocho = 2018,
        AsignacionResponsable = 11
    }

    public enum Turnos
    {
        Uno = 1,
        Dos = 2,
        Tres = 3
    }

    public enum BaseCalculo
    {
        Pozo = 1,
        Campo = 2
    }

    public enum TipoCalculoIFRS
    {
        ReservasProbadas = 1,
        ReservasProbables = 2
    }

    public enum TipoBalance
    { 
       Fiscal = 1,
       IFRS = 2
    }

    public enum TipoMovimientoAct
    {
        Compra  = 0,  
        Adición = 1, 
        Baja = 2,
        Venta = 3, 
        Traslado = 4, 
        AjusteSaldo = 5, 
        EnPoderTerceros = 6,  
        Capitalización = 7,  
        Depreciacion = 8, 
        Arriendo = 9, 
        Deterioro = 10, 
        AltaInventarios = 11, 
        AdicionInventarios = 12, 
        Mantenimiento = 13 
    }

    #endregion

    #region Cartera

    /// <summary>
    /// Ennumera las opciones de control de cuentas
    /// </summary>
    public enum CuentaControl
    {
        Balance = 1,
        Orden = 2, 
        Patrimonio = 3
    }

    /// <summary>
    /// Enumeracion de los estados de deuda del credito
    /// </summary>
    public enum EstadoDeuda
    {
        CuotasPendientes = 0,
        Normal = 1,
        Mora = 2,
        Prejuridico = 3,
        Juridico = 4,
        AcuerdoPago = 5,
        AcuerdoPagoIncumplido = 6,
        Castigada = 7
    }

    /// <summary>
    /// Enumeracion de los estados de poliza del credito
    /// </summary>
    public enum EstadoPoliza
    {
        NuevaConCredito = 1,
        Renovada = 2,
        NuevaSinCredito = 3,
        PolizaExternaNueva = 4,
        PolizaExternaRenovada = 5,
        PolizaPagada = 6,
        PolizaNuevaInmuebles = 7,
        PolizaRenuevaInmuebles = 8
    }

    /// <summary>
    /// Fuente de liquidacion
    /// </summary>
    public enum FuenteLiquida
    {
        NoAplica = 0,
        Componente = 1,
        LineaCredito = 2,
        Monto = 3,
        MontoPlazo = 4,
        ComponenteEdad = 5,
        Pagaduria = 6
    }

    /// <summary>
    /// Indicador de liquidacion de IVA (cartera corporativa)
    /// </summary>
    public enum IVALiquida
    {
        NoAplica = 1,
        Incluido = 2,
        Adicional = 3
    }

    /// <summary>
    /// Enumeración con los propoósitos de los estados de cuenta
    /// </summary>
    public enum PropositoEstadoCuenta
    {
        Proyeccion = 1,
        Prepago = 2,
        RecogeSaldo = 3,
        RestructuracionAbono = 4,
        RestructuracionPlazo = 5,
        EnvioCobroJuridico = 6,
        Desistimiento = 7,
        CondonacionTotal = 8,
        CondonacionParcial = 9,
        Normalizacion = 10,
        CancelaPoliza = 11
    }

    /// <summary>
    /// Enumeracion de la Sector Cartera
    /// </summary>
    public enum SectorCartera
    {
        Solidario = 1,
        Financiero = 2,
        Bancario = 3
    }

    /// <summary>
    /// Enumeracion de la tasa de venta
    /// </summary>
    public enum TasaVenta
    {
        EfectivaAnual = 0,
        NominaMensual = 1
    }

    /// <summary>
    /// Ennumera los tipos de componentes de la cartera coorporativa
    /// </summary>
    public enum TipoComponente
    {
        CapitalSolicitado = 1,
        MayorValor = 2,
        DescuentoGiro = 3,
        ComponenteCuota = 4,
        ComponenteExtra = 5,
        ComponenteGasto = 6
    }

    /// <summary>
    /// Enumeracion con los tipos de compra de cartera
    /// </summary>
    public enum TipoCredito
    {
        Nuevo = 1,
        Refinanciado = 2,
        PolizaRenueva = 3,
        PolizaSinCredito = 4,
        RestructuracionConCambio = 5,
        RestructuracionSinCambio = 6
    }

    /// <summary>
    /// Ennumera los tipos de documentos que hay para una actividad
    /// </summary>
    public enum TipoDocumentoActividad
    {
        Sistema = 0,
        Chequeo = 1,
        Llamada = 2,
        Referencia = 3
    }

    /// <summary>
    /// Enumeracion con los tipos de estado de cartera
    /// </summary>
    public enum TipoEstadoCartera
    {
        Propia = 1, 
        Cedida = 2,
        CobroJuridico = 3,
        AcuerdoPago = 4,
        AcuerdoPagoIncumplido = 5,
        Restructurado = 6,
        Castigada = 7
    }

    /// <summary>
    /// Ennumera los tipos de garantias de la solicitud de cartera
    /// </summary>
    public enum TipoGarantia
    {
        Libranza = 1,
        TipoDocumento = 2,
        Real = 3
    }

    /// <summary>
    /// Enumeracion con los tipos de incorporacion de cartera
    /// </summary>
    public enum TipoIncorporaCartera
    {
        Afiliaciones = 1,
        Desafiliaciones = 2,
    }

    /// <summary>
    /// Enumeracion con los tipos de incorporacion de cartera
    /// </summary>
    public enum TipoIncorporaCarteraArchivos
    {
        Afiliaciones = 1,
        Desafiliaciones = 2,
        Incorporaciones = 3,
        Desincorporaciones = 4
    }

    /// <summary>
    /// Enumeracion con los tipos de Interes de cartera
    /// </summary>
    public enum TipoInteres
    {
        Efectivo = 0,
        Nominal = 1
    }

    /// <summary>
    /// Ennumeracion con los tipos de liquidaciones de la cartera corporativa
    /// </summary>
    public enum TipoLiquidacionCartera
    {
        NoAplica = 0,
        FactorSMMLV = 1,
        FactorSaldo = 2,
        Valor = 3,
        FactorVlrSolicitado = 4,
        FactorVlrCredito = 5,
        FactorVlrGiro = 6,
        FactorVlrNominal = 7,
        FactorSaldoPromediado = 8,
        AjustaVlrNominal = 9
    }

    /// <summary>
    /// Ennumeracion con los tipos de liquidaciones de la cartera corporativa
    /// </summary>
    public enum TipoLiquidacionComprador
    {
        CapitalInteres = 1,
        Total = 2
    }

    /// <summary>
    /// Tipo de pago para la cartera
    /// </summary>
    public enum TipoPago
    {
        Normal = 1,
        Mora = 2,
        Anticipado = 3,
        Prejuridico = 4,
        Juridico = 5,
        Prepago = 6,
        Castigado = 7
    }

    /// <summary>
    /// Tipo de valores en la cartera corporativa
    /// </summary>
    public enum TipoValor
    {
        NoAplica = 0,
        Cuota = 1,
        Valor = 2
    }

    /// <summary>
    /// Tipo de valores en la cartera corporativa
    /// </summary>
    public enum TipoVentaCartera
    {
        Todas = 0,
        Pendiente = 1,
        EnMora = 2,
        Prepagada = 3,
        Recomprada = 4
    }

    /// <summary>
    /// Ennumeración con los tipos de recaudos
    /// </summary>
    public enum TipoRecaudo //Si se agregan nuevos hay que cambiar la generación del compronante de pagos (ver la región de cartera propia)
    {
        Normal,
        PagoTotal,
        CobroJuridico,
        AcuerdoPago,
        AcuerdoPagoIncumplido
    }

    /// <summary>
    /// Ennumeración con los los tiposs de controles para compradores de cartera
    /// </summary>
    public enum TipoControlRecursos
    {
        Flujo = 1,
        RecursosDisponibles = 2,
        Banco = 3
    }

    /// <summary>
    /// Ennumeración porlas posibles pases de deuda de un crédito
    /// </summary>
    public enum ClaseDeuda
    { 
        Principal = 1,
        Adicional = 2
    }

    /// <summary>
    /// Tipos de movimiento histórico para el cobro jurídico
    /// </summary>
    public enum TipoMovimiento_CJHistorico
    { 
        CuentasVencidas = 0,
        CambiaEstado = 1,
        InteresMora = 2,
        Abono = 3,
        Poliza = 4,
        Gastos = 5,
        LiqJudicial = 6,
        Saldos = 7
    }

    /// <summary>
    /// Tipo de noveda de contrato
    /// </summary>
    public enum TipoNovedad
    {
        AdicionDigito = 1,
        ReIncorporar = 2,
        Actualizar = 3,
        NuevoDescuento = 4,
        CambioPagaduria = 5,
        Desincorpora = 6,
        SLN = 10
    }

    /// <summary>
    /// Tipo de noveda de contrato
    /// </summary>
    public enum OrigenDatoIncorporacion
    {
        Automatica = 1,
        Manual = 2,
        Reincorporacion = 3
    }

    #endregion

    #region Contabilidad

    /// <summary>
    /// Enumeracion con los tipos de monedas en la tabla de coDocumento
    /// </summary>
    public enum TipoMoneda_CoDocumento
    {
        NA = 1,
        Local = 2,
        Foreign = 3,
        Both = 4
    }

    /// <summary>
    /// Estado de un periodo segun el modulo
    /// </summary>
    public enum EstadoPeriodo
    {
        Abierto,
        Cerrado,
        EnCierre
    }

    /// <summary>
    /// Enumeracion para saber el estado de un ajuste en cambio
    /// </summary>
    public enum EstadoAjuste
    {
        NoData,
        Preliminar,
        Aprobado
    }

    /// <summary>
    /// Enumeracion con los periodos Fiscales
    /// </summary>
    public enum PeriodoFiscal
    {
        Anual = 1,
        Semestral = 2,
        Bimestral = 6,
        Trimestral = 4,
        Mensual = 12
    }

    /// <summary>
    /// Enumeracion con los signos de suma
    /// </summary>
    public enum SignoSuma
    {
        Suma = 1,
        Resta = 2,
        NoAplica = 3
    }

    /// <summary>
    /// Enumeracion con el alcance del impuesto
    /// </summary>
    public enum ImpuestoAlcance
    {
        Nacional = 1,
        Estatal = 2,
        Municipal = 3
    }

    /// <summary>
    /// Ennumeracion con los tipos de cuentas
    /// </summary>
    public enum TipoCuenta
    {
        Balance = 1,
        Resultado = 2,
        Patrimonio = 3,
        Orden = 4,
        Presupuestal = 5
    }

    /// <summary>
    /// Tipos de cuentas para el grupo
    /// </summary>
    public enum TipoCuentaGrupo
    {
        Costo = 1,
        IngresoXVenta = 2,
        OtrosIngresos = 3,
        IVADescontable = 4,
        InvTransito = 5,
        Otros = 6
    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd1
    {
        IVA, // Indica si el registro maneja IVA,
        Componente // Indica el componente que esta haciendo un pago (solo aplica para pagos de cartera)
    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd2
    {
        ValTarifa //Porcentaje del impuesto
    }

    /// <summary>
    /// Valor Minimo de un anticipo y Nro de Item en legalizaciones
    /// </summary>
    public enum AuxiliarDatoAdd3
    {
        ValMax, // Valor máximo que se puede digitar
        NroItemDetalle, // Nro de item al que le corresponde la contabilizacion
        LlaveDistribucion // LLave origen en distribución de comprobantes
    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd4
    {
        AjEnCambio,
        AjEnCambioContra,
        Anticipo,
        Contrapartida,
        Impuesto,
        Reclasificacion,
        Factura,
        Distribucion,
        Cuota
    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd5
    {
    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd6
    {

    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd7
    {

    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd8
    {

    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// Aplica para el modulo de cartera
    /// </summary>
    public enum AuxiliarDatoAdd9
    {
        Propia,
        Cedida,
        CobroJuridico,
        AciuerdoPago,
        AcuerdoPagoIncumplido
    }

    /// <summary>
    /// Dato que se crea en coAuxiliar para identificar registros creados x el sistema
    /// </summary>
    public enum AuxiliarDatoAdd10
    {
        Proveedores, // Indica viene del modulo de proveedores (Causacion de facturas)
        Directa, // Viene de forma directa (Causacion de facturas)
        NumeroDoc // Identifica un numeroDoc de un documento (Nomina)
    }

    /// <summary>
    /// Enumeracion del campo coSaldoControl
    /// </summary>
    public enum coSaldoControl
    {
        Cuenta,
        Doc_Interno,
        Doc_Externo,
        Componente_Tercero,
        Componente_Activo,
        Componente_Documento,
        Inventario
    }

    /// <summary>
    /// Enumeración con los tipos de proyectos
    /// </summary>
    public enum ProyectoTipo
    {
        Capex = 1,
        Opex = 2,
        Inversion = 3,
        Administracion = 4,
        Inventarios = 5,
        CapitalTrabajo = 6,
        Comercial = 7,
        Distribucion = 8,
        Construccion = 9,
        Investigación = 10,
        Desarrollo = 11
    }

    /// <summary>
    /// Enumeración con los tipos de Rompimiento
    /// </summary>
    public enum RompimientoSaldos
    {
        Cuenta = 1,
        Proyecto = 2,
        CentroCosto = 3,
        LineaPresupuesto = 4,
        ConceptoCargo = 5,
        Tercero = 6
    }

    /// <summary>
    /// Enumeración con los tipos de contabilzación de IVA
    /// </summary>
    public enum ContabilizaIVA
    {
        CuentaGeneral = 1,
        CuentaCosto = 2,
        CuentaEspecial = 3
    }

    /// <summary>
    /// Enumeración con los tipos de Impuestos local
    /// </summary>
    public enum ImpuestoLoc
    {
        PorConceptoCargo = 1,
        PorActividadEconomica = 2
    }
    #endregion

    #region CxP

    /// <summary>
    /// Estado intermedio de Cajas Menores
    /// </summary>
    public enum EstadoInterCajaMenor
    {
        NoAplica = 0,
        Solicitar = 1,
        Revisar = 2,
        Supervisar = 3,
        Contabilizar = 4
    }

    /// <summary>
    /// Estado de la factura en cuetnas por pagar
    /// </summary>
    public enum TipoMovimientoCXP
    {
        Radicado = 6,
        Devuelto = 7,
        Cerrado = -1
    }

    /// <summary>
    /// Enumeracion para el manejo de los tipos de viaje
    /// </summary>
    public enum TipoViaje
    {
        Nacional = 1,
        Exterior = 2
    }

    /// <summary>
    /// Tipo Concepto
    /// </summary>
    public enum TipoConcepto
    {
        Servicio = 1,
        VentaInv = 2,
        VentaAct = 3,
        RentaAct = 4,
        ReembGast = 5,
        Otros = 6,
    }

    /// <summary>
    /// Tipo Cargo
    /// </summary>
    public enum TipoCargo
    {
        Otros = 1,
        Alojamiento = 2,
        Alimentacion = 3,
        TransporteAereo = 4,
        TransporteTerrestre = 5,
        Viaticos = 6,
        Impuestos = 7,
    }

    /// <summary>
    /// Enumeracion para saber el estado de resume de impuestos
    /// </summary>
    public enum EstadoResumenImp
    {
        SinAprobar,
        Aprobado
    }

    /// <summary>
    /// Enumeracion para tipo de anticipo
    /// </summary>
    public enum TipoAnticipo
    {
        Pagados = 1,
        TarjetaCredito = 2,
        Otros = 3
    }
    #endregion

    #region Global

    /// <summary>
    /// Estado documento control
    /// </summary>
    public enum EstadoDocControl
    {
        Cerrado = -1,
        Anulado = 0,
        SinAprobar = 1,
        ParaAprobacion = 2,
        Aprobado = 3,
        Revertido = 4,
        Revisado = 5,
        Radicado = 6,
        Devuelto = 7
    }

    /// <summary>
    /// Enumeración con los tipos de prefijos que puede tener un documento
    /// </summary>
    public enum TipoPrefijo_Documento
    {
        Fijo = 1,
        AreaFuncional = 2,
        Documento = 3,
        Digitado = 4
    }

    /// <summary>
    /// Enumeracion con los tipos de documentos
    /// </summary>
    public enum DocumentoTipo
    {
        DocInterno = 0,
        DocExterno = 1,
        NoFuncional = 2
    }

    /// <summary>
    /// Enumeracion de Entrada/Salida
    /// </summary>
    public enum EntradaSalida
    {
        Entrada = 1,
        Salida = 2,
        Otro = 3
    }

    /// <summary>
    /// Enumeracion para el Tipo de Documento
    /// </summary>
    public enum TipoDocumento
    {
        Sistema = 0,
        Chequeo = 1,
        Llamada = 2,
        Referencia = 3
    }

    /// <summary>
    /// Enumeracion para el Tipo de referencia
    /// </summary>
    public enum TipoReferencia
    {
        Personal = 1,
        Familiar = 2,
        Comercial = 3,
        Interesado = 4
    }

    public enum EstadoTareaIncumplimiento
    {
        Todas = 0,
        Cerradas = 1, 
        Abiertas = 2
    }

    #endregion

    #region Inventarios

    /// <summary>
    /// Enumeracion con el estado de activos e inventarios
    /// </summary>
    public enum EstadoInv
    {
        SinCosto = 0,
        Activo = 1,
        Arrendado = 2,
        Mantenimiento = 3,
        Retirado = 4,
        Vendido = 5
    }

    /// <summary>
    /// Enumeracion con el Tipo de mvto de inventarios
    /// </summary>
    public enum TipoMovimientoInv
    {
        Entradas = 1,
        Salidas = 2,
        Traslados = 3
    }


    /// <summary>
    /// Enumeracion con el Tipo de Costeo de inventarios
    /// </summary>
    public enum TipoCosteoInv
    {
        Promedio = 0,
        PEPS = 1,
        Individual = 2,
        Transaccional = 3,
        SinCosto = 4
    }

    /// <summary>
    /// Enumeracion con el Tipo de Control Especial
    /// </summary>
    public enum ControlEspecial
    {
        NoAplica = 0,
        Kit = 1,
        Consumible = 2,
        Caja = 3,
        Carrete = 4,
        Lote = 5,
        EquiposDotacion = 6
    }

    public enum InventarioFisicoReportType
    {
        Conteo = 0,
        Fisico = 1,
        Diferencia = 2
    }

    /// <summary>
    /// Enumeracion con el Tipo de Bodegas
    /// </summary>
    public enum TipoBodega
    {
        Stock = 0,
        Proyecto = 1,
        Taller = 2,
        Planta = 3,
        Mecanizado = 4,
        Traslado = 5,
        Transito = 6,
        ZonaFranca = 7,
        DespachoNoFacturado= 8,
        Custodia = 9,
        PuertoFOB = 10,
        PuertoDestino = 11,
        Almacen = 12
    }

    /// <summary>
    /// Enumeracion con el Tipo de Bodegas
    /// </summary>
    public enum TipoTransporte
    {
        Aereo = 1,
        Maritimo = 2,
        Terrestre = 3,
        TraficoPostal = 4
    }

    /// <summary>
    /// Enumeracion con el Tipo de traslados
    /// </summary>
    public enum TipoTraslado
    {
        Traslado = 0,
        ConsumoInterno = 1,
        PrestamoProyecto = 2,
        DevolucionProyecto = 3
    }
    #endregion

    #region Nomina
    /// <summary>
    /// Enumeracion para Ultima nomina liquidada
    /// </summary>
    public enum UltimaNomina
    {
        UltimaNomina1=1,
        UltimaNomina2=2
    }

    /// <summary>
    /// Enumeracion para el perido de pago de nomina
    /// </summary>
    public enum PeriodoPago
    {
        PrimeraQuincena = 1,
        SegundaQuincena = 2, //Se usa para el pago mensual
        AmbasQuincenas = 3
    }

    /// <summary>
    /// Enumeracion que indica si la liquidacion se hace quincenal o mensual
    /// </summary>
    public enum PeriodoPagoNomina
    {
        Mensual = 0,
        Quincenal = 1
    }

    /// <summary>
    /// Enumeracion tipo de concepto planilla diaria de trabajo
    /// </summary>
    public enum TipoConceptoPlanillaDiaria
    {
        T = 1,
        D = 2,
        V = 3,
        I = 4,
        S = 5,
        C = 6
    }

    /// <summary>
    /// Enumeracion para determinar el origen de concepto en la nómina
    /// </summary>
    public enum OrigenConcepto
    {
        Automatico = 1,
        Fondo = 2,
        Novedad = 3,
        NovedadContrato = 4,
        Prestamo = 5,
        Planilla = 6,
        CompFlexible = 7,
        Prestacion = 8
    }

    /// <summary>
    /// Enumeracion par determinar el tipo de contrato del empleado
    /// </summary>
    public enum EmpleadoTipoContrato
    {
        SalarioNormal = 1,
        SalarioNormal2 = 2,
        SalarioIntegral = 3,
        SenaLectivo = 4,
        SenaProductivo = 5,
        Pension = 6,
        SustitucionPensinal = 7
    }

    /// <summary>
    /// Enumeración para determinar el tipo de concepto usado
    /// </summary>
    public enum TipoConceptoNOM
    {
        Devengo = 1,
        Deduccion = 2
    }

    /// <summary>
    /// Enumaracion para determinar el tipo de liquidacion usada
    /// </summary>
    public enum TipoLiquidacionNOM
    {
        Automatica = 1,
        Valor = 2,
        Formula = 3
    }

    /// <summary>
    /// Enumeracion para determinar el tipo de liquidacion aplicado
    /// </summary>
    public enum TipoLiquidacion
    {
        N = 1, //Nomina
        P = 2, //Vacaciones
        V = 3, //Prima
        L = 4, //Liquidacion Contrato
        C = 5,  //Cesantias
        Pr = 6, //Provisiones
        Pl = 7 //Planilla 
    }

    /// <summary>
    /// Terceros de la planilla de aportes
    /// </summary>
    public enum TerceroPlanilla
    { 
        FondoSalud  = 1,
        FondePension = 2,
        Caja = 3,
        SENA = 4,
        ICBF = 5,
        ARP = 6       
    }

    /// <summary>
    /// Estado del empleado
    /// </summary>
    public enum EstadoEmpleado
    { 
        Activo = 1,
        Inactivo = 2,
        Liquidado  = 3 
    }

    /// <summary>
    /// Tipo de noveda de contrato
    /// </summary>
    public enum TipoNovedadNomina
    { 
        ING = 1,
        RET = 2,
        TDE = 3,
        TAE = 4,
        TDP = 5,
        TAP = 6,
        VSP = 7,
        VTE = 8,
        VST = 9,
        SLN = 10,
        IGE = 11,
        LMA = 12,
        VAC = 13,
        AVP = 14,
        VCT = 15,
        IRP = 16
    }

    /// <summary>
    /// Causales Retiro Empleado
    /// </summary>
    public enum CausaRetiro
    { 
        Renuncia = 1, 
        DespidoUnilateral = 2, 
        DespidoJustaCausa = 3, 
        TerminacionContrato  = 4, 
        MuerteTrabajador  =5, 
        Jubilacion = 6, 
        SustituciónPatronal = 7, 
        CambioSalarioIntegral = 8
    }

    /// <summary>
    /// Tipo Liquidacion Cesantias
    /// </summary>
    public enum TipoLiqCesantias
    { 
        Anual = 1,
        Parcial = 2
    }

    #endregion

    #region Operaciones Conjutas
    public enum TipoIVA
    {
        MayorValorCashCall=1,
        LineaIndependiente=2,
        DistribuidoPorLinea=3
    }
    #endregion

    #region Planeacion

    /// <summary>
    /// Enumeracion con los tipos de Mvto Presupuesto
    /// </summary>
    public enum TipoMovimientoPresup
    {
        Presupuesto = 1,
        Adicion = 2,
        Traslado = 3,
        Reclasificacion = 4,
        Saldos = 5
    }

    /// <summary>
    /// Enumeracion con los tipos de Transaccion Presupuesto
    /// </summary>
    public enum TipoTransaccionPresup
    {
        Presupuesto = 1,
	    Compromiso = 2,
	    Recibido = 3,
	    Inventario = 4, 
	    Ejecutado = 5,
	    Pagado = 6
    }

    /// <summary>
    /// Enumeracion con los controles de costo
    /// </summary>
    public enum ControlCosto
    {
        Fijo = 1,
        Variable = 2,
        Estacionario = 3
    }


    #endregion

    #region Proveedores

    /// <summary>
    /// Enumeracion con los niveles de Prioridad
    /// </summary>
    public enum Prioridad
    {
        High = 0,
        Medium = 1,
        Low = 2
    }


    /// <summary>
    /// Enumeracion con los destinos
    /// </summary>
    public enum Destino
    {
        OrdenCompra = 0,
        Contrato = 1
    }

    /// <summary>
    /// Enumeracion con parametros de aprobacion de los recibidos
    /// </summary>
    public enum Calificacion
    {
        NoCumple = 0,
        Parcialmente = 1,
        Totalmente = 2,
        SuperaExpect = 3
    }

    /// <summary>
    /// Enumeracion con el Tipo Proveedor
    /// </summary>
    public enum TipoProveedor
    {
        Local = 1,
        Extranjero = 2
    }


    /// <summary>
    /// Enumeracion con el Tipo IncoTerm
    /// </summary>
    public enum Incoterm
    {
          CFR= 1, 
          CIF= 2,
          CIP= 3, 
          CPT= 4, 
          DAF= 5, 
          DDP= 6,
          DDU= 7, 
          DEQ= 8, 
          DES= 9, 
          EXW= 10,
          FAS= 11,
          FCA= 12, 
          FOB= 13 
    }

    /// <summary>
    /// Enumeracion con el Nivel Aprobacion
    /// </summary>
    public enum NivelAprobacionUser
    {
        SeccionDirecta = 0,
        DirectorArea = 1,
        SubDirectorArea = 2,
        DirectorSección = 3,
        DirectorAreaAlterna = 4

    }
    #endregion

    #region Proyectos

     /// <summary>
    /// Enumeracion con los niveles de Prioridad
    /// </summary>
    public enum TipoSolicitud
    {
        Cotizacion = 1,
        Licitacion = 2,
        Garantia = 3,
        Interna = 4,
        Otra = 5,
    }

    /// <summary>
    /// Enumeracion con los tipos de medida
    /// </summary>
    public enum TipoMedida
    {
        PorUnidades = 1,
        Lineal = 2,
        Area = 3,
        Volumen = 4,
        Peso = 5,
        Capacidad = 6,
        Tiempo = 7
    }

    /// <summary>
    /// Enumeracion con los tipos de recurso
    /// </summary>
    public enum TipoRecurso
    {
        Insumo = 1,
        Equipo = 2,
        Personal = 3,
        Transporte = 4,
        Herramienta = 5,
        Software = 6
    }

    /// <summary>
    /// Enumeracion con los tipos de  Presupuesto
    /// </summary>
    public enum TipoPresupuestoProy
    {
        Construccion = 1,
        Otros = 2
    }

    #endregion

}
