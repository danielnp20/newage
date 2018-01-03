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
using System.Diagnostics;
using System.Net;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Resultados;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ShowDocumentForm : Form
    {
        #region Variables

		BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _docCtrl;
        private DTO_Comprobante _comprobante;
        private bool[] _loadedDataInd = new bool[7];
        private List<int> _regenDocs;

        private int _documentID;
        private ModulesPrefix _mod;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private string _tab = "\t";
        private string _unboundPrefix = "Unbound_";
        private bool _firstTime = true;

        private List<DTO_glDocAnexoControl> anexos;

	    #endregion

        /// <summary>
        /// Constructor con un documento
        /// </summary>
        /// <param name="doc"></param>
        public ShowDocumentForm(DTO_glDocumentoControl doc, DTO_Comprobante compr)
        {
            this._docCtrl = doc;
            this._comprobante = compr;

            this._documentID = AppForms.ConsultaDocumentos;
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);
            this.InitControls();
            this.InitTabs();
            this.AddGridCols();

            #region Verificar PDF

            //Carga la info de los archivos pdf que se pueden regenerar
            this._regenDocs = new List<int>();
            //Cartera
            this._regenDocs.Add(AppDocuments.LiquidacionCredito);
            this._regenDocs.Add(AppDocuments.RecaudosManuales);
            this._regenDocs.Add(AppDocuments.RecaudosMasivos);
            this._regenDocs.Add(AppDocuments.PagosTotales);
            //CXP
            this._regenDocs.Add(AppDocuments.CausarFacturas);
            this._regenDocs.Add(AppDocuments.NotaCreditoCxP);
            //Tesoreria
            this._regenDocs.Add(AppDocuments.DesembolsoFacturas);
            this._regenDocs.Add(AppDocuments.TransferenciasBancarias);
            this._regenDocs.Add(AppDocuments.ReciboCaja);
            //CXP
            this._regenDocs.Add(AppDocuments.FacturaVenta);
            //Facturacion
            this._regenDocs.Add(AppDocuments.FacturaVenta);
            //Inventarios
            this._regenDocs.Add(AppDocuments.TransaccionManual);
            this._regenDocs.Add(AppDocuments.TransaccionAutomatica);
            this._regenDocs.Add(AppDocuments.NotaEnvio);            

            //Analiza la url para mostrar o no el boton de PDF
            try
            {
                string url = _bc.UrlDocumentFile(TipoArchivo.Documentos, this._docCtrl.NumeroDoc.Value.Value, null);

                WebRequest req = WebRequest.Create(url);
                WebResponse res = req.GetResponse();
                this.btnPdf.Visible = true;
            }
            catch (WebException ex)
            {
                this.btnPdf.Visible = false;
                if (this._regenDocs.Contains(this._docCtrl.DocumentoID.Value.Value))
                    this.btnRegenPdf.Visible = true;

            }

            #endregion

            for (int i = 0; i < 7; i++)
                this._loadedDataInd[i] = false;

            if (compr != null)
            {
                this.LoadPageData(1);
                this.tp_comprobante.PageVisible = true;
                this.tc_GetDocument.SelectedTabPageIndex = 1;
            }
            else
            {
                this.LoadPageData(0);
                this.tc_GetDocument.SelectedTabPageIndex = 0;
            }
            this._firstTime = false;


            DTO_glDocumento document = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this._docCtrl.DocumentoID.Value.Value.ToString(), true);
            this._mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), document.ModuloID.Value.ToLower());
        } 

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las pestañas
        /// </summary>
        private void InitTabs()
        {
            this.tp_comprobante.PageVisible = false;
            this.tp_detalle.PageVisible = false;
            this.tp_masInfo.PageVisible = false;
        }

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            #region DocumentoControl
            _bc.InitMasterUC(this.uc_DocCtrl_MasterDocumentID, AppMasters.glDocumento, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterAreaFuncional, AppMasters.glAreaFuncional, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterPrefixId, AppMasters.glPrefijo, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasteModena, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterComprobante, AppMasters.coComprobante, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterProject, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterTercero, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterLineaPresupuesto, AppMasters.plLineaPresupuesto, true, true, true, false);
            _bc.InitMasterUC(this.uc_DocCtrl_MasterLugarGeografico, AppMasters.glLugarGeografico, true, true, true, false);

            this.uc_DocCtrl_MasterDocumentID.EnableControl(false);
            this.uc_DocCtrl_MasterAreaFuncional.EnableControl(false);
            this.uc_DocCtrl_MasterPrefixId.EnableControl(false);
            this.uc_DocCtrl_MasteModena.EnableControl(false);
            this.uc_DocCtrl_MasterComprobante.EnableControl(false);
            this.uc_DocCtrl_MasterProject.EnableControl(false);
            this.uc_DocCtrl_MasterCuenta.EnableControl(false);
            this.uc_DocCtrl_MasterTercero.EnableControl(false);
            this.uc_DocCtrl_MasterCentroCosto.EnableControl(false);
            this.uc_DocCtrl_MasterLineaPresupuesto.EnableControl(false);
            this.uc_DocCtrl_MasterLugarGeografico.EnableControl(false);
            #endregion
            #region Comprobante
            _bc.InitMasterUC(this.uc_Comp_MasterComprobante, AppMasters.coComprobante, true, true, true, false);
            _bc.InitMasterUC(this.uc_Comp_MasterMoneda, AppMasters.glMoneda, true, true, true, false);

            this.uc_Comp_MasterComprobante.EnableControl(false);
            this.uc_Comp_MasterMoneda.EnableControl(false);
            #endregion
            #region Anexos
            this.anexos = null;
            #endregion
            #region Saldos

            this.uc_Saldos_PerEditPeriodo.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            
            #endregion
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadPageData(int page)
        {
            switch (page)
            {
                case 0:
                    if (!this._loadedDataInd.ElementAt(0))
                    {
                        #region Documento Control

                        //Fechas
                        this.txtFechaDoc.Text = _docCtrl.FechaDoc.Value != null ? _docCtrl.FechaDoc.Value.Value.ToString(FormatString.ControlDate) : string.Empty;
                        this.txtFechaCreacion.Text = _docCtrl.Fecha.Value.ToString();
                        this.txtPeriodoDoc.Text = _docCtrl.PeriodoDoc.Value != null ? _docCtrl.PeriodoDoc.Value.Value.ToString(FormatString.ControlDate) : string.Empty;
                        //Datos Generales
                        EstadoDocControl est = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), this._docCtrl.Estado.Value.Value.ToString());
                        this.txtEstado.Text = _bc.GetResource(LanguageTypes.Tables, "tbl_estate_" + est.ToString().ToLower());
                        this.uc_DocCtrl_MasterDocumentID.Value = _docCtrl.DocumentoID.Value.Value.ToString();
                        this.uc_DocCtrl_MasterAreaFuncional.Value = _docCtrl.AreaFuncionalID.Value;
                        this.uc_DocCtrl_MasterPrefixId.Value = _docCtrl.PrefijoID.Value;
                        this.txtNumeroDoc.Text = _docCtrl.NumeroDoc.Value.ToString();
                        this.txtNumDocPadre.Text = _docCtrl.DocumentoPadre.Value.ToString();
                        this.txtDocumentoNro.Text = _docCtrl.DocumentoNro.Value != null ? _docCtrl.DocumentoNro.Value.ToString() : string.Empty;
                        this.uc_DocCtrl_MasteModena.Value = _docCtrl.MonedaID.Value;
                        this.uc_DocCtrl_MasterComprobante.Value = _docCtrl.ComprobanteID.Value;
                        this.txtComprobanteIDNro.Text = _docCtrl.ComprobanteIDNro.Value != null ? _docCtrl.ComprobanteIDNro.Value.ToString() : string.Empty;
                        this.uc_DocCtrl_MasterTercero.Value = _docCtrl.TerceroID.Value;
                        this.txtDocumentoTercero.Text = _docCtrl.DocumentoTercero.Value;
                        this.uc_DocCtrl_MasterCuenta.Value = _docCtrl.CuentaID.Value;
                        this.uc_DocCtrl_MasterProject.Value = _docCtrl.ProyectoID.Value;
                        this.uc_DocCtrl_MasterCentroCosto.Value = _docCtrl.CentroCostoID.Value;
                        this.uc_DocCtrl_MasterLineaPresupuesto.Value = _docCtrl.LineaPresupuestoID.Value;
                        this.uc_DocCtrl_MasterLugarGeografico.Value = _docCtrl.LugarGeograficoID.Value;
                        this.txtObservacion.Text = _docCtrl.Observacion.Value;
                        this.txtDescripcion.Text = _docCtrl.Descripcion.Value;
                        this.txtseUsuarioID.Text = this._bc.AdministrationModel.seUsuario_GetUserByReplicaID(this._docCtrl.seUsuarioID.Value.Value) != null? this._bc.AdministrationModel.seUsuario_GetUserByReplicaID(this._docCtrl.seUsuarioID.Value.Value).Descriptivo.Value : string.Empty;
                        //Valores
                        this.txtValor.Text =  _docCtrl.Valor.Value != null ? _docCtrl.Valor.Value.ToString() : "0"; 
                        this.txtIVA.Text =  _docCtrl.Iva.Value != null ? _docCtrl.Iva.Value.ToString() : "0"; 
                        this.txtTasaCambioDOCU.Text = _docCtrl.TasaCambioDOCU.Value != null ? _docCtrl.TasaCambioDOCU.Value.ToString() : "0";
                        this.txtTasaCambioCONT.Text = _docCtrl.TasaCambioCONT.Value != null ? _docCtrl.TasaCambioCONT.Value.ToString() : "0";
                        #endregion
                    }
                    break;
                case 1:
                    if (!this._loadedDataInd.ElementAt(1))
                    {
                        #region Comprobante

                        DTO_ComprobanteHeader header = this._comprobante.Header;
                        this.uc_Comp_MasterComprobante.Value = header.ComprobanteID.Value;
                        this.uc_Comp_MasterMoneda.Value = header.MdaTransacc.Value;
                        this.txt_comp_fecha.Text = header.Fecha.Value.Value.ToString(FormatString.ControlDate);
                        this.txt_comp_NroComp.Text = header.ComprobanteNro.Value.Value.ToString();
                        this.txt_comp_Periodo.Text = header.PeriodoID.Value.Value.ToString(FormatString.ControlDate);
                        this.txt_comp_TCBase.Text = header.TasaCambioBase.Value.Value.ToString();
                        this.txt_comp_TCOtr.Text = header.TasaCambioOtr.Value.Value.ToString();

                        TipoMoneda tipoMda = (TipoMoneda)Enum.Parse(typeof(TipoMoneda), header.MdaOrigen.Value.Value.ToString());
                        switch (tipoMda)
                        {
                            case TipoMoneda.Local:
                                this.txt_comp_MdaOrigen.Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal);
                                break;
                            case TipoMoneda.Foreign:
                                this.txt_comp_MdaOrigen.Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign);
                                break;
                            case TipoMoneda.Both:
                                this.txt_comp_MdaOrigen.Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth);
                                break;
                        }

                        this.gcComprobante.DataSource = this._comprobante.Footer;
                        #endregion
                    }
                    break;
                case 2:
                    break;
                case 3:
                    if (!this._loadedDataInd.ElementAt(3))
                    {
                        #region Anexos
                        try
                        {
                            this.anexos = _bc.AdministrationModel.glDocAnexoControl_GetAnexosByNumeroDoc(this._docCtrl.NumeroDoc.Value.Value);
                            this.gcAnexos.DataSource = this.anexos;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "LoadPageData"));
                        }
                        #endregion
                    }

                    break;
                case 4:
                    if (!this._loadedDataInd.ElementAt(4))
                    {
                        #region Saldos
                        try
                        {
                            string perStr = _bc.GetControlValueByCompany(this._mod, AppControl.co_Periodo);
                            this.uc_Saldos_PerEditPeriodo.DateTime = Convert.ToDateTime(perStr);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "LoadPageData"));
                        }
                        #endregion
                    }
                    break;
                case 5:
                    if (!this._loadedDataInd.ElementAt(5))
                    {
                        #region Bitacora
                        try
                        {
                            var bitacora = _bc.AdministrationModel.glActividadControl_Get(this._docCtrl.NumeroDoc.Value.Value).ToList();
                            bitacora = bitacora.OrderByDescending(x => x.Fecha.Value).ToList();
                            this.gcBitacora.DataSource = bitacora;
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "LoadPageData"));
                        }
                        #endregion
                    }
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default:
                    break;
            }
            this._loadedDataInd[page] = true;
        }

        /// <summary>
        /// Calcula los saldos de un documento
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se van a calcular los saldos</param>
        /// <returns></returns>
        private void CalcularSaldo(DateTime periodo)
        {
            try
            {
                List<DTO_BitacoraSaldo> compList = _bc.AdministrationModel.Comprobante_GetByIdentificadorTR(periodo, this._docCtrl.NumeroDoc.Value.Value);
                this.gcSaldosDetail.DataSource = compList;

                this.txt_saldos_tercero.Text = _docCtrl.TerceroID.Value.ToString();
                this.txt_saldos_document.Text = _docCtrl.DocumentoID.Value.Value.ToString();
                this.txt_saldos_cuenta.Text = _docCtrl.CuentaID.Value.ToString();

                //Si no es multimoneda oculta los valores extranjeros
                if (!_bc.AdministrationModel.MultiMoneda)
                {
                    this.lblMdaExtName.Visible = false;
                    this.txtSaldoME.Visible = false;
                }

                if (!string.IsNullOrEmpty(_docCtrl.TerceroID.Value))
                {
                    DTO_coTercero terceroInfo = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, _docCtrl.TerceroID.Value, false);
                    this.txt_saldos_terceroDesc.Text = ((DTO_coTercero)terceroInfo).Descriptivo.Value;
                }
                else
                    this.txt_saldos_terceroDesc.Text = string.Empty;

                string concSaldo = string.Empty;
                if (!string.IsNullOrEmpty(_docCtrl.CuentaID.Value))
                {
                    DTO_coPlanCuenta _cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, this._docCtrl.CuentaID.Value, false);
                    concSaldo = _cta.ConceptoSaldoID.Value;
                    this.txt_saldos_cuentaDesc.Text = _cta.Descriptivo.Value;

                    #region Trae los saldos
                    DTO_coCuentaSaldo _ctaSaldoDTO = _bc.AdministrationModel.Saldo_GetByDocumento(_cta.ID.Value, concSaldo, _docCtrl.NumeroDoc.Value.Value, string.Empty);
                    if (_ctaSaldoDTO != null)
                    {
                        string msg = string.Empty;

                        this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                        this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                        bool isML = this._docCtrl.MonedaID.Value == _monedaLocal ? true : false;

                        msg = _bc.GetResource(LanguageTypes.Forms, "1011_monedaLocal") + ": (" + _monedaLocal + ")" + Environment.NewLine;
                        this.lblMdaLocName.Text = _monedaLocal;

                        //Trae el valor de las monedas
                        decimal sumaML = _ctaSaldoDTO.DbOrigenLocML.Value.Value + _ctaSaldoDTO.DbOrigenExtML.Value.Value + _ctaSaldoDTO.CrOrigenLocML.Value.Value + _ctaSaldoDTO.CrOrigenExtML.Value.Value +
                            _ctaSaldoDTO.DbSaldoIniLocML.Value.Value + _ctaSaldoDTO.DbSaldoIniExtML.Value.Value + _ctaSaldoDTO.CrSaldoIniLocML.Value.Value + _ctaSaldoDTO.CrSaldoIniExtML.Value.Value;

                        msg += _tab + _bc.GetResource(LanguageTypes.Forms, "1011_saldo") + _tab + " $ " + sumaML;
                        this.txtSaldoML.Text = " $ " + Math.Round(sumaML, 2);

                        //Si es multimoneda muestras los valores extranjeros
                        if (_bc.AdministrationModel.MultiMoneda)
                        {
                            msg += Environment.NewLine + Environment.NewLine;
                            msg += _bc.GetResource(LanguageTypes.Forms, "1011_monedaExtranjera") + ": (" + _monedaExtranjera + ")" + Environment.NewLine;
                            this.lblMdaExtName.Text = _monedaExtranjera;

                            decimal sumaME = _ctaSaldoDTO.DbOrigenLocME.Value.Value + _ctaSaldoDTO.DbOrigenExtME.Value.Value + _ctaSaldoDTO.CrOrigenLocME.Value.Value + _ctaSaldoDTO.CrOrigenExtME.Value.Value
                                + _ctaSaldoDTO.DbSaldoIniLocME.Value.Value + _ctaSaldoDTO.DbSaldoIniExtME.Value.Value + _ctaSaldoDTO.CrSaldoIniLocME.Value.Value + _ctaSaldoDTO.CrSaldoIniExtME.Value.Value;
                            msg += _bc.GetResource(LanguageTypes.Forms, "1011_saldo") + " $ " + sumaME + Environment.NewLine;

                            this.txtSaldoME.Text = " $ " + Math.Round(sumaME, 2);
                        }
                        else
                        {
                            this.lblMdaExtName.Visible = false;
                            this.txtSaldoME.Visible = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DocNotBalance));

                        this.txtSaldoML.Text = " $ 0";
                        this.txtSaldoME.Text = " $ 0";
                    }
                    #endregion
                }
                else
                {
                    this.txt_saldos_cuentaDesc.Text = string.Empty;
                    this.txtSaldoML.Text = " $ 0";
                    this.txtSaldoME.Text = " $ 0";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Comprobante
                int docComprobante = AppDocuments.ComprobanteManual;

                #region Pks
                //Cuenta
                GridColumn cuenta = new GridColumn();
                cuenta.FieldName = this._unboundPrefix + "CuentaID";
                cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_CuentaID"); ;
                cuenta.UnboundType = UnboundColumnType.String;
                cuenta.VisibleIndex = 1;
                cuenta.Width = 70;
                cuenta.Visible = true;
                cuenta.Fixed = FixedStyle.Left;
                cuenta.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(cuenta);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this._unboundPrefix + "TerceroID";
                tercero.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_TerceroID");
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 2;
                tercero.Width = 70;
                tercero.Visible = true;
                tercero.Fixed = FixedStyle.Left;
                tercero.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(tercero);

                //Prefijo
                GridColumn pref = new GridColumn();
                pref.FieldName = this._unboundPrefix + "PrefijoCOM";
                pref.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_PrefijoCOM");
                pref.UnboundType = UnboundColumnType.String;
                pref.VisibleIndex = 3;
                pref.Width = 70;
                pref.Visible = true;
                pref.Fixed = FixedStyle.Left;
                pref.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(pref);

                //Documento
                GridColumn doc = new GridColumn();
                doc.FieldName = this._unboundPrefix + "DocumentoCOM";
                doc.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_DocumentoCOM");
                doc.UnboundType = UnboundColumnType.String;
                doc.VisibleIndex = 4;
                doc.Width = 70;
                doc.Visible = true;
                doc.Fixed = FixedStyle.Left;
                doc.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(doc);

                #endregion
                #region Columnas extras

                //Activo
                GridColumn act = new GridColumn();
                act.FieldName = this._unboundPrefix + "ActivoCOM";
                act.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_ActivoCOM");
                act.UnboundType = UnboundColumnType.String;
                act.VisibleIndex = 5;
                act.Width = 100;
                act.Visible = true;
                act.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(act);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this._unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 6;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this._unboundPrefix + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 7;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(ctoCosto);

                //Linea Presupuestal
                GridColumn linPresup = new GridColumn();
                linPresup.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                linPresup.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_LineaPresupuestoID");
                linPresup.UnboundType = UnboundColumnType.String;
                linPresup.VisibleIndex = 8;
                linPresup.Width = 100;
                linPresup.Visible = true;
                linPresup.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(linPresup);

                //Concepto cargo
                GridColumn concCargo = new GridColumn();
                concCargo.FieldName = this._unboundPrefix + "ConceptoCargoID";
                concCargo.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_ConceptoCargoID");
                concCargo.UnboundType = UnboundColumnType.String;
                concCargo.VisibleIndex = 9;
                concCargo.Width = 100;
                concCargo.Visible = true;
                concCargo.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(concCargo);

                //Lugar Geografico
                GridColumn lg = new GridColumn();
                lg.FieldName = this._unboundPrefix + "LugarGeograficoID";
                lg.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_LugarGeograficoID");
                lg.UnboundType = UnboundColumnType.String;
                lg.VisibleIndex = 10;
                lg.Width = 100;
                lg.Visible = true;
                lg.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(lg);

                //Valor Base ML
                GridColumn vlrBaseML = new GridColumn();
                vlrBaseML.FieldName = this._unboundPrefix + "vlrBaseML";
                vlrBaseML.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_vlrBaseML");
                vlrBaseML.UnboundType = UnboundColumnType.Decimal;
                vlrBaseML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrBaseML.AppearanceCell.Options.UseTextOptions = true;
                vlrBaseML.VisibleIndex = 11;
                vlrBaseML.Width = 120;
                vlrBaseML.Visible = true;
                vlrBaseML.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(vlrBaseML);

                //Valor Moneda local
                GridColumn vlrMdaLoc = new GridColumn();
                vlrMdaLoc.FieldName = this._unboundPrefix + "vlrMdaLoc";
                vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_vlrMdaLoc");
                vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
                vlrMdaLoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrMdaLoc.AppearanceCell.Options.UseTextOptions = true;
                vlrMdaLoc.VisibleIndex = 12;
                vlrMdaLoc.Width = 150;
                vlrMdaLoc.Visible = true;
                vlrMdaLoc.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(vlrMdaLoc);

                //Tasa de Cambio
                GridColumn tasaCambio = new GridColumn();
                tasaCambio.FieldName = this._unboundPrefix + "TasaCambio";
                tasaCambio.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_lblExchangeRate");
                tasaCambio.UnboundType = UnboundColumnType.Decimal;
                tasaCambio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                tasaCambio.AppearanceCell.Options.UseTextOptions = true;
                tasaCambio.VisibleIndex = 13;
                tasaCambio.Width = 100;
                tasaCambio.Visible = _bc.AdministrationModel.MultiMoneda;
                tasaCambio.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(tasaCambio);

                //Valor Base ME
                GridColumn vlrBaseME = new GridColumn();
                vlrBaseME.FieldName = this._unboundPrefix + "vlrBaseME";
                vlrBaseME.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_vlrBaseME");
                vlrBaseME.UnboundType = UnboundColumnType.Decimal;
                vlrBaseME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrBaseME.AppearanceCell.Options.UseTextOptions = true;
                vlrBaseME.VisibleIndex = 14;
                vlrBaseME.Width = 120;
                vlrBaseME.Visible = _bc.AdministrationModel.MultiMoneda;
                vlrBaseME.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(vlrBaseME);

                //Valor Moneda Ext
                GridColumn vlrMdaExt = new GridColumn();
                vlrMdaExt.FieldName = this._unboundPrefix + "vlrMdaExt";
                vlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_vlrMdaExt");
                vlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                vlrMdaExt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrMdaExt.AppearanceCell.Options.UseTextOptions = true;
                vlrMdaExt.VisibleIndex = 15;
                vlrMdaExt.Width = 150;
                vlrMdaExt.Visible = _bc.AdministrationModel.MultiMoneda;
                vlrMdaExt.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(vlrMdaExt);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this._unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 16;
                desc.Width = 100;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvComprobante.Columns.Add(desc);

                //ViewDoc
                GridColumn ViewDocument = new GridColumn();
                ViewDocument.FieldName = this._unboundPrefix + "ViewDoc";
                ViewDocument.Caption = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
                ViewDocument.UnboundType = UnboundColumnType.String;
                ViewDocument.VisibleIndex = 17;
                ViewDocument.Width = 80;
                ViewDocument.OptionsColumn.ShowCaption = false;
                this.gvComprobante.Columns.Add(ViewDocument);

                //Concepto Saldo
                GridColumn cs = new GridColumn();
                cs.FieldName = this._unboundPrefix + "ConceptoSaldoID";
                cs.UnboundType = UnboundColumnType.String;
                cs.Visible = false;
                this.gvComprobante.Columns.Add(cs);

                //IdentificadorTR
                GridColumn iTR = new GridColumn();
                iTR.FieldName = this._unboundPrefix + "IdentificadorTR";
                iTR.UnboundType = UnboundColumnType.String;
                iTR.Visible = false;
                this.gvComprobante.Columns.Add(iTR);

                #endregion
                #endregion
                #region Anexos
                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 0;
                descriptivo.Width = 450;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvAnexos.Columns.Add(descriptivo);

                //Ver
                GridColumn ver = new GridColumn();
                ver.FieldName = this._unboundPrefix + "Ver";
                ver.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ver");
                ver.UnboundType = UnboundColumnType.String;
                ver.VisibleIndex = 1;
                ver.Width = 150;
                ver.ColumnEdit = this.linkVer;
                this.gvAnexos.Columns.Add(ver);
                this.gvAnexos.OptionsView.ColumnAutoWidth = true;
                #endregion
                #region Saldos

                //ComprobanteID
                GridColumn sCompID = new GridColumn();
                sCompID.FieldName = this._unboundPrefix + "ComprobanteID";
                sCompID.Caption = _bc.GetResource(LanguageTypes.Forms, docComprobante + "_ComprobanteID"); ;
                sCompID.UnboundType = UnboundColumnType.String;
                sCompID.VisibleIndex = 1;
                sCompID.Width = 70;
                sCompID.Visible = true;
                sCompID.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sCompID);

                //ComprobanteNro
                GridColumn sCompNro = new GridColumn();
                sCompNro.FieldName = this._unboundPrefix + "ComprobanteNro";
                sCompNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComprobanteNro"); ;
                sCompNro.UnboundType = UnboundColumnType.String;
                sCompNro.VisibleIndex = 2;
                sCompNro.Width = 70;
                sCompNro.Visible = true;
                sCompNro.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sCompNro);

                //Fecha
                GridColumn sFecha = new GridColumn();
                sFecha.FieldName = this._unboundPrefix + "Fecha";
                sFecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                sFecha.UnboundType = UnboundColumnType.DateTime;
                sFecha.VisibleIndex = 3;
                sFecha.Width = 150;
                sFecha.Visible = true;
                sFecha.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sFecha);

                //Descriptivo
                GridColumn sDesc = new GridColumn();
                sDesc.FieldName = this._unboundPrefix + "Descriptivo";
                sDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                sDesc.UnboundType = UnboundColumnType.String;
                sDesc.VisibleIndex = 4;
                sDesc.Width = 150;
                sDesc.Visible = true;
                sDesc.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sDesc);

                //Valor Moneda local
                GridColumn sVlrMdaLoc = new GridColumn();
                sVlrMdaLoc.FieldName = this._unboundPrefix + "vlrMdaLoc";
                sVlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_vlrMdaLoc");
                sVlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
                sVlrMdaLoc.VisibleIndex = 5;
                sVlrMdaLoc.Width = 150;
                sVlrMdaLoc.Visible = true;
                sVlrMdaLoc.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sVlrMdaLoc);

                //Valor Moneda Ext
                GridColumn sVlrMdaExt = new GridColumn();
                sVlrMdaExt.FieldName = this._unboundPrefix + "vlrMdaExt";
                sVlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_vlrMdaExt");
                sVlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                sVlrMdaExt.VisibleIndex = 6;
                sVlrMdaExt.Width = 150;
                sVlrMdaExt.Visible = _bc.AdministrationModel.MultiMoneda;
                sVlrMdaExt.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sVlrMdaExt);

                #endregion
                #region Bitacora

                //ActividadFlujoID
                GridColumn tareaId = new GridColumn();
                tareaId.FieldName = this._unboundPrefix + "ActividadFlujoID";
                tareaId.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoID");
                tareaId.UnboundType = UnboundColumnType.String;
                tareaId.VisibleIndex = 0;
                tareaId.Width = 50;
                tareaId.Visible = true;
                tareaId.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(tareaId);

                //Descriptivo
                GridColumn tarea = new GridColumn();
                tarea.FieldName = this._unboundPrefix + "Descriptivo";
                tarea.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                tarea.UnboundType = UnboundColumnType.String;
                tarea.VisibleIndex = 1;
                tarea.Width = 120;
                tarea.Visible = true;
                tarea.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(tarea);

                //Usuario
                GridColumn usuario = new GridColumn();
                usuario.FieldName = this._unboundPrefix + "UsuarioDesc";
                usuario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Usuario");
                usuario.UnboundType = UnboundColumnType.String;
                usuario.VisibleIndex = 2;
                usuario.Width = 70;
                usuario.Visible = true;
                usuario.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(usuario);

                //Periodo
                GridColumn periodo = new GridColumn();
                periodo.FieldName = this._unboundPrefix + "Periodo";
                periodo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Periodo");
                periodo.UnboundType = UnboundColumnType.String;
                periodo.VisibleIndex = 3;
                periodo.Width = 70;
                periodo.Visible = true;
                periodo.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(periodo);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.String;
                fecha.VisibleIndex = 5;
                fecha.Width = 150;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(fecha);

                //observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this._unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.VisibleIndex = 6;
                observacion.Width = 170;
                observacion.Visible = true;
                observacion.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(observacion);

                //AlarmaInd
                GridColumn alarmaInd = new GridColumn();
                alarmaInd.FieldName = this._unboundPrefix + "AlarmaInd";
                alarmaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_AlarmaInd");
                alarmaInd.UnboundType = UnboundColumnType.String;
                alarmaInd.VisibleIndex = 7;
                alarmaInd.Width = 50;
                alarmaInd.Visible = true;
                alarmaInd.OptionsColumn.AllowEdit = false;
                this.gvBitacora.Columns.Add(alarmaInd);

                #endregion               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "AddGridCols"));
            }
        }

        #endregion

        #region Eventos

        #region Documento

        /// <summary>
        /// Cambio de pestaña. Valida si ya se cargó la información
        /// </summary>
        /// <param name="sender">OBjeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void tc_GetDocument_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int index = tc_GetDocument.SelectedTabPageIndex;
            if (!this._loadedDataInd[index])
                this.LoadPageData(index);
        }

        /// <summary>
        /// Abre el documento pdf
        /// </summary>
        /// <param name="sender">OBjeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnPdf_Click(object sender, EventArgs e)
        {
            Process.Start(_bc.UrlDocumentFile(TipoArchivo.Documentos, this._docCtrl.NumeroDoc.Value.Value, null));
        }

        /// <summary>
        /// Boton para regenerar un archivo pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegenPdf_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> extraParams = new List<string>();

                #region carga los parametros segun el reporte

                switch (this._docCtrl.DocumentoID.Value.Value)
                {
                    case AppDocuments.LiquidacionCredito:
                        extraParams.Add(this._docCtrl.DocumentoTercero.Value);
                        break;
                }

                #endregion

                RegenReports rep = new RegenReports();
                rep.GenerarReporte(this._docCtrl.DocumentoID.Value.Value, this._docCtrl.NumeroDoc.Value.Value, extraParams, this._docCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado? true : false);

                this.btnPdf.Visible = true;
                this.btnRegenPdf.Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "btnRegenPdf_Click"));
            }
        }

        #endregion

        #region Comprobantes
        
        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvComprobante_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "vlrBaseML" || fieldName == "vlrBaseME" || fieldName == "vlrMdaLoc" || fieldName == "vlrMdaExt")
            {
                e.RepositoryItem = this.editValue;
            }
            else if (fieldName == "TasaCambio")
            {
                e.RepositoryItem = this.editValue4;
            }
            if (fieldName == "ViewDoc")
            {
                DTO_ComprobanteFooter row = (DTO_ComprobanteFooter)this.gvComprobante.GetRow(e.RowHandle);
                if (row != null)
                {
                    if (row.IdentificadorTR.Value.HasValue && row.IdentificadorTR.Value != 0 && this._comprobante.Header.NumeroDoc.Value != row.IdentificadorTR.Value)
                        e.RepositoryItem = this.linkVer;
                }

            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvComprobante_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        }

        /// <summary>
        /// Funcion que se ejecuta al hacer doble click sobre la info de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvComprobante_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                GridHitInfo info = view.CalcHitInfo(this.gcComprobante.PointToClient(MousePosition));
                if (info.HitTest != GridHitTest.Column)
                {
                    string msgTitleData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
                    string msgGetData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GetDocument);

                    if (MessageBox.Show(msgGetData, msgTitleData, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (info.InRow || info.InRowCell)
                        {
                            int fila = info.RowHandle;
                            DTO_ComprobanteFooter footer = this._comprobante.Footer[fila];
                            string cSaldo = footer.ConceptoSaldoID.Value;
                            DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cSaldo, false);
                            SaldoControl saldoControl = (SaldoControl)Enum.Parse(typeof(SaldoControl), saldo.coSaldoControl.Value.Value.ToString());

                            switch (saldoControl)
                            {
                                case SaldoControl.Cuenta:
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocCta));
                                    break;
                                case SaldoControl.Doc_Interno:
                                    DTO_glDocumentoControl docCtrlInt = _bc.AdministrationModel.glDocumentoControl_GetByID(Convert.ToInt32(footer.IdentificadorTR.Value.Value));

                                    if (docCtrlInt == null)
                                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                                    else
                                        new ShowDocumentForm(docCtrlInt, null).Show();
                                    break;
                                case SaldoControl.Doc_Externo:
                                    DTO_glDocumentoControl docCtrlExt = _bc.AdministrationModel.glDocumentoControl_GetByID(Convert.ToInt32(footer.IdentificadorTR.Value.Value));

                                    if (docCtrlExt == null)
                                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                                    else
                                        new ShowDocumentForm(docCtrlExt, null).Show();
                                    break;
                                case SaldoControl.Componente_Tercero:
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocTer));
                                    break;
                                case SaldoControl.Componente_Activo:
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocAct));
                                    break;
                                case SaldoControl.Componente_Documento:
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocDoc));
                                    break;
                                case SaldoControl.Inventario:
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocInv));
                                    break;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "gvComprobante_DoubleClick"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta al cambiar la fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvComprobante_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            DTO_ComprobanteFooter row = (DTO_ComprobanteFooter)this.gvComprobante.GetRow(e.FocusedRowHandle);
            if (row != null)
            {
                DTO_MasterBasic dto;
                dto = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, row.CuentaID.Value, true);
                this.txtCuentaDesc.EditValue = dto != null? dto.Descriptivo.Value : string.Empty;

                dto = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, row.TerceroID.Value, true);
                this.txtTerceroDesc.EditValue = dto != null ? dto.Descriptivo.Value : string.Empty;
                
                dto = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, row.ProyectoID.Value, true);
                this.txtProyectoDesc.EditValue = dto != null ? dto.Descriptivo.Value : string.Empty;

                dto = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, row.CentroCostoID.Value, true);
                this.txtCentroDesc.EditValue = dto != null ? dto.Descriptivo.Value : string.Empty;

                dto = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, row.LineaPresupuestoID.Value, true);
                this.txtLineaDesc.EditValue = dto != null ? dto.Descriptivo.Value : string.Empty;

                dto = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coConceptoCargo, false, row.ConceptoCargoID.Value, true);
                this.txtConceptoDesc.EditValue = dto != null ? dto.Descriptivo.Value : string.Empty;
            }
        }

        #endregion

        #region Anexos

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvAnexos_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
           
            if (fieldName == "Ver")
                e.DisplayText = string.IsNullOrWhiteSpace(this.anexos[e.ListSourceRowIndex].ArchivoNombre.Value) ? string.Empty : _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ver");
            else if (fieldName == "ViewDoc")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }

        /// <summary>
        /// Evento que se ejecuta para ver un anexo existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkVer_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tc_GetDocument.SelectedTabPageIndex == 3) //Tab de Anexos
                {
                    DTO_glDocAnexoControl anexo = this.anexos[this.gvAnexos.FocusedRowHandle];

                    string fileFormat = _bc.GetControlValue(AppControl.NombreAnexoDocumento);
                    string fileName = this._mod.ToString() + "&" + anexo.ArchivoNombre.Value;

                    string url = _bc.UrlDocumentFile(TipoArchivo.AnexosDocumentos, null, null, fileName);

                    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    Process.Start(sInfo); 
                }
                if (this.tc_GetDocument.SelectedTabPageIndex == 1) //Tab de Comprobante
                {
                    int fila = this.gvComprobante.FocusedRowHandle;

                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();

                    DTO_ComprobanteFooter rowCurrent = this._comprobante.Footer[fila];
                    int numDoc = rowCurrent.IdentificadorTR.Value.HasValue && rowCurrent.IdentificadorTR.Value != 0 ? Convert.ToInt32(rowCurrent.IdentificadorTR.Value.Value) : this._comprobante.Header.NumeroDoc.Value.Value;
                    if (numDoc != this._docCtrl.NumeroDoc.Value)
                    {
                        ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(numDoc);
                        comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                        ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                        documentForm.Show(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "gvAnexos_FocusedRowChanged"));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblInfoAnexo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Type frm = typeof(glDocumentoAnexo);
                FormProvider.GetInstance(frm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "lblInfoAnexo_LinkClicked"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditAnexo_Click(object sender, EventArgs e)
        {
            try
            {
                AnexosDocumentos anexos = new AnexosDocumentos(this._docCtrl.NumeroDoc.Value.Value, this._mod);
                anexos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "btnEditAnexo_Click"));
            }
        }

        /// <summary>
        /// Actualiza anexos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateAnexos_Click(object sender, EventArgs e)
        {
            #region Actualiza Anexos
            try
            {
                this.anexos = _bc.AdministrationModel.glDocAnexoControl_GetAnexosByNumeroDoc(this._docCtrl.NumeroDoc.Value.Value);
                this.gcAnexos.DataSource = this.anexos;
                this.gcAnexos.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "LoadPageData"));
            }
            #endregion
        }        

        #endregion

        #region Saldos

        /// <summary>
        /// Cambio los saldos dependiendo del periodo
        /// </summary>
        private void uc_Saldos_PerEditPeriodo_ValueChanged()
        {
            if(!this._firstTime)
                this.CalcularSaldo(this.uc_Saldos_PerEditPeriodo.DateTime);
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvSaldosDetail_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "vlrMdaLoc" || fieldName == "vlrMdaExt")
                e.RepositoryItem = this.editValue;
        }

        #endregion

        #region Bitacora

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBitacora_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                try
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        #region Propiedad
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            if (fieldName != "Observacion")
                                e.Value = pi.GetValue(dto, null);
                            else
                            {
                                string val = pi.GetValue(dto, null).ToString();
                                string[] obs = val.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                                if (obs.Count() <= 1)
                                    e.Value = _bc.GetResource(LanguageTypes.Messages, val);
                                else
                                {
                                    string rsx = _bc.GetResource(LanguageTypes.Messages, obs[0]);
                                    e.Value = string.Format(rsx, obs[1]);
                                }
                            }
                        }
                        else
                        {
                            if (fieldName != "Observacion")
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                            else
                            {
                                string val = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null).ToString();

                                string[] obs = val.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                                if (obs.Count() <= 1)
                                    e.Value = _bc.GetResource(LanguageTypes.Messages, val);
                                else
                                {
                                    string rsx = _bc.GetResource(LanguageTypes.Messages, obs[0]);
                                    e.Value = string.Format(rsx, obs[1]);
                                }
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        #region Campo
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                if (fieldName != "Observacion")
                                    e.Value = fi.GetValue(dto);
                                else
                                {
                                    string val = fi.GetValue(dto).ToString();
                                    string[] obs = val.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (obs.Count() == 1)
                                        e.Value = _bc.GetResource(LanguageTypes.Messages, val);
                                    else
                                    {
                                        string rsx = _bc.GetResource(LanguageTypes.Messages, obs[0]);
                                        e.Value = string.Format(rsx, obs[1]);
                                    }
                                }
                            }
                            else
                            {
                                if (fieldName != "Observacion")
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                                else
                                {
                                    string val = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null).ToString();
                                    string[] obs = val.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (obs.Count() == 1)
                                        e.Value = _bc.GetResource(LanguageTypes.Messages, val);
                                    else
                                    {
                                        string rsx = _bc.GetResource(LanguageTypes.Messages, obs[0]);
                                        e.Value = string.Format(rsx, obs[1]);
                                    }
                                }

                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBitacora_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "AlarmaInd")
            {
                e.RepositoryItem = this.editCheck;
            }
        }

        #endregion
    

        #endregion
    }
}
