using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Configuration;
using SentenceTransformer;
using System.Collections;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalResumenActaEjecucion : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private int _documentID;
        private string _unboundPrefix = "Unbound_";
        //Variables de data
        DTO_SolicitudTrabajo _proyecto = new DTO_SolicitudTrabajo();
        List<DTO_pyProyectoTareaCliente> _entregables = new List<DTO_pyProyectoTareaCliente>();
        List<DTO_faFacturacionFooter> _detalleFactura = new List<DTO_faFacturacionFooter>();
        #endregion
        
        public decimal ValorAmortizaActual
        {
            get { return Convert.ToDecimal(this.txtPorAmortiza4.EditValue, CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bodegaIni">Bodega para consultar las existencias</param>
        public ModalResumenActaEjecucion(DTO_SolicitudTrabajo proyecto, List<DTO_pyProyectoTareaCliente> entregables, List<DTO_faFacturacionFooter> detalleFactura)
        {
            this.InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._proyecto = proyecto;
                this._entregables = entregables;
                this._detalleFactura = detalleFactura;
                FormProvider.LoadResources(this, this._documentID);
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalResumenActaEjecucion.cs", "ModalResumenActaEjecucion: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.ActaEntregaPreFactVenta;
            #region Inicializa Controles        
            #endregion            

        }

        /// <summary>
        /// Carga las solicitudes de proveedores
        /// </summary>
        private void LoadData()
        {
            try
            {
                if (this._proyecto != null)
                {
                    #region Valores iniciales
                    decimal vlrFacturadoProy = 0;
                    decimal vlrPreFacturadoProy = 0;
                    decimal vlrRteGarantiaFacturado = 0;
                    decimal vlrAmortizaFacturado = 0;
                    decimal vlrRteGarantiaPreFacturado = 0;
                    decimal vlrAmortizaPreFacturado = 0;
                    string servicioCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ServicioCtaCobro);
                    string tipoFacturaCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_TipoFacturaCtaCobro);
                    this._proyecto.HeaderProyecto.PorcRteGarantia.Value = this._proyecto.HeaderProyecto.PorcRteGarantia.Value ?? 0;
                    decimal vlrProyTotal = Math.Round(this._proyecto.DetalleProyecto.Sum(x => x.CostoLocalCLI.Value.Value), 0);
                    decimal porcIVAProy = this._proyecto.HeaderProyecto.PorIVA.Value.HasValue?(this._proyecto.HeaderProyecto.PorIVA.Value.Value / 100) : 0;
                    bool rteGarantiaIvaIncluido = this._proyecto.HeaderProyecto.RteGarantiaIvaInd.Value.HasValue? this._proyecto.HeaderProyecto.RteGarantiaIvaInd.Value.Value :false;
                    decimal porcRteGarantiaContrato = this._proyecto.HeaderProyecto.PorcRteGarantia.Value.Value;
                    decimal vlrRteGarantiaContrato = Math.Round(vlrProyTotal * (porcRteGarantiaContrato / 100), 0);
                    vlrRteGarantiaContrato = !rteGarantiaIvaIncluido ? vlrRteGarantiaContrato : Math.Round(vlrRteGarantiaContrato + (vlrRteGarantiaContrato * porcIVAProy), 0);
                    
                    //Trae el proyecto cotizado
                    DTO_SolicitudTrabajo cotizacion = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.PreProyecto, string.Empty, null, this._proyecto.HeaderProyecto.DocSolicitud.Value, string.Empty, string.Empty, true, false, false, false);
                    decimal vlrCotizacion = cotizacion != null ? cotizacion.Detalle.Sum(x => x.CostoLocalCLI.Value.Value) : 0;

                    #endregion
                    #region Trae las facturas de venta del proyecto y datos relacionados
                    DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                    filter.DatoAdd4.Value = this._proyecto.HeaderProyecto.NumeroDoc.Value.ToString();
                    filter.ProyectoID.Value = this._proyecto.DocCtrl.ProyectoID.Value;
                    filter.DocumentoID.Value = AppDocuments.FacturaVenta;
                    List<DTO_glMovimientoDeta> mvtos = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, false);//Facturadas
                    mvtos.AddRange(this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, true));//Prefacturadas
                    decimal vlrAnticipoCtaCobro = mvtos.FindAll(x => x.FacturaTipoID.Value == tipoFacturaCtaCobro && x.ServicioID.Value == servicioCtaCobro && !x.DatoAdd5.Value.Equals("INV")).Sum(x => x.Valor1LOC.Value.Value);
                    decimal porcAnticipoCtaCobro = vlrProyTotal != 0 ? (vlrAnticipoCtaCobro) * 100 / vlrProyTotal : 0;
                    mvtos = mvtos.FindAll(x => x.DocSoporte.Value.HasValue && !x.DatoAdd5.Value.Equals("INV"));//Filtra las tareas relacionadas
                    
                    //Recorre las Facturas de Venta Aprobadas
                    foreach (DTO_glMovimientoDeta mvto in mvtos.FindAll(x=>x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado))
                    {                        
                        if (this._proyecto.DetalleProyecto.Exists(x => x.Consecutivo.Value == mvto.DocSoporte.Value))
                            vlrFacturadoProy += mvto.Valor1LOC.Value.Value;
                    }

                    //Recorre las Facturas de Venta Sin Aprobar
                    foreach (DTO_glMovimientoDeta mvto in mvtos.FindAll(x => x.EstadoDocCtrl.Value != (Int16)EstadoDocControl.Aprobado))
                    {
                        if (this._proyecto.DetalleProyecto.Exists(x => x.Consecutivo.Value == mvto.DocSoporte.Value))
                            vlrPreFacturadoProy += mvto.Valor1LOC.Value.Value;
                    }

                    //Obtiene valores adicionales(RteGarantia-Amortizaciones)
                    List<int> facturas = mvtos.Where(x=>x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado).Select(x => x.NumeroDoc.Value.Value).Distinct().ToList();
                    List<int> preFacturas = mvtos.Where(x => x.EstadoDocCtrl.Value != (Int16)EstadoDocControl.Aprobado).Select(x => x.NumeroDoc.Value.Value).Distinct().ToList();
                    foreach (int numDoc in facturas)
                    {
                        DTO_faFacturaDocu docu = this._bc.AdministrationModel.faFacturaDocu_Get(numDoc);
                        docu.Retencion4.Value = docu.Retencion4.Value ?? 0;
                        docu.Retencion10.Value = docu.Retencion10.Value ?? 0;
                        vlrRteGarantiaFacturado += (docu.Retencion4.Value.Value);
                        vlrAmortizaFacturado += (docu.Retencion10.Value.Value);
                    }
                    foreach (int numDoc in preFacturas)
                    {
                        DTO_faFacturaDocu docu = this._bc.AdministrationModel.faFacturaDocu_Get(numDoc);
                        docu.Retencion4.Value = docu.Retencion4.Value ?? 0;
                        docu.Retencion10.Value = docu.Retencion10.Value ?? 0;
                        vlrRteGarantiaPreFacturado += (docu.Retencion4.Value.Value);
                        vlrAmortizaPreFacturado += (docu.Retencion10.Value.Value);
                    }
                    #endregion
                    #region Calculo de valores
                    //Facturas
                    vlrFacturadoProy = Math.Round(vlrFacturadoProy, 0);
                    vlrPreFacturadoProy = Math.Round(vlrPreFacturadoProy, 0);
                    decimal porcFacturado = vlrProyTotal != 0 ? vlrFacturadoProy * 100 / vlrProyTotal : 0;  
                    decimal porcPreFacturado = vlrProyTotal != 0 ? vlrPreFacturadoProy * 100 / vlrProyTotal : 0;
                    decimal porcRteGarantiaFacturado = vlrProyTotal != 0 ? (vlrRteGarantiaFacturado * 100) / vlrProyTotal : 0;
                    decimal porcAmortizaFacturado = vlrProyTotal != 0 ? (vlrAmortizaFacturado) * 100 / vlrProyTotal : 0;
                    decimal porcRteGarantiaPreFacturado = vlrProyTotal != 0 ? (vlrRteGarantiaPreFacturado * 100) / vlrProyTotal : 0;
                    decimal porcAmortizaPreFacturado = vlrProyTotal != 0 ? (vlrAmortizaPreFacturado) * 100 / vlrProyTotal : 0;
                    //Presente Acta
                    decimal vlrPresenteActa = this._detalleFactura.Sum(x => x.Movimiento.Valor1LOC.Value.Value);
                    decimal porcPresenteActa = vlrProyTotal != 0 ? (vlrPresenteActa) * 100 / vlrProyTotal : 0;
                    decimal vlrRteGarantiaPresenteAct = !rteGarantiaIvaIncluido ? Math.Round(vlrPresenteActa * (porcRteGarantiaContrato / 100), 0) : Math.Round((vlrPresenteActa * (porcRteGarantiaContrato / 100) + (vlrPresenteActa * (porcRteGarantiaContrato / 100) * porcIVAProy)), 0);
                    decimal porcRteGarantiaPresenteAct = vlrProyTotal != 0 ? vlrRteGarantiaPresenteAct * 100 / vlrProyTotal : 0;
                    decimal porcAmortizaPresenteAct = vlrProyTotal != 0 ? vlrPresenteActa * 100 / vlrProyTotal : 0;
                    decimal vlrAmortizaPresenteActa = vlrPresenteActa <= vlrProyTotal ? Math.Round(vlrAnticipoCtaCobro * (porcAmortizaPresenteAct / 100), 0) : 0;
                    //Nuevo Saldo Total
                    decimal vlrSaldoContrato = vlrProyTotal - vlrFacturadoProy - vlrPreFacturadoProy - vlrPresenteActa;
                    decimal porcSaldoContrato = 100 - porcFacturado - porcPreFacturado - porcPresenteActa;
                    decimal vlrSaldoRteGarantia = vlrRteGarantiaContrato - vlrRteGarantiaFacturado - vlrRteGarantiaPreFacturado - vlrRteGarantiaPresenteAct;
                    decimal porcSaldoRteGarantia = porcRteGarantiaContrato - porcRteGarantiaFacturado - porcRteGarantiaPreFacturado - porcRteGarantiaPresenteAct;
                    decimal vlrSaldoAmortizacion = vlrAnticipoCtaCobro - vlrAmortizaFacturado - vlrAmortizaPreFacturado - vlrAmortizaPresenteActa;
                    decimal porcSaldoAmortizacion = porcAnticipoCtaCobro - porcAmortizaFacturado - porcAmortizaPreFacturado - porcAmortizaPresenteAct;
                    #endregion
                    #region Asigna Valores 
                    //Valores iniciales
                    this.txtVlrProyectoTot.EditValue = vlrProyTotal;
                    this.txtVlrContratoInicial.EditValue = vlrCotizacion;
                    this.txtVlrAdicionales.EditValue = (vlrProyTotal - vlrCotizacion) < 0 ? 0 : vlrProyTotal - vlrCotizacion;
                    //Contrato
                    this.txtVlrContrato1.EditValue = vlrProyTotal;
                    this.txtPorContrato1.EditValue = 100;
                    this.txtVlrRetegarantia1.EditValue = vlrRteGarantiaContrato;
                    this.txtPorRtegarantia1.EditValue = porcRteGarantiaContrato;
                    this.txtVlrAmortiza1.EditValue = vlrAnticipoCtaCobro;
                    this.txtPorAmortiza1.EditValue = porcAnticipoCtaCobro;
                    //Facturado
                    this.txtVlrContrato2.EditValue = vlrFacturadoProy;
                    this.txtPorContrato2.EditValue = porcFacturado;
                    this.txtVlrRetegarantia2.EditValue = vlrRteGarantiaFacturado;
                    this.txtPorRtegarantia2.EditValue = porcRteGarantiaFacturado;
                    this.txtVlrAmortiza2.EditValue = vlrAmortizaFacturado;
                    this.txtPorAmortiza2.EditValue = porcAmortizaFacturado;
                    //Prefacturado
                    this.txtVlrContrato3.EditValue = vlrPreFacturadoProy;
                    this.txtPorContrato3.EditValue = porcPreFacturado;
                    this.txtVlrRetegarantia3.EditValue = vlrRteGarantiaPreFacturado;
                    this.txtPorRtegarantia3.EditValue = porcRteGarantiaPreFacturado;
                    this.txtVlrAmortiza3.EditValue = vlrAmortizaPreFacturado;
                    this.txtPorAmortiza3.EditValue = porcAmortizaPreFacturado;
                    //PResente Acta
                    this.txtVlrContrato4.EditValue = vlrPresenteActa;
                    this.txtPorContrato4.EditValue = porcPresenteActa;
                    this.txtVlrRetegarantia4.EditValue = vlrRteGarantiaPresenteAct;// vlrSaldoRteGarantia >= 0 ? vlrRteGarantiaPresenteAct : vlrRteGarantiaContrato - vlrRteGarantiaFacturado - vlrRteGarantiaPreFacturado;
                    this.txtPorRtegarantia4.EditValue = porcRteGarantiaPresenteAct;// porcSaldoRteGarantia >= 0 ? porcRteGarantiaPresenteAct : porcRteGarantiaContrato - porcRteGarantiaFacturado - porcRteGarantiaPreFacturado; 
                    this.txtVlrAmortiza4.EditValue = vlrAmortizaPresenteActa;// vlrSaldoAmortizacion >= 0 ? vlrAmortizaPresenteActa : vlrAnticipoCtaCobro- vlrAmortizaFacturado - vlrAmortizaPreFacturado;
                    this.txtPorAmortiza4.EditValue = porcAmortizaPresenteAct;// porcSaldoAmortizacion >= 0 ? porcPresenteActa : porcAnticipoCtaCobro - porcAmortizaFacturado - porcAmortizaPreFacturado; 
                    //Total
                    this.txtVlrContrato5.EditValue = vlrSaldoContrato;
                    this.txtPorContrato5.EditValue = porcSaldoContrato;
                    this.txtVlrRetegarantia5.EditValue = vlrSaldoRteGarantia;// vlrSaldoRteGarantia >= 0? vlrSaldoRteGarantia : 0;
                    this.txtPorRtegarantia5.EditValue = porcSaldoRteGarantia;// porcSaldoRteGarantia >= 0 ? porcSaldoRteGarantia : 0;
                    this.txtVlrAmortiza5.EditValue = vlrSaldoAmortizacion;// vlrSaldoAmortizacion >= 0 ? vlrSaldoAmortizacion: 0;
                    this.txtPorAmortiza5.EditValue = porcSaldoAmortizacion >= 0 ? porcSaldoAmortizacion : 0;

                    if (this._proyecto.HeaderProyecto.RteGarantiaIvaInd.Value.HasValue && this._proyecto.HeaderProyecto.RteGarantiaIvaInd.Value.Value)
                        this.lblIva.Text = "*Valores CON IVA Incluido";
                    else
                        this.lblIva.Text = "*Valores SIN IVA Incluido";
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalResumenActaEjecucion.cs", "LoadData"));
            }
        }
        #endregion

        #region Eventos Controles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.Close();
        }   
        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencias_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {             
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                    else
                    {
                        DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                        pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                e.Value = pi.GetValue(dtoM.Movimiento, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                        }
                        else
                        {
                            fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                    e.Value = fi.GetValue(dtoM.Movimiento);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                            }
                        }
                    }
                }
            }

            if (e.IsSetData)
            {               
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                }
            }
        }

        #endregion
    }
}
