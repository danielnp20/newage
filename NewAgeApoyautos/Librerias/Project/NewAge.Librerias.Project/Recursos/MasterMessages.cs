using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con las llaves para el diccionario
    /// </summary>
    public static class MasterMessages
    {
        #region Activos Fijos

        #region acGrupo

        /// <summary>
        ///  "El Tipo de Activo solo puede tener valores entre 0 y 2"
        /// </summary>
        public static string Ac_Grupo_TipoAct_Restr = "msg_ac_Grupo_TipoAct_Restr";

        /// <summary>
        ///  "El Tipo de Depreciacion Local solo puede tener valores entre 0 y 2"
        /// </summary>
        public static string Ac_Grupo_TipoDepreLOC_Restr = "msg_ac_Grupo_TipoDepreLOC_Restr";

        /// <summary>
        ///  "El Tipo de Depreciacion Extranjera solo puede tener valores entre 0 y 2"
        /// </summary>
        public static string Ac_Grupo_TipoDepreEXT_Restr = "msg_ac_Grupo_TipoDepreEXT_Restr";

        #endregion

        #region acComponente

        /// <summary>
        ///  "El Tipo de Componente solo puede tener valores entre 1 y 9"
        /// </summary>
        public static string Ac_Componente_TipoComponente_Restr = "msg_ac_Componente_TipoComponente_Restr";

        /// <summary>
        ///  "El concepto de saldo de la cuenta no corresopnde a Activos Fijos"
        /// </summary>
        public static string Ac_Componente_CoSaldoCtrl_Restr = "msg_ac_Componente_CoSaldoCtrl_Restr";

        #endregion

        #endregion

        #region Componentes de Cartera
        #region ccCliente
        /// <summary>
        /// El cliente no es mayor de edad, no coincide la fecha del documento de expedicion con la fecha de nacimiento
        /// </summary>
        public static string Cc_Cliente_ServicioTipo_ValidDate = "msg_cc_Cliente_ServicioTipo_ValidDate";

        /// <summary>
        /// El cliente no tiene créditos
        /// </summary>
        public static string Cc_Cliente_NoCredit = "msg_cc_Cliente_NoCredit";

        /// <summary>
        /// El cliente no ha cancelado totalmente el crédito.
        /// </summary>
        public static string Cc_Cliente_NoPayedCredit = "msg_cc_Cliente_NoPayedCredit";

        /// <summary>
        /// En mora.
        /// </summary>
        public static string Cc_Cliente_BehindCredit = "msg_cc_Cliente_BehindCredit";

        /// <summary>
        /// Al día.
        /// </summary>
        public static string Cc_Cliente_Updated = "msg_cc_Cliente_Updated";

        #endregion
        #region ccComponenteCuenta
        /// <summary>
        ///  "El componente {0}, debe tener una cuenta de balance asociada"
        /// </summary>
        public static string Cc_Componente_EmptyCta = "msg_cc_Componente_EmptyCta";
        /// <summary>
        ///  "El concepto de saldo de la cuenta no corresponde a Cartera"
        /// </summary>
        public static string Cc_Componente_CoSaldoCtrl_Restr = "msg_cc_Componente_CoSaldoCtrl_Restr";
        #endregion
        #region ccComponenteCartera
        /// <summary>
        /// Verifica que la cuenta pertenezca al concepto de cartera
        /// </summary>
        public static string Cc_ComponenteCartera_ServicioTipo_ValidDate = "msg_cc_ComponenteCartera_ServicioTipo_ValidDate";
        #endregion
        #region ccCliente Estado Civil
        /// <summary>
        /// Si el estado Civil es no Aplica Se debe Digitar otro estado
        /// </summary>
        public static string Cc_Cliente_ServicioTipo_EstadoCivil = "msg_cc_Cliente_ServicioTipo_EstadoCivil";
        #endregion
        #region ccCliente Cabeza de Famlia
        /// <summary>
        /// Verifica que la mujer sea ama cabeza de familia
        /// </summary>
        public static string Cc_Cliente_ServicioTipo_HeadFamily = "msg_cc_Cliente_ServicioTipo_HeadFamily";
        #endregion
        #endregion

        #region Contabilidad
        #region coActEconomica
        /// <summary>
        /// "El Tipo de Servicio solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Co_ActEconomica_ServicioTipo_Restr = "msg_co_ActEconomica_ServicioTipo_Restr";
        #endregion
        #region coBalanceReclasifica
        /// <summary>
        /// "Agrupación de Tercero solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_BalanceReclasifica_AgrupaTercero_Restr = "msg_co_BalanceReclasifica_AgrupaTercero_Restr";
        #endregion
        #region coCuentaGrupo
        /// <summary>
        /// "El dato consolidado solo puede tener valores entre 1 y 5"
        /// </summary>
        public static string Co_CtaGrupo_ConsolidaPor_Restr = "msg_co_CtaGrupo_ConsolidaPor_Restr";

        /// <summary>
        /// "El tipo de cuenta solo puede tener valores entre 1 y 6"
        /// </summary>
        public static string Co_CtaGrupo_TipoCuenta_Restr = "msg_Co_CtaGrupo_TipoCuenta_Restr";

        #endregion
        #region coConceptoCargo
        /// <summary>
        /// "El Tipo de Concepto solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_ConceptoCargo_TipoConcepto_Restr = "msg_co_ConceptoCargo_TipoConcepto_Restr";
        /// <summary>
        /// "Bien Servicio debe ser 1 Cuando Tipo Concepto se encuentre entre 3,4,5,6,7 o 8"
        /// </summary>
        public static string Co_ConceptoCargo_BienServicio_Restr = "msg_co_ConceptoCargo_BienServicio_Restr";
        #endregion
        #region coRegimenFiscal
        /// <summary>
        /// "El tipo de tercero solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_RegimenFiscal_TipoTercero_Restr = "msg_co_RegimenFiscal_TipoTercero_Restr";
        #endregion
        #region coImpuesto
        /// <summary>
        /// "El regimen fiscal de la empresa no corresponde con el de la tabla de control: {0}"
        /// </summary>
        public static string Co_Impuesto_RegFisEmp_NotCompatible = "msg_co_Impuesto_RegFisEmp_NotCompatible";

        /// <summary>
        /// 'Para el tipo de impuesto nacional el lugar geográfico debe ser: {0} - {1}'
        /// </summary>
        public static string Co_ImpuestoTipo_LugarGeo_ImpNal = "msg_co_ImpuestoTipo_LugarGeo_ImpNal";
        #endregion
        #region coImpuestoTipo
        /// <summary>
        /// "El Periodo solo puede tener valores entre 1 y 6"
        /// </summary>
        public static string Co_ImpTipo_Periodo_Restr = "msg_co_ImpTipo_Periodo_Restr";

        /// <summary>
        /// "El Tipo de Liquidación solo puede tener valores 1 ó 2"
        /// </summary>
        public static string Co_ImpTipo_CausaPago_Restr = "msg_co_ImpTipo_CausaPago_Restr";

        /// <summary>
        /// "El Alcance de Impuesto solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Co_ImpTipo_ImpAlcance_Restr = "msg_co_ImpTipo_ImpAlcance_Restr";

        /// <summary>
        /// "El Pago o Liquidación Aprox solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_ImpTipo_PagoLiqAprox_Restr = "msg_co_ImpTipo_PagoLiqAprox_Restr";

        /// <summary>
        /// "La Clase de Impuesto solo puede tener valores entre 1 y 5"
        /// </summary>
        public static string Co_ImpTipo_ImpClase_Restr = "msg_co_ImpTipo_ImpClase_Restr";

        /// <summary>
        /// "El tipo solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_ImpTipo_Tipo_Restr = "msg_co_ImpTipo_Tipo_Restr";
        #endregion
        #region coPlanCuenta
        /// <summary>
        /// "El tipo solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Co_PlanCta_Tipo_Restr = "msg_co_PlanCta_Tipo_Restr";

        /// <summary>
        /// "Naturaleza solo puede tener valores entre 1 y 2"
        /// </summary>
        public static string Co_PlanCta_Natural_Restr = "msg_co_PlanCta_Natural_Restr";

        /// <summary>
        /// "El Origen Monetario solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_PlanCta_OrigenMon_Restr = "msg_co_PlanCta_OrigenMon_Restr";

        /// <summary>
        /// "Si el Tipo de Impuesto tiene valor, el Porcentaje de Impuesto y Monto mínimo no pueden ir vacios"
        /// </summary>
        public static string Co_PlanCta_ImpPorc_NotEmpty = "msg_co_PlanCta_ImpPorc_NotEmpty";

        /// <summary>
        /// "No puede haber Porcentaje de Impuesto ni monto mínimo si no existe Tipo de Impuesto"
        /// </summary>
        public static string Co_PlanCta_ImpTipo_NotExist = "msg_co_PlanCta_ImpTipo_NotExist";

        /// <summary>
        /// "Asegurese de tener un Tipo de Impuesto asignado"
        /// </summary>
        public static string Co_PlanCta_ImpPor_CheckImp = "msg_co_PlanCta_ImpPor_CheckImp";

        /// <summary>
        /// "Está seguro de borrar o cambiar el Tipo de Impuesto?"
        /// </summary>
        public static string Co_PlanCta_ImpTipo_ChangeSure = "msg_co_PlanCta_ImpTipo_ChangeSure";

        /// <summary>
        /// "El tipo debe pertenecer a la misma jerarquía del padre"
        /// </summary>
        public static string Co_PlanCta_Tipo_HierarCheck = "msg_co_PlanCta_Tipo_HierarCheck";

        /// <summary>
        ///  "Cuando el Saldo de Control del Concepto Saldo es Doc Interno(2) o Doc Externo(3), el indicador de Tercero debe estar activado."
        /// </summary>
        public static string Co_PlanCta_ConceptoSaldo_CheckValue = "msg_co_PlanCta_ConceptoSaldo_CheckValue";

        /// <summary>
        ///  "Cuando el Saldo Control de ese concepto de Saldo es 4 el indicador de terceros saldos debe estar activo"
        /// </summary>
        public static string Co_PlanCta_ConceptoSaldo_SdoCtrl_CheckValue = "msg_co_PlanCta_ConceptoSaldo_SdoCtrl_CheckValue";

        /// <summary>
        /// "El módulo de Operaciones Conjuntas no está activo"
        /// </summary>
        public static string Co_PlanCta_CtaGrupoOc_ActiveModule = "msg_co_PlanCta_CtaGrupoOc_ActiveModule";

        /// <summary>
        /// "Todos los indicadores de Ajustes en Cambio deben ser {0}"
        /// </summary>
        public static string Co_PlanCta_AjCambioInd_ValidValue = "msg_co_PlanCta_AjCambioInd_ValidValue";

        /// <summary>
        /// "El Indicador de Ajuste {0} debe corresponder a: {1}")
        /// </summary>
        public static string Co_PlanCta_AjCambioInd_OnlyValue = "msg_co_PlanCta_AjCambioInd_OnlyValue";

        /// <summary>
        ///  "El indicador de tercero solo debe estar activo si hay Tipo de impuesto"
        /// </summary>
        public static string Co_PlanCta_Tercero_ActiveOnly = "msg_co_PlanCta_Tercero_ActiveOnly";

        /// <summary>
        ///  'El valor del Impuesto Porcentaje debe ser mayor a 0 y menor de 1000.'
        /// </summary>
        public static string Co_PlanCta_ImpPorc_ValueLimit = "msg_co_PlanCta_ImpPorc_ValueLimit";

        /// <summary>
        ///  'El valor del Monto Mínimo del impuesto debe ser mayor o igual a  0'
        /// </summary>
        public static string Co_PlanCta_MontoMinimo_ValueLimit = "msg_co_PlanCta_MontoMinimo_ValueLimit";

        /// <summary>
        ///  'El valor del Monto Mínimo debe ser mayor o igual a 0'
        /// </summary>
        public static string Co_PlanCta_MontoMinimo_Value = "msg_co_PlanCta_MontoMinimo_Value";

        /// <summary>
        ///  "La cuenta de Cierre Anual no puede estar vacía"
        /// </summary>
        public static string Co_PlanCta_CtaCierreAn_NotEmpty = "msg_co_PlanCta_CtaCierreAn_NotEmpty";

        /// <summary>
        ///  "El NIT de Cierre Anual no puede estar vacío"
        /// </summary>
        public static string Co_PlanCta_NITCierreAn_NotEmpty = "msg_co_PlanCta_NITCierreAn_NotEmpty";

        /// <summary>
        ///  "La máscara del grupo cuenta seleccionado no corresponde a ninguna longitud de nivel de ésta tabla"
        /// </summary>
        public static string Co_PlanCta_CuentaGrupo_InvalidMask = "msg_co_PlanCta_CuentaGrupo_InvalidMask";

        /// <summary>
        ///  "La cuenta tiene un módulo diferente al del documento contable {0}"
        /// </summary>
        public static string Co_PlanCta_ConceptoSaldo_ModuleInvalid = "msg_co_PlanCta_ConceptoSaldo_ModuleInvalid";

        /// <summary>
        ///  "Cuando la cuenta es de Costo, debe activar el indicador de Proyecto o Centro de Costo"
        /// </summary>
        public static string Co_PlanCta_ProyectoOrCtoCostoUnchecked = "msg_co_PlanCta_ProyectoOrCtoCostoUnchecked";

        #endregion
        #region coProyecto

        /// <summary>
        /// "El Tipo de Proyecto solo puede tener valores entre 1 y 7"
        /// </summary>
        public static string Co_Proyecto_ProyTipo_Restr = "msg_co_Proyecto_ProyTipo_Restr";

        /// <summary>
        /// "La Fecha de Cierre no puede ser inferior a la Fecha de Apertura"
        /// </summary>
        public static string Co_Proyecto_FCierre_NotLess = "msg_co_Proyecto_FCierre_NotLess";

        /// <summary>
        /// "La Fecha de Apertura no puede ser superior a la Fecha de Cierre"
        /// </summary>
        public static string Co_Proyecto_FApertura_NotSuperior = "msg_co_Proyecto_FApertura_NotSuperior";

        /// <summary>
        /// "El valor de los días estimados debe ser mayor o igual a  0"
        /// </summary>
        public static string Co_Proyecto_DiasEst_DayLimit = "msg_co_Proyecto_DiasEst_DayLimit";

        /// <summary>
        /// "El Tipo Comercial solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Co_Proyecto_TipoComercial_Restr = "msg_co_Proyecto_TipoComercial_Restr";

        #endregion
        #region coPlanillaConsolidacion
        /// <summary>
        /// "La Empresa ya fue asignada"
        /// </summary>
        public static string Co_PlanillaCons_Company_Assigned = "msg_co_PlanillaCons_Company_Assigned";

        #region coPlanillaConsolidacion
        /// <summary>
        /// "El Centro de costo ya fue asignado"
        /// </summary>
        public static string Co_PlanillaCons_CentroCosto_Assigned = "msg_co_PlanillaCons_CentroCosto_Assigned";
        #endregion

        #region coPlanillaConsolidacion
        /// <summary>
        /// "La Empresa Consolidada no puede ser igual a la Empresa Corporativa"
        /// </summary>
        public static string Co_PlanillaCons_EmpresaGrupo_Assigned = "msg_co_PlanillaCons_EmpresaGrupo_Assigned";
        #endregion
        #endregion
        #region coTercero
        /// <summary>
        /// "El tipo de cuenta solo puede tener valores 1 ó 2."
        /// </summary>
        public static string Co_Tercero_CuentaTipo_Restr = "msg_co_Tercero_CuentaTipo_Restr";

        /// <summary>
        /// "El Dígito de Verificación no es correcto, compruebe su NIT o código."
        /// </summary>
        public static string Co_Tercero_DigVerif_Incorrect = "msg_co_Tercero_DigVerif_Incorrect";

        /// <summary>
        /// "El dígito de Verificacion del Tercero {1}, se ha cambiado por {2}"
        /// </summary>
        public static string Co_Tercero_DigVerif_Change = "msg_co_Tercero_DigVerif_Change";

        /// <summary>
        /// "Unicamente puede ingresar números en el código."
        /// </summary>
        public static string Co_Tercero_ID_OnlyNumber = "msg_co_Tercero_ID_OnlyNumber";

        /// <summary>
        /// "Se requiere el Tipo de Cuenta y/o el Nro de Cuenta para el banco {0}."
        /// </summary>
        public static string Co_Tercero_CtaTipo_AccountRequired = "msg_co_Tercero_CtaTipo_AccountRequired";

        /// <summary>
        /// "El tipo de régimen fiscal del tercero no puede declarar IVA"
        /// </summary>
        public static string Co_Tercero_DeclaraIVAInd_Invalid = "msg_co_Tercero_DeclaraIVAInd_Invalid";

        /// <summary>
        /// "Se requiere el banco 1 para el Tipo de Cuenta {0} y/o Nro de Cuenta {1} enviados."
        /// </summary>
        public static string Co_Tercero_BancoID1_BanKRequired = "msg_co_Tercero_BancoID1_BanKRequired";

        /// <summary>
        /// "Se requiere el banco 2 para el Tipo de Cuenta {0} y/o Nro de Cuenta {1} enviados."
        /// </summary>
        public static string Co_Tercero_BancoID2_BanKRequired = "msg_co_Tercero_BancoID2_BanKRequired";

        /// <summary>
        /// "La descripción {0} debe coincidir con los Nombres y Apellidos enviados."
        /// </summary>
        public static string Co_Tercero_Descriptivo_NotCompatible = "msg_co_Tercero_Descriptivo_NotCompatible";

        /// <summary>
        /// "La descripción no puede ir vacía si existen Nombres y Apellidos."
        /// </summary>
        public static string Co_Tercero_Descriptivo_NotEmpty = "msg_co_Tercero_Descriptivo_NotEmpty";

        /// <summary>
        /// "Se requiere el primer nombre y/o el primer apellido."
        /// </summary>
        public static string Co_Tercero_NombreApellidoPri_Required = "msg_co_Tercero_ApellidoNombrePri_Required";

        /// <summary>
        /// "La longitud de la descripcion es demasiado larga, verifique los nombres  y apellidos"
        /// </summary>
        public static string Co_Tercero_Descriptivo_MaxLength = "msg_co_Tercero_Descriptivo_MaxLength";

        /// <summary>
        ///  "Se requiere Razón social(Primer Apellido)"
        /// </summary>
        public static string Co_Tercero_ApellidoPriRazon_Required = "msg_co_Tercero_ApellidoPriRazon_Required";

        /// <summary>
        /// 'No puede ingresar nombres ni segundo apellido si el Tipo de Documento indica que no es persona natural'
        /// </summary>
        public static string Co_Tercero_TerceroDocID_NotNaturalPerson = "msg_co_Tercero_TerceroDocID_NotNaturalPerson";

        /// <summary>
        /// "El formato del Correo Electrónico corporativo es incorrecto"
        /// </summary>
        public static string Co_Tercero_CECorporativo_EmailInvalidFormat = "msg_co_Tercero_CECorporativo_EmailInvalidFormat";

        /// <summary>
        /// "El formato de Correo Electrónico del Representante Legal es incorrecto"
        /// </summary>
        public static string Co_Tercero_RepLegalCE_EmailInvalidFormat = "msg_co_Tercero_RepLegalCE_EmailInvalidFormat";

        /// <summary>
        /// No tiene el indicador (nombre del indicador) habilitado para actualizar terceros"
        /// </summary>
        public static string Co_Tercero_RespTercero_InvalidResp = "msg_co_Tercero_RespTercero_InvalidResp";

        #endregion
        #region coValIVA
        /// <summary>
        /// "Si la cuenta ReteIVA está vacia, no puede ingresar cuenta de costo ReteIVA"
        /// </summary>
        public static string Co_ValIVA_ReteIVA_Empty = "msg_co_ValIVA_ReteIVA_Empty";

        /// <summary>
        /// "Si la cuenta ReteIVA tiene valor, la cuenta de costo ReteIVA no puede ir vacia"
        /// </summary>
        public static string Co_ValIVA_ReteIVA_NotEmpty = "msg_co_ValIVA_ReteIVA_NotEmpty";
        #endregion
        #region coImpuestoDeclaracion

        /// <summary>
        ///  "El periodo de declaración debe ser 1 ó 2 ó 4 ó 6 ó 12"
        /// </summary>
        public static string Co_ImpuestoDec_PeriodoDec_Restr = "msg_co_ImpuestoDec_PeriodoDec_Restr";

        /// <summary>
        ///  "El Impuesto de Alcance debe estar entre 1 y 4"
        /// </summary>
        public static string Co_ImpuestoDec_ImpuestoAlc_Restr = "msg_co_ImpuestoDec_ImpuestoAlc_Restr";

        /// <summary>
        ///  "El periodo de declaración debe estar entre 1 y 6"
        /// </summary>
        public static string Co_ImpuestoDec_PagoAprox_Restr = "msg_co_ImpuestoDec_PagoAprox_Restr";

        /// <summary>
        ///  "El lugar Geográfico {0} no es válido para Impuestos nacionales"
        /// </summary>
        public static string Co_ImpuestoDec_LugarGeoInvalid = "msg_co_ImpuestoDec_LugarGeoInvalid";


        #endregion
        #region coImpuestoFormato

        /// <summary>
        ///  "El Tipo de formato debe estar entre 1 y 2"
        /// </summary>
        public static string Co_ImpuestoFormato_Tipo_Restr = "msg_co_ImpuestoFormato_Tipo_Restr";

        /// <summary>
        ///  "El Tipo Fiscal debe estar entre 1 y 5"
        /// </summary>
        public static string Co_ImpuestoFormato_TipoFiscal_Restr = "msg_co_ImpuestoFormato_TipoFiscal_Restr";

        #endregion
        #region coImpDeclaracionCuenta

        /// <summary>
        ///  "La cuenta seleccionada no declara Impuestos"
        /// </summary>
        public static string Co_ImpDeclCuenta_CuentaID_Invalid = "msg_co_ImpDeclCuenta_CuentaInvalid";

        /// <summary>
        /// Todas las Cuentas deben tener NITCierreanual
        /// </summary>
        public static string Co_ImpDeclCuenta_NITCierreAnual_Empty = "msg_co_ImpDeclCuenta_NITCierreAnual_Empty";

        #endregion
        #region coImpDeclaracionCalendario

        /// <summary>
        ///  "El ano fiscal debe estar entre 1990-2999"
        /// </summary>
        public static string Co_ImpDeclCalendario_AñoFiscal_Invalid = "msg_co_ImpDeclCalendario_AñoFiscal_Invalid";

        #endregion
        #region coImpDeclaracionRenglon

        /// <summary>
        ///  "El Signo debe estar entre 1 y 3"
        /// </summary>
        public static string Co_ImpDeclaracionRenglon_SignoSuma_Restr = "msg_co_ImpDeclaracionRenglon_SignoSuma_Restr";

        #endregion
        #region coReporteLinea

        /// <summary>
        ///  "El saldo de movimiento debe estar entre 1 y 2"
        /// </summary>
        public static string Co_ReporteLinea_SaldoMvto_Restr = "msg_co_ReporteLinea_SaldoMvto_Restr";

        #endregion
        #region coOperacion
        /// <summary>
        /// "El Tipo de Operación solo puede tener valores entre 1 y 7"
        /// </summary>
        public static string Co_Operacion_TipoOpe_Restr = "msg_co_Operacion_TipoOpe_Restr";

        /// <summary>
        /// "El Tipo de Operación solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Co_Operacion_TipoOpe_RestrO = "msg_co_Operacion_TipoOpe_RestrO";

        /// <summary>
        ///  "El Tipo Fiscal solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Co_Operacion_TipoFiscal_Restr = "msg_co_Operacion_TipoFiscal_Restr";


        #endregion
        #region coComprobante

        /// <summary>
        ///  "El Tipo de consecutivo debe estar entre 1 y 3"
        /// </summary>
        public static string Co_Comprobante_TipoConsec_Restr = "msg_co_Comprobante_TipoConsec_Restr";

        /// <summary>
        ///  "El indicador de Multimoneda no está activo, por tanto no puede activar Bimoneda"
        /// </summary>
        public static string Co_Comprobante_BimonedaNotActive = "msg_co_Comprobante_bimonedaNotActive";

        /// <summary>
        ///  "El comprobante de anulacion debe incluir el mismo Libro del comprobante actual"
        /// </summary>
        public static string Co_Comprobante_LibroCompAnulInvalid = "msg_co_Comprobante_LibroCompAnulInvalid";


        #endregion
        #region coDocumento

        /// <summary>
        ///  "La cuenta debe tener Origen Monetario Local(1)"
        /// </summary>
        public static string Co_coDocumento_cuentaLOC_OrigenMonet = "msg_co_CoDocumento_cuentaLOC_OrigenMonet";

        /// <summary>
        ///  "La cuenta debe tener Origen Monetario Extranjero(2)"
        /// </summary>
        public static string Co_coDocumento_cuentaEXT_OrigenMonet = "msg_co_CoDocumento_cuentaEXT_OrigenMonet";

        /// <summary>
        ///  "No Puede ingresar un documento con un prefijo ya existente"
        /// </summary>
        public static string Co_coDocumento_prefijo = "msg_co_CoDocumento_prefijo_Restr";

        /// <summary>
        ///  "Cuando el Documento es Documento Contable(12), la Cuenta debe tener el control de Concepto Saldo en Doc Externo(3)"
        /// </summary>
        public static string Co_coDocumento_cuenta = "msg_co_CoDocumento_cuenta_Restr";

        /// <summary>
        ///  "El Tipo de comprobante solo puede tener valores entre 1 y 2"
        /// </summary>
        public static string Co_coDocumento_TipoCompr_Restr = "msg_co_coDocumento_TipoCompr_Restr";

        /// <summary>
        ///  "Ese Comprobante esta siendo usado en el auxiliar preliminar"
        /// </summary>
        public static string Co_coDocumento_Comprobante_Restr = "msg_co_CoDocumento_Comprobante_Restr";

        /// <summary>
        ///  "La moneda origen solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Co_coDocumento_MonedaOrigen_Restr = "msg_co_CoDocumento_MonedaOrigen_Restr";

        /// <summary>
        ///  "El módulo del Concepto Saldo de la Cuenta Mda Local no corresponde al módulo del documento actual"
        /// </summary>
        public static string Co_coDocumento_CuentaLOC_ModuleInvalid = "msg_co_CoDocumento_CuentaLOC_ModuleInvalid";

        /// <summary>
        ///  "El concepto de saldo de la cuenta local o extranjera debe tener en el control de saldo 3 cuando el documento es 21 0 26"
        /// </summary>
        public static string Co_coDocumento_DocumentoID_DocumentInvalid_LocYExt3 = "msg_co_CoDocumento_DocumentoID_DocumentInvalid_LocYExt3";

        /// <summary>
        ///  "El concepto de saldo de la cuenta local o extranjera debe tener en el control de saldo 2 cuando el documento es 41 0 42"
        /// </summary>
        public static string Co_coDocumento_DocumentoID_DocumentInvalid_LocYExt2 = "msg_co_CoDocumento_DocumentoID_DocumentInvalid_LocYExt2";

        /// <summary>
        ///  "El módulo del Concepto Saldo de la Cuenta Mda Extranjera no corresponde al módulo del documento actual"
        /// </summary>
        public static string Co_coDocumento_CuentaEXT_ModuleInvalid = "msg_co_CoDocumento_CuentaEXT_ModuleInvalid";

        /// <summary>
        ///  "Debe existir una cuenta local para la moneda origen"
        /// </summary>
        public static string Co_coDocumento_CuentaLOC_Empty = "msg_co_coDocumento_CuentaLOC_Empty";

        /// <summary>
        ///  "Debe existir una cuenta extranjera para la moneda origen"
        /// </summary>
        public static string Co_coDocumento_CuentaEXT_Empty = "msg_co_CoDocumento_CuentaEXT_Empty";

        /// <summary>
        ///  "Debe existir una cuenta local y una cuenta extranjera para la moneda origen"
        /// </summary>
        public static string Co_coDocumento_CuentaEXTLOC_Empty = "msg_co_CoDocumento_CuentaEXTLOC_Empty";

        /// <summary>
        ///  "El comprobante no registra el Tipo de Balance Funcional de acuerdo a la parametrizacion del documento digitado"
        /// </summary>
        public static string Co_coDocumento_BalanceInvalid = "msg_co_CoDocumento_BalanceInvalid";

        #endregion
        #region coCargoEspecial
        /// <summary>
        /// "El tipo de cargo solo puede tener valores entre 1 y 7"
        /// </summary>
        public static string Co_CargoEspecial_CargoTipo_Restr = "msg_co_CargoEspecial_CargoTipo_Restr";
        #endregion

        #endregion

        #region Cuentas por pagar

        #region cpConceptoCXP

        /// <summary>
        ///  "En Concepto de CxP solo se permiten Documentos contables 21 o 24"
        /// </summary>
        public static string Cp_ConceptoCXP_DocumentoID_Restr = "msg_cp_ConceptoCXP_DocumentoID_Restr";

        /// <summary>
        ///  "El concepto Tipo solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Cp_ConceptoCXP_TipoConcepto_Restr = "msg_cp_ConceptoCXP_TipoConcepto_Restr";

        #endregion
        #region cpDistribuyeImpLocal

        /// <summary>
        ///  "El porcentaje no puede ser diferente de 100"
        /// </summary>
        public static string Cp_Distribuye_PorcentajeEqual100 = "msg_cp_distribuye_PorcentajeEqual100";

        #endregion

        #endregion

        #region Facturacion
        #region faConceptos
        /// <summary>
        ///  "El Tipo de Concepto solo puede tener valores entre 1 y 6"
        /// </summary>
        public static string Fa_Conceptos_TipoConcepto_Restr = "msg_fa_Conceptos_TipoConcepto_Restr";

        #endregion
        #region faMovimientoTipo
        /// <summary>
        ///  "El Tipo de Movimiento solo puede tener valores entre 1 y 7"
        /// </summary>
        public static string Fa_MovimientoTipo_TipoMov_Restr = "msg_fa_MovimientoTipo_TipoMov_Restr";

        #endregion
        #region faFacturas

        /// <summary>
        ///  "El Documento del Documento Contable para tipo de facturas debe ser 41 o 42"
        /// </summary>
        public static string Fa_FacturaTipo_Documento_Checked = "msg_fa_FacturaTipo_Documento_Checked";

        #endregion
        #endregion

        #region Global

        #region glActividadFlujo

        /// <summary>
        /// Si no pertenece al sistema el tipo de documento no puede ser de tipo Sistema
        /// </summary>
        public static string GL_ActividadFlujo_TipoDoc_restr = "msg_gl_ActividadFlujo_TipoDocDiferent";

        /// <summary>
        /// Si no pertenece al sistema el tipo de documento no puede ser de tipo Sistema
        /// </summary>
        public static string GL_ActividadFlujoBorrar = "msg_gl_actividadFlujoBorrar";

        #endregion
        #region glDatosMensuales
        /// <summary>
        /// No se actulizo la fecha, por favor consulte con el Administrador
        /// </summary>
        public static string gl_DatosMensuales_PeriodoID_Err = "msg_gl_DatosMensuales_PeriodoID";
        #endregion
        #region glDiasFestivos
        /// <summary>
        /// No se actulizo la fecha, por favor consulte con el Administrador
        /// </summary>
        public static string gl_DiasFestivos_PeriodoID_Err = "msg_gl_DiasFestivos_coFechaID_Err";
        #endregion
        #region glConceptoSaldo

        /// <summary>
        /// "El Saldo Control solo puede tener valores entre 1 y 5"
        /// </summary>
        public static string Gl_ConceptoSaldo_coSaldoCtrl_Restr = "msg_gl_ConceptoSaldo_coSaldoCtrl_Restr";

        /// <summary>
        /// "Las cuentas relacionadas al documento contable debe corresponder a un documento Externo"
        /// </summary>
        public static string Gl_ConceptoSaldo_coDocumento_Restr = "msg_gl_ConceptoSaldo_coDocumento_Restr";

        /// <summary>
        /// "No se puede editar el concepto saldo de una cuenta que ya tenga saldos"
        /// </summary>
        public static string Gl_ConceptoSaldo_ID_SaldoExist = "msg_gl_ConceptoSaldo_ID_Restr";

        /// <summary>
        /// El numero comprobante solo puede ser de 0 a 20.
        /// </summary>
        public static string Gl_ConceptoSaldo_NumeroComp = "msg_gl_ConceptoSaldo_NumeroComp";


        #endregion
        #region glEmpresa

        /// <summary>
        /// "La empresa {0} ya tiene movimientos registrados y el grupe de empresas ({1}) no puede ser modificado"
        /// </summary>
        public static string Gl_Empresa_InvalidGE = "msg_gl_Empresa_InvalidGE";

        #endregion
        #region glTabla
        /// <summary>
        ///  "Todos los niveles que tengan longitud, deben tener descripción"
        /// </summary>
        public static string Gl_Tabla_LengthDescriptionRequired = "msg_gl_Tabla_LengthDescriptionRequired";

        /// <summary>
        ///  "La suma de las longitudes de los niveles excede la longitud máxima del código de la maestra"
        /// </summary>
        public static string Gl_Tabla_LengthExceedID = "msg_gl_Tabla_LengthExceedID";

        /// <summary>
        ///  "La longitud de cada nivel de jerarquía NO puede ser 0"
        /// </summary>
        public static string Gl_Tabla_LengthNotZero = "msg_gl_Tabla_LengthNotZero";

        /// <summary>
        ///  "Las descripciones de los niveles de jerarquía no se pueden repetir"
        /// </summary>
        public static string Gl_Tabla_DescriptionNotRepeat = "msg_gl_Tabla_DescriptionNotRepeat";

        #endregion
        #region glHorarioTrabajo

        /// <summary>
        ///  "El rango de los minutos debe estar entre 0-59"
        /// </summary>
        public static string Gl_Horario_RankMinute = "msg_gl_Horario_RankMinute";

        /// <summary>
        ///  "El rango de la hora debe estar entre 0-23""
        /// </summary>
        public static string Gl_Horario_RankHour = "msg_gl_Horario_RankHour";

        #endregion
        #region glDocumentoAnexo

        /// <summary>
        ///  "El tipo de documento solo puede tener valores entre 0 y 3"
        /// </summary>
        public static string Gl_DocumentoAnexo_TipoDoc_Restr = "msg_gl_DocumentoAnexo_TipoDoc_Restr";

        #endregion
        #endregion

        #region Inventarios

        #region inRefTipo

        /// <summary>
        ///  "El Tipo de inventario solo puede tener valores entre 0 y 9"
        /// </summary>
        public static string In_RefTipo_TipoInv_Restr = "msg_in_RefTipo_TipoInv_Restr";

        /// <summary>
        ///  "El control especial solo puede tener valores entre 0 y 3"
        /// </summary>
        public static string In_RefTipo_ControlEspecial_Restr = "msg_in_RefTipo_ControlEspecial_Restr";

        /// <summary>
        ///  "El control de empaque solo puede tener valores entre 0 y 3"
        /// </summary>
        public static string In_RefTipo_EmpaqueCtr_Restr = "msg_in_RefTipo_EmpaqueCtr_Restr";

        /// <summary>
        ///  "El control de importacion solo puede tener valores entre 0 y 3"
        /// </summary>
        public static string In_RefTipo_ControlImportacion_Restr = "msg_in_RefTipo_ControlImportacion_Restr";

        #endregion
        #region inRefClase

        /// <summary>
        ///  "El número de parámetros solo puede tener valores entre 0 y 2"
        /// </summary>
        public static string In_RefClase_NumParametros_Restr = "msg_in_RefClase_NumParametros_Restr";

        #endregion
        #region inCosteoGrupo

        /// <summary>
        ///  "El tipo de costeo solo puede tener valores entre 0 y 3"
        /// </summary>
        public static string In_CosteoGrupo_CosteoTipo_Restr = "msg_in_CosteoGrupo_CosteoTipo_Restr";

        #endregion
        #region inBodega

        /// <summary>
        ///  "El tipo de bodega seleccionado necesito que el Tipo de Costeo sea transaccional"
        /// </summary>
        public static string In_Bodega_BodegaTipoTransaccional_Restr = "msg_in_Bodega_BodegaTipoTransaccional_Restr";

        #endregion
        #region inBodegaTipo

        /// <summary>
        ///  "El tipo de bodega solo puede tener valores entre 0 y 10"
        /// </summary>
        public static string In_BodegaTipo_BodegaTipo_Restr = "msg_in_BodegaTipo_BodegaTipo_Restr";

        /// <summary>
        ///  "El tipo de propiedad solo puede tener valores entre 0 y 3"
        /// </summary>
        public static string In_BodegaTipo_PropiedadTipo_Restr = "msg_in_BodegaTipo_PropiedadTipo_Restr";

        #endregion
        #region inMovimientoTipo

        /// <summary>
        ///  "El tipo de movimiento solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string In_MovimientoTipo_TipoMovimiento_Restr = "msg_in_MovimientoTipo_TipoMovimiento_Restr";

        #endregion
        #region inRefUbicacion

        /// <summary>
        ///  "La referencia de ubicación solo puede tener valores entre 0 y 4"
        /// </summary>
        public static string In_RefUbicacion_EstadoInv_Restr = "msg_in_RefUbicacion_EstadoInv_Restr";

        #endregion
        #region inPosicionArancel

        /// <summary>
        ///  "El valor del porcentaje no puede ser mayor o igual a 100 y el número de decimales debe ser válido"
        /// </summary>
        public static string In_PosicionArancel_Porcentaje_Invalid = "msg_in_PosicionArancel_Porcentaje_Invalid";

        /// <summary>
        ///  "El número de decimales del porcentaje debe ser válido"
        /// </summary>
        public static string In_PosicionArancel_Porcentaje_InvalidDecimal = "msg_in_PosicionArancel_Porcentaje_InvalidDecimal";

        #endregion
        #endregion

        #region Nomina
        #region noEmpleado
        /// <summary>
        /// Ese terceroID ya esta siendo usado
        /// </summary>
        public static string No_Empleado_TerceroID_Restr = "msg_no_Empleado_TerceroID_Restr";

        /// <summary>
        /// El Empleado no puede estar en Estado Liquidado
        /// </summary>
        public static string No_Empleado_Tipo_Restr = "msg_no_Empleado_Tipo_Restr";
        #endregion
        #region noConceptoPlaTra
        /// <summary>
        ///  "El Tipo de Concepto solo puede tener valores entre 1 y 6"
        /// </summary>
        public static string No_ConceptoPlaTra_Tipo_Restr = "msg_no_ConceptoPlaTra_Tipo_Restr";

        #endregion
        #region noConceptoTipoNOM
        /// <summary>
        ///  "El Tipo de Tercero solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string No_ConceptoTipoNOM_TipoTercero_Restr = "msg_no_ConceptoTipoNOM_TipoTercero_Restr";

        #endregion
        #region noConceptoNOM
        /// <summary>
        ///  "El Tipo de concepto solo puede tener valores entre 1 y 2"
        /// </summary>
        public static string No_ConceptoNOM_Tipo_Restr = "msg_no_ConceptoNOM_Tipo_Restr";

        /// <summary>
        ///  "El Tipo de liquidacion solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string No_ConceptoNOM_TipoLiq_Restr = "msg_no_ConceptoNOM_TipoLiq_Restr";

        /// <summary>
        ///  "La base de la formula solo puede tener valores entre 1 y 2"
        /// </summary>
        public static string No_ConceptoNOM_BaseFormula_Restr = "msg_no_ConceptoNOM_BaseFormula_Restr";
        #endregion
        #region noCompFlexible

        /// <summary>
        ///  "El Tipo de Componente solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string No_CompFlexible_Tipo_Restr = "msg_no_CompFlexible_Tipo_Restr";

        /// <summary>
        ///  "El periodo de pago solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string No_CompFlexible_PeriodoPago_Restr = "msg_no_CompFlexible_PeriodoPago_Restr";

        #endregion
        #region noContratoNov
        /// <summary>
        ///  "El Tipo de Novedad solo puede tener valores entre 1 y 10"
        /// </summary>
        public static string No_ContratoNov_TipoNovedad_Restr = "msg_no_ContratoNov_TipoNovedad_Restr";

        /// <summary>
        ///  "El número de días deben estar entre 1 y 360"
        /// </summary>
        public static string No_ContratoNov_Dias_RangeRestr = "msg_no_ContratoNov_Dias_RangeRestr";
        #endregion

        #endregion

        #region Planeacion
        #region plDistribucionCampo

        /// <summary>
        ///  "El porcentaje total deber ser del 100%"
        /// </summary>
        public static string pl_DistribucionCampo_PorcentajeInvalid = "msg_pl_DistribucionCampo_PorcentajeInvalid";
        #endregion
        #endregion

        #region Seguridad

        #region seUsuario

        /// <summary>
        ///  "El formato de Correo Electrónico del usuario es incorrecto"
        /// </summary>
        public static string Se_Usuario_CorreoE_EmailInvalidFormat = "msg_se_Usuario_CorreoE_EmailInvalidFormat";
        #endregion
        #region seMaquina

        /// <summary>
        ///  "El Tipo de máquina no existe"
        /// </summary>
        public static string Se_Maquina_Tipo_MachineNoExist = "msg_se_Maquina_Tipo_MachineNoExist";

        /// <summary>
        ///  "Ya existe un Tipo de Servidor Central, no puede ingresar otro"
        /// </summary>
        public static string Se_Maquina_Tipo_OnlyOneServer = "msg_se_Maquina_Tipo_OnlyOneServer";

        #endregion

        #endregion

        #region Recursos Humanos

        #region rhCompetencia

        /// <summary>
        ///  "El Tipo de competencia solo puede tener valores entre 1 y 4"
        /// </summary>
        public static string Rh_Competencia_TipoCompet_Restr = "msg_rh_Competencia_TipoCompet_Restr";

        #endregion
        #region rhCompetenciaxCargo

        /// <summary>
        ///  "El nivel de competencia solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Rh_CompetenciaxCargo_Nivel_Restr = "msg_rh_CompetenciaxCargo_Nivel_Restr";

        /// <summary>
        ///  "La necesidad de la competencia solo puede tener valores entre 1 y 2"
        /// </summary>
        public static string Rh_CompetenciaxCargo_Necesidad_Restr = "msg_rh_CompetenciaxCargo_Necesidad_Restr";

        #endregion

        #endregion

        #region Tesoreria

        #region tsCaja

        /// <summary>
        ///  "El documento del documento contable asociado a caja debe ser de tipo 32"
        /// </summary>
        public static string Ts_Caja_DocumentoID_Restr = "msg_ts_Caja_DocumentoID_Restr";

        #endregion
        #region tsBancosCuenta

        /// <summary>
        ///  "El Tipo de cuenta solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Ts_BancosCuenta_CuentaTipo_Restr = "msg_ts_BancosCuenta_CuentaTipo_Restr";

        /// <summary>
        ///  "El Tipo de liquidación de comisión solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Ts_BancosCuenta_TipoLiquComision_Restr = "msg_ts_BancosCuenta_TipoLiquComision_Restr";

        /// <summary>
        ///  "El dato numérico solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Ts_BancosCuenta_DatoNumerico_Restr = "msg_ts_BancosCuenta_DatoNumerico_Restr";

        /// <summary>
        ///  "El formato del año solo puede tener valores entre 1 y 2"
        /// </summary>
        public static string Ts_BancosCuenta_FormatoAno_Restr = "msg_ts_BancosCuenta_FormatoAno_Restr";

        /// <summary>
        ///  "El formato de la fecha solo puede tener valores entre 1 y 6"
        /// </summary>
        public static string Ts_BancosCuenta_FormatoFecha_Restr = "msg_ts_BancosCuenta_FormatoFecha_Restr";

        /// <summary>
        ///  "La moneda origen del documento contable no es correcta"
        /// </summary>
        public static string Ts_BancosCuenta_DocContable_MdaOrigenInvalid = "msg_ts_BancosCuenta_DocContable_MdaOrigenInvalid";

        /// <summary>
        ///  "El documento del documento contable asociado a esta cuenta debe ser 31"
        /// </summary>
        public static string Ts_BancosCuenta_DocContable_Checked = "msg_ts_BancosCuenta_DocContable_Checked";

        #endregion
        #region tsFlujoFondo

        /// <summary>
        ///  "El Tipo de flujo solo puede tener valores entre 1 y 3"
        /// </summary>
        public static string Ts_FlujoFondo_TipoFlujo_Restr = "msg_ts_FlujoFondo_TipoFlujo_Restr";

        #endregion

        #endregion
    }
}
