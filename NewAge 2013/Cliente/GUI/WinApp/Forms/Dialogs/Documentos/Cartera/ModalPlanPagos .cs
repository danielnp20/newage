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
using System.Text.RegularExpressions;
using NewAge.DTO.Resultados;
using DevExpress.Utils;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalPlanPagos : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalPlanPagosOC;
        private List<DTO_prContratoPlanPago> _currentData;
        public List<DTO_prContratoPlanPago> ReturnList = new List<DTO_prContratoPlanPago>();
        private string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;       
        private bool deleteOP = false;
        private bool isValid = false;
        private int _numeroDoc;
        private decimal valorDoc = 0;
        private bool onlyEdit = false;
        private bool unsavedChanges = false;
        private bool isPagoVariableInd = false;
        #endregion

        /// <summary>
        /// Constructor por defecto modal independiente
        /// </summary>
        public ModalPlanPagos()
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);
            this.InitControls();
        }

        /// <summary>
        /// Constructor de la modal para documento proveedores 
        /// </summary>
        /// <param name="factResumen">Lista de facturas que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los facturas</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ModalPlanPagos(List<DTO_prContratoPlanPago> polizas, int numeroDoc, DateTime fechaDoc, decimal valorDoc, bool pagoVariableInd)
        {
            try
            {
                //Inicializa el formulario
                InitializeComponent();
                FormProvider.LoadResources(this, this._documentID);

                this._currentData = polizas;
                this._numeroDoc = numeroDoc;
                this.isPagoVariableInd = pagoVariableInd;

                //Carga de datos
                this.AddCols();
                this.LoadGridData();
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "ModalPlanPagos"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddCols()
        {
            try
            {
                GridColumn Fecha = new GridColumn();
                Fecha.FieldName = this.unboundPrefix + "Fecha";
                Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
                Fecha.UnboundType = UnboundColumnType.DateTime;
                Fecha.VisibleIndex = 1;
                Fecha.Width = 80;
                Fecha.OptionsColumn.AllowEdit = this.isPagoVariableInd? true : this.onlyEdit;
                Fecha.ColumnEdit = this.editDate;
                this.gvData.Columns.Add(Fecha);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 2;
                Valor.Width = 130;
                Valor.OptionsColumn.AllowEdit = this.isPagoVariableInd ? true : this.onlyEdit;
                Valor.ColumnEdit = this.editSpin;
                this.gvData.Columns.Add(Valor);

                GridColumn ValorAdicional = new GridColumn();
                ValorAdicional.FieldName = this.unboundPrefix + "ValorAdicional";
                ValorAdicional.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ValorAdicional");
                ValorAdicional.UnboundType = UnboundColumnType.Decimal;
                ValorAdicional.VisibleIndex = 3;
                ValorAdicional.Width = 130;
                ValorAdicional.OptionsColumn.AllowEdit = this.isPagoVariableInd ? true : true;
                ValorAdicional.ColumnEdit = this.editSpin;
                this.gvData.Columns.Add(ValorAdicional);

                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this.unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 4;
                Observacion.OptionsColumn.AllowEdit = this.isPagoVariableInd ? true : this.onlyEdit;
                Observacion.ColumnEdit = this.editMemo;
                Observacion.Width = 200;
                this.gvData.Columns.Add(Observacion);

                this.gvData.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "AddCols"));
            }
        }

        /// <summary>
        /// Inicia los controles al abrir la modal independiente
        /// </summary>
        private void InitControls()
        {
            this.onlyEdit = true;
            this._bc.InitMasterUC(this.masterPrefijoOC, AppMasters.glPrefijo, false, true, true, true);
            this.masterPrefijoOC.Visible = true;
            this.lblPrefijoOC.Visible = true;
            this.lblDocumentNro.Visible = true;
            this.txtDocumentoNro.Visible = true;
            this.btnGet.Visible = true;
            this.btnQueryDoc.Visible = true;
            this.lblValorDoc.Visible = true;
            this.txtValorDoc.Visible = true;
            this.btnAccept.Text = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_btnSave");
            this.AddCols();
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {

                if (!this.onlyEdit)
                {
                    List<DTO_prContratoPlanPago> newData = new List<DTO_prContratoPlanPago>();
                    this.gcData.DataSource = this._currentData;
                }
                else
                {
                    DTO_prOrdenCompra orden = this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.OrdenCompra, this.masterPrefijoOC.Value, Convert.ToInt32(this.txtDocumentoNro.Text));
                    if (orden != null)
                    {
                        this._currentData = orden != null ? orden.ContratoPlanPagos : new List<DTO_prContratoPlanPago>();
                        this.gcData.DataSource = this._currentData;
                        this._numeroDoc = orden.DocCtrl.NumeroDoc.Value.Value;
                        this.valorDoc = orden.DocCtrl.Valor.Value.Value;
                        this.txtValorDoc.EditValue = this.valorDoc;
                    }
                    else
                    { 
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Pr_NotExistOrdenCompra));
                        this._currentData =  new List<DTO_prContratoPlanPago>();
                        this.gcData.DataSource = this._currentData;
                        this.isValid = false;
                        this._numeroDoc = 0;
                        this.valorDoc = 0;
                        this.txtValorDoc.EditValue = this.valorDoc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "LoadGridData"));
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
            GridColumn col = null;
            try
            {
                DTO_prContratoPlanPago currentRow = (DTO_prContratoPlanPago)this.gvData.GetFocusedRow();
                #region Fecha
                string msg = currentRow != null && currentRow.Fecha.Value != null ? string.Empty : "Fecha Vacio";
                col = this.gvData.Columns[this.unboundPrefix + "Fecha"];
                if (!string.IsNullOrEmpty(msg))
                {
                    this.gvData.SetColumnError(col, msg);
                    validRow = false;
                }
                else
                {
                    #region Valida que la fecha sea correcta
                    int indexPrevious = fila - 1;
                    DateTime? datePrevious = indexPrevious > 0 && this.gvData.FocusedRowHandle != 0 ? this._currentData[indexPrevious].Fecha.Value : null;
                    if (datePrevious != null && datePrevious > Convert.ToDateTime(this._currentData[fila].Fecha.Value))
                    {
                        msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_InvalidDateCurrent);
                        this.gvData.SetColumnError(col, msg);
                        validRow = false;
                    } 
	                #endregion                
                }
                #endregion                
                #region Valor
                validField = _bc.ValidGridCellValue(this.gvData, string.Empty, fila, "Valor", false, false, false, false);
                if (!validField)
                    validRow = false;
                else
                {
                    decimal valorPlanPagos = this._currentData.Sum(x => x.Valor.Value.Value + x.ValorAdicional.Value.Value);
                    if (valorPlanPagos > this.valorDoc)
                    {
                        col = this.gvData.Columns[this.unboundPrefix + "Valor"];
                        this.gvData.SetColumnError(col, this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_InvalidValueCurrent));
                        validField = false;
                    }
                }
                #endregion
                #region ValorAdicional
                validField = _bc.ValidGridCellValue(this.gvData, string.Empty, fila, "ValorAdicional", false, true, false, false);
                if (!validField)
                    validRow = false;
                else
                {
                    decimal valorPlanPagos = this._currentData.Sum(x => x.Valor.Value.Value + x.ValorAdicional.Value.Value);
                    if (valorPlanPagos > this.valorDoc)
                    {
                        col = this.gvData.Columns[this.unboundPrefix + "ValorAdicional"];
                        this.gvData.SetColumnError(col, this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_InvalidValueCurrent));
                        validField = false;
                    }
                }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            DTO_prContratoPlanPago cot = new DTO_prContratoPlanPago();
            try
            {

                #region Asigna datos a la fila
                cot.Fecha.Value = null;
                cot.Valor.Value = 0;
                cot.ValorAdicional.Value = 0;
                cot.Observacion.Value = string.Empty;
                #endregion

                this._currentData.Add(cot);
                this.gvData.RefreshData();
                this.gvData.FocusedRowHandle = this.gvData.DataRowCount - 1;

                this.isValid = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "AddNewRow"));
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
            if (this._numeroDoc != 0 || this.isPagoVariableInd)
            {
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    this.deleteOP = false;
                    if (this.isValid || this.gvData.RowCount == 0)
                        this.AddNewRow();
                    else
                    {
                        this.gvData.PostEditor();
                        bool isV = this.ValidateRow(this.gvData.FocusedRowHandle);
                        if (isV)
                            this.AddNewRow();
                    }
                }
                else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove && this.gvData.RowCount > 0 && this.isPagoVariableInd)
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
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_prContratoPlanPago dto = this._currentData.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {                           
                if (fieldName == "Fecha")
                    e.Value = dto.Fecha.Value;
                if (fieldName == "Valor")
                    e.Value = dto.Valor.Value;
                if (fieldName == "ValorAdicional")
                    e.Value = dto.ValorAdicional.Value;
                if (fieldName == "Observacion")
                    e.Value = dto.Observacion.Value;
            }
            if (e.IsSetData)
            {
                if (fieldName == "Fecha")
                    dto.Fecha.Value = Convert.ToDateTime(e.Value);
                if (fieldName == "Valor")
                    dto.Valor.Value = Convert.ToDecimal(e.Value);
                if (fieldName == "ValorAdicional")
                    dto.ValorAdicional.Value = Convert.ToDecimal(e.Value);
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
                if (fieldName == "Fecha")
                    this.unsavedChanges = true;

                if (fieldName == "Valor")
                {
                    validField = this._bc.ValidGridCellValue(this.gvData, string.Empty, e.RowHandle, fieldName, true, false, false, false);
                    if (validField)
                        this.unsavedChanges = true;
                }  

                if (fieldName == "ValorAdicional")
                {
                    validField = this._bc.ValidGridCellValue(this.gvData, string.Empty, e.RowHandle, fieldName, true, true, false, false);
                    if (validField)
                        this.unsavedChanges = true;
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "gvData_CellValueChanged")); 
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
        /// Devuelve el registro seleccionado
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gvData.RowCount > 0 && this.ValidateRow(this.gvData.FocusedRowHandle))
                {
                    if (!this.onlyEdit)
                    {
                        this.ReturnVals = true;
                        this._currentData.ForEach(a => this.ReturnList.Add(a));

                        this.Close(); 
                    }
                    else 
                    {
                        DTO_TxResult result = this._bc.AdministrationModel.prContratoPlanPago_Upd(this._currentData,this._numeroDoc);
                        if (result.Result == ResultValue.NOK)
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_NotUpdatePlanPagos));
                            this.isValid = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalPlanPagos.cs", "btnReturn_Click"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDocumentoNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Al cerrrar el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void ModalPlanPagos_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.onlyEdit)
            {
                if (this._currentData.Count > 0 && !this.ValidateRow(this.gvData.FocusedRowHandle))
                    this._currentData.RemoveAt(this.gvData.FocusedRowHandle); 
            }
            else if (this.unsavedChanges)
            { 
                
            }
        }
        
        /// <summary>
        /// Consulta la informacion de la orden de Compra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            if (this.masterPrefijoOC.ValidID && !string.IsNullOrEmpty(this.txtDocumentoNro.Text))
                this.LoadGridData();
        }

        /// <summary>
        /// Consulta la informacion de la orden de Compra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.OrdenCompra);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs,false,false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {      
                this.txtDocumentoNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.masterPrefijoOC.Value = getDocControl.DocumentoControl.PrefijoID.Value;                   
            }
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
