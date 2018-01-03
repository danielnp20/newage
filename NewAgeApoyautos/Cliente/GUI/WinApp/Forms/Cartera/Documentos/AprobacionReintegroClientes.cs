using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using SentenceTransformer;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionReintegroClientes : DocumentAprobBasicForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.CleanData();
            this.TBUpdate();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private int docRecursos;

        //Listas de datos
        private DTO_ReintegrosCartera reintegroCartera = new DTO_ReintegrosCartera();
        private List<DTO_ccReintegroClienteDeta> reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();

        //Variables Privadas
        private string compCarteraID = String.Empty;
        private string saldoReintegroID = String.Empty;
        private decimal vlrTotal = 0;
        private bool hasAlarma;
        private DateTime periodo;
        private string fileName;

        #endregion

        public AprobacionReintegroClientes()
            : base()
        {
            //this.InitializeComponent();
        }

        public AprobacionReintegroClientes(string mod)
            : base(mod) { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AprobacionReintegroClientes;
            this.docRecursos = AppDocuments.ReintegroClientes;
            this.frmModule = ModulesPrefix.cc;

            //Modifica el tamaño de las Grillas
            this.TbLyPanel.RowStyles[0].Height = 65;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterComponentes, AppMasters.ccCarteraComponente, true, true, true, false);
            _bc.InitMasterUC(this.masterSaldos, AppMasters.ccReintegroSaldo, true, true, true, false);

            //Estable la fecha con base a la fecha del periodo
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            this.dtAprobReintegro.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtAprobReintegro.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            Dictionary<string, string> dicTipoMvto = new Dictionary<string, string>();
            dicTipoMvto.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_ReintegroPagosEspeciales"));
            dicTipoMvto.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_ReintegroGiros"));
            dicTipoMvto.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_ReintegroAjustes"));
            this.lkpTipoAprobacion.Properties.DataSource = dicTipoMvto;

            base.SetInitParameters();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            this.lkpTipoAprobacion.EditValue = "1";
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "Rechazado";
                noAprob.Caption = "X";
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 15;
                noAprob.Visible = false;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 2;
                clienteID.Width = 70;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(clienteID);

                //TerceroID
                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_TerceroID");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 3;
                terceroID.Width = 70;
                terceroID.Visible = false;
                terceroID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(terceroID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 5;
                nombre.Width = 200;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombre);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 6;
                libranza.Width = 100;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);

                //VlrSaldo
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this.unboundPrefix + "Valor";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.Boolean;
                vlrSaldo.VisibleIndex = 7;
                vlrSaldo.Width = 100;
                vlrSaldo.OptionsColumn.AllowEdit = false;
                vlrSaldo.ColumnEdit = editSpin;
                this.gvDocuments.Columns.Add(vlrSaldo);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this.unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.VisibleIndex = 8;
                observacion.Width = 70;
                observacion.Visible = true;
                observacion.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(observacion);

                #region Agrega las columnas de la subgrilla

                //ClienteID
                GridColumn clienteIDDetail = new GridColumn();
                clienteIDDetail.FieldName = this.unboundPrefix + "ClienteID";
                clienteIDDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ClienteID");
                clienteIDDetail.UnboundType = UnboundColumnType.String;
                clienteIDDetail.VisibleIndex = 0;
                clienteIDDetail.Width = 70;
                clienteIDDetail.Visible = true;
                clienteIDDetail.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(clienteIDDetail);

                //Nombre
                GridColumn nombreDetail = new GridColumn();
                nombreDetail.FieldName = this.unboundPrefix + "Nombre";
                nombreDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Nombre");
                nombreDetail.UnboundType = UnboundColumnType.String;
                nombreDetail.VisibleIndex = 1;
                nombreDetail.Width = 300;
                nombreDetail.Visible = true;
                nombreDetail.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(nombreDetail);

                //Libranza
                GridColumn libranzaDetail = new GridColumn();
                libranzaDetail.FieldName = this.unboundPrefix + "Libranza";
                libranzaDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Libranza");
                libranzaDetail.UnboundType = UnboundColumnType.String;
                libranzaDetail.VisibleIndex = 2;
                libranzaDetail.Width = 100;
                libranzaDetail.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(libranzaDetail);

                //VlrSaldo
                GridColumn vlrSaldoDetail = new GridColumn();
                vlrSaldoDetail.FieldName = this.unboundPrefix + "Valor";
                vlrSaldoDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrSaldo");
                vlrSaldoDetail.UnboundType = UnboundColumnType.Boolean;
                vlrSaldoDetail.VisibleIndex = 3;
                vlrSaldoDetail.Width = 100;
                vlrSaldoDetail.OptionsColumn.AllowEdit = true;
                vlrSaldoDetail.ColumnEdit = editSpin;
                this.gvDetalle.Columns.Add(vlrSaldoDetail);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "AddDocumentCols"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        {
            this.compCarteraID = String.Empty;
            this.saldoReintegroID = string.Empty;
            this.vlrTotal = 0;

            this.masterComponentes.Value = this.compCarteraID;
            this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
            this.chkSeleccionar.Checked = false;

            this.reintegroCartera = new DTO_ReintegrosCartera();
            this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();

            this.gcDocuments.DataSource = null;
        }

        #endregion

        #region Evento Formulario

        /// <summary>
        /// Evento que se ejecuta al salir de la maestra de componenentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterComponentes_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.compCarteraID != this.masterComponentes.Value)
                {
                    this.compCarteraID = this.masterComponentes.Value;
                    if (this.masterComponentes.ValidID)
                    {
                        this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
                        object res = this._bc.AdministrationModel.PagosEspeciales_GetAprobByComponente(this.actividadFlujoID, this.compCarteraID);
                        if (res.GetType() == typeof(DTO_TxResult))
                        {
                            MessageForm msg = new MessageForm((DTO_TxResult)res);
                            msg.ShowDialog();
                            this.CleanData();
                        }
                        else
                        {
                            this.reintegroCartera = (DTO_ReintegrosCartera)res;
                            if (reintegroCartera.Reintegros != null)
                            {
                                if (reintegroCartera.DetalleReintegros.Count > 0)
                                {
                                    foreach (DTO_ccReintegroClienteDeta item in reintegroCartera.Reintegros)
                                    {
                                        this.reintegrosClientes.Add(item);
                                    }
                                }
                                else
                                    this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
                            }
                            else
                                this.reintegrosClientes.AddRange(this.reintegroCartera.DetalleReintegros);
                        }

                        if (this.reintegrosClientes.Count > 0)
                        {
                            this.currentRow = 0;
                            this.chkSeleccionar.Checked = true;
                            this.reintegrosClientes.Where(x => x.Aprobado.Value.Value).ToList().ForEach(y => this.vlrTotal += y.Valor.Value.Value);
                            this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                            this.gcDocuments.DataSource = this.reintegrosClientes;
                            this.gvDocuments.BestFitColumns();
                            this.gvDocuments.MoveFirst();
                        }
                        else
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            this.CleanData();
                        }
                    }
                    else
                    {
                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.compCarteraID);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "masterComponentes_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir de la maestra de componenentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterSaldos_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.saldoReintegroID != this.masterSaldos.Value)
                {
                    this.saldoReintegroID = this.masterSaldos.Value;
                    if (this.masterSaldos.ValidID)
                    {
                        DTO_ccReintegroSaldo saldo = (DTO_ccReintegroSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccReintegroSaldo, false, this.saldoReintegroID, true);
                        this.reintegrosClientes = this._bc.AdministrationModel.ReintegroClientes_GetAprobByCuenta(this.actividadFlujoID, saldo.CuentaID.Value);
                        if (this.lkpTipoAprobacion.EditValue == "2")
                            this.reintegrosClientes = this.reintegrosClientes.Where(x => string.IsNullOrWhiteSpace(x.CuentaReintegroID.Value)).ToList();
                        else
                            this.reintegrosClientes = this.reintegrosClientes.Where(x => !string.IsNullOrWhiteSpace(x.CuentaReintegroID.Value)).ToList();

                        this.vlrTotal = this.reintegrosClientes.Sum(x => x.Valor.Value.Value);
                        if (this.reintegrosClientes.Count > 0)
                        {
                            this.currentRow = 0;
                            this.chkSeleccionar.Checked = true;
                            
                            this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                            this.gcDocuments.DataSource = this.reintegrosClientes;
                            this.gvDocuments.BestFitColumns();
                            this.gvDocuments.MoveFirst();
                        }
                        else
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            this.CleanData();
                        }
                    }
                    else
                    {
                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.saldoReintegroID);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "masterSaldos_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el estado del control
        /// </summary>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkSeleccionar.Checked)
                {
                    for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                    {
                        this.vlrTotal = Convert.ToDecimal(this.txtVlrTotalReintegro.EditValue, CultureInfo.InvariantCulture);
                        this.reintegrosClientes[i].Aprobado.Value = true;
                        this.reintegrosClientes[i].Rechazado.Value = false;
                        this.txtVlrTotalReintegro.EditValue = (this.vlrTotal + this.reintegrosClientes[i].Valor.Value);
                    }
                }
                else
                {
                    for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                    {
                        this.vlrTotal = 0;
                        this.reintegrosClientes[i].Aprobado.Value = false;
                        this.reintegrosClientes[i].Rechazado.Value = false;
                        this.txtVlrTotalReintegro.EditValue = 0;
                    }
                }
                this.gcDocuments.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReintegroClientes.cs", "chkAll_CheckedChanged"));
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvDocuments.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();


                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.reintegrosClientes[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "editLink_Click"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el tipo de aprobación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkpTipoAprobacion_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.compCarteraID = string.Empty;
                this.masterComponentes.Value = string.Empty;
                this.saldoReintegroID = string.Empty;
                this.masterSaldos.Value = string.Empty;

                //Componente cartera
                if(Convert.ToInt32(lkpTipoAprobacion.EditValue) == 1)
                {
                    this.masterComponentes.Visible = true;
                    this.masterSaldos.Visible = false;
                    this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].OptionsColumn.AllowEdit = true; 
                    this.gvDocuments.Columns[this.unboundPrefix + "ClienteID"].Visible = true;
                    this.gvDocuments.Columns[this.unboundPrefix + "TerceroID"].Visible = false;

                    // columnas de la subgrilla
                    foreach(GridColumn col in this.gvDetalle.Columns)
                        col.Visible = true;
                }
                else if (Convert.ToInt32(lkpTipoAprobacion.EditValue) == 2)
                {
                    this.masterComponentes.Visible = false;
                    this.masterSaldos.Visible = true;
                    this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].OptionsColumn.AllowEdit = true; 
                    this.gvDocuments.Columns[this.unboundPrefix + "ClienteID"].Visible = false;
                    this.gvDocuments.Columns[this.unboundPrefix + "TerceroID"].Visible = true;

                    // columnas de la subgrilla
                    foreach (GridColumn col in this.gvDetalle.Columns)
                        col.Visible = false;
                }
                else
                {
                    this.masterComponentes.Visible = false;
                    this.masterSaldos.Visible = true;
                    this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].OptionsColumn.AllowEdit = false; 
                    this.gvDocuments.Columns[this.unboundPrefix + "ClienteID"].Visible = false;
                    this.gvDocuments.Columns[this.unboundPrefix + "TerceroID"].Visible = true;

                    // columnas de la subgrilla
                    foreach (GridColumn col in this.gvDetalle.Columns)
                        col.Visible = false;
                }

                this.vlrTotal = 0;
                this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
                this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                this.gcDocuments.DataSource = this.reintegrosClientes;
                //this.gvDocuments.BestFitColumns();
                this.gvDocuments.MoveFirst();
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "lkpTipoAprobacion_EditValueChanged"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            #region Generales
            if (fieldName == "Aprobado")
            {
                decimal vlrTemp = 0;
                if ((bool)e.Value)
                {
                    vlrTemp = Convert.ToDecimal(this.txtVlrTotalReintegro.EditValue, CultureInfo.InvariantCulture);
                    this.vlrTotal = vlrTemp + this.reintegrosClientes[e.RowHandle].Valor.Value.Value;
                    this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                    this.reintegrosClientes[e.RowHandle].Rechazado.Value = false;
                }
                else
                {
                    vlrTemp = Convert.ToDecimal(this.txtVlrTotalReintegro.EditValue, CultureInfo.InvariantCulture);
                    if (vlrTemp == 0)
                        this.vlrTotal = 0;
                    else
                        this.vlrTotal = vlrTemp - this.reintegrosClientes[e.RowHandle].Valor.Value.Value;
                    this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                    this.reintegrosClientes[e.RowHandle].Aprobado.Value = false;
                }
            }

            if (fieldName == "Rechazado")
            {
                decimal vlrTemp = 0;
                if ((bool)e.Value)
                {
                    vlrTemp = Convert.ToDecimal(this.txtVlrTotalReintegro.EditValue, CultureInfo.InvariantCulture);
                    this.vlrTotal = vlrTemp - this.reintegrosClientes[e.RowHandle].Valor.Value.Value;
                    this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                    this.reintegrosClientes[e.RowHandle].Aprobado.Value = false;
                }
                else
                {
                    vlrTemp = Convert.ToDecimal(this.txtVlrTotalReintegro.EditValue, CultureInfo.InvariantCulture);
                    if (vlrTemp == 0)
                        this.vlrTotal = 0;
                    else
                        this.vlrTotal = vlrTemp + this.reintegrosClientes[e.RowHandle].Valor.Value.Value;
                    this.txtVlrTotalReintegro.EditValue = this.vlrTotal;
                    this.reintegrosClientes[e.RowHandle].Aprobado.Value = false;
                }
            }

            #endregion
            this.gcDocuments.RefreshDataSource();
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
                List<DTO_ccReintegroClienteDeta> reintegroTemp = this.reintegrosClientes.Where(x => x.Aprobado.Value == true).ToList();
                if (reintegroTemp.Count > 0 && this.vlrTotal > 0)
                {
                    this.reintegrosClientes = this.reintegrosClientes.Where(x => x.Aprobado.Value == true).ToList();
                    this.reintegrosClientes.ForEach(x => x.FechaAprobacionReintegro.Value = this.dtAprobReintegro.DateTime);
                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.compCarteraID = string.Empty;
                this.saldoReintegroID = string.Empty;
                if (Convert.ToInt32(lkpTipoAprobacion.EditValue) == 1)
                {
                    this.masterComponentes_Leave(null, null);
                }
                else 
                {
                    this.masterSaldos_Leave(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "TBUpdate"));
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
                #region Guarda la info
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();

                string headerRsx = this._bc.GetImportExportFormat(typeof(DTO_MigrarFacturaVenta), AppProcess.MigracionNewFact, ",", true);

                if(Convert.ToInt32(this.lkpTipoAprobacion.EditValue) == 1)
                {
                    results = _bc.AdministrationModel.PagosEspeciales_Aprobar(this.documentID, this.actividadFlujoID, headerRsx, this.reintegrosClientes);
                }
                else if (Convert.ToInt32(this.lkpTipoAprobacion.EditValue) == 2)
                {
                    results = _bc.AdministrationModel.ReintegroClientes_AprobarGiro(this.documentID, this.actividadFlujoID, headerRsx, this.reintegrosClientes);
                }
                else if (Convert.ToInt32(this.lkpTipoAprobacion.EditValue) == 3)
                {
                    results = _bc.AdministrationModel.ReintegroClientes_AprobarAjuste(this.documentID, this.actividadFlujoID, headerRsx, this.reintegrosClientes);
                }

                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                #region Contador
                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                List<int> docsOK = new List<int>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                #endregion
                #region Procesa los resultados
                MessageForm frm = null;
                foreach (object obj in results)
                {
                    #region Funciones de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion

                    if (obj.GetType() == typeof(DTO_TxResult) && ((DTO_TxResult)obj).Result == ResultValue.NOK)
                    {
                        DTO_TxResult r = (DTO_TxResult)obj;
                        resultsNOK.Add(r);
                        i++;
                        continue;
                    }

                    if (Convert.ToInt32(this.lkpTipoAprobacion.EditValue) == 1 && this.reintegrosClientes[i].Aprobado.Value.Value)
                    {
                        bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.AprobacionReintegroClientes, this.actividadDTO.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                        }
                        else
                        {
                            //Trae la direccion del archivo csv y los NumDoc para genera la CxP
                            DTO_Alarma alarma = (DTO_Alarma)obj;
                            this.fileName = alarma.FileName;
                            int numDoc = Convert.ToInt32(alarma.NumeroDoc);
                            docsOK.Add(numDoc);
                            this.hasAlarma = true;
                        }
                    }
                    i++;
                }
                frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                #endregion
                #region Genera Documento de cuentas por pagar y archivo plano para tesoreria

                if (this.hasAlarma)
                {
                    #region Pregunta si desea abrir los reportes

                    bool deseaImp = false;
                    if (docsOK.Count > 0)
                    {
                        string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                        var result = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                            deseaImp = true;
                    }

                    #endregion
                    #region Genera e imprime los reportes
                    foreach (int item in docsOK)
                    {
                        string reportName = this._bc.AdministrationModel.Reportes_Cp_CausacionFacturas(item, true,false, ExportFormatType.pdf);
                        if (deseaImp)
                        {
                            string docName = this._bc.UrlDocumentFile(TipoArchivo.Documentos, item, null, reportName.ToString());
                            Process.Start(docName);
                        }
                    }
                    #endregion

                    //Genera el Archivo Plano
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, this.fileName);
                    Process.Start(fileURl);

                }
                #endregion

                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionReintegroClientes.cs", "ApproveThread"));
                this.Invoke(this.refreshData);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}