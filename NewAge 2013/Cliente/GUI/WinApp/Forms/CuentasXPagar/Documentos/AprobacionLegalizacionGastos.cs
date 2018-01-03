using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
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
    public partial class AprobacionLegalizacionGastos : DocumentAprobBasicForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_LegalizacionAprobacion> _docs = null;
        private string _valueSelect;
        //Nombre de columna segun tarea
        private string colNameSolicita = string.Empty;
        private string colNameRevisa = string.Empty;
        private string colNameSupervisa = string.Empty;
        private string colNameContabiliza = string.Empty;
        private string colNameAprueba = string.Empty;

        #endregion

        #region Funciones Virtuales del DocumentAprobBasicForm

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this._docs = _bc.AdministrationModel.Legalizacion_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);
                List<string> actividades = _bc.AdministrationModel.glActividadPermiso_GetActividadesByUser().OrderBy(x => x).ToList();

                int tareaCurrent = this.cmbUserTareas.SelectedIndex;
                this.cmbUserTareas.Items.Clear();
                this.currentRow = -1;               

                foreach (string act in actividades)
                {
                   DTO_glActividadFlujo flujoID = (DTO_glActividadFlujo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, act, true);
                   if (flujoID != null)
                   {
                       if (flujoID.DocumentoID.Value == AppDocuments.LegalGastosAprob.ToString())
                           this.cmbUserTareas.Items.Add(this.colNameContabiliza);
                       else if (flujoID.DocumentoID.Value == AppDocuments.LegalGastosContabiliza.ToString())
                           this.cmbUserTareas.Items.Add(this.colNameAprueba);
                   }
                }

                base.grpboxHeader.Visible = true;
                this.cmbUserTareas.Visible = true;
                this.lblUserTareas.Visible = true;
                if (this.cmbUserTareas.Items.Count > 0 && tareaCurrent >= 0)
                    this.cmbUserTareas.SelectedIndex = tareaCurrent;
                else
                    this.cmbUserTareas.SelectedIndex = 0;
                this.SourceGrid();       
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizacionGastos.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.LegalGastosContabiliza;
            this.frmModule = ModulesPrefix.cp;
            base.SetInitParameters();

            this.monedaID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjeraID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Width = 40;

                //Recursos Columnas
                this.colNameSolicita = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentoAprobForm + "_Solicitar");
                this.colNameRevisa = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentoAprobForm + "_Revisar");
                this.colNameSupervisa = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentoAprobForm + "_Supervisar");
                this.colNameContabiliza = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentoAprobForm + "_Contabilizar");
                this.colNameAprueba = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentoAprobForm + "_Aprobar");

                //TerceroID
                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 3;
                terceroID.Width = 30;
                terceroID.Visible = true;
                terceroID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(terceroID);

                //NombreTercero
                GridColumn nombreTercero = new GridColumn();
                nombreTercero.FieldName = this.unboundPrefix + "NombreTercero";
                nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NombreTercero");
                nombreTercero.UnboundType = UnboundColumnType.String;
                nombreTercero.VisibleIndex = 4;
                nombreTercero.Width = 30;
                nombreTercero.Visible = true;
                nombreTercero.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombreTercero);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 6;
                fecha.Width = 40;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 7;
                valor.Width = 50;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //VerComprobante
                GridColumn VerComprobante = new GridColumn();
                VerComprobante.FieldName = this.unboundPrefix + "VerComprobante";
                VerComprobante.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VerComprobante");
                VerComprobante.UnboundType = UnboundColumnType.String;
                VerComprobante.Width = 30;
                VerComprobante.VisibleIndex = 8;
                VerComprobante.Visible = false;
                VerComprobante.ColumnEdit = this.editViewReport;
                this.gvDocuments.Columns.Add(VerComprobante);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizaconGastos.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SourceGrid()
        {
            this.currentRow = -1;
            this.gcDocuments.DataSource = null;

            if(this._docs != null)
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
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Se realiza cuando el usuario elige una tarea 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void cmbUserTareas_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cmb = (System.Windows.Forms.ComboBox)sender;

            if (cmb.Text == this.colNameContabiliza)
            {
                this.documentID = AppDocuments.LegalGastosContabiliza;
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Caption = this.colNameContabiliza;
            }
            else if (cmb.Text == this.colNameAprueba)
            {
                this.documentID = AppDocuments.LegalGastosAprob;
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Caption = this.colNameAprueba;
            }

            List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
            if (actividades.Count > 0)
            {
                this.actividadFlujoID = actividades[0];
                this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                this._docs = _bc.AdministrationModel.Legalizacion_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);
            }

            this.SourceGrid();
            this._valueSelect = cmb.Text;
            this.gcDocuments.RefreshDataSource();
            if (cmb.Text == EstadoInterCajaMenor.Contabilizar.ToString())
            {
                this.gvDocuments.Columns[this.unboundPrefix + "VerComprobante"].VisibleIndex = 8;
                this.gvDocuments.Columns[this.unboundPrefix + "VerComprobante"].Visible = true;
            }
            else
            {
                this.gvDocuments.Columns[this.unboundPrefix + "VerComprobante"].VisibleIndex = 8;
                this.gvDocuments.Columns[this.unboundPrefix + "VerComprobante"].Visible = false;
            }
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
        /// Al entrar al link de la grilla para ver el reporte de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                string fileURl;
                //COnsulta los datos
                DTO_Legalizacion leg = _bc.AdministrationModel.Legalizacion_Get(this._docs[this.gvDocuments.FocusedRowHandle].NumeroDoc.Value.Value);
                DateTime periodo = leg.DocCtrl.PeriodoDoc.Value.Value;

                //Crea el comprobante preliminar
                result = _bc.AdministrationModel.Legalizacion_ComprobantePreAdd(this.documentID, leg);
                if (result.Result == ResultValue.NOK)
                {
                    MessageForm form = new MessageForm(result);
                    form.ShowDialog();
                    return;
                }
                this._docs[this.gvDocuments.FocusedRowHandle].NumeroDocCxP.Value = leg.Header.NumeroDocCXP.Value;

                DTO_glDocumentoControl ctrlCxP = this._bc.AdministrationModel.glDocumentoControl_GetByID(leg.Header.NumeroDocCXP.Value.Value);


                //Imprime el reporte
                DTO_TxResult resultReport = this._bc.AdministrationModel.ReportesContabilidad_ComprobantePreliminar(this.documentID, periodo.Year,
                    periodo.Month, ctrlCxP.ComprobanteID.Value, "01", "0", "0", ExportFormatType.pdf);

                //Abre el PDF
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, resultReport.ExtraField);                   
                Process.Start(fileURl);               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionCajaMenor.cs", "editViewReport_Click"));
            }
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
            {
                e.RepositoryItem = this.editSpin;
            }
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
                    if (this._docs.Any(x => x.Aprobado.Value.Value && !x.NumeroDocCxP.Value.HasValue) && this._valueSelect == EstadoInterCajaMenor.Contabilizar.ToString())
                    {
                        MessageBox.Show("Debe generar el comprobante antes de aprobar la contabilización");
                        return;
                    }
                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizacion.cs", "TBSave"));
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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                ////Verifica cual estado es el que envía a Aprobacion
                //  List<DTO_TxResult> results = null;
                //  string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                //  if (this._valueSelect == EstadoInterCajaMenor.Contabilizado.ToString())
                //  {
                      //estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateContabilizado);
                  //}
                  //else if (this._valueSelect == EstadoDocControl.Aprobado.ToString())
                  //    results = _bc.AdministrationModel.Legalizacion_AprobarRechazar(this.documentID, this._valueSelect, this._docs, true); 
                List<DTO_SerializedObject> results = _bc.AdministrationModel.Legalizacion_AprobarRechazar(this.documentID, this.actividadFlujoID, this._docs, true);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizaciongastos.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}