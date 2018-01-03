using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using NewAge.DTO.Resultados;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Aprobacion de causaciones
    /// </summary>
    public partial class AprobDeterioroReval : DocumentAprobBasicForm
    {
        #region Variables formulario
        
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_inDeterioroAprob> _docs = null;
        private bool firstTime = true;
       
        #endregion

        #region Funciones Virtuales del DocumentAprobBasicForm

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this._docs = this._bc.AdministrationModel.inMovimientoDocu_GetPendientesByModulo(ModulesPrefix.@in, this.actividadFlujoID);

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this._docs.Count > 0)
                {
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobDeterioroReval.cs", "LoadDocuments: " + ex.Message));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.DeterioroInvAprob;
            this.frmModule = ModulesPrefix.@in;

            base.SetInitParameters();           
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize() 
        {
            base.grpboxHeader.Visible = true;
            this.cmbUserTareas.Visible = true;
            this.lblUserTareas.Visible = true;
            this.lblUserTareas.Text = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.DeterioroInv + "_lblTipoDoc");
            this.cmbUserTareas.Items.Add(this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DeterioroInv));
            this.cmbUserTareas.Items.Add(this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_RevalorizacionInv));
            this.cmbUserTareas.SelectedIndex = 0;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                //PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 2;
                PrefDoc.Width = 30;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(PrefDoc);

                //Periodo
                GridColumn per = new GridColumn();
                per.FieldName = this.unboundPrefix + "PeriodoID";
                per.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Periodo");
                per.UnboundType = UnboundColumnType.DateTime;
                per.VisibleIndex = 3;
                per.Width = 40;
                per.Visible = true;
                per.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(per);

                //EstadoInv
                GridColumn EstadoInv = new GridColumn();
                EstadoInv.FieldName = this.unboundPrefix + "EstadoInv";
                EstadoInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoInv");
                EstadoInv.UnboundType = UnboundColumnType.Decimal;
                EstadoInv.VisibleIndex = 4;
                EstadoInv.Width = 40;
                EstadoInv.Visible = true;
                EstadoInv.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(EstadoInv);

                //ValorTotalML
                GridColumn ValorTotalML = new GridColumn();
                ValorTotalML.FieldName = this.unboundPrefix + "ValorTotalML";
                ValorTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalML");
                ValorTotalML.UnboundType = UnboundColumnType.Decimal;
                ValorTotalML.VisibleIndex = 5;
                ValorTotalML.Width = 40;
                ValorTotalML.Visible = true;
                ValorTotalML.OptionsColumn.AllowEdit = false;
                ValorTotalML.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(ValorTotalML);

                //ValorTotalME
                GridColumn ValorTotalME = new GridColumn();
                ValorTotalME.FieldName = this.unboundPrefix + "ValorTotalME";
                ValorTotalME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalME");
                ValorTotalME.UnboundType = UnboundColumnType.Decimal;
                ValorTotalME.VisibleIndex = 6;
                ValorTotalME.Width = 40;
                ValorTotalME.Visible = true;
                ValorTotalME.ColumnEdit = this.editSpin;
                ValorTotalME.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ValorTotalME);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FileUrl");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 30;
                file.VisibleIndex = 7;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);

                #region Detalle
                //inReferenciaID
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 1;
                inReferenciaID.Width = 70;
                inReferenciaID.Visible = true;
                this.gvDetalle.Columns.Add(inReferenciaID);

                //DescripTExt
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "DescripTExt";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 2;
                DescripTExt.Width = 150;
                DescripTExt.Visible = true;
                this.gvDetalle.Columns.Add(DescripTExt);

                //Valor1LOC
                GridColumn Valor1LOC = new GridColumn();
                Valor1LOC.FieldName = this.unboundPrefix + "Valor1LOC";
                Valor1LOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1LOC");
                Valor1LOC.UnboundType = UnboundColumnType.Decimal;
                Valor1LOC.VisibleIndex = 2;
                Valor1LOC.Width = 80;
                Valor1LOC.Visible = true;
                Valor1LOC.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(Valor1LOC);

                //Valor1EXT
                GridColumn Valor1EXT = new GridColumn();
                Valor1EXT.FieldName = this.unboundPrefix + "Valor1EXT";
                Valor1EXT.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1EXT");
                Valor1EXT.UnboundType = UnboundColumnType.Decimal;
                Valor1EXT.VisibleIndex = 3;
                Valor1EXT.Width = 80;
                Valor1EXT.Visible = true;
                Valor1EXT.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(Valor1EXT); 
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobDeterioroReval", "AddDocumentCols"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this._docs[i].Aprobado.Value = true;
                    this._docs[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this._docs[i].Aprobado.Value = false;
                    this._docs[i].Rechazado.Value = false;
                }
            }
            this.gcDocuments.RefreshDataSource();
        }

        /// <summary>
        /// Se realiza cuando el usuario elige una tarea 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void cmbUserTareas_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;

            if (!this.firstTime)
            {
                if (cmb.SelectedIndex == 0)
                    this.documentID = AppDocuments.DeterioroInvAprob;
                else
                    this.documentID = AppDocuments.RevalorizacionInvAprob;

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    this.actividadFlujoID = actividades[0];
                    this.LoadDocuments();
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion
                this.gcDocuments.RefreshDataSource();
            }
            else
                this.firstTime = false;
        }

        #endregion

        #region Eventos Grilla

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
                    this._docs[e.RowHandle].Rechazado.Value = false;
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this._docs[e.RowHandle].Aprobado.Value = false;
            }
            #endregion

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            base.riPopup_QueryPopUp(sender, e);

            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
            {
                this.richEditControl.ReadOnly = true;
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
            }
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
                this.richEditControl.ReadOnly = false;
            else
                base.riPopup_QueryResultValue(sender, e);
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
                if (this._docs != null && this._docs.Count != 0)
                {
                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobDeterioroReval", "TBSave"));
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
                #region Aprueba los registros
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_inDeterioroAprob> approveTmp = new List<DTO_inDeterioroAprob>();
                foreach (var itemApprove in this._docs)
                    approveTmp.Add(itemApprove);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.Deterioro_AprobarRechazar(this.documentID, approveTmp, this.actividadFlujoID, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion

                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
 
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object obj in results)
                {
                    #region Funciones de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion

                    if (this._docs[i].Aprobado.Value.Value || this._docs[i].Rechazado.Value.Value)
                    {
                        MailType mType = this._docs[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                        bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                        }
                    }
                    i++;
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobDeterioroReval.cs", "ApproveThread: " + ex.Message));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}