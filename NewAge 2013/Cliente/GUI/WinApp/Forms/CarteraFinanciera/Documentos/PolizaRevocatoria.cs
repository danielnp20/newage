using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;
using SentenceTransformer;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PolizaRevocatoria : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private string libranzaID = string.Empty;
        private DTO_InfoCredito infoCartera = new DTO_InfoCredito();
        private List<DTO_ccCreditoDocu> _creditosRevocar = null;
        private DTO_ccCreditoDocu _creditoCurrent = null;
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private bool validate = true;
        private DateTime fechaPerido;

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public PolizaRevocatoria()  : base() 
        {
            //InitializeComponent();
        }

        public PolizaRevocatoria(string mod) : base(mod) 
        {
            //this.InitializeComponent(); 
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.PolizaRevocatoria;
            this.frmModule = ModulesPrefix.cc;

            InitializeComponent();
            base.SetInitParameters();

            this.AddDocumentCols();
            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[0].Height = 180;
            this.tlSeparatorPanel.RowStyles[1].Height = 340;
            this.tlSeparatorPanel.RowStyles[2].Height = 100;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.grpboxDetail.Dock = DockStyle.Fill;

            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Carga la Informacion del Hedear
            _bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            _bc.InitMasterUC(this.masterAseguradora, AppMasters.ccAseguradora, true, true, true, false);
            this.fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this.dtPeriod.DateTime = this.fechaPerido;
        }

        ///// <summary>
        ///// Agrega las columnas a la grilla superior
        ///// </summary>
        protected virtual void AddDocumentCols()
        {
            try
            {
                //Seleccionar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                aprob.ToolTip = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.VisibleIndex = 0;
                aprob.Width = 35;
                aprob.Visible = true;
                this.gvDocument.Columns.Add(aprob);

                //ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this.unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 1;
                ClienteID.Width = 100;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ClienteID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this.unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 2;
                Nombre.Width = 250;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                Nombre.ColumnEdit = this.editDate;
                this.gvDocument.Columns.Add(Nombre);

                //Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this.unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 3;
                Libranza.Width = 80;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Libranza);

                //Poliza
                GridColumn Poliza = new GridColumn();
                Poliza.FieldName = this.unboundPrefix + "Poliza";
                Poliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Poliza");
                Poliza.UnboundType = UnboundColumnType.String;
                Poliza.VisibleIndex = 4;
                Poliza.Width = 110;
                Poliza.Visible = true;
                Poliza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Poliza);

                //VlrSaldoSeguro
                GridColumn VlrSaldoSeguro = new GridColumn();
                VlrSaldoSeguro.FieldName = this.unboundPrefix + "VlrSaldoSeguro";
                VlrSaldoSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldoSeguro");
                VlrSaldoSeguro.UnboundType = UnboundColumnType.Decimal;
                VlrSaldoSeguro.VisibleIndex = 5;
                VlrSaldoSeguro.Width = 110;
                VlrSaldoSeguro.Visible = true;
                VlrSaldoSeguro.OptionsColumn.AllowEdit = false;
                VlrSaldoSeguro.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(VlrSaldoSeguro);

                //Vlr Revoca
                GridColumn vlrRevocaCartera = new GridColumn();
                vlrRevocaCartera.FieldName = this.unboundPrefix + "VlrRevoca";
                vlrRevocaCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrRevoca");
                vlrRevocaCartera.UnboundType = UnboundColumnType.Decimal;
                vlrRevocaCartera.VisibleIndex = 6;
                vlrRevocaCartera.Width = 110;
                vlrRevocaCartera.Visible = true;
                vlrRevocaCartera.OptionsColumn.AllowEdit = true;
                vlrRevocaCartera.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(vlrRevocaCartera);

                //VlrDiferencia
                GridColumn VlrDiferencia = new GridColumn();
                VlrDiferencia.FieldName = this.unboundPrefix + "VlrDiferencia";
                VlrDiferencia.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrDiferencia");
                VlrDiferencia.UnboundType = UnboundColumnType.Integer;
                VlrDiferencia.VisibleIndex = 7;
                VlrDiferencia.Width = 160;
                VlrDiferencia.Visible = true;
                VlrDiferencia.OptionsColumn.AllowEdit = true;
                VlrDiferencia.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(VlrDiferencia);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            try
            {
                base.AfterInitialize();
                if (this.fechaPerido.Month == DateTime.Now.Date.Month)
                {
                    this.dtFecha.DateTime = DateTime.Now.Date;
                    this.dtFechaRevoca.DateTime = DateTime.Now.Date;
                }
                else
                {
                    this.dtFecha.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
                    this.dtFechaRevoca.DateTime = this.dtFecha.DateTime;
                }

                //Pone la fecha de consignacion con base a la del periodo
                this.dtFechaRevoca.Properties.MaxValue = this.dtFecha.DateTime;             
           }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "AfterInitialize"));
            }

        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;

            this.libranzaID = string.Empty;
            this.masterPrefijo.Value = String.Empty;
            this.masterAseguradora.Value = String.Empty;
            this.lkp_Libranzas.Properties.DataSource = string.Empty;

            this._creditosRevocar = null;
            this._creditoCurrent = null;
            this._ctrl = new DTO_glDocumentoControl();
            this.gcDocument.DataSource = this._creditosRevocar;

            this.validate = true;
        }

        /// <summary>
        ///Carga la info del acta antes de guardar
        /// </summary>
        private void LoadCtrl()
        {
            try
            {
                if (this._ctrl != null && string.IsNullOrEmpty(this._ctrl.NumeroDoc.Value.ToString()))
                {                    
                    #region Carga DocumentoControl
                    this._ctrl = new DTO_glDocumentoControl();
                    this._ctrl.NumeroDoc.Value = 0;
                    this._ctrl.DocumentoID.Value = AppDocuments.PolizaRevocatoria;
                    this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    this._ctrl.MonedaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    this._ctrl.ProyectoID.Value = string.Empty;
                    this._ctrl.Fecha.Value = DateTime.Now;                  
                    this._ctrl.PrefijoID.Value = this.txtPrefix.Text ;
                    this._ctrl.TasaCambioCONT.Value = 0;
                    this._ctrl.TasaCambioDOCU.Value = 0;
                    this._ctrl.DocumentoNro.Value = 0;
                    this._ctrl.PeriodoDoc.Value = new DateTime(this.dtFechaRevoca.DateTime.Year,this.dtFechaRevoca.DateTime.Month,1);
                    this._ctrl.PeriodoUltMov.Value = new DateTime(this.dtFechaRevoca.DateTime.Year, this.dtFechaRevoca.DateTime.Month, 1); ;
                    this._ctrl.seUsuarioID.Value = this.userID;
                    this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this._ctrl.ConsSaldo.Value = 0;
                    this._ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    this._ctrl.Observacion.Value = this.txtDocDesc.Text;
                    this._ctrl.FechaDoc.Value = this.dtFechaRevoca.DateTime;
                    this._ctrl.Descripcion.Value = "Revocatoria Poliza Credito";
                    this._ctrl.DocumentoTercero.Value = this.txtDocRef.Text ;
                    this._ctrl.Valor.Value = 0;
                    this._ctrl.Iva.Value = 0;
                    #endregion                    
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "CreateActaDeta"));
            }
        }

        /// <summary>
        /// Funcion para cargar la grilla de pagos 
        /// </summary>
        private void LoadRevocatoriasGrid()
        {        
            try
            {
                this.gcDocument.DataSource = null;
                this._creditosRevocar = this._creditosRevocar.FindAll(x =>!string.IsNullOrEmpty(x.Poliza.Value)).ToList();
                this._creditosRevocar = this._creditosRevocar.OrderBy(x => x.NumeroDoc.Value).ToList();
                this.gcDocument.DataSource = this._creditosRevocar;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "LoadPagosGrid"));
            }
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
            if (!this.masterAseguradora.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAseguradora.LabelRsx);
                MessageBox.Show(msg);
                this.masterAseguradora.Focus();
                return false;
            }

            if (this.gvDocument.HasColumnErrors)
                return false;
            return true;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e); 
            
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
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_Leave(object sender, System.EventArgs e)
        {
            try
            {
                //if (this.masterPrefijo.ValidID)
                //    this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostos.cs-txtNro_Leave"));
            }
        }

        /// <summary>
        /// Se encargar de buscar un documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                //List<int> docs = new List<int>();
                //docs.Add(AppDocuments.PreProyecto);
                //ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, true);
                //getDocControl.ShowDialog();
                //if (getDocControl.DocumentoControl != null)
                //{
                //    if (getDocControl.CopiadoInd)
                //        this._copyData = true;
                //    this.txtNro.Enabled = true;
                //    this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                //    this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                //    this.txtNro.Focus();
                //    this.btnQueryDoc.Focus();
                //    this.btnQueryDoc.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos", "btnQueryDoc_Click"));
            }
        }

        /// <summary>
        /// Evento que filtra una lista de DTO_ccCreditoDocu de acuerdo al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterAseguradora_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterAseguradora.ValidID)
                {
                    this._creditosRevocar = new List<DTO_ccCreditoDocu>();
                    this._creditosRevocar = _bc.AdministrationModel.EstadoCuenta_GetForRevocatoria(this.masterAseguradora.Value);
                    this.LoadRevocatoriasGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "masterAseguradora_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            }

            this.dtFechaRevoca.Properties.MaxValue = this.dtFecha.DateTime;
            this.dtFechaRevoca.DateTime = base.dtFecha.DateTime;
        }

        #endregion

        #region Eventos Grilla Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
        }

        /// <summary>
        /// Evento q valida antes de dejar el row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
            try
            {
                if (fieldName == "VlrRevoca")
                {
                    //if (this._creditoCurrent.VlrRevoca.Value > this._creditoCurrent.VlrSaldoSeguro.Value)
                    //{
                    //    string msg = string.Format("", fieldName);
                    //    this.gvDocument.SetColumnError(col, "El valor a revocar no puede superar el saldo del seguro");
                    //}
                    //else
                    //{
                        this._creditoCurrent.VlrDiferencia.Value = this._creditoCurrent.VlrSaldoSeguro.Value - this._creditoCurrent.VlrRevoca.Value;
                        this.gvDocument.SetColumnError(col, string.Empty);                        
                    //}                   
                    decimal vlrTotal = this._creditosRevocar.FindAll(x => x.Aprobado.Value.Value).Sum(y => y.VlrRevoca.Value.Value);
                    this.txtVlrRevocar.EditValue = vlrTotal;
                    this.gvDocument.RefreshData();
                }

                if (fieldName == "Aprobado")
                {
                    decimal vlrTotal = this._creditosRevocar.FindAll(x => x.Aprobado.Value.Value).Sum(y => y.VlrRevoca.Value.Value);
                    this.txtVlrRevocar.EditValue = vlrTotal;                   
                }
            }
            catch (Exception ex)
            {                
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.gvDocument.HasColumnErrors)
                e.Allow = false;
            else
                e.Allow = true;
        }

        /// <summary>
        /// Evento q valida al salir de un Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this._creditosRevocar.Count > 0)
                {
                    this._creditoCurrent = (DTO_ccCreditoDocu)this.gvDocument.GetRow(e.FocusedRowHandle);
                    if (string.IsNullOrEmpty(this._creditoCurrent.Poliza.Value))
                        this.gvDocument.Columns[this.unboundPrefix + "Aprobado"].OptionsColumn.AllowEdit = false;
                    else
                        this.gvDocument.Columns[this.unboundPrefix + "Aprobado"].OptionsColumn.AllowEdit = true;
                }                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "gvDocument_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();
                bool isValid = this.ValidateDoc();
                this.LoadCtrl();
                //Valida la ultima fila del plan de pagos
                if (isValid && this._creditosRevocar.Any(x=>x.VlrRevoca.Value > 0))
                {
                    #region Validaciones
                    bool update = false;
                    if (this._ctrl.NumeroDoc.Value != 0)
                        update = true;

                    #endregion                    
                    #region Guarda la info
                    DTO_TxResult result = _bc.AdministrationModel.PolizaRevocatoria_Add(this.documentID, this.masterAseguradora.Value,this._ctrl, this._creditosRevocar.FindAll(x => x.VlrRevoca.Value > 0),update);
                    if (result.Result == ResultValue.OK)
                    {
                        bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._bc.AdministrationModel.User.ID.Value, result, true, true);
                        this.CleanData();
                        this.masterAseguradora.Focus();
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRevocatoria.cs", "TBSave"));
            }
        }

        #endregion
    }
}
