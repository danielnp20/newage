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
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalMaster : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = 0;
        private string _functionCount = string.Empty;
        private string _functionData = string.Empty;
        private Object[] _args = new Object[] { };
        private string _colId = string.Empty;
        private string _colDesc = string.Empty;
        private ButtonEdit _returnEdit; // Para buscarlo desde una grilla
        private TextBox _returnCod; // Para buscarlo desde un control
        private string _empresaGrupoID;
        private IEnumerable<DTO_MasterBasic> _dtoList;
        private string _currentText;
        private DTO_glConsulta _filtrosComplejos = new DTO_glConsulta();
        private List<DTO_glConsultaFiltro> _filtrosModal = new List<DTO_glConsultaFiltro>();
        
        //Propiedades de la grilla
        private int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["Pagging.Master.PageSize.Modal"]);
        private string _sortField = string.Empty;

        //Grilla y dato de retorno
        private string unboundPrefix = "Unbound_";
        public string returnValue = string.Empty;

        #endregion

        /// <summary>
        /// Constructor de la amestar con todos los datos requeridos 
        /// </summary>
        /// <param name="origin">Boton que realiza el llamado</param>
        /// <param name="code">Codigo del formulario</param>
        /// <param name="fCount">Nombre de la función que cuenta el numero de registros</param>
        /// <param name="fData">Nombre de la función que trae la información paginada</param>
        /// <param name="fArgs">Nombre de los argumentos</param>
        /// <param name="colId">Nombre de la columna del código</param>
        /// <param name="colDesc">Nombre de la columna de la descripción</param>
        /// <param name="isHier">Indica si es una maestra jerarquica</param>
        public ModalMaster(ButtonEdit origin, string code, string fCount, string fData, Object[] fArgs, string colId, string colDesc, bool isHier, List<DTO_glConsultaFiltro> filtros = null)
        {
            //Variables
            this._documentID = Convert.ToInt32(code);
            if (filtros != null && filtros.Count > 0)
                this._filtrosComplejos.Filtros = filtros;
            else
                this._filtrosComplejos = null;

            //Selecciona la empresa segun la maestra
            DTO_aplMaestraPropiedades prop = _bc.AdministrationModel.MasterProperties[this._documentID];
            this._empresaGrupoID = _bc.GetMaestraEmpresaGrupoByDocumentID(this._documentID);

            //Inicializa el formulario
            InitializeComponent();
            this.lblTitle.Text = code;
            this._functionCount = fCount;
            this._functionData = fData;
            this._args = fArgs;
            this._colId = colId.ToString();
            this._colDesc = colDesc == "CuentaAlternaDesc" ? colDesc : "Descriptivo";
            this._sortField = colId.ToString();

            if (isHier)
                this.btnSearchHierarchy.Visible = true;

            FormProvider.LoadResources(this, this._documentID);
            _bc.Pagging_Init(this.pgGrid, this._pageSize);
            this.LoadGridData(true, false, false);

            //variables
            this._returnEdit = origin;
            //Entra al control de filtro
            this.txtCode.Focus();
        }

        /// <summary>
        /// Constructor de la amestar con todos los datos requeridos 
        /// </summary>
        /// <param name="code">Codigo del formulario</param>
        /// <param name="fCount">Nombre de la función que cuenta el numero de registros</param>
        /// <param name="fData">Nombre de la función que trae la información paginada</param>
        /// <param name="fArgs">Nombre de los argumentos</param>
        /// <param name="colId">Nombre de la columna del código</param>
        /// <param name="colDesc">Nombre de la columna de la descripción</param>
        /// <param name="isHier">Indica si es una maestra jerarquica</param>
        public ModalMaster(TextBox txtCod, string code, string fCount, string fData, Object[] fArgs, string colId, string colDesc, bool isHier, List<DTO_glConsultaFiltro> filtros = null)
        {
            //Variables
            this._documentID = Convert.ToInt32(code);
            if (filtros != null && filtros.Count > 0)
                this._filtrosComplejos.Filtros = filtros;
            else
                this._filtrosComplejos = null;

            //Selecciona la empresa segun la maestra
            DTO_aplMaestraPropiedades prop = _bc.AdministrationModel.MasterProperties[this._documentID];
            this._empresaGrupoID = _bc.GetMaestraEmpresaGrupoByDocumentID(this._documentID);

            //Inicializa el formulario
            InitializeComponent();
            this.lblTitle.Text = code;
            this._functionCount = fCount;
            this._functionData = fData;
            this._args = fArgs;
            this._colId = colId.ToString();
            this._colDesc = "Descriptivo";
            this._sortField = colId.ToString();            

            if (isHier)
                this.btnSearchHierarchy.Visible = true;

            FormProvider.LoadResources(this, this._documentID);
            _bc.Pagging_Init(this.pgGrid, this._pageSize);
            this.LoadGridData(true, false, false);
            this.txtCode.Focus();

            //variables
            this._returnCod = txtCod;

            //Entra al control de filtro
            this.txtCode.Focus();
        }

        #region Funciones privadas

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="firstPage">Si debe ir a la primera página</param>
        /// <param name="lastPage">Si debe ir a la ultima página</param>
        private void LoadGridData(bool firstTime, bool firstPage, bool lastPage)
        {
            try
            {
                this._filtrosModal = new List<DTO_glConsultaFiltro>();
                Type t = _bc.AdministrationModel.GetType();
                bool? active = true;

                //Agrega el codigo al filtro
                if (!string.IsNullOrWhiteSpace(this.txtCode.Text))
                {
                    DTO_glConsultaFiltro codFilter = new DTO_glConsultaFiltro()
                    {
                        CampoFisico = this._colId,
                        ValorFiltro = this.txtCode.Text.Trim(),
                        OperadorFiltro = OperadorFiltro.Contiene,
                        OperadorSentencia = "AND"
                    };
                    this._filtrosModal.Add(codFilter);
                }

                //Agrega la descripcion al filtro
                if (!string.IsNullOrWhiteSpace(this.txtDescription.Text))
                {
                    DTO_glConsultaFiltro descFilter = new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "Descriptivo",
                        ValorFiltro = this.txtDescription.Text.Trim(),
                        OperadorFiltro = OperadorFiltro.Contiene,
                        OperadorSentencia = "AND"
                    };
                    this._filtrosModal.Add(descFilter);
                }

                //Llama el contador de registros
                MethodInfo mCount = t.GetMethod(this._functionCount);

                //Documeto, Consulta, Filtros, Active
                long count = (long)mCount.Invoke(_bc.AdministrationModel, new Object[] { this._documentID, this._filtrosComplejos, this._filtrosModal, active });
                this.pgGrid.UpdatePageNumber(count, firstTime, firstPage, lastPage);

                //Trae los datos
                //documentID, pageSize, pageNum, consulta, List<DTO_glConsultaFiltro> FiltrosExtra, active
                //DocumentID, this.pageSize, pageNum, this.Consulta, this.FiltrosConsulta, null
                MethodInfo mData = t.GetMethod(this._functionData);
                //var data = mData.Invoke(_bc.AdministrationModel, new Object[] { this._documentID, this._pageSize, this.pgGrid.PageNumber, this._sortField, this._sortOrder, p1, p2, p3, this._filtrosComplejos });
                var data = mData.Invoke(_bc.AdministrationModel, new Object[] { this._documentID, this._pageSize, this.pgGrid.PageNumber, this._filtrosComplejos, this._filtrosModal, active });

                this._dtoList = (IEnumerable<DTO_MasterBasic>)data;
                this.grlcontrolMasterModal.DataSource = data;

                if (firstTime)
                    this.LoadGridStructure();
                this.gvMasterModal.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalMaster.cs", "MasterModal(" + this._documentID + ")-LoadGridData"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void LoadGridStructure()
        {
            try
            {
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.toolTipGrid.SetToolTip(this.grlcontrolMasterModal, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ToolTipGrid));

                #region Organiza la grilla datos

                foreach (GridColumn columna in this.gvMasterModal.Columns)
                {
                    columna.Visible = false;
                }

                GridColumn code = new GridColumn();
                code.FieldName = this.unboundPrefix + this._colId;
                code.Caption = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Code);
                code.UnboundType = UnboundColumnType.String;
                code.VisibleIndex = 0;
                code.Width = 150;
                code.OptionsColumn.AllowEdit = false;
                code.ColumnEdit = btnReturn;
                this.gvMasterModal.Columns.Add(code);

                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + this._colDesc;
                desc.Caption = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Description);
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 1;
                desc.Width = 310;
                desc.OptionsColumn.AllowEdit = false;
                this.gvMasterModal.Columns.Add(desc);
                
                this.btnReturn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalMaster.cs", "MasterModal(" + this._documentID + ")-LoadGridStructure"));
            }           
        }

        /// <summary>
        /// Retorna el valor
        /// </summary>
        /// <param name="row"></param>
        private void ReturnValueEnd(int row)
        {
            try
            {
                DTO_MasterBasic dto = (DTO_MasterBasic)this.gvMasterModal.GetRow(row);
                this._currentText = dto.ID.Value;
                //ButtonEdit origen = (ButtonEdit)sender;
                if (this._returnEdit != null)
                    this._returnEdit.Text = this._currentText;

                if (this._returnCod != null)
                    this._returnCod.Text = this._currentText;

                this.returnValue = this._currentText;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalMaster.cs", "ReturnValueEnd"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Funcion que cambia la pantalla y pone a buscar por jerarquia
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnSearchHierarchy_Click(object sender, EventArgs e)
        {
            try
            {
                string countMethod;
                string dataMethod;
                string dataRowMethod;

                DTO_glTabla table;
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(this._colId); ;

                if (this._colDesc == "CuentaAlternaDesc")
                {
                    countMethod = "coPlanCuenta_Count";
                    dataMethod = "coPlanCuenta_GetPaged";
                    dataRowMethod = "coPlanCuenta_GetCuentaAlterna";

                    string empCorp = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_EmpresaCorporativa);
                    DTO_glEmpresa emp = (DTO_glEmpresa)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, false, empCorp, true);
                    table = _bc.AdministrationModel.glTabla_GetByTablaNombre(props.NombreTabla, emp.EmpresaGrupoID_.Value);
                }
                else
                {
                    countMethod = "MasterSimple_Count";
                    dataMethod = "MasterSimple_GetPaged";
                    dataRowMethod = "MasterSimple_GetByID";

                    string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(this._documentID);
                    Tuple<int, string> tup = new Tuple<int, string>(this._documentID, empGrupo);
                    table = _bc.AdministrationModel.Tables[tup];
                }

                //Definición de la FK
                ForeignKeyFieldConfig fk = new ForeignKeyFieldConfig()
                {
                    CountMethod = countMethod,
                    DataMethod = dataMethod,
                    DataRowMethod = dataRowMethod,
                    DescField = this._colDesc, //"Descriptivo",
                    KeyField = this._colId,
                    ModalFormCode = this._documentID.ToString(),
                    TableName = props.NombreTabla
                };

                MasterHierarchyFind modal = new MasterHierarchyFind();
                List<DTO_glConsultaFiltro> filtros = this._filtrosComplejos != null ? this._filtrosComplejos.Filtros : null;
                modal.Filtros = filtros;
                modal.InitControl(table, fk);
                modal.ShowDialog();
                if (modal.DialogResult == DialogResult.OK)
                {
                    if (this._returnEdit != null)
                        this._returnEdit.Text = modal.ResultCode;
                    else
                        this._returnCod.Text = modal.ResultCode;

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalMaster.cs", "btnSearchHierarchy_Click"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.LoadGridData(false, false, false);
        }

        /// <summary>
        /// Evento que filtra la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            this.LoadGridData(true, false, false);
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
                Point pt = this.grlcontrolMasterModal.PointToClient(Control.MousePosition);

                GridHitInfo hitInfo = this.gvMasterModal.CalcHitInfo(pt);
                if (hitInfo.InColumn)
                    return;

                if (hitInfo.InRow || hitInfo.InRowCell)
                    this.ReturnValueEnd(hitInfo.RowHandle);
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Evento al presionar la tecla relacionada sobre la caja de texto de Código
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnFind_Click(sender, e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// Evento al presionar la tecla relacionada sobre la caja de texto de Descripcion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {       
            if (e.KeyCode == Keys.Enter)
                btnFind_Click(sender, e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// Evento al presionar la tecla relacionada sobre la grilla de datos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void grlcontrolMasterModal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.gvMasterModal.FocusedRowHandle >= 0)
                    {
                        int row = this.gvMasterModal.FocusedRowHandle;
                        this.ReturnValueEnd(row);
                    }
                    else
                        this.Close();
                }
                if (e.KeyCode == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Evento al presionar la tecla relacionada sobre la grilla de datos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void grlcontrolMasterModal_DoubleClick(object sender, EventArgs e)
        {
            this.btnReturn_Click(sender, e);
        }

        /// <summary>
        /// Evento al presionar Enter sobre la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el Evento</param>
        /// <param name="e">Evento</param>
        private void btnReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnReturn_Click(sender, e);
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvMasterModal_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                DTO_MasterBasic dto = this._dtoList.ElementAt(e.ListSourceRowIndex);

                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (fieldName == this._colId)
                    e.Value = dto.ID.Value;
                if (fieldName == this._colDesc)
                    e.Value = dto.Descriptivo.Value;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvMasterModal_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle>=0 && e.FocusedRowHandle<this._dtoList.Count())
                this._currentText = this._dtoList.ElementAt(e.FocusedRowHandle).ID.Value;
        }

        /// <summary>
        /// Evento de click sobre la grilla, se usa para capturar el click en las columnas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMasterModal_Click(object sender, EventArgs e)
        {
            Point pt = this.grlcontrolMasterModal.PointToClient(Control.MousePosition);

            GridHitInfo hitInfo = this.gvMasterModal.CalcHitInfo(pt);
            if (hitInfo.InColumn)
            {
                //if (hitInfo.Column.FieldName.Equals(this.sortField))
                //    return;
                //this.sortField = hitInfo.Column.FieldName;
                //this.LoadGridData(false, true, false);
                //XtraMessageBox.Show(hitInfo.Column.Caption + " clicked");
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar una tecla
        /// </summary>
        /// <param name="msg">Mensaje del evento</param>
        /// <param name="keyData">tecla presionada</param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

    }
}
