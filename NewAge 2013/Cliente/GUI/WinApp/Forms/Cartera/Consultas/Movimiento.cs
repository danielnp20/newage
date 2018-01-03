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
    public partial class Movimiento : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        // Parametros de Query
        private string clienteMov;
        private DateTime fechaInt;
        private DateTime fechaFin;
        private string NroCredito;
        private string NumeroDoc;
        private int tipoMovimiento;
        private int tipoAnulado;

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private List<DTO_QueryCarteraMvto> list = new List<DTO_QueryCarteraMvto>();

        //Variables formulario
        private bool validate = true;
        private bool newSearch = false;
        private DTO_QueryCarteraMvto selectedCredito = new DTO_QueryCarteraMvto();
        string fecha;

        #endregion

        #region Constructor Movimiento
        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Movimiento()
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.cc;

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
                FormProvider.LoadResources(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "Movimiento.cs-Movimiento"));
            }
        } 
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.Movimiento;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterTipoMovimiento, AppMasters.glDocumento, true, true, true, false);
          
            //Deshabilita los botones +- de la grilla
            this.gcGenerales.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            // Convertir fecha Incial en el 1 dia de cada mes
            DateTime periodoInicial = DateTime.Now;
            string periodo = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            if (!string.IsNullOrEmpty(periodo))
                periodoInicial = Convert.ToDateTime(periodo);

            //Carga la fecha para los filtros de fechas
            DateTime FechaActual = DateTime.Today;
            fecha = Convert.ToString("01/" + FechaActual.Month + "/" + FechaActual.Year);
            this.dtFechaDe.EditValue = Convert.ToDateTime(this.fecha);
            this.dtFechaHasta.EditValue = FechaActual;          
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columna Grilla Principal

                //TipoMovimiento
                GridColumn TipoMovimiento = new GridColumn();
                TipoMovimiento.FieldName = this._unboundPrefix + "DocumentoID";
                TipoMovimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                TipoMovimiento.UnboundType = UnboundColumnType.Integer;
                TipoMovimiento.VisibleIndex = 0;
                TipoMovimiento.Width = 130;
                TipoMovimiento.Visible = true;
                TipoMovimiento.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(TipoMovimiento);

                //FechaMovimiento
                GridColumn FechaMovimiento = new GridColumn();
                FechaMovimiento.FieldName = this._unboundPrefix + "Fecha_Movimiento";
                FechaMovimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaMovimiento.UnboundType = UnboundColumnType.DateTime;
                FechaMovimiento.VisibleIndex = 1;
                FechaMovimiento.Width = 130;
                FechaMovimiento.Visible = true;
                FechaMovimiento.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(FechaMovimiento);

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
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefNro");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 6;
                PrefDoc.Width = 100;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(PrefDoc);

                //Comprobante Contable = ComponenteID + ComponenteNumero
                GridColumn ComprobanteCont = new GridColumn();
                ComprobanteCont.FieldName = this._unboundPrefix + "Comprobante";
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

                //Documento
                GridColumn file = new GridColumn();
                file.FieldName = this._unboundPrefix + "ViewDoc";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 50;
                file.ColumnEdit = this.linkEditViewFile;
                file.VisibleIndex = 10;
                file.Visible = true;
                file.OptionsColumn.AllowEdit = true;
                this.gvGenerales.Columns.Add(file);

                #endregion

                #region Columnas Sub Grilla

                // Codigo Campo
                GridColumn CodigoCampo = new GridColumn();
                CodigoCampo.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                CodigoCampo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodigoCampo");
                CodigoCampo.UnboundType = UnboundColumnType.Integer;
                CodigoCampo.VisibleIndex = 0;
                CodigoCampo.Width = 100;
                CodigoCampo.Visible = true;
                CodigoCampo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CodigoCampo);

                // Descripción
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "ComponenteDesc";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 1;
                Descripcion.Width = 100;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Descripcion);

                // Valor cartera Detallado
                GridColumn ValorCarteraDetallado = new GridColumn();
                ValorCarteraDetallado.FieldName = this._unboundPrefix + "AbonoDocumento";
                ValorCarteraDetallado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCarteraDetallado");
                ValorCarteraDetallado.UnboundType = UnboundColumnType.Integer;
                ValorCarteraDetallado.VisibleIndex = 2;
                ValorCarteraDetallado.Width = 100;
                ValorCarteraDetallado.Visible = true;
                ValorCarteraDetallado.ColumnEdit = this.editValue;
                ValorCarteraDetallado.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorCarteraDetallado);

                // Valor Pago Detallado
                GridColumn ValorPagadoDetallado = new GridColumn();
                ValorPagadoDetallado.FieldName = this._unboundPrefix + "AbonoCuota";
                ValorPagadoDetallado.Caption =_bc.GetResource(LanguageTypes.Forms, this._documentID+"_ValorPagadoDetallado");
                ValorPagadoDetallado.UnboundType = UnboundColumnType.Integer;
                ValorPagadoDetallado.VisibleIndex = 3;
                ValorPagadoDetallado.Width = 100;
                ValorPagadoDetallado.Visible = true;
                ValorPagadoDetallado.ColumnEdit = this.editValue;
                ValorPagadoDetallado.OptionsColumn.AllowEdit= false;
                this.gvDetalle.Columns.Add(ValorPagadoDetallado);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimientos.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;

            //Header
            this.masterCliente.Value = string.Empty;
            this.masterTipoMovimiento.Value = string.Empty;
            this.txtLibranza.Text = string.Empty;
            //this.dtFechaDe.EditValue = DateTime.Now;
            
            //Footer
            this.gcGenerales.DataSource = this.list;
        }

        /// <summary>
        /// Funcion que realiza la opreacion de busqueda
        /// </summary>
        private void GetSearch()
        {

                this.clienteMov = !string.IsNullOrWhiteSpace(masterCliente.Value)?(this.masterCliente.Value):null;
                this.NroCredito = this.txtLibranza.Text;
                this.fechaInt = this.dtFechaDe.DateTime;
                this.fechaFin = this.dtFechaHasta.DateTime;
                this.tipoMovimiento = !string.IsNullOrWhiteSpace(masterTipoMovimiento.Value)?(Convert.ToInt32(this.masterTipoMovimiento.Value)):0;
                this.tipoAnulado = 0;
                {
                    this.list = this._bc.AdministrationModel.Cartera_GetMvto(this.clienteMov, this.NroCredito, this.fechaInt, this.fechaFin, this.tipoMovimiento, this.tipoAnulado); // Cambiar
                    this.gcGenerales.DataSource = this.list;

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
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemExport.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta cuando se cambia de pestaña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tc_QueryCreditos_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int index = tc_QueryCreditos.SelectedTabPageIndex;
        }

        /// <summary>
        /// Evento que verifica que la libranza sea numerica
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento/param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsSeparator(e.KeyChar))
                e.Handled = false;            
            else
                e.Handled = true;

            // Valida la fecha para filtrar por Libranza 
            if (string.IsNullOrWhiteSpace(this.txtLibranza.Text))
                this.dtFechaDe.EditValue = Convert.ToDateTime(this.fecha);
            else
                this.dtFechaDe.EditValue = "01/01/2000";
        }

        private void masterCliente_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.masterCliente.Value))
                this.dtFechaDe.EditValue = "01/01/2000";
            else
                this.dtFechaDe.EditValue = Convert.ToDateTime(this.fecha);
        }
        
        private void masterTipoMovimiento_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.masterTipoMovimiento.Value))
                this.dtFechaDe.EditValue = Convert.ToDateTime(this.fecha);
        }

        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        private void gvGenerales_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
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
        private void gvGenerales_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.validate)
                {
                    int row = e.FocusedRowHandle;
                    this.selectedCredito = (DTO_QueryCarteraMvto)this.gvGenerales.GetRow(row);
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
        private void gvGenerales_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
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
        private void gvGenerales_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "ViewDoc")
                e.DisplayText =  this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.ViewDocument);
        }

        /// <summary>
        /// Evento que llama la funcionalidad de buscar documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkEditViewFile_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.selectedCredito.NumeroDoc.Value.Value);
                if(ctrl != null)
                    comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null);

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-linkEditViewFile_Click"));
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
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                if (this.newSearch)
                {
                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string libTemp = this.txtLibranza.Text;
                        string cliTemp = this.masterCliente.Value;

                        this.GetSearch();
                        this.newSearch = false;
                    }
                }
                else
                {
                    this.newSearch = true;
                    this.GetSearch();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para Exportar A Excel
        /// </summary>
        public override void TBExport()
        {
            try
            {
                    DataTableOperations tableOp = new DataTableOperations();

                    List<DTO_ExportMovimiento> tmp = new List<DTO_ExportMovimiento>();
                    foreach (DTO_QueryCarteraMvto mov in this.list)
	                {
                        DTO_ExportMovimiento exM      =     new DTO_ExportMovimiento();
                           exM.DocumentoID.Value      =     mov.DocumentoID.Value;
                           exM.Fecha_Movimiento.Value =     mov.Fecha_Movimiento.Value;
                           exM.FechaAplicacion.Value  =     mov.FechaAplicacion.Value.ToString();  
                           exM.NroCredito.Value       =     mov.NroCredito.Value;
                           exM.ClienteID.Value        =     mov.ClienteID.Value;
                           exM.Nom_Cliente.Value      =     mov.Nom_Cliente.Value;
                           exM.DOCUMENTO.Value        =     mov.PrefDoc.Value;
                           exM.COMPROBANTE.Value      =     mov.Comprobante.Value;
                           exM.TotalDocumento.Value   =     mov.TotalDocumento.Value;
                           exM.TotalCuota.Value       =     mov.TotalCuota.Value;
                           tmp.Add(exM);
	                }
                    System.Data.DataTable TablaTotal = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportMovimiento), tmp);
                    System.Data.DataTable TablaExport = new System.Data.DataTable();

                    ReportExcelBase form = new ReportExcelBase(TablaTotal);
                    form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Movimiento.cs", "TBExport"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}
