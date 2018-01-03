using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Clase con las llaves para los recursos de las tablas
    /// </summary>
    public static class DictionaryTables
    {
        #region Maestras

        #region Modulos

        /// <summary>
        /// "Activos Fijos"
        /// </summary>
        public static string Master_Mod_AC = "tbl_mod_ac";

        /// <summary>
        /// "Aplicacion"
        /// </summary>
        public static string Master_Mod_APL = "tbl_mod_apl";

        /// <summary>
        /// "Cartera"
        /// </summary>
        public static string Master_Mod_CC = "tbl_mod_cc";

        /// <summary>
        /// "Contabilidad"
        /// </summary>
        public static string Master_Mod_CO = "tbl_mod_co";

        /// <summary>
        /// "Cuentas Por Pagar"
        /// </summary>
        public static string Master_Mod_CP = "tbl_mod_cp";

        /// <summary>
        /// "Diferidos"
        /// </summary>
        public static string Master_Mod_DI = "tbl_mod_di";

        /// <summary>
        /// "Facturacion"
        /// </summary>
        public static string Master_Mod_FA = "tbl_mod_fa";

        /// <summary>
        /// "Global"
        /// </summary>
        public static string Master_Mod_GL = "tbl_mod_gl";

        /// <summary>
        /// "Inventarios"
        /// </summary>
        public static string Master_Mod_IN = "tbl_mod_in";

        /// <summary>
        /// "Nomina"
        /// </summary>
        public static string Master_Mod_NO = "tbl_mod_no";

        /// <summary>
        /// "Operaciones"
        /// </summary>
        public static string Master_Mod_OP = "tbl_mod_op";

        /// <summary>
        /// "Operaciones Conjuntas"
        /// </summary>
        public static string Master_Mod_OC = "tbl_mod_oc";

        /// <summary>
        /// "Planeacion"
        /// </summary>
        public static string Master_Mod_PL = "tbl_mod_pl";

        /// <summary>
        /// "Proveedores"
        /// </summary>
        public static string Master_Mod_PR = "tbl_mod_pr";

        /// <summary>
        /// "Seguridad"
        /// </summary>
        public static string Master_Mod_SE = "tbl_mod_se";

        /// <summary>
        /// "Tesoreria"
        /// </summary>
        public static string Master_Mod_TS = "tbl_mod_ts";

        /// <summary>
        /// "Recursos humanos"
        /// </summary>
        public static string Master_Mod_RH = "tbl_mod_rh";

        /// <summary>
        /// "Proyectos"
        /// </summary>
        public static string Master_Mod_PY = "tbl_mod_py";

        #endregion

        #region Periodos

        /// <summary>
        /// "Anual"
        /// </summary>
        public static string PeriodoImp_Anual = "tbl_restr_PeriodoImp_v1";

        /// <summary>
        /// "Semestral"
        /// </summary>
        public static string PeriodoImp_Semestral = "tbl_restr_PeriodoImp_v2";

        /// <summary>
        /// "Trimestral"
        /// </summary>
        public static string PeriodoImp_Trimestral = "tbl_restr_PeriodoImp_v4";

        /// <summary>
        /// "Bimestral"
        /// </summary>
        public static string PeriodoImp_Bimestral = "tbl_restr_PeriodoImp_v6";

        /// <summary>
        /// "Mensual"
        /// </summary>
        public static string PeriodoImp_Mensual = "tbl_restr_PeriodoImp_v12";

        #endregion

        #region Nomina

        /// <summary>
        /// "UltimaNomina1"
        /// </summary>
        public static string UltimaNomina1 = "tbl_restr_UltimaNomina1_v1";

        /// <summary>
        /// "UltimaNomina2"
        /// </summary>
        public static string UltimaNomina2 = "tbl_restr_UltimaNomina2_v2";

        /// <summary>
        /// "Primera Quincena"
        /// </summary>
        public static string Primera_Quincena = "tbl_restr_ComFlexPeriodo_v1";

        /// <summary>
        /// "Segunda Quincena"
        /// </summary>
        public static string Segunda_Quincena = "tbl_restr_ComFlexPeriodo_v2";

        /// <summary>
        /// "Ambas Quincena"
        /// </summary>
        public static string Ambas_Quincena = "tbl_restr_ComFlexPeriodo_v3";

        /// <summary>
        /// Estado Civil
        /// </summary>
        public static string EstadoCivil = "tbl_restr_EstadoCivil_v";

        /// <summary>
        /// Sexo
        /// </summary>
        public static string Sexo = "tbl_restr_Sexo_v";

        /// <summary>
        /// Tipo Contrato 
        /// </summary>
        public static string TipoContrato = "tbl_restr_TipoContrato_v";

        /// <summary>
        /// Estado
        /// </summary>
        public static string Estado = "tbl_restr_EstadoEmp_v";

        /// <summary>
        /// Procedimiento ReteFuente
        /// </summary>
        public static string ProcedimientoRteFte = "tbl_restr_ProcedRteFte_v";

        /// <summary>
        /// Moneda Sueldo
        /// </summary>
        public static string MonedaSueldo = "tbl_restr_MonedaSueldo_v";

        /// <summary>
        /// Periodo Pago
        /// </summary>
        public static string PeriodoPago = "tbl_restr_PeriodoPago_v";

        /// <summary>
        /// Forma Pago
        /// </summary>
        public static string FormaPago = "tbl_restr_FormaPago_v";

        /// <summary>
        /// Tipo Cuenta
        /// </summary>
        public static string TipoCuenta = "tbl_restr_CtaTipo_v";

        /// <summary>
        /// FactorRH
        /// </summary>
        public static string FactorRH = "tbl_restr_RH_v";

        /// <summary>
        /// Pase Categoria
        /// </summary>
        public static string PaseCategoria = "tbl_restr_PaseCategoria_v";

        /// <summary>
        /// Grupo Sanguineo
        /// </summary>
        public static string GrupoSanguineo = "tbl_restr_GrupoSanguineo_v";

        /// <summary>
        /// Termino Contrato
        /// </summary>
        public static string TerminoContrato = "tbl_restr_TerminoContrato_v";

        /// <summary>
        /// CuentaGen
        /// </summary>
        public static string CuentaGen = "tbl_control_CuentaGen";

        /// <summary>
        /// CuentaCosto
        /// </summary>
        public static string CuentaCosto = "tbl_control_CuentaCosto";

        /// <summary>
        /// CuentaEspecial
        /// </summary>
        public static string CuentaEspecial = "tbl_control_CuentaEspecial";

        /// <summary>
        /// ConceptoCargo
        /// </summary>
        public static string PorConceptoCargo = "tbl_control_ConceptoCargo";

        /// <summary>
        /// ActividadEconomica
        /// </summary>
        public static string PorActividadEconomica = "tbl_control_ActividadEconomica";

        #endregion

        #endregion

        #region Documentos

        #region General

        /// <summary>
        /// "Diseñador"
        /// </summary>
        public static string Doc_Diseñador = "tbl_Diseñador";

        /// <summary>
        /// "Xls"
        /// </summary>
        public static string Doc_Xls = "tbl_xls";

        /// <summary>
        /// "Xlsx"
        /// </summary>
        public static string Doc_Xlsx = "tbl_xlsx";

        /// <summary>
        /// "Pdf"
        /// </summary>
        public static string Doc_Pdf = "tbl_pdf";

        /// <summary>
        /// "Pozo"
        /// </summary>
        public static string Doc_Pozo = "tbl_Pozo";

        /// <summary>
        /// "Campo"
        /// </summary>
        public static string Doc_Campo = "tbl_Campo";

        /// <summary>
        /// "Reservas Probables"
        /// </summary>
        public static string Doc_ReservasProbables = "tbl_ReservasProbables";

        /// <summary>
        /// "Reservas Probadas"
        /// </summary>
        public static string Doc_ReservasProbadas = "tbl_ReservasProbadas";

        /// <summary>
        /// Linea Recta
        /// </summary>
        public static string Doc_TipoIVAMayorValor = "tbl_tipoIVA_MayorValorCashCall";

        /// <summary>
        /// Linea Recta
        /// </summary>
        public static string Doc_TipoIVALineaIndependiente = "tbl_tipoIVA_LineaIndependiente";

        /// <summary>
        /// Linea Recta
        /// </summary>
        public static string Doc_TipoIVADistribuidorPorLinea = "tbl_tipoIVA_DistribuidorPorLinea";

        /// <summary>
        /// "No Aplica"
        /// </summary>
        public static string Doc_NoAplica = "tbl_NoAplica";

        /// <summary>
        /// "Todos"
        /// </summary>
        public static string Todos = "tbl_todos";
     
        /// <summary>
        /// "Seleccionados"
        /// </summary>
        public static string Seleccionados = "tbl_seleccionados";

        /// <summary>
        /// "No Seleccionados"
        /// </summary>
        public static string NoSeleccionados = "tbl_noSeleccionados";

        #endregion

        #region Cartera

        /// <summary>
        /// Nuevo
        /// </summary>
        public static string Cartera_Nueva = "tbl_restr_TipoCompra_v1";

        /// <summary>
        /// Refinanciado
        /// </summary>
        public static string Cartera_Refinanciada = "tbl_restr_TipoCompra_v2";

        /// <summary>
        /// Incorpora Liquida
        /// </summary>
        public static string Incorpora_Liquida = "tbl_IncorporaLiquida";

        /// <summary>
        /// Incopora Previa
        /// </summary>
        public static string Incorpora_Previa = "tbl_IncorporaPrevia";

        /// <summary>
        /// Efectivo
        /// </summary>
        public static string Efectivo = "tbl_Efectivo";

        /// <summary>
        /// Nominal
        /// </summary>
        public static string Nominal = "tbl_Nominal";

        /// <summary>
        /// EfectivaAnual
        /// </summary>
        public static string EfectivaAnual = "tbl_EfectivaAnual";

        /// <summary>
        /// NominaMensual
        /// </summary>
        public static string NominaMensual = "tbl_NominaMensual";

        /// <summary>
        /// Solidario
        /// </summary>
        public static string Solidario = "tbl_Solidario";

        /// <summary>
        /// Financiero
        /// </summary>
        public static string Financiero = "tbl_Financiero";

        /// <summary>
        /// Bancario
        /// </summary>
        public static string Bancario = "tbl_Bancario";

        /// <summary>
        /// Adición Digito
        /// </summary>
        public static string Cartera_TipoNovedad_1 = "tbl_tiponovedad_1";

        /// <summary>
        /// Reincorporar
        /// </summary>
        public static string Cartera_TipoNovedad_2 = "tbl_tiponovedad_2";

        /// <summary>
        /// Actualizar
        /// </summary>
        public static string Cartera_TipoNovedad_3 = "tbl_tiponovedad_3";

        /// <summary>
        /// Nuevo descuento
        /// </summary>
        public static string Cartera_TipoNovedad_4 = "tbl_tiponovedad_4";

        /// <summary>
        /// Cambio pagaduria
        /// </summary>
        public static string Cartera_TipoNovedad_5 = "tbl_tiponovedad_5";

        /// <summary>
        /// Desincorporación
        /// </summary>
        public static string Cartera_TipoNovedad_6 = "tbl_tiponovedad_6";

        /// <summary>
        /// Credito Nuevo
        /// </summary>
        public static string Cartera_TipoNovedad_7 = "tbl_tiponovedad_7";

        /// <summary>
        /// Cruce correcto
        /// </summary>
        public static string Cartera_EstadoCruce_1 = "tbl_estadocruce_1";

        /// <summary>
        /// No Opero Inc. Previa
        /// </summary>
        public static string Cartera_EstadoCruce_2 = "tbl_estadocruce_2";

        /// <summary>
        /// No Opero Inc. Liquidacion
        /// </summary>
        public static string Cartera_EstadoCruce_3 = "tbl_estadocruce_3";

        /// <summary>
        /// No Opero Desincorporacion
        /// </summary>
        public static string Cartera_EstadoCruce_4 = "tbl_estadocruce_4";

        /// <summary>
        /// Opero por Valor Diferente
        /// </summary>
        public static string Cartera_EstadoCruce_5 = "tbl_estadocruce_5";

        /// <summary>
        /// Dejo de Operar
        /// </summary>
        public static string Cartera_EstadoCruce_6 = "tbl_estadocruce_6";

        /// <summary>0
        /// Valor diferente
        /// </summary>
        public static string Cartera_EstadoCruce_7 = "tbl_estadocruce_7";

        /// <summary>
        /// Pago Atrasado
        /// </summary>
        public static string Cartera_EstadoCruce_8 = "tbl_estadocruce_8";

        /// <summary>
        /// Desc. Sin saldo
        /// </summary>
        public static string Cartera_EstadoCruce_9 = "tbl_estadocruce_9";

        /// <summary>
        /// Solicitud
        /// </summary>
        public static string Cartera_EstadoCruce_10 = "tbl_estadocruce_10";

        /// <summary>
        /// Opero Adelantado
        /// </summary>
        public static string Cartera_EstadoCruce_11 = "tbl_estadocruce_11";

        /// <summary>
        /// Afiliaciones
        /// </summary>
        public static string Cartera_TipoIncorporacionArchivos_1 = "tbl_TipoIncorporaArchivos_1";

        /// <summary>
        /// Desafiliaciones
        /// </summary>
        public static string Cartera_TipoIncorporacionArchivos_2 = "tbl_TipoIncorporaArchivos_2";

        /// <summary>
        /// Incorporaciones
        /// </summary>
        public static string Cartera_TipoIncorporacionArchivos_3 = "tbl_TipoIncorporaArchivos_3";

        /// <summary>
        /// Desincorporaciones
        /// </summary>
        public static string Cartera_TipoIncorporacionArchivos_4 = "tbl_TipoIncorporaArchivos_4";

        /// <summary>
        /// Giro / Ajuste
        /// </summary>
        public static string Cartera_TipoReintegros_1 = "tbl_tiporeintegros_1";

        /// <summary>
        /// Ajuste Cartera
        /// </summary>
        public static string Cartera_TipoReintegros_2 = "tbl_tiporeintegros_2";

        #endregion

        #region Facturacion

        /// <summary>
        /// "Facturas por Pagar"
        /// </summary>
        public static string Doc_FacturasxPagar = "tbl_FacturasXPagar";

        /// <summary>
        /// "Facturas Pagadas"
        /// </summary>
        public static string Doc_FacturasPagadas = "tbl_FacturasPagadas";

        /// <summary>
        /// "Factura Venta"
        /// </summary>
        public static string Doc_FacturaVenta = "tbl_FacVenta";

        /// <summary>
        /// "Nota Credito"
        /// </summary>
        public static string Doc_NotaCredito = "tbl_NotasCredito";
        #endregion

        #region Proveedores
        /// <summary>
        /// "Alta"
        /// </summary>
        public static string Doc_PriorityHigh = "tbl_priority_high";

        /// <summary>
        /// "Media"
        /// </summary>
        public static string Doc_PriorityMedium = "tbl_priority_medium";

        /// <summary>
        /// "Baja"
        /// </summary>
        public static string Doc_PriorityLow = "tbl_priority_low";

        /// <summary>
        /// "Orden de Compra"
        /// </summary>
        public static string Doc_DestinoOrdenCompra = "tbl_destino_ordenCompra";

        /// <summary>
        /// "Contrato"
        /// </summary>
        public static string Doc_DestinoContrato = "tbl_destino_contrato";

        /// <summary>
        /// "Inconterm CFR"
        /// </summary>
        public static string Doc_IncotermCFR = "tbl_IncotermCFR";

        /// <summary>
        /// "Inconterm CIF"
        /// </summary>
        public static string Doc_IncotermCIF = "tbl_IncotermCIF";

        /// <summary>
        /// "Inconterm CIP"
        /// </summary>
        public static string Doc_IncotermCIP = "tbl_IncotermCIP";

        /// <summary>
        /// "Inconterm CPT"
        /// </summary>
        public static string Doc_IncotermCPT = "tbl_IncotermCPT";

        /// <summary>
        /// "Inconterm DAF"
        /// </summary>
        public static string Doc_IncotermDAF = "tbl_IncotermDAF";

        /// <summary>
        /// "Inconterm DDP"
        /// </summary>
        public static string Doc_IncotermDDP = "tbl_IncotermDDP";

        /// <summary>
        /// "Inconterm DDU"
        /// </summary>
        public static string Doc_IncotermDDU = "tbl_IncotermDDU";

        /// <summary>
        /// "Inconterm DEQ"
        /// </summary>
        public static string Doc_IncotermDEQ = "tbl_IncotermDEQ";

        /// <summary>
        /// "Inconterm DES"
        /// </summary>
        public static string Doc_IncotermDES = "tbl_IncotermDES";

        /// <summary>
        /// "Inconterm EXW"
        /// </summary>
        public static string Doc_IncotermEXW = "tbl_IncotermEXW";

        /// <summary>
        /// "Inconterm FAS"
        /// </summary>
        public static string Doc_IncotermFAS = "tbl_IncotermFAS";

        /// <summary>
        /// "Inconterm FCA"
        /// </summary>
        public static string Doc_IncotermFCA = "tbl_IncotermFCA";

        /// <summary>
        /// "Inconterm FOB"
        /// </summary>
        public static string Doc_IncotermFOB = "tbl_IncotermFOB";

        /// <summary>
        /// "Otrosí"
        /// </summary>
        public static string Doc_Otrosi = "tbl_Otrosi";

        /// <summary>
        /// "Adiciona"
        /// </summary>
        public static string Doc_Adiciona = "tbl_Adiciona";

        /// <summary>
        /// "Reemplaza"
        /// </summary>
        public static string Doc_Reemplaza = "tbl_Reemplaza";
        #endregion

        #region Proyectos

        /// <summary>
        /// "Cotización"
        /// </summary>
        public static string Doc_Cotizacion = "tbl_Cotizacion";

        /// <summary>
        /// "Licitación"
        /// </summary>
        public static string Doc_Licitacion = "tbl_Licitacion";

        /// <summary>
        /// "Garantía"
        /// </summary>
        public static string Doc_Garantia = "tbl_Garantia";

        /// <summary>
        /// "Interna"
        /// </summary>
        public static string Doc_Interna = "tbl_Interna";

        /// <summary>
        /// "Otra"
        /// </summary>
        public static string Doc_Otra = "tbl_Otra";

        #endregion

        #region Inventarios

        /// <summary>
        /// "Aereo"
        /// </summary>
        public static string Doc_TransporteAereo = "tbl_TransporteAereo";

        /// <summary>
        /// "Maritimo"
        /// </summary>
        public static string Doc_TransporteMaritimo = "tbl_TransporteMaritimo";

        /// <summary>
        /// "Terrestre"
        /// </summary>
        public static string Doc_TransporteTerrestre = "tbl_TransporteTerrestre";

        /// <summary>
        /// "Trafico Postal"
        /// </summary>
        public static string Doc_TransporteTraficoPostal = "tbl_TransporteTraficoPostal";

        /// <summary>
        /// "Todos"
        /// </summary>
        public static string Doc_EstadoInvCosto0 = "tbl_estadoInv_Costo0";

        /// <summary>
        /// "Nuevo"
        /// </summary>
        public static string Doc_EstadoInvNuevo = "tbl_estadoInv_Nuevo";

        /// <summary>
        /// "Estado 2"
        /// </summary>
        public static string Doc_EstadoInvEstado2 = "tbl_estadoInv_Estado2";

        /// <summary>
        /// "Estado 3"
        /// </summary>
        public static string Doc_EstadoInvEstado3 = "tbl_estadoInv_Estado3";

        /// <summary>
        /// "Estado 4"
        /// </summary>
        public static string Doc_EstadoInvEstado4 = "tbl_estadoInv_Estado4";

        /// <summary>
        /// "ModalidadImportacion 1: Imp. Ordinaria"
        /// </summary>
        public static string Doc_ModalidadImportacion1 = "tbl_modalidadImportacion1";

        /// <summary>
        /// "ModalidadImportacion 2: Imp. Franquicia"
        /// </summary>
        public static string Doc_ModalidadImportacion2 = "tbl_modalidadImportacion2";

        /// <summary>
        /// "ModalidadImportacion 3: Reimp. X Perfeccionamiento"
        /// </summary>
        public static string Doc_ModalidadImportacion3 = "tbl_modalidadImportacion3";

        /// <summary>
        /// "ModalidadImportacion 4: Reimp. Mismo estado"
        /// </summary>
        public static string Doc_ModalidadImportacion4 = "tbl_modalidadImportacion4";

        /// <summary>
        /// "ModalidadImportacion 5: Imp. X Garantia"
        /// </summary>
        public static string Doc_ModalidadImportacion5 = "tbl_modalidadImportacion5";

        /// <summary>
        /// "ModalidadImportacion 6: Imp. Temporal Corto Plazo"
        /// </summary>
        public static string Doc_ModalidadImportacion6 = "tbl_modalidadImportacion6";

        /// <summary>
        /// "ModalidadImportacion 7: Imp. Temporal Largo Plazo"
        /// </summary>
        public static string Doc_ModalidadImportacion7 = "tbl_modalidadImportacion7";

        /// <summary>
        /// "ModalidadImportacion 8:  Imp. Temporal para Perfeccionamiento"
        /// </summary>
        public static string Doc_ModalidadImportacion8 = "tbl_modalidadImportacion8";

        /// <summary>
        /// "ModalidadImportacion 9: Imp. Para Transformacion"
        /// </summary>
        public static string Doc_ModalidadImportacion9 = "tbl_modalidadImportacion9";

        /// <summary>
        /// "ModalidadImportacion 10: Trafico Postal y Envios Urgentes por Avion"
        /// </summary>
        public static string Doc_ModalidadImportacion10 = "tbl_modalidadImportacion10";

        /// <summary>
        /// "ModalidadImportacion 11: Entrega Urgente"
        /// </summary>
        public static string Doc_ModalidadImportacion11 = "tbl_modalidadImportacion11";

        /// <summary>
        /// "Deterioro Inventarios"
        /// </summary>
        public static string Doc_DeterioroInv = "tbl_DeterioroInv";

        /// <summary>
        /// "Revalorizacion Inventarios"
        /// </summary>
        public static string Doc_RevalorizacionInv = "tbl_RevalorizacionInv";

        #endregion

        #region Planeacion

        /// <summary>
        /// Doc_CentroCosto
        /// </summary>
        public static string Doc_CentroCosto = "tbl_pl_CentroCosto";

        /// <summary>
        /// Doc_Proyectos
        /// </summary>
        public static string Doc_Proyectos = "tbl_pl_Proyectos";

        /// <summary>
        /// Doc_LineaPresu
        /// </summary>
        public static string Doc_LineaPresu = "tbl_pl_LineaPresu";

        /// <summary>
        /// Doc_ConcepCargo
        /// </summary>
        public static string Doc_ConcepCargo = "tbl_pl_ConcepCargo";

        /// <summary>
        /// Doc_Local
        /// </summary>
        public static string Doc_Local = "tbl_pl_Local";

        /// <summary>
        /// Doc_Extranjero
        /// </summary>
        public static string Doc_Extranjero = "tbl_pl_Extranjero";

        /// <summary>
        /// Presupuesto
        /// </summary>
        public static string Doc_TipoInforme_Presupuesto = "tbl_pl_TipoInforme_Presupuesto";

        /// <summary>
        /// Ejecucion
        /// </summary>
        public static string Doc_TipoInforme_Ejecucion = "tbl_pl_TipoInforme_Ejecucion";

        /// <summary>
        /// Por Ejecucion
        /// </summary>
        public static string Doc_Estado_PorSolicitar = "tbl_pl_Estado_PorSolicitar";

        /// <summary>
        /// Por Revisar
        /// </summary>
        public static string Doc_Estado_PorRevisar = "tbl_pl_Estado_PorRevisar";

        /// <summary>
        /// Por Aprobar
        /// </summary>
        public static string Doc_Estado_PorAprobar = "tbl_pl_Estado_PorAprobar";

        /// <summary>
        /// Aprobada
        /// </summary>
        public static string Doc_Estado_Aprobada = "tbl_pl_Estado_Aprobada";

        /// <summary>
        /// Negada
        /// </summary>
        public static string Doc_Estado_Negada = "tbl_pl_Estado_Negada";

        #endregion

        #region Activos
        /// <summary>
        /// Doc_AñosDosmil
        /// </summary>
        public static string Doc_AñosDosmil = "tbl_ac_2000";

        /// <summary>
        /// Doc_AñosDosMilUno
        /// </summary>
        public static string Doc_AñosDosMilUno = "tbl_ac_2001";

        /// <summary>
        /// Doc_AñosDosMilDos
        /// </summary>
        public static string Doc_AñosDosMilDos = "tbl_ac_2002";

        /// <summary>
        /// Doc_AñosDosMilTres
        /// </summary>
        public static string Doc_AñosDosMilTres = "tbl_ac_2003";

        /// <summary>
        /// Doc_AñosDosMilCuatro
        /// </summary>
        public static string Doc_AñosDosMilCuatro = "tbl_ac_2004";

        /// <summary>
        /// Doc_AñosDosmilCinco
        /// </summary>
        public static string Doc_AñosDosmilCinco = "tbl_ac_2005";

        /// <summary>
        /// Doc_AñosDosmilSeis
        /// </summary>
        public static string Doc_AñosDosmilSeis = "tbl_ac_2006";

        /// <summary>
        /// Doc_AñosDosmilSiete
        /// </summary>
        public static string Doc_AñosDosmilSiete = "tbl_ac_2007";

        /// <summary>
        /// Doc_AñosDosmilOcho
        /// </summary>
        public static string Doc_AñosDosmilOcho = "tbl_ac_2008";

        /// <summary>
        /// Doc_AñosDosmilNueve
        /// </summary>
        public static string Doc_AñosDosmilNueve = "tbl_ac_2009";

        /// <summary>
        /// Doc_AñosDosmilDiez
        /// </summary>
        public static string Doc_AñosDosmilDiez = "tbl_ac_2010";

        /// <summary>
        /// Doc_AñosDosmilOnce
        /// </summary>
        public static string Doc_AñosDosmilOnce = "tbl_ac_2011";

        /// <summary>
        /// Doc_AñosDosmilDoce
        /// </summary>
        public static string Doc_AñosDosmilDoce = "tbl_ac_2012";

        /// <summary>
        /// Doc_AñosDosmilTrece
        /// </summary>
        public static string Doc_AñosDosmilTrece = "tbl_ac_2013";

        /// <summary>
        /// Doc_AñosDosmilCatorce
        /// </summary>
        public static string Doc_AñosDosmilCatorce = "tbl_ac_2014";

        /// <summary>
        /// Doc_AñosDosmilQuince
        /// </summary>
        public static string Doc_AñosDosmilQuince = "tbl_ac_2015";

        /// <summary>
        /// Doc_AñosDosmilDieciseis
        /// </summary>
        public static string Doc_AñosDosmilDieciseis = "tbl_ac_2016";

        /// <summary>
        /// Doc_AñosDosmilDiecisiete
        /// </summary>
        public static string Doc_AñosDosmilDiecisiete = "tbl_ac_2017";

        /// <summary>
        /// Doc_AñosDosmilDieciocho
        /// </summary>
        public static string Doc_AñosDosmilDieciocho = "tbl_ac_20018";

        /// <summary>
        /// Linea Recta
        /// </summary>
        public static string Doc_TipoDepreciacionLineaRecta = "tbl_tipoDepre_LineaRecta";

        /// <summary>
        /// Saldos Decrecientes
        /// </summary>
        public static string Doc_TipoDepreciacionSaldosDecrecientes = "tbl_tipoDepre_SaldosDecrecientes";

        /// <summary>
        /// Unidades de Produccion
        /// </summary>
        public static string Doc_TipoDepreciacionUnidadesProduccion = "tbl_tipoDepre_UnidadesDeProduccion";

        /// <summary>
        /// traslado de activos
        /// </summary>
        public static string Doc_Traslado = "tbl_ac_Traslado";

        /// <summary>
        /// Mantenimiento
        /// </summary>
        public static string Doc_Mantenimiento = "tbl_ac_Mantenimiento";

        /// <summary>
        /// Asigancion de responsable
        /// </summary>
        public static string Doc_AsignacionDeResponsable = "tbl_ac_AsignacionDeResponsable";

        #endregion

        #region Cuentas x Pagar

        /// <summary>
        /// "Factura"
        /// </summary>
        public static string Doc_Factura = "tbl_factura";

        /// <summary>
        /// "Devolver"
        /// </summary>
        public static string Doc_Devolver = "tbl_devolver_factura";

        /// <summary>
        /// "Radicar"
        /// </summary>
        public static string Doc_Radicar = "tbl_radicar_factura";

        /// <summary>
        /// "Solicitado"
        /// </summary>
        public static string Doc_EstateSolicitado = "tbl_estate_solicitado";

        /// <summary>
        /// "Supervisado"
        /// </summary>
        public static string Doc_EstateSupervisado = "tbl_estate_supervisado";

        /// <summary>
        /// "Aprobado"
        /// </summary>
        public static string Doc_EstatePreAprobado = "tbl_estate_preAprobado";
        #endregion

        #region Global
        /// <summary>
        /// "Mda Local"
        /// </summary>
        public static string Doc_CurrencyLocal = "tbl_currency_local";

        /// <summary>
        /// "Mda Extranjera"
        /// </summary>
        public static string Doc_CurrencyForeign = "tbl_currency_foreign";

        /// <summary>
        /// "Ambas"
        /// </summary>
        public static string Doc_CurrencyBoth = "tbl_currency_both";

        /// <summary>
        /// "Nacional"
        /// </summary>
        public static string Doc_TipoViajeNacional = "tbl_tp_nacional";

        /// <summary>
        /// "Exterior"
        /// </summary>
        public static string Doc_TipoViajeExterior = "tbl_tp_exterior";

        /// <summary>
        /// "Cerrado"
        /// </summary>
        public static string Doc_EstateCerrado = "tbl_estate_cerrado";

        /// <summary>
        /// "Anulado"
        /// </summary>
        public static string Doc_EstateAnulado = "tbl_estate_anulado";

        /// <summary>
        /// "SinAprobar"
        /// </summary>
        public static string Doc_EstateSinAprobar = "tbl_estate_sinaprobar";

        /// <summary>
        /// "ParaAprobacion"
        /// </summary>
        public static string Doc_EstateParaAprobacion = "tbl_estate_paraaprobacion";

        /// <summary>
        /// "Aprobado"
        /// </summary>
        public static string Doc_EstateAprobado = "tbl_estate_aprobado";

        /// <summary>
        /// "Revertido"
        /// </summary>
        public static string Doc_EstateRevertido = "tbl_estate_revertido";

        /// <summary>
        /// "Devuelto"
        /// </summary>
        public static string Doc_EstateDevuelto = "tbl_estate_devuelto";

        /// <summary>
        /// "Radicado"
        /// </summary>
        public static string Doc_EstateRadicado = "tbl_estate_radicado";

        /// <summary>
        /// "Revisado"
        /// </summary>
        public static string Doc_EstateRevisado = "tbl_estate_revisado";

        /// <summary>
        /// "Contabilizado"
        /// </summary>
        public static string Doc_EstateContabilizado = "tbl_estate_contabilizado";

        /// <summary>
        /// "Cancelado"
        /// </summary>
        public static string Doc_EstateCancelado = "tbl_estate_cancelado";

        #endregion

        #region Contabilidad
        /// <summary>
        /// Cuenta
        /// </summary>
        public static string Doc_Cuenta = "tbl_coSaldoControl_Cuenta";

        /// <summary>
        /// Doc Interno
        /// </summary>
        public static string Doc_DocInterno = "tbl_coSaldoControl_Doc_Interno";

        /// <summary>
        /// Doc Externo
        /// </summary>
        public static string Doc_DocExterno = "tbl_coSaldoControl_Doc_Externo";

        /// <summary>
        /// Componente Tercero
        /// </summary>
        public static string Doc_ComponenteTercero = "tbl_coSaldoControl_ComponenteTercero";

        /// <summary>
        /// Componente Activo
        /// </summary>
        public static string Doc_ComponenteActivo = "tbl_coSaldoControl_ComponenteActivo";

        /// <summary>
        /// Componente Documento
        /// </summary>
        public static string Doc_ComponenteDocumento = "tbl_coSaldoControl_ComponenteDocumento";

        /// <summary>
        /// Inventario
        /// </summary>
        public static string Doc_Inventario = "tbl_coSaldoControl_Inventario";

        /// <summary>
        /// Local
        /// </summary>
        public static string Doc_MdaOrigenLocal = "tbl_restr_MdaOrigen_v1";

        /// <summary>
        /// Extranjera
        /// </summary>
        public static string Doc_MdaOrigenExtr = "tbl_restr_MdaOrigen_v2";

        #endregion

        #endregion

        #region Reportes

        #region Cartera

        /// <summary>
        /// "Formato 19"
        /// </summary>
        public static string Rpt_FormatoF19 = "tbl_FormatoF19";

        /// <summary>
        /// "Formato 21"
        /// </summary>
        public static string Rpt_FormatoF21 = "tbl_FormatoF21";

        /// <summary>
        /// "Formato 25"
        /// </summary>
        public static string Rpt_FormatoF25 = "tbl_FormatoF25";

        /// <summary>
        /// "Formato 47"
        /// </summary>
        public static string Rpt_FormatoF47 = "tbl_FormatoF47";

        /// <summary>
        /// "Formato 49"
        /// </summary>
        public static string Rpt_FormatoF49 = "tbl_FormatoF49";

        /// <summary>
        /// "Creditos"
        /// </summary>
        public static string Rpt_Creditos = "tbl_Creditos";

        /// <summary>
        /// "Total Recaudos"
        /// </summary>
        public static string Rpt_TotalRecaudos = "tbl_TotalRecaudos";

        /// <summary>
        /// "Recaudos Masivos"
        /// </summary>
        public static string Rpt_RecaudosMasivos = "tbl_RecaudosMasivos";

        /// <summary>
        /// "Recaudos Manuales"
        /// </summary>
        public static string Rpt_RecaudosManuales = "tbl_RecaudosManuales";

        /// <summary>
        /// "Pagos Total"
        /// </summary>
        public static string Rpt_PagosTotal = "tbl_PagosTotal";

        /// <summary>
        /// "Ajustes Cartera"
        /// </summary>
        public static string Rpt_AjustesCartera = "tbl_AjustesCartera";

        /// <summary>
        /// "Refinanciacion"
        /// </summary>
        public static string Rpt_Refinanciacion = "tbl_Refinanciacion";

        /// <summary>
        /// "Saldos"
        /// </summary>
        public static string Rpt_Saldos = "tbl_Saldos";

        /// <summary>
        /// "Cuota"
        /// </summary>
        public static string Rpt_Cuota = "tbl_Cuota";

        /// <summary>
        /// "Cliente"
        /// </summary>
        public static string Rpt_Cliente = "tbl_Cliente";

        /// <summary>
        /// "Concesionario"
        /// </summary>
        public static string Rpt_Concesionario = "tbl_Concesionario";

        /// <summary>
        /// "Asesor"
        /// </summary>
        public static string Rpt_Asesor = "tbl_Asesor";

        /// <summary>
        /// "Linea de Credito"
        /// </summary>
        public static string Rpt_LineaCredito = "tbl_LineaCredito";

        /// <summary>
        /// "Comprador Cartera"
        /// </summary>
        public static string Rpt_CompradorCart = "tbl_CompradorCartera";

        /// <summary>
        /// "Vencimiento 5 Días"
        /// </summary>
        public static string Rpt_Vencimiento5Dias = "tbl_Vencimiento5Dias";

        /// <summary>
        /// "Proyección Pagos"
        /// </summary>
        public static string Rpt_ProyeccionPagos = "tbl_ProyeccionPagos";

        /// <summary>
        /// "Estado Cuenta Cesion"
        /// </summary>
        public static string Rpt_EstadoCuentaCesion = "tbl_EstadoCuentaCesion";

        /// <summary>
        /// "Amortizacion Derechos"
        /// </summary>
        public static string Rpt_AmortizacionDer = "tbl_AmortizacionDer";

        /// <summary>
        /// "Cesion Mes"
        /// </summary>
        public static string Rpt_CesionMes = "tbl_CesionMes";

        /// <summary>
        /// "Recompra Mes"
        /// </summary>
        public static string Rpt_RecompraMes = "tbl_RecompraMes";

        /// <summary>
        /// "Principal"
        /// </summary>
        public static string Rpt_CJPrincipal = "tbl_CJPrincipal";

        /// <summary>
        /// "Adicional"
        /// </summary>
        public static string Rpt_CJAdicional = "tbl_CJAdicional";

        /// <summary>
        /// "Juzgado"
        /// </summary>
        public static string Rpt_CJJuzgado = "tbl_CJJuzgado";

        /// <summary>
        /// "Total"
        /// </summary>
        public static string Rpt_CJTotal = "tbl_CJTotal";

        /// <summary>
        /// "Formato Especial"
        /// </summary>
        public static string Rpt_FormatoEspecial = "tbl_FormatoEspecial";
        #endregion

        #region Planeacion

        /// <summary>
        /// "Proyecto por Actividad"
        /// </summary>
        public static string Rpt_ProyectoActividad = "tbl_ProyectoActividad";

        /// <summary>
        /// "Lineas por Recurso"
        /// </summary>
        public static string Rpt_LineasRecurso = "tbl_LineasRecurso";

        /// <summary>
        /// "Recurso por Actividad"
        /// </summary>
        public static string Rpt_RecursoActividad = "tbl_RecursoActividad";

        /// <summary>
        /// "Lineas por Centro de Costo"
        /// </summary>
        public static string Rpt_LineaCentroCost = "tbl_LineaCentroCost";

        /// <summary>
        /// "Capex"
        /// </summary>
        public static string Rpt_Capex = "tbl_Capex";

        /// <summary>
        /// "Opex"
        /// </summary>
        public static string Rpt_Opex = "tbl_Opex";

        /// <summary>
        /// "Inversión"
        /// </summary>
        public static string Rpt_Inversion = "tbl_Inversion";

        /// <summary>
        /// "Administrativo"
        /// </summary>
        public static string Rpt_Administrativo = "tbl_Administrativo";

        /// <summary>
        /// "Inventarios"
        /// </summary>
        public static string Rpt_Inventarios = "tbl_Inventarios";

        /// <summary>
        /// "Capital de Trabajo"
        /// </summary>
        public static string Rpt_CapTrabajo = "tbl_CapTrabajo";

        /// <summary>
        /// "Comercial"
        /// </summary>
        public static string Rpt_Comercial = "tbl_Comercial";

        /// <summary>
        /// "Distribución"
        /// </summary>
        public static string Rpt_Distribucion = "tbl_Distribucion";

        /// <summary>
        /// "Sin Iniciar"
        /// </summary>
        public static string Rpt_SinIniciar = "tbl_SinIniciar";

        /// <summary>
        /// "En Desarrollo"
        /// </summary>
        public static string Rpt_EnDesarrollo = "tbl_EnDesarrollo";

        /// <summary>
        /// "Por Modena"
        /// </summary>
        public static string Rpt_PorMoneda = "tbl_PorMoneda";

        /// <summary>
        /// "Por Origen"
        /// </summary>
        public static string Rpt_PorOrigen = "tbl_PorOrigen";

        #endregion

        #region Proveedores

        /// <summary>
        /// "Facturado"
        /// </summary>
        public static string Rpt_Facturado = "tbl_Facturado";

        /// <summary>
        /// "No Facturado"
        /// </summary>
        public static string Rpt_NoFacturado = "tbl_NoFacturado";

        #endregion

        #region General

        /// <summary>
        /// "Control Comprobantes"
        /// </summary>
        public static string Rpt_Control = "tbl_Control";

        /// <summary>
        /// "Cuenta"
        /// </summary>
        public static string Rpt_Account = "tbl_account";

        /// <summary>
        /// "Anticipos Pendientes"
        /// </summary>
        public static string Rpt_AnticiposPend = "tbl_AnticiposPend";

        /// <summary>
        /// "Bodega"
        /// </summary>
        public static string Rpt_Bodega = "tbl_Bodega";

        /// <summary>
        /// "Funcional"
        /// </summary>
        public static string Rpt_Funcional = "tbl_Funcional";

        /// <summary>
        /// "Funcional"
        /// </summary>
        public static string Rpt_IFRS = "tbl_IFRS";

        /// <summary>
        /// Causadas
        /// </summary>
        public static string Rpt_Causadas = "tbl_Causadas";

        /// <summary>
        /// "Cedula"
        /// </summary>
        public static string Rpt_Cedula = "tbl_Cedula";

        /// <summary>
        /// "CentroCosto"
        /// </summary>
        public static string Rpt_CentroCosto = "tbl_centroCosto";

        /// <summary>
        /// "Certificado"
        /// </summary>
        public static string Rpt_Certificate = "tbl_Certificate";

        /// <summary>
        /// "Comparativo"
        /// </summary>
        public static string Rpt_Comparativo = "tbl_comparativo";

        /// <summary>
        /// "Con Saldo Inicial"
        /// </summary>
        public static string Rpt_ConSaldoInicial = "tbl_conSaldoInicial";

        /// <summary>
        /// "Consecutivo"
        /// </summary>
        public static string Rpt_Consecutivo = "tbl_consecutivo";

        /// <summary>
        /// "Consolidado"
        /// </summary>
        public static string Rpt_Consolidated = "tbl_consolidated";

        /// <summary>
        /// "Cuenta Funcional"
        /// </summary>
        public static string Rpt_CuentaFunc = "tbl_cuentaFunc";

        /// <summary>
        /// "Cuenta Alterna"
        /// </summary>
        public static string Rpt_CuentaAlt = "tbl_cuentaAlt";

        /// <summary>
        /// "Tasas de cierre"
        /// </summary>
        public static string Rpt_DeCierre = "tbl_DeCierre";

        /// <summary>
        /// "Tasas diarias"
        /// </summary>
        public static string Rpt_Diarias = "tbl_Diarias";

        /// <summary>
        /// "Declaracion"
        /// </summary>
        public static string Rpt_Declaracion = "tbl_Declaración";

        /// <summary>
        /// "Detallado"
        /// </summary>
        public static string Rpt_Detailed = "tbl_detailed";

        /// <summary>
        /// "Devueltos"
        /// </summary>
        public static string Rpt_Devueltos = "tbl_devueltos";

        /// <summary>
        /// "Documento"
        /// </summary>
        public static string Rpt_Documento = "tbl_documento";

        /// <summary>
        /// "Facturas por Pagar"
        /// </summary>
        public static string Rpt_FacturasXPagar = "tbl_FacturasXPagar";

        /// <summary>
        /// "Flujo Semanal"
        /// </summary>
        public static string Rpt_FlujoSemanal = "tbl_FlujoSemanal";

        /// <summary>
        /// "Balance General"
        /// </summary>
        public static string Rpt_General = "tbl_general";

        /// <summary>
        /// "ICAs"
        /// </summary>
        public static string Rpt_ICA = "tbl_ICA";

        /// <summary>
        /// "IVAs"
        /// </summary>
        public static string Rpt_IVA = "tbl_IVA";

        /// <summary>
        /// "LineaPresupuesto"
        /// </summary>
        public static string Rpt_LineaPresupuesto = "tbl_lineaPresupuesto";

        /// <summary>
        /// "Balance de Prueba Por Meses"
        /// </summary>
        public static string Rpt_Months = "tbl_months";

        /// <summary>
        /// "Balance de Prueba"
        /// </summary>
        public static string Rpt_dePrueba = "tbl_dePrueba";

        /// <summary>
        /// "Proyecto"
        /// </summary>
        public static string Rpt_Proyecto = "tbl_proyecto";

        /// <summary>
        /// Por Causar
        /// </summary>
        public static string Rpt_PorCausar = "tbl_PorCausar";

        /// <summary>
        /// "Por meses"
        /// </summary>
        public static string Rpt_PorMeses = "tbl_porMeses";

        /// <summary>
        /// "Referencia"
        /// </summary>
        public static string Rpt_Referencia = "tbl_Referencia";

        /// <summary>
        /// "Recibo de Caja"
        /// </summary>
        public static string Rpt_ReciboCaja = "tbl_reciboCaja";

        /// <summary>
        /// "Balance de Prueba Por Trimestres"
        /// </summary>
        public static string Rpt_Quarters = "tbl_quarters";

        /// <summary>
        /// "Resumido"
        /// </summary>
        public static string Rpt_Summarized = "tbl_summarized";

        /// <summary>
        /// "Retefuente"
        /// </summary>
        public static string Rpt_Retefuente = "tbl_Retefuente";

        /// <summary>
        /// "ReteICAs"
        /// </summary>
        public static string Rpt_ReteICA = "tbl_ReteICA";

        /// <summary>
        /// "Renta"
        /// </summary>
        public static string Rpt_Renta = "tbl_Renta";

        /// <summary>
        /// "Imp. Consumo"
        /// </summary>
        public static string Rpt_ImpConsumo = "tbl_ImpConsumo";

        /// <summary>
        /// "Sin Saldo Inicial"
        /// </summary>
        public static string Rpt_SinSaldoInicial = "tbl_sinSaldoInicial";

        /// <summary>
        /// "Soporte"
        /// </summary>
        public static string Rpt_Soporte = "tbl_Soporte";

        /// <summary>
        /// "Tercero"
        /// </summary>
        public static string Rpt_Tercero = "tbl_tercero";

        /// <summary>
        /// "Fecha Vencimiento"
        /// </summary>
        public static string Rpt_FechaVencimiento = "tbl_FechaVencimiento";

        /// <summary>
        /// "Tipo Referencia"
        /// </summary>
        public static string Rpt_TipoReferencia = "tbl_tipoReferencia";

        /// <summary>
        ///"Comprobante"
        /// </summary>
        public static string Rpt_Voucher = "tbl_voucher";

        /// <summary>
        ///"Comprobante"
        /// </summary>
        public static string Rpt_Name = "tbl_name";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Nomina = "tbl_nomina";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Vacaciones = "tbl_vacaciones";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Liquidacion = "tbl_liquidacion";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Prima = "tbl_prima";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Cesantias = "tbl_cesantias";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Prenomina = "tbl_prenomina";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Todos = "tbl_todos";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Det_Empleado_Concepto = "tbl_det.Empleado_Concepto";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Det_Concepto_Empleado = "tbl_det.Concepto_Empleado";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Resumido_X_Concepto = "tbl_resumido_X_Concepto";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Resumido_X_Empleado = "tbl_resumido_X_Empleado";

        /// <summary>
        /// Nomina detalle
        /// </summary>
        public static string Rpt_Resumido_X_Otros = "tbl_resumido_X_Otros";

        /// <summary>
        /// Pension Solidaridad
        /// </summary>
        public static string Rpt_PensionSolidaridad = "tbl_pensionSolidaridad";

        /// <summary>
        /// Salud
        /// </summary>
        public static string Rpt_Salud = "tbl_Salud";

        /// <summary>
        /// Aportes Voluntarios Pension
        /// </summary>
        public static string Rpt_AporVoluntariosPen = "tbl_AporVoluntariosPen";

        /// <summary>
        ///ARP
        /// </summary>
        public static string Rpt_Arp = "tbl_Arp";

        /// <summary>
        /// Cajas
        /// </summary>
        public static string Rpt_Cajas = "tbl_Cajas";

        /// <summary>
        /// Gasto Empresa
        /// </summary>
        public static string Rpt_GastoEmpresa = "tbl_GastoEmpresa";

        /// <summary>
        /// Vacaciones Pagadas
        /// </summary>
        public static string Rpt_VacacionesPagadas = "tbl_VacacionesPagadas";

        /// <summary>
        /// Vacaciones pendientes
        /// </summary>
        public static string Rpt_VacacionesPendientes = "tbl_VacacionesPendientes";

        /// <summary>
        /// Documento Liquidación
        /// </summary>
        public static string Rpt_DocumentoLiquidacion = "tbl_documentoLiquidacion";

        /// <summary>
        /// Relacion Liquidaciòn
        /// </summary>
        public static string Rpt_relacionLiquidacion = "tbl_relacionLiquidacion";

        /// <summary>
        /// Relacion Prestamos
        /// </summary>
        public static string Rpt_RelacionPendientes = "tbl_RelacionPendientes";

        /// <summary>
        /// Afiliaciones de Cartera
        /// </summary>
        public static string Rpt_AfiliacionCartera = "tbl_AfiliacionesCartera";

        /// <summary>
        /// Aseguradora
        /// </summary>
        public static string Rpt_Aseguradora = "tbl_Aseguradora";

        /// <summary>
        /// Contrato Trabajador
        /// </summary>
        public static string Rpt_ContratoTrabajador = "tbl_contratoTrabajador";

        /// <summary>
        /// Ingresos y Retenciones
        /// </summary>
        public static string Rpt_IngresosRetenciones = "tbl_ingresosRetenciones";

        /// <summary>
        /// Certificado Laboral Historico
        /// </summary>
        public static string Rpt_CertificadoLaboralHistorico = "tbl_certificadoLaboralHistorico";

        /// <summary>
        /// Certificado Laboral
        /// </summary>
        public static string Rpt_CertificadoLaboral = "tbl_certificadoLaboral";

        /// <summary>
        /// certificado Salud
        /// </summary>
        public static string Rpt_CertificadoSalud = "tbl_certificadoSalud";

        /// <summary>
        /// Boleta Pago
        /// </summary>
        public static string Rpt_BoletaPago = "tbl_BoletaPago";

        /// <summary>
        /// Saldos de Cartera
        /// </summary>
        public static string Rpt_SaldosCartera = "tbl_saldosCartera";

        /// <summary>
        /// Saldos a Favor
        /// </summary>
        public static string Rpt_SaldosAFavor = "tbl_saldosAFavor";

        /// <summary>
        /// Saldos en Mora
        /// </summary>
        public static string Rpt_SaldosMora = "tbl_saldosMora";

        /// <summary>
        /// Aportes de Cliente
        /// </summary>
        public static string Rpt_AportesCliente = "tbl_aportesCliente";

        /// <summary>
        /// Cartera Propia
        /// </summary>
        public static string Rpt_CarteraPropia = "tbl_carteraPropia";

        /// <summary>
        /// Cartera Cedida
        /// </summary>
        public static string Rpt_CarteraCedida = "tbl_carteraCedida";

        /// <summary>
        /// Cartera Toda
        /// </summary>
        public static string Rpt_CarteraToda = "tbl_carteraToda";

        /// <summary>
        /// Estado de Cuenta
        /// </summary>
        public static string Rpt_EstadoCuenta = "tbl_estadoCuenta";

        /// <summary>
        /// Beneficiario
        /// </summary>
        public static string Rpt_Beneficiario = "tbl_beneficiario";

        /// <summary>
        /// Banco
        /// </summary>
        public static string Rpt_Banco = "tbl_banco";

        /// <summary>
        /// Numero de Cheque
        /// </summary>
        public static string Rpt_ChequeNumero = "tbl_chequeNumero";

        /// <summary>
        /// Detallado
        /// </summary>
        public static string Rpt_Detallado = "tbl_detallado";

        /// <summary>
        /// Resumido
        /// </summary>
        public static string Rpt_Resumido = "tbl_resumido";

        /// <summary>
        /// Sin Costos
        /// </summary>
        public static string Rpt_SinCostos = "tbl_sinCostos";

        /// <summary>
        /// Con Costos
        /// </summary>
        public static string Rpt_ConCostos = "tbl_conCostos";

        /// <summary>
        /// Nit
        /// </summary>
        public static string Rpt_Nit = "tbl_Nit";

        /// <summary>
        /// Cantidad
        /// </summary>
        public static string Rpt_Cantidad = "tbl_Cantidad";

        /// <summary>
        /// Valor Nominal
        /// </summary>
        public static string Rpt_VlrNominal = "tbl_VlrNominal";

        /// <summary>
        /// Valor Capital
        /// </summary>
        public static string Rpt_VlrCapital = "tbl_VlrCapital";

        /// <summary>
        /// Valor Giro
        /// </summary>
        public static string Rpt_VlrGiro = "tbl_VlrGiro";

        /// <summary>
        /// Paz y Salvo
        /// </summary>
        public static string Rpt_PazYSalvo = "tbl_pazSalvo";

        /// <summary>
        /// Certificado de Deuda
        /// </summary>
        public static string Rpt_CertificadoDeuda = "tbl_certificadoDeuda";

        /// <summary>
        /// Paz y Salvo
        /// </summary>
        public static string Rpt_CertificadoPagosAlDia = "tbl_certificadoPagosAlDia";

        /// <summary>
        /// Certificado de Deuda
        /// </summary>
        public static string Rpt_CertificadoRelacionPagos = "tbl_certificadoRelacionPagos";

        /// <summary>
        /// Diario
        /// </summary>
        public static string Rpt_Diario = "tbl_diario";

        /// <summary>
        /// Mensual
        /// </summary>
        public static string Rpt_Mensual = "tbl_Mensual";

        /// <summary>
        /// Libro Local
        /// </summary>
        public static string Rpt_LibroLocal = "tbl_LibroLocal";

        /// <summary>
        /// Libro IFRS
        /// </summary>
        public static string Rpt_LibroIFRS = "tbl_LibroIFRS";

        /// <summary>
        /// Libranzas
        /// </summary>
        public static string Rpt_Libranzas = "tbl_Libranzas";

        /// <summary>
        /// Libranzas
        /// </summary>
        public static string Rpt_Referenciacion = "tbl_Referenciacion";

        /// <summary>
        /// En Edición
        /// </summary>
        public static string Rpt_Edicion = "tbl_Edicion";

        /// <summary>
        /// Pre Aprobadas
        /// </summary>
        public static string Rpt_PreAprobada = "tbl_PreaAprobada";

        /// <summary>
        /// Aprobadas
        /// </summary>
        public static string Rpt_Aprobadas = "tbl_Aprobadas";

        /// <summary>
        /// En Tramite
        /// </summary>
        public static string Rpt_EnTramite = "tbl_Entramite";

        /// <summary>
        /// Cumplidas
        /// </summary>
        public static string Rpt_Cumplidas = "tbl_Cumplidas";

        /// <summary>
        /// Con Saldos
        /// </summary>
        public static string Rpt_Saldo = "tbl_Saldo";

        /// <summary>
        /// Con Movimientos
        /// </summary>
        public static string Rpt_Movimientos = "tbl_Movimientos";

        /// <summary>
        /// Solicitudes
        /// </summary>
        public static string Rpt_Solicitudes = "tbl_Solicitudes";

        /// <summary>
        /// Orden Compra
        /// </summary>
        public static string Rpt_OrdenCompra = "tbl_OrdenCompra";

        /// <summary>
        /// Recibidos
        /// </summary>
        public static string Rpt_Recibidos = "tbl_Recibidos";

        /// <summary>
        /// Todas
        /// </summary>
        public static string Rpt_All = "tbl_Todas";

        /// <summary>
        /// Anuladas
        /// </summary>
        public static string Rpt_Annulled = "tbl_Anuladas";

        /// <summary>
        /// Plantilla Excel
        /// </summary>
        public static string Rpt_Plantilla = "tbl_Plantilla";

        /// <summary>
        /// Local
        /// </summary>
        public static string Rpt_Local = "tbl_Local";

        /// <summary>
        /// Extranjera
        /// </summary>
        public static string Rpt_Extranjera = "tbl_Extranjera";

        /// <summary>
        /// Ambas
        /// </summary>
        public static string Rpt_Ambas = "tbl_Ambas";

        /// <summary>
        /// Acumulado
        /// </summary>
        public static string Rpt_Acumulado = "tbl_Acumulado";

        /// <summary>
        /// Tabla de Amortizacion
        /// </summary>
        public static string Rpt_TablaAmortizacion = "tbl_TablaAmortizacion";

        /// <summary>
        /// Descriptivo de los Componentes del Pivot de la Tabla Amortización
        /// </summary>
        public static string Rpt_DescripcionComponentes = "tbl_DescripcionComponentes";

        /// <summary>
        /// Pagare
        /// </summary>
        public static string Rpt_Pagare = "tbl_Pagare";

        /// <summary>
        /// Formato Especial ISS
        /// </summary>
        public static string Rpt_FormatoISS = "tbl_FormatoISS";

        /// <summary>
        /// Reporte (CF) Prejuridico
        /// </summary>
        public static string Rpt_Prejuridico = "tbl_Prejuridico";

        /// <summary>
        /// Filtro Vacio de la fecha
        /// </summary>
        public static string Rpt_filtroFecha = "tbl_filtroFecha";

        #endregion

        #endregion
    }
}

