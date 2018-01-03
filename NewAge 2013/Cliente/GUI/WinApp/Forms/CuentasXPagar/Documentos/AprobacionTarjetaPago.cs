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
    public partial class AprobacionTarjetaPago : DocumentAprobBasicForm
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

                this._docs = _bc.AdministrationModel.cpTarjetaDocu_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);

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
                throw ex;
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.PagoTarjetaCreditoAprob;
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
                base.AddDocumentCols();

                //tarjetaCredito
                GridColumn tarjetaCredito = new GridColumn();
                tarjetaCredito.FieldName = this.unboundPrefix + "TarjetaCreditoID";
                tarjetaCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TarjetaCreditoID");
                tarjetaCredito.UnboundType = UnboundColumnType.Decimal;
                tarjetaCredito.VisibleIndex = 3;
                tarjetaCredito.Width = 50;
                tarjetaCredito.Visible = true;
                tarjetaCredito.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(tarjetaCredito);

                //Periodo
                GridColumn per = new GridColumn();
                per.FieldName = this.unboundPrefix + "PeriodoID";
                per.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PeriodoID");
                per.UnboundType = UnboundColumnType.DateTime;
                per.VisibleIndex = 4;
                per.Width = 40;
                per.Visible = true;
                per.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(per);

                //TerceroID
                GridColumn ter = new GridColumn();
                ter.FieldName = this.unboundPrefix + "TerceroID";
                ter.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                ter.UnboundType = UnboundColumnType.String;
                ter.VisibleIndex = 5;
                ter.Width = 30;
                ter.Visible = true;
                ter.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ter);

                //TerceroDesc
                GridColumn terDesc = new GridColumn();
                terDesc.FieldName = this.unboundPrefix + "DescriptivoTercero";
                terDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_terDesc");
                terDesc.UnboundType = UnboundColumnType.String;
                terDesc.VisibleIndex = 6;
                terDesc.Width = 40;
                terDesc.Visible = true;
                terDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(terDesc);

                //Documento Tercero
                GridColumn DocTer = new GridColumn();
                DocTer.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocTer.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoTercero");
                DocTer.UnboundType = UnboundColumnType.String;
                DocTer.VisibleIndex = 7;
                DocTer.Width = 30;
                DocTer.Visible = true;
                DocTer.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(DocTer);

                //Moneda
                GridColumn moneda = new GridColumn();
                moneda.FieldName = this.unboundPrefix + "MonedaID";
                moneda.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                moneda.UnboundType = UnboundColumnType.String;
                moneda.VisibleIndex = 8;
                moneda.Width = 40;
                moneda.Visible = true;
                moneda.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(moneda);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 9;
                valor.Width = 50;
                valor.Visible = true;
                valor.ColumnEdit = this.editSpin;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FileUrl");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 30;
                file.VisibleIndex = 10;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);

                //cargoEspecial
                GridColumn cargoEspecial = new GridColumn();
                cargoEspecial.FieldName = this.unboundPrefix + "CargoEspecialID";
                cargoEspecial.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CargoEspecialID");
                cargoEspecial.UnboundType = UnboundColumnType.String;
                cargoEspecial.Width = 100;
                cargoEspecial.VisibleIndex = 1;
                cargoEspecial.Visible = true;
                this.gvDetalle.Columns.Add(cargoEspecial);

                //Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.Width = 180;
                Descriptivo.VisibleIndex = 2;
                Descriptivo.Visible = true;
                this.gvDetalle.Columns.Add(Descriptivo);

                //Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.String;
                Valor.Width = 90;
                Valor.VisibleIndex = 3;
                Valor.Visible = true;
                Valor.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(Valor);

                this.gvDetalle.OptionsView.ColumnAutoWidth =  false;
                this.gvDetalle.OptionsBehavior.Editable = false;

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

                List<DTO_SerializedObject> results = _bc.AdministrationModel.cpTarjetaDocu_AprobarRechazar(this.documentID, this.actividadFlujoID, this._docs, true);
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