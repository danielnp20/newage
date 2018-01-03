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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalMasterHierarchy : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = 0;
        private string _colId = string.Empty;
        private string _colDesc = string.Empty;
        private TextBox _txtcodigo;
        private UDT_BasicID _padre;
        private string _empresaGrupoID;
        private IEnumerable<DTO_MasterBasic> _dtoList;
        private string _currentText;
        private List<DTO_glConsultaFiltro> _filtros;

        //Propiedades de la grilla
        private int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["Pagging.Master.PageSize.Modal"]);
        private string _sortField = string.Empty;
        private OrderDirection _sortOrder = OrderDirection.ASC;

        #endregion

        /// <summary>
        /// Constructor de la amestar con todos los datos requeridos 
        /// </summary>
        /// <param name="txtcodigo">Textbox que debe recibir el codigo seleccionado</param>
        /// <param name="padre">Padre del codigo actual</param>
        /// <param name="code">Codigo del formulario</param>
        /// <param name="fCount">Nombre de la función que cuenta el numero de registros</param>
        /// <param name="fData">Nombre de la función que trae la información paginada</param>
        /// <param name="fID">Nombre de la función para el get by ID</param>
        /// <param name="fArgs">Nombre de los argumentos</param>
        /// <param name="colId">Nombre de la columna del código</param>
        /// <param name="colDesc">Nombre de la columna de la descripción</param>
        public ModalMasterHierarchy(TextBox txtcodigo, UDT_BasicID padre, int documentId, string code, string colId, string colDesc, List<DTO_glConsultaFiltro> filtros = null)
        {
            this._filtros = filtros;
            //variables
            this._txtcodigo = txtcodigo;
            this._padre = padre;
            this._documentID = Convert.ToInt32(code);

            //Selecciona la empresa segun la maestra
            DTO_aplMaestraPropiedades prop = _bc.AdministrationModel.MasterProperties[this._documentID];
            this._empresaGrupoID = _bc.GetMaestraEmpresaGrupoByDocumentID(this._documentID);

            //Inicializa el formulario
            InitializeComponent();
            this.lblTitle.Text = code;
            this._colId = colId;
            this._colDesc = colDesc == "CuentaAlternaDesc" ? colDesc : "Descriptivo";
            this._sortField = colId.ToString();

            FormProvider.LoadResources(this, this._documentID);
            _bc.Pagging_Init(this.pgGrid, this._pageSize);
            this.LoadGridData(true, false, false);
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
                Type t = _bc.AdministrationModel.GetType();
                string p1 = this.txtCode.Text;
                string p2 = this.txtDescription.Text;
                bool? p3 = true;

                long count;
                if (this._colDesc == "CuentaAlternaDesc")
                    count = this._bc.AdministrationModel.coPlanCuenta_CountChildren(this._documentID, this._padre, p1, p2, p3, this._filtros);
                else
                    count = this._bc.AdministrationModel.MasterHierarchy_CountChildren(this._documentID, this._padre, p1, p2, p3, this._filtros);

                this.pgGrid.UpdatePageNumber(count, firstTime, firstPage, lastPage);

                //Trae los datos
                IEnumerable<DTO_MasterBasic> data;
                if (this._colDesc == "CuentaAlternaDesc")
                    data = this._bc.AdministrationModel.coPlanCuenta_GetPagedChildren(this._documentID, this._pageSize, this.pgGrid.PageNumber, this._sortOrder, this._padre, p1, p2, p3, _filtros);
                else
                    data = this._bc.AdministrationModel.MasterHierarchy_GetChindrenPaged(this._documentID, this._pageSize, this.pgGrid.PageNumber, this._sortOrder, this._padre, p1, p2, p3, _filtros);

                this._dtoList = (IEnumerable<DTO_MasterHierarchyBasic>)data;
                this.grlcontrolHierarachy.DataSource = data;
                if (data != null && data.Count() > 0)
                {
                    setCurrentText(0);
                }
                if (firstTime)
                    this.LoadGridStructure();
                this.gvHierarachy.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalMasterHierarchy.cs", "MasterHierarchyModal(" + this._documentID + ")-LoadGridData"));
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
                this.toolTipGrid.SetToolTip(this.grlcontrolHierarachy, _bc.GetResource(LanguageTypes.Messages, "msg_toolTipGrid"));

                #region Organiza la grilla datos

                foreach (GridColumn columna in this.gvHierarachy.Columns)
                {
                    columna.Visible = false;
                }

                GridColumn code = new GridColumn();
                code.FieldName = this._colId;
                code.Caption = _bc.GetResource(LanguageTypes.Forms, "1002_lblCode");
                code.UnboundType = UnboundColumnType.String;
                code.VisibleIndex = 0;
                code.Width = 150;
                code.OptionsColumn.AllowEdit = false;
                code.ColumnEdit = btnReturn;
                this.gvHierarachy.Columns.Add(code);

                GridColumn desc = new GridColumn();
                desc.FieldName = this._colDesc;
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, "1002_lblDescription");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 1;
                desc.Width = 310;
                desc.OptionsColumn.AllowEdit = false;
                this.gvHierarachy.Columns.Add(desc);

                this.btnReturn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalMasterHierarchy.cs", "MasterHierarchyModal(" + this._documentID + ")-LoadGridStructure"));
            }           
        }

        /// <summary>
        /// Retorna el valor
        /// </summary>
        /// <param name="row"></param>
        private void ReturnValueEnd()
        {

            //ButtonEdit origen = (ButtonEdit)sender;
            Type t = _bc.AdministrationModel.GetType();
            UDT_BasicID id = new UDT_BasicID() { Value = this._currentText };

            DTO_MasterHierarchyBasic result = this._bc.AdministrationModel.MasterHierarchy_GetByID(this._documentID, id, true);

            //Llama la funcion de get by ID
            //MethodInfo mID = t.GetMethod(this._functionByID);
            //var result = mID.Invoke(_bc.AdministrationModel, new Object[] {this._documentID, id, this._empresaGrupoID });

            //Obtiene la propiedad de jerarquia para el objeto devuelto
            //PropertyInfo dtoHierarchy =  result.GetType().GetProperty("Jerarquia");

            try
            {
                DTO_hierarchy h = result.Jerarquia;
                this._txtcodigo.Text = h.Codigos[h.NivelInstancia - 1];
                //switch (h.NivelInstancia)
                //{
                //    case 1:
                //        this._txtcodigo.Text = h.CodigoNivel1;
                //        break;
                //    case 2:
                //        this._txtcodigo.Text = h.CodigoNivel2;
                //        break;
                //    case 3:
                //        this._txtcodigo.Text = h.CodigoNivel3;
                //        break;
                //    case 4:
                //        this._txtcodigo.Text = h.CodigoNivel4;
                //        break;
                //    case 5:
                //        this._txtcodigo.Text = h.CodigoNivel5;
                //        break;
                //}

            }
            catch (Exception ex)
            {
                throw;
            }

            this.Close();
        }

        #endregion

        #region Eventos
       
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
            Point pt = this.grlcontrolHierarachy.PointToClient(Control.MousePosition);

            GridHitInfo hitInfo = this.gvHierarachy.CalcHitInfo(pt);
            if (hitInfo.InColumn || (!hitInfo.InRow && !hitInfo.InRowCell))
            {
                //Doble click en el encabezado
                return;
            }
            if (hitInfo.InRow || hitInfo.InRowCell)
            {
                DTO_MasterBasic dto = (DTO_MasterBasic)this.gvHierarachy.GetRow(hitInfo.RowHandle);
                this._currentText = dto.ID.Value;
                this.ReturnValueEnd();
            }
            return;
        }

        /// <summary>
        /// Evento al presionar enter sobre la caja de texto de Código
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
        /// Evento al presionar enter sobre la caja de texto de Descripcion
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
        private void grlcontrolHierarachy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ReturnValueEnd();
            }
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// Evento al presionar la tecla relacionada sobre la grilla de datos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void grlcontrolHierarachy_DoubleClick(object sender, EventArgs e)
        {
            btnReturn_Click(sender, e);
        }

        /// <summary>
        /// Evento al presionar Enter sobre la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el Evento</param>
        /// <param name="e">Evento</param>
        private void btnReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnReturn_Click(sender, e);
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvHierarachy_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e) 
        {
            if (e.IsGetData)
            {
                DTO_MasterBasic dto = this._dtoList.ElementAt(e.ListSourceRowIndex);

                if (e.Column.FieldName == this._colId)
                    e.Value = dto.ID.Value;
                if (e.Column.FieldName == this._colDesc)
                    e.Value = dto.Descriptivo.Value;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvHierarachy_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle < this._dtoList.Count())
                this.setCurrentText(e.FocusedRowHandle);
        }

        /// <summary>
        /// Llena la variable currenttext con el valor de una fila dada
        /// </summary>
        /// <param name="rowhandle">Fila de la grilla</param>
        protected void setCurrentText(int rowhandle)
        {
            this._currentText = this._dtoList.ElementAt(rowhandle).ID.Value;
        }

        /// <summary>
        /// Evento de click sobre la grilla, se usa para capturar el click en las columnas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMasterModal_Click(object sender, EventArgs e)
        {
            Point pt = this.grlcontrolHierarachy.PointToClient(Control.MousePosition);

            GridHitInfo hitInfo = this.gvHierarachy.CalcHitInfo(pt);
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
