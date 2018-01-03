using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Reportes;
using NewAge.Cliente.GUI.WinApp.Reports.Formularios;
using System.Collections;
using NewAge.ReportesComunes;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Furmulario para procesar / consultar una declaracion de impuestos
    /// </summary>
    public partial class ProcesarDeclaracion : Form
    {
        #region Funciones delegados

        /// <summary>
        /// Hilo con la barra de progreso
        /// </summary>
        public Thread ProgressBarThread
        {
            get;
            set;
        }

        /// <summary>
        /// Funcion que se ejecuta ver el estado 
        /// Dependiendo si es maestra, app, etc
        /// </summary>   
        public Func<int> FuncProgressBarThread
        {
            get;
            set;
        }

        #endregion

        #region Delegados

        private delegate void UpdateProgress(int progress);
        private UpdateProgress UpdateProgressDelegate;
        /// <summary>
        /// Delegado que actualiza la barra de progreso
        /// </summary>
        /// <param name="progress"></param>
        private void UpdateProgressBar(int progress)
        {
            this.pbProcess.Position = progress;
            this.pbProcess.Update();
        }

        #endregion

        #region Variables
        BaseController _bc = BaseController.GetInstance();
        private string _frmName;
        private int _documentID;
        private string unboundPrefix = "Unbound_";

        //Info de grilla y formulario
        private List<DTO_coImpDeclaracionDetaRenglon> data;
        private string _impuestoID;
        private short _periodoCalendario;
        private short _añoDeclaracion;
        private short _mesDeclaracion;
        private int? _numeroDoc;

        Form _frmParent;
        #endregion

        public ProcesarDeclaracion(Form parent, string impuestoID, short periodoCalendario, short añoDeclaracion, short mesDeclaracion, int? numeroDoc, EstadoDocControl estado)
        {
            try
            {
                this.InitializeComponent();

                this._frmParent = parent;
                this._impuestoID = impuestoID;
                this._periodoCalendario = periodoCalendario;
                this._numeroDoc = numeroDoc;
                this._añoDeclaracion = añoDeclaracion;
                this._mesDeclaracion = mesDeclaracion;
                this._documentID = AppDocuments.DeclaracionImpuestos;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.LoadResources(this, this._documentID);

                this.UpdateProgressDelegate = new UpdateProgress(this.UpdateProgressBar);

                this.txtAñoFiscal.Text = this._añoDeclaracion.ToString();
                this.txtPeriodoFiscal.Text = this._mesDeclaracion.ToString();

                //Revisa si la declaracion ya esta aprobada
                if (estado != EstadoDocControl.Aprobado)
                    this.btnProcesar.Visible = FormProvider.Master.itemSave.Enabled;
                else
                    this.btnProcesar.Visible = false;

                //Carga las variables iniciales
                this.AddGridCols();
                this.LoadData();

                #region Reports
                this.cmbReportType.Items.Add(new ComboBoxItem("1", _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Declaracion)));
                this.cmbReportType.Items.Add(new ComboBoxItem("2", _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Soporte)));
                this.cmbReportType.Items.Add(new ComboBoxItem("3", _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account)));
                this.cmbReportType.DisplayMember = "Text";
                this.cmbReportType.SelectedIndex = 0;
                if (this.data == null ||this.data.Count == 0)
                    this.gbReport.Enabled = false;
                #endregion
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarDeclaracion.cs", "ProcesarDeclaracion"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Revisa el estado del proceso que se este corriendo en el servidor
        /// </summary>
        private void CheckServerProcessStatus()
        {
            try
            {
                this.Invoke(this.UpdateProgressDelegate, new object[] { 0 });
                while (true)
                {
                    //Thread.Sleep(1000);
                    int progress = this.FuncProgressBarThread.Invoke();
                    this.Invoke(this.UpdateProgressDelegate, new object[] { progress });
                }
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Cancela el hilo que se este ejecutando con la barra de estado
        /// </summary>
        private void StopProgressBarThread()
        {
            try
            {
                this.ProgressBarThread.Abort();
                this.Invoke(this.UpdateProgressDelegate, new object[] { 0 });
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Renglon
            GridColumn renglon = new GridColumn();
            renglon.FieldName = this.unboundPrefix + "Renglon";
            renglon.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Renglon");
            renglon.UnboundType = UnboundColumnType.String;
            renglon.VisibleIndex = 0;
            renglon.Width = 30;
            renglon.OptionsColumn.AllowEdit = false;
            this.gvRenglones.Columns.Add(renglon);

            //Descripcion
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Descriptivo";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 1;
            desc.Width = 130;
            renglon.OptionsColumn.AllowEdit = false;
            this.gvRenglones.Columns.Add(desc);

            //Valor
            GridColumn year = new GridColumn();
            year.FieldName = this.unboundPrefix + "Valor";
            year.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
            year.UnboundType = UnboundColumnType.String;
            year.VisibleIndex = 2;
            year.Width = 60;
            year.OptionsColumn.AllowEdit = false;
            this.gvRenglones.Columns.Add(year);

            //ValorAjustado
            GridColumn period = new GridColumn();
            period.FieldName = this.unboundPrefix + "ValorAjustado";
            period.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorAjustado");
            period.UnboundType = UnboundColumnType.String;
            period.VisibleIndex = 3;
            period.Width = 60;
            period.OptionsColumn.AllowEdit = false;
            this.gvRenglones.Columns.Add(period);
        }

        /// <summary>
        /// Actualiza la informacion de la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {
                //Carga la informacion de la grilla
                if (!this._numeroDoc.HasValue)
                {
                    //DTO_glConsulta filtro = new DTO_glConsulta();

                    //List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    //DTO_glConsultaFiltro campo = new DTO_glConsultaFiltro();
                    //campo.CampoFisico = "ImpuestoDeclID";
                    //campo.ValorFiltro = this._impuestoID;
                    //campo.OperadorFiltro = OperadoresFiltro.Igual;
                    //filtros.Add(campo);

                    //filtro.Filtros = filtros;

                    //List<DTO_coImpDeclaracionDetaRenglon> detaList = new List<DTO_coImpDeclaracionDetaRenglon>();
                    //long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpDeclaracionRenglon, filtro, true);

                    //List<DTO_MasterComplex> renglones = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpDeclaracionRenglon, Convert.ToInt32(count), 1, filtro, true).ToList();
                    //renglones.ForEach(rC =>
                    //{
                    //    DTO_coImpDeclaracionRenglon r = (DTO_coImpDeclaracionRenglon)rC;
                    //    DTO_coImpDeclaracionDetaRenglon deta = new DTO_coImpDeclaracionDetaRenglon();
                    //    deta.Renglon.Value = r.Renglon.Value;
                    //    deta.Descriptivo.Value = r.Descriptivo.Value;
                    //    deta.Valor.Value = 0;
                    //    deta.ValorAjustado.Value = 0;

                    //    detaList.Add(deta);
                    //});
                    //this.data = detaList;
                    this.data = _bc.AdministrationModel.DeclaracionesRenglones_Get(0, this._impuestoID, this._mesDeclaracion, this._añoDeclaracion);
                }
                else
                    this.data = _bc.AdministrationModel.DeclaracionesRenglones_Get(this._numeroDoc.Value, this._impuestoID, this._mesDeclaracion, this._añoDeclaracion);

                this.gcRenglones.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarDeclaracion.cs", "LoadData"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento para procesar una declaracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcesarDeclaracion_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Active = this._frmParent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarDeclaracion.cs", "FormClosed"));
            }
        }

        /// <summary>
        /// Evento para procesar una declaracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                string perCxP = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.co_Periodo);
                DateTime periodoCxP = Convert.ToDateTime(perCxP);

                if (periodoCxP.Month != DateTime.Now.Month || periodoCxP.Year != DateTime.Now.Year)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_ProcDecPeriodo));
                }
                else
                {
                    this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this._documentID));
                    this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                    this.ProgressBarThread.Start();

                    DTO_TxResult result = _bc.AdministrationModel.ProcesarDeclaracion(this._documentID, this._impuestoID, this._periodoCalendario, this._mesDeclaracion, this._añoDeclaracion, this._numeroDoc);

                    if (result.Result == ResultValue.OK)
                    {
                        this._numeroDoc = Convert.ToInt32(result.ResultMessage);
                        DTO_glDocumentoControl ctrl = _bc.AdministrationModel.glDocumentoControl_GetByID(this._numeroDoc.Value);
                        result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Doc) + ": " + result.ResultMessage;

                        if (this.gbReport.Enabled == false)
                            this.gbReport.Enabled = true;

                        try
                        {
                            #region Envia mail
                            string subject = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                            string bodyFormat = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Body);

                            List<string> mails = new List<string>();
                            List<DTO_Alarma> actividades = new List<DTO_Alarma>(); //_bc.AdministrationModel.Comprobante_GetFirstAlarm(out mails, this._documentID, ctrl.PeriodoDoc.Value.Value,
                                //ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value);

                            if (actividades.Count > 0 && mails.Count > 0)
                            {
                                foreach (DTO_Alarma act in actividades)
                                {
                                    string userToken = act.UsuarioRESP + act.DocumentoID + act.DocumentoID + act.FileName;
                                    string body = string.Format(bodyFormat, act.DocumentoID, act.DocumentoDesc, act.TerceroID, act.TerceroDesc, act.PrefijoID,
                                        act.Consecutivo, act.UsuarioRESP, act.FileName);
                                    string recipients = string.Empty;

                                    for (int i = 0; i < mails.Count; ++i)
                                    {
                                        recipients += mails[i];

                                        if (i != mails.Count - 1)
                                            recipients += ",";
                                    }

                                    _bc.SendMail(this._documentID, subject, body, recipients);
                                }
                            }
                            #endregion
                        }
                        catch (Exception)
                        { ; }

                        this.LoadData();
                    }
                    this.StopProgressBarThread();
                    MessageForm frm = new MessageForm(result);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProcesarDeclaracion.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        /// <summary>
        /// Evento para sacar el reporte de la declaracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportDec_Click(object sender, EventArgs e)
        {
            if (this._numeroDoc != null)
            {
                #region Variables
                int declarType = 0;
                DTO_coPlanCuenta _cuentaInfo;
                string _tercDesc;
                int _saldoControl;
                Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, string> cacheTerc = new Dictionary<string, string>();
                Dictionary<string, int> cacheConcSaldo = new Dictionary<string, int>();
                UDT_BasicID udtTercero = new UDT_BasicID() { Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto) };

                DTO_coImpuestoDeclaracion impuestoInfo = (DTO_coImpuestoDeclaracion)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, false, this._impuestoID, true);
                DTO_glDocumentoControl ctrl = _bc.AdministrationModel.glDocumentoControl_GetByID(this._numeroDoc.Value);

                #region Periodo
                PeriodoFiscal peroidoFisc = (PeriodoFiscal)Enum.Parse(typeof(PeriodoFiscal), impuestoInfo.PeriodoDeclaracion.Value.Value.ToString());
                int _period = 1;
                bool _preInd = false;
                switch (peroidoFisc)
                {
                    case PeriodoFiscal.Anual:
                        _period = 1;
                        _preInd = (this._añoDeclaracion == DateTime.Now.Year) ? true : false;
                        break;
                    case PeriodoFiscal.Bimestral:
                        _period = (int)(this._mesDeclaracion / 2) + this._mesDeclaracion % 2;
                        _preInd = (this._añoDeclaracion == DateTime.Now.Year && _period * 2 >= DateTime.Now.Month) ? true : false;
                        break;
                    case PeriodoFiscal.Mensual:
                        _period = this._mesDeclaracion;
                        _preInd = (this._añoDeclaracion == DateTime.Now.Year && _period >= DateTime.Now.Month) ? true : false;
                        break;
                    case PeriodoFiscal.Semestral:
                        _period = (this._mesDeclaracion < 7) ? 1 : 2;
                        _preInd = (this._añoDeclaracion == DateTime.Now.Year && _period * 6 >= DateTime.Now.Month) ? true : false;
                        break;
                    case PeriodoFiscal.Trimestral:
                        _period = (this._mesDeclaracion % 3 != 0) ? (int)(this._mesDeclaracion / 3) + 1 : (int)(this._mesDeclaracion / 3);
                        _preInd = (this._añoDeclaracion == DateTime.Now.Year && _period * 3 >= DateTime.Now.Month) ? true : false;
                        break;
                }
                #endregion
                #endregion
                switch ((this.cmbReportType.SelectedItem as ComboBoxItem).Value)
                {
                    #region Declaracion
                    case "1":
                        DTO_Formularios formData = new DTO_Formularios();
                        #region Trae datos para llenar el cabezote
                   
                        
                        DTO_coTercero terceroInfo = (DTO_coTercero)this._bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, udtTercero, true);
                        DTO_coTerceroDocTipo terceroDocInfo = (DTO_coTerceroDocTipo)this._bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTerceroDocTipo, terceroInfo.TerceroDocTipoID, true);

                        formData.FormDecHeader = new DTO_FormDecHeader();
                        formData.FormDecHeader.Nit = terceroInfo.ID.Value;
                        formData.FormDecHeader.NitTipoDoc = terceroInfo.TerceroDocTipoID.Value;
                        if (terceroDocInfo.PersonaNaturalInd.Value.Value)
                        {
                            formData.FormDecHeader.RazonSoc = string.Empty;
                            formData.FormDecHeader.ApellidoPri = terceroInfo.ApellidoPri.Value;
                            formData.FormDecHeader.ApellidoSdo = terceroInfo.ApellidoSdo.Value;
                            formData.FormDecHeader.NombrePri = terceroInfo.NombrePri.Value;
                            formData.FormDecHeader.NombreOtr = terceroInfo.NombreSdo.Value;
                        }
                        else
                        {
                            formData.FormDecHeader.RazonSoc = terceroInfo.ApellidoPri.Value;
                            formData.FormDecHeader.ApellidoPri = string.Empty;
                            formData.FormDecHeader.ApellidoSdo = string.Empty;
                            formData.FormDecHeader.NombrePri = string.Empty;
                            formData.FormDecHeader.NombreOtr = string.Empty;
                        }
                        formData.FormDecHeader.DV = terceroInfo.DigitoVerif.Value;
                        formData.FormDecHeader.CodigoDir = string.Empty; //???
                        #endregion

                        #region IVA
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA))
                        {
                            #region Variables
                            formData.FormDecDetail = new List<DTO_FormDecDetail>();
                            DTO_FormDecDetail formItem;

                            decimal reng_32 = 0;
                            decimal reng_34 = 0;
                            decimal reng_39 = 0;
                            decimal reng_41 = 0;
                            decimal reng_51 = 0;
                            decimal reng_56 = 0;
                            decimal reng_57 = 0;
                            decimal reng_58 = 0;
                            decimal reng_59 = 0;
                            decimal reng_60 = 0;
                            decimal reng_62 = 0;
                            decimal reng_61 = 0;
                            decimal reng_63 = 0;
                            decimal reng_64 = 0;
                            #endregion

                            foreach (DTO_coImpDeclaracionDetaRenglon renglon in data)
                            {
                                formItem = new DTO_FormDecDetail();
                                formItem.Declaracion = this._impuestoID;
                                formItem.Renglon = renglon.Renglon.Value;
                                formItem.ValorML = renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 27 && Convert.ToInt32(renglon.Renglon.Value) <= 31)
                                    reng_32 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) == 33)
                                    reng_34 += renglon.ValorAjustado.Value.Value * (-1);
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 35 && Convert.ToInt32(renglon.Renglon.Value) <= 38)
                                    reng_39 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) == 40)
                                    reng_41 += renglon.ValorAjustado.Value.Value * (-1);
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 42 && Convert.ToInt32(renglon.Renglon.Value) <= 50)
                                    reng_51 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 52 && Convert.ToInt32(renglon.Renglon.Value) <= 55)
                                    reng_56 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) == 59)
                                    reng_59 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) == 60)
                                    reng_60 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) == 62)
                                    reng_62 += renglon.ValorAjustado.Value.Value;

                                formData.FormDecDetail.Add(formItem);
                            };

                            #region Renglon 32
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "32";
                            formItem.ValorML = reng_32;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 34
                            reng_34 += reng_32;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "34";
                            formItem.ValorML = reng_34;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 39
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "39";
                            formItem.ValorML = reng_39;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 41
                            reng_41 += reng_39;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "41";
                            formItem.ValorML = reng_41;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 51
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "51";
                            formItem.ValorML = reng_51;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 56
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "56";
                            formItem.ValorML = reng_56;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 57
                            reng_57 = (reng_51 - reng_56 >= 0) ? reng_51 - reng_56 : 0;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "57";
                            formItem.ValorML = reng_57;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 58
                            reng_58 = (reng_56 - reng_51 >= 0) ? reng_56 - reng_51 : 0;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "58";
                            formItem.ValorML = reng_58;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 61
                            reng_61 = (reng_57 - reng_58 - reng_59 - reng_60 >= 0) ? reng_57 - reng_58 - reng_59 - reng_60 : 0;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "61";
                            formItem.ValorML = reng_61;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 63
                            reng_63 = (reng_57 - reng_58 - reng_59 - reng_60 + reng_62 >= 0) ? reng_57 - reng_58 - reng_59 - reng_60 + reng_62 : 0;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "63";
                            formItem.ValorML = reng_63;

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 64
                            reng_64 = (reng_58 + reng_59 + reng_60 - reng_57 - reng_62 >= 0) ? reng_58 + reng_59 + reng_60 - reng_57 - reng_62 : 0;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "64";
                            formItem.ValorML = reng_64;

                            formData.FormDecDetail.Add(formItem);
                            #endregion

                            FormularioIVA formIVA = new FormularioIVA(formData, this._añoDeclaracion, _period, _preInd);
                            formIVA.ShowPreview();
                        };
                        #endregion
                        #region RteFte
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente))
                        {
                            #region Variables
                            formData.FormDecDetail = new List<DTO_FormDecDetail>();
                            DTO_FormDecDetail formItem;

                            decimal reng_44 = 0;
                            decimal reng_48 = 0;
                            decimal reng_51 = 0;
                            decimal reng_52 = 0;
                            decimal reng_54 = 0;
                            #endregion

                            foreach (DTO_coImpDeclaracionDetaRenglon renglon in data)
                            {
                                formItem = new DTO_FormDecDetail();

                                formItem.Declaracion = this._impuestoID;
                                formItem.Renglon = renglon.Renglon.Value;
                                formItem.ValorML = renglon.ValorAjustado.Value.Value * (-1);
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 27 && Convert.ToInt32(renglon.Renglon.Value) <= 43)
                                    reng_44 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 45 && Convert.ToInt32(renglon.Renglon.Value) <= 47)
                                    reng_48 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) >= 49 && Convert.ToInt32(renglon.Renglon.Value) <= 50)
                                    reng_44 += renglon.ValorAjustado.Value.Value;
                                if (Convert.ToInt32(renglon.Renglon.Value) == 53)
                                    reng_54 += renglon.ValorAjustado.Value.Value;

                                formData.FormDecDetail.Add(formItem);
                            };

                            #region Renglon 44
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "44";
                            formItem.ValorML = reng_44 * (-1);

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 48
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "48";
                            formItem.ValorML = reng_48 * (-1);

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 51
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "51";
                            formItem.ValorML = reng_51 * (-1);

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 52
                            reng_52 = reng_44 + reng_48 + reng_51;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "52";
                            formItem.ValorML = reng_52 * (-1);

                            formData.FormDecDetail.Add(formItem);
                            #endregion
                            #region Renglon 54
                            reng_54 += reng_52;
                            formItem = new DTO_FormDecDetail();
                            formItem.Declaracion = this._impuestoID;
                            formItem.Renglon = "54";
                            formItem.ValorML = reng_54 * (-1);

                            formData.FormDecDetail.Add(formItem);
                            #endregion

                            FormularioRTEFTE formRTEFTE = new FormularioRTEFTE(formData, this._añoDeclaracion, _period, _preInd);
                            formRTEFTE.ShowPreview();
                        };
                        #endregion
                        #region RICA
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA))
                        {
                            #region Variables
                            formData.FormDecDetail = new List<DTO_FormDecDetail>();
                            DTO_FormDecDetail formItem;
                            #endregion

                            foreach (DTO_coImpDeclaracionDetaRenglon renglon in data)
                            {
                                formItem = new DTO_FormDecDetail();
                                formItem.Declaracion = this._impuestoID;
                                formItem.Renglon = renglon.Renglon.Value;
                                formItem.ValorML = renglon.ValorAjustado.Value.Value * (-1);

                                formData.FormDecDetail.Add(formItem);
                            };

                            FormularioRTEICA formRICA = new FormularioRTEICA(formData, this._añoDeclaracion, _period);
                            formRICA.ShowPreview();
                        };
                        #endregion

                        break;
                    #endregion
                    #region Soporte
                    case "2":
                        List<DTO_FormulariosDetail> formDetData = new List<DTO_FormulariosDetail>();

                        foreach (DTO_coImpDeclaracionDetaRenglon renglon in data)
                        {
                            List<DTO_DetalleRenglon> cuentaDet = _bc.AdministrationModel.DetallesRenglon_Get(this._impuestoID, renglon.Renglon.Value, this._mesDeclaracion, this._añoDeclaracion);
                            foreach (DTO_DetalleRenglon cuentaDetItem in cuentaDet)
                            {
                                DTO_FormulariosDetail formDetItem = new DTO_FormulariosDetail();
                                formDetItem.Declaracion = cuentaDetItem.ImpuestoDeclID.Value;
                                formDetItem.Renglon = cuentaDetItem.Renglon.Value;
                                formDetItem.RenglonDesc = renglon.Descriptivo.Value;
                                formDetItem.CuentaID = cuentaDetItem.CuentaID.Value;
                                #region Obtener Descriptivo de la cuenta
                                if (cacheCuenta.ContainsKey(cuentaDetItem.CuentaID.Value))
                                    _cuentaInfo = cacheCuenta[cuentaDetItem.CuentaID.Value];
                                else
                                {
                                    _cuentaInfo = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = cuentaDetItem.CuentaID.Value }, true);
                                    cacheCuenta.Add(cuentaDetItem.CuentaID.Value, _cuentaInfo);
                                }
                                #endregion
                                formDetItem.CuentaDesc = _cuentaInfo.Descriptivo.Value;
                                formDetItem.BaseML = cuentaDetItem.VlrBaseML.Value.Value * (-1);
                                formDetItem.ValorML = cuentaDetItem.VlrMdaLoc.Value.Value * (-1);
                                formDetItem.ComprobanteID = cuentaDetItem.ComprobanteID.Value;
                                formDetItem.ComprobanteNro = cuentaDetItem.ComprobanteNro.Value.Value.ToString();
                                #region Obtener Documento
                                if (cacheConcSaldo.ContainsKey(_cuentaInfo.ConceptoSaldoID.Value))
                                    _saldoControl = cacheConcSaldo[_cuentaInfo.ConceptoSaldoID.Value];
                                else
                                {
                                    DTO_glConceptoSaldo concSaldoInfo = (DTO_glConceptoSaldo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glConceptoSaldo, new UDT_BasicID() { Value = _cuentaInfo.ConceptoSaldoID.Value }, true);
                                    _saldoControl = concSaldoInfo.coSaldoControl.Value.Value;
                                    cacheConcSaldo.Add(_cuentaInfo.ConceptoSaldoID.Value, _saldoControl);
                                }
                                string docID;
                                switch (_saldoControl)
                                {
                                    case (int)SaldoControl.Doc_Interno:
                                        docID = ctrl.PrefijoID.Value + "  " + ctrl.DocumentoNro.Value.Value.ToString();
                                        break;
                                    case (int)SaldoControl.Doc_Externo:
                                        docID = ctrl.DocumentoTercero.Value;
                                        break;
                                    default:
                                        docID = " - ";
                                        break;
                                };
                                #endregion
                                formDetItem.DocumentoID = docID;
                                formDetItem.Percent = (formDetItem.BaseML != 0) ? (Math.Round((formDetItem.ValorML / formDetItem.BaseML) * 100, 2)).ToString() : "*";
                                formDetItem.TerceroID = cuentaDetItem.TerceroID.Value;
                                #region Obtener Descriptivo del tercero
                                if (cacheTerc.ContainsKey(cuentaDetItem.TerceroID.Value))
                                    _tercDesc = cacheTerc[cuentaDetItem.TerceroID.Value];
                                else
                                {
                                    DTO_coTercero tercInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = cuentaDetItem.TerceroID.Value }, true);
                                    _tercDesc = tercInfo.Descriptivo.Value;
                                    cacheTerc.Add(cuentaDetItem.TerceroID.Value, _tercDesc);
                                }
                                #endregion
                                formDetItem.TerceroDesc = _tercDesc;
                                formDetItem.ReportRompimiento1 = new NewAge.DTO.Reportes.DTO_BasicReport.Rompimiento();
                                formDetItem.ReportRompimiento1.GroupFieldName = "Renglon";
                                formDetItem.ReportRompimiento1.GroupFieldValue = cuentaDetItem.Renglon.Value;
                                formDetItem.ReportRompimiento1.GroupFieldDesc = renglon.Descriptivo.Value;
                                formDetItem.ReportRompimiento2 = new NewAge.DTO.Reportes.DTO_BasicReport.Rompimiento();
                                formDetItem.ReportRompimiento2.GroupFieldName = "CuentaID";
                                formDetItem.ReportRompimiento2.GroupFieldValue = cuentaDetItem.CuentaID.Value;
                                formDetItem.ReportRompimiento2.GroupFieldDesc = _cuentaInfo.Descriptivo.Value;
                                formDetData.Add(formDetItem);
                            }
                        };

                        formDetData = formDetData.OrderBy(x => x.TerceroDesc).ToList();
                        formDetData = formDetData.OrderBy(x => x.TerceroID).ToList();
                        formDetData = formDetData.OrderBy(x => x.CuentaID).ToList();
                        formDetData = formDetData.OrderBy(x => x.Renglon).ToList();

                        ArrayList fieldListFD = new ArrayList();
                        fieldListFD.AddRange(ColumnsInfo.FormSoporteFields);

                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA))
                            declarType = 2;
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente))
                            declarType = 1;
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA))
                            declarType = 4;

                        FormulariosDetailReport formDet = new FormulariosDetailReport(formDetData, fieldListFD, this._añoDeclaracion, _period, _preInd, declarType.ToString());
                        formDet.ShowPreview();

                        break;
                    #endregion
                    #region por Cuenta
                    case "3":
                        List<DTO_FormulariosCuenta> formCuentaData = new List<DTO_FormulariosCuenta>();

                        foreach (DTO_coImpDeclaracionDetaRenglon renglon in data)
                        {
                            List<DTO_DetalleRenglon> cuentaDet = _bc.AdministrationModel.DetallesRenglon_Get(this._impuestoID, renglon.Renglon.Value, this._mesDeclaracion, this._añoDeclaracion);
                            foreach (DTO_DetalleRenglon cuentaDetItem in cuentaDet)
                            {
                                DTO_FormulariosCuenta formCuentaItem = new DTO_FormulariosCuenta();
                                formCuentaItem.Declaracion = cuentaDetItem.ImpuestoDeclID.Value;
                                formCuentaItem.Renglon = cuentaDetItem.Renglon.Value;
                                formCuentaItem.RenglonDesc = renglon.Descriptivo.Value;
                                formCuentaItem.CuentaID = cuentaDetItem.CuentaID.Value;
                                #region Obtener Descriptivo de la cuenta
                                if (cacheCuenta.ContainsKey(cuentaDetItem.CuentaID.Value))
                                    _cuentaInfo = cacheCuenta[cuentaDetItem.CuentaID.Value];
                                else
                                {
                                    _cuentaInfo = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = cuentaDetItem.CuentaID.Value }, true);
                                    cacheCuenta.Add(cuentaDetItem.CuentaID.Value, _cuentaInfo);
                                }
                                #endregion
                                formCuentaItem.CuentaDesc = _cuentaInfo.Descriptivo.Value;
                                formCuentaItem.ValorML = cuentaDetItem.VlrMdaLoc.Value.Value * (-1);
                                formCuentaItem.ReportRompimiento1 = new NewAge.DTO.Reportes.DTO_BasicReport.Rompimiento();
                                formCuentaItem.ReportRompimiento1.GroupFieldName = "Renglon";
                                formCuentaItem.ReportRompimiento1.GroupFieldValue = cuentaDetItem.Renglon.Value;
                                formCuentaItem.ReportRompimiento1.GroupFieldDesc = renglon.Descriptivo.Value;
                                formCuentaData.Add(formCuentaItem);
                            }
                        };
                        formCuentaData = formCuentaData.OrderBy(x => x.CuentaID).ToList();
                        formCuentaData = formCuentaData.OrderBy(x => x.Renglon).ToList();

                        ArrayList fieldListFC = new ArrayList();
                        fieldListFC.AddRange(ColumnsInfo.FormCuentaFields);

                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA))
                            declarType = 2;
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente))
                            declarType = 1;
                        if (impuestoInfo.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA))
                            declarType = 4;

                        FormulariosCuentaReport formCuenta = new FormulariosCuentaReport(formCuentaData, fieldListFC, this._añoDeclaracion, _period, _preInd, declarType.ToString());
                        formCuenta.ShowPreview();

                        break;
                    #endregion
                }
            }
            else
            {
                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NoImpProcessed);
                MessageBox.Show(msg);
            }
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRenglones_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor" || fieldName == "ValorAjustado")
                e.RepositoryItem = this.editSpin;
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRenglones_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                #region Trae datos
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                #endregion
            }
            if (e.IsSetData)
            {
                #region Asigna datos
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Funcion que se ejecuta al hacer doble click sobre la info de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRenglones_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                GridHitInfo info = view.CalcHitInfo(this.gcRenglones.PointToClient(MousePosition));
                if (info.HitTest != GridHitTest.Column)
                {
                    string msgTitleData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
                    string msgGetData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GetAccounts);

                    if (MessageBox.Show(msgGetData, msgTitleData, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (info.InRow || info.InRowCell)
                        {
                            DTO_coImpDeclaracionDetaRenglon renglon = this.data[info.RowHandle];
                            DeclaracionRenglonDetalle frmDet = new DeclaracionRenglonDetalle(this._impuestoID, renglon.Renglon.Value, this._añoDeclaracion, this._mesDeclaracion, this._numeroDoc);
                            frmDet.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
