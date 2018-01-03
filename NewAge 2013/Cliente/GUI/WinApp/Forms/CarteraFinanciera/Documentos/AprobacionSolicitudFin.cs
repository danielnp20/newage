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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionSolicitudFin : DocumentAprobBasicForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.CleanData();
            this.LoadDocuments();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private int docRecursos;

        //Listas de datos
        private List<DTO_ccSolicitudDocu> solicitudes = new List<DTO_ccSolicitudDocu>();

        //Variables Privadas
        private bool isValid = true;
        private DateTime periodo;
        protected Dictionary<string, string> actFlujoForReversion = new Dictionary<string, string>();
        protected List<string> actividadesCombo = new List<string>();
        #endregion

        //public AprobacionSolicitudFin()
        //{
        //    this.InitializeComponent();
        //}
        public AprobacionSolicitudFin()
            : base() { this.GetActividadesPadre(); }

        public AprobacionSolicitudFin(string mod)
            : base(mod) { this.GetActividadesPadre(); }

        /// <summary>
        /// Constructor que permite filtrar la libranza y validar si es solo lectura para Modulo cf
        /// </summary>
        /// <param name="libranza"> Libranza o credito a filtrar</param>
        /// <param name="readOnly"> Si es solo lectura</param>
        public AprobacionSolicitudFin(int libranza, bool readOnly) : base(ModulesPrefix.cf.ToString())
        {
            this.gvDocuments.ActiveFilterString = "StartsWith([Unbound_Libranza]," + libranza.ToString() + ")";
            if (readOnly)
            {
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Visible = false;
                this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"].Visible = false;
                FormProvider.Master.itemSave.Enabled = false;
            }
            this.GetActividadesPadre();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AprobacionSolicitudFin;
            this.docRecursos = AppDocuments.AprobacionSolicitudLibranza;
            this.frmModule = ModulesPrefix.cf;

            //Estable la fecha con base a la fecha del periodo
            this.periodo = Convert.ToDateTime(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this._bc.InitMasterUC(this.mfCliente, AppMasters.ccCliente, true, true, true, false);

            DateTime fecha = DateTime.Now;
            if (fecha.Month > periodo.Month)
            {
                int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                fecha = new DateTime(periodo.Year, periodo.Month, day);
            }
            this.dtFecha.DateTime = fecha;
            base.SetInitParameters();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                #region Columnas Principal
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 50;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Aprobado");
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
                noAprob.Visible = true;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Campo del combo actividades
                GridColumn cmbActividades = new GridColumn();
                cmbActividades.FieldName = this.unboundPrefix + "ActividadFlujoDesc";
                cmbActividades.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ActividadesFlujo");
                cmbActividades.UnboundType = UnboundColumnType.String;
                cmbActividades.VisibleIndex = 2;
                cmbActividades.Width = 80;
                cmbActividades.Visible = false;
                cmbActividades.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                cmbActividades.OptionsColumn.AllowEdit = true;
                cmbActividades.ColumnEdit = this.editCmb;
                this.gvDocuments.Columns.Add(cmbActividades);

                //Solicitud
                GridColumn solicitud = new GridColumn();
                solicitud.FieldName = this.unboundPrefix + "Libranza";
                solicitud.Caption = _bc.GetResource(LanguageTypes.Forms, "32317_Libranza");
                solicitud.UnboundType = UnboundColumnType.String;
                solicitud.VisibleIndex = 2;
                solicitud.Width = 70;
                solicitud.Visible = true;
                solicitud.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(solicitud);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaLiquida";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 3;
                fecha.Width = 300;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);

                //Cedula
                GridColumn cedula = new GridColumn();
                cedula.FieldName = this.unboundPrefix + "ClienteRadica";
                cedula.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Cedula");
                cedula.UnboundType = UnboundColumnType.Integer;
                cedula.VisibleIndex = 4;
                cedula.Width = 300;
                cedula.Visible = true;
                cedula.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(cedula);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 5;
                nombre.Width = 100;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombre);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "VlrSolicitado";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valor.AppearanceCell.Options.UseTextOptions = true;
                valor.VisibleIndex = 6;
                valor.Width = 95;
                valor.OptionsColumn.AllowEdit = false;
                valor.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(valor);

                //Valor Futuro
                GridColumn VlrLibranza = new GridColumn();
                VlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                VlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, "32311_VlrLibranza");
                VlrLibranza.UnboundType = UnboundColumnType.Decimal;
                VlrLibranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrLibranza.AppearanceCell.Options.UseTextOptions = true;
                VlrLibranza.VisibleIndex = 7;
                VlrLibranza.Width = 95;
                VlrLibranza.OptionsColumn.AllowEdit = false;
                VlrLibranza.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrLibranza);

                //Valor Giro
                GridColumn valorGiro = new GridColumn();
                valorGiro.FieldName = this.unboundPrefix + "VlrGiro";
                valorGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ValorGiro");
                valorGiro.UnboundType = UnboundColumnType.Decimal;
                valorGiro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorGiro.AppearanceCell.Options.UseTextOptions = true;
                valorGiro.VisibleIndex = 8;
                valorGiro.Width = 95;
                valorGiro.OptionsColumn.AllowEdit = false;
                valorGiro.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(valorGiro);

                //Valor Financia Seguro
                GridColumn valorSeguro = new GridColumn();
                valorSeguro.FieldName = this.unboundPrefix + "VlrFinanciaSeguro";
                valorSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrFinanciaSeguro");
                valorSeguro.UnboundType = UnboundColumnType.Decimal;
                valorSeguro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorSeguro.AppearanceCell.Options.UseTextOptions = true;
                valorSeguro.VisibleIndex = 9;
                valorSeguro.Width = 95;
                valorSeguro.OptionsColumn.AllowEdit = false;
                valorSeguro.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(valorSeguro);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this.unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.VisibleIndex = 10;
                observacion.Width = 100;
                observacion.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(observacion);
                #endregion
                this.gvDocuments.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionViabilidad.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                //this.currentDoc = null;
                this.solicitudes = this._bc.AdministrationModel.GetSolicitudesByActividad(this.actividadFlujoID);

                if (this.mfCliente.ValidID)
                    this.solicitudes = this.solicitudes.FindAll(x=>x.ClienteID.Value == this.mfCliente.Value);

                if (!string.IsNullOrEmpty(this.txtLibranza.Text))
                    this.solicitudes = this.solicitudes.FindAll(x => x.Libranza.Value == Convert.ToInt32(this.txtLibranza.Text));

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this.solicitudes.Count > 0)
                {
                    //this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this.solicitudes;
                    this.gvDocuments.BestFitColumns();
                    this.allowValidate = true;
                    this.gvDocuments.MoveFirst();
                }
                else
                    this.gcDocuments.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionViabilidad.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected override bool ValidateDocRow(int fila)
        {
            try
            {
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
                bool rechazado = false;
                if (this.gvDocuments.GetRowCellValue(fila, col) != null)
                    rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);

                if (rechazado)
                {
                    col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                    string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();

                    if (string.IsNullOrEmpty(desc))
                    {
                        string msg = string.Format(rsxEmpty, col.Caption);
                        this.gvDocuments.SetColumnError(col, msg);
                        return false;
                    }
                    #region Valida que el combo de actividad de flujo no este vacio
                    col = this.gvDocuments.Columns[this.unboundPrefix + "ActividadFlujoDesc"];
                    string desc2 = this.gvDocuments.GetRowCellValue(fila, col).ToString();
                    if (string.IsNullOrEmpty(desc2))
                    {
                        string msg = string.Format(rsxEmpty, col.Caption);
                        this.gvDocuments.SetColumnError(col, msg);
                        return false;
                    }
                    else
                        this.gvDocuments.SetColumnError(col, string.Empty);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "ValidateDocRow"));
            }

            return true;
        }

      
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        {
            this.currentRow = -1;
            this.solicitudes = new List<DTO_ccSolicitudDocu>();
            this.gcDocuments.DataSource = null;
        }

        /// <summary>
        /// Se ejecuta al entrar al formulario
        /// </summary>
        private void GetActividadesPadre()
        {
            //Carga la actividades a revertir
            List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this.actividadFlujoID);
            foreach (DTO_glActividadFlujo act in actPadres)
            {
                this.actividadesCombo.Add(act.Descriptivo.Value);
                this.actFlujoForReversion.Add(act.Descriptivo.Value, act.ID.Value);
            }
            this.editCmb.Items.AddRange(this.actividadesCombo);
            //this.editCmb.EditValue = "1";
            //this.editCmb.Items.AddRange(this.actividadesCombo);
        }

        #endregion

        #region Evento Header

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
                    this.solicitudes[i].Aprobado.Value = true;
                    this.solicitudes[i].Rechazado.Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this.solicitudes[i].Aprobado.Value = false;
                    this.solicitudes[i].Rechazado.Value = false;
                }
            }
            this.gcDocuments.RefreshDataSource();
        }

        /// <summary>
        /// Evento al dejar el control 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Evento al dejar el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            this.LoadDocuments();
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.currentRow != -1)
                {
                    if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                        this.currentRow = e.FocusedRowHandle;

                    this.txtObservacion.Text = this.solicitudes[this.currentRow].ObservacionRechazo.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionViabilidad.cs", "gvDocuments_FocusedRowChanged"));
            }
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

            DTO_ccSolicitudDocu rowCurrent = (DTO_ccSolicitudDocu)this.gvDocuments.GetRow(e.RowHandle);
            #region Generales
            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value && rowCurrent != null)
                {
                    rowCurrent.Rechazado.Value = false;
                    List<DTO_ccSolicitudDocu> temp = this.solicitudes.Where(x => x.Rechazado.Value == true).ToList();
                    bool visibleON = temp.Count > 0 ? true : false;
                    this.gvDocuments.Columns[this.unboundPrefix + "ActividadFlujoDesc"].Visible = visibleON;
                }                   
            }

            if (fieldName == "Rechazado" && rowCurrent != null)
            {
                if ((bool)e.Value)
                {
                    rowCurrent.Aprobado.Value = false;
                    this.gvDocuments.Columns[this.unboundPrefix+ "ActividadFlujoDesc"].Visible = true;
                }
                   
            }
            if (fieldName == "ActividadFlujoDesc")
            {
                string actFlujoDesc = e.Value.ToString();
                rowCurrent.ActividadFlujoDesc = actFlujoDesc;
                rowCurrent.ActividadFlujoReversion.Value = this.actFlujoForReversion[actFlujoDesc];
            }
            #endregion
            this.gcDocuments.RefreshDataSource();
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "ActividadFlujoDesc" && e.Value == null)
                {
                    e.Value = ((DTO_ccSolicitudDocu)e.Row).ActividadFlujoDesc;
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                e.Value = fi.GetValue(dto);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "ActividadFlujoDesc")
                {
                    DTO_ccSolicitudDocu a = (DTO_ccSolicitudDocu)e.Row;
                    a.ActividadFlujoDesc = e.Value.ToString();
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                    }
                }
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
                this.solicitudes.Where(x => x.Aprobado.Value == true).ToList();
                //this.solicitudes.RemoveAll(x => x.Aprobado.Value ==false);
                if (this.solicitudes != null && this.solicitudes.Count > 0)
                {
                    this.solicitudes.ForEach(x => x.FechaLiquida.Value = this.dtFecha.DateTime);

                    if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionViabilidad.cs", "TBSave"));
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
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_ccSolicitudAnexo> anexos = new List<DTO_ccSolicitudAnexo>();
                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                results = _bc.AdministrationModel.SolicitudFin_AprobarRechazar(this.documentID, this.actividadFlujoID, this.solicitudes, anexos);
                FormProvider.Master.StopProgressBarThread(this.documentID);

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

                        #region Trae Tipos de Correo 
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "TipoCorreo",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = "1",//Bienvenida
                            OperadorSentencia = "OR"
                        });
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "TipoCorreo",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = "3",//Renovacion Poliza
                            OperadorSentencia = "AND"
                        });
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        consulta.Filtros = filtros;
                        //Trae con filtros  
                        long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.glCorreosEspeciales, consulta, null, true);
                        List<DTO_glCorreosEspeciales> correoEspec = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.glCorreosEspeciales, count, 1, consulta, null, true).Cast<DTO_glCorreosEspeciales>().ToList();
                        #endregion
                        if (this.solicitudes[i].Aprobado.Value.Value)
                        {
                            if (obj.GetType() == typeof(DTO_TxResult))
                            {
                                DTO_TxResult r = (DTO_TxResult)obj;
                                resultsNOK.Add(r);
                                this.isValid = false;
                            }
                            else
                            {
                                DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.solicitudes[i].ClienteID.Value, true);
                                if (this.solicitudes[i].TipoCredito.Value == (byte)TipoCredito.Nuevo)
                                {
                                    #region Envia el correo de Bienvenida                                
                                    if (correoEspec.FindAll(x=>x.TipoCorreo.Value == 1).Count > 0) //Filtra los correos de Bienvenida
                                    {
                                        string asunto = correoEspec.Find(x => x.TipoCorreo.Value == 1).Asunto.Value;
                                        string mensaje = correoEspec.Find(x => x.TipoCorreo.Value == 1).PlantillaEMail.Value;
                                        this._bc.SendMail(this.documentID, asunto, mensaje, cliente.Correo.Value, correoEspec.Find(x => x.TipoCorreo.Value == 1).CuentaOrigen.Value.Value);
                                    }
                                    #endregion
                                }
                                else if (this.solicitudes[i].TipoCredito.Value == (byte)TipoCredito.PolizaRenueva || this.solicitudes[i].TipoCredito.Value == (byte)TipoCredito.PolizaSinCredito)
                                {
                                    #region Envia el correo de Renovacion Pol                                
                                    if (correoEspec.FindAll(x => x.TipoCorreo.Value == 3).Count > 0) //Filtra los correos de REnovacion Pol
                                    {
                                        string asunto = correoEspec.Find(x => x.TipoCorreo.Value == 3).Asunto.Value;
                                        string mensaje = correoEspec.Find(x => x.TipoCorreo.Value == 3).PlantillaEMail.Value;
                                        this._bc.SendMail(this.documentID, asunto, mensaje, cliente.Correo.Value, correoEspec.Find(x => x.TipoCorreo.Value == 3).CuentaOrigen.Value.Value);
                                    }
                                    #endregion
                                }

                            }
                        }

                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                //if (this.isValid)
                    this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionViabilidad.cs", "ApproveThread"));
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