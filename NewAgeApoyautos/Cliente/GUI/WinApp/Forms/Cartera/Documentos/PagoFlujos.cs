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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PagoFlujos : DocumentForm
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
        private DTO_PagoFlujos pagoFlujos = new DTO_PagoFlujos();
        private List<DTO_ccFlujoCesionDocu> flujosDocu = new List<DTO_ccFlujoCesionDocu>();

        //Variables privadas
        private DateTime periodo;

        #endregion

        public PagoFlujos()
            : base()
        {
            //InitializeComponent();
        }

        public PagoFlujos(string mod)
            : base(mod)
        {
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            //variables
            this.flujosDocu = new List<DTO_ccFlujoCesionDocu>();
            this.txt_VlrTotalFlujo.EditValue = 0;
            this.txtLibranza.Text = string.Empty;
            this.masterComprador.Value = string.Empty;
            this.txtOferta.Text = string.Empty;

            this.pagoFlujos = new DTO_PagoFlujos();
            this.gcDocument.DataSource = this.pagoFlujos.FlujoCesionDeta;
        }

        /// <summary>
        /// Calcula el total del flujo
        /// </summary>
        private void CalcularTotal()
        {
            try
            {
                decimal valorTot = 0;
                this.flujosDocu.ForEach(x => valorTot += (from c in x.Detalle where c.Aprobado.Value.Value select c.VlrCuota.Value.Value).Sum());
                this.txt_VlrTotalFlujo.EditValue = valorTot;

                this.flujosDocu[this.gvDocument.FocusedRowHandle].ValorPago.Value =
                    this.flujosDocu[this.gvDocument.FocusedRowHandle].Detalle.Where(x => x.Aprobado.Value.Value).Sum(f => f.VlrCuota.Value.Value);

                this.gvDocument.RefreshData();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "CalcularTotal"));
            }
        }

        #endregion

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

                this.documentID = AppDocuments.PagoFlujos;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 140;
                this.tlSeparatorPanel.RowStyles[1].Height = 400;

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);
                this.dtFecha.Enabled = false;
                this.dtFechaFlujo.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtFechaFlujo.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "SetInitParameters"));
            }
        }
        
        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.dtFechaFlujo.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            _bc.InitMasterUC(this.masterComprador, AppMasters.ccCompradorCartera, true, true, true, false);

            this.gvDetalle.OptionsBehavior.Editable = true;
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //PagoFlujoInd
                GridColumn pagoFlujoInd = new GridColumn();
                pagoFlujoInd.FieldName = this.unboundPrefix + "PagoFlujoInd";
                pagoFlujoInd.Caption = "√";
                pagoFlujoInd.UnboundType = UnboundColumnType.Boolean;
                pagoFlujoInd.VisibleIndex = 0;
                pagoFlujoInd.Width = 40;
                pagoFlujoInd.OptionsColumn.AllowEdit = true;
                pagoFlujoInd.Visible = true;
                pagoFlujoInd.ColumnEdit = editChkBox;
                pagoFlujoInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoFlujoInd");
                pagoFlujoInd.AppearanceHeader.ForeColor = Color.Lime;
                pagoFlujoInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                pagoFlujoInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                pagoFlujoInd.AppearanceHeader.Options.UseTextOptions = true;
                pagoFlujoInd.AppearanceHeader.Options.UseFont = true;
                pagoFlujoInd.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(pagoFlujoInd);

                //PagoInversionista
                GridColumn pagoInversionista = new GridColumn();
                pagoInversionista.FieldName = this.unboundPrefix + "PagoInversionista";
                pagoInversionista.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoInversionista");
                pagoInversionista.UnboundType = UnboundColumnType.Boolean;
                pagoInversionista.VisibleIndex = 1;
                pagoInversionista.Width = 50;
                pagoInversionista.Visible = true;
                pagoInversionista.OptionsColumn.AllowEdit = false;
                pagoInversionista.ColumnEdit = this.editChkBox;
                this.gvDocument.Columns.Add(pagoInversionista);

                //Oferta
                GridColumn oferta = new GridColumn();
                oferta.FieldName = this.unboundPrefix + "Oferta";
                oferta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Oferta");
                oferta.UnboundType = UnboundColumnType.String;
                oferta.VisibleIndex = 2;
                oferta.Width = 90;
                oferta.Visible = true;
                oferta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(oferta);

                //Portafolio
                GridColumn portafolio = new GridColumn();
                portafolio.FieldName = this.unboundPrefix + "Portafolio";
                portafolio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Portafolio");
                portafolio.UnboundType = UnboundColumnType.String;
                portafolio.VisibleIndex = 3;
                portafolio.Width = 90;
                portafolio.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(portafolio);

                //Comprador Cartera
                GridColumn compradorCartera = new GridColumn();
                compradorCartera.FieldName = this.unboundPrefix + "CompradorCarteraID";
                compradorCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorCartera");
                compradorCartera.UnboundType = UnboundColumnType.String;
                compradorCartera.VisibleIndex = 4;
                compradorCartera.Width = 90;
                compradorCartera.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(compradorCartera);

                //NombreCompradorCartera
                GridColumn nombreCompradorCartera = new GridColumn();
                nombreCompradorCartera.FieldName = this.unboundPrefix + "NombreCompradorCartera";
                nombreCompradorCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombreCompradorCartera.UnboundType = UnboundColumnType.String;
                nombreCompradorCartera.VisibleIndex = 5;
                nombreCompradorCartera.Width = 90;
                nombreCompradorCartera.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombreCompradorCartera);

                //Inversionista
                GridColumn inversionista = new GridColumn();
                inversionista.FieldName = this.unboundPrefix + "Inversionista";
                inversionista.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Inversionista");
                inversionista.UnboundType = UnboundColumnType.String;
                inversionista.VisibleIndex = 6;
                inversionista.Width = 90;
                inversionista.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inversionista);

                //NombreInversionista
                GridColumn nombreInversionista = new GridColumn();
                nombreInversionista.FieldName = this.unboundPrefix + "NombreInversionista";
                nombreInversionista.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombreInversionista.UnboundType = UnboundColumnType.String;
                nombreInversionista.VisibleIndex = 7;
                nombreInversionista.Width = 90;
                nombreInversionista.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombreInversionista);

                //NumCreditos
                GridColumn numCreditos = new GridColumn();
                numCreditos.FieldName = this.unboundPrefix + "NumCreditos";
                numCreditos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumCreditos");
                numCreditos.UnboundType = UnboundColumnType.Integer;
                numCreditos.VisibleIndex = 8;
                numCreditos.Width = 70;
                numCreditos.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numCreditos);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Integer;
                valor.VisibleIndex = 9;
                valor.Width = 180;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                valor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(valor);

                //ValorPago
                GridColumn valorPago = new GridColumn();
                valorPago.FieldName = this.unboundPrefix + "ValorPago";
                valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorPago");
                valorPago.UnboundType = UnboundColumnType.Integer;
                valorPago.VisibleIndex = 10;
                valorPago.Width = 180;
                valorPago.Visible = true;
                valorPago.OptionsColumn.AllowEdit = false;
                valorPago.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(valorPago);

                #region Campos SubGrilla

                //PagoFlujoInd
                GridColumn pagoFlujoDetalleInd = new GridColumn();
                pagoFlujoDetalleInd.FieldName = this.unboundPrefix + "Aprobado";
                pagoFlujoDetalleInd.Caption = "√";
                pagoFlujoDetalleInd.UnboundType = UnboundColumnType.Boolean;
                pagoFlujoDetalleInd.VisibleIndex = 0;
                pagoFlujoDetalleInd.Width = 40;
                pagoFlujoDetalleInd.OptionsColumn.AllowEdit = true;
                pagoFlujoDetalleInd.Visible = true;
                pagoFlujoDetalleInd.ColumnEdit = editChkBox;
                pagoFlujoDetalleInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoFlujoInd");
                pagoFlujoDetalleInd.AppearanceHeader.ForeColor = Color.Lime;
                pagoFlujoDetalleInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                pagoFlujoDetalleInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                pagoFlujoDetalleInd.AppearanceHeader.Options.UseTextOptions = true;
                pagoFlujoDetalleInd.AppearanceHeader.Options.UseFont = true;
                pagoFlujoDetalleInd.AppearanceHeader.Options.UseForeColor = true;
                this.gvDetalle.Columns.Add(pagoFlujoDetalleInd);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 1;
                libranza.Width = 70;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(libranza);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 2;
                clienteID.Width = 70;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(clienteID);

                //NombreCliente
                GridColumn nombreCliente = new GridColumn();
                nombreCliente.FieldName = this.unboundPrefix + "Nombre";
                nombreCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombreCliente.UnboundType = UnboundColumnType.String;
                nombreCliente.VisibleIndex = 3;
                nombreCliente.Width = 200;
                nombreCliente.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(nombreCliente);

                //CuotaID
                GridColumn cuotaID = new GridColumn();
                cuotaID.FieldName = this.unboundPrefix + "PrimeraCuota";
                cuotaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaID");
                cuotaID.UnboundType = UnboundColumnType.Integer;
                cuotaID.VisibleIndex = 4;
                cuotaID.Width = 70;
                cuotaID.Visible = true;
                cuotaID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(cuotaID);

                //Fecha Cuota
                GridColumn fechaCuota = new GridColumn();
                fechaCuota.FieldName = this.unboundPrefix + "FechaCuota1";
                fechaCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCta");
                fechaCuota.UnboundType = UnboundColumnType.DateTime;
                fechaCuota.VisibleIndex = 5;
                fechaCuota.Width = 70;
                fechaCuota.OptionsColumn.AllowEdit = true;
                this.gvDetalle.Columns.Add(fechaCuota);

                //Valor Cuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrCuota.UnboundType = UnboundColumnType.Integer;
                vlrCuota.VisibleIndex = 6;
                vlrCuota.Width = 100;
                vlrCuota.OptionsColumn.AllowEdit = true;
                vlrCuota.ColumnEdit = editSpin;
                this.gvDetalle.Columns.Add(vlrCuota);

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "AddGridCols"));
            }

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
                    FormProvider.Master.itemSearch.Enabled = true;
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemSearch.Visible = true;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que revisa que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Eventos grillas

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
            if (fieldName == "PagoFlujoInd")
            {
                if ((bool)e.Value)
                {
                    this.flujosDocu[e.RowHandle].PagoFlujoInd.Value = true;
                    this.flujosDocu[e.RowHandle].Detalle.ForEach(x => x.Aprobado.Value = true);
                }
                else
                {
                    this.flujosDocu[e.RowHandle].PagoFlujoInd.Value = false;
                    this.flujosDocu[e.RowHandle].Detalle.ForEach(x => x.Aprobado.Value = false);
                }

                this.CalcularTotal();
            }
            #endregion
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDetalle_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            #region Generales
            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    this.flujosDocu[this.gvDocument.FocusedRowHandle].Detalle[e.RowHandle].Aprobado.Value = true;
                }
                else
                {
                    this.flujosDocu[this.gvDocument.FocusedRowHandle].Detalle[e.RowHandle].Aprobado.Value = false;
                    this.flujosDocu[this.gvDocument.FocusedRowHandle].PagoFlujoInd.Value = false;
                }
                
                this.CalcularTotal();
            }

            #endregion
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para consultar los datos por el filtro
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                bool update = true;
                if (this.flujosDocu != null && this.flujosDocu.Count > 0)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.UpdateData);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                        update = false;
                }

                if(update)
                {
                    this.txt_VlrTotalFlujo.EditValue = 0;
                    this.pagoFlujos = new DTO_PagoFlujos();

                    int? libranza = null;
                    if (!string.IsNullOrWhiteSpace(this.txtLibranza.Text))
                        libranza = Convert.ToInt32(this.txtLibranza.Text);

                    this.pagoFlujos = this._bc.AdministrationModel.PagoFlujos_GetForPago(this._actFlujo.ID.Value, this.dtFechaFlujo.DateTime, this.txtOferta.Text.Trim(),
                        libranza, this.masterComprador.Value);

                    if (this.pagoFlujos.FlujoCesionDocu.Count > 0)
                    {
                        this.flujosDocu = this.pagoFlujos.FlujoCesionDocu;
                        this.gcDocument.DataSource = this.flujosDocu;
                        this.gvDocument.MoveFirst();
                        this.gvDocument.BestFitColumns();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PagoFlujos_NoPagoFound);
                        MessageBox.Show(msg);
                        //this.CleanData();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                bool hasData = false;

                DTO_PagoFlujos temp = new DTO_PagoFlujos();
                foreach (var docu in this.flujosDocu)
                {
                    List<DTO_ccCreditoDocu> details = docu.Detalle.Where(x => x.Aprobado.Value.Value).ToList();
                    if(details.Count > 0)
                    {
                        hasData = true;
                        docu.Detalle = details;
                        docu.FechaPagoFlujo.Value = this.dtFechaFlujo.DateTime;
                        temp.FlujoCesionDocu.Add(docu);
                    }
                }

                if (hasData)
                {
                    this.pagoFlujos = temp;

                    #region Carga la informacion a FlujoDocu
                    foreach (DTO_ccFlujoCesionDocu flujoDocu in this.pagoFlujos.FlujoCesionDocu)
                    {
                        foreach (DTO_ccCreditoDocu crediDocu in flujoDocu.Detalle)
                        {
                            DTO_ccFlujoCesionDeta flujoDeta = new DTO_ccFlujoCesionDeta();
                            flujoDeta.CreditoCuotaNum.Value = crediDocu.CreditoCuotaNum.Value;
                            flujoDeta.VentaDocNum.Value = crediDocu.DocVenta.Value;
                            flujoDeta.Libranza.Value = crediDocu.Libranza.Value.ToString();
                            flujoDeta.Oferta.Value = flujoDocu.Oferta.Value;
                            flujoDeta.Valor.Value = crediDocu.VlrCuota.Value;
                            flujoDeta.VlrCapitalCesion.Value = crediDocu.VlrCapital.Value;
                            flujoDeta.VlrUtilidadCesion.Value = crediDocu.VlrUtilidad.Value;
                            flujoDeta.VlrDerechosCesion.Value = crediDocu.VlrSaldo.Value;
                            flujoDeta.Inversionista.Value = flujoDocu.Inversionista.Value;
                            flujoDeta.NumDocCedito.Value = crediDocu.NumeroDoc.Value;
                            pagoFlujos.FlujoCesionDeta.Add(flujoDeta);
                        }
                    }
                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PagoFlujos_NoPagoSelected);
                    MessageBox.Show(msg);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "TBSave"));
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
                results = _bc.AdministrationModel.PagoFlujos_Add(this.documentID, this._actFlujo.ID.Value, this.masterComprador.Value, this.dtFechaFlujo.DateTime.Date, this.pagoFlujos);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                this.isValid = true;
                MessageForm frm = null;
                if (results.Count == 1 && results.First().GetType() == typeof(DTO_TxResult))
                {
                    frm = new MessageForm((DTO_TxResult)results[0]);
                    this.isValid = false;
                }
                else
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

                        if (this.flujosDocu[i].Detalle.Any(x => x.Aprobado.Value.Value))
                        {
                            bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.PagoFlujos, this._actFlujo.seUsuarioID.Value, obj, false);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoFlujos.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }

}
