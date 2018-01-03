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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionComisiones : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables
        private int currentRow = -1;

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private DTO_LiquidacionComisiones liquidaComisiones = new DTO_LiquidacionComisiones();
        private List<DTO_ccComisionDeta> comisiones = new List<DTO_ccComisionDeta>();
        private DTO_ccComisionDeta detComisiones = new DTO_ccComisionDeta();
        private GridView _gridDetalleCurrent = new GridView();

        //Variables privadas
        DateTime periodo;

        #endregion

        public LiquidacionComisiones()
            : base()
        {
            //InitializeComponent();
        }

        public LiquidacionComisiones(string mod)
            : base(mod)
        {
            this.dtPeriod.Enabled = true;
            this.dtFecha.Visible = false;
            
        }

        #region Funciones Privadas
        
        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.comisiones = new List<DTO_ccComisionDeta>();
            this.gcDocument.DataSource = this.comisiones;
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void LoadDocuments()
        {
            try
            {
                
                this.comisiones = this._bc.AdministrationModel.LiquidacionComisionesCartera_GetForLiquidacion(this.dtPeriod.DateTime);
                if (this.comisiones.Count > 0)
                {
                    this.CalcularValoresGrid();
                    this.gcDocument.DataSource = this.comisiones;
                    this.gvDocument.BestFitColumns();
                    this.gvDetalle.BestFitColumns();
                    this.gvDocument.MoveFirst();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Funcion que calcula los valores de la grilla
        /// </summary>
        private void CalcularValoresGrid()
        {
            foreach (DTO_ccComisionDeta item in this.comisiones)
            {
                decimal porcComision = item.Porcentaje.Value.Value;
                item.NumCreditos.Value = item.Detalle.Count;
                item.VlrBase.Value = (from c in item.Detalle select c.VlrGiro.Value.Value).Sum();
                item.VlrBasePrivado.Value = (from c in item.Detalle select c.VlrBasePrivado.Value.Value).Sum();
                item.VlrBasePublico.Value = (from c in item.Detalle select c.VlrBasePublico.Value.Value).Sum();
                item.VlrComision.Value = (item.VlrBase.Value.Value * porcComision) / 100;
                item.Detalle.ForEach(x => x.VlrComision.Value = (x.VlrGiro.Value.Value * porcComision) / 100);
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {                
                this.InitializeComponent();
                base.SetInitParameters();

                
                this.documentID = AppDocuments.LiquidacionComisiones;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();
                
                this.gvDocument.OptionsView.ShowAutoFilterRow = true;  
                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 140;
                this.tlSeparatorPanel.RowStyles[1].Height = 400;

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);


                this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas Principales
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
                this.gvDocument.Columns.Add(aprob);

                //AsesorID
                GridColumn asesorID = new GridColumn();
                asesorID.FieldName = this.unboundPrefix + "AsesorID";
                asesorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AsesorID");
                asesorID.UnboundType = UnboundColumnType.String;
                asesorID.VisibleIndex = 1;
                asesorID.Width = 70;
                asesorID.Visible = true;
                asesorID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(asesorID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 2;
                nombre.Width = 300;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombre);

                //PagaduriaID
                GridColumn PagaduriaID = new GridColumn();
                PagaduriaID.FieldName = this.unboundPrefix + "PagaduriaID";
                PagaduriaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagaduriaID");
                PagaduriaID.UnboundType = UnboundColumnType.String;
                PagaduriaID.VisibleIndex = 3;
                PagaduriaID.Width = 70;
                PagaduriaID.Visible = true;
                PagaduriaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PagaduriaID);

                //Pagaduria
                GridColumn Pagaduria = new GridColumn();
                Pagaduria.FieldName = this.unboundPrefix + "Pagaduria";
                Pagaduria.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Pagaduria");
                Pagaduria.UnboundType = UnboundColumnType.String;
                Pagaduria.VisibleIndex = 4;
                Pagaduria.Width = 300;
                Pagaduria.Visible = true;
                Pagaduria.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Pagaduria);

                //NumCreditos
                GridColumn numCreditos = new GridColumn();
                numCreditos.FieldName = this.unboundPrefix + "NumCreditos";
                numCreditos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumCreditos");
                numCreditos.UnboundType = UnboundColumnType.Integer;
                numCreditos.VisibleIndex = 5;
                numCreditos.Width = 80;
                numCreditos.Visible = true;
                numCreditos.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numCreditos);

                //VlrGiro
                GridColumn vlrGiro = new GridColumn();
                vlrGiro.FieldName = this.unboundPrefix + "VlrBase"; //VlrGiro = VlrBase
                vlrGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGiro");
                vlrGiro.UnboundType = UnboundColumnType.Decimal;
                vlrGiro.VisibleIndex = 6;
                vlrGiro.Width = 100;
                vlrGiro.OptionsColumn.AllowEdit = false;
                vlrGiro.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(vlrGiro);

                //vlrBasePrivado
                GridColumn vlrBasePrivado = new GridColumn();
                vlrBasePrivado.FieldName = this.unboundPrefix + "VlrBasePrivado"; 
                vlrBasePrivado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrBasePrivado");
                vlrBasePrivado.UnboundType = UnboundColumnType.Decimal;
                vlrBasePrivado.VisibleIndex = 7;
                vlrBasePrivado.Width = 100;
                vlrBasePrivado.OptionsColumn.AllowEdit = false;
                vlrBasePrivado.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(vlrBasePrivado);

                //vlrBasePublico
                GridColumn vlrBasePublico = new GridColumn();
                vlrBasePublico.FieldName = this.unboundPrefix + "VlrBasePublico";
                vlrBasePublico.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrBasePublico");
                vlrBasePublico.UnboundType = UnboundColumnType.Decimal;
                vlrBasePublico.VisibleIndex = 8;
                vlrBasePublico.Width = 100;
                vlrBasePublico.OptionsColumn.AllowEdit = false;
                vlrBasePublico.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(vlrBasePublico);
                                   
                //Porcetanje
                GridColumn porcentaje = new GridColumn();
                porcentaje.FieldName = this.unboundPrefix + "Porcentaje";
                porcentaje.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Porcentaje");
                porcentaje.UnboundType = UnboundColumnType.Decimal;
                porcentaje.VisibleIndex = 9;
                porcentaje.Width = 100;
                porcentaje.OptionsColumn.AllowEdit = true;
                porcentaje.ColumnEdit = this.editSpinPorcen;
                    
                this.gvDocument.Columns.Add(porcentaje);

                //VlrComision
                GridColumn vlrComision = new GridColumn();
                vlrComision.FieldName = this.unboundPrefix + "VlrComision";
                vlrComision.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrComision");
                vlrComision.UnboundType = UnboundColumnType.Boolean;
                vlrComision.VisibleIndex = 10;
                vlrComision.Width = 100;
                vlrComision.OptionsColumn.AllowEdit = false;
                vlrComision.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrComision);
                #endregion

                #region Columnas Detalle

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 1;
                libranza.Width = 70;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(libranza);

                //FechaLiquida
                GridColumn fechaLiquida = new GridColumn();
                fechaLiquida.FieldName = this.unboundPrefix + "FechaLiquida";
                fechaLiquida.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaLiquida");
                fechaLiquida.UnboundType = UnboundColumnType.DateTime;
                fechaLiquida.VisibleIndex = 2;
                fechaLiquida.Width = 100;
                fechaLiquida.Visible = true;
                fechaLiquida.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(fechaLiquida);

                //VlrLibranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Decimal;
                vlrLibranza.VisibleIndex = 3;
                vlrLibranza.Width = 100;
                vlrLibranza.Visible = true;
                vlrLibranza.OptionsColumn.AllowEdit = false;
                vlrLibranza.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrLibranza);

                //VlrPrestamo
                GridColumn vlrPrestamo = new GridColumn();
                vlrPrestamo.FieldName = this.unboundPrefix + "VlrPrestamo";
                vlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrestamo");
                vlrPrestamo.UnboundType = UnboundColumnType.Decimal;
                vlrPrestamo.VisibleIndex = 4;
                vlrPrestamo.Width = 100;
                vlrPrestamo.OptionsColumn.AllowEdit = false;
                vlrPrestamo.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrPrestamo);

                //VlrGiro
                GridColumn vlrGiroDetalle = new GridColumn();
                vlrGiroDetalle.FieldName = this.unboundPrefix + "VlrGiro";
                vlrGiroDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGiro");
                vlrGiroDetalle.UnboundType = UnboundColumnType.String;
                vlrGiroDetalle.VisibleIndex = 5;
                vlrGiroDetalle.Width = 100;
                vlrGiroDetalle.OptionsColumn.AllowEdit = false;
                vlrGiroDetalle.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrGiroDetalle);

                //VlrBasePrivado
                GridColumn VlrBasePrivadoDetalle = new GridColumn();
                VlrBasePrivadoDetalle.FieldName = this.unboundPrefix + "VlrBasePrivado";
                VlrBasePrivadoDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrBasePrivado");
                VlrBasePrivadoDetalle.UnboundType = UnboundColumnType.String;
                VlrBasePrivadoDetalle.VisibleIndex = 6;
                VlrBasePrivadoDetalle.Width = 100;
                VlrBasePrivadoDetalle.OptionsColumn.AllowEdit = false;
                VlrBasePrivadoDetalle.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(VlrBasePrivadoDetalle);

                //vlrBasePublico
                GridColumn vlrBasePublicoDetalle = new GridColumn();
                vlrBasePublicoDetalle.FieldName = this.unboundPrefix + "VlrBasePublico";
                vlrBasePublicoDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrBasePublico");
                vlrBasePublicoDetalle.UnboundType = UnboundColumnType.String;
                vlrBasePublicoDetalle.VisibleIndex = 7;
                vlrBasePublicoDetalle.Width = 100;
                vlrBasePublicoDetalle.OptionsColumn.AllowEdit = false;
                vlrBasePublicoDetalle.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrBasePublicoDetalle);

                //VlrComision
                GridColumn vlrComisionDetalle = new GridColumn();
                vlrComisionDetalle.FieldName = this.unboundPrefix + "VlrComision";
                vlrComisionDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrComision");
                vlrComisionDetalle.UnboundType = UnboundColumnType.Boolean;
                vlrComisionDetalle.VisibleIndex = 8;
                vlrComisionDetalle.Width = 100;
                vlrComisionDetalle.OptionsColumn.AllowEdit = true;
                vlrComisionDetalle.ColumnEdit = editSpin;
                this.gvDetalle.Columns.Add(vlrComisionDetalle);

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "AddGridCols"));
            }

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
            try
            {
                base.Form_Enter(sender, e);
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                    FormProvider.Master.itemGenerateTemplate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate);
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemDelete.Visible = false;
                    FormProvider.Master.itemSendtoAppr.Visible = false;
                    FormProvider.Master.itemCopy.Visible = false;
                    FormProvider.Master.itemPaste.Visible = false;
                    FormProvider.Master.itemImport.Visible = true;
                    FormProvider.Master.itemExport.Visible = true;
                    FormProvider.Master.itemRevert.Visible = false;
                    FormProvider.Master.itemGenerateTemplate.Visible = true;
                    FormProvider.Master.itemFilter.Visible = false;
                    FormProvider.Master.itemFilterDef.Visible = false;
                    FormProvider.Master.tbBreak1.Visible = false;
                    FormProvider.Master.tbBreak2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que trae la informacion de las comisiones a liquidar
        /// </summary>
        private void btnLiquidar_Click(object sender, EventArgs e)
        {
            this.LoadDocuments();
        }


        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
             try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            
                #region Generales
                if (fieldName == "Porcentaje")
                {
                    decimal cantidad = Convert.ToDecimal(e.Value);

                    this.detComisiones = (DTO_ccComisionDeta)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);

                    if (cantidad == Convert.ToDecimal(this.detComisiones.Porcentaje.Value))
                    {
                        //this.periodo = Convert.ToDateTime(strPeriodo);
                    }
                    else
                    {
                        this.comisiones[this._gridDetalleCurrent.FocusedRowHandle].Porcentaje.Value = cantidad;
                        this.CalcularValoresGrid();
                        this.gcDocument.DataSource = this.comisiones;
                        this.gvDocument.BestFitColumns();
                        this.gvDetalle.BestFitColumns();
                        //this.gvDocument.MoveFirst();
                    

                    }
                }

                #endregion

                this.gcDocument.RefreshDataSource();
            }
             catch (Exception ex)
             {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "gvDocument_CellValueChanging"));
             }
        }
        //gvDocument_FocusedColumnChanged
        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.deleteOP && this.gvDocument.DataRowCount > 0)
                    this.ValidateRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>

        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.currentRow != -1)
                {
                    if (e.FocusedRowHandle <= this.gvDocument.RowCount - 1)
                        this.currentRow = e.FocusedRowHandle;
                }

                if (e.FocusedRowHandle >= 0)
                {
                    this._gridDetalleCurrent = (GridView)sender;
                    this.detComisiones = (DTO_ccComisionDeta)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "gvDocument_FocusedRowChanged"));
            }
        }
        
        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                this.comisiones=this.comisiones.Where(x => x.Aprobado.Value == true).ToList();
                if (this.comisiones.Count > 0)
                {                                        
                        DTO_ccComisionDocu comiDocu = new DTO_ccComisionDocu();
                        this.liquidaComisiones.ComisionDeta = this.comisiones;
                        
                        comiDocu.FechaInicial.Value = this.dtFecha.DateTime;
                        comiDocu.Valor.Value = (from c in this.comisiones where c.Aprobado.Value.Value select c.VlrComision.Value.Value).Sum();
                    
                        this.liquidaComisiones.ComisionDocu = comiDocu;
                        this.liquidaComisiones.ComisionDeta = this.comisiones;
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                #region Guarda la info
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                results = _bc.AdministrationModel.LiquidacionComisionesCartera_Add(this.documentID, this._actFlujo.ID.Value, this.liquidaComisiones);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                this.isValid = true;
                MessageForm frm = null;
                bool checkResults = true;
                if (results.Count == 1)
                {
                    if (results[0].GetType() == typeof(DTO_TxResult))
                    {
                        checkResults = false;
                        frm = new MessageForm((DTO_TxResult)results[0]);
                        this.isValid = false;
                    }
                }

                if (checkResults)
                {
                    foreach (object obj in results)
                    {
                        #region Funciones de progreso
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (results.Count);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                            break;
                        }
                        #endregion

                        if (this.comisiones[i].Aprobado.Value.Value)
                        {
                            bool isOK = this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, obj, false);
                            if (!isOK)
                            {
                                DTO_TxResult r = (DTO_TxResult)obj;
                                resultsNOK.Add(r);
                                this.isValid = false;
                            }
                        }

                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (this.isValid)
                    this.Invoke(this.refreshGridDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        
        #endregion       

    }
        
}
