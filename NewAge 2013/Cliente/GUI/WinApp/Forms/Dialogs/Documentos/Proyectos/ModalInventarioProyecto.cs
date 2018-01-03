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
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Configuration;
using SentenceTransformer;
using System.Collections;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalInventarioProyecto : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private string _unboundPrefix = "Unbound_";
        //Variables de data
        private DTO_faFacturacionFooter _rowCurrent = new DTO_faFacturacionFooter();
        private string _bodegaSalida = string.Empty;
        private string _proyectoID = string.Empty;
        List<DTO_glMovimientoDeta> _detSaldosProyecto = new List<DTO_glMovimientoDeta>();
        List<DTO_faFacturacionFooter> _detalleFact = new List<DTO_faFacturacionFooter>();
        List<DTO_faFacturacionFooter> _detalleFactExist = new List<DTO_faFacturacionFooter>();
        private string _empaqueInvIdDef = string.Empty;
        private decimal _tasaCambio = 0;

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<DTO_faFacturacionFooter> DetalleSelected
        {
            get { return _detalleFact.FindAll(x=>x.SelectInd.Value.Value); }
        }

        #endregion

        /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="bodegaIni">Bodega para consultar las existencias</param>
        public ModalInventarioProyecto(List<DTO_faFacturacionFooter> detalleExist, string proyecto, decimal tasaCambio)
        {
            this.InitializeComponent();
            try
            {
                this._tasaCambio = tasaCambio;
                this._proyectoID = proyecto;
                this._detalleFactExist = detalleExist;
                this.SetInitParameters();
                this.AddGridCols();                         
                FormProvider.LoadResources(this, this._documentID);
                this.LoadMovimientosProyecto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalInventarioProyecto.cs", "ModalInventarioProyecto: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalMovimientoProyectoInv;
            #region Inicializa Controles
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, false, false);
            this.masterProyecto.EnableControl(false);
            this.masterProyecto.Value = this._proyectoID;
            #endregion            
            this._empaqueInvIdDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_EmpaquexDef);

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Campo de marca
            GridColumn sel = new GridColumn();
            sel.FieldName = this._unboundPrefix + "SelectInd";
            sel.Caption = "√";
            sel.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Seleccionar");
            sel.AppearanceHeader.ForeColor = Color.Lime;
            sel.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            sel.AppearanceHeader.Options.UseTextOptions = true;
            sel.AppearanceHeader.Options.UseFont = true;
            sel.AppearanceHeader.Options.UseForeColor = true;
            sel.UnboundType = UnboundColumnType.Boolean;
            sel.VisibleIndex = 0;
            sel.Width = 20;
            sel.Fixed = FixedStyle.Left;
            sel.Visible = true;
            sel.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(sel);

            //BodegaID
            GridColumn BodegaID = new GridColumn();
            BodegaID.FieldName = this._unboundPrefix + "BodegaID";
            BodegaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_BodegaID");
            BodegaID.UnboundType = UnboundColumnType.String;
            BodegaID.VisibleIndex = 1;
            BodegaID.Width = 55;
            BodegaID.Visible = true;
            BodegaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(BodegaID);

            //TareaID
            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this._unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 40;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(TareaID);

            //DescriptivoTarea
            GridColumn DescriptivoTarea = new GridColumn();
            DescriptivoTarea.FieldName = this._unboundPrefix + "DescriptivoTarea";
            DescriptivoTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescriptivoTarea");
            DescriptivoTarea.UnboundType = UnboundColumnType.String;
            DescriptivoTarea.VisibleIndex = 2;
            DescriptivoTarea.Width = 100;
            DescriptivoTarea.Visible = true;
            DescriptivoTarea.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DescriptivoTarea);


            //CodigoReferencia
            GridColumn codRef = new GridColumn();
            codRef.FieldName = this._unboundPrefix + "inReferenciaID";
            codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_inReferenciaID");
            codRef.UnboundType = UnboundColumnType.String;
            codRef.VisibleIndex = 3;
            codRef.Width = 50;
            codRef.Visible = true;
            codRef.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(codRef);

            //DescripTExt
            GridColumn DescripTExt = new GridColumn();
            DescripTExt.FieldName = this._unboundPrefix + "DescripTExt";
            DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Descriptivo");
            DescripTExt.UnboundType = UnboundColumnType.String;
            DescripTExt.VisibleIndex = 4;
            DescripTExt.Width = 170;
            DescripTExt.Visible = true;
            DescripTExt.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DescripTExt);

            //UnidadRef
            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this._unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 5;
            UnidadInvID.Width = 20;
            UnidadInvID.Visible = false;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(UnidadInvID);

            //RefProveedor
            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 6;
            RefProveedor.Width = 35;
            RefProveedor.Visible = true;
            RefProveedor.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(RefProveedor);

            //MarcaInvID
            GridColumn MarcaInvID = new GridColumn();
            MarcaInvID.FieldName = this._unboundPrefix + "MarcaInvID";
            MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_MarcaInvID");
            MarcaInvID.UnboundType = UnboundColumnType.String;
            MarcaInvID.VisibleIndex = 7;
            MarcaInvID.Width = 35;
            MarcaInvID.Visible = true;
            MarcaInvID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(MarcaInvID);

            //CantidadDisp
            this.editValue.Mask.EditMask = "n2";
            GridColumn cantidadDisp = new GridColumn();
            cantidadDisp.FieldName = this._unboundPrefix + "CantidadDispon";
            cantidadDisp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadDispon");
            cantidadDisp.UnboundType = UnboundColumnType.Decimal;
            cantidadDisp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantidadDisp.AppearanceCell.Options.UseTextOptions = true;
            cantidadDisp.VisibleIndex = 8;
            cantidadDisp.Width = 45;
            cantidadDisp.Visible = true;
            cantidadDisp.ColumnEdit = this.editValue;
            cantidadDisp.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(cantidadDisp);

            //CantidadRecurso en Proyecto
            GridColumn CantidadRecurso = new GridColumn();
            CantidadRecurso.FieldName = this._unboundPrefix + "CantidadRecurso";
            CantidadRecurso.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadRecurso");
            CantidadRecurso.UnboundType = UnboundColumnType.Decimal;
            CantidadRecurso.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadRecurso.AppearanceCell.Options.UseTextOptions = true;
            CantidadRecurso.VisibleIndex = 9;
            CantidadRecurso.Width = 45;
            CantidadRecurso.Visible = true;
            CantidadRecurso.OptionsColumn.AllowEdit = false;
            CantidadRecurso.ColumnEdit = this.editValue;
            this.gvDetalle.Columns.Add(CantidadRecurso);

            //CantidadPendiente en Solicitud
            GridColumn CantidadUNI = new GridColumn();
            CantidadUNI.FieldName = this._unboundPrefix + "CantidadUNI";
            CantidadUNI.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadUNI");
            CantidadUNI.UnboundType = UnboundColumnType.Decimal;
            CantidadUNI.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadUNI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadUNI.AppearanceCell.Options.UseFont = true;
            CantidadUNI.AppearanceCell.Options.UseTextOptions = true;
            CantidadUNI.AppearanceCell.BackColor = Color.Beige;
            CantidadUNI.AppearanceCell.Options.UseBackColor = true;
            CantidadUNI.VisibleIndex = 10;
            CantidadUNI.Width = 45;
            CantidadUNI.Visible = true;
            CantidadUNI.OptionsColumn.AllowEdit = true;
            CantidadUNI.ColumnEdit = this.editValue;
            this.gvDetalle.Columns.Add(CantidadUNI);

            this.gvDetalle.OptionsView.ColumnAutoWidth = true;        
        }

        /// <summary>
        /// Carga las solicitudes de proveedores
        /// </summary>
        private void LoadMovimientosProyecto()
        {
            DateTime periodoInv =Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));
            this._detSaldosProyecto = this._bc.AdministrationModel.glMovimientoDetaPRE_GetSaldosInvByProyecto(periodoInv,this._proyectoID,true);
            this._detSaldosProyecto = this._detSaldosProyecto.FindAll(t=>t.CantidadDispon.Value > 0).OrderBy(x => x.Descriptivo.Value).ToList();

            foreach (DTO_glMovimientoDeta mov in _detSaldosProyecto)
            {
                DTO_faFacturacionFooter footerDet = new DTO_faFacturacionFooter();

                #region Asigna datos a la fila
                footerDet.Index = 0;
                footerDet.Movimiento = mov;
                footerDet.Movimiento.NroItem.Value = 0;               
                footerDet.Movimiento.Valor1LOC.Value = 0;
                footerDet.Movimiento.Valor2LOC.Value = 0;
                footerDet.Movimiento.Valor1EXT.Value = 0;
                footerDet.Movimiento.Valor2EXT.Value = 0;
                footerDet.Movimiento.ValorUNI.Value = 0;
                footerDet.ValorBruto = 0;
                footerDet.ValorIVA = 0;
                footerDet.ValorTotal = 0;
                footerDet.ValorNeto = 0;
                footerDet.ValorOtros = 0;
                footerDet.ValorRetenciones = 0;
                footerDet.ValorRFT = 0;
                footerDet.ValorRICA = 0;
                footerDet.ValorRIVA = 0;
                footerDet.Movimiento.ImprimeInd.Value = false;
                footerDet.SelectInd.Value = false;
                #endregion

                this._detalleFact.Add(footerDet); 
            }

            //Asigna las que ya fueron seleccionadas
            if ( this._detalleFactExist != null)
            {
                this._detalleFactExist.ForEach(sol =>
                {
                    if (this._detalleFact.Exists(x => x.Movimiento.BodegaID.Value == sol.Movimiento.BodegaID.Value &&
                                                        x.Movimiento.inReferenciaID.Value == sol.Movimiento.inReferenciaID.Value && x.Movimiento.TareaID.Value == sol.Movimiento.TareaID.Value))
                    {
                        DTO_faFacturacionFooter det = this._detalleFact.Find(x => x.Movimiento.BodegaID.Value == sol.Movimiento.BodegaID.Value &&
                                                            x.Movimiento.inReferenciaID.Value == sol.Movimiento.inReferenciaID.Value && x.Movimiento.TareaID.Value == sol.Movimiento.TareaID.Value);
                        det.SelectInd.Value = true;
                        det.Movimiento.CantidadUNI.Value = sol.Movimiento.CantidadUNI.Value;
                        det.Movimiento.CantidadEMP.Value = sol.Movimiento.CantidadEMP.Value;
                    }
                });  
            }       
            this.gcDetalle.DataSource = this._detalleFact;
            this.gcDetalle.RefreshDataSource();            
        }

        /// <summary>
        /// Valida la data digitada
        /// </summary>
        private void ValidData(DTO_faFacturacionFooter row, GridColumn col)
        {
            decimal? cantTot = this._detalleFact.FindAll(x => x.Movimiento.BodegaID.Value == row.Movimiento.BodegaID.Value && x.Movimiento.inReferenciaID.Value == row.Movimiento.inReferenciaID.Value).Sum(x => x.Movimiento.CantidadUNI.Value);

            if (row.SelectInd.Value.Value && (cantTot > row.Movimiento.CantidadDispon.Value || cantTot < 0))
                this.gvDetalle.SetColumnError(col, "Cantidad no disponible en inventarios para el proyecto ");
            else if (row.SelectInd.Value.Value && cantTot == 0)
                this.gvDetalle.SetColumnError(col, "Debe digitar una cantidad diferente de 0 si selecciona este ítem");
            else
            {
                this.gvDetalle.SetColumnError(col, string.Empty);
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.gvDetalle.HasColumnErrors)
                    this.Close();
                else
                    MessageBox.Show("Revise las cantidades que no son validas e intente nuevamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalInventarioProyecto.cs", "btnReturn_Click"));
            }           
        }

        /// <summary>
        /// Al hacer click para cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._detalleFact = new List<DTO_faFacturacionFooter>();
            this.Close();
        }

        /// <summary>
        /// Se realiza al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridColumn col = this.gvDetalle.Columns[this._unboundPrefix + "CantidadUNI"];
                foreach (var d in this._detalleFact)
                {
                    d.SelectInd.Value = this.chkSelect.Checked;    
                    this.ValidData(d, col);
                }
                this.gvDetalle.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalInventarioProyecto.cs", "chkSelect_CheckedChanged"));
            }
        }   
        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencias_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {             
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                }
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
                    else
                    {
                        DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                        pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                e.Value = pi.GetValue(dtoM.Movimiento, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                        }
                        else
                        {
                            fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                    e.Value = fi.GetValue(dtoM.Movimiento);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                            }
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
                        else
                        {
                            DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencias_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                    this._rowCurrent = (DTO_faFacturacionFooter)this.gvDetalle.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalInventarioProyecto.cs", "gvReferencias_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvReferencia_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    this._rowCurrent = (DTO_faFacturacionFooter)this.gvDetalle.GetRow(e.RowHandle);               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalInventarioProyecto.cs", "gvReferencia_RowClick: " + ex.Message));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencia_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            this._rowCurrent = (DTO_faFacturacionFooter)this.gvDetalle.GetRow(e.RowHandle);
            if (this._rowCurrent != null)
            {
                GridColumn col = new GridColumn();
                col = this.gvDetalle.Columns[this._unboundPrefix + "CantidadUNI"];
                if (fieldName == "SelectInd")
                {
                    if (Convert.ToBoolean(e.Value))
                    {
                        this.ValidData(this._rowCurrent, col);
                    }
                    else
                        this.gvDetalle.SetRowCellValue(e.RowHandle, col, 0);

                    this.gvDetalle.FocusedColumn = col;
                }
                if (fieldName == "CantidadUNI")
                {                  
                    this.ValidData(this._rowCurrent, col);                  
                }
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencia_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.gvDetalle.HasColumnErrors)
                e.Allow = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvReferencia_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string fieldName = this.gvDetalle.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
            //if (fieldName == "CantidadUNI")
            //{
            //    if (!this._rowCurrent.SelectInd.Value.Value)
            //        e.Cancel = true;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvReferencia_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                //if (e.Column.FieldName == this._unboundPrefix + "OrigenMonetario")
                //{
                //    if (Convert.ToByte(e.Value) == 1)
                //        e.DisplayText = "Loc";
                //    else if (Convert.ToByte(e.Value) == 2)
                //        e.DisplayText = "Ext";
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTraslado.cs", "gvReferencia_CustomColumnDisplayText"));
            }
        }

        #endregion          
    }
}
