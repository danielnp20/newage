using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Collections;
using SentenceTransformer;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class GestionCobranza : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public void RefreshDataMethod()
        {
            this.LoadData();
        }

        #endregion

        #region Variables
        private int _documentID;
        private ModulesPrefix frmModule;

        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string _unboundPrefix = "Unbound_";
        List<DTO_GestionCobranza> cobranzas = new List<DTO_GestionCobranza>();
        private bool deleteOP = false;
        private bool isValid = false;
        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public GestionCobranza()
        {
            this.Constructor(string.Empty);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public GestionCobranza(string mod)
        {
            this.Constructor(string.Empty,mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public GestionCobranza(string cliente, string mod)
        {
            this.Constructor(cliente,mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        private void Constructor(string cliente, string mod = null)
        {
            try
            {
                this.InitializeComponent();

                this.frmModule = mod == null? ModulesPrefix.cc : ModulesPrefix.cf;
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this.AddGridCols();
                this.refreshData = new RefreshData(RefreshDataMethod);

                if (!string.IsNullOrEmpty(cliente))
                {
                    this.masterCliente.Value = cliente;
                    this.LoadData();
                }                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionCobranza.cs", "GestionCobranza"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.GestionCobranza;           

            this.gcData.ShowOnlyPredefinedDetails = true;
            this.InitControls();
        }

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            //Tipo
            Dictionary<string, string> dicOrden = new Dictionary<string, string>();
            dicOrden.Add("1", this._bc.GetResource(LanguageTypes.Tables, "Cliente"));
            dicOrden.Add("2", this._bc.GetResource(LanguageTypes.Tables, "Vencimiento"));
            this.cmbOrden.Properties.DataSource = dicOrden;
            this.cmbOrden.EditValue = 1;
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadData()
        {
            try
            {
                if (this.ValidateHeader())
                {
                    DTO_GestionCobranza filter = new DTO_GestionCobranza();
                    this.gcData.DataSource = null;
                    this.cobranzas = this._bc.AdministrationModel.ccCierreDiaCartera_GetGestionCobranza(this.masterCliente.Value, Convert.ToByte(this.cmbOrden.EditValue));

                    this.gcData.DataSource = this.cobranzas;
                    this.gvData.RefreshData();
                    this.gvData.FocusedRowHandle = this.gvData.DataRowCount - 1;
                }
            }
            catch (Exception)
            {
                ;
            }

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Principal
                //ClienteDesc
                GridColumn ClienteDesc = new GridColumn();
                ClienteDesc.FieldName = this._unboundPrefix + "ClienteDesc";
                ClienteDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteDesc");
                ClienteDesc.UnboundType = UnboundColumnType.String;
                ClienteDesc.VisibleIndex = 1;
                ClienteDesc.Width = 60;
                ClienteDesc.Visible = false;
                ClienteDesc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(ClienteDesc);

                //Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this._unboundPrefix + "Libranza";
                Libranza.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.String;
                Libranza.VisibleIndex = 2;
                Libranza.Width = 60;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(Libranza);

                //FechaMora(FechaVto)
                GridColumn FechaMora = new GridColumn();
                FechaMora.FieldName = this._unboundPrefix + "FechaMora";
                FechaMora.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaMora");
                FechaMora.UnboundType = UnboundColumnType.DateTime;
                FechaMora.VisibleIndex = 3;
                FechaMora.Width = 45;
                FechaMora.Visible = true;
                FechaMora.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(FechaMora);

                //DiasMora
                GridColumn DiasMora = new GridColumn();
                DiasMora.FieldName = this._unboundPrefix + "DiasMora";
                DiasMora.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_DiasMora");
                DiasMora.UnboundType = UnboundColumnType.Integer;
                DiasMora.VisibleIndex = 4;
                DiasMora.Width = 40;
                DiasMora.Visible = true;
                DiasMora.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(DiasMora);

                //TotalLibranza
                GridColumn TotalLibranza = new GridColumn();
                TotalLibranza.FieldName = this._unboundPrefix + "TotalLibranza";
                TotalLibranza.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalLibranza");
                TotalLibranza.UnboundType = UnboundColumnType.Decimal;
                TotalLibranza.VisibleIndex = 5;
                TotalLibranza.Width = 80;
                TotalLibranza.Visible = true;
                TotalLibranza.ColumnEdit = this.editSpin;
                TotalLibranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                TotalLibranza.AppearanceCell.Options.UseTextOptions = true;
                TotalLibranza.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(TotalLibranza);

                //Numero1(Compromiso Incump)
                GridColumn Numero1 = new GridColumn();
                Numero1.FieldName = this._unboundPrefix + "Numero1";
                Numero1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Numero1");
                Numero1.UnboundType = UnboundColumnType.Integer;
                Numero1.VisibleIndex = 6;
                Numero1.Width = 40;
                Numero1.Visible = true;
                Numero1.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(Numero1);

                //FechaCompromiso
                GridColumn FechaCompromiso = new GridColumn();
                FechaCompromiso.FieldName = this._unboundPrefix + "FechaCompromiso";
                FechaCompromiso.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCompromiso");
                FechaCompromiso.UnboundType = UnboundColumnType.DateTime;
                FechaCompromiso.VisibleIndex = 7;
                FechaCompromiso.Width = 40;
                FechaCompromiso.Visible = true;
                FechaCompromiso.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(FechaCompromiso);

                //Valor1(Valor a Pagar)
                GridColumn Valor1 = new GridColumn();
                Valor1.FieldName = this._unboundPrefix + "Valor1";
                Valor1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor1");
                Valor1.UnboundType = UnboundColumnType.Decimal;
                Valor1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor1.AppearanceCell.Options.UseTextOptions = true;
                Valor1.VisibleIndex = 8;
                Valor1.Width = 50;
                Valor1.Visible = true;
                Valor1.ColumnEdit = this.editSpin;
                Valor1.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(Valor1);

                //ObservacionCompromiso
                GridColumn ObservacionCompromiso = new GridColumn();
                ObservacionCompromiso.FieldName = this._unboundPrefix + "ObservacionCompromiso";
                ObservacionCompromiso.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ObservacionCompromiso");
                ObservacionCompromiso.UnboundType = UnboundColumnType.String;
                ObservacionCompromiso.VisibleIndex = 9;
                ObservacionCompromiso.Width = 120;
                ObservacionCompromiso.Visible = true;
                ObservacionCompromiso.ColumnEdit = this.editPopUp;
                ObservacionCompromiso.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(ObservacionCompromiso);

                //CumplidoInd
                GridColumn CumplidoInd = new GridColumn();
                CumplidoInd.FieldName = this._unboundPrefix + "CumplidoInd";
                CumplidoInd.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CumplidoInd");
                CumplidoInd.UnboundType = UnboundColumnType.Boolean;
                CumplidoInd.VisibleIndex = 10;
                CumplidoInd.Width = 40;
                CumplidoInd.Visible = true;
                CumplidoInd.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(CumplidoInd);

                #region Columnas no visibles
                //Observaciones
                GridColumn Observaciones = new GridColumn();
                Observaciones.FieldName = this._unboundPrefix + "Observaciones";
                Observaciones.UnboundType = UnboundColumnType.String;
                Observaciones.Visible = false;
                this.gvData.Columns.Add(Observaciones);
                
                #endregion
                #endregion
                #region Grilla Detalle
                //Plazo
                GridColumn Plazo = new GridColumn();
                Plazo.FieldName = this._unboundPrefix + "Plazo";
                Plazo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                Plazo.UnboundType = UnboundColumnType.Integer;
                Plazo.VisibleIndex = 4;
                Plazo.Width = 45;
                Plazo.Visible = true;
                Plazo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Plazo);

                //CtasVencidas
                GridColumn CtasVencidas = new GridColumn();
                CtasVencidas.FieldName = this._unboundPrefix + "CtasVencidas";
                CtasVencidas.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CtasVencidas");
                CtasVencidas.UnboundType = UnboundColumnType.Integer;
                CtasVencidas.VisibleIndex = 4;
                CtasVencidas.Width = 45;
                CtasVencidas.Visible = true;
                CtasVencidas.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CtasVencidas);

                //SaldoVencido
                GridColumn SaldoVencido = new GridColumn();
                SaldoVencido.FieldName = this._unboundPrefix + "SaldoVencido";
                SaldoVencido.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoVencido");
                SaldoVencido.UnboundType = UnboundColumnType.Decimal;
                SaldoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoVencido.AppearanceCell.Options.UseTextOptions = true;
                SaldoVencido.VisibleIndex = 5;
                SaldoVencido.Width = 60;
                SaldoVencido.Visible = true;
                SaldoVencido.ColumnEdit = this.editSpin;
                SaldoVencido.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SaldoVencido);

                //VlrIntMora
                GridColumn VlrIntMora = new GridColumn();
                VlrIntMora.FieldName = this._unboundPrefix + "VlrIntMora";
                VlrIntMora.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrIntMora");
                VlrIntMora.UnboundType = UnboundColumnType.Decimal;
                VlrIntMora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrIntMora.AppearanceCell.Options.UseTextOptions = true;
                VlrIntMora.VisibleIndex = 5;
                VlrIntMora.Width = 60;
                VlrIntMora.Visible = true;
                VlrIntMora.ColumnEdit = this.editSpin;
                VlrIntMora.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(VlrIntMora);

                //VlrPrejuridico
                GridColumn VlrPrejuridico = new GridColumn();
                VlrPrejuridico.FieldName = this._unboundPrefix + "VlrPrejuridico";
                VlrPrejuridico.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrejuridico");
                VlrPrejuridico.UnboundType = UnboundColumnType.Decimal;
                VlrPrejuridico.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPrejuridico.AppearanceCell.Options.UseTextOptions = true;
                VlrPrejuridico.VisibleIndex = 5;
                VlrPrejuridico.Width = 60;
                VlrPrejuridico.Visible = true;
                VlrPrejuridico.ColumnEdit = this.editSpin;
                VlrPrejuridico.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(VlrPrejuridico);

                //VlrOtros
                GridColumn VlrOtros = new GridColumn();
                VlrOtros.FieldName = this._unboundPrefix + "VlrOtros";
                VlrOtros.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtros");
                VlrOtros.UnboundType = UnboundColumnType.Decimal;
                VlrOtros.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrOtros.AppearanceCell.Options.UseTextOptions = true;
                VlrOtros.VisibleIndex = 5;
                VlrOtros.Width = 60;
                VlrOtros.Visible = true;
                VlrOtros.ColumnEdit = this.editSpin;
                VlrOtros.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(VlrOtros);

                //TotalLibranzaDet
                GridColumn TotalLibranzaDet = new GridColumn();
                TotalLibranzaDet.FieldName = this._unboundPrefix + "TotalLibranza";
                TotalLibranzaDet.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalLibranza");
                TotalLibranzaDet.UnboundType = UnboundColumnType.Decimal;
                TotalLibranzaDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                TotalLibranzaDet.AppearanceCell.Options.UseTextOptions = true;
                TotalLibranzaDet.VisibleIndex = 5;
                TotalLibranzaDet.Width = 80;
                TotalLibranzaDet.Visible = true;
                TotalLibranzaDet.ColumnEdit = this.editSpin;
                TotalLibranzaDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(TotalLibranzaDet);

                #region Columnas no visibles
                ////Observaciones
                //GridColumn Observaciones = new GridColumn();
                //Observaciones.FieldName = this._unboundPrefix + "Observaciones";
                //Observaciones.UnboundType = UnboundColumnType.String;
                //Observaciones.Visible = false;
                //this.gvDetalle.Columns.Add(Observaciones);

                #endregion
                #endregion
                #region Grilla Codeudor
                //ClienteDescCod
                GridColumn ClienteDescCod = new GridColumn();
                ClienteDescCod.FieldName = this._unboundPrefix + "ClienteDesc";
                ClienteDescCod.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteDesc");
                ClienteDescCod.UnboundType = UnboundColumnType.String;
                ClienteDescCod.VisibleIndex = 1;
                ClienteDescCod.Width = 100;
                ClienteDescCod.Visible = true;
                ClienteDescCod.OptionsColumn.AllowEdit = false;
                this.gvCodeudor.Columns.Add(ClienteDescCod);

                //ProfesionDesc
                GridColumn ProfesionDesc = new GridColumn();
                ProfesionDesc.FieldName = this._unboundPrefix + "ProfesionDes";
                ProfesionDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProfesionDesc");
                ProfesionDesc.UnboundType = UnboundColumnType.String;
                ProfesionDesc.VisibleIndex = 2;
                ProfesionDesc.Width = 30;
                ProfesionDesc.Visible = true;
                ProfesionDesc.OptionsColumn.AllowEdit = false;
                this.gvCodeudor.Columns.Add(ProfesionDesc);

                //Telefono
                GridColumn Telefono = new GridColumn();
                Telefono.FieldName = this._unboundPrefix + "Telefono";
                Telefono.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Telefono");
                Telefono.UnboundType = UnboundColumnType.String;
                Telefono.VisibleIndex = 3;
                Telefono.Width = 40;
                Telefono.Visible = true;
                Telefono.OptionsColumn.AllowEdit = false;
                this.gvCodeudor.Columns.Add(Telefono);

                //Celular
                GridColumn Celular = new GridColumn();
                Celular.FieldName = this._unboundPrefix + "Celular";
                Celular.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Celular");
                Celular.UnboundType = UnboundColumnType.String;
                Celular.VisibleIndex = 4;
                Celular.Width = 50;
                Celular.Visible = true;
                Celular.OptionsColumn.AllowEdit = false;
                this.gvCodeudor.Columns.Add(Celular);

                //Correo
                GridColumn Correo = new GridColumn();
                Correo.FieldName = this._unboundPrefix + "Correo";
                Correo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Correo");
                Correo.UnboundType = UnboundColumnType.String;
                Correo.VisibleIndex = 5;
                Correo.Width = 60;
                Correo.Visible = true;
                Correo.OptionsColumn.AllowEdit = false;
                this.gvCodeudor.Columns.Add(Correo);

                //ResidenciaDir
                GridColumn ResidenciaDir = new GridColumn();
                ResidenciaDir.FieldName = this._unboundPrefix + "ResidenciaDir";
                ResidenciaDir.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ResidenciaDir");
                ResidenciaDir.UnboundType = UnboundColumnType.String;
                ResidenciaDir.VisibleIndex = 6;
                ResidenciaDir.Width = 60;
                ResidenciaDir.Visible = true;
                ResidenciaDir.OptionsColumn.AllowEdit = false;
                this.gvCodeudor.Columns.Add(ResidenciaDir);

                #endregion
                this.gvData.OptionsView.ColumnAutoWidth = true;
                this.gvDetalle.OptionsView.ColumnAutoWidth = true;
                this.gvCodeudor.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TerceroDataForm.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        private bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                //#region FechaVto
                //DTO_prContratoPolizas pol = (DTO_prContratoPolizas)this.gvGarantia.GetFocusedRow();
                //string msg = pol != null && pol.FechaVto.Value != null ? string.Empty : "Fecha Vencimiento Vacio";
                //GridColumn col = this.gvGarantia.Columns[this._unboundPrefix + "FechaVto"];
                //this.gvGarantia.SetColumnError(col, msg);
                //validField = string.IsNullOrEmpty(msg) ? true : false;
                //if (!validField)
                //    validRow = false;
                //#endregion
                //#region Observacion
                //validField = _bc.ValidGridCell(this.gvGarantia, string.Empty, fila, "Observacion", false, false, false, null);
                //if (!validField)
                //    validRow = false;
                //#endregion
                this.isValid = validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            #region Valida datos en la maestra de cliente
            if (!this.masterCliente.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.CodeRsx);

                MessageBox.Show(msg);
                this.masterCliente.Focus();

                return false;
            }
            #endregion
            return true;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                    FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionCobranza.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionCobranza.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionCobranza.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionCobranza.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                Object dto = (Object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                                pi.PropertyType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                    if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                        e.Value = 0;
                }
                if (e.IsSetData)
                {

                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            if (!string.IsNullOrEmpty(e.Value.ToString()))
                                udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                            else
                                udtProp.SetValueFromString(e.Value.ToString());
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                                pi.PropertyType.Name == "Double")
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
                ;
            }
        }       

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.gvData.FocusedRowHandle >= 0)
                {
                    DTO_GestionCobranza gestion = (DTO_GestionCobranza)this.gvData.GetRow( e.FocusedRowHandle);
                    this.gcCodeudor.DataSource = gestion != null? gestion.CodeudorDet : null;
                    this.txtHistoria.EditValue = gestion.ObservacionHistoria.Value;
                    this.gvCodeudor.RefreshData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines cuando los valores etstan engresados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            this.ValidateRow(e.RowHandle);
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
            this.deleteOP = false;

            if (validRow)
            {
                this.isValid = true;
            }
            else
            {
                e.Allow = false;
                this.isValid = false;
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "VerDoc")
            {
                if (!string.IsNullOrEmpty(this.cobranzas[e.ListSourceRowIndex].NumeroDoc.Value.ToString()))
                    e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
            }
            if (fieldName == "UnidadTiempo")
            {
                if (Convert.ToInt32(e.Value) == 1 || (Convert.ToInt32(e.Value) == 2))
                    e.DisplayText = "Hora";
                else if (Convert.ToInt32(e.Value) == 3 || (Convert.ToInt32(e.Value) == 4) || (Convert.ToInt32(e.Value) == 5))
                    e.DisplayText = "Día";
            }
            if (fieldName == "Estado")
            {
                if (Convert.ToInt32(e.Value) == 0)
                    e.DisplayText = "AC";
                else
                    e.DisplayText = "CE";
            }

        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.cobranzas = new List<DTO_GestionCobranza>();
                this.gcData.DataSource = null;              
                this.gcCodeudor.DataSource = null;
                this.gcData.RefreshDataSource();
                this.gcCodeudor.RefreshDataSource();
                this.txtHistoria.EditValue = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TerceroDataForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvData.PostEditor();
            try
            {
                if (this.cobranzas.Any(x=>x.FechaCompromiso.Value == null))
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgFechaEmpty = _bc.GetResource(LanguageTypes.Messages, "No ha registrado Fecha de Compromiso en los datos modificados, desea continuar");

                    if (MessageBox.Show(msgFechaEmpty, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    } 
                }
                else
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Se realiza al salir del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterTercero_Leave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)sender;
                    //int fila = this.gvData.FocusedRowHandle;
                    //if (txt.Name == "txtCodigoGaranPre")
                    //    this.detalle[fila].CodigoGarantia.Value = txt.Text;                    
                }
                if (sender.GetType() == typeof(TextEdit))
                {
                    TextEdit txt = (TextEdit)sender;
                    int fila = this.gvData.FocusedRowHandle;
                    //if (txt.Name == "txtVlrFuente" && !string.IsNullOrEmpty(txt.EditValue.ToString()))
                    //    this.detalle[fila].VlrFuente.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    //if (txt.Name == "txtVlrAsegurado" && !string.IsNullOrEmpty(txt.EditValue.ToString()))
                    //    this.detalle[fila].VlrAsegurado.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                }               
                if (sender.GetType() == typeof(ControlsUC.uc_MasterFind))
                {
                    ControlsUC.uc_MasterFind txt = (ControlsUC.uc_MasterFind)sender;
                    int fila = this.gvData.FocusedRowHandle;
                }
                this.gvData.RefreshData();
            }
            catch (Exception)
            {
                ;
            }

        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al asignar
        /// </summary>
        private void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                DTO_TxResult results = this._bc.AdministrationModel.GestionCobranza_Add(this._documentID, this.cobranzas);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(results);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionCobranza.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }

        #endregion
    }
}
