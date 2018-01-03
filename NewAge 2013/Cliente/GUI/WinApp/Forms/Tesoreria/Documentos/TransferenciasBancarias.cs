using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using SentenceTransformer;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Reports;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class TransferenciasBancarias : FormWithToolbar
    {
        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            try
            {
                this._programaPagoList = _bc.AdministrationModel.ProgramacionPagos_GetProgramacionPagos();
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "SaveMethod"));
            }
        }

        #endregion

        #region Variables

        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private int _monOr;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private string _unboundPrefix = "Unbound_";
        private int _indexFila = 0;
        private bool _allowValidate = false;
        private List<DTO_ProgramacionPagos> _programaPagoList = new List<DTO_ProgramacionPagos>();
        private List<DTO_ProgramacionPagos> _pagosActuales = new List<DTO_ProgramacionPagos>();
        private DTO_ProgramacionPagos _rowPago = new DTO_ProgramacionPagos();
        private string _terceroActual = string.Empty;
        private DTO_tsBancosCuenta _bancoCuenta;
        private string _actFlujoID = string.Empty;
        private decimal _tc = 0;
        private bool _tcValid = false;
        private string reportName;
        private string fileURl;

        #endregion

        #region Propiedades

        /// <summary>
        /// Programacion de pagos sobre el cual se esta trabajando
        /// </summary>
        private DTO_ProgramacionPagos _pagos = null;
        protected virtual DTO_ProgramacionPagos ProgramaPagos
        {
            get { return this._pagos; }
            set { this._pagos = value; }
        }

        /// <summary>
        /// Numero de una fila segun el indice
        /// </summary>
        protected int NumFila
        {
            get
            {
                return this._programaPagoList.FindIndex(det => det.Index == this._indexFila);
            }
        }

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public TransferenciasBancarias()
        {
            try
            {
                this.InitializeComponent();

                this._documentID = AppDocuments.TransferenciasBancarias;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.ts;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Carga el listado de pagos
                string periodoCP = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.co_Periodo);
                string periodoTS = _bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);

                //Inicia los controles de usuario maestras
                _bc.InitMasterUC(this.masterCuenta, AppMasters.tsBancosCuenta, true, true, true, false);
                _bc.InitMasterUC(this.uc_Beneficiario, AppMasters.coTercero, false, true, true, false);

                //Carga info de las monedas
                this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                //Periodo
                _bc.InitPeriodUC(this.dtPeriod, 0);
                this.dtPeriod.DateTime = Convert.ToDateTime(periodoTS);
                int currentMonth = this.dtPeriod.DateTime.Month;
                int currentYear = this.dtPeriod.DateTime.Year;
                int minDay = 1;
                int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);

                DateTime fecha = DateTime.Now;
                if (fecha > this.dtPeriod.DateTime)
                {
                    int day = DateTime.DaysInMonth(currentYear, currentMonth);
                    fecha = new DateTime(currentYear, currentMonth, day);
                }
                this.dtFecha.DateTime = fecha;

                //Carga las columnas de la grilla
                this.AddGridCols();
                //Carga la información de la grilla
                this.saveDelegate = new Save(this.SaveMethod);

                //Carga la info inicial
                if (periodoCP == periodoTS)
                {
                    this._programaPagoList = _bc.AdministrationModel.ProgramacionPagos_GetProgramacionPagos();
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_InvalidModsProgPagos));
                    this._programaPagoList = new List<DTO_ProgramacionPagos>();
                }

                //Llena los combos
                TablesResources.GetTableResources(this.cmbMonedaOrigen, typeof(TipoMoneda_LocExt));
                if (!_bc.AdministrationModel.MultiMoneda)
                    this.cmbMonedaOrigen.Enabled = false;

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    #region Carga la info de la prioxima actividad
                    List<string> NextActs = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                    if (NextActs.Count != 1)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                        MessageBox.Show(string.Format(msg, AppDocuments.EnvioSolicitudLibranza.ToString()));
                    }
                    else
                    {
                        this._actFlujoID = actividades[0];
                    }
                    #endregion

                }
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadData()
        {
            try
            {
               // this._programaPagoList = _bc.AdministrationModel.ProgramacionPagos_GetProgramacionPagos();

                switch (this._monOr)
                {
                    case (int)TipoMoneda.Local:
                        this._pagosActuales = this._programaPagoList.FindAll(p => p.MonedaID.Value == this._monedaLocal && p.SaldoML.Value.Value != 0);
                        this.gvPagos.Columns[this._unboundPrefix + "SaldoML"].Visible = true;
                        this.gvPagos.Columns[this._unboundPrefix + "SaldoME"].Visible = false;
                        break;
                    case (int)TipoMoneda.Foreign:
                        this._pagosActuales = this._programaPagoList.FindAll(p => p.MonedaID.Value == this._monedaExtranjera && p.SaldoME.Value.Value != 0);
                        this.gvPagos.Columns[this._unboundPrefix + "SaldoME"].Visible = true;
                        this.gvPagos.Columns[this._unboundPrefix + "SaldoML"].Visible = false;
                        break;
                }
                this._pagosActuales = this._pagosActuales.FindAll(p => p.BancoCuentaID.Value == this.masterCuenta.Value || string.IsNullOrEmpty(p.BancoCuentaID.Value));

                if (this._pagosActuales.FindAll(p => !p.PagoInd.Value.HasValue || !(bool)p.PagoInd.Value).Count == 0)
                    this.chkSelectAll.CheckState = CheckState.Checked;
                else
                    this.chkSelectAll.CheckState = CheckState.Unchecked;

                this._bancoCuenta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuenta.Value, true);

                this.gcPagos.DataSource = this._pagosActuales;
                this.gcPagos.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "LoadData"));
            }

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Numero factura
                GridColumn nroFactura = new GridColumn();
                nroFactura.FieldName = this._unboundPrefix + "DocumentoTercero";
                nroFactura.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_NroFactura");
                nroFactura.UnboundType = UnboundColumnType.String;
                nroFactura.VisibleIndex = 0;
                nroFactura.Width = 100;
                nroFactura.Visible = true;
                nroFactura.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(nroFactura);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this._unboundPrefix + "TerceroID";
                tercero.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_TerceroID");
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 1;
                tercero.Width = 120;
                tercero.Visible = true;
                tercero.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(tercero);

                //NombreTercero
                GridColumn nombreTercero = new GridColumn();
                nombreTercero.FieldName = this._unboundPrefix + "Descriptivo";
                nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_NombreTercero");
                nombreTercero.UnboundType = UnboundColumnType.String;
                nombreTercero.VisibleIndex = 2;
                nombreTercero.Width = 180;
                nombreTercero.Visible = true;
                nombreTercero.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(nombreTercero);

                //Moneda
                GridColumn monedaFact = new GridColumn();
                monedaFact.FieldName = this._unboundPrefix + "MonedaID";
                monedaFact.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_MonedaID");
                monedaFact.UnboundType = UnboundColumnType.String;
                monedaFact.VisibleIndex = 3;
                monedaFact.Width = 100;
                monedaFact.Visible = true;
                monedaFact.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(monedaFact);

                //Valor Pendiente Moneda Local
                GridColumn valorPendienteML = new GridColumn();
                valorPendienteML.FieldName = this._unboundPrefix + "SaldoML";
                valorPendienteML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_ValorPendiente");
                valorPendienteML.UnboundType = UnboundColumnType.Decimal;
                valorPendienteML.VisibleIndex = 4;
                valorPendienteML.Width = 140;
                valorPendienteML.Visible = true;
                valorPendienteML.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(valorPendienteML);

                //Valor Pendiente Moneda Extranjera
                GridColumn valorPendienteME = new GridColumn();
                valorPendienteME.FieldName = this._unboundPrefix + "SaldoME";
                valorPendienteME.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_ValorPendiente");
                valorPendienteME.UnboundType = UnboundColumnType.Decimal;
                valorPendienteME.VisibleIndex = 4;
                valorPendienteME.Width = 140;
                valorPendienteME.Visible = false;
                valorPendienteME.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(valorPendienteME);

                //IndicadorPago
                GridColumn indicadorPago = new GridColumn();
                indicadorPago.FieldName = this._unboundPrefix + "PagoInd";
                indicadorPago.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_PagoInd");
                indicadorPago.UnboundType = UnboundColumnType.Boolean;
                indicadorPago.VisibleIndex = 5;
                indicadorPago.Width = 60;
                indicadorPago.Visible = true;
                indicadorPago.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(indicadorPago);

                //BancoCuenta
                GridColumn bancoCuenta = new GridColumn();
                bancoCuenta.FieldName = this._unboundPrefix + "BancoCuentaID";
                bancoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_BancoCuentaID");
                bancoCuenta.UnboundType = UnboundColumnType.String;
                bancoCuenta.VisibleIndex = 6;
                bancoCuenta.Width = 120;
                bancoCuenta.Visible = true;
                bancoCuenta.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(bancoCuenta);

                //ValorPago
                GridColumn valorPago = new GridColumn();
                valorPago.FieldName = this._unboundPrefix + "ValorPago";
                valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ProgramacionDesembolsos + "_ValorPago");
                valorPago.UnboundType = UnboundColumnType.Decimal;
                valorPago.VisibleIndex = 7;
                valorPago.Width = 140;
                valorPago.Visible = true;
                valorPago.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(valorPago);

                //beneficiario 
                GridColumn beneficiario = new GridColumn();
                beneficiario.FieldName = this._unboundPrefix + "Beneficiario";
                beneficiario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Beneficiario");
                beneficiario.UnboundType = UnboundColumnType.String;
                beneficiario.VisibleIndex = 8;
                beneficiario.Width = 180;
                beneficiario.Visible = false;
                beneficiario.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(beneficiario);

                //beneficiarioID 
                GridColumn beneficiarioID = new GridColumn();
                beneficiarioID.FieldName = this._unboundPrefix + "BeneficiarioID";
                beneficiarioID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BeneficiarioID ");
                beneficiarioID.UnboundType = UnboundColumnType.String;
                beneficiarioID.VisibleIndex = 9;
                beneficiarioID.Width = 180;
                beneficiarioID.Visible = true;
                beneficiarioID.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(beneficiarioID);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this._unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvPagos.Columns.Add(colIndex);

                //GridView vwPagos = (GridView)gcPagos.MainView;
                //vwPagos.SortInfo.Add(nombreTercero, ColumnSortOrder.Ascending);
                //vwPagos.SortInfo.Add(nroFactura, ColumnSortOrder.Ascending);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            this.IsModalFormOpened = true;
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                ModalMaster modal;
                if (fktable.Jerarquica.Value.Value)
                    modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                else
                    modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);

                modal.ShowDialog();
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        private bool ValidateDocRow(int fila)
        {
            try
            {
                string rsxRangeValue = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_ValorPagoRangeValue);
                GridColumn colVP = this.gvPagos.Columns[this._unboundPrefix + "ValorPago"];
                GridColumn colS = (_monOr == (int)TipoMoneda.Local) ? this.gvPagos.Columns[this._unboundPrefix + "SaldoML"] : this.gvPagos.Columns[this._unboundPrefix + "SaldoME"];
                decimal? dblValorPago = (decimal?)this.gvPagos.GetRowCellValue(fila, colVP);
                decimal? dblSaldo = (decimal?)this.gvPagos.GetRowCellValue(fila, colS);

                if (dblValorPago.HasValue && dblSaldo.HasValue && (dblValorPago > dblSaldo || dblValorPago <= 0))
                {
                    string msg = string.Format(rsxRangeValue, colVP.Caption);
                    this.gvPagos.SetColumnError(colVP, msg);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias.cs-ValidateDocRow"));
                return false;
            }
        }

        /// <summary>
        /// Actualiza la información del footer
        /// </summary>
        private void UpdateFooterData(DTO_ProgramacionPagos programacionPagos)
        {

            this.lblDate.Text = programacionPagos.Fecha.Value.HasValue ? programacionPagos.Fecha.Value.ToString() : string.Empty;
            this.lblComp.Text = programacionPagos.ComprobanteIdNro.Value.HasValue ? programacionPagos.ComprobanteIdNro.Value.ToString() : string.Empty;
            this.lblDetail.Text = programacionPagos.Observacion.Value;
            this.CalcularSumatorias();
        }

        /// <summary>
        /// Calcula las sumatorias
        /// </summary>
        private void CalcularSumatorias()
        {
            string tercero = this.NumFila > -1 ? this._rowPago.TerceroID.Value : string.Empty;
            this.txtSumatoriaTercero.EditValue = tercero != string.Empty ? this._pagosActuales.FindAll(p => p.TerceroID.Value == tercero).Sum(p => p.ValorPago.Value).Value : 0;

            string cuenta = this.NumFila > -1 ? this._rowPago.BancoCuentaID.Value : string.Empty;
            this.txtSumatoriaCuenta.EditValue = cuenta != string.Empty ? this._pagosActuales.FindAll(p => p.BancoCuentaID.Value == cuenta).Sum(p => p.ValorPago.Value).Value : 0;

            this.txtSumatoriaTotal.EditValue = this._pagosActuales.Sum(p => p.ValorPago.Value).Value;
        }

        /// <summary>
        /// Valida la info antes de salvar
        /// </summary>
        private bool ValidateHeader()
        {
            try
            {
                //Footer valido
                bool validateGrid = this.ValidateDocRow(this.gvPagos.FocusedRowHandle);
                if (!validateGrid)
                    return false;

                //Cuenta valida
                if (!this.masterCuenta.ValidID)
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                    MessageBox.Show(string.Format(msg, this.masterCuenta.LabelRsx));
                    return false;
                }

                //Tasa de cambio
                if (!this._tcValid)
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Ts_NoTasaCambioFechaActual));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "ValidateHeader"));
                return false;
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

                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;

                FormProvider.Master.itemUpdate.Enabled = true;
                if (FormProvider.Master.LoadFormTB)
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TransferenciasBancarias.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (_allowValidate && !this.ValidateDocRow(e.RowHandle))
            {
                e.Allow = false;
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "BancoCuentaID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "ValorPago" || fieldName == "SaldoML" || fieldName == "SaldoME")
            {
                e.RepositoryItem = this.editSpin;
            }
            if (fieldName == "PagoInd")
            {
                e.RepositoryItem = this.editCheck;
            }
        }

        /// <summary>
        /// Actualiza valores editables cuando se cambia estado de pago
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "PagoInd")
                {
                    GridColumn col = this.gvPagos.Columns[this._unboundPrefix + "Index"];
                    this._indexFila = Convert.ToInt32(this.gvPagos.GetRowCellValue(e.RowHandle, col));
                    this.UpdateFooterData(this._rowPago);

                    bool value = Convert.ToBoolean(e.Value);
                    int index = this.NumFila;
                    if (value)
                    {
                        if (this.masterCuenta.ValidID)
                        {
                            this._rowPago.PagoInd.Value = value;
                            this._rowPago.BancoCuentaID.Value = this.masterCuenta.Value;
                            this._rowPago.ValorPago.Value = (this._monOr == (int)TipoMoneda.Local) ? this._rowPago.SaldoML.Value : this._rowPago.SaldoME.Value;
                            if (this._pagosActuales.FindAll(p => !p.PagoInd.Value.HasValue || !(bool)p.PagoInd.Value).Count == 0)
                                this.chkSelectAll.CheckState = CheckState.Checked;
                        }
                        else
                        {
                            MessageBox.Show(this.masterCuenta.LabelRsx + " " + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField ));
                            this._rowPago.PagoInd.Value = false;
                            this.gvPagos.RefreshData();
                        }                           
                    }
                    else
                    {
                        this._rowPago.PagoInd.Value = value;
                        this._rowPago.BancoCuentaID.Value = string.Empty;
                        this._rowPago.ValorPago.Value = null;
                        this.chkSelectAll.CheckState = CheckState.Unchecked;
                    }

                    this.gcPagos.RefreshDataSource();

                }
                if (fieldName == "ValorPago")
                {
                    this._allowValidate = true;
                }
                this.CalcularSumatorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "gvPagos_CellValueChanging"));
            }
        }

        /// <summary>
        /// Actualiza los valores en la grilla despues del pago
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                GridColumn col = this.gvPagos.Columns[this._unboundPrefix + fieldName];
                if (fieldName == "ValorPago")
                {
                    this._allowValidate = true;

                    GridColumn colIndex = this.gvPagos.Columns[this._unboundPrefix + "Index"];
                    this._indexFila = Convert.ToInt32(this.gvPagos.GetRowCellValue(e.RowHandle, colIndex));

                    decimal val = this.gvPagos.GetRowCellValue(e.RowHandle, col) != null ? (decimal)this.gvPagos.GetRowCellValue(e.RowHandle, col) : 0;
                    this._rowPago.ValorPago.Value = val;
                }
                this.CalcularSumatorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "gvPagos_CellValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this._allowValidate = false;

            try
            {
                GridColumn col = this.gvPagos.Columns[this._unboundPrefix + "Index"];
                if (e.FocusedRowHandle >= 0  && e.FocusedRowHandle < gvPagos.RowCount)
                {
                    this._rowPago = (DTO_ProgramacionPagos)this.gvPagos.GetRow(e.FocusedRowHandle);
                    this._indexFila = Convert.ToInt32(this.gvPagos.GetRowCellValue(e.FocusedRowHandle, col));
                    this.UpdateFooterData(this._rowPago);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                Object dto = (Object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                e.Value = fi.GetValue(dto);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
                        }
                    }
                }
                if (e.IsSetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "gvPagos_CustomUnboundColumnData"));
            }
        }

        /// <summary>
        /// Evento para deshabilitar celdas que no están seleccionadas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                string fieldname = view.FocusedColumn.FieldName;
                int fila = view.FocusedRowHandle;
                GridColumn col = this.gvPagos.Columns[this._unboundPrefix + "PagoInd"];
                bool? seleccionado = (bool?)this.gvPagos.GetRowCellValue(fila, col);

                if ((fieldname == this._unboundPrefix + "BancoCuentaID" || fieldname == this._unboundPrefix + "ValorPago") && seleccionado.HasValue && !seleccionado.Value)
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "gvPagos_ShowingEditor"));
            }
        }

        /// <summary>
        /// Llama diálogo de bancos desde la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvPagos.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvPagos.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

        #region Eventos Footer

        /// <summary>
        /// Evento que habilita o deshabilita la columna de pagos para programar pagos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCuenta_Leave(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;

                    if (this.masterCuenta.ValidID && this._bancoCuenta == null) //Digita 1ra vez
                    {
                        this._bancoCuenta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuenta.Value, true);

                        //Valida el periodo de los modulos
                        string periodoCP = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.co_Periodo);
                        string periodoTS = _bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                        if (periodoCP == periodoTS)
                            this._programaPagoList = _bc.AdministrationModel.ProgramacionPagos_GetProgramacionPagos();
                        else
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_InvalidModsProgPagos));
                            this._programaPagoList = new List<DTO_ProgramacionPagos>();
                        }

                        if (!this._bc.AdministrationModel.MultiMoneda || this._tc > 0)
                        {
                            DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, true, this._bancoCuenta.coDocumentoID.Value, true);

                            if ((int)coDoc.MonedaOrigen.Value == (int)TipoMoneda_CoDocumento.Both || (int)coDoc.MonedaOrigen.Value == (int)TipoMoneda_CoDocumento.NA)
                            {
                                MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Ts_MonedaOrigenInvalida));
                                isValid = false;
                            }
                        }
                    }
                    else
                    {
                        //Valida si existen ya seleccionados
                        bool exist = this._bancoCuenta != null? this._pagosActuales.Any(x => x.BancoCuentaID.Value == this._bancoCuenta.ID.Value) : false;
                        if (exist && this._bancoCuenta.ID.Value != this.masterCuenta.Value)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_SeleccionCuentaBancaria);
                            if (MessageBox.Show(string.Format(msg, this._bancoCuenta.ID.Value), "Cambiar Cuenta Bancaria", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                //Resetea la Cuenta Bancaria
                                this._pagosActuales.ForEach(x =>
                                {
                                    x.PagoInd.Value = false;
                                    x.BancoCuentaID.Value = string.Empty;
                                    x.ValorPago.Value = 0;
                                });
                                this.gvPagos.RefreshData();
                            }
                            else
                                this.masterCuenta.Value = this._bancoCuenta.ID.Value;
                        }

                        if(!this.masterCuenta.ValidID)
                           isValid = false;
                    }

                #region Actualiza la info de la grilla

                int index = this.NumFila;
                if (isValid)
                {
                    this.LoadData();
                }
                else
                {                 
                    this.LoadData();

                    this.chkSelectAll.CheckState = CheckState.Unchecked;
                    this.chkSelectAll.Enabled = false;
                }
                this.gvPagos.FocusedRowHandle = index;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "masterCuenta_Leave"));
            }
        }

        /// <summary>
        /// Filtra los datos de la grilla segun el tipo de moneda (local / extranjera)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbMonedaOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
            this.LoadData();
        }

        /// <summary>
        /// Evento que se llama cuando se selecciona o desselecciona todo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkSelectAll_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                this._pagosActuales.ForEach(p =>
                {
                    bool value = this.chkSelectAll.Checked;
                    p.PagoInd.Value = value;
                    if (value)
                    {
                        p.PagoInd.Value = value;
                        p.BancoCuentaID.Value = this.masterCuenta.Value;
                        p.ValorPago.Value = (this._monOr == (int)TipoMoneda.Local) ? p.SaldoML.Value : p.SaldoME.Value;
                    }
                    else
                    {
                        p.PagoInd.Value = value;
                        p.BancoCuentaID.Value = string.Empty;
                        p.ValorPago.Value = null;
                    }
                });
                this.gcPagos.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "chkSelectAll_MouseClick"));
            }
        }

        /// <summary>
        /// Asigna el beneficiario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Beneficiario_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.uc_Beneficiario.Value))
                {
                    int index = this.gvPagos.FocusedRowHandle;
                    DTO_coTercero beneficiario = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.uc_Beneficiario.Value, true);
                    this._rowPago.TerceroID.Value = beneficiario.ID.Value;
                    this._rowPago.Descriptivo.Value = beneficiario.Descriptivo.Value;
                    this.LoadData();
                    this.gvPagos.FocusedRowHandle = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "uc_Beneficiario_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la fecha de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFecha_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_bc.AdministrationModel.MultiMoneda)
                {
                    string monedaExtranjeraID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    this._tc = _bc.AdministrationModel.TasaDeCambio_Get(monedaExtranjeraID, this.dtFecha.DateTime.Date);

                    if (this._tc == 0)
                    {
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Ts_NoTasaCambioFechaActual));
                        this._tcValid = false;
                    }
                    else
                        this._tcValid = true;
                }
                else
                {
                    this._tc = 0;
                    this._tcValid = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "dtFecha_EditValueChanged"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gcPagos.Focus();
                this.gvPagos.PostEditor();

                if (this.ValidateHeader() && this._pagosActuales.Where(x => x.PagoInd.Value.Value == true).Count() > 0)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                //Carga el listado de pagos
                string periodoCP = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.co_Periodo);
                string periodoTS = _bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                if (periodoCP == periodoTS)
                    this._programaPagoList = _bc.AdministrationModel.ProgramacionPagos_GetProgramacionPagos();
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_InvalidModsProgPagos));
                    this._programaPagoList = new List<DTO_ProgramacionPagos>();
                }

                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton para imprimir la lista de documentos
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                ProgramacionPagosReportBuilder reportBuilder = new ProgramacionPagosReportBuilder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "TBPrint"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public void SaveThread()
        {
            try
            {
                var programacionesPagos = this._pagosActuales.FindAll(p => p.PagoInd.Value.HasValue && p.PagoInd.Value.Value);

                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                List<DTO_TxResult> results = _bc.AdministrationModel.TransferenciasBancarias_Transferencias(this._documentID, this._actFlujoID, programacionesPagos, this.dtFecha.DateTime.Date, this._tc);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                if(results.Any(x=>x.Result == ResultValue.NOK))
                {
                    MessageForm frm = new MessageForm(results.Find(x => x.Result == ResultValue.NOK));
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    result.Result = ResultValue.NOK;
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }

                #region Genera reportes para el pago de factura
                if (result.Result == ResultValue.OK)
                {
                    foreach (DTO_TxResult r in results)
                    {
                        int docReporte = !string.IsNullOrEmpty(this._bancoCuenta.DocumentoID.Value) ? Convert.ToInt32(this._bancoCuenta.DocumentoID.Value) : this._documentID;
                        reportName = this._bc.AdministrationModel.ReportesTesoreria_PagosFactura(docReporte, Convert.ToInt32(r.ExtraField), ExportFormatType.pdf, true);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(r.ExtraField), null, reportName.ToString());
                        Process.Start(fileURl);
                    }
                    this.Invoke(this.saveDelegate);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransferenciasBancarias.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }

        }

        #endregion

    }
}
