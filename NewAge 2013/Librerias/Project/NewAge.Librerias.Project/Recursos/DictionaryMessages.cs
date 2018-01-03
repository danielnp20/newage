using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase con las llaves para el diccionario
    /// </summary>
    public static class DictionaryMessages
    {
        #region Errores

        #region General

        /// <summary>
        /// "No se pudo realizar la contabilizacion"
        /// </summary>
        public static string Err_Account = "ERR_Account";

        /// <summary>
        /// "No se pudo agregar los datos a la bitacora"
        /// </summary>
        public static string Err_AddBita = "ERR_AddBita";

        /// <summary>
        /// "No se pudo agregar el comprobante"
        /// </summary>
        public static string Err_AddCompr = "ERR_AddCompr";

        /// <summary>
        /// "No se pudo agregar la información a la tabla de control"
        /// </summary>
        public static string Err_AddControl = "ERR_AddControl";

        /// <summary>
        /// "No se pudo ingresar los datos"
        /// </summary>
        public static string Err_AddData = "ERR_AddData";

        /// <summary>
        /// "No se pudo agregar el documento"
        /// </summary>
        public static string Err_AddDocument = "ERR_AddDocument";

        /// <summary>
        /// "No se pudo crear el grupo de empresas, verifique que no exista"
        /// </summary>
        public static string Err_AddEG = "ERR_AddEG";

        /// <summary>
        /// "No fue posible agregar el usuario"
        /// </summary>
        public static string Err_AddUser = "ERR_AddUser";

        /// <summary>
        /// "No se puede ajustar un comprobante con periodo superior al periodo actual"
        /// </summary>
        public static string Err_AjusteComp = "ERR_AjusteComp";

        /// <summary>
        /// "No fue posible aprobar el comprobante. Periodo: {0}, Comprobante: {1} y Numero de comprobante: {2}"
        /// </summary>
        public static string Err_AprobComp = "ERR_AprobComp";

        /// <summary>
        /// "Error haciendo cierre. Aún hay comprobantes sin aprobar"
        /// </summary>
        public static string Err_AuxiliarPreNotClean = "ERR_AuxiliarPreNotClean";

        /// <summary>
        /// "La cuenta {0}, no puede superar el saldo inicial (Tercero: {1} - Documento: {2})"
        /// </summary>
        public static string Err_BalanceSaldoIni = "ERR_BalanceSaldoIni";

        /// <summary>
        /// "Error en el formato de las columnas"
        /// </summary>
        public static string Err_ColumnsFormat = "ERR_ColumnsFormat";

        /// <summary>
        /// "Columna {0} duplicada"
        /// </summary>
        public static string Err_ColumnDuplicated = "ERR_ColumnDuplicated";

        /// <summary>
        /// "Columna {0} inválida"
        /// </summary>
        public static string Err_ColumnInvalid = "ERR_ColumnInvalid";

        /// <summary>
        /// "Falta la columna {0}"
        /// </summary>
        public static string Err_ColumnLeft = "ERR_ColumnLeft";

        /// <summary>
        /// "Error haciendo cierre. No hay un comprobante de cierre configurado"
        /// </summary>
        public static string Err_ComprobanteCierre = "ERR_ComprobanteCierre";

        /// <summary>
        /// "El codigo {0} - {1} no tiene datos en la tabla de control"
        /// </summary>
        public static string Err_ControlNoData = "ERR_ControlNoData";

        /// <summary>
        /// "Los datos no pudieron ser copiados"
        /// </summary>
        public static string Err_Copy = "ERR_Copy";

        /// <summary>
        /// "ERROR EN LA INFORMACIÓN - INCONSISTENCIA DE DATOS'"
        /// </summary>
        public static string Err_Data = "ERR_Data";

        /// <summary>
        /// "No se pudo eliminar la empresa debido a q ya se han hecho transacciones"
        /// </summary>
        public static string Err_DeleteEmp = "ERR_DeleteEmp";

        /// <summary>
        /// "No se pudo eliminar la información"
        /// </summary>
        public static string Err_DeleteData = "ERR_DeleteData";

        /// <summary>
        /// "No se pudo eliminar la información a la tabla de control"
        /// </summary>
        public static string Err_DeleteEmpControl = "ERR_DeleteEmpControl";

        /// <summary>
        /// "No se pudo eliminar el comprobante"
        /// </summary>
        public static string Err_DeleteCompr = "ERR_DeleteCompr";

        /// <summary>
        /// "El documento tiene movimientos relacionados"
        /// </summary>
        public static string Err_DocConMvtos = "ERR_docConMvtos";

        /// <summary>
        /// "Para este periodo aprobo el documento"
        /// </summary>
        public static string Err_DocumentoAprobado = "ERR_DocumentoAprobado";

        /// <summary>
        /// "Para editar el detalle del documento primero debe tener completar el cabezote "
        /// </summary>
        public static string Err_DocInvalidHeader = "ERR_DocInvalidHeader";

        /// <summary>
        /// "El documento no tiene un comprobante asociado"
        /// </summary>
        public static string Err_DocNoComp = "ERR_DocNoComp";

        /// <summary>
        /// "Este documento ya fue procesado"
        /// </summary>
        public static string Err_DocProcessed = "ERR_DocProcessed";

        /// <summary>
        /// "Problema cargando las columnas"
        /// </summary>
        public static string Err_GettingColsInfo = "ERR_GettingColsInfo";

        /// <summary>
        /// "Error obteniendo la empresa general de la tabla de control"
        /// </summary>
        public static string Err_GettingDefaultCompany = "ERR_GettingDefaultCompany";

        /// <summary>
        /// "Error obteniendo los datos"
        /// </summary>
        public static string Err_GettingData = "ERR_GettingData";

        /// <summary>
        /// "Error obteniendo el documento"
        /// </summary>
        public static string Err_GettingDocument = "ERR_GettingDocument";

        /// <summary>
        /// "Error obteniendo lista"
        /// </summary>
        public static string Err_GettingList = "ERR_GettingList";

        /// <summary>
        /// "No fue posible traer el usuario"
        /// </summary>
        public static string Err_GettingUser = "ERR_GettingUser";

        /// <summary>
        /// "Error en los datos jerarquicos"
        /// </summary>
        public static string Err_HierarchyData = "ERR_HierarchyData";

        /// <summary>
        /// "No existe el padre para el item dado"
        /// </summary>
        public static string Err_HierarNoParentFound = "ERR_HierarNoParentFound";

        /// <summary>
        /// "El documento no tiene el indicador de ajuste activado"
        /// </summary>
        public static string Err_IndAjusteComp = "ERR_IndAjusteComp";

        /// <summary>
        /// "La bodega "{0}", con numero "{1}", ya fue aprobada o eliminada"
        /// </summary>
        public static string Err_InvalidBodegaTemp = "ERR_co_InvalidBodegaTemp";

        /// <summary>
        /// "El documento no tiene un comprobante relacionado"
        /// </summary>
        public static string Err_InvalidCompDoc = "ERR_invalidCompDoc";

        /// <summary>
        /// "El comprobante "{0}", con numero "{1}" y periodo "{2}", ya fue aprobado o eliminado"
        /// </summary>
        public static string Err_InvalidCompTemp = "ERR_co_InvalidCompTemp";

        /// <summary>
        /// "La fecha seleccionada no corresponde a la del periodo del modulo"
        /// </summary>
        public static string Err_InvalidDatePeriod = "ERR_invalidDatePeriod";

        /// <summary>
        /// "El documento seleccionado no se puede migrar"
        /// </summary>
        public static string Err_InvalidImport = "ERR_InvalidImport";

        /// <summary>
        /// "Los datos ingresados para el reporte no son validos"
        /// </summary>
        public static string Err_InvalidInputReportData = "ERR_InvalidInputReportData";

        /// <summary>
        /// "Error consultando el prefijo del documento"
        /// </summary>
        public static string Err_InvalidPrefix = "ERR_InvalidPrefix";

        /// <summary>
        /// "El valor del documento no es el mimso del saldo"
        /// </summary>
        public static string Err_InvalidSaldoDoc = "ERR_invalidSaldoDoc";

        /// <summary>
        /// "No existe un salario minimo mensual legal vigente asignado"
        /// </summary>
        public static string Err_InvalidSMMLV = "ERR_InvalidSMMLV";

        /// <summary>
        /// "Error cargando los datos"
        /// </summary>
        public static string Err_LoadingData = "ERR_LoadingData";

        /// <summary>
        /// "El codigo {0}, en la tabla {1}, no esta activo"
        /// </summary>
        public static string Err_MasterItemUnActive = "ERR_MasterItemUnActive";

        /// <summary>
        /// "El modulo asociado a la cuenta ya tiene saldos para el periodo {0}"
        /// </summary>
        public static string Err_ModWithSaldos = "ERR_ModWithSaldos";

        /// <summary>
        /// "Se encontro mas de un registro en la tabla de control (Persistencia)"
        /// </summary>
        public static string Err_MultipleControl = "ERR_MultipleControl";

        /// <summary>
        /// "No se encontro el comprobante"
        /// </summary>
        public static string Err_NoComp = "ERR_NoComp";

        /// <summary>
        /// "No se encontro el documento"
        /// </summary>
        public static string Err_NoDocument = "ERR_NoDocument";

        /// <summary>
        /// "Registro Excluido"
        /// </summary>
        public static string Err_NoRowAdded = "ERR_NoRowAdded";

        /// <summary>
        /// "Por favor realice la estructura de esta maestra en la configuración de tablas"
        /// </summary>
        public static string Err_NotConfiguredMaster = "ERR_NotConfiguredMaster";

        /// <summary>
        /// "El número del primer documento no existe en el control de cartera"
        /// </summary>
        public static string Err_NotFoundNumDoc = "ERR_NotFoundNumDoc";

        /// <summary>
        /// "Solo se puede abrir el ultimo periodo que fue cerrado"
        /// </summary>
        public static string Err_OpenLastPeriod = "ERR_OpenLastPeriod";

        /// <summary>
        /// "No se encontró operación. Verificar los indicadores de proyecto y centro de costo en la cuenta"
        /// </summary>
        public static string Err_OperacionIsNullorEmpty = "ERR_OperacionIsNullorEmpty";

        /// <summary>
        /// "Los datos no pudieron ser pegados"
        /// </summary>
        public static string Err_Paste = "ERR_Paste";

        /// <summary>
        /// "El periodo seleccionado está cerrado"
        /// </summary>
        public static string Err_PeriodoCerrado = "ERR_periodoCerrado";

        /// <summary>
        /// "El periodo seleccionado está en cierre"
        /// </summary>
        public static string Err_PeriodoEnCierre = "ERR_periodoEnCierre";

        /// <summary>
        /// "No fue posible rechazar el comprobante. Periodo: {0}, Comprobante: {1} y Numero de comprobante: {2}"
        /// </summary>
        public static string Err_RechazarComp = "ERR_RechazarComp";

        /// <summary>
        /// "No fue posible rechazar el documento. Periodo: {0}, Documento: {1} y Numero de documento: {2}"
        /// </summary>
        public static string Err_RechazarDoc = "ERR_RechazarDoc";

        /// <summary>
        /// "No se puede revertir un documento con periodo superior al periodo actual"
        /// </summary>
        public static string Err_RevertComp = "ERR_RevertComp";

        /// <summary>
        /// "La cuenta {0} es de naturaleza credito y no puede ingresar saldos nuevos positivos"
        /// </summary>
        public static string Err_SaldoIniCtaCre = "ERR_SaldoIniCtaCre";

        /// <summary>
        /// "La cuenta {0} es de naturaleza debito y no puede ingresar saldos nuevos negativos"
        /// </summary>
        public static string Err_SaldoIniCtaDeb = "ERR_SaldoIniCtaDeb";

        /// <summary>
        /// "La cuenta {0} es de naturaleza credito y no puede actualizar saldos con valores positivos (Tercero: {1} - Documento: {2})"
        /// </summary>
        public static string Err_SaldoUpdCtaCre = "ERR_SaldoUpdCtaCre";

        /// <summary>
        /// "La cuenta {0} es de naturaleza debito y no puede actualizar saldos con valores negativos (Tercero: {1} - Documento: {2})"
        /// </summary>
        public static string Err_SaldoUpdCtaDeb = "ERR_SaldoUpdCtaDeb";

        /// <summary>
        /// "El saldo actual del comprobante,  no corresponde con el saldo actual del documento"
        /// </summary>
        public static string Err_SaldoAjusteComp = "Err_SaldoAjusteComp";

        /// <summary>
        /// "Se presento un error enviando el correo"
        /// </summary>
        public static string Err_SendMail = "ERR_SendMail";

        /// <summary>
        /// "No se pudo enviar para aprobacion el comprobante"
        /// </summary>
        public static string Err_SendToAprobCompr = "ERR_SendToAprobCompr";

        /// <summary>
        /// "No se pudo enviar para aprobacion la transacción actual"
        /// </summary>
        public static string Err_SendToAprobTransaccion = "ERR_SendToAprobTransaccion";

        /// <summary>
        /// "No se pudo enviar para aprobacion el documento, por favor verifique nuevamente el estado"
        /// </summary>
        public static string Err_SendToAprobDoc = "ERR_SendToAprobDoc";

        /// <summary>
        /// "No se pudo cancelar la tarea"
        /// </summary>
        public static string Err_StopThread = "ERR_StopThread";

        /// <summary>
        /// "No hay tasa de cierre"
        /// </summary>
        public static string Err_TasaCierre = "ERR_TasaCierre";

        /// <summary>
        /// Se presento un error cargando la informacion del temporal"
        /// </summary>
        public static string Err_TempLoad = "ERR_TempLoad";

        /// <summary>
        /// "Problema al traer la longitud de "
        /// </summary>
        public static string Err_UdtLength = "ERR_UdtLength";

        /// <summary>
        /// "UDT No registrado para conversion a Sql"
        /// </summary>
        public static string Err_UdtToSQL = "ERR_UdtToSQL";

        /// <summary>
        /// "Error actualizando los datos"
        /// </summary>
        public static string Err_UpdateData = "ERR_UpdateData";

        /// <summary>
        /// "No se pudo actualizar el documento"
        /// </summary>
        public static string Err_UpdateDocument = "ERR_UpdateDocument";

        /// <summary>
        /// "BAJE LA ULTIMA VERSIÓN DE LA GRILLA"
        /// </summary>
        public static string Err_UpdateGrid = "ERR_UpdateGrid";

        /// <summary>
        /// "Error actualizando la contraseña"
        /// </summary>
        public static string Err_UpdatePwd = "ERR_UpdatePwd";

        /// <summary>
        /// "Error validando las credenciales del usuario"
        /// </summary>
        public static string Err_UserValidate = "ERR_UserValidate";

        /// <summary>
        /// "Se presento un problema validando los datops"
        /// </summary>
        public static string Err_ValidateData = "ERR_ValidateData";

        /// <summary>
        /// "El prefijo importado no corresponde al seleccionado"
        /// </summary>
        public static string Err_PrefixNotCompatible = "ERR_PrefixNotCompatible";

        /// <summary>
        /// "La fecha importada no corresponde a la seleccionada"
        /// </summary>
        public static string Err_DateNotCompatible = "ERR_DateNotCompatible";

        /// <summary>
        /// "El codigo {0}: {1} no existe"
        /// </summary>
        public static string Err_CodeInvalid = "ERR_CodeInvalid";

        /// <summary>
        /// "No fue posible aprobar el documento:  Periodo: {0},Prefijo: {1} y Numero de documento: {2}"
        /// </summary>
        public static string Err_AprobarDoc = "ERR_AprobarDoc";

        /// <summary>
        /// "No puede realizar esta tarea, este documento ya se encuentra Aprobado"
        /// </summary>
        public static string Err_DocumentEstateAprob = "ERR_DocumentEstateAprob";

        /// <summary>
        /// "{0} no tiene definida un(a) {1}"
        /// </summary>
        public static string Err_CodeIncompleteFK = "ERR_CodeIncompleteFK";

        /// <summary>
        /// "Se presento un error general en el acceso a datos del metodo '{0}', porfavor comuniquese con el administrador del sistema"
        /// </summary>
        public static string Err_AccesoDatos = "ERR_AccesoDatos";

        /// <summary>
        /// "Ésta(e) {0} ya existe en la grilla "
        /// </summary>
        public static string Err_AlreadyExistInGrid = "ERR_AlreadyExistInGrid";

        #endregion

        #region Cierre Periodo

        /// <summary>
        /// "Operaciones conjuntas y contabilidad deben cerrar de últimos en ese orden"
        /// </summary>
        public static string Err_PrerrequisitosCo = "ERR_PrerrequisitosCo";

        /// <summary>
        /// "Operaciones conjuntas y contabilidad deben cerrar de últimos en ese orden"
        /// </summary>
        public static string Err_PrerrequisitosOc = "ERR_PrerrequisitosOc";

        /// <summary>
        /// "CxP cierra antes que Proveedores."
        public static string Err_PrerrequisitosPr = "ERR_PrerrequisitosPr";

        /// <summary>
        /// " Tesorería, Nómina deben cerrar antes que CxP"
        /// </summary>
        public static string Err_PrerrequisitosCp = "ERR_PrerrequisitosCp";

        /// <summary>
        /// "Tesorería, Inventarios y Activos debe cerrar antes que Facturación"
        /// </summary>
        public static string Err_PrerrequisitosFa = "ERR_PrerrequisitosFa";

        #endregion

        #region Activos Fijos

        /// <summary>
        /// "La moneda del alta no corresponde a la de la Factura."
        /// </summary>
        public static string Err_Ac_BillAndDocumentInfo = "ERR_ac_MdaFacyMdaActivoDiferentes";
        
        /// <summary>
        /// "No existe la cuenta para la creación Cuenta - Comprobante."
        /// </summary>
        public static string Err_Ac_Count = "ERR_ac_ValidaCuenta";

        /// <summary>
        /// "El documento no tinene ninguna cuenta asignada."
        /// </summary>
        public static string Err_Ac_CountNoAssigned = "ERR_ac_CountNoAssigned";

        /// <summary>
        /// "La informaicon digitada no es válida."         
        /// </summary>
        public static string Err_Ac_InvalidInfo = "ERR_ac_InformacionInvalida";

        /// <summary>
        /// "No se encontro componente de costo para el activo con plaqueta {0} y clase {1} ({2})"
        /// </summary>
        public static string Err_Ac_NoCompCosto = "ERR_ac_NoCompCosto";

        /// <summary>
        /// "No hay comprobante asociado al documento del movimiento."
        /// </summary>
        public static string Err_Ac_NoCompForMove = "ERR_ac_NoCompForMove";

        /// <summary>
        /// "No existe un tipo demovimiento para la depreciacion (Tipo movimiento: 8)"
        /// </summary>
        public static string Err_Ac_NoMovDepreciacion = "ERR_ac_NoMovDepreciacion";

        /// <summary>
        /// "No tiene activos para depreciar (libro funcional)"
        /// </summary>
        public static string Err_Ac_NoDepreciaFunc = "ERR_ac_NoDepreciaFunc";

        /// <summary>
        /// "No tiene activos para depreciar (libro IFRS)"
        /// </summary>
        public static string Err_Ac_NoDepreciaIFRS = "ERR_ac_NoDepreciaIFRS";

        /// <summary>
        /// "El valor residual no puede ser nulo."
        /// </summary>
        public static string Err_Ac_VrResidualNull = "ERR_ac_ValorResidualNull";

        /// <summary>
        /// "No ha ejecutado el cierre para el Periodo: {0}"
        /// </summary>
        public static string Err_Ac_CierrePeriodo = "ERR_ac_CierrePeriodo";
                

        #endregion

        #region Cartera Corporativa

        /// <summary>
        /// "Esta solicitud {0}, ya posee una anticipo aprobado, por esta razon no se puede rechazar"
        /// </summary>
        public static string Err_Cc_AnticiposAprobados = "ERR_cc_AnticiposAprobados";

        /// <summary>
        /// "El Centro de pago ({0}) ya tiene migraciones para esta fecha"
        /// </summary>
        public static string Err_Cc_CentroPagoMigraciones = "ERR_cc_CentroPagoMigraciones";

        /// <summary>
        /// "No puede cerrar el mes sin haber cerrado todos los dias del modulo. Dia actual {0}"
        /// </summary>
        public static string Err_Cc_CerrarDias = "ERR_cc_CerrarDias";

        /// <summary>
        /// "El cliente {0} ya tiene miraciones para este periodo con el centro de pago {1}"
        /// </summary>
        public static string Err_Cc_ClienteMigraciones = "ERR_cc_ClienteMigraciones";

        /// <summary>
        /// "El valor del comodin no puede ser negativo. Valor cuota: {0}'
        /// </summary>
        public static string Err_Cc_ComodinNeg = "ERR_cc_ComodinNeg";

        /// <summary>
        /// "El componente {0} no tiene ásignado valor en la columna 'Valor Componente{1}'
        /// </summary>
        public static string Err_Cc_CompNoVal = "ERR_cc_CompNoVal";

        /// <summary>
        /// "La libranza {0} fue agregada en la linea {1}"
        /// </summary>
        public static string Err_Cc_CreditoAgregado = "ERR_cc_CreditoAgregado";

        /// <summary>
        /// "El credito {0} ya esta cancelado"
        /// </summary>
        public static string Err_Cc_CreditoCancelado = "ERR_cc_CreditoCancelado";

        /// <summary>
        /// "El credito {0} tiene un estado de cuenta asignado"
        /// </summary>
        public static string Err_Cc_CreditoConEstadoCuenta = "ERR_cc_CreditoConEstadoCuenta";

        /// <summary>
        /// "El credito {0} se encuentra con desestimiento"
        /// </summary>
        public static string Err_Cc_CreditoDesestido = "ERR_cc_CreditoDesestido";

        /// <summary>
        /// "El credito {0} no tiene incorporaciones asignadas"
        /// </summary>
        public static string Err_Cc_CreditoSinIncorporacion = "ERR_cc_CreditoSinIncorporacion";

        /// <summary>
        /// "El crédito {0} requiere portafolio"
        /// </summary>
        public static string Err_Cc_CreditoSinPortafolio = "ERR_cc_CreditoSinPortafolio";

        /// <summary>
        /// "Error en la parametrización del crédito. Valor de cuota: 0"
        /// </summary>
        public static string Err_Cc_Cuota0 = "ERR_cc_Cuota0";

        /// <summary>
        /// "Error en la parametrización de la póliza. Valor de cuota: 0"
        /// </summary>
        public static string Err_Cc_Cuota0Poliza = "ERR_cc_Cuota0Poliza";

        /// <summary>
        /// "No se puede hacer un estado de cuenta de desistimiento para el crédito {0} ya que ha tenido movimientos posteriores "
        /// </summary>
        public static string Err_Cc_DesistimientoMvtos = "ERR_cc_DesistimientoMvtos";

        /// <summary>
        /// "El valor de giro {0} del crédito {1}, no corresponde con el valor de la cuenta de rechazos {2}"
        /// </summary>
        public static string Err_Cc_DesistimientoRechazo = "ERR_cc_DesistimientoRechazo";

        /// <summary>
        /// "No se puede desistir un crédito sin un rechazo previo "
        /// </summary>
        public static string Err_Cc_DesistimientoNoRechazo = "ERR_cc_DesistimientoNoRechazo";

        /// <summary>
        /// "Los días de rango no pueden ser 0 para la clase {0} y días de mora {1}"
        /// </summary>
        public static string Err_Cc_DiasRango0 = "ERR_cc_DiasRango0";

        /// <summary>
        /// "(No Opero - Libranza {0}) Cuota Incorporada antes de liquidar"
        /// </summary>
        public static string Err_Cc_EstadoCruce2 = "ERR_cc_EstadoCruce2";

        /// <summary>
        /// (No Opero - Libranza {0}) Cuota Incorporada después de liquidar"
        /// </summary>
        public static string Err_Cc_EstadoCruce3 = "ERR_cc_EstadoCruce3";

        /// <summary>
        /// "(Opero - Libranza {0}) Cuota Desincorporada"
        /// </summary>
        public static string Err_Cc_EstadoCruce4 = "ERR_cc_EstadoCruce4";

        /// <summary>
        /// "(Opero - Libranza {0}) Cuota Incorporada por valor diferente"
        /// </summary>
        public static string Err_Cc_EstadoCruce5 = "ERR_cc_EstadoCruce5";

        /// <summary>
        /// "(No Opero - Libranza {0}) La cuota que estaba operando"
        /// </summary>
        public static string Err_Cc_EstadoCruce6 = "ERR_cc_EstadoCruce6";

        /// <summary>
        /// "(Opero - Libranza {0}) Credito con mas de un Registro en la Nomina"
        /// </summary>
        public static string Err_Cc_EstadoCruce7 = "ERR_cc_EstadoCruce7";

        /// <summary>
        /// "(Opero - Libranza {0}) Recaudo atrasado"
        /// </summary>
        public static string Err_Cc_EstadoCruce8 = "ERR_cc_EstadoCruce8";

        /// <summary>
        /// "(Opero - Libranza {0}) Saldo a Favor"
        /// </summary>
        public static string Err_Cc_EstadoCruce9 = "ERR_cc_EstadoCruce9";

        /// <summary>
        /// "("No Opero - Libranza {0}) Cuota Incorporada de Credito sin liquidar"
        /// </summary>
        public static string Err_Cc_EstadoCruce10 = "ERR_cc_EstadoCruce10";

        /// <summary>
        /// "(Opero - Libranza {0}) Recaudo adelantado"
        /// </summary>
        public static string Err_Cc_EstadoCruce11 = "ERR_cc_EstadoCruce11";

        /// <summary>
        /// "El credito {0} no ha sido aprobado y tiene el indicador de incorporacion previa habilitado"
        /// </summary>
        public static string Err_Cc_IncorpPrevia = "ERR_cc_IncorpPrevia";

        /// <summary>
        /// "La Pagaduria no corresponde con la registrada en la libranza"
        /// </summary>
        public static string Err_Cc_InvalidPagaduriaLibranza = "ERR_cc_InvalidPagaduriaLibranza";

        /// <summary>
        /// "El codigo de empleado {0} no esta relacionado con ningún cliente"
        /// </summary>
        public static string Err_Cc_InvalidCodEmpleado = "ERR_cc_InvalidCodEmpleado";

        /// <summary>
        /// "El codigo del cliente no corresponde al credito de la libranza"
        /// </summary>
        public static string Err_Cc_InvalidClienteLibranza = "ERR_cc_InvalidClienteLibranza";

        /// <summary>
        /// "Los componentes automáticos solo pueden ser de tipo 3 (Descuento Giro). Componente Origen: "{0}" Componente Automático: "{1}" "
        /// </summary>
        public static string Err_Cc_InvalidCompAuto = "ERR_cc_InvalidCompAuto";

        /// <summary>
        /// "La cuenta asignada al concepto de CxP debe corresponder a un documento externo o componente documento"
        /// </summary>
        public static string Err_Cc_InvalidCtaConcCxP = "ERR_cc_InvalidCtaConcCxP";

        /// <summary>
        /// "El componente "{0}" No tiene cuentas asociadas para estado "{1}" con clase "{2}""
        /// </summary>
        public static string Err_Cc_InvalidCtasComp = "ERR_cc_InvalidCtasComp";

        /// <summary>
        /// "El componente "{0}" No tiene cuentas asociadas"
        /// </summary>
        public static string Err_Cc_InvalidCtasCompNoData = "ERR_cc_InvalidCtasCompNoData";

        /// <summary>
        /// "La cuenta de ingreso para el componente "{0}", estado "{1}" y clase "{2}", no puede estar vacia"
        /// </summary>
        public static string Err_Cc_InvalidCtaIngreso = "ERR_cc_InvalidCtaIngreso";

        /// <summary>
        /// "El componente de interes debe tener asignada cuenta de ingresos y recursos terceros para cartera propia y clase "{0}""
        /// </summary>
        public static string Err_Cc_InvalidCtasInteres = "ERR_cc_InvalidCtasInteres";

        /// <summary>
        /// "No se encontró información de cuentas para el componente de interes"
        /// </summary>
        public static string Err_Cc_InvalidCtasInteresNoInfo = "ERR_cc_InvalidCtasInteresNoInfo";

        /// <summary>
        /// "El componente de interes anticipado debe tener asignada cuenta de ingresos y recursos terceros para cartera propia y clase "{0}""
        /// </summary>
        public static string Err_Cc_InvalidCtasIntAnt = "ERR_cc_InvalidCtasIntAnt";

        /// <summary>
        /// "No se encontró información de cuentas para el componente de interes anticipado"
        /// </summary>
        public static string Err_Cc_InvalidCtasIntAntNoInfo = "ERR_cc_InvalidCtasIntAntNoInfo";

        /// <summary>
        /// "La cuenta de recursos terceros para el componente "{0}", estado "{1}" y clase "{2}", no puede estar vacia"
        /// </summary>
        public static string Err_Cc_InvalidCtaRecursosTerceros = "ERR_cc_InvalidCtaRecursosTerceros";

        /// <summary>
        /// "El crédito {0} No existe"
        /// </summary>
        public static string Err_Cc_InvalidCredito = "ERR_cc_InvalidCredito";

        /// <summary>
        /// "La libranza {0} No existe"
        /// </summary>
        public static string Err_Cc_InvalidLibranza = "ERR_cc_InvalidLibranza";

        /// <summary>
        /// "No se puede hacer un recaudo masivo sobre un crédito con estado de deuda: Jurídico, Acuerdo de pago o Acuerdo de pago incumplido"
        /// </summary>
        public static string Err_Cc_InvalidPagoEstadoCredito = "ERR_cc_InvalidPagoEstadoCredito";

        /// <summary>
        /// "El periodo del módulo de CxP no corresponde con el periodo del módulo de cartera"
        /// </summary>
        public static string Err_Cc_InvalidPeriodoAnticipos = "ERR_cc_InvalidPeriodoAnticipos";

        /// <summary>
        /// "No existe una relacion entre el plazo y valor en la maestra de valores autorizados"
        /// </summary>
        public static string Err_Cc_InvalidPlazoValor = "ERR_cc_InvalidPlazoValor";

        /// <summary>
        /// "El portafolio del crédito {0} no esta relacionado con el comprador"
        /// </summary>
        public static string Err_Cc_InvalidPortafolioComprador = "ERR_cc_InvalidPortafolioComprador";

        /// <summary>
        /// "No se puede realizar la venta de cartera, ya que no hay cuotas por vender'
        /// </summary>
        public static string Err_Cc_InvalidVentaCartera = "ERR_cc_InvalidVentaCartera";

        /// <summary>
        /// "Los pagos de las cuentas de balance ({0}) no corresponden con el valor del recibo de caja ({1})"
        /// </summary>
        public static string Err_Cc_InvalidVlrBalance = "ERR_cc_InvalidVlrBalance";

        /// <summary>
        /// "El monto de la libranza no debe superar lo autorizado en la maestra de valores amparados"
        /// </summary>
        public static string Err_Cc_InvalidVlrLibranza = "ERR_cc_InvalidVlrLibranza";

        /// <summary>
        /// "La libranza {0}, en el periodo {1} no tiene saldos"
        /// </summary>
        public static string Err_Cc_LibranzaNoSaldo = "ERR_cc_LibranzaNoSaldo";

        /// <summary>
        /// "La libranza {0}, ya se encuentra registrada en el sistema"
        /// </summary>
        public static string Err_Cc_LibranzaRegistrada = "ERR_cc_LibranzaRegistrada";

        /// <summary>
        /// "Esta libranza ya se encuentra liquidada en el sistema"
        /// </summary>
        public static string Err_Cc_LibranzaLiquidada = "ERR_cc_LibranzaLiquidada";

        /// <summary>
        /// "Ya se realizo una migracion previa con ese centro de pago y periodo "
        /// </summary>
        public static string Err_Cc_MigracionExistente = "ERR_cc_MigracionExistente";

        /// <summary>
        /// "El cliente {0} tiene mas de un credito. Debe especificar la libranza" 
        /// </summary>
        public static string Err_Cc_MultipleCredByCliente = "ERR_cc_MultipleCredByCliente";

        /// <summary>
        /// "El cliente con código de empleado {0} tiene mas de un credito. Debe especificar la libranza" 
        /// </summary>
        public static string Err_Cc_MultipleCredByCodigo = "ERR_cc_MultipleCredByCodigo";

        /// <summary>
        /// "El credito {0} cambio el valor del saldo, para la recompra de cartera"
        /// </summary>
        public static string Err_Cc_NewSaldoRecompra = "ERR_cc_NewSaldoRecompra";

        /// <summary>
        /// "No hay información de clasificación de riesgos para la clase {0} y días de mora {1}"
        /// </summary>
        public static string Err_Cc_NoInfoRiesgos = "ERR_cc_NoInfoRiesgos";

        /// <summary>
        /// "No existe clasificación de riesgo para el crédito {0}. Con clase de crédito {1} y días de mora {2}"
        /// </summary>
        public static string Err_Cc_NoClasificacionRiesgo = "Err_cc_NoClasificacionRiesgo";

        /// <summary>
        /// "No se encontro porcentaje o valor para el componente ({0}). Linea de credito ({1}) - Monto ({2}) - Plazo ({3})"
        /// </summary>
        public static string Err_Cc_NoComponentePorc = "ERR_cc_NoComponentePorc";

        /// <summary>
        /// "El cliente {0} No tiene creditos activos" 
        /// </summary>
        public static string Err_Cc_NoCredByCliente = "ERR_cc_NoCredByCliente";

        /// <summary>
        /// "El cliente con código de empleado {0} No tiene creditos activos" 
        /// </summary>
        public static string Err_Cc_NoCredByCodigo = "ERR_cc_NoCredByCodigo";

        /// <summary>
        /// "Debe llenar en la tabla de control toda la información para las cuentas de provisiones"
        /// </summary>
        public static string Err_Cc_NoCtaProv = "ERR_cc_NoCtaProv";

        /// <summary>
        /// "Se requiere cuenta de causación de intereses para la clase de crédito {0} y días de vencimiento {1}"
        /// </summary>
        public static string Err_Cc_NoCtaCausaInt = "ERR_cc_NoCtaCausaInt";

        /// <summary>
        /// "Se requiere cuenta de provision de intereses para la clase de crédito {0} y días de vencimiento {1}"
        /// </summary>
        public static string Err_Cc_NoCtaProvInt = "ERR_cc_NoCtaProvInt";

        /// <summary>
        /// "Debe inresar el tercero de la fiduciaria y las cuentas de patrimonio autónomo"
        /// </summary>
        public static string Err_Cc_NoInfoPatrimonio = "ERR_cc_NoInfoPatrimonio";

        /// <summary>
        /// "No se pudo realizar la migracion de cartera'
        /// </summary>
        public static string Err_Cc_NoMigracion = "ERR_cc_NoMigracion";

        /// <summary>
        /// "No hay ninguna libranza seleccionada"
        /// </summary>
        public static string Err_Cc_NotExistLibranza = "ERR_cc_NotExistLibranza";

        /// <summary>
        /// "El plan de pagos no puede generar cuotas con capital o intereses negativos"
        /// </summary>
        public static string Err_Cc_PlanPagosNeg = "ERR_cc_PlanPagosNeg";

        /// <summary>
        /// "No se puede revertir un crédito que no sea de cartera propia. Libranza: {0}"
        /// </summary>
        public static string Err_Cc_RevCrCarteraCedida = "ERR_cc_RevCrCarteraCedida";

        /// <summary>
        /// "No se puede revertir un crédito si ya se realizó el giro. Libranza: {0}"
        /// </summary>
        public static string Err_Cc_RevCrGiro = "ERR_cc_RevCrGiro";

        /// <summary>
        /// "No se puede revertir el documento porque ya se realizaron pagos para el crédito {0}"
        /// </summary>
        public static string Err_Cc_RevEnvioCJPagos = "ERR_cc_RevEnvioCJPagos";

        /// <summary>
        /// "No se puede revertir el documento porque el cliente cambio de estado"
        /// </summary>
        public static string Err_Cc_RevEnvioCJClienteCambio = "ERR_cc_RevEnvioCJClienteCambio";

        /// <summary>
        /// "No se puede revertir el documento porque el crédito {0}, ha cambiado de estado"
        /// </summary>
        public static string Err_Cc_RevEnvioCJCreditoCambio = "ERR_cc_RevEnvioCJCreditoCambio";

        /// <summary>
        /// Ya se agregó una reincorporación en el periodo ({0}), para el crédito ({1}) con pagaduría(CP) ({2}) y pagaduría mod ({3})
        /// </summary>
        public static string Err_Cc_ReincorporaAdded = "ERR_cc_ReincorporaAdded";

        /// <summary>
        /// "El centro de pago digitado no esta relacionado con el cliente"
        /// </summary>
        public static string Err_Cc_ReincorporaInvalidCP = "ERR_cc_ReincorporaInvalidCP";

        /// <summary>
        /// "El reintegro ({0}) no tiene documento relacionado"
        /// </summary>
        public static string Err_Cc_ReintegroNoDoc = "ERR_cc_ReintegroNoDoc";

        /// <summary>
        /// "No se puede revertir un crédito con pagos realizados. Libranza: {0}"
        /// </summary>
        public static string Err_Cc_RevCrPagos = "ERR_cc_RevCrPagos";

        /// <summary>
        /// "No se puede revertir el crédito ({0}). Se encontraron pagos realizados sobre la compra del crédito ({1})"
        /// </summary>
        public static string Err_Cc_RevCrCompraPagos = "ERR_cc_RevCrCompraPagos";

        /// <summary>
        /// "No se puede revertir el pago porque se han realizado pagos posteriores"
        /// </summary>
        public static string Err_Cc_RevRecCuotasPagadas = "ERR_cc_RevRecCuotasPagadas";

        /// <summary>
        /// "No se puede revertir el pago porque se han realizado movimientos posteriores"
        /// </summary>
        public static string Err_Cc_RevRecMvtos = "ERR_cc_RevRecMvtos";

        /// <summary>
        /// "No se puede revertir la reincorporación. Uno o varios créditos tienen asignados el consecutivo de incorporación"
        /// </summary>
        public static string Err_Cc_RevReIncConsIncorpora = "ERR_cc_RevReIncConsIncorpora";

        /// <summary>
        /// "El número flujos pendientes del crédito {0} es menro al número de cuotas pendientes de los créditos que lo sustituyen"
        /// </summary>
        public static string Err_Cc_SustitucionCuotasPendientes = "ERR_cc_SustitucionCuotasPendientes";

        /// <summary>
        /// "No existe la libranza de sustitución {0}"
        /// </summary>
        public static string Err_Cc_SustitucionInvalidLibranza = "ERR_cc_SustitucionInvalidLibranza";

        /// <summary>
        /// "El crédito que sustituye no puede estar en mora"
        /// </summary>
        public static string Err_Cc_SustitucionMora = "ERR_cc_SustitucionMora";

        /// <summary>
        /// "El crédito a sustituir no tiene flujo pendiente"
        /// </summary>
        public static string Err_Cc_SustitucionNoFlujo = "ERR_cc_SustitucionNoFlujo";

        /// <summary>
        /// "El crédito a sustituir no se encuentra prepagado"
        /// </summary>
        public static string Err_Cc_SustitucionNoPrepagado = "ERR_cc_SustitucionNoPrepagado";

        /// <summary>
        /// "El crédito que sustituye debe ser un crédito propio que no este sustituyendo otro crédito"
        /// </summary>
        public static string Err_Cc_SustitucionPropio = "ERR_cc_SustitucionPropio";

        /// <summary>
        /// "El saldo pendiente de los flujos del crédito {0} es menor al saldo de los créditos que lo sustituyen"
        /// </summary>
        public static string Err_Cc_SustitucionSaldoPendiente = "ERR_cc_SustitucionSaldoPendiente";

        /// <summary>
        /// "El crédito a sustituir no puede estar sustituido"
        /// </summary>
        public static string Err_Cc_SustitucionSustituido = "ERR_cc_SustitucionSustituido";

        /// <summary>
        /// "El crédito a sustituir no puede estar sustituido y recomprado"
        /// </summary>
        public static string Err_Cc_SustitucionSustYRecomprado = "ERR_cc_SustitucionSustYRecomprado";

        /// <summary>
        /// "El crédito a sustituir debe estar vendido o debe estar sustituyendo a otros"
        /// </summary>
        public static string Err_Cc_SustitucionVendido = "ERR_cc_SustitucionVendido";

        /// <summary>
        /// "El crédito no ha sido pagado"
        /// </summary>
        public static string Err_Cc_CreditoNoPagado = "ERR_cc_CreditoNoPagado";

        /// <summary>
        /// "El crédito no tiene Cuenta por Pagar"
        /// </summary>
        public static string Err_Cc_CreditoNoCxP = "ERR_cc_CreditoNoCxP";

        /// <summary>
        /// "La libranza {0} no tiene anticipo para Compra de Cartera"
        /// </summary>
        public static string Err_Cc_AnticipoCompraEmpty = "ERR_cc_AnticipoCompraEmpty";

        /// <summary>
        /// "No existe el componente de Capital o Interes parametrizado en Componente Cuenta"
        /// </summary>
        public static string Err_Cc_ComponenteEmptyRecompra = "ERR_cc_ComponenteEmptyRecompra";

        /// <summary>
        /// "El valor de giro de la solicitud no corresponde con la contrapartida del comprobante"
        /// </summary>
        public static string Err_Cc_VLrGiroInvalid = "ERR_cc_VLrGiroInvalid";

        /// <summary>
        /// "Esta cuenta no se puede utilizar en este documento porque corresponde al módulo de cartera"
        /// </summary>
        public static string Err_Cc_CuentaInvalidModule = "ERR_cc_CuentaInvalidModule";
        #endregion

        #region Cartera Financiera

        /// <summary>
        /// "El pago no corresponde al valor de la cuota"
        /// </summary>
        public static string Err_Cf_EstadoCruce5 = "ERR_cf_EstadoCruce5";

        /// <summary>
        /// "(Crédito {0}) El descuento ya estaba operando"
        /// </summary>
        public static string Err_Cf_EstadoCruce7 = "ERR_cf_EstadoCruce7";

        /// <summary>
        /// "Se presentó descuento, pero el crédito se encuentra atrasado"
        /// </summary>
        public static string Err_Cf_EstadoCruce8 = "ERR_cf_EstadoCruce8";

        /// <summary> 
        /// "Se encontró el crédito, pero no tiene saldo"
        /// </summary>
        public static string Err_Cf_EstadoCruce9 = "ERR_cf_EstadoCruce9";

        /// <summary>
        /// "El crédito opero adelantado mas de un mes adelantado"
        /// </summary>
        public static string Err_Cf_EstadoCruce11 = "ERR_cf_EstadoCruce11";

        /// <summary>
        /// "Ya se realizo el recaudo masivo para el banco {0} con fecha {1}"
        /// </summary>
        public static string Err_Cf_RecaudosMasivosProccesed = "ERR_cf_RecaudosMasivosProccesed";

        /// <summary>
        /// "Debe existir el componente de seguro general para el pago "
        /// </summary>
        public static string Err_Cf_ComponenteSegNotExist = "ERR_cf_ComponenteSegNotExist";

        /// <summary>
        /// "Ya existe esta poliza con el tercero creada sin asignar Solicitud  ni credito"
        /// </summary>
        public static string Err_Cf_PolizaExist = "ERR_cf_PolizaExist";

        #endregion

        #region Contabilidad

        /// <summary>
        /// "Para este periodo ya se realizo el ajuste en cambio"
        /// </summary>
        public static string Err_Co_AjusteAprobado = "ERR_co_AjusteAprobado";

        /// <summary>
        /// "El periodo del módulo ya ha tenido cierres previos"
        /// </summary>
        public static string Err_Co_AjusteCompPeriodo = "ERR_co_AjusteCompPeriodo";

        /// <summary>
        /// "No se pueden ajustar documentos que no estan aprobados"
        /// </summary>
        public static string Err_Co_AjusteInvalidEstado = "ERR_co_AjusteInvalidEstado";

        /// <summary>
        /// "No se encontraron comprobantes para ajstar, primero debe generar la informacion preliminar"
        /// </summary>
        public static string Err_Co_AjusteNoData = "ERR_co_AjusteNoData";

        /// <summary>
        /// "No se puede agregar informacion al auxiliar preliminar, aún existen saldos con el tipo de balance preliminar"
        /// </summary>
        public static string Err_Co_BalPre = "ERR_co_BalPre";

        /// <summary>
        /// "Para el año {0} ya se realizo el cierre anual"
        /// </summary>
        public static string Err_Co_CierreAnualAprobado = "ERR_co_CierreAnualAprobado";

        /// <summary>
        /// "Debe cerrar el periodo 12 de contabilidad antes de realizar el cierre anual"
        /// </summary>
        public static string Err_Co_CierreAnualPeriodoOpen = "ERR_co_CierreAnualPeriodoOpen";

        /// <summary>
        /// "Ya se agregó el comprobante {0} con numero {1} para el periodo actual"
        /// </summary>
        public static string Err_Co_CompAgregado = "ERR_co_CompAgregado";

        /// <summary>
        /// El tipo de Balance para el comprobante de reclasificaciones (IFRS) tiene que corresponder al Tipo de Balance IFRS
        /// </summary>
        public static string Err_Co_CompAndBalanceIFRSDiferents = "ERR_co_CompAndBalanceIFRSDiferents";

        /// <summary>
        /// El tipo de Balance para el comprobante de reclasificaciones (Fiscal) tiene que corresponder al Tipo de Balance Fiscal
        /// </summary>
        public static string Err_Co_CompAndBalanceFiscalDiferents = "ERR_co_CompAndBalanceFiscalDiferents";

        /// <summary>
        /// "No puede iniciar el comprobante {0} en el consecutivo {1}, ya que el último agregado en este periodo tiene el número {2}"
        /// </summary>
        public static string Err_Co_CompInvalidConsecutivo = "ERR_co_CompInvalidConsecutivo";

        /// <summary>
        /// "El comprobante no tiene resultados"
        /// </summary>
        public static string Err_Co_CompNoResults = "ERR_co_CompNoResults";

        /// <summary>
        /// "La empresa {0} tiene informacion en los auxiliares con cuentas alternas que no pudieron ser asignadas"
        /// </summary>
        public static string Err_Co_CtaAlternaNull = "ERR_co_CtaAlternaNull";

        /// <summary>
        /// "La cuenta de ajuste impuesto (control - contabilidad) debe tener nit de cierre anual"
        /// </summary>
        public static string Err_Co_CtaAjImpuesto = "ERR_co_CtaAjImpuesto";

        /// <summary>
        /// "No se pueden hacer movimientos sobre cuentas que tengan el modulo cerrado"
        /// </summary>
        public static string Err_Co_CtaPeriodClosed = "ERR_co_CtaPeriodClosed";

        /// <summary>
        /// "No se pueden ingresar cuentas de inventarios desde un módulo distinto"
        /// </summary>
        public static string Err_Co_CtaInv = "ERR_co_CtaInv";

        /// <summary>
        /// "No se encontro registro de contrapartida en el comprobante"
        /// </summary>
        public static string Err_Co_ContraCero = "ERR_co_ContraCero";

        /// <summary>
        /// "Error eliminando los saldos y el balance"
        /// </summary>
        public static string Err_Co_DeleteBal = "ERR_co_DeleteBal";

        /// <summary>
        /// "Para este periodo ya se realizo la distribucion de comprobantes"
        /// </summary>
        public static string Err_Co_DistribucionAprobado = "ERR_co_DistribucionAprobado";

        /// <summary>
        /// "La distribucion del porcentaje debe ser del 100%"
        /// </summary>
        public static string Err_Co_DistPorc = "ERR_coDistPorc";

        /// <summary>
        /// "El lugar geográfico {0} tiene mal la distribucion de porcentajes de los impuesto"
        /// </summary>
        public static string Err_Co_DistPorcLugGeo = "ERR_co_DistPorcLugGeo";

        /// <summary>
        /// "El documento {0} no tiene cuenta asociada"
        /// </summary>
        public static string Err_Co_DocNoCta = "ERR_co_DocNoCta";

        /// <summary>
        /// "La relacion de los campos del detalle no corresponden con el activo"
        /// </summary>
        public static string Err_Co_InvalidActData = "ERR_co_InvalidActData";

        /// <summary>
        /// Validacion inconsistente. La combinación "Actividad({0}) - Linea Presupuestal({1}) no es válida"
        /// </summary>
        public static string Err_Co_InvalidActLineaPres = "ERR_co_InvalidActLineaPres";

        /// <summary>
        /// El comprobante no es valido"
        /// </summary>
        public static string Err_Co_InvalidComp = "ERR_co_InvalidComp";

        /// <summary>
        /// Validacion inconsistente. La combinación "Cuenta({0}) - Centro Costo({1}) - Operación({2}) no es válida"
        /// </summary>
        public static string Err_Co_InvalidCtaCtoCostoOp = "ERR_co_InvalidCtaCtoCostoOp";

        /// <summary>
        /// La diferencia entre créditos y débitos debe ser 0, tanto para la moneda local como para la extranjera"
        /// </summary>
        public static string Err_Co_InvalidDebCred = "ERR_co_InvalidDebCred";

        /// <summary>
        /// "La relacion de los campos del detalle no corresponden con el documento externo"
        /// </summary>
        public static string Err_Co_InvalidDocExtData = "ERR_co_InvalidDocExtData";

        /// <summary>
        /// "La relacion de los campos del detalle no corresponden con el documento interno"
        /// </summary>
        public static string Err_Co_InvalidDocIntData = "ERR_co_InvalidDocIntData";

        /// <summary>
        /// "El identificador TR no corresponde para el control de saldos de tipo cuenta"
        /// </summary>
        public static string Err_Co_InvalidIdentTR_Cta = "ERR_co_InvalidIdentTR_Cta";

        /// <summary>
        /// "El identificador TR (componente tercero) no corresponde con el tercero ingresado en el registro"
        /// </summary>
        public static string Err_Co_InvalidIdentTR_Terc = "ERR_co_InvalidIdentTR_Terc";

        /// <summary>
        /// El valor debe corresponder a la base multiplicada por el porcentaje del plan de cuentas"
        /// </summary>
        public static string Err_Co_InvalidImpValue = "ERR_co_InvalidImpValue";

        /// <summary>
        /// El signo del valor en el detalle no corresponde a la naturaleza de la cuenta"
        /// </summary>
        public static string Err_Co_InvalidNaturalezaCta = "ERR_co_InvalidNaturalezaCta";

        /// <summary>
        /// "No se ha creado prefijo, para el documento dado"
        /// </summary>
        public static string Err_Co_InvalidPrefDoc = "ERR_co_InvalidPrefDoc";

        /// <summary>
        /// "El valor de los saldos no corresponde a la naturaleza de la cuenta ({0:C})"
        /// </summary>
        public static string Err_Co_InvalidSaldos = "ERR_co_InvalidSaldos";

        /// <summary>
        /// "El valor de los saldos ({0:C}) no corresponde con el valor total importado ({1:C})"
        /// </summary>
        public static string Err_Co_InvalidSaldosImport = "ERR_co_InvalidSaldosImport";

        /// <summary>
        /// "La cuenta debe ser tipo cuenta"
        /// </summary>
        public static string Err_Co_InvalidConcSaldoCta = "ERR_co_InvalidConcSaldoCta";

        /// <summary>
        /// "El concepto de saldo al que se quiere migrar no puede ser de tipo cuenta"
        /// </summary>
        public static string Err_Co_InvalidConcSaldo = "ERR_co_InvalidConcSaldo";

        /// <summary>
        /// "No se pudieron generar saldos para el comprobante {0}-{1} y periodo: {2}"
        /// </summary>
        public static string Err_Co_GenerarSaldos = "ERR_co_GenerarSaldos";

        /// <summary>
        /// "Ha ocurrido un error llenando las cuentas  en la liquidacion de impuestos"
        /// </summary>
        public static string Err_Co_LiquidaImpuestosInvalid = "ERR_co_LiquidaImpuestosInvalid";

        /// <summary>
        /// "No se ha realizado el proceso de ajuste en cambio"
        /// </summary>
        public static string Err_Co_NoAjusteEnCambio = "ERR_co_NoAjusteEnCambio";

        /// <summary>
        /// "El balance no corresponde con los saldos. Debe ejecutar el proceso de mayorizacion"
        /// </summary>
        public static string Err_Co_NoBalance = "ERR_co_NoBalance";

        /// <summary>
        /// "No existe un comprobante de anulacion asignado, para el comprobante {0}"
        /// </summary>
        public static string Err_Co_NoCompAnula = "ERR_co_NoCompAnula";

        /// <summary>
        /// "El comprobante ({0}) no tiene asignado un comprobante para IRFS"
        /// </summary>
        public static string Err_Co_NoCompIFRS = "ERR_co_NoCompIFRS";

        /// <summary>
        /// "ERROR: No se ha generado el consecutivo correspondiente"
        /// </summary>
        public static string Err_Co_NoConsecutivo = "ERR_co_NoConsecutivo";

        /// <summary>
        /// "Error. El documento no ha sido contabilizado"
        /// </summary>
        public static string Err_Co_NoContab = "ERR_co_NoContab";

        /// <summary>
        /// "No se encontro cuenta en cargos de costos. ConceptoCargo: {0} / Linea Presupuesto: {1} / Proyecto: {2} / Centro Costo: {3}"
        /// </summary>
        public static string Err_Co_NoCtaCargoCosto = "ERR_co_NoCtaCargoCosto";

        /// <summary>
        /// "No existe información de datos anuales para la retencion en la fuente"
        /// </summary>
        public static string Err_Co_NoDatosAnuales = "Err_Co_NoDatosAnuales";

        /// <summary>
        /// "No se encontro operacion en el proyecto ({0}) ni en el centro de costo ({1})"
        /// </summary>
        public static string Err_Co_NoOper = "ERR_co_NoOper";
        
        /// <summary>
        /// "No puede correr el cierre mensual sin haber corrido el proceso de prorateo de IVA"
        /// </summary>
        public static string Err_Co_NoProrateoIVA = "ERR_co_NoProrateoIVA";

        /// <summary>
        /// "No existe una tasa de Cambio asignada"
        /// </summary>
        public static string Err_Co_NoTasaCambio = "ERR_co_NoTasaCambio";

        /// <summary>
        /// "No se puede cerrar el modulo con documentos pendientes"
        /// </summary>
        public static string Err_Co_PendingDocs = "ERR_co_PendingDocs";

        /// <summary>
        /// "El proyecto {0} no tiene una actividad relacionada	para la validación de consistencia presupuestal"
        /// </summary>
        public static string Err_Co_ProyNoAct = "ERR_co_ProyNoAct";

        /// <summary>
        /// "No se pudieron sustraer saldos para el comprobante {0} ({1}) y periodo: {2}"
        /// </summary>
        public static string Err_Co_SustraerSaldos = "ERR_co_GenerarSaldos";

        /// <summary>
        /// "El tercero de la CxP tiene radicación Directa, por tanto solo acepta Conceptos de Cargo de Tipo Servicio Público"
        /// </summary>
        public static string Err_Co_TerceroRadicaDirecto = "ERR_co_TerceroRadicaDirecto";

        /// <summary>
        /// "La Cuenta {0} es Invalida o no existe"
        /// </summary>
        public static string Err_Co_CuentaInvalid = "ERR_co_CuentaInvalid";

        /// <summary>
        /// "No se pudo obtener el porcentaje de impuesto de la Cuenta {0}"
        /// </summary>
        public static string Err_Co_PorcentajeImpCuentaInvalid = "ERR_co_PorcentajeImpCuentaInvalid";

        /// <summary>
        /// "El valor modificado requiere que el valor base sea ${0}"
        /// </summary>
        public static string Err_Co_ValorBaseInvalid = "ERR_co_ValorBaseInvalid";

        /// <summary>
        /// "El Concepto saldo de la cuenta {0} debe ser de tipo Componente Documento"
        /// </summary>
        public static string Err_Co_ConcSaldoContraInvalid = "ERR_co_ConcSaldoContraInvalid";

        /// <summary>
        /// "El documento actual solo permite procesar con un libro funcional, verifique la parametrizacion del comprobante"
        /// </summary>
        public static string Err_Co_LibroFuncionalInvalid = "ERR_co_LibroFuncionalInvalid";

        #endregion

        #region Cuentas x Pagar

        /// <summary>
        /// "Ha ocurrido un error al aprobar el Anticipo"
        /// </summary>
        public static string Err_Cp_AnticipoAprobar = "ERR_cp_AnticipoAprobar";

        /// <summary>
        /// "Ha ocurrido un error al rechazar el Anticipo"
        /// </summary>
        public static string Err_Cp_AnticipoRechazar = "ERR_cp_AnticipoRechazar";

        /// <summary>
        /// "Ha ocurrido un error al actualizar el Anticipo"
        /// </summary>
        public static string Err_Cp_AnticipoUpdate = "ERR_cp_AnticipoUpdate";

        /// <summary>
        /// ""Ha ocurrido un error al ingresar el Anticipo"
        /// </summary>
        public static string Err_Cp_AnticipoAdd = "ERR_cp_AnticipoAdd";

        /// <summary>
        /// "Ha ocurrido un error al Aprobar la caja menor"
        /// </summary>
        public static string Err_Cp_CajaAprobar = "ERR_cp_CajaAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar la caja menor"
        /// </summary>
        public static string Err_Cp_CajaRechazar = "ERR_cp_CajaRechazar";

        /// <summary>
        /// "No se puede cerrar el mes con facturas radicadas sin causar"
        /// </summary>
        public static string Err_Cp_CierreFactRadicadas = "ERR_cp_CierreFactRadicadas";

        /// <summary>
        /// "La factura no puede ser causada, por favor verifique que no haya sido aprobada o devuelta"
        /// </summary>
        public static string Err_Cp_FacturaCausar = "ERR_cp_FacturaCausar";

        /// <summary>
        /// "Ha ocurrido un error al devolver la factura"
        /// </summary>
        public static string Err_Cp_FacturaReturn = "ERR_cp_FacturaReturn";

        /// <summary>
        /// " Ha ocurrido un error al ingresar o radicar la factura"
        /// </summary>
        public static string Err_Cp_FacturaRadicate = "ERR_cp_FacturaRadicate";

        /// <summary>
        /// "Ha ocurrido un error al actualizar la factura"
        /// </summary>
        public static string Err_Cp_FacturaUpdate = "ERR_cp_FacturaUpdate";

        /// <summary>
        /// "El concepto de cuenta por pagar no tiene un comprobante relacionado"
        /// </summary>
        public static string Err_Cp_InvalidCompConcCxP = "ERR_cp_InvalidCompConcCxPr";

        /// <summary>
        /// "La cuenta ({0}) asociada al concepto de CxP ({1}) debe corresponder a un documento externo"
        /// </summary>
        public static string Err_Cp_InvalidCtaConcCxP = "ERR_cp_InvalidCtaConcCxP";

        /// <summary>
        /// "Para realizar la provision los módulos de contabilidad e impuestos deben estar abiertos para el periodo {0}"
        /// </summary>
        public static string Err_Cp_InvalidPeriodoProvisiones = "ERR_cp_InvalidPeriodoProvisiones";

        /// <summary>
        /// "Ha ocurrido un error al Aprobar la legalizacion de gastos"
        /// </summary>
        public static string Err_Cp_LegalizacionAprobar = "ERR_cp_LegalizacionAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar la legalizacion de gastos"
        /// </summary>
        public static string Err_Cp_LegalizacionRechazar = "ERR_cp_LegalizacionRechazar";

        /// <summary>
        /// "No existe el concepto de CxP"
        /// </summary>
        public static string Err_Cp_NoConcCxP = "ERR_cp_NoConcCxP";

        /// <summary>
        /// "No existe el documento Contable para leg. de Tarjetas de Credito"
        /// </summary>
        public static string Err_Cp_NoDocContable = "ERR_cp_NoDocContable";

        /// <summary>
        /// "La cuenta asociada al Documento Contable no es de tipo Saldo Componente-Tercero"
        /// </summary>
        public static string Err_Cp_CuentaDocInvalid = "ERR_cp_CuentaDocContableInvalid";

        /// <summary>
        /// "No existe una Cuenta x pagar para los acreedores varios"
        /// </summary>
        public static string Err_Cp_AccountNotExist = "ERR_cp_AccountNotExist";

        /// <summary>
        ///  "El valor de diferencia no puede ser mayor de $ {0} del valor original"
        /// </summary>
        public static string Err_Cp_VlrFacturaInvalid = "ERR_cp_VlrFacturaInvalid";

       
        #endregion

        #region Facturacion


        /// <summary>
        /// "La cuenta asignada al documento contable del Tipo de Factura debe corresponder a un documento interno"
        /// </summary>
        public static string Err_InvalidCuentaTipoFact = "ERR_fa_InvalidCuentaTipoFact";

        /// <summary>
        /// "El documento asignado al documento contable  del Tipo de Factura debe corresponder al documento actual"
        /// </summary>
        public static string Err_InvalidDocFact = "ERR_fa_InvalidDocFact";

        /// <summary>
        /// "La fecha importada no corresponde a la del periodo actual del modulo de facturacion"
        /// </summary>
        public static string Err_InvalidDateFact = "ERR_InvalidDateFact";

        #endregion

        #region Global

        /// <summary>
        /// "El documento {0} no ha sido aprobado"
        /// </summary>
        public static string Err_Gl_DocChildNoApr = "ERR_gl_DocChildNoApr";

        /// <summary>
        /// "El documento externo ya fue agregado"
        /// </summary>
        public static string Err_Gl_DocExtAdded = "ERR_gl_DocExtAdded";

        /// <summary>
        /// "El documento interno ya fue agregado"
        /// </summary>
        public static string Err_Gl_DocIntAdded = "ERR_gl_DocIntAdded";

        /// <summary>
        /// "El documento {0} debe tener una única actividad asignada en la maestras de actividades de flujo"
        /// </summary>
        public static string Err_Gl_DocMultActivities = "ERR_gl_DocMultActivities";

        /// <summary>
        /// "El documento no ha sido aprobado"
        /// </summary>
        public static string Err_Gl_DocNoApr = "ERR_gl_DocNoApr";

        /// <summary>
        /// "No se puede cerrar el mes con documentos radicadossin aprobar"
        /// </summary>
        public static string Err_Gl_DocRadicados = "ERR_gl_DocRadicados";

        /// <summary>
        /// "El documento tiene padres asociados"
        /// </summary>
        public static string Err_Gl_HasParents = "ERR_gl_HasParents";

        /// <summary>
        /// "La busqueda encontro mas de un resultado"
        /// </summary>
        public static string Err_Gl_MultiDoc = "ERR_gl_MultiDoc";

        /// <summary>
        /// "No se encontro documento de anulacion para el documento {0}"
        /// </summary>
        public static string Err_Gl_NoDocAnula = "ERR_gl_NoDocAnula";

        /// <summary>
        /// "No se puede finalizar un proceso con actividades pendientes"
        /// </summary>
        public static string Err_Gl_ProcessWithActivities = "ERR_gl_ProcessWithActivities";

        /// <summary>
        /// "Error actualizando los hijos"
        /// </summary>
        public static string Err_Gl_UpdChilds = "ERR_gl_UpdChilds";

        #endregion

        #region Inventarios

        /// <summary>
        /// "Ha ocurrido un error al Aprobar el inventario físico de la Bodega"
        /// </summary>
        public static string Err_In_BodegaAprobar = "ERR_in_BodegaAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar el inventario físico de la Bodega"
        /// </summary>
        public static string Err_In_BodegaRechazar = "ERR_in_BodegaRechazar";

        /// <summary>
        /// "El periodo de Invetantario no corresponde con el módulo actual"
        /// </summary>
        public static string Err_In_InvalidPeriod = "ERR_in_InvalidPeriod";

        /// <summary>
        /// "No puede retirar una cantidad superior a las existencias en el saldo"
        /// </summary>
        public static string Err_In_InvalidOutExistSaldo = "ERR_in_InvalidOutExistSaldo";

        /// <summary>
        /// "No se encontro la llave Bodega Contab. ({0}) y Grupo de Inv. ({1}) no existe en la maestra de "Contabilizacion de Existencias""
        /// </summary>
        public static string Err_In_NoKeyInContab = "ERR_in_NoKeyInContab";

        /// <summary>
        /// "Debe tener existencias de saldos y costos para realizar salidas"
        /// </summary>
        public static string Err_In_NotPermittedOut = "ERR_in_NotPermittedOut";

        /// <summary>
        /// "Debe tener existencias de saldos y costos para realizar traslados"
        /// </summary>
        public static string Err_In_NotPermittedTraslate = "ERR_in_NotPermittedTraslate";

        /// <summary>
        /// "Ocurrió un error creando el reporte."
        /// </summary>
        public static string Err_In_ReportFail = "ERR_in_ReportFail";

        /// <summary>
        /// "No existe el Tipo de movimiento de Compras Local y/o Extranjero en tabla de Control"
        /// </summary>
        public static string Err_In_MvtoComprasNotExist = "ERR_in_MvtoComprasNotExist";

        /// <summary>
        /// "No existe el Tipo de movimiento de Ventas Local y/o Extranjero en tabla de Control"
        /// </summary>
        public static string Err_In_MvtoVentasNotExist = "ERR_in_MvtoVentasNotExist";

        /// <summary>
        /// "NO EXISTE la factura de Compra del proveedor"
        /// </summary>
        public static string Err_In_FacturaProvNotExist = "ERR_in_FacturaProvNotExist";

        /// <summary>
        /// "NO EXISTE el Tipo de Movimiento {0} obtenido de la tabla de Control"
        /// </summary>
        public static string Err_In_TipoMovInvNotExist = "ERR_in_tipoMovInvNotExist";

        /// <summary>
        /// "Se requiere una bodega de transito o transaccional para crear el documento de Recibido"
        /// </summary>
        public static string Err_In_BodegaTransRequired = "ERR_in_BodegaTransRequired";

        /// <summary>
        /// "El Tipo de movimiento {0}  no corresponde a movimiento de salidas, verifique nuevamente"
        /// </summary>
        public static string Err_In_MvtoVentasInvalid = "ERR_in_MvtoVentasInvalid";

        #endregion

        #region Nomina
             

        /// <summary>
        /// No ha seleccionado a ningún Empleado
        /// </summary>
        public static string Err_No_EmpleadoSelect = "ERR_no_EmpleadoSelect";

        /// <summary>
        /// Error al liquidar Sueldo : {0}
        /// </summary>
        public static string Err_No_liquidarSueldo = "ERR_no_LiquidarSueldo";

        /// <summary>
        /// Error al liquidar Novedades de Contrato : {0}
        /// </summary>
        public static string Err_No_liqudarNovedadesContrato = "ERR_no_LiqudarNovedadesContrato";

        /// <summary>
        /// Error al liquidar Novedades Nomina : {0}
        /// </summary>
        public static string Err_No_liquidarNovedadesNomina = "ERR_no_LiquidarNovedadesNomina";        

        /// <summary>
        /// Error al liquidar Subsidio de transporte : {0}
        /// </summary>
        public static string Err_No_liquidarSubsidioTransporte = "ERR_no_LiquidarSubsidioTransporte";
        
        /// <summary>
        /// Error al liquidar aportes a Salud : {0}
        /// </summary>
        public static string Err_No_liquidarAportesSalud = "ERR_no_LiquidarAportesSalud";

        /// <summary>
        /// Error al liquidar aportes a Pensión : {0}
        /// </summary>
        public static string Err_No_liquidarAportesPension = "ERR_no_LiquidarAportesPension";
        
        /// <summary>
        /// Error al liquidar aportes a Fondo de Solidaridad : {0}
        /// </summary>
        public static string Err_No_liquidarSolidaridad = "ERR_no_LiquidarSolidaridad";

        /// <summary>
        /// Error al liquidar retención en la Fuente : {0}
        /// </summary>
        public static string Err_No_liquidarRetencionFuente = "ERR_no_LiquidarRetencionFuente";

        /// <summary>
        /// Error al liquidar vacaciones : {0}
        /// </summary>
        public static string Err_No_liquidarVacaciones = "ERR_no_LiquidarVacaciones";

        /// <summary>
        /// Error al liquidar Cesantias del Empleado : {0}
        /// </summary>
        public static string Err_No_liquidarCesantias = "ERR_no_LiquidarCesantias";
        
        /// <summary>
        /// Error al liquidar la Prima del Empleado : {0}
        /// </summary>
        public static string Err_No_liquidarPrima = "ERR_no_LiquidarPrima";

        /// <summary>
        /// Error al liquidar Intereses de Cesantias del Empleado : {0}
        /// </summary>
        public static string Err_No_liquidarIntCesantias = "ERR_no_LiquidarIntCesantias";

        /// <summary>
        /// Error al liquidar prestamos : {0}
        /// </summary>
        public static string Err_No_liquidarPrestamos = "ERR_no_LiquidarPrestamos";

        /// <summary>
        /// Error al generar documento de liquidación del empleado {0} : {1}
        /// </summary>
        public static string Err_No_GenerarDocumento = "ERR_no_GenerarDocumento";

        /// <summary>
        /// No se ha creado ningún documento de liquidación para el empleado {0} / {1}
        /// </summary>
        public static string Err_No_NoDocumentoLiq = "ERR_No_NoDocumentoLiq";

        /// <summary>
        /// Error al liquidar los aportes Obligatorios :  {0}
        /// </summary>
        public static string Err_No_LiquidarAportesObligatorios = "ERR_no_LiquidarAportesObligatorios";

        /// <summary>
        /// Error al aprobar la nómina para el empleado :  {0}
        /// </summary>
        public static string Err_No_AprobarNominaEmpleado = "ERR_no_AprobarNominaEmpleado";

        /// <summary>
        /// Ese tercero ya esta siendo usado
        /// </summary>
        public static string Err_No_TerceroID = "ERR_no_TerceroID";

        /// <summary>
        /// El número de días de vacaciones es mayor que el número de días causados
        /// </summary>
        public static string Err_No_VacacionesDiasCausados = "ERR_no_VacacionesDiasCausados";

        /// <summary>
        /// No se han configurado los datos anuales para la empresa {0}
        /// </summary>
        public static string Err_No_DatosAnuales = "ERR_no_DatosAnuales";

        /// <summary>
        /// "No existe el concepto de Ingreso  : {0}, Debe crearlo inialmente en la maestra Novedad Contrato";
        /// </summary>
        public static string Err_No_existConceptoNovContrato = "ERR_no_ExistConceptoNovContrato";
        
        /// <summary>
        /// "Empleado : {0}, Error en la Formula : {1} del Concepto de Novedad : {2};
        /// </summary>
        public static string Err_No_FormulaNovNomina = "ERR_no_FormulaNovNomina";

        /// <summary>
        /// No hay Liquidaciones pendientes para Aprobación
        /// </summary>
        public static string Err_No_DocLiquidacionPendientes = "ERR_no_DocLiquidacionPendientes";

        /// <summary>
        /// "Empleado : {0}, no existe la tasa de cambio para el día {1}"
        /// </summary>
        public static string Err_No_ExitTasaCambioDia = "ERR_no_ExitTasaCambioDia";

        /// <summary>
        /// "Empleado "{0}", el concepto de nomina asociado al glcontrol "{1}", no existe. "
        /// </summary>
        public static string Err_No_noExistConceptoNom = "ERR_ExistConceptoNom";

        /// <summary>
        /// "Ya se agrego la liquidación por Concepto de Prima como Novedad de Nomina"
        /// </summary>
        public static string Err_No_ExistNovedadNominaPrima = "ERR_ExistNovedadNominaPrima";

        /// <summary>
        /// "Existen liquidaciónes pendientes de Aprobación para el periodo actual";
        /// </summary>
        public static string Err_No_NotExistLiqNomPeriodo = "ERR_no_NotExistLiqNomPeriodo";

        /// <summary>
        /// "Empleado "{0}", La fecha de Ingreso es mayor al periodo de liquidación "
        /// </summary>
        public static string Err_No_FechaFueraRango = "ERR_no_FechaFueraRango";

        
        #endregion

        #region Operaciones Conjuntas

        /// <summary>
        /// "Se presentó un problema generando la información del billing"
        /// </summary>
        public static string Err_Oc_Billing = "ERR_oc_Billing";

        /// <summary>
        /// "Para el periodo {0} ya se realizo proceso de billing y legalización"
        /// </summary>
        public static string Err_Oc_BillingAprobado = "ERR_oc_BillingAprobado";

        /// <summary>
        /// "No existe la relacion Contrato ({0}) - Socio ({1}) - Grupo ({2}) - Tipo ({3}) ... en la maestra de cuentas conjuntas
        /// </summary>
        public static string Err_Oc_InvalidCtaConj = "ERR_oc_InvalidCtaConj";

        /// <summary>
        /// "La participacion en la relacion Periodo ({0}) - Proyecto ({1}) no corrresponde al 100%
        /// </summary>
        public static string Err_Oc_InvalidParticionPorc = "ERR_oc_InvalidParticionPorc";

        /// <summary>
        /// "La participacion en la relacion Periodo ({0}) - Proyecto ({1}) no tiene socio operador
        /// </summary>
        public static string Err_Oc_InvalidParticionSocio = "ERR_oc_InvalidParticionSocio";

        /// <summary>
        /// "El proyecto {0} necesita un pozo valido para las cuentas de la contrapartida
        /// </summary>
        public static string Err_Oc_InvalidPozoProy = "ERR_oc_InvalidPozoProy";

        /// <summary>
        /// "No se encontro la relación para obtener el contrato del proyecto {0}"
        /// </summary>
        public static string Err_Oc_NoContrato = "ERR_oc_NoContrato";

        /// <summary>
        /// "No se encontro cuenta de contrapartida en ocCuentaConjunta (Grupo:{0} - ProyectoTipo:{1} - Socio:{2} - Contrato:{3})
        /// </summary>
        public static string Err_Oc_NoCtaContra = "ERR_oc_NoCtaContra";

        /// <summary>
        /// "No existe información de legalización para el periodo {0}
        /// </summary>
        public static string Err_Oc_NoLegalizacion = "ERR_oc_NoLegalizacion";

        #endregion

        #region Planeacion

        /// <summary>
        /// "El saldo mensual consolidado es insuficiente para el Centro de Costo {0} y Linea Presupuestal {1}"
        /// </summary>
        public static string Err_Pl_SaldoMensualNotAvailable = "ERR_pl_SaldoMensualNotAvailable";

        /// <summary>
        /// "Ha ocurrido un error guardando en plPlaneacionProveedores"
        /// </summary>
        public static string Err_Pl_SavePlaneacionProveedor = "ERR_pl_SavePlaneacionProveedor";

        /// <summary>
        /// "La Actividad digitada no existe"
        /// </summary>
        public static string Err_Pl_ActividadLinea = "ERR_pl_ActividadLinea";

        #endregion

        #region Proveedores

        /// <summary>
        /// "No existe operacion para el proyecto {0}"
        /// </summary>
        public static string Err_Pr_Cierre001 = "ERR_pr_Cierre001";

        /// <summary>
        /// "No existe cuenta en los cargos para la relacion Operacion({0}), ConceptoCargo({1}) y Linea Presupuestal ({2})"
        /// </summary>
        public static string Err_Pr_Cierre002 = "ERR_pr_Cierre002";

        /// <summary>
        /// "No existe una cuenta de Presupuesto solicitada (Control Planeacion)"
        /// </summary>
        public static string Err_Pr_CuentaPresupNotExist = "ERR_pr_CuentaPresupNotExist";

        /// <summary>
        /// "Ha ocurrido un error al Aprobar la solicitud"
        /// </summary>
        public static string Err_Pr_SolicitudAprobar = "ERR_pr_SolicitudAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Asignar la solicitud"
        /// </summary>
        public static string Err_Pr_SolicitudAsignar = "ERR_pr_SolicitudAsignar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar la solicitud"
        /// </summary>
        public static string Err_Pr_SolicitudRechazar = "ERR_pr_SolicitudRechazar";

        /// <summary>
        /// "Ha ocurrido un error al Aprobar el Orden de Compra"
        /// </summary>
        public static string Err_Pr_OrdenCompraAprobar = "ERR_pr_OrdenCompraAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar el Orden de Compra"
        /// </summary>
        public static string Err_Pr_OrdenCompraRechazar = "ERR_pr_OrdenCompraRechazar";

        /// <summary>
        /// "Ha ocurrido un error al generar info para el detalle cargo "
        /// </summary>
        public static string Err_Pr_DetalleCargo = "ERR_pr_DetalleCargo";

        /// <summary>
        /// "Ha ocurrido un error al Aprobar el Recibido"
        /// </summary>
        public static string Err_Pr_RecibidoAprobar = "ERR_pr_RecibidoAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar el Recibido"
        /// </summary>
        public static string Err_Pr_RecibidoRechazar = "ERR_pr_RecibidoRechazar";

        /// <summary>
        /// "Ha ocurrido un error al Aprobar el Contrato"
        /// </summary>
        public static string Err_Pr_ContratoAprobar = "ERR_pr_ContratoAprobar";

        /// <summary>
        /// "Ha ocurrido un error al Rechazar el Contrato"
        /// </summary>
        public static string Err_Pr_ContratoRechazar = "ERR_pr_ContratoRechazar";

        /// <summary>
        /// "El saldo a mover no puede ser mayor del que existe en el documento (prSaldosDocu)"
        /// </summary>
        public static string Err_Pr_SaldoInvalid  = "ERR_pr_SaldoInvalid";

        /// <summary>
        /// "EL Documento de Transporte digitado ya existe, ingrese uno nuevo"
        /// </summary>
        public static string Err_Pr_DocTransporteExist = "ERR_pr_DocTransporteExist";

        /// <summary>
        ///   "Error en parametrizacion de usuarios"
        /// </summary>
        public static string Err_Pr_DataUserIncomplete = "ERR_pr_DataUserIncomplete";
      
        /// <summary>
        ///  "No hay niveles de aprobacion parametrizados para este documento"
        /// </summary>
        public static string Err_Pr_NivelApproveNotExist = "ERR_pr_NivelApproveNotExist";

        /// <summary>
        ///  "No hay niveles de aprobacion parametrizados para el Valor de este documento"
        /// </summary>
        public static string Err_Pr_NivelApproveNotExistByValue = "ERR_pr_NivelApproveNotExistByValue";

        /// <summary>
        ///  "El Tipo de Codigo de Bien y Servicio no puede ser de Inventarios, revise la Clase correspondiente"
        /// </summary>
        public static string Err_Pr_TipoCodigoInvalid = "ERR_pr_TipoCodigoInvalid";

        /// <summary>
        ///  "El documento de Recibido {0} no tiene items de Cargos para realizar la contabilizacion"
        /// </summary>
        public static string Err_Pr_CargosEmptyRecibido = "ERR_pr_CargosEmptyRecibido";

        /// <summary>
        ///  "El Codigo BS {0} con la Referencia {1} ya existe, verifique"
        /// </summary>
        public static string Err_Pr_CodigoBSAlreadyExist = "ERR_pr_CodigoBSAlreadyExist";

        /// <summary>
        ///  "No se pudo actualizar la información del plan de pagos de la Orden de Compra"
        /// </summary>
        public static string Err_Pr_NotUpdatePlanPagos = "ERR_pr_NotUpdatePlanPagos";

        /// <summary>
        ///  "El valor total del plan de pagos no puede ser mayor al valor del Documento"
        /// </summary>
        public static string Err_Pr_InvalidValueCurrent = "ERR_pr_InvalidValueCurrent";

        /// <summary>
        ///  "La fecha debe ser superior a la del registro  de pago anterior"
        /// </summary>
        public static string Err_Pr_InvalidDateCurrent = "ERR_pr_InvalidDateCurrent";
        
       #endregion

        #region Proyectos
        /// <summary>
        /// "El ProyectoID actual ya se encuentra registrado  "
        /// </summary>
        public static string Err_Py_ProyectoAlreadyExist = "Err_py_ProyectoAlreadyExist";

        /// <summary>
        /// "Debe digitar meses de duración del proyecto en Planeacion de Tiempos de entrega diferente de 0 "
        /// </summary>
        public static string Err_Py_ProyectoMesesDiracionInvalid = "ERR_py_ProyectoMesesDiracionInvalid";
        #endregion

        #region Reportes

        /// <summary>
        /// "Error creando el reporte."
        /// </summary>
        public static string Err_ReportCreate = "ERR_ReportCreate";

        /// <summary>
        /// "Error exportando el reporte"
        /// </summary>
        public static string Err_ReportExport = "ERR_ReportExport";

        /// <summary>
        /// "No se generaron los archivos Planos"
        /// </summary>
        public static string Err_Cc_GeneraArchivoPlano = "ERR_Cc_GeneraArchivoPlano";

        /// <summary>
        /// No hay crédito(s) Prevendido(s), No se pudo generar la carta de oferta.
        /// </summary>
        public static string Err_Cc_NoseGeneroReporte = "ERR_Cc_NoseGeneroReporte";

        /// <summary>
        /// "La pagaduria del Centro de Pago no pudo ser exportada por que no se encuetra en el control"
        /// </summary>
        public static string Err_Cc_NoSeGeneraReporte = "ERR_Cc_NoSeGeneraReporte";

        /// <summary>
        /// No se puede generar reporte por que el tipo de reporte "-" es incorrecto, cambie el tipo de reporte que desea Imprimir
        /// </summary>
        public static string Err_Pr_NoSeGeneroReporte = "ERR_Pr_NoSeGeneroReporte";

        /// <summary>
        /// Error generando el reporte, el archivo no se pudo generar debido a que se encuentra en uso, por favor cierre el archivo e intente de nuevo.
        /// </summary>
        public static string Err_NoSeGeneroReporte = "ERR_NoSeGeneroReporte";

        #endregion

        #region Tesoreria

        /// <summary>
        /// "A esta cuenta de banco le quedan {0} cheques, por favor seleccione esta cantidad de pagos o menos"
        /// </summary>
        public static string Err_Ts_CantidadChequesDisponible = "ERR_ts_CantidadChequesDisponible";

        /// <summary>
        /// "Esta cuenta de banco no tiene cheques suficientes, actualice su chequera antes de realizar más pagos"
        /// </summary>
        public static string Err_Ts_ChequesFinalizados = "ERR_ts_ChequesFinalizados";

        /// <summary>
        /// "Para el pago de facturas el modulo se debe encontrar en el periodo actual"
        /// </summary>
        public static string Err_Ts_InvalidPeriod = "ERR_ts_InvalidPeriod";

        /// <summary>
        /// "La moneda origen de una de las cuentas bancarias es inválida"
        /// </summary>
        public static string Err_Ts_MonedaOrigenInvalida = "ERR_ts_MonedaOrigenInvalida";

        /// <summary>
        /// "No hay cheque con ese numero"
        /// </summary>
        public static string Err_Ts_NoCheque = "ERR_ts_NoCheque";

        /// <summary>
        /// "La cuenta origen y destino deben ser distintas"
        /// </summary>
        public static string Err_Ts_NoCuentasDistintas = "ERR_ts_NoCuentasDistintas";

        /// <summary>
        /// "No hay pagos seleccionados, seleccione al menos uno para continuar"
        /// </summary>
        public static string Err_Ts_NoHayPagosSeleccionados = "ERR_ts_NoHayPagosSeleccionados";

        /// <summary>
        /// "No hay una tasa de cambio definida para la fecha actual, no se podrá hacer un pago hasta que esta se actualice"
        /// </summary>
        public static string Err_Ts_NoTasaCambioFechaActual = "ERR_ts_NoTasaCambioFechaActual";

        /// <summary>
        /// "No hay una tasa de cambio definida para la fecha seleccionada"
        /// </summary>
        public static string Err_Ts_NoTasaCambioFechaSeleccionada = "ERR_ts_NoTasaCambioFechaSeleccionada";

        /// <summary>
        /// "No hay recibos de esa caja y con ese numero de recibo"
        /// </summary>
        public static string Err_Ts_NoRecibo = "ERR_ts_NoRecibo";

        /// <summary>
        /// "Ya se han registrado movimientos en los saldos para el pago de la factura. Por favor refresque la información"
        /// </summary>
        public static string Err_Ts_SaldosMvto = "ERR_ts_SaldosMvto";


        #endregion        

        #endregion

        #region Mails

        #region Alarmas

        /// <summary>
        /// "NewAge: Tiene un documento con pendientes"
        /// </summary>
        public static string Mail_AlarmDoc_Subject = "mail_alarmDoc_subject";

        /// <summary>
        /// Asunto para documento con tareas pendientes
        /// </summary>
        public static string Mail_AlarmDoc_Body = "mail_alarmDoc_body";

        #endregion

        #region Documentos

        /// <summary>
        /// "NewAge: Se ha creado un(a) {0}"
        /// </summary>
        public static string Mail_DocCreated_Subject = "mail_DocCreated_subject";

        /// <summary>
        /// "NewAge: Han enviado un documento para aprobacion"
        /// </summary>
        public static string Mail_DocSendToAppr_Subject = "mail_docSendToAppr_subject";

        /// <summary>
        /// "NewAge: Han aprobado un(a) {0}"
        /// </summary>
        public static string Mail_Approved_Subject = "mail_Approved_subject";

        /// <summary>
        /// "NewAge: Han rechazado un(a) {0}"
        /// </summary>
        public static string Mail_Rejected_Subject = "mail_Rejected_subject";

        /// <summary>
        /// "NewAge: Han asignado un(a) {0}"
        /// </summary>
        public static string Mail_Assigned_Subject = "mail_Assigned_subject";

        /// <summary>
        /// "NewAge: Se ha creado un {0}"
        /// </summary>
        public static string Mail_DocCreated_Body = "mail_DocCreated_body";

        /// <summary>
        /// Han enviado un documento para aprobacion
        /// </summary>
        public static string Mail_DocSendToAppr_Body = "mail_docSendToAppr_body";

        /// <summary>
        /// "NewAge: Han aprobado un(a) {0}"
        /// </summary>
        public static string Mail_Approved_Body = "mail_Approved_body";

        /// <summary>
        /// "NewAge: Han rechazado un(a) {0}"
        /// </summary>
        public static string Mail_Rejected_Body = "mail_Rejected_body";

        /// <summary>
        /// "NewAge: Han aprobado un(a) {0}"
        /// </summary>
        public static string Mail_ApprovedComp_Body = "mail_ApprovedComp_body";

        /// <summary>
        /// "NewAge: Han rechazado un(a) {0}"
        /// </summary>
        public static string Mail_RejectedComp_Body = "mail_RejectedComp_body";

        /// <summary>
        /// "NewAge: Han aprobado un(a) {0}"
        /// </summary>
        public static string Mail_SentToAprobCartera_Body = "mail_SentToAprobCartera_body";

        /// <summary>
        /// "NewAge: Han aprobado un(a) {0}"
        /// </summary>
        public static string Mail_ApprovedCartera_Body = "mail_ApprovedCartera_body";

        /// <summary>
        /// "NewAge: Han rechazado un(a) {0}"
        /// </summary>
        public static string Mail_RejectedCartera_Body = "mail_RejectedCartera_body";

        /// <summary>
        /// "Un(a) {0} ha sido asignado(a) <br /><br />{1}: {2}<br />Nro: {3}<br />Cantidad de elementos: {4}<br />Periodo: {5}"
        /// </summary>
        public static string Mail_Assigned_Body = "mail_Assigned_body";

        #endregion

        #region Usuarios

        /// <summary>
        /// "NewAge: te han asignado un nuevo usuario"
        /// </summary>
        public static string Mail_NewUser_Subject = "mail_newuser_subject";

        /// <summary>
        /// "NewAge: te han asignado un nuevo usuario"
        /// </summary>
        public static string Mail_NewUser_Body = "mail_newuser_body";

        /// <summary>
        /// "NewAge: reinicio de contraseña"
        /// </summary>
        public static string Mail_ResetPassword_Subject = "mail_resetPassword_subject";

        /// <summary>
        /// "NewAge: reinicio de contraseña"
        /// </summary>
        public static string Mail_ResetPassword_Body = "mail_resetPassword_body";

        #endregion

        #endregion

        #region Mensajes
        
        #region General

        /// <summary>
        /// "Acción cancelada por el usuario"
        /// </summary>
        public static string ActionCancelUser = "msg_actionCancelUser";

        /// <summary>
        /// "¿Desea actualizar el flujo del documento seleccionado?"
        /// </summary>
        public static string ActualizarFlujo = "msg_actualizarFlujo";

        /// <summary>
        /// "¿Desea crear la delegacion de tareas a el nuevo usuario?"
        /// </summary>
        public static string AddDelegado = "msg_addDelegado";

        /// <summary>
        /// "Alarma "
        /// </summary>
        public static string Alarm = "msg_alarm";

        /// <summary>
        /// "El valor digitado es superior al permitido en el anticipo ({0})"
        /// </summary>
        public static string AnticipoMaxValue = "msg_antMaxValue";

        /// <summary>
        /// "El valor digitado para un anticipo no puede ser inferior a 0"
        /// </summary>
        public static string AnticipoMinValue = "msg_antMinValue";

        /// <summary>
        /// "No se ha ingresado valor maximo para el anticipo"
        /// </summary>
        public static string AnticipoNoMaxValue = "msg_antNoMaxValue";

        /// <summary>
        /// "¿Desea anular el documento seleccionado?"
        /// </summary>
        public static string Anular_Doc = "msg_anular_doc";

        /// <summary>
        /// "¿Desea revertir el documento seleccionado?"
        /// </summary>
        public static string Anular_Register = "msg_anular_register";

        /// <summary>
        /// "Esta seguro que desea anular el documento?"
        /// </summary>
        public static string AvoidDoc = "msg_avoidDoc";

        /// <summary>
        /// "CARGANDO SEGURIDADES DE LA EMPRESA ..."
        /// </summary>
        public static string CargandoInfo = "msg_cargandoInfo";

        /// <summary>
        /// "Desea cambiar la empresa de trabajo actual? Asegurese de guardar los cambios pendientes antes de continuar"
        /// </summary>
        public static string ChangeCompany = "msg_changeCompany";

        /// <summary>
        /// "por favor revisar las lineas"
        /// </summary>
        public static string CheckLines = "msg_checkLines";

        /// <summary>
        /// "Codigo"
        /// </summary>
        public static string Code = "msg_code";

        /// <summary>
        /// "No existen pre-comprobantes con el periodo, comprobante y numero de comprobante seleccionado"
        /// </summary>
        public static string ComprobantesCount = "msg_comprobantesCount";

        /// <summary>
        /// "Empresa Actual: "
        /// </summary>
        public static string ConnectionStatusCompany = "msg_conn_statusCompany";

        /// <summary>
        /// "Conectado a {0}"
        /// </summary>
        public static string ConnectionStatusBar = "msg_conn_status";

        /// <summary>
        /// "Cambiar Empresa de Trabajo"
        /// </summary>
        public static string ConnectionStatusUpdateCompany = "msg_conn_statusUpdateCompany";

        /// <summary>
        /// "Usuario: "
        /// </summary>
        public static string ConnectionStatusUser = "msg_conn_statusUser";

        /// <summary>
        /// "La empresa seleccionada no se encuentra en la tabla control"
        /// </summary>
        public static string ControlCompanyNotFound = "msg_controlCompanyNotFound";

        /// <summary>
        /// "Fecha:"
        /// </summary>
        public static string DateReport = "msg_lblDate";

        /// <summary>
        /// "¿Está seguro que desea eliminar el código {0}?"
        /// </summary>
        public static string Delete_Code = "msg_delete_code";

        /// <summary>
        /// "Tiene registros pendientes de eliminar. ¿Desea continuar?"
        /// </summary>
        public static string Delete_Data = "msg_delete_data";

        /// <summary>
        /// "¿Está seguro que desea eliminar el documento actual?"
        /// </summary>
        public static string Delete_Document = "msg_delete_document";

        /// <summary>
        /// "Debe seleccionar un registro para eliminar"
        /// </summary>
        public static string Delete_InvalidOP = "msg_delete_InvalidOp";

        /// <summary>
        /// "¿Desea eliminar el registro seleccionado?"
        /// </summary>
        public static string Delete_Register = "msg_delete_register";

        /// <summary>
        /// "Descripcion"
        /// </summary>
        public static string Description = "msg_description";

        /// <summary>
        /// "Debe ingresar una observación para la devolución del flujo"
        /// </summary>
        public static string DevolucionFlujoObsVacio = "msg_devolucionFlujoObsVacio";

        /// <summary>
        /// "¿Esta seguro que sedea desactivar el registro actual?"
        /// </summary>
        public static string DisableRegister = "msg_disableRegister";

        /// <summary>
        /// "Documento"
        /// </summary>
        public static string Doc = "msg_doc";

        /// <summary>
        /// "Ya existe un documento externo tercero y documento"
        /// </summary>
        public static string DocExtExists = "msg_docExtExists";

        /// <summary>
        /// "Debe agregar detalles al registro actual"
        /// </summary>
        public static string DocNoDetails = "msg_DocNoDetails";

        /// <summary>
        /// "Documentos"
        /// </summary>
        public static string Document = "msg_document";

        /// <summary>
        /// "El documento tiene nuevos cambios y el temporal no se puede usar"
        /// </summary>
        public static string DocumentChanged = "msg_documentChanged";

        /// <summary>
        /// "Tipo Documento"
        /// </summary>
        public static string DocumentType = "msg_documentType";

        /// <summary>
        /// "Puede ver el documento en"
        /// </summary>
        public static string DocumentUrl = "msg_documentUrl";

        /// <summary>
        /// "Documento Aprobado"
        /// </summary>
        public static string DocumentAprob = "msg_documentAprob";

        /// <summary>
        /// "No tiene permisos para revisar este documento"
        /// </summary>
        public static string DocumentNoAccess = "msg_documentNoAccess";

        /// <summary>
        /// "{0} debe ser un numero decimal'
        /// </summary>
        public static string DoubleField = "msg_DoubleField";

        /// <summary>
        /// "Se ha creado el documento con exito: Prefijo {0} - Nro {}"
        /// </summary>
        public static string DocumentOK = "msg_documentOK";

        /// <summary>
        /// "¿Está seguro de {0} el documento actual? No podra realizar cambios posteriores"
        /// </summary>
        public static string DocumentEstadoConfirm = "msg_documentEstadoConfirm";

        /// <summary>
        /// "¿Está seguro de {0} el documento actual?"
        /// </summary>
        public static string DocumentActionConfirm = "msg_documentActionConfirm";

        /// <summary>
        /// "Vacio"
        /// </summary>
        public static string EmptyField = "msg_emptyField";

        /// <summary>
        /// "Si filtra por Factura es necesario seleccionar un Tercero"
        /// </summary>
        public static string FacturaIsEmpty = "msg_facturaIsEmpty";

        /// <summary>
        /// "Si filtra por Tercero es necesario asignar una Factura"
        /// </summary>
        public static string TerceroIsEmpty = "msg_terceroIsEmpty";

        /// <summary>
        /// "{0} Vacio"
        /// </summary>
        public static string EmptyField_Col = "msg_emptyField_col";

        /// <summary>
        /// "Error"
        /// </summary>
        public static string Error = "msg_error";

        /// <summary>
        /// Por favor no manipule el documento mientras se realiza la exportacion de datos"
        /// </summary>
        public static string ExportingNotHandle = "msg_exportingNotHandle";

        /// <summary>
        /// "Exportando...'
        /// </summary>
        public static string Exporting = "msg_exporting";

        /// <summary>
        /// "No existe"
        /// </summary>
        public static string FieldNotFound = "msg_fieldNotFound";

        /// <summary>
        /// Pais no existe en grilla de edición "{0} no existe"
        /// </summary>
        public static string FieldNotFound_Col = "msg_fieldNotFound_col";

        /// <summary>
        /// "Los filtros deben conectarse con un operador y tener valores"
        /// </summary>
        public static string FilterConnection = "msg_filterconnection";

        /// <summary>
        /// "El código "{0}" no existe'
        /// </summary>
        public static string FkNotFound = "msg_fkNotFound";

        /// <summary>
        /// "El código {0} no es hoja en la jerarquia'
        /// </summary>
        public static string FkNotLeaf = "msg_fkNotLeaf";

        /// <summary>
        /// Poner formato de fecha (dd/mm/yyyy)"
        /// </summary>
        public static string FormatDate = "msg_formatDate";

        /// <summary>
        /// Debe ir un número decimal (ej. 4.5)"
        /// </summary>
        public static string FormatDecimal = "msg_formatDecimal";

        /// <summary>
        /// Formato de numero incorrecto"
        /// </summary>
        public static string FormatInvalidNumber = "msg_formatInvalidNumber";

        /// <summary>
        /// Poner formato de numero entero"
        /// </summary>
        public static string FormatInteger = "msg_formatInteger";

        /// <summary>
        /// Poner formato de numero que no supere 32.767"
        /// </summary>
        public static string FormatLimitNumber = "msg_formatLimitNumber";

        /// <summary>
        /// (byte) Debe ir un número del 1 al 255"
        /// </summary>
        public static string FormatNumberRange = "msg_formatNumberRange";

        /// <summary>
        /// Longitud maxima {0} Excedida"
        /// </summary>
        public static string FormatMaxLength = "msg_formatMaxLength";

        /// <summary>
        /// "Desea traer las cuentas relacionadas?"
        /// </summary>
        public static string GetAccounts = "msg_getAccounts";

        /// <summary>
        /// "Desea traer el documento relacionado?"
        /// </summary>
        public static string GetDocument = "msg_getDocument";

        /// <summary>
        /// "¿Desea Caragr los impuestos para el detalle actual?"
        /// </summary>
        public static string GetTaxes = "msg_getTaxes";

        /// <summary>
        /// "El último grupo no puede terminar con AND ó OR, debido a que no tiene con que asociarse"
        /// </summary>
        public static string GroupValidation = "msg_groupValidation";

        /// <summary>
        /// "Jerarquia"
        /// </summary>
        public static string Hierarchy = "msg_hierarchy";

        /// <summary>
        /// "Seleccionar"
        /// </summary>
        public static string HierarFindSelect = "msg_hierarFindSelect";

        /// <summary>
        /// "Cancelar"
        /// </summary>
        public static string HierarFindCancel = "msg_hierarFindCancel";

        /// <summary>
        /// "Los códigos no tienen la misma longitud que el primer registro"
        /// </summary>
        public static string Import_InvalidCodeLength = "msg_import_invalidCodeLength";

        /// <summary>
        /// "Los datos ingresados tienen longitud superior a la permitida"
        /// </summary>
        public static string Import_InvalidLength = "msg_import_invalidLength";

        /// <summary>
        /// Mensaje de error al importar Linea - Columna "Línea: {0} - Columna: {1}
        /// </summary>
        public static string Import_InvalidLengthFormat = "msg_import_invalidLengthFormat";

        /// <summary>
        /// "No tiene copiadas seguridades de ningun grupo"
        /// </summary>
        public static string Import_NoData = "msg_import_nodata";

        /// <summary>
        /// "No puede importar ningún código jerárquico sin movimiento"
        /// </summary>
        public static string Import_NotHierarchyFather = "msg_import_notHierarchyFather";

        /// <summary>
        /// "¿Está seguro que desea copiar las seguridades del grupo '{0}' al grupo '{1}'?"
        /// </summary>
        public static string Import_Security = "msg_import_security";

        /// <summary>
        /// "La linea copiada no corresponde con la longitud requerrida"
        /// </summary>
        public static string IncompleteLine = "msg_incompleteline";

        /// <summary>
        /// "El {0} no existe"
        /// </summary>
        public static string InvalidCode = "msg_invalidCode";

        /// <summary>
        /// El valor ingresado no es valido"
        /// </summary>
        public static string InvalidComboValue = "msg_invalidComboValue";

        /// <summary>
        /// La empresa ingresada no es valida"
        /// </summary>
        public static string InvalidCompany = "msg_invalidCompany";

        /// <summary>
        /// El pais ingresado no es valido"
        /// </summary>
        public static string InvalidCountry = "msg_invalidCountry";

        /// <summary>
        /// Excel no se pudo iniciar. Compruebe que la instalación de Microsoft Office y las referencias del proyecto son correctas.'"
        /// </summary>
        public static string InvalidCreateWorkSheet = "msg_invalidCreateWorkSheet";

        /// <summary>
        /// "La fecha no corresponde a la relacionada en el documento"
        /// </summary>
        public static string InvalidDate = "msg_invalidDate";

        /// <summary>
        /// "El documento ingresado no existe"
        /// </summary>
        public static string InvalidDocument = "msg_invalidDocument";

        /// <summary>
        /// "Este evento no existe ... hizo click en:"
        /// </summary>
        public static string InvalidEvent = "msg_invalidEvent";

        /// <summary>
        /// "Inválido"
        /// </summary>
        public static string InvalidField = "msg_invalidField";

        /// <summary>
        /// "Longitud no valida"
        /// </summary>
        public static string InvalidFieldLength = "msg_invalidFieldLength";

        /// <summary>
        /// "Debe escoger una moneda diferente a la local"
        /// </summary>
        public static string InvalidForeignCurrency = "msg_invalidForeignCurrency";

        /// <summary>
        /// "Formato incorrecto"
        /// </summary>
        public static string InvalidFormat = "msg_invalidFormat";

        /// <summary>
        /// "El valor digitado es superior al máximo permitido ({0})"
        /// </summary>
        public static string InvalidMaxValue = "msg_invalidMaxValue";

        /// <summary>
        /// "Periodo inválido"
        /// </summary>
        public static string InvalidPeriod = "msg_invalidPeriod";

        /// <summary>
        /// "Debe cerrar el periodo de proceso, para abrir un mes mayor"
        /// </summary>
        public static string InvalidPeriodSelected = "msg_invalidPeriodSelected";

        /// <summary>
        /// "Para realizar la busqueda debe digitar los campos básicos"
        /// </summary>
        public static string InvalidSearchCriteria = "msg_invalidSearchCriteria";

        /// <summary>
        /// La Hoja de trabajo de Excel no pudo ser creada. Compruebe que la instalación de Microsoft Office y las referencias del proyecto son correctas."
        /// </summary>
        public static string InvalidStartExcel = "msg_InvalidStartExcel";

        /// <summary>
        /// "Usuario Invalido"
        /// </summary>
        public static string InvalidUser = "msg_invalidUser";

        /// <summary>
        /// La empresa por defecto del usuario no existe o esta inactiva"
        /// </summary>
        public static string InvalidUserCompany = "msg_invalidUserCompany";

        /// <summary>
        /// "Línea"
        /// </summary>
        public static string LineMessage = "msg_lineMessage";

        /// <summary>
        /// "Ha excedido el numero máximo de intentos para acceder al sistema, por motivos de seguridad el usuario ha sido bloqueado"
        /// </summary>
        public static string LoginBlockUser = "msg_login_blockUser";

        /// <summary>
        /// "Se presentó un problema en la consulta, por favor intente nuevamente o comuniquese con el administrador del sistema"
        /// </summary>
        public static string LoginError = "msg_login_error";

        /// <summary>
        /// "Credenciales inválidas, por favor revise e intente nuevamente"
        /// </summary>
        public static string LoginFailure = "msg_login_failure";

        /// <summary>
        /// "Clave concorrecta. Tiene {0} intentos restantes antes de que el usaurio sea bloqueado"
        /// </summary>
        public static string LoginIncorrectPwd = "msg_login_incorrectPwd";

        /// <summary>
        /// "Los campos de usuario y contraseña son obligatorios, por favor ingrese sus datos"
        /// </summary>
        public static string LoginNoData = "msg_login_noData";

        /// <summary>
        /// "Por motivos de seguridad, en la autenticación, el usaurio ha sido bloqueado, por favor contacte un administrador del sistema"
        /// </summary>
        public static string LoginUserBlocked = "msg_login_userBlocked";

        /// <summary>
        /// "Tiene información que no ha guardado, si selecciona este registro perderia los cambios. ¿Está seguro que desea continuar?"
        /// </summary>
        public static string LostEdit = "msg_lostEdit";

        /// <summary>
        /// "Tiene informacion que no ha guardado, si cierra el formulario perderia los cambios. ¿Está seguro de continuar?"
        /// </summary>
        public static string LostInfo = "msg_lostInfo";

        /// <summary>
        /// "Parámetros"
        /// </summary>
        public static string Master = "msg_master";

        /// <summary>
        /// "Los siguientes valores de {0} no se encuentran registrados:"
        /// </summary>
        public static string MultipleFkNotFound = "msg_multipleFkNotFound";

        /// <summary>
        /// "Se encontro mas de un elemento"
        /// </summary>
        public static string MultipleItems = "msg_multipleItems";

        /// <summary>
        /// "No se puede anular mas de un documento a la vez"
        /// </summary>
        public static string MultiAnular = "msg_multiAnular";

        /// <summary>
        /// "Nombre"
        /// </summary>
        public static string Name = "msg_name";

        /// <summary>
        /// "Nuevo"
        /// </summary>
        public static string New = "msg_new";

        /// <summary>
        /// "¿Desea cambiar la busqueda y actualizar la información cargada?"
        /// </summary>
        public static string NewData = "msg_newData";

        /// <summary>
        /// "Tiene informacion que no ha guardado, está seguro de continuar y perder los cambios?"
        /// </summary>
        public static string NewDocument = "msg_newDocument";

        /// <summary>
        /// "¿Desea consultar con los nuevos parametros de busqueda y perder la consulta actual?"
        /// </summary>
        public static string NewSearch = "msg_newSearch";

        /// <summary>
        /// "¿Desea reiniciar los para,etros de búsqueda y perder la consulta actual?"
        /// </summary>
        public static string NewSearchClean = "msg_newSearchClean";

        /// <summary>
        /// "Nit"
        /// </summary>
        public static string Nit = "msg_nit";

        /// <summary>
        /// "No se pueden agregar datos con el filtro activo"
        /// </summary>
        public static string NoAddInFilter = "msg_noAddInFilter";

        /// <summary>
        /// "Ningún registro copiado"
        /// </summary>
        public static string NoCopyField = "msg_nocopyfield";

        /// <summary>
        /// "La version registrada no maneja la opcion de generación de cubos."
        /// </summary>
        public static string NoCube = "msg_noCube";

        /// <summary>
        /// "No tiene datos para agregar, actualizar o copiar"
        /// </summary>
        public static string NoData = "msg_noData";

        /// <summary>
        /// "La búsqueda no arrojo resultados"
        /// </summary>
        public static string NoDataFound = "msg_noDataFound";

        /// <summary>
        /// "El registro seleccionado es de tipo activo"
        /// </summary>
        public static string NoDocAct = "msg_noDocAct";

        /// <summary>
        /// "El registro seleccionado es de tipo Componente Documento"
        /// </summary>
        public static string NoDocDoc = "msg_noDocDoc";

        /// <summary>
        /// "El registro seleccionado es de tipo cuenta y no tiene documentos relacionados"
        /// </summary>
        public static string NoDocCta = "msg_noDocCta";

        /// <summary>
        /// "El registro seleccionado es de tipo Inventario"
        /// </summary>
        public static string NoDocInv = "msg_noDocInv";

        /// <summary>
        /// "El registro seleccionado es de tipo Componente Tercero"
        /// </summary>
        public static string NoDocTer = "msg_noDocTer";

        /// <summary>
        /// "El codigo de documento ({0}) no tiene relacionado información de estructuras o campos para la exportación"
        /// </summary>
        public static string NoExportacionnDoc = "msg_NoExportacionDoc";

        /// <summary>
        /// "El codigo de documento ({0}) no tiene relacionado información de estructuras o campos para la migración"
        /// </summary>
        public static string NoMigracionDoc = "msg_NoMigracionDoc";

        /// <summary>
        /// "Usted no tiene alarmas pendientes"
        /// </summary>
        public static string NoPendingAlarms = "msg_noPendingAlarms";

        /// <summary>
        /// "No hay criterios de busqueda"
        /// </summary>
        public static string NoSearchCriteria = "msg_noSearchCriteria";

        /// <summary>
        /// "Debe ingresar un valor"
        /// </summary>
        public static string NoValue = "msg_noValue";

        /// <summary>
        /// "Página"
        /// </summary>
        public static string PageReport = "msg_lblPage";

        /// <summary>
        /// "Página {0} de {1}"
        /// </summary>
        public static string Pagging = "msg_pagging";

        /// <summary>
        /// "{0} registros"
        /// </summary>
        public static string PaggingRecords = "msg_paggingrecords";

        /// <summary>
        /// "Tiene alarmas activas de documentos pendientes"
        /// </summary>
        public static string PendingAlarms = "msg_pendingAlarms";

        /// <summary>
        /// Llave con el mensaje de llave primaria ya esta en uso "El Código ya se encuentra registrado"
        /// </summary>
        public static string PkInUse = "msg_pkinuse";

        /// <summary>
        /// "El valor de {0} debe ser un número positivo'
        /// </summary>
        public static string PositiveValue = "msg_positiveValue";

        /// <summary>
        /// "Prefijo"
        /// </summary>
        public static string Prefix = "msg_prefix";

        /// <summary>
        /// "Proceso"
        /// </summary>
        public static string Process = "msg_process";

        /// <summary>
        /// "Este proceso esta siendo ejecutado por otro usuario"
        /// </summary>
        public static string ProcessRunning = "msg_processRunning";

        /// <summary>
        /// "Su clave esta vencida, por favor contáctese con el administrador del sistema"
        /// </summary>
        public static string PwdDefeated = "msg_pwdDefeated";

        /// <summary>
        /// "Su clave vencerá dentro de {0} días, desea actualizarla ahora?"
        /// </summary>
        public static string PwdReminder = "msg_pwdReminder";

        /// <summary>
        /// "La contraseña inresada no es correcta"
        /// </summary>
        public static string PwdInvalid = "msg_pwdInvalid";

        /// <summary>
        /// "Por favor digite todos los campos"
        /// </summary>
        public static string PwdEmptyFields = "msg_pwdEmptyFields";

        /// <summary>
        /// "La nueva clave no corresponde a la digitada en el campo de confirmación"
        /// </summary>
        public static string PwdInvalidConfirm = "msg_pwdInvalidConfirm";

        /// <summary>
        /// "Su clave ha sido actualizada"
        /// </summary>
        public static string PwdUpdated = "msg_pwdUpdated";

        /// <summary>
        /// "Se presento un problema actualizando su clave, por favor intente nuevamente"
        /// </summary>
        public static string PwdUpdatedErr = "msg_pwdUpdatedErr";

        /// <summary>
        /// "La contrasea debe contener minimo 6 caracteres"
        /// </summary>
        public static string PwdInvalidLenght = "msg_pwdInvalidLenght";

        /// <summary>
        /// "La contraseña debe contener letras y numeros"
        /// </summary>
        public static string PwdInvalidFormat = "msg_pwdInvalidFormat";

        /// <summary>
        /// "Consultas"
        /// </summary>
        public static string Query = "msg_query";

        /// <summary>
        /// "Estas seguro de eliminar la consulta?"
        /// </summary>
        public static string QueryDelete = "msg_queryDelete";

        /// <summary>
        /// "Esta Variante no tiene selecciones ó filtros"
        /// </summary>
        public static string QueryEmpty = "msg_queryEmpty";

        /// <summary>
        /// "Desea guardar la consulta?, Si solo desea usarla pulse No"
        /// </summary>
        public static string QuerySave = "msg_querySave";

        /// <summary>
        /// "La consulta actual debe tener un nombre"
        /// </summary>
        public static string QueryNameRequired = "msg_queryNameRequired";

        /// <summary>
        /// "Debe seleccionar un filtro antes de realizar la consulta"
        /// </summary>
        public static string QueryNoFilter = "msg_queryNoFilter";

        /// <summary>
        /// "Leyendo filas"
        /// </summary>
        public static string ReadRows = "msg_readRows";

        /// <summary>
        /// "no se pudo eliminar ...."
        /// </summary>
        public static string RecordDeleteErr = "msg_recordDeleteErr";

        /// <summary>
        /// "Reporte"
        /// </summary>
        public static string Report = "msg_report";

        /// <summary>
        /// "Responsable"
        /// </summary>
        public static string Responsible = "msg_responsible";

        /// <summary>
        /// "Seguro que desea resetar la contraseña de {0}"
        /// </summary>
        public static string ResultPwd = "msg_resultPwd";

        /// <summary>
        /// "La tarea se ejecuto con éxito"
        /// </summary>
        public static string ResultOK = "msg_resultOK";

        /// <summary>
        /// "La grilla no puede quedar sin datos"
        /// </summary>
        public static string RowsNeeded = "msg_rowsNeeded";

        /// <summary>
        /// "Guardando en el servidor"
        /// </summary>
        public static string SavingServer = "msg_savingServer";

        /// <summary>
        /// "Enviando Correos"
        /// </summary>
        public static string SendingMails = "msg_sendingMails";

        /// <summary>
        /// "¿Desea mostrar los datos guardados?"
        /// </summary>
        public static string ShowInfo = "msg_showInfo";

        /// <summary>
        /// "Los datos fueron pegados"
        /// </summary>
        public static string SuccessPaste = "msg_successPaste";

        /// <summary>
        /// "Los datos fueron copiados"
        /// </summary>
        public static string SuccessCopy = "msg_successCopy";

        /// <summary>
        /// "Los datos fueron importados'"
        /// </summary>
        public static string SuccessImport = "msg_successImport";

        /// <summary>
        /// La Plantilla fue generada'"
        /// </summary>
        public static string SuccessTemplate = "msg_successTemplate";

        /// <summary>
        /// "Tarea"
        /// </summary>
        public static string Task = "msg_task";

        /// <summary>
        /// "Tiene informacion que no ha sido guardada, ¿Desea cargarla?"
        /// </summary>
        public static string Temp_LoadData = "msg_temp_loadData";

        /// <summary>
        /// "Agregar Delegado"
        /// </summary>
        public static string Title_AddDelegado = "msg_title_addDelegado";

        /// <summary>
        /// "Anular Documento"
        /// </summary>
        public static string Title_Anular = "msg_title_anular";

        /// <summary>
        /// "Eliminar Registro"
        /// </summary>
        public static string Title_Delete = "msg_title_delete";

        /// <summary>
        /// "Desabilitar Registro"
        /// </summary>
        public static string Title_DesableRegister = "msg_title_disableRegister";

        /// <summary>
        /// "Traer Datos"
        /// </summary>
        public static string Title_GetData = "msg_title_getData";

        /// <summary>
        /// "Copiar Seguridades"
        /// </summary>
        public static string Title_ImportSecurity = "msg_title_importSecurity";

        /// <summary>
        /// "Documento Inválido"
        /// </summary>
        public static string Title_NoDocument = "msg_title_noDocument";

        /// <summary>
        /// "Cargar Temporal
        /// </summary>
        public static string Title_TempLoad = "msg_title_tempLoad";

        /// <summary>
        /// "Asignar nueva contraseña"
        /// </summary>
        public static string Title_ResetPwd = "msg_title_resetpwd";

        /// <summary>
        /// "Mostrar información"
        /// </summary>
        public static string Title_Show = "msg_title_show";

        /// <summary>
        /// "Advertencia"
        /// </summary>
        public static string Title_Warning = "msg_title_Warning";

        /// <summary>
        /// "Doble Click o Intro para seleccionar"
        /// </summary>
        public static string ToolTipGrid = "msg_toolTipGrid";

        /// <summary>
        /// Advertencia"
        /// </summary>
        public static string Type_warning = "msg_type_warning";

        /// <summary>
        ///Error"
        /// </summary>
        public static string Type_error = "msg_type_error";

        /// <summary>
        /// Mensaje"
        /// </summary>
        public static string Type_message = "msg_type_message";

        /// <summary>
        /// Confirmacion"
        /// </summary>
        public static string Type_confirm = "msg_type_confirm";

        /// <summary>
        /// "¿Desea actualizar los datos? Si continua los cambio que tenga se perderan"
        /// </summary>
        public static string UpdateData = "msg_updateData";

        /// <summary>
        /// "¿Está seguro que desea habilitar la actualización de los valores?"
        /// </summary>
        public static string UpdateValues = "msg_updateValues";

        /// <summary>
        /// "Usuario:"
        /// </summary>
        public static string UserReport = "msg_lblUser";

        /// <summary>
        /// El documento es valido"
        /// </summary>
        public static string ValidDoc = "msg_validDoc";

        /// <summary>
        /// "Validando Datos'
        /// </summary>
        public static string ValidatingData = "msg_validatingData";

        /// <summary>
        /// "Ver archivo"
        /// </summary>
        public static string ViewFile = "msg_viewFile";

        /// <summary>
        /// "{0} debe tener un valor diferente de cero'
        /// </summary>
        public static string ZeroField = "msg_zeroField";

        /// <summary>
        /// No ha seleccionado ningún item del detalle
        /// </summary>
        public static string NotSelectedItemDetail = "msg_notSelectedItemDetail";

        /// <summary>
        /// El estado del documento no es valido
        /// </summary>
        public static string EstateInvalid = "msg_estateInvalid";

        /// <summary>
        /// El valor del documento debe ser difernte de $0
        /// </summary>
        public static string ValueDocumentInvalid = "msg_valueDocumentInvalid";

        /// <summary>
        /// "Ver Documento"
        /// </summary>
        public static string ViewDocument = "msg_viewDocument";

        /// <summary>
        /// "Esta factura se encuentra Aprobada,si desea copiar los datos del documento, digite la nueva factura"
        /// </summary>
        public static string CopyDocumentFactura = "msg_copyDocumentFactura";

        #endregion

        #region Activos Fijos

        /// <summary>
        /// "La fecha de la factura seleccionada está fuera del período contable"
        /// </summary>
        public static string Ac_FacturaFueraPeriodo = "msg_ac_FacturaFueraPeriodo";

        /// <summary>
        /// "Es necesario seleccionar al menos un registro."
        /// </summary>
        public static string Ac_NoSelected = "msg_ac_NoSelected";

        /// <summary>
        /// "Ya existe un activo con esa plaqueta."
        /// </summary>
        public static string Ac_DuplicatePlate = "msg_ac_DuplicatePlate";

        /// <summary>
        /// "no corresponde a ningun tipo de movimiento valido"
        /// </summary>
        public static string Ac_InvalidData = "msg_ac_InvalidData";

        /// <summary>
        /// "No a seleccionado ningun filtro"
        /// </summary>
        public static string Ac_noFilterSelected = "msg_ac_NoFilterSelected";

        /// <summary>
        /// "El tipo de movimiento no es valido."
        /// </summary>
        public static string Ac_invalidMove = "msg_ac_InvalidMove";

        /// <summary>
        /// "Debe seleccionar al menos un regstro"
        /// </summary>
        public static string Ac_anyRowSelected = "msg_ac_AnyRowSelected";

        /// <summary>
        /// "El valor residual no puede ser mayor al Valor del activo."
        /// </summary>
        public static string Ac_VrResidualMayor = "msg_ac_VrResidualMayor";

        /// <summary>
        /// "Tipo de movimiento no asignado."
        /// </summary>
        public static string Ac_NoAssignedMove = "msg_ac_NoAssignedMove";

        /// <summary>
        /// "No ha asignado tasa de cambio para el día."
        /// </summary>
        public static string Ac_NoAssignedTasaCamabio = "msg_ac_NoAssignedTasaCamabio";

        /// <summary>
        /// "No ha listado activos para procesar."
        /// </summary>
        public static string Ac_NoActivosList = "msg_ac_NoActivosList";
        
        /// <summary>
        /// "Debe diligenciar las Revelaciones para esta Operación."
        /// </summary>
        public static string Ac_NoRevelaciones = "msg_ac_NoRevelaciones";

        /// <summary>
        /// "Debe seleccionar un Activo contenedor."
        /// </summary>
        public static string Ac_ActContenedor = "msg_ac_ActContenedor";

        /// <summary>
        /// "No ha cargado ningún activo al contenedor"
        /// </summary>
        public static string Ac_NoLoadActContenedor = "msg_ac_NoLoadActContenedor";

        /// <summary>
        /// "No ha seleccionado ningún Activo"
        /// </summary>
        public static string Ac_NoActivoSelect = "msg_ac_NoActivoSelect";

        /// <summary>
        /// "No hay activos asociados a la factura ingresada"
        /// </summary>
        public static string Ac_NoActFact = "msg_ac_NoActFact";

        /// <summary>
        /// "El valor total de los activos ingresados no corresponde al valor de la factura"
        /// </summary>
        public static string Ac_NoSumActFactura = "msg_ac_NoSumActFactura";

        #endregion

        #region Cartera Corporativa

        /// <summary>
        /// "¿Está seguro de actualizar los datos del crédito?"
        /// </summary>
        public static string Cc_ActualizarDatosCredito = "msg_cc_ActualizarDatosCredito";

        /// <summary>
        /// No hay libranzas para el cliente {0} que se hayan liquidado  entre las fechas {1} y {2}
        /// </summary>
        public static string Cc_Cartera_ClienteSinLibranzas = "msg_cc_Cartera_ClienteSinLibranzas";

        /// <summary>
        /// "El crédito debe tener un estado de cuenta"
        /// </summary>
        public static string Cc_Cartera_NoEstadoCuenta = "msg_cc_Vartera_NoEstadoCuenta";

        /// <summary>
        /// "El cliente {0}, no posee creditos con estado de cuenta en cobro jurídico"
        /// </summary>
        public static string Cc_ClienteForCJ = "msg_cc_ClienteForCJ";

        /// <summary>
        /// "El cliente {0}, no posee creditos con estado de cuenta para desistimiento"
        /// </summary>
        public static string Cc_ClienteForDesistimiento = "msg_cc_ClienteForDesistimiento";

        /// <summary>
        /// "El cliente {0}, no posee creditos con estado de cuenta para pago total"
        /// </summary>
        public static string Cc_ClienteForPagoTotal = "msg_cc_ClienteForPagoTotal";

        /// <summary>
        /// "El cliente {0} no tiene una cuenta de banco"
        /// </summary>
        public static string Cc_ClienteSinBanco = "msg_cc_ClienteSinBanco";

        /// <summary>
        /// "El cliente no posee creditos para realizar estados de cuenta"
        /// </summary>
        public static string Cc_ClienteSinCreditosEC = "msg_cc_ClienteSinCreditoEC";

        /// <summary>
        /// "El cliente no posee creditos para realizar recaudos"
        /// </summary>
        public static string Cc_ClienteSinCreditoRecaudos = "msg_cc_ClienteSinCreditoRecaudos";

        /// <summary>
        /// "El cliente no posee creditos para realizar abonos de cobro jurídico, acuerdo de pago o acuerdo de apgo incumplido"
        /// </summary>
        public static string Cc_ClienteSinCreditoRecaudosCJ = "msg_cc_ClienteSinCreditoRecaudosCJ";

        /// <summary>
        /// "El cliente ya se encuentra en la lista de datos y require que se especifique la libranza"
        /// </summary>
        public static string Cc_ClienteSinLibranza = "msg_cc_ClienteSinLibranza";

        /// <summary>
        /// "Solo se puede ingresar el codigo de empleado ó el cliente"
        /// </summary>
        public static string Cc_ClienteYCodigo = "msg_cc_ClienteYCodigo";

        /// <summary>
        /// "El comprador de cartera {0}, debe ser diferente al que se encuentra actualmente en el control de cartera"
        /// </summary>
        public static string Cc_CompradorCarteraNotValid = "msg_cc_CompradorCarteraNotValid";

        /// <summary>
        /// "El comprador de cartera {0}, debe ser corresponder al que se encuentra actualmente en el control de cartera"
        /// </summary>
        public static string Cc_CompradorCarteraPropiaNotValid = "msg_cc_CompradorCarteraPropiaNotValid";

        /// <summary>
        /// "El comprador de cartera {0}, no posee ningun valor en los campos de Factor Recompra o Factor Cesion"
        /// </summary>
        public static string Cc_CompradorCarteraNoFactor = "msg_cc_CompradorCarteraNoFactor";

        /// <summary>
        /// "El comprador de cartera {0}, no tiene asociado un portafolio"
        /// </summary>
        public static string Cc_CompradorCarteraNoPortafolio = "msg_cc_CompradorCarteraNoPortafolio";

        /// <summary>
        /// El crédito seleccionado no se encuentra en estado aprobado
        /// </summary>
        public static string Cc_CreditoNoAprob = "msg_cc_CreditoNoAprob";

        /// <summary>
        /// El crédito seleccionado ya fue rechazado
        /// </summary>
        public static string Cc_CreditoRechazado = "msg_cc_CreditoRechazado";

        /// <summary>
        /// "Esta libranza no tiene saldo pendiente"
        /// </summary>
        public static string Cc_CreditoSinSaldos = "msg_cc_CreditoSinSaldos";

        /// <summary>
        /// El crédito {0} ya no tiene saldo para venta
        /// </summary>
        public static string Cc_CreditoSinSaldoVenta = "msg_cc_CreditoSinSaldoVenta";

        /// <summary>
        /// "El credito debe tener la cantida de cuotas vendias"
        /// </summary>
        public static string Cc_CuotasVendidas = "msg_cc_CuotasVendidas";

        /// <summary>
        /// Dejo de operar
        /// </summary>
        public static string Cc_DejoDeOperar = "msg_cc_DejoDeOperar";

        /// <summary>
        /// "Este credito no se puede desincorporar"
        /// </summary>
        public static string Cc_DesIncorporacion_CredNotAllow = "msg_cc_DesIncorporacion_CredNotAllow";

        /// <summary>
        /// El último día ya se cerró
        /// </summary>
        public static string Cc_DiaCerrado = "msg_cc_DiaCerrado";

        /// <summary>
        /// La cantidad de cuotas extras debe ser igual al plazo del crédito
        /// </summary>
        public static string Cc_DifCuotasExtrasPlazo = "msg_cc_DifCuotasExtrasPlazo";

        /// <summary>
        /// "No hay creditos para la distribucion de venta de cartera"
        /// </summary>
        public static string Cc_DistribucionVentaCartera_NoCredito = "msg_cc_DistribucionVentaCartera_NoCredito";

        /// <summary>
        /// "Tiene componentes con errores. Por favor solucioneslos antes de continuar"
        /// </summary>
        public static string Cc_ErrorComponentes = "msg_cc_ErrorComponentes";

        /// <summary>
        /// "Este credito posee un estado de cuenta fijado"
        /// </summary>
        public static string Cc_EstadoCuentaFijado = "msg_cc_EstadoCuentaFijado";

        /// <summary>
        /// "Este credito, no posee un estado de cuenta fijado"
        /// </summary>
        public static string Cc_EstadoCuentaNoFijado = "msg_cc_EstadoCuentaNoFijado";

        /// <summary>
        /// "El estado de cuenta no se puede modificar. Este crédito fue comprado por el crédito {0}"
        /// </summary>
        public static string Cc_EstadoCuentaComprado = "msg_cc_EstadoCuentaComprado";

        /// <summary>
        /// "El estado de cuenta no se puede modificar. Este crédito esta siendo comprado por la solicitud {0}"
        /// </summary>
        public static string Cc_EstadoCuentaEnCompra = "msg_cc_EstadoCuentaEnCompra";

        /// <summary>
        /// "La fecha de apliación debe esta dentro del período actual"
        /// </summary>
        public static string Cc_FechaAplicaInvalid = "msg_cc_FechaAplicaInvalid";

        /// <summary>
        /// "La fecha de pago no puede ser superior a la actual"
        /// </summary>
        public static string Cc_FechaPagoInvalid = "msg_cc_FechaPagoInvalid";

        /// <summary>
        /// "El comprador final debe ser igual, al comprador seleccionado"
        /// </summary>
        public static string Cc_ImpCompInvalid = "msg_cc_ImpCompInvalid";

        /// <summary>
        /// "Debe existir al menos un valor de cliente, codigo de empleado o libranza"
        /// </summary>
        public static string Cc_ImpNoRel = "msg_cc_ImpNoRel";

        /// <summary>
        /// "Debe existir al menos codigo, cedula o nombre del comprador cartera"
        /// </summary>
        public static string Cc_ImpNoComp = "msg_cc_ImpNoComp";

        /// <summary>
        /// "La fecha del la primera cuota, no corresponde a la fecha de la incorporación"
        /// </summary>
        public static string Cc_IncorporacionFechaInvalida = "msg_cc_IncorporacionFechaInvalida";

        /// <summary>
        /// "El comprador de cartera del crédito no corresponde con el comprador seleccionado"
        /// </summary>
        public static string Cc_InvalidCompradorCartera = "msg_cc_InvalidCompradorCartera";

        /// <summary>
        /// "No se puede hacer un estado de cuenta de cobro jurídico para un credito que no sea de cartera propia"
        /// </summary>
        public static string Cc_InvalidEC_CobroJuridico = "msg_cc_InvalidEC_CobroJuridico";

        /// <summary>
        /// "No se puede hacer un estado de cuenta de desistimiento para un credito que no sea de cartera propia"
        /// </summary>
        public static string Cc_InvalidEC_Desistimiento = "msg_cc_InvalidEC_Desistimiento";

        /// <summary>
        /// La póliza del crédito no corresponde con la póliza seleccionada. Por favor refresque la consulta e intente nuevamente
        /// </summary>
        public static string Cc_InvalidECPoliza = "msg_cc_InvalidECPoliza";

        /// <summary>
        /// El propósito del crédito no corresponde con el porpósito seleccionado. Por favor refresque la consulta e intente nuevamente
        /// </summary>
        public static string Cc_InvalidECProposito = "msg_cc_InvalidECProposito";

        /// <summary>
        /// "La fecha de aplicación no corresponde a la nómina migrada"
        /// </summary>
        public static string Cc_InvalidFechaAplica = "msg_cc_invalidFechaAplica";

        /// <summary>
        /// "No se puede repetir la relacion Financiera / Documento'
        /// </summary>
        public static string Cc_InvalidFinDoc = "msg_cc_InvalidFinDoc";

        /// <summary>
        /// Tiene aun otros componentes de pago. ¿Está seguro de cancelar la totalidad del capital?
        /// </summary>
        public static string Cc_InvalidPagoCapital = "msg_cc_InvalidPagoCapital";

        /// <summary>
        /// "La solicitud ya fue aprobada o rechazada"
        /// </summary>
        public static string Cc_InvalidSolicitudEstado = "msg_cc_InvalidSolicitudEstado";

        /// <summary>
        /// "La solicitud ya fue devuelta"
        /// </summary>
        public static string Cc_InvalidSolicitudDevuelta = "msg_cc_InvalidSolicitudDevuelta";

        /// <summary>
        /// "La suma de los valores de los componentes debe ser igual al valo de la libranza"
        /// </summary>
        public static string Cc_InvalidVlrComponentes = "msg_cc_InvalidVlrComponentes";

        /// <summary>
        /// "El valor de la libranza debe ser igual al valor de la cuota por el plazo"
        /// </summary>
        public static string Cc_InvalidVlrLibranza = "msg_cc_InvalidVlrLibranza";

        /// <summary>
        /// "Esta libranza ya se existe en la lista actual"
        /// </summary>
        public static string Cc_LibranzaAdded = "msg_cc_LibranzaAdded";

        /// <summary>
        /// "Esta libranza ya se encuentra aprobada"
        /// </summary>
        public static string Cc_LibranzaAprobada = "msg_cc_LibranzaAprobada";

        /// <summary>
        /// "Esta libranza se encuentra cerrada"
        /// </summary>
        public static string Cc_LibranzaCerrada = "msg_cc_LibranzaCerrada";

        /// <summary>
        /// "Esta libranza ya se encuentra en esta lista"
        /// </summary>
        public static string Cc_LibranzaExiste = "msg_cc_LibranzaExiste";

        /// <summary>
        /// "Se guardó con éxito la libranza número: {0}"
        /// </summary>
        public static string Cc_LibranzaGuardada = "msg_cc_LibranzaGuardada";

        /// <summary>
        /// "Esta libranza no existe, o ya fue aprobada en este documento"
        /// </summary>
        public static string Cc_LibranzaNoDisponible = "msg_cc_LibranzaNoDisponible";

        /// <summary>
        /// "Esta libranza no existe, o no tiene tiene permisos para visualizarla"
        /// </summary>
        public static string Cc_LibranzaNoExiste = "msg_cc_LibranzaNoExiste";

        /// <summary>
        /// "La libranza ({0}) no existe en la lista actual"
        /// </summary>
        public static string Cc_LibranzaNotInList = "msg_cc_LibranzaNotInList";

        /// <summary>
        /// "La libranza {0}, ya se encuentra registrada en el sistema"
        /// </summary>
        public static string Cc_LibranzaRegistrada = "msg_cc_LibranzaRegistrada";

        /// <summary>
        /// "Esta libranza no tiene compras de cartera"
        /// </summary>
        public static string Cc_LibranzaSinCompras = "msg_cc_LibranzaSinCompras";

        /// <summary>
        /// "Esta libranza no tiene compras de cartera pendientes de anticipos"
        /// </summary>
        public static string Cc_LibranzaSinComprasPendientesAnt = "msg_cc_LibranzaSinComprasPendientesAnt";

        /// <summary>
        /// "La libranza {0} ya esta sustituyendo otro crédito"
        /// </summary>
        public static string Cc_LibranzaSustituyeRepetida = "msg_cc_LibranzaSustituyeRepetida";

        /// <summary>
        /// "La linea de credito no tiene componentes"
        /// </summary>
        public static string Cc_LineaCreditoSinComp = "msg_cc_LineaCreditoSinComp";

        /// <summary>
        /// "No se puede guardar la informacion, con respuestas vacia"
        /// </summary>
        public static string Cc_LlamadaSinRespuesta = "msg_cc_LlamadaSinRespuesta";

        /// <summary>
        /// "Se debe digitar la combinación de cobranza (Estado - Gestión) o dejar los 2 campos vacion"
        /// </summary>
        public static string Cc_MigracionEstadoInvalidTuple = "msg_cc_MigracionEstadoInvalidTuple";

        /// <summary>
        /// "Se encontraron multiples créditos pendientes: {0}"
        /// </summary>
        public static string Cc_MultiplesCreditos = "msg_cc_MultiplesCreditos";

        /// <summary>
        /// "No tiene anticipos pendientes de generar"
        /// </summary>
        public static string Cc_NoAnticiposPend = "msg_cc_NoAnticiposPend";

        /// <summary>
        /// "El cliente {0}, no tiene ninguna refencia"
        /// </summary>
        public static string Cc_NoClienteRef = "msg_cc_NoClienteRef";

        /// <summary>
        /// "La relacion de linea de creditos y componentes no tiene el componente comodín {0}"
        /// </summary>
        public static string Cc_NoCompComodin = "msg_cc_NoCompComodin";

        /// <summary>
        /// Deben existir al menos uno o mas componentes
        /// </summary>
        public static string Cc_NoComponentes = "msg_cc_NoComponentes";

        /// <summary>
        /// Los siguientes creditos no poseen un estado de cuenta:
        /// </summary>
        public static string Cc_NoEstadoCuenta = "msg_cc_NoEstadoCuenta";

        /// <summary>
        /// No Opero
        /// </summary>
        public static string Cc_NoOpero = "msg_cc_NoOpero";

        /// <summary>
        /// El crédito {0} no se encuentra vendido
        /// </summary>
        public static string Cc_NoVendido = "msg_cc_NoVendido";

        /// <summary>
        /// "El valor de la nómina ({0}) no corresponde con el valor de las cuotas a pagar ({1}). ¿Desea continuar y enviar el saldo ({2}) a la pagaduria?"
        /// </summary>
        public static string Cc_NominaDifVal = "msg_cc_NominaDifVal";

        /// <summary>
        /// "No tiene informacion de paz y salvos"
        /// </summary>
        public static string Cc_NoPazySalvo = "msg_cc_NoPazySalvo";

        /// <summary>
        /// "La fecha para las cuotas, no puede ser anterior a la fecha actual de la cuota"
        /// </summary>
        public static string Cc_NuevaFechaInvalid = "msg_cc_NuevaFechaInvalid";

        /// <summary>
        /// "No se puede cambiar la fecha de las cuotas, ya que este credito ya posee cuotas pagadas"
        /// </summary>
        public static string Cc_NuevaFechaConPago = "msg_cc_NuevaFechaConPago";

        /// <summary>
        /// La cantidad de cuotas extras es superior al plazo del crédito
        /// </summary>
        public static string Cc_NumCuotasExtras = "msg_cc_NumCuotasExtras";

        /// <summary>
        /// La pagaduria {0} no tiene información de código para exportar
        /// </summary>
        public static string Cc_PagaduriaNoCodExportar = "msg_cc_PagaduriaNoCodExportar";

        /// <summary>
        /// "Debe seleccionar al menos una cuota del flujo para cancelar"
        /// </summary>
        public static string Cc_PagoFlujos_NoPagoSelected = "msg_cc_PagoFlujos_NoPagoSelected";

        /// <summary>
        /// "No se encontraron cuotas a pagar, en la fecha flujo seleccionada"
        /// </summary>
        public static string Cc_PagoFlujos_NoPagoFound = "msg_cc_PagoFlujos_NoPagoFound";

        /// <summary>
        /// Debe digitar un valor de pago
        /// </summary>
        public static string Cc_PagoPositivo = "msg_cc_PagoPositivo";

        /// <summary>
        /// "El valor del pago no puede ser superior al valor del saldo"
        /// </summary>
        public static string Cc_PagoTotalExedido = "msg_cc_PagoTotalExedido";

        /// <summary>
        /// "Va a realizar una pago parical del valor total de la deuda, esta seguro/a que desea realizar este pago"
        /// </summary>
        public static string Cc_PagoTotal_PagoParcial = "msg_cc_PagoTotal_PagoParcial";

        /// <summary>
        /// "No hay creditos para preselecion de venta"
        /// </summary>
        public static string Cc_PreseleccionLibranzas_NoCreditos = "msg_cc_PreseleccionLibranzas_NoCreditos";

        /// <summary>
        /// "No hay creditos disponibles para la preventa"
        /// </summary>
        public static string Cc_PreventaCartera_NoCredito = "msg_cc_PreventaCartera_NoCredito";

        /// <summary>
        /// "La oferta ingresada ya fue usada para una venta. Por favor ingrese una nueva"
        /// </summary>
        public static string Cc_PreventaCartera_OfertaExistente = "msg_cc_PreventaCartera_OfertaExistente";

        /// <summary>
        /// "El credito {0} no ha sido vendido o recomprado"
        /// </summary>
        public static string Cc_QueryLibranza_NoInfoVenta = "msg_cc_QueryLibranza_NoInfoVenta";

        /// <summary>
        /// "El credito {0} no posee informacion de saldos de credito "
        /// </summary>
        public static string Cc_QueryLibranza_NoInfoCartera = "msg_cc_QueryLibranza_NoInfoCartera";

        /// <summary>
        /// "El credito {0}, no posee informacion sobre los componentes"
        /// </summary>
        public static string Cc_QueryLibranza_NoInfoComponentes = "msg_cc_QueryLibranza_NoInfoComponentes";

        /// <summary>
        /// "Tipo de novedad inválida"
        /// </summary>
        public static string Cc_ReincorporacionInvalidTipoNipoNov = "msg_cc_ReincorporacionInvalidTipoNipoNov";

        /// <summary>
        /// "No se puede seleccionar el tipo de novedad {0} con un centro de pago que no tenga habilitado el digito de reincorporación"
        /// </summary>
        public static string Cc_ReincorporacionInvalidTipoNipoNov1 = "msg_cc_ReincorporacionInvalidTipoNipoNov1";

        /// <summary>
        /// "No se puede seleccionar el tipo de novedad {0} con un centro de pago que tenga habilitado el digito de reincorporación"
        /// </summary>
        public static string Cc_ReincorporacionInvalidTipoNipoNov4 = "msg_cc_ReincorporacionInvalidTipoNipoNov4";

        /// <summary>
        /// "El tipo de novedad {0} solo aplica cuando la pagaduria del crédito es deferente a la seleccionada"
        /// </summary>
        public static string Cc_ReincorporacionInvalidTipoNipoNov5 = "msg_cc_ReincorporacionInvalidTipoNipoNov5";

        /// <summary>
        /// "El tipo de novedad seleccionado requiere habilitar el cambio de pagaduría"
        /// </summary>
        public static string Cc_ReincorporacionInvalidTipoNipoNovCambioPag = "msg_cc_ReincorporacionInvalidTipoNipoNovCambioPag";

        /// <summary>
        /// "El tipo de novedad seleccionada no permite incluir crédito con pagadurías diferentes"
        /// </summary>
        public static string Cc_ReincorporacionInvalidTipoNipoNovPagaduriasDif = "msg_cc_ReincorporacionInvalidTipoNipoNovPagaduriasDif";

        /// <summary>
        /// "No se puede renovar la poliza para el crédito solicitado"
        /// </summary>
        public static string Cc_RenovacionPolizaNoDisponible = "msg_cc_RenovacionPolizaNoDisponible";

        /// <summary>
        /// "Debe seleccionar al menos un credito para reasigarle un nuevo comprador de cartera"
        /// </summary>
        public static string Cc_ReasignarCompradorCartera_NoSelected = "msg_cc_ReasignarCompradorCartera_NoSelected";
       
        /// <summary>
        /// Debe ingresar un valor de abono
        /// </summary>
        public static string Cc_RecaudoManual_AbonoNoValue = "msg_cc_RecaudoManual_AbonoNoValue";

        /// <summary>
        /// No puede tener componentes extra con el valor de pago en 0
        /// </summary>
        public static string Cc_RecaudoManual_CompExtraCero = "msg_cc_RecaudoManual_compExtraCero";

        /// <summary>
        /// Falta incluir valores en componentes extra, falta por distribuir ${0}
        /// </summary>
        public static string Cc_RecaudoManual_CompVlrExtra = "msg_cc_RecaudoManual_compVlrExtra";

        /// <summary>
        /// Hay componentes extras sin informacion
        /// </summary>
        public static string Cc_RecaudoManual_CompExtraVacios = "msg_cc_RecaudoManual_compExtraVacios";

        /// <summary>
        /// "El indicador de pago de la cuota, no se encuentra activado"
        /// </summary>
        public static string Cc_RecaudoManual_PagoInd = "msg_cc_RecaudoManual_PagoInd";

        /// <summary>
        /// "Los valores totales no son iguales"
        /// </summary>
        public static string Cc_RecaudoManual_ValoresNotEqual = "msg_cc_RecaudoManual_NotEqual";

        /// <summary>
        /// El crédito {0} se encuenta recomprado
        /// </summary>
        public static string Cc_Recompra = "msg_cc_Recompra";

        /// <summary>
        /// "El comprador no tiene la opción para recomprar con maduración anticipada"
        /// </summary>
        public static string Cc_RecompraCartera_CompradorSinMadAnt = "msg_cc_RecompraCartera_CompradorSinMadAnt";

        /// <summary>
        /// "No hay creditos para recomprar"
        /// </summary>
        public static string Cc_RecompraCartera_NoCreditos = "msg_cc_RecompraCartera_NoCreditos";

        /// <summary>
        /// "No hay creditos para maduración anticipada"
        /// </summary>
        public static string Cc_RecompraCartera_NoCreditosMadAnt = "msg_cc_RecompraCartera_NoCreditosMadAnt";

        /// <summary>
        /// "Debe seleccionar al menos un credito para recomprar"
        /// </summary>
        public static string Cc_RecompraCartera_NoSelected = "msg_cc_RecompraCartera_NoSelected";

        /// <summary>
        /// "El credito a recomprar debe tener un valor de recompra"
        /// </summary>
        public static string Cc_RecompraCartera_VlrRecompra = "msg_cc_RecompraCartera_VlrRecompra";

        /// <summary>
        /// "No puede enviar a aprobación sin completar el mínimo de referencias digitadas ({0})"
        /// </summary>
        public static string Cc_ReferenciasMinimas = "msg_cc_ReferenciasMinimas";

        /// <summary>
        /// El crédito ({0}) ya fue cancelado. Solo puede usar el  tipo de novedad {1}
        /// </summary>
        public static string Cc_ReIncorporacionLibranzaCancelada = "msg_cc_ReIncorporacionLibranzaCancelada";

        /// <summary>
        /// El crédito ({0}) con centro de pago ({1}) ya fue agregada en la línea {2}
        /// </summary>
        public static string Cc_ReIncorporacionPkAdded = "msg_cc_ReIncorporacionPkAdded";

        /// <summary>
        /// El crédito ({0}) ya tiene asignada otro centro de pago para modificar en la línea {1}
        /// </summary>
        public static string Cc_ReIncorporacionMultipleCPMod = "msg_cc_ReIncorporacionMultipleCPMod";

        /// <summary>
        /// El valor del saldo no puede ser negativo
        /// </summary>
        public static string Cc_SaldoPositivo = "msg_cc_SaldoPositivo";

        /// <summary>
        /// Debe seleccionar un centro pago
        /// </summary>
        public static string Cc_SelectCentroPago = "msg_cc_SelectCentroPago";

        /// <summary>
        /// Debe seleccionar un proposito antes de salvar
        /// </summary>
        public static string Cc_SelectProposito = "msg_cc_SelectProposito";

        /// <summary>
        /// "Debe seleccionar el banco para realizar el pago masivo"
        /// </summary>
        public static string Cc_SelectBanco = "msg_cc_SelectBanco";

        /// <summary>
        /// "Todas las solicitudes deben tener un paz y salvo"
        /// </summary>
        public static string Cc_SolictudAnticipos_PazSalvo = "msg_cc_SolictudAnticipos_PazSalvo";

        /// <summary>
        /// El crédito {0} se encuentra sustituido
        /// </summary>
        public static string Cc_Sustituido = "msg_cc_Sustituido";

        /// <summary>
        /// "El credito debe tener un tipo de venta asignado"
        /// </summary>
        public static string Cc_TipoVenta = "msg_cc_TipoVenta";

        /// <summary>
        /// "El credito debe tener una tasa de venta asignada"
        /// </summary>
        public static string Cc_TasaVenta = "msg_cc_TasaVenta";

        /// <summary>
        /// "El tipo de estado solo puede tener los valores 1(Propia) ó 2(Cedida)"
        /// </summary>
        public static string Cc_TipoEstadoConstraint = "msg_cc_TipoEstadoConstraint";

        /// <summary>
        /// El valor total de las cuotas extras no puede ser superior al valor del prestamo 
        /// </summary>
        public static string Cc_VlrCuotasExtras = "msg_cc_VlrCuotasExtras";

        /// <summary>
        /// "El valor digitado, no debe superar el valor solicitado"
        /// </summary>
        public static string Cc_VlrGiroTerceroInvalid = "msg_cc_VlrGiroTerceroInvalid";

        /// <summary>
        /// "El credito debe tener asignado un valor de venta"
        /// </summary>
        public static string Cc_VlrVentaMigracion = "msg_cc_VlrVentaMigracion";

        /// <summary>
        /// "El comprador de cartera {0}, no ha realizado recompras de cartera"
        /// </summary>
        public static string Cc_VentaCartera_CompCarteraNoRecompra = "msg_cc_VentaCartera_CompCarteraNoRecompra";

        /// <summary>
        /// "El comprador de cartera {0}, no posee creditos para la venta con la oferta especificada"
        /// </summary>
        public static string Cc_VentaCartera_CompCarteraNoVenta = "msg_cc_VentaCartera_CompCarteraNoVenta";

        /// <summary>
        /// "El credito a vender debe tener asigando un comprador de cartera"
        /// </summary>
        public static string Cc_VentaCartera_NoComprador = "msg_cc_VentaCartera_NoComprador";

        /// <summary>
        /// "Debe seleccionar al menos un credito para la venta"
        /// </summary>
        public static string Cc_VentaCartera_NoVenta = "msg_cc_VentaCartera_NoSelectedVenta";

        /// <summary>
        /// "La agencia no tiene una zona parametrizada, verifique nuevamente"
        /// </summary>
        public static string Cc_ZonaEmpty = "msg_cc_ZonaEmpty";

        #endregion

        #region Cartera Financiera

        /// <summary>
        /// "El credito tiene mas de una cupta pendiente para pago. Por favor revisar"
        /// </summary>
        public static string Cf_CreditoCJMultiplesCuotas = "msg_cf_CreditoCJMultiplesCuotas";

        /// <summary>
        /// "El credito no tiene un valor solicitado de poliza asignado, esta seguro que desea continuar"
        /// </summary>
        public static string Cf_CreditoSinPoliza = "msg_cf_CreditoSinPoliza";

        /// <summary>
        /// "Debe digitar un cliente o un número de obligación"
        /// </summary>
        public static string Cf_DigitarCliente = "msg_cf_DigitarCliente";

        /// <summary>
        /// "¿Está seguro de enviar los créditos a acuerdo de pago?"
        /// </summary>
        public static string Cf_EnvioAcuerdoPago = "msg_cf_EnvioAcuerdoPago";

        /// <summary>
        /// "¿Está seguro de enviar los créditos a acuerdo de pago incumplido?"
        /// </summary>
        public static string Cf_EnvioAcuerdoPagoIncumplido = "msg_cf_EnvioAcuerdoPagoIncumplido";

        /// <summary>
        /// "La {0} debe ser superior o igual a la {1}"
        /// </summary>
        public static string Cf_FechaVigenciaInvalid = "msg_cf_FechaVigenciaInvalid";

        /// <summary>
        /// "¿Desea reiniciar los valores de la póliza?"
        /// </summary>
        public static string Cf_IniciarPoliza = "msg_cf_IniciarPoliza";

        /// <summary>
        /// "El cliente tiene créditos con un estado de cuenta diferente. Por favor consulte la lista de créditos y verifique la información"
        /// </summary>
        public static string Cf_InvalidClienteEC = "msg_cf_InvalidClienteEC";

        /// <summary>
        /// "No se permite la combinación seleccionada entre el tipo de crédito y el tipo de póliza"
        /// </summary>
        public static string Cf_InvalidPolizaCombinacion = "msg_cf_InvalidPolizaCombinacion";

        /// <summary>
        /// "No se ha registrado una póliza para este solicitud"
        /// </summary>
        public static string Cf_InvalidRegistroCombinacion = "msg_cf_InvalidRegistroPoliza";

        /// <summary>
        /// "La línea de crédito debe tener habilitado el indicador para no permitir desembolso"
        /// </summary>
        public static string Cf_LineaCredNoDesembolso = "msg_cf_LineaCredNoDesembolso";

        /// <summary>
        /// "Debe generar una liquidación para guardar la información"
        /// </summary>
        public static string Cf_LiquidacionRequerida = "msg_cf_LiquidacionRequerida";

        /// <summary>
        /// "La tarea se ejecuto con exito, la obligacion generada es: {0}"
        /// </summary>
        public static string Cf_ObligacionGenerada = "msg_cf_ObligacionGenerada";

        /// <summary>
        /// "La poliza {0} aun no tiene asignada una obligacion"
        /// </summary>
        public static string Cf_PolizaObligNotExist = "msg_cf_PolizaObligNotExist";

        /// <summary>
        /// "El cliente no tiene registrado el Abono del Estado Cuenta"
        /// </summary>
        public static string Cf_SaldoAbonoNotRegist = "msg_cf_SaldoAbonoNotRegist";

        /// <summary>
        /// "El valor del Estado de Cuenta no coincide con el valor reestructurado"
        /// </summary>
        public static string Cf_SaldoAbonoInvalid = "msg_cf_SaldoAbonoInvalid";

        #endregion

        #region Contabilidad

        /// <summary>
        /// "Esta seguro que desea ajustar el comprobante?"
        /// </summary>
        public static string Co_AjustarComp = "msg_co_AjustarComp";

        /// <summary>
        /// "No se pueden modificar cuentas que no sean libres o de controladas por tercero"
        /// </summary>
        public static string Co_AjustarCompInvCta = "msg_co_AjustarCompInvCta";

        /// <summary>
        /// "No se puede realizar el proceso de ajuste en cambio en un periodo superior al 12"
        /// </summary>
        public static string Co_AjustePeriodoInv = "msg_co_AjustePeriodoInv";

        /// <summary>
        /// "El comprobante ha sido anulado"
        /// </summary>
        public static string Co_CancelledComp = "msg_co_CancelledComp";

        /// <summary>
        /// "No existe un comprobante en la relacion documento-prefijo en la maestras de Comprobantes Prefijos"
        /// </summary>
        public static string Co_CompDocPref = "msg_co_CompDocPref";

        /// <summary>
        /// "El consecutivo debe iniciar y aumentar entre comprobantes de a 1 (comprobante {0})"
        /// </summary>
        public static string Co_CompInvalidCompNro = "msg_co_CompInvalidCompNro";

        /// <summary>
        /// "La fecha no corresponde a la del periodo seleccionado (comprobante {0})"
        /// </summary>
        public static string Co_CompInvalidDate = "msg_co_CompInvalidDate";

        /// <summary>
        /// "El libro no corresponde con el libro seleccionado (comprobante {0})"
        /// </summary>
        public static string Co_CompInvalidLibro = "msg_co_CompInvalidLibro";

        /// <summary>
        /// "La moneda no corresponde a la del comprobante {0} (linea {1})"
        /// </summary>
        public static string Co_CompInvalidMda = "msg_co_CompInvalidMda";

        /// <summary>
        /// "La tasa de cambio no corresponde a la del comprobante {0} (linea {1})"
        /// </summary>
        public static string Co_CompInvalidTC = "msg_co_CompInvalidTC";

        /// <summary>
        /// "La cuenta no corresponde a la existente en la relacion (Operación-Conc Cargo-Linea Pres.) en la tabla costos de cargos"
        /// </summary>
        public static string Co_CtaCargoCosto = "msg_co_CtaCargoCosto";

        /// <summary>
        /// "No se encuentra una cuenta con los parametros escritos"
        /// </summary>
        public static string Co_CtaCargoCostoNoExiste = "msg_co_CtaCargoCostoNoExiste";

        /// <summary>
        /// "Se encontro mas de una cuenta con los parametros escritos"
        /// </summary>
        public static string Co_CtaCargoCostoNoUnica = "msg_co_CtaCargoCostoNoUnica";

        /// <summary>
        /// "Una cuenta de IVA debe tener asignada una tarifa"
        /// </summary>
        public static string Co_CtaIVATarifa = "msg_co_CtaIVATarifa";

        /// <summary>
        /// "La llave Cuenta / Centro de Costo / Proyecto debe tener al menos un valor"
        /// </summary>
        public static string Co_DistCompIncomplete = "msg_co_DistCompIncomplete";

        /// <summary>
        /// "La llave Cuenta / Centro de Costo / Proyecto ya existe en esta grilla"
        /// </summary>
        public static string Co_DistCompKeyAdded = "msg_co_DistCompKeyAdded";

        /// <summary>
        /// "El destino no tiene info"
        /// </summary>
        public static string Co_DistCompNoDest = "msg_co_DistCompNoDest";

        /// <summary>
        /// "El documento actual no tiene saldos"
        /// </summary>
        public static string Co_DocNotBalance = "msg_co_DocNotBalance";

        /// <summary>
        /// "La cuenta asignada al concepto de CxP debe corresponder a un documento externo"
        /// </summary>
        public static string Co_IncorrectConcCxPDocExt = "msg_co_IncorrectConcCxPDocExt";

        /// <summary>
        /// "El documento asignado al concepto CxP debe corresponder al documento actual"
        /// </summary>
        public static string Co_IncorrectConcCxPDocID = "msg_co_IncorrectConcCxPDocID";

        /// <summary>
        /// "La cuenta asignada al concepto de CxP debe corresponder a un documento interno"
        /// </summary>
        public static string Co_IncorrectConcCxPDocInt = "msg_co_IncorrectConcCxPDocInt";

        /// <summary>
        /// "El objecto contenido en la papelera de reciclaje con corresponde a un comprobante manual"
        /// </summary>
        public static string Co_InvalidCompPaste = "msg_co_InvalidCompPaste";

        /// <summary>
        /// "El orden solo pueden ser numeros del 1 al 3"
        /// </summary>
        public static string Co_InvalidDistOrder = "msg_co_InvalidDistOrder";

        /// <summary>
        /// "Solo se pueden reclasificar saldos de documentos internos o externos"
        /// </summary>
        public static string Co_InvalidDocReclasifica = "msg_co_InvalidDocReclasifica";

        /// <summary>
        /// "La suma de los totales debe ser 0 para comprobantes que manejen las 2 monedas"
        /// </summary>
        public static string Co_InvalidTotalBiMoneda = "msg_co_InvalidTotalBiMoneda";

        /// <summary>
        /// "La diferencia de creditos y debitos (Moneda Local) no puede ser del mismo signo del saldo"
        /// </summary>
        public static string Co_InvalidSaldoML = "msg_co_InvalidSaldoML";

        /// <summary>
        /// "La diferencia de creditos y debitos (Moneda Extranjera) no puede ser del mismo signo del saldo"
        /// </summary>
        public static string Co_InvalidSaldoME = "msg_co_InvalidSaldoML";

        /// <summary>
        /// "La suma total de creditos y debitos debe ser 0"
        /// </summary>
        public static string Co_InvalidTotal = "msg_co_InvalidTotal";

        /// <summary>
        /// "La suma total de los registros ({0}) no corresponde al valor del documento original. No se tiene en cuenta las contrapartidas"
        /// </summary>
        public static string Co_InvalidTotalAjuste = "msg_co_InvalidTotalAjuste";

        /// <summary>
        /// "El valor de las cuentas de costo ({0}) no corresponden con las del comprobante original ({1})"
        /// </summary>
        public static string Co_InvalidTotalCuentaCosto = "msg_co_InvalidTotalCuentaCosto";

        /// <summary>
        /// "La diferencia de creditos y debitos (Moneda Local) debe corresponder al valor del documento"
        /// </summary>
        public static string Co_InvalidTotalML = "msg_co_InvalidTotalML";

        /// <summary>
        /// "La diferencia de creditos y debitos (Moneda Extranjera) debe corresponder al valor del documento"
        /// </summary>
        public static string Co_InvalidTotalME = "msg_co_InvalidTotalME";

        /// <summary>
        /// "No se puede ajustar el comprobante, no ha sido aprobado"
        /// </summary>
        public static string Co_NotAprovedComprobante = "msg_co_NotAprovedComprobante";

        /// <summary>
        /// "Los impuestos no fueron procesados"
        /// </summary>
        public static string Co_NoImpProcessed = "msg_co_NoImpProcessed";

        /// <summary>
        /// "El documento contable no tiene cuenta asignada"
        /// </summary>
        public static string Co_NocoContCta = "msg_co_NocoContCta";

        /// <summary>
        /// "El documento digitado no corresponde a uno de ajuste de saldos"
        /// </summary>
        public static string Co_NoDocAjSaldos = "msg_co_NoDocAjSaldos";

        /// <summary>
        /// "No se puede eliminar, porque este comprobante no ha sido creado"
        /// </summary>
        public static string Co_NotDeleteComp = "msg_co_NotDeleteComp";

        /// <summary>
        /// "No se ha seleccionado un documento existente"
        /// </summary>
        public static string Co_NoDoc = "msg_co_NoDoc";

        /// <summary>
        /// "No hay ningun comprobante para validar"
        /// </summary>
        public static string Co_NotExistComprobante = "msg_co_NotExistComprobante";

        /// <summary>
        /// "El comprobante ({0}) con numero ({1}), ha sido guardado"
        /// </summary>
        public static string Co_NumberComp = "msg_co_NumberComp";

        /// <summary>
        /// "El documento con numero ({0}), ha sido guardado"
        /// </summary>
        public static string Co_NumberDoc = "msg_co_NumberDoc";

        /// <summary>
        /// "El documento (interno) debe ser numerico"
        /// </summary>
        public static string Co_NumericDocInt = "msg_co_DocInt";

        /// <summary>
        /// "No se puede procesar una declaracion si la fecha no corresponde al periodo de CxP"
        /// </summary>
        public static string Co_ProcDecPeriodo = "msg_co_ProcDecPeriodo";

        /// <summary>
        /// "Esta seguro que desea reiniciar el comprobante actual"
        /// </summary>
        public static string Co_RenewComp = "msg_co_RenewComp";

        /// <summary>
        /// "Esta seguro que desea revertir el comprobante?"
        /// </summary>
        public static string Co_RevertComp = "msg_co_RevertComp";

        /// <summary>
        /// "Revirtiendo comprobante"
        /// </summary>
        public static string Co_Revirtiendo = "msg_co_Revirtiendo";

        /// <summary>
        /// "El documento asignado al Documento Contable {0} no coincide con el documento actual"
        /// </summary>
        public static string Co_IncorrectDocContable = "msg_co_IncorrectDocContable";

        /// <summary>
        /// "La cuenta asignada al Documento Contable {0} debe corresponder a un documento interno"
        /// </summary>
        public static string Co_IncorrectCtaDocContable = "msg_co_IncorrectCtaDocContable";

        #endregion

        #region Cuentas Por Pagar

        /// <summary>
        /// "Abona Anticipo"
        /// </summary>
        public static string Cp_AnticipoAbona = "msg_cp_AnticipoAbona";

        /// <summary>
        /// "El anticipo no esta disponible"
        /// </summary>
        public static string Cp_AnticipoNoDisponible = "msg_cp_AnticipoNoDisponible";

        /// <summary>
        /// "El anticipo no existe"
        /// </summary>
        public static string Cp_AnticipoNoExiste = "msg_cp_AnticipoNoExiste";

        /// <summary>
        /// "El anticipo ha sido enviado para aprobación"
        /// </summary>
        public static string Cp_AnticipoSendApprov = "msg_cp_AnticipoSendApprov";

        /// <summary>
        /// ""El Anticipo ha sido Actualizado""
        /// </summary>
        public static string Cp_AnticipoUpdate = "msg_cp_AnticipoUpdate";

        /// <summary>
        /// "La fecha de Salida no puede ser mayor que la fecha de llegada"
        /// </summary>
        public static string Cp_AnticipoDateStartNotValid = "msg_cp_AnticipoDateStartNotValid";

        /// <summary>
        /// "El valor digitado está fuera del porcentaje del impuesto, por favor reajustelo"
        /// </summary>
        public static string Cp_CajaMenorValueOut = "msg_cp_CajaMenorValueOut";

        /// <summary>
        /// "El Cargo Especial está vacío o  no es válido"
        /// </summary>
        public static string Cp_CargoEspecialEmpty = "msg_cp_CargoEspecialEmpty";

        /// <summary>
        ///  "Este documento no ha sido radicado"
        /// </summary>
        public static string Cp_DocNotRadicate = "msg_cp_DocNotRadicate";

        /// <summary>
        /// "Se ha devuelto la Factura"
        /// </summary>
        public static string Cp_FacturaReturn = "msg_cp_FacturaReturn";

        /// <summary>
        /// "Se ha actualizado la Factura"
        /// </summary>
        public static string Cp_FacturaUpdate = "msg_cp_FacturaUpdate";

        /// <summary>
        /// "Se ha radicado la Factura"
        /// </summary>
        public static string Cp_FacturaRadicate = "msg_cp_FacturaRadicate";

        /// <summary>
        /// "Los siguientes campos son obligatorios : {0}"
        /// </summary>
        public static string Cp_FacturaFieldObligated = "msg_cp_FacturaFieldObligated";

        /// <summary>
        /// "Esta Factura ya ingreso al sistema"
        /// </summary>
        public static string Cp_FacturaAlreadyExist = "msg_cp_FacturaAlreadyExist";

        /// <summary>
        ///  "Debe Ingresar un registro"
        /// </summary>
        public static string Cp_FacturaMustAddRecord = "msg_cp_FacturaMustAddRecord";

        /// <summary>
        ///  "Esta factura ya fue devuelta"
        /// </summary>
        public static string Cp_FacturaAlreadyReturned = "msg_cp_FacturaAlreadyReturned";

        /// <summary>
        /// "El lugar geográfico no puede ser de distribución"
        /// </summary>
        public static string Cp_InvalidLugarGeo = "msg_cp_InvalidLugarGeo";

        /// <summary>
        /// "Solicitud {0} : Cantidad inválida"
        /// </summary>
        public static string Cp_InvalidSolCantidad = "msg_cp_InvalidSolCantidad";

        /// <summary>
        /// "La diferencia de creditos y debitos (Moneda Local - {0:C}) de las cuentas, con indicador de costo, debe corresponder al valor del documento"
        /// </summary>
        public static string Cp_InvalidTotalCostoML = "msg_cp_InvalidTotaCostolML";

        /// <summary>
        /// "La diferencia de creditos y debitos (Moneda Extranjera - {0:C}) de las cuentas, con indicador de costo, debe corresponder al valor del documento"
        /// </summary>
        public static string Cp_InvalidTotalCostoME = "msg_cp_InvalidTotalCostoME";

        /// <summary>
        /// "La fecha de vencimiento no puede ser inferior a la fecha de la factura"
        /// </summary>
        public static string Cp_InvFechaFact = "msg_cp_InvFechaFact";

        /// <summary>
        /// "No existe el concepto CxP para la cuenta relacionada"
        /// </summary>
        public static string Cp_NoCtaCxP = "msg_cp_NoCtaCxP";

        /// <summary>
        /// "No puede devolver Nota Credito"
        /// </summary>
        public static string Cp_NoDevNotaCredito = "msg_cp_NoDevNotaCredito";

        /// <summary>
        /// "No hay facturas con ese tercero y numero de factura"
        /// </summary>
        public static string Cp_NoFact = "msg_cp_NoFact";       

        /// <summary>
        /// "Debe ingresar un numero de factura"
        /// </summary>
        public static string Cp_NoFactNro = "msg_cp_NoFactNro";

        /// <summary>
        /// "La factura digitada se encuentra en un estado diferente a los permitidos"
        /// </summary>
        public static string Cp_NoRadicado = "msg_cp_NoRadicado";

        /// <summary>
        /// "No puede devolver Nota Credito"
        /// </summary>
        public static string Cp_NoReturnNC = "msg_cp_NoReturnNC";

        /// <summary>
        ///  "Debe ingresar un numero de caja"
        /// </summary>
        public static string Cp_NroCajaEmpty = "msg_cp_NroCajaEmpty";

        /// <summary>
        /// "Número de devolución: {0}"
        /// </summary>
        public static string Cp_NumeroDevolucion = "msg_cp_NumeroDevolucion";

        /// <summary>
        /// "Número de radicación: {0}"
        /// </summary>
        public static string Cp_NumeroRadicacion = "msg_cp_NumeroRadicacion";

        /// <summary>
        ///  "Por favor antes de guardar la caja, ingrese el nuevo tercero digitado"
        /// </summary>
        public static string Cp_SaveTerceroBefore = "msg_cp_SaveTerceroBefore";

        /// <summary>
        ///  "El valor del fondo no es suficiente para el valor de la caja (Soportes)"
        /// </summary>
        public static string Cp_ValueFondoInsufficient = "msg_cp_ValueFondoInsufficient";

        /// <summary>
        /// "El valor bruto no puede superior al saldo actual de la factura digitada"
        /// </summary>
        public static string Cp_ValueBrutoInvalid = "msg_cp_ValueBrutoInvalid";

        /// <summary>
        /// "La base del IVA no puede ser mayor al valor bruto, intente nuevamente"
        /// </summary>
        public static string Cp_ValueBaseIVAInvalid = "msg_cp_ValueBaseIVAInvalid";

        /// <summary>
        /// "El valor neto de la legalizacion de gastos debe ser mayor o igual a $0, sino realice un recibo de caja e intente nuevamente"
        /// </summary>
        public static string Cp_ValueLegalInvalid = "msg_cp_ValueLegalInvalid";

        /// <summary>
        /// "No se puede dejar vacio el detalle"
        /// </summary>
        public static string Cp_RowsNeededLegal = "msg_cp_RowsNeededLegal";

        /// <summary>
        /// "No puede legalizar mas de 5 anticipios, seleccione de nuevo"
        /// </summary>
        public static string Cp_AnticiposCountInvalid = "msg_cp_AnticiposCountInvalid";

        /// <summary>
        /// "El resumen de los impuestos no esta aprobado"
        /// </summary>
        public static string Cp_NoResumenImpAprob = "msg_cp_NoResumenImpAprob";

        /// <summary>
        /// "El valor legalizado debe ser igual al valor del anticipo de la tarjeta"
        /// </summary>
        public static string Cp_ValueInvalid = "msg_cp_ValueInvalid";

        /// <summary>
        /// "El año es obligatorio"
        /// </summary>
        public static string Cp_EmptyYear = "msg_cp_EmptyYear";

        /// <summary>
        /// "La factura {0} no existe, vuelva a intentar"
        /// </summary>
        public static string Cp_FacturaInvalid = "msg_cp_FacturaInvalid";

        /// <summary>
        /// "El concepto seleccionado tiene un comprobante diferente al fijado"
        /// </summary>
        public static string Cp_ConceptoCxPInvalid = "msg_cp_ConceptoCxPInvalid";

        /// <summary>
        /// "La factura digitada aun se encuentra en aprobacion"
        /// </summary>
        public static string Cp_FacturaInApprobation = "msg_cp_FacturaInApprobation";

        #endregion

        #region Facturacion

        /// <summary>
        /// "No hay facturas con ese prefijo y numero de recibo"
        /// </summary>
        public static string Fa_NoFacturas = "msg_fa_NoFacturas";

        /// <summary>
        /// "Factura numero ({0}), ha sido guardado"
        /// </summary>
        public static string Fa_FacturaNro = "msg_fa_FacturaNro";

        /// <summary>
        /// "Cuenta ICA no existe"
        /// </summary>
        public static string Fa_NoCuentaIca = "msg_fa_NoCuentaIca";

        /// <summary>
        /// "Valor Neto de la nota credito no puede superar Valor Neto de la factura relacionada"
        /// </summary>
        public static string Fa_InvalidValorNeto = "msg_fa_InvalidValorNeto";

        /// <summary>
        /// "Abona Factura"
        /// </summary>
        public static string Fa_FacturacionAbona = "msg_fa_FacturacionAbona";

        /// <summary>
        /// "El Tipo costeo de la bodega solo puede ser Promedio"
        /// </summary>
        public static string Fa_TipoCosteoOnlyPromedio = "msg_fa_TipoCosteoOnlyPromedio";

        /// <summary>
        ///  "No hay seriales en la bodega {0} con la referencia {1}"
        /// </summary>
        public static string Fa_NotExistSerial = "msg_fa_NotExistSerial";

        /// <summary>
        ///  "Cantidad  disponible es {0}"
        /// </summary>
        public static string Fa_CantityAvailable = "msg_fa_CantityAvailable";
        
        /// <summary>
        ///  "No puede digitar números positivos"
        /// </summary>
        public static string Fa_InvalidNumberPositive = "msg_fa_InvalidNumberPositive";

        /// <summary>
        ///  "Solo puede seleccionar una factura"
        /// </summary>
        public static string Fa_FacturaOnlyOne = "msg_fa_FacturaOnlyOne";

        /// <summary>
        ///  "No hay ningun Servicio configurado en el control de facturación"
        /// </summary>
        public static string Fa_ServicioxDefEmpty = "msg_Fa_ServicioxDefEmpty";

        /// <summary>
        ///  "No puede este Servicio porque es Tipo venta de inventarios y el m'odulo no esta activo"
        /// </summary>
        public static string Fa_ServicioInvalid = "msg_fa_ServicioInvalid";
      
        #endregion

        #region Global

        /// <summary>
        /// "Alarma"
        /// </summary>
        public static string Gl_TareaAlarma = "msg_gl_TareaAlarma";

        /// <summary>
        /// "Cambio Estado {0}"
        /// </summary>
        public static string Gl_TareaCambioEstado = "msg_gl_TareaCambioEstado";

        /// <summary>
        /// "Editado"
        /// </summary>
        public static string Gl_TareaEditado = "msg_gl_TareaEditado";

        /// <summary>
        /// "Nuevo"
        /// </summary>
        public static string Gl_TareaNuevo = "msg_gl_TareaNuevo";

        /// <summary>
        /// "No puede eliminar el registro cuando el campo de SistemaInd está activo"
        /// </summary>
        public static string Gl_ActividadFlujoBorrar = "msg_gl_ActividadFlujoBorrar";

        /// <summary>
        /// "El documento debe Corresponder al Módulo de Cartera"
        /// </summary>
        public static string Gl_ActividadFlujoModuloDiferent = "msg_gl_ActividadFlujoModuloDiferent";

        /// <summary>
        /// "La actividad {0} no tiene una llamada relacionada"
        /// </summary>
        public static string Gl_ActividadFlujoNoLlamada = "msg_gl_ActividadFlujoNoLlamada";

        /// <summary>
        /// "La unidad de tiempo debe estar asignada y diferente de cero"
        /// </summary>
        public static string Gl_ActividadFlujoAlarma = "msg_gl_ActividadFlujoAlarma";

        /// <summary>
        /// "La alarmaPeriodod correspondiente debe tener un valor superior a cero "
        /// </summary>
        public static string Gl_AlarmaPeriodoDiferent = "msg_gl_AlarmaPeriodoAlarmaDiferent";

        /// <summary>
        /// "El m'odulo del documento no coincide con el modulo del concepto saldo"
        /// </summary>
        public static string Gl_ModuleNotCompatible = "msg_gl_ModuleNotCompatible";

        /// <summary>
        /// "Esa Seccion Funcional no existe"
        /// </summary>
        public static string Gl_SeccionNoExiste = "msg_gl_SeccionNoExiste";
        #endregion

        #region Inventarios

        /// <summary>
        /// "Esta referencia no tiene saldos en inventario"
        /// </summary>
        public static string In_NotExistSaldoInventory = "msg_in_NotExistSaldoInventory";

        /// <summary>
        /// "El tipo de medida de las Unidades del empaque y la referencia no son compatibles"
        /// </summary>
        public static string In_UnitInvalidEmp = "msg_in_UnitInvalidEmp";

        /// <summary>
        /// "El serial ya existe"
        /// </summary>
        public static string In_AlreadyExistSerial = "msg_in_AlreadyExistSerial";

        /// <summary>
        /// "El serial ya fue digitado en el documento actual"
        /// </summary>
        public static string In_AlreadyExistSerialCurrent = "msg_in_AlreadyExistSerialCurrent";

        /// <summary>
        /// "El serial no existe para realizar el movimiento de salida"
        /// </summary>
        public static string In_NotExistSerial = "msg_in_NotExistSerial";

        /// <summary>
        /// "No existe una conversión de unidades definida para ésta operación"
        /// </summary>
        public static string In_NotExistConvertUnit = "msg_in_NotExistConvertUnit";

        /// <summary>
        /// "La cantidad de retiro no es válida"
        /// </summary>
        public static string In_InvalidQuantity = "msg_in_InvalidQuantity";

        /// <summary>
        /// "No puede digitar números negativos"
        /// </summary>
        public static string In_InvalidNumber = "msg_in_InvalidNumber";

        /// <summary>
        /// "Error en calculo de valores"
        /// </summary>
        public static string In_InvalidCalculation = "msg_in_InvalidCalculation";

        /// <summary>
        /// "El número de consecutivo no corresponde a la bodega digitada"
        /// </summary>
        public static string In_InvalidNumberxBodega = "msg_in_InvalidNumberxBodega";

        /// <summary>
        /// "No existe el documento con ese número de consecutivo"
        /// </summary>
        public static string In_InvalidNumberTransaccion = "msg_in_InvalidNumberTransaccion";

        /// <summary>
        /// "El consecutivo de este documento ya se encuentra aprobado"
        /// </summary>
        public static string In_NumberAlreadyIsAprob = "msg_in_NumberAlreadyIsAprob";

        /// <summary>
        /// "El presente usuario no tiene permisos de entradas en esta bodega"
        /// </summary>
        public static string In_UserNotPermittedIN = "msg_in_UserNotPermittedIN";

        /// <summary>
        /// "El presente usuario no tiene permisos de salidas o traslados en esta bodega"
        /// </summary>
        public static string In_UserNotPermittedOUTorTRASLATE = "msg_in_UserNotPermittedOUTorTRASLATE";

        /// <summary>
        /// "No puede hacer traslado entre bodegas de Costeo sin costo  a otro tipo de Costeo"
        /// </summary>
        public static string In_NotTraslateBodegas = "msg_in_NotTraslateBodegas";

        /// <summary>
        /// "No puede realizar un traslado a la misma bodega"
        /// </summary>
        public static string In_NotTraslateSameBodega = "msg_in_NotTraslateSameBodega";

        /// <summary>
        /// "El presente usuario no tiene ningún permiso"
        /// </summary>
        public static string In_UserAnyPermission = "msg_in_UserAnyPermission";

        /// <summary>
        /// "El tipo de Proyecto no corresponde al de la bodega"
        /// </summary>
        public static string In_InvalidTipoProyecto = "msg_in_InvalidTipoProyecto";

        /// <summary>
        /// "El Proyecto de Capital de trabajo no corresponde al del proyecto de Capital de trabajo de la bodega"
        /// </summary>
        public static string In_InvalidCapTrabProyecto = "msg_in_InvalidCapTrabProyecto";

        /// <summary>
        /// "Digite un cliente para ver facturas"
        /// </summary>
        public static string In_InvalidCliente = "msg_in_InvalidCliente";

        /// <summary>
        /// "Digite una bodega de origen válida para ver facturas asociadas"
        /// </summary>
        public static string In_InvalidBodegaOrigen = "msg_in_InvalidBodegaOrigen";

        /// <summary>
        /// "No se admiten movimientos de entrada en este documento"
        /// </summary>
        public static string In_InvalidMovimientoIn = "msg_in_InvalidMovimientoIn";

        /// <summary>
        /// "La referencia no puede ser un Kit"
        /// </summary>
        public static string In_ReferenceInvalid = "msg_in_ReferenceInvalid";

        /// <summary>
        /// "Advertencia: El kit no tiene referencias relacionadas"
        /// </summary>
        public static string In_KitWithoutReferences = "msg_in_KitWithoutReferences";

        /// <summary>
        /// "El inventario físico no existe"
        /// </summary>
        public static string In_InventarioFisicoNotExist = "msg_in_InventarioFisicoNotExist";

        /// <summary>
        /// "La cantidad de traslado no puede ser mayor  a la existente"
        /// </summary>
        public static string In_TraslateQuantityInvalid = "msg_in_TraslateQuantityInvalid";

        /// <summary>
        /// "Hay un campo faltante o inválido en la nueva referencia"
        /// </summary>
        public static string In_DataIncompleteNewReferencia = "msg_in_DataIncompleteNewReferencia";

        /// <summary>
        /// "La referencia está vacío o  no es válido"
        /// </summary>
        public static string In_ReferenciaEmpty = "msg_in_ReferenciaEmpty";

        /// <summary>
        /// "El porcentaje total de los items no correponde al 100%"
        /// </summary>
        public static string In_PercentInvalid = "msg_in_PercentInvalid";

        /// <summary>
        /// "Este movimiento de inventarios ya tiene distribución de costos realizada"
        /// </summary>
        public static string In_DistribucionExist = "msg_in_DistribucionExist";

        /// <summary>
        /// "Esta factura ya se uso para asignar a una distribución de costos"
        /// </summary>
        public static string In_FacturaAlreadyDistribucion = "msg_in_FacturaAlreadyDistribucion";

        /// <summary>
        /// "El documento de transporte ingresado no existe"
        /// </summary>
        public static string In_DocTransporteNotExist = "msg_in_DocTransporteNotExist";

        /// <summary>
        ///  "El bodega Origen debe ser Stock y la bodega Destino debe ser de proyecto, verifique"
        /// </summary>
        public static string In_TipoTrasladoConsumoInt = "msg_in_TipoTrasladoConsumoInt";

        #endregion

        #region Nomina

        /// <summary>
        /// Banco Empleado
        /// </summary>
        public static string No_EmpleadoBanco = "msg_no_EmpleadoBanco";

        /// <summary>
        /// ¿Desea cambiar el Banco del Empleado?
        /// </summary>
        public static string No_EmpleadoBancoChange = "msg_no_EmpleadoBancoChange";

        /// <summary>
        /// Tipo Cuenta Empleado
        /// </summary>
        public static string No_EmpleadoTipoCuenta = "msg_no_EmpleadoTipoCuenta";

        /// <summary>
        /// ¿Desea cambiar el Tipo de Cuenta del Empleado?
        /// </summary>
        public static string No_EmpleadoTipoCuentaChange = "msg_no_EmpleadoTipoCuentaChange";
        
        /// <summary>
        /// Cuenta Empleado
        /// </summary>
        public static string No_EmpleadoCuenta = "msg_no_EmpleadoCuenta";

        /// <summary>
        /// ¿Desea cambiar el número de Cuenta del Empleado?
        /// </summary>
        public static string No_EmpleadoCuentaChange = "msg_no_EmpleadoCuentaChange";
                
        /// <summary>
        /// El número de identificación digitado no corresponde a ningun Empleado
        /// </summary>
        public static string No_EmpleadoNotExist = "msg_no_EmpleadoNotExist";
        
        /// <summary>
        /// Debe Digitar un código de Empleado, el campo no puede ser vacio
        /// </summary>
        public static string No_EmpleadoIsEmpty = "msg_EmpleadoIsEmpty";

        /// <summary>
        /// El empleado esta configurado para pago mensual
        /// </summary>
        public static string No_EmpleadoNotLiquidation = "msg_no_EmpleadoNotLiquidation";

        /// <summary>
        /// El Indicador para pago de subsidio de Transporte para la primera quincena esta deshabilitado
        /// </summary>
        public static string No_NominaIndPagoAuxTrans2Quincena = "msg_no_NominaIndPagoAuxTrans2Quincena";

        /// <summary>
        /// "La fecha Final de la novedad no puede ser mayor que la fecha Inicial"
        /// </summary>
        public static string No_NovedadContratoDateStartNotValid = "msg_no_NovedadContratoDateStartNotValid";

        /// <summary>
        /// Ya agrego una novedad con el concepto de nómina seleccionado
        /// </summary>
        public static string No_NovedadNominaExist = "msg_no_NovedadNominaExist";

        /// <summary>
        /// Ya agrego una novedad con el concepto de novedad de contrato y fecha inicial seleccinados
        /// </summary>
        public static string No_NovedadContratoExist = "msg_no_NovedadContratoExist";

        /// <summary>
        /// No ha seleccionado ninguna Novedad de Contrato
        /// </summary>
        public static string No_NovedadContratoSelect = "msg_no_NovedadContratoSelect";
        
        /// <summary>
        /// No se puedo generar el número de Documento para la liquidación del empleado : 
        /// </summary>
        public static string No_LiquidacionPreliminarErrorNumDoc = "msg_no_LiquidacionPreliminarErrorNumDoc";

        /// <summary>
        /// La fecha de nacimiento debe ser menor a la fecha actual
        /// </summary>
        public static string No_FechaNacimientoNotValid = "msg_no_FechaNacimientoNotValid";

        /// <summary>
        /// "La fecha de Salida no puede ser mayor que la fecha de llegada"
        /// </summary>
        public static string No_DateStartNotValid = "msg_no_DateStartNotValid";

        /// <summary>
        /// "El periodo del mes de control no corresponde a los periodos de liquiación de la Prima"
        /// </summary>
        public static string No_PeriodoLiqPrimaNotValid = "msg_no_PeriodoLiqPrimaNotValid";
        
        /// <summary>
        /// ¿ Esta seguro de Liquidar al empleado : {0} ?
        /// </summary>
        public static string No_EmpleadoLiquidarContrato = "msg_no_EmpleadoLiquidarContrato";

        /// <summary>
        /// Liquidación Contrato Empleado
        /// </summary>
        public static string No_EmpleadoLiquidacionContrato = "msg_no_EmpleadoLiquidacionContrato";   
     
        /// <summary>
        /// La liquidación de Cesantias Anuales deben realizarse en el mes de Diciembre
        /// </summary>
        public static string No_LiquidacionAnualCesantias = "msg_no_LiquidacionAnualCesantias";

        /// <summary>
        /// No ha seleccionado ningún Concepto de Nomina
        /// </summary>
        public static string No_ConceptoNOMSelect = "msg_no_ConceptoNOMSelect";

        /// <summary>
        /// Liquidación Planilla Edición
        /// </summary>
        public static string No_EmpleadoEditPlanilla = "msg_No_EmpleadoEditPlanilla";

        /// <summary>
        /// ¿ Desea guardar los cambios realizados en la Planilla del empleado : {0} ?
        /// </summary>
        public static string No_EmpleadoEditPlanillaOK = "msg_no_EmpleadoEditPlanillaOK";

        /// <summary>
        /// No ha ingresado ninguna novedad
        /// </summary>
        public static string No_NovedadContratoNoFound = "msg_no_NovedadContratoNoFound";
               
        /// <summary>
        ///  No se puede eliminar la novedad de contrato, hace parte de una liquidación
        /// </summary>
        public static string No_NovedadContratoDelete = "msg_no_NovedadContratoDelete";

        /// <summary>
        /// "Eliminar Novedad"
        /// </summary>
        public static string No_Title_DeleteNovedad = "msg_title_deleteNovedad";

        /// <summary>
        /// " Esta seguro de eliminar la Novedad de Nomina ? "
        /// </summary>
        public static string No_Body_DeleteNovedad = "msg_body_deleteNovedad";

        /// <summary>
        /// " El valor debe estar entre 0 y {0} "
        /// </summary>
        public static string No_ValidationRange = "msg_ValidationRange";

        /// <summary>
        /// " Va a modificar el valor del registro, Desea Continuar? "
        /// </summary>
        public static string No_VerificationUpdate = "msg_VerificationUpdate";

        /// <summary>
        /// " Modificar Cesantias e Intereses Cesantias "
        /// </summary>
        public static string No_CaptionUpdate = "msg_CaptionUpdate";

        #endregion

        #region Planeacion

        /// <summary>
        /// "'No puede repetir la combinación Línea Presupuesto  y Centro Costo para el documento"
        /// </summary>
        public static string pl_InvalidPresDet = "msg_pl_InvalidPresDet";

        /// <summary>
        /// "No puede repetir la combinación Centro de costo y proyecto"
        /// </summary>
        public static string pl_InvalidPresDoc = "msg_pl_InvalidPresDoc";

        /// <summary>
        /// "El valor de los meses debe corresponder con el valor del movimiento"
        /// </summary>
        public static string pl_InvalidValMes = "msg_pl_InvalidValMes";

        /// <summary>
        /// "Si cambia el periodo va a perder la información pendiente de guardar ¿Desea continuar?"
        /// </summary>
        public static string pl_UpdPeriodo = "msg_pl_UpdPeriodo";

        /// <summary>
        /// "No existe ningun presupuesto con la informacion digitada"
        /// </summary>
        public static string pl_PresupuestoNotExist = "msg_pl_PresupuestoNotExist";

        /// <summary>
        /// "La diferencia de saldos del movimiento debe ser igual a $0 en cualquier origen monetario"
        /// </summary>
        public static string pl_SaldoMvtoInvalid = "msg_pl_SaldoMvtoInvalid";

        /// <summary>
        /// "Debe digitar al menos una valor de movimiento para guardar el documento"
        /// </summary>
        public static string pl_SaveMvtoInvalid = "msg_pl_SaveMvtoInvalid";

        /// <summary>
        /// "Existen documentos de adicion pendientes por aprobar o enviar para aprobacion"
        /// </summary>
        public static string pl_DocAdicionPendientes = "msg_pl_DocAdicionPendientes";

        /// <summary>
        /// "El valor del nuevo saldo no puede ser negativo"
        /// </summary>
        public static string pl_newValueInvalid = "msg_pl_NewValueInvalid";

        /// <summary>
        /// "No existe el saldo suficiente de Linea Presupuesto: {} y Centro Costo: {} para realizar la operacion"
        /// </summary>
        public static string pl_SaldoNotAvailable = "msg_pl_SaldoNotAvailable";

        /// <summary>
        /// "No existe la combinación de Línea Presupuesto y Centro Costo para el traslado automatico"
        /// </summary>
        public static string pl_LineaPresNotExist = "msg_pl_LineaPresNotExist";

        /// <summary>
        /// "No existe la relacion Actividad y linea Presupuesto para obtener el Control de Costo y realizar la distribucion
        /// </summary>
        public static string pl_InvalidKeyActividadLinea = "msg_pl_InvalidKeyActividadLinea";

        /// <summary>
        /// El proyecto no corresponde al tipo de proyecto seleccionado 
        /// </summary>
        public static string pl_InvalidProyecto = "msg_pl_InvalidProyecto";
        
        #endregion

        #region Proveedores

        /// <summary>
        /// "Solicitud no existe"
        /// </summary>
        public static string Pr_NoSolicitudes = "msg_pr_NoSolicitudes";

        /// <summary>
        /// "Datos de los Cargos no es valida"
        /// </summary>
        public static string Pr_InvalidCargos = "msg_pr_InvalidCargos";

        /// <summary>
        /// "Porcentaje debe ser 100%"
        /// </summary>
        public static string Pr_PorcentajeNoCien = "msg_pr_PorcentajeNoCien";

        /// <summary>
        /// "El numero de cargos no puede ser superior a 1 para esta clase de bienes y servicios"
        /// </summary>
        public static string Pr_CantidadDeCargos = "msg_pr_CantidadDeCargos";

        /// <summary>
        /// "Orden de Compra no existe"
        /// </summary>
        public static string Pr_NoOrdenCompra = "msg_pr_NoOrdenCompra";

        /// <summary>
        /// "No puede ingresar 2 servicios con AIU en el documento"
        /// </summary>
        public static string Pr_AIUAlreadyExist = "msg_pr_AIUAlreadyExist";

        /// <summary>
        /// "El valor total del contrato debe ser mayor o igual al total del Plan de Pagos"
        /// </summary>
        public static string Pr_ValueTotalInvalid = "msg_pr_ValueTotalInvalid";

        /// <summary>
        /// "El tipo de servicio no requiere Referencia"
        /// </summary>
        public static string Pr_ReferenciaNotRequired = "msg_pr_ReferenciaNotRequired";

        /// <summary>
        /// "Ésta referencia no se encuentra en el convenio"
        /// </summary>
        public static string Pr_ReferenciaNotExistConvenio = "msg_pr_ReferenciaNotExistConvenio";

        /// <summary>
        /// "Esta orden de compra no tiene convenios"
        /// </summary>
        public static string Pr_NotAlreadyConvenioOrden = "msg_pr_NotAlreadyConvenioOrden";

        /// <summary>
        /// "Lista de Convenios vacía, la orden de compra no tiene valor adicional al contrato"
        /// </summary>
        public static string Pr_ConveniosEmpty = "msg_pr_ConveniosEmpty";

         /// <summary>
        /// "La referencia incluida no corresponde al tipo de Bien Servicio, no es requerida"
        /// </summary>
        public static string Pr_ReferenciaInvalid = "msg_pr_ReferenciaInvalid";

        /// <summary>
        /// "La llave código BienServicio y Referencia está duplicada"
        /// </summary>
        public static string Pr_DuplicateKey = "msg_pr_DuplicateKey";

        /// <summary>
        /// "La fecha del consumo no corresponden a la fecha del documento actual"
        /// </summary>
        public static string Pr_DateInvalid = "msg_pr_DateInvalid";

        /// <summary>
        /// "No existe orden de compra con este prefijo y Nro. de consecutivo"
        /// </summary>
        public static string Pr_NotExistOrdenCompra = "msg_pr_NotExistOrdenCompra";

        /// <summary>
        /// "Esta factura no tiene asociados tipo de cuenta de inventario"
        /// </summary>
        public static string Pr_ServicioNotExistFact = "msg_pr_ServicioNotExistFact";

        /// <summary>
        /// "La bodega es de transito y requiere incluir el Documento de Transporte y Manifiesto de carga"
        /// </summary>
        public static string Pr_TransporteManifiestoEmpty = "msg_pr_TransporteManifiestoEmpty";

        /// <summary>
        /// "No existe un Contrato de Proveedores para esta orden de Compra"
        /// </summary>
        public static string Pr_ContratoNotExist = "msg_pr_ContratoNotExist";

        /// <summary>
        /// "Se requiere una bodega de Transito para recibir esta Orden de Compra(Incoterm)"
        /// </summary>
        public static string Pr_BodegaTransitoRequired = "msg_pr_BodegaTransitoRequired";

        /// <summary>
        /// "Se requiere una bodega de Puerto para recibir esta Orden de Compra(Incoterm)"
        /// </summary>
        public static string Pr_BodegaPuertoRequired = "msg_pr_BodegaPuertoRequired";

        /// <summary>
        /// "El documento con Nro {0} no se puede procesar hasta revertir el documento con Nro {1} el cual tiene relacionado"
        /// </summary>
        public static string Pr_ReversionInvalid = "msg_pr_ReversionInvalid";

        /// <summary>
        /// "No existe el parametro Tipo Codigo de la tabla Clase Bien servicio"
        /// </summary>
        public static string Pr_TipoCodigoNotExist = "msg_pr_TipoCodigoNotExist";

        /// <summary>
        /// "No tiene ningun detalle o recursos relacionados en proveedores"
        /// </summary>
        public static string Pr_DetailProvNotExist = "msg_pr_DetailProvNotExist";

        /// <summary>
        /// "No existe el Codigo BS o Referencia en el proyecto de Solicitud Servicios cargada"
        /// </summary>
        public static string Pr_CodigoBSProyectoNotExist = "msg_pr_CodigoBSProyectoNotExist";

        /// <summary>
        /// "La cantidad solicitada no está disponible en el proyecto de Solicitud de Servicios"
        /// </summary>
        public static string Pr_CantidadSolNotAvailable = "msg_pr_CantidadSolNotAvailable";

        /// <summary>
        /// "No se encontro el Bien Servicio {0} con la Referencia {1} en la lista de Convenios del Contrato relacionado en la Orden de Compra {2}-{3}"
        /// </summary>
        public static string Pr_BienServicioNotAvailable = "msg_pr_BienServicioNotAvailable";
        #endregion

        #region Proyectos

        /// <summary>
        /// "Debe seleccionar la Clase de Servicio"
        /// </summary>
        public static string Py_ClaseServicio = "msg_py_ClaseServicio";

        /// <summary>
        /// "Debe seleccionar el area Funcional"
        /// </summary>
        public static string Py_AreaFunciona = "msg_py_AreaFuncional";

        /// <summary>
        /// "Esta ejecución reiniciar el proyecto de acuerdo a la paremtrizacion actual. Desea continuar?"
        /// </summary>
        public static string Py_ValidarGenerarSolicitud = "msg_py_ValidarGenerarSolicitud";
        
        /// <summary>
        /// "Generar Solicitud Proyecto"
        /// </summary>
        public static string Py_GenerarSolicitud = "msg_py_GenerarSolicitud";

        /// <summary>
        /// "Ya existe una solicitud de Servicio guardada para la clase de Servicio {0} desea cargarla?"
        /// </summary>
        public static string Py_SolicitudExistente = "msg_py_SolicitudExistente";

        /// <summary>
        /// "Se genererá nuevamente la solictud con los parametros establecidos en el sistema. Desea Continuar?"
        /// </summary>
        public static string Py_ReiniciarSolicitud = "msg_py_ReiniciarSolicitud";

        /// <summary>
        /// "No ha realizado cambios en niguna Solicitud de Servicio"
        /// </summary>
        public static string Py_NotChangesSS = "msg_py_NotChangesSS";
                
        /// <summary>
        /// "Ha cargado una Solicitud de Servicio existente, no se puede guardar"
        /// </summary>
        public static string Py_ExistsSS = "msg_py_ExistsSS";

        /// <summary>
        /// "Los siguientes Campos son Obligatorios : {0}"
        /// </summary>
        public static string Py_CamposObligatorios = "msg_CamposObligatorios";

        /// <summary>
        /// "Se ha generado o actualizado el documento  : {0} - {1}"
        /// </summary>
        public static string Py_SuccessSol = "msg_py_SuccessSol";

        /// <summary>
        /// "La tarea importada no corresponde a la seleccionada en la grilla"
        /// </summary>
        public static string Py_TareaInvalid = "msg_py_TareaInvalid";

        /// <summary>
        /// "La tarea Cliente digitada ya existe"
        /// </summary>
        public static string Py_TareaClienteExist = "msg_py_TareaClienteExist";

        /// <summary>
        /// "Está seguro que desea enviar a aprobación el documento actual para solicitud de Orden de Compra"
        /// </summary>
        public static string Py_SendToAprrovOC = "msg_py_SendToAprrovOC";

        /// <summary>
        /// "Ésta orden no existe o ya fue despachada, intente de nuevo"
        /// </summary>
        public static string Py_OrdenDespachoAlreadyExist = "msg_py_OrdenDespachoAlreadyExist";

        /// <summary>
        /// "Ésta orden se encuentra Aprobada, desea modificarla?"
        /// </summary>
        public static string Py_OrdenDespachoApprove = "msg_py_OrdenDespachoApprove";

        /// <summary>
        /// "{0} {1} está duplicado, verifique nuevamente "
        /// </summary>
        public static string Py_FieldAlreadyExist = "msg_py_TareaClienteExist";

        /// <summary>
        /// "El porcentaje total de Entrega debe ser el 100%"
        /// </summary>
        public static string Py_PorcentajeEntregaInvalid = "msg_py_PorcentajeEntregaInvalid";


        #endregion

        #region Reportes

        #region Mensajes Globales


        /// <summary>
        /// El reporte generado no tiene datos, verifique los filtros y vuelva a generar.
        /// </summary>
        public static string Rpt_gl_ReporteNotDataFound = "msg_gl_ReporteNotDataFound";

        /// <summary>
        /// ¿Desea Imprimir el reporte?
        /// </summary>
        public static string Rpt_gl_DeseaImprimirReporte = "msg_gl_DeseaImprimirReporte";

        /// <summary>
        /// No se puede crear la plantilla de excel, revise los datos de consulta ó comuniquece con el Administrador del programa
        /// </summary>
        public static string Rpt_gl_NoSeGeneranDatos = "msg_gl_NoSeGeneranDatos"; 

        /// <summary>
        /// "Los siguientes campos son obligatorios : {0}"
        /// </summary>
        public static string Rpt_gl_FieldObligated = "msg_gl_FieldObligated";

        /// <summary>
        /// "Por favor Ingrese un cliente"
        /// </summary>
        public static string Rpt_gl_ClientObligated = "msg_gl_clientObligated";

        /// <summary>
        /// "La fecha Inicial no puede ser superior a la fecha Final"
        /// </summary>
        public static string Rpt_gl_DateInvalid = "msg_gl_dateInvalid";

	    #endregion

        #region Mensajes Cuentas por Pagar
		
        /// <summary>
        /// Tercero Vacio, Ingrese el Tercero al que desea generar Factura Equivalente
        /// </summary>
        public static string Rpt_Cp_sinTercero = "msg_cp_SinTercero";

        /// <summary>
        /// El Regimen Fiscal asignado al Tercero {0}, No generera Factura Equivalente
        /// </summary>
        public static string Rpt_Cp_RegimenFiscalNogeneraFactEquivalente = "msg_cp_RegimenFiscalNogeneraFactEquivalente";

        /// <summary>
        /// No se ha generado Factura Equivalente para el tercero {0}
        /// </summary>
        public static string Rpt_Cp_NogeneraFactEquivalente = "msg_cp_NogeneraFactEquivalente"; 
	#endregion

        #endregion

        #region Tesoreria
        
        /// <summary>
        /// Las cuentas de banco deben tener un valor y deben ser distintas
        /// </summary>
        public static string Ts_BancoCuentaOrigenDestinoInvalidos = "msg_ts_BancoCuentaOrigenDestinoInvalidos";

        /// <summary>
        /// "El Banco no tiene estructuras de migración"
        /// </summary>
        public static string Ts_BancoSinEstructuras = "msg_ts_bancoSinEstructuras";

        /// <summary>
        /// Para realizar la consignación es necesario llenar todos los datos del formulario
        /// </summary>
        public static string Ts_CamposConsignacionVacios = "msg_ts_CamposConsignacionVacios";

        /// <summary>
        /// "El Banco no corresponde a el del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidBanco = "msg_ts_CompInvalidBanco";

        /// <summary>
        /// "La caja no corresponde a la del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidCaja = "msg_ts_CompInvalidCaja";

        /// <summary>
        /// "La fecha no corresponde a la del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidFecha = "msg_ts_CompInvalidFecha";

        /// <summary>
        /// "La moneda no corresponde a la del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidMda = "msg_ts_CompInvalidMda";

        /// <summary>
        /// "La tasa de cambio no corresponde a la del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidTC = "msg_ts_CompInvalidTC";

        /// <summary>
        /// "El tercero no corresponde con el del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidTercero = "msg_ts_CompInvalidTercero";

        /// <summary>
        /// "El valor no corresponde con el del item {0} (linea {1})"
        /// </summary>
        public static string Ts_CompInvalidValor = "msg_ts_CompInvalidValor";

        /// <summary>
        /// " Ya existen pagos seleccionados con el Cuenta Bancaria {0},los desea deselecccionar? "
        /// </summary>
        public static string Ts_SeleccionCuentaBancaria = "msg_ts_SeleccionCuentaBancaria";       

        /// <summary>
        /// Los modulos de CxP y tesoreria deben estar en el mismo periodo
        /// </summary>
        public static string Ts_InvalidModsProgPagos = "msg_ts_InvalidModsProgPagos";

        /// <summary>
        /// "La suma del comprobante no corresponde al valor del recibo de caja"
        /// </summary>
        public static string Ts_InvalidTotalMigracion = "msg_ts_InvalidTotalMigracion";

        /// <summary>
        /// "El item {0} fue agregado en la línea {1}"
        /// </summary>
        public static string Ts_ItemAgregado = "msg_ts_itemAgregado";

        /// <summary>
        /// "Las monedas de las cuentas deben ser distintas"
        /// </summary>
        public static string Ts_MonedaCuentasDistintas = "msg_ts_MonedaCuentasDistintas";

        /// <summary>
        /// "Recibo numero ({0}), ha sido guardado"
        /// </summary>
        public static string Ts_ReciboNro = "msg_ts_ReciboNro";

        /// <summary>
        /// "Existen terceros en estos pagos, que no tienen definida una cuenta bancaria apropiadamente. Estos pagos no se mostrarán, hasta que el tercero tenga una cuenta bancaria definida"
        /// </summary>
        public static string Ts_TercerosSinCuentaBancaria = "msg_ts_TercerosSinCuentaBancaria";

        /// <summary>
        /// El valor de la transacción debe ser mayor a 0
        /// </summary>
        public static string Ts_ValorATrasladarInvalido = "msg_ts_ValorATrasladarInvalido";

        /// <summary>
        /// "El valor debe ser menor a Valor a Pagar"
        /// </summary>
        public static string Ts_ValorPagoRangeValue = "msg_ts_ValorPagoRangeValue";

        /// <summary>
        /// "El rango de fechas es obligatorio"
        /// </summary>
        public static string Ts_RangoNull = "msg_ts_RangoNull";

        /// <summary>
        /// "El Banco es obligatorio"
        /// </summary>
        public static string Ts_BanckNull = "msg_ts_BanckNull";

        #endregion

        #endregion
    }
}


