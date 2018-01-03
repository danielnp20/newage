using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Text.RegularExpressions;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    public static class DTO_Validations
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private static BaseController _bc = BaseController.GetInstance();

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Valida las reglas de negocio de un DTO
        /// </summary>
        /// <param name="dtoObj">Objeto que se envia</param>
        /// <param name="res">Respuesta si esta bien el DTO</param>
        /// <param name="msg">Mensaje de error (vacio si esta todo ok)</param>
        /// <returns>Retorna un diccionario con la lista de campos y sus errores</returns>
        public static SortedDictionary<string, string> CheckRules(Object dtoObj, out bool res, out string msg, DTO_glTabla table, bool isInsertando)
        {
            res = true;
            msg = string.Empty;
            SortedDictionary<string, string> result = new SortedDictionary<string, string>();

            try
            {

                #region acClase

                if (dtoObj.GetType() == typeof(DTO_acClase))
                {
                    DTO_acClase dto = (DTO_acClase)dtoObj;

                    #region TipoAct

                    //if (dto.TipoAct.Value != 0 && dto.TipoAct.Value != 1 && dto.TipoAct.Value != 2)
                    //{
                    //    res = false;
                    //    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Ac_Grupo_TipoAct_Restr);
                    //    result.Add("TipoAct ", msg);
                    //}

                    #endregion

                    #region TipoDepreLOC

                    if (dto.TipoDepreLOC.Value != 0 && dto.TipoDepreLOC.Value != 1 && dto.TipoDepreLOC.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Ac_Grupo_TipoDepreLOC_Restr);
                        result.Add("TipoDepreLOC ", msg);
                    }

                    #endregion


                }

                #endregion

                #region acComponenteActivo

                if (dtoObj.GetType() == typeof(DTO_acComponenteActivo))
                {
                    DTO_acComponenteActivo dto = (DTO_acComponenteActivo)dtoObj;
                    //DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cuentaLoc.ConceptoSaldoID.Value, true);
                    DTO_glConceptoSaldo conSaldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, dto.ConceptoSaldoID.Value, true);
                    if (conSaldo.coSaldoControl.Value != 5)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Ac_Componente_CoSaldoCtrl_Restr);
                        result.Add("TipoCta", msg);
                    }
                }

                #endregion

                #region ccCliente

                if (dtoObj.GetType() == typeof(DTO_ccCliente))
                {
                    try
                    {
                        DTO_ccCliente dto = (DTO_ccCliente)dtoObj;

                        //Solicita llenar CuentaTipo_1 y BcoCtaNro_1 cuando hay Banco asignado
                        if (!string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && (string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString())
                            || (dto.CuentaTipo_1.Value != 3) && (string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString()))))
                        {
                            res = false;
                            string msgImp = _bc.GetResource(LanguageTypes.Messages,
                                                            MasterMessages.Co_Tercero_CtaTipo_AccountRequired);
                            msg = string.Format(msgImp, dto.BancoID_1.Value);
                            result.Add("BancoID_1", msg);
                        }
                        else if (string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && (!string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString()) || !string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString())))
                        {
                            res = false;
                            string msgBanco = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_BancoID1_BanKRequired);
                            msg = string.Format(msgBanco, dto.CuentaTipo_1.Value, dto.BcoCtaNro_1.Value);
                            result.Add("BancoID_1", msg);
                        }

                        //string sectorCartera = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                        //if (sectorCartera == ((byte)SectorCartera.Financiero).ToString())
                        //{
                            string zona = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Zona);
                            string profesion = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ProfesionPorDefecto);
                            string asesor = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AsesorXDefecto);

                            dto.ResidenciaTipo.Value = 4;
                            dto.ZonaID.Value = zona;
                            dto.Cargo.Value = "No Aplica";
                            dto.ProfesionID.Value = profesion;
                            dto.LaboralEntidad.Value = "No Aplica";
                            dto.Antiguedad.Value = 0;
                            dto.ClienteTipo.Value = 1;
                            dto.Estrato.Value = 1;
                            dto.EscolaridadNivel.Value = 0;
                            dto.JornadaLaboral.Value = 1;
                            dto.Ocupacion.Value = 1;
                            dto.AsesorID.Value = asesor;
                            dto.VlrDevengado.Value = 0;
                            dto.VlrDeducido.Value = 0;
                            dto.VlrOtrosIng.Value = 0;
                            dto.VlrActivos.Value = 0;
                            dto.VlrPasivos.Value = 0;
                            dto.VlrMesada.Value = 0;
                            dto.VlrConsultado.Value = 0;
                            dto.VlrOpera.Value = 0;
                            dto.FechaIngresoPAG.Value = DateTime.Now;
                            dto.FechaIngreso.Value = DateTime.Now;
                            dto.EstadoCartera.Value = 1;
                            dto.TelefonoTrabajo.Value = dto.Telefono.Value;
                        //}
                        //else
                        //{
                        //    #region Valida que el cliente sea mayor de edad

                        //    int edadMinima = 18;
                        //    int fechaNacimiento = 0;

                            //TimeSpan difFechaNacimiento = dto.FechaExpDoc.Value.Value.Subtract(dto.FechaNacimiento.Value.Value);
                            //fechaNacimiento = (int)Math.Floor((double)difFechaNacimiento.Days / 365);

                            //if (fechaNacimiento < edadMinima)
                            //{
                            //    res = false;
                            //    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_ServicioTipo_ValidDate);
                            //    result.Add("FechaExpDoc", msg);
                            //}

                        //    #endregion
                        //}
                        #region Verifica si realmente es mujer ama de casa
                        if (dto.MujerInd.Value == true)
                            if (dto.Sexo.Value == 1)
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_ServicioTipo_HeadFamily);
                                result.Add("Sexo", msg);
                            }

                        #endregion
                    }
                    catch (Exception ex)
                    {                        
                        throw;
                    }
                }

                #endregion

                #region ccComponenteCuenta
                if (dtoObj.GetType() == typeof(DTO_ccComponenteCuenta))
                {
                    DTO_ccComponenteCuenta dto = (DTO_ccComponenteCuenta)dtoObj;
                    DTO_ccCarteraComponente cartera = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, dto.ComponenteCarteraID.Value, true);
                    if (cartera.TipoComponente.Value.Value == 1 || cartera.TipoComponente.Value.Value == 4)
                    {
                        DTO_coPlanCuenta planCuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, true, dto.CuentaID.Value, false);
                        if (planCuenta == null)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Componente_EmptyCta);
                            string msfFormat = string.Format(msg, dto.ComponenteCarteraID.Value);
                            result.Add("TipoCuenta", msfFormat);
                        }
                        else
                        {
                            DTO_glConceptoSaldo conSaldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, planCuenta.ConceptoSaldoID.Value, true);
                            if (cartera.TipoComponente.Value.Value == 1)
                            {
                                if (conSaldo.coSaldoControl.Value.Value != 3)
                                {
                                    res = false;
                                    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Componente_CoSaldoCtrl_Restr);
                                    result.Add("TipoCuenta", msg);
                                }
                            }
                            else if (cartera.TipoComponente.Value.Value == 4)
                            {
                                if (conSaldo.coSaldoControl.Value.Value != 6)
                                {
                                    res = false;
                                    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Componente_CoSaldoCtrl_Restr);
                                    result.Add("TipoCuenta", msg);
                                }
                            }
                            else if (cartera.TipoComponente.Value.Value != 4 && cartera.TipoComponente.Value.Value != 1)
                            {
                                if (conSaldo.coSaldoControl.Value.Value != 1 || conSaldo.coSaldoControl.Value.Value != 4)
                                {
                                    res = false;
                                    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Componente_CoSaldoCtrl_Restr);
                                    result.Add("TipoCuenta", msg);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region ccComponentesCartera

                if (dtoObj.GetType() == typeof(DTO_ccComponenteCuenta))
                {
                    DTO_ccComponenteCuenta dto = (DTO_ccComponenteCuenta)dtoObj;

                    if (!string.IsNullOrEmpty(dto.CuentaID.Value))
                    {
                        DTO_ccCarteraComponente componente = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, true, dto.ComponenteCarteraID.Value, false);
                        DTO_coPlanCuenta cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, true, dto.CuentaID.Value, false);

                        if (cuenta.ConceptoSaldoID.Value != componente.ConceptoSaldoID.Value)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_ComponenteCartera_ServicioTipo_ValidDate);
                            result.Add("conseptoSaldo", msg);
                        }
                    }
                }

                #endregion

                #region ccLineaComponente

                if (dtoObj.GetType() == typeof(DTO_ccLineaComponente))
                {
                    DTO_ccLineaComponente dto = (DTO_ccLineaComponente)dtoObj;

                    if (String.IsNullOrWhiteSpace(dto.Valor.Value.ToString()))
                    {
                        dto.Valor.Value = 0;
                    }

                    if (String.IsNullOrWhiteSpace(dto.PorcentajeID.Value.ToString()))
                    {
                        dto.PorcentajeID.Value = 0;
                    }
                }

                #endregion

                #region ccLineaComponenteMonto

                if (dtoObj.GetType() == typeof(DTO_ccLineaComponenteMonto))
                {
                    DTO_ccLineaComponenteMonto dto = (DTO_ccLineaComponenteMonto)dtoObj;

                    if (String.IsNullOrWhiteSpace(dto.Valor.Value.ToString()))
                    {
                        dto.Valor.Value = 0;
                    }

                    if (String.IsNullOrWhiteSpace(dto.PorcentajeID.Value.ToString()))
                    {
                        dto.PorcentajeID.Value = 0;
                    }
                }

                #endregion

                #region ccLineaComponentePlazo

                if (dtoObj.GetType() == typeof(DTO_ccLineaComponentePlazo))
                {
                    DTO_ccLineaComponentePlazo dto = (DTO_ccLineaComponentePlazo)dtoObj;

                    if (String.IsNullOrWhiteSpace(dto.Valor.Value.ToString()))
                    {
                        dto.Valor.Value = 0;
                    }

                    if (String.IsNullOrWhiteSpace(dto.PorcentajeID.Value.ToString()))
                    {
                        dto.PorcentajeID.Value = 0;
                    }
                }

                #endregion

                #region coActEconomica

                if (dtoObj.GetType() == typeof(DTO_coActEconomica))
                {
                    DTO_coActEconomica dto = (DTO_coActEconomica)dtoObj;

                    #region ServicioTipo

                    if (dto.ServicioTipo.Value != 0 && dto.ServicioTipo.Value != 1 && dto.ServicioTipo.Value != 2 && dto.ServicioTipo.Value != 3 &&
                        dto.ServicioTipo.Value != 4 && dto.ServicioTipo.Value != 5)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ActEconomica_ServicioTipo_Restr);
                        result.Add("ServicioTipo", msg);
                    }

                    #endregion
                }

                #endregion

                #region coBalanceReclasifica

                if (dtoObj.GetType() == typeof(DTO_coBalanceReclasifica))
                {
                    DTO_coBalanceReclasifica dto = (DTO_coBalanceReclasifica)dtoObj;

                    #region AgrupaTercero

                    if (dto.AgrupaTercero.Value != 1 && dto.AgrupaTercero.Value != 2 && dto.AgrupaTercero.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Co_BalanceReclasifica_AgrupaTercero_Restr);
                        result.Add("AgrupaTercero", msg);
                    }

                    #endregion
                }

                #endregion

                #region coComprobante

                if (dtoObj.GetType() == typeof(DTO_coComprobante))
                {
                    DTO_coComprobante dto = (DTO_coComprobante)dtoObj;

                    #region TipoConsec

                    if (dto.TipoConsec.Value != 1 && dto.TipoConsec.Value != 2 && dto.TipoConsec.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Comprobante_TipoConsec_Restr);
                        result.Add("TipoConsec", msg);
                    }

                    #endregion
                    #region BimonedaInd

                    if (!_bc.AdministrationModel.MultiMoneda)
                    {
                        if (!string.IsNullOrWhiteSpace(dto.biMonedaInd.Value.ToString()) && dto.biMonedaInd.Value.Value)
                        {
                            res = false;
                            string msgBim = _bc.GetResource(LanguageTypes.Messages,
                                                            MasterMessages.Co_Comprobante_BimonedaNotActive);
                            result.Add("biMonedaInd", msgBim);
                        }
                    }

                    #endregion
                    #region ComprobanteAnul
                    if (!string.IsNullOrEmpty(dto.ComprobanteAnulID.Value))
                    {
                        DTO_coComprobante comprAnul = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, dto.ComprobanteAnulID.Value, true);
                        if (comprAnul != null && dto.BalanceTipoID.Value != comprAnul.BalanceTipoID.Value)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Comprobante_LibroCompAnulInvalid);
                            result.Add("BalanceTipoID", msg);
                        }
                    }

                    #endregion
                }

                #endregion

                #region coConceptoCargo

                if (dtoObj.GetType() == typeof(DTO_coConceptoCargo))
                {
                    DTO_coConceptoCargo dto = (DTO_coConceptoCargo)dtoObj;

                    #region TipoConcepto

                    if (dto.TipoConcepto.Value != 1 && dto.TipoConcepto.Value != 2 && dto.TipoConcepto.Value != 3 &&
                        dto.TipoConcepto.Value != 4 && dto.TipoConcepto.Value != 5 && dto.TipoConcepto.Value != 6 &&
                        dto.TipoConcepto.Value != 7 && dto.TipoConcepto.Value != 8 && dto.TipoConcepto.Value != 9)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ConceptoCargo_TipoConcepto_Restr);
                        result.Add("TipoConcepto", msg);
                    }

                    #endregion

                    #region Bien Servicio
                    if ((dto.TipoConcepto.Value == 3 || dto.TipoConcepto.Value == 4
                       || dto.TipoConcepto.Value == 5 || dto.TipoConcepto.Value == 6 || dto.TipoConcepto.Value == 7
                       || dto.TipoConcepto.Value == 8) && dto.BienServicio.Value != 1)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ConceptoCargo_BienServicio_Restr);
                        result.Add("BienServicio", msg);
                    }
                    #endregion

                }

                #endregion

                #region coCuentaGrupo

                if (dtoObj.GetType() == typeof(DTO_coCuentaGrupo))
                {
                    DTO_coCuentaGrupo dto = (DTO_coCuentaGrupo)dtoObj;

                    #region Agrupa (Consolida Por)

                    if (dto.ConsolidaPor.Value != 1 && dto.ConsolidaPor.Value != 2 && dto.ConsolidaPor.Value != 3 &&
                        dto.ConsolidaPor.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_CtaGrupo_ConsolidaPor_Restr);
                        result.Add("ConsolidaPor", msg);
                    }

                    #endregion

                    #region Agrupa (Tipo de Cuenta)

                    if (dto.TipoCuenta.Value != 1 && dto.TipoCuenta.Value != 2 && dto.TipoCuenta.Value != 3 &&
                        dto.TipoCuenta.Value != 4 && dto.TipoCuenta.Value != 5 && dto.TipoCuenta.Value != 6)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_CtaGrupo_TipoCuenta_Restr);
                        result.Add("TipoCuenta", msg);
                    }

                    #endregion
                }

                #endregion

                #region coDocumento

                if (dtoObj.GetType() == typeof(DTO_coDocumento))
                {
                    DTO_coDocumento dto = (DTO_coDocumento)dtoObj;
                    DTO_glDocumento Doc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, dto.DocumentoID.Value, true);

                    #region CuentaLOC

                    if (dto != null)
                    {
                        //Solicita la cuenta local o extranjera o ambas segun la moneda origen
                        if ((dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Local ||
                             dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Both) &&
                            string.IsNullOrEmpty(dto.CuentaLOC.Value))
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_CuentaLOC_Empty);
                            result.Add("CuentaLOC", msg);
                        }

                        else if (!string.IsNullOrEmpty(dto.CuentaLOC.Value))
                        {
                            //Permite ingresar solo cuentas con Origen Monetario Local y el mismo modulo si es Doc Interno o externo
                            DTO_coPlanCuenta cuentaLoc = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, dto.CuentaLOC.Value, true);
                            DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cuentaLoc.ConceptoSaldoID.Value, true);
                            if (cuentaLoc != null && saldoLoc != null)
                            {
                                if (cuentaLoc.OrigenMonetario.Value != 1 && dto.MonedaOrigen.Value != 4)
                                {
                                    res = false;
                                    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_cuentaLOC_OrigenMonet);
                                    result.Add("CuentaLOC", msg);
                                }       
                            }
                        }
                    }

                    #endregion
                    #region CuentaEXT

                    if (dto != null)
                    {
                        //Solicita la cuenta local o extranjera o ambas segun la moneda origen
                        if ((dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Foreign ||
                             dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Both) &&
                            string.IsNullOrEmpty(dto.CuentaEXT.Value))
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_CuentaEXT_Empty);
                            result.Add("CuentaEXT", msg);
                        }
                        else if (!string.IsNullOrEmpty(dto.CuentaEXT.Value))
                        {
                            //Permite ingresar solo cuentas con Origen Monetario Local y el mismo modulo si es Doc Interno o Externo
                            DTO_coPlanCuenta cuentaExtr = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, dto.CuentaEXT.Value, true);
                            DTO_glConceptoSaldo saldoExt = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cuentaExtr.ConceptoSaldoID.Value, true);

                            if (cuentaExtr != null && saldoExt != null)
                            {
                                if (cuentaExtr.OrigenMonetario.Value != 2 && dto.MonedaOrigen.Value != 4)
                                {
                                    res = false;
                                    msg = _bc.GetResource(LanguageTypes.Messages,
                                                          MasterMessages.Co_coDocumento_cuentaEXT_OrigenMonet);
                                    result.Add("CuentaEXT", msg);
                                }
                                //else if (saldoExt.ModuloID.Value != Doc.ModuloID.Value &&
                                //         (saldoExt.coSaldoControl.Value.Value == (byte)SaldoControl.Doc_Interno ||
                                //          saldoExt.coSaldoControl.Value.Value == (byte)SaldoControl.Doc_Externo))
                                //{
                                //    res = false;
                                //    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_CuentaEXT_ModuleInvalid);
                                //    result.Add("CuentaEXT", msg);
                                //}
                            }
                        }
                    }

                    #endregion
                    #region MdaLoc y MdaExt
                    //if(){}

                    #endregion
                    #region TipoComprobante

                    if (dto.TipoComprobante.Value != 1 && dto.TipoComprobante.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_TipoCompr_Restr);
                        result.Add("TipoComprobante", msg);
                    }

                    #endregion
                    #region Comprobante
                    if (!isInsertando)
                    {
                        if (dto.ComprobanteID.Value != null && dto.ComprobanteDesc.Value != null)
                        {
                            bool hay = _bc.AdministrationModel.ComprobanteExistsInAuxPre(dto.ComprobanteID.Value);
                            if (hay)
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_Comprobante_Restr);
                                result.Add("ComprobanteID", msg);
                            }
                            //_bc.Get();
                        }
                    }
                    #endregion
                    #region MonedaOrigen

                    if (dto.MonedaOrigen.Value != 1 && dto.MonedaOrigen.Value != 2 && dto.MonedaOrigen.Value != 3 &&
                        dto.MonedaOrigen.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_MonedaOrigen_Restr);
                        result.Add("MonedaOrigen", msg);
                    }

                    #endregion
                    #region Documento
                    if (dto != null)
                    {
                        DTO_glDocumento doc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, dto.DocumentoID.Value, true);
                        if (!string.IsNullOrEmpty(dto.ComprobanteID.Value) && doc != null &&  (!doc.NOLibroFuncionalInd.Value.HasValue || !doc.NOLibroFuncionalInd.Value.Value))
                        {
                            DTO_coComprobante compr = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante,false, dto.ComprobanteID.Value, true);
                            string libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                            if (!compr.BalanceTipoID.Value.Equals(libroFunc))
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_coDocumento_BalanceInvalid);
                                result.Add("DocumentoID", msg);
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                #region coImpuesto

                if (dtoObj.GetType() == typeof(DTO_coImpuesto))
                {
                    DTO_coImpuesto dto = (DTO_coImpuesto)dtoObj;

                    #region Regimen Fiscal

                    string regFis = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
                    if (!string.IsNullOrWhiteSpace(dto.RegimenFiscalEmpresaID.Value) &&
                        dto.RegimenFiscalEmpresaID.Value != regFis)
                    {
                        res = false;

                        string msgImp = _bc.GetResource(LanguageTypes.Messages,
                                                        MasterMessages.Co_Impuesto_RegFisEmp_NotCompatible);
                        msg = string.Format(msgImp, regFis);
                        result.Add("RegimenFiscalEmpresaID", msg);
                    }

                    #endregion

                    #region Tipo de impuesto

                    //Valida el tipo de impuesto
                    int impTipoDoc = AppMasters.coImpuestoTipo;
                    DTO_coImpuestoTipo tipo =
                        (DTO_coImpuestoTipo)
                        _bc.GetMasterDTO(AppMasters.MasterType.Simple, impTipoDoc, false, dto.ImpuestoTipoID.Value, true);
                    //Verifica si el impuesto es nacional: tipo de impuesto (impuesto alcance) 

                    bool isNacional = tipo != null && tipo.ImpuestoAlcance.Value == 1 ? true : false;
                    if (isNacional)
                    {
                        int lgDoc = AppMasters.glLugarGeografico;
                        //string egLG = _bc.GetMaestraEmpresaGrupoByDocumentID(lgDoc);
                        List<DTO_MasterBasic> lgs =
                            _bc.AdministrationModel.MasterHierarchy_GetChindrenPaged(lgDoc, 1, 1,
                                                                                     OrderDirection.ASC,
                                                                                     new UDT_BasicID(), string.Empty,
                                                                                     string.Empty, true).ToList();

                        if (lgs.Count > 0)
                        {

                            UDT_BasicID idParent = new UDT_BasicID() { Value = lgs.First().ID.Value };
                            lgs = _bc.AdministrationModel.MasterHierarchy_GetChindrenPaged(lgDoc, 1, 1,
                                                                                           OrderDirection.ASC, idParent,
                                                                                           string.Empty, string.Empty,
                                                                                           true).ToList();

                            if (lgs.Count > 0)
                            {
                                DTO_MasterBasic lg = lgs.First();
                                if (dto.LugarGeograficoID.Value != _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto))
                                {
                                    res = false;
                                    string msgImp = _bc.GetResource(LanguageTypes.Messages,
                                                                    MasterMessages.Co_ImpuestoTipo_LugarGeo_ImpNal);
                                    msg = string.Format(msgImp, lg.ID.Value, lg.Descriptivo.Value);
                                    result.Add("LugarGeograficoID", msg);
                                }
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                #region coImpuestoTipo

                if (dtoObj.GetType() == typeof(DTO_coImpuestoTipo))
                {
                    DTO_coImpuestoTipo dto = (DTO_coImpuestoTipo)dtoObj;

                    #region CausacionPago

                    if (dto.CausacionPago.Value != 1 && dto.CausacionPago.Value != 2)
                    {
                        res = false;

                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpTipo_CausaPago_Restr);
                        result.Add("CausacionPago", msg);
                    }

                    #endregion

                    #region ImpuestoAlcance

                    if (dto.ImpuestoAlcance.Value != 1 && dto.ImpuestoAlcance.Value != 2 &&
                        dto.ImpuestoAlcance.Value != 3)
                    {
                        res = false;

                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpTipo_ImpAlcance_Restr);
                        result.Add("ImpuestoAlcance", msg);
                    }

                    #endregion
                }

                #endregion

                #region coImpuestoDeclaracion

                if (dtoObj.GetType() == typeof(DTO_coImpuestoDeclaracion))
                {
                    DTO_coImpuestoDeclaracion dto = (DTO_coImpuestoDeclaracion)dtoObj;

                    #region PeriodoDeclaracion

                    if (dto.PeriodoDeclaracion.Value != 1 && dto.PeriodoDeclaracion.Value != 2 &&
                        dto.PeriodoDeclaracion.Value != 4 &&
                        dto.PeriodoDeclaracion.Value != 6 && dto.PeriodoDeclaracion.Value != 12)
                    {
                        res = false;

                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpuestoDec_PeriodoDec_Restr);
                        result.Add("PeriodoDeclaracion", msg);
                    }

                    #endregion

                    #region MunicipalInd

                    if (!dto.MunicipalInd.Value.Value)
                    {
                        string LugarGeoxDefec = _bc.GetControlValueByCompany(ModulesPrefix.co,
                                                                             AppControl.co_LugarGeoXDefecto);

                        if (LugarGeoxDefec != dto.LugarGeograficoID.Value)
                        {
                            res = false;
                            string rsx = _bc.GetResource(LanguageTypes.Messages,
                                                         MasterMessages.Co_ImpuestoDec_LugarGeoInvalid);
                            msg = string.Format(rsx, dto.LugarGeograficoID.Value);
                            result.Add("LugarGeograficoID", msg);
                        }

                    }

                    #endregion
                }

                #endregion

                #region coImpuestoFormato

                if (dtoObj.GetType() == typeof(DTO_coImpuestoFormato))
                {
                    DTO_coImpuestoFormato dto = (DTO_coImpuestoFormato)dtoObj;

                    #region Tipo

                    if (dto.Tipo.Value != 1 && dto.Tipo.Value != 2)
                    {
                        res = false;

                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpuestoFormato_Tipo_Restr);
                        result.Add("Tipo", msg);
                    }

                    #endregion

                    #region TipoFiscal

                    if (dto.TipoFiscal.Value != 1 && dto.TipoFiscal.Value != 2 && dto.TipoFiscal.Value != 3 &&
                        dto.TipoFiscal.Value != 4 && dto.TipoFiscal.Value != 5)
                    {
                        res = false;

                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpuestoFormato_TipoFiscal_Restr);
                        result.Add("TipoFiscal", msg);
                    }

                    #endregion

                }

                #endregion

                #region coImpDeclaracionCuenta

                if (dtoObj.GetType() == typeof(DTO_coImpDeclaracionCuenta))
                {
                    DTO_coImpDeclaracionCuenta dto = (DTO_coImpDeclaracionCuenta)dtoObj;

                    #region CuentaID

                    //Verifica que la cuenta declare impuestos
                    DTO_coPlanCuenta cuentaDto =
                        (DTO_coPlanCuenta)
                        _bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false,
                                         dto.CuentaID.Value, true);
                    if (cuentaDto != null)
                    {
                        if (string.IsNullOrEmpty(cuentaDto.ImpuestoTipoID.Value))
                        {
                            //res = false;
                            //msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpDeclCuenta_CuentaID_Invalid);
                            //result.Add("CuentaID", msg);
                        }
                    }
                    if (cuentaDto.NITCierreAnual.Value == "")
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ImpDeclCuenta_NITCierreAnual_Empty);
                        result.Add("NITCierrreAnual", msg);
                    }

                    #endregion
                }

                #endregion

                #region coImpDeclaracionCalendario

                if (dtoObj.GetType() == typeof(DTO_coImpDeclaracionCalendario))
                {
                    DTO_coImpDeclaracionCalendario dto = (DTO_coImpDeclaracionCalendario)dtoObj;

                    #region AñoFiscal

                    //Verifica que el año fiscal se valido
                    bool isFormat = Regex.IsMatch(dto.AñoFiscal.Value.ToString(), "19[9][0-9]|2[0-9]{3}");
                    if (!isFormat)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Co_ImpDeclCalendario_AñoFiscal_Invalid);
                        result.Add("AñoFiscal", msg);
                    }

                    #endregion
                }

                #endregion

                #region coImpDeclaracionRenglon

                if (dtoObj.GetType() == typeof(DTO_coImpDeclaracionRenglon))
                {
                    DTO_coImpDeclaracionRenglon dto = (DTO_coImpDeclaracionRenglon)dtoObj;

                    #region SignoSuma

                    if (dto.SignoSuma.Value != 1 && dto.SignoSuma.Value != 2 && dto.SignoSuma.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Co_ImpDeclaracionRenglon_SignoSuma_Restr);
                        result.Add("SignoSuma", msg);
                    }

                    #endregion
                }

                #endregion

                #region coOperacion

                if (dtoObj.GetType() == typeof(DTO_coOperacion))
                {
                    DTO_coOperacion dto = (DTO_coOperacion)dtoObj;

                    #region TipoFiscal

                    //if (dto.TipoFiscal.Value != 1 && dto.TipoFiscal.Value != 2 && dto.TipoFiscal.Value != 3 &&
                    //    dto.TipoFiscal.Value != 4)
                    //{
                    //    res = false;
                    //    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Operacion_TipoFiscal_Restr);
                    //    result.Add("TipoFiscal", msg);
                    //}

                    #region TipoOperacion
                    if (dto.TipoOperacion.Value != 1 && dto.TipoOperacion.Value != 2 && dto.TipoOperacion.Value != 3
                        && dto.TipoOperacion.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Operacion_TipoOpe_RestrO);
                        result.Add("TipoOperacion", msg);
                    }
                    #endregion

                    #endregion
                }

                #endregion

                #region coPlanCuenta

                if (dtoObj.GetType() == typeof(DTO_coPlanCuenta))
                {
                    DTO_coPlanCuenta dto = (DTO_coPlanCuenta)dtoObj;

                    #region ImpuestoTipoID

                    //Solicita llenar ImpuestoPorc y MontoMinimo  cuando hay Tipo de impuesto
                    if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) &&
                        (string.IsNullOrWhiteSpace(dto.ImpuestoPorc.Value.ToString()) ||
                         string.IsNullOrWhiteSpace(dto.MontoMinimo.Value.ToString())))
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_ImpPorc_NotEmpty);
                        result.Add("ImpuestoTipoID", msg);
                    }
                    else if (string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) &&
                             (!string.IsNullOrWhiteSpace(dto.ImpuestoPorc.Value.ToString()) ||
                              !string.IsNullOrWhiteSpace(dto.MontoMinimo.Value.ToString())))
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_ImpTipo_NotExist);
                        result.Add("ImpuestoTipoID", msg);
                    }

                    #endregion

                    #region Tipo

                    //Valida que el Tipo ingresado corresponda al mismo Tipo de la jerarquia 
                    int nivel = table.LengthToLevel(dto.ID.Value.Trim().Length);
                    if (nivel != 1)
                    {
                        int nivelPadre = nivel - 1;
                        int lonPadre = table.CodeLength(nivelPadre);
                        string idPadre = dto.ID.Value.Trim().Substring(0, lonPadre);
                        UDT_BasicID idPadreUdt = new UDT_BasicID();
                        idPadreUdt.Value = idPadre;
                        DTO_coPlanCuenta padre =
                            (DTO_coPlanCuenta)
                            _bc.AdministrationModel.MasterHierarchy_GetByID(AppMasters.coPlanCuenta, idPadreUdt, true);
                        if (padre != null)
                        {
                            if (!string.IsNullOrWhiteSpace(padre.Tipo.Value) &&
                                !padre.Tipo.Value.Equals(dto.Tipo.Value.ToString()))
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_Tipo_HierarCheck);
                                result.Add("Tipo", msg);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dto.Tipo.Value) && dto.Tipo.Value != "0" && dto.Tipo.Value != "1" && dto.Tipo.Value != "2" &&
                        dto.Tipo.Value != "3" && dto.Tipo.Value != "4" && dto.Tipo.Value != "5")
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_Tipo_Restr);
                        result.Add("Tipo", msg);
                    }

                    #endregion

                    #region Naturaleza

                    if (dto.Naturaleza.Value != 1 && dto.Naturaleza.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_Natural_Restr);
                        result.Add("Naturaleza", msg);
                    }

                    #endregion

                    #region ConceptoSaldoID

                    //Cuando ConceptoSaldoID es igual a Doc Int o Doc Ext activa los indicadores de Tercero y DocControl
                    int conceptoSaldo = AppMasters.glConceptoSaldo;
                    DTO_glConceptoSaldo saldo =
                        (DTO_glConceptoSaldo)
                        _bc.GetMasterDTO(AppMasters.MasterType.Simple, conceptoSaldo, false, dto.ConceptoSaldoID.Value,
                                         true);
                    //  TerceroSaldosInd esta habilitado, el coSaldoControl debe ser 4
                    if (saldo.coSaldoControl.Value == 4)
                    {
                        if (dto.TerceroSaldosInd.Value == true)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Co_PlanCta_ConceptoSaldo_SdoCtrl_CheckValue);
                            result.Add("ConceptoSaldoID2", msg);
                        }
                    }
                    if (saldo != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dto.ConceptoSaldoID.Value.ToString()) &&
                            (saldo.coSaldoControl.Value == (byte)SaldoControl.Doc_Externo ||
                             saldo.coSaldoControl.Value == (byte)SaldoControl.Doc_Interno) &&
                            (dto.TerceroSaldosInd.Value == false || dto.TerceroInd.Value == false))
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Co_PlanCta_ConceptoSaldo_CheckValue);
                            result.Add("ConceptoSaldoID", msg);
                        }
                    }
                    //Verifica si el Concepto saldo ha sido cambiado
                    bool active = !dto.ActivoInd.Value.Value;
                    DTO_coPlanCuenta cuentaOld =
                        (DTO_coPlanCuenta)
                        _bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, dto.ID.Value,
                                         active);
                    if (cuentaOld != null)
                        if (dto.ConceptoSaldoID.Value != cuentaOld.ConceptoSaldoID.Value)
                        {
                            //Revisa si el concepto saldo cambiado tiene saldos pendientes
                            if (_bc.AdministrationModel.Saldo_ExistsByConcSaldo(dto.ConceptoSaldoID.Value, cuentaOld.ConceptoSaldoID.Value, dto.ID.Value))
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages,
                                                      MasterMessages.Gl_ConceptoSaldo_ID_SaldoExist);
                                result.Add("ConceptoSaldoID", msg);
                            }
                            else
                            {
                                ////Sino tiene saldos trae los coDocumentos filtrados con la cuenta actual
                                //List<DTO_glConsultaFiltro> filtrosCoDocumento = new List<DTO_glConsultaFiltro>();
                                //filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
                                //{
                                //    CampoFisico = "CuentaLOC",
                                //    OperadorFiltro = OperadorFiltro.Igual,
                                //    ValorFiltro = dto.ID.Value,
                                //    OperadorSentencia = "OR"
                                //});
                                //filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
                                //{
                                //    CampoFisico = "CuentaEXT",
                                //    OperadorFiltro = OperadorFiltro.Igual,
                                //    ValorFiltro = dto.ID.Value,
                                //    OperadorSentencia = "OR"
                                //});
                                //filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
                                //{
                                //    CampoFisico = "CuentaPresupuesto",
                                //    OperadorFiltro = OperadorFiltro.Igual,
                                //    ValorFiltro = dto.ID.Value
                                //});
                                //long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coDocumento, null,
                                //                                                        null, true);
                                //var coDocumento =
                                //    _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coDocumento, count, 1, null,
                                //                                                  filtrosCoDocumento, true).ToList();

                                //foreach (var item in coDocumento)
                                //{
                                //    //Verifica que el módulo de la cuenta corresponda al módulo del coDocumento
                                //    DTO_coDocumento cDoc = (DTO_coDocumento)item;
                                //    DTO_glDocumento documento =
                                //        (DTO_glDocumento)
                                //        _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true,
                                //                         cDoc.DocumentoID.Value, active);
                                //    if (saldo.ModuloID.Value != documento.ModuloID.Value)
                                //    {
                                //        res = false;
                                //        string msgcoDoc = _bc.GetResource(LanguageTypes.Messages,
                                //                                          MasterMessages.
                                //                                              Co_PlanCta_ConceptoSaldo_ModuleInvalid);
                                //        msg = string.Format(msgcoDoc, cDoc.ID.Value);
                                //        result.Add("ConceptoSaldoID", msg);
                                //        break;
                                //    }
                                //}
                            }
                        }

                    #endregion

                    #region OrigenMonetario

                    if (dto.OrigenMonetario.Value != 1 && dto.OrigenMonetario.Value != 2 &&
                        dto.OrigenMonetario.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_OrigenMon_Restr);
                        result.Add("OrigenMonetario", msg);
                    }

                    #endregion

                    #region Campos de Ajustes(Aj)

                    //Valida que el indicador de Ajustes sea el mismo que existe en glControl por defecto
                    //AjCambioDocumentoInd / AjCambioTerceroInd / AjCambioProyectoInd / AjCambioCentroCostoInd / AjCambioLinNegInd / AjCambioLinPresupuestal
                    bool indAj = true;
                    if (!_bc.AdministrationModel.MultiMoneda)
                    {
                        indAj = false;
                        if (!string.IsNullOrWhiteSpace(dto.AjCambioTerceroInd.Value.ToString()) &&
                            dto.AjCambioTerceroInd.Value != indAj)
                        {
                            res = false;
                            string msgImp = _bc.GetResource(LanguageTypes.Messages,
                                                            MasterMessages.Co_PlanCta_AjCambioInd_ValidValue);
                            msg = string.Format(msgImp, indAj);
                            result.Add("AjCambioTerceroInd", msg);
                        }
                    }

                    #endregion

                    #region TerceroInd

                    ////Valida que el indicador de tercero este activado cuando hay Tipo de Impuesto
                    //if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.TerceroInd.Value == false||
                    //    (string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.TerceroInd.Value == true))
                    //{
                    //    res = false;

                    //    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_Tercero_ActiveOnly);
                    //    result.Add("TerceroInd", msg);
                    //}

                    #endregion

                    #region ImpuestoPorc

                    //Verifica que el campo ImpuestoPorc siempre sea mayor a 0 cuando existe Tipo de Impuesto
                    if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.ImpuestoPorc.Value <= 0 ||
                        dto.ImpuestoPorc.Value >= 1000)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_ImpPorc_ValueLimit);
                        result.Add("ImpuestoPorc", msg);
                    }

                    #endregion

                    #region MontoMinimo

                    //Verifica que el campo MOntoMinimo siempre sea mayor a 0 cuando existe Tipo de Impuesto
                    if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.MontoMinimo.Value < 0)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_MontoMinimo_ValueLimit);
                        result.Add("MontoMinimo", msg);
                    }

                    #endregion

                    #region NITCierreAnual

                    //Solicita llenar NITCierreAnual cuando tipo igual es a 2
                    //if (dto.Tipo.Value.Equals("2"))
                    //{
                    //    if (string.IsNullOrWhiteSpace(dto.NITCierreAnual.Value))
                    //    {
                    //        res = false;
                    //        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_NITCierreAn_NotEmpty);
                    //        result.Add("NITCierreAnual", msg);
                    //    }
                    //}

                    #endregion

                    #region coCuentaGrupo

                    if (!string.IsNullOrWhiteSpace(dto.CuentaGrupoID.Value))
                    {
                        int mascaraGrupoCta = Convert.ToInt32(dto.MascaraCta.Value);

                        int longiTotal = table.CodeLength(table.LevelsUsed());
                        int[] nivelUsed = table.CompleteLevelLengths();
                        if (!nivelUsed.Contains<int>(mascaraGrupoCta) && mascaraGrupoCta != 0)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Co_PlanCta_CuentaGrupo_InvalidMask);
                            result.Add("CuentaGrupoID", msg);
                        }
                    }


                    #endregion

                    #region TipoTercero

                    if (dto.TipoTercero.Value != 0 && dto.TipoTercero.Value != 1 && dto.TipoTercero.Value != 2 && dto.TipoTercero.Value != 3 &&
                        dto.TipoTercero.Value != null)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_RegimenFiscal_TipoTercero_Restr);
                        result.Add("TipoTercero", msg);
                    }

                    #endregion

                    #region ProyectoInd

                    if (dto.CuentaGrupoID.Value != null)
                    {
                        DTO_coCuentaGrupo dtoCtaGrupo =
                            (DTO_coCuentaGrupo)
                            _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCuentaGrupo, false,
                                             dto.CuentaGrupoID.Value, true);
                        if (dtoCtaGrupo != null && dtoCtaGrupo.CostoInd.Value.Value)
                        {
                            if (!dto.ProyectoInd.Value.Value && !dto.CentroCostoInd.Value.Value)
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_ProyectoOrCtoCostoUnchecked);
                                result.Add("ProyectoInd", msg);
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                #region coProyecto

                if (dtoObj.GetType() == typeof(DTO_coProyecto))
                {
                    DTO_coProyecto dto = (DTO_coProyecto)dtoObj;
                    if (dto != null)
                    {
                        #region FechaApertura-FechaCierre

                        if (dto.FApertura.Value > dto.FCierre.Value)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Proyecto_FCierre_NotLess);
                            result.Add("FCierre", msg);
                        }

                        #endregion

                        #region ProyectoTipo

                        if (dto.ProyectoTipo.Value != 1 && dto.ProyectoTipo.Value != 2 && dto.ProyectoTipo.Value != 3 &&
                            dto.ProyectoTipo.Value != 4 && dto.ProyectoTipo.Value != 5 && dto.ProyectoTipo.Value != 6 &&
                            dto.ProyectoTipo.Value != 7)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Proyecto_ProyTipo_Restr);
                            result.Add("ProyectoTipo", msg);
                        }

                        #endregion

                        #region DiasEstimados

                        //Verifica que el campo DiasEstimados siempre sea mayor a 0 
                        if (dto.DiasEstimados.Value < 0 &&
                            !string.IsNullOrWhiteSpace(dto.DiasEstimados.Value.ToString()))
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Proyecto_DiasEst_DayLimit);
                            result.Add("DiasEstimados", msg);
                        }

                        #endregion

                        #region TipoComercial

                        if (dto.TipoComercial.Value != 1 && dto.TipoComercial.Value != 2 && dto.TipoComercial.Value != 3)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Proyecto_TipoComercial_Restr);
                            result.Add("TipoComercial", msg);
                        }

                        #endregion

                        #region PresupuestalInd

                        //Valida el indicador de presupuesto para controlar los proyectos
                        if (dto.PresupuestalInd.Value.Value)
                        {

                            //Filtra los proyectos
                            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                            filtros.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "ActividadID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = dto.ActividadID.Value,
                                OperadorSentencia = "AND"

                            });
                            filtros.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "LocFisicaID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = dto.LocFisicaID.Value,

                            });
                            DTO_glConsulta consultaProy = new DTO_glConsulta();
                            consultaProy.Filtros.AddRange(filtros);
                            //long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coProyecto, consultaProy, null, true);
                            //var proyectos = _bc.AdministrationModel.MasterHierarchy_GetPaged(AppMasters.coProyecto, count, 1, consultaProy, null, true).ToList();
                            //if (proyectos.Count > 0)
                            //{
                            //    res = false;
                            //    msg = _bc.GetResource(LanguageTypes.Messages, "Ya existe un proyecto de presupuesto con la misma actividad y localidad fisica");
                            //    result.Add("PresupuestalInd", msg);
                            //}
                        }
                        #endregion
                    }
                }

                #endregion

                #region coRegimenFiscal

                if (dtoObj.GetType() == typeof(DTO_coRegimenFiscal))
                {
                    DTO_coRegimenFiscal dto = (DTO_coRegimenFiscal)dtoObj;

                    #region TipoTercero

                    if (dto.TipoTercero.Value != 1 && dto.TipoTercero.Value != 2 && dto.TipoTercero.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_RegimenFiscal_TipoTercero_Restr);
                        result.Add("TipoTercero", msg);
                    }

                    #endregion
                }

                #endregion

                #region coReporteLinea

                if (dtoObj.GetType() == typeof(DTO_coReporteLinea))
                {
                    DTO_coReporteLinea dto = (DTO_coReporteLinea)dtoObj;

                    #region SaldoMvto

                    if (dto.SaldoMvto.Value != 1 && dto.SaldoMvto.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ReporteLinea_SaldoMvto_Restr);
                        result.Add("SaldoMvto", msg);
                    }

                    #endregion
                }

                #endregion

                #region coTercero

                if (dtoObj.GetType() == typeof(DTO_coTercero))
                {
                    DTO_coTercero dto = (DTO_coTercero)dtoObj;

                    #region TerceroID (DigitoVerif)

                    //Calcula el digito de verificación por medio de una rutina dada
                    int dig = 0;
                    long idNIT = 0;
                    try
                    {
                        idNIT = Convert.ToInt64(dto.ID.Value);
                        dig = Evaluador.Nit_DV(idNIT.ToString());
                        if (!string.IsNullOrWhiteSpace(dto.DigitoVerif.Value))
                        {
                            if (dig.ToString() != dto.DigitoVerif.Value)
                            {
                                dto.DigitoVerif.Value = dig.ToString();
                                //string msgDig = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_DigVerif_Change);
                                //MessageBox.Show(string.Format(msgDig, dto.ID, dig));
                            }
                        }
                        else
                            dto.DigitoVerif.Value = dig.ToString();
                    }
                    catch (Exception e)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_ID_OnlyNumber);
                        result.Add("TerceroID", msg);
                    }

                    #endregion

                    #region DeclaraIVAInd

                    DTO_coRegimenFiscal regimen = (DTO_coRegimenFiscal)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, dto.ReferenciaID.Value, true);
                    if (regimen.TipoTercero.Value == 2 && dto.DeclaraIVAInd.Value.Value)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_DeclaraIVAInd_Invalid);
                        result.Add("DeclaraIVAInd", msg);
                    }

                    #endregion

                    #region BancoID_1

                    //Solicita llenar CuentaTipo_1 y BcoCtaNro_1 cuando hay Banco asignado
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && (string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString()) 
                        || (dto.CuentaTipo_1.Value !=3) && (string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString()))))
                    {
                        res = false;
                        string msgImp = _bc.GetResource(LanguageTypes.Messages,
                                                        MasterMessages.Co_Tercero_CtaTipo_AccountRequired);
                        msg = string.Format(msgImp, dto.BancoID_1.Value);
                        result.Add("BancoID_1", msg);
                    }
                    else if (string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && (!string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString()) || !string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString())))
                    {
                        res = false;
                        string msgBanco = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_BancoID1_BanKRequired);
                        msg = string.Format(msgBanco, dto.CuentaTipo_1.Value, dto.BcoCtaNro_1.Value);
                        result.Add("BancoID_1", msg);
                    }

                    #endregion

                    #region CuentaTipo_1

                    if (!string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && dto.CuentaTipo_1.Value != 1 &&
                        dto.CuentaTipo_1.Value != 2 && dto.CuentaTipo_1.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_CuentaTipo_Restr);
                        result.Add("CuentaTipo_1", msg);
                    }

                    #endregion

                    #region BancoID_2

                    //Solicita llenar CuentaTipo_2 y BcoCtaNro_2 cuando hay Banco asignado
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_2.Value) &&
                        (string.IsNullOrWhiteSpace(dto.CuentaTipo_2.Value.ToString()) ||
                         string.IsNullOrWhiteSpace(dto.BcoCtaNro_2.Value.ToString())))
                    {
                        res = false;
                        string msgBanco = _bc.GetResource(LanguageTypes.Messages,
                                                          MasterMessages.Co_Tercero_CtaTipo_AccountRequired);
                        msg = string.Format(msgBanco, dto.BancoID_2.Value);
                        result.Add("BancoID_2", msg);

                    }
                    else if (string.IsNullOrWhiteSpace(dto.BancoID_2.Value) &&
                             (!string.IsNullOrWhiteSpace(dto.CuentaTipo_2.Value.ToString()) ||
                              !string.IsNullOrWhiteSpace(dto.BcoCtaNro_2.Value.ToString())))
                    {
                        res = false;
                        string msgBanco = _bc.GetResource(LanguageTypes.Messages,
                                                          MasterMessages.Co_Tercero_BancoID2_BanKRequired);
                        msg = string.Format(msgBanco, dto.CuentaTipo_2.Value, dto.BcoCtaNro_2.Value);
                        result.Add("BancoID_2", msg);
                    }

                    #endregion

                    #region CuentaTipo_2

                    if (!string.IsNullOrWhiteSpace(dto.BancoID_2.Value) && dto.CuentaTipo_2.Value != 1 &&
                        dto.CuentaTipo_2.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_CuentaTipo_Restr);
                        result.Add("CuentaTipo_2", msg);
                    }

                    #endregion

                    #region Descriptivo = ApellidoPri + ApellidoSdo + NombrePri + NombreSdo

                    //Concatena los valores ingresados de Nombres y apellidos para crear la descripción teniendo el cuenta el campo PersonaNaturalInd de TerceroDocTipo
                    int terDoc = AppMasters.coTerceroDocTipo;
                    DTO_coTerceroDocTipo tipoDoc = (DTO_coTerceroDocTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, terDoc, false, dto.TerceroDocTipoID.Value, true);

                    string nameDesc = string.Empty,
                           ap1 = string.Empty,
                           ap2 = string.Empty,
                           nom1 = string.Empty,
                           nom2 = string.Empty;

                    if (tipoDoc != null)
                    {
                        if (tipoDoc.PersonaNaturalInd.Value == true)
                        {
                            if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value) &&
                                !string.IsNullOrWhiteSpace(dto.NombrePri.Value))
                            {

                                ap1 = dto.ApellidoPri.Value != "" ? dto.ApellidoPri.Value += " " : "";
                                ap2 = dto.ApellidoSdo.Value != "" ? dto.ApellidoSdo.Value += " " : "";
                                nom1 = dto.NombrePri.Value != "" ? dto.NombrePri.Value += " " : "";
                                nom2 = dto.NombreSdo.Value != "" ? dto.NombreSdo.Value += " " : "";
                                nameDesc = string.Format("{0}{1}{2}{3}", ap1, ap2, nom1, nom2);
                                nameDesc = nameDesc.TrimEnd();
                                if (dto.Descriptivo.Value != nameDesc)
                                {
                                    if (nameDesc.Length <= UDT_Descriptivo.MaxLength)
                                    {
                                        dto.Descriptivo.Value = nameDesc;
                                    }
                                    else
                                    {
                                        res = false;
                                        msg = _bc.GetResource(LanguageTypes.Messages,
                                                              MasterMessages.Co_Tercero_Descriptivo_MaxLength);
                                        result.Add("Descriptivo", msg);
                                    }
                                }
                            }
                            else
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages,
                                                      MasterMessages.Co_Tercero_NombreApellidoPri_Required);
                                result.Add("Descriptivo", msg);
                            }
                        }
                        else if (tipoDoc.PersonaNaturalInd.Value == false)
                        {
                            if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value))
                            {
                                dto.Descriptivo.Value = dto.ApellidoPri.Value;
                            }
                            else
                            {
                                res = false;
                                msg = _bc.GetResource(LanguageTypes.Messages,
                                                      MasterMessages.Co_Tercero_ApellidoPriRazon_Required);
                                result.Add("Descriptivo", msg);
                            }
                        }
                    }

                    #endregion

                    #region TerceroDocTipoID

                    //Valida El Documento de Tercero verificando si es personal natural
                    int TipoTerDoc = AppMasters.coTerceroDocTipo;
                    DTO_coTerceroDocTipo tipoterc = (DTO_coTerceroDocTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, TipoTerDoc, false, dto.TerceroDocTipoID.Value, true);
                    if (tipoterc != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dto.TerceroDocTipoID.Value) && tipoterc.PersonaNaturalInd.Value == false
                            && (!string.IsNullOrWhiteSpace(dto.ApellidoSdo.Value)
                            || !string.IsNullOrWhiteSpace(dto.NombrePri.Value)
                            || !string.IsNullOrWhiteSpace(dto.NombreSdo.Value)))
                        {
                            res = false;
                            string msgReg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_TerceroDocID_NotNaturalPerson);
                            msg = string.Format(msgReg, dto.BancoID_1.Value);
                            result.Add("TerceroDocTipoID", msg);
                        }
                    }

                    #endregion

                    #region CECorporativo

                    if (!string.IsNullOrWhiteSpace(dto.CECorporativo.Value))
                    {
                        bool isFormat = Regex.IsMatch(dto.CECorporativo.Value,
                                                      @"[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[A-Za-z.]{2,6}");
                        if (!isFormat)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Co_Tercero_CECorporativo_EmailInvalidFormat);
                            result.Add("CECorporativo", msg);
                        }
                    }

                    #endregion

                    #region RepLegalCE

                    if (!string.IsNullOrWhiteSpace(dto.RepLegalCE.Value))
                    {
                        bool isFormat = Regex.IsMatch(dto.RepLegalCE.Value, @"[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[A-Za-z.]{2,6}");
                        if (!isFormat)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_RepLegalCE_EmailInvalidFormat);
                            result.Add("RepLegalCE", msg);
                        }
                    }
                    #endregion

                    #region ResponsableTercero
                    int terce = AppMasters.coTercero;
                    bool isNew = true;

                    int coTer = AppMasters.coTercero;
                    DTO_coTercero idTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, coTer, false, dto.ID.Value, true);

                    if (idTercero == null)
                        isNew = true;
                    else
                        isNew = false;
                    DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(_bc.AdministrationModel.User.ID.Value);
                    _bc.AdministrationModel.User = user;
                    bool usuario = _bc.AdministrationModel.User.ResponsableTercerosInd.Value.Value;

                    if (isNew)
                    {
                        if (usuario)
                            dto.UsuarioResponsable.Value = _bc.AdministrationModel.User.ID.Value;
                        else
                            dto.UsuarioResponsable.Value = string.Empty;
                    }
                    else
                    {
                        if (usuario)
                        {
                            //if (dto.UsuarioResponsable.Value != string.Empty)
                            //    _bc.AdministrationModel.User.ID.Value = dto.UsuarioResponsable.Value;
                            //else
                            //{
                                dto.UsuarioResponsable.Value = _bc.AdministrationModel.User.ID.Value;
                            //}
                        }
                        else
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Tercero_RespTercero_InvalidResp);
                            result.Add("ResponsableTercero", msg);
                        }
                    }
                    #endregion
                }

                #endregion

                #region coValIva

                if (dtoObj.GetType() == typeof(DTO_coValIVA))
                {
                    //DTO_coValIVA dto = (DTO_coValIVA)dtoObj;
                    //#region Cuenta ReteIva
                    //if (!string.IsNullOrWhiteSpace(dto.CuentaReteIVA.Value) && string.IsNullOrWhiteSpace(dto.CuentaCostoReteIVA.Value))
                    //{
                    //    res = false;
                    //    _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ValIVA_ReteIVA_NotEmpty);
                    //    result.Add("CuentaReteIVA", msg);
                    //}
                    //else if (string.IsNullOrWhiteSpace(dto.CuentaReteIVA.Value) && !string.IsNullOrWhiteSpace(dto.CuentaCostoReteIVA.Value))
                    //{
                    //    res = false;
                    //    _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_ValIVA_ReteIVA_Empty);
                    //    result.Add("CuentaReteIVA", msg);
                    //}
                    //#endregion
                }

                #endregion

                #region coPlanillaConsolidacion

                if (dtoObj.GetType() == typeof(DTO_coPlanillaConsolidacion))
                {
                    Dictionary<string, string> pksEmpresa = new Dictionary<string, string>();
                    Dictionary<string, string> pksCtroCosto = new Dictionary<string, string>();
                    Dictionary<string, string> pksEmpresaGrupo = new Dictionary<string, string>();
                    int ctroCosto = AppMasters.coCentroCosto;
                    DTO_coPlanillaConsolidacion dto = (DTO_coPlanillaConsolidacion)dtoObj;
                    bool empresaIgual = false;
                    int glEmpresaGrupo = AppMasters.glEmpresa;
                    #region Obtiene el grupo de empresas y lo valida con la empresa que viene

                    DTO_glEmpresa grpEmpresa = (DTO_glEmpresa)_bc.GetMasterDTO(AppMasters.MasterType.Simple, glEmpresaGrupo, false, dto.EmpresaID.Value, true);
                    DTO_glEmpresa empresa = _bc.AdministrationModel.Empresa;
                    pksEmpresaGrupo.Add("EmpresaGrupoID", grpEmpresa.EmpresaGrupoID_.Value);
                    DTO_coPlanillaConsolidacion empresaGrupoDTO = (DTO_coPlanillaConsolidacion)_bc.GetMasterComplexDTO(AppMasters.coPlanillaConsolidacion, pksEmpresaGrupo, true);
                    if (empresaGrupoDTO != null)
                        empresaIgual = true;

                    if (!empresaIgual)
                    {
                        pksEmpresa.Add("EmpresaID", dto.EmpresaID.Value);
                        DTO_coPlanillaConsolidacion empresaDTO = (DTO_coPlanillaConsolidacion)_bc.GetMasterComplexDTO(AppMasters.coPlanillaConsolidacion, pksEmpresa, true);
                        if (empresaDTO != null)
                        {
                            string msgReg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanillaCons_Company_Assigned);
                            msg = string.Format(msgReg, dto.EmpresaID.Value);
                            result.Add("EmpresaID", msg);
                        }
                    }
                    else if (empresa.ID.Value != dto.EmpresaID.Value)
                    {
                        pksEmpresa.Add("EmpresaID", dto.EmpresaID.Value);
                        pksCtroCosto.Add("CentroCostoID", dto.CentroCostoID.Value);
                        DTO_coPlanillaConsolidacion empresaDTO = (DTO_coPlanillaConsolidacion)_bc.GetMasterComplexDTO(AppMasters.coPlanillaConsolidacion, pksEmpresa, true);
                        if (empresaDTO != null)
                        {
                            string msgReg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanillaCons_Company_Assigned);
                            msg = string.Format(msgReg, dto.EmpresaID.Value);
                            result.Add("EmpresaID", msg);
                        }
                        DTO_coPlanillaConsolidacion ctroCostoDTO = (DTO_coPlanillaConsolidacion)_bc.GetMasterComplexDTO(AppMasters.coPlanillaConsolidacion, pksCtroCosto, true);
                        if (ctroCostoDTO != null)
                        {
                            string msgRe = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanillaCons_CentroCosto_Assigned);
                            msg = string.Format(msgRe, dto.EmpresaID.Value);
                            result.Add("CentroCostoID", msg);
                        }
                    }
                    else
                    {
                        string msgReg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanillaCons_EmpresaGrupo_Assigned);
                        msg = string.Format(msgReg, dto.EmpresaID.Value);
                        result.Add("EmpresaGrupo", msg);
                    }


                    //if (empresaGrupoDTO.EmpresaGrupoID.Value != empresa.ID.Value)
                    //{
                    //    DTO_coPlanillaConsolidacion empresaDTO = (DTO_coPlanillaConsolidacion)_bc.GetMasterComplexDTO(AppMasters.coPlanillaConsolidacion, pksEmpresa, true);
                    //    if (empresaDTO != null)
                    //    {
                    //        string msgReg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanillaCons_Company_Assigned);
                    //        msg = string.Format(msgReg, dto.EmpresaID.Value);
                    //        result.Add("EmpresaID", msg);
                    //    }
                    //}


                    #endregion
                }

                #endregion

                #region cpAnticipoTipo

                if (dtoObj.GetType() == typeof(DTO_cpAnticipoTipo))
                {
                    DTO_cpAnticipoTipo dto = (DTO_cpAnticipoTipo)dtoObj;

                    #region coDocumentoID

                    DTO_coDocumento doc =
                        (DTO_coDocumento)
                        _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false,
                                         dto.coDocumentoID.Value, true);

                    if (doc != null)
                    {
                        DTO_coPlanCuenta ctaLoc =
                            (DTO_coPlanCuenta)
                            _bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false,
                                             doc.CuentaLOC.Value, true);
                        DTO_coPlanCuenta ctaExt =
                            (DTO_coPlanCuenta)
                            _bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false,
                                             doc.CuentaEXT.Value, true);
                        if (ctaLoc != null && ctaExt != null)
                        {
                            DTO_glConceptoSaldo saldoLoc =
                                (DTO_glConceptoSaldo)
                                _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false,
                                                 ctaLoc.ConceptoSaldoID.Value, true);
                            DTO_glConceptoSaldo saldoExt =
                                (DTO_glConceptoSaldo)
                                _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false,
                                                 ctaLoc.ConceptoSaldoID.Value, true);
                            if (saldoLoc != null && saldoExt != null)
                            {
                                if (saldoLoc.coSaldoControl.Value != (byte)SaldoControl.Doc_Externo)
                                {
                                    res = false;
                                    msg = _bc.GetResource(LanguageTypes.Messages,
                                                          MasterMessages.Gl_ConceptoSaldo_coDocumento_Restr);
                                    result.Add("coDocumentoID", msg);
                                }
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                #region cpCargoEspecial

                if (dtoObj.GetType() == typeof(DTO_cpCargoEspecial))
                {
                    DTO_cpCargoEspecial dto = (DTO_cpCargoEspecial)dtoObj;

                    #region CargoTipo

                    if (dto.CargoTipo.Value != 1 && dto.CargoTipo.Value != 2 && dto.CargoTipo.Value != 3 &&
                        dto.CargoTipo.Value != 4 && dto.CargoTipo.Value != 5 && dto.CargoTipo.Value != 6 &&
                        dto.CargoTipo.Value != 7 && dto.CargoTipo.Value != 8 && dto.CargoTipo.Value != 9)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_CargoEspecial_CargoTipo_Restr);
                        result.Add("CargoTipo", msg);
                    }

                    #endregion
                }

                #endregion

                #region cpConceptoCXP

                if (dtoObj.GetType() == typeof(DTO_cpConceptoCXP))
                {

                    DTO_cpConceptoCXP dto = (DTO_cpConceptoCXP)dtoObj;
                    DTO_coDocumento documento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, true, dto.coDocumentoID.Value, true);

                    #region ConceptoTipo

                    if (dto.ConceptoTipo.Value != 1 && dto.ConceptoTipo.Value != 2
                        && dto.ConceptoTipo.Value != 3 && dto.ConceptoTipo.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cp_ConceptoCXP_TipoConcepto_Restr);
                        result.Add("ConceptoTipo", msg);
                    }

                    #endregion

                    #region Documento Contable

                    // Verifica que el documento Contable sea 21 o 24
                    if (documento.DocumentoID.Value != AppDocuments.CausarFacturas.ToString() && documento.DocumentoID.Value != AppDocuments.NotaCreditoCxP.ToString())
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cp_ConceptoCXP_DocumentoID_Restr);
                        result.Add("DocumentoID", msg);
                    }

                    #endregion
                }

                #endregion

                #region cpDistribuyeImpLocal

                if (dtoObj.GetType() == typeof(DTO_cpDistribuyeImpLocal))
                {
                    DTO_cpDistribuyeImpLocal dto = (DTO_cpDistribuyeImpLocal)dtoObj;
                    DTO_glConsulta consulta = new DTO_glConsulta();

                    //Sino tiene saldos trae los coDocumentos filtrados con la cuenta actual
                    List<DTO_glConsultaFiltro> filtrosCP = new List<DTO_glConsultaFiltro>();
                    filtrosCP.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "LugarGeograficoORI",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = dto.LugarGeograficoORI.Value,

                    });
                    consulta.Filtros.AddRange(filtrosCP);
                    long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.cpDistribuyeImpLocal, null, null,
                                                                            true);
                    var CxP =
                        _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.cpDistribuyeImpLocal, count, 1,
                                                                       consulta, true).ToList();


                    decimal porcentajeAdd = dto.Porcentaje.Value.Value;
                    decimal porcentajesDTO = 0;

                    foreach (var item in CxP)
                    {
                        DTO_cpDistribuyeImpLocal dtos = (DTO_cpDistribuyeImpLocal)item;
                        porcentajesDTO += dtos.Porcentaje.Value.Value;
                    }
                    if (dto.ActivoInd.Value.Value)
                    {
                        decimal porcentajeTotal = porcentajesDTO += porcentajeAdd;
                        if (porcentajeTotal != 100)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Cp_Distribuye_PorcentajeEqual100);
                            result.Add("cpDistribuyeImpLocal", msg);
                        }
                    }
                    else
                    {
                        decimal porcentajeTotal = porcentajesDTO;
                        List<DTO_MasterComplex> igualReplica =
                            CxP.Where(x => x.ReplicaID.Value == dto.ReplicaID.Value).ToList();

                        if (igualReplica.Count > 0)
                        {
                            porcentajeTotal = -dto.Porcentaje.Value.Value;
                            foreach (var item in CxP)
                            {
                                DTO_cpDistribuyeImpLocal dtos = (DTO_cpDistribuyeImpLocal)item;
                                dtos.ActivoInd.Value = false;
                                _bc.AdministrationModel.MasterComplex_Update(AppMasters.cpDistribuyeImpLocal, dtos);
                            }

                        }

                        if (porcentajeTotal != 100)
                        {
                            //res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Cp_Distribuye_PorcentajeEqual100);
                            result.Add("cpDistribuyeImpLocal", msg);
                        }
                    }
                }

                #endregion

                #region faConceptos

                if (dtoObj.GetType() == typeof(DTO_faConceptos))
                {
                    DTO_faConceptos dto = (DTO_faConceptos)dtoObj;

                    #region TipoConcepto

                    if (dto.TipoConcepto.Value != 1 && dto.TipoConcepto.Value != 2 && dto.TipoConcepto.Value != 3 &&
                        dto.TipoConcepto.Value != 4 && dto.TipoConcepto.Value != 5 && dto.TipoConcepto.Value != 6)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Fa_Conceptos_TipoConcepto_Restr);
                        result.Add("TipoConcepto ", msg);
                    }

                    #endregion
                }

                #endregion

                #region faFacturaTipo

                if (dtoObj.GetType() == typeof(DTO_faFacturaTipo))
                {
                    DTO_faFacturaTipo dto = (DTO_faFacturaTipo)dtoObj;
                    DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, dto.coDocumentoID.Value, true);

                    #region coDocumentoID

                    if (coDoc.DocumentoID.Value != "41" && coDoc.DocumentoID.Value != "42")
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Fa_FacturaTipo_Documento_Checked);
                        result.Add("coDocumentoID", msg);
                    }

                    #endregion

                }

                #endregion

                #region glActividadFlujo

                if (dtoObj.GetType() == typeof(DTO_glActividadFlujo))
                {
                    DTO_glActividadFlujo dto = (DTO_glActividadFlujo)dtoObj;

                    #region Alarmas

                    if (Convert.ToBoolean(dto.Alarma1Ind.Value))
                    {
                        if (dto.UnidadTiempo.Value == 0)
                        {
                            string msgNO = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_ActividadFlujoAlarma);
                            MessageBox.Show(msgNO);
                            result.Add("Alarma1Ind", msg);
                        }
                        if (dto.AlarmaPeriodo1.Value == 0)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_AlarmaPeriodoDiferent);
                        }
                    }
                    if (Convert.ToBoolean(dto.Alarma2Ind.Value))
                    {
                        if (dto.UnidadTiempo.Value == 0)
                        {
                            string msgNO = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_ActividadFlujoAlarma);
                            MessageBox.Show(msgNO);
                            result.Add("Alarma2Ind", msg);
                        }
                        if (dto.AlarmaPeriodo2.Value == 0)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_AlarmaPeriodoDiferent);
                        }
                    }
                    if (Convert.ToBoolean(dto.Alarma3Ind.Value))
                    {
                        if (dto.UnidadTiempo.Value == 0)
                        {
                            string msgNO = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_ActividadFlujoAlarma);
                            MessageBox.Show(msgNO);
                            result.Add("Alarma3Ind", msg);
                        }
                        if (dto.AlarmaPeriodo3.Value == 0)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_AlarmaPeriodoDiferent);
                        }
                    }
                    #endregion
                }
                #endregion

                #region glConceptoSaldo

                if (dtoObj.GetType() == typeof(DTO_glConceptoSaldo))
                {
                    DTO_glConceptoSaldo dto = (DTO_glConceptoSaldo)dtoObj;
                    //DTO_coCuentaSaldo dto = ;

                    #region coSaldoControl

                    if (dto.coSaldoControl.Value != 1 && dto.coSaldoControl.Value != 2 && dto.coSaldoControl.Value != 3 &&
                        dto.coSaldoControl.Value != 4 && dto.coSaldoControl.Value != 5 && dto.coSaldoControl.Value != 6 &&
                        dto.coSaldoControl.Value != 7)
                    {

                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_ConceptoSaldo_coSaldoCtrl_Restr);
                        result.Add("coSaldoControl", msg);
                    }
                    #endregion
                    #region NumeroComp
                    if (dto.NumeroComp.Value > 20 || dto.NumeroComp.Value < 0)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_ConceptoSaldo_NumeroComp);
                        result.Add("NumeroComp", msg);
                    }
                    #endregion
                    #region ID

                    DTO_glConceptoSaldo conceptoExist =
                        (DTO_glConceptoSaldo)
                        _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, dto.ID.Value,
                                         true);
                    if (conceptoExist != null)
                        if (_bc.AdministrationModel.Saldo_ExistsByConcSaldo(null, dto.ID.Value, null))
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_ConceptoSaldo_ID_SaldoExist);
                            result.Add("coSaldoControl", msg);
                        }

                    #endregion
                }

                #endregion

                #region glDatosMensuales

                if (dtoObj.GetType() == typeof(DTO_glDatosMensuales))
                {
                    DTO_glDatosMensuales dto = (DTO_glDatosMensuales)dtoObj;
                    if (dto.Periodo.Value != dto.PeriodoID.Value)
                        dto.PeriodoID.Value = dto.Periodo.Value;
                    if (dto.Tasa1.Value < 0 || dto.Tasa1.Value == null)
                        dto.Tasa1.Value = 0;
                    if (dto.Tasa2.Value < 0 || dto.Tasa2.Value == null)
                        dto.Tasa2.Value = 0;
                    if (dto.Tasa3.Value < 0 || dto.Tasa3.Value == null)
                        dto.Tasa3.Value = 0;
                    if (dto.Tasa4.Value < 0 || dto.Tasa4.Value == null)
                        dto.Tasa4.Value = 0;
                    if (dto.Tasa5.Value < 0 || dto.Tasa5.Value == null)
                        dto.Tasa5.Value = 0;
                    if (dto.Tasa6.Value < 0 || dto.Tasa6.Value == null)
                        dto.Tasa6.Value = 0;
                    if (dto.Tasa7.Value < 0 || dto.Tasa7.Value == null)
                        dto.Tasa7.Value = 0;
                    if (dto.Tasa8.Value < 0 || dto.Tasa8.Value == null)
                        dto.Tasa8.Value = 0;
                    if (dto.Tasa9.Value < 0 || dto.Tasa9.Value == null)
                        dto.Tasa9.Value = 0;
                    if (dto.Tasa10.Value < 0 || dto.Tasa10.Value == null)
                        dto.Tasa10.Value = 0;
                    if (dto.Valor1.Value < 0 || dto.Valor1.Value == null)
                        dto.Valor1.Value = 0;
                    if (dto.Valor2.Value < 0 || dto.Valor2.Value == null)
                        dto.Valor2.Value = 0;
                    if (dto.Valor3.Value < 0 || dto.Valor3.Value == null)
                        dto.Valor3.Value = 0;
                    if (dto.Valor4.Value < 0 || dto.Valor4.Value == null)
                        dto.Valor4.Value = 0;
                    if (dto.Valor5.Value < 0 || dto.Valor5.Value == null)
                        dto.Valor5.Value = 0;
                    if (dto.Valor6.Value < 0 || dto.Valor6.Value == null)
                        dto.Valor6.Value = 0;
                    if (dto.Valor7.Value < 0 || dto.Valor7.Value == null)
                        dto.Valor7.Value = 0;
                    if (dto.Valor8.Value < 0 || dto.Valor8.Value == null)
                        dto.Valor8.Value = 0;
                    if (dto.Valor9.Value < 0 || dto.Valor9.Value == null)
                        dto.Valor9.Value = 0;
                    if (dto.Valor10.Value < 0 || dto.Valor10.Value == null)
                        dto.Valor10.Value = 0;
                }

                #endregion

                #region glDiasFestivos

                if (dtoObj.GetType() == typeof(DTO_glDiasFestivos))
                {
                    DTO_glDiasFestivos dto = (DTO_glDiasFestivos)dtoObj;
                    if (dto.Fecha.Value != dto.DiasFestivoID.Value)
                        dto.DiasFestivoID.Value = dto.Fecha.Value;
                }
                #endregion

                #region glEmpresa

                if (dtoObj.GetType() == typeof(DTO_glEmpresa))
                {
                    DTO_glEmpresa dto = (DTO_glEmpresa)dtoObj;
                    DTO_glEmpresa emp =
                        (DTO_glEmpresa)
                        _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, false, dto.ID.Value, false);

                    if (emp != null && dto.ID.Value != dto.EmpresaGrupoID_.Value)
                    {
                        res = false;
                        string rsx = _bc.GetResource(LanguageTypes.Messages, "La Empresa Grupo debe ser igual al ID de la empresa");
                        msg = string.Format(rsx, dto.ID.Value, emp.EmpresaGrupoID_.Value);
                        result.Add("EmpresaGrupoID", msg);
                    }

                    if (emp != null)
                    {
                        DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "EmpresaID",
                            OperadorFiltro = "=",
                            ValorFiltro = dto.ID.Value,
                            OperadorSentencia = string.Empty
                        };

                        DTO_glConsulta query = new DTO_glConsulta();
                        query.Filtros.Add(filter);

                        long cont = _bc.AdministrationModel.aplBitacoraCountFiltered(query);
                        if (cont > 0)
                        {
                            res = false;
                            string rsx = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Empresa_InvalidGE);
                            msg = string.Format(rsx, dto.ID.Value, emp.EmpresaGrupoID_.Value);
                            result.Add("EmpresaGrupoID", msg);
                        }
                    }
                }

                #endregion

                #region glTabla

                if (dtoObj.GetType() == typeof(DTO_glTabla))
                {
                    DTO_glTabla dto = (DTO_glTabla)dtoObj;
                    int totalLength = 0;
                    for (int i = 0; i < DTO_glTabla.MaxLevels; i++)
                    {
                        if (dto.LevelLengths()[i] != null && dto.LevelLengths()[i] > 0)
                        {
                            if (string.IsNullOrWhiteSpace(dto.LevelDescs()[i]))
                            {
                                res = false;
                                string msgReg = _bc.GetResource(LanguageTypes.Messages,
                                                                MasterMessages.Gl_Tabla_LengthDescriptionRequired);
                                result.Add("descrNivel" + (i + 1), msgReg);
                            }
                            else
                            {
                                if (dto.LevelDescs().Count(x => x.Equals(dto.LevelDescs()[i])) > 1)
                                {
                                    res = false;
                                    string msgReg = _bc.GetResource(LanguageTypes.Messages,
                                                                    MasterMessages.Gl_Tabla_DescriptionNotRepeat);
                                    result.Add("descrNivel" + (i + 1), msgReg);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(dto.LevelDescs()[i]))
                            {
                                res = false;
                                string msgReg = _bc.GetResource(LanguageTypes.Messages,
                                                                MasterMessages.Gl_Tabla_LengthDescriptionRequired);
                                result.Add("descrNivel" + (i + 1), msgReg);
                            }
                        }
                        if (dto.LevelLengths()[i] != null && dto.LevelLengths()[i] > 0)
                            totalLength += dto.LevelLengths()[i];

                    }
                    try
                    {
                        int max = _bc.AdministrationModel.MasterProperties[Convert.ToInt32(dto.ID.Value)].IDLongitudMax;
                        if (max < totalLength)
                        {
                            res = false;
                            string msgReg =
                                _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Tabla_LengthExceedID) + " ( " +
                                max + " ) ";
                            result.Add("lonNivel1", msgReg);
                        }
                    }
                    catch
                    {
                        ;
                    }
                }

                #endregion

                #region glHorarioTrabajo

                if (dtoObj.GetType() == typeof(DTO_glHorarioTrabajo))
                {
                    DTO_glHorarioTrabajo dto = (DTO_glHorarioTrabajo)dtoObj;

                    #region EntradaHora

                    if (dto.EntradaHora.Value != null)
                    {
                        //bool isFormat = Regex.IsMatch(dto.EntradaHora.Value.ToString(), "[0-2][0-9]");
                        if (dto.EntradaHora.Value < 0 || dto.EntradaHora.Value > 23)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Horario_RankHour);
                            result.Add("EntradaHora", msg);
                        }
                    }

                    #endregion

                    #region SalidaHora

                    if (dto.SalidaHora.Value != null)
                    {
                        // bool isFormat = Regex.IsMatch(dto.SalidaHora.Value.ToString(), "[0-2][0-9]");
                        if (dto.SalidaHora.Value < 0 || dto.SalidaHora.Value > 23)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Horario_RankHour);
                            result.Add("SalidaHora", msg);
                        }
                    }

                    #endregion

                    #region EntradaMinutos

                    if (dto.EntradaMinutos.Value != null)
                    {
                        // bool isFormat = Regex.IsMatch(dto.EntradaMinutos.Value.ToString(), "[0-5][0-9]");
                        if (dto.EntradaMinutos.Value < 0 || dto.EntradaMinutos.Value > 59)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Horario_RankMinute);
                            result.Add("EntradaMinutos", msg);
                        }
                    }

                    #endregion

                    #region SalidaMinutos

                    if (dto.SalidaMinutos.Value != null)
                    {
                        // bool isFormat = Regex.IsMatch(dto.SalidaMinutos.Value.ToString(), "[0-5][0-9]");
                        if (dto.SalidaMinutos.Value < 0 || dto.SalidaMinutos.Value > 59)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_Horario_RankMinute);
                            result.Add("SalidaMinutos", msg);
                        }
                    }

                    #endregion
                }

                #endregion

                #region glDocumentoAnexo

                if (dtoObj.GetType() == typeof(DTO_glDocumentoAnexo))
                {
                    DTO_glDocumentoAnexo dto = (DTO_glDocumentoAnexo)dtoObj;

                    #region TipoDocumento

                    //if (dto.TipoDocumento.Value != 0 && dto.TipoDocumento.Value != 1 && dto.TipoDocumento.Value != 2 &&
                    //    dto.TipoDocumento.Value != 3)
                    //{
                    //    res = false;
                    //    msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Gl_DocumentoAnexo_TipoDoc_Restr);
                    //    result.Add("TipoDocumento", msg);
                    //}

                    #endregion
                }

                #endregion

                #region inRefTipo

                if (dtoObj.GetType() == typeof(DTO_inRefTipo))
                {
                    DTO_inRefTipo dto = (DTO_inRefTipo)dtoObj;

                    #region TipoInv

                    if (dto.TipoInv.Value != 0 && dto.TipoInv.Value != 1 && dto.TipoInv.Value != 2 &&
                        dto.TipoInv.Value != 3 && dto.TipoInv.Value != 4 && dto.TipoInv.Value != 5 &&
                        dto.TipoInv.Value != 6 && dto.TipoInv.Value != 7 && dto.TipoInv.Value != 8 &&
                        dto.TipoInv.Value != 9)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.In_RefTipo_TipoInv_Restr);
                        result.Add("TipoInv", msg);
                    }

                    #endregion

                    #region ControlEspecial

                    if (dto.ControlEspecial.Value != 0 && dto.ControlEspecial.Value != 1 &&
                        dto.ControlEspecial.Value != 2 && dto.ControlEspecial.Value != 3 &&
                        dto.ControlEspecial.Value != 4 && dto.ControlEspecial.Value != 5 &&
                        dto.ControlEspecial.Value != 6)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.In_RefTipo_ControlEspecial_Restr);
                        result.Add("ControlEspecial", msg);
                    }

                    #endregion

                    #region NumParametros

                    #endregion

                    #region ControlImportacion

                    if (dto.ControlImportacion.Value != 0 && dto.ControlImportacion.Value != 1 &&
                        dto.ControlImportacion.Value != 2 && dto.ControlImportacion.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.In_RefTipo_ControlImportacion_Restr);
                        result.Add("ControlImportacion", msg);
                    }

                    #endregion
                }

                #endregion

                #region inBodega

                if (dtoObj.GetType() == typeof(DTO_inBodega))
                {
                    DTO_inBodega dto = (DTO_inBodega)dtoObj;

                    #region BodegaTipoID

                    DTO_inBodegaTipo inBodTipo = (DTO_inBodegaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, dto.BodegaTipoID.Value, true);
                    DTO_inCosteoGrupo inCosteo = (DTO_inCosteoGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, dto.CosteoGrupoInvID.Value, true);

                    if ((inBodTipo.BodegaTipo.Value == (byte)TipoBodega.PuertoFOB || inBodTipo.BodegaTipo.Value == (byte)TipoBodega.ZonaFranca ||
                        inBodTipo.BodegaTipo.Value == (byte)TipoBodega.Transito || inBodTipo.BodegaTipo.Value == (byte)TipoBodega.Traslado) &&
                        inCosteo.CosteoTipo.Value != (byte)TipoCosteoInv.Transaccional)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.In_Bodega_BodegaTipoTransaccional_Restr);
                        result.Add("BodegaTipoID", msg);
                    }

                    DTO_inBodega bodExist = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, dto.ID.Value,true);
                    if (bodExist != null && bodExist.CosteoGrupoInvID.Value != dto.CosteoGrupoInvID.Value)
                    {
                        DTO_inControlSaldosCostos filterInv = new DTO_inControlSaldosCostos();
                        filterInv.BodegaID.Value = dto.ID.Value;
                        List<DTO_inControlSaldosCostos> mvtosxBodega = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(AppDocuments.TransaccionManual, filterInv);
                        if (mvtosxBodega.Count > 0)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, "La bodega ya ha tenido movimientos de saldos, no es posible cambiar el grupo de costeo");
                            result.Add("CosteoGrupoInvID", msg);
                        }
                    }
                       
                    #endregion
                }

                #endregion

                #region inBodegaTipo

                if (dtoObj.GetType() == typeof(DTO_inBodegaTipo))
                {
                    DTO_inBodegaTipo dto = (DTO_inBodegaTipo)dtoObj;

                    #region BodegaTipo

                    if (dto.BodegaTipo.Value != 0 && dto.BodegaTipo.Value != 1 && dto.BodegaTipo.Value != 2 &&
                        dto.BodegaTipo.Value != 3 && dto.BodegaTipo.Value != 4 && dto.BodegaTipo.Value != 5 &&
                        dto.BodegaTipo.Value != 6 && dto.BodegaTipo.Value != 7 && dto.BodegaTipo.Value != 8 &&
                        dto.BodegaTipo.Value != 9 && dto.BodegaTipo.Value != 10 && dto.BodegaTipo.Value != 11)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.In_BodegaTipo_BodegaTipo_Restr);
                        result.Add("BodegaTipo", msg);
                    }

                    #endregion
                }

                #endregion

                #region inCosteoGrupo

                if (dtoObj.GetType() == typeof(DTO_inCosteoGrupo))
                {
                    DTO_inCosteoGrupo dto = (DTO_inCosteoGrupo)dtoObj;

                    #region CosteoTipo

                    if (dto.CosteoTipo.Value != 0 && dto.CosteoTipo.Value != 1 && dto.CosteoTipo.Value != 2 &&
                        dto.CosteoTipo.Value != 3 && dto.CosteoTipo.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.In_CosteoGrupo_CosteoTipo_Restr);
                        result.Add("CosteoTipo", msg);
                    }

                    #endregion
                }

                #endregion

                #region inMovimientoTipo

                if (dtoObj.GetType() == typeof(DTO_inMovimientoTipo))
                {
                    DTO_inMovimientoTipo dto = (DTO_inMovimientoTipo)dtoObj;

                    #region TipoMovimiento

                    if (dto.TipoMovimiento.Value != 1 && dto.TipoMovimiento.Value != 2 && dto.TipoMovimiento.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.In_MovimientoTipo_TipoMovimiento_Restr);
                        result.Add("TipoMovimiento", msg);
                    }

                    #endregion
                }

                #endregion

                #region inPosicionArancel

                if (dtoObj.GetType() == typeof(DTO_inPosicionArancel))
                {
                    DTO_inPosicionArancel dto = (DTO_inPosicionArancel)dtoObj;

                    #region Porcentaje

                    //Valida que cumpla con el RegEx
                    if (dto.Porcentaje.Value >= 100)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.In_PosicionArancel_Porcentaje_Invalid);
                        result.Add("Porcentaje", msg);
                    }
                    else
                    {
                        //Valida los decimales del valor
                        string valor = dto.Porcentaje.Value.Value.ToString();
                        int nroDec = valor.IndexOf(',') + 1;
                        int nroDecim = valor.Substring(nroDec).Length;
                        if (nroDecim > 8)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.In_PosicionArancel_Porcentaje_InvalidDecimal);
                            result.Add("Porcentaje", msg);
                        }
                        //bool isFormat = Regex.IsMatch(valor, @"\ D + ( \, \ d { 1 , 2 })?");
                    }

                    #endregion
                }

                #endregion

                #region noConceptoPlaTra

                if (dtoObj.GetType() == typeof(DTO_noConceptoPlaTra))
                {
                    DTO_noConceptoPlaTra dto = (DTO_noConceptoPlaTra)dtoObj;

                    #region Tipo

                    if (dto.Tipo.Value != 1 && dto.Tipo.Value != 2 && dto.Tipo.Value != 3 &&
                        dto.Tipo.Value != 4 && dto.Tipo.Value != 5 && dto.Tipo.Value != 6)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ConceptoPlaTra_Tipo_Restr);
                        result.Add("Tipo", msg);
                    }

                    #endregion
                }

                #endregion

                #region noConceptoNOM

                if (dtoObj.GetType() == typeof(DTO_noConceptoNOM))
                {
                    DTO_noConceptoNOM dto = (DTO_noConceptoNOM)dtoObj;

                    #region Tipo

                    if (dto.Tipo.Value != 1 && dto.Tipo.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ConceptoNOM_Tipo_Restr);
                        result.Add("Tipo", msg);
                    }

                    #endregion

                    #region TipoLiquidacion

                    if (dto.TipoLiquidacion.Value != 1 && dto.TipoLiquidacion.Value != 2 &&
                        dto.TipoLiquidacion.Value != 3 && dto.TipoLiquidacion.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ConceptoNOM_TipoLiq_Restr);
                        result.Add("TipoLiquidacion", msg);
                    }

                    #endregion

                    #region BaseFormula

                    if (dto.BaseFormula.Value != 0 && dto.BaseFormula.Value != 1 && dto.BaseFormula.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ConceptoNOM_TipoLiq_Restr);
                        result.Add("BaseFormula", msg);
                    }

                    #endregion

                    #region TipoTercero

                    if (dto.TipoTercero.Value != 0 && dto.TipoTercero.Value != 1 && dto.TipoTercero.Value != 2 && dto.TipoTercero.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.No_ConceptoTipoNOM_TipoTercero_Restr);
                        result.Add("TipoTercero", msg);
                    }

                    #endregion
                }

                #endregion

                #region noCompFexible

                if (dtoObj.GetType() == typeof(DTO_noCompFlexible))
                {
                    DTO_noCompFlexible dto = (DTO_noCompFlexible)dtoObj;

                    #region Tipo

                    if (dto.Tipo.Value != 1 && dto.Tipo.Value != 2 && dto.Tipo.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_CompFlexible_Tipo_Restr);
                        result.Add("Tipo", msg);
                    }

                    #endregion

                    #region PeriodoPago

                    if (dto.PeriodoPago.Value != 1 && dto.PeriodoPago.Value != 2 && dto.PeriodoPago.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_CompFlexible_PeriodoPago_Restr);
                        result.Add("PeriodoPago", msg);
                    }

                    #endregion
                }

                #endregion

                #region noContratoNov

                if (dtoObj.GetType() == typeof(DTO_noContratoNov))
                {
                    DTO_noContratoNov dto = (DTO_noContratoNov)dtoObj;

                    #region Dias Iniciales y finales

                    if (dto.DiasIni1.Value < 1 || dto.DiasIni1.Value > 360)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_Dias_RangeRestr);
                        result.Add("DiasIni1", msg);
                    }
                    if (dto.DiasFin1.Value < 1 || dto.DiasFin1.Value > 360)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_Dias_RangeRestr);
                        result.Add("DiasFin1", msg);
                    }
                    if (dto.DiasIni2.Value < 1 || dto.DiasIni2.Value > 360)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_Dias_RangeRestr);
                        result.Add("DiasIni2", msg);
                    }
                    if (dto.DiasFin2.Value < 1 || dto.DiasFin2.Value > 360)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_Dias_RangeRestr);
                        result.Add("DiasFin2", msg);
                    }
                    if (dto.DiasIni3.Value < 1 || dto.DiasIni3.Value > 360)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_Dias_RangeRestr);
                        result.Add("DiasIni3", msg);
                    }
                    if (dto.DiasFin3.Value < 1 || dto.DiasFin3.Value > 360)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_Dias_RangeRestr);
                        result.Add("DiasFin3", msg);
                    }

                    #endregion

                    #region TipoNovedad

                    if (dto.TipoNovedad.Value != 0 && dto.TipoNovedad.Value != 1 && dto.TipoNovedad.Value != 2 &&
                        dto.TipoNovedad.Value != 3 && dto.TipoNovedad.Value != 4 && dto.TipoNovedad.Value != 5 &&
                        dto.TipoNovedad.Value != 6 && dto.TipoNovedad.Value != 7 && dto.TipoNovedad.Value != 8 &&
                        dto.TipoNovedad.Value != 9 && dto.TipoNovedad.Value != 10 && dto.TipoNovedad.Value != 11 &&
                        dto.TipoNovedad.Value != 12 && dto.TipoNovedad.Value != 13 && dto.TipoNovedad.Value != 14
                        && dto.TipoNovedad.Value != 15 && dto.TipoNovedad.Value != 16)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_ContratoNov_TipoNovedad_Restr);
                        result.Add("TipoNovedad", msg);
                    }

                    #endregion
                }

                #endregion

                #region noEmpleado
                if (dtoObj.GetType() == typeof(DTO_noEmpleado))
                {
                    DTO_noEmpleado dto = (DTO_noEmpleado)dtoObj;

                    #region Enmpleado Activo o Inactivo
                    if (dto.Estado.Value != 1 && dto.Estado.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_Empleado_Tipo_Restr);
                        result.Add("Estado", msg);
                    }
                    #endregion

                    #region Tercero
                    if (isInsertando)
                    {
                        if (_bc.AdministrationModel.noEmpleado_CountTerceroID(dto.TerceroID.Value) != 0)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_Empleado_TerceroID_Restr);
                            result.Add("TerceroID", msg);
                        }
                    }
                    else
                    {
                        if (_bc.AdministrationModel.noEmpleado_CountTerceroID(dto.TerceroID.Value) != 0
                            && _bc.AdministrationModel.noEmpleado_CountTerceroID(dto.TerceroID.Value) != 1
                            )
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.No_Empleado_TerceroID_Restr);
                            result.Add("TerceroID", msg);
                        }
                    }

                    #endregion
                }
                #endregion

                #region plDistribucionCampo

                if (dtoObj.GetType() == typeof(DTO_plDistribucionCampo))
                {
                    DTO_plDistribucionCampo dto = (DTO_plDistribucionCampo)dtoObj;

                    #region Porcentaje Consolidado

                    decimal porcentajeTotal = dto.Porcentaje01.Value.Value + dto.Porcentaje02.Value.Value +
                                              dto.Porcentaje03.Value.Value + dto.Porcentaje04.Value.Value +
                                              dto.Porcentaje05.Value.Value + dto.Porcentaje06.Value.Value +
                                              dto.Porcentaje07.Value.Value + dto.Porcentaje08.Value.Value +
                                              dto.Porcentaje09.Value.Value + dto.Porcentaje10.Value.Value +
                                              dto.Porcentaje11.Value.Value + dto.Porcentaje12.Value.Value;

                    if (porcentajeTotal != 100)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.pl_DistribucionCampo_PorcentajeInvalid);
                        result.Add("Porcentaje", msg);
                    }

                    #endregion
                }

                #endregion

                #region rhCompetencia

                if (dtoObj.GetType() == typeof(DTO_rhCompetencia))
                {
                    DTO_rhCompetencia dto = (DTO_rhCompetencia)dtoObj;

                    #region TipoCompetencia

                    if (dto.TipoCompetencia.Value != 1 && dto.TipoCompetencia.Value != 2 &&
                        dto.TipoCompetencia.Value != 3 && dto.TipoCompetencia.Value != 4)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Rh_Competencia_TipoCompet_Restr);
                        result.Add("TipoCompetencia", msg);
                    }

                    #endregion
                }

                #endregion

                #region rhCompetenciaxCargo

                if (dtoObj.GetType() == typeof(DTO_rhCompetenciasxCargo))
                {
                    DTO_rhCompetenciasxCargo dto = (DTO_rhCompetenciasxCargo)dtoObj;

                    #region Nivel

                    if (dto.Nivel.Value != 1 && dto.Nivel.Value != 2 && dto.Nivel.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Rh_CompetenciaxCargo_Nivel_Restr);
                        result.Add("Nivel", msg);
                    }

                    #endregion

                    #region Necesidad

                    if (dto.Necesidad.Value != 1 && dto.Necesidad.Value != 2)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Rh_CompetenciaxCargo_Necesidad_Restr);
                        result.Add("Necesidad", msg);
                    }

                    #endregion
                }

                #endregion

                #region seUsuario

                if (dtoObj.GetType() == typeof(DTO_seUsuario))
                {
                    DTO_seUsuario dto = (DTO_seUsuario)dtoObj;

                    #region CorreoElectronico

                    if (!string.IsNullOrWhiteSpace(dto.CorreoElectronico.Value))
                    {
                        bool isFormat = Regex.IsMatch(dto.CorreoElectronico.Value,
                                                      @"[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[A-Za-z.]{2,6}");
                        if (!isFormat)
                        {
                            res = false;
                            msg = _bc.GetResource(LanguageTypes.Messages,
                                                  MasterMessages.Se_Usuario_CorreoE_EmailInvalidFormat);
                            result.Add("CorreoElectronico", msg);
                        }
                    }

                    #endregion
                }

                #endregion

                #region tsBancosCuenta

                if (dtoObj.GetType() == typeof(DTO_tsBancosCuenta))
                {
                    DTO_tsBancosCuenta dto = (DTO_tsBancosCuenta)dtoObj;

                    #region CuentaTipo

                    if (dto.CuentaTipo.Value != 1 && dto.CuentaTipo.Value != 2 && dto.CuentaTipo.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Ts_BancosCuenta_CuentaTipo_Restr);
                        result.Add("CuentaTipo", msg);
                    }

                    #endregion

                    #region coDocumentoID

                    DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, dto.coDocumentoID.Value, true);

                    // Verifica que solo hayan Documentos contables con Documento 31
                    if (coDoc.DocumentoID.Value != "31")
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Ts_BancosCuenta_DocContable_Checked);
                        result.Add("DocContable", msg);
                    }

                    if (coDoc.MonedaOrigen.Value != (byte)TipoMoneda_CoDocumento.Local &&
                        coDoc.MonedaOrigen.Value != (byte)TipoMoneda_CoDocumento.Foreign)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Ts_BancosCuenta_DocContable_MdaOrigenInvalid);
                        result.Add("coDocumentoID", msg);
                    }

                    #endregion
                }

                #endregion

                #region tsCaja

                if (dtoObj.GetType() == typeof(DTO_tsCaja))
                {
                    DTO_tsCaja dto = (DTO_tsCaja)dtoObj;

                    #region coDocumentoID

                    DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, dto.coDocumentoID.Value, true);

                    if (coDoc.DocumentoID.Value != AppDocuments.ReciboCaja.ToString() && coDoc.DocumentoID.Value != AppDocuments.RecaudosManuales.ToString() &&
                        coDoc.DocumentoID.Value != AppDocuments.RecaudosMasivos.ToString() && coDoc.DocumentoID.Value != AppDocuments.PagosTotales.ToString() &&
                        coDoc.DocumentoID.Value != AppDocuments.RecaudoReoperacion.ToString())
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages,
                                              MasterMessages.Ts_Caja_DocumentoID_Restr);
                        result.Add("coDocumentoID", msg);
                    }
                    #endregion

                }


                #endregion

                #region tsFlujoFondo

                if (dtoObj.GetType() == typeof(DTO_tsFlujoFondo))
                {
                    DTO_tsFlujoFondo dto = (DTO_tsFlujoFondo)dtoObj;

                    #region TipoFlujo

                    if (dto.TipoFlujo.Value != 1 && dto.TipoFlujo.Value != 2 && dto.TipoFlujo.Value != 3)
                    {
                        res = false;
                        msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Ts_FlujoFondo_TipoFlujo_Restr);
                        result.Add("TipoFlujo", msg);
                    }

                    #endregion
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion
    }
}
