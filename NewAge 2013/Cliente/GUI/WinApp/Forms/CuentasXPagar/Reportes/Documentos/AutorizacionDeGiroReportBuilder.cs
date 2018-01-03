using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Reportes;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Forms;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.ReportesComunes;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public class AutorizacionDeGiroReportBuilder
    {
        #region Variables
        protected DTO_glConsulta Consulta = null;
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Autorizacion de Giro (collects and organises report data)
        /// </summary>
        /// <param name="reportData">data for the report</param>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <param name="allFields">Indica si se debe mostrar todos los campos</param>
        public AutorizacionDeGiroReportBuilder(DTO_CuentaXPagar data, bool multiMoneda, bool show, bool allFields = false)
        {
            #region Variables
            DTO_coPlanCuenta _cuenta = new DTO_coPlanCuenta();
            DTO_coTercero _tercero = new DTO_coTercero();
            DTO_coCuentaGrupo _cuentaGrupo = new DTO_coCuentaGrupo();
            Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
            Dictionary<string, DTO_coTercero> cacheTercero = new Dictionary<string, DTO_coTercero>();
            Dictionary<string, DTO_coCuentaGrupo> cacheCuentaGrupo = new Dictionary<string, DTO_coCuentaGrupo>();

            DTO_glDocumentoControl docCtrl = data.DocControl;
            DTO_cpCuentaXPagar cuentaXPagar = data.CxP;
            DTO_Comprobante comp = data.Comp;
            DTO_cpConceptoCXP conceptoCxP = (DTO_cpConceptoCXP)this._bc.AdministrationModel.MasterSimple_GetByID(AppMasters.cpConceptoCXP, new UDT_BasicID { Value = cuentaXPagar.ConceptoCxPID.Value }, true);

            DTO_ReportAutorizacionDeGiro autGiro = new DTO_ReportAutorizacionDeGiro();
            #endregion

            #region Llena DTO del reporte
            #region Header
            autGiro.EmpresaID = cuentaXPagar.EmpresaID.Value;
            autGiro.TerceroID = docCtrl.TerceroID.Value;
            #region Trae Tercero
            if (cacheTercero.ContainsKey(docCtrl.TerceroID.Value))
                _tercero = cacheTercero[docCtrl.TerceroID.Value];
            else
            {
                _tercero = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = docCtrl.TerceroID.Value }, true);
                cacheTercero.Add(docCtrl.TerceroID.Value, _tercero);
            }
            #endregion 
            autGiro.TerceroDesc = _tercero.Descriptivo.ToString();
            EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
            autGiro.EstadoInd = (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar) ? false : true;
            autGiro.DocumentoTercero = docCtrl.DocumentoTercero.Value;
            autGiro.FechaFact = cuentaXPagar.FacturaFecha.Value.Value;
            autGiro.Fecha = docCtrl.Fecha.Value.Value;
            autGiro.FechaVto = cuentaXPagar.VtoFecha.Value.Value;
            autGiro.Descripcion = docCtrl.Observacion.Value;
            autGiro.ComprobanteID = docCtrl.ComprobanteID.Value.Trim();
            autGiro.ComprobanteNro = docCtrl.ComprobanteIDNro.Value.ToString().Trim();
            //autGiro.UsuarioResp = docCtrl.UsuarioRESP.Value;
            autGiro.TasaCambio = docCtrl.TasaCambioCONT.Value.Value;
            autGiro.DocumentoID = docCtrl.DocumentoID.Value.ToString().Trim();
            #endregion
            #region Detail
            autGiro.AutorizacionDetail = new List<DTO_AutorizacionDetail>();
            foreach (DTO_ComprobanteFooter footer in comp.Footer)
            {
                if (footer.DatoAdd4.Value != AuxiliarDatoAdd4.Contrapartida.ToString())
                {
                    DTO_AutorizacionDetail autGiroDet = new DTO_AutorizacionDetail();
                    #region Trae Cuenta y CuentaGrupo
                    if (cacheCuenta.ContainsKey(footer.CuentaID.Value))
                        _cuenta = cacheCuenta[footer.CuentaID.Value];
                    else
                    {
                        _cuenta = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = footer.CuentaID.Value }, true);
                        cacheCuenta.Add(footer.CuentaID.Value, _cuenta);
                    }

                    if (cacheCuentaGrupo.ContainsKey(_cuenta.CuentaGrupoID.Value))
                        _cuentaGrupo = cacheCuentaGrupo[_cuenta.CuentaGrupoID.Value];
                    else
                    {
                        _cuentaGrupo = (DTO_coCuentaGrupo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coCuentaGrupo, _cuenta.CuentaGrupoID, true);
                        cacheCuentaGrupo.Add(_cuenta.CuentaGrupoID.Value, _cuentaGrupo);
                    }
                    #endregion
                    autGiroDet.CuentaID = footer.CuentaID.Value;
                    autGiroDet.CentroCostoID = footer.CentroCostoID.Value;
                    autGiroDet.ProyectoID = footer.ProyectoID.Value;
                    autGiroDet.LineaPresupuestoID = footer.LineaPresupuestoID.Value;
                    autGiroDet.CuentaDesc = _cuenta.Descriptivo.Value;
                    autGiroDet.ImpuestoTipoID = _cuenta.ImpuestoTipoID.Value;

                    //autGiroDet.ValorME_SinSigno = 0;
                    autGiroDet.ValorME = 0;
                    autGiroDet.VrBrutoME = 0;
                    autGiroDet.IvaME = 0;
                    autGiroDet.ReteIvaME = 0;
                    autGiroDet.ReteFuenteME = 0;
                    autGiroDet.ReteIcaME = 0;
                    autGiroDet.TimbreME = 0;
                    autGiroDet.AnticiposME = 0;
                    autGiroDet.OtrosDtosME = 0;

                    if (conceptoCxP.ControlCostoInd.Value.Value)
                    {
                        //autGiroDet.ValorML_SinSigno = footer.vlrMdaLoc.Value.Value;
                        autGiroDet.ValorML = footer.vlrMdaLoc.Value.Value; //(_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaLoc.Value.Value * (-1) : footer.vlrMdaLoc.Value.Value;
                        autGiroDet.VrBrutoML = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString()
                            && footer.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString() && _cuentaGrupo.CostoInd.Value.Value) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.IvaML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA)
                            || _cuentaGrupo.CostoInd.Value.Value && !string.IsNullOrEmpty(footer.DatoAdd1.Value) && footer.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString()) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.ReteIvaML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA)) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.ReteFuenteML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value ==this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente)) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.ReteIcaML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoReteICA)) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.TimbreML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoTipoImpuestoConsumo)) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.AnticiposML = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString()) ? footer.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.OtrosDtosML = footer.vlrMdaLoc.Value.Value - autGiroDet.VrBrutoML - autGiroDet.IvaML - autGiroDet.ReteIvaML - autGiroDet.ReteFuenteML
                            - autGiroDet.ReteIcaML - autGiroDet.TimbreML - autGiroDet.AnticiposML;
                        if (multiMoneda)
                        {
                            //autGiroDet.ValorME_SinSigno = footer.vlrMdaExt.Value.Value;
                            autGiroDet.ValorME = footer.vlrMdaExt.Value.Value;// (_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaExt.Value.Value * (-1) : footer.vlrMdaExt.Value.Value;
                            autGiroDet.VrBrutoME = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString()
                            && footer.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString() && _cuentaGrupo.CostoInd.Value.Value) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.IvaME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoIVA)
                                || !string.IsNullOrEmpty(footer.DatoAdd1.Value) && footer.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString()) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.ReteIvaME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoReteIVA)) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.ReteFuenteME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoReteFuente)) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.ReteIcaME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoReteICA)) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.TimbreME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoTipoImpuestoConsumo)) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.AnticiposME = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString()) ? footer.vlrMdaExt.Value.Value : 0;
                            autGiroDet.OtrosDtosME = footer.vlrMdaExt.Value.Value - autGiroDet.VrBrutoME - autGiroDet.IvaME - autGiroDet.ReteIvaME - autGiroDet.ReteFuenteME
                                - autGiroDet.ReteIcaME - autGiroDet.TimbreME - autGiroDet.AnticiposME;
                        }
                    }
                    else
                    {
                        //autGiroDet.ValorML_SinSigno = footer.vlrMdaLoc.Value.Value;
                        autGiroDet.ValorML = footer.vlrMdaLoc.Value.Value; // (_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaLoc.Value.Value * (-1) : footer.vlrMdaLoc.Value.Value;
                        autGiroDet.VrBrutoML = footer.vlrMdaLoc.Value.Value; //(_cuenta.NITCierreAnual.Value != autGiro.TerceroID) ? autGiroDet.vlrMdaLoc.Value.Value : 0;
                        autGiroDet.IvaML = 0;
                        autGiroDet.ReteIvaML = 0;
                        autGiroDet.ReteFuenteML = 0;
                        autGiroDet.ReteIcaML = 0;
                        autGiroDet.TimbreML = 0;
                        autGiroDet.AnticiposML = 0;
                        autGiroDet.OtrosDtosML = 0;
                        if (multiMoneda)
                        {
                            //autGiroDet.ValorME_SinSigno = footer.vlrMdaExt.Value.Value;
                            autGiroDet.ValorME = footer.vlrMdaExt.Value.Value;
                            autGiroDet.VrBrutoME = footer.vlrMdaLoc.Value.Value;//(_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaLoc.Value.Value * (-1) : footer.vlrMdaLoc.Value.Value; //(multiMoneda && _cuenta.NITCierreAnual.Value != autGiro.TerceroID) ? autGiroDet.vlrMdaLoc.Value.Value : 0;
                        }
                    }
                    autGiroDet.BaseML = footer.vlrBaseML.Value.Value;
                    autGiroDet.Percent = (footer.vlrBaseML.Value.Value != 0) ? (Math.Round(footer.vlrMdaLoc.Value.Value / footer.vlrBaseML.Value.Value, 2)).ToString() : "*";
                    autGiroDet.VrNetoML = autGiroDet.VrBrutoML + autGiroDet.IvaML + autGiroDet.ReteIvaML + autGiroDet.ReteFuenteML + autGiroDet.ReteIcaML
                        + autGiroDet.TimbreML + autGiroDet.AnticiposML + autGiroDet.OtrosDtosML + autGiroDet.VrNetoML;
                    autGiroDet.VrNetoME = autGiroDet.VrBrutoME + autGiroDet.IvaME + autGiroDet.ReteIvaME + autGiroDet.ReteFuenteME + autGiroDet.ReteIcaME
                        + autGiroDet.TimbreME + autGiroDet.AnticiposME + autGiroDet.OtrosDtosME + autGiroDet.VrNetoME;

                    autGiro.AutorizacionDetail.Add(autGiroDet);
                }
            };
            #endregion 
            #region Footer
            autGiro.VrBrutoML = autGiro.AutorizacionDetail.Sum(x => x.VrBrutoML);
            autGiro.IvaML = autGiro.AutorizacionDetail.Sum(x => x.IvaML);
            autGiro.ReteIvaML = autGiro.AutorizacionDetail.Sum(x => x.ReteIvaML) * (-1);
            autGiro.ReteFuenteML = autGiro.AutorizacionDetail.Sum(x => x.ReteFuenteML) * (-1);
            autGiro.ReteIcaML = autGiro.AutorizacionDetail.Sum(x => x.ReteIcaML) * (-1);
            autGiro.TimbreML = autGiro.AutorizacionDetail.Sum(x => x.TimbreML) * (-1);
            autGiro.AnticiposML = autGiro.AutorizacionDetail.Sum(x => x.AnticiposML) * (-1);
            autGiro.OtrosDtosML = autGiro.AutorizacionDetail.Sum(x => x.OtrosDtosML) * (-1);
            autGiro.VrNetoML = autGiro.AutorizacionDetail.Sum(x => x.VrNetoML);

            autGiro.VrBrutoME = autGiro.AutorizacionDetail.Sum(x => x.VrBrutoME);
            autGiro.IvaME = autGiro.AutorizacionDetail.Sum(x => x.IvaME);
            autGiro.ReteIvaME = autGiro.AutorizacionDetail.Sum(x => x.ReteIvaME) * (-1);
            autGiro.ReteFuenteME = autGiro.AutorizacionDetail.Sum(x => x.ReteFuenteME) * (-1);
            autGiro.ReteIcaME = autGiro.AutorizacionDetail.Sum(x => x.ReteIcaME) * (-1);
            autGiro.TimbreME = autGiro.AutorizacionDetail.Sum(x => x.TimbreME) * (-1);
            autGiro.AnticiposME = autGiro.AutorizacionDetail.Sum(x => x.AnticiposME) * (-1);
            autGiro.OtrosDtosME = autGiro.AutorizacionDetail.Sum(x => x.OtrosDtosME) * (-1);
            autGiro.VrNetoME = autGiro.AutorizacionDetail.Sum(x => x.VrNetoME);
            #endregion
            List<DTO_ReportAutorizacionDeGiro> autGiroList = new List<DTO_ReportAutorizacionDeGiro>();
            autGiroList.Add(autGiro);
            #endregion

            #region Report field list
            ArrayList fieldList = ColumnsInfo.AutorGiroFields;
            if (multiMoneda && !fieldList.Contains("ValorME"))
                fieldList.Add("ValorME");
            #endregion

            AutorizacionDeGiroReport report = new AutorizacionDeGiroReport(AppReports.cpAutorizacionDeGiro, autGiroList, fieldList, multiMoneda, autGiro.EstadoInd, _bc);
            if (show)
                report.ShowPreview();
        } 
        #endregion
    }
}
