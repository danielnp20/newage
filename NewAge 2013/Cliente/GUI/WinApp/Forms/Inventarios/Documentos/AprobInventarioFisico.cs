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
    public partial class AprobInventarioFisico : DocumentAprobBasicForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_InvFisicoAprobacion> _docs = null;
        #endregion

        #region Funciones Virtuales del DocumentAprobBasicForm

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                //this.currentDoc = null;
                this._docs = _bc.AdministrationModel.InventarioFisico_GetPendientesByModulo(ModulesPrefix.@in, this.actividadFlujoID);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInPosteoComprobantes.cs", "LoadDocuments: " + ex.Message));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.InventarioFisicoAprob;
            this.frmModule = ModulesPrefix.@in;

            base.SetInitParameters();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                //BodegaID
                GridColumn BodegaID = new GridColumn();
                BodegaID.FieldName = this.unboundPrefix + "BodegaID";
                BodegaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BodegaID");
                BodegaID.UnboundType = UnboundColumnType.String;
                BodegaID.VisibleIndex = 2;
                BodegaID.Width = 30;
                BodegaID.Visible = true;
                BodegaID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(BodegaID);

                //Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "BodegaDesc";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BodegaDesc");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 3;
                Descriptivo.Width = 30;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Descriptivo);

                //Periodo
                GridColumn per = new GridColumn();
                per.FieldName = this.unboundPrefix + "PeriodoID";
                per.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Periodo");
                per.UnboundType = UnboundColumnType.DateTime;
                per.VisibleIndex = 4;
                per.Width = 40;
                per.Visible = true;
                per.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(per);

                //CantAjusteEntrada
                GridColumn CantAjusteEntrada = new GridColumn();
                CantAjusteEntrada.FieldName = this.unboundPrefix + "CantAjusteEntrada";
                CantAjusteEntrada.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantAjusteEntrada");
                CantAjusteEntrada.UnboundType = UnboundColumnType.Decimal;
                CantAjusteEntrada.VisibleIndex = 5;
                CantAjusteEntrada.Width = 40;
                CantAjusteEntrada.Visible = true;
                CantAjusteEntrada.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(CantAjusteEntrada);

                //ValorAjusteEntrada
                GridColumn ValorAjusteEntrada = new GridColumn();
                ValorAjusteEntrada.FieldName = this.unboundPrefix + "ValorAjusteEntrada";
                ValorAjusteEntrada.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjusteEntrada");
                ValorAjusteEntrada.UnboundType = UnboundColumnType.Decimal;
                ValorAjusteEntrada.VisibleIndex = 6;
                ValorAjusteEntrada.Width = 40;
                ValorAjusteEntrada.Visible = true;
                ValorAjusteEntrada.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ValorAjusteEntrada);

                //CantAjusteSalida
                GridColumn CantAjusteSalida = new GridColumn();
                CantAjusteSalida.FieldName = this.unboundPrefix + "CantAjusteSalida";
                CantAjusteSalida.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantAjusteSalida");
                CantAjusteSalida.UnboundType = UnboundColumnType.Decimal;
                CantAjusteSalida.VisibleIndex = 7;
                CantAjusteSalida.Width = 40;
                CantAjusteSalida.Visible = true;
                CantAjusteSalida.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(CantAjusteSalida);

                //ValorAjusteSalida
                GridColumn ValorAjusteSalida = new GridColumn();
                ValorAjusteSalida.FieldName = this.unboundPrefix + "ValorAjusteSalida";
                ValorAjusteSalida.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAjusteSalida");
                ValorAjusteSalida.UnboundType = UnboundColumnType.Decimal;
                ValorAjusteSalida.VisibleIndex = 8;
                ValorAjusteSalida.Width = 40;
                ValorAjusteSalida.Visible = true;
                ValorAjusteSalida.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ValorAjusteSalida);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FileUrl");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 30;
                file.VisibleIndex = 9;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInventarioFisico", "AddDocumentCols"));
            }
        }

        #endregion

        #region Eventos Controles

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

        #endregion

        #region Eventos grillas

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
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            base.gvDocuments_CustomRowCellEdit(sender, e);

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ValorAjusteSalida" || fieldName == "ValorAjusteEntrada")
            {
                e.RepositoryItem = this.editSpin;
            }
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
                throw ex;
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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_InvFisicoAprobacion> approveTmp = new List<DTO_InvFisicoAprobacion>();
                foreach (var itemApprove in this._docs)
                    approveTmp.Add(itemApprove);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.InventarioFisico_AprobarRechazar(this.documentID, approveTmp, this.actividadFlujoID, true, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInventarioFisico.cs", "ApproveThread: " + ex.Message));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}