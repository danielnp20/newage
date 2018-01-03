using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AprobacionSolicitudLibranza : DocumentAprobForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        private List<DTO_SolicitudAprobacionCartera> soliAprobacion = new List<DTO_SolicitudAprobacionCartera>();
        private List<DTO_ccSolicitudComponentes> componentes = new List<DTO_ccSolicitudComponentes>();
        private DTO_DigitacionCredito digiCredito = new DTO_DigitacionCredito();
        private DTO_glActividadFlujo actividadDTOTemp = null;
        private DTO_ccCreditoDocu numDocLibranza = new DTO_ccCreditoDocu();

        private Dictionary<string, string> actFlujoForReversion = new Dictionary<string, string>();
        private List<string> actividadesCombo = new List<string>();
        private bool isOK = true;
        private string msgAnulado = string.Empty;
        private string actFlujoTemp = string.Empty;
        private string repName;
        #endregion

        #region Delegados

        private delegate void DelegateSave();
        private DelegateSave saveDelegate;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        private void SaveMethod()
        {
            this.CleanData();
        }

        #endregion

        public AprobacionSolicitudLibranza()
            : base()
        {
            //InitializeComponent();
        }

        public AprobacionSolicitudLibranza(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AprobacionSolicitudLibranza;
            this.frmModule = ModulesPrefix.cc;

            base.SetInitParameters();

            //Carga la Informacion de las maestras
            _bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor,true, true, true, false);
            _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);

            //Carga el rescurso de Anulado
            this.msgAnulado = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado);
            this.msgAnulado = this.msgAnulado.ToUpper();

            //Modifica el tamaño de las Grillas
            this.TbLyPanel.RowStyles[2].Height = 0;
            this.TbLyPanel.RowStyles[4].Height = 150;

            this.gvDocuments.OptionsView.ShowAutoFilterRow = true;

            //Delegado para borrar controles
            this.saveDelegate = new DelegateSave(this.SaveMethod);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //Carga la actividades a revertir
            List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this.actividadFlujoID);
            foreach (DTO_glActividadFlujo act in actPadres)
            {
                this.actividadesCombo.Add(act.Descriptivo.Value);
                this.actFlujoForReversion.Add(act.Descriptivo.Value, act.ID.Value);
            }

            //Agrega el texto de anulado
            this.actividadesCombo.Add(this.msgAnulado);
            this.actFlujoForReversion.Add(this.msgAnulado, string.Empty);

            this.editCmb.Items.AddRange(this.actividadesCombo);

            //Estable la fecha con base a la fecha del periodo
            string strperiodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            DateTime periodo = Convert.ToDateTime(strperiodo);

            if (DateTime.Now.Month != periodo.Month)
            {
                this.dtFecha.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                this.dtFecha.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year,periodo.Month));
                this.dtFecha.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year,periodo.Month));
            }
            else
                this.dtFecha.DateTime = DateTime.Now;
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.currentDoc = null;

                DTO_SolicitudAprobacionCartera soliDocu = new DTO.Negocio.DTO_SolicitudAprobacionCartera();
                int libranza = string.IsNullOrEmpty(this.txt_filtro.Text) ? 0 : Convert.ToInt32(this.txt_filtro.Text);// Estudiar
                this.soliAprobacion = this._bc.AdministrationModel.SolicitudDocu_GetForAprobacion(this.documentID, this.actividadFlujoID, libranza);

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;
                this.gvDocuments.Columns[2].Visible = false;
                if (this.soliAprobacion.Count > 0)
                {
                    //this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this.soliAprobacion;
                    this.masterAsesor.Value = this.soliAprobacion[0].AsesorID.Value;
                    this.masterPagaduria.Value = this.soliAprobacion[0].PagaduriaID.Value;
                    this.txtVlrCuota.Text = this.soliAprobacion[0].VlrCuota.Value.Value.ToString();
                    this.txtVlrCupoDisponible.Text = this.soliAprobacion[0].VlrCupoDisponible.Value.Value.ToString();
                    this.txtPlazo.Text = this.soliAprobacion[0].Plazo.Value.Value.ToString();
                    this.txtVlrSolicitado.Text = this.soliAprobacion[0].VlrSolicitado.Value.Value.ToString();
                    this.txtVlrAdicional.Text = this.soliAprobacion[0].VlrAdicional.Value.Value.ToString();
                    this.txtVlrDescuento.Text = this.soliAprobacion[0].VlrDescuento.Value.Value.ToString();
                    this.txtVlrCompra.Text = this.soliAprobacion[0].VlrCompra.Value.Value.ToString();
                    this.txtVlrGiro.Text = this.soliAprobacion[0].VlrGiro.Value.Value.ToString();
                    this.txtVlrLibranza.Text = this.soliAprobacion[0].VlrLibranza.Value.Value.ToString();
                    this.txtVlrPrestamo.Text = this.soliAprobacion[0].VlrPrestamo.Value.Value.ToString();
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                    this.gvDocuments.BestFitColumns();
                    this.gvDocuments.Columns[this.unboundPrefix + "NombreCliente"].Width = 140;
                    this.gvDocuments.Columns[this.unboundPrefix + "DescripcionPagaduria"].Width = 70;
                    this.gvDocuments.Columns[this.unboundPrefix + "VlrPrestamo"].Width = 70;
                    this.gvDocuments.Columns[this.unboundPrefix + "VlrGiro"].Width = 70;
                    this.gvDocuments.Columns[this.unboundPrefix + "VlrCompra"].Width = 70;
                    this.gvDocuments.Columns[this.unboundPrefix + "VlrLibranza"].Width = 70;
                    this.gvDocuments.Columns[10].Width = 100;
                }
                else
                {
                    this.gcDocuments.DataSource = null;
                    this.gcDetails.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudLibranza.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla de Detalles
        /// </summary>
        protected override void LoadDetails()
        {
            try
            {
                DTO_SolicitudAprobacionCartera soliAprobacion = (DTO_SolicitudAprobacionCartera)this.currentDoc;
                string lineaCredito = soliAprobacion.LineaCreditoID.Value;
                int libranza = Convert.ToInt32(soliAprobacion.Libranza.Value);
                
                this.digiCredito = this._bc.AdministrationModel.DigitacionCredito_GetByLibranza(libranza, this.actividadFlujoID);
                this.componentes = this.digiCredito.Componentes;
                this.gcDetails.DataSource = this.componentes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudLibranza.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas de los Documentos
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();
                this.gvDocuments.Columns.RemoveAt(2);

                //Campo del combo actividades
                GridColumn cmbActividades = new GridColumn();
                cmbActividades.FieldName = this.unboundPrefix + "ActividadFlujoDesc";
                cmbActividades.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActividadesFlujo");
                cmbActividades.UnboundType = UnboundColumnType.String;
                cmbActividades.VisibleIndex = 2;
                cmbActividades.Width = 20;
                cmbActividades.Visible = false;
                cmbActividades.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                cmbActividades.OptionsColumn.AllowEdit = true;
                cmbActividades.ColumnEdit = this.editCmb;
                this.gvDocuments.Columns.Add(cmbActividades);

                //Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this.unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 3;
                Libranza.Width = 20;
                Libranza.Visible = true;
                Libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Libranza);

                //ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this.unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.Integer;
                ClienteID.VisibleIndex = 4;
                ClienteID.Width = 20;
                ClienteID.Visible = true;
                ClienteID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ClienteID);

                //Codigo Empleado
                GridColumn EmpleadoCodigo = new GridColumn();
                EmpleadoCodigo.FieldName = this.unboundPrefix + "EmpleadoCodigo";
                EmpleadoCodigo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpleadoCodigo");
                EmpleadoCodigo.UnboundType = UnboundColumnType.Integer;
                EmpleadoCodigo.VisibleIndex = 4;
                EmpleadoCodigo.Width = 20;
                EmpleadoCodigo.Visible = true;
                EmpleadoCodigo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                EmpleadoCodigo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(EmpleadoCodigo);

                //Linea de Credito
                GridColumn LineaCreditoID = new GridColumn();
                LineaCreditoID.FieldName = this.unboundPrefix + "LineaCreditoID";
                LineaCreditoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaCreditoID");
                LineaCreditoID.UnboundType = UnboundColumnType.Integer;
                LineaCreditoID.VisibleIndex = 4;
                LineaCreditoID.Width = 20;
                LineaCreditoID.Visible = true;
                LineaCreditoID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                LineaCreditoID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(LineaCreditoID);

                //Descripción Pagaduría
                GridColumn DescripcionPagaduria = new GridColumn();
                DescripcionPagaduria.FieldName = this.unboundPrefix + "DescripcionPagaduria";
                DescripcionPagaduria.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                DescripcionPagaduria.UnboundType = UnboundColumnType.Integer;
                DescripcionPagaduria.VisibleIndex = 4;
                DescripcionPagaduria.Width = 25;
                DescripcionPagaduria.Visible = true;
                DescripcionPagaduria.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                DescripcionPagaduria.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(DescripcionPagaduria);

                //NombreCliente
                GridColumn NombreCliente = new GridColumn();
                NombreCliente.FieldName = this.unboundPrefix + "NombreCliente";
                NombreCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NombreCliente");
                NombreCliente.UnboundType = UnboundColumnType.String;
                NombreCliente.VisibleIndex = 5;
                NombreCliente.Width = 40;
                NombreCliente.Visible = true;
                NombreCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                NombreCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(NombreCliente);

                //VlrPrestamo
                GridColumn VlrPrestamo = new GridColumn();
                VlrPrestamo.FieldName = this.unboundPrefix + "VlrPrestamo";
                VlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrestamo");
                VlrPrestamo.UnboundType = UnboundColumnType.Decimal;
                VlrPrestamo.VisibleIndex = 6;
                VlrPrestamo.Width = 25;
                VlrPrestamo.Visible = true;
                VlrPrestamo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrPrestamo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrPrestamo);

                //VlrGiro
                GridColumn VlrGiro = new GridColumn();
                VlrGiro.FieldName = this.unboundPrefix + "VlrGiro";
                VlrGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGiro");
                VlrGiro.UnboundType = UnboundColumnType.Decimal;
                VlrGiro.VisibleIndex = 7;
                VlrGiro.Width = 25;
                VlrGiro.Visible = true;
                VlrGiro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrGiro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrGiro);

                //VlrCompra
                GridColumn VlrCompra = new GridColumn();
                VlrCompra.FieldName = this.unboundPrefix + "VlrCompra";
                VlrCompra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCompra");
                VlrCompra.UnboundType = UnboundColumnType.Decimal;
                VlrCompra.VisibleIndex = 8;
                VlrCompra.Width = 25;
                VlrCompra.Visible = true;
                VlrCompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrCompra.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrCompra);

                //VlrLibranza
                GridColumn VlrLibranza = new GridColumn();
                VlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                VlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                VlrLibranza.UnboundType = UnboundColumnType.Decimal;
                VlrLibranza.VisibleIndex = 9;
                VlrLibranza.Width = 25;
                VlrLibranza.Visible = true;
                VlrLibranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                VlrLibranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(VlrLibranza);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 10;
                desc.Width = 70;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudes.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        protected override void AddDetailCols()
        {
            try
            {
                //Campo de codigo
                GridColumn codigo = new GridColumn();
                codigo.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                codigo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                codigo.UnboundType = UnboundColumnType.String;
                codigo.VisibleIndex = 0;
                codigo.Width = 50;
                codigo.Visible = true;
                codigo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                codigo.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codigo);

                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this.unboundPrefix + "Descripcion";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 1;
                descriptivo.Width = 125;
                descriptivo.Visible = true;
                descriptivo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(descriptivo);

                //cuotaValor
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this.unboundPrefix + "CuotaValor";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorCuota");
                cuotaValor.UnboundType = UnboundColumnType.Decimal;
                cuotaValor.VisibleIndex = 2;
                cuotaValor.Width = 125;
                cuotaValor.Visible = true;
                cuotaValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                cuotaValor.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(cuotaValor);

                //valorTotal
                GridColumn valorTotal = new GridColumn();
                valorTotal.FieldName = this.unboundPrefix + "TotalValor";
                valorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotal");
                valorTotal.UnboundType = UnboundColumnType.Decimal;
                valorTotal.VisibleIndex = 3;
                valorTotal.Width = 125;
                valorTotal.Visible = true;
                valorTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                valorTotal.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(valorTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudes.cs", "AddDetailCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected override bool ValidateDocRow(int fila)
        {
            try
            {
                if (fila >= 0)
                {
                    if (fila != this.gvDocuments.FocusedRowHandle)
                        this.gvDocuments.MoveFirst();
                    string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                    GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
                    bool rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);
                    this.isOK = true;
                    if (rechazado)
                    {
                        #region Valida que la observacion no sea vacia
                        col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                        string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();
                        if (string.IsNullOrEmpty(desc))
                        {
                            string msg = string.Format(rsxEmpty, col.Caption);
                            this.gvDocuments.SetColumnError(col, msg);
                            this.isOK = false;
                        }
                        else
                            this.gvDocuments.SetColumnError(col, string.Empty);
                        #endregion
                        #region Valida que el combo de actividad de flujo no este vacio
                        col = this.gvDocuments.Columns[this.unboundPrefix + "ActividadFlujoDesc"];
                        string desc2 = this.gvDocuments.GetRowCellValue(fila, col).ToString();
                        if (string.IsNullOrEmpty(desc2))
                        {
                            string msg = string.Format(rsxEmpty, col.Caption);
                            this.gvDocuments.SetColumnError(col, msg);
                            isOK = false;
                        }
                        else
                            this.gvDocuments.SetColumnError(col, string.Empty);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this.isOK = false;
                //MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudLibranza.cs", "ValidateDocRow"));
            }

            return this.isOK;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion para limpiar los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            this.masterAsesor.Value = string.Empty;
            this.masterPagaduria.Value = string.Empty;
            this.txtVlrCuota.Text = string.Empty;
            this.txtVlrCupoDisponible.Text = string.Empty;
            this.txtPlazo.Text = string.Empty;
            this.txtVlrSolicitado.Text = string.Empty;
            this.txtVlrAdicional.Text = string.Empty;
            this.txtVlrDescuento.Text = string.Empty;
            this.txtVlrCompra.Text = string.Empty;
            this.txtVlrGiro.Text = string.Empty;
            this.txtVlrLibranza.Text = string.Empty;
            this.txtVlrPrestamo.Text = string.Empty;
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this.soliAprobacion[i].Aprobado.Value = true;
                    this.soliAprobacion[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this.soliAprobacion[i].Aprobado.Value = false;
                    this.soliAprobacion[i].Rechazado.Value = false;
                }
            }
            this.gcDocuments.RefreshDataSource();
        }


        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                    e.Handled = true;
                if (e.KeyChar == 46)
                    e.Handled = true;
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                        this.LoadDocuments();
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudes.cs", "txtFiltro_KeyPress"));
            }
        }

        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFiltro_Leave(object sender, EventArgs e)
        {
            try
            {  
                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                   this.LoadDocuments();
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudes.cs", "txtFiltro_KeyPress"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "ActividadFlujoDesc" && e.Value == null)
                {
                    e.Value = ((DTO_SolicitudAprobacionCartera)e.Row).ActividadFlujoDesc;
                }
                else
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
            if (e.IsSetData)
            {
                if (fieldName == "ActividadFlujoDesc")
                {
                    DTO_SolicitudAprobacionCartera a = (DTO_SolicitudAprobacionCartera)e.Row;
                    a.ActividadFlujoDesc = e.Value.ToString();
                }
                else
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
        }

        /// <summary>
        /// Modifica el Formato de los Valores.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            base.gvDocuments_CustomRowCellEdit(sender, e);

            string fieldName = null;
            fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrPrestamo" || fieldName == "VlrLibranza" || fieldName == "VlrCuota" ||
                fieldName == "VlrGiro" || fieldName == "VlrCompra")
            {
                e.RepositoryItem = this.editSpin;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (!this.gvDocuments.IsFilterRow(e.FocusedRowHandle))
                {
                    if (this.currentRow != -1)
                    {
                        if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                            this.currentRow = e.FocusedRowHandle;

                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                        this.masterAsesor.Value = this.soliAprobacion[this.currentRow].AsesorID.Value;
                        this.masterPagaduria.Value = this.soliAprobacion[this.currentRow].PagaduriaID.Value;
                        this.txtVlrCuota.Text = this.soliAprobacion[this.currentRow].VlrCuota.Value.Value.ToString();
                        this.txtVlrCupoDisponible.Text = this.soliAprobacion[this.currentRow].VlrCupoDisponible.Value.Value.ToString();
                        this.txtPlazo.Text = this.soliAprobacion[this.currentRow].Plazo.Value.Value.ToString();
                        this.txtVlrSolicitado.Text = this.soliAprobacion[this.currentRow].VlrSolicitado.Value.Value.ToString();
                        this.txtVlrAdicional.Text = this.soliAprobacion[this.currentRow].VlrAdicional.Value.Value.ToString();
                        this.txtVlrDescuento.Text = this.soliAprobacion[this.currentRow].VlrDescuento.Value.Value.ToString();
                        this.txtVlrCompra.Text = this.soliAprobacion[this.currentRow].VlrCompra.Value.Value.ToString();
                        this.txtVlrGiro.Text = this.soliAprobacion[this.currentRow].VlrGiro.Value.Value.ToString();
                        this.txtVlrLibranza.Text = this.soliAprobacion[this.currentRow].VlrLibranza.Value.Value.ToString();
                        this.txtVlrPrestamo.Text = this.soliAprobacion[this.currentRow].VlrPrestamo.Value.Value.ToString();

                        this.LoadDetails();
                        this.detailsLoaded = true;
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudLibranza.cs", "glDocuments_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Modifica el Formato de los Valores.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = null;
            fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "CuotaValor" || fieldName == "TotalValor")
            {
                e.RepositoryItem = this.editSpin;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (!this.gvDocuments.IsFilterRow(e.RowHandle))
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region Generales
                if (fieldName == "Aprobado")
                {
                    if ((bool)e.Value)
                    {
                        this.soliAprobacion[e.RowHandle].Aprobado.Value = true;
                        this.soliAprobacion[e.RowHandle].Rechazado.Value = false;
                        this.soliAprobacion[e.RowHandle].Observacion.Value = string.Empty;
                        List<DTO_SolicitudAprobacionCartera> temp = this.soliAprobacion.Where(x => x.Rechazado.Value == true).ToList();
                        bool visibleON = temp.Count > 0 ? true : false;
                        this.gvDocuments.Columns[2].Visible = visibleON;
                    }
                }

                if (fieldName == "Rechazado")
                {
                    if ((bool)e.Value)
                    {
                        this.gvDocuments.Columns[2].Visible = true;
                        this.soliAprobacion[e.RowHandle].Aprobado.Value = false;
                        this.soliAprobacion[e.RowHandle].Rechazado.Value = true;
                    }
                }

                if (fieldName == "ActividadFlujoDesc")
                {
                    string actFlujoDesc = e.Value.ToString();
                    this.soliAprobacion[this.currentRow].ActividadFlujoDesc = actFlujoDesc;
                    this.soliAprobacion[this.currentRow].ActividadFlujoReversion.Value = this.actFlujoForReversion[actFlujoDesc];
                }

                if (fieldName == "Observacion")
                {
                    this.soliAprobacion[this.currentRow].Observacion.Value = e.Value.ToString();
                }
                #endregion
                this.ValidateDocRow(e.RowHandle);
                this.gcDocuments.RefreshDataSource();
                this.gvDocuments.Columns[10].Width = 100;  
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                List<DTO_SolicitudAprobacionCartera> soliTemp = this.soliAprobacion.Where(x => x.Aprobado.Value == true || x.Rechazado.Value == true).ToList();
                if (soliTemp != null && soliTemp.Count != 0 && this.isOK)
                {
                    this.soliAprobacion = soliTemp;
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                    MessageBox.Show(msg);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected override void ApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                foreach (DTO_SolicitudAprobacionCartera sol in this.soliAprobacion)
                    sol.FechaAprobacion.Value = this.dtFecha.DateTime;

                List<DTO_SerializedObject> results = _bc.AdministrationModel.LiquidacionCredito_AprobarRechazar(this.documentID, this.actividadFlujoID, this.soliAprobacion);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                int i = 0;
                int percent = 0;
                #region Variables para el mail

                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this.actividadDTO.seUsuarioID.Value);

                string body = string.Empty;
                string subject = string.Empty;
                string email = user.CorreoElectronico.Value;

                string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                string subjectRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                string bodyRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_RejectedCartera_Body);
                string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                #endregion
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
               
                foreach (object result in results)
                {
                    #region Manejo de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion
                    if (this.soliAprobacion[i].Aprobado.Value.Value)
                    {
                        MailType mType = this.soliAprobacion[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                        bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, result, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)result;
                            resultsNOK.Add(r);
                        }
                        #region Genera el reporte de las libranzas con el plan pagos
                        numDocLibranza = this._bc.AdministrationModel.GetCreditoByLibranza(this.soliAprobacion[i].Libranza.Value.Value);
                        if (numDocLibranza != null)
                            repName = this._bc.AdministrationModel.ReportesCartera_Cc_LiquidacionCredito(this.soliAprobacion[i].Libranza.Value.Value, ExportFormatType.pdf, numDocLibranza.NumeroDoc.Value.Value);
                        #endregion
                    }
                    else if (this.soliAprobacion[i].Rechazado.Value.Value)
                    {
                        MailType mType = this.soliAprobacion[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                        bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, result, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)result;
                            resultsNOK.Add(r);
                        }
                    }                                      
                    i++;
                }
                
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.saveDelegate);
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitudLibranza.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
