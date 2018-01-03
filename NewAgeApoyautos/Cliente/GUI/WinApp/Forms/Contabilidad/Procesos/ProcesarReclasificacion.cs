using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ProcesarReclasificacion : ProcessForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _result = null;

        //Datos
        private List<DTO_coReclasificaBalance> _data;
        private string tipoBalance;

        //Recursos
        private string _cuentaRsx = string.Empty;
        private string _centroCostoRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string _tipoBalRsx = string.Empty;
        private string unboundPrefix = "Unbound_";

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesar;
        public void EndProcesarMethod()
        {
            this.ControlBox = true;
            this.btnProcesar.Enabled = false;
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ProcesarReclasificacion() : base() { }

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Proyecto Origen
                GridColumn balTipo = new GridColumn();
                balTipo.FieldName = this.unboundPrefix + "BalanceTipoID";
                balTipo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BalanceTipoID");
                balTipo.UnboundType = UnboundColumnType.String;
                balTipo.VisibleIndex = 0;
                balTipo.Width = 110;
                balTipo.OptionsColumn.AllowEdit = false;
                this.gvOrigen.Columns.Add(balTipo);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarReclasificacion.cs", "AddGridCols"));
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
                this._data = _bc.AdministrationModel.ReclasificacionFiscal_GetForProcess(this.tipoBalance);

                this.gcOrigen.DataSource = this._data;
                this.gcOrigen.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarReclasificacion.cs", "LoadData"));
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
                this.documentID = AppProcess.ReclasifFiscal;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                this.endProcesar = new EndProcesar(EndProcesarMethod);

                //Inicia los calendarios
                string periodo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                string p14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);

                if (p14 == "1")
                    _bc.InitPeriodUC(this.period, 2);
                else
                    _bc.InitPeriodUC(this.period, 1);

                this.period.EnabledControl = false;
                this.period.DateTime = Convert.ToDateTime(periodo);

                #region Inicia el control de tipo de balance

                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                //Tipo balance IFRS
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "BalanceTipoID",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS),
                    OperadorSentencia = "OR"
                });

                //Tipo balance fiscal
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "BalanceTipoID",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFiscal),
                    OperadorSentencia = "OR"
                });

                _bc.InitMasterUC(this.masterBalanceTipo, AppMasters.coBalanceTipo, true, true, true, true, filtros);

                #endregion

                //Carga la info
                this.btnProcesar.Enabled = false;
                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarReclasificacion.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de procesar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
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

        /// <summary>
        /// Evento que se ejecuta al salir del control de balance Tipo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterBalanceTipo_Leave(object sender, EventArgs e)
        {
            if (this.tipoBalance != this.masterBalanceTipo.Value)
            {
                this.tipoBalance = this.masterBalanceTipo.Value;

                if (this.masterBalanceTipo.ValidID)
                {
                    this.btnProcesar.Enabled = true;
                    this.LoadData();
                }
                else
                    this.btnProcesar.Enabled = false;
            }
        }

        #endregion

        #region Hilos

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
                this._result = _bc.AdministrationModel.ReclasificacionFiscal_Procesar(this.documentID, this._actFlujo.ID.Value, this.tipoBalance);

                this.Invoke(this.endProcesar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarReclasificacion.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
