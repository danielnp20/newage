using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Aprobacion de causaciones
    /// </summary>
    public partial class AprobacionCausacionFact : DocumentAprobForm
    {
        //public AprobacionCausacionFact()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_CausacionAprobacion> _docs = null;
        #endregion

        #region Funciones Virtuales del DocumentAprobForm

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.currentDoc = null;
                this._docs = _bc.AdministrationModel.CuentasXPagar_GetPendientesByModulo(ModulesPrefix.cp, this.actividadFlujoID);
                if (this.masterTercero.ValidID && this._docs.Count > 0)
                    this._docs = this._docs.FindAll(x => x.TerceroID.Value == this.masterTercero.Value);
                foreach (var item in this._docs)
                    item.FileUrl = string.Empty;

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this._docs.Count > 0)
                {
                    this.detailsLoaded = false;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = true;

                    if (!detailsLoaded)
                    {
                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                        this.LoadDetails();
                    }

                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this.gcDetails.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected override void LoadDetails()
        {
            try
            {
                DTO_CausacionAprobacion doc = (DTO_CausacionAprobacion)this.currentDoc;
                DateTime periodo = doc.PeriodoID.Value.Value;
                string compID = doc.ComprobanteID.Value;
                int compNro = doc.ComprobanteNro.Value.Value;

                DTO_Comprobante c = _bc.AdministrationModel.Comprobante_Get(false, true, periodo, compID, compNro, this.numDetails, 1);
                List<DTO_ComprobanteFooter> details = c != null ? c.Footer : new List<DTO_ComprobanteFooter>();
                this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        protected override void AddDetailCols()
        {
            //Cuenta
            GridColumn cuenta = new GridColumn();
            cuenta.FieldName = this.unboundPrefix + "CuentaID";
            cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
            cuenta.UnboundType = UnboundColumnType.String;
            cuenta.VisibleIndex = 0;
            cuenta.Width = 70;
            cuenta.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(cuenta);

            //Tercero
            GridColumn tercero = new GridColumn();
            tercero.FieldName = this.unboundPrefix + "TerceroID";
            tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
            tercero.UnboundType = UnboundColumnType.String;
            tercero.VisibleIndex = 1;
            tercero.Width = 70;
            tercero.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(tercero);

            //Prefijo
            GridColumn pref = new GridColumn();
            pref.FieldName = this.unboundPrefix + "PrefijoCOM";
            pref.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoCOM");
            pref.UnboundType = UnboundColumnType.String;
            pref.VisibleIndex = 2;
            pref.Width = 70;
            pref.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(pref);

            //Documento
            GridColumn doc = new GridColumn();
            doc.FieldName = this.unboundPrefix + "DocumentoCOM";
            doc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
            doc.UnboundType = UnboundColumnType.String;
            doc.VisibleIndex = 3;
            doc.Width = 70;
            doc.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(doc);

            //Activo
            GridColumn act = new GridColumn();
            act.FieldName = this.unboundPrefix + "ActivoCOM";
            act.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoCOM");
            act.UnboundType = UnboundColumnType.String;
            act.VisibleIndex = 4;
            act.Width = 70;
            act.Visible = true;
            act.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(act);

            //Proyecto
            GridColumn proyecto = new GridColumn();
            proyecto.FieldName = this.unboundPrefix + "ProyectoID";
            proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            proyecto.UnboundType = UnboundColumnType.String;
            proyecto.VisibleIndex = 5;
            proyecto.Width = 70;
            proyecto.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(proyecto);

            //Centro de costo
            GridColumn ctoCosto = new GridColumn();
            ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
            ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            ctoCosto.UnboundType = UnboundColumnType.String;
            ctoCosto.VisibleIndex = 6;
            ctoCosto.Width = 70;
            ctoCosto.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(ctoCosto);

            //Linea Presupuestal
            GridColumn linPresup = new GridColumn();
            linPresup.FieldName = this.unboundPrefix + "LineaPresupuestoID";
            linPresup.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
            linPresup.UnboundType = UnboundColumnType.String;
            linPresup.VisibleIndex = 7;
            linPresup.Width = 70;
            linPresup.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(linPresup);

            //Concepto cargo
            GridColumn concCargo = new GridColumn();
            concCargo.FieldName = this.unboundPrefix + "ConceptoCargoID";
            concCargo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoCargoID");
            concCargo.UnboundType = UnboundColumnType.String;
            concCargo.VisibleIndex = 8;
            concCargo.Width = 70;
            concCargo.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(concCargo);

            //Lugar Geografico
            GridColumn lg = new GridColumn();
            lg.FieldName = this.unboundPrefix + "LugarGeograficoID";
            lg.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LugarGeograficoID");
            lg.UnboundType = UnboundColumnType.String;
            lg.VisibleIndex = 9;
            lg.Width = 70;
            lg.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(lg);

            //Valor Base ML
            GridColumn vlrBaseML = new GridColumn();
            vlrBaseML.FieldName = this.unboundPrefix + "vlrBaseML";
            vlrBaseML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseML");
            vlrBaseML.UnboundType = UnboundColumnType.Decimal;
            vlrBaseML.VisibleIndex = 10;
            vlrBaseML.Width = 120;
            vlrBaseML.Visible = true;
            vlrBaseML.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(vlrBaseML);

            //Valor Moneda local
            GridColumn vlrMdaLoc = new GridColumn();
            vlrMdaLoc.FieldName = this.unboundPrefix + "vlrMdaLoc";
            vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
            vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
            vlrMdaLoc.VisibleIndex = 11;
            vlrMdaLoc.Width = 150;
            vlrMdaLoc.Visible = true;
            vlrMdaLoc.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(vlrMdaLoc);

            if (this.multiMoneda)
            {
                //Valor Base ME
                GridColumn vlrBaseME = new GridColumn();
                vlrBaseME.FieldName = this.unboundPrefix + "vlrBaseME";
                vlrBaseME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseME");
                vlrBaseME.UnboundType = UnboundColumnType.Decimal;
                vlrBaseME.VisibleIndex = 13;
                vlrBaseME.Width = 120;
                vlrBaseME.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(vlrBaseME);

                //Valor Moneda Ext
                GridColumn vlrMdaExt = new GridColumn();
                vlrMdaExt.FieldName = this.unboundPrefix + "vlrMdaExt";
                vlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                vlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                vlrMdaExt.VisibleIndex = 14;
                vlrMdaExt.Width = 150;
                vlrMdaExt.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(vlrMdaExt);
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.CausacionFacturasAprob;
            this.frmModule = ModulesPrefix.cp;

            base.SetInitParameters();
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            //this.gvDocuments.OptionsView.ShowAutoFilterRow = true;
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
                aprob.Width = 5;
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
                noAprob.Width = 5;
                noAprob.Visible = true;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Documento Tercero
                GridColumn DocTer = new GridColumn();
                DocTer.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocTer.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoTercero");
                DocTer.UnboundType = UnboundColumnType.String;
                DocTer.VisibleIndex = 3;
                DocTer.Width = 30;
                DocTer.Visible = true;
                DocTer.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(DocTer);

                //FacturaFecha
                GridColumn factFecha = new GridColumn();
                factFecha.FieldName = this.unboundPrefix + "FacturaFecha";
                factFecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PeriodoID");
                factFecha.UnboundType = UnboundColumnType.DateTime;
                factFecha.VisibleIndex = 4;
                factFecha.Width = 30;
                factFecha.Visible = true;
                factFecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(factFecha);

                //Descripcion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 5;
                Descripcion.Width = 50;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Descripcion);

                //TerceroID
                GridColumn ter = new GridColumn();
                ter.FieldName = this.unboundPrefix + "TerceroID";
                ter.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                ter.UnboundType = UnboundColumnType.String;
                ter.VisibleIndex = 6;
                ter.Width = 40;
                ter.Visible = true;
                ter.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(ter);

                //TerceroDesc
                GridColumn terDesc = new GridColumn();
                terDesc.FieldName = this.unboundPrefix + "DescriptivoTercero";
                terDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_terDesc");
                terDesc.UnboundType = UnboundColumnType.String;
                terDesc.VisibleIndex = 7;
                terDesc.Width = 60;
                terDesc.Visible = true;
                terDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(terDesc);

                if (this.multiMoneda)
                {
                    //Moneda
                    GridColumn moneda = new GridColumn();
                    moneda.FieldName = this.unboundPrefix + "MonedaID";
                    moneda.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                    moneda.UnboundType = UnboundColumnType.String;
                    moneda.VisibleIndex = 8;
                    moneda.Width = 25;
                    moneda.Visible = true;
                    moneda.OptionsColumn.AllowEdit = false;
                    this.gvDocuments.Columns.Add(moneda);

                    //Tasa de Cambio
                    GridColumn tasaCambio = new GridColumn();
                    tasaCambio.FieldName = this.unboundPrefix + "TasaCambio";
                    tasaCambio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblExchangeRate");
                    tasaCambio.UnboundType = UnboundColumnType.Decimal;
                    tasaCambio.VisibleIndex = 9;
                    tasaCambio.Width = 30;
                    tasaCambio.Visible = this.multiMoneda;
                    tasaCambio.OptionsColumn.AllowEdit = false;
                    this.gvDocuments.Columns.Add(tasaCambio);
                }

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 10;
                valor.Width = 50;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 11;
                desc.Width = 75;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);

                //Documento
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
                file.UnboundType = UnboundColumnType.String;
                file.Width = 30;
                file.VisibleIndex = 12;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionCausacion.cs", "AddDocumentCols"));
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this._docs[i].Aprobado.Value = true;
                    this._docs[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this._docs[i].Aprobado.Value = false;
                    this._docs[i].Rechazado.Value = false;
                }
            }
            this.gcDocuments.RefreshDataSource();
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

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._docs[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionCausacion.cs", "editLink_Click"));
            }
        }
        
        /// <summary>
        /// Valida que el campo Fk al salir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterTercero_Leave(object sender, EventArgs e)
        {
            try
            {                
                this.LoadDocuments();             
                this.gcDocuments.DataSource = this._docs;
                this.gcDocuments.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-AprobacionCausac.cs", "masterTercero_Leave: " + ex.Message));
            }
        }

        #endregion

        #region Eventos grillas

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }

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
                if ((bool)e.Value)
                    this._docs[e.RowHandle].Rechazado.Value = false;
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                    this._docs[e.RowHandle].Aprobado.Value = false;
            }
            #endregion

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "vlrBaseML" || fieldName == "vlrBaseME" || fieldName == "vlrMdaLoc" || fieldName == "vlrMdaExt")
            {
                e.RepositoryItem = this.editSpin;
            }
            if (fieldName == "TasaCambio")
            {
                e.RepositoryItem = this.editSpin4;
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            base.gvDocuments_CustomRowCellEdit(sender, e);

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor")
            {
                e.RepositoryItem = this.editSpin;
            }
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            base.riPopup_QueryPopUp(sender, e);

            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
            {
                this.richEditControl.ReadOnly = true;
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
            }
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
                this.richEditControl.ReadOnly = false;
            else
                base.riPopup_QueryResultValue(sender, e);
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
                if (this._docs != null && this._docs.Count != 0)
                {
                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
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
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected override void ApproveThread()
        {
            try
            {
                #region Aprueba los registros
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_ComprobanteAprobacion> ApproveTmp = new List<DTO_ComprobanteAprobacion>();
                foreach (var itemApprove in this._docs)
                    ApproveTmp.Add(itemApprove);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.CuentasXPagar_AprobarRechazar(this.documentID, this.actividadFlujoID, ApproveTmp, true, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                #region Envia alarmas y carga los resultados NOKS
                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                List<int> docsOK = new List<int>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object obj in results)
                {
                    #region Funciones de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion

                    if (this._docs[i].Aprobado.Value.Value || this._docs[i].Rechazado.Value.Value)
                    {
                        MailType mType = this._docs[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                        bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                        }
                        else
                        {
                            DTO_Alarma r = (DTO_Alarma)obj;
                            docsOK.Add(Convert.ToInt32(r.NumeroDoc));
                        }
                    }
                    i++;
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                #endregion
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
                    string reportName = this._bc.AdministrationModel.Reportes_Cp_CausacionFacturas(item, true, false,ExportFormatType.pdf);
                    if (deseaImp)
                    {
                        string fileURl =    this._bc.UrlDocumentFile(TipoArchivo.Documentos, item, null, reportName.ToString());
                        Process.Start(fileURl);
                    }
                }
                #endregion

                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionCausacionFact.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}