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
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionConsumoProyecto : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public virtual void RefreshDataMethod()
        {
            this.LoadDocuments();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        protected int userID = 0;
        //Para manejo de propiedades
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected string unboundPrefix = "Unbound_";
        protected bool multiMoneda;
        protected bool allowValidate = true;
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;

        protected int currentRow = -1;
        protected string monedaID;
        protected string monedaExtranjeraID;
        protected string areaFuncionalID;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private List<DTO_ConvenioAprob> _docs = null;
        #endregion

        public AprobacionConsumoProyecto()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.pr;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AfterInitialize();

                //Asigna la lista de columnas
                this.AddDocumentCols();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    this.actividadFlujoID = actividades[0];
                    this.LoadDocuments();
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion

                this.refreshData = new RefreshData(RefreshDataMethod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "DocumentAprobBasicForm"));
            }
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.documentID = AppDocuments.ConsumoProyectoAprob;
            this.frmModule = ModulesPrefix.pr;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() { }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected virtual void LoadDocuments()
        {
            this.monedaID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjeraID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;

            this._docs = _bc.AdministrationModel.Convenio_GetPendientesByModulo(this.documentID, this.actividadFlujoID, this._bc.AdministrationModel.User, true);
            foreach (var item in this._docs)
                item.FileUrl = string.Empty;
            this.currentRow = -1;
            this.gcDocuments.DataSource = null;

            if (this._docs.Count > 0)
            {
                //this.gcDocuments.DataSource = this._docs;
                this.allowValidate = false;
                this.currentRow = 0;
                this.gcDocuments.DataSource = this._docs;
                this.allowValidate = true;
                this.gvDocuments.MoveFirst();
            }
            else
            {
                this.gcDocuments.DataSource = null;
            }
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddDocumentCols()
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
            noAprob.Visible = true;
            noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
            noAprob.AppearanceHeader.ForeColor = Color.Red;
            noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            noAprob.AppearanceHeader.Options.UseTextOptions = true;
            noAprob.AppearanceHeader.Options.UseFont = true;
            noAprob.AppearanceHeader.Options.UseForeColor = true;   
            this.gvDocuments.Columns.Add(noAprob);

            //Observacion
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Observacion";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 2;
            desc.Width = 100;
            desc.Visible = true;
            this.gvDocuments.Columns.Add(desc);

            //Periodo
            GridColumn per = new GridColumn();
            per.FieldName = this.unboundPrefix + "PeriodoID";
            per.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PeriodoID");
            per.UnboundType = UnboundColumnType.DateTime;
            per.VisibleIndex = 3;
            per.Width = 40;
            per.Visible = true;
            per.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(per);

            //Documento Nro ORden Compra
            GridColumn docNroOrden = new GridColumn();
            docNroOrden.FieldName = this.unboundPrefix + "DocumentoNro";
            docNroOrden.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoNroOrdenCompra");
            docNroOrden.UnboundType = UnboundColumnType.String;
            docNroOrden.VisibleIndex = 4;
            docNroOrden.Width = 30;
            docNroOrden.Visible = true;
            docNroOrden.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(docNroOrden);

            //ProyectoID
            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 5;
            ProyectoID.Width = 40;
            ProyectoID.Visible = true;
            ProyectoID.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(ProyectoID);

            //CentroCostoID
            GridColumn CentroCostoID = new GridColumn();
            CentroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
            CentroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            CentroCostoID.UnboundType = UnboundColumnType.String;
            CentroCostoID.VisibleIndex = 6;
            CentroCostoID.Width = 40;
            CentroCostoID.Visible = true;
            CentroCostoID.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(CentroCostoID);

            //Moneda
            GridColumn moneda = new GridColumn();
            moneda.FieldName = this.unboundPrefix + "Moneda";
            moneda.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
            moneda.UnboundType = UnboundColumnType.String;
            moneda.VisibleIndex = 7;
            moneda.Width = 40;
            moneda.Visible = true;
            moneda.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(moneda);

            //Valor
            GridColumn valor = new GridColumn();
            valor.FieldName = this.unboundPrefix + "Valor";
            valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
            valor.UnboundType = UnboundColumnType.Decimal;
            valor.VisibleIndex = 8;
            valor.Width = 70;
            valor.Visible = true;
            valor.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(valor);

            //Archivo
            GridColumn file = new GridColumn();
            file.FieldName = this.unboundPrefix + "FileUrl";
            file.OptionsColumn.ShowCaption = false;
            file.UnboundType = UnboundColumnType.String;
            file.Width = 50;
            file.VisibleIndex = 9;
            file.Visible = true;
            this.gvDocuments.Columns.Add(file);

            #region Columnas Detalle

            //ProveedorID
            GridColumn ProveedorID = new GridColumn();
            ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
            ProveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
            ProveedorID.UnboundType = UnboundColumnType.String;
            ProveedorID.VisibleIndex = 0;
            ProveedorID.Width = 80;
            ProveedorID.Visible = true;
            ProveedorID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(ProveedorID);

            //DescriptivoProv
            GridColumn DescriptivoProv = new GridColumn();
            DescriptivoProv.FieldName = this.unboundPrefix + "DescriptivoProv";
            DescriptivoProv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescriptivoProv");
            DescriptivoProv.UnboundType = UnboundColumnType.String;
            DescriptivoProv.VisibleIndex = 1;
            DescriptivoProv.Width = 100;
            DescriptivoProv.Visible = true;
            DescriptivoProv.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DescriptivoProv);

            //CodigoBSID
            GridColumn codBS = new GridColumn();
            codBS.FieldName = this.unboundPrefix + "CodigoBSID";
            codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            codBS.UnboundType = UnboundColumnType.String;
            codBS.VisibleIndex = 2;
            codBS.Width = 80;
            codBS.Visible = true;
            codBS.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(codBS);

            //DescriptivoCodBS
            GridColumn DescriptivoCodBS = new GridColumn();
            DescriptivoCodBS.FieldName = this.unboundPrefix + "DescriptivoCodBS";
            DescriptivoCodBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescriptivoCodBS");
            DescriptivoCodBS.UnboundType = UnboundColumnType.String;
            DescriptivoCodBS.VisibleIndex = 3;
            DescriptivoCodBS.Width = 110;
            DescriptivoCodBS.Visible = true;
            DescriptivoCodBS.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DescriptivoCodBS);

            //inReferenciaID
            GridColumn inReferenciaID = new GridColumn();
            inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
            inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            inReferenciaID.UnboundType = UnboundColumnType.String;
            inReferenciaID.VisibleIndex = 4;
            inReferenciaID.Width = 80;
            inReferenciaID.Visible = true;
            inReferenciaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(inReferenciaID);

            //DescriptivoCodBS
            GridColumn DescriptivoRef = new GridColumn();
            DescriptivoRef.FieldName = this.unboundPrefix + "DescriptivoRef";
            DescriptivoRef.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescriptivoRef");
            DescriptivoRef.UnboundType = UnboundColumnType.String;
            DescriptivoRef.VisibleIndex = 5;
            DescriptivoRef.Width = 110;
            DescriptivoRef.Visible = true;
            DescriptivoRef.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DescriptivoRef);

            //SerialID
            GridColumn SerialID = new GridColumn();
            SerialID.FieldName = this.unboundPrefix + "SerialID";
            SerialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
            SerialID.UnboundType = UnboundColumnType.String;
            SerialID.VisibleIndex = 6;
            SerialID.Width = 90;
            SerialID.Visible = true;
            SerialID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(SerialID);

            //Cantidad
            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            Cantidad.UnboundType = UnboundColumnType.Integer;
            Cantidad.VisibleIndex = 7;
            Cantidad.Width = 70;
            Cantidad.Visible = true;
            Cantidad.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Cantidad);

            //ValorUniDet
            GridColumn ValorUniDet = new GridColumn();
            ValorUniDet.FieldName = this.unboundPrefix + "ValorUniDet";
            ValorUniDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUniDet");
            ValorUniDet.UnboundType = UnboundColumnType.Decimal;
            ValorUniDet.VisibleIndex = 8;
            ValorUniDet.Width = 70;
            ValorUniDet.Visible = true;
            ValorUniDet.ColumnEdit = this.editSpin;
            ValorUniDet.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(ValorUniDet);

            //ValorDet
            GridColumn ValorDet = new GridColumn();
            ValorDet.FieldName = this.unboundPrefix + "ValorDet";
            ValorDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorDet");
            ValorDet.UnboundType = UnboundColumnType.Decimal;
            ValorDet.VisibleIndex = 9;
            ValorDet.Width = 70;
            ValorDet.Visible = true;
            ValorDet.ColumnEdit = this.editSpin;
            ValorDet.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(ValorDet);
            #endregion
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected virtual bool ValidateDocRow(int fila)
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
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "ValidateDocRow"));
            }

            return true;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Se realiza cuando el usuario elige una tarea 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void cmbUserTareas_SelectedValueChanged(object sender, EventArgs e) { }

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
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
        protected virtual void editLink_Click(object sender, EventArgs e) 
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionConsumoProyecto.cs", "editLink_Click"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
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
            if (e.IsSetData)
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

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                e.RepositoryItem = this.richText1;

            if (fieldName == "FileUrl")
                e.RepositoryItem = this.editLink;
        }
        
        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Observacion")
                {
                    e.RepositoryItem = this.riPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
                e.Allow = false;
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                    this.currentRow = e.FocusedRowHandle;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {  //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
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
        protected virtual void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor")
                e.RepositoryItem = this.editSpin;
        }

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();

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
        protected virtual void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Descriptivo")
                this.richEditControl.ReadOnly = false;
            else
                e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.LoadDocuments();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
        protected virtual void ApproveThread()
        {
            try
            {
                #region Aprueba los registros
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = this._bc.AdministrationModel.Convenio_AprobarRechazar(this.documentID, this.actividadFlujoID, this._bc.AdministrationModel.User, this._docs, true);
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
            catch (Exception e)
            {
                MessageBox.Show("Err: " + e.Message);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion

    }
}