using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors.Controls;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaDocumentosFact : QueryForm
    {
        public ConsultaDocumentosFact()
        {
            //InitializeComponent();
        }

        #region Variables
        private DTO_QueryDetailFactura _rowDetalleFact = null;
        private DTO_QueryHeadFactura _rowFactura = null;
        private List<DTO_QueryHeadFactura> _data = null;        
        private Dictionary<int, string> _tipoFac = new Dictionary<int, string>();
        private Dictionary<int, string> _tipoConsulta = new Dictionary<int, string>();
        private GridView _gridDetalleCurrent = null;
        private string _tercero = string.Empty;
        private string _periodoDefult = string.Empty;

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        protected override void AddGridCols()
        {
            base.AddGridCols();
            try
            {
                #region Columnas de grilla principal

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 2;
                TerceroID.Width = 100;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 3;
                Nombre.Width = 120;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Nombre);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms,  "Proyecto");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 3;
                ProyectoID.Width = 90;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

                //MdaPago
                GridColumn MdaPago = new GridColumn();
                MdaPago.FieldName = this._unboundPrefix + "MdaPago";
                MdaPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MdaPago");
                MdaPago.UnboundType = UnboundColumnType.String;
                MdaPago.VisibleIndex = 4;
                MdaPago.Width = 80;
                MdaPago.Visible = true;
                MdaPago.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MdaPago);

                //ValorBruto
                GridColumn ValorBruto = new GridColumn();
                ValorBruto.FieldName = this._unboundPrefix + "ValorBruto";
                ValorBruto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorBruto");
                ValorBruto.UnboundType = UnboundColumnType.Decimal;
                ValorBruto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorBruto.AppearanceCell.Options.UseTextOptions = true;
                ValorBruto.VisibleIndex = 5;
                ValorBruto.Width = 90;
                ValorBruto.Visible = true;
                ValorBruto.ColumnEdit = this.TextEdit;
                ValorBruto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorBruto);

                //IVA
                GridColumn IVA = new GridColumn();
                IVA.FieldName = this._unboundPrefix + "IVA";
                IVA.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_IVA");
                IVA.UnboundType = UnboundColumnType.Decimal;
                IVA.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                IVA.AppearanceCell.Options.UseTextOptions = true;
                IVA.VisibleIndex = 6;
                IVA.Width = 80;
                IVA.Visible = true;
                IVA.ColumnEdit = this.TextEdit;
                IVA.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(IVA);

                //SaldoLoc
                GridColumn SaldoLoc = new GridColumn();
                SaldoLoc.FieldName = this._unboundPrefix + "SaldoLoc";
                SaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoLoc");
                SaldoLoc.UnboundType = UnboundColumnType.Decimal;
                SaldoLoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoLoc.AppearanceCell.Options.UseTextOptions = true;
                SaldoLoc.VisibleIndex = 7;
                SaldoLoc.Width = 90;
                SaldoLoc.Visible = true;
                SaldoLoc.ColumnEdit = this.TextEdit;
                SaldoLoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoLoc);

                // Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this._unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor.AppearanceCell.Options.UseTextOptions = true;
                Valor.VisibleIndex = 8;
                Valor.Width = 100;
                Valor.Visible = true;
                Valor.ColumnEdit = this.TextEdit;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this._unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 9;
                Observacion.Width = 120;
                Observacion.Visible = true;
                Observacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Observacion);

                //FacturaFijaInd
                GridColumn FacturaFijaInd = new GridColumn();
                FacturaFijaInd.FieldName = this._unboundPrefix + "FacturaFijaInd";
                FacturaFijaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FacturaFijaInd");
                FacturaFijaInd.UnboundType = UnboundColumnType.Boolean;
                FacturaFijaInd.VisibleIndex = 10;
                FacturaFijaInd.Width = 70;
                FacturaFijaInd.Visible = true;
                FacturaFijaInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(FacturaFijaInd);

                #endregion

                #region Grilla Interna de Detalle

                // Banco
                GridColumn banco = new GridColumn();
                banco.FieldName = this._unboundPrefix + "Banco";
                banco.Caption = _bc.GetResource(LanguageTypes.Forms, "Banco");
                banco.UnboundType = UnboundColumnType.String;
                banco.VisibleIndex = 1;
                banco.Width = 100;
                banco.Visible = true;
                banco.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(banco);

                //fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, "Fecha Movimiento");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 3;
                fecha.Width = 80;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(fecha);

                //ComprobanteID
                GridColumn ComprobanteID = new GridColumn();
                ComprobanteID.FieldName = this._unboundPrefix + "ComprobanteID";
                ComprobanteID.Caption = _bc.GetResource(LanguageTypes.Forms, "Comprobante");
                ComprobanteID.UnboundType = UnboundColumnType.Decimal;
                ComprobanteID.VisibleIndex = 4;
                ComprobanteID.Width = 100;
                ComprobanteID.Visible = true;
                ComprobanteID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ComprobanteID);

                //comprobanteNro
                GridColumn comprobanteNro = new GridColumn();
                comprobanteNro.FieldName = this._unboundPrefix + "ComprobanteNro";
                comprobanteNro.Caption = _bc.GetResource(LanguageTypes.Forms, "Comprobante Nro");
                comprobanteNro.UnboundType = UnboundColumnType.String;
                comprobanteNro.VisibleIndex = 5;
                comprobanteNro.Width = 70;
                comprobanteNro.Visible = true;
                comprobanteNro.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(comprobanteNro);

                //Valor
                GridColumn vlrMdaLoc = new GridColumn();
                vlrMdaLoc.FieldName = this._unboundPrefix + "vlrMdaLoc";
                vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms,"Valor");
                vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
                vlrMdaLoc.VisibleIndex = 6;
                vlrMdaLoc.Width = 100;
                vlrMdaLoc.Visible = true;
                vlrMdaLoc.ColumnEdit = this.TextEdit;
                vlrMdaLoc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(vlrMdaLoc);

                //Ver Documento
                GridColumn file = new GridColumn();
                file.FieldName = this._unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, "Ver Documento");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 100;
                file.VisibleIndex = 6;
                file.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                file.Visible = true;
                file.ColumnEdit = this.linkEditViewFile;
                file.OptionsColumn.AllowEdit = true;
                file.AppearanceCell.ForeColor = Color.Blue;
                this.gvDetalle.Columns.Add(file);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this._documentID = AppQueries.QueryDocumentosFacturas;
            this._frmModule = ModulesPrefix.fa;
            this.saveDelegate = new Save(this.SaveMethod);
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        protected override void InitControls()
        {
            try
            {
                //Inicia los controles maestras
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
                this._bc.InitMasterUC(this.masterAsesor, AppMasters.faAsesor, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
                this._periodoDefult = _bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo);
                this.uc_Periodo.DateTime = Convert.ToDateTime(this._periodoDefult);

                //Carga el Combo para ver que se desea consultar
                this._tipoConsulta.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Todos));
                this._tipoConsulta.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_FacturaVenta));
                this._tipoConsulta.Add(3, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_NotaCredito));
                this.lkp_TipoConsulta.Properties.DataSource = this._tipoConsulta;
                this.lkp_TipoConsulta.EditValue = 1;

                //Carga el Compo de tipo de Facturas
                this._tipoFac.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Facturas por Cobrar"));
                this._tipoFac.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Facturas Cobradas"));
                this._tipoFac.Add(3, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Todos));
                this.lkp_TipoFactura.Properties.DataSource = this._tipoFac;
                this.lkp_TipoFactura.EditValue = 3;

                this.masterCliente.EnableControl(false);
                this.uc_Periodo.Enabled = false;
                this.gvDocument.OptionsView.ShowAutoFilterRow = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Initontrols"));
            }
        }

        /// <summary>
        /// Funcion que se encarga de limpiar los controles
        /// </summary>
        protected override void CleanData(bool cleanAll)
        {
            if (!cleanAll)
            {
                this.masterCliente.Value = string.Empty;
                this.lkp_TipoFactura.Properties.DataSource =  this._tipoFac;
                this.gcDocument.DataSource = new object();
                this._tercero = string.Empty;
            }
            else 
            {
                this.lkp_TipoConsulta.Properties.DataSource = this._tipoConsulta;
                this.lkp_TipoFactura.Properties.DataSource = this._tipoFac;  
                this.masterCliente.Value = string.Empty;
                this.masterPrefijo.Value = string.Empty;
                this.txtFacturaVtaNro.Text = string.Empty;
                this.masterAsesor.Value = string.Empty;
                this.masterZona.Value = string.Empty;
                this.masterProyecto.Value = string.Empty;
                this.gcDocument.DataSource = null;
                this._tercero = string.Empty;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            if (this.masterCliente.ValidID)
            {
                DTO_faCliente cliente = (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                this._tercero = cliente.TerceroID.Value;
            }
        }

        /// <summary>
        /// Evento que habilita los controles para realizar la consulta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lkp_TipoConsulta_EditValueChanged(object sender, EventArgs e)
        {
            switch ((int)this.lkp_TipoConsulta.EditValue)
            {
                case 1:
                    this.CleanData(false);
                    this.masterCliente.EnableControl(true);
                    this.uc_Periodo.Enabled = true;
                    break;
                case 2:
                    this.CleanData(false);
                    this.masterCliente.EnableControl(true);
                    this.uc_Periodo.Enabled = true;
                    break;
                case 3:
                    this.CleanData(false);
                    this.masterCliente.EnableControl(true);
                    this.uc_Periodo.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Evento que habilita los controles para realizar la consulta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lkp_TipoFactura_EditValueChanged(object sender, EventArgs e)
        {
            switch ((int)this.lkp_TipoFactura.EditValue)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Permite saber la vista actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gcDocument_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)
        {
            try
            {
                this._gridDetalleCurrent = (GridView)e.View;
                if (this._gridDetalleCurrent != null && this._gridDetalleCurrent.FocusedRowHandle >= 0 && this._gridDetalleCurrent.DataRowCount > 0)
                {
                    var row = this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                    if (row.GetType() == typeof(DTO_QueryHeadFactura))
                        this._rowFactura = (DTO_QueryHeadFactura)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                    else if (row.GetType() == typeof(DTO_QueryDetailFactura))
                        this._rowDetalleFact = (DTO_QueryDetailFactura)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gcProyectos_FocusedViewChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this.gvDocument.DataRowCount > 0)
                {
                    this._rowFactura = (DTO_QueryHeadFactura)this.gvDocument.GetRow(e.FocusedRowHandle);
                }
                else
                    this._rowFactura = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-gvDocument_FocusedRowChanged"));
            }
        }


        #region Detail

        /// <summary>
        /// Evento que se ejecuta al cambiar de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDetalle_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.FocusedRowHandle >= 0 && view.DataRowCount > 0)
                    this._rowDetalleFact = (DTO_QueryDetailFactura)view.GetRow(e.FocusedRowHandle);
                else
                    this._rowDetalleFact = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-gvDetalle_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetalle_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = e.Column.Caption;
        }

        /// <summary>
        /// Evento que llama la funcionalidad de buscar documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void linkEditViewFile_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;

                if (this._rowDetalleFact != null)
                {
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();

                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._rowDetalleFact.NumeroDoc.Value.Value);
                    comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, this._rowDetalleFact.ComprobanteID.Value, this._rowDetalleFact.ComprobanteNro.Value.Value, null, null, null);

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-linkEditViewFile_Click"));
            }
        }

        #endregion
        #endregion

        #region Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            { 
                this._data = new List<DTO_QueryHeadFactura>();
                this._rowDetalleFact = new DTO_QueryDetailFactura();
                this.CleanData(true);
                this.lkp_TipoConsulta.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();

                this.gvDocument.ActiveFilterString = string.Empty;
                if (this._data != null && this._data.Count > 0)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                FormProvider.Master.Focus();
                if (!string.IsNullOrWhiteSpace(this.uc_Periodo.DateTime.ToString()))
                {
                   // if (this.masterCliente.ValidID)
                    {
                        if (string.IsNullOrEmpty(this._tercero) || this._tercero != this.masterCliente.Value)
                            this.uc_Periodo.Focus();
                        if (this.lkp_TipoConsulta.EditValue != null && (int)this.lkp_TipoConsulta.EditValue != 0)
                        {
                            this.gcDocument.DataSource = null;
                            this._data = new List<DTO_QueryHeadFactura>();
                            //DateTime año, string terceroId, int tipoConsulta, string Asesor, string Zona, string Proyecto,int TipoFact
                            int tipo = 0;
                            if (this.lkp_TipoFactura.EditValue == null)
                                tipo = -1;
                            else
                                tipo = (int)this.lkp_TipoFactura.EditValue;
                            this._data = this._bc.AdministrationModel.ConsultarFacturasXNota((DateTime)this.uc_Periodo.DateTime, this._tercero, (int)this.lkp_TipoConsulta.EditValue,
                                                                                            this.masterAsesor.Value, this.masterZona.Value, this.masterProyecto.Value, tipo, this.txtFacturaVtaNro.Text, 
                                                                                            this.masterPrefijo.Value, this.chkFacturaFija.Checked);
                            this.gcDocument.DataSource = this._data;
                        }
                        else
                        {
                            if (this.lkp_TipoFactura.EditValue != null)
                            {
                                this.gcDocument.DataSource = null;
                                this._data = new List<DTO_QueryHeadFactura>();
                                this._data = this._bc.AdministrationModel.ConsultarFacturasXNota(this.uc_Periodo.DateTime, this._tercero, (int)this.lkp_TipoConsulta.EditValue, this.masterAsesor.Value, this.masterZona.Value, this.masterProyecto.Value, (int)this.lkp_TipoFactura.EditValue, txtFacturaVtaNro.Text, this.masterPrefijo.Value, this.chkFacturaFija.Checked);
                                this.gcDocument.DataSource = this._data;
                            }
                            else
                            {
                                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lbl_tipoFactura.Text);
                                MessageBox.Show(msg);
                                this.lkp_TipoFactura.Focus();
                            }
                        }
                    }
                    //else
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterCliente.LabelRsx);
                    //    MessageBox.Show(msg);
                    //    this.masterCliente.Focus();
                    //}
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
        /// Hilo de salvar
        /// </summary>
        public virtual void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                List<DTO_faFacturaDocu> listDocs = new List<DTO_faFacturaDocu>();
                foreach (DTO_QueryHeadFactura item in this._data)
	            {
                    DTO_faFacturaDocu factura = new DTO_faFacturaDocu();
                    factura.NumeroDoc.Value = item.NumeroDoc.Value;
                    factura.FacturaFijaInd.Value = item.FacturaFijaInd.Value;
                    listDocs.Add(factura);
	            }
                result = _bc.AdministrationModel.FacturaDocu_Upd(this._documentID, listDocs, true);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (result.Result.Equals(ResultValue.OK))
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this._documentID.ToString(), _bc.AdministrationModel.User);

                    this._data = new List<DTO_QueryHeadFactura>();
                    this._rowDetalleFact = new DTO_QueryDetailFactura();
                    this.Invoke(this.saveDelegate);
                }
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
