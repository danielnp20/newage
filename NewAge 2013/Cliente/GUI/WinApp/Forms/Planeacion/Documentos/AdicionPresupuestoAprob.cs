using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using SentenceTransformer;
using System.Diagnostics;
using DevExpress.XtraEditors.Controls;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class AdicionPresupuestoAprob : DocumentAprobBasicForm
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private DTO_PresupuestoAprob presupuesto;
        private List<DTO_PresupuestoAprob> _docs = new List<DTO_PresupuestoAprob>();

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AdicionPresupuestoAprob;
            this.frmModule = ModulesPrefix.pl;
            base.SetInitParameters();
        }

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this._docs = _bc.AdministrationModel.Presupuesto_GetNuevosForAprob(this.documentID, this.actividadFlujoID);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuestosAprob", "LoadData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
              try
            {
                #region Documentos

                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_Aprobado");
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
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 2;
                PrefDoc.Width = 100;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(PrefDoc);

                //Periodo
                GridColumn Periodo = new GridColumn();
                Periodo.FieldName = this.unboundPrefix + "PeriodoDoc";
                Periodo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_PeriodoDoc");
                Periodo.UnboundType = UnboundColumnType.DateTime;
                Periodo.VisibleIndex = 3;
                Periodo.Width = 100;
                Periodo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Periodo);
                
                //ProyectoID
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 4;
                proyecto.Width = 100;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(proyecto);

                //ProyectoDesc
                GridColumn proyectoDesc = new GridColumn();
                proyectoDesc.FieldName = this.unboundPrefix + "ProyectoDesc";
                proyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_ProyectoDesc");
                proyectoDesc.UnboundType = UnboundColumnType.String;
                proyectoDesc.VisibleIndex = 5;
                proyectoDesc.Width = 200;
                proyectoDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(proyectoDesc);

                //TotalML
                GridColumn TotalML = new GridColumn();
                TotalML.FieldName = this.unboundPrefix + "TotalML";
                TotalML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_TotalML");
                TotalML.UnboundType = UnboundColumnType.Decimal;
                TotalML.VisibleIndex = 6;
                TotalML.Width = 150;
                TotalML.Visible = true;
                TotalML.OptionsColumn.AllowEdit = false;
                TotalML.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(TotalML);

                //TotalME
                if (_bc.AdministrationModel.MultiMoneda)
                {
                    GridColumn TotalME = new GridColumn();
                    TotalME.FieldName = this.unboundPrefix + "TotalME";
                    TotalME.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_TotalME");
                    TotalME.UnboundType = UnboundColumnType.Decimal;
                    TotalME.VisibleIndex = 7;
                    TotalME.Width = 150;
                    TotalME.Visible = true;
                    TotalME.ColumnEdit = this.editSpin;
                    TotalME.OptionsColumn.AllowEdit = false;
                    this.gvDocuments.Columns.Add(TotalME);
                }
                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 8;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);
                #endregion
                #region Detalles
                //CentroCostoID
                GridColumn ctoCostoID = new GridColumn();
                ctoCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                ctoCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_CentroCostoID");
                ctoCostoID.UnboundType = UnboundColumnType.String;
                ctoCostoID.VisibleIndex = 0;
                ctoCostoID.Width = 100;
                ctoCostoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ctoCostoID);

                //CentroCosto Descripcion
                GridColumn ctoCostoDesc = new GridColumn();
                ctoCostoDesc.FieldName = this.unboundPrefix + "CentroCostoDesc";
                ctoCostoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_CentroCostoDesc");
                ctoCostoDesc.UnboundType = UnboundColumnType.String;
                ctoCostoDesc.VisibleIndex = 1;
                ctoCostoDesc.Width = 200;
                ctoCostoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ctoCostoDesc);

                //LineaPresup
                GridColumn LineaPresup = new GridColumn();
                LineaPresup.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                LineaPresup.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_LineaPresupuestoID");
                LineaPresup.UnboundType = UnboundColumnType.String;
                LineaPresup.VisibleIndex = 2;
                LineaPresup.Width = 100;
                LineaPresup.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(LineaPresup);

                //LineaPresDesc
                GridColumn LineaPresDesc = new GridColumn();
                LineaPresDesc.FieldName = this.unboundPrefix + "LineaPresDesc";
                LineaPresDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_LineaPresDesc");
                LineaPresDesc.UnboundType = UnboundColumnType.String;
                LineaPresDesc.VisibleIndex = 3;
                LineaPresDesc.Width = 200;
                LineaPresDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(LineaPresDesc);

                //ValorML
                GridColumn valorDetML = new GridColumn();
                valorDetML.FieldName = this.unboundPrefix + "ValorML";
                valorDetML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_ValorML");
                valorDetML.UnboundType = UnboundColumnType.Decimal;
                valorDetML.VisibleIndex = 4;
                valorDetML.Width = 150;
                valorDetML.Visible = true;
                valorDetML.ColumnEdit = this.editSpin;
                valorDetML.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(valorDetML);

                //ValorME
                if (this._bc.AdministrationModel.MultiMoneda)
                {
                    GridColumn valorDetME = new GridColumn();
                    valorDetME.FieldName = this.unboundPrefix + "ValorME";
                    valorDetME.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuestoAprob + "_ValorME");
                    valorDetME.UnboundType = UnboundColumnType.Decimal;
                    valorDetME.VisibleIndex = 5;
                    valorDetME.Width = 150;
                    valorDetME.Visible = true;
                    valorDetME.ColumnEdit = this.editSpin;
                    valorDetME.OptionsColumn.AllowEdit = false;
                    this.gvDetalle.Columns.Add(valorDetME);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuestosAprob", "AddCols_Documentos"));
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
        #endregion

        #region Eventos editor de texto

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuestosAprob", "TBSave"));
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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.Presupuesto_AprobarRechazar(this.documentID, this.actividadFlujoID,this._docs);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAnticipos.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion
    }
}
