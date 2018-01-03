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
    public partial class CambioFechaPlanPagos : DocumentForm
    {

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private DTO_Credito credito = new DTO_Credito();

        //Variables privadas
        private string libranzaID = string.Empty;
        private string libranzaTmp = string.Empty;
        private DateTime periodo;
        private DateTime fechaCuota;
        private bool firstTime = true;
        private bool isQuincenal = false;
        private SectorCartera sector = SectorCartera.Solidario;

        #endregion

        public CambioFechaPlanPagos()
            : base()
        {
            //this.InitializeComponent();
        }

        public CambioFechaPlanPagos(string mod)
            : base(mod)
        {
            //this.InitializeComponent();
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

                this.documentID = AppDocuments.CambioFechaPlanPagos;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 155;
                this.tlSeparatorPanel.RowStyles[1].Height = 400;

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                //Sector cartera
                string sectorStr = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                this.sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorStr);

                //this.dtFecha.DateTime = DateTime.Now;
                //this.dtNuevaFechaCta.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtNuevaFechaCta.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);
                this.firstTime = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioFechaPlanPagos.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //CuotaID
                GridColumn cuotaID = new GridColumn();
                cuotaID.FieldName = this.unboundPrefix + "CuotaID";
                cuotaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaID");
                cuotaID.UnboundType = UnboundColumnType.Integer;
                cuotaID.VisibleIndex = 0;
                cuotaID.Width = 80;
                cuotaID.Visible = true;
                cuotaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cuotaID);

                //Fecha Cuota
                GridColumn fechaCuota = new GridColumn();
                fechaCuota.FieldName = this.unboundPrefix + "FechaCuota";
                fechaCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCuota");
                fechaCuota.UnboundType = UnboundColumnType.DateTime;
                fechaCuota.VisibleIndex = 1;
                fechaCuota.Width = 80;
                fechaCuota.Visible = true;
                fechaCuota.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(fechaCuota);

                //VlrCuota
                GridColumn vlrCuotaCartera = new GridColumn();
                vlrCuotaCartera.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuotaCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrCuotaCartera.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCartera.VisibleIndex = 2;
                vlrCuotaCartera.Width = 150;
                vlrCuotaCartera.Visible = true;
                vlrCuotaCartera.ColumnEdit = this.editSpin;
                vlrCuotaCartera.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrCuotaCartera);

                //VlrPagadoCuota
                GridColumn vlrPagadoCuota = new GridColumn();
                vlrPagadoCuota.FieldName = this.unboundPrefix + "VlrPagadoCuota";
                vlrPagadoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPagadoCuota");
                vlrPagadoCuota.UnboundType = UnboundColumnType.Integer;
                vlrPagadoCuota.VisibleIndex = 3;
                vlrPagadoCuota.Width = 80;
                vlrPagadoCuota.Visible = true;
                vlrPagadoCuota.ColumnEdit = this.editSpin;
                vlrPagadoCuota.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrPagadoCuota);

                //VlrCapital
                GridColumn vlrCapital = new GridColumn();
                vlrCapital.FieldName = this.unboundPrefix + "VlrCapital";
                vlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCapital");
                vlrCapital.UnboundType = UnboundColumnType.Integer;
                vlrCapital.VisibleIndex = 4;
                vlrCapital.Width = 150;
                vlrCapital.Visible = true;
                vlrCapital.ColumnEdit = this.editSpin;
                vlrCapital.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrCapital);

                //VlrInteres
                GridColumn vlrInteres = new GridColumn();
                vlrInteres.FieldName = this.unboundPrefix + "VlrInteres";
                vlrInteres.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrInteres");
                vlrInteres.UnboundType = UnboundColumnType.Integer;
                vlrInteres.VisibleIndex = 5;
                vlrInteres.Width = 150;
                vlrInteres.Visible = true;
                vlrInteres.ColumnEdit = this.editSpin;
                vlrInteres.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrInteres);

                //VlrSeguro
                GridColumn vlrSeguro = new GridColumn();
                vlrSeguro.FieldName = this.unboundPrefix + "VlrSeguro";
                vlrSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSeguro");
                vlrSeguro.UnboundType = UnboundColumnType.Integer;
                vlrSeguro.VisibleIndex = 6;
                vlrSeguro.Width = 150;
                vlrSeguro.Visible = true;
                vlrSeguro.ColumnEdit = this.editSpin;
                vlrSeguro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrSeguro);

                //VlrOtrosFijos
                GridColumn vlrOtros = new GridColumn();
                vlrOtros.FieldName = this.unboundPrefix + "VlrOtrosFijos";
                vlrOtros.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrOtrosFijos");
                vlrOtros.UnboundType = UnboundColumnType.Integer;
                vlrOtros.VisibleIndex = 7;
                vlrOtros.Width = 150;
                vlrOtros.Visible = true;
                vlrOtros.ColumnEdit = this.editSpin;
                vlrOtros.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrOtros);

                this.gvDocument.BestFitColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "CambioFechaPlanPagos.cs-AddGridCols"));
            }

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            if (DateTime.Now.Month != periodo.Month)
            {
                this.dtFecha.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                this.dtFecha.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                this.dtFecha.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
            }
            else
                this.dtFecha.DateTime = DateTime.Now;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que limpia el documento
        /// </summary>
        private void CleanData()
        {
            this.firstTime = true;
            this.libranzaID = string.Empty;
            this.txtLibranza.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;

            this.credito = new DTO_Credito();
            this.gcDocument.DataSource = this.credito.PlanPagos;
            this.dtNuevaFechaCta.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);
            this.firstTime = false;
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
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Visible = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioFechaPlanPagos.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que trae el plan de pagos de una libranza especifica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.libranzaID != this.txtLibranza.Text.Trim())
                {
                    this.libranzaTmp = this.txtLibranza.Text;
                    this.libranzaID = this.txtLibranza.Text.Trim();
                    int libranzaTemp = Convert.ToInt32(this.txtLibranza.Text.Trim());
                    this.credito = _bc.AdministrationModel.GetCredito_All(libranzaTemp);
                    if (this.credito.CreditoDocu != null)
                    {
                        DTO_ccPagaduria pag = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.credito.CreditoDocu.PagaduriaID.Value, true);
                        this.isQuincenal = pag.PeriodoPago.Value == 1 ? false : true;
                        this.fechaCuota = this.credito.PlanPagos[0].FechaCuota.Value.Value;
                        //Validación financiera
                        //if (this.sector == SectorCartera.Financiero)
                        //{
                        //    foreach (var pp in this.credito.PlanPagos)
                        //        pp.VlrSeguro.Value = pp.VlrSeguro.Value + pp.VlrOtro1.Value;                           
                        //}
                            
                        this.gcDocument.DataSource = this.credito.PlanPagos;
                        this.gvDocument.MoveFirst();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                        MessageBox.Show(msg);
                        this.CleanData();
                        this.txtLibranza.Text = this.libranzaTmp;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioFechaPlanPagos.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que establece la nueva fecha del plan de pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtNuevaFechaCta_DateTimeChanged(object sender, EventArgs e)
        {
            if (!this.firstTime)
            {
                int dia = this.dtNuevaFechaCta.DateTime.Day;
                int mes = this.dtNuevaFechaCta.DateTime.Month;
                string msgTitle = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NuevaFechaInvalid);
                if (this.dtNuevaFechaCta.DateTime.CompareTo(this.fechaCuota) < 0 && MessageBox.Show(msg + ", desea continuar?", msgTitle, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    this.dtNuevaFechaCta.Focus();                  
                }
                else if (this.credito.PlanPagos[0].VlrPagadoCuota.Value != 0)
                {
                    msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NuevaFechaConPago);
                    MessageBox.Show(msg);
                    this.txtLibranza.Focus();
                }
                else
                {
                    int i = 1;
                    DateTime fechaTmp = this.dtNuevaFechaCta.DateTime;
                    if (this.sector != SectorCartera.Financiero)
                    {
                        if (this.isQuincenal)
                        {
                            #region Quincenal
                            if (dia > 15)
                            {
                                if (mes == 2)
                                    fechaTmp = new DateTime(fechaTmp.Year, fechaTmp.Month, 28);
                                else
                                    fechaTmp = new DateTime(fechaTmp.Year, fechaTmp.Month, 30);
                            }
                            else
                                fechaTmp = new DateTime(fechaTmp.Year, fechaTmp.Month, 15); 
                            #endregion
                        }
                        else
                        {
                            #region Mensual
                            if (mes == 2)
                                fechaTmp = new DateTime(fechaTmp.Year, fechaTmp.Month, 28);
                            else
                                fechaTmp = new DateTime(fechaTmp.Year, fechaTmp.Month, 30); 
                            #endregion
                        }
                    }

                    #region Asigna Nuevas Fechas
                    foreach (DTO_ccCreditoPlanPagos pp in this.credito.PlanPagos)
                    {
                        if (pp.CuotaID.Value == 1)
                        {
                            pp.FechaCuota.Value = fechaTmp;
                            this.credito.CreditoDocu.FechaCuota1.Value = fechaTmp;
                        }
                        else
                        {
                            if (this.sector != SectorCartera.Financiero)
                            {
                                if (mes == 2 && this.isQuincenal)
                                {
                                    pp.FechaCuota.Value = fechaTmp.AddMonths(i - 1);
                                    if (pp.FechaCuota.Value.Value.Month != 2)
                                        pp.FechaCuota.Value = pp.FechaCuota.Value.Value.AddDays(2);
                                }
                                else
                                    pp.FechaCuota.Value = fechaTmp.AddMonths(i - 1);
                            }
                            else
                                pp.FechaCuota.Value = fechaTmp.AddMonths(i - 1);
                        }

                        pp.FechaLiquidaMora.Value = pp.FechaCuota.Value;
                        pp.FechaLiquidaMoraANT.Value = pp.FechaCuota.Value;
                        i++;
                    } 
                    #endregion

                    this.gcDocument.RefreshDataSource();
                }
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
            this.gcDocument.RefreshDataSource();
            this.ValidateRow(e.RowHandle);
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
                if (this.credito != null && this.credito.PlanPagos.Count != 0)
                {
                    Thread process = new Thread(this.SaveThread);
                    this.credito.CreditoDocu.Observacion.Value = this.txtObservacion.Text;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioFechaPlanPago.cs", "TBNew"));
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = this._bc.AdministrationModel.CambiaFechaPlanPagos(documentID, this.credito);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                int i = 0;
                int percent = 0;
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
                        bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.CambioFechaPlanPagos, this._actFlujo.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                            this.isValid = false;
                        }
                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (isValid)
                    this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioFechaPlanPagos.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }

}
