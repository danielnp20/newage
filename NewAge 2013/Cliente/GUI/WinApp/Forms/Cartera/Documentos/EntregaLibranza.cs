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
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EntregaLibranza : DocumentAprobForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccSolicitudAnexo> anexos = new List<DTO_ccSolicitudAnexo>();
        private List<DTO_ccSolicitudAnexo> anexosAll = new List<DTO_ccSolicitudAnexo>();
        private List<DTO_ccSolicitudAnexo> _solicitudAnexos = new List<DTO_ccSolicitudAnexo>();
        private string _pagaduriaID = "";

        #endregion

        public EntregaLibranza()
            : base()
        {
            //InitializeComponent();
        }

        public EntregaLibranza(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales del DocumentAprobForm

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.EntregaLibranza;
            this.frmModule = ModulesPrefix.cc;

            base.SetInitParameters();
            this.anexosAll = new List<DTO_ccSolicitudAnexo>();

            //Modifica el tamaño de las Grillas
            this.TbLyPanel.RowStyles[2].Height = 0;
            this.TbLyPanel.RowStyles[4].Height = 0;
        }

        /// <summary>
        /// Asigna la lista de columnas de los Documentos
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                //Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this.unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 1;
                Libranza.Width = 20;
                Libranza.Visible = true;
                Libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Libranza);

                //ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this.unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.Integer;
                ClienteID.VisibleIndex = 1;
                ClienteID.Width = 20;
                ClienteID.Visible = true;
                ClienteID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ClienteID);

                //VlrPrestamo
                GridColumn VlrPrestamo = new GridColumn();
                VlrPrestamo.FieldName = this.unboundPrefix + "VlrPrestamo";
                VlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrestamo");
                VlrPrestamo.UnboundType = UnboundColumnType.Decimal;
                VlrPrestamo.VisibleIndex = 1;
                VlrPrestamo.Width = 30;
                VlrPrestamo.Visible = true;
                VlrPrestamo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrPrestamo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrPrestamo);

                //VlrCapital
                GridColumn VlrCapital = new GridColumn();
                VlrCapital.FieldName = this.unboundPrefix + "VlrGiro";
                VlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGiro");
                VlrCapital.UnboundType = UnboundColumnType.Decimal;
                VlrCapital.VisibleIndex = 1;
                VlrCapital.Width = 30;
                VlrCapital.Visible = true;
                VlrCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrCapital.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrCapital);

                //PagaduriaID
                GridColumn pagaduriaID = new GridColumn();
                pagaduriaID.FieldName = this.unboundPrefix + "PagaduriaID";
                pagaduriaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagaduriaID");
                pagaduriaID.UnboundType = UnboundColumnType.Decimal;
                pagaduriaID.VisibleIndex = 1;
                pagaduriaID.Width = 30;
                pagaduriaID.Visible = false;
                pagaduriaID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                pagaduriaID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(pagaduriaID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudes.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas de los Detalles
        /// </summary>
        protected override void AddDetailCols()
        {
            try
            {
                //Campo de IncluidoInd
                GridColumn IncluidoInd = new GridColumn();
                IncluidoInd.FieldName = this.unboundPrefix + "IncluidoInd";
                IncluidoInd.Caption = "√";
                IncluidoInd.UnboundType = UnboundColumnType.Boolean;
                IncluidoInd.VisibleIndex = 0;
                IncluidoInd.Width = 15;
                IncluidoInd.Visible = true;
                IncluidoInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IncluidoInd");
                IncluidoInd.AppearanceHeader.ForeColor = Color.Lime;
                IncluidoInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                IncluidoInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                IncluidoInd.AppearanceHeader.Options.UseTextOptions = true;
                IncluidoInd.AppearanceHeader.Options.UseFont = true;
                IncluidoInd.AppearanceHeader.Options.UseForeColor = true;
                this.gvDetails.Columns.Add(IncluidoInd);


                //Campo de Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 2;
                Descriptivo.Width = 70;
                Descriptivo.Visible = true;
                Descriptivo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(Descriptivo);

                //Campo de DescripTExt
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "Descripcion";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 3;
                DescripTExt.Width = 200;
                DescripTExt.Visible = true;
                DescripTExt.OptionsColumn.AllowEdit = false;
                DescripTExt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                this.gvDetails.Columns.Add(DescripTExt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "AprobacionSolicitudes.cs-AddGridCols"));
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
                this.currentRow = -1;

                this.anexosAll = new List<DTO_ccSolicitudAnexo>();
                this.creditos = this._bc.AdministrationModel.LiquidacionCredito_GetAll(this.actividadFlujoID, false, false);
                if (this.creditos.Count > 0)
                {
                    this.allowValidate = false;
                    this.currentRow = 0;
                    gcDocuments.DataSource = this.creditos;
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                    this.LoadDetails();
                }
                else
                {
                    this.gcDocuments.DataSource = null;
                    this.gcDetails.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "EntregaLibranza.cs-LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected override void LoadDetails()
        {
            try
            {
                DTO_ccCreditoDocu doc = (DTO_ccCreditoDocu)this.currentDoc;
                int numeroDoc = doc.NumeroDoc.Value.Value;

                List<DTO_ccSolicitudAnexo> temp =
                    this.anexosAll.Where(x => x.NumeroDoc.Value.Value == numeroDoc).ToList();

                if (temp.Count > 0)
                    this.anexos = temp;
                else
                {
                    this.anexos = new List<DTO_ccSolicitudAnexo>();
                    List<DTO_MasterBasic> _pagaduriaAnexos = this._bc.AdministrationModel.ccAnexosLista_GetByPagaduria(this._pagaduriaID);
                    if (_pagaduriaAnexos.Count > 0)
                    {
                        foreach (DTO_MasterBasic basic in _pagaduriaAnexos)
                        {
                            DTO_ccSolicitudAnexo anexo = new DTO_ccSolicitudAnexo();
                            anexo.NumeroDoc.Value = numeroDoc;
                            anexo.DocumListaID.Value = basic.ID.Value;
                            anexo.Descriptivo.Value = basic.Descriptivo.Value;
                            anexo.Descripcion.Value = String.Empty;
                            anexo.IncluidoInd.Value = false;

                            this.anexos.Add(anexo);
                        }

                        this.anexosAll.AddRange(this.anexos);
                    }
                }

                this.gcDetails.DataSource = this.anexos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregaLibranza.cs", "LoadDetails"));
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
                    this.creditos[i].Aprobado.Value = true;
                    this.creditos[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this.creditos[i].Aprobado.Value = false;
                    this.creditos[i].Rechazado.Value = false;
                }
            }
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Modifica el Formato de los Valores.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            base.gvDocuments_CustomRowCellEdit(sender, e);

            string fieldName = null;
            fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrPrestamo" || fieldName == "VlrGiro")
            {
                e.RepositoryItem = this.editSpin;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.currentRow != -1)
                {
                    if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                    {
                        int fila = this.gvDocuments.RowCount;
                        this.currentRow = e.FocusedRowHandle;
                        if (gvDocuments != null)
                        {
                            this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                            this._pagaduriaID = this.creditos[fila - 1].PagaduriaID.Value;
                        }
                    }
                    this.LoadDetails();
                    this.detailsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregaLibranza.cs", "gvDocuments_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Modifica el Formato de los Valores.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //string fieldName = null;
            //fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            //if (fieldName == "ObligatorioInd")
            //{
            //    e.RepositoryItem = this.editChkBox;
            //}
        }

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
                    this.creditos[e.RowHandle].Rechazado.Value = false;
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this.creditos[e.RowHandle].Aprobado.Value = false;
            }
            #endregion

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
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
                if (this.creditos != null && this.creditos.Count != 0)
                {
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
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

                List<DTO_SerializedObject> results = _bc.AdministrationModel.Credito_AprobarRechazar(documentID, this.actividadFlujoID, this.creditos);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                int i = 0;
                int percent = 0;
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object result in results)
                {
                    #region manejo de resultados
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }

                    i++;
                    #endregion
                }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregaLibranza.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
