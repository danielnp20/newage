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
    public partial class ReasignaCompradorFinal  : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private DTO_ReasignaCompradorFinal reasignaCompradorFinal = new DTO_ReasignaCompradorFinal();
        private List<DTO_ccCompradorFinalDeta> compradorFinDeta = new List<DTO_ccCompradorFinalDeta>();

        //Variables privadas
        private string compradorCarteraID = String.Empty;
        private string compradorCarteraFinal = String.Empty;
        private DateTime periodo;

        #endregion

        public ReasignaCompradorFinal()
            : base()
        {
            //InitializeComponent();
        }

        public ReasignaCompradorFinal(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.InitializeComponent();
                base.SetInitParameters();

                this.documentID = AppDocuments.ReasignaCompradorFinal;
                this.frmModule = ModulesPrefix.cc;

                //Carga la maestra de comprador de cartera
                #region Crea el filtro de Inversionista FinalInd para la maestra de comprador Cartera
                List<DTO_glConsultaFiltro> filtroCompradorCartera = new List<DTO_glConsultaFiltro>();
                filtroCompradorCartera.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "InversionistaFinalInd",
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = OperadorSentencia.And,
                    ValorFiltro = "0"
                });
                #endregion
                this._bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, false, filtroCompradorCartera);

                #region Crea el filtro de Inversionista FinalInd para la maestra de comprador Cartera
                List<DTO_glConsultaFiltro> filtroCompradorFinal = new List<DTO_glConsultaFiltro>();
                filtroCompradorFinal.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "InversionistaFinalInd",
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = OperadorSentencia.And,
                    ValorFiltro = "1"
                });
                #endregion
                this._bc.InitMasterUC(this.masterCompradorFinal, AppMasters.ccCompradorCartera, false, true, true, false, filtroCompradorFinal);

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 180;
                this.tlSeparatorPanel.RowStyles[1].Height = 500;

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReasignaCompradorFinal.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //PagoFlujoInd
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.OptionsColumn.AllowEdit = false;
                aprob.Visible = true;
                aprob.ColumnEdit = editChkBox;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoFlujoInd");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(aprob);

                //CompradorFinal
                GridColumn compradorFinal = new GridColumn();
                compradorFinal.FieldName = this.unboundPrefix + "CompradorFinal";
                compradorFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorFinal");
                compradorFinal.UnboundType = UnboundColumnType.String;
                compradorFinal.VisibleIndex = 1;
                compradorFinal.Width = 60;
                compradorFinal.Visible = true;
                compradorFinal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(compradorFinal);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "LibranzaID";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LibranzaID");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 2;
                libranza.Width = 70;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //Cliente
                GridColumn cliente = new GridColumn();
                cliente.FieldName = this.unboundPrefix + "ClienteID";
                cliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                cliente.UnboundType = UnboundColumnType.String;
                cliente.VisibleIndex = 3;
                cliente.Width = 100;
                cliente.Visible = true;
                cliente.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cliente);

                //Nombre
                GridColumn ombre = new GridColumn();
                ombre.FieldName = this.unboundPrefix + "Nombre";
                ombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                ombre.UnboundType = UnboundColumnType.String;
                ombre.VisibleIndex = 4;
                ombre.Width = 250;
                ombre.Visible = true;
                ombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ombre);

                //Vlr Libranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Boolean;
                vlrLibranza.VisibleIndex = 5;
                vlrLibranza.Width = 100;
                vlrLibranza.OptionsColumn.AllowEdit = true;
                vlrLibranza.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrLibranza);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this.unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.VisibleIndex = 6;
                observacion.Width = 150;
                observacion.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(observacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReasignaCompradorFinal.cs", "AddGridCols"));
            }
            
        }

        #endregion

        #region Funciones Privadas
        
        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.masterCompradorCartera.Value = String.Empty;
            this.masterCompradorFinal.Value = String.Empty;
            
            //Variables
            this.compradorCarteraID = string.Empty;
            this.compradorCarteraFinal = string.Empty;

            this.reasignaCompradorFinal = new DTO_ReasignaCompradorFinal();
            this.gcDocument.DataSource = this.reasignaCompradorFinal.CompradorFinalDeta;

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
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemNew.Visible = true;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReasignaCompradorFinal.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al momento de salir del comprador de cartera
        /// </summary>
        private void masterCompradorCartera_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.compradorCarteraID != this.masterCompradorCartera.Value)
                {
                    this.compradorCarteraID = this.masterCompradorCartera.Value;
                    if (this.masterCompradorCartera.ValidID)
                    {
                        this.reasignaCompradorFinal = this._bc.AdministrationModel.ReasignaCompradorCartera_Get(this._actFlujo.ID.Value, this.compradorCarteraID);
                        if (this.reasignaCompradorFinal.CompradorFinalDeta.Count > 0)
                        {
                            this.reasignaCompradorFinal.CompradorFinalDocu.CompradorCarteraID.Value = this.compradorCarteraID;
                            this.reasignaCompradorFinal.CompradorFinalDocu.FechaReasigna.Value = this.dtFecha.DateTime;
                            this.compradorFinDeta = this.reasignaCompradorFinal.CompradorFinalDeta;
                            this.gcDocument.DataSource = this.compradorFinDeta;
                            this.gvDocument.MoveFirst();
                        }
                        else
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            this.CleanData();
                        }
                    }
                    else
                    {
                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.compradorCarteraID);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReasignaCompradorFinal.cs", "masterCompradorCartera_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al momento de salir del comprador final de cartera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCompradorFinal_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.compradorCarteraFinal != this.masterCompradorFinal.Value)
                {
                    if (this.masterCompradorFinal.ValidID)
                    {
                        this.compradorCarteraFinal = this.masterCompradorFinal.Value;
                        this.gvDocument.Columns[0].OptionsColumn.AllowEdit = true;
                    }
                        
                    else
                    {
                        this.compradorCarteraFinal = string.Empty;
                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lbl_CompradorFinal.Text);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReasignaCompradorFinal.cs", "masterCompradorCartera_Leave"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            #region Generales
            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    this.reasignaCompradorFinal.CompradorFinalDocu.CompradorFinal.Value = this.compradorCarteraFinal;

                }
                else
                {
                    this.reasignaCompradorFinal.CompradorFinalDocu.CompradorFinal.Value = null;
                }

            }

            #endregion

            this.gcDocument.RefreshDataSource();
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                List<DTO_ccCompradorFinalDeta> compradorFinDetaTemp = this.compradorFinDeta.Where(x => x.Aprobado.Value == true && x.CompradorFinal.Value != null).ToList();
                if (compradorFinDetaTemp.Count > 0)
                {
                    this.reasignaCompradorFinal.CompradorFinalDeta = compradorFinDetaTemp;
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReasignarCompradorCartera_NoSelected);
                    MessageBox.Show(msg);
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
        public override void SaveThread()
        {
            try
            {
                #region Guarda la info
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                results = _bc.AdministrationModel.ReasignaCompradorCartera_Add(this.documentID, this._actFlujo.ID.Value, this.reasignaCompradorFinal);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                this.isValid = true;
                MessageForm frm = null;
                bool checkResults = true;
                if (results.Count == 1)
                {
                    if (results[0].GetType() == typeof(DTO_TxResult))
                    {
                        checkResults = false;
                        frm = new MessageForm((DTO_TxResult)results[0]);
                        this.isValid = false;
                    }
                }

                if (checkResults)
                {
                    foreach (object obj in results)
                    {
                        #region Funciones de progreso
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (results.Count);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                            break;
                        }
                        #endregion

                        if (this.compradorFinDeta[i].Aprobado.Value.Value)
                        {
                            bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.ReasignaCompradorFinal, this._actFlujo.seUsuarioID.Value, obj, false);
                            if (!isOK)
                            {
                                DTO_TxResult r = (DTO_TxResult)obj;
                                resultsNOK.Add(r);
                                this.isValid = false;
                            }
                        }

                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (this.isValid)
                    this.Invoke(this.refreshGridDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReasignaCompradorFinal.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        
        #endregion

    }
        
}
