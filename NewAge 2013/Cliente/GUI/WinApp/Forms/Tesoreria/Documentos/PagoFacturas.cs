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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class PagoFacturas : FormWithToolbar
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
                bool deseaImp = false;
                if (!this.results.Any(x=>x.Result == ResultValue.NOK))
                {
                    string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                    var result = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        deseaImp = true;
                }

                if (deseaImp)
                {
                    foreach (DTO_TxResult item in this.results)
                    {
                        if (item.Result == ResultValue.OK)
                        {
                            int docReporte = !string.IsNullOrEmpty(this._bancoCuenta.DocumentoID.Value) ? Convert.ToInt32(this._bancoCuenta.DocumentoID.Value) : this._documentID;
                            reportName = this._bc.AdministrationModel.ReportesTesoreria_PagosFactura(docReporte, Convert.ToInt32(item.ExtraField), ExportFormatType.pdf);
                        }
                    } 
                }

                this.GetPagoFacturas();
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "SaveMethod"));
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
        private string _unboundPrefix = "Unbound_";
        private int _indexFila = 0;
        private List<DTO_PagoFacturas> _pagoFacturasList = new List<DTO_PagoFacturas>();
        private List<DTO_PagoFacturas> _pagoFacturasActual = new List<DTO_PagoFacturas>();
        private List<DTO_DetalleFactura> _detalleFacturas = new List<DTO_DetalleFactura>();
        private string _monedaLocal;
        private string _monedaExtranjera;
        private string _areaFuncionalID;

        private DTO_tsBancosCuenta _bancoCuenta;
        private string _actFlujoID = string.Empty;
        List<DTO_TxResult> results;
        //Variables para reportes
        private string reportName;
        private string fileURl;

        #endregion

        #region Propiedades

        /// <summary>
        /// Pago facturas sobre el cual se esta trabajando
        /// </summary>
        private DTO_PagoFacturas _pagoFacturas = null;
        protected virtual DTO_PagoFacturas PagoFacturasAct
        {
            get { return this._pagoFacturas; }
            set { this._pagoFacturas = value; }
        }

        /// <summary>
        /// Numero de una fila segun el indice
        /// </summary>
        protected int NumFila
        {
            get
            {
                return this._pagoFacturasList.FindIndex(det => det.Index == this._indexFila);
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
        public PagoFacturas()
        {
            try
            {
                this.InitializeComponent();

                this._documentID = AppDocuments.DesembolsoFacturas;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.ts;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Carga info de las monedas
                this._monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                
                //Carga info del área funcional del usuario
                this._areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;

                this.AfterInitialize();

                //Inicia los controles de usuario maestras
                _bc.InitMasterUC(this.masterCuenta, AppMasters.tsBancosCuenta, true, true, true, false);

                #region Periodo

                _bc.InitPeriodUC(this.dtPeriod, 0);
                DateTime periodo = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo));
                this.dtPeriod.DateTime = periodo;
                DateTime fecha = DateTime.Now;
                if (fecha.Month > periodo.Month)
                {
                    int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    fecha = new DateTime(periodo.Year, periodo.Month, day);
                }
                this.dtFecha.DateTime = fecha;
                
                #endregion

                //Carga las columnas de las grillas
                this.AddGridColsGvPagos();
                this.AddGridColsGvDetalleFacturas();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    this._actFlujoID = actividades[0];
            
                    //Carga la información de las grillas
                    this.LoadDataGvPagos();
                    this.LoadDataGvDetalleFacturas();
                }
                #endregion
                
                this.saveDelegate = new Save(this.SaveMethod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a la grilla gvPagos
        /// </summary>
        private void AddGridColsGvPagos()
        {
            try
            {
                #region Cabezote

                //IndicadorPago
                GridColumn indicadorPago = new GridColumn();
                indicadorPago.FieldName = this._unboundPrefix + "PagoFacturasInd";
                indicadorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PagoFacturasInd");
                indicadorPago.UnboundType = UnboundColumnType.Boolean;
                indicadorPago.VisibleIndex = 0;
                indicadorPago.Width = 60;
                indicadorPago.Visible = true;
                indicadorPago.OptionsColumn.AllowEdit = true;
                this.gvDetallePagos.Columns.Add(indicadorPago);

                //BancoCuenta
                GridColumn bancoCuenta = new GridColumn();
                bancoCuenta.FieldName = this._unboundPrefix + "BancoCuentaID";
                bancoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BancoCuentaID");
                bancoCuenta.UnboundType = UnboundColumnType.String;
                bancoCuenta.VisibleIndex = 1;
                bancoCuenta.Width = 120;
                bancoCuenta.Visible = true;
                bancoCuenta.OptionsColumn.AllowEdit = false;
                this.gvDetallePagos.Columns.Add(bancoCuenta);

                ////Tercero
                //GridColumn tercero = new GridColumn();
                //tercero.FieldName = this._unboundPrefix + "TerceroID";
                //tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                //tercero.UnboundType = UnboundColumnType.String;
                //tercero.VisibleIndex = 2;
                //tercero.Width = 120;
                //tercero.Visible = true;
                //tercero.OptionsColumn.AllowEdit = false;
                //this.gvDetallePagos.Columns.Add(tercero);

                ////NombreTercero
                //GridColumn nombreTercero = new GridColumn();
                //nombreTercero.FieldName = this._unboundPrefix + "Descriptivo";
                //nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NombreTercero");
                //nombreTercero.UnboundType = UnboundColumnType.String;
                //nombreTercero.VisibleIndex = 3;
                //nombreTercero.Width = 180;
                //nombreTercero.Visible = true;
                //nombreTercero.OptionsColumn.AllowEdit = false;
                //this.gvDetallePagos.Columns.Add(nombreTercero);



                //Tercero
                GridColumn beneficiarioID = new GridColumn();
                beneficiarioID.FieldName = this._unboundPrefix + "BeneficiarioID";
                beneficiarioID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BeneficiarioID");
                beneficiarioID.UnboundType = UnboundColumnType.String;
                beneficiarioID.VisibleIndex = 4;
                beneficiarioID.Width = 120;
                beneficiarioID.Visible = true;
                beneficiarioID.OptionsColumn.AllowEdit = false;
                this.gvDetallePagos.Columns.Add(beneficiarioID);

                //NombreTercero
                GridColumn beneficiario = new GridColumn();
                beneficiario.FieldName = this._unboundPrefix + "BeneficiarioDesc";
                beneficiario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BeneficiarioDesc");
                beneficiario.UnboundType = UnboundColumnType.String;
                beneficiario.VisibleIndex = 5;
                beneficiario.Width = 180;
                beneficiario.Visible = true;
                beneficiario.OptionsColumn.AllowEdit = false;
                this.gvDetallePagos.Columns.Add(beneficiario);

                //Numero de facturas
                GridColumn nroFacturas = new GridColumn();
                nroFacturas.FieldName = this._unboundPrefix + "NumeroFacturas";
                nroFacturas.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NroFactura");
                nroFacturas.UnboundType = UnboundColumnType.Integer;
                nroFacturas.VisibleIndex = 6;
                nroFacturas.Width = 80;
                nroFacturas.Visible = true;
                nroFacturas.OptionsColumn.AllowEdit = false;
                this.gvDetallePagos.Columns.Add(nroFacturas);

                //Moneda
                GridColumn monedaFact = new GridColumn();
                monedaFact.FieldName = this._unboundPrefix + "MonedaID";
                monedaFact.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MonedaID");
                monedaFact.UnboundType = UnboundColumnType.String;
                monedaFact.VisibleIndex = 7;
                monedaFact.Width = 70;
                monedaFact.Visible = true;
                monedaFact.OptionsColumn.AllowEdit = false;
                this.gvDetallePagos.Columns.Add(monedaFact);

                //Total de facturas
                GridColumn totalFacturas = new GridColumn();
                totalFacturas.FieldName = this._unboundPrefix + "TotalFacturas";
                totalFacturas.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPago");
                totalFacturas.UnboundType = UnboundColumnType.Decimal;
                totalFacturas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                totalFacturas.AppearanceCell.Options.UseTextOptions = true;
                totalFacturas.VisibleIndex = 8;
                totalFacturas.Width = 130;
                totalFacturas.Visible = true;
                totalFacturas.OptionsColumn.AllowEdit = false;
                this.gvDetallePagos.Columns.Add(totalFacturas);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this._unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetallePagos.Columns.Add(colIndex);

                #endregion

                #region Detalle

                //Numero Doc
                GridColumn numeroDoc = new GridColumn();
                numeroDoc.FieldName = this._unboundPrefix + "NumeroDoc";
                numeroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NumeroDoc");
                numeroDoc.UnboundType = UnboundColumnType.Integer;
                numeroDoc.VisibleIndex = 0;
                numeroDoc.Width = 60;
                numeroDoc.Visible = true;
                numeroDoc.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(numeroDoc);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex =1;
                TerceroID.Width = 80;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(TerceroID);

                //Tercero
                GridColumn Tercero = new GridColumn();
                Tercero.FieldName = this._unboundPrefix + "Tercero";
                Tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Tercero");
                Tercero.UnboundType = UnboundColumnType.String;
                Tercero.VisibleIndex = 2;
                Tercero.Width = 150;
                Tercero.Visible = true;
                Tercero.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(Tercero);

                //Documento Tercero
                GridColumn documentoTercero = new GridColumn();
                documentoTercero.FieldName = this._unboundPrefix + "DocumentoTercero";
                documentoTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoTercero");
                documentoTercero.UnboundType = UnboundColumnType.String;
                documentoTercero.VisibleIndex = 3;
                documentoTercero.Width = 180;
                documentoTercero.Visible = true;
                documentoTercero.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(documentoTercero);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this._unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.VisibleIndex = 3;
                observacion.Width = 180;
                observacion.Visible = true;
                observacion.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(observacion);

                //Moneda
                GridColumn monedaID = new GridColumn();
                monedaID.FieldName = this._unboundPrefix + "MonedaID";
                monedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MonedaID");
                monedaID.UnboundType = UnboundColumnType.String;
                monedaID.VisibleIndex = 7;
                monedaID.Width = 100;
                monedaID.Visible = true;
                monedaID.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(monedaID);

                //Valor Pago
                GridColumn valorPago = new GridColumn();
                valorPago.FieldName = this._unboundPrefix + "ValorPago";
                valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPago");
                valorPago.UnboundType = UnboundColumnType.Decimal;
                valorPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorPago.AppearanceCell.Options.UseTextOptions = true;
                valorPago.VisibleIndex = 8;
                valorPago.Width = 140;
                valorPago.Visible = true;
                valorPago.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(valorPago);

                //Valor Pago Local
                GridColumn valorPagoLocal = new GridColumn();
                valorPagoLocal.FieldName = this._unboundPrefix + "ValorPagoLocal";
                valorPagoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPagoLocal");
                valorPagoLocal.UnboundType = UnboundColumnType.Decimal;
                valorPagoLocal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorPagoLocal.AppearanceCell.Options.UseTextOptions = true;
                valorPagoLocal.VisibleIndex = 8;
                valorPagoLocal.Width = 140;
                valorPagoLocal.Visible = true;
                valorPagoLocal.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(valorPagoLocal);

                //Valor Pago Extra
                GridColumn valorPagoExtra = new GridColumn();
                valorPagoExtra.FieldName = this._unboundPrefix + "ValorPagoExtra";
                valorPagoExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPagoExtra");
                valorPagoExtra.UnboundType = UnboundColumnType.Decimal;
                valorPagoExtra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorPagoExtra.AppearanceCell.Options.UseTextOptions = true;
                valorPagoExtra.VisibleIndex = 8;
                valorPagoExtra.Width = 140;
                valorPagoExtra.Visible = true;
                valorPagoExtra.OptionsColumn.AllowEdit = false;
                this.gvSubDetalle.Columns.Add(valorPagoExtra);

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla gvDetalleFacturas
        /// </summary>
        private void AddGridColsGvDetalleFacturas()
        {
            try
            {


                //Documento Tercero
                GridColumn docTercero = new GridColumn();
                docTercero.FieldName = this._unboundPrefix + "DocumentoTercero";
                docTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoTercero");
                docTercero.UnboundType = UnboundColumnType.String;
                docTercero.VisibleIndex = 0;
                docTercero.Width = 150;
                docTercero.Visible = true;
                docTercero.OptionsColumn.AllowEdit = false;
                this.gvDetalleFacturas.Columns.Add(docTercero);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 1;
                TerceroID.Width = 80;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDetalleFacturas.Columns.Add(TerceroID);

                //Tercero
                GridColumn Tercero = new GridColumn();
                Tercero.FieldName = this._unboundPrefix + "Tercero";
                Tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Tercero");
                Tercero.UnboundType = UnboundColumnType.String;
                Tercero.VisibleIndex = 2;
                Tercero.Width = 150;
                Tercero.Visible = true;
                Tercero.OptionsColumn.AllowEdit = false;
                this.gvDetalleFacturas.Columns.Add(Tercero);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this._unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.VisibleIndex = 3;
                observacion.Width = 350;
                observacion.Visible = true;
                observacion.OptionsColumn.AllowEdit = false;
                this.gvDetalleFacturas.Columns.Add(observacion);

                //ValorPago
                GridColumn valorPago = new GridColumn();
                valorPago.FieldName = this._unboundPrefix + "ValorPago";
                valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPago");
                valorPago.UnboundType = UnboundColumnType.Decimal;
                valorPago.VisibleIndex = 4;
                valorPago.Width = 140;
                valorPago.Visible = true;
                valorPago.OptionsColumn.AllowEdit = false;
                this.gvDetalleFacturas.Columns.Add(valorPago);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this._unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetalleFacturas.Columns.Add(colIndex);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializeComponents
        /// </summary>
        private void AfterInitialize()
        {
            this.GetPagoFacturas();
        }

        /// <summary>
        /// Obtiene el pago de las facturas validando que la tasa de cambio existe para el día en que se esta realizando la transacción
        /// </summary>
        private void GetPagoFacturas()
        {
            try
            {
                this._pagoFacturasList = new List<DTO_PagoFacturas>();
                List<DTO_SerializedObject> data = _bc.AdministrationModel.PagoFacturas_GetPagoFacturas();

                if (data.Count > 0 && data.First().GetType() == typeof(DTO_TxResult))
                {
                    DTO_TxResult txResult = (DTO_TxResult)data[0];

                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, txResult.ResultMessage));
                    this.masterCuenta.Enabled = false;
                } 
                if (data.Count > 0 && data.First().GetType() == typeof(DTO_PagoFacturas))
                {
                    this._pagoFacturasList = data.Cast<DTO_PagoFacturas>().ToList();
                    this.masterCuenta.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "GetPagoFacturas"));
            }
        }

        /// <summary>
        /// Carga la información del formulario, con respecto a la cuenta de banco seleccionada
        /// </summary>
        private void LoadData()
        {
            try
            {
                //Verifica si el glControl esta habilitado para cambiar el usuario Beneficiario
                bool cambioBeneficiario = this._bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_HabilitarCambioBeneficiarioGirarCheque).Equals("1")? true : false;
                if (cambioBeneficiario)
                    this.gvDetallePagos.Columns[this._unboundPrefix +  "BeneficiarioDesc"].OptionsColumn.AllowEdit = true;

                if (this.masterCuenta.ValidID)
                    this._pagoFacturasActual = this._pagoFacturasList.FindAll(p => p.BancoCuentaID.Value == this.masterCuenta.Value);
                else
                    this._pagoFacturasActual = new List<DTO_PagoFacturas>();

                if (this._pagoFacturasActual.Count > 0)
                {
                    this.chkSelectAll.Enabled = true;
                    this.chkSelectAll.CheckState = CheckState.Checked;
                    this.txtNroEgreso.Text = this._pagoFacturasActual[0].ConsecutivoEgreso.Value.ToString();

                    this._bancoCuenta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuenta.Value, true);

                    //Carga la informacion de los numeros del cheque
                    int chequeIni = this._bancoCuenta.ChequeInicial.Value.Value;
                    int chequeFin = this._bancoCuenta.ChequeFinal.Value.Value;

                    if (chequeIni + this._pagoFacturasActual.Count() > chequeFin + 1)
                    {
                        string msg = _bc.GetResourceError(DictionaryMessages.Err_Ts_ChequesFinalizados);
                        this._pagoFacturasActual = new List<DTO_PagoFacturas>();
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        this.txtChequeIni.Text = chequeIni.ToString();
                        this.txtChequeFin.Text = (chequeIni + this._pagoFacturasActual.Count()).ToString();
                    }
                }
                else
                {
                    this.chkSelectAll.Enabled = false;
                    this.chkSelectAll.CheckState = CheckState.Unchecked;

                    this._bancoCuenta = null;

                    this.txtChequeIni.Text = "0";
                    this.txtChequeFin.Text = "0";
                    this.txtNroEgreso.Text = "0";
                }
                this.LoadDataGvPagos();
                if (this._pagoFacturasActual.Count > 0)
                    this.UpdateFooterGridData(this._pagoFacturasActual[0]);
                else
                {
                    this._detalleFacturas = new List<DTO_DetalleFactura>();
                    this.LoadDataGvDetalleFacturas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla gvPagos
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadDataGvPagos()
        {
            this.gcPagos.DataSource = this._pagoFacturasActual;
            this.gcPagos.RefreshDataSource();
        }

        /// <summary>
        /// Actualiza la información de la factura en el grid
        /// </summary>
        private void UpdateFooterGridData(DTO_PagoFacturas pagoFactura)
        {
            this._detalleFacturas = pagoFactura.DetallesFacturas;
            this.LoadDataGvDetalleFacturas();
        }

        /// <summary>
        /// Carga la información de las grilla gvDetalleFacturas
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadDataGvDetalleFacturas()
        {
            this.gcDetalleFacturas.DataSource = this._detalleFacturas;
            this.gcDetalleFacturas.RefreshDataSource();
        }

        /// <summary>
        /// Valida la info antes de enviar al servidor
        /// </summary>
        private bool ValidateData()
        {
            try
            {
                if (this._bancoCuenta == null)
                    return false;

                //Valida que el modulo este en el periodo del dia actual
                DateTime periodo = this.dtPeriod.DateTime;
                DateTime today = this.dtFecha.DateTime;
                if (periodo.Year != today.Year || periodo.Month != today.Month)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_InvalidPeriod));
                    return false;
                }

                //Valida la tasa de cambio
                if (_bc.AdministrationModel.MultiMoneda)
                {
                    decimal tc = _bc.AdministrationModel.TasaDeCambio_Get(this._monedaExtranjera, this.dtFecha.DateTime);
                    if (tc <= 0)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_NoTasaCambioFechaActual));
                        return false;
                    }
                }

                // Valida Cheques Faltantes
                List<DTO_PagoFacturas> facturasAPagar = this._pagoFacturasActual.FindAll(p => p.PagoFacturasInd.Value.HasValue && p.PagoFacturasInd.Value.Value);
                int cantChequesDisponibles = this._bancoCuenta.ChequeFinal.Value.Value - this._bancoCuenta.ChequeInicial.Value.Value + 1;
                if (cantChequesDisponibles < facturasAPagar.Count)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_CantidadChequesDisponible);
                    MessageBox.Show(string.Format(msg, cantChequesDisponibles.ToString()));
                    return false;
                }

                //Valida que hayan facturas seleccionadas
                if (facturasAPagar.Count == 0)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_NoHayPagosSeleccionados));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "ValidateData"));
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
                FormProvider.Master.itemPrint.Visible = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "PagoFacturas.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que selecciona la Cuenta de Banco con la que se va a realizar el pago
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCuenta_Leave(object sender, EventArgs e)
        {
            this.LoadData();  
        }

        /// <summary>
        /// Evento que se llama cuando se selecciona o desselecciona todo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkSelectAll_MouseClick(object sender, MouseEventArgs e)
        {
            bool value = this.chkSelectAll.Checked;
            this._pagoFacturasActual.ForEach(p => p.PagoFacturasInd.Value = value);
            this.gcPagos.RefreshDataSource();

            if (value)
            {
                int chequeIni = this._bancoCuenta.ChequeInicial.Value.Value + 1;
                this.txtChequeFin.Text = (chequeIni + this._pagoFacturasActual.Count()).ToString();
            }
            else
                this.txtChequeFin.Text = this.txtChequeIni.Text;
        }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtPeriod_EditValueChanged()
        {
            try
            {
                int currentMonth = this.dtPeriod.DateTime.Month;
                int currentYear = this.dtPeriod.DateTime.Year;
                int minDay = 1;
                int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas", "dtPeriod_EditValueChanged"));
            }
        }

        #endregion

        #region Eventos Grilla gvPagos

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "PagoFacturasInd")
            {
                e.RepositoryItem = this.editCheck;
            }
            if (fieldName == "TotalFacturas")
            {
                e.RepositoryItem = this.editSpin;
            }
            
        }

        /// <summary>
        /// Formato campos grilla subdetalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvSubDetalle_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "ValorPago" || fieldName == "ValorPagoLocal" || fieldName == "ValorPagoExtra")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Formato de las columnas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvSubDetalle_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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

        /// <summary>
        /// Actualiza valores editables cuando se cambia estado de pago
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (!this.gvDetallePagos.IsFilterRow(e.RowHandle))
            {
                if (fieldName == "PagoFacturasInd")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    int index = this.NumFila;
                    this._pagoFacturasList[index].PagoFacturasInd.Value = value;

                    int chequeFin = Convert.ToInt32(this.txtChequeFin.Text);
                    if (value)
                    {
                        this.txtChequeFin.Text = (chequeFin + 1).ToString();
                        if (this._pagoFacturasActual.FindAll(p => !p.PagoFacturasInd.Value.HasValue || !(bool)p.PagoFacturasInd.Value).Count == 0)
                            this.chkSelectAll.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        this.txtChequeFin.Text = (chequeFin - 1).ToString();
                        this.chkSelectAll.CheckState = CheckState.Unchecked;
                    }

                    this.gcPagos.RefreshDataSource();
                } 
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (!this.gvDetallePagos.IsFilterRow(e.FocusedRowHandle))
                {
                    GridColumn col = this.gvDetallePagos.Columns[this._unboundPrefix + "Index"];
                    if (0 <= e.FocusedRowHandle && e.FocusedRowHandle < gvDetallePagos.RowCount)
                    {
                        this._indexFila = Convert.ToInt16(this.gvDetallePagos.GetRowCellValue(e.FocusedRowHandle, col));
                        var pagoFactura = this._pagoFacturasList.Find(p => p.Index == this._indexFila);
                        this.UpdateFooterGridData(pagoFactura);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Grilla gvDetalleFacturas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalleFacturas_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalleFacturas_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "ValorPago")
            {
                e.RepositoryItem = this.editValue;
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
                this.gvDetallePagos.PostEditor();

                bool isValid = this.ValidateData();
                if (isValid)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.GetPagoFacturas();
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "TBUpdate"));
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
                #region Paga las facturas
                List<DTO_PagoFacturas> facturasAPagar = this._pagoFacturasActual.FindAll(p => p.PagoFacturasInd.Value.HasValue && p.PagoFacturasInd.Value.Value);

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                this.results = _bc.AdministrationModel.PagoFacturas_RegistrarPagoFacturas(this._documentID, this._actFlujoID, facturasAPagar, 
                    this.dtFecha.DateTime.Date, this._areaFuncionalID);

                FormProvider.Master.StopProgressBarThread(this._documentID);
                #endregion
                #region Genera el reporte para el pago de factura

                if (this.results.Count > 0)
                {
                    List<DTO_TxResult> resultsNOK =new List<DTO_TxResult>();
                    foreach (DTO_TxResult NOK in this.results)
                    {
                        if(NOK.Result == ResultValue.NOK)
                            resultsNOK.Add(NOK);
                    }

                    MessageForm frm = new MessageForm(resultsNOK);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                    #region Pregunta si desea abrir los reportes
                    
                    //bool deseaImp = false;
                    //if (resultsNOK.Count != this.results.Count)
                    //{
                    //    string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                    //    var result = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //    if (result == DialogResult.Yes)
                    //        deseaImp = true;
                    //}

                    #endregion
                    //foreach (DTO_TxResult item in this.results)
                    //{
                    //    if (item.Result == ResultValue.OK)
                    //    {
                    //        int docReporte = !string.IsNullOrEmpty(this._bancoCuenta.DocumentoID.Value) ? Convert.ToInt32(this._bancoCuenta.DocumentoID.Value) : this._documentID;
                    //        reportName = this._bc.AdministrationModel.ReportesTesoreria_PagosFactura(docReporte, Convert.ToInt32(item.ExtraField), ExportFormatType.pdf);
                    //        if (deseaImp)
                    //        {
                    //            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(item.ExtraField), null, reportName.ToString());
                    //            Process.Start(fileURl);
                    //        }
                    //    }
                    //}
                }

                #endregion

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFacturas.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        
        }

        #endregion        

       
    }
}
