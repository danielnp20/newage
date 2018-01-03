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
    public partial class AprobacionLegalizacionTarjetas : DocumentAprobBasicForm
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
                this.currentRow = -1;

                this.SourceGrid();            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizacionTarjetas.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.LegalizacionTarjetasAprob;
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

                //TarjetaCreditoID
                GridColumn tarjeta = new GridColumn();
                tarjeta.FieldName = this.unboundPrefix + "TarjetaCreditoID";
                tarjeta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TarjetaCreditoID");
                tarjeta.UnboundType = UnboundColumnType.String;
                tarjeta.VisibleIndex = 3;
                tarjeta.Width = 30;
                tarjeta.Visible = true;
                tarjeta.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(tarjeta);

                //TarjetaDesc
                GridColumn tarjetaDesc = new GridColumn();
                tarjetaDesc.FieldName = this.unboundPrefix + "Descriptivo";
                tarjetaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                tarjetaDesc.UnboundType = UnboundColumnType.String;
                tarjetaDesc.VisibleIndex = 4;
                tarjetaDesc.Width = 30;
                tarjetaDesc.Visible = true;
                tarjetaDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(tarjetaDesc);

                //TerceroID
                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 5;
                terceroID.Width = 30;
                terceroID.Visible = true;
                terceroID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(terceroID);

                //NombreTercero
                GridColumn nombreTercero = new GridColumn();
                nombreTercero.FieldName = this.unboundPrefix + "NombreTercero";
                nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NombreTercero");
                nombreTercero.UnboundType = UnboundColumnType.String;
                nombreTercero.VisibleIndex = 6;
                nombreTercero.Width = 30;
                nombreTercero.Visible = true;
                nombreTercero.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombreTercero);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 7;
                fecha.Width = 40;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizaconGastos.cs", "AddDocumentCols"));
            }
        }

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
            {
                e.RepositoryItem = this.editSpin;
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
    
            if (cmb.Text == EstadoInterCajaMenor.Contabilizar.ToString())
            {
                this._docs = _bc.AdministrationModel.Legalizacion_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Caption = this.colNameContabiliza;
            }
            else if (cmb.Text ==  EstadoDocControl.Aprobado.ToString())
            {
                this._docs = _bc.AdministrationModel.Legalizacion_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Caption = this.colNameAprueba;
            }
            this.SourceGrid();
            this._valueSelect = cmb.Text;
            this.gcDocuments.RefreshDataSource();
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionLegalizacionTarjetas.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}