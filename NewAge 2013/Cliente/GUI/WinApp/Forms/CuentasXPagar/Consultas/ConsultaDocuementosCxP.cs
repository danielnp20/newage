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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaDocumentosCxP : QueryForm
    {
        #region Variables
        private List<DTO_QueryFacturas> _data = null;
        private List<DTO_QueryFacturas> _dataCopy = null;   
        private DTO_QueryFacturasDetail _detalle;
        private string _periodoDefult;
        Dictionary<int, string> tipoFac = new Dictionary<int, string>();
        Dictionary<int, string> tipoConsulta = new Dictionary<int, string>();

        #endregion

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public ConsultaDocumentosCxP()
        {
           // InitializeComponent();
        }

        #region Funciones Protected

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas de grilla principal

                //Numero Factura
                GridColumn numFac = new GridColumn();
                numFac.FieldName = this._unboundPrefix + "numFac";
                numFac.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NumFactura");
                numFac.UnboundType = UnboundColumnType.String;
                numFac.VisibleIndex = 2;
                numFac.Width = 90;
                numFac.Visible = true;
                numFac.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numFac);

                //C.C. Tercero
                GridColumn numIdentifica = new GridColumn();
                numIdentifica.FieldName = this._unboundPrefix + "numIdentifica";
                numIdentifica.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_numIdentifica");
                numIdentifica.UnboundType = UnboundColumnType.String;
                numIdentifica.VisibleIndex = 3;
                numIdentifica.Width = 70;
                numIdentifica.Visible = true;
                numIdentifica.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numIdentifica);

                //Nombre Tercero
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 4;
                Descriptivo.Width = 250;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descriptivo);

                //Fecha Factura
                GridColumn FacturaFecha = new GridColumn();
                FacturaFecha.FieldName = this._unboundPrefix + "FacturaFecha";
                FacturaFecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FacturaFecha");
                FacturaFecha.UnboundType = UnboundColumnType.DateTime;
                FacturaFecha.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                FacturaFecha.AppearanceCell.Options.UseTextOptions = true;
                FacturaFecha.VisibleIndex = 5;
                FacturaFecha.Width = 100;
                FacturaFecha.Visible = true;
                FacturaFecha.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FacturaFecha);

                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this._unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.DateTime;
                Observacion.VisibleIndex = 6;
                Observacion.Width = 285;
                Observacion.Visible = true;
                Observacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Observacion);

                //Moneda
                GridColumn MonedaID = new GridColumn();
                MonedaID.FieldName = this._unboundPrefix + "MonedaID";
                MonedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MonedaID");
                MonedaID.UnboundType = UnboundColumnType.DateTime;
                MonedaID.VisibleIndex = 7;
                MonedaID.Width = 50;
                MonedaID.Visible = true;
                MonedaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MonedaID);

                //Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this._unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor.AppearanceCell.Options.UseTextOptions = true;
                Valor.VisibleIndex = 9;
                Valor.Width = 100;
                Valor.Visible = true;
                Valor.ColumnEdit = this.TextEdit;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                //Iva
                GridColumn iva = new GridColumn();
                iva.FieldName = this._unboundPrefix + "Iva";
                iva.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Iva");
                iva.UnboundType = UnboundColumnType.Decimal;
                iva.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                iva.AppearanceCell.Options.UseTextOptions = true;
                iva.VisibleIndex = 10;
                iva.Width = 100;
                iva.Visible = true;
                iva.ColumnEdit = this.TextEdit;
                iva.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(iva);

                // Saldo Mda Loc
                GridColumn SaldoLoc = new GridColumn();
                SaldoLoc.FieldName = this._unboundPrefix + "SaldoLoc";
                SaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoLoc");
                SaldoLoc.UnboundType = UnboundColumnType.Decimal;
                SaldoLoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoLoc.AppearanceCell.Options.UseTextOptions = true;
                SaldoLoc.VisibleIndex = 11;
                SaldoLoc.Width = 100;
                SaldoLoc.Visible = true;
                SaldoLoc.ColumnEdit = this.TextEdit;
                SaldoLoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoLoc); 

                #endregion

                #region Grilla Interna de Detalle

                //DocumentoID
                GridColumn DocumentoID = new GridColumn();
                DocumentoID.FieldName = this._unboundPrefix + "DocumentoID";
                DocumentoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                DocumentoID.UnboundType = UnboundColumnType.Integer;
                DocumentoID.VisibleIndex = 0;
                DocumentoID.Width = 60;
                DocumentoID.Visible = true;
                DocumentoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DocumentoID);

                // DocumentoDesc
                GridColumn DocumentoDesc = new GridColumn();
                DocumentoDesc.FieldName = this._unboundPrefix + "DocumentoDesc";
                DocumentoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoDesc");
                DocumentoDesc.UnboundType = UnboundColumnType.String;
                DocumentoDesc.VisibleIndex = 1;
                DocumentoDesc.Width = 100;
                DocumentoDesc.Visible = true;
                DocumentoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DocumentoDesc);

                // Banco
                GridColumn banco = new GridColumn();
                banco.FieldName = this._unboundPrefix + "Banco";
                banco.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Banco");
                banco.UnboundType = UnboundColumnType.String;
                banco.VisibleIndex = 2;
                banco.Width = 100;
                banco.Visible = true;
                banco.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(banco);

                //DocumentoTercero
                GridColumn DocumentoTercero = new GridColumn();
                DocumentoTercero.FieldName = this._unboundPrefix + "DocumentoTercero";
                DocumentoTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoNumero");
                DocumentoTercero.UnboundType = UnboundColumnType.DateTime;
                DocumentoTercero.VisibleIndex = 3;
                DocumentoTercero.Width = 100;
                DocumentoTercero.Visible = true;
                DocumentoTercero.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DocumentoTercero);

                //fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaMovimiento");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 4;
                fecha.Width = 100;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(fecha);

                //ComprobanteID
                GridColumn ComprobanteID = new GridColumn();
                ComprobanteID.FieldName = this._unboundPrefix + "ComprobanteID";
                ComprobanteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComprobanteID");
                ComprobanteID.UnboundType = UnboundColumnType.Decimal;
                ComprobanteID.VisibleIndex = 5;
                ComprobanteID.Width = 100;
                ComprobanteID.Visible = true;
                ComprobanteID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ComprobanteID);

                //comprobanteNro
                GridColumn comprobanteNro = new GridColumn();
                comprobanteNro.FieldName = this._unboundPrefix + "ComprobanteNro";
                comprobanteNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComprobanteNro");
                comprobanteNro.UnboundType = UnboundColumnType.String;
                comprobanteNro.VisibleIndex = 6;
                comprobanteNro.Width = 100;
                comprobanteNro.Visible = true;
                comprobanteNro.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(comprobanteNro);

                //Valor
                GridColumn vlrMdaLoc = new GridColumn();
                vlrMdaLoc.FieldName = this._unboundPrefix + "vlrMdaLoc";
                vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
                vlrMdaLoc.VisibleIndex = 7;
                vlrMdaLoc.Width = 100;
                vlrMdaLoc.Visible = true;
                vlrMdaLoc.ColumnEdit = this.TextEdit;
                vlrMdaLoc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(vlrMdaLoc);

                //Ver Documento
                GridColumn file = new GridColumn();
                file.FieldName = this._unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SearchDocument");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 100;
                file.VisibleIndex = 8;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this._documentID = AppQueries.QueryDocumentosCxP;
            this._frmModule = ModulesPrefix.cp;
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        protected override void InitControls()
        {
            try
            {
                //Inicia los controles Master Find
                this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
                this._bc.InitMasterUC(this.masterConceptoCxP, AppMasters.cpConceptoCXP, true, true, true, false);
                this._periodoDefult = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_Periodo);
                this.uc_Periodo.DateTime = Convert.ToDateTime(this._periodoDefult);

                //Carga el Compo de tipo de Facturas
                this.tipoFac.Add(0, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_NoAplica));
                this.tipoFac.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_FacturasxPagar));
                this.tipoFac.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_FacturasPagadas));
                this.tipoFac.Add(3, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Todos));
                this.lkpTipoFactura.Properties.DataSource = this.tipoFac;
                this.lkpTipoFactura.EditValue = 3;

                this.gbHeader.Size = new System.Drawing.Size(1099, 100);
                this.masterTercero.EnableControl(false);
                this.masterConceptoCxP.EnableControl(false);
                this.uc_Periodo.Enabled = false;
                this.lkpTipoFactura.Enabled = false;
                this.txtNroFactura.Enabled = false;

                //Carga el Combo para ver que se desea consultar
                this.tipoConsulta.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Todos));
                this.tipoConsulta.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Factura));
                this.tipoConsulta.Add(3, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_NotaCredito));
                this.lkp_TipoConsulta.Properties.DataSource = this.tipoConsulta;
                this.lkp_TipoConsulta.EditValue = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsltaDocumentosCxP.cs", "Initontrols"));
            }

        }

        /// <summary>
        /// Funcion que se encarga de limpiar los controles
        /// </summary>
        protected override void CleanData(bool cleanAll)
        {
            if (!cleanAll)
            {
                this.masterTercero.Value = string.Empty;
                this.masterConceptoCxP.Value = string.Empty;
                this.txtNroFactura.Text = string.Empty;
                this.lkpTipoFactura.Properties.DataSource = this.tipoFac;
                this.gcDocument.DataSource = null;
                this._data = new List<DTO_QueryFacturas>();
                this._dataCopy = new List<DTO_QueryFacturas>();
            }
            else
            {
                this.lkp_TipoConsulta.Properties.DataSource = this.tipoConsulta;
                this.masterTercero.Value = string.Empty;
                this.masterConceptoCxP.Value = string.Empty;
                this.txtNroFactura.Text = string.Empty;
                this.lkpTipoFactura.Properties.DataSource = this.tipoFac;
                this.gcDocument.DataSource = null;
                this._data = new List<DTO_QueryFacturas>();
                this._dataCopy = new List<DTO_QueryFacturas>();
            }    
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que carga el data source de las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Consultar_Click(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que habilita los controles para realizar la consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_TipoConsulta_EditValueChanged(object sender, EventArgs e)
        {
            switch ((int)this.lkp_TipoConsulta.EditValue)
            {
                case 0:
                    this.CleanData(false);
                    this.masterTercero.EnableControl(true);
                    this.masterConceptoCxP.EnableControl(true);
                    this.uc_Periodo.Enabled = true;
                    this.lkpTipoFactura.Enabled = false;
                    this.txtNroFactura.Enabled = true;
                    break;
                case 1:
                    this.CleanData(false);
                    this.masterTercero.EnableControl(true);
                    this.masterConceptoCxP.EnableControl(true);
                    this.uc_Periodo.Enabled = true;
                    this.lkpTipoFactura.Enabled = true;
                    this.txtNroFactura.Enabled = true;
                    break;
                case 2:
                    this.CleanData(false);
                    this.masterTercero.EnableControl(true);
                    this.masterConceptoCxP.EnableControl(true);
                    this.uc_Periodo.Enabled = true;
                    this.lkpTipoFactura.Enabled = true;
                    this.txtNroFactura.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Evento que habilita los controles para realizar la consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_TipoFactura_EditValueChanged(object sender, EventArgs e)
        {
            if (this._data != null && this._dataCopy != null && this.gvDocument.DataRowCount > 0)
            {
                switch ((int)this.lkpTipoFactura.EditValue)
                {                    
                    case 1: // No Pagadas
                        this._data = this._dataCopy.FindAll(x => x.SaldoLoc.Value != 0);
                        this.gcDocument.DataSource = this._data;
                        this.gcDocument.RefreshDataSource();
                        break;
                    case 2: //Pagadas
                        this._data = this._dataCopy.FindAll(x => x.SaldoLoc.Value == 0);
                        this.gcDocument.DataSource = this._data;
                        this.gcDocument.RefreshDataSource();
                        break;
                    default:
                        this._data = this._dataCopy;
                        this.gcDocument.DataSource = this._data;
                        this.gcDocument.RefreshDataSource();
                        break;
                } 
            }
        }

        #endregion

        #region Eventos Grillas

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
                if (e.FocusedRowHandle >= 0 && this._data.Count > 0)
                {
                    int filaDoc = this.gvDocument.FocusedRowHandle;
                    this._detalle = this._data[filaDoc].Detalle[e.FocusedRowHandle];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-gvDetalle_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetalle_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
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
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._detalle.NumeroDoc.Value.Value);
                comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, this._detalle.ComprobanteID.Value, this._detalle.ComprobanteNro.Value.Value, null, null, null);

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-linkEditViewFile_Click"));
            }
        }

        #endregion

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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Form_Enter"));
            }
        }
        #endregion

        #region Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._data = new List<DTO_QueryFacturas>();
                this._detalle = new DTO_QueryFacturasDetail();
                this.CleanData(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.uc_Periodo.DateTime.ToString()))
                {
                    if ((int)this.lkp_TipoConsulta.EditValue != 1)
                    {
                        this.gcDocument.DataSource = null;
                        this._data = new List<DTO_QueryFacturas>();
                        this._data = this._bc.AdministrationModel.ConsultarFacturas(this.uc_Periodo.DateTime, this.masterTercero.Value, this.masterConceptoCxP.Value,this.txtNroFactura.Text, (int)this.lkp_TipoConsulta.EditValue, null);
                        if ((int)this.lkpTipoFactura.EditValue == 1) // Filtra las No Pagadas
                            this._data = this._data.FindAll(x => x.SaldoLoc.Value != 0);
                        else if ((int)this.lkpTipoFactura.EditValue == 2) // Filtras las Pagadas
                            this._data = this._data.FindAll(x => x.SaldoLoc.Value == 0);
                        this.gcDocument.DataSource = this._data;
                        this._dataCopy = ObjectCopier.Clone(this._data);
                    }
                    else
                    {                           
                        this.gcDocument.DataSource = null;
                        this._data = new List<DTO_QueryFacturas>();
                        this._data = this._bc.AdministrationModel.ConsultarFacturas(this.uc_Periodo.DateTime, this.masterTercero.Value,this.masterConceptoCxP.Value, this.txtNroFactura.Text, (int)this.lkp_TipoConsulta.EditValue, (int)this.lkpTipoFactura.EditValue);
                        if ((int)this.lkpTipoFactura.EditValue == 1) // Filtra las No Pagadas
                            this._data = this._data.FindAll(x => x.SaldoLoc.Value != 0);
                        else if ((int)this.lkpTipoFactura.EditValue == 2) // Filtras las Pagadas
                            this._data = this._data.FindAll(x => x.SaldoLoc.Value == 0);
                        this.gcDocument.DataSource = this._data;
                        this._dataCopy = ObjectCopier.Clone(this._data);                       
                    }
                }
                else
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_EmptyYear), this.txtNroFactura.Text);
                    MessageBox.Show(msg);
                    this.uc_Periodo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-btn_Consultar_Click"));
            }
        }

        #endregion

    }
}
