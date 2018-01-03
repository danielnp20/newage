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
    public partial class Reports_DataCredito : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private DataTable results = new DataTable();

        #endregion

        #region Constructor Reports_DataCredito

           /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Reports_DataCredito()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Reports_DataCredito(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                FormProvider.LoadResources(this, this._documentID);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "Reports_DataCredito.cs-Reports_DataCredito"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryDataCredito;
            this._frmModule = ModulesPrefix.cc;
          
            //Deshabilita los botones +- de la grilla
            this.gcGenerales.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            // Convertir fecha Incial en el 1 dia de cada mes
            string periodoStr = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.dtPeriodo.DateTime = Convert.ToDateTime(periodoStr).AddMonths(-1);
            this.dtPeriodo.Enabled = false;

            Dictionary<string, string> datosTipo = new Dictionary<string, string>();
            datosTipo.Add("0", "Cierre Mensual");
            datosTipo.Add("1", "Ajustes");
            this.cmbTipoConsulta.Properties.ValueMember = "Key";
            this.cmbTipoConsulta.Properties.DisplayMember = "Value";
            this.cmbTipoConsulta.Properties.DataSource = datosTipo;
            this.cmbTipoConsulta.EditValue = "0";

        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //TipoReports_DataCredito
                GridColumn TipoReports_DataCredito = new GridColumn();
                TipoReports_DataCredito.FieldName = this._unboundPrefix + "DocumentoID";
                TipoReports_DataCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                TipoReports_DataCredito.UnboundType = UnboundColumnType.Integer;
                TipoReports_DataCredito.VisibleIndex = 0;
                TipoReports_DataCredito.Width = 130;
                TipoReports_DataCredito.Visible = true;
                TipoReports_DataCredito.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(TipoReports_DataCredito);

                //FechaReports_DataCredito
                GridColumn FechaReports_DataCredito = new GridColumn();
                FechaReports_DataCredito.FieldName = this._unboundPrefix + "Fecha_Reports_DataCredito";
                FechaReports_DataCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaReports_DataCredito.UnboundType = UnboundColumnType.DateTime;
                FechaReports_DataCredito.VisibleIndex = 1;
                FechaReports_DataCredito.Width = 130;
                FechaReports_DataCredito.Visible = true;
                FechaReports_DataCredito.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(FechaReports_DataCredito);

                //FechaAplicacion
                GridColumn FechaAplicacion = new GridColumn();
                FechaAplicacion.FieldName = this._unboundPrefix + "FechaAplicacion";
                FechaAplicacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaAplicacion");
                FechaAplicacion.UnboundType = UnboundColumnType.DateTime;
                FechaAplicacion.VisibleIndex = 2;
                FechaAplicacion.Width = 110;
                FechaAplicacion.Visible = true;
                FechaAplicacion.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(FechaAplicacion);

                // NoLibranza
                GridColumn NoLibranza = new GridColumn();
                NoLibranza.FieldName = this._unboundPrefix + "NroCredito";
                NoLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoTercero");
                NoLibranza.UnboundType = UnboundColumnType.String; 
                NoLibranza.VisibleIndex = 3;
                NoLibranza.Width = 90;
                NoLibranza.Visible = true;
                NoLibranza.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(NoLibranza);

                //Tercero
                GridColumn Tercero = new GridColumn();
                Tercero.FieldName = this._unboundPrefix + "ClienteID";
                Tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                Tercero.UnboundType = UnboundColumnType.String; 
                Tercero.VisibleIndex = 4;
                Tercero.Width = 90;
                Tercero.Visible = true;
                Tercero.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(Tercero);

                // Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nom_Cliente";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 5;
                Nombre.Width = 130;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(Nombre);

                // Documento = PrefijoID + NomeroDoc
                GridColumn PrefNro = new GridColumn();
                PrefNro.FieldName = this._unboundPrefix + "DOCUMENTO";
                PrefNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefNro");
                PrefNro.UnboundType = UnboundColumnType.String;
                PrefNro.VisibleIndex = 6;
                PrefNro.Width = 100;
                PrefNro.Visible = true;
                PrefNro.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(PrefNro);

                //Comprobante Contable = ComponenteID + ComponenteNumero
                GridColumn ComprobanteCont = new GridColumn();
                ComprobanteCont.FieldName = this._unboundPrefix + "COMPROBANTE";
                ComprobanteCont.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComprobanteCont");
                ComprobanteCont.UnboundType = UnboundColumnType.Integer; 
                ComprobanteCont.VisibleIndex = 7;
                ComprobanteCont.Width = 120;
                ComprobanteCont.Visible = true;
                ComprobanteCont.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(ComprobanteCont);

                // Valor Cartera Total
                GridColumn ValorCarteraTotal = new GridColumn();
                ValorCarteraTotal.FieldName = this._unboundPrefix + "TotalDocumento";
                ValorCarteraTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCarteraTotala");
                ValorCarteraTotal.UnboundType = UnboundColumnType.Integer;
                ValorCarteraTotal.VisibleIndex = 8;
                ValorCarteraTotal.Width = 100;
                ValorCarteraTotal.Visible = true;
                ValorCarteraTotal.ColumnEdit = this.editValue;
                ValorCarteraTotal.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(ValorCarteraTotal);

                // Valor Pago Total
                GridColumn ValorPagoTotal = new GridColumn();
                ValorPagoTotal.FieldName = this._unboundPrefix + "TotalCuota";
                ValorPagoTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorPagoTotal");
                ValorPagoTotal.UnboundType = UnboundColumnType.Integer;
                ValorPagoTotal.VisibleIndex = 9;
                ValorPagoTotal.Width = 100;
                ValorPagoTotal.Visible = true;
                ValorPagoTotal.ColumnEdit = this.editValue;
                ValorPagoTotal.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(ValorPagoTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DataCreditos.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la info
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.results = _bc.AdministrationModel.Report_Cc_DataCredito(this.dtPeriodo.DateTime, Convert.ToByte(this.cmbTipoConsulta.EditValue));
                this.FormatResults();

                this.gcGenerales.DataSource = results;
                this.gvGenerales.PopulateColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "Reports_DataCredito.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Funcion que se encarga de Traer los recuros
        /// </summary>
        private void FormatResults()
        {
            //Clear columns 
            this.gvGenerales.Columns.Clear();

            //Colums names
            for (int i = 0; i < this.results.Columns.Count; i++)
            {
                DataColumn col = this.results.Columns[i];
                col.Caption = col.ColumnName;
            }

            //Trim data
            foreach (DataRow dr in this.results.Rows)
            {
                foreach (DataColumn dc in results.Columns)
                {
                    if (dc.DataType == typeof(string))
                    {
                        object o = dr[dc];
                        if (!Convert.IsDBNull(o) && o != null)
                        {
                            dr[dc] = o.ToString().Trim();
                        }
                    }
                }
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
                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;

                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemExport.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DataCredito.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DataCredito.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DataCredito.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para Exportar A Excel
        /// </summary>
        public override void TBExport()
        {
            try
            {
                ReportExcelBase form = new ReportExcelBase(this.results, null, false);
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DataCredito.cs", "TBExport"));
            }
        }

        /// <summary>
        /// Boton para buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DataCredito.cs", "TBSearch"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}
