using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class RecibidoSolicitudDespacho : FormWithToolbar
    {
        #region Delegados

        private delegate void Save();
        private Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private void SaveMethod()
        {
            this._docCtrl = new DTO_glDocumentoControl();
            this._listConsumoRec = new List<DTO_ConveniosResumen>();
            this.gcDocument.DataSource = this._listConsumoRec;
            this.EnableHeader(true);
        }
        #endregion

        #region Variables
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _docCtrl = null;
        private List<DTO_ConveniosResumen> _listConsumoRec;

        private int userID = 0;
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";      
        private bool multiMoneda;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private bool firstTime = true;

        private bool _proveedorFocus = false;
        private bool _prefijoFocus = false;
        private bool _controlFocus = false;
        private string proveedorID;
        private string bodegaID;

        private string monedaLocal;       
        private string defProyecto = string.Empty;
        private string defCentroCosto = string.Empty;
        private string defLineaPresupuesto = string.Empty;
        private string defLugarGeo = string.Empty;
        private string areaFuncionalID = string.Empty;
        private string UsoReferenciaCodigoInd = string.Empty;
        protected List<int> select = new List<int>();

        protected DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        #endregion

        #region Propiedades

        #endregion

        public RecibidoSolicitudDespacho()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this._docCtrl = new  DTO_glDocumentoControl();
                this._listConsumoRec = new List<DTO_ConveniosResumen>();

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
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);  
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecibidoSolicitudDespacho.cs.cs", "RecibidoSolicitudDespacho"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;

            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, true);
            this.documentID = AppDocuments.RecibidoSolicitudDespacho;
            this.frmModule = ModulesPrefix.pr;           

            DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
            this.txtAF.Text = basicDTO.Descriptivo.Value;

            string prefijoID = this._bc.GetPrefijo(this.areaFuncionalID, this.documentID);
            this.txtPrefix.Text = prefijoID;

            DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);
            this.txtDocumentoID.Text = this.documentID.ToString();
            this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
            this.txtNumeroDoc.Text = "0";

            this.lblPrefix.Visible = false;
            this.txtPrefix.Visible = false;   

            string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.pr_Periodo);
            this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
            this.dtFecha.DateTime = this.dtPeriod.DateTime;
            this.dtPeriod.Enabled = false;

            this.saveDelegate = new Save(this.SaveMethod);
            //this.ShowResultDialogDelegate = new ShowResultDialog(this.ShowResultDialogMethod);
        }
        
        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                #region Columnas Header

                //Campo de marca
                GridColumn marca = new GridColumn();
                marca.FieldName = this.unboundPrefix + "Seleccionar";
                marca.Caption = "√";
                marca.UnboundType = UnboundColumnType.Boolean;
                marca.VisibleIndex = 0;
                marca.Width = 40;
                marca.Visible = true;
                marca.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Seleccionar");
                marca.AppearanceHeader.ForeColor = Color.Lime;
                marca.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                marca.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                marca.AppearanceHeader.Options.UseTextOptions = true;
                marca.AppearanceHeader.Options.UseFont = true;
                marca.AppearanceHeader.Options.UseForeColor = true;  
                this.gvDocument.Columns.Add(marca);

                //Documento SolDespacho
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 1;
                PrefDoc.Width = 100;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PrefDoc);

                GridColumn MonedaID = new GridColumn();
                MonedaID.FieldName = this.unboundPrefix + "MonedaIDConvenio";
                MonedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                MonedaID.UnboundType = UnboundColumnType.String;
                MonedaID.VisibleIndex = 2;
                MonedaID.Width = 80;
                MonedaID.Visible = true;
                MonedaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MonedaID);

                //Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 3;
                Valor.Width = 120;
                Valor.Visible = true;
                Valor.ColumnEdit = this.editSpin;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                //IVA
                GridColumn IVA = new GridColumn();
                IVA.FieldName = this.unboundPrefix + "IVA";
                IVA.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IVA");
                IVA.UnboundType = UnboundColumnType.Decimal;
                IVA.VisibleIndex = 4;
                IVA.Width = 100;
                IVA.Visible = true;
                IVA.ColumnEdit = this.editSpin;
                IVA.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(IVA);

                #endregion
                #region Detalle

                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 0;
                CodigoBSID.Width = 80;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(CodigoBSID);

                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 1;
                inReferenciaID.Width = 80;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(inReferenciaID);

                GridColumn descrDetalle = new GridColumn();
                descrDetalle.FieldName = this.unboundPrefix + "DescripDetalle";
                descrDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripDetalle");
                descrDetalle.UnboundType = UnboundColumnType.String;
                descrDetalle.VisibleIndex = 2;
                descrDetalle.Width = 100;
                descrDetalle.Visible = true;
                descrDetalle.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(descrDetalle);   

                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "CantidadConvenio";
                Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
                Cantidad.UnboundType = UnboundColumnType.Decimal;
                Cantidad.VisibleIndex = 6;
                Cantidad.Width = 50;
                Cantidad.Visible = true;
                Cantidad.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(Cantidad);

                GridColumn ValorUni = new GridColumn();
                ValorUni.FieldName = this.unboundPrefix + "ValorUni";
                ValorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                ValorUni.UnboundType = UnboundColumnType.Decimal;
                ValorUni.VisibleIndex = 8;
                ValorUni.Width = 80;
                ValorUni.Visible = true;
                ValorUni.ColumnEdit = this.editSpin;
                ValorUni.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(ValorUni);

                GridColumn ValorTotal = new GridColumn();
                ValorTotal.FieldName = this.unboundPrefix + "ValorTotal";
                ValorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotal");
                ValorTotal.UnboundType = UnboundColumnType.Decimal;
                ValorTotal.VisibleIndex = 9;
                ValorTotal.Width = 80;
                ValorTotal.Visible = true;
                ValorTotal.ColumnEdit = this.editSpin;
                ValorTotal.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(ValorTotal);

                GridColumn IVATotal = new GridColumn();
                IVATotal.FieldName = this.unboundPrefix + "IVATotal";
                IVATotal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IVATotal");
                IVATotal.UnboundType = UnboundColumnType.Decimal;
                IVATotal.VisibleIndex = 10;
                IVATotal.Width = 80;
                IVATotal.Visible = true;
                IVATotal.ColumnEdit = this.editSpin;
                IVATotal.OptionsColumn.AllowEdit = false;
                this.gvPreliminar.Columns.Add(IVATotal);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecibidoSolicitudDespacho.cs.cs", "RecibidoSolicitudDespacho.cs-AddDocumentCols"));
            }
        }

        /// <summary>
        /// Habilita o no el header
        /// </summary>
        /// <param name="enable"></param>
        private void EnableHeader(bool enable)
        {
           // this.dtFechaCorte.Enabled = enable;
        }

        /// <summary>
        /// Valida las filas de la grilla
        /// </summary>
        /// <param name="fila"></param>
        /// <returns></returns>
        private bool ValidateRow(int fila)
        {
            GridColumn col = new GridColumn();

            col = this.gvDocument.Columns[this.unboundPrefix + "CantidadRec"];
            //if (!this._detList[fila].invSerialInd && this._detList[fila].CantidadRec.Value > this._detList[fila].CantidadOC.Value ||
            //    this._detList[fila].invSerialInd && this._detList[fila].CantidadRec.Value > 1 ||
            //    this._detList[fila].invSerialInd && this._detList[fila].CantidadRec.Value < 0)
            //{
            //    this.gvDocument.FocusedRowHandle = fila;
            //    this.gvDocument.SetColumnError(col, "Valor invalido");
            //    return false;
            //}

            col = this.gvDocument.Columns[this.unboundPrefix + "SerialID"];
            //if (this._detList[fila].invSerialInd && this._detList[fila].CantidadRec.Value.Value > 0)
            //{
            //    if (string.IsNullOrEmpty(this._detList[fila].SerialID.Value.Trim()))
            //    {
            //        this.gvDocument.FocusedRowHandle = fila;
            //        this.gvDocument.SetColumnError(col, "Valor invalido");
            //        return false;
            //    }

            //    DTO_acActivoControl activo = new DTO_acActivoControl();
            //    activo.SerialID.Value = this._detList[fila].SerialID.Value;
            //    int result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo).Count;
            //    if (result > 0)
            //    {
            //        this.gvDocument.FocusedRowHandle = fila;
            //        this.gvDocument.SetColumnError(col, DictionaryMessages.In_AlreadyExistSerial);
            //        return false;
            //    }
            //}

            this.gvDocument.SetColumnError(col, null);
            return true;
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadData(bool firstTime)
        {
            this._listConsumoRec = this._bc.AdministrationModel.SolicitudDespachoConvenio_GetResumen(this.documentID, this._bc.AdministrationModel.User, ModulesPrefix.pr, this.masterProveedor.Value);

            if (firstTime)
            {
                #region Load DocumentoControl
                this._docCtrl.EmpresaID.Value = this.empresaID;
                //this._docCtrl.TerceroID.Value = this.terceroID; ////
                this._docCtrl.NumeroDoc.Value = 0;
                this._docCtrl.ComprobanteID.Value = string.Empty; ////this.comprobanteID;
                this._docCtrl.ComprobanteIDNro.Value = 0;
                this._docCtrl.DocumentoNro.Value = 0;
                //this.data.DocCtrl.MonedaID.Value = this.masterMoneda.Value; ////
                this._docCtrl.CuentaID.Value = string.Empty; ////
                this._docCtrl.ProyectoID.Value = this.defProyecto;////
                this._docCtrl.CentroCostoID.Value = this.defCentroCosto; ////
                this._docCtrl.LugarGeograficoID.Value = string.Empty; ////
                this._docCtrl.LineaPresupuestoID.Value = string.Empty;////
                this._docCtrl.Fecha.Value = DateTime.Now;
                this._docCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._docCtrl.PrefijoID.Value = this.txtPrefix.Text;
                this._docCtrl.TasaCambioCONT.Value = 0;////
                this._docCtrl.TasaCambioDOCU.Value = 0;////
              
                this._docCtrl.DocumentoID.Value = this.documentID;
                this._docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;////
                this._docCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._docCtrl.seUsuarioID.Value = this.userID;
                this._docCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._docCtrl.ConsSaldo.Value = 0;
                this._docCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                this._docCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._docCtrl.Descripcion.Value = this.txtDocDesc.Text;
                this._docCtrl.Valor.Value = 0;
                this._docCtrl.Iva.Value = 0;                
                #endregion
            }

            this.gcDocument.DataSource = this._listConsumoRec;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this._listConsumoRec.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla de Detalles

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected  void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
                object dto = (object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    //if (fieldName == "Seleccionar" && e.Value == null)
                    //    e.Value = this.select.Contains(e.ListSourceRowIndex);
                    //else
                    {
                        PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                e.Value = pi.GetValue(dto, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                        else
                        {
                            FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                    e.Value = fi.GetValue(dto);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
                            else
                            {
                                DTO_ConveniosResumen dtoDet = (DTO_ConveniosResumen)e.Row;
                                pi = dtoDet.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (pi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                        e.Value = pi.GetValue(dtoDet, null);
                                    else
                                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoDet, null), null);
                                }
                                else
                                {
                                    fi = dtoDet.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                    if (fi != null)
                                    {
                                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                            e.Value = fi.GetValue(dtoDet);
                                        else
                                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoDet), null);
                                    }
                                }
                            }
                        }
                    }
                }

                if (e.IsSetData)
                {
                    //if (fieldName == "Seleccionar")
                    //{
                    //    bool value = Convert.ToBoolean(e.Value);
                    //    if (value)
                    //        this.select.Add(e.ListSourceRowIndex);
                    //    else
                    //        this.select.Remove(e.ListSourceRowIndex);
                    //}
                    //else
                    {
                        PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (e.Value == null)
                            e.Value = string.Empty;
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                            {
                                pi.SetValue(dto, e.Value, null);
                                //e.Value = pi.GetValue(dto, null);
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
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                {
                                    pi.SetValue(dto, e.Value, null);
                                    //e.Value = pi.GetValue(dto, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)fi.GetValue(dto);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                DTO_ConveniosResumen dtoDet = (DTO_ConveniosResumen)e.Row;
                                pi = dtoDet.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (pi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                    {
                                        e.Value = pi.GetValue(dtoDet, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)pi.GetValue(dtoDet, null);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                                else
                                {
                                    fi = dtoDet.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                    if (fi != null)
                                    {
                                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                        {
                                            pi.SetValue(dto, e.Value, null);
                                            //e.Value = pi.GetValue(dto, null);
                                        }
                                        else
                                        {
                                            UDT udtProp = (UDT)fi.GetValue(dtoDet);
                                            udtProp.SetValueFromString(e.Value.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            #region Se modifican CantidadRec
            if (fieldName == "CantidadRec")
            {
                //if (this._detList[e.RowHandle].CantidadRec.Value == null)
                //    this._detList[e.RowHandle].CantidadRec.Value = 0;

                //if (!this._detList[e.RowHandle].invSerialInd && this._detList[e.RowHandle].CantidadRec.Value.Value > this._detList[e.RowHandle].CantidadOC.Value.Value ||
                //    this._detList[e.RowHandle].invSerialInd && this._detList[e.RowHandle].CantidadRec.Value.Value > 1 ||
                //    this._detList[e.RowHandle].CantidadRec.Value.Value < 0)
                //    this.gvDocument.SetColumnError(e.Column, "Valor invalido");
                //else
                //    this.gvDocument.SetColumnError(e.Column, null);
            }
            #endregion
            #region Se modifican Otros
            if (fieldName == "SerialID")
            {
                //if (this._detList[e.RowHandle].invSerialInd && this._detList[e.RowHandle].CantidadRec.Value.Value > 0 && string.IsNullOrEmpty(this._detList[e.RowHandle].SerialID.Value.Trim()))
                //    this.gvDocument.SetColumnError(e.Column, "Valor invalido");
                //else
                //    this.gvDocument.SetColumnError(e.Column, null);
            }
            #endregion
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvPreliminar_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
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

        #endregion

        #region Eventos del Header

        /// <summary>
        /// Se realiza al dejar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            if (this.masterProveedor.ValidID)
            {
                this.LoadData(firstTime);
                firstTime = false;
            }
        }

        #endregion

        #region Eventos Barra de Herramientas
        
        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                bool selectValid = false;
                foreach (var cons in this._listConsumoRec)
                {
                    if (cons.Seleccionar.Value.Value)
                    {
                        selectValid = true;
                        break;
                    }
                }
                if (selectValid)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._listConsumoRec = new List<DTO_ConveniosResumen>();
                this._docCtrl = new  DTO_glDocumentoControl();
                this.masterProveedor.Value = string.Empty;
                this.gcDocument.DataSource = this._listConsumoRec;
                this.EnableHeader(true);
                this.masterProveedor.Focus();
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
        public void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_SerializedObject result = _bc.AdministrationModel.RecibidoConvenios_Add(this.documentID, this._listConsumoRec, this.masterProveedor.Value, this.dtFecha.DateTime);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);
                if (isOK)
                {
                    this._docCtrl = new DTO_glDocumentoControl();
                    this._listConsumoRec = new List<DTO_ConveniosResumen>();
                    this.Invoke(this.saveDelegate);
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecibidoSolicitudDespacho.cs.cs", "RecibidoSolicitudDespacho.cs-SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
