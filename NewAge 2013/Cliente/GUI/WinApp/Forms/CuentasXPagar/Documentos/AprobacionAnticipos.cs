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
using NewAge.DTO.UDT;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionAnticipos : DocumentAprobBasicForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_AnticipoAprobacion> _docs = null;

        #endregion

        #region Funciones Virtuales del DocumentAprobBasicForm

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.monedaID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this.monedaExtranjeraID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;

                this._docs = _bc.AdministrationModel.cpAnticipos_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this._docs.Count > 0)
                {
                    //this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this.gcDocuments.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAnticipos.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AnticiposAprob;
            this.frmModule = ModulesPrefix.cp;
            base.SetInitParameters();
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
                noAprob.Visible = true;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Documento Tercero
                GridColumn DocTer = new GridColumn();
                DocTer.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocTer.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoTercero");
                DocTer.UnboundType = UnboundColumnType.String;
                DocTer.VisibleIndex = 2;
                DocTer.Width = 30;
                DocTer.Visible = true;
                DocTer.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(DocTer);

                //Periodo
                GridColumn per = new GridColumn();
                per.FieldName = this.unboundPrefix + "PeriodoID";
                per.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PeriodoID");
                per.UnboundType = UnboundColumnType.DateTime;
                per.VisibleIndex = 3;
                per.Width = 40;
                per.Visible = true;
                per.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(per);

                //TerceroID
                GridColumn ter = new GridColumn();
                ter.FieldName = this.unboundPrefix + "TerceroID";
                ter.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                ter.UnboundType = UnboundColumnType.String;
                ter.VisibleIndex = 4;
                ter.Width = 30;
                ter.Visible = true;
                ter.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ter);

                //TerceroDesc
                GridColumn terDesc = new GridColumn();
                terDesc.FieldName = this.unboundPrefix + "DescriptivoTercero";
                terDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_terDesc");
                terDesc.UnboundType = UnboundColumnType.String;
                terDesc.VisibleIndex = 5;
                terDesc.Width = 40;
                terDesc.Visible = true;
                terDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(terDesc);


                //AnticipoTipoID
                GridColumn AnticipoTipoID = new GridColumn();
                AnticipoTipoID.FieldName = this.unboundPrefix + "AnticipoTipoID";
                AnticipoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AnticipoTipoID");
                AnticipoTipoID.UnboundType = UnboundColumnType.String;
                AnticipoTipoID.VisibleIndex = 6;
                AnticipoTipoID.Width = 40;
                AnticipoTipoID.Visible = true;
                AnticipoTipoID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(AnticipoTipoID);

                //Moneda
                GridColumn moneda = new GridColumn();
                moneda.FieldName = this.unboundPrefix + "MonedaID";
                moneda.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                moneda.UnboundType = UnboundColumnType.String;
                moneda.VisibleIndex = 7;
                moneda.Width = 40;
                moneda.Visible = true;
                moneda.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(moneda);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 8;
                valor.Width = 50;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FileUrl");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 30;
                file.VisibleIndex = 9;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);


                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 10;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAnticipos.cs", "AddDocumentCols"));
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
            if (fieldName == "Valor")
                e.RepositoryItem = this.editSpin;
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
                #region Aprueba los registros
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.cpAnticipos_AprobarRechazar(this.documentID, this.actividadFlujoID, this._docs, true);
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
                foreach (DTO_AnticipoAprobacion ant in this._docs)
                {
                    if (ant.Aprobado.Value.Value)
                    {
                        DTO_cpAnticipoTipo tipoAnticipo = (DTO_cpAnticipoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, false, ant.AnticipoTipoID.Value, true);
                        string reportName = string.Empty;
                        if (tipoAnticipo.GastosViajeInd.Value.HasValue && !tipoAnticipo.GastosViajeInd.Value.Value)
                            reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipo(ant.NumeroDoc.Value.Value, true, ExportFormatType.pdf);
                        else
                            reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipoViaje(ant.NumeroDoc.Value.Value, true, ExportFormatType.pdf);

                        //if (reportName == string.Empty)
                        //    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                        //else
                        //{
                        //    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, ant.NumeroDoc.Value.Value, null, reportName);
                        //    Process.Start(fileURl);
                        //} 
                    }
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