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
    public partial class SaldosLibranza : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private DTO_ccCreditoDocu credito = new DTO_ccCreditoDocu();

        //Variable glControl
        private string componenteCapital = string.Empty;     
        private string componenteInteres = string.Empty;
        private string componenteSeguro = string.Empty;
        private string componenteIntSeguro = string.Empty;
        private string componentePrejuridico = string.Empty;
        private string componenteGastos = string.Empty;
        private string componenteUsura = string.Empty;
        private string componenteMora = string.Empty;

        //Variables formulario
        private bool validate = true;
        private string clienteID = string.Empty;


        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SaldosLibranza()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SaldosLibranza(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public void Constructor(string mod = null)
        {
            this.InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranza.cs", "SaldosLibranza.cs-SaldosLibranza"));
            }
        }     
     
        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QuerySaldosLibranza;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);         
          
            //Carga los componentes de glControl
            this.componenteCapital = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
            this.componenteMora = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
            this.componenteUsura = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteUsura);
            this.componenteInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
            this.componenteSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
            this.componenteIntSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);
            this.componentePrejuridico = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);

            //Carga los combos de la fecha
            this.dtFechaCorte.EditValue = DateTime.Now;
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Principal
                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this._unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 0;
                libranza.Width = 80;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvLibranzas.Columns.Add(libranza);

                //VlrSaldo
                GridColumn VlrSaldo = new GridColumn();
                VlrSaldo.FieldName = this._unboundPrefix + "VlrSaldo";
                VlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldo");
                VlrSaldo.UnboundType = UnboundColumnType.Integer;
                VlrSaldo.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                VlrSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldo.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldo.AppearanceCell.BackColor = Color.LightSteelBlue;
                VlrSaldo.AppearanceCell.Options.UseBackColor = true;
                VlrSaldo.VisibleIndex = 1;
                VlrSaldo.Width = 140;
                VlrSaldo.Visible = true;
                VlrSaldo.ColumnEdit = this.editValue;
                VlrSaldo.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSaldo.FieldName, "{0:c0}"); 
                VlrSaldo.OptionsColumn.AllowEdit = false;
                this.gvLibranzas.Columns.Add(VlrSaldo);

                //VlrSaldoVencido
                GridColumn VlrSaldoVencido = new GridColumn();
                VlrSaldoVencido.FieldName = this._unboundPrefix + "VlrSaldoVencido";
                VlrSaldoVencido.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoVencido");
                VlrSaldoVencido.UnboundType = UnboundColumnType.Integer;
                VlrSaldoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoVencido.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoVencido.AppearanceCell.BackColor = Color.SeaShell;
                VlrSaldoVencido.AppearanceCell.Options.UseBackColor = true;
                VlrSaldoVencido.VisibleIndex = 2;
                VlrSaldoVencido.Width = 140;
                VlrSaldoVencido.Visible = true;
                VlrSaldoVencido.ColumnEdit = this.editValue;
                VlrSaldoVencido.OptionsColumn.AllowEdit = false;
                VlrSaldoVencido.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSaldoVencido.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(VlrSaldoVencido);                

                //NumCuotas
                GridColumn plazo = new GridColumn();
                plazo.FieldName = this._unboundPrefix + "Plazo";
                plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                plazo.UnboundType = UnboundColumnType.Integer;
                plazo.VisibleIndex = 3;
                plazo.Width = 120;
                plazo.Visible = false;
                plazo.OptionsColumn.AllowEdit = false;
                this.gvLibranzas.Columns.Add(plazo);

                //Vlr Capital
                GridColumn capital = new GridColumn();
                capital.FieldName = this._unboundPrefix + "VlrCapital";
                capital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                capital.UnboundType = UnboundColumnType.Integer;
                capital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                capital.AppearanceCell.Options.UseTextOptions = true;
                capital.VisibleIndex = 4;
                capital.Width = 150;
                capital.Visible = true;
                capital.OptionsColumn.AllowEdit = false;
                capital.ColumnEdit = this.editValue;
                capital.Summary.Add(DevExpress.Data.SummaryItemType.Sum, capital.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(capital);

                //VlrInteres
                GridColumn vlrInteres = new GridColumn();
                vlrInteres.FieldName = this._unboundPrefix + "VlrInteres";
                vlrInteres.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrInteres");
                vlrInteres.UnboundType = UnboundColumnType.Integer;
                vlrInteres.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrInteres.AppearanceCell.Options.UseTextOptions = true;
                vlrInteres.VisibleIndex = 5;
                vlrInteres.Width = 150;
                vlrInteres.Visible = true;
                vlrInteres.ColumnEdit = this.editValue;
                vlrInteres.OptionsColumn.AllowEdit = false;
                vlrInteres.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrInteres.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(vlrInteres);

                //Vlr seguro
                GridColumn VlrSeguro = new GridColumn();
                VlrSeguro.FieldName = this._unboundPrefix + "VlrSeguro";
                VlrSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSeguro");
                VlrSeguro.UnboundType = UnboundColumnType.Integer;
                VlrSeguro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSeguro.AppearanceCell.Options.UseTextOptions = true;
                VlrSeguro.VisibleIndex = 6;
                VlrSeguro.Width = 150;
                VlrSeguro.Visible = true;
                VlrSeguro.OptionsColumn.AllowEdit = false;
                VlrSeguro.ColumnEdit = this.editValue;
                VlrSeguro.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSeguro.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(VlrSeguro);

                //Vlr VlrMora
                GridColumn VlrMora = new GridColumn();
                VlrMora.FieldName = this._unboundPrefix + "VlrMora";
                VlrMora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMora");
                VlrMora.UnboundType = UnboundColumnType.Integer;
                VlrMora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrMora.AppearanceCell.Options.UseTextOptions = true;
                VlrMora.VisibleIndex = 7;
                VlrMora.Width = 150;
                VlrMora.Visible = true;
                VlrMora.OptionsColumn.AllowEdit = false;
                VlrMora.ColumnEdit = this.editValue;
                VlrMora.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrMora.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(VlrMora);

                //Vlr Prejuridico
                GridColumn VlrPrejuridico = new GridColumn();
                VlrPrejuridico.FieldName = this._unboundPrefix + "VlrPrejuridico";
                VlrPrejuridico.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrejuridico");
                VlrPrejuridico.UnboundType = UnboundColumnType.Integer;
                VlrPrejuridico.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPrejuridico.AppearanceCell.Options.UseTextOptions = true;
                VlrPrejuridico.VisibleIndex = 8;
                VlrPrejuridico.Width = 150;
                VlrPrejuridico.Visible = true;
                VlrPrejuridico.OptionsColumn.AllowEdit = false;
                VlrPrejuridico.ColumnEdit = this.editValue;
                VlrPrejuridico.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrPrejuridico.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(VlrPrejuridico);

                //Vlr VlrSaldoOtros
                GridColumn VlrSaldoOtros = new GridColumn();
                VlrSaldoOtros.FieldName = this._unboundPrefix + "VlrSaldoOtros";
                VlrSaldoOtros.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoOtros");
                VlrSaldoOtros.UnboundType = UnboundColumnType.Integer;
                VlrSaldoOtros.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoOtros.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoOtros.VisibleIndex = 9;
                VlrSaldoOtros.Width = 150;
                VlrSaldoOtros.Visible = true;
                VlrSaldoOtros.OptionsColumn.AllowEdit = false;
                VlrSaldoOtros.ColumnEdit = this.editValue;
                VlrSaldoOtros.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSaldoOtros.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(VlrSaldoOtros);

                //Vlr VlrGastos
                GridColumn VlrGastos = new GridColumn();
                VlrGastos.FieldName = this._unboundPrefix + "VlrGastos";
                VlrGastos.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGastos");
                VlrGastos.UnboundType = UnboundColumnType.Integer;
                VlrGastos.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGastos.AppearanceCell.Options.UseTextOptions = true;
                VlrGastos.VisibleIndex = 10;
                VlrGastos.Width = 150;
                VlrGastos.Visible = true;
                VlrGastos.OptionsColumn.AllowEdit = false;
                VlrGastos.ColumnEdit = this.editValue;
                VlrGastos.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrGastos.FieldName, "{0:c0}"); 
                this.gvLibranzas.Columns.Add(VlrGastos);                 
                
                #endregion
                #region Datos Plan Pagos
                //CuotaID
                GridColumn cuotaID = new GridColumn();
                cuotaID.FieldName = this._unboundPrefix + "CuotaID";
                cuotaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaID");
                cuotaID.UnboundType = UnboundColumnType.Integer;
                cuotaID.VisibleIndex = 0;
                cuotaID.Width = 80;
                cuotaID.Visible = true;
                cuotaID.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(cuotaID);

                //Fecha Cuota
                GridColumn fechaCuota = new GridColumn();
                fechaCuota.FieldName = this._unboundPrefix + "FechaCuota";
                fechaCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota");
                fechaCuota.UnboundType = UnboundColumnType.DateTime;
                fechaCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaCuota.AppearanceCell.Options.UseTextOptions = true;
                fechaCuota.VisibleIndex = 1;
                fechaCuota.Width = 100;
                fechaCuota.Visible = true;
                fechaCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(fechaCuota);

                //vlrSaldo
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this._unboundPrefix + "VlrSaldo";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.Integer;
                vlrSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrSaldo.AppearanceCell.Options.UseTextOptions = true;
                vlrSaldo.VisibleIndex = 2;
                vlrSaldo.Width = 150;
                vlrSaldo.Visible = true;
                vlrSaldo.ColumnEdit = this.editValue;
                vlrSaldo.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(vlrSaldo);

                //VlrCuota
                GridColumn vlrCuotaCartera = new GridColumn();
                vlrCuotaCartera.FieldName = this._unboundPrefix + "VlrCuota";
                vlrCuotaCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                vlrCuotaCartera.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCartera.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuotaCartera.AppearanceCell.Options.UseTextOptions = true;
                vlrCuotaCartera.VisibleIndex = 3;
                vlrCuotaCartera.Width = 150;
                vlrCuotaCartera.Visible = false;
                vlrCuotaCartera.ColumnEdit = this.editValue;
                vlrCuotaCartera.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(vlrCuotaCartera);

                //VlrCapital
                GridColumn vlrCapital = new GridColumn();
                vlrCapital.FieldName = this._unboundPrefix + "VlrCapital";
                vlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                vlrCapital.UnboundType = UnboundColumnType.Integer;
                vlrCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCapital.AppearanceCell.Options.UseTextOptions = true;
                vlrCapital.VisibleIndex = 4;
                vlrCapital.Width = 120;
                vlrCapital.Visible = true;
                vlrCapital.ColumnEdit = this.editValue;
                vlrCapital.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(vlrCapital);

                //VlrInteres
                GridColumn vlrInteresDet = new GridColumn();
                vlrInteresDet.FieldName = this._unboundPrefix + "VlrInteres";
                vlrInteresDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrInteres");
                vlrInteresDet.UnboundType = UnboundColumnType.Integer;
                vlrInteresDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrInteresDet.AppearanceCell.Options.UseTextOptions = true;
                vlrInteresDet.VisibleIndex = 5;
                vlrInteresDet.Width = 120;
                vlrInteresDet.Visible = true;
                vlrInteresDet.ColumnEdit = this.editValue;
                vlrInteresDet.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(vlrInteresDet);

                //VlrSeguro
                GridColumn vlrSeguro = new GridColumn();
                vlrSeguro.FieldName = this._unboundPrefix + "VlrSeguro";
                vlrSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSeguro");
                vlrSeguro.UnboundType = UnboundColumnType.Integer;
                vlrSeguro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrSeguro.AppearanceCell.Options.UseTextOptions = true;
                vlrSeguro.VisibleIndex = 6;
                vlrSeguro.Width = 120;
                vlrSeguro.Visible = true;
                vlrSeguro.ColumnEdit = this.editValue;
                vlrSeguro.OptionsColumn.AllowEdit = false;
                this.gvCuotas.Columns.Add(vlrSeguro);

                //Vlr VlrMora
                GridColumn VlrMoraDet = new GridColumn();
                VlrMoraDet.FieldName = this._unboundPrefix + "VlrMora";
                VlrMoraDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMora");
                VlrMoraDet.UnboundType = UnboundColumnType.Integer;
                VlrMoraDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrMoraDet.AppearanceCell.Options.UseTextOptions = true;
                VlrMoraDet.VisibleIndex = 7;
                VlrMoraDet.Width = 120;
                VlrMoraDet.Visible = true;
                VlrMoraDet.OptionsColumn.AllowEdit = false;
                VlrMoraDet.ColumnEdit = this.editValue;
                this.gvCuotas.Columns.Add(VlrMoraDet);

                //Vlr Prejuridico
                GridColumn VlrPrejuridicoDet = new GridColumn();
                VlrPrejuridicoDet.FieldName = this._unboundPrefix + "VlrPrejuridico";
                VlrPrejuridicoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrejuridico");
                VlrPrejuridicoDet.UnboundType = UnboundColumnType.Integer;
                VlrPrejuridicoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPrejuridicoDet.AppearanceCell.Options.UseTextOptions = true;
                VlrPrejuridicoDet.VisibleIndex = 8;
                VlrPrejuridicoDet.Width = 120;
                VlrPrejuridicoDet.Visible = true;
                VlrPrejuridicoDet.OptionsColumn.AllowEdit = false;
                VlrPrejuridicoDet.ColumnEdit = this.editValue;
                this.gvCuotas.Columns.Add(VlrPrejuridicoDet);

                //Vlr VlrSaldoOtros
                GridColumn VlrSaldoOtrosDet = new GridColumn();
                VlrSaldoOtrosDet.FieldName = this._unboundPrefix + "VlrOtro2";
                VlrSaldoOtrosDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoOtros");
                VlrSaldoOtrosDet.UnboundType = UnboundColumnType.Integer;
                VlrSaldoOtrosDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoOtrosDet.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoOtrosDet.VisibleIndex = 9;
                VlrSaldoOtrosDet.Width = 120;
                VlrSaldoOtrosDet.Visible = true;
                VlrSaldoOtrosDet.OptionsColumn.AllowEdit = false;
                VlrSaldoOtrosDet.ColumnEdit = this.editValue;
                this.gvCuotas.Columns.Add(VlrSaldoOtrosDet);

                //Vlr VlrGastos
                GridColumn VlrGastosDet = new GridColumn();
                VlrGastosDet.FieldName = this._unboundPrefix + "VlrOtro1";
                VlrGastosDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGastos");
                VlrGastosDet.UnboundType = UnboundColumnType.Integer;
                VlrGastosDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGastosDet.AppearanceCell.Options.UseTextOptions = true;
                VlrGastosDet.VisibleIndex = 10;
                VlrGastosDet.Width = 120;
                VlrGastosDet.Visible = true;
                VlrGastosDet.OptionsColumn.AllowEdit = false;
                VlrGastosDet.ColumnEdit = this.editValue;
                this.gvCuotas.Columns.Add(VlrGastosDet);  
                #endregion                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranzas.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;
            this.masterCliente.Value = string.Empty;
            this.dtFechaCorte.EditValue = DateTime.Now;
            this.credito = new DTO_ccCreditoDocu();
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.gcLibranzas.DataSource = this.creditos;
            this.clienteID = string.Empty;
            this.validate = true;
        }

        /// <summary>
        /// Funcion que realiza la opreacion de busqueda
        /// </summary>
        private void GetSearch()
        {
            try
            {
                if (this.masterCliente.ValidID)
                {
                    this.clienteID = this.masterCliente.Value;
                    this.creditos = this._bc.AdministrationModel.GetCreditoByClienteAndFecha(this.clienteID, DateTime.Now,true,true);

                    if (this.creditos != null && this.creditos.Count > 0)
                    {
                        foreach (DTO_ccCreditoDocu cred in this.creditos)
                        {
                            #region Carga Saldos
                            cred.Detalle = new List<DTO_ccCreditoComponentes>();
                            DTO_InfoCredito info = this._bc.AdministrationModel.GetSaldoCredito(cred.NumeroDoc.Value.Value, this.dtFechaCorte.DateTime.Date, true, true, true, false);
                            if (info != null)
                            {
                                foreach (DTO_ccCreditoPlanPagos pp in info.PlanPagos)
                                {
                                    pp.VlrCapital.Value = info.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteCapital).Sum(y => y.CuotaSaldo.Value);
                                    pp.VlrInteres.Value = info.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteInteres).Sum(y => y.CuotaSaldo.Value);
                                    pp.VlrSeguro.Value = info.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && (x.ComponenteCarteraID.Value == this.componenteSeguro || x.ComponenteCarteraID.Value == this.componenteIntSeguro)).Sum(y => y.CuotaSaldo.Value);
                                    pp.VlrMora.Value = info.SaldosComponentes.FindAll(x => x.ComponenteCarteraID.Value == this.componenteMora).Sum(y => y.CuotaSaldo.Value);
                                    pp.VlrPrejuridico.Value = info.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componentePrejuridico).Sum(y => y.CuotaSaldo.Value);
                                    pp.VlrOtro1.Value = info.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.TipoComponente.Value == (byte)TipoComponente.ComponenteGasto).Sum(y => y.CuotaSaldo.Value);//Gastos
                                    pp.VlrOtro2.Value = info.SaldosComponentes.FindAll(x => (x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value != this.componenteCapital &&
                                                                                             x.ComponenteCarteraID.Value != this.componenteInteres &&
                                                                                             x.ComponenteCarteraID.Value != this.componenteSeguro &&
                                                                                             x.ComponenteCarteraID.Value != this.componenteIntSeguro) &&
                                                                                             x.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota).Sum(y => y.CuotaSaldo.Value);//Otros

                                }
                                cred.Cuotas = info.PlanPagos;
                                cred.VlrSaldo.Value = info.PlanPagos.Sum(y => y.VlrSaldo.Value);
                                cred.VlrSaldoVencido.Value = info.PlanPagos.FindAll(x => x.FechaCuota.Value < this.dtFechaCorte.DateTime).Sum(y => y.VlrSaldo.Value);
                                cred.VlrCapital.Value = info.PlanPagos.Sum(y => y.VlrCapital.Value);
                                cred.VlrInteres.Value = info.PlanPagos.Sum(y => y.VlrInteres.Value);
                                cred.VlrSeguro.Value = info.PlanPagos.Sum(y => y.VlrSeguro.Value);
                                cred.VlrMora.Value = info.PlanPagos.Sum(y => y.VlrMora.Value);
                                cred.VlrPrejuridico.Value = info.PlanPagos.Sum(y => y.VlrPrejuridico.Value);
                                cred.VlrGastos.Value = info.PlanPagos.Sum(y => y.VlrOtro1.Value);
                                cred.VlrSaldoOtros.Value = info.PlanPagos.Sum(y => y.VlrOtro2.Value);
                            }
                            #endregion
                        }
                        #region Carga la informacion de la grilla
                        this.gcLibranzas.DataSource = this.creditos;
                        this.gcLibranzas.RefreshDataSource();
                        this.gvLibranzas.BestFitColumns();
                        this.gvCuotas.BestFitColumns();
                        this.gvLibranzas.MoveFirst();
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        this.CleanData();
                    }
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidSearchCriteria));
                    return;
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranza.cs", "GetSearch"));
            }
        }

        #endregion Funciones Privadas

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
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemSave.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranza.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranza.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranza.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranzas_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
                    e.Value = true;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
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
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranzas_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranzas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.validate)
                {
                    //int row = e.FocusedRowHandle;
                    //this.selectedCredito = (DTO_ccCreditoDocu)this.gvGenerales.GetRow(row);
                    //this.GetValues(this.selectedCredito);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranzas_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.RepositoryItem = this.linkEditViewFile;
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranzas_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = e.Column.Caption;
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
               this.GetSearch();             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SaldosLibranza.cs", "TBSearch"));
            }
        }

        #endregion

    }
}
