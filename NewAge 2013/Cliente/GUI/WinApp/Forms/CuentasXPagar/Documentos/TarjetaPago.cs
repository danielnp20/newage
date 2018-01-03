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
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using System.Text.RegularExpressions;
using System.Threading;
using NewAge.DTO.Reportes;
using NewAge.ReportesComunes;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;
using DevExpress.Data;
using SentenceTransformer;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class TarjetaPago : DocumentForm
    {
        public TarjetaPago()
        {
           //InitializeComponent();
        }

        #region Variables 

        BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _dtoCtrl;
        private DTO_cpTarjetaDocu _tarjetaDocu;
        private string _tercero;
        private List<DTO_cpTarjetaPagos> _listTarjetaPagos;
        private int _tipoMoneda;
        private string _message;
        private decimal _tc;
        private bool _nuevoDoc = true;

        private string monedaLoc;
        private string monedaExtranjera;
        private int NumFila
        {
            get
            {
                return this._listTarjetaPagos.FindIndex(det => det.Index == this.indexFila);
            }
        }

        //Variable para reporte
        private string reportName;
        private string fileURl;
        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.RefreshDocument(true);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles de la aplicacion
        /// </summary>
        public void InitControls()
        {
            TablesResources.GetTableResources(this.cmbMoneda, typeof(TipoMoneda_LocExt));
            this._bc.InitMasterUC(this.masterTarjetaCredito, AppMasters.cpTarjetaCredito, true, true, true, false);           
            this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            List<DTO_glConsultaFiltro> filtro = new List<DTO_glConsultaFiltro>();

            //Carga info de las monedas
            this.monedaLoc = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga valores por defecto
            this.masterProyecto.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.masterCentroCosto.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);

            this._tipoMoneda = 1;           

            //Personaliza algunos controles
            this.tlSeparatorPanel.RowStyles[0].Height = 150;
            this.tlSeparatorPanel.RowStyles[1].Height = 180;
            this.gbGridDocument.Dock = DockStyle.Fill;
            this.gcDocument.Size = new System.Drawing.Size(828, 227);
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
        }

        /// <summary>
        /// Carga la informacion del glDocumentoControl y el Anticipo en los controles
        /// </summary>
        private void LoadDocumentInfo()
        {
            try
            {
                this.dtFecha.DateTime = this._tarjetaDocu.PeriodoPago.Value.Value;
                this.txtValor.EditValue = this._tarjetaDocu.Valor.Value != null ? this._tarjetaDocu.Valor.Value : 0;
                this.txtDescripcion.Text = this._dtoCtrl.Observacion.Value;
                this.masterCentroCosto.Value = this._dtoCtrl.CentroCostoID.Value;
                this.masterProyecto.Value = this._dtoCtrl.ProyectoID.Value;
                TipoMoneda_LocExt tm = this._dtoCtrl.MonedaID.Value == this.monedaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                string tmVal = ((int)tm).ToString();
                this.cmbMoneda.SelectedItem = this.cmbMoneda.GetItem(tmVal);

                this.txtValor.Enabled = true;

                //this._listTarjetaPagos.OrderBy

                this.gcDocument.DataSource = this._listTarjetaPagos;
                decimal pagos = 0;
                foreach (var pago in this._listTarjetaPagos)
                {
                    pagos += pago.Valor.Value.Value;
                }
                this.txtTotal.EditValue = pagos;
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TarjetaPago.cs-LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Recarga los campos del formulario
        /// </summary>
        private void RefreshDocument(bool allData)
        {            
            if (allData)
            {
                this.masterTarjetaCredito.Value = string.Empty;
                this.dtDocumentoTercero.DateTime = base.dtFecha.DateTime;
                this.cmbMoneda.SelectedIndex = 0;
            }
            this._nuevoDoc = true;
            this.txtDescripcion.Text = string.Empty;
            this.txtValor.EditValue = 0;
            this.masterCentroCosto.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterProyecto.EnableControl(true);
            this.masterCentroCosto.EnableControl(true);

            this._listTarjetaPagos = new List<DTO_cpTarjetaPagos>();
            this._tarjetaDocu = null;
            this._tercero = string.Empty;            
            this.gcDocument.DataSource = null;
            this.LoadData(true);
            this.masterTarjetaCredito.Focus();
        }

        /// <summary>
        /// Devuelve el documento control asociado al tercero 
        /// </summary>
        /// <returns></returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(AppDocuments.PagoTarjetaCredito, this._tercero, this.dtDocumentoTercero.Text);
                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Valida controles de fechas
        /// </summary>
        private void validateDates()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);

            this.dtDocumentoTercero.DateTime = base.dtFecha.DateTime;
            
        }

        /// <summary>
        /// Valida los campos obligatorios
        /// </summary>
        /// <returns></returns>
        private string FieldsObligated()
        {
            this._message = string.Empty;

            if (this.multiMoneda)
                this._tc = _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.dtFecha.DateTime);

            var field = string.Empty;
            if (this.multiMoneda && this._tc == 0)
                return _bc.GetResource(LanguageTypes.Forms, DictionaryMessages.Err_Co_NoTasaCambio); 
            
            field = string.IsNullOrWhiteSpace(this.dtFecha.Text) ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaAnticipo") : string.Empty;
            field = !this.masterTarjetaCredito.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.cpTarjetaCredito +"_lblTitle") : field;
            field = string.IsNullOrWhiteSpace(this.dtDocumentoTercero.Text) ? field = field + "\n" + this.lblDocumentoTercero.Text : field;
            field = string.IsNullOrWhiteSpace(this.cmbMoneda.Text) ? field = field + "\n" + this.lblMoneda.Text : field;
            field = string.IsNullOrWhiteSpace(this.txtDescripcion.Text) ? field = field + "\n" + this.lblDescripcion.Text : field;
            field = !this.masterCentroCosto.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.coCentroCosto + "_lblTitle") : field;
            field = !this.masterProyecto.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.coProyecto + "_lblTitle") : field;
            if (!string.IsNullOrWhiteSpace(field))
            {
                this._message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaFieldObligated);
                this._message = string.Format(this._message, field);
            }
            if (this.gvDocument.DataRowCount == 0)
                 this._message = this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.RowsNeeded);

            return _message;
        }

        /// <summary>
        /// Carga la informacion para radicar un anticipo
        /// </summary>
        private bool LoadNewData()
        {
            try
            {
                //Campos variables DTO_glDocumentoControl
                this._dtoCtrl = new DTO_glDocumentoControl();
                this._dtoCtrl.DocumentoID.Value = this.documentID;
                this._dtoCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._dtoCtrl.Fecha.Value = DateTime.Now;
                this._dtoCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._dtoCtrl.PrefijoID.Value = this.txtPrefix.Text;

                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                //this._dtoCtrl.CuentaID.Value = this._cta.ID.Value;
                this._dtoCtrl.ProyectoID.Value = this.masterProyecto.Value;
                this._dtoCtrl.CentroCostoID.Value = this.masterCentroCosto.Value;
                this._dtoCtrl.LineaPresupuestoID.Value = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                this._dtoCtrl.LugarGeograficoID.Value = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                this._dtoCtrl.ConsSaldo.Value = 0;
                this._dtoCtrl.MonedaID.Value = this._tipoMoneda == (int)TipoMoneda.Local ? this.monedaLoc : this.monedaExtranjera;
                this._dtoCtrl.TasaCambioCONT.Value = this._tc;
                this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;
                this._dtoCtrl.TerceroID.Value = this._tercero;
                this._dtoCtrl.DocumentoTercero.Value = this.dtDocumentoTercero.Text;
                this._dtoCtrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                this._dtoCtrl.seUsuarioID.Value = this.userID;
                this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._dtoCtrl.Descripcion.Value = this.txtDocDesc.Text;
                this._dtoCtrl.Valor.Value = 0;
                this._dtoCtrl.Iva.Value = 0;

                //Campos variables DTO_cpTarjetaDocu
                this._tarjetaDocu = new DTO_cpTarjetaDocu();
                this._tarjetaDocu.TarjetaCreditoID.Value = this.masterTarjetaCredito.Value;
                this._tarjetaDocu.PeriodoPago.Value = this.dtDocumentoTercero.DateTime;
                this._tarjetaDocu.Valor.Value = !string.IsNullOrEmpty(this.txtValor.Text) ? Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) : 0;

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_LoadingData));
                return false;
            }
        }

        /// <summary>
        /// Carga la informacion para actualizar un anticipo
        /// </summary>
        private bool LoadUpdateData()
        {
            try
            {
                //Campos variables DTO_glDocumentoControl       
                this._dtoCtrl.Fecha.Value = DateTime.Now;
                this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.MonedaID.Value = this._tipoMoneda == (int)TipoMoneda.Local ? this.monedaLoc : this.monedaExtranjera;

                //Campos variables 
                this._tarjetaDocu = new DTO_cpTarjetaDocu();
                this._tarjetaDocu.TarjetaCreditoID.Value = this.masterTarjetaCredito.Value;
                this._tarjetaDocu.PeriodoPago.Value = this.dtFecha.DateTime;
                this._tarjetaDocu.NumeroDoc.Value = this._dtoCtrl.NumeroDoc.Value;
                this._tarjetaDocu.Valor.Value = !string.IsNullOrEmpty(this.txtValor.Text) ? Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) : 0;
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_LoadingData));
                return false;
            }
        }

        #endregion
                              
        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            if (firstTime)
            {
                DTO_glConsulta consultaCargos = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtrosCargos = new List<DTO_glConsultaFiltro>();
                this._listTarjetaPagos = new List<DTO_cpTarjetaPagos>();
                filtrosCargos.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "CargoTipo",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "8"
                });
                consultaCargos.Filtros = filtrosCargos;
                long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.cpCargoEspecial, consultaCargos, null, true);
                List<DTO_cpCargoEspecial> masterCargo = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.cpCargoEspecial, count, 1, consultaCargos, null, true).Cast<DTO_cpCargoEspecial>().ToList();
                foreach (var cargo in masterCargo)
                {
                    DTO_cpTarjetaPagos pago = new DTO_cpTarjetaPagos();
                    pago.CargoEspecialID.Value = cargo.ID.Value;
                    pago.Descriptivo.Value = cargo.Descriptivo.Value;
                    pago.Valor.Value = 0;
                    this._listTarjetaPagos.Add(pago);
                }
                this.gcDocument.DataSource = this._listTarjetaPagos;
            }
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {
            this.indexFila = fila;
            this._listTarjetaPagos[this.indexFila].Valor.Value = Convert.ToDecimal(this.gvDocument.GetRowCellValue(fila, this.unboundPrefix + "Valor"));
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            this.InitializeComponent();
            this.InitControls();
            this.documentID =  AppDocuments.PagoTarjetaCredito;

            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.cp;
        }

        /// <summary>
        /// Funcion q se ejecuta despues de inicializar la pantalla
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this.validateDates();
            if (!this.multiMoneda)
                this.cmbMoneda.Enabled = false;
            this.AddGridCols();
            this.LoadData(true);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            //Cargo Especial
            GridColumn cargo = new GridColumn();
            cargo.FieldName = this.unboundPrefix + "CargoEspecialID";
            cargo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CargoEspecialID");
            cargo.UnboundType = UnboundColumnType.String;
            cargo.VisibleIndex = 1;
            cargo.Width = 70;
            cargo.Visible = true;
            cargo.OptionsColumn.AllowFocus = false;
            cargo.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(cargo);

            //Descripcion
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Descriptivo";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 2;
            desc.Width = 140;
            desc.Visible = true;
            desc.OptionsColumn.AllowFocus = false;
            cargo.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(desc);

            //Valor
            GridColumn valor = new GridColumn();
            valor.FieldName = this.unboundPrefix + "Valor";
            valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
            valor.UnboundType = UnboundColumnType.Decimal;
            valor.VisibleIndex = 3;
            valor.Width = 70;
            valor.ColumnEdit = this.editSpin;
            valor.Visible = true;
            valor.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valor.AppearanceCell.Options.UseTextOptions = true;
            valor.AppearanceCell.Options.UseFont = true;
            cargo.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(valor);

            this.gvDocument.OptionsView.ColumnAutoWidth = true;
        }

        #endregion

        #region Eventos Virtuales MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak.Visible = false;
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
            FormProvider.Master.itemSave.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
               FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
               FormProvider.Master.itemPrint.Enabled = false;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que valida si existe el documento y si este tiene asociado un anticipo a su vez
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void dtDocumentoTercero_Leave(object sender, EventArgs e)
        {
            try
            {
                this._nuevoDoc = true;
                this._dtoCtrl = this.GetDocumentExt();
                if (this._dtoCtrl != null)
                {
                    List<DTO_cpTarjetaPagos> _listTarjetaExist = null;
                    this._tarjetaDocu = _bc.AdministrationModel.cpTarjetaDocu_GetByEstado(_dtoCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, out _listTarjetaExist);
                    if (this._tarjetaDocu != null)
                    {
                        this._listTarjetaPagos = _listTarjetaExist;
                        this._nuevoDoc = false;
                        this.LoadDocumentInfo();
                        FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);    
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: dtDocumentoTercero_Leave" + ex.Message);
            }
        }

        /// <summary>
        /// Carga los campos para tipo de anticipo de viajes
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void master_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            GridColumn col = new GridColumn();
            int index = this.NumFila;
            try
            {
                if (master.ValidID)
                {
                    switch (master.ColId)
                    {
                        case "TarjetaCreditoID":
                            DTO_cpTarjetaCredito tarjeta = (DTO_cpTarjetaCredito)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpTarjetaCredito, false, master.Value, true);
                            this._tercero =  tarjeta != null ? tarjeta.TerceroID.Value : string.Empty;
                            this._tipoMoneda = (int)tarjeta.TipoMoneda.Value;
                        break;
                    }                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: master_Leave" + ex.Message);
            }
        }

        /// <summary>
        /// Al cambiar un item del combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tm = 0;
            try
            {
                tm = Convert.ToInt32((this.cmbMoneda.SelectedItem as ComboBoxItem).Value);
            }
            catch (Exception)
            {
                this.cmbMoneda.SelectedIndex = 0;
            }

            if (this._tipoMoneda != tm)
                this._tipoMoneda = tm;
        }

        /// <summary>
        /// Evento que controlar la digitacion del campo este es solo numerico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            GridColumn col = this.gvDocument.Columns[e.Column.FieldName];
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Valor")
                {
                    decimal pagos = 0;
                    foreach (var pago in this._listTarjetaPagos)
                    {
                        pagos += pago.Valor.Value.Value;
                    }
                    this.txtTotal.EditValue = pagos;
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            base.TBNew();
            this.RefreshDocument(true);
        }

        /// <summary>
        /// Se envia la tarjeta pago para aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            this._message = this.FieldsObligated();
            if (string.IsNullOrEmpty(this._message))
            {
                if (this._nuevoDoc)
                {
                    if (this.LoadNewData())
                    {
                        Thread process = new Thread(this.SendToApproveThread);
                        process.Start();
                    }
                }
                else
                {
                    if (this.LoadUpdateData())
                    {
                        Thread process = new Thread(this.SendToApproveThread);
                        process.Start();
                    }
                }
            }
            else
                MessageBox.Show(this._message);
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo 
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            { 
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.cpTarjetaDocu_Guardar(this.documentID, this._dtoCtrl, this._tarjetaDocu, this._listTarjetaPagos, !this._nuevoDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                #region Genera el Documento de la Tarjerta de Credito

                if (obj.GetType() == typeof(DTO_Alarma))
                {
                    DTO_Alarma alarmaTarjetaPago = (DTO_Alarma)obj;
                    int numDoc = Convert.ToInt32(alarmaTarjetaPago.NumeroDoc);

                    reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_TarjetasPago(numDoc, ExportFormatType.pdf);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                #endregion

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                    this.Invoke(this.sendToApproveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err: " + ex.Message);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion

 
    }
}
