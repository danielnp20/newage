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
using DevExpress.XtraGrid.Views.Base;
using SentenceTransformer;
using System.Linq;
using System.Threading;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionLiquidacionComisiones : DocumentAprobBasicForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.CleanData();
            this.LoadDocuments();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private int docRecursos;
        
        //Listas de datos
        private List<DTO_ccComisionDeta> liquidaComisionDeta = new List<DTO_ccComisionDeta>();

        //Variables Privadas
        private string compoCarteraID = String.Empty;
        private decimal vlrTotal = 0;
        private bool isValid = true;
        private DateTime periodo;

        #endregion

        public AprobacionLiquidacionComisiones()
            : base() { }

        public AprobacionLiquidacionComisiones(string mod)
            : base(mod) { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AprobacionLiquidacionComisiones;
            this.docRecursos = AppDocuments.LiquidacionComisiones;
            this.frmModule = ModulesPrefix.cc;

            //Estable la fecha con base a la fecha del periodo
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            this.dtAprobComisiones.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtAprobComisiones.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            base.SetInitParameters();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                #region Columnas Principal
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 50;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Aprobado");
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
                noAprob.Visible = true;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //AsesorID
                GridColumn asesorID = new GridColumn();
                asesorID.FieldName = this.unboundPrefix + "AsesorID";
                asesorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_AsesorID");
                asesorID.UnboundType = UnboundColumnType.String;
                asesorID.VisibleIndex = 2;
                asesorID.Width = 70;
                asesorID.Visible = true;
                asesorID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(asesorID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 3;
                nombre.Width = 300;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombre);

                //NumCreditos
                GridColumn numCreditos = new GridColumn();
                numCreditos.FieldName = this.unboundPrefix + "NumCreditos";
                numCreditos.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_NumCreditos");
                numCreditos.UnboundType = UnboundColumnType.Integer;
                numCreditos.VisibleIndex = 4;
                numCreditos.Width = 300;
                numCreditos.Visible = true;
                numCreditos.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(numCreditos);

                //VlrGiro
                GridColumn vlrGiro = new GridColumn();
                vlrGiro.FieldName = this.unboundPrefix + "VlrBase"; //VlrGiro = VlrBase
                vlrGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrGiro");
                vlrGiro.UnboundType = UnboundColumnType.Decimal;
                vlrGiro.VisibleIndex = 5;
                vlrGiro.Width = 100;
                vlrGiro.OptionsColumn.AllowEdit = false;
                vlrGiro.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrGiro);

                //Porcetanje
                GridColumn porcentaje = new GridColumn();
                porcentaje.FieldName = this.unboundPrefix + "Porcentaje";
                porcentaje.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Porcentaje");
                porcentaje.UnboundType = UnboundColumnType.Decimal;
                porcentaje.VisibleIndex = 6;
                porcentaje.Width = 100;
                porcentaje.OptionsColumn.AllowEdit = false;
                porcentaje.ColumnEdit = this.editSpinPorc;
                this.gvDocuments.Columns.Add(porcentaje);

                //VlrComision
                GridColumn vlrComision = new GridColumn();
                vlrComision.FieldName = this.unboundPrefix + "VlrComision";
                vlrComision.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrComision");
                vlrComision.UnboundType = UnboundColumnType.Boolean;
                vlrComision.VisibleIndex = 7;
                vlrComision.Width = 100;
                vlrComision.OptionsColumn.AllowEdit = false;
                vlrComision.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrComision); 
                #endregion
                #region Columnas Detalle

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 1;
                libranza.Width = 70;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(libranza);

                //FechaLiquida
                GridColumn fechaLiquida = new GridColumn();
                fechaLiquida.FieldName = this.unboundPrefix + "FechaLiquida";
                fechaLiquida.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_FechaLiquida");
                fechaLiquida.UnboundType = UnboundColumnType.DateTime;
                fechaLiquida.VisibleIndex = 2;
                fechaLiquida.Width = 100;
                fechaLiquida.Visible = true;
                fechaLiquida.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(fechaLiquida);

                //VlrLibranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrLibranza");
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
                vlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrPrestamo");
                vlrPrestamo.UnboundType = UnboundColumnType.Decimal;
                vlrPrestamo.VisibleIndex = 4;
                vlrPrestamo.Width = 100;
                vlrPrestamo.OptionsColumn.AllowEdit = false;
                vlrPrestamo.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrPrestamo);

                //VlrGiro
                GridColumn vlrGiroDetalle = new GridColumn();
                vlrGiroDetalle.FieldName = this.unboundPrefix + "VlrGiro";
                vlrGiroDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrGiro");
                vlrGiroDetalle.UnboundType = UnboundColumnType.String;
                vlrGiroDetalle.VisibleIndex = 5;
                vlrGiroDetalle.Width = 100;
                vlrGiroDetalle.OptionsColumn.AllowEdit = false;
                vlrGiroDetalle.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrGiroDetalle);

                //VlrComision
                GridColumn vlrComisionDetalle = new GridColumn();
                vlrComisionDetalle.FieldName = this.unboundPrefix + "VlrComision";
                vlrComisionDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrComision");
                vlrComisionDetalle.UnboundType = UnboundColumnType.Boolean;
                vlrComisionDetalle.VisibleIndex = 6;
                vlrComisionDetalle.Width = 100;
                vlrComisionDetalle.OptionsColumn.AllowEdit = true;
                vlrComisionDetalle.ColumnEdit = editSpin;
                this.gvDetalle.Columns.Add(vlrComisionDetalle);

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                //this.currentDoc = null;
                this.liquidaComisionDeta = this._bc.AdministrationModel.LiquidacionComisionesCartera_GetForAprobacion(this.actividadFlujoID);

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this.liquidaComisionDeta.Count > 0)
                {
                    //this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.CalcularValoresGrid();
                    this.gcDocuments.DataSource = this.liquidaComisionDeta;
                    this.gvDocuments.BestFitColumns();
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                }
                else
                    this.gcDocuments.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudLibranza.cs", "LoadDocuments"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que calcula los valores de la grilla
        /// </summary>
        private void CalcularValoresGrid()
        {
            foreach (DTO_ccComisionDeta item in this.liquidaComisionDeta)
            {
                decimal porcComision = item.Porcentaje.Value.Value;
                item.NumCreditos.Value = item.Detalle.Count;
                item.VlrBase.Value = (from c in item.Detalle select c.VlrGiro.Value.Value).Sum();
                item.VlrComision.Value = (item.VlrBase.Value.Value * porcComision) / 100;
                item.Detalle.ForEach(x => x.VlrComision.Value = (x.VlrGiro.Value.Value * porcComision) / 100);
            }
        }

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        { 
            this.compoCarteraID = String.Empty;
            this.vlrTotal = 0;

            this.liquidaComisionDeta = new List<DTO_ccComisionDeta>();
           
            this.gcDocuments.DataSource = null;
        }

        #endregion

        #region Evento Header

        #endregion

        #region Eventos grilla de Documentos

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
                    this.liquidaComisionDeta[e.RowHandle].Rechazado.Value = false;
            }

            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this.liquidaComisionDeta[e.RowHandle].Aprobado.Value = false;
            }
            #endregion
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                this.liquidaComisionDeta.Where(x => x.Aprobado.Value == true).ToList();
                if (this.liquidaComisionDeta != null && this.liquidaComisionDeta.Count > 0)
                {
                    this.liquidaComisionDeta.ForEach(x => x.FechaAprobacion.Value = this.dtAprobComisiones.DateTime);
                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "TBSave"));
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
                results = _bc.AdministrationModel.LiquidacionComisionesCartera_AprobarRechazar(this.documentID, this.actividadFlujoID, this.liquidaComisionDeta);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                
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

                        if (this.liquidaComisionDeta[i].Aprobado.Value.Value)
                        {
                            bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.AprobacionReintegroClientes, this.actividadDTO.seUsuarioID.Value, obj, false);
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
                    this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "ApproveThread"));
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