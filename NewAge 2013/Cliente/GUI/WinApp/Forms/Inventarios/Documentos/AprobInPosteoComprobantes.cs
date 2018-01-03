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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobInPosteoComprobantes : DocumentAprobForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_glDocumentoControl> _docs = null;
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
                this._docs = _bc.AdministrationModel.glDocumentoControl_GetForPosteo(ModulesPrefix.@in, true);

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (_docs.Count > 0)
                {
                    this.detailsLoaded = false;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = true;

                    if (!detailsLoaded)
                    {
                        this.currentDoc = this.gvDocuments.GetRow(this.currentRow);//(e.FocusedRowHandle);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInPosteoComprobantes.cs", "LoadDocuments: " + ex.Message));
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected override void LoadDetails()
        {
            try
            {
                DTO_glDocumentoControl doc = (DTO_glDocumentoControl)this.currentDoc;
                DateTime periodo = doc.PeriodoDoc.Value.Value;
                string compID = doc.ComprobanteID.Value;
                int compNro = doc.ComprobanteIDNro.Value.Value;

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

            //Proyecto
            GridColumn proyecto = new GridColumn();
            proyecto.FieldName = this.unboundPrefix + "ProyectoID";
            proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            proyecto.UnboundType = UnboundColumnType.String;
            proyecto.VisibleIndex = 4;
            proyecto.Width = 70;
            proyecto.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(proyecto);

            //Centro de costo
            GridColumn ctoCosto = new GridColumn();
            ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
            ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            ctoCosto.UnboundType = UnboundColumnType.String;
            ctoCosto.VisibleIndex = 5;
            ctoCosto.Width = 70;
            ctoCosto.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(ctoCosto);

            //Valor Moneda local
            GridColumn vlrMdaLoc = new GridColumn();
            vlrMdaLoc.FieldName = this.unboundPrefix + "vlrMdaLoc";
            vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
            vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
            vlrMdaLoc.VisibleIndex = 6;
            vlrMdaLoc.Width = 150;
            vlrMdaLoc.Visible = true;
            vlrMdaLoc.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(vlrMdaLoc);

            if (this.multiMoneda)
            {
                //Valor Moneda Ext
                GridColumn vlrMdaExt = new GridColumn();
                vlrMdaExt.FieldName = this.unboundPrefix + "vlrMdaExt";
                vlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                vlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                vlrMdaExt.VisibleIndex = 7;
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
            this.documentID = AppDocuments.PosteoComprobantesInAprob;
            this.frmModule = ModulesPrefix.@in;
            this.chkSeleccionar.Visible = false;
            base.SetInitParameters();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                //Periodo
                GridColumn per = new GridColumn();
                per.FieldName = this.unboundPrefix + "PeriodoDoc";
                per.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PeriodoDoc");
                per.UnboundType = UnboundColumnType.DateTime;
                per.VisibleIndex = 0;
                per.Width = 50;
                per.Visible = true;
                per.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(per);

                //DocumentoID
                GridColumn docID = new GridColumn();
                docID.FieldName = this.unboundPrefix + "DocumentoID";
                docID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoID");
                docID.UnboundType = UnboundColumnType.String;
                docID.VisibleIndex = 1;
                docID.Width = 50;
                docID.Visible = true;
                docID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(docID);

                //ComprobanteID
                GridColumn comp = new GridColumn();
                comp.FieldName = this.unboundPrefix + "ComprobanteID";
                comp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComprobanteID");
                comp.UnboundType = UnboundColumnType.String;
                comp.VisibleIndex = 2;
                comp.Width = 50;
                comp.Visible = true;
                comp.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(comp);

                //Comprobante Numero
                GridColumn compNro = new GridColumn();
                compNro.FieldName = this.unboundPrefix + "ComprobanteIDNro";
                compNro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComprobanteNro");
                compNro.UnboundType = UnboundColumnType.String;
                compNro.VisibleIndex = 3;
                compNro.Width = 50;
                compNro.Visible = true;
                compNro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(compNro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInPosteoComprobantes.cs", "AddGridCols"));
            }
        }

        #endregion

        #region Eventos grillas

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
                e.RepositoryItem = this.editSpin;
            if (fieldName == "TasaCambio")
                e.RepositoryItem = this.editSpin4;
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
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInPosteoCompribantes.cs", "TBSave"));
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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = _bc.AdministrationModel.AprobarPosteoInv(this.documentID, this.actividadFlujoID, ModulesPrefix.co, this._docs);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(results);

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobInPosteoCompribantes.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}