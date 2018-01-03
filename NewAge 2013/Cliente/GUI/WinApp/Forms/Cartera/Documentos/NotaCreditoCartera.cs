using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;
using SentenceTransformer;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class NotaCreditoCartera : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private string libranzaID = string.Empty;
        private DTO_InfoCredito infoCartera = new DTO_InfoCredito();
        private DTO_ccCreditoDocu _credito = null;
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private List<DTO_NotaCreditoResumen> resumenComp = new List<DTO_NotaCreditoResumen>();
        private DTO_NotaCreditoResumen _rowCurrent = new DTO_NotaCreditoResumen();

        private bool validate = true;
        private DateTime fechaPerido;
        #endregion

        public NotaCreditoCartera()  : base()
        {
           // InitializeComponent();
        }

        public NotaCreditoCartera(string mod): base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.NotaCreditoCartera;
            this.frmModule = ModulesPrefix.cc;

            InitializeComponent();
            base.SetInitParameters();

            this.AddGridCols();

            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[0].Height = 145;
            this.tlSeparatorPanel.RowStyles[1].Height = 150;
            this.tlSeparatorPanel.RowStyles[2].Height = 250;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.gvCuotas.OptionsView.ColumnAutoWidth = true;
            this.gvComponentes.OptionsView.ColumnAutoWidth = true;
            //this.grpboxDetail.Dock = DockStyle.Fill;

            //Deshabilita los botones +- de la grilla cuotas
            this.gcCuotas.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcCuotas.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Carga la Informacion del Header
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            _bc.InitMasterUC(this.masterReintegroSaldo, AppMasters.ccReintegroSaldo, true, true, true, false);
            this.fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this.dtPeriod.DateTime = this.fechaPerido;

            string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
            if (indCierreDiaStr == "1")
            {
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                    diaCierreStr = "1";            
            }        
        }

        ///// <summary>
        ///// Agrega las columnas a la grilla superior
        ///// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Grilla Principal Componentes
                //ComponenteCarteraID
                GridColumn ComponenteCarteraID = new GridColumn();
                ComponenteCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                ComponenteCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                ComponenteCarteraID.UnboundType = UnboundColumnType.String;
                ComponenteCarteraID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ComponenteCarteraID.AppearanceCell.Options.UseTextOptions = true;
                ComponenteCarteraID.ColumnEdit = base.editBtnGrid;
                ComponenteCarteraID.VisibleIndex = 0;
                ComponenteCarteraID.Width = 100;
                ComponenteCarteraID.Visible = true;
                ComponenteCarteraID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ComponenteCarteraID);

                //Descripcion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 1;
                Descripcion.Width = 200;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                //VlrSaldoActual
                GridColumn VlrSaldoActual = new GridColumn();
                VlrSaldoActual.FieldName = this.unboundPrefix + "VlrSaldoActual";
                VlrSaldoActual.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldoActual");
                VlrSaldoActual.UnboundType = UnboundColumnType.Decimal;
                VlrSaldoActual.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoActual.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoActual.VisibleIndex = 3;
                VlrSaldoActual.Width = 130;
                VlrSaldoActual.Visible = true;
                VlrSaldoActual.OptionsColumn.AllowEdit = false;
                VlrSaldoActual.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(VlrSaldoActual);

                //VlrNotaCredito
                GridColumn VlrNotaCredito = new GridColumn();
                VlrNotaCredito.FieldName = this.unboundPrefix + "VlrNotaCredito";
                VlrNotaCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNotaCredito");
                VlrNotaCredito.UnboundType = UnboundColumnType.Decimal;
                VlrNotaCredito.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrNotaCredito.AppearanceCell.Options.UseTextOptions = true;
                VlrNotaCredito.VisibleIndex = 4;
                VlrNotaCredito.Width = 130;
                VlrNotaCredito.Visible = true;
                VlrNotaCredito.OptionsColumn.AllowEdit = false;
                VlrNotaCredito.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(VlrNotaCredito);

                //VlrNuevoSaldo
                GridColumn VlrNuevoSaldo = new GridColumn();
                VlrNuevoSaldo.FieldName = this.unboundPrefix + "VlrNuevoSaldo";
                VlrNuevoSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNuevoSaldo");
                VlrNuevoSaldo.UnboundType = UnboundColumnType.Decimal;
                VlrNuevoSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrNuevoSaldo.AppearanceCell.Options.UseTextOptions = true;
                VlrNuevoSaldo.VisibleIndex = 5;
                VlrNuevoSaldo.Width = 130;
                VlrNuevoSaldo.Visible = true;
                VlrNuevoSaldo.OptionsColumn.AllowEdit = false;
                VlrNuevoSaldo.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(VlrNuevoSaldo);    
                #endregion                         
                #region Grilla Cuotas
                //CuotaID
                GridColumn CuotaID = new GridColumn();
                CuotaID.FieldName = this.unboundPrefix + "CuotaID";
                CuotaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaID");
                CuotaID.UnboundType = UnboundColumnType.String;
                CuotaID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                CuotaID.AppearanceCell.Options.UseTextOptions = true;
                CuotaID.VisibleIndex = 0;
                CuotaID.Width = 80;
                CuotaID.Visible = true;
                CuotaID.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(CuotaID);

                //FechaCuota
                GridColumn FechaCuota = new GridColumn();
                FechaCuota.FieldName = this.unboundPrefix + "FechaCuota";
                FechaCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCuota");
                FechaCuota.UnboundType = UnboundColumnType.DateTime;
                FechaCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FechaCuota.AppearanceCell.Options.UseTextOptions = true;
                FechaCuota.VisibleIndex = 1;
                FechaCuota.Width = 80;
                FechaCuota.Visible = true;
                FechaCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(FechaCuota);

                //SaldoActual
                GridColumn VlrSaldo = new GridColumn();
                VlrSaldo.FieldName = this.unboundPrefix + "VlrSaldo";
                VlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldo");
                VlrSaldo.UnboundType = UnboundColumnType.Decimal;
                VlrSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldo.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldo.VisibleIndex = 2;
                VlrSaldo.Width = 200;
                VlrSaldo.Visible = true;
                VlrSaldo.OptionsColumn.AllowEdit = false;
                VlrSaldo.ColumnEdit = this.editSpin0;
                this.gvCuotas.Columns.Add(VlrSaldo);

                //VlrAbonos
                GridColumn VlrAbonos = new GridColumn();
                VlrAbonos.FieldName = this.unboundPrefix + "VlrAbonos";
                VlrAbonos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrAbonos");
                VlrAbonos.UnboundType = UnboundColumnType.Decimal;
                VlrAbonos.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrAbonos.AppearanceCell.Options.UseTextOptions = true;
                VlrAbonos.VisibleIndex = 3;
                VlrAbonos.Width = 200;
                VlrAbonos.Visible = true;
                VlrAbonos.OptionsColumn.AllowEdit = false;
                VlrAbonos.ColumnEdit = this.editSpin0;
                this.gvCuotas.Columns.Add(VlrAbonos);

                //VlrNotaCredito
                GridColumn VlrNotaCreditoCuota = new GridColumn();
                VlrNotaCreditoCuota.FieldName = this.unboundPrefix + "VlrNotaCredito";
                VlrNotaCreditoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNotaCredito");
                VlrNotaCreditoCuota.UnboundType = UnboundColumnType.Decimal;
                VlrNotaCreditoCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrNotaCreditoCuota.AppearanceCell.Options.UseTextOptions = true;
                VlrNotaCreditoCuota.VisibleIndex = 4;
                VlrNotaCreditoCuota.Width = 110;
                VlrNotaCreditoCuota.Visible = true;
                VlrNotaCreditoCuota.OptionsColumn.AllowEdit = false;
                VlrNotaCreditoCuota.ColumnEdit = this.editSpin0;
                this.gvCuotas.Columns.Add(VlrNotaCreditoCuota);

                //NuevoSaldo
                GridColumn NuevoSaldoCuota = new GridColumn();
                NuevoSaldoCuota.FieldName = this.unboundPrefix + "VlrNuevoSaldo";
                NuevoSaldoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNuevoSaldo");
                NuevoSaldoCuota.UnboundType = UnboundColumnType.Decimal;
                NuevoSaldoCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                NuevoSaldoCuota.AppearanceCell.Options.UseTextOptions = true;
                NuevoSaldoCuota.VisibleIndex = 5;
                NuevoSaldoCuota.Width = 110;
                NuevoSaldoCuota.Visible = true;
                NuevoSaldoCuota.OptionsColumn.AllowEdit = false;
                NuevoSaldoCuota.ColumnEdit = this.editSpin0;
                this.gvCuotas.Columns.Add(NuevoSaldoCuota);
                #endregion                         
                #region Grilla Detalle Cuotas(Componentes)
                //ComponenteCarteraIDDet
                GridColumn ComponenteCarteraIDDet = new GridColumn();
                ComponenteCarteraIDDet.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                ComponenteCarteraIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                ComponenteCarteraIDDet.UnboundType = UnboundColumnType.String;
                ComponenteCarteraIDDet.VisibleIndex = 0;
                ComponenteCarteraIDDet.Width = 80;
                ComponenteCarteraIDDet.Visible = true;
                ComponenteCarteraIDDet.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(ComponenteCarteraIDDet);

                //Descriptivo
                GridColumn DescriptivoDet = new GridColumn();
                DescriptivoDet.FieldName = this.unboundPrefix + "Descriptivo";
                DescriptivoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                DescriptivoDet.UnboundType = UnboundColumnType.String;
                DescriptivoDet.VisibleIndex = 1;
                DescriptivoDet.Width = 80;
                DescriptivoDet.Visible = true;
                DescriptivoDet.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(DescriptivoDet);

                //CuotaSaldo
                GridColumn CuotaSaldo = new GridColumn();
                CuotaSaldo.FieldName = this.unboundPrefix + "CuotaSaldo";
                CuotaSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaSaldo");
                CuotaSaldo.UnboundType = UnboundColumnType.Decimal;
                CuotaSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CuotaSaldo.AppearanceCell.Options.UseTextOptions = true;
                CuotaSaldo.VisibleIndex = 2;
                CuotaSaldo.Width = 200;
                CuotaSaldo.Visible = true;
                CuotaSaldo.OptionsColumn.AllowEdit = false;
                CuotaSaldo.ColumnEdit = this.editSpin0;
                this.gvComponentes.Columns.Add(CuotaSaldo);

                //VlrAbonos
                GridColumn VlrAbonosDet = new GridColumn();
                VlrAbonosDet.FieldName = this.unboundPrefix + "AbonoValor";
                VlrAbonosDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrAbonos");
                VlrAbonosDet.UnboundType = UnboundColumnType.Decimal;
                VlrAbonosDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrAbonosDet.AppearanceCell.Options.UseTextOptions = true;
                VlrAbonosDet.VisibleIndex = 3;
                VlrAbonosDet.Width = 200;
                VlrAbonosDet.Visible = true;
                VlrAbonosDet.OptionsColumn.AllowEdit = false;
                VlrAbonosDet.ColumnEdit = this.editSpin0;
                this.gvComponentes.Columns.Add(VlrAbonosDet);

                //VlrNotaCredito
                GridColumn VlrNotaCreditoDet = new GridColumn();
                VlrNotaCreditoDet.FieldName = this.unboundPrefix + "VlrNotaCredito";
                VlrNotaCreditoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNotaCredito");
                VlrNotaCreditoDet.UnboundType = UnboundColumnType.Decimal;
                VlrNotaCreditoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrNotaCreditoDet.AppearanceCell.Options.UseTextOptions = true;
                VlrNotaCreditoDet.VisibleIndex = 4;
                VlrNotaCreditoDet.Width = 110;
                VlrNotaCreditoDet.Visible = true;
                VlrNotaCreditoDet.OptionsColumn.AllowEdit = false;
                VlrNotaCreditoDet.ColumnEdit = this.editSpin0;
                this.gvComponentes.Columns.Add(VlrNotaCreditoDet);

                //VlrNuevoSaldoDet
                GridColumn VlrNuevoSaldoDet= new GridColumn();
                VlrNuevoSaldoDet.FieldName = this.unboundPrefix + "VlrNuevoSaldo";
                VlrNuevoSaldoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNuevoSaldo");
                VlrNuevoSaldoDet.UnboundType = UnboundColumnType.Decimal;
                VlrNuevoSaldoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrNuevoSaldoDet.AppearanceCell.Options.UseTextOptions = true;
                VlrNuevoSaldoDet.VisibleIndex = 5;
                VlrNuevoSaldoDet.Width = 110;
                VlrNuevoSaldoDet.Visible = true;
                VlrNuevoSaldoDet.OptionsColumn.AllowEdit = false;
                VlrNuevoSaldoDet.ColumnEdit = this.editSpin0;
                this.gvComponentes.Columns.Add(VlrNuevoSaldoDet);
                #endregion                         
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            try
            {
                base.AfterInitialize();
                if (this.fechaPerido.Month == DateTime.Now.Date.Month)
                {
                    this.dtFecha.DateTime = DateTime.Now.Date;
                    this.dtFechaAplica.DateTime = DateTime.Now.Date;
                    this.dtFechaDoc.DateTime = DateTime.Now.Date;
                }
                else
                {
                    this.dtFecha.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
                    this.dtFechaAplica.DateTime = this.dtFecha.DateTime;
                    this.dtFechaDoc.DateTime = this.dtFecha.DateTime;
                }

                //Pone la fecha de consignacion con base a la del periodo
                this.dtFechaDoc.Properties.MaxValue = this.dtFecha.DateTime;
                if (this.dtFechaAplica.DateTime.Day == 31)
                    this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);
               
                this.dtFechaAplica.DateTimeChanged += new System.EventHandler(this.dtFechaAplica_DateTimeChanged);
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "AfterInitialize"));
            }

        }

        #endregion

        #region Funciones Privadas
        
        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;
            this.libranzaID = string.Empty;
            this.masterCliente.Value = String.Empty;
            this.masterReintegroSaldo.Value = string.Empty;
            this.txtLibranza.EditValue = string.Empty;
            this.txtValorCuota.EditValue = 0;
            this.txtObservacion.Text = string.Empty;
            this.infoCartera.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.infoCartera.SaldosComponentes = new List<DTO_ccSaldosComponentes>();
            this.gcDocument.DataSource = this.infoCartera.PlanPagos;
            this.gcCuotas.DataSource = this.infoCartera.SaldosComponentes;
            this.resumenComp = new List<DTO_NotaCreditoResumen>();
            this._credito = new DTO_ccCreditoDocu();
            this.validate = true;
            this.txtLibranza.Focus();
        }

        /// <summary>
        /// Funcion para cargar la grilla de pagos 
        /// </summary>
        private void CalculateValuesNC()
        {
            try
            {
                decimal valorNC =  Convert.ToDecimal(this.txtVlrNC.EditValue, CultureInfo.InvariantCulture) - Convert.ToDecimal(this.txtVlrNCOtros.EditValue, CultureInfo.InvariantCulture);  //Valor Nota Credito
                this.txtVlrNCObligac.EditValue = valorNC;
                foreach (DTO_ccCreditoPlanPagos pp in this.infoCartera.PlanPagos) //Recorre los pagos
                {
                    foreach (DTO_ccSaldosComponentes c in pp.DetalleComp)
                    {
                        c.AbonoValor.Value = c.AbonoValor.Value ?? 0; //Valida que no sea null
                        decimal vlr = valorNC - c.AbonoValor.Value.Value;
                        if (vlr >= 0) //Valida que no sea negativo
                        {
                            c.VlrNotaCredito.Value = c.AbonoValor.Value;
                            c.VlrNuevoSaldo.Value = c.CuotaSaldo.Value + c.VlrNotaCredito.Value;
                            valorNC -= c.AbonoValor.Value.Value; //Va descontando del Valor Nota Credito
                        }
                        else
                        {
                            c.VlrNotaCredito.Value = valorNC;
                            valorNC -= valorNC;
                            c.VlrNuevoSaldo.Value = c.CuotaSaldo.Value + c.VlrNotaCredito.Value;
                           // break;
                        }
                    }
                    pp.VlrNotaCredito.Value = pp.DetalleComp.Sum(x => x.VlrNotaCredito.Value);
                    pp.VlrNuevoSaldo.Value = pp.DetalleComp.Sum(x => x.VlrNuevoSaldo.Value);

                    //if (valorNC <= 0)
                    //    break;
                }

                #region Llena Grilla Principal de Comp
                foreach (var r in this.resumenComp)
                {
                    r.VlrNotaCredito.Value = 0;
                    foreach (var p in this.infoCartera.PlanPagos)
                    {
                        r.VlrNotaCredito.Value += p.DetalleComp.FindAll(x => x.ComponenteCarteraID.Value == r.ComponenteCarteraID.Value).Sum(x => x.VlrNotaCredito.Value);
                    }
                    r.VlrNuevoSaldo.Value = r.VlrSaldoActual.Value + r.VlrNotaCredito.Value;
                }

                #endregion

                this.gcCuotas.RefreshDataSource();
                this.gcDocument.RefreshDataSource();
                this.gvDocument.PostEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "CalculateValuesNC"));
            }
        }

        /// <summary>
        /// Agrega un nuevo registro a la grilla de componentes
        /// </summary>
        private void AddComponente()
        {
            try
            {
                DTO_NotaCreditoResumen newComp = new DTO_NotaCreditoResumen();
                newComp.ComponenteCarteraID.Value = string.Empty;
                newComp.Descripcion.Value = string.Empty;
                newComp.NumDocCredito.Value = _credito.NumeroDoc.Value;
                newComp.NumeroDoc.Value = null;
                newComp.VlrNotaCredito.Value = 0;
                newComp.VlrSaldoActual.Value = 0;
                newComp.VlrNuevoSaldo.Value = 0;
                newComp.ComponenteAdicionalInd.Value = true;
                this.resumenComp.Add(newComp);

                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "AddComponente"));
            }
        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        protected override bool ValidateRow(int fila)
        {
            try
            {
                this.gvDocument.PostEditor();
                this.isValid = true;

                if (fila >= 0)
                {
                    bool rowValid = true;
                    string fieldName;

                    #region ComponenteCarteraID

                    rowValid = true;
                    fieldName = "ComponenteCarteraID";
                    GridColumn colFinanciera = this.gvDocument.Columns[this.unboundPrefix + fieldName];

                    //Valida el componente
                    rowValid = _bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, fieldName, false, true, false, AppMasters.ccCarteraComponente);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colFinanciera, string.Empty);
                    else
                        this.isValid = false;

                    #endregion                    
                    #region VlrNotaCredito
                    rowValid = true;
                    fieldName = "VlrNotaCredito";
                    GridColumn colVlrCuota = this.gvDocument.Columns[this.unboundPrefix + fieldName];

                    //Valida que tenga valores positivos y no sea cero
                    rowValid = this._bc.ValidGridCellValue(this.gvDocument, this.unboundPrefix, fila, fieldName, false, true, true, false);
                    if (rowValid)
                        this.gvDocument.SetColumnError(colVlrCuota, string.Empty);
                    else
                        this.isValid = false;

                    #endregion                    
                }
                return this.isValid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "ValidateRow_CompraCartera"));
                return this.isValid = false;
            }
        }

        /// <summary>
        /// Carga informacion del doc
        /// </summary>
        protected void LoadInfo(DTO_glDocumentoControl ctrl)
        {
            try
            {
                if (ctrl == null && this._credito != null)
                {
                    var ctrlCredito = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._credito.NumeroDoc.Value.Value);
                    DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this._credito.ClienteID.Value, true);
                    #region Load DocumentoControl
                    this._ctrl.EmpresaID.Value = this.empresaID;
                    this._ctrl.ComprobanteID.Value = string.Empty;
                    this._ctrl.ComprobanteIDNro.Value = 0;
                    this._ctrl.TerceroID.Value = cliente.TerceroID.Value;
                    this._ctrl.MonedaID.Value = ctrlCredito.MonedaID.Value;
                    this._ctrl.CuentaID.Value = string.Empty;
                    this._ctrl.ProyectoID.Value = ctrlCredito.ProyectoID.Value; 
                    this._ctrl.CentroCostoID.Value = ctrlCredito.CentroCostoID.Value; 
                    this._ctrl.LugarGeograficoID.Value = ctrlCredito.LugarGeograficoID.Value; 
                    this._ctrl.LineaPresupuestoID.Value = ctrlCredito.LineaPresupuestoID.Value;
                    this._ctrl.Fecha.Value = DateTime.Now;
                    this._ctrl.PeriodoDoc.Value = base.dtPeriod.DateTime;
                    this._ctrl.FechaDoc.Value = this.dtFechaDoc.DateTime;
                    this._ctrl.PeriodoUltMov.Value = base.dtPeriod.DateTime;
                    this._ctrl.PrefijoID.Value = ctrlCredito.PrefijoID.Value;
                    this._ctrl.TasaCambioCONT.Value = ctrlCredito.TasaCambioCONT.Value;
                    this._ctrl.TasaCambioDOCU.Value = ctrlCredito.TasaCambioDOCU.Value;
                    this._ctrl.DocumentoNro.Value = 0;
                    this._ctrl.DocumentoID.Value = this.documentID;
                    this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                  
                    this._ctrl.seUsuarioID.Value = this.userID;
                    this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this._ctrl.ConsSaldo.Value = 0;
                    this._ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                 
                    this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
                    this._ctrl.Observacion.Value = this.txtObservacion.Text;
                    this._ctrl.Valor.Value = Convert.ToDecimal(this.txtVlrNCObligac.EditValue);
                    this._ctrl.Iva.Value = 0;
                    #endregion
                }
                else
                    this._ctrl = ctrl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "LoadTempHeader"));
            }
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
            base.Form_Enter(sender, e); 
            
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false; 
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que filtra una lista de DTO_ccCreditoDocu de acuerdo al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                //{
                //    if (this.masterCliente.ValidID)
                //        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                //    else
                //        this.cliente = null;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que carga la grilla del cliente de acuerdo a la libranza seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranzas_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this.txtLibranza.Text) && (this.libranzaID != this.txtLibranza.Text))
                {                   
                    this.libranzaID = this.txtLibranza.Text;
                    this._credito = this._bc.AdministrationModel.GetCreditoByLibranza(Convert.ToInt32(this.txtLibranza.Text));
                    if (this._credito != null)
                    {
                        Dictionary<string,string> componentes = new Dictionary<string,string>();
                        this.resumenComp = new List<DTO_NotaCreditoResumen>();
                        this.libranzaID = this.txtLibranza.Text;
                        this.masterCliente.Value =  this._credito.ClienteID.Value;
                        this.txtValorCuota.EditValue =  this._credito.VlrCuota.Value;
                        this.infoCartera = this._bc.AdministrationModel.GetInfoCredito(this._credito.NumeroDoc.Value.Value,DateTime.Now);
                        this.infoCartera.PlanPagos = this.infoCartera.PlanPagos.FindAll(x => x.VlrPagadoCuota.Value != 0).ToList();
                        this.infoCartera.SaldosComponentes = this.infoCartera.SaldosComponentes.FindAll(x => x.CuotaSaldo.Value != x.CuotaInicial.Value).ToList();
                        DTO_InfoPagos pagos =  this._bc.AdministrationModel.GetInfoPagos(this._credito.NumeroDoc.Value.Value);
                        if (this.infoCartera.PlanPagos.Count > 0)
                        {
                            #region Llena Grilla de Cuotas
                            foreach (var pp in this.infoCartera.PlanPagos)
                            {
                                #region LLena grilla de Componentes por Cuota
                                pp.VlrAbonos.Value = 0;
                                pp.VlrNotaCredito.Value = 0;                               
                                pp.DetalleComp = this.infoCartera.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value);
                                foreach (var comp in pp.DetalleComp)
                                {
                                    comp.NumDocCredito.Value = this._credito.NumeroDoc.Value;
                                    comp.AbonoValor.Value = comp.CuotaInicial.Value - comp.CuotaSaldo.Value;
                                    comp.VlrNuevoSaldo.Value = comp.CuotaSaldo.Value;
                                    if (!componentes.ContainsKey(comp.ComponenteCarteraID.Value))
                                        componentes.Add(comp.ComponenteCarteraID.Value,comp.Descriptivo.Value);
                                }
                                pp.VlrSaldo.Value = pp.VlrCuota.Value - pagos.CreditoPagos.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(y=>y.Valor.Value);
                                pp.VlrAbonos.Value = pp.DetalleComp.Sum(x => x.AbonoValor.Value);
                                pp.VlrNuevoSaldo.Value = pp.VlrSaldo.Value + pp.VlrNotaCredito.Value;
                                #endregion
                            }
                            this.infoCartera.PlanPagos = this.infoCartera.PlanPagos.OrderByDescending(x => x.CuotaID.Value).ToList();
                            this.gcCuotas.DataSource = this.infoCartera.PlanPagos; 
                            #endregion

                            #region Llena Grilla Principal de Comp
                            foreach (var res in componentes)
                            {
                                DTO_NotaCreditoResumen r = new DTO_NotaCreditoResumen();
                                r.ComponenteCarteraID.Value = res.Key;
                                r.Descripcion.Value = res.Value;
                                r.NumDocCredito.Value = _credito.NumeroDoc.Value;
                                r.VlrSaldoActual.Value = 0;
                                foreach (var p in this.infoCartera.PlanPagos)
                                {
                                    r.VlrSaldoActual.Value += p.DetalleComp.FindAll(x => x.ComponenteCarteraID.Value == res.Key).Sum(x => x.CuotaSaldo.Value);
                                }
                                r.VlrNotaCredito.Value = 0;
                                r.VlrNuevoSaldo.Value = r.VlrSaldoActual.Value + r.VlrNotaCredito.Value;
                                this.resumenComp.Add(r);
                            }
                            this.gcDocument.DataSource = this.resumenComp;
                            this.gcDocument.RefreshDataSource();
                            this.gcCuotas.RefreshDataSource();
                            this.gvDocument.PostEditor();   
                            #endregion                          
                        }
                        else
                        {
                            this._credito = new DTO_ccCreditoDocu();
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldos);
                            MessageBox.Show(msg);
                            this.CleanData();
                           
                        }
                    }
                    else
                        this.CleanData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "lkp_Libranzas_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            }

            this.dtFechaDoc.Properties.MaxValue = this.dtFecha.DateTime;
            this.dtFechaDoc.DateTime = base.dtFecha.DateTime;

            //Pone la fecha de aplica con base a la del periodo
            this.dtFechaAplica.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
            if (this.dtFechaAplica.DateTime.Day == 31)
                this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);

            //Carga la infor del crédito
            this.txtLibranzas_Leave(sender, e);
        }

        /// <summary>
        /// Evento que asigna el valor maximo de la fecha de consignacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaAplica_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                //this.dtFechaAplica.DateTimeChanged -= new System.EventHandler(this.dtFechaAplica_DateTimeChanged);

                //this.dtFechaAplica.DateTime = new DateTime(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month, DateTime.DaysInMonth(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month));
                //if (this.dtFechaAplica.DateTime.Day == 31)
                //    this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);

                //this.dtFechaAplica.DateTimeChanged += new System.EventHandler(this.dtFechaAplica_DateTimeChanged);

                //this.dtFechaDoc.Properties.MaxValue = this.dtFechaAplica.DateTime;
                //this.dtFechaDoc.DateTime = this.dtFechaDoc.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "dtFechaAplica_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Evento al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNC_EditValueChanged(object sender, EventArgs e)
        {
            this.CalculateValuesNC();
        }
        #endregion

        #region Eventos Grilla Documentos

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    this.ValidateRow(fila);
                    if (this.isValid)
                    {
                        this.AddComponente();
                        this.gvDocument.FocusedRowHandle = this.resumenComp.Count - 1;
                    }
                }

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    if (fila >= 0 && this._rowCurrent.ComponenteAdicionalInd.Value.Value)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.isValid = true;
                            this.deleteOP = true;
                            this.resumenComp.RemoveAt(fila);
                            this.gcDocument.RefreshDataSource();
                            if (this.resumenComp.Count > 0)
                                this.gvDocument.FocusedRowHandle = fila - 1;
                            this.deleteOP = false;
                        }

                        e.Handled = true;
                    }
                    else
                        e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "gcDetail_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
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
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
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
        }

        /// <summary>
        /// Evento q valida antes de dejar el row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ComponenteCarteraID")
            {
                DTO_ccCarteraComponente dtoComp = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, e.Value.ToString(), true);
                this._rowCurrent.Descripcion.Value = dtoComp.Descriptivo.Value;
                this.gvDocument.RefreshRow(e.RowHandle);    
            
                GridColumn componente = this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"];
                if (this.resumenComp.Count(x => x.ComponenteCarteraID.Value == this._rowCurrent.ComponenteCarteraID.Value) >= 2)
                {
                    this.gvDocument.SetColumnError(componente, "Este componente ya existe, verifique nuevamente");
                    this.isValid = false;
                }
                else
                {
                    this.gvDocument.SetColumnError(componente, string.Empty);
                    this.isValid = true;
                }
                  
            }
            else if (fieldName == "VlrNotaCredito" && this._rowCurrent.ComponenteAdicionalInd.Value.Value)
            {
                if (this.infoCartera.PlanPagos.Count > 0)
                {
                    DTO_ccCreditoPlanPagos pp = this.infoCartera.PlanPagos.First();
                    if (!pp.DetalleComp.Exists(x => x.ComponenteCarteraID.Value == this._rowCurrent.ComponenteCarteraID.Value))
                    {
                        //Si no existe lo agrega
                        DTO_ccCarteraComponente dtoComp = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, this._rowCurrent.ComponenteCarteraID.Value, true);
                        DTO_ccSaldosComponentes compAdic = new DTO_ccSaldosComponentes();
                        compAdic.ComponenteCarteraID.Value = this._rowCurrent.ComponenteCarteraID.Value;
                        compAdic.Descriptivo.Value = this._rowCurrent.Descripcion.Value;
                        compAdic.PagoTotalInd.Value = true;
                        compAdic.ComponenteFijo.Value = true;
                        compAdic.TipoComponente.Value = dtoComp.TipoComponente.Value;
                        compAdic.Editable.Value = true;
                        compAdic.NumDocCredito.Value = this._credito.NumeroDoc.Value;
                        compAdic.CuotaID.Value = pp.CuotaID.Value;
                        compAdic.AbonoValor.Value = 0;
                        compAdic.AbonoSaldo.Value = 0;
                        compAdic.TotalInicial.Value = 0;
                        compAdic.TotalSaldo.Value = 0;
                        compAdic.CuotaSaldo.Value = 0;
                        compAdic.CuotaInicial.Value = this._rowCurrent.VlrNotaCredito.Value;
                        compAdic.TipoPago.Value = 7;
                        compAdic.VlrNotaCredito.Value = this._rowCurrent.VlrNotaCredito.Value;
                        compAdic.VlrNuevoSaldo.Value = this._rowCurrent.VlrNotaCredito.Value;
                        pp.DetalleComp.Add(compAdic);
                    }
                    else
                    {
                        //Si existe lo actualiza
                        DTO_ccSaldosComponentes compAdic = pp.DetalleComp.Find(X=>X.ComponenteCarteraID.Value == this._rowCurrent.ComponenteCarteraID.Value);
                        compAdic.VlrNotaCredito.Value = this._rowCurrent.VlrNotaCredito.Value;
                        compAdic.VlrNuevoSaldo.Value = this._rowCurrent.VlrNotaCredito.Value;
                    }
                    pp.VlrNotaCredito.Value = pp.DetalleComp.Sum(x => x.VlrNotaCredito.Value);
                    pp.VlrNuevoSaldo.Value = pp.DetalleComp.Sum(x => x.VlrNuevoSaldo.Value);
                }
                this._rowCurrent.VlrNuevoSaldo.Value = this._rowCurrent.VlrSaldoActual.Value + this._rowCurrent.VlrNotaCredito.Value;
                this.txtVlrNCOtros.EditValue = this.resumenComp.FindAll(x=>x.ComponenteAdicionalInd.Value.Value).Sum(x => x.VlrNotaCredito.Value);
                this.gvDocument.RefreshRow(e.RowHandle);
                this.gcCuotas.RefreshDataSource();
            }
           
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {           
            #region ComponenteCarteraID

            GridColumn componente = this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"];
            if (this.resumenComp.Count(x => x.ComponenteCarteraID.Value == this._rowCurrent.ComponenteCarteraID.Value) >= 2)
            {
                this.gvDocument.SetColumnError(componente, "Este componente ya existe, verifique nuevamente");
                this.isValid = false;
                e.Allow = false;
            }
            else
              this.isValid = true;

            #endregion                    
        }

        /// <summary>
        /// Evento q valida al salir de un Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this._rowCurrent = (DTO_NotaCreditoResumen)this.gvDocument.GetRow(e.FocusedRowHandle);
                if (this._rowCurrent != null && this._rowCurrent.ComponenteAdicionalInd.Value.Value)
                {
                    this.gvDocument.Columns[this.unboundPrefix + "VlrNotaCredito"].OptionsColumn.AllowEdit = true;
                    this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    this.gvDocument.Columns[this.unboundPrefix + "VlrNotaCredito"].OptionsColumn.AllowEdit = false;
                    this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
                }
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "gldocuments_focusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Grilla Cuotas
        /// <summary>
        /// Evento q valida al salir de un Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCuotas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                //int row = e.FocusedRowHandle;
                //DTO_ccCreditoPlanPagos pp = (DTO_ccCreditoPlanPagos)this.gvCuotas.GetRow(row);
                //if (pp != null)
                //{
                //    var det = this.infoCartera.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).ToList();

                //    det.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                //    this.gcCuotas.DataSource = det;
                //    //this.CalcularTotal_Componentes();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "gldocuments_focusedRowChanged"));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.txtLibranza.Focus();
                this.gvDocument.PostEditor();
                this.gvCuotas.PostEditor();
                this.LoadInfo(null);

                if (!this.masterReintegroSaldo.ValidID)
                    MessageBox.Show("Debe digitar un Reintegro Saldo");
                else if (Convert.ToDecimal(this.txtVlrNCObligac.EditValue) == 0)
                    MessageBox.Show("El valor de la Nota Crédito debe ser diferente de $0");
                else if (this.isValid)
                {                                    
                    #region Guarda la info
                    DTO_SerializedObject result = this._bc.AdministrationModel.NotaCredito_Add(this.documentID, this.resumenComp, this.infoCartera, this._ctrl, this.masterReintegroSaldo.Value);
                    
                    bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);
                    if (result.GetType() != typeof(DTO_TxResult))
                    {
                        #region Variables para el mail

                        //DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                        //string body = string.Empty;
                        //string subject = string.Empty;
                        //string email = user.CorreoElectronico.Value;

                        //string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                        //string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                        //string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                        #endregion
                        #region Envia el correo
                        //subject = string.Format(subjectApr, formName);
                        //body = string.Format(bodyApr, formName, this._credito.Libranza.Value, this.cliente.Descriptivo.Value, string.Empty);
                        //_bc.SendMail(this.documentID, subject, body, email);
                        #endregion   
                        this.CleanData();
                        this.txtLibranza.Focus();
                    }
                    
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCartera.cs", "TBSave"));
            }
        }

        #endregion
    }
}
