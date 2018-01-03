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
    public partial class AprobacionAjusteComp : DocumentAprobForm
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        private List<DTO_ComprobanteAprobacion> _docs = null;

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
                this._docs = _bc.AdministrationModel.AjusteComprobante_GetPendientes(this.actividadFlujoID);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAjusteComp.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected override void LoadDetails()
        {
            try
            {
                DTO_ComprobanteAprobacion doc = (DTO_ComprobanteAprobacion)this.currentDoc;
                DateTime periodo = doc.PeriodoID.Value.Value;
                string compID = doc.ComprobanteID.Value;
                int compNro = doc.ComprobanteNro.Value.Value;

                DTO_Comprobante c = _bc.AdministrationModel.AjusteComprobante_Get(periodo, compID, compNro);
                List<DTO_ComprobanteFooter> details = c != null ? c.Footer : new List<DTO_ComprobanteFooter>();
                this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAjusteComp.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ComprobanteAjusteAprob;
            this.frmModule = ModulesPrefix.co;

            base.SetInitParameters();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                base.AddDocumentCols();

                //Comprobante
                GridColumn comp = new GridColumn();
                comp.FieldName = this.unboundPrefix + "ComprobanteID";
                comp.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_ComprobanteID");
                comp.UnboundType = UnboundColumnType.String;
                comp.VisibleIndex = 3;
                comp.Width = 50;
                comp.Visible = true;
                comp.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(comp);

                //Comprobante Numero
                GridColumn compNro = new GridColumn();
                compNro.FieldName = this.unboundPrefix + "ComprobanteNro";
                compNro.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_ComprobanteNro");
                compNro.UnboundType = UnboundColumnType.String;
                compNro.VisibleIndex = 4;
                compNro.Width = 50;
                compNro.Visible = true;
                compNro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(compNro);

                //Periodo
                GridColumn per = new GridColumn();
                per.FieldName = this.unboundPrefix + "PeriodoID";
                per.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_PeriodoID");
                per.UnboundType = UnboundColumnType.DateTime;
                per.VisibleIndex = 5;
                per.Width = 50;
                per.Visible = true;
                per.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(per);

                ////Archivo
                //GridColumn file = new GridColumn();
                //file.FieldName = this.unboundPrefix + "FileUrl";
                //file.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_FileUrl");
                //file.UnboundType = UnboundColumnType.String;
                //file.Width = 30;
                //file.VisibleIndex = 7;
                //file.Visible = true;
                //this.gvDocuments.Columns.Add(file);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAjusteComp.cs", "AddDocumentCols"));
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
            cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_CuentaID");
            cuenta.UnboundType = UnboundColumnType.String;
            cuenta.VisibleIndex = 0;
            cuenta.Width = 70;
            cuenta.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(cuenta);

            //Tercero
            GridColumn tercero = new GridColumn();
            tercero.FieldName = this.unboundPrefix + "TerceroID";
            tercero.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_TerceroID");
            tercero.UnboundType = UnboundColumnType.String;
            tercero.VisibleIndex = 1;
            tercero.Width = 70;
            tercero.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(tercero);

            //Prefijo
            GridColumn pref = new GridColumn();
            pref.FieldName = this.unboundPrefix + "PrefijoCOM";
            pref.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_PrefijoCOM");
            pref.UnboundType = UnboundColumnType.String;
            pref.VisibleIndex = 2;
            pref.Width = 70;
            pref.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(pref);

            //Documento
            GridColumn doc = new GridColumn();
            doc.FieldName = this.unboundPrefix + "DocumentoCOM";
            doc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_DocumentoCOM");
            doc.UnboundType = UnboundColumnType.String;
            doc.VisibleIndex = 3;
            doc.Width = 70;
            doc.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(doc);

            //Activo
            GridColumn act = new GridColumn();
            act.FieldName = this.unboundPrefix + "ActivoCOM";
            act.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_ActivoCOM");
            act.UnboundType = UnboundColumnType.String;
            act.VisibleIndex = 4;
            act.Width = 70;
            act.Visible = true;
            act.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(act);

            //Proyecto
            GridColumn proyecto = new GridColumn();
            proyecto.FieldName = this.unboundPrefix + "ProyectoID";
            proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_ProyectoID");
            proyecto.UnboundType = UnboundColumnType.String;
            proyecto.VisibleIndex = 5;
            proyecto.Width = 70;
            proyecto.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(proyecto);

            //Centro de costo
            GridColumn ctoCosto = new GridColumn();
            ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
            ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_CentroCostoID");
            ctoCosto.UnboundType = UnboundColumnType.String;
            ctoCosto.VisibleIndex = 6;
            ctoCosto.Width = 70;
            ctoCosto.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(ctoCosto);

            //Linea Presupuestal
            GridColumn linPresup = new GridColumn();
            linPresup.FieldName = this.unboundPrefix + "LineaPresupuestoID";
            linPresup.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_LineaPresupuestoID");
            linPresup.UnboundType = UnboundColumnType.String;
            linPresup.VisibleIndex = 7;
            linPresup.Width = 70;
            linPresup.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(linPresup);

            //Concepto cargo
            GridColumn concCargo = new GridColumn();
            concCargo.FieldName = this.unboundPrefix + "ConceptoCargoID";
            concCargo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_ConceptoCargoID");
            concCargo.UnboundType = UnboundColumnType.String;
            concCargo.VisibleIndex = 8;
            concCargo.Width = 70;
            concCargo.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(concCargo);

            //Lugar Geografico
            GridColumn lg = new GridColumn();
            lg.FieldName = this.unboundPrefix + "LugarGeograficoID";
            lg.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_LugarGeograficoID");
            lg.UnboundType = UnboundColumnType.String;
            lg.VisibleIndex = 9;
            lg.Width = 70;
            lg.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(lg);

            //Valor Base ML
            GridColumn vlrBaseML = new GridColumn();
            vlrBaseML.FieldName = this.unboundPrefix + "vlrBaseML";
            vlrBaseML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_vlrBaseML");
            vlrBaseML.UnboundType = UnboundColumnType.Decimal;
            vlrBaseML.VisibleIndex = 10;
            vlrBaseML.Width = 120;
            vlrBaseML.Visible = true;
            vlrBaseML.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(vlrBaseML);

            //Valor Moneda local
            GridColumn vlrMdaLoc = new GridColumn();
            vlrMdaLoc.FieldName = this.unboundPrefix + "vlrMdaLoc";
            vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_vlrMdaLoc");
            vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
            vlrMdaLoc.VisibleIndex = 11;
            vlrMdaLoc.Width = 150;
            vlrMdaLoc.Visible = true;
            vlrMdaLoc.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(vlrMdaLoc);

            ////Tasa de Cambio
            //GridColumn tasaCambio = new GridColumn();
            //tasaCambio.FieldName = this.unboundPrefix + "TasaCambio";
            //tasaCambio.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_lblExchangeRate");
            //tasaCambio.UnboundType = UnboundColumnType.Decimal;
            //tasaCambio.VisibleIndex = 12;
            //tasaCambio.Width = 100;
            //tasaCambio.Visible = this.multiMoneda;
            //tasaCambio.OptionsColumn.AllowEdit = false;
            //this.gvDetails.Columns.Add(tasaCambio);

            if (this.multiMoneda)
            {
                //Valor Base ME
                GridColumn vlrBaseME = new GridColumn();
                vlrBaseME.FieldName = this.unboundPrefix + "vlrBaseME";
                vlrBaseME.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_vlrBaseME");
                vlrBaseME.UnboundType = UnboundColumnType.Decimal;
                vlrBaseME.VisibleIndex = 13;
                vlrBaseME.Width = 120;
                vlrBaseME.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(vlrBaseME);

                //Valor Moneda Ext
                GridColumn vlrMdaExt = new GridColumn();
                vlrMdaExt.FieldName = this.unboundPrefix + "vlrMdaExt";
                vlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_vlrMdaExt");
                vlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                vlrMdaExt.VisibleIndex = 14;
                vlrMdaExt.Width = 150;
                vlrMdaExt.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(vlrMdaExt);
            }

            //Descriptivo
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Descriptivo";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DocumentoContableAprob + "_Descriptivo");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 15;
            desc.Width = 100;
            desc.Visible = true;
            desc.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(desc);
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

        #endregion

        #region Eventos grillas

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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_ComprobanteAprobacion> ApproveTmp = new List<DTO_ComprobanteAprobacion>();
                foreach (var itemApprove in this._docs)
                    ApproveTmp.Add(itemApprove);

                List<DTO_SerializedObject> results = _bc.AdministrationModel.AjusteComprobante_AprobarRechazar(this.documentID, this.actividadFlujoID, ApproveTmp);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion

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
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionAjusteComp.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}