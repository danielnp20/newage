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
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Aprobacion de Solicitudes
    /// </summary>
    public partial class AprobacionContrato : DocumentAprobComplexForm
    {
        //public AprobacionContrato()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_prContratoAprob> _docs = null;

        private string unboundPrefixCargo = "UnboundPref_";
        private string _valueSelect;
        private decimal _tasaCambio = 0;
        #endregion

        #region Delegados

        public delegate void CleanData();
        public CleanData cleanData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public void CleanDataMethod()
        {
            this.LoadDocuments();
        }

        #endregion

        #region Funciones Virtuales del DocumentAprobForm

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ContratoAprob;
            this.frmModule = ModulesPrefix.pr;
            this.cleanData = new CleanData(CleanDataMethod);
            base.SetInitParameters();
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize() 
        {
            this.lookUpDocumentos.Visible = false;
            this.lblUserTareas.Visible = false;
        }

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                this.currentRow = -1;
                this._docs = _bc.AdministrationModel.Contrato_GetPendientesByModulo(AppDocuments.SolicitudPreAprob, this.actividadFlujoID, this._bc.AdministrationModel.User);
                foreach (var item in this._docs)
                {
                    item.ContratoAprobDet = new List<DTO_prContratoAprobDet>();
                    item.FileUrl = string.Empty;
                }
                if (this.gvDocuments.Columns.Count > 0)
                    this.SourceGrid();
                DTO_prContratoAprob currentDocTemp = (DTO_prContratoAprob)currentDoc;

                if (currentDocTemp != null)
                {
                    this.txtTotalMdaLocal.EditValue = currentDocTemp.ValorTotalML.Value.Value;
                    string monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    if (this.multiMoneda)
                    {
                        this._tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(monedaExtranjera, currentDocTemp.Fecha.Value.Value);
                        this.txtTotalMdaExt.EditValue = _tasaCambio != 0 ? currentDocTemp.ValorTotalML.Value.Value / _tasaCambio : 0;
                    }
                }
                else
                {
                    this.txtTotalMdaLocal.EditValue = 0;
                    this.txtTotalMdaExt.EditValue =  0;
                }
            }
            catch(Exception ex)
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
                this.currentDet = null;
                this.currentDetRow = -1;

                DTO_prContratoAprob doc = (DTO_prContratoAprob)this.currentDoc;
                string prefijo = doc.PrefijoID.Value;
                int solNro = doc.DocumentoNro.Value.Value;

                DTO_prOrdenCompra sol = _bc.AdministrationModel.OrdenCompra_Load(AppDocuments.Contrato, prefijo, solNro);

                List<DTO_prOrdenCompraFooter> details = null;
                if (sol != null && sol.Footer.Count != 0)
                {
                    this.detFooterLoaded = false;
                    this.currentDetRow = 0;
                    details = sol.Footer;
                    this.currentDet = sol.Footer[this.currentDetRow];
                    this.LoadDetFooter();
                    this.gvDetails.MoveFirst();
                }
                else 
                {
                    details = new List<DTO_prOrdenCompraFooter>();
                    this.gcDetFooter.DataSource = null;
                }

                this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected override void LoadDetFooter()
        {
            try
            {
                DTO_prOrdenCompraFooter det = (DTO_prOrdenCompraFooter)this.currentDet;

                List<DTO_prSolicitudCargos> footer = null;
                if (det != null && det.SolicitudCargos.Count > 0)
                    footer = det.SolicitudCargos;
                else
                    footer = new List<DTO_prSolicitudCargos>();

                this.gcDetFooter.DataSource = footer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                //Prefijo
                GridColumn pref = new GridColumn();
                pref.FieldName = this.unboundPrefix + "PrefijoID";
                pref.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoID");
                pref.UnboundType = UnboundColumnType.String;
                pref.VisibleIndex = 3;
                pref.Width = 50;
                pref.Visible = true;
                pref.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(pref);

                //Documento Numero
                GridColumn docNro = new GridColumn();
                docNro.FieldName = this.unboundPrefix + "DocumentoNro";
                docNro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoNro");
                docNro.UnboundType = UnboundColumnType.String;
                docNro.VisibleIndex = 4;
                docNro.Width = 50;
                docNro.Visible = true;
                docNro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(docNro);

                //ValorTotalML
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "ValorTotML";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 9;
                valor.Width = 100;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 60;
                file.VisibleIndex = 10;
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        protected override void AddDetailCols()
        {
            try
            {
                #region Columnas basicas
                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 100;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 100;
                codRef.Visible = true;
                codRef.Fixed = FixedStyle.Left;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro1");
                param1.UnboundType = UnboundColumnType.String;
                param1.VisibleIndex = 3;
                param1.Width = 100;
                param1.Visible = true;
                param1.Fixed = FixedStyle.Left;
                param1.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro2");
                param2.UnboundType = UnboundColumnType.String;
                param2.VisibleIndex = 4;
                param2.Width = 100;
                param2.Visible = true;
                param2.Fixed = FixedStyle.Left;
                param2.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(param2);
                #endregion
                #region Columnas extras
                #region Columnas Visible
                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 5;
                desc.Width = 110;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(desc);

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.VisibleIndex = 6;
                unidad.Width = 70;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(unidad);

                //Cantidad Solicitud
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadCont";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
                cant.UnboundType = UnboundColumnType.Integer;
                cant.VisibleIndex = 7;
                cant.Width = 70;
                cant.Visible = true;
                cant.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(cant);

                ////Proyecto
                //GridColumn proyecto = new GridColumn();
                //proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                //proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                //proyecto.UnboundType = UnboundColumnType.String;
                //proyecto.VisibleIndex = 8;
                //proyecto.Width = 100;
                //proyecto.Visible = true;
                //proyecto.OptionsColumn.AllowEdit = true;
                //this.gvDetails.Columns.Add(proyecto);

                ////Centro de costo
                //GridColumn ctoCosto = new GridColumn();
                //ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                //ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                //ctoCosto.UnboundType = UnboundColumnType.String;
                //ctoCosto.VisibleIndex = 9;
                //ctoCosto.Width = 100;
                //ctoCosto.Visible = true;
                //ctoCosto.OptionsColumn.AllowEdit = true;
                //this.gvDetails.Columns.Add(ctoCosto);
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetails.Columns.Add(numDoc);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefix + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDetails.Columns.Add(solDocu);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetails.Columns.Add(consDeta);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDetails.Columns.Add(solDeta);


                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetails.Columns.Add(colIndex);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "AddDetailsCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del footer del detalle
        /// </summary>
        protected override void AddDetFooterCols()
        {
            try
            {
                #region Columnas Visibles
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefixCargo + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 1;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefixCargo + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 2;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(ctoCosto);

                //Centro de costo
                GridColumn percent = new GridColumn();
                percent.FieldName = this.unboundPrefixCargo + "PorcentajeID";
                percent.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
                percent.UnboundType = UnboundColumnType.Decimal;
                percent.VisibleIndex = 3;
                percent.Width = 100;
                percent.Visible = true;
                percent.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(percent);
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixCargo + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetFooter.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixCargo + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetFooter.Columns.Add(consDeta);

                //Indice de la fila de la grilla de los cargos
                GridColumn cargoColIndex = new GridColumn();
                cargoColIndex.FieldName = this.unboundPrefixCargo + "Index";
                cargoColIndex.UnboundType = UnboundColumnType.Integer;
                cargoColIndex.Visible = false;
                this.gvDetFooter.Columns.Add(cargoColIndex);

                //Indice de la fila la grilla principal
                GridColumn detColIndex = new GridColumn();
                detColIndex.FieldName = this.unboundPrefixCargo + "IndexDet";
                detColIndex.UnboundType = UnboundColumnType.Integer;
                detColIndex.Visible = false;
                this.gvDetFooter.Columns.Add(detColIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "AddDetFooter"));
            }
        }

        private void SourceGrid()
        {
            this.currentDoc = null;
            this.currentRow = -1;
            this.gcDocuments.DataSource = null;
            if (this._docs != null && this._docs.Count > 0)
            {
                this.detailsLoaded = false;
                this.allowValidate = false;
                this.currentRow = 0;
                this.gcDocuments.DataSource = this._docs;
                this.allowValidate = true;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);

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
                this.gcDetFooter.DataSource = null;
            }
        }
        #endregion

        #region Eventos Virtuales grillas

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
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                    this.currentRow = e.FocusedRowHandle;

                this.currentDoc = (DTO_prContratoAprob)this.gvDocuments.GetRow(this.currentRow);
                this.LoadDetails();
                DTO_prContratoAprob currentDocTemp = (DTO_prContratoAprob)currentDoc;
                if (currentDocTemp != null)
                {                    
                    this.txtTotalMdaLocal.EditValue = currentDocTemp.ValorTotalML.Value.Value;
                    if (this.multiMoneda)
                        this.txtTotalMdaExt.EditValue = _tasaCambio != 0 ? currentDocTemp.ValorTotalML.Value.Value / _tasaCambio : 0; 
                }

                this.detailsLoaded = true;
            }
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
            
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDetFooter_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionContrato.cs", "editLink_Click"));
            }
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

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadDocuments();
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
               
                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

               //List<DTO_TxResult> results = null;

               List<DTO_SerializedObject> results = _bc.AdministrationModel.Contrato_AprobarRechazar(AppDocuments.ContratoAprob, this.actividadFlujoID, this._bc.AdministrationModel.User, this._docs, false);
               FormProvider.Master.StopProgressBarThread(this.documentID);
               int i = 0;
               int percent = 0;
               List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

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
                   }
                   i++;
               }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.cleanData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}