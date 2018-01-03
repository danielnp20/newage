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
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaObligacionesGarantias : FormWithToolbar
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

        private List<DTO_QueryObligaciones> cursor = new List<DTO_QueryObligaciones>();
        private DTO_QueryObligaciones rowCurrent = new DTO_QueryObligaciones();

        private List<DTO_QueryGarantiaControl> cursorGarantia = new List<DTO_QueryGarantiaControl>();
        private DTO_QueryGarantiaControl rowCurrentGarantia = new DTO_QueryGarantiaControl();

        List<DTO_QueryGarantiaControl> Gardeudor = new List<DTO_QueryGarantiaControl>();
        List<DTO_QueryObligaciones> Obldeudor = new List<DTO_QueryObligaciones>();

        //Variables formulario
        private bool _modifica = false;
        private bool validate = true;
        private string clienteID = string.Empty;
        private int numerodoc = 0;
        private int NumObligaciones = 0;
        private int NumGarantias = 0;
        private Dictionary<string, int> Datos = new Dictionary<string, int>();
        private decimal _SaldoCapital = 0;
        private decimal _SaldoVencido = 0;
        private decimal _SaldoCredito = 0;
        private decimal _TotalGarantia = 0;

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ConsultaObligacionesGarantias()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ConsultaObligacionesGarantias(string clienteID, int numerodoc, Dictionary<string, int> Datos, bool Modifica=false)
        {
            this.Constructor(clienteID,numerodoc, Datos,Modifica);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public void Constructor(string clienteID = null, int? numerodoc = null, Dictionary<string, int> Datos = null, bool Modifica = false)
        {
            this.InitializeComponent();
            try
            {
                this._modifica = Modifica;
                this.clienteID = clienteID;
                this.numerodoc = Convert.ToInt32(numerodoc);
                this.Datos = Datos;
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.cf;
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.txtDato1.EditValue = Convert.ToInt32(this.Datos["1"].ToString());
                this.txtDato2.EditValue = Convert.ToInt32(this.Datos["2"].ToString());
                this.txtDato3.EditValue = Convert.ToInt32(this.Datos["3"].ToString());
                this.txtDato4.EditValue = Convert.ToInt32(this.Datos["4"].ToString());

                this.AddGridCols();
                this.AddGridColsObligaciones();
                this.AddGridColsConyuge();
                this.AddGridColsObligacionesCony();
                this.AddGridColsCod1();
                this.AddGridColsObligacionesCod1();
                this.AddGridColsCod2();
                this.AddGridColsObligacionesCod2();

                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantias.cs", "ConsultaObligacionesGarantias.cs-ConsultaObligacionesGarantias"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryGarantias;
            this._frmModule = ModulesPrefix.dr;

            //Carga los combos de la fecha
            //this.dtFechaInicial.EditValue = DateTime.Now;
            //this.dtFechaCorte.EditValue = DateTime.Now;
            //this._bc.InitMasterUC(this.masterEtapa, AppMasters.glIncumplimientoEtapa, true, true, true, false);

        }
        #region Grillas deudor
        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridCols()
        {
            try
            {
                #region Grilla Principal

                GridColumn GarantiaID = new GridColumn();
                GarantiaID.FieldName = this._unboundPrefix + "GarantiaID";
                GarantiaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaID");
                GarantiaID.UnboundType = UnboundColumnType.Integer;
                GarantiaID.VisibleIndex = 1;
                GarantiaID.Width = 80;
                GarantiaID.Visible = true;
                GarantiaID.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(GarantiaID);

                GridColumn NroDoc = new GridColumn();
                NroDoc.FieldName = this._unboundPrefix + "NroDoc";
                NroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NroDoc");
                NroDoc.UnboundType = UnboundColumnType.Integer;
                NroDoc.VisibleIndex = 2;
                NroDoc.Width = 80;NroDoc.Visible = true;
                NroDoc.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(NroDoc);

                GridColumn Propietario = new GridColumn();
                Propietario.FieldName = this._unboundPrefix + "Propietario";
                Propietario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Propietario");
                Propietario.UnboundType = UnboundColumnType.Integer;
                Propietario.VisibleIndex = 2;
                Propietario.Width = 80;
                Propietario.Visible = true;
                Propietario.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(Propietario);

                GridColumn Documento = new GridColumn();
                Documento.FieldName = this._unboundPrefix + "Documento";
                Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Documento");
                Documento.UnboundType = UnboundColumnType.Integer;
                Documento.VisibleIndex = 2;
                Documento.Width = 80;
                Documento.Visible = true;
                Documento.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(Documento);

                GridColumn Referencia = new GridColumn();
                Referencia.FieldName = this._unboundPrefix + "Referencia";
                Referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Referencia");
                Referencia.UnboundType = UnboundColumnType.Integer;
                Referencia.VisibleIndex = 2;
                Referencia.Width = 80;
                Referencia.Visible = true;
                Referencia.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(Referencia);


                // FechaRegistro
                GridColumn FechaRegistro = new GridColumn();
                FechaRegistro.FieldName = this._unboundPrefix + "FechaRegistro";
                FechaRegistro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaRegistro");
                FechaRegistro.UnboundType = UnboundColumnType.DateTime;
                FechaRegistro.VisibleIndex = 2;
                FechaRegistro.Width = 80;
                FechaRegistro.Visible = true;
                FechaRegistro.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(FechaRegistro);

                // FechaVTO
                GridColumn FechaVTO = new GridColumn();
                FechaVTO.FieldName = this._unboundPrefix + "FechaVTO";
                FechaVTO.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVTO");
                FechaVTO.UnboundType = UnboundColumnType.DateTime;
                FechaVTO.VisibleIndex = 2;
                FechaVTO.Width = 80;
                FechaVTO.Visible = true;
                FechaVTO.OptionsColumn.AllowEdit = false;
                this.gvGarantias.Columns.Add(FechaVTO);

                GridColumn VlrGarantia = new GridColumn();
                VlrGarantia.FieldName = this._unboundPrefix + "VlrGarantia";
                VlrGarantia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGarantia");
                VlrGarantia.UnboundType = UnboundColumnType.Integer;
                VlrGarantia.VisibleIndex = 2;
                VlrGarantia.Width = 80;
                VlrGarantia.Visible = true;
                VlrGarantia.OptionsColumn.AllowEdit = false;
                VlrGarantia.ColumnEdit = this.editSpin;
                VlrGarantia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGarantia.AppearanceCell.Options.UseTextOptions = true;
                this.gvGarantias.Columns.Add(VlrGarantia);

                // Cancela
                GridColumn Cancela = new GridColumn();
                Cancela.FieldName = this._unboundPrefix + "CancelaInd";
                Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                Cancela.UnboundType = UnboundColumnType.Boolean;
                Cancela.VisibleIndex = 10;
                Cancela.Width = 50;
                Cancela.Visible = true;
                Cancela.OptionsColumn.AllowEdit = this._modifica;
                Cancela.AppearanceCell.Options.UseTextOptions = true;
                this.gvGarantias.Columns.Add(Cancela);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsObligaciones()
        {
            try
            {
                #region Grilla Principal

                GridColumn Oblig = new GridColumn();
                Oblig.FieldName = this._unboundPrefix + "Oblig";
                Oblig.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Oblig");
                Oblig.UnboundType = UnboundColumnType.String;
                Oblig.VisibleIndex = 1;
                Oblig.Width = 80;
                Oblig.Visible = true;
                Oblig.OptionsColumn.AllowEdit = false;
                this.gvObligaciones.Columns.Add(Oblig);

                GridColumn Pagare = new GridColumn();
                Pagare.FieldName = this._unboundPrefix + "Pagare";
                Pagare.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Pagare");
                Pagare.UnboundType = UnboundColumnType.String;
                Pagare.VisibleIndex = 2;
                Pagare.Width = 80;
                Pagare.Visible = true;
                Pagare.OptionsColumn.AllowEdit = false;
                this.gvObligaciones.Columns.Add(Pagare);

                GridColumn LineaCreditoID = new GridColumn();
                LineaCreditoID.FieldName = this._unboundPrefix + "LineaCreditoID";
                LineaCreditoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaCreditoID");
                LineaCreditoID.UnboundType = UnboundColumnType.String;
                LineaCreditoID.VisibleIndex = 3;
                LineaCreditoID.Width = 80;
                LineaCreditoID.Visible = true;
                LineaCreditoID.OptionsColumn.AllowEdit = false;
                this.gvObligaciones.Columns.Add(LineaCreditoID);

                GridColumn Altura = new GridColumn();
                Altura.FieldName = this._unboundPrefix + "Altura";
                Altura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Altura");
                Altura.UnboundType = UnboundColumnType.String;
                Altura.VisibleIndex = 4;
                Altura.Width = 80;
                Altura.Visible = true;
                Altura.OptionsColumn.AllowEdit = false;
                Altura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Altura.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligaciones.Columns.Add(Altura);

                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 5;
                VlrCuota.Width = 80;
                VlrCuota.Visible = true;
                VlrCuota.ColumnEdit = this.editSpin;
                VlrCuota.OptionsColumn.AllowEdit = false;
                VlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCuota.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligaciones.Columns.Add(VlrCuota);
                //SaldoInicialDeta1

                GridColumn SdoCapital = new GridColumn();
                SdoCapital.FieldName = this._unboundPrefix + "SdoCapital";
                SdoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCapital");
                SdoCapital.UnboundType = UnboundColumnType.Decimal;
                SdoCapital.VisibleIndex = 6;
                SdoCapital.Width = 80;
                SdoCapital.Visible = true;
                SdoCapital.ColumnEdit = this.editSpin;
                SdoCapital.OptionsColumn.AllowEdit = false;
                SdoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoCapital.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligaciones.Columns.Add(SdoCapital);

                GridColumn SdoVencido = new GridColumn();
                SdoVencido.FieldName = this._unboundPrefix + "SdoVencido";
                SdoVencido.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoVencido");
                SdoVencido.UnboundType = UnboundColumnType.Decimal;
                SdoVencido.VisibleIndex = 7;
                SdoVencido.Width = 80;
                SdoVencido.Visible = true;
                SdoVencido.ColumnEdit = this.editSpin;
                SdoVencido.OptionsColumn.AllowEdit = false;
                SdoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoVencido.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligaciones.Columns.Add(SdoVencido);

                //GridColumn SdoCredito = new GridColumn();
                //SdoCredito.FieldName = this._unboundPrefix + "SdoCredito";
                //SdoCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCredito");
                //SdoCredito.UnboundType = UnboundColumnType.Integer;
                //SdoCredito.VisibleIndex = 8;
                //SdoCredito.Width = 80;
                //SdoCredito.Visible = true;
                //SdoCredito.ColumnEdit = this.editSpin;
                //SdoCredito.OptionsColumn.AllowEdit = false;
                //SdoCredito.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //SdoCredito.AppearanceCell.Options.UseTextOptions = true;

                //this.gvObligaciones.Columns.Add(SdoCredito);

                // Cubrimiento
                GridColumn Cubrimiento = new GridColumn();
                Cubrimiento.FieldName = this._unboundPrefix + "Cubrimiento";
                Cubrimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cubrimiento");
                Cubrimiento.UnboundType = UnboundColumnType.Decimal;
                Cubrimiento.VisibleIndex = 9;
                Cubrimiento.Width = 80;
                Cubrimiento.Visible = true;
                Cubrimiento.OptionsColumn.AllowEdit = false;
                Cubrimiento.ColumnEdit = this.editNums;
                Cubrimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cubrimiento.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligaciones.Columns.Add(Cubrimiento);
                
                // Cancela
                GridColumn Cancela = new GridColumn();
                Cancela.FieldName = this._unboundPrefix + "CancelaInd";
                Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                Cancela.UnboundType = UnboundColumnType.Boolean;
                Cancela.VisibleIndex = 10;
                Cancela.Width = 50;
                Cancela.Visible = true;
                Cancela.OptionsColumn.AllowEdit = this._modifica;
                //Cancela.ColumnEdit = this.editCant;
                //Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cancela.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligaciones.Columns.Add(Cancela);




                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }
        #endregion

        #region Grillas Conyuge
        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsConyuge()
        {
            try
            {
                #region Grilla Principal

                GridColumn GarantiaID = new GridColumn();
                GarantiaID.FieldName = this._unboundPrefix + "GarantiaID";
                GarantiaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaID");
                GarantiaID.UnboundType = UnboundColumnType.Integer;
                GarantiaID.VisibleIndex = 1;
                GarantiaID.Width = 80;
                GarantiaID.Visible = true;
                GarantiaID.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(GarantiaID);

                GridColumn NroDoc = new GridColumn();
                NroDoc.FieldName = this._unboundPrefix + "NroDoc";
                NroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NroDoc");
                NroDoc.UnboundType = UnboundColumnType.Integer;
                NroDoc.VisibleIndex = 2;
                NroDoc.Width = 80;
                NroDoc.Visible = true;
                NroDoc.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(NroDoc);

                GridColumn Propietario = new GridColumn();
                Propietario.FieldName = this._unboundPrefix + "Propietario";
                Propietario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Propietario");
                Propietario.UnboundType = UnboundColumnType.Integer;
                Propietario.VisibleIndex = 2;
                Propietario.Width = 80;
                Propietario.Visible = true;
                Propietario.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(Propietario);

                GridColumn Documento = new GridColumn();
                Documento.FieldName = this._unboundPrefix + "Documento";
                Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Documento");
                Documento.UnboundType = UnboundColumnType.Integer;
                Documento.VisibleIndex = 2;
                Documento.Width = 80;
                Documento.Visible = true;
                Documento.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(Documento);

                GridColumn Referencia = new GridColumn();
                Referencia.FieldName = this._unboundPrefix + "Referencia";
                Referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Referencia");
                Referencia.UnboundType = UnboundColumnType.Integer;
                Referencia.VisibleIndex = 2;
                Referencia.Width = 80;
                Referencia.Visible = true;
                Referencia.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(Referencia);


                // FechaRegistro
                GridColumn FechaRegistro = new GridColumn();
                FechaRegistro.FieldName = this._unboundPrefix + "FechaRegistro";
                FechaRegistro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaRegistro");
                FechaRegistro.UnboundType = UnboundColumnType.DateTime;
                FechaRegistro.VisibleIndex = 2;
                FechaRegistro.Width = 80;
                FechaRegistro.Visible = true;
                FechaRegistro.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(FechaRegistro);

                // FechaVTO
                GridColumn FechaVTO = new GridColumn();
                FechaVTO.FieldName = this._unboundPrefix + "FechaVTO";
                FechaVTO.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVTO");
                FechaVTO.UnboundType = UnboundColumnType.DateTime;
                FechaVTO.VisibleIndex = 2;
                FechaVTO.Width = 80;
                FechaVTO.Visible = true;
                FechaVTO.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCony.Columns.Add(FechaVTO);

                GridColumn VlrGarantia = new GridColumn();
                VlrGarantia.FieldName = this._unboundPrefix + "VlrGarantia";
                VlrGarantia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGarantia");
                VlrGarantia.UnboundType = UnboundColumnType.Integer;
                VlrGarantia.VisibleIndex = 2;
                VlrGarantia.Width = 80;
                VlrGarantia.Visible = true;
                VlrGarantia.OptionsColumn.AllowEdit = false;
                VlrGarantia.ColumnEdit = this.editSpin;
                VlrGarantia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGarantia.AppearanceCell.Options.UseTextOptions = true;
                this.gvGarantiasCony.Columns.Add(VlrGarantia);

                //// Cancela
                //GridColumn Cancela = new GridColumn();
                //Cancela.FieldName = this._unboundPrefix + "RetiraInd";
                //Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                //Cancela.UnboundType = UnboundColumnType.Boolean;
                //Cancela.VisibleIndex = 10;
                //Cancela.Width = 50;
                //Cancela.Visible = true;
                //Cancela.OptionsColumn.AllowEdit = false;
                ////Cancela.ColumnEdit = this.editCant;
                ////Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //Cancela.AppearanceCell.Options.UseTextOptions = true;
                //this.gvGarantiasCony.Columns.Add(Cancela);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsObligacionesCony()
        {
            try
            {
                #region Grilla Principal

                GridColumn Oblig = new GridColumn();
                Oblig.FieldName = this._unboundPrefix + "Oblig";
                Oblig.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Oblig");
                Oblig.UnboundType = UnboundColumnType.String;
                Oblig.VisibleIndex = 1;
                Oblig.Width = 80;
                Oblig.Visible = true;
                Oblig.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCony.Columns.Add(Oblig);

                GridColumn Pagare = new GridColumn();
                Pagare.FieldName = this._unboundPrefix + "Pagare";
                Pagare.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Pagare");
                Pagare.UnboundType = UnboundColumnType.String;
                Pagare.VisibleIndex = 2;
                Pagare.Width = 80;
                Pagare.Visible = true;
                Pagare.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCony.Columns.Add(Pagare);

                GridColumn LineaCreditoID = new GridColumn();
                LineaCreditoID.FieldName = this._unboundPrefix + "LineaCreditoID";
                LineaCreditoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaCreditoID");
                LineaCreditoID.UnboundType = UnboundColumnType.String;
                LineaCreditoID.VisibleIndex = 3;
                LineaCreditoID.Width = 80;
                LineaCreditoID.Visible = true;
                LineaCreditoID.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCony.Columns.Add(LineaCreditoID);

                GridColumn Altura = new GridColumn();
                Altura.FieldName = this._unboundPrefix + "Altura";
                Altura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Altura");
                Altura.UnboundType = UnboundColumnType.String;
                Altura.VisibleIndex = 4;
                Altura.Width = 80;
                Altura.Visible = true;
                Altura.OptionsColumn.AllowEdit = false;
                Altura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Altura.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCony.Columns.Add(Altura);

                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 5;
                VlrCuota.Width = 80;
                VlrCuota.Visible = true;
                VlrCuota.ColumnEdit = this.editSpin;
                VlrCuota.OptionsColumn.AllowEdit = false;
                VlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCuota.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCony.Columns.Add(VlrCuota);
                //SaldoInicialDeta1

                GridColumn SdoCapital = new GridColumn();
                SdoCapital.FieldName = this._unboundPrefix + "SdoCapital";
                SdoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCapital");
                SdoCapital.UnboundType = UnboundColumnType.Decimal;
                SdoCapital.VisibleIndex = 6;
                SdoCapital.Width = 80;
                SdoCapital.Visible = true;
                SdoCapital.ColumnEdit = this.editSpin;
                SdoCapital.OptionsColumn.AllowEdit = false;
                SdoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoCapital.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCony.Columns.Add(SdoCapital);

                GridColumn SdoVencido = new GridColumn();
                SdoVencido.FieldName = this._unboundPrefix + "SdoVencido";
                SdoVencido.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoVencido");
                SdoVencido.UnboundType = UnboundColumnType.Decimal;
                SdoVencido.VisibleIndex = 7;
                SdoVencido.Width = 80;
                SdoVencido.Visible = true;
                SdoVencido.ColumnEdit = this.editSpin;
                SdoVencido.OptionsColumn.AllowEdit = false;
                SdoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoVencido.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCony.Columns.Add(SdoVencido);

                //GridColumn SdoCredito = new GridColumn();
                //SdoCredito.FieldName = this._unboundPrefix + "SdoCredito";
                //SdoCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCredito");
                //SdoCredito.UnboundType = UnboundColumnType.Integer;
                //SdoCredito.VisibleIndex = 8;
                //SdoCredito.Width = 80;
                //SdoCredito.Visible = true;
                //SdoCredito.ColumnEdit = this.editSpin;
                //SdoCredito.OptionsColumn.AllowEdit = false;
                //SdoCredito.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //SdoCredito.AppearanceCell.Options.UseTextOptions = true;

                //this.gvObligacionesCony.Columns.Add(SdoCredito);

                // Cubrimiento
                GridColumn Cubrimiento = new GridColumn();
                Cubrimiento.FieldName = this._unboundPrefix + "Cubrimiento";
                Cubrimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cubrimiento");
                Cubrimiento.UnboundType = UnboundColumnType.Decimal;
                Cubrimiento.VisibleIndex = 9;
                Cubrimiento.Width = 80;
                Cubrimiento.Visible = true;
                Cubrimiento.OptionsColumn.AllowEdit = false;
                Cubrimiento.ColumnEdit = this.editNums;
                Cubrimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cubrimiento.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCony.Columns.Add(Cubrimiento);

                //// Cancela
                //GridColumn Cancela = new GridColumn();
                //Cancela.FieldName = this._unboundPrefix + "IndRestructurado";
                //Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                //Cancela.UnboundType = UnboundColumnType.Boolean;
                //Cancela.VisibleIndex = 10;
                //Cancela.Width = 50;
                //Cancela.Visible = true;
                //Cancela.OptionsColumn.AllowEdit = false;
                ////Cancela.ColumnEdit = this.editCant;
                ////Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //Cancela.AppearanceCell.Options.UseTextOptions = true;
                //this.gvObligacionesCony.Columns.Add(Cancela);




                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }
        #endregion

        #region Grillas Codeudor1
        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsCod1()
        {
            try
            {
                #region Grilla Principal

                GridColumn GarantiaID = new GridColumn();
                GarantiaID.FieldName = this._unboundPrefix + "GarantiaID";
                GarantiaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaID");
                GarantiaID.UnboundType = UnboundColumnType.Integer;
                GarantiaID.VisibleIndex = 1;
                GarantiaID.Width = 80;
                GarantiaID.Visible = true;
                GarantiaID.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(GarantiaID);

                GridColumn NroDoc = new GridColumn();
                NroDoc.FieldName = this._unboundPrefix + "NroDoc";
                NroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NroDoc");
                NroDoc.UnboundType = UnboundColumnType.Integer;
                NroDoc.VisibleIndex = 2;
                NroDoc.Width = 80;
                NroDoc.Visible = true;
                NroDoc.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(NroDoc);

                GridColumn Propietario = new GridColumn();
                Propietario.FieldName = this._unboundPrefix + "Propietario";
                Propietario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Propietario");
                Propietario.UnboundType = UnboundColumnType.Integer;
                Propietario.VisibleIndex = 2;
                Propietario.Width = 80;
                Propietario.Visible = true;
                Propietario.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(Propietario);

                GridColumn Documento = new GridColumn();
                Documento.FieldName = this._unboundPrefix + "Documento";
                Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Documento");
                Documento.UnboundType = UnboundColumnType.Integer;
                Documento.VisibleIndex = 2;
                Documento.Width = 80;
                Documento.Visible = true;
                Documento.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(Documento);

                GridColumn Referencia = new GridColumn();
                Referencia.FieldName = this._unboundPrefix + "Referencia";
                Referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Referencia");
                Referencia.UnboundType = UnboundColumnType.Integer;
                Referencia.VisibleIndex = 2;
                Referencia.Width = 80;
                Referencia.Visible = true;
                Referencia.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(Referencia);


                // FechaRegistro
                GridColumn FechaRegistro = new GridColumn();
                FechaRegistro.FieldName = this._unboundPrefix + "FechaRegistro";
                FechaRegistro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaRegistro");
                FechaRegistro.UnboundType = UnboundColumnType.DateTime;
                FechaRegistro.VisibleIndex = 2;
                FechaRegistro.Width = 80;
                FechaRegistro.Visible = true;
                FechaRegistro.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(FechaRegistro);

                // FechaVTO
                GridColumn FechaVTO = new GridColumn();
                FechaVTO.FieldName = this._unboundPrefix + "FechaVTO";
                FechaVTO.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVTO");
                FechaVTO.UnboundType = UnboundColumnType.DateTime;
                FechaVTO.VisibleIndex = 2;
                FechaVTO.Width = 80;
                FechaVTO.Visible = true;
                FechaVTO.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod1.Columns.Add(FechaVTO);

                GridColumn VlrGarantia = new GridColumn();
                VlrGarantia.FieldName = this._unboundPrefix + "VlrGarantia";
                VlrGarantia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGarantia");
                VlrGarantia.UnboundType = UnboundColumnType.Integer;
                VlrGarantia.VisibleIndex = 2;
                VlrGarantia.Width = 80;
                VlrGarantia.Visible = true;
                VlrGarantia.OptionsColumn.AllowEdit = false;
                VlrGarantia.ColumnEdit = this.editSpin;
                VlrGarantia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGarantia.AppearanceCell.Options.UseTextOptions = true;
                this.gvGarantiasCod1.Columns.Add(VlrGarantia);

                //// Cancela
                //GridColumn Cancela = new GridColumn();
                //Cancela.FieldName = this._unboundPrefix + "RetiraInd";
                //Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                //Cancela.UnboundType = UnboundColumnType.Boolean;
                //Cancela.VisibleIndex = 10;
                //Cancela.Width = 50;
                //Cancela.Visible = true;
                //Cancela.OptionsColumn.AllowEdit = false;
                ////Cancela.ColumnEdit = this.editCant;
                ////Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //Cancela.AppearanceCell.Options.UseTextOptions = true;
                //this.gvGarantiasCod1.Columns.Add(Cancela);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsObligacionesCod1()
        {
            try
            {
                #region Grilla Principal

                GridColumn Oblig = new GridColumn();
                Oblig.FieldName = this._unboundPrefix + "Oblig";
                Oblig.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Oblig");
                Oblig.UnboundType = UnboundColumnType.String;
                Oblig.VisibleIndex = 1;
                Oblig.Width = 80;
                Oblig.Visible = true;
                Oblig.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCod1.Columns.Add(Oblig);

                GridColumn Pagare = new GridColumn();
                Pagare.FieldName = this._unboundPrefix + "Pagare";
                Pagare.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Pagare");
                Pagare.UnboundType = UnboundColumnType.String;
                Pagare.VisibleIndex = 2;
                Pagare.Width = 80;
                Pagare.Visible = true;
                Pagare.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCod1.Columns.Add(Pagare);

                GridColumn LineaCreditoID = new GridColumn();
                LineaCreditoID.FieldName = this._unboundPrefix + "LineaCreditoID";
                LineaCreditoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaCreditoID");
                LineaCreditoID.UnboundType = UnboundColumnType.String;
                LineaCreditoID.VisibleIndex = 3;
                LineaCreditoID.Width = 80;
                LineaCreditoID.Visible = true;
                LineaCreditoID.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCod1.Columns.Add(LineaCreditoID);

                GridColumn Altura = new GridColumn();
                Altura.FieldName = this._unboundPrefix + "Altura";
                Altura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Altura");
                Altura.UnboundType = UnboundColumnType.String;
                Altura.VisibleIndex = 4;
                Altura.Width = 80;
                Altura.Visible = true;
                Altura.OptionsColumn.AllowEdit = false;
                Altura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Altura.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod1.Columns.Add(Altura);

                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 5;
                VlrCuota.Width = 80;
                VlrCuota.Visible = true;
                VlrCuota.ColumnEdit = this.editSpin;
                VlrCuota.OptionsColumn.AllowEdit = false;
                VlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCuota.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod1.Columns.Add(VlrCuota);
                //SaldoInicialDeta1

                GridColumn SdoCapital = new GridColumn();
                SdoCapital.FieldName = this._unboundPrefix + "SdoCapital";
                SdoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCapital");
                SdoCapital.UnboundType = UnboundColumnType.Decimal;
                SdoCapital.VisibleIndex = 6;
                SdoCapital.Width = 80;
                SdoCapital.Visible = true;
                SdoCapital.ColumnEdit = this.editSpin;
                SdoCapital.OptionsColumn.AllowEdit = false;
                SdoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoCapital.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod1.Columns.Add(SdoCapital);

                GridColumn SdoVencido = new GridColumn();
                SdoVencido.FieldName = this._unboundPrefix + "SdoVencido";
                SdoVencido.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoVencido");
                SdoVencido.UnboundType = UnboundColumnType.Decimal;
                SdoVencido.VisibleIndex = 7;
                SdoVencido.Width = 80;
                SdoVencido.Visible = true;
                SdoVencido.ColumnEdit = this.editSpin;
                SdoVencido.OptionsColumn.AllowEdit = false;
                SdoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoVencido.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod1.Columns.Add(SdoVencido);

                //GridColumn SdoCredito = new GridColumn();
                //SdoCredito.FieldName = this._unboundPrefix + "SdoCredito";
                //SdoCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCredito");
                //SdoCredito.UnboundType = UnboundColumnType.Integer;
                //SdoCredito.VisibleIndex = 8;
                //SdoCredito.Width = 80;
                //SdoCredito.Visible = true;
                //SdoCredito.ColumnEdit = this.editSpin;
                //SdoCredito.OptionsColumn.AllowEdit = false;
                //SdoCredito.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //SdoCredito.AppearanceCell.Options.UseTextOptions = true;

                //this.gvObligacionesCod1.Columns.Add(SdoCredito);

                // Cubrimiento
                GridColumn Cubrimiento = new GridColumn();
                Cubrimiento.FieldName = this._unboundPrefix + "Cubrimiento";
                Cubrimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cubrimiento");
                Cubrimiento.UnboundType = UnboundColumnType.Decimal;
                Cubrimiento.VisibleIndex = 9;
                Cubrimiento.Width = 80;
                Cubrimiento.Visible = true;
                Cubrimiento.OptionsColumn.AllowEdit = false;
                Cubrimiento.ColumnEdit = this.editNums;
                Cubrimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cubrimiento.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod1.Columns.Add(Cubrimiento);

                //// Cancela
                //GridColumn Cancela = new GridColumn();
                //Cancela.FieldName = this._unboundPrefix + "IndRestructurado";
                //Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                //Cancela.UnboundType = UnboundColumnType.Boolean;
                //Cancela.VisibleIndex = 10;
                //Cancela.Width = 50;
                //Cancela.Visible = true;
                //Cancela.OptionsColumn.AllowEdit = false;
                ////Cancela.ColumnEdit = this.editCant;
                ////Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //Cancela.AppearanceCell.Options.UseTextOptions = true;
                //this.gvObligacionesCod1.Columns.Add(Cancela);




                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }
        #endregion

        #region Grillas Codeudor2
        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsCod2()
        {
            try
            {
                #region Grilla Principal

                GridColumn GarantiaID = new GridColumn();
                GarantiaID.FieldName = this._unboundPrefix + "GarantiaID";
                GarantiaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaID");
                GarantiaID.UnboundType = UnboundColumnType.Integer;
                GarantiaID.VisibleIndex = 1;
                GarantiaID.Width = 80;
                GarantiaID.Visible = true;
                GarantiaID.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(GarantiaID);

                GridColumn NroDoc = new GridColumn();
                NroDoc.FieldName = this._unboundPrefix + "NroDoc";
                NroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NroDoc");
                NroDoc.UnboundType = UnboundColumnType.Integer;
                NroDoc.VisibleIndex = 2;
                NroDoc.Width = 80;
                NroDoc.Visible = true;
                NroDoc.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(NroDoc);

                GridColumn Propietario = new GridColumn();
                Propietario.FieldName = this._unboundPrefix + "Propietario";
                Propietario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Propietario");
                Propietario.UnboundType = UnboundColumnType.Integer;
                Propietario.VisibleIndex = 2;
                Propietario.Width = 80;
                Propietario.Visible = true;
                Propietario.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(Propietario);

                GridColumn Documento = new GridColumn();
                Documento.FieldName = this._unboundPrefix + "Documento";
                Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Documento");
                Documento.UnboundType = UnboundColumnType.Integer;
                Documento.VisibleIndex = 2;
                Documento.Width = 80;
                Documento.Visible = true;
                Documento.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(Documento);

                GridColumn Referencia = new GridColumn();
                Referencia.FieldName = this._unboundPrefix + "Referencia";
                Referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Referencia");
                Referencia.UnboundType = UnboundColumnType.Integer;
                Referencia.VisibleIndex = 2;
                Referencia.Width = 80;
                Referencia.Visible = true;
                Referencia.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(Referencia);


                // FechaRegistro
                GridColumn FechaRegistro = new GridColumn();
                FechaRegistro.FieldName = this._unboundPrefix + "FechaRegistro";
                FechaRegistro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaRegistro");
                FechaRegistro.UnboundType = UnboundColumnType.DateTime;
                FechaRegistro.VisibleIndex = 2;
                FechaRegistro.Width = 80;
                FechaRegistro.Visible = true;
                FechaRegistro.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(FechaRegistro);

                // FechaVTO
                GridColumn FechaVTO = new GridColumn();
                FechaVTO.FieldName = this._unboundPrefix + "FechaVTO";
                FechaVTO.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVTO");
                FechaVTO.UnboundType = UnboundColumnType.DateTime;
                FechaVTO.VisibleIndex = 2;
                FechaVTO.Width = 80;
                FechaVTO.Visible = true;
                FechaVTO.OptionsColumn.AllowEdit = false;
                this.gvGarantiasCod2.Columns.Add(FechaVTO);

                GridColumn VlrGarantia = new GridColumn();
                VlrGarantia.FieldName = this._unboundPrefix + "VlrGarantia";
                VlrGarantia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGarantia");
                VlrGarantia.UnboundType = UnboundColumnType.Integer;
                VlrGarantia.VisibleIndex = 2;
                VlrGarantia.Width = 80;
                VlrGarantia.Visible = true;
                VlrGarantia.OptionsColumn.AllowEdit = false;
                VlrGarantia.ColumnEdit = this.editSpin;
                VlrGarantia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGarantia.AppearanceCell.Options.UseTextOptions = true;
                this.gvGarantiasCod2.Columns.Add(VlrGarantia);

                //// Cancela
                //GridColumn Cancela = new GridColumn();
                //Cancela.FieldName = this._unboundPrefix + "RetiraInd";
                //Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                //Cancela.UnboundType = UnboundColumnType.Boolean;
                //Cancela.VisibleIndex = 10;
                //Cancela.Width = 50;
                //Cancela.Visible = true;
                //Cancela.OptionsColumn.AllowEdit = false;
                ////Cancela.ColumnEdit = this.editCant;
                ////Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //Cancela.AppearanceCell.Options.UseTextOptions = true;
                //this.gvGarantiasCod2.Columns.Add(Cancela);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>

        private void AddGridColsObligacionesCod2()
        {
            try
            {
                #region Grilla Principal

                GridColumn Oblig = new GridColumn();
                Oblig.FieldName = this._unboundPrefix + "Oblig";
                Oblig.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Oblig");
                Oblig.UnboundType = UnboundColumnType.String;
                Oblig.VisibleIndex = 1;
                Oblig.Width = 80;
                Oblig.Visible = true;
                Oblig.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCod2.Columns.Add(Oblig);

                GridColumn Pagare = new GridColumn();
                Pagare.FieldName = this._unboundPrefix + "Pagare";
                Pagare.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Pagare");
                Pagare.UnboundType = UnboundColumnType.String;
                Pagare.VisibleIndex = 2;
                Pagare.Width = 80;
                Pagare.Visible = true;
                Pagare.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCod2.Columns.Add(Pagare);

                GridColumn LineaCreditoID = new GridColumn();
                LineaCreditoID.FieldName = this._unboundPrefix + "LineaCreditoID";
                LineaCreditoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaCreditoID");
                LineaCreditoID.UnboundType = UnboundColumnType.String;
                LineaCreditoID.VisibleIndex = 3;
                LineaCreditoID.Width = 80;
                LineaCreditoID.Visible = true;
                LineaCreditoID.OptionsColumn.AllowEdit = false;
                this.gvObligacionesCod2.Columns.Add(LineaCreditoID);

                GridColumn Altura = new GridColumn();
                Altura.FieldName = this._unboundPrefix + "Altura";
                Altura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Altura");
                Altura.UnboundType = UnboundColumnType.String;
                Altura.VisibleIndex = 4;
                Altura.Width = 80;
                Altura.Visible = true;
                Altura.OptionsColumn.AllowEdit = false;
                Altura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Altura.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod2.Columns.Add(Altura);

                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 5;
                VlrCuota.Width = 80;
                VlrCuota.Visible = true;
                VlrCuota.ColumnEdit = this.editSpin;
                VlrCuota.OptionsColumn.AllowEdit = false;
                VlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCuota.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod2.Columns.Add(VlrCuota);
                //SaldoInicialDeta1

                GridColumn SdoCapital = new GridColumn();
                SdoCapital.FieldName = this._unboundPrefix + "SdoCapital";
                SdoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCapital");
                SdoCapital.UnboundType = UnboundColumnType.Decimal;
                SdoCapital.VisibleIndex = 6;
                SdoCapital.Width = 80;
                SdoCapital.Visible = true;
                SdoCapital.ColumnEdit = this.editSpin;
                SdoCapital.OptionsColumn.AllowEdit = false;
                SdoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoCapital.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod2.Columns.Add(SdoCapital);

                GridColumn SdoVencido = new GridColumn();
                SdoVencido.FieldName = this._unboundPrefix + "SdoVencido";
                SdoVencido.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoVencido");
                SdoVencido.UnboundType = UnboundColumnType.Decimal;
                SdoVencido.VisibleIndex = 7;
                SdoVencido.Width = 80;
                SdoVencido.Visible = true;
                SdoVencido.ColumnEdit = this.editSpin;
                SdoVencido.OptionsColumn.AllowEdit = false;
                SdoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoVencido.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod2.Columns.Add(SdoVencido);

                //GridColumn SdoCredito = new GridColumn();
                //SdoCredito.FieldName = this._unboundPrefix + "SdoCredito";
                //SdoCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoCredito");
                //SdoCredito.UnboundType = UnboundColumnType.Integer;
                //SdoCredito.VisibleIndex = 8;
                //SdoCredito.Width = 80;
                //SdoCredito.Visible = true;
                //SdoCredito.ColumnEdit = this.editSpin;
                //SdoCredito.OptionsColumn.AllowEdit = false;
                //SdoCredito.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //SdoCredito.AppearanceCell.Options.UseTextOptions = true;

                //this.gvObligacionesCod2.Columns.Add(SdoCredito);

                // Cubrimiento
                GridColumn Cubrimiento = new GridColumn();
                Cubrimiento.FieldName = this._unboundPrefix + "Cubrimiento";
                Cubrimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cubrimiento");
                Cubrimiento.UnboundType = UnboundColumnType.Decimal;
                Cubrimiento.VisibleIndex = 9;
                Cubrimiento.Width = 80;
                Cubrimiento.Visible = true;
                Cubrimiento.OptionsColumn.AllowEdit = false;
                Cubrimiento.ColumnEdit = this.editNums;
                Cubrimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cubrimiento.AppearanceCell.Options.UseTextOptions = true;
                this.gvObligacionesCod2.Columns.Add(Cubrimiento);

                //// Cancela
                //GridColumn Cancela = new GridColumn();
                //Cancela.FieldName = this._unboundPrefix + "IndRestructurado";
                //Cancela.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cancela");
                //Cancela.UnboundType = UnboundColumnType.Boolean;
                //Cancela.VisibleIndex = 10;
                //Cancela.Width = 50;
                //Cancela.Visible = true;
                //Cancela.OptionsColumn.AllowEdit = false;
                ////Cancela.ColumnEdit = this.editCant;
                ////Cancela.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //Cancela.AppearanceCell.Options.UseTextOptions = true;
                //this.gvObligacionesCod2.Columns.Add(Cancela);




                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantiass.cs", "AddGridCols"));
            }
        }
        #endregion
        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;
            //this.dtFechaInicial.EditValue = DateTime.Now;
            //this.dtFechaCorte.EditValue = DateTime.Now;
            this.cursor = new List<DTO_QueryObligaciones>();
            this.gcObligaciones.DataSource = this.cursor;

            this.cursorGarantia = new List<DTO_QueryGarantiaControl>();
            this.gcGarantias.DataSource = this.cursorGarantia;

            this.clienteID = string.Empty;
            this.validate = true;
        }

        /// <summary>
        /// Funcion que realiza la opreacion de busqueda
        /// </summary>
        private void LoadData()
        {
            DTO_QueryObligaciones DatoObligacion = new DTO_QueryObligaciones();
            try
            {
                this.cursorGarantia = this._bc.AdministrationModel.glGarantiaControl_Decisor(this.numerodoc);
                this.Gardeudor = this.cursorGarantia.FindAll(x => x.TipoPersona.Value == 1);

                List<DTO_QueryGarantiaControl> Garconyuge = this.cursorGarantia.FindAll(x => x.TipoPersona.Value == 2);
                List<DTO_QueryGarantiaControl> Garcodeudor1 = this.cursorGarantia.FindAll(x => x.TipoPersona.Value == 3);
                List<DTO_QueryGarantiaControl> Garcodeudor2 = this.cursorGarantia.FindAll(x => x.TipoPersona.Value == 4);
                List<DTO_QueryGarantiaControl> Garcodeudor3 = this.cursorGarantia.FindAll(x => x.TipoPersona.Value == 5);

                this.cursor = this._bc.AdministrationModel.Obligaciones(this.numerodoc);
                this.Obldeudor = this.cursor.FindAll(x => x.TipoPersona.Value == 1);
                List<DTO_QueryObligaciones> Oblconyuge = this.cursor.FindAll(x => x.TipoPersona.Value == 2);
                List<DTO_QueryObligaciones> Oblcodeudor1 = this.cursor.FindAll(x => x.TipoPersona.Value == 3);
                List<DTO_QueryObligaciones> Oblcodeudor2 = this.cursor.FindAll(x => x.TipoPersona.Value == 4);
                List<DTO_QueryObligaciones> Oblcodeudor3 = this.cursor.FindAll(x => x.TipoPersona.Value == 5);

                this.tpDatosDeudor.PageVisible = false;
                this.tpDatosConyuge.PageVisible = false;
                this.tpDatosCod1.PageVisible = false;
                this.tpDatosCod2.PageVisible = false;
                _TotalGarantia = 0;
                _SaldoCapital = 0;
                _SaldoVencido = 0;
                _SaldoCredito = 0;

                if (Gardeudor != null || Obldeudor != null)
                {
                    foreach (DTO_QueryGarantiaControl garantia in Gardeudor)
                    {
                        _TotalGarantia += Convert.ToDecimal(garantia.VlrGarantia.Value);
                    }
                    DTO_QueryGarantiaControl DatoGarantiaDeudor = new DTO_QueryGarantiaControl();
                    DatoGarantiaDeudor.NroDoc.Value = "TOTAL";
                    DatoGarantiaDeudor.VlrGarantia.Value = _TotalGarantia;
                    Gardeudor.Add(DatoGarantiaDeudor);


                    foreach (DTO_QueryObligaciones obligacion in Obldeudor)
                    {
                        _SaldoCapital += Convert.ToDecimal(obligacion.SdoCapital.Value);
                        _SaldoVencido += Convert.ToDecimal(obligacion.SdoVencido.Value);
                        _SaldoCredito += Convert.ToDecimal(obligacion.SdoCredito.Value);
                    }
                    DatoObligacion = new DTO_QueryObligaciones();
                    DatoObligacion.Pagare.Value = "TOTAL";
                    DatoObligacion.SdoCapital.Value = _SaldoCapital;
                    DatoObligacion.SdoVencido.Value = _SaldoVencido;
                    DatoObligacion.SdoCredito.Value = _SaldoCredito;
                    if (_TotalGarantia != 0)
                        DatoObligacion.Cubrimiento.Value = Math.Round(100 * _SaldoCapital / _TotalGarantia, 2);
                    else
                        DatoObligacion.Cubrimiento.Value = 0;
                    Obldeudor.Add(DatoObligacion);

                    this.tpDatosDeudor.PageVisible = true;
                    this.gcObligaciones.DataSource = null;
                    this.gcObligaciones.DataSource = Obldeudor;
                    this.gcObligaciones.RefreshDataSource();
                    this.gvObligaciones.MoveFirst();

                    this.gcGarantias.DataSource = null;
                    this.gcGarantias.DataSource = Gardeudor;
                    this.gcGarantias.RefreshDataSource();
                    this.gvGarantias.MoveFirst();

                }
                _TotalGarantia = 0;
                _SaldoCapital = 0;
                _SaldoVencido = 0;
                _SaldoCredito = 0;

                if (Garconyuge.Count>0|| Oblconyuge.Count>0)
                {

                    foreach (DTO_QueryGarantiaControl garantia in Garconyuge)
                    {
                        _TotalGarantia += Convert.ToDecimal(garantia.VlrGarantia.Value);
                    }
                    DTO_QueryGarantiaControl DatoGarantiaCony = new DTO_QueryGarantiaControl();
                    DatoGarantiaCony.NroDoc.Value = "TOTAL";
                    DatoGarantiaCony.VlrGarantia.Value = _TotalGarantia;
                    Garconyuge.Add(DatoGarantiaCony);
                    foreach (DTO_QueryObligaciones obligacion in Oblconyuge)
                    {
                        _SaldoCapital += Convert.ToDecimal(obligacion.SdoCapital.Value);
                        _SaldoVencido += Convert.ToDecimal(obligacion.SdoVencido.Value);
                        _SaldoCredito += Convert.ToDecimal(obligacion.SdoCredito.Value);
                    }
                    DatoObligacion = new DTO_QueryObligaciones();
                    DatoObligacion.Pagare.Value = "TOTAL";
                    DatoObligacion.SdoCapital.Value = _SaldoCapital;
                    DatoObligacion.SdoVencido.Value = _SaldoVencido;
                    DatoObligacion.SdoCredito.Value = _SaldoCredito;
                    if (_TotalGarantia != 0)
                        DatoObligacion.Cubrimiento.Value = Math.Round(100 * _SaldoCapital / _TotalGarantia, 2);
                    else
                        DatoObligacion.Cubrimiento.Value = 0;
                    Oblconyuge.Add(DatoObligacion);
                    
                    this.tpDatosConyuge.PageVisible = true;
                    this.gcObligacionesCony.DataSource = null;
                    this.gcObligacionesCony.DataSource = Oblconyuge;
                    this.gcObligacionesCony.RefreshDataSource();
                    this.gvObligacionesCony.MoveFirst();

                    this.gcGarantiasCony.DataSource = null;
                    this.gcGarantiasCony.DataSource = Garconyuge;
                    this.gcGarantiasCony.RefreshDataSource();
                    this.gvGarantiasCony.MoveFirst();

                }

                _TotalGarantia = 0;
                _SaldoCapital = 0;
                _SaldoVencido = 0;
                _SaldoCredito = 0;

                if (Garcodeudor1.Count > 0 || Oblcodeudor1.Count > 0)
                {

                    foreach (DTO_QueryGarantiaControl garantia in Garcodeudor1)
                    {
                        _TotalGarantia += Convert.ToDecimal(garantia.VlrGarantia.Value);
                    }
                    DTO_QueryGarantiaControl DatoGarantiaCod1 = new DTO_QueryGarantiaControl();
                    DatoGarantiaCod1.NroDoc.Value = "TOTAL";
                    DatoGarantiaCod1.VlrGarantia.Value = _TotalGarantia;
                    Garcodeudor1.Add(DatoGarantiaCod1);

                    foreach (DTO_QueryObligaciones obligacion in Oblcodeudor1)
                    {
                        _SaldoCapital += Convert.ToDecimal(obligacion.SdoCapital.Value);
                        _SaldoVencido += Convert.ToDecimal(obligacion.SdoVencido.Value);
                        _SaldoCredito += Convert.ToDecimal(obligacion.SdoCredito.Value);
                    }
                    DatoObligacion = new DTO_QueryObligaciones();
                    DatoObligacion.Pagare.Value = "TOTAL";
                    DatoObligacion.SdoCapital.Value = _SaldoCapital;
                    DatoObligacion.SdoVencido.Value = _SaldoVencido;
                    DatoObligacion.SdoCredito.Value = _SaldoCredito;
                    if (_TotalGarantia != 0)
                        DatoObligacion.Cubrimiento.Value = Math.Round(100 * _SaldoCapital / _TotalGarantia, 2);
                    else
                        DatoObligacion.Cubrimiento.Value = 0;
                    Oblcodeudor1.Add(DatoObligacion);

                    this.tpDatosCod1.PageVisible = true;
                    this.gcObligacionesCod1.DataSource = null;
                    this.gcObligacionesCod1.DataSource = Oblcodeudor1;
                    this.gcObligacionesCod1.RefreshDataSource();
                    this.gvObligacionesCod1.MoveFirst();

                    this.gcGarantiasCod1.DataSource = null;
                    this.gcGarantiasCod1.DataSource = Garcodeudor1;
                    this.gcGarantiasCod1.RefreshDataSource();
                    this.gvGarantiasCod1.MoveFirst();
                }

                _TotalGarantia = 0;
                _SaldoCapital = 0;
                _SaldoVencido = 0;
                _SaldoCredito = 0;

                if (Garcodeudor2.Count > 0 || Oblcodeudor2.Count > 0)
                {
                    foreach (DTO_QueryGarantiaControl garantia in Garcodeudor2)
                    {
                        _TotalGarantia += Convert.ToDecimal(garantia.VlrGarantia.Value);
                    }
                    DTO_QueryGarantiaControl DatoGarantiaCod2 = new DTO_QueryGarantiaControl();
                    DatoGarantiaCod2.NroDoc.Value = "TOTAL";
                    DatoGarantiaCod2.VlrGarantia.Value = _TotalGarantia;
                    Garcodeudor2.Add(DatoGarantiaCod2);

                    foreach (DTO_QueryObligaciones obligacion in Oblcodeudor2)
                    {

                        _SaldoCapital += Convert.ToDecimal(obligacion.SdoCapital.Value);
                        _SaldoVencido += Convert.ToDecimal(obligacion.SdoVencido.Value);
                        _SaldoCredito += Convert.ToDecimal(obligacion.SdoCredito.Value);
                    }

                    DatoObligacion = new DTO_QueryObligaciones();
                    DatoObligacion.Pagare.Value = "TOTAL";
                    DatoObligacion.SdoCapital.Value = _SaldoCapital;
                    DatoObligacion.SdoVencido.Value = _SaldoVencido;
                    DatoObligacion.SdoCredito.Value = _SaldoCredito;
                    if (_TotalGarantia != 0)
                        DatoObligacion.Cubrimiento.Value = Math.Round(100 * _SaldoCapital / _TotalGarantia, 2);
                    else
                        DatoObligacion.Cubrimiento.Value = 0;
                    Oblcodeudor2.Add(DatoObligacion);

                    this.tpDatosCod2.PageVisible = true;
                    this.gcObligacionesCod2.DataSource = null;
                    this.gcObligacionesCod2.DataSource = Oblcodeudor2;
                    this.gcObligacionesCod2.RefreshDataSource();
                    this.gvObligacionesCod2.MoveFirst();

                    this.gcGarantiasCod2.DataSource = null;
                    this.gcGarantiasCod2.DataSource = Garcodeudor2;
                    this.gcGarantiasCod2.RefreshDataSource();
                    this.gvGarantiasCod2.MoveFirst();
                }

             

                if (this.cursor== null )
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    this.CleanData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantias.cs", "GetSearch"));
            }
        }
        #endregion Funciones Privadas

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
        }

        protected void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Visible = false;
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemUpdate.Visible = false;
                    FormProvider.Master.itemUpdate.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Visible = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                    FormProvider.Master.itemUpdate.Visible = false;
                    FormProvider.Master.itemSave.Visible = true;
                    if (_modifica)
                        FormProvider.Master.itemSave.ToolTipText = "Guarda Obligaciones";
                    else
                        FormProvider.Master.itemSave.Visible = false;

                    FormProvider.Master.itemSendtoAppr.Visible = false;
                    //FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                    FormProvider.Master.itemSave.Enabled = true;
                    FormProvider.Master.itemSendtoAppr.Enabled = true;
                    //FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantias.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantias.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaObligacionesGarantias.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

                /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result = _bc.AdministrationModel.drSolicitudObligaciones_Update(this.Obldeudor);
                result = _bc.AdministrationModel.drSolicitudGarantias_Update(this.Gardeudor);


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-drSolicitudLibranza.cs", "TBSave"));
            }
        }
        #endregion Eventos Formulario

        #region Eventos Grilla


        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvObligaciones_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

            try
            {


                if (e.FocusedRowHandle >= 0)
                {
                    int row = e.FocusedRowHandle;
                    if (row != null && this.cursor!=null)
                    { 
                        this.rowCurrent= (DTO_QueryObligaciones)this.gvObligaciones.GetRow(row);
                        this.gvObligaciones.Columns[this._unboundPrefix + "CancelaInd"].OptionsColumn.AllowEdit = false;
                        if (this.rowCurrent.Pagare.Value.ToUpper().TrimEnd()!="TOTAL")
                            this.gvObligaciones.Columns[this._unboundPrefix + "CancelaInd"].OptionsColumn.AllowEdit = _modifica;
                    }
                }
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
        private void gvGarantias_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this.cursorGarantia!=null)
                {
                    int row = e.FocusedRowHandle;
                    if (row != null )
                    {
                        this.rowCurrentGarantia = (DTO_QueryGarantiaControl)this.gvGarantias.GetRow(row);
                        this.gvGarantias.Columns[this._unboundPrefix + "CancelaInd"].OptionsColumn.AllowEdit = false;
                        if (this.rowCurrentGarantia.NroDoc.Value.ToUpper().TrimEnd() != "TOTAL")
                            this.gvGarantias.Columns[this._unboundPrefix + "CancelaInd"].OptionsColumn.AllowEdit = _modifica;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void gvObligaciones_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "CancelaInd")
            {
                this.NumObligaciones = this.cursor.Count(x => x.CancelaInd.Value.Value);
            }

        }
        private void gvGarantias_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "CancelaInd")
            {
                this.NumGarantias = this.cursorGarantia.Count(x => x.CancelaInd.Value.Value);
            }
        }


        private void gvObligaciones_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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

        //protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    try
        //    {
        //        //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
        //        string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

        //        #region Generales
        //        if (fieldName == "Porcentaje")
        //        {
        //            decimal cantidad = Convert.ToDecimal(e.Value);

        //            this.detComisiones = (DTO_ccComisionDeta)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);

        //            if (cantidad == Convert.ToDecimal(this.detComisiones.Porcentaje.Value))
        //            {
        //                //this.periodo = Convert.ToDateTime(strPeriodo);
        //            }
        //            else
        //            {
        //                this.comisiones[this._gridDetalleCurrent.FocusedRowHandle].Porcentaje.Value = cantidad;
        //                this.CalcularValoresGrid();
        //                this.gcDocument.DataSource = this.comisiones;
        //                this.gvDocument.BestFitColumns();
        //                this.gvDetalle.BestFitColumns();
        //                //this.gvDocument.MoveFirst();


        //            }
        //        }

        //        #endregion

        //        this.gcDocument.RefreshDataSource();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionComisiones.cs", "gvDocument_CellValueChanging"));
        //    }
        //}

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvObligaciones_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            //string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            //if (fieldName == "Etapa" && e.Value!=null)
            //    e.DisplayText = e.Value.ToString();
            //else if (fieldName == "ViewDoc")
            //    e.DisplayText = "Ver Documento";
        }


        #endregion

        #region barraherramientas
        public override void TBUpdate()
        {
            this.LoadData();
        }
        #endregion




    }
}