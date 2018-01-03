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
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Simulador : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private int userID = 0;
        private string empresaId;
        //Variables basicas
        private int _statusBarProgress;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int documentID;
        private ModulesPrefix frmModule;
        private string UnboundPrefix = "Unbound_";
        //Variabes de consulta
        private string _proveedor = string.Empty;
        private string _tercero = string.Empty;
        private string _regimen = string.Empty;
        private string _lugGeo = string.Empty;
        private string _bienSer = string.Empty;
        private string _concCargo = string.Empty;
        private string _linPresup = string.Empty;
        private string _proyecto = string.Empty;
        private string _centroCosto = string.Empty;
        private string _operacion = string.Empty;

        private DTO_coTercero _dto_Tercero;
        private List<DTO_CuentaValor> data = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Progreso de procesos (importación)
        /// </summary>
        protected int StatusBarProgressProgress
        {
            get
            {
                return _statusBarProgress;
            }
            set
            {
                _statusBarProgress = value;
                FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, value });
            }
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public Simulador()
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this.InitControls();
                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Simulador.cs", "DocumentForm"));
            }

        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this.documentID = AppQueries.Simulador;
            this.frmModule = ModulesPrefix.co;
            //Inicia las variables del formulario
            this.empresaId = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        protected virtual void InitControls()
        {
            try
            {
                this.txtValue.Text = "0";

                _bc.InitMasterUC(this.masterRegimenEmp, AppMasters.coRegimenFiscal, false, true, true, true);
                _bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, false);
                _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, true);
                _bc.InitMasterUC(this.masterRegimenTer, AppMasters.coRegimenFiscal, false, true, true, true);
                _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
                _bc.InitMasterUC(this.masterBienServicio, AppMasters.prBienServicio, true, true, true, false);
                _bc.InitMasterUC(this.masterConcCargo, AppMasters.coConceptoCargo, true, true, true, true);
                _bc.InitMasterUC(this.masterLineaPresup, AppMasters.plLineaPresupuesto, true, true, true, true);
                _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
                _bc.InitMasterUC(this.masterOperacion, AppMasters.coOperacion, true, true, true, true);

                this.masterRegimenEmp.EnableControl(false);
                this.masterRegimenTer.EnableControl(false);
                //this.masterProveedor.EnableControl(false);

                //Trae el this._regimen fiscal de la empresa
                string tercIDEmp = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                DTO_MasterBasic basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, tercIDEmp, false);
                if (basic != null)
                {
                    DTO_coTercero tercEmp = (DTO_coTercero)basic;
                    this.masterRegimenEmp.Value = tercEmp.ReferenciaID.Value;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            GridColumn cuenta = new GridColumn();
            cuenta.FieldName = this.UnboundPrefix + "CuentaID";
            cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
            cuenta.UnboundType = UnboundColumnType.String;
            cuenta.VisibleIndex = 0;
            cuenta.Width = 170;
            cuenta.Visible = true;
            cuenta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvResults.Columns.Add(cuenta);

            GridColumn valor = new GridColumn();
            valor.FieldName = this.UnboundPrefix + "Valor";
            valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
            valor.UnboundType = UnboundColumnType.Decimal;
            valor.VisibleIndex = 1;
            valor.Width = 150;
            valor.Visible = true;
            valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvResults.Columns.Add(valor);

            GridColumn baseVlr = new GridColumn();
            baseVlr.FieldName = this.UnboundPrefix + "Base";
            baseVlr.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Base");
            baseVlr.UnboundType = UnboundColumnType.Decimal;
            baseVlr.VisibleIndex = 2;
            baseVlr.Width = 150;
            baseVlr.Visible = true;
            baseVlr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvResults.Columns.Add(baseVlr);
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Simulador.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Simulador.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Simulador.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Simulador.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al salir de un control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            DTO_MasterBasic basic;

            switch (master.ColId)
            {
                case "ProveedorID":
                    #region Proveedor
                    if (master.ValidID)
                    {
                        if (master.Value != this._proveedor)
                        {
                            this._proveedor = master.Value;
                            //Trae la info del proveedor
                            basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this._proveedor, true);
                            DTO_prProveedor pr = (DTO_prProveedor)basic;

                            //Calcula el this._tercero
                            this.masterTercero.Value = pr.TerceroID.Value;
                            this._tercero = this.masterTercero.Value;
                            basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._tercero, true);
                            this._dto_Tercero = (DTO_coTercero)basic;

                            //Calcula el this._regimen Fiscal
                            this._regimen = this._dto_Tercero.ReferenciaID.Value;
                            this.masterRegimenTer.Value = this._regimen;
                        }
                    }
                    else
                    {
                        this._proveedor = string.Empty;
                        //if (master.Value != string.Empty)
                        //{
                        //    this._tercero = string.Empty;
                        //    this._regimen = string.Empty;

                        //    this.masterTercero.Value = string.Empty;
                        //    this.masterRegimenTer.Value = string.Empty;
                        //}
                    }
                    #endregion
                    break;
                case "TerceroID":
                    #region Tercero
                    if (master.ValidID)
                    {
                        if (master.Value != this._tercero)
                        {
                            this._tercero = master.Value;

                            //Calcula el this._regimen Fiscal
                            basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._tercero, true);

                            this._dto_Tercero = (DTO_coTercero)basic;
                            this._regimen = this._dto_Tercero.ReferenciaID.Value;
                            this.masterRegimenTer.Value = this._regimen;

                            this._proveedor = string.Empty;
                            this.masterProveedor.Value = string.Empty;
                        }
                    }
                    else
                    {
                        this._tercero = string.Empty;

                        this._proveedor = string.Empty;
                        this.masterProveedor.Value = string.Empty;

                        //this._regimen = string.Empty;
                        //this.masterRegimenTer.Value = string.Empty;
                    }
                    #endregion
                    break;
                case "LugarGeograficoID":
                    #region Lugar Geografico
                    if (master.ValidID)
                    {
                        if (master.Value != this._lugGeo)
                            this._lugGeo = master.Value;
                    }
                    else
                        this._lugGeo = string.Empty;

                    #endregion
                    break;
                case "CodigoBSID":
                    #region Bien y/ Servicio
                    if (master.ValidID)
                    {
                        if (master.Value != this._bienSer)
                        {
                            this._bienSer = master.Value;
                            basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, false, this._bienSer, true);

                            DTO_prBienServicio bs = (DTO_prBienServicio)basic;
                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bs.ClaseBSID.Value, true);

                            this._concCargo = bsClase != null ? bsClase.ConceptoCargoID.Value : null;
                            this._linPresup = bsClase != null ? bsClase.LineaPresupuestoID.Value : null;

                            this.masterConcCargo.Value = this._concCargo;
                            this.masterLineaPresup.Value = this._linPresup;
                        }
                    }
                    else
                    {
                        this._bienSer = string.Empty;
                        //this._concCargo = string.Empty;
                        //this._linPresup = string.Empty;

                        //this.masterConcCargo.Value = string.Empty;
                        //this.masterLineaPresup.Value = string.Empty;
                    }
                    #endregion
                    break;
                case "ConceptoCargoID":
                    #region ConceptoCargoID
                    if (master.ValidID)
                    {
                        if (master.Value != this._concCargo)
                        {
                            this._concCargo = master.Value;

                            this._bienSer = string.Empty;
                            this.masterBienServicio.Value = string.Empty;
                        }
                    }
                    else
                    {
                        this._concCargo = string.Empty;
                        this._bienSer = string.Empty;

                        this.masterBienServicio.Value = string.Empty;
                    }
                    #endregion
                    break;
                case "LineaPresupuestoID":
                    #region Linea Presupuestal
                    if (master.ValidID)
                    {
                        if (master.Value != this._linPresup)
                        {
                            this._linPresup = master.Value;

                            this._bienSer = string.Empty;
                            this.masterBienServicio.Value = string.Empty;
                        }
                    }
                    else
                    {
                        this._linPresup = string.Empty;
                        this._bienSer = string.Empty;

                        this.masterBienServicio.Value = string.Empty;
                    }
                    #endregion
                    break;
                case "ProyectoID":
                    #region Proyecto
                    if (master.ValidID)
                    {
                        if (master.Value != this._proyecto)
                        {
                            this._proyecto = master.Value;
                            basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, false, this._proyecto, true);

                            DTO_coProyecto proy = (DTO_coProyecto)basic;
                            this._operacion = proy.OperacionID.Value;
                            this.masterOperacion.Value = this._operacion;

                            this._centroCosto = string.Empty;
                            this.masterCentroCosto.Value = string.Empty;
                        }
                    }
                    else
                    {
                        this._proyecto = string.Empty;

                        //this._operacion = string.Empty;
                        //this.masterOperacion.Value = string.Empty;
                    }
                    #endregion
                    break;
                case "CentroCostoID":
                    #region Centro de Costo
                    if (master.ValidID)
                    {
                        if (master.Value != this._centroCosto)
                        {
                            this._centroCosto = master.Value;
                            basic = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, this._centroCosto, true);

                            DTO_coCentroCosto ctoCosto = (DTO_coCentroCosto)basic;

                            this._operacion = ctoCosto.OperacionID.Value;
                            this.masterOperacion.Value = this._operacion;

                            this._proyecto = string.Empty;
                            this.masterProyecto.Value = string.Empty;
                        }
                    }
                    else
                    {
                        this._centroCosto = string.Empty;
                        //this._operacion = string.Empty;

                        //this.masterOperacion.Value = string.Empty;
                    }
                    #endregion
                    break;
                case "OperacionID":
                    #region Operacion
                    if (master.ValidID)
                    {
                        if (master.Value != this._operacion)
                        {
                            this._operacion = master.Value;

                            this._proyecto = string.Empty;
                            this._centroCosto = string.Empty;
                            this.masterProyecto.Value = string.Empty;
                            this.masterCentroCosto.Value = string.Empty;
                        }
                    }
                    else
                    {
                        this._proyecto = string.Empty;
                        this._centroCosto = string.Empty;
                        this._operacion = string.Empty;

                        this.masterProyecto.Value = string.Empty;
                        this.masterCentroCosto.Value = string.Empty;
                    }
                    #endregion
                    break;
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvResults_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length);

            if (fieldName == "Valor" || fieldName == "Base")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvResults_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para busquedas
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.gcResults.Focus();
                if (!this.masterRegimenEmp.ValidID || !this.masterTercero.ValidID || !this.masterRegimenTer.ValidID ||
                    !this.masterConcCargo.ValidID || !this.masterOperacion.ValidID || !this.masterLineaPresup.ValidID)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidSearchCriteria));
                    return;
                }

                //Si no hay lugar geografico saca el que es por defecto
                if (string.IsNullOrWhiteSpace(this.masterLugarGeo.Value))
                {
                    this.masterLugarGeo.Value = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                    this._lugGeo = this.masterLugarGeo.Value;
                }

                if (!this.masterLugarGeo.ValidID)
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLugarGeo.CodeRsx);
                    MessageBox.Show(msg);
                    return;
                }

                if (this.data != null)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        decimal valor = Convert.ToDecimal(this.txtValue.EditValue, CultureInfo.InvariantCulture);

                        List<DTO_SerializedObject> res = _bc.AdministrationModel.LiquidarImpuestos(ModulesPrefix.co, this._dto_Tercero, string.Empty, this._concCargo, this._operacion, this._lugGeo, this._linPresup, valor);
                        if (res.Count > 0 && res.First().GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult txRes = (DTO_TxResult)res.First();
                            MessageForm msg = new MessageForm(txRes);
                            msg.Show();
                        }
                        else
                        {
                            List<DTO_SerializedObject> lt = (List<DTO_SerializedObject>)res;
                            List<DTO_CuentaValor> results = lt.Cast<DTO_CuentaValor>().ToList();

                            this.data = results;
                            this.gcResults.DataSource = results;

                            if (results.Count == 0)
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        }
                    }
                }
                else
                {
                    decimal valor = Convert.ToDecimal(this.txtValue.EditValue, CultureInfo.InvariantCulture);

                    List<DTO_SerializedObject> res = _bc.AdministrationModel.LiquidarImpuestos(ModulesPrefix.co, this._dto_Tercero, string.Empty, this._concCargo, this._operacion, this._lugGeo, this._linPresup, valor);
                    if (res.Count > 0 && res.First().GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult txRes = (DTO_TxResult)res.First();
                        MessageForm msg = new MessageForm(txRes);
                        msg.Show();
                    }
                    else
                    {
                        List<DTO_SerializedObject> lt = (List<DTO_SerializedObject>)res;
                        List<DTO_CuentaValor> results = lt.Cast<DTO_CuentaValor>().ToList();

                        this.data = results;
                        this.gcResults.DataSource = results;
                        if (results.Count == 0)
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Simulador.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para actualizar las busquedas
        /// </summary>
        public override void TBUpdate()
        {
            if (this.data != null)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearchClean);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this._proveedor = string.Empty;
                    this._tercero = string.Empty;
                    this._regimen = string.Empty;
                    this._lugGeo = string.Empty;
                    this._bienSer = string.Empty;
                    this._concCargo = string.Empty;
                    this._linPresup = string.Empty;
                    this._proyecto = string.Empty;
                    this._operacion = string.Empty;
                    this._centroCosto = string.Empty;

                    this.masterBienServicio.Value = string.Empty;
                    this.masterConcCargo.Value = string.Empty;
                    this.masterLineaPresup.Value = string.Empty;
                    this.masterLugarGeo.Value = string.Empty;
                    this.masterOperacion.Value = string.Empty;
                    this.masterProveedor.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterRegimenTer.Value = string.Empty;
                    this.masterTercero.Value = string.Empty;
                    this.masterCentroCosto.Value = string.Empty;
                    this.txtValue.Text = "0";

                    this.data = null;
                    this.gcResults.DataSource = null;
                    this.gcResults.RefreshDataSource();
                }
            }
            else
            {
                this._proveedor = string.Empty;
                this._tercero = string.Empty;
                this._regimen = string.Empty;
                this._lugGeo = string.Empty;
                this._bienSer = string.Empty;
                this._concCargo = string.Empty;
                this._linPresup = string.Empty;
                this._proyecto = string.Empty;
                this._operacion = string.Empty;
                this._centroCosto = string.Empty;

                this.masterBienServicio.Value = string.Empty;
                this.masterConcCargo.Value = string.Empty;
                this.masterLineaPresup.Value = string.Empty;
                this.masterLugarGeo.Value = string.Empty;
                this.masterOperacion.Value = string.Empty;
                this.masterProveedor.Value = string.Empty;
                this.masterProyecto.Value = string.Empty;
                this.masterRegimenTer.Value = string.Empty;
                this.masterTercero.Value = string.Empty;
                this.masterCentroCosto.Value = string.Empty;
                this.txtValue.Text = "0";
            }
        }

        #endregion

    }
}
