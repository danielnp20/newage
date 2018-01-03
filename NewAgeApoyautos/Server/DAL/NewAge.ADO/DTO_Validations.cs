using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SentenceTransformer;

namespace NewAge.ADO
{
    public static class DTO_Validations
    {
        private static DAL_glControl ctrlDAL = null;

        /// <summary>
        /// Valida las reglas de negocio de un DTO
        /// </summary>
        /// <param name="dtoObj">Objeto que se envia</param>
        /// <param name="res">Respuesta si esta bien el DTO</param>
        /// <param name="msg">Mensaje de error (vacio si esta todo ok)</param>
        /// <returns>Retorna un diccionario con la lista de campos y sus errores</returns>
        public static List<DTO_TxResultDetailFields> CheckRules(string loggerConn, SqlConnection c, SqlTransaction tx, Object dtoObj, DTO_glEmpresa empresa, string empresaGrupo, int userID, bool insertando)
        {
            List<DTO_TxResultDetailFields> result = new List<DTO_TxResultDetailFields>();
            ctrlDAL = new DAL_glControl(c, tx, empresa, userID, loggerConn);

            try
            {
                #region acClase
                if (dtoObj.GetType() == typeof(DTO_acClase))
                {
                    DTO_acClase dto = (DTO_acClase)dtoObj;
                    #region TipoAct
                    //if (dto.TipoAct.Value != 0 && dto.TipoAct.Value != 1 && dto.TipoAct.Value != 2)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "TipoAct";
                    //    rdF.Message = MasterMessages.Ac_Grupo_TipoAct_Restr;
                    //    result.Add(rdF);
                    //}
                    #endregion
                    #region TipoDepreLOC
                    if (dto.TipoDepreLOC.Value != 0 && dto.TipoDepreLOC.Value != 1 && dto.TipoDepreLOC.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoDepreLOC";
                        rdF.Message = MasterMessages.Ac_Grupo_TipoDepreLOC_Restr;
                        result.Add(rdF);
                    }
                    #endregion

                }
                #endregion
                #region acComponenteActivo

                if (dtoObj.GetType() == typeof(DTO_acComponenteActivo))
                {
                    DTO_acComponenteActivo dto = (DTO_acComponenteActivo)dtoObj;
                    DAL_MasterSimple conc = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    conc.DocumentID = AppMasters.acComponenteActivo;
                    UDT_BasicID udtConsepto = new UDT_BasicID() { Value = dto.ConceptoSaldoID.Value };
                    DTO_glConceptoSaldo componente = (DTO_glConceptoSaldo)conc.DAL_MasterSimple_GetByID(udtConsepto, true);
                    //DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)msCuenta.DAL_MasterSimple_GetByID(udt, true);
                    if (componente != null)
                    {
                        if (componente.coSaldoControl.Value != 5)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "TipoCta";
                            rdF.Message = MasterMessages.Ac_Componente_CoSaldoCtrl_Restr;
                            result.Add(rdF);
                        }
                    }
                    
                }

                #endregion
                #region ccCliente

                if (dtoObj.GetType() == typeof(DTO_ccCliente))
                {
                    DTO_ccCliente dto = (DTO_ccCliente)dtoObj;

                    //Solicita llenar CuentaTipo_1 y BcoCtaNro_1 cuando hay Banco asignado
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && (string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString())
                        || ((dto.CuentaTipo_1.Value != 3) && string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString()))))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BancoID_1";
                        rdF.Message = MasterMessages.Co_Tercero_CtaTipo_AccountRequired + "&&" + dto.BancoID_1.Value;
                        result.Add(rdF);
                    }
                    else if (string.IsNullOrWhiteSpace(dto.BancoID_1.Value) &&
                            (!string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString()) || !string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString())))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BancoID_1";
                        rdF.Message = MasterMessages.Co_Tercero_BancoID1_BanKRequired + "&&" + dto.CuentaTipo_1.Value + "&&" + dto.BcoCtaNro_1.Value;
                        result.Add(rdF);
                    }

                    string sectorCartera = GetControlValueByCompany(empresa, ModulesPrefix.cc, AppControl.cc_SectorCartera);
                    if (sectorCartera == ((byte)SectorCartera.Financiero).ToString())
                    {
                        string zona = GetControlValueByCompany(empresa, ModulesPrefix.cc, AppControl.cc_Zona);
                        string profesion = GetControlValueByCompany(empresa, ModulesPrefix.cc, AppControl.cc_ProfesionPorDefecto);
                        string asesor = GetControlValueByCompany(empresa, ModulesPrefix.cc, AppControl.cc_AsesorXDefecto);

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
                    }                
                    else
                    {                    
                        #region Valida que el cliente sea mayor de edad

                        int edadMinima = 18;
                        int fechaNacimiento = 0;

                        TimeSpan difFechaNacimiento = dto.FechaExpDoc.Value.Value.Subtract(dto.FechaNacimiento.Value.Value);
                        fechaNacimiento = (int)Math.Floor((double)difFechaNacimiento.Days / 365);

                        if (fechaNacimiento < edadMinima)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "FechaExpDoc";
                            rdF.Message = MasterMessages.Cc_Cliente_ServicioTipo_ValidDate;
                            result.Add(rdF);
                        }
                        #endregion
                    }

                    #region Valida El tipo de Estado Civil
                    if (dto.EstadoCivil.Value == 0)
                        if (string.IsNullOrEmpty(dto.EstadoOtro.Value))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "EstadoCivil";
                            rdF.Message = MasterMessages.Cc_Cliente_ServicioTipo_EstadoCivil;
                            result.Add(rdF);
                        }
                    #endregion
                    #region Valida si es mujer cuando se Chequea Cabeza de Famila
                    if (dto.MujerInd.Value == true)
                        if (dto.Sexo.Value == 1)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "Sexo";
                            rdF.Message = MasterMessages.Cc_Cliente_ServicioTipo_HeadFamily;
                            result.Add(rdF);
                        }
                    #endregion
                }
                
                #endregion
                #region ccComponentesCuenta

                if (dtoObj.GetType() == typeof(DTO_ccComponenteCuenta))
                {
                    DTO_ccComponenteCuenta dto = (DTO_ccComponenteCuenta)dtoObj;

                    if (!string.IsNullOrEmpty(dto.CuentaID.Value))
                    {
                        DAL_MasterHierarchy Cuenta = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                        Cuenta.DocumentID = AppMasters.coPlanCuenta;
                        UDT_BasicID udtCuenta = new UDT_BasicID() { Value = dto.CuentaID.Value };
                        DTO_coPlanCuenta cuenta = (DTO_coPlanCuenta)Cuenta.DAL_MasterSimple_GetByID(udtCuenta, false);

                        DAL_MasterSimple consepto = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                        consepto.DocumentID = AppMasters.ccCarteraComponente;
                        UDT_BasicID udtConsepto = new UDT_BasicID() { Value = dto.ComponenteCarteraID.Value };
                        DTO_ccCarteraComponente componente = (DTO_ccCarteraComponente)consepto.DAL_MasterSimple_GetByID(udtConsepto, false);

                        if (cuenta.ConceptoSaldoID.Value != componente.ConceptoSaldoID.Value)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "conseptoSaldo";
                            rdF.Message = MasterMessages.Cc_ComponenteCartera_ServicioTipo_ValidDate;
                            result.Add(rdF);

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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ServicioTipo";
                        rdF.Message = MasterMessages.Co_ActEconomica_ServicioTipo_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "AgrupaTercero";
                        rdF.Message = MasterMessages.Co_BalanceReclasifica_AgrupaTercero_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoConsec";
                        rdF.Message = MasterMessages.Co_Comprobante_TipoConsec_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region BimonedaInd
                    string indAjusDefault = GetControlValueByCompany(empresa, ModulesPrefix.co, AppControl.co_IndMultimoneda);
                    if (indAjusDefault.Equals("0"))
                    {
                        if (!string.IsNullOrWhiteSpace(dto.biMonedaInd.Value.ToString()) && dto.biMonedaInd.Value.Value)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "biMonedaInd";
                            rdF.Message = MasterMessages.Co_Comprobante_BimonedaNotActive;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                    #region ComprobanteAnul
                    if (dto != null &&  !string.IsNullOrEmpty(dto.ComprobanteAnulID.Value))
                    {
                        DAL_MasterSimple ms = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                        ms.DocumentID = AppMasters.coComprobante;
                        UDT_BasicID udt = new UDT_BasicID() { Value = dto.ComprobanteAnulID.Value };
                        DTO_coComprobante comprAnul = (DTO_coComprobante)ms.DAL_MasterSimple_GetByID(udt, true);
                        if (comprAnul != null && dto.BalanceTipoID.Value != comprAnul.BalanceTipoID.Value)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "BalanceTipoID";
                            rdF.Message = MasterMessages.Co_Comprobante_LibroCompAnulInvalid;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                }
                #endregion
                #region coConceptocargo
                if (dtoObj.GetType() == typeof(DTO_coConceptoCargo))
                {
                    DTO_coConceptoCargo dto = (DTO_coConceptoCargo)dtoObj;
                    #region TipoConcepto
                    if (dto.TipoConcepto.Value != 1 && dto.TipoConcepto.Value != 2 && dto.TipoConcepto.Value != 3 &&
                        dto.TipoConcepto.Value != 4 && dto.TipoConcepto.Value != 5 && dto.TipoConcepto.Value != 6 &&
                        dto.TipoConcepto.Value != 7 && dto.TipoConcepto.Value != 8 && dto.TipoConcepto.Value != 9)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoConcepto";
                        rdF.Message = MasterMessages.Co_ConceptoCargo_TipoConcepto_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Bien Servicio
                    if ((dto.TipoConcepto.Value == 3 || dto.TipoConcepto.Value == 4
                       || dto.TipoConcepto.Value == 5 || dto.TipoConcepto.Value == 6 || dto.TipoConcepto.Value == 7
                       || dto.TipoConcepto.Value == 8) && dto.BienServicio.Value != 1)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BienServicio";
                        rdF.Message = MasterMessages.Co_ConceptoCargo_BienServicio_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ConsolidaPor";
                        rdF.Message = MasterMessages.Co_CtaGrupo_ConsolidaPor_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Agrupa (Tipo de Cuenta)
                    if (dto.TipoCuenta.Value != 1 && dto.TipoCuenta.Value != 2 && dto.TipoCuenta.Value != 3 &&
                        dto.TipoCuenta.Value != 4 && dto.TipoCuenta.Value != 5 && dto.TipoCuenta.Value != 6)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoCuenta";
                        rdF.Message = MasterMessages.Co_CtaGrupo_TipoCuenta_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region coDocumento
                if (dtoObj.GetType() == typeof(DTO_coDocumento))
                {
                    DTO_coDocumento dto = (DTO_coDocumento)dtoObj;
                    DAL_MasterHierarchy msCuenta = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                    DAL_MasterSimple msDoc = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    DAL_Comprobante dalcom = new DAL_Comprobante(c, tx, empresa, userID, loggerConn);
                    msDoc.DocumentID = AppMasters.glDocumento;

                    UDT_BasicID udt = new UDT_BasicID() { Value = dto.DocumentoID.Value };
                    DTO_glDocumento Doc = (DTO_glDocumento)msDoc.DAL_MasterSimple_GetByID(udt, true);
                    #region CuentaLOC
                    //Solicita la cuenta local o extranjera o ambas segun la moneda origen
                    if (dto != null)
                    {
                        if ((dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Local || dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Both) &&
                             string.IsNullOrEmpty(dto.CuentaLOC.Value))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CuentaLOC";
                            rdF.Message = MasterMessages.Co_coDocumento_CuentaLOC_Empty;
                            result.Add(rdF);
                        }
                        else if (!string.IsNullOrEmpty(dto.CuentaLOC.Value))
                        {
                            //Permite ingresar solo cuentas con Origen Monetario Local y el mismo modulo si es Doc Interno o externo
                            msCuenta.DocumentID = AppMasters.coPlanCuenta;
                            udt = new UDT_BasicID() { Value = dto.CuentaLOC.Value };
                            DTO_coPlanCuenta cuentaLoc = (DTO_coPlanCuenta)msCuenta.DAL_MasterSimple_GetByID(udt, true);

                            msDoc.DocumentID = AppMasters.glConceptoSaldo;
                            udt = new UDT_BasicID() { Value = cuentaLoc.ConceptoSaldoID.Value };
                            DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)msDoc.DAL_MasterSimple_GetByID(udt, true);
                            if (cuentaLoc != null && saldoLoc != null)
                            {
                                if (cuentaLoc.OrigenMonetario.Value != 1)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "CuentaLOC";
                                    rdF.Message = MasterMessages.Co_coDocumento_cuentaLOC_OrigenMonet;
                                    result.Add(rdF);
                                }                              
                            }
                        }
                        // Verifica que si hay cuenta LOC o EXT y el docmento es 21 0 26 el control de saldo en el concepto de saldo de esas cuenta debe ser 3
                        if (!string.IsNullOrEmpty(dto.CuentaLOC.Value) && (dto.DocumentoID.Value == "21" || dto.DocumentoID.Value == "26"))
                        {
                            msDoc.DocumentID = AppMasters.glConceptoSaldo;
                            udt = new UDT_BasicID() { Value = dto.CuentaLOC.Value };
                            DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)msDoc.DAL_MasterSimple_GetByID(udt, true);
                            if (saldoLoc != null)
                            {
                                if (saldoLoc.coSaldoControl.Value != 3)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "DocumentoID";
                                    rdF.Message = MasterMessages.Co_coDocumento_DocumentoID_DocumentInvalid_LocYExt3;
                                    result.Add(rdF);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(dto.CuentaEXT.Value) && (dto.DocumentoID.Value == "21" || dto.DocumentoID.Value == "26"))
                        {
                            msDoc.DocumentID = AppMasters.glConceptoSaldo;
                            udt = new UDT_BasicID() { Value = dto.CuentaEXT.Value };
                            DTO_glConceptoSaldo saldoExt = (DTO_glConceptoSaldo)msDoc.DAL_MasterSimple_GetByID(udt, true);
                            if (saldoExt != null)
                            {
                                if (saldoExt.coSaldoControl.Value != 3)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "DocumentoID";
                                    rdF.Message = MasterMessages.Co_coDocumento_DocumentoID_DocumentInvalid_LocYExt3;
                                    result.Add(rdF);
                                }
                            }
                        }
                        // Verifica que si hay cuenta LOC o EXT y el docmento es 41 0 42 el control de saldo en el concepto de saldo de esas cuenta debe ser 2
                        if (!string.IsNullOrEmpty(dto.CuentaLOC.Value) && (dto.DocumentoID.Value == "41" || dto.DocumentoID.Value == "42"))
                        {
                            msDoc.DocumentID = AppMasters.glConceptoSaldo;
                            udt = new UDT_BasicID() { Value = dto.CuentaLOC.Value };
                            DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)msDoc.DAL_MasterSimple_GetByID(udt, true);
                            if (saldoLoc != null)
                            {
                                if (saldoLoc.coSaldoControl.Value != 2)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "DocumentoID";
                                    rdF.Message = MasterMessages.Co_coDocumento_DocumentoID_DocumentInvalid_LocYExt2;
                                    result.Add(rdF);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(dto.CuentaEXT.Value) && (dto.DocumentoID.Value == "41" || dto.DocumentoID.Value == "42"))
                        {
                            msDoc.DocumentID = AppMasters.glConceptoSaldo;
                            udt = new UDT_BasicID() { Value = dto.CuentaEXT.Value };
                            DTO_glConceptoSaldo saldoExt = (DTO_glConceptoSaldo)msDoc.DAL_MasterSimple_GetByID(udt, true);
                            if (saldoExt != null)
                            {
                                if (saldoExt.coSaldoControl.Value != 2)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "DocumentoID";
                                    rdF.Message = MasterMessages.Co_coDocumento_DocumentoID_DocumentInvalid_LocYExt2;
                                    result.Add(rdF);
                                }
                            }
                        }
                    }
                    #endregion
                    #region CuentaEXT
                    if (dto != null)
                    {
                        //Solicita la cuenta local o extranjera o ambas segun la moneda origen
                        if ((dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Foreign || dto.MonedaOrigen.Value == (byte)TipoMoneda_CoDocumento.Both) &&
                             string.IsNullOrEmpty(dto.CuentaEXT.Value))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CuentaEXT";
                            rdF.Message = MasterMessages.Co_coDocumento_CuentaEXT_Empty;
                            result.Add(rdF);
                        }
                        else if (!string.IsNullOrEmpty(dto.CuentaEXT.Value))
                        {
                            //Permite ingresar solo cuentas con Origen Monetario Local y el mismo modulo si es Doc Interno o externo
                            msCuenta.DocumentID = AppMasters.coPlanCuenta;
                            udt = new UDT_BasicID() { Value = dto.CuentaEXT.Value };
                            DTO_coPlanCuenta cuentaExtr = (DTO_coPlanCuenta)msCuenta.DAL_MasterSimple_GetByID(udt, true);

                            msDoc.DocumentID = AppMasters.glConceptoSaldo;
                            udt = new UDT_BasicID() { Value = cuentaExtr.ConceptoSaldoID.Value };
                            DTO_glConceptoSaldo saldoExt = (DTO_glConceptoSaldo)msCuenta.DAL_MasterSimple_GetByID(udt, true);

                            if (cuentaExtr != null && saldoExt != null)
                            {
                                if (cuentaExtr.OrigenMonetario.Value != 2)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "CuentaEXT";
                                    rdF.Message = MasterMessages.Co_coDocumento_cuentaEXT_OrigenMonet;
                                    result.Add(rdF);
                                }
                                else if (saldoExt.ModuloID.Value != Doc.ModuloID.Value &&
                                        (saldoExt.coSaldoControl.Value.Value == (byte)SaldoControl.Doc_Interno || saldoExt.coSaldoControl.Value.Value == (byte)SaldoControl.Doc_Externo))
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "CuentaEXT";
                                    rdF.Message = MasterMessages.Co_coDocumento_CuentaEXT_ModuleInvalid;
                                    result.Add(rdF);
                                }
                            }
                        }
                    }

                    #endregion
                    #region TipoComprobante
                    if (dto.TipoComprobante.Value != 1 && dto.TipoComprobante.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoComprobante";
                        rdF.Message = MasterMessages.Co_coDocumento_TipoCompr_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Comprobante
                    if (!insertando)
                    {
                        if (dto.ComprobanteID.Value != null && dto.ComprobanteDesc.Value != null)
                        {

                            int count = dalcom.DAL_ComprobanteExistsInAuxPre(dto.ComprobanteID.Value);
                            if (count > 0)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "ComprobanteID";
                                rdF.Message = MasterMessages.Co_coDocumento_Comprobante_Restr;
                                result.Add(rdF);
                            }
                            //_bc.Get();
                        }
                    }
                    #endregion
                    #region MonedaOrigen
                    if (dto.MonedaOrigen.Value != 1 && dto.MonedaOrigen.Value != 2 && dto.MonedaOrigen.Value != 3 && dto.MonedaOrigen.Value != 4)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "MonedaOrigen";
                        rdF.Message = MasterMessages.Co_coDocumento_MonedaOrigen_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Documento
                    if (dto != null)
                    {
                        msDoc.DocumentID = AppMasters.glDocumento;
                        udt = new UDT_BasicID() { Value = dto.DocumentoID.Value };
                        DTO_glDocumento doc = (DTO_glDocumento)msDoc.DAL_MasterSimple_GetByID(udt, true);
                        if (!string.IsNullOrEmpty(dto.ComprobanteID.Value) && doc != null && (!doc.NOLibroFuncionalInd.Value.HasValue || !doc.NOLibroFuncionalInd.Value.Value))
                        {
                            msDoc.DocumentID = AppMasters.coComprobante;
                            udt = new UDT_BasicID() { Value = dto.ComprobanteID.Value };
                            DTO_coComprobante compr = (DTO_coComprobante)msDoc.DAL_MasterSimple_GetByID(udt, true);
                            string libroFunc = GetControlValueByCompany(empresa,ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                            if (!compr.BalanceTipoID.Value.Equals(libroFunc))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "DocumentoID";
                                rdF.Message = MasterMessages.Co_coDocumento_BalanceInvalid;
                                result.Add(rdF);
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
                    string numCtrl = GetControlValueByCompany(empresa, ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
                    if (!string.IsNullOrWhiteSpace(dto.RegimenFiscalEmpresaID.Value) && dto.RegimenFiscalEmpresaID.Value != numCtrl)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "RegimenFiscalEmpresaID";
                        rdF.Message = MasterMessages.Co_Impuesto_RegFisEmp_NotCompatible + "&&" + numCtrl;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Tipo de impuesto

                    //Valida el tipo de impuesto
                    string eg = dto.EmpresaGrupoID.Value;

                    DAL_MasterSimple msImpTipo = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msImpTipo.DocumentID = AppMasters.coImpuestoTipo;

                    UDT_BasicID udtImpTipo = new UDT_BasicID() { Value = dto.ImpuestoTipoID.Value };
                    DTO_coImpuestoTipo tipo = (DTO_coImpuestoTipo)msImpTipo.DAL_MasterSimple_GetByID(udtImpTipo, true);

                    //Verifica si el impuesto es nacional: tipo de impuesto (impuesto alcance) 
                    bool isNacional = tipo != null && tipo.ImpuestoAlcance.Value == 1 ? true : false;
                    if (isNacional)
                    {
                        int lgDoc = AppMasters.glLugarGeografico;
                        DAL_MasterHierarchy msLugGeo = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                        msLugGeo.DocumentID = lgDoc;

                        List<DTO_MasterBasic> lgs = msLugGeo.DAL_MasterHierarchy_GetChindrenPaged(1, 1, OrderDirection.ASC, new UDT_BasicID(),
                            string.Empty, string.Empty, true).ToList();

                        if (lgs.Count > 0)
                        {

                            UDT_BasicID idParent = new UDT_BasicID() { Value = lgs.First().ID.Value };
                            lgs = msLugGeo.DAL_MasterHierarchy_GetChindrenPaged(1, 1, OrderDirection.ASC, idParent, string.Empty, string.Empty, true).ToList();
                            if (lgs.Count > 0)
                            {
                                DTO_MasterBasic lg = lgs.First();
                                // variable que contiene el lugar por defecto
                                string lugarDef=GetControlValueByCompany(empresa, ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                                if (dto.LugarGeograficoID.Value != lugarDef)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "LugarGeograficoID";
                                    rdF.Message = MasterMessages.Co_ImpuestoTipo_LugarGeo_ImpNal + "&&" + lg.ID.Value + "&&" + lg.Descriptivo.Value;
                                    result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CausacionPago";
                        rdF.Message = MasterMessages.Co_ImpTipo_CausaPago_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region ImpuestoAlcance
                    if (dto.ImpuestoAlcance.Value != 1 && dto.ImpuestoAlcance.Value != 2 && dto.ImpuestoAlcance.Value != 3)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ImpuestoAlcance";
                        rdF.Message = MasterMessages.Co_ImpTipo_ImpAlcance_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region coImpuestoDeclaracion
                if (dtoObj.GetType() == typeof(DTO_coImpuestoDeclaracion))
                {
                    DTO_coImpuestoDeclaracion dto = (DTO_coImpuestoDeclaracion)dtoObj;
                    #region PeriodoDeclaracion
                    if (dto.PeriodoDeclaracion.Value != 1 && dto.PeriodoDeclaracion.Value != 2 && dto.PeriodoDeclaracion.Value != 4 &&
                        dto.PeriodoDeclaracion.Value != 6 && dto.PeriodoDeclaracion.Value != 12)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "PeriodoDeclaracion";
                        rdF.Message = MasterMessages.Co_ImpuestoDec_PeriodoDec_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region MunicipalInd
                    if (!dto.MunicipalInd.Value.Value)
                    {
                        string LugarGeoxDefec = GetControlValueByCompany(empresa, ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                        if (LugarGeoxDefec != dto.LugarGeograficoID.Value)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "LugarGeograficoID";
                            rdF.Message = MasterMessages.Co_ImpuestoDec_LugarGeoInvalid;
                            result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Tipo";
                        rdF.Message = MasterMessages.Co_ImpuestoFormato_Tipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region TipoFiscal
                    //if (dto.TipoFiscal.Value != 1 && dto.TipoFiscal.Value != 2 && dto.TipoFiscal.Value != 3 &&
                    //    dto.TipoFiscal.Value != 4 && dto.TipoFiscal.Value != 5)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "TipoFiscal";
                    //    rdF.Message = MasterMessages.Co_ImpuestoFormato_TipoFiscal_Restr;
                    //    result.Add(rdF);
                    //}
                    #endregion
                }
                #endregion
                #region coImpDeclaracionCuenta
                if (dtoObj.GetType() == typeof(DTO_coImpDeclaracionCuenta))
                {
                    DTO_coImpDeclaracionCuenta dto = (DTO_coImpDeclaracionCuenta)dtoObj;
                    //Verifica que la cuenta declare impuestos
                    UDT_BasicID udtCuenta = new UDT_BasicID() { Value = dto.CuentaID.Value };
                    string eg = dto.EmpresaGrupoID.Value;

                    DAL_MasterHierarchy msTCuenta = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                    msTCuenta.DocumentID = AppMasters.coPlanCuenta;

                    DTO_coPlanCuenta cuentaDto = (DTO_coPlanCuenta)msTCuenta.DAL_MasterSimple_GetByID(udtCuenta, true);
                    if (cuentaDto != null)
                    {
                        if (string.IsNullOrEmpty(cuentaDto.ImpuestoTipoID.Value))
                        {
                            //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            //    rdF.Field = "CuentaID";
                            //    rdF.Message = MasterMessages.Co_ImpDeclCuenta_CuentaID_Invalid;
                            //    result.Add(rdF);
                        }
                    }
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "AñoFiscal";
                        rdF.Message = MasterMessages.Co_ImpDeclCalendario_AñoFiscal_Invalid;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "SignoSuma";
                        rdF.Message = MasterMessages.Co_ImpDeclaracionRenglon_SignoSuma_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region coOperacion
                if (dtoObj.GetType() == typeof(DTO_coOperacion))
                {

                    DTO_coOperacion dto = (DTO_coOperacion)dtoObj;
                    #region TipoOperacion
                    if (dto.TipoOperacion.Value != 1 && dto.TipoOperacion.Value != 2 && dto.TipoOperacion.Value != 3
                        && dto.TipoOperacion.Value != 4)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoOperacion";
                        rdF.Message = MasterMessages.Co_Operacion_TipoOpe_RestrO;
                        result.Add(rdF);
                    }
                    #endregion
                    //#region TipoFiscal
                    //if (dto.TipoFiscal.Value != 1 && dto.TipoFiscal.Value != 2 && dto.TipoFiscal.Value != 3 && dto.TipoFiscal.Value != 4)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "TipoFiscal";
                    //    rdF.Message = MasterMessages.Co_Operacion_TipoFiscal_Restr;
                    //    result.Add(rdF);
                    //}
                    //#endregion
                }
                #endregion
                #region coPlanCuenta
                if (dtoObj.GetType() == typeof(DTO_coPlanCuenta))
                {
                    DTO_coPlanCuenta dto = (DTO_coPlanCuenta)dtoObj;
                    #region ImpuestoTipoID
                    //Solicita llenar ImpuestoPorc y MontoMinimo  cuando hay Tipo de impuesto
                    if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) &&
                       (string.IsNullOrWhiteSpace(dto.ImpuestoPorc.Value.ToString()) || string.IsNullOrWhiteSpace(dto.MontoMinimo.Value.ToString())))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ImpuestoTipoID";
                        rdF.Message = MasterMessages.Co_PlanCta_ImpPorc_NotEmpty;
                        result.Add(rdF);
                    }
                    else if (string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) &&
                            (!string.IsNullOrWhiteSpace(dto.ImpuestoPorc.Value.ToString()) || !string.IsNullOrWhiteSpace(dto.MontoMinimo.Value.ToString())))
                    {

                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ImpuestoTipoID";
                        rdF.Message = MasterMessages.Co_PlanCta_ImpTipo_NotExist;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Tipo
                    //Valida que el Tipo ingresado corresponda al mismo Tipo de la jerarquia 
                    DAL_glTabla table = new DAL_glTabla(c, tx, empresa, userID, loggerConn);
                    DTO_glTabla getTable = table.DAL_glTabla_GetByTablaNombre("coPlanCuenta", empresaGrupo);

                    int nivel = getTable.LengthToLevel(dto.ID.Value.Trim().Length);
                    if (nivel != 1)
                    {
                        int nivelPadre = nivel - 1;
                        int lonPadre = getTable.CodeLength(nivelPadre);
                        string idPadre = dto.ID.Value.Trim().Substring(0, lonPadre);

                        UDT_BasicID idPadreUdt = new UDT_BasicID();
                        idPadreUdt.Value = idPadre;

                        string eg = dto.EmpresaGrupoID.Value;

                        DAL_MasterHierarchy msPlanCta = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                        msPlanCta.DocumentID = AppMasters.coPlanCuenta;

                        DTO_coPlanCuenta padre = (DTO_coPlanCuenta)msPlanCta.DAL_MasterSimple_GetByID(idPadreUdt, true);

                        if (padre != null)
                        {
                            if (!string.IsNullOrWhiteSpace(padre.Tipo.Value) && !padre.Tipo.Value.Equals(dto.Tipo.Value.ToString()))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "Tipo";
                                rdF.Message = MasterMessages.Co_PlanCta_Tipo_HierarCheck;
                                result.Add(rdF);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(dto.Tipo.Value) && dto.Tipo.Value != "0" && dto.Tipo.Value != "1" && dto.Tipo.Value != "2" &&
                        dto.Tipo.Value != "3" && dto.Tipo.Value != "4" && dto.Tipo.Value != "5")
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Tipo";
                        rdF.Message = MasterMessages.Co_PlanCta_Tipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Naturaleza
                    if (dto.Naturaleza.Value != 1 && dto.Naturaleza.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Naturaleza";
                        rdF.Message = MasterMessages.Co_PlanCta_Natural_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region ConceptoSaldoID
                    //Cuando ConceptoSaldoID es igual a Doc Int o Doc Ext activa los indicadores de Tercero y DocControl
                    DAL_MasterSimple msSaldo = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msSaldo.DocumentID = AppMasters.glConceptoSaldo;

                    UDT_BasicID udtSaldo = new UDT_BasicID() { Value = dto.ConceptoSaldoID.Value };
                    DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)msSaldo.DAL_MasterSimple_GetByID(udtSaldo, true);

                    //  TerceroSaldosInd esta habilitado, el coSaldoControl debe ser 4
                    if (saldo.coSaldoControl.Value == 4 )
                    {
                        if (dto.TerceroSaldosInd.Value == true)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "ConceptoSaldoID";
                            rdF.Message = MasterMessages.Co_PlanCta_ConceptoSaldo_SdoCtrl_CheckValue;
                            result.Add(rdF);
                        }
                    }

                    if (saldo != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dto.ConceptoSaldoID.Value.ToString()) &&
                            (saldo.coSaldoControl.Value == (byte)SaldoControl.Doc_Externo || saldo.coSaldoControl.Value == (byte)SaldoControl.Doc_Interno) &&
                           (dto.TerceroSaldosInd.Value == false || dto.TerceroInd.Value == false))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "ConceptoSaldoID";
                            rdF.Message = MasterMessages.Co_PlanCta_ConceptoSaldo_CheckValue;
                            result.Add(rdF);
                        }
                    }
                    //Verifica si el Concepto saldo ha sido cambiado
                    DAL_Contabilidad contDAL = new DAL_Contabilidad(c, tx, empresa, userID, loggerConn);
                    DAL_MasterHierarchy cuentaHier = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                    cuentaHier.DocumentID = AppMasters.coPlanCuenta;
                    UDT_BasicID udtCuenta = new UDT_BasicID() { Value = dto.ID.Value };
                    bool active = !dto.ActivoInd.Value.Value;
                    DTO_coPlanCuenta cuentaOld = (DTO_coPlanCuenta)cuentaHier.DAL_MasterSimple_GetByID(udtCuenta, active);
                    if (cuentaOld != null)
                        if (dto.ConceptoSaldoID.Value != cuentaOld.ConceptoSaldoID.Value)
                        {
                            ////Revisa si el concepto saldo cambiado tiene saldos pendientes
                            //if (contDAL.DAL_Contabilidad_SaldoExists(cuentaOld.ConceptoSaldoID.Value, dto.ID.Value))
                            //{
                            //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            //    rdF.Field = "ConceptoSaldoID";
                            //    rdF.Message = MasterMessages.Gl_ConceptoSaldo_ID_SaldoExist;
                            //    result.Add(rdF);
                            //}
                            //else
                            //{
                            //    //Sino tiene saldos trae los coDocumentos filtrados con la cuenta actual
                            //    List<DTO_glConsultaFiltro> filtrosCoDocumento = new List<DTO_glConsultaFiltro>();
                            //    filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
                            //    {
                            //        CampoFisico = "CuentaLOC",
                            //        OperadorFiltro = OperadoresFiltro.Igual,
                            //        ValorFiltro = dto.ID.Value,
                            //        OperadorSentencia = "OR"
                            //    });
                            //    filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
                            //    {
                            //        CampoFisico = "CuentaEXT",
                            //        OperadorFiltro = OperadoresFiltro.Igual,
                            //        ValorFiltro = dto.ID.Value,
                            //        OperadorSentencia = "OR"
                            //    });
                            //    filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
                            //    {
                            //        CampoFisico = "CuentaPresupuesto",
                            //        OperadorFiltro = OperadoresFiltro.Igual,
                            //        ValorFiltro = dto.ID.Value
                            //    });
                            //    msSaldo.DocumentID = AppMasters.coDocumento;

                            //    long count = msSaldo.DAL_MasterSimple_Count(null, null, true);
                            //    var coDocumento = msSaldo.DAL_MasterSimple_GetPaged(Convert.ToInt32(count), 1, null, filtrosCoDocumento, true).ToList();

                            //    foreach (var item in coDocumento)
                            //    {   //Verifica que el módulo de la cuenta corresponda al módulo del coDocumento
                            //        DTO_coDocumento coDoc = (DTO_coDocumento)item;
                            //        msSaldo.DocumentID = AppMasters.glDocumento;
                            //        udtSaldo = new UDT_BasicID() { Value = coDoc.DocumentoID.Value };

                            //        DTO_glDocumento documento = (DTO_glDocumento)msSaldo.DAL_MasterSimple_GetByID(udtSaldo, active);
                            //        if (saldo.ModuloID.Value != documento.ModuloID.Value)
                            //        {
                            //            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            //            rdF.Field = "ConceptoSaldoID";
                            //            rdF.Message = MasterMessages.Co_PlanCta_ConceptoSaldo_ModuleInvalid + "&&" + coDoc.ID.Value;
                            //            result.Add(rdF);
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                    #endregion
                    #region OrigenMonetario
                    if (dto.OrigenMonetario.Value != 1 && dto.OrigenMonetario.Value != 2 && dto.OrigenMonetario.Value != 3)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "OrigenMonetario";
                        rdF.Message = MasterMessages.Co_PlanCta_OrigenMon_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Campos de Ajustes(Aj)
                    //AjCambioDocumentoInd / AjCambioTerceroInd / AjCambioProyectoInd / AjCambioCentroCostoInd / AjCambioLinNegInd / AjCambioLinPresupuestal
                    //Valida que el indicador de Ajustes sea el mismo que existe en glControl por defecto
                    string indAjusDefault = GetControlValueByCompany(empresa, ModulesPrefix.co, AppControl.co_IndMultimoneda);
                    bool indAj = true;
                    if (indAjusDefault.Equals("0"))
                    {
                        indAj = false;
                        if (!string.IsNullOrWhiteSpace(dto.AjCambioTerceroInd.Value.ToString()) && dto.AjCambioTerceroInd.Value != indAj)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "AjCambioTerceroInd";
                            rdF.Message = MasterMessages.Co_PlanCta_AjCambioInd_ValidValue;
                            result.Add(rdF);

                        }
                    }

                    #endregion
                    #region TerceroInd
                    ////Valida que el indicador de tercero este activado cuando hay Tipo de Impuesto
                    //if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.TerceroInd.Value == false ||
                    //    (string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.TerceroInd.Value == true))
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "TerceroInd";
                    //    rdF.Message = MasterMessages.Co_PlanCta_Tercero_ActiveOnly;
                    //    result.Add(rdF);
                    //}
                    #endregion
                    #region ImpuestoPorc
                    //Verifica que el campo ImpuestoPorc siempre sea mayor a 0 cuando existe Tipo de Impuesto
                    if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && (dto.ImpuestoPorc.Value <= 0 || dto.ImpuestoPorc.Value >= 1000))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ImpuestoPorc";
                        rdF.Message = MasterMessages.Co_PlanCta_ImpPorc_ValueLimit;
                        result.Add(rdF);
                    }
                    #endregion
                    #region MontoMinimo
                    //Verifica que el campo MontoMinimo siempre sea mayor o igual a 0 cuando existe Tipo de Impuesto
                    if (!string.IsNullOrWhiteSpace(dto.ImpuestoTipoID.Value) && dto.MontoMinimo.Value < 0)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "MontoMinimo";
                        rdF.Message = MasterMessages.Co_PlanCta_MontoMinimo_ValueLimit;
                        result.Add(rdF);
                    }
                    #endregion
                    #region NITCierreAnual
                    //Solicita llenar  NITCierreAnual cuando tipo igual es a 2
                    //if (dto.Tipo.Value.Equals("2"))
                    //{
                    //    if (string.IsNullOrWhiteSpace(dto.NITCierreAnual.Value))
                    //    {
                    //        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //        rdF.Field = "NITCierreAnual";
                    //        rdF.Message = MasterMessages.Co_PlanCta_NITCierreAn_NotEmpty;
                    //        result.Add(rdF);
                    //    }
                    //}
                    #endregion
                    #region coCuentaGrupo

                    if (!string.IsNullOrWhiteSpace(dto.CuentaGrupoID.Value))
                    {
                        DAL_glTabla tableCta = new DAL_glTabla(c, tx, empresa, userID, loggerConn);
                        DTO_glTabla getTableCta = table.DAL_glTabla_GetByTablaNombre("coPlanCuenta", empresaGrupo);
                        int mascaraGrupoCta = Convert.ToInt32(dto.MascaraCta.Value);

                        int longiTotal = getTableCta.CodeLength(getTableCta.LevelsUsed());
                        int[] nivelUsed = getTableCta.CompleteLevelLengths();
                        if (!nivelUsed.Contains<int>(mascaraGrupoCta) && mascaraGrupoCta != 0)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CuentaGrupoID";
                            rdF.Message = MasterMessages.Co_PlanCta_CuentaGrupo_InvalidMask;
                            result.Add(rdF);
                        }

                    }
                    #endregion
                    #region TipoTercero
                    if (dto.TipoTercero.Value != 0 && dto.TipoTercero.Value != 1 && dto.TipoTercero.Value != 2 && dto.TipoTercero.Value != 3 && dto.TipoTercero.Value != null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoTercero";
                        rdF.Message = MasterMessages.Co_RegimenFiscal_TipoTercero_Restr;
                        result.Add(rdF);
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
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "FCierre";
                            rdF.Message = MasterMessages.Co_Proyecto_FCierre_NotLess;
                            result.Add(rdF);
                        }
                        #endregion
                        #region ProyectoTipo
                        if (dto.ProyectoTipo.Value != 1 && dto.ProyectoTipo.Value != 2 && dto.ProyectoTipo.Value != 3 &&
                            dto.ProyectoTipo.Value != 4 && dto.ProyectoTipo.Value != 5 && dto.ProyectoTipo.Value != 6 &&
                            dto.ProyectoTipo.Value != 7)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "ProyectoTipo";
                            rdF.Message = MasterMessages.Co_Proyecto_ProyTipo_Restr;
                            result.Add(rdF);
                        }
                        #endregion
                        #region DiasEstimados
                        //Verifica que el campo DiasEstimados siempre sea mayor a 0 
                        if (dto.DiasEstimados.Value < 0 && !string.IsNullOrWhiteSpace(dto.DiasEstimados.Value.ToString()))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "DiasEstimados";
                            rdF.Message = MasterMessages.Co_Proyecto_DiasEst_DayLimit;
                            result.Add(rdF);
                        }
                        #endregion
                        #region TipoComercial
                        if (dto.TipoComercial.Value != 1 && dto.TipoComercial.Value != 2 && dto.TipoComercial.Value != 3)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "TipoComercial";
                            rdF.Message = MasterMessages.Co_Proyecto_TipoComercial_Restr;
                            result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoTercero";
                        rdF.Message = MasterMessages.Co_RegimenFiscal_TipoTercero_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "SaldoMvto";
                        rdF.Message = MasterMessages.Co_ReporteLinea_SaldoMvto_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region coTercero
                if (dtoObj.GetType() == typeof(DTO_coTercero))
                {
                    DTO_coTercero dto = (DTO_coTercero)dtoObj;
                    //Valida El Documento de Tercero  verificando si es personal natural
                    UDT_BasicID udtTerDoc = new UDT_BasicID() { Value = dto.TerceroDocTipoID.Value };
                    string eg = dto.EmpresaGrupoID.Value;

                    DAL_MasterSimple msTerDoc = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msTerDoc.DocumentID = AppMasters.coTerceroDocTipo;

                    DTO_coTerceroDocTipo tipoDoc = (DTO_coTerceroDocTipo)msTerDoc.DAL_MasterSimple_GetByID(udtTerDoc, true);

                    #region TerceroID (DigitVerif)
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
                                string digIncorr = dto.DigitoVerif.Value;
                                dto.DigitoVerif.Value = dig.ToString();
                                //DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                //rdF.Field = "DigitoVerif";
                                //rdF.Message = MasterMessages.Co_Tercero_DigVerif_Change + "&&" + digIncorr + "&&" + dig;
                                //result.Add(rdF);
                            }
                        }
                        else
                            dto.DigitoVerif.Value = dig.ToString();

                    }
                    catch (Exception e)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TerceroID";
                        rdF.Message = MasterMessages.Co_Tercero_ID_OnlyNumber;
                        result.Add(rdF);
                    }



                    #endregion
                    #region BancoID_1
                    //Solicita llenar CuentaTipo_1 y BcoCtaNro_1 cuando hay Banco asignado
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && (string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString())
                        || ((dto.CuentaTipo_1.Value != 3) && string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString()))))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BancoID_1";
                        rdF.Message = MasterMessages.Co_Tercero_CtaTipo_AccountRequired + "&&" + dto.BancoID_1.Value;
                        result.Add(rdF);
                    }
                    else if (string.IsNullOrWhiteSpace(dto.BancoID_1.Value) &&
                            (!string.IsNullOrWhiteSpace(dto.CuentaTipo_1.Value.ToString()) || !string.IsNullOrWhiteSpace(dto.BcoCtaNro_1.Value.ToString())))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BancoID_1";
                        rdF.Message = MasterMessages.Co_Tercero_BancoID1_BanKRequired + "&&" + dto.CuentaTipo_1.Value + "&&" + dto.BcoCtaNro_1.Value;
                        result.Add(rdF);
                    }
                    #endregion
                    #region DeclaraIVAInd
                    UDT_BasicID udtRegimen = new UDT_BasicID() { Value = dto.ReferenciaID.Value };
                    DAL_MasterSimple msRegimen = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msRegimen.DocumentID = AppMasters.coRegimenFiscal;
                    DTO_coRegimenFiscal regimen = (DTO_coRegimenFiscal)msRegimen.DAL_MasterSimple_GetByID(udtRegimen, true);

                    if (regimen.TipoTercero.Value == 2 && dto.DeclaraIVAInd.Value.Value)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DeclaraIVAInd";
                        rdF.Message = MasterMessages.Co_Tercero_DeclaraIVAInd_Invalid;
                        result.Add(rdF);
                    }

                    #endregion
                    #region CuentaTipo_1
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_1.Value) && dto.CuentaTipo_1.Value != 1 && dto.CuentaTipo_1.Value != 2 && dto.CuentaTipo_1.Value !=3)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CuentaTipo_1";
                        rdF.Message = MasterMessages.Co_Tercero_CuentaTipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region BancoID_2
                    //Solicita llenar CuentaTipo_2 y BcoCtaNro_2 cuando hay Banco asignado
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_2.Value) &&
                       (string.IsNullOrWhiteSpace(dto.CuentaTipo_2.Value.ToString()) || string.IsNullOrWhiteSpace(dto.BcoCtaNro_2.Value.ToString())))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BancoID_2";
                        rdF.Message = MasterMessages.Co_Tercero_CtaTipo_AccountRequired + "&&" + dto.BancoID_2.Value;
                        result.Add(rdF);
                    }
                    else if (string.IsNullOrWhiteSpace(dto.BancoID_2.Value) &&
                            (!string.IsNullOrWhiteSpace(dto.CuentaTipo_2.Value.ToString()) || !string.IsNullOrWhiteSpace(dto.BcoCtaNro_2.Value.ToString())))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BancoID_2";
                        rdF.Message = MasterMessages.Co_Tercero_BancoID2_BanKRequired + "&&" + dto.CuentaTipo_2.Value + "&&" + dto.BcoCtaNro_2.Value;
                        result.Add(rdF);
                    }
                    #endregion
                    #region CuentaTipo_2
                    if (!string.IsNullOrWhiteSpace(dto.BancoID_2.Value) && dto.CuentaTipo_2.Value != 1 && dto.CuentaTipo_2.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CuentaTipo_2";
                        rdF.Message = MasterMessages.Co_Tercero_CuentaTipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Descriptivo = ApellidoPri + ApellidoSdo + NombrePri + NombreSdo
                    //Concatena los valores ingresados de Nombres y apellidos para crear la descripción teniendo el cuenta el campo PersonaNaturalInd de TerceroDocTipo
                    string nameDesc = string.Empty, ap1 = string.Empty, ap2 = string.Empty, nom1 = string.Empty, nom2 = string.Empty;

                    if (tipoDoc != null)
                    {
                        if (tipoDoc.PersonaNaturalInd.Value == true)
                        {
                            if (!string.IsNullOrWhiteSpace(dto.ApellidoPri.Value) && !string.IsNullOrWhiteSpace(dto.NombrePri.Value))
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
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = "Descriptivo";
                                        rdF.Message = MasterMessages.Co_Tercero_Descriptivo_MaxLength;
                                        result.Add(rdF);
                                    }
                                }
                            }
                            else
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "Descriptivo";
                                rdF.Message = MasterMessages.Co_Tercero_NombreApellidoPri_Required;
                                result.Add(rdF);
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
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "Descriptivo";
                                rdF.Message = MasterMessages.Co_Tercero_ApellidoPriRazon_Required;
                                result.Add(rdF);
                            }
                        }
                    }
                    #endregion
                    #region TerceroDocTipoID
                    //Valida que los datos ingresados correspondan a persona natural
                    if (tipoDoc != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dto.TerceroDocTipoID.Value) && tipoDoc.PersonaNaturalInd.Value == false &&
                                      (!string.IsNullOrWhiteSpace(dto.ApellidoSdo.Value) || !string.IsNullOrWhiteSpace(dto.NombrePri.Value) || !string.IsNullOrWhiteSpace(dto.NombreSdo.Value)))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "TerceroDocTipoID";
                            rdF.Message = MasterMessages.Co_Tercero_TerceroDocID_NotNaturalPerson;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                    #region CECorporativo
                    if (!string.IsNullOrWhiteSpace(dto.CECorporativo.Value))
                    {
                        bool isFormat = Regex.IsMatch(dto.CECorporativo.Value, @"[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[A-Za-z.]{2,6}");
                        if (!isFormat)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CECorporativo";
                            rdF.Message = MasterMessages.Co_Tercero_CECorporativo_EmailInvalidFormat;
                            result.Add(rdF);
                        }
                    }

                    #endregion
                    #region RepLegalCE
                    if (!string.IsNullOrWhiteSpace(dto.RepLegalCE.Value))
                    {
                        bool isFormat = Regex.IsMatch(dto.RepLegalCE.Value, @"[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[A-Za-z.]{2,6}");
                        if (!isFormat)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "RepLegalCE";
                            rdF.Message = MasterMessages.Co_Tercero_RepLegalCE_EmailInvalidFormat;
                            result.Add(rdF);
                        }
                    }

                    #endregion
                    #region ResponsableTercero
                    int terce = AppMasters.coTercero;
                    bool isNew = true;

                    UDT_BasicID udtResp = new UDT_BasicID() { Value = dto.ID.Value };
                    DAL_MasterSimple msRespon = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msRespon.DocumentID = AppMasters.coTercero;
                    DTO_coTercero idTercero = (DTO_coTercero)msRespon.DAL_MasterSimple_GetByID(udtResp, true);

                    if (idTercero == null)
                        isNew = true;
                    else
                        isNew = false;
                    DTO_seUsuario userRplica = new DTO_seUsuario();
                    userRplica.ReplicaID.Value = userID;
                    UDT_BasicID udtUser = new UDT_BasicID() { Value = userRplica.ReplicaID.Value.ToString() };
                    DAL_MasterSimple msUser = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msUser.DocumentID = AppMasters.seUsuario;
                    DAL_seUsuario dal_user = new DAL_seUsuario(c, tx, empresa, userID, loggerConn);
                    DTO_seUsuario user = (DTO_seUsuario)dal_user.DAL_seUsuario_GetUserByReplicaID(userID);

                    bool usuario = user.ResponsableTercerosInd.Value.Value;

                    if (isNew)
                    {
                        if (usuario)
                            dto.UsuarioResponsable.Value = user.ID.Value;
                        else
                            dto.UsuarioResponsable.Value = string.Empty;
                    }
                    else
                    {
                        if (usuario)
                        {
                            //if (dto.UsuarioResponsable.Value != string.Empty)
                            //    user.ID.Value = dto.UsuarioResponsable.Value;
                            //else
                            //{
                                dto.UsuarioResponsable.Value = user.ID.Value.ToString();
                            //}
                        }
                        else
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "ResponsableTercero";
                            rdF.Message = MasterMessages.Co_Tercero_RespTercero_InvalidResp;
                            result.Add(rdF);
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
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "CuentaReteIVA";
                    //    rdF.Message = MasterMessages.Co_ValIVA_ReteIVA_NotEmpty;
                    //    result.Add(rdF);
                    //}
                    //else if (string.IsNullOrWhiteSpace(dto.CuentaReteIVA.Value) && !string.IsNullOrWhiteSpace(dto.CuentaCostoReteIVA.Value))
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "CuentaReteIVA";
                    //    rdF.Message = MasterMessages.Co_ValIVA_ReteIVA_Empty;
                    //    result.Add(rdF);
                    //}
                    //#endregion
                }
                #endregion
                #region coPLanillaConsolidacion
                if (dtoObj.GetType() == typeof(DTO_coPlanillaConsolidacion))
                {
                    Dictionary<string, string> pksplanilla = new Dictionary<string, string>();
                    Dictionary<string, string> pksCtroCosto = new Dictionary<string, string>();

                    int ctroCosto = AppMasters.coCentroCosto;
                    DTO_coPlanillaConsolidacion dto = (DTO_coPlanillaConsolidacion)dtoObj;

                    UDT_BasicID udtCola = new UDT_BasicID() { Value = dto.EmpresaID.Value };
                    DAL_MasterComplex msRespon = new DAL_MasterComplex(c, tx, empresa, userID, loggerConn);
                    msRespon.DocumentID = AppMasters.coPlanillaConsolidacion;

                    pksplanilla.Add("EmpresaID", dto.EmpresaID.Value);
                    pksCtroCosto.Add("CentroCostoID", dto.CentroCostoID.Value);

                    DTO_coPlanillaConsolidacion empresaPLa = (DTO_coPlanillaConsolidacion)msRespon.DAL_MasterComplex_GetByID(pksplanilla, true);
                    DTO_coPlanillaConsolidacion ctrocsto = (DTO_coPlanillaConsolidacion)msRespon.DAL_MasterComplex_GetByID(pksCtroCosto, true);
                    if (empresa.ID.Value != dto.EmpresaID.Value)
                    {
                        if (empresaPLa != null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "EmpresaID";
                            rdF.Message = MasterMessages.Co_PlanillaCons_Company_Assigned;
                            result.Add(rdF);
                        }
                        else if (ctrocsto != null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CentroCostoID";
                            rdF.Message = MasterMessages.Co_PlanillaCons_CentroCosto_Assigned;
                            result.Add(rdF);
                        }
                    }
                    else
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "EmpresaGrupo";
                        rdF.Message = MasterMessages.Co_PlanillaCons_EmpresaGrupo_Assigned;
                        result.Add(rdF);
                    }
                }
                #endregion
                #region cpAnticipoTipo
                if (dtoObj.GetType() == typeof(DTO_cpAnticipoTipo))
                {
                    DTO_cpAnticipoTipo dto = (DTO_cpAnticipoTipo)dtoObj;
                    #region coDocumentoID

                    DAL_MasterSimple masterSimple = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    masterSimple.DocumentID = AppMasters.coDocumento;

                    UDT_BasicID udtEmp = new UDT_BasicID() { Value = dto.coDocumentoID.Value };
                    DTO_coDocumento doc = (DTO_coDocumento)masterSimple.DAL_MasterSimple_GetByID(udtEmp, false);

                    if (doc != null)
                    {
                        DAL_MasterHierarchy masterHierar = new DAL_MasterHierarchy(c, tx, empresa, userID, loggerConn);
                        masterHierar.DocumentID = AppMasters.coPlanCuenta;
                        UDT_BasicID udtctaLoc = new UDT_BasicID() { Value = doc.CuentaLOC.Value };
                        DTO_coPlanCuenta ctaLoc = (DTO_coPlanCuenta)masterHierar.DAL_MasterSimple_GetByID(udtctaLoc, false);

                        UDT_BasicID udtctaExt = new UDT_BasicID() { Value = doc.CuentaEXT.Value };
                        DTO_coPlanCuenta ctaExt = (DTO_coPlanCuenta)masterHierar.DAL_MasterSimple_GetByID(udtctaExt, false);

                        if (ctaLoc != null && ctaExt != null)
                        {
                            masterSimple.DocumentID = AppMasters.glConceptoSaldo;
                            UDT_BasicID udtsaldoLoc = new UDT_BasicID() { Value = ctaLoc.ConceptoSaldoID.Value };
                            DTO_glConceptoSaldo saldoLoc = (DTO_glConceptoSaldo)masterSimple.DAL_MasterSimple_GetByID(udtsaldoLoc, false);

                            UDT_BasicID udtsaldoExt = new UDT_BasicID() { Value = ctaExt.ConceptoSaldoID.Value };
                            DTO_glConceptoSaldo saldoExt = (DTO_glConceptoSaldo)masterSimple.DAL_MasterSimple_GetByID(udtsaldoExt, false);
                            if (saldoLoc != null && saldoExt != null)
                            {
                                if (saldoLoc.coSaldoControl.Value != (byte)SaldoControl.Doc_Externo)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "coDocumentoID";
                                    rdF.Message = MasterMessages.Gl_ConceptoSaldo_coDocumento_Restr;
                                    result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CargoTipo";
                        rdF.Message = MasterMessages.Co_CargoEspecial_CargoTipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region cpConceptoCXP
                if (dtoObj.GetType() == typeof(DTO_cpConceptoCXP))
                {
                    DTO_cpConceptoCXP dto = (DTO_cpConceptoCXP)dtoObj;
                    DAL_MasterSimple msSaldo = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msSaldo.DocumentID = AppMasters.coDocumento;

                    UDT_BasicID udtEmp = new UDT_BasicID(true) { Value = dto.coDocumentoID.Value};
                    DTO_coDocumento documento = (DTO_coDocumento)msSaldo.DAL_MasterSimple_GetByID(udtEmp, true);

                    #region ConceptoTipo
                    if (dto.ConceptoTipo.Value != 1 && dto.ConceptoTipo.Value != 2
                        && dto.ConceptoTipo.Value != 3 && dto.ConceptoTipo.Value != 4)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ConceptoTipo";
                        rdF.Message = MasterMessages.Cp_ConceptoCXP_TipoConcepto_Restr;
                        result.Add(rdF);
                    }
                    #endregion

                    #region Documento Contable

                    if (documento.DocumentoID.Value != AppDocuments.CausarFacturas.ToString() && documento.DocumentoID.Value != AppDocuments.NotaCreditoCxP.ToString())
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DocumentoID";
                        rdF.Message = MasterMessages.Cp_ConceptoCXP_DocumentoID_Restr;
                        result.Add(rdF);
                    }

                    #endregion
                }
                #endregion
                #region cpDistribuyeImpLocal
                if (dtoObj.GetType() == typeof(DTO_cpDistribuyeImpLocal))
                {
                    DTO_cpDistribuyeImpLocal dto = (DTO_cpDistribuyeImpLocal)dtoObj;

                    #region Porcentaje
                    //Verifica que el campo Porcentaje siempre sea igual a 100%
                    //if (dto.Porcentaje.Value != 100)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "Porcentaje";
                    //    rdF.Message = MasterMessages.Cp_Distribuye_PorcentajeEqual100;
                    //    result.Add(rdF);
                    //}
                    #endregion
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoConcepto";
                        rdF.Message = MasterMessages.Fa_Conceptos_TipoConcepto_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion                
                #region faFacturaTipo

                if (dtoObj.GetType() == typeof(DTO_faFacturaTipo))
                {
                    DTO_faFacturaTipo dto = (DTO_faFacturaTipo)dtoObj;
                    
                    #region coDocumentoID

                    UDT_BasicID udtDoc = new UDT_BasicID() { Value = dto.coDocumentoID.Value };
                    DAL_MasterSimple msDoc = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msDoc.DocumentID = AppMasters.coDocumento;
                    DTO_coDocumento coDoc = (DTO_coDocumento)msDoc.DAL_MasterSimple_GetByID(udtDoc, true);


                    if (coDoc.DocumentoID.Value != "41" && coDoc.DocumentoID.Value != "42")
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "coDocumentoID";
                        rdF.Message = MasterMessages.Fa_FacturaTipo_Documento_Checked;
                        result.Add(rdF);
                    }

                    #endregion

                }

                #endregion
                #region glConceptoSaldo
                if (dtoObj.GetType() == typeof(DTO_glConceptoSaldo))
                {
                    DTO_glConceptoSaldo dto = (DTO_glConceptoSaldo)dtoObj;
                    #region coSaldoControl
                    if (dto.coSaldoControl.Value != 1 && dto.coSaldoControl.Value != 2 && dto.coSaldoControl.Value != 3 &&
                        dto.coSaldoControl.Value != 4 && dto.coSaldoControl.Value != 5 && dto.coSaldoControl.Value != 6)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "coSaldoControl";
                        rdF.Message = MasterMessages.Gl_ConceptoSaldo_coSaldoCtrl_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region glDatosMensuales
                if (dtoObj.GetType() == typeof(DTO_glDatosMensuales))
                {
                    DTO_glDatosMensuales dto = (DTO_glDatosMensuales)dtoObj;
                    if (dto.Periodo.Value != dto.PeriodoID.Value)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "PeriodoID";
                        rdF.Message = MasterMessages.gl_DatosMensuales_PeriodoID_Err;
                        result.Add(rdF);
                    }
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
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasFestivoID";
                        rdF.Message = MasterMessages.gl_DiasFestivos_PeriodoID_Err;
                        result.Add(rdF);
                    }
                }
                #endregion
                #region glEmpresa
                if (dtoObj.GetType() == typeof(DTO_glEmpresa))
                {
                    DTO_glEmpresa dto = (DTO_glEmpresa)dtoObj;

                    //Trae la maestra 
                    DAL_MasterSimple msImpTipo = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msImpTipo.DocumentID = AppMasters.glEmpresa;

                    UDT_BasicID udtEmp = new UDT_BasicID() { Value = dto.ID.Value };
                    DTO_glEmpresa emp = (DTO_glEmpresa)msImpTipo.DAL_MasterSimple_GetByID(udtEmp, false);

                    if (emp != null && dto.EmpresaGrupoID_.Value != emp.EmpresaGrupoID_.Value)
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

                        DAL_aplBitacora bit = new DAL_aplBitacora(c, tx, empresa, userID, loggerConn);

                        long cont = 0;//bit.DAL_aplBitacoraCountFiltered(query);
                        if (cont > 0)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "EmpresaGrupoID";
                            rdF.Message = MasterMessages.Gl_Empresa_InvalidGE + "&&" + dto.ID.Value + "&&" + emp.EmpresaGrupoID_.Value;
                            result.Add(rdF);
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
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "descrNivel" + (i + 1);
                                rdF.Message = MasterMessages.Gl_Tabla_LengthDescriptionRequired;
                                result.Add(rdF);
                            }
                            else
                            {
                                if (dto.LevelDescs().Count(x => x.Equals(dto.LevelDescs()[i])) > 1)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "descrNivel" + (i + 1);
                                    rdF.Message = MasterMessages.Gl_Tabla_DescriptionNotRepeat;
                                    result.Add(rdF);
                                }
                            }
                        }
                        if (dto.LevelLengths()[i] != null && dto.LevelLengths()[i] > 0)
                            totalLength += dto.LevelLengths()[i];
                    }
                    try
                    {

                        int max = StaticMethods.GetParameters(c, tx, Convert.ToInt32(dto.ID.Value), loggerConn).IDLongitudMax;
                        if (max < totalLength)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "lonNivel1";
                            rdF.Message = MasterMessages.Gl_Tabla_LengthExceedID + " ( " + max + " ) ";
                            result.Add(rdF);
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
                        // bool isFormat = Regex.IsMatch(dto.EntradaHora.Value.ToString(), "[0-23]");
                        if (dto.EntradaHora.Value < 0 || dto.EntradaHora.Value > 23)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "EntradaHora";
                            rdF.Message = MasterMessages.Gl_Horario_RankHour;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                    #region SalidaHora
                    if (dto.SalidaHora.Value != null)
                    {
                        //bool isFormat = Regex.IsMatch(dto.SalidaHora.Value.ToString(), "[0-23]");
                        if (dto.SalidaHora.Value < 0 || dto.SalidaHora.Value > 23)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "SalidaHora";
                            rdF.Message = MasterMessages.Gl_Horario_RankHour;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                    #region EntradaMinutos
                    if (dto.EntradaMinutos.Value != null)
                    {
                        //bool isFormat = Regex.IsMatch(dto.EntradaMinutos.Value.ToString(), "[0-5][0-9]");
                        if (dto.EntradaMinutos.Value < 0 || dto.EntradaMinutos.Value > 59)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "EntradaMinutos";
                            rdF.Message = MasterMessages.Gl_Horario_RankMinute;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                    #region SalidaMinutos
                    if (dto.SalidaMinutos.Value != null)
                    {
                        // bool isFormat = Regex.IsMatch(dto.SalidaMinutos.Value.ToString(), "[0-5][0-9]");
                        if (dto.SalidaMinutos.Value < 0 || dto.SalidaMinutos.Value > 59)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "SalidaMinutos";
                            rdF.Message = MasterMessages.Gl_Horario_RankMinute;
                            result.Add(rdF);
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
                    //if (dto.TipoDocumento.Value != 0 && dto.TipoDocumento.Value != 1 && dto.TipoDocumento.Value != 2 && dto.TipoDocumento.Value != 3)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = "TipoDocumento";
                    //    rdF.Message = MasterMessages.Gl_DocumentoAnexo_TipoDoc_Restr;
                    //    result.Add(rdF);
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
                        dto.TipoInv.Value != 6 && dto.TipoInv.Value != 7 && dto.TipoInv.Value != 8 && dto.TipoInv.Value != 9)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoInv";
                        rdF.Message = MasterMessages.In_RefTipo_TipoInv_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region ControlEspecial
                    if (dto.ControlEspecial.Value != 0 && dto.ControlEspecial.Value != 1 &&
                        dto.ControlEspecial.Value != 2 && dto.ControlEspecial.Value != 3 &&
                        dto.ControlEspecial.Value != 4 && dto.ControlEspecial.Value != 5 &&
                        dto.ControlEspecial.Value != 6)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ControlEspecial";
                        rdF.Message = MasterMessages.In_RefTipo_ControlEspecial_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region NumParametros

                    #endregion
                    #region ControlImportacion
                    if (dto.ControlImportacion.Value != 0 && dto.ControlImportacion.Value != 1 &&
                        dto.ControlImportacion.Value != 2 && dto.ControlImportacion.Value != 3)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ControlImportacion";
                        rdF.Message = MasterMessages.In_RefTipo_ControlImportacion_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region inBodega

                if (dtoObj.GetType() == typeof(DTO_inBodega))
                {
                    DTO_inBodega dto = (DTO_inBodega)dtoObj;

                    #region BodegaTipoID

                    DAL_MasterSimple ms = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    UDT_BasicID udt = new UDT_BasicID() { Value = dto.BodegaTipoID.Value };
                    ms.DocumentID = AppMasters.inBodegaTipo;
                    DTO_inBodegaTipo inBodTipo = (DTO_inBodegaTipo)ms.DAL_MasterSimple_GetByID(udt, true);

                    udt = new UDT_BasicID() { Value = dto.CosteoGrupoInvID.Value };
                    ms.DocumentID = AppMasters.inCosteoGrupo;
                    DTO_inCosteoGrupo inCosteo = (DTO_inCosteoGrupo)ms.DAL_MasterSimple_GetByID(udt, true);

                    if ((inBodTipo.BodegaTipo.Value == (byte)TipoBodega.PuertoFOB || inBodTipo.BodegaTipo.Value == (byte)TipoBodega.ZonaFranca ||
                        inBodTipo.BodegaTipo.Value == (byte)TipoBodega.Transito || inBodTipo.BodegaTipo.Value == (byte)TipoBodega.Traslado) &&
                        inCosteo.CosteoTipo.Value != (byte)TipoCosteoInv.Transaccional)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BodegaTipoID";
                        rdF.Message = MasterMessages.In_Bodega_BodegaTipoTransaccional_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BodegaTipo";
                        rdF.Message = MasterMessages.In_BodegaTipo_BodegaTipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region inCosteoGrupo
                if (dtoObj.GetType() == typeof(DTO_inCosteoGrupo))
                {
                    DTO_inCosteoGrupo dto = (DTO_inCosteoGrupo)dtoObj;
                    #region CosteoTipo
                    if (dto.CosteoTipo.Value != 0 && dto.CosteoTipo.Value != 1 && dto.CosteoTipo.Value != 2 && dto.CosteoTipo.Value != 3 && dto.CosteoTipo.Value != 4)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CosteoTipo";
                        rdF.Message = MasterMessages.In_CosteoGrupo_CosteoTipo_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoMovimiento";
                        rdF.Message = MasterMessages.In_MovimientoTipo_TipoMovimiento_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Porcentaje";
                        rdF.Message = MasterMessages.In_PosicionArancel_Porcentaje_Invalid;
                        result.Add(rdF);
                    }
                    else
                    {  //Valida los decimales del valor
                        string valor = dto.Porcentaje.Value.Value.ToString();
                        int nroDec = valor.IndexOf(',') + 1;
                        int nroDecim = valor.Substring(nroDec).Length;
                        if (nroDecim > 8)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "Porcentaje";
                            rdF.Message = MasterMessages.In_PosicionArancel_Porcentaje_InvalidDecimal;
                            result.Add(rdF);
                        }
                    }
                    #endregion
                }
                #endregion
                #region noEmpleado
                if (dtoObj.GetType() == typeof(DTO_noEmpleado))
                {
                    DTO_noEmpleado dto = (DTO_noEmpleado)dtoObj;

                    DAL_noEmpleado dalEmp = new DAL_noEmpleado(c, tx, empresa, userID, loggerConn);

                    int count = dalEmp.DAL_noEmpleado_CountTerceroID(dto.TerceroID.Value);

                    #region Tercero ID
                    if (insertando)
                    {
                        if (count != 0)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "TerceroID";
                            rdF.Message = MasterMessages.No_Empleado_TerceroID_Restr;
                            result.Add(rdF);
                        }
                    }
                    else
                    {
                        if (count != 0 && count!=1)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "TerceroID";
                            rdF.Message = MasterMessages.No_Empleado_TerceroID_Restr;
                            result.Add(rdF);
                        }
                    }

                    #endregion

                    #region Enmpleado Activo o Inactivo
                    if (dto.Estado.Value != 1 && dto.Estado.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Estado";
                        rdF.Message = MasterMessages.No_Empleado_Tipo_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Tipo";
                        rdF.Message = MasterMessages.No_ConceptoPlaTra_Tipo_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Tipo";
                        rdF.Message = MasterMessages.No_ConceptoNOM_Tipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region TipoLiquidacion
                    if (dto.TipoLiquidacion.Value != 1 && dto.TipoLiquidacion.Value != 2 && dto.TipoLiquidacion.Value != 3 && dto.TipoLiquidacion.Value != 4)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoLiquidacion";
                        rdF.Message = MasterMessages.No_ConceptoNOM_TipoLiq_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region BaseFormula
                    if (dto.BaseFormula.Value != 0 && dto.BaseFormula.Value != 1 && dto.BaseFormula.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "BaseFormula";
                        rdF.Message = MasterMessages.No_ConceptoNOM_TipoLiq_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region TipoTercero
                    if (dto.TipoTercero.Value != 0 && dto.TipoTercero.Value != 1 && dto.TipoTercero.Value != 2 && dto.TipoTercero.Value != 3)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoTercero";
                        rdF.Message = MasterMessages.No_ConceptoTipoNOM_TipoTercero_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Tipo";
                        rdF.Message = MasterMessages.No_CompFlexible_Tipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region PeriodoPago
                    if (dto.PeriodoPago.Value != 1 && dto.PeriodoPago.Value != 2 && dto.PeriodoPago.Value != 3)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "PeriodoPago";
                        rdF.Message = MasterMessages.No_CompFlexible_PeriodoPago_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasIni1";
                        rdF.Message = MasterMessages.No_ContratoNov_Dias_RangeRestr;
                        result.Add(rdF);
                    }
                    if (dto.DiasFin1.Value < 1 || dto.DiasFin1.Value > 360)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasFin1";
                        rdF.Message = MasterMessages.No_ContratoNov_Dias_RangeRestr;
                        result.Add(rdF);
                    }
                    if (dto.DiasIni2.Value < 1 || dto.DiasIni2.Value > 360)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasIni2";
                        rdF.Message = MasterMessages.No_ContratoNov_Dias_RangeRestr;
                        result.Add(rdF);
                    }
                    if (dto.DiasFin2.Value < 1 || dto.DiasFin2.Value > 360)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasFin2";
                        rdF.Message = MasterMessages.No_ContratoNov_Dias_RangeRestr;
                        result.Add(rdF);
                    }
                    if (dto.DiasIni3.Value < 1 || dto.DiasIni3.Value > 360)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasIni3";
                        rdF.Message = MasterMessages.No_ContratoNov_Dias_RangeRestr;
                        result.Add(rdF);
                    }
                    if (dto.DiasFin3.Value < 1 || dto.DiasFin3.Value > 360)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "DiasFin3";
                        rdF.Message = MasterMessages.No_ContratoNov_Dias_RangeRestr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoNovedad";
                        rdF.Message = MasterMessages.No_ContratoNov_TipoNovedad_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Porcentaje";
                        rdF.Message = MasterMessages.pl_DistribucionCampo_PorcentajeInvalid;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoCompetencia";
                        rdF.Message = MasterMessages.Rh_Competencia_TipoCompet_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Nivel";
                        rdF.Message = MasterMessages.Rh_CompetenciaxCargo_Nivel_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region Necesidad
                    if (dto.Necesidad.Value != 1 && dto.Necesidad.Value != 2)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "Necesidad";
                        rdF.Message = MasterMessages.Rh_CompetenciaxCargo_Necesidad_Restr;
                        result.Add(rdF);
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
                        bool isFormat = Regex.IsMatch(dto.CorreoElectronico.Value, @"[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[A-Za-z.]{2,6}");
                        if (!isFormat)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CorreoElectronico";
                            rdF.Message = MasterMessages.Se_Usuario_CorreoE_EmailInvalidFormat;
                            result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CuentaTipo";
                        rdF.Message = MasterMessages.Ts_BancosCuenta_CuentaTipo_Restr;
                        result.Add(rdF);
                    }
                    #endregion
                    #region coDocumentoID

                    UDT_BasicID udtDoc = new UDT_BasicID() { Value = dto.coDocumentoID.Value };
                    DAL_MasterSimple msDoc = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msDoc.DocumentID = AppMasters.coDocumento;
                    DTO_coDocumento coDoc = (DTO_coDocumento)msDoc.DAL_MasterSimple_GetByID(udtDoc, true);

                    // Verifica que solo hayan Documentos contables con Documento 31
                    if(coDoc.DocumentoID.Value!="31")
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "coDocumentoID";
                        rdF.Message = MasterMessages.Ts_BancosCuenta_DocContable_Checked;
                        result.Add(rdF);
                    }

                    if (coDoc.MonedaOrigen.Value != (byte)TipoMoneda_CoDocumento.Local && coDoc.MonedaOrigen.Value != (byte)TipoMoneda_CoDocumento.Foreign)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "coDocumentoID";
                        rdF.Message = MasterMessages.Ts_BancosCuenta_DocContable_MdaOrigenInvalid;
                        result.Add(rdF);
                    }
                    #endregion
                }
                #endregion
                #region tsCaja
                
                if(dtoObj.GetType()== typeof(DTO_tsCaja))
                {
                    DTO_tsCaja dto = (DTO_tsCaja) dtoObj;
                    


                    #region coDocumentoID
		            UDT_BasicID udtDoc = new UDT_BasicID() { Value = dto.coDocumentoID.Value };
                    DAL_MasterSimple msDoc = new DAL_MasterSimple(c, tx, empresa, userID, loggerConn);
                    msDoc.DocumentID = AppMasters.coDocumento;
                    DTO_coDocumento coDoc = (DTO_coDocumento)msDoc.DAL_MasterSimple_GetByID(udtDoc, true);
                    if(coDoc.DocumentoID.Value!="32")
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "coDocumentoID";
                        rdF.Message = MasterMessages.Ts_Caja_DocumentoID_Restr;
                        result.Add(rdF);
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
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "TipoFlujo";
                        rdF.Message = MasterMessages.Ts_FlujoFondo_TipoFlujo_Restr;
                        result.Add(rdF);
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

        #region Control

        /// <summary>
        /// Obtiene el valor de un item de control deacuerdo al ID
        /// </summary>
        /// <param name="ctrl">Identificador del control</param>
        /// <returns>Retorna el valor de la tabla glControl</returns>
        private static string GetControlValue(int ctrl)
        {
            DTO_glControl dto = ctrlDAL.DAL_glControl_GetById(ctrl);
            string data = dto.Data.ToString();

            return data;
        }

        /// <summary>
        /// Devuelve in valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <returns>Retorna el valor buscado</returns>
        private static string GetControlValueByCompany(DTO_glEmpresa empresa, ModulesPrefix mod, string ctrl)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = GetControlValue(Convert.ToInt32(key));
            return result;
        }

        #endregion

    }
}
