using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.XtraEditors;
using SentenceTransformer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.Utils;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class UC_Empleados : UserControl
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        public DTO_noEmpleado empleadoActivo;
        public List<DTO_noEmpleado> _empleados = new List<DTO_noEmpleado>();
        public List<DTO_noEmpleado> _empleadosOrigen = new List<DTO_noEmpleado>();
        private List<string> dSourceEstados = new List<string>();
        private int documentID;       
        private int empActivos = 1;
        private bool isMultipleSeleccion = true;
        private bool chechAll = false;
        private DateTime periodoNomina;


        #endregion

        #region Handlers

        // Declaración de delagados y evento click
        public delegate void EventHandler(object sender, System.EventArgs e);
        EventHandler handlerSelectRowEmpleado;        

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler SelectRowEmpleado_Click
        {
            add { this.handlerSelectRowEmpleado += value; }
            remove { this.handlerSelectRowEmpleado -= value; }
        }


        #endregion

        /// <summary>
        /// Constructor control
        /// </summary>
        /// <param name="empleado">empleado</param>
        public UC_Empleados()
        {
            InitializeComponent();
        }

        #region Funciones Públicas

        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Init()
        {
            this.documentID = AppMasters.noEmpleado;
            this.periodoNomina = DateTime.Parse(this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));

            this.GetResorcesGrid();            
            this.InitControls();                       
            this.LoadData();
            this.ActivateFilters();

            //Inicializa la grilla de empleados
            if (this._empleados != null && this._empleados.Count > 0)
            {
                this._empleadosOrigen = this._empleados.OrderBy(x =>x.Descriptivo.Value).ToList();
                this._empleados = this._empleadosOrigen.Where(x => x.Estado.Value == 1).ToList();
                this.gcEmpleado.DataSource = this._empleados.OrderBy(x =>x.Descriptivo.Value);
            }
            else
                this.gcEmpleado.DataSource = null;
        }

        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Init(List<DTO_noEmpleado> lempleados)
        {
            this.documentID = AppMasters.noEmpleado;
            this.GetResorcesGrid();  
            this.InitControls();            
            this.LoadData(lempleados);
            this.ActivateFilters();

            if (this._empleados != null && this._empleados.Count > 0)
            {
                this._empleadosOrigen = this._empleados.OrderBy(x => x.Descriptivo.Value).ToList();
                this._empleados = this._empleadosOrigen;
                this.gcEmpleado.DataSource = this._empleados.OrderBy(x => x.Descriptivo.Value);
            }
            else
                this.gcEmpleado.DataSource = null;
        }
               
        #endregion

        #region Funciones privadas


        /// <summary>
        /// Inicializa los controles 
        /// </summary>
        private void InitControls()
        { 
            if (!isMultipleSeleccion)
                this.gvEmpleado.OptionsSelection.MultiSelect = false;

            this._bc.InitMasterUC(this.uc_CentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this.uc_CentroCosto.EnableControl(false);
            this._bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, true, false);
            this.uc_Proyecto.EnableControl(false);
            this._bc.InitMasterUC(this.uc_Operacion, AppMasters.noOperacion, true, true, true, false);
            this.uc_Operacion.EnableControl(false);
            this._bc.InitMasterUC(this.uc_AreaFuncional, AppMasters.glAreaFuncional, true, true, true, false);
            this.uc_AreaFuncional.EnableControl(false);

            dSourceEstados.Clear();
            dSourceEstados.Add("Activos");
            dSourceEstados.Add("Inactivos");
            dSourceEstados.Add("Retirados");

            foreach (var item in dSourceEstados)
                this.lkpEstadoDesc.Properties.Items.Add(item);  

            this.lkpEstadoDesc.SelectedIndex = 0;

            Dictionary<string, string> datosTipoContrato = new Dictionary<string, string>();
            datosTipoContrato.Add("0", "-");
            datosTipoContrato.Add("1", "Ley 50");
            datosTipoContrato.Add("2", "Tradicional");
            datosTipoContrato.Add("3", "Salario Integral");
            datosTipoContrato.Add("4", "Sena Productivo");
            datosTipoContrato.Add("5", "Sena Lectivo");
            datosTipoContrato.Add("6", "Pensión");
            datosTipoContrato.Add("7", "Sustitución Pensional");
            this.cmbTipoContrato.Properties.ValueMember = "Key";
            this.cmbTipoContrato.Properties.DisplayMember = "Value";
            this.cmbTipoContrato.Properties.DataSource = datosTipoContrato;
            this.cmbTipoContrato.EditValue = "0";
              
        }

        /// <summary>
        /// Carga la información de los empleados
        /// </summary>
        private void LoadData()
        {
            long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.noEmpleado, null, null, true);
            var ltemp = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.noEmpleado, count, 1, null, null, true);

            this._empleados.Clear();
            foreach (var item in ltemp)
            {
                var emp = (DTO_noEmpleado)item;
                this._empleados.Add((DTO_noEmpleado)item);
            }

        }            

        /// <summary>
        /// Carga la información de los empleados
        /// </summary>
        private void LoadData(List<DTO_noEmpleado> lempleados)
        {
            this._empleadosOrigen = lempleados;
            this._empleados =this._empleadosOrigen;
        }

        /// <summary>
        /// Carga los recursos de la Grilla
        /// </summary>
        private void GetResorcesGrid()
        {
            this.empleadoId.Caption = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado + "_EmpleadoID");
            this.empleado.Caption = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado + "_Empleado_Descriptivo");
        }

        /// <summary>
        /// Activa los filtros
        /// </summary>
        private void ActivateFilters()
        {
            this.uc_AreaFuncional.EnableControl(this.UseFilters);
            this.uc_Operacion.EnableControl(this.UseFilters);
            this.uc_CentroCosto.EnableControl(this.UseFilters);
            this.uc_Proyecto.EnableControl(this.UseFilters);
        }


        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvEmpleado_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                DTO_noEmpleado row = (DTO_noEmpleado)grid.GetRow(e.FocusedRowHandle);
                if (row == null)
                {
                    this.uc_CentroCosto.Value = string.Empty;
                    this.uc_Proyecto.Value = string.Empty;
                    this.uc_Operacion.Value = string.Empty;
                    this.uc_AreaFuncional.Value = string.Empty;
                    this.dtFechaIngreso.Text = string.Empty;
                    this.cmbTipoContrato.EditValue = "0";
                    return;
                }

                if (this._empleados != null && this._empleados.Count > 0)
                {
                    this.empleadoActivo = row;

                    this.uc_CentroCosto.Value = row.CentroCostoID.Value;
                    this.uc_Proyecto.Value = row.ProyectoID.Value;
                    this.uc_Operacion.Value = row.OperacionNOID.Value;
                    this.uc_AreaFuncional.Value = row.AreaFuncionalID.Value;
                    this.dtFechaIngreso.DateTime = row.FechaIngreso.Value.Value;
                    this.cmbTipoContrato.EditValue = row.TipoContrato.Value.ToString();
                    if (this.handlerSelectRowEmpleado != null)
                        this.handlerSelectRowEmpleado(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-UC_Empleados.cs", "gvEmpleado_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Seleccionar empleados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    {
                        ((DTO_noEmpleado)this._empleados[e.ControllerRow]).Visible.Value = true;
                        break;
                    }
                case CollectionChangeAction.Remove:
                    {
                        ((DTO_noEmpleado)this._empleados[e.ControllerRow]).Visible.Value = false;
                        break;
                    }
                case CollectionChangeAction.Refresh:
                    {
                        this.chechAll = !this.chechAll;
                        foreach (var emp in this._empleados)
                        {
                            emp.Visible.Value = this.chechAll;
                        }
                        break;
                    }
            }
        }

        private void lkpEstadoDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = ((ComboBoxEdit)sender);
            byte valueItem = 1;
            bool filtroFechaRetiro = false;

            if (combo.SelectedText == "Activos")
            {
                valueItem = 1;
                this.chkRetirosmes.Visible = false;
            }
            if (combo.SelectedText == "Inactivos")
            {
                valueItem = 2;
                this.chkRetirosmes.Visible = false;
            }
            if (combo.SelectedText == "Retirados")
            {
                valueItem = 3;
                this.chkRetirosmes.Visible = true;
                if (this.chkRetirosmes.Checked)
                    filtroFechaRetiro = true;
                else
                    filtroFechaRetiro = false;
            }

            //this.txtEstado.Text = item.Key.ToString();
            this.LoadData();

            //Inicializa la grilla de empleados
            if (this._empleados != null)
            {
                this._empleadosOrigen = this._empleados.OrderBy(x =>x.Descriptivo.Value).ToList();
                this._empleados = this._empleadosOrigen.Where(x => x.Estado.Value == valueItem).ToList();
                if (filtroFechaRetiro)
                {
                    this._empleados = this._empleados.Where(x => x.FechaRetiro.Value.Value >= this.periodoNomina).ToList();
                }
                else
                {
                    this._empleados = this._empleadosOrigen.Where(x => x.Estado.Value == valueItem).ToList();
                }

                this.gcEmpleado.DataSource = this._empleados.OrderBy(x => x.Descriptivo.Value);
            }
            else
                this.gcEmpleado.DataSource = null;
        }
        
        #endregion

        #region Propiedades

        /// <summary>
        /// Valida si se cargan las empleados activos o inactivos
        /// </summary>
        public int EmpActivos
        {
            get { return empActivos; }
            set { empActivos = value; }
        }
        
        /// <summary>
        /// Valida si se puede seleccionar mas de un empleado en la grilla
        /// </summary>
        public bool IsMultipleSeleccion
        {
            get { return isMultipleSeleccion; }
            set { isMultipleSeleccion = value; }
        }

        /// <summary>
        /// Indica si usa Filtros
        /// </summary>
        public bool UseFilters { get; set; }

        #endregion  

        

      
                
    }

}
