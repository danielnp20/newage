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
using DevExpress.XtraEditors.Controls;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EndosoLibranzasVendidas : DocumentAprobBasicForm
    {

        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int docRecursos;

        //DTO's
        private List<DTO_ccVentaDeta> creditosVendidos = new List<DTO_ccVentaDeta>();
        private DTO_VentaCartera ventaCartera = new DTO_VentaCartera();

        //Variables privadas
        protected bool isChecked = false;

        #endregion

        public EndosoLibranzasVendidas()
            : base() { }

        public EndosoLibranzasVendidas(string mod)
            : base(mod) { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.EndosoLibranzasVendidas;
                this.frmModule = ModulesPrefix.cc;
                base.SetInitParameters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EndosoLibranzasVendidas.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Funcion que pinta la informacion en la grilla
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.ventaCartera = this._bc.AdministrationModel.VentaCartera_GetByActividadFlujo(this.actividadFlujoID);
                this.creditosVendidos = this.ventaCartera.VentaDeta;
                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this.creditosVendidos.Count > 0)
                {
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this.creditosVendidos;
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    this.gcDocuments.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EndosoLibranzasVendidas.cs", "LoadDocuments"));
            }

        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();
                this.gvDocuments.Columns.Remove(this.gvDocuments.Columns[this.unboundPrefix + "Observacion"]);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 2;
                libranza.Width = 70;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);

                //Cliente Id
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 3;
                clienteID.Width = 90;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 4;
                nombCliente.Width = 150;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombCliente);

                //CompradorFinal
                GridColumn compradorFinal = new GridColumn();
                compradorFinal.FieldName = this.unboundPrefix + "CompradorFinal";
                compradorFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorFinal");
                compradorFinal.UnboundType = UnboundColumnType.String;
                compradorFinal.VisibleIndex = 5;
                compradorFinal.Width = 80;
                compradorFinal.Visible = true;
                compradorFinal.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(compradorFinal);

                //VlrCuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrCuota.UnboundType = UnboundColumnType.Decimal;
                vlrCuota.VisibleIndex = 6;
                vlrCuota.Width = 150;
                vlrCuota.OptionsColumn.AllowEdit = false;
                vlrCuota.ColumnEdit = editSpin;
                this.gvDocuments.Columns.Add(vlrCuota);

                //VlrVenta
                GridColumn vlrVenta = new GridColumn();
                vlrVenta.FieldName = this.unboundPrefix + "VlrVenta";
                vlrVenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrVenta");
                vlrVenta.UnboundType = UnboundColumnType.Decimal;
                vlrVenta.VisibleIndex = 7;
                vlrVenta.Width = 150;
                vlrVenta.OptionsColumn.AllowEdit = false;
                vlrVenta.ColumnEdit = editSpin;
                this.gvDocuments.Columns.Add(vlrVenta);

                //VlrLibranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Decimal;
                vlrLibranza.VisibleIndex = 8;
                vlrLibranza.Width = 150;
                vlrLibranza.OptionsColumn.AllowEdit = false;
                vlrLibranza.ColumnEdit = editSpin;
                this.gvDocuments.Columns.Add(vlrLibranza);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 9;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "EndosoLibranzasVendidas.cs-AddGridCols"));
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
                    FormProvider.Master.itemPrint.Visible = false;
                    FormProvider.Master.itemNew.Visible = false;
                    FormProvider.Master.itemDelete.Visible = false;
                    FormProvider.Master.itemSendtoAppr.Visible = false;
                    FormProvider.Master.itemCopy.Visible = false;
                    FormProvider.Master.itemPaste.Visible = false;
                    FormProvider.Master.itemImport.Visible = false;
                    FormProvider.Master.itemExport.Visible = false;
                    FormProvider.Master.itemRevert.Visible = false;
                    FormProvider.Master.itemGenerateTemplate.Visible = false;
                    FormProvider.Master.itemFilter.Visible = false;
                    FormProvider.Master.itemFilterDef.Visible = false;
                    FormProvider.Master.tbBreak1.Visible = false;
                    FormProvider.Master.tbBreak2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EndosoLibranzasVendidas.cs", "Form_Enter"));
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
                    this.creditosVendidos[i].Aprobado.Value = true;
                    this.creditosVendidos[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this.creditosVendidos[i].Aprobado.Value = false;
                    this.creditosVendidos[i].Rechazado.Value = false;
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
                    this.creditosVendidos[e.RowHandle].Rechazado.Value = false;
            }

            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this.creditosVendidos[e.RowHandle].Aprobado.Value = false;
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

        #region Eventos Barra Herramientas

        /// <summary>
        /// Funcion que refresca el formulario
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.LoadDocuments();
            }
            catch (Exception ex)
            {
                throw ex;
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
                if (this.creditosVendidos != null && this.creditosVendidos.Count != 0)
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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = this._bc.AdministrationModel.EndosoLibranzasVendidas_AprobarRechazar(documentID, this.actividadFlujoID, this.creditosVendidos);
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
                    DTO_ccVentaDeta ventaAprobacion = this.creditosVendidos[i];
                    if (ventaAprobacion.Aprobado.Value.Value || ventaAprobacion.Rechazado.Value.Value)
                    {

                        if (result.GetType() == typeof(DTO_TxResult))
                            resultsNOK.Add((DTO_TxResult)result);
                        else
                        {
                            #region Envia el correo
                            if (ventaAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, ventaAprobacion.NumDocCredito.Value, ventaAprobacion.ClienteID.Value,
                                  ventaAprobacion.Observacion.Value);
                            }
                            else if (ventaAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, ventaAprobacion.Observacion.Value, ventaAprobacion.NumDocCredito.Value,
                                    ventaAprobacion.ClienteID.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EndosoLibranzasVendidas.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        
        #endregion       

    }
        
}
