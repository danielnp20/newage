using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalPolizaProveedor : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalPolizasProveedor;
        private List<DTO_prContratoPolizas> _currentData;
        //private string _tercero;
        private string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;
        public List<DTO_prContratoPolizas> ReturnList = new List<DTO_prContratoPolizas>();
        private bool deleteOP = false;
        private bool isValid = false;
        private int _numeroDoc;
        private DateTime _fechaCot;
        private string monedaLocal = string.Empty;
        private string monedaExtr = string.Empty;
        #endregion

        /// <summary>
        /// Constructor de la grilla de facturas 
        /// </summary>
        /// <param name="factResumen">Lista de facturas que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los facturas</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ModalPolizaProveedor(List<DTO_prContratoPolizas> polizas, int numeroDoc)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            this._currentData = polizas;
            this._numeroDoc = numeroDoc;

            //Carga de datos
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtr = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.AddCols();
            this.LoadGridData();
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddCols()
        {
            try
            {
                GridColumn CubrimientoPolizaID = new GridColumn();
                CubrimientoPolizaID.FieldName = this.unboundPrefix + "CubrimientoPolizaID";
                CubrimientoPolizaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CubrimientoPolizaID");
                CubrimientoPolizaID.UnboundType = UnboundColumnType.String;
                CubrimientoPolizaID.VisibleIndex = 1;
                CubrimientoPolizaID.Width = 120;
                CubrimientoPolizaID.ColumnEdit = this.editBtnGrid;
                this.gvData.Columns.Add(CubrimientoPolizaID);

                GridColumn FechaVto = new GridColumn();
                FechaVto.FieldName = this.unboundPrefix + "FechaVto";
                FechaVto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_FechaVto");
                FechaVto.UnboundType = UnboundColumnType.DateTime;
                FechaVto.VisibleIndex = 2;
                FechaVto.Width = 80;
                FechaVto.ColumnEdit = this.editDate;
                this.gvData.Columns.Add(FechaVto);

                GridColumn PorCubrimiento = new GridColumn();
                PorCubrimiento.FieldName = this.unboundPrefix + "PorCubrimiento";
                PorCubrimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_PorCubrimiento");
                PorCubrimiento.UnboundType = UnboundColumnType.Decimal;
                PorCubrimiento.VisibleIndex = 3;
                PorCubrimiento.Width = 80;
                PorCubrimiento.ColumnEdit = this.editSpin;
                this.gvData.Columns.Add(PorCubrimiento);

                GridColumn VlrCubrimiento = new GridColumn();
                VlrCubrimiento.FieldName = this.unboundPrefix + "VlrCubrimiento";
                VlrCubrimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_VlrCubrimiento");
                VlrCubrimiento.UnboundType = UnboundColumnType.Decimal; 
                VlrCubrimiento.VisibleIndex = 4;
                VlrCubrimiento.Width = 130;
                VlrCubrimiento.ColumnEdit = this.editSpin;
                this.gvData.Columns.Add(VlrCubrimiento);

                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this.unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 4;
                Observacion.Width = 200;
                this.gvData.Columns.Add(Observacion);

                this.gvData.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPolizaProveedor.cs", "AddCols"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                List<DTO_prContratoPolizas> newData = new List<DTO_prContratoPolizas>();
                this.gcData.DataSource = this._currentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "LoadGridData"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                modal.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
               
                #region CubrimientoPolizaID

                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "CubrimientoPolizaID", false, true, false, AppMasters.prPolizaCubrimiento);
                if (!validField)
                    validRow = false;
                #endregion                               
                #region FechaVto
                DTO_prContratoPolizas pol = (DTO_prContratoPolizas)this.gvData.GetFocusedRow();
                string msg = pol != null && pol.FechaVto.Value != null ? string.Empty : "Fecha Vencimiento Vacio";
                GridColumn col = this.gvData.Columns[this.unboundPrefix + "FechaVto"];
                this.gvData.SetColumnError(col, msg);
                validField = string.IsNullOrEmpty(msg) ? true : false;
                if (!validField)
                    validRow = false;
                #endregion
                #region PorCubrimiento
                validField = _bc.ValidGridCellValue(this.gvData, string.Empty, fila, "PorCubrimiento", false, false, false, false);
                if (!validField)
                    validRow = false;
                if (validField)
                {
                    msg = pol != null && pol.PorCubrimiento.Value > 999 ? "El pocentaje debe ser menor a 1000" : string.Empty;
                    col = this.gvData.Columns[this.unboundPrefix + "PorCubrimiento"];
                    this.gvData.SetColumnError(col, msg);
                    validRow = string.IsNullOrEmpty(msg) ? true : false;
                }
                #endregion
                #region VlrCubrimiento
                validField = _bc.ValidGridCellValue(this.gvData, string.Empty, fila, "VlrCubrimiento", false, false, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region Observacion
                validField  = _bc.ValidGridCell(this.gvData, string.Empty, fila, "Observacion", false, false, false,null);
                if (!validField)
                    validRow = false;
                #endregion
                this.isValid = validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            DTO_prContratoPolizas cot = new DTO_prContratoPolizas();
            try
            {

                #region Asigna datos a la fila
                cot.CubrimientoPolizaID.Value = null;
                cot.FechaVto.Value = null;
                cot.PorCubrimiento.Value = 0;
                cot.VlrCubrimiento.Value = 0;
                cot.Observacion.Value = string.Empty;
                #endregion

                this._currentData.Add(cot);
                this.gvData.RefreshData();
                this.gvData.FocusedRowHandle = this.gvData.DataRowCount - 1;

                this.isValid = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "AddNewRow"));
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcData_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
            {
                this.deleteOP = false;
                if (this.isValid || this.gvData.RowCount==0)                    
                    this.AddNewRow();
                else
                {
                    this.gvData.PostEditor();
                    bool isV = this.ValidateRow(this.gvData.FocusedRowHandle);
                    if (isV)
                        this.AddNewRow();
                }
            }
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove && this.gvData.RowCount > 0)
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                //Revisa si desea cargar los temporales
                if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.deleteOP = true;
                    int rowHandle = this.gvData.FocusedRowHandle;

                    this.gvData.DeleteRow(this.gvData.FocusedRowHandle);

                    if (this.gvData.RowCount > 0)
                    {
                        //Si borra el primer registro
                        if (rowHandle == 0)
                            this.gvData.FocusedRowHandle = 0;
                        //Si selecciona el ultimo
                        else
                            this.gvData.FocusedRowHandle = rowHandle - 1;
                    }
                    else
                        this.gcData.Focus();
                }
                e.Handled = true;
            }            
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_prContratoPolizas dto = this._currentData.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {              
                if (fieldName == "CubrimientoPolizaID")
                    e.Value = dto.CubrimientoPolizaID.Value;
                if (fieldName == "FechaVto")
                    e.Value = dto.FechaVto.Value;
                if (fieldName == "PorCubrimiento")
                    e.Value = dto.PorCubrimiento.Value;
                if (fieldName == "VlrCubrimiento")
                    e.Value = dto.VlrCubrimiento.Value;
                if (fieldName == "Observacion")
                    e.Value = dto.Observacion.Value;
            }
            if (e.IsSetData)
            {
                if (fieldName == "CubrimientoPolizaID")
                    dto.CubrimientoPolizaID.Value = e.Value.ToString();
                if (fieldName == "FechaVto")
                    dto.FechaVto.Value = Convert.ToDateTime(e.Value);
                if (fieldName == "PorCubrimiento")
                    dto.PorCubrimiento.Value = Convert.ToDecimal(e.Value);
                if (fieldName == "VlrCubrimiento")
                    dto.VlrCubrimiento.Value = Convert.ToDecimal(e.Value);
                if (fieldName == "Observacion")
                    dto.Observacion.Value = e.Value.ToString();
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            int index = e.RowHandle;

            bool validField = true;

            try
            {
                if (fieldName == "CubrimientoPolizaID")
                    validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.prPolizaCubrimiento);
                if (fieldName == "PorCubrimiento")
                {
                    validField = _bc.ValidGridCellValue(this.gvData, string.Empty, e.RowHandle, fieldName, true, false, false, false);
                    if (validField)
                    {
                        decimal value = Convert.ToDecimal(e.Value);
                        if (value > 999)
                        {
                            this.gvData.SetColumnError(e.Column, "El pocentaje debe ser menor a 1000");
                            validField = false;
                        }
                    }
                }
                if (fieldName == "VlrCubrimiento")
                    validField = _bc.ValidGridCellValue(this.gvData, string.Empty, e.RowHandle, fieldName, true, false, false, false);
                if (fieldName == "Observacion")
                    validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, "Observacion", false, false, false, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "gvData_CellValueChanged")); 
            }
            if (!validField)
                this.isValid = false;
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {           
                bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                this.deleteOP = false;

                if (validRow)
                {
                    this.isValid = true;
                }
                else
                {
                    e.Allow = false;
                    this.isValid = false;
                }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el foco de una columna a otra
        /// Cambia el estilo de una celda segun las condiciones del formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            this.gvData.Appearance.FocusedCell.BackColor = Color.White;
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvData.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvData.FocusedRowHandle, colName, origin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Devuelve el registro seleccionado
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateRow(this.gvData.FocusedRowHandle))
                {
                    this.ReturnVals = true;
                    this._currentData.ForEach(a => this.ReturnList.Add(a));

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Al cerrrar el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void ModalPolizaProveedor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this._currentData.Count > 0 && !this.ValidateRow(this.gvData.FocusedRowHandle))
                this._currentData.RemoveAt(this.gvData.FocusedRowHandle);
        }

        /// <summary>
        /// Consulta la informacion de la orden de Compra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void toolTip_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            try
            {
                GridHitInfo info = this.gvData.CalcHitInfo(e.ControlMousePosition);
                if (info.Column != null && info.Column.FieldName == this.unboundPrefix + "Observacion" && info.RowHandle >= 0)
                {
                    string val = this.gvData.GetRowCellValue(info.RowHandle, info.Column).ToString();
                    toolTip.ShowHint(val);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "ttControler_GetActiveObjectInfo"));
            }
        }


        #endregion

    
    }
}
