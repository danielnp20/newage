using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Reportes;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaMovimiento : FormWithToolbar
    {

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_glConsulta _consulta;
        private Dictionary<short, string> _acciones = new Dictionary<short, string>();
        private FormTypes frmType = FormTypes.Query;
        private int documentID=AppQueries.QueryMovimiento;
        private ModulesPrefix frmModule=ModulesPrefix.@in;
        private string frmName;
        private List<DTO_glMovimientoDeta> data = new List<DTO_glMovimientoDeta>();
        private DataTable dataexcel = new DataTable();
        private string UnboundPrefix = "Unbound_";

        #endregion

        public ConsultaMovimiento()
        {
            InitializeComponent();
            frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this.frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            this.AddGridCols();
            this.InitControls();
            try
            {
                this._bc.Pagging_Init(this.pgGrid, 50);
                //this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.pgGrid.PageNumber = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "ConsultaMovimiento: " + ex.Message));
            }
        }

        #region Funciones Publicas

        /// <summary>
        /// Carga los mvtos con filtros
        /// </summary>
        private void LoadMvtos()
        {
            try
            { 
                DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                filter.BodegaID.Value = this.masterBodega.Value;
                filter.inReferenciaID.Value = this.masterReferencia.Value;
                filter.ProyectoID.Value = this.masterProyecto.Value;
                filter.CentroCostoID.Value = this.masterCentroCto.Value;
                filter.MarcaInvID.Value = this.masterMarca.Value;
                filter.RefProveedor.Value = this.txtRefProveedor.Text;
                filter.MvtoTipoInvID.Value = this.masterTipoMvtoInv.Value;
                
                if(this.rdEntradaSalida.SelectedIndex != 0)
                    filter.EntradaSalida.Value = this.rdEntradaSalida.SelectedIndex == 1 ? (byte)1 : (byte)2;   
                filter.DocumentoID.Value = this.masterDocumento.ValidID ? Convert.ToInt32(this.masterDocumento.Value) : filter.DocumentoID.Value;
                filter.PrefijoID.Value = this.masterPrefijo.Value;
                filter.DocumentoNro.Value = !string.IsNullOrEmpty(this.txtDocNro.Text) ? Convert.ToInt32(this.txtDocNro.EditValue) : filter.DocumentoNro.Value;
                filter.FechaIni.Value = new DateTime(this.dtPeriodoInicio.DateTime.Year, this.dtPeriodoInicio.DateTime.Month,1);
                filter.FechaFin.Value = new DateTime( this.dtPeriodoFinal.DateTime.Year, this.dtPeriodoFinal.DateTime.Month,DateTime.DaysInMonth(this.dtPeriodoFinal.DateTime.Year, this.dtPeriodoFinal.DateTime.Month));
                this.data = _bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter,false,true);
                this.data = this.data.FindAll(x => x.DocumentoID.Value >= 51 && x.DocumentoID.Value <= 59).ToList();
                this.dataexcel = _bc.AdministrationModel.glMovimientoDeta_GetByParameterExcel(filter, false,true);
                this.data = this.data.OrderByDescending(x => x.NumeroDoc.Value).ToList();

                //this.pgGrid.UpdatePageNumber(this.data.Count, true, true, false);
                //List<DTO_glMovimientoDeta> pag = this.data.Skip((this.pgGrid.PageNumber - 1) * this.pgGrid.PageSize).Take(this.pgGrid.PageSize).ToList<DTO_glMovimientoDeta>();
                //this.gvMovimiento.MoveFirst();
                this.gcMovimiento.DataSource = null;
                this.gcMovimiento.DataSource = this.data;

                this.pgGrid.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "SetConsulta: " + ex.Message));
            }
        }

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterTipoMvtoInv, AppMasters.inMovimientoTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, true, false);
                this._bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false);
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
                this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, false);
                this._bc.InitMasterUC(this.masterMarca, AppMasters.inMarca, true, true, true, false);

                DateTime periodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                this.dtPeriodoInicio.DateTime = new DateTime(periodo.Year, 1, 1);
                this.dtPeriodoFinal.DateTime = periodo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Documento
            GridColumn DocumentoID = new GridColumn();
            DocumentoID.FieldName = this.UnboundPrefix + "DocumentoID";
            DocumentoID.Caption = "Documento";
            DocumentoID.UnboundType = UnboundColumnType.String;
            DocumentoID.VisibleIndex = 0;
            DocumentoID.Width = 40;
            DocumentoID.Visible = true;
            DocumentoID.OptionsColumn.AllowEdit = false;
            this.gvMovimiento.Columns.Add(DocumentoID);

            GridColumn referencia = new GridColumn();
            referencia.FieldName = this.UnboundPrefix + "Prefijo_Documento";
            referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Prefijo_Documento");
            referencia.UnboundType = UnboundColumnType.String;
            referencia.VisibleIndex = 0;
            referencia.Width = 62;
            referencia.Visible = true;
            referencia.ColumnEdit = this.editLink;
            this.gvMovimiento.Columns.Add(referencia);

            GridColumn BodegaID = new GridColumn();
            BodegaID.FieldName = this.UnboundPrefix + "BodegaID";
            BodegaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BodegaID");
            BodegaID.UnboundType = UnboundColumnType.String;
            BodegaID.VisibleIndex = 1;
            BodegaID.Width = 65;
            BodegaID.Visible = true;
            this.gvMovimiento.Columns.Add(BodegaID);

            GridColumn Fecha = new GridColumn();
            Fecha.FieldName = this.UnboundPrefix + "Fecha";
            Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
            Fecha.UnboundType = UnboundColumnType.DateTime;
            Fecha.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            Fecha.AppearanceCell.Options.UseTextOptions = true;
            Fecha.VisibleIndex = 2;
            Fecha.Width = 65;
            Fecha.Visible = true;
            this.gvMovimiento.Columns.Add(Fecha);

            GridColumn TipoMvtoInv = new GridColumn();
            TipoMvtoInv.FieldName = this.UnboundPrefix + "MvtoTipoInvID";
            TipoMvtoInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MvtoTipoInvID");
            TipoMvtoInv.UnboundType = UnboundColumnType.String;
            TipoMvtoInv.VisibleIndex = 3;
            TipoMvtoInv.Width = 80;
            TipoMvtoInv.Visible = true;
            this.gvMovimiento.Columns.Add(TipoMvtoInv);

            GridColumn EntradaSalidaLetras = new GridColumn();
            EntradaSalidaLetras.FieldName = this.UnboundPrefix + "EntradaSalidaLetras";
            EntradaSalidaLetras.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EntradaSalidaLetras");
            EntradaSalidaLetras.UnboundType = UnboundColumnType.String;
            EntradaSalidaLetras.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            EntradaSalidaLetras.AppearanceCell.Options.UseTextOptions = true;
            EntradaSalidaLetras.VisibleIndex = 4;
            EntradaSalidaLetras.Width = 28;
            EntradaSalidaLetras.Visible = true;
            EntradaSalidaLetras.ToolTip = "E: Entrada de Inventarios  S: Salida de Inventarios";
            this.gvMovimiento.Columns.Add(EntradaSalidaLetras);

            GridColumn inReferenciaID = new GridColumn();
            inReferenciaID.FieldName = this.UnboundPrefix + "inReferenciaID";
            inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            inReferenciaID.UnboundType = UnboundColumnType.String;
            inReferenciaID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            inReferenciaID.AppearanceCell.Options.UseTextOptions = true;
            inReferenciaID.VisibleIndex = 5;
            inReferenciaID.Width = 80;
            inReferenciaID.Visible = true;
            this.gvMovimiento.Columns.Add(inReferenciaID);

            GridColumn DescripTExt = new GridColumn();
            DescripTExt.FieldName = this.UnboundPrefix + "DescripTExt";
            DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
            DescripTExt.UnboundType = UnboundColumnType.String;
            DescripTExt.VisibleIndex = 6;
            DescripTExt.Width = 300;
            DescripTExt.Visible = true;
            this.gvMovimiento.Columns.Add(DescripTExt);

            GridColumn CantidadUNI = new GridColumn();
            CantidadUNI.FieldName = this.UnboundPrefix + "CantidadUNI";
            CantidadUNI.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadUNI");
            CantidadUNI.UnboundType = UnboundColumnType.Integer;
            CantidadUNI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            CantidadUNI.AppearanceCell.Options.UseTextOptions = true;
            CantidadUNI.VisibleIndex = 7;
            CantidadUNI.Width = 55;
            CantidadUNI.Visible = true;
            CantidadUNI.ColumnEdit = this.editCant;
            CantidadUNI.Summary.Add(DevExpress.Data.SummaryItemType.Sum, CantidadUNI.FieldName, "{0:c0}");
            this.gvMovimiento.Columns.Add(CantidadUNI);

            GridColumn ValorUNI = new GridColumn();
            ValorUNI.FieldName = this.UnboundPrefix + "ValorUNI";
            ValorUNI.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUNI");
            ValorUNI.UnboundType = UnboundColumnType.Decimal;
            ValorUNI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorUNI.AppearanceCell.Options.UseTextOptions = true;
            ValorUNI.VisibleIndex = 8;
            ValorUNI.Width = 90;
            ValorUNI.Visible = true;
            ValorUNI.ColumnEdit = this.editValue;
            this.gvMovimiento.Columns.Add(ValorUNI);

            GridColumn Valor1LOC = new GridColumn();
            Valor1LOC.FieldName = this.UnboundPrefix + "Valor1LOC";
            Valor1LOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1LOC");
            Valor1LOC.UnboundType = UnboundColumnType.Decimal;
            Valor1LOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Valor1LOC.AppearanceCell.Options.UseTextOptions = true;
            Valor1LOC.VisibleIndex = 9;
            Valor1LOC.Width = 90;
            Valor1LOC.Visible = true;
            Valor1LOC.ColumnEdit = this.editValue;
            Valor1LOC.Summary.Add(DevExpress.Data.SummaryItemType.Sum, Valor1LOC.FieldName, "{0:c0}");
            this.gvMovimiento.Columns.Add(Valor1LOC);

            GridColumn Valor1EXT = new GridColumn();
            Valor1EXT.FieldName = this.UnboundPrefix + "Valor1EXT";
            Valor1EXT.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1EXT");
            Valor1EXT.UnboundType = UnboundColumnType.Decimal;
            Valor1EXT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Valor1EXT.AppearanceCell.Options.UseTextOptions = true;
            Valor1EXT.VisibleIndex = 10;
            Valor1EXT.Width = 90;
            Valor1EXT.Visible = true;
            Valor1EXT.Summary.Add(DevExpress.Data.SummaryItemType.Sum, Valor1EXT.FieldName, "{0:c0}");
            Valor1EXT.ColumnEdit = this.editValue;
            this.gvMovimiento.Columns.Add(Valor1EXT);
            
            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this.UnboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 11;
            RefProveedor.Width = 90;
            RefProveedor.Visible = true;
            this.gvMovimiento.Columns.Add(RefProveedor);

            GridColumn MarcaDesc = new GridColumn();
            MarcaDesc.FieldName = this.UnboundPrefix + "MarcaDesc";
            MarcaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaDesc");
            MarcaDesc.UnboundType = UnboundColumnType.String;
            MarcaDesc.VisibleIndex = 12;
            MarcaDesc.Width = 90;
            MarcaDesc.Visible = true;
            this.gvMovimiento.Columns.Add(MarcaDesc);

            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this.UnboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 13;
            ProyectoID.Width = 80;
            ProyectoID.Visible = true;
            this.gvMovimiento.Columns.Add(ProyectoID);

            GridColumn CentroCostoID = new GridColumn();
            CentroCostoID.FieldName = this.UnboundPrefix + "CentroCostoID";
            CentroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            CentroCostoID.UnboundType = UnboundColumnType.String;
            CentroCostoID.VisibleIndex = 14;
            CentroCostoID.Width = 80;
            CentroCostoID.Visible = true;
            this.gvMovimiento.Columns.Add(CentroCostoID);

            GridColumn SerialID = new GridColumn();
            SerialID.FieldName = this.UnboundPrefix + "SerialID";
            SerialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
            SerialID.UnboundType = UnboundColumnType.String;
            SerialID.VisibleIndex = 15;
            SerialID.Width = 80;
            SerialID.Visible = true;
            this.gvMovimiento.Columns.Add(SerialID);          
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
                FormProvider.Master.Form_Enter(this, this.documentID, this.frmType, this.frmModule);
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemUpdate.Visible = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "Form_Enter: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "Form_Leave: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "Form_Closing: " + ex.Message));
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
                FormProvider.Master.Form_FormClosed(this.frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "Form_FormClosed: " + ex.Message));
            }
        }

        #endregion

        #region Eventos grilla
        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvMovimiento_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {

        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvMvto_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
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
                        e.Value = pi.GetValue(dto, null);
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
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gvMovimiento.FocusedRowHandle >= 0)
                {
                    DTO_glMovimientoDeta row = (DTO_glMovimientoDeta)this.gvMovimiento.GetRow(this.gvMovimiento.FocusedRowHandle);

                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();

                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(row.NumeroDoc.Value.Value);
                    comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMvto.cs", "editLink_Click"));
            }
        }
        #endregion
      
        #region Barra de Herramientas

        /// <summary>
        /// Boton para busquedas
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.gvMovimiento.PostEditor();
                this.LoadMvtos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                this.LoadMvtos();
                if (this.data.Count > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();
                    List<DTO_glMovimientoDeta> tmp = new List<DTO_glMovimientoDeta>();                    
                    //System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_glMovimientoDeta), this.data);
                    //System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_glMovimientoDeta), this.dataexcel);
                    List<string> colsRemove = new List<string>();
                    foreach (DataColumn col in this.dataexcel.Columns)
                    {
                        var columnsVisibles = this.gvMovimiento.VisibleColumns;
                        var colGrid = columnsVisibles.Where(x => x.FieldName == this.UnboundPrefix + col.Caption).ToList();
                        if (colGrid.Count == 0)
                            colsRemove.Add(col.Caption);
                    }
                    foreach (var col in colsRemove)
                        this.dataexcel.Columns.Remove(col);
                   
                    ReportExcelBase frm = new ReportExcelBase(this.dataexcel,this.documentID);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "TBExport"));
            }
        }

	    #endregion

    }
}
