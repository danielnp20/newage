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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class SolicitudAnticipos : DocumentForm
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
        private List<DTO_ccSolicitudCompraCartera> compCarteraAll = new List<DTO_ccSolicitudCompraCartera>();
        private List<DTO_ccSolicitudCompraCartera> compCarteraAnticipos = new List<DTO_ccSolicitudCompraCartera>();
        private List<DTO_ccSolicitudCompraCartera> compCarteraPyS = new List<DTO_ccSolicitudCompraCartera>();
        private List<DTO_ccSolicitudCompraCartera> compCarteraAnular = new List<DTO_ccSolicitudCompraCartera>();

        //Variables privadas
        private string libranzaID = String.Empty;

        #endregion

        public SolicitudAnticipos()
            : base()
        {
            //InitializeComponent();
        }

        public SolicitudAnticipos(string mod)
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

                this.documentID = AppDocuments.SolicitudAnticipo;
                this.frmModule = ModulesPrefix.cc;

                //Carga la fecha
                //int diaCorte = Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre));
                if (this.dtFecha.DateTime.Month == this.dtPeriod.DateTime.Month)
                    this.dtFecha.DateTime = DateTime.Now;

                //Carga la grilla con las columnas
                this.AddGridCols();

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 200;
                this.tlSeparatorPanel.RowStyles[1].Height = 500;
                this.tlSeparatorPanel.RowStyles[2].Height = 100;

                //Carga la informacion de Header
                _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, this.documentID.ToString() + "_tbl_EnviarAprobacion"));
                dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, this.documentID.ToString() + "_tbl_GuardarPazySalvo"));
                dic.Add("3", this._bc.GetResource(LanguageTypes.Tables, this.documentID.ToString() + "_tbl_AnulacionAnticipo"));
                this.lkp_Opcion.Properties.DataSource = dic;
                this.lkp_Opcion.Enabled = false;

                this.lkp_Opcion.EditValue = "1";

                this.gvDocument.Columns[this.unboundPrefix + "DocAnticipo"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "FechaPazySalvo"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "fileUrl"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "DocumentForm"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //IndRecibePazSalvo
                GridColumn indReciboPazSalvo = new GridColumn();
                indReciboPazSalvo.FieldName = this.unboundPrefix + "IndRecibePazySalvo";
                indReciboPazSalvo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IndRecibePazySalvo");
                indReciboPazSalvo.UnboundType = UnboundColumnType.Boolean;
                indReciboPazSalvo.VisibleIndex = 1;
                indReciboPazSalvo.Width = 40;
                indReciboPazSalvo.OptionsColumn.AllowEdit = true;
                indReciboPazSalvo.ColumnEdit = editChkBox;
                this.gvDocument.Columns.Add(indReciboPazSalvo);

                //CodEntidad
                GridColumn financieraID = new GridColumn();
                financieraID.FieldName = this.unboundPrefix + "FinancieraID";
                financieraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FinancieraID");
                financieraID.UnboundType = UnboundColumnType.String;
                financieraID.VisibleIndex = 2;
                financieraID.Width = 100;
                financieraID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(financieraID);

                //NomEntidad
                GridColumn NomEntidad = new GridColumn();
                NomEntidad.FieldName = this.unboundPrefix + "Descriptivo";
                NomEntidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                NomEntidad.UnboundType = UnboundColumnType.String;
                NomEntidad.VisibleIndex = 3;
                NomEntidad.Width = 250;
                NomEntidad.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(NomEntidad);

                //Documento
                GridColumn Documento = new GridColumn();
                Documento.FieldName = this.unboundPrefix + "Documento";
                Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
                Documento.UnboundType = UnboundColumnType.Integer;
                Documento.VisibleIndex = 4;
                Documento.Width = 100;
                Documento.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Documento);

                //VlrCarteraComprada
                GridColumn VlrCarteraComprada = new GridColumn();
                VlrCarteraComprada.FieldName = this.unboundPrefix + "VlrSaldo";
                VlrCarteraComprada.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldo");
                VlrCarteraComprada.UnboundType = UnboundColumnType.Decimal;
                VlrCarteraComprada.VisibleIndex = 5;
                VlrCarteraComprada.Width = 170;
                VlrCarteraComprada.OptionsColumn.AllowEdit = false;
                VlrCarteraComprada.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrCarteraComprada);

                //DocAnticipo
                GridColumn DocAnticipo = new GridColumn();
                DocAnticipo.FieldName = this.unboundPrefix + "DocAnticipo";
                DocAnticipo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocAnticipo");
                DocAnticipo.UnboundType = UnboundColumnType.Integer;
                DocAnticipo.VisibleIndex = 6;
                DocAnticipo.Width = 100;
                DocAnticipo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocAnticipo);

                //FechaPazSalvo
                GridColumn FechaPazySalvo = new GridColumn();
                FechaPazySalvo.FieldName = this.unboundPrefix + "FechaPazySalvo";
                FechaPazySalvo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaPazySalvo");
                FechaPazySalvo.UnboundType = UnboundColumnType.DateTime;
                FechaPazySalvo.VisibleIndex = 7;
                FechaPazySalvo.Width = 130;
                FechaPazySalvo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaPazySalvo);

                //Usuario
                GridColumn Usuario = new GridColumn();
                Usuario.FieldName = this.unboundPrefix + "UsuarioID";
                Usuario.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UsuarioID");
                Usuario.UnboundType = UnboundColumnType.String;
                Usuario.VisibleIndex = 8;
                Usuario.Width = 160;
                Usuario.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Usuario);

                //fileUrl
                GridColumn linkDoc = new GridColumn();
                linkDoc.FieldName = this.unboundPrefix + "fileUrl";
                linkDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_fileUrl");
                linkDoc.UnboundType = UnboundColumnType.String;
                linkDoc.VisibleIndex = 9;
                linkDoc.Width = 80;
                linkDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(linkDoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "AddGridCols"));
            }
            
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que valida las condiciones para que el documento se pueda guardar
        /// </summary>
        private bool ValidateDoc()
        {
            if (String.IsNullOrWhiteSpace(this.txtLibranza.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblLibranza);
                MessageBox.Show(msg);
                return false;
            }

            if (this.lkp_Opcion.EditValue == "1")
            {
                int count = (from a in this.compCarteraAnticipos where !a.AnticipoInd.Value.Value select a).Count();
                if (this.compCarteraAnticipos == null || this.compCarteraAnticipos.Count == 0 || count == 0)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NoAnticiposPend));
                    return false;
                }
            }
            else if (this.lkp_Opcion.EditValue == "2")
            {
                if (this.compCarteraPyS == null || this.compCarteraPyS.Count == 0)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NoPazySalvo));
                    return false;
                }
            }
            else
            {
                if (this.compCarteraAnular == null || this.compCarteraAnular.Count == 0)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "No hay anticipos para anular"));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Funcion que limpia los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            this.disableValidate = true;
            this.lkp_Opcion.EditValueChanged -= new System.EventHandler(this.lkp_Opcion_EditValueChanged);

            //TB
            FormProvider.Master.itemSave.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;

            //Header            
            this.txtLibranza.Text = String.Empty;
            this.lkp_Opcion.EditValue = "1";
            this.masterCliente.Value = String.Empty;

            this.gvDocument.Columns[this.unboundPrefix + "DocAnticipo"].Visible = false;
            this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].Visible = false;
            this.gvDocument.Columns[this.unboundPrefix + "FechaPazySalvo"].Visible = false;
            this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].Visible = false;
            this.gvDocument.Columns[this.unboundPrefix + "fileUrl"].Visible = false;

            //Footer           
            this.compCarteraAll = new List<DTO_ccSolicitudCompraCartera>();
            this.compCarteraAnticipos = new List<DTO_ccSolicitudCompraCartera>();
            this.compCarteraPyS = new List<DTO_ccSolicitudCompraCartera>();
            this.compCarteraAnular = new List<DTO_ccSolicitudCompraCartera>();

            //Variables
            this.libranzaID = String.Empty;
            this.lkp_Opcion.Enabled = false;

            this.gcDocument.DataSource = this.compCarteraAll;
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
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lkp_Opcion.EditValue.ToString() == "1")
                    this.compCarteraAnticipos.ForEach(c => c.FechaDoc.Value = this.dtFecha.DateTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "dtFecha_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Evento que carga las solicitudes dependiendo de la pagaduria
        /// </summary>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.libranzaID != this.txtLibranza.Text.Trim())
                {
                    string tmp = this.txtLibranza.Text;
                    this.libranzaID = this.txtLibranza.Text.Trim();
                    int libranzaTemp = Convert.ToInt32(this.txtLibranza.Text.Trim());

                    DTO_glDocumentoControl ctrl = _bc.AdministrationModel.glDocumentoControl_GetByLibranzaSolicitud(libranzaTemp, string.Empty, true);
                    if (ctrl == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoExiste));
                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                    }
                    else if (ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                    }
                    else if (ctrl.Estado.Value.Value == (int)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaAprobada));
                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                    }
                    else
                    {
                        this.compCarteraAll = _bc.AdministrationModel.SolicitudAnticipo_GetByLibranza(this.documentID, libranzaTemp, ctrl.NumeroDoc.Value.Value);

                        if (compCarteraAll.Count > 0)
                        {
                            this.masterCliente.Value = this.compCarteraAll[0].ClienteID.Value;
                            this.compCarteraAnticipos = ObjectCopier.Clone(this.compCarteraAll).Where(x => !x.AnticipoInd.Value.Value).ToList();
                            this.compCarteraPyS = ObjectCopier.Clone(this.compCarteraAll);
                            this.compCarteraAnular = ObjectCopier.Clone(this.compCarteraAll).Where(x => !string.IsNullOrEmpty(x.DocAnticipo.Value.ToString())).ToList();

                            if (this.lkp_Opcion.EditValue.ToString() == "1")
                                this.compCarteraAnticipos.ForEach(c => c.FechaDoc.Value = this.dtFecha.DateTime);
                            
                            this.gcDocument.DataSource = this.compCarteraAnticipos;
                            this.gvDocument.MoveFirst();

                            this.lkp_Opcion.EditValueChanged += new System.EventHandler(this.lkp_Opcion_EditValueChanged);


                            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                            FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);

                            this.lkp_Opcion.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaSinCompras));
                            this.CleanData();
                            this.txtLibranza.Text = tmp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que cambia la info que se debe mostrar en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_Opcion_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lkp_Opcion.EditValue.ToString() == "1")
                {
                    this.disableValidate = true;
                    this.gvDocument.Columns[this.unboundPrefix + "DocAnticipo"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "FechaPazySalvo"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "fileUrl"].Visible = false;

                    this.compCarteraAnticipos.ForEach(c => c.FechaDoc.Value = this.dtFecha.DateTime);
                    this.gcDocument.DataSource = this.compCarteraAnticipos;
                    this.gvDocument.MoveFirst();
                }
                else if (this.lkp_Opcion.EditValue.ToString() == "2")
                {
                    this.disableValidate = false;
                    this.gvDocument.Columns[this.unboundPrefix + "DocAnticipo"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "FechaPazySalvo"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "fileUrl"].Visible = true;

                    this.gcDocument.DataSource = this.compCarteraPyS;
                    this.gvDocument.MoveFirst();
                }
                else
                {
                    this.disableValidate = false;
                    this.gvDocument.Columns[this.unboundPrefix + "DocAnticipo"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "FechaPazySalvo"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "fileUrl"].Visible = true;

                    this.gcDocument.DataSource = this.compCarteraAnular;
                    this.gvDocument.MoveFirst();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "lkp_Opcion_EditValueChanged"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (!this.disableValidate && e.FocusedRowHandle >= 0)
                {
                    if (!this.compCarteraPyS[e.FocusedRowHandle].ExternaInd.Value.Value || !this.compCarteraPyS[e.FocusedRowHandle].AnticipoInd.Value.Value)
                        this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].OptionsColumn.AllowEdit = false;
                    else
                        this.gvDocument.Columns[this.unboundPrefix + "IndRecibePazySalvo"].OptionsColumn.AllowEdit = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "gvDocuments_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                if (this.ValidateDoc())
                {
                    Thread process = new Thread(this.SaveThread);
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
                if(this.lkp_Opcion.EditValue == "1")
                    results = _bc.AdministrationModel.SolicitudAnticipos_SolicitarAnticipos(this.documentID, this.compCarteraAnticipos);
                else if (this.lkp_Opcion.EditValue == "2")
                    results = _bc.AdministrationModel.SolicitudAnticipos_GenerarPazYSalvo(this.documentID, this.compCarteraPyS);
                else
                    results = _bc.AdministrationModel.SolicitudAnticipos_RevertirAnticipos(this.documentID, this.compCarteraAnular);

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
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                            break;
                        }
                        #endregion

                        if (!this.compCarteraAll[i].IndRecibePazySalvo.Value.Value)
                        {
                            bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.Anticipos, this._actFlujo.seUsuarioID.Value, obj, false);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudAnticipos.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
