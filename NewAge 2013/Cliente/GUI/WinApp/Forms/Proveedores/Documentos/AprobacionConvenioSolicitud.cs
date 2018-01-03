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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionConvenioSolicitud : DocumentAprobBasicForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_ConvenioAprob> _docs = null;

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

                this._docs = _bc.AdministrationModel.Convenio_GetPendientesByModulo(this.documentID,this.actividadFlujoID,this._bc.AdministrationModel.User,true);
                foreach (var item in this._docs)
                    item.FileUrl = string.Empty;
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
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ConvenioSolicitudAprob;
            this.frmModule = ModulesPrefix.pr;
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

                //ProveedorID
                GridColumn ProveedorID = new GridColumn();
                ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
                ProveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
                ProveedorID.UnboundType = UnboundColumnType.String;
                ProveedorID.VisibleIndex = 4;
                ProveedorID.Width = 50;
                ProveedorID.Visible = true;
                ProveedorID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ProveedorID);

                //ProveedorNombre
                GridColumn provDesc = new GridColumn();
                provDesc.FieldName = this.unboundPrefix + "ProveedorNombre";
                provDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorNombre");
                provDesc.UnboundType = UnboundColumnType.String;
                provDesc.VisibleIndex = 5;
                provDesc.Width = 70;
                provDesc.Visible = true;
                provDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(provDesc);

                //Documento Nro ORden Compra
                GridColumn docNroOrden = new GridColumn();
                docNroOrden.FieldName = this.unboundPrefix + "DocumentoNro";
                docNroOrden.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoNroOrdenCompra");
                docNroOrden.UnboundType = UnboundColumnType.String;
                docNroOrden.VisibleIndex = 6;
                docNroOrden.Width = 40;
                docNroOrden.Visible = true;
                docNroOrden.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(docNroOrden);

                //Moneda
                GridColumn moneda = new GridColumn();
                moneda.FieldName = this.unboundPrefix + "Moneda";
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
                valor.Width = 80;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 60;
                file.VisibleIndex = 9;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);

                #region Detalle  
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 0;
                CodigoBSID.Width = 80;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CodigoBSID);

                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 1;
                inReferenciaID.Width = 80;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(inReferenciaID);

                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 2;
                Descriptivo.Width = 100;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Descriptivo);

                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "CantidadDoc1";
                Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadDoc1");
                Cantidad.UnboundType = UnboundColumnType.Decimal;
                Cantidad.VisibleIndex = 6;
                Cantidad.Width = 50;
                Cantidad.Visible = true;
                Cantidad.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Cantidad);

                GridColumn ValorUni = new GridColumn();
                ValorUni.FieldName = this.unboundPrefix + "ValorUni";
                ValorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                ValorUni.UnboundType = UnboundColumnType.Decimal;
                ValorUni.VisibleIndex = 8;
                ValorUni.Width = 80;
                ValorUni.Visible = true;
                ValorUni.ColumnEdit = this.editSpin;
                ValorUni.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorUni);

                GridColumn ValorTotML = new GridColumn();
                ValorTotML.FieldName = this.unboundPrefix + "ValorTotML";
                ValorTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                ValorTotML.UnboundType = UnboundColumnType.Decimal;
                ValorTotML.VisibleIndex = 9;
                ValorTotML.Width = 80;
                ValorTotML.Visible = true;
                ValorTotML.ColumnEdit = this.editSpin;
                ValorTotML.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorTotML);

                GridColumn IvaTotML = new GridColumn();
                IvaTotML.FieldName = this.unboundPrefix + "IvaTotML";
                IvaTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IvaTotML");
                IvaTotML.UnboundType = UnboundColumnType.Decimal;
                IvaTotML.VisibleIndex = 10;
                IvaTotML.Width = 80;
                IvaTotML.Visible = true;
                IvaTotML.ColumnEdit = this.editSpin;
                IvaTotML.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(IvaTotML);

                GridColumn ValorTotME = new GridColumn();
                ValorTotME.FieldName = this.unboundPrefix + "ValorTotME";
                ValorTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotME");
                ValorTotME.UnboundType = UnboundColumnType.Decimal;
                ValorTotME.VisibleIndex = 9;
                ValorTotME.Width = 80;
                ValorTotME.Visible = true;
                ValorTotME.ColumnEdit = this.editSpin;
                ValorTotME.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorTotME);

                GridColumn IvaTotME = new GridColumn();
                IvaTotME.FieldName = this.unboundPrefix + "IvaTotME";
                IvaTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IvaTotME");
                IvaTotME.UnboundType = UnboundColumnType.Decimal;
                IvaTotME.VisibleIndex = 10;
                IvaTotME.Width = 80;
                IvaTotME.Visible = true;
                IvaTotME.ColumnEdit = this.editSpin;
                IvaTotME.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(IvaTotME);

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "AprobacionTarjetaPago.cs-AddGridCols"));
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

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvDocuments.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._docs[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionConvenioSolicitud.cs", "editLink_Click"));
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
                e.RepositoryItem = this.editSpin;
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
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

                List<DTO_SerializedObject> results = this._bc.AdministrationModel.Convenio_AprobarRechazar(this.documentID, this.actividadFlujoID, this._bc.AdministrationModel.User, this._docs, true);
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
            catch (Exception e)
            {
                MessageBox.Show("Err: " + e.Message);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}