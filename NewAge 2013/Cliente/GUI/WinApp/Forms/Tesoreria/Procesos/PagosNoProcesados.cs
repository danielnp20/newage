using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class PagosNoProcesados : FormWithToolbar
    {
        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Process;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int _indexFila = 0;
        private List<DTO_PagosElectronicos> _pagosElectronicosList = new List<DTO_PagosElectronicos>();

        #endregion

        #region Propiedades

        /// <summary>
        /// Numero de una fila segun el indice
        /// </summary>
        protected int NumFila
        {
            get
            {
                return this._pagosElectronicosList.FindIndex(det => det.Index == this._indexFila);
            }
        }

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public PagosNoProcesados()
        {
            try
            {
                this.InitializeComponent();

                this._documentID = AppProcess.PagosNoProcesados;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.ts;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Inicia los controles de usuario maestras
                _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
                
                //Carga las columnas de las grillas
                this.AddGridCols();
               
                this.saveDelegate = new Save(this.SaveMethod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "PagosNoProcesados"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Carga la información del formulario, con respecto a la cuenta de banco seleccionada
        /// </summary>
        private void LoadData()
        {
            if (this.masterTercero.Value != string.Empty && this.dtFecha.Text != string.Empty)
                this._pagosElectronicosList = this._bc.AdministrationModel.PagosElectronicos_GetPagosElectronicosTransmitidos(this.masterTercero.Value, dtFecha.DateTime);
            else
                this._pagosElectronicosList = new List<DTO_PagosElectronicos>();

            if (this._pagosElectronicosList.Count > 0)
            {
                this.chkSelectAll.Enabled = true;
                if (this._pagosElectronicosList.FindAll(p => !p.DevolverTransmicionInd.Value.HasValue || !(bool)p.DevolverTransmicionInd.Value).Count == 0)
                    this.chkSelectAll.CheckState = CheckState.Checked;
                else
                    this.chkSelectAll.CheckState = CheckState.Unchecked;
            }
            else
            {
                this.chkSelectAll.Enabled = false;
                this.chkSelectAll.CheckState = CheckState.Unchecked;
            }
            
            this.gcPagos.DataSource = this._pagosElectronicosList;
            this.gcPagos.RefreshDataSource();

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //IndicadorPago
                GridColumn indicadorPago = new GridColumn();
                indicadorPago.FieldName = this._unboundPrefix + "DevolverTransmicionInd";
                indicadorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DevolverTransmisionInd");
                indicadorPago.UnboundType = UnboundColumnType.Boolean;
                indicadorPago.VisibleIndex = 0;
                indicadorPago.Width = 60;
                indicadorPago.Visible = true;
                indicadorPago.OptionsColumn.AllowEdit = true;
                this.gvPagos.Columns.Add(indicadorPago);

                //Numero de documento
                GridColumn nroDoc = new GridColumn();
                nroDoc.FieldName = this._unboundPrefix + "NumeroDoc";
                nroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NumeroDoc");
                nroDoc.UnboundType = UnboundColumnType.Integer;
                nroDoc.Visible = false;
                nroDoc.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(nroDoc);

                //BancoCuenta
                GridColumn bancoCuenta = new GridColumn();
                bancoCuenta.FieldName = this._unboundPrefix + "BancoCuentaID";
                bancoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BancoCuentaID");
                bancoCuenta.UnboundType = UnboundColumnType.String;
                bancoCuenta.Width = 120;
                bancoCuenta.Visible = false;
                bancoCuenta.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(bancoCuenta);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this._unboundPrefix + "TerceroID";
                tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 1;
                tercero.Width = 120;
                tercero.Visible = true;
                tercero.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(tercero);

                //NombreTercero
                GridColumn nombreTercero = new GridColumn();
                nombreTercero.FieldName = this._unboundPrefix + "Descriptivo";
                nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NombreTercero");
                nombreTercero.UnboundType = UnboundColumnType.String;
                nombreTercero.VisibleIndex = 2;
                nombreTercero.Width = 180;
                nombreTercero.Visible = true;
                nombreTercero.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(nombreTercero);

                //Banco
                GridColumn banco = new GridColumn();
                banco.FieldName = this._unboundPrefix + "Banco";
                banco.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Banco");
                banco.UnboundType = UnboundColumnType.String;
                banco.VisibleIndex = 3;
                banco.Width = 100;
                banco.Visible = true;
                banco.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(banco);

                //Fecha del pago
                GridColumn fechaPago = new GridColumn();
                fechaPago.FieldName = this._unboundPrefix + "Fecha";
                fechaPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                fechaPago.UnboundType = UnboundColumnType.DateTime;
                fechaPago.VisibleIndex = 4;
                fechaPago.Width = 100;
                fechaPago.Visible = true;
                fechaPago.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(fechaPago);

                //ComprobanteID
                GridColumn comprobanteID = new GridColumn();
                comprobanteID.FieldName = this._unboundPrefix + "ComprobanteID";
                comprobanteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComprobanteID");
                comprobanteID.UnboundType = UnboundColumnType.String;
                comprobanteID.VisibleIndex = 5;
                comprobanteID.Width = 100;
                comprobanteID.Visible = true;
                comprobanteID.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(comprobanteID);

                //ComprobanteIDNro
                GridColumn comprobanteIDNro = new GridColumn();
                comprobanteIDNro.FieldName = this._unboundPrefix + "ComprobanteIDNro";
                comprobanteIDNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComprobanteIDNro");
                comprobanteIDNro.UnboundType = UnboundColumnType.Integer;
                comprobanteIDNro.VisibleIndex = 6;
                comprobanteIDNro.Width = 100;
                comprobanteIDNro.Visible = true;
                comprobanteIDNro.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(comprobanteIDNro);
                
                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this._unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 7;
                valor.Width = 140;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(valor);

                //TipoCuenta
                GridColumn tipoCuenta = new GridColumn();
                tipoCuenta.FieldName = this._unboundPrefix + "TipoCuenta";
                tipoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoCuenta");
                tipoCuenta.UnboundType = UnboundColumnType.Integer;
                tipoCuenta.VisibleIndex = 8;
                tipoCuenta.Width = 100;
                tipoCuenta.Visible = true;
                tipoCuenta.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(tipoCuenta);

                //CuentaNro
                GridColumn cuentaNro = new GridColumn();
                cuentaNro.FieldName = this._unboundPrefix + "CuentaNro";
                cuentaNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuentaNro");
                cuentaNro.UnboundType = UnboundColumnType.String;
                cuentaNro.VisibleIndex = 9;
                cuentaNro.Width = 100;
                cuentaNro.Visible = true;
                cuentaNro.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(cuentaNro);
                
                //Fecha del pago
                GridColumn fechatransmicion = new GridColumn();
                fechatransmicion.FieldName = this._unboundPrefix + "FechaTransmicion";
                fechatransmicion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaTransmision");
                fechatransmicion.UnboundType = UnboundColumnType.DateTime;
                fechatransmicion.VisibleIndex = 10;
                fechatransmicion.Width = 100;
                fechatransmicion.Visible = true;
                fechatransmicion.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(fechatransmicion);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this._unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvPagos.Columns.Add(colIndex);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "PagosNoProcesados.cs-AddGridCols"));
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

                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;

                FormProvider.Master.itemSave.Enabled = true;
                FormProvider.Master.itemUpdate.Enabled = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "PagosNoProcesados.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "PagosNoProcesados.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "PagosNoProcesados.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "PagosNoProcesados.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header
        
        /// <summary>
        /// Botón que busca los pagos para los criterios seleccionados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.masterTercero.Value != string.Empty && this.dtFecha.Text != string.Empty)
                {
                    this.LoadData();
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoSearchCriteria));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "DevolverTransmicionInd")
            {
                e.RepositoryItem = this.editCheck;
            }
            if (fieldName == "Valor")
            {
                e.RepositoryItem = this.editSpin;
            }
            if (fieldName == "Fecha" || fieldName == "FechaTransmicion")
            {
                e.RepositoryItem = this.editDate;                
            }
            
        }

        /// <summary>
        /// Actualiza valores editables cuando se cambia estado de pago
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "DevolverTransmicionInd")
            {
                bool value = Convert.ToBoolean(e.Value);
                int index = this.NumFila;
                this._pagosElectronicosList[index].DevolverTransmicionInd.Value = value;
                if (value)
                {
                    if (this._pagosElectronicosList.FindAll(p => !p.DevolverTransmicionInd.Value.HasValue || !(bool)p.DevolverTransmicionInd.Value).Count == 0)
                        this.chkSelectAll.CheckState = CheckState.Checked;
                }
                else
                {
                    this.chkSelectAll.CheckState = CheckState.Unchecked;
                }

                this.gcPagos.RefreshDataSource();

            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
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
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPagos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                GridColumn col = this.gvPagos.Columns[this._unboundPrefix + "Index"];
                if (0 <= e.FocusedRowHandle && e.FocusedRowHandle < gvPagos.RowCount)
                {
                    this._indexFila = Convert.ToInt16(this.gvPagos.GetRowCellValue(e.FocusedRowHandle, col));
                    var pagoFactura = this._pagosElectronicosList.Find(p => p.Index == this._indexFila);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosNoProcesados.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Footer
        
        /// <summary>
        /// Evento que se llama cuando se selecciona o desselecciona todo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkSelectAll_MouseClick(object sender, MouseEventArgs e)
        {
            bool value = this.chkSelectAll.Checked;
            this._pagosElectronicosList.ForEach(p => p.DevolverTransmicionInd.Value = value);
            this.gcPagos.RefreshDataSource();
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvPagos.PostEditor();

                if (this._pagosElectronicosList.FindAll(p => p.DevolverTransmicionInd.Value.HasValue && (bool)p.DevolverTransmicionInd.Value).Count > 0)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_NoHayPagosSeleccionados));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public void SaveThread()
        {
            try
            {
                List<DTO_PagosElectronicos> pagosElectronicosARevertir = this._pagosElectronicosList.FindAll(p => p.DevolverTransmicionInd.Value.HasValue && p.DevolverTransmicionInd.Value.Value);

                
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                result = _bc.AdministrationModel.PagosElectronicos_RevertirTransmicion(this._documentID, pagosElectronicosARevertir);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if(result.Result == ResultValue.OK)
                    this.Invoke(this.saveDelegate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        
        }

        #endregion

    }
}
