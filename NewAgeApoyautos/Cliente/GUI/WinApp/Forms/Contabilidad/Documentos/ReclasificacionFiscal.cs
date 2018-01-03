using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de distribucion de comprobantes
    /// </summary>
    public partial class ReclasificacionFiscal : FormWithToolbar
    {
        #region Variables

        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        //Para manejo de propiedades
        private int documentID;
        private ModulesPrefix frmModule;
        //Recursos
        private string _cuentaRsx = string.Empty;
        private string _centroCostoRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string unboundPrefix = "Unbound_";
        //Datos
        private int consecutivo = 0;
        private DTO_coPlanCuenta cta;
        private DTO_glConceptoSaldo concSaldo;
        private Dictionary<string, bool> cacheCtas;
        private bool deleteOP = false;
        private decimal currentPorc = 0;
        private int indexFilaData = 0;
        private int indexFilaExcluye = 0;
        private string tipoBalance;
        private bool isValidOrigen;
        private bool isValidDestino;
        private bool isValidExcluye;
        private DTO_coReclasificaBalance _currentOrigen;
        private DTO_coReclasificaBalance _currentDestino;
        private DTO_coReclasificaBalExcluye _currentExcluye;
        private List<DTO_coReclasificaBalance> _data;
        private List<DTO_coReclasificaBalance> _dataOrigen;
        private List<DTO_coReclasificaBalance> _dataDestino;
        private List<DTO_coReclasificaBalExcluye> _dataExcluye;
        private List<DTO_coReclasificaBalExcluye> _dataExcluyeGrilla;

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        private bool IsModalFormOpened
        {
            get;
            set;
        }

        /// <summary>
        /// Numero de fila de origen/destino
        /// </summary>
        private int NumFilaData
        {
            get
            {
                return this._data.FindIndex(det => det.Index.Value.Value == this.indexFilaData);
            }
        }

        /// <summary>
        /// Numero de fila de excluye
        /// </summary>
        private int NumFilaExcluye
        {
            get
            {
                return this._dataExcluye.FindIndex(det => det.Index.Value.Value == this.indexFilaExcluye);
            }
        }

        #endregion

        public ReclasificacionFiscal()
        {
            InitializeComponent();

            this.documentID = AppDocuments.ReclasificacionFiscal;
            this.frmModule = ModulesPrefix.co;
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

            //Recursos genéricos
            this._cuentaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
            this._centroCostoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            this._proyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");

            #region Inicia el control de tipo de balance

            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            
            //Tipo balance IFRS
            filtros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "BalanceTipoID",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS),
                OperadorSentencia = "OR"
            });

            //Tipo balance fiscal
            filtros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "BalanceTipoID",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFiscal),
                OperadorSentencia = "OR"
            });

            _bc.InitMasterUC(this.masterBalanceTipo, AppMasters.coBalanceTipo, true, true, true, true, filtros);

            #endregion

            this.cacheCtas = new Dictionary<string, bool>();
            this.AddGridsCols();
            this.LoadData();
        }

        #region Funciones Privadas

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
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridsCols()
        {
            try
            {
                #region Origen

                //Cuenta Origen
                GridColumn cuentaOrig = new GridColumn();
                cuentaOrig.FieldName = this.unboundPrefix + "CuentaORIG";
                cuentaOrig.Caption = this._cuentaRsx;
                cuentaOrig.UnboundType = UnboundColumnType.String;
                cuentaOrig.VisibleIndex = 0;
                cuentaOrig.Width = 110;
                cuentaOrig.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(cuentaOrig);

                //Centro de costo Origen
                GridColumn ctoCostoOrig = new GridColumn();
                ctoCostoOrig.FieldName = this.unboundPrefix + "CtoCostoORIG";
                ctoCostoOrig.Caption = this._centroCostoRsx;
                ctoCostoOrig.UnboundType = UnboundColumnType.String;
                ctoCostoOrig.VisibleIndex = 1;
                ctoCostoOrig.Width = 110;
                ctoCostoOrig.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(ctoCostoOrig);

                //Proyecto Origen
                GridColumn proyectoOrig = new GridColumn();
                proyectoOrig.FieldName = this.unboundPrefix + "ProyectoORIG";
                proyectoOrig.Caption = this._proyectoRsx;
                proyectoOrig.UnboundType = UnboundColumnType.String;
                proyectoOrig.VisibleIndex = 2;
                proyectoOrig.Width = 110;
                proyectoOrig.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(proyectoOrig);

                //Orden
                GridColumn orden = new GridColumn();
                orden.FieldName = this.unboundPrefix + "Orden";
                orden.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Orden");
                orden.UnboundType = UnboundColumnType.Integer;
                orden.VisibleIndex = 3;
                orden.Width = 50;
                orden.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(orden);

                //Cuenta Contrapartida
                GridColumn cuentaCont = new GridColumn();
                cuentaCont.FieldName = this.unboundPrefix + "CuentaCONT";
                cuentaCont.Caption = this._cuentaRsx;
                cuentaCont.UnboundType = UnboundColumnType.String;
                cuentaCont.VisibleIndex = 4;
                cuentaCont.Width = 110;
                cuentaCont.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(cuentaCont);

                //Centro de costo Contrapartida
                GridColumn ctoCostoCont = new GridColumn();
                ctoCostoCont.FieldName = this.unboundPrefix + "CtoCostoCONT";
                ctoCostoCont.Caption = this._centroCostoRsx;
                ctoCostoCont.UnboundType = UnboundColumnType.String;
                ctoCostoCont.VisibleIndex = 5;
                ctoCostoCont.Width = 110;
                ctoCostoCont.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(ctoCostoCont);

                //Proyecto Contrapartida
                GridColumn proyectoCont = new GridColumn();
                proyectoCont.FieldName = this.unboundPrefix + "ProyectoCONT";
                proyectoCont.Caption = this._proyectoRsx;
                proyectoCont.UnboundType = UnboundColumnType.String;
                proyectoCont.VisibleIndex = 6;
                proyectoCont.Width = 110;
                proyectoCont.OptionsColumn.AllowEdit = true;
                this.gvOrigen.Columns.Add(proyectoCont);

                #endregion
                #region Destino

                //Cuenta destino
                GridColumn cuentaDEST = new GridColumn();
                cuentaDEST.FieldName = this.unboundPrefix + "CuentaDEST";
                cuentaDEST.Caption = this._cuentaRsx;
                cuentaDEST.UnboundType = UnboundColumnType.String;
                cuentaDEST.VisibleIndex = 0;
                cuentaDEST.Width = 110;
                cuentaDEST.OptionsColumn.AllowEdit = true;
                this.gvDestino.Columns.Add(cuentaDEST);

                //Centro de costo destino
                GridColumn ctoCostoDEST = new GridColumn();
                ctoCostoDEST.FieldName = this.unboundPrefix + "CtoCostoDEST";
                ctoCostoDEST.Caption = this._centroCostoRsx;
                ctoCostoDEST.UnboundType = UnboundColumnType.String;
                ctoCostoDEST.VisibleIndex = 1;
                ctoCostoDEST.Width = 110;
                ctoCostoDEST.OptionsColumn.AllowEdit = true;
                this.gvDestino.Columns.Add(ctoCostoDEST);

                //Proyecto destino
                GridColumn proyectoDEST = new GridColumn();
                proyectoDEST.FieldName = this.unboundPrefix + "ProyectoDEST";
                proyectoDEST.Caption = this._proyectoRsx;
                proyectoDEST.UnboundType = UnboundColumnType.String;
                proyectoDEST.VisibleIndex = 2;
                proyectoDEST.Width = 110;
                proyectoDEST.OptionsColumn.AllowEdit = true;
                this.gvDestino.Columns.Add(proyectoDEST);

                //Porcentaje
                GridColumn porcentaje = new GridColumn();
                porcentaje.FieldName = this.unboundPrefix + "PorcentajeID";
                porcentaje.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
                porcentaje.UnboundType = UnboundColumnType.Decimal;
                porcentaje.VisibleIndex = 3;
                porcentaje.Width = 50;
                porcentaje.OptionsColumn.AllowEdit = true;
                this.gvDestino.Columns.Add(porcentaje);

                //Indice
                GridColumn indexDest = new GridColumn();
                indexDest.FieldName = this.unboundPrefix + "Index";
                indexDest.UnboundType = UnboundColumnType.Integer;
                indexDest.Visible = false;
                this.gvDestino.Columns.Add(indexDest);

                #endregion
                #region Excluye

                //Cuenta destino
                GridColumn cuentaEXCL = new GridColumn();
                cuentaEXCL.FieldName = this.unboundPrefix + "CuentaEXCL";
                cuentaEXCL.Caption = this._cuentaRsx;
                cuentaEXCL.UnboundType = UnboundColumnType.String;
                cuentaEXCL.VisibleIndex = 0;
                cuentaEXCL.Width = 110;
                cuentaEXCL.OptionsColumn.AllowEdit = true;
                this.gvExcluye.Columns.Add(cuentaEXCL);

                //Centro de costo destino
                GridColumn ctoCostoEXCL = new GridColumn();
                ctoCostoEXCL.FieldName = this.unboundPrefix + "CtoCostoEXCL";
                ctoCostoEXCL.Caption = this._centroCostoRsx;
                ctoCostoEXCL.UnboundType = UnboundColumnType.String;
                ctoCostoEXCL.VisibleIndex = 1;
                ctoCostoEXCL.Width = 110;
                ctoCostoEXCL.OptionsColumn.AllowEdit = true;
                this.gvExcluye.Columns.Add(ctoCostoEXCL);

                //Proyecto destino
                GridColumn proyectoEXCL = new GridColumn();
                proyectoEXCL.FieldName = this.unboundPrefix + "ProyectoEXCL";
                proyectoEXCL.Caption = this._proyectoRsx;
                proyectoEXCL.UnboundType = UnboundColumnType.String;
                proyectoEXCL.VisibleIndex = 2;
                proyectoEXCL.Width = 110;
                proyectoEXCL.OptionsColumn.AllowEdit = true;
                this.gvExcluye.Columns.Add(proyectoEXCL);

                //Indice
                GridColumn indexExcl = new GridColumn();
                indexExcl.FieldName = this.unboundPrefix + "Index";
                indexExcl.UnboundType = UnboundColumnType.Integer;
                indexExcl.Visible = false;
                this.gvExcluye.Columns.Add(indexExcl);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Valida que concepto de saldo de las cuentas sean de tipo cuenta
        /// </summary>
        private bool ValidateConcSaldoCta(GridView gv, int fila, string colName)
        {
            bool isValid = true;

            GridColumn col = gv.Columns[this.unboundPrefix + colName];
            string colVal = gv.GetRowCellValue(fila, col).ToString();

            if (!string.IsNullOrWhiteSpace(colVal))
            {
                if (cacheCtas.ContainsKey(colVal))
                    isValid = cacheCtas[colVal];
                else
                {
                    cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, colVal, true);
                    concSaldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cta.ConceptoSaldoID.Value, true);

                    isValid = concSaldo.coSaldoControl.Value.Value == (short)SaldoControl.Cuenta ? true : false;
                    cacheCtas.Add(colVal, isValid);
                }

                if (!isValid)
                    gv.SetColumnError(col, _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidConcSaldoCta));
                else
                    gv.SetColumnError(col, string.Empty);
            }
            else
                gv.SetColumnError(col, string.Empty);

            return isValid;
        }

        /// <summary>
        /// Calcula el porcentaje de los destinos
        /// </summary>
        private void CalcularPorcentaje()
        {
            decimal total = 0;
            foreach (DTO_coReclasificaBalance det in this._dataDestino)
                total = Math.Round(total + det.PorcentajeID.Value.Value, 2);

            this.txtPorcentaje.Text = total.ToString() + "%";

        }

        /// <summary>
        /// Carga la info de las grillas
        /// </summary>
        private void LoadData()
        {
            try
            {
                //Trae los datos
                this._data = _bc.AdministrationModel.ReclasificacionFiscal_GetDistribucion();
                this._dataExcluye = _bc.AdministrationModel.ReclasificacionFiscal_GetExclusiones();

                this.masterBalanceTipo.Value = string.Empty;

                if (this._data.Count > 0)
                    this.consecutivo = this._data.Last().Consecutivo.Value.Value;

                this.LoadDataOrigen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Filtra los datos para la grilla de origen
        /// </summary>
        private void LoadDataOrigen()
        {
            try
            {
                this.deleteOP = true;

                List<DTO_coReclasificaBalance> temp = this._data.Where(x =>
                    x.BalanceTipoID.Value == this.masterBalanceTipo.Value
                ).ToList();

                //Trae la info del cabezote
                List<int> indices = temp.Select(x => x.Consecutivo.Value.Value).Distinct().ToList();
                this._dataOrigen = new List<DTO_coReclasificaBalance>();
                foreach (int i in indices)
                {
                    int row = this._data.FindIndex(x => x.Consecutivo.Value.Value == i);
                    this._dataOrigen.Add(this._data[row]);
                }

                this.gcOrigen.DataSource = this._dataOrigen;
                this.gcOrigen.RefreshDataSource();
                bool hasItems = this._dataOrigen.GetEnumerator().MoveNext() ? true : false;
                if (hasItems)
                {
                    this._currentOrigen = this._dataOrigen.First();
                    this.gvDestino.MoveFirst();
                }
                else
                    this._currentOrigen = null;

                this.LoadDataDestino();
                this.LoadDataExcluye();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "LoadDataOrigen"));
            }
        }

        /// <summary>
        /// Carga la info de la grilla de destinos
        /// </summary>
        private void LoadDataDestino()
        {
            try
            {
                this.deleteOP = true;

                if (this._currentOrigen != null)
                {
                    this._dataDestino = this._data.Where(x =>
                        x.Consecutivo.Value.Value == this._currentOrigen.Consecutivo.Value.Value
                    ).ToList();

                    this.gcDestino.DataSource = this._dataDestino;
                    this.gcDestino.RefreshDataSource();
                    bool hasItems = this._dataDestino.GetEnumerator().MoveNext() ? true : false;
                    if (hasItems)
                    {
                        this.gvDestino.MoveFirst();
                        this._currentDestino = this._dataDestino[0];
                    }
                    else
                        this._currentDestino = null;

                    this.CalcularPorcentaje();
                }
                else
                {
                    this._currentDestino = null;
                    this._dataDestino = new List<DTO_coReclasificaBalance>();

                    this.gcDestino.DataSource = this._dataDestino;
                    this.gcDestino.RefreshDataSource();
                }
                this.deleteOP = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "LoadDataDestino"));
            }
        }

        /// <summary>
        /// Carga la info de la grilla de destinos
        /// </summary>
        private void LoadDataExcluye()
        {
            try
            {
                this.deleteOP = true;
                if (this._currentOrigen != null)
                {
                    this._dataExcluyeGrilla = this._dataExcluye.Where(x =>
                        x.Consecutivo.Value.Value == this._currentOrigen.Consecutivo.Value.Value
                    ).ToList();

                    this.gcExcluye.DataSource = this._dataExcluyeGrilla;
                    this.gcExcluye.RefreshDataSource();
                    bool hasItems = this._dataExcluyeGrilla.GetEnumerator().MoveNext() ? true : false;
                    if (hasItems)
                    {
                        this.gvExcluye.MoveFirst();
                        this._currentExcluye = this._dataExcluye[0];
                    }
                    else
                        this._currentExcluye = null;

                }
                else
                {
                    this._currentExcluye = null;
                    this._dataExcluyeGrilla = new List<DTO_coReclasificaBalExcluye>();

                    this.gcExcluye.DataSource = this._dataExcluyeGrilla;
                    this.gcExcluye.RefreshDataSource();
                }
                this.deleteOP = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "LoadDataExcluye"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowOrigen()
        {
            DTO_coReclasificaBalance det = new DTO_coReclasificaBalance();

            #region Asigna datos a la fila

            //if (this._dataOrigen.Count > 0)
            //    det.Consecutivo.Value = this._dataOrigen.Last().Consecutivo.Value.Value + 1;
            //else
            //    det.Consecutivo.Value = 0;

            this.consecutivo++;
            det.Consecutivo.Value = this.consecutivo;

            det.CuentaORIG.Value = string.Empty;
            det.CtoCostoORIG.Value = string.Empty;
            det.ProyectoORIG.Value = string.Empty;
            det.BalanceTipoID.Value = this.masterBalanceTipo.Value;
            det.Orden.Value = 1;
            det.CuentaCONT.Value = string.Empty;
            det.CtoCostoCONT.Value = string.Empty;
            det.ProyectoCONT.Value = string.Empty;
            det.CuentaDEST.Value = string.Empty;
            det.CtoCostoDEST.Value = string.Empty;
            det.ProyectoDEST.Value = string.Empty;
            det.PorcentajeID.Value = 0;

            #endregion

            this._dataOrigen.Add(det);
            this._currentOrigen = det;

            this.deleteOP = true;
            this.gvOrigen.RefreshData();
            this.gvOrigen.FocusedRowHandle = this.gvOrigen.DataRowCount - 1;

            this.isValidOrigen = false;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowDestino()
        {
            DTO_coReclasificaBalance det = new DTO_coReclasificaBalance();

            #region Asigna datos a la fila

            det.Index.Value = this._data.Count == 0 ? 0 : this._data.Last().Index.Value.Value + 1;
            det.Consecutivo.Value = this._currentOrigen.Consecutivo.Value.Value;
            det.CuentaORIG.Value = this._currentOrigen.CuentaORIG.Value;
            det.CtoCostoORIG.Value = this._currentOrigen.CtoCostoORIG.Value;
            det.ProyectoORIG.Value = this._currentOrigen.ProyectoORIG.Value;
            det.Orden.Value = this._currentOrigen.Orden.Value.Value;
            det.CuentaCONT.Value = this._currentOrigen.CuentaCONT.Value;
            det.CtoCostoCONT.Value = this._currentOrigen.CtoCostoCONT.Value;
            det.ProyectoCONT.Value = this._currentOrigen.ProyectoCONT.Value;
            det.BalanceTipoID.Value = this._currentOrigen.BalanceTipoID.Value;
            det.CuentaDEST.Value = string.Empty;
            det.CtoCostoDEST.Value = string.Empty;
            det.ProyectoDEST.Value = string.Empty;
            det.PorcentajeID.Value = 0;

            #endregion

            this._data.Add(det);
            this._dataDestino.Add(det);
            this._currentDestino = det;

            this.deleteOP = true;
            this.gvDestino.RefreshData();
            this.gvDestino.FocusedRowHandle = this.gvDestino.DataRowCount - 1;

            this.isValidDestino = false;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowExcluye()
        {
            DTO_coReclasificaBalExcluye det = new DTO_coReclasificaBalExcluye();

            #region Asigna datos a la fila

            det.Index.Value = this._dataExcluye.Count == 0 ? 0 : this._dataExcluye.Last().Index.Value.Value + 1;
            det.Consecutivo.Value = this._currentOrigen.Consecutivo.Value.Value;
            det.CuentaEXCL.Value = string.Empty;
            det.CtoCostoEXCL.Value = string.Empty;
            det.ProyectoEXCL.Value = string.Empty;

            #endregion

            this._dataExcluye.Add(det);
            this._dataExcluyeGrilla.Add(det);
            this._currentExcluye = det;

            this.deleteOP = true;
            this.gvExcluye.RefreshData();
            this.gvExcluye.FocusedRowHandle = this.gvExcluye.DataRowCount - 1;

            this.isValidExcluye = false;
        }

        /// <summary>
        /// Valida una fila del cabezote
        /// </summary>
        /// <param name="fromOrigen">Indica si se esta validando desde la grilla de origen</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRowOrigen(bool fromOrigen)
        {
            bool validRow = true;

            try
            {
                string msg;
                string colVal;
                bool validField;
                GridColumn col = new GridColumn();
                int fila = this.gvOrigen.FocusedRowHandle;

                if (fila >= 0 && fila < this.gvOrigen.DataRowCount)
                {
                    bool hasPKData = false;
                    #region Valida las Fks
                    #region CuentaORIG
                    if (!string.IsNullOrWhiteSpace(this._currentOrigen.CuentaORIG.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, fila, "CuentaORIG", true, true, true, AppMasters.coPlanCuenta, false);
                        if (!validField)
                            validRow = false;
                        else
                        {
                            validField = this.ValidateConcSaldoCta(this.gvOrigen, fila, "CuentaORIG");
                            if (!validField)
                                validRow = false;
                        }
                    }
                    #endregion
                    #region CtoCostoORIG
                    if (!string.IsNullOrWhiteSpace(this._currentOrigen.CtoCostoORIG.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, fila, "CtoCostoORIG", true, true, true, AppMasters.coCentroCosto, false);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #region ProyectoORIG
                    if (!string.IsNullOrWhiteSpace(this._currentOrigen.ProyectoORIG.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, fila, "ProyectoORIG", true, true, true, AppMasters.coProyecto, false);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #region CuentaCONT
                    if (!string.IsNullOrWhiteSpace(this._currentOrigen.CuentaCONT.Value))
                    {
                        validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, fila, "CuentaCONT", true, true, true, AppMasters.coPlanCuenta);
                        if (!validField)
                            validRow = false;
                        else
                        {
                            validField = this.ValidateConcSaldoCta(this.gvOrigen, fila, "CuentaCONT");
                            if (!validField)
                                validRow = false;
                        }
                    }
                    #endregion
                    #region CtoCostoCONT
                    if (!string.IsNullOrWhiteSpace(this._currentOrigen.CtoCostoCONT.Value))
                    {
                        validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, fila, "CtoCostoCONT", true, true, true, AppMasters.coCentroCosto);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #region ProyectoCONT
                    if (!string.IsNullOrWhiteSpace(this._currentOrigen.ProyectoCONT.Value))
                    {
                        validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, fila, "ProyectoCONT", true, true, true, AppMasters.coProyecto);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #endregion
                    #region Valida que alguno de los campos de la PK tenga datos
                    if (!hasPKData)
                    {
                        msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompIncomplete);
                        col = this.gvOrigen.Columns[this.unboundPrefix + "CuentaORIG"];
                        colVal = this.gvOrigen.GetRowCellValue(fila, col).ToString();
                        this.gvOrigen.SetColumnError(col, msg);

                        validRow = false;
                    }
                    #endregion
                    #region Valida que no se repita la PK
                    if (validRow)
                    {
                        List<DTO_coReclasificaBalance> temp = this._dataOrigen.Where(x =>
                            x.CuentaORIG.Value == this._currentOrigen.CuentaORIG.Value &&
                            x.CtoCostoORIG.Value == this._currentOrigen.CtoCostoORIG.Value &&
                            x.ProyectoORIG.Value == this._currentOrigen.ProyectoORIG.Value
                        ).ToList();

                        if (temp.Count == 0)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                            validRow = false;
                        }
                        else if (temp.Count > 1)
                        {
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompKeyAdded);
                            col = this.gvOrigen.Columns[this.unboundPrefix + "CuentaORIG"];
                            colVal = this.gvOrigen.GetRowCellValue(fila, col).ToString();
                            this.gvOrigen.SetColumnError(col, msg);

                            validRow = false;
                        }
                    }
                    #endregion
                    #region Validacion la info del orden
                    col = this.gvOrigen.Columns[this.unboundPrefix + "Orden"];
                    colVal = this.gvOrigen.GetRowCellValue(fila, col).ToString();
                    if (colVal != "1" && colVal != "2" && colVal != "3")
                    {
                        msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidDistOrder);
                        this.gvOrigen.SetColumnError(col, msg);
                        validRow = false;
                    }

                    #endregion
                    #region Valida que tenga info en el destino
                    if (validRow && fromOrigen)
                    {
                        if (this._dataDestino.Count == 0)
                        {
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompNoDest);
                            col = this.gvOrigen.Columns[this.unboundPrefix + "CuentaORIG"];
                            colVal = this.gvOrigen.GetRowCellValue(fila, col).ToString();
                            this.gvOrigen.SetColumnError(col, msg);

                            validRow = false;
                        }
                        else
                        {
                            this.gvDestino.PostEditor();
                            bool validDest = this.ValidateRowDestino();
                            if (!validDest)
                                validRow = false;
                        }
                    }
                    #endregion
                    #region Valida que tenga info en las exclusiones
                    if (validRow && fromOrigen && this._dataExcluye.Count > 0)
                    {
                        this.gvExcluye.PostEditor();
                        bool validExcl = this.ValidateRowExcluye();
                        if (!validExcl)
                            validRow = false;
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "ValidateRow"));
            }

            if (validRow)
                this.isValidOrigen = true;
            else
                this.isValidOrigen = false;

            return validRow;
        }

        /// <summary>
        /// Valida una fila del cabezote
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRowDestino()
        {
            bool validRow = true;
            try
            {
                string colVal;
                bool validField = true;
                GridColumn col = new GridColumn();
                this.isValidOrigen = true;
                int fila = this.gvDestino.FocusedRowHandle;

                if (fila >= 0 && fila < this.gvDestino.DataRowCount)
                {
                    bool hasPKData = false;
                    #region FKs
                    #region CuentaDEST
                    if (!string.IsNullOrWhiteSpace(this._currentDestino.CuentaDEST.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvDestino, string.Empty, fila, "CuentaDEST", true, true, true, AppMasters.coPlanCuenta);
                        if (!validField)
                            validRow = false;
                        else
                        {
                            validField = this.ValidateConcSaldoCta(this.gvDestino, fila, "CuentaDEST");
                            if (!validField)
                                validRow = false;
                        }
                    }
                    #endregion
                    #region CtoCostoDEST
                    if (!string.IsNullOrWhiteSpace(this._currentDestino.CtoCostoDEST.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvDestino, string.Empty, fila, "CtoCostoDEST", true, true, true, AppMasters.coCentroCosto);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #region ProyectoDEST
                    if (!string.IsNullOrWhiteSpace(this._currentDestino.ProyectoDEST.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvDestino, string.Empty, fila, "ProyectoDEST", true, true, true, AppMasters.coProyecto);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #endregion
                    #region Valida que alguno de los campos de la PK tenga datos
                    if (!hasPKData)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompIncomplete);
                        col = this.gvDestino.Columns[this.unboundPrefix + "CuentaDEST"];
                        colVal = this.gvDestino.GetRowCellValue(fila, col).ToString();
                        this.gvDestino.SetColumnError(col, msg);

                        validRow = false;
                    }
                    else if(validRow)
                    {
                        col = this.gvDestino.Columns[this.unboundPrefix + "CuentaDEST"];
                        this.gvDestino.SetColumnError(col, string.Empty);
                    }
                    #endregion
                    #region Porcentaje
                    validField = _bc.ValidGridCellValue(this.gvDestino, string.Empty, fila, "PorcentajeID", false, false, true, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Valida que no se repita la PK
                    if (validRow)
                    {
                        List<DTO_coReclasificaBalance> temp = this._dataDestino.Where(x =>
                            x.CuentaDEST.Value == this._currentDestino.CuentaDEST.Value &&
                            x.CtoCostoDEST.Value == this._currentDestino.CtoCostoDEST.Value &&
                            x.ProyectoDEST.Value == this._currentDestino.ProyectoDEST.Value
                        ).ToList();

                        if (temp.Count > 1)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompKeyAdded);
                            col = this.gvDestino.Columns[this.unboundPrefix + "CuentaDEST"];
                            colVal = this.gvDestino.GetRowCellValue(fila, col).ToString();
                            this.gvDestino.SetColumnError(col, msg);

                            validRow = false;
                        }
                        else
                        {
                            col = this.gvDestino.Columns[this.unboundPrefix + "CuentaDEST"];
                            this.gvDestino.SetColumnError(col, string.Empty);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "ValidateRow"));
            }

            if (validRow)
                this.isValidDestino = true;
            else
                this.isValidDestino = false;

            return validRow;
        }

        /// <summary>
        /// Valida una fila del cabezote
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRowExcluye()
        {
            bool validRow = true;

            try
            {
                string colVal;
                bool validField = true;
                GridColumn col = new GridColumn();
                this.isValidOrigen = true;
                int fila = this.gvExcluye.FocusedRowHandle;

                if (fila >= 0 && fila < this.gvExcluye.DataRowCount)
                {
                    bool hasPKData = false;
                    #region FKs
                    #region CuentaEXCL
                    if (!string.IsNullOrWhiteSpace(this._currentExcluye.CuentaEXCL.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvExcluye, string.Empty, fila, "CuentaEXCL", true, true, true, AppMasters.coPlanCuenta, false);
                        if (!validField)
                            validRow = false;
                        else
                        {
                            validField = this.ValidateConcSaldoCta(this.gvExcluye, fila, "CuentaEXCL");
                            if (!validField)
                                validRow = false;
                        }
                    }
                    #endregion
                    #region CtoCostoEXCL
                    if (!string.IsNullOrWhiteSpace(this._currentExcluye.CtoCostoEXCL.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvExcluye, string.Empty, fila, "CtoCostoEXCL", true, true, true, AppMasters.coCentroCosto, false);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #region ProyectoEXCL
                    if (!string.IsNullOrWhiteSpace(this._currentExcluye.ProyectoEXCL.Value))
                    {
                        hasPKData = true;
                        validField = _bc.ValidGridCell(this.gvExcluye, string.Empty, fila, "ProyectoEXCL", true, true, true, AppMasters.coProyecto, false);
                        if (!validField)
                            validRow = false;
                    }
                    #endregion
                    #endregion
                    #region Valida que alguno de los campos de la PK tenga datos
                    if (!hasPKData)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompIncomplete);
                        col = this.gvExcluye.Columns[this.unboundPrefix + "CuentaEXCL"];
                        colVal = this.gvExcluye.GetRowCellValue(fila, col).ToString();
                        this.gvExcluye.SetColumnError(col, msg);

                        validRow = false;
                    }
                    else if(validRow)
                    {
                        col = this.gvDestino.Columns[this.unboundPrefix + "CuentaEXCL"];
                        this.gvDestino.SetColumnError(col, string.Empty);
                    }
                    #endregion
                    #region Valida que no se repita la PK
                    if (validRow)
                    {
                        List<DTO_coReclasificaBalExcluye> temp = this._dataExcluye.Where(x =>
                            x.CuentaEXCL.Value == this._currentExcluye.CuentaEXCL.Value &&
                            x.CtoCostoEXCL.Value == this._currentExcluye.CtoCostoEXCL.Value &&
                            x.ProyectoEXCL.Value == this._currentExcluye.ProyectoEXCL.Value
                        ).ToList();

                        if (temp.Count > 1)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DistCompKeyAdded);
                            col = this.gvExcluye.Columns[this.unboundPrefix + "CuentaEXCL"];
                            colVal = this.gvExcluye.GetRowCellValue(fila, col).ToString();
                            this.gvExcluye.SetColumnError(col, msg);

                            validRow = false;
                        }
                        else
                        {
                            col = this.gvDestino.Columns[this.unboundPrefix + "CuentaEXCL"];
                            this.gvDestino.SetColumnError(col, string.Empty);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "ValidateRow"));
            }

            if (validRow)
                this.isValidExcluye = true;
            else
                this.isValidExcluye = false;

            return validRow;
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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Visible = false;
                    FormProvider.Master.itemDelete.Visible = false;
                    FormProvider.Master.itemPrint.Visible = false;
                    FormProvider.Master.itemFilter.Visible = false;
                    FormProvider.Master.itemFilterDef.Visible = false;
                    FormProvider.Master.itemGenerateTemplate.Visible = false;
                    FormProvider.Master.itemCopy.Visible = false;
                    FormProvider.Master.itemPaste.Visible = false;
                    FormProvider.Master.itemImport.Visible = false;
                    FormProvider.Master.itemExport.Visible = false;
                    FormProvider.Master.itemRevert.Visible = false;
                    FormProvider.Master.itemSendtoAppr.Visible = false;

                    FormProvider.Master.tbBreak.Visible = false;
                    FormProvider.Master.tbBreak0.Visible = false;
                    FormProvider.Master.tbBreak1.Visible = false;

                    FormProvider.Master.itemUpdate.Visible = true;

                    FormProvider.Master.itemUpdate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al salir de la seleccion del tipo de balance
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterBalanceTipo_Leave(object sender, EventArgs e)
        {
            try
            {
                this.gvOrigen.PostEditor();
                if (this.masterBalanceTipo.Value != this.tipoBalance)
                {
                    if (this.ValidateRowOrigen(true))
                    {
                        this.LoadDataOrigen();
                        this.tipoBalance = this.masterBalanceTipo.Value;
                    }
                    else
                        this.masterBalanceTipo.Value = this.tipoBalance;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "masterBalanceTipo_Leave"));
            }
        }

        #endregion

        #region Eventos Grilla

        #region General

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
            }
            if (e.IsSetData)
            {
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
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                GridView gv = ((DevExpress.XtraGrid.GridControl)(((ButtonEdit)sender).Parent)).FocusedView as DevExpress.XtraGrid.Views.Grid.GridView;
                string colName = gv.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                colName = colName.Replace("ORIG", "ID");
                colName = colName.Replace("CONT", "ID");
                colName = colName.Replace("DEST", "ID");
                colName = colName.Replace("EXCL", "ID");
                colName = colName.Replace("CtoCosto", "CentroCosto");

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(gv.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "editVtnGrid_ButtonClick"));
            }
        }
       
        #endregion

        #region Origen

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcOrigen_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            this.gvOrigen.PostEditor();

            if (this._dataOrigen == null || !this.masterBalanceTipo.ValidID)
                e.Handled = true;
            else
            {
                try
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        #region Nuevo
                        bool isV = this.ValidateRowOrigen(true);
                        if (isV)
                            this.AddNewRowOrigen();
                        #endregion
                    }
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        #region Eliminar
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.deleteOP = true;
                            int rowHandle = this.gvOrigen.FocusedRowHandle;

                            this._data.RemoveAll(x => x.Consecutivo.Value.Value == this._currentOrigen.Consecutivo.Value.Value);
                            this._dataExcluye.RemoveAll(x => x.Consecutivo.Value.Value == this._currentOrigen.Consecutivo.Value.Value);

                            this.LoadDataOrigen();
                            //#region Vuelve a cargar los datos

                            ////Trae la info del cabezote
                            //List<int> indices = this._data.Select(x => x.Consecutivo.Value.Value).Distinct().ToList();
                            //this._dataOrigen = new List<DTO_coReclasificaBalance>();
                            //foreach (int i in indices)
                            //{
                            //    int row = this._data.FindIndex(x => x.Consecutivo.Value.Value == i);
                            //    this._dataOrigen.Add(this._data[row]);
                            //}

                            //bool hasItems = this._dataOrigen.GetEnumerator().MoveNext() ? true : false;
                            //if (hasItems)
                            //{
                            //    if (rowHandle == 0)
                            //        this.gvOrigen.FocusedRowHandle = 0;
                            //    else
                            //        this.gvOrigen.FocusedRowHandle = this.gvOrigen.FocusedRowHandle - 1;

                            //    this._currentOrigen = this._dataOrigen[this.gvOrigen.FocusedRowHandle];
                            //}
                            //else
                            //{
                            //    this._currentOrigen = null;

                            //    this.deleteOP = true;

                            //    //Carga la info de los destinos
                            //    this._dataDestino = new List<DTO_coReclasificaBalance>();
                            //    this.gcDestino.DataSource = this._dataDestino;
                            //    this.gcDestino.RefreshDataSource();

                            //    //Carga la info de exclusiones
                            //    this._dataExcluye = new List<DTO_coReclasificaBalExcluye>();
                            //    this.gcExcluye.DataSource = this._dataExcluye;
                            //    this.gcExcluye.RefreshDataSource();
                            //}

                            //this.gcOrigen.DataSource = this._dataOrigen;
                            //this.gcOrigen.RefreshDataSource();

                            //this.LoadDataDestino();
                            //this.LoadDataExcluye();
                            //#endregion
                        }

                        e.Handled = true;
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "ValidateRowOrigen"));
                }
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvOrigen_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {            
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "CuentaORIG" || fieldName == "CtoCostoORIG" || fieldName == "ProyectoORIG" ||
                fieldName == "CuentaCONT" || fieldName == "CtoCostoCONT" || fieldName == "ProyectoCONT")
                e.RepositoryItem = this.editBtnGrid;

            if (fieldName == "Orden")
                e.RepositoryItem = this.editCmb;

        }

        /// <summary>
        /// Realiza operaciones al momento de cambiar un valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvOrigen_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            int index = this.gvOrigen.FocusedRowHandle;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvOrigen.Columns[this.unboundPrefix + fieldName];
            string colVal = this.gvOrigen.GetRowCellValue(index, col).ToString(); ;

            bool validField = true;

            #region FKs

            if (fieldName == "CuentaORIG")
            {
                validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coPlanCuenta, false);
                if (validField)
                {
                    validField = this.ValidateConcSaldoCta(this.gvOrigen, e.RowHandle, fieldName);
                    if (validField)
                    {
                        foreach (DTO_coReclasificaBalance det in this._dataDestino)
                        {
                            this.indexFilaData = det.Index.Value.Value;
                            this._data[this.NumFilaData].CuentaORIG.Value = colVal;
                        }
                    }
                }

            }

            if (fieldName == "CtoCostoORIG")
            {
                validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coCentroCosto, false);
                if (validField)
                {
                    foreach (DTO_coReclasificaBalance det in this._dataDestino)
                    {
                        this.indexFilaData = det.Index.Value.Value;
                        this._data[this.NumFilaData].CtoCostoORIG.Value = colVal;
                    }
                }
            }

            if (fieldName == "ProyectoORIG")
            {
                validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coProyecto, false);
                if (validField)
                {
                    foreach (DTO_coReclasificaBalance det in this._dataDestino)
                    {
                        this.indexFilaData = det.Index.Value.Value;
                        this._data[this.NumFilaData].ProyectoORIG.Value = colVal;
                    }
                }
            }

            if (fieldName == "CuentaCONT")
            {
                validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coPlanCuenta);
                if (validField)
                {
                    validField = this.ValidateConcSaldoCta(this.gvOrigen, e.RowHandle, fieldName);
                    if (validField)
                    {
                        foreach (DTO_coReclasificaBalance det in this._dataDestino)
                        {
                            this.indexFilaData = det.Index.Value.Value;
                            this._data[this.NumFilaData].CuentaCONT.Value = colVal;
                        }
                    }
                }
            }

            if (fieldName == "CtoCostoCONT")
            {
                validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coCentroCosto);
                if (validField)
                {
                    foreach (DTO_coReclasificaBalance det in this._dataDestino)
                    {
                        this.indexFilaData = det.Index.Value.Value;
                        this._data[this.NumFilaData].CtoCostoCONT.Value = colVal;
                    }
                }
            }

            if (fieldName == "ProyectoCONT")
            {
                validField = _bc.ValidGridCell(this.gvOrigen, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coProyecto);
                if (validField)
                {
                    foreach (DTO_coReclasificaBalance det in this._dataDestino)
                    {
                        this.indexFilaData = det.Index.Value.Value;
                        this._data[this.NumFilaData].ProyectoCONT.Value = colVal;
                    }
                }
            }

            #endregion
            #region Origen

            if (fieldName == "Orden")
            {
                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidDistOrder);
                if (colVal != "1" && colVal != "2" && colVal != "3")
                {
                    this.gvOrigen.SetColumnError(col, msg);
                    validField = false;
                }
                else
                {
                    this.gvOrigen.SetColumnError(col, string.Empty);
                    foreach (DTO_coReclasificaBalance det in this._dataDestino)
                    {
                        this.indexFilaData = det.Index.Value.Value;
                        this._data[this.NumFilaData].Orden.Value = Convert.ToByte(colVal);
                    }
                }
            }

            #endregion

            if (!validField)
                this.isValidOrigen = false;
            else
                this.LoadDataDestino();
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvOrigen_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            bool validRow = this.deleteOP ? true : this.ValidateRowOrigen(true);
            this.deleteOP = false;

            if (validRow)
                this.isValidOrigen = true;
            else
            {
                e.Allow = false;
                this.isValidOrigen = false;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvOrigen_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.isValidOrigen = true;

            this._currentOrigen = this.gvOrigen.FocusedRowHandle >= 0 ? this._dataOrigen[this.gvOrigen.FocusedRowHandle] : null;
            this.LoadDataDestino();
            this.LoadDataExcluye();
        }

        #endregion

        #region Destino

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDestino_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this._dataOrigen.Count > 0 && (this.isValidOrigen || this.ValidateRowOrigen(false)))
                {
                    this.gvDestino.PostEditor();

                    if (this._dataDestino == null || !this.masterBalanceTipo.ValidID)
                        e.Handled = true;
                    else
                    {
                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                        {
                            #region Nuevo
                            if (this.isValidDestino)
                                this.AddNewRowDestino();
                            else
                            {
                                bool isV = this.ValidateRowDestino();
                                if (isV)
                                    this.AddNewRowDestino();
                            }
                            #endregion
                        }
                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                        {
                            #region Eliminar
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                            //Revisa si desea cargar los temporales
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (this._dataDestino.Count == 1)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                    e.Handled = true;
                                }
                                else
                                {
                                    this.deleteOP = true;
                                    int rowHandle = this.gvDestino.FocusedRowHandle;

                                    this._data.RemoveAll(x =>
                                        x.Consecutivo.Value == this._currentDestino.Consecutivo.Value &&
                                        x.CuentaORIG.Value == this._currentDestino.CuentaORIG.Value &&
                                        x.CtoCostoORIG.Value == this._currentDestino.CtoCostoORIG.Value &&
                                        x.ProyectoORIG.Value == this._currentDestino.ProyectoORIG.Value &&
                                        x.CuentaDEST.Value == this._currentDestino.CuentaDEST.Value &&
                                        x.CtoCostoDEST.Value == this._currentDestino.CtoCostoDEST.Value &&
                                        x.ProyectoDEST.Value == this._currentDestino.ProyectoDEST.Value
                                    );

                                    this._dataDestino.RemoveAt(rowHandle);
                                    this.gvDestino.FocusedRowHandle = rowHandle - 1;

                                    this.gvDestino.RefreshData();
                                    this.CalcularPorcentaje();
                                }
                            }

                            e.Handled = true;
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "gcDestino_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDestino_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "CuentaDEST" || fieldName == "CtoCostoDEST" || fieldName == "ProyectoDEST")
                e.RepositoryItem = this.editBtnGrid;
           
            if (fieldName == "PorcentajeID")
                e.RepositoryItem = this.editValue;
        }

        /// <summary>
        /// Realiza operaciones al momento de cambiar un valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDestino_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            int index = this.gvDestino.FocusedRowHandle;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDestino.Columns[this.unboundPrefix + fieldName];
            string colVal = this.gvDestino.GetRowCellValue(index, col).ToString();

            bool validField = true;

            #region FKs

            if (fieldName == "CuentaDEST")
            {
                validField = _bc.ValidGridCell(this.gvDestino, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coPlanCuenta);
                if (validField)
                {
                    validField = this.ValidateConcSaldoCta(this.gvDestino, e.RowHandle, fieldName);
                    if (validField)
                        this._data[this.NumFilaData].CuentaDEST.Value = colVal;
                }
            }

            if (fieldName == "CtoCostoDEST")
            {
                validField = _bc.ValidGridCell(this.gvDestino, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coCentroCosto);
                if (validField)
                    this._data[this.NumFilaData].CtoCostoDEST.Value = colVal;
            }

            if (fieldName == "ProyectoDEST")
            {
                validField = _bc.ValidGridCell(this.gvDestino, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coProyecto);
                if (validField)
                    this._data[this.NumFilaData].ProyectoDEST.Value = colVal;
            }

            #endregion
            #region Porcentaje

            if (fieldName == "PorcentajeID")
            {
                validField = _bc.ValidGridCellValue(this.gvDestino, string.Empty, index, "PorcentajeID", false, false, true, false);
                if (validField)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_DistPorc);

                    string val = this.txtPorcentaje.Text.Replace("%", "").Trim();
                    decimal newVal = Convert.ToDecimal(colVal, CultureInfo.InvariantCulture);
                    decimal total = Convert.ToDecimal(this.txtPorcentaje.Text.Replace("%", "").Trim());

                    decimal porc = Math.Round(total + newVal - this.currentPorc, 2);
                    if (porc > 100)
                    {
                        this.gvDestino.SetColumnError(col, msg);
                        validField = false;
                    }
                    else
                    {
                        this.currentPorc = newVal;
                        this.gvDestino.SetColumnError(col, string.Empty);
                        this.txtPorcentaje.Text = porc.ToString() + "%";
                        this._data[this.NumFilaData].PorcentajeID.Value = newVal;
                    }
                }
            }

            #endregion

            if (!validField)
                this.isValidDestino = false;
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDestino_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            bool validRow = this.deleteOP ? true : this.ValidateRowDestino();
            this.deleteOP = false;

            if (validRow)
                this.isValidDestino = true;
            else
            {
                e.Allow = false;
                this.isValidDestino = false;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDestino_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                GridColumn col = this.gvDestino.Columns[this.unboundPrefix + "Index"];
                this.indexFilaData = Convert.ToInt16(this.gvDestino.GetRowCellValue(e.FocusedRowHandle, col));
                this.isValidDestino = true;

                int nFila = this.NumFilaData;
                if (nFila >= 0)
                {
                    this.currentPorc = this._data[this.NumFilaData].PorcentajeID.Value.Value;
                    this._currentDestino = this._data[this.NumFilaData];
                }
                else
                {
                    this.currentPorc = 0;
                    this._currentDestino = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "gcDestino_FocusedRowChanged"));
            }
        }

        #endregion

        #region Excluye

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcExcluye_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this._dataOrigen.Count > 0 && (this.isValidOrigen || this.ValidateRowOrigen(false)))
                {
                    this.gvExcluye.PostEditor();

                    if (this._dataExcluye == null || !this.masterBalanceTipo.ValidID)
                        e.Handled = true;
                    else
                    {
                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                        {
                            #region Nuevo
                            if (this.isValidExcluye)
                                this.AddNewRowExcluye();
                            else
                            {
                                bool isV = this.ValidateRowExcluye();
                                if (isV)
                                    this.AddNewRowExcluye();
                            }
                            #endregion
                        }
                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                        {
                            #region Eliminar
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                            //Revisa si desea cargar los temporales
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (this._dataExcluye.Count > 0)
                                {
                                    this.deleteOP = true;
                                    int rowHandle = this.gvExcluye.FocusedRowHandle;

                                    this._dataExcluye.RemoveAll(x =>
                                        x.Consecutivo.Value == this._currentExcluye.Consecutivo.Value &&
                                        x.CuentaEXCL.Value == this._currentExcluye.CuentaEXCL.Value &&
                                        x.CtoCostoEXCL.Value == this._currentExcluye.CtoCostoEXCL.Value &&
                                        x.ProyectoEXCL.Value == this._currentExcluye.ProyectoEXCL.Value
                                    );

                                    this._dataExcluyeGrilla.RemoveAt(rowHandle);
                                    if (rowHandle == 0)
                                        this.gvExcluye.FocusedRowHandle = 0;
                                    else
                                        this.gvExcluye.FocusedRowHandle = rowHandle - 1;

                                    this.gvExcluye.RefreshData();
                                    this.CalcularPorcentaje();
                                }
                                else
                                    e.Handled = true;
                            }

                            e.Handled = true;
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "gcExcluye_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvExcluye_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            
            if (fieldName == "CuentaEXCL" || fieldName == "CtoCostoEXCL" || fieldName == "ProyectoEXCL")
                e.RepositoryItem = this.editBtnGrid;
        }

        /// <summary>
        /// Realiza operaciones al momento de cambiar un valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvExcluye_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            int index = this.gvExcluye.FocusedRowHandle;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvExcluye.Columns[this.unboundPrefix + fieldName];
            string colVal = this.gvExcluye.GetRowCellValue(index, col).ToString();

            bool validField = true;

            #region FKs

            if (fieldName == "CuentaEXCL")
            {
                validField = _bc.ValidGridCell(this.gvExcluye, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coPlanCuenta, false);
                if (validField)
                {
                    validField = this.ValidateConcSaldoCta(this.gvExcluye, e.RowHandle, fieldName);
                    if (validField)
                        this._dataExcluye[this.indexFilaExcluye].CuentaEXCL.Value = colVal;
                }
            }

            if (fieldName == "CtoCostoEXCL")
            {
                validField = _bc.ValidGridCell(this.gvExcluye, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coCentroCosto, false);
                if (validField)
                    this._dataExcluye[this.indexFilaExcluye].CtoCostoEXCL.Value = colVal;
            }

            if (fieldName == "ProyectoEXCL")
            {
                validField = _bc.ValidGridCell(this.gvExcluye, string.Empty, e.RowHandle, fieldName, true, true, true, AppMasters.coProyecto, false);
                if (validField)
                    this._dataExcluye[this.indexFilaExcluye].ProyectoEXCL.Value = colVal;
            }

            #endregion

            if (!validField)
                this.isValidExcluye = false;
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvExcluye_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            bool validRow = this.deleteOP ? true : this.ValidateRowExcluye();
            this.deleteOP = false;

            if (validRow)
                this.isValidExcluye = true;
            else
            {
                e.Allow = false;
                this.isValidExcluye = false;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvExcluye_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                GridColumn col = this.gvExcluye.Columns[this.unboundPrefix + "Index"];
                this.indexFilaExcluye = Convert.ToInt16(this.gvExcluye.GetRowCellValue(e.FocusedRowHandle, col));
                this.isValidExcluye = true;

                int nFila = this.NumFilaExcluye;
                this._currentExcluye = this.NumFilaExcluye >= 0 ? this._dataExcluye[this.NumFilaExcluye] : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "gvExcluye_FocusedRowChanged"));
            }
        }

        #endregion

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvOrigen.PostEditor();

                if (this.ValidateRowOrigen(true))
                {
                    DTO_TxResult result = _bc.AdministrationModel.ReclasificacionFiscal_Update(this.documentID, this._data, this._dataExcluye);
                    MessageForm msg = new MessageForm(result);
                    msg.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionFiscal.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {
            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.UpdateData);

            //Revisa si desea cargar los temporales
            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.LoadData();
        }

        #endregion

    }

}
