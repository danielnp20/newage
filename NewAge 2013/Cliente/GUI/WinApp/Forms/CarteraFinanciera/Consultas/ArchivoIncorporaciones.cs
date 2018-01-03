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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ArchivoIncorporaciones : FormWithToolbar
    {
        #region Delegados

        private void RefreshDataMethod()
        {
            this.comboTipoIncorp.SelectedIndex = -1;

            this.firstTime = true;
            this.LoadDocuments();
        }

        #endregion

        public ArchivoIncorporaciones()
        {
            InitializeComponent();
            this.SetInitParameters();
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
            FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
        }

        #region Variables
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //Variables Privadas
        private bool firstTime = true;
        private string centroPagoID = string.Empty;
        private object currentDoc;
        private bool incorporaLiquida;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query; private string _unboundPrefix = "Unbound_";
        private string _frmName;

        //DTO
        private List<DTO_ccIncorporaciones> incorporados = new List<DTO_ccIncorporaciones>();
        private DTO_ccCentroPagoPAG centroPago = new DTO_ccCentroPagoPAG();
        
        //Reporte
        string reportName;
        string fileURl;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.ArchivosIncorporaciones;
            this._frmModule = ModulesPrefix.cc;

            //Carga los recursos del combo
            TablesResources.GetTableResources(this.comboTipoIncorp, typeof(TipoIncorporaCartera));
            this.comboTipoIncorp.SelectedIndex = -1;

            this.AddGridCols();
            this.Initontrols();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        private void Initontrols()
        {
            //Inicia los controles Master Find
            this._bc.InitMasterUC(this.uc_MasterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
            this.dtFechaIncorpora.DateTime = DateTime.Now;
            //this.comboTipoIncorp.
        }

        /// <summary>
        /// Funcion que limpia los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            //Header
            this.comboTipoIncorp.SelectedIndex = -1;
            this.uc_MasterCentroPago.Value = String.Empty;

            //Variables
            this.centroPagoID = String.Empty;
            this.incorporaLiquida = false;

            this.gcDocument.Enabled = true;
            this.gcDocument.DataSource = null;
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Campo de Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this._unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 2;
                Libranza.Width = 65;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Libranza);

                //Cliente Id
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this._unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 3;
                ClienteID.Width = 65;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ClienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this._unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 4;
                nombCliente.Width = 110;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombCliente);

                //Valor Libranza
                GridColumn VlrLibranza = new GridColumn();
                VlrLibranza.FieldName = this._unboundPrefix + "VlrLibranza";
                VlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrLibranza");
                VlrLibranza.UnboundType = UnboundColumnType.Decimal;
                VlrLibranza.VisibleIndex = 5;
                VlrLibranza.Width = 120;
                VlrLibranza.Visible = true;
                VlrLibranza.OptionsColumn.AllowEdit = false;
                VlrLibranza.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrLibranza);

                //Valor Cuota
                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 6;
                VlrCuota.Width = 90;
                VlrCuota.Visible = true;
                VlrCuota.OptionsColumn.AllowEdit = false;
                VlrCuota.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrCuota);

                //Fecha Cuota 1
                GridColumn FechaCuota1 = new GridColumn();
                FechaCuota1.FieldName = this._unboundPrefix + "FechaCuota1";
                FechaCuota1.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota1");
                FechaCuota1.UnboundType = UnboundColumnType.DateTime;
                FechaCuota1.VisibleIndex = 7;
                FechaCuota1.Width = 120;
                FechaCuota1.Visible = true;
                FechaCuota1.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(FechaCuota1);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this._unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 8;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Carga el cabezote con los documentos
        /// </summary>
        private void LoadDocuments()
        {
            try
            {
                this.currentDoc = null;
                this.incorporados = this._bc.AdministrationModel.CreditosIncorporados(this.dtFechaIncorpora.DateTime.ToShortDateString(), this.uc_MasterCentroPago.Value, true);

                this.gcDocument.DataSource = this.incorporados;


                if (!this.firstTime)
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                }

                this.firstTime = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncoporaciones.cs", "LoadDocuments"));
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
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
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
        /// Evento que se ejecuta al modificar la seleccion del combo, especifica como se debe filtrar la incopracion
        /// </summary>
        private void comboTipoIncorp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.firstTime)
            {
                this.centroPagoID = String.Empty;
                this.uc_MasterCentroPago.Value = string.Empty;
                this.gcDocument.DataSource = null;
            }
        }

        /// <summary>
        /// Evento que filtra los documentos de acuerdo a la pagaduria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_MasterCentroPago_Leave(object sender, EventArgs e)
        {
            if (this.centroPagoID != this.uc_MasterCentroPago.Value)
            {
                this.centroPagoID = this.uc_MasterCentroPago.Value;
                this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.uc_MasterCentroPago.Value, true);
                if (this.centroPago != null)
                {
                    this.LoadDocuments();
                }
                else
                {
                    this.CleanData();
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.uc_MasterCentroPago.LabelRsx);
                    MessageBox.Show(msg);
                }
            }
        }

        /// <summary>
        /// Boton que imprime el reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Consultar_Click(object sender, EventArgs e)
        {
            try
            {
                reportName = this._bc.AdministrationModel.Report_Cc_Incorporacion(incorporados[0].NumeroDoc.Value.Value, true, ExportFormatType.pdf);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ArchivoIncorporaciones", "btn_Consultar_Click"));
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

    }
}
