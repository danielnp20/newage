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
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Verificacion : SolicitudCreditoChequeo
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.solicitudes = new List<DTO_ccSolicitudDocu>();
            this.lkp_TipoIncorporacion.EditValue = string.Empty;
            
            this.firstTime = true;
            this.LoadDocuments();
        }

        #endregion

        public Verificacion()
            : base()
        {
            //InitializeComponent();
        }

        public Verificacion(string mod)
            : base(mod)
        {
        }

        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        int docRecursos;

        //DTO's
        private DTO_ccCentroPagoPAG centroPago = new DTO_ccCentroPagoPAG();
        private DTO_ccPagaduria pagaduria = new DTO_ccPagaduria();
        private List<DTO_ccSolicitudDocu> solicitudes = new List<DTO_ccSolicitudDocu>();
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        
        //Variables Privadas
        private int diaTope;
        private bool recaudoMes;
        private string centroPagoID = string.Empty;
        private bool firstTime = true;
        private bool incorporaLiquida;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que valida las condiciones para que el documento se pueda guardar
        /// </summary>
        private bool ValidateDoc()
        {
            if (!this.masterCentroPago.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                MessageBox.Show(msg);                
                return false;
            }
          
            return true;
        }

        /// <summary>
        /// Funcion que limpia los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            //Header
            this.lkp_TipoIncorporacion.EditValue = string.Empty;
            this.masterCentroPago.Value = String.Empty;

            //Footer           
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.solicitudes = new List<DTO_ccSolicitudDocu>();

            //Variables
            this.centroPagoID = String.Empty;
            this.incorporaLiquida = false;
            
            this.gcDocuments.Enabled = true;
            this.gcDocuments.DataSource = null;
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        ///  Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Verificacion;
            this.docRecursos = AppDocuments.Incorporacion;
            this.frmModule = ModulesPrefix.cc;

            //Cargar los Controles de Mestras
            this._bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, false, false);

            //Carga el combo
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(string.Empty, string.Empty);
            dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, "175_tbl_Liquida"));
            dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, "175_tbl_Previa"));
            dic.Add("3", this._bc.GetResource(LanguageTypes.Tables, "175_tbl_Visado"));
            this.lkp_TipoIncorporacion.Properties.DataSource = dic;
               
            //Permite modificar los paneles
            this.tableLayoutPanel1.RowStyles[0].Height = 40;
            this.tableLayoutPanel1.RowStyles[1].Height = 250;
            this.tableLayoutPanel1.RowStyles[2].Height = 0;
            this.tableLayoutPanel1.RowStyles[3].Height = 0;

            //Deshabilita los botones +- de la grilla
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Pone la fecha de aplica con base a la del periodo
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            DateTime fechaPerido = Convert.ToDateTime(strPeriodo);
            fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this.dtFechaVerificacion.Properties.MaxValue = new DateTime(fechaPerido.Year, fechaPerido.Month, DateTime.DaysInMonth(fechaPerido.Year, fechaPerido.Month));
            this.dtFechaVerificacion.Properties.MinValue = new DateTime(fechaPerido.Year, fechaPerido.Month, 1);
            this.dtFechaVerificacion.DateTime = new DateTime(fechaPerido.Year, fechaPerido.Month, fechaPerido.Day);

            //Carga recursos
            FormProvider.LoadResources(this, AppDocuments.Incorporacion);
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
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Aprobado");
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
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Campo de Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this.unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 2;
                Libranza.Width = 65;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Libranza);

                //Cliente Id
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this.unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 3;
                ClienteID.Width = 65;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ClienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 4;
                nombCliente.Width = 110;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombCliente);

                //Valor Libranza
                GridColumn VlrLibranza = new GridColumn();
                VlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                VlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrLibranza");
                VlrLibranza.UnboundType = UnboundColumnType.Decimal;
                VlrLibranza.VisibleIndex = 5;
                VlrLibranza.Width = 120;
                VlrLibranza.Visible = true;
                VlrLibranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrLibranza);

                //Valor Cuota
                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 6;
                VlrCuota.Width = 90;
                VlrCuota.Visible = true;
                VlrCuota.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrCuota);

                //Fecha Cuota 1
                GridColumn FechaCuota1 = new GridColumn();
                FechaCuota1.FieldName = this.unboundPrefix + "FechaCuota1";
                FechaCuota1.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_FechaCuota1");
                FechaCuota1.UnboundType = UnboundColumnType.DateTime;
                FechaCuota1.VisibleIndex = 7;
                FechaCuota1.Width = 120;
                FechaCuota1.Visible = true;
                FechaCuota1.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(FechaCuota1);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 8;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Carga el cabezote con los documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.currentDoc = null;
                if (this.lkp_TipoIncorporacion.EditValue == null)
                {
                    this.firstTime = false;
                    return;
                }
                
                if(this.lkp_TipoIncorporacion.EditValue.ToString() == "1")
                {
                    this.incorporaLiquida = true;
                    this.creditos = this._bc.AdministrationModel.IncorporacionCreditoVerificacion_GetByCentroPago(this.masterCentroPago.Value, this.actividadFlujoID);
                    this.solicitudes = new List<DTO_ccSolicitudDocu>();
                }
                else if(this.lkp_TipoIncorporacion.EditValue.ToString()  == "2")
                {
                    this.incorporaLiquida = false;
                    this.solicitudes = this._bc.AdministrationModel.IncorporacionSolicitudVerificacion_GetByCentroPago(this.masterCentroPago.Value,this.actividadFlujoID, 2);
                    this.creditos = new List<DTO_ccCreditoDocu>();
                }
                else if(this.lkp_TipoIncorporacion.EditValue.ToString() == "3")
                {
                    this.incorporaLiquida = false;
                    this.solicitudes = this._bc.AdministrationModel.IncorporacionSolicitudVerificacion_GetByCentroPago(this.masterCentroPago.Value, this.actividadFlujoID, 3);
                    this.creditos = new List<DTO_ccCreditoDocu>();
                }
                    
                this.currentRow = -1;
                if (this.creditos.Count > 0 || this.solicitudes.Count > 0)
                {
                    this.detailsLoaded = false;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    if (this.lkp_TipoIncorporacion.EditValue.ToString() == "1")
                        this.gcDocuments.DataSource = this.creditos;
                    else
                        this.gcDocuments.DataSource = this.solicitudes;

                    this.allowValidate = true;

                    if (!detailsLoaded)
                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);

                    this.gvDocuments.MoveFirst();
                }
                else if (!this.firstTime)
                {
                    this.creditos = new List<DTO_ccCreditoDocu>();
                    this.solicitudes = new List<DTO_ccSolicitudDocu>();
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                }
                this.firstTime = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "LoadDocuments"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < gvDocuments.DataRowCount; i++)
                {
                    if (this.lkp_TipoIncorporacion.EditValue.ToString() == "1")
                    {
                        this.creditos[i].Aprobado.Value = true;
                        this.creditos[i].Rechazado.Value = false;
                    }
                    else
                    {
                        this.solicitudes[i].Aprobado.Value = true;
                        this.solicitudes[i].Rechazado.Value = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < gvDocuments.DataRowCount; i++)
                {
                    if (this.lkp_TipoIncorporacion.EditValue.ToString() == "1")
                    {
                        this.creditos[i].Aprobado.Value = false;
                        this.creditos[i].Rechazado.Value = false;
                    }
                    else
                    {
                        this.solicitudes[i].Aprobado.Value = false;
                        this.solicitudes[i].Rechazado.Value = false;
                    }
                }
            }
            this.gcDocuments.RefreshDataSource();
        }

        /// <summary>
        /// Evento que se ejecuta al modificar la seleccion del combo, especifica como se debe filtrar la incopracion
        /// </summary>
        private void lkp_TipoIncorporacion_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.firstTime)
            {
                this.centroPagoID = String.Empty;
                this.masterCentroPago.Value = string.Empty;
                this.gcDocuments.DataSource = null;
            }
        }

        /// <summary>
        /// Evento que filtra los documentos de acuerdo a la pagaduria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCentroPago_Leave(object sender, EventArgs e)
        {
            if (this.centroPagoID != this.masterCentroPago.Value)
            {                
                this.centroPagoID = this.masterCentroPago.Value;
                this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.masterCentroPago.Value, true);
                if (this.centroPago != null)
                {
                    this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                    this.recaudoMes = pagaduria.RecaudoMes.Value.Value;
                    this.diaTope = pagaduria.DiaTope.Value.Value;
                    this.LoadDocuments();
                }
                else
                {
                    this.CleanData();
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                    MessageBox.Show(msg);
                }
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = null;
            fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "VlrLibranza" || fieldName == "VlrCuota")
                e.RepositoryItem = this.editSpin;
           
            if (fieldName == "Observacion")
                e.RepositoryItem = this.richText1;
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
                {
                    if (this.lkp_TipoIncorporacion.EditValue.ToString() == "1")
                        this.creditos[e.RowHandle].Rechazado.Value = false;
                    else
                        this.solicitudes[e.RowHandle].Rechazado.Value = false;
                }
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                {
                    if (this.lkp_TipoIncorporacion.EditValue.ToString() == "1")
                        this.creditos[e.RowHandle].Aprobado.Value = false;
                    else
                        this.solicitudes[e.RowHandle].Aprobado.Value = false;
                }
            }

            #endregion

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                if (((this.creditos != null && this.creditos.Count != 0) || (this.solicitudes != null && this.solicitudes.Count != 0)) && this.ValidateDoc())
                {
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "TBSave"));
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

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                decimal vlrIncorporacion = 0;
                
                if(incorporaLiquida)
                {
                    vlrIncorporacion = (from c in this.creditos where c.Aprobado.Value.Value select c.VlrLibranza.Value.Value).Sum();
                    this.solicitudes = new List<DTO_ccSolicitudDocu>();
                    results = _bc.AdministrationModel.VerificacionIncorporacion_AprobarRechazar(documentID, this.actividadFlujoID, this.centroPagoID, this.dtFechaVerificacion.DateTime, vlrIncorporacion, this.creditos, this.solicitudes);
                }
                else 
                {
                    vlrIncorporacion = (from c in this.solicitudes where c.Aprobado.Value.Value select c.VlrLibranza.Value.Value).Sum();
                    this.creditos = new List<DTO_ccCreditoDocu>();
                    results = _bc.AdministrationModel.VerificacionIncorporacion_AprobarRechazar(documentID, this.actividadFlujoID, this.centroPagoID, this.dtFechaVerificacion.DateTime, vlrIncorporacion, this.creditos, this.solicitudes);
                }

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

                    if (incorporaLiquida)
                    {
                        #region Envia el correo de los creditos aprobados
                        DTO_ccCreditoDocu crediAprobacion = this.creditos[i];
                        if (result.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult txtResult = (DTO_TxResult)result;
                            if (txtResult.Result == ResultValue.NOK)
                                resultsNOK.Add(txtResult);
                        }
                        else
                        {
                            #region Envia el correo
                            if (crediAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, crediAprobacion.NumeroDoc.Value, crediAprobacion.ClienteID.Value,
                                    crediAprobacion.Observacion.Value);
                            }
                            else if (crediAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, crediAprobacion.Observacion.Value, crediAprobacion.NumeroDoc.Value,
                                    crediAprobacion.ClienteID.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Envia el correo de las solicitudes aprobadas
                        DTO_ccSolicitudDocu soliAprobacion = this.solicitudes[i];
                        if (result.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult txtResult = (DTO_TxResult)result;
                            if (txtResult.Result == ResultValue.NOK)
                                resultsNOK.Add(txtResult);
                        }
                        else
                        {
                            #region Envia el correo
                            if (soliAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, soliAprobacion.NumeroDoc.Value, soliAprobacion.ClienteID.Value,
                                    soliAprobacion.Observacion.Value);
                            }
                            else if (soliAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, soliAprobacion.Observacion.Value, soliAprobacion.NumeroDoc.Value,
                                    soliAprobacion.ClienteID.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                        #endregion
                    }
                    i++;
                }

                MessageForm frm = new MessageForm(resultsNOK);
                frm.ShowDialog();
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        
        #endregion

    }
}
