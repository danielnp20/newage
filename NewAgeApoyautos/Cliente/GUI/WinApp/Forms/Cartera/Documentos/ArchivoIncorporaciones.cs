using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Views.Base;
using System.Diagnostics;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ArchivoIncorporaciones : FormWithToolbar
    {
        #region Variables
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //Variables Privadas
        private bool firstTime = true;
        private string centroPagoID = string.Empty;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query; 
        private string _unboundPrefix = "Unbound_";
        private string _frmName;

        //DTO
        private List<DTO_ccArchivoIncorporaciones> incorporaciones = new List<DTO_ccArchivoIncorporaciones>();
        private List<DTO_ccArchivoIncorporaciones> incorporacionesFilter = new List<DTO_ccArchivoIncorporaciones>();
        private DTO_ccCentroPagoPAG centroPago = new DTO_ccCentroPagoPAG();
        private DTO_ccPagaduria pagaduria = new DTO_ccPagaduria();
        private DataTableOperations exportTableOp;
        private List<object> exportFields;

        #endregion

        public ArchivoIncorporaciones()
        {
            InitializeComponent();
            this.SetInitParameters();
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
            FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.ArchivosIncorporaciones;
            this._frmModule = ModulesPrefix.cc;

            //Carga los recursos del combo
            Dictionary<byte, string> tipos = new Dictionary<byte, string>();
            tipos[(byte)TipoIncorporaCarteraArchivos.Afiliaciones] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoIncorporacionArchivos_1);
            tipos[(byte)TipoIncorporaCarteraArchivos.Desafiliaciones] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoIncorporacionArchivos_2);
            tipos[(byte)TipoIncorporaCarteraArchivos.Incorporaciones] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoIncorporacionArchivos_3);
            tipos[(byte)TipoIncorporaCarteraArchivos.Desincorporaciones] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoIncorporacionArchivos_4);
            this.lkp_Tipo.Properties.DataSource = tipos;
            this.lkp_Tipo.EditValue = (byte)TipoIncorporaCartera.Afiliaciones;

            //Carga los recursos del combo
            Dictionary<byte, string> tiposExportar = new Dictionary<byte, string>();
            tiposExportar[1] = "xls";
            tiposExportar[2] = "txt";
            this.lkp_Exportar.Properties.DataSource = tiposExportar;
            this.lkp_Exportar.EditValue = 1;

            //Maestras
            this._bc.InitMasterUC(this.uc_MasterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);

            //Periodo y fecha
            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.dtPeriodo.DateTime = Convert.ToDateTime(periodo);
            //this.dtPeriodo.Enabled = true;

            int currentMonth = this.dtPeriodo.DateTime.Month;
            int currentYear = this.dtPeriodo.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);

            this.AddGridCols();
        }

        /// <summary>
        /// Funcion que limpia los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            //Variables
            this.centroPagoID = String.Empty;
            this.centroPago = null;
            this.pagaduria = null;
            this.exportTableOp = null;
            this.exportFields = new List<object>();

            //Listas
            this.incorporaciones = new List<DTO_ccArchivoIncorporaciones>();
            this.incorporacionesFilter = new List<DTO_ccArchivoIncorporaciones>();
            this.gcDocument.DataSource = this.incorporacionesFilter;

            //Header
            this.uc_MasterCentroPago.Value = String.Empty;
            this.lkp_Tipo.EditValue = (byte)TipoIncorporaCartera.Afiliaciones;
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Campo de Libranza
                GridColumn CodigoNovedad = new GridColumn();
                CodigoNovedad.FieldName = this._unboundPrefix + "CodigoNovedad";
                CodigoNovedad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodigoNovedad");
                CodigoNovedad.UnboundType = UnboundColumnType.Integer;
                CodigoNovedad.VisibleIndex = 0;
                CodigoNovedad.Width = 70;
                CodigoNovedad.Visible = true;
                CodigoNovedad.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(CodigoNovedad);

                //Campo de Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this._unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 1;
                Libranza.Width = 70;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Libranza);

                //Cliente Id
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this._unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 2;
                ClienteID.Width = 70;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ClienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this._unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 3;
                nombCliente.Width = 150;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombCliente);

                //Código Empleado
                GridColumn CodEmpleado = new GridColumn();
                CodEmpleado.FieldName = this._unboundPrefix + "CodEmpleado";
                CodEmpleado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodEmpleado");
                CodEmpleado.UnboundType = UnboundColumnType.String;
                CodEmpleado.VisibleIndex = 4;
                CodEmpleado.Width = 110;
                CodEmpleado.Visible = true;
                CodEmpleado.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(CodEmpleado);

                //Valor Incorpora
                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrIncorpora";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrIncorpora");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 5;
                VlrCuota.Width = 110;
                VlrCuota.Visible = true;
                VlrCuota.OptionsColumn.AllowEdit = false;
                VlrCuota.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrCuota);

                //Plazo
                GridColumn Plazo = new GridColumn();
                Plazo.FieldName = this._unboundPrefix + "Plazo";
                Plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                Plazo.UnboundType = UnboundColumnType.Decimal;
                Plazo.VisibleIndex = 6;
                Plazo.Width = 70;
                Plazo.Visible = true;
                Plazo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Plazo);

                //Fecha Afiliación
                GridColumn FechaAfilia = new GridColumn();
                FechaAfilia.FieldName = this._unboundPrefix + "FechaAfilia";
                FechaAfilia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaAfilia");
                FechaAfilia.UnboundType = UnboundColumnType.DateTime;
                FechaAfilia.VisibleIndex = 7;
                FechaAfilia.Width = 150;
                FechaAfilia.Visible = true;
                FechaAfilia.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(FechaAfilia);

                //Fecha Desafiliación
                GridColumn FechaDesafilia = new GridColumn();
                FechaDesafilia.FieldName = this._unboundPrefix + "FechaDesafilia";
                FechaDesafilia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDesafilia");
                FechaDesafilia.UnboundType = UnboundColumnType.DateTime;
                FechaDesafilia.VisibleIndex = 8;
                FechaDesafilia.Width = 150;
                FechaDesafilia.Visible = true;
                FechaDesafilia.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(FechaDesafilia);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga el cabezote con los documentos
        /// </summary>
        private void LoadDocuments()
        {
            try
            {
                this.incorporaciones = new List<DTO_ccArchivoIncorporaciones>();
                if (this.uc_MasterCentroPago.ValidID)
                {
                    this.incorporaciones = this._bc.AdministrationModel.GetArchivosIncorporacion(this.dtPeriodo.DateTime.Date, this.uc_MasterCentroPago.Value);
                    if (!this.firstTime && this.incorporaciones.Count == 0)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                        MessageBox.Show(msg);
                    }
                }

                this.lkp_EditValueChanged(null, null);
                this.firstTime = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncoporaciones.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga elas estructuras de la pagaduria y la info a exportar
        /// </summary>
        /// <param name="docEstruct">Codigo con la información de las estructuras y campos a exportar</param>
        private void LoadEstructuras(string docEstruct)
        {
            try
            {
                DTO_glDocMigracionEstructura estructura = (DTO_glDocMigracionEstructura)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocMigracionEstructura, false, docEstruct, true);
                if (estructura != null)
                {
                    DTO_glConsulta query = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "CodigoDoc",
                        OperadorFiltro = OperadorFiltro.Igual,
                        OperadorSentencia = OperadorSentencia.And,
                        ValorFiltro = docEstruct
                    });
                    query.Filtros = filtros;

                    long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.glDocMigracionCampo, query, true);
                    if (count == 0)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoMigracionDoc);
                        MessageBox.Show(string.Format(msg, docEstruct));
                    }
                    else
                    {
                        List<DTO_MasterComplex> fieldsObj = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glDocMigracionCampo, count, 1, query, true).ToList();

                        List<DTO_glDocMigracionCampo> columnasImportacion = fieldsObj.Cast<DTO_glDocMigracionCampo>().ToList();
                        this.exportFields = fieldsObj.Cast<object>().ToList();
                        this.exportTableOp = new DataTableOperations(estructura, this.exportFields);
                    }
                }
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoExportacionnDoc);
                    MessageBox.Show(string.Format(msg, docEstruct));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "TBExport"));
            }
        }
        
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);

                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemSearch.Visible = false;

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosCxP.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosCxP.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosCxP.cs", "Form_FormClosing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosCxP.cs", "Form_FormClosed"));
            }
        }
        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento al cmabiar de periodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.LoadDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "dtPeriodo_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que filtra los documentos de acuerdo a la pagaduria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_MasterCentroPago_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.centroPagoID != this.uc_MasterCentroPago.Value)
                {
                    this.centroPagoID = this.uc_MasterCentroPago.Value;
                    this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.uc_MasterCentroPago.Value, true);
                    if (this.centroPago != null)
                    {
                        this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                        this.lkp_Tipo.EditValue = (byte)TipoIncorporaCartera.Afiliaciones;
                        this.LoadDocuments();
                    }
                    else
                    {
                        string cp = this.uc_MasterCentroPago.Value;
                        this.CleanData();
                        this.uc_MasterCentroPago.Value = cp;
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.uc_MasterCentroPago.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "uc_MasterCentroPago_Leave"));
            }
        }

        /// <summary>
        /// Evento para cambiar el filtro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.exportTableOp = null;
                this.exportFields = new List<object>();
                string codExportacion = string.Empty;
                this.incorporacionesFilter = new List<DTO_ccArchivoIncorporaciones>();
                if (this.incorporaciones != null && this.incorporaciones.Count > 0)
                {
                    byte tipo = Convert.ToByte(this.lkp_Tipo.EditValue);
                    switch (tipo)
                    {
                        case (byte)TipoIncorporaCartera.Afiliaciones:
                            codExportacion = this.pagaduria.CodTransmisionAf.Value;
                            this.incorporacionesFilter = this.incorporaciones.Where(i => i.FechaAfilia.Value.HasValue
                                && i.FechaAfilia.Value.Value.Year == this.dtPeriodo.DateTime.Date.Year
                                && i.FechaAfilia.Value.Value.Month == this.dtPeriodo.DateTime.Date.Month).ToList();
                            this.incorporacionesFilter.ForEach(i => i.CodigoNovedad.Value = i.AfiliaIDE.Value);
                            
                            break;
                        case (byte)TipoIncorporaCartera.Desafiliaciones:
                            codExportacion = this.pagaduria.CodTransmisionDesaf.Value;
                            this.incorporacionesFilter = this.incorporaciones.Where(i => i.FechaDesafilia.Value.HasValue
                                && i.FechaDesafilia.Value.Value.Year == this.dtPeriodo.DateTime.Date.Year
                                && i.FechaDesafilia.Value.Value.Month == this.dtPeriodo.DateTime.Date.Month).ToList();
                            this.incorporacionesFilter.ForEach(i => i.CodigoNovedad.Value = i.DesafiliaIDE.Value);
                           
                            break;
                        case (byte)TipoIncorporaCarteraArchivos.Incorporaciones:
                            codExportacion = this.pagaduria.CodTransmisionInc.Value;
                            this.incorporacionesFilter = this.incorporaciones.Where(i => i.TipoNovedad.Value != (byte)TipoNovedad.Desincorpora).ToList();
                            this.incorporacionesFilter.ForEach(i => i.CodigoNovedad.Value = i.IncorporaIDE.Value);
                            
                            break;
                        case (byte)TipoIncorporaCarteraArchivos.Desincorporaciones:
                            codExportacion = this.pagaduria.CodTransmisionDesinc.Value;
                            this.incorporacionesFilter = this.incorporaciones.Where(i => i.TipoNovedad.Value == (byte)TipoNovedad.Desincorpora).ToList();
                            this.incorporacionesFilter.ForEach(i => i.CodigoNovedad.Value = i.DesincorporaIDE.Value);
                         
                            break;
                    }
                }

                this.gcDocument.DataSource = this.incorporacionesFilter;
                this.gvDocuments.RefreshData();

                if(this.incorporaciones != null && this.incorporaciones.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(codExportacion))
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PagaduriaNoCodExportar);
                        MessageBox.Show(string.Format(msg, this.pagaduria.ID.Value));
                    }
                    else
                        this.LoadEstructuras(codExportacion);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "lkp_Tipo_Leave"));
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);

            if (pi != null)
                e.Value = pi.GetValue(dto, null);

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
                this.uc_MasterCentroPago.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton que exporta archivos planos para las pagadurias
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadDocuments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton que exporta archivos planos para las pagadurias
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.exportTableOp != null)
                {
                    List<int> consecutivos = this.incorporacionesFilter.Select(i => i.Consecutivo.Value.Value).ToList();
                    _bc.AdministrationModel.UpdateIncorporacionFechaTransmite(this.dtFecha.DateTime, consecutivos);
                    DataTable table = this.exportTableOp.Convert_GenericListToDataTable(typeof(DTO_ccArchivoIncorporaciones), this.incorporacionesFilter);

                    byte tipo = Convert.ToByte(this.lkp_Exportar.EditValue);
                    this.Enabled = false;
                    if (tipo == 1)
                    {
                        DataTable formatedTable = this.exportTableOp.Export_DataToXls(table, this.exportFields);

                        ReportExcelBase frm = new ReportExcelBase(formatedTable, null, false);
                        frm.Show();
                    }
                    else
                    {
                        this.Enabled = false;
                        this.exportTableOp.Export_DataToTxt(false, table, this.exportFields, false, string.Empty);

                        DTO_TxResult result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.ResultMessage = string.Empty;

                        MessageForm frm = new MessageForm(result);
                        frm.Show();
                    }
                    this.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "TBExport"));
                this.Enabled = true;
            }
        }

        #endregion
    }
}
