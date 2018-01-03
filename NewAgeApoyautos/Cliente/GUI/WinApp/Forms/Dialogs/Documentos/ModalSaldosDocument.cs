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
using DevExpress.XtraEditors;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class ModalSaldosDocument : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_coCuentaSaldo> _listCuentaSaldosDoc = null;
        protected List<DTO_coCuentaSaldo> _listCuentaSaldoSelected = null;
        protected DTO_coCuentaSaldo _cuentaSaldo = null;
        private string unboundPrefix = "Unbound_";
        private int _documentID;
        private int _pageSize = 0;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalSaldosDocument()
        {
           // this.InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalSaldosDocument(DateTime periodoID, string cuentaID, string terceroD,string comprobanteID, bool isMulSelection = true)
        {
            try
            {
                this.InitializeComponent();
                if (isMulSelection)
                    this.MultipleSelection = isMulSelection;
                this.SetInitParameters();
                this.InitControls();
                this.LoadData(periodoID, cuentaID, terceroD,comprobanteID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalSaldosDocument.cs", "ModalSaldosDocument")); ;
            }
        }  
    
        #region Funciones Virtuales

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        protected virtual void InitControls()
        {
            #region Controles Maestras
            this._bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            this.masterCuenta.EnableControl(false);
            this.masterTercero.EnableControl(false);
            #endregion                        

            #region Paginador
            this._bc.Pagging_Init(this.pgGrid, this._pageSize);
            this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
            this.pgGrid.UpdatePageNumber(this._listCuentaSaldosDoc.Count, true, true, false);
            this.toolTipGrid.SetToolTip(this.gcDocument, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ToolTipGrid)); 
            #endregion

            FormProvider.LoadResources(this, this._documentID);
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        protected virtual void SetInitParameters()
        {
            this._documentID = AppForms.BalanceForm;
            this._listCuentaSaldosDoc = new List<DTO_coCuentaSaldo>();
            this._listCuentaSaldoSelected = new List<DTO_coCuentaSaldo>();
            this._pageSize = Convert.ToInt32(this._bc.GetControlValue(AppControl.PaginadorMaestra));
            this.AddGridCols();           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            try
            {
                GridColumn marca = new GridColumn();
                marca.FieldName = this.unboundPrefix + "Marca";
                marca.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Marca");
                marca.UnboundType = UnboundColumnType.Boolean;
                marca.VisibleIndex = 0;
                marca.Width = 10;
                marca.Visible = true;
                marca.OptionsColumn.ShowCaption = false;
                marca.OptionsColumn.AllowEdit = true;
                marca.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                marca.ColumnEdit = this.editChkBox;
                this.gvDocument.Columns.Add(marca);
                //this.chkSelectAll.Visible = true;

                GridColumn DocumentoPrefijoNro = new GridColumn();
                DocumentoPrefijoNro.FieldName = this.unboundPrefix + "PrefDoc";
                DocumentoPrefijoNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocMask");
                DocumentoPrefijoNro.UnboundType = UnboundColumnType.String;
                DocumentoPrefijoNro.VisibleIndex = 0;
                DocumentoPrefijoNro.Width = 100;
                DocumentoPrefijoNro.Visible = true;
                DocumentoPrefijoNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoPrefijoNro);

                GridColumn SaldoTotalML = new GridColumn();
                SaldoTotalML.FieldName = this.unboundPrefix + "SaldoTotalML";
                SaldoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoTotalML");
                SaldoTotalML.UnboundType = UnboundColumnType.Decimal;
                SaldoTotalML.VisibleIndex = 0;
                SaldoTotalML.Width = 120;
                SaldoTotalML.Visible = true;
                SaldoTotalML.OptionsColumn.AllowEdit = false;
                SaldoTotalML.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(SaldoTotalML);

                GridColumn SaldoTotalME = new GridColumn();
                SaldoTotalME.FieldName = this.unboundPrefix + "SaldoTotalME";
                SaldoTotalME.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoTotalME");
                SaldoTotalME.UnboundType = UnboundColumnType.Decimal;
                SaldoTotalME.VisibleIndex = 0;
                SaldoTotalME.Width = 120;
                SaldoTotalME.Visible = this._bc.AdministrationModel.MultiMoneda;
                SaldoTotalME.OptionsColumn.AllowEdit = false;
                SaldoTotalME.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(SaldoTotalME);

                #region Columnas no visibles

                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 0;
                Descripcion.Width = 130;
                Descripcion.Visible = false;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.UnboundType = UnboundColumnType.String;              
                TerceroID.Visible = false;
                this.gvDocument.Columns.Add(TerceroID);

                GridColumn LineaPresupuestoID = new GridColumn();
                LineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                LineaPresupuestoID.UnboundType = UnboundColumnType.String;
                LineaPresupuestoID.Visible = false;
                this.gvDocument.Columns.Add(LineaPresupuestoID);

                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.Visible = false;
                this.gvDocument.Columns.Add(ProyectoID);

                GridColumn LugarGeograficoID = new GridColumn();
                LugarGeograficoID.FieldName = this.unboundPrefix + "LugarGeograficoID";
                LugarGeograficoID.UnboundType = UnboundColumnType.String;
                LugarGeograficoID.Visible = false;
                this.gvDocument.Columns.Add(LugarGeograficoID);

                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.Visible = false;
                this.gvDocument.Columns.Add(CentroCostoID);

                GridColumn DocumentoTercero = new GridColumn();
                DocumentoTercero.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocumentoTercero.UnboundType = UnboundColumnType.String;
                DocumentoTercero.Visible = false;
                this.gvDocument.Columns.Add(DocumentoTercero);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSaldosDocument", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected virtual void LoadData(DateTime periodoID, string cuentaID, string terceroID, string comprobanteID)
        {
            try
            {
                DTO_coComprobante comprobante = null;

                //Trae el valor de los campos 
                this.masterCuenta.Value = cuentaID;
                this.masterTercero.Value = terceroID;

                //Trae el comprobante actual para obtener saldos
                comprobante = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, comprobanteID, true);
                if (comprobante != null)
                {
                    //Crea filtro
                    DTO_coCuentaSaldo filter = new DTO_coCuentaSaldo();
                    filter.PeriodoID.Value = periodoID;
                    filter.BalanceTipoID.Value = comprobante.BalanceTipoID.Value;
                    filter.CuentaID.Value = cuentaID;
                    filter.TerceroID.Value = terceroID;
                    this._listCuentaSaldosDoc = this._bc.AdministrationModel.Saldos_GetByParameter(filter);
                    this._listCuentaSaldosDoc.RemoveAll(x => x.IdentificadorTR.Value == 0);
                    foreach (var item in this._listCuentaSaldosDoc)
                    {
                        #region Asigna variables
                        item.SaldoTotalML.Value = item.DbOrigenLocML.Value.Value + item.DbOrigenExtML.Value.Value + item.CrOrigenLocML.Value.Value + item.CrOrigenExtML.Value.Value
                                              + item.DbSaldoIniLocML.Value.Value + item.DbSaldoIniExtML.Value.Value + item.CrSaldoIniLocML.Value.Value + item.CrSaldoIniExtML.Value.Value;
                        if (_bc.AdministrationModel.MultiMoneda)
                            item.SaldoTotalME.Value = item.DbOrigenLocME.Value.Value + item.DbOrigenExtME.Value.Value + item.CrOrigenLocME.Value.Value + item.CrOrigenExtME.Value.Value
                                                   + item.DbSaldoIniLocME.Value.Value + item.DbSaldoIniExtME.Value.Value + item.CrSaldoIniLocME.Value.Value + item.CrSaldoIniExtME.Value.Value;

                        DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetByID(Convert.ToInt32(item.IdentificadorTR.Value.Value));
                        if (doc != null)
                        {
                            item.PrefijoID.Value = doc.PrefijoID.Value;
                            item.LugarGeograficoID.Value = doc.LugarGeograficoID.Value;
                            if (doc.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno)
                                item.PrefDoc.Value = doc.PrefijoID.Value + "-" + doc.DocumentoNro.Value;
                            else
                                item.PrefDoc.Value = doc.DocumentoTercero.Value;
                        } 
                        #endregion
                    }
                }
                #region Filtra y ordena resultados
                DTO_coPlanCuenta dtoCuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, cuentaID, true);
                if (dtoCuenta.OrigenMonetario.Value == 1)//Local
                    this._listCuentaSaldosDoc = this._listCuentaSaldosDoc.Where(x => x.SaldoTotalML.Value.Value != 0).ToList();
                else if (dtoCuenta.OrigenMonetario.Value == 2) //Extranjera
                    this._listCuentaSaldosDoc = this._listCuentaSaldosDoc.Where(x => x.SaldoTotalME.Value.Value != 0).ToList();
                this._listCuentaSaldosDoc = this._listCuentaSaldosDoc.OrderBy(x => x.PrefDoc.Value).ToList(); 
                #endregion
                #region Actualiza grilla
                this.pgGrid.UpdatePageNumber(this._listCuentaSaldosDoc.Count, false, true, false);
                this.gcDocument.DataSource = this._listCuentaSaldosDoc;
                this.gcDocument.RefreshDataSource();
                if (this._listCuentaSaldosDoc.Count > 0)
                    this._cuentaSaldo = this._listCuentaSaldosDoc[0];
                else
                    this._cuentaSaldo = null; 
                #endregion
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSaldosDocument.cs", "LoadData"));
            }
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Carga la Data en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Selecciona el activo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this._listCuentaSaldosDoc.Count > 0)
                this._cuentaSaldo = this._listCuentaSaldosDoc[e.FocusedRowHandle];
        }

        /// <summary>
        /// Se realiza al digitar una tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDocument_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Se ejecuta al seleccionar registro de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editChek_CheckedChanged(object sender, EventArgs e)
        {
            int index = this.gvDocument.FocusedRowHandle;
            if (((CheckEdit)sender).Checked)
                this._listCuentaSaldoSelected.Add(this._listCuentaSaldosDoc[index]);
            else
                this._listCuentaSaldoSelected.Remove(this._listCuentaSaldosDoc[index]);
            this.gvDocument.RefreshData();
            this.gcDocument.RefreshDataSource();
        }
        
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.pgGrid.UpdatePageNumber(this._listCuentaSaldosDoc.Count, false, false, false);
        }

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckEdit)sender).Checked)
            {
                this._listCuentaSaldoSelected.AddRange(this._listCuentaSaldosDoc);
                foreach (var doc in this._listCuentaSaldosDoc)
                    doc.Marca.Value = true;
            }
            else
            {
                this._listCuentaSaldoSelected.Clear();
                foreach (var doc in this._listCuentaSaldosDoc)
                    doc.Marca.Value = false;
            }
            this.gcDocument.DataSource  = this._listCuentaSaldosDoc;
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Documento Control Seleccionado
        /// </summary>
        public DTO_coCuentaSaldo CuentaSaldo
        {
            get { return this._cuentaSaldo; }
        }

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<DTO_coCuentaSaldo> ListaSaldosSelected
        {
            get { return this._listCuentaSaldoSelected; }
        }

        /// <summary>
        /// Indica si permite selecionar y retornar varios Docs
        /// </summary>
        public Boolean MultipleSelection
        {
            get;
            set;
        }

        #endregion

    }
}
