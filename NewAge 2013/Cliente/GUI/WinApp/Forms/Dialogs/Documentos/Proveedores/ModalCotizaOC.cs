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
    public partial class ModalCotizaOC : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalCotizaOCForm;
        //private TipoMoneda _tipoMoneda;
        //private decimal _tasaCambio;
        //private DateTime _periodo;
        private List<DTO_prOrdenCompraCotiza> _currentData;
        //private string _tercero;
        private string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;
        public List<DTO_prOrdenCompraCotiza> ReturnList = new List<DTO_prOrdenCompraCotiza>();
        private bool deleteOP = false;
        private bool isValid = false;
        private int _numeroDoc;
        private DateTime _fechaCot;
        #endregion

        /// <summary>
        /// Constructor de la grilla de facturas 
        /// </summary>
        /// <param name="factResumen">Lista de facturas que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los facturas</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ModalCotizaOC(List<DTO_prOrdenCompraCotiza> cortizaciones, int numeroDoc, DateTime fechaCot)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            //variables
            //this._tipoMoneda = tm;
            //this._tasaCambio = tasaCambio;
            //this._periodo = periodo;
            this._currentData = cortizaciones;
            this._numeroDoc = numeroDoc;
            this._fechaCot = fechaCot;
            //this._tercero = terceroID;

            //Carga de datos
            this.AdGridCols();
            this.LoadGridData();
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AdGridCols()
        {
            try
            {

                GridColumn emp = new GridColumn();
                emp.FieldName = this.unboundPrefix + "EmpresaID";
                emp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_EmpresaID");
                emp.UnboundType = UnboundColumnType.String;
                emp.VisibleIndex = 0;
                emp.Width = 100;
                emp.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(emp);

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaCotiza";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_FechaCotiza");
                fecha.UnboundType = UnboundColumnType.String;
                fecha.VisibleIndex = 1;
                fecha.Width = 60;
                fecha.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(fecha);

                GridColumn Direccion = new GridColumn();
                Direccion.FieldName = this.unboundPrefix + "Direccion";
                Direccion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Direccion");
                Direccion.UnboundType = UnboundColumnType.String;
                Direccion.VisibleIndex = 2;
                Direccion.Width = 70;
                Direccion.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(Direccion);

                GridColumn Telefono = new GridColumn();
                Telefono.FieldName = this.unboundPrefix + "Telefono";
                Telefono.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Telefono");
                Telefono.UnboundType = UnboundColumnType.String;
                Telefono.VisibleIndex = 3;
                Telefono.Width = 50;
                Telefono.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(Telefono);

                GridColumn observ = new GridColumn();
                observ.FieldName = this.unboundPrefix + "Observacion";
                observ.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Observacion");
                observ.UnboundType = UnboundColumnType.String;
                observ.VisibleIndex = 3;
                observ.Width = 150;
                observ.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(observ);

                this.gvData.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCotizaOC.cs", "AdGridCols"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                //List<DTO_faFacturacionResumen> facturas = _bc.AdministrationModel.faFacturacion_GetResumen(this._periodo, this._tipoMoneda, this._tercero);
                List<DTO_prOrdenCompraCotiza> newData = new List<DTO_prOrdenCompraCotiza>();

                ////Carga la informacion de los facturas de acuerdo con el tipo de moneda
                //facturas.ForEach(newFact =>
                //{
                //    #region Revisa los facturas que ya estan asignados
                //    this._currentData.ForEach(fact =>
                //    {
                //        bool found = false;
                //        if
                //        (
                //            !found &&
                //            fact.CuentaID.Value == newFact.CuentaID.Value &&
                //            fact.TerceroID.Value == newFact.TerceroID.Value &&
                //            fact.ProyectoID.Value == newFact.ProyectoID.Value &&
                //            fact.CentroCostoID.Value == newFact.CentroCostoID.Value &&
                //            fact.LineaPresupuestoID.Value == newFact.LineaPresupuestoID.Value &&
                //            fact.ConceptoSaldoID.Value == newFact.ConceptoSaldoID.Value &&
                //            fact.ConceptoCargoID.Value == newFact.ConceptoCargoID.Value &&
                //            fact.IdentificadorTR.Value == newFact.IdentificadorTR.Value
                //        )
                //        {
                //            found = true;
                //            newFact.Seleccionar.Value = true;
                //        }
                //    });
                //    #endregion
                //    newData.Add(newFact);
                //});

                //this._currentData = facturas;
                this.gcData.DataSource = this._currentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCotizaOC.cs", "LoadGridData"));
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
                #region EmpresaID
                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "EmpresaID", false, false, false, null);
                if (!validField)
                {
                    MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col),"Empresa"));
                    validRow = false;
                }
                #endregion
                #endregion
                #region Validaciones Otros 
                #region Observacion
                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "Observacion", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region FechaCotiza
                validField = _bc.ValidGridCell(this.gvData, string.Empty, fila, "FechaCotiza", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion

                this.isValid = validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCotizaOC.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            DTO_prOrdenCompraCotiza cot = new DTO_prOrdenCompraCotiza();

            #region Asigna datos a la fila
            cot.EmpresaID.Value = string.Empty;
            cot.FechaCotiza.Value = this._fechaCot;
            cot.NumeroDoc.Value = this._numeroDoc;
            cot.Observacion.Value = string.Empty;   
            cot.Consecutivo.Value = null;                  
            #endregion

            this._currentData.Add(cot);
            this.gvData.RefreshData();
            this.gvData.FocusedRowHandle = this.gvData.DataRowCount - 1;
           
            this.isValid = false;
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
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
         
            if (fieldName == "FechaCotiza")
                e.RepositoryItem = this.editDate;
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_prOrdenCompraCotiza dto = this._currentData.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                //if (fieldName == "Seleccionar")
                //    e.Value = dto.Seleccionar.Value;
                if (fieldName == "EmpresaID")
                    e.Value = dto.EmpresaID.Value;
                if (fieldName == "FechaCotiza")
                    e.Value = dto.FechaCotiza.Value.Value.ToString(FormatString.DB_Date_YYYY_MM_DD);
                if (fieldName == "Observacion")
                    e.Value = dto.Observacion.Value;
                if (fieldName == "Telefono")
                    e.Value = dto.Telefono.Value;
                if (fieldName == "Direccion")
                    e.Value = dto.Direccion.Value;
            }
            if (e.IsSetData)
            {
                //if (fieldName == "Seleccionar")
                //    dto.Seleccionar.Value = Convert.ToBoolean(e.Value);
                if (fieldName == "EmpresaID")
                    dto.EmpresaID.Value = e.Value.ToString();
                if (fieldName == "FechaCotiza")
                    dto.FechaCotiza.Value = Convert.ToDateTime(e.Value);
                if (fieldName == "Observacion")
                    dto.Observacion.Value = e.Value.ToString();
                if (fieldName == "Telefono")
                    dto.Telefono.Value = e.Value.ToString();
                if (fieldName == "Direccion")
                    dto.Direccion.Value = e.Value.ToString();
                //    dto.Seleccionar.Value = Convert.ToInt32(e.Value);
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

            #region Valida la Empresa
            if (fieldName == "EmpresaID")
                validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, fieldName, false, false, false, null);
            #endregion
            #region Valida los valores
            if (fieldName == "Consecutivo")
            {
                validField = _bc.ValidGridCellValue(this.gvData, string.Empty, e.RowHandle, fieldName, true, false, false, false);
            }

            #endregion
            #region Valida Otros
            if (fieldName == "Observacion" || fieldName == "FechaCotiza")
                validField = _bc.ValidGridCell(this.gvData, string.Empty, e.RowHandle, fieldName, false, false, false, null);
            #endregion

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
                if (this.gvData.DataRowCount > 0)
                {
                    this.gvData.PostEditor();
                    bool valid = this.ValidateRow(this.gvData.FocusedRowHandle);
                    this.ReturnVals = true;
                    this.ReturnList = new List<DTO_prOrdenCompraCotiza>();
                    this._currentData.ForEach(a => this.ReturnList.Add(a));                    
                }
                if(!this.gvData.HasColumnErrors)
                    this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
