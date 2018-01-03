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
using NewAge.Cliente.GUI.WinApp.Reports;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ProgramacionPagos : FormWithToolbar
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "SaveMethod"));
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
        private string _terceroActual = string.Empty;

        private string _actFlujoID = string.Empty;

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
        public ProgramacionPagos()
        {
            try
            {
                this.InitializeComponent();

                this._documentID = AppDocuments.ProgramacionDesembolsos;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.ts;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

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

                //Inicia los controles de usuario maestras
                _bc.InitMasterUC(this.masterCuenta, AppMasters.tsBancosCuenta, true, true, true, false);
                _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
                _bc.InitMasterUC(this.uc_Beneficiario, AppMasters.coTercero, false, true, true, false);

                //Carga info de las monedas
                this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                _bc.InitPeriodUC(this.dtPeriod, 0);
                this.dtPeriod.DateTime = Convert.ToDateTime(periodoTS);

                //Carga las columnas de la grilla
                this.AddGridCols();
                //Carga la información de la grilla
                this.saveDelegate = new Save(this.SaveMethod);

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
                    #region Carga la info de la proxima actividad
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos"));
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
                if (this.masterTercero.Value != string.Empty)
                    this._pagosActuales = this._pagosActuales.FindAll(p => p.TerceroID.Value == this.masterTercero.Value);

                if (this._pagosActuales.FindAll(p => !p.PagoInd.Value.HasValue || !(bool)p.PagoInd.Value).Count == 0)
                    this.chkSelectAll.CheckState = CheckState.Checked;
                else
                    this.chkSelectAll.CheckState = CheckState.Unchecked;


                this.gcPagos.DataSource = this._pagosActuales;
                this.gcPagos.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "LoadData"));
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
                nroFactura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NroFactura");
                nroFactura.UnboundType = UnboundColumnType.String;
                nroFactura.VisibleIndex = 0;
                nroFactura.Width = 100;
                nroFactura.Visible = true;
                nroFactura.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(nroFactura);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this._unboundPrefix + "TerceroID";
                tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 1;
                tercero.Width = 120;
                tercero.Visible = true;
                tercero.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(tercero);

                //NombreTercero
                GridColumn nombreTercero = new GridColumn();
                nombreTercero.FieldName = this._unboundPrefix + "Descriptivo";
                nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NombreTercero");
                nombreTercero.UnboundType = UnboundColumnType.String;
                nombreTercero.VisibleIndex = 2;
                nombreTercero.Width = 180;
                nombreTercero.Visible = true;
                nombreTercero.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(nombreTercero);

                //Moneda
                GridColumn monedaFact = new GridColumn();
                monedaFact.FieldName = this._unboundPrefix + "MonedaID";
                monedaFact.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MonedaID");
                monedaFact.UnboundType = UnboundColumnType.String;
                monedaFact.VisibleIndex = 3;
                monedaFact.Width = 100;
                monedaFact.Visible = true;
                monedaFact.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(monedaFact);

                //Valor Pendiente Moneda Local
                GridColumn valorPendienteML = new GridColumn();
                valorPendienteML.FieldName = this._unboundPrefix + "SaldoML";
                valorPendienteML.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPendiente");
                valorPendienteML.UnboundType = UnboundColumnType.Decimal;
                valorPendienteML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorPendienteML.AppearanceCell.Options.UseTextOptions = true;
                valorPendienteML.VisibleIndex = 4;
                valorPendienteML.Width = 140;
                valorPendienteML.Visible = true;
                valorPendienteML.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(valorPendienteML);

                //Valor Pendiente Moneda Extranjera
                GridColumn valorPendienteME = new GridColumn();
                valorPendienteME.FieldName = this._unboundPrefix + "SaldoME";
                valorPendienteME.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPendiente");
                valorPendienteME.UnboundType = UnboundColumnType.Decimal;
                valorPendienteME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorPendienteME.AppearanceCell.Options.UseTextOptions = true;
                valorPendienteME.VisibleIndex = 4;
                valorPendienteME.Width = 140;
                valorPendienteME.Visible = false;
                valorPendienteME.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(valorPendienteME);

                //IndicadorPago
                GridColumn indicadorPago = new GridColumn();
                indicadorPago.FieldName = this._unboundPrefix + "PagoInd";
                indicadorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PagoInd");
                indicadorPago.UnboundType = UnboundColumnType.Boolean;
                indicadorPago.VisibleIndex = 5;
                indicadorPago.Width = 60;
                indicadorPago.Visible = true;
                indicadorPago.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(indicadorPago);

                //BancoCuenta
                GridColumn bancoCuenta = new GridColumn();
                bancoCuenta.FieldName = this._unboundPrefix + "BancoCuentaID";
                bancoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BancoCuentaID");
                bancoCuenta.UnboundType = UnboundColumnType.String;
                bancoCuenta.VisibleIndex = 6;
                bancoCuenta.Width = 120;
                bancoCuenta.Visible = true;
                bancoCuenta.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(bancoCuenta);

                //ValorPago
                GridColumn valorPago = new GridColumn();
                valorPago.FieldName = this._unboundPrefix + "ValorPago";
                valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPago");
                valorPago.UnboundType = UnboundColumnType.Decimal;
                valorPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorPago.AppearanceCell.Options.UseTextOptions = true;
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
                beneficiario.Visible = true;
                beneficiario.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(beneficiario);

                //beneficiarioID 
                GridColumn beneficiarioID = new GridColumn();
                beneficiarioID.FieldName = this._unboundPrefix + "BeneficiarioID";
                beneficiarioID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BeneficiarioID ");
                beneficiarioID.UnboundType = UnboundColumnType.String;
                beneficiarioID.VisibleIndex = 8;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos.cs-AddGridCols"));
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
                DTO_ProgramacionPagos row = (DTO_ProgramacionPagos)this.gvPagos.GetRow(fila);
                string rsxRangeValue = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_ValorPagoRangeValue);
                GridColumn colVP = this.gvPagos.Columns[this._unboundPrefix + "ValorPago"];
                GridColumn colS = (_monOr == (int)TipoMoneda.Local) ? this.gvPagos.Columns[this._unboundPrefix + "SaldoML"] : this.gvPagos.Columns[this._unboundPrefix + "SaldoME"];
                decimal? dblValorPago = (decimal?)this.gvPagos.GetRowCellValue(fila, colVP);
                decimal? dblSaldo = (decimal?)this.gvPagos.GetRowCellValue(fila, colS);

                if (row != null)
                {
                    if (row.DocumentoID.Value != AppDocuments.NotaCreditoCxP && dblValorPago.HasValue && dblSaldo.HasValue && (dblValorPago > dblSaldo || dblValorPago <= 0))
                    {
                        string msg = string.Format(rsxRangeValue, colVP.Caption);
                        this.gvPagos.SetColumnError(colVP, msg);
                        return false;
                    }                   
                }

                bool invalidBanco = this._programaPagoList.Any(r => r.PagoInd.Value.HasValue && r.PagoInd.Value.Value && string.IsNullOrEmpty(r.BancoCuentaID.Value));
                if (invalidBanco)
                {
                    MessageBox.Show("No puede programar sin asignar un banco, verifique nuevamente");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos.cs-ValidateDocRow"));
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
            
            this.txtDetail.ReadOnly = true;
            if (Convert.ToBoolean(programacionPagos.PagoInd.Value.Value))
                this.txtDetail.ReadOnly=false;

            this.txtDetail.Text = programacionPagos.Observacion.Value;
            this.uc_Beneficiario.Value = programacionPagos.BeneficiarioID.Value;
            
            this.CalcularSumatorias();
        }

        /// <summary>
        /// Calcula las sumatorias
        /// </summary>
        private void CalcularSumatorias()
        {
            string tercero = this.NumFila > -1 ? this._programaPagoList.Find(p => p.Index == this.NumFila).TerceroID.Value : string.Empty;
            txtSumatoriaTercero.EditValue = tercero != string.Empty ? this._pagosActuales.FindAll(p => p.TerceroID.Value == tercero).Sum(p => p.ValorPago.Value).Value : 0;

            string cuenta = this.NumFila > -1 ? this._programaPagoList.Find(p => p.Index == this.NumFila).BancoCuentaID.Value : string.Empty;
            txtSumatoriaCuenta.EditValue = cuenta != string.Empty ? this._pagosActuales.FindAll(p => p.BancoCuentaID.Value == cuenta).Sum(p => p.ValorPago.Value).Value : 0;

            txtSumatoriaTotal.EditValue = this._pagosActuales.Sum(p => p.ValorPago.Value).Value;
        }

        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidGrid()
        {
            if (!this.ValidateDocRow(this.gvPagos.FocusedRowHandle))
                return false;

            return true;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "ProgramacionPagos.cs-Form_FormClosed"));
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
            if (!this.gvPagos.IsFilterRow(e.RowHandle))
            {
                if (_allowValidate && !this.ValidateDocRow(e.RowHandle))
                {
                    e.Allow = false;
                } 
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

                if (!this.gvPagos.IsFilterRow(e.RowHandle))
                {
                    if (fieldName == "PagoInd")
                    {
                        bool value = Convert.ToBoolean(e.Value);
                        int index = this.NumFila;
                        if (value)
                        {
                            this._programaPagoList[index].PagoInd.Value = value;
                            this._programaPagoList[index].BancoCuentaID.Value = this.masterCuenta.Value;
                            
                            this.txtDetail.ReadOnly = false;
                            
                            this._programaPagoList[index].ValorPago.Value = (this._monOr == (int)TipoMoneda.Local) ? this._programaPagoList[index].SaldoML.Value : this._programaPagoList[index].SaldoME.Value;
                            if (this._pagosActuales.FindAll(p => !p.PagoInd.Value.HasValue || !(bool)p.PagoInd.Value).Count == 0)
                                this.chkSelectAll.CheckState = CheckState.Checked;
                        }
                        else
                        {
                            this._programaPagoList[index].PagoInd.Value = value;
                            this._programaPagoList[index].BancoCuentaID.Value = string.Empty;
                            this._programaPagoList[index].ValorPago.Value = null;
                            this.chkSelectAll.CheckState = CheckState.Unchecked;
                            this.txtDetail.ReadOnly = true;

                        }

                        this.gcPagos.RefreshDataSource();

                    }
                    if (fieldName == "ValorPago")
                    {
                        this._allowValidate = true;
                    }
                    this.CalcularSumatorias(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "gvPagos_CellValueChanging"));
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
                if (!this.gvPagos.IsFilterRow(e.RowHandle))
                {
                    string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                    GridColumn col = this.gvPagos.Columns[this._unboundPrefix + fieldName];
                    if (fieldName == "ValorPago")
                    {
                        this._allowValidate = true;
                        decimal val = this.gvPagos.GetRowCellValue(e.RowHandle, col) != null ? (decimal)this.gvPagos.GetRowCellValue(e.RowHandle, col) : 0;
                        this._programaPagoList[e.RowHandle].ValorPago.Value = val;
                    }
                    this.CalcularSumatorias(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "gvPagos_CellValueChanged"));
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
                if (!this.gvPagos.IsFilterRow(e.FocusedRowHandle))
                {
                    GridColumn col = this.gvPagos.Columns[this._unboundPrefix + "Index"];
                    if (0 <= e.FocusedRowHandle && e.FocusedRowHandle < gvPagos.RowCount)
                    {
                        this._indexFila = Convert.ToInt16(this.gvPagos.GetRowCellValue(e.FocusedRowHandle, col));
                        var programacionPagos = this._pagosActuales.Find(p => p.Index == this._indexFila);
                        this.UpdateFooterData(programacionPagos);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "gvDocument_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "gvPagos_CustomUnboundColumnData"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "gvPagos_ShowingEditor"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "editBtnGrid_ButtonClick"));
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
                if (this.masterCuenta.Value != string.Empty)
                {
                    int index = this.gvPagos.FocusedRowHandle;
                    this.gvPagos.Columns[this._unboundPrefix + "PagoInd"].OptionsColumn.AllowEdit = true;
                    this.gvPagos.Columns[this._unboundPrefix + "BancoCuentaID"].OptionsColumn.AllowEdit = true;
                    this.gvPagos.Columns[this._unboundPrefix + "ValorPago"].OptionsColumn.AllowEdit = true;

                    this.chkSelectAll.Enabled = true;
                    this._programaPagoList[index].PagoInd.Value = true;
                    this._programaPagoList[index].BancoCuentaID.Value = this.masterCuenta.Value;
                    if (string.IsNullOrWhiteSpace(this._programaPagoList[index].ValorPago.Value.ToString()))
                        this._programaPagoList[index].ValorPago.Value = (this._monOr == (int)TipoMoneda.Local) ?
                            this._programaPagoList[index].SaldoML.Value : this._programaPagoList[index].SaldoME.Value;

                    this.LoadData();
                    this.gvPagos.FocusedRowHandle = index;
                }
                else
                {
                    this.gvPagos.Columns[this._unboundPrefix + "PagoInd"].OptionsColumn.AllowEdit = false;
                    this.gvPagos.Columns[this._unboundPrefix + "BancoCuentaID"].OptionsColumn.AllowEdit = false;
                    this.gvPagos.Columns[this._unboundPrefix + "ValorPago"].OptionsColumn.AllowEdit = false;
                    this.chkSelectAll.CheckState = CheckState.Unchecked;
                    this.chkSelectAll.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "masterCuenta_Leave"));
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
        /// Evento que se llama para refrescar la grilla con los nuevos valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterTercero_Leave(object sender, EventArgs e)
        {
            LoadData();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "chkSelectAll_MouseClick"));
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
                    this.gvPagos.Columns[this._unboundPrefix + "BeneficiarioID"].OptionsColumn.AllowEdit = true;
                    int index = this.gvPagos.FocusedRowHandle;
                    DTO_coTercero beneficiario = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.uc_Beneficiario.Value, true);
                    //this._programaPagoList[index].TerceroID.Value = beneficiario.ID.Value;
                    //this._programaPagoList[index].Descriptivo.Value = beneficiario.Descriptivo.Value;
                    this._programaPagoList[index].BeneficiarioID.Value = beneficiario.ID.Value;
                    this._programaPagoList[index].BeneficiarioID.Value = this.uc_Beneficiario.Value;
                    this._programaPagoList[index].Beneficiario.Value = beneficiario.Descriptivo.Value;
                    this.LoadData();
                    this.gvPagos.FocusedRowHandle = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "uc_Beneficiario_Leave"));
            }
        }

        private void txtDetail_Leave(object sender, EventArgs e)
        {
            try
            {
                int index = this.gvPagos.FocusedRowHandle;
                this._programaPagoList[index].Observacion.Value = this.txtDetail.Text;
                this.LoadData();
                this.gvPagos.FocusedRowHandle = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "txtDetail_Leave"));
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

                if (ValidGrid())
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "TBSave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "TBUpdate"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "TBPrint"));
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
                var strPagoAprobacionInd = _bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_IndicadorPagosAprobacion);
                bool pagoAprobacionInd = false;

                if (strPagoAprobacionInd == "1")
                    pagoAprobacionInd = true;

                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                result = _bc.AdministrationModel.ProgramacionPagos_ProgramarPagos(this._documentID, this._actFlujoID, this._programaPagoList, pagoAprobacionInd);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (result.Result == ResultValue.OK)
                    this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProgramacionPagos.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }

        }

        #endregion


    }
}
