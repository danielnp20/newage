using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class PolizaPago : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public  void RefreshDataMethod()
        {
            this.CleanData();
            //this.LoadDocuments();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";
        private string areaFuncionalID;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
      
        //Listas de datos
        private List<DTO_ccPolizaEstado> polizas = new List<DTO_ccPolizaEstado>();

        //Variables Privadas
        private bool isValid = true;
        private DateTime periodo;
        #endregion

        //public PolizaPago()
        //{
        //    this.InitializeComponent();
        //}

        public PolizaPago(string mod = null)
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this.frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddDocumentCols();
                this.refreshData = new RefreshData(RefreshDataMethod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "PolizaPago"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private  void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.documentID = AppDocuments.PolizasPago;
            this.frmModule = ModulesPrefix.cf;
            this._bc.InitMasterUC(this.masterAseguradora, AppMasters.ccAseguradora, true, true, true, true);
            this.dtFechaDoc.DateTime = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this.dtFechaDoc.Properties.MinValue = new DateTime(this.dtFechaDoc.DateTime.Year, this.dtFechaDoc.DateTime.Month, 1);
            this.dtFechaCorte.DateTime = this.dtFechaDoc.DateTime;
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        private  void LoadDocuments()
        {
            try
            {
                if (this.masterAseguradora.ValidID)
                {
                    //this.currentDoc = null;
                    this.polizas = this._bc.AdministrationModel.Poliza_GetForPagos();
                    this.polizas = this.polizas.FindAll(x => x.FechaPagoSeguro.Value <= this.dtFechaCorte.DateTime && x.AseguradoraID.Value == this.masterAseguradora.Value).OrderBy(x => x.Nombre.Value).ToList();
                    this.gcDocuments.DataSource = null;
                    this.chkSeleccionar.Checked = false;
                    if (this.polizas.Count > 0)
                    {
                        this.gcDocuments.DataSource = this.polizas;
                        this.gvDocuments.BestFitColumns();
                        this.gvDocuments.MoveFirst();
                    }
                    else
                        this.gcDocuments.DataSource = null;

                    this.lblNro.Text = this.polizas.Count(x => x.PagarInd.Value.Value).ToString();
                    this.txtTotal.EditValue = this.polizas.FindAll(x => x.PagarInd.Value.Value).Sum(x => x.VlrPoliza.Value);
                }
                else
                {
                    MessageBox.Show("Debe digitar una Aseguradora");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private  void AddDocumentCols()
        {
            try
            {
                //pagar
                GridColumn pagar = new GridColumn();
                pagar.FieldName = this.unboundPrefix + "PagarInd";
                pagar.Caption = "√";
                pagar.UnboundType = UnboundColumnType.Boolean;
                pagar.VisibleIndex = 0;
                pagar.Width = 50;
                pagar.Visible = true;
                pagar.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Pagar");
                pagar.AppearanceHeader.ForeColor = Color.Lime;
                pagar.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                pagar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                pagar.AppearanceHeader.Options.UseTextOptions = true;
                pagar.AppearanceHeader.Options.UseFont = true;
                pagar.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(pagar);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 1;
                TerceroID.Width = 50;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(TerceroID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this.unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 2;
                Nombre.Width = 80;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Nombre);

                //Poliza
                GridColumn Poliza = new GridColumn();
                Poliza.FieldName = this.unboundPrefix + "Poliza";
                Poliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Poliza");
                Poliza.UnboundType = UnboundColumnType.String;
                Poliza.VisibleIndex = 3;
                Poliza.Width = 50;
                Poliza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Poliza);

                //FechaMov
                GridColumn FechaLiqSeguro = new GridColumn();
                FechaLiqSeguro.FieldName = this.unboundPrefix + "FechaLiqSeguro";
                FechaLiqSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaLiqSeguro");
                FechaLiqSeguro.UnboundType = UnboundColumnType.DateTime;
                FechaLiqSeguro.VisibleIndex = 4;
                FechaLiqSeguro.Width = 40;
                FechaLiqSeguro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(FechaLiqSeguro);

                //VlrPoliza
                GridColumn VlrPoliza = new GridColumn();
                VlrPoliza.FieldName = this.unboundPrefix + "VlrPoliza";
                VlrPoliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                VlrPoliza.UnboundType = UnboundColumnType.Decimal;
                VlrPoliza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPoliza.AppearanceCell.Options.UseTextOptions = true;
                VlrPoliza.VisibleIndex = 5;
                VlrPoliza.Width = 100;
                VlrPoliza.OptionsColumn.AllowEdit = false;
                VlrPoliza.ColumnEdit = editSpin;
                this.gvDocuments.Columns.Add(VlrPoliza);

                //TipoMovimiento
                GridColumn TipoMovimiento = new GridColumn();
                TipoMovimiento.FieldName = this.unboundPrefix + "TipoMovimiento";
                TipoMovimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoMovimiento");
                TipoMovimiento.UnboundType = UnboundColumnType.String;
                TipoMovimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                TipoMovimiento.AppearanceCell.Options.UseTextOptions = true;
                TipoMovimiento.ToolTip = "P: Pago  -  R: Revocatoria";
                TipoMovimiento.VisibleIndex = 5;
                TipoMovimiento.Width = 100;
                TipoMovimiento.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(TipoMovimiento);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        private  bool ValidateDocRow(int fila)
        {
            try
            {
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
                bool rechazado = false;
                if (this.gvDocuments.GetRowCellValue(fila, col) != null)
                    rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);

                if (rechazado)
                {
                    col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                    string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();

                    if (string.IsNullOrEmpty(desc))
                    {
                        string msg = string.Format(rsxEmpty, col.Caption);
                        this.gvDocuments.SetColumnError(col, msg);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "ValidateDocRow"));
            }

            return true;
        }

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        {
            this.polizas = new List<DTO_ccPolizaEstado>();
            this.gcDocuments.DataSource = null;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;              
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSeleccionar_CheckedChanged(object sender, EventArgs e) 
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                    this.polizas[i].PagarInd.Value = true;
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                    this.polizas[i].PagarInd.Value = false;
            }
            this.lblNro.Text = this.polizas.Count(x => x.PagarInd.Value.Value).ToString();
            this.txtTotal.EditValue = this.polizas.FindAll(x => x.PagarInd.Value.Value).Sum(x => x.VlrPoliza.Value);
            this.gcDocuments.RefreshDataSource();
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void editLink_Click(object sender, EventArgs e) { }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechaCorte_EditValueChanged(object sender, EventArgs e)
        {
            if(this.masterAseguradora.ValidID)
              this.LoadDocuments();
        }

        private void masterAseguradora_Leave(object sender, EventArgs e)
        {
            this.LoadDocuments();
        }
        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "TipoMovimiento")
                {
                    var row = (DTO_ccPolizaEstado)this.gvDocuments.GetRow(e.ListSourceRowIndex);
                    if (row.NumDocRevoca.Value.HasValue && !row.NumDocPagoRevoca.Value.HasValue)
                        e.DisplayText = "R";
                    else
                        e.DisplayText = "P";
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            this.gvDocuments.PostEditor();
            this.lblNro.Text = this.polizas.Count(x => x.PagarInd.Value.Value).ToString();
            this.txtTotal.EditValue = this.polizas.FindAll(x => x.PagarInd.Value.Value).Sum(x => x.VlrPoliza.Value);
            this.gcDocuments.RefreshDataSource();
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
        
        }

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                  this.LoadDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                this.polizas.Where(x => x.PagarInd.Value == true).ToList();
                if (this.polizas != null && this.polizas.Count > 0)
                {                   
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        private  void ApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_TxResult result = _bc.AdministrationModel.PagoPolizasCartera(this.documentID,this.dtFechaDoc.DateTime,polizas, this.masterAseguradora.Value);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                this.isValid = true;
                MessageForm frm =  new MessageForm(result);

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (this.isValid)
                    this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaPago.cs", "ApproveThread"));
                this.Invoke(this.refreshData);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion


    }
}
