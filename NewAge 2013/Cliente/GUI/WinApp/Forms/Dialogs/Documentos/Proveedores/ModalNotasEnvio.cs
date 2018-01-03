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
    public partial class ModalNotasEnvio : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalNotasEnvio;
        //private TipoMoneda _tipoMoneda;
        //private decimal _tasaCambio;
        //private DateTime _periodo;
        private List<DTO_NotasEnvioResumen> _currentData;
        private bool _isFactCompra = false;
        int _notaEnvioRel = 0;      
        protected string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;      
        public List<DTO_NotasEnvioResumen> ReturnList;

        #endregion

        /// <summary>
        /// Constructor de la grilla de NotasEnvio 
        /// </summary>
        /// <param name="factResumen">Lista de notas envio que ya fueron cargados</param>
        public ModalNotasEnvio(int notaEnvioRel)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            //variables
            this._currentData = new List<DTO_NotasEnvioResumen>();
            this._notaEnvioRel = notaEnvioRel;

            //Carga de datos
            this.LoadGridStructure();
            this.LoadGridData();
        }

        /// <summary>
        /// Constructor de la grilla de facturas segun Bodega Origen
        /// </summary>
        /// <param name="BodegaPuerto">Bodega origen de la cual se obtiene los saldos para las facturas pendientes</param>
        public ModalNotasEnvio(string BodegaPuerto)
        {
            //Inicializa el formulario
            InitializeComponent();
            _bc.InitMasterUC(this.master, AppMasters.inBodega, true, true, false, false);
            this.master.EnableControl(false);
            this.master.Visible = true;
            this.master.Value = BodegaPuerto;
            this.lblTitle.Text = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_lblFacturasCompra");
            FormProvider.LoadResources(this, this._documentID);

            //Carga de datos
            this.LoadGridStructureBodegaPuerto();
            this.LoadGridDataBodegaOrigenPuerto(BodegaPuerto);
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
                prefDoc.Width = 100;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(prefDoc);

                GridColumn bod = new GridColumn();
                bod.FieldName = this.unboundPrefix + "BodegaOrigenID";
                bod.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_BodegaOrigenID");
                bod.UnboundType = UnboundColumnType.String;
                bod.VisibleIndex = 3;
                bod.Width = 100;
                bod.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(bod);

                GridColumn client = new GridColumn();
                client.FieldName = this.unboundPrefix + "ClienteID";
                client.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ClienteID");
                client.UnboundType = UnboundColumnType.String;
                client.VisibleIndex = 4;
                client.Width = 100;
                client.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(client);

                GridColumn obs = new GridColumn();
                obs.FieldName = this.unboundPrefix + "Observacion";
                obs.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Observacion");
                obs.UnboundType = UnboundColumnType.String;
                obs.VisibleIndex = 5;
                obs.Width = 250;
                obs.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(obs);

                //GridColumn vtoFecha = new GridColumn();
                //vtoFecha.FieldName = this.unboundPrefix + "VtoFecha";
                //vtoFecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_VtoFecha");
                //vtoFecha.UnboundType = UnboundColumnType.String;
                //vtoFecha.VisibleIndex = 6;
                //vtoFecha.Width = 100;
                //vtoFecha.OptionsColumn.AllowEdit = false;
                //this.gvData.Columns.Add(vtoFecha);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalNotasEnvio.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla para Bodega ORigen Puerto
        /// </summary>
        private void LoadGridStructureBodegaPuerto()
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
                sel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                sel.OptionsColumn.ShowCaption = false;
                this.gvData.Columns.Add(sel);

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 1;
                fecha.Width = 100;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(fecha);

                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Proveedor");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 2;
                terceroID.Width = 100;
                terceroID.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(terceroID);

                GridColumn documentoTercero = new GridColumn();
                documentoTercero.FieldName = this.unboundPrefix + "DocumentoTercero";
                documentoTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DocumentoTercero");
                documentoTercero.UnboundType = UnboundColumnType.String;
                documentoTercero.VisibleIndex = 3;
                documentoTercero.Width = 100;
                documentoTercero.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(documentoTercero);

                GridColumn valorLocal = new GridColumn();
                valorLocal.FieldName = this.unboundPrefix + "ValorLocal";
                valorLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ValorLocal");
                valorLocal.UnboundType = UnboundColumnType.Decimal;
                valorLocal.VisibleIndex = 4;
                valorLocal.Width = 110;
                valorLocal.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(valorLocal);

                GridColumn valorExt = new GridColumn();
                valorExt.FieldName = this.unboundPrefix + "ValorExt";
                valorExt.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ValorExt");
                valorExt.UnboundType = UnboundColumnType.Decimal;
                valorExt.VisibleIndex = 4;
                valorExt.Width = 110;
                valorExt.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(valorExt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalNotasEnvio.cs", "ModalFacturacion-LoadGridStructure"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                List<DTO_NotasEnvioResumen> notas = _bc.AdministrationModel.NotasEnvio_GetResumen(this._documentID);

                if (this._notaEnvioRel != 0)
                {
                    notas.Find(nota => nota.NumeroDoc.Value.Value == this._notaEnvioRel).Seleccionar.Value = true;
                }

                this._currentData = notas;
                this.gcData.DataSource = this._currentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalNotasEnvio.cs", "LoadGridData"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla para bodegaOrigen
        /// </summary>
        private void LoadGridDataBodegaOrigenPuerto(string BodegaPuerto)
        {
            try
            {
                DTO_inCostosExistencias costos = new DTO_inCostosExistencias();
                List<DTO_inMovimientoDocu> docuList = new List<DTO_inMovimientoDocu>();
                DTO_inMovimientoDocu docu = new DTO_inMovimientoDocu();
                this._currentData = new List<DTO_NotasEnvioResumen>();

                #region Obtiene registros filtrados por la bodega Puerto Embarque
                docu.BodegaOrigenID.Value = BodegaPuerto;
                docuList = this._bc.AdministrationModel.inMovimientoDocu_GetByParameter(this._documentID, docu); 
                #endregion

                foreach (var item in docuList)
                {
                    DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
                    decimal valorLoc = 0;
                    decimal valorExt = 0;
                    saldosCostos.BodegaID.Value = BodegaPuerto;
                    saldosCostos.IdentificadorTr.Value = item.NumeroDoc.Value;
                    List<DTO_inControlSaldosCostos> listSaldosCostos = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, saldosCostos);

                    foreach (var saldoCosto in listSaldosCostos)
                    {
                        saldosCostos.CantidadDisp.Value = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(this._documentID, saldoCosto, ref costos);
                        valorLoc += (costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value);//Sin FOB
                        valorExt += (costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value);//Sin FOB
                    }

                    if (valorLoc != 0)
                    {
                        #region Asigna valores a las Notas de Envio que tengan Valor
                        DTO_glDocumentoControl docControl = this._bc.AdministrationModel.glDocumentoControl_GetByID(item.NumeroDoc.Value.Value);
                        DTO_NotasEnvioResumen nota = new DTO_NotasEnvioResumen();
                        nota.Seleccionar.Value = false;
                        nota.NumeroDoc.Value = docControl.NumeroDoc.Value;
                        nota.DocumentoNro.Value = docControl.DocumentoNro.Value;
                        nota.PrefijoID.Value = docControl.PrefijoID.Value;
                        nota.Fecha.Value = docControl.Fecha.Value.Value;
                        nota.TerceroID.Value = docControl.TerceroID.Value;
                        nota.DocumentoTercero.Value = docControl.DocumentoTercero.Value;
                        nota.ValorLocal.Value = valorLoc;
                        nota.ValorExt.Value = valorExt;
                        this._currentData.Add(nota); 
                        #endregion
                    }
                }
                this._isFactCompra = true;
                this.gcData.DataSource = this._currentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalNotasEnvio.cs", "LoadGridDataBodegaOrigenPuerto"));
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

            if (fieldName == "ValorLocal" || fieldName == "ValorExt")
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
            DTO_NotasEnvioResumen dto = this._currentData.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Seleccionar")
                    e.Value = dto.Seleccionar.Value;
                if (fieldName == "Fecha")            
                    e.Value = dto.Fecha.Value.Value.ToString(FormatString.DB_Date_YYYY_MM_DD);
                if (fieldName == "PrefDoc")
                    e.Value = dto.PrefijoID.Value + " - " + dto.DocumentoNro.Value;
                if (fieldName == "BodegaOrigenID")
                    e.Value = dto.BodegaOrigenID.Value;
                if (fieldName == "ClienteID")
                    e.Value = dto.ClienteID.Value;
                if (fieldName == "Observacion")
                    e.Value = dto.Observacion.Value;
                if (fieldName == "TerceroID")
                    e.Value = dto.TerceroID.Value;
                if (fieldName == "DocumentoTercero")
                    e.Value = dto.DocumentoTercero.Value;
                if (fieldName == "ValorLocal")
                    e.Value = dto.ValorLocal.Value;
                if (fieldName == "ValorExt")
                    e.Value = dto.ValorExt.Value;
                //if (fieldName == "VtoFecha")
                //    e.Value = dto.VtoFecha==null?string.Empty: dto.VtoFecha.Value.Value.ToString(FormatString.DB_Date_YYYY_MM_DD);
            }
            if (e.IsSetData)
            {
                if (fieldName == "Seleccionar")
                    dto.Seleccionar.Value = Convert.ToBoolean(e.Value);
            }                 
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Seleccionar")
            {
                if (Convert.ToBoolean(e.Value))
                {
                    if (!this._isFactCompra)
                    {
                        for (int i = 0; i < this.gvData.RowCount; i++)
                        {
                            if (i != e.RowHandle)
                                this.gvData.SetRowCellValue(i, e.Column, false);
                        }
                        this._notaEnvioRel = this._currentData[e.RowHandle].NumeroDoc.Value.Value;
                    }
                }
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
                if (this._currentData.Exists(x => x.Seleccionar.Value.Value == true))
                {
                    this.ReturnVals = true;
                    this.ReturnList = new List<DTO_NotasEnvioResumen>();
                    this._currentData.ForEach(a =>
                    {
                        if (a.Seleccionar.Value.Value)
                        {
                            this.ReturnList.Add(a);
                        }
                    });
                }
                if (this.ReturnVals)
                    this.Close();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalNotasEnvio.cs", "btnReturn_Click"));
            }
        }

        #endregion

    }
}
