using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Globalization;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ReintegroClientes : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private List<DTO_ccReintegroClienteDeta> reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
        private List<DTO_ccReintegroClienteDeta> reintegrosClientesFiltro = new List<DTO_ccReintegroClienteDeta>();

        //Variables privadas
        private string reintegroID = String.Empty;
        private DateTime periodo;
        private decimal vlrTotal = 0;
        private bool validate = true;

        #endregion

        public ReintegroClientes()
            : base()
        {
            //InitializeComponent();
        }

        public ReintegroClientes(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.InitializeComponent();
                base.SetInitParameters();

                this.documentID = AppDocuments.ReintegroClientes;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();
                this._bc.InitMasterUC(this.masterReintegros, AppMasters.ccReintegroSaldo, false, true, true, false);
                this._bc.InitMasterUC(this.masterTerceros, AppMasters.coTercero, true, true, true, false);

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

                this.lkpTipos.EditValueChanged += new System.EventHandler(this.lkpTipos_EditValueChanged);
                Dictionary<int, string> tipos = new Dictionary<int, string>();
                tipos[0] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoReintegros_1);
                tipos[1] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoReintegros_2);
                this.lkpTipos.Properties.DataSource = tipos;
                this.lkpTipos.EditValue = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 40;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(aprob);

                //TerceroID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "TerceroID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 1;
                clienteID.Width = 130;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 2;
                nombre.Width = 250;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombre);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 3;
                libranza.Width = 150;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //VlrSaldo
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this.unboundPrefix + "ValorMax";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.Decimal;
                vlrSaldo.VisibleIndex = 4;
                vlrSaldo.Width = 170;
                vlrSaldo.OptionsColumn.AllowEdit = false;
                vlrSaldo.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrSaldo);

                //VlrPago
                GridColumn vlrPago = new GridColumn();
                vlrPago.FieldName = this.unboundPrefix + "ValorPago";
                vlrPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorPago");
                vlrPago.UnboundType = UnboundColumnType.Decimal;
                vlrPago.VisibleIndex = 5;
                vlrPago.Width = 170;
                vlrPago.OptionsColumn.AllowEdit = true;
                vlrPago.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrPago);

                //VlrPago
                GridColumn vlrAjuste = new GridColumn();
                vlrAjuste.FieldName = this.unboundPrefix + "ValorAjuste";
                vlrAjuste.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjuste");
                vlrAjuste.UnboundType = UnboundColumnType.Decimal;
                vlrAjuste.VisibleIndex = 6;
                vlrAjuste.Width = 170;
                vlrAjuste.OptionsColumn.AllowEdit = false;
                vlrAjuste.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrAjuste);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            try
            {
                if (this.reintegrosClientesFiltro[fila].Aprobado.Value.Value)
                {
                    string colName = Convert.ToInt32(this.lkpTipos.EditValue) == 0 ? "ValorPago" : "ValorAjuste";
                    bool validField = _bc.ValidGridCellValue(this.gvDocument, this.unboundPrefix, fila, colName, false, false, true, false);
                    if (!validField)
                        return false;

                    //Valida que el valor no supere el máximo permitido
                    if (this.reintegrosClientesFiltro[fila].Valor.Value.Value > this.reintegrosClientesFiltro[fila].ValorMax.Value.Value)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidMaxValue);
                        msg = string.Format(msg, this.reintegrosClientesFiltro[fila].ValorMax.Value.Value.ToString("c"));

                        GridColumn col = this.gvDocument.Columns[this.unboundPrefix + colName];
                        this.gvDocument.SetColumnError(col, msg);

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "ValidateRow"));
                return false;
            }

            return true;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.masterReintegros.Value = String.Empty;
            this.txt_VlrTotaReintegro.Text = String.Empty;
            this.chkAll.Checked = false;

            //Variables
            this.reintegroID = string.Empty;
            this.terceroID = string.Empty;
            this.vlrTotal = 0;

            this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
            this.reintegrosClientesFiltro = new List<DTO_ccReintegroClienteDeta>();

            this.validate = false;
            this.gcDocument.DataSource = this.reintegrosClientesFiltro;
            this.validate = true;
        }

        /// <summary>
        /// Calcula el total de los reintegros
        /// </summary>
        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DTO_ccReintegroClienteDeta deta in this.reintegrosClientesFiltro)
            {
                if(deta.Aprobado.Value.Value)
                {
                    total += deta.Valor.Value.Value;
                }
            }

            this.vlrTotal = total;
            this.txt_VlrTotaReintegro.EditValue = total;
        }
        
        /// <summary>
        /// Filter Data
        /// </summary>
        private void FilterData()
        {
            try
            {
                if (this.reintegrosClientes != null && this.reintegrosClientes.Count > 0)
                {
                    this.reintegrosClientesFiltro = this.reintegrosClientes;

                    //Tercero
                    if (!string.IsNullOrWhiteSpace(this.masterTerceros.Value))
                        this.reintegrosClientesFiltro = this.reintegrosClientes.Where(r => r.TerceroID.Value == this.masterTerceros.Value).ToList();

                    if (Convert.ToInt32(this.lkpTipos.EditValue) == 0)
                    {
                        //Giro
                        this.reintegrosClientesFiltro = this.reintegrosClientesFiltro.Where(r => r.Valor.Value == 0 || r.ValorPago.Value != 0).ToList();
                    }
                    else
                    {
                        //Ajuste
                        this.reintegrosClientesFiltro = this.reintegrosClientesFiltro.Where(r => r.Valor.Value == 0 || r.ValorAjuste.Value != 0).ToList();
                    }

                    //Pendientes de aprobación
                    if (this.chkPendientes.Checked)
                        this.reintegrosClientesFiltro = this.reintegrosClientesFiltro.Where(r => r.NumeroDoc.Value.HasValue).ToList();


                    this.CalcularTotal();
                    this.validate = false;
                    this.gcDocument.DataSource = this.reintegrosClientesFiltro;
                    this.gvDocument.MoveFirst();
                    this.gvDocument.RefreshData();
                    this.validate = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "FilterData"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);
              
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;               
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al momento de salir del componente de cartera
        /// </summary>
        private void masterReintegros_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterReintegros.ValidID)
                {
                    if (this.reintegroID != this.masterReintegros.Value)
                    {
                        this.reintegroID = this.masterReintegros.Value;
                        this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
                        this.vlrTotal = 0;

                        DTO_ccReintegroSaldo reintegro = (DTO_ccReintegroSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccReintegroSaldo, false, this.reintegroID, true);
                        string cuentaID = reintegro.CuentaID.Value;
                        this.reintegrosClientes = this._bc.AdministrationModel.ReintegroClientes_GetByCuenta(cuentaID);

                        this.FilterData();
                    }
                }
                else
                { 
                    string reintegroTemp =  this.masterReintegros.Value;
                    this.reintegroID = string.Empty;
                    string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.reintegroID);
                    MessageBox.Show(msg);
                    this.CleanData();

                    this.masterReintegros.Value = reintegroTemp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "masterCompradorCartera_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al momento de salir del filtro de tercero
        /// </summary>
        private void masterTerceros_Leave(object sender, EventArgs e)
        {
            this.FilterData();
        }

        /// <summary>
        /// Evento que permite seleccionar solo los registros pendientes por aprobar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPendientes_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterData();
        }

        /// <summary>
        /// Evento que permite seleccionar solo los registros pendientes por aprobar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkpTipos_EditValueChanged(object sender, EventArgs e)
        {
            try 
            { 
                if(this.reintegrosClientes != null && this.reintegrosClientes.Count > 0)
                {
                    if(Convert.ToInt32(this.lkpTipos.EditValue) == 0)
                    {
                        //Giro
                        this.gvDocument.Columns[this.unboundPrefix + "ValorPago"].OptionsColumn.AllowEdit = true;
                        this.gvDocument.Columns[this.unboundPrefix + "ValorAjuste"].OptionsColumn.AllowEdit = false;
                    }
                    else
                    {
                        //Ajuste
                        this.gvDocument.Columns[this.unboundPrefix + "ValorPago"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "ValorAjuste"].OptionsColumn.AllowEdit = true;
                    }

                    this.FilterData();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "lkpTipos_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el estado del control
        /// </summary>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DTO_ccReintegroClienteDeta item in this.reintegrosClientesFiltro)
                {
                    if (this.chkAll.Checked)
                    {
                        item.Aprobado.Value = true;
                        if (item.Valor.Value == 0)
                            item.Valor.Value = item.ValorMax.Value;

                        if (Convert.ToInt32(this.lkpTipos.EditValue) == 0)
                            item.ValorPago.Value = item.Valor.Value;
                        else
                            item.ValorAjuste.Value = item.Valor.Value;
                    }
                    else
                    {
                        item.Aprobado.Value = false;
                        item.Valor.Value = 0;
                        item.ValorAjuste.Value = 0;
                        item.ValorPago.Value = 0;
                    }
                }

                this.txt_VlrTotaReintegro.EditValue = this.reintegrosClientesFiltro.Sum(r => r.Valor.Value.Value);
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "chkAll_CheckedChanged"));
            }
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Aprobado")
                {
                    #region Aprobado
                    if ((bool)e.Value)
                    {
                        this.reintegrosClientesFiltro[e.RowHandle].Aprobado.Value = true;
                    }
                    else
                    {
                        this.reintegrosClientesFiltro[e.RowHandle].Aprobado.Value = false;
                    }

                    this.CalcularTotal();
                    #endregion
                }

                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "gvDocument_CellValueChanging"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (fieldName == "Aprobado")
                {
                    if ((bool)e.Value)
                    {
                        //if (this.reintegrosClientesFiltro[fila].Valor.Value == 0)
                        //{
                        this.reintegrosClientesFiltro[fila].Valor.Value = this.reintegrosClientesFiltro[fila].ValorMax.Value;
                        if (Convert.ToInt32(this.lkpTipos.EditValue) == 0)
                        {
                            this.reintegrosClientesFiltro[fila].ValorPago.Value = this.reintegrosClientesFiltro[fila].Valor.Value;
                            this.reintegrosClientesFiltro[fila].ValorAjuste.Value = 0;
                        }
                        else
                        {
                            this.reintegrosClientesFiltro[fila].ValorPago.Value = 0;
                            this.reintegrosClientesFiltro[fila].ValorAjuste.Value = this.reintegrosClientesFiltro[fila].Valor.Value;
                        }
                        //}
                    }
                    else
                    {
                        this.reintegrosClientesFiltro[fila].Valor.Value = 0;
                        this.reintegrosClientesFiltro[fila].ValorAjuste.Value = 0;
                        this.reintegrosClientesFiltro[fila].ValorPago.Value = 0;
                    }
                }

                if (fieldName == "ValorPago")
                {
                    if (this.reintegrosClientesFiltro[fila].ValorPago == null || !this.reintegrosClientesFiltro[fila].ValorPago.Value.HasValue)
                    {
                        this.reintegrosClientesFiltro[fila].Aprobado.Value = false;
                        this.reintegrosClientesFiltro[fila].ValorPago.Value = 0;
                    }

                    this.reintegrosClientesFiltro[fila].ValorAjuste.Value = 0;
                    this.reintegrosClientesFiltro[fila].Valor.Value = this.reintegrosClientesFiltro[fila].ValorPago.Value;
                }

                if (fieldName == "ValorAjuste")
                {
                    if (this.reintegrosClientesFiltro[fila].ValorAjuste == null || !this.reintegrosClientesFiltro[fila].ValorAjuste.Value.HasValue)
                    {
                        this.reintegrosClientesFiltro[fila].Aprobado.Value = false;
                        this.reintegrosClientesFiltro[fila].ValorAjuste.Value = 0;
                    }

                    this.reintegrosClientesFiltro[fila].ValorPago.Value = 0;
                    this.reintegrosClientesFiltro[fila].Valor.Value = this.reintegrosClientesFiltro[fila].ValorPago.Value;
                }

                this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {

                if (this.validate && e.RowHandle >= 0)
                {
                    bool validRow = this.ValidateRow(e.RowHandle);
                    this.deleteOP = false;

                    if (validRow)
                        this.isValid = true;
                    else
                    {
                        e.Allow = false;
                        this.isValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.masterReintegros.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                if (this.isValid)
                {
                    //List<DTO_ccReintegroClienteDeta> reintegroAprobados = this.reintegrosClientesFiltro.Where(x => x.Aprobado.Value.Value).ToList();
                    //if (reintegroAprobados.Count > 0 && this.vlrTotal >= 0)
                    if (this.reintegrosClientesFiltro.Count > 0)
                    {
                        Thread process = new Thread(this.SaveThread);
                        this.reintegrosClientesFiltro.ForEach(x => x.FechaReintegro.Value = this.dtFecha.DateTime);
                        process.Start();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.gvDocument.DataRowCount > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();                 
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ccReintegroClienteDeta), this.reintegrosClientesFiltro);
                    if (tableAll.Columns.Contains("NumeroDoc"))
                        tableAll.Columns.Remove("NumeroDoc");
                    if (tableAll.Columns.Contains("NumDocCredito"))
                        tableAll.Columns.Remove("NumDocCredito");
                    if (tableAll.Columns.Contains("ClienteID"))
                        tableAll.Columns.Remove("ClienteID");
                    if (tableAll.Columns.Contains("CuentaID"))
                        tableAll.Columns.Remove("CuentaID");
                    if (tableAll.Columns.Contains("ComponenteCarteraID"))
                        tableAll.Columns.Remove("ComponenteCarteraID");
                    if (tableAll.Columns.Contains("FechaReintegro"))
                        tableAll.Columns.Remove("FechaReintegro");
                    if (tableAll.Columns.Contains("FechaAprobacionReintegro"))
                        tableAll.Columns.Remove("FechaAprobacionReintegro");
                    if (tableAll.Columns.Contains("Valor"))
                        tableAll.Columns.Remove("Valor");
                    if (tableAll.Columns.Contains("Consecutivo"))
                        tableAll.Columns.Remove("Consecutivo");
                    if (tableAll.Columns.Contains("Aprobado"))
                        tableAll.Columns.Remove("Aprobado");
                    if (tableAll.Columns.Contains("AsesorID"))
                        tableAll.Columns.Remove("AsesorID");
                    if (tableAll.Columns.Contains("Rechazado"))
                        tableAll.Columns.Remove("Rechazado");
                    if (tableAll.Columns.Contains("HasDetalle"))
                        tableAll.Columns.Remove("HasDetalle");
                    if (tableAll.Columns.Contains("Detalle"))
                        tableAll.Columns.Remove("Detalle");
                    if (tableAll.Columns.Contains("NumDocCxP"))
                        tableAll.Columns.Remove("NumDocCxP");
                    if (tableAll.Columns.Contains("CuentaReintegroID"))
                        tableAll.Columns.Remove("CuentaReintegroID");
                    ReportExcelBase frm = new ReportExcelBase(tableAll,this.documentID);
                    frm.Show();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "TBExport"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                #region Guarda la info
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                bool isGiro = Convert.ToInt32(this.lkpTipos.EditValue) == 0;
                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                results = _bc.AdministrationModel.ReintegroClientes_Add(this.documentID, this._actFlujo.ID.Value, this.reintegrosClientesFiltro, this.dtFecha.DateTime, 
                    this.vlrTotal, isGiro, this.masterReintegros.Value);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                #region Oganiza resultados
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                bool valid = true;
                DTO_TxResult res = new DTO_TxResult();
                res.Result = ResultValue.OK;
                MessageForm frm = new MessageForm(res);
                if (results.Count == 1)
                {
                    if (results[0].GetType() == typeof(DTO_TxResult))
                    {
                        frm = new MessageForm((DTO_TxResult)results[0]);
                        valid = false;
                    }
                }
                #endregion

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (valid)
                    this.Invoke(this.refreshGridDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }

}
