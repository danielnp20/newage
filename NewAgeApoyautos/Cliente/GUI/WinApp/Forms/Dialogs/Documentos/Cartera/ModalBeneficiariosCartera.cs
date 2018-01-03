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
    public partial class ModalBeneficiariosCartera : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int documentID = AppForms.ModalBeneficiariosCartera;
        private string unboundPrefix = "Unbound_";
        private DTO_ccSolicitudDetallePago pagoDetalle = new DTO_ccSolicitudDetallePago();
        private List<DTO_ccSolicitudDetallePago> detallePagos = new List<DTO_ccSolicitudDetallePago>();

        private bool isValid;
        private bool firstTime = true;
        private bool deleteOp;
        private bool isModalFormOpened;
        //private int vlrGiroModal;
        private int vlrGiro;
        private int vlrGiroToValidate;
        private int vlrTerceroTemp;
        private string msgError;
        #endregion

        /// <summary>
        /// Constructor de la grilla de plan pagos 
        /// </summary>
        /// <param name="saldosCartera">DTO que tiene el plan de pagos y los componentes</param>
        public ModalBeneficiariosCartera(List<DTO_ccSolicitudDetallePago> detallePagos, decimal valorGiro)
        {
            try
            {
                //Inicializa el formulario
                InitializeComponent();
                this.AddBeneficiariosCols();

                FormProvider.LoadResources(this, this.documentID);
                this.msgError = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_VlrGiroTerceroInvalid);
                this.vlrGiro = Convert.ToInt32(valorGiro);
                this.LoadGridPlanPagos(detallePagos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "ModalBeneficiariosCartera"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Agrega las Columna a la grilla de plan pagos
        /// </summary>
        private void AddBeneficiariosCols()
        {
            try
            {
                //Tercero ID
                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 0;
                terceroID.Width = 100;
                terceroID.Visible = true;
                terceroID.OptionsColumn.AllowEdit = true;
                terceroID.ColumnEdit = this.editBtnGrid;
                this.gvBeneficiario.Columns.Add(terceroID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 2;
                nombre.Width = 180;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvBeneficiario.Columns.Add(nombre);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Integer;
                valor.VisibleIndex = 3;
                valor.Width = 200;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = true;
                valor.ColumnEdit = this.editSpin;
                this.gvBeneficiario.Columns.Add(valor);

                this.gvBeneficiario.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "AddBeneficiariosCols"));
            }
        }

        /// <summary>
        /// Carga los datos del plan de pagos
        /// </summary>
        private void LoadGridPlanPagos(List<DTO_ccSolicitudDetallePago> pagos)
        {
            try
            {

                this.detallePagos = pagos;
                this.gcBeneficiario.DataSource = this.detallePagos;
                this.gvBeneficiario.PostEditor();
                this.vlrGiroToValidate = this.vlrGiro;
                if (this.detallePagos.Count > 0)
                {
                    decimal vlrTerceros = 0;
                    int i = 0;
                    int vlrGiroTercero = 0;
                    foreach (DTO_ccSolicitudDetallePago detaPago in this.detallePagos)
                    {
                        DTO_coTercero coTercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, detaPago.TerceroID.Value, false);
                        detaPago.Nombre.Value = coTercero.Descriptivo.Value;
                        vlrTerceros += detaPago.Valor.Value.Value;
                        vlrGiroTercero = Convert.ToInt32(detaPago.Valor.Value.Value);
                        this.gvBeneficiario.FocusedRowHandle = i;
                        this.ValidateRow_CompraCartera(i, this.vlrGiroToValidate, vlrGiroTercero);
                        this.vlrGiroToValidate = this.vlrGiro - vlrGiroTercero;
                        i++;
                    }
                    this.txtVlrBenefi.EditValue = vlrTerceros;
                    this.txtVlrGiro.EditValue = this.vlrGiro - vlrTerceros;
                }
                else
                {
                    this.txtVlrGiro.EditValue = this.vlrGiro;
                    this.txtVlrBenefi.EditValue = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "LoadGridPlanPagos"));
            }
        }

        /// <summary>
        /// Metodo que permite crea una nueva fila en una grilla
        /// </summary>
        private void AddNewRow()
        {
            DTO_ccSolicitudDetallePago detaPagos = new DTO_ccSolicitudDetallePago();
            try
            {
                isValid = false;
                #region Asigna datos a la fila
                detaPagos.TerceroID.Value = string.Empty;
                detaPagos.Nombre.Value = string.Empty;
                detaPagos.Valor.Value = 0;
                #endregion
                this.detallePagos.Add(detaPagos);
                this.gcBeneficiario.DataSource = this.detallePagos;
                this.gcBeneficiario.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "AddNewRow"));
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
            this.isModalFormOpened = true;
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
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.isModalFormOpened = false;
            }
        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_CompraCartera(int fila, int vlrGiro, int vlrDigitado)
        {
            try
            {
                this.gvBeneficiario.PostEditor();
                this.isValid = true;
                if (fila >= 0)
                {
                    bool rowValid = true;
                    string fieldName;
                    #region Tercero ID
                    rowValid = true;
                    fieldName = "TerceroID";
                    GridColumn colFinanciera = this.gvBeneficiario.Columns[this.unboundPrefix + fieldName];
                    //Valida la financiera
                    rowValid = this._bc.ValidGridCell(this.gvBeneficiario, this.unboundPrefix, fila, fieldName, false, true, false, AppMasters.coTercero);
                    string tercero = this.detallePagos[fila].TerceroID.Value;
                    List<DTO_ccSolicitudDetallePago> dtPagosTemp = (from c in this.detallePagos where c.TerceroID.Value == tercero select c).ToList();
                    if (rowValid || dtPagosTemp.Count > 1)
                        this.gvBeneficiario.SetColumnError(colFinanciera, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                    #region Valor
                    rowValid = true;
                    fieldName = "Valor";
                    GridColumn colValor = this.gvBeneficiario.Columns[this.unboundPrefix + fieldName];
                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvBeneficiario, this.unboundPrefix, fila, fieldName, false, false, true, false);
                    if (rowValid)
                        this.gvBeneficiario.SetColumnError(colValor, string.Empty);
                    else
                        this.isValid = false;

                    if (vlrGiro < vlrDigitado)
                    {
                        this.gvBeneficiario.SetColumnError(colValor, this.msgError);
                        this.isValid = false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "ValidateRow_CompraCartera"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBeneficiario_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
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
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
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
        /// Evento que carga los componentes segun la cuota del plan de pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBeneficiario_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.pagoDetalle = (DTO_ccSolicitudDetallePago)this.gvBeneficiario.GetRow(e.FocusedRowHandle);
            if (this.pagoDetalle != null)
                this.vlrTerceroTemp = Convert.ToInt32(this.pagoDetalle.Valor.Value.Value);
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcBeneficiario_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvBeneficiario.FocusedRowHandle;
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    this.ValidateRow_CompraCartera(fila, this.vlrGiro, this.vlrGiroToValidate);
                    if (this.isValid)
                    {
                        this.AddNewRow();
                        this.gvBeneficiario.FocusedRowHandle = this.detallePagos.Count - 1;
                    }
                }

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    if (fila >= 0)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.isValid = true;
                            this.deleteOp = true;
                            this.detallePagos.RemoveAt(fila);
                            if (this.detallePagos.Count > 0)
                            {
                                decimal vlrBenifi = (from c in this.detallePagos select c.Valor.Value.Value).Sum();
                                this.txtVlrBenefi.EditValue = vlrBenifi;
                                this.txtVlrGiro.EditValue = this.vlrGiro - vlrBenifi;
                                this.gvBeneficiario.FocusedRowHandle = fila - 1;
                            }
                            else
                            {
                                this.txtVlrBenefi.EditValue = 0;
                                this.txtVlrGiro.EditValue = this.vlrGiro;
                            }
                            this.gcBeneficiario.RefreshDataSource();
                            this.deleteOp = false;
                        }

                        e.Handled = true;
                    }
                    else
                        e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "gcBeneficiario_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que valida las columna de la grilla antes de salir de esta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBeneficiario_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            int fila = e.RowHandle;
            if (!this.deleteOp)
            {
                this.ValidateRow_CompraCartera(fila, this.vlrGiro, this.vlrGiroToValidate);
                if (!this.isValid)
                    e.Allow = false;
            }
        }

        /// <summary>
        /// Evento que valida las columna de la grilla despues de editarlas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBeneficiario_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                int fila = e.RowHandle;

                if (fieldName == "Valor")
                {
                    int vlrGiroModal = Convert.ToInt32(e.Value);
                    this.vlrGiroToValidate = Convert.ToInt32((from c in this.detallePagos select c.Valor.Value.Value).Sum());
                    int vlrBenefiTotalTemp = Convert.ToInt32(this.txtVlrBenefi.EditValue);
                    int vlrBenefiTotal = Convert.ToInt32(this.txtVlrBenefi.EditValue);
                    this.ValidateRow_CompraCartera(fila, this.vlrGiro, this.vlrGiroToValidate);
                    if (this.isValid)
                    {
                        if (vlrGiroModal > this.vlrTerceroTemp)
                        {
                            vlrBenefiTotalTemp += vlrGiroModal;
                            vlrBenefiTotal = vlrBenefiTotalTemp - this.vlrTerceroTemp;
                            this.vlrGiroToValidate = this.vlrGiro - vlrBenefiTotal;
                            this.txtVlrGiro.EditValue = this.vlrGiroToValidate;
                            this.txtVlrBenefi.EditValue = vlrBenefiTotal;
                            this.vlrTerceroTemp = vlrGiroModal;
                        }
                        else
                        {
                            vlrBenefiTotalTemp = this.vlrTerceroTemp - vlrGiroModal;
                            vlrBenefiTotal = vlrBenefiTotal - vlrBenefiTotalTemp;
                            this.vlrGiroToValidate = this.vlrGiro - vlrBenefiTotal;
                            this.txtVlrGiro.EditValue = this.vlrGiroToValidate;
                            this.txtVlrBenefi.EditValue = vlrBenefiTotal;
                            this.vlrTerceroTemp = vlrGiroModal;
                        }
                    }
                }
                this.gcBeneficiario.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "gvBeneficiario_CellValueChanged"));
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
                string colName = this.gvBeneficiario.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                int fila = this.gvBeneficiario.FocusedRowHandle;
                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(fila, colName, origin);
                if (!String.IsNullOrWhiteSpace(origin.Text))
                {
                    DTO_coTercero coTercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, origin.Text, false);
                    this.detallePagos[fila].TerceroID.Value = coTercero.ID.Value;
                    this.detallePagos[fila].Nombre.Value = coTercero.Descriptivo.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalBeneficiariosCartera.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

    }
}
