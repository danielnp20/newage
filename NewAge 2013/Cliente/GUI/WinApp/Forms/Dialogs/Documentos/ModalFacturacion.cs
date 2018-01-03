using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalFacturacion : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalFacturacionForm;
        private TipoMoneda _tipoMoneda;
        private decimal _tasaCambio;
        private DateTime _periodo;
        private List<DTO_faFacturacionResumen> _currentData;
        private List<DTO_faFacturaDocu> _currentDataCliente;
        private string _tercero;
        protected string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;
        private bool _factCliente = false;
        public List<DTO_faFacturacionResumen> ReturnList = new List<DTO_faFacturacionResumen>();
        public List<DTO_faFacturaDocu> ReturnListCliente = new List<DTO_faFacturaDocu>();

        #endregion

        /// <summary>
        /// Constructor de la grilla de facturas 
        /// </summary>
        /// <param name="factResumen">Lista de facturas que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los facturas</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ModalFacturacion(List<DTO_faFacturacionResumen> factResumen, DateTime periodo, TipoMoneda tm, decimal tasaCambio, string terceroID)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);
            this.master.EnableControl(false);

            //variables
            this._tipoMoneda = tm;
            this._tasaCambio = tasaCambio;
            this._periodo = periodo;
            this._currentData = factResumen;
            this._tercero = terceroID;

            //Carga de datos
            this.LoadGridStructure();
            this.LoadGridData();
        }

        /// <summary>
        /// Constructor de la grilla de facturas segun cliente
        /// </summary>
        /// <param name="ClienteID">Cliente asociado a las facturas</param>
        public ModalFacturacion(string ClienteID)
        {
            //Inicializa el formulario
            InitializeComponent();
            _bc.InitMasterUC(this.master, AppMasters.faCliente, true, true, false, false);
            this.master.EnableControl(false);
            this.master.Visible = true;
            this.master.Value = ClienteID;
            FormProvider.LoadResources(this, this._documentID);
            this._factCliente = true;

            //Carga de datos
            this.LoadGridStructureCliente();
            this.LoadGridDataCliente(ClienteID);
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void LoadGridStructure()
        {
            try
            {
                //Campo de marca
                GridColumn sel = new GridColumn();
                sel.FieldName = this.unboundPrefix + "Seleccionar";
                sel.UnboundType = UnboundColumnType.Boolean;
                sel.VisibleIndex = 0;
                sel.Width = 20;
                sel.Visible = true;
                //sel.ReadOnly = false;
                sel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                sel.OptionsColumn.ShowCaption = false;
                this.gvData.Columns.Add(sel);

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
                fecha.UnboundType = UnboundColumnType.String;
                fecha.VisibleIndex = 1;
                fecha.Width = 100;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(fecha);

                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Documento");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 2;
                prefDoc.Width = 170;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(prefDoc);

                GridColumn mda = new GridColumn();
                mda.FieldName = this.unboundPrefix + "MonedaID";
                mda.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_MonedaID");
                mda.UnboundType = UnboundColumnType.String;
                mda.VisibleIndex = 3;
                mda.Width = 100;
                mda.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(mda);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 4;
                valor.Width = 110;
                valor.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(valor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFacturacion.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla para cliente
        /// </summary>
        private void LoadGridStructureCliente()
        {
            try
            {
                //Campo de marca
                GridColumn sel = new GridColumn();
                sel.FieldName = this.unboundPrefix + "Seleccionar";
                sel.UnboundType = UnboundColumnType.Boolean;
                sel.VisibleIndex = 0;
                sel.Width = 20;
                sel.Visible = true;
                //sel.ReadOnly = false;
                sel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                sel.OptionsColumn.ShowCaption = false;
                this.gvData.Columns.Add(sel);

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
                fecha.UnboundType = UnboundColumnType.String;
                fecha.VisibleIndex = 2;
                fecha.Width = 100;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(fecha);

                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Documento");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 3;
                prefDoc.Width = 100;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(prefDoc);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 5;
                valor.Width = 110;
                valor.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(valor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFacturacion.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                List<DTO_faFacturacionResumen> facturas = _bc.AdministrationModel.Facturacion_GetResumen(this._periodo, this._tipoMoneda, this._tercero);
                List<DTO_faFacturacionResumen> newData = new List<DTO_faFacturacionResumen>();

                //Carga la informacion de los facturas de acuerdo con el tipo de moneda
                facturas.ForEach(newFact =>
                {
                    #region Revisa los facturas que ya estan asignados
                    this._currentData.ForEach(fact =>
                    {
                        bool found = false;
                        if
                        (
                            !found &&
                            fact.CuentaID.Value == newFact.CuentaID.Value &&
                            fact.TerceroID.Value == newFact.TerceroID.Value &&
                            fact.ProyectoID.Value == newFact.ProyectoID.Value &&
                            fact.CentroCostoID.Value == newFact.CentroCostoID.Value &&
                            fact.LineaPresupuestoID.Value == newFact.LineaPresupuestoID.Value &&
                            fact.ConceptoSaldoID.Value == newFact.ConceptoSaldoID.Value &&
                            fact.ConceptoCargoID.Value == newFact.ConceptoCargoID.Value &&
                            fact.IdentificadorTR.Value == newFact.IdentificadorTR.Value
                        )
                        {
                            found = true;
                            newFact.Seleccionar.Value = true;
                        }
                    });
                    #endregion
                    newData.Add(newFact);
                });

                this._currentData = facturas;
                this.gcData.DataSource = this._currentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFacturacion.cs", "LoadGridData"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla para cliente
        /// </summary>
        private void LoadGridDataCliente(string clienteID)
        {
            try
            {
                List<DTO_faFacturaDocu> facturasCliente = _bc.AdministrationModel.FacturaVenta_GetByCliente(AppDocuments.FacturaVenta,clienteID);
                List<DTO_faFacturaDocu> newData = new List<DTO_faFacturaDocu>();

                this._currentDataCliente = facturasCliente;
                this.gcData.DataSource = this._currentDataCliente;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFacturacion.cs", "LoadGridData"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Valor")
            {
                e.RepositoryItem = this.editSpin;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (!this._factCliente)
            {
                #region Recibo Caja
                DTO_faFacturacionResumen dto = this._currentData.ElementAt(e.ListSourceRowIndex);
                if (e.IsGetData)
                {
                    if (fieldName == "Seleccionar")
                        e.Value = dto.Seleccionar.Value;
                    if (fieldName == "Fecha")
                        e.Value = dto.Fecha.Value.Value.ToString(FormatString.DB_Date_YYYY_MM_DD);
                    if (fieldName == "PrefDoc")
                        e.Value = dto.PrefijoID.Value + " - " + dto.DocumentoNro.Value;
                    if (fieldName == "MonedaID")
                        e.Value = dto.MonedaID.Value;
                    if (fieldName == "Valor")
                        e.Value = this._tipoMoneda == TipoMoneda.Local ? dto.ML.Value : dto.ME.Value;
                }
                if (e.IsSetData)
                {
                    if (fieldName == "Seleccionar")
                        dto.Seleccionar.Value = Convert.ToBoolean(e.Value);
                } 
                #endregion
            }
            else
            {
                #region Nota Envio
                DTO_faFacturaDocu dto = this._currentDataCliente.ElementAt(e.ListSourceRowIndex);
                DTO_glDocumentoControl docControl = null;
                if (e.IsGetData)
                {
                    if (fieldName == "Seleccionar")
                        e.Value = dto.Seleccionar.Value;
                    if (fieldName == "ClienteID")
                        e.Value = dto.ClienteID.Value;
                    if (fieldName == "Fecha")
                    {
                        docControl = _bc.AdministrationModel.glDocumentoControl_GetByID(dto.NumeroDoc.Value.Value);
                        e.Value = docControl.Fecha.Value.Value.ToString(FormatString.DB_Date_YYYY_MM_DD);
                    }
                    if (fieldName == "PrefDoc")
                    {
                        docControl = _bc.AdministrationModel.glDocumentoControl_GetByID(dto.NumeroDoc.Value.Value);
                        e.Value = docControl.PrefijoID.Value + " - " + docControl.DocumentoNro.Value;
                    }
                    if (fieldName == "MonedaPago")
                        e.Value = dto.MonedaPago.Value;
                    if (fieldName == "Valor")
                        e.Value = dto.Valor.Value.Value;
                }
                if (e.IsSetData)
                {
                    if (fieldName == "Seleccionar")
                        dto.Seleccionar.Value = Convert.ToBoolean(e.Value);
                } 
                #endregion
            }
        }

        /// <summary>
        /// Devuelve el registro seleccionado
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReturnVals = true;
                if (!this._factCliente)
                {
                    this._currentData.ForEach(a =>
                    {
                        if (a.Seleccionar.Value.Value)
                        {
                            if (a.OrigenMonetario.Value != (byte)this._tipoMoneda)
                            {
                                if (this._tipoMoneda == TipoMoneda.Local)
                                    a.ML.Value = Math.Round(a.ME.Value.Value * this._tasaCambio, 2);
                                else
                                    a.ME.Value = Math.Round(a.ML.Value.Value / this._tasaCambio, 2);
                            }

                            this.ReturnList.Add(a);
                        }
                    });
                }
                else
                {
                    this.ReturnListCliente.Clear();
                    this._currentDataCliente.ForEach(a =>
                    {
                        if (a.Seleccionar.Value.Value)
                            this.ReturnListCliente.Add(a);
                    });
                    if (this.ReturnListCliente.Count > 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_FacturaOnlyOne));
                        this.ReturnVals = false;
                    }                          
                }
                if(this.ReturnVals)
                    this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
