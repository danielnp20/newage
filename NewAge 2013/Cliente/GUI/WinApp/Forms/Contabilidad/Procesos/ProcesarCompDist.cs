using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Threading;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ProcesarCompDist : ProcessForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resPreliminar = null;
        private List<DTO_TxResult> _resAprobar = null;
        private List<DTO_ComprobanteAprobacion> _aprob = null;

        //Datos
        private string libroFunc = string.Empty;
        private List<DTO_coCompDistribuyeTabla> _data;

        //Recursos
        private string _cuentaRsx = string.Empty;
        private string _centroCostoRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string unboundPrefix = "Unbound_";

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndPreliminar();
        public EndPreliminar endPreliminar;
        public void EndPreliminarMethod()
        {
            this.btnPreliminar.Enabled = true;
            this.ControlBox = true;
            if (_resPreliminar != null && _resPreliminar.Result == ResultValue.OK && _aprob != null)
            {
                this.btnProcesar.Enabled = true;
                foreach (DTO_ComprobanteAprobacion ap in _aprob)
                    ap.DocumentoID.Value = this.documentID;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesar;
        public void EndProcesarMethod()
        {
            this.btnPreliminar.Enabled = true;
            this.ControlBox = true;
            if (_resAprobar != null)
            {
                bool isOK = true;
                foreach (DTO_TxResult result in _resAprobar)
                {
                    if (result.Details != null && result.Details.Count > 0)
                        isOK = false;
                }
                if (isOK)
                    this.btnProcesar.Enabled = false;
            }
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ProcesarCompDist() : base() { }

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Cuenta Origen
                GridColumn cuentaOrig = new GridColumn();
                cuentaOrig.FieldName = this.unboundPrefix + "CuentaORIG";
                cuentaOrig.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaORIG");
                cuentaOrig.UnboundType = UnboundColumnType.String;
                cuentaOrig.VisibleIndex = 1;
                cuentaOrig.Width = 110;
                cuentaOrig.OptionsColumn.AllowEdit = false;
                this.gvOrigen.Columns.Add(cuentaOrig);

                //Centro de costo Origen
                GridColumn ctoCostoOrig = new GridColumn();
                ctoCostoOrig.FieldName = this.unboundPrefix + "CtoCostoORIG";
                ctoCostoOrig.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CtoCostoORIG");
                ctoCostoOrig.UnboundType = UnboundColumnType.String;
                ctoCostoOrig.VisibleIndex = 2;
                ctoCostoOrig.Width = 110;
                ctoCostoOrig.OptionsColumn.AllowEdit = false;
                this.gvOrigen.Columns.Add(ctoCostoOrig);

                //Proyecto Origen
                GridColumn proyectoOrig = new GridColumn();
                proyectoOrig.FieldName = this.unboundPrefix + "ProyectoORIG";
                proyectoOrig.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoORIG");
                proyectoOrig.UnboundType = UnboundColumnType.String;
                proyectoOrig.VisibleIndex = 3;
                proyectoOrig.Width = 110;
                proyectoOrig.OptionsColumn.AllowEdit = false;
                this.gvOrigen.Columns.Add(proyectoOrig);

                //Orden
                GridColumn orden = new GridColumn();
                orden.FieldName = this.unboundPrefix + "Orden";
                orden.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Orden");
                orden.UnboundType = UnboundColumnType.Integer;
                orden.VisibleIndex = 4;
                orden.Width = 50;
                orden.OptionsColumn.AllowEdit = false;
                this.gvOrigen.Columns.Add(orden);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarCompDist.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la info de las grillas
        /// </summary>
        private void LoadData()
        {
            try
            {
                //Trae los datos
                this._data = _bc.AdministrationModel.ComprobanteDistribucion_GetForProcess();

                this.gcOrigen.DataSource = this._data;
                this.gcOrigen.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarCompDist.cs", "LoadData"));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.ComprobanteDistrib;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                this.endPreliminar = new EndPreliminar(EndPreliminarMethod);
                this.endProcesar = new EndProcesar(EndProcesarMethod);

                //Inicia los calendarios
                string periodo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                string p14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);

                if (p14 == "1")
                {
                    _bc.InitPeriodUC(this.periodBegin, 2);
                    _bc.InitPeriodUC(this.periodEnd, 2);
                }
                else
                {
                    _bc.InitPeriodUC(this.periodBegin, 1);
                    _bc.InitPeriodUC(this.periodEnd, 1);
                }

                this.periodEnd.EnabledControl = false;

                this.periodBegin.DateTime = Convert.ToDateTime(periodo);
                this.periodEnd.DateTime = Convert.ToDateTime(periodo);

                this.periodBegin.MinValue = new DateTime(this.periodBegin.DateTime.Year, 1, 1);
                this.periodBegin.MaxValue = this.periodBegin.DateTime;

                this.libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                //Carga la info
                this.AddGridCols();
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarCompDist.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAjuste_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnPreliminar.Enabled = false;
            new Thread(PreliminarThread).Start();
        }

        /// <summary>
        /// Boton de procesar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnPreliminar.Enabled = false;
            this.btnProcesar.Enabled = false;
            new Thread(ProcesarThread).Start();
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
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
                }
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Ajuste en Cambio
        /// </summary>
        private void PreliminarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> result = this._bc.AdministrationModel.ComprobanteDistribucion_GenerarPreliminar(this.documentID, this._actFlujo.ID.Value, this._data, this.periodBegin.DateTime, this.periodEnd.DateTime, libroFunc);

                this._aprob = result.Item2;
                this._resPreliminar = result.Item1;

                this.Invoke(this.endPreliminar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._resPreliminar);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarCompDist.cs", "PreliminarThread"));
                this.StopProgressBarThread();
            }
        }

        /// <summary>
        /// Hilo de procesar cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
                this._resAprobar = _bc.AdministrationModel.Proceso_ProcesarBalancePreliminar(this.documentID, this._actFlujo.ID.Value, this._aprob, dt, libroFunc);

                List<DTO_TxResult> resNOK = new List<DTO_TxResult>();
                foreach (DTO_TxResult result in _resAprobar)
                {
                    if (result.Result == ResultValue.NOK || (result.Details != null && result.Details.Count > 0))
                        resNOK.Add(result);
                }

                this.Invoke(this.endProcesar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(resNOK);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarCompDist.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
