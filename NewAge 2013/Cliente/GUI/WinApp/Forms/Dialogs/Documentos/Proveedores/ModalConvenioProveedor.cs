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
    public partial class ModalConvenioProveedor : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalConvenio;
        private List<DTO_prConvenio> _currentData;
        //private string _tercero;
        private string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;
        public List<DTO_prConvenio> ReturnList = new List<DTO_prConvenio>();
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
        public ModalConvenioProveedor(List<DTO_prConvenio> convenios, int numeroDoc, bool onlyQuery = false)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            //Valida Solo Consulta
            if (onlyQuery) 
            {
                this.gcData.EmbeddedNavigator.CustomButtons[0].Visible = false;
                this.gcData.EmbeddedNavigator.Buttons.Remove.Visible = false;
                this.gvData.OptionsBehavior.Editable = false;
            }

            this._currentData = convenios;
            this._numeroDoc = numeroDoc;

            //Carga de datos
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtr = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.AddGridCols();
            this.LoadGridData();
            
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Campo de seleccion
                GridColumn sel = new GridColumn();
                sel.FieldName = this.unboundPrefix + "Seleccionar";
                sel.UnboundType = UnboundColumnType.Boolean;
                sel.VisibleIndex = 0;
                sel.Width = 25;
                sel.Visible = true;
                sel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                sel.OptionsColumn.ShowCaption = false;
                this.gvData.Columns.Add(sel);

                GridColumn codigoBS = new GridColumn();
                codigoBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codigoBS.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CodigoBSID");
                codigoBS.UnboundType = UnboundColumnType.String;
                codigoBS.VisibleIndex = 1;
                codigoBS.Width = 100;
                codigoBS.ColumnEdit = this.editBtnGrid;
                this.gvData.Columns.Add(codigoBS);

                GridColumn referencia = new GridColumn();
                referencia.FieldName = this.unboundPrefix + "inReferenciaID";
                referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_inReferenciaID");
                referencia.UnboundType = UnboundColumnType.String;
                referencia.VisibleIndex = 2;
                referencia.Width = 100;
                referencia.ColumnEdit = this.editBtnGrid;
                this.gvData.Columns.Add(referencia);

                GridColumn monedaID = new GridColumn();
                monedaID.FieldName = this.unboundPrefix + "MonedaID";
                monedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_MonedaID");
                monedaID.UnboundType = UnboundColumnType.String;
                monedaID.VisibleIndex = 3;
                monedaID.Width = 80;
                monedaID.ColumnEdit = this.editBtnGrid;
                this.gvData.Columns.Add(monedaID);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 4;
                valor.Width = 100;
                valor.ColumnEdit = this.editSpin;
                this.gvData.Columns.Add(valor);

                this.gvData.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "AddCols"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                List<DTO_prConvenio> newData = new List<DTO_prConvenio>();              
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
                GridColumn col = new GridColumn();
                #region Validacion de nulls y Fks
                #region CodigoBSID
                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "CodigoBSID", false, true, true, AppMasters.prBienServicio);
                if (!validField)
                    validRow = false;
                #endregion
                #region inReferenciaID
                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "inReferenciaID", false, true, false, AppMasters.inReferencia);
                if (!validField)
                    validRow = false;
                #endregion
                #region MonedaID
                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "MonedaID", false, true, false, AppMasters.glMoneda);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Validaciones de valores
                #region Valor
                validField = _bc.ValidGridCellValue(this.gvData, string.Empty, fila, "Valor", true, false, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Valida que cada registro sea unico
                var validPKUnique = this._currentData.Count(x => x.CodigoBSID.Value == this._currentData[fila].CodigoBSID.Value &&
                                                                 x.inReferenciaID.Value == this._currentData[fila].inReferenciaID.Value);
                if (validPKUnique > 1)
                {
                    col = this.gvData.Columns[this.unboundPrefix + "CodigoBSID"];
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_CodigoBSAlreadyExist), this._currentData[fila].CodigoBSID.Value, this._currentData[fila].inReferenciaID.Value);
                    this.gvData.SetColumnError(col, msg);
                    validRow = false;
                }
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
            DTO_prConvenio cot = new DTO_prConvenio();
            try
            {

                #region Asigna datos a la fila
                cot.CodigoBSID.Value = null;
                cot.inReferenciaID.Value = null;
                cot.MonedaID.Value = this._bc.AdministrationModel.MultiMoneda? this.monedaExtr: this.monedaLocal;
                cot.Valor.Value = 0;
                cot.Seleccionar.Value = true;
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
            DTO_prConvenio dto = this._currentData.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Seleccionar")
                    e.Value = dto.Seleccionar.Value;
                if (fieldName == "CodigoBSID")
                    e.Value = dto.CodigoBSID.Value;
                if (fieldName == "inReferenciaID")
                    e.Value = dto.inReferenciaID.Value;
                if (fieldName == "MonedaID")
                    e.Value = dto.MonedaID.Value;
                if (fieldName == "Valor")
                    e.Value = dto.Valor.Value;
            }
            if (e.IsSetData)
            {
                if (fieldName == "Seleccionar")
                    dto.Seleccionar.Value = Convert.ToBoolean(e.Value);
                if (fieldName == "CodigoBSID")
                    dto.CodigoBSID.Value = e.Value.ToString();
                if (fieldName == "inReferenciaID")
                    dto.inReferenciaID.Value = e.Value.ToString();
                if (fieldName == "MonedaID")
                    dto.MonedaID.Value = e.Value.ToString();
                if (fieldName == "Valor")
                    dto.Valor.Value = Convert.ToDecimal(e.Value);
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
                #region Se modifican FKs
                if (fieldName == "CodigoBSID")
                {
                    validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.prBienServicio);
                    #region Valida el Tipo de CodigoBS
                    if (validField)
                    {
                        DTO_prBienServicio bienServicio = (DTO_prBienServicio)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, e.Value.ToString(), true);
                        DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bienServicio != null ? bienServicio.ClaseBSID.Value : string.Empty, true);
                        if (bienServicioClase != null)
                        {
                            #region Clase Servicios
                            if (bienServicioClase.TipoCodigo.Value == (byte)TipoCodigo.Servicio || bienServicioClase.TipoCodigo.Value == (byte)TipoCodigo.Suministros || bienServicioClase.TipoCodigo.Value == (byte)TipoCodigo.SuministroPersonal)
                            {
                                string refxDefecto = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                                this._currentData[e.RowHandle].inReferenciaID.Value = refxDefecto;
                                this.gvData.Columns[this.unboundPrefix + "inReferenciaID"].OptionsColumn.AllowEdit = false;
                            }
                            #endregion
                            #region Clase Inventarios
                            else if (bienServicioClase.TipoCodigo.Value == (byte)TipoCodigo.Inventario)
                            {
                                this.gvData.Columns[this.unboundPrefix + "inReferenciaID"].OptionsColumn.AllowEdit = true;
                            }
                            #endregion
                            #region Clase Activos
                            else if (bienServicioClase.TipoCodigo.Value == (byte)TipoCodigo.Activo)
                            {
                                string refxDefecto = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                                this._currentData[e.RowHandle].inReferenciaID.Value = refxDefecto;
                                this.gvData.Columns[this.unboundPrefix + "inReferenciaID"].OptionsColumn.AllowEdit = true;
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                if (fieldName == "inReferenciaID")
                    validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, fieldName, true, true, false, AppMasters.inReferencia);
                if (fieldName == "MonedaID")
                    validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.glMoneda);

                #endregion
                #region Se modifican Valor
                if (fieldName == "Valor")
                    validField = _bc.ValidGridCellValue(this.gvData, string.Empty, e.RowHandle, fieldName, true, false, false, false);

                #endregion
                #region Valida que cada registro sea unico
                var validPKUnique = this._currentData.Count(x => x.CodigoBSID.Value == this._currentData[e.RowHandle].CodigoBSID.Value &&
                                                                       x.inReferenciaID.Value == this._currentData[e.RowHandle].inReferenciaID.Value);
                if (validPKUnique > 1)
                {
                    GridColumn col = this.gvData.Columns[unboundPrefix + fieldName];
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_CodigoBSAlreadyExist), this._currentData[e.RowHandle].CodigoBSID.Value, this._currentData[e.RowHandle].inReferenciaID.Value);
                    this.gvData.SetColumnError(col, msg);
                    this.isValid = false;
                } 
                #endregion
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
        private void ModalConvenioProveedor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this._currentData.Count > 0 && !this.ValidateRow(this.gvData.FocusedRowHandle))
                this._currentData.RemoveAt(this.gvData.FocusedRowHandle);
        }

        #endregion
   
    }
}
