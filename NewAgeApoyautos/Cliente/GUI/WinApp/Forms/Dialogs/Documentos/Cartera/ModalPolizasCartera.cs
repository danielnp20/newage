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
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalPolizasCartera : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int _pageSize = 50;
        private bool _filterActive = false;
        private bool _soloSinLiquidar = false;
        //Variables de data
        private DTO_ccPolizaEstado _rowCurrent = new DTO_ccPolizaEstado();
        private List<DTO_ccPolizaEstado> _listPolizas = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public DTO_ccPolizaEstado RowSelected
        {
            get { return this._rowCurrent;}
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ModalPolizasCartera(string tercero, string libranza, bool soloSinLiquidar = false)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                _bc.Pagging_Init(this.pgGrid, _pageSize);
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.AddGridCols();
                this.masterTercero.Value = tercero;
                this.txtLibranza.Text = libranza;
                this._soloSinLiquidar = soloSinLiquidar;
                this.LoadGridData();                  
                FormProvider.LoadResources(this, AppForms.ModalPolizaCartera);
                this.Text = this._bc.GetResource(LanguageTypes.Forms, "1047");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalPolizasCartera.cs", "ModalPolizasCartera: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalPolizaCartera;
            this._frmModule = ModulesPrefix.cc;

            #region Inicializa Controles
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero,false,true, false, false);
            #endregion            
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            GridColumn Poliza = new GridColumn();
            Poliza.FieldName = this._unboundPrefix + "Poliza";
            Poliza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Poliza");
            Poliza.UnboundType = UnboundColumnType.String;
            Poliza.VisibleIndex = 0;
            Poliza.Width = 70;
            Poliza.Visible = true;
            Poliza.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(Poliza);

            GridColumn TerceroID = new GridColumn();
            TerceroID.FieldName = this._unboundPrefix + "TerceroID";
            TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
            TerceroID.UnboundType = UnboundColumnType.String;
            TerceroID.VisibleIndex = 1;
            TerceroID.Width = 65;
            TerceroID.Visible = true;
            TerceroID.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(TerceroID);

            GridColumn Nombre = new GridColumn();
            Nombre.FieldName = this._unboundPrefix + "Nombre";
            Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
            Nombre.UnboundType = UnboundColumnType.String;
            Nombre.VisibleIndex = 2;
            Nombre.Width = 170;
            Nombre.Visible = true;
            Nombre.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(Nombre);

            GridColumn Libranza = new GridColumn();
            Libranza.FieldName = this._unboundPrefix + "Libranza";
            Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
            Libranza.UnboundType = UnboundColumnType.String;
            Libranza.VisibleIndex = 3;
            Libranza.Width = 50;
            Libranza.Visible = true;
            Libranza.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(Libranza);  

            GridColumn AseguradoraID = new GridColumn();
            AseguradoraID.FieldName = this._unboundPrefix + "AseguradoraID";
            AseguradoraID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_AseguradoraID");
            AseguradoraID.UnboundType = UnboundColumnType.String;
            AseguradoraID.VisibleIndex = 4;
            AseguradoraID.Width = 60;
            AseguradoraID.Visible = true;
            AseguradoraID.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(AseguradoraID);  

            GridColumn FechaVigenciaINI = new GridColumn();
            FechaVigenciaINI.FieldName = this._unboundPrefix + "FechaVigenciaINI";
            FechaVigenciaINI.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVigenciaINI");
            FechaVigenciaINI.UnboundType = UnboundColumnType.DateTime;
            FechaVigenciaINI.VisibleIndex = 5;
            FechaVigenciaINI.Width = 60;
            FechaVigenciaINI.Visible = true;
            FechaVigenciaINI.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(FechaVigenciaINI);  

            GridColumn FechaVigenciaFIN = new GridColumn();
            FechaVigenciaFIN.FieldName = this._unboundPrefix + "FechaVigenciaFIN";
            FechaVigenciaFIN.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVigenciaFIN");
            FechaVigenciaFIN.UnboundType = UnboundColumnType.DateTime;
            FechaVigenciaFIN.VisibleIndex = 6;
            FechaVigenciaFIN.Width = 60;
            FechaVigenciaFIN.Visible = true;
            FechaVigenciaFIN.OptionsColumn.AllowEdit = false;
            this.gvPoliza.Columns.Add(FechaVigenciaFIN);  
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                DTO_ccPolizaEstado polizaTemp = new DTO_ccPolizaEstado();
                polizaTemp.TerceroID.Value = this.masterTercero.Value;
                polizaTemp.AnuladaIND.Value = false;
                polizaTemp.Poliza.Value = this.txtPoliza.Text;
                polizaTemp.Libranza.Value = !string.IsNullOrEmpty(this.txtLibranza.Text) ? Convert.ToInt32(this.txtLibranza.Text) : polizaTemp.Libranza.Value;
                this._listPolizas = this._bc.AdministrationModel.PolizaEstado_GetByParameter(polizaTemp);
                if (this._soloSinLiquidar)
                    this._listPolizas = this._listPolizas.FindAll(x => x.NumeroDocLiquida.Value == null);
                this._listPolizas = this._listPolizas.OrderBy(x => x.TerceroID.Value).ToList();
                this.pgGrid.UpdatePageNumber(this._listPolizas.Count, true, true, false);

                var tmp = this._listPolizas.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ccPolizaEstado>();
                this.gvPoliza.MoveFirst();
                this.gcPoliza.DataSource = tmp;
                this.gcPoliza.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPolizasCartera.cs", "ModalPolizasCartera(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
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
            try
            {
                var tmp = this._filterActive ? this._listPolizas.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ccPolizaEstado>() :
                                                this._listPolizas.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ccPolizaEstado>(); 
                this.pgGrid.UpdatePageNumber(this._listPolizas.Count, false, false, false);
                this.gvPoliza.MoveFirst();
                tmp = tmp.OrderBy(x => x.TerceroID.Value).ToList();
                this.gcPoliza.DataSource = tmp;

                if (this.gvPoliza.DataRowCount > 0)
                    this._rowCurrent = (DTO_ccPolizaEstado)this.gvPoliza.GetRow(this.gvPoliza.FocusedRowHandle);
                else
                    this._rowCurrent = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPolizasCartera.cs", "pagging_Click: " + ex.Message)); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.LoadGridData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnFilter_Click(this.btnFilter, e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if(this.gvPoliza.DataRowCount > 0)
                this._rowCurrent = (DTO_ccPolizaEstado)this.gvPoliza.GetRow(this.gvPoliza.FocusedRowHandle);
            this.Close();
        }

        /// <summary>
        /// Al hacer click para cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._rowCurrent = null;
            this.Close();
        }

        /// <summary>
        /// Valida el texto ingresado
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPoliza_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

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
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPoliza_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this.gvPoliza.DataRowCount > 0)
                    this._rowCurrent = (DTO_ccPolizaEstado)this.gvPoliza.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPolizasCartera.cs", "gvPoliza_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPoliza_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0 && this.gvPoliza.DataRowCount > 0)
                    this._rowCurrent = (DTO_ccPolizaEstado)this.gvPoliza.GetRow(e.RowHandle);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPolizasCartera.cs", "gvPoliza_RowClick: " + ex.Message));
            }
        }

        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPoliza_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion      
    
    }
}
