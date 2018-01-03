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
using System.Globalization;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PagosEspeciales : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private DTO_ReintegrosCartera reintegroCartera = new DTO_ReintegrosCartera();
        private List<DTO_ccReintegroClienteDeta> reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();

        //Variables privadas
        private int indexPadre = 0;
        private string compCarteraID = String.Empty;
        private DateTime periodo;
        private decimal vlrTotal = 0;
        private decimal vlrTotalSubGrilla = 0;

        #endregion

        public PagosEspeciales()
            : base()
        {
            //InitializeComponent();
        }

        public PagosEspeciales(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.InitializeComponent();
                base.SetInitParameters();

                this.documentID = AppDocuments.ReintegroClientes;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();

                //Carga la maestra de componentes de cartera
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoComponente",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = ((byte)TipoComponente.MayorValor).ToString(),
                    OperadorSentencia = OperadorSentencia.Or
                });
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoComponente",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = ((byte)TipoComponente.ComponenteExtra).ToString(),
                    OperadorSentencia = OperadorSentencia.Or
                });

                this._bc.InitMasterUC(this.masterComponentes, AppMasters.ccCarteraComponente, true, true, true, false, filtros);

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 160;
                this.tlSeparatorPanel.RowStyles[1].Height = 400;

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

                //gvDetale
                this.gvDetalle.OptionsBehavior.Editable = true;
                this._frmNewName = _bc.GetResource(LanguageTypes.Menu, "mnu_cc_pagosespeciales");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            try
            {
                this.Text = _bc.GetResource(LanguageTypes.Menu, "PagosEspeciales");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 50;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(aprob);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 2;
                clienteID.Width = 70;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 3;
                nombre.Width = 300;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombre);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 4;
                libranza.Width = 100;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //VlrSaldo
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this.unboundPrefix + "Valor";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.Boolean;
                vlrSaldo.VisibleIndex = 5;
                vlrSaldo.Width = 100;
                vlrSaldo.OptionsColumn.AllowEdit = true;
                vlrSaldo.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrSaldo);


                #region Agrega las columnas de la subgrilla
                //Aprobar
                GridColumn aprobDetail = new GridColumn();
                aprobDetail.FieldName = this.unboundPrefix + "Aprobado";
                aprobDetail.Caption = "√";
                aprobDetail.UnboundType = UnboundColumnType.Boolean;
                aprobDetail.VisibleIndex = 0;
                aprobDetail.Width = 50;
                aprobDetail.Visible = true;
                aprobDetail.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprobDetail.AppearanceHeader.ForeColor = Color.Lime;
                aprobDetail.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprobDetail.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprobDetail.AppearanceHeader.Options.UseTextOptions = true;
                aprobDetail.AppearanceHeader.Options.UseFont = true;
                aprobDetail.AppearanceHeader.Options.UseForeColor = true;
                this.gvDetalle.Columns.Add(aprobDetail);

                //ClienteID
                GridColumn clienteIDDetail = new GridColumn();
                clienteIDDetail.FieldName = this.unboundPrefix + "ClienteID";
                clienteIDDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteIDDetail.UnboundType = UnboundColumnType.String;
                clienteIDDetail.VisibleIndex = 2;
                clienteIDDetail.Width = 70;
                clienteIDDetail.Visible = true;
                clienteIDDetail.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(clienteIDDetail);

                //Nombre
                GridColumn nombreDetail = new GridColumn();
                nombreDetail.FieldName = this.unboundPrefix + "Nombre";
                nombreDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombreDetail.UnboundType = UnboundColumnType.String;
                nombreDetail.VisibleIndex = 3;
                nombreDetail.Width = 300;
                nombreDetail.Visible = true;
                nombreDetail.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(nombreDetail);

                //Libranza
                GridColumn libranzaDetail = new GridColumn();
                libranzaDetail.FieldName = this.unboundPrefix + "Libranza";
                libranzaDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranzaDetail.UnboundType = UnboundColumnType.String;
                libranzaDetail.VisibleIndex = 4;
                libranzaDetail.Width = 100;
                libranzaDetail.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(libranzaDetail);

                //VlrSaldo
                GridColumn vlrSaldoDetail = new GridColumn();
                vlrSaldoDetail.FieldName = this.unboundPrefix + "Valor";
                vlrSaldoDetail.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldo");
                vlrSaldoDetail.UnboundType = UnboundColumnType.Boolean;
                vlrSaldoDetail.VisibleIndex = 5;
                vlrSaldoDetail.Width = 100;
                vlrSaldoDetail.OptionsColumn.AllowEdit = true;
                vlrSaldoDetail.ColumnEdit = editSpin;
                this.gvDetalle.Columns.Add(vlrSaldoDetail);
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "AddGridCols"));
            }

        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.masterComponentes.Value = String.Empty;
            this.txt_VlrTotaReintegro.Text = String.Empty;
            this.chkAll.Checked = false;

            //Variables
            this.compCarteraID = string.Empty;
            this.vlrTotal = 0;

            this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
            this.gcDocument.DataSource = this.reintegrosClientes;

        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemDelete.Visible = false;
                    FormProvider.Master.itemSendtoAppr.Visible = false;
                    FormProvider.Master.itemCopy.Visible = false;
                    FormProvider.Master.itemPaste.Visible = false;
                    FormProvider.Master.itemImport.Visible = false;
                    FormProvider.Master.itemExport.Visible = false;
                    FormProvider.Master.itemRevert.Visible = false;
                    FormProvider.Master.itemGenerateTemplate.Visible = false;
                    FormProvider.Master.itemFilter.Visible = false;
                    FormProvider.Master.itemFilterDef.Visible = false;
                    FormProvider.Master.tbBreak1.Visible = false;
                    FormProvider.Master.tbBreak2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al momento de salir del componente de cartera
        /// </summary>
        private void masterComponentes_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.compCarteraID != this.masterComponentes.Value)
                {
                    this.compCarteraID = this.masterComponentes.Value;
                    this.reintegrosClientes = new List<DTO_ccReintegroClienteDeta>();
                    this.vlrTotal = 0;
                    if (this.masterComponentes.ValidID)
                    {
                        object res = this._bc.AdministrationModel.PagosEspeciales_GetByComponente(this._actFlujo.ID.Value, this.compCarteraID);
                        if (res.GetType() == typeof(DTO_TxResult))
                        {
                            MessageForm msg = new MessageForm((DTO_TxResult)res);
                            msg.ShowDialog();
                            this.CleanData();
                        }
                        else
                        {
                            this.reintegroCartera = (DTO_ReintegrosCartera)res;
                            bool hasDetalle = true;
                            if (reintegroCartera.Reintegros != null)
                            {
                                foreach (DTO_ccReintegroClienteDeta item in reintegroCartera.Reintegros)
                                {
                                    this.reintegrosClientes.Add(item);
                                    hasDetalle = item.HasDetalle.Value.Value;
                                }
                            }
                            else
                            {
                                this.reintegrosClientes.AddRange(this.reintegroCartera.DetalleReintegros);
                                hasDetalle = this.reintegrosClientes.Count > 0 ? true : false;
                            }

                            if (this.reintegrosClientes.Count > 0 && hasDetalle)
                            {
                                this.chkAll.Checked = true;
                                this.reintegrosClientes.Where(x => x.Aprobado.Value.Value).ToList().ForEach(y => this.vlrTotal += y.Valor.Value.Value);
                                this.txt_VlrTotaReintegro.EditValue = this.vlrTotal;
                                this.gcDocument.DataSource = this.reintegrosClientes;
                                this.gvDocument.MoveFirst();
                            }
                            else
                            {
                                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                                this.CleanData();
                            }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "masterCompradorCartera_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el estado del control
        /// </summary>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {                
                decimal vlrTotalTemp = 0;
                if (this.chkAll.Checked)
                {
                    foreach (DTO_ccReintegroClienteDeta item in this.reintegrosClientes)
                    {
                        decimal vlrTempSum = 0;
                        item.Aprobado.Value = true;
                        if (item.Detalle.Count > 0)
                        {
                            item.Detalle.ForEach(y => y.Aprobado.Value = true);
                            item.Detalle.ForEach(y => vlrTempSum += y.Valor.Value.Value);
                            item.Valor.Value = vlrTempSum;
                            vlrTotalTemp += vlrTempSum;
                        }
                        else
                        {
                            vlrTempSum = item.Valor.Value.Value;
                            vlrTotalTemp += vlrTempSum;
                        }                                                
                    }
                    this.txt_VlrTotaReintegro.EditValue = vlrTotalTemp;
                }
                else
                {
                    foreach (DTO_ccReintegroClienteDeta item in this.reintegrosClientes)
                    {
                        item.Aprobado.Value = false;
                        if (item.Detalle.Count > 0)
                        {
                            item.Detalle.ForEach(y => y.Aprobado.Value = false);
                            item.Valor.Value = 0;
                        }                       
                    }
                    this.txt_VlrTotaReintegro.EditValue = 0;
                }
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "chkAll_CheckedChanged"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            #region Generales
            if (fieldName == "Aprobado")
            {
                decimal vlrTemp = 0;
                decimal vlrTempSum = 0;
                if ((bool)e.Value)
                {
                    vlrTemp = Convert.ToDecimal(this.txt_VlrTotaReintegro.EditValue, CultureInfo.InvariantCulture);
                    if (this.reintegrosClientes[e.RowHandle].Detalle.Count > 0)
                    {
                        this.reintegrosClientes[e.RowHandle].Detalle.ForEach(x => x.Aprobado.Value = true);
                        vlrTempSum = (from c in this.reintegrosClientes[e.RowHandle].Detalle select c.Valor.Value.Value).Sum();
                        this.reintegrosClientes[e.RowHandle].Valor.Value = vlrTempSum;
                        this.txt_VlrTotaReintegro.EditValue = vlrTemp + vlrTempSum;
                    }
                    else
                    {
                        this.vlrTotal = vlrTemp + this.reintegrosClientes[e.RowHandle].Valor.Value.Value;
                        this.txt_VlrTotaReintegro.EditValue = this.vlrTotal;
                    }

                    this.reintegrosClientes[e.RowHandle].Aprobado.Value = true;
                }
                else
                {
                    vlrTemp = Convert.ToDecimal(this.txt_VlrTotaReintegro.EditValue, CultureInfo.InvariantCulture);

                    if (this.reintegrosClientes[e.RowHandle].Detalle.Count > 0)
                    {
                        this.reintegrosClientes[e.RowHandle].Detalle.ForEach(x => x.Aprobado.Value = false);
                        vlrTempSum = (from c in this.reintegrosClientes[e.RowHandle].Detalle select c.Valor.Value.Value).Sum();
                        this.reintegrosClientes[e.RowHandle].Valor.Value = 0;
                        this.txt_VlrTotaReintegro.EditValue = vlrTemp - vlrTempSum;
                    }
                    else
                    {
                        this.vlrTotal = vlrTemp - this.reintegrosClientes[e.RowHandle].Valor.Value.Value;
                        this.txt_VlrTotaReintegro.EditValue = this.vlrTotal;
                    }

                    this.reintegrosClientes[e.RowHandle].Aprobado.Value = false;
                }
            }

            #endregion
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.indexPadre = e.FocusedRowHandle;
        }

        #endregion

        #region Eventos grilla de detalles

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDetalle_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            bool aprobTempPadre = true;
            #region Generales
            if (fieldName == "Aprobado")
            {
                decimal vlrTemp = Convert.ToDecimal(this.txt_VlrTotaReintegro.EditValue, CultureInfo.InvariantCulture);
                decimal vlrTempSubgrilla = this.reintegrosClientes[this.indexPadre].Detalle[e.RowHandle].Valor.Value.Value;
                this.vlrTotalSubGrilla = this.reintegrosClientes[this.indexPadre].Valor.Value.Value;
                if ((bool)e.Value)
                {
                    this.vlrTotalSubGrilla = this.vlrTotalSubGrilla + vlrTempSubgrilla;
                    this.reintegrosClientes[this.indexPadre].Valor.Value = this.vlrTotalSubGrilla;
                    this.txt_VlrTotaReintegro.EditValue = vlrTemp + vlrTempSubgrilla;
                    this.reintegrosClientes[this.indexPadre].Detalle[e.RowHandle].Aprobado.Value = true;
                }
                else
                {
                    this.vlrTotalSubGrilla = this.vlrTotalSubGrilla - vlrTempSubgrilla;
                    this.reintegrosClientes[this.indexPadre].Valor.Value = this.vlrTotalSubGrilla;
                    this.txt_VlrTotaReintegro.EditValue = vlrTemp - vlrTempSubgrilla;
                    this.reintegrosClientes[this.indexPadre].Detalle[e.RowHandle].Aprobado.Value = false;
                }
                List<DTO_ccReintegroClienteDeta> cliTemp = this.reintegrosClientes[this.indexPadre].Detalle.Where(x => x.Aprobado.Value == true).ToList();
                aprobTempPadre = cliTemp.Count > 0 ? true : false;
                this.reintegrosClientes[this.indexPadre].Aprobado.Value = aprobTempPadre;
            }
            #endregion
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.masterComponentes.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            this.gvDetalle.PostEditor();
            try
            {

                int numDoc = 0;
                foreach (DTO_ccReintegroClienteDeta item in this.reintegrosClientes)
                {
                    if (item.Detalle.Count > 0)
                        numDoc = (from c in item.Detalle where c.NumeroDoc.Value.Value != 0 select c.NumeroDoc.Value.Value).FirstOrDefault();
                    else
                        numDoc = (from c in this.reintegrosClientes where c.NumeroDoc.Value.Value != 0 select c.NumeroDoc.Value.Value).FirstOrDefault();

                    if (numDoc != 0)
                        break;
                }

                List<DTO_ccReintegroClienteDeta> reintegroTemp = this.reintegrosClientes.Where(x => x.Aprobado.Value == true).ToList();                
                if (numDoc != 0)
                {
                    foreach (DTO_ccReintegroClienteDeta item in reintegroTemp)
                    {
                        if (item.Detalle.Count > 0)
                            item.Detalle.ForEach(x => x.NumeroDoc.Value = numDoc);
                        else
                            item.NumeroDoc.Value = numDoc;
                    }
                }
               
                if (reintegroTemp.Count > 0 && this.vlrTotal > 0)
                {
                    this.reintegrosClientes = reintegroTemp;
                    this.reintegrosClientes.ForEach(x => x.Detalle = x.Detalle.Where(y => y.Aprobado.Value == true).ToList());
                    Thread process = new Thread(this.SaveThread);
                    this.reintegrosClientes.ForEach(x => x.FechaReintegro.Value = this.dtFecha.DateTime);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                    MessageBox.Show(msg);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        public override void SaveThread()
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
                results = _bc.AdministrationModel.ReintegroClientes_Add(this.documentID, this._actFlujo.ID.Value, this.reintegrosClientes, this.dtFecha.DateTime, 
                    this.vlrTotal, true, string.Empty);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                this.isValid = true;
                MessageForm frm = null;
                bool checkResults = true;
                if (results.Count == 1)
                {
                    if (results[0].GetType() == typeof(DTO_TxResult))
                    {
                        checkResults = false;
                        frm = new MessageForm((DTO_TxResult)results[0]);
                        this.isValid = false;
                    }
                }

                if (checkResults)
                {
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

                        if (this.reintegrosClientes[i].Aprobado.Value.Value)
                        {
                            bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.ReintegroClientes, this._actFlujo.seUsuarioID.Value, obj, false);
                            if (!isOK)
                            {
                                DTO_TxResult r = (DTO_TxResult)obj;
                                resultsNOK.Add(r);
                                this.isValid = false;
                            }
                        }

                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (this.isValid)
                    this.Invoke(this.refreshGridDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosEspeciales.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }

}
