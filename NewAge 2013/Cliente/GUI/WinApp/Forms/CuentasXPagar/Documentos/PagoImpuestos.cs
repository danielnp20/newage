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
using System.Reflection;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PagoImpuestos : DocumentForm
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _dtoCtrl;
        private List<DTO_PagoImpuesto> data = new List<DTO_PagoImpuesto>();
        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
        }

        #endregion

        public PagoImpuestos()
        {
            //  InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_Periodo));
            this.dtPeriod.DateTime = dt;
            this.dtPeriodoFilter.DateTime =  this.dtFecha.DateTime;

            //Ajuste el tamaño de las secciones
            base.tlSeparatorPanel.RowStyles[0].Height = 70;
            base.tlSeparatorPanel.RowStyles[1].Height = 355;
            base.tlSeparatorPanel.RowStyles[2].Height = 50;
        }

        /// <summary>
        /// Calcula valor factura + iva
        /// </summary>
        private void CalcularTotal()
        {


        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
         
            this.dtFecha.DateTime = this.dtPeriod.DateTime;
            this.data = new List<DTO_PagoImpuesto>();
           
        }

        /// <summary>
        /// Valida los campos obligatoriso de la grilla
        /// </summary>
        private string ValidGrid()
        {
            string result = string.Empty;

            if (this.data.FindAll(x => x.Selected.Value.Value).Count == 0)
                return result = "No ha seleccionado ningún item";

            for (int i = 0; i < this.data.Count; i++)
            {
                DTO_PagoImpuesto dto = this.data[i];
                if (string.IsNullOrEmpty(dto.TerceroID.Value))
                {
                    result += "Linea " + i.ToString() + "Tercero No existe \n ";
                }
            }       

            return result;
        }


         #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            try
            {
                this.data = this._bc.AdministrationModel.Comprobante_GetAuxForImpuesto(this.dtPeriodoFilter.DateTime);
                this.gcDocument.DataSource = this.data;
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoImpuestos.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.PagoImpuestos;

            base.SetInitParameters();            
        
            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.cp;
            this.AddGridCols();
            this.InitControls();
            this.LoadData(true);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Grilla Principal

                //Selected
                GridColumn Selected = new GridColumn();
                Selected.FieldName = this.unboundPrefix + "Selected";
                Selected.Caption = "√";
                Selected.UnboundType = UnboundColumnType.Boolean;
                Selected.VisibleIndex = 0;
                Selected.Width = 25;
                Selected.Visible = true;
                Selected.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Selected");
                Selected.AppearanceHeader.ForeColor = Color.Lime;
                Selected.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Selected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                Selected.AppearanceHeader.Options.UseTextOptions = true;
                Selected.AppearanceHeader.Options.UseFont = true;
                Selected.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(Selected);

                //ImpuestoTipoID
                GridColumn ImpuestoTipoID = new GridColumn();
                ImpuestoTipoID.FieldName = this.unboundPrefix + "ImpuestoTipoID";
                ImpuestoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ImpuestoTipoID");
                ImpuestoTipoID.UnboundType = UnboundColumnType.String;
                ImpuestoTipoID.VisibleIndex = 1;
                ImpuestoTipoID.Width = 100;
                ImpuestoTipoID.Visible = true;
                ImpuestoTipoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ImpuestoTipoID);

                //ImpuestoTipoDesc
                GridColumn ImpuestoTipoDesc = new GridColumn();
                ImpuestoTipoDesc.FieldName = this.unboundPrefix + "ImpuestoTipoDesc";
                ImpuestoTipoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ImpuestoTipoDesc");
                ImpuestoTipoDesc.UnboundType = UnboundColumnType.String;
                ImpuestoTipoDesc.VisibleIndex = 2;
                ImpuestoTipoDesc.Width = 150;
                ImpuestoTipoDesc.Visible = true;
                ImpuestoTipoDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ImpuestoTipoDesc);

                //LugarGeoID
                GridColumn LugarGeograficoID = new GridColumn();
                LugarGeograficoID.FieldName = this.unboundPrefix + "LugarGeoID";
                LugarGeograficoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LugarGeoID");
                LugarGeograficoID.UnboundType = UnboundColumnType.String;
                LugarGeograficoID.VisibleIndex = 3;
                LugarGeograficoID.Width = 100;
                LugarGeograficoID.Visible = true;
                LugarGeograficoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(LugarGeograficoID);

                //LugarGeoDesc
                GridColumn LugarGeograficoDesc = new GridColumn();
                LugarGeograficoDesc.FieldName = this.unboundPrefix + "LugarGeoDesc";
                LugarGeograficoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LugarGeoDesc");
                LugarGeograficoDesc.UnboundType = UnboundColumnType.String;
                LugarGeograficoDesc.VisibleIndex = 4;
                LugarGeograficoDesc.Width = 150;
                LugarGeograficoDesc.Visible = true;
                LugarGeograficoDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(LugarGeograficoDesc);

                //NITCierreAnual
                GridColumn NITCierreAnual = new GridColumn();
                NITCierreAnual.FieldName = this.unboundPrefix + "NITCierreAnual";
                NITCierreAnual.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NITCierreAnual");
                NITCierreAnual.UnboundType = UnboundColumnType.String;
                NITCierreAnual.VisibleIndex = 5;
                NITCierreAnual.Width = 100;
                NITCierreAnual.Visible = true;
                NITCierreAnual.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(NITCierreAnual);

                //NITCierreAnualDesc
                GridColumn NITCierreAnualDesc = new GridColumn();
                NITCierreAnualDesc.FieldName = this.unboundPrefix + "NITCierreAnualDesc";
                NITCierreAnualDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NITCierreAnualDesc");
                NITCierreAnualDesc.UnboundType = UnboundColumnType.String;
                NITCierreAnualDesc.VisibleIndex = 6;
                NITCierreAnualDesc.Width = 150;
                NITCierreAnualDesc.Visible = true;
                NITCierreAnualDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(NITCierreAnualDesc);

                //ValorTotal
                GridColumn ValorTotal = new GridColumn();
                ValorTotal.FieldName = this.unboundPrefix + "ValorTotal";
                ValorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotal");
                ValorTotal.UnboundType = UnboundColumnType.Decimal;
                ValorTotal.VisibleIndex = 7;
                ValorTotal.Width = 100;
                ValorTotal.Visible = true;
                ValorTotal.ColumnEdit = this.editSpin;
                ValorTotal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorTotal);

                //ValorTotalMiles
                GridColumn ValorTotalMiles = new GridColumn();
                ValorTotalMiles.FieldName = this.unboundPrefix + "ValorTotalMiles";
                ValorTotalMiles.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalMiles");
                ValorTotalMiles.UnboundType = UnboundColumnType.Decimal;
                ValorTotalMiles.VisibleIndex = 8;
                ValorTotalMiles.Width = 100;
                ValorTotalMiles.Visible = true;
                ValorTotalMiles.ColumnEdit = this.editSpin;
                ValorTotalMiles.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorTotalMiles);

                //ValorTotalDif
                GridColumn ValorTotalDif = new GridColumn();
                ValorTotalDif.FieldName = this.unboundPrefix + "ValorTotalDif";
                ValorTotalDif.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalDif");
                ValorTotalDif.UnboundType = UnboundColumnType.Decimal;
                ValorTotalDif.VisibleIndex = 9;
                ValorTotalDif.Width = 100;
                ValorTotalDif.Visible = true;
                ValorTotalDif.ColumnEdit = this.editSpin;
                ValorTotalDif.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ValorTotalDif);

                #endregion
                #region Grilla Detalle

                //CuentaID
                GridColumn CuentaID = new GridColumn();
                CuentaID.FieldName = this.unboundPrefix + "CuentaID";
                CuentaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
                CuentaID.UnboundType = UnboundColumnType.String;
                CuentaID.VisibleIndex = 0;
                CuentaID.Width = 100;
                CuentaID.Visible = true;
                CuentaID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CuentaID);

                //CuentaDesc
                GridColumn CuentaDesc = new GridColumn();
                CuentaDesc.FieldName = this.unboundPrefix + "CuentaDesc";
                CuentaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaDesc");
                CuentaDesc.UnboundType = UnboundColumnType.String;
                CuentaDesc.VisibleIndex = 1;
                CuentaDesc.Width = 150;
                CuentaDesc.Visible = true;
                CuentaDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CuentaDesc);

                //ValorLocal
                GridColumn ValorLocal = new GridColumn();
                ValorLocal.FieldName = this.unboundPrefix + "ValorLocal";
                ValorLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorLocal");
                ValorLocal.UnboundType = UnboundColumnType.String;
                ValorLocal.VisibleIndex = 2;
                ValorLocal.Width = 100;
                ValorLocal.Visible = true;
                ValorLocal.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorLocal);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoImpuestos.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();        

        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemUpdate.Visible = true;
            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemNew.Visible = false;
            FormProvider.Master.itemSave.Visible = true;
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
        }

        #endregion

        #region Eventos Header

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "SiNo" && e.Value == null)
                {
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
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
                if (fieldName == "SiNo")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
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

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "SiNo")
            {
                e.RepositoryItem = this.editChkBox;
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para recargar los controles del formulario
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadData(true);
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();

            var message = this.ValidGrid();// this.FieldsObligated();
            if (string.IsNullOrEmpty(message))
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            else
                MessageBox.Show(message);
        }

        #endregion          
     
        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoImpuestos.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
