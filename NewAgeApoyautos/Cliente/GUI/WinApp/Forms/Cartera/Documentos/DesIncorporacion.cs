using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DesIncorporacion : SolicitudCreditoChequeo
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.LoadDocuments();
        }

        public delegate void RefreshGrid();
        public RefreshGrid refreshGridDelegate;
        public virtual void RefreshGridMethod()
        {
            this.libranzaID = string.Empty;
            this.txtLibranza.Text = string.Empty;
            this.creditosToSave = this.creditos;
            this.gcDocuments.DataSource = this.creditosToSave;
            this.gvDocuments.RefreshData();
        }

        #endregion

        public DesIncorporacion()
            : base()
        {
            //InitializeComponent();
        }

        public DesIncorporacion(string mod)
            : base(mod)
        {
        }

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> creditosToSave = new List<DTO_ccCreditoDocu>();
        
        //Variables Privadas
        private string libranzaID = string.Empty;
        private bool isValid;
        private bool validateData;
        private bool deleteOp;
        private DateTime periodo;

        //Variable mensajes de error
        private string msgCredExiste;
        private string msgEmptyField;
        private string msgPositive;
        private string msgLibranzaInvalida;
        private string msgLibranzaCancelada;
        private string msgLibranzaRepetida;
        private string msgFkNotFound;
        private string msgInvalidCP;

        //Rsx
        private string libranzaRsx;
        private string centroPagoRsx;
        private string vlrCuotaRsx;

        private string format;
        private string formatSeparator = "\t";
        private PasteOpDTO pasteRet;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        ///  Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.DesIncorporacion;
            this.frmModule = ModulesPrefix.cc;

            //Cargar los Controles de Mestras
            this._bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, false, false);

            //Permite modificar los paneles
            this.tableLayoutPanel1.RowStyles[0].Height = 40;
            this.tableLayoutPanel1.RowStyles[1].Height = 250;
            this.tableLayoutPanel1.RowStyles[2].Height = 0;
            this.tableLayoutPanel1.RowStyles[3].Height = 0;

            //Carga la info de los mensajes
            this.msgCredExiste = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaExiste);
            this.msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
            this.msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
            this.msgLibranzaInvalida = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_InvalidLibranza);
            this.msgLibranzaCancelada = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_CreditoCancelado);
            this.msgLibranzaRepetida = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_CreditoAgregado);
            this.msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            this.msgInvalidCP = _bc.GetResourceError(DictionaryMessages.Err_Cc_InvalidCentroPagoLibranza);

            //Carga la informacion de la fecha
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            this.dtFecha.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            //Carga recursos
            FormProvider.LoadResources(this, AppDocuments.Incorporacion);
            base.SetInitParameters();

            this.refreshGridDelegate = new RefreshGrid(RefreshGridMethod);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "Rechazado";
                noAprob.Caption = "X";
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 15;
                noAprob.Visible = false;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Campo de Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.Integer;
                libranza.VisibleIndex = 2;
                libranza.Width = 65;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);
                this.libranzaRsx = libranza.Caption;

                //Cliente Id
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 3;
                clienteID.Width = 65;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 4;
                nombCliente.Width = 110;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombCliente);

                //Centro de pago
                GridColumn cp = new GridColumn();
                cp.FieldName = this.unboundPrefix + "Otro";
                cp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroPago");
                cp.UnboundType = UnboundColumnType.String;
                cp.VisibleIndex = 5;
                cp.Width = 110;
                cp.Visible = true;
                cp.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(cp);
                this.centroPagoRsx = cp.Caption;

                //VlrCuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaPendiente");
                vlrCuota.UnboundType = UnboundColumnType.Decimal;
                vlrCuota.VisibleIndex = 6;
                vlrCuota.Width = 120;
                vlrCuota.Visible = true;
                vlrCuota.OptionsColumn.AllowEdit = false;
                vlrCuota.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrCuota);
                this.vlrCuotaRsx = vlrCuota.Caption;

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 7;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);

                this.format = this.libranzaRsx + this.formatSeparator + this.centroPagoRsx + this.formatSeparator + this.vlrCuotaRsx;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desincoroparcion.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Carga el cabezote con los documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.isValid = false;
                this.currentDoc = null;
                this.creditos = this._bc.AdministrationModel.DesIncorporacionCredito_Get();
                this.creditosToSave = this.creditos;

                this.currentRow = -1;
                if (this.creditos.Count > 0)
                {
                    this.detailsLoaded = false;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.isValid = true;
                    this.gcDocuments.DataSource = this.creditosToSave;

                    this.allowValidate = true;

                    if (!detailsLoaded)
                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);

                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this.creditosToSave = new List<DTO_ccCreditoDocu>();
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desincoroparcion.cs", "LoadDocuments"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que valida las condiciones para que el documento se pueda guardar
        /// </summary>
        private bool ValidateDoc()
        {
            List<DTO_ccCreditoDocu> credValidacion = this.creditos.Where(x => x.Aprobado.Value == true).ToList();
            if (credValidacion.Count == 0)
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                MessageBox.Show(msg);
                return false;
            }
          
            return true;
        }

        /// <summary>
        /// Funcion que limpia los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            this.validateData = false;

            //Header
            this.masterCentroPago.Value = String.Empty;
            this.libranzaID = String.Empty;

            //Footer           
            this.creditosToSave = new List<DTO_ccCreditoDocu>();
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.gcDocuments.DataSource = this.creditos;

            this.validateData = true;
        }

        /// <summary>
        /// Metodo que permite crea una nueva fila en una grilla
        /// </summary>
        private void AddNewRow()
        {
            DTO_ccCreditoDocu creditoNew = new DTO_ccCreditoDocu();
            try
            {
                this.isValid = false;

                #region Asigna datos a la fila

                creditoNew.Aprobado.Value = false;
                creditoNew.Rechazado.Value = false;
                creditoNew.Libranza.Value = 0;
                creditoNew.ClienteID.Value = string.Empty;
                creditoNew.Nombre.Value = string.Empty;
                creditoNew.Observacion.Value = string.Empty;
                creditoNew.Editable.Value = true;
                creditoNew.Otro1.Value = ((byte)TipoNovedad.Desincorpora).ToString();
                creditoNew.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();

                #endregion

                this.creditos.Add(creditoNew);
                this.gcDocuments.DataSource = this.creditosToSave;
                this.gcDocuments.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesIncorporacion.cs", "AddNewRow"));
            }
        }

        /// <summary>
        /// Filtra resultados
        /// </summary>
        private void FilterData()
        {
            try
            {
                this.creditosToSave = this.creditos;

                //Centro de pago
                if(!string.IsNullOrWhiteSpace(this.masterCentroPago.Value))
                {
                    this.creditosToSave = this.creditosToSave.Where(x => x.CentroPagoID.Value == this.masterCentroPago.Value).ToList();
                }

                //Libranza
                if (!string.IsNullOrWhiteSpace(this.txtLibranza.Text))
                {
                    this.creditosToSave = this.creditosToSave.Where(x => x.Libranza.Value.ToString() == this.txtLibranza.Text).ToList();
                }

                this.validateData = false;
                this.gcDocuments.DataSource = creditosToSave;
                this.gvDocuments.RefreshData();
                this.validateData = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesIncorporacion.cs", "FilterData"));
            }
        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_DesIncorporacion(int fila)
        {
            try
            {
                this.gvDocuments.PostEditor();
                this.isValid = true;

                if (fila >= 0)
                {
                    bool rowValid = true;
                    string fieldName;
                    #region Crédito

                    rowValid = true;
                    fieldName = "Libranza";
                    GridColumn colLibranza = this.gvDocuments.Columns[this.unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvDocuments, this.unboundPrefix, fila, fieldName, false, false, true, false);
                    if (rowValid)
                        this.gvDocuments.SetColumnError(colLibranza, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesIncorporacion.cs", "ValidateRow_DesIncorporacion"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que filtra los documentos de acuerdo a la pagaduria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCentroPago_Leave(object sender, EventArgs e)
        {
            if (this.isValid)
                this.FilterData();
        }

        /// <summary>
        /// Evento que que filtra la lista con base a la libranza
        /// </summary>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            if (this.isValid)
                this.FilterData();
        }

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < gvDocuments.DataRowCount; i++)
                {
                    this.creditos[i].Aprobado.Value = true;
                    this.creditos[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < gvDocuments.DataRowCount; i++)
                {
                    this.creditos[i].Aprobado.Value = false;
                    this.creditos[i].Rechazado.Value = false;
                }
            }
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Carga el Boton de Exportar Archivo
        /// </summary>
        /// <param name="sender">Evento que se ejecuta cuando sale del control</param>
        /// <param name="e"></param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.itemGenerateTemplate.Visible = true;
            FormProvider.Master.itemImport.Visible = true;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemGenerateTemplate.Enabled = true;
                FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocuments_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvDocuments.FocusedRowHandle;
                if (this.validateData)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.ValidateRow_DesIncorporacion(fila);
                        if (this.isValid)
                        {
                            this.AddNewRow();
                            this.gvDocuments.FocusedRowHandle = this.creditos.Count - 1;
                        }
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        if (fila >= 0 && this.creditosToSave[fila].Editable.Value.Value)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.isValid = true;
                                this.deleteOp = true;
                                this.creditosToSave.RemoveAt(fila);
                                this.gcDocuments.RefreshDataSource();

                                if (fila == 0)
                                    this.gvDocuments.FocusedRowHandle = 0;
                                else
                                    this.gvDocuments.FocusedRowHandle = fila - 1; 
                                
                                this.deleteOp = false;
                            }

                            e.Handled = true;
                        }
                        else
                            e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesIncorporacion.cs", "gcDocuments_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            #region Generales

            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                    this.creditosToSave[e.RowHandle].Rechazado.Value = false;
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this.creditosToSave[e.RowHandle].Aprobado.Value = false;
            }

            #endregion
            this.gcDocuments.RefreshDataSource();

            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Evento que valida las columna de la grilla despues de editarlas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gcDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + fieldName];
                DTO_ccCreditoDocu credito = this.creditosToSave[e.RowHandle];

                if (fieldName == "Libranza")
                {
                    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        credito = new DTO_ccCreditoDocu();
                        credito.Editable.Value = true;
                        credito.Aprobado.Value = true;
                        credito.Rechazado.Value = false;
                        credito.Libranza.Value = 0;
                    }
                    else
                    {
                        int libTemp = (int)e.Value;
                        List<DTO_ccCreditoDocu> crediTemp = (from c in this.creditos where c.Libranza.Value == libTemp select c).ToList();
                        if (crediTemp.Count > 1)
                        {
                            this.gvDocuments.SetColumnError(col, this.msgCredExiste);
                            this.isValid = false;
                        }
                        else
                        {
                            string obs = credito.Observacion.Value;

                            credito = _bc.AdministrationModel.GetCreditoByLibranza(libTemp);
                            if (credito == null)
                            {
                                credito = new DTO_ccCreditoDocu();
                                credito.Libranza.Value = libTemp;
                            }
                            else
                            {
                                DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, credito.ClienteID.Value, true);
                                credito.Nombre.Value = cliente.Descriptivo.Value;

                                DTO_ccCentroPagoPAG cp = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, credito.CentroPagoID.Value, true);
                                credito.Otro.Value = cp.Descriptivo.Value;
                            }

                            credito.Editable.Value = true;
                            credito.Aprobado.Value = false;
                            credito.Rechazado.Value = false;
                            credito.NumeroINC.Value = 0;
                            credito.Otro1.Value = ((byte)TipoNovedad.Desincorpora).ToString();
                            credito.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();
                            credito.Observacion.Value = obs;

                            this.creditosToSave[e.RowHandle] = credito;
                        }
                    }
                }

                this.gcDocuments.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desincorporacion.cs", "gvDetail_CellValueChanged"));
            }
        }

        /// <summary>
        /// Evento que valida las columna de la grilla antes de salir de esta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocuments_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.validateData)
            {
                int fila = e.RowHandle;
                if (!this.deleteOp)
                {
                    this.ValidateRow_DesIncorporacion(fila);
                    if (!this.isValid)
                        e.Allow = false;
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta la momento de cambiar entre filas de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int fila = e.FocusedRowHandle;
            if (this.validateData && fila >= 0)
            {
                if (this.creditosToSave[fila].Editable.Value.Value)
                    gvDocuments.Columns[2].OptionsColumn.AllowEdit = true;
                else
                    gvDocuments.Columns[2].OptionsColumn.AllowEdit = false;
            }
        }
        
        #endregion 

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                if (((this.creditosToSave != null && this.creditosToSave.Count != 0)) && this.ValidateDoc() && this.isValid)
                {
                    Thread process = new Thread(this.ApproveThread);
                    this.creditosToSave.Where(c => c.Aprobado.Value == true).ToList().ForEach(cc => cc.FechaDesIncorpora.Value = this.dtFecha.DateTime);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesIncorporacion.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para generar la plantilla de importar datos
        /// </summary>
        public override void TBGenerateTemplate()
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    string colName = cols[colIndex];
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Evento para importar datos de excel
        /// </summary>
        public override void TBImport()
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                        Thread process = new Thread(this.ImportThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TBGenerateTemplate.cs", "TBImport"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected override void ApproveThread()
        {
            try
            {

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                
                results = _bc.AdministrationModel.DesIncorporacion_AprobarRechazar(documentID, this.actividadFlujoID, this.creditosToSave, this.dtFecha.DateTime);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                int i = 0;
                int percent = 0;
                #region Variables para el mail

                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this.actividadDTO.seUsuarioID.Value);

                string body = string.Empty;
                string subject = string.Empty;
                string email = user.CorreoElectronico.Value;

                string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                string subjectRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                string bodyRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_RejectedCartera_Body);
                string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                #endregion
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object result in results)
                {
                    #region Manejo de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion
                    #region Envia el correo de los creditos aprobados
                    DTO_ccCreditoDocu crediAprobacion = this.creditosToSave[i];
                    if (crediAprobacion.Aprobado.Value.Value || crediAprobacion.Rechazado.Value.Value)
                    {
                        if (result.GetType() == typeof(DTO_TxResult))
                            resultsNOK.Add((DTO_TxResult)result);
                        else
                        {
                            #region Envia el correo
                            if (crediAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, crediAprobacion.NumeroDoc.Value, crediAprobacion.ClienteID.Value,
                                    crediAprobacion.Observacion.Value);
                            }
                            else if (crediAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, crediAprobacion.Observacion.Value, crediAprobacion.NumeroDoc.Value,
                                    crediAprobacion.ClienteID.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }

                    }
                    #endregion
                    i++;
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (resultsNOK.Count == 0)
                {
                    this.Invoke(this.refreshData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desincoroparcion.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        private void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error

                    string msgNoSaldo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldos);

                    DTO_TxResult result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    Dictionary<string, bool> centrosPago = new Dictionary<string, bool>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();

                    //Mensajes de error
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgPkAdded = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReIncorporacionPkAdded);

                    List<DTO_ccCreditoDocu> list = new List<DTO_ccCreditoDocu>();
                    List<DTO_ccCreditoDocu> finalList = new List<DTO_ccCreditoDocu>();

                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas

                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    colNames.Add(this.libranzaRsx, "Libranza");
                    colNames.Add(this.centroPagoRsx, "CentroPagoID");
                    colNames.Add(this.vlrCuotaRsx, "VlrCuota");

                    colVals.Add(this.libranzaRsx, string.Empty);
                    colVals.Add(this.centroPagoRsx, string.Empty);
                    colVals.Add(this.vlrCuotaRsx, string.Empty);

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)

                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer

                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }

                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_ccCreditoDocu cred = new DTO_ccCreditoDocu();
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica

                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];

                                    #region Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        #region Centro de pago
                                        if (colRsx == this.centroPagoRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();
                                            if (centrosPago.ContainsKey(line[colIndex].Trim()))
                                            {
                                                if (centrosPago[line[colIndex].Trim()])
                                                    continue;
                                                else
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            else
                                            {
                                                DTO_MasterBasic cp = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, line[colIndex], true);
                                                if (cp != null)
                                                {
                                                    centrosPago[colRsx] = true;
                                                }
                                                else
                                                {
                                                    centrosPago[colRsx] = false;

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion // Carga la info de las fks
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                cred = new DTO_ccCreditoDocu();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colName == this.libranzaRsx || colName == this.masterCentroPago.ColId))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        PropertyInfo pi = cred.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(cred, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                    if (val <= 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                    if (val <= 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                    if (val <= 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                    if (val <= 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                    else if (val <= 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        }
                                        #endregion  Validacion si no es null y formatos

                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                        {
                                            udt.SetValueFromString(colValue);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp-Reincorporacion.cs", "ImportThread - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la información de los resultados

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                                list.Add(cred);
                            else
                                validList = false;

                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Validaciones de los datos

                    if (validList)
                    {
                        int i = 0;
                        percent = 0;
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        foreach (DTO_ccCreditoDocu cred in list)
                        {
                            bool isValid = true;

                            #region Carga de porcentaje
                            ++i;
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (list.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            #region Valida los nuevos créditos
                            if (isValid)
                            {
                                DTO_ccCreditoDocu newCred = null;
                                if (this.creditos.Any(c => c.Libranza.Value == cred.Libranza.Value && !c.Editable.Value.Value))
                                {
                                    // Créditos existentes
                                    newCred = creditos.FirstOrDefault(c => c.Libranza.Value == cred.Libranza.Value);
                                    if (newCred.CentroPagoID.Value != cred.CentroPagoID.Value)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = this.msgInvalidCP;
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                }
                                else
                                {
                                    #region Nuevos créditos

                                    DTO_ccCreditoDocu temp = _bc.AdministrationModel.GetCreditoByLibranza(cred.Libranza.Value.Value);
                                    if (temp == null)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = string.Format(this.msgLibranzaInvalida, cred.Libranza.Value.ToString());
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else if (temp.CanceladoInd.Value.Value)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = string.Format(this.msgLibranzaCancelada, cred.Libranza.Value.ToString());
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else if (temp.CentroPagoID.Value != cred.CentroPagoID.Value)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = this.msgInvalidCP;
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else
                                    {
                                        newCred = ObjectCopier.Clone(temp);

                                        // VlrCuota
                                        if (cred.VlrCuota.Value != null && cred.VlrCuota.Value.HasValue && cred.VlrCuota.Value.Value > 0)
                                        {
                                            newCred.VlrCuota.Value = cred.VlrCuota.Value;
                                        }

                                        //Cliente
                                        DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, newCred.ClienteID.Value, true);
                                        newCred.Nombre.Value = cliente.Descriptivo.Value;

                                        //CP
                                        DTO_ccCentroPagoPAG cp = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, newCred.CentroPagoID.Value, true);
                                        newCred.Otro.Value = cp.Descriptivo.Value;

                                        newCred.Editable.Value = true;
                                        newCred.Aprobado.Value = false;
                                        newCred.Rechazado.Value = false;
                                        newCred.NumeroINC.Value = 0;
                                        newCred.Otro1.Value = ((byte)TipoNovedad.Desincorpora).ToString();
                                        newCred.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();

                                    }
                                    #endregion
                                }

                                if (isValid)
                                {
                                    newCred.Aprobado.Value = true;
                                    newCred.Rechazado.Value = false;
                                    newCred.Editable.Value = true;
                                    newCred.Otro1.Value = ((byte)TipoNovedad.Desincorpora).ToString();
                                    newCred.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();

                                    finalList.Add(newCred);
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion

                    if (validList)
                    {
                        this.creditos.RemoveAll(cr => cr.Editable.Value.Value);
                        List<int> libs = finalList.Select(l => l.Libranza.Value.Value).Distinct().OrderByDescending(l1 => l1).ToList();
                        List<DTO_ccCreditoDocu> newList = new List<DTO_ccCreditoDocu>();
                        libs.ForEach(l =>
                        {
                            this.creditos.RemoveAll(r => r.Libranza.Value == l);

                            var listLib = finalList.Where(fl => fl.Libranza.Value == l);
                            newList.AddRange(listLib);
                        });

                        this.creditos.InsertRange(0, newList);
                        this.Invoke(this.refreshGridDelegate);
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desincoroparcion.cs", "ImportThread"));
            }
            finally
            {
                if (!this.pasteRet.Success)
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
