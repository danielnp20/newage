using System;
using System.Collections.Generic;
using System.Linq;
using NewAge.DTO.Negocio;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors.Repository;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Clase para obtener los recursos de tablas (combos)
    /// </summary>
    public static class TablesResources
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private static BaseController _bc = BaseController.GetInstance();

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Llena un combo con los recursos deseados
        /// </summary>
        /// <param name="container">Combo</param>
        /// <param name="tipoEnum">Enumeracion para llenar el combo</param>
        public static void GetTableResources(ComboBoxEx container, Type tipoEnum)
        {
            #region Tipos de Viaje

            if (tipoEnum == typeof(TipoViaje))
            {
                // Moneda local
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoViaje.Nacional).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoViajeNacional)
                ));
                // Moneda extranjera
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoViaje.Exterior).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoViajeExterior)
                ));
            }

            #endregion
            #region Estados Factura Cuentas X Pagar
            if (tipoEnum == typeof(TipoMovimientoCXP))
            {
                // Radicado
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMovimientoCXP.Radicado).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Radicar)
                ));
                // Devuelto
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMovimientoCXP.Devuelto).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Devolver)
                ));
                // Cerrado
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMovimientoCXP.Cerrado).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateCancelado)
                ));
            }
            #endregion
            #region Modulos
            if (tipoEnum == typeof(ModulesPrefix))
            {

                long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.aplModulo, null, null, true);
                List<DTO_aplModulo> mods = _bc.AdministrationModel.aplModulo_GetByVisible(1, true).ToList();

                foreach (DTO_aplModulo mod in mods)
                {
                    string modID = mod.ModuloID.Value.ToLower();
                    string rsx = string.Empty;
                    #region Asignacion de recurso
                    switch (modID)
                    {
                        case "ac":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_AC);
                            break;
                        case "cc":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_CC);
                            break;
                        case "cf":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_CC);
                            break;
                        case "co":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_CO);
                            break;
                        case "cp":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_CP);
                            break;
                        case "di":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_DI);
                            break;
                        case "fa":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_FA);
                            break;
                        case "in":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_IN);
                            break;
                        case "no":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_NO);
                            break;
                        case "op":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_OP);
                            break;
                        case "oc":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_OC);
                            break;
                        case "pl":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_PL);
                            break;
                        case "pr":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_PR);
                            break;
                        case "ts":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_TS);
                            break;
                        case "rh":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_RH);
                            break;
                        case "py":
                            rsx = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_PY);
                            break;
                    }
                    #endregion

                    if (modID == "cf")
                        modID = "cc";

                    container.Items.Add(new ComboBoxItem(modID, rsx));
                }
            }
            #endregion
            #region Nomina
            #region Periodos Pago
            if (tipoEnum == typeof(PeriodoPago))
            {
                // Primera Quincena
                container.Items.Add(new ComboBoxItem(
                    ((int)PeriodoPago.PrimeraQuincena).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Primera_Quincena)
                ));
                // Segunda Quincena
                container.Items.Add(new ComboBoxItem(
                    ((int)PeriodoPago.SegundaQuincena).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Segunda_Quincena)
                ));
                // Ambas Quincenas
                container.Items.Add(new ComboBoxItem(
                    ((int)PeriodoPago.AmbasQuincenas).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Ambas_Quincena)
                ));
            }
            #endregion
            #region UltimaNomina
            if (tipoEnum == typeof(UltimaNomina))
            {
                // Primera Quincena
                container.Items.Add(new ComboBoxItem(
                    ((int)UltimaNomina.UltimaNomina1).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.UltimaNomina1)
                ));
                // Segunda Quincena
                container.Items.Add(new ComboBoxItem(
                    ((int)UltimaNomina.UltimaNomina2).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.UltimaNomina2)
                ));
            }
            #endregion
            #region Tipo Concepto Planilla Diaria de Trabajo

            if (tipoEnum == typeof(TipoConceptoPlanillaDiaria))
            {
                // TRABAJO
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoConceptoPlanillaDiaria.T).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, TipoConceptoPlanillaDiaria.T.ToString())
                ));
                // DESCANSO
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoConceptoPlanillaDiaria.D).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, TipoConceptoPlanillaDiaria.D.ToString())
                ));
                // VACACIONES
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoConceptoPlanillaDiaria.V).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, TipoConceptoPlanillaDiaria.V.ToString())
                ));
                // INCAPACIDAD
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoConceptoPlanillaDiaria.I).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, TipoConceptoPlanillaDiaria.I.ToString())
                ));
                // Primera Quincena
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoConceptoPlanillaDiaria.S).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, TipoConceptoPlanillaDiaria.S.ToString())
                ));
                // CAPACITACION
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoConceptoPlanillaDiaria.C).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, TipoConceptoPlanillaDiaria.C.ToString())
                ));
            }

            #endregion
            #endregion
            #region Tipo de movimientos para los traslados
            if (tipoEnum == typeof(TipoMvtoTraslado))
            {
                // Línea Recta
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMvtoTraslado.Traslado).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Traslado)
                ));
                // Saldos Decrecientes
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMvtoTraslado.Mantenimiento).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Mantenimiento)
                ));
                // Unidades de Produccion
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMvtoTraslado.AsignacionResponsable).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AsignacionDeResponsable)
                ));
            }

            #endregion
            #region Tipo Depreciacion.
            if (tipoEnum == typeof(TipoDepreciacion))
            {
                // Línea Recta
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoDepreciacion.LineaRecta).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoDepreciacionLineaRecta)
                ));
                // Saldos Decrecientes
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoDepreciacion.SaldosDecrecientes).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoDepreciacionSaldosDecrecientes)
                ));
                // Unidades de Produccion
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoDepreciacion.UnidadesDeProduccion).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoDepreciacionUnidadesProduccion)
                ));
            }
            #endregion
            #region Tipos de monedas
            if (tipoEnum == typeof(TipoMoneda))
            {
                // Moneda local
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMoneda.Local).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)
                ));
                // Moneda extranjera
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMoneda.Foreign).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign)
                ));
                // Ambas
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMoneda.Both).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth)
                ));
            }
            #endregion
            #region Tipos de monedas (Solo local y extranjera)
            if (tipoEnum == typeof(TipoMoneda_LocExt))
            {
                // Moneda local
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMoneda.Local).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)
                ));
                // Moneda extranjera
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoMoneda.Foreign).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign)
                ));
            }
            #endregion
            #region Niveles de prioridad
            if (tipoEnum == typeof(Prioridad))
            {
                // High
                container.Items.Add(new ComboBoxItem(
                    ((int)Prioridad.High).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_PriorityHigh)
                ));
                // Medium
                container.Items.Add(new ComboBoxItem(
                    ((int)Prioridad.Medium).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_PriorityMedium)
                ));
                // Low
                container.Items.Add(new ComboBoxItem(
                    ((int)Prioridad.Low).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_PriorityLow)
                ));
            }
            #endregion
            #region Estados Inv
            if (tipoEnum == typeof(EstadoInv))
            {
                // Todos
                container.Items.Add(new ComboBoxItem(
                    ((int)EstadoInv.SinCosto).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvCosto0)
                ));
                // Nuevo
                container.Items.Add(new ComboBoxItem(
                    ((int)EstadoInv.Activo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvNuevo)
                ));
                // Estado 2
                container.Items.Add(new ComboBoxItem(
                    ((int)EstadoInv.Arrendado).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvEstado2)
                ));
                // Estado 3
                container.Items.Add(new ComboBoxItem(
                    ((int)EstadoInv.Mantenimiento).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvEstado3)
                ));
                // Estado 4
                container.Items.Add(new ComboBoxItem(
                    ((int)EstadoInv.Retirado).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstadoInvEstado4)
                ));
            }
            #endregion
            #region Tipo Transporte
            if (tipoEnum == typeof(TipoTransporte))
            {
                // Aereo
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoTransporte.Aereo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteAereo)
                ));
                // Maritimo
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoTransporte.Maritimo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteMaritimo)
                ));
                // Terrestre
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoTransporte.Terrestre).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteTerrestre)
                ));
                // Trafico Postal
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoTransporte.TraficoPostal).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteTraficoPostal)
                ));
            }
            #endregion
            #region Años
            if (tipoEnum == typeof(AñosSaldos))
            {
                // 2000
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmil)
                ));
                // 2001
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Uno).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosMilUno)
                ));
                // 2002
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Dos).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosMilDos)
                ));
                // 2003
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Tres).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosMilTres)
                ));
                // 2004
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Cuatro).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosMilCuatro)
                ));
                // 2005
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Cinco).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilCinco)
                ));
                // 2006
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Seis).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilSeis)
                ));
                // 2007
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Siete).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilSiete)
                ));
                // 2008
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Ocho).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilOcho)
                ));
                // 2009
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Nueve).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilNueve)
                ));
                // 2010
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Diez).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilDiez)
                ));
                // 2011
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Once).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilOnce)
                ));
                // 2012
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Doce).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilDoce)
                ));
                // 2013
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Trece).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilTrece)
                ));
                // 2014
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Catorce).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilCatorce)
                ));
                // 2015
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Quince).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilQuince)
                ));
                // 2016
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_DieciSeis).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilDieciseis)
                ));
                // 2017
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_DieciSiete).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilDiecisiete)
                ));
                // 2018
                container.Items.Add(new ComboBoxItem(
                    ((int)AñosSaldos.Dos_Mil_Dieciocho).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_AñosDosmilDieciocho)
                ));
            }
            #endregion
            #region coSaldoControl
            if (tipoEnum == typeof(coSaldoControl))
            {
                // Cuenta
                container.Items.Add(new ComboBoxItem(
                    ((int)coSaldoControl.Cuenta).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Cuenta)
                ));
                // Doc_Interno
                container.Items.Add(new ComboBoxItem(
                    ((int)coSaldoControl.Doc_Interno).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DocInterno)
                ));
                // Doc Externo
                container.Items.Add(new ComboBoxItem(
                    ((int)coSaldoControl.Doc_Externo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DocExterno)
                ));
                //Componente tercero
                container.Items.Add(new ComboBoxItem(
                ((int)coSaldoControl.Componente_Tercero).ToString(),
                _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ComponenteTercero)
                ));
                //Componente Activo
                container.Items.Add(new ComboBoxItem(
                ((int)coSaldoControl.Componente_Activo).ToString(),
                _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ComponenteActivo)
                ));
                //Componente Documento
                container.Items.Add(new ComboBoxItem(
                ((int)coSaldoControl.Componente_Documento).ToString(),
                _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ComponenteDocumento)
                ));
                //Inventario
                container.Items.Add(new ComboBoxItem(
                ((int)coSaldoControl.Inventario).ToString(),
                _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Inventario)
                ));
            }
            container.DisplayMember = "Text";
            #endregion
            #region Destino de solicitud
            if (tipoEnum == typeof(Destino))
            {
                // para Orden de Compra
                container.Items.Add(new ComboBoxItem(
                    ((int)Destino.OrdenCompra).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoOrdenCompra)
                ));
                // para Contrato
                container.Items.Add(new ComboBoxItem(
                    ((int)Destino.Contrato).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoContrato)
                ));
            }
            #endregion
            #region Planeacion
            #region PresupuestarCostos
            if (tipoEnum == typeof(PresupuestarCostos1))
            {
                // para Centros De Costos
                container.Items.Add(new ComboBoxItem(
                    ((int)PresupuestarCostos1.CentrosDeCostos).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CentroCosto)
                ));
                // para Proyectos
                container.Items.Add(new ComboBoxItem(
                    ((int)PresupuestarCostos1.Proyectos).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Proyectos)
                ));
            }
            #endregion
            #region PresupuestarCostos2
            if (tipoEnum == typeof(PresupuestarCostos2))
            {
                // para Linea Presupuestal
                container.Items.Add(new ComboBoxItem(
                    ((int)PresupuestarCostos2.LineaPresupuestal).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_LineaPresu)
                ));
                // para Concepto Cargo
                container.Items.Add(new ComboBoxItem(
                    ((int)PresupuestarCostos2.ConceptoCargo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ConcepCargo)
                ));
            }
            #endregion
            #region Mda
            if (tipoEnum == typeof(Mda))
            {
                // para Linea Presupuestal
                container.Items.Add(new ComboBoxItem(
                    ((int)Mda.Local).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Local)
                ));
                // para Concepto Cargo
                container.Items.Add(new ComboBoxItem(
                    ((int)Mda.Extranjera).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Extranjero)
                ));
            }
            #endregion
            #endregion
            #region Cartera
            #region Sector Cartera
            if (tipoEnum == typeof(SectorCartera))
            {
                container.Items.Add(new ComboBoxItem(
                    ((int)SectorCartera.Solidario).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Solidario)
                    ));

                container.Items.Add(new ComboBoxItem(
                    ((int)SectorCartera.Financiero).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Financiero)
                    ));

                container.Items.Add(new ComboBoxItem(
                    ((int)SectorCartera.Bancario).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Bancario)
                    ));
            }
            #endregion
            #region Tipo Credito
            if (tipoEnum == typeof(TipoCredito))
            {
                //Cartera Nueva
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoCredito.Nuevo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_Nueva)
                    ));

                //Cartera Refinanciada
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoCredito.Refinanciado).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_Refinanciada)
                    ));
            } 
            #endregion
            #region Tasa Venta
            if (tipoEnum == typeof(TasaVenta))
            {
                container.Items.Add(new ComboBoxItem(
                    ((int)TasaVenta.EfectivaAnual).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.EfectivaAnual)
                    ));

                container.Items.Add(new ComboBoxItem(
                    ((int)TasaVenta.NominaMensual).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.NominaMensual)
                    ));
            }
            #endregion
            #region Tipo Interes
            if (tipoEnum == typeof(TipoInteres))
            {
                //Cartera Nueva
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoInteres.Efectivo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Efectivo)
                    ));

                //Cartera Refinanciada
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoInteres.Nominal).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Nominal)
                    ));
            } 
            #endregion
            #region Tipo Incorporación Cartera
            if (tipoEnum == typeof(TipoIncorporaCartera))
            {
                //Cartera Nueva
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoIncorporaCartera.Afiliaciones).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Incorpora_Liquida)
                    ));

                //Cartera Refinanciada
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoIncorporaCartera.Desafiliaciones).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Incorpora_Previa)
                    ));
            }
            #endregion
            #endregion
            #region General
            if (tipoEnum == typeof(ExportFormatType))
            {
                // Pdf
                container.Items.Add(new ComboBoxItem(
                    ((int)ExportFormatType.pdf).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Pdf)
                ));
                // Xls
                container.Items.Add(new ComboBoxItem(
                    ((int)ExportFormatType.xls).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Xls)
                ));
                // Xlsx
                container.Items.Add(new ComboBoxItem(
                    ((int)ExportFormatType.xlsx).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Xlsx)
                ));
                // Diseñador
                container.Items.Add(new ComboBoxItem(
                    ((int)ExportFormatType.Diseñador).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Diseñador)
                ));
            }
            #endregion
            #region Base Calculo
            if (tipoEnum == typeof(BaseCalculo))
            {
                // Pozo
                container.Items.Add(new ComboBoxItem(
                    ((int)BaseCalculo.Pozo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Pozo)
                ));
                // Campo
                container.Items.Add(new ComboBoxItem(
                    ((int)BaseCalculo.Campo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Campo)
                ));
            }

            #endregion
            #region Tipo CalculoIFRS
            if (tipoEnum == typeof(TipoCalculoIFRS))
            {
                // ReservasProbables
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoCalculoIFRS.ReservasProbadas).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ReservasProbadas)
                ));
                // ReservasProbadas
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoCalculoIFRS.ReservasProbables).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ReservasProbables)
                ));
            }

            #endregion
            #region Control IVA
            if (tipoEnum == typeof(TipoIVA))
            {
                // ReservasProbables
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoIVA.MayorValorCashCall).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoIVAMayorValor)
                ));
                // ReservasProbadas
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoIVA.LineaIndependiente).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoIVALineaIndependiente)
                ));
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoIVA.DistribuidoPorLinea).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoIVADistribuidorPorLinea)
                ));
            }

            #endregion
            #region Contabilidad
            if (tipoEnum == typeof(ContabilizaIVA))
            {
          
                container.Items.Add(new ComboBoxItem(
                    ((int)ContabilizaIVA.CuentaGeneral).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.CuentaGen)
                ));
                container.Items.Add(new ComboBoxItem(
                    ((int)ContabilizaIVA.CuentaCosto).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.CuentaCosto)
                ));
                container.Items.Add(new ComboBoxItem(
                    ((int)ContabilizaIVA.CuentaEspecial).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.CuentaEspecial)
                ));
            }
            if (tipoEnum == typeof(ImpuestoLoc))
            {
                container.Items.Add(new ComboBoxItem(
                    ((int)ImpuestoLoc.PorConceptoCargo).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.PorConceptoCargo)
                ));
                container.Items.Add(new ComboBoxItem(
                    ((int)ImpuestoLoc.PorActividadEconomica).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.PorActividadEconomica)
                ));
            }
            #endregion
            #region TipoSolicitud
            if (tipoEnum == typeof(TipoSolicitud))
            {
                // Cotizacion
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoSolicitud.Cotizacion).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Cotizacion)
                ));
                // Licitacion
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoSolicitud.Licitacion).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Licitacion)
                ));
                // Garantia
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoSolicitud.Garantia).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Garantia)
                ));
                // Interna
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoSolicitud.Interna).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Interna)
                ));
                // Otra
                container.Items.Add(new ComboBoxItem(
                    ((int)TipoSolicitud.Otra).ToString(),
                    _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Otra)
                ));
                // Campo
            }
            #endregion

            container.SelectedIndex = 0;
        }

        /// <summary>
        /// Llena un combo con los recursos deseados
        /// </summary>
        /// <param name="container">Combo</param>
        /// <param name="docId">identificador documento</param>
        /// <param name="columnName">columna</param>
        /// <param name="llave">llave</param>
        public static void GetTableResources(object container, int docId, string columnName, string llave)
        {
            var datos = _bc.AdministrationModel.GetOptionsControl(docId, columnName, llave);
            if (container.GetType() == typeof(ComboBoxEx))
            {                
                foreach (var d in datos)
                {
                    ((ComboBoxEx)container).Items.Add(new ComboBoxItem(
                         d.Key,
                         d.Value
                    ));
                }
                ((ComboBoxEx)container).DisplayMember = "Text";
                ((ComboBoxEx)container).SelectedIndex = 0;
            }
            if (container.GetType() == typeof(RepositoryItemComboBox))
            {
                foreach (var d in datos)
                {
                    ((RepositoryItemComboBox)container).Items.Add(d.Key + "-" + d.Value);
          
                }
            }
            if (container.GetType() == typeof(RepositoryItemLookUpEdit))
            {
                ((RepositoryItemLookUpEdit)container).ValueMember = "Key";
                ((RepositoryItemLookUpEdit)container).DisplayMember = "Value";
                ((RepositoryItemLookUpEdit)container).DataSource = datos;
             
            }
        }

        #endregion
    }
}
