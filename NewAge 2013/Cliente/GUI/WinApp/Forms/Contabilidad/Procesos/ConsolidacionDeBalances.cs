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
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsolidacionDeBalances : ProcessForm
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private string unboundPrefix = "Unbound_";
        private List<DTO_ComprobanteConsolidacion> _data;

        #endregion

        #region Delegados

        public delegate void EndProcess();
        public EndProcess endProcessDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        public void EndProcessMethod()
        {
            this.btnProcesar.Enabled = true;
            this.ControlBox = true;
            this.LoadData();
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ConsolidacionDeBalances() : base() { }

        #region Funciones privadas

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Consolidar
            GridColumn aprob = new GridColumn();
            aprob.FieldName = this.unboundPrefix + "Consolidar";
            aprob.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Consolidar");
            aprob.UnboundType = UnboundColumnType.Boolean;
            aprob.VisibleIndex = 0;
            aprob.Width = 20;
            aprob.Visible = true;
            aprob.ImageAlignment = StringAlignment.Center;
            this.gvCompanies.Columns.Add(aprob);

            //Empresa
            GridColumn emp = new GridColumn();
            emp.FieldName = this.unboundPrefix + "EmpresaID";
            emp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpresaID");
            emp.UnboundType = UnboundColumnType.String;
            emp.VisibleIndex = 1;
            emp.Width = 90;
            emp.Visible = true;
            emp.OptionsColumn.AllowEdit = false;
            this.gvCompanies.Columns.Add(emp);

            //Centro Costo
            GridColumn ctoCosto = new GridColumn();
            ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
            ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            ctoCosto.UnboundType = UnboundColumnType.String;
            ctoCosto.VisibleIndex = 2;
            ctoCosto.Width = 100;
            ctoCosto.Visible = true;
            ctoCosto.OptionsColumn.AllowEdit = false;
            this.gvCompanies.Columns.Add(ctoCosto);

            //Procesado
            GridColumn proc = new GridColumn();
            proc.FieldName = this.unboundPrefix + "Procesado";
            proc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Procesado");
            proc.UnboundType = UnboundColumnType.Boolean;
            proc.VisibleIndex = 3;
            proc.Width = 60;
            proc.Visible = true;
            proc.ImageAlignment = StringAlignment.Center;
            proc.OptionsColumn.AllowEdit = false;
            this.gvCompanies.Columns.Add(proc);
        }

        /// <summary>
        /// Trae los datos de la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {
                this._data = _bc.AdministrationModel.coPlanillaConsolidacion_GetEmpresas();
                this.gcCompanies.DataSource = this._data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsolidacionDeBalances.cs", "LoadData"));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.ConsolidacionBalances;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcessDelegate = new EndProcess(EndProcessMethod);

            #region Asigna la info del periodo
            string periodo14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
            if (periodo14 == "1")
                _bc.InitPeriodUC(this.periodoEdit, 2);
            else
                _bc.InitPeriodUC(this.periodoEdit, 1);

            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
            DateTime dt = DateTime.Today;
            try
            {
                dt = DateTime.Parse(periodo);
            }
            catch (Exception)
            {
                dt = DateTime.Today;
            }
            this.periodoEdit.DateTime = dt;

            #endregion

            this.AddGridCols();
            this.LoadData();
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvCompanies_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Boolean")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Boolean")
                        pi.SetValue(dto, e.Value, null);
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnProcesar.Enabled = false;
            new Thread(ProcesarThread).Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de procesar
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();
                List<DTO_TxResult> result = this._bc.AdministrationModel.Proceso_ConsolidacionBalances(this.documentID, this._actFlujo.ID.Value, this._data);

                this.Invoke(this.endProcessDelegate);
                this.StopProgressBarThread();
                
                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsolidacionDeBalance.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }

}
