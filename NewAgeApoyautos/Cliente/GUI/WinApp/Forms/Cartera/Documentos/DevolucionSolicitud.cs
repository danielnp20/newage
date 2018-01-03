using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.Globalization;
using System.Linq;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DevolucionSolicitud : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private bool isValid = true;
        private bool deleteOP = false;
        private bool enableGrid = false;

        //DTOs        
        private DTO_ccSolicitudDocu _header = new DTO_ccSolicitudDocu();
        private List<DTO_ccSolicitudDevolucionDeta> _listDevol = new List<DTO_ccSolicitudDevolucionDeta>();
        private DTO_ccSolicitudDevolucionDeta _currentRow = new DTO_ccSolicitudDevolucionDeta();
     
        //Identificador de la proxima actividad
        private string _libranzaID;
        private DateTime periodo = DateTime.Now;
        private DTO_TxResult result = new DTO_TxResult();

        //Variables Reporte
        private string _credito = string.Empty;
        private int _numDoc = 0;
        private int _numDev = 0;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DevolucionSolicitud()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DevolucionSolicitud(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);
                
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "Constructor"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this._documentID = AppDocuments.DevolucionSolicitudes;
                this._frmModule = ModulesPrefix.cc;

                //Carga la informacion de la maestras
                _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
                _bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
                _bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, true, true, true, false);
                _bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
                _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
                _bc.InitMasterUC(this.masterComercio, AppMasters.ccConcesionario, false, true, true, false);
                _bc.InitMasterUC(this.masterCooperativa, AppMasters.ccCooperativa, true, true, true, false);
                _bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
                _bc.InitMasterUC(this.masterTipoCredito, AppMasters.ccTipoCredito, true, true, true, false);
                _bc.InitMasterUC(this.masterCodeudor1, AppMasters.coTercero, false, true, true, false);
                _bc.InitMasterUC(this.masterCodeudor2, AppMasters.coTercero, false, true, true, false);

                //Deshabilita los controles
                this.masterCliente.EnableControl(false);
                this.masterAsesor.EnableControl(false);
                this.masterCiudad.EnableControl(false);
                this.masterCentroPago.EnableControl(false);
                this.masterPagaduria.EnableControl(false);
                this.masterComercio.EnableControl(false);
                this.masterCooperativa.EnableControl(false);
                this.masterLineaCredito.EnableControl(false);
                this.masterTipoCredito.EnableControl(false);
                this.masterCodeudor1.EnableControl(false);
                this.masterCodeudor2.EnableControl(false);

                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.AddGridCols();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //DevCausalID
                GridColumn DevCausalID = new GridColumn();
                DevCausalID.FieldName = this._unboundPrefix + "DevCausalID";
                DevCausalID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DevCausalID");
                DevCausalID.UnboundType = UnboundColumnType.String;
                DevCausalID.VisibleIndex = 1;
                DevCausalID.Width = 40;
                DevCausalID.Visible = true;
                DevCausalID.ColumnEdit = this.editBtnGrid;
                DevCausalID.OptionsColumn.AllowEdit = true; 
                this.gvDevoluciones.Columns.Add(DevCausalID);

                //DevCausalDesc
                GridColumn DevCausalDesc = new GridColumn();
                DevCausalDesc.FieldName = this._unboundPrefix + "DevCausalDesc";
                DevCausalDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DevCausalDesc");
                DevCausalDesc.UnboundType = UnboundColumnType.String;
                DevCausalDesc.VisibleIndex = 2;
                DevCausalDesc.Width = 150;
                DevCausalDesc.Visible = true;
                DevCausalDesc.OptionsColumn.AllowEdit = false;
                this.gvDevoluciones.Columns.Add(DevCausalDesc);

                //DevCausalGrupoID
                GridColumn DevCausalGrupoID = new GridColumn();
                DevCausalGrupoID.FieldName = this._unboundPrefix + "DevCausalGrupoID";
                DevCausalGrupoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DevCausalGrupoID");
                DevCausalGrupoID.UnboundType = UnboundColumnType.String;
                DevCausalGrupoID.VisibleIndex = 3;
                DevCausalGrupoID.Width = 20;
                DevCausalGrupoID.Visible = false;
                DevCausalGrupoID.OptionsColumn.AllowEdit = false;
                this.gvDevoluciones.Columns.Add(DevCausalGrupoID);

                //DevCausalGrupoDesc
                GridColumn DevCausalGrupoDesc = new GridColumn();
                DevCausalGrupoDesc.FieldName = this._unboundPrefix + "DevCausalGrupoDesc";
                DevCausalGrupoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DevCausalGrupoDesc");
                DevCausalGrupoDesc.UnboundType = UnboundColumnType.String;
                DevCausalGrupoDesc.VisibleIndex = 4;
                DevCausalGrupoDesc.Width = 70;
                DevCausalGrupoDesc.Visible = true;
                DevCausalGrupoDesc.OptionsColumn.AllowEdit = false;
                this.gvDevoluciones.Columns.Add(DevCausalGrupoDesc);

                //Observaciones
                GridColumn Observaciones = new GridColumn();
                Observaciones.FieldName = this._unboundPrefix + "Observaciones";
                Observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observaciones");
                Observaciones.UnboundType = UnboundColumnType.String;
                Observaciones.VisibleIndex = 5;
                Observaciones.Width = 200;
                Observaciones.Visible = true;
                Observaciones.OptionsColumn.AllowEdit = true;
                this.gvDevoluciones.Columns.Add(Observaciones);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudLibranza.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            try
            {
                DTO_ccSolicitudDevolucionDeta footerDet = new DTO_ccSolicitudDevolucionDeta();

                #region Asigna datos a la fila
                footerDet.NumeroDEV.Value = Convert.ToByte(Convert.ToByte(this.txtNumDevoluciones.Text) + 1);
                footerDet.NumeroDoc.Value = this._header.NumeroDoc.Value;
                footerDet.DevCausalID.Value =  string.Empty;                   
                footerDet.Observaciones.Value = string.Empty;                 
                this._listDevol.Add(footerDet);
                #endregion
                this.gcDevoluciones.DataSource = this._listDevol;
                this.gcDevoluciones.RefreshDataSource();
                this.gvDevoluciones.FocusedRowHandle = this.gvDevoluciones.DataRowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this._libranzaID = string.Empty;
            this.txtLibranza.Text = string.Empty;

            this.masterCliente.Value = string.Empty;
            this.masterComercio.Value = string.Empty;
            this.masterCooperativa.Value = string.Empty;
            this.masterLineaCredito.Value = string.Empty;
            this.masterTipoCredito.Value = string.Empty;
            this.masterCentroPago.Value = string.Empty;
            this.masterPagaduria.Value = string.Empty;
            this.masterAsesor.Value = string.Empty;
            this.masterCiudad.Value = string.Empty;
            this.masterCodeudor1.Value = string.Empty;
            this.masterCodeudor2.Value = string.Empty;
            this.txtPriApellido.Text = string.Empty;
            this.txtSdoApellido.Text = string.Empty;
            this.txtPriNombre.Text = string.Empty;
            this.txtSdoNombre.Text = string.Empty;
            this.txtNumDevoluciones.Text = string.Empty;
            this.comboPlazo.SelectedIndex = -1;
            this.txtValor.EditValue = 0;
            this.chkCompraCartera.Checked = false;

            //Footer        
            this.enableGrid = false;
            this._header = new DTO_ccSolicitudDocu();
            this._listDevol = new List<DTO_ccSolicitudDevolucionDeta>();
            this._currentRow = new DTO_ccSolicitudDevolucionDeta();
            this.gcDevoluciones.Enabled = true;
            this.gcDevoluciones.DataSource = null;
            this.gcDevoluciones.RefreshDataSource();
            this.gvDevoluciones.Columns[this._unboundPrefix + "Observaciones"].OptionsColumn.AllowEdit = false;
            this.gvDevoluciones.Columns[this._unboundPrefix + "DevCausalID"].OptionsColumn.AllowEdit = false;
            this.isValid = true;
            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void GetValues()
        {
            //Header
            this.masterCliente.Value = this._header.ClienteRadica.Value;
            this.txtPriApellido.Text = this._header.ApellidoPri.Value;
            this.txtSdoApellido.Text = this._header.ApellidoSdo.Value;
            this.txtPriNombre.Text = this._header.NombrePri.Value;
            this.txtSdoNombre.Text = this._header.NombreSdo.Value;
            this.txtLibranza.Text = this._header.Libranza.Value.ToString();
            this.masterAsesor.Value = this._header.AsesorID.Value;
            this.masterComercio.Value = this._header.ConcesionarioID.Value;
            this.masterCiudad.Value = this._header.Ciudad.Value;
            this.masterCentroPago.Value = this._header.CentroPagoID.Value;
            this.masterPagaduria.Value = this._header.PagaduriaID.Value;
            this.masterCooperativa.Value = this._header.CooperativaID.Value;
            this.masterLineaCredito.Value = this._header.LineaCreditoID.Value;
            this.chkCompraCartera.Checked = this._header.CompraCarteraInd.Value.Value;
            this.comboPlazo.Text = this._header.Plazo.Value.ToString();
            this.txtValor.EditValue = this._header.VlrPrestamo.Value;
            this.masterTipoCredito.Value = this._header.TipoCreditoID.Value;
            this.masterCodeudor1.Value = this._header.Codeudor1.Value;
            this.masterCodeudor2.Value = this._header.Codeudor2.Value;
            this.txtNumDevoluciones.Text = this._header.NumDevoluciones.Value.ToString();

            //Footer
            this.gcDevoluciones.DataSource = this._listDevol;
            this.gcDevoluciones.RefreshDataSource();
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
               
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (fila >= 0)
                {
                    #region DevCausalID
                    validField = this._bc.ValidGridCell(this.gvDevoluciones, this._unboundPrefix, fila, "DevCausalID", false, true, false, AppMasters.ccDevolucionCausal);
                    if (!validField)
                        validRow = false;
                    else
                    {
                        int count = this._listDevol.Count(x => x.DevCausalID.Value == this._currentRow.DevCausalID.Value);
                        if(count > 1)
                        {
                            GridColumn col = this.gvDevoluciones.Columns[this._unboundPrefix + "DevCausalID"];
                            string colVal = this.gvDevoluciones.GetRowCellValue(this.gvDevoluciones.FocusedRowHandle, col).ToString();
                            this.gvDevoluciones.SetColumnError(col, string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_AlreadyExistInGrid),col.Caption));
                            validRow = false;
                        }
                    }
                    #endregion

                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "ValidateRow"));
            }
            return validRow;
        }

        #endregion Funciones Privadas

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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemPrint.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {                  
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Approve) ||
                        SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSearch.Visible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que permite crear, habilitar o deshabilitar las propiedades del documento con base a la Libranza  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._libranzaID != this.txtLibranza.Text.Trim())
                {
                    string tmp = this.txtLibranza.Text;
                    this._libranzaID = this.txtLibranza.Text.Trim();
                    int libranzaTemp = Convert.ToInt32(this._libranzaID);
                    List<DTO_ccSolicitudDevolucion> devoluciones = new List<DTO_ccSolicitudDevolucion>();
                    this._header = _bc.AdministrationModel.DevolucionSolicitud_GetByLibranza(libranzaTemp, ref this._listDevol);
                    this._listDevol = this._listDevol.FindAll(x => x.NumeroDEV.Value == this._header.NumDevoluciones.Value);
                    if (this._header != null)
                    {
                        this.gcDevoluciones.Enabled = true;
                        this.gvDevoluciones.Columns[this._unboundPrefix + "Observaciones"].OptionsColumn.AllowEdit = true;
                        this.gvDevoluciones.Columns[this._unboundPrefix + "DevCausalID"].OptionsColumn.AllowEdit = true;
                        foreach (var dev in this._listDevol)
                            dev.NumeroDEV.Value = Convert.ToByte((this._header.NumDevoluciones.Value) + 1);

                        this.GetValues();
                        this.enableGrid = true;
                        FormProvider.Master.itemPrint.Enabled = true;

                        if (this._header.Estado.Value.Value != (int)EstadoDocControl.SinAprobar && this._header.Estado.Value.Value != (int)EstadoDocControl.ParaAprobacion)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidSolicitudEstado));

                            this.enableGrid = false;
                            this.gvDevoluciones.Columns[this._unboundPrefix + "Observaciones"].OptionsColumn.AllowEdit = false;
                            this.gvDevoluciones.Columns[this._unboundPrefix + "DevCausalID"].OptionsColumn.AllowEdit = false;
                            FormProvider.Master.itemSave.Enabled = false;
                        }
                        else if (this._header.DevueltaInd.Value.HasValue && this._header.DevueltaInd.Value.Value)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidSolicitudDevuelta));

                            this.enableGrid = false;
                            this.gvDevoluciones.Columns[this._unboundPrefix + "Observaciones"].OptionsColumn.AllowEdit = false;
                            this.gvDevoluciones.Columns[this._unboundPrefix + "DevCausalID"].OptionsColumn.AllowEdit = false;
                            FormProvider.Master.itemSave.Enabled = false;
                        }
                    }
                    else
                    {                        
                        string msg = this.lblLibranza.Text + " " + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField);
                        MessageBox.Show(msg);

                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                        FormProvider.Master.itemSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que revisa que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDevol_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (!this.enableGrid)
                {
                    e.Handled = true;
                    return;
                }

                #region Agregar

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvDevoluciones.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid && this.gvDevoluciones.DataRowCount == 0)
                            this.AddNewRow();
                        else
                        {
                            if (this.gvDevoluciones.DataRowCount >= 0)
                            {
                                bool isV = this.ValidateRow(this.gvDevoluciones.DataRowCount == 0? -1 : this.gvDevoluciones.FocusedRowHandle);
                                if (isV)
                                   this.AddNewRow(); 
                            }
                        }
                    }
                }
                #endregion

                #region Eliminar
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                    e.Handled = true;
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvDevoluciones.FocusedRowHandle;

                        if (this._listDevol.Count > 0)
                        {
                            DTO_ccSolicitudDevolucionDeta row = (DTO_ccSolicitudDevolucionDeta)this.gvDevoluciones.GetRow(rowHandle);
                            this._listDevol.RemoveAll(x => x.DevCausalID.Value == row.DevCausalID.Value);
                            //Si borra el primer registro
                            if (rowHandle >= 0)
                                this.gvDevoluciones.FocusedRowHandle = 0;
                            this.isValid = true;
                        }
                        this.gvDevoluciones.RefreshData();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDevol_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                GridColumn col = this.gvDevoluciones.Columns[this._unboundPrefix + fieldName];

                if (fieldName == "DevCausalID")
                {
                    DTO_ccDevolucionCausal dev = (DTO_ccDevolucionCausal)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccDevolucionCausal, false, e.Value.ToString(), true);
                    if(dev != null)
                    {
                        this._currentRow.DevCausalDesc.Value = dev.Descriptivo.Value;
                        this._currentRow.DevCausalGrupoID.Value = dev.DevCausalGrupoID.Value;
                        this._currentRow.DevCausalGrupoDesc.Value = dev.DevCausalGrupoDesc.Value;
                        this.gcDevoluciones.RefreshDataSource();
                    }                       
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDevol_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.deleteOP && this.gvDevoluciones.DataRowCount > 0)
                    this.ValidateRow(e.RowHandle);
                if (!this.isValid)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDevol_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                    e.Value = 0;
            }
            if (e.IsSetData)
            {

                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
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
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDevoluciones_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (!this.deleteOP  && e.FocusedRowHandle >= 0)
                   this._currentRow = (DTO_ccSolicitudDevolucionDeta)this.gvDevoluciones.GetRow(e.FocusedRowHandle);
        
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "gvDocument_FocusedRowChanged"));
            }
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
                string colName = this.gvDevoluciones.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDevoluciones.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                if (this._header != null)
                {
                    //Valida que existan Libranzas
                    if (this.gvDevoluciones.DataRowCount == 0)
                    {
                        this.gcDevoluciones.Focus();
                        return;
                    }                
                    if(!this.ValidateRow(this.gvDevoluciones.FocusedRowHandle))
                    {
                        this.gcDevoluciones.Focus();
                        return;
                    }

                    DTO_ccSolicitudDevolucion devolucion = new DTO_ccSolicitudDevolucion();
                    devolucion.NumeroDoc.Value = this._header.NumeroDoc.Value;
                    devolucion.NumeroDEV.Value = Convert.ToByte(Convert.ToByte(this.txtNumDevoluciones.Text) + 1);
                    devolucion.FechaDEV.Value = DateTime.Now;
                    devolucion.seUsuarioID.Value = _bc.AdministrationModel.User.ReplicaID.Value;
                    devolucion.Detalle = this._listDevol;
                    result = _bc.AdministrationModel.DevolucionSolicitud_Add(this._documentID, devolucion);
                    if (result.Result == ResultValue.OK)
                    {
                        string msg =string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada),this.txtLibranza.Text);
                        MessageBox.Show(string.Format(msg, result.ExtraField));
                        this._numDev = Convert.ToByte(Convert.ToByte(this.txtNumDevoluciones.Text)+1);
                        this._credito = this.txtLibranza.Text;
                        this.CleanData();
                        this.txtLibranza.Focus();
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();
                    }
                }
                #region Pregunta si desea abrir los reportes

                bool deseaImp = false;
                if (this._header != null || result.Result == ResultValue.OK)
                {
                    string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                    var resultA = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultA == DialogResult.Yes)
                        deseaImp = true;
                }

                #endregion
                #region Genera e imprime los reportes                
                    if (deseaImp)
                    {
                        string reportName = this._bc.AdministrationModel.Report_cc_DevolucionSolicitud(this._credito, this._numDoc, this._numDev);
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);
                    }
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para impresion del Documento
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                this._numDev = Convert.ToByte(Convert.ToByte(this.txtNumDevoluciones.Text));
                this._credito = this._libranzaID;
                string reportName = this._bc.AdministrationModel.Report_cc_DevolucionSolicitud(this._credito, this._numDoc, this._numDev);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }   
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DevolucionSolicitud.cs", "TBPrint"));
            }
        }

        #endregion Eventos Barra Herramientas
            
    }
}